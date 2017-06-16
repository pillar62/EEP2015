<%@ LANGUAGE="VBSCRIPT" %>
<html>
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
</head>
<script language="VBScript">
Sub btn1_onClick()  

  window.close  
End Sub
</script>
<body>
<table width="100%" ID="Table1">
  <tr class=dataListTitle align=center>AVS用戶繳款狀況查詢</td><tr>
  </table>
  <u></u>
<%
set conn=server.CreateObject("ADODB.connection")
set rs=server.CreateObject("ADODB.recordset")
dsn="DSN=RTLIB"
conn.Open dsn
KEYARY=SPLIT(REQUEST("KEY"),";")
sql="select * FROM RTEBTCUST WHERE COMQ1=" & KEYARY(0) & " AND LINEQ1=" & KEYARY(1) & " AND CUSID='" & KEYARY(2) & "' "
RS.Open SQL,CONN
IF NOT RS.EOF THEN
   CUSNC=RS("CUSNC")
   DOCKETDAT=RS("DOCKETDAT")
   STRBILLINGDAT=RS("STRBILLINGDAT")
   DROPDAT=RS("DROPDAT")
   ENDBILLINGDAT=RS("ENDBILLINGDAT")
ELSE
   CUSNC=""
   DOCKETDAT=""
   STRBILLINGDAT=""
   DROPDAT=""
   ENDBILLINGDAT=""
END IF
RS.CLOSE
%>  
<table width="100%" border=0 cellPadding=0 cellSpacing=0 ID="Table4">
<tr>
<td><font size=2  color=blue>用戶名稱︰</font><font size=2  color=red><%=cusnc%></font>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br><font size=2  color=blue>報竣日︰</font><font size=2  color=red><%=docketdat%></font>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font size=2  color=blue>合約生效日︰</font><font size=2  color=red><%=strbillingdat%></font>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font size=2  color=blue>退租日︰</font><font size=2  color=red><%=dropdat%></font>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font size=2  color=blue>合約終止日︰</font><font size=2  color=red><%=endbillingdat%></font></td>
</tr>
<table width="100%" border=1 cellPadding=0 cellSpacing=0 ID="Table2">
<tr><td class=dataListHead width="10%" align=center>項次</td>
    <td class=dataListHead width="20%" align=center>列帳日期</td>
    <td class=dataListHead width="20%" align=center>付款日期</td>
    <td class=dataListHead width="20%" align=center> 金  額 </td>
    <td class=dataListHead width="20%" align=center>帳單處理次數</td>
    <td class=dataListHead width="20%" align=center>拆帳年月</td>
</tr>
<%
KEYARY=SPLIT(REQUEST("KEY"),";")
sql="select * from AVSMonthlyaccountSRC where COMQ1=" & KEYARY(0) & " AND LINEQ1=" & KEYARY(1) & " AND CUSID='" & KEYARY(2) & "' order by billdat "
rs.Open sql,conn
cnt=0
DO WHILE not rs.EOF 
   cnt=cnt + 1
   k=cnt mod 2
   if k=0 then
      response.write "<tr>" 
      response.Write "<td bgcolor=""lightblue"" align=center> " & cnt & "</td>"
      response.Write "<td bgcolor=""lightblue"" align=center> " & rs("billdat") & "</td>"
      response.Write "<td bgcolor=""lightblue"" align=center> " & rs("paydat") & "</td>"
      response.Write "<td bgcolor=""lightblue"" align=center> " & rs("amt") & "</td>"
      response.Write "<td bgcolor=""lightblue"" align=center> " & rs("billprocessmonth") & "</td>"
      response.Write "<td bgcolor=""lightblue"" align=center> " & rs("rym") & "</td>" 
      response.Write "</tr>"
    else
      response.write "<tr>" 
      response.Write "<td bgcolor=""lightyellow"" align=center> " & cnt & "</td>"
      response.Write "<td bgcolor=""lightyellow"" align=center> " & rs("billdat") & "</td>"
      response.Write "<td bgcolor=""lightyellow"" align=center> " & rs("paydat") & "</td>"
      response.Write "<td bgcolor=""lightyellow"" align=center> " & rs("amt") & "</td>"
      response.Write "<td bgcolor=""lightyellow"" align=center> " & rs("billprocessmonth") & "</td>"
      response.Write "<td bgcolor=""lightyellow"" align=center> " & rs("rym") & "</td>" 
      response.Write "</tr>"
    end if
RS.MoveNext
LOOP
rs.Close
conn.Close
set rs=nothing
set conn=nothing
%>
</table>
<table width="100%" align=right ID="Table3"><tr><TD></td><td align="right">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand" ID="Button1">
</td></tr></table>
</body>
</html>