<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="HI-Building主線設備資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName=" 作廢 ; 作廢返轉;異動查詢"
  functionOptProgram="RTcmtyHARDWAREDROP.ASP;RTcmtyHARDWAREDROPc.ASP;RTcmtyHARDWARELOGK.ASP"
  functionOptPrompt="N;N;N"  
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;<center>派工單號</center>;<center>項次</center>;<center>設備名稱/規格</center>;<center>數量</center>;<center>出庫別</center>;<center>資產編號</center>;<center>作廢日期</center>;<center>作廢原因</center>;<center>作廢人員</center>"
  sqlDelete="SELECT RTcmtyHARDWARE.COMQ1, " _
         &"RTcmtyHARDWARE.PRTNO, RTcmtyHARDWARE.ENTRYNO, " _
         &"RTProdH.PRODNC + '--' + RTProdD1.SPEC, RTcmtyHARDWARE.QTY, " _
         &"HBwarehouse.WARENAME, RTcmtyHARDWARE.ASSETNO, RTcmtyHARDWARE.DROPDAT, RTcmtyHARDWARE.DROPREASON, RTObj.CUSNC " _
         &"FROM RTProdH RIGHT OUTER JOIN RTcmtyHARDWARE LEFT OUTER JOIN " _
         &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
         &"RTcmtyHARDWARE.DROPUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
         &"HBwarehouse ON RTcmtyHARDWARE.WAREHOUSE = HBwarehouse.WAREHOUSE LEFT OUTER " _
         &"JOIN RTProdD1 ON RTcmtyHARDWARE.PRODNO = RTProdD1.PRODNO AND " _
         &"RTcmtyHARDWARE.ITEMNO = RTProdD1.ITEMNO ON RTProdH.PRODNO = RTcmtyHARDWARE.PRODNO " _
         &"WHERE RTcmtyHARDWARE.COMQ1=0 "
  dataTable="RTcmtyHARDWARE"
  extTable=""
  numberOfKey=3
  dataProg="RTcmtyHARDWARED.asp"
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
  sqlYY="select * from rtcmty LEFT OUTER JOIN RTCOUNTY ON RTCMTY.CUTID=RTCOUNTY.CUTID where COMQ1=" & ARYPARMKEY(0)
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
     TELEADDR=RSYY("CUTNC") & RSYY("TOWNSHIP") & RSYY("ADDR")
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
     searchQry=" RTcmtyHARDWARE.COMQ1=" & aryparmkey(0)  & " and RTcmtyHARDWARE.prtno='" & aryparmkey(1) & "' "
     searchShow="社區序號︰"& aryparmkey(0) & ",社區名稱︰" & COMN & ",電信室位置︰" & COMADDR & ",派工單號︰" & aryparmkey(1)
  ELSE
     SEARCHFIRST=FALSE
  End If  
  searchProg="self"
  sqlList="SELECT RTcmtyHARDWARE.COMQ1, " _
         &"RTcmtyHARDWARE.PRTNO, RTcmtyHARDWARE.ENTRYNO, " _
         &"RTProdH.PRODNC + '--' + RTProdD1.SPEC, RTcmtyHARDWARE.QTY, " _
         &"HBwarehouse.WARENAME, RTcmtyHARDWARE.ASSETNO, RTcmtyHARDWARE.DROPDAT, RTcmtyHARDWARE.DROPREASON, RTObj.CUSNC " _
         &"FROM RTProdH RIGHT OUTER JOIN RTcmtyHARDWARE LEFT OUTER JOIN " _
         &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
         &"RTcmtyHARDWARE.DROPUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
         &"HBwarehouse ON RTcmtyHARDWARE.WAREHOUSE = HBwarehouse.WAREHOUSE LEFT OUTER " _
         &"JOIN RTProdD1 ON RTcmtyHARDWARE.PRODNO = RTProdD1.PRODNO AND " _
         &"RTcmtyHARDWARE.ITEMNO = RTProdD1.ITEMNO ON RTProdH.PRODNO = RTcmtyHARDWARE.PRODNO " _
         &"WHERE " &searchQry & ""
'Response.Write "sql=" & SQLLIST         
End Sub
%>
