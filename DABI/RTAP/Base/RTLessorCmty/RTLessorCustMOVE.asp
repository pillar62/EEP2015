<%
key=REQUEST("KEY")
keyary=split(key,";")
COMQ1=KEYARY(0)
LINEQ1=KEYARY(1)
CUSID=KEYARY(2)

Set conn=Server.CreateObject("ADODB.Connection")
conn.open "DSN=RTLib"
Set rs=Server.CreateObject("ADODB.Recordset")
sql="select cusnc from rtlessorcust where comq1=" & comq1 & " and lineq1=" & lineq1 & " and cusid='" & cusid & "'"
rs.open sql,conn
if rs.eof then
   cusnc=""
else
   cusnc=rs("cusnc")   
end if
rs.close
sql="select comn from rtlessorcmtyh where comq1=" & comq1 
rs.open sql,conn
if rs.eof then
   comn=""
else
   comn=rs("comn")   
end if
rs.close
'社區主線清單，並排除現有的主線序號
SQL="SELECT LINEQ1 FROM RTLessorCmtyLine where comq1=" & comq1 & " and canceldat is null and dropdat is null and lineq1<>" & lineq1
rs.Open sql,CONN
if rs.eof then
   s18="<option value="""">無其它主線</option>"
else
    Do While Not rs.Eof
       s18=s18 &"<option value=""" &rs("lineq1") & """>" &rs("lineq1") &"</option>"
       rs.MoveNext
    Loop
end if
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
  '移動主線序號
  comq1=document.all("comq1x").value 
  lineq1=document.all("lineq1x").value 
  cusid=document.all("cusidx").value 
  s18=document.all("search18").value 
  if  Len(trim(s18))=0 Or s18="" then
     msgbox "無其它主線可供移動!"
  else
     StrFeature="top=0,left=0,scrollbars=yes,status=yes," _
                 &"location=no,menubar=no,width=5px,height=5px" 
     prog="RTLessorCustMoveExe.asp" & "?key=" & comq1 & ";" & lineq1 & ";" & cusid & ";" & s18
     Set diagWindow=Window.open(prog,"",StrFeature)
     window.close
  end if
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
<body >
<table ALIGN=CENTER width="80%">
  <tr class=dataListTitle align=center>ET-City用戶資料移動</td><tr>
</table>
<table ALIGN=CENTER width="80%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="30%" align=center>社區名稱</td>
    <td width="70%" bgcolor="silver"><font size=2 color=red>
      <%=comn%></font>
    </td></tr>
<tr><td class=dataListHead align=center>用戶名稱</td>
    <td  bgcolor="silver"><font size=2 color=red>
      <%=cusnc%></font> 
    </td></tr> 
<tr><td class=dataListHead align=center >社區序號</td>
    <td  bgcolor="silver"><font size=2  color=red>
      <input type="text" size="5" name="comq1x" readonly class=dataListdata value="<%=comq1%>"></font> 
    </td></tr> 
<tr><td class=dataListHead align=center style="display:none">用戶代號</td>
    <td  bgcolor="silver" style="display:none">
     <input type="text" size="5" name="cusidx" style="display:none" class=dataListEntry value="<%=cusid%>"> 
    </td></tr>            
<tr><td class=dataListHead align=center >原始主線</td>
    <td  bgcolor="silver"><font size=2  color=red>
     <input type="text" size="5" name="lineq1x" readonly class=dataListdata value="<%=lineq1%>"></font> 
    </td></tr>         
<tr><td class=dataListHead align=center >移動主線</td>
    <td  bgcolor="silver">
      <select name="search18" size="1" class=dataListEntry ID="Select1">
        <%=s18%> 
      </select>
     </td>
</tr>    

<table width="80%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 移動 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>