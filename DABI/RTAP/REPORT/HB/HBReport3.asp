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
    'parm=request("parm")
    'v=split(parm,";")

    Dim rs,conn, formid
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
    'rs.Open "usp_RTSparqFtp '2','" & v(0) &"'", CONN
    rs.Open "SELECT 	case c.PARM1 when 'AA' then '直銷' else '經銷' end as BREAKID, "&_
			"case c.PARM1 when 'AA' then e.AREANC else c.CODENC end AREANC, "&_
			"isnull(k.CUSNC,'') as SALESNC, b.COMN, f.CUSNC, "&_
			"a.RCVD, a.FINISHDAT, a.DOCKETDAT, a.IP, "&_
			"isnull(g.CUTNC, '') +a.TOWNSHIP1+a.RADDR1 as CUSTADDR "&_
			"FROM	RTCust a "&_
			"inner join RTCmty b on a.COMQ1 = b.COMQ1 "&_
			"left outer join RTCode c on b.COMTYPE = c.CODE and c.KIND ='B3' "&_
			"left outer join RTCtyTown d inner join RTArea e on d.AREAID = e.AREAID on b.CUTID = d.CUTID and b.TOWNSHIP = d.TOWNSHIP "&_
			"left outer join RTObj f on f.CUSID = a.CUSID "&_
			"left outer join RTCounty g on g.CUTID = a.CUTID1 "&_
			"left outer join RTCtyTownSale h inner join (select cutid, township,  max(tdat) as maxtdat from RTCtyTownSale  where cusid <>'' group by cutid, township) i "&_
			"on h.CUTID = i.CUTID and h.TOWNSHIP = i.TOWNSHIP and h.TDAT = i.MAXTDAT "&_
			"on h.CUTID = a.CUTID1 and h.TOWNSHIP = a.TOWNSHIP1 "&_
			"left outer join rtobj k on k.CUSID = h.CUSID "&_
			"WHERE	b.T1APPLY is not Null AND b.RCOMDROP is null and a.DROPDAT is null "&_
			"AND	(a.DOCKETDAT is null or a.FINISHDAT is null) "&_
			"AND	left(ltrim(a.IP),1) between '0' and '9' "&_
			"ORDER BY 1, 2, b.COMN ", CONN
    
    ' 文字檔  =============================================
	'Do While Not rs.Eof
    '   response.Write rs("ftptext") & "<br><br>"
    '   rs.MoveNext
    'Loop
    
    ' EXCEL 檔 ==========================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=計量制有ip無完工或無報竣一覽表.xls"
	response.Write "<table border=2>"
	response.Write "<tr><td align =center colspan=10><b>計量制有ip無完工或無報竣一覽表</b></td></tr>"
	response.Write "<tr><td colspan=10><font size=2>建檔日期：" &now()& "</font></td></tr>"
    response.Write "<TR>" &_
				   "<td class=titleN>直/經銷</td>" &_
				   "<td class=titleN>轄區</td>" &_
				   "<td class=titleN>業務員</td>" &_
				   "<td class=titleN>社區名稱</td>" &_
				   "<td class=titleN>客戶名稱</td>" &_
				   "<td class=titleN>申請日</td>" &_
				   "<td class=titleY>完工日</td>" &_
				   "<td class=titleY>報竣日</td>" &_
				   "<td class=titleN>IP</td>" &_
				   "<td class=titleN>客戶地址</td>" &_
				   "</TR>"
	Do While Not rs.Eof
    response.Write "<TR>" &_
				   "<td class=tochar>"& rs("BREAKID") &"</td>" &_
				   "<td class=tochar>"& rs("AREANC") &"</td>" &_
				   "<td class=tochar>"& rs("SALESNC") &"</td>" &_
				   "<td class=tochar>"& rs("COMN") &"</td>" &_
				   "<td class=tochar>"& rs("CUSNC") &"</td>" &_
				   "<td class=tochar>"& rs("RCVD") &"</td>" &_
				   "<td class=tochar>"& rs("FINISHDAT") &"</td>" &_
				   "<td class=tochar>"& rs("DOCKETDAT") &"</td>" &_
				   "<td class=tochar>"& rs("IP") &"</td>" &_
				   "<td class=tochar>"& rs("CUSTADDR") &"</td>" &_
				   "</TR>"
       rs.MoveNext
    Loop
	response.Write "</table>"   
   
    rs.Close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
