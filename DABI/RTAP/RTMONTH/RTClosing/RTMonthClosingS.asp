<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
Sub btn_onClick()
  dim aryStr,s,t,r,k1
  k1=document.all("search1").value
  if len(trim(k1)) > 0 then
     s="客戶名稱：含('" & K1 & "')字元"
     t=t&" (RTFAQH.FaQMan like '%" & k1 & "%') and "
  end if
  aryStr=Split(document.all("search2").value,";")
  s=s & "　處理狀態：" &aryStr(1) &"  "
  if arystr(0) = "1" then
     t=t& " (RTFAQH.finishdate is Null and RTFAQH.dropdate is null )"
  elseif arystr(0) = "2" then
     t=t& " (RTFAQH.finishdate is Not Null)"
  elseif arystr(0) = "3" then
     t=t& " (RTFAQH.dropdate is Not Null)"
  else
     t=t& " (RTFAQH.caseno<>'*') "
  end if
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
  <tr class=dataListTitle align=center>客訴資料查詢</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">客戶名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search1" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead width="40%">處理狀態</td>
    <td width="60%"  bgcolor="silver">
      <select name="search2" size="1" class=dataListEntry>
        <option value="1;未結案">未結案</option>
        <option value="2;已結案">已結案</option>
        <option value="3;已作廢">已作廢</option>
        <option value="4;全部">全部</option>        
      </select>
     </td>
</tr>

</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="button" value=" 查詢 " class=dataListButton name="btn" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>