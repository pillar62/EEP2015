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
                   case ucase("/webap/rtap/base/RTLessorAVScmty/RTLessorAVSCmtyCONTRACTd.asp")
                     '  response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       if i <> 1 then rs.Fields(i).Value=dspKey(i)    
                       if i=1 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         cusidxx="ET" & right("00" & trim(datePART("yyyy",NOW())),2) 
                         rsc.open "select max(CONTRACTNO) AS CONTRACTNO from RTLessorAVSCmtyCONTRACT where CONTRACTNO LIKE '" & cusidxx & "%'",conn
                         if len(rsc("CONTRACTNO")) > 0 then
                            dspkey(1)=cusidxx & right("0000" & cstr(cint(right(rsc("CONTRACTNO"),4)) + 1),4)
                         else
                            dspkey(1)=cusidxx & "0001"
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
                 case ucase("/webap/rtap/base/RTLessorAVScmty/RTLessorAVSCmtyCONTRACTd.asp")
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
       if ucase(runpgm)=ucase("/webap/rtap/base/RTLessorAVScmty/RTLessorAVSCmtyCONTRACTd.asp") then
          cusidxx="ET" & right("00" & trim(datePART("yyyy",NOW())),2) 
           rs.open "select max(CONTRACTNO) AS CONTRACTNO from RTLessorAVSCmtyCONTRACT where CONTRACTNO LIKE '" & cusidxx & "%'",conn
          if not rs.eof then
            dspkey(1)=rs("CONTRACTNO")
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
  title="AVS-City社區合約資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT COMQ1,CONTRACTNO,STRDAT,CONTDAT,CONTCNT,ENDDAT,CONTRACTOBJ,OBJTEL,OBJMOBILE,SIGNDEPT, " _
             &"SIGNPERSON,REMITBANK1,REMITBANK2,ACNO,AC,POWERBILLKIND,POWERBILLPAYKIND,SCALEAMT, PAYKIND, PAYCYCLE,  " _
             &"CLOSEDAT, CLOSEUSR, CANCELDAT, CANCELUSR, EUSR, EDAT, UUSR, UDAT,MEMO, BATCHNO, " _
             &"TARDAT, TUSR " _
             &"FROM RTLessorAVSCmtyContract WHERE COMQ1=0 "
  sqlList="SELECT COMQ1,CONTRACTNO,STRDAT,CONTDAT,CONTCNT,ENDDAT,CONTRACTOBJ,OBJTEL,OBJMOBILE,SIGNDEPT, " _
             &"SIGNPERSON,REMITBANK1,REMITBANK2,ACNO,AC,POWERBILLKIND,POWERBILLPAYKIND,SCALEAMT, PAYKIND, PAYCYCLE,  " _
             &"CLOSEDAT, CLOSEUSR, CANCELDAT, CANCELUSR, EUSR, EDAT, UUSR,UDAT,MEMO, BATCHNO,  " _
             &"TARDAT, TUSR " _
         &"FROM RTLessorAVSCmtyContract WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    
    if len(trim(DSPKEY(1))) = 0 THEN DSPKEY(1)=""
    if len(trim(DSPKEY(4))) = 0 THEN DSPKEY(4)=0
    if len(trim(DSPKEY(17))) = 0 THEN DSPKEY(17)=0
    If len(trim(dspkey(2)))=0 or Not Isdate(dspkey(2)) then
       formValid=False
       message="合約起始日不可空白或格式錯誤"    
    ELSEIf len(trim(dspkey(5)))=0 or Not Isdate(dspkey(5)) then
       formValid=False
       message="合約到期日不可空白或格式錯誤"          
    ELSEIf dspkey(5) <= dspkey(2) then
       formValid=False
       message="合約到期日必須大於合約起始日"                
    elseif len(trim(dspkey(6)))=0 then
       formValid=False
       message="合約對象名稱不可空白"    
    elseif (len(trim(dspkey(7))) = 0 and len(trim(dspkey(8))) = 0 )  then
       formValid=False
       message="合約對象聯絡電話及行動電話至少需留一項"    
    elseif len(trim(DSPKEY(9)))  = 0  THEN
       formValid=False
       message="元訊簽約部門不可空白"                             
    elseif  len(trim(dspkey(10)))=0 THEN
       formValid=False
       message="元訊簽約人員不可空白"                   
    elseif  len(trim(dspkey(11)))=0 AND (DSPKEY(18)="03" OR DSPKEY(18)="05") THEN
       formValid=False
       message="付款方式為匯款或劃撥時，匯款銀行代號不可空白"     
    elseif  len(trim(dspkey(12)))=0 AND (DSPKEY(18)="03" OR DSPKEY(18)="05") THEN
       formValid=False
       message="付款方式為匯款或劃撥時，匯款銀行分行代號不可空白"           
    elseif  len(trim(dspkey(13)))=0 AND (DSPKEY(18)="03" OR DSPKEY(18)="05") THEN
       formValid=False
       message="付款方式為匯款或劃撥時，匯款帳號不可空白"         
    elseif  len(trim(dspkey(14)))=0 AND (DSPKEY(18)="03" OR DSPKEY(18)="05") THEN
       formValid=False
       message="付款方式為匯款或劃撥時，匯款戶名不可空白"                          
    elseif  len(trim(dspkey(15)))=0 THEN
       formValid=False
       message="電費補助種類不可空白"                
    elseif  len(trim(dspkey(16)))=0 THEN
       formValid=False
       message="電費補助付款(先付/後付)不可空白"           
    elseif  dspkey(15)="02" AND dspkey(16)="01"  THEN
       formValid=False
       message="電費補助種類為(以度數計費)時，必須選擇(後付)"                  
    elseif  len(trim(dspkey(18)))=0 THEN
       formValid=False
       message="電費補助付款方式不可空白"          
    elseif  len(trim(dspkey(19)))=0 THEN
       formValid=False
       message="電費補助付款週期不可空白"   
    elseif  dspkey(19)="D" OR dspkey(19)="W" THEN
       formValid=False
       message="電費補助付款週期不可為(日/週)"                                     
    end if
  if accessmode="A" AND formValid=true then
     Set connxx=Server.CreateObject("ADODB.Connection")
     Set rsxx=Server.CreateObject("ADODB.Recordset")
     connxx.open DSN
     sqlxx="select count(*) as cnt from RTLessorAVScmtycontract where comq1=" & dspkey(0) & " and canceldat is null "
     rsxx.open sqlxx,connxx
     if rsxx("cnt") > 0 then
        formValid=False
        message="此社區已存在合約資料，不可再新增。(若續約者請改用合約續約作業)"             
     end if
     rsxx.close
     set rsxx=nothing
     connxx.close
     set connxx=nothing
  end if
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(26)=V(0)
        dspkey(27)=datevalue(now())
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
               
   Sub Srcounty12onclick()
       prog="RTGetbankbranchD.asp"
       prog=prog & "?KEY=" & document.all("KEY11").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key12").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub                        
   Sub Srcounty10onclick()
       prog="RTGetEMPLOYEED.asp"
       prog=prog & "?KEY=" & document.all("KEY9").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key10").value =  trim(Fusrid(0))
       End if       
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
       <td width="15%"  bgcolor="silver">
           <input type="text" name="key0"
                 <%=fieldRole(1)%> readonly size="5" value="<%=dspKey(0)%>" maxlength="8" class=dataListdata></td>
       <td width="15%" class=dataListHead>合約編號</td>
       <td width="15%"  bgcolor="silver">
           <input type="text" name="key1"
                 <%=fieldRole(1)%> readonly size="10" value="<%=dspKey(1)%>" maxlength="8" class=dataListdata></td> 
         </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(24))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(24)=V(0)
        End if  
       dspkey(25)=datevalue(now())
    else
        if len(trim(dspkey(26))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(26)=V(0)
        End if         
        dspkey(27)=datevalue(now())
    end if      
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
    '結案或作廢後,基本資料 protect
     If len(trim(dspKey(20))) > 0 or len(trim(dspKey(22))) > 0 Then
       fieldPa=" class=""dataListData"" readonly "
       FIELDPC=" DISABLED "
    Else
       fieldPa=""
       FIELDPC=""
    End If
    If len(trim(dspKey(20))) > 0 or len(trim(dspKey(22))) > 0 Then
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
  <span id="tags1" class="dataListTagsOn">AVS-City社區合約資訊</span>
                                                            
  <div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td><td>     
    <DIV ID="SRTAG0" onclick="srtag0" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="CENTER">基本資料</td></tr></table></div>
 <DIV ID=SRTAB0 >   
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">

<tr><td width="15%" class=dataListHEAD>合約起始日</td>
    <td width="18%" bgcolor="silver" >
        <input type="text" name="key2" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(2)%>" size="10" class=dataListEntry>
        <input  type="button" id="B2"  <%=FIELDPC%>  <%=FIELDPF%>  name="B2" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
       <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPC%>  <%=FIELDPF%>  alt="清除" id="C2"  name="C2"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">

        </td>
    <td width="15%" class=dataListHEAD>最近續約日</td>
    <td width="18%" bgcolor="silver" >
    <input type="text" name="key3" size="10" readonly MAXLENGTH=10 value="<%=dspKey(3)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListdata" ID="Text46">
    
    </td>      
    <td width="15%" class=dataListHEAD>續約次數</td>
    <td width="18%" bgcolor="silver" >
    <input type="text" name="key4" size="5" readonly MAXLENGTH=5 value="<%=dspKey(4)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListdata" ID="Text46">
    </td>          
</tr>
<tr><td class=dataListsearch>合約到期日</td>
    <td  bgcolor="silver" colspan=5>
        <input type="text" name="key5" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(5)%>"  size="10" class=dataListEntry>
        <input  type="button" id="B5"  <%=FIELDPC%>  <%=FIELDPF%>  name="B5" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
       <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPC%>  <%=FIELDPF%>  alt="清除" id="C5"  name="C5"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
               
        </td>
       </tr> 
<tr>       
    <td  class=dataListHEAD>簽約對象</td>
    <td  bgcolor="silver" >
    <input type="text" name="key6" size="12" MAXLENGTH=12 value="<%=dspKey(6)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text46">
    </td>       
    <td  class=dataListHEAD>連絡電話</td>
    <td  bgcolor="silver" >
    <input type="text" name="key7" size="15" MAXLENGTH=15 value="<%=dspKey(7)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text46">
    </td>     
    <td  class=dataListHEAD>行動電話</td>
    <td  bgcolor="silver" >
    <input type="text" name="key8" size="10" MAXLENGTH=10 value="<%=dspKey(8)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text46">
    </td>         
</tr>
<tr>       
    <td  class=dataListHEAD>元訊簽約部門</td>
    <td  bgcolor="silver" >
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(20))) = 0 AND len(trim(dspKey(22))) = 0  Then  
       sql="SELECT * FROM  RTDept " 
       If len(trim(dspkey(12))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & ">(簽約部門)</option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & ">(簽約部門)</option>"  
          sx=""
       end if     
       SXX10=" onclick=""Srcounty10onclick()""  "     
    Else
       sql="SELECT * FROM  RTDept WHERE dept='" & dspkey(9) & "'"
       SXX10=""
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("dept")=dspkey(9) Then sx=" selected "
       s=s &"<option value=""" &rs("dept") &"""" &sx &">" &rs("deptn4") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key9" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35">                                                                  
        <%=s%>
   </select>    </td>       
    <td  class=dataListHEAD>元訊簽約人員</td>
    <td  bgcolor="silver" colspan=3>
    <%  name="" 
           if dspkey(10) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(10) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>   
    <input type="text" name="key10" size="6"  maxlength="6" READONLY value="<%=dspKey(10)%>"  <%=fieldpA%><%=fieldRole(1)%> class="dataListEntry" ID="Text37"><font size=2><%=name%></font>
    <input type="button" id="B10"  name="B10"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX10%>  > </td>     
     
</tr>
<tr>
        <td   class="dataListHEAD" height="23">建檔人員</td>                                 
        <td   height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(24) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(24) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>    <input type="text" name="key24" size="6" READONLY value="<%=dspKey(24)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  width="15%" class="dataListHEAD" height="23">建檔日期</td>                                 
        <td  width="35%" height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key25" size="10" READONLY value="<%=dspKey(25)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>  
<tr>
        <td  class="dataListHEAD" height="23">修改人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(26) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(26) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>    <input type="text" name="key26" size="6" READONLY value="<%=dspKey(26)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">修改日期</td>                                 
        <td  height="23" bgcolor="silver"  colspan=3>
        <input type="text" name="key27" size="10" READONLY value="<%=dspKey(27)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>                
</table> </div>
<!--
    <table border="1" width="100%" cellpadding="0" cellspacing="0" id="tag2" style="display: none"> 
    -->
    <DIV ID="srtag1" onclick="srtag1" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table10">
    <tr><td bgcolor="BDB76B" align="CENTER">電費補助內容</td></tr></table></div>
     <DIV ID="srtab1" >
     <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table11">    

<tr><td width="15%" class=dataListHEAD>電費補助種類</td>
    <td width="18%" bgcolor="silver" >
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(20))) = 0 AND len(trim(dspKey(22))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='O3' " 
       If len(trim(dspkey(15))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & ">(電費補助種類)</option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & ">(電費補助種類)</option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='O3' AND CODE='" & dspkey(15) & "'"
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
    <td  width="15%" class=dataListHEAD>先付/後付</td>
    <td  width="18%" bgcolor="silver" >
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(20))) = 0 AND len(trim(dspKey(22))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='O4' " 
       If len(trim(dspkey(16))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & ">(電費補助方式)</option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & ">(電費補助方式)</option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='O4' AND CODE='" & dspkey(16) & "'"
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
   </select>    </td>  
    <td  width="15%" class=dataListHEAD>每月(度)補助金額</td>
    <td  width="18%" bgcolor="silver" >
   <input type="text" name="key17" size="6"  maxlength="6" value="<%=dspKey(17)%>"  <%=fieldpA%><%=fieldRole(1)%> class="dataListEntry" ID="Text37">
   </td>              
</tr>   
<tr><td  class=dataListHEAD>付款方式</td>
    <td  bgcolor="silver" >
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(20))) = 0 AND len(trim(dspKey(22))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='F5' " 
       If len(trim(dspkey(18))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & ">(付款方式)</option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & ">(付款方式)</option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='F5' AND CODE='" & dspkey(18) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(18) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key18" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35">                                                                  
        <%=s%>
   </select>
        </td>
    <td   class=dataListHEAD>付款週期</td>
    <td  bgcolor="silver" COLSPAN=3>
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(20))) = 0 AND len(trim(dspKey(22))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='F9' " 
       If len(trim(dspkey(19))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & ">(付款週期)</option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & ">(付款週期)</option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='F9' AND CODE='" & dspkey(19) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(19) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key19" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35">                                                                  
        <%=s%>
   </select>    </td>  
     
</tr>   
<tr><td  class=dataListHEAD>匯款銀行代號</td>
    <td  bgcolor="silver" >
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(20))) = 0 AND len(trim(dspKey(22))) = 0  Then  
       sql="SELECT  * FROM  RTBank ORDER BY  HEADNC, HEADNO " 
       If len(trim(dspkey(11))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & ">(匯款銀行代號)</option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & ">(匯款銀行代號)</option>"  
          sx=""
       end if     
       SXX12=" onclick=""Srcounty12onclick()""  "
    Else
       sql="SELECT  * FROM  RTBank WHERE HEADNO='" & dspkey(11) & "'"
       SXX12=""
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("HEADNO")=dspkey(11) Then sx=" selected "
       s=s &"<option value=""" &rs("HEADNO") &"""" &sx &">" &rs("HEADNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key11" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35">                                                                  
        <%=s%>
   </select>
        </td>
    <td   class=dataListHEAD>匯款分行代號</td>
    <td  bgcolor="silver" >
   <input class=dataListDATA name="key12"  maxlength=4 size=4 style="TEXT-ALIGN: left" value
            ="<%=dspkey(12)%>"  readOnly >
    <input type="button" id="B12"  name="B12"  width="100%" style="Z-INDEX: 1"  value="...." <%=SXX12%>  > </td>  
       <td  class=dataListHEAD>匯款帳號</td>
    <td   bgcolor="silver" >
   <input type="text" name="key13" size="16"  maxlength="16" value="<%=dspKey(13)%>"  <%=fieldpA%><%=fieldRole(1)%> class="dataListEntry" ID="Text37">
   </td>           
</tr>   
<tr><td  class=dataListHEAD>匯款戶名</td>
    <td  bgcolor="silver" COLSPAN=5>
    <input type="text" name="key14" size="50"  maxlength="50" value="<%=dspKey(14)%>"  <%=fieldpA%><%=fieldRole(1)%> class="dataListEntry" ID="Text37">
   </td>    
   </table>
   
   </div></div>
 
      <DIV ID="SRTAG2" onclick="srtag2" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr><td bgcolor="BDB76B" align="CENTER">合約狀態</td></tr></table></DIV>
    <DIV ID=SRTAB2 >  
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
      <tr>       
       <td width="15%" class="dataListHEAD" height="23">結案日</td>                                 
        <td width="18%"  height="23" bgcolor="silver">
                <input type="text" name="key20" size="10" value="<%=dspKey(20)%>"  <%=fieldpb%><%=fieldRole(1)%> readonly class="dataListDATA" ID="Text71">     
      </TD>
      <td  width="15%" class="dataListHEAD" height="23">結案人員</td>                                 
        <td width="18%"  height="23" bgcolor="silver" COLSPAN=3>
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
  %>          
                <input type="text" name="key21" size="6" value="<%=dspKey(21)%>"  <%=fieldpb%><%=fieldRole(1)%> readonly class="dataListDATA" ID="Text71"> <font size=2><%=name%></FONT>    
      </TD>
       </tr>           
     <tr>       
       <td width="15%" class="dataListHEAD" height="23">應付帳款編號</td>                                 
        <td width="18%"  height="23" bgcolor="silver">
                <input type="text" name="key29" READONLY size="12" value="<%=dspKey(29)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListDATA" ID="Text71">     
      </TD>
       <td width="15%" class="dataListHEAD" height="23">轉應付帳款日</td>                                 
        <td width="18%"  height="23" bgcolor="silver">
                <input type="text" name="key30" READONLY size="10" value="<%=dspKey(30)%>"  <%=fieldpb%><%=fieldRole(1)%>  class="dataListDATA" ID="Text71">     
      </TD>      
      <td  width="15%" class="dataListHEAD" height="23">轉應付帳款人員</td>                                 
        <td width="18%"  height="23" bgcolor="silver" >
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
  %>          
                <input type="text" name="key31" READONLY size="6" value="<%=dspKey(31)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListDATA" ID="Text71"> <font size=2><%=name%></FONT>    
      </TD>
       </tr>             
      <tr>
        <td  class="dataListHEAD" height="23">作廢日</td>                                 
        <td   height="23" bgcolor="silver" >
        <input type="text" name="key22" size="10" value="<%=dspKey(22)%>"  <%=fieldpb%><%=fieldRole(1)%> readonly class="dataListDATA" ID="Text72">     </td>
       <td  class="dataListHEAD" height="23">作廢人員</td>                                 
        <td   height="23" bgcolor="silver" COLSPAN=3 >
        <%  name="" 
           if dspkey(23) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(23) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>         
        <input type="text" name="key23" size="6" value="<%=dspKey(23)%>"  <%=fieldpb%><%=fieldRole(1)%> readonly class="dataListDATA" ID="Text71"><font size=2><%=name%></FONT>
      </tr>                         
  </table> 
  </DIV>
    <DIV ID="SRTAG3" onclick="srtag3" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="CENTER">備註說明</td></tr></table></DIV>
   <DIV ID="SRTAB3" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
    <TR><TD align="CENTER">
     <TEXTAREA  cols="100%"  name="key28" rows=8  MAXLENGTH=500  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(29)%>" ID="Textarea1"><%=dspkey(28)%></TEXTAREA>
   </td></tr>
 </table> 
  </DIV>    
<tr>                                   
  </div> 
<%  
End Sub 
' --------------------------------------------------------------------------------------------  
%>
<!-- #include file="RTGetUserRight.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->