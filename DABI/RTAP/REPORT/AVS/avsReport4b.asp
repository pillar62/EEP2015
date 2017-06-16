<%@ LANGUAGE="VBSCRIPT" %>
<% option Explicit %>
<%
    Dim rs,conn, sqlstr, v,fso, ftpPathFile, ftpFile, fsoText, fsoini,tble,tbls, closebit, btnname', iCount
    
    v=split(request("parm"),";")  

    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")
    Set rs = conn.Execute("usp_RTEBTFtp '3','" & v(0) &"'")
    closebit = rs("tbl")
	rs.MoveNext
	
	if closebit ="2" then
		btnname = "btnAlert_onClick"
	elseif closebit ="0" or closebit ="1" then
		btnname = "btnFTP_onClick"	
		'loop 1 start -------------------------------------------------    
		Set fso = CreateObject("Scripting.FileSystemObject")        
		Set fsoini = fso.CreateTextFile("d:\CBNWEB\WebAP\misFTP\AvsFtp\ftp.ini", True)
		fsoini.WriteLine("ebt")
		fsoini.WriteLine("ebtlogbak")
	    'fsoini.WriteLine("lcd d:\CBNWEB\WebAP\misFTP\AvsFtp")					   		
		'loop 2 start -------------------------------------------------
		'iCount = 0
		tbls = ""
		While not rs.EOF
			tble = rs("tbl")
			if tble <> tbls then 
				if  tbls <>"" then fsoText.Close
				ftpFile = tble & ".txt"
					if len(tble) = 17 then
					   fsoini.WriteLine("lcd d:\CBNWEB\WebAP\misFTP\AvsFtp\Building\EBT")
					   fsoini.WriteLine("cd /avs/Building/EBT")
					   ftpPathFile = Server.MapPath("/WebAP/misFtp/AvsFtp/Building/EBT") & "\" & ftpFile
					elseif len(tble) = 13 then
					   fsoini.WriteLine("lcd d:\CBNWEB\WebAP\misFTP\AvsFtp\ADSL\EBT")
					   fsoini.WriteLine("cd /avs/ADSL/EBT")
					   ftpPathFile = Server.MapPath("/WebAP/misFtp/AvsFtp/ADSL/EBT") & "\" & ftpFile
					elseif len(tble) = 16 then
					   fsoini.WriteLine("lcd d:\CBNWEB\WebAP\misFTP\AvsFtp\AVS\EBT")					
					   fsoini.WriteLine("cd /avs/AVS/EBT")
					   ftpPathFile = Server.MapPath("/WebAP/misFtp/AvsFtp/AVS/EBT") & "\" & ftpFile	
					end if   
				fsoini.WriteLine("put " & ftpFile)		   

				Set fsoText = fso.CreateTextFile(ftpPathFile, True)
				tbls =tble
				Response.Write "<b><font color =""red"">" & rs("tbl") & "</font></b><br>"							
			end if    
			'iCount = iCount + 1
			fsoText.WriteLine(rs("FTPTEXT"))
			Response.Write replace(rs("ftptext"),"!","<font color=""red"">!</font>") & "<br>"			
			rs.MoveNext
		Wend
		if tbls <>"" then fsoText.Close    
		rs.Close
		conn.Close
		Set rs=Nothing
		Set conn=Nothing
		'loop 2 end --------------------------------------------------
		fsoini.WriteLine("bye")						
		fsoini.Close
		'loop 1 end --------------------------------------------------    
    end if
%>

<HTML>
	<HEAD>
		<title>東森AVS申報/報竣轉檔作業</title>
		<SCRIPT language="VBScript">    
	    sub window_onload
	        'window.opener.close
	    end sub
	    
		Sub btnFtp_onClick
			set wHandle = window.open ("/WebAP/misFtp/AvsFtp/ftp.shtml", "win1")						
		end sub

		Sub btnAlert_onClick
		    Alert("已結超過一天，無法上傳 !!")
		end sub

		Sub btnExit_onClick
		    window.close
		end sub
		</SCRIPT>
	</HEAD>
	<BODY bgcolor="lightblue">
		<br><P><center><br>
				<INPUT name="btnSure" type="button" value="FTP上傳" ID="Button1" onclick="<%=btnname%>"> 
				<INPUT name="btnExit" type="button" value="離開" ID="Button3">
				<input type=hidden name="hdFtpFile" value="<%=ftpFile%>" ID="Hidden1">
		</center></P>
		<hr>
	</BODY>
</HTML>
