<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="Sparq* 管理系統"
  title="速博ADSL社區主線派工作業"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="設備查詢;用戶查詢;建立派工單"
  functionOptProgram="rtSPARQADSLCMTYHARDWAREK2.asp;rtcustK.asp;rtSPARQADSLCMTYSNDWORKk.asp"
  functionOptPrompt="N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;社區名稱;附掛電話;IP;電信室位置;none;直經銷;none;none;設備到位;線路到位;測通日;撤線日;申請戶;完工戶;報竣戶;退租戶;撤銷戶;待處理" 
  sqlDelete="SELECT RTSparqAdslCmty.CUTYID, RTSparqAdslCmty.COMN, RTSparqAdslCmty.CMTYTEL, RTSparqAdslCmty.IPADDR, " _
           &"RTSparqAdslCmty.TELEADDR, RTCode.CODENC, " _
           &"CASE WHEN RTObj.SHORTNC IS NOT NULL THEN RTObj.SHORTNC ELSE RTArea.AREANC END, " _
           &"RTSparqAdslCmty.RCVD, RTSparqAdslCmty.CASESNDWRK, RTSparqAdslCmty.EQUIPARRIVE, RTSparqAdslCmty.LINEARRIVE, " _
           &"RTSparqAdslCmty.ADSLAPPLY,RTSparqAdslCmty.rcomdrop,sum(case when rtsparqadslcust.cusid is not null then 1 else 0 end),sum(case when rtsparqadslcust.finishdat is not null then 1 else 0 end)," _
           &"sum(case when rtsparqadslcust.docketdat is not null then 1 else 0 end)," _
           &"sum(case when rtsparqadslcust.docketdat is not null and rtsparqadslcust.dropdat is not null  then 1 else 0 end)," _
           &"sum(case when rtsparqadslcust.docketdat is  null and rtsparqadslcust.dropdat is not null  then 1 else 0 end)," _
           &"sum(case when rtsparqadslcust.cusid is not null then 1 else 0 end)-sum(case when rtsparqadslcust.docketdat is not null then 1 else 0 end)-sum(case when rtsparqadslcust.docketdat is not null and rtsparqadslcust.dropdat is not null  then 1 else 0 end)-sum(case when rtsparqadslcust.docketdat is  null and rtsparqadslcust.dropdat is not null  then 1 else 0 end) " _
           &"FROM RTObj RIGHT OUTER JOIN RTSparqAdslCmty ON RTObj.CUSID = RTSparqAdslCmty.CONSIGNEE LEFT OUTER JOIN " _
           &"RTCode ON RTSparqAdslCmty.CONNECTTYPE = RTCode.CODE AND RTCode.KIND = 'G5' LEFT OUTER JOIN " _
           &"RTArea INNER JOIN RTSalesGroup ON RTArea.AREAID = RTSalesGroup.COMPLOCATION ON " _
           &"RTSparqAdslCmty.AREAID = RTSalesGroup.AREAID AND RTSparqAdslCmty.GROUPID = RTSalesGroup.GROUPID  LEFT OUTER JOIN RTSPARQADSLCUST ON RTSPARQADSLCMTY.CUTYID=RTSPARQADSLCUST.COMQ1 " _
           &"WHERE (RTSparqAdslCmty.IPADDR <> '') AND (RTSparqAdslCmty.ADSLAPPLY IS NULL) AND (RTSparqAdslCmty.RCOMDROP IS NULL) " _
           &"GROUP BY  RTSparqAdslCmty.CUTYID, RTSparqAdslCmty.COMN,RTSparqAdslCmty.CMTYTEL, RTSparqAdslCmty.IPADDR, " _
           &"RTSparqAdslCmty.EQUIPADDR, RTSparqAdslCmty.TELEADDR, RTCode.CODENC, CASE WHEN RTObj.SHORTNC IS NOT NULL " _
           &"THEN RTObj.SHORTNC ELSE RTArea.AREANC END, RTSparqAdslCmty.RCVD, RTSparqAdslCmty.CASESNDWRK, RTSparqAdslCmty.EQUIPARRIVE, " _
           &"RTSparqAdslCmty.LINEARRIVE, RTSparqAdslCmty.ADSLAPPLY,RTSparqAdslCmty.rcomdrop "
  dataTable="RTSparqAdslCmty"
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
  searchProg="RTsparqadslcmtyschctlS.asp"
' Open search program when first entry this keylist
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=true
  If searchQry="" Then
    ' searchQry=" RTCUSTADSLCMTY.CUTYID<>0 and rtcustadsl.dropdat is null and rtcustadsl.agree <>'N' "
     searchQry=" (RTSparqAdslCmty.IPADDR <> '') AND (RTSparqAdslCmty.ADSLAPPLY IS NULL) AND (RTSparqAdslCmty.RCOMDROP IS NULL) "
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
  sqllist="SELECT RTSparqAdslCmty.CUTYID, RTSparqAdslCmty.COMN, RTSparqAdslCmty.CMTYTEL, RTSparqAdslCmty.IPADDR, " _
           &"RTSparqAdslCmty.TELEADDR, RTCode.CODENC, " _
           &"CASE WHEN RTObj.SHORTNC IS NOT NULL THEN RTObj.SHORTNC ELSE RTArea.AREANC END, " _
           &"RTSparqAdslCmty.RCVD, RTSparqAdslCmty.CASESNDWRK, RTSparqAdslCmty.EQUIPARRIVE, RTSparqAdslCmty.LINEARRIVE, " _
           &"RTSparqAdslCmty.ADSLAPPLY,RTSparqAdslCmty.rcomdrop,sum(case when rtsparqadslcust.cusid is not null then 1 else 0 end),sum(case when rtsparqadslcust.finishdat is not null then 1 else 0 end)," _
           &"sum(case when rtsparqadslcust.docketdat is not null then 1 else 0 end)," _
           &"sum(case when rtsparqadslcust.docketdat is not null and rtsparqadslcust.dropdat is not null  then 1 else 0 end)," _
           &"sum(case when rtsparqadslcust.docketdat is  null and rtsparqadslcust.dropdat is not null  then 1 else 0 end)," _
           &"sum(case when rtsparqadslcust.cusid is not null then 1 else 0 end) -sum(case when rtsparqadslcust.docketdat is not null then 1 else 0 end)-sum(case when rtsparqadslcust.docketdat is not null and rtsparqadslcust.dropdat is not null  then 1 else 0 end)-sum(case when rtsparqadslcust.docketdat is  null and rtsparqadslcust.dropdat is not null  then 1 else 0 end) " _
           &"FROM RTObj RIGHT OUTER JOIN RTSparqAdslCmty ON RTObj.CUSID = RTSparqAdslCmty.CONSIGNEE LEFT OUTER JOIN " _
           &"RTCode ON RTSparqAdslCmty.CONNECTTYPE = RTCode.CODE AND RTCode.KIND = 'G5' LEFT OUTER JOIN " _
           &"RTArea INNER JOIN RTSalesGroup ON RTArea.AREAID = RTSalesGroup.COMPLOCATION ON " _
           &"RTSparqAdslCmty.AREAID = RTSalesGroup.AREAID AND RTSparqAdslCmty.GROUPID = RTSalesGroup.GROUPID  LEFT OUTER JOIN RTSPARQADSLCUST ON RTSPARQADSLCMTY.CUTYID=RTSPARQADSLCUST.COMQ1 " _
           &"WHERE " & searchqry & " " _
           &"GROUP BY  RTSparqAdslCmty.CUTYID, RTSparqAdslCmty.COMN,RTSparqAdslCmty.CMTYTEL, RTSparqAdslCmty.IPADDR, " _
           &"RTSparqAdslCmty.EQUIPADDR, RTSparqAdslCmty.TELEADDR, RTCode.CODENC, CASE WHEN RTObj.SHORTNC IS NOT NULL " _
           &"THEN RTObj.SHORTNC ELSE RTArea.AREANC END, RTSparqAdslCmty.RCVD, RTSparqAdslCmty.CASESNDWRK, RTSparqAdslCmty.EQUIPARRIVE, " _
           &"RTSparqAdslCmty.LINEARRIVE, RTSparqAdslCmty.ADSLAPPLY,RTSparqAdslCmty.rcomdrop "

 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>