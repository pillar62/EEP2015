<%
 '   Dim rs,i,conn
 '   Dim search1Opt,search2Opt,search6Opt, search12pt
 '   Set conn=Server.CreateObject("ADODB.Connection")
 '   conn.open "DSN=RTLib"
    
'    Set rs=Server.CreateObject("ADODB.Recordset")
'----------主機建置方式
'    S9=""
'    rs.Open "SELECT CODE,CODENC FROM RTCODE WHERE KIND='G4'",CONN
'    s9="<option value="";全部"" selected>全部</option>" &vbCrLf    
'    Do While Not rs.Eof
'       s9=s9 &"<option value=""" &rs("CODE") & ";" & rs("CODENC") & """>" &rs("CODENC") &"</option>"
'       rs.MoveNext
'    Loop
'    rs.Close
'    
'    conn.Close
'    Set rs=Nothing
'    Set conn=Nothing
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  dim aryStr,s,t,r
  '----社區名稱
  S1=document.all("search1").value  
  If Len(s1)=0 Or s1="" Then
     t=t &" (RTCmty.ComN<> '*' )" 
  Else
     s=s &"  社區名稱:包含('" &S1 & "'字元)"
     t=t &" (RTCmty.ComN LIKE '%" &S1 &"%')" 
  End If
  '----主線裝機地址
 ' S2=document.all("search2").value  
 ' If Len(s2)=0 Or s2="" Then
 ' Else
 '    s=s &"  主線裝機位址:包含('" &S2 & "'字元) "
 '    t=t &" AND (RTCounty.CUTNC+RTEBTCmtyLINE.Township+rtEBTcmtyLINE.Raddr LIKE '%" &S2 &"%')" 
 ' End If
  '----主線IP
  s3=document.all("search3").value
  IF LEN(TRIM(S3)) > 0 THEN
     s=s &"  主線IP:包含('" &s3 & "'字元) "
     t=t &" AND (RTCMTY.NETIP LIKE '%" & S3 & "%') "
  END IF
  '----主線聯單編號
 ' s4=document.all("search4").value
 ' If Len(trim(s4)) > 0 Then
 '    s=s &"  聯單編號:包含('" &s4 & "'字元) "
 '    t=t &" AND (RTEBTCMTYLINE.APPLYNO LIKE '%" & S4 & "%') "
 ' End If    
  '----主線附掛電話
 ' s5=document.all("search5").value
 ' If Len(trim(s5)) > 0 Then
 '    s=s &"  附掛電話:包含('" &s5 & "'字元) "
 '    t=t &" AND (RTEBTCMTYLINE.LINETEL LIKE '%" & S5 & "%') "
 ' End If      
  '----主線申請列印單號
 ' s6=document.all("search6").value
 ' If Len(trim(s6)) > 0 Then
 '    s=s &"  申請列印單號:包含('" &s6 & "'字元) "
 '    t=t &" AND (RTEBTCMTYLINE.APPLYPRTNO LIKE '%" & S6 & "%') "
 ' End If       
  '----主線進度狀況
  s7ary=split(document.all("search7").value,";")  
  If Len(trim(s7ary(0)))=0 Or s7ary(0)="" Then
  Elseif s7ary(0)="1" then
     s=s &"  進度狀況:" &s7ary(1)
     t=t &" AND  (RTCMTY.NETIP <> '') AND (RTCMTY.T1APPLY IS NULL) AND (RTCMTY.RCOMDROP IS NULL)  "
  elseif s7ary(0)="2" then
     s=s &"  進度狀況:" &s7ary(1)
     t=t &" AND (T1apply is NOT null) " 
 End If  
 '社區地址      
  s11=document.all("search11").value
  If Len(s11)=0 Or s11="" Then
  Else
     s=s &"  社區地址:包含('" &S11 & "'字元)"
     t=t &" AND (RTCounty.CUTNC + RTCmty.TOWNSHIP + RTCmty.ADDR LIKE '%" &S11 &"%')" 
  End If
  '----社區序號
 ' s9=document.all("search9").value
 ' If Len(trim(s9)) > 0 Then
 '    s=s &"  社區序號:'" &s9 & "') "
 '    t=t &" AND (RTEBTCMTYLINE.COMQ1=" & S9 & ") "
 ' End If   
  '----主線序號
 ' s10=document.all("search10").value
 ' If Len(trim(s10)) > 0 Then
 '    s=s &"  主線序號:'" &s10 & "') "
 '    t=t &" AND (RTEBTCMTYLINE.LINEQ1=" & S10 & ") "
 ' End If          
  
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
  <tr class=dataListTitle align=center>AVS主線資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<!--
<tr><td class=dataListHead width="40%">社區/主線序號</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="5" name="search9" class=dataListEntry ID="Text5"> 
      <font size=2>-</font>
      <input type="text" size="5" name="search10" class=dataListEntry ID="Text6"> 
    </td></tr>        
    -->
<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search1" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead width="40%">社區地址</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="40" name="search11" class=dataListEntry ID="Text2"> 
    </td></tr>    
    <!--
<tr><td class=dataListHead width="40%">主線裝機位址</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="40" name="search2" class=dataListEntry> 
    </td></tr> -->
<tr><td class=dataListHead width="40%">主線IP</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search3" class=dataListEntry ID="Text1"> 
    </td></tr>    
<!--
<tr><td class=dataListHead width="40%">主線聯單編號</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="12" name="search4" class=dataListEntry ID="Text3"> 
    </td></tr>        
<tr><td class=dataListHead width="40%">主線附掛電話</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search5" class=dataListEntry ID="Text2"> 
    </td></tr>        
<tr><td class=dataListHead width="40%">主線申請列印單號</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="10" name="search6" class=dataListEntry ID="Text4"> 
    </td></tr>       
    -->
<tr><td class=dataListHead width="40%">主線進度狀況</td>
    <td width="60%"  bgcolor="silver">
      <select name="search7" size="1" class=dataListEntry>
        <option value=";全部" selected>全部</option>
        <option value="1;取得IP，主線未開通">主線未開通</option>            
        <option value="2;主線已開通">主線已開通</option>        
      </select>
     </td>
</tr>
</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>