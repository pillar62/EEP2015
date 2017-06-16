<%
    Dim rs,i,conn
    Dim searchComtype, searchSndwork
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")

    '處理人員 ----------------------------------------------------
    rs.Open "select min(b.emply) as emply, min(c.cusnc) as cusnc, min(b.areaid) as areaid " &_
			"from RTSalesSch a " &_
			"inner join RTEmployee b on a.dealusr = b.emply " &_
			"inner join RTobj c on b.cusid = c.cusid " &_
			"group by b.emply, c.cusnc " &_
			"order by b.areaid, c.cusnc ", conn
			
    searchSndwork="<option value=""0;全部"" selected></option>" &vbCrLf
    Do While Not rs.Eof
       searchSndwork = searchSndwork &"<option value="""& rs("emply") & ";" & rs("cusnc") &""">" &_
						 rs("cusnc") & "</option>" &vbCrLf
       rs.MoveNext
    Loop
    rs.Close

    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<script language="VBScript">
Sub btn_onClick()
  dim aryStr,s,t
  s=""	:	t=""

    ' 處理人員 -------------------------
  aryStr=Split(document.all("search4").value,";")  
  IF arystr(0)<>"0" then
     s=s & "  處理人員︰" & arystr(1)
	 t=t & " and a.dealusr = '"& aryStr(0) &"' "
  end if
  
  ' 處理日期 -----------------------
  s12=document.all("search12").value  
  s13=document.all("search13").value    
  if s12 <>"" and s13 <>"" then 
 	 s=s & "  處理日期：" & s12 & " 至 " & s13
 	 t=t & " and a.dealdat >= '"& s12 &"' and a.dealdat <= '"& s13 &"' "
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

' Sub btn1_onClick()  
  ' Dim winP
  ' Set winP=window.Opener
  ' winP.focus()
  ' window.close  
' End Sub

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
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"      codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>

<body>
<table width="100%">
  <tr class=dataListTitle align=center>行程資料查詢</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>

<tr><td class=dataListHead width="40%">處理人員</td>
    <td width="60%" bgcolor="silver" >
      <select name="search4" size="1" class=dataListEntry ID="Select5">
      <%=searchSndwork%>
      </select></td>
</tr>
	  
<tr><td class=dataListHead width="40%">處理日期</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search12" size="10" maxlength="10" class=dataListEntry  value="<%=Sdate%>" ID="Text3">
    <input type="button" id="B12"  name="B12" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">         
    至
    <input type=text name="search13" size="10" maxlength="10" class=dataListEntry  value="<%=edate%>" ID="Text4">    
    <input type="button" id="B13"  name="B13" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"></td>
</tr>    

</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>
