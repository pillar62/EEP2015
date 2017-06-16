<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
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
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;社區名稱;用戶名稱;裝機地址;完工日;報竣日;退租日;連絡電話;經銷商 " 
  sqlDelete="SELECT a.COMQ1,isnull(b.cusid,''),isnull(b.entryno,''),a.comn,isnull(c.cusnc,'')," _
         &"isnull(d.cutnc+b.township1+b.raddr1,'') as rrr,isnull(b.finishdat,''),isnull(b.docketdat,''),b.dropdat," _
         &"isnull(b.home,''),CASE WHEN ( e.CODENC = '' or e.CODENC is null ) THEN CASE WHEN  " _
         &"a.CUTID IN ('01', '02', '03', '04', '21', '22') AND a.township NOT IN ('三峽鎮', '鶯歌鎮') " _
         &"THEN '台北' WHEN a.cutid IN ('05', '06', '07', '08') OR ( a.cutid = '03' AND  a.township IN ('三峽鎮','鶯歌鎮')) " _
         &"THEN '桃園' WHEN  a.cutid IN ('09', '10', '11', '12', '13') THEN '台中' WHEN  a.cutid IN " _
         &"('14', '15', '16', '17', '18', '19', '20') THEN '高雄' ELSE '' END ELSE e.CODENC END " _
         &"FROM " _
         &"(SELECT RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN,t1no,netip, RTCounty.CUTNC, RTCmty.COMCNT, " _
         &"sum( CASE custype when '申裝戶' THEN 1 ELSE 0 end) HHH, sum(CASE custype when '深耕戶' THEN 1 ELSE 0 end) AS AAA, " _
         &"Sum(CASE custype when '' THEN 1 ELSE 0 end) AS JJJ, Sum(CASE when DROPDAT is Null THEN 0 ELSE 1 END ) AS BBB, " _
         &"Sum(CASE when DROPDAT is Null and rtcust.cusid is not null THEN 1 ELSE 0 END) AS CCC, " _
         &"case when RTCmty.COMCNT = 0 then 0 else Sum(CASE when DROPDAT is Null and rtcust.cusid is not null THEN 1 ELSE 0 END) " _
         &"* 100 / (RTCmty.COMCNT*1.0) end AS DDD ," _
         &"Sum(CASE when FINISHDAT is Null and dropdat is null and rtcust.cusid is not null THEN 1 ELSE 0 END) AS EEE, " _
         &"Sum(CASE when FINISHDAT is not Null and dropdat is null THEN 1 ELSE 0 END) AS FFF," _
         &"Sum(CASE when docketDAT is not Null and dropdat is null THEN 1 ELSE 0 END) AS GGG, RTcmty.T1PETITION," _
         &"RTcmty.T1Apply,RTcmty.COMTYPE,RTcmty.CUTID,RTcmty.TOWNSHIP " _
         &"FROM RTArea INNER JOIN RTAreaCty ON RTArea.AREAID = RTAreaCty.AREAID and RTArea.AREATYPE='1' and rtarea.areaid<>'*' " _
         &"RIGHT OUTER JOIN RTVCmtyGroup RIGHT OUTER JOIN RTCust RIGHT OUTER JOIN RTCmty ON RTCUST.COMQ1 = RTCMTY.COMQ1 " _
         &"ON RTVCmtyGroup.COMQ1 = RTCmty.COMQ1 ON RTAreaCty.CUTID = RTCmty.CUTID LEFT OUTER JOIN RTCounty ON " _
         &"RTCmty.CUTID = RTCounty.CUTID " _
         &"WHERE RTCmty.ComQ1<>0 AND RTCmty.T1APPLY IS NOT NULL AND RTCmty.RCOMDROP IS null AND RTCmty.CONNECTTYPE='04' AND " _
         &"RTCmty.ComQ1<>0 AND RTCmty.T1APPLY IS NOT NULL AND RTCmty.RCOMDROP IS null AND RTCmty.CONNECTTYPE='04' " _
         &"group by RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN,t1no,netip, RTCounty.CUTNC, RTCmty.COMCNT, RTCmty.APPLYCNT, " _
         &"RTCmty.T1PETITION, RTCmty.SCHDAT, RTCmty.T1APPLY,RTCMTY.COMTYPE,RTcmty.CUTID,RTcmty.TOWNSHIP ) A " _
         &"LEFT OUTER JOIN RTCUST B ON A.COMQ1=B.COMQ1 LEFT OUTER JOIN RTOBJ C ON B.CUSID=C.CUSID LEFT OUTER JOIN RTCOUNTY D " _
         &"ON B.CUTID1=D.CUTID LEFT OUTER JOIN RTCODE E ON A.COMTYPE=E.CODE AND E.KIND='B3' " _
         &"WHERE B.DOCKETDAT IS NOT NULL AND B.DROPDAT IS NULL ORDER BY A.COMN ,C.CUSNC "       
  dataTable="RTCmty"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="/WEBAP/RTAP/BASE/RTCMTY/RTCUSTD.asp"
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
    '讀取暫存KEY
  KEYXX=SPLIT(SESSION("search2"),";")
  searchProg="RTCMTYCHTCUST599ADSLS.ASP"
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" A.COMQ1<>0 "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  if len(trim(keyxx(0)))> 0 then
     searchQry2=" HAVING SUM(CASE WHEN RTCUST.DOCKETdat IS NOT NULL AND rtcust.DROPDAT IS NULL THEN 1 ELSE 0 END) " & KEYXX(0) & " " & KEYXX(1)
  else
     SEARCHQRY2=""
  END IF  
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  '讀取登入帳號之群組資料
  set connXX=server.CreateObject("ADODB.connection")
  set rsXX=server.CreateObject("ADODB.recordset")
  dsnxx="DSN=XXLIB"
  sqlxx="select * from usergroup where userid='" & Request.ServerVariables("LOGON_USER") & "'"
  connxx.Open dsnxx
  rsxx.Open sqlxx,connxx
  if not rsxx.EOF then
     usergroup=rsxx("group")
  else
     usergroup=""
  end if
  rsxx.Close
  connxx.Close
  set rsxx=nothing
  set connxx=nothing
  'Response.Write "GP=" & usergroup
  '-------------------------------------------------------------------------------------------
  'userlevel=2:為業務工程師==>只能看所屬社區資料
  'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
 ' Response.Write "DOMAIN=" & domain & "<BR>"
  Domain=Mid(Emply,1,1)
  select case Domain
         case "T"
            DAreaID="='A1'"
         case "C"
            DAreaID="='A2'"         
         case "K"
            DAreaID="='A3'"         
         case else
            DareaID="=''"
  end select
  '高階主管可讀取全部資料
  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or  Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="T90076" OR _
     Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T89076" or Ucase(emply)="T90089" then
     DAreaID="<>'*'"
  end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  sqlList="SELECT a.COMQ1,isnull(b.cusid,''),isnull(b.entryno,''),a.comn,isnull(c.cusnc,'')," _
         &"isnull(d.cutnc+b.township1+b.raddr1,'') as rrr,isnull(b.finishdat,''),isnull(b.docketdat,''),b.dropdat," _
         &"isnull(b.home,''),CASE WHEN ( e.CODENC = '' or e.CODENC is null ) THEN CASE WHEN  " _
         &"a.CUTID IN ('01', '02', '03', '04', '21', '22') AND a.township NOT IN ('三峽鎮', '鶯歌鎮') " _
         &"THEN '台北' WHEN a.cutid IN ('05', '06', '07', '08') OR ( a.cutid = '03' AND  a.township IN ('三峽鎮','鶯歌鎮')) " _
         &"THEN '桃園' WHEN  a.cutid IN ('09', '10', '11', '12', '13') THEN '台中' WHEN  a.cutid IN " _
         &"('14', '15', '16', '17', '18', '19', '20') THEN '高雄' ELSE '' END ELSE e.CODENC END " _
         &"FROM " _
         &"(SELECT RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN,t1no,netip, RTCounty.CUTNC, RTCmty.COMCNT, " _
         &"sum( CASE custype when '申裝戶' THEN 1 ELSE 0 end) HHH, sum(CASE custype when '深耕戶' THEN 1 ELSE 0 end) AS AAA, " _
         &"Sum(CASE custype when '' THEN 1 ELSE 0 end) AS JJJ, Sum(CASE when DROPDAT is Null THEN 0 ELSE 1 END ) AS BBB, " _
         &"Sum(CASE when DROPDAT is Null and rtcust.cusid is not null THEN 1 ELSE 0 END) AS CCC, " _
         &"case when RTCmty.COMCNT = 0 then 0 else Sum(CASE when DROPDAT is Null and rtcust.cusid is not null THEN 1 ELSE 0 END) " _
         &"* 100 / (RTCmty.COMCNT*1.0) end AS DDD ," _
         &"Sum(CASE when FINISHDAT is Null and dropdat is null and rtcust.cusid is not null THEN 1 ELSE 0 END) AS EEE, " _
         &"Sum(CASE when FINISHDAT is not Null and dropdat is null THEN 1 ELSE 0 END) AS FFF," _
         &"Sum(CASE when docketDAT is not Null and dropdat is null THEN 1 ELSE 0 END) AS GGG, RTcmty.T1PETITION," _
         &"RTcmty.T1Apply,RTcmty.COMTYPE,RTcmty.CUTID,RTcmty.TOWNSHIP " _
         &"FROM RTArea INNER JOIN RTAreaCty ON RTArea.AREAID = RTAreaCty.AREAID and RTArea.AREATYPE='1' and rtarea.areaid<>'*' " _
         &"RIGHT OUTER JOIN RTVCmtyGroup RIGHT OUTER JOIN RTCust RIGHT OUTER JOIN RTCmty ON RTCUST.COMQ1 = RTCMTY.COMQ1 " _
         &"ON RTVCmtyGroup.COMQ1 = RTCmty.COMQ1 ON RTAreaCty.CUTID = RTCmty.CUTID LEFT OUTER JOIN RTCounty ON " _
         &"RTCmty.CUTID = RTCounty.CUTID " _
         &"WHERE RTCmty.ComQ1<>0 AND RTCmty.T1APPLY IS NOT NULL AND RTCmty.RCOMDROP IS null AND RTCmty.CONNECTTYPE='04' AND " _
         &"RTCmty.ComQ1<>0 AND RTCmty.T1APPLY IS NOT NULL AND RTCmty.RCOMDROP IS null AND RTCmty.CONNECTTYPE='04' " _
         &"group by RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN,t1no,netip, RTCounty.CUTNC, RTCmty.COMCNT, RTCmty.APPLYCNT, " _
         &"RTCmty.T1PETITION, RTCmty.SCHDAT, RTCmty.T1APPLY,RTCMTY.COMTYPE,RTcmty.CUTID,RTcmty.TOWNSHIP " _
         &" " & searchqry2 & ") A " _
         &"LEFT OUTER JOIN RTCUST B ON A.COMQ1=B.COMQ1 LEFT OUTER JOIN RTOBJ C ON B.CUSID=C.CUSID LEFT OUTER JOIN RTCOUNTY D " _
         &"ON B.CUTID1=D.CUTID LEFT OUTER JOIN RTCODE E ON A.COMTYPE=E.CODE AND E.KIND='B3' " _
         &"WHERE B.DOCKETDAT IS NOT NULL AND B.DROPDAT IS NULL AND " &SEARCHQRY & " " _
         &"ORDER BY A.COMN ,C.CUSNC "       
    '   RESPONSE.Write SQLLIST
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