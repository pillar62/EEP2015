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
  title="速博VoIP用戶資料異動"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT CUSID, ENTRYNO, MODIFYCODE, MODIFYDESC, EUSR, EDAT, UUSR, UDAT, "_
			 &"OCUSNC, OFIRSTIDTYPE, OSOCIALID, OSECONDIDTYPE, OSECONDNO, "_
			 &"OBIRTHDAY, OEMAIL, OCONTACTTEL, OMOBILE, OFAX, OCUTID1, "_
			 &"OTOWNSHIP1, ORADDR1, ORZONE1, OCUTID2, OTOWNSHIP2, ORADDR2, "_
			 &"ORZONE2, OCUTID3, OTOWNSHIP3, ORADDR3, ORZONE3, OCOCONTACT, "_
			 &"OCOCONTACTTEL, OCOCONTACTTELEXT, OCOMOBILE, OCOFAX, OCOEMAIL, "_
			 &"OCOBOSS, OCOBOSSSOCIAL, OTRADETYPE, OAREAID, OGROUPID, OSALESID, "_
			 &"OCASETYPE, OFREECODE, OAGENTNAME, OAGENTSOCIAL, OAGENTTEL, "_
			 &"ORCVD, OAPPLYDAT, OFINISHDAT, ODOCKETDAT, ODROPDAT, OCANCELDAT, "_
			 &"OCANCELUSR, OOVERDUE, OMEMO, ONCICCUSNO, OCUSTIP1, OCUSTIP2, "_
			 &"OCUSTIP3, OCUSTIP4, OCUSTIPEND, OMACNO, OVOIPTEL, OVOIPTELSTR, "_
			 &"OVOIPTELEND, OISPTYPE, OISPETC, OCIRCUITTYPE, OCIRCUITETC, "_
			 &"OLINKTYPE, OLINKETC, OLINERATE, OLINESHARE, OLINETEL, OCREDITTYPE, "_
			 &"OCREDITBANK, OCREDITNO, OCREDITNAME, OVALIDMONTH, OVALIDYEAR, "_
			 &"OCONSIGNEE, ODEVELOPERID, OWRKNO1, OWRKNO2, OWRKRCVDAT, "_
			 &"OWRKSETDAT, OCONSIGNEESALE, NCUSNC, "_
			 &"NFIRSTIDTYPE, NSOCIALID, NSECONDIDTYPE, NSECONDNO, NBIRTHDAY, "_
			 &"NEMAIL, NCONTACTTEL, NMOBILE, NFAX, NCUTID1, NTOWNSHIP1, NRADDR1, "_
			 &"NRZONE1, NCUTID2, NTOWNSHIP2, NRADDR2, NRZONE2, NCUTID3, "_
			 &"NTOWNSHIP3, NRADDR3, NRZONE3, NCOCONTACT, NCOCONTACTTEL, "_
			 &"NCOCONTACTTELEXT, NCOMOBILE, NCOFAX, NCOEMAIL, NCOBOSS, "_
			 &"NCOBOSSSOCIAL, NTRADETYPE, NAREAID, NGROUPID, NSALESID, "_
			 &"NCASETYPE, NFREECODE, NAGENTNAME, NAGENTSOCIAL, NAGENTTEL, "_ 
			 &"NRCVD, NAPPLYDAT, NFINISHDAT, NDOCKETDAT, NDROPDAT, NCANCELDAT, "_
			 &"NCANCELUSR, NOVERDUE, NMEMO, NNCICCUSNO, NCUSTIP1, NCUSTIP2, "_
			 &"NCUSTIP3, NCUSTIP4, NCUSTIPEND, NMACNO, NVOIPTEL, NVOIPTELSTR, "_
			 &"NVOIPTELEND, NISPTYPE, NISPETC, NCIRCUITTYPE, NCIRCUITETC, "_
			 &"NLINKTYPE, NLINKETC, NLINERATE, NLINESHARE, NLINETEL, NCREDITTYPE, "_
			 &"NCREDITBANK, NCREDITNO, NCREDITNAME, NVALIDMONTH, NVALIDYEAR, "_
			 &"NCONSIGNEE, NDEVELOPERID, NWRKNO1, NWRKNO2, NWRKRCVDAT, "_
			 &"NWRKSETDAT, NCONSIGNEESALE "_
			 &"FROM RTSparqVoIPCustChg "_
			 &"WHERE CUSID='' "
			 
  sqlList    ="SELECT CUSID, ENTRYNO, MODIFYCODE, MODIFYDESC, EUSR, EDAT, UUSR, UDAT, "_
			 &"OCUSNC, OFIRSTIDTYPE, OSOCIALID, OSECONDIDTYPE, OSECONDNO, "_
			 &"OBIRTHDAY, OEMAIL, OCONTACTTEL, OMOBILE, OFAX, OCUTID1, "_
			 &"OTOWNSHIP1, ORADDR1, ORZONE1, OCUTID2, OTOWNSHIP2, ORADDR2, "_
			 &"ORZONE2, OCUTID3, OTOWNSHIP3, ORADDR3, ORZONE3, OCOCONTACT, "_
			 &"OCOCONTACTTEL, OCOCONTACTTELEXT, OCOMOBILE, OCOFAX, OCOEMAIL, "_
			 &"OCOBOSS, OCOBOSSSOCIAL, OTRADETYPE, OAREAID, OGROUPID, OSALESID, "_
			 &"OCASETYPE, OFREECODE, OAGENTNAME, OAGENTSOCIAL, OAGENTTEL, "_
			 &"ORCVD, OAPPLYDAT, OFINISHDAT, ODOCKETDAT, ODROPDAT, OCANCELDAT, "_
			 &"OCANCELUSR, OOVERDUE, OMEMO, ONCICCUSNO, OCUSTIP1, OCUSTIP2, "_
			 &"OCUSTIP3, OCUSTIP4, OCUSTIPEND, OMACNO, OVOIPTEL, OVOIPTELSTR, "_
			 &"OVOIPTELEND, OISPTYPE, OISPETC, OCIRCUITTYPE, OCIRCUITETC, "_
			 &"OLINKTYPE, OLINKETC, OLINERATE, OLINESHARE, OLINETEL, OCREDITTYPE, "_
			 &"OCREDITBANK, OCREDITNO, OCREDITNAME, OVALIDMONTH, OVALIDYEAR, "_
			 &"OCONSIGNEE, ODEVELOPERID, OWRKNO1, OWRKNO2, OWRKRCVDAT, "_
			 &"OWRKSETDAT, OCONSIGNEESALE, NCUSNC, "_
			 &"NFIRSTIDTYPE, NSOCIALID, NSECONDIDTYPE, NSECONDNO, NBIRTHDAY, "_
			 &"NEMAIL, NCONTACTTEL, NMOBILE, NFAX, NCUTID1, NTOWNSHIP1, NRADDR1, "_
			 &"NRZONE1, NCUTID2, NTOWNSHIP2, NRADDR2, NRZONE2, NCUTID3, "_
			 &"NTOWNSHIP3, NRADDR3, NRZONE3, NCOCONTACT, NCOCONTACTTEL, "_
			 &"NCOCONTACTTELEXT, NCOMOBILE, NCOFAX, NCOEMAIL, NCOBOSS, "_
			 &"NCOBOSSSOCIAL, NTRADETYPE, NAREAID, NGROUPID, NSALESID, "_
			 &"NCASETYPE, NFREECODE, NAGENTNAME, NAGENTSOCIAL, NAGENTTEL, "_ 
			 &"NRCVD, NAPPLYDAT, NFINISHDAT, NDOCKETDAT, NDROPDAT, NCANCELDAT, "_
			 &"NCANCELUSR, NOVERDUE, NMEMO, NNCICCUSNO, NCUSTIP1, NCUSTIP2, "_
			 &"NCUSTIP3, NCUSTIP4, NCUSTIPEND, NMACNO, NVOIPTEL, NVOIPTELSTR, "_
			 &"NVOIPTELEND, NISPTYPE, NISPETC, NCIRCUITTYPE, NCIRCUITETC, "_
			 &"NLINKTYPE, NLINKETC, NLINERATE, NLINESHARE, NLINETEL, NCREDITTYPE, "_
			 &"NCREDITBANK, NCREDITNO, NCREDITNAME, NVALIDMONTH, NVALIDYEAR, "_
			 &"NCONSIGNEE, NDEVELOPERID, NWRKNO1, NWRKNO2, NWRKRCVDAT, "_
			 &"NWRKSETDAT, NCONSIGNEESALE "_
			 &"FROM RTSparqVoIPCustChg "_
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
	
  if len(trim(dspkey(43)))=0 then dspkey(43)=""
  if len(trim(dspkey(73)))=0 then dspkey(73)=""
  if len(trim(dspkey(132)))<>0 then 
     Call SrGetEmployeeRef(Rtnvalue,1,logonid)
     V=split(rtnvalue, ";")  
     DSpkey(133)=V(0)
  end if   
  'if len(trim(dspkey(56)))=0 then dspkey(56)=0
  'if len(trim(dspkey(57)))=0 then dspkey(57)=0
'  IF instr(1,dspkey(67),"-",1) <> 0 THEN
'  RESPONSE.Write "AAA=" & instr(1,dspkey(67),"-",1) & "<BR>"
'  RESPONSE.Write "BBB=" & instr(1,dspkey(69),"-",1) 
'  RESPONSE.END
'  ELSE
'  RESPOSNE.WRITE "XXX"
'  RESPONSE.End
'  END IF

  '身份證檢查 -------------------------
  DSPKEY(90)=UCASE(DSPKEY(90))		'身份證第一碼大寫
  LEADINGCHAR=LEFT(DSPKEY(90),1)	'身份證欄位第一碼,用以判別是個人還是公司,若為公司則出生日期必須空白,反之則不可空白
  IF LEADINGCHAR >="0" AND LEADINGCHAR <="9" THEN
	 COMPANY="Y"     
  ELSE
     COMPANT="N"  
	 if (len(trim(dspkey(90)))=0 or (len(trim(dspkey(90)))<>10 and len(trim(dspkey(90)))<>8 ) ) AND DSPKEY(123) <> "Y" then
		formValid=False
		message="除公關機外,用戶身分證(統編)不可空白或長度不對"
	 end if
  END IF
  
  if len(trim(dspkey(88)))=0 then
       formValid=False
       message="用戶名稱不可空白"          
  elseIf len(trim(dspkey(127)))=0 or Not Isdate(dspkey(127)) then
       formValid=False
       message="收件日不可空白或格式錯誤"    
  elseif len(trim(dspkey(128)))=0 then
       formValid=False
       message="用戶申請日不可空白"   

  elseif len(trim(dspkey(98)))=0 then
       formValid=False
       message="戶籍地址(縣市)不可空白"   
  elseif len(trim(dspkey(99)))=0 and dspkey(98)<>"06" and dspkey(98)<>"15" then
       formValid=False
       message="戶籍地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(100)))=0 then
       formValid=False
       message="戶籍地址(地址)不可空白"          

  elseif len(trim(dspkey(102)))=0 then
       formValid=False
       message="裝機地址(縣市)不可空白"   
  elseif len(trim(dspkey(103)))=0 and dspkey(102)<>"06" and dspkey(102)<>"15" then
       formValid=False
       message="裝機地址(鄉鎮)不可空白"
  elseif len(trim(dspkey(104)))=0 then
       formValid=False
       message="裝機地址(地址)不可空白"

  elseif len(trim(dspkey(106)))=0 then
       formValid=False
       message="帳單地址(縣市)不可空白"   
  elseif len(trim(dspkey(107)))=0 and dspkey(106)<>"06" and dspkey(106)<>"15" then
       formValid=False
       message="帳單地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(108)))=0 then
       formValid=False
       message="帳單地址(地址)不可空白"   

'  elseif (len(trim(dspkey(6)))=0 or Not Isdate(dspkey(6))) AND COMPANY="N" then
'       formValid=False
'       message="用戶為個人時，出生日期不可空白或格式錯誤"   
  elseif len(trim(dspkey(2)))=0 then
       formValid=False
       message="異動類別不能空白"
  elseif len(trim(dspkey(95)))=0 and len(trim(dspkey(96)))=0 then
       formValid=False
       message="用戶連絡電話及行動電話至少須輸入一項"   
  elseif instr(1,dspkey(95),"-",1) > 0 then
       formValid=False
       message="連絡電話不可包含'-'符號"         
  elseif instr(1,dspkey(96),"-",1) > 0 then
       formValid=False
       message="行動電話不可包含'-'符號"          
  elseif len(trim(dspkey(110)))=0  AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，企業連絡人不可空白"         
  elseif len(trim(dspkey(111)))=0  AND len(trim(dspkey(113)))=0 AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，企業連絡人連絡電話及行動電話至少需輸入一項"    
  elseif len(trim(dspkey(116)))=0  AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，企業負責人不可空白"
  elseif len(trim(dspkey(117)))=0  AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，企業負責人身份證字號不可空白"                     
  elseif len(trim(dspkey(122)))= 0 then
       formValid=False
       message="方案種類不可空白"
  elseif len(trim(dspkey(161)))=0 and len(trim(dspkey(121))) = 0 then
       message="經銷商與業務員不可同時空白!"
       formValid=False
  'elseif len(trim(dspkey(40)))= 0 AND DSPKEY(38) <> "Y" then
  '     formValid=False
  '     message="AVS繳款方式不可空白"      
  'elseif len(trim(dspkey(40)))> 0 AND DSPKEY(38) = "Y" then
  '     formValid=False
  '     message="公關機時，AVS繳款方式必須空白"           
  elseif len(trim(dspkey(130)))<> 0 AND len(trim(dspkey(129)))= 0 then
       formValid=False
       message="完工日期為空白時不可輸入報竣日"       
  'elseif len(trim(dspkey(46)))<> 0 AND ( len(trim(dspkey(55)))= 0 or len(trim(dspkey(56)))= 0 or len(trim(dspkey(57)))= 0 or len(trim(dspkey(58)))= 0 )then
  '     formValid=False
  '     message="輸入完工日期時，用戶IP不可空白"              
  end if

  IF formValid=TRUE THEN
    IF dspkey(90) <> "" and (dspkey(89)="01" or dspkey(89)="02") then
       idno=dspkey(90)
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
  IF formValid=TRUE THEN
   if len(trim(dspkey(117)))<> 0 then
      idno=dspkey(117)
        if UCASE(left(idno,1)) >="A" AND UCASE(left(idno,1)) <="Z" THEN
          AAA=CheckID(idno)
          SELECT CASE AAA
             CASE "True"
             case "False"
                   message="企業負責人身份證字號不合法"
                   formvalid=false 
             case "ERR-1"
                   message="企業負責人身份證號不可留空白或輸入位數錯誤"
                   formvalid=false       
             case "ERR-2"
                   message="企業負責人身份證字號的第一碼必需是合法的英文字母"
                   formvalid=false    
             case "ERR-3"
                   message="企業負責人身份證字號的第二碼必需是數字 1 或 2"
                   formvalid=false   
             case "ERR-4"
                   message="企業負責人身份證字號的後九碼必需是數字"
                   formvalid=false              
             case else
          end select  
       ELSE
          AAA=ValidBID(idno)
          if aaa = false then
              message="企業負責人統一編號不合法"
              formvalid=false   
          end if
       END IF
   END IF
  END IF
  
  '檢查主線開發為直銷或經銷==當經銷時,則績效歸屬部份為空白,反之則必須輸入
  'IF formValid=TRUE THEN
  ' Set connxx=Server.CreateObject("ADODB.Connection")
  ' Set rsxx=Server.CreateObject("ADODB.Recordset")
  ' connxx.open DSN
  ' sqlxx="select * from RTSparq499Cmtyline where comq1=" & aryparmkey(0) & " AND LINEQ1=" & ARYPARMKEY(1)
  ' rsxx.Open sqlxx,connxx
  ' if not rsxx.eof then
  '    if len(trim(rsxx("consignee"))) <> 0 then
  '       if len(trim(dspkey(34))) <> 0 or len(trim(dspkey(35))) <> 0 or len(trim(dspkey(36))) <> 0then
  '          formValid=False
  '          message="主線開發為經銷商,績效歸屬必須空白" 
  '       end if
  '    else
  '       if len(trim(dspkey(34))) = 0 or len(trim(dspkey(35))) = 0 or len(trim(dspkey(36))) = 0 then
  '          formValid=False
  '          message="主線開發為直銷,績效歸屬不可空白" 
  '       end if
  '    end if
      '主線未測通者，不可輸入avs申請日
  '    if isnull(rsxx("ADSLOPENDAT")) and len(trim(dspkey(46))) <> 0 then
  '          formValid=False
  '          message="主線未測通，不可輸入用戶完工日" 
  '    ELSEif isnull(rsxx("ADSLOPENDAT")) and len(trim(dspkey(47))) <> 0 then
  '          formValid=False
  '          message="主線未測通，不可輸入用戶報竣日" 
  '    end if

  '   IF NOT ISNULL(RSXX("DROPDAT")) OR NOT ISNULL(RSXX("CANCELDAT")) THEN
  '      formValid=False
  '      message="主線已作廢或撤銷，不可新增及異動用戶資料" 
  '   END IF
  ' end if
  ' rsxx.close
  ' connxx.Close   
  ' set rsxx=Nothing   
  ' set connxx=Nothing 
  'END IF
  
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue, ";")  
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

   Sub Srcounty12onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY98").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key99").value =  trim(Fusrid(0))
          document.all("key101").value =  trim(Fusrid(1))
       End if       
       end if
   End Sub

   Sub Srcounty16onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY102").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key103").value =  trim(Fusrid(0))
          document.all("key105").value =  trim(Fusrid(1))
       End if       
       end if
    END SUB

   Sub Srcounty20onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY106").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key107").value =  trim(Fusrid(0))
          document.all("key109").value =  trim(Fusrid(1))
       End if       
       end if
    END SUB

  Sub Srsalesgrouponclick()
       prog="RTGetsalesgroupD.asp"
       prog=prog & "?KEY=" & document.all("KEY119").VALUE 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
		  FUsrID=Split(Fusr,";")   
		  if Fusrid(2) ="Y" then
			 document.all("key120").value =  trim(Fusrid(0))
		  End if       
       end if
   End Sub

   Sub Srsalesonclick()
       prog="RTGetsalesD.asp"
       prog=prog & "?KEY=" & document.all("KEY119").VALUE & ";" & document.all("KEY120").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
		  FUsrID=Split(Fusr,";")   
		  if Fusrid(2) ="Y" then
			 document.all("key121").value =  trim(Fusrid(0))
		  End if       
       end if
   End Sub

   Sub SrDeveloperonclick()
       prog="RTGetDeveloperD.asp"
       prog=prog & "?KEY=" & document.all("KEY162").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key162").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub      
   

Sub SrAddrEqual1()
    document.All("key102").value=document.All("key98").value
    document.All("key103").value=document.All("key99").value
    document.All("key104").value=document.All("key100").value
    document.All("key105").value=document.All("key101").value
End Sub 
Sub SrAddrEqual2()
    document.All("key106").value=document.All("key98").value
    document.All("key107").value=document.All("key99").value
    document.All("key108").value=document.All("key100").value
    document.All("key109").value=document.All("key101").value
End Sub         
Sub SrAddrEqual3()
    document.All("key106").value=document.All("key102").value
    document.All("key107").value=document.All("key103").value
    document.All("key108").value=document.All("key104").value
    document.All("key109").value=document.All("key105").value
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
				rs.open "select max(entryno) AS entryno from RTSparqVoIPCustChg where cusid='" & dspkey(0) & "'",conn
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
		sql="SELECT CUSID, CUSNC, FIRSTIDTYPE, SOCIALID, SECONDIDTYPE, SECONDNO, "_
			&"BIRTHDAY, EMAIL, CONTACTTEL, MOBILE, FAX, CUTID1, TOWNSHIP1, "_
			&"RADDR1, RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, CUTID3, "_
			&"TOWNSHIP3, RADDR3, RZONE3, COCONTACT, COCONTACTTEL, "_
			&"COCONTACTTELEXT, COMOBILE, COFAX, COEMAIL, COBOSS, COBOSSSOCIAL, "_
			&"TRADETYPE, AREAID, GROUPID, SALESID, "_
			&"CASETYPE, FREECODE, AGENTNAME, AGENTSOCIAL, AGENTTEL, RCVD, "_
			&"APPLYDAT, FINISHDAT, DOCKETDAT, DROPDAT, CANCELDAT, "_
			&"CANCELUSR, OVERDUE, MEMO, NCICCUSNO, CUSTIP1, CUSTIP2, CUSTIP3, "_
			&"CUSTIP4, CUSTIPEND, MACNO, VOIPTEL, VOIPTELSTR, VOIPTELEND, "_
			&"ISPTYPE, ISPETC, CIRCUITTYPE, CIRCUITETC, LINKTYPE, LINKETC, LINERATE, "_
			&"LINESHARE, LINETEL, CREDITTYPE, CREDITBANK, CREDITNO, CREDITNAME, "_
			&"VALIDMONTH, VALIDYEAR, CONSIGNEE, DEVELOPERID, WRKNO1, WRKNO2, "_
			&"WRKRCVDAT, WRKSETDAT, CONSIGNEESALE "_
			&"FROM RTSparqVoIPCust "_
			&"WHERE cusid ='" &dspKey(0) &"' "
		rs.Open sql, conn
	    For i = 1 To rs.Fields.Count-1
			  dspkey(i+7)=rs.Fields(i).Value
			  dspkey(i+87)=rs.Fields(i).Value			  
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


<table width="100%" ID="Table12" bgcolor="silver"><tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
		<tr><td>&nbsp;</td><td>

<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
	<tr><td bgcolor="BDB76B" align="LEFT" colspan="4">基本資料</td></tr>

	<tr><td class="dataListSEARCH" height="23">用戶號碼</td>                                 
        <td height="23" bgcolor="silver" COLSPAN=3>
        <input type="text" name="key136" size="15" maxlength="10" <%=fieldpa%> value="<%=dspKey(136)%>" <%=fieldRole(1)%><%=dataProtect%> class="dataListENTRY"></TD></TR>

	<tr><td width="15%" class=dataListHEAD>收件日</td>
		<td width="35%" bgcolor="silver" >
        <input type="text" name="key127" value="<%=dspKey(127)%>" <%=fieldpa%> <%=fieldRole(1)%> <%=dataProtect%> style="text-align:left;" maxlength="10" READONLY size="10" class=dataListData>
		<input type="button" name="B127" id="B127" <%=fieldpb%> height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
		<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" name="C127" id="C127" <%=fieldpb%> alt="清除"  style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>

		<td width="15%" class=dataListHEAD>用戶申請日</td>
		<td width="35%" bgcolor="silver">
        <input type="text" name="key128" value="<%=dspKey(128)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" maxlength="10" READONLY size="10" class=dataListData>
		<input  type="button" id="B128" name="B128" <%=fieldpb%> height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
		<IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" id="C128" name="C128" <%=fieldpb%> alt="清除" style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td></tr>
		
	<tr><td width="15%" class=dataListHEAD>用戶(公司)名稱</td>
		<td width="35%" bgcolor="silver">
        <input type="text" name="key88" value="<%=dspKey(88)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" maxlength="30" size="30" class=dataListENTRY></td>

		<td class="dataListHEAD" height="23">出生日期</td>                                 
		<td height="23" bgcolor="silver">
        <input type="text" name="key93" size="10" value="<%=dspKey(93)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListData" READONLY ID="Text1">  
        <input type="button" id="B93"  <%=fieldpb%>  name="B93" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C93" name="C93" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td></tr>

<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='J5' " 
       If len(trim(dspkey(89))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='J5' AND CODE='" & dspkey(89) &"' " 
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(第一證照別)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(89) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td width="15%" class=dataListHEAD>身分證(統編)</td>
		<td width="35%" bgcolor="silver">
		<select size="1" name="key89"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><%=s%></select>    
		<input type="text" name="key90" value="<%=dspKey(90)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
			style="text-align:left;" maxlength="15" size="15" class=dataListENTRY></td>

<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='L3' " 
       If len(trim(dspkey(91))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='L3' AND CODE='" & dspkey(91) &"' " 
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(第二證照別)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(91) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
		<td width="10%" class="dataListHead" height="25">第二證照別及號碼</td>
        <td width="18%" height="25" bgcolor="silver">
		<select size="1" name="key91"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><%=s%></select>	
        <input type="text" name="key92" value="<%=dspkey(92)%>" size="15" maxlength="15" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td></tr>

	<TR><td class="dataListHEAD" height="23">連絡電話</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key95" size="20" maxlength="20" <%=fieldpa%> value="<%=dspKey(95)%>" <%=fieldRole(1)%> class="dataListEntry"></td>

        <td class="dataListHEAD" height="23">行動電話</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key96" size="30" maxlength="30" <%=fieldpa%> value="<%=dspKey(96)%>" <%=fieldRole(1)%> class="dataListEntry"</td></tr>

	<tr><td class="dataListHEAD" height="23">傳真</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key97" size="30" maxlength="30" <%=fieldpa%> value="<%=dspKey(97)%>" <%=fieldRole(1)%> class="dataListEntry"</td>

		<td class="dataListHEAD" height="23">連絡EMAIL</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key94" size="50" maxlength="50" <%=fieldpa%> value="<%=dspKey(94)%>" <%=fieldRole(1)%> class="dataListEntry"></td></tr>

<%
	s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)  Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(98))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX12=" onclick=""Srcounty12onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(98) & "' " 
       SXX12=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(98) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td class=dataListHEAD>戶籍/公司地址</td>
		<td bgcolor="silver" colspan="3">
		<select size="1" name="key98" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry"><%=s%></select>
        <input type="text" name="key99" readonly  size="8" value="<%=dspkey(99)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA"><font SIZE=2>(鄉鎮)
			<input type="button" id="B99" <%=fieldpb%> name="B99" width="100%" style="Z-INDEX: 1" value="...." <%=SXX12%> >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C99" name="C99" style="Z-INDEX: 1" onclick="SrClear" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
        <input type="text" name="key100" <%=fieldpa%> size="40" value="<%=dspkey(100)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><font size=2>
		<input type="text" name="key101" readonly size="5" value="<%=dspkey(101)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA"></td></tr>
<%
	s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(102))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX16=" onclick=""Srcounty16onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(102) & "' " 
       SXX16=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(102) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td class=dataListHEAD>裝機地址<br><input type="radio" name="rd1" <%=fieldpb%> onClick="SrAddrEqual1()" ID="Radio3" VALUE="Radio3">同戶籍</td>
		<td bgcolor="silver" colspan=3>
		<select size="1" name="key102" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry"><%=s%></select>
        <input type="text" name="key103" readonly  size="8" value="<%=dspkey(103)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA"><font SIZE=2>(鄉鎮)
			<input type="button" id="B103"  <%=fieldpb%>  name="B103"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX16%>  >        
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C103" name="C103" style="Z-INDEX: 1" onclick="SrClear" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">          
		<input type="text" name="key104" <%=fieldpa%> size="40" value="<%=dspkey(104)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><font size=2>
		<input type="text" name="key105"  readonly size="5" value="<%=dspkey(105)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA"></td></tr>
<%
	s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(106))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX20=" onclick=""Srcounty20onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(106) & "' " 
       SXX20=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(106) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td class=dataListHEAD>帳單地址<br><input type="radio" name="rd2" <%=fieldpb%>  onClick="SrAddrEqual2()" ID="Radio4" VALUE="Radio4"><font SIZE=2>同戶籍</font><input type="radio"  <%=fieldpb%> name="rd2" onClick="SrAddrEqual3()" ID="radio"  <%=fieldpb%>1><font SIZE=2>同裝機</font></td>
		<td bgcolor="silver" colspan=3>
		<select size="1" name="key106" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry"><%=s%></select>
        <input type="text" name="key107" readonly size="8" value="<%=dspkey(107)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA"><font SIZE=2>(鄉鎮)
			<input type="button" id="B107" <%=fieldpb%> name="B107" width="100%" style="Z-INDEX: 1" value="...." <%=SXX20%> >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C107"  name="C107"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >
        <input type="text" name="key108" <%=fieldpa%> size="40" value="<%=dspkey(108)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><font size=2>
		<input type="text" name="key109" readonly size="5" value="<%=dspkey(109)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA"></td></tr>

	<TR><td class="dataListHEAD" height="23">企業負責人</td>
        <td height="23" bgcolor="silver" >
        <input type="text" name="key116" size="10" maxlength="10" <%=fieldpa%> value="<%=dspKey(116)%>" <%=fieldRole(1)%> class="dataListEntry"></td>

        <td class="dataListHEAD" height="23">負責人身份證號</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key117" size="10" maxlength="10" <%=fieldpa%> value="<%=dspKey(117)%>" <%=fieldRole(1)%> class="dataListEntry"></td></tr>

	<TR><td class="dataListHEAD" height="23">行業別</td>
        <td height="23" bgcolor="silver" >
        <input type="text" name="key118" size="20" maxlength="20" <%=fieldpa%> value="<%=dspKey(118)%>" <%=fieldRole(1)%> class="dataListEntry"></td>
        
		<td class="dataListHEAD" height="23">企業連絡人</td>
		<td height="23" bgcolor="silver" >
        <input type="text" name="key110" size="15" maxlength="12" <%=fieldpa%> value="<%=dspKey(110)%>" <%=fieldRole(1)%> class="dataListEntry"></td></tr>

	<TR><td class="dataListHEAD" height="23">企業連絡電話</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key111" size="15" maxlength="15" <%=fieldpa%> value="<%=dspKey(111)%>" <%=fieldRole(1)%> class="dataListEntry">
        <font size=2>分機︰</font>
        <input type="text" name="key112" size="5" maxlength="5" <%=fieldpa%> value="<%=dspKey(112)%>" <%=fieldRole(1)%> class="dataListEntry"></td>

		<td class="dataListHEAD" height="23">企業傳真</td>
		<td height="23" bgcolor="silver" >
        <input type="text" name="key114" size="30" maxlength="30" <%=fieldpa%> value="<%=dspKey(114)%>" <%=fieldRole(1)%> class="dataListEntry"></td></tr>

	<tr><td class="dataListHEAD" height="23">企業行動電話</td>
        <td height="23" bgcolor="silver" >
        <input type="text" name="key113" size="10" maxlength="10" <%=fieldpa%> value="<%=dspKey(113)%>" <%=fieldRole(1)%> class="dataListEntry"></td>

		<td class="dataListHEAD" height="23">企業 E-Mail</td>
		<td height="23" bgcolor="silver" >
        <input type="text" name="key115" size="50" maxlength="50" <%=fieldpa%> value="<%=dspKey(115)%>" <%=fieldRole(1)%> class="dataListEntry"></td></tr>


	<tr><td bgcolor="BDB76B" align="LEFT" colspan="4">績效歸屬</td></tr>
<%
    s80=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT RTObj.CUSID AS CusID, RTObj.SHORTNC AS SHORTNC " _
           &"FROM RTObj INNER JOIN RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
           &"WHERE (((RTObjLink.CUSTYID)='02')) " _
           &"ORDER BY RTObj.SHORTNC " 
       If len(trim(dspkey(161))) < 1 Then
          sx=" selected " 
          s80=s80 & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s80=s80 & "<option value=""""" & sx & "></option>"  
          sx=""
       end if
    Else
       sql="SELECT RTObj.CUSID AS CusID, RTObj.SHORTNC AS SHORTNC " _
           &"FROM RTObj INNER JOIN RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
           &"WHERE (((RTObjLink.CUSTYID)='02')) and rtobj.cusid='" & DSPKEY(161) & "' " _
           &"ORDER BY RTObj.SHORTNC "
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(161) Then sx=" selected "
       s80=s80 &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td WIDTH="15%" class="dataListHEAD" height="23">經銷商</td>
        <td WIDTH="35%" height="23" bgcolor="silver">
			<select size="1" name="KEY161" <%=fieldpg%><%=FIELDROLE(1)%> class="dataListEntry"><%=S80%></select>
		<font size=2>經銷商開發業務:</font>
        <input type="text" name="key167" size="20" maxlength="20" value="<%=dspKey(167)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td>

<%
	name=""
	if dspkey(162) <> "" then
		sqlxx=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			 &"where rtemployee.emply='" & dspkey(162) & "' "
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
		<td width="35%"><input type="text" name="key162"value="<%=dspKey(162)%>" <%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" size="8" maxlength="6" readonly class="dataListDATA" ID="Text2">
			<input type="BUTTON" id="B162" name="B162" style="Z-INDEX: 1"  value="...." onclick="Srdeveloperonclick()">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C162" name="C162" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">
			<font size=2><%=name%></font></td></tr>

<%  
	If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)  Then 
	   sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '1') "
       s="<option value="""" >(業務轄區)</option>"
    Else
       sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '1') AND AREAID='" & DSPKEY(119) & "' "
       s="<option value="""" >(業務轄區)</option>"
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(業務轄區)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("areaid")=dspkey(119) Then sx=" selected "
       s=s &"<option value=""" &rs("areaid") &"""" &sx &">" &rs("areanc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td id="tagT1" WIDTH="15%" class="dataListHEAD" height="23">業務轄區</td>
        <td WIDTH="85%" height="23" bgcolor="silver" colspan=3>
		<select size="1" name="key119" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry"><%=s%></select>

<%
	name=""
	if dspkey(120) <> "" then
		sqlxx=" select groupnc from RTSalesGroup where groupid='" & dspkey(120) & "' "
		rs.Open sqlxx,conn
		if rs.eof then
			name="(對象檔找不到業務組別)"
		else
			name=rs("groupnc")
		end if
		rs.close
	end if
%>
		<input type="text" name="key120" value="<%=dspKey(120)%>" <%=fieldpa%> <%=fieldRole(1)%><%=dataProtect%> size="3" maxlength="2" style="text-align:left;" readonly class="dataListData">
			<input type="button" id="B120" name="B120" <%=fieldpb%> width="100%" style="Z-INDEX: 1" value="...." readonly onclick="SrsalesGrouponclick()">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C120" name="C120" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">
			<font size=2><%=name%></font>

<%
	name=""
	if dspkey(121) <> "" then
		sqlxx=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			 &"where rtemployee.emply='" & dspkey(121) & "' "
		rs.Open sqlxx,conn
		if rs.eof then
			name="(對象檔找不到業務員)"
		else
			name=rs("cusnc")
		end if
		rs.close
	end if
%>
		<input type="text" name="key121" value="<%=dspKey(121)%>" <%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" size="8" maxlength="6" readonly class="dataListDATA" ID="Hidden1">
			<input type="BUTTON" id="B121" <%=fieldpb%> name="B121"  width="100%" style="Z-INDEX: 1"  value="...." onclick="Srsalesonclick()">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C121" name="C121" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">
			<font size=2><%=name%></font></td></tr>


	<tr><td bgcolor="BDB76B" align="LEFT" colspan="4">申請服務 / 電話號嗎明細表</td></tr>

<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M2' " 
       If len(trim(dspkey(122))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M2' AND CODE='" & dspkey(122) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(122) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td WIDTH="15%"  class="dataListHEAD" height="23">方案類別</td>               
        <td WIDTH="35%" height="23" bgcolor="silver" >
		<select size="1"name="key122" <%=fieldpC%> <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry"><%=s%></select></td>

<%  
	dim FREECODE1,FREECODE2
    If Len(Trim(fieldRole(1) &dataProtect)) < 1 and flg = "Y" Then
       FREECODE1=""
       FREECODE2=""
    Else
      ' sexd1=" disabled "
      ' sexd2=" disabled "
    End If
    If dspKey(123)="Y" Then FREECODE1=" checked "    
    If dspKey(123)="N" or len(dspKey(123)) =0 Then FREECODE2=" checked " 
%>                          
        <td  WIDTH="15%" class="dataLISTSEARCH" height="23">公關機</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver" >
        <input type="radio" name="key123" value="Y" <%=FREECODE1%> <%=fieldRole(1)%><%=dataProtect%> ID="Radio1">是
        <input type="radio" name="key123" value="N" <%=FREECODE2%>  <%=fieldRole(1)%><%=dataProtect%> ID="Radio2">否</td></tr>

	<TR><td width=15% class="dataListHEAD" height="23">網路電話MAC號碼</td>
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key142" size="20" maxlength="17" value="<%=dspKey(142)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td>

        <td width=15% class="dataListHEAD" height="23">電話號碼(代表號)</td>
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key143" size="11" maxlength="11" value="<%=dspKey(143)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry">
        <font size=2>，區間</font>
        <input type="text" name="key144" size="5" maxlength="4" value="<%=dspKey(144)%>" <%=fieldpc%> <%=fieldRole(1)%> class="dataListEntry"> ~ 
        <input type="text" name="key145" size="5" maxlength="4" value="<%=dspKey(145)%>" <%=fieldpc%> <%=fieldRole(1)%> class="dataListEntry"></td></tr>

<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M3' " 
       If len(trim(dspkey(146))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M3' AND CODE='" & dspkey(146) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(146) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td WIDTH="15%"  class="dataListHEAD" height="23">目前使用之寬頻服務</td>               
        <td WIDTH="35%" height="23" bgcolor="silver" >
		<select size="1"name="key146" <%=fieldpC%> <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry"><%=s%></select>
        <font size=2>　</font>
        <input type="text" name="key147" size="20" maxlength="20" value="<%=dspKey(147)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td>

<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M4' " 
       If len(trim(dspkey(148))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M4' AND CODE='" & dspkey(148) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(148) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
		<td WIDTH="15%"  class="dataListHEAD" height="23">電路服務業者</td>               
        <td WIDTH="35%" height="23" bgcolor="silver" >
		<select size="1"name="key148" <%=fieldpC%> <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry"><%=s%></select>
        <font size=2>　</font>
        <input type="text" name="key149" size="20" maxlength="20" value="<%=dspKey(149)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td></tr>
        
<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M5' " 
       If len(trim(dspkey(150))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M5' AND CODE='" & dspkey(150) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(150) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td WIDTH="15%"  class="dataListHEAD" height="23">上網型態</td>               
        <td WIDTH="35%" height="23" bgcolor="silver" >
		<select size="1"name="key150" <%=fieldpC%> <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry"><%=s%></select>
        <font size=2>　</font>
        <input type="text" name="key151" size="20" maxlength="20" value="<%=dspKey(151)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td>

<%
    s="<option value="""" >(頻寬)</option>"
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D3' " 
       If len(trim(dspkey(152))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D3' AND CODE='" & dspkey(152) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(152) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
		<td WIDTH="15%"  class="dataListHEAD" height="23">頻寬</td>               
        <td WIDTH="35%" height="23" bgcolor="silver" >
		<select size="1"name="key152" <%=fieldpC%> <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry"><%=s%></select>

<%  
	dim lineshare1,lineshare2
    If Len(Trim(fieldRole(1) &dataProtect)) < 1 and flg = "Y" Then
       lineshare1=""
       lineshare2=""
    Else
      ' sexd1=" disabled "
      ' sexd2=" disabled "
    End If
    If dspKey(153)="Y" Then lineshare1=" selected "    
    If dspKey(153)="N" Then lineshare2=" selected "
%>       

        <font size=2>是否與上網共用</font>
		<select size="1" name="key153"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
			<option value=""></option>
			<option value="Y" <%=lineshare1%>>Y</option>
			<option value="N" <%=lineshare2%>>N</option></select></td></tr>
<!--		        
        <input type="radio" value="Y" <%=lineshare1%> name="key72" <%=fieldRole(1)%><%=dataProtect%> ID="Radio5">是
        <input type="radio" name="key72" value="N" <%=lineshare2%>  <%=fieldRole(1)%><%=dataProtect%> ID="Radio6">否</td></tr>
-->        

	<TR><td width=15% class="dataListHEAD" height="23">ADSL附掛電話</td>
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key154" size="12" maxlength="11" value="<%=dspKey(154)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td>
        
        <td class="dataListSEARCH" height="23">固定IP位址</td>                                 
        <td height="23" bgcolor="silver"><font color=red>
		<input type="text" name="key137" size="3" maxlength="3" value="<%=dspKey(137)%>" <%=fieldRole(1)%> class="dataListEntry"><font size=2>.</font>
        <input type="text" name="key138" size="3" maxlength="3" value="<%=dspKey(138)%>" <%=fieldRole(1)%> class="dataListEntry"><font size=2>.</font>
        <input type="text" name="key139" size="3" maxlength="3" value="<%=dspKey(139)%>" <%=fieldRole(1)%> class="dataListEntry"><font size=2>.</font>
        <input type="text" name="key140" size="3" maxlength="3" value="<%=dspKey(140)%>" <%=fieldRole(1)%> class="dataListEntry"><font size=2> ~ </font>
        <input type="text" name="key141" size="3" maxlength="3" value="<%=dspKey(141)%>" <%=fieldRole(1)%> class="dataListEntry"></font></td></tr>

	
	<tr><td bgcolor="BDB76B" align="LEFT" colspan="4">代理人資訊</td></tr>

	<TR><td width=15% class="dataListHEAD" height="23">代理人姓名</td>
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key124" size="15" maxlength="12" value="<%=dspKey(124)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td>

        <td width=15% class="dataListHEAD" height="23">代理人身份證號</td>
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key125" size="10" maxlength="10" value="<%=dspKey(125)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td></tr>

	<TR><td class="dataListHEAD" height="23">代理人電話</td>
        <td height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key126" size="20" maxlength="20" value="<%=dspKey(126)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td></tr>


    <tr><td bgcolor="BDB76B" align="LEFT" colspan="4">用戶派工狀態</td></tr>

	<tr><td width=15% class="dataListHEAD" height="23">派工單號</td>
        <td width=35% height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key163" size="12" maxlength="11" value="<%=dspKey(163)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"> / 
        <input type="text" name="key164" size="12" maxlength="11" value="<%=dspKey(164)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td></tr>

	<tr><td class="dataListHEAD" height="23">工單收件日(派工日)</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key165" size="10" READONLY value="<%=dspKey(165)%>" <%=fieldpC%>  <%=fieldRole(1)%> class="dataListentry">
			<input type="button" id="B165" name="B165" height="100%" width="100%" <%=fieldpD%> style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C165" name="C165" <%=fieldpD%> style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>

		<td class="dataListHEAD" height="23">話機設定日(計費起算)</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key166" size="10" READONLY value="<%=dspKey(166)%>" <%=fieldpC%>  <%=fieldRole(1)%> class="dataListentry">
			<input type="button" id="B166" name="B166" height="100%" width="100%"  <%=fieldpD%> style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C166"  name="C166" <%=fieldpD%> style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td></tr>

	<tr><td class="dataListHEAD" height="23">安裝完工日</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key129" size="10" READONLY value="<%=dspKey(129)%>" <%=fieldpC%>  <%=fieldRole(1)%> class="dataListentry">
			<input type="button" id="B129"  name="B129" height="100%" width="100%"  <%=fieldpD%> style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C129"  name="C129"    <%=fieldpD%> style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>

        <td class="dataListHEAD" height="23">報竣回覆日(回報Sparq)</td>                                 
        <td height="23" bgcolor="silver">
		<input type="text" name="key130" size="10" READONLY value="<%=dspKey(130)%>" <%=fieldpe%> <%=fieldRole(1)%> class="dataListDATA">
			<input type="button" id="B130"  name="B130" height="100%" width="100%" <%=fieldpf%>style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C130"  name="C130"   <%=fieldpf%>style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></TD></tr> 

	<tr><td class="dataListHEAD" height="23">退租日</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key131" size="10" value="<%=dspKey(131)%>" <%=fieldRole(1)%> class="dataListentry">
			<input type="button" id="B131"  name="B131" height="100%" width="100%" <%=fieldpf%>style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C131" name="C131" <%=fieldpf%>style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">

        <font size=2>欠費︰</font>
        <input type="text" name="key134" size="2" value="<%=dspKey(134)%>" <%=fieldRole(1)%> class="dataListentry"></td></tr>

	<tr><td width=15% class="dataListHEAD" height="23">作廢日期</td>                                 
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key132" size="10" value="<%=dspKey(132)%>" <%=fieldpa%><%=fieldRole(1)%> readonly class="dataListentry">
			<input type="button" id="B132"  name="B132" height="100%" width="100%" <%=fieldpf%>style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C132" name="C132" <%=fieldpf%>style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
        </td>
        

<%
	name="" 
	if dspkey(133) <> "" then
		sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			&"where rtemployee.emply='" & dspkey(133) & "' "
		rs.Open sql,conn
		if rs.eof then
			name=""
		else
			name=rs("cusnc")
		end if
		rs.close
	end if
%>         
        <td width=15% class="dataListHEAD" height="23">作廢人員</td>                                 
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key133" size="10" value="<%=dspKey(133)%>" <%=fieldRole(1)%> readonly class="dataListDATA"><font size=2><%=name%></font>
        </td></tr>           


    <tr><td bgcolor="BDB76B" align="LEFT" colspan="4">信用卡資料</td></tr>

<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='M6' " 
       If len(trim(dspkey(155))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='M6' AND CODE='" & dspkey(155) &"' " 
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(信用卡類型)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(155) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td width="15%" class="dataListHEAD" height="23">信用卡</td>
		<td width="35%" bgcolor="silver">
		<select size="1" name="key155"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry"><%=s%></select>

		<td width="15%" class="dataListHEAD" height="23">發卡銀行</td>
        <td width="35%" height="23" bgcolor="silver">
		<input type="text" name="key156" value="<%=dspKey(156)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
			style="text-align:left;" maxlength="20" size="25" class=dataListENTRY></td></tr>

	<tr><td class="dataListHEAD" height="23">信用卡卡號</td>
        <td height="23" bgcolor="silver">
		<input type="text" name="key157" value="<%=dspKey(157)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
			style="text-align:left;" maxlength="16" size="20" class=dataListENTRY></td>

		<td class="dataListHEAD" height="23">持卡人姓名</td>
        <td height="23" bgcolor="silver">
		<input type="text" name="key158" value="<%=dspKey(158)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
			style="text-align:left;" maxlength="20" size="20" class=dataListENTRY></td></tr>

	<tr><td class="dataListHEAD" height="23">信用卡有效期限</td>
        <td height="23" bgcolor="silver" colspan=3>
		<input type="text" name="key159" value="<%=dspKey(159)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
			style="text-align:left;" maxlength="2" size="5" class=dataListENTRY>月/
		<input type="text" name="key160" value="<%=dspKey(160)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
			style="text-align:left;" maxlength="2" size="5" class=dataListENTRY>年</td></tr>


   <tr><td bgcolor="BDB76B" align="LEFT" colspan="4">備註說明</td></tr>

    <TR><TD align="CENTER" bgcolor="silver" colspan="4">
		<TEXTAREA cols="100%" name="key135" rows=8 MAXLENGTH=500 class="dataListentry" <%=dataprotect%> value="<%=dspkey(135)%>" ID="Textarea1"><%=dspkey(135)%></TEXTAREA></td></tr>
</table>


   
<!-- 異動前資料 ========================================================================================================== -->
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag2" style="display: none">
	<tr><td bgcolor="BDB76B" align="LEFT" colspan="4">基本資料</td></tr>

	<tr><td class="dataListSEARCH" height="23">用戶號碼</td>                                 
        <td height="23" bgcolor="silver" COLSPAN=3>
        <input type="text" name="key56" size="15" maxlength="10" <%=fieldlock%> value="<%=dspKey(56)%>" class="dataListENTRY" ID="Text9"></TD></TR>

	<tr><td width="15%" class=dataListHEAD>收件日</td>
		<td width="35%" bgcolor="silver" >
        <input type="text" name="key47" value="<%=dspKey(47)%>" <%=fieldlock%> style="text-align:left;" maxlength="10" READONLY size="10" class=dataListEntry ID="Text10"></td>

		<td width="15%" class=dataListHEAD>用戶申請日</td>
		<td width="35%" bgcolor="silver">
        <input type="text" name="key48" value="<%=dspKey(48)%>" <%=fieldlock%> style="text-align:left;" maxlength="10" READONLY size="10" class=dataListEntry ID="Text11"></td></tr>
		
	<tr><td width="15%" class=dataListHEAD>用戶(公司)名稱</td>
		<td width="35%" bgcolor="silver">
        <input type="text" name="key8" value="<%=dspKey(8)%>" <%=fieldlock%> style="text-align:left;" maxlength="30" size="30" class=dataListENTRY ID="Text12"></td>

		<td class="dataListHEAD" height="23">出生日期</td>                                 
		<td height="23" bgcolor="silver">
        <input type="text" name="key13" size="10" value="<%=dspKey(13)%>" <%=fieldlock%> class="dataListentry" ID="Text13"></td></tr>

<%
    sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='J5' AND CODE='" & dspkey(9) &"' " 
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(第一證照別)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(9) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td width="15%" class=dataListHEAD>身分證(統編)</td>
		<td width="35%" bgcolor="silver">
		<select size="1" name="key9"<%=fieldlock%> class="dataListEntry" ID="Select1"><%=s%></select>    
		<input type="text" name="key10" value="<%=dspKey(10)%>" <%=fieldlock%> style="text-align:left;" 
			maxlength="15" size="15" class=dataListENTRY ID="Text14"></td>

<%
    sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='L3' AND CODE='" & dspkey(11) &"' " 
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(第二證照別)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(11) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
		<td width="10%" class="dataListHead" height="25">第二證照別及號碼</td>
        <td width="18%" height="25" bgcolor="silver">
		<select size="1" name="key11"<%=fieldlock%> class="dataListEntry" ID="Select2"><%=s%></select>	
        <input type="text" name="key12" value="<%=dspkey(12)%>" size="15" maxlength="15" <%=fieldlock%> class="dataListEntry" ID="Text15"></td></tr>

	<TR><td class="dataListHEAD" height="23">連絡電話</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key15" size="20" maxlength="20" <%=fieldlock%> value="<%=dspKey(15)%>"  class="dataListEntry" ID="Text16"></td>

        <td class="dataListHEAD" height="23">行動電話</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key16" size="30" maxlength="30" <%=fieldlock%> value="<%=dspKey(16)%>" class="dataListEntry" ID="Text21""</td></tr>

	<tr><td class="dataListHEAD" height="23">傳真</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key17" size="30" maxlength="30" <%=fieldlock%> value="<%=dspKey(17)%>" class="dataListEntry" ID="Text22""</td>

		<td class="dataListHEAD" height="23">連絡EMAIL</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key14" size="50" maxlength="50" <%=fieldlock%> value="<%=dspKey(14)%>" class="dataListEntry" ID="Text23"></td></tr>

<%
    sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(18) & "' " 
    s=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(18) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &""""  &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
%>
	<tr><td class=dataListHEAD>戶籍/公司地址</td>
		<td bgcolor="silver" colspan="3">
		<select size="1" name="key18" <%=fieldlock%> size="1" class="dataListEntry" ID="Select3"><%=s%></select>
        <input type="text" name="key19" <%=fieldlock%>  size="8" value="<%=dspkey(19)%>" maxlength="10" class="dataListDATA" ID="Text24"><font SIZE=2>(鄉鎮)
        <input type="text" name="key20" <%=fieldlock%> size="40" value="<%=dspkey(20)%>" maxlength="60" class="dataListEntry" ID="Text25"><font size=2>
		<input type="text" name="key21" <%=fieldlock%> size="5" value="<%=dspkey(21)%>" maxlength="5" class="dataListDATA" ID="Text26"></td></tr>
<%
    sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(22) & "' " 
	s=""
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(22) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td class=dataListHEAD>裝機地址</td>
		<td bgcolor="silver" colspan=3>
		<select size="1" name="key22" <%=fieldlock%> size="1" class="dataListEntry" ID="Select4"><%=s%></select>
        <input type="text" name="key23" <%=fieldlock%> size="8" value="<%=dspkey(23)%>" maxlength="10"  class="dataListDATA" ID="Text27"><font SIZE=2>(鄉鎮)
		<input type="text" name="key24" <%=fieldlock%> size="40" value="<%=dspkey(24)%>" maxlength="60" class="dataListEntry" ID="Text28"><font size=2>
		<input type="text" name="key25" <%=fieldlock%> size="5" value="<%=dspkey(25)%>" maxlength="5" class="dataListDATA" ID="Text29"></td></tr>
<%
	s=""
    sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(26) & "' " 
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(26) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td class=dataListHEAD>帳單地址</td>
		<td bgcolor="silver" colspan=3>
		<select size="1" name="key26" <%=fieldlock%> size="1" class="dataListEntry" ID="Select5"><%=s%></select>
        <input type="text" name="key27" <%=fieldlock%> size="8" value="<%=dspkey(27)%>" maxlength="10"  class="dataListDATA" ID="Text30"><font SIZE=2>(鄉鎮)
	    <input type="text" name="key28" <<%=fieldlock%>size="40" value="<%=dspkey(28)%>" maxlength="60" class="dataListEntry" ID="Text31"><font size=2>
		<input type="text" name="key29" <%=fieldlock%> size="5" value="<%=dspkey(29)%>" maxlength="5" class="dataListDATA" ID="Text32"></td></tr>

	<TR><td class="dataListHEAD" height="23">企業負責人</td>
        <td height="23" bgcolor="silver" >
        <input type="text" name="key36" size="10" maxlength="10" <%=fieldlock%> value="<%=dspKey(36)%>" class="dataListEntry" ID="Text33"></td>

        <td class="dataListHEAD" height="23">負責人身份證號</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key37" size="10" maxlength="10" <%=fieldlock%> value="<%=dspKey(37)%>" class="dataListEntry" ID="Text34"></td></tr>

	<TR><td class="dataListHEAD" height="23">行業別</td>
        <td height="23" bgcolor="silver" >
        <input type="text" name="key38" size="20" maxlength="20" <%=fieldlock%> value="<%=dspKey(38)%>" class="dataListEntry" ID="Text35"></td>
        
		<td class="dataListHEAD" height="23">企業連絡人</td>
		<td height="23" bgcolor="silver" >
        <input type="text" name="key30" size="15" maxlength="12" <%=fieldlock%> value="<%=dspKey(30)%>"  class="dataListEntry" ID="Text36"></td></tr>

	<TR><td class="dataListHEAD" height="23">企業連絡電話</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key31" size="15" maxlength="15" <%=fieldlock%> value="<%=dspKey(31)%>" class="dataListEntry" ID="Text38">
        <font size=2>分機︰</font>
        <input type="text" name="key32" size="5" maxlength="5" <%=fieldlock%> value="<%=dspKey(32)%>" class="dataListEntry" ID="Text39"></td>

		<td class="dataListHEAD" height="23">企業傳真</td>
		<td height="23" bgcolor="silver" >
        <input type="text" name="key34" size="30" maxlength="30" <%=fieldlock%> value="<%=dspKey(34)%>" class="dataListEntry" ID="Text40"></td></tr>

	<tr><td class="dataListHEAD" height="23">企業行動電話</td>
        <td height="23" bgcolor="silver" >
        <input type="text" name="key33" size="10" maxlength="10" <%=fieldlock%> value="<%=dspKey(33)%>" class="dataListEntry" ID="Text41"></td>

		<td class="dataListHEAD" height="23">企業 E-Mail</td>
		<td height="23" bgcolor="silver" >
        <input type="text" name="key35" size="50" maxlength="50" <%=fieldlock%> value="<%=dspKey(35)%>" class="dataListEntry" ID="Text42"></td></tr>


	<tr><td bgcolor="BDB76B" align="LEFT" colspan="4">績效歸屬</td></tr>
<%
    s80="<option value="""" >(經銷商)</option>"    
    sql="SELECT RTObj.CUSID AS CusID, RTObj.SHORTNC AS SHORTNC " _
       &"FROM RTObj INNER JOIN RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
       &"WHERE (((RTObjLink.CUSTYID)='02')) and rtobj.cusid='" & DSPKEY(81) & "' " _
       &"ORDER BY RTObj.SHORTNC "
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(81) Then sx=" selected "
       s80=s80 &"<option value=""" &rs("CUSID") &"""" &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td WIDTH="15%" class="dataListHEAD" height="23">經銷商</td>
        <td WIDTH="35%" height="23" bgcolor="silver">
			<select size="1" name="KEY81" <%=fieldlock%> class="dataListEntry" ID="Select6"><%=S80%></select>
		<font size=2>經銷商開發業務:</font>
        <input type="text" name="key87" size="20" maxlength="20" value="<%=dspKey(87)%>" <%=fieldlock%> class="dataListEntry" ID="Text43"></td>

<%
	name=""
	if dspkey(82) <> "" then
		sqlxx=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			 &"where rtemployee.emply='" & dspkey(82) & "' "
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
		<td width="35%"><input type="text" name="key82"value="<%=dspKey(82)%>" <%=fieldlock%> style="text-align:left;" size="8" maxlength="6" class="dataListDATA" ID="Text44">
			<font size=2><%=name%></font></td></tr>

<%  
    sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '1') AND AREAID='" & DSPKEY(39) & "' "
    s="<option value="""" >(業務轄區)</option>"
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(業務轄區)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("areaid")=dspkey(39) Then sx=" selected "
       s=s &"<option value=""" &rs("areaid") &"""" &sx &">" &rs("areanc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td id="tagT1" WIDTH="15%" class="dataListHEAD" height="23">業務轄區</td>
        <td WIDTH="85%" height="23" bgcolor="silver" colspan=3>
		<select size="1" name="key39" <%=fieldlock%> class="dataListEntry" ID="Select7"><%=s%></select>

<%
	name=""
	if dspkey(40) <> "" then
		sqlxx=" select groupnc from RTSalesGroup where groupid='" & dspkey(40) & "' "
		rs.Open sqlxx,conn
		if rs.eof then
			name="(對象檔找不到業務組別)"
		else
			name=rs("groupnc")
		end if
		rs.close
	end if
%>
		<input type="text" name="key40" value="<%=dspKey(40)%>" <%=fieldlock%> size="3" maxlength="2" style="text-align:left;" class="dataListEntry" ID="Text45">
			<font size=2><%=name%></font>

<%
	name=""
	if dspkey(41) <> "" then
		sqlxx=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			 &"where rtemployee.emply='" & dspkey(41) & "' "
		rs.Open sqlxx,conn
		if rs.eof then
			name="(對象檔找不到業務員)"
		else
			name=rs("cusnc")
		end if
		rs.close
	end if
%>
		<input type="text" name="key41" value="<%=dspKey(41)%>" <%=fieldlock%> style="text-align:left;" size="8" maxlength="6" class="dataListDATA" ID="Text46">
			<font size=2><%=name%></font></td></tr>


	<tr><td bgcolor="BDB76B" align="LEFT" colspan="4">申請服務 / 電話號嗎明細表</td></tr>

<%
    s=""
    sx=" selected "
    sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M2' AND CODE='" & dspkey(42) & "'"
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(42) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td WIDTH="15%"  class="dataListHEAD" height="23">方案類別</td>               
        <td WIDTH="35%" height="23" bgcolor="silver" >
		<select size="1"name="key42" <%=fieldlock%> class="dataListEntry" ID="Select8"><%=s%></select></td>

<%  
    If Len(Trim(fieldRole(1) &dataProtect)) < 1 and flg = "Y" Then
       FREECODE1=""
       FREECODE2=""
    Else
      ' sexd1=" disabled "
      ' sexd2=" disabled "
    End If
    If dspKey(43)="Y" Then FREECODE1=" checked "    
    If dspKey(43)="N" or len(dspKey(43)) =0 Then FREECODE2=" checked " 
%>                          
        <td  WIDTH="15%" class="dataLISTSEARCH" height="23">公關機</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver" >
        <input type="radio" name="key43" value="Y" <%=FREECODE1%> disabled <%=fieldlock%> ID="Radio7">是
        <input type="radio" name="key43" value="N" <%=FREECODE2%> disabled <%=fieldlock%> ID="Radio8">否</td></tr>

	<TR><td width=15% class="dataListHEAD" height="23">網路電話MAC號碼</td>
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key62" size="20" maxlength="17" value="<%=dspKey(62)%>" <%=fieldlock%><%=fieldRole(1)%> class="dataListEntry" ID="Text47"></td>

        <td width=15% class="dataListHEAD" height="23">電話號碼(代表號)</td>
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key63" size="11" maxlength="11" value="<%=dspKey(63)%>" <%=fieldlock%> class="dataListEntry" ID="Text48">
        <font size=2>，區間</font>
        <input type="text" name="key64" size="5" maxlength="4" value="<%=dspKey(64)%>" <%=fieldlock%> class="dataListEntry" ID="Text49"> ~ 
        <input type="text" name="key65" size="5" maxlength="4" value="<%=dspKey(65)%>" <%=fieldlock%> class="dataListEntry" ID="Text50"></td></tr>

<%
    s="<option value="""" >(寬頻ISP業者)</option>"
    sx=" selected "
    sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M3' AND CODE='" & dspkey(66) & "'"
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(66) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td WIDTH="15%"  class="dataListHEAD" height="23">目前使用之寬頻服務</td>               
        <td WIDTH="35%" height="23" bgcolor="silver" >
		<select size="1"name="key66" <%=fieldlock%> class="dataListEntry" ID="Select9"><%=s%></select>
        <font size=2>　</font>
        <input type="text" name="key67" size="20" maxlength="20" value="<%=dspKey(67)%>" <%=fieldlock%> class="dataListEntry" ID="Text51"></td>

<%
    s="<option value="""" >(電路業者)</option>"
    sx=" selected "
    sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M4' AND CODE='" & dspkey(68) & "'"
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(68) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
		<td WIDTH="15%"  class="dataListHEAD" height="23">電路服務業者</td>               
        <td WIDTH="35%" height="23" bgcolor="silver" >
		<select size="1"name="key68" <%=fieldlock%> class="dataListEntry" ID="Select10"><%=s%></select>
        <font size=2>　</font>
        <input type="text" name="key69" size="20" maxlength="20" value="<%=dspKey(69)%>" <%=fieldlock%> class="dataListEntry" ID="Text52"></td></tr>
        
<%
    s="<option value="""" >(上網型態)</option>"
    sx=" selected "
    sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M5' AND CODE='" & dspkey(70) & "'"
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(70) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td WIDTH="15%"  class="dataListHEAD" height="23">上網型態</td>               
        <td WIDTH="35%" height="23" bgcolor="silver" >
		<select size="1"name="key70" <%=fieldlock%> class="dataListEntry" ID="Select11"><%=s%></select>
        <font size=2>　</font>
        <input type="text" name="key71" size="20" maxlength="20" value="<%=dspKey(71)%>" <%=fieldlock%> class="dataListEntry" ID="Text53"></td>

<%
    s="<option value="""" >(頻寬)</option>"
    sx=" selected "
    sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D3' AND CODE='" & dspkey(72) & "'"
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(72) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
		<td WIDTH="15%"  class="dataListHEAD" height="23">頻寬</td>               
        <td WIDTH="35%" height="23" bgcolor="silver" >
		<select size="1"name="key72" <%=fieldlock%> class="dataListEntry" ID="Select12"><%=s%></select>

<%  
    If Len(Trim(fieldRole(1) &dataProtect)) < 1 and flg = "Y" Then
       lineshare1=""
       lineshare2=""
    Else
      ' sexd1=" disabled "
      ' sexd2=" disabled "
    End If
    If dspKey(73)="Y" Then lineshare1=" selected "    
    If dspKey(73)="N" Then lineshare2=" selected "
%>       

        <font size=2>是否與上網共用</font>
		<select size="1" name="key73" <%=fieldlock%> disabled class="dataListEntry" ID="Select13">
			<option value=""></option>
			<option value="Y" <%=lineshare1%>>Y</option>
			<option value="N" <%=lineshare2%>>N</option></select></td></tr>
<!--		        
        <input type="radio" value="Y" <%=lineshare1%> name="key72" <%=fieldRole(1)%><%=dataProtect%> ID="Radio5">是
        <input type="radio" name="key72" value="N" <%=lineshare2%>  <%=fieldRole(1)%><%=dataProtect%> ID="Radio6">否</td></tr>
-->        

	<TR><td width=15% class="dataListHEAD" height="23">ADSL附掛電話</td>
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key74" size="12" maxlength="11" value="<%=dspKey(74)%>" <%=fieldlock%> class="dataListEntry" ID="Text54"></td>
        
        <td class="dataListSEARCH" height="23">固定IP位址</td>                                 
        <td height="23" bgcolor="silver"><font color=red>
		<input type="text" name="key57" size="3" maxlength="3" value="<%=dspKey(57)%>" <%=fieldlock%> class="dataListEntry" ID="Text55"><font size=2>.</font>
        <input type="text" name="key58" size="3" maxlength="3" value="<%=dspKey(58)%>" <%=fieldlock%> class="dataListEntry" ID="Text56"><font size=2>.</font>
        <input type="text" name="key59" size="3" maxlength="3" value="<%=dspKey(59)%>" <%=fieldlock%> class="dataListEntry" ID="Text57"><font size=2>.</font>
        <input type="text" name="key60" size="3" maxlength="3" value="<%=dspKey(60)%>" <%=fieldlock%> class="dataListEntry" ID="Text58"><font size=2> ~ </font>
        <input type="text" name="key61" size="3" maxlength="3" value="<%=dspKey(61)%>" <%=fieldlock%> class="dataListEntry" ID="Text59"></font></td></tr>

	
	<tr><td bgcolor="BDB76B" align="LEFT" colspan="4">代理人資訊</td></tr>

	<TR><td width=15% class="dataListHEAD" height="23">代理人姓名</td>
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key44" size="15" maxlength="12" value="<%=dspKey(44)%>" <%=fieldlock%> class="dataListEntry" ID="Text60"></td>

        <td width=15% class="dataListHEAD" height="23">代理人身份證號</td>
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key45" size="10" maxlength="10" value="<%=dspKey(45)%>" <%=fieldlock%> class="dataListEntry" ID="Text61"></td></tr>

	<TR><td class="dataListHEAD" height="23">代理人電話</td>
        <td height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key46" size="20" maxlength="20" value="<%=dspKey(46)%>" <%=fieldlock%> class="dataListEntry" ID="Text62"></td></tr>


    <tr><td bgcolor="BDB76B" align="LEFT" colspan="4">用戶派工狀態</td></tr>

	<tr><td width=15% class="dataListHEAD" height="23">派工單號</td>
        <td width=35% height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key83" size="12" maxlength="11" value="<%=dspKey(83)%>" <%=fieldlock%> class="dataListEntry" ID="Text63"> / 
        <input type="text" name="key84" size="12" maxlength="11" value="<%=dspKey(84)%>" <%=fieldlock%> class="dataListEntry" ID="Text64"></td></tr>

	<tr><td class="dataListHEAD" height="23">工單收件日(派工日)</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key85" size="10"  value="<%=dspKey(85)%>" <%=fieldlock%> class="dataListentry" ID="Text65"></td>

		<td class="dataListHEAD" height="23">話機設定日(計費起算)</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key86" size="10"  value="<%=dspKey(86)%>" <%=fieldlock%> class="dataListentry" ID="Text66"></td></tr>

	<tr><td class="dataListHEAD" height="23">安裝完工日</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key49" size="10"  value="<%=dspKey(49)%>" <%=fieldlock%>  class="dataListentry" ID="Text67"></td>

        <td class="dataListHEAD" height="23">報竣回覆日(回報Sparq)</td>                                 
        <td height="23" bgcolor="silver">
		<input type="text" name="key50" size="10"  value="<%=dspKey(50)%>" <%=fieldlock%> class="dataListDATA" ID="Text68"></TD></tr> 

	<tr><td class="dataListHEAD" height="23">退租日</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key51" size="10" value="<%=dspKey(51)%>" <%=fieldlock%> class="dataListDATA" ID="Text69">

        <font size=2>欠費︰</font>
        <input type="text" name="key54" size="2" value="<%=dspKey(54)%>" <%=fieldlock%> class="dataListDATA" ID="Text70"></td></tr>

	<tr><td width=15% class="dataListHEAD" height="23">作廢日期</td>                                 
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key52" size="10" value="<%=dspKey(52)%>" <%=fieldlock%> class="dataListdata" ID="Text71"></td>

<%
	name="" 
	if dspkey(53) <> "" then
		sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			&"where rtemployee.emply='" & dspkey(53) & "' "
		rs.Open sql,conn
		if rs.eof then
			name=""
		else
			name=rs("cusnc")
		end if
		rs.close
	end if
%>         
        <td width=15% class="dataListHEAD" height="23">作廢人員</td>                                 
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key53" size="10" value="<%=dspKey(53)%>" <%=fieldlock%>  class="dataListDATA" ID="Text72"><font size=2><%=name%></font>
        </td></tr>           


    <tr><td bgcolor="BDB76B" align="LEFT" colspan="4">信用卡資料</td></tr>

<%
    s=""
    sx=" selected "
    sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='M6' AND CODE='" & dspkey(75) &"' " 
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(信用卡類型)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(75) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td width="15%" class="dataListHEAD" height="23">信用卡</td>
		<td width="35%" bgcolor="silver">
		<select size="1" name="key75"<%=fieldlock%> size="1" class="dataListEntry" ID="Select14"><%=s%></select>

		<td width="15%" class="dataListHEAD" height="23">發卡銀行</td>
        <td width="35%" height="23" bgcolor="silver">
		<input type="text" name="key76" value="<%=dspKey(76)%>" <%=fieldlock%>
			style="text-align:left;" maxlength="20" size="25" class=dataListENTRY ID="Text73"></td></tr>

	<tr><td class="dataListHEAD" height="23">信用卡卡號</td>
        <td height="23" bgcolor="silver">
		<input type="text" name="key77" value="<%=dspKey(77)%>" <%=fieldlock%>
			style="text-align:left;" maxlength="16" size="20" class=dataListENTRY ID="Text74"></td>

		<td class="dataListHEAD" height="23">持卡人姓名</td>
        <td height="23" bgcolor="silver">
		<input type="text" name="key78" value="<%=dspKey(78)%>" <%=fieldlock%>
			style="text-align:left;" maxlength="20" size="20" class=dataListENTRY ID="Text75"></td></tr>

	<tr><td class="dataListHEAD" height="23">信用卡有效期限</td>
        <td height="23" bgcolor="silver" colspan=3>
		<input type="text" name="key79" value="<%=dspKey(79)%>" <%=fieldlock%>
			style="text-align:left;" maxlength="2" size="5" class=dataListENTRY ID="Text76">月/
		<input type="text" name="key80" value="<%=dspKey(80)%>" <%=fieldlock%>
			style="text-align:left;" maxlength="2" size="5" class=dataListENTRY ID="Text77">年</td></tr>


   <tr><td bgcolor="BDB76B" align="LEFT" colspan="4">備註說明</td></tr>

    <TR><TD align="CENTER" bgcolor="silver" colspan="4">
		<TEXTAREA cols="100%" name="key55" rows=8 MAXLENGTH=500 class="dataListENTRY" <%=fieldlock%> value="<%=dspkey(55)%>" ID="Textarea2"><%=dspkey(55)%></TEXTAREA></td></tr>

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
    'Set rs=Server.CreateObject("ADODB.Recordset")
	sql="UPDATE	RTSparqVoIPCust "_
	   &"SET	CUSNC = NCUSNC, FIRSTIDTYPE = NFIRSTIDTYPE, SOCIALID = NSOCIALID, SECONDIDTYPE = NSECONDIDTYPE, SECONDNO = NSECONDNO, "_
	   &"BIRTHDAY = NBIRTHDAY, EMAIL = NEMAIL, CONTACTTEL = NCONTACTTEL, MOBILE = NMOBILE, FAX = NFAX, CUTID1=NCUTID1, "_
	   &"TOWNSHIP1=NTOWNSHIP1, RADDR1=NRADDR1, RZONE1=NRZONE1, CUTID2=NCUTID2, TOWNSHIP2=NTOWNSHIP2, RADDR2=NRADDR2, "_
	   &"RZONE2=NRZONE2, CUTID3=NCUTID3, TOWNSHIP3=NTOWNSHIP3, RADDR3=NRADDR3, RZONE3=NRZONE3, COCONTACT=NCOCONTACT, "_
	   &"COCONTACTTEL=NCOCONTACTTEL, COCONTACTTELEXT=NCOCONTACTTELEXT, COMOBILE=NCOMOBILE, COFAX=NCOFAX, COEMAIL=NCOEMAIL, "_
	   &"COBOSS=NCOBOSS, COBOSSSOCIAL=NCOBOSSSOCIAL, TRADETYPE=NTRADETYPE, AREAID=NAREAID, GROUPID=NGROUPID, SALESID=NSALESID, "_
	   &"CASETYPE=NCASETYPE, FREECODE=NFREECODE, AGENTNAME=NAGENTNAME, AGENTSOCIAL=NAGENTSOCIAL, AGENTTEL=NAGENTTEL, "_
	   &"RCVD=NRCVD, APPLYDAT=NAPPLYDAT, FINISHDAT=NFINISHDAT, DOCKETDAT=NDOCKETDAT, DROPDAT=NDROPDAT, CANCELDAT=NCANCELDAT, "_
	   &"CANCELUSR=NCANCELUSR, OVERDUE=NOVERDUE, MEMO=NMEMO, NCICCUSNO=NNCICCUSNO, CUSTIP1=NCUSTIP1, CUSTIP2=NCUSTIP2, "_
	   &"CUSTIP3=NCUSTIP3, CUSTIP4=NCUSTIP4, CUSTIPEND=NCUSTIPEND, MACNO=NMACNO, VOIPTEL=NVOIPTEL, VOIPTELSTR=NVOIPTELSTR, "_
	   &"VOIPTELEND=NVOIPTELEND, ISPTYPE=NISPTYPE, ISPETC=NISPETC, CIRCUITTYPE=NCIRCUITTYPE, CIRCUITETC=NCIRCUITETC, "_
	   &"LINKTYPE=NLINKTYPE, LINKETC=NLINKETC, LINERATE=NLINERATE, LINESHARE=NLINESHARE, LINETEL=NLINETEL, CREDITTYPE=NCREDITTYPE, "_
	   &"CREDITBANK=NCREDITBANK, CREDITNO=NCREDITNO, CREDITNAME=NCREDITNAME, VALIDMONTH=NVALIDMONTH, VALIDYEAR=NVALIDYEAR, "_
	   &"CONSIGNEE=NCONSIGNEE, DEVELOPERID=NDEVELOPERID, WRKNO1=NWRKNO1, WRKNO2=NWRKNO2, WRKRCVDAT=NWRKRCVDAT, "_
	   &"WRKSETDAT=NWRKSETDAT, CONSIGNEESALE=NCONSIGNEESALE, UUSR= '" &V(0)& "',UDAT ='" &datevalue(now())& "' "_
	   &"from	RTSparqVoIPCust a "_
	   &"inner join RTSparqVoIPCustChg b on a.CUSID =b.CUSID "_
	   &"WHERE	b.cusid='" & dspkey(0) & "' and entryno=" &dspkey(1)
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