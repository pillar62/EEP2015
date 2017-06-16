<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>ADSL券商專案－各類報表列印</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  PGM="/report/adsl/adsldeliver3y.asp?parm=" 
  PERSON=document.all("sEARCH2").value
  symd=document.all("search1").value
  Eymd=document.all("search3").value  
  deliverSymd=document.all("search4").value
  deliverEymd=document.all("search5").value  
  
  IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
  IF LEN(TRIM(EYMD))=0 THEN EYMD="2999/12/31"
  IF LEN(TRIM(deliverSymd))=0 THEN deliverSymd="1900/01/01"
  IF LEN(TRIM(deliverEymd))=0 THEN deliverEymd="2999/12/31"
  
  OPT="1"
  pgm=pgm & PERSON & ";" & opt & ";" & deliverSymd & ";" & deliverEymd & ";" & symd & ";" & eymd  '1:送件已三人 2:送件未三人
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
sub b5_onclick()
	if isdate(document.all("search5").value) then
		objEF2KDT.varDefaultDateTime=document.all("search5").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search5").value = objEF2KDT.strDateTime
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
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">ADSL券商專案－報表列印</font></i> </DIV>
<DIV align=Center><i><font face="標楷體" size="3" color="#FF00FF">三人成行送件客戶明細表</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<% 

'完工日--default為系統日期

    Edate=DateValue(Now())    
%>  
</SELECT>
 </font></td>
<tr><td ALIGN="RIGHT"><font face="標楷體">送件人數達 :</font></TD>
    <td><input type="text" size="10" maxlength="10" name="search2" align=right class=dataListEntry value="3" >人</td>
</tr>

<tr><td ALIGN="RIGHT"><font face="標楷體">請輸入送件日期(起) :</font></td>
    <td><input type="text" size="10" maxlength="10" name="search4" align=right class=dataListEntry value="<%=Sdate%>" readonly>
        <input type="button" id="B4" name="B4" height=100% width=100% style="Z-INDEX: 1" value="....">
    </td></tr>
    
<tr><td ALIGN="RIGHT"><font face="標楷體">請輸入送件日期(迄) :</font></td>
    <td><input type="text" size="10" maxlength="10" name="search5" align=right class=dataListEntry value="<%=Edate%>" readonly>
        <input type="button" id="B5" name="B5" height=100% width=100% style="Z-INDEX: 1" value="....">
    </td></tr>

<tr><td ALIGN="RIGHT"><font face="標楷體">請輸入資料建檔日期(起) :</font></td>
    <td><input type="text" size="10" maxlength="10" name="search1" align=right class=dataListEntry value="<%=Sdate%>" readonly>
        <input type="button" id="B1" name="B1" height=100% width=100% style="Z-INDEX: 1" value="....">
    </td></tr>
    
<tr><td ALIGN="RIGHT"><font face="標楷體">請輸入資料建檔日期(迄) :</font></td>
    <td><input type="text" size="10" maxlength="10" name="search3" align=right class=dataListEntry value="<%=Edate%>" readonly>
        <input type="button" id="B3" name="B3" height=100% width=100% style="Z-INDEX: 1" value="....">
    </td></tr>
    
</table> 
<p><center><font face="標楷體">
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand">
 </center>
  <HR><P>
  <table width="100%"><tr>
  <td width="15%"> 說明：</td>
  <TD WIDTH="85%"><font size="2">(1)本表資料範圍，定義以資料截止日期往前七天為（本週）</FONT></TD>
  </TR>
  <TABLE></font></p> 
</BODY> 
</HTML>