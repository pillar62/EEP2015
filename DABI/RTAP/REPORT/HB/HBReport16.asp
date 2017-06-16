<style>
<!--
.toChar
	{font-size:10.0pt;mso-number-format:"\@";border:0.5pt solid black;}
.toNum
	{font-size:10.0pt;border:0.5pt solid black;}
.toNumY
	{font-size:10.0pt;border:0.5pt solid black;background:peachpuff;}
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
    sql="usp_RTAllCaseCount '" &v(0)& "' "
   rs.Open sql, CONN
   
'response.Write sqlstr    
    
    ' 灿 ===================================================================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=よキА絬计&め计参璸"

	response.Write "<table>"
	response.Write "<tr><td align =center colspan=15><b>じ癟糴繵呼隔Τそ</b></td></tr>"
	response.Write "<tr><td align =center colspan=15><b>よキА絬计&め计参璸</b></td></tr>"
	response.Write "<tr><td align =left colspan=15><font size=2><u>参璸戳丁  "&v(0)&" </u></font></td></tr>"
	response.Write "<tr><td align =right colspan=15><font size=2>籹ら戳" &now()& "</font></td></tr>"
	
	response.Write "<TR>" &_
		"<td class=titleN colspan=""2"" rowspan=""2"">&nbsp;</td>" &_    
		"<td class=titleN align=""center"" colspan=""3""></td>" &_
		"<td class=titleN align=""center"" colspan=""3"">堕</td>" &_
		"<td class=titleN align=""center"" colspan=""3"">い</td>" &_
		"<td class=titleN align=""center"" colspan=""3"">蔼动</td>" &_
		"<td class=titleN align=""center"" rowspan=""2"">璸</td>" &_
		"</TR>"
	response.Write "<TR>" &_
		"<td class=titleN align=""center"">綪</td>" &_
		"<td class=titleN align=""center"">竒綪</td>" &_
		"<td class=titleN align=""center"">璸</td>" &_
		"<td class=titleN align=""center"">綪</td>" &_
		"<td class=titleN align=""center"">竒綪</td>" &_
		"<td class=titleN align=""center"">璸</td>" &_
		"<td class=titleN align=""center"">綪</td>" &_
		"<td class=titleN align=""center"">竒綪</td>" &_
		"<td class=titleN align=""center"">璸</td>" &_
		"<td class=titleN align=""center"">綪</td>" &_
		"<td class=titleN align=""center"">竒綪</td>" &_
		"<td class=titleN align=""center"">璸</td>" &_
		"</TR>"
		
	if rs("num") ="キА" then
		colored = tonum
	else
		colored = tonumY
	end if
		
	Do While Not rs.Eof
		if rs("num") ="キА" then
			colored = "tonumY"
		else
			colored = "tonum"
		end if
	
		response.Write "<TR>" &_
			"<td class=tochar>"& rs("casetype") &"</td>" &_				   
			"<td class=tochar>"& rs("num") &"</td>" &_
			"<td class=" &colored& ">"& rs("c1aa") &"</td>" &_
			"<td class=" &colored& ">"& rs("c1ab") &"</td>" &_
			"<td class=" &colored& ">"& rs("c1") &"</td>" &_
			"<td class=" &colored& ">"& rs("c2aa") &"</td>" &_
			"<td class=" &colored& ">"& rs("c2ab") &"</td>" &_
			"<td class=" &colored& ">"& rs("c2") &"</td>" &_
			"<td class=" &colored& ">"& rs("c3aa") &"</td>" &_
			"<td class=" &colored& ">"& rs("c3ab") &"</td>" &_
			"<td class=" &colored& ">"& rs("c3") &"</td>" &_
			"<td class=" &colored& ">"& rs("c4aa") &"</td>" &_
			"<td class=" &colored& ">"& rs("c4ab") &"</td>" &_
			"<td class=" &colored& ">"& rs("c4") &"</td>" &_
			"<td class=" &colored& ">"& rs("sumh") &"</td>" 
		response.Write "</TR>"
		rs.MoveNext      
    	Loop
	response.Write "</table>"  
    
	rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
%>
