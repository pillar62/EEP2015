<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim s,t
  '客服單資料狀態
  s4=document.all("search4").value
  s5=document.all("search5").value
  s6=document.all("search6").value  

  if s4<>"" then
     t=t & " and d.comn like '%" & s4 & "%' "
     s=s & "，社區名稱(含)︰'" & s4 & "'字元 " 
  end if
  if s5<>"" then
     t=t & " and b.cusnc like '%" & s5 & "%' "
     s=s & "，用戶名稱(含)︰'" & s5 & "'字元 " 
  end if
  if s6<>"" then
     t=t & " and (b.contacttel like '%" & s6 & "%' or b.mobile like '%" & s6 & "%') "
     s=s & "，用戶電話(含)︰'" & s6 & "'字元 " 
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
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"
			codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px"
	        width=60 >
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<body>
<table width="100%" ID="Table1">
  <tr class=dataListTitle align=center>ET-City續約帳單客戶明細搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0 ID="Table2">

<tr><td class=dataListHead width="40%">社區名稱</td>
<td width="60%"  bgcolor="silver">
<input type="text" size="20" name="search4" class=dataListEntry ID="Text1"> 
</td></tr>

<tr><td class=dataListHead width="40%">客戶名稱</td>
<td width="60%"  bgcolor="silver">
<input type="text" size="20" name="search5" class=dataListEntry ID="Text2"> 
</td></tr>

<tr><td class=dataListHead width="40%">客戶電話</td>
<td width="60%"  bgcolor="silver">
<input type="text" size="20" name="search6" class=dataListEntry ID="Text3"> 
</td></tr>

</table>
<p>
<table width="100%" align=right ID="Table3"><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand" ID="Submit1">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand" ID="Button1">
</td></tr></table>
</body>
</html>