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
    sql="usp_RTAllCaseConsigneeCount '" &v(0)& "' "
   rs.Open sql, CONN
   
'response.Write sqlstr    
    
    ' 明細表 ===================================================================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=經銷業務員線數&戶數統計"

	response.Write "<table>"
	response.Write "<tr><td align =center colspan=17><b>元訊寬頻網路股份有限公司</b></td></tr>"
	response.Write "<tr><td align =center colspan=17><b>經銷業務員線數&戶數統計</b></td></tr>"
	response.Write "<tr><td align =left colspan=17><font size=2><u>統計期間 至： "&v(0)&" </u></font></td></tr>"
	response.Write "<tr><td align =right colspan=17><font size=2>製表日期：" &now()& "</font></td></tr>"
	
	response.Write "<TR>" &_
		"<td class=titleN colspan=""2"">&nbsp;</td>" &_    
		"<td class=titleN align=""center"" colspan=""5"">轄區總戶數</td>" &_
		"<td class=titleN align=""center"" colspan=""5"">轄區總線數</td>" &_
		"<td class=titleN align=""center"" colspan=""5"">平均戶數</td>" &_
		"</TR>"
	response.Write "<TR>" &_
		"<td class=titleN align=""center"">轄區</td>" &_
		"<td class=titleN align=""center"">業務員</td>" &_
		"<td class=titleN align=""center"">CHT 599</td>" &_
		"<td class=titleN align=""center"">CHT 399</td>" &_
		"<td class=titleN align=""center"">Sparq 399</td>" &_
		"<td class=titleN align=""center"">Sparq 499</td>" &_
		"<td class=titleN align=""center"">EBT</td>" &_
		"<td class=titleN align=""center"">CHT 599</td>" &_
		"<td class=titleN align=""center"">CHT 399</td>" &_
		"<td class=titleN align=""center"">Sparq 399</td>" &_
		"<td class=titleN align=""center"">Sparq 499</td>" &_
		"<td class=titleN align=""center"">EBT</td>" &_
		"<td class=titleN align=""center"">CHT 599</td>" &_
		"<td class=titleN align=""center"">CHT 399</td>" &_
		"<td class=titleN align=""center"">Sparq 399</td>" &_
		"<td class=titleN align=""center"">Sparq 499</td>" &_
		"<td class=titleN align=""center"">EBT</td>" &_
		"</TR>"

		
	'if rs("num") ="平均" then
	'	colored = tonum
	'else
	'	colored = tonumY
	'end if
		
	Do While Not rs.Eof
'		if rs("num") ="平均" then
'			colored = "tonumY"
'		else
			colored = "tonum"
'		end if
	
		response.Write "<TR>" &_
			"<td class=tochar>"& rs("belongnc") &"</td>" &_				   
			"<td class=tochar>"& rs("sales") &"</td>" &_
			"<td class=" &colored& ">"& rs("CUSTcht599") &"</td>" &_
			"<td class=" &colored& ">"& rs("CUSTcht399") &"</td>" &_
			"<td class=" &colored& ">"& rs("CUSTsparq399") &"</td>" &_
			"<td class=" &colored& ">"& rs("CUSTsparq499") &"</td>" &_
			"<td class=" &colored& ">"& rs("CUSTebt") &"</td>" &_
			"<td class=" &colored& ">"& rs("LINEcht599") &"</td>" &_
			"<td class=" &colored& ">"& rs("LINEcht399") &"</td>" &_
			"<td class=" &colored& ">"& rs("LINEsparq399") &"</td>" &_
			"<td class=" &colored& ">"& rs("LINEsparq499") &"</td>" &_
			"<td class=" &colored& ">"& rs("LINEebt") &"</td>" &_
			"<td class=" &colored& ">"& rs("AVGcht599") &"</td>" &_
			"<td class=" &colored& ">"& rs("AVGcht399") &"</td>" &_
			"<td class=" &colored& ">"& rs("AVGsparq399") &"</td>" &_
			"<td class=" &colored& ">"& rs("AVGsparq499") &"</td>" &_
			"<td class=" &colored& ">"& rs("AVGebt") &"</td>"
			
		response.Write "</TR>"
		rs.MoveNext      
    	Loop
	response.Write "</table>"  
    
	rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
%>
