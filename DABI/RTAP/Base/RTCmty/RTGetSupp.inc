<%
Sub SrGetsupp(accessMode,sw,protect,cty,cusid,v)
    Dim conn,rs,sql,sx
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 Then 
       sql="select  DISTINCT RTObj.CUSID, RTObj.SHORTNC from RTSuppCty INNER JOIN " _
          &"RTCmty ON RTSuppCty.CUTID = RTCmty.CUTID INNER JOIN " _
          &"RTObj ON RTSuppCty.CUSID = RTObj.CUSID and rtsuppcty.cutid='" & cty & "' " _
          &"GROUP BY rtobj.cusid, rtobj.shortnc "
       V="<option value="""" selected>(┐дел)</option>"
    Else
       sql="SELECT RTObj.CUSID, RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN RTSupp ON RTObj.CUSID = RTSupp.CUSID " _
          &"WHERE RTSupp.CUSID='" & cusid &"' "
       v=""
    End If
    rs.Open sql,conn
    If rs.Eof Then V="<option value="""" selected>(┐дел)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("cusid")=cusid Then sx=" selected "
       v=v &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("shortnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing 
End Sub
%>    