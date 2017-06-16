<%
srt1=""
srt1=srt1 &"<option value=""""></option>"
srt1=srt1 &"<option value=""RTLessorCMTYH.comn"">社區名稱</option>"
srt1=srt1 &"<option value=""RTLessorCust.cusnc"">用戶名稱</option>"
srt1=srt1 &"<option value=""addr"">用戶地址</option>"
srt1=srt1 &"<option value=""RTLessorCust.duedat"">到期日</option>"
srt1=srt1 &"<option value=""validdat"">可用日數</option>"
srt2=""
srt2=srt2 &"<option value=""""></option>"
srt2=srt2 &"<option value=""RTLessorCMTYH.comn"">社區名稱</option>"
srt2=srt2 &"<option value=""RTLessorCust.cusnc"">用戶名稱</option>"
srt2=srt2 &"<option value=""addr"">用戶地址</option>"
srt2=srt2 &"<option value=""RTLessorCust.duedat"">到期日</option>"
srt2=srt2 &"<option value=""validdat"">可用日數</option>"
srt3=""
srt3=srt3 &"<option value=""""></option>"
srt3=srt3 &"<option value=""RTLessorCMTYH.comn"">社區名稱</option>"
srt3=srt3 &"<option value=""RTLessorCust.cusnc"">用戶名稱</option>"
srt3=srt3 &"<option value=""addr"">用戶地址</option>"
srt3=srt3 &"<option value=""RTLessorCust.duedat"">到期日</option>"
srt3=srt3 &"<option value=""validdat"">可用日數</option>"
srt4=""
srt4=srt4 &"<option value=""""></option>"
srt4=srt4 &"<option value=""RTLessorCMTYH.comn"">社區名稱</option>"
srt4=srt4 &"<option value=""RTLessorCust.cusnc"">用戶名稱</option>"
srt4=srt4 &"<option value=""addr"">用戶地址</option>"
srt4=srt4 &"<option value=""RTLessorCust.duedat"">到期日</option>"
srt4=srt4 &"<option value=""validdat"">可用日數</option>"
srt5=""
srt5=srt5 &"<option value=""""></option>"
srt5=srt5 &"<option value=""RTLessorCMTYH.comn"">社區名稱</option>"
srt5=srt5 &"<option value=""RTLessorCust.cusnc"">用戶名稱</option>"
srt5=srt5 &"<option value=""addr"">用戶地址</option>"
srt5=srt5 &"<option value=""RTLessorCust.duedat"">到期日</option>"
srt5=srt5 &"<option value=""validdat"">可用日數</option>"
srt99=""
srt99=srt99 &"<option value=""A"">由小到大排序</option>"
srt99=srt99 &"<option value=""D"">由大到小排序</option>"
%>
<html>
<head>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim aryStr,s,t,r
  '----社區名稱
  S1=document.all("search1").value  
  If Len(s1)=0 Or s1="" Then
     t=t &" (RTLessorCmtyH.ComN<> '*' )" 
  Else
     s=s &"  社區名稱:包含('" &S1 & "'字元)"
     t=t &" (RTLessorCmtyH.ComN LIKE '%" &S1 &"%')" 
  End If
  '----主線IP
  s3=document.all("search3").value
  IF LEN(TRIM(S3)) > 0 THEN
     s=s &"  主線IP:包含('" &s3 & "'字元) "
     t=t &" AND (RTLessorCmtyLINE.LINEIP LIKE '%" & S3 & "%') "
  end if
  '----主線附掛電話
  s5=document.all("search5").value
  If Len(trim(s5)) > 0 Then
     s=s &"  附掛電話:包含('" &s5 & "'字元) "
     t=t &" AND (RTLessorCmtyLINE.LINETEL LIKE '%" & S5 & "%') "
  End If      
  '用戶名稱
  s9=document.all("search9").value 
  if  Len(trim(s9))=0 Or s9="" then
  else
     s=s & " 用戶名稱︰包含('" & s9 & "')字元 "
     t=t & " and (RTLessorCust.cusnc like '%" & s9 & "%') "
  end if
  '----繳款週期
  s10ary=split(document.all("search10").value,";")  
  If Len(trim(s10ary(0)))=0 Or s10ary(0)="" Then
  Else
     s=s &"  繳款週期:" &s10ary(1)
     t=t &" AND (RTLessorCust.paycycle='" & s10ary(0) & "') "
  End If   
  '----繳款方式
  s11ary=split(document.all("search11").value,";")  
  If Len(trim(s11ary(0)))=0 Or s11ary(0)="" Then
  Else
     s=s &"  繳款方式:" &s11ary(1)
     t=t &" AND (RTLessorCust.paytype='" & s11ary(0) & "') "
  End If 
  '聯絡電話
  s12=document.all("search12").value 
  if  Len(trim(s12))=0 Or s12="" then
  else
     s=s & " 聯絡電話︰包含('" & s12 & "')字元 "
     t=t & " and ((RTLessorCust.contacttel like '%" & s12 & "%') OR (RTLessorCust.mobile like '%" & s12 & "%')) "
  end if   
  '身分證/統編
  s13=document.all("search13").value 
  if  Len(trim(s13))=0 Or s13="" then
  else
     s=s & " 身份證/統編︰包含('" & s13 & "')字元 "
     t=t & " and (RTLessorCust.socialid like '%" & s13 & "%') "
  end if     
  '到期日起迄
  s14=document.all("search14").value 
  s15=document.all("search15").value   
  if  (Len(trim(s14))=0 Or s14="") and (Len(trim(s15))=0 Or s15="") then
  else
     if  (Len(trim(s14))=0 Or s14="") then s14="1900/01/01"
     if  (Len(trim(s15))=0 Or s15="") then s15="9999/12/31"
     s=s & " 用戶到期日起迄︰自('" & s14 & "') 至 ('" & s15 & "') "
     t=t & " and RTLessorCust.duedat >= '" & s14 & " 00:00.000' and RTLessorCust.duedat  <= '" & s15 & " 23:59.997' "
  end if               
  '----社區序號
  s16=document.all("search16").value
  If Len(trim(s16)) > 0 Then
     s=s &"  社區序號:('" &s16 & "') "
     t=t &" AND (RTLessorCust.COMQ1=" & S16 & ") "
  End If   
  '----主線序號
  s17=document.all("search17").value
  If Len(trim(s17)) > 0 Then
     s=s &"  主線序號:('" &s17 & "') "
     t=t &" AND (RTLessorCust.LINEQ1=" & S17 & ") "
  End If            
  '----用戶進度狀況
  s18ary=split(document.all("search18").value,";")  
  If Len(trim(s18ARY(0))) = 0 Then
 '未完工
  ELSEIF s18ARY(0) = "1" THEN
     s=s &"  用戶進度:('" &s18ARY(1) & "') "
     t=t &" AND (RTLessorCust.FINISHDAT IS NULL and RTLessorCust.dropdat is null and RTLessorCust.canceldat is null) "      
  '已完工無計費日
  ELSEIF s18ARY(0) = "2" THEN
     s=s &"  用戶進度:('" &s18ARY(1) & "') "
     t=t &" AND RTLessorCust.FINISHDAT IS NOT NULL AND  RTLessorCust.strbillingdat IS NULL "               
  '已退租
  ELSEIF s18ARY(0) = "3" THEN
     s=s &"  用戶進度:('" &s18ARY(1) & "') "
     t=t &" AND  RTLessorCust.dropdat IS NOT NULL AND RTLessorCust.strbillingdat IS not NULL "       
  '已作廢
  ELSEIF s18ARY(0) = "4" THEN
     s=s &"  用戶進度:('" &s18ARY(1) & "') "
     t=t &" AND  RTLessorCust.CANCELDAT IS NOT NULL AND RTLessorCust.finishdat IS NULL "                 
  End If              
  '排序
  SRT1 =document.all("srt1X").value
  SRT2 =document.all("srt2X").value
  SRT3 =document.all("srt3X").value
  SRT4 =document.all("srt4X").value
  SRT5 =document.all("srt5X").value
  SRT99=document.all("srt99X").value
  srtx=""
  if Len(trim(srt1))> 0 then
     srtx=srtx & " order by " & srt1
  ELSE
     srtx=srtx & " order by RTLESSORCUST.COMQ1 "
  end if
  if Len(trim(srt2))> 0 then
     srtx=srtx & "," & srt2
  end if
  if Len(trim(srt3))> 0 then
     srtx=srtx & "," & srt3
  end if
  if Len(trim(srt4))> 0 then
     srtx=srtx & "," & srt4
  end if
  if Len(trim(srt5))> 0 then
     srtx=srtx & "," & srt5
  end if
  if srt99="D" THEN
     SRTX=SRTX & " DESC "
  END IF

  Dim winP,docP
  Set winP=window.Opener
  Set docP=winP.document
  docP.all("searchQry").value=t
  docP.all("searchQry2").value=SRTX
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
  <tr class=dataListTitle align=center>ET-City用戶資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="25%">社區/主線序號</td>
    <td width="75%" bgcolor="silver">
      <input type="text" size="5" name="search16" class=dataListEntry ID="Text5"> 
      <font size=2>-</font>
      <input type="text" size="5" name="search17" class=dataListEntry ID="Text6"> 
    </td></tr>  
<tr><td class=dataListHead >用戶名稱</td>
    <td  bgcolor="silver">
      <input type="text" size="20" name="search9" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead >用戶進度狀況</td>
    <td  bgcolor="silver">
      <select name="search18" size="1" class=dataListEntry ID="Select1">
        <option value=";全部" selected>全部</option>
        <option value="1;未完工">未完工</option>
        <option value="2;已完工無計費日">已完工無計費日</option>
        <option value="3;已退租">已退租</option>      
        <option value="4;已作廢">已作廢</option>                     
      </select>
     </td>
</tr>    
<tr><td class=dataListHead >繳款週期</td>
    <td  bgcolor="silver">
     <select name="search10" size="1" class=dataListEntry>
        <option value=";全部" selected>全部</option>
        <option value="05;季繳">季繳</option>
        <option value="01;半年繳">半年繳</option>
        <option value="02;一年繳">一年繳</option>     
        <option value="03;兩年繳">兩年繳</option> 
        <option value="04;三年繳">三年繳</option>            
     </select>
    </td></tr>        
<tr><td class=dataListHead >繳款方式</td>
    <td  bgcolor="silver">
     <select name="search11" size="1" class=dataListEntry>
        <option value=";全部" selected>全部</option>
        <option value="02;現金">現金</option>
        <option value="01;信用卡">信用卡</option>
        <option value="03;ATM轉帳">ATM轉帳</option>                
     </select>
    </td></tr>            
<tr><td class=dataListHead >聯絡電話(或行動)</td>
    <td bgcolor="silver">
      <input type="text" size="10" name="search12" class=dataListEntry> 
    </td></tr>   
<tr><td class=dataListHead >用戶身份證/統編</td>
    <td  bgcolor="silver">
      <input type="text" size="10" name="search13" class=dataListEntry> 
    </td></tr>          
<tr><td class=dataListHead >用戶到期日</td>
    <td  bgcolor="silver"><font size=2>自</font>
      <input type="text" size="10" name="search14" class=dataListEntry> 
<input type="button" id="B14"  name="B14" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">      
      <font size=2>至</font>
      <input type="text" size="10" name="search15" class=dataListEntry> 
<input type="button" id="B15"  name="B15" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">            
    </td></tr>        
<tr><td class=dataListHead >社區名稱</td>
    <td  bgcolor="silver">
      <input type="text" size="20" name="search1" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead >主線IP</td>
    <td  bgcolor="silver">
      <input type="text" size="20" name="search3" class=dataListEntry ID="Text1"> 
    </td></tr>    
<tr><td class=dataListHead >主線附掛電話</td>
    <td bgcolor="silver">
      <input type="text" size="20" name="search5" class=dataListEntry ID="Text2"> 
    </td></tr>        
<tr><td class=dataListHead >資料排序</td>
    <td  bgcolor="silver">
    <select name="srt1X" size="1" class=dataListEntry>
        <%=srt1%>
    </select>
    <select name="srt2X" size="1" class=dataListEntry>
        <%=srt2%>
    </select>
    <select name="srt3X" size="1" class=dataListEntry>
        <%=srt3%>
    </select>
    <select name="srt4X" size="1" class=dataListEntry>
        <%=srt4%>
    </select>
    <select name="srt5X" size="1" class=dataListEntry>
        <%=srt5%>
    </select>
    <select name="srt99X" size="1" class=dataListEntry>
        <%=srt99%>
    </select>
    </td></tr>           
</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>