<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
	logonid=Request.ServerVariables("LOGON_USER")
	Call SrGetEmployeeRef(Rtnvalue,1,logonid)
	logonid=split(rtnvalue,";")  

    parm=request("key")
    v=split(parm,";")

    Dim rs,conn, formid
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")
    
    sql="select m.codenc as cmtytype, convert(varchar(5), a.comq1)+ case a.lineq1 when 0 then '' else '-'+ convert(varchar(2), a.lineq1) end as comq, " &_
		"case a.cmtytype when '03' then b.comn when '05' then h.comn when '06' then i.comn when '07' then j.comn else '' end as comn, a.tel, " &_
		"case a.cmtytype when '03' then replace(b.ipaddr, ' ', '') when '05' then replace(c.gateway, ' ', '') when '06' then replace(d.lineip, ' ', '') when '07' then replace(e.lineip, ' ', '') else '' end as ip1, " &_
		"replace(isnull(c.idslamip, ''), '0.0.0.-2', '') as ip2, a.tel " &_
		"from 	RTReset a " &_
		"left outer join RTSparqAdslCmty b on a.comq1 = b.cutyid and a.cmtytype ='03' " &_
		"left outer join RTSparq499CmtyLine c inner join RTSparq499CmtyH h on h.comq1 = c.comq1 on a.comq1 = c.comq1 and a.lineq1 = c.lineq1 and a.cmtytype ='05' " &_
		"left outer join RTLessorCmtyLine d inner join RTLessorCmtyH i on i.comq1 = d.comq1 on a.comq1 = d.comq1 and a.lineq1 = d.lineq1 and a.cmtytype ='06' " &_
		"left outer join RTLessorAvsCmtyLine e inner join RTLessorAvsCmtyH j on j.comq1 = e.comq1 on a.comq1 = e.comq1 and a.lineq1 = e.lineq1 and a.cmtytype ='07' " &_
		"left outer join RTCode m on m.code = a.cmtytype and m.kind ='L5' " &_
		"where 	a.RESETNO ='" & v(0) &"' "
    rs.Open sql, CONN
		ip1 = rs("ip1")
		ip2 = rs("ip2")
		comq = rs("comq")
		comn = rs("comn")
		tel = rs("tel")
		cmtytype = rs("cmtytype")
	rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
	
	pingW = Cstr(request("colPingW"))
	if len(pingW)=0 then pingW = "4000"

	Set sh = Server.CreateObject("wscript.shell")
	if len(trim(ip1)) >0 then
		sh.run "%comspec% /c ping " &ip1&" -w "&pingW&" > d:\CBNWEB\WebAP\RTAP\Base\RTHBADSLCMTY\tempPing1.txt",0,true
	else
		sh.run "%comspec% /c echo 無主線 IP> d:\CBNWEB\WebAP\RTAP\Base\RTHBADSLCMTY\tempPing1.txt",0,true
	end if
	
	if len(trim(ip2)) >0 then
		sh.run "%comspec% /c ping " &ip2&" -w "&pingW&" > d:\CBNWEB\WebAP\RTAP\Base\RTHBADSLCMTY\tempPing2.txt",0,true
	else
		sh.run "%comspec% /c echo 無 iDslam IP> d:\CBNWEB\WebAP\RTAP\Base\RTHBADSLCMTY\tempPing2.txt",0,true
	end if
	set sh = nothing
	
	Set fso = CreateObject("Scripting.FileSystemObject")        
	file1 = server.MapPath("tempPing1.txt")
	file2 = server.MapPath("tempPing2.txt")
	set txt1 = fso.OpenTextFile(file1)
	set txt2 = fso.OpenTextFile(file2)
	content1 = replace(txt1.readall, vbCrLf, "<BR>")
	content2 = replace(txt2.readall, vbCrLf, "<BR>")
	set fso = nothing
%>

<html>
<head>
	<link REL="stylesheet" HREF="/WebUtilityV4EBT/DBAUDI/dataList.css" TYPE="text/css">
	<meta http-equiv="Content-Type" content="text/html; charset=big5">
	<TITLE>PingASP</TITLE>
	<SCRIPT language=VBScript>
		Sub cmdSure_onClick
			tel=document.all("colTel").value
			window.open "RTReset.asp?v="& Rnd &"&key=" & tel &";","frameReset"
			for i =1 to 120
				document.form1.cmdSure.value = inum
				setTimeout "chgsec("+cstr(i)+")", i * 1000
			Next
		End Sub

		sub chgsec(cnt)
			sec = 120
			if cnt = sec then
				document.form1.cmdPing.disabled = false
				document.form1.cmdSure.disabled = false
				document.form1.cmdSure.value ="Reset"
			else 
				document.form1.cmdPing.disabled = true
				document.form1.cmdSure.disabled = true
				document.FORM1.cmdSure.value = "請稍待" +cstr(sec-cnt)+"秒"
			end if
		End sub

		Sub cmdcancel_onClick
			window.parent.close
		End Sub
	</SCRIPT>
</head>
<body>

<form id="form1" method="post" name="form1">
	<TABLE id="Table1" border="1" cellPadding=0 cellSpacing=0>
		<TR>
			<TD class="dataListHEAD3">方案別</TD>
			<TD class="dataListHEAD3">主線</TD>
			<TD class="dataListHEAD3">社區名稱</TD>
			<TD class="dataListHEAD3">主線 IP</TD>
			<TD class="dataListHEAD3">iDslam IP</TD>
			<TD class="dataListHEAD3">Reset電話</TD>
		</TR>
		<TR>
			<TD class="dataListDATA3"><%=cmtytype%>&nbsp;</TD>
			<TD class="dataListDATA3"><%=comq%>&nbsp;</TD>
			<TD class="dataListDATA3"><%=comn%>&nbsp;</TD>
			<TD class="dataListDATA3"><%=ip1%>&nbsp;</TD>
			<TD class="dataListDATA3"><%=ip2%>&nbsp;</TD>
			<TD class="dataListDATA3"><%=tel%>&nbsp;</TD>
		</TR>

		<TR bgcolor=black>
			<TD colSpan="3"><font color=white><%=content1%></font><BR></TD>

			<TD colSpan="3"><font color=white><%=content2%></font><BR></TD>
		</TR>

		<TR bgcolor=black>
			<TD align=center colSpan="5" style="border:0px;">
				<INPUT TYPE=submit VALUE="再ping一次" ID="Submit1" style="cursor:hand" NAME="cmdPing">
				<font color=yellow>-w</font>
				<INPUT TYPE=text value=<%=pingW%> name=colPingW maxlength=4 size=4>
　
				<INPUT TYPE=button VALUE="Reset" ID="Button2" style="cursor:hand" NAME="cmdSure">
				<INPUT TYPE=text value=<%=tel%> name=colTel style="display:none">
			</TD>
			<TD align=right style="border:0px;">
				<INPUT TYPE=button VALUE="關閉" ID="Button1" style="cursor:hand" NAME="cmdcancel">		
			</TD>
		</TR>
	</TABLE>
</form>

</body>
</html>
