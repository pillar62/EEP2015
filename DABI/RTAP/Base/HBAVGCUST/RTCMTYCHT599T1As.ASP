<%
    Dim rs,i,conn
    Dim search1Opt,search2Opt,search6Opt, search12pt
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    
    Set rs=Server.CreateObject("ADODB.Recordset")
'--------- 業務轄區
    rs.Open "SELECT RTArea.AREAID AS AreaID,RTArea.AREANC AS AreaNC, RTCounty.CUTID AS CutID, " _
           &"RTCounty.CUTNC AS CutNC " _
           &"FROM RTCounty INNER JOIN (RTArea INNER JOIN RTAreaCty ON RTArea.AREAID = " _
           &"RTAreaCty.AREAID) ON RTCounty.CUTID = RTAreaCty.CUTID " _
           &"WHERE (((RTArea.AREATYPE)='1')) " _
           &"ORDER BY RTArea.AREAID, RTCounty.CUTID ",conn
    preAreaID=""
    areaCnt=0
    search1Opt="<option value=""<>'*';全部"" selected>全部</option>" &vbCrLf
    s=""
    Do While Not rs.Eof
       If preAreaID <> rs("AreaID") Then
          If areaCnt > 0 Then
             s=s &"</select>""" &vbCrLf      
          End If
          areaCnt=areaCnt + 1
          s=s &"aryCty(" &areaCnt &")=""<select name=""""search2"""" size=""""1"""">" _   
              &"<option value=""""<>'*';全部"""">全部</option>"
          search1Opt=search1Opt &"<option value=""='" &rs("AreaID") &"';" &rs("AreaNC") &""">"  _
                                &rs("AreaNC") &"</option>" &vbCrLf

          preAreaID=rs("AreaID")
       End If
       s=s &"<option value=""""='" &rs("CutID") &"';" &rs("CutNC") &""""">" _
                             &rs("CutNC") &"</option>"    
       rs.MoveNext
    Loop 
    If areaCnt > 0 Then
       s=s &"</select>""" &vbCrLf
       s="Dim aryCty(" &areaCnt &")" &vbCrLf _
        &"aryCty(0)=""<select name=""""search2""""><option value=""""<>'*';全部"""">全部</option></select>""" &vbCrLf &s     
    End If     
    rs.Close

'--------- 業務組別    
    rs.Open "SELECT GROUPID, GROUPNC FROM RTSALESGROUP ",CONN
    search12pt="<option value=""<>'*';全部"" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       search12pt=search12pt &"<option value=""='" &rs("GROUPID") &"';" &rs("GROUPNC") &""">" _
                             &rs("GROUPNC") &"</option>" &vbCrLf
       rs.MoveNext
    Loop
    rs.Close
'--------- 縣市別 
'--------- 業務員 
    rs.Open  "SELECT RTObj.CUSID AS CusID, RTObj.CUSNC AS CusNC " _
            &"FROM RTObj INNER JOIN " _
            &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID " _
            &"WHERE rtemployee.authlevel = '2' " _
            &"ORDER BY RTObj.CUSNC ",conn
    search6Opt="<option value=""<>'*';全部"" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       search6Opt=search6Opt &"<option value=""='" &rs("CusID") &"';" &rs("CusNC") &""">" _
                             &rs("CusNC") &"</option>" &vbCrLf
       rs.MoveNext
    Loop 
    rs.Close
'----------社區類別
    S10=""
    rs.Open "SELECT CODE,CODENC FROM RTCODE WHERE KIND='B3'",CONN
    s10="<option value=""<>'*';全部"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s10=s10 &"<option value=""" &rs("CODE") & ";" & rs("CODENC") & """>" &rs("CODENC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'----------
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV3/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub search1_OnChange()
    <%=s%>
    document.all("search2TD").innerHTML=aryCty(document.all("search1").selectedIndex)
End Sub
Sub btn_onClick()
  dim aryStr,s,t,r
  '----業務轄區
  aryStr=Split(document.all("search1").value,";")
  s="業務轄區:" &aryStr(1) &"  "
  t="(RTCmty.COMQ1 <>0) "
  if arystr(0) <> "<>'*'" then
     t=t& " AND (RTArea.AreaID" &aryStr(0) &")"
  end if
  '----業務組別
  aryStr=Split(document.all("search12").value,";")
  s="業務組別:" &aryStr(1) &"  "
  if arystr(0) <> "<>'*'" then
     t=t& " AND (RTVCmtyGroup.GROUPID" &aryStr(0) &")"
  end if
  '----縣市別  
  aryStr=Split(document.all("search2").value,";")
  s=s &"  縣市別:" &aryStr(1)
  if arystr(0) <> "<>'*'" then
     t=t &" AND (RTCmty.CutID " &aryStr(0) &")"
  end if
  '----社區名稱
  r=document.all("search3").value  
  If Len(r)=0 Or r="" Then
  Else
     s=s &"  社區名稱:" &r
     t=t &" AND (RTCmty.ComN LIKE '%" &r &"%')" 
  End If
  '----社區 HB 號碼
  r=document.all("search13").value  
  If Len(r)=0 Or r="" Then
  Else
     s=s &"  社區HB號碼:" &r
     t=t &" AND (RTCmty.HBNO LIKE '%"&r&"%' or RTCmty.HBNO2 LIKE '%"&r&"%') " 
  End If
  '----社區地址
  r=document.all("search14").value  
  If Len(r)=0 Or r="" Then
  Else
     s=s &"  社區地址:" &r
     t=t &" AND (RTCounty.CUTNC+RTCmty.Township+rtcmty.addr LIKE '%" &r &"%')" 
  End If
  '----社區總戶數
  aryStr=Split(document.all("search4").value,";")
  r=document.all("search5").value  
  If Len(r)=0 Or r="" Then
  Else
     s=s &"  總戶數:" &aryStr(1) &r
     t=t &" AND (RTCmty.ComCnt" &aryStr(0) &r &")"
  End If
  '----業務員
  aryStr=Split(document.all("search6").value,";")
  s=s &"  業務員:" &aryStr(1)
  if arystr(0) <> "<>'*'" then
     t=t &" AND (RTVCmtyGroup.CusID" &aryStr(0) &")"
  end if
  '----T1開通狀況
  arystr=split(document.all("search7").value,";")
  s=s &"  T1開通狀況:" & aryStr(1)
  if aryStr(0)="1" then
     t=t & ""
  elseif aryStr(0)="2" then
     t=t & " and (rtcmty.T1apply is not null ) "
  elseif aryStr(0)="3" then
     t=t & " and (rtcmty.T1apply is null) "
  end if
  '----是否同意建置  
  arystr=split(document.all("search8").value,";")
  s=s &"  是否同意建置:" & aryStr(1)
  if aryStr(0)="*" then
     t=t & ""
  elseif aryStr(0)="Y" or arystr(0)="N" or arystr(0)="H" then
     t=t & " and (rtcmty.agree='" & arystr(0) & "' ) " 
  elseif aryStr(0)="" then
     t=t & " and (rtcmty.agree='') " 
  end if  
  '----社區類別
  arystr=split(document.all("search10").value,";")    
  s=s &"  社區類別:" & aryStr(1)
  if aryStr(0)="<>'*'" then
     t=t & ""
  else
     t=t & " and (rtcmty.COMTYPE='" & arystr(0) & "' ) " 
  end if   
  '----社區連結型態
  arystr=split(document.all("search11").value,";")    
  s=s &"  連結型態:" & aryStr(1)
  if aryStr(0)="" then
     t=t & ""
  else
     t=t & " and (rtcmty.ConnectTYPE='" & arystr(0) & "' ) " 
  end if     
  '----社區IP
  r=document.all("search15").value  
  If Len(r)=0 Or r="" Then
  Else
     s=s &"  社區主線IP含:(" &r & ") "
     t=t &" AND ((RTCMTY.NETIP LIKE '%" &r & "%') OR (RTCMTY.NETIP2 LIKE '%" &r &"%'))" 
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
  <tr class=dataListTitle align=center>社區基本資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">業務轄區</td>
    <td width="60%"  bgcolor="silver">
      <select name="search1" size="1" class=dataListEntry>
      <%=search1Opt%>
      </select>
    </td></tr>
<tr><td class=dataListHead width="40%">業務組別</td>
    <td width="60%"  bgcolor="silver">
      <select name="search12" size="1" class=dataListEntry>
      <%=search12pt%>
      </select>
    </td></tr>
<tr><td class=dataListHead width="40%">縣市別</td>
    <td width="60%"  id="search2TD" bgcolor="silver">
      <select name="search2" size="1" class=dataListEntry>
        <option value="<>'*';全部">全部</option>
      </select>
    </td></tr>
<tr><td class=dataListHead width="40%">社區類別</td>
    <td width="60%"  bgcolor="silver">
      <select name="search10" size="1" class=dataListEntry>
        <%=s10%>
      </select>
    </td></tr>    
<tr><td class=dataListHead width="40%">HB號碼(含固定&計量制)</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="10" name="search13" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search3" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead width="40%">社區主線IP</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search15" class=dataListEntry ID="Text1"> 
    </td></tr>    
<tr><td class=dataListHead width="40%">社區住址</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="40" name="search14" class=dataListEntry> 
    </td></tr>
<tr><td class=dataListHead width="40%">T1開通狀況</td>
    <td width="60%"  bgcolor="silver">
      <select name="search7" size="1" class=dataListEntry>
        <option value="1;全部" selected>全部</option>
        <option value="2;已開通">已開通</option>
        <option value="3;未開通">未開通</option>
      </select>
     </td>
</tr>
<tr><td class=dataListHead width="40%">社區連結方式</td>
    <td width="60%"  bgcolor="silver">
      <select name="search11" size="1" class=dataListEntry>
        <option value=";全部" selected>全部</option>
        <option value="01;固定制599">固定制599</option>
        <option value="02;計量制">計量制</option>
        <option value="03;固定制599+計量制">固定制599+計量制</option>        
      </select>
     </td>
</tr>
<tr><td class=dataListHead width="40%">開通時間超過</td>
    <td width="60%"  bgcolor="silver">
    <input type="text" size="5" name="search9" align=right class=dataListEntry>
    天 
     </td>
</tr>
<tr><td class=dataListHead width="40%">是否同意建置</td>
    <td width="60%"  bgcolor="silver">
      <select name="search8" size="1" class=dataListEntry>
        <option value="*;全部" selected>全部</option>
        <option value="Y;同意">同　意</option>
        <option value="N;不同意">不同意</option>
        <option value="H;暫緩">暫　緩</option>        
        <option value=";未審核">未審核</option>
      </select>
     </td>
</tr>
<tr><td class=dataListHead width="40%">總戶數</td>
    <td width="60%"  bgcolor="silver">
      <select name="search4" size="1" class=dataListEntry>
        <option value=">;大於" selected>大於</option>
        <option value="<;小於">小於</option>
        <option value="=;等於">等於</option>
      </select>
      <input type="text" size="5" name="search5" align=right class=dataListEntry> 
    </td></tr>    
<tr><td class=dataListHead width="40%">業務員</td>
    <td width="60%" bgcolor="silver" >
      <select name="search6" size="1" class=dataListEntry>
      <%=search6Opt%>
      </select>
    </td></tr>
</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>