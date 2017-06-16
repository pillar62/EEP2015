<html>
	<head>
		<meta http-equiv="Content-Language" content="zh-tw">
		<title>Hinet欠退復文字檔匯入作業</title>

<script language=vbscript>
Sub cmdcancel_onClick
  window.close
End Sub
</script>		

	</head>
	<BODY style="BACKGROUND: lightblue">
		<form method="POST" action="upload.asp" enctype="multipart/form-data">
		<table align="center"">
			<tr><tb>請選擇欲匯入之Hinet欠退復文字檔</tb></tr>
			<tr></tr>
			<tr><td>文字檔：<input type="file" name="fruit" size="20"></td></tr>
		</table>	
		<p><center>
			   <input type="submit" value="匯入" name="subbutt" ID="Submit1">
			  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel" NAME="cmdcancel">
		</center><HR>			  
		</form>
		<p><b>注意事項：</b><br><br>
			1.文字檔必須為純文字檔(.txt)<br>
			2.文字檔內容含七個欄位，各欄位以tab隔開，第一列為欄位名稱，餘為資料列<br>
			3.七個欄位依順為：HN號碼, HB號碼, 申請日期, 狀態, 客戶名稱, 電話, 帳寄地址<br>
			4.文字檔製作請從hinet web網頁拷貝，轉貼Excel，再從Excel轉存為tab字元分隔的文字檔<br>
		</p>
	</body>
</html>
