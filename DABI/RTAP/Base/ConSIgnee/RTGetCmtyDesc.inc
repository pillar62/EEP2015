<%
  Function FrGetCmtyDesc(Comq1)
  Dim s,conn,rs,sql
  Set conn=Server.CreateObject("ADODB.Connection")  
  Set rs=Server.CreateObject("ADODB.Recordset")
  conn.Open DSN
  sql="SELECT COMN, EQUIPADDR FROM rtsparqadslcmty WHERE CUTYID=" & comq1
  rs.Open sql,conn
  s=""
  If rs.Eof Then
     s="���ϥN��:" &Comq1 &"�b���ϰ򥻸�Ƥ��䤣��"
  Else 
     s="���ϦW��:" &rs("ComN") & "  ���d��m:" & rs("equipaddr")
  End If
  rs.Close
  Set rs=Nothing
  conn.Close
  Set conn=Nothing
  FrGetCmtyDesc=s
End Function
%>