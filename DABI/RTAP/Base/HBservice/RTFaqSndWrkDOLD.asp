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
              If Instr(cTypeDate,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=Null
              If Instr(cTypeAuto,sType) > 0 Or (dspKey(i)=-1 And i<numberOfKey) Then
              Else
              '   On Error Resume Next
				   'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
					if i=0 then
						Set rsc=Server.CreateObject("ADODB.Recordset")
						cusidxx="W" & datePART("yyyy",NOW()) & right("0" & trim(datePART("m",NOW())),2)& right("0" & trim(datePART("d",NOW())),2)
						rsc.open "select max(workno) AS maxworkno from RTSndWork where workno LIKE '" & cusidxx & "%' " ,conn
						if len(trim(rsc("maxworkno"))) > 0 then 
							dspkey(i)=cusidxx & right("00" & cstr(cint(right(rsc("maxworkno"),3)) + 1),3)
						else
							dspkey(i)=cusidxx & "001"
						end if
						rsc.close
					'elseif i=1 then
					'	dspkey(i) =	aryParmKey(i)
					end if      
					rs.Fields(i).Value=dspKey(i)      
              End if
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
              If Instr(cTypeDate,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=Null
         '     On Error Resume Next
              'runpgm=Request.ServerVariables("PATH_INFO") 
              'select case ucase(runpgm)   
                 ' 當程式為ADSL社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
              '   case ucase("/webap/rtap/base/HBservice/RTFaqD.asp")
                    'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                     rs.Fields(i).Value=dspKey(i)    
               '  case else
               '      rs.Fields(i).Value=dspKey(i)
               'end select
          Next
          rs.Update
          rwCnt=rwCnt+1
          If userDefineSave="Yes" Then Call SrSaveExtDB("U")
          sw=""
       End If
    End If
    rs.Close
    ' 當程式為HB社區基本資料維護作業時,將sql自行產生之identify值dspkey(0)讀出至畫面
    if accessmode ="A" then
		runpgm=Request.ServerVariables("PATH_INFO")
        cusidxx="W" & datePART("yyyy",NOW()) & right("0" & trim(datePART("m",NOW())),2)& right("0" & trim(datePART("d",NOW())),2)
		rsc.open "select max(workno) AS maxworkno from RTSndWork where workno LIKE '" & cusidxx & "%' " ,conn        
        if not rsC.eof then
			dspkey(0) = rsC("maxworkno")
			dspkey(1) =	aryParmKey(0)
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
  title="客訴派工資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB=	"SELECT WORKNO, LINKNO, WORKTYPE, ASSIGNENG, ASSIGNCONS, MEMO, SNDWRKUSR, SNDWRKDAT, " &_
  				"FINISHENG, FINISHDAT, UUSR, UDAT, CANCELUSR, CANCELDAT, FINISHCONS, FINISHUSR,FINISHTYP, " &_
				"ASSIGNDAT, ASSIGNTIME " &_
				"FROM RTSndWork " &_
				"WHERE WORKTYPE ='' "
  sqlList=		"SELECT WORKNO, LINKNO, WORKTYPE, ASSIGNENG, ASSIGNCONS, MEMO, SNDWRKUSR, SNDWRKDAT, " &_
  				"FINISHENG, FINISHDAT, UUSR, UDAT, CANCELUSR, CANCELDAT, FINISHCONS, FINISHUSR,FINISHTYP, " &_
				"ASSIGNDAT, ASSIGNTIME " &_
				"FROM RTSndWork " &_
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
  'If len(trim(dspKey(16)))=0 Then dspKey(16)=""
  if len(trim(dspkey(2)))=0 then
       formValid=False
       message="[派工別]不可空白"   
  elseif len(trim(dspkey(3)))=0 and len(trim(dspkey(4)))=0 then
       formValid=False
       message="[預定施工人員or經銷商]至少需輸入一項"
  elseif len(trim(dspkey(3)))>0 and len(trim(dspkey(4)))>0 then
       formValid=False
       message="[預定施工人員or經銷商]不可同時輸入"
  elseif len(trim(dspkey(8)))>0 and len(trim(dspkey(14)))>0 then
       formValid=False
       message="[實際施工人員or經銷商]不可同時輸入"
  end if

  '檢查客訴主檔狀態︰已結案or已作廢則不可轉派工單
   Set connxx=Server.CreateObject("ADODB.Connection")
   Set rsxx=Server.CreateObject("ADODB.Recordset")
   connxx.open DSN
   sqlxx="select closedat, canceldat from RTFaqM where caseno ='" & aryparmkey(1) & "' "
   rsxx.Open sqlxx,connxx
   if not rsxx.eof then
     IF NOT ISNULL(RSXX("closedat")) THEN
        formValid=False
        message="客訴單已結案，不可新增派工單" 
     ELSEIF NOT ISNULL(RSXX("canceldat")) THEN
        formValid=False
        message="客訴單已作廢，不可新增派工單" 
     END IF
   end if
   rsxx.close
   connxx.Close   
   set rsxx=Nothing   
   set connxx=Nothing 

'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(10)=V(0)
        dspkey(11)=now()
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
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clearkey="KEY" & clickid    
       if len(trim(document.all(clearkey).value)) <> 0 then
          	document.all(clearkey).value = ""
   			if clearkey = "KEY3" then
      			document.all("colAssigneng").value = ""
   			elseif clearkey = "KEY4" then
      			document.all("colAssigncons").value = ""
   			elseif clearkey = "KEY8" then
      			document.all("colFinisheng").value = ""
  			elseif clearkey = "KEY14" then
      			document.all("colFinishcons").value = ""
			end if
       end if
   End Sub

   Sub Srsalesonclick()
       prog="RTGetsalesD.asp"
       prog=prog & "?KEY=" & ";"
       'prog=prog & "?KEY=" & document.all("KEY3").VALUE & ";"
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
			FUsrID=Split(Fusr,";")
       		if Fusrid(2) ="Y" then
       			clickkey="KEY" & mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
          		document.all(clickkey).value = trim(Fusrid(0))
       			if clickkey = "KEY3" then
          			document.all("colAssigneng").value =  trim(Fusrid(1))
       			elseif clickkey = "KEY8" then
          			document.all("colFinisheng").value =  trim(Fusrid(1))
				end if
       		End if
       end if
   End Sub

   Sub SrConsOnClick()
		prog="RTGetConsD.asp"
		prog=prog & "?KEY=" & ";"
       'prog=prog & "?KEY=" & document.all("KEY4").VALUE & ";"
		FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
			FUsrID=Split(Fusr,";")
       		if Fusrid(2) ="Y" then
       			clickkey="KEY" & mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
          		document.all(clickkey).value = trim(Fusrid(0))
       			if clickkey = "KEY4" then
          			document.all("colAssigncons").value =  trim(Fusrid(1))
       			elseif clickkey = "KEY14" then
          			document.all("colFinishcons").value =  trim(Fusrid(1))
				end if
       		End if
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
				<input type="text" name="key0" readonly size="12" value="<%=dspKey(0)%>" class=dataListdata>
			</td>
			<td width="15%" class=dataListHead>客服單號</td>
			<td width="35%"  bgcolor="silver">
				<input type="text" name="key1" readonly size="15" value="<%=dspKey(1)%>" class=dataListdata ID="Text1">
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
        if len(trim(dspkey(10))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(10)=V(0)
			   	dspkey(6)=V(0)		'派工人員
        End if  
       dspkey(11)=now()
       dspkey(7)=now()	'派工時間
    'else
        'if len(trim(dspkey(16))) < 1 then
        '   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
        '        V=split(rtnvalue,";")  
        '        DSpkey(16)=V(0)
        'End if         
        'dspkey(17)=now()
    end if      
' -------------------------------------------------------------------------------------------- 

    '派工單完工後 protect
    If len(trim(dspKey(9))) > 0  Then
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
	select case aryparmkey(2)
		case "1", "4"
			sql="select	n.codenc as comtypenc, case p.groupnc when '' then '直銷' else '經銷' end as belongnc, " &_
			    "case p.groupnc when '' then p.leader else p.groupnc end as salesnc, convert(varchar(5),j.comq1) as comq, b.comn, b.t1attachtel as LINETEL, " &_
				"e.snmpip as CMTYIP, f.codenc as LINERATE, b.t1arrive as ARRIVEDAT, b.rcomdrop, '' as gateway, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, a.usekind as CASEKIND, '' as paycycle, " &_
				"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
				"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
				"a.cusid, a.entryno, '' as salesid, '' as consignee, " &_
				"j.faqman, j.tel, j.mobile, k.codenc as faqreason, j.memo, l.name as rcvusr, j.rcvdat, " &_
				"m.name as closeusr, j.closedat " &_
                "from RTFaqM j " &_
                "left outer join RTCust a on j.cusid = a.cusid and j.entryno = a.entryno " &_
                "left outer join RTCmty b on a.comq1 = b.comq1 " &_
                "left outer join HBAdslCmtyCust c on j.cusid = c.cusid and j.entryno = c.entryno and c.comtype =j.comtype " &_
				"left outer join RTsnmp e on e.comq1 = b.comq1 and e.comkind ='3' " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"left outer join RTCode k on k.code = j.faqreason and k.kind ='P7' " &_
				"left outer join RTEmployee l on l.emply = j.rcvusr " &_
				"left outer join RTEmployee m on m.emply = j.closeusr " &_
                "left outer join RTCode n on n.code = j.comtype and n.kind ='P5' " &_
                "left outer join RTCode o on o.code = c.comtype and o.kind ='B3' and o.parm1<>'AA' " &_
                "left outer join HBAdslCmty p on p.comq1 = j.comq1 and p.comtype = j.comtype " &_
				"where j.caseno ='" &aryparmkey(1)& "' "
		case "2"
			sql="select n.codenc as comtypenc, case p.groupnc when '' then '直銷' else '經銷' end as belongnc, " &_
			    "isnull(o.codenc, q.cusnc) as salesnc, convert(varchar(5),j.comq1) as comq, b.comn, b.cmtytel as LINETEL, " &_
				"b.ipaddr as CMTYIP, f.codenc as LINERATE, b.linearrive as ARRIVEDAT, b.rcomdrop, '' as gateway, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, a.usekind as CASEKIND, '' as paycycle, " &_
				"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
				"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
				"a.cusid, a.entryno, '' as salesid, '' as consignee, " &_
				"j.faqman, j.tel, j.mobile, k.codenc as faqreason, j.memo, l.name as rcvusr, j.rcvdat, " &_
				"m.name as closeusr, j.closedat " &_
                "from RTFaqM j " &_
                "left outer join RTCustAdsl a on j.cusid = a.cusid and j.entryno = a.entryno " &_
                "left outer join RTCustAdslCmty b on a.comq1 = b.cutyid " &_
                "left outer join HBAdslCmtyCust c on j.cusid = c.cusid and j.entryno = c.entryno and c.comtype =j.comtype " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"left outer join RTCode k on k.code = j.faqreason and k.kind ='P7' " &_
				"left outer join RTEmployee l on l.emply = j.rcvusr " &_
				"left outer join RTEmployee m on m.emply = j.closeusr " &_
                "left outer join RTCode n on n.code = j.comtype and n.kind ='P5' " &_
                "left outer join RTCode o on o.code = c.comtype and o.kind ='B3' and o.parm1<>'AA' " &_
                "left outer join RTEmployee p inner join RTObj q on p.cusid = q.cusid on p.emply = b.bussid  " &_
				"where j.caseno ='" &aryparmkey(1)& "' "				
		case "3"
			sql="select n.codenc as comtypenc, case when j.comtype ='' then '' when b.consignee ='' then '直銷' else '經銷' end as belongnc, " &_
			    "isnull(o.shortnc, q.cusnc) as salesnc, convert(varchar(5),j.comq1) as comq, b.comn, b.cmtytel as LINETEL, " &_
				"b.ipaddr as CMTYIP, f.codenc as LINERATE, b.linearrive as ARRIVEDAT, b.rcomdrop, '' as gateway, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, a.usekind as CASEKIND, '' as paycycle, " &_
				"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
				"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
				"a.cusid, a.entryno, b.bussid as salesid, b.consignee, " &_
				"j.faqman, j.tel, j.mobile, k.codenc as faqreason, j.memo, l.name as rcvusr, j.rcvdat, " &_
				"m.name as closeusr, j.closedat " &_
                "from RTFaqM j " &_
                "left outer join RTSparqAdslCust a on j.cusid = a.cusid and j.entryno = a.entryno " &_
                "left outer join RTSparqAdslCmty b on a.comq1 = b.cutyid " &_
                "left outer join HBAdslCmtyCust c on j.cusid = c.cusid and j.entryno = c.entryno and c.comtype =j.comtype " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"left outer join RTCode k on k.code = j.faqreason and k.kind ='P7' " &_
				"left outer join RTEmployee l on l.emply = j.rcvusr " &_
				"left outer join RTEmployee m on m.emply = j.closeusr " &_
                "left outer join RTCode n on n.code = j.comtype and n.kind ='P5' " &_
                "left outer join RTObj o on o.cusid = b.consignee " &_
                "left outer join RTEmployee p inner join RTObj q on p.cusid = q.cusid on p.emply = b.bussid  " &_
				"where j.caseno ='" &aryparmkey(1)& "' "				
		case "5"
			sql="select n.codenc as comtypenc, case when j.comtype ='' then '' when b.consignee ='' then '直銷' else '經銷' end as belongnc, " &_
			    "isnull(o.shortnc, q.cusnc) as salesnc, convert(varchar(5),j.comq1)+'-'+convert(varchar(5),j.lineq1) as comq, d.comn, b.linetel as LINETEL, " &_
				"b.lineip as CMTYIP, f.codenc as LINERATE, b.hinetnotifydat as ARRIVEDAT, b.dropdat as RCOMDROP, b.gateway, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, g.codenc as CASEKIND, h.codenc as paycycle, " &_
				"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
				"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
				"a.cusid, null as entryno, b.salesid, b.consignee, " &_
				"j.faqman, j.tel, j.mobile, k.codenc as faqreason, j.memo, l.name as rcvusr, j.rcvdat, " &_
				"m.name as closeusr, j.closedat " &_
                "from RTFaqM j " &_
                "left outer join RTEbtCust a on j.comq1 = a.comq1 and j.lineq1 = a.lineq1 and j.cusid = a.cusid " &_
                "left outer join RTEbtCmtyLine b on j.comq1 = b.comq1 and j.lineq1 = b.lineq1 " &_
                "left outer join RTEbtCmtyH d on d.comq1 = b.comq1 " &_
                "left outer join HBAdslCmtyCust c on c.comq1 = j.comq1 and c.lineq1 = j.lineq1 and j.cusid = c.cusid and c.comtype = j.comtype " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"left outer join RTCode g on g.code = a.casetype and g.kind ='H5' " &_
				"left outer join RTCode h on h.code = a.paytype and h.kind ='G6' " &_
				"left outer join RTCode k on k.code = j.faqreason and k.kind ='P7' " &_
				"left outer join RTEmployee l on l.emply = j.rcvusr " &_
				"left outer join RTEmployee m on m.emply = j.closeusr " &_
                "left outer join RTCode n on n.code = j.comtype and n.kind ='P5' " &_
                "left outer join RTObj o on o.cusid = b.consignee " &_
                "left outer join RTEmployee p inner join RTObj q on p.cusid = q.cusid on p.emply = b.salesid  " &_
				"where j.caseno ='" &aryparmkey(1)& "' "
		case "6"
			sql="select n.codenc as comtypenc, case when j.comtype ='' then '' when b.consignee ='' then '直銷' else '經銷' end as belongnc, " &_
                "isnull(o.shortnc, q.cusnc) as salesnc, convert(varchar(5),j.comq1)+'-'+convert(varchar(5),j.lineq1) as comq, d.comn, b.linetel as LINETEL, " &_
                "replace(str(b.lineipstr1) +'.'+ str(b.lineipstr2) +'.'+ str(b.lineipstr3) +'.'+ str(b.lineipstr4) +'~'+ str(b.lineipend),' ','') as CMTYIP, " &_
                "f.codenc as LINERATE, b.linearrivedat as ARRIVEDAT, b.dropdat as RCOMDROP, b.gateway, c.cusnc, c.contacttel, c.companytel, " &_
                "c.raddr, replace(a.custip1+'.'+ a.custip2 +'.'+ a.custip3 +'.'+ a.custip4, '...', '') as CUSTIP, g.codenc as CASEKIND, h.codenc as paycycle, " &_
                "'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, c.docketdat, null as strbillingdat, null as newbillingdat, " &_
                "null as duedat, c.dropdat, c.canceldat, a.cusid, null as entryno, b.salesid, b.consignee, j.faqman, j.tel, j.mobile, k.codenc as faqreason, j.memo, " &_
                "l.name as rcvusr, j.rcvdat, m.name as closeusr, j.closedat " &_
                "from RTFaqM j " &_
                "left outer join RTSparq499Cust a on j.comq1 = a.comq1 and j.lineq1 = a.lineq1 and j.cusid = a.cusid " &_
                "left outer join RTSparq499CmtyLine b on j.comq1 = b.comq1 and j.lineq1 = b.lineq1 " &_
                "left outer join RTSparq499CmtyH d on d.comq1 = b.comq1 " &_
                "left outer join HBAdslCmtyCust c on c.comq1 = j.comq1 and c.lineq1 = j.lineq1 and j.cusid = c.cusid and c.comtype = j.comtype " &_
                "left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
                "left outer join RTCode g on g.code = a.casetype and g.kind ='L9' " &_
                "left outer join RTCode h on h.code = a.paytype and h.kind ='M1' " &_
                "left outer join RTCode k on k.code = j.faqreason and k.kind ='P7' " &_
                "left outer join RTEmployee l on l.emply = j.rcvusr " &_
                "left outer join RTEmployee m on m.emply = j.closeusr " &_
                "left outer join RTCode n on n.code = j.comtype and n.kind ='P5' " &_
                "left outer join RTObj o on o.cusid = b.consignee " &_
                "left outer join RTEmployee p inner join RTObj q on p.cusid = q.cusid on p.emply = b.salesid  " &_
				"where j.caseno ='" &aryparmkey(1)& "' "
		case "7"
			sql="select n.codenc as comtypenc, case when j.comtype ='' then '' when b.consignee ='' then '直銷' else '經銷' end as belongnc, " &_
	            "isnull(o.shortnc, q.cusnc) as salesnc, convert(varchar(5),j.comq1)+'-'+convert(varchar(5),j.lineq1) as comq, d.comn, " &_
	            "b.linetel as LINETEL, b.lineip as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, " &_
	            "b.gateway, c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11+'.'+a.ip12+'.'+a.ip13+'.'+a.ip14,'...','') as CUSTIP, " &_
	            "g.codenc as CASEKIND, h.codenc as paycycle, i.codenc as paytype, replace(a.overdue,'N','') as overdue, " &_
	            "replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, a.newbillingdat, a.duedat, c.dropdat, c.canceldat, a.cusid, null as entryno, " &_
	            "b.salesid, b.consignee, j.faqman, j.tel, j.mobile, k.codenc as faqreason, j.memo, l.name as rcvusr, j.rcvdat, m.name as closeusr, j.closedat " &_
	            "from RTFaqM j " &_
	            "left outer join RTLessorAvsCust a on j.comq1 = a.comq1 and j.lineq1 = a.lineq1 and j.cusid = a.cusid " &_
	            "left outer join RTLessorAvsCmtyLine b on j.comq1 = b.comq1 and j.lineq1 = b.lineq1 " &_
	            "left outer join RTLessorAvsCmtyH d on d.comq1 = b.comq1 " &_
	            "left outer join HBAdslCmtyCust c on c.comq1 = j.comq1 and c.lineq1 = j.lineq1 and j.cusid = c.cusid and c.comtype = j.comtype " &_
	            "left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
	            "left outer join RTCode g on g.code = a.casekind and g.kind ='O9' " &_
	            "left outer join RTCode h on h.code = a.paycycle and h.kind ='M8' " &_
	            "left outer join RTCode i on i.code = a.paytype and i.kind ='M9' " &_
	            "left outer join RTCode k on k.code = j.faqreason and k.kind ='P7' " &_
	            "left outer join RTEmployee l on l.emply = j.rcvusr " &_
	            "left outer join RTEmployee m on m.emply = j.closeusr " &_
	            "left outer join RTCode n on n.code = j.comtype and n.kind ='P5' " &_
	            "left outer join RTObj o on o.cusid = b.consignee " &_
	            "left outer join RTEmployee p inner join RTObj q on p.cusid = q.cusid on p.emply = b.salesid " &_
				"where j.caseno ='" &aryparmkey(1)& "' "
		case "8"
			sql="select n.codenc as comtypenc, case when j.comtype ='' then '' when b.consignee ='' then '直銷' else '經銷' end as belongnc, " &_
	            "isnull(o.shortnc, q.cusnc) as salesnc, convert(varchar(5),j.comq1)+'-'+convert(varchar(5),j.lineq1) as comq, d.comn, " &_
	            "b.linetel as LINETEL, b.lineip as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, " &_
	            "b.gateway, c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11+'.'+a.ip12+'.'+a.ip13+'.'+a.ip14,'...','') as CUSTIP, " &_
	            "g.codenc as CASEKIND, h.codenc as paycycle, i.codenc as paytype, replace(a.overdue,'N','') as overdue, " &_
	            "replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, a.newbillingdat, a.duedat, c.dropdat, c.canceldat, a.cusid, null as entryno, " &_
	            "b.salesid, b.consignee, j.faqman, j.tel, j.mobile, k.codenc as faqreason, j.memo, l.name as rcvusr, j.rcvdat, m.name as closeusr, j.closedat " &_
	            "from RTFaqM j " &_
	            "left outer join RTLessorCust a on j.comq1 = a.comq1 and j.lineq1 = a.lineq1 and j.cusid = a.cusid " &_
	            "left outer join RTLessorCmtyLine b on j.comq1 = b.comq1 and j.lineq1 = b.lineq1 " &_
	            "left outer join RTLessorCmtyH d on d.comq1 = b.comq1 " &_
	            "left outer join HBAdslCmtyCust c on c.comq1 = j.comq1 and c.lineq1 = j.lineq1 and j.cusid = c.cusid and c.comtype = j.comtype " &_
	            "left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
	            "left outer join RTCode g on g.code = a.casekind and g.kind ='O9' " &_
	            "left outer join RTCode h on h.code = a.paycycle and h.kind ='M8' " &_
	            "left outer join RTCode i on i.code = a.paytype and i.kind ='M9' " &_
	            "left outer join RTCode k on k.code = j.faqreason and k.kind ='P7' " &_
	            "left outer join RTEmployee l on l.emply = j.rcvusr " &_
	            "left outer join RTEmployee m on m.emply = j.closeusr " &_
	            "left outer join RTCode n on n.code = j.comtype and n.kind ='P5' " &_
	            "left outer join RTObj o on o.cusid = b.consignee " &_
	            "left outer join RTEmployee p inner join RTObj q on p.cusid = q.cusid on p.emply = b.salesid " &_
				"where j.caseno ='" &aryparmkey(1)& "' "
		case "9"
			sql="select n.codenc as comtypenc, case when j.comtype ='' then '' when d.consignee ='' then '直銷' else '經銷' end as belongnc, " &_
	            "isnull(o.shortnc, q.cusnc) as salesnc, convert(varchar(5),j.comq1)+'-'+convert(varchar(5),j.lineq1) as comq, d.comn, " &_
	            "b.linetel as LINETEL, b.lineip as CMTYIP, f.codenc as LINERATE, b.ARRIVEDAT, b.dropdat as RCOMDROP, " &_
	            "b.gateway, c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11+'.'+a.ip12+'.'+a.ip13+'.'+a.ip14,'...','') as CUSTIP, " &_
	            "'' as CASEKIND, '' as paycycle, '' as paytype, '' as overdue, replace(a.freecode,'N','') as freecode, c.docketdat, " &_
	            "a.strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, a.cusid, null as entryno, d.salesid, d.consignee, " &_
	            "j.faqman, j.tel, j.mobile, k.codenc as faqreason, j.memo, l.name as rcvusr, j.rcvdat, m.name as closeusr, j.closedat " &_
	            "from RTFaqM j " &_
	            "left outer join RTPrjCust a on j.comq1 = a.comq1 and j.lineq1 = a.lineq1 and j.cusid = a.cusid " &_
	            "left outer join RTPrjCmtyLine b on j.comq1 = b.comq1 and j.lineq1 = b.lineq1 " &_
	            "left outer join RTPrjCmtyH d on d.comq1 = b.comq1 " &_
	            "left outer join HBAdslCmtyCust c on c.comq1 = j.comq1 and c.lineq1 = j.lineq1 and j.cusid = c.cusid and c.comtype = j.comtype " &_
	            "left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
	            "left outer join RTCode k on k.code = j.faqreason and k.kind ='P7' " &_
	            "left outer join RTEmployee l on l.emply = j.rcvusr " &_
	            "left outer join RTEmployee m on m.emply = j.closeusr " &_
	            "left outer join RTCode n on n.code = j.comtype and n.kind ='P5' " &_
	            "left outer join RTObj o on o.cusid = d.consignee " &_
	            "left outer join RTEmployee p inner join RTObj q on p.cusid = q.cusid on p.emply = d.salesid " &_
				"where j.caseno ='" &aryparmkey(1)& "' "
		case "A"
			sql="select n.codenc as comtypenc, case when j.comtype ='' then '' when d.consignee ='' then '直銷' else '經銷' end as belongnc,  " &_
				"isnull(o.shortnc, q.cusnc) as salesnc, convert(varchar(5),j.comq1)+'-'+convert(varchar(5),j.lineq1) as comq, d.comn,  " &_
				"b.linetel as LINETEL, b.lineip as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP,  " &_
				"b.gateway, c.cusnc, c.contacttel, c.companytel, c.raddr, a.ip11 as CUSTIP, '' as CASEKIND, '' as paycycle, '' as paytype,  " &_
				"convert(varchar(10), overduedat, 111) as overdue, replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat,  " &_
				"null as newbillingdat, null as duedat, c.dropdat, c.canceldat, a.cusid, null as entryno, d.salesid, d.consignee, j.faqman,  " &_
				"j.tel, j.mobile, k.codenc as faqreason, j.memo, l.name as rcvusr, j.rcvdat, m.name as closeusr, j.closedat  " &_
				"from RTFaqM j  " &_
				"left outer join RTSonetCust a on j.comq1 = a.comq1 and j.lineq1 = a.lineq1 and j.cusid = a.cusid  " &_
				"left outer join RTSonetCmtyLine b on j.comq1 = b.comq1 and j.lineq1 = b.lineq1  " &_
				"left outer join RTSonetCmtyH d on d.comq1 = b.comq1  " &_
				"left outer join HBAdslCmtyCust c on c.comq1 = j.comq1 and c.lineq1 = j.lineq1 and j.cusid = c.cusid and c.comtype = j.comtype  " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"left outer join RTCode k on k.code = j.faqreason and k.kind ='P7' " &_
				"left outer join RTEmployee l on l.emply = j.rcvusr " &_
				"left outer join RTEmployee m on m.emply = j.closeusr " &_
				"left outer join RTCode n on n.code = j.comtype and n.kind ='P5' " &_
				"left outer join RTObj o on o.cusid = d.consignee " &_
				"left outer join RTEmployee p inner join RTObj q on p.cusid = q.cusid on p.emply = d.salesid " &_
				"where j.caseno ='" &aryparmkey(1)& "' "
		case else
			sql=""	
	end select			
	'response.Write "sql=" &sql

    Dim conn,rs,s,sx,sql,t
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
    if sql <>"" then 
		rs.OPEN sql,CONN
		IF RS.EOF THEN
			comtypenc =""		:   belongnc =""	:	salesnc =""		:	comq =""	
			comn =""	        :	linetel =""		:	cmtyip =""		:	linerate =""	
			arrivedat =""		:	rcomdrop =""	:	salesid =""		:	consignee =""
			gateway =""			
			cusnc =""			:	contacttel =""	:   companytel =""	:	raddr =""
			custip =""			:   casekind =""	:	paycycle =""	:   paytype =""		
			overdue =""			:	freecode =""	:   docketdat =""	:   strbillingdat =""
			newbillingdat =""	:   duedat =""		:	dropdat =""		:   canceldat =""
			cusid =""			:	entryno =""
			faqman =""			:	tel =""			:	mobile =""		:	faqreason =""
			memo =""			:	rcvusr =""		:	rcvdat =""		:	closeusr =""
			closedat =""
		ELSE
			comtypenc =rs("comtypenc")	:	belongnc = rs("belongnc")	:	salesnc = rs("salesnc")			
			comq = rs("comq")			:	comn = rs("comn")			:	linetel = rs("linetel")
			cmtyip = rs("cmtyip")       :	linerate = rs("linerate")	:	arrivedat = rs("arrivedat")
			rcomdrop = rs("rcomdrop")	:	gateway	= rs("gateway")
			cusnc = rs("cusnc")			:	companytel = rs("companytel")
			raddr = rs("raddr")			:	custip = rs("custip")		:	contacttel = rs("contacttel")
			paycycle = rs("paycycle")	:	paytype = rs("paytype")		:	casekind = rs("casekind")
			overdue = rs("overdue")     :	freecode = rs("freecode")	:	strbillingdat = rs("strbillingdat")
			docketdat = rs("docketdat")	:	dropdat = rs("dropdat")		:	newbillingdat = rs("newbillingdat")
			canceldat = rs("canceldat") :	duedat = rs("duedat")		:	cusid = rs("cusid")
			entryno = rs("entryno")		:	salesid = rs("salesid")		:	consignee = rs("consignee")
			faqman = rs("faqman")		:	tel = rs("tel")				:	mobile = rs("mobile")
			memo = rs("memo")			:	rcvusr = rs("rcvusr")		:	faqreason = rs("faqreason")
			rcvdat = rs("rcvdat")		:	closeusr = rs("closeusr")	:	closedat = rs("closedat")
		END IF
		RS.CLOSE
	end if
%>
<DIV ID="SRTAG0">
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
	<tr><td bgcolor="BDB76B" align="center">客戶基本資料</td></tr>
</table>
</DIV>

<DIV ID=SRTAB0 >
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3">
	<tr>
		<td width="10%" class=dataListsearch>社區名稱</td>
		<td bgcolor="silver" colspan=5 >
			<input type="text" readonly size="42" name="colCOMN" value="<%=comn%>" class="dataListsearch3" ID="Text27">
		</td>
	</tr>
	<tr>
	    <td width="10%" class=dataListsearch>主線序號</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" readonly size="7" name="colComq" value="<%=comq%>" class="dataListsearch3" ID="Text22">
		</td>
	    <td width="10%" class=dataListsearch>附掛電話</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" readonly size="15" name="colLinetel" value="<%=linetel%>" class="dataListsearch3" ID="Text16">
		</td>
	    <td width="10%" class=dataListsearch>主線速率</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" readonly size="15" name="colLinerate" value="<%=linerate%>" class="dataListsearch3" ID="Text23">
		</td>
	</tr>
	<tr>		
	    <td width="10%" class=dataListsearch>社區IP</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" readonly size="20" name="colCmtyip" value="<%=cmtyip%>" class="dataListsearch3" ID="Text6">
		</td>
	    <td width="10%" class=dataListsearch>Gateway IP</td>
		<td width="23%" bgcolor="silver" colspan=3>
			<input type="text" readonly size="20" name="colGateway" value="<%=gateway%>" class="dataListsearch3" ID="Text52">
		</td>
	</tr>	
	<tr>
		<td class=dataListsearch>線路到位日</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="12" name="colArrivedat" value="<%=arrivedat%>" class="dataListsearch3" ID="Text24">
		</td>
	    <td class=dataListsearch>撤線日</td>
		<td bgcolor="silver" colspan=3>
			<input type="text" readonly size="12" name="colRcomdrop" value="<%=rcomdrop%>" class="dataListsearch3" style="color:red;" ID="Text25" >
		</td>
	</tr>
	<tr>
		<td class=dataListsearch>方案別</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="10" name="colComtypenc" value="<%=comtypenc%>" class="dataListsearch3" ID="Text42">
		</td>
	    <td class=dataListsearch>直經銷</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="5" name="colBelongnc" value="<%=belongnc%>" class="dataListsearch3" ID="Text14">
		</td>
		<td class=dataListsearch>轄區業務</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="8" name="colSalesnc" value="<%=salesnc%>" class="dataListsearch3" ID="Text15">
		</td>
	</tr>
</table>
</DIV>
	
<DIV ID=SRTAB2 >
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table4">	
	<tr>
		<td width="10%" class=dataListsearch>用戶名稱</td>
		<td bgcolor="silver" colspan=5>
			<input type="text" readonly size="42" value="<%=cusnc%>" name="colCusnc" class="dataListsearch3" ID="Text41">
		</td>
	</tr>
	<tr>
	    <td class=dataListsearch>用戶地址</td>
		<td bgcolor="silver" colspan=5>
			<input type="text" readonly size="73" value="<%=raddr%>" name="colRaddr" class="dataListsearch3" ID="Text26">
		</td>
	</tr>
	<tr>
		<td width="10%" class=dataListsearch>用戶電話</td>
		<td bgcolor="silver" colspan=3>
			<input type="text" readonly size="50" name="colContacttel" value="<%=contacttel%>" class="dataListsearch3" ID="Text31">			
		</td>
	    <td width="10%" class=dataListsearch>行動電話</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" readonly size="29" name="colCompanytel" value="<%=companytel%>" class="dataListsearch3" ID="Text30">
		</td>
	</tr>
	<tr>
		<td width="10%" class=dataListsearch>用戶編號</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" readonly size="15" name="colCusid" value="<%=cusid%>" class="dataListsearch3" ID="Text28">
			<font size=2 color=black>項次: </font>
			<input type="text" readonly size="2" name="colEntryno" value="<%=entryno%>" class="dataListsearch3" ID="Text4">
		</td>
	    <td width="10%" class=dataListsearch>公關機</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" readonly size="2" name="colFreecode" value="<%=freecode%>" class="dataListsearch3" style="color:red;" ID="Text5">
		</td>
	    <td width="10%" class=dataListsearch>用戶IP</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" readonly size="20" name="colCustip" value="<%=custip%>" class="dataListsearch3" ID="Text29">
		</td>
	</tr>	
	<tr>
		<td class=dataListsearch>用戶方案</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="29" name="colCasekind" value="<%=casekind%>" class="dataListsearch3" ID="Text32">
		</td>
	    <td class=dataListsearch>繳費方式</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="15" name="colPaytype" value="<%=paytype%>" class="dataListsearch3" ID="Text33">
		</td>
		<td class=dataListsearch>繳費週期</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="29" name="colPaycycle" value="<%=paycycle%>" class="dataListsearch3" ID="Text34">
		</td>
	</tr>
	<tr>
		<td class=dataListsearch>報竣日</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="25" name="colDocketdat" value="<%=docketdat%>" class="dataListsearch3" ID="Text35">
		</td>
		<td class=dataListsearch>退租日</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="10" name="colDropdat" value="<%=dropdat%>" class="dataListsearch3" style="color:red;" ID="Text38">
		</td>
	    <td class=dataListsearch>是否欠拆</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="2" name="colOverdue" value="<%=overdue%>" class="dataListsearch3" style="color:red;" ID="Text39">
		</td>
	</tr>
	<tr>
	    <td class=dataListsearch>開始計費日</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="10" name="colStrbillingdat" value="<%=strbillingdat%>" class="dataListsearch3" ID="Text36">
		</td>
	    <td class=dataListsearch>續約日</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="10" name="colNewbillingdat" value="<%=newbillingdat%>" class="dataListsearch3" ID="Text37">
		</td>
	    <td class=dataListsearch>到期日</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="10" name="colDuedat" value="<%=duedat%>" class="dataListsearch3"
			<%	if len(duedat)> 0 then
					if dateadd("d",1,duedat) < now() then response.Write "style=""color:red;""" 
				end if
			%> ID="Text43">
		</td>
	</tr>
	<tr>
	    <td class=dataListsearch>作廢日</td>
		<td bgcolor="silver" colspan=5>
			<input type="text" readonly size="25" name="colCanceldat" value="<%=canceldat%>" class="dataListsearch3" style="color:red;" ID="Text40">
		</td>
	</tr>
</TABLE>
</DIV>


<DIV ID="Div1">
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table7">
	<tr><td bgcolor="BDB76B" align="center">客訴內容</td></tr>
</table>
</DIV>

<DIV ID="Div2" >
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
	<tr>
		<td width="10%" class=dataListsearch></td><td width="23%">
		<td width="10%" class=dataListsearch></td><td width="23%">
		<td width="10%" class=dataListsearch></td><td width="23%">
	</tr>
	<tr>
		<td class=dataListsearch>報修原因</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="25" name="colFaqreason" value="<%=faqreason%>" class="dataListsearch3" ID="Text3">
		</td>
		<td class=dataListsearch>聯絡人</td>
		<td bgcolor="silver" colspan=3 >
			<input type="text" readonly size="50" name="colFaqMan" value="<%=faqman%>" class="dataListsearch3" ID="Text2">
		</td>
	</tr>
	<tr>
		<td class=dataListsearch>手機</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="25" name="colMobile" value="<%=mobile%>" class="dataListsearch3" ID="Text44">
		</td>
		<td class=dataListsearch>聯絡電話</td>
		<td bgcolor="silver" colspan=3 >
			<input type="text" readonly size="50" name="colTel" value="<%=tel%>" class="dataListsearch3" ID="Text45">
		</td>
	</tr>
	<tr>
		<td class=dataListsearch>受理人員</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="10" name="colRcvusr" value="<%=rcvusr%>" class="dataListsearch3" ID="Text46">			
		</td>
	    <td class=dataListsearch>受理時間</td>
		<td bgcolor="silver" colspan=3 >
			<input type="text" readonly size="25" name="colRcvdat" value="<%=rcvdat%>" class="dataListsearch3" ID="Text47">
		</td>
	</tr>
	<tr>
		<td class=dataListsearch>結案人員</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="10" name="colCloseusr" value="<%=closeusr%>" class="dataListsearch3" style="color:red;" ID="Text48">			
		</td>
	    <td class=dataListsearch>結案時間</td>
		<td bgcolor="silver" colspan=3>
			<input type="text" readonly size="25" name="colClosedat" value="<%=closedat%>" class="dataListsearch3" style="color:red;" ID="Text49">
		</td>
	</tr>
	<tr>
		<td class=dataListsearch>客訴備註</td>
		<td bgcolor="silver" colspan=5>
			<TEXTAREA cols="100%" readonly rows="8" name="colMemo" value="<%=memo%>" class="dataListsearch3" ID="Textarea1"><%=memo%></TEXTAREA>
		</td>
	</tr>
</table>
</DIV>

<DIV ID="Div3">
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table10">
	<tr><td bgcolor="BDB76B" align="center">客訴追件</td></tr>
</table>
</DIV>

<DIV ID="Div4" > 
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table11">
    <tr class="dataListHEAD"><td>項次</td><td nowrap>進出線</td><td>追件<br>人員</td><td>追件時間</td><td>追件內容</td></tr>
	<%   
		sqlfaqlist="select a.entryno, d.codenc as ioboundnc, c.name as addusrname, a.adddat, a.memo " &_
					"from RTFaqAdd a " &_
					"inner join RTFaqM b on a.caseno = b.caseno " &_
					"left outer join RTEmployee c on c.emply = a.addusr " &_
					"left outer join RTCode d on d.code = a.iobound and d.kind ='P8' " &_
					"WHERE a.canceldat is null " &_
					"and a.caseno ='" & dspkey(1) & "' "
		rs.open sqlfaqlist,conn
		Do While Not rs.Eof
			response.Write	"<tr class=""dataListentry"">" &_
							"<td>"& rs("entryno") & "</td>" &_
							"<td>"& rs("ioboundnc") & "</td>" &_	
							"<td nowrap>"& rs("addusrname") & "</td>" &_	
							"<td nowrap>"& rs("adddat") & "</td>" &_	
							"<td>"& rs("memo") & "</td>" &_	
							"</tr>"
			rs.MoveNext
		Loop
		rs.close
	%>
</table>
<tr><td height=23></td></tr>
</div>


<DIV ID="SRTAG2">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
		<tr><td bgcolor="BDB76B" align="center">派工資料</td></tr>
    </table>
</DIV>

<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3">
	<tr><td width="10%" class=dataListHEAD>派工別</td>
		<%
			s=""
			sx=" selected "
			If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 and len(trim(dspkey(9)))=0 Then  
			sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='P6' and code in ('01','09') " 
			If len(trim(dspkey(2))) < 1 Then
				sx=" selected " 
				s=s & "<option value=""""" & sx & "></option>"  
				sx=""
			else
				s=s & "<option value=""""" & sx & "></option>"  
				sx=""
			end if     
			Else
			sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='P6' AND CODE='" & dspkey(2) & "'"
			End If
			rs.Open sql,conn
			Do While Not rs.Eof
			If rs("CODE")=dspkey(2) Then sx=" selected "
			s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
			rs.MoveNext
			sx=""
			Loop
			rs.Close
		%>
		<td width="23%" bgcolor="silver">
			<select size="1" name="key2" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select1" >                                                                  
				<%=s%>
			</select>
		</td>

		<td width="10%" class="dataListHEAD" height="23">派工日</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" name="key7" size="25" READONLY value="<%=dspKey(7)%>"  class="dataListDATA" ID="Text9">
		</td> 

		<td width="10%" class="dataListHEAD" height="23">派工人</td>
		<td width="23%" bgcolor="silver">
			<input type="text" name="key6" size="6" READONLY value="<%=dspKey(6)%>" class="dataListDATA">
			<font size=2><%=SrGetEmployeeName(dspKey(6))%></font>
		</td>  
	</tr>

	<tr>
		<td width="10%" class=dataListHEAD>預定施工人員</td>
		<td width="23%" bgcolor="silver" >
			<% If accessMode="A" And sw="" Then  dspKey(3) =salesid %>
			<input type="TEXT" name="key3" value="<%=dspKey(3)%>" size="6" readonly <%=fieldpa%> class="dataListEntry" ID="Text50">
			<input type="BUTTON" id="B3" name="B3" width="100%" onclick="Srsalesonclick()" <%=fieldpb%> style="Z-INDEX: 1"  value="...." >   
			<IMG alt="清除" id="C3" name="C3" onclick="SrClear" <%=fieldpb%> SRC="/WEBAP/IMAGE/IMGDELETE.GIF" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
			<input type="text" readonly size="10" name="colAssigneng" value="<%=SrGetEmployeeName(dspKey(3))%>" class="dataListsearch3" ID="Text21">
        </td>

		<td width="10%" class=dataListHEAD>預定施工經銷商</td>
		<td width="23%" bgcolor="silver">
			<% If accessMode="A" And sw="" Then  dspKey(4) =consignee %>
			<input type="password"  name="key4" value="<%=dspKey(4)%>" size="12" readonly <%=fieldpa%> class="dataListEntry" ID="Text51">
			<input type="BUTTON" id="B4" name="B4" width="100%" onclick="SrConsOnClick()" <%=fieldpb%> style="Z-INDEX: 1"  value="...." >   
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C4" name="C4" onclick="SrClear" <%=fieldpb%> style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
			<%
				sql="select shortnc from RTObj where cusid ='" &dspKey(4)& "' "  
			    rs.Open sql,conn
				If not rs.Eof Then Assigncons = rs("shortnc")
				rs.Close
			%>
			<input type="text" readonly size="10" name="colAssigncons" value="<%=Assigncons%>" class="dataListsearch3" ID="Text21">
        </td>

		<td width="10%" class=dataListHEAD>預定施工時間</td>
		<td width="23%" bgcolor="silver">
        	<input type="text" name="key17" <%=fieldpa%> <%=fieldRole(1)%><%=dataProtect%> value="<%=dspKey(17)%>" READONLY size="12" class=dataListEntry>
       		<input type="button" name="B17" id="B17"  <%=fieldpb%> height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C17" name="C17" onclick="SrClear" <%=fieldpb%> style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
			
		<%
			s=""
			sx=" selected "
			If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 and len(trim(dspkey(9)))=0 Then  
			sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='Q4' " 
			If len(trim(dspkey(18))) < 1 Then
				sx=" selected " 
				s=s & "<option value=""""" & sx & "></option>"  
				sx=""
			else
				s=s & "<option value=""""" & sx & "></option>"  
				sx=""
			end if     
			Else
			sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='Q4' AND CODE='" & dspkey(18) & "'"
			End If
			rs.Open sql,conn
			Do While Not rs.Eof
			If rs("CODE")=dspkey(18) Then sx=" selected "
			s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
			rs.MoveNext
			sx=""
			Loop
			rs.Close
		%>
			<select size="1" name="key18" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select1" >
				<%=s%>
			</select>
        </td>
        
        
	</tr>

	<tr>
		<td class="dataListHEAD" height="23">實際施工人員</td>
        <td height="23" bgcolor="silver">
			<input type="text" name="key8" size="6" readonly value="<%=dspKey(8)%>" <%=fieldpa%> class="dataListEntry" ID="Text8">
			<input type="BUTTON" id="B8" name="B8" width="100%" onclick="Srsalesonclick()" <%=fieldpb%> style="Z-INDEX: 1"  value="...." >   
			<IMG alt="清除" id="C8" name="C8" onclick="SrClear" <%=fieldpb%> SRC="/WEBAP/IMAGE/IMGDELETE.GIF" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
			<input type="text" readonly size="10" name="colFinisheng" value="<%=SrGetEmployeeName(dspKey(8))%>" class="dataListsearch3" ID="Text21">
        </td>
		<td class="dataListHEAD" height="23">實際施工經銷商</td>
        <td height="23" bgcolor="silver" colspan=3 >
			<input type="password" name="key14" value="<%=dspKey(14)%>" size="12" readonly <%=fieldpa%> class="dataListEntry" ID="Text51">
			<input type="BUTTON" id="B14" name="B14" width="100%" onclick="SrConsOnClick()" <%=fieldpb%> style="Z-INDEX: 1"  value="...." >   
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C14" name="C14" onclick="SrClear" <%=fieldpb%> style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
			<%
				sql="select shortnc from RTObj where cusid ='" &dspKey(14)& "' "  
			    rs.Open sql,conn
				If not rs.Eof Then Finishcons = rs("shortnc")
				rs.Close
			%>
			<input type="text" readonly size="10" name="colFinishcons" value="<%=Finishcons%>" class="dataListsearch3" ID="Text21">
        </td>
	</tr>

	<tr><td class="dataListHEAD" height="23">完工人</td>
		<td height="23" bgcolor="silver">
			<input type="text" name="key15" size="6" READONLY value="<%=dspKey(15)%>" class="dataListDATA">
			<font size=2><%=SrGetEmployeeName(dspKey(15))%></font>
		</td>  
		<td  class="dataListHEAD" height="23">完工日</td>
		<td height="23" bgcolor="silver">
			<input type="text" name="key9" size="25" readonly value="<%=dspKey(9)%>" class="dataListdata" ID="Text7">
		</td>
		<td  class="dataListHEAD" height="23">完工種類</td>
		<%
			s="<option value=""""" & sx & "></option>"  
			sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='P9' AND CODE='" & dspkey(16) & "'"
			rs.Open sql,conn
			Do While Not rs.Eof
				s=s &"<option value=""" &rs("CODE") &""" selected>" &rs("CODENC") &"</option>"
				rs.MoveNext
			Loop
			rs.Close
		%>
		
		<td height="23" bgcolor="silver">
			<select size="1" name="key16" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListData" ID="Select2" >                                                                  
				<%=s%>
			</select>
		</td>
		
	</tr>

	<tr><td  class="dataListHEAD" height="23">異動人員</td>
        <td  height="23" bgcolor="silver">
			<input type="text" name="key10" size="6" READONLY value="<%=dspKey(10)%>" class="dataListDATA" ID="Text10">
			<font size=2><%=SrGetEmployeeName(dspKey(10))%></font>
        </td>
        <td  class="dataListHEAD" height="23">異動時間</td>
        <td  height="23" bgcolor="silver" colspan=3>
	        <input type="text" name="key11" size="25" READONLY value="<%=dspKey(11)%>" class="dataListDATA" ID="Text11">
        </td>
	</tr>

	<tr><td  class="dataListHEAD" height="23">作廢人員</td>
        <td  height="23" bgcolor="silver">
			<input type="text" name="key12" size="6" READONLY value="<%=dspKey(12)%>" style="color:red;" class="dataListDATA" ID="Text12">
			<font size=2 color=red><%=SrGetEmployeeName(dspKey(12))%></font>
        </td>
        <td  class="dataListHEAD" height="23">作廢時間</td>
        <td  height="23" bgcolor="silver" colspan=3>
	        <input type="text" name="key13" size="25" READONLY value="<%=dspKey(13)%>" style="color:red;" class="dataListDATA" ID="Text13">
        </td>
	</tr>

	<tr><td class="dataListHEAD">派工處理措施</td>
		<td bgcolor="silver" colspan=5>
			<TEXTAREA cols="100%" name="key5" rows=10 MAXLENGTH=800 <%=fieldpa%> class="dataListentry" <%=dataprotect%> value="<%=dspkey(5)%>" ID="Textarea2"><%=dspkey(5)%></TEXTAREA>
		</td>
	</tr>
</table>

<DIV ID="SRTAG5">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table5">
		<tr><td bgcolor="BDB76B" align="center">績效點數</td></tr>
    </table>
</DIV>


</DIV>
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
