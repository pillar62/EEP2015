<%
  Function FrGetCmtyDesc(Comq1)
  Dim s,conn,rs,sql
  Set conn=Server.CreateObject("ADODB.Connection")  
  Set rs=Server.CreateObject("ADODB.Recordset")
  conn.Open DSN
  sql="SELECT RTCounty.CUTNC AS CuntC, RTArea.AREANC AS AreaNC, RTCmty.COMN AS ComN " _
     &"FROM (RTArea INNER JOIN RTAreaCty ON RTArea.AREAID = RTAreaCty.AREAID) " _
     &"INNER JOIN (RTCmty INNER JOIN RTCounty ON RTCmty.CUTID = RTCounty.CUTID) " _
     &"ON RTAreaCty.CUTID = RTCmty.CUTID " _
     &"WHERE RTCmty.COMQ1=" &Comq1 &" AND RTArea.AREATYPE='2' "
  rs.Open sql,conn
  s=""
  If rs.Eof Then
     s="社區代號:" &Comq1 &"在社區基本資料內找不到"
  Else 
     s="施工轄區:" &rs("AreaNC") &"  縣市別:" &rs("CuntC") &"  社區名稱:" &rs("ComN")
  End If
  rs.Close
  Set rs=Nothing
  conn.Close
  Set conn=Nothing
  FrGetCmtyDesc=s
End Function
%>