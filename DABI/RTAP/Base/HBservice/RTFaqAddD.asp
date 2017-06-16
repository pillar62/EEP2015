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
    Dim sql,i,sType, conn,rs,rsc
    sql=GetSql()
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open DSN
    Set rs=Server.CreateObject("ADODB.Recordset")
    Set rsc=Server.CreateObject("ADODB.Recordset")
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
                'runpgm=Request.ServerVariables("PATH_INFO") 
                'select case ucase(runpgm)   
                   'case ucase("/webap/rtap/base/HBservice/RTFaqAddD.asp")
                   
                       if i=1 then
		'response.write "select max(entryno) AS maxentryno from RTFaqAdd where caseno ='"& dspkey(0) &"' "
                         rsc.open "select max(entryno) AS maxentryno from RTFaqAdd where caseno ='"& dspkey(0) &"' " ,conn
                         if rsc("maxentryno") > 0 then 
                            dspKey(1) = rsc("maxentryno") + 1
                         else
                            dspKey(1) = 1
                         end if
                         rsc.close
                       end if
	  'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       rs.Fields(i).Value=dspKey(i) 
                   'case else
                   '     rs.Fields(i).Value=dspKey(i)      
                'END SELECT
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
                 'case ucase("/webap/rtap/base/HBservice/RTFaqAddD.asp")
                    'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                     rs.Fields(i).Value=dspKey(i)    
                 'case else
                 '    rs.Fields(i).Value=dspKey(i)
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
       if ucase(runpgm)=ucase("/webap/rtap/base/HBservice/RTFaqAddD.asp") then
'response.write "select max(entryno) AS maxentryno from RTFaqAdd where caseno ='"& dspkey(0) &"' "       
          rsc.open "select max(entryno) AS maxentryno from RTFaqAdd where caseno ='"& dspkey(0) &"' " ,conn          
          if len(trim(rsc("maxentryno"))) > 0 then 
            'dspkey(1) = rsc("maxentryno") + 1
          else
            'dspkey(1) = 1
          end if
          rsC.close
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
  numberOfKey=2
  title="客服追件資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB=	"SELECT CASENO, ENTRYNO, IOBOUND, MEMO, ADDUSR, ADDDAT, UUSR, UDAT, CANCELUSR, CANCELDAT " &_
				"FROM RTFaqAdd " &_
				"WHERE caseno='' "
  sqlList=		"SELECT CASENO, ENTRYNO, IOBOUND, MEMO, ADDUSR, ADDDAT, UUSR, UDAT, CANCELUSR, CANCELDAT " &_
				"FROM RTFaqAdd " &_
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
  If Not IsNumeric(dspKey(1)) Then dspKey(1)=0
  if len(trim(dspkey(2)))=0 then
       formValid=False
       message="[進出線別]不可空白"   
  end if
  '檢查客戶主檔狀態是否允許建立客服資料︰(1)已退租則不可轉派工單

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

'   Sub SrClear()
'       Dim ClickID
'       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
'       clickkey="C" & clickid
'       clearkey="key" & clickid    
'       if len(trim(document.all(clearkey).value)) <> 0 then
'          document.all(clearkey).value =  ""
         '當處理人員及處理廠商皆為空白時，才可清除此欄位資料
'       end if
'   End Sub    

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
		<tr><td width="15%" class=dataListHead>客服單號</td>
			<td width="35%"  bgcolor="silver">
				<input type="text" name="key0" readonly size="15" value="<%=dspKey(0)%>" class=dataListdata>
			</td>

			<%
				if dspmode="新增" then
					Set conn=Server.CreateObject("ADODB.Connection")
				    Set rsd=Server.CreateObject("ADODB.Recordset")
				    conn.open DSN
				    rsd.open "select max(entryno) AS maxentryno from RTFaqAdd where caseno ='"& dspkey(0) &"' " ,conn
				    if rsd("maxentryno") > 0 then 
				       dspKey(1) = rsd("maxentryno") + 1
				    else
				       dspKey(1) = 1
				    end if
				    rsd.close
				    set rsd=nothing
				    set conn=nothing
				end if
			%>
			<td width="15%" class=dataListHead>項次</td>
			<td width="35%"  bgcolor="silver">
				<input type="text" name="key1" readonly size="3" value="<%=dspKey(1)%>" class=dataListdata ID="Text44">
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
        if len(trim(dspkey(6))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
			   	dspkey(6)=V(0)
                dspkey(4)=V(0)	'追件人
        End if  
       dspkey(7)=now()
       dspkey(5)=now()			'追件時間
    else
        'if len(trim(dspkey(16))) < 1 then
        '   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
        '        V=split(rtnvalue,";")  
        '        DSpkey(16)=V(0)
        'End if         
        'dspkey(17)=now()
    end if      
' -------------------------------------------------------------------------------------------- 

    '追件單作廢後 protect
    If len(trim(dspKey(9))) > 0  Then
       fieldPa=" class=""dataListData"" readonly "
    Else
       fieldPa=""        
    end if

    Dim conn,rs,s,sx,sql,t
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
%>
  <!--
  <span id="tags1" class="dataListTagsOn"
        onClick="vbscript:tag1.style.display=''    :tags1.classname='dataListTagsOn':
                          tag2.style.display='none':tags2.classname='dataListTagsOf'">基本資料</span>
  <span id="tags2" class="dataListTagsOf"
        onClick="vbscript:tag1.style.display='none':tags1.classname='dataListTagsOf':
                          tag2.style.display=''    :tags2.classname='dataListTagsOn'">發包安裝</span>           
  -->
<span id="tags1" class="dataListTagsOn">用戶追件資訊</span>
                                                            
<div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td>
<td>     

<DIV ID=SRTAB2 >
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3">
	<tr><td width="15%" class=dataListHEAD>進出線別</td>
		<%
			s=""
			sx=" selected "
			If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 and len(trim(dspkey(9)))=0 Then  
			sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='P8' " 
			If len(trim(dspkey(2))) < 1 Then
				sx=" selected " 
				s=s & "<option value=""""" & sx & "></option>"  
				sx=""
			else
				s=s & "<option value=""""" & sx & "></option>"  
				sx=""
			end if     
			Else
			sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='P8' AND CODE='" & dspkey(2) & "'"
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
		<td width="35%" bgcolor="silver" colspan=3 >
		<select size="1" name="key2" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select1" >
			<%=s%>
		</select></td>
	</tr>

	<tr><td width="15%" class="dataListHEAD" height="23">追件人員</td>
		<td width="35%" height="23" bgcolor="silver">
			<input type="text" name="key4" size="6" READONLY value="<%=dspKey(4)%>" class="dataListDATA">
			<font size=2><%=SrGetEmployeeName(dspKey(4))%></font>
		</td>  
		<td width="15%" class="dataListHEAD" height="23">追件時間</td>
		<td width="35%" height="23" bgcolor="silver">
			<input type="text" name="key5" size="25" READONLY value="<%=dspKey(5)%>"  class="dataListDATA" ID="Text9">
		</td>       
	</tr>

	<tr><td  class="dataListHEAD" height="23">修改人員</td>
        <td  height="23" bgcolor="silver">
			<input type="text" name="key6" size="6" READONLY value="<%=dspKey(6)%>" class="dataListDATA" ID="Text10">
			<font size=2><%=SrGetEmployeeName(dspKey(6))%></font>
        </td>
        <td  class="dataListHEAD" height="23">修改時間</td>
        <td  height="23" bgcolor="silver" >
	        <input type="text" name="key7" size="25" READONLY value="<%=dspKey(7)%>" class="dataListDATA" ID="Text11">
        </td>
	</tr>

	<tr><td  class="dataListHEAD" height="23">作廢人員</td>
        <td  height="23" bgcolor="silver">
			<input type="text" name="key8" size="6" READONLY value="<%=dspKey(8)%>" style="color:red;" class="dataListDATA" ID="Text12">
			<font size=2 color=red><%=SrGetEmployeeName(dspKey(8))%></font>
        </td>
        <td  class="dataListHEAD" height="23">作廢時間</td>
        <td  height="23" bgcolor="silver">
	        <input type="text" name="key9" size="25" READONLY value="<%=dspKey(9)%>" style="color:red;" class="dataListDATA" ID="Text13">
        </td>
	</tr>

	<tr><td class="dataListHEAD">備註</td>
		<td colspan=3>
			<TEXTAREA cols="100%" name="key3" rows=10 MAXLENGTH=1024 <%=fieldpa%> class="dataListentry" <%=dataprotect%> value="<%=dspkey(3)%>" ID="Textarea2"><%=dspkey(3)%></TEXTAREA>
		</td>
	</tr>
</table>
</DIV>


<P></P>

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
