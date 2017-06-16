<%
'role=0:全部人員皆不可輸入;role=1:業務助理;role=2:業務;role=4:技術部;role=8:財會;role=16:洪協理
'ROLE=31:資訊部管理員
Function FrGetUserRight(program,user)
   Dim Role,i,r
  ' response.write "user=" & user
   dim conn,rs,sql,dsn
   set conn=server.createobject("ADODB.Connection")
   set rs=server.createobject("ADODB.recordset")
   DSN="DSN=RTLib"
   SQL="select authlevel from rtemployee where netid='" & user & "'"
   conn.open dsn
   rs.open sql,conn
   if not rs.eof then
     Role=rs("authlevel")
   else
     Role=0
   end if
   r=""
   For i = 0 To 32
      If Role And i Then 
         r=r &";"
      Else
         r=r &" class=dataListData readonly;"
      End If
   Next
   FrGetUserRight=r
   'response.write "R=" & r & "<BR>"
   rs.close
   conn.close
   set rs=nothing
   set conn=nothing
End Function  
%>