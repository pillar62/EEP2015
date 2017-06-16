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
                   case ucase("/webap/rtap/base/rtlessorcmty/RTLessorCmtyLined.asp")
                     '  response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       if i <> 1 then rs.Fields(i).Value=dspKey(i)    
                       if i=1 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         rsc.open "select max(lineq1) AS lineq1 from RTLessorCmtyLine where comq1=" & dspkey(0) ,conn
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
                 case ucase("/webap/rtap/base/rtlessorcmty/RTLessorCmtyLined.asp")
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
       if ucase(runpgm)=ucase("/webap/rtap/base/rtlessorcmty/RTLessorCmtyLined.asp") then
          rs.open "select max(lineq1) AS lineq1 from RTLessorCmtyLine where comq1=" & dspkey(0) ,conn
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
  title="ET-City主線資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT COMQ1,LINEQ1,LINEGROUP,CONSIGNEE,AREAID,GROUPID,SALESID,DEVELOPERID,LINEIP,GATEWAY,SUBNET," _
             &"DNSIP, PPPOEACCOUNT,PPPOEPASSWORD, LINETEL, LINERATE, LINEISP, LINEIPTYPE, IPCNT,LINEDUEDAT, CUTID," _
             &"TOWNSHIP, VILLAGE, COD1, NEIGHBOR, COD2, STREET,COD3, SEC, COD4, LANE, " _
             &"COD5, TOWN, COD6, ALLEYWAY, COD7, NUM, COD8,FLOOR, COD9, ROOM," _
             &"COD10, ADDROTHER, RZONE, RCVDAT,INSPECTDAT,AGREE,UNAGREEREASON,HINETNOTIFYDAT, HARDWAREDAT,ADSLAPPLYDAT," _
             &"EUSR, EDAT, UUSR, UDAT, CANCELDAT, CANCELUSR,DROPDAT, SUPPLYRANGE, LOANNAME, LOANSOCIAL," _
             &"MEMO,APPLYDAT,APPLYNAME,APPLYSOCIAL,APPLYCONTACTTEL,APPLYMOBILE,LOANCONTACTTEL,LOANMOBILE,CONTAPPLYDAT,DROPKIND " _
             &"FROM RTLessorCmtyLine WHERE COMQ1=0 "
  sqlList="SELECT COMQ1,LINEQ1,LINEGROUP,CONSIGNEE,AREAID,GROUPID,SALESID,DEVELOPERID,LINEIP,GATEWAY,SUBNET," _
             &"DNSIP, PPPOEACCOUNT,PPPOEPASSWORD, LINETEL, LINERATE, LINEISP, LINEIPTYPE, IPCNT,LINEDUEDAT, CUTID," _
             &"TOWNSHIP, VILLAGE, COD1, NEIGHBOR, COD2, STREET,COD3, SEC, COD4, LANE, " _
             &"COD5, TOWN, COD6, ALLEYWAY, COD7, NUM, COD8,FLOOR, COD9, ROOM," _
             &"COD10, ADDROTHER, RZONE, RCVDAT,INSPECTDAT,AGREE,UNAGREEREASON,HINETNOTIFYDAT, HARDWAREDAT,ADSLAPPLYDAT," _
             &"EUSR, EDAT, UUSR, UDAT, CANCELDAT, CANCELUSR,DROPDAT, SUPPLYRANGE, LOANNAME, LOANSOCIAL," _
             &"MEMO,APPLYDAT,APPLYNAME,APPLYSOCIAL,APPLYCONTACTTEL,APPLYMOBILE,LOANCONTACTTEL,LOANMOBILE,CONTAPPLYDAT,DROPKIND  " _
         &"FROM RTLessorCmtyLine WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=1
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    
    if len(trim(DSPKEY(1))) = 0 THEN DSPKEY(1)=0
    if len(trim(DSPKEY(2))) = 0 THEN DSPKEY(2)="" 
    if len(trim(DSPKEY(70))) = 0 THEN DSPKEY(70)="" 
    if dspkey(46) <> "Y" and dspkey(46) <>"N" then dspkey(46)=""         
    If len(trim(dspKey(0))) <= 0 Then
       dspkey(0)=0
    END IF       
    If len(trim(dspkey(44)))=0 or Not Isdate(dspkey(44)) then
 '      formValid=False
 '      message="收件日不可空白或格式錯誤"    
 '   elseif len(trim(dspkey(6)))=0 AND len(trim(dspkey(3)))=0 AND len(trim(dspkey(7)))=0 THEN
 '      formValid=False
 '      message="開發業務員或經銷商不可同時空白"
 '   elseif len(trim(dspkey(20)))=0 then
 '      formValid=False
 '      message="裝機地址(縣市)不可空白"   
 '   elseif len(trim(dspkey(21)))=0 and dspkey(21)<>"06" and dspkey(21)<>"15" then
 '      formValid=False
 '      message="裝機地址(鄉鎮)不可空白"    
 '   elseif len(trim(dspkey(26)))=0 then
 '      formValid=False
 '      message="裝機地址(路/街)不可空白"          
 '   elseif len(trim(dspkey(28))) > 0 AND DSPKEY(28) <="一" AND DSPKEY(28) >= "九" then
 '      formValid=False
 '      message="裝機地址(段)必須為中文數字(一~九)"                 
 '   elseif len(trim(dspkey(36)))=0 then
 '      formValid=False
 '      message="裝機地址(號)不可空白"           
  '  elseif len(trim(dspkey(58)))=0 then
  '     formValid=False
  '     message="主線可供裝範圍不可空白"       
  '  elseif len(trim(dspkey(15)))=0 then
  '     formValid=False
  '     message="主線速率不可空白"    
  '  elseif (len(trim(dspkey(59))) <> 0 and len(trim(dspkey(60))) = 0 ) or   (len(trim(dspkey(59))) = 0 and len(trim(dspkey(60))) <> 0 ) then
  '     formValid=False
  '     message="藉名裝機時，用戶名稱及身份證號必須同時存在"    
  '  elseif len(trim(DSPKEY(60)))  <> 0 AND  len(trim(DSPKEY(60)))  <> 10 AND  len(trim(DSPKEY(60)))  <> 8 THEN
  '     formValid=False
  '     message="身份證(統編)字號長度不足(必須為10碼或8碼)"                             
  '  elseif DSPKEY(46)<> "" AND LEN(TRIM(DSPKEY(45)))=0 THEN
  '     formValid=False
  '     message="請輸入勘查日期"                                                                                                                                                            
  '  elseif DSPKEY(46)="N" AND LEN(TRIM(DSPKEY(47)))=0 THEN
  '     formValid=False
  '     message="勘察為不可建置時必須輸入原因"  
  '  elseif len(trim(DSPKEY(62))) > 0 and dspkey(46)<>"Y" THEN
  '     formValid=False
  '     message="主線申請必須為勘查為[可建置]狀態"    
  '  elseif len(trim(DSPKEY(62))) > 0 and len(trim(dspkey(16)))=0 THEN
  '     formValid=False
  '     message="主線線路申請時，線路ISP不可空白"                   
  '  elseif len(trim(DSPKEY(62))) > 0 and len(trim(dspkey(17)))=0 THEN
  '     formValid=False
  '     message="主線線路申請時，線路IP種類不可空白"     
  '  elseif len(trim(DSPKEY(62))) > 0 and len(trim(dspkey(15)))=0 THEN
  '     formValid=False
  '     message="主線線路申請時，線路速率不可空白"                
  '  elseif len(trim(DSPKEY(62))) > 0 and len(trim(dspkey(18)))=0 THEN
  '     formValid=False
  '     message="主線線路申請時，線路IP數量不可空白"                       
  '  elseif len(trim(DSPKEY(48))) > 0 and len(trim(dspkey(14)))=0 THEN
  '     formValid=False
  '     message="主線(CHT通知測通日)輸入時，主線[附掛電話]不可空白"                   
  '  elseif len(trim(DSPKEY(48))) > 0 and len(trim(dspkey(8)))=0 THEN
  '     formValid=False
  '     message="主線(CHT通知測通日)輸入時，(主線網路IP)不可空白"     
  '  elseif len(trim(DSPKEY(48))) > 0 and len(trim(dspkey(10)))=0 THEN
  '     formValid=False
  '     message="主線(CHT通知測通日)輸入時，(主線網路subnet)不可空白"         
  '  elseif len(trim(DSPKEY(48))) > 0 and len(trim(dspkey(9)))=0 THEN
  '     formValid=False
  '     message="主線(CHT通知測通日)輸入時，(主線網路Gateway IP)不可空白"     
  '  elseif len(trim(DSPKEY(48))) > 0 and len(trim(dspkey(11)))=0 THEN
  '     formValid=False
  '     message="主線(CHT通知測通日)輸入時，(主線網路DNS IP)不可空白"     
  '  elseif len(trim(DSPKEY(48))) > 0 and dspkey(17)="02" AND ( len(trim(DSPKEY(12)))=0 OR len(trim(DSPKEY(13)))=0 ) THEN
  '     formValid=False
  '     message="主線線路種類為PPPOE時，PPPOE帳號及密碼不可空白"            
    end if
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(53)=V(0)
        dspkey(54)=datevalue(now())
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
   Sub Srcounty21onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY20").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key21").value =  trim(Fusrid(0))
          document.all("key43").value =  trim(Fusrid(1))
       End if       
       end if
   End Sub       

   Sub Srsalesgrouponclick()
       prog="RTGetsalesgroupD.asp"
       prog=prog & "?KEY=" & document.all("KEY4").VALUE 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key5").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub        
   Sub Srsalesonclick()
       prog="RTGetsalesD.asp"
       prog=prog & "?KEY=" & document.all("KEY4").VALUE & ";" & document.all("KEY5").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key6").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub      
   Sub Srsales79onclick()
       prog="RTGetsalesD2.asp"
       prog=prog & "?KEY=" & document.all("KEY3").VALUE 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key79").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub
   Sub SrDeveloperonclick()
       prog="RTGetDeveloperD.asp"
       prog=prog & "?KEY=" & document.all("KEY7").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key7").value =  trim(Fusrid(0))
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
       if window.SRTAB1.style.display="" then
          window.SRTAB1.style.display="none"
       elseif window.SRTAB1.style.display="none" then
          window.SRTAB1.style.display=""
       end if
   End Sub        
   Sub SrTAG2()
       if window.SRTAB2.style.display="" then
          window.SRTAB2.style.display="none"
       elseif window.SRTAB2.style.display="none" then
          window.SRTAB2.style.display=""
       end if
   End Sub          
   Sub SrTAG3()
       if window.SRTAB3.style.display="" then
          window.SRTAB3.style.display="none"
       elseif window.SRTAB3.style.display="none" then
          window.SRTAB3.style.display=""
       end if
   End Sub         
   Sub SrTAG4()
       if window.SRTAB4.style.display="" then
          window.SRTAB4.style.display="none"
       elseif window.SRTAB4.style.display="none" then
          window.SRTAB4.style.display=""
       end if
   End Sub             
   Sub SrTAG5()
       if window.SRTAB5.style.display="" then
          window.SRTAB5.style.display="none"
       elseif window.SRTAB5.style.display="none" then
          window.SRTAB5.style.display=""
       end if
   End Sub                     
   Sub SrTAG6()
       if window.SRTAB6.style.display="" then
          window.SRTAB6.style.display="none"
       elseif window.SRTAB6.style.display="none" then
          window.SRTAB6.style.display=""
       end if
   End Sub                    
   Sub SrTAG7()
       if window.SRTAB7.style.display="" then
          window.SRTAB7.style.display="none"
       elseif window.SRTAB7.style.display="none" then
          window.SRTAB7.style.display=""
       end if
   End Sub        
   Sub SrTAG8()
       if window.SRTAB8.style.display="" then
          window.SRTAB8.style.display="none"
       elseif window.SRTAB8.style.display="none" then
          window.SRTAB8.style.display=""
       end if
   End Sub              
   Sub SrTAG9()
       if window.SRTAB9.style.display="" then
          window.SRTAB9.style.display="none"
       elseif window.SRTAB9.style.display="none" then
          window.SRTAB9.style.display=""
       end if
   End Sub           
   Sub SrTAG10()
       msgbox "aaa"
       if window.SRTAB10.style.display="" then
          window.SRTAB10.style.display="none"
       elseif window.SRTAB10.style.display="none" then
          window.SRTAB10.style.display=""
       end if
   End Sub             
   Sub SrTAG11()
       if window.SRTAB11.style.display="" then
          window.SRTAB11.style.display="none"
       elseif window.SRTAB11.style.display="none" then
          window.SRTAB11.style.display=""
       end if
   End Sub         
   Sub SrTAG12()
       if window.SRTAB12.style.display="" then
          window.SRTAB12.style.display="none"
       elseif window.SRTAB12.style.display="none" then
          window.SRTAB12.style.display=""
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
          document.all("KEY59").VALUE="元訊寬頻網路股份有限公司"
          document.all("KEY60").VALUE="70770184"
          document.all("OPT2").checked=false
          document.all("KEY59").CLASSNAME="dataListDATA"
          document.all("KEY59").READONLY=TRUE
          document.all("KEY60").CLASSNAME="dataListDATA"
          document.all("KEY60").READONLY=TRUE
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
          document.all("KEY59").VALUE=""
          document.all("KEY60").VALUE=""
          document.all("OPT1").checked=false
          document.all("KEY59").CLASSNAME="dataListENTRY"
          document.all("KEY59").READONLY=FALSE
          document.all("KEY60").CLASSNAME="dataListENTRY"
          document.all("KEY60").READONLY=FALSE
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
        if len(trim(dspkey(51))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(51)=V(0)
        End if  
       dspkey(52)=datevalue(now())
    else
        if len(trim(dspkey(53))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(53)=V(0)
        End if         
        dspkey(54)=datevalue(now())
    end if      
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
    '主線申請後(DSPKEY62),基本資料 protect
    '920218改為以主線申請送件單號為控制點DSPKEY(80)
    If len(trim(dspKey(62))) > 0 or len(trim(dspKey(50))) > 0 Then
       fieldPa=" class=""dataListData"" readonly "
       FIELDPC=" DISABLED "
    Else
       fieldPa=""
       FIELDPC=""
    End If
    If len(trim(dspKey(50))) > 0 Then
       fieldPB=" class=""dataListData"" readonly "
       FIELDPD=" DISABLED "
    Else
       fieldPB=""
       FIELDPD=""
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
  <span id="tags1" class="dataListTagsOn">ET-City主線資訊</span>
                                                            
  <div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td><td>     
    <DIV ID="SRTAG0" onclick="srtag0" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="CENTER">基本資料</td></tr></table></div>
 <DIV ID=SRTAB0 >   
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
<tr><td width="15%" class=dataListHEAD>收件日</td>
    <td width="35%" bgcolor="silver" >
        <input type="text" name="key44" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(44)%>"  READONLY size="10" class=dataListEntry>
       <input  type="button" id="B44"  <%=FIELDPC%>  <%=FIELDPF%>  name="B44" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPC%>  <%=FIELDPF%>  alt="清除" id="C44"  name="C44"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
                             </td>
    <td width="15%" class=dataListHEAD>主線群組</td>
    <td width="35%" bgcolor="silver" >
    <input type="text" name="key2" size="2" MAXLENGTH=2 value="<%=dspKey(2)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text46">
    </td>        
</tr>
<tr><td width="15%" class=dataListHEAD>申請人姓名</td>
    <td width="35%" bgcolor="silver" >
        <input type="text" name="key63" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="30"
               value="<%=dspKey(63)%>"  READONLY size="30" class=dataListEntry>
        </td>
    <td width="15%" class=dataListHEAD>申請人身份證(統編)</td>
    <td width="35%" bgcolor="silver" >
    <input type="text" name="key64" size="10" MAXLENGTH=10 value="<%=dspKey(64)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text46">
    </td>        
</tr>
<tr><td width="15%" class=dataListHEAD>申請人連絡電話</td>
    <td width="35%" bgcolor="silver" >
        <input type="text" name="key65" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="15"
               value="<%=dspKey(65)%>"  READONLY size="15" class=dataListEntry>
        </td>
    <td width="15%" class=dataListHEAD>申請人行動電話</td>
    <td width="35%" bgcolor="silver" >
    <input type="text" name="key66" size="10" MAXLENGTH=10 value="<%=dspKey(66)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text46">
    </td>        
</tr>
<tr><td class=dataListHEAD>ADSL裝機地址</td>
    <td bgcolor="silver" COLSPAN=3>
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND len(trim(dspKey(59))) = 0   AND FIELDPA = "" Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(20))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX21=" onclick=""Srcounty21onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(20) & "' " 
       SXX21=""
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
         <select size="1" name="key20" <%=fieldpA%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select2"><%=s%></select>
        <input type="text" name="key21" readonly  size="8" value="<%=dspkey(21)%>" maxlength="10" readonly <%=fieldpA%><%=fieldpB%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text4">                 
         <input type="button" id="B21"   <%=FIELDPC%>  <%=FIELDPF%>  name="B21"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX21%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPC%>  <%=FIELDPF%>  alt="清除" id="C21"  name="C21"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
        
        <input type="text" name="key22" <%=fieldpA%> size="10" value="<%=dspkey(22)%>" maxlength="10" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text5"><font size=2>
        <% aryOption=Array("村","里")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1    AND FIELDPA = "" Then
      For i = 0 To Ubound(aryOption)
          If dspKey(23)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(23) &""">" &dspKey(23) &"</option>"
   End If%>                                  
       <select size="1" name="key23" <%=fieldpA%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select3">                                            
        <%=s%></select>      
        <input type="text" name="key24"  size="6" value="<%=dspkey(24)%>" maxlength="6" <%=fieldpA%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text6"><font size=2>
        <% aryOption=Array("鄰")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1    AND FIELDPA = "" Then
      For i = 0 To Ubound(aryOption)
          If dspKey(25)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(25) &""">" &dspKey(25) &"</option>"
   End If%>                                  
       <select size="1" name="key25" <%=fieldpA%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select4">                                            
        <%=s%></select>              
        <input type="text" name="key26" size="10" value="<%=dspkey(26)%>" maxlength="10" <%=fieldpA%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text27"><font size=2>
        <% aryOption=Array("路","街")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1    AND FIELDPA = "" Then
      For i = 0 To Ubound(aryOption)
          If dspKey(27)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(27) &""">" &dspKey(27) &"</option>"
   End If%>                                  
       <select size="1" name="key27" <%=fieldpA%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select5">                                            
        <%=s%></select>                      
        <input type="text" name="key28"  size="6" value="<%=dspkey(28)%>" maxlength="6" <%=fieldpA%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text29"><font size=2>
        <% aryOption=Array("段")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1   AND FIELDPA = ""  Then
      For i = 0 To Ubound(aryOption)
          If dspKey(29)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(29) &""">" &dspKey(29) &"</option>"
   End If%>                                  
       <select size="1" name="key29" <%=fieldpA%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select6">                                            
        <%=s%></select>
        <input type="text" name="key30" size="6" value="<%=dspkey(30)%>" maxlength="6" <%=fieldpA%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text30"><font size=2>
        <% aryOption=Array("巷")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1   AND FIELDPA = ""  Then
      For i = 0 To Ubound(aryOption)
          If dspKey(31)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(31) &""">" &dspKey(31) &"</option>"
   End If%>                                  
       <select size="1" name="key31" <%=fieldpA%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select9">                                            
        <%=s%></select>
        <input type="text" name="key43"  readonly size="5" value="<%=dspkey(43)%>" maxlength="5" <%=fieldpA%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text35">        
        <br>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         
        
        <input type="text" name="key34" size="10" value="<%=dspkey(34)%>" maxlength="6" <%=fieldpA%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text31"><font size=2>
                <% aryOption=Array("弄")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1    AND FIELDPA = "" Then
      For i = 0 To Ubound(aryOption)
          If dspKey(35)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(35) &""">" &dspKey(35) &"</option>"
   End If%>                                  
       <select size="1" name="key35" <%=fieldpA%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select10">                                            
        <%=s%></select>    
        <input type="text" name="key36" size="6" value="<%=dspkey(36)%>" maxlength="6" <%=fieldpA%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text32"><font size=2>
                <% aryOption=Array("號")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1    AND FIELDPA = "" Then
      For i = 0 To Ubound(aryOption)
          If dspKey(37)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(37) &""">" &dspKey(37) &"</option>"
   End If%>                                  
       <select size="1" name="key37" <%=fieldpA%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select11">                                            
        <%=s%></select>            
        <input type="text" name="key38" size="10" value="<%=dspkey(38)%>" maxlength="6" <%=fieldpA%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text33"><font size=2>
                <% aryOption=Array("樓")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1    AND FIELDPA = "" Then
      For i = 0 To Ubound(aryOption)
          If dspKey(39)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(39) &""">" &dspKey(39) &"</option>"
   End If%>                                  
       <select size="1" name="key39" <%=fieldpA%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select12">                                            
        <%=s%></select>
        <input type="text" name="key40" size="6" value="<%=dspkey(40)%>" maxlength="6" <%=fieldpA%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text34"><font size=2>
                <% aryOption=Array("室")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1   AND FIELDPA = ""  Then
      For i = 0 To Ubound(aryOption)
          If dspKey(41)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(41) &""">" &dspKey(41) &"</option>"
   End If%>                                  
       <select size="1" name="key41" <%=fieldpA%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select13">                                            
        <%=s%></select>
        <input type="text" name="key32" size="12" value="<%=dspkey(32)%>" maxlength="6" <%=fieldpA%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text31"><font size=2>
                <% aryOption=Array("部落")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1    AND FIELDPA = "" Then
      For i = 0 To Ubound(aryOption)
          If dspKey(33)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(33) &""">" &dspKey(33) &"</option>"
   End If%>                                  
       <select size="1" name="key33" <%=fieldpA%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select10">                                            
        <%=s%></select>  
        <br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <font size=2>設備位置</font>       
 <input type="text" name="key42" size="30" value="<%=dspkey(42)%>" maxlength="30" <%=fieldpA%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text21">             
        </td>                                 
</tr>  
<tr><td class=dataListHEAD >可供裝範圍</td>
<td bgcolor="silver" COLSPAN=3>
<input type="text" name="key58" size="90" value="<%=dspkey(58)%>" maxlength="90" <%=fieldpA%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text62">
</tr>
<tr>
        <td  width="15%" class="dataListHEAD" height="23">建檔人員</td>                                 
        <td  width="35%" height="23" bgcolor="silver">
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
  %>    <input type="text" name="key51" size="6" READONLY value="<%=dspKey(51)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  width="15%" class="dataListHEAD" height="23">建檔日期</td>                                 
        <td  width="35%" height="23" bgcolor="silver">
        <input type="text" name="key52" size="10" READONLY value="<%=dspKey(52)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>  
<tr>
        <td  class="dataListHEAD" height="23">修改人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
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
  %>    <input type="text" name="key53" size="6" READONLY value="<%=dspKey(53)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">修改日期</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key54" size="10" READONLY value="<%=dspKey(54)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>                
</table> </div>
<!--
    <table border="1" width="100%" cellpadding="0" cellspacing="0" id="tag2" style="display: none"> 
    -->
    <DIV ID="srtag6" onclick="srtag6" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table10">
    <tr><td bgcolor="BDB76B" align="CENTER">藉名裝機</td></tr></table></div>
     <DIV ID="srtab6" >
     <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table11">    
     <tr><td colspan=4>
     <% '當主線未申請前才可變更借名裝機資料
     IF LEN(TRIM(DSPKEY(48))) > 0 THEN
        SROPT1=""
        SROPT2=""
        OPT1=" DISABLED "
        OPT2=" DISABLED "
     ELSE
        SROPT1=" ONCLICK=""SROPT1CLICK()"" "
        SROPT2=" ONCLICK=""SROPT2CLICK()"" "
        OPT1=""
        opt2=""
     END IF
     %>
     <input type="checkbox" <%=OPT1%> name="OPT1" ID="OPT1" size="1" VALUE="1" <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" <%=SROPT1%>><FONT size=2>預設值</FONT>
     <input type="checkbox" <%=OPT2%> name="OPT2" ID="OPT2" size="1" VALUE="2" <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry"  <%=SROPT2%>><FONT size=2>藉名申請</FONT>     
     </td></tr>
     <tr>
     <td  WIDTH="15%" class="dataListSEARCH" height="23">借名用戶名稱</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
          
        <input type="text" name="key59" size="30"  maxlength="10" value="<%=dspKey(59)%>"  <%=fieldpA%><%=fieldRole(1)%>  readonly class="dataListDATA" ID="Text69"></td>        
      <td  WIDTH="15%" class="dataListSEARCH" height="23">借名用戶身份證號</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
       
        <input type="text" name="key60" size="20"  maxlength="10" value="<%=dspKey(60)%>"  <%=fieldpA%><%=fieldRole(1)%>  readonly class="dataListDATA" ID="Text70"></td>        
   </tr>
<tr><td width="15%" class=dataListHEAD>借名用戶連絡電話</td>
    <td width="35%" bgcolor="silver" >
        <input type="text" name="key67" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="15"
               value="<%=dspKey(67)%>"  READONLY size="15" class=dataListEntry>
        </td>
    <td width="15%" class=dataListHEAD>借名用戶行動電話</td>
    <td width="35%" bgcolor="silver" >
    <input type="text" name="key68" size="10" MAXLENGTH=10 value="<%=dspKey(68)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text46">
    </td>        
</tr>   
   </table>
   </div></div>
   <DIV ID="SRTAG1" onclick="srtag1" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="CENTER">績效歸屬</td></tr></table></div>
     <DIV ID=SRTAB1>
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table7">
<tr>   <td id="tagT1" WIDTH="15%" class="dataListHEAD" height="23">業務轄區</td>               
        <td  WIDTH="85%" height="23" bgcolor="silver" colspan=3>
<%  If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(62))) = 0  Then 
       sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '1') "
       s="<option value="""" >(業務轄區)</option>"
    Else
       sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '1') "
       s="<option value="""" >(業務轄區)</option>"
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(業務轄區)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("areaid")=dspkey(4) Then sx=" selected "
       s=s &"<option value=""" &rs("areaid") &"""" &sx &">" &rs("areanc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>    
           <select size="1" name="key4" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" ID="Select1">                                            
              <%=s%>
           </select>
           
    <input type="text" name="key5" <%=fieldRole(1)%><%=dataProtect%> <%=fieldpa%>
               style="text-align:left;" size="15" maxlength="10" 
               value="<%=dspKey(5)%>" readonly class="dataListEntry" ID="Text64">
         <input type="button" id="B5" name="B5" <%=fieldRole(1)%> <%=fieldpc%>width="100%" style="Z-INDEX: 1" value="...." readonly onclick="SrsalesGrouponclick()" > 
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldRole(1)%> <%=fieldpc%> alt="清除" id="C5" name="C5" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
         <%name="" 
           if dspkey(6) <> "" then
              sqlxx=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(6) & "' "
              rs.Open sqlxx,conn
              if rs.eof then
                 name="(對象檔找不到業務員)"
              else
                 name=rs("cusnc")
              end if
               rs.close
           end if
          
        %>
        <input type="TEXT" name="key6" <%=fieldRole(1)%><%=dataProtect%> 
               style="text-align:left;" size="15" maxlength="10" 
               value="<%=dspKey(6)%>"  readonly class="dataListDATA" ID="Hidden1">
           <input type="BUTTON" id="B6" name="B6" <%=fieldRole(1)%> <%=fieldpc%>width="100%" style="Z-INDEX: 1"  value="...." onclick="Srsalesonclick()"  >   
           <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldRole(1)%> <%=fieldpc%>alt="清除" id="C6"  name="C6"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">
           <font size=2><%=name%></font>                               
        </td></tr>
	<tr>    <td width="15%" class=dataListHEAD>經銷商</td>
    <td width="35%" bgcolor="silver" >
<%  If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 AND len(trim(dspKey(62))) = 0   AND FIELDPA = "" Then 
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
          &"WHERE (RTObjLink.CUSTYID = '02')  "
       s="<option value="""" >(經銷商)</option>"
    Else
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
          &"WHERE (RTObjLink.CUSTYID = '02')  and rtobj.cusid='" & dspkey(3) & "' "
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(經銷商)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(3) Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>
           <select size="1" name="key3" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" ID="Select34">                                            
              <%=s%>
           </select></td>   <td WIDTH="15%" class="dataListHEAD" height="23">二線負責人</td>
		<td width="35%"><input type="text" name="key7" value="<%=dspKey(7)%>" <%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" size="8" maxlength="6" readonly class="dataListDATA" ID="Text82">
			<input type="BUTTON" id="B7" name="B7" style="Z-INDEX: 1" <%=fieldpc%> value="...." onclick="Srdeveloperonclick()">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=fieldpc%> alt="清除" id="C7" name="C7" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">
			<font size=2><%=name%></font></td>
			
	</tr>  </table>     
  </DIV> 
  </DIV>   
  
    <DIV ID="SRTAG3" onclick="srtag3" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table4">
    <tr><td bgcolor="BDB76B" align="CENTER">網路資料</td></tr></table></DIV>
   <DIV ID=SRTAB3 > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table5">
<tr>   <td  WIDTH="15%" class="dataListHEAD" height="23">線路ISP</td>               
        <td  WIDTH="35%" height="23" bgcolor="silver" >
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(62))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='C3' " 
       If len(trim(dspkey(16))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & ">(線路ISP)</option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & ">(線路ISP)</option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='C3' AND CODE='" & dspkey(16) & "'"
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
        <td  WIDTH="15%" class="dataListHEAD" height="23">線路IP種類</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(62))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M5' " 
       If len(trim(dspkey(17))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & ">(線路IP種類)</option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & ">(線路IP種類)</option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M5' AND CODE='" & dspkey(17) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(17) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key17" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35">                                                                  
        <%=s%>
   </select></td>                                 
                              
 </tr>         
<tr>   <td  WIDTH="15%" class="dataListHEAD" height="23">主線速率</td>               
        <td  WIDTH="35%" height="23" bgcolor="silver" >
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(62))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D3' " 
       If len(trim(dspkey(15))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & ">(主線速率)</option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & ">(主線速率)</option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D3' AND CODE='" & dspkey(15) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(15) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key15" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35">                                                                  
        <%=s%>
   </select>
        </td>
        <td  WIDTH="15%" class="dataListHEAD" height="23">附掛電話</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key14" size="15" maxlength="15" value="<%=dspKey(14)%>"  <%=fieldpB%><%=fieldRole(1)%> class="dataListEntry" ID="Text43"></td>                                 
                              
 </tr>       
<tr>                                 
        <td  WIDTH="15%" class="dataListHEAD" height="23">主線網路IP</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key8" size="20"  maxlength="20" value="<%=dspKey(8)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListEntry" ID="Text37"></td>        
        <td  WIDTH="15%" class="dataListHEAD" height="23">IP數量</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key18" size="5"  maxlength="5" value="<%=dspKey(18)%>"  <%=fieldpA%><%=fieldRole(1)%> class="dataListEntry" ID="Text37"></td>        
 
 </tr>        
<tr>     <td  WIDTH="15%" class="dataListHEAD" height="23">主線SUBNET</td>                                 
   <% aryOption=Array("","255.255.255.0","255.255.255.128")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1  AND FIELDPB = "" Then 
      For i = 0 To Ubound(aryOption)
          If dspKey(10)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(10) &""">" &dspKey(10) &"</option>"
   End If%>                                 
        <td width="35%" height="23" bgcolor="silver">
        <select size="1" name="key10" <%=fieldpB%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select14">                                 
        <%=s%>
        </select></td>                                
        <td  class="dataListHEAD" height="23">閘道IP(Gateway)</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key9" size="20"   maxlength="20" value="<%=dspKey(9)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListEntry" ID="Text38"></td>        
 <tr>        
        <td  class="dataListHEAD" height="23">DNS IP</td>                                 
        <td  height="23" bgcolor="silver" COLSPAN=3>
        <input type="text" name="key11" size="20" maxlength="20" value="<%=dspKey(11)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListEntry" ID="Text40"></td>                                 
 </tr>     
 <tr>                                 
        <td  class="dataListHEAD" height="23">PPPOE撥接帳號</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key12" size="10" maxlength="10" value="<%=dspKey(12)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListEntry" ID="Text41"></td>        
        <td  class="dataListHEAD" height="23">PPPOE撥接密碼</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key13" size="10" maxlength="10" value="<%=dspKey(13)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListEntry" ID="Text42"></td>                                 
 </tr>    
  

  </table>   
  </DIV>
      <DIV ID="SRTAG4" onclick="srtag4" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr><td bgcolor="BDB76B" align="CENTER">主線申請及施工進度狀態</td></tr></table></DIV>
    <DIV ID=SRTAB4 >  
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr>
        <td  WIDTH="15%" class="dataListHEAD" height="23">主線勘察日</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key45" size="10"  READONLY  value="<%=dspKey(45)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text44">     
        <input type="button" id="B45"   <%=FIELDPC%>   name="B45" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPC%>  alt="清除" id="C45"  name="C45"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">  </td>         
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
    If dspKey(46)="Y" Then sexd5=" checked "    
    If dspKey(46)="N" Then sexd6=" checked " %>           
        <input type="RADIO" <%=sexd5%> name="key46" size="1" value="Y"  <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry" ID="radio" >可
        <input type="RADIO" <%=sexd6%> name="key46" size="1" value="N"  <%=fieldpc%><%=fieldRole(1)%> class="dataListEntry" ID="radio" >不可                
         </td></tr>
    <tr>
        <td   class="dataListHEAD" height="23">不可建置原因</td>                                 
        <td   height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key47" size="90" MAXLENGTH=90 value="<%=dspKey(47)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text46"></td>        
    </tr>
         <tr>
        <td   class="dataListHEAD" height="23">線路申請日</td>                                 
        <td   height="23" bgcolor="silver">
        <input type="text" name="key62" size="10"   READONLY value="<%=dspKey(62)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListentry" ID="Text55">     
        <input type="button" id="B62"  <%=FIELDPD%>    name="B62" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPD%>  alt="清除" id="C62"  name="C62"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"> </td>          
        <td   class="dataListHEAD" height="23">續約申請日</td>                                 
        <td   height="23" bgcolor="silver">
        <input type="text" name="key69" size="10"   READONLY value="<%=dspKey(69)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListentry" ID="Text55">     
        <input type="button" id="B69"  <%=FIELDPD%>    name="B69" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPD%>  alt="清除" id="C69"  name="C69"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"> </td>          
      </tr>       
         <tr>
         <td   class="dataListHEAD" height="23">CHT通知測通日</td>                                 
        <td   height="23" bgcolor="silver">
        <input type="text" name="key48" size="10"   READONLY value="<%=dspKey(48)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListentry" ID="Text55">     
        <input type="button" id="B48"  <%=FIELDPD%>    name="B48" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPD%>   alt="清除" id="C48"  name="C48"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"> </td>          
      <td   class="dataListHEAD" height="23">主線測通日</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key50" size="10"   READONLY value="<%=dspKey(50)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListDATA" ID="Text56">
        </td>   
       </tr>             
      <tr>
        <td   class="dataListSEARCH" height="23">線路到期日</td>                                 
        <td   height="23" bgcolor="silver" >
        <input type="text" name="key19" size="10"   READONLY value="<%=dspKey(19)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListentry" ID="Text55">     
        <input type="button" id="B19"  <%=FIELDPD%>   name="B19" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPD%> alt="清除" id="C19"  name="C19"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"> </td>          
        <td  class="dataListHEAD" height="23">撤線日</td>                                 
        <td   height="23" bgcolor="silver">
                <input type="text" name="key57" size="10" value="<%=dspKey(57)%>"  <%=fieldpb%><%=fieldRole(1)%> readonly class="dataListDATA" ID="Text71">     
      <FONT SIZE=2>撤線種類︰</FONT>
      <%
    s=""
    sx=" selected "
    sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='N9' AND CODE='" & dspkey(70) & "'"
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(70) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key70"  READONLY <%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Select35">                                                                  
        <%=s%>
   </select>
      </TD>
      </tr>            
      <tr>
        <td  class="dataListHEAD" height="23">作廢日</td>                                 
        <td   height="23" bgcolor="silver" >
        <input type="text" name="key55" size="10" value="<%=dspKey(55)%>"  <%=fieldpb%><%=fieldRole(1)%> readonly class="dataListDATA" ID="Text72">     </td>
       <td  class="dataListHEAD" height="23">作廢人員</td>                                 
        <td   height="23" bgcolor="silver" >
        <%  name="" 
           if dspkey(56) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(56) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>         
        <input type="text" name="key56" size="6" value="<%=dspKey(56)%>"  <%=fieldpb%><%=fieldRole(1)%> readonly class="dataListDATA" ID="Text71"><font size=2><%=name%>
      </tr>                         
  </table> 
  </DIV>
    <DIV ID="SRTAG5" onclick="srtag5" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="CENTER">備註說明</td></tr></table></DIV>
   <DIV ID="SRTAB5" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
    <TR><TD align="CENTER">
     <TEXTAREA  cols="100%"  name="key61" rows=8  MAXLENGTH=500  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(61)%>" ID="Textarea1"><%=dspkey(61)%></TEXTAREA>
   </td></tr>

<tr>                                   
  </div> 
  <% Set rsxx=Server.CreateObject("ADODB.Recordset")
     sqlfaqlist="SELECT COUNT(*) AS CNT FROM RTLessorCmtylinesndwork WHERE comq1=" & DSPKEY(0) & " and lineq1=" & dspkey(1) & " AND dropdat IS NULL "
     rsxx.open sqlfaqlist,conn
     IF RSXX("CNT") > 0 THEN
        XXsndFLAG="Y"
     ELSE
        XXsndFLAG=""
     END IF
     RSXX.CLOSE
     SET RSXX=NOTHING
   %>
    <% if XXsndFLAG = "Y" then %>
   <DIV ID="SRTAG10" onclick="SRTAG10" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr><td bgcolor="BDB76B" align="CENTER">主線派工資料</td></tr></table></DIV>
    <DIV ID="SRTAB10" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr class="dataListsearch"><td colspan=15 align=center><A HREF="/webap/rtap/BASE/RTlessorCMTY/RTLessorcmtylinesndworkk.asp?V=<%=XRND%>&accessMode=U&key=<%=dspkey(0)%>;<%=dspkey(1)%>;" TARGET="NEWWINDOW" ><font color=white>主線派工資料明細</font></A></td></tr>
    <tr class="dataListHEAD" align=center><td>主線</td><td>派工單號</td><td>派工類別</td><td>派工日期</td><td>列印人員</td><td>預定<br>施工員</td><td>實際<br>施工員</td><td>結案日</td><td>未完工<br>結案日</td><td>設備<br>數量</td><td>轉領用單<br>數量</td><td>已領<br>數量</td><td>待領<br>數量</td><td>處理<br>天數</td></tr>
    <%
           Set rsxx=Server.CreateObject("ADODB.Recordset")
           sqlfaqlist="SELECT RTLessorCmtyLineSNDWORK.comq1,RTLessorCmtyLineSNDWORK.lineq1,rtrim(convert(char(6),RTLessorCmtyLineSNDWORK.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorCmtyLineSNDWORK.lineQ1))  as comqline, RTLessorCmtyLineSNDWORK.PRTNO,RTCODE_9.CODENC as codenc1, RTLessorCmtyLineSNDWORK.SENDWORKDAT, " _
           &"RTOBJ.CUSNC as cusnc1,CASE WHEN RTOBJ_2.SHORTNC <>'' THEN RTOBJ_2.SHORTNC ELSE RTOBJ_1.CUSNC END cusnc2,CASE WHEN RTOBJ_4.SHORTNC <>'' THEN RTOBJ_4.SHORTNC ELSE RTOBJ_3.CUSNC END cusnc3, " _
           &"RTLessorCmtyLineSNDWORK.closedat,RTLessorCmtyLineSNDWORK.unclosedat,RTLessorCmtyLineSNDWORK.BONUSCLOSEYM, RTLessorCmtyLineSNDWORK.BONUSFINCHK,RTLessorCmtyLineSNDWORK.batchno, RTLessorCmtyLineSNDWORK.STOCKCLOSEYM, RTLessorCmtyLineSNDWORK.STOCKFINCHK, " _
           &"RTLessorCmtyLineSNDWORK.DROPDAT ,SUM(CASE WHEN RTLessorCmtyLineHARDWARE.dropdat IS NULL AND RTLessorCmtyLineHARDWARE.QTY > 0 " _
           &"THEN RTLessorCmtyLineHARDWARE.QTY ELSE 0 END) as qty1, SUM(CASE WHEN RTLessorCmtyLineHARDWARE.dropdat IS NULL AND " _
           &"RCVPRTNO <> '' THEN RTLessorCmtyLineHARDWARE.QTY ELSE 0 END) as qty2, SUM(CASE WHEN RTLessorCmtyLineHARDWARE.dropdat IS NULL " _
           &"AND RCVPRTNO <> '' AND RTLessorCmtyLineHARDWARE.rcvfinishdat IS NOT NULL THEN RTLessorCmtyLineHARDWARE.QTY ELSE 0 END) as qty3 , " _
           &"SUM(CASE WHEN RTLessorCmtyLineHARDWARE.dropdat IS NULL AND RTLessorCmtyLineHARDWARE.QTY > 0 THEN RTLessorCmtyLineHARDWARE.QTY ELSE 0 END) - " _
           &"SUM(CASE WHEN RTLessorCmtyLineHARDWARE.dropdat IS NULL AND RCVPRTNO <> '' AND RTLessorCmtyLineHARDWARE.rcvfinishdat IS NOT NULL THEN RTLessorCmtyLineHARDWARE.QTY ELSE 0 END) as qty4, " _
           &"case when RTLessorCmtyLineSNDWORK.closedat is not null then datediff(dd,RTLessorCmtyLineSNDWORK.sendworkdat,RTLessorCmtyLineSNDWORK.closedat) when RTLessorCmtyLineSNDWORK.unclosedat is not null then datediff(dd,RTLessorCmtyLineSNDWORK.sendworkdat,RTLessorCmtyLineSNDWORK.unclosedat) else 0 end as PROCESSDAT " _
           &"FROM RTLessorCmtyLineSNDWORK LEFT OUTER JOIN RTObj RTObj_4 ON RTLessorCmtyLineSNDWORK.REALCONSIGNEE = RTObj_4.CUSID LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_2 INNER JOIN RTObj RTObj_3 ON RTEmployee_2.CUSID = RTObj_3.CUSID ON RTLessorCmtyLineSNDWORK.REALENGINEER = RTEmployee_2.EMPLY LEFT OUTER JOIN " _
           &"RTObj RTObj_2 ON RTLessorCmtyLineSNDWORK.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN " _
           &"RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON RTLessorCmtyLineSNDWORK.ASSIGNENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
           &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON RTLessorCmtyLineSNDWORK.PRTUSR = RTEmployee.EMPLY  " _
           &"LEFT OUTER JOIN RTLessorCmtyLineHARDWARE ON RTLessorCmtyLineSNDWORK.COMQ1=RTLessorCmtyLineHARDWARE.COMQ1 AND RTLessorCmtyLineSNDWORK.LINEQ1=RTLessorCmtyLineHARDWARE.LINEQ1 " _
           &"AND RTLessorCmtyLineSNDWORK.PRTNO=RTLessorCmtyLineHARDWARE.PRTNO left outer join rtcode rtcode_9 on " _
           &"RTLessorCmtyLineSNDWORK.sndkind=rtcode_9.code and rtcode_9.kind='G9' " _
           &"where RTLessorCmtyLineSNDWORK.comq1=" & dspkey(0) & " and RTLessorCmtyLineSNDWORK.lineq1=" & dspkey(1)  & " " _
           &"GROUP BY  RTLessorCmtyLineSNDWORK.comq1,RTLessorCmtyLineSNDWORK.lineq1, RTLessorCmtyLineSNDWORK.PRTNO, " _
           &"rtrim(CONVERT(char(6), RTLessorCmtyLineSNDWORK.COMQ1)) + '-' + rtrim(CONVERT(char(6), RTLessorCmtyLineSNDWORK.lineQ1)), " _
           &"RTLessorCmtyLineSNDWORK.PRTNO,RTCODE_9.CODENC, RTLessorCmtyLineSNDWORK.SENDWORKDAT, RTOBJ.CUSNC, " _
           &"CASE WHEN RTOBJ_2.SHORTNC <> '' THEN RTOBJ_2.SHORTNC ELSE RTOBJ_1.CUSNC END, " _
           &"CASE WHEN RTOBJ_4.SHORTNC <> '' THEN RTOBJ_4.SHORTNC ELSE RTOBJ_3.CUSNC END, RTLessorCmtyLineSNDWORK.closedat, " _
           &"RTLessorCmtyLineSNDWORK.unclosedat, RTLessorCmtyLineSNDWORK.BONUSCLOSEYM, RTLessorCmtyLineSNDWORK.BONUSFINCHK, " _
           &"RTLessorCmtyLineSNDWORK.batchno, RTLessorCmtyLineSNDWORK.STOCKCLOSEYM, RTLessorCmtyLineSNDWORK.STOCKFINCHK, " _
           &"RTLessorCmtyLineSNDWORK.DROPDAT,case when RTLessorCmtyLineSNDWORK.closedat is not null then datediff(dd,RTLessorCmtyLineSNDWORK.sendworkdat,RTLessorCmtyLineSNDWORK.closedat) when RTLessorCmtyLineSNDWORK.unclosedat is not null then datediff(dd,RTLessorCmtyLineSNDWORK.sendworkdat,RTLessorCmtyLineSNDWORK.unclosedat) else 0 end "  
           rsxx.open sqlfaqlist,conn
         '  response.write sqlfaqlist
           Randomize
           XRND=RND()
           do until rsxx.eof %>
           <tr class="dataListentry">
           <td><%=rsxx("comqline")%>&nbsp;</td>
           <td><A HREF="/webap/rtap/BASE/RTlessorCMTY/RTLessorcmtylinesndworkD.asp?V=<%=XRND%>&accessMode=U&key=<%=RSXX("comq1")%>;<%=RSXX("lineq1")%>;<%=RSXX("prtno")%>" TARGET="NEWWINDOW" ><%=RSXX("prtno")%></A></td>
           <td><%=rsxx("CODENC1")%>&nbsp;</td>           
           <td><%=rsxx("SENDWORKDAT")%>&nbsp;</td>
           <td><%=rsxx("cusnc1")%>&nbsp;</td>
           <td><%=rsxx("cusnc2")%>&nbsp;</td>
           <td><%=rsxx("cusnc3")%>&nbsp;</td>
           <td><%=rsxx("closedat")%>&nbsp;</td>
           <td><%=rsxx("unclosedat")%>&nbsp;</td>
           <td><%=rsxx("qty1")%>&nbsp;</td>
           <td><%=rsxx("qty2")%>&nbsp;</td>
           <td><%=rsxx("qty3")%>&nbsp;</td>
           <td><%=rsxx("qty4")%>&nbsp;</td>
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
     sqlfaqlist="SELECT COUNT(*) AS CNT FROM RTLessorCmtylinecont WHERE comq1=" & DSPKEY(0) & " and lineq1=" & dspkey(1) & " AND CANCELDAT IS NULL "
     rsxx.open sqlfaqlist,conn
     IF RSXX("CNT") > 0 THEN
        XXcontFLAG="Y"
     ELSE
        XXcontFLAG=""
     END IF
     RSXX.CLOSE
     SET RSXX=NOTHING
   %>
    <% if XXcontFLAG = "Y" then %>
  <DIV ID="SRTAG9" onclick="SRTAG9" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="CENTER">主線續約資料</td></tr></table></DIV>
    <DIV ID="SRTAB9" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr class="dataListsearch"><td colspan=15 align=center><A HREF="/webap/rtap/BASE/RTlessorCMTY/RTLessorcmtylinecontk.asp?V=<%=XRND%>&accessMode=U&key=<%=dspkey(0)%>;<%=dspkey(1)%>;" TARGET="NEWWINDOW" ><font color=white>主線續約資料明細</font></A></td></tr>
    <tr class="dataListHEAD" align=center><td>項次</td><td>主線</td><td>社區</td><td>主線IP</td><td>主線附掛</td><td>線路ISP</td><td>IP種類</td><td>主線速率</td><td>IP數</td><td>續約申請日</td><td>通測日</td><td>測通日</td><td>線路到期日</td><td>結案日期</td><td>處理<br>天數</td></tr>
    <%
           Set rsxx=Server.CreateObject("ADODB.Recordset")
           sqlfaqlist="SELECT RTLessorCmtyLineCont.COMQ1, RTLessorCmtyLineCont.LINEQ1, RTLessorCmtyLineCont.ENTRYNO,RTLESSORCMTYH.COMN,rtrim(convert(char(6),RTLessorCmtyLineCont.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorCmtyLineCont.lineQ1)) as comnline,  " _
                &"RTLessorCmtyLineCont.LINEIP,RTLessorCmtyLineCont.GATEWAY, " _
                &"RTLessorCmtyLineCont.PPPOEACCOUNT, RTLessorCmtyLineCont.PPPOEPASSWORD, RTLessorCmtyLineCont.LINETEL, " _
                &"RTCode_1.CODENC as codenc1, RTCode_3.CODENC AS codenc2, RTCode_2.CODENC AS codenc3, RTLessorCmtyLineCont.IPCNT, " _
                &"RTLessorCmtyLineCont.CONTAPPLYDAT, RTLessorCmtyLineCont.HINETNOTIFYDAT, " _
                &"RTLessorCmtyLineCont.ADSLAPPLYDAT, RTLessorCmtyLineCont.LINEDUEDAT, RTLessorCmtyLineCont.closedat, " _
                &"RTLessorCmtyLineCont.CANCELDAT,case when RTLessorCmtyLineCont.closedat is not null then datediff(d,RTLessorCmtyLineCont.CONTAPPLYDAT,RTLessorCmtyLineCont.closedat) else datediff(d,RTLessorCmtyLineCont.CONTAPPLYDAT,getdate()) end as PROCESSDAT " _
                &"FROM    RTLessorCmtyLineCont  LEFT OUTER JOIN RTCode RTCode_3 ON " _
                &"RTLessorCmtyLineCont.LINEIPTYPE = RTCode_3.CODE AND RTCode_3.KIND = 'M5' LEFT OUTER JOIN " _
                &"RTCode RTCode_1 ON RTLessorCmtyLineCont.LINEISP = RTCode_1.CODE AND RTCode_1.KIND = 'C3' LEFT OUTER JOIN " _
                &"RTCode RTCode_2 ON RTLessorCmtyLineCont.LINERATE = RTCode_2.CODE AND RTCode_2.KIND = 'D3' " _
                &"LEFT OUTER JOIN RTLESSORCMTYH ON " _
                &"RTLessorCmtyLineCont.COMQ1 = RTLESSORCMTYH.COMQ1 " _
                &"WHERE RTLessorCmtyLineCont.comq1=" & dspkey(0) & " and RTLessorCmtyLineCont.lineq1=" & dspkey(1) & " ORDER BY ENTRYNO" 
           rsxx.open sqlfaqlist,conn
         '  response.write sqlfaqlist
           Randomize
           XRND=RND()
           do until rsxx.eof %>
           <tr class="dataListentry">
           <td><%=rsxx("entryno")%>&nbsp;</td>
           <td><A HREF="/webap/rtap/BASE/RTlessorCMTY/RTLessorcmtylinecontD.asp?V=<%=XRND%>&accessMode=U&key=<%=RSXX("comq1")%>;<%=RSXX("lineq1")%>;<%=RSXX("entryno")%>" TARGET="NEWWINDOW" ><%=RSXX("comq1")%>-<%=RSXX("lineq1")%>-<%=RSXX("entryno")%></A></td>
           <td><%=rsxx("comn")%>&nbsp;</td>           
           <td><%=rsxx("lineip")%>&nbsp;</td>
           <td><%=rsxx("linetel")%>&nbsp;</td>
           <td><%=rsxx("codenc1")%>&nbsp;</td>
           <td><%=rsxx("codenc2")%>&nbsp;</td>
           <td><%=rsxx("codenc3")%>&nbsp;</td>
           <td><%=rsxx("ipcnt")%>&nbsp;</td>
           <td><%=rsxx("CONTAPPLYDAT")%>&nbsp;</td>
           <td><%=rsxx("HINETNOTIFYDAT")%>&nbsp;</td>
           <td><%=rsxx("ADSLAPPLYDAT")%>&nbsp;</td>
           <td><%=rsxx("LINEDUEDAT")%>&nbsp;</td>
           <td><%=rsxx("closedat")%>&nbsp;</td>
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
     sqlfaqlist="SELECT COUNT(*) AS CNT FROM RTLessorCmtylinehardware WHERE comq1=" & DSPKEY(0) & " and lineq1=" & dspkey(1) & " AND dropdat IS NULL "
     rsxx.open sqlfaqlist,conn
     IF RSXX("CNT") > 0 THEN
        XXhwFLAG="Y"
     ELSE
        XXhwFLAG=""
     END IF
     RSXX.CLOSE
     SET RSXX=NOTHING
   %>
    <% if XXhwFLAG = "Y" then %>
  <DIV ID="SRTAG11" onclick="SRTAG11" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="CENTER">主線設備資料</td></tr></table></DIV>
    <DIV ID="SRTAB11" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr class="dataListsearch"><td colspan=15 align=center><A HREF="/webap/rtap/BASE/RTlessorCMTY/RTLessorcmtylinehardwarek2.asp?V=<%=XRND%>&accessMode=U&key=<%=dspkey(0)%>;<%=dspkey(1)%>;" TARGET="NEWWINDOW" ><font color=white>主線設備資料明細</font></A></td></tr>
    <tr class="dataListHEAD" align=center><td>派工單號</td><td>項次</td><td>派工日期</td><td>設備名稱/規格</td><td>數量</td><td>金額</td><td>出庫別</td><td>領用單號</td><td>領用結案日</td></tr>
    <%
           Set rsxx=Server.CreateObject("ADODB.Recordset")
           sqlfaqlist="SELECT RTLessorCmtyLineHARDWARE.comq1,RTLessorCmtyLineHARDWARE.lineq1, " _
         &"RTLessorCmtyLineHARDWARE.PRTNO, RTLessorCmtyLineHARDWARE.seq,RTLessorCmtyLinesndwork.SENDWORKDAT, " _
         &"RTProdH.PRODNC + '--' + RTProdD1.itemnc + '('+ RTProdD1.SPEC+')' as prodc, RTLessorCmtyLineHARDWARE.QTY, RTLessorCmtyLineHARDWARE.amt, " _
         &"HBwarehouse.WARENAME, RTLessorCmtyLineHARDWARE.DROPDAT, RTLessorCmtyLineHARDWARE.DROPREASON, RTObj.CUSNC,RTLessorCmtyLineHARDWARE.BATCHNO,RTLessorCmtyLineHARDWARE.TARDAT,RTLessorCmtyLineHARDWARE.rcvprtno,RTLessorCmtyLineHARDWARE.rcvfinishdat  " _
         &"FROM RTProdH RIGHT OUTER JOIN RTLessorCmtyLineHARDWARE LEFT OUTER JOIN " _
         &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
         &"RTLessorCmtyLineHARDWARE.DROPUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
         &"HBwarehouse ON RTLessorCmtyLineHARDWARE.WAREHOUSE = HBwarehouse.WAREHOUSE LEFT OUTER " _
         &"JOIN RTProdD1 ON RTLessorCmtyLineHARDWARE.PRODNO = RTProdD1.PRODNO AND " _
         &"RTLessorCmtyLineHARDWARE.ITEMNO = RTProdD1.ITEMNO ON RTProdH.PRODNO = RTLessorCmtyLineHARDWARE.PRODNO " _
         &"LEFT OUTER JOIN RTLessorCmtyLinesndwork on RTLessorCmtyLineHARDWARE.comq1=RTLessorCmtyLinesndwork.comq1 and RTLessorCmtyLineHARDWARE.lineq1=RTLessorCmtyLinesndwork.lineq1 and RTLessorCmtyLineHARDWARE.prtno=RTLessorCmtyLinesndwork.prtno " _
         &"WHERE RTLessorCmtyLinehardware.comq1=" & aryparmkey(0) & " and RTLessorCmtyLineHARDWARE.lineq1=" & aryparmkey(1) & " and RTLessorCmtyLineHARDWARE.dropdat is null " 
           rsxx.open sqlfaqlist,conn
         '  response.write sqlfaqlist
           Randomize
           XRND=RND()
           do until rsxx.eof %>
           <tr class="dataListentry">
           <td><%=rsxx("prtno")%>&nbsp;</td>
           <td><A HREF="/webap/rtap/BASE/RTlessorCMTY/RTLessorcmtylinehardwareD.asp?V=<%=XRND%>&accessMode=U&key=<%=RSXX("comq1")%>;<%=RSXX("lineq1")%>;<%=RSXX("prtno")%>;<%=RSXX("seq")%>" TARGET="NEWWINDOW" ><%=RSXX("prtno")%>-<%=RSXX("seq")%></A></td>
           <td><%=rsxx("SENDWORKDAT")%>&nbsp;</td>           
           <td><%=rsxx("prodc")%>&nbsp;</td>
           <td><%=rsxx("qty")%>&nbsp;</td>
           <td><%=rsxx("amt")%>&nbsp;</td>
           <td><%=rsxx("WARENAME")%>&nbsp;</td>
           
           <td><%=rsxx("rcvprtno")%>&nbsp;</td>
           <td><%=rsxx("rcvfinishdat")%>&nbsp;</td>
           </tr>           
        <% rsxx.MoveNext
           loop    
           rsxx.close
           set rsxx=nothing
      %>
   <%
           Set rsxx=Server.CreateObject("ADODB.Recordset")
           sqlfaqlist="SELECT RTLessorCmtyLinefaqHARDWARE.comq1,RTLessorCmtyLinefaqHARDWARE.lineq1,RTLessorCmtyLinefaqHARDWARE.faqno, " _
         &"RTLessorCmtyLinefaqHARDWARE.PRTNO, RTLessorCmtyLinefaqHARDWARE.seq,RTLessorCmtyLinefaqsndwork.SENDWORKDAT, " _
         &"RTProdH.PRODNC + '--' + RTProdD1.itemnc + '('+ RTProdD1.SPEC+')' as prodc, RTLessorCmtyLinefaqHARDWARE.QTY, RTLessorCmtyLinefaqHARDWARE.amt, " _
         &"HBwarehouse.WARENAME, RTLessorCmtyLinefaqHARDWARE.DROPDAT, RTLessorCmtyLinefaqHARDWARE.DROPREASON, RTObj.CUSNC,RTLessorCmtyLinefaqHARDWARE.BATCHNO,RTLessorCmtyLinefaqHARDWARE.TARDAT,RTLessorCmtyLinefaqHARDWARE.rcvprtno,RTLessorCmtyLinefaqHARDWARE.rcvfinishdat  " _
         &"FROM RTProdH RIGHT OUTER JOIN RTLessorCmtyLinefaqHARDWARE LEFT OUTER JOIN " _
         &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
         &"RTLessorCmtyLinefaqHARDWARE.DROPUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
         &"HBwarehouse ON RTLessorCmtyLinefaqHARDWARE.WAREHOUSE = HBwarehouse.WAREHOUSE LEFT OUTER " _
         &"JOIN RTProdD1 ON RTLessorCmtyLinefaqHARDWARE.PRODNO = RTProdD1.PRODNO AND " _
         &"RTLessorCmtyLinefaqHARDWARE.ITEMNO = RTProdD1.ITEMNO ON RTProdH.PRODNO = RTLessorCmtyLinefaqHARDWARE.PRODNO " _
         &"LEFT OUTER JOIN RTLessorCmtyLinefaqsndwork on RTLessorCmtyLinefaqHARDWARE.comq1=RTLessorCmtyLinefaqsndwork.comq1 and RTLessorCmtyLinefaqHARDWARE.lineq1=RTLessorCmtyLinefaqsndwork.lineq1 and RTLessorCmtyLinefaqHARDWARE.faqno=RTLessorCmtyLinefaqsndwork.faqno and RTLessorCmtyLinefaqHARDWARE.prtno=RTLessorCmtyLinefaqsndwork.prtno " _
         &"WHERE RTLessorCmtyLinefaqHARDWARE.comq1=" & aryparmkey(0) & " and RTLessorCmtyLinefaqHARDWARE.lineq1=" & aryparmkey(1) & " and RTLessorCmtyLinefaqHARDWARE.dropdat is null " 
           rsxx.open sqlfaqlist,conn
         '  response.write sqlfaqlist
           Randomize
           XRND=RND()
           do until rsxx.eof %>
           <tr class="dataListentry">
           <td><%=rsxx("prtno")%>&nbsp;</td>
           <td><A HREF="/webap/rtap/BASE/RTlessorCMTY/RTLessorcmtylinefaqhardwareD.asp?V=<%=XRND%>&accessMode=U&key=<%=RSXX("comq1")%>;<%=RSXX("lineq1")%>;<%=RSXX("faqno")%>;<%=RSXX("prtno")%>;<%=RSXX("seq")%>" TARGET="NEWWINDOW" ><%=RSXX("prtno")%>-<%=RSXX("seq")%></A></td>
           <td><%=rsxx("SENDWORKDAT")%>&nbsp;</td>           
           <td><%=rsxx("prodc")%>&nbsp;</td>
           <td><%=rsxx("qty")%>&nbsp;</td>
           <td><%=rsxx("amt")%>&nbsp;</td>
           <td><%=rsxx("WARENAME")%>&nbsp;</td>
           
           <td><%=rsxx("rcvprtno")%>&nbsp;</td>
           <td><%=rsxx("rcvfinishdat")%>&nbsp;</td>
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
     sqlfaqlist="SELECT COUNT(*) AS CNT FROM RTLessorCmtyLineFaqH WHERE comq1=" & DSPKEY(0) & " and lineq1=" & dspkey(1) & " AND canceldat IS NULL "
     rsxx.open sqlfaqlist,conn
     IF RSXX("CNT") > 0 THEN
        XXfaqFLAG="Y"
     ELSE
        XXfaqFLAG=""
     END IF
     RSXX.CLOSE
     SET RSXX=NOTHING
   %>
    <% if XXfaqFLAG = "Y" then %>
  <DIV ID="SRTAG12" onclick="SRTAG12" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="CENTER">主線客服單資料</td></tr></table></DIV>
    <DIV ID="SRTAB12" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr class="dataListsearch"><td colspan=15 align=center><A HREF="/webap/rtap/BASE/RTlessorCMTY/RTLessorcmtylinefaqk.asp?V=<%=XRND%>&accessMode=U&key=<%=dspkey(0)%>;<%=dspkey(1)%>;" TARGET="NEWWINDOW" ><font color=white>主線客服單資料明細</font></A></td></tr>
    <tr class="dataListHEAD" align=center><td>客服單號</td><td>來電日</td><td>類型</td><td>　摘　　　　　　　　　　要</td><td>派工日期</td><td>派工單號</td><td>派工結案</td><td>客服結案</td><td>結案員</td><td>處理<br>天數</td></tr>
    <%
           Set rsxx=Server.CreateObject("ADODB.Recordset")
           sqlfaqlist="SELECT  RTLessorCmtyLineFaqH.comq1,RTLessorCmtyLineFaqH.lineq1, RTLessorCmtyLineFaqH.FAQNO, RTLessorCmtyLineFaqH.RCVDAT, RTCode.CODENC as codenc1, " _
                      &"  LEFT(RTLessorCmtyLineFaqH.MEMO, 15) AS memo15,RTLessorCmtyLineFaqH.CONTACTTEL, RTLessorCmtyLineFaqH.MOBILE,  " _
                      &"  RTLessorCmtyLineFaqH.EMAIL,  " _
                      &"  RTLessorCmtyLineFaqH.SNDWORK, RTObj_4.CUSNC,  " _
                      &"  RTLessorCmtyLineFaqH.SNDPRTNO, RTLessorCmtyLineFaqH.PRTDAT, RTLessorCmtyLineFaqH.SNDCLOSEDAT,  " _
                      &"  RTLessorCmtyLineFaqH.CALLBACKDAT, RTObj_5.CUSNC AS cusnc1,  " _
                      &"  RTLessorCmtyLineFaqH.FINISHDAT, RTObj_6.CUSNC AS Expr2,  " _
                      &"  RTLessorCmtyLineFaqH.CANCELDAT, RTObj_1.CUSNC AS Expr3,  " _
                      &"   RTObj_2.CUSNC AS Expr4,  " _
                      &"  RTObj_3.CUSNC AS Expr5,case when RTLessorCmtyLineFaqH.finishdat is null then datediff(dd,RTLessorCmtyLineFaqH.rcvdat,getdate())+1 else datediff(dd,RTLessorCmtyLineFaqH.rcvdat,RTLessorCmtyLineFaqH.finishdat)+1 end as processdat " _
                &"  FROM  RTEmployee RTEmployee_5 INNER JOIN " _
                      &"  RTObj RTObj_5 ON RTEmployee_5.CUSID = RTObj_5.CUSID RIGHT OUTER JOIN " _
                      &"  RTLessorCmtyLineFaqH ON  " _
                      &"  RTEmployee_5.EMPLY = RTLessorCmtyLineFaqH.CALLBACKUSR LEFT OUTER JOIN " _
                      &"  RTEmployee RTEmployee_4 INNER JOIN " _
                      &"  RTObj RTObj_4 ON RTEmployee_4.CUSID = RTObj_4.CUSID ON  " _ 
                      &"  RTLessorCmtyLineFaqH.SNDUSR = RTEmployee_4.EMPLY LEFT OUTER JOIN " _
                      &"  RTEmployee RTEmployee_3 INNER JOIN " _
                      &"  RTObj RTObj_3 ON RTEmployee_3.CUSID = RTObj_3.CUSID ON  " _
                      &"  RTLessorCmtyLineFaqH.UUSR = RTEmployee_3.EMPLY LEFT OUTER JOIN " _
                      &"  RTEmployee RTEmployee_2 INNER JOIN " _
                      &"  RTObj RTObj_2 ON RTEmployee_2.CUSID = RTObj_2.CUSID ON  " _
                      &"  RTLessorCmtyLineFaqH.EUSR = RTEmployee_2.EMPLY LEFT OUTER JOIN " _
                      &"  RTEmployee RTEmployee_1 INNER JOIN " _
                      &"  RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON  " _
                      &"  RTLessorCmtyLineFaqH.CANCELUSR = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
                      &"  RTObj RTObj_6 INNER JOIN " _
                      &"  RTEmployee RTEmployee_6 ON RTObj_6.CUSID = RTEmployee_6.CUSID ON  " _
                      &"  RTLessorCmtyLineFaqH.FUSR = RTEmployee_6.EMPLY LEFT OUTER JOIN " _
                      &"  RTCode ON RTLessorCmtyLineFaqH.SERVICETYPE = RTCode.CODE AND  " _
                      &"  RTCode.KIND = 'N4'"  _
                      &"  where RTLessorCmtyLineFaqH.comq1=" & aryparmkey(0) & " and lineq1=" & aryparmkey(1) & "  and RTLessorCmtyLineFaqH.canceldat is null " 
           rsxx.open sqlfaqlist,conn
         '  response.write sqlfaqlist
           Randomize
           XRND=RND()
           do until rsxx.eof %>
           <tr class="dataListentry">
           <td><A HREF="/webap/rtap/BASE/RTlessorCMTY/RTLessorcmtylinefaqD.asp?V=<%=XRND%>&accessMode=U&key=<%=RSXX("comq1")%>;<%=RSXX("lineq1")%>;<%=RSXX("faqno")%>" TARGET="NEWWINDOW" ><%=RSXX("faqno")%></A></td>
           <td><%=rsxx("RCVDAT")%>&nbsp;</td>           
           <td><%=rsxx("codenc1")%>&nbsp;</td>
           <td><%=rsxx("memo15")%>&nbsp;</td>
           <td><%=rsxx("SNDWORK")%>&nbsp;</td>
           <td><%=rsxx("SNDPRTNO")%>&nbsp;</td>
           <td><%=rsxx("SNDCLOSEDAT")%>&nbsp;</td>
           <td><%=rsxx("FINISHDAT")%>&nbsp;</td>
           <td><%=rsxx("cusnc1")%>&nbsp;</td>
           <td><%=rsxx("processdat")%>&nbsp;</td>
           </tr>           
        <% rsxx.MoveNext
           loop    
           rsxx.close
           set rsxx=nothing
      %>
      </table>
  </div>   
 <%END IF %>    
  </table> 
  </DIV>    
<%
    conn.Close   
    set rs=Nothing   
    set conn=Nothing 
End Sub 
' --------------------------------------------------------------------------------------------  
%>
<!-- #include virtual="/Webap/include/RTGetUserRight.asp" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->