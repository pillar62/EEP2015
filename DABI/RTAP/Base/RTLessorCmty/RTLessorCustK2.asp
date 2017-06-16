<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="ET-City管理系統"
  title="ET-City用戶資料維護"
  buttonName=" 新  增 ; 刪  除 ; 結  束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="維修收款;裝機派工;續約作業;復機作業;退租作業;應收應付;客服案件;設備保管收據列印;用戶移動;調整到期;設備查詢;分　配 IP;作　　廢;作廢返轉;歷史異動"
  functionOptProgram="RTLessorCustRepairK.asp;RTLessorCustsndworkk.asp;RTLessorCustContK.asp;RTLessorCustReturnK.asp;RTLessorCustDropK.asp;RTLessorCustARK.asp;RTLessorCustfaqK.asp;/RTAP/REPORT/Common/RTStorageReceiptET.asp;RTLessorCustmove.asp;RTLessorCustadjdayK.asp;RTLessorCusttothardwareK.asp;RTLessorCustAutoIP.asp;RTLessorCustCANCEL.asp;RTLessorCustCANCELRTN.asp;RTLessorCustLOGK.asp"
  functionOptPrompt="N;N;N;N;N;N;N;N;N;Y;N;N;Y;Y;N"
  functionoptopen=  "1;1;1;1;1;1;1;1;1;1;1;1;1;1;1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;主線;社區;用戶;第二<br>戶;方案;週期;繳款;IP位址;none;申請日;完工日;報竣日;開始計費;最近<br>續約日;到期日;公關;退租日;作廢日;none;none;可用<BR>日數;最近<br>收款額"
  sqlDelete="SELECT RTLessorCust.COMQ1, RTLessorCust.LINEQ1, RTLessorCust.CUSID, " _
                &"RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.COMQ1))) " _
                &"+ '-' + RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.LINEQ1))) AS comqline, " _
                &"RTLessorCmtyH.COMN,RTLessorCust.CUSNC,case when RTLessorCust.secondcase='Y' THEN RTLessorCust.secondcase ELSE '' END,ltrim(rtrim(RTLessorCust.IP11))+'.'+ltrim(rtrim(RTLessorCust.IP12))+'.'+ltrim(rtrim(RTLessorCust.IP13))+'.'+ltrim(rtrim(RTLessorCust.IP14)), RTLessorCust.TOWNSHIP1 + " _
                &"RTLessorCust.RADDR1 AS ADDR, RTLESSORCUST.APPLYDAT, RTLESSORCUST.FINISHDAT, RTLESSORCUST.DOCKETDAT, " _
                &"RTLESSORCUST.STRBILLINGDAT,RTLESSORCUST.DUEDAT,RTLESSORCUST.DROPDAT,RTLESSORCUST.CANCELDAT,ltrim(rtrim(RTLessorCust.IP11))+'.'+ltrim(rtrim(RTLessorCust.IP12))+'.'+ltrim(rtrim(RTLessorCust.IP13))+'.'+ltrim(rtrim(RTLessorCust.IP14)), RTLESSORCUST.MAC," _
                &"FROM RTLessorCust LEFT OUTER JOIN RTCounty ON RTLessorCust.CUTID1 = RTCounty.CUTID LEFT OUTER JOIN " _
                &"RTLessorCmtyH ON RTLessorCust.COMQ1 = RTLessorCmtyH.COMQ1 left outer join rtlessorcmtyline on " _
                &"rtlessorcust.comq1=rtlessorcmtyline.comq1 and  rtlessorcust.lineq1=rtlessorcmtyline.lineq1 " _
                &"left outer join rtcode rtcode_1 on rtlessorcust.paycycle=rtcode_1.code and rtcode_1.kind='M8' " _
                &"left outer join rtcode rtcode_2 on rtlessorcust.payTYPE=rtcode_2.code and rtcode_2.kind='M9' " _
                &"left outer join RTCode c on c.code = rtlessorcust.casekind and c.kind ='O9' " _
                &"where RTLessorCust.COMQ1=0 "
  dataTable="RTLessorCust"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="RTLessorCustD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=400,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth="400"
  diaHeight="250"
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="RTLessorCusts2.asp"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=TRUE
  If searchQry="" Then
     searchQry=" RTLessorCust.comq1=0 "
     SEATCHQRY2=""
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  'userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  '讀取登入帳號之群組資料
  'Response.Write "GP=" & usergroup
  '-------------------------------------------------------------------------------------------
  'userlevel=2:為業務工程師==>只能看所屬社區資料
  'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  set connXX=server.CreateObject("ADODB.connection")
  set rsXX=server.CreateObject("ADODB.recordset")
  dsnxx="DSN=RTLIB"
  sqlxx="select * from RTAreaSales where cusid='" & Emply & "' and areaid ='D0' "
  connxx.Open dsnxx
  rsxx.Open sqlxx,connxx
  if not rsxx.EOF then
     limitemply	=" and RTLessorcmtyline.salesid ='" & Emply & "' "
  else
     limitemply =" " 
  end if
  rsxx.Close

  connxx.Close
  set rsxx=nothing
  set connxx=nothing


  '業務工程師只能讀取該工程師的社區
         sqlList="SELECT RTLessorCust.COMQ1, RTLessorCust.LINEQ1, RTLessorCust.CUSID, " _
                &"RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.COMQ1))) " _
                &"+ '-' + RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.LINEQ1))) AS comqline, " _
                &"RTLessorCmtyH.COMN,case when len(RTLessorCust.CUSNC) > 4 then substring(RTLessorCust.CUSNC,1,4)+'...' else RTLessorCust.CUSNC end,case when RTLessorCust.secondcase='Y' THEN RTLessorCust.secondcase ELSE '' END,c.codenc,RTCODE_1.CODENC,RTCODE_2.CODENC,ltrim(rtrim(RTLessorCust.IP11))+'.'+ltrim(rtrim(RTLessorCust.IP12))+'.'+ltrim(rtrim(RTLessorCust.IP13))+'.'+ltrim(rtrim(RTLessorCust.IP14)), RTLessorCust.TOWNSHIP1 + " _
                &"RTLessorCust.RADDR1 AS ADDR, RTLESSORCUST.APPLYDAT, RTLESSORCUST.FINISHDAT, RTLESSORCUST.DOCKETDAT, " _
                &"RTLESSORCUST.STRBILLINGDAT,RTLESSORCUST.newBILLINGDAT,RTLESSORCUST.DUEDAT,case when RTLESSORCUST.freecode='Y' THEN RTLESSORCUST.freecode ELSE '' END,RTLESSORCUST.DROPDAT,RTLESSORCUST.CANCELDAT,ltrim(rtrim(RTLessorCust.IP11))+'.'+ltrim(rtrim(RTLessorCust.IP12))+'.'+ltrim(rtrim(RTLessorCust.IP13))+'.'+ltrim(rtrim(RTLessorCust.IP14)), RTLESSORCUST.MAC," _
                &"case when RTLESSORCUST.DUEDAT is null then 0 when RTLESSORCUST.canceldat is not null or RTLESSORCUST.dropdat is not null then 0 else DATEDiFF(d,getdate(),RTLESSORCUST.DUEDAT) end as validdat,rtlessorcust.rcvmoney  " _
                &"FROM RTLessorCust LEFT OUTER JOIN RTCounty ON RTLessorCust.CUTID1 = RTCounty.CUTID LEFT OUTER JOIN " _
                &"RTLessorCmtyH ON RTLessorCust.COMQ1 = RTLessorCmtyH.COMQ1 inner join rtlessorcmtyline on " _
                &"rtlessorcust.comq1=rtlessorcmtyline.comq1 and  rtlessorcust.lineq1=rtlessorcmtyline.lineq1 " _
                &"left outer join rtcode rtcode_1 on rtlessorcust.paycycle=rtcode_1.code and rtcode_1.kind='M8' " _
                &"left outer join rtcode rtcode_2 on rtlessorcust.payTYPE=rtcode_2.code and rtcode_2.kind='M9' " _
                &"left outer join RTCode c on c.code = rtlessorcust.casekind and c.kind ='O9' " _
                &"where " & searchqry & " " & limitemply & " " & searchqry2                 
 'response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>