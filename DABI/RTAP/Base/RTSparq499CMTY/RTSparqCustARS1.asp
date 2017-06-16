<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim aryStr,s,t1,t2,r
  '應收應付帳款狀態
  s1ary=split(document.all("search1").value,";")
  if s1ary(0)="" then
     t1=t1 & " a.cusid <> '' "
     t2=t1
     s="全部 "
  elseif s1ary(0)="1" then
     t1=t1 & " a.canceldat is null and ( a.amt - a.realamt = 0 ) "
     t2=t1
     s="已沖帳 "
  elseif s1ary(0)="2" then
     t1=t1 & " a.canceldat is null and ( a.amt - a.realamt <> 0 ) "
     t2=t1
     s="未沖帳或部份沖帳 "
  elseif s1ary(0)="3" then
     t1=t1 & " a.canceldat is not null "
     t2=t1
     s="已作廢 "
  elseif s1ary(0)="4" then
     t1=t1 & " a.canceldat is null "
     t2=t1
     s="全部(不含作廢) "
  elseif s1ary(0)="5" then
     t1=t1 & " a.canceldat is null and ( a.amt - a.realamt < 0 ) "
     t2=t1
     s="未沖金額為負 "
  end if
  '用戶名稱
  s2=document.all("search2").value
  if len(trim(s2)) > 0 then
     t1=t1 & " and i.cusnc like '%" & s2 & "%' "
     t2=t2 & " and b.cusnc like '%" & s2 & "%' "
     s=s & "，用戶名稱(含)︰'" & s2 & "'字元 "
  end if
    '社區代號
  s3=document.all("search3").value
  if len(trim(s3)) > 0 then
     t1=t1 & " and b.comq1 =" & s3 &" "
     t2=t2 & " and b.comq1 =" & s3 &" "
     s=s & "，社區代號︰" & s3 & " "
  end if
    '社區名稱
  s4=document.all("search4").value
  if len(trim(s4)) > 0 then
     t1=t1 & " and c.comn like '%" & s4 & "%' "
     t2=t2 & " and c.comn like '%" & s4 & "%' "
     s=s & "，社區名稱(含)︰'" & s4 & "'字元 "
  end if
 
  Dim winP,docP
  Set winP=window.opener
  Set docP=winP.document
  docP.all("searchQry").value=t1
  docP.all("searchQry2").value=t2
  docP.all("searchShow").value=s
  docP.all("keyform").Submit
  winP.focus()
  window.close
End Sub
Sub Srbtnonclick()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="search" & clickid
	   if isdate(document.all(clickkey).value) then
	      objEF2KDT.varDefaultDateTime=document.all(clickkey).value
       end if
       call objEF2KDT.show(1)
       if objEF2KDT.strDateTime <> "" then
          document.all(clickkey).value = objEF2KDT.strDateTime
       end if
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
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<body>
<table width="100%">
  <tr class=dataListTitle align=center>速博用戶應收應付帳款資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">社區代號</td>
    <td width="60%"  bgcolor="silver">
      <input type="text" size="5" name="search3" class=dataListEntry> 
     </td>
</tr>    
<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%"  bgcolor="silver">
      <input type="text" size="20" name="search4" class=dataListEntry> 
     </td>
</tr>    
<tr><td class=dataListHead width="40%">用戶名稱</td>
    <td width="60%"  bgcolor="silver">
      <input type="text" size="12" name="search2" class=dataListEntry> 
     </td>
</tr>    
<tr><td class=dataListHead width="40%">應收應付帳款狀態</td>
    <td width="60%"  bgcolor="silver">
      <select name="search1" size="1" class=dataListEntry ID="Select1">
        <option value="2;全部未沖帳" selected>全部未沖帳</option>
        <option value="1;已沖帳">已沖帳</option>
        <option value="3;已作廢">已作廢</option>      
        <option value="4;全部(不含作廢)">全部(不含作廢)</option>
        <option value="5;未沖金額為負">未沖金額為負</option>
        <option value=";全部">全部</option>               
      </select>
     </td>
</tr>    
</table>
<p>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>