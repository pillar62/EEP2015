<%
  Function FrGetSPARQCust499Desc(COMQ1,LINEQ1,Cusid)
  Dim s,conn,rs,sql
  Set conn=Server.CreateObject("ADODB.Connection")  
  Set rs=Server.CreateObject("ADODB.Recordset")
  conn.Open DSN
  sql="SELECT * FROM RTSPARQ499CUST WHERE COMQ1=" & COMQ1 & " AND LINEQ1=" & LINEQ1 & " AND CUSID='" & CUSID & "'"
 ' response.write "SQL=" & SQL
  rs.Open sql,conn
  s=""
  If rs.Eof Then
     s="客戶代碼:" &cusid &"在客戶基本資料內找不到"
  Else 
     s="客戶簡稱:" &rs("CUSNC") 
  End If
  rs.Close
  Set rs=Nothing
  conn.Close
  Set conn=Nothing
  FrGetSPARQCust499Desc=s
End Function
%>