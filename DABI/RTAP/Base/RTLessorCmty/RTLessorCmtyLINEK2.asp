<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="ET-City管理系統"
  title="ET-City主線資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  functionOptName="主線派工;設備查詢;用戶維護;客服案件;到期續約;撤線作業;作　　廢;作廢返轉;歷史異動"
  functionOptProgram="RTLessorCmtyLineSNDWORKK.asp;RTLessorCmtyLINEHardwareK2.asp;RTLessorCustK.asp;RTLessorCmtyLineFAQK.asp;RTLessorCmtyLineContK.asp;RTLessorCmtyLineDROPK.asp;RTLessorCmtyLineCANCEL.asp;RTLessorCmtyLineCANCELRTN.asp;RTLessorCmtyLineLOGK.asp"
  functionOptPrompt="N;N;N;N;N;N;Y;Y;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  'formatName="none;none;社區名稱;主線;鄉鎮;群組;主線IP;none;none;none;主線附掛;線路ISP;IP種類;主線速率;IP數;Reset電話;none;none;none;到位日;none;撤線日;作廢日;用戶;報竣;退租;計費"
  formatName="none;none;轄區;社區名稱;主線;縣市;鄉鎮;none;主線IP;none;none;none;none;線路<br>ISP;IP<br>種類;主線速率;none;Reset電話;none;none;none;到位日;none;撤線日;作廢日;用戶;報竣;退租;計費"  
    sqlDelete="select a.comq1, a.lineq1, isnull(d.shortnc, f.cusnc), c.comn, convert(varchar(6), a.comq1) +'-'+ convert(varchar(6),a.lineq1), " &_
			"g.cutnc, a.township, a.linegroup,a.lineip,a.gateway, a.pppoeaccount, a.pppoepassword, a.linetel, " &_
			"substring(h.codenc,1,2), i.codenc, j.codenc, a.ipcnt, k.tel, a.rcvdat, a.inspectdat, a.hinetnotifydat, " &_
			"a.hardwaredat, a.adslapplydat, a.dropdat, a.canceldat, " &_
			"sum(case when b.cusid is not null and b.canceldat is null then 1 else 0 end), " &_
			"sum(case when b.cusid is not null and b.canceldat is null and b.finishdat is not null then 1 else 0 end), " &_
			"sum(case when b.cusid is not null and b.canceldat is null and b.finishdat is not null and b.dropdat is not null then 1 else 0 end), " &_
			"sum(case when b.cusid is not null and b.canceldat is null and (b.strbillingdat is not null or b.newbillingdat is not null) and b.dropdat is null then 1 else 0 end) " &_
			"from RTLessorCmtyLine a " &_
			"left outer join RTLessorCust b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
			"inner join RTLessorCmtyH c on c.comq1 = a.comq1 " &_
			"left outer join RTObj d on a.consignee = d.cusid " &_
			"left outer join RTEmployee e inner join RTObj f on e.cusid = f.cusid on a.salesid = e.emply " &_
			"left outer join RTCounty g on a.cutid=g.cutid " &_
			"left outer join RTCode h on a.lineisp = h.code and h.kind = 'c3' " &_
			"left outer join RTCode i on a.lineiptype = i.code and i.kind = 'm5' " &_
			"left outer join RTCode j on a.linerate = j.code and j.kind = 'd3' " &_
			"left outer join RTReset k on k.comq1 = a.comq1 and k.lineq1 = a.lineq1 and k.cmtytype ='07' and k.canceldat is null " &_
			"where " & searchqry & limitemply &_
			" group by  a.comq1, a.lineq1, isnull(d.shortnc, f.cusnc), c.comn, convert(varchar(6), a.comq1) +'-'+ convert(varchar(6),a.lineq1), " &_
			"g.cutnc, a.township, a.linegroup,a.lineip,a.gateway, a.pppoeaccount, a.pppoepassword, a.linetel, " &_
			"substring(h.codenc,1,2), i.codenc, j.codenc, a.ipcnt, k.tel, a.rcvdat, a.inspectdat, a.hinetnotifydat, " &_
			"a.hardwaredat, a.adslapplydat, a.dropdat, a.canceldat " &_
            "ORDER BY  a.COMQ1, a.LINEQ1 "

  dataTable="RTLessorCmtyLine"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="RTLessorCmtyLineD.asp"
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
  searchProg="RTLessorCmtylineS2.asp"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  set connXX=server.CreateObject("ADODB.connection")
  set rsXX=server.CreateObject("ADODB.recordset")
  dsnxx="DSN=RTLIB"
  sqlxx="select * from RTAreaSales where cusid='" & Emply & "' and areaid ='D0' "
  connxx.Open dsnxx
  rsxx.Open sqlxx,connxx
  if not rsxx.EOF then
     limitemply	=" and a.salesid ='" & Emply & "' "
  else
     limitemply =" " 
  end if
  rsxx.Close

  connxx.Close
  set rsxx=nothing
  set connxx=nothing
  '----
 
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" a.ComQ1<>0 "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  'Response.Write "GP=" & usergroup
  '-------------------------------------------------------------------------------------------
  'userlevel=2:為業務工程師==>只能看所屬社區資料
 ' Response.Write "DOMAIN=" & domain & "<BR>"
    sqlList="select a.comq1, a.lineq1, isnull(d.shortnc, f.cusnc), c.comn, convert(varchar(6), a.comq1) +'-'+ convert(varchar(6),a.lineq1), " &_
			"g.cutnc, a.township, a.linegroup,a.lineip,a.gateway, a.pppoeaccount, a.pppoepassword, a.linetel, " &_
			"substring(h.codenc,1,2), i.codenc, j.codenc, a.ipcnt, k.tel, a.rcvdat, a.inspectdat, a.hinetnotifydat, " &_
			"a.hardwaredat, a.adslapplydat, a.dropdat, a.canceldat, " &_
			"sum(case when b.cusid is not null and b.canceldat is null then 1 else 0 end), " &_
			"sum(case when b.cusid is not null and b.canceldat is null and b.finishdat is not null then 1 else 0 end), " &_
			"sum(case when b.cusid is not null and b.canceldat is null and b.finishdat is not null and b.dropdat is not null then 1 else 0 end), " &_
			"sum(case when b.cusid is not null and b.canceldat is null and (b.strbillingdat is not null or b.newbillingdat is not null) and b.dropdat is null then 1 else 0 end) " &_
			"from RTLessorCmtyLine a " &_
			"left outer join RTLessorCust b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
			"inner join RTLessorCmtyH c on c.comq1 = a.comq1 " &_
			"left outer join RTObj d on a.consignee = d.cusid " &_
			"left outer join RTEmployee e inner join RTObj f on e.cusid = f.cusid on a.salesid = e.emply " &_
			"left outer join RTCounty g on a.cutid=g.cutid " &_
			"left outer join RTCode h on a.lineisp = h.code and h.kind = 'c3' " &_
			"left outer join RTCode i on a.lineiptype = i.code and i.kind = 'm5' " &_
			"left outer join RTCode j on a.linerate = j.code and j.kind = 'd3' " &_
			"left outer join RTReset k on k.comq1 = a.comq1 and k.lineq1 = a.lineq1 and k.cmtytype ='07' and k.canceldat is null " &_
			"where " & searchqry & limitemply &_
			" group by  a.comq1, a.lineq1, isnull(d.shortnc, f.cusnc), c.comn, convert(varchar(6), a.comq1) +'-'+ convert(varchar(6),a.lineq1), " &_
			"g.cutnc, a.township, a.linegroup,a.lineip,a.gateway, a.pppoeaccount, a.pppoepassword, a.linetel, " &_
			"substring(h.codenc,1,2), i.codenc, j.codenc, a.ipcnt, k.tel, a.rcvdat, a.inspectdat, a.hinetnotifydat, " &_
			"a.hardwaredat, a.adslapplydat, a.dropdat, a.canceldat " &_
            "ORDER BY  a.COMQ1, a.LINEQ1 "

  'end if
 'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>