<%
Sub SrGetUserInfo(accessMode,Eusr,Edat,Uusr,Udat)
    If accessMode="A" Then
       Eusr=Request.ServerVariables("LOGON_USER")
       Edat=Now()
    Else 
       Uusr=Request.ServerVariables("LOGON_USER")
       Udat=Now()
    End If
End Sub  
%>