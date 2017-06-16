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
                   case ucase("/webap/rtap/base/rtsparq0809/rtsparq0809CUSTd.asp")
                       'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       if i <> 0 then rs.Fields(i).Value=dspKey(i)    
                       if i=0 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         cusidxx="S" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
                         rsc.open "select max(cusid) AS cusid from rtsparq0809cust where cusid like '" & cusidxx & "%' " ,conn
                         if len(rsc("cusid")) > 0 then
                            dspkey(0)=cusidxx & right("000" & cstr(cint(right(rsc("cusid"),3)) + 1),3)
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
                 case ucase("/webap/rtap/base/rtsparq0809/rtsparq0809CUSTd.asp")
                   'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                     if i<>0  then rs.Fields(i).Value=dspKey(i)         
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
       if ucase(runpgm)=ucase("/webap/rtap/base/rtsparq0809/rtsparq0809CUSTd.asp") then
          cusidxx="S" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
          rsc.open "select max(cusid) AS cusid from rtsparq0809cust where cusid like '" & cusidxx & "%' " ,conn
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
<input type="text" name="sw" value="<%=sw%>" style="display:none;" ID="Text17" size="20">
<input type="text" name="reNew" value="N" style="display:none;" ID="Text18" size="20">
<input type="text" name="rwCnt" value="<%=rwCnt%>" style="display:none;" ID="Text19" size="20">
<input type="text" name="accessMode" value="<%=accessMode%>" style="display:none;" ID="Text20" size="20">
<table width="100%" ID="Table1">
  <tr class=dataListTitle><td width="20%">　</td><td width="60%" align=center>
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
  numberOfKey=1
  title="[有話好說]輕鬆省專案用戶資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT CUSID, CUSNC, SOCIALID, BIRTHDAY, MOBILE, FAX1, FAX12, EMAIL, APPLYDAT, CANCELDAT, DROPDAT, " _
             &"CUTID1, TOWNSHIP1, RADDR1 ,RZONE1, CUTID2, TOWNSHIP2,RADDR2,RZONE2,EUSR,EDAT,UUSR,UDAT,MEMO, " _
             &"SECONDIDTYPE,SECONDNO,CONSIGNEE,AREAID,SALESID,SETUPEMPLOYEE,SECONDLINEDEVP, ITEMNO, SENDDAT, REALRCVAMT, " _
             &"ATTCONSENT, ATTIDCOPY, ATTHEALTHINS, ATTETC, FIRSTIDTYPE, PHONENUM, BOSS, SVITEM,nciccusno " _
             &"from RTSparq0809Cust WHERE CUSID='' "
  sqlList="SELECT CUSID, CUSNC, SOCIALID, BIRTHDAY, MOBILE, FAX1, FAX12, EMAIL, APPLYDAT, CANCELDAT, DROPDAT, " _
             &"CUTID1, TOWNSHIP1, RADDR1, RZONE1, CUTID2, TOWNSHIP2,RADDR2,RZONE2,EUSR,EDAT,UUSR,UDAT,MEMO, " _
			 &"SECONDIDTYPE,SECONDNO,CONSIGNEE,AREAID,SALESID,SETUPEMPLOYEE,SECONDLINEDEVP, ITEMNO, SENDDAT, REALRCVAMT, " _
             &"ATTCONSENT, ATTIDCOPY, ATTHEALTHINS, ATTETC, FIRSTIDTYPE, PHONENUM, BOSS, SVITEM,nciccusno " _
             &"from RTSparq0809Cust WHERE " 
  userDefineRead="Yes"      
  userDefineSave="Yes"       
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
  IF LEN(TRIM(DSPKEY(33))) = 0 THEN DSPKEY(33)=0
  IF LEN(TRIM(DSPKEY(34))) = 0 THEN DSPKEY(34)=""
  IF LEN(TRIM(DSPKEY(35))) = 0 THEN DSPKEY(35)=""
  IF LEN(TRIM(DSPKEY(36))) = 0 THEN DSPKEY(36)=""
  IF LEN(TRIM(DSPKEY(39))) = 0 THEN DSPKEY(39)=0

  If len(trim(dspkey(1)))=0 then
       formValid=False
       message="用戶名稱不可空白"    
  elseif len(trim(dspkey(2)))=0 then
       formValid=False
       message="申請用戶身分證號不可空白"   
  elseif len(trim(dspkey(3)))=0 then
       formValid=False
       message="申請用戶出生日期不可空白"     
  ELSEIf NOT ISDATE(dspkey(3)) then
       formValid=False
       message="出生日期格式錯誤，請用(西元年/月/日)"                   
  elseif len(trim(dspkey(4))) > 0 and len(trim(dspkey(4))) < 10 then
       formValid=False
       message="行動電話號碼長度必須10位"      
  elseif len(trim(dspkey(5))) > 0 and len(trim(dspkey(5))) < 2 then
       formValid=False
       message="用戶傳真電話區碼不可少於2位"      
  elseif len(trim(dspkey(6))) > 0 and len(trim(dspkey(6))) < 6 then
       formValid=False
       message="用戶傳真電話不可少於6位"     
  elseif len(trim(dspkey(7))) > 0 and len(trim(dspkey(7))) < 10 then
       formValid=False
       message="Email不可少於10位"                    
  elseif len(trim(dspkey(11)))=0 then
       formValid=False
       message="申請用戶戶籍地址(縣市)不可空白"               
  elseif dspkey(11)<>"06" and dspkey(11)<>"15" and len(trim(dspkey(12)))=0 then
       formValid=False
       message="申請用戶戶籍地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(13)))=0 then
       formValid=False
       message="申請用戶戶籍地址不可空白"          
  elseif len(trim(dspkey(15)))=0 then
       formValid=False
       message="申請用戶帳寄地址(縣市)不可空白"               
  elseif dspkey(15)<>"06" and dspkey(15)<>"15" and len(trim(dspkey(16)))=0 then
       formValid=False
       message="申請用戶帳寄地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(17)))=0 then
       formValid=False
       message="申請用戶帳寄地址不可空白"          
  ELSEIf len(trim(dspkey(32)))=0 then
       formValid=False
       message="送件日期不可空白"                
  ELSEIf len(trim(dspkey(24)))=0 then
       formValid=False
       message="第二證照類別不可空白"          
  ELSEIf len(trim(dspkey(25)))=0 then
       formValid=False
       message="第二證照號碼不可空白"                          
  ELSEIf len(trim(dspkey(26)))=0 AND  len(trim(dspkey(28)))=0  AND len(trim(dspkey(30)))=0then
       formValid=False
       message="開發人員不可空白"                 
  ELSEIf (len(trim(dspkey(26)))<>0 AND  len(trim(dspkey(28)))<>0)  or (len(trim(dspkey(26)))<>0 AND len(trim(dspkey(30)))<>0 ) or (len(trim(dspkey(28)))<>0 and len(trim(dspkey(30)))<>0 )then
       formValid=False
       message="(經銷、業務、二線)開發人員不可同時輸入"             
  end if
  
  IF formValid=TRUE THEN
    IF dspkey(2) <> "" and len(dspkey(38))=0 then
       idno=dspkey(2)
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
                DSpkey(21)=V(0)
        dspkey(22)=datevalue(now())
    end if        
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveXScript()%>
   <SCRIPT Language="VBScript">
   Sub SrDeveloperonclick()
       prog="RTGetDeveloperD.asp"
       prog=prog & "?KEY=" & document.all("KEY30").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key30").value =  trim(Fusrid(0))
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

Sub SrAddrEqual1()
     document.All("key15").value=document.All("key11").value
     document.All("key16").value=document.All("key12").value
     document.All("key17").value=document.All("key13").value
     document.All("key18").value=document.All("key14").value
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
Sub SrGetUserDefineKey()
	%>
      <table width="60%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="10%" class=dataListHead>用戶代號</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key0"
                 <%=fieldRole(1)%> readonly size="15" value="<%=dspKey(0)%>" maxlength="15" class=dataListdata></td>
         <td width="10%" class=dataListHead>NCIC用戶編號</td>
            <td width="10%"  bgcolor="silver">
           <input type="text" name="key42"
                 <%=fieldRole(1)%> size="15" value="<%=dspKey(42)%>" maxlength="15" class=dataListentry></td>

  </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(19))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(19)=V(0)
        End if  
       dspkey(20)=datevalue(now())
    else
        if len(trim(dspkey(21))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(21)=V(0)
        End if         
        dspkey(22)=datevalue(now())
    end if      
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
    '用戶送件申請後,基本資料 protect
  '  If len(trim(dspKey(32))) > 0 OR len(trim(dspKey(38))) > 0 OR len(trim(dspKey(39))) > 0 OR len(trim(dspKey(40))) > 0 Then
  '     fieldPa=" class=""dataListData"" readonly "
  '     fieldpb=" disabled "
  '  Else
  '     fieldPa=""
  '     fieldpb=""
  '  End If
      
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
  <span id="tags1" class="dataListTagsOn">0809用戶資訊</span>
                                                            
  <div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">　</td><td width="96%">　</td><td width="2%">　</td></tr>
<tr><td>　</td>
<td>     
    <DIV ID="SRTAG0" onclick="srtag0" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="LEFT">基本資料</td></tr></table></div>
 <DIV ID=SRTAB0 >   
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
<tr><td WIDTH=15% class=dataListHEAD>用戶名稱</td>
    <td WIDTH=20% bgcolor="silver" >
        <input type="text" name="key1" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="50"
               value="<%=dspKey(1)%>"  size="20" class=dataListENTRY ID="Text3"></td>
    <td WIDTH=15% class=dataListHEAD>出生日</td>
    <td WIDTH=20% bgcolor="silver" >
        <input type="text" name="key3" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(3)%>"   size="10" class=dataListENTRY ID="Text33">
        <input  type="button" id="B3" name="B3" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
       <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C3"  name="C3"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>
       
	<td class=dataListHEAD>企業負責人</td>
    <td bgcolor="silver" >
        <input type="text" name="key40" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="20"
               value="<%=dspKey(40)%>"   size="30" class=dataListEntry ID="Text48"></td>      

</tr>
<tr>
<td  class=dataListHEAD>行動電話</td>
    <td  bgcolor="silver" >
        <input type="text" name="key4" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(4)%>"   size="10" class=dataListEntry ID="Text48"></td>      
    <td  class=dataListSEARCH>傳真電話</td>       
    <td  bgcolor="silver">
        <font size=2>(</font><input type="text" name="key5" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="3"
               value="<%=dspKey(5)%>"  size="3" class=dataListEntry ID="Text1"><font size=2>)</font>
               <input type="text" name="key6" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="8"
               value="<%=dspKey(6)%>"   size="10" class=dataListEntry ID="Text30">
      </td>         
    <td  class=dataListSEARCH>Email</td>       
    <td  bgcolor="silver">
        <input type="text" name="key7" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="50"
               value="<%=dspKey(7)%>"   size="50" class=dataListEntry ID="Text28">
      </td>               
      </tr>      

    <%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='L3' " 
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='L3' AND CODE='" & dspkey(38) &"' " 
       'SXX60=""
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(身份證字號)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(38) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>        
    <tr><td  class="dataListHead" height="25">第一證照別及號碼</td>
    <td  height="25" bgcolor="silver" colspan=7> 
		<select size="1" name="key38"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select5"><%=s%></select>	
        <input type="text" name="key2" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(2)%>"   size="12" class=dataListENTRY ID="Text31"></td></tr>
    <tr>
    <%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='L3' " 
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='L3' AND CODE='" & dspkey(24) &"' " 
       'SXX60=""
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(第二證照別)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(24) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>        
        <td  class="dataListHead" height="25">第二證照別及號碼</td>
        <td  height="25" bgcolor="silver" colspan=7> 
		<select size="1" name="key24"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select16"><%=s%></select>	
        <input type="text" name="key25" size="15" maxlength="12" value="<%=dspkey(25)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text49"></td> 
     </tr> 

<tr>
        <td class="dataListHead" height="25">申請書附件</td>
        <td  height="25" bgcolor="silver" colspan=7>
			<%   IF DSPKEY(34)="Y" THEN CHECK34=" CHECKED "%>
			<input type="checkbox" name="key34" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" 
               value="Y"  <%=CHECK34%> READONLY  bgcolor="silver"  ID="Checkbox1"><font size=2>同意書　</font>
			<%   IF DSPKEY(35)="Y" THEN CHECK35=" CHECKED "%>
			<input type="checkbox" name="key35" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" 
               value="Y"  <%=CHECK35%> READONLY  bgcolor="silver"  ID="Checkbox2"><font size=2>ID影本　</font>
			<%   IF DSPKEY(36)="Y" THEN CHECK36=" CHECKED "%>
			<input type="checkbox" name="key36" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" 
               value="Y"  <%=CHECK36%> READONLY  bgcolor="silver"  ID="Checkbox3"><font size=2>健保卡影本　　其他</font>
			
			<input type="text" name="key37" size="30" maxlength="30" value="<%=dspkey(37)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text8"></td> 
</td></tr> 
     
        
<tr><td class=dataListHEAD>戶籍地址</td>
    <td bgcolor="silver" COLSPAN=5>
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
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
         <select size="1" name="key11" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select2"><%=s%></select>
        <input type="text" name="key12" readonly  size="8" value="<%=dspkey(12)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text4"><font SIZE=2>(鄉鎮)                 
         <input type="button" id="B12" <%=fieldpb%> name="B12"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX12%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C12"  name="C12"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
        
        <input type="text" name="key13" <%=fieldpa%> size="60" value="<%=dspkey(13)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text5"><font size=2>
         <input type="text" name="key14"  readonly size="5" value="<%=dspkey(14)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text12">
        </td>                                 
</tr>  
<tr><td class=dataListHEAD>帳寄地址
    <br><input type="radio" name="rd1"  <%=fieldpb%> onClick="SrAddrEqual1()" ID="Radio1" VALUE="Radio1">同戶籍</td>
    <td bgcolor="silver" COLSPAN=5>
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
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
         <select size="1" name="key15" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select1"><%=s%></select>
        <input type="text" name="key16" readonly  size="8" value="<%=dspkey(16)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text14"><font SIZE=2>(鄉鎮)                 
         <input type="button" id="B16"  <%=fieldpb%>  name="B16"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX16%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%>  alt="清除" id="C16"  name="C16"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
        
        <input type="text" name="key17" <%=fieldpa%> size="60" value="<%=dspkey(17)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text16"><font size=2>
        <input type="text" name="key18"  readonly size="5" value="<%=dspkey(18)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text25">
        </td>                                 
</tr>  


    <tr>
    <%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="select code, codenc from RTCode where kind ='O2' " 
    Else
       sql="select code, codenc from RTCode where kind ='O2' AND code='" & dspkey(31) &"' " 
       'SXX60=""
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(用戶設備)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("code")=dspkey(31) Then sx=" selected "
       s=s &"<option value=""" &rs("code") &"""" &sx &">" &rs("codenc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>        
        <td  class="dataListHead" height="25">用戶設備</td>
        <td  height="25" bgcolor="silver" colspan=2> 
		<select size="1" name="key31"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><%=s%></select></td> 


		<% aryOption=Array("","ISR-Like D","ISR-Like D4")
   		s=""
   		If Len(Trim(fieldRole(1) &dataProtect)) < 1 Then
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
		<td class="dataListHead" height="32">申請服務項目</td>
        <td  height="32" bgcolor="silver" colspan=3><select size="1" name="key41" <%=fieldpg%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry">                                            
        <%=s%></select></td>                                     

     </tr>

</table> </div>
    <DIV ID="SRTAG1" onclick="srtag1" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table10">
    <tr><td bgcolor="BDB76B" align="LEFT">申請服務明細</td></tr></table></div>
     <DIV ID="SRTAB1" >
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table11">
    <tr><td WIDTH=5% bgcolor="silver" COLSPAN=6>
    <input  type="button" id="STL" name="STL"  height="100%" width="100%" style="Z-INDEX: 1" value="顯示電話明細" onclick="SrSHOWTELLISTOnClick">
    </td></tr>
    <tr id="SRTAR1" STYLE="">
    <td colspan=6><table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table4">
    <tr>
            <td WIDTH=10% ALIGN="center" class=dataListDATA>項次</td>
            <td WIDTH=15% ALIGN="center" class=dataListDATA >區域號碼</td>
            <td WIDTH=15% ALIGN="center" class=dataListDATA>電話號碼</td>
            <td WIDTH=10% ALIGN="center" class=dataListDATA>項次</td>
            <td WIDTH=15% ALIGN="center" class=dataListDATA >區域號碼</td>
            <td WIDTH=15% ALIGN="center" class=dataListDATA>電話號碼</td>
           </tr>
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
    </table>
    </td></tr>
  </table>     
  </DIV>


	<DIV ID="SRTAG2" onclick="srtag2" style="cursor:hand">
	<table border="1" width="100%" cellpadding="0" cellspacing="0" >
		<tr><td bgcolor="BDB76B" align="LEFT">用戶申請及施工進度狀態</td></tr>
	</table></DIV>

    <DIV ID="SRTAB2">  
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
	<tr><td class="dataListHEAD" height="23" width="10%">送件日期</td>
        <td height="23" bgcolor="silver" width="20%">
			<input type="text" name="key32" size="10" READONLY value="<%=dspKey(32)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text40">
			<input type="button" id="B32" name="B32" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C32" name="C32" style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>

		<td class="dataListHEAD" height="23" width="10%">申請日期</td>
        <td height="23" bgcolor="silver" width="20%" colspan=3>
			<input type="text" name="key8" size="10" READONLY value="<%=dspKey(8)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListENTRY" ID="Text39">
			<input  type="button" id="B8" name="B8" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C8"  name="C8"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>

        <td class="dataListHEAD" height="23" width="10%">退租日期</td>
        <td height="23" bgcolor="silver" colspan=3 width="20%">
			<input type="text" name="key10" size="10" READONLY value="<%=dspKey(10)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListENTRY" ID="Text41">
			<input  type="button" id="B10" name="B10" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C10"  name="C10"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>
	</tr>

	<tr><td class="dataListHEAD" height="23" width="10%">作廢日期</td>
        <td  height="23" bgcolor="silver" colspan=9 width="20%">
        <input type="text" name="key9" size="10" READONLY value="<%=dspKey(9)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text6">
        </td></tr>
	</table></DIV>


    <DIV ID="SRTAG4" onclick="SRTAG4" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="LEFT">績效歸屬</td></tr></table></DIV>
   <DIV ID="SRTAB4" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
    <TR>
<td  class="dataListHEAD" height="23" width="10%">開發經銷商</td>                                 
        <td  height="23" bgcolor="silver" width="20%">
        <%  If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1   Then 
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, rtrim(RTObj.CUSID) as CUSID, RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
          &"WHERE (RTObjLink.CUSTYID = '02')  "
       s="<option value="""" >(經銷商)</option>"
    Else
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, rtrim(RTObj.CUSID) as CUSID, RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
          &"WHERE (RTObjLink.CUSTYID = '02')  and rtobj.cusid='" & dspkey(26) & "' "
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(經銷商)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(26) Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>
           <select size="1" name="key26" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" ID="Select34">                                            
              <%=s%>
           </select>
        </td>  

        <td  class="dataListHEAD" height="23" width="10%">業務轄區</td>                                 
        <td  height="23" bgcolor="silver" width="20%" >
    <%  If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  Then 
       sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '3') "
       s="<option value="""" >(業務轄區)</option>"
    Else
       sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '3') AND AREAID='" & DSPKEY(27) & "' "
       s="<option value="""" >(業務轄區)</option>"
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(業務轄區)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("areaid")=dspkey(27) Then sx=" selected "
       s=s &"<option value=""" &rs("areaid") &"""" &sx &">" &rs("areanc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>    
           <select size="1" name="key27" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" ID="Select1">                                            
              <%=s%>
           </select>
        </td>       

        <td  class="dataListHEAD" height="23" width="10%">業務人員</td>                                 
        <td  height="23" bgcolor="silver"  width="20%">
<%  If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1   Then 
       sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
          &"WHERE (RTEmployee.DEPT IN ('B100', 'B106', 'B107', 'B109', 'B200', 'B300','B401', 'B600', 'B700', 'C100')) AND (RTEmployee.TRAN2 <> '10')  " _
          &" ORDER BY  RTObj.CUSNC "
       s="<option value="""" >(開發業務)</option>"
    Else
       sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
          &"WHERE RTEmployee.EMPLY='" & DSPKEY(28) & "' "
       s="<option value="""" >(開發業務)</option>"
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(開發業務)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("EMPLY")=dspkey(28) Then sx=" selected "
       s=s &"<option value=""" &rs("EMPLY") &"""" &sx &">" &rs("CUSNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>    
           <select size="1" name="key28" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" ID="Select3">                                            
              <%=s%>
           </select></td></tr>

    <TR><td  class="dataListHEAD" height="23" width="10%">裝機員工</td>                                 
        <td  height="23" bgcolor="silver" width="20%">
        <%  If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1   Then 
       sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
          &"WHERE (RTEmployee.DEPT IN ('B100', 'B106', 'B107', 'B109', 'B200', 'B300', 'B401','B600', 'B700', 'B701', 'C100')) AND (RTEmployee.TRAN2 <> '10')  " _
          &" ORDER BY  RTObj.CUSNC "
       s="<option value="""" >(開發業務)</option>"
    Else
       sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
          &"WHERE RTEmployee.EMPLY='" & DSPKEY(29) & "' "
       s="<option value="""" >(開發業務)</option>"
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(開發業務)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("EMPLY")=dspkey(29) Then sx=" selected "
       s=s &"<option value=""" &rs("EMPLY") &"""" &sx &">" &rs("CUSNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>    
           <select size="1" name="key29" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" ID="Select3">                                            
              <%=s%>
           </select></td>  
        <td  class="dataListHEAD" height="23" width="10%">二線開發人員</td>                                 
        <td width="35%" width="20%">
<%
	name=""
	if dspkey(30) <> "" then
		sqlxx=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			 &"where rtemployee.emply='" & dspkey(30) & "' "
		rs.Open sqlxx,conn
		if rs.eof then
			name="(對象檔找不該員工)"
		else
			name=rs("cusnc")
		end if
		rs.close
	end if
%>
<input type="text" name="key30"value="<%=dspKey(30)%>" <%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" size="8" maxlength="6" readonly class="dataListDATA" ID="Text23">
			<input type="BUTTON" id="B30" name="B30" style="Z-INDEX: 1"  value="...." onclick="Srdeveloperonclick()">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  alt="清除" id="C30" name="C30" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">
			<font size=2><%=name%></font></td>
			
	<td  class=dataListHEAD>實收金額</td>
    <td  bgcolor="silver" colspan=3>
        <input type="text" name="key33" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10" value="<%=dspKey(33)%>"   size="10" class=dataListEntry ID="Text7"></td>      
 </tr>
<TR><td class=dataListHEAD>話機數</td>
    <td bgcolor="silver" colspan=5>
        <input type="text" name="key39" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="2" value="<%=dspKey(39)%>"   size="10" class=dataListEntry ID="Text7"></td>
</tr>
 </table> 
  </div> 

    <DIV ID="SRTAG3" onclick="SRTAG3" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="LEFT">說明</td></tr></table></DIV>
   <DIV ID="SRTAB3" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
    <TR><TD align="CENTER">
     <TEXTAREA  cols="100%"  name="key23" rows=8  MAXLENGTH=500  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(23)%>" ID="Textarea1"><%=dspkey(23)%></TEXTAREA>
   </td></tr>
 </table> 
  </div>
  
  
	<DIV ID="SRTAG5" onclick="srtag5" style="cursor:hand">
	<table border="1" width="100%" cellpadding="0" cellspacing="0" >
		<tr><td bgcolor="BDB76B" align="LEFT">系統資訊</td></tr>
	</table></DIV>

    <DIV ID="SRTAB5">  
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
<tr>
        <td  class="dataListHEAD" height="23">建檔人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(19) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(19) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>    <input type="text" name="key19" size="6" READONLY value="<%=dspKey(19)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">建檔日期</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key20" size="10" READONLY value="<%=dspKey(20)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>  
<tr>
        <td  class="dataListHEAD" height="23">修改人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(21) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(21) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>    <input type="text" name="key21" size="6" READONLY value="<%=dspKey(21)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">修改日期</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key22" size="10" READONLY value="<%=dspKey(22)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>         
	</table></DIV>
  
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