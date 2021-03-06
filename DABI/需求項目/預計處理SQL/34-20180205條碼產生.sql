CREATE PROCEDURE usp_RTLessorAVSCustBillingPrtCtlTRNExeN 
	@syy int,	@smm int,	
	@sdd int,			-- 1: 上半月,	16: 下半月
	@usr varchar(6)		-- 執行人員
AS

declare @batch varchar(8), @duedats datetime, @duedate datetime, @maxno int
	,@scmpid varchar(10) --公司編號 取參數 rtcode-->cmpid
	, @syy2 int --民國年
	, @sduedate varchar(6)--到期日期 民國年月日取年的末兩碼
set @batch  = convert(varchar(4), @syy) + right('0'+ convert(varchar(2), @smm), 2) + right('0'+ convert(varchar(2), @sdd), 2)
set @duedats =  dateadd(m,1, convert(datetime, convert(varchar(4), @syy) +'/'+ convert(varchar(2), @smm) +'/'+ convert(varchar(2), @sdd)))
set @syy2 = @syy-1911
set @sduedate = right(substring(convert(varchar(8), @duedats, 112),1,4)-1911, 2)+substring(convert(varchar(8), @duedats, 112),5,4)
select @sduedate

set @scmpid = ''
select @scmpid = CODENC
from rtcode where KIND = 'S1' and CODE = '01'

-- insert 頭檔 RTLessorAVSCustBillingPrt --------------------------------------------------------------------------------------------
IF @sdd =1 BEGIN
	set @duedate = dateadd(d, 14, @duedats)
	
	insert into RTLessorAVSCustBillingPrt(batch, duedatsa, duedatea,cdat, cusr)
	values (@batch, @duedats, @duedate, getdate(), @usr)
END
ELSE IF @sdd =16 BEGIN
	set @duedate =  dateadd(d, -1, dateadd(m,2, convert(datetime, convert(varchar(4), @syy) +'/'+ convert(varchar(2), @smm) +'/1')))

	insert into RTLessorAVSCustBillingPrt(batch, duedatsa, duedatea,cdat, cusr)
	values (@batch, @duedats, @duedate, getdate(), @usr)
END

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
AND	a.duedat between @duedats and @duedate
ORDER BY a.comq1,a.lineq1,a.cusnc

select  @maxno = isnull(convert(int, right(max(noticeid),4)), 0)
from RTLessorAvsCustBillingPrtSub where noticeid like 'A' + convert(char(8), getdate(), 112)+'%'

INSERT INTO RTLessorAVSCustBillingPrtSub (noticeid, batch, comq1, lineq1, cusid, duedat, casekind, paycycle)
SELECT	'A' + convert(char(8), getdate(), 112) + right('000'+convert(varchar(4), noticeid+@maxno), 4),
	batch,comq1, lineq1,cusid, duedat, casekind, paycycle
FROM	#tempContSub

SELECT * FROM #tempContSub

-- insert 條碼檔 RTLessorAVSCustBillingBarcode -----------------------------------------------------------------------------------------
IF Exists (SELECT * FROM tempdb..sysobjects WHERE ID =Object_id('tempdb..#tempContBarcode')) 
    DROP TABLE #tempContBarcode

SELECT	 IDENTITY(int, 1,1) as CSNOTICEID, a.cusid+'-'+c.paycycle as CSCUSID, b.noticeid, a.casekind, c.paycycle
INTO	#tempContBarcode
FROM	#tempContSub a 
		inner join RTLessorAVSCustBillingPrtSub b on b.noticeid = 'A' + convert(char(8), getdate(), 112) + right('000'+convert(varchar(4), a.noticeid+@maxno), 4)
		inner join RTBillCharge c on c.casekind = a.casekind and c.casetype ='07' 
ORDER BY a.noticeid, c.paycycle

select  @maxno =  isnull(convert(int, right(max(CSNOTICEID),5)), 0)
from RTLessorAvsCustBillingBarcode where CSNOTICEID like 'A' + convert(char(4), getdate(), 12) + '%'

INSERT INTO RTLessorAVSCustBillingBarcode(CSNOTICEID, CSCUSID, NOTICEID, CASEKIND, PAYCYCLE)
SELECT	'A' + convert(char(4), getdate(), 12) + right('0000'+convert(varchar(5), CSNOTICEID+@maxno), 5),
			CSCUSID, NOTICEID, CASEKIND, PAYCYCLE
FROM	#tempContBarcode

SELECT * FROM #tempContSub
SELECT * FROM #tempContBarcode

UPDATE A
SET A.CSBARCOD1=@sduedate+@scmpid, A.CSBARCOD2=SUBSTRING(A.CSCUSID, 2, 9)+RIGHT(A.CSCUSID,2)+RIGHT(A.CSNOTICEID, 5)
,  A.CSBARCOD3=SUBSTRING(@sduedate, 1, 4) + 'XX' +right('000000000'+ convert(varchar(10), b.amt), 9)
--SELECT A.CSBARCOD1,@sduedate+@scmpid, A.CSBARCOD2,SUBSTRING(A.CSCUSID, 2, 9)+RIGHT(A.CSCUSID,2)+RIGHT(A.CSNOTICEID, 5)
--,  A.CSBARCOD3,SUBSTRING(@sduedate, 1, 4) + 'XX' +right('000000000'+ convert(varchar(10), b.amt), 9)
--SELECT * 
FROM RTLessorAVSCustBillingBarcode A
left join RTBillCharge b on b.CASETYPE='07' AND B.CASEKIND=A.CASEKIND AND B.PAYCYCLE=A.PAYCYCLE
WHERE NOTICEID IN (SELECT NOTICEID FROM #tempContBarcode)


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
WHERE NOTICEID IN (SELECT NOTICEID FROM #tempContBarcode)
 
SET @TabelCount = (SELECT COUNT(ID) FROM #temptb)

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
		IF @iMOD  = 1
		BEGIN
		  SET @IODD=@IODD + SUBSTRING(@sbc, @II, 1)
		END
		ELSE
		BEGIN
		  SET @iEVEN=@iEVEN + SUBSTRING(@sbc, @II, 1)
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
