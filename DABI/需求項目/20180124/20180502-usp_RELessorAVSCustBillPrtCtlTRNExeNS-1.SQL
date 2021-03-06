USE [RTLib]
GO
/****** Object:  StoredProcedure [dbo].[usp_RTLessorAVSCustBillingPrtCtlTRNExeNS]    Script Date: 2018/5/2 下午 03:01:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Batch submitted through debugger: SQLQuery15.sql|7|0|C:\Users\PILLAR~1.MCH\AppData\Local\Temp\~vs88AA.sql
ALTER PROCEDURE [dbo].[usp_RTLessorAVSCustBillingPrtCtlTRNExeNS]
	@batch varchar(20),
	@cusid varchar(20), --指定用戶
	@usr varchar(6)		-- 執行人員
AS
--為了處理前一段的有效期限問題所以增加這一段功能 PILLAR 20180502

declare @duedats datetime, @duedate datetime, @maxno int
	, @scmpid varchar(10) --公司編號 取參數 rtcode-->cmpid-->S1-->01
	, @smarket varchar(10) --超商識別碼編號 取參數 rtcode-->S1-->02
	, @syy2 int --民國年
	, @sduedate varchar(6)--到期日期 民國年月日取年的末兩碼

set @scmpid = ''
select @scmpid = CODENC
from rtcode where KIND = 'S1' and CODE = '01'

set @smarket = ''
select @smarket = CODENC
from rtcode where KIND = 'S1' and CODE = '02'

-- insert 明細檔 RTLessorAVSCustBillingPrtSub --------------------------------------------------------------------------------------
IF Exists (SELECT * FROM tempdb..sysobjects WHERE ID =Object_id('tempdb..#tempContSub')) 
    DROP TABLE #tempContSub

SELECT	IDENTITY(int, 1,1) as noticeid, @batch as batch, a.comq1, a.lineq1, a.cusid, a.duedat, a.casekind, a.paycycle
INTO	#tempContSub
FROM 	RTLessorAVSCust a 
		inner join RTLessorAVSCmtyLine b on a.COMQ1 = b.COMQ1 and a.LINEQ1 = b.LINEQ1 
WHERE 	a.DROPDAT is null and a.CANCELDAT is null and a.FINISHDAT is not null 
AND	b.DROPDAT is null and b.CANCELDAT is null 
AND	a.freecode<>'Y' 
AND	a.CUSID=@cusid
ORDER BY a.comq1,a.lineq1,a.cusnc

-- insert 條碼檔 RTLessorAVSCustBillingBarcode -----------------------------------------------------------------------------------------
IF Exists (SELECT * FROM tempdb..sysobjects WHERE ID =Object_id('tempdb..#tempContBarcode')) 
    DROP TABLE #tempContBarcode

SELECT	 IDENTITY(int, 1,1) as CSNOTICEID, a.cusid+'-'+c.paycycle as CSCUSID, b.noticeid, a.casekind, c.paycycle
INTO	#tempContBarcode
FROM	#tempContSub a 
		inner join RTLessorAVSCustBillingPrtSub b on b.noticeid in (select NOTICEID from RTLessorAVSCustBillingPrtSub where BATCH =@batch AND CUSID =@cusid)
		inner join RTBillCharge c on c.casekind = a.casekind and c.casetype ='07' 
ORDER BY a.noticeid, c.paycycle

select  @maxno =  isnull(convert(int, right(max(CSNOTICEID),5)), 0)
from RTLessorAvsCustBillingBarcode where CSNOTICEID like 'A' + convert(char(4), getdate(), 12) + '%'

delete from RTLessorAVSCustBillingBarcode
WHERE NOTICEID IN (select NOTICEID from RTLessorAVSCustBillingPrtSub
where BATCH =@batch AND CUSID =@cusid)

INSERT INTO RTLessorAVSCustBillingBarcode(CSNOTICEID, CSCUSID, NOTICEID, CASEKIND, PAYCYCLE)
SELECT	'A' + convert(char(4), getdate(), 12) + right('0000'+convert(varchar(5), CSNOTICEID+@maxno), 5),
			CSCUSID, NOTICEID, CASEKIND, PAYCYCLE
FROM	#tempContBarcode


UPDATE A
SET A.CSBARCOD1=right(substring(convert(varchar(8), dateadd(m,2, D.DUEDAT), 112),1,4)-1911, 2)+substring(convert(varchar(8), dateadd(m,2, D.DUEDAT), 112),5,4)+@scmpid
, A.CSBARCOD2=@smarket+SUBSTRING(A.CSCUSID,2,9)+RIGHT(A.CSNOTICEID, 3)
, A.CSBARCOD3=right(substring(convert(varchar(8), dateadd(m,2, D.DUEDAT), 112),1,4)-1911, 2)+substring(convert(varchar(8), dateadd(m,2, D.DUEDAT), 112),5,2) + 'XX' +right('000000000'+ convert(varchar(10), b.amt), 9)
FROM RTLessorAVSCustBillingBarcode A
left join RTLessorAVSCust d on a.CSCUSID like '%'+d.CUSID +'%'
left join rtcode c on c.kind = 'L5' and c.PARM1=d.comtype
left join RTBillCharge b on b.CASETYPE=c.CODE AND B.CASEKIND=A.CASEKIND AND B.PAYCYCLE=A.PAYCYCLE
WHERE NOTICEID IN (select NOTICEID from RTLessorAVSCustBillingPrtSub
where BATCH =@batch AND CUSID =@cusid)


IF Exists (SELECT * FROM tempdb..sysobjects WHERE ID =Object_id('tempdb..#temptb')) 
    DROP TABLE #temptb

DECLARE @TabelCount INT			 --loop的條件
       ,@ii INT
	   ,@iODD INT
	   ,@iEVEN INT
	   ,@sODD VARCHAR(1)
	   ,@sEVEN VARCHAR(1)
	   ,@sKEY VARCHAR(30)
	   ,@iMOD INT
	   ,@WhileTableCount INT = 1 
	   --
	   ,@Id      INT
	   ,@sbc VARCHAR(50)

SELECT ROW_NUMBER() OVER(ORDER BY CSNOTICEID) as ID , CSNOTICEID, CSBARCOD1+'0'+CSBARCOD2+CSBARCOD3 as sBC 
INTO	#temptb
FROM RTLessorAVSCustBillingBarcode
WHERE NOTICEID IN (select NOTICEID from RTLessorAVSCustBillingPrtSub
where BATCH =@batch AND CUSID =@cusid)
 
SET @TabelCount = (SELECT COUNT(ID) FROM #temptb)
declare @ssub varchar(1) --用來紀錄取得的字串
declare @isub INT --用來暫存 字串轉換的數字

WHILE @WhileTableCount <= @TabelCount
BEGIN
  SELECT @Id = ID, @sbc = sBC, @sKEY=CSNOTICEID FROM #temptb WHERE ID = @WhileTableCount
  SET @II=1 
  SET @IODD=0
  SET @iEVEN=0
  WHILE @II <= 41 
  BEGIN
    IF @II <> 31 AND @II <> 32 
	BEGIN
		SET @iMOD = @II % 2
		set @ssub = SUBSTRING(@sbc, @II, 1)
		if @ssub >= '0' AND @ssub <= '9'
		begin
		  set @isub = @ssub
		end
		else
		begin
		  if @ssub = 'A' set @isub = 1
		  if @ssub = 'B' set @isub = 2
		  if @ssub = 'C' set @isub = 3
		  if @ssub = 'D' set @isub = 4
		  if @ssub = 'E' set @isub = 5
		  if @ssub = 'F' set @isub = 6
		  if @ssub = 'G' set @isub = 7
		  if @ssub = 'H' set @isub = 8
		  if @ssub = 'I' set @isub = 9
		  if @ssub = 'J' set @isub = 1
		  if @ssub = 'K' set @isub = 2
		  if @ssub = 'L' set @isub = 3
		  if @ssub = 'M' set @isub = 4
		  if @ssub = 'N' set @isub = 5
		  if @ssub = 'O' set @isub = 6
		  if @ssub = 'P' set @isub = 7
		  if @ssub = 'Q' set @isub = 8
		  if @ssub = 'R' set @isub = 9
		  if @ssub = 'S' set @isub = 2
		  if @ssub = 'T' set @isub = 3
		  if @ssub = 'U' set @isub = 4
		  if @ssub = 'V' set @isub = 5
		  if @ssub = 'W' set @isub = 6
		  if @ssub = 'X' set @isub = 7
		  if @ssub = 'Y' set @isub = 8
		  if @ssub = 'Z' set @isub = 9
		  if @ssub = '+' set @isub = 1
		  if @ssub = '%' set @isub = 2
		  if @ssub = '-' set @isub = 6
		  if @ssub = '.' set @isub = 7
		  if @ssub = ' ' set @isub = 8
		  if @ssub = '$' set @isub = 9
		  if @ssub = '/' set @isub = 0
		end

		IF @iMOD  = 1
		BEGIN
		  SET @IODD=@IODD + @isub
		END
		ELSE
		BEGIN
		  SET @iEVEN=@iEVEN + @isub
		END
	END
    SET @II = @II + 1
  END;

  SET @iODD=@iODD % 11
  SET @iEVEN=@iEVEN % 11

  IF @iODD = 0 
  BEGIN
    SET @sODD = 'A'
  END
  ELSE
  IF @iODD = 10 
  BEGIN
    SET @sODD = 'B'
  END
  ELSE
  BEGIN
    SET @sODD = @iODD
  END

  IF @iEVEN = 0 
  BEGIN
    SET @sEVEN = 'X'
  END
  ELSE
  IF @iEVEN = 10 
  BEGIN
    SET @sEVEN = 'Y'
  END
  ELSE
  BEGIN
    SET @sEVEN = @iEVEN
  END
  
  UPDATE RTLessorAVSCustBillingBarcode
  SET CSBARCOD3 = REPLACE(CSBARCOD3, 'XX', @sODD+@sEVEN)
  WHERE CSNOTICEID = @sKEY 

  SET @WhileTableCount = @WhileTableCount + 1
END		
