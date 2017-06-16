
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV3/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim s,t
  t=""
  s=""
  s1=document.all("search1").value
  if len(trim(s1)) > 0 then
     s="社區名稱：含('" & s1 & "')字元"  
     t=t & " and hbadslcmty.comn like '%" & s1 & "%' "
  else
     s="社區名稱：全部  "
     t=t & " and hbadslcmty.comQ1 <> 0 "
  end if
  s2=document.all("search2").value
  s2ary=split(s2,";")
  if s2ary(1) <>"" then
     s=s & "  社區來源：" & s2ary(1) & " " 
     t=t & " and hbadslcmty.comsource ='" &  s2ary(1) & "' "
  else
     s=s & "  社區來源：全部  "
  end if  
  s3=document.all("search3").value
  if trim(s3) <>"" then
     s3ary=split(s3,";")
     s=s & "  線路類型：" & s3ary(1) & " "  
     t=t & " and HBADSLCMTY.COMTYPE='" & S3ary(0) & "' "
  else
     s=s & "  線路類型：全部  "
  end if  
  s4=document.all("search4").value
  s4ary=split(s4,";")  
  if s4ary(1) <>"" then
     s=s & "  業務組別：" & s4ary(1) 
     t=t & " and hbadslcmty.groupnc='" & s4ary(1) & "' "
  end if      
  s5=document.all("search5").value
  s6=document.all("search6").value
  if len(trim(s5))= 0 and len(trim(s6))=0 then
     S=s & "  線路開通日期︰全部  "
  else
     if len(trim(s5))=0 then s5="1900/01/01"
     if len(trim(s6))=0 then s6="9999/12/31"
     s=s & "  線路開通日期︰自" & s5 & " 至 " & S6 & " "
     t=t & " and hbadslcmty.t1applydat >='" & s5 & "' and t1applydat <='" & s6 & "' "
  end if
  s7=document.all("search7").value  
  s7ary=split(s7,";")
  if len(trim(s7)) > 0 then
     s=s & "  建置同意書︰" & s7ary(1)
     t=t & " and hbadslcmty.comagree" & S7ary(0) & "'' "
  end if    
  s8=document.all("search8").value  
  s8ary=split(s8,";")
  if len(trim(s8)) > 0 then
     s=s & "  合作契約書︰" & s8ary(1)
     t=t & " and hbadslcmty.contract" & S8ary(0) & "'' "
  end if    
  s9=document.all("search9").value
  s9ary=split(s9,";")
  s10=document.all("search10").value  
  if len(trim(s9)) > 0 then
     s=s & "  寬頻使用戶︰" & s9ary(1) & s10 & "戶"
     t=t & " and hbadslcmty.usercnt" & S9ary(0) & s10 & " "
  end if      
  s11=document.all("search11").value
  s11ary=split(s11,";")
  s12=document.all("search12").value  
  if len(trim(s11)) > 0 then
     s=s & "  社區總戶數︰" & s11ary(1) & s12 & "戶"
     t=t & " and hbadslcmty.comcnt" & S11ary(0) & s12 & " "
  end if            
  S13=document.all("search13").value
  IF LEN(TRIM(S13))=0 THEN
     xxx=""
  ELSE
     S13ARY=split(s13,";")
     xxx=" AND convert(varchar, HBADSLCMTY.comq1)+  HBADSLCMTY.comtype " _
     &" IN  (SELECT Convert(varchar,comq1)+kind FROM rtcmtymsg WHERE RTCmtyMSG.EVENTID = '" & s13ary(0) &"') "
     t=t & xxx
     s=s & "社區重大訊息事件︰" & s13ary(1)
  END IF
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
       width=60 VIEWASTEXT>
      <PARAM NAME="_ExtentX" VALUE="1270">
      <PARAM NAME="_ExtentY" VALUE="1270">
</OBJECT>
<body>
<center>
<table width="85%">
  <tr class=dataListTitle align=center>請輸入(選擇)社區資料搜尋條件</td><tr>
</table>
<table width="85%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search1" size="30" maxlength="200" class=dataListEntry>
    </td></tr>
<tr><td class=dataListHead width="40%">社區來源</td>
    <td width="60%" bgcolor="silver" >
 <% Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    DSN="DSN=RTLIB"
    conn.open DSN
    sql="SELECT CODE,codenc FROM RTcode where kind='B3' order by CODENC "
    s="<option value="";"" >(全部)</option>"   
    rs.Open sql,conn
    If rs.Eof Then s="<option value="";"" >(全部)</option>"
    sx=""
    Do While Not rs.Eof
       s=s &"<option value=""" &rs("CODE") & ";" & rs("CODEnc") &"""" &sx &">" &rs("CODEnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close    
    conn.close       
    set rs=nothing
    set conn=nothing
 %>
      <select name="search2" size="1" class=dataListEntry>
        <%=s%>
      </select>        
    </td></tr>
<tr><td class=dataListHead width="40%">線路類型</td>
    <td width="60%" bgcolor="silver" >
    <select class=dataListEntry name="search3">
       <option value="">(全部)</option>
       <option value="2;ADSL">ADSL</option>
       <option value="1;T1">T1</option>
    </select>        
    </td></tr>    
<tr><td class=dataListHead width="40%">重大訊息事件</td>
    <td width="60%" bgcolor="silver" >
 <% Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    DSN="DSN=RTLIB"
    conn.open DSN
    sql="SELECT CODE,codenc FROM RTcode where kind='C9' order by CODENC "
    s="<option value="""" >(全部)</option>"   
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(全部)</option>"
    sx=""
    Do While Not rs.Eof
       s=s &"<option value=""" &rs("CODE") & ";" & rs("CODEnc") &"""" &sx &">" &rs("CODEnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close    
    conn.close       
    set rs=nothing
    set conn=nothing
 %>    
    <select class=dataListEntry name="search13">
       <%=S%>
    </select>        
    </td></tr>        
<tr><td class=dataListHead width="40%">業務組別</td>
    <td width="60%" bgcolor="silver" >
 <% Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    DSN="DSN=RTLIB"
    conn.open DSN
    sql="SELECT GROUPID, GROUPNC FROM RTSALESGROUP where custyID='02' order by groupnc "
    s="<option value="";"" >(全部)</option>"   
    rs.Open sql,conn
    If rs.Eof Then s="<option value="";"" >(全部)</option>"
    sx=""
    Do While Not rs.Eof
       s=s &"<option value=""" &rs("GROUPNC") & ";" & rs("GROUPnc") &"""" &sx &">" &rs("GROUPnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close    
    conn.close       
    set rs=nothing
    set conn=nothing
 %>
      <select name="search4" size="1" class=dataListEntry>
        <%=s%>
      </select>    
    </td></tr>        
<tr><td class=dataListHead width="40%">開通日期</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search5" size="10" maxlength="10" class=dataListEntry>
             <input type="button" id="B5"  name="B5"   width="100%" style="Z-INDEX: 1"  value="..." ONCLICK="Srbtnonclick">        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C5"  name="C5"   style="Z-INDEX: 1"  ONCLICK="Srclear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >     
          迄
    <input type=text name="search6" size="10" maxlength="10" class=dataListEntry>
             <input type="button" id="B6"  name="B6"   width="100%" style="Z-INDEX: 1"  value="..." ONCLICK="Srbtnonclick" >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C6"  name="C6"   style="Z-INDEX: 1"  ONCLICK="Srclear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >     
    </td></tr>            
<tr><td class=dataListHead width="40%">建置同意書</td>
    <td width="60%" bgcolor="silver" >
    <select class=dataListEntry name="search7">
       <option value="">(全部)</option>
       <option value="<>;有">有</option>
       <option value="=;免">免</option>
    </select>
    </td></tr>            
<tr><td class=dataListHead width="40%">合約契約書</td>
    <td width="60%" bgcolor="silver" >
    <select class=dataListEntry name="search8">
       <option value="">(全部)</option>
       <option value="<>;有">有</option>
       <option value="=;無">無</option>
    </select>
    </td></tr>          
<tr><td class=dataListHead width="40%">寬頻使用戶</td>
    <td width="60%" bgcolor="silver" >
    <select class=dataListEntry name="search9">
       <option value="">(全部)</option>
       <option value=">;大於">大於</option>
       <option value="=;等於">等於</option>
       <option value="<;小於">小於</option>
    </select>
    <input type=text name="search10" size="5" maxlength="5" class=dataListEntry>戶
    </td></tr>                 
<tr><td class=dataListHead width="40%">社區總戶數</td>
    <td width="60%" bgcolor="silver" >
    <select class=dataListEntry name="search11">
       <option value="">(全部)</option>    
       <option value=">;大於">大於</option>
       <option value="=;等於">等於</option>
       <option value="<;小於">小於</option>
    </select>    
    <input type=text name="search12" size="5" maxlength="5" class=dataListEntry>戶
    </td></tr>                         
</table>
<table width="85%" align=right><tr><td></td><td align=right>
  <input type="submit" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</body>
</html>