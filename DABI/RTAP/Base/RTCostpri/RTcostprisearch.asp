<!-- #include virtual="/webap/include/lockright.inc" -->
<html>
<head>

<script language="VBScript">
<!--
Sub btn_onClick()
  Dim winP,docP
  Set winP=window.Opener
  Set docP=winP.document
  docP.all("search1").value=document.all("T1").value
  docP.all("keyform").Submit
  winP.focus()
  window.close
End Sub
-->
</script>
</head>
<body>
<!-- #include virtual="/WebUtility/DBAUDI/DataList.css" -->
<form name="form">
<center>
<div class=datalisttitle>請輸入（選擇）搜尋條件</div><p>
<table border=1 cellspacing=0 cellpadding=0>
<tr><td class=datalisthead>搜尋條件</td>
<td>
<input class=dataLISTENTRY MAXLENGTH=9 SIZE=9 type="text" name="T1"><td>
</td>
<td>
<input class=datalistbutton type="SUBMIT" name="btn" onsubmit="btn_onclick" value="執　行"><td>
</form>
</body>
</html>