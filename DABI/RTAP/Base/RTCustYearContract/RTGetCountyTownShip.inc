<%
Sub SrGetCountyTownShip(accessMode,sw,protect,cty,town,s,t)
    Dim conn,rs,sql,sx
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 Then 
       sql="SELECT * FROM RTCounty ORDER BY CutID "
       s="<option value="""" selected>(¿¤¥«)</option>"
    Else
       sql="SELECT * FROM RTCounty WHERE CutID='" &cty &"' "
       s=""
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" selected>(¿¤¥«)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CutID")=cty Then sx=" selected "
       s=s &"<option value=""" &rs("CutID") &"""" &sx &">" &rs("CutNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
    If sw="E" And protect<1 Then    
       rs.Open "SELECT * FROM RTCtyTown WHERE CutID='" &cty &"' ORDER BY TownShip ",conn    
       t=""
       If rs.Eof Then t="<option value="""" selected>(¶mÂí)</option>"  
       sx=""    
       Do While Not rs.Eof    
          If rs("TownShip")=town Then sx=" selected "    
          t=t &"<option value=""" &rs("TownShip") &"""" &sx &">" &rs("TownShip") &"</option>"    
          rs.MoveNext    
          sx=""    
       Loop    
       rs.Close    
    Else    
       t="<option value=""" &town &""" selected>" &town &"</option>"    
    End If
    conn.Close
    Set rs=Nothing
    Set conn=Nothing 
End Sub
%>    