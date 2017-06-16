<%
  Function FrGetContractDesc(ctno)
  Dim s,conn,rs,sql
  Set conn=Server.CreateObject("ADODB.Connection")  
  Set rs=Server.CreateObject("ADODB.Recordset")
  conn.Open DSN
  sql="SELECT CTNO, CTOBJNAME FROM HBCONTRACTH WHERE HBcontracth.ctno=" &ctno
 ' response.write "SQL=" & SQL
  rs.Open sql,conn
  s=""
  If rs.Eof Then
     s="歸檔序號:" &ctno &"在廠商基本資料內找不到"
  Else 
     s="歸檔序號:" &rs("CTNO") & " 合約對象:" & rs("CTOBJNAME")
  End If
  rs.Close
  Set rs=Nothing
  conn.Close
  Set conn=Nothing
  FrGetContractDesc=s
End Function
%>