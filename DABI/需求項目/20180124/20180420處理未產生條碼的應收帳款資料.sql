declare 
    @syy int,	@smm int,	
	@sdd int,			-- 1: 上半月,	16: 下半月
	@usr varchar(6),	@batch varchar(8), @duedats datetime, @duedate datetime, @maxno int
	, @scmpid varchar(10) --公司編號 取參數 rtcode-->cmpid-->S1-->01
	, @smarket varchar(10) --超商識別碼編號 取參數 rtcode-->S1-->02
	, @syy2 int --民國年
	, @sduedate varchar(6)--到期日期 民國年月日取年的末兩碼
set @syy=2018
set @smm=4
set @sdd=1
set @usr='001'
set @batch  = convert(varchar(4), @syy) + right('0'+ convert(varchar(2), @smm), 2) + right('0'+ convert(varchar(2), @sdd), 2)
set @duedats =  dateadd(m,1, convert(datetime, convert(varchar(4), @syy) +'/'+ convert(varchar(2), @smm) +'/'+ convert(varchar(2), @sdd)))
set @syy2 = @syy-1911
set @sduedate = right(substring(convert(varchar(8), @duedats, 112),1,4)-1911, 2)+substring(convert(varchar(8), @duedats, 112),5,4)
select @sduedate

set @scmpid = ''
select @scmpid = CODENC
from rtcode where KIND = 'S1' and CODE = '01'

set @smarket = ''
select @smarket = CODENC
from rtcode where KIND = 'S1' and CODE = '02'

UPDATE A
SET A.CSBARCOD1=@sduedate+@scmpid, A.CSBARCOD2=@smarket+SUBSTRING(A.CSCUSID,2,9)+RIGHT(A.CSNOTICEID, 3)
,  A.CSBARCOD3=SUBSTRING(@sduedate, 1, 4) + 'XX' +right('000000000'+ convert(varchar(10), b.amt), 9)
--SELECT A.CSBARCOD1,@sduedate+@scmpid, A.CSBARCOD2,SUBSTRING(A.CSCUSID, 2, 9)+RIGHT(A.CSCUSID,2)+RIGHT(A.CSNOTICEID, 5)
--,  A.CSBARCOD3,SUBSTRING(@sduedate, 1, 4) + 'XX' +right('000000000'+ convert(varchar(10), b.amt), 9)
--SELECT * 
FROM RTLessorAVSCustBillingBarcode A
left join RTLessorAVSCust d on a.CSCUSID like '%'+d.CUSID +'%'
left join rtcode c on c.kind = 'L5' and c.PARM1=d.comtype
left join RTBillCharge b on b.CASETYPE=c.CODE AND B.CASEKIND=A.CASEKIND AND B.PAYCYCLE=A.PAYCYCLE
WHERE NOTICEID IN (SELECT NOTICEID FROM RTLessorAVSCustBillingBarcode where NOTICEID like '%201804%')
and a.CSBARCOD1=''

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
WHERE NOTICEID like '%201804%'
 
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
