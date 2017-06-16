<%@ Language=VBScript %>
<%
keyary=split(request("key"),";")
KEYXX=SESSION("COMQ1XX") & ";" & SESSION("COMTYPEXX") & ";" & KEYARY(1)
response.Redirect "/webap/rtap/base/HBCALLOUTPROJECT/HBCALLOUTK.asp?key=" & KEYXX
%>