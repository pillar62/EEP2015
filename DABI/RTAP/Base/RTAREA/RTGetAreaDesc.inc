<%
  Function FrGetAreaDesc(areaid)
  Dim s,conn,rs,sql
  Set conn=Server.CreateObject("ADODB.Connection")  
  Set rs=Server.CreateObject("ADODB.Recordset")
  conn.Open DSN
  sql="SELECT areanc from rtarea " _
     &"WHERE areaid='" &areaid & "'"
  rs.Open sql,conn
  s=""
  If rs.Eof Then
     s="�ҰϥN��:" &areaid &"�b�Ұϰ򥻸�Ƥ��䤣��"
  Else 
     s="�Ұ�:" &rs("areanc") 
  End If
  rs.Close
  Set rs=Nothing
  conn.Close
  Set conn=Nothing
  FrGetAreaDesc=s
End Function
%>