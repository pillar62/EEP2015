<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="Sparq 網路電話管理系統"
  title="Sparq 網路電話用戶資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'ButtonEnable="Y;N;Y;Y;Y;Y"

'  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
'	if emply ="T89039" then
	  functionOptName="派工單"
	  functionOptProgram="RTSparqWagalySndWrkK.asp"
	  functionOptPrompt="N"
'	end if
  
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="用戶代號;方案類別;Ｔ帳號;用戶名稱;裝機縣市;業務員;經銷商;申請日;完工日;報竣日;退租日;作廢日;派工日"
  sqlDelete="SELECT	CUSID, CASETYPE, NCICCUSNO, CUSNC, RADDR2, SALESID, consignee, "_
		   &"		APPLYDAT, FINISHDAT, DOCKETDAT, DROPDAT, CANCELDAT, TRANSDAT "_
		   &"FROM	RTSparqWagalyCust "_
		   &"WHERE	CUSID ='' "_
		   &"order by cusid "

  dataTable="RTSparqWagalyCust"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="RTSparqWagalyCustD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=TRUE
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="RTSparqWagalyCustS.asp"
  '----
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" a.CUSID <>'' "
     searchShow="全部用戶(不含作廢)"
  ELSE
     SEARCHFIRST=FALSE
  End If
  'userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  'Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  '讀取登入帳號之群組資料
  'Response.Write "GP=" & usergroup
  '-------------------------------------------------------------------------------------------
  'userlevel=2:為業務工程師==>只能看所屬社區資料
  'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
 ' Response.Write "DOMAIN=" & domain & "<BR>"
  'Domain=Mid(Emply,1,1)
  
  'If searchShow="全部" Then
  sqlList=	"SELECT	a.CUSID, f.codenc, a.NCICCUSNO, a.cusnc, isnull(b.cutnc, a.cutid2)+a.township2+a.RADDR2, " &_
			"isnull(e.cusnc, ''), isnull(c.shortnc,''), " &_ 
			"a.APPLYDAT, a.FINISHDAT, a.DOCKETDAT, a.DROPDAT, a.CANCELDAT, g.sndwrkdat " &_
			"FROM RTSparqWagalyCust a " &_
			"left outer join RTCounty b on a.cutid2 = b.cutid " &_
			"left outer join RTObj c on c.cusid = a.consignee " &_
			"left outer join RTEmployee d inner join RTObj e on e.cusid = d.cusid on d.emply = a.salesid " &_
			"left outer join RTCode f on f.code = a.casetype and f.kind ='Q5' " &_
			"left outer join RTSparqWagalySndWrk g inner join (select cusid, max(workno) as maxworkno from RTSparqWagalySndWrk where canceldat is null group by cusid) h " &_
			"		on g.cusid = h.cusid on g.cusid = a.cusid and g.workno = h.maxworkno " &_
			"where " & searchqry &_
			" order by a.cusid "
  
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>
