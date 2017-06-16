<%
  Function FrGetStockDesc(Cusid)
  Dim s,conn,rs,sql
  Set conn=Server.CreateObject("ADODB.Connection")  
  Set rs=Server.CreateObject("ADODB.Recordset")
  conn.Open DSN
  sql="SELECT RTObj.CUSID, RTObj.SHORTNC, RTCounty.CUTNC, RTObj.TOWNSHIP1," _ 
           &"RTObj.RADDR1, RTStock.TEL, RTStock.RURL, RTStock.BOSS,RTStock.FAX " _
           &"FROM RTObj INNER JOIN " _
           &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID INNER JOIN " _
           &"RTObjKind ON RTObjLink.CUSTYID = RTObjKind.CUSTYID LEFT OUTER JOIN " _
           &"RTCounty ON RTObj.CUTID1 = RTCounty.CUTID INNER JOIN " _
           &"RTStock ON RTObjLink.CUSID = RTStock.CUSID " _
           &"WHERE (RTObjKind.CUSTYID = '06') AND rtobj.cusid='" &cusid & "'"
  rs.Open sql,conn
  s=""
  If rs.Eof Then
     s="券商代號:" &cusid &"在券商基本資料內找不到"
  Else 
     s="縣市別:" &rs("cutnc") &"  券商簡稱:" &rs("shortnc") 
  End If
  rs.Close
  Set rs=Nothing
  conn.Close
  Set conn=Nothing
  FrGetstockDesc=s
End Function
%>