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
                   case ucase("/webap/rtap/base/ktscust/ktsCUSTd.asp")
                       'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       if i <> 0 then rs.Fields(i).Value=dspKey(i)
                       if i=0 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         cusidxx="K" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
                         rsc.open "select max(cusid) AS cusid from KTSCUST where cusid like '" & cusidxx & "%' " ,conn
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
                 case ucase("/webap/rtap/base/KTSCUST/KTScustd.asp")
                  ' response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
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
       if ucase(runpgm)=ucase("/webap/rtap/base/KTSCUST/KTSCUSTD.asp") then
          cusidxx="K" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
          rsc.open "select max(cusid) AS cusid from KTScust where cusid like '" & cusidxx & "%' " ,conn
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
Sub SrDeveloperonclick()
       prog="RTGetDeveloperD.asp"
       prog=prog & "?KEY=" & document.all("KEY60").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key60").value =  trim(Fusrid(0))
       End if       
       end if
End Sub
   Sub Srsalesonclick()
       prog="RTGetsalesD.asp"
       prog=prog & "?KEY=" & ";" 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key49").value =  trim(Fusrid(0))
       End if       
       end if
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
  title="KTS用戶資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT CUSID,CUSNC,SOCIALID,BUSINESSTYPE,COTEL11,COTEL12,COFAX11,COFAX12,COEMAIL,CUTID1,TOWNSHIP1,RADDR1," _
             &"RZONE1,CUTID2,TOWNSHIP2, RADDR2, RZONE2, CUTID3, TOWNSHIP3, RADDR3, RZONE3,  COBOSS, BOSSSOCIALID, " _
             &"COCONTACTMAN, COCONTACTTEL11, COCONTACTTEL12, COCONTACTTEL13, COCONTACTFAX11, COCONTACTFAX12, COCONTACTMOBILE," _
             &"COCONTACTEMAIL, APFORMAPPLYDAT, APPLYDAT, APPLYTNSDAT, CONTRACTSTRDAT, NCICAPPLYREPLYDAT, NCICCUSID, " _
             &"NCICOPENDAT, FINISHDAT, DOCKETDAT, TRANSDAT, CANCELDAT, CANCELUSR, DROPDAT, NCICDROPFLAG, RUNONCEBILLDAT," _
             &"RUNONCESALESDAT, CONSIGNEE1, CONSIGNEE2, EMPLY,eusr,edat,uusr,udat,LISTTELDETAIL,SERVICE0809,CBBNPULLDAT,CBBNPULLUSR,MEMO,NOTATION, DEVELOPERID " _
             &"from KTSCUST WHERE CUSID='' "
  sqlList="SELECT CUSID,CUSNC,SOCIALID,BUSINESSTYPE,COTEL11,COTEL12,COFAX11,COFAX12,COEMAIL,CUTID1,TOWNSHIP1,RADDR1," _
             &"RZONE1,CUTID2,TOWNSHIP2, RADDR2, RZONE2, CUTID3, TOWNSHIP3, RADDR3, RZONE3,  COBOSS, BOSSSOCIALID, " _
             &"COCONTACTMAN, COCONTACTTEL11, COCONTACTTEL12, COCONTACTTEL13, COCONTACTFAX11, COCONTACTFAX12, COCONTACTMOBILE," _
             &"COCONTACTEMAIL, APFORMAPPLYDAT, APPLYDAT, APPLYTNSDAT, CONTRACTSTRDAT, NCICAPPLYREPLYDAT, NCICCUSID, " _
             &"NCICOPENDAT, FINISHDAT, DOCKETDAT, TRANSDAT, CANCELDAT, CANCELUSR, DROPDAT, NCICDROPFLAG, RUNONCEBILLDAT," _
             &"RUNONCESALESDAT, CONSIGNEE1, CONSIGNEE2, EMPLY,eusr,edat,uusr,udat,LISTTELDETAIL,SERVICE0809,CBBNPULLDAT,CBBNPULLUSR,MEMO,NOTATION, DEVELOPERID " _
             &"from KTSCUST WHERE "
  userDefineRead="Yes"      
  userDefineSave="Yes"       
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
  IF LEN(TRIM(DSPKEY(36))) = 0 THEN DSPKEY(36)=""
  IF LEN(TRIM(DSPKEY(54))) = 0 THEN DSPKEY(54)=""
  '檢查NCIC用戶編號之重覆性(930225 因速博同一用戶即使申請兩線仍會用同一用戶編號,故此一判斷無法正確執行
'  IF LEN(TRIM(DSPKEY(36))) > 0 THEN
'      Set connxx=Server.CreateObject("ADODB.Connection")  
'      Set rsxx=Server.CreateObject("ADODB.Recordset")
'      DSNXX="DSN=RTLIB"
'      connxx.Open DSNxx
      
	  '排除目前這筆資料本身
'      sqlXX="SELECT count(*) AS CNT FROM KTSCUST where NCICCUSID='" & trim(dspkey(36)) & "' and not ( CUSID='" & DSPKEY(0) & "')"
      
'      rsxx.Open sqlxx,connxx
'      s=""
      'Response.Write "CNT=" & RSXX("CNT")
'      If RSXX("CNT") > 0 Then
'         errflag="Y"
'         message="NCIC用戶編號已存在其它客戶資料中，不可重複輸入!"
'         formvalid=false
'      ELSE
'         ERRFLAG=""
'      End If
'      rsxx.Close
'      Set rsxx=Nothing
'      connxx.Close
'      Set connxx=Nothing    
'   end IF  
IF ERRFLAG <> "Y" THEN
  If len(trim(dspkey(31)))=0 or Not Isdate(dspkey(31)) then
       formValid=False
       message="用戶AP form申請日不可空白或格式錯誤"    
  elseif len(trim(dspkey(2)))=0 then
       formValid=False
       message="申請用戶統一編號或身分證號不可空白"   
  elseif len(trim(dspkey(3)))=0 then
       formValid=False
       message="申請用戶行業別不可空白"     
  elseif len(trim(dspkey(4)))=0 or len(trim(dspkey(5)))=0 then
       formValid=False
       message="用戶電話號碼不可空白"           
  elseif len(trim(dspkey(4))) > 0 and len(trim(dspkey(4))) < 2 then
       formValid=False
       message="用戶電話號碼區碼不可少於2位"      
  elseif len(trim(dspkey(5))) > 0 and len(trim(dspkey(5))) < 6 then
       formValid=False
       message="用戶電話號碼不可少於6位"   
  elseif len(trim(dspkey(6)))=0 or len(trim(dspkey(7)))=0 then
       formValid=False
       message="用戶傳真電話不可空白"                    
  elseif len(trim(dspkey(6))) > 0 and len(trim(dspkey(6))) < 2 then
       formValid=False
       message="用戶傳真電話區碼不可少於2位"      
  elseif len(trim(dspkey(7))) > 0 and len(trim(dspkey(7))) < 6 then
       formValid=False
       message="用戶傳真電話不可少於6位"             
'  elseif len(trim(dspkey(8)))=0 then
'       formValid=False
'       message="申請用戶EMAIL不可空白"          
  elseif len(trim(dspkey(9)))=0 then
       formValid=False
       message="申請用戶公司地址(縣市)不可空白"               
  elseif dspkey(9)<>"06" and dspkey(9)<>"15" and len(trim(dspkey(10)))=0 then
       formValid=False
       message="申請用戶公司地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(11)))=0 then
       formValid=False
       message="申請用戶公司地址(住址)不可空白"          
  elseif len(trim(dspkey(12)))=0 then
       formValid=False
       message="申請用戶公司地址(郵遞區號)不可空白"      
  elseif len(trim(dspkey(13)))=0 then
       formValid=False
       message="申請用戶帳寄地址(縣市)不可空白"               
  elseif dspkey(13)<>"06" and dspkey(13)<>"15" and len(trim(dspkey(14)))=0 then  
       formValid=False
       message="申請用戶帳寄地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(15)))=0 then
       formValid=False
       message="申請用戶帳寄地址(住址)不可空白"          
  elseif len(trim(dspkey(16)))=0 then
       formValid=False
       message="申請用戶帳寄地址(郵遞區號)不可空白"       
  elseif len(trim(dspkey(21)))=0 then
       formValid=False
       message="申請用戶公司負責人不可空白"    
  elseif len(trim(dspkey(22)))=0 then
       formValid=False
       message="申請用戶公司負責人身份證號不可空白"        
  elseif len(trim(dspkey(23)))=0 then
       formValid=False
       message="申請用戶公司連絡人不可空白"        
  elseif len(trim(dspkey(24)))=0 or len(trim(dspkey(25)))=0 then
       formValid=False
       message="公司連絡人電話號碼不可空白"           
  elseif len(trim(dspkey(24))) > 0 and len(trim(dspkey(24))) < 2 then
       formValid=False
       message="公司連絡人電話號碼區碼不可少於2位"      
  elseif len(trim(dspkey(25))) > 0 and len(trim(dspkey(25))) < 6 then
       formValid=False
       message="公司連絡人電話號碼不可少於6位"      
  elseif len(trim(dspkey(27))) > 0 and len(trim(dspkey(27))) < 2 then
       formValid=False
       message="公司連絡人傳真電話區碼不可少於2位"      
  elseif len(trim(dspkey(28))) > 0 and len(trim(dspkey(28))) < 6 then
       formValid=False
       message="公司連絡人傳真電話不可少於6位"      
  elseif len(trim(dspkey(29))) > 0 and len(trim(dspkey(29))) <> 10 then
       formValid=False
       message="公司連絡人行動電話長度須10位"      
  elseif len(trim(dspkey(17)))=0 then
       formValid=False
       message="申請用戶KTS安裝地址(縣市)不可空白"               
  elseif dspkey(17)<>"06" and dspkey(17)<>"15" and len(trim(dspkey(18)))=0 then    
       formValid=False
       message="申請用戶KTS安裝地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(19)))=0 then
       formValid=False
       message="申請用戶KTS安裝地址(住址)不可空白"          
  elseif len(trim(dspkey(20)))=0 then
       formValid=False
       message="申請用戶KTS安裝地址(郵遞區號)不可空白"        
  ELSEIf len(trim(dspkey(34)))=0 or Not Isdate(dspkey(34)) then
       formValid=False
       message="用戶合約起算日不可空白或格式錯誤"           
  ELSEIf len(trim(dspkey(48)))=0 AND len(trim(dspkey(49)))=0 then
       formValid=False
       message="用戶開發經銷商及開發業務至少須輸入一項"         
  ELSEIf dspkey(55) <> "Y" then
       formValid=False
       message="0809動態轉接服務必須勾選"                
  end if

  IF formValid=TRUE THEN
    IF dspkey(2) <> "" then
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

  'IF formValid=TRUE THEN
  '  IF dspkey(22) <> "" then
  '     idno=dspkey(22)
  '        AAA=CheckID(idno)
  '        SELECT CASE AAA
  '           CASE "True"
  '           case "False"
  '                 message="公司負責人身份證字號不合法"
  '                 formvalid=false 
  '           case "ERR-1"
  '                 message="公司負責人身份證號不可留空白或輸入位數錯誤"
  '                 formvalid=false       
  '           case "ERR-2"
  '                 message="公司負責人身份證字號的第一碼必需是合法的英文字母"
  '                 formvalid=false    
  '           case "ERR-3"
  '                 message="公司負責人身份證字號的第二碼必需是數字 1 或 2"
  '                 formvalid=false   
  '           case "ERR-4"
  '                 message="公司負責人身份證字號的後九碼必需是數字"
  '                 formvalid=false              
  '           case else
  '        end select  
  '  END IF
  'END IF  
 
END IF

'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(52)=V(0)
        dspkey(53)=datevalue(now())
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
   Sub Srcounty10onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY9").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key10").value =  trim(Fusrid(0))
          document.all("key12").value =  trim(Fusrid(1))
       End if       
       end if
   End Sub       
   Sub Srcounty14onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY13").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key14").value =  trim(Fusrid(0))
          document.all("key16").value =  trim(Fusrid(1))
       End if       
       end if
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
   document.All("key13").value=document.All("KEY9").value
   document.All("key14").value=document.All("key10").value
   document.All("key15").value=document.All("KEY11").value
   document.All("key16").value=document.All("key12").value
End Sub 
Sub SrAddrEqual2()
   document.All("key17").value=document.All("KEY9").value
   document.All("key18").value=document.All("key10").value
   document.All("key19").value=document.All("KEY11").value
   document.All("key20").value=document.All("key12").value
End Sub         
Sub SrAddrEqual3()
   document.All("key17").value=document.All("KEY13").value
   document.All("key18").value=document.All("key14").value
   document.All("key19").value=document.All("KEY15").value
   document.All("key20").value=document.All("key16").value
End Sub         
Sub SrAddrEqual4()
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
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<%	
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()
	'申請轉檔日,完工日,報竣日 三者任一欄有值就鎖定fieldPC, fieldPD
    If len(trim(dspKey(33))) > 0 OR len(trim(dspKey(38))) > 0 OR len(trim(dspKey(39))) > 0 OR len(trim(dspKey(40))) > 0 Then
       fieldPC=" class=""dataListData"" readonly "
       fieldpD=" disabled "
    Else
       fieldPC=""
       fieldpD=""
    End If%>
      <table width="60%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="10%" class=dataListHead>用戶代號</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key0"
                 <%=fieldRole(1)%> readonly size="15" value="<%=dspKey(0)%>" maxlength="15" class=dataListdata></td>
         <td  WIDTH=10%  class="dataListsearch" height="23">NCIC用戶編號</td>                                 
        <td   WIDTH=10% height="23" bgcolor="silver" >
        <input type="text" name="key36" size="15" value="<%=dspKey(36)%>"  <%=fieldpC%> class="dataListENTRY" ID="Text3">     
        </td>
  </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(50))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(50)=V(0)
        End if  
       dspkey(51)=datevalue(now())
    else
        if len(trim(dspkey(52))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(52)=V(0)
        End if         
        dspkey(53)=datevalue(now())
    end if      
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
    '用戶送件申請後,基本資料 protect
    If len(trim(dspKey(32))) > 0 OR len(trim(dspKey(38))) > 0 OR len(trim(dspKey(39))) > 0 OR len(trim(dspKey(40))) > 0 Then
       fieldPa=" class=""dataListData"" readonly "
       fieldpb=" disabled "
    Else
       fieldPa=""
       fieldpb=""
    End If
    '申請轉檔後，送件申請日protect
    If len(trim(dspKey(33))) > 0 OR len(trim(dspKey(38))) > 0 OR len(trim(dspKey(39))) > 0 OR len(trim(dspKey(40))) > 0 Then
       fieldPC=" class=""dataListData"" readonly "
       fieldpD=" disabled "
    Else
       fieldPC=""
       fieldpD=""
    End If
        '報竣後，全部資料protect(不含作廢日)
    If len(trim(dspKey(39))) > 0 OR len(trim(dspKey(38))) > 0  Then
       fieldPe=" class=""dataListData"" readonly "
       fieldpf=" disabled "
    Else
       fieldPe=""
       fieldpf=""
    End If
        '報竣轉檔後，報竣日protect(不含作廢日)
    If len(trim(dspKey(40))) > 0 Then
       fieldPg=" class=""dataListData"" readonly "
       fieldph=" disabled "
    Else
       fieldPg=""
       fieldph=""
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
  <span id="tags1" class="dataListTagsOn">KTS用戶資訊</span>
                                                            
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
<tr>
<td width="14%" class=dataListHEAD>用戶申請日</td>
    <td width="20%" bgcolor="silver" >
        <input type="text" name="key31" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(31)%>"  READONLY size="10" class=dataListEntry ID="Text48">
       <input  type="button" id="B31"  <%=fieldpb%> name="B31" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C31"  name="C31"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
      </td>      
    <td width="14%" class=dataListSEARCH>CBBN拉單日</td>       
    <td width="20%" bgcolor="silver">
        <input type="text" name="key56" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(56)%>"  READONLY size="10" class=dataListDATA ID="Text1">
      </td>         
    <td width="14%" class=dataListSEARCH>CBBN拉單人員</td>       
    <td width="20%" bgcolor="silver">
            <%  name="" 
           if dspkey(57) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(57) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>  
        <input type="text" name="key57" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="6"
               value="<%=dspKey(57)%>"  READONLY size="6" class=dataListDATA ID="Text28"><font size=2><%=name%></font>
      </td>               
      </tr>      
<tr><td width="10%" class=dataListHEAD>用戶(公司)名稱</td>
    <td  width="20%"  bgcolor="silver" >
        <input type="text" name="key1" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="40"
               value="<%=dspKey(1)%>"  size="40" class=dataListENTRY ID="Text22"></td>
     <td width="10%" class=dataListHEAD>統編</td>
    <td width="21%" bgcolor="silver" >
        <input type="text" name="key2" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10" value="<%=dspKey(2)%>" size="12" class=dataListENTRY></td>
     <td width="10%" class=dataListHEAD>行業別</td>
    <td width="20%" bgcolor="silver" >
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(DSPKEY(89))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='J8'  ORDER BY CODENC" 
       If len(trim(dspkey(3))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='J8' AND CODE='" & dspkey(3) & "' ORDER BY CODENC"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(3) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key3" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select8">                                                                  
        <%=s%>
   </select>  </td>                              
</tr>
<tr><td  class=dataListHEAD>用戶電話</td>
    <td  bgcolor="silver" >
        <input type="text" name="key4" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="4"
               value="<%=dspKey(4)%>"  size="4" class=dataListENTRY ID="Text50"><font size=2>-</font>
               <input type="text" name="key5" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="8"
               value="<%=dspKey(5)%>"  size="8" class=dataListENTRY ID="Text58"></td>
    <td  class=dataListHEAD>用戶傳真</td>
    <td  bgcolor="silver" >
        <input type="text" name="key6" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="4"
               value="<%=dspKey(6)%>"   size="4" class=dataListENTRY ID="Text53"><font size=2>-</font>
               <input type="text" name="key7" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="8"
               value="<%=dspKey(7)%>"  size="8" class=dataListENTRY ID="Text59"></td>      
     <td  class=dataListHEAD>用戶E-Mail</td>
    <td bgcolor="silver" >
        <input type="text" name="key8" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="30"
               value="<%=dspKey(8)%>"   size="30" class=dataListENTRY ID="Text54"></td>                              
</tr>
<tr><td class=dataListHEAD>公司地址</td>
    <td bgcolor="silver" COLSPAN=5>
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND len(trim(DSPKEY(89))) = 0 Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(9))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX10=" onclick=""Srcounty10onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(9) & "' " 
       SXX10=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(9) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
         <select size="1" name="key9" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select2" ><%=s%></select>
        <input type="text" name="key10" readonly  size="8" value="<%=dspkey(10)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text4"><font SIZE=2>(鄉鎮)                 
         <input type="button" id="B10" <%=fieldpb%> name="B10"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX10%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C10"  name="C10"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
        <input type="text" name="key12"  readonly size="5" value="<%=dspkey(12)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text12">
        <input type="text" name="key11" <%=fieldpa%> size="60" value="<%=dspkey(11)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text5"><font size=2>

         
      </tr>  
<tr><td class=dataListHEAD>帳單地址<br><input type="radio" name="rd1"  <%=fieldpb%> onClick="SrAddrEqual1()" ID="Radio1" VALUE="Radio1">同裝機</td>
    <td bgcolor="silver" COLSPAN=5>
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND len(trim(DSPKEY(89))) = 0 Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(13))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX14=" onclick=""Srcounty14onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(13) & "' " 
       SXX14=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(13) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
         <select size="1" name="key13" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select15" ><%=s%></select>
        <input type="text" name="key14" readonly  size="8" value="<%=dspkey(14)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text13"><font SIZE=2>(鄉鎮)                 
         <input type="button" id="B14" name="B14"  <%=fieldpb%> width="100%" style="Z-INDEX: 1"  value="...." <%=SXX14%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C14"  name="C14"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
        <input type="text" name="key16"  readonly size="5" value="<%=dspkey(16)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text60">
        <input type="text" name="key15" <%=fieldpa%> size="60" value="<%=dspkey(15)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text61"><font size=2>

         
      </tr>  

<tr>                                 
        <td  class="dataListHEAD" height="23">企業負責人</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key21" size="12" maxlength="12" value="<%=dspKey(21)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text8"></td>
        <td  class="dataListHEAD" height="23">身分證字號</td>                                 
        <td  height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key22" size="10" maxlength="10" value="<%=dspKey(22)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text7">
        </td>                                 
 
 </tr>        
<TR>        
        <td  class="dataListHEAD" height="23">企業連絡人</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key23" size="15" maxlength="15" value="<%=dspKey(23)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text9"></td>                                 
        <td  class="dataListHEAD" height="23">連絡人電話</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" name="key24" size="4" maxlength="4" value="<%=dspKey(24)%>"  <%=fieldpa%><%=fieldRole(1)%>  class="dataListentry" ID="Text45"><font size=3>-</font>
        <input type="text" name="key25" size="8" maxlength="8" value="<%=dspKey(25)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text16"><font size=2>分機</font>
        <input type="text" name="key26" size="5" maxlength="5" value="<%=dspKey(26)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text65"></td>
         <td  class="dataListHEAD" height="23">連絡人傳真1</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key27" size="4" maxlength="4" value="<%=dspKey(27)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text66"><font size=3>-</font>
        <input type="text" name="key28" size="8" maxlength="8" value="<%=dspKey(28)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text67"></td>                                 

 </tr>
 <TR>        
        <td  class="dataListHEAD" height="23">連絡人行動電話</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key29" size="10" maxlength="10" value="<%=dspKey(29)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text68"></td>                                 
        <td  class="dataListHEAD" height="23">連絡人E-MAIL</td>                                 
        <td  height="23" bgcolor="silver" COLSPAN=3>
        <input type="text" name="key30" size="30" maxlength="30" value="<%=dspKey(30)%>"  <%=fieldpa%><%=fieldRole(1)%>  class="dataListentry" ID="Text69"></td>
  </tr>       
<tr>
        <td  class="dataListHEAD" height="23">建檔人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(50) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(50) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>    <input type="text" name="key50" size="6" READONLY value="<%=dspKey(50)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">建檔日期</td>                                 
        <td  height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key51" size="10" READONLY value="<%=dspKey(51)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>  
<tr>
        <td  class="dataListHEAD" height="23">修改人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(52) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(52) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>    <input type="text" name="key52" size="6" READONLY value="<%=dspKey(52)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">修改日期</td>                                 
        <td  height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key53" size="10" READONLY value="<%=dspKey(53)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>         
</table> </div>
    <DIV ID="SRTAG1" onclick="srtag1" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table10">
    <tr><td bgcolor="BDB76B" align="LEFT">申請服務明細</td></tr></table></div>
     <DIV ID="SRTAB1" >
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table11">
<tr><td WIDTH=8% class=dataListHEAD>KTS安裝地址<br>
<input type="radio" name="rd2" <%=fieldpb%>  onClick="SrAddrEqual2()" ID="Radio2" VALUE="Radio2"><font SIZE=2>同公司</font>
<input type="radio"  <%=fieldpb%> name="rd2" onClick="SrAddrEqual3()" ID="radio"  <%=fieldpb%>><font SIZE=2>同帳寄</font>
    </td>
    <td WIDTH=20% bgcolor="silver" COLSPAN=5>
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND len(trim(DSPKEY(89))) = 0 Then 
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
         <select size="1" name="key17" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select16" ><%=s%></select>
        <input type="text" name="key18" readonly  size="8" value="<%=dspkey(18)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text14"><font SIZE=2>(鄉鎮)                 
         <input type="button" id="B18" name="B18"  <%=fieldpb%> width="100%" style="Z-INDEX: 1"  value="...." <%=SXX18%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C18"  name="C18"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
        <input type="text" name="key20"  readonly size="5" value="<%=dspkey(20)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text62">
        <input type="text" name="key19" <%=fieldpa%> size="60" value="<%=dspkey(19)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text63"><font size=2>
      </tr>  
   <tr>
    <td width="5%" class=dataListHEAD>加值服務</td>
    <td width="20%" bgcolor="silver" >
    <%   IF DSPKEY(54)="Y" THEN CHECK54=" CHECKED "%>
        <input type="checkbox" name="key54" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" 
               value="Y"  <%=CHECK54%> READONLY  bgcolor="silver"  ID="Text11"><font size=2>於帳單中增列長途電話通話明細</font><br>
    <%   IF DSPKEY(55)="Y" THEN CHECK55=" CHECKED "%>
        <input type="checkbox" name="key55"  <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" 
               value="Y"  <%=CHECK55%>  READONLY  bgcolor="silver" ID="Radio3"><font size=2>0809動態轉接服務</font>
        <TD width=8% class=dataListSEARCH>合約起算日</TD>     
        <TD width=15% bgcolor="silver" colspan=3>
                <input type="text" name="key34" size="10" READONLY value="<%=dspKey(34)%>" <%=fieldpa%> <%=fieldRole(1)%> class="dataListENTRY" ID="Text6"> 
                 <input type="button" id="B34"  name="B34" height="100%" width="100%" <%=fieldpB%>style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C34"  name="C34"   <%=fieldpB%>style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>
      </tr>
    <tr><td WIDTH=5% bgcolor="silver" COLSPAN=6>
    <input  type="button" id="STL" name="STL"  height="100%" width="100%" style="Z-INDEX: 1" value="顯示電話明細" onclick="SrSHOWTELLISTOnClick">
    </td></tr>
    <tr id="SRTAR1" STYLE="">
    <td colspan=6><table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table4">
    <tr>
            <td WIDTH=10% ALIGN="center" class=dataListDATA>項次</td>
            <td WIDTH=15% ALIGN="center" class=dataListDATA >電話號碼</td>
            <td WIDTH=15% ALIGN="center" class=dataListDATA>申請日</td>
            <td WIDTH=15% ALIGN="center" class=dataListDATA>申請單號</td>
            <td WIDTH=15% ALIGN="center" class=dataListDATA>作廢日</td>
            <td WIDTH=15% ALIGN="center" class=dataListDATA>作廢單號</td>
            <td WIDTH=15% ALIGN="center" class=dataListDATA>取消日</td>
          </tr>
          <%
    IF (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(DSPKEY(38))) = 0 THEN
       BTNENABLE=""
    ELSE
       BTNENABLE=" DISABLED "
    END IF
    %>
    <%
     s=""
     sql="SELECT  *  FROM  KTSCUSTD1 where cusid='" & dspkey(0) & "' and dropdat is null and applydat is not null and canceldat is null ORDER BY ENTRYNO"
     rs.Open sql,conn
     cnt=0
     Do While Not rs.Eof
       RESPONSE.Write "<TR>"
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("ENTRYNO") &  "&nbsp;</FONT></td>"
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("TEL11") & "-" & RS("TEL12") & "&nbsp;</FONT></td>"
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & datevalue(RS("APPLYDAT")) & "&nbsp;</FONT></td>"
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("APPLYNO") & "&nbsp;</FONT></td>"
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("DROPDAT") & "&nbsp;</FONT></td>"
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("DROPNO") & "&nbsp;</FONT></td>"
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("CANCELDAT") & "&nbsp;</FONT></td></TR>"
       rs.MoveNext
     Loop
     rs.Close
    %>
    </table>
    </td></tr>
  </table>     
  </DIV>       
<!--
    <table border="1" width="100%" cellpadding="0" cellspacing="0" id="tag2" style="display: none"> 
    -->
    <DIV ID="SRTAG2" onclick="srtag2" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="LEFT">績效歸屬</td></tr></table></div>
     <DIV ID=SRTAB2 >
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table7">
    <tr><td WIDTH="12%" class="dataListHEAD" height="23">備註</td>
        <td WIDTH="12%" height="23" bgcolor="silver">
			<input type="text" name="key59" size="8" maxlength="6" value="<%=dspKey(59)%>" <%=fieldRole(1)%> class="dataListEntry"></td>
		<td id="tagT1" WIDTH="12%" class="dataListHEAD" height="23">開發經銷商</td>
        <td WIDTH="12%" height="23" bgcolor="silver">
			<input type="text" name="key48" size="8" maxlength="10" value="<%=dspKey(48)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry"></td>
		<td WIDTH=10% class="dataListHEAD" height="23">盤商</td>                                 
        <td WIDTH=15% height="23" bgcolor="silver" colspan=3>
<%  If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(DSPKEY(38))) = 0 AND len(trim(DSPKEY(32))) = 0 Then 
       sql="SELECT RTConsignee.CUSID, RTObj.SHORTNC FROM RTConsignee INNER JOIN  RTConsigneeCASE ON " _
          &"RTConsignee.CUSID = RTConsigneeCASE.CUSID INNER JOIN RTObj ON RTConsignee.CUSID = RTObj.CUSID " _
          &"WHERE (RTConsigneeCASE.CASEID = '05') ORDER BY  RTObj.SHORTNC"
       s="<option value="""" >(盤商)</option>"
    Else
       sql="SELECT RTConsignee.CUSID, RTObj.SHORTNC FROM RTConsignee INNER JOIN  RTConsigneeCASE ON " _
          &"RTConsignee.CUSID = RTConsigneeCASE.CUSID INNER JOIN RTObj ON RTConsignee.CUSID = RTObj.CUSID " _
          &"WHERE (RTConsigneeCASE.CASEID = '05') AND RTConsignee.CUSID='" & DSPKEY(47) & "' ORDER BY RTObj.SHORTNC "
       
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(盤商)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(47) Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>    
           <select size="1" name="key47" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" ID="Select4">                                            
              <%=s%>
           </select> </td></tr>


	<tr><td id="tagT1" WIDTH="10%" class="dataListHEAD" height="23">開發業務</td>               
        <td WIDTH="20%" height="23" bgcolor="silver">
			<input type="TEXT" name="key49" size="7" readonly value="<%=dspKey(49)%>"  <%=fieldRole(1)%><%=dataProtect%> class="dataListDATA">
			<input type="BUTTON" id="B49" name="B49" <%=fieldRole(1)%> <%=fieldpf%> style="Z-INDEX: 1"  value="...." onclick="Srsalesonclick()"  >   
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldRole(1)%> <%=fieldpf%>alt="清除" id="C49"  name="C49"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">
			<font size=2><%=SrGetEmployeeName(dspKey(49))%></font>
		</td>

<%
	name=""
	if dspkey(60) <> "" then
		sqlxx=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			 &"where rtemployee.emply='" & dspkey(60) & "' "
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
		<td width="35%" colspan="5"><input type="text" name="key60"value="<%=dspKey(60)%>" <%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" size="8" maxlength="6" readonly class="dataListDATA" ID="Text23">
			<input type="BUTTON" id="B60" name="B60" style="Z-INDEX: 1"  value="...." onclick="Srdeveloperonclick()">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  alt="清除" id="C60" name="C60" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">
			<font size=2><%=name%></font></td></tr>
  </table>     
  </DIV>   
   <DIV ID="SRTAG3" onclick="srtag3" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr><td bgcolor="BDB76B" align="LEFT">用戶申請及施工進度狀態</td></tr></table></DIV>
    <DIV ID=SRTAB3 >  
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
       <tr>
        <td  width=14% class="dataListHEAD" height="23">KTS送件申請日</td>                                 
        <td  width=26% height="23" bgcolor="silver">
        <input type="text" name="key32" size="10" value="<%=dspKey(32)%>"  <%=fieldpe%><%=fieldRole(1)%> readonly class="dataListentry" ID="Text51">     
       <input type="button" id="B32"  name="B32" height="100%" width="100%" style="Z-INDEX: 1" <%=fieldpf%> value="...." onclick="SrBtnOnClick">
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C32"  name="C32"  <%=fieldpf%>  style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">     
        <td   width=10% class="dataListHEAD" height="23">KTS申請轉檔日</td>                                 
        <td   width=20% height="23" bgcolor="silver" >
        <input type="text" name="key33" size="23" value="<%=dspKey(33)%>" <%=fieldRole(1)%> readonly class="dataListDATA" ID="Text52">
        </td>  
        <td  WIDTH=10%  class="dataListHEAD" height="23">NCIC回覆日</td>                                 
        <td   WIDTH=20% height="23" bgcolor="silver">
        <input type="text" name="key35" size="10" READONLY value="<%=dspKey(35)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListdata" ID="Text26">     
        </td>
      
     </tr>             
        <tr>
        <td  width=10% class="dataListHEAD" height="23" STYLE="DISPLAY:NONE">用戶電話開通日</td>                                 
        <td  width=20% height="23" bgcolor="silver" colspan=3 STYLE="DISPLAY:NONE">
        <input type="text" name="key37" size="10" READONLY value="<%=dspKey(37)%>" <%=fieldpE%> <%=fieldRole(1)%> class="dataListentry" ID="Text24">
                <input type="button" id="B37"  name="B37" height="100%" width="100%" <%=fieldpF%>style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C37"  name="C37"   <%=fieldpF%>style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">     
       </TR>
       <tr>       
        <td   width=10% class="dataListHEAD" height="23">完工日期</td>                                 
        <td   width=20% height="23" bgcolor="silver" >
        <input type="text" name="key38" size="10" READONLY value="<%=dspKey(38)%>"  <%=fieldRole(1)%> class="dataListdata" ID="Text57">
        </td>
        <td  width=10% class="dataListHEAD" height="23" STYLE="DISPLAY:NONE">報竣日期</td>                                 
        <td  width=20%  height="23" bgcolor="silver" STYLE="DISPLAY:NONE">
        <input type="text" name="key39" size="10" READONLY value="<%=dspKey(39)%>" <%=fieldpC%> <%=fieldRole(1)%> class="dataListentry" ID="Text56">
         <input type="button" id="B39"  name="B39" height="100%" width="100%" <%=fieldpD%>style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C39"  name="C39"   <%=fieldpD%>style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">     
        <td  width=10%  class="dataListHEAD" height="23" STYLE="DISPLAY:NONE">報竣轉檔日</td>                                 
        <td   width=20% height="23" bgcolor="silver" STYLE="DISPLAY:NONE">
        <input type="text" name="key40" size="23" READONLY value="<%=dspKey(40)%>"  <%=fieldRole(1)%> class="dataListDATA" ID="Text57">     
        </td>
        <td  width=10%  class="dataListHEAD" height="23">退租日</td>                                 
        <td  width=20% height="23" bgcolor="silver">
        <input type="text" name="key43" size="10" READONLY value="<%=dspKey(43)%>"  <%=fieldRole(1)%> class="dataListentry" ID="Text21">
        <input type="button" id="B43"  name="B43" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C43"  name="C43"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">     </td>
        <td  width=10%  class="dataListHEAD" height="23">NCIC強退註記</td>     
        <td  width=20% height="23" bgcolor="silver">
        <input type="text" name="key44" size="5" READONLY value="<%=dspKey(44)%>"  <%=fieldRole(1)%> class="dataListdata" ID="Text29"></td>
       </TR>
        <tr>
        <td  width=10%  class="dataListHEAD" height="23">作廢日</td>                                 
        <td  width=20% height="23" bgcolor="silver">
         <input type="text" name="key41" size="10" READONLY value="<%=dspKey(41)%>"  <%=fieldRole(1)%> class="dataListDATA" ID="Text10">
        </td>
         <td   width=10% class="dataListHEAD" height="23">作廢人員</td>                                 
        <td   width=20% height="23" bgcolor="silver" colspan=3>
        <%  name="" 
           if dspkey(42) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(42) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>            
        <input type="text" name="key42" size="10" value="<%=dspKey(42)%>" <%=fieldRole(1)%> readonly class="dataListDATA" ID="Text15"><font size=2><%=name%></font>
        </td>
      </TR>

  </table> 
  </DIV>
    <DIV ID="SRTAG5" onclick="SRTAG5" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
    <tr><td bgcolor="BDB76B" align="LEFT">拆帳/繳款資訊</td></tr></table></DIV>
   <DIV ID="SRTAB5" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3">
    <TR>
        <td  WIDTH=14% class="dataListHEAD" height="23">NCIC佣獎拆帳日</td>                                 
        <td  WIDTH=26% height="23" bgcolor="silver">
        <input type="text" name="key45" size="10" READONLY value="<%=dspKey(45)%>"  <%=fieldRole(1)%> class="dataListdata" ID="Text25"></td>
        <td   WIDTH=10% class="dataListHEAD" height="23">業務獎金拆帳日</td>     
        <td  WIDTH=20% height="23" bgcolor="silver" >
        <input type="text" name="key46" size="10" READONLY value="<%=dspKey(46)%>"  <%=fieldRole(1)%> class="dataListdata" ID="Text27"></td>
        <td WIDTH=10% style="border-left:none;border-right:none">　</td><td WIDTH=20%>　</td>
        </tr>
 </table> 
  </div>   
    <DIV ID="SRTAG4" onclick="SRTAG4" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="LEFT">說明</td></tr></table></DIV>
   <DIV ID="SRTAB4" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
    <TR><TD align="CENTER">
     <TEXTAREA  cols="100%"  name="key58" rows=8  MAXLENGTH=500  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(58)%>" ID="Textarea1"><%=dspkey(58)%></TEXTAREA>
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
%><!-- #include virtual="/Webap/include/checkid.inc" --><!-- #include virtual="/Webap/include/companyid.inc" --><!-- #include file="RTGetUserRight.inc" --><!-- #include virtual="/Webap/include/employeeref.inc" -->