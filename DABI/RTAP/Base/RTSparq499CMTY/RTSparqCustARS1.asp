<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim aryStr,s,t1,t2,r
  '�������I�b�ڪ��A
  s1ary=split(document.all("search1").value,";")
  if s1ary(0)="" then
     t1=t1 & " a.cusid <> '' "
     t2=t1
     s="���� "
  elseif s1ary(0)="1" then
     t1=t1 & " a.canceldat is null and ( a.amt - a.realamt = 0 ) "
     t2=t1
     s="�w�R�b "
  elseif s1ary(0)="2" then
     t1=t1 & " a.canceldat is null and ( a.amt - a.realamt <> 0 ) "
     t2=t1
     s="���R�b�γ����R�b "
  elseif s1ary(0)="3" then
     t1=t1 & " a.canceldat is not null "
     t2=t1
     s="�w�@�o "
  elseif s1ary(0)="4" then
     t1=t1 & " a.canceldat is null "
     t2=t1
     s="����(���t�@�o) "
  elseif s1ary(0)="5" then
     t1=t1 & " a.canceldat is null and ( a.amt - a.realamt < 0 ) "
     t2=t1
     s="���R���B���t "
  end if
  '�Τ�W��
  s2=document.all("search2").value
  if len(trim(s2)) > 0 then
     t1=t1 & " and i.cusnc like '%" & s2 & "%' "
     t2=t2 & " and b.cusnc like '%" & s2 & "%' "
     s=s & "�A�Τ�W��(�t)�J'" & s2 & "'�r�� "
  end if
    '���ϥN��
  s3=document.all("search3").value
  if len(trim(s3)) > 0 then
     t1=t1 & " and b.comq1 =" & s3 &" "
     t2=t2 & " and b.comq1 =" & s3 &" "
     s=s & "�A���ϥN���J" & s3 & " "
  end if
    '���ϦW��
  s4=document.all("search4").value
  if len(trim(s4)) > 0 then
     t1=t1 & " and c.comn like '%" & s4 & "%' "
     t2=t2 & " and c.comn like '%" & s4 & "%' "
     s=s & "�A���ϦW��(�t)�J'" & s4 & "'�r�� "
  end if
 
  Dim winP,docP
  Set winP=window.opener
  Set docP=winP.document
  docP.all("searchQry").value=t1
  docP.all("searchQry2").value=t2
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
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<body>
<table width="100%">
  <tr class=dataListTitle align=center>�t�եΤ��������I�b�ڸ�Ʒj�M����</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">���ϥN��</td>
    <td width="60%"  bgcolor="silver">
      <input type="text" size="5" name="search3" class=dataListEntry> 
     </td>
</tr>    
<tr><td class=dataListHead width="40%">���ϦW��</td>
    <td width="60%"  bgcolor="silver">
      <input type="text" size="20" name="search4" class=dataListEntry> 
     </td>
</tr>    
<tr><td class=dataListHead width="40%">�Τ�W��</td>
    <td width="60%"  bgcolor="silver">
      <input type="text" size="12" name="search2" class=dataListEntry> 
     </td>
</tr>    
<tr><td class=dataListHead width="40%">�������I�b�ڪ��A</td>
    <td width="60%"  bgcolor="silver">
      <select name="search1" size="1" class=dataListEntry ID="Select1">
        <option value="2;�������R�b" selected>�������R�b</option>
        <option value="1;�w�R�b">�w�R�b</option>
        <option value="3;�w�@�o">�w�@�o</option>      
        <option value="4;����(���t�@�o)">����(���t�@�o)</option>
        <option value="5;���R���B���t">���R���B���t</option>
        <option value=";����">����</option>               
      </select>
     </td>
</tr>    
</table>
<p>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" �d�� " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" ���� " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>