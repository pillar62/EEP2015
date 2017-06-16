<%
	 Response.Buffer = True
     v=split(request("parm"),";")  
     Dim rs,conn
     Set conn=Server.CreateObject("ADODB.Connection")
     conn.open "DSN=RTLib"
	 Set rs = conn.Execute("usp_RTEnvPassList '"& v(0) &"','"& v(1) & "','"& v(2) & "','"& v(3) & "','"& v(4) &"'")
   
     fileName = "HBReport6a.xls"
     ftpPathFile = Server.MapPath("./txt") &"\"& filename 
     Set fso = CreateObject("Scripting.FileSystemObject")    
     Set fsoText = fso.CreateTextFile(ftpPathFile, True)
     iCount = 0
     fsoText.WriteLine("郵遞區號" &vbTab& "HN號碼" &vbTab& "客戶名稱" &vbTab& _
 					   "客戶地址" &vbTab& "完工日" &vbTab& "報竣日")
     Do While Not rs.Eof
	    iCount = iCount + 1
        fsoText.WriteLine(rs("RZONE") &vbTab& rs("CUSNO") &vbTab& rs("CUSNC") &vbTab& rs("RADDR") &vbTab& _
                          rs("FINISHDAT") & vbTab & rs("DOCKETDAT"))
        rs.MoveNext
     Loop
     fsoText.Close
     rs.Close
     conn.Close
     Set rs=Nothing
     Set conn=Nothing

	'Dim fileName
	'fileName = Request.QueryString("fileName")
	'Dim fso,fo,fContent
	'Set fso=server.CreateObject ("Scripting.FileSystemObject")
	set fo=fso.OpenTextFile(server.MapPath("./txt/" & fileName))
	fContent=fo.readall
	fo.close
	set fso=nothing
	Response.ContentType = "text/plain"
	Response.AddHeader "Content-Disposition", "attachment;filename=" & filename
	Response.Expires=0
	Response.Flush 
	Response.Write fContent
%> 

