<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HiBuilding管理系統"
  title="中華ADS399L社區及客戶資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"  
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
  formatName="none;none;none;社區名稱;用戶名稱;裝機地址;完工日;報竣日;退租日;連絡電話;經銷商" 
  sqlDelete="SELECT a.cutyid,isnull(b.cusid,''),isnull(b.entryno,''),a.comn,isnull(c.cusnc,'')," _
       &"isnull(d.cutnc+b.township2+b.raddr2,'') as rrr,isnull(b.finishdat,''),isnull(b.docketdat,''),b.dropdat,isnull(b.home,'')," _
       &"CASE WHEN ( e.CODENC = '' or e.CODENC is null ) THEN CASE WHEN  a.CUTID IN ('01', '02', '03', '04', '21', '22') " _
       &"AND a.township NOT IN ('三峽鎮', '鶯歌鎮') THEN '台北' WHEN a.cutid IN ('05', '06', '07', '08') OR " _
       &"( a.cutid = '03' AND  a.township IN ('三峽鎮', '鶯歌鎮')) THEN '桃園' WHEN  a.cutid IN ('09', '10', '11', '12', '13') " _
       &"THEN '台中' WHEN  a.cutid IN ('14', '15', '16', '17', '18', '19', '20') THEN '高雄' ELSE '' END ELSE e.CODENC END " _
       &"FROM " _
       &"(SELECT RTcustadslcmty.CUTYID, RTcustadslcmty.COMN, RTcustadslcmty.cmtytel, RTcustadslcmty.HBNO, RTcustadslcmty.IPADDR, " _
       &"IsNull(RTCounty.CUTNC,'')+RTcustadslcmty.TOWNSHIP+RTcustadslcmty.ADDR as bbb, RTcustadslcmty.ADSLAPPLY," _
       &"RTcustadslcmty.RCOMDROP, SUM(CASE WHEN RTcustadsl.cusid IS NOT NULL OR RTcustadsl.cusid <> '' THEN 1 ELSE 0 END) as aaa, " _
       &"SUM(CASE WHEN RTcustadsl.finishdat IS NOT NULL AND RTcustadsl.DROPDAT IS NULL THEN 1 ELSE 0 END) as cnt1, " _
       &"SUM(CASE WHEN RTcustadsl.docketdat IS NOT NULL AND RTcustadsl.DROPDAT IS NULL THEN 1 ELSE 0 END) as cnt2," _
       &"RTcustadslcmty.COMTYPE,RTcustadslcmty.cutid,RTcustadslcmty.township " _
       &"FROM RTcustadslcmty LEFT OUTER JOIN RTcustadsl ON RTcustadslcmty.CUTYID = RTcustadsl.COMQ1 " _
       &"LEFT OUTER JOIN RTCounty ON RTcustadslcmty.CUTID = RTCounty.CUTID " _
       &"WHERE RTcustadslcmty.CUTYID<>0 AND RTcustadslcmty.ADSLAPPLY IS NOT NULL AND RTcustadslcmty.RCOMDROP IS NULL AND " _
       &"RTcustadslcmty.CUTYID<>0 AND RTcustadslcmty.ADSLAPPLY IS NOT NULL AND RTcustadslcmty.RCOMDROP IS NULL " _
       &"GROUP BY RTcustadslcmty.CUTYID, RTcustadslcmty.COMN, RTcustadslcmty.cmtytel, RTcustadslcmty.HBNO, RTcustadslcmty.IPADDR, " _
       &"IsNull(RTCounty.CUTNC,'')+RTcustadslcmty.TOWNSHIP+RTcustadslcmty.ADDR, RTcustadslcmty.ADSLAPPLY," _
       &"RTcustadslcmty.RCOMDROP,RTcustadslcmty.COMTYPE,RTcustadslcmty.cutid,RTcustadslcmty.township " _
       &" " & SEARCHQRY5 &") a " _
       &"left outer join RTcustadsl b on a.cutyid=b.comq1 left outer join rtobj c on b.cusid=c.cusid left outer join " _
       &"rtcounty d on b.cutid2=d.cutid left outer join rtcode e on a.comtype = e.code and e.kind='B3' " _
       &"where b.docketdat is not null and b.dropdat is null AND " & SEARCHQRY & " " _
       &"order by a.comn,c.cusnc "
  dataTable="RTcustAdslCmty"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="/webap/rtap/base/rtadslcmty/rtcustd.asp"
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
  searchProg="RTCMTYCHTCUST399s.asp"
  searchFirst=FALSE
  '讀取暫存KEY
  KEYXX=SPLIT(SESSION("search5"),";")
  If searchQry="" Then
    searchQry=" B.cusid <> '*' "
    searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  if len(trim(keyxx(0)))> 0 then
     searchQry5=" HAVING SUM(CASE WHEN rtcustADSL.docketdat IS NOT NULL AND rtcustADSL.DROPDAT IS NULL THEN 1 ELSE 0 END) " & KEYXX(0) & " " & KEYXX(1)
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
sqllist="SELECT a.cutyid,isnull(b.cusid,''),isnull(b.entryno,''),a.comn,isnull(c.cusnc,'')," _
       &"isnull(d.cutnc+b.township2+b.raddr2,'') as rrr,isnull(b.finishdat,''),isnull(b.docketdat,''),b.dropdat,isnull(b.home,'')," _
       &"CASE WHEN ( e.CODENC = '' or e.CODENC is null ) THEN CASE WHEN  a.CUTID IN ('01', '02', '03', '04', '21', '22') " _
       &"AND a.township NOT IN ('三峽鎮', '鶯歌鎮') THEN '台北' WHEN a.cutid IN ('05', '06', '07', '08') OR " _
       &"( a.cutid = '03' AND  a.township IN ('三峽鎮', '鶯歌鎮')) THEN '桃園' WHEN  a.cutid IN ('09', '10', '11', '12', '13') " _
       &"THEN '台中' WHEN  a.cutid IN ('14', '15', '16', '17', '18', '19', '20') THEN '高雄' ELSE '' END ELSE e.CODENC END " _
       &"FROM " _
       &"(SELECT RTcustadslcmty.CUTYID, RTcustadslcmty.COMN, RTcustadslcmty.cmtytel, RTcustadslcmty.HBNO, RTcustadslcmty.IPADDR, " _
       &"IsNull(RTCounty.CUTNC,'')+RTcustadslcmty.TOWNSHIP+RTcustadslcmty.ADDR as bbb, RTcustadslcmty.ADSLAPPLY," _
       &"RTcustadslcmty.RCOMDROP, SUM(CASE WHEN RTcustadsl.cusid IS NOT NULL OR RTcustadsl.cusid <> '' THEN 1 ELSE 0 END) as aaa, " _
       &"SUM(CASE WHEN RTcustadsl.finishdat IS NOT NULL AND RTcustadsl.DROPDAT IS NULL THEN 1 ELSE 0 END) as cnt1, " _
       &"SUM(CASE WHEN RTcustadsl.docketdat IS NOT NULL AND RTcustadsl.DROPDAT IS NULL THEN 1 ELSE 0 END) as cnt2," _
       &"RTcustadslcmty.COMTYPE,RTcustadslcmty.cutid,RTcustadslcmty.township " _
       &"FROM RTcustadslcmty LEFT OUTER JOIN RTcustadsl ON RTcustadslcmty.CUTYID = RTcustadsl.COMQ1 " _
       &"LEFT OUTER JOIN RTCounty ON RTcustadslcmty.CUTID = RTCounty.CUTID " _
       &"WHERE RTcustadslcmty.CUTYID<>0 AND RTcustadslcmty.ADSLAPPLY IS NOT NULL AND RTcustadslcmty.RCOMDROP IS NULL AND " _
       &"RTcustadslcmty.CUTYID<>0 AND RTcustadslcmty.ADSLAPPLY IS NOT NULL AND RTcustadslcmty.RCOMDROP IS NULL " _
       &"GROUP BY RTcustadslcmty.CUTYID, RTcustadslcmty.COMN, RTcustadslcmty.cmtytel, RTcustadslcmty.HBNO, RTcustadslcmty.IPADDR, " _
       &"IsNull(RTCounty.CUTNC,'')+RTcustadslcmty.TOWNSHIP+RTcustadslcmty.ADDR, RTcustadslcmty.ADSLAPPLY," _
       &"RTcustadslcmty.RCOMDROP,RTcustadslcmty.COMTYPE,RTcustadslcmty.cutid,RTcustadslcmty.township " _
       &" " & SEARCHQRY5 &") a " _
       &"left outer join RTcustadsl b on a.cutyid=b.comq1 left outer join rtobj c on b.cusid=c.cusid left outer join " _
       &"rtcounty d on b.cutid2=d.cutid left outer join rtcode e on a.comtype = e.code and e.kind='B3' " _
       &"where b.docketdat is not null and b.dropdat is null AND " & SEARCHQRY & " " _
       &"order by a.comn,c.cusnc "
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>