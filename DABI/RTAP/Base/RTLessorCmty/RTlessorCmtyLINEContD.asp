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
                   case ucase("/webap/rtap/base/rtlessorcmty/RTLessorCmtyLineContd.asp")
                     '  response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       if i <> 2 then rs.Fields(i).Value=dspKey(i)    
                       if i=2 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         rsc.open "select max(entryno) AS entryno from RTLessorCmtyLineCont where comq1=" & dspkey(0) & " and lineq1=" & dspkey(1),conn
                         if len(rsc("entryno")) > 0 then
                            dspkey(2)=rsc("entryno") + 1
                         else
                            dspkey(2)=1
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
                 case ucase("/webap/rtap/base/rtlessorcmty/RTLessorCmtyLineContd.asp")
                   ' response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
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
       if ucase(runpgm)=ucase("/webap/rtap/base/rtlessorcmty/RTLessorCmtyLineContd.asp") then
          rs.open "select max(entryno) AS entryno from RTLessorCmtyLineCont where comq1=" & dspkey(0) & " and lineq1=" & dspkey(1) ,conn
          if not rs.eof then
            dspkey(2)=rs("entryno")
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
  numberOfKey=3
  title="ET-City主線續約資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT COMQ1,LINEQ1,ENTRYNO,CONTAPPLYDAT,APPLYNAME,APPLYSOCIAL,APPLYCONTACTTEL,APPLYMOBILE,LOANCONTACTTEL,LOANMOBILE,LINERATE, " _
             &"LINEIPTYPE,LINEISP,LINEDUEDAT,LINETEL,IPCNT,LINEIP,GATEWAY,SUBNET,DNSIP,PPPOEACCOUNT," _
             &"PPPOEPASSWORD,HINETNOTIFYDAT,ADSLAPPLYDAT,CLOSEDAT,CLOSEUSR,CANCELDAT,CANCELUSR,MEMO,EUSR,EDAT," _
             &"UUSR, UDAT,LOANNAME,LOANSOCIAL " _
             &"FROM RTLessorCmtyLineCont WHERE COMQ1=0 "
  sqlList="SELECT COMQ1,LINEQ1,ENTRYNO,CONTAPPLYDAT,APPLYNAME,APPLYSOCIAL,APPLYCONTACTTEL,APPLYMOBILE,LOANCONTACTTEL,LOANMOBILE,LINERATE,  " _
             &"LINEIPTYPE,LINEISP,LINEDUEDAT,LINETEL,IPCNT,LINEIP,GATEWAY,SUBNET,DNSIP,PPPOEACCOUNT," _
             &"PPPOEPASSWORD,HINETNOTIFYDAT,ADSLAPPLYDAT,CLOSEDAT,CLOSEUSR,CANCELDAT,CANCELUSR,MEMO,EUSR,EDAT," _
             &"UUSR, UDAT,LOANNAME,LOANSOCIAL  " _
         &"FROM RTLessorCmtyLineCont WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    
    if len(trim(DSPKEY(2))) = 0 THEN DSPKEY(2)=0
    If len(trim(dspkey(3)))=0 or Not Isdate(dspkey(3)) then
       formValid=False
       message="續約申請日不可空白或格式錯誤"    
   elseif len(trim(dspkey(10)))=0 then
       formValid=False
       message="主線速率不可空白"    
    elseif (len(trim(dspkey(33))) <> 0 and len(trim(dspkey(34))) = 0 ) or   (len(trim(dspkey(33))) = 0 and len(trim(dspkey(34))) <> 0 ) then
       formValid=False
       message="藉名裝機時，用戶名稱及身份證號必須同時存在"    
    elseif len(trim(DSPKEY(34)))  <> 0 AND  len(trim(DSPKEY(34)))  <> 10 AND  len(trim(DSPKEY(34)))  <> 8 THEN
       formValid=False
       message="身份證(統編)字號長度不足(必須為10碼或8碼)"                             
    elseif  len(trim(dspkey(12)))=0 THEN
       formValid=False
       message="線路ISP不可空白"                   
    elseif  len(trim(dspkey(11)))=0 THEN
       formValid=False
       message="線路IP種類不可空白"     
    elseif  len(trim(dspkey(10)))=0 THEN
       formValid=False
       message="線路速率不可空白"                
    elseif  len(trim(dspkey(15)))=0 THEN
       formValid=False
       message="線路IP數量不可空白"                       
 '   elseif len(trim(DSPKEY(48))) > 0 and len(trim(dspkey(14)))=0 THEN
 '      formValid=False
 '      message="主線(CHT通知測通日)輸入時，主線[附掛電話]不可空白"                   
 '   elseif len(trim(DSPKEY(22))) > 0 and len(trim(dspkey(16)))=0 THEN
 '      formValid=False
 '      message="主線(CHT通知測通日)輸入時，(主線網路IP)不可空白"     
 '   elseif len(trim(DSPKEY(22))) > 0 and len(trim(dspkey(18)))=0 THEN
 '      formValid=False
 '      message="主線(CHT通知測通日)輸入時，(主線網路subnet)不可空白"         
 '   elseif len(trim(DSPKEY(22))) > 0 and len(trim(dspkey(17)))=0 THEN
 '      formValid=False
 '      message="主線(CHT通知測通日)輸入時，(主線網路Gateway IP)不可空白"     
 '   elseif len(trim(DSPKEY(22))) > 0 and len(trim(dspkey(19)))=0 THEN
 '      formValid=False
 '      message="主線(CHT通知測通日)輸入時，(主線網路DNS IP)不可空白"     
    elseif len(trim(DSPKEY(23))) > 0 and dspkey(11)="02" AND ( len(trim(DSPKEY(20)))=0 OR len(trim(DSPKEY(21)))=0 ) THEN
       formValid=False
       message="主線線路種類為PPPOE時，PPPOE帳號及密碼不可空白"            
    end if
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(31)=V(0)
        dspkey(32)=datevalue(now())
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
          document.all("KEY33").VALUE="元訊寬頻網路股份有限公司"
          document.all("KEY34").VALUE="70770184"
          document.all("OPT2").checked=false
          document.all("KEY33").CLASSNAME="dataListDATA"
          document.all("KEY33").READONLY=TRUE
          document.all("KEY34").CLASSNAME="dataListDATA"
          document.all("KEY34").READONLY=TRUE
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
          document.all("KEY33").VALUE=""
          document.all("KEY34").VALUE=""
          document.all("OPT1").checked=false
          document.all("KEY33").CLASSNAME="dataListENTRY"
          document.all("KEY33").READONLY=FALSE
          document.all("KEY34").CLASSNAME="dataListENTRY"
          document.all("KEY34").READONLY=FALSE
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
       <tr><td width="15%" class=dataListHead>社區序號</td>
       <td width="15%"  bgcolor="silver">
           <input type="text" name="key0"
                 <%=fieldRole(1)%> readonly size="10" value="<%=dspKey(0)%>" maxlength="8" class=dataListdata></td>
       <td width="15%" class=dataListHead>主線序號</td>
       <td width="15%"  bgcolor="silver">
           <input type="text" name="key1"
                 <%=fieldRole(1)%> readonly size="10" value="<%=dspKey(1)%>" maxlength="8" class=dataListdata></td> 
        <td width="15%" class=dataListHead>續約序號</td>
        <td width="15%"  bgcolor="silver">
           <input type="text" name="key2"
                 <%=fieldRole(1)%> readonly size="3" value="<%=dspKey(2)%>" maxlength="3" class=dataListdata></td> </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(29))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(29)=V(0)
        End if  
       dspkey(30)=datevalue(now())
    else
        if len(trim(dspkey(31))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(31)=V(0)
        End if         
        dspkey(32)=datevalue(now())
    end if      
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
    '主線測通後(DSPKEY62),基本資料 protect
     If len(trim(dspKey(24))) > 0  Then
       fieldPa=" class=""dataListData"" readonly "
       FIELDPC=" DISABLED "
    Else
       fieldPa=""
       FIELDPC=""
    End If
    If len(trim(dspKey(24))) > 0 Then
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
  <span id="tags1" class="dataListTagsOn">ET-City主線續約資訊</span>
                                                            
  <div class=dataListTagOn> 
<table width="100%">
<%
Set rsxx=Server.CreateObject("ADODB.Recordset")
sqlxx="select * from rtlessorcmtyline where comq1=" & dspkey(0) & " and lineq1=" & dspkey(1)
rsxx.open sqlxx,conn
%>
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td><td>     
    <DIV ID="SRTAG0" onclick="srtag0" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="CENTER">基本資料</td></tr></table></div>
 <DIV ID=SRTAB0 >   
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">

<tr><td width="15%" class=dataListHEAD>申請人姓名</td>
    <td width="35%" bgcolor="silver" >
    <% if len(trim(dspkey(4)))=0 then dspkey(4)=rsxx("applyname") %>
        <input type="text" name="key4" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="30"
               value="<%=dspKey(4)%>" size="30" class=dataListEntry>
        </td>
    <td width="15%" class=dataListHEAD>申請人身份證(統編)</td>
    <td width="35%" bgcolor="silver" >
    <% if len(trim(dspkey(5)))=0 then dspkey(5)=rsxx("applysocial") %>
    <input type="text" name="key5" size="10" MAXLENGTH=10 value="<%=dspKey(5)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text46">
    </td>        
</tr>
<tr><td width="15%" class=dataListHEAD>申請人連絡電話</td>
    <td width="35%" bgcolor="silver" >
    <% if len(trim(dspkey(6)))=0 then dspkey(6)=rsxx("APPLYCONTACTTEL") %>
        <input type="text" name="key6" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="15"
               value="<%=dspKey(6)%>"  size="15" class=dataListEntry>
        </td>
    <td width="15%" class=dataListHEAD>申請人行動電話</td>
    <td width="35%" bgcolor="silver" >
    <% if len(trim(dspkey(7)))=0 then dspkey(7)=rsxx("APPLYMOBILE") %>
    <input type="text" name="key7" size="10" MAXLENGTH=10 value="<%=dspKey(7)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text46">
    </td>        
</tr>
<tr>
        <td  width="15%" class="dataListHEAD" height="23">建檔人員</td>                                 
        <td  width="35%" height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(29) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(29) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>    <input type="text" name="key29" size="6" READONLY value="<%=dspKey(29)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  width="15%" class="dataListHEAD" height="23">建檔日期</td>                                 
        <td  width="35%" height="23" bgcolor="silver">
        <input type="text" name="key30" size="10" READONLY value="<%=dspKey(30)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>  
<tr>
        <td  class="dataListHEAD" height="23">修改人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(31) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(31) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>    <input type="text" name="key31" size="6" READONLY value="<%=dspKey(31)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">修改日期</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key32" size="10" READONLY value="<%=dspKey(32)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
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
     <% '當主線未結案前才可變更借名裝機資料
     IF LEN(TRIM(DSPKEY(24))) > 0 THEN
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
         <% if len(trim(dspkey(33)))=0 then dspkey(33)=rsxx("LOANNAME") %>
        <input type="text" name="key33" size="30"  maxlength="10" value="<%=dspKey(33)%>"  <%=fieldpA%><%=fieldRole(1)%>  readonly class="dataListDATA" ID="Text69"></td>        
      <td  WIDTH="15%" class="dataListSEARCH" height="23">借名用戶身份證號</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
          <% if len(trim(dspkey(34)))=0 then dspkey(34)=rsxx("LOANSOCIAL") %>
        <input type="text" name="key34" size="20"  maxlength="10" value="<%=dspKey(34)%>"  <%=fieldpA%><%=fieldRole(1)%>  readonly class="dataListDATA" ID="Text70"></td>        
   </tr>
<tr><td width="15%" class=dataListHEAD>借名用戶連絡電話</td>
    <td width="35%" bgcolor="silver" >
     <% if len(trim(dspkey(8)))=0 then dspkey(8)=rsxx("LOANCONTACTTEL") %>
        <input type="text" name="key8" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="15"
               value="<%=dspKey(8)%>"  size="15" class=dataListEntry>
        </td>
    <td width="15%" class=dataListHEAD>借名用戶行動電話</td>
    <td width="35%" bgcolor="silver" >
       <% if len(trim(dspkey(9)))=0 then dspkey(9)=rsxx("LOANMOBILE") %>
    <input type="text" name="key9" size="10" MAXLENGTH=10 value="<%=dspKey(9)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text46">
    </td>        
</tr>   
   </table>
   </div></div>
  
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
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(23))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='C3' " 
       If len(trim(dspkey(12))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & ">(線路ISP)</option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & ">(線路ISP)</option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='C3' AND CODE='" & dspkey(12) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(12) Then sx=" selected "
       if sx="" and rs("CODE")=rsxx("LINEISP") then
          sx=" selected "
       end if
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key12" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35">                                                                  
        <%=s%>
   </select>
        </td>
        <td  WIDTH="15%" class="dataListHEAD" height="23">線路IP種類</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(23))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M5' " 
       If len(trim(dspkey(11))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & ">(線路IP種類)</option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & ">(線路IP種類)</option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M5' AND CODE='" & dspkey(11) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(11) Then sx=" selected "
       if sx="" and rs("CODE")=rsxx("LINEIPTYPE") then
          sx=" selected "
       end if
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key11" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35">                                                                  
        <%=s%>
   </select></td>                                 
                              
 </tr>         
<tr>   <td  WIDTH="15%" class="dataListHEAD" height="23">主線速率</td>               
        <td  WIDTH="35%" height="23" bgcolor="silver" >
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(23))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D3' " 
       If len(trim(dspkey(10))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & ">(主線速率)</option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & ">(主線速率)</option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D3' AND CODE='" & dspkey(10) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(10) Then sx=" selected "
       if sx="" and rs("CODE")=rsxx("LINErate") then
          sx=" selected "
       end if
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key10" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35">                                                                  
        <%=s%>
   </select>
        </td>
        <td  WIDTH="15%" class="dataListHEAD" height="23">附掛電話</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
         <% if len(trim(dspkey(14)))=0 then dspkey(14)=rsxx("LINETEL") %>
        <input type="text" name="key14" size="15" maxlength="15" value="<%=dspKey(14)%>"  <%=fieldpA%> <%=fieldpB%><%=fieldRole(1)%> class="dataListEntry" ID="Text43"></td>                                 
                              
 </tr>       
<tr>                                 
        <td  WIDTH="15%" class="dataListHEAD" height="23">主線網路IP</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
         <% if len(trim(dspkey(16)))=0 then dspkey(16)=rsxx("LINEip") %>
        <input type="text" name="key16" size="20"  maxlength="20" value="<%=dspKey(16)%>"  <%=fieldpA%><%=fieldRole(1)%> class="dataListEntry" ID="Text37"></td>        
        <td  WIDTH="15%" class="dataListHEAD" height="23">IP數量</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
          <% if len(trim(dspkey(15)))=0 then dspkey(15)=rsxx("IPCNT") %>
        <input type="text" name="key15" size="5"  maxlength="5" value="<%=dspKey(15)%>"  <%=fieldpA%><%=fieldRole(1)%> class="dataListEntry" ID="Text37"></td>        
 
 </tr>        
<tr>     <td  WIDTH="15%" class="dataListHEAD" height="23">主線SUBNET</td>                                 
   <% aryOption=Array("","255.255.255.0","255.255.255.128")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1  AND FIELDPB = "" Then 
      For i = 0 To Ubound(aryOption)
          If dspKey(18)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          if sx="" and aryOption(i)=rsxx("SUBNET") then
             sx=" selected "
          end if
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(18) &""">" &dspKey(18) &"</option>"
   End If%>                                 
        <td width="35%" height="23" bgcolor="silver">
        <select size="1" name="key18" <%=fieldpA%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select14">                                 
        <%=s%>
        </select></td>                                
        <td  class="dataListHEAD" height="23">閘道IP(Gateway)</td>                                 
        <td  height="23" bgcolor="silver">
        <% if len(trim(dspkey(17)))=0 then dspkey(17)=rsxx("GATEWAY") %>
        <input type="text" name="key17" size="20"   maxlength="20" value="<%=dspKey(17)%>"  <%=fieldpA%><%=fieldRole(1)%> class="dataListEntry" ID="Text38"></td>        
 <tr>        
        <td  class="dataListHEAD" height="23">DNS IP</td>                                 
        <td  height="23" bgcolor="silver" COLSPAN=3>
         <% if len(trim(dspkey(19)))=0 then dspkey(19)=rsxx("DNSIP") %>
        <input type="text" name="key19" size="20" maxlength="20" value="<%=dspKey(19)%>"  <%=fieldpA%><%=fieldRole(1)%> class="dataListEntry" ID="Text40"></td>                                 
 </tr>     
 <tr>                                 
        <td  class="dataListHEAD" height="23">PPPOE撥接帳號</td>                                 
        <td  height="23" bgcolor="silver">
         <% if len(trim(dspkey(20)))=0 then dspkey(20)=rsxx("PPPOEACCOUNT") %>
        <input type="text" name="key20" size="10" maxlength="10" value="<%=dspKey(20)%>"  <%=fieldpA%><%=fieldRole(1)%> class="dataListEntry" ID="Text41"></td>        
        <td  class="dataListHEAD" height="23">PPPOE撥接密碼</td>                                 
        <td  height="23" bgcolor="silver">
        <% if len(trim(dspkey(21)))=0 then dspkey(21)=rsxx("PPPOEPASSWORD") %>
        <input type="text" name="key21" size="10" maxlength="10" value="<%=dspKey(21)%>"  <%=fieldpA%><%=fieldRole(1)%> class="dataListEntry" ID="Text42"></td>                                 
 </tr>    
  

  </table>   
  </DIV>
      <DIV ID="SRTAG4" onclick="srtag4" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr><td bgcolor="BDB76B" align="CENTER">主線申請及施工進度狀態</td></tr></table></DIV>
    <DIV ID=SRTAB4 >  
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
     <tr><td width="15%" class=dataListHEAD>續約申請日</td>
    <td width="35%" bgcolor="silver" >
        <input type="text" name="key3" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(3)%>"  READONLY size="10" class=dataListEntry>
       <input  type="button" id="B3"  <%=FIELDPC%>  <%=FIELDPF%>  name="B3" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
       <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPC%>  <%=FIELDPF%>  alt="清除" id="C3"  name="C3"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
          </td>
        <td   class="dataListHEAD" height="23">CHT通知測通日</td>                                 
        <td   height="23" bgcolor="silver">
        <input type="text" name="key22" size="10"   READONLY value="<%=dspKey(22)%>"  <%=fieldpA%><%=fieldRole(1)%> class="dataListentry" ID="Text55">     
        <input type="button" id="B22"  <%=FIELDPC%>    name="B22" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPC%>   alt="清除" id="C22"  name="C22"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"> </td>          
       </tr>             
      <tr>      
      <td   class="dataListHEAD" height="23">主線測通日</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key23" size="10"   READONLY value="<%=dspKey(23)%>"  <%=fieldpA%><%=fieldRole(1)%> class="dataListENTRY" ID="Text56">
       <input type="button" id="B23"  <%=FIELDPC%>   name="B23" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPC%> alt="清除" id="C23"  name="C23"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"> </td>          
         <td   class="dataListSEARCH" height="23">線路到期日</td>                                 
        <td   height="23" bgcolor="silver" >
        <input type="text" name="key13" size="10"   READONLY value="<%=dspKey(13)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListentry" ID="Text55">     
        <input type="button" id="B13"  <%=FIELDPD%>   name="B13" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPD%> alt="清除" id="C13"  name="C13"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"> </td>          
       </tr>             
      <tr>       
       <td  class="dataListHEAD" height="23">結案日</td>                                 
        <td   height="23" bgcolor="silver">
                <input type="text" name="key24" size="10" value="<%=dspKey(24)%>"  <%=fieldpb%><%=fieldRole(1)%> readonly class="dataListDATA" ID="Text71">     
      </TD>
      <td  class="dataListHEAD" height="23">結案人員</td>                                 
        <td   height="23" bgcolor="silver">
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
                <input type="text" name="key25" size="6" value="<%=dspKey(25)%>"  <%=fieldpb%><%=fieldRole(1)%> readonly class="dataListDATA" ID="Text71"> <font size=2><%=name%></FONT>    
      </TD>
       </tr>             
      <tr>
        <td  class="dataListHEAD" height="23">作廢日</td>                                 
        <td   height="23" bgcolor="silver" >
        <input type="text" name="key26" size="10" value="<%=dspKey(26)%>"  <%=fieldpb%><%=fieldRole(1)%> readonly class="dataListDATA" ID="Text72">     </td>
       <td  class="dataListHEAD" height="23">作廢人員</td>                                 
        <td   height="23" bgcolor="silver" >
        <%  name="" 
           if dspkey(27) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(27) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>         
        <input type="text" name="key27" size="6" value="<%=dspKey(27)%>"  <%=fieldpb%><%=fieldRole(1)%> readonly class="dataListDATA" ID="Text71"><font size=2><%=name%></FONT>
      </tr>                         
  </table> 
  </DIV>
    <DIV ID="SRTAG5" onclick="srtag5" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="CENTER">備註說明</td></tr></table></DIV>
   <DIV ID="SRTAB5" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
    <TR><TD align="CENTER">
     <TEXTAREA  cols="100%"  name="key28" rows=8  MAXLENGTH=500  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(28)%>" ID="Textarea1"><%=dspkey(28)%></TEXTAREA>
   </td></tr>
 </table> 
  </DIV>    
<tr>                                   
  </div> 
<%  rsxx.close
    conn.Close   
    set rs=Nothing   
    set rsxx=Nothing 
    set conn=Nothing 
End Sub 
' --------------------------------------------------------------------------------------------  
%>
<!-- #include virtual="/Webap/include/RTGetUserRight.asp" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->