<%
srt1=""
srt1=srt1 &"<option value=""""></option>"
srt1=srt1 &"<option value=""RTLessorAVSCMTYH.comn"">���ϦW��</option>"
srt1=srt1 &"<option value=""RTLessorAVSCust.cusnc"">�Τ�W��</option>"
srt1=srt1 &"<option value=""addr"">�Τ�a�}</option>"
srt1=srt1 &"<option value=""RTLessorAVSCust.duedat"">�����</option>"
srt1=srt1 &"<option value=""validdat"">�i�Τ��</option>"
srt2=""
srt2=srt2 &"<option value=""""></option>"
srt2=srt2 &"<option value=""RTLessorAVSCMTYH.comn"">���ϦW��</option>"
srt2=srt2 &"<option value=""RTLessorAVSCust.cusnc"">�Τ�W��</option>"
srt2=srt2 &"<option value=""addr"">�Τ�a�}</option>"
srt2=srt2 &"<option value=""RTLessorAVSCust.duedat"">�����</option>"
srt2=srt2 &"<option value=""validdat"">�i�Τ��</option>"
srt3=""
srt3=srt3 &"<option value=""""></option>"
srt3=srt3 &"<option value=""RTLessorAVSCMTYH.comn"">���ϦW��</option>"
srt3=srt3 &"<option value=""RTLessorAVSCust.cusnc"">�Τ�W��</option>"
srt3=srt3 &"<option value=""addr"">�Τ�a�}</option>"
srt3=srt3 &"<option value=""RTLessorAVSCust.duedat"">�����</option>"
srt3=srt3 &"<option value=""validdat"">�i�Τ��</option>"
srt4=""
srt4=srt4 &"<option value=""""></option>"
srt4=srt4 &"<option value=""RTLessorAVSCMTYH.comn"">���ϦW��</option>"
srt4=srt4 &"<option value=""RTLessorAVSCust.cusnc"">�Τ�W��</option>"
srt4=srt4 &"<option value=""addr"">�Τ�a�}</option>"
srt4=srt4 &"<option value=""RTLessorAVSCust.duedat"">�����</option>"
srt4=srt4 &"<option value=""validdat"">�i�Τ��</option>"
srt5=""
srt5=srt5 &"<option value=""""></option>"
srt5=srt5 &"<option value=""RTLessorAVSCMTYH.comn"">���ϦW��</option>"
srt5=srt5 &"<option value=""RTLessorAVSCust.cusnc"">�Τ�W��</option>"
srt5=srt5 &"<option value=""addr"">�Τ�a�}</option>"
srt5=srt5 &"<option value=""RTLessorAVSCust.duedat"">�����</option>"
srt5=srt5 &"<option value=""validdat"">�i�Τ��</option>"
srt99=""
srt99=srt99 &"<option value=""A"">�Ѥp��j�Ƨ�</option>"
srt99=srt99 &"<option value=""D"">�Ѥj��p�Ƨ�</option>"
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim aryStr,s,t,r
  '�Τ�W��
  s9=document.all("search9").value 
  if  Len(trim(s9))=0 Or s9="" then
     t=t & " and RTLessorAVScust.comq1 <> 0 "
  else
     s=s & " �Τ�W�١J�]�t('" & s9 & "')�r�� "
     t=t & " and (RTLessorAVSCust.cusnc like '%" & s9 & "%') "
  end if
  '----ú�ڶg��
  s10ary=split(document.all("search10").value,";")  
  If Len(trim(s10ary(0)))=0 Or s10ary(0)="" Then
  Else
     s=s &"  ú�ڶg��:" &s10ary(1)
     t=t &" AND (RTLessorAVSCust.paycycle='" & s10ary(0) & "') "
  End If   
  '----ú�ڤ覡
  s11ary=split(document.all("search11").value,";")  
  If Len(trim(s11ary(0)))=0 Or s11ary(0)="" Then
  Else
     s=s &"  ú�ڤ覡:" &s11ary(1)
     t=t &" AND (RTLessorAVSCust.paytype='" & s11ary(0) & "') "
  End If 
  '�p���q��
  s12=document.all("search12").value 
  if  Len(trim(s12))=0 Or s12="" then
  else
     s=s & " �p���q�ܡJ�]�t('" & s12 & "')�r�� "
     t=t & " and ((RTLessorAVSCust.contacttel like '%" & s12 & "%') OR (RTLessorAVSCust.mobile like '%" & s12 & "%')) "
  end if   
  '������/�νs
  s13=document.all("search13").value 
  if  Len(trim(s13))=0 Or s13="" then
  else
     s=s & " ������/�νs�J�]�t('" & s13 & "')�r�� "
     t=t & " and (RTLessorAVSCust.socialid like '%" & s13 & "%') "
  end if     
  '�����_��
  s14=document.all("search14").value 
  s15=document.all("search15").value   
  if  (Len(trim(s14))=0 Or s14="") and (Len(trim(s15))=0 Or s15="") then
  else
     if  (Len(trim(s14))=0 Or s14="") then s14="1900/01/01"
     if  (Len(trim(s15))=0 Or s15="") then s15="9999/12/31"
     s=s & " �Τ�����_���J��('" & s14 & "') �� ('" & s15 & "') "
     t=t & " and RTLessorAVSCust.duedat >= '" & s14 & " 00:00.000' and RTLessorAVSCust.duedat  <= '" & s15 & " 23:59.997' "
  end if               
  '----�Τ�i�ת��p
  s18ary=split(document.all("search18").value,";")  
  If Len(trim(s18ARY(0))) = 0 Then
 '�����u
  ELSEIF s18ARY(0) = "1" THEN
     s=s &"  �Τ�i��:('" &s18ARY(1) & "') "
     t=t &" AND (RTLessorAVSCust.FINISHDAT IS NULL and RTLessorAVSCust.dropdat is null and RTLessorAVSCust.canceldat is null) "      
  '�w���u�L�p�O��
  ELSEIF s18ARY(0) = "2" THEN
     s=s &"  �Τ�i��:('" &s18ARY(1) & "') "
     t=t &" AND RTLessorAVSCust.FINISHDAT IS NOT NULL AND  RTLessorAVSCust.strbillingdat IS NULL "               
  '�w�h��
  ELSEIF s18ARY(0) = "3" THEN
     s=s &"  �Τ�i��:('" &s18ARY(1) & "') "
     t=t &" AND  RTLessorAVSCust.dropdat IS NOT NULL AND RTLessorAVSCust.strbillingdat IS not NULL "       
  '�w�@�o
  ELSEIF s18ARY(0) = "4" THEN
     s=s &"  �Τ�i��:('" &s18ARY(1) & "') "
     t=t &" AND  RTLessorAVSCust.CANCELDAT IS NOT NULL AND RTLessorAVSCust.finishdat IS NULL "                 
  End If              
  '�Ƨ�
  SRT1 =document.all("srt1X").value
  SRT2 =document.all("srt2X").value
  SRT3 =document.all("srt3X").value
  SRT4 =document.all("srt4X").value
  SRT5 =document.all("srt5X").value
  SRT99=document.all("srt99X").value
  srtx=""
  if Len(trim(srt1))> 0 then
     srtx=srtx & " order by " & srt1
  ELSE
     srtx=srtx & " order by RTLessorAVSCUST.COMQ1 "
  end if
  if Len(trim(srt2))> 0 then
     srtx=srtx & "," & srt2
  end if
  if Len(trim(srt3))> 0 then
     srtx=srtx & "," & srt3
  end if
  if Len(trim(srt4))> 0 then
     srtx=srtx & "," & srt4
  end if
  if Len(trim(srt5))> 0 then
     srtx=srtx & "," & srt5
  end if
  if srt99="D" THEN
     SRTX=SRTX & " DESC "
  END IF  

  Dim winP,docP
  Set winP=window.Opener
  Set docP=winP.document
  docP.all("searchQry").value=t
  docP.all("searchQry2").value=SRTX
  docP.all("searchShow").value=s
  docP.all("keyform").Submit
  winP.focus()
  window.close
End Sub
Sub Srbtnonclick()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="search" & clickid
	   if isdate(document.all(clickkey).value) then
	      objEF2KDT.varDefaultDateTime=document.all(clickkey).value
       end if
       call objEF2KDT.show(1)
       if objEF2KDT.strDateTime <> "" then
          document.all(clickkey).value = objEF2KDT.strDateTime
       end if
End Sub 
Sub btn1_onClick()  
  Dim winP
  Set winP=window.Opener
  winP.focus()
  window.close  
End Sub
-->
</script>
</head>
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"       codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<body>
<table width="100%">
  <tr class=dataListTitle align=center>AVS-City�Τ��Ʒj�M����</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">�Τ�W��</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search9" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead width="40%">�Τ�i�ת��p</td>
    <td width="60%"  bgcolor="silver">
      <select name="search18" size="1" class=dataListEntry ID="Select1">
        <option value=";����" selected>����</option>
        <option value="1;�����u">�����u</option>
        <option value="2;�w���u�L�p�O��">�w���u�L�p�O��</option>
        <option value="3;�w�h��">�w�h��</option>      
        <option value="4;�w�@�o">�w�@�o</option>                     
      </select>
     </td>
</tr>    
<tr><td class=dataListHead width="40%">ú�ڶg��</td>
    <td width="60%" bgcolor="silver">
     <select name="search10" size="1" class=dataListEntry>
        <option value=";����" selected>����</option>
        <option value="05;�uú">�uú</option>
        <option value="01;�b�~ú">�b�~ú</option>
        <option value="02;�@�~ú">�@�~ú</option>     
        <option value="03;��~ú">��~ú</option> 
        <option value="04;�T�~ú">�T�~ú</option>            
     </select>
    </td></tr>        
<tr><td class=dataListHead width="40%">ú�ڤ覡</td>
    <td width="60%" bgcolor="silver">
     <select name="search11" size="1" class=dataListEntry>
        <option value=";����" selected>����</option>
        <option value="02;�{��">�{��</option>
        <option value="01;�H�Υd">�H�Υd</option>
        <option value="03;ATM��b">ATM��b</option>                
     </select>
    </td></tr>            
<tr><td class=dataListHead width="40%">�p���q��(�Φ��)</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="10" name="search12" class=dataListEntry> 
    </td></tr>   
<tr><td class=dataListHead width="40%">�Τᨭ����/�νs</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="10" name="search13" class=dataListEntry> 
    </td></tr>          
<tr><td class=dataListHead width="40%">�Τ�����</td>
    <td width="60%" bgcolor="silver"><font size=2>��</font>
      <input type="text" size="10" name="search14" class=dataListEntry> 
<input type="button" id="B14"  name="B14" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">      
      <font size=2>��</font>
      <input type="text" size="10" name="search15" class=dataListEntry> 
<input type="button" id="B15"  name="B15" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">            
    </td></tr>        
<tr><td class=dataListHead >��ƱƧ�</td>
    <td  bgcolor="silver">
    <select name="srt1X" size="1" class=dataListEntry>
        <%=srt1%>
    </select>
    <select name="srt2X" size="1" class=dataListEntry>
        <%=srt2%>
    </select>
    <select name="srt3X" size="1" class=dataListEntry>
        <%=srt3%>
    </select>
    <select name="srt4X" size="1" class=dataListEntry>
        <%=srt4%>
    </select>
    <select name="srt5X" size="1" class=dataListEntry>
        <%=srt5%>
    </select>
    <select name="srt99X" size="1" class=dataListEntry>
        <%=srt99%>
    </select>
    </td></tr>               
</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" �d�� " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" ���� " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>