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
    sql="usp_RTCmtyLine '" &v(0)& "' "
   rs.Open sql, CONN
   
'response.Write sqlstr    
    
    ' 明細表 ===================================================================================
	Response.Charset ="BIG5"    
	Response.ContentType = "Content-Language;content=zh-tw"     
	response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=社區主線資料"

	response.Write "<table>"
	response.Write "<tr><td align =center colspan=13><b>元訊寬頻網路股份有限公司</b></td></tr>"
	response.Write "<tr><td align =center colspan=13><b>社區主線資料</b></td></tr>"
	'response.Write "<tr><td align =left colspan=13><font size=2><u>統計期間 至： "&v(0)&" </u></font></td></tr>"
	response.Write "<tr><td align =right colspan=13><font size=2>製表日期：" &now()& "</font></td></tr>"
	
if v(0) ="01" then
	 response.Write "<TR>" &_
		"<td class=titleN align=""center"">方案別</td>" &_
		"<td class=titleN align=""center"">經銷別</td>" &_
		"<td class=titleN align=""center"">營運點</td>" &_
		"<td class=titleN align=""center"">HB號碼(固定)</td>" &_
		"<td class=titleN align=""center"">HB號碼(計量)</td>" &_
		"<td class=titleN align=""center"">HB號碼(光纖)</td>" &_
		"<td class=titleN align=""center"">HB號碼(ADSL)</td>" &_
		"<td class=titleN align=""center"">社區名稱</td>" &_
		"<td class=titleN align=""center"">連結型態</td>" &_
		"<td class=titleN align=""center"">計量制電路編號</td>" &_
		"<td class=titleN align=""center"">光纖電路編號</td>" &_
		"<td class=titleN align=""center"">規模戶數</td>" &_
		"<td class=titleN align=""center"">COT IP</td>" &_
		"<td class=titleN align=""center"">Router IP</td>" &_
		"<td class=titleN align=""center"">IP 網段</td>" &_
		"<td class=titleN align=""center"">有效戶數</td>" &_						
		"<td class=titleN align=""center"">社區地址</td>" &_
		"</TR>"
elseif v(0) ="02" then
		 response.Write "<TR>" &_
		"<td class=titleN align=""center"">方案別</td>" &_
		"<td class=titleN align=""center"">經銷別</td>" &_
		"<td class=titleN align=""center"">營運點</td>" &_
		"<td class=titleN align=""center"">社區名稱</td>" &_
		"<td class=titleN align=""center"">主線頻寬</td>" &_
		"<td class=titleN align=""center"">附掛號碼</td>" &_
		"<td class=titleN align=""center"">規模戶數</td>" &_
		"<td class=titleN align=""center"">IDSLAM IP</td>" &_
		"<td class=titleN align=""center"">GATEWAY IP</td>" &_
		"<td class=titleN align=""center"">主線網段</td>" &_
		"<td class=titleN align=""center"">有效戶數</td>" &_						
		"<td class=titleN align=""center"">社區地址</td>" &_
		"</TR>"
elseif v(0) ="03" then
			 response.Write "<TR>" &_
		"<td class=titleN align=""center"">方案別</td>" &_
		"<td class=titleN align=""center"">經銷別</td>" &_
		"<td class=titleN align=""center"">營運點</td>" &_
		"<td class=titleN align=""center"">HB號碼</td>" &_
		"<td class=titleN align=""center"">社區名稱</td>" &_
		"<td class=titleN align=""center"">主線頻寬</td>" &_
		"<td class=titleN align=""center"">附掛號碼</td>" &_
		"<td class=titleN align=""center"">規模戶數</td>" &_
		"<td class=titleN align=""center"">主線IP</td>" &_
		"<td class=titleN align=""center"">RESET方式</td>" &_
		"<td class=titleN align=""center"">RESET備註</td>" &_
		"<td class=titleN align=""center"">有效戶數</td>" &_
		"<td class=titleN align=""center"">社區地址</td>" &_
		"</TR>"
elseif v(0) ="04" then
			 response.Write "<TR>" &_
		"<td class=titleN align=""center"">方案別</td>" &_
		"<td class=titleN align=""center"">經銷別</td>" &_
		"<td class=titleN align=""center"">營運點</td>" &_
		"<td class=titleN align=""center"">HB號碼</td>" &_
		"<td class=titleN align=""center"">社區名稱</td>" &_
		"<td class=titleN align=""center"">主線頻寬</td>" &_
		"<td class=titleN align=""center"">附掛號碼</td>" &_
		"<td class=titleN align=""center"">規模戶數</td>" &_
		"<td class=titleN align=""center"">主線IP</td>" &_
		"<td class=titleN align=""center"">RESET方式</td>" &_
		"<td class=titleN align=""center"">RESET備註</td>" &_
		"<td class=titleN align=""center"">有效戶數</td>" &_
		"<td class=titleN align=""center"">社區地址</td>" &_
		"</TR>"
elseif v(0) ="05" then
			 response.Write "<TR>" &_
		"<td class=titleN align=""center"">方案別</td>" &_
		"<td class=titleN align=""center"">經銷別</td>" &_
		"<td class=titleN align=""center"">營運點</td>" &_
		"<td class=titleN align=""center"">HB號碼</td>" &_
		"<td class=titleN align=""center"">社區名稱</td>" &_
		"<td class=titleN align=""center"">主線頻寬</td>" &_
		"<td class=titleN align=""center"">附掛號碼</td>" &_
		"<td class=titleN align=""center"">規模戶數</td>" &_
		"<td class=titleN align=""center"">主線IP</td>" &_
		"<td class=titleN align=""center"">RESET方式</td>" &_
		"<td class=titleN align=""center"">RESET備註</td>" &_
		"<td class=titleN align=""center"">有效戶數</td>" &_
		"<td class=titleN align=""center"">社區地址</td>" &_
		"</TR>"
end if
	
		
	Do While Not rs.Eof
		 if v(0) ="01" then	
				response.Write "<TR>" &_
				"<td class=tochar>"& rs("casetype") &"</td>" &_				   
				"<td class=tochar>"& rs("belongnc") &"</td>" &_
				"<td class=tochar>"& rs("operationname") &"</td>" &_
				"<td class=tochar>"& rs("hbno") &"</td>" &_
				"<td class=tochar>"& rs("hbno2") &"</td>" &_
				"<td class=tochar>"& rs("hbno3") &"</td>" &_
				"<td class=tochar>"& rs("hbno4") &"</td>" &_
				"<td class=tochar>"& rs("comn") &"</td>" &_
				"<td class=tochar>"& rs("connecttypenc") &"</td>" &_
				"<td class=tochar>"& rs("t1no2") &"</td>" &_	
				"<td class=tochar>"& rs("t1no3") &"</td>" &_
				"<td class=tochar>"& rs("comcnt") &"</td>" &_
				"<td class=tochar>"& rs("cotip") &"</td>" &_
				"<td class=tochar>"& rs("idslamip") &"</td>" &_
				"<td class=tochar>"& rs("netip") &"</td>" &_								
				"<td class=tochar>"& rs("num") &"</td>" &_				   												
				"<td class=tochar>"& rs("addr") &"</td>"
		 elseif v(0) ="02" then		
				response.Write "<TR>" &_
				"<td class=tochar>"& rs("casetype") &"</td>" &_				   
				"<td class=tochar>"& rs("belongnc") &"</td>" &_
				"<td class=tochar>"& rs("operationname") &"</td>" &_
				"<td class=tochar>"& rs("comn") &"</td>" &_				   				
				"<td class=tochar>"& rs("lineratenc") &"</td>" &_
				"<td class=tochar>"& rs("linetel") &"</td>" &_	
				"<td class=tochar>"& rs("comcnt") &"</td>" &_
				"<td class=tochar>"& rs("idslamip") &"</td>" &_
				"<td class=tochar>"& rs("gateway") &"</td>" &_	
				"<td class=tochar>"& rs("netip") &"</td>" &_											
				"<td class=tochar>"& rs("num") &"</td>" &_				   												
				"<td class=tochar>"& rs("addr") &"</td>"
	   elseif v(0) ="03" then		
				response.Write "<TR>" &_	   	
				"<td class=tochar>"& rs("casetype") &"</td>" &_				   
				"<td class=tochar>"& rs("belongnc") &"</td>" &_
				"<td class=tochar>"& rs("operationname") &"</td>" &_
				"<td class=tochar>"& rs("hbno") &"</td>" &_
				"<td class=tochar>"& rs("comn") &"</td>" &_				   				
				"<td class=tochar>"& rs("lineratenc") &"</td>" &_
				"<td class=tochar>"& rs("cmtytel") &"</td>" &_	
				"<td class=tochar>"& rs("comcnt") &"</td>" &_
				"<td class=tochar>"& rs("ipaddr") &"</td>" &_
				"<td class=tochar>"& rs("resetnc") &"</td>" &_	
				"<td class=tochar>"& rs("resetdesc") &"</td>" &_											
				"<td class=tochar>"& rs("num") &"</td>" &_				   								
				"<td class=tochar>"& rs("addr") &"</td>"
	   elseif v(0) ="04" then		
				response.Write "<TR>" &_	   	
				"<td class=tochar>"& rs("casetype") &"</td>" &_				   
				"<td class=tochar>"& rs("belongnc") &"</td>" &_
				"<td class=tochar>"& rs("operationname") &"</td>" &_
				"<td class=tochar>"& rs("hbno") &"</td>" &_
				"<td class=tochar>"& rs("comn") &"</td>" &_				   				
				"<td class=tochar>"& rs("lineratenc") &"</td>" &_
				"<td class=tochar>"& rs("cmtytel") &"</td>" &_	
				"<td class=tochar>"& rs("comcnt") &"</td>" &_
				"<td class=tochar>"& rs("ipaddr") &"</td>" &_
				"<td class=tochar>"& rs("resetnc") &"</td>" &_	
				"<td class=tochar>"& rs("resetdesc") &"</td>" &_											
				"<td class=tochar>"& rs("num") &"</td>" &_				   								
				"<td class=tochar>"& rs("addr") &"</td>"
	   elseif v(0) ="05" then		
				response.Write "<TR>" &_	   	
				"<td class=tochar>"& rs("casetype") &"</td>" &_				   
				"<td class=tochar>"& rs("belongnc") &"</td>" &_
				"<td class=tochar>"& rs("operationname") &"</td>" &_
				"<td class=tochar>"& rs("hbno") &"</td>" &_
				"<td class=tochar>"& rs("comn") &"</td>" &_				   				
				"<td class=tochar>"& rs("lineratenc") &"</td>" &_
				"<td class=tochar>"& rs("cmtytel") &"</td>" &_	
				"<td class=tochar>"& rs("comcnt") &"</td>" &_
				"<td class=tochar>"& rs("ipaddr") &"</td>" &_
				"<td class=tochar>"& rs("resetnc") &"</td>" &_	
				"<td class=tochar>"& rs("resetdesc") &"</td>" &_											
				"<td class=tochar>"& rs("num") &"</td>" &_				   								
				"<td class=tochar>"& rs("addr") &"</td>"
	   end if
		 response.Write "</TR>"
		 rs.MoveNext      
	Loop
	response.Write "</table>"  
    
	rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
%>
