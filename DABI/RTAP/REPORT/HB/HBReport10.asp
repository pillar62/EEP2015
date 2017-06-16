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
    v(0)=v(0) & "%"

    Dim rs,conn, formid
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
    sql="select	'CHT 599' as casetype, a.comn, isnull(g.cusnc, '') as salesman, count(*) as num, isnull(h.cutnc,'')+a.township+a.addr as RADDR "_
	&"from 	rtcmty a "_
	&"	inner join rtcode b on a.comtype = b.code and b.kind ='B3' and b.parm1 ='AA' "_
	&"	inner join rtcust c on a.comq1 = c.comq1 "_
	&"	inner join rtctytown d on d.cutid = a.cutid and d.township = a.township and d.areaid like '"& v(0) &"' "_
	&"	left outer join rtcmtysale e inner join (select comq1, max(seq) as maxseq from rtcmtysale group by comq1) f "_
	&"	on f.comq1 = e.comq1 and f.maxseq = e.seq on e.comq1 = a.comq1 "_
	&"	left outer join rtobj g on g.cusid = e.cusid "_
	&"	left outer join rtcounty h on h.cutid = a.cutid "_
	&"where	a.rcomdrop is null and c.dropdat is null and c.docketdat is not null and c.freecode<>'Y' "
	if v(1)	="台北縣市" then
		sql = sql & "and a.CUTID in ('03','04') "
	end if
sql=sql &"group by comn, isnull(g.cusnc, ''), isnull(h.cutnc,'') +a.township +a.addr "_
	&"union	"_
	&"select 'CHT 399' as casetype, a.comn,  isnull(f.cusnc, '') as salesman, count(*) as NUM, isnull(h.cutnc,'') +a.township +a.addr as RADDR "_
	&"from 	rtcustadslcmty a "_
	&"	inner join rtcustadsl b on a.cutyid = b.comq1 "_
	&"	inner join rtcode c on a.comtype = c.code and c.kind ='B3' and c.parm1 ='AA' "_
	&"	inner join rtctytown d on d.cutid = a.cutid and d.township = a.township and d.areaid like '"& v(0) &"' "_
	&"	left outer join rtemployee e inner join rtobj f on f.cusid = e.cusid on e.emply = a.bussid "_
	&"	left outer join rtcounty h on h.cutid = a.cutid "_
	&"where	a.rcomdrop is null and b.dropdat is null and b.docketdat is not null and b.freecode<>'Y' "
	if v(1)	="台北縣市" then
		sql = sql & "and a.CUTID in ('03','04') "
	end if
sql=sql &"group by comn, isnull(f.cusnc, ''), isnull(h.cutnc,'') +a.township +a.addr "_	
	&"union	"_
	&"select 'Sparq 399' as casetype, a.comn,  isnull(f.cusnc, '') as salesman, count(*) as NUM, isnull(h.cutnc,'') +a.township +a.addr  as RADDR "_
	&"from 	rtsparqadslcmty a "_
	&"	inner join rtsparqadslcust b on a.cutyid = b.comq1 "_
	&"	inner join rtctytown d on d.cutid = a.cutid and d.township = a.township and d.areaid like '"& v(0) &"' "_
	&"	left outer join rtemployee e inner join rtobj f on f.cusid = e.cusid on e.emply = a.bussid "_
	&"	left outer join rtcounty h on h.cutid = a.cutid "_
	&"where	a.rcomdrop is null and b.dropdat is null and b.docketdat is not null and b.freecode<>'Y' "_
	&"and	 a.consignee ='' "
	if v(1)	="台北縣市" then
		sql = sql & "and a.CUTID in ('03','04') "
	end if
sql=sql &"group by comn,  isnull(f.cusnc, ''), isnull(h.cutnc,'') +a.township +a.addr "_
	&"union	"_
	&"select 'EBT' as casetype, b.comn,  isnull(f.cusnc, '') as salesman,  count(*) as num, isnull(h.cutnc,'') +a.township  as RADDR "_
	&"from 	rtebtcmtyline a "_
	&"	inner join rtebtcmtyh b on a.comq1 = b.comq1 "_
	&"	inner join rtebtcust c on c.comq1 = a.comq1 and a.lineq1 = c.lineq1 "_
	&"	inner join rtctytown d on d.cutid = a.cutid and d.township = a.township and d.areaid like '"& v(0) &"' "_
	&"	left outer join rtemployee e inner join rtobj f on f.cusid = e.cusid on e.emply = a.salesid "_
	&"	left outer join rtcounty h on h.cutid = a.cutid "_
	&"where	 a.consignee ='' "_
	&"and 	a.dropdat is null and a.adslapplydat is not null "_
	&"and	c.docketdat is not null and c.dropdat is null and c.freecode<>'Y' "
	if v(1)	="台北縣市" then
		sql = sql & "and a.CUTID in ('03','04') "
	end if
sql=sql &"group by comn, isnull(f.cusnc, ''), isnull(h.cutnc,'') +a.township "_
	&"order by 1, 2	"

    rs.Open sql, CONN
    
    ' 文字檔  =============================================
	'Do While Not rs.Eof
    '   response.Write rs("ftptext") & "<br><br>"
    '   rs.MoveNext
    'Loop
    
    ' EXCEL 檔 ==========================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename="& v(1) &"直銷社區.xls"
	response.Write "<table border=2>"
	'response.Write "<tr><td align =center colspan=50><b>元訊電子轉檔報竣及異動</b></td></tr>"
	'response.Write "<tr><td colspan=50><font size=2>文檔日期：" &now()& "</font></td></tr>"
    response.Write "<TR>" &_
				   "<td class=titleY>方案別</td>" &_
				   "<td class=titleY>社區名稱</td>" &_
				   "<td class=titleY>業務</td>" &_
				   "<td class=titleY>客戶數</td>" &_
				   "<td class=titleN>地址</td>" &_
				   "</TR>"
	Do While Not rs.Eof
    response.Write "<TR>" &_
				   "<td class=tochar>"& rs("CASETYPE") &"</td>" &_
				   "<td class=tochar>"& rs("COMN") &"</td>" &_
				   "<td class=tochar>"& rs("SALESMAN") &"</td>" &_
				   "<td class=tochar>"& rs("NUM") &"</td>" &_
				   "<td class=tochar>"& rs("RADDR") &"</td>" &_
				   "</TR>"
       rs.MoveNext
    Loop
	response.Write "</table>"   
   
    rs.Close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
