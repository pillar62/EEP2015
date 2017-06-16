
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim aryStr,s,t,r
  '----距合約到期月數
  S1=document.all("search1").value  
  If Len(s1)=0 Or s1="" Then
  Else
     s=s &"  距合約到期月數: " &S1 & "個月內)"
     t=t &" (  CASE WHEN (12 - DATEDIFF(Month,RTSparqAdslCust.DOCKETDAT, GETDATE())  % 12 )  =  12 THEN 0 ELSE (12 - DATEDIFF(Month,RTSparqAdslCust.DOCKETDAT, GETDATE())  % 12 )  END <= " &S1 &" )" 
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
<CENTER>
<table width="60%">
  <tr class=dataListTitle align=center>Sparq年約年繳用戶到期查詢搜尋條件</td><tr>
</table>
<table width="60%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">距合約到期月數</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="5" name="search1" class=dataListEntry ID="Text1"> 
      <font size=3>個月內</font>
    </td></tr>    
</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>