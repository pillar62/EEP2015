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
    rs.Open "usp_RTCustDocketDrop '" &v(0)& "','" &v(1)& "' ", CONN
     
'response.Write sqlstr    
    
    ' 明細表 ===================================================================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=各方案北區直銷社區報竣退租欠拆數統計"
	response.Write "<table>"
	response.Write "<tr><td align =center colspan=8><b>元訊寬頻網路股份有限公司</b></td></tr>"
	response.Write "<tr><td align =center colspan=8><b>各方案北區直銷社區報竣退租欠拆數統計</b></td></tr>"
	'response.Write "<tr><td align =center colspan=11><font size=2><u>送件申請日"& v(0)&"～"& v(1)&"用戶</u></font></td></tr>"
	response.Write "<tr><td align =right colspan=8><font size=2>製表日期：" &now()& "</font></td></tr>"
    response.Write "<TR>" &_
				   "<td class=titleN>方案別</td>" &_
				   "<td class=titleN>縣市</td>" &_
				   "<td class=titleN>鄉鎮</td>" &_	   
				   "<td class=titleN>社區名稱</td>" &_	   
				   "<td class=titleN>業務人員</td>" &_	   				   				   
				   "<td class=titleY>報竣數</td>" &_
				   "<td class=titleY>退租數</td>" &_
				   "<td class=titleY>欠拆數</td>" &_				   
				   "</TR>"

	Do While Not rs.Eof
	    response.Write "<TR>" &_
				   "<td class=tochar>"& rs("casetype") &"</td>" &_				   
				   "<td class=tochar>"& rs("cutnc") &"</td>" &_
				   "<td class=tochar>"& rs("township") &"</td>" &_
				   "<td class=tochar>"& rs("comn") &"</td>" &_
				   "<td class=tochar>"& rs("sales") &"</td>" &_				   				   
				   "<td class=tonum>"& rs("docketnum") &"</td>" &_
				   "<td class=tonum>"& rs("dropnum") &"</td>" &_
				   "<td class=tonum>"& rs("overnum") &"</td>" &_				   
				   "</TR>"
      rs.MoveNext
    Loop
	response.Write "</table>"  
    rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
%>
