<%  
if not Session("passed") then
   Response.Redirect "http://www.cbbn.com.tw/Consignee/logon.asp"
end if

  Dim fieldRole,fieldPa
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
                   case ucase("/webap/rtap/base/RTSparq499Cmty/RTSparq499Custd.asp")
                   ' response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       if i <> 2 then rs.Fields(i).Value=dspKey(i)    
                       if i=2 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         cusidxx="A" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
                         rsc.open "select max(cusid) AS cusid from RTSparq499Cust where cusid like '" & cusidxx & "%' " ,conn
                         if len(rsc("cusid")) > 0 then
                            dspkey(2)=cusidxx & right("0000" & cstr(cint(right(rsc("cusid"),4)) + 1),4)
                         else
                            dspkey(2)=cusidxx & "0001"
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
                 case ucase("/webap/rtap/base/RTSparq499Cmty/RTSparq499Custd.asp")
                    'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                     if i<>0 and i <> 1 then rs.Fields(i).Value=dspKey(i)         
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
       if ucase(runpgm)=ucase("/webap/rtap/base/RTSparq499Cmty/RTSparq499CustD.asp") then
          cusidxx="A" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
          rsc.open "select max(cusid) AS cusid from RTSparq499Cust where cusid like '" & cusidxx & "%' " ,conn
          if not rsC.eof then
            dspkey(2)=rsC("CUSID")
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
  title="速博499用戶資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT COMQ1, LINEQ1, CUSID, CUSNC, SOCIALID, SECONDIDTYPE, SECONDNO, BIRTHDAY, EMAIL, CONTACTTEL," _
             &"MOBILE, CUTID1, TOWNSHIP1, RADDR1,  RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, CUTID3, " _
             &"TOWNSHIP3, RADDR3, RZONE3, COCONTACT, COCONTACTTEL, COCONTACTTELEXT, COMOBILE, COBOSS, COBOSSSOCIAL, TRADETYPE, " _
             &"EUSR, EDAT, UUSR, UDAT, AREAID, GROUPID, SALESID, CASETYPE, FREECODE, PMCODE, " _
             &"PAYTYPE, AGENTNAME, AGENTSOCIAL, AGENTTEL, RCVD, APPLYDAT, FINISHDAT, DOCKETDAT, TRANSDAT, DROPDAT, " _
             &"CANCELDAT, CANCELUSR, OVERDUE, MEMO, MOVETOCOMQ1, MOVETOLINEQ1, MOVEFROMCOMQ1, MOVEFROMLINEQ1, MOVETODAT, MOVEFROMDAT, " _
             &"NCICCUSNO, CUSTIP1, CUSTIP2, CUSTIP3, CUSTIP4, FIRSTIDTYPE,sphnno, CREDITTYPE, CREDITBANK, CREDITNO, CREDITNAME, " _
			 &"VALIDMONTH, VALIDYEAR, DEVELOPERID "_
             &"from RTSparq499Cust WHERE COMQ1=0 "
  sqlList="SELECT COMQ1, LINEQ1, CUSID, CUSNC, SOCIALID, SECONDIDTYPE, SECONDNO, BIRTHDAY, EMAIL, CONTACTTEL," _
             &"MOBILE, CUTID1, TOWNSHIP1, RADDR1,  RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, CUTID3, " _
             &"TOWNSHIP3, RADDR3, RZONE3, COCONTACT, COCONTACTTEL, COCONTACTTELEXT, COMOBILE, COBOSS, COBOSSSOCIAL, TRADETYPE, " _
             &"EUSR, EDAT, UUSR, UDAT, AREAID, GROUPID, SALESID, CASETYPE, FREECODE, PMCODE, " _
             &"PAYTYPE, AGENTNAME, AGENTSOCIAL, AGENTTEL, RCVD, APPLYDAT, FINISHDAT, DOCKETDAT, TRANSDAT, DROPDAT, " _
             &"CANCELDAT, CANCELUSR, OVERDUE, MEMO, MOVETOCOMQ1, MOVETOLINEQ1, MOVEFROMCOMQ1, MOVEFROMLINEQ1, MOVETODAT, MOVEFROMDAT, " _
             &"NCICCUSNO, CUSTIP1, CUSTIP2, CUSTIP3, CUSTIP4, FIRSTIDTYPE,sphnno, CREDITTYPE, CREDITBANK, CREDITNO, CREDITNAME, " _
			 &"VALIDMONTH, VALIDYEAR, DEVELOPERID " _
             &"from RTSparq499Cust WHERE "
  userDefineRead="Yes"      
  userDefineSave="Yes"       
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
  if len(trim(dspkey(38)))=0 then dspkey(38)=""
  if len(trim(dspkey(54)))=0 then dspkey(54)=0
  if len(trim(dspkey(55)))=0 then dspkey(55)=0
  if len(trim(dspkey(56)))=0 then dspkey(56)=0
  if len(trim(dspkey(57)))=0 then dspkey(57)=0
  '身份證欄位第一碼,用以判別是個人還是公司,若為公司則出生日期必須空白,反之則不可空白
  LEADINGCHAR=LEFT(DSPKEY(4),1)
  IF LEADINGCHAR >="0" AND LEADINGCHAR <="9" THEN
     COMPANY="Y"
  ELSE
     COMPANT="N"
  END IF
  '身份證第一碼大寫
  DSPKEY(4)=UCASE(DSPKEY(4))

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
  elseif len(trim(dspkey(3)))=0 then
       formValid=False
       message="用戶名稱不可空白"          
  '公關機時不檢查身份證
  elseif ( len(trim(dspkey(4)))=0 or (len(trim(dspkey(4)))<>10 and len(trim(dspkey(4)))<>8 ) ) AND DSPKEY(38) <> "Y" AND (dspkey(65)="01" or dspkey(65)="02") then
       formValid=False
       message="用戶身分證(統編)不可空白或長度不對"    
  elseif len(trim(dspkey(11)))=0 then
       formValid=False
       message="戶籍地址(縣市)不可空白"   
  elseif len(trim(dspkey(12)))=0 and dspkey(11)<>"06" and dspkey(5)<>"11" then
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
  elseif (len(trim(dspkey(7)))=0 or Not Isdate(dspkey(7))) AND COMPANY="N" then
       formValid=False
       message="用戶為個人時，出生日期不可空白或格式錯誤"   
  elseif len(trim(dspkey(7)))<>0  AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，出生日期必須空白"          
  elseif len(trim(dspkey(9)))=0 and len(trim(dspkey(10)))=0 then
       formValid=False
       message="用戶連絡電話及行動電話至少須輸入一項"   
  elseif instr(1,dspkey(10),"-",1) > 0 then
       formValid=False
       message="行動電話不可包含'-'符號"          
  elseif instr(1,dspkey(9),"-",1) > 0 then
       formValid=False
       message="連絡電話不可包含'-'符號"         
  elseif len(trim(dspkey(23)))=0  AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，企業連絡人不可空白"         
  elseif len(trim(dspkey(24)))=0  AND len(trim(dspkey(26)))=0 AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，企業連絡人連絡電話及行動電話至少需輸入一項"    
  elseif len(trim(dspkey(27)))=0  AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，企業負責人不可空白"
  elseif len(trim(dspkey(28)))=0  AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，企業負責人身份證字號不可空白"                     
  elseif len(trim(dspkey(37)))= 0 then
       formValid=False
       message="方案種類不可空白"   
  elseif len(trim(dspkey(40)))= 0 AND DSPKEY(38) <> "Y" then
       formValid=False
       message="AVS繳款方式不可空白"      
  elseif len(trim(dspkey(40)))> 0 AND DSPKEY(38) = "Y" then
       formValid=False
       message="公關機時，AVS繳款方式必須空白"           
  elseif len(trim(dspkey(47)))<> 0 AND len(trim(dspkey(46)))= 0 then
       formValid=False
       message="完工日期為空白時不可輸入報竣日"       
  elseif len(trim(dspkey(46)))<> 0 AND ( len(trim(dspkey(61)))= 0 or dspkey(61)="0" or len(trim(dspkey(64)))= 0 or dspkey(64)="0" )then
       formValid=False
       message="輸入完工日期時，用戶IP不可空白"              
  end if
  IF formValid=TRUE THEN
    IF dspkey(4) <> "" and (dspkey(65)="01" or dspkey(65)="02") then
       idno=dspkey(4)
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
   if len(trim(dspkey(28)))<> 0 then
      idno=dspkey(28)
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
  IF formValid=TRUE THEN
   Set connxx=Server.CreateObject("ADODB.Connection")
   Set rsxx=Server.CreateObject("ADODB.Recordset")
   connxx.open DSN
   sqlxx="select * from RTSparq499Cmtyline where comq1=" & aryparmkey(0) & " AND LINEQ1=" & ARYPARMKEY(1)
   rsxx.Open sqlxx,connxx
   if not rsxx.eof then
      if len(trim(rsxx("consignee"))) <> 0 then
         if len(trim(dspkey(34))) <> 0 or len(trim(dspkey(35))) <> 0 or len(trim(dspkey(36))) <> 0then
            formValid=False
            message="主線開發為經銷商,績效歸屬必須空白" 
         end if
      else
         if len(trim(dspkey(34))) = 0 or len(trim(dspkey(35))) = 0 or len(trim(dspkey(36))) = 0 then
            formValid=False
            message="主線開發為直銷,績效歸屬不可空白" 
         end if
      end if
      '主線未測通者，不可輸入avs申請日
      if isnull(rsxx("ADSLOPENDAT")) and len(trim(dspkey(46))) <> 0 then
            formValid=False
            message="主線未測通，不可輸入用戶完工日" 
      ELSEif isnull(rsxx("ADSLOPENDAT")) and len(trim(dspkey(47))) <> 0 then
            formValid=False
            message="主線未測通，不可輸入用戶報竣日" 
      end if

     IF NOT ISNULL(RSXX("DROPDAT")) OR NOT ISNULL(RSXX("CANCELDAT")) THEN
        formValid=False
        message="主線已作廢或撤銷，不可新增及異動用戶資料" 
     END IF
   end if
   rsxx.close
   connxx.Close   
   set rsxx=Nothing   
   set connxx=Nothing 
 END IF
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
       prog=prog & "?KEY=" & document.all("KEY34").VALUE 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key35").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub        
   Sub Srsalesonclick()
       prog="RTGetsalesD.asp"
       prog=prog & "?KEY=" & document.all("KEY34").VALUE & ";" & document.all("KEY35").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key36").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub
   Sub SrDeveloperonclick()
       prog="RTGetDeveloperD.asp"
       prog=prog & "?KEY=" & document.all("KEY73").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key73").value =  trim(Fusrid(0))
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
   Sub SrTAG8()
      ' msgbox window.SRTAB1.style.display
       if window.SRTAB8.style.display="" then
          window.SRTAB8.style.display="none"
       elseif window.SRTAB8.style.display="none" then
          window.SRTAB8.style.display=""
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
       <tr><td width="15%" class=dataListHead>社區序號</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key0"
                  readonly size="10" value="<%=dspKey(0)%>" maxlength="8" class=dataListdata></td>
           <td width="15%" class=dataListHead>主線序號</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key1"
                  readonly size="10" value="<%=dspKey(1)%>" maxlength="8" class=dataListdata></td>                 
           <td width="25%" class=dataListHead>用戶序號</td>
           <td width="25%"  bgcolor="silver">
           <input type="text" name="key2"
                  readonly size="15" value="<%=dspKey(2)%>" maxlength="15" class=dataListdata></td>
  </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(30))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(30)=V(0)
        End if  
       dspkey(31)=datevalue(now())
    else
        if len(trim(dspkey(32))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(32)=V(0)
        End if         
        dspkey(33)=datevalue(now())
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
  <span id="tags1" class="dataListTagsOn">速博499用戶資訊</span>
                                                            
  <div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td>
<td>     
    <DIV ID="SRTAG0" onclick="srtag0" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="LEFT">基本資料</td></tr></table></div>
 <DIV ID=SRTAB0 >   
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
<TR><td  class="dataListSEARCH" height="23">用戶IP</td>                                 
        <td  height="23" bgcolor="silver"  >
        <%
        '讀取主線IP網段...
     '   SQL="SELECT LINEIPSTR1,  LINEIPSTR2, LINEIPSTR3, LINEIPSTR4,LINEIPEND FROM RTSparq499CmtyLINE WHERE COMQ1=" & DSPKEY(0) & " AND LINEQ1=" & DSPKEY(1)
     '   RS.OPEN SQL,CONN
     '   IF NOT RS.EOF THEN
     '      IP1=RS("LINEIPSTR1")
     '      IP2=RS("LINEIPSTR2")
     '      IP3=RS("LINEIPSTR3")
     '      IP4=RS("LINEIPSTR4")
     '      IPe=RS("LINEIPEND")
     '   ELSE
     '      ip1=""
     '      ip2=""
     '      ip3=""
     '      ip4=""
     '      ipE=""
     '   END IF
     '   RS.CLOSE
     '   IF LEN(TRIM(DSPKEY(61)))=0 THEN DSPKEY(61)=IP1
     '   IF LEN(TRIM(DSPKEY(62)))=0 THEN DSPKEY(62)=IP2
     '   IF LEN(TRIM(DSPKEY(63)))=0 THEN DSPKEY(63)=IP3
        %>
        <font color=red><input type="text" name="key61" size="3" maxlength="3" value="<%=dspKey(61)%>"  readonly class="dataListdata" ID="Text3">
        <font size=2>.</font>
        <input type="text" name="key62" size="3" maxlength="3" value="<%=dspKey(62)%>"  readonly class="dataListdata" ID="Text6">
        <font size=2>.</font>
        <input type="text" name="key63" size="3" maxlength="3" value="<%=dspKey(63)%>"  readonly class="dataListdata" ID="Text7">
        <font size=2>.</font>
        <input type="text" name="key64" size="3" maxlength="3" value="<%=dspKey(64)%>"  readonly class="dataListdata" ID="Text10"></font></td>     
<td  class="dataListSEARCH" height="23">對帳序號</td>                                 
        <td  height="23" bgcolor="silver"  >
        <input type="text" name="key60" size="15" maxlength="15" <%=fieldpa%> value="<%=dspKey(60)%>"  class="dataListDATA" READONLY ID="Text1">
        <FONT SIZE=2>-</FONT>
        <input type="text" name="key66" size="3" maxlength="3" <%=fieldpa%> value="<%=dspKey(66)%>"  class="dataListDATA" READONLY ID="Text1">
</TD>
</TR>        
<tr><td width="15%" class=dataListHEAD>收件日</td>
    <td width="35%" bgcolor="silver" >
        <input type="text" name="key44" <%=fieldpa%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(44)%>"  READONLY size="10" class=dataListEntry>
       <input  type="button" id="B44"  <%=fieldpb%> name="B44" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG  SRC="/consignee/images/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C44"  name="C44"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
      </td>
<td width="15%" class=dataListHEAD>用戶申請日</td>
    <td width="35%" bgcolor="silver" >
        <input type="text" name="key45" <%=fieldpa%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(45)%>"  READONLY size="10" class=dataListEntry>
       <input  type="button" id="B45"  <%=fieldpb%> name="B45" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG  SRC="/consignee/images/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C45"  name="C45"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
      </td>      
</tr>
<tr><td width="15%" class=dataListHEAD>用戶(公司)名稱</td>
    <td  width="35%"  bgcolor="silver" >
        <input type="text" name="key3" <%=fieldpa%><%=dataProtect%>
               style="text-align:left;" maxlength="30"
               value="<%=dspKey(3)%>"  size="30" class=dataListENTRY ID="Text22"></td>
<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='J5' " 
       If len(trim(dspkey(65))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='J5' AND CODE='" & dspkey(65) &"' " 
       'SXX60=""
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(第一證照別)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(65) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>        
              
<td width="15%" class=dataListHEAD>身分證(統編)</td>
    <td width="35%" bgcolor="silver" >
	<select size="1" name="key65"<%=fieldpg%><%=fieldpa%><%=dataProtect%> size="1" class="dataListEntry"><%=s%></select>    
        <input type="text" name="key4" <%=fieldpa%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(4)%>"   size="12" class=dataListENTRY ID="Text23"></td>               
</tr>
<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='L3' " 
       If len(trim(dspkey(5))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='L3' AND CODE='" & dspkey(5) &"' " 
       'SXX60=""
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(第二證照別)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(5) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>        
    <tr>
        <td width="10%" class="dataListHead" height="25">第二證照別及號碼</td>
        <td width="18%" height="25" bgcolor="silver" colspan=5> 
		<select size="1" name="key5"<%=fieldpg%><%=fieldpa%><%=dataProtect%> size="1" class="dataListEntry" ID="Select16"><%=s%></select>	
        <input type="text" name="key6" size="15" maxlength="12" value="<%=dspkey(6)%>" <%=fieldpg%><%=fieldpa%><%=dataProtect%> class="dataListEntry" ID="Text49"></td> 
     </tr>    
<tr>                                 
        <td  class="dataListHEAD" height="23">出生日期</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key7" size="10"  value="<%=dspKey(7)%>"  <%=fieldpa%> class="dataListentry" ID="Text8">  
        <input type="button" id="B7"  <%=fieldpb%>  name="B7" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/consignee/images/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C7"  name="C7"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"> </td>           
        <td  class="dataListHEAD" height="23">連絡EMAIL</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" name="key8" size="30" maxlength="30" <%=fieldpa%> value="<%=dspKey(8)%>"   class="dataListEntry" ID="Text53"></td>                                 
 
 </tr>        
<TR>        
                                    
        <td  class="dataListHEAD" height="23">連絡電話</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" name="key9" size="15" maxlength="15" <%=fieldpa%> value="<%=dspKey(9)%>"  class="dataListEntry" ID="Text50"></td>     
        <td  class="dataListHEAD" height="23">行動電話</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key10" size="15" maxlength="15" <%=fieldpa%> value="<%=dspKey(10)%>"  class="dataListEntry" ID="Text11"></td>                                 
 </tr>
<tr><td class=dataListHEAD>戶籍/公司地址</td>
    <td bgcolor="silver" COLSPAN=3>
  <%s=""
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
         <select size="1" name="key11" <%=fieldpa%><%=dataProtect%> size="1" class="dataListEntry" ID="Select2" ><%=s%></select>
        <input type="text" name="key12" readonly  size="8" value="<%=dspkey(12)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=dataProtect%> class="dataListDATA" ID="Text4"><font SIZE=2>(鄉鎮)                 
         <input type="button" id="B12" <%=fieldpb%> name="B12"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX12%>  >        
          <IMG SRC="/consignee/images/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C12"  name="C12"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
        
        <input type="text" name="key13" <%=fieldpa%> size="40" value="<%=dspkey(13)%>" maxlength="60" <%=dataProtect%> class="dataListEntry" ID="Text5"><font size=2>
       <input type="text" name="key14"  readonly size="5" value="<%=dspkey(14)%>" maxlength="5" <%=fieldpa%><%=dataProtect%> class="dataListDATA" ID="Text12">
        </td>                                 
</tr>  
<tr><td class=dataListHEAD>裝機地址
    <br><input type="radio" name="rd1"  <%=fieldpb%> onClick="SrAddrEqual1()">同戶籍</td>
    <td bgcolor="silver" COLSPAN=3>
  <%s=""
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
         <select size="1" name="key15" <%=fieldpa%><%=dataProtect%> size="1" class="dataListEntry" ID="Select2"><%=s%></select>
        <input type="text" name="key16" readonly  size="8" value="<%=dspkey(16)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=dataProtect%> class="dataListDATA" ID="Text4"><font SIZE=2>(鄉鎮)                 
         <input type="button" id="B16"  <%=fieldpb%>  name="B16"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX16%>  >        
          <IMG SRC="/consignee/images/IMGDELETE.GIF" <%=fieldpb%>  alt="清除" id="C16"  name="C16"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
        
        <input type="text" name="key17" <%=fieldpa%> size="40" value="<%=dspkey(17)%>" maxlength="60" <%=dataProtect%> class="dataListEntry" ID="Text5"><font size=2>
<input type="text" name="key18"  readonly size="5" value="<%=dspkey(18)%>" maxlength="5" <%=fieldpa%><%=dataProtect%> class="dataListDATA" ID="Text13">
</td>                                 
</tr>  
<tr><td class=dataListHEAD>帳單地址
    <br><input type="radio" name="rd2" <%=fieldpb%>  onClick="SrAddrEqual2()"><font SIZE=2>同戶籍</font><input type="radio"  <%=fieldpb%> name="rd2" onClick="SrAddrEqual3()"><font SIZE=2>同裝機</font>
    </td>
    <td bgcolor="silver" COLSPAN=3>
  <%s=""
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
         <select size="1" name="key19" <%=fieldpa%><%=dataProtect%> size="1" class="dataListEntry" ID="Select2"><%=s%></select>
        <input type="text" name="key20" readonly  size="8" value="<%=dspkey(20)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=dataProtect%> class="dataListDATA" ID="Text4"><font SIZE=2>(鄉鎮)                 
         <input type="button" id="B20"   <%=fieldpb%> name="B20"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX20%>  >        
          <IMG SRC="/consignee/images/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C20"  name="C20"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
        
        <input type="text" name="key21" <%=fieldpa%> size="40" value="<%=dspkey(21)%>" maxlength="60" <%=dataProtect%> class="dataListEntry" ID="Text5"><font size=2>
<input type="text" name="key22"  readonly size="5" value="<%=dspkey(22)%>" maxlength="5" <%=fieldpa%><%=dataProtect%> class="dataListDATA" ID="Text14">
</td>                                 
</tr>
<TR>        
                                    
        <td  class="dataListHEAD" height="23">企業連絡人</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" name="key23" size="15" maxlength="15" <%=fieldpa%> value="<%=dspKey(23)%>"  class="dataListEntry" ID="Text54"></td>     
        <td  class="dataListHEAD" height="23">連絡電話</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key24" size="15" maxlength="15" <%=fieldpa%> value="<%=dspKey(24)%>"  class="dataListEntry" ID="Text58">
        <font size=2>分機︰</font>
        <input type="text" name="key25" size="5" maxlength="5" <%=fieldpa%> value="<%=dspKey(25)%>"  class="dataListEntry" ID="Text59"></td>                                 
 </tr>
 <TR>        
                                    
        <td  class="dataListHEAD" height="23">行動電話</td>                                 
        <td  height="23" bgcolor="silver" COLSPAN=3>
        <input type="text" name="key26" size="10" maxlength="10" <%=fieldpa%> value="<%=dspKey(26)%>"  class="dataListEntry" ID="Text61"></td>     
 </tr>
 <TR>        
                                    
        <td  class="dataListHEAD" height="23">企業負責人</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" name="key27" size="10" maxlength="10" <%=fieldpa%> value="<%=dspKey(27)%>"  class="dataListEntry" ID="Text62"></td>     
        <td  class="dataListHEAD" height="23">身份證字號</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key28" size="10" maxlength="10" <%=fieldpa%> value="<%=dspKey(28)%>"  class="dataListEntry" ID="Text65"></td>                                 
 </tr>
 <TR>        
                                    
        <td  class="dataListHEAD" height="23">行業別</td>                                 
        <td  height="23" bgcolor="silver" COLSPAN=3>
        <input type="text" name="key29" size="20" maxlength="20" <%=fieldpa%> value="<%=dspKey(29)%>" class="dataListEntry" ID="Text66"></td>     
 </tr>
<tr>
        <td  class="dataListHEAD" height="23">建檔人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(30) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(30) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>    <input type="text" name="key30" size="6" READONLY value="<%=dspKey(30)%>"  <%=fieldpa%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">建檔日期</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key31" size="10" READONLY value="<%=dspKey(31)%>"  <%=fieldpa%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>  
<tr>
        <td  class="dataListHEAD" height="23">修改人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
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
  %>    <input type="text" name="key32" size="6" READONLY value="<%=dspKey(32)%>"  <%=fieldpa%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">修改日期</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key33" size="10" READONLY value="<%=dspKey(33)%>"  <%=fieldpa%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>         
</table> </div>
<!--
    <table border="1" width="100%" cellpadding="0" cellspacing="0" id="tag2" style="display: none"> 
    -->
    <DIV ID="SRTAG1" onclick="srtag1" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="LEFT">績效歸屬</td></tr></table></div>
    
     <DIV ID=SRTAB1>
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table7">
	<tr><td id="tagT1" WIDTH="15%" class="dataListHEAD" height="23">業務轄區</td>
        <td WIDTH="85%" height="23" colspan=5 bgcolor="silver">
<%  If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(DSPKEY(46))) = 0  Then 
       sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '1') "
       s="<option value="""" >(業務轄區)</option>"
    Else
       sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '1') AND AREAID='" & DSPKEY(34) & "' "
       s="<option value="""" >(業務轄區)</option>"
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(業務轄區)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("areaid")=dspkey(34) Then sx=" selected "
       s=s &"<option value=""" &rs("areaid") &"""" &sx &">" &rs("areanc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>    
           <select size="1" name="key34" <%=fieldpa%><%=dataProtect%>  class="dataListEntry" ID="Select1">                                            
              <%=s%>
           </select>
    <input type="text" name="key35" <%=fieldpa%> <%=dataProtect%> 
               style="text-align:left;" size="15" maxlength="10" 
               value="<%=dspKey(35)%>"   readonly class="dataListEntry" ID="Text64">
         <input type="button" id="B35"  <%=fieldpb%>  name="B35"   width="100%" style="Z-INDEX: 1"  value="...." readonly onclick="SrsalesGrouponclick()"  >  
          <IMG SRC="/consignee/images/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C35"  name="C35"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">                               
    <input type="TEXT" name="key36" <%=dataProtect%> 
               style="text-align:left;" size="15" maxlength="10" 
               value="<%=dspKey(36)%>"  readonly class="dataListDATA" ID="Hidden1">
           <input type="BUTTON" id="B36"  <%=fieldpb%>  name="B36"  width="100%" style="Z-INDEX: 1"  value="...." onclick="Srsalesonclick()"  >   
           <IMG SRC="/consignee/images/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C36"  name="C36"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">                               
        </td>
 </tr>
 
 <%
	name=""
	if dspkey(73) <> "" then
		sqlxx=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			 &"where rtemployee.emply='" & dspkey(73) & "' "
		rs.Open sqlxx,conn
		if rs.eof then
			name="(對象檔找不該員工)"
		else
			name=rs("cusnc")
		end if
		rs.close
	end if
%>
	<tr><td WIDTH="15%" class="dataListHEAD" height="23">二線開發人員</td>
		<td width="35%"><input type="text" name="key73"value="<%=dspKey(73)%>" <%=dataProtect%> style="text-align:left;" size="8" maxlength="6" readonly class="dataListDATA" ID="Text21">
			<input type="BUTTON" id="B73" name="B73" <%=fieldpb%> style="Z-INDEX: 1"  value="...." onclick="Srdeveloperonclick()">
			<IMG SRC="/consignee/images/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C73" name="C73" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">
			<font size=2><%=name%></font></td></tr>
  </table>     
  </DIV> 
  </DIV>   
      <DIV ID="SRTAG2" onclick="srtag2" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
    <tr><td bgcolor="BDB76B" align="LEFT">寬頻服務</td></tr></table></DIV>
    <DIV ID=SRTAB2 >
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3">
     <tr>   <td  WIDTH="15%"  class="dataListHEAD" height="23">方案類別</td>               
        <td  WIDTH="35%" height="23" bgcolor="silver" >
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(DSPKEY(47))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='L9' " 
       If len(trim(dspkey(37))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='L9' AND CODE='" & dspkey(37) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(37) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key37" <%=fieldpC%><%=dataProtect%> class="dataListEntry" ID="Select8">                                                                  
        <%=s%>
   </select>
        </td>
        <td  WIDTH="15%" class="dataLISTSEARCH" height="23">公關機</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver" >
<%  dim FREECODE1,FREECODE2
    If Len(Trim(dataProtect)) < 1 and flg = "Y" Then
       FREECODE1=""
       FREECODE2=""
    Else
      ' sexd1=" disabled "
      ' sexd2=" disabled "
    End If
    If dspKey(38)="Y" Then FREECODE1=" checked "    
    If dspKey(38)="N" Then FREECODE2=" checked " %>                          
        <input type="radio" value="Y" <%=FREECODE1%> name="key38" <%=dataProtect%> ID="Radio1">是
        <input type="radio" name="key38" value="N" <%=FREECODE2%>  <%=dataProtect%> ID="Radio2">否</td>
        </tr>
<tr>   <td  WIDTH="15%" class="dataListHEAD" height="23">PROMOTION CODE</td>               
        <td  WIDTH="35%" height="23" bgcolor="silver" >
        <input type="text" name="key39" size="15" maxlength="10" value="<%=dspKey(39)%>"  <%=fieldpa%> class="dataListdata" ID="Text15"></td>                                 
        </td>
    <td  WIDTH="15%"  class="dataListHEAD" height="23">繳款方式</td>               
        <td  WIDTH="35%" height="23" bgcolor="silver" >
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(DSPKEY(47))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M1' " 
       If len(trim(dspkey(40))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M1' AND CODE='" & dspkey(40) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(40) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key40" <%=fieldpC%><%=dataProtect%> class="dataListEntry" ID="Select35">                                                                  
        <%=s%>
   </select>
        </td></tr>
           
  </table>     
  </DIV>
      <DIV ID="SRTAG5" onclick="srtag5" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
    <tr><td bgcolor="BDB76B" align="LEFT">代理人資訊</td></tr></table></DIV>
    <DIV ID=SRTAB5 >
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3"> 
<TR>        
        <td   width=15% class="dataListHEAD" height="23">代理人姓名</td>                                 
        <td   width=35% height="23" bgcolor="silver">
        <input type="text" name="key41" size="10" maxlength="15" value="<%=dspKey(41)%>"  <%=fieldpc%> class="dataListEntry" ID="Text9"></td>                                 
        <td   width=15% class="dataListHEAD" height="23">代理人身份證號</td>                                 
        <td  width=35% height="23" bgcolor="silver" >
        <input type="text" name="key42" size="10" maxlength="10" value="<%=dspKey(42)%>"  <%=fieldpc%> class="dataListEntry" ID="Text16"></td>                                 
 </tr>
<TR>        
        <td  class="dataListHEAD" height="23">代理人電話</td>                                 
        <td  height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key43" size="15" maxlength="15" value="<%=dspKey(43)%>"  <%=fieldpc%> class="dataListEntry" ID="Text9"></td>                                 
 </tr>     
</TABLE>    
    </div> 
   <DIV ID="SRTAG4" onclick="srtag4" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr><td bgcolor="BDB76B" align="LEFT">用戶申請及施工進度狀態</td></tr></table></DIV>
    <DIV ID=SRTAB4 >  
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
        <tr>
        <td   class="dataListHEAD" height="23">完工日期</td>                                 
        <td   height="23" bgcolor="silver" >
        <input type="text" name="key46" size="10" READONLY value="<%=dspKey(46)%>" <%=fieldpC%>   class="dataListentry" ID="Text57">
                <input type="button" id="B46"  name="B46" height="100%" width="100%"  <%=fieldpD%> style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
        <IMG SRC="/consignee/images/IMGDELETE.GIF" alt="清除" id="C46"  name="C46"    <%=fieldpD%> style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">     
        </td>
        <td   class="dataListHEAD" height="23">報竣日期</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key47" size="10" READONLY value="<%=dspKey(47)%>" <%=fieldpe%>  class="dataListDATA" ID="Text56">
           <!--     <input type="button" id="B47"  name="B47" height="100%" width="100%" <%=fieldpf%>style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
        <IMG SRC="/consignee/images/IMGDELETE.GIF" alt="清除" id="C47"  name="C47"   <%=fieldpf%>style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"> -->    
        </TD>
</tr> 
        <tr>
        <td   class="dataListHEAD" height="23">報竣轉檔日</td>                                 
        <td   height="23" bgcolor="silver" >
        <input type="text" name="key48" size="10" value="<%=dspKey(48)%>"   class="dataListDATA" ID="Text57">     
        </td>
                <td   class="dataListHEAD" height="23">退租日</td>                                 
        <td   height="23" bgcolor="silver" >
        <input type="text" name="key49" size="10" value="<%=dspKey(49)%>"   class="dataListDATA" ID="Text24">
        <font size=2>欠費︰</font> 
        <input type="text" name="key52" size="2" READONLY value="<%=dspKey(52)%>"   class="dataListDATA" ID="Text48">  
        </td>
    
</tr> 
       <tr>
        <td  width=15% class="dataListHEAD" height="23">作廢日期</td>                                 
        <td  width=35% height="23" bgcolor="silver">
        <input type="text" name="key50" size="10" value="<%=dspKey(50)%>"  <%=fieldpa%> readonly class="dataListdata" ID="Text41">
         </td>
        <%  name="" 
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
        <td   width=15% class="dataListHEAD" height="23">作廢人員</td>                                 
        <td   width=35% height="23" bgcolor="silver">
        <input type="text" name="key51" size="10" value="<%=dspKey(51)%>"  readonly class="dataListDATA" ID="Text43"><font size=2><%=name%></font>
        </td></tr>           
  </table></DIV>

<DIV ID="SRTAG8" onclick="SRTAG8" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table10">
    <tr><td bgcolor="BDB76B" align="LEFT">信用卡資料</td></tr></table></DIV>

<DIV ID="SRTAB8" >  
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table11">
<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='M6' " 
       If len(trim(dspkey(67))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='M6' AND CODE='" & dspkey(67) &"' " 
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(信用卡類型)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(67) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<tr><td width="15%" class="dataListHEAD" height="23">信用卡</td>
		<td width="35%" bgcolor="silver">
		<select size="1" name="key67"<%=fieldpg%><%=fieldpa%><%=dataProtect%> size="1" class="dataListEntry"><%=s%></select>

		<td width="15%" class="dataListHEAD" height="23">發卡銀行</td>
        <td width="35%" height="23" bgcolor="silver">
		<input type="text" name="key68" value="<%=dspKey(68)%>" <%=fieldpa%><%=dataProtect%>
			style="text-align:left;" maxlength="20" size="25" class=dataListENTRY></td></tr>

	<tr><td class="dataListHEAD" height="23">信用卡卡號</td>
        <td height="23" bgcolor="silver">
		<input type="text" name="key69" value="<%=dspKey(69)%>" <%=fieldpa%><%=dataProtect%>
			style="text-align:left;" maxlength="16" size="20" class=dataListENTRY></td>

		<td class="dataListHEAD" height="23">持卡人姓名</td>
        <td height="23" bgcolor="silver">
		<input type="text" name="key70" value="<%=dspKey(70)%>" <%=fieldpa%><%=dataProtect%>
			style="text-align:left;" maxlength="20" size="20" class=dataListENTRY></td></tr>

	<tr><td class="dataListHEAD" height="23">信用卡有效期限</td>
        <td height="23" bgcolor="silver" colspan=3>
		<input type="text" name="key71" value="<%=dspKey(71)%>" <%=fieldpa%><%=dataProtect%>
			style="text-align:left;" maxlength="2" size="5" class=dataListENTRY>月/
		<input type="text" name="key72" value="<%=dspKey(72)%>" <%=fieldpa%><%=dataProtect%>
			style="text-align:left;" maxlength="2" size="5" class=dataListENTRY>年</td></tr>
    </table></div>


   <DIV ID="SRTAG7" onclick="SRTAG7" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table4">
    <tr><td bgcolor="BDB76B" align="LEFT">異動狀態</td></tr></table></DIV>
    <DIV ID="SRTAB7" >  
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table5">
       <tr>
        <td  width=15% class="dataListHEAD" height="23">移出社區主線序號</td>                                 
        <td  width=35% height="23" bgcolor="silver">
        <% comn=""
           if dspkey(54) <> 0 then
              sqlxx=" select * from RTSparq499Cmtyh where comq1=" & dspkey(54) 
              rs.Open sqlxx,conn
              if rs.eof then
                 comn=""
              else
                 comn=rs("comn")
              end if
              rs.close
           end if
        %>                   
        <input type="text" name="key54" size="5" value="<%=dspKey(54)%>"  <%=fieldpa%> readonly class="dataListDATA" ID="Text36">
        <input type="text" name="key55" size="5" value="<%=dspKey(55)%>"  <%=fieldpa%> readonly class="dataListDATA" ID="Text38">
        <font size=2><%=comn%></font></td>
        <td   width=15% class="dataListHEAD" height="23">移入社區主線序號</td>                                 
        <td   width=35% height="23" bgcolor="silver">
        <% comn=""
           if dspkey(56) <> 0 then
              sqlxx=" select * from RTSparq499Cmtyh where comq1=" & dspkey(56) 
              rs.Open sqlxx,conn
              if rs.eof then
                 comn=""
              else
                 comn=rs("comn")
              end if
              rs.close
           end if
        %>                   
        <input type="text" name="key56" size="5" value="<%=dspKey(56)%>"  readonly class="dataListDATA" ID="Text37">
        <input type="text" name="key57" size="5" value="<%=dspKey(57)%>"  readonly class="dataListDATA" ID="Text39">
        <font size=2><%=comn%></font></td>
     </tr>      
       <tr>
        <td  width=15% class="dataListHEAD" height="23">移出異動結案日</td>                                 
        <td  width=35% height="23" bgcolor="silver">
        <input type="text" name="key58" size="10" value="<%=dspKey(58)%>"  <%=fieldpa%> readonly class="dataListDATA" ID="Text40">
         </td>
        <td   width=15% class="dataListHEAD" height="23">移入異動結案日</td>                                 
        <td   width=35% height="23" bgcolor="silver">
        <input type="text" name="key59" size="10" value="<%=dspKey(59)%>"  readonly class="dataListDATA" ID="Text42">
        </td></tr>           
    </table> 
</div>
    <DIV ID="SRTAG6" onclick="SRTAG6" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="LEFT">備註說明</td></tr></table></DIV>
   <DIV ID="SRTAB6" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
    <TR><TD align="CENTER">
     <TEXTAREA  cols="100%"  name="key53" rows=8  MAXLENGTH=500  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(53)%>" ID="Textarea1"><%=dspkey(53)%></TEXTAREA>
   </td></tr>
 </table> 
  </div> 
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
<!-- #include virtual="/Consignee/checkid.inc" -->
<!-- #include virtual="/Consignee/companyid.inc" -->
<!-- #include virtual="/Consignee/employeeref.inc" -->