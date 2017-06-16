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
    sql="usp_RTSparqAdslCount '"&v(0) &"','"& v(1) &"','"& V(2) &"','"& V(3) &"' "

'response.Write sql

   rs.Open sql, CONN
     
    ' 明細表 ===================================================================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	response.ContentType = "application/vnd.ms-excel"
	'Response.AddHeader "Content-Disposition","filename=速博399主線速率及客戶數統計"
	response.Write "<html><head><meta http-equiv=content-type content=""text/html; charset=big5""><head>"
	response.Write "<table>"
	response.Write "<tr><td align =center colspan=6><b>八合投資股份有限公司</b></td></tr>"
	response.Write "<tr><td align =center colspan=6><b>速博399主線速率及客戶數統計</b></td></tr>"
	response.Write "<tr><td align =left colspan=6><font size=2><u>報竣期間： "&v(0)&" 至 "&v(1)&" </u></font></td></tr>"
	response.Write "<tr><td align =left colspan=6><font size=2><u>主線測通期間： "&v(2)&" 至 "&v(3)&" </u></font></td></tr>"
	response.Write "<tr><td align =right colspan=6><font size=2>製表日期：" &now()& "</font></td></tr>"
	
    response.Write "<TR>" &_
    			   "<td class=titleY>方案</td>" &_
				   "<td class=titleY>直經銷</td>" &_
				   "<td class=titleY>轄區</td>" &_				   
				   "<td class=titleY>主線速率</td>" &_
				   "<td class=titleY>主線開通數</td>" &_
				   "<td class=titleY>客戶報竣數</td>" &_
				   "</TR>"

	Do While Not rs.Eof
	    response.Write "<TR>" &_
				   "<td class=tochar>"& rs("casetype") &"</td>" &_				   
				   "<td class=tochar>"& rs("belongtype") &"</td>" &_
				   "<td class=tochar>"& rs("belongnc") &"</td>" &_
				   "<td class=tochar>"& rs("lineratenc") &"</td>" &_
				   "<td class=toNum>"& rs("linenum") &"</td>" &_
				   "<td class=toNum>"& rs("num") &"</td>" &_
				   "</TR>"
      rs.MoveNext      
    Loop

    rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
	response.Write "</table>"
	response.Write "</html>"
%>
