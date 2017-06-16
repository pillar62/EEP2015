<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="AVS-City管理系統"
  title="應退租或續約用戶資料查詢"
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
  formatName="none;none;none;主線;社區;用戶;週期;繳款;連絡電話;申請日;完工日;開始計費;最近<br>續約日;調整<br>日數;到期日;公關;退租日;作廢日;期數;可用<BR>日數;最近<br>收款額"
  sqlDelete="SELECT	a.COMQ1, a.LINEQ1, a.CUSID, " &_
			"RTRIM(LTRIM(CONVERT(char(6), a.COMQ1))) + '-' + RTRIM(LTRIM(CONVERT(char(6), a.LINEQ1))), " &_
			"c.COMN, case when len(a.CUSNC) > 4 then substring(a.CUSNC,1,4)+'...' else a.CUSNC end, " &_
			"e.CODENC, f.CODENC, " &_
			"a.CONTACTTEL + case when a.CONTACTTEL <>'' and a.MOBILE <>'' then ' / ' else '' end + a.MOBILE, " &_
			"a.APPLYDAT, a.FINISHDAT, a.STRBILLINGDAT, a.newBILLINGDAT, a.adjustday, a.DUEDAT, " &_
			"case when a.freecode='Y' then a.freecode else '' end, a.DROPDAT, a.CANCELDAT, a.PERIOD, " &_
			"isnull(DATEDiFF(d,getdate(),a.DUEDAT), 0) as validdat, a.rcvmoney " &_
			"FROM RTLessorAVSCust a " &_
			"left outer join RTLessorAVScmtyline b on a.comq1=b.comq1 and a.lineq1=b.lineq1 " &_
			"left outer join RTLessorAVSCmtyH c on a.COMQ1 = c.COMQ1 " &_
			"left outer join RTCounty d on a.CUTID1 = d.CUTID " &_
			"left outer join rtcode e on a.paycycle=e.code and e.kind='M8' " &_
			"left outer join rtcode f on a.payTYPE=f.code and f.kind='M9' " &_
			"where a.COMQ1=0 "
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
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="RTLessorAVSCusts4.asp"
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
     searchQry=" a.DUEDAT <= dateadd(m, 1, getdate()) "
     SEATCHQRY2=""
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  'userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  'Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  '讀取登入帳號之群組資料
  '-------------------------------------------------------------------------------------------
         sqlList="SELECT	a.COMQ1, a.LINEQ1, a.CUSID, " &_
			"RTRIM(LTRIM(CONVERT(char(6), a.COMQ1))) + '-' + RTRIM(LTRIM(CONVERT(char(6), a.LINEQ1))), " &_
			"c.COMN, case when len(a.CUSNC) > 4 then substring(a.CUSNC,1,4)+'...' else a.CUSNC end, " &_
			"e.CODENC, f.CODENC, " &_
			"a.CONTACTTEL + case when a.CONTACTTEL <>'' and a.MOBILE <>'' then ' / ' else '' end + a.MOBILE, " &_
			"a.APPLYDAT, a.FINISHDAT, a.STRBILLINGDAT, a.newBILLINGDAT, a.adjustday, a.DUEDAT, " &_
			"case when a.freecode='Y' then a.freecode else '' end, a.DROPDAT, a.CANCELDAT, a.PERIOD, " &_
			"isnull(DATEDiFF(d,getdate(),a.DUEDAT), 0) as validdat, a.rcvmoney " &_
			"FROM RTLessorAVSCust a " &_
			"left outer join RTLessorAVScmtyline b on a.comq1=b.comq1 and a.lineq1=b.lineq1 " &_
			"left outer join RTLessorAVSCmtyH c on a.COMQ1 = c.COMQ1 " &_
			"left outer join RTCounty d on a.CUTID1 = d.CUTID " &_
			"left outer join rtcode e on a.paycycle=e.code and e.kind='M8' " &_
			"left outer join rtcode f on a.payTYPE=f.code and f.kind='M9' " &_
			"where  a.canceldat is null and	a.dropdat is null " &_
			" and " & searchqry & " " & searchqry2
'response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>