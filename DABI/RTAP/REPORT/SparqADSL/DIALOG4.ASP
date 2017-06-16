<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>Sparq399退租客戶一覽表</TITLE>

<SCRIPT language=VBScript>
Sub cmdSure_onClick
  symd=document.all("search1").value
  eymd=document.all("search2").value 
  syymmdd=document.all("search3").value
  eyymmdd=document.all("search4").value 
  IF LEN(TRIM(SYYMMDD))=0 THEN SYYMMDD="1900/01/01"
  IF LEN(TRIM(EYYMMDD))=0 THEN EYYMMDD="2070/12/31"
  IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
  IF LEN(TRIM(EYMD))=0 THEN EYMD="2070/12/31"

  
  pgm="/report/sparqADSL/ADSLReport4.asp?parm=" & symd &";"& eymd &";"& syymmdd &";"& eyymmdd
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

sub b3_onclick()
	if isdate(document.all("search3").value) then
		objEF2KDT.varDefaultDateTime=document.all("search3").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search3").value = objEF2KDT.strDateTime
	end if
end sub

sub b4_onclick()
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
	width=60 >
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="BACKGROUND: lightblue">

<DIV align=center><i><font face="標楷體" size="5" color="#ff00ff">Sparq報表列印</font></i> </DIV>
<DIV align=center><i><font face="標楷體" size="3" color="#ff00ff">Sparq399主線速率及客戶數統計</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<% 
    Edate=DateValue(Now())    
%>  
</SELECT></FONT></TD>

<tr><td ALIGN="right"><font face="標楷體">報竣日(起):</font></td>
<td>
   <input size="10" maxlength="10" name="search1" align=right class=dataListEntry value="<%=Sdate%>" readonly>
   <input type="button" id="B1" name="B1" height="100%" width="100%" style="Z-INDEX: 1" value="....">
</td></tr>

<tr><td ALIGN="right"><font face="標楷體">報竣日(迄):</font></td>
<td>
   <input size="10" maxlength="10" name="search2" align=right class=dataListEntry value="<%=Edate%>" readonly>
   <input type="button" id="B2" name="B2" height="100%" width="100%" style="Z-INDEX: 1" value="....">
</td></tr>

<tr><td ALIGN="right"><font face="標楷體">主線測通日(起):</font></td>
<td>
   <input size="10" maxlength="10" name="search3" align=right class=dataListEntry value="<%=Sdate%>" readonly>
   <input type="button" id="B3" name="B3" height="100%" width="100%" style="Z-INDEX: 1" value="....">
</td></tr>

<tr><td ALIGN="right"><font face="標楷體">主線測通日(迄):</font></td>
<td>
   <input size="10" maxlength="10" name="search4" align=right class=dataListEntry value="<%=Edate%>" readonly>
   <input type="button" id="B4" name="B4" height="100%" width="100%" style="Z-INDEX: 1" value="....">
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
