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
   ' response.write "sql=" & sql & ";accessmode=" & accessmode
   ' response.end
    if accessmode="" then accessmode="A"
    rs.Open sql,conn,3,3
    If rs.Eof Or rs.Bof Then
     '  RESPONSE.WRITE "ACCESSMODE=" & ACCESSMODE
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
                   case ucase("/webap/rtap/base/FTTBCMTYSTAT/FTTBCUSTd2.asp")
                   ' response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                    '   if i <> 1  then rs.Fields(i).Value=dspKey(i)    
                    '   if i=1  then
                    '     Set rsc=Server.CreateObject("ADODB.Recordset")
                    '     cusidxx="F" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
                    '     rsc.open "select max(cusid) AS cusid from FTTBCust where cusid like '" & cusidxx & "%' " ,conn
                    '     if len(rsc("cusid")) > 0 then
                    '        dspkey(1)=cusidxx & right("000" & cstr(cint(right(rsc("cusid"),3)) + 1),3)
                    '     else
                    '        dspkey(1)=cusidxx & "001"
                    '     end if
                    '     rsc.close
                         rs.Fields(i).Value=dspKey(i) 
                    '   end if      
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
                 case ucase("/webap/rtap/base/FTTBCMTYSTAT/FTTBCUSTd2.asp")
            '        response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
            '         if i<>1  then rs.Fields(i).Value=dspKey(i) 
                     rs.Fields(i).Value=dspKey(i)        
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
     '  runpgm=Request.ServerVariables("PATH_INFO")
     '  if ucase(runpgm)=ucase("/webap/rtap/base/FTTBCMTYSTAT/FTTBCUSTd2.asp") then
     '     cusidxx="F" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
     '     rsc.open "select max(cusid) AS cusid from FTTBCust where cusid like '" & cusidxx & "%' " ,conn
     '     if not rsC.eof then
     '       dspkey(1)=rsC("CUSID")
     '     end if
     '     rsC.close
     '  end if
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
  numberOfKey=3
  title="FTTB用戶資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT COMQ1,CUSID, ENTRYNO, CUSNC, HBCUSNO, FTTBCUSNO, OFFICE,EXTENSION, CONTACTTEL, MOBILE, CONTACT, " _
             &"EMAIL, BIRTHDAY, IDNUMBERTYPE, SOCIALID, SECONDIDTYPE, SECONDNO, COBOSS, CUTID1, TOWNSHIP1, RADDR1," _
             &"RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, CUTID3,TOWNSHIP3, RADDR3, RZONE3, APPLYDAT," _
             &"SNDDAT, SERVICENET1,  SERVICENET2, SERVICEMOD1, SERVICEMOD2, OTHSRV1, V2HNNO, OTHSRV2,OTHSRV3, V3EMAIL," _
             &"OTHSRV4, CUSKIND, CANCELDAT, MEMO,EUSR,EDAT,UUSR,UDAT,RADDR1TEL,COMBINETEL,CANCELUSR " _
             &"FROM  FTTBCUST  WHERE CUSID='' " 

  sqlList="SELECT COMQ1,CUSID, ENTRYNO,  CUSNC, HBCUSNO, FTTBCUSNO, OFFICE,EXTENSION, CONTACTTEL, MOBILE, CONTACT, " _
             &"EMAIL, BIRTHDAY, IDNUMBERTYPE, SOCIALID, SECONDIDTYPE, SECONDNO, COBOSS, CUTID1, TOWNSHIP1, RADDR1," _
             &"RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, CUTID3,TOWNSHIP3, RADDR3, RZONE3, APPLYDAT," _
             &"SNDDAT, SERVICENET1,  SERVICENET2, SERVICEMOD1, SERVICEMOD2, OTHSRV1, V2HNNO, OTHSRV2,OTHSRV3, V3EMAIL," _
             &"OTHSRV4, CUSKIND, CANCELDAT, MEMO,EUSR,EDAT,UUSR,UDAT,RADDR1TEL,COMBINETEL,CANCELUSR  " _
             &"FROM  FTTBCUST  WHERE "
  userDefineRead="Yes"      
  userDefineSave="Yes"       
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userdefineactivex="Yes"
  accessmode="A"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
'  RESPONSE.WRITE "SW=" & SW & ";ACCESSMODE=" & ACCESSMODE
  if accessmode="" then accessmode="A"
  if len(trim(dspkey(1)))=0 then dspkey(1)=""
 ' if len(trim(dspkey(2)))=0 then dspkey(2)=1
  if len(trim(dspkey(13)))=0 then dspkey(13)=""
  if len(trim(dspkey(15)))=0 then dspkey(15)=""
  if len(trim(dspkey(18)))=0 then dspkey(18)=""
  if len(trim(dspkey(22)))=0 then dspkey(22)=""
  if len(trim(dspkey(26)))=0 then dspkey(26)=""
  if len(trim(dspkey(32)))=0 then dspkey(32)="Y"
  if len(trim(dspkey(33)))=0 then dspkey(33)=""
  if len(trim(dspkey(34)))=0 then dspkey(34)=""
  if len(trim(dspkey(35)))=0 then dspkey(35)=""
  if len(trim(dspkey(36)))=0 then dspkey(36)=""
  if len(trim(dspkey(38)))=0 then dspkey(38)=""
  if len(trim(dspkey(39)))=0 then dspkey(39)=""
  if len(trim(dspkey(41)))=0 then dspkey(41)=""
  if len(trim(dspkey(42)))=0 then dspkey(42)=""
  '身份證欄位第一碼,用以判別是個人還是公司,若為公司則出生日期必須空白,反之則不可空白
  LEADINGCHAR=LEFT(DSPKEY(14),1)
  IF LEADINGCHAR >="0" AND LEADINGCHAR <="9" THEN
     COMPANY="Y"
  ELSE
     COMPANY="N"
  END IF

  '身份證第一碼大寫
  DSPKEY(14)=UCASE(DSPKEY(14))
  if len(trim(dspkey(30)))=0 then
       formValid=False
       message="用戶申請日不可空白"   
  elseif len(trim(dspkey(5)))>0 AND len(trim(dspkey(5))) <> 8 then
       formValid=False
       message="FTTB HNNO的長度必須為8位數字"  
  elseif len(trim(dspkey(4))) =0 then
       formValid=False
       message="原HiBuilding的HNNO不可空白"               
  elseif dspkey(36)="Y" AND len(trim(dspkey(37))) =0 then
       formValid=False
       message="保留HiNet撥接帳號必須輸入撥接帳號HNNO"        
  elseif len(trim(dspkey(37))) > 0 AND len(trim(dspkey(37)))<> 8 then
       formValid=False
       message="撥接帳號HNNO必須為8位數字"             
  elseif len(trim(dspkey(3)))=0 then
       formValid=False
       message="用戶名稱不可空白"         
  elseif len(trim(dspkey(14)))=0  then
       formValid=False
       message="第一證照別不可空白"                    
  elseif ( len(trim(dspkey(14)))<>0 and (len(trim(dspkey(14)))<>10 and len(trim(dspkey(14)))<>8 ) ) then
       formValid=False
       message="用戶身分證(統編)長度不對"    
  elseif len(trim(dspkey(15)))=0  then
       formValid=False
       message="第二證照別不可空白"         
  elseif len(trim(dspkey(16)))=0  then
       formValid=False
       message="第二證照號碼不可空白"                
  elseif len(trim(dspkey(8)))=0 AND len(trim(dspkey(9)))=0 then
       formValid=False
       message="用戶聯絡電話或行動電話至少需輸入一項"      
 ' elseif instr(1,dspkey(8),"-",1) > 0 then
 '      formValid=False
 '      message="用戶聯絡電話不可包含'-'符號"                     
 ' elseif instr(1,dspkey(9),"-",1) > 0 then
 '      formValid=False
 '      message="用戶行動電話不可包含'-'符號"                    
  elseif len(trim(dspkey(18)))=0 then
       formValid=False
       message="裝機地址(縣市)不可空白"   
  elseif len(trim(dspkey(19)))=0 and dspkey(18)<>"06" and dspkey(18)<>"15" then
       formValid=False
       message="裝機地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(20)))=0 then
       formValid=False
       message="裝機地址不可空白"          
  elseif len(trim(dspkey(22)))=0 then
       formValid=False
       message="帳單地址(縣市)不可空白"   
  elseif len(trim(dspkey(23)))=0 and dspkey(22)<>"06" and dspkey(22)<>"15" then
       formValid=False
       message="帳單地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(24)))=0 then
       formValid=False
       message="帳單地址不可空白"     
  elseif len(trim(dspkey(26)))=0 then
       formValid=False
       message="戶籍地址(縣市)不可空白"   
  elseif len(trim(dspkey(27)))=0 and dspkey(26)<>"06" and dspkey(26)<>"15" then
       formValid=False
       message="戶籍地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(28)))=0 then
       formValid=False
       message="戶籍地址不可空白"          
 elseif (len(trim(dspkey(12)))=0 or Not Isdate(dspkey(12))) AND COMPANY="N" then
       formValid=False
       message="用戶為個人時，出生日期不可空白或格式錯誤"   
  elseif len(trim(dspkey(12)))<>0  AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，出生日期必須空白"          
  elseif len(trim(dspkey(17)))=0 AND COMPANY="Y" then
       formValid=False
       message="公司負責人不可空白"              
  elseif len(trim(dspkey(31)))<> 0 AND len(trim(dspkey(5)))=0 then
       formValid=False
       message="輸入送件日期後，FTTB的HNNO不可空白。"                
  end if
  IF formValid=TRUE THEN
    IF dspkey(14) <> "" and (dspkey(13)="01" or dspkey(13)="02") then
       idno=dspkey(14)
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
  '當用戶送件日不為空白時，檢查社區的光化進度是否為"00"，"00"者才可送件
  IF formValid=TRUE and len(trim(dspkey(31)))<> 0 THEN
     Set connxx=Server.CreateObject("ADODB.Connection")
     Set rsxx=Server.CreateObject("ADODB.Recordset")
     connxx.open DSN
     sqlxx="select top 1 * from  FTTBCMTYSTAT where canceldat is null order by smode "
     rsxx.open sqlxx,connxx
     if rsxx.eof then
        formvalid=false   
        message="此用戶送件時，主線光化進度資料找不到。"
     elseif rsxx("smode") <> "00" then
        formvalid=false   
        message="此用戶送件時，主線光化進度必須在(00:FTTB光纖已到位)，目前為︰" & RSXX("SMODE")
     end if
     rsxx.close
     connxx.close
     set rsxx=nothing
     set connxx=nothing
  end if
 
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(47)=V(0)
        dspkey(48)=datevalue(now())
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
 
   Sub SrChangetelzip1()
       ctyid=document.all("KEY18").VALUE
       if ctyid="" then
          document.all("key21").value=""
       else
          if ctyid="01" or ctyid="03" or ctyid="04" then
              document.all("key21").value="02"
          elseif ctyid="02" or ctyid="05" or ctyid="06" or ctyid="07"or ctyid="21" then
              document.all("key21").value="03"
          elseif ctyid="08"  then
              document.all("key21").value="037"
          elseif ctyid="09" or ctyid="10" or ctyid="12" then
              document.all("key21").value="04"    
          elseif ctyid="11" then
              document.all("key21").value="049"       
          elseif ctyid="13" or ctyid="14" or ctyid="15" then
              document.all("key21").value="05"    
          elseif ctyid="16" or ctyid="17" or ctyid="23" then
              document.all("key21").value="06"    
          elseif ctyid="18" or ctyid="19" then
              document.all("key21").value="07" 
          elseif ctyid="20" then
              document.all("key21").value="08"         
          elseif ctyid="22" then
              document.all("key21").value="089"      
          elseif ctyid="24" then
              document.all("key21").value="0823"     
          elseif ctyid="25" then
              document.all("key21").value="0836"   
          else
              document.all("key21").value=""       
          end if                                  
       end if
   End Sub    
   Sub SrChangetelzip2()
       ctyid=document.all("KEY22").VALUE
       if ctyid="" then
          document.all("key25").value=""
       else
          if ctyid="01" or ctyid="03" or ctyid="04" then
              document.all("key25").value="02"
          elseif ctyid="02" or ctyid="05" or ctyid="06" or ctyid="07"or ctyid="21" then
              document.all("key25").value="03"
          elseif ctyid="08"  then
              document.all("key25").value="037"
          elseif ctyid="09" or ctyid="10" or ctyid="12" then
              document.all("key25").value="04"    
          elseif ctyid="11" then
              document.all("key25").value="049"       
          elseif ctyid="13" or ctyid="14" or ctyid="15" then
              document.all("key25").value="05"    
          elseif ctyid="16" or ctyid="17" or ctyid="23" then
              document.all("key25").value="06"    
          elseif ctyid="18" or ctyid="19" then
              document.all("key25").value="07" 
          elseif ctyid="20" then
              document.all("key25").value="08"         
          elseif ctyid="22" then
              document.all("key25").value="089"      
          elseif ctyid="24" then
              document.all("key25").value="0823"     
          elseif ctyid="25" then
              document.all("key25").value="0836"   
          else
              document.all("key25").value=""       
          end if                                  
       end if
   End Sub  
      Sub SrChangetelzip3()
       ctyid=document.all("KEY26").VALUE
       if ctyid="" then
          document.all("key29").value=""
       else
          if ctyid="01" or ctyid="03" or ctyid="04" then
              document.all("key29").value="02"
          elseif ctyid="02" or ctyid="05" or ctyid="06" or ctyid="07"or ctyid="21" then
              document.all("key29").value="03"
          elseif ctyid="08"  then
              document.all("key29").value="037"
          elseif ctyid="09" or ctyid="10" or ctyid="12" then
              document.all("key29").value="04"    
          elseif ctyid="11" then
              document.all("key29").value="049"       
          elseif ctyid="13" or ctyid="14" or ctyid="15" then
              document.all("key29").value="05"    
          elseif ctyid="16" or ctyid="17" or ctyid="23" then
              document.all("key29").value="06"    
          elseif ctyid="18" or ctyid="19" then
              document.all("key29").value="07" 
          elseif ctyid="20" then
              document.all("key29").value="08"         
          elseif ctyid="22" then
              document.all("key29").value="089"      
          elseif ctyid="24" then
              document.all("key29").value="0823"     
          elseif ctyid="25" then
              document.all("key29").value="0836"   
          else
              document.all("key29").value=""       
          end if                                  
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

   Sub Srcounty19onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY18").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key19").value =  trim(Fusrid(0))
          document.all("key21").value =  trim(Fusrid(1))
       End if       
       end if
    END SUB
   Sub Srcounty23onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY22").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key23").value =  trim(Fusrid(0))
          document.all("key25").value =  trim(Fusrid(1))
       End if       
       end if
    END SUB    
   Sub Srcounty27onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY26").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key27").value =  trim(Fusrid(0))
          document.all("key29").value =  trim(Fusrid(1))
       End if       
       end if
    END SUB

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

Sub SrAddrEqual1()
   document.All("key22").value=document.All("key18").value
   document.All("key23").value=document.All("key19").value
   document.All("key24").value=document.All("key20").value
   document.All("key25").value=document.All("key21").value
End Sub 
Sub SrAddrEqual2()
   document.All("key26").value=document.All("key18").value
   document.All("key27").value=document.All("key19").value
   document.All("key28").value=document.All("key20").value
   document.All("key29").value=document.All("key21").value
End Sub      
Sub SrAddrEqual3()   
   document.All("key26").value=document.All("key22").value
   document.All("key27").value=document.All("key23").value
   document.All("key28").value=document.All("key24").value
   document.All("key29").value=document.All("key25").value
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
      <table width="60%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="15%" class=dataListHead>社區序號</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key0"
                 <%=fieldRole(1)%> readonly size="5" value="<%=dspKey(0)%>" maxlength="5" class=dataListdata></td>
                 <td width="15%" class=dataListHead>用戶序號</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key1"
                 <%=fieldRole(1)%> readonly size="10" value="<%=dspKey(1)%>" maxlength="10" class=dataListdata></td>
           <td width="15%" class=dataListHead>用戶項次</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key2"
                 <%=fieldRole(1)%> readonly size="3" value="<%=dspKey(2)%>" maxlength="3" class=dataListdata></td>                 
  </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(45))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(45)=V(0)
        End if  
       dspkey(46)=datevalue(now())
    else
  '      if len(trim(dspkey(47))) < 1 then
  '         Call SrGetEmployeeRef(Rtnvalue,1,logonid)
  '              V=split(rtnvalue,";")  
  '              DSpkey(47)=V(0)
  '      End if         
  '      dspkey(48)=datevalue(now())
    end if      
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
    '用戶送件後,資料 protect
    If len(trim(dspKey(31))) > 0 AND (formvalid =TRUE OR FORMVALID="") Then
       fieldPa=" class=""dataListData"" readonly "
       fieldpb=" disabled "
    Else
       fieldPa=""
       fieldpb=""
    End If

    '用戶作廢後資料 protect
    If len(trim(dspKey(43))) > 0 Then
       fieldPa=" class=""dataListData"" readonly "
       fieldpb=" disabled "
       fieldPe=" class=""dataListData"" readonly "
       fieldpf=" disabled "
    Else
       fieldPe=""
       fieldpf=""
    End If    
      
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    Set connXX=Server.CreateObject("ADODB.Connection")
    Set rsXX=Server.CreateObject("ADODB.Recordset")
    CONNXX.OPEN DSN
    IF DSPKEY(2)="" THEN DSPKEY(2)=1
    SQLXX="SELECT rtcust.*,rtobj.cusnc FROM RTCUST LEFT OUTER JOIN RTOBJ ON RTCUST.CUSID=RTOBJ.CUSID WHERE RTCUST.COMQ1=" & DSPKEY(0) & " AND RTCUST.CUSID='" & DSPKEY(1) & "' AND RTCUST.ENTRYNO=" & DSPKEY(2)
   ' response.write sqlxx
    RSXX.OPEN SQLXX,CONNXX
    conn.open DSN
    IF Accessmode="" then accessmode="A"
    %>
  <!--
  <span id="tags1" class="dataListTagsOn"
        onClick="vbscript:tag1.style.display=''    :tags1.classname='dataListTagsOn':
                          tag2.style.display='none':tags2.classname='dataListTagsOf'">基本資料</span>
  <span id="tags2" class="dataListTagsOf"
        onClick="vbscript:tag1.style.display='none':tags1.classname='dataListTagsOf':
                          tag2.style.display=''    :tags2.classname='dataListTagsOn'">發包安裝</span>           
  -->
  <span id="tags1" class="dataListTagsOn">FTTB用戶資訊</span>
                                                            
  <div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td>
<td>     
    <DIV ID="SRTAG0" onclick="srtag0" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="CENTER">基本資料</td></tr></table></div>
 <DIV ID=SRTAB0 >   
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
<tr>
    <td width="10%" class=dataListHEAD>用戶申請日</td>
    <td width="23%" bgcolor="silver" >
        <input type="text" name="key30" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(30)%>"  READONLY size="10" class=dataListEntry>
       <input  type="button" id="B30"  <%=fieldpb%> name="B30" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C30"  name="C30"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
      </td>    
    <td width="10%" class=dataListHEAD>FTTB HNNO</td>
    <td width="23%" bgcolor="silver" >
        <input type="text" name="key5" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="8"
               value="<%=dspKey(5)%>"  size="8" class=dataListEntry></TD>  
     <td width="10%" class=dataListHEAD>客戶種類</td>
    <td width="23%" bgcolor="silver" >
<%
    s=""
    sx=" selected "
  '  response.write "sw=" & sw & ";" & "accessmode=" & accessmode
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) or accessmode="" ) and len(trim(dspkey(43)))=0  and len(trim(dspkey(31)))=0 Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='O7' AND CODE IN ('01','02') ORDER BY CODE" 
       If len(trim(dspkey(42))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='O7' AND CODE='" & dspkey(42) &"' " 
       'SXX60=""
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(客戶類別)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(42) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
   <select size="1" name="key42"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select16">
   <%=s%>
   </select>          
        </TD>   
     </TR>   
<tr><td colspan=6>
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
<tr>                                 
        <td   width="10%" class="dataListHEAD" height="23" ROWSPAN=2>申請事項<BR>(請勾選)</td>                                 
         <td  class="dataListHEAD" height="23"><CENTER>上網服務</CENTER></td>                                 
        <td  class="dataListHEAD" height="23"><CENTER>MOD服務</CENTER></td>         
        <td  class="dataListHEAD" height="23"><CENTER>其它服務</CENTER></td>                         
</tr>   
<tr>                                 
        <td  bgcolor="silver"  height="23" style="vertical-align:top">
        <%IF DSPKEY(32)="Y" THEN
             FREECODE1=" CHECKED "
          ELSE
             DSPKEY(32)="Y"
             FREECODE1=" CHECKED "
          END IF
        %>
        <input type="CHECKBOX" ID="KEY32" value="Y" <%=FREECODE1%> name="key32" <%=fieldRole(1)%><%=dataProtect%> 
        ID="Radio1"><FONT SIZE=2>新租用FTTB+HiNet經濟型10M(EN08+F004)</FONT><BR>
        <%IF DSPKEY(33)="Y" THEN
             FREECODE2=" CHECKED "
          ELSE
             DSPKEY(33)="Y"
             FREECODE2=" CHECKED "
          END IF
        %>
        <input type="CHECKBOX" ID="KEY33"  value="Y" <%=FREECODE2%> name="key33" <%=fieldRole(1)%><%=dataProtect%> 
        ID="Radio1"><FONT SIZE=2>終止租用Hibuilding，HN</FONT>
        <%
        IF DSPKEY(4)="" AND NOT RSXX.EOF THEN
           DSPKEY(4)=RSXX("CUSNO")
        END IF
        %>
        <input type="text" name="key4" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="8"
               value="<%=dspKey(4)%>"  READONLY size="8" class=dataListDATA>
        </td>                                 
         <td  bgcolor="silver" height="23" style="vertical-align:top">
         <%IF DSPKEY(34)="Y" THEN
             FREECODE3=" CHECKED "
          ELSE
             FREECODE3=""
          END IF
        %>
         <input type="CHECKBOX" ID="KEY34" value="Y" <%=FREECODE3%> name="key34" <%=fieldRole(1)%><%=dataProtect%> 
        ID="Radio1"><FONT SIZE=2>租用MOD (MD41)</FONT><BR>
        <%IF DSPKEY(35)="Y" THEN
             FREECODE4=" CHECKED "
          ELSE
             FREECODE4=""
          END IF
        %>
        <input type="CHECKBOX" ID="KEY35" value="Y" <%=FREECODE4%> name="key35" <%=fieldRole(1)%><%=dataProtect%> 
        ID="Radio1"><FONT SIZE=2>租用PLC 2個(001P)</FONT>
         </td>                                 
        <td  bgcolor="silver" height="23" style="vertical-align:top">
        <%IF DSPKEY(36)="Y" THEN
             FREECODE5=" CHECKED "
          ELSE
             FREECODE5=""
          END IF
        %>
        <input type="CHECKBOX" ID="KEY36" value="Y" <%=FREECODE5%> name="key36" <%=fieldRole(1)%><%=dataProtect%> 
        ID="Radio1"><FONT SIZE=2>保留原HiNet撥接帳號(HN</FONT>
        <input type="text" name="key37" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="8"
               value="<%=dspKey(37)%>"   size="8" class=dataListEntry>
               <FONT SIZE=2>)，同客戶名稱</FONT>
               <BR>
        <%IF DSPKEY(38)="Y" THEN
             FREECODE6=" CHECKED "
          ELSE
             FREECODE6=""
          END IF
        %>
        <input type="CHECKBOX" ID="KEY38" value="Y" <%=FREECODE6%> name="key38" <%=fieldRole(1)%><%=dataProtect%> 
        ID="Radio1"><FONT SIZE=2>新申請HiNet撥接帳號</FONT><br>
        <%IF DSPKEY(39)="Y" THEN
             FREECODE7=" CHECKED "
          ELSE
             FREECODE7=""
          END IF
        %>
        <input type="CHECKBOX" ID="KEY39" DISABLED value="Y" <%=FREECODE7%> name="key39" <%=fieldRole(1)%><%=dataProtect%> 
        ID="Radio1"><FONT SIZE=2>保留原Hibuilding mail,Email︰</FONT>
        <input type="text" name="key40" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="8"
               value="<%=dspKey(40)%>"   size="30" class=dataListDATA>
               <BR>
        <%IF DSPKEY(41)="Y" THEN
             FREECODE8=" CHECKED "
          ELSE
             FREECODE8=""
          END IF
        %>
        <input type="CHECKBOX" ID="KEY41" value="Y" <%=FREECODE8%> name="key41" <%=fieldRole(1)%><%=dataProtect%> 
        ID="Radio1"><FONT SIZE=2>保留網頁空間(my web)</FONT>
        </td>                         
</tr>   
 </table>
 </td></tr>
<tr><td width="10%" class=dataListHEAD ROWSPAN=2>電路編號</td>
    <td  width="23%"  bgcolor="silver" ROWSPAN=2>
        &nbsp;</td>
    <td width="10%" class=dataListHEAD>聯絡電話</td>
    <td  width="23%"  bgcolor="silver" COLSPAN=3><FONT SIZE=2>市內電話︰</FONT>
    <%
        IF DSPKEY(8)="" AND NOT RSXX.EOF THEN
           DSPKEY(8)=RSXX("HOME")
        END IF
    %>
        <input type="text" name="key8" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="15"
               value="<%=dspKey(8)%>"  size="15" class=dataListENTRY ID="Text22">
    <%
        IF DSPKEY(6)="" AND NOT RSXX.EOF THEN
           DSPKEY(6)=RSXX("OFFICE")
        END IF
    %>               
        <input type="text" name="key6" STYLE="DISPLAY:NONE" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="15"
               value="<%=dspKey(6)%>"  size="15" class=dataListENTRY ID="Text22">
    <%
        IF DSPKEY(7)="" AND NOT RSXX.EOF THEN
           DSPKEY(7)=RSXX("EXTENSION")
        END IF
    %>                  
        <input type="text" name="key7" STYLE="DISPLAY:NONE"  <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="6"
               value="<%=dspKey(7)%>"  size="6" class=dataListENTRY ID="Text22">                              
        <FONT SIZE=2>行動電話︰</FONT>
    <%
        IF DSPKEY(9)="" AND NOT RSXX.EOF THEN
           DSPKEY(9)=RSXX("MOBILE")
        END IF
    %>           
        <input type="text" name="key9" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="15"
               value="<%=dspKey(9)%>"  size="15" class=dataListENTRY ID="Text22"></td>
</TR>
<tr>
    <td width="10%" class=dataListHEAD>聯絡人</td>
    <td  width="23%"  bgcolor="silver" COLSPAN=3>
        <input type="text" name="key10" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="12"
               value="<%=dspKey(10)%>"  size="12" class=dataListENTRY ID="Text22">
        <FONT SIZE=2>EMAIL︰</FONT>
        <input type="text" name="key11" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="30"
               value="<%=dspKey(11)%>"  size="30" class=dataListENTRY ID="Text22"></td>
</TR>

<tr><td width="10%" class=dataListHEAD ROWSPAN=2>客戶名稱</td>
    <td  width="23%"  bgcolor="silver" ROWSPAN=2>
    <%
        IF DSPKEY(3)="" AND NOT RSXX.EOF THEN
           DSPKEY(3)=RSXX("CUSNC")
        END IF
      '  response.write rsxx("cusnc") & ";" & rsxx("cutid1")& ";" & rsxx("cutid2")& ";" & rsxx("cutid3")
    %>       
        <input type="text" name="key3" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="30"
               value="<%=dspKey(3)%>"  size="30" class=dataListENTRY ID="Text22"></td>
<td width="10%" class=dataListHEAD >客戶生日</td>
    <td  width="23%"  bgcolor="silver" >
    <%
        IF DSPKEY(12)="" AND NOT RSXX.EOF THEN
           DSPKEY(12)=RSXX("BIRTHDAY")
        END IF
    %>       
        <input type="text" name="key12" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(12)%>"  size="10" class=dataListENTRY ID="Text22">
<input type="button" id="B12"  <%=fieldpb%>  name="B12" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C12"  name="C12"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">          
        </td>
<%  IF DSPKEY(13)="" AND NOT RSXX.EOF THEN
       DSPKEY(13)=RSXX("IDNUMBERTYPE")
    END IF
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) and len(trim(dspkey(31)))=0 and len(trim(dspkey(43)))=0 Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='J5' AND CODE IN ('01','02')" 
       If len(trim(dspkey(13))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='J5' AND CODE='" & dspkey(13) &"' " 
       'SXX60=""
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(第一證照別)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(13) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>        
              
<td width="9%" class=dataListHEAD>身分證(統編)</td>
    <td width="23%" bgcolor="silver" >
	<select size="1" name="key13"<%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry"><%=s%></select>    
          <%
        IF DSPKEY(14)="" AND NOT RSXX.EOF THEN
           DSPKEY(14)=RSXX("IDNUMBER")
        END IF
    %>   
        <input type="password" name="key14" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(14)%>"   size="12" class=dataListENTRY ID="Text23"></td> 
</TR>
<TR>    
<td  class="dataListHEAD" height="23">公司負責人</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" name="key17" size="12" maxlength="12" value="<%=dspKey(17)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text16">
</td>                                 
                            
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) and len(trim(dspkey(31)))=0  and len(trim(dspkey(43)))=0 Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='L3' " 
       If len(trim(dspkey(15))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='L3' AND CODE='" & dspkey(15) &"' " 
       'SXX60=""
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(第二證照別)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(15) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>        
        <td width="9%" class="dataListHead" height="25">第二證照號碼</td>
        <td width="25%" height="25" bgcolor="silver" > 
		<select size="1" name="key15"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select16"><%=s%></select>	
        <input type="password" name="key16" size="15" maxlength="12" value="<%=dspkey(16)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text49"></td> 
     </tr>    
<tr><td class=dataListHEAD>裝機地址</td>
    <td bgcolor="silver" COLSPAN=3>
  <%  '  response.write "dspkey(18)=" & dspkey(18)
        IF DSPKEY(18)="" AND NOT RSXX.EOF THEN
           DSPKEY(18)=RSXX("CUTID1")
        END IF
  '  response.write "dspkey(18)=" & dspkey(18) & ";cutid1=" & rsxx("cutid1")
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) OR accessmode="" ) and len(trim(dspkey(31)))=0 and len(trim(dspkey(43)))=0 Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(18))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX19=" onclick=""Srcounty19onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(18) & "' " 
       SXX19=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(18) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
         <select size="1" name="key18" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select2" ><%=s%></select>
            <%
        IF DSPKEY(19)="" AND NOT RSXX.EOF THEN
           DSPKEY(19)=RSXX("TOWNSHIP1")
        END IF
    %>   
        <input type="text" name="key19" readonly  size="8" value="<%=dspkey(19)%>" maxlength="10" readonly <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text4"><font SIZE=2>(鄉鎮)                 
         <input type="button" id="B19" <%=fieldpb%> name="B19"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX19%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C19"  name="C19"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
      <%
        IF DSPKEY(20)="" AND NOT RSXX.EOF THEN
           DSPKEY(20)=RSXX("RADDR1")
        END IF
    %>   
        <input type="text" name="key20"  size="50" value="<%=dspkey(20)%>" maxlength="60"  <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListENTRY" ID="Text4">
            <%
        IF DSPKEY(21)="" AND NOT RSXX.EOF THEN
           DSPKEY(21)=RSXX("RZONE1")
        END IF
    %>   
        <input type="text" name="key21" readonly  size="5" value="<%=dspkey(21)%>" maxlength="5" readonly <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text4">
        </td>                                 
<td  class="dataListHEAD" height="23">裝址電話</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" name="key49" size="15" maxlength="15" value="<%=dspKey(49)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text16">
</td>   </tr>  

<tr><td class=dataListHEAD>帳單地址</td>
    <td bgcolor="silver" COLSPAN=3>
  <%

        IF DSPKEY(22)="" AND NOT RSXX.EOF THEN
           DSPKEY(22)=RSXX("CUTID2")
        END IF

  s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) OR accessmode="") and len(trim(dspkey(31)))=0 and len(trim(dspkey(43)))=0  Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(22))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX23=" onclick=""Srcounty23onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(22) & "' " 
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
        <input type="radio" name="rd1"  <%=fieldpb%> onClick="SrAddrEqual1()"><font SIZE=2>同裝機地址</font>
 <select size="1" name="key22" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select2" ><%=s%></select>
    <%
        IF DSPKEY(23)="" AND NOT RSXX.EOF THEN
           DSPKEY(23)=RSXX("TOWNSHIP2")
        END IF
    %>         
        <input type="text" name="key23" readonly  size="8" value="<%=dspkey(23)%>" maxlength="10" readonly <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text4"><font SIZE=2>(鄉鎮)                 
         <input type="button" id="B23" <%=fieldpb%> name="B23"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX23%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C23"  name="C23   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
    <%
        IF DSPKEY(24)="" AND NOT RSXX.EOF THEN
           DSPKEY(24)=RSXX("RADDR2")
        END IF
    %>           
        <input type="text" name="key24"  size="50" value="<%=dspkey(24)%>" maxlength="60"  <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListENTRY" ID="Text4">
    <%
        IF DSPKEY(25)="" AND NOT RSXX.EOF THEN
           DSPKEY(25)=RSXX("RZONE2")
        END IF
    %>          
       <input type="text" name="key25" readonly  size="5" value="<%=dspkey(25)%>" maxlength="5" readonly <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text4">
        </td>                                 
<td  class="dataListHEAD" height="23">併帳電話</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" name="key50" size="15" maxlength="15" value="<%=dspKey(50)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text16">
</td>  
</tr>  
<tr><td class=dataListHEAD>戶籍地址</td>
    <td bgcolor="silver" COLSPAN=5>
  <%

        IF DSPKEY(26)="" AND NOT RSXX.EOF THEN
           DSPKEY(26)=RSXX("CUTID3")
        END IF
  
  s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) OR accessmode="") and len(trim(dspkey(31)))=0 and len(trim(dspkey(43)))=0 Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(26))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX27=" onclick=""Srcounty27onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(26) & "' " 
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
<input type="radio" name="rd2"  <%=fieldpb%> onClick="SrAddrEqual2()"><font SIZE=2>同裝機地址</font>
        <input type="radio" name="rd4"  <%=fieldpb%> onClick="SrAddrEqual3()"><font SIZE=2>同帳單地址</font>
         <select size="1" name="key26" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select2" ><%=s%></select>
    <%
        IF DSPKEY(27)="" AND NOT RSXX.EOF THEN
           DSPKEY(27)=RSXX("TOWNSHIP3")
        END IF
    %>          
        <input type="text" name="key27" readonly  size="8" value="<%=dspkey(27)%>" maxlength="10" readonly <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text4"><font SIZE=2>(鄉鎮)                 
         <input type="button" id="B27" <%=fieldpb%> name="B27"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX27%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C27"  name="C27"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
    <%
        IF DSPKEY(28)="" AND NOT RSXX.EOF THEN
           DSPKEY(28)=RSXX("RADDR3")
        END IF
    %>           
        <input type="text" name="key28"  size="50" value="<%=dspkey(28)%>" maxlength="60"  <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListENTRY" ID="Text4">
    <%
        IF DSPKEY(29)="" AND NOT RSXX.EOF THEN
           DSPKEY(29)=RSXX("RZONE3")
        END IF
    %>         
        <input type="text" name="key29" readonly  size="5" value="<%=dspkey(29)%>" maxlength="5" readonly <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text4">
        
</td>                                 
</tr>  

<tr>
        <td  class="dataListHEAD" height="23">建檔人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(45) <> "" then
              sql=" select rtobj.cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(45) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>    <input type="text" name="key45" size="6" READONLY value="<%=dspKey(45)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">建檔日期</td>                                 
        <td  height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key46" size="10" READONLY value="<%=dspKey(46)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>  
<tr>
        <td  class="dataListHEAD" height="23">修改人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(47) <> "" then
              sql=" select rtobj.cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(47) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>    <input type="text" name="key47" size="6" READONLY value="<%=dspKey(47)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">修改日期</td>                                 
        <td  height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key48" size="10" READONLY value="<%=dspKey(48)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>         
</table> </div>

    <DIV ID="SRTAG1" onclick="SRTAG1" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="CENTER">進度狀態</td></tr></table></DIV>
   <DIV ID="SRTAB1" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
<tr>
<td width="10%" class=dataListHEAD >送件日期</td>
    <td  width="23%"  bgcolor="silver" >
        <input type="text" name="key31" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(31)%>"  READONLY size="10" class=dataListENTRY ID="Text22">
<input type="button" id="B31"  <%=fieldpb%>  name="B31" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"   alt="清除" id="C31"  name="C31"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">          
        </td>
<td width="10%" class=dataListHEAD >作廢日期</td>
    <td  width="23%"  bgcolor="silver" >
        <input type="text" name="key43" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(43)%>"  size="10" class=dataListDATA ID="Text22">
        </td>
<td  class="dataListHEAD" height="23">作廢人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(51) <> "" then
              sql=" select rtobj.cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(51) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>    <input type="text" name="key51" size="6" READONLY value="<%=dspKey(51)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
</TR>
 </table> 
</div>

    <DIV ID="SRTAG2" onclick="SRTAG2" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="CENTER">備註說明</td></tr></table></DIV>
   <DIV ID="SRTAB2" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
    <TR><TD align="CENTER">
     <TEXTAREA  cols="100%"  name="key44" rows=8  MAXLENGTH=500  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(44)%>" ID="Textarea1"><%=dspkey(44)%></TEXTAREA>
   </td></tr>
 </table> 
</div>
<%
    RSXX.CLOSE
    CONNXX.CLOSE
    SET RSXX=NOTHING
    SET CONNXX=NOTHING
%>
  <% Set rsxx=Server.CreateObject("ADODB.Recordset")
     sqlfaqlist="SELECT COUNT(*) AS CNT FROM RTLessorCustFaqH WHERE CUSID='" & DSPKEY(2) & "' AND CANCELDAT IS NULL "
     rsxx.open sqlfaqlist,conn
     IF RSXX("CNT") > 0 THEN
        XXFAQFLAG="Y"
     ELSE
        XXFAQFLAG=""
     END IF
     RSXX.CLOSE
     SET RSXX=NOTHING
   %>
    <% if XXFAQFLAG = "Y" then %>
  <DIV ID="SRTAG6" onclick="SRTAG6" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="CENTER">客戶服務單</td></tr></table></DIV>
    <DIV ID="SRTAB6" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr class="dataListsearch"><td colspan=15 align=center><A HREF="/webap/rtap/BASE/RTlessorCMTY/RTLessorCustFaqK.asp?V=<%=XRND%>&accessMode=U&key=<%=DSPKEY(0)%>;<%=DSPKEY(1)%>;<%=DSPKEY(2)%>;" TARGET="NEWWINDOW" ><FONT COLOR=WHITE>客戶服務單明細</FONT></A></td></tr>
    <tr class="dataListHEAD" align=center><td>客服單號</td><td>來電日</td><td>類型</td><td>摘要</td><td>連絡電話</td><td>行動電話</td><td>派工日</td><td>派工員</td><td>派工單號</td><td>派工結案</td><td>客服回覆</td><td>回覆員</td><td>客服結案</td><td>結案員</td><td>處理<br>天數</td></tr>
    <%
           Set rsxx=Server.CreateObject("ADODB.Recordset")
           sqlfaqlist="SELECT RTLessorCustFaqH.CUSID,RTLessorCustFaqH.FAQNO,RTLessorCustFaqH.RCVDAT, RTCode.CODENC, " _
                     &"LEFT(RTLessorCustFaqH.MEMO, 15) AS memo15,RTLessorCustFaqH.CONTACTTEL, RTLessorCustFaqH.MOBILE, " _
                     &"RTLessorCustFaqH.SNDWORK, RTObj_4.CUSNC AS CUSNC1, RTLessorCustFaqH.SNDPRTNO, " _
                     &"RTLessorCustFaqH.SNDCLOSEDAT, RTLessorCustFaqH.CALLBACKDAT, " _
                     &"RTObj_5.CUSNC AS CUSNC2, RTLessorCustFaqH.FINISHDAT, RTObj_6.CUSNC AS CUSNC3, " _
                     &"RTObj_1.CUSNC AS CUSNC4, RTObj_2.CUSNC AS CUSNC5, " _
                     &"RTObj_3.CUSNC AS CUSNC6, CASE WHEN RTLessorCustFaqH.finishdat IS NULL THEN " _
                     &"datediff(dd, RTLessorCustFaqH.rcvdat, getdate()) + 1 ELSE " _
                     &"datediff(dd, RTLessorCustFaqH.rcvdat, RTLessorCustFaqH.finishdat) + 1 END AS PROCESSDAT " _
                     &"FROM RTEmployee RTEmployee_5 INNER JOIN RTObj RTObj_5 ON RTEmployee_5.CUSID = RTObj_5.CUSID " _
                     &"RIGHT OUTER JOIN RTLessorCustFaqH ON RTEmployee_5.EMPLY = RTLessorCustFaqH.CALLBACKUSR " _
                     &"LEFT OUTER JOIN RTEmployee RTEmployee_4 INNER JOIN RTObj RTObj_4 ON RTEmployee_4.CUSID = " _
                     &"RTObj_4.CUSID ON RTLessorCustFaqH.SNDUSR = RTEmployee_4.EMPLY LEFT OUTER JOIN " _
                     &"RTEmployee RTEmployee_3 INNER JOIN RTObj RTObj_3 ON RTEmployee_3.CUSID = RTObj_3.CUSID ON " _
                     &"RTLessorCustFaqH.UUSR = RTEmployee_3.EMPLY LEFT OUTER JOIN RTEmployee RTEmployee_2 INNER JOIN " _
                     &"RTObj RTObj_2 ON RTEmployee_2.CUSID = RTObj_2.CUSID ON RTLessorCustFaqH.EUSR = RTEmployee_2.EMPLY " _
                     &"LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN RTObj RTObj_1 ON RTEmployee_1.CUSID = " _
                     &"RTObj_1.CUSID ON RTLessorCustFaqH.CANCELUSR = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
                     &"RTObj RTObj_6 INNER JOIN RTEmployee RTEmployee_6 ON RTObj_6.CUSID = RTEmployee_6.CUSID ON " _
                     &"RTLessorCustFaqH.FUSR = RTEmployee_6.EMPLY LEFT OUTER JOIN RTCode ON " _
                     &"RTLessorCustFaqH.SERVICETYPE = RTCode.CODE AND RTCode.KIND = 'N4' " _
                     &"WHERE    RTLessorCustFaqH.cusid = '" & DSPKEY(2) & "' AND RTLessorCustFaqH.canceldat IS NULL " 
           rsxx.open sqlfaqlist,conn
           Randomize
           XRND=RND()
           do until rsxx.eof %>
           <tr class="dataListentry" >
           <td><A HREF="/webap/rtap/BASE/RTlessorCMTY/RTLessorCustFaqD.asp?V=<%=XRND%>&accessMode=U&key=<%=RSXX("CUSID")%>;<%=RSXX("FAQNO")%>;" TARGET="NEWWINDOW" ><%=RSXX("FAQNO")%></A></td>
           <td align=center><%=rsxx("RCVDAT")%>&nbsp;</td>
           <td align=center><%=rsxx("CODENC")%>&nbsp;</td>
           <td><%=rsxx("memo15")%>&nbsp;</td>
           <td><%=rsxx("CONTACTTEL")%>&nbsp;</td>
           <td><%=rsxx("MOBILE")%>&nbsp;</td>
           <td align=center>
           <%
           if isnull(rsxx("SNDWORK")) then
              xxSNDWORK=""
           else
              xxSNDWORK=datevalue(rsxx("SNDWORK"))
           end if
           %>
           <%=xxSNDWORK%>&nbsp;</td>
           <td align=center><%=rsxx("CUSNC1")%>&nbsp;</td>
           <td>
           <% if len(trim(RSXX("SNDPRTNO"))) > 0 then %>
           <A HREF="/webap/rtap/BASE/RTlessorCMTY/RTLessorCustfaqsndworkD.asp?V=<%=XRND%>&accessMode=U&key=<%=RSXX("CUSID")%>;<%=RSXX("faqno")%>;<%=RSXX("SNDPRTNO")%>" TARGET="NEWWINDOW" ><%=rsxx("SNDPRTNO")%>&nbsp;</a>
           <% else %>
           &nbsp;
           <%end if %>
           </td>
           <td align=center>
           <%
           if isnull(rsxx("SNDclosedat")) then
              xxsndclosedat=""
           else
              xxsndclosedat=datevalue(rsxx("SNDclosedat"))
           end if
           %>
           <%=xxsndclosedat%>&nbsp;</td>
           <td align=center>
           <%
           if isnull(rsxx("CALLBACKDAT")) then
              xxcallbackdat=""
           else
              xxcallbackdat=datevalue(rsxx("CALLBACKDAT"))
           end if
           %>
           <%=xxcallbackdat%>&nbsp;</td>
           <td align=center><%=rsxx("CUSNC2")%>&nbsp;</td>
           <td align=center>
           <%
           if isnull(rsxx("FINISHDAT")) then
              xxFINISHDAT=""
           else
              xxFINISHDAT=datevalue(rsxx("FINISHDAT"))
           end if
           %>
           <%=xxFINISHDAT%>&nbsp;</td>
           <td align=center><%=rsxx("CUSNC3")%>&nbsp;</td>
           <td align=right><%=rsxx("PROCESSDAT")%>&nbsp;</td>
           </tr>           
        <% rsxx.MoveNext
           loop    
           rsxx.close
           set rsxx=nothing
      %>
      </table>
  </div>   
 <%END IF %>

  <% Set rsxx=Server.CreateObject("ADODB.Recordset")
     sqlfaqlist="SELECT COUNT(*) AS CNT FROM RTLessorCustAR WHERE CUSID='" & DSPKEY(2) & "' AND CANCELDAT IS NULL "
     rsxx.open sqlfaqlist,conn
     IF RSXX("CNT") > 0 THEN
        XXarLAG="Y"
     ELSE
        XXarLAG=""
     END IF
     RSXX.CLOSE
     SET RSXX=NOTHING
   %>
   <% if XXarLAG = "Y" then %>
  <DIV ID="SRTAG9" onclick="SRTAG9" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr><td bgcolor="BDB76B" align="CENTER">客戶應收付帳款</td></tr></table></DIV>
    <DIV ID="SRTAB9" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr class="dataListsearch"><td colspan=11 align=center><A HREF="/webap/rtap/BASE/RTlessorCMTY/RTLessorCustARK.asp?V=<%=XRND%>&accessMode=U&key=<%=DSPKEY(0)%>;<%=DSPKEY(1)%>;<%=DSPKEY(2)%>;" TARGET="NEWWINDOW" ><FONT COLOR=WHITE>應收應付帳款明細</FONT></A></td></tr>
    <tr class="dataListHEAD" align=center><td>帳款編號</td><td>產生日期</td><td>AR/AP</td><td>明細數</td><td colspan=3>應沖/已沖/未沖金額</td><td>沖立項一</td><td>沖立項二</td><td>沖帳日</td></tr>
    <%
           Set rsxx=Server.CreateObject("ADODB.Recordset")
           sqlfaqlist="SELECT RTLessorCustAR.CUSID, RTLessorCustAR.BATCHNO, RTCode.CODENC, RTLessorCustAR.PERIOD, " _
                     &"RTLessorCustAR.AMT, RTLessorCustAR.REALAMT, " _
                     &"CASE WHEN RTLessorCustAR.CANCELDAT IS NOT NULL THEN 0 ELSE RTLessorCustAR.AMT - RTLessorCustAR.REALAMT END AS DIFFAMT, " _ 
                     &"RTLessorCustAR.COD1, RTLessorCustAR.COD2, RTLessorCustAR.COD3, RTLessorCustAR.COD4, RTLessorCustAR.COD5," _
                     &"RTLessorCustAR.CDAT, RTLessorCustAR.MDAT, RTLessorCustAR.CANCELDAT " _
                     &"FROM RTLessorCustAR LEFT OUTER JOIN RTCode ON RTLessorCustAR.ARTYPE = RTCode.CODE AND " _
                     &"RTCode.KIND = 'N2' " _
                     &"WHERE    RTLessorCustAR.cusid = '" & DSPKEY(2) & "' AND RTLessorCustAR.canceldat IS NULL "  _
                     &"ORDER BY RTLessorCustAR.CDAT "
           rsxx.open sqlfaqlist,conn
           Randomize
           XRND=RND()
           do until rsxx.eof %>
           <tr class="dataListentry">
           <td align=center><A HREF="/webap/rtap/BASE/RTlessorCMTY/RTLessorCustARDTLK.ASP?V=<%=XRND%>&key=<%=RSXX("CUSID")%>;<%=RSXX("BATCHNO")%>;" TARGET="NEWWINDOW" ><%=RSXX("BATCHNO")%></A></td>
           <td align=center>
           <%
           if isnull(rsxx("CDAT")) then
              xxCDAT=""
           else
              xxCDAT=datevalue(rsxx("CDAT"))
           end if
           %>
           <%=xxCDAT%>&nbsp;</td>
           <td align=center><%=rsxx("CODENC")%>&nbsp;</td>
           <td align=right><%=rsxx("PERIOD")%>&nbsp;</td>
           <td align=right><%=rsxx("AMT")%>&nbsp;</td>
           <td align=right><%=rsxx("REALAMT")%>&nbsp;</td>
           <td align=right><%=rsxx("DIFFAMT")%>&nbsp;</td>
           <td><%=rsxx("COD1")%>&nbsp;</td>
           <td><%=rsxx("COD2")%>&nbsp;</td>
            <td align=center>
            <%
           if isnull(rsxx("mDAT")) then
              xxmDAT=""
           else
              xxmDAT=datevalue(rsxx("mDAT"))
           end if
           %>
           <%=xxmDAT%>&nbsp;</td>
           </tr>           
        <% rsxx.MoveNext
           loop    
           rsxx.close
           set rsxx=nothing
      %>
      </table>
  </div>   
 <%END IF %> 
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