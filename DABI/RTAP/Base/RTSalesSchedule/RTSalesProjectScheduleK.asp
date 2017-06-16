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
    ckind=request.form("search5")
 
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--

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
    Sub SrClear()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="C" & clickid
       clearkey="search" & clickid       
       if len(trim(document.all(clearkey).value)) <> 0 then
          document.all(clearkey).value =  ""
          document.all("name").value =  ""
          '當處理人員及處理廠商皆為空白時，才可清除此欄位資料
       end if
   End Sub    
Sub Srclose()  
 ' on error resume next
 ' Dim winP
 ' Set winP=window.Opener
 ' winP.focus()
  window.close  
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
<body background="/WEBAP/IMAGE/bg.gif">
<center><font size=3>業務行程資料搜尋條件</font></center>
<form method="post" id="form" target="main" action="RTSalesprojectschedulek2.asp" >
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
      <!--<select name="search1" size="1" class=dataListEntry ID="Select3" onchange="vbscript:window.form.submit">-->
      <select name="search1" size="1" class=dataListEntry ID="Select4" >
      <% =s1 %>
      </select>
      <input type="text" size="6" readonly name="search2" class=dataListdata ID="Text5" value="<%=cemply%>" > 
      <input type="BUTTON" id="B2"  <%=fieldpb%>  name="B2"  width="100%" style="Z-INDEX: 1"  value="...." onclick="Srsalesonclick()"  >  
      <input type="text" size="8" readonly name="name" class=dataListdata ID="Text1" value="<%=cname%>" > 
      <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C2"  name="C2"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="srclear()" >  
    </td>    
<td class=dataListHead width="7%" align=center>年月</td>
    <td width="15%" bgcolor="silver">
      <select name="search3" size="1" class=dataListEntry ID="Select2" >
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
            response.Write "<option value=""" & cstr(stryy) & """ " & xsel & ">" & stryy & "</option>"
        NEXT
        %>
      </select>
      <select name="search4" size="1" class=dataListEntry ID="Select1" >
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
            response.Write "<option value=""" & right("0" & cstr(strmm),2) & """ " & xsel & ">" & strmm & "月" & "</option>"
        NEXT
      %>
      </select>
    </td>
    <td width="5%" bgcolor="silver"><input type="submit" value="查詢" class=dataListButton name="btn" style="Z-INDEX:1;cursor:hand" ONSUBMIT="window.form2.submit">
     
    </td></tr>
<tr><td class=dataListHead width="7%" align=center >方案</td><TD bgcolor="silver" colspan=4>
<select name="search5" size="1" class=dataListEntry ID="Select5" >
<%
if ckind="" then
   xkind0=" selected "
elseif ckind="1" then
   xkind1=" selected "
elseif ckind="2" then
   xkind2=" selected "
elseif ckind="3" then
   xkind3=" selected "
elseif ckind="5" then
   xkind5=" selected "
end if
%>
<option value="" <%=xkind0%>>全部方案</option>
<option value="1" <%=xkind1%>>HB599</option>
<option value="2" <%=xkind2%>>中華399</option>
<option value="3" <%=xkind3%>>速博399</option>
<option value="5" <%=xkind5%>>東森499</option>
</select>
</TD>
<!--<td width="5%" bgcolor="silver"><input type="button" value="結束" class=dataListButton name="btn1" style="Z-INDEX:1;cursor:hand"  ID="Button1" onclick="srclose()">   
    </td> -->
</tr>
</table>
</form>
</body>
</html>
<%
    Set rs=Nothing
    Set conn=Nothing
%>
