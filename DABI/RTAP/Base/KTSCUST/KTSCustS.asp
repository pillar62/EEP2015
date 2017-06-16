<%
    Dim rs,i,conn
    Dim search1Opt,search2Opt,search6Opt, search12pt
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    
    Set rs=Server.CreateObject("ADODB.Recordset")
'----------大盤經銷商
    S6=""
    SQLXX="SELECT RTConsignee.CUSID, RTObj.SHORTNC FROM RTConsignee INNER JOIN  RTConsigneeISP ON " _
          &"RTConsignee.CUSID = RTConsigneeISP.CUSID INNER JOIN RTObj ON RTConsignee.CUSID = RTObj.CUSID " _
          &"WHERE (RTConsigneeISP.ISP = '03') ORDER BY  RTObj.SHORTNC "
    rs.Open SQLXX,CONN
    s6="<option value="";全部"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s6=s6 &"<option value=""" &rs("CUSID") & ";" & rs("SHORTNC") & """>" &rs("SHORTNC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'-----------開發業務
    S7=""
    SQLXX="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
          &"WHERE (RTEmployee.DEPT IN ('B100', 'B106', 'B107', 'B200', 'B300', 'B401','B600')) AND (RTEmployee.TRAN2 <> '10') AND " _
          &"(RTEmployee.AUTHLEVEL = '2') ORDER BY  RTObj.CUSNC "
    rs.Open SQLXX,CONN
    s7="<option value="";全部"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s7=s7 &"<option value=""" &rs("EMPLY") & ";" & rs("CUSNC") & """>" &rs("CUSNC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close    
'----------經銷商
    S15=""
    rs.Open "SELECT  CASE WHEN KTSCUST.CONSIGNEE1 = '' THEN '直銷' ELSE RTOBJ.SHORTNC END AS shortnc " _
           &"FROM  KTSCUST LEFT OUTER JOIN RTOBJ ON KTSCUST.CONSIGNEE1= RTOBJ.CUSID " _
           &"GROUP BY  CASE WHEN KTSCUST.CONSIGNEE1 = '' THEN '直銷' ELSE RTOBJ.SHORTNC END " _
           &"ORDER BY  CASE WHEN KTSCUST.CONSIGNEE1 = '' THEN '直銷' ELSE RTOBJ.SHORTNC END",CONN
    s15="<option value=""*"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s15=s15 &"<option value=""" & rs("SHORTNC") & """>" &rs("SHORTNC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'----------營運點
    S14=""
    rs.Open "SELECT OPERATIONID, OPERATIONNAME FROM RTCtyTown WHERE (OPERATIONNAME <> '') GROUP BY  OPERATIONID, OPERATIONNAME ORDER BY  OPERATIONID ",CONN
    s14="<option value=""*"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s14=s14 &"<option value=""" & rs("OPERATIONNAME") & """>" &rs("OPERATIONNAME") &"</option>"
       rs.MoveNext
    Loop
    s14=s14 &"<option value=""無法歸屬"">無法歸屬</option>"
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
  '---用戶名稱
  S1=document.all("search1").value  
  If Len(s1)=0 Or s1="" Then
     t=t &" (KTSCUST.CUSNC<> '*' )" 
  Else
     s=s &"  用戶名稱:包含('" &S1 & "'字元)"
     t=t &" (KTSCUST.CUSNC LIKE '%" &S1 &"%')" 
  End If
  '---NCIC用戶編號
  S2=document.all("search2").value  
  If Len(TRIM(s2)) > 0 Then
     s=s &"  NCIC用戶編號:包含('" &S2 & "'字元)"
     t=t &" AND (KTSCUST.NCICCUSID LIKE '%" &S2 &"%')" 
  End If
  '---用戶統編
  S3=document.all("search3").value  
  If Len(TRIM(s3)) > 0 Then
     s=s &"  用戶統編:包含('" &S3 & "'字元)"
     t=t &" AND (KTSCUST.SOCIALID LIKE '%" &S3 &"%')" 
  End If
  '---安裝地址
  S4=document.all("search4").value  
  If Len(TRIM(s4)) > 0 Then
     s=s &"  安裝地址:包含('" &S4 & "'字元)"
     t=t &" AND (RTCOUNTY.CUTNC + KTSCUST.TOWNSHIP3+KTSCUST.RADDR3 LIKE '%" &S4& "%')" 
  End If  
  '---電話(包含公司電話、公司傳真、聯絡人電話、聯絡人行動電話、聯絡人傳真)
  S5=document.all("search5").value  
  If Len(TRIM(s5)) > 0 Then
     s=s &"  電話:包含('" &S5 & "'字元)"
     t=t &" AND ( (KTSCUST.COTEL11 + KTSCUST.COTEL12 LIKE '%" &S5 &"%') OR (KTSCUST.COFAX11 + KTSCUST.COFAX12 LIKE '%" &S5 &"%') OR " _
         &"   (KTSCUST.COCONTACTTEL11 + KTSCUST.COCONTACTTEL12+COCONTACTTEL13 LIKE '%" &S5 &"%') OR (KTSCUST.COCONTACTFAX11 + KTSCUST.COCONTACTFAX12 LIKE '%" &S5 &"%' ) OR " _
         &"   (KTSCUST.COCONTACTMOBILE  LIKE '%" &S5 &"%') )" 
  End If    
  '---大盤經銷商
  S6=SPLIT(document.all("search6").value,";")
  If Len(TRIM(s6(0))) > 0 Then
     s=s &"  大盤經銷商:包含('" &S6(1) & "'字元)"
     t=t &" AND ( KTSCUST.CONSIGNEE1 LIKE '%" &S6(0) &"%')" 
  End If    
  '---小盤經銷商
  S7=SPLIT(document.all("search7").value,";")
  If Len(TRIM(s7(0))) > 0 Then
     s=s &"  小盤經銷商:包含('" &S7(1) & "'字元)"
     t=t &" AND ( KTSCUST.CONSIGNEE2 LIKE '%" &S7(0) &"%')" 
  End If      
  '---開發業務
  S8=SPLIT(document.all("search8").value,";")
  If Len(TRIM(s8(0))) > 0 Then
     s=s &"  開發業務員:包含('" &S8(1) & "'字元)"
     t=t &" AND ( KTSCUST.EMPLY LIKE '%" &S8(0) &"%')" 
  End If        
  '---合約起算日
  S9=document.all("search9").value
  S10=document.all("search10").value
  IF LEN(TRIM(S9)) > 0 OR LEN(TRIM(S9)) > 0 THEN
     IF LEN(TRIM(S9))=0 THEN
        S9="1900/01/01 00:00:00"
     END IF
     IF LEN(TRIM(S10))=0 THEN
        S10="9999/12/31 11:59:59"
     END IF
     s=s &"  合約起算日:自( " &S9 & " 至 " & S10 & " )"
     t=t &" AND ( KTSCUST.CONTRACTSTRDAT >= '" &S9 &"' AND KTSCUST.CONTRACTSTRDAT <='" & S10 & "' )" 
  END IF
  '---送件申請日
  S12=document.all("search12").value
  S13=document.all("search13").value
  IF LEN(TRIM(S12)) > 0 OR LEN(TRIM(S13)) > 0 THEN
     IF LEN(TRIM(S12))=0 THEN
        S12="1900/01/01 00:00:00"
     END IF
     IF LEN(TRIM(S13))=0 THEN
        S13="9999/12/31 11:59:59"
     END IF
     s=s &"  合約起算日:自( " &S12 & " 至 " & S13 & " )"
     t=t &" AND ( KTSCUST.APPLYDAT >= '" &S12 &"' AND KTSCUST.APPLYDAT <='" & S13 & "' )" 
  END IF
  '---用戶申請進度
  S11=SPLIT(document.all("search11").value,";")
  IF LEN(TRIM(S11(0))) > 0 THEN
     '尚未送件申請
     IF S11(0)="1" THEN
        s=s &"  申請進度:" &S11(1) & "  "
        t=t &" AND ( KTSCUST.CANCELDAT IS NULL AND KTSCUST.DROPDAT IS NULL AND KTSCUST.APPLYDAT IS NULL )" 
     '已申請尚未取得NCIC用戶編號
     ELSEIF S11(0)="2" THEN
        s=s &"  申請進度:" &S11(1) & "  "
        t=t &" AND ( KTSCUST.CANCELDAT IS NULL AND KTSCUST.DROPDAT IS NULL AND KTSCUST.APPLYDAT IS NOT NULL )" 
     '已取得NCIC用戶編號尚未完工
     ELSEIF S11(0)="3" THEN
        s=s &"  申請進度:" &S11(1) & "  "
        t=t &" AND ( KTSCUST.CANCELDAT IS NULL AND KTSCUST.DROPDAT IS NULL AND KTSCUST.NCICCUSID <>'' AND KTSCUST.FINISHDAT IS NULL )" 
     '已完工未報竣
     ELSEIF S11(0)="4" THEN
        s=s &"  申請進度:" &S11(1) & "  "
        t=t &" AND ( KTSCUST.CANCELDAT IS NULL AND KTSCUST.DROPDAT IS NULL  AND KTSCUST.FINISHDAT IS NOT NULL AND KTSCUST.FINISHDAT IS  NULL )"  
     '已報竣未轉檔
     ELSEIF S11(0)="5" THEN
        s=s &"  申請進度:" &S11(1) & "  "
        t=t &" AND ( KTSCUST.CANCELDAT IS NULL AND KTSCUST.DROPDAT IS NULL  AND KTSCUST.FINISHDAT IS NOT NULL AND KTSCUST.TRANSDAT IS  NULL )"  
     '已退租
     ELSEIF S11(0)="6" THEN
        s=s &"  申請進度:" &S11(1) & "  "
        t=t &" AND ( KTSCUST.DROPDAT IS NOT NULL )"  
     '已作廢
     ELSEIF S11(0)="7" THEN
        s=s &"  申請進度:" &S11(1) & "  "
        t=t &" AND ( KTSCUST.CANCELDAT IS NOT NULL )"  
     END IF
  END IF
  s14=document.all("search14").value
  if S14 <> "*" and s14<>"無法歸屬" then
     t=t &" AND (RTCTYTOWN.OPERATIONNAME='" & S14 & "') AND KTSCUST.consignee1='' "
  elseif s14="無法歸屬" then
     t=t &" AND (RTCTYTOWN.OPERATIONNAME='') and KTSCUST.consignee1='' "
  end if
  s15=document.all("search15").value
  s=S & "經銷商:" &S15 &"  "
  if S15 <> "*" AND S15 <> "直銷" then
     t=t &" AND (rtobj_3.shortnc='" & S15 & "') "
  ELSEIF S15="直銷" THEN 
     t=t &" AND (ktscust.consignee1='') "
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
   Sub ImageIconOver()
       self.event.srcElement.style.borderBottom = "black 1px solid"
       self.event.srcElement.style.borderLeft="white 1px solid"
       self.event.srcElement.style.borderRight="black 1px solid"
       self.event.srcElement.style.borderTop="white 1px solid"   
   End Sub
   
   Sub ImageIconOut()
       self.event.srcElement.style.borderBottom = ""
       self.event.srcElement.style.borderLeft=""
       self.event.srcElement.style.borderRight=""
       self.event.srcElement.style.borderTop=""
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
  <tr class=dataListTitle align=center>KTS用戶資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">營運點</td>
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
<tr><td class=dataListHead width="40%">用戶名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="30" name="search1" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead width="40%">NCIC用戶編號</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="15" name="search2" class=dataListEntry> 
    </td></tr>  
<tr><td class=dataListHead width="40%">用戶統編</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="10" name="search3" class=dataListEntry ID="Text1"> 
    </td></tr>      
<tr><td class=dataListHead width="40%">安裝地址</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="40" name="search4" class=dataListEntry ID="Text2"> 
    </td></tr>      
<tr><td class=dataListHead width="40%">電話</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="10" name="search5" class=dataListEntry ID="Text3"> 
    </td></tr>   
<tr><td class=dataListHead width="40%">大盤經銷商</td>
    <td width="60%"  bgcolor="silver">
      <select name="search6" size="1" class=dataListEntry ID="Select2">
       <%=S6%>
      </select>
     </td>
</tr>       
<tr><td class=dataListHead width="40%">小盤經銷商</td>
    <td width="60%"  bgcolor="silver">
      <select name="search7" size="1" class=dataListEntry ID="Select3">
       <%=S6%>
      </select>
     </td>
</tr>        
<tr><td class=dataListHead width="40%">開發業務員</td>
    <td width="60%"  bgcolor="silver">
      <select name="search8" size="1" class=dataListEntry ID="Select4">
       <%=S7%>
      </select>
     </td>
</tr>        
<tr><td class=dataListHead width="40%">合約起算日</td>
    <td width="60%"  bgcolor="silver">
      <input type="text" name="search9" size="10"  class="dataListentry" ID="Text56">
         <input type="button" id="B9"  name="B9" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C9"  name="C9"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">     
        <font size=3>-</font>
         <input type="text" name="search10" size="10"  class="dataListentry" ID="Text4">
         <input type="button" id="B10"  name="B10" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C10"  name="C10"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">     
     </td>
</tr>        
<tr><td class=dataListHead width="40%">送件申請日</td>
    <td width="60%"  bgcolor="silver">
      <input type="text" name="search12" size="10"  class="dataListentry">
         <input type="button" id="B12"  name="B12" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="Img1"  name="C11"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">     
        <font size=3>-</font>
         <input type="text" name="search13" size="10"  class="dataListentry">
         <input type="button" id="B13"  name="B13" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="Img2"  name="C12"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">     
     </td>
</tr>        
<tr><td class=dataListHead width="40%">用戶進度狀況</td>
    <td width="60%"  bgcolor="silver">
      <select name="search11" size="1" class=dataListEntry ID="Select1">
        <option value=";全部" selected>全部</option>
        <option value="1;尚未送件申請">尚未送件申請</option>
        <option value="2;已申請尚未取得NCIC用戶編號">已申請尚未取得NCIC用戶編號</option>
        <option value="3;已取得NCIC用戶編號尚未完工">已取得NCIC用戶編號尚未完工</option>                
        <option value="4;已完工未報竣">已完工未報竣</option>
        <option value="5;已報竣未轉檔">已報竣未轉檔</option>      
        <option value="6;已退租">已退租</option>      
        <option value="7;已作廢">已作廢</option>                     
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