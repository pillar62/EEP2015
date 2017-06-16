<%
    parm=request("key")
    v=split(parm,";")
%>
<html>
	<head>
		<title>Ping ¤Î Reset</title>
	</head>
	<frameset rows="85%,15%">
		<frame name="framePing" scrolling="auto" src="RTPing.asp?key=<%=v(0)%>;">
		<frame name="frameReset" scrolling="auto" src="RTReset.asp?key=;">
	</frameset>
</html>
