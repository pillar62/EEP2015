<style>
<!--
.toChar2
	{font-size:10.0pt;mso-number-format:"\@";border:0.5pt solid black;}
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
	sql="usp_RTCallOutNeedData '2', '"&v(0)&"', '"&v(1)&"', '"&v(2)&"', '"&v(3)&"' "

   rs.Open sql, CONN
     
'response.Write sql    
    
    ' 明細表 ===================================================================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=CALL OUT 記錄表"
	response.Write "<table>"
	'response.Write "<tr><td align =center colspan=12><b>元訊寬頻網路股份有限公司</b></td></tr>"
	response.Write "<tr><td align =center colspan=12><font size=3>CALL OUT 記錄表</font></td></tr>"
	response.Write "<tr><td align =right colspan=12><font size=2>製表日期：" &now()& "</font></td></tr>"
    response.Write "<TR>" &_
    			   "<td class=titleN>No.</td>" &_
    			   "<td class=titleN>方案別</td>" &_
				   "<td class=titleN>社區名稱</td>" &_
    			   "<td class=titleY>CALL OUT 時間</td>" &_
    			   "<td class=titleY>CALL OUT 人員</td>" &_
				   "<td class=titleN>客戶名稱</td>" &_
				   "<td class=titleN>客戶狀態</td>" &_
				   "<td class=titleN>TEL1</td>" &_
				   "<td class=titleN>TEL2</td>" &_
				   "<td class=titleN>手機</td>" &_
				   "<td class=titleY>電訪結果</td>" &_
				   "<td class=titleY>後續</td>" &_
				   "</TR>"
	serno=0
	Do While Not rs.Eof
	    serno = serno+1
	    response.Write "<TR>" &_
				   "<td class=tonum>"& serno &"</td>" &_
				   "<td class=tochar>"& rs("casetype") &"</td>" &_
				   "<td class=tochar>"& rs("comn") &"</td>" &_
   				   "<td class=tochar>&nbsp;</td>" &_
   				   "<td class=tochar>&nbsp;</td>" &_
				   "<td class=tochar>"& rs("cusnc") &"</td>" &_
				   "<td class=tochar>"& rs("custstats") &"</td>" &_
				   "<td class=tochar>"& rs("tel1") &"</td>" &_
				   "<td class=tochar>"& rs("tel2") &"&nbsp;</td>" &_
				   "<td class=tochar>"& rs("mobile") &"&nbsp;</td>" &_
   				   "<td class=tochar>&nbsp;</td>" &_
   				   "<td class=tochar>&nbsp;</td>" &_
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
	response.Write "</table>"
%>
