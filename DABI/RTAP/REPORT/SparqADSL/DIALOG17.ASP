<%
    Dim rs,conn,S6, S7
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
'---業務轄區
    rs.Open "SELECT AREAID, AREANC FROM RTArea WHERE AREATYPE ='1' ",CONN
    s7="<option value=""*"" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       s7=s7 &"<option value=""" &rs("AREAID") & """>" &rs("AREANC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'---業務群組
    rs.Open "SELECT GROUPID, GROUPNC FROM RTSALESGROUP ",CONN
    s6="<option value=""*"" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       s6=s6 &"<option value=""" &rs("GROUPID") & """>" &rs("GROUPNC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'---經銷商
    rs.Open "SELECT a.CUSID, b.SHORTNC FROM RTConsignee a, RTObj b WHERE a.CUSID = b.CUSID ORDER BY b.SHORTNC ",CONN
    s8="<option value=""*"" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       s8=s8 &"<option value=""" &rs("CUSID") & """>" &rs("SHORTNC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'----------
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>速博 ADSL－各類報表列印</TITLE>
<SCRIPT language=VBScript>

Sub cmdSure_onClick
  pgm="/report/sparqADSL/ADSLReport17.asp?parm=" 
  'msgbox pgm
  yesno=document.all("search6").value    
  symd=document.all("search9").value
  Eymd=document.all("search10").value
  applySymd=document.all("search11").value
  applyEymd=document.all("search12").value
  groupid=document.all("sEARCH13").value  '業務群組代號  
  areaid=document.all("sEARCH14").value
  setupcontact=document.all("sEARCH15").value
  consignee=document.all("sEARCH16").value
  IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
  IF LEN(TRIM(EYMD))=0 THEN EYMD="2999/12/31"
  IF LEN(TRIM(applySymd))=0 THEN applySYMD="1900/01/01"
  IF LEN(TRIM(applyEymd))=0 THEN applyEYMD="2999/12/31"
  pgm=pgm & yesno &";"& symd &";"& eymd &";"& applysymd &";"& applyeymd &";"& areaid &";"& groupid &";"& setupcontact &";"& consignee
  window.open pgm 
   window.close
End Sub

Sub cmdcancel_onClick
  window.close
End Sub

sub b1_onclick()
	if isdate(document.all("search9").value) then
		objEF2KDT.varDefaultDateTime=document.all("search9").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search9").value = objEF2KDT.strDateTime
	end if
end sub

sub b3_onclick()
	if isdate(document.all("search10").value) then
		objEF2KDT.varDefaultDateTime=document.all("search10").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search10").value = objEF2KDT.strDateTime
	end if
end sub

sub b2_onclick()
	if isdate(document.all("search11").value) then
		objEF2KDT.varDefaultDateTime=document.all("search11").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search11").value = objEF2KDT.strDateTime
	end if
end sub

sub b4_onclick()
	if isdate(document.all("search12").value) then
		objEF2KDT.varDefaultDateTime=document.all("search12").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search12").value = objEF2KDT.strDateTime
	end if
end sub

Sub Sr34salesonclick()
    prog="RTGetsalesD.asp"
    prog=prog & "?KEY=" & document.all("search14").VALUE & ";" & document.all("search13").VALUE
    FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
    if fusr <> "" then
    FUsrID=Split(Fusr,";")   
    if Fusrid(2) ="Y" then
       document.all("search15").value =  trim(Fusrid(0))
    End if       
    end if
End Sub               

</SCRIPT>
</HEAD>

<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codeBase=http://www.cbbn.com.tw/stock/EF2KDT.CAB#version=9,0,0,3 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 >
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>
	
<BODY style="background:lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">速博ADSL－報表列印</font></i> </DIV>
<DIV align=Center><i><font face="標楷體" size="3" color="#FF00FF">速博ADSL社區資料表</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>

</SELECT>  
 </font></td>
 
 <tr><td ALIGN="RIGHT"><font face="標楷體">若社區號碼空白，是否列印 :</font></td>
    <td><select name="search6" size="1" class=dataListEntry>
        <option value="Y">是</option>
        <option value="N">否</option>
        </select>
    </td>
</tr>

<tr><td ALIGN="RIGHT"><font face="標楷體">申請日期(起迄) :</font></td>
	<td><input type="text" size="10" maxlength="10" name="search9" align=right class=dataListEntry value="<%=Sdate%>" readonly ID="Text1">
		<input type="button" id="B1" name="B1" height=100% width=100% style="Z-INDEX: 1" value="...."></td><td>～&nbsp;</td>
	<td><input type="text" size="10" maxlength="10" name="search10" align=right class=dataListEntry value="<%=Edate%>" readonly ID="Text2">
		<input type="button" id="B3" name="B3" height=100% width=100% style="Z-INDEX: 1" value="...."></td></tr>

<tr><td ALIGN="RIGHT"><font face="標楷體">社區開通日(起迄) :</font></td>
	<td><input type="text" size="10" maxlength="10" name="search11" align=right class=dataListEntry value="<%=Sdate%>" readonly>
		<input type="button" id="B2" name="B2" height=100% width=100% style="Z-INDEX: 1" value="...."></td><td>～&nbsp;</td>
	<td><input type="text" size="10" maxlength="10" name="search12" align=right class=dataListEntry value="<%=Edate%>" readonly ID="Text3">
		<input type="button" id="Button1" name="B4" height=100% width=100% style="Z-INDEX: 1" value="...."></td></tr>

<tr><td ALIGN="RIGHT"><font face="標楷體">業務轄區 :</font></td>
    <td><select name="search14" size="1" class=dataListEntry>
        <%=s7%>
        </select></td></tr>

<tr><td ALIGN="RIGHT"><font face="標楷體">業務群組 :</font></td>
    <td><select name="search13" size="1" class=dataListEntry>
        <%=s6%>
        </select></td></tr>
        
<tr><td ALIGN="RIGHT"><font face="標楷體">建置聯絡人 :</font></td>                          
    <td><input type="text" name="search15" size="10" value="全部" class="dataListEntry" ID="Text4">   
        <input type="button" id="B34"  name="B34" height=100% width=100% style="Z-INDEX: 1"  value="..." onclick="Sr34salesonclick()"></td><tr>
        
<tr><td ALIGN="RIGHT"><font face="標楷體">經銷商 :</font></td>
    <td><select name="search16" size="1" class=dataListEntry>
        <%=s8%>
        </select></td></tr>
  
</table><p>

<center><font face="標楷體">
  <INPUT TYPE="button" VALUE="送出" ID="cmdSure" style="font-family: 標楷體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel" style="font-family: 標楷體; color: #FF0000;cursor:hand">
</center><HR>

</BODY> 
</HTML>
