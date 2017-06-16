<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="ET-City管理系統"
  title="ET-City社區合約資料異動查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""
  functionoptopen=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;異動<BR>項次;異動日;異動別;異動員;合約<BR>起算日;最近<BR>續約日;續約<BR>次數;合約<BR>到期日;簽約<BR對象;none;none;簽約部門;簽約人;none;none;電費<BR>補助種類;先付<BR後付;每月(度)<BR>金額;付款<BR>方式;付款<BR>週期;結案日;結案員;作廢日;作廢員"
  sqlDelete="SELECT  RTLessorCmtyContractLog.COMQ1, RTLessorCmtyContractLog.CONTRACTNO, RTLessorCmtyContractLog.STRDAT, " _
                &"RTLessorCmtyContractLog.CONTDAT,RTLessorCmtyContractLog.CONTCNT, RTLessorCmtyContractLog.ENDDAT, " _
                &"RTLessorCmtyContractLog.CONTRACTOBJ, RTLessorCmtyContractLog.OBJTEL, RTLessorCmtyContractLog.OBJMOBILE, " _
                &"RTDept.DEPTN4, RTObj_3.CUSNC, case when RTLessorCmtyContractLog.REMITBANK1 <> '' then RTLessorCmtyContractLog.REMITBANK1 else '' end + case when  RTLessorCmtyContractLog.REMITBANK1 <> '' and RTLessorCmtyContractLog.REMITBANK2<>'' then '-' + RTLessorCmtyContractLog.REMITBANK2 else '' end + case when RTLessorCmtyContractLog.REMITBANK1 <> '' or RTLessorCmtyContractLog.REMITBANK2<>'' then '-' + RTLessorCmtyContractLog.ACNO else '' end, " _
                &"RTLessorCmtyContractLog.AC, RTCode_2.CODENC, RTCode_1.CODENC AS Expr1, RTLessorCmtyContractLog.SCALEAMT, " _
                &"RTCode_3.CODENC AS Expr2,RTCode_4.CODENC AS Expr3,RTLessorCmtyContractLog.CLOSEDAT,RTObj_2.CUSNC AS Expr4," _
                &"RTLessorCmtyContractLog.CANCELDAT, RTObj_1.CUSNC AS Expr5 " _
                &"FROM    RTEmployee RTEmployee_3 LEFT OUTER JOIN RTObj RTObj_3 ON RTEmployee_3.CUSID = RTObj_3.CUSID " _
                &"RIGHT OUTER JOIN RTLessorCmtyContractLog ON RTEmployee_3.EMPLY = RTLessorCmtyContractLog.SIGNPERSON " _
                &"LEFT OUTER JOIN RTEmployee RTEmployee_1 LEFT OUTER JOIN RTObj RTObj_1 ON " _
                &"RTEmployee_1.CUSID = RTObj_1.CUSID ON RTLessorCmtyContractLog.CANCELUSR = RTEmployee_1.EMPLY " _
                &"LEFT OUTER JOIN RTEmployee RTEmployee_2 LEFT OUTER JOIN RTObj RTObj_2 ON " _
                &"RTEmployee_2.CUSID = RTObj_2.CUSID ON RTLessorCmtyContractLog.CLOSEUSR = RTEmployee_2.EMPLY LEFT OUTER JOIN " _
                &"RTCode RTCode_4 ON RTLessorCmtyContractLog.PAYCYCLE = RTCode_4.CODE AND RTCode_4.KIND = 'F9' LEFT OUTER JOIN " _
                &"RTCode RTCode_3 ON RTLessorCmtyContractLog.PAYKIND = RTCode_3.CODE AND RTCode_3.KIND = 'F5' LEFT OUTER JOIN " _
                &"RTCode RTCode_1 ON RTLessorCmtyContractLog.POWERBILLPAYKIND = RTCode_1.CODE AND RTCode_1.KIND = 'O4' LEFT OUTER JOIN " _
                &"RTCode RTCode_2 ON RTLessorCmtyContractLog.POWERBILLKIND = RTCode_2.CODE AND RTCode_2.KIND = 'O3' " _
                &"LEFT OUTER JOIN RTBankNo ON RTLessorCmtyContractLog.REMITBANK1 = RTBankNo.HEADNO AND " _
                &"RTLessorCmtyContractLog.REMITBANK2 = RTBankNo.BRANCHNO LEFT OUTER JOIN RTBank ON " _
                &"RTLessorCmtyContractLog.REMITBANK1 = RTBank.HEADNO LEFT OUTER JOIN RTDept ON " _
                &"RTLessorCmtyContractLog.SIGNDEPT = RTDept.DEPT " _
                &"WHERE RTLessorCmtyContractLog.COMQ1=0"

  dataTable="RTLessorCmtyContractLog"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=300,height=160,scrollbars=yes"
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
  set connYY=server.CreateObject("ADODB.connection")
  set rsYY=server.CreateObject("ADODB.recordset")
  dsnYY="DSN=RTLIB"
  sqlYY="select * from RTLessorCmtyH LEFT OUTER JOIN RTCOUNTY ON RTLessorCmtyH.CUTID=RTCOUNTY.CUTID where COMQ1=" & ARYPARMKEY(0)
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
     COMADDR=RSYY("CUTNC") & RSYY("TOWNSHIP") & RSYY("RADDR")
  else
     COMN=""
     COMADDR=""
  end if
  rsYY.Close
  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTLessorCmtyContractLog.ComQ1=" & aryparmkey(0) & " and RTLessorCmtyContractLog.contractno='" & aryparmkey(1) & "'"
     searchShow="社區序號︰"& aryparmkey(0) & ",社區名稱︰" & COMN & ",社區地址︰" & COMADDR
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
         sqlList="SELECT  RTLessorCmtyContractLog.COMQ1, RTLessorCmtyContractLog.CONTRACTNO, RTLessorCmtyContractLog.ENTRYNO,RTLessorCmtyContractLog.CHGDAT,RTCODE_8.CODENC,RTOBJ_8.CUSNC, RTLessorCmtyContractLog.STRDAT, " _
                &"RTLessorCmtyContractLog.CONTDAT,RTLessorCmtyContractLog.CONTCNT, RTLessorCmtyContractLog.ENDDAT, " _
                &"RTLessorCmtyContractLog.CONTRACTOBJ, RTLessorCmtyContractLog.OBJTEL, RTLessorCmtyContractLog.OBJMOBILE, " _
                &"RTDept.DEPTN4, RTObj_3.CUSNC,  case when RTLessorCmtyContractLog.REMITBANK1 <> '' then RTLessorCmtyContractLog.REMITBANK1 else '' end + case when  RTLessorCmtyContractLog.REMITBANK1 <> '' and RTLessorCmtyContractLog.REMITBANK2<>'' then '-' + RTLessorCmtyContractLog.REMITBANK2 else '' end + case when RTLessorCmtyContractLog.REMITBANK1 <> '' or RTLessorCmtyContractLog.REMITBANK2<>'' then '-' + RTLessorCmtyContractLog.ACNO else '' end, " _
                &"RTLessorCmtyContractLog.AC, RTCode_2.CODENC, RTCode_1.CODENC AS Expr1, RTLessorCmtyContractLog.SCALEAMT, " _
                &"RTCode_3.CODENC AS Expr2,RTCode_4.CODENC AS Expr3,RTLessorCmtyContractLog.CLOSEDAT,RTObj_2.CUSNC AS Expr4," _
                &"RTLessorCmtyContractLog.CANCELDAT, RTObj_1.CUSNC AS Expr5 " _
                &"FROM    RTEmployee RTEmployee_3 LEFT OUTER JOIN RTObj RTObj_3 ON RTEmployee_3.CUSID = RTObj_3.CUSID " _
                &"RIGHT OUTER JOIN RTLessorCmtyContractLog ON RTEmployee_3.EMPLY = RTLessorCmtyContractLog.SIGNPERSON " _
                &"LEFT OUTER JOIN RTEmployee RTEmployee_1 LEFT OUTER JOIN RTObj RTObj_1 ON " _
                &"RTEmployee_1.CUSID = RTObj_1.CUSID ON RTLessorCmtyContractLog.CANCELUSR = RTEmployee_1.EMPLY " _
                &"LEFT OUTER JOIN RTEmployee RTEmployee_2 LEFT OUTER JOIN RTObj RTObj_2 ON " _
                &"RTEmployee_2.CUSID = RTObj_2.CUSID ON RTLessorCmtyContractLog.CLOSEUSR = RTEmployee_2.EMPLY LEFT OUTER JOIN " _
                &"RTCode RTCode_4 ON RTLessorCmtyContractLog.PAYCYCLE = RTCode_4.CODE AND RTCode_4.KIND = 'F9' LEFT OUTER JOIN " _
                &"RTCode RTCode_3 ON RTLessorCmtyContractLog.PAYKIND = RTCode_3.CODE AND RTCode_3.KIND = 'F5' LEFT OUTER JOIN " _
                &"RTCode RTCode_1 ON RTLessorCmtyContractLog.POWERBILLPAYKIND = RTCode_1.CODE AND RTCode_1.KIND = 'O4' LEFT OUTER JOIN " _
                &"RTCode RTCode_2 ON RTLessorCmtyContractLog.POWERBILLKIND = RTCode_2.CODE AND RTCode_2.KIND = 'O3' " _
                &"LEFT OUTER JOIN RTBankNo ON RTLessorCmtyContractLog.REMITBANK1 = RTBankNo.HEADNO AND " _
                &"RTLessorCmtyContractLog.REMITBANK2 = RTBankNo.BRANCHNO LEFT OUTER JOIN RTBank ON " _
                &"RTLessorCmtyContractLog.REMITBANK1 = RTBank.HEADNO LEFT OUTER JOIN RTDept ON " _
                &"RTLessorCmtyContractLog.SIGNDEPT = RTDept.DEPT left outer join rtcode rtcode_8 on RTLessorCmtyContractLog.chgcode=rtcode_8.code and rtcode_8.kind='G2' " _
                &"LEFT OUTER JOIN RTEMPLOYEE RTEMPLOYEE_8 ON RTLessorCmtyContractLog.CHGUSR=RTEMPLOYEE_8.EMPLY LEFT OUTER JOIN RTOBJ RTOBJ_8 ON RTEMPLOYEE_8.CUSID=RTOBJ_8.CUSID " _
                &"WHERE RTLessorCmtyContractLog.COMQ1=" & ARYPARMKEY(0) & " AND RTLessorCmtyContractLog.contractno='" & ARYPARMKEY(1) & "'"
                      

 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>