<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="Sparq* 管理系統"
  title="速博ADSL主線設備資料查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="異動查詢"
  functionOptProgram="RTSPARQADSLCMTYHARDWARELOGK.ASP"
  functionOptPrompt="N"  
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;<center>派工單號</center>;<center>項次</center>;<center>設備名稱/規格</center>;<center>數量</center>;<center>出庫別</center>;<center>資產編號</center>;<center>作廢日期</center>;<center>作廢原因</center>;<center>作廢人員</center>"
  sqlDelete="SELECT RTSPARQADSLCMTYHARDWARE.CUTYID, " _
         &"RTSPARQADSLCMTYHARDWARE.PRTNO, RTSPARQADSLCMTYHARDWARE.ENTRYNO, " _
         &"RTProdH.PRODNC + '--' + RTProdD1.SPEC, RTSPARQADSLCMTYHARDWARE.QTY, " _
         &"HBwarehouse.WARENAME, RTSPARQADSLCMTYHARDWARE.ASSETNO, RTSPARQADSLCMTYHARDWARE.DROPDAT, RTSPARQADSLCMTYHARDWARE.DROPREASON, RTObj.CUSNC " _
         &"FROM RTProdH RIGHT OUTER JOIN RTSPARQADSLCMTYHARDWARE LEFT OUTER JOIN " _
         &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
         &"RTSPARQADSLCMTYHARDWARE.DROPUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
         &"HBwarehouse ON RTSPARQADSLCMTYHARDWARE.WAREHOUSE = HBwarehouse.WAREHOUSE LEFT OUTER " _
         &"JOIN RTProdD1 ON RTSPARQADSLCMTYHARDWARE.PRODNO = RTProdD1.PRODNO AND " _
         &"RTSPARQADSLCMTYHARDWARE.ITEMNO = RTProdD1.ITEMNO ON RTProdH.PRODNO = RTSPARQADSLCMTYHARDWARE.PRODNO " _
         &"WHERE RTSPARQADSLCMTYHARDWARE.CUTYID=0 "
  dataTable="RTSPARQADSLCMTYHARDWARE"
  extTable=""
  numberOfKey=3
  dataProg="RTSPARQADSLCMTYHARDWARED.asp"
  datawindowFeature=""
  searchWindowFeature=""
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=False
  goodMorningImage=""
  colSplit=1
  keyListPageSize=25
  '----
  set connYY=server.CreateObject("ADODB.connection")
  set rsYY=server.CreateObject("ADODB.recordset")
  dsnYY="DSN=RTLIB"
  sqlYY="select * from RTSPARQADSLCMTY where CUTYID=" & ARYPARMKEY(0)
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
     TELEADDR=RSYY("TELEADDR")
  else
     COMN=""
     TELEADDR=""
  end if
  rsYY.Close
  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTSPARQADSLCMTYHARDWARE.CUTYID=" & aryparmkey(0) 
     searchShow="社區序號︰"& aryparmkey(0) & ",社區名稱︰" & COMN & ",主線位址︰" & COMADDR 
  ELSE
     SEARCHFIRST=FALSE
  End If  
  searchProg="self"
  sqlList="SELECT RTSPARQADSLCMTYHARDWARE.CUTYID, " _
         &"RTSPARQADSLCMTYHARDWARE.PRTNO, RTSPARQADSLCMTYHARDWARE.ENTRYNO, " _
         &"RTProdH.PRODNC + '--' + RTProdD1.SPEC, RTSPARQADSLCMTYHARDWARE.QTY, " _
         &"HBwarehouse.WARENAME, RTSPARQADSLCMTYHARDWARE.ASSETNO, RTSPARQADSLCMTYHARDWARE.DROPDAT, RTSPARQADSLCMTYHARDWARE.DROPREASON, RTObj.CUSNC " _
         &"FROM RTProdH RIGHT OUTER JOIN RTSPARQADSLCMTYHARDWARE LEFT OUTER JOIN " _
         &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
         &"RTSPARQADSLCMTYHARDWARE.DROPUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
         &"HBwarehouse ON RTSPARQADSLCMTYHARDWARE.WAREHOUSE = HBwarehouse.WAREHOUSE LEFT OUTER " _
         &"JOIN RTProdD1 ON RTSPARQADSLCMTYHARDWARE.PRODNO = RTProdD1.PRODNO AND " _
         &"RTSPARQADSLCMTYHARDWARE.ITEMNO = RTProdD1.ITEMNO ON RTProdH.PRODNO = RTSPARQADSLCMTYHARDWARE.PRODNO " _
         &"WHERE " &searchQry & ""
'Response.Write "sql=" & SQLLIST         
End Sub
%>
