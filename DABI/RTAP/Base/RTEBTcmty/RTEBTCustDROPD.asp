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
  '  response.Write sql
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
                   case ucase("/webap/rtap/base/rtEBTcmty/RTEBTCUSTdropd.asp")
                      ' response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       if i <> 3 then rs.Fields(i).Value=dspKey(i)    
                       if i=3 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         rsc.open "select max(entryno) AS entryno from rtEBTcustdrop where comq1=" & dspkey(0) & " and lineq1=" & dspkey(1) & " and cusid='" & dspkey(2) & "' " ,conn
                         if len(rsc("entryno")) > 0 then
                            dspkey(3)=rsc("entryno") + 1
                         else
                            dspkey(3)=1
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
                 case ucase("/webap/rtap/base/rtEBTcmty/RTEBTcustdropd.asp")
                    'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                    ' if i<>0 and i <> 1 then rs.Fields(i).Value=dspKey(i)      
                      rs.Fields(i).Value=dspKey(i)        
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
       if ucase(runpgm)=ucase("/webap/rtap/base/rtEBTcmty/RTEBTCUSTdropD.asp") then
          rsc.open "select max(entryno) AS ENTRYNO from rtEBTcustdrop where comq1=" & dspkey(0) & " and lineq1=" & dspkey(1) & " and cusid='" & dspkey(2) & "' " ,conn
          if not rsc.eof then
            dspkey(3)=rsC("entryno")
          end if
          rsC.close
       '發送EMAIL通知客服派工窗口
          rsc.open "select rtEBTcustdrop.*,rtebtcust.cusnC,rtebtcust.avsno,RTEBTCMTYH.comn from rtEBTcustdrop INNER JOIN RTEBTCMTYH ON rtEBTcustdrop.COMQ1=RTEBTCMTYH.COMQ1 INNER JOIN  rtEBTcust ON  rtEBTcustdrop.COMQ1= rtEBTcust.COMQ1 AND rtEBTcustdrop.LINEQ1= rtEBTcust.LINEQ1 AND rtEBTcustdrop.CUSID= rtEBTcust.CUSID where rtEBTcustdrop.comq1=" & dspkey(0) & " and rtEBTcustdrop.lineq1=" & dspkey(1) & " and rtEBTcustdrop.cusid='" & dspkey(2) & "' " ,conn
          Set jmail = Server.CreateObject("Jmail.Message")
          jmail.charset="BIG5"
          jmail.from = "MIS@cbbn.com.tw"
          Jmail.fromname="東森AVS用戶退租派工通知"
          jmail.Subject = "東森AVS退租用戶︰" & RSc("CUSNC") & "-" & RSc("AVSNO") & "，待拆機派工通知"
          jmail.priority = 1  
          body="<html><body><table border=1 width=""80%""> " 
          BODY=BODY & "<tr><H3>東森AVS退租用戶待派工通知</h3></td></tr>" _
                   &"<tr><td bgcolor=lightblue align=center>主線</td><td bgcolor=lightblue align=center>社區名稱</td>"_
                   &"<td bgcolor=lightblue align=center>用戶名稱</td><td bgcolor=lightblue align=center>合約編號</td>"_
                   &"<td bgcolor=lightblue align=center>退租申請日</td><td bgcolor=lightblue align=center>預計服務中止日</td></tr>"
                  
          BODY=BODY & "<tr>" _
                   &"<td bgcolor=pink align=left>" &RSc("COMQ1") & "-" & RSc("LINEQ1")  &"</td>" _
                   &"<td bgcolor=pink align=left>" &RSc("COMN")  &"</td>" _
                   &"<td bgcolor=pink align=left>" &RSc("CUSNC")&"</td>" _
                   &"<td bgcolor=pink align=left>" &RSc("AVSNO")&"</td>" _
                   &"<td bgcolor=pink align=left>" &RSc("APPLYDAT")&"</td>" _
                   &"<td bgcolor=pink align=left>" &RSc("EXPECTDAT")&"</td></TR>" 
                       
          BODY=BODY & "</table><P><U>請執行退租拆機派工作業</U></body></html>"
          FROMEMAIL="MIS@CBBN.COM.TW"
          jmail.HTMLBody = BODY
          JMail.AddRecipient "mis@cbbn.com.tw","資訊部"
          JMail.AddRecipient "maybe0606@cbbn.com.tw","經銷窗口"
          JMail.AddRecipient "gracewu@cbbn.com.tw","客服退租派工窗口"
          jmail.Send ( "219.87.146.239" )      
             
          rsC.close
          set rsc=nothing
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
  numberOfKey=4
  title="AVS用戶服務終止作業資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT COMQ1,LINEQ1,CUSID, ENTRYNO, APPLYDAT,EXPECTDAT,REFUNDBANK,REFUNDBANKBRANCH,REFUNDSAVINGTYPE,REFUNDACCOUNT1, " _
             &"REFUNDACCOUNT2, REFUNDACCOUNT3, REFUNDACCOUNT4,POSTOFFICENAME, POSTOFFICEBRANCH, POSTOFFICEACCOUNT11,POSTOFFICEACCOUNT12, POSTOFFICEACCOUNT21, POSTOFFICEACCOUNT22,DROPCOD1, " _
             &"DROPCOD2, DROPCOD3, DROPCOD4, DROPCOD5, DROPCOD6, DROPCOD7, DROPCOD8, DROPCOD9, DROPCOD10, DROPCOD11, " _
             &"DROPCOD12, DROPCOD13, DROPCOD14, DROPCOD15, DROPOTHER, DROPDESC, DROPDAT, DROPUSR, TRANSCHKDAT, TRANSDAT, " _
             &"TRANSNO, EBTREPLYDAT, EBTREPLYCOD,EUSR,EDAT,UUSR,UDAT,FINISHDAT,FINISHCHKDAT,FINISHTNSDAT,FINISHTNSNO " _
             &"from RTEBTCUSTDROP WHERE COMQ1=0 "
  sqlList="SELECT COMQ1,LINEQ1,CUSID, ENTRYNO, APPLYDAT,EXPECTDAT,REFUNDBANK,REFUNDBANKBRANCH,REFUNDSAVINGTYPE,REFUNDACCOUNT1, " _
             &"REFUNDACCOUNT2, REFUNDACCOUNT3, REFUNDACCOUNT4,POSTOFFICENAME, POSTOFFICEBRANCH, POSTOFFICEACCOUNT11,POSTOFFICEACCOUNT12, POSTOFFICEACCOUNT21, POSTOFFICEACCOUNT22,DROPCOD1, " _
             &"DROPCOD2, DROPCOD3, DROPCOD4, DROPCOD5, DROPCOD6, DROPCOD7, DROPCOD8, DROPCOD9, DROPCOD10, DROPCOD11, " _
             &"DROPCOD12, DROPCOD13, DROPCOD14, DROPCOD15, DROPOTHER, DROPDESC, DROPDAT, DROPUSR, TRANSCHKDAT, TRANSDAT, " _
             &"TRANSNO, EBTREPLYDAT, EBTREPLYCOD,EUSR,EDAT,UUSR,UDAT,FINISHDAT,FINISHCHKDAT,FINISHTNSDAT,FINISHTNSNO " _
             &"from RTEBTCUSTDROP WHERE "
  userDefineRead="Yes"      
  userDefineSave="Yes"       
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
  if len(trim(dspkey(3)))=0 then dspkey(3)=0
  if len(trim(dspkey(6)))=0 then dspkey(6)=""
  if len(trim(dspkey(8)))=0 then dspkey(8)=""
  if len(trim(dspkey(13)))=0 then dspkey(13)=""  
  '退租原因initial value
  i=19
  DROPFLAG=""
  Do until I > 34
     IF LEN(TRIM(DSPKEY(i))) = 0 THEN
        DSPKEY(i)=0
     ELSE
        DROPFLAG="Y"      
     END iF
     I=I+1
  LOOP
  Set connXX=Server.CreateObject("ADODB.Connection")
  Set rsXX=Server.CreateObject("ADODB.Recordset")
  connXX.open DSN
  SQLxx="SELECT * FROM RTEBTCUST WHERE COMQ1=" &DSPKEY(0) & " AND LINEQ1=" & DSPKEY(1) & " AND CUSID='" & DSPKEY(2) & "' "
  RSXX.OPEN sqlxx,CONNXX
 ' RESPONSE.Write "MODE=" & ACCESSMODE
 ' if isnull(RSxx("DOCKETDAT")) AND ACCESSMODE="A" then
  if isnull(RSxx("DOCKETDAT")) then 
       formValid=False
       message="此用戶尚未報竣，不可建立用戶服務終止資料"    
  ELSEif NOT isnull(RSxx("DROPDAT")) AND ACCESSMODE="A" then
       formValid=False
       message="此用戶資料已退租，不可重複建立用戶服務終止資料"           
  ELSE
     If len(trim(dspkey(4)))=0  then
       formValid=False
       message="退租申請日不可空白或格式錯誤"    
     ELSEIf len(trim(dspkey(5)))=0  then
       formValid=False
       message="預計服務終止日不可空白或格式錯誤"        
     ELSEIf DROPFLAG <> "Y"  then
       formValid=False
       message="服務終止原因至少需勾選一項"               
     ELSEIf DSPKEY(34)=1 AND LEN(TRIM(DSPKEY(35)))=0  then
       formValid=False
       message="服務終止原因為[其它]，必須輸入說明"            
     ELSEIf LEN(TRIM(DSPKEY(6))) <> 0 AND (LEN(TRIM(DSPKEY(7))) = 0 OR LEN(TRIM(DSPKEY(8))) = 0 OR LEN(TRIM(DSPKEY(9))) = 0 OR LEN(TRIM(DSPKEY(10))) = 0 OR LEN(TRIM(DSPKEY(11))) = 0 OR LEN(TRIM(DSPKEY(12))) = 0 )then
       formValid=False
       message="銀行帳號資料不齊全"                 
     ELSEIf LEN(TRIM(DSPKEY(13))) <> 0 AND (LEN(TRIM(DSPKEY(15))) = 0 OR LEN(TRIM(DSPKEY(16))) = 0 OR LEN(TRIM(DSPKEY(17))) = 0 OR LEN(TRIM(DSPKEY(18))) = 0  )then
       formValid=False
       message="郵局帳號資料不齊全"                       
     end if
'-------UserInformation----------------------       
     logonid=session("userid")
     if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(45)=V(0)
        dspkey(46)=datevalue(now())
      end if       
   END IF 
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
   Sub Srcounty6onclick()
       prog="RTGetBANKBRANCHD.asp"
       prog=prog & "?KEY=" & document.all("KEY6").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
     '  MSGBOX FUSR
       if Fusrid(2) ="Y" then
          document.all("key7").value =  trim(Fusrid(0))
          document.all("kXX").value =  trim(Fusrid(1))
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
                 <%=fieldRole(1)%> readonly size="10" value="<%=dspKey(0)%>" maxlength="8" class=dataListdata></td>
           <td width="15%" class=dataListHead>主線序號</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key1"
                 <%=fieldRole(1)%> readonly size="10" value="<%=dspKey(1)%>" maxlength="8" class=dataListdata></td>                 
           <td width="15%" class=dataListHead>用戶序號</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key2"
                 <%=fieldRole(1)%> readonly size="15" value="<%=dspKey(2)%>" maxlength="15" class=dataListdata></td>
           <td width="15%" class=dataListHead>項次</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key3"
                 <%=fieldRole(1)%> readonly size="4" value="<%=dspKey(3)%>" maxlength="4" class=dataListdata ID="Text37"></td>                 
  </tr>
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
    '轉檔審核日存在(或已作廢)時,資料 protect
    If len(trim(dspKey(38))) > 0 OR len(trim(dspKey(36))) > 0 Then
       fieldPa=" class=""dataListData"" readonly "
       fieldpb=" disabled "
    Else
       fieldPa=""
       fieldpb=""
    End If
      
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    Set rsXX=Server.CreateObject("ADODB.Recordset")
    conn.open DSN%>
  <!--
  <span id="tags1" class="dataListTagsOn"
        onClick="vbscript:tag1.style.display=''    :tags1.classname='dataListTagsOn':
                          tag2.style.display='none':tags2.classname='dataListTagsOf'">基本資料</span>
  <span id="tags2" class="dataListTagsOf"
        onClick="vbscript:tag1.style.display='none':tags1.classname='dataListTagsOf':
                          tag2.style.display=''    :tags2.classname='dataListTagsOn'">發包安裝</span>           
  -->
  <span id="tags1" class="dataListTagsOn">AVS用戶服務終止資訊</span>
                                                            
  <div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td>
<td>     
    <DIV ID="SRTAG0" onclick="srtag0" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="LEFT">用戶基本資料</td></tr></table></div>
 <DIV ID=SRTAB0 >   
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
<tr><td width="15%" class=dataListHEAD>退租申請日</td>
    <td width="35%" bgcolor="silver" >
        <input type="text" name="key4" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(4)%>"  READONLY size="10" class=dataListEntry>
       <input  type="button" id="B4"  <%=fieldpb%> name="B4" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C4"  name="C4"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
      </td>
        <td  class="dataListHEAD" height="23">預計服務終止日</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" name="key5" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(5)%>"  READONLY size="10" class=dataListEntry ID="Text1">
       <input  type="button" id="B5" <%=fieldpb%> name="B5" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C5"  name="C5"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
       
</tr>
<tr><td width="15%" class=dataListHEAD>用戶(公司)名稱</td>
    <td  width="35%"  bgcolor="silver" >
<% sql="SELECT * FROM RTebtcust where comq1=" & DSPkey(0) & " and lineq1=" & DSPKEY(1) & " and cusid='" & DSPKEY(2) & "' "
   rs.open sql,conn
   if not rs.eof then
      cusnc=rs("cusnc")
   else
      cusnc=""
   end if
%>  
        <input type="text"   style="text-align:left;" maxlength="30"
               value="<%=cusnc%>"  size="30"  READONLY class=dataListDATA ID="Text22"></td>
<td width="15%" class=dataListHEAD>身分證(統編)</td>
    <td width="35%" bgcolor="silver" >
 <%  if not rs.eof then
      socialid=rs("socialid")
   else
      socialid=""
   end if    
   %>
        <input type="password" style="text-align:left;" maxlength="10"
               value="<%=socialid%>"   size="12"  READONLY class=dataListDATA ID="Text23"></td>               
</tr>
<tr><td class=dataListHEAD>ADSL裝機地址</td>
    <td bgcolor="silver" COLSPAN=3>
  <%s=""
    sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & RS("CUTID1") & "' " 
    sx=""    
    rsXX.Open sql,conn
    IF NOT RSXX.EOF THEN
       CUTNC=RSXX("CUTNC")
    ELSE
       CUTNC=""
    END IF
    RSXX.CLOSE
   %>
       <input type="text"  readonly  size="6" value="<%=CUTNC%>" maxlength="10" readonly  class="dataListDATA" ID="Text38" NAME="Text38">
        <input type="text"  readonly  size="8" value="<%=RS("TOWNSHIP1")%>" maxlength="10" readonly  class="dataListDATA" ID="Text4">              
        
        <input type="text"  size="10" value="<%=RS("VILLAGE1")%>" maxlength="10" class="dataListDATA" ID="Text5"><font size=2>
        <% aryOption=Array("村","里")
      s=""
      s="<option value=""" &RS("COD11") &""">" &RS("COD11") &"</option>"
        %>                                  
       <select size="1"  class="dataListDATA" ID="Select3">                                            
        <%=s%></select>       
        <input type="text"  size="6"  READONLY value="<%=RS("NEIGHBOR1")%>" maxlength="6" class="dataListDATA" ID="Text6"><font size=2>
        <% aryOption=Array("鄰")
        s=""
        s="<option value=""" &RS("COD12") &""">" &RS("COD12")&"</option>"
        %>                                  
       <select size="1"   class="dataListDATA" ID="Select4">                                            
        <%=s%></select>              
        <input type="text"  size="10"  READONLY value="<%=RS("STREET1")%>" maxlength="10"  class="dataListDATA" ID="Text27"><font size=2>
        <% aryOption=Array("路","街")
   s=""
      s="<option value=""" &RS("COD13") &""">" &RS("COD13") &"</option>"
%>                                  
       <select size="1" class="dataListDATA" ID="Select5">                                            
        <%=s%></select>                      
        <input type="text"  size="6"  READONLY value="<%=RS("SEC1")%>" maxlength="6"  class="dataListDATA" ID="Text29"><font size=2>
        <% aryOption=Array("段")
   s=""
      s="<option value=""" &RS("COD14") &""">" &RS("COD14") &"</option>"
%>                                  
       <select size="1"  class="dataListDATA" ID="Select6">                                            
        <%=s%></select>
        <input type="text"  size="6"  READONLY value="<%=RS("LANE1")%>" maxlength="6"  class="dataListDATA" ID="Text30"><font size=2>
        <% aryOption=Array("巷")
   s=""
      s="<option value=""" &RS("COD15") &""">" &RS("COD15") &"</option>"
%>                                  
       <select size="1"  class="dataListDATA" ID="Select9">                                            
        <%=s%></select>     
                <br>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
         <input type="text"  readonly size="5" value="<%=RS("RZONE1")%>" maxlength="5"  class="dataListDATA" ID="Text12">
        <input type="text"  size="10"  READONLY value="<%=RS("ALLEYWAY1")%>" maxlength="6" class="dataListDATA" ID="Text31"><font size=2>
                <% aryOption=Array("弄")
   s=""
      s="<option value=""" &RS("COD16")  &""">" &RS("COD16")  &"</option>"
%>                                  
       <select size="1"  class="dataListDATA" ID="Select10">                                            
        <%=s%></select>    
        <input type="text"  size="6"  READONLY value="<%=RS("NUM1")%>" maxlength="6"  class="dataListDATA" ID="Text32"><font size=2>
                <% aryOption=Array("號")
   s=""
      s="<option value=""" &RS("COD17") &""">" &RS("COD17") &"</option>"
%>                                  
       <select size="1"  class="dataListDATA" ID="Select11">                                            
        <%=s%></select>            
        <input type="text"  size="10"  READONLY value="<%=RS("FLOOR1")%>" maxlength="6"  class="dataListDATA" ID="Text33"><font size=2>
                <% aryOption=Array("樓")
   s=""
      s="<option value=""" &RS("COD18") &""">" &RS("COD18") &"</option>"
%>                                  
       <select size="1" class="dataListDATA" ID="Select12">                                            
        <%=s%></select>
        <input type="text"  size="6"  READONLY value="<%=RS("ROOM1")%>" maxlength="6"  class="dataListDATA" ID="Text34"><font size=2>
                <% aryOption=Array("室")
   s=""
      s="<option value=""" &RS("COD19") &""">" &RS("COD19") &"</option>"
%>                                  
       <select size="1" READONLY  class="dataListDATA" ID="Select13">                                            
        <%=s%></select>       
        </td>                                 
</tr>  
<tr>                                 
        <td  class="dataListHEAD" height="23">市話電話號碼</td>                                 
        <td  height="23" bgcolor="silver">
<%    sql=" select TELNO from rtEBTCUSTEXT where COMQ1=" & DSPKEY(0) & " AND LINEQ1=" & DSPKEY(1) & " AND CUSID='" & DSPKEY(2) & "' AND DROPDAT IS NULL "
      rsXX.Open sql,conn
      if rsXX.eof then
         TELNO=""
      else
         TELNO=rsXX("TELNO")
      end if
      rsXX.close
%>        
        <input type="text"  size="10" READONLY value="<%=TELNO%>"   class="dataListDATA" ID="Text8">  
        <td  class="dataListHEAD" height="23">聯絡E-MAIL</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text"  size="50" READONLY maxlength="50" value="<%=RS("EMAIL")%>"   class="dataListDATA" ID="Text7">
        </td>                                 
 
 </tr>        
<TR>        
        <td  class="dataListHEAD" height="23">聯絡人姓名</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text"  size="15"  READONLY maxlength="15" value="<%=RS("CONTACT")%>"   class="dataListDATA" ID="Text9">
        <td  class="dataListHEAD" height="23">聯絡電話(白天)</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" size="15"  READONLY maxlength="15" value="<%=RS("CONTACTTEL")%>"  class="dataListDATA" ID="Text16"></td>                                 
 </tr>
<TR>        
       <td  class="dataListhead" height="23">行動電話</td>                                 
        <td  height="23" bgcolor="silver" COLSPAN=3>
        <input type="text"  size="17"  READONLY maxlength="15" value="<%=RS("MOBILE")%>"   class="dataListDATA" ID="Text11"></td>                                 
 </tr> 
<tr>
        <td  class="dataListHEAD" height="23">建檔人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(43) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(43) & "' "
              rsXX.Open sql,conn
              if rsXX.eof then
                 name=""
              else
                 name=rsXX("cusnc")
              end if
              rsXX.close
           end if
  %>    <input type="text" name="key43" size="6" READONLY value="<%=dspKey(43)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">建檔日期</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key44" size="10" READONLY value="<%=dspKey(44)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text46">
        </td>       
 </tr>  
<tr>
        <td  class="dataListHEAD" height="23">修改人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(45) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(45) & "' "
              rsXX.Open sql,conn
              if rsXX.eof then
                 name=""
              else
                 name=rsXX("cusnc")
              end if
              rsXX.close
           end if
  %>    <input type="text" name="key45" size="6" READONLY value="<%=dspKey(45)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text47"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">修改日期</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key46" size="10" READONLY value="<%=dspKey(46)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text48">
        </td>       
 </tr>   
</table> </div>
<!--
    <table border="1" width="100%" cellpadding="0" cellspacing="0" id="tag2" style="display: none"> 
    -->
    <DIV ID="SRTAG1" onclick="srtag1" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="LEFT">服務終止原因</td></tr></table></div>
     <DIV ID=SRTAB1 >
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table7">
<tr>   <td id="tagT1" WIDTH="15%" rowspan=2 bgcolor="silver" height="23">
<%   IF DSPKEY(19)=1 THEN CHECK19=" CHECKED "%>
<INPUT type="checkbox" name="key19" value=1 <%=CHECK19%>   <%=fieldpa%>   <%=fieldRole(1)%> bgcolor="silver" ID="Text15" ><font size=2>申請其它方案</font>
<%   IF DSPKEY(20)=1 THEN CHECK20=" CHECKED "%>
<INPUT type="checkbox" name="key20" value=1 <%=CHECK20%>   <%=fieldpa%> <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox1" ><font size=2>品質不佳</font>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<%   IF DSPKEY(21)=1 THEN CHECK21=" CHECKED "%>
<INPUT type="checkbox" name="key21" value=1 <%=CHECK21%>   <%=fieldpa%>  <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox2" ><font size=2>服務不好</font>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<%   IF DSPKEY(22)=1 THEN CHECK22=" CHECKED "%>
<INPUT type="checkbox" name="key22" value=1 <%=CHECK22%>   <%=fieldpa%>  <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox3" ><font size=2>盜用因素</font>
<%   IF DSPKEY(23)=1 THEN CHECK23=" CHECKED "%>
<INPUT type="checkbox" name="key23" value=1 <%=CHECK23%>   <%=fieldpa%> <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox4"><font size=2>價格偏高</font>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<%   IF DSPKEY(24)=1 THEN CHECK24=" CHECKED "%>
<INPUT type="checkbox" name="key24" value=1 <%=CHECK24%>   <%=fieldpa%>  <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox5" ><font size=2>申裝時間過長</font>
<%   IF DSPKEY(25)=1 THEN CHECK25=" CHECKED "%>
<INPUT type="checkbox" name="key25" value=1 <%=CHECK25%>   <%=fieldpa%>  <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox6"><font size=2>維修不佳</font>
<%   IF DSPKEY(26)=1 THEN CHECK26=" CHECKED "%>
<INPUT type="checkbox" name="key26" value=1 <%=CHECK26%>   <%=fieldpa%> <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox7"><font size=2>遷移</font>
<%   IF DSPKEY(27)=1 THEN CHECK27=" CHECKED "%>
<INPUT type="checkbox" name="key27" value=1 <%=CHECK27%>   <%=fieldpa%>  <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox8" ><font size=2>使用不方便</font>
<%   IF DSPKEY(28)=1 THEN CHECK28=" CHECKED "%>
<INPUT type="checkbox" name="key28" value=1 <%=CHECK28%>   <%=fieldpa%> <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox9" ><font size=2>個人因素</font><br>
<%   IF DSPKEY(29)=1 THEN CHECK29=" CHECKED "%>
<INPUT type="checkbox" name="key29" value=1 <%=CHECK29%>   <%=fieldpa%>  <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox10" ><font size=2>公司因素</font>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<%   IF DSPKEY(30)=1 THEN CHECK30=" CHECKED "%>
<INPUT type="checkbox" name="key30" value=1 <%=CHECK30%>   <%=fieldpa%>  <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox11"><font size=2>帳單或繳款問題</font>
<%   IF DSPKEY(31)=1 THEN CHECK31=" CHECKED "%>
<INPUT type="checkbox" name="key31" value=1 <%=CHECK31%>   <%=fieldpa%> <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox12"><font size=2>其他業者原號退租</font>
<%   IF DSPKEY(32)=1 THEN CHECK32=" CHECKED "%>
<INPUT type="checkbox" name="key32" value=1 <%=CHECK32%>   <%=fieldpa%> <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox13"><font size=2>使用量低</font>
<%   IF DSPKEY(33)=1 THEN CHECK33=" CHECKED "%>
<INPUT type="checkbox" name="key33" value=1 <%=CHECK33%>   <%=fieldpa%> <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox14"><font size=2>轉用CABLE/ADSL</font><BR>
<%   IF DSPKEY(34)=1 THEN CHECK34=" CHECKED "%>
<INPUT type="checkbox" name="key34" value=1 <%=CHECK34%>   <%=fieldpa%> <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox15"><font size=2>其他︰原因說明</font>
<input type="text" name="key35" size="50" maxlength="10" value="<%=dspKey(35)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListENTRY" ID="Text15">
</td>               
 </tr>        
  </table>     
  </DIV> 
  </DIV>   
      <DIV ID="SRTAG2" onclick="srtag2" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
    <tr><td bgcolor="BDB76B" align="LEFT">退款帳戶資訊</td></tr></table></DIV>
    <DIV ID=SRTAB2 >
<table x:str border=1 cellpadding=0 cellspacing=0 width="100%"  ID="Table3">
 <col >
 <col  span=5 >
 <col  >
 <col >
 <col >
 <col >
 <tr height=22 style='height:16.5pt'>
  <td rowspan=4 WIDTH=2% class="dataListHEAD" align="center">退款帳號</td>
  <td WIDTH=6% class="dataListHEAD" align="center">金融機構</td>
  <td colspan=4 WIDTH="40%"><font size=2>總機構</font>
 <% s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND LEN(TRIM(DSPKEY(38))) < 1 Then 
       sql="SELECT headno,headnc FROM RTbank ORDER BY  HEADNC, HEADNO "
       If len(trim(dspkey(6))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
       SXX6=" onclick=""Srcounty6onclick()""  "
    Else
       sql="SELECT HEADNO,headnc FROM RTbank WHERE headno='" &dspkey(6) &"' ORDER BY  HEADNC, HEADNO "
       SXX6=""
    End If
    rsXX.Open sql,conn
    Do While Not rsXX.Eof
       If rsXX("headno")=dspkey(6) Then sx=" selected "
       s=s &"<option value=""" &rsXX("headno") &"""" &sx &">" &rsXX("headnc") &"</option>"
       rsXX.MoveNext
       sx=""
    Loop
    rsXX.Close%>
    <select name="key6"  <%=fieldpa%> <%=dataProtect%> size="1"  class=dataListEntry
       style="text-align:left;" maxlength="8" ID="Select1"><%=s%></select>      
  <font size=2>分支機構</font>
    <input name="key7"    <%=fieldpa%>  class=dataListDATA maxlength=4 size=4 style="TEXT-ALIGN: left" value
            ="<%=dspkey(7)%>"  readOnly ID="Text3">
    <input type="button" id="B7"  name="B7"   width="100%" style="Z-INDEX: 1"  value="..." <%=SXX6%>  >      
        <input  name="KXX"  class=dataListDATA maxlength=15 size=15 style="TEXT-ALIGN: left" value=""  readOnly ID="Text39">
  </td>
  <td rowspan=4  WIDTH=2%  class="dataListHEAD" align="center">郵局</td>
  <td colspan=3 rowspan=2  WIDTH=30%><font size=2>局名</font>
 <% s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND  LEN(TRIM(DSPKEY(38))) < 1  Then 
       sql="SELECT POSTOFFICENO,POSTOFFICENAME FROM RTPOSTOFFICE ORDER BY  POSTOFFICENAME "
       If len(trim(dspkey(13))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
       SXX13=" onclick=""Srcounty13onclick()""  "
    Else
       sql="SELECT POSTOFFICENO,POSTOFFICENAME FROM RTPOSTOFFICE WHERE POSTOFFICENO='" &dspkey(13) &"' ORDER BY  POSTOFFICENAME "
       SXX13=""
    End If
    rsXX.Open sql,conn
    Do While Not rsXX.Eof
       If rsXX("POSTOFFICENO")=dspkey(13) Then sx=" selected "
       s=s &"<option value=""" &rsXX("POSTOFFICENO") &"""" &sx &">" &rsXX("POSTOFFICENAME") &"</option>"
       rsXX.MoveNext
       sx=""
    Loop
    rsXX.Close%>
    <select  name="key13"  <%=fieldpa%><%=dataProtect%> class=dataListEntry size="1" 
       style="text-align:left;" maxlength="8" ID="Select2"><%=s%></select><font size=2>郵局</font>
  <input  name="key14"  maxlength=15 size=15 style="TEXT-ALIGN: left" value="<%=dspkey(14)%>"  <%=fieldpa%><%=fieldRole(1)%> class=dataListEntry ID="Text25"><font size=2>支局</font></td>
 </tr>
 <tr height=22 >
  <td height=22 class="dataListHEAD" align="center">存款種類</td>
  <td colspan=4  >
 <% s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND   LEN(TRIM(DSPKEY(38))) < 1   Then 
       sql="SELECT CODE,CODENC FROM RTCODE WHERE RTCODE.KIND='H7' ORDER BY  CODE "
       If len(trim(dspkey(8))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE RTCODE.KIND='H7' AND RTCODE.CODE='" &dspkey(8) &"' ORDER BY  CODE "
    End If
    rsXX.Open sql,conn
    Do While Not rsXX.Eof
       If rsXX("CODE")=dspkey(8) Then sx=" selected "
       s=s &"<option value=""" &rsXX("CODE") &"""" &sx &">" &rsXX("CODENC") &"</option>"
       rsXX.MoveNext
       sx=""
    Loop
    rsXX.Close%>
    <select  name="key8" <%=fieldpa%> <%=dataProtect%> class=dataListEntry size="1" 
       style="text-align:left;" maxlength="8" ID="Select7"><%=s%></select>
  </td>
 </tr>
 <tr height=22 >
  <td rowspan=2  class="dataListHEAD" align="center">帳號</td>
  <td class="dataListHEAD" align="center">分行別</td>
  <td class="dataListHEAD" align="center">科目</td>
  <td class="dataListHEAD" align="center">編(戶)號</td>
  <td class="dataListHEAD" align="center">檢支號</td>
  <td rowspan=2 WIDTH=5% class="dataListHEAD" align="center">存簿儲金</td>
  <td  WIDTH=5% class="dataListHEAD" align="center">局號</td>
  <td>
  <input name="key15"  maxlength=6 size=6 style="TEXT-ALIGN: left" value="<%=dspkey(15)%>" <%=fieldpa%><%=fieldRole(1)%> class=dataListEntry  ID="Text26"><font SIZE=2>－</font>
  <input  name="key16"  maxlength=1 size=1 style="TEXT-ALIGN: left" value="<%=dspkey(16)%>" <%=fieldpa%><%=fieldRole(1)%> class=dataListEntry ID="Text28"></td>
 </tr>
 <tr  >
  <td align="center"><input  name="key9"  maxlength=4 size=4 style="TEXT-ALIGN: left" value="<%=dspkey(9)%>" <%=fieldpa%><%=fieldRole(1)%>  class=dataListEntry ID="Text10"></td>
  <td align="center"><input  name="key10"  maxlength=5 size=5 style="TEXT-ALIGN: left" value="<%=dspkey(10)%>" <%=fieldpa%><%=fieldRole(1)%> class=dataListEntry ID="Text13">　</td>
  <td align="center"><input  name="key11"  maxlength=10 size=10 style="TEXT-ALIGN: left" value="<%=dspkey(11)%>" <%=fieldpa%><%=fieldRole(1)%> class=dataListEntry  ID="Text14">　</td>
  <td align="center"><input name="key12"  maxlength=1 size=1 style="TEXT-ALIGN: left" value="<%=dspkey(12)%>" <%=fieldpa%><%=fieldRole(1)%> class=dataListEntry  ID="Text21">　</td>
  <td class="dataListHEAD" align="center">帳號</td>
  <td ><input  name="key17"  maxlength=6 size=6 style="TEXT-ALIGN: left" value="<%=dspkey(17)%>" <%=fieldpa%><%=fieldRole(1)%> class=dataListEntry ID="Text35"><font SIZE=2>－</font>
  <input  name="key18"  maxlength=1 size=1 style="TEXT-ALIGN: left" value="<%=dspkey(18)%>" <%=fieldpa%><%=fieldRole(1)%> class=dataListEntry ID="Text36"></td>
 </tr>

</table>
    <DIV ID="SRTAG3" onclick="srtag3" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table4">
    <tr><td bgcolor="BDB76B" align="LEFT">作業進度</td></tr></table></div>
 <DIV ID="SRTAB3" >   
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="Table5">
<tr><td width="15%" class=dataListHEAD>作廢日</td>
    <td width="35%" bgcolor="silver" >
        <input type="text" name="key36" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(36)%>"  READONLY size="10" class=dataListdata ID="Text24">
      </td>
        <td  class="dataListHEAD" height="23">作廢人員</td>                                 
        <td  height="23" bgcolor="silver" >
        <%  name="" 
           if dspkey(37) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(37) & "' "
              rsXX.Open sql,conn
              if rsXX.eof then
                 name=""
              else
                 name=rsXX("cusnc")
              end if
              rsXX.close
           end if
  %>           
        <input type="text" name="key37" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(37)%>"  READONLY size="10" class=dataListdata ID="Text40"><font size=2><%=name%></font>
</tr>
<tr><td width="15%" class=dataListHEAD>申請轉檔審核日</td>
    <td width="35%" bgcolor="silver" >
        <input type="text" name="key38" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(38)%>"  READONLY size="10" class=dataListENTRY ID="Text41">
       <input  type="button" id="B38" name="B38" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  alt="清除" id="C38"  name="C38"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
      </td>
        <td  class="dataListHEAD" height="23">申請轉檔日</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" name="key39" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(39)%>"  READONLY size="10" class=dataListdata ID="Text42">
</tr>
<tr><td width="15%" class=dataListHEAD>轉檔序號</td>
    <td width="35%" bgcolor="silver" >
        <input type="text" name="key40" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(40)%>"  READONLY size="10" class=dataListdata ID="Text43">
      </td>
        <td  class="dataListHEAD" height="23">EBT回覆日</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" name="key41" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(41)%>"  READONLY size="10" class=dataListdata ID="Text44">
</tr>
<tr><td width="15%" class=dataListHEAD>EBT回覆結果</td>
    <td width="35%" bgcolor="silver" COLSPAN=3>
        <input type="text" name="key42" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(42)%>"  READONLY size="10" class=dataListdata ID="Text45">
      </td>
</tr>
<tr><td width="15%" class=dataListHEAD>拆機完成日</td>
    <td width="35%" bgcolor="silver" >
        <input type="text" name="key47" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(47)%>"  READONLY size="10" class=dataListDATA ID="Text51">
      </td>
    <td width="15%" class=dataListHEAD>拆機回報審核日</td>
    <td width="35%" bgcolor="silver" >
        <input type="text" name="key48" <%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(48)%>"  READONLY size="10" class=dataListENTRY ID="Text49">
       <input  type="button" id="B48" name="B48" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  alt="清除" id="C48"  name="C48"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
      </td>
</TR>
<TR>
    <td  class="dataListHEAD" height="23">拆機回報轉檔日</td>                                 
    <td  height="23" bgcolor="silver" >
    <input type="text" name="key49" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(49)%>"  READONLY size="10" class=dataListdata ID="Text50"></td>
    <td width="15%" class=dataListHEAD>拆機回報轉檔序號</td>
    <td width="35%" bgcolor="silver" >
        <input type="text" name="key50" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="15"
               value="<%=dspKey(50)%>"  READONLY size="15" class=dataListdata ID="Text52">
      </td>               
</tr>
</table></div>
  </DIV>

<%  RS.CLOSE
    conn.Close   
    set rs=Nothing   
    set rsXX=Nothing   
    set conn=Nothing 
End Sub 
' -------------------------------------------------------------------------------------------- 
Sub SrReadExtDB()
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrSaveExtDB(Smode)
	'Add by Laputa 2005/12/15
    Dim conn,rs
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open DSN
    'Set rs=Server.CreateObject("ADODB.Recordset")
    'sqlstr="SELECT COMQ1, LINEQ1, CUSID, DROPDAT FROM RTEBTCust WHERE comq1=" &dspKey(0) &" and lineq1=" &dspKey(1)& " and cusid='" &dspKey(2)& "' "
    sqlstr="update RTEBTCUST set dropdat = a.transchkdat, overdue ='' "_
    	  &"FROM RTEBTCUSTdrop a inner join rtebtcust b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 and a.cusid = b.cusid "_
    	  &"WHERE a.comq1=" &dspKey(0) &" and a.lineq1=" &dspKey(1)& " and a.cusid='" &dspKey(2)& "' and a.transchkdat='" &dspKey(38)& "' "
	conn.Execute sqlstr
'response.write "sqlstr="& sqlstr
    'rs.Open sqlstr,conn,3,3
    'If len(trim(rs("dropdat")))=0 Then
	'	rs("dropdat")=dspKey(38)
	'	rs.Update
    'End If
    'rs.close
    conn.close
    'set rs=nothing
    set conn=nothing
End Sub
' -------------------------------------------------------------------------------------------- 
' --------------------------------------------------------------------------------------------  
%>
<!-- #include file="RTGetUserRight.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->