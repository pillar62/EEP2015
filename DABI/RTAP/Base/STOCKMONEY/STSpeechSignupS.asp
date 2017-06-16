<%
    Dim rs,i,conn
    Dim search1Opt,search2Opt,search6Opt, search12pt
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=STOCK;uid=sa;pwd=alittlecat@cbn"
    
    Set rs=Server.CreateObject("ADODB.Recordset")
'----------演講場次
    S4=""
    rs.Open "SELECT YYMMDD FROM   SpeechSignUP GROUP BY  YYMMDD ",CONN
    s4="<option value="""" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s4=s4 &"<option value=""" &rs("yymmdd") & """>" &rs("yymmdd") &"</option>"
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
  '---姓名
  S1=document.all("search1").value  
  If Len(s1)=0 Or s1="" Then
     t=t &" (SpeechSignUP.NAME<> '*' )" 
  Else
     s=s &"  姓名:包含('" &S1 & "'字元)"
     t=t &" (SpeechSignUP.NAME LIKE '%" &S1 &"%')" 
  End If
  '----EMAIL
  s2=document.all("search2").value
  If S2<>""  Then
     s=s &"  EMAIL:包含('" &s2 & "') "
     t=t &" AND (SpeechSignUP.EMAIL LIKE '%" & S2 & "%') "
  End If            
  '----報名種類
  s3=document.all("search3").value
  S3ARY=SPLIT(S3,";")
  If s3ARY(0) <> "" Then
     s=s &"  報名種類:" &s3ARY(1) & " "
        t=t &" AND (SpeechSignUP.LIVEORNET ='" & S3ARY(0) & "' ) "
  End If      
  '----報名場次
  s4=document.all("search4").value
  If s4 <> "" Then
     s=s &"  報名場次:" &s4 & " "
        t=t &" AND (SpeechSignUP.YYMMDD LIKE '%" & S4 & "%' ) "
  End If                        
  '----審核狀態
  s5=document.all("search5").value
  S5ARY=SPLIT(S5,";")
  If s5ARY(0) <> "" Then
     s=s &"  審核狀態:" &s5ARY(1) & " "
     IF S5ARY(0)="1" THEN
        t=t &" AND (SpeechSignUP.CONFIRMDAT IS NOT NULL) "
     ELSEIF S5ARY(0)="2" THEN
        t=t &" AND (SpeechSignUP.CANCELDAT IS NOT NULL) "
     ELSEIF S5ARY(0)="3" THEN
        t=t &" AND (SpeechSignUP.CONFIRMDAT IS NULL) AND (SpeechSignUP.CANCELDAT IS NULL) "
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
  <tr class=dataListTitle align=center>演講活動報名資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">姓名</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search1" class=dataListEntry ID="Text5"> 
    </td></tr>        
<tr><td class=dataListHead width="40%">EMAIL</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="30" name="search2" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead width="40%">報名種類</td>
    <td width="60%"  bgcolor="silver">
      <select name="search3" size="1" class=dataListEntry>
        <option value=";全部" selected>全部</option>
        <option value="N;網路演講">網路演講</option>
        <option value="L;現場演講">現場演講</option>
      </select>
     </td>
</tr>
<tr><td class=dataListHead width="40%">報名場次</td>
    <td width="60%"  bgcolor="silver">
      <select name="search4" size="1" class=dataListEntry ID="Select2">
      <%=S4%>
      </select>
     </td>
</tr>
<tr><td class=dataListHead width="40%">審核狀態</td>
    <td width="60%"  bgcolor="silver">
      <select name="search5" size="1" class=dataListEntry ID="Select1">
        <option value=";全部" selected>全部</option>
        <option value="1;已審核">已審核</option>
        <option value="2;已作廢">已作廢</option>
        <option value="3;尚未審核">尚未審核</option>
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