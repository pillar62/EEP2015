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
    sqlstr="select 	a.CUSNC, NCICCUSID, APPLYDAT, FINISHDAT, ONEPAY, 300 as BONUS, CONSIGNEE2, "_
		  &"Isnull(b.SHORTNC, '') as CONSIGNEE1, Isnull(d.CUSNC, '') as EMPLY, a.NOTATION "_
		  &"from	KTSCust a "_
	      	  &"		left outer join RTObj b on a.CONSIGNEE1 = b.CUSID "_
		  &"		left outer join RTEmployee c inner join RTObj d on d.CUSID = c.CUSID on c.EMPLY = a.EMPLY "_
		  &"WHERE	a.APPLYDAT Between '"& v(0) &"' and '"& v(1) &"' "_
		  &"order by a.APPLYDAT, a.CUSNC "
'response.Write sqlstr    
    rs.Open sqlstr, CONN
    
    
    ' 明細表 ===================================================================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=工程師獎金明細表.xls"
	response.Write "<table>"
	response.Write "<tr><td align =center colspan=11><b>元訊寬頻網路股份有限公司</b></td></tr>"
	response.Write "<tr><td align =center colspan=11><b>速博 KTS 總機語音系統 工程師獎金明細表 - 經銷開發中心</b></td></tr>"
	response.Write "<tr><td align =center colspan=11><font size=2><u>送件申請日"& v(0)&"～"& v(1)&"用戶</u></font></td></tr>"
	response.Write "<tr><td align =right colspan=11><font size=2>製表日期：" &now()& "</font></td></tr>"
    response.Write "<TR>" &_
				   "<td class=titleY>流水號</td>" &_
				   "<td class=titleY>用戶名稱</td>" &_
				   "<td class=titleY>Account ID</td>" &_				   
				   "<td class=titleY>送件申請日</td>" &_
				   "<td class=titleY>完工日期</td>" &_
				   "<td class=titleY>NCIC一次性</td>" &_
				   "<td class=titleY>獎金($300/件)</td>" &_				   
				   "<td class=titleY>經銷商</td>" &_
				   "<td class=titleY>盤商</td>" &_
				   "<td class=titleY>開發業務</td>" &_
				   "<td class=titleY>備註</td>" &_				   				   
				   "</TR>"
	SERNO =0
	BonusSum = 0
	Do While Not rs.Eof
		SERNO = SERNO +1
		BonusSum = BonusSum + rs("BONUS")
	    response.Write "<TR>" &_
				   "<td class=toNum>"& SERNO &"</td>" &_
				   "<td class=tochar>"& rs("CUSNC") &"</td>" &_				   
				   "<td class=tochar>"& rs("NCICCUSID") &"</td>" &_
				   "<td class=tochar>"& rs("APPLYDAT") &"</td>" &_
				   "<td class=tochar>"& rs("FINISHDAT") &"</td>" &_
				   "<td class=tochar>"& rs("ONEPAY") &"</td>" &_				   
				   "<td class=toNum>"& rs("BONUS") &"</td>" &_
				   "<td class=tochar>"& rs("CONSIGNEE2") &"</td>" &_
				   "<td class=tochar>"& rs("CONSIGNEE1") &"</td>" &_
				   "<td class=tochar>"& rs("EMPLY") &"</td>" &_				   
				   "<td class=tochar>"& rs("NOTATION") &"</td>" &_
				   "</TR>"
      rs.MoveNext
    Loop
    rs.Close
    
    response.Write "<TR>" &_
				   "<td class=titleN>&nbsp; </td>" &_
				   "<td class=titleN>合計</td>" &_
   				   "<td class=titleN>&nbsp; </td>" &_				   
				   "<td class=titleN>&nbsp; </td>" &_
				   "<td class=titleN>&nbsp; </td>" &_
				   "<td class=titleN>&nbsp; </td>" &_				   
				   "<td class=titleN>"& BonusSum &"</td>" &_
				   "<td class=titleN>&nbsp; </td>" &_
				   "<td class=titleN>&nbsp; </td>" &_
				   "<td class=titleN>&nbsp; </td>" &_
				   "<td class=titleN>&nbsp; </td>" &_				   
				   "</TR>"
	response.Write "</table>"  
	

' 小計表 ========================================================================================
    sqlstr="select	Isnull(d.CUSNC, '陳祈良') as EMPLY, Count(*) as NUM, Count(*) * 300  as Bonus "_
		  &"from	KTSCust a "_
		  &"		left outer join RTEmployee c inner join RTObj d on d.CUSID = c.CUSID on c.EMPLY = a.EMPLY "_
		  &"WHERE	a.APPLYDAT Between '"& v(0) &"' and '"& v(1) &"' "_
		  &"group by Isnull(d.CUSNC, '陳祈良') "
'response.Write sqlstr    
    rs.Open sqlstr, CONN

	response.Write "<br><br><table>"	
    response.Write "<TR>" &_
				   "<td colspan=4 style='mso-ignore:colspan'></td>" &_
   				   "<td class=titleY>專案人員</td>" &_
				   "<td class=titleY>開通件數</td>" &_
				   "<td class=titleY>開通獎金($300/件)</td>" &_				   
				   "</TR>"
	SERNO =0
	BonusSum = 0
	Do While Not rs.Eof
		SERNO = SERNO + rs("NUM")
		BonusSum = BonusSum + rs("BONUS")
	    response.Write "<TR>" &_
				   "<td colspan=4 style='mso-ignore:colspan'></td>" &_
				   "<td class=tochar>"& rs("EMPLY") &"</td>" &_
				   "<td class=toNum>"& rs("NUM") &"</td>" &_
				   "<td class=toNum>"& rs("BONUS") &"</td>" &_
				   "</TR>"
      rs.MoveNext
    Loop
    rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
	
    response.Write "<TR>" &_
				   "<td colspan=4 style='mso-ignore:colspan'></td>" &_
				   "<td class=titleN>合計</td>" &_
				   "<td class=titleN>"& SERNO&"</td>" &_
				   "<td class=titleN>"& BonusSum &"</td>" &_
				   "</TR></table>"
	response.Write "<br><br><font size=2>" &_
				   "總經理：　　　　　　　副總經理：　　　　　　　部門主管：　　　　　　　單位主管：　　　　　　　製表人：</font>"
%>
