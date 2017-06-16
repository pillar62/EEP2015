<%  Set conn=Server.CreateObject("ADODB.Connection")
    Set RS=Server.CreateObject("ADODB.Recordset")
    conn.open "DSN=RTLib"
    csalesarea=request.form("search1")
    cemply=request.form("search2")
    cname=request.form("name")
    cyy=request.form("search3")
    cmm=request.form("search4")
    if cyy="" then cyy=DATEPART("yyyy",now())
    if cmm="" then cmm=DATEPART("m",now())
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
'
Sub btn1_onClick()  
  Dim winP
  Set winP=window.Opener
  winP.focus()
  window.close  
End Sub
Sub Srsalesonclick()
       prog="RTGetsalesD.asp"
       prog=prog & "?KEY=" & document.all("search1").VALUE 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("name").value=mid(Fusrid(1),7,6)
          document.all("search2").value =  trim(Fusrid(0))
       End if       
       end if
 End Sub      
-->
</script>
</head>
<body>
<center><font size=3>業務行程資料搜尋條件</font></center>
<form method="post" id="form">
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="7%" align=center>業務員</td>
    <td width="30%" bgcolor="silver">
    <%
    s1=""
    sql="SELECT AREAID, GROUPID, GROUPNC FROM RTSalesGroup WHERE (EDATE IS NULL) "
    rs.Open sql,conn
    Do While Not rs.Eof
       if csalesarea=rs("areaid") & ";" & rs("groupid") then
          xsel=" selected "
       else
          xsel=""
       end if
       s1=s1 &"<option value=""" &rs("areaid") & ";" & rs("groupid") & """" & xsel & ">" &rs("groupNC") &"</option>"
       rs.MoveNext
    Loop
    %>
      <select name="search1" size="1" class=dataListEntry ID="Select3" onchange="vbscript:window.form.submit">
      <% =s1 %>
      </select>
      <input type="text" size="6" readonly name="search2" class=dataListdata ID="Text5" value="<%=cemply%>" onchange="vbscript:window.form.submit"> 
      <input type="BUTTON" id="B2"  <%=fieldpb%>  name="B2"  width="100%" style="Z-INDEX: 1"  value="...." onclick="Srsalesonclick()"  >  
      <input type="text" size="8" readonly name="name" class=dataListdata ID="Text1" value="<%=cname%>"> 
    </td>    
<td class=dataListHead width="7%" align=center>年月</td>
    <td width="15%" bgcolor="silver">
      <select name="search3" size="1" class=dataListEntry ID="Select2" onchange="vbscript:window.form.submit">
        <%
        NOWYY=DATEPART("yyyy",now())
        stryy=cint(nowyy)-10
        For I=0 to 20
            stryy=stryy+1
            if stryy=cint(cyy) then
               xsel=" selected "
            else
               xsel=""
            end if
            response.Write "<option value=" & stryy & " " & xsel & ">" & stryy & "</option>"
        NEXT
        %>
      </select>
      <select name="search4" size="1" class=dataListEntry ID="Select1" onchange="vbscript:window.form.submit">
      <%
      NOWMM=DATEPART("m",now())
      strmm=0
      For I=1 to 12
            strmm=strmm+1
            if strmm=cint(cmm) then
               xsel=" selected "
            else
               xsel=""
            end if
            response.Write "<option value=" & strmm & " " & xsel & ">" & strmm & "月" & "</option>"
        NEXT
      %>
      </select>
    </td>
    <td width="5%" bgcolor="silver"><input type="submit" value="查詢" class=dataListButton name="btn" style="Z-INDEX:1;cursor:hand">
       <!-- <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand"> -->
    </td></tr></table>
    <table width="100%" border=5 cellPadding=0 cellSpacing=0 id=tab1>
    <%
    '抓取當月第一天星期
   ' cyy=request.form("search3")
  '  cmm=request.form("search4")
  '  response.Write "k1=" & cyy & ";k2=" & cmm
    if cyy="" then
       cyy=nowyy
    end if
    if cmm="" then
       cmm=nowmm
    end if
    firstdat=cyy & "/" & cmm & "/" & "01"
    firstweek=weekday(firstdat)
   ' response.Write "week=" & firstweek & ";"
    '抓取當月日數
    nextmonth=datepart("yyyy",firstdat) & "/" & datepart("m",dateadd("m",1,firstdat)) & "/" & "01"
    nextmonth=dateadd("d",-1,nextmonth)
    maxmonthday=datepart("d",nextmonth)
   ' response.Write "maxday=" & maxmonthday
    RESPONSE.Write "<TR>"
    str="日一二三四五六"
    FOR I=1 TO 7
       RESPONSE.Write "<TD height=40 align=center bgcolor=""darkkhaki"">" & mid(str,i,1) & "</TD>"
    NEXT
    RESPONSE.Write "</TR>"
    xcnt=0
    if ( maxmonthday >=30 and firstweek=7 ) or ( maxmonthday >=31 and firstweek=6 ) then
       k=5
    elseif ( maxmonthday <= 28 and firstweek=1 ) then
       k=3
    else
       k=4
    end if
    FOR I=0 TO k
        RESPONSE.Write "<TR>"
        FOR J=0 TO 6
            cnt=cnt+1
            if cnt>=firstweek  then
               xcnt=xcnt+1
               xxx=cstr(xcnt)
            end if
            if j=0 then
               xbgcolor=" bgcolor=""cornflowerblue"" "
            elseif j=6 then
               xbgcolor=" bgcolor=""honeydew"" "
            else
               xbgcolor=" bgcolor=""aliceblue"" "
            end if
            if xcnt=0 or xcnt > maxmonthday then
               xxx="&nbsp;"
            end if
            RESPONSE.Write "<TD " & xbgcolor & " height=80 ID=""" & I&J &""" style=""text-align:center;vertical-align:top;color:#DC143C;font-size:10.0pt"">" & xxx & "</TD>"
        NEXT
        RESPONSE.Write "</TR>"
    NEXT
    %>
    </table>
</form>
</body>
</html>
<%
    Set rs=Nothing
    Set conn=Nothing
%>