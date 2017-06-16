<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim aryStr,s,t,r
  '----社區名稱
  S1=document.all("search1").value  
  If Len(s1)=0 Or s1="" Then
     t=t &" a.comq1 >0 " 
  Else
     s=s &"  社區名稱:包含('" &S1 & "'字元)"
     t=t &" a.comn LIKE '%"& S1 &"%' " 
  End If
  '----社區地址
  S2=trim(document.all("search2").value)
  If Len(s2) >0 Then
     s=s &"  社區地址:包含('" &S2 & "'字元)"
     t=t &" and b.cutnc + a.township + a.raddr LIKE '%"& S2 &"%' " 
  End If

  Dim winP,docP
  Set winP=window.Opener
  Set docP=winP.document
  docP.all("searchQry").value=t
  docP.all("searchShow").value=s
  docP.all("keyform").Submit
  winP.focus()
  window.close
End Sub

Sub btn1_onClick()  
  Dim winP
  Set winP=window.Opener
  winP.focus()
  window.close  
End Sub
-->
</script>
</head>
<body>
<table width="100%">
  <tr class=dataListTitle align=center>社區基本資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>

<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%" bgcolor="silver"><input type="text" size="20" name="search1" class=dataListEntry></td>
</tr>
<tr><td class=dataListHead width="40%">社區地址</td>
    <td width="60%" bgcolor="silver"><input type="text" size="20" name="search2" class=dataListEntry></td>
</tr>

</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>