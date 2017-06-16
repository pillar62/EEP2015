<%
  Function FrGetAreatownshipDesc(areaid,groupid,cutid)
  Dim s,conn,rs,sql
  Set conn=Server.CreateObject("ADODB.Connection")  
  Set rs=Server.CreateObject("ADODB.Recordset")
  conn.Open DSN
  '----轄區名稱
  sql="SELECT areanc from rtarea " _
     &"WHERE areaid='" &areaid & "'"
  rs.Open sql,conn
  s=""
  If rs.Eof Then
     s="轄區代號:" &areaid &"在轄區基本資料內找不到"
  Else 
     s="轄區:" &rs("areanc") 
  End If
  rs.Close

  sql="Select * from RTSalesGroup where areaid='" & areaid & "' and groupid='" & groupid & "'"
  rs.open sql,conn
  If rs.Eof Then
     s= s & "  業務組別:" &groupid &"在基本資料內找不到"
  Else 
     s= s & "業務組別:" &rs("groupnc") 
  End If
  rs.close  
  Set rs=Nothing
  conn.Close
  Set conn=Nothing
  FrGetAreatownshipDesc=s
End Function
%>