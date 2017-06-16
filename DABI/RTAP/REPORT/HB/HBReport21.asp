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
    sql="usp_RTChtCmtyCustCnt '" & v(0) &"' "
   rs.Open sql, CONN
   
'response.Write sqlstr    
    
    ' 明細表 ===================================================================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=CHT599及399各組累總表"
	response.Write "<table>"
	response.Write "<tr><td align =center colspan=6><b>元訊寬頻網路股份有限公司</b></td></tr>"
	response.Write "<tr><td align =center colspan=6><b>CHT599及399各組累總表</b></td></tr>"
	response.Write "<tr><td align =left colspan=6><font size=2><u>統計期間 至： "&v(0)&" </u></font></td></tr>"
	response.Write "<tr><td align =right colspan=6><font size=2>製表日期：" &now()& "</font></td></tr>"
	
	response.Write "<TR>" &_
		"<td class=titleN align=""center"">方案別</td>" &_
		"<td class=titleN align=""center"">直/經銷</td>" &_
		"<td class=titleN align=""center"">組別/經銷商</td>" &_
		"<td class=titleY align=""center"">送件數</td>" &_
		"<td class=titleY align=""center"">開通數</td>" &_
		"<td class=titleY align=""center"">報竣數</td>" &_
		"</TR>"
		
	Do While Not rs.Eof
		response.Write "<TR>" &_
			"<td class=tochar>"& rs("CASETYPE") &"</td>" &_
			"<td class=tochar>"& rs("BELONGID") &"</td>" &_
			"<td class=tochar>"& rs("BELONGNC") &"</td>" &_
			"<td class=tonum>"& rs("PSNUM") &"</td>" &_
			"<td class=tonum>"& rs("OPENNUM") &"</td>" &_
			"<td class=tonum>"& rs("DOCKETNUM") &"</td>" &_
			"</TR>"
		rs.MoveNext      
    	Loop
	response.Write "</table>"  
    
	rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
%>
