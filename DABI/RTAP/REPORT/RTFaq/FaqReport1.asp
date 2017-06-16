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
    sql="usp_RTFaqList '"&v(0)&"', '"&v(1)&"', '"&v(2)&"', '"&v(3)&"', '"&v(4)&"', '"&v(5)&"', '"&v(6)&"', '"&v(7)&"' "

   rs.Open sql, CONN
     
'response.Write sql    
    
    ' 明細表 ===================================================================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=客訴一覽表.xls"
	response.Write "<table>"
	response.Write "<tr><td align =center colspan=14><b>元訊寬頻網路股份有限公司</b></td></tr>"
	response.Write "<tr><td align =center colspan=14><font size=3>客戶一覽表 </font></td></tr>"
	response.Write "<tr><td align =right colspan=14><font size=2>製表日期：" &now()& "</font></td></tr>"
    response.Write "<TR>" &_
    			   "<td class=titleN>方案別</td>" &_
				   "<td class=titleN>轄區</td>" &_
				   "<td class=titleN>工程師</td>" &_
				   "<td class=titleN>社區名稱</td>" &_
				   "<td class=titleN>用戶名稱</td>" &_
				   "<td class=titleN>受理時間</td>" &_
				   "<td class=titleN>受理人</td>" &_
				   "<td class=titleN>客訴單列印日</td>" &_
				   "<td class=titleN>客服處理</td>" &_
				   "<td class=titleN>排除員工</td>" &_
				   "<td class=titleY>處理日期</td>" &_
				   "<td class=titleY>處理人員</td>" &_
   				   "<td class=titleY>處理措施</td>" &_
				   "<td class=titleN>及時完修</td>" &_
				   "</TR>"

	Do While Not rs.Eof
	    response.Write "<TR>" &_
				   "<td class=tochar>"& rs("casetype") &"</td>" &_
   				   "<td class=tochar>"& rs("areanc") &"</td>" &_
   				   "<td class=tochar>"& rs("operationname") &"</td>" &_
				   "<td class=tochar>"& rs("comn") &"</td>" &_
				   "<td class=tochar>"& rs("faqman") &"</td>" &_
				   "<td class=tochar>"& rs("rcvdate") &"</td>" &_
				   "<td class=tochar>"& rs("rcvusr") &"</td>" &_
				   "<td class=tochar>"& rs("faqprtdate") &"</td>" &_
				   "<td class=tochar>"& rs("memo") &"</td>" &_
				   "<td class=tochar>"& rs("finishusr") &"</td>" &_
				   "<td class=tochar>"& rs("logdate") &"</td>" &_
				   "<td class=tochar>"& rs("logusr") &"</td>" &_
				   "<td class=tochar>"& rs("logdesc") &"</td>" &_
				   "<td class=tochar>"& rs("fixtime") &"</td>" &_
				   "</TR>"
      rs.MoveNext
    Loop
	response.Write "</table>"  
	'response.Write "<br><br><font size=2>" &_
	'			   "審件人：　　　　　　　　　　　　　　　　　　　速博簽收：　　　　　　　　　　　　　　　　　　　製表：</font>"
    rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
%>
