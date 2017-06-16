<%
    parm=request("parm")
    v=split(parm,";")
%>
<html>
	<head>
		<meta http-equiv="Content-Language" content="zh-tw">
		<title>Sparq499 Excel轉檔上傳作業</title>
<script language=vbscript>
Sub cmdcancel_onClick
  window.close
End Sub
</script>		
	</head>
	<BODY style="BACKGROUND: lightblue">
		<form method="POST" action="upload499.asp?parm=<%=v(0)%>" enctype="multipart/form-data">
		<table align="center"">
			<tr><tb>請選擇欲上傳之Sparq Excel檔</tb></tr>
			<tr></tr>
			<tr><td>Excel檔(499方案)：<input type="file" name="fruit" size="30"></td></tr>
		</table>	
		<p><center>
			   <input type="submit" value="上傳" name="subbutt" ID="Submit1">
			  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel" NAME="cmdcancel">
		</center><HR>			  
		</form>
		<p><b>注意事項：</b><br><br>
			1.文字檔必須為Excel檔<br>
		</p>
	</body>
</html>
