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
    sql="select	case a.consignee when '' then '直銷' ELSE '經銷' END as belong, "_
	   &"case a.consignee when '' then  e.areanc ELSE h.shortnc END as consignee, "_
	   &"isnull(j.cusnc, '') as salesnc, g.comn, b.cusnc, b.applydat, b.finishdat, b.docketdat, "_
	   &"case b.custip1 when '' then '' else b.custip1+'.'+ b.custip2+'.'+ b.custip3+'.'+ b.custip4 end as IPADDR, "_
	   &"isnull(f.cutnc,'') + b.township2 + b.raddr2 as raddr "_
	   &"from 	RTSparq499CmtyLine a "_
	   &"inner join RTSparq499Cust b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 "_
	   &"inner join RTCtyTown d inner join rtarea e on e.areaid = d.areaid and e.areatype ='3' on d.cutid = a.cutid and d.township= a.township "_
	   &"left outer join RTCounty f on f.cutid =b.cutid2 "_
	   &"inner join RTSparq499CmtyH g on g.comq1 = a.comq1 "_
	   &"left outer join rtobj h on h.cusid = a.consignee "_
	   &"left outer join rtemployee  i inner join rtobj j on j.cusid = i.cusid on i.emply = a.salesid "_
	   &"where 	a.dropdat is null and a.canceldat is null "_
	   &"and	 b.custip1 <>'' "_
	   &"and 	(b.finishdat is null or b.docketdat is null) "_
	   &"order by 	1, 2,4,5"
'response.Write sql           
   rs.Open sql, CONN

    
    ' 明細表 ===================================================================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=速博499有IP無完工或無報竣一覽表"
	response.Write "<table>"
	response.Write "<tr><td align =center colspan=10><b>八合投資股份有限公司</b></td></tr>"
	response.Write "<tr><td align =center colspan=10><b>速博499有IP無完工或無報竣一覽表</b></td></tr>"
	'response.Write "<tr><td align =left colspan=10><font size=2><u>統計期間 至： "&v(0)&" </u></font></td></tr>"
	response.Write "<tr><td align =right colspan=10><font size=2>製表日期：" &now()& "</font></td></tr>"
	
    response.Write "<TR>" &_
    			   "<td class=titleN>直經銷</td>" &_
				   "<td class=titleN>經銷商</td>" &_
				   "<td class=titleN>業務員</td>" &_
				   "<td class=titleN>社區名稱</td>" &_
				   "<td class=titleN>客戶名稱</td>" &_
				   "<td class=titleN>申請日</td>" &_
				   "<td class=titleY>完工日</td>" &_
				   "<td class=titleY>報竣日</td>" &_
				   "<td class=titleN>IP</td>" &_
				   "<td class=titleN>客戶地址</td>" &_
				   "</TR>"
	'i=0
	Do While Not rs.Eof
		'i=i+1
	    response.Write "<TR>" &_
				   "<td class=tochar>"& rs("belong") &"</td>" &_
				   "<td class=tochar>"& rs("consignee") &"</td>" &_
				   "<td class=tochar>"& rs("salesnc") &"</td>" &_
				   "<td class=tochar>"& rs("comn") &"</td>" &_
				   "<td class=tochar>"& rs("cusnc") &"</td>" &_
				   "<td class=tochar>"& rs("applydat") &"</td>" &_
				   "<td class=tochar>"& rs("finishdat") &"</td>" &_
				   "<td class=tochar>"& rs("docketdat") &"</td>" &_
				   "<td class=tochar>"& rs("ipaddr") &"</td>" &_
				   "<td class=tochar>"& rs("raddr") &"</td>" &_				   				   
				   "</TR>"
      rs.MoveNext      
    Loop

	response.Write "</table>"  
    
    rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
%>

