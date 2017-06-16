<%
  Function FrGetConsigneeDesc(Cusid)
  Dim s,conn,rs,sql
  Set conn=Server.CreateObject("ADODB.Connection")  
  Set rs=Server.CreateObject("ADODB.Recordset")
  conn.Open DSN
  sql="SELECT RTObj.SHORTNC, RTObjLink.CUSTYID " _
     &"FROM RTObj INNER JOIN " _
     &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
     &"WHERE (RTObjLink.CUSTYID = '02') " _
     &"AND rtobj.cusid='" &cusid & "'"
 ' response.write "SQL=" & SQL
  rs.Open sql,conn
  s=""
  If rs.Eof Then
     s="經銷商代號:" &cusid &"在廠商基本資料內找不到"
  Else 
     s="經銷商簡稱:" &rs("shortnc") 
  End If
  rs.Close
  Set rs=Nothing
  conn.Close
  Set conn=Nothing
  FrGetConsigneeDesc=s
End Function
%>