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
    rs.Open sql,conn,3,3
    If rs.Eof Or rs.Bof Then
       If accessMode="A" Then
          rs.AddNew
          For i = 0 To numberOfField-1
			  'BY 欄位資料型別填入預設值
              sType=Right("000" &Cstr(aryKeyType(i)),3)
              If Instr(cTypeNumeric,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=0
			  If Instr(cTypeChar,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=""
              If Instr(cTypeDate,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=Null				  
              If Instr(cTypeAuto,sType) > 0 Or (dspKey(i)=-1 And i<numberOfKey) Then
              Else
              '   On Error Resume Next
					'runpgm=Request.ServerVariables("PATH_INFO") 
                   ' 當程式為ADSL社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
                     'if i=16 then
                     '   Set rsc=Server.CreateObject("ADODB.Recordset")
                     '   rsc.open "select max(BATCH) AS batch from RTInvoice ",conn
                     '   if len(rsc("batch")) > 0 then
                     '      dspkey(i)=rsc("batch") + 1
                     '   else
                     '      dspkey(i)=1
                     '   end if
					 '   rsc.close
					 'Set rsc=Nothing
					 'end if
					if i=0 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         cusidxx="E" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
                         rsc.open "select max(cusid) AS cusid from RTLessorAVSCust where cusid like '" & cusidxx & "%' " ,conn
                         if len(rsc("cusid")) > 0 then
                            dspkey(2)=cusidxx & right("000" & cstr(cint(right(rsc("cusid"),3)) + 1),3)
                         else
                            dspkey(2)=cusidxx & "001"
                         end if
                         rsc.close
                         rs.Fields(i).Value=dspKey(i) 
					end if      
					rs.fields(i).value=dspkey(i)
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
              runpgm=Request.ServerVariables("PATH_INFO") 
              select case ucase(runpgm)   
                 ' 當程式為ADSL社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/KTSCUST/KTScustd.asp")
                  ' response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                     if i<>0  then rs.Fields(i).Value=dspKey(i)         
                 case else
                     rs.Fields(i).Value=dspKey(i)
                   '  response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
               end select
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
       if ucase(runpgm)=ucase("/webap/rtap/base/RTInvoice/RTInvoiceD.asp") then
          rs.open "select max(BATCH) AS batch from RTInvoice ",conn
          if not rs.eof then
			 dspkey(16)=rs("batch")
		  end if
		  rs.close
       end if
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
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
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
<input type="text" name="sw" value="<%=sw%>" style="display:none;" ID="Text17" size="20">
<input type="text" name="reNew" value="N" style="display:none;" ID="Text18" size="20">
<input type="text" name="rwCnt" value="<%=rwCnt%>" style="display:none;" ID="Text19" size="20">
<input type="text" name="accessMode" value="<%=accessMode%>" style="display:none;" ID="Text20" size="20">
<table width="100%" ID="Table1">
  <tr class=dataListTitle><td width="20%">　</td><td width="60%" align=center>
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
  title="發票資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT INVNO, INVDAT, INVTITLE, UNINO, CHKNO, TAXTYPE, SALESUM, TAXSUM, TOTALSUM, "_
			 &"INVTYPE, AMTC, INVDATC, CANCELDAT, MEMO, UUSR, UDAT, BATCH, CASETYPE, ARSRC, BATCHNO "_
			 &"FROM RTInvoice WHERE INVNO ='' "
  sqlList=	"SELECT INVNO, INVDAT, INVTITLE, UNINO, CHKNO, TAXTYPE, SALESUM, TAXSUM, TOTALSUM, "_
			 &"INVTYPE, AMTC, INVDATC, CANCELDAT, MEMO, UUSR, UDAT, BATCH, CASETYPE, ARSRC, BATCHNO "_
			 &"FROM RTInvoice WHERE "
  userDefineRead="Yes"
  userDefineSave="Yes"
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
	'發票號碼
	DSPKEY(0)= UCase(DSPKEY(0))
	'發票金額
	IF LEN(TRIM(DSPKEY(6))) = 0 THEN DSPKEY(6)=0
	IF LEN(TRIM(DSPKEY(7))) = 0 THEN DSPKEY(7)=0
	IF LEN(TRIM(DSPKEY(8))) = 0 THEN DSPKEY(8)=0
	'列印批次
	IF LEN(TRIM(DSPKEY(10))) = 0 THEN DSPKEY(10)="00000000"
	'列印批次
	IF LEN(TRIM(DSPKEY(16))) = 0 THEN DSPKEY(16)=0

	IF ERRFLAG <> "Y" THEN
		invcheckno = CheckINVNO(dspkey(0))
		if len(invcheckno) = 1 then
			dspkey(4) = invcheckno
		else
			formValid=False
			message = invcheckno
		end if
       
		if len(trim(dspkey(0))) <> 10 then
			formValid=False
			message="發票號碼須10位數"
		elseif len(trim(dspkey(1)))=0 or Not Isdate(dspkey(1)) then
			formValid=False
			message="發票日期不可空白或格式錯誤"
		elseif len(trim(dspkey(2)))=0 then
			'formValid=False
			message="發票抬頭不可空白"
		elseif len(trim(dspkey(3))) <> 0 and len(trim(dspkey(3))) <> 8 then
			formValid=False
			message="公司統編須8位數或空白"
		END IF  
 
	END IF

	'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" or dspmode="修改" then
		'最後修改人員&日期
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(14)=V(0)
        dspkey(15)=now()
        
		'發票日(轉民國)
		if Isdate(dspkey(1)) then
			dspkey(11) = Right("0" & CStr(DatePart("yyyy", dspkey(1)) -1911), 3) &_
						 Right("0" & Cstr(DatePart("m", dspkey(1))), 2) &_
						 Right("0" & Cstr(DatePart("d", dspkey(1))), 2)
		end if	
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
       clickkey="C" & clickid
       clearkey="key" & clickid    
       if len(trim(document.all(clearkey).value)) <> 0 then
          document.all(clearkey).value =  ""
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
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<%	
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()
	'申請轉檔日,完工日,報竣日 三者任一欄有值就鎖定fieldPC, fieldPD
    'If len(trim(dspKey(33))) > 0 OR len(trim(dspKey(38))) > 0 OR len(trim(dspKey(39))) > 0 OR len(trim(dspKey(40))) > 0 Then
    '   fieldPC=" class=""dataListData"" readonly "
    '   fieldpD=" disabled "
    'Else
    '   fieldPC=""
    '   fieldpD=""
    'End If%>
    
<!--
	<table width="60%" border=1 cellPadding=0 cellSpacing=0>
		<tr><td width="10%" class=dataListHead>用戶代號</td>
			<td width="10%"  bgcolor="silver">
				<input type="text" name="key0" <%=fieldRole(1)%> readonly size="15" value="<%=dspKey(0)%>" maxlength="15" class=dataListdata>
			</td>
			<td WIDTH=10%  class="dataListsearch" height="23">NCIC用戶編號</td>                                 
			<td WIDTH=10% height="23" bgcolor="silver" >
				<input type="text" name="key36" size="15" value="<%=dspKey(36)%>"  <%=fieldpC%> class="dataListENTRY" ID="Text3">     
			</td>
		</tr>
	</table>
-->
	
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
'    logonid=session("userid")
'    if dspmode="新增" then
'        if len(trim(dspkey(50))) < 1 then
'           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
'                V=split(rtnvalue,";")  
'                dspkey(50)=V(0)
'        End if  
'       dspkey(51)=datevalue(now())
'    else
'        if len(trim(dspkey(52))) < 1 then
'           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
'                V=split(rtnvalue,";")  
'                DSpkey(52)=V(0)
'        End if         
'        dspkey(53)=datevalue(now())
'    end if      
' -------------------------------------------------------------------------------------------- 

    '用戶送件申請後,基本資料 protect
    If len(trim(dspKey(0))) > 0 Then
       fieldPa=" class=""dataListData"" readonly "
       'fieldpb=" disabled "
    Else
       fieldPa=" class=""dataListEntry"" "
       'fieldpb=""
    End If

	Dim conn,rs,s,sx,sql,t      
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
%>
                                                    
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
<tr><td width="14%" class=dataListHEAD>發票號碼</td>
	<td width="20%" bgcolor="silver" >
        <input type="text" name="key0" ID="Text0" size="12" maxlength="10" 
               value="<%=dspKey(0)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> >
	</td>
	<td width="14%" class=dataListHEAD>檢查號</td>
	<td width="20%" bgcolor="silver">
        <input type="text" name="key4" ID="Text4" size="3" READONLY class=dataListData
               value="<%=dspKey(4)%>"  <%=fieldRole(1)%><%=dataProtect%> >
	</td>               
</tr>

<tr><td width="14%" class=dataListHEAD>發票抬頭</td>
	<td width="20%" bgcolor="silver" >
        <input type="text" name="key2" ID="Text2" size="45" maxlength="50" class=dataListENTRY 
               value="<%=dspKey(2)%>"  <%=fieldRole(1)%><%=dataProtect%> >
	</td>
	<td width="14%" class=dataListHEAD>發票日期</td>
    <td width="20%" bgcolor="silver" >
		<input type="text" name="key1" ID="Date1" size="10" value="<%=dspKey(1)%>" READONLY class=dataListEntry <%=fieldRole(1)%><%=dataProtect%> >
		<input type="button" name="B1" id="B1" <%=fieldpb%> height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
		<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> id="C1" name="C1" alt="清除" style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
		<font id="Font1" color=Blue>(<%=dspKey(11)%>)</font>
	</td>
</tr>

<tr><td width="14%" class=dataListHEAD>公司統編</td>
	<td width="20%" bgcolor="silver" >
        <input type="text" name="key3" ID="Text3" size="12" maxlength="8" class=dataListENTRY 
               value="<%=dspKey(3)%>"  <%=fieldRole(1)%><%=dataProtect%> >
	</td>

	<td width="10%" class=dataListHEAD>課稅別</td>
	<%
		If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 Then  
			sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='P1' ORDER BY CODE" 
		Else
			sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='P1' AND CODE='" & dspkey(5) & "' "
		End If

		s=""
		rs.Open sql,conn
		Do While Not rs.Eof
			If rs("CODE")=dspkey(5) Then 
				sx=" selected "
			else 
				sx=""
			end if
			s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
			rs.MoveNext
		Loop
		rs.Close
	%>         
    <td width="20%" bgcolor="silver" >
		<select size="1" name="key5" ID="Select3" class="dataListEntry"  <%=FIELDROLE(1)%><%=dataProtect%>>
			<%=s%></select>
	</td>
</tr>

<tr><td width="14%" class=dataListHEAD>發票聯數</td>
	<%
		if dspKey(9) = 3 then 
			key9a =""
			key9b=" Selected "
		else
			key9a=" Selected "
			key9b =""
		end if
	%>
	<td width="20%" bgcolor="silver" >
		<select name="key9" ID="Select9" class="dataListEntry"  <%=FIELDROLE(1)%><%=dataProtect%>>
			<option value="2" <%=key9a%> >2</option>
			<option value="3" <%=key9b%> >3</option>
		</select>
	</td>

	<td width="14%" class=dataListHEAD>作廢日期</td>
    <td width="20%" bgcolor="silver" >
		<input type="text" name="key12" ID="Date12" size="10" value="<%=dspKey(12)%>" READONLY class=dataListEntry <%=fieldRole(1)%><%=dataProtect%> >
		<input type="button" name="B12" id="B12" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
		<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" name="C12" id="C12" <%=fieldpb%> alt="清除" style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
	</td>
</tr>

<tr><td width="14%" class=dataListHEAD>銷售額</td>
	<td width="20%" bgcolor="silver" >
        <input type="text" name="key6" ID="Text6" size="10" READONLY class=dataListData
               value="<%=dspKey(6)%>"  <%=fieldRole(1)%><%=dataProtect%> >
	</td>
	<td width="14%" class=dataListHEAD>發票總金額</td>
	<td width="20%" bgcolor="silver">
        <input type="text" name="key8" ID="Text8" size="10" READONLY class=dataListData
               value="<%=dspKey(8)%>"  <%=fieldRole(1)%><%=dataProtect%> >
		<font id="Font1" color=Blue>(<%=dspKey(10)%>)</font>
	</td>
</tr>

<tr><td width="14%" class=dataListHEAD>稅額</td>
	<td width="20%" bgcolor="silver" >
        <input type="text" name="key7" ID="Text7" size="10" value="<%=dspKey(7)%>" READONLY class=dataListData >
	</td>
	<td width="14%" class=dataListHEAD>列印批次</td>
	<td width="20%" bgcolor="silver">
        <input type="text" name="key16" ID="Text16" size="10" value="<%=dspKey(16)%>" READONLY class=dataListData>
	</td>
</tr>

<tr><td class="dataListHEAD" height="23">方案別</td>
	<td height="23" bgcolor="silver">
		<input type="text" name="key17" size="3" READONLY value="<%=dspKey(17)%>" class="dataListDATA">
		<%
			sql="SELECT CODENC FROM RTCODE WHERE KIND='L5' AND CODE='" & dspkey(17) & "' "
			rs.Open sql,conn
			if Not rs.Eof then
				response.write "<font size=2>" &rs("CODENC")& "</font>"
			end if
			rs.Close
		%>         
	</td>

	<td class="dataListHEAD" height="23">應收帳款</td>                                 
	<td height="23" bgcolor="silver">
		<font size=2>來源別：</font>
        <input type="text" name="key18" ID="Date18" size="2" READONLY value="<%=dspKey(18)%>" class="dataListDATA">
		<%
			sql="SELECT CODENC FROM RTCODE WHERE KIND='R1' AND CODE='" & dspkey(18) & "' "
			rs.Open sql,conn
			if Not rs.Eof then
				response.write "<font size=2>" &rs("CODENC")& "</font>"
			end if
			rs.Close
		%>         
		<font size=2>　編號：</font>
        <input type="text" name="key19" ID="Date19" size="13" READONLY value="<%=dspKey(19)%>" class="dataListDATA">		
	</td>
</tr>

<tr><td class="dataListHEAD" height="23">最後異動人員</td>
	<td height="23" bgcolor="silver">
		<input type="text" name="key14" ID="Text14" size="6" READONLY value="<%=dspKey(14)%>" class="dataListDATA">
		<font size=2><%=SrGetEmployeeName(dspKey(14))%></font>
	</td>

	<td class="dataListHEAD" height="23">最後異動日期</td>                                 
	<td height="23" bgcolor="silver">
        <input type="text" name="key15" ID="Date15" size="30" READONLY value="<%=dspKey(15)%>" class="dataListDATA">
	</td>
</tr>

<tr><td WIDTH="12%" class="dataListHEAD" height="23">備註</td>
	<TD align="CENTER" bgcolor="silver" colspan=3>
		<TEXTAREA cols="100%" name="key13" ID="Textarea1" rows=8  MAXLENGTH=250 class="dataListentry"
			<%=dataprotect%>  value="<%=dspkey(13)%>" ><%=dspkey(13)%>
		</TEXTAREA>
	</td>
</tr>

</table>

<table border="1" width="100%" cellpadding="0" cellspacing="0">
	<tr><td bgcolor="BDB76B" align="Center" colspan=7>發票明細</td></tr>
    <tr><td WIDTH=10% ALIGN="center" class=dataListDATA>項次</td>
		<td WIDTH=15% ALIGN="center" class=dataListDATA>產品名稱</td>
		<td WIDTH=15% ALIGN="center" class=dataListDATA>數量</td>
		<td WIDTH=15% ALIGN="center" class=dataListDATA>單價</td>
		<td WIDTH=15% ALIGN="center" class=dataListDATA>銷售額</td>
		<td WIDTH=15% ALIGN="center" class=dataListDATA>稅額</td>
		<td WIDTH=15% ALIGN="center" class=dataListDATA>合計</td>
	</tr>
    <%
		sql="SELECT ENTRY, PRODNC, QTY, UNITAMT, SALEAMT, TAXAMT, SALEAMT+TAXAMT as NUM FROM RTInvoiceSub where INVNO ='"& dspkey(0) &"' ORDER BY ENTRY "
		rs.Open sql,conn
		Do While Not rs.Eof
			RESPONSE.Write "<TR>"
			RESPONSE.Write "<td WIDTH=10% ALIGN=""center"" class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("ENTRY") &  "&nbsp;</FONT></td>"
			RESPONSE.Write "<td WIDTH=10% ALIGN=""center"" class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("PRODNC") & "&nbsp;</FONT></td>"
			RESPONSE.Write "<td WIDTH=10% ALIGN=""center"" class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("QTY") & "&nbsp;</FONT></td>"
			RESPONSE.Write "<td WIDTH=10% ALIGN=""center"" class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("UNITAMT") & "&nbsp;</FONT></td>"
			RESPONSE.Write "<td WIDTH=10% ALIGN=""center"" class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("SALEAMT") & "&nbsp;</FONT></td>"
			RESPONSE.Write "<td WIDTH=10% ALIGN=""center"" class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("TAXAMT") & "&nbsp;</FONT></td>"
			RESPONSE.Write "<td WIDTH=10% ALIGN=""center"" class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("NUM") & "&nbsp;</FONT></td>"
			RESPONSE.Write "</TR>"
			rs.MoveNext
		Loop
		rs.Close
    %>
</table>

<table border="1" width="100%" cellpadding="0" cellspacing="0">
 </table> 

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

End Sub
' -------------------------------------------------------------------------------------------- 
%>
<!-- #include virtual="/Webap/include/checkid.inc" -->
<!-- #include virtual="/Webap/include/companyid.inc" -->
<!-- #include file="RTGetUserRight.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
