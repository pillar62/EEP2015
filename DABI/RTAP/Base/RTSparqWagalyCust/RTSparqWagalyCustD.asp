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
                   case ucase("/webap/rtap/base/RTSparqWagalyCust/RTSparqWagalyCustD.asp")
      'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       if i <> 0 then rs.Fields(i).Value=dspKey(i)    
                       if i=0 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         cusidxx="F" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
                         'cusidxx=trim(datePART("yyyy",NOW())-1911) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
                         rsc.open "select max(cusid) AS cusid from RTSparqWagalyCust where cusid like '" & cusidxx & "%' " ,conn
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
                 case ucase("/webap/rtap/base/RTSparqWagalyCust/RTSparqWagalyCustD.asp")
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
       if ucase(runpgm)=ucase("/webap/rtap/base/RTSparqWagalyCust/RTSparqWagalyCustD.asp") then
          cusidxx="F" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
          rsc.open "select max(cusid) AS cusid from RTSparqWagalyCust where cusid like '" & cusidxx & "%' " ,conn
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
<meta http-equiv="Content-Type" content="text/html; charset=big5">
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
  title="Sparq網路電話用戶資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB=	"SELECT CUSID, CUSNC, FIRSTIDTYPE, SOCIALID, SECONDIDTYPE, SECONDNO, EMAIL, " &_
                "CONTACTTEL, MOBILE, FAX, CUTID1, TOWNSHIP1, RADDR1, RZONE1, CUTID2, " &_
                "TOWNSHIP2, RADDR2, RZONE2, UUSR, UDAT, SALESID, CONSIGNEE, " &_
                "NCICCUSNO, SPHNNO, APPLYDAT, FINISHDAT, DOCKETDAT, TRANSDAT, " &_
                "DROPDAT, CANCELDAT, CANCELUSR, FREECODE, OVERDUE, MEMO, CASETYPE, TEL070, IPADDR " &_
                "FROM RTSparqWagalyCust WHERE CUSID='' "
  sqlList=	"SELECT CUSID, CUSNC, FIRSTIDTYPE, SOCIALID, SECONDIDTYPE, SECONDNO, EMAIL, " &_
			"CONTACTTEL, MOBILE, FAX, CUTID1, TOWNSHIP1, RADDR1, RZONE1, CUTID2, " &_
			"TOWNSHIP2, RADDR2, RZONE2, UUSR, UDAT, SALESID, CONSIGNEE, " &_
			"NCICCUSNO, SPHNNO, APPLYDAT, FINISHDAT, DOCKETDAT, TRANSDAT, " &_
			"DROPDAT, CANCELDAT, CANCELUSR, FREECODE, OVERDUE, MEMO, CASETYPE, TEL070, IPADDR " &_
			"FROM RTSparqWagalyCust WHERE "
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
  LEADINGCHAR=LEFT(DSPKEY(2),1)
  IF LEADINGCHAR >="0" AND LEADINGCHAR <="9" THEN
     COMPANY="Y"
  ELSE
     COMPANT="N"
  END IF
  '身份證第一碼大寫
  DSPKEY(2)=UCASE(DSPKEY(2))

'  IF instr(1,dspkey(67),"-",1) <> 0 THEN
'  RESPONSE.Write "AAA=" & instr(1,dspkey(67),"-",1) & "<BR>"
'  RESPONSE.Write "BBB=" & instr(1,dspkey(69),"-",1) 
'  RESPONSE.END
'  ELSE
'  RESPOSNE.WRITE "XXX"
'  RESPONSE.End
'  END IF

  if len(trim(dspkey(24)))=0 then
       formValid=False
       message="用戶申請日不可空白"   
  elseif len(trim(dspkey(1)))=0 then
       formValid=False
       message="用戶名稱不可空白"          
  '公關機時不檢查身份證
  elseif len(trim(dspkey(2)))=0 AND DSPKEY(31) <>"Y" then
       formValid=False
       message="第一證照別不可空白"             
  elseif len(trim(dspkey(3)))<>10 and len(trim(dspkey(3)))<>8  AND DSPKEY(31) <>"Y" then
       formValid=False
       message="第一證照號碼長度不對"    
  'elseif len(trim(dspkey(4)))=0  AND DSPKEY(31) <>"Y" then
  '     formValid=False
  '     message="第二證照別不可空白"             
  'elseif len(trim(dspkey(5)))<>10) and len(trim(dspkey(5)))<>8 ) AND DSPKEY(31) <>"Y" then
  '     formValid=False
  '     message="第二證照號碼長度不對"    
  elseif len(trim(dspkey(14)))=0 then
       formValid=False
       message="裝機地址(縣市)不可空白"   
  elseif len(trim(dspkey(15)))=0 and dspkey(14)<>"06" and dspkey(14)<>"15" then  
       formValid=False
       message="裝機地址(鄉鎮)不可空白"
  elseif len(trim(dspkey(16)))=0 then
       formValid=False
       message="裝機地址(地址)不可空白"

  elseif len(trim(dspkey(10)))=0 then
       formValid=False
       message="帳單地址(縣市)不可空白"   
  elseif len(trim(dspkey(11)))=0 and dspkey(10)<>"06" and dspkey(10)<>"15" then    
       formValid=False
       message="帳單地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(12)))=0 then
       formValid=False
       message="帳單地址(地址)不可空白"   

  elseif len(trim(dspkey(7)))=0 and len(trim(dspkey(8)))=0 then
       formValid=False
       message="用戶連絡電話及行動電話至少須輸入一項"   
  'elseif len(trim(dspkey(39)))= 0 then
  '     formValid=False
  '     message="方案種類不可空白"
  elseif len(trim(dspkey(20)))=0 and len(trim(dspkey(21))) = 0 then
       message="社區檔之經銷商欄位與業務員不可同時空白!"
       formValid=False
  elseif len(trim(dspkey(20))) <> 0 and len(trim(dspkey(21))) <> 0 then
       message="社區檔之經銷商與業務員請擇一填寫!"
       formValid=False
  'elseif len(trim(dspkey(40)))= 0 AND DSPKEY(38) <> "Y" then
  '     formValid=False
  '     message="AVS繳款方式不可空白"      
  'elseif len(trim(dspkey(40)))> 0 AND DSPKEY(38) = "Y" then
  '     formValid=False
  '     message="公關機時，AVS繳款方式必須空白"           
  elseif len(trim(dspkey(26)))<> 0 AND len(trim(dspkey(25)))= 0 then
       formValid=False
       message="完工日期為空白時不可輸入報竣日"       
  'elseif len(trim(dspkey(46)))<> 0 AND ( len(trim(dspkey(55)))= 0 or len(trim(dspkey(56)))= 0 or len(trim(dspkey(57)))= 0 or len(trim(dspkey(58)))= 0 )then
  '     formValid=False
  '     message="輸入完工日期時，用戶IP不可空白"              
  end if

  IF formValid=TRUE THEN
    IF dspkey(2) <> "" and (dspkey(2)="01" or dspkey(2)="02") then
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
    if dspmode="新增" or dspmode="修改" then
		Call SrGetEmployeeRef(Rtnvalue,1,logonid)
		V=split(rtnvalue,";")  
		DSpkey(18)=V(0)
        dspkey(19)=now()
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
			if clearkey = "KEY20" then
      			document.all("colAssigneng").value = ""
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
   Sub Srcounty12onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY10").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key11").value =  trim(Fusrid(0))
          document.all("key13").value =  trim(Fusrid(1))
       End if       
       end if
   End Sub       
   Sub Srcounty16onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY14").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key15").value =  trim(Fusrid(0))
          document.all("key17").value =  trim(Fusrid(1))
       End if       
       end if
    END SUB
   Sub Srsalesonclick()
       prog="RTGetsalesD.asp?KEY=;"
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
		  FUsrID=Split(Fusr,";")   
		  if Fusrid(2) ="Y" then
			 clickkey="KEY" & mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
			 document.all(clickkey).value = trim(Fusrid(0))			 
  			 if clickkey = "KEY20" then
         		document.all("colAssigneng").value = trim(Fusrid(1))
			 end if
		  End if       
       end if
   End Sub
   
Sub SrAddrEqual1()
    document.All("key14").value=document.All("key10").value
    document.All("key15").value=document.All("key11").value
    document.All("key16").value=document.All("key12").value
    document.All("key17").value=document.All("key13").value
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
    'logonid=session("userid")
    'if dspmode="新增" then
    '    if len(trim(dspkey(18))) < 1 then
    '       Call SrGetEmployeeRef(Rtnvalue,1,logonid)
    '            V=split(rtnvalue,";")  
    '            dspkey(18)=V(0)
    '    End if  
    '   dspkey(19)=now()
    'else
    '    if len(trim(dspkey(18))) < 1 then
    '       Call SrGetEmployeeRef(Rtnvalue,1,logonid)
    '            V=split(rtnvalue,";")  
                'DSpkey(18)=V(0)
    '    End if         
        'dspkey(19)=now()
    'end if      
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
    '完工後基本資料PROTECT
    If len(trim(dspKey(25))) > 0 Then
       fieldPa=" class=""dataListData"" readonly "
       fieldpb=" disabled "
    Else
       fieldPa=""
       fieldpb=""
    End If
    '報竣日輸入後，寬頻服務+代理人+績效資料PROTECT
    If len(trim(dspKey(26))) > 0 Then
       fieldPC=" class=""dataListData"" readonly "
       fieldpD=" disabled "
    Else
       fieldPC=""
       fieldpD=""
    End If
    '報竣轉檔後，報竣日期PROTECT
    If len(trim(dspKey(28))) > 0 Then
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
<span id="tags1" class="dataListTagsOn">速博網路電話用戶資訊</span>
                                                            
<div class=dataListTagOn> 
	<table width="100%"><tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
		<tr><td>&nbsp;</td><td>

<DIV ID="SRTAG0" >
	<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
	<tr><td bgcolor="BDB76B" align="LEFT">基本資料</td></tr></table></DIV>
    
<DIV ID=SRTAB0>
	<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
	<tr><td width="15%" class=dataListHEAD>用戶(公司)名稱</td>
		<td width="35%" bgcolor="silver">
        <input type="text" name="key1" value="<%=dspKey(1)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" maxlength="30" size="30" class=dataListENTRY></td>

		<td width="15%" class=dataListHEAD>用戶申請日</td>
		<td width="35%" bgcolor="silver">
        <input type="text" name="key24" value="<%=dspKey(24)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" maxlength="10" READONLY size="10" class=dataListEntry ID="Text1">
		<input  type="button" id="B24" name="B24" <%=fieldpb%> height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
		<IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" id="C24" name="C24" <%=fieldpb%> alt="清除" style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>
	</tr>

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
		<input type="text" name="key3" value="<%=dspkey(3)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
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
        <input type="text" name="key7" size="20" maxlength="20" <%=fieldpa%> value="<%=dspKey(7)%>" <%=fieldRole(1)%> class="dataListEntry"></td>

        <td class="dataListHEAD" height="23">行動電話</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key8" size="30" maxlength="30" <%=fieldpa%> value="<%=dspKey(8)%>" <%=fieldRole(1)%> class="dataListEntry"</td></tr>

	<tr><td class="dataListHEAD" height="23">傳真</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key9" size="30" maxlength="30" <%=fieldpa%> value="<%=dspKey(9)%>" <%=fieldRole(1)%> class="dataListEntry"</td>

		<td class="dataListHEAD" height="23">連絡EMAIL</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key6" size="50" maxlength="50" <%=fieldpa%> value="<%=dspKey(6)%>" <%=fieldRole(1)%> class="dataListEntry"></td></tr>

<%
	s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND len(trim(DSPKEY(26))) = 0 Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(10))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX20=" onclick=""Srcounty12onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(10) & "' " 
       SXX20=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(10) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td class=dataListHEAD>帳單地址</td>
		<td bgcolor="silver" colspan=3>
		<select size="1" name="key10" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select1"><%=s%></select>
        <input type="text" name="key11" readonly size="8" value="<%=dspkey(11)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text3"><font SIZE=2>(鄉鎮)
			<input type="button" id="B11" <%=fieldpb%> name="B11" width="100%" style="Z-INDEX: 1" value="...." <%=SXX20%> >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C11"  name="C11"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >
        <input type="text" name="key12" <%=fieldpa%> size="40" value="<%=dspkey(12)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text4"><font size=2>
		<input type="text" name="key13" readonly size="5" value="<%=dspkey(13)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text5"></td>
	</tr>


<%
	s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND len(trim(DSPKEY(26))) = 0 Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(14))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX16=" onclick=""Srcounty16onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(14) & "' " 
       SXX16=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(14) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td class=dataListHEAD>裝機地址<br><input type="radio" name="rd1" <%=fieldpb%> onClick="SrAddrEqual1()" ID="Radio3" VALUE="Radio3">同戶籍</td>
		<td bgcolor="silver" colspan=3>
		<select size="1" name="key14" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry"><%=s%></select>
        <input type="text" name="key15" readonly  size="8" value="<%=dspkey(15)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA"><font SIZE=2>(鄉鎮)
			<input type="button" id="B15"  <%=fieldpb%>  name="B15"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX16%>  >        
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C15" name="C15" style="Z-INDEX: 1" onclick="SrClear" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">          
		<input type="text" name="key16" <%=fieldpa%> size="40" value="<%=dspkey(16)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><font size=2>
		<input type="text" name="key17"  readonly size="5" value="<%=dspkey(17)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA"></td>
	</tr>
	</table>
</div>

<DIV ID="SRTAG2" >
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="LEFT">方案資料</td></tr></table></div>
<DIV ID=SRTAB2>
	<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
	<tr><td WIDTH="15%" class="dataListHEAD" height="23">Ｔ帳號</td>
        <td WIDTH="35%" bgcolor="silver">
        	<input type="text" name="key22" size="15" maxlength="10" <%=fieldpa%> value="<%=dspKey(22)%>" <%=fieldRole(1)%><%=dataProtect%> class="dataListENTRY">
        	<FONT SIZE=2>-</FONT><input type="text" name="key23" size="3" maxlength="3" <%=fieldpa%> value="<%=dspKey(23)%>" <%=fieldRole(1)%> class="dataListDATA" READONLY> 
        </TD>

		<%
	    s=""
	    sx=" selected "
	    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
	       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='Q5' " 
	       If len(trim(dspkey(34))) < 1 Then
	          sx=" selected " 
	       else
	          sx=""
	       end if
	    Else
	       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='Q5' AND CODE='" & dspkey(34) &"' " 
	    End If
	    rs.Open sql,conn
	    s=""
	    s=s &"<option value=""" &"""" &sx &">(方案類別)</option>"
	    sx=""
	    Do While Not rs.Eof
	       If rs("CODE")=dspkey(34) Then sx=" selected "
	       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
	       rs.MoveNext
	       sx=""
	    Loop
	    rs.Close
		%>
		<td width="15%" class=dataListHEAD>方案類別</td>
		<td width="35%" bgcolor="silver">
			<select size="1" name="key34" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><%=s%></select>
		</td>
	</tr>

	<TR><td class="dataListHEAD" height="23">070電話號碼</td>
        <td bgcolor="silver">
        	<input type="text" name="key35" size="12" maxlength="11" <%=fieldpa%> value="<%=dspKey(35)%>" <%=fieldRole(1)%> class="dataListEntry">
		</td>

		<td class="dataListHEAD" height="23">IP位址</td>
        <td bgcolor="silver">
        	<input type="text" name="key36" size="18" maxlength="15" <%=fieldpa%> value="<%=dspKey(36)%>" <%=fieldRole(1)%> class="dataListEntry">
		</td>
	</tr>

	</table>
</DIV>

<!--	<table border="1" width="100%" cellpadding="0" cellspacing="0" id="tag2" style="display: none">		-->
<DIV ID="SRTAG1" >
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="LEFT">績效歸屬</td></tr></table></div>
<DIV ID=SRTAB1>
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table7">
<%
    s=""
    sx=""
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) and len(trim(dspkey(48)))=0  and FIELDROLE(1)="" Then 
		sql=	"select c.cusid, c.shortnc " &_
				"from RTConsignee a " &_
				"inner join RTConsigneeCASE b on a.CUSID = b.CUSID " &_
				"inner join RTObj c on c.cusid = a.cusid " &_
				"where	b.caseid ='00' order by c.shortnc " 
		s="<option value="""" >(經銷商)</option>"
	Else
		sql=	"select c.cusid, c.shortnc " &_
				"from RTConsignee a " &_
				"inner join RTObj c on c.cusid = a.cusid " &_
				"where	c.cusid='" & dspkey(21) & "' "
		s =""
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(21) Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td WIDTH="15%" class="dataListHEAD" height="23">經銷商</td>
        <td WIDTH="35%" height="21" bgcolor="silver" >
			<select size="1" name="KEY21" <%=fieldpg%><%=FIELDROLE(1)%> class="dataListEntry"><%=S%></select>
		</td>

        <td  WIDTH="15%" class="dataLISTSEARCH" height="23">公關機</td>                                 
		<%  
			dim FREECODE1,FREECODE2
			If Len(Trim(fieldRole(1) &dataProtect)) < 1 and flg = "Y" Then
			FREECODE1=""
			FREECODE2=""
			Else
			' sexd1=" disabled "
			' sexd2=" disabled "
			End If
			If dspKey(31)="Y" Then FREECODE1=" checked "    
			If dspKey(31)="N" or len(dspKey(31)) =0 Then FREECODE2=" checked " 
		%>                          
        <td  WIDTH="35%" height="23" bgcolor="silver" >
        <input type="radio" name="key31" value="Y" <%=FREECODE1%> <%=fieldRole(1)%><%=dataProtect%> ID="Radio4">是
        <input type="radio" name="key31" value="N" <%=FREECODE2%>  <%=fieldRole(1)%><%=dataProtect%> ID="Radio5">否</td>
	</tr>

	<tr><td WIDTH="15%" class="dataListHEAD" height="23">業務員</td>

		<td width="23%" bgcolor="silver" colspan=3>
			<input type="TEXT" name="key20" value="<%=dspKey(20)%>" size="6" readonly <%=fieldpa%> class="dataListEntry" ID="Text50">
			<input type="BUTTON" id="B20" name="B20" width="100%" onclick="Srsalesonclick()" <%=fieldpb%> style="Z-INDEX: 1"  value="...." >   
			<IMG alt="清除" id="C20" name="C20" onclick="SrClear" <%=fieldpb%> SRC="/WEBAP/IMAGE/IMGDELETE.GIF" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
			<input type="text" readonly size="10" name="colAssigneng" value="<%=SrGetEmployeeName(dspKey(20))%>" class="dataListsearch3" ID="Text21">
        </td>
	</tr>
	</table></DIV>


<DIV ID="SRTAG4">
    <table border="1" width="100%" cellpadding="0" cellspacing="0">
    <tr><td bgcolor="BDB76B" align="LEFT">用戶施工進度狀態</td></tr></table></DIV>

<DIV ID=SRTAB4 >
	<table border="1" width="100%" cellpadding="0" cellspacing="0">
	<tr><td class="dataListHEAD" height="23">完工日</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key25" size="10" READONLY value="<%=dspKey(25)%>" <%=fieldpC%>  <%=fieldRole(1)%> class="dataListentry">
			<input type="button" id="B25"  name="B25" height="100%" width="100%"  <%=fieldpD%> style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C25"  name="C25"    <%=fieldpD%> style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>

        <td class="dataListHEAD" height="23">報竣日</td>                                 
        <td height="23" bgcolor="silver">
		<input type="text" name="key26" size="10" READONLY value="<%=dspKey(26)%>" <%=fieldpe%> <%=fieldRole(1)%> class="dataListDATA">
			<input type="button" id="B26"  name="B26" height="100%" width="100%" <%=fieldpf%>style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C26"  name="C26"   <%=fieldpf%>style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
		</TD>
	</tr> 

	<tr><td class="dataListHEAD" height="23">轉檔日</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key27" size="10" value="<%=dspKey(27)%>" <%=fieldRole(1)%> class="dataListDATA"></td>

		<td class="dataListHEAD" height="23">退租日</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key28" size="10" value="<%=dspKey(28)%>" <%=fieldRole(1)%> class="dataListDATA">
			<input type="button" id="B28"  name="B28" height="100%" width="100%" <%=fieldpf%>style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C28"  name="C28"   <%=fieldpf%>style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">

        <font size=2>欠費︰</font>
        <input type="text" name="key32" size="2" READONLY value="<%=dspKey(32)%>" <%=fieldRole(1)%> class="dataListDATA"></td></tr>

	<tr><td width=15% class="dataListHEAD" height="23">作廢人員</td>                                 
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key30" size="6" value="<%=dspKey(30)%>" <%=fieldRole(1)%> readonly class="dataListDATA">
        	<font size=2><%=SrGetEmployeeName(dspKey(30))%></font>
        </td>

		<td width=15% class="dataListHEAD" height="23">作廢日期</td>                                 
        <td width=35% height="23" bgcolor="silver">
        <input type="text" name="key29" size="10" value="<%=dspKey(29)%>" <%=fieldpa%><%=fieldRole(1)%> readonly class="dataListdata">
			<input type="button" id="B29"  name="B29" height="100%" width="100%" <%=fieldpf%>style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C29"  name="C29"   <%=fieldpf%>style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
        </td>
	</tr>

	<tr><td class="dataListHEAD" height="23">修改人員</td>
        <td height="23" bgcolor="silver">
		<input type="text" name="key18" size="6" READONLY value="<%=dspKey(18)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2">
		<font size=2><%=SrGetEmployeeName(dspKey(18))%></font></td>

        <td class="dataListHEAD" height="23">修改日期</td>
        <td height="23" bgcolor="silver">
        <input type="text" name="key19" size="25" READONLY value="<%=dspKey(19)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text6"></td>
    </tr>
           
	</table>
</DIV>


<DIV ID="SRTAG6">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="LEFT">備註說明</td></tr></table></DIV>

<DIV ID="SRTAB6" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
    <TR><TD align="CENTER">
		<TEXTAREA cols="100%" name="key33" rows=8 MAXLENGTH=500 class="dataListentry" <%=dataprotect%> value="<%=dspkey(33)%>" ID="Textarea1"><%=dspkey(33)%></TEXTAREA></td></tr>
   </table></div> 

</DIV>
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
<!-- #include virtual="/Webap/include/RTGetUserRight.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
