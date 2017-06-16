<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="速博管理系統"
  title="速博用戶應收應付帳款查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=" 沖  帳 ;沖帳明細;帳款明細"
  functionOptProgram="RTSparqCustARClear.asp;RTSparqCustARClearK.asp;RTSparqCustARDTLK.asp"
  functionOptPrompt="N;N;N"
  functionoptopen="2;1;1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;帳款編號;方案別;社區;客戶;AR/AP;期數;應沖<br>金額;已沖<br>金額;未沖<br>金額;沖帳日;沖帳員;沖立項一;沖立項二;none;產生日;作廢日;作廢員;作廢原因"
  sqlDelete="SELECT  RTSparq499CustAR.CUSID, RTSparq499CustAR.BATCHNO, RTSparq499CmtyH.COMN,RTSparq499Cust.CUSNC," _
                &" RTCode.CODENC, RTSparq499CustAR.PERIOD,RTSparq499CustAR.AMT,RTSparq499CustAR.REALAMT," _
                &"RTSparq499CustAR.AMT - RTSparq499CustAR.REALAMT AS DIFFAMT, RTSparq499CustAR.MDAT, RTObj_1.CUSNC AS MUSR, " _
                &"RTSparq499CustAR.COD1, RTSparq499CustAR.COD2,RTSparq499CustAR.COD3, RTSparq499CustAR.CDAT, " _
                &"RTSparq499CustAR.CANCELDAT, RTObj_2.CUSNC AS CANCELUSR, " _
                &", RTSparq499CustAR.CANCELMEMO " _
                &"FROM    RTSparq499CmtyH RIGHT OUTER JOIN RTSparq499Cust ON RTSparq499CmtyH.COMQ1 = RTSparq499Cust.COMQ1 " _
                &"RIGHT OUTER JOIN RTEmployee RTEmployee_2 INNER JOIN RTObj RTObj_2 ON RTEmployee_2.CUSID = " _
                &"RTObj_2.CUSID RIGHT OUTER JOIN RTSparq499CustAR ON RTEmployee_2.EMPLY = RTSparq499CustAR.CANCELUSR " _
                &"LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN RTObj RTObj_1 ON RTEmployee_1.CUSID = " _
                &"RTObj_1.CUSID ON RTSparq499CustAR.MUSR = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
                &"RTCode ON RTSparq499CustAR.ARTYPE = RTCode.CODE AND RTCode.KIND = 'N2' ON RTSparq499Cust.CUSID = " _
                &"RTSparq499CustAR.CUSID " _
                &"WHERE RTSparq499CustAR.cusid='' "
  dataTable="RTSparq499CUSTLOG"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg=""
  datawindowFeature=""
  searchWindowFeature="width=350,height=250,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth="500"
  diaHeight="500"
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="RTSparqCustARS1.asp"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  set connXX=server.CreateObject("ADODB.connection")
  set rsXX=server.CreateObject("ADODB.recordset")
  dsnxx="DSN=XXLIB"
  sqlxx="select * from usergroup where userid='" & Request.ServerVariables("LOGON_USER") & "'"
  connxx.Open dsnxx
  rsxx.Open sqlxx,connxx
  if not rsxx.EOF then
     usergroup=rsxx("group")
  else
     usergroup=""
  end if
  rsxx.Close
  connxx.Close
  set rsxx=nothing
  set connxx=nothing
  '----

  searchFirst=FALSE
  If searchQry="" Then
     searchQry ="  a.amt <> a.realamt and a.canceldat is null " 
     searchQry2 =searchQry
     searchShow="全部未沖銷帳款"
  ELSE
     SEARCHFIRST=FALSE
  End If
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  '讀取登入帳號之群組資料
  'Response.Write "GP=" & usergroup
  '-------------------------------------------------------------------------------------------
  'userlevel=2:為業務工程師==>只能看所屬社區資料
  'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
 ' Response.Write "DOMAIN=" & domain & "<BR>"
  'Domain=Mid(Emply,1,1)
	sqlList="select	a.cusid, a.batchno, 'Sparq399', c.comn, i.cusnc, d.codenc, a.period, " &_
			"a.amt, a.realamt, a.amt-a.realamt, a.mdat, f.cusnc, " &_
			"a.cod1, a.cod2, a.cod3, a.cdat, a.canceldat, h.cusnc, a.cancelmemo " &_
			"from 	RTSparqAdslCustAR a " &_
			"inner join RTSparqAdslCust b on a.cusid = b.cusid " &_
			"inner join RTSparqAdslCmty c on c.cutyid = b.comq1 " &_
			"left outer join RTCode d on a.artype = d.CODE AND d.KIND = 'N2' " &_
			"left outer join RTEmployee e inner join RTObj f on f.cusid = e.cusid on e.emply = a.musr " &_
			"left outer join RTEmployee g inner join RTObj h on h.cusid = g.cusid on g.emply = a.cancelusr " &_
			"left outer join RTObj i on i.cusid = b.cusid " &_
			"where " & searchQry &_
			" union " &_
			"select	a.cusid, a.batchno, 'Sparq499', c.comn, b.cusnc, d.codenc, a.period, " &_
			"a.amt, a.realamt, a.amt-a.realamt, a.mdat, f.cusnc, " &_
			"a.cod1, a.cod2, a.cod3, a.cdat, a.canceldat, h.cusnc, a.cancelmemo " &_
			"from 	RTSparq499CustAR a " &_
			"inner join RTSparq499Cust b on a.cusid = b.cusid " &_
			"inner join RTSparq499CmtyH c on c.comq1 = b.comq1 " &_
			"left outer join RTCode d on a.artype = d.CODE AND d.KIND = 'N2' " &_
			"left outer join RTEmployee e inner join RTObj f on f.cusid = e.cusid on e.emply = a.musr " &_
			"left outer join RTEmployee g inner join RTObj h on h.cusid = g.cusid on g.emply = a.cancelusr " &_
			"where " & searchQry2 &_ 
			" order by  a.CDAT " 

  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>