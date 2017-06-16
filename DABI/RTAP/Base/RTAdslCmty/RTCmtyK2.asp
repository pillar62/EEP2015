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
  functionOptName="客　戶;加入社區"
  functionOptProgram="RTCustK.asp;RTJOINcmtycfm.ASP"
  functionOptPrompt="N;N;N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="序號;社區名稱;HB號碼;IP;設備位置;ADSL測通日;總戶數;已完工;己報峻" 
  sqlDelete="SELECT RTCustAdslCmty.CUTYID, RTCustAdslCmty.COMN, RTCustAdslCmty.HBNO, " _
           &"RTCustAdslCmty.IPADDR, RTCustAdslCmty.EQUIPADDR, RTCustAdslCmty.ADSLAPPLY, " _
           &"SUM(CASE WHEN rtcustadsl.cusid IS NOT NULL OR rtcustadsl.CUSID <> '' THEN 1 ELSE 0 END), " _
           &"SUM(CASE WHEN rtcustadsl.finishdat IS NOT NULL OR rtcustadsl.finishdat <> '' THEN 1 ELSE 0 END), " _           
           &"SUM(CASE WHEN rtcustadsl.docketdat IS NOT NULL OR rtcustadsl.docketdat <> '' THEN 1 ELSE 0 END) " _
           &"FROM RTCustAdslCmty LEFT OUTER JOIN RTCustADSL ON RTCustAdslCmty.CUTYID = RTCustADSL.COMQ1 " _
           &"WHERE (RTCustAdslCmty.COMN <> '*') " _
           &"GROUP BY  RTCustAdslCmty.CUTYID, RTCustAdslCmty.COMN, RTCustAdslCmty.HBNO, " _
           &"RTCustAdslCmty.IPADDR, RTCustAdslCmty.EQUIPADDR, " _
           &"RTCustAdslCmty.ADSLAPPLY " _
           &"ORDER BY  RTCustAdslCmty.equipaddr "
  dataTable="RTCUSTADSLCmty"
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
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20
  searchProg="RTCmtyS.asp"
' Open search program when first entry this keylist
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=true
  If searchQry="" Then
     searchQry=" RTCUSTADSLCMTY.CUTYID<>0 "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
'  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
'  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  '讀取登入帳號之群組資料
'  set connXX=server.CreateObject("ADODB.connection")
'  set rsXX=server.CreateObject("ADODB.recordset")
'  dsnxx="DSN=XXLIB"
'  sqlxx="select * from usergroup where userid='" & Request.ServerVariables("LOGON_USER") & "'"
'  connxx.Open dsnxx
'  rsxx.Open sqlxx,connxx
'  if not rsxx.EOF then
'     usergroup=rsxx("group")
'  else
'     usergroup=""
'  end if
'  rsxx.Close
'  connxx.Close
'  set rsxx=nothing
'  set connxx=nothing
  'Response.Write "GP=" & usergroup
  '-------------------------------------------------------------------------------------------
  'userlevel=2:為業務工程師==>只能看所屬社區資料
  'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
 ' Response.Write "DOMAIN=" & domain & "<BR>"
'  Domain=Mid(Emply,1,1)
'  select case Domain
'         case "T"
'            DAreaID="='A1'"
'         case "C"
'            DAreaID="='A2'"         
'         case "K"
'            DAreaID="='A3'"         
''         case else
'            DareaID="=''"
 ' end select
  '高階主管可讀取全部資料
 ' if UCASE(emply)="T89001" or Ucase(emply)="T89002" or  Ucase(emply)="T89020" or Ucase(emply)="T89018" OR _
 '    Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="P92010" then
 '    DAreaID="<>'*'"
 ' end if
  '資訊部管理員可讀取全部資料
 ' if userlevel=31 then DAreaID="<>'*'"
  '業務工程師只能讀取該工程師的社區
'
sqllist="SELECT RTCustAdslCmty.CUTYID, RTCustAdslCmty.COMN, RTCustAdslCmty.HBNO, " _
       &"RTCustAdslCmty.IPADDR, RTCustAdslCmty.EQUIPADDR, RTCustAdslCmty.ADSLAPPLY, " _
       &"SUM(CASE WHEN rtcustadsl.cusid IS NOT NULL OR rtcustadsl.cusid <> '' THEN 1 ELSE 0 END), " _ 
       &"SUM(CASE WHEN rtcustadsl.finishdat IS NOT NULL OR rtcustadsl.finishdat <> '' THEN 1 ELSE 0 END), " _           
       &"SUM(CASE WHEN rtcustadsl.docketdat IS NOT NULL OR rtcustadsl.docketdat <> '' THEN 1 ELSE 0 END) " _     
       &"FROM RTCustAdslCmty LEFT OUTER JOIN RTCustADSL ON RTCustAdslCmty.CUTYID = RTCustADSL.COMQ1 " _
       &"WHERE (RTCustAdslCmty.COMN <> '*') and " & searchqry _
       &"GROUP BY  RTCustAdslCmty.CUTYID, RTCustAdslCmty.COMN, RTCustAdslCmty.HBNO, " _
       &"RTCustAdslCmty.IPADDR, RTCustAdslCmty.EQUIPADDR, " _
       &"RTCustAdslCmty.ADSLAPPLY " _
       &"ORDER BY  RTCustAdslCmty.equipaddr "
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>