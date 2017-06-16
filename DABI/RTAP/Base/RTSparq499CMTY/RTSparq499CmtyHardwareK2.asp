<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="速博499管理系統"
  title="速博499主線設備資料查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="異動查詢"
  functionOptProgram="RTSparq499CmtyHARDWARELOGK.ASP"
  functionOptPrompt="N"  
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;<center>派工單號</center>;<center>項次</center>;<center>設備名稱/規格</center>;<center>數量</center>;<center>出庫別</center>;<center>資產編號</center>;<center>作廢日期</center>;<center>作廢原因</center>;<center>作廢人員</center>"
  sqlDelete="SELECT RTSparq499CmtyHARDWARE.COMQ1, RTSparq499CmtyHARDWARE.LINEQ1, " _
         &"RTSparq499CmtyHARDWARE.PRTNO, RTSparq499CmtyHARDWARE.ENTRYNO, " _
         &"RTProdH.PRODNC + '--' + RTProdD1.SPEC, RTSparq499CmtyHARDWARE.QTY, " _
         &"HBwarehouse.WARENAME, RTSparq499CmtyHARDWARE.ASSETNO, RTSparq499CmtyHARDWARE.DROPDAT, RTSparq499CmtyHARDWARE.DROPREASON, RTObj.CUSNC " _
         &"FROM RTProdH RIGHT OUTER JOIN RTSparq499CmtyHARDWARE LEFT OUTER JOIN " _
         &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
         &"RTSparq499CmtyHARDWARE.DROPUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
         &"HBwarehouse ON RTSparq499CmtyHARDWARE.WAREHOUSE = HBwarehouse.WAREHOUSE LEFT OUTER " _
         &"JOIN RTProdD1 ON RTSparq499CmtyHARDWARE.PRODNO = RTProdD1.PRODNO AND " _
         &"RTSparq499CmtyHARDWARE.ITEMNO = RTProdD1.ITEMNO ON RTProdH.PRODNO = RTSparq499CmtyHARDWARE.PRODNO " _
         &"WHERE RTSparq499CmtyHARDWARE.COMQ1=0 "
  dataTable="RTSparq499CmtyHARDWARE"
  extTable=""
  numberOfKey=4
  dataProg="RTSparq499CmtyHARDWARED.asp"
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
  sqlYY="select * from RTSparq499CmtyH LEFT OUTER JOIN RTCOUNTY ON RTSparq499CmtyH.CUTID=RTCOUNTY.CUTID where COMQ1=" & ARYPARMKEY(0)
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
  else
     COMN=""
  end if
  rsYY.Close
  sqlYY="select * from RTSparq499Cmtyline LEFT OUTER JOIN RTCOUNTY ON RTSparq499Cmtyline.CUTID=RTCOUNTY.CUTID where COMQ1=" & ARYPARMKEY(0) & " and lineq1=" & aryparmkey(1)
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     comaddr=""
     COMaddr=rsYY("cutnc") & rsyy("township") & rsyy("raddr")
  else
     COMaddr=""
  end if
  rsYY.Close
  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTSparq499Cmtyhardware.ComQ1=" & aryparmkey(0) & " and RTSparq499Cmtyhardware.lineq1=" & aryparmkey(1) 
     searchShow="社區序號︰"& aryparmkey(0) & ",社區名稱︰" & COMN &",主線序號︰" & aryparmkey(1) & ",主線位址︰" & COMADDR 
  ELSE
     SEARCHFIRST=FALSE
  End If  
  searchProg="self"
  sqlList="SELECT RTSparq499CmtyHARDWARE.COMQ1, RTSparq499CmtyHARDWARE.LINEQ1, " _
         &"RTSparq499CmtyHARDWARE.PRTNO, RTSparq499CmtyHARDWARE.ENTRYNO, " _
         &"RTProdH.PRODNC + '--' + RTProdD1.SPEC, RTSparq499CmtyHARDWARE.QTY, " _
         &"HBwarehouse.WARENAME, RTSparq499CmtyHARDWARE.ASSETNO, RTSparq499CmtyHARDWARE.DROPDAT, RTSparq499CmtyHARDWARE.DROPREASON, RTObj.CUSNC " _
         &"FROM RTProdH RIGHT OUTER JOIN RTSparq499CmtyHARDWARE LEFT OUTER JOIN " _
         &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
         &"RTSparq499CmtyHARDWARE.DROPUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
         &"HBwarehouse ON RTSparq499CmtyHARDWARE.WAREHOUSE = HBwarehouse.WAREHOUSE LEFT OUTER " _
         &"JOIN RTProdD1 ON RTSparq499CmtyHARDWARE.PRODNO = RTProdD1.PRODNO AND " _
         &"RTSparq499CmtyHARDWARE.ITEMNO = RTProdD1.ITEMNO ON RTProdH.PRODNO = RTSparq499CmtyHARDWARE.PRODNO " _
         &"WHERE " &searchQry & ""
'Response.Write "sql=" & SQLLIST         
End Sub
%>
