<%
	parm=request("key")
	v=split(parm,";")
	if len(v(0))=0 then
		response.Write "未撥號 !!"
	else
		Dim conn, rs, sql, comn
		set conn=server.CreateObject("ADODB.Connection")
		set rs=server.CreateObject("ADODB.Recordset")
		DSN="DSN=RTLib"
		Conn.Open DSN
		sql="select monend from RTCloseCtrl where kind ='00'"
		rs.Open sql,conn
		monend = rs("monend")
		rs.Close
		'modem 若使用中, 則等待12秒
		While monend ="Y"
			pause_time=12
			b_s=int(second(now())) 
			e_s=int(second(now())) 
			do while not(abs(e_s-b_s)>=pause_time and abs(e_s-b_s)<=(60-pause_time)) 
				e_s=int(second(now())) 
			loop 
			rs.Open sql,conn
			monend = rs("monend")
			rs.Close
		wend
		conn.Close   
		set rs=Nothing   
		set conn=Nothing

		Set sh = Server.CreateObject("wscript.shell")
			sh.run "%comspec% /c mode com1:baud=9600 parity=n data=8 stop=1 dtr=off>NUL",0,true
			sh.run "%comspec% /c echo ath0>com1",0,true
			sh.run "%comspec% /c echo atdt" &v(0)& ">com1",0,true
			'撥號後10秒再掛斷
			pause_time=10
			b_s=int(second(now())) 
			e_s=int(second(now())) 
			do while not(abs(e_s-b_s)>=pause_time and abs(e_s-b_s)<=(60-pause_time)) 
				e_s=int(second(now())) 
			loop 
			sh.run "%comspec% /c echo ath0>com1",0,true
		set sh = nothing

		response.Write Cstr(now())+" 已Reset !!<BR> Reset電話: "&v(0)	
	end if
%>

<html>
<head>
	<link REL="stylesheet" HREF="/WebUtilityV4EBT/DBAUDI/dataList.css" TYPE="text/css">
	<meta http-equiv="Content-Type" content="text/html; charset=big5">
	<TITLE>ResetASP</TITLE>
</head>
<body>
</body>

</html>