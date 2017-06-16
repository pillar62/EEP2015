<style>
<!--
.toChar
	{font-size:10.0pt;mso-number-format:"\@";border:0.5pt solid black;}
.toNum
	{font-size:10.0pt;border:0.5pt solid black;}
.titleY
	{font-size:10.0pt;font-weight:bold;background:peachpuff;border:0.5pt solid black;}
.titleN
	{font-size:10.0pt;font-weight:bold;background:silver;border:1.0pt solid black;}
-->
</style>

<%
    parm=request("parm")
    v=split(parm,";")

    Dim rs,conn, formid
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
	sqlstr="usp_RTSparq0809ApplyList '" &v(0)& "','" &v(1)& "' "
'response.Write sqlstr    
    rs.Open sqlstr, CONN
    
    
    ' 明細表 ===================================================================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=速博0809申請清單.xls"
	response.Write "<table>"
	response.Write "<tr><td align =left colspan=7><b>TO:速博開通中心 - 有話好說輕鬆省專案</b></td></tr>"
	response.Write "<tr><td align =left colspan=7><font size=2>業務單位 元訊寬頻網路股份有限公司</font></td></tr>"
	response.Write "<tr><td align =left colspan=7><font size=2>傳真人員 曹姿怡 02-26552888 分機 311</font></td></tr>"
	response.Write "<tr><td align =left colspan=7><font size=2>傳真日期：" &now()& "</font></td></tr>"
	response.Write "<tr><td align =left colspan=6><font size=2>傳真號碼： (02)2655-2940</b></td>"
	response.Write "<td align =right><font size=2><u>申請日"& v(0)&"～"& v(1)&"用戶</u></font></td></tr>"
    response.Write "<TR>" &_
    			   "<td class=titleY>No</td>" &_
				   "<td class=titleY>用戶名稱</td>" &_
				   "<td class=titleY>申請服務項目</td>" &_
				   "<td class=titleY>申請書份數</td>" &_
				   "<td class=titleY>申請書及<BR>相關附件頁</td>" &_
				   "<td class=titleY>備註(載明附件內容)</td>" &_
				   "<td class=titleY>帳單地址</td>" &_
				   "</TR>"
	SERNO =0
	'BonusSum = 0
	Do While Not rs.Eof
		SERNO = SERNO +1
		'BonusSum = BonusSum + rs("BONUS")
	    response.Write "<TR>" &_
				   "<td class=toNum>"& SERNO &"</td>" &_
				   "<td class=tochar>"& rs("CUSNC") &"</td>" &_
				   "<td class=tochar>"& rs("svitem") &"</td>" &_				   
				   "<td class=toNum>"& rs("APFormNum") &"</td>" &_
				   "<td class=toNum>"& rs("AttNum") &"</td>" &_
				   "<td class=tochar>"& rs("attachment") &"</td>" &_
				   "<td class=tochar>"& rs("addr2") &"</td>" &_
				   "</TR>"
      rs.MoveNext
    Loop
    rs.Close
    
'    response.Write "<TR>" &_
'				   "<td class=titleN>&nbsp; </td>" &_
'				   "<td class=titleN>合計</td>" &_
'   				   "<td class=titleN>&nbsp; </td>" &_				   
'				   "<td class=titleN>&nbsp; </td>" &_
'				   "<td class=titleN>&nbsp; </td>" &_
'				   "<td class=titleN>&nbsp; </td>" &_
'				   "<td class=titleN>&nbsp; </td>" &_
'				   "<td class=titleN>&nbsp; </td>" &_				   
'				   "</TR>"
	response.Write "</table><BR><BR>"  
	response.Write "OM簽收人 ______________<BR><BR>" &_
					"OM簽收日 ______________"
	

'' 小計表 ========================================================================================
'    sqlstr="select	Isnull(d.CUSNC, '陳祈良') as EMPLY, Count(*) as NUM, Count(*) * 300  as Bonus "_
'		  &"from	KTSCust a "_
'		  &"		left outer join RTEmployee c inner join RTObj d on d.CUSID = c.CUSID on c.EMPLY = a.EMPLY "_
'		  &"WHERE	a.APPLYDAT Between '"& v(0) &"' and '"& v(1) &"' "_
'		  &"group by Isnull(d.CUSNC, '陳祈良') "
''response.Write sqlstr    
'    rs.Open sqlstr, CONN
'
'	response.Write "<br><br><table>"	
'   response.Write "<TR>" &_
'				   "<td colspan=4 style='mso-ignore:colspan'></td>" &_
'   				   "<td class=titleY>專案人員</td>" &_
'				   "<td class=titleY>開通件數</td>" &_
'				   "<td class=titleY>開通獎金($300/件)</td>" &_				   
'				   "</TR>"
'	SERNO =0
'	BonusSum = 0
'	Do While Not rs.Eof
'		SERNO = SERNO + rs("NUM")
'		BonusSum = BonusSum + rs("BONUS")
'	    response.Write "<TR>" &_
'				   "<td colspan=4 style='mso-ignore:colspan'></td>" &_
'				   "<td class=tochar>"& rs("EMPLY") &"</td>" &_
'				   "<td class=toNum>"& rs("NUM") &"</td>" &_
'				   "<td class=toNum>"& rs("BONUS") &"</td>" &_
'				   "</TR>"
'      rs.MoveNext
'    Loop
'    rs.Close
'	conn.Close
'	set rs = nothing
'	set conn = nothing
	
'    response.Write "<TR>" &_
'				   "<td colspan=4 style='mso-ignore:colspan'></td>" &_
'				   "<td class=titleN>合計</td>" &_
'				   "<td class=titleN>"& SERNO&"</td>" &_
'				   "<td class=titleN>"& BonusSum &"</td>" &_
'				   "</TR></table>"
	'response.Write "<br><br><font size=2>" &_
	'			   "總經理：　　　　　　　副總經理：　　　　　　　部門主管：　　　　　　　單位主管：　　　　　　　製表人：</font>"
%>
