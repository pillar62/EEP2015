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
					'rsc.open "select max(entryno) AS entryno from RTSparq0809CustChg where cusid='" & dspkey(0) & "' " ,conn
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
'                 case ucase("/webap/rtap/base/RTSparq0809/RTSparq0809CustChgD.asp")
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
    '      rsc.open "select max(entryno) AS ENTRYNO from RTSparq0809CustChg where cusid='" & dspkey(1) & "' " ,conn
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
  title="速博0809用戶資料異動"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"

  sqlFormatDB="SELECT CUSID, ENTRYNO, MODIFYCODE, MODIFYDESC, EUSR, EDAT, UUSR, UDAT, "_
			 &"OCUSNC, OSOCIALID, OBIRTHDAY, OMOBILE, OFAX1, OFAX12, OEMAIL, "_
			 &"OCUTID1, OTOWNSHIP1, ORADDR1, ORZONE1, OCUTID2, OTOWNSHIP2, "_
			 &"ORADDR2, ORZONE2, OAPPLYDAT, OCANCELDAT, ODROPDAT, OMEMO, "_
			 &"OSECONDIDTYPE, OSECONDNO, OCONSIGNEE, OAREAID, OSALESID, "_
			 &"OSETUPEMPLOYEE, OSECONDLINEDEVP, OITEMNO, OSENDDAT, "_
			 &"OREALRCVAMT, OATTCONSENT, OATTIDCOPY, OATTHEALTHINS, OATTETC, "_
			 &"OFIRSTIDTYPE, OPHONENUM, OBOSS, OSVITEM, NCUSNC, NSOCIALID, "_
			 &"NBIRTHDAY, NMOBILE, NFAX1, NFAX12, NEMAIL, NCUTID1, NTOWNSHIP1, "_
			 &"NRADDR1, NRZONE1, NCUTID2, NTOWNSHIP2, NRADDR2, NRZONE2, "_
			 &"NAPPLYDAT, NCANCELDAT, NDROPDAT, NMEMO, NSECONDIDTYPE, "_
			 &"NSECONDNO, NCONSIGNEE, NAREAID, NSALESID, NSETUPEMPLOYEE, "_
			 &"NSECONDLINEDEVP, NITEMNO, NSENDDAT, NREALRCVAMT, NATTCONSENT, "_
			 &"NATTIDCOPY, NATTHEALTHINS, NATTETC, NFIRSTIDTYPE, NPHONENUM, "_
			 &"NBOSS, NSVITEM "_
			 &"FROM RTSparq0809CustChg "_
			 &"WHERE CUSID='' "
			 
  sqlList    ="SELECT CUSID, ENTRYNO, MODIFYCODE, MODIFYDESC, EUSR, EDAT, UUSR, UDAT, "_
			 &"OCUSNC, OSOCIALID, OBIRTHDAY, OMOBILE, OFAX1, OFAX12, OEMAIL, "_
			 &"OCUTID1, OTOWNSHIP1, ORADDR1, ORZONE1, OCUTID2, OTOWNSHIP2, "_
			 &"ORADDR2, ORZONE2, OAPPLYDAT, OCANCELDAT, ODROPDAT, OMEMO, "_
			 &"OSECONDIDTYPE, OSECONDNO, OCONSIGNEE, OAREAID, OSALESID, "_
			 &"OSETUPEMPLOYEE, OSECONDLINEDEVP, OITEMNO, OSENDDAT, "_
			 &"OREALRCVAMT, OATTCONSENT, OATTIDCOPY, OATTHEALTHINS, OATTETC, "_
			 &"OFIRSTIDTYPE, OPHONENUM, OBOSS, OSVITEM, NCUSNC, NSOCIALID, "_
			 &"NBIRTHDAY, NMOBILE, NFAX1, NFAX12, NEMAIL, NCUTID1, NTOWNSHIP1, "_
			 &"NRADDR1, NRZONE1, NCUTID2, NTOWNSHIP2, NRADDR2, NRZONE2, "_
			 &"NAPPLYDAT, NCANCELDAT, NDROPDAT, NMEMO, NSECONDIDTYPE, "_
			 &"NSECONDNO, NCONSIGNEE, NAREAID, NSALESID, NSETUPEMPLOYEE, "_
			 &"NSECONDLINEDEVP, NITEMNO, NSENDDAT, NREALRCVAMT, NATTCONSENT, "_
			 &"NATTIDCOPY, NATTHEALTHINS, NATTETC, NFIRSTIDTYPE, NPHONENUM, "_
			 &"NBOSS, NSVITEM "_
			 &"FROM RTSparq0809CustChg "_
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
	
  IF LEN(TRIM(DSPKEY(37))) = 0 THEN DSPKEY(37)=""
  IF LEN(TRIM(DSPKEY(38))) = 0 THEN DSPKEY(38)=""
  IF LEN(TRIM(DSPKEY(39))) = 0 THEN DSPKEY(39)=""

  IF LEN(TRIM(DSPKEY(73))) = 0 THEN DSPKEY(73)=0	'實收金額
  IF LEN(TRIM(DSPKEY(74))) = 0 THEN DSPKEY(74)=""	'同意書
  IF LEN(TRIM(DSPKEY(75))) = 0 THEN DSPKEY(75)=""	'ID影本
  IF LEN(TRIM(DSPKEY(76))) = 0 THEN DSPKEY(76)=""	'健保卡影本
  IF LEN(TRIM(DSPKEY(79))) = 0 THEN DSPKEY(79)=0	'話機數

  If len(trim(dspkey(45)))=0 then
       formValid=False
       message="用戶名稱不可空白"    
  elseif len(trim(dspkey(46)))=0 then
       formValid=False
       message="申請用戶身分證號不可空白"   
  elseif len(trim(dspkey(47)))=0 then
       formValid=False
       message="申請用戶出生日期不可空白"     
  ELSEIf NOT ISDATE(dspkey(47)) then
       formValid=False
       message="出生日期格式錯誤，請用(西元年/月/日)"                   
  elseif len(trim(dspkey(48))) > 0 and len(trim(dspkey(48))) < 10 then
       formValid=False
       message="行動電話號碼長度必須10位"      
  elseif len(trim(dspkey(49))) > 0 and len(trim(dspkey(49))) < 2 then
       formValid=False
       message="用戶傳真電話區碼不可少於2位"      
  elseif len(trim(dspkey(50))) > 0 and len(trim(dspkey(50))) < 6 then
       formValid=False
       message="用戶傳真電話不可少於6位"     
  elseif len(trim(dspkey(51))) > 0 and len(trim(dspkey(51))) < 10 then
       formValid=False
       message="E-Mail不可少於10位"                    
  elseif len(trim(dspkey(52)))=0 then
       formValid=False
       message="申請用戶戶籍地址(縣市)不可空白"               
  elseif len(trim(dspkey(53)))=0 then
       formValid=False
       message="申請用戶戶籍地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(54)))=0 then
       formValid=False
       message="申請用戶戶籍地址不可空白"          
  elseif len(trim(dspkey(56)))=0 then
       formValid=False
       message="申請用戶帳寄地址(縣市)不可空白"               
  elseif len(trim(dspkey(57)))=0 then
       formValid=False
       message="申請用戶帳寄地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(58)))=0 then
       formValid=False
       message="申請用戶帳寄地址不可空白"          
  ELSEIf len(trim(dspkey(72)))=0 then
       formValid=False
       message="送件日期不可空白"                
  ELSEIf len(trim(dspkey(64)))=0 then
       formValid=False
       message="第二證照類別不可空白"          
  ELSEIf len(trim(dspkey(65)))=0 then
       formValid=False
       message="第二證照號碼不可空白"                          
  ELSEIf len(trim(dspkey(66)))=0 AND  len(trim(dspkey(68)))=0  AND len(trim(dspkey(70)))=0then
       formValid=False
       message="開發人員不可空白"                 
  ELSEIf (len(trim(dspkey(66)))<>0 AND  len(trim(dspkey(68)))<>0)  or (len(trim(dspkey(66)))<>0 AND len(trim(dspkey(70)))<>0 ) or (len(trim(dspkey(68)))<>0 and len(trim(dspkey(70)))<>0 )then
       formValid=False
       message="(經銷、業務、二線)開發人員不可同時輸入"
  ELSEIf len(trim(dspkey(2)))=0 then
       formValid=False
       message="異動類別不可空白"
  end if
  
  IF formValid=TRUE THEN
    IF dspkey(46) <> "" and len(dspkey(78))=0 then
       idno=dspkey(46)
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
   Sub SrDeveloperonclick()
       prog="RTGetDeveloperD.asp"
       prog=prog & "?KEY=" & document.all("KEY70").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key70").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub      
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
   Sub Srcounty12onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY52").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key53").value =  trim(Fusrid(0))
          document.all("key55").value =  trim(Fusrid(1))
       End if       
       end if
   End Sub       
   Sub Srcounty16onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY56").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key57").value =  trim(Fusrid(0))
          document.all("key59").value =  trim(Fusrid(1))
       End if       
       end if
   End Sub       

Sub SrAddrEqual1()
     document.All("key56").value=document.All("key52").value
     document.All("key57").value=document.All("key53").value
     document.All("key58").value=document.All("key54").value
     document.All("key59").value=document.All("key55").value
End Sub 
    
SUB SrSHOWTELLISTOnClick()
       IF window.SRTAR1.style.display="" THEN
          window.SRTAR1.style.display="none"
          document.all("STL").value="顯示電話明細"
       ELSE
          window.SRTAR1.style.display=""
          document.all("STL").value="隱藏電話明細"
       end if
 END SUB 
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
				rs.open "select max(entryno) AS entryno from RTSparq0809CustChg where cusid='" & dspkey(0) & "'",conn
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
                 <%=fieldRole(1)%> readonly size="4" value="<%=dspKey(1)%>" maxlength="4" class=dataListdata></td>
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
		sql="SELECT CUSID, CUSNC, SOCIALID, BIRTHDAY, MOBILE, FAX1, FAX12, EMAIL, CUTID1, "_
           &"TOWNSHIP1, RADDR1, RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, "_
           &"APPLYDAT, CANCELDAT, DROPDAT, MEMO, SECONDIDTYPE, SECONDNO, "_
           &"CONSIGNEE, AREAID, SALESID, SETUPEMPLOYEE, SECONDLINEDEVP, "_
           &"ITEMNO, SENDDAT, REALRCVAMT, ATTCONSENT, ATTIDCOPY, ATTHEALTHINS, "_
           &"ATTETC, FIRSTIDTYPE, PHONENUM, BOSS, SVITEM "_
           &"FROM RTSparq0809Cust "_
		   &"WHERE cusid ='" &dspKey(0) &"' "
		rs.Open sql, conn
	    For i = 1 To rs.Fields.Count-1
			  dspkey(i+7)=rs.Fields(i).Value
			  dspkey(i+44)=rs.Fields(i).Value			  
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
        <input type="text" name="key5" size="15" READONLY value="<%=dspKey(5)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text6"></td></tr>  

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
        <input type="text" name="key7" size="15" READONLY value="<%=dspKey(7)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text8"></td></tr>
	</table><br>


  <span id="tags1" class="dataListTagsOn"
        onClick="vbscript:tag1.style.display=''	   :tags1.classname='dataListTagsOn':
						  tag2.style.display='none':tags2.classname='dataListTagsOf'"><u>異動後資料</u> |</span>
  <span id="tags2" class="dataListTagsOf"
        onClick="vbscript:tag1.style.display='none':tags1.classname='dataListTagsOf':
                          tag2.style.display=''    :tags2.classname='dataListTagsOn'"><u>異動前資料</u></span>


<table width="100%" bgcolor="silver">
	<tr><td width="2%">&nbsp;</td>
		<td width="96%">&nbsp;</td>
		<td width="2%">&nbsp;</td></tr>
	<tr><td>&nbsp;</td><td>


<!-- 異動後資料 ========================================================================================================== -->
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
	<tr><td bgcolor="BDB76B" align="LEFT" colspan="6">基本資料</td></tr>

	<tr><td WIDTH=15% class=dataListHEAD>用戶名稱</td>
    	<td WIDTH=20% bgcolor="silver" >
        	<input type="text" name="key45" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" 
				maxlength="50" value="<%=dspKey(45)%>"  size="20" class=dataListENTRY ID="Text3"></td>
    	<td WIDTH=15% class=dataListHEAD>出生日</td>
    	<td WIDTH=20% bgcolor="silver" >
        	<input type="text" name="key47" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10" value="<%=dspKey(47)%>"   size="10" class=dataListENTRY >
        	<input type="button" id="B3" name="B3" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
       		<IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C3"  name="C3"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>
		<td class=dataListHEAD>企業負責人</td>
    	<td bgcolor="silver" >
        <input type="text" name="key80" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="20"
               value="<%=dspKey(80)%>"   size="30" class=dataListEntry></td></tr>

	<tr><td  class=dataListHEAD>行動電話</td>
    	<td  bgcolor="silver" >
        	<input type="text" name="key48" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10" value="<%=dspKey(48)%>"   size="10" class=dataListEntry></td>
		<td  class=dataListSEARCH>傳真電話</td>       
    	<td  bgcolor="silver"><font size=2>(</font>
    		<input type="text" name="key49" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
				style="text-align:left;" maxlength="3" value="<%=dspKey(49)%>" size="3" class=dataListEntry><font size=2>)</font>
			<input type="text" name="key50" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="8" value="<%=dspKey(50)%>"   size="10" class=dataListEntry></td>
		<td  class=dataListSEARCH>E-Mail</td>       
		<td  bgcolor="silver">
        	<input type="text" name="key51" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="50" value="<%=dspKey(51)%>" size="40" class=dataListEntry></td></tr>      

	<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='L3' " 
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='L3' AND CODE='" & dspkey(78) &"' " 
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(身份證字號)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(78) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
	%>
	<tr><td  class="dataListHead" height="25">第一證照別及號碼</td>
		<td  height="25" bgcolor="silver" colspan=5> 
			<select size="1" name="key78"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry"><%=s%></select>
        	<input type="text" name="key46" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10" value="<%=dspKey(46)%>"   size="12" class=dataListENTRY></td></tr>

	<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='L3' " 
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='L3' AND CODE='" & dspkey(64) &"' " 
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(第二證照別)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(64) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
	%>
    <tr><td  class="dataListHead" height="25">第二證照別及號碼</td>
        <td  height="25" bgcolor="silver" colspan=5> 
		<select size="1" name="key64"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select16"><%=s%></select>	
        <input type="text" name="key65" size="15" maxlength="12" value="<%=dspkey(65)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td></tr> 

	<tr><td class="dataListHead" height="25">申請書附件</td>
        <td  height="25" bgcolor="silver" colspan=5>
			<%   IF DSPKEY(74)="Y" THEN CHECK74=" CHECKED "%>
			<input type="checkbox" name="key74" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" 
               value="Y"  <%=CHECK74%> READONLY  bgcolor="silver"  ID="Checkbox1"><font size=2>同意書　</font>
			<%   IF DSPKEY(75)="Y" THEN CHECK75=" CHECKED "%>
			<input type="checkbox" name="key75" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" 
               value="Y"  <%=CHECK75%> READONLY  bgcolor="silver"  ID="Checkbox2"><font size=2>ID影本　</font>
			<%   IF DSPKEY(76)="Y" THEN CHECK76=" CHECKED "%>
			<input type="checkbox" name="key76" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" 
               value="Y"  <%=CHECK76%> READONLY  bgcolor="silver"  ID="Checkbox3"><font size=2>健保卡影本　　其他</font>
			<input type="text" name="key77" size="30" maxlength="30" value="<%=dspkey(77)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"</td></tr>

	<%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(52))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX12=" onclick=""Srcounty12onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(52) & "' " 
       SXX12=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(52) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
	<tr><td class=dataListHEAD>戶籍地址</td>
    	<td bgcolor="silver" COLSPAN=5>
			<select size="1" name="key52" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select2"><%=s%></select>
			<input type="text" name="key53" readonly  size="8" value="<%=dspkey(53)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text4"><font SIZE=2>(鄉鎮)
			<input type="button" id="B53" <%=fieldpb%> name="B53" width="100%" style="Z-INDEX: 1"  value="...." <%=SXX12%>  >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C53"  name="C53"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >
	        <input type="text" name="key54" <%=fieldpa%> size="60" value="<%=dspkey(54)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text5"><font size=2>
         <input type="text" name="key55"  readonly size="5" value="<%=dspkey(55)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA"></td></tr>

	<%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(56))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX16=" onclick=""Srcounty16onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(56) & "' " 
       SXX16=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(56) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
	%>
	<tr><td class=dataListHEAD>帳寄地址<br>
			<input type="radio" name="rd1"  <%=fieldpb%> onClick="SrAddrEqual1()" ID="Radio1" VALUE="Radio1">同戶籍</td>
		<td bgcolor="silver" COLSPAN=5>
			<select size="1" name="key56" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select1"><%=s%></select>
			<input type="text" name="key57" readonly  size="8" value="<%=dspkey(57)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text14"><font SIZE=2>(鄉鎮)
			<input type="button" id="B57"  <%=fieldpb%>  name="B57"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX16%>  >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%>  alt="清除" id="C57"  name="C57"   style="Z-INDEX: 1" onclick="SrClear" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >
			<input type="text" name="key58" <%=fieldpa%> size="60" value="<%=dspkey(58)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><font size=2>
			<input type="text" name="key59"  readonly size="5" value="<%=dspkey(59)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA"></td></tr>

	<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="select code, codenc from RTCode where kind ='O2' " 
    Else
       sql="select code, codenc from RTCode where kind ='O2' AND code='" & dspkey(71) &"' " 
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(用戶設備)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("code")=dspkey(71) Then sx=" selected "
       s=s &"<option value=""" &rs("code") &"""" &sx &">" &rs("codenc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
	%>
	<tr><td  class="dataListHead" height="25">用戶設備</td>
        <td  height="25" bgcolor="silver"> 
			<select size="1" name="key71"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select4"><%=s%></select></td>

		<% 
		aryOption=Array("","ISR-Like D","ISR-Like D4")
		s=""
		If Len(Trim(fieldRole(1) &dataProtect)) < 1 Then
			For i = 0 To Ubound(aryOption)
				If dspKey(81)=aryOption(i) Then
					sx=" selected "
				Else
					sx=""
				End If
				s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
			Next
		Else
			s="<option value=""" &dspKey(81) &""">" &dspKey(81) &"</option>"
		End If
		%>
		<td class="dataListHead" height="32">申請服務項目</td>
        <td  height="32" bgcolor="silver" colspan=3><select size="1" name="key81" <%=fieldpg%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry"><%=s%></select></td></tr>


    <tr><td bgcolor="BDB76B" align="LEFT" colspan=6>申請服務明細</td></tr>
    <tr><td WIDTH=10% ALIGN="center" class=dataListDATA>項次</td>
		<td WIDTH=15% ALIGN="center" class=dataListDATA >區域號碼</td>
		<td WIDTH=15% ALIGN="center" class=dataListDATA>電話號碼</td>
		<td WIDTH=10% ALIGN="center" class=dataListDATA>項次</td>
		<td WIDTH=15% ALIGN="center" class=dataListDATA >區域號碼</td>
		<td WIDTH=15% ALIGN="center" class=dataListDATA>電話號碼</td></tr>
	<%
    IF (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  THEN
       BTNENABLE=""
    ELSE
       BTNENABLE=" DISABLED "
    END IF
    %>
    <%
     s=""
     sql="SELECT  *  FROM  RTSparq0809custTEL where cusid='" & dspkey(0) & "' and canceldat is null ORDER BY SEQ "
     rs.Open sql,conn
     cnt=0
     Do While Not rs.Eof
       CNT=CNT+1
       K=CNT MOD 2
       IF K=1 THEN
          RESPONSE.Write "<TR>"
       END IF
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & CNT &  "&nbsp;</FONT></td>"
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("TEL11")   & "&nbsp;</FONT></td>"
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("TEL12") & "&nbsp;</FONT></td>"
       IF K=0 THEN
          RESPONSE.Write "</TR>"
       END IF
       rs.MoveNext
     Loop
     rs.Close
    %>


	<tr><td bgcolor="BDB76B" align="LEFT"  colspan=6>用戶申請及施工進度狀態</td></tr>
	<tr><td class="dataListHEAD" height="23" width="10%">送件日期</td>
        <td height="23" bgcolor="silver" width="20%">
			<input type="text" name="key72" size="10" READONLY value="<%=dspKey(72)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text40">
			<input type="button" id="B72" name="B72" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C72" name="C72" style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>

		<td class="dataListHEAD" height="23" width="10%">申請日期</td>
        <td height="23" bgcolor="silver" width="20%">
			<input type="text" name="key60" size="10" READONLY value="<%=dspKey(60)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListENTRY">
			<input  type="button" id="B60" name="B60" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C60"  name="C60"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>

        <td class="dataListHEAD" height="23" width="10%">退租日期</td>
        <td height="23" bgcolor="silver"width="20%">
			<input type="text" name="key62" size="10" READONLY value="<%=dspKey(62)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListENTRY" ID="Text41">
			<input  type="button" id="B62" name="B62" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C62"  name="C62"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td></tr>

	<tr><td class="dataListHEAD" height="23" width="10%">作廢日期</td>
        <td  height="23" bgcolor="silver" colspan=5 width="20%">
        <input type="text" name="key61" size="10" READONLY value="<%=dspKey(61)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA"></td></tr>


	<tr><td bgcolor="BDB76B" align="LEFT" colspan=6>績效歸屬</td></tr>

	<%
	If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1   Then 
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, rtrim(RTObj.CUSID) as CUSID, RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
          &"WHERE (RTObjLink.CUSTYID = '02')  "
       s="<option value="""" >(經銷商)</option>"
    Else
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, rtrim(RTObj.CUSID) as CUSID, RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
          &"WHERE (RTObjLink.CUSTYID = '02')  and rtobj.cusid='" & dspkey(66) & "' "
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(經銷商)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(66) Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
    %>
    <tr><td  class="dataListHEAD" height="23" width="10%">開發經銷商</td>
        <td  height="23" bgcolor="silver" width="20%">
			<select size="1" name="key66" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry"><%=s%></select></td>

		<%
			If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  Then 
				sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '3') "
       			s="<option value="""" >(業務轄區)</option>"
    		Else
       			sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '3') AND AREAID='" & DSPKEY(67) & "' "
       			s="<option value="""" >(業務轄區)</option>"
    		End If
    		rs.Open sql,conn
    		If rs.Eof Then s="<option value="""" >(業務轄區)</option>"
    		sx=""
    		Do While Not rs.Eof
       			If rs("areaid")=dspkey(67) Then sx=" selected "
       			s=s &"<option value=""" &rs("areaid") &"""" &sx &">" &rs("areanc") &"</option>"
       			rs.MoveNext
       			sx=""
    		Loop
    		rs.Close
    	%>
		<td  class="dataListHEAD" height="23" width="10%">業務轄區</td>                                 
        <td  height="23" bgcolor="silver" width="20%" >
			<select size="1" name="key67" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry"><%=s%></select></td>

		<%
			If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1   Then 
       			sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
          		&"WHERE (RTEmployee.DEPT IN ('B100', 'B106', 'B107', 'B109', 'B200', 'B300', 'B401','B600', 'B700', 'C100')) AND (RTEmployee.TRAN2 <> '10')  " _
          		&" ORDER BY  RTObj.CUSNC "
       			s="<option value="""" >(開發業務)</option>"
    		Else
       			sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
          		&"WHERE RTEmployee.EMPLY='" & DSPKEY(68) & "' "
       			s="<option value="""" >(開發業務)</option>"
    		End If
    		rs.Open sql,conn
    		If rs.Eof Then s="<option value="""" >(開發業務)</option>"
    		sx=""
    		Do While Not rs.Eof
       			If rs("EMPLY")=dspkey(68) Then sx=" selected "
       			s=s &"<option value=""" &rs("EMPLY") &"""" &sx &">" &rs("CUSNC") &"</option>"
       			rs.MoveNext
       			sx=""
    		Loop
    		rs.Close
    	%>    
        <td  class="dataListHEAD" height="23" width="10%">業務人員</td>
        <td  height="23" bgcolor="silver"  width="20%">
			<select size="1" name="key68" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry"><%=s%></select></td></tr>

	<%  
       	If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1   Then 
			sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
         	&"WHERE (RTEmployee.DEPT IN ('B100', 'B106', 'B107', 'B109', 'B200', 'B300', 'B401','B600', 'B700', 'B701','C100')) AND (RTEmployee.TRAN2 <> '10')  " _
         	&" ORDER BY  RTObj.CUSNC "
       		s="<option value="""" >(裝機員工)</option>"
    	Else
    		sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
        	&"WHERE RTEmployee.EMPLY='" & DSPKEY(69) & "' "
       		s="<option value="""" >(裝機員工)</option>"
    	End If
    	rs.Open sql,conn
    	If rs.Eof Then s="<option value="""" >(裝機員工)</option>"
    	sx=""
    	Do While Not rs.Eof
       		If rs("EMPLY")=dspkey(69) Then sx=" selected "
       		s=s &"<option value=""" &rs("EMPLY") &"""" &sx &">" &rs("CUSNC") &"</option>"
       		rs.MoveNext
       		sx=""
   		Loop
    	rs.Close
    %>
    <tr><td  class="dataListHEAD" height="23" width="10%">裝機員工</td>
        <td  height="23" bgcolor="silver" width="20%">
			<select size="1" name="key69" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry"><%=s%></select></td>

		<%
			name=""
			if dspkey(70) <> "" then
				sqlxx=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			 	&"where rtemployee.emply='" & dspkey(70) & "' "
				rs.Open sqlxx,conn
				if rs.eof then
					name="(對象檔找不該員工)"
				else
					name=rs("cusnc")
				end if
				rs.close
			end if
		%>
        <td  class="dataListHEAD" height="23" width="10%">二線開發人員</td>                                 
        <td width="35%" width="20%" bgcolor="silver">
			<input type="text" name="key70"value="<%=dspKey(70)%>" <%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" size="8" maxlength="6" readonly class="dataListDATA">
			<input type="BUTTON" id="B70" name="B70" style="Z-INDEX: 1"  value="...." onclick="Srdeveloperonclick()">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  alt="清除" id="C70" name="C70" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">
			<font size=2><%=name%></font></td>
			
		<td  class=dataListHEAD>實收金額</td>
    	<td  bgcolor="silver" colspan=3>
        	<input type="text" name="key73" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10" value="<%=dspKey(73)%>"   size="10" class=dataListEntry></td></tr>
	
	<tr><td class=dataListHEAD>話機數</td>
    	<td bgcolor="silver" colspan=5>
        	<input type="text" name="key79" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="2" value="<%=dspKey(79)%>"   size="10" class=dataListEntry></td></tr>


	<tr><td bgcolor="BDB76B" align="LEFT" colspan=6>說明</td></tr>
	    <tr><td align="CENTER" colspan=6>
     		<TEXTAREA  cols="100%"  name="key63" rows=8  MAXLENGTH=500  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(63)%>" ID="Textarea1"><%=dspkey(63)%></TEXTAREA></td></tr>
</table>


<!-- 異動前資料 ========================================================================================================== -->
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag2" style="display: none">
	<tr><td bgcolor="BDB76B" align="LEFT" colspan="6">基本資料</td></tr>

	<tr><td WIDTH=15% class=dataListHEAD>用戶名稱</td>
    	<td WIDTH=20% bgcolor="silver" >
        	<input type="text" name="key8" <%=fieldlock%> style="text-align:left;" 
				maxlength="50" value="<%=dspKey(8)%>"  size="20" class=dataListENTRY></td>
    	<td WIDTH=15% class=dataListHEAD>出生日</td>
    	<td WIDTH=20% bgcolor="silver" >
        	<input type="text" name="key10" <%=fieldlock%>
               style="text-align:left;" maxlength="10" value="<%=dspKey(10)%>"   size="10" class=dataListENTRY ></td>
		<td class=dataListHEAD>企業負責人</td>
    	<td bgcolor="silver" >
        <input type="text" name="key43" <%=fieldlock%>
               style="text-align:left;" maxlength="20"
               value="<%=dspKey(43)%>" size="30" class=dataListEntry></td></tr>

	<tr><td  class=dataListHEAD>行動電話</td>
    	<td  bgcolor="silver" >
        	<input type="text" name="key11" <%=fieldlock%>
               style="text-align:left;" maxlength="10" value="<%=dspKey(11)%>"   size="10" class=dataListEntry></td>
		<td  class=dataListSEARCH>傳真電話</td>       
    	<td  bgcolor="silver"><font size=2>(</font>
    		<input type="text" name="key12" <%=fieldlock%>
				style="text-align:left;" maxlength="3" value="<%=dspKey(12)%>" size="3" class=dataListEntry><font size=2>)</font>
			<input type="text" name="key13" <%=fieldlock%>
               style="text-align:left;" maxlength="8" value="<%=dspKey(13)%>" size="10" class=dataListEntry></td>
		<td  class=dataListSEARCH>E-Mail</td>       
		<td  bgcolor="silver">
        	<input type="text" name="key14" <%=fieldlock%>
               style="text-align:left;" maxlength="50" value="<%=dspKey(14)%>" size="40" class=dataListEntry></td></tr>

	<%
    	s=""
    	sx=" selected "
    	'If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       	'	sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='L3' " 
    	'Else
       		sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='L3' AND CODE='" & dspkey(41) &"' " 
    	'End If
	    rs.Open sql,conn
	    s=""
    	s=s &"<option value=""" &"""" &sx &">(身份證字號)</option>"
    	sx=""
    	Do While Not rs.Eof
	    	s=""    	
			If rs("CODE")=dspkey(41) Then sx=" selected "
       		s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       		rs.MoveNext
       		sx=""
    	Loop
    	rs.Close
	%>
	<tr><td  class="dataListHead" height="25">第一證照別及號碼</td>
		<td  height="25" bgcolor="silver" colspan=5> 
			<select size="1" name="key41" <%=fieldlock%> size="1" class="dataListEntry"><%=s%></select>
        	<input type="text" name="key9" <%=fieldlock%>
               style="text-align:left;" maxlength="10" value="<%=dspKey(9)%>" size="12" class=dataListENTRY></td></tr>

	<%
    s=""
    sx=" selected "
    'If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
    '    sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='L3' " 
    'Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='L3' AND CODE='" & dspkey(27) &"' " 
    'End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(第二證照別)</option>"
    sx=""
    Do While Not rs.Eof
	    s=""
       If rs("CODE")=dspkey(27) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
	%>
    <tr><td  class="dataListHead" height="25">第二證照別及號碼</td>
        <td  height="25" bgcolor="silver" colspan=5> 
		<select size="1" name="key27" <%=fieldlock%> size="1" class="dataListEntry"><%=s%></select>	
        <input type="text" name="key28" size="15" maxlength="12" value="<%=dspkey(28)%>" <%=fieldlock%> class="dataListEntry"></td></tr> 

	<tr><td class="dataListHead" height="25">申請書附件</td>
        <td  height="25" bgcolor="silver" colspan=5>
        	<input type="text" name="key37" <%=fieldlock%>
               style="text-align:left;" maxlength="1" value="<%=dspKey(37)%>"   size="1" class=dataListEntry>
               <font size=2>同意書　　</font>
        	<input type="text" name="key38" <%=fieldlock%>
               style="text-align:left;" maxlength="1" value="<%=dspKey(38)%>"   size="1" class=dataListEntry>
               <font size=2>ID影本　　</font>
        	<input type="text" name="key39" <%=fieldlock%>
               style="text-align:left;" maxlength="1" value="<%=dspKey(39)%>"   size="1" class=dataListEntry>
               <font size=2>健保卡影本　　其他</font>
			<input type="text" name="key40" size="30" maxlength="30" value="<%=dspkey(40)%>" <%=fieldlock%> class="dataListEntry"</td></tr>

	<%
		s=""
    	sx=" selected "
    	'If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
       	'	sql="SELECT Cutid,Cutnc FROM RTCounty " 
       	'	If len(trim(dspkey(15))) < 1 Then
        '  		sx=" selected " 
       	'	else
        '  		sx=""
       	'	end if     
       	'	s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       	'	SXX12=" onclick=""Srcounty12onclick()""  "
    	'Else
       		sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(15) & "' " 
       	'	SXX12=""
	    'End If
    	sx=""    
    	rs.Open sql,conn
    	Do While Not rs.Eof
       		If rs("cutid")=dspkey(15) Then sx=" selected "
       		s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       		rs.MoveNext
       		sx=""
    	Loop
    	rs.Close
   %>
	<tr><td class=dataListHEAD>戶籍地址</td>
    	<td bgcolor="silver" COLSPAN=5>
			<select size="1" name="key15" <%=fieldlock%> size="1" class="dataListEntry"><%=s%></select>
			<input type="text" name="key16" <%=fieldlock%> size="8" value="<%=dspkey(16)%>" maxlength="10" class="dataListDATA"><font SIZE=2>(鄉鎮)
	        <input type="text" name="key17" <%=fieldlock%> size="60" value="<%=dspkey(17)%>" maxlength="60" class="dataListEntry"><font size=2>
         <input type="text" name="key18" <%=fieldlock%> size="5" value="<%=dspkey(18)%>" maxlength="5" class="dataListDATA"></td></tr>

	<%
		s=""
    	sx=" selected "
    	'If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
       	'	sql="SELECT Cutid,Cutnc FROM RTCounty " 
       	'	If len(trim(dspkey(19))) < 1 Then
        '  		sx=" selected " 
       	'	else
        '  		sx=""
       	'	end if
       	'	s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       	'	SXX16=" onclick=""Srcounty16onclick()""  "
    	'Else
       		sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(19) & "' " 
       	'	SXX16=""
    	'End If
    	sx=""    
    	rs.Open sql,conn
    	Do While Not rs.Eof
       		If rs("cutid")=dspkey(19) Then sx=" selected "
       		s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       		rs.MoveNext
       		sx=""
    	Loop
    	rs.Close
	%>
	<tr><td class=dataListHEAD>帳寄地址</td>
		<td bgcolor="silver" COLSPAN=5>
			<select size="1" name="key19" <%=fieldlock%> size="1" class="dataListEntry" ><%=s%></select>
			<input type="text" name="key20" <%=fieldlock%>  size="8" value="<%=dspkey(20)%>" maxlength="10" class="dataListDATA" ><font SIZE=2>(鄉鎮)
			<input type="text" name="key21" <%=fieldlock%> size="60" value="<%=dspkey(21)%>" maxlength="60" class="dataListEntry"><font size=2>
			<input type="text" name="key22"  <%=fieldlock%> size="5" value="<%=dspkey(22)%>" maxlength="5" class="dataListDATA"></td></tr>

	<%
    s=""
    sx=" selected "
    'If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
    '   sql="select code, codenc from RTCode where kind ='N8' "
    'Else
       sql="select code, codenc from RTCode where kind ='N8' AND code='" & dspkey(34) &"' " 
    'End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(用戶設備)</option>"
    sx=""
    Do While Not rs.Eof
		s=""
       If rs("code")=dspkey(34) Then sx=" selected "
       s=s &"<option value=""" &rs("code") &"""" &sx &">" &rs("codenc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
	%>
	<tr><td  class="dataListHead" height="25">用戶設備</td>
        <td  height="25" bgcolor="silver"> 
			<select size="1" name="key34" <%=fieldlock%> class="dataListEntry"><%=s%></select></td>

		<td class="dataListHead" height="32">申請服務項目</td>
        <td  height="32" bgcolor="silver" colspan=3>
			<input type="text" name="key44" <%=fieldlock%>
			style="text-align:left;" value="<%=dspKey(44)%>" size="10" class=dataListEntry></td></tr>


    <tr><td bgcolor="BDB76B" align="LEFT" colspan=6>申請服務明細</td></tr>
    <tr><td WIDTH=10% ALIGN="center" class=dataListDATA>項次</td>
		<td WIDTH=15% ALIGN="center" class=dataListDATA >區域號碼</td>
		<td WIDTH=15% ALIGN="center" class=dataListDATA>電話號碼</td>
		<td WIDTH=10% ALIGN="center" class=dataListDATA>項次</td>
		<td WIDTH=15% ALIGN="center" class=dataListDATA >區域號碼</td>
		<td WIDTH=15% ALIGN="center" class=dataListDATA>電話號碼</td></tr>
	<%
    IF (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  THEN
       BTNENABLE=""
    ELSE
       BTNENABLE=" DISABLED "
    END IF
    %>
    <%
     s=""
     sql="SELECT  *  FROM  RTSparq0809custTEL where cusid='" & dspkey(0) & "' and canceldat is null ORDER BY SEQ "
     rs.Open sql,conn
     cnt=0
     Do While Not rs.Eof
       CNT=CNT+1
       K=CNT MOD 2
       IF K=1 THEN
          RESPONSE.Write "<TR>"
       END IF
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & CNT &  "&nbsp;</FONT></td>"
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("TEL11")   & "&nbsp;</FONT></td>"
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("TEL12") & "&nbsp;</FONT></td>"
       IF K=0 THEN
          RESPONSE.Write "</TR>"
       END IF
       rs.MoveNext
     Loop
     rs.Close
    %>


	<tr><td bgcolor="BDB76B" align="LEFT" colspan=6>用戶申請及施工進度狀態</td></tr>
	<tr><td class="dataListHEAD" height="23" width="10%">送件日期</td>
        <td height="23" bgcolor="silver" width="20%">
			<input type="text" name="key35" size="10" <%=fieldlock%> value="<%=dspKey(35)%>" class="dataListDATA"></td>
		<td class="dataListHEAD" height="23" width="10%">申請日期</td>
        <td height="23" bgcolor="silver" width="20%">
			<input type="text" name="key23" size="10" <%=fieldlock%> value="<%=dspKey(23)%>" class="dataListENTRY"></td>
        <td class="dataListHEAD" height="23" width="10%">退租日期</td>
        <td height="23" bgcolor="silver" width="20%">
			<input type="text" name="key25" size="10" <%=fieldlock%> value="<%=dspKey(25)%>" class="dataListENTRY"></td></tr>

	<tr><td class="dataListHEAD" height="23" width="10%">作廢日期</td>
        <td  height="23" bgcolor="silver" colspan=5 width="20%">
        <input type="text" name="key24" size="10" <%=fieldlock%> value="<%=dspKey(24)%>" class="dataListDATA"></td></tr>


	<tr><td bgcolor="BDB76B" align="LEFT" colspan=6>績效歸屬</td></tr>

	<%
	'If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1   Then 
    '   sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, rtrim(RTObj.CUSID) as CUSID, RTObj.SHORTNC " _
    '      &"FROM RTObj INNER JOIN " _
    '      &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
    '      &"WHERE (RTObjLink.CUSTYID = '02')  "
    '   s="<option value="""" >(經銷商)</option>"
    'Else
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, rtrim(RTObj.CUSID) as CUSID, RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
          &"WHERE (RTObjLink.CUSTYID = '02')  and rtobj.cusid='" & dspkey(29) & "' "
    'End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(經銷商)</option>"
    sx=""
    Do While Not rs.Eof
		s=""
       If rs("CUSID")=dspkey(29) Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
    %>
    <tr><td  class="dataListHEAD" height="23" width="10%">開發經銷商</td>
        <td  height="23" bgcolor="silver" width="20%">
			<select size="1" name="key29" <%=fieldlock%> class="dataListEntry"><%=s%></select></td>

		<%
			'If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  Then 
			'	sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '3') "
       		'	s="<option value="""" >(業務轄區)</option>"
    		'Else
       			sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '3') AND AREAID='" & DSPKEY(30) & "' "
       			s="<option value="""" >(業務轄區)</option>"
    		'End If
    		rs.Open sql,conn
    		If rs.Eof Then s="<option value="""" >(業務轄區)</option>"
    		sx=""
    		Do While Not rs.Eof
				s=""
       			If rs("areaid")=dspkey(30) Then sx=" selected "
       			s=s &"<option value=""" &rs("areaid") &"""" &sx &">" &rs("areanc") &"</option>"
       			rs.MoveNext
       			sx=""
    		Loop
    		rs.Close
    	%>
		<td  class="dataListHEAD" height="23" width="10%">業務轄區</td>                                 
        <td  height="23" bgcolor="silver" width="20%" >
			<select size="1" name="key30" <%=fieldlock%> class="dataListEntry"><%=s%></select></td>

		<%
			'If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1   Then 
       		'	sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
          	'	&"WHERE (RTEmployee.DEPT IN ('B100', 'B106', 'B107', 'B200', 'B300', 'B600', 'B700', 'C100')) AND (RTEmployee.TRAN2 <> '10')  " _
          	'	&" ORDER BY  RTObj.CUSNC "
       		'	s="<option value="""" >(開發業務)</option>"
    		'Else
       			sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
          		&"WHERE RTEmployee.EMPLY='" & DSPKEY(31) & "' "
       			s="<option value="""" >(開發業務)</option>"
    		'End If
    		rs.Open sql,conn
    		If rs.Eof Then s="<option value="""" >(開發業務)</option>"
    		sx=""
    		Do While Not rs.Eof
    			s=""
       			If rs("EMPLY")=dspkey(31) Then sx=" selected "
       			s=s &"<option value=""" &rs("EMPLY") &"""" &sx &">" &rs("CUSNC") &"</option>"
       			rs.MoveNext
       			sx=""
    		Loop
    		rs.Close
    	%>    
        <td  class="dataListHEAD" height="23" width="10%">業務人員</td>
        <td  height="23" bgcolor="silver"  width="20%">
			<select size="1" name="key31" <%=fieldlock%> class="dataListEntry"><%=s%></select></td></tr>

	<%  
       	'If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1   Then 
		'	sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
        ' 	&"WHERE (RTEmployee.DEPT IN ('B100', 'B106', 'B107', 'B200', 'B300', 'B600', 'B700', 'B701', 'C100')) AND (RTEmployee.TRAN2 <> '10')  " _
        ' 	&" ORDER BY  RTObj.CUSNC "
       	'	s="<option value="""" >(裝機員工)</option>"
    	'Else
    		sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
        	&"WHERE RTEmployee.EMPLY='" & DSPKEY(32) & "' "
       		s="<option value="""" >(裝機員工)</option>"
    	'End If
    	rs.Open sql,conn
    	If rs.Eof Then s="<option value="""" >(裝機員工)</option>"
    	sx=""
    	Do While Not rs.Eof
    		s=""
       		If rs("EMPLY")=dspkey(32) Then sx=" selected "
       		s=s &"<option value=""" &rs("EMPLY") &"""" &sx &">" &rs("CUSNC") &"</option>"
       		rs.MoveNext
       		sx=""
   		Loop
    	rs.Close
    %>
    <tr><td  class="dataListHEAD" height="23" width="10%">裝機員工</td>
        <td  height="23" bgcolor="silver" width="20%">
			<select size="1" name="key32" <%=fieldlock%> class="dataListEntry"><%=s%></select></td>

		<%
			name=""
			if dspkey(33) <> "" then
				sqlxx=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			 	&"where rtemployee.emply='" & dspkey(33) & "' "
				rs.Open sqlxx,conn
				if rs.eof then
					name="(對象檔找不該員工)"
				else
					name=rs("cusnc")
				end if
				rs.close
			end if
		%>
        <td  class="dataListHEAD" height="23" width="10%">二線開發人員</td>                                 
        <td width="35%" width="20%">
			<input type="text" name="key33"value="<%=dspKey(33)%>" <%=fieldlock%> style="text-align:left;" size="8" maxlength="6" class="dataListDATA">
			<font size=2><%=name%></font></td>
			
		<td  class=dataListHEAD>實收金額</td>
    	<td  bgcolor="silver">
        	<input type="text" name="key36" <%=fieldlock%>
               style="text-align:left;" maxlength="10" value="<%=dspKey(36)%>" size="10" class=dataListEntry></td></tr>
	
	<tr><td class=dataListHEAD>話機數</td>
    	<td bgcolor="silver" colspan=5>
        	<input type="text" name="key42" <%=fieldlock%>
               style="text-align:left;" maxlength="2" value="<%=dspKey(42)%>" size="10" class=dataListEntry></td></tr>


	<tr><td bgcolor="BDB76B" align="LEFT" colspan=6>說明</td></tr>
	    <tr><td align="CENTER" colspan=6>
     		<TEXTAREA  cols="100%"  name="key26" rows=8  MAXLENGTH=500  class="dataListentry" <%=fieldlock%> value="<%=dspkey(26)%>"><%=dspkey(26)%></TEXTAREA></td></tr>
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
	sql="UPDATE	RTSparq0809Cust "_
	   &"SET CUSNC = NCUSNC, SOCIALID = NSOCIALID, "_
	   &"BIRTHDAY = NBIRTHDAY, MOBILE = NMOBILE, FAX1 = NFAX1, FAX12 = NFAX12, EMAIL = NEMAIL, CUTID1 = NCUTID1, TOWNSHIP1 = NTOWNSHIP1, "_
	   &"RADDR1 = NRADDR1, RZONE1 = NRZONE1, CUTID2 = NCUTID2, TOWNSHIP2 = NTOWNSHIP2, RADDR2 = NRADDR2, RZONE2 = NRZONE2, "_
	   &"APPLYDAT = NAPPLYDAT, CANCELDAT = NCANCELDAT, DROPDAT = NDROPDAT, MEMO = NMEMO, SECONDIDTYPE = NSECONDIDTYPE, "_
	   &"SECONDNO = NSECONDNO, CONSIGNEE = NCONSIGNEE, AREAID = NAREAID, SALESID = NSALESID, SETUPEMPLOYEE = NSETUPEMPLOYEE, "_
	   &"SECONDLINEDEVP = NSECONDLINEDEVP, ITEMNO = NITEMNO, SENDDAT = NSENDDAT, REALRCVAMT = NREALRCVAMT, ATTCONSENT = NATTCONSENT, "_
	   &"ATTIDCOPY = NATTIDCOPY, ATTHEALTHINS = NATTHEALTHINS, ATTETC = NATTETC, FIRSTIDTYPE = NFIRSTIDTYPE, PHONENUM = NPHONENUM, "_
	   &"BOSS = NBOSS, SVITEM = NSVITEM "_
	   &"from	RTSparq0809Cust a "_
	   &"inner join  RTSparq0809CustChg b on a.CUSID =b.CUSID "_
	   &"WHERE	b.cusid='" & dspkey(0) & "' and b.entryno=" &dspkey(1)
	'response.write "sql=" & sql
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