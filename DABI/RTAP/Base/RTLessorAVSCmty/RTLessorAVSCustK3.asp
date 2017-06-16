<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="AVS-City管理系統"
  title="AVS-City用戶資料維護(客服)"
  buttonName=" 新  增 ; 刪  除 ; 結  束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="客服案件;歷史異動"
  functionOptProgram="RTLessorAVSCustfaqK.asp;RTLessorAVSCustLOGK.asp"
  functionOptPrompt="N;N"
  functionoptopen="1;1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;主線;社區;用戶;週期;繳款;none;none;連絡電話;申請日;完工日;開始計費;最近<br>續約日;調整<br>日數;到期日;公關;退租日;作廢日;none;none;期數;可用<BR>日數;最近<br>收款額"
  sqlDelete="SELECT RTLessorAVSCust.COMQ1, RTLessorAVSCust.LINEQ1, RTLessorAVSCust.CUSID, " _
                &"RTRIM(LTRIM(CONVERT(char(6), RTLessorAVSCust.COMQ1))) " _
                &"+ '-' + RTRIM(LTRIM(CONVERT(char(6), RTLessorAVSCust.LINEQ1))) AS comqline, " _
                &"RTLessorAVSCmtyH.COMN,RTLessorAVSCust.CUSNC,ltrim(rtrim(RTLessorAVSCust.IP11))+'.'+ltrim(rtrim(RTLessorAVSCust.IP12))+'.'+ltrim(rtrim(RTLessorAVSCust.IP13))+'.'+ltrim(rtrim(RTLessorAVSCust.IP14)), RTLessorAVSCust.TOWNSHIP1 + " _
                &"RTLessorAVSCust.RADDR1 AS ADDR,RTLessorAVSCust.CONTACTTEL + CASE WHEN RTLessorAVSCust.CONTACTTEL = '' OR " _
                &"RTLessorAVSCust.CONTACTTEL IS NULL OR RTLessorAVSCust.MOBILE = '' OR RTLessorAVSCust.MOBILE IS NULL " _
                &"THEN '' ELSE ',' END + RTLessorAVSCust.MOBILE AS Expr1, RTLessorAVSCUST.APPLYDAT, RTLessorAVSCUST.FINISHDAT, " _
                &"RTLessorAVSCUST.STRBILLINGDAT,RTLessorAVSCUST.DUEDAT,RTLessorAVSCUST.DROPDAT,RTLessorAVSCUST.CANCELDAT,ltrim(rtrim(RTLessorAVSCust.IP11))+'.'+ltrim(rtrim(RTLessorAVSCust.IP12))+'.'+ltrim(rtrim(RTLessorAVSCust.IP13))+'.'+ltrim(rtrim(RTLessorAVSCust.IP14)), RTLessorAVSCUST.MAC, RTLessorAVSCUST.PERIOD " _
                &"FROM RTLessorAVSCust LEFT OUTER JOIN RTCounty ON RTLessorAVSCust.CUTID1 = RTCounty.CUTID LEFT OUTER JOIN " _
                &"RTLessorAVSCmtyH ON RTLessorAVSCust.COMQ1 = RTLessorAVSCmtyH.COMQ1 left outer join RTLessorAVScmtyline on " _
                &"RTLessorAVScust.comq1=RTLessorAVScmtyline.comq1 and  RTLessorAVScust.lineq1=RTLessorAVScmtyline.lineq1 " _
                &"left outer join rtcode rtcode_1 on RTLessorAVScust.paycycle=rtcode_1.code and rtcode_1.kind='M8' " _
                &"left outer join rtcode rtcode_2 on RTLessorAVScust.payTYPE=rtcode_2.code and rtcode_2.kind='M9' " _
                &"where RTLessorAVSCust.COMQ1=0 "
  dataTable="RTLessorAVSCust"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="RTLessorAVSCustD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=400,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth="400"
  diaHeight="250"
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=TRUE
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="RTLessorAVSCusts2.asp"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTLessorAVSCust.ComQ1<>0 "
     SEATCHQRY2=""
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
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
  set connXX=server.CreateObject("ADODB.connection")
  set rsXX=server.CreateObject("ADODB.recordset")
  dsnxx="DSN=rtLIB"
  sqlxx="select * from RTEmployee where emply='" & emply & "'"
  connxx.Open dsnxx
  rsxx.Open sqlxx,connxx
  IF not rsxx.EOF then
     if rsxx("dept")="B400" THEN Dareaid="<>'*'"
  end if
  rsxx.Close
  connxx.Close
  set rsxx=nothing
  set connxx=nothing
  '高階主管可讀取全部資料
  '  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or Ucase(emply)="T89003" or _
  '	 Ucase(emply)="T89018" or Ucase(emply)="T89020" or Ucase(emply)="T89025" or Ucase(emply)="T91099" or _
  '	 Ucase(emply)="T92134" or Ucase(emply)="T93168" or Ucase(emply)="T93177" or Ucase(emply)="T94180" then
  '     DAreaID="<>'*'"
  'end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
 ' if userlevel=31 or userlevel =1  or userlevel =5 then DAreaID="<>'*'"
  if userlevel=31 then DAreaID="<>'*'"
  '業務工程師只能讀取該工程師的社區
         sqlList="SELECT RTLessorAVSCust.COMQ1, RTLessorAVSCust.LINEQ1, RTLessorAVSCust.CUSID, " _
                &"RTRIM(LTRIM(CONVERT(char(6), RTLessorAVSCust.COMQ1))) " _
                &"+ '-' + RTRIM(LTRIM(CONVERT(char(6), RTLessorAVSCust.LINEQ1))) AS comqline, " _
                &"RTLessorAVSCmtyH.COMN,case when len(RTLessorAVSCust.CUSNC) > 4 then substring(RTLessorAVSCust.CUSNC,1,4)+'...' else RTLessorAVSCust.CUSNC end,RTCODE_1.CODENC,RTCODE_2.CODENC,ltrim(rtrim(RTLessorAVSCust.IP11))+'.'+ltrim(rtrim(RTLessorAVSCust.IP12))+'.'+ltrim(rtrim(RTLessorAVSCust.IP13))+'.'+ltrim(rtrim(RTLessorAVSCust.IP14)), RTLessorAVSCust.TOWNSHIP1 + " _
                &"RTLessorAVSCust.RADDR1 AS ADDR,RTLessorAVSCust.CONTACTTEL + CASE WHEN RTLessorAVSCust.CONTACTTEL = '' OR " _
                &"RTLessorAVSCust.CONTACTTEL IS NULL OR RTLessorAVSCust.MOBILE = '' OR RTLessorAVSCust.MOBILE IS NULL " _
                &"THEN '' ELSE ',' END + RTLessorAVSCust.MOBILE AS Expr1, RTLessorAVSCUST.APPLYDAT, RTLessorAVSCUST.FINISHDAT, " _
                &"RTLessorAVSCUST.STRBILLINGDAT,RTLessorAVSCUST.newBILLINGDAT,RTLessorAVSCUST.adjustday,RTLessorAVSCUST.DUEDAT,case when RTLESSORAVSCUST.freecode='Y' THEN RTLESSORAVSCUST.freecode ELSE '' END,RTLessorAVSCUST.DROPDAT,RTLessorAVSCUST.CANCELDAT,ltrim(rtrim(RTLessorAVSCust.IP11))+'.'+ltrim(rtrim(RTLessorAVSCust.IP12))+'.'+ltrim(rtrim(RTLessorAVSCust.IP13))+'.'+ltrim(rtrim(RTLessorAVSCust.IP14)), RTLessorAVSCUST.MAC, RTLessorAVSCUST.PERIOD," _
                &"case when RTLessorAVSCUST.DUEDAT is null then 0 when RTLessorAVSCUST.canceldat is not null or RTLessorAVSCUST.dropdat is not null then 0 else DATEDiFF(d,getdate(),RTLessorAVSCUST.DUEDAT) end as validdat,RTLessorAVScust.rcvmoney  " _
                &"FROM RTLessorAVSCust LEFT OUTER JOIN RTCounty ON RTLessorAVSCust.CUTID1 = RTCounty.CUTID LEFT OUTER JOIN " _
                &"RTLessorAVSCmtyH ON RTLessorAVSCust.COMQ1 = RTLessorAVSCmtyH.COMQ1 left outer join RTLessorAVScmtyline on " _
                &"RTLessorAVScust.comq1=RTLessorAVScmtyline.comq1 and  RTLessorAVScust.lineq1=RTLessorAVScmtyline.lineq1 " _
                &"left outer join rtcode rtcode_1 on RTLessorAVScust.paycycle=rtcode_1.code and rtcode_1.kind='M8' " _
                &"left outer join rtcode rtcode_2 on RTLessorAVScust.payTYPE=rtcode_2.code and rtcode_2.kind='M9' " _
                &"where " & searchqry & " " & searchqry2
 'response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>