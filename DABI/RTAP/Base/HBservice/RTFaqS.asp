<%
    Dim rs,i,conn
    Dim searchRcvusr, searchSales, searchComtype, searchSndwork
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")

    '受理人員 -----------------------------------------------------
	rs.Open "select b.emply, c.cusnc from RTFaqM a " &_
			"inner join RTEmployee b on b.emply = a.rcvusr and tran2 ='' " &_
			"inner join RTObj c on c.cusid = b.cusid " &_
			"group by b.emply, cusnc order by c.cusnc ", conn
    searchRcvusr="<option value=""0;全部"" selected></option>" &vbCrLf
    Do While Not rs.Eof
       searchRcvusr = searchRcvusr &"<option value="""& rs("EMPLY") & ";" & rs("CUSNC") &""">" &_
						 rs("CUSNC") & "</option>" &vbCrLf
       rs.MoveNext
    Loop
    rs.Close

    '業務員,  -----------------------------------------------------
    rs.Open "select b.areaid, salesnc from HBAdslCmtyCust a inner join RTEmployee b on a.salesnc = b.name " &_
			"where b.tran2='' group by b.areaid, salesnc order by b.areaid, salesnc "
    searchSales="<option value=""0;全部"" selected></option>" &vbCrLf
    'searchSales=searchSales &"<option value="";未建檔"">未建檔</option>" &vbCrLf           
    Do While Not rs.Eof
       searchSales = searchSales &"<option value="""& rs("areaid") & ";" & rs("salesnc") &""">" &_
						 rs("salesnc") & "</option>" &vbCrLf
       rs.MoveNext
    Loop
    rs.Close

    '方案別 -----------------------------------------------------
    '20150607移除sonet資料
    '20150623加回sonet
    rs.Open "select code, codenc from RTCode where kind ='P5' and code in('3','6','7','8','9','A','B') "
    'rs.Open "select code, codenc from RTCode where kind ='P5' and code in('3','6','7','8','9','B') "
    searchComtype="<option value=""0;全部"" selected></option>" &vbCrLf
    Do While Not rs.Eof
       searchComtype = searchComtype &"<option value="""& rs("code") & ";" & rs("codenc") &""">" &_
						 rs("codenc") & "</option>" &vbCrLf
       rs.MoveNext
    Loop
    rs.Close

    '客源別 -----------------------------------------------------
    rs.Open "select code, codenc from RTCode where kind ='Q3' "
    searchSource="<option value=""0;全部"" selected></option>" &vbCrLf
    Do While Not rs.Eof
       searchSource = searchSource &"<option value="""& rs("code") & ";" & rs("codenc") &""">" &_
						 rs("codenc") & "</option>" &vbCrLf
       rs.MoveNext
    Loop
    rs.Close

    '預定施工人員 ----------------------------------------------------
    rs.Open "select '直銷' as belongnc, c.cusnc " &_
			"from RTAreaSales a " &_
			"inner join RTEmployee b on a.cusid = b.emply " &_
			"inner join RTobj c on b.cusid = c.cusid " &_
			"where b.tran2 <>'10' and a.areaid LIKE 'C%' " &_
			"union " &_
			"select '經銷' as belongnc, c.shortnc as cusnc " &_
			"from RTConsignee a " &_
			"inner join RTConsigneeCASE b on a.CUSID = b.CUSID " &_
			"inner join RTObj c on c.cusid = a.cusid " &_
			"where	b.caseid ='00' " &_
			"order by 1,2 "
    searchSndwork="<option value=""0;全部"" selected></option>" &vbCrLf
    Do While Not rs.Eof
       searchSndwork = searchSndwork &"<option value="""& rs("belongnc") & ";" & rs("cusnc") &""">" &_
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
  dim aryStr,s,t,r,k1
  s=""	:	t=""	:	t1 =""

  ' 社區名稱 -------------------------
  aryStr=document.all("search10").value  
  if aryStr <>"" then 
     s= s & "　社區名稱：" &aryStr &"  "  
     t= t & " AND n.comn like '%"& aryStr &"%' "
  end if

  ' 客戶名稱 -------------------------
  k1=document.all("search1").value
  if len(trim(k1)) > 0 then
     s=s & "　客戶名稱：含('" & K1 & "')字元"
     t = t & " and a.FaqMan like '%" & k1 & "%' "
  end if

  ' 受理時間 -----------------------
  s5=document.all("search5").value  
  s6=document.all("search6").value    
  if s5 <>"" and s6 <>"" then   
	 s =s & "  受理時間：" & s5 & " 至 " & s6
	 t =t & " and a.RCVDAT >= '" & s5 & " 00:00.000' and a.RCVDAT <= '" & s6 & " 23:59.997' "
  end if  

  ' 結案時間 -----------------------
  s12=document.all("search12").value  
  s13=document.all("search13").value    
  if s12 <>"" and s13 <>"" then 
 	 s=s & "  結案時間：" & s12 & " 至 " & s13
 	 t=t & " and a.closedat >= '" & s12 & " 00:00.000' and a.closedat <= '" & s13 & " 23:59.997' "
  end if

  ' 受理人員 -------------------------
  aryStr=Split(document.all("search7").value,";")
  if aryStr(0) <>"0" then 
     s=s & "  受理人員：" &aryStr(1) &"  "  
	 t = t & " AND a.rcvusr ='" &aryStr(0)& "' "
  end if 

' 直經銷 -------------------------
  aryStr=Split(document.all("search11").value,";")  
  IF arystr(0)<>"0" then
     s=s & "  直經銷︰" & arystr(1)
	 t=t & " AND case n.groupnc when '' then '直銷' else '經銷' end = '"& aryStr(1) &"' "
  end if

  ' 業務員 -------------------------
  aryStr=Split(document.all("search8").value,";")  
  IF arystr(0)<>"0" then
     s=s & "  業務員︰" & arystr(1)
	 t=t & " AND n.groupnc + n.leader = '"& aryStr(1) &"' "
  end if

  ' 結案狀態 -------------------------
  aryStr=Split(document.all("search2").value,";")
  s=s & "　結案狀態：" &aryStr(1)
  if arystr(0) = "0" then	 '全部
	t =	t & " "
  elseif arystr(0) = "1" then '未結
	t =	t & " and a.closedat is null and a.canceldat is null "
  elseif arystr(0) = "2" then '已結
	t =	t & " and a.closedat is not null and a.canceldat is null "
  elseif arystr(0) = "3" then '未完工
	t1 = t1 & " inner "
  end if

 ' 方案別 -------------------------
  aryStr=Split(document.all("search3").value,";")  
  IF arystr(0)<>"0" then
     s=s & "  方案別︰" & arystr(1)
	 t=t & " AND c.comtypenc = '"& aryStr(1) &"' "
  end if

 ' 客源別 -------------------------
  aryStr=Split(document.all("search9").value,";")  
  IF arystr(0)<>"0" then
     s=s & "  客戶來源︰" & arystr(1)
	 t=t & " AND a.custsrc = '"& aryStr(0) &"' "
  end if

  ' 預定施工人員 -------------------------
  aryStr=Split(document.all("search4").value,";")  
  IF arystr(0)<>"0" then
     s=s & "  預定施工人員︰" & arystr(1)
	 t=t & " AND isnull(k.shortnc,i.name) = '"& aryStr(1) &"' "
  end if

  
  Dim winP,docP
  Set winP=window.Opener
  Set docP=winP.document
  docP.all("searchQry").value=t
  docP.all("searchQry1").value=t1
  'docP.all("searchQry2").value=t2
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
  <tr class=dataListTitle align=center>客訴資料查詢</td><tr>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>

<tr><td class=dataListHead width="40%">直經銷</td>
    <td width="60%"  bgcolor="silver">
      <select name="search11" size="1" class=dataListEntry ID="Select2">
        <option value="0;全部"></option>              
        <option value="1;直銷">直銷</option>
        <option value="2;經銷">經銷</option>
      </select></td></tr>

<tr><td class=dataListHead width="40%">業務員</td>
    <td width="60%" bgcolor="silver" >
      <select name="search8" size="1" class=dataListEntry ID="Select4">
      <%=searchSales%>
      </select></td></tr>

<tr><td class=dataListHead width="40%">方案別</td>
    <td width="60%"  bgcolor="silver">
      <select name="search3" size="1" class=dataListEntry ID="Select1">
      <%=searchComtype%>
      </select></td></tr>

<tr><td class=dataListHead width="40%">客戶來源</td>
    <td width="60%"  bgcolor="silver">
      <select name="search9" size="1" class=dataListEntry ID="Select9">
      <%=searchSource%>
      </select></td></tr>

<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search10" class=dataListEntry></td></tr>

<tr><td class=dataListHead width="40%">客戶名稱</td>
    <td width="60%" bgcolor="silver">
      <input type="text" size="20" name="search1" class=dataListEntry></td></tr>
    
<tr><td class=dataListHead width="40%">受理人員</td>
    <td width="60%" bgcolor="silver" >
      <select name="search7" size="1" class=dataListEntry ID="Select3">
      <%=searchRcvusr%>
      </select></td></tr>

<tr><td class=dataListHead width="40%">受理時間</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search5" size="10" maxlength="10" class=dataListEntry  value="<%=Sdate%>" ID="Text1">
    <input type="button" id="B5"  name="B5" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">         
    至
    <input type=text name="search6" size="10" maxlength="10" class=dataListEntry  value="<%=edate%>" ID="Text2">    
    <input type="button" id="B6"  name="B6" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"></td></tr>    

<tr><td class=dataListHead width="40%">結案時間</td>
    <td width="60%" bgcolor="silver" >
    <input type=text name="search12" size="10" maxlength="10" class=dataListEntry  value="<%=Sdate%>" ID="Text3">
    <input type="button" id="B12"  name="B12" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">         
    至
    <input type=text name="search13" size="10" maxlength="10" class=dataListEntry  value="<%=edate%>" ID="Text4">    
    <input type="button" id="B13"  name="B13" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"></td></tr>    

<tr><td class=dataListHead width="40%">結案狀態</td>
    <td width="60%"  bgcolor="silver">
      <select name="search2" size="1" class=dataListEntry>
        <option value="0;全部">全部</option>        
        <option value="1;未結案" selected>未結案</option>
        <option value="2;已結案">已結案</option>
        <option value="3;未完工">未完工</option>
      </select></td></tr>

<tr><td class=dataListHead width="40%">預定施工人員</td>
    <td width="60%" bgcolor="silver" >
      <select name="search4" size="1" class=dataListEntry ID="Select5">
      <%=searchSndwork%>
      </select></td></tr>

</table>
<table width="100%" align=right><tr><TD></td><td align="right">
  <input type="SUBMIT" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</td></tr></table>
</body>
</html>
