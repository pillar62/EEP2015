<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="ET-City管理系統"
  title="ET-City主線資料異動記錄查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;項次;主線;異動日期;異動別;異動員;鄉鎮;群組;主線IP;none;none;none;主線附掛;線路ISP;IP種類;主線速率;IP數;none;none;通測日;設備安裝;測通日;線路到期;到期天數;撤線日;作廢日;none;none;none;none;none"
  sqlDelete="SELECT RTLessorCmtyLineLog.COMQ1, RTLessorCmtyLineLog.LINEQ1, RTLessorCmtyLineLog.entryno,rtrim(convert(char(6),RTLessorCmtyLineLog.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorCmtyLineLog.lineQ1)) , " _
                &"RTLessorCmtyLineLog.chgdat,RTCODE_9.CODENC,''," _
                &"RTLessorCmtyLineLog.TOWNSHIP,RTLessorCmtyLineLog.LINEGROUP,RTLessorCmtyLineLog.LINEIP,RTLessorCmtyLineLog.GATEWAY, " _
                &"RTLessorCmtyLineLog.PPPOEACCOUNT, RTLessorCmtyLineLog.PPPOEPASSWORD, RTLessorCmtyLineLog.LINETEL, " _
                &"RTCode_1.CODENC, RTCode_3.CODENC AS Expr1, RTCode_2.CODENC AS Expr2, RTLessorCmtyLineLog.IPCNT, " _
                &"RTLessorCmtyLineLog.RCVDAT, RTLessorCmtyLineLog.INSPECTDAT, RTLessorCmtyLineLog.HINETNOTIFYDAT, " _
                &"RTLessorCmtyLineLog.HARDWAREDAT, RTLessorCmtyLineLog.ADSLAPPLYDAT, RTLessorCmtyLineLog.LINEDUEDAT, " _
                &"RTLessorCmtyLineLog.DROPDAT, RTLessorCmtyLineLog.CANCELDAT, " _
                &"SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL THEN 1 ELSE 0 END), " _
                &"SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL AND RTLessorCust.FINISHDAT IS NOT NULL THEN 1 ELSE 0 END), " _
                &"SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL AND RTLessorCust.FINISHDAT IS NOT NULL AND " _
                &"RTLessorCust.DROPDAT IS NOT NULL THEN 1 ELSE 0 END), SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL AND " _
                &"(RTLessorCust.STRBILLINGDAT IS NOT NULL OR RTLessorCust.NEWBILLINGDAT IS NOT NULL) AND " _
                &"RTLessorCust.DROPDAT IS NULL THEN 1 ELSE 0 END), SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL AND " _
                &"RTLessorCust.FINISHDAT IS NOT NULL THEN 1 ELSE 0 END) - SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL AND " _
                &"RTLessorCust.FINISHDAT IS NOT NULL AND RTLessorCust.DROPDAT IS NOT NULL THEN 1 ELSE 0 END) " _
                &"- SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL AND (RTLessorCust.STRBILLINGDAT IS NOT NULL OR " _
                &"RTLessorCust.NEWBILLINGDAT IS NOT NULL) AND RTLessorCust.DROPDAT IS NULL THEN 1 ELSE 0 END) " _
                &"FROM    RTLessorCmtyLineLog LEFT OUTER JOIN RTLessorCust ON RTLessorCmtyLineLog.COMQ1 = RTLessorCust.COMQ1 AND " _
                &"RTLessorCmtyLineLog.LINEQ1 = RTLessorCust.LINEQ1 LEFT OUTER JOIN RTCode RTCode_3 ON " _
                &"RTLessorCmtyLineLog.LINEIPTYPE = RTCode_3.CODE AND RTCode_3.KIND = 'M5' LEFT OUTER JOIN " _
                &"RTCode RTCode_1 ON RTLessorCmtyLineLog.LINEISP = RTCode_1.CODE AND RTCode_1.KIND = 'C3' LEFT OUTER JOIN " _
                &"RTCode RTCode_2 ON RTLessorCmtyLineLog.LINERATE = RTCode_2.CODE AND RTCode_2.KIND = 'D3' LEFT OUTER JOIN " _
                &"RTEmployee INNER JOIN RTObj RTObj_1 ON RTEmployee.CUSID = RTObj_1.CUSID ON " _
                &"RTLessorCmtyLineLog.SALESID = RTEmployee.EMPLY LEFT OUTER JOIN RTObj RTObj_2 ON " _
                &"RTLessorCmtyLineLog.CONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTLESSORCMTYH ON " _
                &"RTLessorCmtyLineLog.COMQ1 = RTLESSORCMTYH.COMQ1 LEFT OUTER JOIN RTCOUNTY ON " _
                &"RTLessorCmtyLineLog.CUTID = RTCOUNTY.CUTID " _
                &"WHERE RTLessorCmtyLineLog.COMQ1=0 " _
                &"GROUP BY  RTLessorCmtyLineLog.COMQ1, RTLessorCmtyLineLog.LINEQ1, RTLessorCmtyLineLog.entryno,rtrim(convert(char(6),RTLessorCmtyLineLog.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorCmtyLineLog.lineQ1)) ," _
                &"RTLessorCmtyLineLog.TOWNSHIP, RTLessorCmtyLineLog.LINEGROUP, RTLessorCmtyLineLog.LINEIP, " _
                &"RTLessorCmtyLineLog.GATEWAY, RTLessorCmtyLineLog.PPPOEACCOUNT, RTLessorCmtyLineLog.PPPOEPASSWORD, " _
                &"RTLessorCmtyLineLog.LINETEL, RTCode_1.CODENC, RTCode_3.CODENC, RTCode_2.CODENC, " _
                &"RTLessorCmtyLineLog.IPCNT, RTLessorCmtyLineLog.RCVDAT, RTLessorCmtyLineLog.INSPECTDAT, " _
                &"RTLessorCmtyLineLog.HINETNOTIFYDAT, RTLessorCmtyLineLog.HARDWAREDAT, RTLessorCmtyLineLog.ADSLAPPLYDAT, " _
                &"RTLessorCmtyLineLog.LINEDUEDAT, RTLessorCmtyLineLog.DROPDAT, RTLessorCmtyLineLog.CANCELDAT " _
                &"ORDER BY  RTLessorCmtyLineLog.COMQ1, RTLessorCmtyLineLog.LINEQ1 "

  dataTable="RTLessorCmtyLineLog"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="None"
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
  searchProg="self"
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
     searchQry=" RTLessorCmtyLineLog.ComQ1=" & aryparmkey(0) & " and RTLessorCmtyLineLog.lineq1=" & aryparmkey(1) & " "
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
         sqlList="SELECT RTLessorCmtyLineLog.COMQ1, RTLessorCmtyLineLog.LINEQ1, RTLessorCmtyLineLog.entryno,rtrim(convert(char(6),RTLessorCmtyLineLog.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorCmtyLineLog.lineQ1)) , " _
                &"RTLessorCmtyLineLog.chgdat,RTCODE_9.CODENC,rtobj_9.cusnc," _
                &"RTLessorCmtyLineLog.TOWNSHIP,RTLessorCmtyLineLog.LINEGROUP,RTLessorCmtyLineLog.LINEIP,RTLessorCmtyLineLog.GATEWAY, " _
                &"RTLessorCmtyLineLog.PPPOEACCOUNT, RTLessorCmtyLineLog.PPPOEPASSWORD, RTLessorCmtyLineLog.LINETEL, " _
                &"RTCode_1.CODENC, RTCode_3.CODENC AS Expr1, RTCode_2.CODENC AS Expr2, RTLessorCmtyLineLog.IPCNT, " _
                &"RTLessorCmtyLineLog.RCVDAT, RTLessorCmtyLineLog.INSPECTDAT, RTLessorCmtyLineLog.HINETNOTIFYDAT, " _
                &"RTLessorCmtyLineLog.HARDWAREDAT, RTLessorCmtyLineLog.ADSLAPPLYDAT, RTLessorCmtyLineLog.LINEDUEDAT, " _
                &"RTLessorCmtyLineLog.DROPDAT, RTLessorCmtyLineLog.CANCELDAT, " _
                &"SUM(CASE WHEN RTLessorCust.cusid is not null and RTLessorCust.CANCELDAT IS NULL THEN 1 ELSE 0 END), " _
                &"SUM(CASE WHEN RTLessorCust.cusid is not null and RTLessorCust.CANCELDAT IS NULL AND RTLessorCust.FINISHDAT IS NOT NULL THEN 1 ELSE 0 END), " _
                &"SUM(CASE WHEN RTLessorCust.cusid is not null and RTLessorCust.CANCELDAT IS NULL AND RTLessorCust.FINISHDAT IS NOT NULL AND " _
                &"RTLessorCust.DROPDAT IS NOT NULL THEN 1 ELSE 0 END), SUM(CASE  WHEN RTLessorCust.cusid is not null and RTLessorCust.CANCELDAT IS NULL AND " _
                &"(RTLessorCust.STRBILLINGDAT IS NOT NULL OR RTLessorCust.NEWBILLINGDAT IS NOT NULL) AND " _
                &"RTLessorCust.DROPDAT IS NULL THEN 1 ELSE 0 END), SUM(CASE WHEN RTLessorCust.cusid is not null and RTLessorCust.CANCELDAT IS NULL AND " _
                &"RTLessorCust.FINISHDAT IS NOT NULL THEN 1 ELSE 0 END) - SUM(CASE WHEN RTLessorCust.cusid is not null and RTLessorCust.CANCELDAT IS NULL AND " _
                &"RTLessorCust.FINISHDAT IS NOT NULL AND RTLessorCust.DROPDAT IS NOT NULL THEN 1 ELSE 0 END) " _
                &"- SUM(CASE WHEN RTLessorCust.cusid is not null and RTLessorCust.CANCELDAT IS NULL AND (RTLessorCust.STRBILLINGDAT IS NOT NULL OR " _
                &"RTLessorCust.NEWBILLINGDAT IS NOT NULL) AND RTLessorCust.DROPDAT IS NULL THEN 1 ELSE 0 END) " _
                &"FROM    RTLessorCmtyLineLog LEFT OUTER JOIN RTLessorCust ON RTLessorCmtyLineLog.COMQ1 = RTLessorCust.COMQ1 AND " _
                &"RTLessorCmtyLineLog.LINEQ1 = RTLessorCust.LINEQ1 LEFT OUTER JOIN RTCode RTCode_3 ON " _
                &"RTLessorCmtyLineLog.LINEIPTYPE = RTCode_3.CODE AND RTCode_3.KIND = 'M5' LEFT OUTER JOIN " _
                &"RTCode RTCode_1 ON RTLessorCmtyLineLog.LINEISP = RTCode_1.CODE AND RTCode_1.KIND = 'C3' LEFT OUTER JOIN " _
                &"RTCode RTCode_2 ON RTLessorCmtyLineLog.LINERATE = RTCode_2.CODE AND RTCode_2.KIND = 'D3' LEFT OUTER JOIN " _
                &"RTEmployee INNER JOIN RTObj RTObj_1 ON RTEmployee.CUSID = RTObj_1.CUSID ON " _
                &"RTLessorCmtyLineLog.SALESID = RTEmployee.EMPLY LEFT OUTER JOIN RTObj RTObj_2 ON " _
                &"RTLessorCmtyLineLog.CONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTLESSORCMTYH ON " _
                &"RTLessorCmtyLineLog.COMQ1 = RTLESSORCMTYH.COMQ1 LEFT OUTER JOIN RTCOUNTY ON " _
                &"RTLessorCmtyLineLog.CUTID = RTCOUNTY.CUTID left outer join rtcode rtcode_9 on RTLessorCmtyLineLog.chgcodE=rtcode_9.code and rtcode_9.kind='G2' " _
                &"LEFT OUTER JOIN rtemployee rtemployee_9 on RTLessorCmtyLineLog.chgusr=rtemployee.emply left outer join rtobj rtobj_9 on rtemployee_9.cusid=rtobj_9.cusid " _
                &"WHERE " & SEARCHQRY & " " _
                &"GROUP BY  RTLessorCmtyLineLog.COMQ1, RTLessorCmtyLineLog.LINEQ1, RTLessorCmtyLineLog.entryno,rtrim(convert(char(6),RTLessorCmtyLineLog.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorCmtyLineLog.lineQ1)) , " _
                &"RTLessorCmtyLineLog.chgdat,RTCODE_9.CODENC,rtobj_9.cusnc," _
                &"RTLessorCmtyLineLog.TOWNSHIP,RTLessorCmtyLineLog.LINEGROUP,RTLessorCmtyLineLog.LINEIP,RTLessorCmtyLineLog.GATEWAY, " _
                &"RTLessorCmtyLineLog.PPPOEACCOUNT, RTLessorCmtyLineLog.PPPOEPASSWORD, RTLessorCmtyLineLog.LINETEL, " _
                &"RTCode_1.CODENC, RTCode_3.CODENC, RTCode_2.CODENC, RTLessorCmtyLineLog.IPCNT, " _
                &"RTLessorCmtyLineLog.RCVDAT, RTLessorCmtyLineLog.INSPECTDAT, RTLessorCmtyLineLog.HINETNOTIFYDAT, " _
                &"RTLessorCmtyLineLog.HARDWAREDAT, RTLessorCmtyLineLog.ADSLAPPLYDAT, RTLessorCmtyLineLog.LINEDUEDAT, " _
                &"RTLessorCmtyLineLog.DROPDAT, RTLessorCmtyLineLog.CANCELDAT " _
                &"ORDER BY  RTLessorCmtyLineLog.COMQ1, RTLessorCmtyLineLog.LINEGROUP, RTLessorCmtyLineLog.LINEQ1 "
 
  'end if
  ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>