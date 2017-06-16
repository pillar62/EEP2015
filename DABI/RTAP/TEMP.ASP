<%@ Language=VBScript %>
<%
sPage = Request("To")

set adoconn = Server.CreateObject("ADODB.Connection") 
set rs=Server.CreateObject("ADODB.Recordset")
adoconn.open "dsn=RTlib"
'ORDERS 是在Northwind
SQLstr = "Select * From RTcmty"
rs.open SQLstr,adoconn,1 ,1
rs.pagesize=15 

Page = CLng(sPage) 
If Page < 1 Then Page = 1
If Page > rs.PageCount Then Page = rs.PageCount
Response.Write "totpage=" & rs.pagecount
rs.AbsolutePage =page
%>
<HTML>
<HEAD>
<META name=VI60_defaultClientScript content=VBScript>
<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<SCRIPT ID=clientEventHandlersVBS LANGUAGE=vbscript>
<!--

Sub NEXTPAGE_onclick
iPage = <%=Page%>
iPage = iPage + 1
if ipage < 1 then 
   ipage=<%=rs.pagecount%>
else if ipage > <%=rs.pagecount%> then 
        ipage=1
     end if
end if
  
window.location = "Temp.asp?To=" & iPage
End Sub

Sub PREPAGE_onclick
iPage = <%=Page%>
iPage = iPage - 1
if ipage < 1 then 
   ipage=<%=rs.pagecount%>
else if ipage > <%=rs.pagecount%> then 
        ipage=1
     end if
end if
window.location = "Temp.asp?To=" & iPage
End Sub

Sub firstPAGE_onclick
iPage = 1
window.location = "Temp.asp?To=" & iPage
End Sub

Sub lastpage_onclick
iPage = <%=rs.pagecount%>
window.location = "Temp.asp?To=" & iPage
End Sub
-->
</SCRIPT>
</HEAD>
<BODY>
頁次:<%=RS.AbsolutePage%>
<Table>
<%FOR I = 1 TO RS.PageSize%>
<TR>
<TD><%=RS("comq1")%>
<TD><%=RS("comq2")%>
<TD><%=RS("comn")%>
</TR>
<%NEXT%>
</Table>
<INPUT TYPE=BUTTON NAME=firstPAGE VALUE=第一頁>
<INPUT TYPE=BUTTON NAME=PREPAGE VALUE=上一頁>
<INPUT TYPE=BUTTON NAME=NEXTPAGE VALUE=下一頁>
<INPUT TYPE=BUTTON NAME=lastPAGE VALUE=最末頁>
</BODY>
</BODY>
</HTML>