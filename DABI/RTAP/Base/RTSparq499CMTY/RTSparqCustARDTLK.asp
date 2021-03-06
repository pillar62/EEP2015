<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="速博管理系統"
  title="速博用戶應收應付帳款明細查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""
  functionoptopen=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;項次;會計科目;科目名稱;立帳年月;認列年月;項目名稱;正/負;應收(付)金額;已沖銷金額;未沖帳金額;產生日;沖帳日;作廢日;作廢原因"
  sqlDelete="SELECT     RTSparq499CustARDTL.CUSID, RTSparq499CustARDTL.BATCHNO, " _
                        &"  RTSparq499CustARDTL.SEQ, " _
                        &"  RTSparq499CustARDTL.L14 + '-' + RTSparq499CustARDTL.L23 AS Expr2, " _
                        &"  RTAccountNo.ACNAMEC, RTSparq499CustARDTL.ITEMNC, " _
                        &"  RTSparq499CustARDTL.PORM, RTSparq499CustARDTL.AMT, " _
                        &"  RTSparq499CustARDTL.REALAMT, " _
                        &"  RTSparq499CustARDTL.AMT - RTSparq499CustARDTL.REALAMT AS Expr1, " _
                        &"  RTSparq499CustARDTL.CDAT, RTSparq499CustARDTL.MDAT, " _
                        &"  RTSparq499CustARDTL.CANCELDAT, RTSparq499CustARDTL.CANCELMEMO " _
           &"FROM           RTSparq499CustARDTL LEFT OUTER JOIN " _
                        &"  RTAccountNo ON RTSparq499CustARDTL.L14 = RTAccountNo.L14 AND " _
                        &"  RTSparq499CustARDTL.L23 = RTAccountNo.L23 " _
           &"where RTSparq499CustARDTL.cusid='' "
  'dataTable="RTSparq499CustARDTL"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg=""
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
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
  searchProg="self"
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
  set connYY=server.CreateObject("ADODB.connection")
  set rsYY=server.CreateObject("ADODB.recordset")
  dsnYY="DSN=RTLIB"
  
  'comn -------------------------------------------------------------------------------------------------
  if ARYPARMKEY(2) ="Sparq399" then 
	sqlYY="select comn from RTCounty RIGHT OUTER JOIN RTSparqAdslCmty ON " _
		&"RTCounty.CUTID = RTSparqAdslCmty.CUTID RIGHT OUTER JOIN RTSparqAdslCust ON RTSparqAdslCmty.CUTYID = RTSparqAdslCust.COMQ1 " _
		&"where RTSparqAdslCust.cusid='" & ARYPARMKEY(0) & "'"
  elseif ARYPARMKEY(2) ="Sparq499" then 
	sqlYY="select comn from RTCounty RIGHT OUTER JOIN RTSparq499CmtyH ON " _
		&"RTCounty.CUTID = RTSparq499CmtyH.CUTID RIGHT OUTER JOIN RTSparq499Cust ON RTSparq499CmtyH.COMQ1 = RTSparq499Cust.COMQ1 " _
		&"where RTSparq499Cust.cusid='" & ARYPARMKEY(0) & "'"
  end if
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
  else
     COMN=""
  end if
  rsYY.Close
  
  'addr -------------------------------------------------------------------------------------------------
  if ARYPARMKEY(2) ="Sparq399" then 
	sqlYY="select cutnc+township+addr as cmtyaddr from RTCounty RIGHT OUTER JOIN RTSparqAdslCmty ON  " _
		&"RTCounty.CUTID = RTSparqAdslCmty.CUTID RIGHT OUTER JOIN " _
		&"RTSparqAdslCust ON RTSparqAdslCmty.cutyid = RTSparqAdslCust.COMQ1 " _
		&"where RTSparqAdslCust.cusid='" & ARYPARMKEY(0) & "'"
  elseif ARYPARMKEY(2) ="Sparq499" then 
	sqlYY="select cutnc+township+raddr as cmtyaddr from RTCounty RIGHT OUTER JOIN RTSparq499CmtyLine ON  " _
		&"RTCounty.CUTID = RTSparq499CmtyLine.CUTID RIGHT OUTER JOIN " _
		&"RTSparq499Cust ON RTSparq499CmtyLine.COMQ1 = RTSparq499Cust.COMQ1 AND " _
		&"RTSparq499CmtyLine.LINEQ1 = RTSparq499Cust.LINEQ1 " _
		&"where RTSparq499Cust.cusid='" & ARYPARMKEY(0) & "'"
  end if       
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
	 COMaddr=rsYY("cmtyaddr")
  else
     COMaddr=""
  end if
  RSYY.Close
  
  'comq1, cusnc ------------------------------------------------------------------------------------------
	if ARYPARMKEY(2) ="Sparq399" then 
		sqlYY="select b.cusnc, a.comq1 from RTSparqAdslCUST a " &_
				"inner join RTObj b on a.cusid = b.cusid Where a.CUSID='" & ARYPARMKEY(0) & "' "
	elseif ARYPARMKEY(2) ="Sparq499" then 
		sqlYY="select convert(varchar(5), comq1) +'-'+ convert(varchar(5), lineq1) as comq1, cusnc " &_
			  "from	  RTSparq499CUST where CUSID='" & ARYPARMKEY(0) & "' "
	end if       
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     CUSNC=rsYY("CUSNC")
     comq1xx=rsyy("comq1")
  else
     CUSNC=""
     comq1xx=""
  end if
  rsYY.Close
  
  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
	if ARYPARMKEY(2) ="Sparq399" then 
	     searchQry=" RTSparqAdslCustARDTL.CUSID='" & ARYPARMKEY(0) & "' AND RTSparqAdslCustARDTL.BATCHNO='" & ARYPARMKEY(1) & "' "
	elseif ARYPARMKEY(2) ="Sparq499" then 
	     searchQry=" RTSparq499CustARDTL.CUSID='" & ARYPARMKEY(0) & "' AND RTSparq499CustARDTL.BATCHNO='" & ARYPARMKEY(1) & "' "
	end if       
    searchShow="主線︰"& comq1xx & ",社區︰" & COMN & ",主線位址︰" & COMADDR & ",用戶序號︰" & aryparmkey(0) & ",用戶名稱︰" & CUSNC & ",應收帳款編號︰" & ARYPARMKEY(1)
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
	if ARYPARMKEY(2) ="Sparq399" then
		sqlList="SELECT          RTSparqAdslCustARDTL.CUSID, RTSparqAdslCustARDTL.BATCHNO, " _
							&"  RTSparqAdslCustARDTL.SEQ, " _
							&"  RTSparqAdslCustARDTL.L14 + '-' + RTSparqAdslCustARDTL.L23 AS Expr2, " _
							&"  RTAccountNo.ACNAMEC,convert(varchar(4),RTSparqAdslCustARDTL.syy)+'/'+convert(varchar(2),RTSparqAdslCustARDTL.smm),convert(varchar(4),RTSparqAdslCustARDTL.tyy)+'/'+convert(varchar(2),RTSparqAdslCustARDTL.tmm), RTSparqAdslCustARDTL.ITEMNC, " _
							&"  RTSparqAdslCustARDTL.PORM, RTSparqAdslCustARDTL.AMT, " _
							&"  RTSparqAdslCustARDTL.REALAMT, " _
							&"  RTSparqAdslCustARDTL.AMT - RTSparqAdslCustARDTL.REALAMT AS Expr1, " _
							&"  RTSparqAdslCustARDTL.CDAT, RTSparqAdslCustARDTL.MDAT, " _
							&"  RTSparqAdslCustARDTL.CANCELDAT, RTSparqAdslCustARDTL.CANCELMEMO " _
			&"FROM           RTSparqAdslCustARDTL LEFT OUTER JOIN " _
							&"  RTAccountNo ON RTSparqAdslCustARDTL.L14 = RTAccountNo.L14 AND " _
							&"  RTSparqAdslCustARDTL.L23 = RTAccountNo.L23 " _
			&"where " & searchqry & " ORDER BY RTSparqAdslCustARDTL.SEQ "
	elseif ARYPARMKEY(2) ="Sparq499" then
		sqlList="SELECT          RTSparq499CustARDTL.CUSID, RTSparq499CustARDTL.BATCHNO, " _
							&"  RTSparq499CustARDTL.SEQ, " _
							&"  RTSparq499CustARDTL.L14 + '-' + RTSparq499CustARDTL.L23 AS Expr2, " _
							&"  RTAccountNo.ACNAMEC,convert(varchar(4),RTSparq499CustARDTL.syy)+'/'+convert(varchar(2),RTSparq499CustARDTL.smm),convert(varchar(4),RTSparq499CustARDTL.tyy)+'/'+convert(varchar(2),RTSparq499CustARDTL.tmm), RTSparq499CustARDTL.ITEMNC, " _
							&"  RTSparq499CustARDTL.PORM, RTSparq499CustARDTL.AMT, " _
							&"  RTSparq499CustARDTL.REALAMT, " _
							&"  RTSparq499CustARDTL.AMT - RTSparq499CustARDTL.REALAMT AS Expr1, " _
							&"  RTSparq499CustARDTL.CDAT, RTSparq499CustARDTL.MDAT, " _
							&"  RTSparq499CustARDTL.CANCELDAT, RTSparq499CustARDTL.CANCELMEMO " _
			&"FROM           RTSparq499CustARDTL LEFT OUTER JOIN " _
							&"  RTAccountNo ON RTSparq499CustARDTL.L14 = RTAccountNo.L14 AND " _
							&"  RTSparq499CustARDTL.L23 = RTAccountNo.L23 " _
			&"where " & searchqry & " ORDER BY RTSparq499CustARDTL.SEQ "
	end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>