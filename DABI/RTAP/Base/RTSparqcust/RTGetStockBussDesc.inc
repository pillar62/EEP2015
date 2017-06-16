<%
  Function FrGetStockBussDesc(Stockid,branch)
  Dim s,conn,rs,sql
  Set conn=Server.CreateObject("ADODB.Connection")  
  Set rs=Server.CreateObject("ADODB.Recordset")
  conn.Open DSN
  sql="SELECT RTObj.CUSID, RTObj.SHORTNC, RTObj.TOWNSHIP1, RTObj.RADDR1, " _
     &"RTBranch.BRANCH " _
     &"FROM RTObj INNER JOIN " _
     &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID INNER JOIN " _
     &"RTObjKind ON RTObjLink.CUSTYID = RTObjKind.CUSTYID INNER JOIN " _
     &"RTBranch ON RTObj.CUSID = RTBranch.CUSID " _
     &"WHERE (RTObjKind.CUSTYID = '06') AND (RTObj.CUSID ='" & stockid & "') AND " _ 
     &"(RTBranch.BRANCH ='" & branch & "')"
  rs.Open sql,conn
  s=""
  If rs.Eof Then
     s="券商代號:" &stockid &"分行代號:" & branch &"在券商分行基本資料內找不到"
  Else 
     s="證券公司:" &rs("shortnc") &"  分行:" &rs("branch") 
  End If
  rs.Close
  Set rs=Nothing
  conn.Close
  Set conn=Nothing
  FrGetstockBussDesc=s
End Function
%>