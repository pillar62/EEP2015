<style>
<!--
.tochar
	{font-size:10.0pt;mso-number-format:"\@";}
.titleY
	{font-size:10.0pt;font-weight:bold;background:peachpuff;}
.titleN
	{font-size:10.0pt;font-weight:bold;background:silver;}
-->
</style>

<%
    parm=request("parm")
    v=split(parm,";")

    Dim rs,conn, formid
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
    sql="SELECT	case f.PARM1 when 'AA' then h.AREANC else f.CODENC end as BREAKNC, "&_
		"		b.comn, d.cusnc, a.CUSNO, usekind, a.IP, isnull(e.cutnc,'')+ a.township1+a.raddr1 as ADDR, "&_
		"		a.FINISHDAT, a.docketdat, dropdat "&_
		"FROM	rtcust a "&_
		"inner join rtcmty b on a.comq1 = b.comq1 "&_
		"inner join rtobj d on d.cusid = a.cusid "&_
		"left outer join rtcode c on c.code = b.comtype and c.kind='B3' "&_
		"left outer join rtcounty e on e.cutid = a.cutid1 "&_
		"left outer join RTCode f on f.code = b.comtype and f.kind ='B3' "&_
		"left outer join RTCtyTown g inner join RTArea h "&_
		"	on h.AREAID = g.AREAID and h.AREATYPE ='3' on g.CUTID = b.CUTID and g.TOWNSHIP = b.TOWNSHIP "&_
		"WHERE	left(ltrim(a.ip),2) between '00' and '99'  AND a.FREECODE <> 'Y' "&_
		"AND	a.docketdat between '" &v(0)& "' and '" &v(1)& "' "&_
		"ORDER BY f.PARM1, 1, c.codenc, b.comn "
    rs.Open sql, CONN
    
    ' EXCEL 檔 ==========================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename="& v(2)
	response.Write "<table border=2>"
	'response.Write "<tr><td align =center colspan=50><b>元訊電子轉檔報竣及異動</b></td></tr>"
	'response.Write "<tr><td colspan=50><font size=2>文檔日期：" &now()& "</font></td></tr>"
    response.Write "<TR>" &_
				   "<td class=titleN>經銷商</td>" &_
				   "<td class=titleN>社區名稱</td>" &_
				   "<td class=titleY>用戶名稱</td>" &_
				   "<td class=titleY>用戶HN</td>" &_
				   "<td class=titleY>連接方式</td>" &_
				   "<td class=titleY>用戶IP</td>" &_
				   "<td class=titleY>裝機地址</td>" &_
				   "<td class=titleY>完工日</td>" &_
				   "<td class=titleY>報竣日</td>" &_
				   "<td class=titleY>退租日</td>" &_
				   "</TR>"
	Do While Not rs.Eof
    response.Write "<TR>" &_
				   "<td class=tochar>"& rs("BREAKNC") &"</td>" &_
				   "<td class=tochar>"& rs("COMN") &"</td>" &_
				   "<td class=tochar>"& rs("CUSNC") &"</td>" &_
				   "<td class=tochar>"& rs("CUSNO") &"</td>" &_
				   "<td class=tochar>"& rs("USEKIND") &"</td>" &_
				   "<td class=tochar>"& rs("IP") &"</td>" &_
				   "<td class=tochar>"& rs("ADDR") &"</td>" &_
				   "<td class=tochar>"& rs("FINISHDAT") &"</td>" &_
				   "<td class=tochar>"& rs("DOCKETDAT") &"</td>" &_
				   "<td class=tochar>"& rs("DROPDAT") &"</td>" &_
				   "</TR>"
       rs.MoveNext
    Loop
	response.Write "</table>"   
   
    rs.Close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
