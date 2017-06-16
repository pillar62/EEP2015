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
  <tr class=dataListTitle align=center>KTS用戶繳款狀況查詢</td><tr>
  </table>
  <u></u>
<%
set conn=server.CreateObject("ADODB.connection")
set rs=server.CreateObject("ADODB.recordset")
dsn="DSN=RTLIB"
conn.Open dsn
KEYARY=SPLIT(REQUEST("KEY"),";")
sql="select * FROM KTSCUST WHERE CUSID='" & KEYARY(0) & "'"
RS.Open SQL,CONN
IF NOT RS.EOF THEN
   CUSNC=RS("CUSNC")
   FINISHDAT=RS("FINISHDAT")
   DROPDAT=RS("DROPDAT")

ELSE
   CUSNC=""
   FINISHDA=""
   DROPDAT=""

END IF
RS.CLOSE
%>  
<table width="100%" border=0 cellPadding=0 cellSpacing=0 ID="Table4">
<tr>
<td><font size=2  color=blue>用戶名稱︰</font><font size=2  color=red><%=cusnc%></font>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br><font size=2  color=blue>報竣日︰</font><font size=2  color=red><%=FINISHdat%></font>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font size=2  color=blue>退租日︰</font><font size=2  color=red><%=dropdat%></font>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
</tr>
<table width="100%" border=1 cellPadding=0 cellSpacing=0 ID="Table2">
<tr><td class=dataListHead width="10%" align=center>項次</td>
    <td class=dataListHead width="20%" align=center>列帳日期</td>
    <td class=dataListHead width="20%" align=center>金額</td>
    <td class=dataListHead width="20%" align=center>拆帳比率</td>
    <td class=dataListHead width="20%" align=center>拆帳金額</td>
    <td class=dataListHead width="20%" align=center>拆帳年月</td>
</tr>
<%
KEYARY=SPLIT(REQUEST("KEY"),";")
sql="select * from KTSMonthlyaccountSRC where CUSID='" & KEYARY(0) & "' order by SERVICECYCLE "
rs.Open sql,conn
cnt=0
DO WHILE not rs.EOF 
   cnt=cnt + 1
   k=cnt mod 2
   if k=0 then
      response.write "<tr>" 
      response.Write "<td bgcolor=""lightblue"" align=center> " & cnt & "</td>"
      response.Write "<td bgcolor=""lightblue"" align=center> " & rs("billCYCLE") & "</td>"
      response.Write "<td bgcolor=""lightblue"" align=center> " & rs("servicevalue") & "</td>"
      response.Write "<td bgcolor=""lightblue"" align=center> " & rs("rate") & "%</td>"
      response.Write "<td bgcolor=""lightblue"" align=center> " & rs("commissionamount") & "</td>"
      response.Write "<td bgcolor=""lightblue"" align=center> " & rs("servicecycle") & "</td>" 
      response.Write "</tr>"
    else
      response.write "<tr>" 
      response.Write "<td bgcolor=""lightyellow"" align=center> " & cnt & "</td>"
      response.Write "<td bgcolor=""lightyellow"" align=center> " & rs("billCYCLE") & "</td>"
      response.Write "<td bgcolor=""lightyellow"" align=center> " & rs("servicevalue") & "</td>"
      response.Write "<td bgcolor=""lightyellow"" align=center> " & rs("rate") & "%</td>"
      response.Write "<td bgcolor=""lightyellow"" align=center> " & rs("commissionamount") & "</td>"
      response.Write "<td bgcolor=""lightyellow"" align=center> " & rs("servicecycle") & "</td>" 
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