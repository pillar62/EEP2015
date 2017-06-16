<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<meta http-equiv="Content-Type" content="text/html; charset=big5">
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"
		    codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 >
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<script language="VBScript">
<!--
Sub btn_onClick()
  '----社區名稱 -------------------------------------------------------------------
  r=document.all("search1").value  
  If Len(r) <>0 Or r <>"" Then
     s=s &"  社區名稱:" &r & " "
     t=t &"  and b.comn LIKE '%" &r &"%' " 
  End If
  '----社區名稱 -------------------------------------------------------------------
  r=document.all("search2").value  
  If Len(r) <>0 Or r <>"" Then
     s=s &"  支票抬頭:" &r & " "
     t=t &" and a.CHECKTITLE LIKE '%" &r &"%' " 
  End If
  '計算方式 --------------------------------------------------------------------
  s3=document.all("search3").value
  s3ary=split(s3,";")
  IF S3ARY(0) <> "" THEN
     s=s & "計算方式︰" & s3ary(1) & " "
     if s3ary(0) ="電錶" then
     	t=t & " and x.counttype in ('08','09','10') "
     elseif s3ary(0) ="非電錶" then
     	t=t & " and x.counttype not in ('08','09','10') "
     else
     	t=t & " and x.counttype='" & s3ary(0) & "' "
	 end if 
  END IF
  '----起算年月 -------------------------------------------------------------------
  r=document.all("search5").value  
  If Len(r) <>0 Or r <>"" Then
     s=s &"  起算年月:" &r & " "
     t=t &" and x.strym LIKE '%" &r &"%' " 
  End If
  '----截止年月 -------------------------------------------------------------------
  r=document.all("search6").value  
  If Len(r) <>0 Or r <>"" Then
     s=s &"  截止年月:" &r & " "
     t=t &" and x.endym LIKE '%" &r &"%' " 
  End If
  '方案別 --------------------------------------------------------------------
  s4=document.all("search4").value
  s4ary=split(s4,";")
  IF S4ARY(0) <> "" THEN
     s=s & "方案︰" & s4ary(1) & " "
     t=t & " and a.CASETYPE='" & s4ary(0) & "' "
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
  Dim winP
  Set winP=window.Opener
  winP.focus()
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
END SUB

Sub SrClear()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="C" & clickid
       clearkey="SEARCH" & clickid       
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
<body>
<table width="100%">
  <tr class=dataListTitle align=center>電費基本資料查詢搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search1" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead width="40%">支票抬頭</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search2" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead width="40%">補助起迄年月</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search5" class=dataListEntry> ～
      <input type="text" size="20" name="search6" class=dataListEntry>
    </td></tr>
<tr><td class=dataListHead width="40%">計算方式</td>
    <td width="60%" bgcolor="silver">    
  <%Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    DSN="DSN=RTLIB"
    conn.open DSN
    sql="SELECT code,CODENC FROM RTCODE WHERE KIND='R4' order by CODE "
    s="<option value="";"" >(全部)</option>" &_
      "<option value=""電錶;電錶"" >(電錶)</option>" &_
      "<option value=""非電錶;非電錶"" >(非電錶)</option>"
    rs.Open sql,conn
    'If rs.Eof Then s="<option value="";"" >(全部)</option>"
    sx=""
    Do While Not rs.Eof
       s=s &"<option value=""" &rs("CODE") & ";" & rs("CODENC") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close    
    conn.close       
    set rs=nothing
    set conn=nothing
	%>
       <select name="search3" size="1" class=dataListEntry>
        <%=s%>
      </select>
    </td>
</tr>
<tr><td class=dataListHead width="40%">方案</td>
    <td width="60%" bgcolor="silver">    
  <%Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    DSN="DSN=RTLIB"
    conn.open DSN
    sql="SELECT code,CODENC FROM RTCODE WHERE KIND='P5' order by CODE "
    s="<option value="";"" >(全部)</option>"   
    rs.Open sql,conn
    If rs.Eof Then s="<option value="";"" >(全部)</option>"
    sx=""
    Do While Not rs.Eof
       s=s &"<option value=""" &rs("CODE") & ";" & rs("CODENC") &"""" &sx &">" &rs("CODENC") &"</option>"
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
</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="submit" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>