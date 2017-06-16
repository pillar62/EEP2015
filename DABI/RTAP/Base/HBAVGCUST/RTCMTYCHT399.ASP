<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="ADSL社區及客戶資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="網路流量;客　戶"
  functionOptProgram="RTCMTYFLOW.ASP;/WEBAP/RTAP/BASE/RTadslcmty/RTCustK.asp"
  functionOptPrompt="N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;社區名稱;HB號碼;IP;設備位置;測通日;撤線日;申請;完工;報峻;撤銷退租;差異;經銷商" 
  sqlDelete="SELECT RTCustAdslCmty.CUTYID, RTCustAdslCmty.COMN, RTCustAdslCmty.HBNO, " _
           &"RTCustAdslCmty.IPADDR, RTCustAdslCmty.EQUIPADDR, RTCustAdslCmty.ADSLAPPLY, RTCustAdslCmty.RCOMDROP, " _
           &"SUM(CASE WHEN rtcustadsl.cusid IS NOT NULL OR rtcustadsl.CUSID <> '' THEN 1 ELSE 0 END), " _
           &"SUM(CASE WHEN (rtcustadsl.finishdat IS NOT NULL) AND rtcustadsl.dropdat is null THEN 1 ELSE 0 END), " _           
           &"SUM(CASE WHEN (rtcustadsl.docketdat IS NOT NULL ) and rtcustadsl.dropdat is null THEN 1 ELSE 0 END), " _
           &"SUM(CASE WHEN rtcustadsl.dropdat   IS NOT NULL OR rtcustadsl.dropdat <> ''   THEN 1 ELSE 0 END), " _
           &"case when rtcustadslcmty.rcomdrop is null then ( SUM(CASE WHEN rtcustadsl.cusid IS NOT NULL OR rtcustadsl.cusid <> '' THEN 1 ELSE 0 END)- " _
           &"SUM(CASE WHEN (rtcustadsl.docketdat IS NOT NULL OR rtcustadsl.docketdat <> '') and rtcustadsl.dropdat is null THEN 1 ELSE 0 END)- " _
           &"SUM(CASE WHEN rtcustadsl.dropdat   IS NOT NULL OR rtcustadsl.dropdat <> ''   THEN 1 ELSE 0 END)) else 0 end, " _       
           &"CASE WHEN ( RTCODE.CODENC = '' or RTCODE.CODENC is null ) THEN CASE WHEN  RTCustAdslCmty.CUTID IN ('01', '02', '03', '04', '21', '22') " _
           &"AND RTCustAdslCmty.township NOT IN ('三峽鎮', '鶯歌鎮') THEN '台北' WHEN RTCustAdslCmty.cutid IN ('05', '06', '07', '08') OR " _
           &"( RTCustAdslCmty.cutid = '03' AND  RTCustAdslCmty.township IN ('三峽鎮', '鶯歌鎮')) THEN '桃園' WHEN  RTCustAdslCmty.cutid IN ('09', '10', '11', '12', '13') " _
           &"THEN '台中' WHEN RTCustAdslCmty.cutid IN ('14', '15', '16', '17', '18', '19', '20') THEN '高雄' ELSE '' END ELSE RTCODE.CODENC END " _
           &"FROM RTCustAdslCmty LEFT OUTER JOIN RTCustADSL ON RTCustAdslCmty.CUTYID = RTCustADSL.COMQ1 " _
           &"WHERE (RTCustAdslCmty.COMN <> '*') " _
           &"GROUP BY  RTCustAdslCmty.CUTYID, RTCustAdslCmty.COMN, RTCustAdslCmty.HBNO, " _
           &"RTCustAdslCmty.IPADDR, RTCustAdslCmty.EQUIPADDR, " _
           &"RTCustAdslCmty.ADSLAPPLY, RTCustAdslCmty.RCOMDROP, " _
           &"CASE WHEN ( RTCODE.CODENC = '' or RTCODE.CODENC is null ) THEN CASE WHEN  RTCustAdslCmty.CUTID IN ('01', '02', '03', '04', '21', '22') " _
           &"AND RTCustAdslCmty.township NOT IN ('三峽鎮', '鶯歌鎮') THEN '台北' WHEN RTCustAdslCmty.cutid IN ('05', '06', '07', '08') OR " _
           &"( RTCustAdslCmty.cutid = '03' AND  RTCustAdslCmty.township IN ('三峽鎮', '鶯歌鎮')) THEN '桃園' WHEN  RTCustAdslCmty.cutid IN ('09', '10', '11', '12', '13') " _
           &"( RTCustAdslCmty.cutid = '03' AND  RTCustAdslCmty.township IN ('三峽鎮', '鶯歌鎮')) THEN '桃園' WHEN  RTCustAdslCmty.cutid IN ('09', '10', '11', '12', '13') " _
           &"THEN '台中' WHEN RTCustAdslCmty.cutid IN ('14', '15', '16', '17', '18', '19', '20') THEN '高雄' ELSE '' END ELSE RTCODE.CODENC END " _
           &"ORDER BY  RTCustAdslCmty.equipaddr "
  dataTable="RTCUSTADSLCmty"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="/WEBAP/RTAP/BASE/RTADSLCMTY/RTCmtyD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=FALSE
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="RTCMTYCHT399s.ASP"
' Open search program when first entry this keylist
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=FALSE
    '讀取暫存KEY
  KEYXX=SPLIT(SESSION("search5"),";")
  If searchQry="" Then
    ' searchQry=" RTCUSTADSLCMTY.CUTYID<>0 and rtcustadsl.dropdat is null and rtcustadsl.agree <>'N' "
     searchQry=" RTCUSTADSLCMTY.CUTYID<>0  AND RTCUSTADSLCMTY.ADSLAPPLY IS NOT NULL AND RTCUSTADSLCMTY.RCOMDROP IS null "
    ' searchShow="全部(不含退租、撤銷、不可建置戶)"
     searchQry2=" "
    searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  if len(trim(keyxx(0)))> 0 then
     searchQry5=" HAVING SUM(CASE WHEN rtcustadsl.DOCKETdat IS NOT NULL AND rtcustadsl.DROPDAT IS NULL THEN 1 ELSE 0 END) " & KEYXX(0) & " " & KEYXX(1)
  else
     SEARCHQRY5=""
  END IF

  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  '讀取登入帳號之群組資料
  set connXX=server.CreateObject("ADODB.connection")
  set rsXX=server.CreateObject("ADODB.recordset")
  dsnxx="DSN=RTLIB"
  sqlxx="select areaid,groupid from RTsalesgroupref where emply='" & emply & "' "
  connxx.Open dsnxx
  rsxx.Open sqlxx,connxx
  areaid=""
  groupid=""
  DO while not rsxx.eof
     leadingA=","
     leadingB=","  
     if len(trim(areaid))=0 then leadingA=""
     if len(trim(groupid))=0 then leadingB=""
     areaid=areaid & leadingA & "'" &  rsxx("areaid") & "' "
     groupid=groupid & leadingB & "'" & rsxx("groupid") & "' "
     rsxx.movenext 
  loop
  rsxx.Close
  connxx.Close
  set rsxx=nothing
  set connxx=nothing

  sqllist="SELECT RTCustAdslCmty.CUTYID, RTCustAdslCmty.COMN, RTCustAdslCmty.HBNO, " _
       &"RTCustAdslCmty.IPADDR, RTCustAdslCmty.EQUIPADDR, RTCustAdslCmty.ADSLAPPLY, RTCustAdslCmty.RCOMDROP, " _
       &"SUM(CASE WHEN rtcustadsl.cusid IS NOT NULL OR rtcustadsl.cusid <> '' THEN 1 ELSE 0 END), " _ 
       &"SUM(CASE WHEN (rtcustadsl.finishdat IS NOT NULL ) AND rtcustadsl.dropdat is null THEN 1 ELSE 0 END), " _           
       &"SUM(CASE WHEN (rtcustadsl.docketdat IS NOT NULL ) and rtcustadsl.dropdat is null THEN 1 ELSE 0 END), " _
       &"SUM(CASE WHEN rtcustadsl.dropdat   IS NOT NULL   THEN 1 ELSE 0 END), " _ 
       &"case when rtcustadslcmty.rcomdrop is null then ( SUM(CASE WHEN rtcustadsl.cusid IS NOT NULL OR rtcustadsl.cusid <> '' THEN 1 ELSE 0 END)- " _
       &"SUM(CASE WHEN (rtcustadsl.docketdat IS NOT NULL ) and rtcustadsl.dropdat is null THEN 1 ELSE 0 END)- " _
       &"SUM(CASE WHEN rtcustadsl.dropdat   IS NOT NULL OR rtcustadsl.dropdat <> ''   THEN 1 ELSE 0 END)) else 0 end, " _        
       &"CASE WHEN ( RTCODE.CODENC = '' or RTCODE.CODENC is null ) THEN CASE WHEN  RTCustAdslCmty.CUTID IN ('01', '02', '03', '04', '21', '22') " _
       &"AND RTCustAdslCmty.township NOT IN ('三峽鎮', '鶯歌鎮') THEN '台北' WHEN RTCustAdslCmty.cutid IN ('05', '06', '07', '08') OR " _
       &"( RTCustAdslCmty.cutid = '03' AND  RTCustAdslCmty.township IN ('三峽鎮', '鶯歌鎮')) THEN '桃園' WHEN  RTCustAdslCmty.cutid IN ('09', '10', '11', '12', '13') " _
       &"THEN '台中' WHEN RTCustAdslCmty.cutid IN ('14', '15', '16', '17', '18', '19', '20') THEN '高雄' ELSE '' END ELSE RTCODE.CODENC END " _
       &"FROM RTCustAdslCmty LEFT OUTER JOIN RTCustADSL ON RTCustAdslCmty.CUTYID = RTCustADSL.COMQ1 " _
       &"left outer join rtcode on rtcustadslcmty.comtype=rtcode.code and rtcode.kind='B3' " _
       &"WHERE " &  searchqry  & " " _
       &"GROUP BY  RTCustAdslCmty.CUTYID, RTCustAdslCmty.COMN, RTCustAdslCmty.HBNO, " _
       &"RTCustAdslCmty.IPADDR, RTCustAdslCmty.EQUIPADDR, " _
       &"RTCustAdslCmty.ADSLAPPLY, RTCustAdslCmty.RCOMDROP, " _
       &"CASE WHEN ( RTCODE.CODENC = '' or RTCODE.CODENC is null ) THEN CASE WHEN  RTCustAdslCmty.CUTID IN ('01', '02', '03', '04', '21', '22') " _
       &"AND RTCustAdslCmty.township NOT IN ('三峽鎮', '鶯歌鎮') THEN '台北' WHEN RTCustAdslCmty.cutid IN ('05', '06', '07', '08') OR " _
       &"( RTCustAdslCmty.cutid = '03' AND RTCustAdslCmty.township IN ('三峽鎮', '鶯歌鎮')) THEN '桃園' WHEN  RTCustAdslCmty.cutid IN ('09', '10', '11', '12', '13') " _
       &"THEN '台中' WHEN RTCustAdslCmty.cutid IN ('14', '15', '16', '17', '18', '19', '20') THEN '高雄' ELSE '' END ELSE RTCODE.CODENC END " _
       & SEARCHQRY5 & " " _
       &"ORDER BY  RTCustAdslCmty.comn "
'response.Write SQLLIST
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>