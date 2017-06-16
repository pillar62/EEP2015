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
  'dim extDBField2,extDB2(150),extDBField3,extDB3(150),extDBField4,extDB4(150)
  'extDBfield2=0
  'extDBField3=0
  'extDBField4=0
  'extDBField=0
  
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
'response.Write "dspKey"& i &"="& dspKey(i) & "<br>"
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
    'If extDBField > 0 Then
    '   For i = 0 To extDBField-1
    '       extDB(i)=Request.Form("ext" &i)
    '   Next
    'End If
    'If extDBField2 > 0 Then
    '   For i = 0 To extDBField2-1
    '       extDB2(i)=Request.Form("extA" &i)
    '   Next
    'End If
    'If extDBField3 > 0 Then
    '   For i = 0 To extDBField3-1
    '       extDB3(i)=Request.Form("extB" &i)
    '   Next
    'End If        
    'If extDBField4 > 0 Then
    '   For i = 0 To extDBField4-1
    '       extDB4(i)=Request.Form("extC" &i)
    '   Next
    'End If            
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
'response.Write "sType=" &sType&"<br>"
	  if dspKey(i) ="" then dspKey(i) =0
      If Instr(cTypeChar,sType) > 0 Or dspKey(i)=Null Then  
         sql=sql &"[" &aryKeyNameDB(i) &"]='" &dspKey(i) &"'"
      Else
         sql=sql &"[" &aryKeyNameDB(i) &"]=" &dspKey(i)
      End If
    Next
    GetSql=sqlList &sql 
'response.write getsql
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
              sType=Right("000" &Cstr(aryKeyType(i)),3)
              If Instr(cTypeDate,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=Null
              If Instr(cTypeAuto,sType) > 0 Or (dspKey(i)=-1 And i<numberOfKey) Then
              Else
              '   On Error Resume Next
'                runpgm=Request.ServerVariables("PATH_INFO") 
'                select case ucase(runpgm)   
                   ' 因dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
'                   case ucase("/webap/rtap/base/RTPowerBill/RTPowerBillD.asp")
                     '  response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
						if i <> 0 then rs.Fields(i).Value=dspKey(i)    
						if i=0 then
							Set rsc=Server.CreateObject("ADODB.Recordset")
				            cusidxx="R" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
							sql = "select max(resetno) AS maxresetno from RTReset where resetno like '" & cusidxx & "%' "
							rsc.open sql,conn
							if len(rsc("maxresetno")) > 0 then
								dspkey(0)=cusidxx & right("000" & cstr(cint(right(rsc("maxresetno"),3)) + 1),3)
							else
								dspkey(0)=cusidxx & "001"
							end if
							rsc.close
							rs.Fields(i).Value=dspKey(i) 
						end if      
'                   case else
'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"                         
'                        rs.Fields(i).Value=dspKey(i)     

'                END SELECT
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
'              runpgm=Request.ServerVariables("PATH_INFO") 
'              select case ucase(runpgm)   
                 ' 當程式為ADSL社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
'                 case ucase("/webap/rtap/base/RTPowerBill/RTPowerBillD.asp")                 
                  ' response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
'                     if i<>0  then rs.Fields(i).Value=dspKey(i)         
'                 case else
                     rs.Fields(i).Value=dspKey(i)
                   '  response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
'               end select
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
       if ucase(runpgm)=ucase("/webap/rtap/base/RTHBADSLCMTY/RTResetD.asp") then
          cusidxx="R" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
          rs.open "select max(resetno) AS maxresetno from RTReset where resetno like '" & cusidxx & "%' " ,conn
          if not rs.eof then
			dspkey(0)=cusidxx & right("000" & cstr(cint(right(rs("maxresetno"),3)) + 1),3)
		  else
			dspkey(0)=cusidxx & "001"
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
<input type="text" name="sw" value="<%=sw%>" style="display:none;" ID="Text17" size="20">
<input type="text" name="reNew" value="N" style="display:none;" ID="Text18" size="20">
<input type="text" name="rwCnt" value="<%=rwCnt%>" style="display:none;" ID="Text19" size="20">
<input type="text" name="accessMode" value="<%=accessMode%>" style="display:none;" ID="Text20" size="20">
<table width="100%" ID="Table1">
  <tr class=dataListTitle><td width="20%">　</td>
	<td width="60%" align=center><%=title%></td>
	<td width="20%" align=right><%=dspMode%></td>
  </tr>
</table>
<%
  s=""
  If userDefineKey="Yes" Then
     s=s &"<table width=""100%"" cellPadding=0 cellSpacing=0>" &vbCrLf _
         &"  <tr><td width=""70%"">" &vbCrLf 
     Response.Write s
'Response.Write "dspkey(0)="&dspkey(0)
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
  title="社區Reset主檔資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB=	"SELECT RESETNO, CMTYTYPE, COMQ1, LINEQ1, TEL, MEMO, UUSR, UDAT, CANCELUSR, CANCELDAT " &_
				"FROM RTReset WHERE RESETNO='' "
  sqlList=		"SELECT RESETNO, CMTYTYPE, COMQ1, LINEQ1, TEL, MEMO, UUSR, UDAT, CANCELUSR, CANCELDAT " &_
				"FROM RTReset  WHERE "
  'userDefineRead="Yes"      
  'userDefineSave="Yes"       
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    '(自動編號) 存檔時預設值給-1
'    If accessMode="A" And sw="S" Then dspKey(0)=-1
    IF LEN(TRIM(DSPKEY(3))) = 0 THEN DSPKEY(3)=0
'    IF LEN(TRIM(DSPKEY(54))) = 0 THEN DSPKEY(54)=""
'	IF ERRFLAG <> "Y" THEN
'		If len(trim(dspkey(31)))=0 or Not Isdate(dspkey(31)) then
'			formValid=False
'			message="用戶AP form申請日不可空白或格式錯誤"    
'		elseif dspkey(55) <> "Y" then
'			formValid=False
'			message="0809動態轉接服務必須勾選"                
'		end if
'	END IF 
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

   Sub Srcounty3onclick()
	   colComn = document.all("colCOMN").VALUE
	   'if colComn ="" then colComn ="*"
   
       prog="RTGetCmty.asp?KEY=" & document.all("KEY1").VALUE &";"& colComn
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")
       'FUsr=Window.open(prog,"d2","resizable=yes", true)
       if fusr <> "" then 
			FUsrID=Split(Fusr,";")
			if Fusrid(8) ="Y" then
				document.all("key2").value =  trim(Fusrid(0))
				document.all("key3").value =  trim(Fusrid(1))
				document.all("colIP1").value =  trim(Fusrid(2))
				document.all("colIP2").value =  trim(Fusrid(3))
				document.all("colLinetel").value =  trim(Fusrid(4))
				document.all("colDropdat").value =  trim(Fusrid(5))
				document.all("colCanceldat").value =  trim(Fusrid(6))
				document.all("colCOMN").value =  trim(Fusrid(7))
			End if       
       end if
   End Sub
 </Script>
<%   
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveX() %>
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"   codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 >
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<%	
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()
	'在最上方顯示key值%>
	<table width="60%" border=1 cellPadding=0 cellSpacing=0>
		<tr><td width="10%" class=dataListHead>建檔序號</td>
			<td width="10%" bgcolor="silver">
				<input type="text" name="key0" readonly size="12" value="<%=dspKey(0)%>" class=dataListdata >
			</td>
		</tr>
	</table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
	'UUSR,UDAT四欄取得
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(6))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")
                dspkey(6)=V(0)
        End if
       dspkey(7)=now()
    end if
	
	' by作業流程鎖定欄位--------------------------------------------------------------------------
    '用戶送件申請後,基本資料 protect
    If accessMode <>"A" Then
       fieldpb=" disabled "
    Else
       fieldpb=""
    End If
    '申請轉檔後，送件申請日protect
'    If len(trim(dspKey(33))) > 0 OR len(trim(dspKey(38))) > 0 OR len(trim(dspKey(39))) > 0 OR len(trim(dspKey(40))) > 0 Then
'       fieldPC=" class=""dataListData"" readonly "
'       fieldpD=" disabled "
'    Else
'       fieldPC=""
'       fieldpD=""
'    End If
        '報竣後，全部資料protect(不含作廢日)
'    If len(trim(dspKey(39))) > 0 OR len(trim(dspKey(38))) > 0  Then
'       fieldPe=" class=""dataListData"" readonly "
'       fieldpf=" disabled "
'    Else
'       fieldPe=""
'       fieldpf=""
'    End If
        '報竣轉檔後，報竣日protect(不含作廢日)
'    If len(trim(dspKey(40))) > 0 Then
'       fieldPg=" class=""dataListData"" readonly "
'       fieldph=" disabled "
'    Else
'       fieldPg=""
'       fieldph=""
'    End If    
      
    Dim conn,rs,s,sx,sql,t      
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
    %>

<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">

<tr><td width="15%" class=dataListHead height="23">社區類別</td>
	<%
		If sw="E" Or (accessMode="A" And sw="") Then
			sql="SELECT CODE, CODENC FROM RTCode WHERE KIND='L5' and code in ('03','05','06','07')"
		Else
			sql="SELECT CODE, CODENC FROM RTCode WHERE KIND='L5' and code in ('03','05','06','07') " _
				&"AND CODE='" &dspKey(1) &"' "
		End If
		rs.Open sql,conn
		's="<option value=""""></option>"
		sx=""
		Do While Not rs.Eof
			If rs("Code")=dspKey(1) Then sx=" selected "
			s=s &"<option value=""" &rs("Code") &"""" &sx &">" &rs("CodeNC") &"</option>"
			rs.MoveNext
			sx=""
		Loop
		rs.Close
	%>
    <td width="85%" height="23" bgcolor="silver" colspan="3">
      <select class=dataListEntry name="key1" <%=dataProtect%> size="1"><%=s%></select>
	</td>
</tr>


<tr><td width="15%" class=dataListHEAD>社區主線序號</td>
    <td width="85%" height="23" bgcolor="silver" colspan="3">
		<input type="text" name="key2" readonly size="4" value="<%=dspkey(2)%>" class="dataListDATA">-
		<input type="text" name="key3" readonly size="3" value="<%=dspkey(3)%>" class="dataListDATA">
		<%
			sql="select case a.cmtytype when '03' then b.comn when '05' then h.comn when '06' then i.comn when '07' then j.comn else '' end as comn, " &_
				"		case a.cmtytype when '03' then b.rcomdrop when '05' then c.dropdat when '06' then d.dropdat when '07' then e.dropdat end as dropdat, " &_
				"		case a.cmtytype when '05' then c.canceldat when '06' then d.canceldat when '07' then e.canceldat end as canceldat, " &_
				"		case a.cmtytype when '03' then b.cmtytel when '05' then c.linetel when '06' then d.linetel when '07' then e.linetel end as linetel, " &_
				"case a.cmtytype when '03' then replace(b.ipaddr, ' ', '') " &_
				"			when '05' then replace(c.gateway, ' ', '') " &_
				"			when '06' then replace(d.lineip, ' ', '') " &_
				"			when '07' then replace(e.lineip, ' ', '') else '' end as ip1, " &_
				"isnull(c.idslamip, '') as ip2 " &_
				"from RTReset a " &_
				"left outer join RTSparqAdslCmty b on a.comq1 = b.cutyid and a.cmtytype ='03' " &_
				"left outer join RTSparq499CmtyLine c inner join RTSparq499CmtyH h on h.comq1 = c.comq1 on a.comq1 = c.comq1 and a.lineq1 = c.lineq1 and a.cmtytype ='05' " &_
				"left outer join RTLessorCmtyLine d inner join RTLessorCmtyH i on i.comq1 = d.comq1 on a.comq1 = d.comq1 and a.lineq1 = d.lineq1 and a.cmtytype ='06' " &_
				"left outer join RTLessorAvsCmtyLine e inner join RTLessorAvsCmtyH j on j.comq1 = e.comq1 on a.comq1 = e.comq1 and a.lineq1 = e.lineq1 and a.cmtytype ='07' " &_
				"where a.cmtytype ='"& dspkey(1) &"' and a.COMQ1 ='"& dspkey(2) &"' and a.LINEQ1 ='"& dspkey(3) &"' "
			rs.Open sql,conn
			if not rs.eof and accessMode <>"A" then 
				comn = rs("COMN")
				dropdat = rs("dropdat")
				canceldat = rs("canceldat")
				linetel = rs("linetel")
				IP1 = rs("IP1")
				IP2 = rs("IP2")
			end if
			rs.Close
		%>
		<input type="text" size="30" style="color=brown;" ID="colCOMN" value="<%=comn%>">
		<input type="button" id="B1" name="B1" width="100%" style="Z-INDEX: 1" value="社區名稱搜尋" onclick="Srcounty3onclick()" >
		<font size=2>可輸入社區名稱再按鈕搜尋</font>
	</td>
</tr>

<tr><td width="15%" class=dataListHEAD>附掛電話</td>
	<td width="85%" height="23" bgcolor="silver" colspan="3">
		<input type="text" ID="colLinetel" name="colLinetel" readonly size="15" value="<%=linetel%>" class="dataListDATA">
	</td>
</tr>

<tr><td width="15%" class=dataListHEAD>主線IP</td>
	<td width="35%" height="23" bgcolor="silver">
		<input type="text" ID="colIP1" name="colIP1" readonly size="20" value="<%=IP1%>" class="dataListDATA" NAME="IP1">
	</td>

	<td width="15%" class=dataListHEAD>iDslam IP</td>
	<td width="35%" height="23" bgcolor="silver">
		<input type="text" ID="colIP2" name="colIP2" readonly size="20" value="<%=IP2%>" class="dataListDATA" NAME="IP2">
	</td>
</tr>

<tr><td width="15%" class=dataListHEAD>主線撤線日</td>
	<td width="35%" height="23" bgcolor="silver">
		<input type="text" ID="colDropdat" name="colDropdat" readonly size="25" value="<%=dropdat%>" class="dataListDATA">
	</td>

	<td width="15%" class=dataListHEAD>主線作廢日</td>
	<td width="35%" height="23" bgcolor="silver">
		<input type="text" ID="colCanceldat" name="colCanceldat" readonly size="25" value="<%=canceldat%>" class="dataListDATA">
	</td>
</tr>

<tr><td width="15%" class=dataListHead height="23">Reset電話</td>
    <td width="85%" height="23" bgcolor="silver" colspan="3">
        <input type="text" name="key4" value="<%=dspKey(4)%>" maxlength="10" size="12" <%=dataProtect%> class="dataListEntry noime">
	</td>
</tr>

<tr><td width="15%" class=dataListHead height="23">備註說明</td>
	<TD width="85%" align="Left" bgcolor="silver" colspan="3">
		<TEXTAREA cols="100%" name="key5" value="<%=dspkey(5)%>" <%=dataprotect%> rows=5 MAXLENGTH=300 class="dataListentry" ID="Textarea1"><%=dspkey(5)%></TEXTAREA>
	</td>
</tr>

<tr><td width="15%" class="dataListHEAD" height="23">作廢人員</td>
    <td width="35%" height="23" bgcolor="silver">
		<input type="text" name="key8" size="6" READONLY value="<%=dspKey(8)%>" class="dataListDATA">
		<font size=2><%=SrGetEmployeeName(dspKey(8))%></font>
	</td>

    <td width="15%" class="dataListHEAD" height="23">作廢日期</td>
    <td width="35%" height="23" bgcolor="silver">
		<input type="text" name="key9" size="25" READONLY value="<%=dspKey(9)%>" class="dataListDATA">
	</td>       
</tr>

<tr><td width="15%" class="dataListHEAD" height="23">最後修改人員</td>                                 
    <td height="23" bgcolor="silver">
		<input type="text" name="key6" size="6" READONLY value="<%=dspKey(6)%>" class="dataListDATA">
		<font size=2><%=SrGetEmployeeName(dspKey(6))%></font>
	</td>

    <td width="15%" class="dataListHEAD" height="23">最後修改日期</td>
    <td height="23" bgcolor="silver">
		<input type="text" name="key7" size="25" READONLY value="<%=dspKey(7)%>" class="dataListDATA">
	</td>
</tr>
</table><br><br>

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
%>
<!-- #include file="RTGetUserRight.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
