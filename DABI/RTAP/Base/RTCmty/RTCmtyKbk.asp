<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="社區及客戶資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="營業員;業務員;管委會;客戶"
  functionOptProgram="RTCmtyBusK.asp;RTCmtySaleK.asp;RTCmtySpK.asp;RTCustK.asp"
  functionOptPrompt="N;N;N;N"
  accessMode="U"
  DSN="DSN=RTLib"
  formatName="建檔流水號;社區序號;社區名稱;縣市;總戶數;申裝戶數"
  sqlDelete="SELECT RTCmty.COMQ1 , RTCmty.COMQ2, RTCmty.COMN, RTCounty.CUTNC, RTCmty.COMCNT, " _
           &"RTCmty.APPLYCNT " _
           &"FROM RTCmty INNER JOIN RTCounty ON RTCmty.CUTID = RTCounty.CUTID " _
           &"WHERE (((RTCmty.COMQ1)=0)) "
  dataTable="RTCmty"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="RTCmtyD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=true
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=12
  searchProg="RTCmtyS.asp"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=False
  If searchQry="" Then
     searchQry=" RTCmty.ComQ1<>0 "
     searchShow="全部"
  End If
  If searchShow="全部" Then
  sqlList="SELECT RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN, RTCounty.CUTNC, " _
         &"RTCmty.COMCNT, RTCmty.APPLYCNT " _
         &"FROM (RTCounty INNER JOIN RTCmty ON " _
         &"RTCounty.CUTID=RTCmty.CUTID) INNER JOIN " _
         &"(RTArea INNER JOIN RTAreaCty ON RTArea.AREAID=RTAreaCty.AREAID) " _
         &"ON RTCmty.CUTID=RTAreaCty.CUTID " _
         &"WHERE RTArea.AREATYPE='1' AND " &searchQry &" " _
         &"ORDER BY RTCmty.COMQ1, RTCmty.COMQ2 "
  Else
  sqlList="SELECT RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN, RTCounty.CUTNC, " _
         &"RTCmty.COMCNT, RTCmty.APPLYCNT " _
         &"FROM (RTCounty INNER JOIN (RTCmtySale INNER JOIN RTCmty ON " _
         &"RTCmtySale.COMQ1 = RTCmty.COMQ1) ON RTCounty.CUTID = RTCmty.CUTID) " _
         &"INNER JOIN (RTArea INNER JOIN RTAreaCty ON RTArea.AREAID = RTAreaCty.AREAID) " _
         &"ON RTCmty.CUTID = RTAreaCty.CUTID " _
         &"WHERE RTArea.AREATYPE='1' AND rtcmtysale.exdat is null and " &searchQry &" " _
         &"ORDER BY RTCmty.COMQ1, RTCmty.COMQ2 "
  End If
End Sub
Sub SrRunUserDefineDelete()
  Dim conn,i
  Set conn=Server.CreateObject("ADODB.Connection")
  On Error Resume Next  
  conn.Open DSN
  If Len(extDeleList(1)) > 0 Then
     delSql="DELETE  FROM RTCmtyBus WHERE COMQ1 IN " &extDeleList(1) &" " 
     conn.Execute delSql
     delSql="DELETE  FROM RTCmtySale WHERE COMQ1 IN " &extDeleList(1) &" "
     conn.Execute delSql
     delSql="DELETE  FROM RTCmtySp WHERE COMQ1 IN " &extDeleList(1) &" "
     conn.Execute delSql
  End If
  conn.Close
End Sub
%>