
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim aryStr,s,t,r
  '客服單資料狀態
  s1ary=split(document.all("search1").value,";")
  s2=document.all("search2").value
  s3=document.all("search3").value
  s4=document.all("search4").value
  s5=document.all("search5").value
  if len(trim(s2)) > 0 and not IsNumeric(s2) then
     msgbox "社區序號必須為數字"
  elseif len(trim(s3)) > 0 and not IsNumeric(s3) then
     msgbox "主線序號必須為數字"
  else
  if s1ary(0)="" then
     t=t & " RTLessorCustFaqH.cusid <> '' "
     s="全部"
  elseif s1ary(0)="1" then
     t=t & " RTLessorCustFaqH.finishdat is not null  "
     s="已結案 "
  elseif s1ary(0)="2" then
     t=t & " RTLessorCustFaqH.finishdat is null  and RTLessorCustFaqH.canceldat is null "
     s="未結案 "     
  elseif s1ary(0)="3" then
     t=t & " RTLessorCustFaqH.canceldat is not null AND RTLessorCustFaqH.finishdat is null "
     s="已作廢 "     
  elseif s1ary(0)="4" then
     t=t & " RTLessorCustFaqH.canceldat is null "
     s="全部(不含作廢)"
  end if
  if len(trim(s2)) > 0 then
     t=t & " and RTLessorCust.comq1=" & s2 & " "
     s=s & "，社區序號︰" & s2 
     if len(trim(s3)) > 0 then
        t=t & " and RTLessorCust.lineq1=" & s3 & " "
        s=s & "，主線序號︰" & s3 
     end if
  end if
  if s4<>"" then
     t=t & " and RTLessorCmtyh.comn like '%" & s4 & "%' "
     s=s & "，社區名稱(含)︰'" & s4 & "'字元 " 
  end if
  if s5<>"" then
     t=t & " and RTLessorcust.cusnc like '%" & s5 & "%' "
     s=s & "，用戶名稱(含)︰'" & s5 & "'字元 " 
  end if  
  Dim winP,docP
  Set winP=window.Opener
  Set docP=winP.document
  docP.all("searchQry").value=t
  docP.all("searchShow").value=s
  docP.all("keyform").Submit
  winP.focus()
  window.close
  end if
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
  <tr class=dataListTitle align=center>ET-City用戶客訴單資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">社區-主線序號</td>
<td width="60%"  bgcolor="silver">
<input type="text" size="5" name="search2" class=dataListEntry>-
<input type="text" size="5" name="search3" class=dataListEntry> 
</td>
</tr>  
<tr><td class=dataListHead width="40%">社區名稱</td>
<td width="60%"  bgcolor="silver">
<input type="text" size="20" name="search4" class=dataListEntry> 
</td>
</tr>  
<tr><td class=dataListHead width="40%">客戶名稱</td>
<td width="60%"  bgcolor="silver">
<input type="text" size="20" name="search5" class=dataListEntry> 
</td>
</tr>  
<tr><td class=dataListHead width="40%">客訴單狀態</td>
    <td width="60%"  bgcolor="silver">
      <select name="search1" size="1" class=dataListEntry ID="Select1">
        <option value=";全部" >全部</option>
        <option value="1;已結案">已結案</option>      
        <option value="2;未結案" selected>未結案</option>  
        <option value="3;已作廢">已作廢</option>          
        <option value="4;全部(不含作廢)">全部(不含作廢)</option>                     
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