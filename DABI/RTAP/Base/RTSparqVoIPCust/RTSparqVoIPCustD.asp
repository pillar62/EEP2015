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
                   ' 當程式為ADSL社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
                   case ucase("/webap/rtap/base/RTSparqVoIPCust/RTSparqVoIPCustD.asp")
      'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       if i <> 0 then rs.Fields(i).Value=dspKey(i)    
                       if i=0 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         cusidxx="V" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
                         'cusidxx=trim(datePART("yyyy",NOW())-1911) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
                         rsc.open "select max(cusid) AS cusid from RTSparqVoIPCust where cusid like '" & cusidxx & "%' " ,conn
                         if len(rsc("cusid")) > 0 then
                            dspkey(0)=cusidxx & right("0000" & cstr(cint(right(rsc("cusid"),4)) + 1),3)
                         else
                            dspkey(0)=cusidxx & "001"
                         end if
                         rsc.close
                         rs.Fields(i).Value=dspKey(i) 
                       end if      
                   case else
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
                 case ucase("/webap/rtap/base/RTSparqVoIPCust/RTSparqVoIPCustD.asp")
     'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                     if i<>0 then rs.Fields(i).Value=dspKey(i)
                 case else
                     rs.Fields(i).Value=dspKey(i)
                     'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
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
       if ucase(runpgm)=ucase("/webap/rtap/base/RTSparqVoIPCust/RTSparqVoIPCustD.asp") then
          cusidxx="V" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
          rsc.open "select max(cusid) AS cusid from RTSparqVoIPCust where cusid like '" & cusidxx & "%' " ,conn
          if not rsC.eof then
            dspkey(0)=rsC("CUSID")
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
  numberOfKey=1
  title="速博VoIP用戶資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT CUSID, CUSNC, FIRSTIDTYPE, SOCIALID, SECONDIDTYPE, SECONDNO, "_
			 &"BIRTHDAY, EMAIL, CONTACTTEL, MOBILE, FAX, CUTID1, TOWNSHIP1, "_
			 &"RADDR1, RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, CUTID3, "_
			 &"TOWNSHIP3, RADDR3, RZONE3, COCONTACT, COCONTACTTEL, "_
			 &"COCONTACTTELEXT, COMOBILE, COFAX, COEMAIL, COBOSS, COBOSSSOCIAL, "_
			 &"TRADETYPE, EUSR, EDAT, UUSR, UDAT, AREAID, GROUPID, SALESID, "_
			 &"CASETYPE, FREECODE, AGENTNAME, AGENTSOCIAL, AGENTTEL, RCVD, "_
			 &"APPLYDAT, FINISHDAT, DOCKETDAT, TRANSDAT, DROPDAT, CANCELDAT, "_
			 &"CANCELUSR, OVERDUE, MEMO, NCICCUSNO, CUSTIP1, CUSTIP2, CUSTIP3, "_
			 &"CUSTIP4, CUSTIPEND, SPHNNO, MACNO, VOIPTEL, VOIPTELSTR, VOIPTELEND, "_
			 &"ISPTYPE, ISPETC, CIRCUITTYPE, CIRCUITETC, LINKTYPE, LINKETC, LINERATE, "_
			 &"LINESHARE, LINETEL, CREDITTYPE, CREDITBANK, CREDITNO, CREDITNAME, "_
			 &"VALIDMONTH, VALIDYEAR, CONSIGNEE, DEVELOPERID, WRKNO1, WRKNO2, WRKRCVDAT, WRKSETDAT, CONSIGNEESALE "_
			 &"FROM RTSparqVoIPCust WHERE CUSID='' "
  sqlList=	"SELECT CUSID, CUSNC, FIRSTIDTYPE, SOCIALID, SECONDIDTYPE, SECONDNO, "_
			 &"BIRTHDAY, EMAIL, CONTACTTEL, MOBILE, FAX, CUTID1, TOWNSHIP1, "_
			 &"RADDR1, RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, CUTID3, "_
			 &"TOWNSHIP3, RADDR3, RZONE3, COCONTACT, COCONTACTTEL, "_
			 &"COCONTACTTELEXT, COMOBILE, COFAX, COEMAIL, COBOSS, COBOSSSOCIAL, "_
			 &"TRADETYPE, EUSR, EDAT, UUSR, UDAT, AREAID, GROUPID, SALESID, "_
			 &"CASETYPE, FREECODE, AGENTNAME, AGENTSOCIAL, AGENTTEL, RCVD, "_
			 &"APPLYDAT, FINISHDAT, DOCKETDAT, TRANSDAT, DROPDAT, CANCELDAT, "_
			 &"CANCELUSR, OVERDUE, MEMO, NCICCUSNO, CUSTIP1, CUSTIP2, CUSTIP3, "_
			 &"CUSTIP4, CUSTIPEND, SPHNNO, MACNO, VOIPTEL, VOIPTELSTR, VOIPTELEND, "_
			 &"ISPTYPE, ISPETC, CIRCUITTYPE, CIRCUITETC, LINKTYPE, LINKETC, LINERATE, "_
			 &"LINESHARE, LINETEL, CREDITTYPE, CREDITBANK, CREDITNO, CREDITNAME, "_
			 &"VALIDMONTH, VALIDYEAR, CONSIGNEE, DEVELOPERID, WRKNO1, WRKNO2, WRKRCVDAT, WRKSETDAT, CONSIGNEESALE "_
			 &"FROM RTSparqVoIPCust WHERE "
  userDefineRead="Yes"      
  userDefineSave="Yes"       
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
	
  'if len(trim(dspkey(40)))=0 then dspkey(39)=""
  'if len(trim(dspkey(54)))=0 then dspkey(54)=0
  'if len(trim(dspkey(55)))=0 then dspkey(55)=0
  'if len(trim(dspkey(56)))=0 then dspkey(56)=0
  'if len(trim(dspkey(57)))=0 then dspkey(57)=0
  
  '身份證欄位第一碼,用以判別是個人還是公司,若為公司則出生日期必須空白,反之則不可空白
  LEADINGCHAR=LEFT(DSPKEY(3),1)
  IF LEADINGCHAR >="0" AND LEADINGCHAR <="9" THEN
     COMPANY="Y"
  ELSE
     COMPANT="N"
  END IF
  '身份證第一碼大寫
  DSPKEY(3)=UCASE(DSPKEY(3))

'  IF instr(1,dspkey(67),"-",1) <> 0 THEN
'  RESPONSE.Write "AAA=" & instr(1,dspkey(67),"-",1) & "<BR>"
'  RESPONSE.Write "BBB=" & instr(1,dspkey(69),"-",1) 
'  RESPONSE.END
'  ELSE
'  RESPOSNE.WRITE "XXX"
'  RESPONSE.End
'  END IF

  If len(trim(dspkey(44)))=0 or Not Isdate(dspkey(44)) then
       formValid=False
       message="收件日不可空白或格式錯誤"    
  elseif len(trim(dspkey(45)))=0 then
       formValid=False
       message="用戶申請日不可空白"   
  elseif len(trim(dspkey(1)))=0 then
       formValid=False
       message="用戶名稱不可空白"          
  '公關機時不檢查身份證
  elseif ( len(trim(dspkey(3)))=0 or (len(trim(dspkey(3)))<>10 and len(trim(dspkey(3)))<>8 ) ) AND DSPKEY(40) <> "Y" then
       formValid=False
       message="用戶身分證(統編)不可空白或長度不對"    
  
  elseif len(trim(dspkey(11)))=0 then
       formValid=False
       message="戶籍地址(縣市)不可空白"   
  elseif len(trim(dspkey(12)))=0 and dspkey(11)<>"06" and dspkey(11)<>"15" then
       formValid=False
       message="戶籍地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(13)))=0 then
       formValid=False
       message="戶籍地址(地址)不可空白"          

  elseif len(trim(dspkey(15)))=0 then
       formValid=False
       message="裝機地址(縣市)不可空白"   
  elseif len(trim(dspkey(16)))=0 and dspkey(15)<>"06" and dspkey(15)<>"15" then
       formValid=False
       message="裝機地址(鄉鎮)不可空白"
  elseif len(trim(dspkey(17)))=0 then
       formValid=False
       message="裝機地址(地址)不可空白"

  elseif len(trim(dspkey(19)))=0 then
       formValid=False
       message="帳單地址(縣市)不可空白"   
  elseif len(trim(dspkey(20)))=0 and dspkey(19)<>"06" and dspkey(19)<>"15" then
       formValid=False
       message="帳單地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(21)))=0 then
       formValid=False
       message="帳單地址(地址)不可空白"   

  elseif (len(trim(dspkey(6)))=0 or Not Isdate(dspkey(6))) AND COMPANY="N" then
       formValid=False
       message="用戶為個人時，出生日期不可空白或格式錯誤"   
  elseif len(trim(dspkey(6)))<>0  AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，出生日期必須空白"          
  elseif len(trim(dspkey(9)))=0 and len(trim(dspkey(8)))=0 then
       formValid=False
       message="用戶連絡電話及行動電話至少須輸入一項"   
  elseif instr(1,dspkey(9),"-",1) > 0 then
       formValid=False
       message="行動電話不可包含'-'符號"          
  elseif instr(1,dspkey(8),"-",1) > 0 then
       formValid=False
       message="連絡電話不可包含'-'符號"         
  elseif len(trim(dspkey(23)))=0  AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，企業連絡人不可空白"         
  elseif len(trim(dspkey(24)))=0  AND len(trim(dspkey(26)))=0 AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，企業連絡人連絡電話及行動電話至少需輸入一項"    
  elseif len(trim(dspkey(29)))=0  AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，企業負責人不可空白"
  elseif len(trim(dspkey(30)))=0  AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，企業負責人身份證字號不可空白"                     
  elseif len(trim(dspkey(39)))= 0 then
       formValid=False
       message="方案種類不可空白"
  elseif len(trim(dspkey(80)))=0 and len(trim(dspkey(38))) = 0 then
       message="社區檔之經銷商欄位與業務員不可同時空白!"
       formValid=False
  'elseif len(trim(dspkey(40)))= 0 AND DSPKEY(38) <> "Y" then
  '     formValid=False
  '     message="AVS繳款方式不可空白"      
  'elseif len(trim(dspkey(40)))> 0 AND DSPKEY(38) = "Y" then
  '     formValid=False
  '     message="公關機時，AVS繳款方式必須空白"           
  elseif len(trim(dspkey(47)))<> 0 AND len(trim(dspkey(46)))= 0 then
       formValid=False
       message="完工日期為空白時不可輸入報竣日"       
  'elseif len(trim(dspkey(46)))<> 0 AND ( len(trim(dspkey(55)))= 0 or len(trim(dspkey(56)))= 0 or len(trim(dspkey(57)))= 0 or len(trim(dspkey(58)))= 0 )then
  '     formValid=False
  '     message="輸入完工日期時，用戶IP不可空白"              
  end if
  IF formValid=TRUE THEN
    IF dspkey(3) <> "" and (dspkey(2)="01" or dspkey(2)="02") then
       idno=dspkey(3)
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
   if len(trim(dspkey(30)))<> 0 then
      idno=dspkey(30)
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
                V=split(rtnvalue,";")  
                DSpkey(32)=V(0)
        dspkey(33)=datevalue(now())
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
       prog=prog & "?KEY=" & document.all("KEY11").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key12").value =  trim(Fusrid(0))
          document.all("key14").value =  trim(Fusrid(1))
       End if       
       end if
   End Sub       
   Sub Srcounty16onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY15").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key16").value =  trim(Fusrid(0))
          document.all("key18").value =  trim(Fusrid(1))
       End if       
       end if
    END SUB
   Sub Srcounty20onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY19").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key20").value =  trim(Fusrid(0))
          document.all("key22").value =  trim(Fusrid(1))
       End if       
       end if
    END SUB    
  Sub Srsalesgrouponclick()
       prog="RTGetsalesgroupD.asp"
       prog=prog & "?KEY=" & document.all("KEY36").VALUE 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key37").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub        
   Sub Srsalesonclick()
       prog="RTGetsalesD.asp"
       prog=prog & "?KEY=" & document.all("KEY36").VALUE & ";" & document.all("KEY37").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key38").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub
   Sub SrDeveloperonclick()
       prog="RTGetDeveloperD.asp"
       prog=prog & "?KEY=" & document.all("KEY81").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key81").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub      
   
   Sub SrTAG0()
       'msgbox window.SRTAB1.style.display
       if window.SRTAB0.style.display="" then
          window.SRTAB0.style.display="none"
       elseif window.SRTAB0.style.display="none" then
          window.SRTAB0.style.display=""
       end if
   End Sub               
   Sub SrTAG1()
      ' msgbox window.SRTAB1.style.display
       if window.SRTAB1.style.display="" then
          window.SRTAB1.style.display="none"
       elseif window.SRTAB1.style.display="none" then
          window.SRTAB1.style.display=""
       end if
   End Sub        
   Sub SrTAG2()
      ' msgbox window.SRTAB1.style.display
       if window.SRTAB2.style.display="" then
          window.SRTAB2.style.display="none"
       elseif window.SRTAB2.style.display="none" then
          window.SRTAB2.style.display=""
       end if
   End Sub          
   Sub SrTAG3()
      ' msgbox window.SRTAB1.style.display
       if window.SRTAB3.style.display="" then
          window.SRTAB3.style.display="none"
       elseif window.SRTAB3.style.display="none" then
          window.SRTAB3.style.display=""
       end if
   End Sub         
   Sub SrTAG4()
      ' msgbox window.SRTAB1.style.display
       if window.SRTAB4.style.display="" then
          window.SRTAB4.style.display="none"
       elseif window.SRTAB4.style.display="none" then
          window.SRTAB4.style.display=""
       end if
   End Sub       
   Sub SrTAG5()
      ' msgbox window.SRTAB1.style.display
       if window.SRTAB5.style.display="" then
          window.SRTAB5.style.display="none"
       elseif window.SRTAB5.style.display="none" then
          window.SRTAB5.style.display=""
       end if
   End Sub                   
   Sub SrTAG6()
      ' msgbox window.SRTAB1.style.display
       if window.SRTAB6.style.display="" then
          window.SRTAB6.style.display="none"
       elseif window.SRTAB6.style.display="none" then
          window.SRTAB6.style.display=""
       end if
   End Sub                  
   Sub SrTAG7()
      ' msgbox window.SRTAB1.style.display
       if window.SRTAB7.style.display="" then
          window.SRTAB7.style.display="none"
       elseif window.SRTAB7.style.display="none" then
          window.SRTAB7.style.display=""
       end if
   End Sub                  
Sub SrAddrEqual1()
    document.All("key15").value=document.All("key11").value
    document.All("key16").value=document.All("key12").value
    document.All("key17").value=document.All("key13").value
    document.All("key18").value=document.All("key14").value
End Sub 
Sub SrAddrEqual2()
    document.All("key19").value=document.All("key11").value
    document.All("key20").value=document.All("key12").value
    document.All("key21").value=document.All("key13").value
    document.All("key22").value=document.All("key14").value
End Sub         
Sub SrAddrEqual3()
    document.All("key19").value=document.All("key15").value
    document.All("key20").value=document.All("key16").value
    document.All("key21").value=document.All("key17").value
    document.All("key22").value=document.All("key18").value
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
       <tr><td width="25%" class=dataListHead>建檔流水號</td>
           <td width="25%"  bgcolor="silver">
           <input type="text" name="key0"
                 <%=fieldRole(1)%> readonly size="15" value="<%=dspKey(0)%>" maxlength="15" class=dataListdata></td>
  </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(32))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(32)=V(0)
        End if  
       dspkey(33)=datevalue(now())
    else
        if len(trim(dspkey(34))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(34)=V(0)
        End if         
        dspkey(35)=datevalue(now())
    end if      
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
    '完工後基本資料PROTECT
    If len(trim(dspKey(46))) > 0 Then
       fieldPa=" class=""dataListData"" readonly "
       fieldpb=" disabled "
    Else
       fieldPa=""
       fieldpb=""
    End If
    '報竣日輸入後，寬頻服務+代理人+績效資料PROTECT
    If len(trim(dspKey(47))) > 0 Then
       fieldPC=" class=""dataListData"" readonly "
       fieldpD=" disabled "
    Else
       fieldPC=""
       fieldpD=""
    End If
    '報竣轉檔後，報竣日期PROTECT
    If len(trim(dspKey(48))) > 0 Then
       fieldPe=" class=""dataListData"" readonly "
       fieldpf=" disabled "
    Else
       fieldPe=""
       fieldpf=""
    End If    
      
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN%>
  <!--
  <span id="tags1" class="dataListTagsOn"
        onClick="vbscript:tag1.style.display=''    :tags1.classname='dataListTagsOn':
                          tag2.style.display='none':tags2.classname='dataListTagsOf'">基本資料</span>
  <span id="tags2" class="dataListTagsOf"
        onClick="vbscript:tag1.style.display='none':tags1.classname='dataListTagsOf':
                          tag2.style.display=''    :tags2.classname='dataListTagsOn'">發包安裝</span>           
  -->
<span id="tags1" class="dataListTagsOn">速博VoIP用戶資訊</span>
                                                            
<div class=dataListTagOn> 
	<table width="100%"><tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
		<tr><td>&nbsp;</td><td>

<DIV ID="SRTAG0" onclick="srtag0" style="cursor:hand">
	<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
	<tr><td bgcolor="BDB76B" align="LEFT">基本資料</td></tr></table></DIV>
    
<DIV ID=SRTAB0>
	<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
	<tr><td class="dataListSEARCH" height="23">用戶號碼</td>                                 
        <td height="23" bgcolor="silver" COLSPAN=3>
        <input type="text" name="key54" size="15" maxlength="10" <%=fieldpa%> value="<%=dspKey(54)%>" <%=fieldRole(1)%><%=dataProtect%> class="dataListENTRY">
        <FONT SIZE=2>-</FONT><input type="text" name="key60" size="3"  maxlength="3" <%=fieldpa%> value="<%=dspKey(60)%>" <%=fieldRole(1)%> class="dataListDATA" READONLY> 
        </TD></TR>

	<tr><td width="15%" class=dataListHEAD>收件日</td>
		<td width="35%" bgcolor="silver" >
        <input type="text" name="key44" value="<%=dspKey(44)%>" <%=fieldpa%> <%=fieldRole(1)%> <%=dataProtect%> style="text-align:left;" maxlength="10" READONLY size="10" class=dataListEntry>
		<input type="button" name="B44" id="B44" <%=fieldpb%> height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
		<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" name="C44" id="C44" <%=fieldpb%> alt="清除"  style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>

		<td width="15%" class=dataListHEAD>用戶申請日</td>
		<td width="35%" bgcolor="silver">
        <input type="text" name="key45" value="<%=dspKey(45)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" maxlength="10" READONLY size="10" class=dataListEntry>
		<input  type="button" id="B45" name="B45" <%=fieldpb%> height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
		<IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" id="C45" name="C45" <%=fieldpb%> alt="清除" style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td></tr>
		
	<tr><td width="15%" class=dataListHEAD>用戶(公司)名稱</td>
		<td width="35%" bgcolor="silver">
        <input type="text" name="key1" value="<%=dspKey(1)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" maxlength="30" size="30" class=dataListENTRY></td>

		<td class="dataListHEAD" height="23">出生日期</td>                                 
		<td height="23" bgcolor="silver">
        <input type="text" name="key6" size="10" value="<%=dspKey(6)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListentry" ID="Text1">  
        <input type="button" id="B6"  <%=fieldpb%>  name="B6" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C6" name="C6" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td></tr>

<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='J5' " 
       If len(trim(dspkey(2))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='J5' AND CODE='" & dspkey(2) &"' " 
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(第一證照別)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(2) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td width="15%" class=dataListHEAD>身分證(統編)</td>
		<td width="35%" bgcolor="silver">
		<select size="1" name="key2"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><%=s%></select>    
		<input type="text" name="key3" value="<%=dspKey(3)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
			style="text-align:left;" maxlength="15" size="15" class=dataListENTRY></td>

<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='L3' " 
       If len(trim(dspkey(4))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='L3' AND CODE='" & dspkey(4) &"' " 
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(第二證照別)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(4) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
		<td width="10%" class="dataListHead" height="25">第二證照別及號碼</td>
        <td width="18%" height="25" bgcolor="silver">
		<select size="1" name="key4"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><%=s%></select>	
        <input type="text" name="key5" value="<%=dspkey(5)%>" size="15" maxlength="15" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td></tr>

	<TR><td class="dataListHEAD" height="23">連絡電話</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key8" size="20" maxlength="20" <%=fieldpa%> value="<%=dspKey(8)%>" <%=fieldRole(1)%> class="dataListEntry"></td>

        <td class="dataListHEAD" height="23">行動電話</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key9" size="30" maxlength="30" <%=fieldpa%> value="<%=dspKey(9)%>" <%=fieldRole(1)%> class="dataListEntry"</td></tr>

	<tr><td class="dataListHEAD" height="23">傳真</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key10" size="30" maxlength="30" <%=fieldpa%> value="<%=dspKey(10)%>" <%=fieldRole(1)%> class="dataListEntry"</td>

		<td class="dataListHEAD" height="23">連絡EMAIL</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key7" size="50" maxlength="50" <%=fieldpa%> value="<%=dspKey(7)%>" <%=fieldRole(1)%> class="dataListEntry"></td></tr>

<%
	s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND len(trim(DSPKEY(46))) = 0 Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(11))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX12=" onclick=""Srcounty12onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(11) & "' " 
       SXX12=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(11) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td class=dataListHEAD>戶籍/公司地址</td>
		<td bgcolor="silver" colspan="3">
		<select size="1" name="key11" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry"><%=s%></select>
        <input type="text" name="key12" readonly  size="8" value="<%=dspkey(12)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA"><font SIZE=2>(鄉鎮)
			<input type="button" id="B12" <%=fieldpb%> name="B12" width="100%" style="Z-INDEX: 1" value="...." <%=SXX12%> >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C12" name="C12" style="Z-INDEX: 1" onclick="SrClear" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
        <input type="text" name="key13" <%=fieldpa%> size="40" value="<%=dspkey(13)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><font size=2>
		<input type="text" name="key14" readonly size="5" value="<%=dspkey(14)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA"></td></tr>
<%
	s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND len(trim(DSPKEY(46))) = 0 Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(15))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX16=" onclick=""Srcounty16onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(15) & "' " 
       SXX16=""
    End If
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
	<tr><td class=dataListHEAD>裝機地址<br><input type="radio" name="rd1" <%=fieldpb%> onClick="SrAddrEqual1()" ID="Radio3" VALUE="Radio3">同戶籍</td>
		<td bgcolor="silver" colspan=3>
		<select size="1" name="key15" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry"><%=s%></select>
        <input type="text" name="key16" readonly  size="8" value="<%=dspkey(16)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA"><font SIZE=2>(鄉鎮)
			<input type="button" id="B16"  <%=fieldpb%>  name="B16"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX16%>  >        
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C16" name="C16" style="Z-INDEX: 1" onclick="SrClear" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">          
		<input type="text" name="key17" <%=fieldpa%> size="40" value="<%=dspkey(17)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><font size=2>
		<input type="text" name="key18"  readonly size="5" value="<%=dspkey(18)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA"></td></tr>
<%
	s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND len(trim(DSPKEY(46))) = 0 Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(19))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX20=" onclick=""Srcounty20onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(19) & "' " 
       SXX20=""
    End If
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
	<tr><td class=dataListHEAD>帳單地址<br><input type="radio" name="rd2" <%=fieldpb%>  onClick="SrAddrEqual2()" ID="Radio4" VALUE="Radio4"><font SIZE=2>同戶籍</font><input type="radio"  <%=fieldpb%> name="rd2" onClick="SrAddrEqual3()" ID="radio"  <%=fieldpb%>1><font SIZE=2>同裝機</font></td>
		<td bgcolor="silver" colspan=3>
		<select size="1" name="key19" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry"><%=s%></select>
        <input type="text" name="key20" readonly size="8" value="<%=dspkey(20)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA"><font SIZE=2>(鄉鎮)
			<input type="button" id="B20" <%=fieldpb%> name="B20" width="100%" style="Z-INDEX: 1" value="...." <%=SXX20%> >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C20"  name="C20"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >
        <input type="text" name="key21" <%=fieldpa%> size="40" value="<%=dspkey(21)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><font size=2>
		<input type="text" name="key22" readonly size="5" value="<%=dspkey(22)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA"></td></tr>

	<TR><td class="dataListHEAD" height="23">企業負責人</td>
        <td height="23" bgcolor="silver" >
        <input type="text" name="key29" size="10" maxlength="10" <%=fieldpa%> value="<%=dspKey(29)%>" <%=fieldRole(1)%> class="dataListEntry"></td>

        <td class="dataListHEAD" height="23">負責人身份證號</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key30" size="10" maxlength="10" <%=fieldpa%> value="<%=dspKey(30)%>" <%=fieldRole(1)%> class="dataListEntry"></td></tr>

	<TR><td class="dataListHEAD" height="23">行業別</td>
        <td height="23" bgcolor="silver" >
        <input type="text" name="key31" size="20" maxlength="20" <%=fieldpa%> value="<%=dspKey(31)%>" <%=fieldRole(1)%> class="dataListEntry"></td>
        
		<td class="dataListHEAD" height="23">企業連絡人</td>
		<td height="23" bgcolor="silver" >
        <input type="text" name="key23" size="15" maxlength="12" <%=fieldpa%> value="<%=dspKey(23)%>" <%=fieldRole(1)%> class="dataListEntry"></td></tr>

	<TR><td class="dataListHEAD" height="23">企業連絡電話</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key24" size="15" maxlength="15" <%=fieldpa%> value="<%=dspKey(24)%>" <%=fieldRole(1)%> class="dataListEntry">
        <font size=2>分機︰</font>
        <input type="text" name="key25" size="5" maxlength="5" <%=fieldpa%> value="<%=dspKey(25)%>" <%=fieldRole(1)%> class="dataListEntry"></td>

		<td class="dataListHEAD" height="23">企業傳真</td>
		<td height="23" bgcolor="silver" >
        <input type="text" name="key27" size="30" maxlength="30" <%=fieldpa%> value="<%=dspKey(27)%>" <%=fieldRole(1)%> class="dataListEntry"></td></tr>

	<tr><td class="dataListHEAD" height="23">企業行動電話</td>
        <td height="23" bgcolor="silver" >
        <input type="text" name="key26" size="10" maxlength="10" <%=fieldpa%> value="<%=dspKey(26)%>" <%=fieldRole(1)%> class="dataListEntry"></td>

		<td class="dataListHEAD" height="23">企業 E-Mail</td>
		<td height="23" bgcolor="silver" >
        <input type="text" name="key28" size="50" maxlength="50" <%=fieldpa%> value="<%=dspKey(28)%>" <%=fieldRole(1)%> class="dataListEntry"></td></tr>

<%  
	name="" 
	if dspkey(32) <> "" then
		sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			&"where rtemployee.emply='" & dspkey(32) & "' "
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
		<input type="text" name="key32" size="6" READONLY value="<%=dspKey(32)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA"><font size=2><%=name%></font></td>
		
        <td class="dataListHEAD" height="23">建檔日期</td>                                 
        <td height="23" bgcolor="silver">
        <input type="text" name="key33" size="10" READONLY value="<%=dspKey(33)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA"></td></tr>  

<%
	name="" 
	if dspkey(34) <> "" then
		sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			&"where rtemployee.emply='" & dspkey(34) & "' "
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
		<input type="text" name="key34" size="6" READONLY value="<%=dspKey(34)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA"><font size=2><%=name%></font></td>

        <td class="dataListHEAD" height="23">修改日期</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key35" size="10" READONLY value="<%=dspKey(35)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA"></td></tr>
	</table></div>


<!--	<table border="1" width="100%" cellpadding="0" cellspacing="0" id="tag2" style="display: none">		-->
<DIV ID="SRTAG1" onclick="srtag1" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="LEFT">績效歸屬</td></tr></table></div>
<DIV ID=SRTAB1>
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table7">
<%
    s80=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) and len(trim(dspkey(48)))=0  and FIELDROLE(1)="" Then 
       sql="SELECT RTObj.CUSID AS CusID, RTObj.SHORTNC AS SHORTNC " _
           &"FROM RTObj INNER JOIN RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
           &"WHERE (((RTObjLink.CUSTYID)='02')) " _
           &"ORDER BY RTObj.SHORTNC " 
       If len(trim(dspkey(80))) < 1 Then
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
           &"WHERE (((RTObjLink.CUSTYID)='02')) and rtobj.cusid='" & DSPKEY(80) & "' " _
           &"ORDER BY RTObj.SHORTNC "
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(80) Then sx=" selected "
       s80=s80 &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td WIDTH="15%" class="dataListHEAD" height="23">經銷商</td>
        <td WIDTH="35%" height="23" bgcolor="silver">
			<select size="1" name="KEY80" <%=fieldpg%><%=FIELDROLE(1)%> class="dataListEntry"><%=S80%></select>
		<font size=2>經銷商開發業務:</font>
        <input type="text" name="key86" size="20" maxlength="20" value="<%=dspKey(86)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td>

<%
	name=""
	if dspkey(81) <> "" then
		sqlxx=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			 &"where rtemployee.emply='" & dspkey(81) & "' "
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
		<td width="35%"><input type="text" name="key81"value="<%=dspKey(81)%>" <%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" size="8" maxlength="6" readonly class="dataListDATA" ID="Text2">
			<input type="BUTTON" id="B81" name="B81" style="Z-INDEX: 1"  value="...." onclick="Srdeveloperonclick()">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C81" name="C81" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">
			<font size=2><%=name%></font></td></tr>

<%  
	If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(DSPKEY(46))) = 0  Then 
	   sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '1') "
       s="<option value="""" >(業務轄區)</option>"
    Else
       sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '1') AND AREAID='" & DSPKEY(36) & "' "
       s="<option value="""" >(業務轄區)</option>"
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(業務轄區)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("areaid")=dspkey(36) Then sx=" selected "
       s=s &"<option value=""" &rs("areaid") &"""" &sx &">" &rs("areanc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td id="tagT1" WIDTH="15%" class="dataListHEAD" height="23">業務轄區</td>
        <td WIDTH="85%" height="23" bgcolor="silver" colspan=3>
		<select size="1" name="key36" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry"><%=s%></select>

<%
	name=""
	if dspkey(37) <> "" then
		sqlxx=" select groupnc from RTSalesGroup where groupid='" & dspkey(37) & "' "
		rs.Open sqlxx,conn
		if rs.eof then
			name="(對象檔找不到業務組別)"
		else
			name=rs("groupnc")
		end if
		rs.close
	end if
%>
		<input type="text" name="key37" value="<%=dspKey(37)%>" <%=fieldpa%> <%=fieldRole(1)%><%=dataProtect%> size="3" maxlength="2" style="text-align:left;" readonly class="dataListEntry">
			<input type="button" id="B37" name="B37" <%=fieldpb%> width="100%" style="Z-INDEX: 1" value="...." readonly onclick="SrsalesGrouponclick()">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C37" name="C37" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">
			<font size=2><%=name%></font>

<%
	name=""
	if dspkey(38) <> "" then
		sqlxx=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			 &"where rtemployee.emply='" & dspkey(38) & "' "
		rs.Open sqlxx,conn
		if rs.eof then
			name="(對象檔找不到業務員)"
		else
			name=rs("cusnc")
		end if
		rs.close
	end if
%>
		<input type="text" name="key38" value="<%=dspKey(38)%>" <%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" size="8" maxlength="6" readonly class="dataListDATA" ID="Hidden1">
			<input type="BUTTON" id="B38" <%=fieldpb%> name="B38"  width="100%" style="Z-INDEX: 1"  value="...." onclick="Srsalesonclick()">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C38" name="C38" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">
			<font size=2><%=name%></font></td></tr>
	</table></DIV></DIV>


<DIV ID="SRTAG2" onclick="srtag2" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
    <tr><td bgcolor="BDB76B" align="LEFT">申請服務 / 電話號嗎明細表</td></tr></table></DIV>
<DIV ID=SRTAB2 >
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(DSPKEY(47))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M2' " 
       If len(trim(dspkey(39))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M2' AND CODE='" & dspkey(39) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(39) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table10">
	<tr><td WIDTH="15%"  class="dataListHEAD" height="23">方案類別</td>               
        <td WIDTH="35%" height="23" bgcolor="silver" >
		<select size="1"name="key39" <%=fieldpC%> <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry"><%=s%></select></td>

<%  
	dim FREECODE1,FREECODE2
    If Len(Trim(fieldRole(1) &dataProtect)) < 1 and flg = "Y" Then
       FREECODE1=""
       FREECODE2=""
    Else
      ' sexd1=" disabled "
      ' sexd2=" disabled "
    End If
    If dspKey(40)="Y" Then FREECODE1=" checked "    
    If dspKey(40)="N" or len(dspKey(40)) =0 Then FREECODE2=" checked " 
%>                          
        <td  WIDTH="15%" class="dataLISTSEARCH" height="23">公關機</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver" >
        <input type="radio" name="key40" value="Y" <%=FREECODE1%> <%=fieldRole(1)%><%=dataProtect%> ID="Radio1">是
        <input type="radio" name="key40" value="N" <%=FREECODE2%>  <%=fieldRole(1)%><%=dataProtect%> ID="Radio2">否</td></tr>

	<TR><td width=15% class="dataListHEAD" height="23">網路電話MAC號碼</td>
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key61" size="20" maxlength="17" value="<%=dspKey(61)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td>

        <td width=15% class="dataListHEAD" height="23">電話號碼(代表號)</td>
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key62" size="11" maxlength="11" value="<%=dspKey(62)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry">
        <font size=2>，區間</font>
        <input type="text" name="key63" size="5" maxlength="4" value="<%=dspKey(63)%>" <%=fieldpc%> <%=fieldRole(1)%> class="dataListEntry"> ~ 
        <input type="text" name="key64" size="5" maxlength="4" value="<%=dspKey(64)%>" <%=fieldpc%> <%=fieldRole(1)%> class="dataListEntry"></td></tr>

<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(DSPKEY(47))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M3' " 
       If len(trim(dspkey(65))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M3' AND CODE='" & dspkey(65) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(65) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td WIDTH="15%"  class="dataListHEAD" height="23">目前使用之寬頻服務</td>               
        <td WIDTH="35%" height="23" bgcolor="silver" >
		<select size="1"name="key65" <%=fieldpC%> <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry"><%=s%></select>
        <font size=2>　</font>
        <input type="text" name="key66" size="20" maxlength="20" value="<%=dspKey(66)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td>

<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(DSPKEY(47))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M4' " 
       If len(trim(dspkey(67))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M4' AND CODE='" & dspkey(67) & "'"
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
		<td WIDTH="15%"  class="dataListHEAD" height="23">電路服務業者</td>               
        <td WIDTH="35%" height="23" bgcolor="silver" >
		<select size="1"name="key67" <%=fieldpC%> <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry"><%=s%></select>
        <font size=2>　</font>
        <input type="text" name="key68" size="20" maxlength="20" value="<%=dspKey(68)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td></tr>
        
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(DSPKEY(47))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M5' " 
       If len(trim(dspkey(69))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M5' AND CODE='" & dspkey(69) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(69) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td WIDTH="15%"  class="dataListHEAD" height="23">上網型態</td>               
        <td WIDTH="35%" height="23" bgcolor="silver" >
		<select size="1"name="key69" <%=fieldpC%> <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry"><%=s%></select>
        <font size=2>　</font>
        <input type="text" name="key70" size="20" maxlength="20" value="<%=dspKey(70)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td>

<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(DSPKEY(47))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D3' " 
       If len(trim(dspkey(71))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D3' AND CODE='" & dspkey(71) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(71) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
		<td WIDTH="15%"  class="dataListHEAD" height="23">頻寬</td>               
        <td WIDTH="35%" height="23" bgcolor="silver" >
		<select size="1"name="key71" <%=fieldpC%> <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry"><%=s%></select>

<%  
	dim lineshare1,lineshare2
    If Len(Trim(fieldRole(1) &dataProtect)) < 1 and flg = "Y" Then
       lineshare1=""
       lineshare2=""
    Else
      ' sexd1=" disabled "
      ' sexd2=" disabled "
    End If
    If dspKey(72)="Y" Then lineshare1=" selected "    
    If dspKey(72)="N" Then lineshare2=" selected "
%>       

        <font size=2>是否與上網共用</font>
		<select size="1" name="key72"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
			<option value=""></option>
			<option value="Y" <%=lineshare1%>>Y</option>
			<option value="N" <%=lineshare2%>>N</option></select></td></tr>
<!--		        
        <input type="radio" value="Y" <%=lineshare1%> name="key72" <%=fieldRole(1)%><%=dataProtect%> ID="Radio5">是
        <input type="radio" name="key72" value="N" <%=lineshare2%>  <%=fieldRole(1)%><%=dataProtect%> ID="Radio6">否</td></tr>
-->        

	<TR><td width=15% class="dataListHEAD" height="23">ADSL附掛電話</td>
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key73" size="12" maxlength="11" value="<%=dspKey(73)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td>
        
        <td class="dataListSEARCH" height="23">固定IP位址</td>                                 
        <td height="23" bgcolor="silver"><font color=red>
		<input type="text" name="key55" size="3" maxlength="3" value="<%=dspKey(55)%>" <%=fieldRole(1)%> class="dataListEntry"><font size=2>.</font>
        <input type="text" name="key56" size="3" maxlength="3" value="<%=dspKey(56)%>" <%=fieldRole(1)%> class="dataListEntry"><font size=2>.</font>
        <input type="text" name="key57" size="3" maxlength="3" value="<%=dspKey(57)%>" <%=fieldRole(1)%> class="dataListEntry"><font size=2>.</font>
        <input type="text" name="key58" size="3" maxlength="3" value="<%=dspKey(58)%>" <%=fieldRole(1)%> class="dataListEntry"><font size=2> ~ </font>
        <input type="text" name="key59" size="3" maxlength="3" value="<%=dspKey(59)%>" <%=fieldRole(1)%> class="dataListEntry"></font></td></tr>
	</table></DIV>

	
<DIV ID="SRTAG5" onclick="srtag5" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
    <tr><td bgcolor="BDB76B" align="LEFT">代理人資訊</td></tr></table></DIV>

<DIV ID=SRTAB5>
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3"> 
	<TR><td width=15% class="dataListHEAD" height="23">代理人姓名</td>
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key41" size="15" maxlength="12" value="<%=dspKey(41)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td>

        <td width=15% class="dataListHEAD" height="23">代理人身份證號</td>
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key42" size="10" maxlength="10" value="<%=dspKey(42)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td></tr>

	<TR><td class="dataListHEAD" height="23">代理人電話</td>
        <td height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key43" size="20" maxlength="20" value="<%=dspKey(43)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td></tr>
	</TABLE></div>


<DIV ID="SRTAG4" onclick="srtag4" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0">
    <tr><td bgcolor="BDB76B" align="LEFT">用戶派工狀態</td></tr></table></DIV>

<DIV ID=SRTAB4 >
	<table border="1" width="100%" cellpadding="0" cellspacing="0">
	<tr><td width=15% class="dataListHEAD" height="23">派工單號</td>
        <td width=35% height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key82" size="12" maxlength="11" value="<%=dspKey(82)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"> / 
        <input type="text" name="key83" size="12" maxlength="11" value="<%=dspKey(83)%>" <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry"></td></tr>

	<tr><td class="dataListHEAD" height="23">工單收件日(派工日)</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key84" size="10" READONLY value="<%=dspKey(84)%>" <%=fieldpC%>  <%=fieldRole(1)%> class="dataListentry">
			<input type="button" id="B84" name="B84" height="100%" width="100%" <%=fieldpD%> style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C84" name="C84" <%=fieldpD%> style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>

		<td class="dataListHEAD" height="23">話機設定日(計費起算)</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key85" size="10" READONLY value="<%=dspKey(85)%>" <%=fieldpC%>  <%=fieldRole(1)%> class="dataListentry">
			<input type="button" id="B85" name="B85" height="100%" width="100%"  <%=fieldpD%> style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C85"  name="C85" <%=fieldpD%> style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td></tr>

	<tr><td class="dataListHEAD" height="23">安裝完工日</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key46" size="10" READONLY value="<%=dspKey(46)%>" <%=fieldpC%>  <%=fieldRole(1)%> class="dataListentry">
			<input type="button" id="B46"  name="B46" height="100%" width="100%"  <%=fieldpD%> style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C46"  name="C46"    <%=fieldpD%> style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>

        <td class="dataListHEAD" height="23">報竣回覆日(回報Sparq)</td>                                 
        <td height="23" bgcolor="silver">
		<input type="text" name="key47" size="10" READONLY value="<%=dspKey(47)%>" <%=fieldpe%> <%=fieldRole(1)%> class="dataListDATA">
			<input type="button" id="B47"  name="B47" height="100%" width="100%" <%=fieldpf%>style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C47"  name="C47"   <%=fieldpf%>style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></TD></tr> 

	<tr><td class="dataListHEAD" height="23">報竣轉檔日</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key48" size="10" value="<%=dspKey(48)%>" <%=fieldRole(1)%> class="dataListDATA"></td>

		<td class="dataListHEAD" height="23">退租日</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key49" size="10" value="<%=dspKey(49)%>" <%=fieldRole(1)%> class="dataListDATA">

        <font size=2>欠費︰</font>
        <input type="text" name="key52" size="2" READONLY value="<%=dspKey(52)%>" <%=fieldRole(1)%> class="dataListDATA"></td></tr>

	<tr><td width=15% class="dataListHEAD" height="23">作廢日期</td>                                 
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key50" size="10" value="<%=dspKey(50)%>" <%=fieldpa%><%=fieldRole(1)%> readonly class="dataListdata"></td>

<%
	name="" 
	if dspkey(51) <> "" then
		sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			&"where rtemployee.emply='" & dspkey(51) & "' "
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
        <input type="text" name="key51" size="10" value="<%=dspKey(51)%>" <%=fieldRole(1)%> readonly class="dataListDATA"><font size=2><%=name%></font>
        </td></tr>           
	</table></DIV>


<DIV ID="SRTAG7" onclick="SRTAG7" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table4">
    <tr><td bgcolor="BDB76B" align="LEFT">信用卡資料</td></tr></table></DIV>

<DIV ID="SRTAB7" >  
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table5">
<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='M6' " 
       If len(trim(dspkey(74))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='M6' AND CODE='" & dspkey(74) &"' " 
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(信用卡類型)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(74) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td width="15%" class="dataListHEAD" height="23">信用卡</td>
		<td width="35%" bgcolor="silver">
		<select size="1" name="key74"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry"><%=s%></select>

		<td width="15%" class="dataListHEAD" height="23">發卡銀行</td>
        <td width="35%" height="23" bgcolor="silver">
		<input type="text" name="key75" value="<%=dspKey(75)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
			style="text-align:left;" maxlength="20" size="25" class=dataListENTRY></td></tr>

	<tr><td class="dataListHEAD" height="23">信用卡卡號</td>
        <td height="23" bgcolor="silver">
		<input type="text" name="key76" value="<%=dspKey(76)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
			style="text-align:left;" maxlength="16" size="20" class=dataListENTRY></td>

		<td class="dataListHEAD" height="23">持卡人姓名</td>
        <td height="23" bgcolor="silver">
		<input type="text" name="key77" value="<%=dspKey(77)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
			style="text-align:left;" maxlength="20" size="20" class=dataListENTRY></td></tr>

	<tr><td class="dataListHEAD" height="23">信用卡有效期限</td>
        <td height="23" bgcolor="silver" colspan=3>
		<input type="text" name="key78" value="<%=dspKey(78)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
			style="text-align:left;" maxlength="2" size="5" class=dataListENTRY>月/
		<input type="text" name="key79" value="<%=dspKey(79)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
			style="text-align:left;" maxlength="2" size="5" class=dataListENTRY>年</td></tr>
    </table></div>


<DIV ID="SRTAG6" onclick="SRTAG6" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="LEFT">備註說明</td></tr></table></DIV>

<DIV ID="SRTAB6" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
    <TR><TD align="CENTER">
		<TEXTAREA cols="100%" name="key53" rows=8 MAXLENGTH=500 class="dataListentry" <%=dataprotect%> value="<%=dspkey(53)%>" ID="Textarea1"><%=dspkey(53)%></TEXTAREA></td></tr>
   </table></div> 
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
' --------------------------------------------------------------------------------------------  
%>
<!-- #include virtual="/Webap/include/checkid.inc" -->
<!-- #include virtual="/Webap/include/companyid.inc" -->
<!-- #include file="RTGetUserRight.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->