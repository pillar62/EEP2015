<%
    Dim rs,i,conn
    Dim search1Opt,search2Opt,search6Opt, search12pt
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    
    Set rs=Server.CreateObject("ADODB.Recordset")
'----------經銷商
    S11=""
    rs.Open "SELECT  a.CONSIGNEE,  CASE WHEN a.CONSIGNEE = '' THEN '直銷' ELSE RTObj.SHORTNC  END as shortnc FROM RTSonetCmtyH a LEFT OUTER JOIN RTObj ON a.CONSIGNEE = RTObj.CUSID GROUP BY  a.CONSIGNEE,  CASE WHEN a.CONSIGNEE = '' THEN '直銷' ELSE RTObj.SHORTNC  END ",CONN
    s11="<option value=""*;全部"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s11=s11 &"<option value=""" &rs("CONSIGNEE") & ";" & rs("SHORTNC") & """>" &rs("SHORTNC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'----------業務轄區
    S14=""
	sqlstr="select  a.salesid, c.cusnc " &_
		   "from RTSonetCmtyH a " &_
		   "inner join RTEmployee b on a.salesid = b.emply " &_
		   "inner join RTObj c on c.CUSID = b.CUSID " &_
		   "group by a.salesid, c.cusnc " 
    rs.Open sqlstr,CONN
    S14="<option value=""*;全部"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       S14=S14 &"<option value=""" &rs("salesid") & ";" & rs("CUSNC") & """>" &rs("CUSNC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close

    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<meta http-equiv="Content-Type" content="text/html; charset=big5" />
<script language="VBScript">
<!--
Sub btn_onClick()
  dim aryStr,s,t,r
  '----社區名稱
  S1=document.all("search1").value  
  If Len(s1)=0 Or s1="" Then
     t=t &" (c.ComN<> '*' )" 
  Else
     s=s &"  社區名稱:包含('" &S1 & "'字元)"
     t=t &" c.ComN LIKE '%" &S1 &"%' " 
  End If
  '----主線裝機地址
  S2=document.all("search2").value  
  If Len(s2)=0 Or s2="" Then
  Else
     s=s &"  主線裝機位址:包含('" &S2 & "'字元) "
     t=t &" AND g.CUTNC + a.TOWNSHIP + a.RADDR LIKE '%" &S2& "%' " 
  End If
  '----主線IP
  s3=document.all("search3").value
  IF LEN(TRIM(S3)) > 0 THEN
     s=s &"  主線IP:包含('" &s3 & "'字元) "
     t=t &" AND a.LINEIP LIKE '%"& S3 &"%' "
  END IF
  '----PPPoE撥接帳號
  s4=document.all("search4").value
  If Len(trim(s4)) > 0 Then
     s=s &"  PPPoE撥接帳號:包含('" &s4 & "'字元) "
     t=t &" AND a.PPPoEAccount LIKE '%"& S4 &"%' "
  end if
  '----主線附掛電話
  s5=document.all("search5").value
  If Len(trim(s5)) > 0 Then
     s=s &"  專線編號:包含('" &s5 & "'字元) "
     t=t &" AND a.LINETEL LIKE '%" & S5 & "%' "
  end if
  '----主線進度狀況
  s7ary=split(document.all("search7").value,";")
  If Len(trim(s7ary(0))) >0 Then t=t &" AND a.dropdat is null AND a.canceldat is null "
  if s7ary(0)="0" then
     s=s &"  進度狀況:" &s7ary(1)
     t=t &" AND a.INSPECTDAT is NOT null AND a.AGREE='Y' and a.HARDWAREDAT is null AND a.applydat is null "
  Elseif s7ary(0)="1" then
     s=s &"  進度狀況:" &s7ary(1)
     t=t &" AND a.applydat is not null and a.HARDWAREDAT is null "
  elseif s7ary(0)="2" then
     s=s &"  進度狀況:" &s7ary(1)
     t=t &" AND a.HARDWAREDAT is not null "
  End If        
  '----是否可建置
  s8ary=split(document.all("search8").value,";")  
  If Len(trim(s8ary(0)))=0 Or s8ary(0)="" Then
  Elseif s8ary(0)="Y" then
     s=s &"  是否可建置:" &s8ary(1)
     t=t &" AND (a.agree='Y') "
  elseif s8ary(0)="N" then
     s=s &"  是否可建置:" &s8ary(1)
     t=t &" AND (a.agree='N') " 
  elseif s8ary(0)="B" then
      s=s &"  是否可建置:" &s8ary(1)
     t=t &" AND (a.agree='') "  
  End If      
  '----社區序號
  s9=document.all("search9").value
  If Len(trim(s9)) > 0 Then
     s=s &"  社區序號:'" &s9 & "') "
     t=t &" AND (a.COMQ1=" & S9 & ") "
  End If   
  '----主線序號
  s10=document.all("search10").value
  If Len(trim(s10)) > 0 Then
     s=s &"  主線序號:'" &s10 & "') "
     t=t &" AND (a.LINEQ1=" & S10 & ") "
  End If          
  '----經銷商
  s11=document.all("search11").value
  S11ARY=SPLIT(S11,";")
  If s11ARY(0) <> "*" Then
     s=s &"  經銷商:" &s11ARY(1) & " "
     t=t &" AND (c.CONSIGNEE='" & S11ARY(0) & "') "
  End If            
  '---- 業務轄區
  s14=document.all("search14").value
  S14ARY=SPLIT(S14,";")
  If s14ARY(0) <> "*" Then
     s=s &"  業務轄區:" &s14ARY(1) & " "
     t=t &" AND (c.SALESID='" & S14ARY(0) & "') "
  End If            
  '----無效主線
  s12=document.all("search12").value
  S12ARY=SPLIT(S12,";")
  If s12ARY(0) <> "" Then
     s=s &"  無效主線:" &s12ARY(1) & " "
     IF S12ARY(0)="1" THEN
        t=t &" AND (a.CANCELDAT IS NOT NULL ) "
     ELSEIF S12ARY(0)="2" THEN
        t=t &" AND (a.DROPDAT IS NOT NULL ) "
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
  <tr class=dataListTitle align=center>So-net主線資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>

<tr><td class=dataListHead width="40%">業務轄區</td>
    <td width="60%"  bgcolor="silver">
    <select name="search14" size="1" class=dataListEntry ID="Select14">
        <%=S14%>
    </select>      
    </td>
</tr>    
<tr><td class=dataListHead width="40%">經銷商</td>
    <td width="60%"  bgcolor="silver">
    <select name="search11" size="1" class=dataListEntry ID="Select1">
        <%=S11%>
    </select>      
    </td>
</tr>    
<tr><td class=dataListHead width="40%">社區/主線序號</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="5" name="search9" class=dataListEntry ID="Text5"> 
      <font size=2>-</font>
      <input type="text" size="5" name="search10" class=dataListEntry ID="Text6"> 
    </td></tr>        
<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search1" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead width="40%">主線裝機位址</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="40" name="search2" class=dataListEntry> 
    </td></tr> 
<tr><td class=dataListHead width="40%">主線IP</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search3" class=dataListEntry ID="Text1"> 
    </td></tr>    
<tr><td class=dataListHead width="40%">主線專線編號</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search5" class=dataListEntry ID="Text2"> 
    </td></tr>
<tr><td class=dataListHead width="40%">PPPoE撥接帳號</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="12" name="search4" class=dataListEntry ID="Text2"> 
    </td></tr>
<tr><td class=dataListHead width="40%">主線進度狀況</td>
    <td width="60%"  bgcolor="silver">
      <select name="search7" size="1" class=dataListEntry>
        <option value=";全部" selected>全部</option>
        <option value="0;已勘查為可建置">已勘查為可建置</option>
        <option value="1;主線已申請">主線已申請</option>
        <option value="2;主線到位">主線已到位</option>
      </select>
     </td>
</tr>
<tr><td class=dataListHead width="40%">是否可建置</td>
    <td width="60%"  bgcolor="silver">
      <select name="search8" size="1" class=dataListEntry>
        <option value=";全部" selected>全部</option>
        <option value="Y;可建置">可建置</option>
        <option value="N;不可建置">不可建置</option>
        <option value="B;尚未勘察">尚未勘察</option>
      </select>
     </td>
</tr>
<tr><td class=dataListHead width="40%">無效主線</td>
    <td width="60%"  bgcolor="silver">
      <select name="search12" size="1" class=dataListEntry ID="Select2">
        <option value=";無" selected>無</option>
        <option value="1;已作廢">已作廢</option>
        <option value="2;已撤線">已撤線</option>
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