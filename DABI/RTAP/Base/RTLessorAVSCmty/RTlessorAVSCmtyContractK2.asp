<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="AVS-City管理系統"
  title="AVS-City社區合約資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  functionOptName="度數維護;續約作業;合約結案;結案返轉;作　　廢;作廢返轉;異動查詢"
  functionOptProgram="RTLessorAVSCmtyContractMonthlyScaleK.asp;RTLessorAVSCmtyContractContK.asp;RTLessorAVSCmtyContractF.asp;RTLessorAVSCmtyContractFR.asp;RTLessorAVSCmtyContractCancel.asp;RTLessorAVSCmtyContractCancelRTN.asp;RTLessorAVSCmtyContractLogK.asp"
  functionOptPrompt="N;N;Y;Y;Y;Y;N"
  functionoptopen="1;1;1;1;1;1;1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;合約編號;社區名稱;合約<BR>起算日;最近<BR>續約日;續約<BR>次數;合約<BR>到期日;簽約<BR>對象;none;none;none;none;none;none;電費<BR>補助種類;先付<BR後付;每月(度)<BR>金額;付款<BR>方式;付款<BR>週期;結案日;結案員;應收帳<BR>款編號;轉應收<BR>帳款日;作廢日;作廢員"
  sqlDelete="SELECT  RTLessorAVSCmtyContract.COMQ1, RTLessorAVSCmtyContract.CONTRACTNO, RTLessorAVSCmtyContract.STRDAT, " _
                &"RTLessorAVSCmtyContract.CONTDAT,RTLessorAVSCmtyContract.CONTCNT, RTLessorAVSCmtyContract.ENDDAT, " _
                &"RTLessorAVSCmtyContract.CONTRACTOBJ, RTLessorAVSCmtyContract.OBJTEL, RTLessorAVSCmtyContract.OBJMOBILE, " _
                &"RTDept.DEPTN4, RTObj_3.CUSNC, case when RTLessorAVSCmtyContract.REMITBANK1 <> '' then RTLessorAVSCmtyContract.REMITBANK1 else '' end + case when  RTLessorAVSCmtyContract.REMITBANK1 <> '' and RTLessorAVSCmtyContract.REMITBANK2<>'' then '-' + RTLessorAVSCmtyContract.REMITBANK2 else '' end + case when RTLessorAVSCmtyContract.REMITBANK1 <> '' or RTLessorAVSCmtyContract.REMITBANK2<>'' then '-' + RTLessorAVSCmtyContract.ACNO else '' end, " _
                &"RTLessorAVSCmtyContract.AC, RTCode_2.CODENC, RTCode_1.CODENC AS Expr1, RTLessorAVSCmtyContract.SCALEAMT, " _
                &"RTCode_3.CODENC AS Expr2,RTCode_4.CODENC AS Expr3,RTLessorAVSCmtyContract.CLOSEDAT,RTObj_2.CUSNC AS Expr4," _
                &"RTLessorAVSCmtyContract.CANCELDAT, RTObj_1.CUSNC AS Expr5 " _
                &"FROM    RTEmployee RTEmployee_3 LEFT OUTER JOIN RTObj RTObj_3 ON RTEmployee_3.CUSID = RTObj_3.CUSID " _
                &"RIGHT OUTER JOIN RTLessorAVSCmtyContract ON RTEmployee_3.EMPLY = RTLessorAVSCmtyContract.SIGNPERSON " _
                &"LEFT OUTER JOIN RTEmployee RTEmployee_1 LEFT OUTER JOIN RTObj RTObj_1 ON " _
                &"RTEmployee_1.CUSID = RTObj_1.CUSID ON RTLessorAVSCmtyContract.CANCELUSR = RTEmployee_1.EMPLY " _
                &"LEFT OUTER JOIN RTEmployee RTEmployee_2 LEFT OUTER JOIN RTObj RTObj_2 ON " _
                &"RTEmployee_2.CUSID = RTObj_2.CUSID ON RTLessorAVSCmtyContract.CLOSEUSR = RTEmployee_2.EMPLY LEFT OUTER JOIN " _
                &"RTCode RTCode_4 ON RTLessorAVSCmtyContract.PAYCYCLE = RTCode_4.CODE AND RTCode_4.KIND = 'F9' LEFT OUTER JOIN " _
                &"RTCode RTCode_3 ON RTLessorAVSCmtyContract.PAYKIND = RTCode_3.CODE AND RTCode_3.KIND = 'F5' LEFT OUTER JOIN " _
                &"RTCode RTCode_1 ON RTLessorAVSCmtyContract.POWERBILLPAYKIND = RTCode_1.CODE AND RTCode_1.KIND = 'O4' LEFT OUTER JOIN " _
                &"RTCode RTCode_2 ON RTLessorAVSCmtyContract.POWERBILLKIND = RTCode_2.CODE AND RTCode_2.KIND = 'O3' " _
                &"LEFT OUTER JOIN RTBankNo ON RTLessorAVSCmtyContract.REMITBANK1 = RTBankNo.HEADNO AND " _
                &"RTLessorAVSCmtyContract.REMITBANK2 = RTBankNo.BRANCHNO LEFT OUTER JOIN RTBank ON " _
                &"RTLessorAVSCmtyContract.REMITBANK1 = RTBank.HEADNO LEFT OUTER JOIN RTDept ON " _
                &"RTLessorAVSCmtyContract.SIGNDEPT = RTDept.DEPT " _
                &"WHERE RTLessorAVSCmtyContract.COMQ1=0"

  dataTable="RTLessorAVSCmtyContract"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="RTLessorAVSCmtyContractD.asp"
  datawindowFeature=""
  searchWindowFeature="width=300,height=210,scrollbars=yes"
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
  searchProg="RTLessorAVSCmtyContractS2.asp"
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
     searchQry=" RTLessorAVSCmtyContract.ComQ1<>0 AND RTLessorAVSCmtyContract.CANCELDAT IS NULL "
     searchShow="全部未作廢"
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
         sqlList="SELECT  RTLessorAVSCmtyContract.COMQ1, RTLessorAVSCmtyContract.CONTRACTNO,RTLessorAVSCmtyh.comn, RTLessorAVSCmtyContract.STRDAT, " _
                &"RTLessorAVSCmtyContract.CONTDAT,RTLessorAVSCmtyContract.CONTCNT, RTLessorAVSCmtyContract.ENDDAT, " _
                &"RTLessorAVSCmtyContract.CONTRACTOBJ, RTLessorAVSCmtyContract.OBJTEL, RTLessorAVSCmtyContract.OBJMOBILE, " _
                &"RTDept.DEPTN4, RTObj_3.CUSNC,  case when RTLessorAVSCmtyContract.REMITBANK1 <> '' then RTLessorAVSCmtyContract.REMITBANK1 else '' end + case when  RTLessorAVSCmtyContract.REMITBANK1 <> '' and RTLessorAVSCmtyContract.REMITBANK2<>'' then '-' + RTLessorAVSCmtyContract.REMITBANK2 else '' end + case when RTLessorAVSCmtyContract.REMITBANK1 <> '' or RTLessorAVSCmtyContract.REMITBANK2<>'' then '-' + RTLessorAVSCmtyContract.ACNO else '' end, " _
                &"RTLessorAVSCmtyContract.AC, RTCode_2.CODENC, RTCode_1.CODENC AS Expr1, RTLessorAVSCmtyContract.SCALEAMT, " _
                &"RTCode_3.CODENC AS Expr2,RTCode_4.CODENC AS Expr3,RTLessorAVSCmtyContract.CLOSEDAT,RTObj_2.CUSNC AS Expr4," _
                &"RTLessorAVSCmtyContract.CANCELDAT, RTObj_1.CUSNC AS Expr5,RTLessorAVSCmtyContract.batchno,RTLessorAVSCmtyContract.tardat " _
                &"FROM    RTEmployee RTEmployee_3 LEFT OUTER JOIN RTObj RTObj_3 ON RTEmployee_3.CUSID = RTObj_3.CUSID " _
                &"RIGHT OUTER JOIN RTLessorAVSCmtyContract ON RTEmployee_3.EMPLY = RTLessorAVSCmtyContract.SIGNPERSON " _
                &"LEFT OUTER JOIN RTEmployee RTEmployee_1 LEFT OUTER JOIN RTObj RTObj_1 ON " _
                &"RTEmployee_1.CUSID = RTObj_1.CUSID ON RTLessorAVSCmtyContract.CANCELUSR = RTEmployee_1.EMPLY " _
                &"LEFT OUTER JOIN RTEmployee RTEmployee_2 LEFT OUTER JOIN RTObj RTObj_2 ON " _
                &"RTEmployee_2.CUSID = RTObj_2.CUSID ON RTLessorAVSCmtyContract.CLOSEUSR = RTEmployee_2.EMPLY LEFT OUTER JOIN " _
                &"RTCode RTCode_4 ON RTLessorAVSCmtyContract.PAYCYCLE = RTCode_4.CODE AND RTCode_4.KIND = 'F9' LEFT OUTER JOIN " _
                &"RTCode RTCode_3 ON RTLessorAVSCmtyContract.PAYKIND = RTCode_3.CODE AND RTCode_3.KIND = 'F5' LEFT OUTER JOIN " _
                &"RTCode RTCode_1 ON RTLessorAVSCmtyContract.POWERBILLPAYKIND = RTCode_1.CODE AND RTCode_1.KIND = 'O4' LEFT OUTER JOIN " _
                &"RTCode RTCode_2 ON RTLessorAVSCmtyContract.POWERBILLKIND = RTCode_2.CODE AND RTCode_2.KIND = 'O3' " _
                &"LEFT OUTER JOIN RTBankNo ON RTLessorAVSCmtyContract.REMITBANK1 = RTBankNo.HEADNO AND " _
                &"RTLessorAVSCmtyContract.REMITBANK2 = RTBankNo.BRANCHNO LEFT OUTER JOIN RTBank ON " _
                &"RTLessorAVSCmtyContract.REMITBANK1 = RTBank.HEADNO LEFT OUTER JOIN RTDept ON " _
                &"RTLessorAVSCmtyContract.SIGNDEPT = RTDept.DEPT left outer join RTLessorAVSCmtyh on RTLessorAVSCmtyContract.comq1=RTLessorAVSCmtyh.comq1 " _
                &"WHERE RTLessorAVSCmtyContract.COMQ1<>0 AND " & SEARCHQRY & " ORDER BY RTLessorAVSCmtyContract.CONTRACTNO" 
                      

 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>