<%
  Function FrGetAreatownshipDesc(areaid,groupid,cutid)
  Dim s,conn,rs,sql
  Set conn=Server.CreateObject("ADODB.Connection")  
  Set rs=Server.CreateObject("ADODB.Recordset")
  conn.Open DSN
  '----�ҰϦW��
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

  sql="Select * from RTSalesGroup where areaid='" & areaid & "' and groupid='" & groupid & "'"
  rs.open sql,conn
  If rs.Eof Then
     s= s & "  �~�ȲէO:" &groupid &"�b�򥻸�Ƥ��䤣��"
  Else 
     s= s & "�~�ȲէO:" &rs("groupnc") 
  End If
  rs.close  
  Set rs=Nothing
  conn.Close
  Set conn=Nothing
  FrGetAreatownshipDesc=s
End Function
%>