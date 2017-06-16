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
     t=t &" (RTEBTCmtyH.ComN<> '*' )" 
  Else
     s=s &"  社區名稱:包含('" &S1 & "'字元)"
     t=t &" (RTEBTCmtyH.ComN LIKE '%" &S1 &"%')" 
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
     t=t &" AND (RTEBTCMTYLINE.LINEIP LIKE '%" & S3 & "%') "
  END IF
  '----主線聯單編號
  s4=document.all("search4").value
  If Len(trim(s4)) > 0 Then
     s=s &"  聯單編號:包含('" &s4 & "'字元) "
     t=t &" AND (RTEBTCMTYLINE.APPLYNO LIKE '%" & S4 & "%') "
  End If    
  '----主線附掛電話
  s5=document.all("search5").value
  If Len(trim(s5)) > 0 Then
     s=s &"  附掛電話:包含('" &s5 & "'字元) "
     t=t &" AND (RTEBTCMTYLINE.LINETEL LIKE '%" & S5 & "%') "
  End If      
  '----主線申請列印單號
  s6=document.all("search6").value
  If Len(trim(s6)) > 0 Then
     s=s &"  申請列印單號:包含('" &s6 & "'字元) "
     t=t &" AND (RTEBTCMTYLINE.APPLYPRTNO LIKE '%" & S6 & "%') "
  End If       
  '----主線進度狀況
  s7ary=split(document.all("search7").value,";")  
  If Len(trim(s7ary(0)))=0 Or s7ary(0)="" Then
  Elseif s7ary(0)="1" then
     s=s &"  進度狀況:" &s7ary(1)
     t=t &" AND (RTEBTCMTYLINE.UPDEBTCHKDAT is not null) "
  elseif s7ary(0)="2" then
     s=s &"  進度狀況:" &s7ary(1)
     t=t &" AND (RTEBTCMTYLINE.UPDEBTCHKDAT  is not null AND RTEBTCMTYLINE.LINEIP <>'' AND RTEBTCMTYLINE.LINETEL <>'' ) " 
  elseif s7ary(0)="3" then
     s=s &"  進度狀況:" &s7ary(1)
     t=t &" AND (RTEBTCMTYLINE.UPDEBTCHKDAT  is not null AND RTEBTCMTYLINE.LINEIP <>'' AND RTEBTCMTYLINE.LINETEL <>'' ) " 
  elseif s7ary(0)="4" then
     s=s &"  進度狀況:" &s7ary(1)
     t=t &" AND (RTEBTCMTYLINE.UPDEBTCHKDAT  is not null AND RTEBTCMTYLINE.LINEIP <>'' AND RTEBTCMTYLINE.LINETEL <>'' AND RTEBTCMTYLINE.HINETNOTIFYDAT IS NOT NULL) " 
  elseif s7ary(0)="5" then
     s=s &"  進度狀況:" &s7ary(1)
     t=t &" AND (RTEBTCMTYLINE.UPDEBTCHKDAT  is not null AND RTEBTCMTYLINE.LINEIP <>'' AND RTEBTCMTYLINE.LINETEL <>'' AND RTEBTCMTYLINE.HINETNOTIFYDAT IS NOT NULL AND RTEBTCMTYLINE.ADSLAPPLYDAT IS NOT NULL) " 
 End If        
  '----是否可建置
  s8ary=split(document.all("search8").value,";")  
  If Len(trim(s8ary(0)))=0 Or s8ary(0)="" Then
  Elseif s8ary(0)="Y" then
     s=s &"  是否可建置:" &s8ary(1)
     t=t &" AND (RTEBTCMTYLINE.agree='Y') "
  elseif s8ary(0)="N" then
     s=s &"  是否可建置:" &s8ary(1)
     t=t &" AND (RTEBTCMTYLINE.agree='N') " 
  elseif s8ary(0)="B" then
      s=s &"  是否可建置:" &s8ary(1)
     t=t &" AND (RTEBTCMTYLINE.agree='') "  
  End If      
  '用戶名稱
  s9=document.all("search9").value 
  if  Len(trim(s9))=0 Or s9="" then
  else
     s=s & " 用戶名稱︰包含('" & s9 & "')字元 "
     t=t & " and (rtebtcust.cusnc like '%" & s9 & "%') "
  end if
  '用戶合約編號
  s10=document.all("search10").value 
  if  Len(trim(s10))=0 Or s10="" then
  else
     s=s & " 合約編號︰包含('" & s10 & "')字元 "
     t=t & " and (rtebtcust.cusid like '%" & s10 & "%') "
  end if  
  '----avs繳款方式
  s11ary=split(document.all("search11").value,";")  
  If Len(trim(s11ary(0)))=0 Or s11ary(0)="" Then
  Elseif s11ary(0)="1" then
     s=s &"  繳款方式:" &s11ary(1)
     t=t &" AND (RTEBTCust.paytype='Y') "
  elseif s11ary(0)="2" then
     s=s &"  繳款方式:" &s11ary(1)
     t=t &" AND (RTEBTCust.paytype='H') " 
  elseif s11ary(0)="3" then
     s=s &"  繳款方式:" &s11ary(1)
     t=t &" AND (RTEBTCust.paytype='M') "  
  End If 
  '聯絡電話
  s12=document.all("search12").value 
  if  Len(trim(s12))=0 Or s12="" then
  else
     s=s & " 聯絡電話︰包含('" & s12 & "')字元 "
  '   t=t & " and (rtebtcust.contacttel + rtebtcust.mobile like '%" & s12 & "%') "
  end if   
  '身分證/統編
  s13=document.all("search13").value 
  if  Len(trim(s13))=0 Or s13="" then
  else
     s=s & " 身份證/統編︰包含('" & s13 & "')字元 "
     t=t & " and (rtebtcust.socialid like '%" & s13 & "%') "
  end if     
  '舊服務到期日
  s14=document.all("search14").value 
  s15=document.all("search15").value   
  if  (Len(trim(s14))=0 Or s14="") and (Len(trim(s15))=0 Or s15="") then
  else
     if  (Len(trim(s14))=0 Or s14="") then s14="1900/01/01"
     if  (Len(trim(s15))=0 Or s15="") then s15="9999/12/31"
     s=s & " 舊服務起迄日︰自('" & s14 & "') 至 ('" & s15 & "') "
     t=t & " and rtebtcust.oldservicecutdat >= '" & s14 & " 00:00.000' and rtebtcust.oldservicecutdat  <= '" & s15 & " 23:59.997' "
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
<tr><td class=dataListHead width="40%">用戶名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search9" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead width="40%">用戶合約編號</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search10" class=dataListEntry> 
    </td></tr>  
<tr><td class=dataListHead width="40%">AVS繳款方式</td>
    <td width="60%" bgcolor="silver">
     <select name="search11" size="1" class=dataListEntry>
        <option value=";全部" selected>全部</option>
        <option value="1;年約年繳">年約年繳</option>
        <option value="2;年約月繳">年約月繳</option>
        <option value="3;月繳(不簽約)">月繳(不簽約)</option>                
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
<tr><td class=dataListHead width="40%">舊服務到期日</td>
    <td width="60%" bgcolor="silver"><font size=2>自</font>
      <input type="text" size="10" name="search14" class=dataListEntry> 
<input type="button" id="B14"  name="B14" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">      
      <font size=2>至</font>
      <input type="text" size="10" name="search15" class=dataListEntry> 
<input type="button" id="B15"  name="B15" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">            
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
<tr><td class=dataListHead width="40%">主線申請列印單號</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="10" name="search6" class=dataListEntry ID="Text4"> 
    </td></tr>       
<tr><td class=dataListHead width="40%">主線進度狀況</td>
    <td width="60%"  bgcolor="silver">
      <select name="search7" size="1" class=dataListEntry>
        <option value=";全部" selected>全部</option>
        <option value="1;已申請">已申請</option>
        <option value="2;已核發IP">已核發IP</option>
        <option value="3;已取得附掛電話">已取得附掛電話</option>                
        <option value="4;INET已通知測通">HINET已通知測通</option>
        <option value="5;主線已測通">主線已測通</option>      
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