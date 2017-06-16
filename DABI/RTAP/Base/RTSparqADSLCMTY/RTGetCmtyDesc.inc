<%
  Function FrGetCmtyDesc(Comq1)
  Dim s,conn,rs,sql
  Set conn=Server.CreateObject("ADODB.Connection")  
  Set rs=Server.CreateObject("ADODB.Recordset")
  conn.Open DSN
  sql="SELECT * FROM rtsparqadslcmty WHERE CUTYID=" & comq1
 ' response.write sql
  rs.Open sql,conn
  s=""
  If rs.Eof Then
     s="社區代號:" &Comq1 &"在社區基本資料內找不到"
  Else 
     s="社區名稱:" &rs("ComN") & "  機櫃位置:" & rs("equipaddr")
  End If
  rs.Close
  Set rs=Nothing
  conn.Close
  Set conn=Nothing
  FrGetCmtyDesc=s
End Function
%>