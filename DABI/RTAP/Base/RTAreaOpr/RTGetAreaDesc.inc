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
     s="轄區代號:" &areaid &"在轄區基本資料內找不到"
  Else 
     s="業務轄區:" &rs("areanc") 
  End If
  rs.Close
  Set rs=Nothing
  conn.Close
  Set conn=Nothing
  FrGetAreaDesc=s
End Function
%>