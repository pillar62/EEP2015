<%
  Function FrGetCustDesc(Comq1,cusid,entryno)
  Dim s,conn,rs,sql
  Set conn=Server.CreateObject("ADODB.Connection")  
  Set rs=Server.CreateObject("ADODB.Recordset")
  conn.Open DSN
  sql="select * from rtcustadslcmty where cutyid=" & comq1
  rs.Open sql,conn
  s=""
  If rs.Eof Then
     s="社區代號:" &Comq1 &"在社區基本資料內找不到"
  Else 
     s="社區名稱:" &rs("comn") 
  End If
  rs.Close

  sql="select * from rtobj where cusid='" & cusid & "'"
  rs.Open sql,conn
  If rs.Eof Then
     s= s & "  客戶代號:" & Cusid &" 在對象基本資料內找不到"
  Else 
     s= s & "  客戶名稱:" &rs("shortnc") 
  End If
  rs.Close
  sql="select * from rtcustadsl where cusid='" & cusid & "' and entryno=" & entryno
  rs.Open sql,conn
  If rs.Eof Then
     s= s & "  客戶:" & Cusid &" 在客戶基本資料內找不到"
  Else 
     s= s & "  完工日期:" &rs("finishdat") 
  End If
  rs.Close  
  Set rs=Nothing
  conn.Close
  Set conn=Nothing
  FrGetcustDesc=s
End Function
%>