<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
	parm=split(REQUEST("KEY"),";")
	Call SrGetEmployeeRef(Rtnvalue,1,session("userid"))
	V=split(rtnvalue,";")  	

    'Dim rs,conn,S1
    'Set conn=Server.CreateObject("ADODB.Connection")
    'conn.open "DSN=RTLib"
    'Set rs=Server.CreateObject("ADODB.Recordset")    
    'rs.Open "select code, codenc from rtcode where kind ='P9' ",conn
    's1="<option value="""" selected></option>" &vbCrLf
    'Do While Not rs.Eof
    '   s1=s1 &"<option value=""" &rs("code") & """>" &rs("codenc") &"</option>" &vbCrLf
    '   rs.MoveNext
    'Loop
    'rs.Close

    'conn.Close
    'Set rs=Nothing
    'Set conn=Nothing
%>
<html>
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=big5">
		<title>網路電話派工單完工作業</title>
		<script language="vbscript">
			Sub cmdSure_onClick
				'finishtyp = document.all("search1").value
				'if len(finishtyp)=0 then
				'	msgbox "請選擇完工種類!!", vbCritical+vbOKOnly
				'else
					'pgm="RTFaqSndWrkF.asp?key=" & "<%=parm(0)%>" &";"& finishtyp &";"& "<%=v(0)%>" &";"
					pgm="RTSparqWagalySndWrkF.asp?key=" & "<%=parm(0)%>" &";"& "<%=parm(1)%>" &";"& "<%=v(0)%>" &";"
					window.open pgm 
					'window.close	
				'end if
			End Sub

			Sub cmdcancel_onClick
				window.close
			End Sub
		</script>
	</head>
	<BODY style="BACKGROUND: lightblue">
	<!--
		<table align="center">
			<tr>
				<td ALIGN="RIGHT"><font face="標楷體">請選擇完工種類：</font></td>
				<td><select name="search1" size="1" class="dataListEntry" ID="Select1"><%=s1%></select></td>
			</tr>
		</table>
	-->
		<p><center>
				<INPUT TYPE="button" VALUE="送出" ID="cmdSure" name="cmdSure" style="font-family: 標楷體; color: #FF0000;cursor:hand">
				<INPUT TYPE="button" VALUE="取消" ID="cmdcancel" name="cmdcancel" style="font-family: 標楷體; color: #FF0000;cursor:hand">
			</center>
		</p>
		<HR>
	</BODY>
</html>
