<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="So-net管理系統"
  title="So-net主線資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  if ARYPARMKEY(0) ="" then
    ButtonEnable="N;N;Y;Y;Y;Y"
  else
    ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  end if
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  functionOptName="主線流量;網管(ping);用戶維護;主線派工;作　　廢;作廢返轉"
  functionOptProgram="RTSonetCmtyLineMrtg.asp;RTSonetCmtyLineTool.asp;RTSonetCustK2.asp;RTSonetCmtyLineSndWrkK.asp;RTSonetCmtyLineCancel.asp;RTSonetCmtyLineCancelRtn.asp"
  functionOptPrompt="N;N;N;N;Y;Y"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;業務<br>轄區;工務<br>轄區;社區名稱;主線<br>序號;縣市;鄉鎮;專線編號;主線IP;IP<br>種類;主線速率;主線<br>到位日;撤線日;作廢日;已報竣<br>戶數"
  sqlDelete="select i_smallint, i.tinyint, c_varchar, c_varchar, c_varchar, c_varchar, c_varchar,c_varchar, c_varchar, c_varchar, " &_
  			"c_varchar, c_varchar, d_datetime, d_datetime, d_datetime, i_smallint " &_
  			"from RTTemplate "
  dataTable="RTSonetCmtyLine"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="RTSonetCmtyLineD.asp"
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
  keyListPageSize=30
  searchProg="RTSonetCmtyLineS2.asp"

  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" a.comq1<>0 "
     searchShow="全部"
  End If

    if ARYPARMKEY(0) <>"" then 
		searchQry = searchQry & " and a.ComQ1=" & aryparmkey(0)
	end if

  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  '讀取登入帳號之群組資料
  'Response.Write "GP=" & usergroup
  '-------------------------------------------------------------------------------------------
    sqlList="select  a.comq1, a.lineq1, isnull(m.cusnc,''), isnull(d.shortnc, f.cusnc), c.comn, convert(varchar(6), a.comq1) +'-'+ convert(varchar(3),a.lineq1), " &_
			"g.cutnc, a.township, a.linetel, a.lineip, i.codenc, j.codenc, a.hardwaredat, a.dropdat, a.canceldat, isnull(k.custnum, 0) " &_
			"from RTSonetCmtyLine a " &_
			"inner join RTSonetCmtyH c on c.comq1 = a.comq1 " &_
			"left outer join RTObj d on c.consignee = d.cusid " &_
			"left outer join RTEmployee e inner join RTObj f on e.cusid = f.cusid on c.engid = e.emply " &_
			"left outer join RTEmployee l inner join RTObj m on l.cusid = m.cusid on c.salesid = l.emply " &_
			"left outer join RTCounty g on a.cutid=g.cutid " &_
			"left outer join RTCode i on a.lineiptype = i.code and i.kind = 'm5' " &_
			"left outer join RTCode j on a.linerate = j.code and j.kind = 'd3' " &_
			"left outer join (select y.comq1, y.lineq1, count(*) as custnum from RTSonetCust x inner join RTSonetCmtyLine y " &_
			"	on x.comq1 = y.comq1 and x.lineq1 = y.lineq1 where x.dropdat is null and x.canceldat is null " &_
			"	and x.docketdat is not null group by  y.comq1, y.lineq1) k on k.comq1 = a.comq1 and k.lineq1 = a.lineq1 " &_
			"where " & searchqry &_
			" ORDER BY a.comq1, a.lineq1 "
 'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>