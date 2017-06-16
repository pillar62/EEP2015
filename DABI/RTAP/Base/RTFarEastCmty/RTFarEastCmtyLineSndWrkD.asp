<%  
  Dim fieldRole,fieldPa
  fieldRole=Split(FrGetUserRight("RTCustD",Request.ServerVariables("LOGON_USER")),";")
%>
<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/cType.inc" -->
<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/dataList.inc" -->
<%
  Dim aryKeyName,aryKeyType(150),aryKeyValue(150),numberOfField,aryKey,aryKeyNameDB(150)
  Dim dspKey(150),userDefineKey,userDefineData,extDBField,extDB(150),userDefineRead,userDefineSave
  Dim conn,rs,i,formatName,sqlList,sqlFormatDB,userdefineactivex
  Dim aryParmKey
 '90/09/03 ADD-START
 '增加EXTDBFIELD2,EXTDBFILELD3(多檔維護)
  dim extDBField2,extDB2(150),extDBField3,extDB3(150),extDBField4,extDB4(150)
  extDBfield2=0
  extDBField3=0
  extDBField4=0
 '----90/09/03 add-end
  extDBField=0
  aryParmKey=Split(Request("Key") &";;;;;;;;;;;;;;;",";")
' -------------------------------------------------------------------------------------------- 
  Call SrEnvironment
  Call SrGetFormat
  Call SrProcessForm
' -------------------------------------------------------------------------------------------- 
  Sub SrGetFormat()
    Dim rs,i,conn
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open DSN
    Set rs=Server.CreateObject("ADODB.Recordset")
    aryKeyName=Split(formatName,";")
    rs.Open sqlFormatDB,conn
    For i = 0 To rs.Fields.Count-1
      aryKeyNameDB(i)=rs.Fields(i).Name
      aryKeyType(i)=rs.Fields(i).Type
    Next
    numberOfField=rs.Fields.Count
    rs.Close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
  End Sub
' --------------------------------------------------------------------------------------------        
  Sub SrInit(accessMode,sw)
    Dim i
    aryKey=Split(";;;;;;;;;;",";")
    accessMode=Request.Form("accessMode")
    If accessMode="" Then
       accessMode=Request("accessMode")
       aryKey=Split(Request("key") &";;;;;;;;;;;;;;;;;;;;",";")
       For i = 0 To numberOfKey-1
           dspKey(i)=aryKey(i)
       Next
    End IF
    sw=Request("sw")
    reNew=Request("reNew")
    rwCnt=Request("rwCnt")
    if Not IsNumeric(rwCnt) Then rwCnt=0
  End Sub
' --------------------------------------------------------------------------------------------        
  Sub SrClearForm()
    Dim i,sType
    For i = 0 To Ubound(aryParmKey)
       If Len(Trim(aryParmKey(i))) > 0 Then
           dspKey(i)=aryParmKey(i)
        End If
    Next
'    For i = 0 To numberOfField-1
'        sType=Right("000" &Cstr(aryKeyType(i)),3) 
'        If Instr(cTypeChar,sType) > 0 Then
'           dspKey(i)=""
'        ElseIf Instr(cTypeNumeric,sType) > 0 Then
'           dspKey(i)=0
'        ElseIf Instr(cTypeDate,sType) > 0 Then
'           dspKey(i)=Now()
'        ElseIf Instr(cTypeBoolean,sType) > 0 Then
'           dspKey(i)=True
'        Else
'           dspKey(i)=""
'        End If
'    Next
  End Sub
' -------------------------------------------------------------------------------------------- 
  Sub SrReceiveForm
    Dim i
    For i = 0 To numberOfField-1
        dspKey(i)=Request.Form("key" &i)
    Next
    If extDBField > 0 Then
       For i = 0 To extDBField-1
           extDB(i)=Request.Form("ext" &i)
       Next
    End If
    If extDBField2 > 0 Then
       For i = 0 To extDBField2-1
           extDB2(i)=Request.Form("extA" &i)
       Next
    End If
    If extDBField3 > 0 Then
       For i = 0 To extDBField3-1
           extDB3(i)=Request.Form("extB" &i)
       Next
    End If        
    If extDBField4 > 0 Then
       For i = 0 To extDBField4-1
           extDB4(i)=Request.Form("extC" &i)
       Next
    End If            
  End Sub
' -------------------------------------------------------------------------------------------- 
  Sub SrCheckForm(message,formValid)
    formValid=True
    message=""
    Call SrCheckData(message,formValid)
  End Sub
' -------------------------------------------------------------------------------------------- 
  Function GetSql()
    Dim sql,i,sType
    sql=""
    For i = 0 To numberOfKey-1
      If i > 0 Then sql=sql &" AND "
      sType=Right("000" &Cstr(aryKeyType(i)),3)
      If Instr(cTypeChar,sType) > 0 Or dspKey(i)=Null Then  
         sql=sql &"[" &aryKeyNameDB(i) &"]='" &dspKey(i) &"'"
      'edson 2001/11/1 增加==>為了日期欄位能當key使用..必須有單引號
      elseIf Instr(cTypedate,sType) > 0 Or dspKey(i)=Null Then 
          sql=sql &"[" &aryKeyNameDB(i) &"]='" &dspKey(i) &"'"         
      Else
         sql=sql &"[" &aryKeyNameDB(i) &"]=" &dspKey(i)
      End If
    Next
    GetSql=sqlList &sql &";"
  '  response.write getsql
  End Function
' -------------------------------------------------------------------------------------------- 
  Sub SrSaveData(message)
    message=msgSaveOK
    Dim sql,i,sType
    sql=GetSql()
    Dim conn,rs
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open DSN
    Set rs=Server.CreateObject("ADODB.Recordset")
   ' RESPONSE.WRITE SQL
    rs.Open sql,conn,3,3
    If rs.Eof Or rs.Bof Then
       If accessMode="A" Then
          rs.AddNew
          For i = 0 To numberOfField-1
              sType=Right("000" &Cstr(aryKeyType(i)),3)
              If Instr(cTypeNumeric,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=0
			  If Instr(cTypeChar,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=""
              If Instr(cTypeDate,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=Null
				if i=0 then
					Set rsc=Server.CreateObject("ADODB.Recordset")
					cusidxx="S" & right(datePART("yyyy",NOW()),2) & right("0" & trim(datePART("m",NOW())),2)& right("0" & trim(datePART("d",NOW())),2)
					rsc.open "select max(wrkno) AS maxwrkno from RTfareastSndWrk where wrkno LIKE '" & cusidxx & "%' " ,conn
					if len(trim(rsc("maxwrkno"))) > 0 then 
						dspkey(i)=cusidxx & right("0" & cstr(cint(right(rsc("maxwrkno"),2)) + 1),2)
					else
						dspkey(i)=cusidxx & "01"
					end if
					rsc.close
				end if
'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
				rs.Fields(i).Value=dspKey(i)
          Next
          rs.Update
          rwCnt=rwCnt+1
          If userDefineSave="Yes" Then Call SrSaveExtDB("A")
       Else
          message=msgErrorRec
       End If
    Else
       If accessMode="A" Then
          message=msgDupKey
          sw="E"
       Else
          For i = 0 To numberOfField-1
              sType=Right("000" &Cstr(aryKeyType(i)),3)
              If Instr(cTypeNumeric,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=0
			  If Instr(cTypeChar,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=""
              If Instr(cTypeDate,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=Null
'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
              rs.Fields(i).Value=dspKey(i)
          Next
          rs.Update
          rwCnt=rwCnt+1
          If userDefineSave="Yes" Then Call SrSaveExtDB("U")
          sw=""
       End If
    End If
    rs.Close
    ' 將sql自行產生之identify值dspkey(0)讀出至畫面
    if accessmode ="A" then
		'runpgm=Request.ServerVariables("PATH_INFO")
        cusidxx="S" & right(datePART("yyyy",NOW()),2) & right("0" & trim(datePART("m",NOW())),2)& right("0" & trim(datePART("d",NOW())),2)
		Set rsc=Server.CreateObject("ADODB.Recordset")
		rsc.open "select max(wrkno) AS maxwrkno from RTfareastSndWrk where wrkno LIKE '" & cusidxx & "%' " ,conn
        if not rsC.eof then
			dspkey(0) = rsC("maxwrkno")
			dspkey(1) =	aryParmKey(1)
			dspkey(2) =	aryParmKey(2)
        end if
        rsC.close
    end if
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
  End Sub
' -------------------------------------------------------------------------------------------- 
  Sub SrReadData(dataFound)
    dataFound=True
    Dim  sql,i
    sql=GetSql()
    Dim conn,rs
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open DSN
    Set rs=Server.CreateObject("ADODB.Recordset")
    'response.write "SQL=" & SQL
    rs.Open sql,conn
    If rs.Eof Then
       dataFound=False
    Else
       For i = 0 To numberOfField-1
           dspKey(i)=rs.Fields(i).Value
       Next
       If userDefineRead="Yes" Then Call SrReadExtDB()
    End If
    rs.Close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
  End Sub
' -------------------------------------------------------------------------------------------- 
  Sub SrSendForm(message)
      Dim s,i,t,sType
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4EBT/DBAUDI/dataList.css" TYPE="text/css">
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<script language="vbscript">
Sub Window_onLoad()
  window.Focus()
  SrWrkTypChange()
End Sub
Sub Window_onbeforeunload()
  dim rwCnt
  rwCnt=document.all("rwCnt").value
  If IsNumeric(rwCnt) Then
     If rwCnt > 0 Then Window.Opener.document.all("keyForm").Submit
  End If
  Window.Opener.Focus()
End Sub
Sub SrReNew()
  document.all("sw").Value="E"
  document.all("reNew").Value="Y"
  Window.form.Submit
End Sub
</script>
</head>
<% if userdefineactivex="Yes" then
      SrActiveX
      SrActiveXScript
   End if
%>
<body bgcolor="#FFFFFF" text="#0000FF"  background="backgroup.jpg" bgproperties="fixed">
<form method="post" id="form">
<input type="text" name="sw" value="<%=sw%>" style="display:none;" ID="Text17">
<input type="text" name="reNew" value="N" style="display:none;" ID="Text18">
<input type="text" name="rwCnt" value="<%=rwCnt%>" style="display:none;" ID="Text19">
<input type="text" name="accessMode" value="<%=accessMode%>" style="display:none;" ID="Text20">
<table width="100%" ID="Table1">
  <tr class=dataListTitle><td width="20%">&nbsp;</td><td width="60%" align=center>
<%=title%></td><td width="20%" align=right><%=dspMode%></td></tr>
</table>
<%
  s=""
  If userDefineKey="Yes" Then
     s=s &"<table width=""100%"" cellPadding=0 cellSpacing=0>" &vbCrLf _
         &"  <tr><td width=""70%"">" &vbCrLf 
     Response.Write s
     SrGetUserDefineKey()
     s=""
     s=s &"      </td>" &vbCrLf _
         &"      <td width=""30%"">" &vbCrLf _
         &"          <table width=""100%"" cellPadding=0 cellSpacing=0>" &vbCrLf _
         &"            <tr><td class=dataListMessage>" &message &"</td></tr>" &vbCrLf _
         &"            <tr align=""right""><td>&nbsp;</td><td align=""right"">" &strBotton &"</td></tr>" &vbCrLf _
         &"          </table></td></tr>" &vbCrLf _
         &"</table>" &vbCrLf
     Response.Write s
  Else 
     s=s &"<table width=""100%"">" &vbCrLf _
         &"  <tr><td width=""60%"">" &vbCrLf _
         &"      <table width=""100%"">" &vbCrLf 
     For i = 0 To numberOfKey-1
	 s=s &"       <tr><td width=""30%"" class=dataListHead>" &aryKeyName(i) &"</td>" _
          &"<td width=""70%"">" _
          &"<input class=dataListEntry type=""text"" name=""key" &i &""" " &keyProtect _
          &" size=""20"" value=""" &dspKey(i) &"""></td></tr>" &vbCrLf
     Next
     s=s &"      </table></td>" &vbCrLf _
         &"      <td width=""40%"">" &vbCrLf _
         &"          <table width=""100%"">" &vbCrLf _
         &"            <tr><td class=dataListMessage>" &message &"</td></tr>" &vbCrLf _
         &"            <tr><td>&nbsp;</td></tr>" &vbCrLf _
         &"            <tr><td>" &strBotton &"</td></tr>" &vbCrLf _
         &"          </table></td></tr>" &vbCrLf _ 
         &"</table>" &vbCrLf
     Response.Write s
  End If
  s=""
  If userDefineData="Yes" Then
     SrGetUserDefineData()
  Else
     s="<table width=""100%"">" &vbCrLf
     For i = numberOfKey To numberOfField-1
       sType=Right("000" &Cstr(aryKey(i)),3)
       s=s &"  <tr><td width=""20%"" class=dataListHead>" &aryKeyName(i) &"</td>" &vbCrLf _
           &"      <td width=""80%"">" &vbCrLf
       If Instr(cTypeVarChar,sType) > 0 Then
         s=s &"      <textarea class=dataListEntry name=""key" &i &""" rows=""4"" cols=""40"" istextedit " _
             &dataProtect &" style=""text-align:left;"">" &dspKey(i) &"</textarea></td></tr>" &vbCrLf 
       ElseIf Instr(cTypeFloat,sType) > 0 Then
         s=s &"      <input class=dataListEntry type=""text"" name=""key" &i &""" size=""40"" " _ 
             &dataProtect &" style=""text-align:right;"" " _
             &"value=""" &FormatNumber(dspKey(i)) &"""></td></tr>" &vbCrLf
       ElseIf Instr(cTypeInteger,sType) > 0 Then 
         s=s &"      <input class=dataListEntry type=""text"" name=""key" &i &""" size=""40"" " _ 
             &dataProtect &" style=""text-align:right;"" " _
             &"value=""" &FormatNumber(dspKey(i),0) &"""></td></tr>" &vbCrLf
       Else
         s=s &"      <input class=dataListEntry type=""text"" name=""key" &i &""" size=""40"" " _ 
             &dataProtect &" style=""text-align:left;"" " _
             &"value=""" &dspKey(i) &"""></td></tr>" &vbCrLf
       End If
     Next
     s=s &"</table>" &vbCrLf
     Response.Write s
  End If
%>
</form>
</body>
</html>
<%End Sub%>
<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=1
  title="主線派工資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB=	"SELECT WRKNO, COMQ1, LINEQ1, CUSID, WRKTYP, IMPNO, IMPDAT, SNDWRKUSR, " &_
				"SNDWRKDAT, SNDMEMO, ASSIGNENG, ASSIGNCONS, ASSIGNDAT, " &_
				"ASSIGNTIME, FINISHENG, FINISHCONS, FINISHDAT, FINISHTYP, FINISHMEMO, " &_
				"CLOSEUSR, CLOSEDAT, UUSR, UDAT, CANCELUSR, CANCELDAT, " &_
				"WORKINGDAT, LINEDROPTYP, ISPTEL " &_
				"FROM RTfareastSndWrk " &_
				"WHERE WRKTYP ='' "
  sqlList=		"SELECT WRKNO, COMQ1, LINEQ1, CUSID, WRKTYP, IMPNO, IMPDAT, SNDWRKUSR, " &_
				"SNDWRKDAT, SNDMEMO, ASSIGNENG, ASSIGNCONS, ASSIGNDAT, " &_
				"ASSIGNTIME, FINISHENG, FINISHCONS, FINISHDAT, FINISHTYP, FINISHMEMO, " &_
				"CLOSEUSR, CLOSEDAT, UUSR, UDAT, CANCELUSR, CANCELDAT, " &_
				"WORKINGDAT, LINEDROPTYP, ISPTEL " &_
				"FROM RTfareastSndWrk " &_
				"WHERE "
  userDefineRead="Yes"      
  userDefineSave="Yes"       
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=99
  userdefineactivex="Yes"
End Sub

' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
	'完工方式
	IF len(trim(dspkey(17)))=0 then dspkey(17)="01"
	'派工別非社區撤線
	if trim(dspkey(4)) <>"07" then
		dspkey(26) =""
	end if 

  if len(trim(dspkey(4)))=0 then
       formValid=False
       message="[派工別]不可空白"
  elseif dspkey(4)="10" and len(trim(dspkey(25)))=0 then
       formValid=False
       message="[預定到位日]不可空白"
  elseif dspkey(4)="07" and len(trim(dspkey(26)))=0 then
       formValid=False
       message="[撤線種類]不可空白"
  elseif dspkey(4)="07" and len(trim(dspkey(25)))=0 then
       formValid=False
       message="[預定撤線日]不可空白"
  elseif len(trim(dspkey(10)))=0 and len(trim(dspkey(11)))=0 then
       formValid=False
       message="[預定施工人員or經銷商]至少需輸入一項"
  elseif len(trim(dspkey(10)))>0 and len(trim(dspkey(11)))>0 then
       formValid=False
       message="[預定施工人員or經銷商]不可同時輸入"
  elseif len(trim(dspkey(14)))>0 and len(trim(dspkey(15)))>0 then
       formValid=False
       message="[實際施工人員or經銷商]不可同時輸入"
  end if

  '檢查主線檔狀態︰已撤線or已作廢則不可建立派工單
   Set connxx=Server.CreateObject("ADODB.Connection")
   Set rsxx=Server.CreateObject("ADODB.Recordset")
   connxx.open DSN
   sqlxx="select dropdat, canceldat from RTfareastCmtyLine where comq1 =" & aryparmkey(1) & " and lineq1 =" & aryparmkey(2)
   rsxx.Open sqlxx,connxx
   if not rsxx.eof then
     IF NOT ISNULL(RSXX("dropdat")) THEN
        formValid=False
        message="主線已撤線，不可建立派工單" 
     ELSEIF NOT ISNULL(RSXX("canceldat")) THEN
        formValid=False
        message="主線檔已作廢，不可建立派工單" 
     END IF
   end if
   rsxx.close
   connxx.Close   
   set rsxx=Nothing   
   set connxx=Nothing 

'-------UserInformation----------------------
    logonid=session("userid")
    if dspmode="新增" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(21)=V(0)
        dspkey(22)=now()
    end if
    if dspmode="修改" then
    	'資料異動者
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(21)=V(0)
        dspkey(22)=now()
    end if        
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveXScript()%>
   <SCRIPT Language="VBScript">
   Sub Srbtnonclick()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="KEY" & clickid
	   if isdate(document.all(clickkey).value) then
	      objEF2KDT.varDefaultDateTime=document.all(clickkey).value
       end if
       call objEF2KDT.show(1)
       if objEF2KDT.strDateTime <> "" then
          document.all(clickkey).value = objEF2KDT.strDateTime
       end if
   END SUB

   Sub SrClear()
       Dim clickkey
       clickkey="KEY" & mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       if len(trim(document.all(clickkey).value)) <> 0 then
          	document.all(clickkey).value = ""
   			if clearkey = "KEY10" then
      			document.all("colAssigneng").value = ""
   			elseif clearkey = "KEY11" then
      			document.all("colAssigncons").value = ""
   			elseif clearkey = "KEY14" then
      			document.all("colFinisheng").value = ""
  			elseif clearkey = "KEY15" then
      			document.all("colFinishcons").value = ""
			end if
       end if
   End Sub

   Sub Srsalesonclick()
       Dim prog, clickkey
   	   prog="/Webap/include/RTGetSalesD.asp"
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
	       FUsrID=Split(Fusr,";")
	       if Fusrid(3) ="Y" then
				clickkey="KEY" & mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
          		document.all(clickkey).value = trim(Fusrid(0))
       			if clickkey = "KEY10" then
          			document.all("colAssigneng").value =  trim(Fusrid(1))
       			elseif clickkey = "KEY14" then
          			document.all("colFinisheng").value =  trim(Fusrid(1))
				end if
	       End if
       end if
   End Sub

   Sub SrConsOnClick()
       Dim prog, clickkey
		prog="/Webap/include/RTGetConsD.asp"
		FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
			FUsrID=Split(Fusr,";")
       		if Fusrid(3) ="Y" then
       			clickkey="KEY" & mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
          		document.all(clickkey).value = trim(Fusrid(0))
       			if clickkey = "KEY11" then
          			document.all("colAssigncons").value =  trim(Fusrid(1))
       			elseif clickkey = "KEY15" then
          			document.all("colFinishcons").value =  trim(Fusrid(1))
				end if
       		End if
       end if
   End Sub

	Sub SrWrkTypChange()
		IF document.all("key4").value ="07" THEN		'社區撤線
		  document.all("colWorkingDat").value = "預定撤線日"
		  window.tdWorkingDat.colspan =1
		  window.tdLineDropTyp.style.display=""
		  window.tdLineDropTypNC.style.display=""
		  window.trWrkTypAbout.style.display=""
		ELSEIF document.all("key4").value ="10" THEN	'主線測通
		  document.all("colWorkingDat").value = "預定到位日"
		  window.tdWorkingDat.colspan=5
		  window.tdLineDropTyp.style.display="none"
		  window.tdLineDropTypNC.style.display="none"
		  window.trWrkTypAbout.style.display=""
		ELSE
			window.trWrkTypAbout.style.display="none"
		end if
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

   </Script>
<%   
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveX() %>
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"   codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<%	
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()%>
	<table width="100%" border=1 cellPadding=0 cellSpacing=0>
		<tr><td width="15%" class=dataListHead>派工單號</td>
			<td width="35%"  bgcolor="silver">
				<input type="text" name="key0" readonly size="10" value="<%=dspKey(0)%>" class=dataListdata>
			</td>
			<td width="15%" class=dataListHead>主線序號</td>
			<td width="35%"  bgcolor="silver">
				<input type="text" name="key1" readonly size="5" value="<%=dspKey(1)%>" class=dataListdata ID="Text1">
				<font color=blue>-</font>
				<input type="text" name="key2" readonly size="3" value="<%=dspKey(2)%>" class=dataListdata ID="Text2">
			</td>
		</tr>
	</table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
		'if len(trim(dspkey(21))) < 1 then
		Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                'dspkey(21)=V(0)
			   	dspkey(7)=V(0)		'派工員
        'End if  
       'dspkey(22)=now()
       dspkey(8)=datevalue(now())	'派工日
    'else
        'if len(trim(dspkey(16))) < 1 then
        '   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
        '        V=split(rtnvalue,";")  
        '        DSpkey(16)=V(0)
        'End if         
        'dspkey(17)=now()
    end if      
' -------------------------------------------------------------------------------------------- 

    '派工單結案後 protect
    If len(trim(dspKey(20))) > 0  Then
       fieldPa=" class=""dataListData"" readonly "
       fieldpb=" disabled "
    Else
       fieldPa=""
       fieldpb=""
    end if
%>

<span id="tags1" class="dataListTagsOn">派工資訊</span>
                                                            
<div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td>
<td>     
<%
	sql="select isnull(d.shortnc, f.cusnc) as belongnc, c.comn, g.cutnc+a.township+a.raddr as addr, a.linetel, a.lineip, " &_
		"substring(h.codenc,1,2) as lineisp, i.codenc as lineiptype, j.codenc as linerate, a.hardwaredat, c.salesid, c.consignee " &_
		"from RTfareastCmtyLine a " &_
		"inner join RTfareastCmtyH c on c.comq1 = a.comq1 " &_
		"left outer join RTObj d on c.consignee = d.cusid " &_
		"left outer join RTEmployee e inner join RTObj f on e.cusid = f.cusid on c.salesid = e.emply " &_
		"left outer join RTCounty g on a.cutid=g.cutid " &_
		"left outer join RTCode h on a.lineisp = h.code and h.kind = 'c3' " &_
		"left outer join RTCode i on a.lineiptype = i.code and i.kind = 'm5' " &_
		"left outer join RTCode j on a.linerate = j.code and j.kind = 'd3' " &_
		"where a.comq1 =" &aryparmkey(1)& " and a.lineq1 =" &aryparmkey(2)
	'response.Write "sql=" &sql

    Dim conn,rs,s,sx,sql,t
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
	rs.OPEN sql,CONN
	IF RS.EOF THEN
		belongnc =""	:	comn =""	    :	addr =""
		linetel =""		:	lineip =""		:	lineisp =""
		lineiptype =""	:	linerate =""	:	hardwaredat =""
		salesid=""		:	consignee=""
	ELSE
		belongnc =rs("belongnc")		:	comn = rs("comn")			:	addr = rs("addr")
		linetel = rs("linetel")			:	lineip = rs("lineip")		:	lineisp = rs("lineisp")
		lineiptype = rs("lineiptype")	:	linerate = rs("linerate")	:	hardwaredat	= rs("hardwaredat")
		salesid =rs("salesid")			:	consignee =rs("consignee")
	END IF
	RS.CLOSE
%>
<DIV ID="SRTAG0">
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
	<tr><td bgcolor="BDB76B" align="center">主線基本資料</td></tr>
</table>
</DIV>

<DIV ID=SRTAB0 >
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3">
	<tr><td width="10%" class=dataListsearch>社區名稱</td>
		<td width="23%" bgcolor="silver" colspan=3 >
			<input type="text" readonly size="30" name="colComn" value="<%=comn%>" class="dataListsearch3" ID="Text27">
		</td>
		<td width="10%" class=dataListsearch>轄區業務</td>
		<td width="23%" bgcolor="silver">
			<input type="text" readonly size="10" name="colBelongnc" value="<%=belongnc%>" class="dataListsearch3" ID="Text15">
		</td>
	</tr>
	<tr><td class=dataListsearch>社區地址</td>
		<td bgcolor="silver" colspan=5>
			<input type="text" readonly size="100" name="colAddr" value="<%=addr%>" class="dataListsearch3" ID="Text42">
		</td>
	</tr>

	<tr><td width="10%" class=dataListsearch>附掛電話</td>
		<td width="23%" bgcolor="silver">
			<input type="text" readonly size="15" name="colLinetel" value="<%=linetel%>" class="dataListsearch3" ID="Text16">
		</td>
	    <td width="10%" class=dataListsearch>社區IP</td>
		<td width="23%" bgcolor="silver">
			<input type="text" readonly size="20" name="colLineip" value="<%=lineip%>" class="dataListsearch3" ID="Text6">
		</td>
	    <td width="10%" class=dataListsearch>主線ISP</td>
		<td width="23%" bgcolor="silver">
			<input type="text" readonly size="10" name="colLineisp" value="<%=lineisp%>" class="dataListsearch3" ID="Text22">
		</td>
	</tr>

	<tr><td width="10%" class=dataListsearch>線路IP種類</td>
		<td width="23%" bgcolor="silver">
			<input type="text" readonly size="10" name="colLineiptype" value="<%=lineiptype%>" class="dataListsearch3" ID="Text52">
		</td>
	    <td width="10%" class=dataListsearch>主線速率</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" readonly size="10" name="colLinerate" value="<%=linerate%>" class="dataListsearch3" ID="Text23">
		</td>
		<td width="10%" class=dataListsearch>線路到位日</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" readonly size="12" name="colHardwaredat" value="<%=hardwaredat%>" class="dataListsearch3" ID="Text24">
		</td>
	</tr>	
</table>
</DIV>

<DIV ID="SRTAG2">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
		<tr><td bgcolor="BDB76B" align="center">派工資料</td></tr>
    </table>
</DIV>

<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3">
	<tr><td colspan=4>
	<!-- 客戶代號 -->
	<input type="text" name="key3" value="<%=dspKey(3)%>" style="display:none;" ID="Text3">
	<!-- FTP匯入資料ID -->
	<input type="text" name="key5" value="<%=dspKey(5)%>" style="display:none;" ID="Text5">
	<!-- FTP匯入日 -->
	<input type="text" name="key6" value="<%=dspKey(6)%>" style="display:none;" ID="Text6">
	<!-- 完工方式 -->
	<input type="text" name="key17" value="<%=dspKey(17)%>" style="display:none;" ID="Text17">
	</td></tr>

	<tr><td width="10%" class=dataListHEAD>派工別</td>
		<%
			s=""
			sx=" selected "
			If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 and len(trim(dspkey(20)))=0 Then  
			sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='P6' and parm1 like ('%Sonet主線%') " 
			If len(trim(dspkey(4))) < 1 Then
				sx=" selected " 
				s=s & "<option value=""""" & sx & "></option>"  
				sx=""
			else
				s=s & "<option value=""""" & sx & "></option>"  
				sx=""
			end if     
			Else
			sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='P6' AND CODE='" & dspkey(4) & "'"
			End If
			rs.Open sql,conn
			Do While Not rs.Eof
			If rs("CODE")=dspkey(4) Then sx=" selected "
			s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
			rs.MoveNext
			sx=""
			Loop
			rs.Close
		%>
		<td width="23%" bgcolor="silver">
			<select size="1" name="key4" onChange="SrWrkTypChange()" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select1" >                                                                  
				<%=s%>
			</select>
		</td>
		<td width="10%" class="dataListHEAD" height="23">派工日</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" name="key8" size="12" READONLY value="<%=dspKey(8)%>"  class="dataListDATA" ID="Text9">
		</td> 

		<td width="10%" class="dataListHEAD" height="23">派工人</td>
		<td width="23%" bgcolor="silver">
			<input type="text" name="key7" size="6" READONLY value="<%=dspKey(7)%>" class="dataListDATA">
			<font size=2><%=SrGetEmployeeName(dspKey(7))%></font>
		</td>
	</tr>

	<%
	    if dspkey(4) = "07" then 
	       WorkingNC="預定撤線日"
	       stWrkTypAbout=""
	       stWorkingDat ="1"
	       stLineDropTypNC=""
	       stLineDropTyp=""
	    elseif dspkey(4) = "10" then 
	       WorkingNC="預定到位日"
	       stWrkTypAbout=""
	       stWorkingDat ="5"
	       stLineDropTypNC="display:none;"
	       stLineDropTyp="display:none;"
		else
	       stWrkTypAbout="display:none;"
	    end if
    %>
	<tr id="trWrkTypAbout" style="<%=stWrkTypAbout%>"><td width="10%" class=dataListHEAD>
			<input type="text" readonly size="10" name="colWorkingdat" value="<%=WorkingNC%>" style="color:white;border:0px;background:transparent;">
		</td>
		<td bgcolor="silver" id ="tdWorkingDat" colspan="<%=stWorkingDat%>">
			<input type="text" name="key25" <%=fieldpa%> <%=fieldRole(1)%><%=dataProtect%> value="<%=dspKey(25)%>" READONLY size="12" class=dataListEntry>
			<input type="button" name="B25" id="B25"  <%=fieldpb%> height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C25" name="C25" onclick="SrClear()" <%=fieldpb%> style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
		</td>

		<td WIDTH="10%" class="dataListHEAD" id ="tdLineDropTypNC" style="<%=stLineDropTypNC%>">撤線種類</td>
		<%
		    s=""
		    sx=" selected "
		    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  and len(trim(dspkey(20)))=0  Then  
		       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='N9' " 
		       If len(trim(dspkey(26))) < 1 Then
		          sx=" selected " 
		          s=s & "<option value=""""" & sx & "></option>"  
		          sx=""
		       else
		          s=s & "<option value=""""" & sx & "></option>"  
		          sx=""
		       end if     
		    Else
		       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='N9' AND CODE='" & dspkey(26) & "'"
		    End If
		    rs.Open sql,conn
		    Do While Not rs.Eof
		       If rs("CODE")=dspkey(26) Then sx=" selected "
		       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
		       rs.MoveNext
		       sx=""
		    Loop
		    rs.Close
		%>
        <td WIDTH="23%" bgcolor="silver" colspan=3 id ="tdLineDropTyp" style="<%=stLineDropTyp%>">
		   <select size="1" name="key26" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35"><%=s%></select>
        </td>
	</tr>

	<tr>
		<td width="10%" class=dataListHEAD>預定施工人員</td>
		<td width="23%" bgcolor="silver" >
			<% If accessMode="A" And sw="" Then dspKey(10) =salesid %>
			<input type="TEXT" name="key10" value="<%=dspKey(10)%>" size="6" readonly <%=fieldpa%> class="dataListEntry" ID="Text50">
			<input type="BUTTON" id="B10" name="B10" width="100%" onclick="Srsalesonclick()" <%=fieldpb%> style="Z-INDEX: 1"  value="...." >   
			<IMG alt="清除" id="C10" name="C10" onclick="SrClear" <%=fieldpb%> SRC="/WEBAP/IMAGE/IMGDELETE.GIF" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
			<input type="text" readonly size="10" name="colAssigneng" value="<%=SrGetEmployeeName(dspKey(10))%>" class="dataListsearch3" ID="Text21">
        </td>

		<td width="10%" class=dataListHEAD>預定施工經銷商</td>
		<td width="23%" bgcolor="silver">
			<% If accessMode="A" And sw="" Then  dspKey(11) =consignee %>
			<input type="password"  name="key11" value="<%=dspKey(11)%>" size="6" readonly <%=fieldpa%> class="dataListEntry" ID="Text51">
			<input type="BUTTON" id="B11" name="B11" width="100%" onclick="SrConsOnClick()" <%=fieldpb%> style="Z-INDEX: 1"  value="...." >   
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C11" name="C11" onclick="SrClear" <%=fieldpb%> style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
			<input type="text" readonly size="10" name="colAssigncons" value="<%=SrGetShortncName(dspKey(11))%>" class="dataListsearch3" ID="Text21">
        </td>

		<td width="10%" class=dataListHEAD>預定施工時間</td>
		<td width="23%" bgcolor="silver">
        	<input type="text" name="key12" <%=fieldpa%> <%=fieldRole(1)%><%=dataProtect%> value="<%=dspKey(12)%>" READONLY size="12" class=dataListEntry>
       		<input type="button" name="B12" id="B12"  <%=fieldpb%> height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C12" name="C12" onclick="SrClear" <%=fieldpb%> style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
			<%
				s=""
				sx=" selected "
				If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 and len(trim(dspkey(20)))=0 Then  
				sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='Q4' " 
				If len(trim(dspkey(13))) < 1 Then
					sx=" selected " 
					s=s & "<option value=""""" & sx & "></option>"  
					sx=""
				else
					s=s & "<option value=""""" & sx & "></option>"  
					sx=""
				end if     
				Else
				sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='Q4' AND CODE='" & dspkey(13) & "'"
				End If
				rs.Open sql,conn
				Do While Not rs.Eof
				If rs("CODE")=dspkey(13) Then sx=" selected "
				s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
				rs.MoveNext
				sx=""
				Loop
				rs.Close
			%>
			<select size="1" name="key13" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select1" ><%=s%></select>
        </td>
	</tr>
	<tr><td width="15%" class=dataListHEAD>工班電話</td>
	    <td width="35%" bgcolor="silver" colspan="5">
	        <input type="text" maxlength="30" size="35" name="key27" value="<%=dspKey(27)%>" <%=fieldRole(1)%><%=dataProtect%> class=dataListEntry>
	    </td>
	</tr>
	<tr><td class="dataListHEAD" height="23">結案人</td>
		<td height="23" bgcolor="silver">
			<input type="text" name="key19" size="6" READONLY value="<%=dspKey(19)%>" class="dataListDATA">
			<font size=2><%=SrGetEmployeeName(dspKey(19))%></font>
		</td>
        <td  class="dataListHEAD" height="23">結案日</td>
        <td  height="23" bgcolor="silver" colspan=3>
	        <input type="text" name="key20" size="25" READONLY value="<%=dspKey(20)%>" class="dataListDATA" ID="Text11">
        </td>
	</tr>

	<tr><td  class="dataListHEAD" height="23">異動人員</td>
        <td  height="23" bgcolor="silver">
			<input type="text" name="key21" size="6" READONLY value="<%=dspKey(21)%>" class="dataListDATA" ID="Text10">
			<font size=2><%=SrGetEmployeeName(dspKey(21))%></font>
        </td>
        <td  class="dataListHEAD" height="23">異動時間</td>
        <td  height="23" bgcolor="silver" colspan=3>
	        <input type="text" name="key22" size="25" READONLY value="<%=dspKey(22)%>" class="dataListDATA" ID="Text11">
        </td>
	</tr>

	<tr><td  class="dataListHEAD" height="23">作廢人員</td>
        <td  height="23" bgcolor="silver">
			<input type="text" name="key23" size="6" READONLY value="<%=dspKey(23)%>" style="color:red;" class="dataListDATA" ID="Text12">
			<font size=2 color=red><%=SrGetEmployeeName(dspKey(23))%></font>
        </td>
        <td  class="dataListHEAD" height="23">作廢時間</td>
        <td  height="23" bgcolor="silver" colspan=3>
	        <input type="text" name="key24" size="25" READONLY value="<%=dspKey(24)%>" style="color:red;" class="dataListDATA" ID="Text13">
        </td>
	</tr>	
	<tr><td class="dataListHEAD">派工備註</td>
		<td bgcolor="silver" colspan=5 align=center>
			<TEXTAREA cols="100%" name="key9" rows=5 MAXLENGTH=800 <%=fieldpa%> class="dataListentry" <%=dataprotect%> value="<%=dspkey(9)%>" ID="Textarea2"><%=dspkey(9)%></TEXTAREA>
		</td>
	</tr>
</table>

<DIV ID="SRTAG3">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
		<tr><td bgcolor="BDB76B" align="center">完工資料</td></tr>
    </table>
</DIV>
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table4">
	<tr><td width="10%" class="dataListHEAD" height="23">完工日</td>
		<td height="23" bgcolor="silver" >
			<input type="text" name="key16" size="12" readonly value="<%=dspKey(16)%>" class="dataListdata" ID="Text7">
       		<input type="button" name="B16" id="B16"  <%=fieldpb%> height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C16" name="C16" onclick="SrClear" <%=fieldpb%> style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
		</td>
		<td width="10%" class="dataListHEAD" height="23">完工工程師</td>
        <td width="23%" bgcolor="silver">
			<input type="text" name="key14" size="6" readonly value="<%=dspKey(14)%>" <%=fieldpa%> class="dataListEntry" ID="Text8">
			<input type="BUTTON" id="B14" name="B14" width="100%" onclick="Srsalesonclick()" <%=fieldpb%> style="Z-INDEX: 1"  value="...." >   
			<IMG alt="清除" id="C14" name="C14" onclick="SrClear" <%=fieldpb%> SRC="/WEBAP/IMAGE/IMGDELETE.GIF" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
			<input type="text" readonly size="10" name="colFinisheng" value="<%=SrGetEmployeeName(dspKey(14))%>" class="dataListsearch3" ID="Text21">
        </td>
		<td width="10%" class="dataListHEAD" height="23">完工經銷商</td>
        <td width="23%" bgcolor="silver">
			<input type="password" name="key15" value="<%=dspKey(15)%>" size="12" readonly <%=fieldpa%> class="dataListEntry" ID="Text51">
			<input type="BUTTON" id="B15" name="B15" width="100%" onclick="SrConsOnClick()" <%=fieldpb%> style="Z-INDEX: 1"  value="...." >   
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C15" name="C15" onclick="SrClear" <%=fieldpb%> style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
			<input type="text" readonly size="6" name="colFinishcons" value="<%=SrGetShortncName(dspKey(15))%>" class="dataListsearch3" ID="Text21">
        </td>
	</tr>
	<tr><td class="dataListHEAD">完工備註</td>
		<td bgcolor="silver" colspan=5 align=center>
			<TEXTAREA cols="100%" name="key18" rows=5 MAXLENGTH=800 <%=fieldpa%> class="dataListentry" <%=dataprotect%> value="<%=dspkey(18)%>" ID="Textarea2"><%=dspkey(18)%></TEXTAREA>
		</td>
	</tr>
</table>
<P></P>
<%
    conn.Close   
    set rs=Nothing   
    set conn=Nothing 
End Sub 
' -------------------------------------------------------------------------------------------- 
Sub SrReadExtDB()
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrSaveExtDB(Smode)
'    Dim conn,rs
' Smode A:add U:update
' extDBField = n
' use extDB(i) for Screen ,and map it to DataBase
'
    'Set conn=Server.CreateObject("ADODB.Connection")
    'conn.open DSN
    'Set rs=Server.CreateObject("ADODB.Recordset")
    'Set comm=Server.CreateObject("ADODB.Command")
    
'------ RTObj ---------------------------------------------------
    'DELFAQlist="delete from RTLessorAVScustfaqlist where faqno='" & dspkey(1) & "'"
    'conn.Execute DELFAQlist  
    'For i=0 to 99
    '    if len(trim(extdb(i))) > 0  then
    '       rs.Open "SELECT * FROM RTLessorAVScustfaqlist WHERE faqno='" &dspKey(1) &"' and faqcod='" & extDB(i) & "'" ,conn,3,3
    '       If rs.Eof Or rs.Bof Then
    '          rs.AddNew
    '          rs("cusid")=dspKey(0)
    '          rs("faqno")=dspKey(1)
    '          rs("faqcod")=extDB(i)          
    '       End If
    '       rs.Update
    '       rs.Close
    '    end if
    'Next
    'conn.Close
    'Set rs=Nothing
    'Set conn=Nothing
End Sub
' -------------------------------------------------------------------------------------------- 
' --------------------------------------------------------------------------------------------  
%>
<!-- #include virtual="/Webap/include/checkid.inc" -->
<!-- #include virtual="/Webap/include/companyid.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserRight.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
