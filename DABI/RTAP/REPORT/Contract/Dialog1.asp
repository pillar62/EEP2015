<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>�X�����ҦC�L</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  PGM="/report/contract/ContractReport1.asp?parm=" 
  symd=document.all("search1").value
  eymd=document.all("search2").value
  IF LEN(TRIM(SYMD))=0 THEN SYMD="1900/01/01"
  IF LEN(TRIM(EYMD))=0 THEN SYMD="2070/12/31"
  
  'yyyymm=document.all("search3").value 
  'if Len(Rtrim(yyyymm)) = 1 then yyyymm = "0" & yyyymm
  'yyyymm=document.all("search1").value & yyyymm
  'groupid=document.all("sEARCH6").value  '�~�ȸs�եN��
  'eusr=document.all("search4").value 
  'pgm=pgm & yyyymm & ";" & Rtrim(groupid) & ";" & eusr
  'msgbox pgm
  window.open pgm & symd &";"& eymd
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
	width=60 >
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="background:lightblue">
<DIV align=Center><i><font face="�з���" size="5" color="#FF00FF">�X���޲z�г����C�L</font></i> </DIV>
<DIV align=Center><i><font face="�з���" size="3" color="#FF00FF">�X�����ҦC�L </font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
</SELECT>  
 </font></td> 
  <tr><td ALIGN="RIGHT"><font face="�з���">�п�J���ɤ�_���G</font></td>
	<td><input type="text" size="10" maxlength="10" name="search1" align=right class=dataListEntry value="<%=Sdate%>" readonly ID="Text1">
		<input type="button" id="B1" name="B1" height=100% width=100% style="Z-INDEX: 1" value="...."></td></tr>

  <tr><td>&nbsp;</td>
	<td><input type="text" size="10" maxlength="10" name="search2" align=right class=dataListEntry value="<%=Edate%>" readonly ID="Text2">
		<input type="button" id="B2" name="B2" height=100% width=100% style="Z-INDEX: 1" value="...."></td></tr>
		
</table> 
<p><center><font face="�з���">
 <INPUT TYPE="button" VALUE="�e�X" ID="cmdSure"   
 style="font-family: �з���; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="����" ID="cmdcancel"   
 style="font-family: �з���; color: #FF0000;cursor:hand">
 </center>
  <HR><P>
  <table width="100%">
  �`�N�ƶ��G<BR>
	�L�����п��-- LQ2080C<BR>
	�ȱi���-- German Std Fanfold (German SF)<BR>
	�ȱi�ӷ�-- �o�޾�
  <TABLE></font></p> 
</BODY> 
</HTML>