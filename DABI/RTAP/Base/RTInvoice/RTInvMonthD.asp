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
    For i = 0 To numberOfField-1
        sType=Right("000" &Cstr(aryKeyType(i)),3) 
        If Instr(cTypeChar,sType) > 0 Then
           dspKey(i)=""
        ElseIf Instr(cTypeNumeric,sType) > 0 Then
           dspKey(i)=0
        'ElseIf Instr(cTypeDate,sType) > 0 Then
        '   dspKey(i)=Now()
        ElseIf Instr(cTypeBoolean,sType) > 0 Then
           dspKey(i)=True
        Else
           dspKey(i)=""
        End If
    Next
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
                runpgm=Request.ServerVariables("PATH_INFO") 
                select case ucase(runpgm)   
                   ' 因dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
                   case ucase("/webap/rtap/base/RTPowerBill/RTPowerBillD.asp")
                       if i <> 0 then rs.Fields(i).Value=dspKey(i)    
                       if i=0 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         cusidxx="B" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
                         sql = "select max(BILLNO) AS BILLNO from RTPowerBillH where BILLNO like '" & cusidxx & "%' "
                         rsc.open sql,conn
'response.Write "BILLNO="& rsc("BILLNO")
                         if len(rsc("BILLNO")) > 0 then
                            dspkey(0)=cusidxx & right("000" & cstr(cint(right(rsc("BILLNO"),3)) + 1),3)
                         else
                            dspkey(0)=cusidxx & "001"
                         end if
                         rsc.close
                         rs.Fields(i).Value=dspKey(i) 
                       end if      
                   case else
'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                        rs.Fields(i).Value=dspKey(i)      
                END SELECT
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
                 case ucase("/webap/rtap/base/RTPowerBill/RTPowerBillD.asp")                 
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
       if ucase(runpgm)=ucase("/webap/rtap/base/RTPowerBill/RTPowerBillD.asp") then
          cusidxx="B" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
          rsc.open "select max(BillNO) AS BillNO from RTPowerBillH where BillNO like '" & cusidxx & "%' " ,conn
          if not rsC.eof then
            dspkey(0)=rsC("BILLNO")
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
'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
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
  numberOfKey=2
  title="每月發票號碼字軌維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT	INVYEAR, INVMONTH, INVTRACK, INVNOS, INVNOE, INVNOS3, INVNOE3 " &_
			  "FROM RTInvMonth WHERE INVYEAR='' "
  sqlList    ="SELECT	INVYEAR, INVMONTH, INVTRACK, INVNOS, INVNOE, INVNOS3, INVNOE3 " &_
			  "FROM RTInvMonth WHERE "             
  userDefineRead="Yes"      
  userDefineSave="Yes"       
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    '(自動編號) 存檔時預設值給-1
    'If accessMode="A" And sw="S" Then dspKey(0)=888
	'    IF LEN(TRIM(DSPKEY(36))) = 0 THEN DSPKEY(36)=""
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
    'logonid=session("userid")
    'if dspmode="修改" then
    '    Call SrGetEmployeeRef(Rtnvalue,1,logonid)
    '            V=split(rtnvalue,";")  
    '            DSpkey(8)=V(0)
    '    dspkey(9)=datevalue(now())
    'end if        
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveXScript()
%>
   <SCRIPT Language="VBScript">

'   Sub Srcounty3onclick()
'       prog="RTGetcountyD.asp?KEY=" & document.all("KEY3").VALUE
'       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")
'       if fusr <> "" then 
'			FUsrID=Split(Fusr,";")
'			if Fusrid(3) ="Y" then
'				document.all("key4").value =  trim(Fusrid(0))
'			End if       
'      end if
'   End Sub
   
 </Script>
<%   
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveX() 
%>
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
	'在最上方顯示key值
%>
	<!--
	<table width="60%" border=1 cellPadding=0 cellSpacing=0>
		<tr><td width="10%" class=dataListHead>建檔序號</td>
			<td width="10%" bgcolor="silver">
				<input type="text" name="key0" readonly size="15" maxlength="15"
					class=dataListdata <%=fieldRole(1)%>  value="<%=dspKey(0)%>"></td>
		</tr>
	</table>
	-->
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
	Dim conn,rs,sql
	Set conn=Server.CreateObject("ADODB.Connection")
	Set rs=Server.CreateObject("ADODB.Recordset")
	conn.open DSN

	sql ="select * from rtinvmonth where convert(char(4), invyear)+right('0'+convert(varchar(2),invmonth),2) = (select max(convert(char(4), invyear)+right('0'+convert(varchar(2),invmonth),2)) from rtinvmonth) "
	rs.Open sql,conn
	If rs.Eof Then
		LastYear= year(now())
		LastMonth= month(now())
		if LastMonth mod 2 =0 then LastMonth = LastMonth -1
		LastTrack = "AA"
		LastInvNoS= "42754550"
		LastInvNoE= "42757549"
	Else
		LastYear= rs("invYear")
		LastMonth= rs("InvMonth") + 2
		if LastMonth > 12 then 
			LastMonth = 1 
			LastYear = LastYear + 1
		end if
		LastTrack = rs("InvTrack")
		LastTrack1 = asc(left(LastTrack, 1))+1
		if LastTrack1 >90 then LastTrack1 =65
		LastTrack = chr(LastTrack1) & right(LastTrack,1)
		LastInvNoS= rs("InvNoS")
		LastInvNoE= rs("InvNoE")
	End If
	rs.Close

    if dspmode="新增" then
       dspkey(0)= LastYear
	   dspkey(1)= LastMonth
	   dspkey(2)= LastTrack
	   dspkey(3)= LastInvNoS
	   dspkey(4)= LastInvNoE
	   dspkey(5)= LastInvNoS
	   dspkey(6)= LastInvNoE
    end if      

    conn.Close   
    set rs=Nothing   
    set conn=Nothing 

	' by作業流程鎖定欄位--------------------------------------------------------------------------
    '用戶送件申請後,基本資料 protect
'    If len(trim(dspKey(32))) > 0 OR len(trim(dspKey(38))) > 0 OR len(trim(dspKey(39))) > 0 OR len(trim(dspKey(40))) > 0 Then
'       fieldPa=" class=""dataListData"" readonly "
'       fieldpb=" disabled "
'    Else
'       fieldPa=""
'       fieldpb=""
'    End If
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
%>

<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">

<tr><td width="15%" class=dataListHead height="23">年份</td>
    <td width="35%" height="23" bgcolor="silver">
        <input type="text" name="key0" value="<%=dspKey(0)%>" maxlength="4" size="5" class=dataListEntry <%=dataProtect%>>
	</td>
	<td width="15%" class=dataListHead height="23">月份</td>
    <td width="35%" height="23" bgcolor="silver">
        <input type="text" name="key1" value="<%=dspKey(1)%>" maxlength="2" size="3" class=dataListEntry <%=dataProtect%>>
	</td>
</tr>

<tr><td width="15%" class=dataListHead height="23">發票字軌</td>
    <td width="35%" height="23" bgcolor="silver" colspan=3 >
        <input type="text" name="key2" value="<%=dspKey(2)%>" maxlength="2" size="3" class=dataListEntry <%=dataProtect%>>
	</td>
</tr>

<tr><td width="15%" class=dataListHead height="23">二聯發票起始號碼</td>
    <td width="35%" height="23" bgcolor="silver">
        <input type="text" name="key3" value="<%=dspKey(3)%>" maxlength="8" size="10" class=dataListEntry <%=dataProtect%>>
	</td>
	<td width="15%" class=dataListHead height="23">二聯發票結束號碼</td>
    <td width="35%" height="23" bgcolor="silver">
        <input type="text" name="key4" value="<%=dspKey(4)%>" maxlength="8" size="10" class=dataListEntry <%=dataProtect%>>
	</td>
</tr>

<tr><td width="15%" class=dataListHead height="23">三聯發票起始號碼</td>
    <td width="35%" height="23" bgcolor="silver">
        <input type="text" name="key5" value="<%=dspKey(5)%>" maxlength="8" size="10" class=dataListEntry <%=dataProtect%>>
	</td>
	<td width="15%" class=dataListHead height="23">三聯發票結束號碼</td>
    <td width="35%" height="23" bgcolor="silver">
        <input type="text" name="key6" value="<%=dspKey(6)%>" maxlength="8" size="10" class=dataListEntry <%=dataProtect%>>
	</td>
</tr>


</table>
<%
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
