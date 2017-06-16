<%
    Dim rs,i,conn
    Dim search1Opt,search2Opt,search6Opt, search12pt
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    
    Set rs=Server.CreateObject("ADODB.Recordset")
'----------主機建置方式
    S19=""
    rs.Open "SELECT CODE,CODENC FROM RTCODE WHERE KIND='H8'",CONN
    s19="<option value="";全部"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s19=s19 &"<option value=""" &rs("CODE") & ";" & rs("CODENC") & """>" &rs("CODENC") &"</option>"
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
<script language="VBScript">
<!--
Sub btn_onClick()
  dim aryStr,s,t,r
  '----社區名稱
  S1=document.all("search1").value  
  If Len(s1)=0 Or s1="" Then
     t=t &" and (RTSparq499CmtyH.ComN<> '*' )" 
  Else
     s=s &"  社區名稱︰包含('" &S1 & "'字元)"
     t=t &" and (RTSparq499CmtyH.ComN LIKE '%" &S1 &"%')" 
  End If
  '----主線裝機地址
 ' S2=document.all("search2").value  
 ' If Len(s2)=0 Or s2="" Then
 ' Else
 '    s=s &"  主線裝機位址:包含('" &S2 & "'字元) "
 '    t=t &" AND (RTCounty.CUTNC+RTSparq499CmtyLine.Township+RTSparq499CmtyLine.Raddr LIKE '%" &S2 &"%')" 
 ' End If
  '----主線IP
  s3=document.all("search3").value
  IF LEN(TRIM(S3)) > 0 THEN
     s=s &"  主線IP︰包含('" &s3 & "'字元) "
     t=t &" AND (RTSparq499CmtyLine.LINEIPSTR1+'.'+RTSparq499CmtyLine.LINEIPSTR2+'.'+RTSparq499CmtyLine.LINEIPSTR3+'.'+RTSparq499CmtyLine.LINEIPSTR4 LIKE '%" & S3 & "%') "
  END IF
  '----主線聯單編號
  s4=document.all("search4").value
  If Len(trim(s4)) > 0 Then
     s=s &"  聯單編號︰包含('" &s4 & "'字元) "
     t=t &" AND (RTSparq499CmtyLine.CHTWORKINGNO LIKE '%" & S4 & "%') "
  End If    
  '----主線附掛電話
  s5=document.all("search5").value
  If Len(trim(s5)) > 0 Then
     s=s &"  附掛電話︰包含('" &s5 & "'字元) "
     t=t &" AND (RTSparq499CmtyLine.LINETEL LIKE '%" & S5 & "%') "
  End If      
  '----主線進度狀況
  s7ary=split(document.all("search7").value,";")  
  If Len(trim(s7ary(0)))=0 Or s7ary(0)="" Then
  Elseif s7ary(0)="1" then
  '已勘察為可建置(勘察日<>空白 and 申請日=空白)
     s=s &"  進度狀況︰" &s7ary(1)
     t=t &" AND (RTSparq499CmtyLine.INSPECTDAT is NOT null AND RTSparq499CmtyLine.AGREE='Y' AND RTSparq499CmtyLine.adslapplyDAT is null) "
  Elseif s7ary(0)="1" then
  '已申請(申請日<>空白 and ip =空白 )
     s=s &"  進度狀況︰" &s7ary(1)
     t=t &" AND (RTSparq499CmtyLine.adslapplyDAT is not null) AND RTSparq499CmtyLine.LINEIPSTR1='' "     
  elseif s7ary(0)="2" then
  '已核發ip(ip <>空白 and 附掛=空白)
     s=s &"  進度狀況︰" &s7ary(1)
     t=t &" AND (RTSparq499CmtyLine.LINEIPSTR1 <> '' AND RTSparq499CmtyLine.LINEtel =''  )  " 
  elseif s7ary(0)="3" then
  '已取得附掛(附掛<>空白 and 測通日=空白)
     s=s &"  進度狀況︰" &s7ary(1)
     t=t &" AND (RTSparq499CmtyLine.LINEtel <> '' AND RTSparq499CmtyLine.adslopendat IS NULL ) " 
  elseif s7ary(0)="4" then
  '主線已測通(adslopendat <> 空白 and 退租日 = 空白 and 作廢日 = 空白)
     s=s &"  進度狀況︰" &s7ary(1)
     t=t &" AND (RTSparq499CmtyLine.adslopendat  is not null AND RTSparq499CmtyLine.dropdat is null ) " 
  elseif s7ary(0)="5" then
  '主線已退租(adslopendat <> 空白 and 退租日 <> 空白 )
     s=s &"  進度狀況︰" &s7ary(1)
     t=t &" AND (RTSparq499CmtyLine.adslopendat  is not null AND RTSparq499CmtyLine.dropdat is not null ) "      
  elseif s7ary(0)="6" then
  '主線已作廢(作廢日 <> 空白 )
     s=s &"  進度狀況︰" &s7ary(1)
     t=t &" AND (RTSparq499CmtyLine.canceldat  is not null ) "           
 End If        
  '----是否可建置
  s8ary=split(document.all("search8").value,";")  
  If Len(trim(s8ary(0)))=0 Or s8ary(0)="" Then
  Elseif s8ary(0)="Y" then
     s=s &"  是否可建置︰" &s8ary(1)
     t=t &" AND (RTSparq499CmtyLine.agree='Y') "
  elseif s8ary(0)="N" then
     s=s &"  是否可建置︰" &s8ary(1)
     t=t &" AND (RTSparq499CmtyLine.agree='N') " 
  elseif s8ary(0)="B" then
      s=s &"  是否可建置︰" &s8ary(1)
     t=t &" AND (RTSparq499CmtyLine.agree='') "  
  End If      
  '用戶名稱
  s9=document.all("search9").value 
  if  Len(trim(s9))=0 Or s9="" then
  else
     s=s & " 用戶名稱︰包含('" & s9 & "')字元 "
     t=t & " and (RTSparq499Cust.cusnc like '%" & s9 & "%') "
  end if
  '----avs繳款方式
  s11ary=split(document.all("search11").value,";")  
  If Len(trim(s11ary(0)))=0 Or s11ary(0)="" Then
  Else
     s=s &"  繳款方式︰" &s11ary(1)
     t=t &" AND (RTSparq499Cust.paytype='" & s11ary(0) & "') "
  End If 
  '聯絡電話
  s12=document.all("search12").value 
  if  Len(trim(s12))=0 Or s12="" then
  else
     s=s & " 聯絡電話︰包含('" & s12 & "')字元 "
     t=t & " and (RTSparq499Cust.contacttel + RTSparq499Cust.mobile like '%" & s12 & "%') "
  end if   
  '身分證/統編
  s13=document.all("search13").value 
  if  Len(trim(s13))=0 Or s13="" then
  else
     s=s & " 身份證/統編︰包含('" & s13 & "')字元 "
     t=t & " and (RTSparq499Cust.socialid like '%" & s13 & "%') "
  end if     
  '----社區序號
  s16=document.all("search16").value
  If Len(trim(s16)) > 0 Then
     s=s &"  社區序號︰('" &s16 & "') "
     t=t &" AND (RTSparq499Cust.COMQ1=" & S16 & ") "
  End If   
  '----主線序號
  s17=document.all("search17").value
  If Len(trim(s17)) > 0 Then
     s=s &"  主線序號︰('" &s17 & "') "
     t=t &" AND (RTSparq499Cust.LINEQ1=" & S17 & ") "
  End If            
  '----用戶進度狀況
  s18ary=split(document.all("search18").value,";")  
  If Len(trim(s18ARY(0))) = 0 Then
  '尚未取得IP(未作廢及退租)
  ELSEIF s18ARY(0) = "1" THEN
     s=s &"  用戶進度︰('" &s18ARY(1) & "') "
     t=t &" AND (RTSparq499Cust.CUSTIP1 ='' ) AND RTSparq499Cust.CANCELDAT IS NULL AND RTSparq499Cust.DROPDAT IS NULL "
  '已取得IP，尚未完工
  ELSEIF s18ARY(0) = "2" THEN
     s=s &"  用戶進度︰('" &s18ARY(1) & "') "
     t=t &" AND (RTSparq499Cust.CUSTIP1 <>'' ) AND RTSparq499Cust.FINISHDAT IS NULL "     
  '已完工，尚未報竣
  ELSEIF s18ARY(0) = "3" THEN
     s=s &"  用戶進度︰('" &s18ARY(1) & "') "
     t=t &" AND RTSparq499Cust.FINISHDAT IS NOT NULL AND  RTSparq499Cust.DOCKETDAT IS NULL "      
  '已報竣，尚未轉檔
  ELSEIF s18ARY(0) = "4" THEN
     s=s &"  用戶進度︰('" &s18ARY(1) & "') "
     t=t &" AND RTSparq499Cust.DOCKETDAT IS NOT NULL AND  RTSparq499Cust.TRANSDAT IS NULL "               
  '已退租
  ELSEIF s18ARY(0) = "5" THEN
     s=s &"  用戶進度︰('" &s18ARY(1) & "') "
     t=t &" AND  RTSparq499Cust.dropdat IS NOT NULL AND RTSparq499Cust.docketdat IS not NULL "       
  '已作廢
  ELSEIF s18ARY(0) = "6" THEN
     s=s &"  用戶進度︰('" &s18ARY(1) & "') "
     t=t &" AND  RTSparq499Cust.CANCELDAT IS NOT NULL "                 
  End If              
  '----用戶IP
  s19=document.all("search19").value
  If Len(trim(s19)) > 0 Then
     s=s &"  用戶IP︰包含('" &s19 & "') "
     t=t &" AND (RTSparq499Cust.CUSTIP1+'.'+RTSparq499Cust.CUSTIP2+'.'+RTSparq499Cust.CUSTIP3+'.'+RTSparq499Cust.CUSTIP4 LIKE '%" & S19 & "%') "
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
  <tr class=dataListTitle align=center>AVS用戶資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">社區/主線序號</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="5" name="search16" class=dataListEntry ID="Text5"> 
      <font size=2>-</font>
      <input type="text" size="5" name="search17" class=dataListEntry ID="Text6"> 
    </td></tr>  
<tr><td class=dataListHead width="40%">用戶名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search9" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead width="40%">用戶IP</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="15" name="search19"  class=dataListEntry ID="Text4"> 
    </td></tr>
<tr><td class=dataListHead width="40%">用戶進度狀況</td>
    <td width="60%"  bgcolor="silver">
      <select name="search18" size="1" class=dataListEntry ID="Select1">
        <option value=";全部" selected>全部</option>
        <option value="1;尚未取得IP">尚未取得IP</option>
        <option value="2;已取得IP，尚未完工">已取得IP，尚未完工</option>
        <option value="3;已完工，尚未報竣">已完工，尚未報竣</option>
        <option value="4;已報竣，尚未轉檔">已報竣，未轉檔</option>      
        <option value="5;已退租">已退租</option>      
        <option value="6;已作廢">已作廢</option>                     
      </select>
     </td>
</tr>    
<tr><td class=dataListHead width="40%">繳款方式</td>
    <td width="60%" bgcolor="silver">
     <select name="search11" size="1" class=dataListEntry>
        <option value=";全部" selected>全部</option>
        <option value="01;年約年繳，免設定費及租費九折">年約年繳，免設定費及租費九折</option>
        <option value="02;年約月繳，免設定費">年約月繳，免設定費</option>
        <option value="03;一般月繳">一般月繳</option>                
     </select>
    </td></tr>        
<tr><td class=dataListHead width="40%">聯絡電話(或行動)</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="10" name="search12" class=dataListEntry> 
    </td></tr>   
<tr><td class=dataListHead width="40%">用戶身份證/統編</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="10" name="search13" class=dataListEntry> 
    </td></tr>          
<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search1" class=dataListEntry> 
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
</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>