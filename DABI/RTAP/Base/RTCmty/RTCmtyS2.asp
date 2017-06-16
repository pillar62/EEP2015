<%
    Dim rs,i,conn
    Dim search1Opt,search2Opt,search6Opt
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")
'--------- 穨叭烈跋
    rs.Open "SELECT RTArea.AREAID AS AreaID,RTArea.AREANC AS AreaNC, RTCounty.CUTID AS CutID, " _
           &"RTCounty.CUTNC AS CutNC " _
           &"FROM RTCounty INNER JOIN (RTArea INNER JOIN RTAreaCty ON RTArea.AREAID = " _
           &"RTAreaCty.AREAID) ON RTCounty.CUTID = RTAreaCty.CUTID " _
           &"WHERE (((RTArea.AREATYPE)='1')) " _
           &"ORDER BY RTArea.AREAID, RTCounty.CUTID ",conn
    preAreaID=""
    areaCnt=0
    search1Opt="<option value=""<>'*';场"" selected>场</option>" &vbCrLf
    s=""
    Do While Not rs.Eof
       If preAreaID <> rs("AreaID") Then
          If areaCnt > 0 Then
             s=s &"</select>""" &vbCrLf      
          End If
          areaCnt=areaCnt + 1
          s=s &"aryCty(" &areaCnt &")=""<select name=""""search2"""" size=""""1"""">" _   
              &"<option value=""""<>'*';场"""">场</option>"
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
        &"aryCty(0)=""<select name=""""search2""""><option value=""""<>'*';场"""">场</option></select>""" &vbCrLf &s     
    End If     
    rs.Close
'--------- 郡カ 
'--------- 穨叭 
    rs.Open "SELECT RTObj.CUSID AS CusID, RTObj.CUSNC AS CusNC " _
           &"FROM RTObj INNER JOIN RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
           &"WHERE (((RTObjLink.CUSTYID)='08')) " _
           &"ORDER BY RTObj.CUSNC ",conn
    search6Opt="<option value=""<>'*';场"" selected>场</option>" &vbCrLf
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
  aryStr=Split(document.all("search1").value,";")
  s="穨叭烈跋:" &aryStr(1) &"  "
  t="(RTArea.AreaType='1') AND (RTArea.AreaID" &aryStr(0) &")"
  aryStr=Split(document.all("search2").value,";")
  s=s &"  郡カ:" &aryStr(1)
  t=t &" AND (RTCmty.CutID " &aryStr(0) &")"
  r=document.all("search3").value  
  If Len(r)=0 Or r="" Then
  Else
     s=s &"  跋嘿:" &r
     t=t &" AND (RTCmty.ComN LIKE '%" &r &"%')" 
  End If
  aryStr=Split(document.all("search4").value,";")
  r=document.all("search5").value  
  If Len(r)=0 Or r="" Then
  Else
     s=s &"  羆め计:" &aryStr(1) &r
     t=t &" AND (RTCmty.ComCnt" &aryStr(0) &r &")"
  End If
  aryStr=Split(document.all("search6").value,";")
  s=s &"  穨叭:" &aryStr(1)
  t=t &" AND (RTCmtySale.CusID" &aryStr(0) &")"

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
  window.close
End Sub
-->
</script>
</head>
<body>
<table width="100%">
  <tr class=dataListTitle align=center>め戈穓碝兵ン</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">秨祇贺摸</td>
    <td width="60%" bgcolor="silver">
      <select name="search4" size="1" class=datalistentry>
        <option value="<>'*';场" selected>场</option>
        <option value="=;">ビ杆め</option>
        <option value="=;单">瞏め</option>
        <option value="=;单">發め</option>        
      </select>
    </td></tr>
<tr><td class=dataListHead width="40%">秈篈</td>
    <td width="60%" bgcolor="silver" id="search2TD">
      <select name="search4"  size="1" class=datalistentry>
        <option value=">;" selected>场</option>
        <option value="<;">ゼ祇</option>
        <option value="=;单">祇ゼЧ</option>
        <option value="=;单">Ч甿虫ゼΜン</option>        
        <option value="=;单">ЧゼΜ蹿</option>        
        <option value="=;单">Μ蹿ゼ眀</option>                        
      </select>
    </td></tr>
<tr><td class=dataListHead width="40%">Μ蹿</td>
    <td width="60%"  bgcolor="silver">
      <select name="search4" size="1" class=datalistentry>
        <option value=">;" selected>场</option>
        <option value="<;">Μ蹿</option>
        <option value="=;单">ゼΜ蹿</option>
      </select>
    </td></tr>
<tr><td class=dataListHead width="40%">ビ叫篈</td>
    <td width="60%"  bgcolor="silver">
      <select name="search4" size="1" class=datalistentry>
        <option value=">;" selected>场</option>
        <option value="<;">ゼ篗綪</option>
        <option value="=;单">篗綪</option>
      </select>
     </td></tr>
<tr><td class=dataListHead width="40%">め﹎</td>
    <td width="60%"  bgcolor="silver" >
     <input type="text" size="30" name="search5" align=right class=datalistentry> 
    </td></tr>
<tr><td class=dataListHead width="40%">琁ぱ计</td>
    <td width="60%"  bgcolor="silver" >
      <select name="search4" size="1" class=datalistentry>
        <option value=">;" selected></option>
        <option value="<;"></option>
        <option value="=;单">单</option>
      </select>
      <input type="text" size="5" name="search5" align=right value=3 class=datalistentry>ぱ
    </td></tr>    
</table>
<table width="100%" align=right><tr><TD></td><td align=right>
  <input type="SUBMIT" value=" 琩高 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 挡 " class=dataListButton name="btn1" style="cursor:hand">  
</td></tr></table>
</body>
</html>