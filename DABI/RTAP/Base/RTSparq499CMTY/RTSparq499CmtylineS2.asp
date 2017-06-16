<%
    Dim rs,i,conn
    Dim search1Opt,search2Opt,search6Opt, search12pt
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    
    Set rs=Server.CreateObject("ADODB.Recordset")
'----------經銷商
    S15=""
    rs.Open "SELECT  CASE WHEN RTSparq499CmtyLINE.CONSIGNEE = '' THEN '直銷' ELSE RTOBJ.SHORTNC END AS shortnc " _
           &"FROM  RTSparq499CmtyLINE LEFT OUTER JOIN RTOBJ ON RTSparq499CmtyLINE.CONSIGNEE= RTOBJ.CUSID " _
           &"GROUP BY  CASE WHEN RTSparq499CmtyLINE.CONSIGNEE = '' THEN '直銷' ELSE RTOBJ.SHORTNC END " _
           &"ORDER BY  CASE WHEN RTSparq499CmtyLINE.CONSIGNEE = '' THEN '直銷' ELSE RTOBJ.SHORTNC END",CONN
    s15="<option value=""*"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s15=s15 &"<option value=""" & rs("SHORTNC") & """>" &rs("SHORTNC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'----------工程師
    S14=""
    rs.Open " select a.salesid, c.cusnc from rtsparq499cmtyline a inner join rtemployee b on a.salesid = b.emply inner join rtobj c on c.cusid = b.cusid group by a.salesid, c.cusnc order by 1,2 ",CONN
    s14="<option value=""*"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s14=s14 &"<option value=""" & rs("salesid") & """>" &rs("cusnc") &"</option>"
       rs.MoveNext
    Loop
    's14=s14 &"<option value=""無法歸屬"">無法歸屬</option>"
    rs.Close
'----------連接方式
    S8=""
    rs.Open "SELECT code, codenc FROM rtcode WHERE kind='G5' and code not in ('01','02','03')",CONN
    S8="<option value="""" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s8=s8 &"<option value=""" & rs("code") & """>" &rs("codenc") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
      
'---------------------    
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
  '----社區名稱
  S1=document.all("search1").value  
  If Len(s1)=0 Or s1="" Then
     t=t &" (RTSparq499CmtyH.ComN<> '*' )" 
  Else
     s=s &"  社區名稱:包含('" &S1 & "'字元)"
     t=t &" (RTSparq499CmtyH.ComN LIKE '%" &S1 &"%')" 
  End If
  '----主線裝機地址
  S2=document.all("search2").value  
  If Len(s2)=0 Or s2="" Then
  Else
     s=s &"  主線裝機位址:包含('" &S2 & "'字元) "
     t=t &" AND ((RTCounty.CUTNC + RTSparq499CmtyLine.TOWNSHIP + RTSparq499CmtyLine.raddr ) LIKE '%" &S2 &"%') " 
  End If
  '----主線IP
  s3=document.all("search3").value
  IF LEN(TRIM(S3)) > 0 THEN
     s=s &"  主線IP:包含('" &s3 & "'字元) "
     t=t &" AND (convert(varchar(3),RTSparq499CmtyLine.LINEIPSTR1)+'.'+convert(varchar(3),RTSparq499CmtyLine.LINEIPSTR2)+'.'+convert(varchar(3),RTSparq499CmtyLine.LINEIPSTR3)+'.'+convert(varchar(3),RTSparq499CmtyLine.LINEIPSTR4) LIKE '%" & S3 & "%') "
  END IF
  '----主線聯單編號
  s4=document.all("search4").value
  If Len(trim(s4)) > 0 Then
     s=s &"  聯單編號:包含('" &s4 & "'字元) "
     t=t &" AND (RTSparq499CmtyLine.CHTWORKINGNO LIKE '%" & S4 & "%') "
  End If    
  '----主線附掛電話
  s5=document.all("search5").value
  If Len(trim(s5)) > 0 Then
     s=s &"  附掛電話:包含('" &s5 & "'字元) "
     t=t &" AND (RTSparq499CmtyLine.LINETEL LIKE '%" & S5 & "%') "
  End If      
  '----主線進度狀況
  s7ary=split(document.all("search7").value,";")  
  If Len(trim(s7ary(0)))=0 Or s7ary(0)="" Then
  Elseif s7ary(0)="0" then
  '已勘察為可建置(勘察日<>空白 and 申請日=空白)
     s=s &"  進度狀況:" &s7ary(1)
     t=t &" AND (RTSparq499CmtyLine.INSPECTDAT is NOT null AND RTSparq499CmtyLine.AGREE='Y' AND RTSparq499CmtyLine.adslapplyDAT is null) "
  Elseif s7ary(0)="1" then
  '已申請(申請日<>空白 and ip =空白 )
     s=s &"  進度狀況:" &s7ary(1)
     t=t &" AND (RTSparq499CmtyLine.adslapplyDAT is not null) AND RTSparq499CmtyLine.LINEIPSTR1='' "     
  elseif s7ary(0)="2" then
  '已核發ip(ip <>空白 and 附掛=空白)
     s=s &"  進度狀況:" &s7ary(1)
     t=t &" AND (RTSparq499CmtyLine.LINEIPSTR1 <> '' AND RTSparq499CmtyLine.LINEtel =''  )  " 
  elseif s7ary(0)="3" then
  '已取得附掛(附掛<>空白 and 測通日=空白)
     s=s &"  進度狀況:" &s7ary(1)
     t=t &" AND (RTSparq499CmtyLine.LINEtel <> '' AND RTSparq499CmtyLine.adslopendat IS NULL ) " 
  elseif s7ary(0)="4" then
  '主線已測通(adslopendat <> 空白 and 退租日 = 空白 and 作廢日 = 空白)
     s=s &"  進度狀況:" &s7ary(1)
     t=t &" AND (RTSparq499CmtyLine.adslopendat  is not null AND RTSparq499CmtyLine.dropdat is null ) " 
  elseif s7ary(0)="5" then
  '主線已退租(adslopendat <> 空白 and 退租日 <> 空白 )
     s=s &"  進度狀況:" &s7ary(1)
     t=t &" AND (RTSparq499CmtyLine.adslopendat  is not null AND RTSparq499CmtyLine.dropdat is not null ) "      
  elseif s7ary(0)="6" then
  '主線已作廢(作廢日 <> 空白 )
     s=s &"  進度狀況:" &s7ary(1)
     t=t &" AND (RTSparq499CmtyLine.canceldat  is not null ) "           
 End If        
  '----連接方式
  s8=document.all("search8").value
  If Len(trim(s8)) > 0 Then
     s=s &"  連接方式:" &s8 & " "
     t=t &" AND (RTSparq499CmtyLine.connecttype='" & S8 & "') "
  End If      
  '----社區序號
  s9=document.all("search9").value
  If Len(trim(s9)) > 0 Then
     s=s &"  社區序號:'" &s9 & "') "
     t=t &" AND (RTSparq499CmtyLine.COMQ1=" & S9 & ") "
  End If   
  '----主線序號
  s10=document.all("search10").value
  If Len(trim(s10)) > 0 Then
     s=s &"  主線序號:'" &s10 & "') "
     t=t &" AND (RTSparq499CmtyLine.LINEQ1=" & S10 & ") "
  End If          
  
  s14=document.all("search14").value
  if S14 <> "*" then
       s=s &"  工程師:'" &s14 & "') "
     t=t &" AND (RTSparq499CmtyLINE.salesid='" & S14 & "') "
  end if
  s15=document.all("search15").value
  s=S & "經銷商:" &S15 &"  "
  if S15 <> "*" AND S15 <> "直銷" then
     t=t &" AND (rtobj.shortnc='" & S15 & "') "
  ELSEIF S15="直銷" THEN 
     t=t &" AND (RTSparq499CmtyLINE.consignee='') "
  end if  
  
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
  <tr class=dataListTitle align=center>速博499主線資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">工程師</td>
    <td width="60%" bgcolor="silver">
      <select name="search14" size="1" class=dataListEntry ID="Select1">
        <%=S14%>
    </select>      
    </td></tr>        
<tr><td class=dataListHead width="40%">經銷商</td>
    <td width="60%"  bgcolor="silver">
    <select name="search15" size="1" class=dataListEntry ID="Select1">
        <%=S15%>
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
<tr><td class=dataListHead width="40%">主線聯單編號</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="12" name="search4" class=dataListEntry ID="Text3"> 
    </td></tr>        
<tr><td class=dataListHead width="40%">主線附掛電話</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search5" class=dataListEntry ID="Text2"> 
    </td></tr>        
<tr><td class=dataListHead width="40%">主線進度狀況</td>
    <td width="60%"  bgcolor="silver">
      <select name="search7" size="1" class=dataListEntry>
        <option value=";全部" selected>全部</option>
        <option value="0;已勘查為可建置">已勘查為可建置</option>
        <option value="1;已申請">已申請</option>
        <option value="2;已核發IP">已核發IP</option>
        <option value="3;已取得附掛電話">已取得附掛電話</option>                
        <option value="4;主線已測通">主線已測通</option>    
        <option value="5;主線已退租">主線已退租</option>  
        <option value="6;主線已作廢">主線已作廢</option>      
      </select>
     </td>
</tr>
<tr><td class=dataListHead width="40%">連接方式</td>
    <td width="60%"  bgcolor="silver">
      <select name="search8" size="1" class=dataListEntry>
        <%=S8%>
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