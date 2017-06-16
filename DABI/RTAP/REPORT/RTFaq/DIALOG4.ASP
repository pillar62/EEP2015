<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>桃竹苗客訴一覽表(Excel)</TITLE>

<SCRIPT language=VBScript>
Sub cmdSure_onClick
  'rpttype=document.all("search4").value
  symd=document.all("search1").value
  eymd=document.all("search2").value 
  IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
  IF LEN(TRIM(EYMD))=0 THEN EYMD="2070/12/31"
  
  'if rpttype="A" then
		pgm="/report/RTFaq/FaqReport4.asp?parm=" & symd &";"&eymd
  'elseif rpttype ="B" then
		'pgm="/report/HB/HBReport17b.asp?parm=" & eymd
  'end if    
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
	if isdate(document.all("search2").value) then
		objEF2KDT.varDefaultDateTime=document.all("search2").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search2").value = objEF2KDT.strDateTime
	end if
end sub

</SCRIPT>

</HEAD>
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codeBase=http://www.cbbn.com.tw/stock/EF2KDT.CAB#version=9,0,0,3 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 >
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="BACKGROUND: lightblue">

<DIV align=center><i><font face="標楷體" size="5" color="#ff00ff">客訴案報表列印</font></i> </DIV>
<DIV align=center><i><font face="標楷體" size="3" color="#ff00ff">桃竹苗客訴一覽表(Excel)</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<% 
    Edate=DateValue(Now())    
%>  
</SELECT></FONT></TD>
<!--
<tr><td ALIGN="RIGHT"><font face="標楷體">類別：</font></td>  
	<td><select name="search4" size="1" class=dataListEntry>
	   		<option value="A" selected>總表</option>
			<option value="B">明細</option></select></td></tr>
-->
<tr><td ALIGN="right"><font face="標楷體">請輸入受理起始日:</font></td>
<td>
   <input size="10" maxlength="10" name="search1" align=right class=dataListEntry value="<%=Sdate%>" readonly>
   <input type="button" id="B1" name="B1" height="100%" width="100%" style="Z-INDEX: 1" value="....">
</td></tr>

<tr><td ALIGN="right"><font face="標楷體">受理截止日:</font></td>
<td>
   <input size="10" maxlength="10" name="search2" align=right class=dataListEntry value="<%=Edate%>" readonly>
   <input type="button" id="B2" name="B2" height="100%" width="100%" style="Z-INDEX: 1" value="....">
</td></tr>

		
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
