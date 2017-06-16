<style>
<!--
.toChar
	{font-size:10.0pt;mso-number-format:"\@";border:0.5pt solid black;}
.toNum
	{font-size:10.0pt;border:0.5pt solid black;}
.toNumY
	{font-size:10.0pt;border:0.5pt solid black;background:peachpuff;}
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
    'sql="usp_RTFTTBApply '" & v(0) &"', '"& v(1) &"' "
    sql="usp_RTFTTBApply 0,'" & v(0) &"' "
   rs.Open sql, CONN
   
'response.Write sqlstr    
    
    ' 明細表 ===================================================================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=HiBuilding客戶移轉FTTB清單"
	response.Write "<table>"
	response.Write "<tr><td align =center colspan=8><b>元訊寬頻網路股份有限公司</b></td></tr>"
	response.Write "<tr><td align =center colspan=8><b>FTTB申請清單</b></td></tr>"
	'response.Write "<tr><td align =left colspan=8><font size=2><u>統計期間"&v(0)&" 至 "&v(1)&" </u></font></td></tr>"
	response.Write "<tr><td align =right colspan=8><font size=2>製表日期：" &now()& "</font></td></tr>"

	response.Write "<TR>" &_
		"<td class=titleN align=""center"">社區名稱</td>" &_
		"<td class=titleN align=""center"">客戶名稱</td>" &_
		"<td class=titleY align=""center"">HN號碼</td>" &_
		"<td class=titleN align=""center"">聯絡人</td>" &_
		"<td class=titleN align=""center"">TEL(H)</td>" &_
		"<td class=titleN align=""center"">TEL(O)</td>" &_
		"<td class=titleN align=""center"">行動電話</td>" &_
		"<td class=titleN align=""center"">裝機地址</td>" &_
		"</TR>"
		
	Do While Not rs.Eof
		response.Write "<TR>" &_
			"<td class=tochar>"& rs("COMN") &"</td>" &_
			"<td class=tochar>"& rs("CUSNC") &"</td>" &_
			"<td class=tochar>"& rs("CUSNO") &"</td>" &_
			"<td class=tochar>"& rs("CONTACT") &"</td>" &_
			"<td class=tochar>"& rs("HOME") &"</td>" &_
			"<td class=tochar>"& rs("OFFICE") &"</td>" &_
			"<td class=tochar>"& rs("MOBILE") &"</td>" &_
			"<td class=tochar>"& rs("ADDR1") &"</td>" &_
			"</TR>"
		rs.MoveNext      
    	Loop
	response.Write "</table>"  
    
	rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
%>
