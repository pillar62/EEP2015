<%
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim s,t,t1
  t=""
  s=""
  s1=document.all("search1").value
  if len(trim(s1)) > 0 then
     s="同一社區名稱有=>" & s1 & " 人成行"
     t=t & " singlecustadsl.cusid <> '*' "
     t1=" Having Count(*) >= " & s1 
  else
     s="客戶名稱：全部  "
     t=t & " singlecustadsl.cusid <> '*' "
     t1=" Having Count(*) >= 3"  
  end if

  Dim winP,docP
  Set winP=window.Opener
  Set docP=winP.document
  docP.all("searchQry").value=t
  docP.all("searchQry2").value=t1
  docP.all("searchShow").value=s
  docP.all("keyform").Submit
  winP.focus()
  window.close
End Sub
Sub btn1_onClick()
  Dim winP,docP
  Set winP=window.Opener
  Set docP=winP.document
  winP.focus()
  window.close
End Sub
Sub SrReNew()
  Window.form.Submit
End Sub
-->
</script>
</head>
<body>
<form method="post" id="form">
<center>
<table width="80%">
  <tr class=dataListTitle align=center>請輸入(選擇)客戶資料搜尋條件</td><tr>
</table>
<table width="80%" border=1 cellPadding=0 cellSpacing=0>
 
<tr><td class=dataListHead width="30%">同一社區名稱有</td>
    <td width="70%" bgcolor="silver" >
    <input type=text name="search1" size="25" maxlength="25" class=dataListEntry>人成行
    </td></tr>
</table>
<table width="80%" align=right><tr><td></td><td align=right>
  <input type="submit" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</FORM>
</body>
</html>
<!-- #include file="rtgetBRANCHBUSS.inc" -->