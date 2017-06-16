<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="東森AVS管理系統"
  title="東森AVS欠費用戶查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""
  functionoptopen=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;主線;社區名稱;用戶;連絡電話;AVS合約號;完工日;報竣日;轉檔日;開始計費日;退租日;作廢日;帳單日;金額;拆帳月份"
  sqlDelete="SELECT RTEBTCUST.COMQ1, RTEBTCUST.LINEQ1, RTEBTCUST.CUSID,rtrim(ltrim(convert(char(6),RTEBTCUST.COMQ1))) +'-'+ rtrim(ltrim(convert(char(6),RTEBTCUST.lineQ1))) as comqline,RTEBTCMTYH.COMN,RTEBTCUST.CUSNC,RTEBTCUST.contacttel+','+RTEBTCUST.mobile, RTEBTCUST.AVSNO, " _
         &"RTEBTCUST.FINISHDAT, RTEBTCUST.DOCKETDAT, RTEBTCUST.TRANSDAT, RTEBTCUST.STRBILLINGDAT, RTEBTCUST.DROPDAT, " _
         &"RTEBTCUST.CANCELDAT, AVSMonthlyaccountSRC.BILLDAT, ISNULL(AVSMonthlyaccountSRC.AMT,0), ISNULL(AVSMonthlyaccountSRC.RYM,0) "  _
         &"FROM RTEBTCUST LEFT OUTER JOIN AVSMonthlyaccountSRC ON RTEBTCUST.AVSNO = AVSMonthlyaccountSRC.AVSNO " _
         &"WHERE (RTEBTCUST.DOCKETDAT IS NOT NULL) AND (RTEBTCUST.DROPDAT IS NULL) AND (RTEBTCUST.CANCELDAT IS NULL) " _
         &"ORDER BY  RTEBTCUST.AVSNO, AVSMonthlyaccountSRC.BILLDAT " 

  dataTable="rtebtcust"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="RTebtCustD.asp"
  datawindowFeature=""
  searchWindowFeature="width=300,height=150,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth="600"
  diaHeight="400"
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="RTEBTCUSTARS.ASP"
  '----
  'START INQUERY AR MONTH
   IF searchQry = "" THEN
      SYM=REQUEST("KEY")
      SYMD=CDATE(MID(SYM,1,4) & "/" & MID(SYM,5,2) & "/" & "01")
      SYMDX=MID(SYM,1,4) & "/" & MID(SYM,5,2)
   END IF
   '----
  searchFirst=FALSE
  If searchQry="" Then
     searchQry="  RTEBTCUST.STRBILLINGDAT < '" & SYMD & "' and ( RTEBTCUST.DROPDAT >='" &  Symd & "' or RTEBTCUST.DROPDAT is null) " 
     searchShow="應收帳款月份︰" & SYMDX 
     searchQry2=SYMD
  ELSE
     SEARCHFIRST=FALSE
  End If
  '判斷當月是否有拆帳資料，如果沒有則表當月尚未拆款!
  set connYY=server.CreateObject("ADODB.connection")
  set rsYY=server.CreateObject("ADODB.recordset")
  dsnYY="DSN=RTLIB"
  sqlYY="select count(*) as cnt from AVSMonthlyaccountSRC WHERE BILLDAT = '" & SYMD & "'"
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if rsyy("cnt") > 0 then
  sqlList="SELECT RTEBTCUST.COMQ1, RTEBTCUST.LINEQ1, RTEBTCUST.CUSID,rtrim(ltrim(convert(char(6),RTEBTCUST.COMQ1))) +'-'+ rtrim(ltrim(convert(char(6),RTEBTCUST.lineQ1))) as comqline,RTEBTCMTYH.COMN,RTEBTCUST.CUSNC,RTEBTCUST.contacttel+case when RTEBTCUST.contacttel <> '' and RTEBTCUST.mobile <> '' then'、' else '' end +RTEBTCUST.mobile, RTEBTCUST.AVSNO, " _
         &"RTEBTCUST.FINISHDAT, RTEBTCUST.DOCKETDAT, RTEBTCUST.TRANSDAT, RTEBTCUST.STRBILLINGDAT, RTEBTCUST.DROPDAT, " _
         &"RTEBTCUST.CANCELDAT, AVSMonthlyaccountSRC.BILLDAT, ISNULL(AVSMonthlyaccountSRC.AMT,0), ISNULL(AVSMonthlyaccountSRC.RYM,0) " _
         &"FROM  RTEBTCUST LEFT OUTER JOIN AVSMonthlyaccountSRC ON RTEBTCUST.AVSNO = AVSMonthlyaccountSRC.AVSNO AND " _
         &"(AVSMonthlyaccountSRC.BILLDAT = '" & SEARCHQRY2 & "' OR AVSMonthlyaccountSRC.RYM IS NULL) LEFT OUTER JOIN " _
         &"RTEBTCMTYH ON RTEBTCUST.COMQ1 = RTEBTCMTYH.COMQ1 " _
         &"WHERE (RTEBTCUST.DOCKETDAT IS NOT NULL)  AND " _ 
         &"(RTEBTCUST.STRBILLINGDAT IS NOT NULL) AND " & SEARCHQRY & " AND (AVSMonthlyaccountSRC.RYM IS NULL) order by RTEBTCUST.comq1,RTEBTCUST.lineq1" 
   else
   '如果當月沒拆帳資料，則不秀任何資料(設comq1=0)。
     sqllist="SELECT RTEBTCUST.COMQ1, RTEBTCUST.LINEQ1, RTEBTCUST.CUSID,rtrim(ltrim(convert(char(6),RTEBTCUST.COMQ1))) +'-'+ rtrim(ltrim(convert(char(6),RTEBTCUST.lineQ1))) as comqline,RTEBTCMTYH.COMN,RTEBTCUST.CUSNC,RTEBTCUST.contacttel+case when RTEBTCUST.contacttel <> '' and RTEBTCUST.mobile <> '' then'、' else '' end +RTEBTCUST.mobile, RTEBTCUST.AVSNO, " _
         &"RTEBTCUST.FINISHDAT, RTEBTCUST.DOCKETDAT, RTEBTCUST.TRANSDAT, RTEBTCUST.STRBILLINGDAT, RTEBTCUST.DROPDAT, " _
         &"RTEBTCUST.CANCELDAT, AVSMonthlyaccountSRC.BILLDAT, ISNULL(AVSMonthlyaccountSRC.AMT,0), ISNULL(AVSMonthlyaccountSRC.RYM,0) " _
         &"FROM  RTEBTCUST LEFT OUTER JOIN AVSMonthlyaccountSRC ON RTEBTCUST.AVSNO = AVSMonthlyaccountSRC.AVSNO AND " _
         &"(AVSMonthlyaccountSRC.BILLDAT = '" & SEARCHQRY2 & "' OR AVSMonthlyaccountSRC.RYM IS NULL) LEFT OUTER JOIN " _
         &"RTEBTCMTYH ON RTEBTCUST.COMQ1 = RTEBTCMTYH.COMQ1 " _
         &"WHERE  RTEBTCUST.COMQ1=0 "
   end if  
   rsyy.close
   connyy.close
   set rsyy=nothing
   set connyy=nothing
 'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>