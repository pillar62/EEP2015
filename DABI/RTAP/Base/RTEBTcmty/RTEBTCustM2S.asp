<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim aryStr,s,t,r
    '----社區序號
  s1=document.all("search1").value
  If Len(trim(s)) > 0 Then
     s=s &"  社區序號:('" &s1 & "') "
     t=t &" (RTEBTCUST.COMQ1=" & S1 & ") "
  ELSE
     t=t &" (rtebtcust.comq1 <> 0 ) "
  End If   
  '----主線序號
  s2=document.all("search2").value
  If Len(trim(s2)) > 0 Then
     s=s &"  主線序號:('" &s2 & "') "
     t=t &" AND (RTEBTCUST.LINEQ1=" & S2 & ") "
  End If            
  '----社區名稱
  S3=document.all("search3").value  
  If Len(s1)=0 Or s1="" Then
     t=t &" and (RTEBTCmtyH.ComN<> '*' )" 
  Else
     s=s &"  社區名稱:包含('" &S3 & "'字元)"
     t=t &" and (RTEBTCmtyH.ComN LIKE '%" &S3 &"%')" 
  End If
  '----用戶名稱
  s4=document.all("search4").value
  If Len(trim(s4)) > 0 Then
     s=s &"  用戶名稱:包含('" &s4 & "'字元) "
     t=t &" AND (RTEBTcust.cusnc LIKE '%" & S4 & "%') "
  End If    
  '----AVS合約編號
  s5=document.all("search5").value
  If Len(trim(s5)) > 0 Then
     s=s &"  AVS合約編號:包含('" &s5 & "'字元) "
     t=t &" AND (RTEBTCUST.AVSNO LIKE '%" & S5 & "%') "
  End If       
  '----M2M3資料選擇
  s6ary=split(document.all("search6").value,";")  
  If Len(trim(s6ary(0)))=0 Or s6ary(0)="" Then
  Elseif s6ary(0)="1" then
     s=s &"  資料內容:" &s6ary(1)
     t=t &" AND (RTEBTCustM2M3.M2M3='302') "
  elseif s6ary(0)="2" then
     s=s &"  資料內容:" &s6ary(1)
     t=t &" AND (RTEBTCustM2M3.M2M3='303') " 
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
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"       codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<body>
<table width="100%">
  <tr class=dataListTitle align=center>AVS用戶M2M3資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">社區/主線序號</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="5" name="search1" class=dataListEntry ID="Text5"> 
      <font size=2>-</font>
      <input type="text" size="5" name="search2" class=dataListEntry ID="Text6"> 
    </td></tr>  
<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search3" class=dataListEntry ID="Text1"> 
    </td></tr>    
<tr><td class=dataListHead width="40%">用戶名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search4" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead width="40%">AVS合約編號</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search5" class=dataListEntry> 
    </td></tr>  
<tr><td class=dataListHead width="40%">M2M3資料選擇</td>
    <td width="60%"  bgcolor="silver">
      <select name="search6" size="1" class=dataListEntry ID="Select1">
        <option value=";全部" selected>全部</option>
        <option value="1;M2">M2</option>
      </select>
     </td>
</tr>    

</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>