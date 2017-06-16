<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="遠傳大寬頻社區型管理系統"
  title="遠傳大寬頻社區型用戶資料維護"
  buttonName=" 新  增 ; 刪  除 ; 結  束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  functionOptName="維修收款;派工作業;設備保管收據列印;作　　廢;作廢返轉"
  functionOptProgram="RTFareastCustRepairK.asp;RTfareastCustSndWrkK.asp;/RTAP/REPORT/Common/RTStorageReceiptfareast.asp;RTfareastCustCANCEL.asp;RTfareastCustCANCELRTN.asp"
  functionOptPrompt="N;N;N;Y;Y"
  functionoptopen=  "1;1;1;1;1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;業務<br>轄區;主<br>線;社區;用戶;公<br>關;速率;週期;用戶IP;申請日;完工日;報竣日;So-net<br>啟用日;So-net首次<br>繳款日;退租日;作廢日"
  sqlDelete="SELECT i_smallint, i_tinyint, c_varchar, c_varchar, c_varchar,c_varchar,c_varchar,c_varchar,c_varchar,c_varchar, " &_
  			"c_varchar,d_datetime,d_datetime,d_datetime,d_datetime,d_datetime,d_datetime,d_datetime from RTTemplate"
  dataTable="RTfareastCust"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="RTfareastCustD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=500,scrollbars=yes"
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
  searchProg="RTfareastCustS2.asp"
  searchFirst=False
  If searchQry="" Then
     searchQry=" a.COMQ1 <>0 "
     SEATCHQRY2=""
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If

    if ARYPARMKEY(0) <>"" then 
		searchQry = searchQry & " and a.comq1=" & aryparmkey(0)
	end if

    if ARYPARMKEY(1) <>"" then 
		searchQry = searchQry & " and a.lineq1=" & aryparmkey(1)
	end if

  '-------------------------------------------------------------------------------------------
	sqlList="select a.comq1, a.lineq1, a.cusid, isnull(f.shortnc, e.cusnc), convert(varchar(6), a.comq1) +'-'+ convert(varchar(3),a.lineq1), c.comn, " &_
			"a.cusnc, a.freecode, g.codenc, h.codenc, " &_
			"case when a.dropdat is not null or a.canceldat is not null then '<font color=""red"">' end + a.ip11 + case when a.dropdat is not null or a.canceldat is not null then '</font>' end, " &_
			"a.applydat, a.finishdat, a.docketdat, a.activatedat, a.strbillingdat, a.dropdat, a.canceldat " &_
			"from RTfareastCust a " &_
			"	inner join RTfareastCmtyLine b on a.comq1 = b.comq1 and a.lineq1= b.lineq1 " &_
			"	inner join RTfareastCmtyH c on b.comq1 = c.comq1 " &_
			"	left outer join RTEmployee d inner join RTObj e on e.cusid = d.cusid on d.emply = c.salesid " &_
			"	left outer join RTObj f on f.cusid = c.consignee " &_
			"	left outer join RTCode g on g.code = a.userrate and g.kind ='R6' " &_
			"	left outer join RTCode h on h.code = a.paycycle and h.kind ='M8' " &_
			"where " & searchqry &_
			" order by a.comq1,  case when a.dropdat is null and a.canceldat is null then 0 else 1 end, case a.ip11 when '' then 0 else convert(tinyint,right(a.ip11,len(a.ip11)-charindex('.', a.ip11, charindex('.', a.ip11, charindex('.', a.ip11)+1)+1))) end "
'response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>