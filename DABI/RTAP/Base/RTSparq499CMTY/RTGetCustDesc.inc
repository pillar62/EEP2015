<%
  Function FrGetCustDesc(Comq1,LINEQ1,CUSID)
  Dim s,conn,rs,sql
  Set conn=Server.CreateObject("ADODB.Connection")  
  Set rs=Server.CreateObject("ADODB.Recordset")
  conn.Open DSN
  sql="select * from rtEBTcmtyH where COMQ1=" & comq1
  rs.Open sql,conn
  s=""
  If rs.Eof Then
     s="���ϥN��:" &Comq1 &"�b���ϰ򥻸�Ƥ��䤣��"
  Else 
     s="���ϦW��:" &rs("comn") 
  End If
  rs.Close

  sql="select * from rtEBTCUST where COMQ1=" & COMQ1 & " AND LINEQ1=" & LINEQ1 & " AND CUSID='" & CUSID & "' "
  rs.Open sql,conn
  If rs.Eof Then
     s= s & "  �Ȥ�N��:" & Cusid &" �b��H�򥻸�Ƥ��䤣��"
  Else 
     s= s & "  �Ȥ�W��:" &rs("CUSNC") 
     s= s & "  ���u���:" &rs("finishdat") 
  End If
  rs.Close
  Set rs=Nothing
  conn.Close
  Set conn=Nothing
  FrGetcustDesc=s
End Function
%>