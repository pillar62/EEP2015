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
    sql="usp_RTFTTBApplyList 'B', '" & v(0) &"' "
   rs.Open sql, CONN
   
'response.Write sqlstr
	if rs.RecordCount >0 then
		workplace = rs("WORKPLACE")
	end if
    
    ' 明細表 ===================================================================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=HiBuilding客戶移轉FTTB清單"
	response.Write "<table>"
	response.Write "<tr><td align =center colspan=6><b><font size=4>HiBuilding客戶移轉FTTB(ADSL)業務申請保留原Mail & my web客戶明細</font></b></td></tr>"
	response.Write "<tr><td align =left colspan=2><font size=2>申請日期：" &v(0)&"</font></td><td align=left colspan=2><font size=2>所屬營運處：" &workplace& "</font></td><td align=left colspan=2><font size=2>聯絡人：</font></td></tr>"
	'response.Write "<tr><td align =left colspan=6><font size=2><u>統計期間"&v(0)&" 至 "&v(1)&" </u></font></td></tr>"
	'response.Write "<tr><td align =right colspan=6><font size=2>製表日期：" &now()& "</font></td></tr>"

	response.Write "<TR>" &_
		"<td class=titleN align=""center"">原HiBuilding客戶名稱</td>" &_
		"<td class=titleN align=""center"">HiBuilding HN</td>" &_
		"<td class=titleN align=""center"">FTTB(ADSL)HN</td>" &_
		"<td class=titleN align=""center"">E-Mail</td>" &_
		"<td class=titleN align=""center"">保留my web(勾選)</td>" &_
		"<td class=titleN align=""center"">客戶連絡電話</td>" &_
		"</TR>"
	
	Do While Not rs.Eof
			response.Write "<TR>" &_
				"<td class=tochar>"& rs("CUSNC") &"</td>" &_
				"<td class=tochar>"& rs("HBCUSNO") &"</td>" &_
				"<td class=tochar>"& rs("FTTBCUSNO") &"</td>" &_
				"<td class=tochar>"& rs("V3EMAIL") &"</td>" &_
				"<td class=tochar>"& rs("OTHSRV4") &"</td>" &_
				"<td class=tochar>"& rs("CONTACTTEL") &"</td>" &_
				"</TR>"
		rs.MoveNext      
    Loop
    	
	response.Write "<tr></tr><tr></tr><tr><td align =left colspan=2><font size=2>中華營運處簽收回覆：</font></td><td align=left colspan=2><font size=2>中華電信營運處：" &workplace& "</font></td><td align=left colspan=2><font size=2>元訊製表人：</font></td></tr>"
	response.Write "</table>"  
    
	rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing

%>
