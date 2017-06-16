<%
  Dim fieldRole,fieldPa
  fieldRole=Split(FrGetUserRight("RTCustD",Request.ServerVariables("LOGON_USER")),";")
%>
<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/cType.inc" -->
<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/dataList.inc" -->
<%
  Dim aryKeyName,aryKeyType(200),aryKeyValue(200),numberOfField,aryKey,aryKeyNameDB(200)
  Dim dspKey(200),userDefineKey,userDefineData,extDBField,extDB(200),userDefineRead,userDefineSave
  Dim conn,rs,i,formatName,sqlList,sqlFormatDB,userdefineactivex
  Dim aryParmKey
 '90/09/03 ADD-START
 '增加EXTDBFIELD2,EXTDBFILELD3(多檔維護)
  dim extDBField2,extDB2(200),extDBField3,extDB3(200),extDBField4,extDB4(200)
  extDBField=0  
  extDBfield2=0
  extDBField3=0
  extDBField4=0

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
              sType=Right("000" &Cstr(aryKeyType(i)),3)
              If Instr(cTypeDate,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=Null
              If Instr(cTypeAuto,sType) > 0 Or (dspKey(i)=-1 And i<numberOfKey) Then
              Else
'response.write "sType=" & sType & ";I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                 'On Error Resume Next
                 
                 'if i <> 1 then rs.Fields(i).Value=dspKey(i)
				 'if i=1 then
					'Set rsc=Server.CreateObject("ADODB.Recordset")
					'rsc.open "select max(entryno) AS entryno from RTSparqVoIPCustChg where cusid='" & dspkey(0) & "' " ,conn
					'if len(rsc("entryno")) > 0 then
					'	dspkey(i)=rsc("entryno") + 1
					'else
					'	dspkey(i)=1
					'end if
                    'rsc.close
                    rs.Fields(i).Value=dspKey(i) 
				 'end if      
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
'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"              
         '     On Error Resume Next
              runpgm=Request.ServerVariables("PATH_INFO") 
'              select case ucase(runpgm)   
'                 case ucase("/webap/rtap/base/RTSparqVoIPCust/RTSparqVoIPCustChgD.asp")
'                     if i<>0 then rs.Fields(i).Value=dspKey(i)
'                 case else
                     rs.Fields(i).Value=dspKey(i)
                     'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
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
    'if accessmode ="A" then
    '   runpgm=Request.ServerVariables("PATH_INFO")
    '      rsc.open "select max(entryno) AS ENTRYNO from RTSparqVoIPCustChg where cusid='" & dspkey(1) & "' " ,conn
    '      if not rsC.eof then
    '        dspkey(1)=rsC("entryno")
    '      end if
    '      rsC.close
    'end if
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
    If  rs.Eof Then
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
<meta http-equiv=content-type content="text/html; charset=big5">
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
Sub SrDeveloperonclick()
       prog="RTGetDeveloperD.asp"
       prog=prog & "?KEY=" & document.all("KEY121").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key121").value =  trim(Fusrid(0))
       End if       
       end if
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
       sType=Right("000" & Cstr(aryKey(i)),3)
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
  title="速博KTS用戶資料異動"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT	CUSID, ENTRYNO, MODIFYCODE, MODIFYDESC, EUSR, EDAT, UUSR, UDAT, "_
			 &"OCUSNC, OSOCIALID, OBUSINESSTYPE, OCOTEL11, OCOTEL12, OCOFAX11, "_
			 &"OCOFAX12, OCOEMAIL, OCUTID1, OTOWNSHIP1, ORADDR1, ORZONE1, "_
			 &"OCUTID2, OTOWNSHIP2, ORADDR2, ORZONE2, OCUTID3, OTOWNSHIP3, "_
			 &"ORADDR3, ORZONE3, OCOBOSS, OBOSSSOCIALID, OCOCONTACTMAN, "_
			 &"OCOCONTACTTEL11, OCOCONTACTTEL12, OCOCONTACTTEL13, "_
			 &"OCOCONTACTFAX11, OCOCONTACTFAX12, OCOCONTACTMOBILE, "_
			 &"OCOCONTACTEMAIL, OAPFORMAPPLYDAT, OAPPLYDAT, OAPPLYTNSDAT, "_
			 &"OCONTRACTSTRDAT, ONCICAPPLYREPLYDAT, ONCICCUSID, ONCICOPENDAT, "_
			 &"OFINISHDAT, ODOCKETDAT, OTRANSDAT, OCANCELDAT, OCANCELUSR, "_
			 &"ODROPDAT, ONCICDROPFLAG, ORUNONCEBILLDAT, ORUNONCESALESDAT, "_
			 &"OCONSIGNEE1, OCONSIGNEE2, OEMPLY, OLISTTELDETAIL, OSERVICE0809, "_
			 &"OCBBNPULLDAT, OCBBNPULLUSR, OMEMO, ONOTATION, OONEPAY, "_
			 &"ODEVELOPERID, NCUSNC, NSOCIALID, NBUSINESSTYPE, NCOTEL11, "_
			 &"NCOTEL12, NCOFAX11, NCOFAX12, NCOEMAIL, NCUTID1, NTOWNSHIP1, "_
			 &"NRADDR1, NRZONE1, NCUTID2, NTOWNSHIP2, NRADDR2, NRZONE2, "_
			 &"NCUTID3, NTOWNSHIP3, NRADDR3, NRZONE3, NCOBOSS, NBOSSSOCIALID, "_
			 &"NCOCONTACTMAN, NCOCONTACTTEL11, NCOCONTACTTEL12, "_
			 &"NCOCONTACTTEL13, NCOCONTACTFAX11, NCOCONTACTFAX12, "_
			 &"NCOCONTACTMOBILE, NCOCONTACTEMAIL, NAPFORMAPPLYDAT, "_
			 &"NAPPLYDAT, NAPPLYTNSDAT, NCONTRACTSTRDAT, NNCICAPPLYREPLYDAT, "_
			 &"NNCICCUSID, NNCICOPENDAT, NFINISHDAT, NDOCKETDAT, NTRANSDAT, "_
			 &"NCANCELDAT, NCANCELUSR, NDROPDAT, NNCICDROPFLAG, "_
			 &"NRUNONCEBILLDAT, NRUNONCESALESDAT, NCONSIGNEE1, NCONSIGNEE2, "_
			 &"NEMPLY, NLISTTELDETAIL, NSERVICE0809, NCBBNPULLDAT, "_
			 &"NCBBNPULLUSR, NMEMO, NNOTATION, NONEPAY, NDEVELOPERID "_
			 &"FROM	KTSCustChg "_
			 &"WHERE CUSID='' "
			 
  sqlList    ="SELECT	CUSID, ENTRYNO, MODIFYCODE, MODIFYDESC, EUSR, EDAT, UUSR, UDAT, "_
			 &"OCUSNC, OSOCIALID, OBUSINESSTYPE, OCOTEL11, OCOTEL12, OCOFAX11, "_
			 &"OCOFAX12, OCOEMAIL, OCUTID1, OTOWNSHIP1, ORADDR1, ORZONE1, "_
			 &"OCUTID2, OTOWNSHIP2, ORADDR2, ORZONE2, OCUTID3, OTOWNSHIP3, "_
			 &"ORADDR3, ORZONE3, OCOBOSS, OBOSSSOCIALID, OCOCONTACTMAN, "_
			 &"OCOCONTACTTEL11, OCOCONTACTTEL12, OCOCONTACTTEL13, "_
			 &"OCOCONTACTFAX11, OCOCONTACTFAX12, OCOCONTACTMOBILE, "_
			 &"OCOCONTACTEMAIL, OAPFORMAPPLYDAT, OAPPLYDAT, OAPPLYTNSDAT, "_
			 &"OCONTRACTSTRDAT, ONCICAPPLYREPLYDAT, ONCICCUSID, ONCICOPENDAT, "_
			 &"OFINISHDAT, ODOCKETDAT, OTRANSDAT, OCANCELDAT, OCANCELUSR, "_
			 &"ODROPDAT, ONCICDROPFLAG, ORUNONCEBILLDAT, ORUNONCESALESDAT, "_
			 &"OCONSIGNEE1, OCONSIGNEE2, OEMPLY, OLISTTELDETAIL, OSERVICE0809, "_
			 &"OCBBNPULLDAT, OCBBNPULLUSR, OMEMO, ONOTATION, OONEPAY, "_
			 &"ODEVELOPERID, NCUSNC, NSOCIALID, NBUSINESSTYPE, NCOTEL11, "_
			 &"NCOTEL12, NCOFAX11, NCOFAX12, NCOEMAIL, NCUTID1, NTOWNSHIP1, "_
			 &"NRADDR1, NRZONE1, NCUTID2, NTOWNSHIP2, NRADDR2, NRZONE2, "_
			 &"NCUTID3, NTOWNSHIP3, NRADDR3, NRZONE3, NCOBOSS, NBOSSSOCIALID, "_
			 &"NCOCONTACTMAN, NCOCONTACTTEL11, NCOCONTACTTEL12, "_
			 &"NCOCONTACTTEL13, NCOCONTACTFAX11, NCOCONTACTFAX12, "_
			 &"NCOCONTACTMOBILE, NCOCONTACTEMAIL, NAPFORMAPPLYDAT, "_
			 &"NAPPLYDAT, NAPPLYTNSDAT, NCONTRACTSTRDAT, NNCICAPPLYREPLYDAT, "_
			 &"NNCICCUSID, NNCICOPENDAT, NFINISHDAT, NDOCKETDAT, NTRANSDAT, "_
			 &"NCANCELDAT, NCANCELUSR, NDROPDAT, NNCICDROPFLAG, "_
			 &"NRUNONCEBILLDAT, NRUNONCESALESDAT, NCONSIGNEE1, NCONSIGNEE2, "_
			 &"NEMPLY, NLISTTELDETAIL, NSERVICE0809, NCBBNPULLDAT, "_
			 &"NCBBNPULLUSR, NMEMO, NNOTATION, NONEPAY, NDEVELOPERID "_
			 &"FROM	KTSCustChg "_
			 &"WHERE "
  userDefineRead="Yes"      
  userDefineSave="Yes"       
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
  IF LEN(TRIM(DSPKEY(57))) = 0 THEN DSPKEY(57)=""
  IF LEN(TRIM(DSPKEY(58))) = 0 THEN DSPKEY(58)=""
  IF LEN(TRIM(DSPKEY(100))) = 0 THEN DSPKEY(100)=""
  IF LEN(TRIM(DSPKEY(114))) = 0 THEN DSPKEY(114)=""
  '檢查NCIC用戶編號之重覆性(930225 因速博同一用戶即使申請兩線仍會用同一用戶編號,故此一判斷無法正確執行
'  IF LEN(TRIM(DSPKEY(36))) > 0 THEN
'      Set connxx=Server.CreateObject("ADODB.Connection")  
'      Set rsxx=Server.CreateObject("ADODB.Recordset")
'      DSNXX="DSN=RTLIB"
'      connxx.Open DSNxx
      
	  '排除目前這筆資料本身
'      sqlXX="SELECT count(*) AS CNT FROM KTSCUST where NCICCUSID='" & trim(dspkey(36)) & "' and not ( CUSID='" & DSPKEY(0) & "')"
      
'      rsxx.Open sqlxx,connxx
'      s=""
      'Response.Write "CNT=" & RSXX("CNT")
'      If RSXX("CNT") > 0 Then
'         errflag="Y"
'         message="NCIC用戶編號已存在其它客戶資料中，不可重複輸入!"
'         formvalid=false
'      ELSE
'         ERRFLAG=""
'      End If
'      rsxx.Close
'      Set rsxx=Nothing
'      connxx.Close
'      Set connxx=Nothing    
'   end IF  
IF ERRFLAG <> "Y" THEN
  If len(trim(dspkey(95)))=0 or Not Isdate(dspkey(95)) then
       formValid=False
       message="用戶AP form申請日不可空白或格式錯誤"    
  elseif len(trim(dspkey(66)))=0 then
       formValid=False
       message="申請用戶統一編號或身分證號不可空白"   
  elseif len(trim(dspkey(67)))=0 then
       formValid=False
       message="申請用戶行業別不可空白"     
  elseif len(trim(dspkey(68)))=0 or len(trim(dspkey(69)))=0 then
       formValid=False
       message="用戶電話號碼不可空白"           
  elseif len(trim(dspkey(68))) > 0 and len(trim(dspkey(68))) < 2 then
       formValid=False
       message="用戶電話號碼區碼不可少於2位"      
  elseif len(trim(dspkey(69))) > 0 and len(trim(dspkey(69))) < 6 then
       formValid=False
       message="用戶電話號碼不可少於6位"   
  elseif len(trim(dspkey(70)))=0 or len(trim(dspkey(71)))=0 then
       formValid=False
       message="用戶傳真電話不可空白"                    
  elseif len(trim(dspkey(70))) > 0 and len(trim(dspkey(70))) < 2 then
       formValid=False
       message="用戶傳真電話區碼不可少於2位"      
  elseif len(trim(dspkey(71))) > 0 and len(trim(dspkey(71))) < 6 then
       formValid=False
       message="用戶傳真電話不可少於6位"             
'  elseif len(trim(dspkey(8)))=0 then
'       formValid=False
'       message="申請用戶EMAIL不可空白"          
  elseif len(trim(dspkey(73)))=0 then
       formValid=False
       message="申請用戶公司地址(縣市)不可空白"               
  elseif dspkey(73)<>"06" and dspkey(73)<>"15" and len(trim(dspkey(74)))=0 then
       formValid=False
       message="申請用戶公司地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(75)))=0 then
       formValid=False
       message="申請用戶公司地址(住址)不可空白"          
  elseif len(trim(dspkey(76)))=0 then
       formValid=False
       message="申請用戶公司地址(郵遞區號)不可空白"      
  elseif len(trim(dspkey(77)))=0 then
       formValid=False
       message="申請用戶帳寄地址(縣市)不可空白"               
  elseif dspkey(77)<>"06" and dspkey(77)<>"15" and len(trim(dspkey(78)))=0 then
       formValid=False
       message="申請用戶帳寄地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(79)))=0 then
       formValid=False
       message="申請用戶帳寄地址(住址)不可空白"          
  elseif len(trim(dspkey(80)))=0 then
       formValid=False
       message="申請用戶帳寄地址(郵遞區號)不可空白"       
  elseif len(trim(dspkey(85)))=0 then
       formValid=False
       message="申請用戶公司負責人不可空白"    
  elseif len(trim(dspkey(86)))=0 then
       formValid=False
       message="申請用戶公司負責人身份證號不可空白"        
  elseif len(trim(dspkey(87)))=0 then
       formValid=False
       message="申請用戶公司連絡人不可空白"        
  elseif len(trim(dspkey(88)))=0 or len(trim(dspkey(89)))=0 then
       formValid=False
       message="公司連絡人電話號碼不可空白"           
  elseif len(trim(dspkey(88))) > 0 and len(trim(dspkey(88))) < 2 then
       formValid=False
       message="公司連絡人電話號碼區碼不可少於2位"      
  elseif len(trim(dspkey(89))) > 0 and len(trim(dspkey(89))) < 6 then
       formValid=False
       message="公司連絡人電話號碼不可少於6位"      
  elseif len(trim(dspkey(91))) > 0 and len(trim(dspkey(91))) < 2 then
       formValid=False
       message="公司連絡人傳真電話區碼不可少於2位"      
  elseif len(trim(dspkey(92))) > 0 and len(trim(dspkey(92))) < 6 then
       formValid=False
       message="公司連絡人傳真電話不可少於6位"      
  elseif len(trim(dspkey(93))) > 0 and len(trim(dspkey(93))) <> 10 then
       formValid=False
       message="公司連絡人行動電話長度須10位"      
  elseif len(trim(dspkey(81)))=0 then
       formValid=False
       message="申請用戶KTS安裝地址(縣市)不可空白"               
  elseif dspkey(81)<>"06" and dspkey(81)<>"15" and len(trim(dspkey(82)))=0 then
       formValid=False
       message="申請用戶KTS安裝地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(83)))=0 then
       formValid=False
       message="申請用戶KTS安裝地址(住址)不可空白"          
  elseif len(trim(dspkey(84)))=0 then
       formValid=False
       message="申請用戶KTS安裝地址(郵遞區號)不可空白"        
  ELSEIf len(trim(dspkey(98)))=0 or Not Isdate(dspkey(98)) then
       formValid=False
       message="用戶合約起算日不可空白或格式錯誤"           
  ELSEIf len(trim(dspkey(112)))=0 AND len(trim(dspkey(113)))=0 then
       formValid=False
       message="用戶開發經銷商及開發業務至少須輸入一項"         
  ELSEIf dspkey(115) <> "Y" then
       formValid=False
       message="0809動態轉接服務必須勾選"                
  end if
  IF formValid=TRUE THEN
    IF dspkey(66) <> "" then
       idno=dspkey(66)
        if UCASE(left(idno,1)) >="A" AND UCASE(left(idno,1)) <="Z" THEN
          AAA=CheckID(idno)
          SELECT CASE AAA
             CASE "True"
             case "False"
                   message="申請用戶身份證字號不合法"
                   formvalid=false 
             case "ERR-1"
                   message="申請用戶身份證號不可留空白或輸入位數錯誤"
                   formvalid=false       
             case "ERR-2"
                   message="申請用戶身份證字號的第一碼必需是合法的英文字母"
                   formvalid=false    
             case "ERR-3"
                   message="申請用戶身份證字號的第二碼必需是數字 1 或 2"
                   formvalid=false   
             case "ERR-4"
                   message="申請用戶身份證字號的後九碼必需是數字"
                   formvalid=false              
             case else
          end select  
       ELSE
          AAA=ValidBID(idno)
          if aaa = false then
              message="申請用戶統一編號不合法"
              formvalid=false   
          end if
       END IF
    END IF
  END IF
  
  'IF formValid=TRUE THEN
  '  IF dspkey(86) <> "" then
  '     idno=dspkey(86)
  '        AAA=CheckID(idno)
  '        SELECT CASE AAA
  '           CASE "True"
  '           case "False"
  '                 message="公司負責人身份證字號不合法"
  '                 formvalid=false 
  '           case "ERR-1"
  '                 message="公司負責人身份證號不可留空白或輸入位數錯誤"
  '                 formvalid=false       
  '           case "ERR-2"
  '                 message="公司負責人身份證字號的第一碼必需是合法的英文字母"
  '                 formvalid=false    
  '           case "ERR-3"
  '                 message="公司負責人身份證字號的第二碼必需是數字 1 或 2"
  '                 formvalid=false   
  '           case "ERR-4"
  '                 message="公司負責人身份證字號的後九碼必需是數字"
  '                 formvalid=false              
  '           case else
  '        end select  
  '  END IF
  'END IF  
 
END IF

'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(6)=V(0)
        dspkey(7)=datevalue(now())
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
          '當處理人員及處理廠商皆為空白時，才可清除此欄位資料
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
   Sub Srcounty10onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY73").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key74").value =  trim(Fusrid(0))
          document.all("key76").value =  trim(Fusrid(1))
       End if       
       end if
   End Sub       
   Sub Srcounty14onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY77").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key78").value =  trim(Fusrid(0))
          document.all("key80").value =  trim(Fusrid(1))
       End if       
       end if
   End Sub       
   Sub Srcounty18onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY81").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key82").value =  trim(Fusrid(0))
          document.all("key84").value =  trim(Fusrid(1))
       End if       
       end if
   End Sub          
	Sub SrAddrEqual1()
		document.All("key77").value=document.All("KEY73").value
		document.All("key78").value=document.All("key74").value
		document.All("key79").value=document.All("KEY75").value
		document.All("key80").value=document.All("key76").value
	End Sub 
	Sub SrAddrEqual2()
		document.All("key81").value=document.All("KEY73").value
		document.All("key82").value=document.All("key74").value
		document.All("key83").value=document.All("KEY75").value
		document.All("key84").value=document.All("key76").value
	End Sub         
	Sub SrAddrEqual3()
		document.All("key81").value=document.All("KEY77").value
		document.All("key82").value=document.All("key78").value
		document.All("key83").value=document.All("KEY79").value
		document.All("key84").value=document.All("key80").value
	End Sub         
	Sub SrAddrEqual4()
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
       <tr><td width="15%" class=dataListHead>用戶代號</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key0"
                 <%=fieldRole(1)%> readonly size="15" value="<%=dspKey(0)%>" maxlength="15" class=dataListdata ID="Text3"></td>

			<%           
			'項次 -------------------------------------------------------
				Dim  conn,rs
				Set conn=Server.CreateObject("ADODB.Connection")
				Set rs=Server.CreateObject("ADODB.Recordset")    
				conn.open DSN	
				rs.open "select max(entryno) AS entryno from KTSCustChg where cusid='" & dspkey(0) & "'",conn
				if len(rs("entryno")) > 0 then
					dspkey(1)=rs("entryno") + 1
				else
					dspkey(1)=1
				end if
				rs.close
				conn.close
				set rs=nothing
				set	conn=nothing
			%>
           <td width="15%" class=dataListHead>項次</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key1"
                 <%=fieldRole(1)%> readonly size="4" value="<%=dspKey(1)%>" maxlength="4" class=dataListdata ID="Text37"></td>                 
  </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
	Dim  conn,rs,s,sx,t,sql,rsc
	Set conn=Server.CreateObject("ADODB.Connection")
	Set rs=Server.CreateObject("ADODB.Recordset")    
	conn.open DSN	
	
    logonid=session("userid")
    if dspmode="新增" then
		'建檔人員 & 建檔日期----------------------------------------    
        if len(trim(dspkey(4))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
           V=split(rtnvalue,";")  
           dspkey(4)=V(0)
        End if  
        dspkey(5)=datevalue(now())
        ' 所有欄位先帶入原始預設資料 --------------------------------    
		sql="SELECT CUSID, CUSNC, SOCIALID, BUSINESSTYPE, COTEL11, COTEL12, COFAX11, "_
		   &"COFAX12, COEMAIL, CUTID1, TOWNSHIP1, RADDR1, RZONE1, CUTID2, "_
		   &"TOWNSHIP2, RADDR2, RZONE2, CUTID3, TOWNSHIP3, RADDR3, RZONE3, "_
		   &"COBOSS, BOSSSOCIALID, COCONTACTMAN, COCONTACTTEL11, "_
		   &"COCONTACTTEL12, COCONTACTTEL13, COCONTACTFAX11, COCONTACTFAX12, "_
		   &"COCONTACTMOBILE, COCONTACTEMAIL, APFORMAPPLYDAT, APPLYDAT, "_
		   &"APPLYTNSDAT, CONTRACTSTRDAT, NCICAPPLYREPLYDAT, NCICCUSID, "_
		   &"NCICOPENDAT, FINISHDAT, DOCKETDAT, TRANSDAT, CANCELDAT, "_
		   &"CANCELUSR, DROPDAT, NCICDROPFLAG, RUNONCEBILLDAT, "_
		   &"RUNONCESALESDAT, CONSIGNEE1, CONSIGNEE2, EMPLY, "_
		   &"LISTTELDETAIL, SERVICE0809, CBBNPULLDAT, CBBNPULLUSR, "_
		   &"MEMO, NOTATION, ONEPAY, DEVELOPERID "_
		   &"FROM KTSCUST "_
		   &"WHERE cusid ='" &dspKey(0) &"' "
		rs.Open sql, conn
	    For i = 1 To rs.Fields.Count-1
			  dspkey(i+7)=rs.Fields(i).Value
			  dspkey(i+64)=rs.Fields(i).Value			  
		Next
		rs.Close
    else
        if len(trim(dspkey(6))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(6)=V(0)
        End if         
        dspkey(7)=datevalue(now())
    end if
         
' -------------------------------------------------------------------------------------------- 
    '異動前資料PROTECT
    fieldLock=" class=""dataListData"" readonly "
    '完工後基本資料PROTECT
    'If len(trim(dspKey(46))) > 0 Then
    '   fieldPa=" class=""dataListData"" readonly "
    '   fieldpb=" disabled "
    'Else
    '   fieldPa=""
    '   fieldpb=""
    'End If
    '報竣日輸入後，寬頻服務+代理人+績效資料PROTECT
    'If len(trim(dspKey(47))) > 0 Then
    '   fieldPC=" class=""dataListData"" readonly "
    '   fieldpD=" disabled "
    'Else
    '   fieldPC=""
    '   fieldpD=""
    'End If
    '報竣轉檔後，報竣日期PROTECT
    'If len(trim(dspKey(48))) > 0 Then
    '   fieldPe=" class=""dataListData"" readonly "
    '   fieldpf=" disabled "
    'Else
    '   fieldPe=""
    '   fieldpf=""
    'End If    
%>
  
<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='M7' " 
       If len(trim(dspkey(2))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='M7' AND CODE='" & dspkey(2) &"' " 
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(異動類別)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(2) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table11">
	<tr><td width="15%" class="dataListHEAD" height="23">異動類別</td>
		<td width="85%" bgcolor="silver" colspan="3">
		<select size="1" name="key2"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry"><%=s%></select></td>
		
	<tr><td width="15%" class="dataListHEAD" height="23">異動說明</td>
		<td width="85%" bgcolor="silver" colspan="3">
        <input type="text" name="key3" value="<%=dspKey(3)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> maxlength="255" size="100" class=dataListENTRY ID="Text4"></td></tr>

<%  
	name="" 
	if dspkey(4) <> "" then
		sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			&"where rtemployee.emply='" & dspkey(4) & "' "
		rs.Open sql,conn
		if rs.eof then
			name=""
		else
			name=rs("cusnc")
		end if
		rs.close
	end if
%>    
	<tr><td class="dataListHEAD" height="23">建檔人員</td>                                 
        <td height="23" bgcolor="silver">
		<input type="text" name="key4" size="6" READONLY value="<%=dspKey(4)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text5"><font size=2><%=name%></font></td>
		
        <td class="dataListHEAD" height="23">建檔日期</td>                                 
        <td height="23" bgcolor="silver">
        <input type="text" name="key5" size="10" READONLY value="<%=dspKey(5)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text6"></td></tr>  

<%
	name="" 
	if dspkey(6) <> "" then
		sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			&"where rtemployee.emply='" & dspkey(6) & "' "
		rs.Open sql,conn
		if rs.eof then
			name=""
		else
			name=rs("cusnc")
		end if
		rs.close
	end if
%>
	<tr><td class="dataListHEAD" height="23">修改人員</td>
        <td height="23" bgcolor="silver">
		<input type="text" name="key6" size="6" READONLY value="<%=dspKey(6)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text7"><font size=2><%=name%></font></td>

        <td class="dataListHEAD" height="23">修改日期</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key7" size="10" READONLY value="<%=dspKey(7)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text8"></td></tr>
	</table><br>


  <span id="tags1" class="dataListTagsOn"
        onClick="vbscript:tag1.style.display=''	   :tags1.classname='dataListTagsOn':
						  tag2.style.display='none':tags2.classname='dataListTagsOf'"><u>異動後資料</u> |</span>
  <span id="tags2" class="dataListTagsOf"
        onClick="vbscript:tag1.style.display='none':tags1.classname='dataListTagsOf':
                          tag2.style.display=''    :tags2.classname='dataListTagsOn'"><u>異動前資料</u></span>

<!-- 異動後資料 -->
<table width="100%" ID="Table12" bgcolor="silver"><tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
		<tr><td>&nbsp;</td><td>

<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
	<tr><td bgcolor="BDB76B" align="LEFT" colspan="6">基本資料</td></tr>

	<tr><td WIDTH=10%  class="dataListsearch" height="23">NCIC用戶編號</td>                                 
        <td WIDTH=10% height="23" bgcolor="silver">
			<input type="text" name="key100" size="15" value="<%=dspKey(100)%>" <%=fieldpC%> class="dataListENTRY" ID="Text63"></td>
		<td WIDTH=10%  class="dataListsearch" height="23">經銷商一次性佣獎拆帳</td>                                 
        <td WIDTH=10% height="23" bgcolor="silver" colspan="3">
			<input type="text" name="key120" size="15" value="<%=dspKey(120)%>" <%=fieldlock%> class="dataListENTRY" ID="Text64"></td></tr>

	<tr><td width="14%" class=dataListHEAD>用戶申請日</td>
		<td width="20%" bgcolor="silver" >
			<input type="text" name="key95" <%=fieldpa%><%=fieldRole(1)%>
               style="text-align:left;" maxlength="10" value="<%=dspKey(95)%>" size="10" class="dataListEntry">
			<input  type="button" id="B95"  <%=fieldpb%> name="B95" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C95"  name="C95"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>      
		<td width="14%" class=dataListSEARCH>CBBN拉單日</td>       
		<td width="20%" bgcolor="silver">
			<input type="text" name="key116" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(116)%>"  READONLY size="10" class=dataListDATA ID="Text78"></td>
               
		<%  
			name="" 
           if dspkey(117) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(117) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
		%>  
		<td width="14%" class=dataListSEARCH>CBBN拉單人員</td>       
		<td width="20%" bgcolor="silver">
			<input type="text" name="key117" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="6"
               value="<%=dspKey(117)%>"  READONLY size="6" class=dataListDATA ID="Text79"><font size=2><%=name%></font></td></tr>
               
	<tr><td width="10%" class=dataListHEAD>用戶(公司)名稱</td>
		<td  width="20%"  bgcolor="silver" >
			<input type="text" name="key65" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="40"
               value="<%=dspKey(65)%>"  size="40" class=dataListENTRY ID="Text22"></td>
		<td width="10%" class=dataListHEAD>統編</td>
		<td width="21%" bgcolor="silver" >
			<input type="text" name="key66" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10" value="<%=dspKey(66)%>" size="12" class=dataListENTRY ID="Text80"></td>
               
	<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='J8'  ORDER BY CODENC" 
       If len(trim(dspkey(67))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='J8' AND CODE='" & dspkey(67) & "' ORDER BY CODENC"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(67) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
    %>         
		<td width="10%" class=dataListHEAD>行業別</td>
		<td width="20%" bgcolor="silver" >
		   <select size="1" name="key67" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select15">                                                                  
			<%=s%></select></td></tr>
			
	<tr><td  class=dataListHEAD>用戶電話</td>
		<td  bgcolor="silver" >
			<input type="text" name="key68" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="4"
               value="<%=dspKey(68)%>"  size="4" class=dataListENTRY ID="Text81"><font size=2>-</font>
               <input type="text" name="key69" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="8"
               value="<%=dspKey(69)%>"  size="8" class=dataListENTRY ID="Text82"></td>
		<td  class=dataListHEAD>用戶傳真</td>
		<td  bgcolor="silver" >
			<input type="text" name="key70" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="4"
               value="<%=dspKey(70)%>"   size="4" class=dataListENTRY ID="Text83"><font size=2>-</font>
			<input type="text" name="key71" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="8"
               value="<%=dspKey(71)%>"  size="8" class=dataListENTRY ID="Text84"></td>      
		<td  class=dataListHEAD>用戶E-Mail</td>
		<td bgcolor="silver" >
			<input type="text" name="key72" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="30"
               value="<%=dspKey(72)%>"   size="30" class=dataListENTRY ID="Text85"></td></tr>

  <% 
	s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(73))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX10=" onclick=""Srcounty10onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(73) & "' " 
       SXX10=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(73) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
	<tr><td class=dataListHEAD>公司地址</td>
		<td bgcolor="silver" COLSPAN=5>
			<select size="1" name="key73" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select16" ><%=s%></select>
			<input type="text" name="key74" readonly  size="8" value="<%=dspkey(74)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text86"><font SIZE=2>(鄉鎮)</font>
			<input type="button" id="B74" <%=fieldpb%> name="B74"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX10%>  >        
				<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C74"  name="C74"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
			<input type="text" name="key76"  readonly size="5" value="<%=dspkey(76)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text87">
			<input type="text" name="key75" <%=fieldpa%> size="60" value="<%=dspkey(75)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text88"></td></tr>  

  <%
	s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(77))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX14=" onclick=""Srcounty14onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(77) & "' " 
       SXX14=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(77) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
	<tr><td class=dataListHEAD>帳單地址<br><input type="radio" name="rd1"  <%=fieldpb%> onClick="SrAddrEqual1()" ID="Radio3" VALUE="Radio1">同裝機</td>
		<td bgcolor="silver" COLSPAN=5>
			<select size="1" name="key77" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select17" ><%=s%></select>
			<input type="text" name="key78" readonly  size="8" value="<%=dspkey(78)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text89"><font SIZE=2>(鄉鎮)</font>
			<input type="button" id="B78" name="B78"  <%=fieldpb%> width="100%" style="Z-INDEX: 1"  value="...." <%=SXX14%>  >        
				<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C78"  name="C78"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
			<input type="text" name="key80"  readonly size="5" value="<%=dspkey(80)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text90">
			<input type="text" name="key79" <%=fieldpa%> size="60" value="<%=dspkey(79)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text91"></tr>

	<tr><td  class="dataListHEAD" height="23">企業負責人</td>                                 
        <td  height="23" bgcolor="silver">
			<input type="text" name="key85" size="12" maxlength="12" value="<%=dspKey(85)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text92"></td>
        <td  class="dataListHEAD" height="23">身分證字號</td>                                 
        <td  height="23" bgcolor="silver" colspan=3>
			<input type="text" name="key86" size="10" maxlength="10" value="<%=dspKey(86)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text93"></td></tr>
			
	<TR><td  class="dataListHEAD" height="23">企業連絡人</td>                                 
		<td  height="23" bgcolor="silver">
	        <input type="text" name="key87" size="15" maxlength="15" value="<%=dspKey(87)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text94"></td>                                 
		<td  class="dataListHEAD" height="23">連絡人電話</td>                                 
        <td  height="23" bgcolor="silver" >
			<input type="text" name="key88" size="4" maxlength="4" value="<%=dspKey(88)%>"  <%=fieldpa%><%=fieldRole(1)%>  class="dataListentry" ID="Text95"><font size=3>-</font>
			<input type="text" name="key89" size="8" maxlength="8" value="<%=dspKey(89)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry"><font size=2>分機</font>
			<input type="text" name="key90" size="5" maxlength="5" value="<%=dspKey(90)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text97"></td>
		<td  class="dataListHEAD" height="23">連絡人傳真1</td>                                 
        <td  height="23" bgcolor="silver">
			<input type="text" name="key91" size="4" maxlength="4" value="<%=dspKey(91)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text98"><font size=3>-</font>
			<input type="text" name="key92" size="8" maxlength="8" value="<%=dspKey(92)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text99"></td></tr>

	<TR><td  class="dataListHEAD" height="23">連絡人行動電話</td>                                 
        <td  height="23" bgcolor="silver">
			<input type="text" name="key93" size="10" maxlength="10" value="<%=dspKey(93)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text100"></td>                                 
        <td  class="dataListHEAD" height="23">連絡人E-MAIL</td>                                 
        <td  height="23" bgcolor="silver" COLSPAN=3>
			<input type="text" name="key94" size="30" maxlength="30" value="<%=dspKey(94)%>"  <%=fieldpa%><%=fieldRole(1)%>  class="dataListentry" ID="Text101"></td></tr>       


	<tr><td bgcolor="BDB76B" align="LEFT" colspan="6">申請服務明細</td></tr>
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(81))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX18=" onclick=""Srcounty18onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(81) & "' " 
       SXX18=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(81) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
	<tr><td WIDTH=8% class=dataListHEAD>KTS安裝地址<br>
			<input type="radio" name="rd2" <%=fieldpb%>  onClick="SrAddrEqual2()" ID="Radio4" VALUE="Radio2"><font SIZE=2>同公司</font>
			<input type="radio"  <%=fieldpb%> name="rd2" onClick="SrAddrEqual3()" ID="radio"  <%=fieldpb%>><font SIZE=2>同帳寄</font></td>
		<td WIDTH=20% bgcolor="silver" COLSPAN=5>
			<select size="1" name="key81" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select18" ><%=s%></select>
			<input type="text" name="key82" readonly  size="8" value="<%=dspkey(82)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA"><font SIZE=2>(鄉鎮)</font>
			<input type="button" id="B82" name="B82"  <%=fieldpb%> width="100%" style="Z-INDEX: 1"  value="...." <%=SXX18%>  >        
				<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C82"  name="C82"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >
			<input type="text" name="key84"  readonly size="5" value="<%=dspkey(84)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text103">
			<input type="text" name="key83" <%=fieldpa%> size="60" value="<%=dspkey(83)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text104"></tr>
	
	<tr><td width="5%" class=dataListHEAD>加值服務</td>
		<td width="20%" bgcolor="silver" colspan="3">
			<%   IF DSPKEY(114)="Y" THEN CHECK114=" CHECKED "%>
			<input type="checkbox" name="key114" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" 
               value="Y"  <%=CHECK114%> READONLY  bgcolor="silver"  ID="Checkbox1"><font size=2>於帳單中增列長途電話通話明細</font><br>
			<%   IF DSPKEY(115)="Y" THEN CHECK115=" CHECKED "%>
			<input type="checkbox" name="key115"  <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" 
               value="Y"  <%=CHECK115%>  READONLY  bgcolor="silver" ID="Checkbox2"><font size=2>0809動態轉接服務</font>
        <TD width=8% class=dataListSEARCH>合約起算日</TD>     
        <TD width=15% bgcolor="silver" colspan=3>
			<input type="text" name="key98" size="10" READONLY value="<%=dspKey(98)%>" <%=fieldpa%> <%=fieldRole(1)%> class="dataListENTRY" ID="Text105"> 
			<input type="button" id="B98"  name="B98" height="100%" width="100%" <%=fieldpB%>style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
				<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C98"  name="C98"   <%=fieldpB%>style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td></tr>
	
	
	<tr><td bgcolor="BDB76B" align="LEFT" colspan="6">績效歸屬</td></tr>
    <tr><td WIDTH="12%" class="dataListHEAD" height="23">備註</td>
        <td WIDTH="12%" height="23" bgcolor="silver">
			<input type="text" name="key119" size="8" maxlength="6" value="<%=dspKey(119)%>" <%=fieldRole(1)%> class="dataListEntry" ID="Text2"></td>
	<td id="tagT1" WIDTH="12%" class="dataListHEAD" height="23">開發經銷商</td>
        <td WIDTH="12%" height="23" bgcolor="silver">
			<input type="text" name="key112" size="8" maxlength="10" value="<%=dspKey(112)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry"></td>

	<%  
	If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 Then 
       sql="SELECT RTConsignee.CUSID, RTObj.SHORTNC FROM RTConsignee INNER JOIN  RTConsigneeCASE ON " _
          &"RTConsignee.CUSID = RTConsigneeCASE.CUSID INNER JOIN RTObj ON RTConsignee.CUSID = RTObj.CUSID " _
          &"WHERE (RTConsigneeCASE.CASEID = '05') ORDER BY  RTObj.SHORTNC"
       s="<option value="""" >(盤商)</option>"
    Else
       sql="SELECT RTConsignee.CUSID, RTObj.SHORTNC FROM RTConsignee INNER JOIN  RTConsigneeCASE ON " _
          &"RTConsignee.CUSID = RTConsigneeCASE.CUSID INNER JOIN RTObj ON RTConsignee.CUSID = RTObj.CUSID " _
          &"WHERE (RTConsigneeCASE.CASEID = '05') AND RTConsignee.CUSID='" & DSPKEY(111) & "' ORDER BY RTObj.SHORTNC "
       
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(盤商)</option>"
    sx=""
    s=""
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(111) Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>    
		<td WIDTH=10% class="dataListHEAD" height="23">盤商</td>
        <td WIDTH=15% height="23" bgcolor="silver">
           <select size="1" name="key111" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
              <%=s%>
           </select> </td></tr>

	<%  
	If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 Then 
       sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
          &"WHERE (RTEmployee.DEPT IN ('B100', 'B106', 'B107', 'B109','B200', 'B300','B401', 'B600')) AND (RTEmployee.TRAN2 <> '10') AND " _
          &"(RTEmployee.AUTHLEVEL in ('1', '2')) ORDER BY  RTObj.CUSNC "
       s="<option value="""" >(開發業務)</option>"
    Else
       sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
          &"WHERE RTEmployee.EMPLY='" & DSPKEY(113) & "' "
       s="<option value="""" >(開發業務)</option>"
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(開發業務)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("EMPLY")=dspkey(113) Then sx=" selected "
       s=s &"<option value=""" &rs("EMPLY") &"""" &sx &">" &rs("CUSNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>    
	<tr><td id="tagT1" WIDTH="10%" class="dataListHEAD" height="23">開發業務</td>               
        <td  WIDTH="20%" height="23" bgcolor="silver">
           <select size="1" name="key113" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" ID="Select20">                                            
              <%=s%>
           </select></td>

<%
	name=""
	if dspkey(121) <> "" then
		sqlxx=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			 &"where rtemployee.emply='" & dspkey(121) & "' "
		rs.Open sqlxx,conn
		if rs.eof then
			name="(對象檔找不該員工)"
		else
			name=rs("cusnc")
		end if
		rs.close
	end if
%>
		<td WIDTH="15%" class="dataListHEAD" height="23">二線開發人員</td>
		<td width="35%" colspan="3"><input type="text" name="key121"value="<%=dspKey(121)%>" <%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" size="8" maxlength="6" readonly class="dataListDATA" ID="Text107">
			<input type="BUTTON" id="B121" name="B121" style="Z-INDEX: 1"  value="...." onclick="Srdeveloperonclick()">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  alt="清除" id="C121" name="C121" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">
			<font size=2><%=name%></font></td></tr>

	<tr><td bgcolor="BDB76B" align="LEFT" colspan="6">用戶申請及施工進度狀態</td></tr>
       <tr>
        <td  width=14% class="dataListHEAD" height="23">KTS送件申請日</td>                                 
        <td  width=26% height="23" bgcolor="silver">
        <input type="text" name="key96" size="10" value="<%=dspKey(96)%>"  <%=fieldpe%><%=fieldRole(1)%> readonly class="dataListentry" ID="Text108">     
       <input type="button" id="B96"  name="B96" height="100%" width="100%" style="Z-INDEX: 1" <%=fieldpf%> value="...." onclick="SrBtnOnClick">
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C96"  name="C96"  <%=fieldpf%>  style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">     
        <td   width=10% class="dataListHEAD" height="23">KTS申請轉檔日</td>                                 
        <td   width=20% height="23" bgcolor="silver" >
        <input type="text" name="key97" size="23" value="<%=dspKey(97)%>" <%=fieldRole(1)%> readonly class="dataListDATA" ID="Text109">
        </td>  
        <td  WIDTH=10%  class="dataListHEAD" height="23">NCIC回覆日</td>                                 
        <td   WIDTH=20% height="23" bgcolor="silver">
        <input type="text" name="key99" size="10" READONLY value="<%=dspKey(99)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListdata" ID="Text110">     
        </td></tr>
        
	<tr><td  width=10% class="dataListHEAD" height="23" STYLE="DISPLAY:NONE">用戶電話開通日</td>                                 
        <td  width=20% height="23" bgcolor="silver" colspan=3 STYLE="DISPLAY:NONE">
        <input type="text" name="key101" size="10" READONLY value="<%=dspKey(101)%>" <%=fieldpE%> <%=fieldRole(1)%> class="dataListentry">
                <input type="button" id="B101" name="B101" height="100%" width="100%" <%=fieldpF%>style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C101" name="C101" <%=fieldpF%>style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></TR>
        
	<tr><td width=10% class="dataListHEAD" height="23">完工日期</td>
        <td width=20% height="23" bgcolor="silver" >
        <input type="text" name="key102" size="10" READONLY value="<%=dspKey(102)%>"  <%=fieldRole(1)%> class="dataListdata" ID="Text112">
        </td>
        <td  width=10% class="dataListHEAD" height="23" STYLE="DISPLAY:NONE">報竣日期</td>                                 
        <td  width=20%  height="23" bgcolor="silver" STYLE="DISPLAY:NONE">
        <input type="text" name="key103" size="10" READONLY value="<%=dspKey(103)%>" <%=fieldpC%> <%=fieldRole(1)%> class="dataListentry" ID="Text113">
         <input type="button" id="B103"  name="B103" height="100%" width="100%" <%=fieldpD%>style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C103"  name="C103" <%=fieldpD%>style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
        <td width=10%  class="dataListHEAD" height="23" STYLE="DISPLAY:NONE">報竣轉檔日</td>                                 
        <td width=20% height="23" bgcolor="silver" STYLE="DISPLAY:NONE">
        <input type="text" name="key104" size="23" READONLY value="<%=dspKey(104)%>"  <%=fieldRole(1)%> class="dataListDATA" ID="Text114">     
        </td>
        <td  width=10%  class="dataListHEAD" height="23">退租日</td>                                 
        <td  width=20% height="23" bgcolor="silver">
        <input type="text" name="key107" size="10" READONLY value="<%=dspKey(107)%>"  <%=fieldRole(1)%> class="dataListentry" ID="Text21">
        <input type="button" id="B107"  name="B107" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C107"  name="C107"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">     </td>
        <td  width=10%  class="dataListHEAD" height="23">NCIC強退註記</td>     
        <td  width=20% height="23" bgcolor="silver">
        <input type="text" name="key108" size="5" READONLY value="<%=dspKey(108)%>"  <%=fieldRole(1)%> class="dataListdata" ID="Text115"></td>
       </TR>
        <tr>
        <td  width=10%  class="dataListHEAD" height="23">作廢日</td>                                 
        <td  width=20% height="23" bgcolor="silver">
         <input type="text" name="key105" size="10" READONLY value="<%=dspKey(105)%>"  <%=fieldRole(1)%> class="dataListDATA" ID="Text116">
        </td>
         <td   width=10% class="dataListHEAD" height="23">作廢人員</td>                                 
        <td   width=20% height="23" bgcolor="silver" colspan=3>
        <%  name="" 
           if dspkey(106) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(106) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
		%>            
        <input type="text" name="key106" size="10" value="<%=dspKey(106)%>" <%=fieldRole(1)%> readonly class="dataListDATA" ID="Text117"><font size=2><%=name%></font>
        </td>
      </TR>

	<tr><td bgcolor="BDB76B" align="LEFT" colspan="6">拆帳/繳款資訊</td></tr>
    <TR>
        <td  WIDTH=14% class="dataListHEAD" height="23">NCIC佣獎拆帳日</td>                                 
        <td  WIDTH=26% height="23" bgcolor="silver">
        <input type="text" name="key109" size="10" READONLY value="<%=dspKey(109)%>"  <%=fieldRole(1)%> class="dataListdata" ID="Text118"></td>
        <td   WIDTH=10% class="dataListHEAD" height="23">業務獎金拆帳日</td>     
        <td  WIDTH=20% height="23" bgcolor="silver" >
        <input type="text" name="key110" size="10" READONLY value="<%=dspKey(110)%>"  <%=fieldRole(1)%> class="dataListdata" ID="Text119"></td>
        <td WIDTH=10% style="border-left:none;border-right:none">　</td><td WIDTH=20%>　</td>
        </tr>

   <tr><td bgcolor="BDB76B" align="LEFT" colspan="6">說明</td></tr>
   <TR><TD align="CENTER" colspan="5">
     <TEXTAREA  cols="100%"  name="key118" rows=8  MAXLENGTH=500  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(118)%>" ID="Textarea1"><%=dspkey(118)%></TEXTAREA>
   </td></tr>
</table>




<!-- 異動前資料 ========================================================================================================== -->
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag2" style="display: none">
	<tr><td bgcolor="BDB76B" align="LEFT" colspan="6">基本資料</td></tr>

	<tr><td WIDTH=10%  class="dataListsearch" height="23">NCIC用戶編號</td>                                 
        <td WIDTH=10% height="23" bgcolor="silver">
			<input type="text" name="key43" size="15" value="<%=dspKey(43)%>" <%=fieldlock%> class="dataListENTRY" ID="Text62"></td>
		<td WIDTH=10%  class="dataListsearch" height="23">經銷商一次性佣獎拆帳</td>                                 
        <td WIDTH=10% height="23" bgcolor="silver" colspan="3">
			<input type="text" name="key63" size="15" value="<%=dspKey(63)%>" <%=fieldlock%> class="dataListENTRY" ID="Text65"></td></tr>

	<tr><td width="14%" class=dataListHEAD>用戶申請日</td>
		<td width="20%" bgcolor="silver" >
			<input type="text" name="key38" <%=fieldlock%> style="text-align:left;" maxlength="10"
				value="<%=dspKey(38)%>"  READONLY size="10" class=dataListEntry ID="Text9"></td>
		<td width="14%" class=dataListSEARCH>CBBN拉單日</td>       
		<td width="20%" bgcolor="silver">
			<input type="text" name="key59" <%=fieldlock%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(59)%>"  READONLY size="10" class=dataListDATA ID="Text10"></td>
               
		<%  
			name="" 
           if dspkey(60) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(60) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
		%>  
		<td width="14%" class=dataListSEARCH>CBBN拉單人員</td>       
		<td width="20%" bgcolor="silver">
			<input type="text" name="key60" <%=fieldlock%>
               style="text-align:left;" maxlength="6"
               value="<%=dspKey(60)%>"  READONLY size="6" class=dataListDATA ID="Text11"><font size=2><%=name%></font></td></tr>
               
	<tr><td width="10%" class=dataListHEAD>用戶(公司)名稱</td>
		<td  width="20%"  bgcolor="silver" >
			<input type="text" name="key8" <%=fieldlock%>
               style="text-align:left;" maxlength="40"
               value="<%=dspKey(8)%>"  size="40" class=dataListENTRY ID="Text12"></td>
		<td width="10%" class=dataListHEAD>統編</td>
		<td width="21%" bgcolor="silver" >
			<input type="text" name="key9" <%=fieldlock%>
               style="text-align:left;" maxlength="10" value="<%=dspKey(9)%>" size="12" class=dataListENTRY ID="Text13"></td>
               
	<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(DSPKEY(41))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='J8'  ORDER BY CODENC" 
       If len(trim(dspkey(10))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='J8' AND CODE='" & dspkey(10) & "' ORDER BY CODENC"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(10) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
    %>         
		<td width="10%" class=dataListHEAD>行業別</td>
		<td width="20%" bgcolor="silver" >
		   <select size="1" name="key10" <%=fieldlock%> class="dataListEntry" ID="Select1">                                                                  
			<%=s%></select></td></tr>
			
	<tr><td  class=dataListHEAD>用戶電話</td>
		<td  bgcolor="silver" >
			<input type="text" name="key11" <%=fieldlock%>
               style="text-align:left;" maxlength="4"
               value="<%=dspKey(11)%>"  size="4" class=dataListENTRY ID="Text14"><font size=2>-</font>
               <input type="text" name="key12" <%=fieldlock%>
               style="text-align:left;" maxlength="8"
               value="<%=dspKey(12)%>"  size="8" class=dataListENTRY ID="Text15"></td>
		<td  class=dataListHEAD>用戶傳真</td>
		<td  bgcolor="silver" >
			<input type="text" name="key13" <%=fieldlock%>
               style="text-align:left;" maxlength="4"
               value="<%=dspKey(13)%>"   size="4" class=dataListENTRY ID="Text16"><font size=2>-</font>
			<input type="text" name="key14" <%=fieldlock%>
               style="text-align:left;" maxlength="8"
               value="<%=dspKey(14)%>"  size="8" class=dataListENTRY ID="Text23"></td>      
		<td  class=dataListHEAD>用戶E-Mail</td>
		<td bgcolor="silver" >
			<input type="text" name="key15" <%=fieldlock%>
               style="text-align:left;" maxlength="30"
               value="<%=dspKey(15)%>"   size="30" class=dataListENTRY ID="Text24"></td></tr>

  <% 
	s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND len(trim(DSPKEY(41))) = 0 Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(16))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX10=" onclick=""Srcounty10onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(16) & "' " 
       SXX10=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(16) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
	<tr><td class=dataListHEAD>公司地址</td>
		<td bgcolor="silver" COLSPAN=5>
			<select size="1" name="key16" <%=fieldlock%> size="1" class="dataListEntry" ID="Select2" ><%=s%></select>
			<input type="text" name="key17" readonly  size="8" value="<%=dspkey(17)%>" maxlength="10" readonly <%=fieldlock%> class="dataListDATA" ID="Text25"><font SIZE=2>(鄉鎮)</font>
			<input type="text" name="key19" readonly size="5" value="<%=dspkey(19)%>" maxlength="5" <%=fieldlock%> class="dataListDATA" ID="Text26">
			<input type="text" name="key18" size="60" value="<%=dspkey(18)%>" maxlength="60" <%=fieldlock%> class="dataListEntry" ID="Text27"></td></tr>  

  <%
	s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND len(trim(DSPKEY(41))) = 0 Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(20))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX14=" onclick=""Srcounty14onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(20) & "' " 
       SXX14=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(20) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
   
	<tr><td class=dataListHEAD>帳單地址</td>
		<td bgcolor="silver" COLSPAN=5>
			<select size="1" name="key20" <%=fieldlock%> size="1" class="dataListEntry" ID="Select3" ><%=s%></select>
			<input type="text" name="key21" readonly  size="8" value="<%=dspkey(21)%>" maxlength="10" readonly <%=fieldlock%> class="dataListDATA" ID="Text28"><font SIZE=2>(鄉鎮)</font>
			<input type="text" name="key23" readonly size="5" value="<%=dspkey(23)%>" maxlength="5" <%=fieldlock%> class="dataListDATA" ID="Text29">
			<input type="text" name="key22" size="60" value="<%=dspkey(22)%>" maxlength="60" <%=fieldlock%> class="dataListEntry" ID="Text30"></tr>

	<tr><td  class="dataListHEAD" height="23">企業負責人</td>                                 
        <td  height="23" bgcolor="silver">
			<input type="text" name="key28" size="12" maxlength="12" value="<%=dspKey(28)%>" <%=fieldlock%> class="dataListEntry" ID="Text31"></td>
        <td  class="dataListHEAD" height="23">身分證字號</td>                                 
        <td  height="23" bgcolor="silver" colspan=3>
			<input type="text" name="key29" size="10" maxlength="10" value="<%=dspKey(29)%>" <%=fieldlock%> class="dataListEntry" ID="Text32"></td></tr>
			
	<TR><td  class="dataListHEAD" height="23">企業連絡人</td>                                 
		<td  height="23" bgcolor="silver">
	        <input type="text" name="key30" size="15" maxlength="15" value="<%=dspKey(30)%>" <%=fieldlock%> class="dataListEntry" ID="Text33"></td>                                 
		<td  class="dataListHEAD" height="23">連絡人電話</td>                                 
        <td  height="23" bgcolor="silver" >
			<input type="text" name="key31" size="4" maxlength="4" value="<%=dspKey(31)%>" <%=fieldlock%> class="dataListentry" ID="Text34"><font size=3>-</font>
			<input type="text" name="key32" size="8" maxlength="8" value="<%=dspKey(32)%>" <%=fieldlock%> class="dataListEntry" ID="Text35"><font size=2>分機</font>
			<input type="text" name="key33" size="5" maxlength="5" value="<%=dspKey(33)%>" <%=fieldlock%> class="dataListEntry" ID="Text36"></td>
		<td  class="dataListHEAD" height="23">連絡人傳真1</td>                                 
        <td  height="23" bgcolor="silver">
			<input type="text" name="key34" size="4" maxlength="4" value="<%=dspKey(34)%>" <%=fieldlock%> class="dataListEntry" ID="Text38"><font size=3>-</font>
			<input type="text" name="key35" size="8" maxlength="8" value="<%=dspKey(35)%>" <%=fieldlock%> class="dataListEntry" ID="Text39"></td></tr>

	<TR><td  class="dataListHEAD" height="23">連絡人行動電話</td>                                 
        <td  height="23" bgcolor="silver">
			<input type="text" name="key36" size="10" maxlength="10" value="<%=dspKey(36)%>" <%=fieldlock%> class="dataListEntry" ID="Text40"></td>                                 
        <td  class="dataListHEAD" height="23">連絡人E-MAIL</td>                                 
        <td  height="23" bgcolor="silver" COLSPAN=3>
			<input type="text" name="key37" size="30" maxlength="30" value="<%=dspKey(37)%>" <%=fieldlock%> class="dataListentry" ID="Text41"></td></tr>


	<tr><td bgcolor="BDB76B" align="LEFT" colspan="6">申請服務明細</td></tr>
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND len(trim(DSPKEY(41))) = 0 Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(24))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX18=" onclick=""Srcounty18onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(24) & "' " 
       SXX18=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(24) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
	<tr><td WIDTH=8% class=dataListHEAD>KTS安裝地址</td>
		<td WIDTH=20% bgcolor="silver" COLSPAN=5>
			<select size="1" name="key24" <%=fieldlock%> size="1" class="dataListEntry" ID="Select4" ><%=s%></select>
			<input type="text" name="key25" readonly  size="8" value="<%=dspkey(25)%>" maxlength="10" readonly <%=fieldlock%> class="dataListDATA" ID="Text42"><font SIZE=2>(鄉鎮)</font>
			<input type="text" name="key27" readonly size="5" value="<%=dspkey(27)%>" maxlength="5" <%=fieldlock%> class="dataListDATA" ID="Text43">
			<input type="text" name="key26" size="60" value="<%=dspkey(26)%>" maxlength="60" <%=fieldlock%> class="dataListEntry" ID="Text44"></td></tr>
	
	<tr><td width="5%" class=dataListHEAD>加值服務</td>
		<td width="20%" bgcolor="silver" colspan="3">
			<%   IF DSPKEY(57)="Y" THEN CHECK57=" CHECKED "%>
			<input type="checkbox" name="key57" <%=fieldlock%>
               style="text-align:left;" 
               value="Y"  <%=CHECK57%> READONLY  bgcolor="silver"  ID="Checkbox3"><font size=2>於帳單中增列長途電話通話明細</font><br>
			<%   IF DSPKEY(58)="Y" THEN CHECK58=" CHECKED "%>
			<input type="checkbox" name="key58" <%=fieldlock%>
               style="text-align:left;" 
               value="Y"  <%=CHECK58%>  READONLY  bgcolor="silver" ID="Checkbox4"><font size=2>0809動態轉接服務</font>
        <TD width=8% class=dataListSEARCH>合約起算日</TD>     
        <TD width=15% bgcolor="silver" colspan=3>
			<input type="text" name="key41" size="10" READONLY value="<%=dspKey(41)%>" <%=fieldlock%> class="dataListENTRY" ID="Text45"> </td></tr>
	
	
	<tr><td bgcolor="BDB76B" align="LEFT" colspan="6">績效歸屬</td></tr>
    <tr><td WIDTH="12%" class="dataListHEAD" height="23">備註</td>
        <td WIDTH="12%" height="23" bgcolor="silver">
			<input type="text" name="key62" size="8" maxlength="6" value="<%=dspKey(62)%>" <%=fieldlock%> class="dataListEntry" ID="Text46"></td>
		<td id="tagT1" WIDTH="12%" class="dataListHEAD" height="23">開發經銷商</td>
        <td WIDTH="12%" height="23" bgcolor="silver">
			<input type="text" name="key55" size="8" maxlength="10" value="<%=dspKey(55)%>" <%=fieldlock%> class="dataListEntry" ID="Text47"></td>

	<%  
	If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(DSPKEY(45))) = 0 AND len(trim(DSPKEY(39))) = 0 Then 
       sql="SELECT RTConsignee.CUSID, RTObj.SHORTNC FROM RTConsignee INNER JOIN  RTConsigneeCASE ON " _
          &"RTConsignee.CUSID = RTConsigneeCASE.CUSID INNER JOIN RTObj ON RTConsignee.CUSID = RTObj.CUSID " _
          &"WHERE (RTConsigneeCASE.CASEID = '05') ORDER BY  RTObj.SHORTNC"
       s="<option value="""" >(盤商)</option>"
    Else
       sql="SELECT RTConsignee.CUSID, RTObj.SHORTNC FROM RTConsignee INNER JOIN  RTConsigneeCASE ON " _
          &"RTConsignee.CUSID = RTConsigneeCASE.CUSID INNER JOIN RTObj ON RTConsignee.CUSID = RTObj.CUSID " _
          &"WHERE (RTConsigneeCASE.CASEID = '05') AND RTConsignee.CUSID='" & DSPKEY(54) & "' ORDER BY RTObj.SHORTNC "
       
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(盤商)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(54) Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>    
		<td WIDTH=10% class="dataListHEAD" height="23">盤商</td>                                 
        <td WIDTH=15% height="23" bgcolor="silver">
           <select size="1" name="key54" <%=fieldlock%> class="dataListEntry" ID="Select5">                                            
              <%=s%>
           </select> </td></tr>

	<%  
	If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(DSPKEY(45))) = 0 AND len(trim(DSPKEY(39))) = 0 Then 
       sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
          &"WHERE (RTEmployee.DEPT IN ('B100', 'B106', 'B107', 'B200', 'B300', 'B401', 'B600')) AND (RTEmployee.TRAN2 <> '10') AND " _
          &"(RTEmployee.AUTHLEVEL = '2') ORDER BY  RTObj.CUSNC "
       s="<option value="""" >(開發業務)</option>"
    Else
       sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
          &"WHERE RTEmployee.EMPLY='" & DSPKEY(56) & "' "
       s="<option value="""" >(開發業務)</option>"
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(開發業務)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("EMPLY")=dspkey(56) Then sx=" selected "
       s=s &"<option value=""" &rs("EMPLY") &"""" &sx &">" &rs("CUSNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>    
	<tr><td id="tagT1" WIDTH="10%" class="dataListHEAD" height="23">開發業務</td>               
        <td  WIDTH="20%" height="23" bgcolor="silver">
           <select size="1" name="key56" <%=fieldlock%> class="dataListEntry" ID="Select6">                                            
              <%=s%>
           </select></td>

<%
	name=""
	if dspkey(64) <> "" then
		sqlxx=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			 &"where rtemployee.emply='" & dspkey(64) & "' "
		rs.Open sqlxx,conn
		if rs.eof then
			name="(對象檔找不該員工)"
		else
			name=rs("cusnc")
		end if
		rs.close
	end if
%>
		<td WIDTH="15%" class="dataListHEAD" height="23">二線開發人員</td>
		<td width="35%" colspan="3"><input type="text" name="key64" value="<%=dspKey(64)%>" <%=fieldlock%> style="text-align:left;" size="8" maxlength="6" readonly class="dataListDATA" ID="Text48">
			<font size=2><%=name%></font></td></tr>


	<tr><td bgcolor="BDB76B" align="LEFT" colspan="6">用戶申請及施工進度狀態</td></tr>
       <tr>
        <td width=14% class="dataListHEAD" height="23">KTS送件申請日</td>                                 
        <td width=26% height="23" bgcolor="silver">
			<input type="text" name="key39" size="10" value="<%=dspKey(39)%>" <%=fieldlock%> readonly class="dataListentry" ID="Text49"></td>
        <td width=10% class="dataListHEAD" height="23">KTS申請轉檔日</td>                                 
        <td width=20% height="23" bgcolor="silver" >
			<input type="text" name="key40" size="23" value="<%=dspKey(40)%>" <%=fieldlock%> readonly class="dataListDATA" ID="Text50"></td>  
        <td WIDTH=10%  class="dataListHEAD" height="23">NCIC回覆日</td>                                 
        <td WIDTH=20% height="23" bgcolor="silver">
        <input type="text" name="key42" size="10" READONLY value="<%=dspKey(42)%>" <%=fieldlock%> class="dataListdata" ID="Text51">     
        </td></tr>
        
	<tr><td width=10% class="dataListHEAD" height="23" STYLE="DISPLAY:NONE">用戶電話開通日</td>                                 
        <td width=20% height="23" bgcolor="silver" colspan=3 STYLE="DISPLAY:NONE">
        <input type="text" name="key44" size="10" READONLY value="<%=dspKey(44)%>" <%=fieldlock%> class="dataListentry" ID="Text52"></td></TR>

	<tr><td width=10% class="dataListHEAD" height="23">完工日期</td>
        <td width=20% height="23" bgcolor="silver" >
			<input type="text" name="key45" size="10" READONLY value="<%=dspKey(45)%>" <%=fieldlock%> class="dataListdata" ID="Text53"></td>
        <td width=10% class="dataListHEAD" height="23" STYLE="DISPLAY:NONE">報竣日期</td>                                 
        <td width=20%  height="23" bgcolor="silver" STYLE="DISPLAY:NONE">
			<input type="text" name="key46" size="10" READONLY value="<%=dspKey(46)%>" <%=fieldlock%> class="dataListentry" ID="Text54">

        <td width=10%  class="dataListHEAD" height="23" STYLE="DISPLAY:NONE">報竣轉檔日</td>                                 
        <td width=20% height="23" bgcolor="silver" STYLE="DISPLAY:NONE">
			<input type="text" name="key47" size="23" READONLY value="<%=dspKey(47)%>" <%=fieldlock%> class="dataListDATA" ID="Text55"></td>
        <td width=10%  class="dataListHEAD" height="23">退租日</td>                                 
        <td width=20% height="23" bgcolor="silver">
			<input type="text" name="key50" size="10" READONLY value="<%=dspKey(50)%>" <%=fieldlock%> class="dataListentry" ID="Text56"></td>
        <td width=10%  class="dataListHEAD" height="23">NCIC強退註記</td>     
        <td width=20% height="23" bgcolor="silver">
        <input type="text" name="key51" size="5" READONLY value="<%=dspKey(51)%>" <%=fieldlock%> class="dataListdata" ID="Text57"></td></TR>
		
	<tr><td width=10%  class="dataListHEAD" height="23">作廢日</td>                                 
        <td width=20% height="23" bgcolor="silver">
			<input type="text" name="key48" size="10" READONLY value="<%=dspKey(48)%>" <%=fieldlock%> class="dataListDATA" ID="Text58"></td>
		<td width=10% class="dataListHEAD" height="23">作廢人員</td>                                 
        <td width=20% height="23" bgcolor="silver" colspan=3>
        <%  name="" 
           if dspkey(49) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(49) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
		%>            
        <input type="text" name="key49" size="10" value="<%=dspKey(49)%>" <%=fieldlock%> readonly class="dataListDATA" ID="Text59"><font size=2><%=name%></font>
        </td></TR>

	<tr><td bgcolor="BDB76B" align="LEFT" colspan="6">拆帳/繳款資訊</td></tr>
    <TR>
        <td  WIDTH=14% class="dataListHEAD" height="23">NCIC佣獎拆帳日</td>                                 
        <td  WIDTH=26% height="23" bgcolor="silver">
			<input type="text" name="key52" size="10" READONLY value="<%=dspKey(52)%>" <%=fieldlock%> class="dataListdata" ID="Text60"></td>
        <td   WIDTH=10% class="dataListHEAD" height="23">業務獎金拆帳日</td>     
        <td  WIDTH=20% height="23" bgcolor="silver" >
        <input type="text" name="key53" size="10" READONLY value="<%=dspKey(53)%>" <%=fieldlock%> class="dataListdata" ID="Text61"></td>
        <td WIDTH=10% style="border-left:none;border-right:none">　</td><td WIDTH=20%>　</td>
        </tr>

   <tr><td bgcolor="BDB76B" align="LEFT" colspan="6">說明</td></tr>
   <TR><TD align="CENTER" colspan="5">
     <TEXTAREA  cols="100%"  name="key61" rows=8  MAXLENGTH=500  class="dataListentry" <%=fieldlock%> value="<%=dspkey(61)%>" ID="Textarea2"><%=dspkey(61)%></TEXTAREA>
   </td></tr>
</table>

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
    logonid=session("userid")
    Call SrGetEmployeeRef(Rtnvalue,1,logonid)
    V=split(rtnvalue,";")  
    Dim conn,rs
    Set conn=Server.CreateObject("ADODB.Connection")
	sql="UPDATE	KTSCUST "_
	   &"SET CUSNC=NCUSNC, SOCIALID=NSOCIALID, BUSINESSTYPE=NBUSINESSTYPE, COTEL11=NCOTEL11, COTEL12=NCOTEL12, COFAX11=NCOFAX11, COFAX12=NCOFAX12, "_
	   &"COEMAIL=NCOEMAIL, CUTID1=NCUTID1, TOWNSHIP1=NTOWNSHIP1, RADDR1=NRADDR1, RZONE1=NRZONE1, CUTID2=NCUTID2, TOWNSHIP2=NTOWNSHIP2, "_
	   &"RADDR2=NRADDR2, RZONE2=NRZONE2, CUTID3=NCUTID3, TOWNSHIP3=NTOWNSHIP3, RADDR3=NRADDR3, RZONE3=NRZONE3, COBOSS=NCOBOSS, "_
	   &"BOSSSOCIALID=NBOSSSOCIALID, COCONTACTMAN=NCOCONTACTMAN, COCONTACTTEL11=NCOCONTACTTEL11, COCONTACTTEL12=NCOCONTACTTEL12, "_
	   &"COCONTACTTEL13=NCOCONTACTTEL13, COCONTACTFAX11=NCOCONTACTFAX11, COCONTACTFAX12=NCOCONTACTFAX12, "_
	   &"COCONTACTMOBILE=NCOCONTACTMOBILE, COCONTACTEMAIL=NCOCONTACTEMAIL, APFORMAPPLYDAT=NAPFORMAPPLYDAT, APPLYDAT=NAPPLYDAT, "_
	   &"APPLYTNSDAT=NAPPLYTNSDAT, CONTRACTSTRDAT=NCONTRACTSTRDAT, NCICAPPLYREPLYDAT=NNCICAPPLYREPLYDAT, NCICCUSID=NNCICCUSID, "_
	   &"NCICOPENDAT=NNCICOPENDAT, FINISHDAT=NFINISHDAT, DOCKETDAT=NDOCKETDAT, TRANSDAT=NTRANSDAT, CANCELDAT=NCANCELDAT, "_
	   &"CANCELUSR=NCANCELUSR, DROPDAT=NDROPDAT, NCICDROPFLAG=NNCICDROPFLAG, RUNONCEBILLDAT=NRUNONCEBILLDAT, "_
	   &"RUNONCESALESDAT=NRUNONCESALESDAT, CONSIGNEE1=NCONSIGNEE1, CONSIGNEE2=NCONSIGNEE2, EMPLY=NEMPLY, LISTTELDETAIL=NLISTTELDETAIL, "_
	   &"SERVICE0809=NSERVICE0809, CBBNPULLDAT=NCBBNPULLDAT, CBBNPULLUSR=NCBBNPULLUSR, MEMO=NMEMO, NOTATION=NNOTATION, ONEPAY=NONEPAY, "_
	   &"DEVELOPERID=NDEVELOPERID, UUSR= '" &V(0)& "',UDAT ='" &datevalue(now())& "' "_
	   &"from	KTSCUST a "_
	   &"inner join KTSCUSTChg b on a.CUSID =b.CUSID "_
	   &"WHERE	b.cusid='" & dspkey(0) & "' and b.entryno=" &dspkey(1)
    conn.open DSN    
    conn.execute(sql)
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
End Sub
' -------------------------------------------------------------------------------------------- 
' --------------------------------------------------------------------------------------------  
%>
<!-- #include virtual="/Webap/include/checkid.inc" -->
<!-- #include virtual="/Webap/include/companyid.inc" -->
<!-- #include file="RTGetUserRight.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->