<%
	Dim rs,i,conn
	Set conn=Server.CreateObject("ADODB.Connection")
	Set rs=Server.CreateObject("ADODB.Recordset")
	conn.open "DSN=RTLib"

	'----------揭祙
	S6=""
	SQLXX="SELECT CODE, CODENC FROM RTCode WHERE KIND = 'P1' "
	rs.Open SQLXX,CONN
	s6="<option value="";场"" selected>场</option>" &vbCrLf
	Do While Not rs.Eof
		s6=s6 &"<option value=""" &rs("CODE") & ";" & rs("CODENC") & """>" &rs("CODENC") &"</option>"
		rs.MoveNext
	Loop
	rs.Close

	conn.Close
	Set rs=Nothing
	Set conn=Nothing
%>
<html>
<head>
	<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
	<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
	
	<script language="VBScript">
		Sub btn_onClick()
			dim aryStr,s,t,r

			S1=document.all("search1").value  
			If Len(s1)=0 Or s1="" Then
				t=t &" a.INVNO<> '*' "
			Else
				s=s &" 祇布腹絏:('" &S1& "'じ)"
				t=t &" a.INVNO LIKE '%" &S1& "%' " 
			End If

			S9=document.all("search9").value
			S10=document.all("search10").value
			IF LEN(TRIM(S9)) > 0 OR LEN(TRIM(S10)) > 0 THEN
				IF LEN(TRIM(S9))=0 THEN
					S9="1900/01/01 00:00:00"
				END IF
				IF LEN(TRIM(S10))=0 THEN
					S10="9999/12/31 11:59:59"
				END IF
				s=s &"  祇布ら戳:( " &S9 & "  " & S10 & " )"
				t=t &" AND a.INVDAT between '" & S9 & "' AND '" & S10 & "' "
	  		END IF

			S2=document.all("search2").value  
			If Len(TRIM(s2)) > 0 Then
				s=s &"  祇布╋繷:('" &S2 & "'じ)"
				t=t &" AND a.INVTITLE LIKE '%" &S2 &"%' " 
			End If

			S3=document.all("search3").value  
				If Len(TRIM(s3)) > 0 Then
				s=s &"  そ参絪:('" & S3 & "'じ)"
				t=t &" AND a.UNINO LIKE '%" & S3 &"%' " 
			End If

			S4=document.all("search4").value  
			If Len(TRIM(s4)) > 0 Then
				s=s &"  羛计:('" &S4 & "'羛)"
				t=t &" AND INVTYPE = " &S4
			End If

			S6=SPLIT(document.all("search6").value,";")
			If Len(TRIM(s6(0))) > 0 Then
				s=s &"  揭祙:('" &S6(1) & "')"
				t=t &" AND b.CODE = '" &S6(0) &"' " 
			End If

			S12=document.all("search12").value
			S13=document.all("search13").value
			IF LEN(TRIM(S12)) > 0 OR LEN(TRIM(S13)) > 0 THEN
				IF LEN(TRIM(S12))=0 THEN
					S12="1900/01/01 00:00:00"
				END IF
				IF LEN(TRIM(S13))=0 THEN
					S13="9999/12/31 11:59:59"
				END IF
				s=s &"  祇布紀ら:( " &S12 & "  " & S13 & " )"
				t=t &" AND a.CANCELDAT between '" &S12 &"' AND '" & S13 & "' " 
			END IF

			Dim winP,docP
  			Set winP=window.Opener
  			Set docP=winP.document
	  		docP.all("searchQry").value=t
			docP.all("searchShow").value=s
  			docP.all("keyform").Submit
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

		Sub btn1_onClick()
			Dim winP
			Set winP=window.Opener
			winP.focus()
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
	</script>
</head>

	<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
			style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
			height="60" width="60" id="objEF2KDT" VIEWASTEXT>
		<PARAM NAME="_ExtentX" VALUE="1270">
		<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
	<body>
		<table width="100%">
			<tr class="dataListTitle" align="center"><td>祇布戈穓碝兵ン</td><tr>
		</table>
		<table width="100%" border="1" cellPadding="0" cellSpacing="0">
			<tr>
				<td class="dataListHead" width="40%">祇布腹絏</td>
				<td width="60%" bgcolor="silver">
					<input type="text" size="12" maxlength="10" name="search1" class="dataListEntry">
				</td>
			</tr>
			<tr>
				<td class="dataListHead" width="40%">祇布ら戳</td>
				<td width="60%" bgcolor="silver">
					<input type="text" name="search9" size="10" class="dataListentry"> <input type="button" id="B9" name="B9" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
					<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="睲埃" id="C9" name="C9" style="Z-INDEX: 1" border="0" onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
					<font size="3">°</font> <input type="text" name="search10" size="10" class="dataListentry">
					<input type="button" id="B10" name="B10" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
					<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="睲埃" id="C10" name="C10" style="Z-INDEX: 1" border="0" onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
				</td>
			</tr>
			<tr>
				<td class="dataListHead" width="40%">祇布╋繷</td>
				<td width="60%" bgcolor="silver">
					<input type="text" size="55" maxlength="50" name="search2" class="dataListEntry">
				</td>
			</tr>
			<tr>
				<td class="dataListHead" width="40%">そ参絪</td>
				<td width="60%" bgcolor="silver">
					<input type="text" size="10" maxlength="10" name="search3" class="dataListEntry">
				</td>
			</tr>
			<tr>
				<td class="dataListHead" width="40%">羛计</td>
				<td width="60%" bgcolor="silver">
					<input type="text" size="1" maxlength="2" name="search4" class="dataListEntry">
				</td>
			</tr>
			<tr>
				<td class="dataListHead" width="40%">揭祙</td>
				<td width="60%" bgcolor="silver">
					<select name="search6" class="dataListEntry">
						<%=S6%>
					</select>
				</td>
			</tr>
			<tr>
				<td class="dataListHead" width="40%">祇布紀ら</td>
				<td width="60%" bgcolor="silver">
					<input type="text" name="search12" size="10" class="dataListentry"> <input type="button" id="B12" name="B12" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
					<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="睲埃" id="Img1" name="C11" style="Z-INDEX: 1" border="0" onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
					<font size="3">°</font> <input type="text" name="search13" size="10" class="dataListentry">
					<input type="button" id="B13" name="B13" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
					<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="睲埃" id="Img2" name="C12" style="Z-INDEX: 1" border="0" onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
				</td>
			</tr>
		</table>
		<table width="100%" align="right">
			<tr>
				<TD></TD>
				<td align="right">
					<input type="SUBMIT" value=" 琩高 " class="dataListButton" name="btn" onsubmit="btn_onclick" style="cursor:hand">
					<input type="button" value=" 挡 " class="dataListButton" name="btn1" style="cursor:hand">
				</td>
			</tr>
		</table>
	</body>
</html>
