<%
    parm=request("key")
    v=split(parm,";")
	v(0)= v(0)&";"
%>
<html>
	<head>
		<meta http-equiv="Content-Language" content="zh-tw">
		<title>ET-City 條碼檔匯入作業</title>

<script language=vbscript>
Sub cmdcancel_onClick
  window.close
End Sub
</script>		

	</head>
	<BODY style="BACKGROUND: lightblue">
		<form method="POST" action="upload.asp?key=<%=V(0)%>" enctype="multipart/form-data">
			<table align="center"">
				<tr><tb>請選擇欲匯入之ET-City 條碼檔</tb></tr>
				<tr></tr>
				<tr><td>條碼檔：<input type="file" name="fruit" size="20"></td></tr>
			</table>	
			<p><center>
				<input type="submit" value="匯入" name="subbutt" ID="Submit1">
				<INPUT TYPE="button" VALUE="取消" ID="cmdcancel" NAME="cmdcancel">
			</center><HR>			  
		</form>

		<p><b>注意事項：</b><br><br>
			1.文字檔必須為純文字檔，副檔名為 .barcode<br>
			2.文字檔內容含三個欄位，各欄位以[分號]隔開，不含欄位名稱，每列皆為資料列<br>
			3.三個欄位依順為：Seednet用戶編號, 五大超商條碼, ATM轉帳條碼<br>
		</p>
	</body>
</html>
