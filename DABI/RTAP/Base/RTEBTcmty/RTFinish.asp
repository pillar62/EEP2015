<%
key1=request("key")
keyary=split(key1,";")
set conn=server.CreateObject("ADODB.connection")
set rs=server.CreateObject("ADODB.recordset")
DSN="DSN=RTLIB"
sql="select CMTYTEL from rtsparqadslcmty where cutyid=" & keyary(0)
conn.Open DSN
rs.Open sql,conn
if not rs.EOF then
   MainLineNo=rs("CMTYTEL")
end if
rs.Close
sql="select exttel,SPHNNO from rtsparqadslcust where comq1=" & keyary(0) & " and cusid='" & keyary(1) & "' and entryno=" & keyary(2)
rs.Open sql,conn
if not rs.EOF then
   HNNO=rs("exttel") & rs("sphnno")
end if
rs.Close
conn.Close
set rs=nothing
set conn=nothing
%>
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>�t��ADSL399�����T�{�@�~</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  key1=document.all("search1").value '���ϥN�� + �Ȥ�N�X + �Ȥ�榸
  key2=document.all("search2").value '���Ϫ����q�ܸ��X
  key3=document.all("search3").value '��b�s��
  FDAT=document.all("search7").value '�������
 
  if Len(trim(FDAT)) = 0 then 
     MSGBOX "����������i�ť�!"
  elseif len(trim(key2)) = 0 then
     msgbox "���Ϫ����q�ܪťաA���i�i������@�~!"
  elseif len(trim(key3)) > 0 then
     msgbox "���Ȥ�w�����A���i���г���!"
  else
    prog="RTFinishProcess.asp" & "?key=" & key1 & "&FDAT=" & FDAT & "&TEL=" & KEY2
   ' Scrxx=window.screen.width
   ' Scryy=window.screen.height - 30
    Scrxx=0
    Scryy=0
    StrFeature="top=0,left=0,scrollbars=yes,status=yes," _
                 &"location=no,menubar=no,width=" & scrxx & "px" _
                 &",height=" & scryy & "px,fullscreen=yes" 
    Set diagWindow=Window.open(prog,"d2",StrFeature)  
    diawidth=0
    diaheight=0
  '  Set diagWindow=Window.Showmodaldialog(prog,"d2","dialogWidth:" & diawidth & "px;dialogHeight:" & diaheight &"px;")  
    if returnvalue=ture then
       msgbox "�Ȥ�����@�~����!"
    else
       msgbox "�Ȥ�����@�~���`�A�гq����T���B�z�C"
    end if
    returnvalue=True
    window.close  
  end if
End Sub
Sub cmdcancel_onClick
   window.close
End Sub
sub b1_onclick()
	if isdate(document.all("search7").value) then
		objEF2KDT.varDefaultDateTime=document.all("search7").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search7").value = objEF2KDT.strDateTime
	end if
end sub

</SCRIPT>
</HEAD>
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"  codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="background:lightblue">
<DIV align=Center><i><font face="�з���" size="5" color="#FF00FF">�t��399�Ȥ�����@�~</font></i> </DIV>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
  <tr><td><font face="�з���" align=right>�п�ܳ������ :</font></td>

<td><input type="text" size="10" maxlength="10" name="search7" align=right class=dataListEntry value="<%=Sdate%>" readonly> 
    <input type="button" id="B1" name="B1" height=100% width=100% style="Z-INDEX: 1" value="....">
    <input type="TEXT"  name="search1" height=100% width=100%  style="display:none" value="<%=KEY1%>">
    <input type="TEXT"  name="search2" height=100% width=100%  style="display:none" value="<%=MainlineNo%>">   
         <input type="TEXT"  name="search3" height=100% width=100%  style="display:none" value="<%=HNNO%>">    </td>
</tr>
</table>
<center>
<p>
<INPUT TYPE="button" VALUE="�T�{" ID="cmdSure" style="font-family: �з���; color: #FF0000;cursor:hand">   
<INPUT TYPE="button" VALUE="����" ID="cmdcancel" style="font-family: �з���; color: #FF0000;cursor:hand">   
</center>
<p><font size=2 >(1)��[�T�{]��A�t�αN�̾ڨC��Τ����<font size=2 color=red>[����]</font>���������q�ܸ�ơA���͹�t��<font size=2 color=red>[�Τ��b�N�X]</font> �C</font> 
<br><font size=2 >�è̾ڱz�ҿ�������������s��s<font size=2 color=red>[�������]</font>�椧���A�C</font>
<br><font size=2 color=red>(2)�@���z������A�Ӫ��Ϥ������q�ܱN�L�k�A���ʡA�Х��T�{���Ϥ������q�ܬO�_���T!</font> 
</BODY> 
</HTML>