<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim aryStr,s,t,r
  '---會員帳號
  S1=document.all("search1").value  
  If Len(s1)=0 Or s1="" Then
     t=t &" (STMember.memberid <> '*' )" 
  Else
     s=s &"  會員帳號:包含('" &S1 & "'字元)"
     t=t &" (STMember.memberid LIKE '%" &S1 &"%')" 
  End If
  '---姓名
  S2=document.all("search2").value  
  If Len(s2)<>0 Or s2<>"" Then
     s=s &"  姓名:包含('" &S2 & "'字元)"
     t=t &" and (STMember.cusnc LIKE '%" &S2 &"%')" 
  End If  
  '----EMAIL
  s3=document.all("search3").value
  If S3<>""  Then
     s=s &"  EMAIL:包含('" &s3 & "') "
     t=t &" AND (STMember.EMAIL LIKE '%" & S3 & "%') "
  End If           
  '----tel,mobile,fax
  s4=document.all("search4").value
  If S4<>""  Then
     s=s &"  電話/手機/傳真:包含('" &s4 & "') "
     t=t &" AND (STMember.tel + STMember.mobile + STMember.fax  LIKE '%" & S4 & "%') "
  End If         
  '----地址
  s5=document.all("search5").value
  If S5<>""  Then
     s=s &"  地址:包含('" &s5 & "') "
     t=t &" AND (STCounty.CUTNC + STMember.TOWNSHIP + STMember.RADDR LIKE '%" & S5 & "%') "
  End If      
  '----使用狀態
  s6=document.all("search6").value
  s6ary=split(s6,";")
  If S6ary(0)<>""  Then
     s=s &"  使用狀態:('" &s6ary(1) & "') "
     '已登入
     if s6ary(0)="1" then
        t=t &" AND (STMember.firstlogin is not null ) and STMember.closedat is null "
     '未登入
     elseif s6ary(0)="2" then
        t=t &" AND (STMember.firstlogin is null ) and STMember.closedat is null "
     '已失效
     elseif s6ary(0)="3" then
        t=t &" AND  STMember.closedat is not null "
     end if
  End If      
  '----會員申請日
  s7=document.all("search7").value
  s8=document.all("search8").value
  if s7<>"" or s8<>"" then
     if s7="" then 
        s7="1911/01/01 00:00:00"
     else
        s7=s7 & " 00:00:00 "
     end if
     if s8="" then
        s8="9999/12/31 23:59:59"
     else
        s8=s8 & " 23:59:59 "
     end if
     s=s &"  會員申請日:('" &s7 & " - " & s8 &"')"
     t=t & " and STMember.memberapplydat >='" & s7 & "' and STMember.memberapplydat <='" & s8 & "' "
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
  Dim winP
  Set winP=window.Opener
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
   Sub SrClear()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="C" & clickid
       clearkey="search" & clickid       
       if len(trim(document.all(clearkey).value)) <> 0 then
          document.all(clearkey).value =  ""
          '當處理人員及處理廠商皆為空白時，才可清除此欄位資料
       end if
   End Sub    
   Sub ImageIconOver()
       self.event.srcElement.style.borderBottom = "black 1px solid"
       self.event.srcElement.style.borderLeft="white 1px solid"
       self.event.srcElement.style.borderRight="black 1px solid"
       self.event.srcElement.style.borderTop="white 1px solid"   
   End Sub
   
   Sub ImageIconOut()
       self.event.srcElement.style.borderBottom = ""
       self.event.srcElement.style.borderLeft=""
       self.event.srcElement.style.borderRight=""
       self.event.srcElement.style.borderTop=""
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
  <tr class=dataListTitle align=center>網站會員資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">會員帳號</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search1" class=dataListEntry ID="Text5"> 
    </td></tr>        
<tr><td class=dataListHead width="40%">姓名</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search2" class=dataListEntry ID="Text2"> 
    </td></tr>            
<tr><td class=dataListHead width="40%">EMAIL</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="30" name="search3" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead width="40%">電話(手機/傳真)</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="30" name="search4" class=dataListEntry ID="Text3"> 
    </td></tr>    
<tr><td class=dataListHead width="40%">地址</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="40" name="search5" class=dataListEntry ID="Text4"> 
    </td></tr>    
<tr><td class=dataListHead width="40%">使用狀態</td>
    <td width="60%" bgcolor="silver">
      <select name="search6" size="1" class=dataListEntry ID="Select1">
        <option value=";全部" selected>全部</option>
        <option value="1;已登入">已登入</option>
        <option value="2;未登入">未登入</option>
        <option value="3;已失效">已失效</option>
      </select>
    </td></tr>        
<tr><td class=dataListHead width="40%">會員申請日</td>
    <td width="60%" bgcolor="silver">
<font size=2>自</font>
      <input type="text" size="10" readonly name="search7" class=dataListEntry ID="Text1"> 
<input type="button" id="B7"  name="B7" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">  
<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C7"  name="C7"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">    
      <font size=2>至</font>
      <input type="text" size="10" readonly name="search8" class=dataListEntry ID="Text6"> 
<input type="button" id="B8"  name="B8" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="c8"  name="C8"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
    </td></tr>            
</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>