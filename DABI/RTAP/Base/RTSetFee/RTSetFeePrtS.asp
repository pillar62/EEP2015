
<!-- #include virtual="/Webap/include/RTGetUserlevel.inc" -->
<%
    Dim fieldRole
    fieldRole=FrGetUserlevel(Request.ServerVariables("LOGON_USER")) 
    Dim rs,i,conn ,s1
    Dim search1Opt,search2Opt,search6Opt
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")
'--- à︹:穨叭瞶(狝いみ)
'    if fieldrole="1" then
'------ 穨叭祘畍
'       rs.open "SELECT RTEmployee.emply, RTObj.CUSNC " _
'              &"FROM RTEmployee INNER JOIN " _
'              &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
'              &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08' " _
'              &"and rtemployee.authlevel in ('2') ",conn   
'       s1="<option value=""<>'*';场"" selected>场</option>" &vbCrLf           
'       Do while not rs.EOF
'          s1= s1 & "<option value=""='" & rs("emply") & "';" & "" &  rs("cusnc") & """>" & rs("CUsNC") & "</option>" & vbcrlf
'       rs.movenext
'       Loop                    
'--- à︹:м砃场    
'    else
'------ 紅坝
       rs.Open "SELECT RTObj.CUSID, RTObj.CUSNC " _
              &"FROM RTObj INNER JOIN " _
              &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
              &"WHERE (RTObjLink.CUSTYID = '04') ",conn
       s1="<option value=""<>'*';场"" selected>场</option>" &vbCrLf           
       Do while not rs.EOF
          s1= s1 & "<option value=""='" & rs("cusid") & "';" & "" &  rs("cusnc") & """>" & rs("CUsNC") & "</option>" & vbcrlf
       rs.movenext
       Loop
       rs.Close       
'-------м砃场
       rs.open "SELECT RTEmployee.emply, RTObj.CUSNC " _
              &"FROM RTEmployee INNER JOIN " _
              &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
              &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08' " _
              &"and rtemployee.authlevel in ('4') ",conn   
       Do while not rs.EOF
          s1= s1 & "<option value=""='" & rs("emply") & "';" & "" &  rs("cusnc") & """>" & rs("CUsNC") & "</option>" & vbcrlf
       rs.movenext
       Loop        
'    end if
    rs.Close
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
Sub btn_onClick()
  dim s,t
  t=""
  s=""
  s1ary=Split(document.all("search1").value,";")
  s2ary=Split(document.all("search2").value,";")
  if s2ary(0) = "=''" then
     s2ary(0) = " (a.paydtldat is null) and "
  elseif s2ary(0) = "<>''" then
     s2ary(0) = " (a.paydtldat is not null) and "
  elseif s2ary(0) = "<>'*'" then
     s2ary(0) = ""
  end if
  if len(trim(document.all("search3").value)) < 1 then
     s3="<>'*'"
     SPRTNO=""
  else
     S3="=" & document.all("search3").value & ""
      SPRTNO=document.all("search3").value
  end if 
  s4ary=Split(document.all("search4").value,";")
  if len(trim(document.all("search5").value)) < 1 then
     S4ary(1)="=场"
  else
     S5= document.all("search5").value
  end if 
  s6ary=split(document.all("search6").value,";")
  if s6ary(0) = "=''" then
     s6ary(0) = " and (a.docketdat is null)  "
  elseif s6ary(0) = "<>''" then
     s6ary(0) = " and (a.docketdat is not null)  "
  elseif s6ary(0) = "<>'*'" then
     s6ary(0) = ""
  end if  
  s7ary=split(document.all("search7").value,";")
  if s7ary(0) = "=''" then
     s7ary(0) = " and (a.acccfmdat is null)  "
  elseif s7ary(0) = "<>''" then
     s7ary(0) = " and (a.acccfmdat is not null)  "
  elseif s7ary(0) = "<>'*'" then
     s7ary(0) = ""
  end if  
  s8ary=split(document.all("search8").value,";")
  if s8ary(0) = "=''" then
     s8ary(0) = " and (a.incomedat is null)  "
  elseif s8ary(0) = "<>''" then
     s8ary(0) = " and (a.incomedat is not null)  "
  elseif s8ary(0) = "<>'*'" then
     s8ary(0) = ""
  end if  
  s="琁紅坝" & s1ary(1) & "  " & "猵" & s2ary(1) & "  " & "у腹" & s3 & "  " &"甿虫"& s6ary(1) & "  " & "琁肂 " & s4ary(1) & S5
  s=s &"  " &"穦璸糵"& s7ary(1) & "  " & "Μ蹿眀"& s8ary(1)
  t=t & "(a.PROFAC " & s1ary(0) & ") AND " & s2ary(0) & " (a.paydtlprtno" & S3 & ") and (a.setfee + a.setfeediff " & s4ary(0) & s5 & ") "
  t=t & s6ary(0) & s7ary(0) & s8ary(0)
  t=t & ";" & s2ary(1) & ";" & SPRTNO & ";" & s1ary(0) & ";" & s8ary(1)
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
<center>
<table width="70%">
  <tr class=dataListTitle align=center>叫块(匡拒)琁禣や灿戈穓碝兵ン</td><tr>
</table>
<table width="50%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">琁紅坝</td>
    <td width="60%" bgcolor="silver" >
      <select name="search1" size="1" class=dataListEntry>
        <%=S1%>
      </select>
    </td></tr>
<tr><td class=dataListHead width="40%">や猵</td>
    <td width="60%" bgcolor="silver">
      <select name="search2" size="1" class=dataListEntry >
        <option value="='';ゼ" selected>ゼ</option>
        <option value="<>'';"></option>
        <option value="<>'*';场">场</option>
      </select>    </td></tr>
<tr><td class=dataListHead width="40%">у腹</td>
    <td width="60%" bgcolor="silver">
    <INPUT TYPE=TEXT NAME="search3" maxlength=9 size=9 class=dataListEntry >
    </td></tr>      
<tr><td class=dataListHead width="40%">甿虫Μン猵</td>
    <td width="60%" bgcolor="silver" >
    <select name="search6" size="1"  class=dataListEntry>
        <option value="<>'';Μン">Μン</option>
        <option value="='';ゼΜン">ゼΜン</option>
        <option value="<>'*';场">场</option>        
    </select>  
    </td></tr>     
<tr><td class=dataListHead width="40%">Μ蹿眀</td>
    <td width="60%"  bgcolor="silver" >
    <select name="search8" size="1"  class=dataListEntry>
        <option value="<>'';眀">眀</option>
        <option value="='';ゼ眀">ゼ眀</option>
        <option value="<>'*';场">场</option>        
    </select>  
    </td></tr>                      
<tr><td class=dataListHead width="40%">穦璸糵</td>
    <td width="60%"  bgcolor="silver">
    <select name="search7" size="1"  class=dataListEntry>
        <option value="='';ゼ糵">ゼ糵</option>
        <option value="<>'';糵">糵</option>
        <option value="<>'*';场">场</option>        
    </select>  
    </td></tr>                     
<tr><td class=dataListHead width="40%">琁肂</td>
    <td width="60%"  bgcolor="silver">
    <select name="search4" size="1"  class=dataListEntry>
        <option value=">;" selected></option>
        <option value="<;"></option>
        <option value="=;单">单</option>
    </select>  
    <INPUT TYPE=TEXT NAME="search5" maxlength=5 size=9 value=0  class=dataListEntry>
    </td></tr>         
</table>
<table width="50%" align=right><tr><td></td><td align=right>
  <input type="submit" value=" 琩高 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 挡 " class=dataListButton name="btn1" style="cursor:hand"></td></tr></table>
</body>
</html>