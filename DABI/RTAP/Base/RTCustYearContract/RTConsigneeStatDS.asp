<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
sub b1_onclick()
	if isdate(document.all("search3").value) then
		objEF2KDT.varDefaultDateTime=document.all("search3").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search3").value = objEF2KDT.strDateTime
	end if
end sub

sub b2_onclick()
	if isdate(document.all("search4").value) then
		objEF2KDT.varDefaultDateTime=document.all("search4").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search4").value = objEF2KDT.strDateTime
	end if
end sub


Sub btn_onClick()
  dim aryStr,s,t,r
  '----退租日起迄
  symd=document.all("search3").value
  eymd=document.all("search4").value 
  IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
  IF LEN(TRIM(EYMD))=0 THEN SYMD="2070/12/31"
  t=t &" DROPDAT between '"& symd &"' and '"& eymd &"' " 
  s=s &" 退租日起迄： "& symd & "～" & eymd
  
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
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codeBase=http://www.cbbn.com.tw/stock/EF2KDT.CAB#version=9,0,0,3 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<body>
<% 
	yyyy= Datepart("yyyy", Now())
	m = Datepart("m", Now())
	Sdate = yyyy &"/"& m &"/"& "1"
	
	Edate=Dateadd("m", 1, Now())
	yyyy= Datepart("yyyy", Edate)
	m = Datepart("m", Edate)
	Edate = Cdate(yyyy &"/"& m &"/"& "1")
	Edate = Dateadd("d", -1, Edate)
%>  
<table width="100%">
  <tr class=dataListTitle align=center>退租統計搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>

<tr><td class=dataListHead width="40%">退租日起迄</td>
<td width="60%" bgcolor="silver">
   <input size="10" maxlength="10" name="search3" align=right class=dataListEntry value="<%=Sdate%>" readonly ID="Text1">
   <input type="button" id="B1" name="B1" height="100%" width="100%" style="Z-INDEX: 1" value="....">
	～
   <input size="10" maxlength="10" name="search4" align=right class=dataListEntry value="<%=Edate%>" readonly ID="Text2">
   <input type="button" id="B2" name="B2" height="100%" width="100%" style="Z-INDEX: 1" value="...."></td>
   
</tr><BR>

</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>
