<%
    theyear = Datepart("yyyy", Now())
    themonth = Datepart("m", Now())
    Edate = Dateadd("d", -1, CStr(theyear) &"/"& CStr(themonth) &"/"& "1")
    Sdate = Datepart("yyyy", Edate) &"/"& Datepart("m", Edate) &"/"& "1"
    rocdate = Cstr(Datepart("yyyy", Sdate) -1911) &_
			right("0" & Cstr(Datepart("m", Sdate)), 2) & "計量制新用戶.xls"
%>
<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>計量制新用戶轉Excel檔</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  PGM="RTCmtyInqFlowCountX.ASP?parm=" 
  yymmdd=document.all("search1").value
  yymmdd2=document.all("search2").value
  xlsname=document.all("search3").value
  pgm=pgm & yymmdd &";"& yymmdd2 &";"& xlsname
 ' msgbox pgm
  window.open pgm 
   window.close
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
	width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="background:lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">計量制用戶轉Excel檔</font></i> </DIV>

<P></P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>

</SELECT>  
 </font></td>
 
<tr><td ALIGN="RIGHT"><font face="標楷體">報竣日(起) :</font></td>
<td>
   <input type="text" size="10" maxlength="10" name="search1" align=right class=dataListEntry value="<%=Sdate%>" readonly>
   <input type="button" id="B1" name="B1" height=100% width=100% style="Z-INDEX: 1" value="....">
</td></tr>

<tr><td ALIGN="RIGHT"><font face="標楷體">報竣日(迄) :</font></td>
<td>
   <input type="text" size="10" maxlength="10" name="search2" align=right class=dataListEntry value="<%=Edate%>" readonly>
   <input type="button" id="B2" name="B2" height=100% width=100% style="Z-INDEX: 1" value="....">
</td></tr>

<tr><td>
   <input type="hidden" name="search3" value="<%=rocdate%>">
</td></tr>

</table> 
<p><center><font face="標楷體">
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand">
 </center></font></p>
  <HR>
</BODY> 
</HTML>