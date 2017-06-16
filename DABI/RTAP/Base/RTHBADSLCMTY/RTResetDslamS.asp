<%
    Dim rs,i,conn
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")
'--------- 社區別
    rs.Open "select code, codenc from rtcode where kind ='L5' and code in ('03','05','06','07') ",conn
    S4="<option value="";"" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       S4=S4 &"<option value=""" &rs("code") &";" &rs("codenc") &""">" _
                             &rs("codenc") &"</option>" &vbCrLf
       rs.MoveNext
    Loop 
    rs.Close
    
'--------- 工程師別
	sql="select case a.cmtytype when '03' then isnull(p.shortnc, r.cusnc) " &_
		"when '05' then isnull(s.shortnc, u.cusnc) " &_
		"when '06' then isnull(v.shortnc, x.cusnc) " &_
		"when '07' then isnull(y.shortnc, z.cusnc) else '' end as belongnc " &_
		"from	RTReset a " &_
		"left outer join RTSparqAdslCmty b on a.comq1 = b.cutyid and a.cmtytype ='03' " &_
		"left outer join RTSparq499CmtyLine c inner join RTSparq499CmtyH h on h.comq1 = c.comq1 on a.comq1 = c.comq1 and a.lineq1 = c.lineq1 and a.cmtytype ='05' " &_
		"left outer join RTLessorCmtyLine d  inner join RTLessorCmtyH i on i.comq1 = d.comq1 on a.comq1 = d.comq1 and a.lineq1 = d.lineq1 and a.cmtytype ='06' " &_
		"left outer join RTLessorAvsCmtyLine e inner join RTLessorAvsCmtyH j on j.comq1 = e.comq1 on a.comq1 = e.comq1 and a.lineq1 = e.lineq1 and a.cmtytype ='07' " &_
		"left outer join RTObj p on p.cusid = b.consignee " &_		"left outer join RTEmployee q inner join RTObj r on q.cusid = r.cusid on q.emply = b.bussid " &_		"left outer join RTObj s on s.cusid = c.consignee " &_		"left outer join RTEmployee t inner join RTObj u on t.cusid = u.cusid on t.emply = c.salesid " &_		"left outer join RTObj v on v.cusid = d.consignee " &_		"left outer join RTEmployee w inner join RTObj x on w.cusid = x.cusid on w.emply = d.salesid " &_		"left outer join RTObj y on y.cusid = e.consignee " &_		"left outer join RTEmployee aa inner join RTObj z on aa.cusid = z.cusid on aa.emply = e.salesid " &_
		"group by  	case a.cmtytype when '03' then isnull(p.shortnc, r.cusnc) " &_
		"when '05' then isnull(s.shortnc, u.cusnc) " &_
		"when '06' then isnull(v.shortnc, x.cusnc) " &_
		"when '07' then isnull(y.shortnc, z.cusnc) else '' end " &_
		"order by 1 "
    rs.Open sql,conn
    S5="<option value="";"" selected>全部</option>" &vbCrLf
    Do While Not rs.Eof
       S5=S5 &"<option value=""" &rs("belongnc") &";" &rs("belongnc") &""">" _
                             &rs("belongnc") &"</option>" &vbCrLf
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
<script language="VBScript">
<!--
Sub btn_onClick()
  
  dim s,t
  t=""
  s=""

  s4=document.all("search4").value
  s4ary=split(s4,";")  
  if s4ary(1) <>"" then
     s=s & "  社區別：" & s4ary(1) 
     t=t & " a.cmtytype ='" & s4ary(0) & "' "
  else
     s="社區別：全部  "
     t=t & " a.cmtytype <> '*' "
  end if      

  s5=document.all("search5").value
  s5ary=split(s5,";")  
  if s5ary(1) <>"" then
     s=s & "  工程師：" & s5ary(1) 
     t=t & " and case a.cmtytype when '03' then isnull(p.shortnc, r.cusnc) " &_
			"when '05' then isnull(s.shortnc, u.cusnc) " &_
			"when '06' then isnull(v.shortnc, x.cusnc) " &_
			"when '07' then isnull(y.shortnc, z.cusnc) else '' end = '" & s5ary(0) & "' "
  end if      

  s1=document.all("search1").value
  if len(trim(s1)) > 0 then
     s="社區名稱：含('" & s1 & "')字元"  
     t=t & " and (b.comn like '%" & s1 & "%' or h.comn like '%" & s1 & "%' or i.comn like '%" & s1 & "%' or j.comn like '%" & s1 & "%')"
  end if

  s16=document.all("search16").value
  if len(trim(s16))= 0 then
  else
     s=s & 	"  社區附掛電話含︰(" & s16 & ") "
     t=t & 	" and case a.cmtytype when '03' then b.cmtytel " &_
			" when '05' then c.linetel " &_
			" when '06' then d.linetel " &_
			" when '07' then e.linetel else '' end like '%" & s16 & "%' "
  end if

  s6=document.all("search6").value
  if len(trim(s6))= 0 then
  else
     s=s & 	"  Restart IP含︰(" & s6 & ") "
     t=t & 	" and convert(varchar(3),a.ip1)+'.'+convert(varchar(3),a.ip2)+'.'+convert(varchar(3),a.ip3)+'.'+convert(varchar(3),a.ip4) like '%" & s6 & "%' "
  end if

  s7=document.all("search7").value
  if s7="*" then
  elseif s7="Y" then
     s=s & "  iDslam自動Restart資料己作廢者 "
     t=t & " and a.canceldat is not null "
  elseif s7="N" then
     s=s & "  iDslam自動Restart資料未作廢者 "
     t=t & " and a.canceldat is null "
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
Sub btn1_onClick()
  window.close
End Sub

Sub ImageIconOver()
       self.event.srcElement.style.borderBottom = "black 1px solid"
       self.event.srcElement.style.borderLeft="white 1px solid"
       self.event.srcElement.style.borderRight="black 1px solid"
       self.event.srcElement.style.borderTop="white 1px solid"   
End Sub
   
Sub ImageIconOut()
       self.event.srcElement.style.borderBottom = ""
       self.event.srcElement.style.borderLeft=""
       self.event.srcElement.style.borderRight=""
       self.event.srcElement.style.borderTop=""
End Sub      
-->
</script>
</head>
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"     codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
       height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
       width=60 >
      <PARAM NAME="_ExtentX" VALUE="1270">
      <PARAM NAME="_ExtentY" VALUE="1270">
</OBJECT>
<body>
<center>
<table width="85%">
  <tr class=dataListTitle align=center>請輸入(選擇)iDslam自動Restart資料搜尋條件</td><tr>
</table>

<table width="85%" border=1 cellPadding=0 cellSpacing=0>

<tr><td class=dataListHead width="40%">社區別</td>
    <td width="60%"  bgcolor="silver">
    <select name="search4" size="1" class=dataListEntry ID="Select2">
        <%=S4%>
    </select>
    </td>
</tr>

<tr><td class=dataListHead width="40%">工程師</td>
    <td width="60%"  bgcolor="silver">
    <select name="search5" size="1" class=dataListEntry ID="Select3">
        <%=S5%>
    </select>
    </td>
</tr>

<tr><td class=dataListHead width="40%">社區名稱</td>
    <td width="60%" bgcolor="silver" >
		<input type=text name="search1" size="30" maxlength="100" class=dataListEntry>
    </td>
</tr>

<tr><td class=dataListHead width="40%">附掛電話</td>
    <td width="60%" bgcolor="silver" >
		<input type=text name="search16" size="30" maxlength="15" class=dataListEntry ID="Text2">
    </td>
</tr>

<tr><td class=dataListHead width="40%">Restart IP</td>
    <td width="60%" bgcolor="silver" >
		<input type=text name="search6" size="30" maxlength="15" class=dataListEntry ID="Text6">
    </td>
</tr>

<tr><td class=dataListHead width="40%">是否已作廢</td>
    <td width="60%" bgcolor="silver" >
		<select name="search7" size="1" class=dataListEntry ID="Select1">
			<option value="*" selected>全部</option>
			<option value="Y">已作廢</option>
			<option value="N">未作廢</option>
		</select>
	</td>
</tr>


</table>

<table width="85%" align=right><tr><td></td><td align=right>
  <input type="submit" value=" 查詢 " class=dataListButton name="btn" onsubmit="btn_onclick" style="cursor:hand">
  <input type="button" value=" 結束 " class=dataListButton name="btn1" style="cursor:hand">
</body>
</html>
