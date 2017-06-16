<%
'由網路帳號讀取員工編號
Function FrGetUserEmply(user)
   Dim Role,i,r
  ' response.write "user=" & user
   dim conn,rs,sql,dsn
   set conn=server.createobject("ADODB.Connection")
   set rs=server.createobject("ADODB.recordset")
   DSN="DSN=RTLib"
   SQL="select emply from rtemployee where netid='" & user & "'"
   conn.open dsn
   rs.open sql,conn
   if not rs.eof then
     Domain=rs("emply")
   else
     Domain=""
   end if
   FrGetUserEmply=Domain
   rs.close
   conn.close
   set rs=nothing
   set conn=nothing
End Function  
%>