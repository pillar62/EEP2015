<%
    parm=request("parm")
    v=split(parm,";")

    Dim rs,conn,S6,tble,tbls
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
    rs.Open "usp_RTEBTFtp '2','" & v(0) &"'", CONN
	tbls = ""
    Do While Not rs.Eof
	   tble = rs("tbl")
	   if tble <> tbls then 
	       response.Write "<B><font color=""red"">" & rs("tbl") & "</font></b><br>"
	       tbls =tble
	   end if    
       response.Write rs("ftptext") & "<br>"
       rs.MoveNext
    Loop
    rs.Close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>    
