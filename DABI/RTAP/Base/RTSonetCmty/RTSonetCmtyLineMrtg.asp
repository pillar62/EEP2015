<%
dim conn, rs, dsn, sql
set conn=server.CreateObject("ADODB.connection")
set rs=server.CreateObject("ADODB.recordset")
dsn="DSN=RTLIB"
conn.Open dsn
KEYARY=SPLIT(REQUEST("KEY"),";")
sql="select * FROM RTSonetCmtyLine WHERE COMQ1=" & KEYARY(0) & " AND lineq1=" & KEYARY(1)
'response.write sql
RS.Open SQL,CONN
IF NOT RS.EOF THEN
   LINEIP=RS("LINEIP")
ELSE
   LINEIP=""
END IF
RS.CLOSE
conn.Close
set rs=nothing
set conn=nothing
%>

<html>
<head>
	<title>主線流量查詢</title>
</head>

<script language="VBScript">
<!--
Sub Window_Onload
	On Error Resume Next
	  dim lineip
	  lineip=document.all("lineip").value

	if lineip ="" then
		window.alert "主線無IP資料!!"
		window.close
	else
		On Error Resume Next
		prog = "http://"& lineip &":10011/cgi/grhistory.cgi"
		window.open prog,"_self"
	end if
End Sub
-->
</script>

<body>
<input type="text" name="lineip" value="<%=lineip%>" style="display:none;" ID="Text1">
<!--
<table width="100%" align=right ID="Table3"><tr><TD></td><td align="right">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand" ID="Button1">
</td></tr></table>
-->
</body>
</html>