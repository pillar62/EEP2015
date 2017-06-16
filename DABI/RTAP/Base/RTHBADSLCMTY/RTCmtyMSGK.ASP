<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="社區重大訊息維護作業"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="Y;Y;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;項次;事件;發生日期;結束日期;標題" 
  sqlDelete="SELECT rtcmtymsg.comq1,RTCMTYMSG.KIND, rtcmtymsg.entryno, RTcode.codenc, RTcmtymsg.occurdat, " _   
         &"RTcmtymsg.stopdat,rtcmtymsg.headline " _
         &"FROM RTCmtyMSG INNER JOIN " _
         &"RTCode ON RTCmtyMSG.EVENTID = RTCode.CODE AND RTCode.KIND = 'C9' " _
         &"WHERE Rtcmtymsg.comq1=0 "
  dataTable="RTcmtymsg"
  userDefineDelete=""
  numberOfKey=3
  dataProg="RTcmtymsgd.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=FALSE
  goodMorningImage="cbbn.JPG"
  colSplit=1
  keyListPageSize=20
  searchProg="self"
  searchFirst=False
  searchFirst=False
'  if TRIM(aryparmkey(1))="HB" then
'     kind="1"
'  else
'     KIND="2"
'  END IF
  If searchQry="" Then
     searchQry=" rtcmtymsg.comq1=" & aryparmkey(0) & " AND rtcmtymsg.kind='" & aryparmkey(1) & "' "
     searchShow=FrGetcmtyDesc(aryparmkey(0),aryparmkey(1))     
   '  searchShow="全部"
  End If
  sqlList="SELECT rtcmtymsg.comq1,RTCMTYMSG.KIND, rtcmtymsg.entryno, RTcode.codenc, RTcmtymsg.occurdat, " _   
         &"RTcmtymsg.stopdat,rtcmtymsg.headline " _
         &"FROM RTCmtyMSG INNER JOIN " _
         &"RTCode ON RTCmtyMSG.EVENTID = RTCode.CODE AND RTCode.KIND = 'C9' " _
         &"WHERE " & searchQry &" " _
         &"ORDER BY RTcmtymsg.comq1,rtcmtymsg.entryno"
 ' session("comq1")=aryparmkey(0)
 ' session("kind")=aryparmkey(1)
'  session("entryno")=aryparmkey(2)  
'Response.Write "SQL=" & SQLlist
End Sub

%>
<!-- #include file="RTGetCMTYDesc.inc" -->