<%
  Function FrGetCtyDesc(cutid)
  Dim s,conn,rs,sql
  Set conn=Server.CreateObject("ADODB.Connection")  
  Set rs=Server.CreateObject("ADODB.Recordset")
  conn.Open DSN
  sql="SELECT cutnc from rtcounty " _
     &"WHERE cutid='" &cutid & "'"
  rs.Open sql,conn
  s=""
  If rs.Eof Then
     s="�����N��:" &cutid &"�b�����򥻸�Ƥ��䤣��"
  Else 
     s="�����W��:" &rs("cutnc") 
  End If
  rs.Close
  Set rs=Nothing
  conn.Close
  Set conn=Nothing
  FrGetCtyDesc=s
End Function
%>