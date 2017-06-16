<%@ Language=VBScript %>
<% 
   Key=split(request("key"),";")
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
      returnvalue= window.document.all("parm").value 
      window.close
 end sub
</script> 
</head>  
<body>
<INPUT id=parm style="LEFT: 130px; POSITION: absolute; TOP: 360px; display:none" size=30 type=text readonly value="<%=key(2)%>">
</body>
</html>