<%
    Dim rs,i,conn
    Dim search1Opt,search2Opt,search6Opt, search12pt
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    
    Set rs=Server.CreateObject("ADODB.Recordset")
'----------�g�P��
    S11=""
    rs.Open "SELECT  RTEBTCMTYLINE.CONSIGNEE,  CASE WHEN RTEBTCMTYLINE.CONSIGNEE = '' THEN '���P' ELSE RTObj.SHORTNC  END as shortnc FROM  RTEBTCMTYLINE LEFT OUTER JOIN RTObj ON RTEBTCMTYLINE.CONSIGNEE = RTObj.CUSID GROUP BY  RTEBTCMTYLINE.CONSIGNEE,  CASE WHEN RTEBTCMTYLINE.CONSIGNEE = '' THEN '���P' ELSE RTObj.SHORTNC  END ",CONN
    s11="<option value=""*;����"" selected>����</option>" &vbCrLf    
    Do While Not rs.Eof
       s11=s11 &"<option value=""" &rs("CONSIGNEE") & ";" & rs("SHORTNC") & """>" &rs("SHORTNC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'    
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim aryStr,s,t,r
  '----���ϦW��
  S1=document.all("search1").value  
  If Len(s1)=0 Or s1="" Then
     t=t &" (RTEBTCmtyH.ComN<> '*' )" 
  Else
     s=s &"  ���ϦW��:�]�t('" &S1 & "'�r��)"
     t=t &" (RTEBTCmtyH.ComN LIKE '%" &S1 &"%')" 
  End If
  '----�D�u�˾��a�}
  S2=document.all("search2").value  
  If Len(s2)=0 Or s2="" Then
  Else
     s=s &"  �D�u�˾���}:�]�t('" &S2 & "'�r��) "
     t=t &" AND ((RTCounty.CUTNC + RTEBTCMTYLINE.TOWNSHIP + CASE WHEN RTEBTCMTYLINE.VILLAGE " _
         &"<> '' THEN RTEBTCMTYLINE.VILLAGE + RTEBTCMTYLINE.COD1 ELSE '' END + " _
         &"CASE WHEN RTEBTCMTYLINE.NEIGHBOR <> '' THEN RTEBTCMTYLINE.NEIGHBOR " _
         &"+ RTEBTCMTYLINE.COD2 ELSE '' END + CASE WHEN RTEBTCMTYLINE.STREET " _
         &"<> '' THEN RTEBTCMTYLINE.STREET + RTEBTCMTYLINE.COD3 ELSE '' END + CASE " _
         &"WHEN RTEBTCMTYLINE.SEC <> '' THEN RTEBTCMTYLINE.SEC + RTEBTCMTYLINE.COD4 " _
         &"ELSE '' END + CASE WHEN RTEBTCMTYLINE.LANE <> '' THEN RTEBTCMTYLINE.LANE " _
         &"+ RTEBTCMTYLINE.COD5 ELSE '' END + CASE WHEN RTEBTCMTYLINE.TOWN " _
         &"<> '' THEN RTEBTCMTYLINE.TOWN + RTEBTCMTYLINE.COD6 ELSE '' END + CASE " _
         &"WHEN RTEBTCMTYLINE.ALLEYWAY <> '' THEN RTEBTCMTYLINE.ALLEYWAY + " _
         &"RTEBTCMTYLINE.COD7 ELSE '' END + CASE WHEN RTEBTCMTYLINE.NUM <> '' " _
         &"THEN RTEBTCMTYLINE.NUM + RTEBTCMTYLINE.COD8 ELSE '' END + CASE WHEN " _
         &"RTEBTCMTYLINE.FLOOR <> '' THEN RTEBTCMTYLINE.FLOOR + RTEBTCMTYLINE.COD9 " _
         &"ELSE '' END + CASE WHEN RTEBTCMTYLINE.ROOM <> '' THEN RTEBTCMTYLINE.ROOM " _
         &"+ RTEBTCMTYLINE.COD10 ELSE '' END ) LIKE '%" &S2 &"%') " 
  End If
  '----�D�uIP
  s3=document.all("search3").value
  IF LEN(TRIM(S3)) > 0 THEN
     s=s &"  �D�uIP:�]�t('" &s3 & "'�r��) "
     t=t &" AND (RTEBTCMTYLINE.LINEIP LIKE '%" & S3 & "%') "
  END IF
  '----�D�u�p��s��
  s4=document.all("search4").value
  If Len(trim(s4)) > 0 Then
     s=s &"  �p��s��:�]�t('" &s4 & "'�r��) "
     t=t &" AND (RTEBTCMTYLINE.APPLYNO LIKE '%" & S4 & "%') "
  End If    
  '----�D�u�����q��
  s5=document.all("search5").value
  If Len(trim(s5)) > 0 Then
     s=s &"  �����q��:�]�t('" &s5 & "'�r��) "
     t=t &" AND (RTEBTCMTYLINE.LINETEL LIKE '%" & S5 & "%') "
  End If      
  '----�D�u�ӽЦC�L�渹
  s6=document.all("search6").value
  If Len(trim(s6)) > 0 Then
     s=s &"  �ӽЦC�L�渹:�]�t('" &s6 & "'�r��) "
     t=t &" AND (RTEBTCMTYLINE.APPLYPRTNO LIKE '%" & S6 & "%') "
  End If       
  '----�D�u�i�ת��p
  s7ary=split(document.all("search7").value,";")  
  If Len(trim(s7ary(0)))=0 Or s7ary(0)="" Then
  Elseif s7ary(0)="0" then
     s=s &"  �i�ת��p:" &s7ary(1)
     t=t &" AND (RTEBTCMTYLINE.INSPECTDAT is NOT null AND RTEBTCMTYLINE.AGREE='Y' AND RTEBTCMTYLINE.UPDEBTCHKDAT is null) "
  Elseif s7ary(0)="1" then
     s=s &"  �i�ת��p:" &s7ary(1)
     t=t &" AND (RTEBTCMTYLINE.UPDEBTCHKDAT is not null) AND RTEBTCMTYLINE.ADSLAPPLYDAT IS NULL "     
  elseif s7ary(0)="2" then
     s=s &"  �i�ת��p:" &s7ary(1)
     t=t &" AND (RTEBTCMTYLINE.UPDEBTCHKDAT  is not null AND RTEBTCMTYLINE.LINEIP <>''  ) AND RTEBTCMTYLINE.ADSLAPPLYDAT IS NULL " 
  elseif s7ary(0)="3" then
     s=s &"  �i�ת��p:" &s7ary(1)
     t=t &" AND (RTEBTCMTYLINE.UPDEBTCHKDAT  is not null AND RTEBTCMTYLINE.LINEIP <>'' AND RTEBTCMTYLINE.LINETEL <>'' ) AND RTEBTCMTYLINE.ADSLAPPLYDAT IS NULL " 
  elseif s7ary(0)="4" then
     s=s &"  �i�ת��p:" &s7ary(1)
     t=t &" AND (RTEBTCMTYLINE.UPDEBTCHKDAT  is not null AND RTEBTCMTYLINE.LINEIP <>''  AND RTEBTCMTYLINE.LINETEL <>'' AND RTEBTCMTYLINE.HINETNOTIFYDAT IS NOT NULL) AND RTEBTCMTYLINE.ADSLAPPLYDAT IS NULL " 
  elseif s7ary(0)="5" then
     s=s &"  �i�ת��p:" &s7ary(1)
     t=t &" AND (RTEBTCMTYLINE.UPDEBTCHKDAT  is not null AND RTEBTCMTYLINE.LINEIP <>''  AND RTEBTCMTYLINE.LINETEL <>'' AND RTEBTCMTYLINE.HINETNOTIFYDAT IS NOT NULL AND RTEBTCMTYLINE.ADSLAPPLYDAT IS NOT NULL) " 
 End If        
  '----�O�_�i�ظm
  s8ary=split(document.all("search8").value,";")  
  If Len(trim(s8ary(0)))=0 Or s8ary(0)="" Then
  Elseif s8ary(0)="Y" then
     s=s &"  �O�_�i�ظm:" &s8ary(1)
     t=t &" AND (RTEBTCMTYLINE.agree='Y') "
  elseif s8ary(0)="N" then
     s=s &"  �O�_�i�ظm:" &s8ary(1)
     t=t &" AND (RTEBTCMTYLINE.agree='N') " 
  elseif s8ary(0)="B" then
      s=s &"  �O�_�i�ظm:" &s8ary(1)
     t=t &" AND (RTEBTCMTYLINE.agree='') "  
  End If      
  '----���ϧǸ�
  s9=document.all("search9").value
  If Len(trim(s9)) > 0 Then
     s=s &"  ���ϧǸ�:'" &s9 & "') "
     t=t &" AND (RTEBTCMTYLINE.COMQ1=" & S9 & ") "
  End If   
  '----�D�u�Ǹ�
  s10=document.all("search10").value
  If Len(trim(s10)) > 0 Then
     s=s &"  �D�u�Ǹ�:'" &s10 & "') "
     t=t &" AND (RTEBTCMTYLINE.LINEQ1=" & S10 & ") "
  End If          
  '----�g�P��
  s11=document.all("search11").value
  S11ARY=SPLIT(S11,";")
  If s11ARY(0) <> "*" Then
     s=s &"  �g�P��:" &s11ARY(1) & " "
     t=t &" AND (RTEBTCMTYLINE.CONSIGNEE='" & S11ARY(0) & "') "
  End If            
  '----�L�ĥD�u
  s12=document.all("search12").value
  S12ARY=SPLIT(S12,";")
  If s12ARY(0) <> "" Then
     s=s &"  �L�ĥD�u:" &s12ARY(1) & " "
     IF S12ARY(0)="1" THEN
        t=t &" AND (RTEBTCMTYLINE.CANCELDAT IS NOT NULL ) "
     ELSEIF S12ARY(0)="2" THEN
        t=t &" AND (RTEBTCMTYLINE.DROPDAT IS NOT NULL ) "
     END IF
  End If              
  
  Dim winP,docP
  Set winP=window.Opener
  Set docP=winP.document
  docP.all("searchQry").value=t
  docP.all("searchShow").value=s
  docP.all("keyform").Submit
  winP.focus()
  window.close
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
<body>
<table width="100%">
  <tr class=dataListTitle align=center>AVS�D�u��Ʒj�M����</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">����/�D�u�Ǹ�</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="5" name="search9" class=dataListEntry ID="Text5"> 
      <font size=2>-</font>
      <input type="text" size="5" name="search10" class=dataListEntry ID="Text6"> 
    </td></tr>        
<tr><td class=dataListHead width="40%">���ϦW��</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search1" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead width="40%">�D�u�˾���}</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="40" name="search2" class=dataListEntry> 
    </td></tr> 
<tr><td class=dataListHead width="40%">�D�uIP</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search3" class=dataListEntry ID="Text1"> 
    </td></tr>    
<tr><td class=dataListHead width="40%">�D�u�p��s��</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="12" name="search4" class=dataListEntry ID="Text3"> 
    </td></tr>        
<tr><td class=dataListHead width="40%">�D�u�����q��</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search5" class=dataListEntry ID="Text2"> 
    </td></tr>        
<tr><td class=dataListHead width="40%">�D�u�ӽЦC�L�渹</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="10" name="search6" class=dataListEntry ID="Text4"> 
    </td></tr>       
<tr><td class=dataListHead width="40%">�D�u�i�ת��p</td>
    <td width="60%"  bgcolor="silver">
      <select name="search7" size="1" class=dataListEntry>
        <option value=";����" selected>����</option>
        <option value="0;�w�ɬd���i�ظm">�w�ɬd���i�ظm</option>
        <option value="1;�w�ӽ�">�w�ӽ�</option>
        <option value="2;�w�ֵoIP">�w�ֵoIP</option>
        <option value="3;�w���o�����q��">�w���o�����q��</option>                
        <option value="4;INET�w�q�����q">HINET�w�q�����q</option>
        <option value="5;�D�u�w���q">�D�u�w���q</option>        
      </select>
     </td>
</tr>
<tr><td class=dataListHead width="40%">�O�_�i�ظm</td>
    <td width="60%"  bgcolor="silver">
      <select name="search8" size="1" class=dataListEntry>
        <option value=";����" selected>����</option>
        <option value="Y;�i�ظm">�i�ظm</option>
        <option value="N;���i�ظm">���i�ظm</option>
        <option value="B;�|���ɹ�">�|���ɹ�</option>
      </select>
     </td>
</tr>
<tr><td class=dataListHead width="40%">�L�ĥD�u</td>
    <td width="60%"  bgcolor="silver">
      <select name="search12" size="1" class=dataListEntry ID="Select2">
        <option value=";�L" selected>�L</option>
        <option value="1;�w�@�o">�w�@�o</option>
        <option value="2;�w�M�P(����)">�w�M�P(����)</option>
      </select>
     </td>
</tr>
<tr><td class=dataListHead width="40%">�g�P��</td>
    <td width="60%"  bgcolor="silver">
    <select name="search11" size="1" class=dataListEntry ID="Select1">
        <%=S11%>
    </select>      
    </td>
</tr>
</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" �d�� " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" ���� " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>