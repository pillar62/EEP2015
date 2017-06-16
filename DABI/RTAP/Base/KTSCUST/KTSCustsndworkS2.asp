<%
    Dim rs,i,conn
    Dim search1Opt,search2Opt,search6Opt, search12pt
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    
    Set rs=Server.CreateObject("ADODB.Recordset")
'----------大盤經銷商
    S3=""
    SQLXX="SELECT RTConsignee.CUSID, RTObj.SHORTNC FROM RTConsignee INNER JOIN  RTConsigneeISP ON " _
          &"RTConsignee.CUSID = RTConsigneeISP.CUSID INNER JOIN RTObj ON RTConsignee.CUSID = RTObj.CUSID " _
          &"WHERE (RTConsigneeISP.ISP = '03') ORDER BY  RTObj.SHORTNC "
    rs.Open SQLXX,CONN
    s3="<option value="";全部"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s3=s3 &"<option value=""" &rs("CUSID") & ";" & rs("SHORTNC") & """>" &rs("SHORTNC") &"</option>"
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
  '---用戶名稱
  S1=document.all("search1").value  
  If Len(s1)=0 Or s1="" Then
     t=t &" (KTSCUST.CUSNC<> '*' )" 
  Else
     s=s &"  用戶名稱:包含('" &S1 & "'字元)"
     t=t &" (KTSCUST.CUSNC LIKE '%" &S1 &"%')" 
  End If
  '---派工單號
  S2=document.all("search2").value
  If Len(TRIM(s2)) > 0 Then
     s=s &"  派工單號:包含('" &S2 & "'字元)"
     t=t &" AND ( KTSCUSTSNDWORK.PRTNO LIKE '%" &S2 &"%')" 
  End If    
  '---預定施工員
  S3=SPLIT(document.all("search3").value,";")
  If Len(TRIM(s3(0))) > 0 Then
     s=s &"  預定施工員:('" &S3(1) & "')"
     t=t &" AND ( RTOBJ_2.SHORTNC LIKE '%" &S3(0) &"%')" 
  End If    
  '---實際施工員
   S4=SPLIT(document.all("search4").value,";")
  If Len(TRIM(s4(0))) > 0 Then
     s=s &"  實際施工員:('" &S4(1) & "')"
     t=t &" AND ( RTOBJ_4.SHORTNC LIKE '%" &S4(0) &"%')" 
  End If      
  '---派工單狀況
  S5=SPLIT(document.all("search5").value,";")
  IF LEN(TRIM(S5(0))) > 0 THEN
     '尚未完工
     IF S5(0)="1" THEN
        s=s &"  派工單進度:" &S5(1) & "  "
        t=t &" AND ( KTSCUSTSNDWORK.DROPDAT IS NULL AND KTSCUST.UNCLOSEDAT IS NULL AND KTSCUST.CLOSEDAT IS NULL )" 
     '已完工結案
     ELSEIF S5(0)="2" THEN
        s=s &"  派工單進度:" &S5(1) & "  "
        t=t &" AND ( KTSCUSTSNDWORK.DROPDAT IS NULL AND KTSCUST.UNCLOSEDAT IS NULL AND KTSCUST.CLOSEDAT IS NOT NULL )" 
     '已作廢
     ELSEIF S5(0)="3" THEN
        s=s &"  派工單進度:" &S5(1) & "  "
        t=t &" AND (KTSCUSTSNDWORK.DROPDAT IS NOT NULL  )" 
     '已未完工結案
     ELSEIF S5(0)="4" THEN
        s=s &"  派工單進度:" &S5(1) & "  "
        t=t &" AND ( KTSCUSTSNDWORK.DROPDAT IS NULL AND KTSCUST.UNCLOSEDAT IS NOT NULL AND KTSCUST.CLOSEDAT IS NULL )"  
     END IF
  END IF

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
  <tr class=dataListTitle align=center>KTS用戶派工單資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">用戶名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="30" name="search1" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead width="40%">派工單號</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="10" name="search2" class=dataListEntry ID="Text1"> 
    </td></tr>      
<tr><td class=dataListHead width="40%">預定施工員</td>
    <td width="60%"  bgcolor="silver">
      <select name="search3" size="1" class=dataListEntry ID="Select2">
       <%=S3%>
      </select>
     </td>
</tr>       
<tr><td class=dataListHead width="40%">實際施工員</td>
    <td width="60%"  bgcolor="silver">
      <select name="search4" size="1" class=dataListEntry ID="Select3">
       <%=S3%>
      </select>
     </td>
</tr>        
<tr><td class=dataListHead width="40%">派工單狀況</td>
    <td width="60%"  bgcolor="silver">
      <select name="search5" size="1" class=dataListEntry ID="Select1">
        <option value=";全部" selected>全部</option>
        <option value="1;尚未完工">尚未完工</option>
        <option value="2;已完工結案">已完工結案</option>
        <option value="3;已作廢">已作廢</option>                
        <option value="4;未完工結案">未完工結案</option>
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