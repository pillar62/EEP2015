<%
    key=request("key")
    keyary=split(key,";")
    Dim rs,conn,S6
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
    S6=""
    today=datevalue(now())
    updsql="SELECT RTCode.CODE, RTCode.CODENC " _
          &"FROM RTCode " _
          &"WHERE (RTCode.KIND = 'C8') and rtcode.code not in ( select  modifycode from RTEBTcustCHGLOG where " _
          &"RTEBTcustCHGLOG.COMQ1=" & keyary(0) & " and RTEBTcustCHGLOG.LINEQ1=" & keyary(1)  _
          &" and RTEBTcustCHGLOG.CUSID='" & KEYARY(2) & "' AND RTEBTcustCHGLOG.modifyDAT='" & today & "' and RTEBTcustCHGLOG.transdat is NOT null ) "
    'Response.Write "UPDSQL=" & updsql
    rs.Open updsql,CONN
  '  s1="<option value=""<>'*';����"" selected>����</option>" &vbCrLf    
    Do While Not rs.Eof
       s6=s6 &"<option value=""" &rs("code") & """>" &rs("codenc") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'------�ˬd�ӫȤ�O�_�w�����B�w����,�u���w���ɹL�~�i�Ѧ�����
    sql="select * from RTEBTCUST where COMQ1=" & keyary(0) & " and LINEQ1=" & KEYARY(1) & " AND CUSID='" & keyary(2) & "' "
    rs.Open sql,conn
 '   Response.Write "DATALEN=" & rs("transdat") 
 '   Response.end
    if rs.EOF then
       endpgm="2"
    elseif NOT ISDATE(rs("transdat")) then
       endpgm="3"
    else
       endpgm="1"
    end if
'---------------------------------------------------------
    rs.close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>�F��AVS�޲z�t��--�w�����Ȥ��Ʋ���</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  Randomize  
  accessmode="U"
  'key:key(0)=���Ͻs�� key(1)=�Ȥ�s�� key(2)=���� key(3)=���ʶ���
  if keyform.htmlfld.value="1" then
     key=document.all("search1").value
     key=key & document.all("search6").value
     prog="RTEBTCustCHGD.asp?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&"
     Scrxx=window.screen.width
     Scryy=window.screen.height - 30
     StrFeature="Top=0,left=0,scrollbars=yes,status=yes," _
               &"location=no,menubar=no,width=" & scrxx & "px" _
               &",height=" & scryy & "px"
    'Set diagWindow=Window.Open(prog,"diag",strFeature)
     Window.Open prog,"diag",strFeature 
    ' window.open pgm 
    ' window.close
   elseif keyform.htmlfld.value="2" then
     msgbox "�Ȥ��Ƨ䤣��A���ˬd�Ȥ��ɸ�ƬO�_�w�Q�R��!"
   elseif keyform.htmlfld.value="3" then
     msgbox "���Ȥ�|���ܪF�˳���(�|������)�A�����\���楻�@�~==> �ЦܫȤ��ƺ��@�@�~�����ץ��Y�i!"
   else
     msgbox "�{������o�Ͳ��`�A�гq����T�H��!"
   end if
End Sub
Sub cmdcancel_onClick
  Dim winP,docP
  Set winP=window.Opener
'  Set docP=winP.document
'  winP.focus()
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

</SCRIPT>
</HEAD>
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codeBase=http://www.cbbn.com.tw/stock/EF2KDT.CAB#version=9,0,0,3 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="background:lightblue">
<!--form name=keyform�O���F�t�X�Ҳլ[�c�һ�-->
<form name="keyform">
<DIV align=Center><i><font face="�ө���" size="5" color="#FF00FF">�F��AVS�޲z�t��</font></i> </DIV>
<DIV align=Center><i><font face="�ө���" size="3" color="#FF00FF">�w�����Ȥ��Ʋ���</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<tr>
 </font></td>
  <tr><td ALIGN="RIGHT"><font face="�ө���">�п�ܲ��ʶ��� :</font></TD>
  <TD><select name="search6" size="1" class=dataListEntry>
      <%=s6%>
      </select>  </td>
  <td style="display:none"><input type="text" name="search1" value="<%=key%>"></td>
</tr>
</table> 
<p><center><font face="�ө���">
 <INPUT TYPE="button" VALUE="�e�X" ID="cmdSure"   
 style="font-family: �ө���; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="����" ID="cmdcancel"   
 style="font-family: �ө���; color: #FF0000;cursor:hand">
 </center>
  <HR><P>
  <center>
  <table width="80%">
  <tr><td colspan="2" width="15%">�@�~����</td></tr>
  <tr><TD WIDTH="5%" align="top">(1)</td><td  WIDTH="95%"align="left">�����@�~�����ʥ������Ȥᤧ"���ʥӽг�"��������! ��Ƨ��ᶷ������~�ޤH���H�������������ɦܪF�ˤ��̾�!</td></tr>
  <tr><TD WIDTH="5%" align="top">(2)</td><td  WIDTH="95%"align="left">���ʶ��ب̨C�@�Ȥ��ڪ��p�Ӧ��Ҥ��P�A�Y�Y�ӫȤ�P�@��P�@���ʶ��ؤw�����ʥB�w���ɮɡA�h�Ӳ��ʶ��رN���A�e�{!</td></tr>
  <TABLE>
  <input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
  </form>
</BODY> 
</HTML>