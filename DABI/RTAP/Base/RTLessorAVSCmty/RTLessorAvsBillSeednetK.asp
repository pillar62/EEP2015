<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="AVS-City管理系統"
  title="Seednet每日交易檔及逢8結算檔查詢"
  buttonName=" 新  增 ; 刪  除 ; 結  束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="結算檔匯入;列印Seednet未結算交易檔"
  functionOptProgram="RTLessorAvsBillReckonImport.asp;RTLessorAvsBillNoReckonXls.asp"
  functionOptPrompt="H;H"
  functionoptopen="1;1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="帳單編號;用戶編號;列帳年月;用戶名稱;實繳<br>金額;用戶方案;繳費超商;用戶至超<br>商繳款日;超商<br>處理日;Seednet<br>收款日;Seednet<br>沖銷日;Seednet<br>結算日"
  sqlDelete="select  a.csnoticeid, a.cscusid, a.accountym, a.cusnc, a.amt, a.memo, a.csname, a.cspaydat, a.csseednetdat, b.rcvdat, b.abatedat, b.closedat " &_
			"from RTBillSeednetTrade a " &_
			"left outer join RTBillSeednetReckon b on a.csnoticeid = b.csnoticeid and a.cscusid = b.cscusid " &_
			"where a.csnoticeid ='' " &_
  dataTable="RTBillSeednetTrade"
  userDefineDelete="Yes"
  numberOfKey=2
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
     searchQry=	" a.accountym in ( select  a.accountym from RTBillSeednetTrade a " &_
				"				left outer join RTBillSeednetReckon b on a.csnoticeid = b.csnoticeid and a.cscusid = b.cscusid " &_
				"				where b.closedat is null group by a.accountym ) " 
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

  sqlList="select  a.csnoticeid, a.cscusid, a.accountym, a.cusnc, a.amt, a.memo, a.csname, a.cspaydat, a.csseednetdat, b.rcvdat, b.abatedat, b.closedat " &_
			"from RTBillSeednetTrade a " &_
			"left outer join RTBillSeednetReckon b on a.csnoticeid = b.csnoticeid and a.cscusid = b.cscusid " &_
			"where " & SEARCHQRY &_
			" order by a.csseednetdat "
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>