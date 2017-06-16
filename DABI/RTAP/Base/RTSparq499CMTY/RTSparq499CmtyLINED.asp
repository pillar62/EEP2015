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
                   case ucase("/webap/rtap/base/RTSparq499Cmty/RTSparq499Cmtylined.asp")
                     '  response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       if i <> 1 then rs.Fields(i).Value=dspKey(i)    
                       if i=1 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         rsc.open "select max(lineq1) AS lineq1 from RTSparq499Cmtyline where comq1=" & dspkey(0) ,conn
                         if len(rsc("lineq1")) > 0 then
                            dspkey(1)=rsc("lineq1") + 1
                         else
                            dspkey(1)=1
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
                 case ucase("/webap/rtap/base/RTSparq499Cmty/RTSparq499CmtyLINEd.asp")
                  '  response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                     if i<>0 and i <> 1 then rs.Fields(i).Value=dspKey(i)         
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
       if ucase(runpgm)=ucase("/webap/rtap/base/RTSparq499Cmty/RTSparq499CmtyLINEd.asp") then
          rs.open "select max(lineq1) AS lineq1 from RTSparq499Cmtyline where comq1=" & dspkey(0) ,conn
          if not rs.eof then
            dspkey(1)=rs("lineq1")
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
         &"            <tr><td class=dataListMessage>"& message &"</td></tr>" &vbCrLf _
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
  title="速博499主線資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT  COMQ1, LINEQ1, CONSIGNEE, AREAID, GROUPID, SALESID, LINEIPSTR1,  LINEIPSTR2, LINEIPSTR3, LINEIPSTR4, " _
             &"LINEIPEND, GATEWAY, SUBNET,  DNSIP, CHTWORKINGNO, LINETEL, LINERATE, CUTID, TOWNSHIP, RADDR, " _
             &"RZONE, EQUIPPOSITION, CUTID1, TOWNSHIP1, RADDR1, RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, " _
             &"RCVDAT, CONTACT, CONTACTTEL,  SUPPLYRANGE, SUPPLYCNT, SUPPLYBUILDCNT, COBOSS, COID, COBOSSID,TRADETYPE," _
             &"APPLYNAMEC, APPLYNAMEE, MEMO, EUSR, EDAT, UUSR, UDAT, TELCOMROOM, LOANNAME, LOANSOCIAL, " _
             &"CEDEAPPLYDAT, CEDEFINISHDAT, SETUPCONTACT, SETUPCONTACTTEL, INSPECTDAT, AGREE, UNAGREEREASON, ADSLAPPLYDAT, NCICAGREEDAT, TOCHTWORKING, " _
             &"LINEARRIVEDAT, BOXARRIVE, EQUIPARRIVE, ADSLOPENDAT, NCICOPENDAT,  DROPDAT, CANCELDAT, MOVETOCOMQ1, MOVETOLINEQ1, MOVETODAT, " _
             &"MOVEFROMCOMQ1, MOVEFROMLINEQ1, MOVEFROMDAT, COTPORT1,  COTPORT2, MDF1, MDF2, RESET, RESETDESC,IDSLAMIP, DEVELOPERID,EMAILFLG,CONNECTTYPE " _
             &"FROM RTSparq499CmtyLINE WHERE COMQ1=0 "
  sqlList="SELECT COMQ1, LINEQ1, CONSIGNEE, AREAID, GROUPID, SALESID, LINEIPSTR1,  LINEIPSTR2, LINEIPSTR3, LINEIPSTR4, " _
             &"LINEIPEND, GATEWAY, SUBNET,  DNSIP, CHTWORKINGNO, LINETEL, LINERATE, CUTID, TOWNSHIP, RADDR, " _
             &"RZONE, EQUIPPOSITION, CUTID1, TOWNSHIP1, RADDR1, RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, " _
             &"RCVDAT, CONTACT, CONTACTTEL,  SUPPLYRANGE, SUPPLYCNT, SUPPLYBUILDCNT, COBOSS, COID, COBOSSID,TRADETYPE," _
             &"APPLYNAMEC, APPLYNAMEE, MEMO, EUSR, EDAT, UUSR, UDAT, TELCOMROOM, LOANNAME, LOANSOCIAL, " _
             &"CEDEAPPLYDAT, CEDEFINISHDAT, SETUPCONTACT, SETUPCONTACTTEL, INSPECTDAT, AGREE, UNAGREEREASON, ADSLAPPLYDAT, NCICAGREEDAT, TOCHTWORKING, " _
             &"LINEARRIVEDAT, BOXARRIVE, EQUIPARRIVE, ADSLOPENDAT, NCICOPENDAT,  DROPDAT, CANCELDAT, MOVETOCOMQ1, MOVETOLINEQ1, MOVETODAT, " _
             &"MOVEFROMCOMQ1, MOVEFROMLINEQ1, MOVEFROMDAT, COTPORT1,  COTPORT2, MDF1, MDF2, RESET, RESETDESC,IDSLAMIP, DEVELOPERID,EMAILFLG,CONNECTTYPE " _
         &"FROM RTSparq499CmtyLINE WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=1
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    '檢查該主線所隸屬之社區是否為"可建置"社區==>若為可建置,則才可申請主線
    Set connXX=Server.CreateObject("ADODB.Connection")
    Set rsXX=Server.CreateObject("ADODB.Recordset")
    connXX.open DSN
    SQLXX="SELECT * FROM RTSparq499CmtyH WHERE COMQ1=" & DSPKEY(0)
    RSXX.Open sqlXX,CONNXX
    errcode=""
    IF RSXX.EOF THEN
       COMN=""
       ERRCODE="1"
    ELSE
       COMN=RSXX("COMN")
       IF RSXX("AGREE") <> "Y" then
          ERRCODE="2"
       ELSE
          ERRcode=""
       END IF
    END  IF
    RSXX.Close
    CONNXX.Close
    SET RSXX=NOTHING
    SET CONNXX=NOTHING
    
    if len(trim(DSPKEY(1))) = 0 THEN DSPKEY(1)=0
    if dspkey(55) <> "Y" and dspkey(55) <>"N" then dspkey(55)=""      
    IF LEN(TRIM(DSPKEY(34))) = 0 OR DSPKEY(34) = "" THEN DSPKEY(34)=0
    IF LEN(TRIM(DSPKEY(35))) = 0 OR DSPKEY(35) = "" THEN DSPKEY(35)=0   
    IF LEN(TRIM(DSPKEY(67))) = 0 OR DSPKEY(67) = "" THEN DSPKEY(67)=0
    IF LEN(TRIM(DSPKEY(68))) = 0 OR DSPKEY(68) = "" THEN DSPKEY(68)=0
    IF LEN(TRIM(DSPKEY(70))) = 0 OR DSPKEY(70) = "" THEN DSPKEY(70)=0
    IF LEN(TRIM(DSPKEY(71))) = 0 OR DSPKEY(71) = "" THEN DSPKEY(71)=0
    IF LEN(TRIM(DSPKEY(4))) = 0 THEN DSPKEY(4)=""
    IF LEN(TRIM(DSPKEY(6))) = 0 THEN DSPKEY(6)=0
    IF LEN(TRIM(DSPKEY(7))) = 0 THEN DSPKEY(7)=0
    IF LEN(TRIM(DSPKEY(8))) = 0 THEN DSPKEY(8)=0
    IF LEN(TRIM(DSPKEY(9))) = 0 THEN DSPKEY(9)=0
    IF LEN(TRIM(DSPKEY(10))) = 0 THEN DSPKEY(10)=0
    If len(trim(dspKey(0))) <= 0 Then
       dspkey(0)=0
    END IF       
    If len(trim(dspkey(30)))=0 or Not Isdate(dspkey(30)) then
       formValid=False
       message="收件日不可空白或格式錯誤"    
    elseif len(trim(dspkey(2))) >0 and len(trim(dspkey(5))) > 0 then
       formValid=False
       message="[經銷商]及[直銷業務]欄位請勿同時填入資料"
    elseif len(trim(dspkey(2))) =0 and len(trim(dspkey(5))) = 0 then   
       formValid=False
       message="[經銷商]及[直銷業務]欄位請擇一輸入資料"
    elseif len(trim(dspkey(36)))=0 then
       formValid=False
       message="公司負責人中文姓名不可空白"    
    elseif len(trim(dspkey(37)))=0 then
       formValid=False
       message="申請統一編號不可空白"    
 '   elseif len(trim(dspkey(38)))=0 then
 '      formValid=False
 '      message="公司負責人身份證號不可空白"       
    elseif len(trim(dspkey(40)))=0 then
       formValid=False
       message="申請人或公司中文名稱不可空白"   
    elseif len(trim(dspkey(41)))=0 then
       formValid=False
       message="申請人或公司英文名稱不可空白"       
    elseif len(trim(dspkey(17)))=0 then
       formValid=False
       message="裝機地址(縣市)不可空白"   
    elseif len(trim(dspkey(18)))=0 and dspkey(17)<>"06" and dspkey(17)<>"15" then
       formValid=False
       message="裝機地址(鄉鎮)不可空白"    
    elseif len(trim(dspkey(19)))=0 then
       formValid=False
       message="裝機地址不可空白"          
    elseif len(trim(dspkey(33)))=0 then
       formValid=False
       message="主線可供裝範圍不可空白"       
    elseif len(trim(dspkey(34)))=0 OR DSPKEY(34)=0 then
       formValid=False
       message="主線可供裝戶數不可空白或為0"       
    elseif len(trim(dspkey(35)))=0  OR DSPKEY(35)=0 then
       formValid=False
       message="主線可供裝棟數不可空白或為0"              
    elseif len(trim(dspkey(22)))=0 or len(trim(dspkey(23)))=0 or len(trim(dspkey(24)))=0 or len(trim(dspkey(25)))=0 then
       formValid=False
       message="戶籍地址/營業地址不可空白或不完裝"     
    elseif len(trim(dspkey(26)))=0 or len(trim(dspkey(27)))=0 or len(trim(dspkey(28)))=0 or len(trim(dspkey(29)))=0 then
       formValid=False
       message="帳單地址不可空白或不完整"   
    elseif len(trim(dspkey(31)))=0 then
       formValid=False
       message="連絡人不可空白"     
    elseif len(trim(dspkey(32)))=0 then
       formValid=False
       message="連絡人電話不可空白"   
    elseif len(trim(dspkey(16)))=0 then
       formValid=False
       message="主線速率不可空白"    
    elseif len(trim(dspkey(82)))=0 then
       formValid=False
       message="連接方式不可空白"    
    elseif dspkey(82)="04" and dspkey(16)="15" then
       formValid=False
       message="Off-Net499 主線速率錯誤"    
    elseif dspkey(82)="05" and dspkey(16)="14" then
       formValid=False
       message="On-Net499 主線速率錯誤"    
    elseif (len(trim(dspkey(48))) <> 0 and len(trim(dspkey(49))) = 0 ) or   (len(trim(dspkey(48))) = 0 and len(trim(dspkey(49))) <> 0 ) then
       formValid=False
       message="藉名裝機時，用戶名稱及身份證號必須同時存在"    
    elseif len(trim(DSPKEY(49)))  <> 0 AND  len(trim(DSPKEY(49)))  <> 10 AND  len(trim(DSPKEY(49)))  <> 8 THEN
       formValid=False
       message="身份證字號長度不足(必須為10碼)"                             
    elseif DSPKEY(54)<> "" AND LEN(TRIM(DSPKEY(54)))=0 THEN
       formValid=False
       message="請輸入勘查日期"                                                                                                                                                            
    elseif DSPKEY(55)="N" AND LEN(TRIM(DSPKEY(56)))=0 THEN
       formValid=False
       message="勘察為不可建置時必須輸入原因"  
    elseif len(trim(DSPKEY(57))) > 0 and dspkey(55)<>"Y" THEN
       formValid=False
       message="主線申請必須為勘查為[可建置]狀態"    
    elseif len(trim(DSPKEY(57))) > 0 and errcode="1" THEN
       formValid=False
       message="主線所屬之社區基本檔不存在，請檢查!"     
    elseif len(trim(DSPKEY(57))) > 0 and errcode="2" THEN
       formValid=False
       message="主線所屬之社區基本檔必須為(可建置)才可申請"    
    elseif len(trim(DSPKEY(63))) > 0 and ( len(trim(dspkey(6)))=0 OR TRIM(DSPKEY(6))="0" OR len(trim(dspkey(7)))=0 OR len(trim(dspkey(8)))=0 OR len(trim(dspkey(9)))=0 OR len(trim(dspkey(10)))=0  OR TRIM(DSPKEY(10))="0" ) THEN
       formValid=False
       message="主線測通後，主線IP網段不可空白"     
    elseif (len(trim(dspkey(6)))>0 and TRIM(dspkey(6)) > 255) OR ( len(trim(dspkey(7)))>0 and TRIM(dspkey(7)) > 255) OR ( len(trim(dspkey(8)))>0 and TRIM(dspkey(8)) > 255) OR ( len(trim(dspkey(9)))>0 and TRIM(dspkey(9)) > 255) OR ( len(trim(dspkey(10)))>0 and TRIM(dspkey(10)) > 255) then 
       formValid=False
       message="主線IP網段不可大於255"      
    ELSEIF  len(trim(DSPKEY(63))) > 0 AND ( LEN(TRIM(DSPKEY(12)))=0  ) THEN
       formValid=False
       message="主線測通後，主線SUBNET不可空白"       
    ELSEIF  len(trim(DSPKEY(63))) > 0 AND ( LEN(TRIM(DSPKEY(11)))=0  ) THEN
       'formValid=False
       message="主線測通後，主線Gateway不可空白"  
    ELSEIF  len(trim(DSPKEY(63))) > 0 AND ( LEN(TRIM(DSPKEY(13)))=0 ) THEN
       'formValid=False
       message="主線測通後，主線DNS IP不可空白"  
    elseif len(trim(DSPKEY(48))) = 0  THEN
       'formValid=False
       message="藉名用戶名稱不可空白"            
    elseif len(trim(DSPKEY(49))) = 0 OR (len(trim(dspkey(49))) <> 8 AND len(trim(dspkey(49))) <> 10) THEN
       formValid=False
       message="藉名用戶身份證號不可空白或長度不符合8碼或10碼"                      
    elseif len(trim(DSPKEY(14))) > 0 and len(trim(dspkey(14)))<>12 and len(trim(dspkey(14)))<>7 THEN
       formValid=False
       message="主線聯單編號長度必須為7碼或12碼"          
    elseif  dspkey(6) > 0  and ( len(trim(dspkey(10))) = 0 or dspkey(10) = 0 ) then
       formValid=False
       message="主線IP不完整(截止IP不可為零或空白)"  
    elseif  dspkey(7) > 0  and ( len(trim(dspkey(10))) = 0 or dspkey(10) = 0 ) then
       formValid=False
       message="主線IP不完整(截止IP不可為零或空白)"        
    elseif  dspkey(8) > 0  and ( len(trim(dspkey(10))) = 0 or dspkey(10) = 0 ) then
       formValid=False
       message="主線IP不完整(截止IP不可為零或空白)"        
    elseif  dspkey(9) > 0  and ( len(trim(dspkey(10))) = 0 or dspkey(10) = 0 ) then
       formValid=False
       message="主線IP不完整(截止IP不可為零或空白)"    
    elseif  dspkey(10) > 0  and ( len(trim(dspkey(6))) = 0 or dspkey(6) = 0 ) and ( len(trim(dspkey(7))) = 0 or dspkey(7) = 0 ) then
       formValid=False
       message="主線IP不完整(截止IP不為零或空白時，起始IP不可空白)"    
    elseif ( len(trim(dspkey(10))) > 0 or dspkey(10) > 0 ) and Cint(dspkey(10)) < Cint(dspkey(9)) then
       formValid=False
       message="主線IP起迄錯誤(截止IP不可小於起始IP)"  
    end if 
    if ( len(trim(dspkey(10))) > 0 or dspkey(10) > 0 ) AND (len(trim(dspkey(79))) = 0 OR dspkey(79)="0" ) then
       DSPKEY(79)=DSPKEY(6) & "." & DSPKEY(7) & "." & DSPKEY(8) & "." & CSTR(CINT(DSPKEY(10)) - 2)
    elseif dspkey(6) = 0 and dspkey(7) = 0 and dspkey(8) = 0 and dspkey(9) = 0 and dspkey(10) = 0 then
       dspkey(79)=""
    end if

    IF formValid <> FALSE THEN
       IF dspkey(10)>0 AND dspkey(9)>0 THEN
         IF ( CINT(TRIM(DSPKEY(10))) - CINT(TRIM(DSPKEY(9))) + 1 <> 16 ) THEN
            formValid=False
            message="主線IP網段起迄IP範圍必須為16組IP，請檢查。"     
         END IF
       END IF
    END IF
  '發送EMAIL(WHEN 線路到位日不為空白)
  IF DSPKEY(60) <> "" AND DSPKEY(81)<>"Y" AND formValid=TRUE THEN '當需更新主線之完工日的派工種類(STS=3)返轉才須要通知相關人員
      DSPKEY(81)="Y"
      FROMEMAIL="MIS@CBBN.COM.TW"
      Set jmail = Server.CreateObject("Jmail.Message")
      jmail.charset="BIG5"
      jmail.from = "MIS@cbbn.com.tw"
      Jmail.fromname="速博優質社區499系統通知"
      jmail.Subject = "優質499社區︰" & COMN & "，線路已到位，到位日︰" & DSPKEY(60) & "，可進行設備安裝派工"
      jmail.priority = 1  
      
      body="<html><body><table border=0 width=""60%""><tr><td colspan=2>" _
      &"<H3>速博優質社區499主線線路到位通知</h3></td></tr>" _
      &"<tr><td width=""30%"">&nbsp;</td><td width=""70%"">&nbsp;</td></tr>" _
      &"<tr><td bgcolor=lightblue align=center>社區主線序號</td><td bgcolor=pink align=left>" &DSPKEY(0) & "-" & DSPKEY(1) &"</td></tr>" _
      &"<tr><td bgcolor=lightblue align=center>社區名稱</td><td bgcolor=pink align=left>" &comn &"</td></tr>" _
      &"<tr><td bgcolor=lightblue align=center>線路到位日</td><td bgcolor=pink align=left>" & DSPKEY(60) &"</td></tr>" _
      &"</table>" _
      &"<P><U>此主線線路已到位，可執行設備安裝作業</U></body></html>"  
      jmail.HTMLBody = BODY
      'JMail.AddRecipient "fiona@cbbn.com.tw","優質499總窗口"
      JMail.AddRecipient "mis@cbbn.com.tw","資訊部"
      'JMail.AddRecipient "brian@cbbn.com.tw","工務窗口"
      'IF CONSIGNEE="Y" THEN
        ' JMail.AddRecipient "EDSON@cbbn.com.tw","坡朋"
      'ELSE
      '   IF AREAID="A2" THEN
      '      JMail.AddRecipient "lini@cbbn.com.tw","台中業助"
      '   ELSEIF AREAID="A3" THEN
      '      JMail.AddRecipient "cute0318@cbbn.com.tw","高雄業助"
      '   ELSEIF AREAID="A1" AND (GROUPID="01" OR GROUPID="07") THEN '桃園
      '      JMail.AddRecipient "lilu@cbbn.com.tw","桃園業助"
      '   ELSEIF AREAID="A1" AND (GROUPID="G1" OR GROUPID="G2" OR GROUPID="G3"  OR GROUPID="G4"  OR GROUPID="02" OR GROUPID="03" OR GROUPID="04" OR GROUPID="05" OR GROUPID="06") THEN '台北
      '      JMail.AddRecipient "tiffany01@cbbn.com.tw","台北業助"            
      '   END IF
      'END IF
      jmail.Send ( "email.cbbn.com.tw" )
   END IF          
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(45)=V(0)
        dspkey(46)=datevalue(now())
    end if     
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveXScript()%>
   <SCRIPT Language="VBScript">
	Sub srAutoGateway()
		if len(trim(document.all("key6").value)) <> 0 and len(trim(document.all("key7").value)) <> 0 and len(trim(document.all("key8").value)) <> 0 and len(trim(document.all("key9").value)) <> 0 and len(trim(document.all("key10").value)) <> 0 and len(trim(document.all("key1").value)) = 0 and document.all("key10").value <> "0" then
		   document.all("key11").value = document.all("key6").value &"."& document.all("key7").value &"."& document.all("key8").value &"."& document.all("key10").value -1
		end if
   end sub
   
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
   Sub Srcounty18onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY17").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key18").value =  trim(Fusrid(0))
          document.all("key20").value =  trim(Fusrid(1))
       End if       
       end if
   End Sub
   Sub Srsalesonclick()
       prog="RTGetsalesD.asp"
       prog=prog & "?KEY=" & document.all("KEY3").VALUE & ";"
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
			FUsrID=Split(Fusr,";")
       		if Fusrid(2) ="Y" then
          		document.all("key5").value =  trim(Fusrid(0))
       		End if
       end if
   End Sub
      Sub SrDeveloperonclick()
       prog="RTGetDeveloperD.asp"
       prog=prog & "?KEY=" & document.all("KEY80").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key80").value =  trim(Fusrid(0))
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
   Sub SrOPT1CLICK()
      ' msgbox window.SRTAB1.style.display
       if document.all("OPT1").checked=true then
          optvalue="Y"
       ELSE
          optvalue="N"
       END IF
       IF OPTVALUE="Y" THEN
          document.all("KEY48").VALUE="新世紀資通股份有限公司"
          document.all("KEY49").VALUE="70774626"
          document.all("KEY27").VALUE="內湖區"
          document.all("KEY28").VALUE="瑞光路218號6樓"
          document.all("KEY29").VALUE="114"
          document.all("OPT2").checked=false
          document.all("KEY48").CLASSNAME="dataListDATA"
          document.all("KEY48").READONLY=TRUE
          document.all("KEY49").CLASSNAME="dataListDATA"
          document.all("KEY49").READONLY=TRUE
       END IF
   End Sub                   
   Sub SrOPT2CLICK()
      ' msgbox window.SRTAB1.style.display
       if document.all("OPT2").checked=true then
          optvalue="Y"
       ELSE
          optvalue="N"
       END IF
       IF OPTVALUE="Y" THEN
          document.all("KEY48").VALUE=""
          document.all("KEY49").VALUE=""
          document.all("KEY27").VALUE="南港區"
          document.all("KEY28").VALUE="三重路19-8號6樓(八合股份有公司)"
          document.all("KEY29").VALUE="115"
          document.all("OPT1").checked=false
          document.all("KEY48").CLASSNAME="dataListENTRY"
          document.all("KEY48").READONLY=FALSE
          document.all("KEY49").CLASSNAME="dataListENTRY"
          document.all("KEY49").READONLY=FALSE
       END IF
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
       <tr><td width="20%" class=dataListHead>社區序號</td><td width="25%"  bgcolor="silver">
           <input type="text" name="key0"
                 <%=fieldRole(1)%> readonly size="10" value="<%=dspKey(0)%>" maxlength="8" class=dataListdata></td>
<td width="20%" class=dataListHead>主線序號</td><td width="25%"  bgcolor="silver">
           <input type="text" name="key1"
                 <%=fieldRole(1)%> readonly size="10" value="<%=dspKey(1)%>" maxlength="8" class=dataListdata></td>                 </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(43))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(43)=V(0)
        End if  
       dspkey(44)=datevalue(now())
    else
        if len(trim(dspkey(45))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(45)=V(0)
        End if         
        dspkey(46)=datevalue(now())
    end if      
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN

    '業助
	sql ="SELECT * FROM XXLib..UserGroup WHERE [GROUP] in('RTADMIN','業助') and userid ='" &logonid&"' "
    rs.Open sql,conn
    If rs.Eof Then
		basedata=False
    Else
		basedata=true
    End If
    rs.Close

    '主線測通後，網路資料+進度 PROTECT(業助除外)
	If len(trim(dspKey(63))) > 0 and basedata=false Then
       fieldPm=" class=""dataListData"" readonly "
       fieldpn=" disabled "
    Else
       fieldPm=""
       fieldpn=""
    End If

    '主線測通後，網路資料+進度 PROTECT
    If len(trim(dspKey(63))) > 0 Then
       fieldPb=" class=""dataListData"" readonly "
       fieldPa=" class=""dataListData"" readonly "
       FIELDPD=" DISABLED "
    Else
       fieldPb=""
       FIELDPD=""
    End If

    '主線申請後(DSPKEY57),基本資料+藉名裝機+績效歸屬+ADSL服務內容 protect
'    If len(trim(dspKey(57))) > 0 AND accessmode <> "A" Then
'       fieldPa=" class=""dataListData"" readonly "
'       FIELDPC=" DISABLED "
'    Else
'       fieldPa=""
'       FIELDPC=""
'    End If
%>

  <!--
  <span id="tags1" class="dataListTagsOn"
        onClick="vbscript:tag1.style.display=''    :tags1.classname='dataListTagsOn':
                          tag2.style.display='none':tags2.classname='dataListTagsOf'">基本資料</span>
  <span id="tags2" class="dataListTagsOf"
        onClick="vbscript:tag1.style.display='none':tags1.classname='dataListTagsOf':
                          tag2.style.display=''    :tags2.classname='dataListTagsOn'">發包安裝</span>           
  -->
  <span id="tags1" class="dataListTagsOn">速博499主線資訊</span>
                                                            
  <div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td><td>     
    <DIV ID="SRTAG0" onclick="srtag0" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="LEFT">基本資料</td></tr></table></div>
 <DIV ID=SRTAB0 >   
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
<tr><td width="15%" class=dataListHEAD>收件日</td>
    <td width="35%" bgcolor="silver" colspan=3>
        <input type="text" name="key30" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(30)%>"  READONLY size="10" class=dataListEntry>
                <input  type="HIDDEN" name="KEY81" height="100%" width="100%" style="Z-INDEX: 1" value="<%=dspKey(81)%>" >
       <input  type="button" id="B30"  <%=FIELDPC%>   name="B30" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPC%>  alt="清除" id="C30"  name="C30"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
                             </td>
</tr>
<tr><td rowspan=2 class=dataListHEAD>公司負責人<br>(個人申請免填)</td>
    <td  bgcolor="silver" rowspan=2  >
    <%if dspkey(36)="" then dspkey(36)="徐旭東" %>
        <input type="text" name="key36" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(36)%>"  readonly size="10" class=dataListdata ID="Text22"></td>
<td width="15%" class=dataListHEAD>申請人統一編號</td>
    <td width="35%" bgcolor="silver" >
    <%if dspkey(37)="" then dspkey(37)="70774626" %>    
        <input type="text" name="key37" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(37)%>"  readonly  size="10" class=dataListDATA ID="Text23"></td>               
</tr>
<tr>
   
<td  class=dataListHEAD>負責人身份證字號</td>
    <td  bgcolor="silver" >
    <%if dspkey(38)="" then dspkey(38)="" %>            
        <input type="text" name="key38" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(38)%>"  readonly  size="10" class=dataListDATA ID="Text25"></td>               
</tr>
<tr><td class=dataListHEAD>申請人姓名<br>/公司名稱</td>
    <td  bgcolor="silver" colspan=3>
 <%   'reference comty name
  '  Set connXX=Server.CreateObject("ADODB.Connection")
  '  Set rsXX=Server.CreateObject("ADODB.Recordset")
  '  connXX.open DSN
  '  SQLXX="SELECT * FROM  RTCounty RIGHT OUTER JOIN RTSparq499CmtyH ON RTCounty.CUTID = RTSparq499CmtyH.CUTID WHERE COMQ1=" & DSPKEY(0)
  '  RSXX.Open sqlXX,CONNXX
  '  errcode=""
  '  IF RSXX.EOF THEN
  '     comn=""
  '     areanc=""
  '  ELSE
  '     comn=rsxx("comn")
  '     if rsxx("ebtarea")="N" THEN
  '        AREANC="北區"
  '     ELSEIF rsxx("ebtarea")="M" THEN
  '        AREANC="中區"
  '     ELSEIF rsxx("ebtarea")="S" THEN
  '        AREANC="南區"
  '     ELSE
  '        AREANC=""
  '     END IF   
  '  END  IF
  '  RSXX.Close
  '  CONNXX.Close
  '  SET RSXX=NOTHING
  '  SET CONNXX=NOTHING    
    %>
    <%if dspkey(40)="" then dspkey(40)="新世紀資通股份有限公司" %>       
    <%if dspkey(41)="" then dspkey(41)="NCIC" %>       
    <%if dspkey(39)="" then dspkey(39)="" %>    
        <font size=2>中文︰</font><input type="text" name="key40" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="80"
               value="<%=dspKey(40)%>"  readonly  size="40" class=dataListDATA ID="Text26">
               <font size=2>英文︰</font><input type="text" name="key41" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="80"
               value="<%=dspKey(41)%>"  readonly  size="40" class=dataListDATA ID="Text1">
               <font size=2>行業別︰</font><input type="text" name="key39" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(39)%>"  readonly  size="20" class=dataListDATA ID="Text3">
               </td>
</tr>

<tr><td class=dataListHEAD>ADSL裝機地址</td>
    <td bgcolor="silver" COLSPAN=3>
  <%s=""
    sx=" selected "
    'If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND len(trim(dspKey(63))) = 0   AND FIELDPA = ""  Then 
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) and basedata=true Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(17))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX18=" onclick=""Srcounty18onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(17) & "' " 
       SXX18=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(17) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
         <select size="1" name="key17" <%=fieldPm%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ><%=s%></select>
        <input type="text" name="key18" readonly  size="8" value="<%=dspkey(18)%>" <%=fieldPm%> class="dataListEntry"><font size=2>(鄉鎮)                 
         <input type="button" id="B18"   <%=FIELDPn%>   name="B18"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX18%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPn%>  alt="清除" id="C18"  name="C18"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
        
        <input type="text" name="key19" <%=fieldPm%> size="40" value="<%=dspkey(19)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text5"><font size=2>
         <input type="text" name="key20"  readonly size="5" value="<%=dspkey(20)%>" class="dataListDATA" ID="Text35">
        
        <font size=2>設備位置</font>       
 <input type="text" name="key21" size="30" value="<%=dspkey(21)%>" maxlength="30" <%=fieldPm%> <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text21">             
        </td>                                 
</tr>  
<tr><td class=dataListHEAD >電信室/箱位置</td>
<td bgcolor="silver" COLSPAN=3>
 <input type="text" name="key47" size="90" value="<%=dspkey(47)%>" maxlength="60" <%=fieldPm%> <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text68">             
</tr>
<tr><td class=dataListHEAD >可供裝範圍</td>
<td bgcolor="silver" COLSPAN=3>
<input type="text" name="key33" size="90" value="<%=dspkey(33)%>" maxlength="90" <%=fieldPm%> <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text62">
</tr>
<tr>                                 
        <td  class="dataListHEAD" height="23">可供裝戶數</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key34" size="5" maxlength="5" value="<%=dspKey(34)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text41"></td>        
        <td  class="dataListHEAD" height="23">可供裝棟數</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key35" size="5" maxlength="5" value="<%=dspKey(35)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text48"></td>                                 
 
 </tr>        
<tr><td class=dataListHEAD >戶籍地址<br>/營業地址</td>
    <td bgcolor="silver" COLSPAN=3>
  <%s=""
    sx=" selected "
    IF DSPKEY(22)="" THEN 
       DSPKEY(22)="04"
    end if
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  AND len(trim(dspKey(63))) = 0   AND FIELDPA = "" Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='04' " 
       If len(trim(dspkey(22))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
  '    SXX23=" onclick=""Srcounty23onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(22) & "' and cutid='04' " 
       SXX23=""
    End If
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
         <select size="1" name="key22"   <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListDATA" ID="Select7"><%=s%></select>
<%if DSPKEY(23)="" THEN DSPKEY(23)="內湖區" %>        
        <input type="text" name="key23" size="8" value="<%=dspkey(23)%>" maxlength="10"  readonly <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text10"><font size=2>(鄉鎮)                 
         <input type="button" id="B37"  <%=FIELDPC%>  name="B37"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX23%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPC%>  alt="清除" id="C23"  name="C23"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
<%if DSPKEY(24)="" THEN DSPKEY(24)="瑞光路218號6樓" %>        
        <input type="text" name="key24"  readonly size="40" value="<%=dspkey(24)%>" maxlength="60" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text11"><font size=2>郵遞區號</font>  
<% IF DSPKEY(25)="" THEN DSPKEY(25)="114" %>
<input type="text" name="key25" size="5"   READONLY value="<%=dspKey(25)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text15">               
        </td>                                            
    </TR>
<tr><td class=dataListHEAD>ADSL帳寄地址</td>
    <td bgcolor="silver" COLSPAN=3>
  <%s=""
    sx=" selected "
    IF DSPKEY(26)="" THEN 
       DSPKEY(26)="04"
    end if
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  AND len(trim(dspKey(63))) = 0   AND FIELDPA = "" Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='04'" 
       If len(trim(dspkey(26))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
   '    s=s &"<option value=""" &"""" &sx &">(縣市別)</option>"       
    '   SXX27=" onclick=""Srcounty27onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(26) & "' and cutid='04'" 
       SXX27=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(26) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
         <select size="1" name="key26"  <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListDATA" ID="Select8"><%=s%></select>
<%if DSPKEY(27)="" THEN DSPKEY(27)="內湖區" %>          
        <input type="text" name="key27" size="8" value="<%=dspkey(27)%>" maxlength="10"   readonly <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text12"><font size=2>(鄉鎮)                 
         <input type="button" id="B27"  <%=FIELDPC%>  name="B27"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX27%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPC%>   alt="清除" id="C27"  name="C27"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
<%if DSPKEY(28)="" THEN DSPKEY(28)="瑞光路218號6樓" %>             
        <input type="text" name="key28"   readonly size="40" value="<%=dspkey(28)%>" maxlength="60" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text13"><font size=2>郵遞區號</font>  
<% IF DSPKEY(29)="" THEN DSPKEY(29)="114" %>
<input type="text" name="key29" size="5"   READONLY value="<%=dspKey(29)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text14">               
                     
</tr>        

<tr>                                 
        <td  class="dataListHEAD" height="23">連絡人姓名</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key31" size="15" value="<%=dspKey(31)%>" <%=fieldpm%><%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry" ID="Text8"></td>        
        </td>           
        <td  class="dataListHEAD" height="23">連絡電話(白天)</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key32" size="15" maxlength="15" value="<%=dspKey(32)%>" <%=fieldpm%><%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry" ID="Text7"></td>                                 
 
 </tr>        
<tr>
        <td  class="dataListHEAD" height="23">建檔人員</td>                                 
        <td  height="23" bgcolor="silver">
			<input type="text" name="key43" size="6" READONLY value="<%=dspKey(43)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA">
			<font size=2><%=SrGetEmployeeName(dspKey(43))%></font>
        </td>  
        <td  class="dataListHEAD" height="23">建檔日期</td>                                 
        <td  height="23" bgcolor="silver">
			<input type="text" name="key44" size="10" READONLY value="<%=dspKey(44)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>  
<tr>
        <td  class="dataListHEAD" height="23">修改人員</td>                                 
        <td  height="23" bgcolor="silver">
			<input type="text" name="key45" size="6" READONLY value="<%=dspKey(45)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA">
			<font size=2><%=SrGetEmployeeName(dspKey(45))%></font>
        </td>
        <td class="dataListHEAD" height="23">修改日期</td>                                 
        <td height="23" bgcolor="silver">
        	<input type="text" name="key46" size="10" READONLY value="<%=dspKey(46)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>                
</table> </div>
<!--
    <table border="1" width="100%" cellpadding="0" cellspacing="0" id="tag2" style="display: none"> 
    -->
    <DIV ID="srtag6" onclick="srtag6" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table10">
    <tr><td bgcolor="BDB76B" align="LEFT">藉名裝機</td></tr></table></div>
     <DIV ID="srtab6" >
     <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table11">    
     <tr><td colspan=4>
     <% '當主線未申請前才可變更借名裝機資料
'     IF LEN(TRIM(DSPKEY(57))) > 0 THEN
'        SROPT1=""
'        SROPT2=""
'        OPT1=" DISABLED "
'        OPT2=" DISABLED "
'     ELSE
        SROPT1=" ONCLICK=""SROPT1CLICK()"" "
        SROPT2=" ONCLICK=""SROPT2CLICK()"" "
        OPT1=""
        opt2=""
'     END IF
     %>
     <input type="checkbox" <%=OPT1%> name="OPT1" ID="OPT1" size="1" VALUE="1" <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" <%=SROPT1%>><FONT size=2>預設值</FONT>
     <input type="checkbox" <%=OPT2%> name="OPT2" ID="OPT2" size="1" VALUE="2" <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry"  <%=SROPT2%>><FONT size=2>藉名申請</FONT>     
     </td></tr>
     <tr>
     <td  WIDTH="15%" class="dataListHEAD" height="23">借名用戶名稱</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
<%if DSPKEY(48)="" THEN DSPKEY(48)="新世紀資通股份有限公司" %>             
        <input type="text" name="key48" size="30"  maxlength="10" value="<%=dspKey(48)%>"  <%=fieldpA%><%=fieldRole(1)%>  readonly class="dataListDATA" ID="Text69"></td>        
      <td  WIDTH="15%" class="dataListHEAD" height="23">借名用戶身份證號</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
<%if DSPKEY(49)="" THEN DSPKEY(49)="70774626" %>             
        <input type="text" name="key49" size="20"  maxlength="10" value="<%=dspKey(49)%>"  <%=fieldpA%><%=fieldRole(1)%>  readonly class="dataListDATA" ID="Text70"></td>        
   </tr>
   <tr>
     <td  WIDTH="15%" class="dataListHEAD" height="23">過戶申請日</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key50" size="10"  maxlength="10" value="<%=dspKey(50)%>"  <%=fieldpA%><%=fieldRole(1)%>  readonly class="dataListEntry" ID="Text6">        
      <input type="button" id="B50"  <%=FIELDPC%> name="B50" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPC%> alt="清除" id="C50"  name="C50"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"> </td>
      <td  WIDTH="15%" class="dataListHEAD" height="23">過戶完工日</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key51" size="10"  maxlength="10" value="<%=dspKey(51)%>"  <%=fieldpA%><%=fieldRole(1)%>  readonly class="dataListEntry" ID="Text16">
        <input type="button" id="B51" <%=FIELDPC%>  name="B51" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPC%> alt="清除" id="C51"  name="C51"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"> </td>        
   </tr>
      <tr>
     <td  WIDTH="15%" class="dataListHEAD" height="23">安裝連絡人</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key52" size="10"  maxlength="10" value="<%=dspKey(52)%>"  <%=fieldpA%><%=fieldRole(1)%>  class="dataListEntry" ID="Text24"></td>        
      <td  WIDTH="15%" class="dataListHEAD" height="23">安裝連絡電話</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key53" size="15"  maxlength="10" value="<%=dspKey(53)%>"  <%=fieldpA%><%=fieldRole(1)%>  class="dataListEntry" ID="Text27"></td>        
   </tr>
   </table>
   </div></div>

	<DIV ID="SRTAG1" onclick="srtag1" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="LEFT">績效歸屬</td></tr></table></div>

	<DIV ID=SRTAB1 >
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table7">
	<tr><td id="tagT1" WIDTH="15%" class="dataListHEAD" height="23">直銷業務</td>
		<%
		    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) and basedata=true Then 
       			sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '3') "
				If len(trim(dspkey(3))) < 1 Then
					sx=" selected " 
				else
					sx=""
				end if     
				s=s &"<option value=""" &"""" &sx &">(業務轄區)</option>"       
			Else
				sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '3') and areaid='" & dspkey(3) & "' " 
				sx=""    
			End If
			s=""
    		rs.Open sql,conn
    		If rs.Eof Then s="<option value="""" >(業務轄區)</option>"
    		Do While Not rs.Eof
				If rs("areaid")=dspkey(3) Then sx=" selected "
				s=s &"<option value=""" &rs("areaid") &"""" &sx &">" &rs("areanc") &"</option>"
				rs.MoveNext
       			sx=""
    		Loop
    		rs.Close
		%>
        <td  WIDTH="85%" height="23" bgcolor="silver" colspan=3>
           <select size="1" name="key3" <%=fieldpm%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" >
              <%=s%>
           </select>

        <input type="TEXT" name="key5" size="6" value="<%=dspKey(5)%>" readonly <%=fieldpm%> <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
           <input type="BUTTON" id="B5" name="B5"  width="100%" style="Z-INDEX: 1"  value="...." onclick="Srsalesonclick()"  <%=fieldpn%> >   
           <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C5" name="C5" onclick="SrClear" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" <%=fieldpn%> >
			<font size=2><%=SrGetEmployeeName(dspKey(5))%></font>
        </td></tr>

	<tr>
    <td width="15%" class=dataListHEAD>經銷商</td>
    <td width="35%" bgcolor="silver" >
<%  If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) and basedata=true Then 
		sql="select c.cusid, c.shortnc " &_
			"from RTConsignee a " &_
			"inner join RTConsigneeCASE b on a.CUSID = b.CUSID " &_
			"inner join RTObj c on c.cusid = a.cusid " &_
			"where	b.caseid ='00' order by c.shortnc " 
       s="<option value="""" >(經銷商)</option>"
    Else
	   sql=	"select c.cusid, c.shortnc " &_
			"from RTConsignee a " &_
			"inner join RTObj c on c.cusid = a.cusid " &_
			"where	c.cusid='" & dspkey(2) & "' "
		s =""
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(經銷商)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(2) Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>
           <select size="1" name="key2" <%=fieldpm%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" ID="Select34">                                            
              <%=s%>
           </select></td>

	<td WIDTH="15%" class="dataListHEAD" height="23">二線負責人</td>
		<td width="35%"><input type="text" name="key80"value="<%=dspKey(80)%>" <%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" size="8" maxlength="6" readonly class="dataListDATA" ID="Text36">
			<input type="BUTTON" id="B80" name="B80" style="Z-INDEX: 1"  value="...." onclick="Srdeveloperonclick()">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" id="C80" name="C80" onclick="SrClear" alt="清除" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" <%=FIELDPC%> >
			<font size=2><%=SrGetEmployeeName(dspKey(80))%></font>
	</td></tr>
  </table>     
  </DIV> 
  </DIV>   
      <DIV ID="SRTAG2" onclick="srtag2" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
    <tr><td bgcolor="BDB76B" align="LEFT">ADSL服務內容</td></tr></table></DIV>
    <DIV ID=SRTAB2 >
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3">

<tr>   <td  WIDTH="15%" class="dataListSEARCH" height="23">連接方式</td>
        <td WIDTH="35%" height="23" bgcolor="silver" colspan=5 >
		<%
			s=""
			sx=" selected "
			If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(63))) = 0  Then  
			sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='G5' AND CODE IN ('04','05') " 
			If len(trim(dspkey(82))) < 1 Then
				sx=" selected " 
				s=s & "<option value=""""" & sx & ">(連接方式)</option>"  
				sx=""
			else
				s=s & "<option value=""""" & sx & ">(連接方式)</option>"  
				sx=""
			end if     
			Else
			sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='G5' AND CODE='" & dspkey(82) & "'"
			End If
			rs.Open sql,conn
			Do While Not rs.Eof
			If rs("CODE")=dspkey(82) Then sx=" selected "
			s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
			rs.MoveNext
			sx=""
			Loop
			rs.Close
		%>         
			<select size="1" name="key82" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select1">                                                                  
					<%=s%>
			</select>
        </td>
</tr>
    
<tr>   <td  WIDTH="15%" class="dataListHEAD" height="23">主線速率</td>
        <td  WIDTH="35%" height="23" bgcolor="silver" >
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(63))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D3' AND CODE IN ('13','14','15') " 
       If len(trim(dspkey(16))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & ">(主線速率)</option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & ">(主線速率)</option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D3' AND CODE='" & dspkey(16) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(16) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key16" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35">                                                                  
        <%=s%>
   </select>
        </td>
        <td  WIDTH="15%" class="dataListSEARCH" height="23">附掛電話</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key15" size="15" maxlength="15" value="<%=dspKey(15)%>"  <%=fieldpB%><%=fieldRole(1)%> class="dataListEntry" ID="Text43"></td>                                 
                              
 </tr>        
  </table>     
  </DIV>
    <DIV ID="SRTAG3" onclick="srtag3" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table4">
    <tr><td bgcolor="BDB76B" align="LEFT">網路資料</td></tr></table></DIV>
   <DIV ID=SRTAB3 > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table5">
<tr>                                 
        <td  WIDTH="15%" class="dataListSEARCH" height="23">主線IP網段</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key6" size="3" onblur="srAutoGateway" maxlength="3" value="<%=dspKey(6)%>"  <%=filedPm%><%=fieldRole(1)%> class="dataListEntry" ID="Text37">
        <font size=2>.</font>
        <input type="text" name="key7" size="3" onblur="srAutoGateway" maxlength="3" value="<%=dspKey(7)%>"  <%=filedPm%><%=fieldRole(1)%> class="dataListEntry" ID="Text28">
        <font size=2>.</font>
        <input type="text" name="key8" size="3" onblur="srAutoGateway" maxlength="3" value="<%=dspKey(8)%>"  <%=filedPm%><%=fieldRole(1)%> class="dataListEntry" ID="Text29">
        <font size=2>.</font>
        <input type="text" name="key9" size="3" onblur="srAutoGateway" maxlength="3" value="<%=dspKey(9)%>"  <%=filedPm%><%=fieldRole(1)%> class="dataListEntry" ID="Text30">
        <font size=3>-</font>
        <input type="text" name="key10" size="3" onblur="srAutoGateway" maxlength="3" value="<%=dspKey(10)%>"  <%=filedPm%><%=fieldRole(1)%> class="dataListEntry" ID="Text31">
        </td>        
        <td  WIDTH="15%" class="dataListHEAD" height="23">主線SUBNET</td>     
         <td  WIDTH="35%" height="23" bgcolor="silver">                            
        <input type="text" name="key12" size="15"  maxlength="15" value="<%=dspKey(12)%>"  <%=filedPm%><%=fieldRole(1)%> class="dataListEntry" ID="Text32">
      </td>                                 
 
 </tr>        
<tr>                                 
        <td  class="dataListHEAD" height="23">閘道IP(Gateway)</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key11" size="15"   maxlength="15" value="<%=dspKey(11)%>"  <%=filedPm%><%=fieldRole(1)%> class="dataListEntry" ID="Text38"></td>        
        <td  class="dataListHEAD" height="23">DNS IP</td>                                 
        <td  height="23" bgcolor="silver">
        <%
        IF LEN(TRIM(DSPKEY(13)))=0 THEN DSPKEY(13)="211.78.130.1"
        %>
        <input type="text" name="key13" size="15" maxlength="15" value="<%=dspKey(13)%>"  <%=filedPm%><%=fieldRole(1)%> class="dataListEntry" ID="Text40"></td>                                 
 </tr>     
<tr> 
        <td  class="dataListHEAD" height="23">IDSLAM IP</td>                                 
        <td  height="23" bgcolor="silver"  >
        <input type="text" name="key79" size="15" MAXLENGTH=15 value="<%=dspKey(79)%>"  <%=filedPm%><%=fieldRole(1)%> class="dataListEntry" ID="Text39">
<!--        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  alt="清除" id="C79" name="C79" style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">  -->
		</td>

        <td  class="dataListSEARCH" height="23">CHT聯單編號</td>                                 
        <td  height="23" bgcolor="silver"  >
        <input type="text" name="key14" size="12" MAXLENGTH=12 value="<%=dspKey(14)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListEntry" ID="Text39"></td>         
</tr>
 <tr>                                 
        <td  class="dataListHEAD" height="23">COT PORT</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key73" size="10" maxlength="10" value="<%=dspKey(73)%>"  <%=fieldRole(1)%> class="dataListEntry" ID="Text77"></td>        
        <td  class="dataListHEAD" height="23">局端PORT</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key74" size="10" maxlength="10" value="<%=dspKey(74)%>"  <%=fieldRole(1)%> class="dataListEntry" ID="Text78"></td>                                 
 </tr>    
 <tr>                                 
        <td  class="dataListHEAD" height="23">MDF1</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key75" size="10" maxlength="10" value="<%=dspKey(75)%>"  <%=fieldRole(1)%> class="dataListEntry" ID="Text79"></td>        
        <td  class="dataListHEAD" height="23">MDF2</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key76" size="10" maxlength="10" value="<%=dspKey(76)%>" <%=fieldRole(1)%> class="dataListEntry" ID="Text80"></td>                                 
 </tr>
 <tr>
     <td class="dataListHEAD" height="23">遠端Reset方式</td>               
    <td height="23" bgcolor="silver" colspan=3>
	<%
		s=""
		sx=" selected "
		If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  Then  
	       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='K4' " 
		Else
	       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='K4' AND CODE='" & dspkey(77) & "'"
		End If
		rs.Open sql,conn
		Do While Not rs.Eof
	       If rs("CODE")=dspkey(77) Then sx=" selected "
			s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
		      rs.MoveNext
		sx=""
		Loop
		rs.Close%>         
	 <select size="1" name="key77"  <%=fieldRole(1)%> class="dataListEntry">                                                                  
		    <%=s%>
	 </select>	 </td></tr>          
	 <tr>
	<td  height="23" class="dataListHead">Reset備註</td>                     
    <td  height="23" bgcolor="silver" colspan=3>
		<input  class="dataListENTRY" type="text" size="100" maxlength="50" name="key78" value="<%=dspkey(78)%>"></td>          
     
 </tr>
  </table>   
  </DIV>
      <DIV ID="SRTAG4" onclick="srtag4" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr><td bgcolor="BDB76B" align="LEFT">主線申請及施工進度狀態</td></tr></table></DIV>
    <DIV ID=SRTAB4 >  
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr>
        <td  WIDTH="15%" class="dataListHEAD" height="23">主線勘察日</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key54" size="10"  READONLY  value="<%=dspKey(54)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text44">     
        <input type="button" id="B54"   <%=FIELDPC%>  <%=FIELDPF%>  name="B54" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPC%>  <%=FIELDPF%>  alt="清除" id="C54"  name="C54"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">  </td>         
        <td  WIDTH="15%" class="dataListHEAD" height="23">可否建置</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
<%  dim sexd5, sexd6
    If Len(Trim(fieldRole(1) &dataProtect)) < 1 and flg = "Y"   AND FIELDPB = "" Then
       sexd5=""
       sexd6=""
    Else
      ' sexd1=" disabled "
      ' sexd2=" disabled "
    End If
    If dspKey(55)="Y" Then sexd5=" checked "    
    If dspKey(55)="N" Then sexd6=" checked " %>           
        <input type="RADIO" <%=sexd5%> name="key55" size="1" value="Y"  <%=fieldRole(1)%> class="dataListEntry" ID="radio" >可
        <input type="RADIO" <%=sexd6%> name="key55" size="1" value="N"  <%=fieldRole(1)%> class="dataListEntry" ID="radio" >不可                
         </td></tr>
    <tr>
        <td   class="dataListHEAD" height="23">不可建置原因</td>                                 
        <td   height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key56" size="90" MAXLENGTH=90 value="<%=dspKey(56)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text46"></td>        
    </tr>
        <tr>
        <td  class="dataListHEAD" height="23" TITLE="主線經勘察為可建置者，才可提出主線申請!">主線申請日</td>                                 
        <td   height="23" bgcolor="silver">
        <input type="text" name="key57" size="10"   READONLY value="<%=dspKey(57)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text47">     
        <input type="button" id="B57"  <%=FIELDPD%>    name="B57" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPD%>   alt="清除" id="C57"  name="C57"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"> </td>          
        <td  class="dataListHEAD" height="23" >速博核准日</td>                                 
        <td   height="23" bgcolor="silver">
        <input type="text" name="key58" size="10"   READONLY value="<%=dspKey(58)%>"  <%=fieldpB%><%=fieldRole(1)%> class="dataListEntry" ID="Text33">     
        <input type="button" id="B58"   <%=FIELDPD%>  name="B58" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPD%>   alt="清除" id="C58"  name="C58"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"> </td>          
      </tr>
     <tr>
        <td  class="dataListHEAD" height="23">至營運處日</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key59" size="10" value="<%=dspKey(59)%>"  <%=fieldpB%><%=fieldRole(1)%> readonly class="dataListEntry" ID="Text34">
        <input type="button" id="B59"   <%=FIELDPD%>  name="B59" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPD%>    alt="清除" id="C59"  name="C59"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>     
        <td   class="dataListHEAD" height="23">線路到位日</td>                                 
        <td   height="23" bgcolor="silver">
        <input type="text" name="key60" size="10" value="<%=dspKey(60)%>"  <%=fieldpb%><%=fieldRole(1)%> readonly class="dataListEntry" ID="Text45">
        <input type="button" id="B60"   <%=FIELDPD%>  name="B60" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPD%>   alt="清除" id="C60"  name="C60"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
      </td>        
      </tr>     
      
     <tr>
        <td  class="dataListHEAD" height="23">機櫃派工日</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key61" size="10" value="<%=dspKey(61)%>"  <%=fieldpB%><%=fieldRole(1)%> readonly class="dataListDATA" ID="Text51">     
        <td   class="dataListHEAD" height="23">設備到位日</td>                                 
        <td   height="23" bgcolor="silver">
        <input type="text" name="key62" size="10" value="<%=dspKey(62)%>"  <%=fieldpb%><%=fieldRole(1)%> readonly class="dataListDATA" ID="Text52">
      </td>        
      </tr>     
        <tr>
        <td   class="dataListHEAD" height="23">主線測通日</td>                                 
        <td   height="23" bgcolor="silver">
        <input type="text" name="key63" size="10"   READONLY value="<%=dspKey(63)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListDATA" ID="Text55">     
        <!--<input type="button" id="B63"  <%=FIELDPD%>  <%=FIELDPF%>    name="B63" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"   <%=FIELDPF%>  alt="清除" id="C63"  name="C63"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
        --> </td>          
        <td   class="dataListHEAD" height="23">速博開通日</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key64" <%=fieldPm%> size="10"   READONLY value="<%=dspKey(64)%>"  <%=fieldRole(1)%> class="dataListEntry" ID="Text56">
        <input type="button" id="B64" <%=fieldPn%> name="B64" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldPn%> alt="清除" id="C64"  name="C64"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
      </td>          
      </tr>     
      <tr>
        <td  class="dataListSEARCH" height="23">撤線日</td>                                 
        <td   height="23" bgcolor="silver">
        <input type="text" name="key65" size="10" <%=fieldPm%> value="<%=dspKey(65)%>" <%=fieldRole(1)%> readonly class="dataListEntry" ID="Text71">
        <input type="button" id="B65" name="B65" <%=fieldPn%> height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldPn%> alt="清除" id="C65"  name="C65"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
        
        <td  class="dataListSEARCH" height="23">作廢日</td>                                 
        <td   height="23" bgcolor="silver" >
        <input type="text" name="key66" <%=fieldPm%> size="10" value="<%=dspKey(66)%>" <%=fieldRole(1)%> readonly class="dataListEntry" ID="Text72">
        <input type="button" id="B66" name="B66" <%=fieldPn%> height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldPn%> alt="清除" id="C66"  name="C66"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
             </td></tr>            
    <tr>
        <td  WIDTH="15%" class="dataListHEAD" height="23">移出社區主線序號</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key67" size="5"  READONLY  value="<%=dspKey(67)%>"  class="dataListDATA" ID="Text124">  
        <input type="text" name="key68" size="5"  READONLY  value="<%=dspKey(68)%>"   class="dataListDATA" ID="Text73">  
        <%
        IF LEN(TRIM(DSPKEY(67))) = 0 OR DSPKEY(67) = "" THEN DSPKEY(67)=0
        IF LEN(TRIM(DSPKEY(68))) = 0 OR DSPKEY(68) = "" THEN DSPKEY(68)=0
        if LEN(TRIM(dspkey(67))) > 0 AND DSPKEY(67) > 1 then
              sqlxx=" select * from RTSparq499CmtyH where COMQ1=" & dspkey(67) 
              rs.Open sqlxx,conn
              if rs.eof then
                 comn="(移出社區找不到)"
              else
                 comn=rs("comn")
              end if
              rs.close
        else
           comn=""
        end if
        %>
        <font size=2><%=comn%></font>
         </td>         
        <td  WIDTH="15%" class="dataListHEAD" height="23">移入社區主線序號</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key70" size="5"  READONLY  value="<%=dspKey(70)%>"  class="dataListDATA" ID="Text74">  
        <input type="text" name="key71" size="5"  READONLY  value="<%=dspKey(71)%>"   class="dataListDATA" ID="Text75">  
        <%
        IF LEN(TRIM(DSPKEY(70))) = 0 OR DSPKEY(70) = "" THEN DSPKEY(70)=0
        IF LEN(TRIM(DSPKEY(71))) = 0 OR DSPKEY(71) = "" THEN DSPKEY(71)=0
        if LEN(TRIM(dspkey(70))) > 0 AND DSPKEY(70) > 1 then
               sqlxx=" select * from RTSparq499CmtyH where COMQ1=" & dspkey(70) 
              rs.Open sqlxx,conn
              if rs.eof then
                 comn="(移入社區找不到)"
              else
                 comn=rs("comn")
              end if
              rs.close
        else
           comn=""
        end if        
        %>
         <font size=2><%=comn%></font>
         </td></tr>
    <tr>
        <td  WIDTH="15%" class="dataListHEAD" height="23">主線移出日期</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key69" size="10"  READONLY  value="<%=dspKey(69)%>"  class="dataListDATA" ID="Text126">     
         </td>         
        <td  WIDTH="15%" class="dataListHEAD" height="23">主線移入日期</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key72" size="10"  READONLY  value="<%=dspKey(72)%>"   class="dataListDATA" ID="Text127">  
         </td></tr>                        
  </table> 
  </DIV>
    <DIV ID="SRTAG5" onclick="srtag5" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="LEFT">備註說明</td></tr></table></DIV>
   <DIV ID="SRTAB5" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
    <TR><TD align="CENTER">
     <TEXTAREA  cols="100%"  name="key42" rows=8  MAXLENGTH=500  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(42)%>" ID="Textarea1"><%=dspkey(42)%></TEXTAREA>
   </td></tr>
 </table> 
  </DIV>    
<tr>                                   
  </div> 
<%
    conn.Close   
    set rs=Nothing   
    set conn=Nothing 
End Sub 
' --------------------------------------------------------------------------------------------  
%>
<!-- #include file="RTGetUserRight.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->