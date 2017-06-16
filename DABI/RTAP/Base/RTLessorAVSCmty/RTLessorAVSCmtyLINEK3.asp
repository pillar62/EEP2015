<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="AVS-City管理系統"
  title="AVS-City主線派工作業"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  functionOptName="主線派工;設備查詢;用戶查詢"
  functionOptProgram="RTLessorAVSCmtyLineSNDWORKK.asp;RTLessorAVSCmtyLINEHardwareK2.asp;RTLessorAVSCustK.asp"
  functionOptPrompt="N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;社區名稱;主線;鄉鎮;群組;主線IP;none;none;none;主線附掛;線路ISP;IP種類;主線速率;IP數;none;none;通測日;設備安裝;測通日;線路到期;到期天數;撤線日;作廢日;用戶;報竣;退租;計費;差異"
  sqlDelete="SELECT RTLessorAVSCmtyLine.COMQ1, RTLessorAVSCmtyLine.LINEQ1,RTLessorAVSCMTYH.COMN,rtrim(convert(char(6),RTLessorAVSCmtyLine.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorAVSCmtyLine.lineQ1)) , " _
                &"RTLessorAVSCMTYLINE.TOWNSHIP,RTLessorAVSCmtyLine.LINEGROUP,RTLessorAVSCmtyLine.LINEIP,RTLessorAVSCmtyLine.GATEWAY, " _
                &"RTLessorAVSCmtyLine.PPPOEACCOUNT, RTLessorAVSCmtyLine.PPPOEPASSWORD, RTLessorAVSCmtyLine.LINETEL, " _
                &"RTCode_1.CODENC, RTCode_3.CODENC AS Expr1, RTCode_2.CODENC AS Expr2, RTLessorAVSCmtyLine.IPCNT, " _
                &"RTLessorAVSCmtyLine.RCVDAT, RTLessorAVSCmtyLine.INSPECTDAT, RTLessorAVSCmtyLine.HINETNOTIFYDAT, " _
                &"RTLessorAVSCmtyLine.HARDWAREDAT, RTLessorAVSCmtyLine.ADSLAPPLYDAT, RTLessorAVSCmtyLine.LINEDUEDAT,isnull(datediff(dd,getdate(),RTLessorAVSCmtyLine.LINEDUEDAT),0), " _
                &"RTLessorAVSCmtyLine.DROPDAT, RTLessorAVSCmtyLine.CANCELDAT, " _
                &"SUM(CASE WHEN RTLessorAVSCust.CANCELDAT IS NULL THEN 1 ELSE 0 END), " _
                &"SUM(CASE WHEN RTLessorAVSCust.CANCELDAT IS NULL AND RTLessorAVSCust.FINISHDAT IS NOT NULL THEN 1 ELSE 0 END), " _
                &"SUM(CASE WHEN RTLessorAVSCust.CANCELDAT IS NULL AND RTLessorAVSCust.FINISHDAT IS NOT NULL AND " _
                &"RTLessorAVSCust.DROPDAT IS NOT NULL THEN 1 ELSE 0 END), SUM(CASE WHEN RTLessorAVSCust.CANCELDAT IS NULL AND " _
                &"(RTLessorAVSCust.STRBILLINGDAT IS NOT NULL OR RTLessorAVSCust.NEWBILLINGDAT IS NOT NULL) AND " _
                &"RTLessorAVSCust.DROPDAT IS NULL THEN 1 ELSE 0 END), SUM(CASE WHEN RTLessorAVSCust.CANCELDAT IS NULL AND " _
                &"RTLessorAVSCust.FINISHDAT IS NOT NULL THEN 1 ELSE 0 END) - SUM(CASE WHEN RTLessorAVSCust.CANCELDAT IS NULL AND " _
                &"RTLessorAVSCust.FINISHDAT IS NOT NULL AND RTLessorAVSCust.DROPDAT IS NOT NULL THEN 1 ELSE 0 END) " _
                &"- SUM(CASE WHEN RTLessorAVSCust.CANCELDAT IS NULL AND (RTLessorAVSCust.STRBILLINGDAT IS NOT NULL OR " _
                &"RTLessorAVSCust.NEWBILLINGDAT IS NOT NULL) AND RTLessorAVSCust.DROPDAT IS NULL THEN 1 ELSE 0 END) " _
                &"FROM    RTLessorAVSCmtyLine LEFT OUTER JOIN RTLessorAVSCust ON RTLessorAVSCmtyLine.COMQ1 = RTLessorAVSCust.COMQ1 AND " _
                &"RTLessorAVSCmtyLine.LINEQ1 = RTLessorAVSCust.LINEQ1 LEFT OUTER JOIN RTCode RTCode_3 ON " _
                &"RTLessorAVSCmtyLine.LINEIPTYPE = RTCode_3.CODE AND RTCode_3.KIND = 'M5' LEFT OUTER JOIN " _
                &"RTCode RTCode_1 ON RTLessorAVSCmtyLine.LINEISP = RTCode_1.CODE AND RTCode_1.KIND = 'C3' LEFT OUTER JOIN " _
                &"RTCode RTCode_2 ON RTLessorAVSCmtyLine.LINERATE = RTCode_2.CODE AND RTCode_2.KIND = 'D3' LEFT OUTER JOIN " _
                &"RTEmployee INNER JOIN RTObj RTObj_1 ON RTEmployee.CUSID = RTObj_1.CUSID ON " _
                &"RTLessorAVSCmtyLine.SALESID = RTEmployee.EMPLY LEFT OUTER JOIN RTObj RTObj_2 ON " _
                &"RTLessorAVSCmtyLine.CONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTLessorAVSCMTYH ON " _
                &"RTLessorAVSCmtyLine.COMQ1 = RTLessorAVSCMTYH.COMQ1 LEFT OUTER JOIN RTCOUNTY ON " _
                &"RTLessorAVSCMTYLINE.CUTID = RTCOUNTY.CUTID " _
                &"WHERE RTLessorAVSCmtyLine.COMQ1=0 " _
                &"GROUP BY  RTLessorAVSCmtyLine.COMQ1, RTLessorAVSCmtyLine.LINEQ1, RTLessorAVSCMTYH.COMN,rtrim(convert(char(6),RTLessorAVSCmtyLine.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorAVSCmtyLine.lineQ1)) ," _
                &"RTLessorAVSCMTYLINE.TOWNSHIP, RTLessorAVSCmtyLine.LINEGROUP, RTLessorAVSCmtyLine.LINEIP, " _
                &"RTLessorAVSCmtyLine.GATEWAY, RTLessorAVSCmtyLine.PPPOEACCOUNT, RTLessorAVSCmtyLine.PPPOEPASSWORD, " _
                &"RTLessorAVSCmtyLine.LINETEL, RTCode_1.CODENC, RTCode_3.CODENC, RTCode_2.CODENC, " _
                &"RTLessorAVSCmtyLine.IPCNT, RTLessorAVSCmtyLine.RCVDAT, RTLessorAVSCmtyLine.INSPECTDAT, " _
                &"RTLessorAVSCmtyLine.HINETNOTIFYDAT, RTLessorAVSCmtyLine.HARDWAREDAT, RTLessorAVSCmtyLine.ADSLAPPLYDAT, " _
                &"RTLessorAVSCmtyLine.LINEDUEDAT, RTLessorAVSCmtyLine.DROPDAT, RTLessorAVSCmtyLine.CANCELDAT " _
                &"ORDER BY  RTLessorAVSCmtyLine.COMQ1, RTLessorAVSCmtyLine.LINEQ1 "

  dataTable="RTLessorAVSCmtyLine"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="RTLessorAVSCmtyLineD.asp"
  datawindowFeature=""
  searchWindowFeature="width=450,height=350,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="RTLessorAVSCmtylineS2.asp"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
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
  '----
 
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTLessorAVSCmtyLine.ComQ1<>0 "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  '讀取登入帳號之群組資料
  'Response.Write "GP=" & usergroup
  '-------------------------------------------------------------------------------------------
  'userlevel=2:為業務工程師==>只能看所屬社區資料
  'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
 ' Response.Write "DOMAIN=" & domain & "<BR>"
  Domain=Mid(Emply,1,1)
  select case Domain
         case "T"
            DAreaID="<>'*'"
         case "P"
            DAreaID="='A1'"                        
         case "C"
            DAreaID="='A2'"         
         case "K"
            DAreaID="='A3'"         
         case else
            DareaID="=''"
  end select
  '高階主管可讀取全部資料
  '  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or Ucase(emply)="T89003" or _
  '	 Ucase(emply)="T89018" or Ucase(emply)="T89020" or Ucase(emply)="T89025" or Ucase(emply)="T91099" or _
  '	 Ucase(emply)="T92134" or Ucase(emply)="T93168" or Ucase(emply)="T93177" or Ucase(emply)="T94180" then
  '   DAreaID="<>'*'"
  'end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  if userlevel=31 or userlevel =1  or userlevel =5 then DAreaID="<>'*'"
         sqlList="SELECT RTLessorAVSCmtyLine.COMQ1, RTLessorAVSCmtyLine.LINEQ1,RTLessorAVSCMTYH.COMN,rtrim(convert(char(6),RTLessorAVSCmtyLine.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorAVSCmtyLine.lineQ1)) , " _
                &"RTLessorAVSCMTYLINE.TOWNSHIP,RTLessorAVSCmtyLine.LINEGROUP,RTLessorAVSCmtyLine.LINEIP,RTLessorAVSCmtyLine.GATEWAY, " _
                &"RTLessorAVSCmtyLine.PPPOEACCOUNT, RTLessorAVSCmtyLine.PPPOEPASSWORD, RTLessorAVSCmtyLine.LINETEL, " _
                &"RTCode_1.CODENC, RTCode_3.CODENC AS Expr1, RTCode_2.CODENC AS Expr2, RTLessorAVSCmtyLine.IPCNT, " _
                &"RTLessorAVSCmtyLine.RCVDAT, RTLessorAVSCmtyLine.INSPECTDAT, RTLessorAVSCmtyLine.HINETNOTIFYDAT, " _
                &"RTLessorAVSCmtyLine.HARDWAREDAT, RTLessorAVSCmtyLine.ADSLAPPLYDAT, RTLessorAVSCmtyLine.LINEDUEDAT,isnull(datediff(dd,getdate(),RTLessorAVSCmtyLine.LINEDUEDAT),0), " _
                &"RTLessorAVSCmtyLine.DROPDAT, RTLessorAVSCmtyLine.CANCELDAT, " _
                &"SUM(CASE WHEN RTLessorAVSCust.cusid is not null and RTLessorAVSCust.CANCELDAT IS NULL THEN 1 ELSE 0 END), " _
                &"SUM(CASE WHEN RTLessorAVSCust.cusid is not null and RTLessorAVSCust.CANCELDAT IS NULL AND RTLessorAVSCust.FINISHDAT IS NOT NULL THEN 1 ELSE 0 END), " _
                &"SUM(CASE WHEN RTLessorAVSCust.cusid is not null and RTLessorAVSCust.CANCELDAT IS NULL AND RTLessorAVSCust.FINISHDAT IS NOT NULL AND " _
                &"RTLessorAVSCust.DROPDAT IS NOT NULL THEN 1 ELSE 0 END), SUM(CASE  WHEN RTLessorAVSCust.cusid is not null and RTLessorAVSCust.CANCELDAT IS NULL AND " _
                &"(RTLessorAVSCust.STRBILLINGDAT IS NOT NULL OR RTLessorAVSCust.NEWBILLINGDAT IS NOT NULL) AND " _
                &"RTLessorAVSCust.DROPDAT IS NULL THEN 1 ELSE 0 END), SUM(CASE WHEN RTLessorAVSCust.cusid is not null and RTLessorAVSCust.CANCELDAT IS NULL AND " _
                &"RTLessorAVSCust.FINISHDAT IS NOT NULL THEN 1 ELSE 0 END) - SUM(CASE WHEN RTLessorAVSCust.cusid is not null and RTLessorAVSCust.CANCELDAT IS NULL AND " _
                &"RTLessorAVSCust.FINISHDAT IS NOT NULL AND RTLessorAVSCust.DROPDAT IS NOT NULL THEN 1 ELSE 0 END) " _
                &"- SUM(CASE WHEN RTLessorAVSCust.cusid is not null and RTLessorAVSCust.CANCELDAT IS NULL AND (RTLessorAVSCust.STRBILLINGDAT IS NOT NULL OR " _
                &"RTLessorAVSCust.NEWBILLINGDAT IS NOT NULL) AND RTLessorAVSCust.DROPDAT IS NULL THEN 1 ELSE 0 END) " _
                &"FROM    RTLessorAVSCmtyLine LEFT OUTER JOIN RTLessorAVSCust ON RTLessorAVSCmtyLine.COMQ1 = RTLessorAVSCust.COMQ1 AND " _
                &"RTLessorAVSCmtyLine.LINEQ1 = RTLessorAVSCust.LINEQ1 LEFT OUTER JOIN RTCode RTCode_3 ON " _
                &"RTLessorAVSCmtyLine.LINEIPTYPE = RTCode_3.CODE AND RTCode_3.KIND = 'M5' LEFT OUTER JOIN " _
                &"RTCode RTCode_1 ON RTLessorAVSCmtyLine.LINEISP = RTCode_1.CODE AND RTCode_1.KIND = 'C3' LEFT OUTER JOIN " _
                &"RTCode RTCode_2 ON RTLessorAVSCmtyLine.LINERATE = RTCode_2.CODE AND RTCode_2.KIND = 'D3' LEFT OUTER JOIN " _
                &"RTEmployee INNER JOIN RTObj RTObj_1 ON RTEmployee.CUSID = RTObj_1.CUSID ON " _
                &"RTLessorAVSCmtyLine.SALESID = RTEmployee.EMPLY LEFT OUTER JOIN RTObj RTObj_2 ON " _
                &"RTLessorAVSCmtyLine.CONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTLessorAVSCMTYH ON " _
                &"RTLessorAVSCmtyLine.COMQ1 = RTLessorAVSCMTYH.COMQ1 LEFT OUTER JOIN RTCOUNTY ON " _
                &"RTLessorAVSCMTYLINE.CUTID = RTCOUNTY.CUTID " _
                &"WHERE RTLessorAVSCmtyLine.ADSLAPPLYDAT is null and RTLessorAVSCmtyLine.LINEIP <> '' and " & SEARCHQRY & " " _
                &"GROUP BY  RTLessorAVSCmtyLine.COMQ1, RTLessorAVSCmtyLine.LINEQ1, RTLessorAVSCMTYH.COMN,rtrim(convert(char(6),RTLessorAVSCmtyLine.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorAVSCmtyLine.lineQ1)) ,  " _
                &"RTLessorAVSCMTYLINE.TOWNSHIP, RTLessorAVSCmtyLine.LINEGROUP, RTLessorAVSCmtyLine.LINEIP, " _
                &"RTLessorAVSCmtyLine.GATEWAY, RTLessorAVSCmtyLine.PPPOEACCOUNT, RTLessorAVSCmtyLine.PPPOEPASSWORD, " _
                &"RTLessorAVSCmtyLine.LINETEL, RTCode_1.CODENC, RTCode_3.CODENC, RTCode_2.CODENC, " _
                &"RTLessorAVSCmtyLine.IPCNT, RTLessorAVSCmtyLine.RCVDAT, RTLessorAVSCmtyLine.INSPECTDAT, " _
                &"RTLessorAVSCmtyLine.HINETNOTIFYDAT, RTLessorAVSCmtyLine.HARDWAREDAT, RTLessorAVSCmtyLine.ADSLAPPLYDAT, " _
                &"RTLessorAVSCmtyLine.LINEDUEDAT, RTLessorAVSCmtyLine.DROPDAT, RTLessorAVSCmtyLine.CANCELDAT " _
                &"ORDER BY  RTLessorAVSCmtyLine.COMQ1, RTLessorAVSCmtyLine.LINEGROUP, RTLessorAVSCmtyLine.LINEQ1 "
 
  'end if
 'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>