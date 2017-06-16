<!-- #include virtual="/webap/include/lockright.inc" -->
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
'--------- 縣市別 
'--------- 業務員 
    rs.Open "SELECT RTObj.CUSID AS CusID, RTObj.CUSNC AS CusNC " _
           &"FROM RTObj INNER JOIN RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
           &"WHERE (((RTObjLink.CUSTYID)='08')) " _
           &"ORDER BY RTObj.CUSNC ",conn
    search6Opt="<option value=""<>'*';全部"" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       search6Opt=search6Opt &"<option value=""='" &rs("CusID") &"';" &rs("CusNC") &""">" _
                             &rs("CusNC") &"</option>" &vbCrLf
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
<link REL="stylesheet" HREF="/WebUtilityV2/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub search1_OnChange()
    <%=s%>
    document.all("search2TD").innerHTML=aryCty(document.all("search1").selectedIndex)
End Sub
Sub btn_onClick()
  dim aryStr,s,t,r
  aryStr=Split(document.all("search1").value,";")
  s="業務轄區:" &aryStr(1) &"  "
  t="(RTArea.AreaType='1') AND (RTArea.AreaID" &aryStr(0) &")"
  aryStr=Split(document.all("search2").value,";")
  s=s &"  縣市別:" &aryStr(1)
  t=t &" AND (RTCmty.CutID " &aryStr(0) &")"
  r=document.all("search3").value  
  If Len(r)=0 Or r="" Then
  Else
     s=s &"  社區名稱:" &r
     t=t &" AND (RTCmty.ComN LIKE '" &r &"%')" 
  End If
  aryStr=Split(document.all("search4").value,";")
  r=document.all("search5").value  
  If Len(r)=0 Or r="" Then
  Else
     s=s &"  總戶數:" &aryStr(1) &r
     t=t &" AND (RTCmty.ComCnt" &aryStr(0) &r &")"
  End If
  aryStr=Split(document.all("search6").value,";")
  s=s &"  業務員:" &aryStr(1)
  t=t &" AND (RTCmtyBus.CusID" &aryStr(0) &")"

  Dim winP,docP
  Set winP=window.Opener
  Set docP=winP.document
  docP.all("searchQry").value=t
  docP.all("searchShow").value=s
  docP.all("keyform").Submit
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
    <td width="60%" class=dataListEntry >
      <select name="search1" size="1">
      <%=search1Opt%>
      </select>
    </td></tr>
<tr><td class=dataListHead width="40%">縣市別</td>
    <td width="60%" class=dataListEntry id="search2TD">
      <select name="search2" size="1">
        <option value="<>'*';全部">全部</option>
      </select>
    </td></tr>
<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%" class=dataListEntry >
      <input type="text" size="20" name="search3"> 
    </td></tr>
<tr><td class=dataListHead width="40%">總戶數</td>
    <td width="60%" class=dataListEntry>
      <select name="search4" size="1">
        <option value=">;大於" selected>大於</option>
        <option value="<;小於">小於</option>
        <option value="=;等於">等於</option>
      </select>
      <input type="text" size="5" name="search5" align=right> 
    </td></tr>
<tr><td class=dataListHead width="40%">業務員</td>
    <td width="60%" class=dataListEntry >
      <select name="search6" size="1">
      <%=search6Opt%>
      </select>
    </td></tr>
</table>
<table width="100%" align=right><tr><td>
  <input type="submit" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
</td></tr></table>
</body>
</html>