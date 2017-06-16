<style>
<!--
.toChar
	{font-size:10.0pt;mso-number-format:"\@";border:0.5pt solid black;}
.toNum
	{font-size:10.0pt;border:0.5pt solid black;}
.toNumY
	{font-size:10.0pt;border:0.5pt solid black;background:peachpuff;}
.titleY
	{font-size:10.0pt;font-weight:bold;background:peachpuff;border:1.0pt solid black;}
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
    sql="usp_RTEngCmtyCustCount 'E', '" & v(0) &"' "
   rs.Open sql, CONN
   
'response.Write sqlstr    
    
    ' 明細表 ===================================================================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=工務人員各方案戶數統計.xls"
	response.Write "<table>"
	response.Write "<tr><td align =center colspan=35><b>元訊寬頻網路股份有限公司</b></td></tr>"
	response.Write "<tr><td align =center colspan=35><b>工務人員各方案戶數統計</b></td></tr>"
	response.Write "<tr><td align =left colspan=35><font size=2><u>統計期間 至： "&v(0)&" </u></font></td></tr>"
	response.Write "<tr><td align =right colspan=35><font size=2>製表日期：" &now()& "</font></td></tr>"
	
	response.Write "<TR>" &_
		"<td class=titleN align=""center"" rowspan=2>轄區</td>" &_
		"<td class=titleN align=""center"" rowspan=2>工程師</td>" &_
		"<td class=titleY align=""center"" colspan=2>速博方案</td>" &_
		"<td class=titleN align=""center"" colspan=3>AVS經濟型</td>" &_
		"<td class=titleN align=""center"" colspan=3>AVS專業型</td>" &_
		"<td class=titleN align=""center"" colspan=3>專案250</td>" &_		
		"<td class=titleN align=""center"" colspan=3>專案200</td>" &_
		"<td class=titleN align=""center"" colspan=3>AVS標準型</td>" &_
		"<td class=titleN align=""center"" colspan=3>專案400</td>" &_
		"<td class=titleN align=""center"">AVS</td>" &_
		"<td class=titleY align=""center"" colspan=5>ET經濟型</td>" &_
		"<td class=titleY align=""center"" colspan=3>ET專業型</td>" &_
		"<td class=titleY align=""center"" colspan=3>ET標準型</td>" &_		
		"<td class=titleY align=""center"">ET</td>" &_
		"</TR>"

	response.Write "<TR>" &_
		"<td class=titleY align=""center"">SP399</td>" &_
		"<td class=titleY align=""center"">SP499</td>" &_
		"<td class=titleN align=""center"">季繳</td>" &_
		"<td class=titleN align=""center"">半年繳</td>" &_
		"<td class=titleN align=""center"">年繳</td>" &_
		"<td class=titleN align=""center"">季繳</td>" &_
		"<td class=titleN align=""center"">半年繳</td>" &_
		"<td class=titleN align=""center"">年繳</td>" &_
		"<td class=titleN align=""center"">季繳</td>" &_
		"<td class=titleN align=""center"">半年繳</td>" &_
		"<td class=titleN align=""center"">年繳</td>" &_
		"<td class=titleN align=""center"">季繳</td>" &_
		"<td class=titleN align=""center"">半年繳</td>" &_
		"<td class=titleN align=""center"">年繳</td>" &_
		"<td class=titleN align=""center"">季繳</td>" &_
		"<td class=titleN align=""center"">半年繳</td>" &_
		"<td class=titleN align=""center"">年繳</td>" &_
		"<td class=titleN align=""center"">季繳</td>" &_
		"<td class=titleN align=""center"">半年繳</td>" &_
		"<td class=titleN align=""center"">年繳</td>" &_
		"<td class=titleN align=""center"">其他</td>" &_
		"<td class=titleY align=""center"">季繳</td>" &_
		"<td class=titleY align=""center"">半年繳</td>" &_
		"<td class=titleY align=""center"">年繳</td>" &_
		"<td class=titleY align=""center"">兩年繳</td>" &_
		"<td class=titleY align=""center"">三年繳</td>" &_
		"<td class=titleY align=""center"">季繳</td>" &_
		"<td class=titleY align=""center"">半年繳</td>" &_
		"<td class=titleY align=""center"">年繳</td>" &_
		"<td class=titleY align=""center"">季繳</td>" &_
		"<td class=titleY align=""center"">半年繳</td>" &_
		"<td class=titleY align=""center"">年繳</td>" &_
		"<td class=titleY align=""center"">其他</td>" &_
		"</TR>"
		
	Do While Not rs.Eof
		response.Write "<TR>" &_
			"<td class=tochar>"& rs("areanc") &"</td>" &_
			"<td class=tochar>"& rs("engineernc") &"</td>" &_
			"<td class=tonum>"& rs("sp399") &"</td>" &_
			"<td class=tonum>"& rs("sp499") &"</td>" &_
			"<td class=tonum>"& rs("avs0105") &"</td>" &_
			"<td class=tonum>"& rs("avs0101") &"</td>" &_
			"<td class=tonum>"& rs("avs0102") &"</td>" &_
			"<td class=tonum>"& rs("avs0205") &"</td>" &_
			"<td class=tonum>"& rs("avs0201") &"</td>" &_
			"<td class=tonum>"& rs("avs0202") &"</td>" &_
			"<td class=tonum>"& rs("avs0305") &"</td>" &_
			"<td class=tonum>"& rs("avs0301") &"</td>" &_
			"<td class=tonum>"& rs("avs0302") &"</td>" &_
			"<td class=tonum>"& rs("avs0405") &"</td>" &_
			"<td class=tonum>"& rs("avs0401") &"</td>" &_
			"<td class=tonum>"& rs("avs0402") &"</td>" &_
			"<td class=tonum>"& rs("avs0505") &"</td>" &_
			"<td class=tonum>"& rs("avs0501") &"</td>" &_
			"<td class=tonum>"& rs("avs0502") &"</td>" &_
			"<td class=tonum>"& rs("avs0605") &"</td>" &_
			"<td class=tonum>"& rs("avs0601") &"</td>" &_
			"<td class=tonum>"& rs("avs0602") &"</td>" &_
			"<td class=tonum>"& rs("avsETC") &"</td>" &_
			"<td class=tonum>"& rs("et0105") &"</td>" &_
			"<td class=tonum>"& rs("et0101") &"</td>" &_
			"<td class=tonum>"& rs("et0102") &"</td>" &_
			"<td class=tonum>"& rs("et0103") &"</td>" &_
			"<td class=tonum>"& rs("et0104") &"</td>" &_
			"<td class=tonum>"& rs("et0205") &"</td>" &_
			"<td class=tonum>"& rs("et0201") &"</td>" &_
			"<td class=tonum>"& rs("et0202") &"</td>" &_
			"<td class=tonum>"& rs("et0505") &"</td>" &_
			"<td class=tonum>"& rs("et0501") &"</td>" &_
			"<td class=tonum>"& rs("et0502") &"</td>" &_
			"<td class=tonum>"& rs("etETC") &"</td>" &_
			"</TR>"
		rs.MoveNext      
    	Loop
	response.Write "</table>"  
    
	rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
%>

