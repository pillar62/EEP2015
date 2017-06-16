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

<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
	logonid=Request.ServerVariables("LOGON_USER")
	Call SrGetEmployeeRef(Rtnvalue,1,logonid)
	logonid=split(rtnvalue,";")  

    parm=request("key")
    v=split(parm,";")

    Dim rs,conn, formid
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
    sql="usp_RTLessorBillingNotice '" & v(1) &"','" & v(0) &"','" & logonid(0) &"' "
   rs.Open sql, CONN

    ' 明細表 ===================================================================================
	'Response.Charset ="BIG5"    
	'Response.ContentType = "Content-Language;content=zh-tw"     
	'Response.ContentType = "application/vnd.ms-excel"
	'Response.AddHeader "Content-Disposition","filename=HiBuilding客戶移轉FTTB清單"
	response.Write "<table>"
	'response.Write "<tr><td align =center colspan=8><b>元訊寬頻網路股份有限公司</b></td></tr>"
	'response.Write "<tr><td align =center colspan=8><b>FTTB申請清單</b></td></tr>"
	'response.Write "<tr><td align =left colspan=8><font size=2><u>統計期間"&v(0)&" 至 "&v(1)&" </u></font></td></tr>"
	'response.Write "<tr><td align =right colspan=8><font size=2>製表日期：" &now()& "</font></td></tr>"

		
	Do While Not rs.Eof
		response.Write "<BR><BR><BR><BR><BR><BR><BR>"
		response.Write "<font face=""標楷體"" size=4>　　　　　　　" &RS("rzone3")& "</font>"
		response.Write "<BR>"
		response.Write "<font face=""標楷體"" size=4>　　　　　　　　　" &RS("ADDR3")& "</font>"
		response.Write "<BR><BR><BR>"
		response.Write "<font face=""標楷體"" size=5>　　　　　　　　" &RS("CUSNC")& "　收</font>"
		response.Write  "<DIV style=""PAGE-BREAK-AFTER: always""></DIV>"
		rs.MoveNext      
    	Loop
	response.Write "</table>"  
    
	rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
%>
