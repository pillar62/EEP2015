<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="電費管理系統"
  title="每期電費維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=v(0)&";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;N"
  'functionOptName="社區其他方案"
  'functionOptProgram="RTPowerBillCmtyK.asp"
  'functionOptPrompt="N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="電費序號;none;方案;社區名稱;起算<br>年月;截止<br>年月;計算方式;本期<BR>線數;本期<BR>戶數;給付<br>方式;本期<br>金額;前期<BR>度數;本期<br>度數;派工<br>單號;支票<br>寄出日;收據<br>回收日;是否向<br>Sonet請款;縣市;鄉鎮;作廢日;"
  sqlDelete="select x.billno, x.ctno, c.codenc, b.comn, x.strym, x.endym, " &_
			"g.codenc, x.linenum, x.custnum, f.codenc, x.pay, x.ratiobefore, x.ratio, " &_
			"x.workno, x.checkoutdat, receiptdat, " &_
			"replace(sonetreq,'N',''), d.cutnc, b.township, x.canceldat " &_
			"from	RTPowBillMM x " &_
			"inner join 	RTPowBillH a on x.ctno = a.ctno " &_
			"left outer join RTCmtyAll b on a.comtype = b.comtype and a.comq1 = b.comq1 " &_
			"left outer join RTCode c on c.code = a.comtype and c.kind ='P5' " &_
			"left outer join RTCounty d on d.cutid = b.cutid " &_
			"left outer join RTCode f on f.code = x.paytype and f.kind ='F5' " &_
			"left outer join RTCode g on g.code = x.counttype and g.kind ='R4' " &_
			"where x.billno ='' "
  dataTable="RTPowBillMM"
  extTable=""
  numberOfKey=1
  dataProg="RTPowerBillMMD.asp"
  datawindowFeature=""
  searchWindowFeature=""
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=False
  goodMorningImage=""
  colSplit=1
  keyListPageSize=30
  
  searchProg="RTPowerBillMMS.asp"
  searchFirst=FALSE
  If searchQry="" then
     'searchQry=" and x.billno<>'' "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If  

  sqlList="select x.billno, x.ctno, c.codenc, b.comn, x.strym, x.endym, " &_
			"g.codenc, x.linenum, x.custnum, f.codenc, x.pay, x.ratiobefore, x.ratio, " &_
			"x.workno, x.checkoutdat, receiptdat, " &_
			"replace(sonetreq,'N',''), d.cutnc, b.township, x.canceldat " &_
			"from	RTPowBillMM x " &_
			"inner join 	RTPowBillH a on x.ctno = a.ctno " &_
			"left outer join RTCmtyAll b on a.comtype = b.comtype and a.comq1 = b.comq1 " &_
			"left outer join RTCode c on c.code = a.comtype and c.kind ='P5' " &_
			"left outer join RTCounty d on d.cutid = b.cutid " &_
			"left outer join RTCode f on f.code = x.paytype and f.kind ='F5' " &_
			"left outer join RTCode g on g.code = x.counttype and g.kind ='R4' " &_
  			"where x.billno<>'' " &searchQry &_
			" order by c.codenc, b.comn "
'Response.Write "sql=" & SQLLIST         
End Sub
%>
