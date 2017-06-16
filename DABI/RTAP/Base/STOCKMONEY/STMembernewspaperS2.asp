<%
    Dim rs,i,conn
    Dim search1Opt,search2Opt,search6Opt, search12pt
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=STOCK;uid=sa;pwd=alittlecat@cbn"
    
    Set rs=Server.CreateObject("ADODB.Recordset")
'----------會員類別
    S4=""
    rs.Open "SELECT CODE,CODENC FROM STCODE WHERE KIND='A4'",CONN
    s4="<option value="";全部"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s4=s4 &"<option value=""" &rs("CODE") & ";" & rs("CODENC") & """>" &rs("CODENC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'----------電子報種類
    S5=""
    rs.Open "SELECT CODE,CODENC FROM STCODE WHERE KIND='A2'",CONN
    s5="<option value="";全部"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s5=s5 &"<option value=""" &rs("CODE") & ";" & rs("CODENC") & """>" &rs("CODENC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
        
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>

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
     t=t &" (STMemberNewsPaper.memberid <> '*' )" 
  Else
     s=s &"  會員帳號:包含('" &S1 & "'字元)"
     t=t &" (STMemberNewsPaper.memberid LIKE '%" &S1 &"%')" 
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
  '----會員類別
  s4=document.all("search4").value
  s4ary=split(s4,";")
  If S4ary(0)<>""  Then
     s=s &"  會員類別:" &s4ary(1) & " "
     t=t &" AND (STRegMember.yormort = '" & S4ary(0) & "') "
  End If        
  '----電子報種類
  s5=document.all("search5").value
  s5ary=split(s5,";")
  If S5ary(0)<>""  Then
     s=s &"  電子報種類:" &s5ary(1) & " "
     t=t &" AND (STMemberNewsPaper.NEWSPAPERCODE = '" & S5ary(0) & "') "
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
<tr><td class=dataListHead width="40%">會員類別</td>
    <td width="60%" bgcolor="silver">
      <select name="search4" size="1" class=dataListEntry ID="Select1">
      <%=S4%>
      </select>
    </td></tr>    
<tr><td class=dataListHead width="40%">電子報種類</td>
    <td width="60%" bgcolor="silver">
      <select name="search5" size="1" class=dataListEntry ID="Select2">
      <%=S5%>
      </select>
    </td></tr>    

</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>