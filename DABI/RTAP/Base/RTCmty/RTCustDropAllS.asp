<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim aryStr,s,t,r
  '----社區名稱
  S3=document.all("search3").value  
  If Len(s3)=0 Or s3="" Then
  Else
     s=s &"  社區名稱:包含('" &S3 & "'字元)"
     t=t &" AND (RTCmty.COMN LIKE '%" &S3 &"%')" 
  End If
  '用戶名稱
  s4=document.all("search4").value 
  if  Len(trim(s4))=0 Or s4="" then
  else
     s=s & " 用戶名稱︰包含('" & s4 & "')字元 "
     t=t & " and (RTObj.cusnc like '%" & s4 & "%') "
  end if
  '拆機工單號
  s9=document.all("search9").value 
  if  Len(trim(s9))=0 Or s9="" then
  else
     s=s & " 拆機工單號︰包含('" & s9 & "')字元 "
     t=t & " and (RTCUSTDROPSNDWORK.prtno like '%" & s9 & "%') "
  end if   
  '-拆機進度
  s10ary=split(document.all("search10").value,";")  
  If Len(trim(s10ARY(0))) = 0 Then
  '已拆機
  ELSEIF s10ARY(0) = "1" THEN
     s=s &"  拆機進度:('" &s10aRY(1) & "') "
     t=t &" AND (RTCUSTDROPSNDWORK.FINISHDAT is not null) "    
  '已派工尚未拆機
  ELSEIF s10ARY(0) = "2" THEN
     s=s &"  拆機進度:('" &s10aRY(1) & "') "
     t=t &" AND (RTCUSTDROPSNDWORK.prtno is not null) AND (RTCUSTDROPSNDWORK.FINISHDAT is null) "         
  '尚未派工
  ELSEIF s10ARY(0) = "3" THEN
     s=s &"  拆機進度:('" &s10aRY(1) & "') "
     t=t &" AND (RTCUSTDROPSNDWORK.prtno is null) "              
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
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"       codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<body>
<table width="100%">
  <tr class=dataListTitle align=center>HB599用戶退租資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search3" class=dataListEntry ID="Text1"> 
    </td></tr>    
<tr><td class=dataListHead width="40%">用戶名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search4" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead width="40%">拆機工單號</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="15" name="search9" class=dataListEntry ID="Text2"> 
    </td></tr>       
<tr><td class=dataListHead width="40%">退租拆機進度</td>
    <td width="60%" bgcolor="silver">
     <select name="search10" size="1" class=dataListEntry ID="Select2">
        <option value=";全部" selected>全部</option>
        <option value="1;已拆機">已拆機</option>
        <option value="2;已派工尚未拆機">已派工尚未拆機</option>
        <option value="3;尚未派工">尚未派工</option>
     </select>
    </td></tr>            

</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>