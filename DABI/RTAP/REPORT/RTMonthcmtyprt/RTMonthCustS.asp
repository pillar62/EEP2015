<!-- #include virtual="/Webap/include/EmployeeRef.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<%
    logonid=session("userid")
    Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")
  Domain=Mid(V(0),1,1)
  select case Domain
         case "T"
            DAreaID="='A1'"
         case "C"
            DAreaID="='A2'"         
         case "K"
            DAreaID="='A3'"         
         case else
            DareaID="=''"
  end select
  '高階主管可讀取全部資料
  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or _
     Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" then
     DAreaID="<>'*'"
  end if
  '資訊部管理員可讀取全部資料
  if userlevel=31 then DAreaID="<>'*'"
'--------------
    Dim rs,i,conn
    Dim search1Opt,search2Opt,search6Opt
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")
'--------- 業務轄區
    sql="SELECT RTArea.AREAID AS AreaID,RTArea.AREANC AS AreaNC, RTCounty.CUTID AS CutID, " _
           &"RTCounty.CUTNC AS CutNC " _
           &"FROM RTCounty INNER JOIN (RTArea INNER JOIN RTAreaCty ON RTArea.AREAID = " _
           &"RTAreaCty.AREAID and rtarea.areaid" & DareaID & ") ON RTCounty.CUTID = RTAreaCty.CUTID " _
           &"WHERE (((RTArea.AREATYPE)='1')) " _
           &"ORDER BY RTArea.AREAID, RTCounty.CUTID "
    rs.Open sql,conn

    preAreaID=""
    areaCnt=0
    search1Opt="<option value="";全部"" selected>全部</option>" &vbCrLf
    s=""
    Do While Not rs.Eof
       If preAreaID <> rs("AreaID") Then
          If areaCnt > 0 Then
             s=s &"</select>""" &vbCrLf      
          End If
          areaCnt=areaCnt + 1
          s=s &"aryCty(" &areaCnt &")=""<select name=""""search2"""" size=""""1"""">" _   
              &"<option value="""";全部"""">全部</option>"
          search1Opt=search1Opt &"<option value=""" &rs("AreaID") &";" &rs("AreaNC") &""">"  _
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
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
Sub btn_onClick()
  dim aryStr,s,t,r
  aryStr=Split(document.all("search1").value,";")
  s2=aryStr(0)
  arystr=split(document.all("search2").value,";")
  S3=aryStr(0)
  S1=document.all("key1").value
  if len(trim(s1))=0 then
     msgbox "年度月份欄位不可空白!"
  else
     prog="RTMonthCustP.asp" & "?PARM=" & S1 & ";" & S2 & ";" & S3
     Scrxx=window.screen.width
     Scryy=window.screen.height - 30
     StrFeature="top=0,left=0,scrollbars=yes,status=yes," _
            &"location=no,menubar=no,width=" & scrxx & "px" _
            &",height=" & scryy & "px" 
     Set diagWindow=window.open(prog,"",StrFeature)
  end if
End Sub
' -------------------------------------------------------------------------------------------- 
</script>
</head>
<body>
<form name=frm1 method=post action=rtcmtyprts.asp>
<table width="50%" align="center">
  <tr class=dataListTitle align=center>社區用戶戶數統計表</td><tr>
</table>
<table width="50%" border=1 cellPadding=0 cellSpacing=0 align="center">
<tr><td class=dataListHead width="40%">年度月份</td>
    <td width="60%"  bgcolor="silver">
<input type="text" name="key1" size="6"  maxlength="6" class="dataListEntry" >
     </td>
</tr>
<tr><td class=dataListHead width="40%">轄區</td>
    <td width="60%"  bgcolor="silver">
      <select name="search1" size="1" class=dataListEntry>
      <%=search1Opt%>
      </select>
    </td></tr>
<tr><td class=dataListHead width="40%">社區類別</td>
    <td width="60%" bgcolor="silver" >
      <select name="search2" size="1" class=dataListEntry>
        <option value=";全部" selected>全部</option>
        <option value="01;元訊社區">元訊社區</option>
        <option value="02;東訊社區">東訊社區</option>
        <option value="03;先銳社區">先銳社區</option>        
        <option value="04;元訊及先銳社區">元訊及先銳社區</option>        
      </select>
    </td></tr>
</table>
<table width="50%" align=right><tr><TD></td><td align="left">
  <input type="button" value=" 列印 " class=dataListButton name="btn" style="cursor:hand">
</td></tr></table>
</form>
</body>
</html>