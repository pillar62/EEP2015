
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4EBT/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  
  dim s,t
  t="":t2="":t3="":t4="":t5="":t6="":t7=""
  s=""
  'searchqry
  s1=document.all("search1").value
  s1ary=split(s1,";")  
  if S1="" then
     s="方案種類：全部,  "
     t=t & " CASEID <> '' "
  ELSE
     s="方案種類：" & S1ARY(1) & ","
     t=t & " CASEID ='" & S1ARY(0) & "' "
  end if
 'searchqry2
  s2=document.all("search2").value
  s2ary=split(s2,";")
  S3=document.all("search3").value
  if s2 <>"" then
     s=s & "  HB(固定制ADSL線路)報竣戶數：" & s2ary(1) & " " & S3 & " 戶 "
     t=t & " and  (( CASEID = 'CHT599ADSL' AND CUSTCNT " & S2ARY(0) & S3 & ")"
     t2=s2ary(0) & ";" & s3
  else
     t=t & " and  (( CASEID = 'CHT599ADSL' ) "
     t2=";"
  end if  
  'searchqry3
  s4=document.all("search4").value
  s4ary=split(s4,";")
  S5=document.all("search5").value
  if s4 <>"" then
     s=s & "  HB(固定制T1線)報竣戶數：" & s4ary(1) & " " & S5 & " 戶 "
     t=t & " OR  ( CASEID = 'CHT599T1A' AND CUSTCNT " & S4ARY(0) & S5 & ")"
     t3=s4ary(0) & ";" & s5
  else
     t=t & " OR  ( CASEID = 'CHT599T1A' ) "
     t3=";"
  end if  
  'searchqry4
  s6=document.all("search6").value
  s6ary=split(s6,";")
  S7=document.all("search7").value
  if s6 <>"" then
     s=s & "  HB(計量制T1線路)報竣戶數：" & s6ary(1) & " " & S7 & " 戶 "
     t=t & " OR  ( CASEID = 'CHT599T1B' AND CUSTCNT " & S6ARY(0) & S7 & ")"
     t4=s6ary(0) & ";" & s7
  else
     t=t & " OR  ( CASEID = 'CHT599T1B' ) "
     t4=";"
  end if    
  'searchqry5
  s8=document.all("search8").value
  s8ary=split(s8,";")
  S9=document.all("search9").value
  if s8 <>"" then
     s=s & "  中華ADSL399報竣戶數：" & s8ary(1) & " " & S9 & " 戶 "
     t=t & " OR  ( CASEID = 'CHT399' AND CUSTCNT " & S8ARY(0) & S9 & ")"
     t5=s8ary(0) & ";" & s9
  else
     t=t & " OR  ( CASEID = 'CHT399' ) "
     t5=";"
  end if      
  'searchqry6
  s10=document.all("search10").value
  s10ary=split(s10,";")
  S11=document.all("search11").value
  if s10 <>"" then
     s=s & "  東森AVS499報竣戶數：" & s10ary(1) & " " & S11 & " 戶 "
     t=t & " OR  ( CASEID = 'AVS499' AND CUSTCNT " & S10ARY(0) & S11 & ")"
     t6=s10ary(0) & ";" & s11
  else
     t=t & " OR  ( CASEID = 'AVS499' ) "
     t6=";"
  end if        
  'searchqry7
  s12=document.all("search12").value
  s12ary=split(s12,";")
  S13=document.all("search13").value
  if s12 <>"" then
     s=s & "  速博ADSL399報竣戶數：" & s12ary(1) & " " & S13 & " 戶 "
     t=t & " OR  ( CASEID = 'NCIC399' AND CUSTCNT " & S12ARY(0) & S13 & ") )"
     t7=s12ary(0) & ";" & s13
  else
     t=t & " OR  ( CASEID = 'NCIC399' ) )"
     t7=";"
  end if          
  
  Dim winP,docP
  Set winP=window.Opener
  Set docP=winP.document
  docP.all("searchQry").value=t
  docP.all("searchQry2").value=t2
  docP.all("searchQry3").value=t3
  docP.all("searchQry4").value=t4
  docP.all("searchQry5").value=t5
  docP.all("searchQry6").value=t6
  docP.all("searchQry7").value=t7
  docP.all("searchShow").value=s
  docP.all("keyform").Submit
  winP.focus()
  window.close
End Sub
Sub btn1_onClick()
  window.close
End Sub

Sub Srbtnonclick()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="SEARCH" & clickid
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
    clearkey="SEARCH" & clickid       
    if len(trim(document.all(clearkey).value)) <> 0 then
       document.all(clearkey).value =  ""
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
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"     codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
       height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
       width=60 >
      <PARAM NAME="_ExtentX" VALUE="1270">
      <PARAM NAME="_ExtentY" VALUE="1270">
</OBJECT>
<body>
<center>
<table width="85%">
  <tr class=dataListTitle align=center>請輸入(選擇)資料搜尋條件</td><tr>
</table>
<table width="85%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">方案</td>
    <td width="60%" bgcolor="silver" >
    <select class=dataListEntry name="search1" >
       <option value="">(全部)</option>
       <option value="CHT599ADSL;HB(固定制ADSL線路)">HB(固定制ADSL線路)</option>
       <option value="CHT599T1A;HB(固定制T1線)">HB(固定制T1線)</option>
       <option value="CHT599T1B;HB(計量制T1線路)">HB(計量制T1線路)</option>
       <option value="CHT399;中華ADSL399">中華ADSL399</option>
       <option value="AVS499;東森AVS499">東森AVS499</option>
       <option value="NCIC399;速博ADSL399">速博ADSL399</option>
    </select>        
    </td></tr>
<tr><td class=dataListHead width="40%">HB(固定制ADSL線路)報竣戶數</td>
    <td width="60%" bgcolor="silver" >
    <select class=dataListEntry name="search2">
       <option value="">(全部)</option>
       <option value=">;大於">大於</option>
       <option value="=;等於">等於</option>
       <option value="<;小於">小於</option>
    </select>
    <input type=text name="search3" size="5" maxlength="5" class=dataListEntry>戶
    </td></tr>                 
<tr><td class=dataListHead width="40%">HB(固定制T1線)報竣戶數</td>
    <td width="60%" bgcolor="silver" >
    <select class=dataListEntry name="search4" ID="Select1">
       <option value="">(全部)</option>
       <option value=">;大於">大於</option>
       <option value="=;等於">等於</option>
       <option value="<;小於">小於</option>
    </select>
    <input type=text name="search5" size="5" maxlength="5" class=dataListEntry ID="Text1">戶
    </td></tr>                     
<tr><td class=dataListHead width="40%">HB(計量制T1線路)報竣戶數</td>
    <td width="60%" bgcolor="silver" >
    <select class=dataListEntry name="search6" ID="Select2">
       <option value="">(全部)</option>
       <option value=">;大於">大於</option>
       <option value="=;等於">等於</option>
       <option value="<;小於">小於</option>
    </select>
    <input type=text name="search7" size="5" maxlength="5" class=dataListEntry ID="Text2">戶
    </td></tr>               
<tr><td class=dataListHead width="40%">中華ADSL399報竣戶數</td>
    <td width="60%" bgcolor="silver" >
    <select class=dataListEntry name="search8" ID="Select3">
       <option value="">(全部)</option>
       <option value=">;大於">大於</option>
       <option value="=;等於">等於</option>
       <option value="<;小於">小於</option>
    </select>
    <input type=text name="search9" size="5" maxlength="5" class=dataListEntry ID="Text3">戶
    </td></tr>               
<tr><td class=dataListHead width="40%">東森AVS499報竣戶數</td>
    <td width="60%" bgcolor="silver" >
    <select class=dataListEntry name="search10" ID="Select4">
       <option value="">(全部)</option>
       <option value=">;大於">大於</option>
       <option value="=;等於">等於</option>
       <option value="<;小於">小於</option>
    </select>
    <input type=text name="search11" size="5" maxlength="5" class=dataListEntry ID="Text4">戶
    </td></tr>                     
<tr><td class=dataListHead width="40%">速博ADSL399報竣戶數</td>
    <td width="60%" bgcolor="silver" >
    <select class=dataListEntry name="search12" ID="Select5">
       <option value="">(全部)</option>
       <option value=">;大於">大於</option>
       <option value="=;等於">等於</option>
       <option value="<;小於">小於</option>
    </select>
    <input type=text name="search13" size="5" maxlength="5" class=dataListEntry ID="Text5">戶
    </td></tr>                      
</table>
<table width="85%" align=right><tr><td></td><td align=right>
  <input type="submit" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</body>
</html>