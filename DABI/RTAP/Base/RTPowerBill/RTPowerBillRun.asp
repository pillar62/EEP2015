<%
  '  Dim rs,conn,S6
  '  Set conn=Server.CreateObject("ADODB.Connection")
  '  conn.open "DSN=RTLib"
  '  Set rs=Server.CreateObject("ADODB.Recordset")    
  '  S6=""
  '  rs.Open "SELECT RTStock.CUSID, RTObj.SHORTNC FROM RTStock INNER JOIN " _
  '         &"RTObj ON RTStock.CUSID = RTObj.CUSID",CONN
  '  s1="<option value=""<>'*';全部"" selected>全部</option>" &vbCrLf    
  '  Do While Not rs.Eof
  '     s6=s6 &"<option value=""" &rs("CUSID") & """>" &rs("SHORTNC") &"</option>"
  '     rs.MoveNext
  '  Loop
  '  rs.Close
'----------
  '  conn.Close
  '  Set rs=Nothing
  '  Set conn=Nothing

Edate=DateValue(Now())

run = request.Form("run")
if run ="Y" then 
	response.Write "OK"
else
	response.Write "not yet"
end if
%>

<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>電費補助計算</TITLE>
<SCRIPT language=VBScript>

Sub cmdcancel_onClick
  window.close
End Sub
</SCRIPT>
</HEAD>
<BODY style="background:lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">電費補助計算</font></i> </DIV>
<P></P>
<form name=form1 method=post action=RTPowerBillRun.asp target=_self>
<table align="center" border=0 cellPadding=0 cellSpacing=0>
<tr><td><font face="標楷體">計算年月起迄 :</font></td></tr>
<tr><td><input type="text" size="5" maxlength="4" name="years">
		<select name="months" size="1" class=dataListEntry>
	   		<option value="1">1</option><option value="2">2</option>
			<option value="3">3</option><option value="4">4</option>
			<option value="5">5</option><option value="6">6</option>
			<option value="7">7</option><option value="8">8</option>
			<option value="9">9</option><option value="10">10</option>
			<option value="11">11</option><option value="12">12</option>
		</select>～</td>
	<td><input type="text" size="5" maxlength="4" NAME="yeare">
		<select name="monthe" size="1" class=dataListEntry>
	   		<option value="1">1</option><option value="2">2</option>
			<option value="3">3</option><option value="4">4</option>
			<option value="5">5</option><option value="6">6</option>
			<option value="7">7</option><option value="8">8</option>
			<option value="9">9</option><option value="10">10</option>
			<option value="11">11</option><option value="12">12</option>
		</select></td></tr>
</table>
<HR>
<p><center><font face="標楷體">
	<INPUT TYPE="submit" VALUE="送出" style="font-family: 標楷體; color: #FF0000"> 
	<INPUT TYPE="button" VALUE="取消" ID="cmdcancel" style="font-family: 標楷體; color: #FF0000">
	<input type="text" name="run" value="Y" style="display:none">
	</center>
</p> 
</form>
</BODY>
</HTML>