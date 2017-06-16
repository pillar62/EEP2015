<%
    Dim rs,i,conn
    Dim search1Opt,search2Opt,search6Opt, search12pt
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    
    Set rs=Server.CreateObject("ADODB.Recordset")
'----------大盤經銷商
    S5=""
    SQLXX="SELECT RTConsignee.CUSID, RTObj.SHORTNC FROM RTConsignee INNER JOIN  RTConsigneeISP ON " _
          &"RTConsignee.CUSID = RTConsigneeISP.CUSID INNER JOIN RTObj ON RTConsignee.CUSID = RTObj.CUSID " _
          &"WHERE (RTConsigneeISP.ISP = '03') ORDER BY  RTObj.SHORTNC "
    rs.Open SQLXX,CONN
    s5="<option value="";全部"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s5=s5 &"<option value=""" &rs("CUSID") & ";" & rs("SHORTNC") & """>" &rs("SHORTNC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'-----------開發業務
    S6=""
    SQLXX="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
          &"WHERE (RTEmployee.DEPT IN ('B100', 'B106', 'B107', 'B200', 'B300', 'B600')) AND (RTEmployee.TRAN2 <> '10') AND " _
          &"(RTEmployee.AUTHLEVEL = '2') ORDER BY  RTObj.CUSNC "
    rs.Open SQLXX,CONN
    s6="<option value="";全部"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s6=s6 &"<option value=""" &rs("EMPLY") & ";" & rs("CUSNC") & """>" &rs("CUSNC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close    
'-----------方案別
    S7=""
    SQLXX="SELECT * from rtcode where kind='M2' "
    rs.Open SQLXX,CONN
    s7="<option value="";全部"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s7=s7 &"<option value=""" &rs("code") & ";" & rs("codenc") & """>" &rs("codenc") &"</option>"
       rs.MoveNext
    Loop
    rs.Close        
'----------經銷商
    S15=""
    rs.Open "SELECT  CASE WHEN RTSparqVoIPCust.CONSIGNEE = '' THEN '直銷' ELSE RTOBJ.SHORTNC END AS shortnc " _
           &"FROM  RTSparqVoIPCust LEFT OUTER JOIN RTOBJ ON RTSparqVoIPCust.CONSIGNEE= RTOBJ.CUSID " _
           &"GROUP BY  CASE WHEN RTSparqVoIPCust.CONSIGNEE = '' THEN '直銷' ELSE RTOBJ.SHORTNC END " _
           &"ORDER BY  CASE WHEN RTSparqVoIPCust.CONSIGNEE = '' THEN '直銷' ELSE RTOBJ.SHORTNC END",CONN
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
     t=t &" (a.CUSNC<> '*' )" 
  Else
     s=s &"  用戶名稱:包含('" &S1 & "'字元)"
     t=t &" (a.CUSNC LIKE '%" &S1 &"%')" 
  End If
  '---T 帳號
  S10=document.all("search10").value  
  If Len(s10)>0 then
     s=s &"  T 帳號:包含('" &S10 & "'字元)"
     t=t &" and (a.nciccusno LIKE '%" &S10 &"%')" 
  End If
  '---用戶統編
  S2=document.all("search2").value  
  If Len(TRIM(s2)) > 0 Then
     s=s &"  用戶統編:包含('" &S2 & "'字元)"
     t=t &" AND (a.SOCIALID LIKE '%" &S2 &"%')" 
  End If
  '---安裝地址
  S3=document.all("search3").value  
  If Len(TRIM(s3)) > 0 Then
     s=s &"  安裝地址:包含('" &S3 & "'字元)"
     t=t &" AND (isnull(b.CUTNC,'') + a.TOWNSHIP2+a.RADDR2 LIKE '%" &S3 &"%')" 
  End If  
  '---電話(包含公司電話、公司傳真、聯絡人電話、聯絡人行動電話、聯絡人傳真)
  S4=document.all("search4").value  
  If Len(TRIM(s4)) > 0 Then
     s=s &"  電話:包含('" &S4 & "'字元)"
     t=t &" AND ( (a.CONTACTTEL  LIKE '%" &S4 &"%') OR (a.FAX LIKE '%" &S4 &"%') OR " _
         &"   (A.MOBILE LIKE '%" &S4 &"%') OR (a.COCONTACTTEL  LIKE '%" &S4 &"%' ) OR " _
         &"   (A.COMOBILE  LIKE '%" &S4 &"%') OR ( A.COFAX LIKE '%" & S4 & "%' ))" 
  End If    
  '---經銷商
  S5=SPLIT(document.all("search5").value,";")
  If Len(TRIM(s5(0))) > 0 Then
     s=s &"  小盤經銷商:包含('" &S5(1) & "'字元)"
     t=t &" AND ( A.CONSIGNEE LIKE '%" &S5(0) &"%')" 
  End If      
  '---開發業務
  S6=SPLIT(document.all("search6").value,";")
  If Len(TRIM(s6(0))) > 0 Then
     s=s &"  開發業務員:包含('" &S6(1) & "'字元)"
     t=t &" AND ( A.salesid LIKE '%" &S6(0) &"%')" 
  End If        
  '---方案別
  S7=SPLIT(document.all("search7").value,";")
  If Len(TRIM(s7(0))) > 0 Then
     s=s &"  方案別:'" &S7(1) & "'"
     t=t &" AND ( A.casetype = '" &S7(0) &"')" 
  End If          
  '---用戶申請進度
  S8=SPLIT(document.all("search8").value,";")
  IF LEN(TRIM(S8(0))) > 0 THEN
     '尚未派工
     IF S8(0)="1" THEN
        s=s &"  申請進度:" &S8(1) & "  "
        t=t &" AND ( a.CANCELDAT IS NULL AND a.DROPDAT IS NULL AND a.wrkrcvdat IS NULL )" 
     '已派工未完工
     ELSEIF S8(0)="2" THEN
        s=s &"  申請進度:" &S8(1) & "  "
        t=t &" AND ( a.CANCELDAT IS NULL AND a.DROPDAT IS NULL AND a.wrkrcvdat IS NOT NULL and a.finishdat is null)"         
     '已完工未報竣
     ELSEIF S8(0)="3" THEN
        s=s &"  申請進度:" &S8(1) & "  "
        t=t &" AND ( a.CANCELDAT IS NULL AND a.DROPDAT IS NULL AND a.finishdat IS NOT NULL and a.docketdat is null )" 
     '已報竣未轉檔
     ELSEIF S8(0)="4" THEN
        s=s &"  申請進度:" &S8(1) & "  "
        t=t &" AND ( a.CANCELDAT IS NULL AND a.DROPDAT IS NULL AND a.docketdat IS not NULL and a.transdat is null)" 
     '已退租
     ELSEIF S8(0)="5" THEN
        s=s &"  申請進度:" &S8(1) & "  "
        t=t &" AND (  a.DROPDAT IS not NULL )"  
     '已作廢
     ELSEIF S8(0)="6" THEN
        s=s &"  申請進度:" &S8(1) & "  "
        t=t &" AND ( a.CANCELDAT IS not NULL )"  
     END IF
  END IF
  '---電話號碼(代表號)
  S9=document.all("search9").value  
  If Len(TRIM(s9)) > 0 Then
     s=s &"  電話號碼(代表號):包含('" &S9 & "'字元)"
     t=t &" AND (a.VOIPTEL LIKE '%" &S9 &"%')" 
  End If
  s14=document.all("search14").value
  if S14 <> "*" and s14<>"無法歸屬" then
     t=t &" AND (RTCTYTOWN.OPERATIONNAME='" & S14 & "') AND a.consignee='' "
  elseif s14="無法歸屬" then
     t=t &" AND (RTCTYTOWN.OPERATIONNAME='') and a.consignee='' "
  end if
  s15=document.all("search15").value
  s=S & "經銷商:" &S15 &"  "
  if S15 <> "*" AND S15 <> "直銷" then
     t=t &" AND (e.shortnc='" & S15 & "') "
  ELSEIF S15="直銷" THEN 
     t=t &" AND (a.consignee='') "
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
  <tr class=dataListTitle align=center><td>Voip用戶資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="30%">營運點</td>
    <td width="70%" bgcolor="silver">
      <select name="search14" size="1" class=dataListEntry ID="Select1">
        <%=S14%>
    </select>      
    </td></tr>        
<tr><td class=dataListHead>經銷商</td>
    <td bgcolor="silver">
    <select name="search15" size="1" class=dataListEntry ID="Select1">
        <%=S15%>
    </select>      
    </td>
</tr>    
  <tr>
    <td class=dataListHead>用戶名稱</td>
    <td bgcolor="silver"> 
      <input type="text" size="30" name="search1" class=dataListEntry>
    </td>
  </tr>

  <tr>
    <td class=dataListHead>T 帳號</td>
    <td bgcolor="silver"> 
      <input type="text" maxlength=10  size="12" name="search10" class=dataListEntry>
    </td>
  </tr>

  <tr><td class=dataListHead>用戶身份證字號(統編)</td>
	  <td bgcolor="silver">
	  <input type="text" size="10" name="search2" class=dataListEntry ID="Text1"></td></tr>

  <tr><td class=dataListHead>電話號碼(代表號)</td>
	  <td bgcolor="silver">
	  <input type="text" size="15" name="search9" class=dataListEntry ID="Text1"></td></tr>

  <tr> 
    <td class=dataListHead>安裝地址</td>
    <td bgcolor="silver"> 
      <input type="text" size="40" name="search3" class=dataListEntry ID="Text2">
    </td>
  </tr>
  <tr> 
    <td class=dataListHead>聯絡電話</td>
    <td bgcolor="silver"> 
      <input type="text" size="10" name="search4" class=dataListEntry ID="Text3">
    </td>
  </tr>
  <tr> 
    <td class=dataListHead>經銷商</td>
    <td bgcolor="silver"> 
      <select name="search5" size="1" class=dataListEntry ID="Select2">
        <%=S5%> 
      </select>
    </td>
  </tr>
  <tr> 
    <td class=dataListHead>開發業務員</td>
    <td bgcolor="silver"> 
      <select name="search6" size="1" class=dataListEntry ID="Select4">
        <%=S6%> 
      </select>
    </td>
  </tr>
  <tr> 
    <td class=dataListHead>方案別</td>
    <td bgcolor="silver"> 
      <select name="search7" size="1" class=dataListEntry ID="Select4">
        <%=S7%> 
      </select>
    </td>
  </tr>
  <tr> 
    <td class=dataListHead>用戶進度狀況</td>
    <td bgcolor="silver"> 
      <select name="search8" size="1" class=dataListEntry ID="Select1">
        <option value=";全部" selected>全部</option>
        <option value="1;尚未派工">尚未派工</option>
        <option value="2;已派工未完工">已派工未完工</option>
        <option value="3;已完工未報竣">已完工未報竣</option>
        <option value="4;已報竣未轉檔">已報竣未轉檔</option>
        <option value="5;已退租">已退租</option>
        <option value="6;已作廢">已作廢</option>
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