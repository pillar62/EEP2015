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
    sql="usp_RTFaqListTY '" & v(0) &"', '"& v(1) &"' "
   rs.Open sql, CONN
   
'response.Write sqlstr    
    
    ' 明細表 ===================================================================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=桃竹苗客訴一覽表"
	response.Write "<table>"
	response.Write "<tr><td align =center colspan=13><b>元訊寬頻網路股份有限公司</b></td></tr>"
	response.Write "<tr><td align =center colspan=13><b>桃竹苗客訴一覽表</b></td></tr>"
	response.Write "<tr><td align =left colspan=13><font size=2><u>統計期間"&v(0)&" 至 "&v(1)&" </u></font></td></tr>"
	response.Write "<tr><td align =right colspan=13><font size=2>製表日期：" &now()& "</font></td></tr>"
	
	response.Write "<TR>" &_
		"<td class=titleN align=""center"">歸屬</td>" &_
		"<td class=titleN align=""center"">案件編號</td>" &_
		"<td class=titleN align=""center"">社區類別</td>" &_
		"<td class=titleN align=""center"">鄉鎮</td>" &_
		"<td class=titleN align=""center"">社區名稱</td>" &_
		"<td class=titleN align=""center"">客戶名稱</td>" &_
		"<td class=titleY align=""center"">受理時間</td>" &_
		"<td class=titleY align=""center"">受理人</td>" &_
		"<td class=titleN align=""center"">排除員工</td>" &_
		"<td class=titleN align=""center"">排除廠商</td>" &_
		"<td class=titleY align=""center"">處理日期</td>" &_
		"<td class=titleY align=""center"">處理人員</td>" &_
		"<td class=titleY align=""center"">處理措施</td>" &_
		"</TR>"
		
	Do While Not rs.Eof
		response.Write "<TR>" &_
			"<td class=tochar>"& rs("belongNC") &"</td>" &_
			"<td class=tochar>"& rs("caseno") &"</td>" &_
			"<td class=tochar>"& rs("casetype") &"</td>" &_
			"<td class=tochar>"& rs("areanc") &"</td>" &_
			"<td class=tochar>"& rs("comn") &"</td>" &_
			"<td class=tochar>"& rs("faqman") &"</td>" &_
			"<td class=tochar>"& rs("rcvdate") &"</td>" &_
			"<td class=tochar>"& rs("rcvusr") &"</td>" &_
			"<td class=tochar>"& rs("finishusr") &"</td>" &_
			"<td class=tochar>"& rs("finishfac") &"</td>" &_
			"<td class=tochar>"& rs("logdate") &"</td>" &_
			"<td class=tochar>"& rs("logusr") &"</td>" &_
			"<td class=tochar>"& rs("logdesc") &"</td>" &_
			"</TR>"
		rs.MoveNext      
    	Loop
	response.Write "</table>"  
    
	rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
%>
