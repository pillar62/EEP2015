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
    'server.scripttimeout =30000
    parm=request("parm")
    v=split(parm,";")
	if v(0) ="*" then v(0) ="%"
	if v(1) ="*" then v(1) ="%"	

    Dim rs,conn, formid
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
    rs.Open "usp_RTCmtyCustCount '" &v(0)& "','" &v(1)& "','" &v(2)& "','" &v(3)& "','" &v(4)& "' ", CONN
     
'response.Write sqlstr    
    
    
    ' 明細表 ===================================================================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=各方案客戶數統計表.xls"
	response.Write "<table>"
	response.Write "<tr><td align =center colspan=9><b>元訊寬頻網路股份有限公司</b></td></tr>"
	response.Write "<tr><td align =center colspan=9><b>各方案客戶數統計表</b></td></tr>"
	response.Write "<tr><td align =center colspan=11><font size=2><u>統計期間: "& v(2)&"～"& v(3)&"用戶</u></font></td></tr>"
	response.Write "<tr><td align =right colspan=9><font size=2>製表日期：" &now()& "</font></td></tr>"
    response.Write "<TR>" &_
				   "<td class=titleN>方案別</td>" &_
				   "<td class=titleN>直/經銷</td>" &_
				   "<td class=titleN>轄區</td>" &_				   
				   "<td class=titleN>業務員</td>" &_
				   "<td class=titleN>社區名稱</td>" &_
				   "<td class=titleN>社區開通日</td>" &_
				   "<td class=titleN>社區地址</td>" &_				   
				   "<td class=titleY>有效戶數</td>" &_
				   "<td class=titleY>退租戶數</td>" &_
				   "</TR>"

	Do While Not rs.Eof
	    response.Write "<TR>" &_
				   "<td class=tochar>"& rs("casetype") &"</td>" &_				   
				   "<td class=tochar>"& rs("belongid") &"</td>" &_
				   "<td class=tochar>"& rs("belongnc") &"</td>" &_
				   "<td class=tochar>"& rs("salesman") &"</td>" &_
				   "<td class=tochar>"& rs("comn") &"</td>" &_				   
				   "<td class=tochar>"& rs("lineapplydat") &"</td>" &_
				   "<td class=tochar>"& rs("addr") &"</td>" &_
				   "<td class=toNum>"& rs("num") &"</td>" &_
				   "<td class=toNum>"& rs("dropnum") &"</td>" &_				   
				   "</TR>"
      rs.MoveNext
    Loop
	response.Write "</table>"  
	

    rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
	
%>
