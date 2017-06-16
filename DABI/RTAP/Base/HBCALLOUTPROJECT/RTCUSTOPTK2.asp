<%@ Language=VBScript %>
<%
keyary=split(request("key"),";")
KEYXX=KEYARY(0) & ";" & KEYARY(2) & ";" & KEYARY(3)
response.Redirect "/webap/rtap/base/HBCALLOUTPROJECT/HBCALLOUTK.asp?key=" & KEYXX
%>