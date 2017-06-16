<%
    Dim rs,i,conn
    Dim search1Opt,search2Opt,search6Opt
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
'--------- 安裝人員 
    rs.Open  "SELECT RTObj.CUSID AS CusID, RTObj.shortnc AS shortnc " _
            &"FROM RTObj INNER JOIN " _
            &"RTSupp ON RTObj.CUSID = RTSupp.CUSID " _
            &"ORDER BY RTObj.CUSNC ",conn
    search6Opt="<option value=""<>'*';全部"" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       search6Opt=search6Opt &"<option value=""='" &rs("CusID") &"';" &rs("ShortNc") &""">" _
                             &rs("ShortNc") &"</option>" &vbCrLf
       rs.MoveNext
    Loop 
    rs.Close
'----------
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
'通知發包日--default為系統日期
    Edate=DateValue(Now())
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
  SDate=document.all("search7").value
  Edate=document.all("search8").value
  if Len(Trim(Sdate)) = 0 then
     document.all("search7").value = "2000/01/01"
  End if 
  if Len(Trim(Edate)) = 0 then
     document.all("search8").value = "9999/12/31"
  End if
  If Not IsDate(document.all("search7").value) or Not IsDate(document.all("search8").value) then
     msgbox "通知發包日條件錯誤：日期格式錯誤!"
  elseIf Sdate > Edate then
     msgbox "通知發包日條件錯誤：起日(" &Sdate & ")須小於迄日(" & EDate & ")!"
  Else
    dim aryStr,s,t,r
    t=""
   aryStr=Split(document.all("search1").value,";")
   s="業務轄區:" &aryStr(1) &"  "
   t=t & " and (d.AreaType='1') AND (d.AreaID" &aryStr(0) &")"
   aryStr=Split(document.all("search2").value,";")
   s=s &"  縣市別:" &aryStr(1)
   t=t &" and (c.CutID " &aryStr(0) &")"

    aryStr=Split(document.all("search4").value,";")
    s=s &"  發包別:" &aryStr(1) 
    if arystr(0) = "1" then
       t=t & " and (a.reqdat is null) "
    elseif arystr(0)="2" then
       t=t &" and (A.reqdat is not null ) "
    end if
    aryStr=Split(document.all("search5").value,";")
    s=s &"  撤銷別:" &aryStr(1) 
    if arystr(0) = "1" then
       t=t & " and (a.dropdat is null) "
    elseif arystr(0)="2" then
       t=t &" AND (A.dropdat is not null ) "
    end if
    aryStr=Split(document.all("search6").value,";")
    s=s &"  安裝人員:" &aryStr(1) & "  通知發包日：自(" & document.all("search7").value & ")至(" & document.all("search8").value & ")"
    t=t &" AND (a.Profac" &aryStr(0) &")"
    t=t &" and a.sndinfodat >='" & document.all("search7").value + " 00:00:00.000" & "' and a.sndinfodat <='" & document.all("search8").value + " 23:59:59.999" & "'"
  
    Dim winP,docP
    Set winP=window.Opener
    Set docP=winP.document
    docP.all("searchQry").value=t
    docP.all("searchShow").value=s
    docP.all("keyform").Submit
    winP.focus()
    window.close
  End if
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
  <tr class=dataListTitle align=center>RT發包資料搜尋條件</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">業務轄區</td>
    <td width="60%"  bgcolor="silver">
      <select name="search1" size="1" class=dataListEntry>
      <%=search1Opt%>
      </select>
    </td></tr>
<tr><td class=dataListHead width="40%">縣市別</td>
    <td width="60%"  id="search2TD" bgcolor="silver">
      <select name="search2" size="1" class=dataListEntry>
        <option value="<>'*';全部">全部</option>
      </select>
    </td></tr>
<tr><td class=dataListHead width="40%">發包別</td>
    <td width="60%"  bgcolor="silver">
      <select name="search4" size="1" class=dataListEntry>
        <option value="1;未發包" selected>未發包</option>
        <option value="2;已發包">已發包</option>
        <option value="3;全部">全部</option>
      </select>
    </td></tr>    
<tr><td class=dataListHead width="40%">撤銷別</td>
    <td width="60%"  bgcolor="silver">
      <select name="search5" size="1" class=dataListEntry>
        <option value="1;未撤銷" selected>未撤銷</option>
        <option value="2;已撤銷">已撤銷</option>
        <option value="3;全部">全部</option>
      </select>
    </td></tr>        
<tr><td class=dataListHead width="40%">安裝人員</td>
    <td width="60%" bgcolor="silver" >
      <select name="search6" size="1" class=dataListEntry>
      <%=search6Opt%>
      </select>
    </td></tr>
<tr><td class=dataListHead width="40%">通知發包日</td>
    <td width="60%"  bgcolor="silver">
    <input type="text" size="10" maxlength="10" name="search7" align=right class=dataListEntry value="<%=Sdate%>">
    至 
     <input type="text" size="10" maxlength="10" name="search8" align=right class=dataListEntry value="<%=Edate%>">
    </td></tr>            
</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="submit" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>