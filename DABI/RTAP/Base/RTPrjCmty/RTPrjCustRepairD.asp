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
	Dim	rs,i,conn
	Set	conn=Server.CreateObject("ADODB.Connection")
	conn.open DSN
	Set	rs=Server.CreateObject("ADODB.Recordset")
	aryKeyName=Split(formatName,";")
	rs.Open	sqlFormatDB,conn
	For	i =	0 To rs.Fields.Count-1
	  aryKeyNameDB(i)=rs.Fields(i).Name
	  aryKeyType(i)=rs.Fields(i).Type
	Next
	numberOfField=rs.Fields.Count
	rs.Close
	conn.Close
	Set	rs=Nothing
	Set	conn=Nothing
  End Sub
' --------------------------------------------------------------------------------------------		  
  Sub SrInit(accessMode,sw)
	Dim	i
	aryKey=Split(";;;;;;;;;;",";")
	accessMode=Request.Form("accessMode")
	If accessMode="" Then
	   accessMode=Request("accessMode")
	   aryKey=Split(Request("key") &";;;;;;;;;;;;;;;;;;;;",";")
	   For i = 0 To	numberOfKey-1
		   dspKey(i)=aryKey(i)
	   Next
	End	IF
	sw=Request("sw")
	reNew=Request("reNew")
	rwCnt=Request("rwCnt")
	if Not IsNumeric(rwCnt)	Then rwCnt=0
  End Sub
' --------------------------------------------------------------------------------------------		  
  Sub SrClearForm()
	Dim	i,sType
	For	i =	0 To Ubound(aryParmKey)
	   If Len(Trim(aryParmKey(i))) > 0 Then
		   dspKey(i)=aryParmKey(i)
		End	If
	Next
'	 For i = 0 To numberOfField-1
'		 sType=Right("000" &Cstr(aryKeyType(i)),3) 
'		 If	Instr(cTypeChar,sType) > 0 Then
'			dspKey(i)=""
'		 ElseIf	Instr(cTypeNumeric,sType) >	0 Then
'			dspKey(i)=0
'		 ElseIf	Instr(cTypeDate,sType) > 0 Then
'			dspKey(i)=Now()
'		 ElseIf	Instr(cTypeBoolean,sType) >	0 Then
'			dspKey(i)=True
'		 Else
'			dspKey(i)=""
'		 End If
'	 Next
  End Sub
' -------------------------------------------------------------------------------------------- 
  Sub SrReceiveForm
	Dim	i
	For	i =	0 To numberOfField-1
		dspKey(i)=Request.Form("key" &i)
	Next
	If extDBField >	0 Then
	   For i = 0 To	extDBField-1
		   extDB(i)=Request.Form("ext" &i)
	   Next
	End	If
	If extDBField2 > 0 Then
	   For i = 0 To	extDBField2-1
		   extDB2(i)=Request.Form("extA" &i)
	   Next
	End	If
	If extDBField3 > 0 Then
	   For i = 0 To	extDBField3-1
		   extDB3(i)=Request.Form("extB" &i)
	   Next
	End	If		  
	If extDBField4 > 0 Then
	   For i = 0 To	extDBField4-1
		   extDB4(i)=Request.Form("extC" &i)
	   Next
	End	If			  
  End Sub
' -------------------------------------------------------------------------------------------- 
  Sub SrCheckForm(message,formValid)
	formValid=True
	message=""
	Call SrCheckData(message,formValid)
  End Sub
' -------------------------------------------------------------------------------------------- 
  Function GetSql()
	Dim	sql,i,sType
	sql=""
	For	i =	0 To numberOfKey-1
	  If i > 0 Then	sql=sql	&" AND "
	  sType=Right("000"	&Cstr(aryKeyType(i)),3)
	  If Instr(cTypeChar,sType)	> 0	Or dspKey(i)=Null Then	
		 sql=sql &"[" &aryKeyNameDB(i) &"]='" &dspKey(i) &"'"
	  'edson 2001/11/1 增加==>為了日期欄位能當key使用..必須有單引號
	  elseIf Instr(cTypedate,sType)	> 0	Or dspKey(i)=Null Then 
		  sql=sql &"[" &aryKeyNameDB(i)	&"]='" &dspKey(i) &"'"		   
	  Else
		 sql=sql &"[" &aryKeyNameDB(i) &"]=" &dspKey(i)
	  End If
	Next
	GetSql=sqlList &sql	&";"
  '	 response.write	getsql
  End Function
' -------------------------------------------------------------------------------------------- 
  Sub SrSaveData(message)
	message=msgSaveOK
	Dim	sql,i,sType
	sql=GetSql()
	Dim	conn,rs
	Set	conn=Server.CreateObject("ADODB.Connection")
	conn.open DSN
	Set	rs=Server.CreateObject("ADODB.Recordset")
   ' RESPONSE.WRITE	SQL
	rs.Open	sql,conn,3,3
	If rs.Eof Or rs.Bof	Then
	   If accessMode="A" Then
		  rs.AddNew
		  For i	= 0	To numberOfField-1
			  sType=Right("000"	&Cstr(aryKeyType(i)),3)
			  If Instr(cTypeDate,sType)	> 0	And	Len(Trim(dspKey(i))) = 0 Then dspKey(i)=Null
			  If Instr(cTypeAuto,sType)	> 0	Or (dspKey(i)=-1 And i<numberOfKey)	Then
			  Else
			  '	  On Error Resume Next
				runpgm=Request.ServerVariables("PATH_INFO")	
				select case	ucase(runpgm)	
				   ' 當程式為ADSL社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
				   case	ucase("/webap/rtap/base/RTPrjcmty/RTPrjCUSTRepaird.asp")
					'response.write	"I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
					   if i	<> 1 then rs.Fields(i).Value=dspKey(i)	  
					   if i=1 then
						 Set rsc=Server.CreateObject("ADODB.Recordset")
						 rsc.open "select max(entryno) AS entryno from RTPrjCustRepair where	CUSID='" & DSPKEY(0) & "' "	,conn
						 if	len(trim(rsc("entryno"))) >	0 then 
							dspkey(1)=rsc("entryno") + 1
						 else
							dspkey(1)=1
						 end if
						 rsc.close
						 rs.Fields(i).Value=dspKey(i) 
					   end if	   
				   case	else
						rs.Fields(i).Value=dspKey(i)	  
				END	SELECT
			  End if
		  Next
		  rs.Update
		  rwCnt=rwCnt+1
		  If userDefineSave="Yes" Then Call	SrSaveExtDB("A")
	   Else
		  message=msgErrorRec
	   End If
	Else
	   If accessMode="A" Then
		  message=msgDupKey
		  sw="E"
	   Else
		  For i	= 0	To numberOfField-1
			  sType=Right("000"	&Cstr(aryKeyType(i)),3)
			  If Instr(cTypeDate,sType)	> 0	And	Len(Trim(dspKey(i))) = 0 Then dspKey(i)=Null
		 '	   On Error	Resume Next
			  runpgm=Request.ServerVariables("PATH_INFO") 
			  select case ucase(runpgm)	  
				 ' 當程式為ADSL社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
				 case ucase("/webap/rtap/base/RTPrjCmty/RTPrjCustRepaird.asp")
					'response.write	"I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
					 rs.Fields(i).Value=dspKey(i)	 
				 case else
					 rs.Fields(i).Value=dspKey(i)
					 'response.write "I=" &	i &	";VALUE=" &	dspkey(i) &	"<BR>"
			   end select
		  Next
		  rs.Update
		  rwCnt=rwCnt+1
		  If userDefineSave="Yes" Then Call	SrSaveExtDB("U")
		  sw=""
	   End If
	End	If
	rs.Close
	' 當程式為HB社區基本資料維護作業時,將sql自行產生之identify值dspkey(0)讀出至畫面
	if accessmode ="A" then
	   runpgm=Request.ServerVariables("PATH_INFO")
	   if ucase(runpgm)=ucase("/webap/rtap/base/RTPrjcmty/RTPrjCustRepairD.asp") then
		  rsc.open "select max(entryno)	AS ENTRYNO from	RTPrjCustRepair where	 CUSID='" &	DSPKEY(0) &	"' " ,conn
		  if not rsC.eof then
			dspkey(1)=rsC("enTryno")
		  end if
		  rsC.close
	   end if
	end	if
	conn.Close
	Set	rs=Nothing
	Set	conn=Nothing
  End Sub
' -------------------------------------------------------------------------------------------- 
  Sub SrReadData(dataFound)
	dataFound=True
	Dim	 sql,i
	sql=GetSql()
	Dim	conn,rs
	Set	conn=Server.CreateObject("ADODB.Connection")
	conn.open DSN
	Set	rs=Server.CreateObject("ADODB.Recordset")
	'response.write	"SQL=" & SQL
	rs.Open	sql,conn
	If rs.Eof Then
	   dataFound=False
	Else
	   For i = 0 To	numberOfField-1
		   dspKey(i)=rs.Fields(i).Value
	   Next
	   If userDefineRead="Yes" Then	Call SrReadExtDB()
	End	If
	rs.Close
	conn.Close
	Set	rs=Nothing
	Set	conn=Nothing
  End Sub
' -------------------------------------------------------------------------------------------- 
  Sub SrSendForm(message)
	  Dim s,i,t,sType
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4EBT/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<script	language="vbscript">
Sub	Window_onLoad()
  window.Focus()
End	Sub
Sub	Window_onbeforeunload()
  dim rwCnt
  rwCnt=document.all("rwCnt").value
  If IsNumeric(rwCnt) Then
	 If	rwCnt >	0 Then Window.Opener.document.all("keyForm").Submit
  End If
  Window.Opener.Focus()
End	Sub
Sub	SrReNew()
  document.all("sw").Value="E"
  document.all("reNew").Value="Y"
  Window.form.Submit
End	Sub
</script>
</head>
<% if userdefineactivex="Yes" then
	  SrActiveX
	  SrActiveXScript
   End if
%>
<body bgcolor="#FFFFFF"	text="#0000FF"	background="backgroup.jpg" bgproperties="fixed">
<form method="post"	id="form">
<input type="text" name="sw" value="<%=sw%>" style="display:none;" ID="Text17">
<input type="text" name="reNew"	value="N" style="display:none;"	ID="Text18">
<input type="text" name="rwCnt"	value="<%=rwCnt%>" style="display:none;" ID="Text19">
<input type="text" name="accessMode" value="<%=accessMode%>" style="display:none;" ID="Text20">
<table width="100%"	ID="Table1">
  <tr class=dataListTitle><td width="20%">&nbsp;</td><td width="60%" align=center>
<%=title%></td><td width="20%" align=right><%=dspMode%></td></tr>
</table>
<%
  s=""
  If userDefineKey="Yes" Then
	 s=s &"<table width=""100%"" cellPadding=0 cellSpacing=0>" &vbCrLf _
		 &"	 <tr><td width=""70%"">" &vbCrLf 
	 Response.Write	s
	 SrGetUserDefineKey()
	 s=""
	 s=s &"		 </td>"	&vbCrLf	_
		 &"		 <td width=""30%"">" &vbCrLf _
		 &"			 <table	width=""100%"" cellPadding=0 cellSpacing=0>" &vbCrLf _
		 &"			   <tr><td class=dataListMessage>" &message	&"</td></tr>" &vbCrLf _
		 &"			   <tr align=""right""><td>&nbsp;</td><td align=""right"">"	&strBotton &"</td></tr>" &vbCrLf _
		 &"			 </table></td></tr>" &vbCrLf _
		 &"</table>" &vbCrLf
	 Response.Write	s
  Else 
	 s=s &"<table width=""100%"">" &vbCrLf _
		 &"	 <tr><td width=""60%"">" &vbCrLf _
		 &"		 <table	width=""100%"">" &vbCrLf 
	 For i = 0 To numberOfKey-1
	 s=s &"		  <tr><td width=""30%""	class=dataListHead>" &aryKeyName(i)	&"</td>" _
		  &"<td	width=""70%"">"	_
		  &"<input class=dataListEntry type=""text"" name=""key" &i	&""" " &keyProtect _
		  &" size=""20"" value=""" &dspKey(i) &"""></td></tr>" &vbCrLf
	 Next
	 s=s &"		 </table></td>"	&vbCrLf	_
		 &"		 <td width=""40%"">" &vbCrLf _
		 &"			 <table	width=""100%"">" &vbCrLf _
		 &"			   <tr><td class=dataListMessage>" &message	&"</td></tr>" &vbCrLf _
		 &"			   <tr><td>&nbsp;</td></tr>" &vbCrLf _
		 &"			   <tr><td>" &strBotton	&"</td></tr>" &vbCrLf _
		 &"			 </table></td></tr>" &vbCrLf _ 
		 &"</table>" &vbCrLf
	 Response.Write	s
  End If
  s=""
  If userDefineData="Yes" Then
	 SrGetUserDefineData()
  Else
	 s="<table width=""100%"">"	&vbCrLf
	 For i = numberOfKey To	numberOfField-1
	   sType=Right("000" &Cstr(aryKey(i)),3)
	   s=s &"  <tr><td width=""20%"" class=dataListHead>" &aryKeyName(i) &"</td>" &vbCrLf _
		   &"	   <td width=""80%"">" &vbCrLf
	   If Instr(cTypeVarChar,sType)	> 0	Then
		 s=s &"		 <textarea class=dataListEntry name=""key" &i &""" rows=""4"" cols=""40"" istextedit " _
			 &dataProtect &" style=""text-align:left;"">" &dspKey(i) &"</textarea></td></tr>" &vbCrLf 
	   ElseIf Instr(cTypeFloat,sType) >	0 Then
		 s=s &"		 <input	class=dataListEntry	type=""text"" name=""key" &i &""" size=""40"" "	_ 
			 &dataProtect &" style=""text-align:right;"" " _
			 &"value=""" &FormatNumber(dspKey(i)) &"""></td></tr>" &vbCrLf
	   ElseIf Instr(cTypeInteger,sType)	> 0	Then 
		 s=s &"		 <input	class=dataListEntry	type=""text"" name=""key" &i &""" size=""40"" "	_ 
			 &dataProtect &" style=""text-align:right;"" " _
			 &"value=""" &FormatNumber(dspKey(i),0)	&"""></td></tr>" &vbCrLf
	   Else
		 s=s &"		 <input	class=dataListEntry	type=""text"" name=""key" &i &""" size=""40"" "	_ 
			 &dataProtect &" style=""text-align:left;""	" _
			 &"value=""" &dspKey(i)	&"""></td></tr>" &vbCrLf
	   End If
	 Next
	 s=s &"</table>" &vbCrLf
	 Response.Write	s
  End If
%>
</form>
</body>
</html>
<%End Sub%>
<%
' -------------------------------------------------------------------------------------------- 
Sub	SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=2
  title="專案社區用戶維修收款資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB=	"SELECT	 CUSID,	ENTRYNO, APPLYDAT, EQUIP, EQUIPAMT,	SETAMT,	MOVEAMT, " &_
						"PAYTYPE, CREDITCARDTYPE, CREDITBANK, CREDITCARDNO,	CREDITNAME,	" &_
						"CREDITDUEM, CREDITDUEY, TARDAT, BATCHNO, TUSR,	" &_
						"FINISHDAT,	CANCELDAT, CANCELUSR, MEMO,	REALENGINEER, "	&_
						"REALCONSIGNEE,	UDAT, UUSR,	RCVMONEYDAT	" &_
				"FROM	RTPrjCustRepair WHERE	cusid='' "
  sqlList=		"SELECT	 CUSID,	ENTRYNO, APPLYDAT, EQUIP, EQUIPAMT,	SETAMT,	MOVEAMT, " &_
						"PAYTYPE, CREDITCARDTYPE, CREDITBANK, CREDITCARDNO,	CREDITNAME,	" &_
						"CREDITDUEM, CREDITDUEY, TARDAT, BATCHNO, TUSR,	" &_
						"FINISHDAT,	CANCELDAT, CANCELUSR, MEMO,	REALENGINEER, "	&_
						"REALCONSIGNEE,	UDAT, UUSR,	RCVMONEYDAT	" &_
				"from RTPrjCustRepair	WHERE "
  userDefineRead="Yes"		
  userDefineSave="Yes"		 
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=4
  userdefineactivex="Yes"
End	Sub
' -------------------------------------------------------------------------------------------- 
Sub	SrCheckData(message,formValid)
  IF len(trim(dspkey(1)))=0	then dspkey(1)=0	'Entryno
  IF len(trim(dspkey(4)))=0	then dspkey(4)=0	'AMT
  IF len(trim(dspkey(5)))=0	then dspkey(5)=0	'AMT
  IF len(trim(dspkey(6)))=0	then dspkey(6)=0	'AMT
  IF len(trim(dspkey(3)))=0	then dspkey(3)=""	'設備
  IF len(trim(dspkey(8)))=0	then dspkey(8)=""	'信用卡種類
  IF len(trim(dspkey(9)))=0	then dspkey(9)=""	'發卡銀行
  if len(trim(dspkey(2)))=0	then
	   formValid=False
	   message="派單日期不可空白"	
  elseif DSPKEY(4) <1 and DSPKEY(5) <1 and DSPKEY(6) <1 then
	   formValid=False
	   message="收款金額必須大於0"
  ELSEif len(trim(DSPKEY(22))) > 0 and len(trim(DSPKEY(21))) > 0 THEN
       formValid=False
       message="維修員工及維修經銷商只能輸入其中一項"
  elseif len(trim(dspkey(7)))=0	 then
	   formValid=False
	   message="繳費方式不可空白"		 
  elseif len(trim(dspkey(8)))=0	AND	dspkey(7)="01" then
	   formValid=False
	   message="信用卡種類不可空白"			   
  elseif len(trim(dspkey(9)))=0 AND dspkey(7)="01"	then
		formValid=Fals
		message="信用卡發卡銀行不可空白"		 
  elseif len(trim(dspkey(10)))=0 AND dspkey(7)="01"	then
	   formValid=False
	   message="信用卡卡號不可空白"			   
  elseif len(trim(dspkey(11)))=0 AND dspkey(7)="01"	then
	   formValid=False
	   message="信用卡持卡人不可空白"			 
  elseif (len(trim(dspkey(12)))=0 OR len(trim(dspkey(13)))=0 ) AND dspkey(7)="01" then
	   formValid=False
	   message="信用卡有效年月不可空白"					  
  elseif len(trim(dspkey(12)))<>0 and right("00" & dspkey(12),2)<="01" and right("00" &	dspkey(12),2)>="12"	then
	   formValid=False
	   message="信用卡有效月超出範圍(01-12)"	   
  elseif len(trim(dspkey(13)))<>0 and dspkey(13)< right(datepart("yyyy",now()),2) then
	   formValid=False
	   message="信用卡已過期(小於系統年)"	 
  elseif len(trim(dspkey(13)))<>0 and dspkey(13)< right(datepart("yyyy",now()),2) and len(trim(dspkey(12)))<>0 and dspkey(12) <	right("00" & datepart("m",now()),2)	then
	   formValid=False
	   message="信用卡已過期(小於系統月)"									 
  end if


 '檢查是否已存在續約資料(尚未完工或作廢)，若存在則不允許重複建檔。
' IF formValid=TRUE	and	accessMode="A" THEN
'	Set	connxx=Server.CreateObject("ADODB.Connection")
'	Set	rsxx=Server.CreateObject("ADODB.Recordset")
'	connxx.open	DSN
'	sqlxx="select count(*) as cnt from RTPrjcustCont where cusid='" &	aryparmkey(0) &	"' and finishdat is	null and canceldat is null "
'	rsxx.Open sqlxx,connxx
'	if rsxx("cnt") > 0 then
'	   formValid=False
'	   message="客戶續約檔尚有續約資料未完工，不可重複建立續約資料。"		  
'	end	if
'	rsxx.close
'	connxx.Close   
'	set	rsxx=Nothing   
'	set	connxx=Nothing 
' end if

'-------UserInformation----------------------		
	logonid=session("userid")
	if dspmode="修改" then
		Call SrGetEmployeeRef(Rtnvalue,1,logonid)
				V=split(rtnvalue,";")  
				DSpkey(24)=V(0)
		dspkey(23)=now()
	end	if		  
End	Sub
' -------------------------------------------------------------------------------------------- 
Sub	SrActiveXScript()%>
   <SCRIPT Language="VBScript">
   Sub Srbtnonclick()
	   Dim ClickID
	   ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
	   clickkey="KEY" &	clickid
	   if isdate(document.all(clickkey).value) then
		  objEF2KDT.varDefaultDateTime=document.all(clickkey).value
	   end if
	   call	objEF2KDT.show(1)
	   if objEF2KDT.strDateTime	<> "" then
		  document.all(clickkey).value = objEF2KDT.strDateTime
	   end if
   END SUB

   Sub SrClear()
	   Dim ClickID
	   ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
	   clickkey="C"	& clickid
	   clearkey="key" &	clickid	   
	   if len(trim(document.all(clearkey).value)) <> 0 then
		  document.all(clearkey).value =  ""
		  '當處理人員及處理廠商皆為空白時，才可清除此欄位資料
	   end if
   End Sub	  
  
   Sub ImageIconOver()
	   self.event.srcElement.style.borderBottom	= "black 1px solid"
	   self.event.srcElement.style.borderLeft="white 1px solid"
	   self.event.srcElement.style.borderRight="black 1px solid"
	   self.event.srcElement.style.borderTop="white	1px	solid"	 
   End Sub
   
   Sub ImageIconOut()
	   self.event.srcElement.style.borderBottom	= ""
	   self.event.srcElement.style.borderLeft=""
	   self.event.srcElement.style.borderRight=""
	   self.event.srcElement.style.borderTop=""
   End Sub
   
   Sub Srsales8onclick()
       prog="RTGetsalesD2.asp?KEY=;"
       'prog=prog & "?KEY=" & document.all("AREAID").VALUE 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
          FUsrID=Split(Fusr,";")   
          if Fusrid(2) ="Y" then
             document.all("key21").value =  trim(Fusrid(0))
          End if       
       end if
   End Sub

   </Script>
<%	 
End	Sub
' -------------------------------------------------------------------------------------------- 
Sub	SrActiveX()	%>
	<OBJECT	classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"   codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3"	
			height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT:	0px; TOP: 0px; WIDTH: 0px" 
			width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<%	
End	Sub
' -------------------------------------------------------------------------------------------- 
Sub	SrGetUserDefineKey()%>
	<table width="100%" border=1 cellPadding=0 cellSpacing=0>
		<tr><td width="15%" class=dataListHead>用戶序號</td>
			<td width="15%" bgcolor="silver">
				<input type="text" name="key0" readonly	size="15" value="<%=dspKey(0)%>" class=dataListdata>
			</td>
			<td width="10%" class=dataListHead>項次</td>
			<td width="15%" bgcolor="silver">
				<input type="text" name="key1" readonly	size="3" value="<%=dspKey(1)%>"	class=dataListdata>
			</td>
		</tr>
	</table>
<%
End	Sub
' -------------------------------------------------------------------------------------------- 
Sub	SrGetUserDefineData()
'-------UserInformation----------------------		
	logonid=session("userid")
	if dspmode="新增" then
		if len(trim(dspkey(24))) < 1 then
		   Call	SrGetEmployeeRef(Rtnvalue,1,logonid)
				V=split(rtnvalue,";")  
				dspkey(24)=V(0)
		End	if	
	   dspkey(23)=now()
	'else
	'	if len(trim(dspkey(24))) < 1 then
	'	   Call	SrGetEmployeeRef(Rtnvalue,1,logonid)
	'			V=split(rtnvalue,";")  
	'			DSpkey(24)=V(0)
	'	End	if		   
	'	dspkey(23)=now()
	end	if		
' -------------------------------------------------------------------------------------------- 
	Dim	conn,rs,s,sx,sql,t

	'用戶結案或作廢後,資料 protect
	If len(trim(dspKey(17))) > 0 or	len(trim(dspKey(18))) >	0 Then
	   fieldPa=" class=""dataListData""	readonly "
	   fieldpD=" disabled "
       protect=1
	Else
	   fieldPa=""
	   fieldpD=""
       protect=0
	end	if
	  
	Set	conn=Server.CreateObject("ADODB.Connection")
	Set	rs=Server.CreateObject("ADODB.Recordset")
	conn.open DSN
%>
  <!--
  <span	id="tags1" class="dataListTagsOn"
		onClick="vbscript:tag1.style.display=''	   :tags1.classname='dataListTagsOn':
						  tag2.style.display='none':tags2.classname='dataListTagsOf'">基本資料</span>
  <span	id="tags2" class="dataListTagsOf"
		onClick="vbscript:tag1.style.display='none':tags1.classname='dataListTagsOf':
						  tag2.style.display=''	   :tags2.classname='dataListTagsOn'">發包安裝</span>			
  -->
  <span	id="tags1" class=dataListTagOn>專案社區用戶維修收款資訊</span>
  <!--
		   <FONT SIZE=2	COLOR=RED>
		  <MARQUEE width=250 behavior=slide	scrolldelay=30 behavior=alternate>
		  <%
		  SQLYY="SELECT	FINISHDAT FROM RTPrjCUST WHERE CUSID='" &	DSPKEY(0) &	"'"
		  RS.OPEN SQLYY,CONN
		  IF isnull(RS("FINISHDAT")) THEN
			 XXMSG="此客戶尚未完工，不可建立續約資料。"
		  ELSE
			 XXMSG=""
		  END if
		  RS.CLOSE
		  response.Write xxmsg
		  %>
		  </MARQUEE>
		  </FONT>
	-->
<div class=dataListTagOn> 
<table width="100%" ID="Table4">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td>
<td>     



<DIV ID="SRTAG2">
	<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
		<tr><td	bgcolor="BDB76B" align="center">維修收款內容</td></tr>
	</table>
</DIV>

<DIV ID=SRTAB2 >
	<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3">
		<tr><td	width="10%"	class=dataListHEAD>派單日期</td>
			<td	width="23%"	bgcolor="silver" >
				<input type="text" name="key2" value="<%=dspKey(2)%>" <%=fieldpa%> <%=fieldRole(1)%>
					  READONLY size="10" class=dataListEntry  >
				<input type="button" value="...." id="B2" name="B2" onclick="SrBtnOnClick" <%=fieldpD%>	height="100%" width="100%" style="Z-INDEX: 1">
				<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" id="C2" name="C2" onclick="SrClear" <%=fieldpD%> alt="清除" style="Z-INDEX:1"	border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
			</td>		 
			<td	class=dataListHEAD>維修員工</td>
			<td	bgcolor="silver" >
				<input type="TEXT" name="key21" value="<%=dspKey(21)%>" <%=fieldpa%> <%=fieldRole(1)%>
					readonly size="6" class="dataListENTRY" >
				<input type="BUTTON" id="B21" name="B21" <%=fieldpD%> onclick="Srsales8onclick()" value="...." width="100%" style="Z-INDEX: 1">	 
				<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" id="C21" name="C21" onclick="SrClear" <%=fieldpD%> alt="清除" style="Z-INDEX:1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
				<font size=2><%=SrGetEmployeeName(dspKey(21))%></font>
			</td>
			<td class=dataListHead>維修經銷商</td>
			<%  
				If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect <1 Then 
				sql="SELECT RTObj.CUSID,RTObj.SHORTNC " _
					&"FROM RTObj INNER JOIN " _
					&"RTConsignee ON RTObj.CUSID = RTConsignee.CUSID INNER JOIN " _
					&"RTConsigneeCASE ON RTConsigneeCASE.CUSID = RTConsignee.CUSID " _
					&"WHERE (RTConsigneeCASE.CASEID = '08') " 
				s="<option value="""" >(經銷商)</option>"
				Else
				sql="SELECT RTObj.CUSID,RTObj.SHORTNC " _
					&"FROM RTObj INNER JOIN " _
					&"RTConsignee ON RTObj.CUSID = RTConsignee.CUSID INNER JOIN " _
					&"RTConsigneeCASE ON RTConsigneeCASE.CUSID = RTConsignee.CUSID " _
					&"WHERE (RTConsigneeCASE.CASEID = '08') " _
					&"AND rtobj.cusid='" & dspkey(22) & "' "
				End If
				rs.Open sql,conn
				If rs.Eof Then s="<option value="""" >(經銷商)</option>"
					sx=""
				Do While Not rs.Eof
					If rs("CUSID")=dspkey(22) Then sx=" selected "
					s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
					rs.MoveNext
					sx=""
				Loop
				rs.Close        
			%>
			<td bgcolor="silver" >
				<select name="key22"  <%=fieldpa%> <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select1">
					<%=s%>
				</select>
			</td>
		</TR>

		<tr><td	WIDTH="10%"  class="dataListSEARCH" height="23">收款日期</td>				 
			<td WIDTH="23%" height="23" bgcolor="silver" >			   
				<input type="text" name="key25"	<%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
						value="<%=dspKey(25)%>"	READONLY size="10"	class=dataListEntry	ID="Text1">
				<input type="button" value="...." id="B25" name="B25" onclick="SrBtnOnClick" <%=fieldpD%> height="100%" width="100%" style="Z-INDEX: 1">
				<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" id="C25" name="C25" onclick="SrClear" <%=fieldpD%> alt="清除" style="Z-INDEX:1"	border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
			</TD>
			<td	WIDTH="10%"	 class="dataListHEAD" height="23">用戶購買設備</td>				  
			<td	WIDTH="23%"	height="23"	bgcolor="silver" >
				<%
					s=""
					sx=" selected "
					If (sw="E" Or (accessMode="A" And sw="") or	(sw="S"	and	formvalid=false)) And protect <1 Then  
						sql="SELECT	CODE,CODENC	FROM RTCODE	WHERE KIND='P2' " 
					If len(trim(dspkey(3)))	< 1	Then
						sx=" selected "	
						s=s	& "<option value=""""" & sx	& "></option>"	
						sx=""
					else
						s=s	& "<option value=""""" & sx	& "></option>"	
						sx=""
					end	if	   
					Else
						sql="SELECT	CODE,CODENC	FROM RTCODE	WHERE KIND='P2'	AND	CODE='"	& dspkey(3)	& "'"
					End	If
					rs.Open	sql,conn
					Do While Not rs.Eof
					If rs("CODE")=dspkey(3)	Then sx=" selected "
						s=s	&"<option value="""	&rs("CODE")	&"""" &sx &">" &rs("CODENC") &"</option>"
						rs.MoveNext
						sx=""
					Loop
					rs.Close
				%>		   
				<select	size="1" name="key3" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select2">
					<%=s%>
				</select>
			</td>
			<td	WIDTH="10%"	class="dataListSEARCH" height="23">設備費</td>			   
			<td	WIDTH="23%"	height="23"	bgcolor="silver" >
				<input type="text" size="8" name="key4" value="<%=dspKey(4)%>" <%=fieldpa%> <%=fieldRole(1)%> class="dataListENTRY noIme" ID="Text3">
			</TD>	   
		</TR>

		<tr><td	WIDTH="10%"	class="dataListSEARCH" height="23">設定費</td>			   
			<td	WIDTH="23%"	height="23"	bgcolor="silver" >
				<input type="text" size="8" name="key5" value="<%=dspKey(5)%>" <%=fieldpa%> <%=fieldRole(1)%> class="dataListENTRY noIme" ID="Text4">
			</TD>
			<td	WIDTH="10%"	class="dataListSEARCH" height="23">移機費</td>			   
			<td	WIDTH="23%"	height="23"	bgcolor="silver" colspan=3>
				<input type="text" size="8" name="key6" value="<%=dspKey(6)%>" <%=fieldpa%> <%=fieldRole(1)%> class="dataListENTRY noIme" ID="Text5">
			</TD>
		</tr>

		<tr><td  WIDTH="10%"  class="dataListHEAD" height="23">繳費方式</td>               
			<%
				s=""
				sx=" selected "
				If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect <1 Then
				sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M9' " 
				If len(trim(dspkey(7))) < 1 Then
					sx=" selected " 
					s=s & "<option value=""""" & sx & "></option>"  
					sx=""
				else
					s=s & "<option value=""""" & sx & "></option>"  
					sx=""
				end if     
				Else
					sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M9' AND CODE='" & dspkey(7) & "'"
				End If
				rs.Open sql,conn
				Do While Not rs.Eof
					If rs("CODE")=dspkey(7) Then sx=" selected "
					s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
					rs.MoveNext
					sx=""
				Loop
				rs.Close
			%>
			<td  WIDTH="23%" height="23" bgcolor="silver" >
				<select name="key7" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select3" >                                                                  
					<%=s%>
				</select>
			</td>   
			<td	WIDTH="10%"	class="dataListHEAD" height="23">信用卡種類</td>			   
			<%
				s=""
				sx=" selected "
				If (sw="E" Or (accessMode="A" And sw="") or	(sw="S"	and	formvalid=false)) And protect <1 Then
				sql="SELECT	CODE,CODENC	FROM RTCODE	WHERE KIND='M6'	" 
				If len(trim(dspkey(8)))	< 1	Then
					sx=" selected "	
					s=s	& "<option value=""""" & sx	& "></option>"	
					sx=""
				else
					s=s	& "<option value=""""" & sx	& "></option>"	
					sx=""
				end	if	   
				Else
				sql="SELECT	CODE,CODENC	FROM RTCODE	WHERE KIND='M6'	AND	CODE='"	& dspkey(8)	& "'"
				End	If
				rs.Open	sql,conn
				Do While Not rs.Eof
					If rs("CODE")=dspkey(8)	Then sx=" selected "
					s=s	&"<option value="""	&rs("CODE")	&"""" &sx &">" &rs("CODENC") &"</option>"
					rs.MoveNext
					sx=""
				Loop
				rs.Close
			%>
			<td	WIDTH="23%"	height="23"	bgcolor="silver" >
				<select	name="key8" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35">
					<%=s%>
				</select>
			</td>
			<td	WIDTH="10%"	 class="dataListHEAD" height="23">發卡銀行</td>				  
			<%
				s=""
				sx=" selected "
				If (sw="E" Or (accessMode="A" And sw="") or	(sw="S"	and	formvalid=false)) And protect <1 Then
					sql="SELECT	* FROM RTBANK WHERE	CREDITCARD='Y' ORDER BY	HEADNC " 
					If len(trim(dspkey(9)))	< 1	Then
						sx=" selected "	
						s=s	& "<option value=""""" & sx	& "></option>"	
						sx=""
					else
						s=s	& "<option value=""""" & sx	& "></option>"	
						sx=""
					end	if	   
				Else
					sql="SELECT	* FROM RTBANK WHERE	HEADNO='" &	dspkey(9) &	"'"
				End	If
				rs.Open	sql,conn
				Do While Not rs.Eof
					If rs("HEADNO")=dspkey(9) Then sx="	selected "
					s=s	&"<option value="""	&rs("HEADNO") &""""	&sx	&">" &rs("HEADNC") &"</option>"
					rs.MoveNext
					sx=""
				Loop
				rs.Close
			%>
			<td	WIDTH="23%"	 height="23" bgcolor="silver" >			 
				<select	size="1" name="key9" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35">																	 
					<%=s%>
				</select>		 
			</TD>

		<tr><td	WIDTH="10%"	  class="dataListHEAD" height="23">卡號</td>			   
			<td	WIDTH="23%"	height="23"	bgcolor="silver" >	   
				<input type="text" name="key10"	size="16" value="<%=dspKey(10)%>" <%=fieldRole(1)%><%=fieldpa%><%=dataProtect%>	class="dataListEntry noIme" maxlength="16">
			</TD>	  
			<td	  class="dataListHEAD" height="23">持卡人姓名</td>				 
			<td	height="23"	bgcolor="silver" >	   
				<input type="text" name="key11"	size="20" value="<%=dspKey(11)%>" <%=fieldRole(1)%><%=fieldpa%><%=dataProtect%>	class="dataListCht" maxlength="30">
			</TD>	  
			<td	class="dataListHEAD" height="23">信用卡有效期限</td>			   
			<td	height="23"	bgcolor="silver">
				<input type="text" name="key12"	size="2" value="<%=dspKey(12)%>" <%=fieldRole(1)%><%=fieldpa%><%=dataProtect%> class="dataListEntry noIme" maxlength="2">
				<FONT SIZE=2>月／</FONT>
				<input type="text" name="key13"	size="2" value="<%=dspKey(13)%>" <%=fieldRole(1)%><%=fieldpa%><%=dataProtect%> class="dataListEntry noIme" maxlength="2">
				<FONT SIZE=2>年<br>( 西元年後二碼，如2005則輸入05 )</FONT>
			</TD>	  
		</tr>
	</table>
</DIV>


<DIV ID="SRTAG4">
	<table border="1" width="100%" cellpadding="0" cellspacing="0" >
		<tr><td	bgcolor="BDB76B" align="center">用戶申請、異動及施工進度狀態</td></tr>
	</table>
</DIV>

<DIV ID=SRTAB4 >  
	<table border="1" width="100%" cellpadding="0" cellspacing="0" >
		<tr><td	WIDTH="10%" class="dataListHEAD" height="23">結案日期</td>									
			<td	WIDTH="23%" height="23" bgcolor="silver" colspan=5>
				<input type="text" name="key17"	size="25" READONLY value="<%=dspKey(17)%>" class="dataListdata" ID="Text57">
			</td>
		</TR>

		<tr><td	WIDTH="10%" class="dataListHEAD" height="23">應收帳款產生日</td>								  
			<td	WIDTH="23%" height="23" bgcolor="silver" >
				<input type="text" name="key14"	size="25" READONLY value="<%=dspKey(14)%>" class="dataListdata" ID="Text57">
			</td>
			<td	WIDTH="10%" class="dataListHEAD" height="23">應收帳款編號</td>									
			<td	WIDTH="23%" height="23" bgcolor="silver" >
				<input type="text" name="key15"	size="15" READONLY value="<%=dspKey(15)%>" class="dataListdata" ID="Text56" >
			</td>
			<td	WIDTH="10%"	 class="dataListHEAD" height="23">帳款產生人員</td>				  
			<td	WIDTH="23%" height="23" bgcolor="silver" >	  
				<input type="text" name="key16"	size="6" READONLY value="<%=dspKey(16)%>" class="dataListdata">
				<font size=2><%=SrGetEmployeeName(dspKey(16))%></font>
			</td>
		</TR>

		<tr><td	class="dataListHEAD" height="23">作廢日期</td>									
			<td	height="23" bgcolor="silver">
				<input type="text" readonly size="25" name="key18" value="<%=dspKey(18)%>" class="dataListdata" ID="Text41">
			</td>
			<td class="dataListHEAD"	height="23">作廢人員</td>								  
			<td height="23" bgcolor="silver"	COLSPAN=3>
				<input type="text" readonly size="7" name="key19" value="<%=dspKey(19)%>" class="dataListDATA" ID="Text43">
				<font size=2><%=SrGetEmployeeName(dspKey(19))%></font>
			</td>
		</tr>
		
		<tr><td	class="dataListHEAD" height="23">修改日期</td>								   
			<td	height="23"	bgcolor="silver">
				<input type="text" name="key23"	size="25" READONLY value="<%=dspKey(23)%>" class="dataListDATA" ID="Text9">
			</td>
			<td	class="dataListHEAD" height="23">修改人員</td>								   
			<td	height="23"	bgcolor="silver" colspan=3>
				<input type="text" name="key24"	size="7" READONLY value="<%=dspKey(24)%>" class="dataListDATA" ID="Text2">
				<font size=2><%=SrGetEmployeeName(dspKey(24))%></font>
			</td>  
		</tr>
	</table> 
</DIV>


<DIV ID="SRTAG6">
	<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
		<tr><td	bgcolor="BDB76B" align="center">備註說明</td></tr>
	</table>
</DIV>

<DIV ID="SRTAB6">
	<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
		<TR><TD	align="CENTER">
				<TEXTAREA cols="100%" name="key20" rows=8 MAXLENGTH=500	class="dataListCht" ID="Textarea1"
				value="<%=dspkey(20)%>" <%=dataprotect%> ><%=dspkey(20)%></TEXTAREA>
			</td>
		</tr>
	</table> 
</div>



</td>
<td>&nbsp;</td></tr>
</table>
<%
	conn.Close	 
	set	rs=Nothing	 
	set	conn=Nothing 
End	Sub	

' -------------------------------------------------------------------------------------------- 
Sub	SrReadExtDB()
End	Sub

' -------------------------------------------------------------------------------------------- 
Sub	SrSaveExtDB(Smode)
End	Sub

' --------------------------------------------------------------------------------------------	
%>
<!-- #include virtual="/Webap/include/checkid.inc" -->
<!-- #include virtual="/Webap/include/companyid.inc" -->
<!-- #include file="RTGetUserRight.inc"	-->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
