<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="電費管理系統"
  title="電費基本資料查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=v(0)&";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="社區其他方案"
  functionOptProgram="RTPowerBillCmtyK.asp"
  functionOptPrompt="N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="合約序號;方案;社區名稱;縣市;鄉鎮;計算方式;公電週期;補助方式;合約生效日;合約終止日;聯絡人;聯絡電話;是否向Sonet請款;作廢日;"
  sqlDelete="select 	a.ctno, c.codenc, b.comn, d.cutnc, b.township, g.codenc, e.codenc, f.codenc, strdat, enddat, " &_
  			"contact, contacttel, replace(sonetreq,'N',''), canceldat " &_
			"from	RTPowBillH a " &_
			"left outer join RTCmtyAll b on a.comtype = b.comtype and a.comq1 = b.comq1 " &_
			"left outer join RTCode c on c.code = a.comtype and c.kind ='P5' " &_
			"left outer join RTCounty d on d.cutid = b.cutid " &_
			"left outer join RTCode e on e.code = a.paycycle and e.kind ='K5' " &_
			"left outer join RTCode f on f.code = a.paytype and f.kind ='F5' " &_
			"left outer join RTCode g on g.code = a.counttype and g.kind ='R4' " &_
			"where	a.ctno ='' "
  dataTable="RTPowBillH"
  extTable=""
  numberOfKey=1
  dataProg="RTPowerBillD.asp"
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
  
  searchProg="RTPowerBillS.asp"
  searchFirst=FALSE
  If searchQry="" then
     searchQry=" and a.ctno<>'' "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If  

  sqlList=	"select 	a.ctno, c.codenc, b.comn, d.cutnc, b.township, g.codenc, e.codenc, f.codenc, strdat, enddat, " &_
  			"contact, contacttel, replace(sonetreq,'N',''), canceldat " &_
			"from	RTPowBillH a " &_
			"left outer join RTCmtyAll b on a.comtype = b.comtype and a.comq1 = b.comq1 " &_
			"left outer join RTCode c on c.code = a.comtype and c.kind ='P5' " &_
			"left outer join RTCounty d on d.cutid = b.cutid " &_
			"left outer join RTCode e on e.code = a.paycycle and e.kind ='K5' " &_
			"left outer join RTCode f on f.code = a.paytype and f.kind ='F5' " &_
			"left outer join RTCode g on g.code = a.counttype and g.kind ='R4' " &_
			"where a.ctno<>'' " &searchQry &_
			" order by 4,5,3 "
'Response.Write "sql=" & SQLLIST         
End Sub
%>
