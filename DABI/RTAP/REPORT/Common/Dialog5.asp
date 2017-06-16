<HTML>
<HEAD>
	<meta http-equiv=Content-Type content="text/html; charset=Big5">
	<TITLE>裝機報竣一覽表(自營+Sparq)</TITLE>
</HEAD>

<BODY style="background:lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">自營+Sparq -- 報表列印(Excel)</font></i> </DIV>
<DIV align=Center><i><font face="標楷體" size="3" color="#FF00FF">裝機報竣一覽表(自營+Sparq)</font></i> </DIV>

<P></P>

<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
  <tr><td ALIGN="RIGHT"><font face="標楷體">請輸入報竣日起迄：</font></td>
	<td><input type="text" size="10" maxlength="10" name="search1" align=right class=dataListEntry value="<%=datevalue(now())%>" readonly ID="Text1">
		<input type="button" onmousedown="Cal('search1')" height=100% width=100% style="Z-INDEX: 1" value="...."></td></tr>

  <tr><td>&nbsp;</td>
	<td><input type="text" size="10" maxlength="10" name="search2" align=right class=dataListEntry value="<%=datevalue(now())%>" readonly ID="Text2">
		<input type="button" onmousedown="Cal('search2')" height=100% width=100% style="Z-INDEX: 1" value="...."></td></tr>
		
</table> 

<p><center>
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure" style="font-family: 標楷體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel" style="font-family: 標楷體; color: #FF0000;cursor:hand">
 </center>
  <HR>
</P>

<Script Language=vbscript>
	Sub cmdSure_onClick
		PGM="Report5.asp?parm=" 
		symd=document.all("search1").value
		eymd=document.all("search2").value
		IF LEN(TRIM(SYMD))=0 THEN SYMD=datevalue(now())
		IF LEN(TRIM(EYMD))=0 THEN SYMD=datevalue(now())
		window.open pgm & symd &";"& eymd &";"
		window.close
	End Sub

	Sub cmdcancel_onClick
		window.close
	End Sub
</Script>
<!-- #include virtual="/webap/include/Calendar.asp" -->

</BODY> 
</HTML>
