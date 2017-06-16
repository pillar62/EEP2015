<%
  Function FrGetSalesGroupDesc(AREAID,GROUPID)
  Dim s,conn,rs,sql
  Set conn=Server.CreateObject("ADODB.Connection")  
  Set rs=Server.CreateObject("ADODB.Recordset")
  conn.Open DSN
  sql="SELECT GROUPNC from RTSalesGroup " _
     &"WHERE areaid='" &areaid & "' and groupid='" & groupid & "'"
  rs.Open sql,conn
  s=""
  If rs.Eof Then
     s="組別代號:" &areaid &"在業務組別資料檔內找不到"
  Else 
     s="　組別:" &rs("groupnc") 
  End If
  rs.Close
  Set rs=Nothing
  conn.Close
  Set conn=Nothing
  FrGetSalesGroupDesc=s
End Function
%>