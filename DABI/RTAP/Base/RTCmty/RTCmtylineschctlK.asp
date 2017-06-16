<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="社區主線派工單資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="設備查詢;用戶查詢;建立派工單"
  functionOptProgram="rtCMTYHARDWAREK2.asp;rtcustK.asp;rtCMTYSNDWORKk.asp"
  functionOptPrompt="N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;社區名稱;IP;社區位置;經銷商;機櫃到位;COT到位;測通日;撤線日;申請;完工;報竣;退租;撤銷;有效戶;待處理" 
  sqlDelete="SELECT RTCmty.COMQ1, RTCmty.COMN, RTCmty.NETIP,RTCounty.CUTNC + RTCmty.TOWNSHIP + RTCmty.ADDR AS Expr1, " _
         &"RTCode.CODENC, RTCmty.RACKARRIVE, RTCmty.COTARRIVE,RTCmty.T1APPLY, RTCmty.RCOMDROP, " _
         &"SUM(CASE WHEN RTCUST.CUSID IS NOT NULL THEN 1 ELSE 0 END) AS CUSTCNT, " _
         &"SUM(CASE WHEN RTCUST.FINISHDAT IS NOT NULL THEN 1 ELSE 0 END) AS FCNT, " _
         &"SUM(CASE WHEN RTCUST.DOCKETDAT IS NOT NULL THEN 1 ELSE 0 END) AS DCNT, " _
         &"SUM(CASE WHEN RTCUST.DROPDAT IS NOT NULL AND DOCKETDAT IS NOT NULL THEN 1 ELSE 0 END) AS RCNT, " _
         &"SUM(CASE WHEN RTCUST.DROPDAT IS NOT NULL AND DOCKETDAT IS NULL THEN 1 ELSE 0 END) AS CCNT, " _
         &"SUM(CASE WHEN RTCUST.DOCKETDAT IS NOT NULL AND DROPDAT IS NULL THEN 1 ELSE 0 END) AS UCNT, " _
         &"SUM(CASE WHEN RTCUST.CUSID IS NOT NULL THEN 1 ELSE 0 END) - " _
         &"SUM(CASE WHEN RTCUST.DOCKETDAT IS NOT NULL AND DROPDAT IS NULL THEN 1 ELSE 0 END) - " _
         &"SUM(CASE WHEN RTCUST.DROPDAT IS NOT NULL AND DOCKETDAT IS NOT NULL THEN 1 ELSE 0 END) - " _
         &"SUM(CASE WHEN RTCUST.DROPDAT IS NOT NULL AND DOCKETDAT IS NULL THEN 1 ELSE 0 END) AS PCNT " _
         &"FROM RTCmty LEFT OUTER JOIN  RTCust ON RTCmty.COMQ1 = RTCust.COMQ1 LEFT OUTER JOIN RTCode ON " _
         &"RTCmty.COMTYPE = RTCode.CODE AND RTCode.KIND = 'B3' LEFT OUTER JOIN RTCounty ON RTCmty.CUTID = RTCounty.CUTID " _
         &"where " & searchqry & " " _
         &"GROUP BY  RTCmty.COMQ1, RTCmty.COMN, RTCmty.NETIP, RTCounty.CUTNC + RTCmty.TOWNSHIP + RTCmty.ADDR, RTCode.CODENC, " _
         &"RTCmty.RACKARRIVE, RTCmty.COTARRIVE, RTCmty.T1APPLY, RTCmty.RCOMDROP "
  dataTable="RTCMTY"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="RTCMTYD.ASP"
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
  keyListPageSize=25
  searchProg="RTCMTYLINEschctlS.asp"
' Open search program when first entry this keylist
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=true
  If searchQry="" Then
    ' searchQry=" RTCUSTADSLCMTY.COMQ1<>0 and rtcustadsl.dropdat is null and rtcustadsl.agree <>'N' "
     searchQry=" (RTCMTY.NETIP <> '') AND (RTCMTY.T1APPLY IS NULL) AND (RTCMTY.RCOMDROP IS NULL) "
    ' searchShow="全部(不含退租、撤銷、不可建置戶)"
    searchShow="待派工主線全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  '讀取登入帳號之群組資料

  'Response.Write "GP=" & usergroup
  '-------------------------------------------------------------------------------------------
  sqllist="SELECT RTCmty.COMQ1, RTCmty.COMN, RTCmty.NETIP,RTCounty.CUTNC + RTCmty.TOWNSHIP + RTCmty.ADDR AS Expr1, " _
         &"RTCode.CODENC, RTCmty.RACKARRIVE, RTCmty.COTARRIVE,RTCmty.T1APPLY, RTCmty.RCOMDROP, " _
         &"SUM(CASE WHEN RTCUST.CUSID IS NOT NULL THEN 1 ELSE 0 END) AS CUSTCNT, " _
         &"SUM(CASE WHEN RTCUST.FINISHDAT IS NOT NULL THEN 1 ELSE 0 END) AS FCNT, " _
         &"SUM(CASE WHEN RTCUST.DOCKETDAT IS NOT NULL THEN 1 ELSE 0 END) AS DCNT, " _
         &"SUM(CASE WHEN RTCUST.DROPDAT IS NOT NULL AND DOCKETDAT IS NOT NULL THEN 1 ELSE 0 END) AS RCNT, " _
         &"SUM(CASE WHEN RTCUST.DROPDAT IS NOT NULL AND DOCKETDAT IS NULL THEN 1 ELSE 0 END) AS CCNT, " _
         &"SUM(CASE WHEN RTCUST.DOCKETDAT IS NOT NULL AND DROPDAT IS NULL THEN 1 ELSE 0 END) AS UCNT, " _
         &"SUM(CASE WHEN RTCUST.CUSID IS NOT NULL THEN 1 ELSE 0 END) - " _
         &"SUM(CASE WHEN RTCUST.DOCKETDAT IS NOT NULL AND DROPDAT IS NULL THEN 1 ELSE 0 END) - " _
         &"SUM(CASE WHEN RTCUST.DROPDAT IS NOT NULL AND DOCKETDAT IS NOT NULL THEN 1 ELSE 0 END) - " _
         &"SUM(CASE WHEN RTCUST.DROPDAT IS NOT NULL AND DOCKETDAT IS NULL THEN 1 ELSE 0 END) AS PCNT " _
         &"FROM RTCmty LEFT OUTER JOIN  RTCust ON RTCmty.COMQ1 = RTCust.COMQ1 LEFT OUTER JOIN RTCode ON " _
         &"RTCmty.COMTYPE = RTCode.CODE AND RTCode.KIND = 'B3' LEFT OUTER JOIN RTCounty ON RTCmty.CUTID = RTCounty.CUTID " _
         &"where " & searchqry & " " _
         &"GROUP BY  RTCmty.COMQ1, RTCmty.COMN, RTCmty.NETIP, RTCounty.CUTNC + RTCmty.TOWNSHIP + RTCmty.ADDR, RTCode.CODENC, " _
         &"RTCmty.RACKARRIVE, RTCmty.COTARRIVE, RTCmty.T1APPLY, RTCmty.RCOMDROP "

 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>