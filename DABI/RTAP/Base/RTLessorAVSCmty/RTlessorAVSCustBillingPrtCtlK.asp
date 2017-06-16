<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="AVS-City管理系統"
  title="AVS-City每月續約帳單列印查詢<BR><font color=white>廠商編號：BPIS0334<BR>廠商密碼：yyvu7knt</font><BR><font color=red size=4><B>續約文字檔請於下午 1:00 前上傳</B></font>"
  buttonName=" 新  增 ; 刪  除 ; 結  束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="0.產生續約單;0.產生續約單(過期);1.匯出續約文字檔;2.上傳續約文字檔;3.匯入條碼檔;4.列印續約單;5.列印信封;用戶明細"
  functionOptProgram="RTLessorAVSCustBillingPrtCtlTRNK.asp;RTLessorAVSCustBillingPrtCtlTRNK_duedate.asp;RTLessorAVSCustBillSeednetBatch.asp;https://service.seed.net.tw/proxy_portal.htm;RTLessorAVSCustBillBarcode.asp;/REPORT/Common/RTLessorAVSCustBillingPrtCtlP.asp;/Report/AVSCity/RTLessorAVSCustBillingPrtEnvP.asp;RTLessorAVSCustBillingPrtCtlk2.asp"
  functionOptPrompt="N;N;N;N;N;N;N;N"
  functionoptopen="1;1;1;1;1;1;1;1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  'formatName="通知書批次;到期日B(起);到期日B(迄);到期日A(起);到期日A(迄);產生日;產生員;最後列印日;列印員"
  formatName="通知單批次;none;none;到期日(起);到期日(迄);產生日;產生員;匯出日;匯出員;條碼匯入日;條碼匯入員;最後列印日;列印員"
  sqlDelete="SELECT BATCH, DUEDATSB, DUEDATEB, DUEDATSA, DUEDATEA, CDAT, c.CUSNC, " &_
			"BARCODOUTDAT, g.cusnc, BARCODINDAT, i.cusnc, PRTDAT, e.CUSNC " &_
			"FROM RTLessorAVSCustBillingPrt a " &_
			"left outer join RTEmployee b inner join RTObj c on c.cusid =b.cusid on b.emply = a.CUSR " &_
			"left outer join RTEmployee d inner join RTObj e on d.cusid =e.cusid on d.emply = a.PRTUSR " &_
			"left outer join RTEmployee f inner join RTObj g on f.cusid =g.cusid on f.emply = a.BARCODOUTUSR " &_
			"left outer join RTEmployee h inner join RTObj i on h.cusid =i.cusid on h.emply = a.BARCODINUSR " &_
			"WHERE BATCH=0 " &_
  dataTable="RTLessorAVSCustBillingPrt"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=640,height=300,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth="600"
  diaHeight="400"
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=50
  searchProg="self"
  If searchQry="" Then
     searchFirst=FALSE
     searchQry=" BATCH <> 0 "
     searchShow="全部"
  Else
     searchFirst=False
  End If


  '----------------------------------------------------------------------------------------------
  'set connXX=server.CreateObject("ADODB.connection")
  'set rsXX=server.CreateObject("ADODB.recordset")
  'dsnxx="DSN=XXLIB"
  'sqlxx="select * from usergroup where userid='" & Request.ServerVariables("LOGON_USER") & "'"
  'connxx.Open dsnxx
  'rsxx.Open sqlxx,connxx
  'if not rsxx.EOF then
  '   usergroup=rsxx("group")
  'else
  '   usergroup=""
  'end if
  'rsxx.Close
  'connxx.Close
  'set rsxx=nothing
  'set connxx=nothing
  '----------------------------------------------------------------------------------------------
  'userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  'Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  
  sqlList="SELECT BATCH, DUEDATSB, DUEDATEB, DUEDATSA, DUEDATEA, CDAT, c.CUSNC, " &_
			"BARCODOUTDAT, g.cusnc, BARCODINDAT, i.cusnc, PRTDAT, e.CUSNC " &_
			"FROM RTLessorAVSCustBillingPrt a " &_
			"left outer join RTEmployee b inner join RTObj c on c.cusid =b.cusid on b.emply = a.CUSR " &_
			"left outer join RTEmployee d inner join RTObj e on d.cusid =e.cusid on d.emply = a.PRTUSR " &_
			"left outer join RTEmployee f inner join RTObj g on f.cusid =g.cusid on f.emply = a.BARCODOUTUSR " &_
			"left outer join RTEmployee h inner join RTObj i on h.cusid =i.cusid on h.emply = a.BARCODINUSR " &_
			"WHERE " & SEARCHQRY &_
			"ORDER BY BATCH DESC "
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>