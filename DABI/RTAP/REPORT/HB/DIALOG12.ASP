<%
    Dim rs,conn,S6
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
    rs.Open "SELECT CODENC FROM RTCODE WHERE KIND ='B3' AND PARM1 <>'AA' ",CONN
    Do While Not rs.Eof
       s6=s6 &"<option value=""" &rs("CODENC") & """>" &rs("CODENC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>

<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>CHT599 社區送線一覽表</TITLE>

<SCRIPT language=VBScript>
Sub cmdSure_onClick
  symd=document.all("search1").value
  eymd=document.all("search4").value 
  belong=document.all("search2").value
  consignee=document.all("search5").value  
  
  IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
  IF LEN(TRIM(EYMD))=0 THEN SYMD="2070/12/31"
  
  pgm="/report/HB/HBReport12.asp?parm=" & symd &";"& eymd &";"& belong &";"& consignee
  set wHandle=window.open (pgm,"win1")
  ' window.close
End Sub

Sub cmdcancel_onClick
  window.close
End Sub

sub b1_onclick()
	if isdate(document.all("search1").value) then
		objEF2KDT.varDefaultDateTime=document.all("search1").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search1").value = objEF2KDT.strDateTime
	end if
end sub

sub b2_onclick()
	if isdate(document.all("search4").value) then
		objEF2KDT.varDefaultDateTime=document.all("search4").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search4").value = objEF2KDT.strDateTime
	end if
end sub

</SCRIPT>

</HEAD>
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codeBase=http://www.cbbn.com.tw/stock/EF2KDT.CAB#version=9,0,0,3 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="BACKGROUND: lightblue">

<DIV align=center><i><font face="標楷體" size="5" color="#ff00ff">HiBuilding報表列印或轉檔</font></i> </DIV>
<DIV align=center><i><font face="標楷體" size="3" color="#ff00ff">CHT599 社區送線一覽表</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<% 
    Edate=DateValue(Now())    
%>  
</SELECT></FONT></TD>
<tr><td ALIGN="right"><font face="標楷體">請輸入社區送線日期(起):</font></td>
<td>
   <input size="10" maxlength="10" name="search1" align=right class=dataListEntry value="<%=Sdate%>" readonly>
   <input type="button" id="B1" name="B1" height="100%" width="100%" style="Z-INDEX: 1" value="....">
</td></tr>

  <tr><td ALIGN="right"><font face="標楷體">請輸入社區送線日期(迄):</font></td>
<td>
   <input size="10" maxlength="10" name="search4" align=right class=dataListEntry value="<%=Edate%>" readonly>
   <input type="button" id="B2" name="B2" height="100%" width="100%" style="Z-INDEX: 1" value="....">
</td></tr>
<tr><td ALIGN="right"><font face="標楷體">直銷:</font></td>
	<td><select name="search2" size="1" class=dataListEntry>
        <option value="*" SELECTED>全部</option>          
        <option value="no">無</option>
        <option value="台北">台北</option>
        <option value="桃園">桃園</option>
        <option value="台中">台中</option>
        <option value="高雄">高雄</option>
    </select></td></tr>
<tr><td ALIGN="right"><font face="標楷體">經銷:</font></td>
	<td><select name="search5" size="1" class=dataListEntry>
        <option value="*" SELECTED>全部</option>          
        <option value="no">無</option>
		<%=s6%>
    </select></td></tr>
		
</table> 
<p><center><font face="標楷體">
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure"   
 style="COLOR: #ff0000; CURSOR: hand; FONT-FAMILY: 標楷體"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel"   
 style="COLOR: #ff0000; CURSOR: hand; FONT-FAMILY: 標楷體">
 </center><HR>
</FONT>
</BODY> 
</HTML>
