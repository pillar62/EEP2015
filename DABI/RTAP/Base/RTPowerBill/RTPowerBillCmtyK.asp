<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="電費管理系統"
  title="電費其他社區方案查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=v(0)&";Y;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="合約序號;none;社區序號;方案;社區名稱;縣市;鄉鎮;作廢日;"
     sqlDelete="select a.ctno, a.comtype, a.comq1, c.codenc, b.comn, d.cutnc, b.township, a.canceldat " &_
			"from 	RTPowBillCmty a " &_
			"left outer join RTCmtyAll b on a.comtype = b.comtype and a.comq1 = b.comq1 " &_
			"left outer join RTCode c on c.code = a.comtype and c.kind ='P5' " &_
			"left outer join RTCounty d on d.cutid = b.cutid " &_
			"where a.CTNO ='' "
  dataTable="RTPowBillCmty"
  extTable=""
  numberOfKey=3
  dataProg="RTPowerBillCmtyD.asp"
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
  keyListPageSize=25
  
  searchProg=""
  searchFirst=FALSE
  If searchQry="" then
     searchQry= " and a.ctno<>'' "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If  
  
     sqlList="select a.ctno, a.comtype, a.comq1, c.codenc, b.comn, d.cutnc, b.township, a.canceldat " &_
			"from 	RTPowBillCmty a " &_
			"left outer join RTCmtyAll b on a.comtype = b.comtype and a.comq1 = b.comq1 " &_
			"left outer join RTCode c on c.code = a.comtype and c.kind ='P5' " &_
			"left outer join RTCounty d on d.cutid = b.cutid " &_
			"where a.ctno ='"& aryparmkey(0) &"' "& searchQry
'Response.Write "sql=" & SQLLIST         
End Sub
%>
