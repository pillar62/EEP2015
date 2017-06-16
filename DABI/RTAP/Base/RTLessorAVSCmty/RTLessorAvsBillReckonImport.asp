<%
    parm=request("key") & ";"
    v=split(parm,";")
	'v(0)= v(0)&";"
%>
<html>
	<head>
		<meta http-equiv="Content-Language" content="zh-tw">
		<title>Seednet逢8結算檔匯入作業</title>

<script language=vbscript>
Sub cmdcancel_onClick
  window.close
End Sub
</script>		

	</head>
	<BODY style="BACKGROUND: lightblue">
		<form method="POST" action="uploadReckon.asp?key=<%=V(0)%>" enctype="multipart/form-data">
			<table align="center">
				<tr><tb>請選擇欲匯入之Seednet結算檔</tb></tr>
				<tr></tr>
				<tr><td>條碼檔：<input type="file" name="fruit" size="20"></td></tr>
			</table>	
			<p><center>
				<input type="submit" value="匯入" name="subbutt" ID="Submit1">
				<INPUT TYPE="button" VALUE="取消" ID="cmdcancel" NAME="cmdcancel">
			</center><HR>			  
		</form>

	</body>
</html>
