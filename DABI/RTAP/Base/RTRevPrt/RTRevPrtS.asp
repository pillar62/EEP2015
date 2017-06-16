
<!-- #include virtual="/Webap/include/RTGetUserlevel.inc" -->
<%
    Dim fieldRole
    fieldRole=FrGetUserlevel(Request.ServerVariables("LOGON_USER")) 
    Dim rs,i,conn ,s1
    Dim search1Opt,search2Opt,search6Opt
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")
'--- 角色為:業務助理(客服中心)
'    if fieldrole="1" then
'------ 業務工程師
'       rs.open "SELECT RTEmployee.emply, RTObj.CUSNC " _
'              &"FROM RTEmployee INNER JOIN " _
'              &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
'              &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08' " _
'              &"and rtemployee.authlevel in ('2') ",conn   
'       s1="<option value=""<>'*';：全部"" selected>全部</option>" &vbCrLf           
'       Do while not rs.EOF
'          s1= s1 & "<option value=""='" & rs("emply") & "';" & "：" &  rs("cusnc") & """>" & rs("CUsNC") & "</option>" & vbcrlf
'       rs.movenext
'       Loop                    
'--- 角色為:技術部    
'    else
'------ 廠商
       rs.Open "SELECT RTObj.CUSID, RTObj.CUSNC " _
              &"FROM RTObj INNER JOIN " _
              &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
              &"WHERE (RTObjLink.CUSTYID = '04') ",conn
       s1="<option value=""<>'*';：全部"" selected>全部</option>" &vbCrLf           
       Do while not rs.EOF
          s1= s1 & "<option value=""'" & rs("cusid") & "';" & "：" &  rs("cusnc") & """>" & rs("CUsNC") & "</option>" & vbcrlf
       rs.movenext
       Loop
       rs.Close       
'-------技術部人員
       rs.open "SELECT RTEmployee.emply, RTObj.CUSNC " _
              &"FROM RTEmployee INNER JOIN " _
              &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
              &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08' " _
              &"and rtemployee.authlevel in ('4') ",conn   
       Do while not rs.EOF
          s1= s1 & "<option value=""'" & rs("emply") & "';" & "：" &  rs("cusnc") & """>" & rs("CUsNC") & "</option>" & vbcrlf
       rs.movenext
       Loop        
'    end if
    rs.Close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
'完工日期起迄
Edate=datevalue(now())            
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV3/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub btn_onClick()
  SDate=document.all("search6").value
  Edate=document.all("search7").value
  if Len(Trim(Sdate)) = 0 then
     document.all("search6").value = "2000/01/01"
  End if 
  if Len(Trim(Edate)) = 0 then
     document.all("search7").value = "9999/12/31"
  End if
  If Not IsDate(document.all("search6").value) or Not IsDate(document.all("search7").value) then
     msgbox "完工日期條件錯誤：日期格式錯誤!"
  elseIf Sdate > Edate then
     msgbox "完工日期條件錯誤：起日(" &Sdate & ")須小於迄日(" & EDate & ")!"
  Else    
  dim s,t
  t=""
  s=""
  s1ary=Split(document.all("search1").value,";")
  if s1ary(0) = "<>'*'" then
     s1str=""
  else
     s1str=" and (a.profac=" & s1ary(0) & ") "
  end if    
  s2ary=Split(document.all("search2").value,";")
  if len(trim(document.all("search3").value)) < 1 then
     s3="<>'*'"
     SPRTNO=""
  else
     S3="=" & document.all("search3").value & ""
     SPRTNO=document.all("search3").value
  end if 
  s4ary=Split(document.all("search4").value,";")
  if len(trim(document.all("search5").value)) < 1 then
     S4ary(1)="=全部"
  else
     S5= document.all("search5").value
  end if 
  s="施工廠商" & s1ary(1) & "  " & "收款表列印狀況" & s2ary(1) & "  " & "列印批號：" & sprtno & "  " & "實收金額 " & s4ary(1) & S5
  t=t &" and a.finishdat between '" & document.all("search6").value + " 00:00:00.000 " & "' and '" & document.all("search7").value + " 23:59:59.997 " & "' "  
  s=s & "  完工日期：自(" & document.all("search6").value & ")至(" & document.all("search7").value & ")"  
  '---有輸入列印批號時，只以列印批號為條件
  if len(rtrim(sprtno)) > 0 then
     t=t & " AND a.rcvdtlno" & S3 & " "
     t=t & ";" & s2ary(0) & ";" & SPRTNO & ";" & s1ary(0)
     s="列印批號=" & sprtno
  else
     t=t & s1str & " AND a.rcvdtlno" & S3 & " and a.rcvdtlno" & s2ary(0) & " and a.actrcvamt" & s4ary(0) & s5 & " "
     t=t & ";" & s2ary(0) & ";" & SPRTNO & ";" & s1ary(0)
  end if    
  Dim winP,docP
  Set winP=window.Opener
  Set docP=winP.document
  docP.all("searchQry").value=t
  docP.all("searchShow").value=s
  docP.all("keyform").Submit
  winP.focus()
  window.close
  end if
End Sub
Sub btn1_onClick()
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
-->
</script>
</head>
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"     codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<body>
<center>
<table width="70%">
  <tr class=dataListTitle align=center>請輸入(選擇)收款明細表資料搜尋條件</td><tr>
</table>
<table width="70%" border=1 cellPadding=0 cellSpacing=0>
<tr><td class=dataListHead width="40%">施工廠商</td>
    <td width="60%" bgcolor="silver">
      <select name="search1" size="1" class=dataListEntry >
        <%=S1%>
      </select>
    </td></tr>
<tr><td class=dataListHead width="40%">收款表列印</td>
    <td width="60%"  bgcolor="silver">
      <select name="search2" size="1" class=dataListEntry >
        <option value="='';：未列印" selected>未列印</option>
        <option value="<>'';：已列印">已列印</option>
        <option value="<>'*';：全部">全部</option>
      </select>    </td></tr>
<tr><td class=dataListHead width="40%">列印批號</td>
    <td width="60%"  bgcolor="silver">
    <INPUT TYPE=TEXT NAME="search3" maxlength=9 size=9 class=dataListEntry >
    </td></tr>      
<tr><td class=dataListHead width="40%">完工日期</td>
    <td width="60%"  bgcolor="silver">
    <INPUT TYPE=TEXT NAME="search6" maxlength=10 size=10 class=dataListEntry  value="<%=Sdate%>">
               <input type="button" id="B6"  name="B6" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">        
    至
    <INPUT TYPE=TEXT NAME="search7" maxlength=10 size=10 class=dataListEntry  value="<%=edate%>">
               <input type="button" id="B7"  name="B7" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">        
    </td></tr>          
<tr><td class=dataListHead width="40%">實收金額</td>
    <td width="60%" bgcolor="silver">
    <select name="search4" size="1" class=dataListEntry >
        <option value=">;：大於" selected>大於</option>
        <option value="<;：小於">小於</option>
        <option value="=;：等於">等於</option>
    </select>  
    <INPUT TYPE=TEXT NAME="search5" maxlength=5 size=9 value=0 class=dataListEntry >
    </td></tr>         
</table>
<table width="50%" align=right><tr><td></td><td align=right>
  <input type="submit" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand"></td></tr></table>
</body>
</html>