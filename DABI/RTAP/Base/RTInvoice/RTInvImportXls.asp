<html>
	<head>
		<meta http-equiv="Content-Language" content="zh-tw">
		<title>發票Excel檔匯入作業</title>

<script language=vbscript>
Sub cmdcancel_onClick
  window.close
End Sub
</script>		
	</head>
	<BODY style="BACKGROUND: lightblue">
		<form method="POST"  action="RTInvImportSql.asp" enctype="multipart/form-data">
		<table align="center"">
			<tr><tb>請選擇欲匯入之發票檔</tb></tr>
			<tr></tr>
			<tr><td>Excel檔：<input type="file" name="fruit" size="20"></td></tr>
		</table>	
		<p><center>
			   <input type="submit" value="匯入" name="subbutt" ID="Submit1">
			  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel" NAME="cmdcancel">
		</center><HR>			  
		</form>
		<p><b>注意事項：</b><br><br>
			1.上傳之文字檔必須為Excel檔(.xls)<br>
		</p>
	</body>
</html>
