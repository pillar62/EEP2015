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
						cusidxx="I" & datePART("yyyy",NOW()) & right("0" & trim(datePART("m",NOW())),2) 
						rsc.open "select max(callno) AS maxcallno from RTSndWorkCall where callno LIKE '" & cusidxx & "%' " ,conn
						if len(trim(rsc("maxcallno"))) > 0 then 
							dspkey(i)=cusidxx & right("000" & cstr(cint(right(rsc("maxworkno"),4)) + 1),4)
						else
							dspkey(i)=cusidxx & "0001"
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
        cusidxx="I" & datePART("yyyy",NOW()) & right("0" & trim(datePART("m",NOW())),2) 
		rsc.open "select max(callno) AS maxcallno from RTSndWorkCall where callno LIKE '" & cusidxx & "%' " ,conn
        if not rsC.eof then
			dspkey(0) = rsC("maxcallno")
			dspkey(1) =	aryParmKey(1)
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
  title="客訴維修滿意度調查表維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB=	"SELECT CALLNO, WORKNO, CALLDAT, CALLUSR, CALLCUST, CALLMEMO, UUSR, UDAT, CANCELUSR, CANCELDAT " &_
				"FROM RTSndWorkCall " &_
				"WHERE CALLNO ='' "
  sqlList=		"SELECT CALLNO, WORKNO, CALLDAT, CALLUSR, CALLCUST, CALLMEMO, UUSR, UDAT, CANCELUSR, CANCELDAT " &_
				"FROM RTSndWorkCall " &_
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
  if len(trim(dspkey(4)))=0 then
       formValid=False
       message="[電訪客戶]不可空白"   
  'elseif len(trim(dspkey(3)))=0 and len(trim(dspkey(4)))=0 then
  '     formValid=False
  '     message="[預定施工人員or經銷商]至少需輸入一項"
  'elseif len(trim(dspkey(3)))>0 and len(trim(dspkey(4)))>0 then
  '     formValid=False
  '     message="[預定施工人員or經銷商]不可同時輸入"
  'elseif len(trim(dspkey(8)))>0 and len(trim(dspkey(14)))>0 then
  '     formValid=False
  '     message="[實際施工人員or經銷商]不可同時輸入"
  end if

  '檢查派工單狀態︰未完工or已作廢則不可轉派工單
   Set connxx=Server.CreateObject("ADODB.Connection")
   Set rsxx=Server.CreateObject("ADODB.Recordset")
   connxx.open DSN
   sqlxx="select finishdat, canceldat from RTFaqM where workno ='" & aryparmkey(1) & "' "
   rsxx.Open sqlxx,connxx
   if not rsxx.eof then
     IF ISNULL(RSXX("finishdat")) THEN
        formValid=False
        message="派工單未完工，不可進行滿意度調查" 
     ELSEIF NOT ISNULL(RSXX("canceldat")) THEN
        formValid=False
        message="派工單已作廢，不可新增滿意度調查表" 
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
                DSpkey(6)=V(0)
        dspkey(7)=now()
    end if        
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveXScript()%>
   <SCRIPT Language="VBScript">
'   Sub Srbtnonclick()
'       Dim ClickID
'       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
'       clickkey="KEY" & clickid
'	   if isdate(document.all(clickkey).value) then
'	      objEF2KDT.varDefaultDateTime=document.all(clickkey).value
'       end if
'       call objEF2KDT.show(1)
'       if objEF2KDT.strDateTime <> "" then
'          document.all(clickkey).value = objEF2KDT.strDateTime
'       end if
'   END SUB

   Sub SrClear()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clearkey="KEY" & clickid    
       if len(trim(document.all(clearkey).value)) <> 0 then
          	document.all(clearkey).value = ""
   			'if clearkey = "KEY3" then
      		'	document.all("colAssigneng").value = ""
   			'elseif clearkey = "KEY4" then
      		'	document.all("colAssigncons").value = ""
   			'elseif clearkey = "KEY8" then
      		'	document.all("colFinisheng").value = ""
  			'elseif clearkey = "KEY14" then
      		'	document.all("colFinishcons").value = ""
			'end if
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
       			'if clickkey = "KEY3" then
          		'	document.all("colAssigneng").value =  trim(Fusrid(1))
       			'elseif clickkey = "KEY8" then
          		'	document.all("colFinisheng").value =  trim(Fusrid(1))
				'end if
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
<%	
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()%>
	<table width="100%" border=1 cellPadding=0 cellSpacing=0>
		<tr><td width="15%" class=dataListHead>滿意度調查單號</td>
			<td width="35%"  bgcolor="silver">
				<input type="text" name="key0" readonly size="12" value="<%=dspKey(0)%>" class=dataListdata>
			</td>
			<td width="15%" class=dataListHead>派工單號</td>
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
                dspkey(6)=V(0)
			   	dspkey(3)=V(0)		'電訪人員
        End if  
       dspkey(7)=now()
       dspkey(2)=now()	'電訪時間
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

<span id="tags1" class="dataListTagsOn">滿意度調查</span>
                                                            
<div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td>
<td>     

<%
	select case aryparmkey(3)
		case "1", "4"
			sql="select	c.comtypenc, c.belongnc, c.salesnc, c.comq, b.comn, b.t1attachtel as LINETEL, " &_
				"e.snmpip as CMTYIP, f.codenc as LINERATE, b.t1arrive as ARRIVEDAT, b.rcomdrop, '' as gateway, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, a.usekind as CASEKIND, '' as paycycle, " &_
				"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
				"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
				"a.cusid, a.entryno, '' as salesid, '' as consignee, " &_
				"j.faqman, j.tel, j.mobile, k.codenc as faqreason, j.memo, l.name as rcvusr, j.rcvdat, " &_
				"m.name as closeusr, j.closedat " &_
				"from 	RTCust a " &_
				"inner join RTCmty b on a.comq1 = b.comq1 " &_
				"inner join HBAdslCmtyCust c on a.cusid = c.cusid and a.entryno = c.entryno " &_
				"left outer join RTsnmp e on e.comq1 = b.comq1 and e.comkind ='3' " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"inner join RTFaqM j on j.cusid = a.cusid and j.entryno = a.entryno " &_
				"left outer join RTCode k on k.code = j.faqreason and k.kind ='P7' " &_
				"left outer join RTEmployee l on l.emply = j.rcvusr " &_
				"left outer join RTEmployee m on m.emply = j.closeusr " &_
				"where (c.comtype ='1' or c.comtype ='4') " &_
				"and j.caseno ='" &aryparmkey(2)& "' "
		case "2"
			sql="select c.comtypenc, c.belongnc, c.salesnc, c.comq, b.comn, b.cmtytel as LINETEL, " &_
				"b.ipaddr as CMTYIP, f.codenc as LINERATE, b.linearrive as ARRIVEDAT, b.rcomdrop, '' as gateway, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, a.usekind as CASEKIND, '' as paycycle, " &_
				"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
				"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
				"a.cusid, a.entryno, '' as salesid, '' as consignee, " &_
				"j.faqman, j.tel, j.mobile, k.codenc as faqreason, j.memo, l.name as rcvusr, j.rcvdat, " &_
				"m.name as closeusr, j.closedat " &_
				"from 	RTCustAdsl a " &_
				"inner join RTCustAdslCmty b on a.comq1 = b.cutyid " &_
				"inner join HBAdslCmtyCust c on a.cusid = c.cusid and a.entryno = c.entryno " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"inner join RTFaqM j on j.cusid = a.cusid and j.entryno = a.entryno " &_
				"left outer join RTCode k on k.code = j.faqreason and k.kind ='P7' " &_
				"left outer join RTEmployee l on l.emply = j.rcvusr " &_
				"left outer join RTEmployee m on m.emply = j.closeusr " &_
				"where 	c.comtype ='2' " &_
				"and j.caseno ='" &aryparmkey(2)& "' "
		case "3"
			sql="select c.comtypenc, c.belongnc, c.salesnc, c.comq, b.comn, b.cmtytel as LINETEL, " &_
				"b.ipaddr as CMTYIP, f.codenc as LINERATE, b.linearrive as ARRIVEDAT, b.rcomdrop, '' as gateway, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, a.usekind as CASEKIND, '' as paycycle, " &_
				"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
				"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
				"a.cusid, a.entryno, b.bussid as salesid, b.consignee, " &_
				"j.faqman, j.tel, j.mobile, k.codenc as faqreason, j.memo, l.name as rcvusr, j.rcvdat, " &_
				"m.name as closeusr, j.closedat " &_
				"from 	RTSparqAdslCust a " &_
				"inner join RTSparqAdslCmty b on a.comq1 = b.cutyid " &_
				"inner join HBAdslCmtyCust c on a.cusid = c.cusid and a.entryno = c.entryno " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"inner join RTFaqM j on j.cusid = a.cusid and j.entryno = a.entryno " &_
				"left outer join RTCode k on k.code = j.faqreason and k.kind ='P7' " &_
				"left outer join RTEmployee l on l.emply = j.rcvusr " &_
				"left outer join RTEmployee m on m.emply = j.closeusr " &_
				"where 	c.comtype ='3' " &_
				"and j.caseno ='" &aryparmkey(2)& "' "
		case "5"
			sql="select c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, " &_
				"b.lineip as CMTYIP, f.codenc as LINERATE, b.hinetnotifydat as ARRIVEDAT, b.dropdat as RCOMDROP, b.gateway, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, g.codenc as CASEKIND, h.codenc as paycycle, " &_
				"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
				"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
				"a.cusid, null as entryno, b.salesid, b.consignee, " &_
				"j.faqman, j.tel, j.mobile, k.codenc as faqreason, j.memo, l.name as rcvusr, j.rcvdat, " &_
				"m.name as closeusr, j.closedat " &_
				"from 	RTEbtCust a " &_
				"inner join RTEbtCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
				"inner join RTEbtCmtyH d on d.comq1 = b.comq1 " &_
				"inner join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"left outer join RTCode g on g.code = a.casetype and g.kind ='H5' " &_
				"left outer join RTCode h on h.code = a.paytype and h.kind ='G6' " &_
				"inner join RTFaqM j on j.comq1 = a.comq1 and j.lineq1 = a.lineq1 and j.cusid = a.cusid " &_
				"left outer join RTCode k on k.code = j.faqreason and k.kind ='P7' " &_
				"left outer join RTEmployee l on l.emply = j.rcvusr " &_
				"left outer join RTEmployee m on m.emply = j.closeusr " &_
				"where 	c.comtype ='5' " &_
				"and j.caseno ='" &aryparmkey(2)& "' "
		case "6"
			sql="select c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, " &_
				"replace(str(b.lineipstr1) +'.'+ str(b.lineipstr2) +'.'+ str(b.lineipstr3) +'.'+ str(b.lineipstr4) +'~'+ str(b.lineipend),' ','')  as CMTYIP, " &_
				"f.codenc as LINERATE, b.linearrivedat as ARRIVEDAT, b.dropdat as RCOMDROP, b.gateway, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.custip1+'.'+ a.custip2 +'.'+ a.custip3 +'.'+ a.custip4, '...', '') as CUSTIP, " &_
				"g.codenc as CASEKIND, h.codenc as paycycle, " &_
				"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
				"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
				"a.cusid, null as entryno, b.salesid, b.consignee, " &_
				"j.faqman, j.tel, j.mobile, k.codenc as faqreason, j.memo, l.name as rcvusr, j.rcvdat, " &_
				"m.name as closeusr, j.closedat " &_
				"from 	RTSparq499Cust a " &_
				"inner join RTSparq499CmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
				"inner join RTSparq499CmtyH d on d.comq1 = b.comq1 " &_
				"inner join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"left outer join RTCode g on g.code = a.casetype and g.kind ='L9' " &_
				"left outer join RTCode h on h.code = a.paytype and h.kind ='M1' " &_
				"inner join RTFaqM j on j.comq1 = a.comq1 and j.lineq1 = a.lineq1 and j.cusid = a.cusid " &_				
				"left outer join RTCode k on k.code = j.faqreason and k.kind ='P7' " &_
				"left outer join RTEmployee l on l.emply = j.rcvusr " &_
				"left outer join RTEmployee m on m.emply = j.closeusr " &_
				"where 	c.comtype ='6' " &_
				"and j.caseno ='" &aryparmkey(2)& "' "
		case "7"
			sql="select	c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, " &_
				"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, b.gateway, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11+'.'+a.ip12+'.'+a.ip13+'.'+a.ip14,'...','') as CUSTIP, " &_
				"g.codenc as CASEKIND, h.codenc as paycycle, i.codenc as paytype, replace(a.overdue,'N','') as overdue, " &_
				"replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, a.newbillingdat, a.duedat, c.dropdat, c.canceldat, " &_
				"a.cusid, null as entryno, b.salesid, b.consignee, " &_
				"j.faqman, j.tel, j.mobile, k.codenc as faqreason, j.memo, l.name as rcvusr, j.rcvdat, " &_
				"m.name as closeusr, j.closedat " &_
				"from 	RTLessorAvsCust a " &_
				"inner join RTLessorAvsCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
				"inner join RTLessorAvsCmtyH d on d.comq1 = b.comq1 " &_
				"inner join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"left outer join RTCode g on g.code = a.casekind and g.kind ='O9' " &_
				"left outer join RTCode h on h.code = a.paycycle and h.kind ='M8' " &_
				"left outer join RTCode i on i.code = a.paytype and i.kind ='M9' " &_
				"inner join RTFaqM j on j.comq1 = a.comq1 and j.lineq1 = a.lineq1 and j.cusid = a.cusid " &_
				"left outer join RTCode k on k.code = j.faqreason and k.kind ='P7' " &_
				"left outer join RTEmployee l on l.emply = j.rcvusr " &_
				"left outer join RTEmployee m on m.emply = j.closeusr " &_
				"where 	c.comtype ='7' " &_
				"and j.caseno ='" &aryparmkey(2)& "' "
		case "8"
			sql="select	c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, " &_
				"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, b.gateway, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11+'.'+a.ip12+'.'+a.ip13+'.'+a.ip14,'...','') as CUSTIP, " &_
				"g.codenc as CASEKIND, h.codenc as paycycle, i.codenc as paytype, replace(a.overdue,'N','') as overdue, " &_
				"replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, a.newbillingdat, a.duedat, c.dropdat, c.canceldat, " &_
				"a.cusid, null as entryno, b.salesid, b.consignee, " &_
				"j.faqman, j.tel, j.mobile, k.codenc as faqreason, j.memo, l.name as rcvusr, j.rcvdat, " &_
				"m.name as closeusr, j.closedat " &_
				"from 	RTLessorCust a " &_
				"inner join RTLessorCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
				"inner join RTLessorCmtyH d on d.comq1 = b.comq1 " &_
				"inner join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"left outer join RTCode g on g.code = a.casekind and g.kind ='O9' " &_
				"left outer join RTCode h on h.code = a.paycycle and h.kind ='M8' " &_
				"left outer join RTCode i on i.code = a.paytype and i.kind ='M9' " &_
				"inner join RTFaqM j on j.comq1 = a.comq1 and j.lineq1 = a.lineq1 and j.cusid = a.cusid " &_
				"left outer join RTCode k on k.code = j.faqreason and k.kind ='P7' " &_
				"left outer join RTEmployee l on l.emply = j.rcvusr " &_
				"left outer join RTEmployee m on m.emply = j.closeusr " &_
				"where 	c.comtype ='8' " &_
				"and j.caseno ='" &aryparmkey(2)& "' "
		case "9"
			sql="select	c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, " &_
				"b.lineip  as CMTYIP, f.codenc as LINERATE, b.arrivedat, b.dropdat as RCOMDROP, b.gateway, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11+'.'+a.ip12+'.'+a.ip13+'.'+a.ip14,'...','') as CUSTIP, " &_
				"'' as CASEKIND, '' as paycycle, '' as paytype,'' as overdue, replace(a.freecode,'N','') as freecode, c.docketdat, " &_
				"a.strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, a.cusid, null as entryno, d.salesid, d.consignee, j.faqman, " &_
				"j.tel, j.mobile, k.codenc as faqreason, j.memo, l.name as rcvusr, j.rcvdat, m.name as closeusr, j.closedat " &_
				"from 	RTPrjCust a " &_
				"inner join RTPrjCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
				"inner join RTPrjCmtyH d on d.comq1 = b.comq1 " &_
				"inner join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"inner join RTFaqM j on j.comq1 = a.comq1 and j.lineq1 = a.lineq1 and j.cusid = a.cusid " &_
				"left outer join RTCode k on k.code = j.faqreason and k.kind ='P7' " &_
				"left outer join RTEmployee l on l.emply = j.rcvusr " &_
				"left outer join RTEmployee m on m.emply = j.closeusr " &_
				"where 	c.comtype ='9' " &_
				"and j.caseno ='" &aryparmkey(2)& "' "
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
	<tr>
		<td class=dataListsearch>用戶名稱</td>
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
		<td class=dataListsearch>用戶電話</td>
		<td bgcolor="silver" colspan=3>
			<input type="text" readonly size="50" name="colContacttel" value="<%=contacttel%>" class="dataListsearch3" ID="Text31">			
		</td>
	    <td class=dataListsearch>行動電話</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="29" name="colCompanytel" value="<%=companytel%>" class="dataListsearch3" ID="Text30">
		</td>
	</tr>
	<tr>
		<td class=dataListsearch>用戶編號</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="15" name="colCusid" value="<%=cusid%>" class="dataListsearch3" ID="Text28">
			<font size=2 color=black>項次: </font>
			<input type="text" readonly size="2" name="colEntryno" value="<%=entryno%>" class="dataListsearch3" ID="Text4">
		</td>
	    <td class=dataListsearch>公關機</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="2" name="colFreecode" value="<%=freecode%>" class="dataListsearch3" style="color:red;" ID="Text5">
		</td>
	    <td class=dataListsearch>用戶IP</td>
		<td bgcolor="silver" >
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
<br>
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
					"and a.caseno ='" & aryparmkey(2) & "' "
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
</div>
<br>
<DIV ID="Div5">
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table4">
	<tr><td bgcolor="BDB76B" align="center">施工內容</td></tr>
</table>
</DIV>

<DIV ID="Div6" > 
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table5">
    <tr class="dataListHEAD"><td>派工單號</td><td>派工別</td><td nowrap>完工別</td><td>實際<br>完工人</td><td>施工內容</td></tr>
	<%   
		sqlfaqlist=	"select a.workno, b.codenc as worktypenc, c.codenc as finishtypnc, isnull(d.shortnc, f.cusnc) as finishnc, a.memo " &_
					"from RTSndWork a " &_
					"left outer join RTCode b on a.worktype = b.code and b.kind ='P6' " &_
					"left outer join RTCode c on a.finishtyp = c.code and c.kind ='P9' " &_
					"left outer join RTObj d on d.cusid = a.finishcons " &_
					"left outer join RTEmployee e inner join RTObj f on e.cusid = f.cusid on e.emply = a.finisheng " &_
					"where a.canceldat is null " &_
					"and a.workno ='" & dspkey(1) & "' "
		rs.open sqlfaqlist,conn
		Do While Not rs.Eof
			response.Write	"<tr class=""dataListentry"">" &_
							"<td>"& rs("workno") & "</td>" &_
							"<td>"& rs("worktypenc") & "</td>" &_	
							"<td>"& rs("finishtypnc") & "</td>" &_	
							"<td>"& rs("finishnc") & "</td>" &_	
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
		<tr><td bgcolor="BDB76B" align="center">電訪資料</td></tr>
    </table>
</DIV>

<DIV ID=SRTAB2 >
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3">
	<tr>
		<td width="10%" class=dataListHEAD>電訪客戶</td>
		<td width="23%" bgcolor="silver">   
			<% IF DSPKEY(4)="" THEN DSPKEY(4)=CUSNC %>
			<input type="text" size="30" maxlength=50 name="key4" value="<%=dspKey(4)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListentry" ID="Text53">
		</td>
		<td width="10%" class="dataListHEAD" height="23">電訪日期</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" name="key2" size="25" READONLY value="<%=dspKey(2)%>"  class="dataListDATA" ID="Text9">
		</td> 
		<td width="10%" class="dataListHEAD" height="23">電訪人員</td>
		<td width="23%" bgcolor="silver">
			<input type="text" name="key3" size="6" READONLY value="<%=dspKey(3)%>" class="dataListDATA">
			<font size=2><%=SrGetEmployeeName(dspKey(3))%></font>
		</td>  
	</tr>

	<tr><td  class="dataListHEAD" height="23">異動人員</td>
        <td  height="23" bgcolor="silver">
			<input type="text" name="key6" size="6" READONLY value="<%=dspKey(6)%>" class="dataListDATA" ID="Text10">
			<font size=2><%=SrGetEmployeeName(dspKey(6))%></font>
        </td>
        <td  class="dataListHEAD" height="23">異動時間</td>
        <td  height="23" bgcolor="silver" colspan=3>
	        <input type="text" name="key7" size="25" READONLY value="<%=dspKey(7)%>" class="dataListDATA" ID="Text11">
        </td>
	</tr>

	<tr><td  class="dataListHEAD" height="23">作廢人員</td>
        <td  height="23" bgcolor="silver">
			<input type="text" name="key12" size="6" READONLY value="<%=dspKey(8)%>" style="color:red;" class="dataListDATA" ID="Text12">
			<font size=2 color=red><%=SrGetEmployeeName(dspKey(8))%></font>
        </td>
        <td  class="dataListHEAD" height="23">作廢時間</td>
        <td  height="23" bgcolor="silver" colspan=3>
	        <input type="text" name="key9" size="25" READONLY value="<%=dspKey(9)%>" style="color:red;" class="dataListDATA" ID="Text13">
        </td>
	</tr>

	<tr><td class="dataListHEAD">滿意度評分</td>
		<td bgcolor="silver" colspan=5>
			<table>
			<%
				Set rsc=Server.CreateObject("ADODB.Recordset")
				sql="select a.code, a.codenc, isnull(b.score,0) as score, isnull(b.unsatiscode,'') as unsatiscode " &_
					"from RTCode a left outer join RTSndWorkCallSub b on a.code = b.satiscode and b.callno ='" &dspkey(0)& "' " &_
					"where a.kind ='Q1' "
				'response.Write sql
				rs.Open sql,conn
				Do While Not rs.Eof
					s="<tr><td>" & rs("codenc") & "</td><td><select name=""ext"&rs("code")&"""><option value=0></option>"
					aryOption=Array("(5分)非常滿意","(4分)滿意","(3分)普通","(2分)不滿意","(1分)非常不滿意")
					For i = 0 To Ubound(aryOption)
						if rs("score") = 5-i then 
							sx=" selected "
						else
							sx=" "
						end if
						s=s & "<option value="""& 5-i &"""" &sx&">" &aryOption(i)& "</option>"
					next	
					
					s=s &"</select></td><td>不滿意的原因：<select name=""extA"&rs("code")&"""><option value=""""></option>"
					sqlsub ="select subcode, subcodenc from RTCodeSub where kind ='Q1' and code='" &rs("code")& "' "
					rsc.Open sqlsub,conn
					Do While Not rsc.Eof
						if rsc("subcode") = rs("unsatiscode") then 
							sx=" selected "
						else
							sx=" "
						end if
						s=s &"<option value=""" &rsc("subcode") &"""" &sx &">" &rsc("subcodenc") &"</option>"
						rsc.movenext
					Loop
					rsc.Close
					
					rs.movenext
					s=s &"</select></td>"
					response.Write s
				Loop
				rs.Close
			%>         
			</table>
		</td>
	</tr>

	<tr><td class="dataListHEAD">電訪備註</td>
		<td bgcolor="silver" colspan=5>
			<TEXTAREA cols="100%" name="key5" rows=10 MAXLENGTH=800 <%=fieldpa%> class="dataListentry" <%=dataprotect%> value="<%=dspkey(5)%>" ID="Textarea2"><%=dspkey(5)%></TEXTAREA>
		</td>
	</tr>
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
