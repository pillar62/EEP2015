<%@ Language=VBScript %>
<html>
<head>
<meta http-equiv="Pragma" content="no-cache">
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<meta http-equiv="Content-Language" content="zh-tw">
<link REL="stylesheet" HREF="keyList.css" TYPE="text/css">
</head>
<center>
<body background="/WEBAP/IMAGE/bg.gif">
<form name="form" method="post">
<table width="100%" cellPadding=0 cellSpacing=0>
  <tr class=keyListTitle><td width="20%" align=left><%=Request.ServerVariables("LOGON_USER")%></td>
                         <td width="60%" align=middle><STRONG>元訊寬頻網路股份有限公司<br>HI-Building管理系統</STRONG></td>
                         <td width="20%" align=right><%=Now()%></td><tr>
  <tr class=keyListTitle><td>&nbsp;</td><td align=middle><%=title%></td><td>&nbsp;</td><tr></tr>
</table>
<table widtH="100%" border=1 cellPadding=0 cellSpacing=0 bgcolor="lightyellow">
  <tr bgcolor="darkseagreen"><TD ALIGN="CENTER"><FONT size=2>項次</FONT></TD><TD ALIGN="CENTER"><FONT size=2>日期</font></TD><TD ALIGN="CENTER"><FONT size=2>系　統　訊　息</font></TD></TR>
<%
 Set conn=Server.CreateObject("ADODB.Connection")
 Set rs=Server.CreateObject("ADODB.Recordset")
 DSN="DSN=RTLib"
 conn.open DSN
 SQL="SELECT MSGID, TOPIC, CONTENT, MSGDAT, APPEAR, UPDAT, DOWNDAT, IMG FROM RTSYSMSG where appear='Y' and ( UPDAT <= GETDATE() OR UPDAT IS NULL ) AND ( DOWNDAT IS NULL OR DOWNDAT > GETDATE() ) order by msgdat desc"
 RS.Open SQL,CONN
 cnt=0
 do while not rs.eof
    cnt=cnt+1
    K=cnt mod 2
    if k=1 then
       response.Write "<TR bgcolor=lightyellow>"
    else
       response.Write "<TR bgcolor=silver>"
    end if
       response.Write "<TD align=center><FONT size=2>" & RS("MSGID") & "</FONT></TD>" & "<TD><FONT size=2>" & RS("MSGDAT") & "</FONT></TD>" & "<TD><FONT size=2>" & RS("TOPIC") & "<BR><BR>"& RS("Content") &"</FONT></TD></TR>"    
    RS.movenext
  LOOP
rs.Close
conn.Close
set rs=nothing
set conn=nothing
%>  

</table>
</form>
</body>
</html>
