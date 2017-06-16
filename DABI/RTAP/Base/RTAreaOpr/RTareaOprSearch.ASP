<!-- #include virtual="/webap/include/lockright.inc" -->
<html>
<head>
<meta http-equiv="Pragma" content="no-cache">
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<meta http-equiv="Content-Language" content="zh-tw">
<script language="VBScript">
<!--
Sub btn_onClick()
  Dim s,t
  s=""
  t=""
  aryk1=split(document.all("k1").value,";")
  t=t & " and rtareacty.areaid" & aryk1(0)
  s=s & "施工轄區 " & aryk1(1)
  Dim winP,docP
  Set winP=window.Opener
  Set docP=winP.document
  docP.all("searchqry").value=t
  docP.all("searchshow").value=s
  docP.all("keyform").Submit
  winP.focus()
  window.close
End Sub
-->
</script>
</head>
<body>
<!-- #include virtual="/WebUtilityV3/DBAUDI/DataList.css" -->
<form name="form">
<center>
<div class=datalisttitle>請輸入（選擇）施工轄區代碼資料搜尋條件</div><p>
<table border=1 cellspacing=0 cellpadding=0>
<tr><td class=datalisthead>搜尋條件</td>
<td class=datalistentry>
<select name="k1" size="1">
<%
  Dim conn,rs,sql,s
  Set conn=Server.CreateObject("ADODB.Connection")
  Set rs=Server.CreateObject("ADODB.Recordset")
  sql="SELECT areaid,areanc FROM RTarea where areatype='2' "
  conn.Open "DSN=RTlib"
  rs.Open sql,conn
  s=""
  s=s &"<option value=""<>'*';：全部""" & ">全部</option>" &vbcrlf 
  Do while not rs.Eof
    sel="" 
    s=s &"<option value=""='" &rs("areaid") &"';：" & rs("areanc") &"""" &sel &">" &rs("areanc") &"</option>" &vbCrLf
    rs.MoveNext
  Loop
  rs.Close
  s=s &"</select>"
  Set rs=nothing
  Set conn=nothing
%>
<%=s%>
</td><td>
<input class=datalistbutton type="SUBMIT" name="btn" onsubmit="btn_onclick" value="執　行"><td>
</form>
</body>
</html>
