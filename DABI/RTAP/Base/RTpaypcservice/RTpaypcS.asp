<!-- #include virtual="/webap/include/lockright.inc" -->
<html>
<head>

<script language="VBScript">
<!--
Sub btn_onClick()
  Dim winP,docP,S,t
  t=""
  s=""
  s1ary=split(document.all("D1").value,";")
  s2=document.all("search2").value
  s=s & "RT施工費用明細表列印狀況：" & s1ary(1) & "  " & "列印批號：含('" & s2 & "')字元"
  if s1ary(0)="1" then
     t=t & " and datalength(rtrim(rtcust.paydtlprtno)) >0 "
  elseif s1ary(0)="2" then
     t=t & " and  rtcust.paydtldat IS not NULL "
  elseif s1ary(0)="3" then
     t=t & " and  rtcust.paydtldat Is null "
 
  end if
  if len(trim(s2))> 0 then
     t=t & " and rtcust.paydtlprtno like '%" & s2 & "%'"
  end if     
  '---t transfor to array
  't=t & ";" & s2
  Set winP=window.Opener
  Set docP=winP.document
  docP.all("searchQry").value=t
  docP.all("searchShow").value=s  
  docP.all("keyform").Submit
  winP.focus()
  window.close
End Sub
Sub btn1_onClick()
  window.close
End Sub
-->
</script>
</head>
<body>
<!-- #include virtual="/WebUtility/DBAUDI/DataList.css" -->
<form name="form">
<center>
<div class=datalisttitle>請輸入（選擇）RT施工費用明細表列印搜尋條件</div><p>
<table border=1 cellspacing=0 cellpadding=0>
<tr>
<td class=datalisthead>RT施工費用明細表列印狀況</td>
<td bgcolor="silver">
	<select class=dataLISTENTRY name="D1" size=1 readonly>
	    <option value="2;已列印">已列印</option></td></TR></select>
		<!--
		<option value="1;全部">全  部</option>
		<option value="2;已審核">已審核</option>
		-->
<tr><td class=datalisthead>列印批號</td><td bgcolor="silver">
	<input class=datalistentry name="search2" size="9" maxlength="9">
</td></tr>
</table>
<table><tr><td></td><td align="right">
	<input class=datalistbutton type="submit" name="btn" onsubmit="btn_onclick" value="查　詢">
    <input class=datalistbutton type="button" name="btn1" value="結　束">
</td></tr></table>
</form>
</body>
</html>