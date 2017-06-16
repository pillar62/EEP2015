
<html>
<head>
<meta http-equiv="Pragma" content="no-cache">
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<meta http-equiv="Content-Language" content="zh-tw">
<script language="VBScript">
<!--
Sub btn_onClick()
  on error resume next
  Dim winP,docP
  Set winP=window.Opener
  Set docP=winP.document
  s=""
  arykey=split(document.all("k1").value,";")
  s="業務轄區 " & arykey(1)
  t=""
  t=" and rtareacty.areaid " & arykey(0)
  docP.all("searchQry").value=t
  docP.all("searchShow").value=s
  docP.all("keyform").Submit
  winP.focus()
  window.close
End Sub
Sub Btn1_onclick()
  on error resume next
  Dim winP
  Set winP=window.Opener
  winP.focus()
  window.close  
End Sub
-->
</script>
</head>
<body>
<!-- #include virtual="/WebUtilityv3/DBAUDI/DataList.css" -->
<form name="form">
<center>
<div class=datalisttitle>請輸入（選擇）業務轄區代碼資料搜尋條件</div><p>
<table border=1 cellspacing=0 cellpadding=0>
<tr><td class=dataListHead width="40%">轄區名稱</td><td class=datalistentry>
<select name="k1" size="1" >
<%
  Dim conn,rs,sql,s
  Set conn=Server.CreateObject("ADODB.Connection")
  Set rs=Server.CreateObject("ADODB.Recordset")
  sql="SELECT areaid,areanc FROM RTarea where areatype='1' "
  conn.Open "DSN=RTlib"
  rs.Open sql,conn
  s=""
  s=s &"<option value=""<>'*';：全部""" &  ">全部</option>" &vbcrlf 
  Do while not rs.Eof
    s=s &"<option value=""='" & rs("areaid") & "';："  & rs("areanc")  & """>" &rs("areanc") &"</option>" &vbCrLf
    rs.MoveNext
  Loop
  rs.Close
  s=s &"</select>"
  Set rs=nothing
  Set conn=nothing
%>
<%=s%>
</td></tr></table>
<table>
<tr><td></td><td align="right">
<input class=datalistbutton type="SUBMIT" name="btn" onsubmit="btn_onclick" value=" 查詢 ">
<input class=datalistbutton type="button" name="btn1" value=" 結束 "></td>
</tr></table>
</form>
</body>
</html>
