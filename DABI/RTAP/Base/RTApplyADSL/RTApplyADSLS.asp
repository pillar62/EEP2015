
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV3/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim s,t
  t=""
  s=""
  s1=document.all("search1").value
  if len(trim(s1)) > 0 then
     s="社區名稱：含('" & s1 & "')字元"  
     t=t & " rtcmty.comn like '%" & s1 & "%' "
  else
     s="社區名稱：全部  "
     t=t & " rtcust.comq1 <> 0 "
  end if
  s2=document.all("search2").value
  if len(trim(s2)) > 0 then
     s="  客戶名稱：含('" & s2 & "')字元"  
     t=t & " and rtobj.cusnc like '%" & s2 & "%'"
  else
     s=s & "  客戶名稱：全部  "
     t=t & " and rtcust.cusid <>'*'"
  end if  
  s3=document.all("search3").value
  if len(trim(s3)) > 0 then
     s="  HN聯單號碼：含('" & s3 & "')字元"  
     t=t & " and rtCUST.cusno like '%" & s3 & "%'"
  else
     s=s & "  HN聯單號碼：全部  "
     t=t & " and rtcust.cusNO <>'*'"
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
  window.close
End Sub
-->
</script>
</head>
<body>
<center>
<table width="70%">
  <tr class=dataListTitle align=center>請輸入(選擇)客戶資料搜尋條件</td><tr>
</table>
<table width="70%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">客戶名稱</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search1" size="25" maxlength="15" class=dataListEntry>
    </td></tr>
<tr><td class=dataListHead width="40%">申請類別</td>
    <td width="60%" bgcolor="silver" >
      <select name="search8" size="1" class=dataListEntry>
        <option value="*;全部" selected>全部</option>
        <option value="A1;分享型599">同　意</option>
        <option value="A2;分享型399">不同意</option>
        <option value="A3;">暫　緩</option>        
        <option value=";未審核">未審核</option>
      </select>
    </td></tr>  
<tr><td class=dataListHead width="40%">申請狀態</td>
    <td width="60%" bgcolor="silver" >
      <select name="search8" size="1" class=dataListEntry>
        <option value="*;全部" selected>全部</option>
        <option value="Y;同意">同　意</option>
        <option value="N;不同意">不同意</option>
        <option value="H;暫緩">暫　緩</option>        
        <option value=";未審核">未審核</option>
      </select>
    </td></tr>       
</table>
<table width="70%" align=right><tr><td></td><td align=right>
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</body>
</html>