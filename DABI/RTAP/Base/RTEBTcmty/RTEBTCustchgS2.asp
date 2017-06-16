
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
  If Len(trim(s1)) > 0 Then
     s=s &"  社區序號:('" &s1 & "') "
     t=t &" (RTEBTCUSTchg.COMQ1=" & S1 & ") "
  ELSE
     t=t &" (RTEBTCUSTchg.COMQ1 <> 0 ) "
  End If   
  '----主線序號
  s2=document.all("search2").value
  If Len(trim(s2)) > 0 Then
     s=s &"  主線序號:('" &s2 & "') "
     t=t &" AND (RTEBTCUSTchg.LINEQ1=" & S2 & ") "
  End If              
  '----社區名稱
  S3=document.all("search3").value  
  If Len(s3)=0 Or s3="" Then
  Else
     s=s &"  社區名稱:包含('" &S3 & "'字元)"
     t=t &" AND (RTEBTCmtyH.ComN LIKE '%" &S3 &"%')" 
  End If
  '用戶名稱
  s4=document.all("search4").value 
  if  Len(trim(s4))=0 Or s4="" then
  else
     s=s & " 用戶名稱︰包含('" & s4 & "')字元 "
     t=t & " and (rtebtcust.cusnc like '%" & s4 & "%') "
  end if
  '用戶合約編號
  s5=document.all("search5").value 
  if  Len(trim(s5))=0 Or s5="" then
  else
     s=s & " 合約編號︰包含('" & s5 & "')字元 "
     t=t & " and (rtebtcust.avsno like '%" & s5 & "%') "
  end if  
 '聯絡電話
  s6=document.all("search6").value 
  if  Len(trim(s6))=0 Or s6="" then
  else
     s=s & " 聯絡電話︰包含('" & s6 & "')字元 "
     t=t & " and (rtebtcust.contacttel + rtebtcust.mobile like '%" & s6 & "%') "
  end if   
  '身分證/統編
  s7=document.all("search7").value 
  if  Len(trim(s7))=0 Or s7="" then
  else
     s=s & " 身份證/統編︰包含('" & s7 & "')字元 "
     t=t & " and (rtebtcust.socialid like '%" & s7 & "%') "
  end if     
  '----異動項目
  s8ary=split(document.all("search8").value,";")  
  If Len(trim(s8ARY(0))) = 0 Then
  '變更用戶資料
  ELSEIF s8ARY(0) = "1" THEN
     s=s &"  異動項目:含('" &s8ARY(1) & "') "
     t=t &" AND (RTEBTCUSTchg.chgcod1 > 0 ) "
  '換號
  ELSEIF s8ARY(0) = "2" THEN
     s=s &"  異動項目:含('" &s8ARY(1) & "') "
     t=t &" AND (RTEBTCUSTchg.chgcod2 > 0 ) "
  '移機
  ELSEIF s8ARY(0) = "3" THEN
     s=s &"  異動項目:含('" &s8ARY(1) & "') "
     t=t &" AND (RTEBTCUSTchg.chgcod3 > 0 ) "
  '其它
  ELSEIF s8ARY(0) = "4" THEN
     s=s &"  異動項目:含('" &s8ARY(1) & "') "
     t=t &" AND (RTEBTCUSTchg.chgcod4 > 0 ) "            
  End If     
  '移機工單號
  s9=document.all("search9").value 
  if  Len(trim(s9))=0 Or s9="" then
  else
     s=s & " 移機工單號︰包含('" & s9 & "')字元 "
     t=t & " and (rtebtcustchgsndwork.prtno like '%" & s9 & "%') "
  end if   
  '-移機進度(只挑選異動項目為移機者==>chgcod2)
  s10ary=split(document.all("search10").value,";")  
  If Len(trim(s10ARY(0))) = 0 Then
  '已派工
  ELSEIF s10ARY(0) = "1" THEN
     s=s &"  移機進度:('" &s10aRY(1) & "') "
     t=t &" AND (RTEBTCUSTchg.chgcod2 > 0 ) AND (RTEBTCUSTchgsndwork.prtno is not null ) "    
  '未派工
  ELSEIF s10ARY(0) = "2" THEN
     s=s &"  移機進度:('" &s10aRY(1) & "') "
     t=t &" AND (RTEBTCUSTchg.chgcod2 > 0 ) AND (RTEBTCUSTchgsndwork.prtno is null ) "         
  end if
  '-用戶異動進度
  s11ary=split(document.all("search11").value,";")  
  If Len(trim(s11ARY(0))) = 0 Then
  '已結案
  ELSEIF s11ARY(0) = "1" THEN
     s=s &"  異動進度:('" &s11aRY(1) & "') "
     t=t &" AND (( RTEBTCUSTchg.chgcod2 > 0 and RTEBTCUSTchg.FINISHCHKDAT is not null  ) OR ( RTEBTCUSTchg.chgcod2 = 0 and RTEBTCUSTchg.TRANSCHKDAT is not null  ) ) "    
  '未結案
  ELSEIF s11ARY(0) = "2" THEN
     s=s &"  異動進度:('" &s11aRY(1) & "') "
     t=t &" AND (( RTEBTCUSTchg.chgcod2 > 0 and RTEBTCUSTchg.FINISHCHKDAT is null  ) OR ( RTEBTCUSTchg.chgcod2 = 0 and RTEBTCUSTchg.TRANSCHKDAT is null  ) ) "    
  '己作廢
  ELSEIF s11ARY(0) = "3" THEN
     s=s &"  異動進度:('" &s11aRY(1) & "') "
     t=t &" AND (RTEBTCUSTchg.dropdat is not null ) "             
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
  <tr class=dataListTitle align=center>AVS用戶資料搜尋條件</td><tr>
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
<tr><td class=dataListHead width="40%">用戶合約編號</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search5" class=dataListEntry> 
    </td></tr>  
<tr><td class=dataListHead width="40%">聯絡電話(或行動)</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="10" name="search6" class=dataListEntry> 
    </td></tr>   
<tr><td class=dataListHead width="40%">用戶身份證/統編</td>
    <td width="60%" bgcolor="silver">
      <input type="password" size="10" name="search7" class=dataListEntry> 
    </td></tr>          
<tr><td class=dataListHead width="40%">異動項目</td>
    <td width="60%" bgcolor="silver">
     <select name="search8" size="1" class=dataListEntry ID="Select1">
        <option value=";全部" selected>全部</option>
        <option value="1;變更用戶資料">變更用戶資料</option>
        <option value="2;換號">換號</option>
        <option value="3;移機">移機</option>   
        <option value="4;其它">其它</option>             
     </select>
    </td></tr>        
<tr><td class=dataListHead width="40%">移機工單號</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="15" name="search9" class=dataListEntry ID="Text2"> 
    </td></tr>       
<tr><td class=dataListHead width="40%">移機進度</td>
    <td width="60%" bgcolor="silver">
     <select name="search10" size="1" class=dataListEntry ID="Select2">
        <option value=";全部" selected>全部</option>
        <option value="1;已派工">已派工</option>
        <option value="2;未派工">未派工</option>
     </select>
    </td></tr>            
<tr><td class=dataListHead width="40%">用戶異動進度</td>
    <td width="60%" bgcolor="silver">
     <select name="search11" size="1" class=dataListEntry ID="Select3">
        <option value=";全部" selected>全部</option>
        <option value="1;已結案">已結案</option>
        <option value="2;未結案">未結案</option>
        <option value="3;已作廢">已作廢</option>   
     </select>
    </td></tr>            
</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>