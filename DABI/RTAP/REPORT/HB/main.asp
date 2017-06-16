<%@ Language=VBScript %>
<html>
<head>
<meta http-equiv="Pragma" content="no-cache">
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<meta http-equiv="Content-Language" content="zh-tw">
<link REL="stylesheet" HREF="keyList.css" TYPE="text/css">
</head>
<center>
<body>
<form name="form" method="post">
<table width="100%" cellPadding=0 cellSpacing=0>
  <tr class=keyListTitle><td width="20%" align=left><%=Request.ServerVariables("LOGON_USER")%></td>
                         <td width="60%" align=middle><STRONG>元訊寬頻網路股份有限公司<br>HI-Building管理系統<br>ADSL報表管理</STRONG></td>
                         <td width="20%" align=right><%=Now()%></td><tr>
  <tr class=keyListTitle><td>&nbsp;</td><td align=middle><%=title%></td><td>&nbsp;</td><tr></tr>
</table>
<table >
  <tr><td><IMG  src="BGNXX.JPG" width=400 height=450>&nbsp;</td></tr>
</table>
</form>
</body>
</html>
