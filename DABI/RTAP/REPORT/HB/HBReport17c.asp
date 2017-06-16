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
    sql="usp_RTEngCmtyCustCount 'C', '" & v(0) &"' "
   rs.Open sql, CONN
   
'response.Write sqlstr    
    
    ' 明細表 ===================================================================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=工務人員各方案社區線數戶數統計(含經銷).xls"
	response.Write "<table>"
	response.Write "<tr><td align =center colspan=7><b>元訊寬頻網路股份有限公司</b></td></tr>"
	response.Write "<tr><td align =center colspan=7><b>工務人員各方案社區線數戶數統計(含經銷)</b></td></tr>"
	response.Write "<tr><td align =left colspan=7><font size=2><u>統計期間 至： "&v(0)&" </u></font></td></tr>"
	response.Write "<tr><td align =right colspan=7><font size=2>製表日期：" &now()& "</font></td></tr>"
	
	response.Write "<TR>" &_
		"<td class=titleN align=""center"" >鄉鎮</td>" &_
		"<td class=titleN align=""center"">縣市</td>" &_
		"<td class=titleN align=""center"">工務人員</td>" &_
		"<td class=titleN align=""center"">方案別</td>" &_
		"<td class=titleN align=""center"">直經銷</td>" &_
		"<td class=titleY align=""center"">線數</td>" &_
		"<td class=titleY align=""center"">戶數</td>" &_
		"</TR>"
		
	Do While Not rs.Eof
		response.Write "<TR>" &_
			"<td class=tochar>"& rs("CUTNC") &"</td>" &_
			"<td class=tochar>"& rs("TOWNSHIP") &"</td>" &_
			"<td class=tochar>"& rs("CUSNC") &"</td>" &_
			"<td class=tochar>"& rs("casetype") &"</td>" &_
			"<td class=tochar>"& rs("belongnc") &"</td>" &_
			"<td class=tonum>"& rs("LINENUM") &"</td>" &_
			"<td class=tonum>"& rs("CUSTNUM") &"</td>" &_
			"</TR>"
		rs.MoveNext      
    	Loop
	response.Write "</table>"  
    
	rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
%>
