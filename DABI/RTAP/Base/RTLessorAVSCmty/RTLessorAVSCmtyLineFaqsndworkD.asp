<%  
  Dim fieldRole,fieldPa
  fieldRole=Split(FrGetUserRight("RTCustD",Request.ServerVariables("LOGON_USER")),";")
%>
<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/cType.inc" -->
<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/dataList.inc" -->
<%
  Dim aryKeyName,aryKeyType(100),aryKeyValue(100),numberOfField,aryKey,aryKeyNameDB(100)
  Dim dspKey(100),userDefineKey,userDefineData,extDBField,extDB(100),userDefineRead,userDefineSave
  Dim conn,rs,i,formatName,sqlList,sqlFormatDB,userdefineactivex
  Dim aryParmKey
 '90/09/03 ADD-START
 '增加EXTDBFIELD2,EXTDBFILELD3(多檔維護)
  dim extDBField2,extDB2(100),extDBField3,extDB3(100),extDBField4,extDB4(100)
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
                   case ucase("/webap/rtap/base/RTLessorAVScmty/RTLessorAVSCmtyLineFaqSNDWORKd.asp")
                       'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       if i <> 3 then rs.Fields(i).Value=dspKey(i)    
                       if i=3 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         cusidxx="EY" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
                         rsc.open "select max(PRTNO) AS PRTNO from RTLessorAVSCmtyLineFaqSNDWORK where PRTNO like '" & cusidxx & "%' " ,conn
                         if len(rsc("PRTNO")) > 0 then
                            dspkey(3)=cusidxx & right("0000" & cstr(cint(right(rsc("PRTNO"),4)) + 1),4)
                         else
                            dspkey(3)=cusidxx & "0001"
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
                 case ucase("/webap/rtap/base/RTLessorAVScmty/RTLessorAVSCmtyLineFaqSNDWORKd.asp")
                ' response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
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
    ' 當程式為HB社區基本資料維護作業時,將sql自行產生之identify值dspkey(2)讀出至畫面
    if accessmode ="A" then
       runpgm=Request.ServerVariables("PATH_INFO")
       if ucase(runpgm)=ucase("/webap/rtap/base/RTLessorAVScmty/RTLessorAVSCmtyLineFaqSNDWORKd.asp") then
          cusidxx="EY" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
          rsc.open "select max(PRTNO) AS PRTNO from RTLessorAVSCmtyLineFaqSNDWORK where PRTNO like '" & cusidxx & "%' " ,conn
          if not rsC.eof then
            dspkey(3)=rsC("PRTNO")
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
   ' response.write "SQL=" & SQL
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
<link REL="stylesheet" HREF="/WebUtilityV4/DBAUDI/dataList.css" TYPE="text/css">
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
  title="AVS-City主線維修派工單資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT COMQ1,LINEQ1,FAQNO, PRTNO, SENDWORKDAT, PRTUSR, ASSIGNENGINEER," _
             &"ASSIGNCONSIGNEE,REALENGINEER, REALCONSIGNEE,  " _
             &"DROPDAT, DROPDESC, CLOSEDAT, BONUSCLOSEYM, BONUSCLOSEDAT, BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, STOCKCLOSEDAT, " _
             &" STOCKCLOSEUSR, STOCKFINCHK, MDF1,MDF2,HOSTNO,HOSTPORT, MEMO,PRTDAT,UNCLOSEDAT,DROPUSR,CLOSEUSR,eusr,edat,uusr,udat,BATCHNO,CDAT " _
             &"FROM RTLessorAVSCmtyLineFaqSNDWORK WHERE COMQ1=0 "
  sqlList="SELECT COMQ1,LINEQ1,FAQNO, PRTNO, SENDWORKDAT, PRTUSR, ASSIGNENGINEER, " _
             &"ASSIGNCONSIGNEE, REALENGINEER, REALCONSIGNEE,  " _
             &"DROPDAT, DROPDESC, CLOSEDAT, BONUSCLOSEYM, BONUSCLOSEDAT, BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, STOCKCLOSEDAT, " _
             &" STOCKCLOSEUSR, STOCKFINCHK, MDF1,MDF2,HOSTNO,HOSTPORT, MEMO,PRTDAT,UNCLOSEDAT,DROPUSR,CLOSEUSR,eusr,edat,uusr,udat,BATCHNO,CDAT " _
             &"FROM RTLessorAVSCmtyLineFaqSNDWORK WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  userDefineSave="Yes"
  extDBField=99
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
   if LEN(TRIM(dspkey(4))) = 0 then
       formValid=False
       message="派工日期不可空白" 
   ELSEif len(trim(DSPKEY(6))) = 0 and len(trim(DSPKEY(7))) = 0 THEN
       formValid=False
       message="預定裝機人員或經銷商必須輸入其中一項"        
   ELSEif len(trim(DSPKEY(6))) > 0 and len(trim(DSPKEY(7))) > 0 THEN
       formValid=False
       message="預定裝機人員及經銷商只能輸入其中一項"    
   ELSEif len(trim(DSPKEY(8))) = 0 and len(trim(DSPKEY(9))) = 0 AND LEN(TRIM(DSPKEY(12))) > 0 THEN
       formValid=False
       message="實際裝機人員或經銷商必須輸入其中一項" 
   ELSEif len(trim(DSPKEY(8))) > 0 and len(trim(DSPKEY(9))) > 0 AND LEN(TRIM(DSPKEY(12))) > 0 THEN
       formValid=False
       message="實際裝機人員及經銷商只能輸入其中一項"    
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

   Sub Srsales6onclick()
       prog="RTGetsalesD2.asp"
       prog=prog & "?KEY=" & document.all("AREAID").VALUE 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
          FUsrID=Split(Fusr,";")   
          if Fusrid(2) ="Y" then
             document.all("key6").value =  trim(Fusrid(0))
          End if       
       end if
   End Sub               
   Sub Srsales8onclick()
       prog="RTGetsalesD2.asp"
       prog=prog & "?KEY=" & document.all("AREAID").VALUE 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
          FUsrID=Split(Fusr,";")   
          if Fusrid(2) ="Y" then
             document.all("key8").value =  trim(Fusrid(0))
          End if       
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

   Sub SrTAG4()
      ' msgbox window.SRTAB1.style.display
       if window.SRTAB4.style.display="" then
          window.SRTAB4.style.display="none"
       elseif window.SRTAB4.style.display="none" then
          window.SRTAB4.style.display=""
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
      <table width="100%" border=1 cellPadding=0 cellSpacing=0 ID="Table2">
       <tr><td width="10%" class=dataListHead>社區序號</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key0"
                 <%=fieldRole(1)%> readonly size="5" value="<%=dspKey(0)%>" maxlength="5" class=dataListdata ID="Text9"></td>
           <td width="10%" class=dataListHead>主線序號</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key1"
                 <%=fieldRole(1)%> readonly size="5" value="<%=dspKey(1)%>" maxlength="5" class=dataListdata ID="Text9"></td>
          
           <td width="10%" class=dataListHead>客服單號</td>
           <td width="15%"  bgcolor="silver">
           <input type="text" name="key2"
                 <%=fieldRole(1)%> readonly size="13" value="<%=dspKey(2)%>" maxlength="12" class=dataListdata ID="Text24"></td>
             <td width="10%" class=dataListHead>派工單號</td>
           <td width="15%"  bgcolor="silver">
           <input type="text" name="key3"
                 <%=fieldRole(1)%> readonly size="12" value="<%=dspKey(3)%>" maxlength="12" class=dataListdata ID="Text24"></td>
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
    '獎金計算後，資料protect
    If len(trim(dspKey(12))) > 0 Then
       fieldPa=" class=""dataListData"" readonly "
       fieldPb=" disabled "
       FIELDPC=""
       FIELDPD=""
       FIELDP6=""
       FIELDP8=""
    Else
       fieldPa=""
       FIELDPC=" onclick=""SrBtnOnClick"" "
       FIELDPD=" onclick=""SrCLEAR"" "
       FIELDP6="  onclick=""Srsales6onclick()""  "
       FIELDP8="  onclick=""Srsales8onclick()""  "
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
  <span id="tags1" class="dataListTagsOn">派工單資料</span>
                                                            
  <div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td><td>        
      <DIV ID="SRTAG2" onclick="srtag2" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
    <tr>    <td bgcolor="BDB76B" align="center">維修派工單內容</td></tr></table></DIV>
    <DIV ID=SRTAB2 >
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
<tr><td width="14%" class=dataListHead>派工日期</td>
    <td width="40%" bgcolor="silver" COLSPAN=3>
        <input type="text" name="key4" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(4)%>" size="10" class=dataListEntry ID="Text1">
       <input  type="button" id="B4" name="B4" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=FIELDPC%>>
       <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpc%> alt="清除" id="C4"  name="C4"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" <%=FIELDPD%>>                              
    </td>
 </tr>
 <tr>
    <td width="14%" class=dataListHead>派工單列印日</td></td>
    <td width="40%" bgcolor="silver" >
        <input type="text" name="key26" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(26)%>" size="10" READONLY  class=dataListDATA ID="Text8">
     </td>    
    <td width="13%" class=dataListHead>最近列印人員</td></td>
<%
  name="" 
           if dspkey(5) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(5) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
          
 %>    
    <td width="35%" bgcolor="silver" >
        <input type="text" name="key5" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="6"
               value="<%=dspKey(5)%>" size="6" READONLY class=dataListDATA><font size=2><%=name%></font>
    </td></tr>

<tr><td class=dataListHEAD>預定裝機人員</td>
<td bgcolor="silver" >
<%
  sql="SELECT * FROM   RTLessorAVSCmtyLineFaqh LEFT OUTER JOIN RTLessorAVSCmtyLine on RTLessorAVSCmtyLineFaqh.COMQ1=RTLessorAVSCmtyLine.COMQ1 " _
     &" WHERE RTLessorAVSCmtyLineFaqh.COMQ1=" & DSPKEY(0) & " AND RTLessorAVSCmtyLineFaqh.LINEQ1=" & DSPKEY(1)
  rs.Open sql,conn
  IF RS.EOF THEN
     DOMAIN=""
  ELSE
 '    DOMAIN=RS("AREAID")
     if RS("consignee") <>"" then consignee ="disabled"
  END IF
  RS.CLOSE
  
  name="" 
           if dspkey(6) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(6) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
          
 %>
        <INPUT type="HIDDEN" name="AREAID" value="<%=DOMAIN%>">
        <input type="TEXT" name="key6" <%=fieldRole(1)%><%=dataProtect%> 
               style="text-align:left;" <%=fieldpa%> size="6" maxlength="6" 
               value="<%=dspKey(6)%>"  readonly class="dataListENTRY" >
           <input type="BUTTON" id="B6"  name="B6"  width="100%" style="Z-INDEX: 1"  value="...." <%=FIELDP6%> <%=consignee%> >   
           <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPD%> alt="清除" id="C6"  name="C6" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  <%=FIELDPD%>>
           <font size=2><%=name%></font>
 </td>
<td class=dataListHEAD>實際裝機人員</td>
<td bgcolor="silver" >
<%
  name="" 
           if dspkey(8) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(8) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
          
 %>
        <input type="TEXT" name="key8" <%=fieldRole(1)%><%=dataProtect%> 
               style="text-align:left;" size="6" maxlength="6" 
               value="<%=dspKey(8)%>"  <%=fieldpa%> readonly class="dataListENTRY" ID="Text3">
           <input type="BUTTON" id="B8" name="B8"  width="100%" style="Z-INDEX: 1"  value="...."  <%=FIELDP8%>  >   
           <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPD%> alt="清除" id="C8"  name="C8"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"   <%=FIELDPD%>>
           <font size=2><%=name%></font>
</td>
 </tr>  
<tr><td  class=dataListHead>預定裝機經銷商</td>
    <td  bgcolor="silver" >
        <%  If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 and len(trim(dspkey(12)))=0 Then 
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID inner join rtconsigneeisp on rtobj.cusid=rtconsigneeisp.cusid " _
          &"WHERE (RTObjLink.CUSTYID = '02' and rtconsigneeisp.isp='04')  "
       s="<option value="""" >(經銷商)</option>"
    Else
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID  inner join rtconsigneeisp on rtobj.cusid=rtconsigneeisp.cusid " _
          &"WHERE (RTObjLink.CUSTYID = '02')  and rtobj.cusid='" & dspkey(7) & "' and rtconsigneeisp.isp='04' "
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(經銷商)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(7) Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>
           <select size="1" name="key7" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" ID="Select34">                                            
              <%=s%>
           </select>
    </td>
    <td  class=dataListHead>實際裝機經銷商</td>
    <td  bgcolor="silver" >
        <%  If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 and len(trim(dspkey(12)))=0  Then 
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID inner join rtconsigneeisp on rtobj.cusid=rtconsigneeisp.cusid " _
          &"WHERE (RTObjLink.CUSTYID = '02' and rtconsigneeisp.isp='04')  "
       s="<option value="""" >(經銷商)</option>"
    Else
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID inner join rtconsigneeisp on rtobj.cusid=rtconsigneeisp.cusid " _
          &"WHERE (RTObjLink.CUSTYID = '02')  and rtobj.cusid='" & dspkey(9) & "' and rtconsigneeisp.isp='04' "
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(經銷商)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(9) Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>
           <select size="1" name="key9" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" ID="Select1">                                            
              <%=s%>
           </select>
    </td></tr>
<tr>
    <td  class=dataListHead>完工結案日</td>
    <td  bgcolor="silver" >
        <input type="text" name="key12" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(12)%>" size="10" READONLY class=dataListdata ID="Text13">
               <!--
       <input  type="button" id="B12" name="B12" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=FIELDPC%>>
       <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpa%> alt="清除" id="C12"  name="C12"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" <%=FIELDPD%>>                              
    -->
    </td>   
    <td  class=dataListHead>未完工結案日</td>
    <td  bgcolor="silver" >
        <input type="text" name="key27" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(27)%>" size="10" READONLY class=dataListdata ID="Text7">
               <!--
       <input  type="button" id="B27" name="B27" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=FIELDPC%>>
       <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpa%> alt="清除" id="C27"  name="C27"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" <%=FIELDPD%>>                              
    -->
    </td>       
  </tr>
  <tr>
      <td  class=dataListHead>結案人員</td>
    <%
  name="" 
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
          
 %>    
    <td  bgcolor="silver"  colspan=3>
        <input type="text" name="key29" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(29)%>" size="10" READONLY class=dataListDATA ID="Text29">
               <font size=2><%=name%></font>
    </td>        
 </TR>
  <tr>
<td  class=dataListHead>作廢日期</td>
    <td  bgcolor="silver" >
        <input type="text" name="key10" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(10)%>" size="10" READONLY class=dataListDATA ID="Text27">
    </td>    
    <td  class=dataListHead>作廢人員</td>
    <%
  name="" 
           if dspkey(28) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(28) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
          
 %>    
    <td  bgcolor="silver" >
        <input type="text" name="key28" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(28)%>" size="10" READONLY class=dataListDATA ID="Text28">
               <font size=2><%=name%></font>
    </td>        
 </TR>

 <tr>
   <td class=dataListHEAD>未完工/作廢原因</td>          
   <td  height="23" bgcolor="silver" COLSPAN=3>
     <input type="text" name="key11" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="90"
               value="<%=dspKey(11)%>" size="90" class=dataListEntry ID="Text2">
   </TD>
   </TR>
 <tr> 
    <td  class=dataListHead>MDF位置</td>
    <td  bgcolor="silver" >
        <input type="text" name="key21" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(21)%>" size="10"  class=dataListENTRY ID="Text12">
        <font size=3>端子版位置</font>
        <input type="text" name="key22" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(22)%>" size="10"  class=dataListENTRY ID="Text4">               
    </td>    
    <td  class=dataListHead>SWITCH(PNA)編號</td>
    <td  bgcolor="silver" >
        <input type="text" name="key23" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(23)%>" size="10"  class=dataListENTRY ID="Text5">
        <font size=3>PORT號</font>
        <input type="text" name="key24" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(24)%>" size="10"  class=dataListENTRY ID="Text6">               
    </td>        
</tr>           
<tr>
 <td class=dataListHEAD>獎金計算月</td>
    <td bgcolor="silver">   
    <input type="text" name="key13" size="6" MAXLENGTH=6 value="<%=dspKey(13)%>"  READONLY  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text10">
    </td>                            
   <td class=dataListHead>獎金計算日</td>          
    <td bgcolor="silver">   
    <input type="text" name="key14" size="10" MAXLENGTH=10 value="<%=dspKey(14)%>"  READONLY  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text11">
    </td>  
  </TR>
  <tr>        
   <td class=dataListHead>獎金結帳員</td>          
    <td  bgcolor="silver">     
   <%   NAME=""
          if dspkey(15) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(15) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name="(對象檔找不到結帳人員)"
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if    %>
    <input type="text" name="key15" size="6" MAXLENGTH=6 value="<%=dspKey(15)%>"  READONLY  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text15"><font size=3><%=NAME%></font>
    </td>
   <td class=dataListHead>獎金會計審核日</td>          
    <td  bgcolor="silver">     
 <input type="text" name="key16" size="10" MAXLENGTH=10 value="<%=dspKey(16)%>"  READONLY  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text16">
    </td>    
</tr>             
<tr>
 <td class=dataListHEAD>庫存計算月</td>
    <td bgcolor="silver">   
    <input type="text" name="key17" size="6" MAXLENGTH=6 value="<%=dspKey(17)%>"  READONLY  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text14">
    </td>                            
   <td class=dataListHead>庫存計算日</td>          
    <td bgcolor="silver">   
    <input type="text" name="key18" size="10" MAXLENGTH=10 value="<%=dspKey(18)%>"  READONLY  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text21">
    </td>  
  </TR>
  <tr>        
   <td class=dataListHead>庫存結帳員</td>          
    <td  bgcolor="silver">     
   <%   NAME=""
          if dspkey(19) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(19) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name="(對象檔找不到庫存結帳人員)"
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if    %>
    <input type="text" name="key19" size="6" MAXLENGTH=6 value="<%=dspKey(19)%>"  READONLY  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text25"><font size=3><%=NAME%></font>
    </td>
   <td class=dataListHead>庫存會計審核日</td>          
    <td  bgcolor="silver">     
    <input type="text" name="key20" size="10" MAXLENGTH=10 value="<%=dspKey(20)%>"  READONLY  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text26">
    </td>    
</tr>  
<tr>
 <td class=dataListSEARCH>應收帳款編號</td>
    <td bgcolor="silver">   
    <input type="text" name="key34" size="13" MAXLENGTH=13 value="<%=dspKey(34)%>"  READONLY  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text10">
    </td>                            
   <td class=dataListSEARCH>轉應收帳款日</td>          
    <td bgcolor="silver">   
    <input type="text" name="key35" size="10" MAXLENGTH=10 value="<%=dspKey(35)%>"  READONLY  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text11">
    </td>  
  </TR>
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
  %>    <input type="text" name="key30" size="6" READONLY value="<%=dspKey(30)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">建檔日期</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key31" size="10" READONLY value="<%=dspKey(31)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
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
  %>    <input type="text" name="key32" size="6" READONLY value="<%=dspKey(32)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">修改日期</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key33" size="10" READONLY value="<%=dspKey(33)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>     
  </table> 
  </DIV>    
     <DIV ID="SRTAG6" onclick="SRTAG6" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="center">維修過程說明</td></tr></table></DIV>
   <DIV ID="SRTAB6" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
     <TR><TD align="LEFT">
         <%
    Set rsxx=Server.CreateObject("ADODB.Recordset")
    sqlfaqlist="SELECT * FROM RTLessorAVSCmtyLineFaqSndworkFixCode RIGHT OUTER JOIN " _
            &"RTCode ON RTLessorAVSCmtyLineFaqSndworkFixCode.FIXcod = RTCode.CODE AND faqno ='" & DSPKEY(2) & "' AND PRTNO='" & DSPKEY(3) & "' "  _
            &"WHERE RTCODE.KIND = 'A9' "
  '  response.write sqlfaqlist
    rsxx.open sqlfaqlist,conn
    Dtlcnt=0
    Do until rsxx.eof
       IF not IsNull(RSxx("faqno")) then
          fieldpc=" checked "
       else
          fieldpc=""
       end if
    '-----
  %>
        <p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 5px; MARGIN-TOP: 5px">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" name="ext<%=Dtlcnt%>" <%=fieldpc%> <%=fieldpd%> value="<%=rsxx("code")%>"><font size=2><%=rsxx("codenc")%></font></p>  
  <%
    dtlcnt=Dtlcnt+1
    rsxx.MoveNext
    loop
    rsxx.close
    set rsxx=nothing
  %>                               
      <p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 3px; MARGIN-TOP: 3px" align="center">
      </font>
     <TEXTAREA  cols="100%" name="key25" rows=7  MAXLENGTH=300 <%=fieldpa%>  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(25)%>" ID="Textarea1"><%=dspkey(25)%></TEXTAREA>
   </td></tr>
 </table>            
   </table> 
  </div> 
<% 
    conn.Close   
    set rs=Nothing   
    set conn=Nothing 
End Sub 
Sub SrSaveExtDB(Smode)
    Dim conn,rs
' Smode A:add U:update
' extDBField = n
' use extDB(i) for Screen ,and map it to DataBase
'
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open DSN
    Set rs=Server.CreateObject("ADODB.Recordset")
    Set comm=Server.CreateObject("ADODB.Command")
    
'------ RTObj ---------------------------------------------------
    DELFAQlist="delete from RTLessorAVSCmtyLineFaqSndworkFixCode where COMQ1=" & dspkey(0) & " AND LINEQ1=" & DSPKEY(1) & " AND faqno='" & dspkey(2) & "' AND PRTNO='" & DSPKEY(3) & "' "
    conn.Execute DELFAQlist  
    For i=0 to 99
        if len(trim(extdb(i))) > 0  then
           rs.Open "SELECT * FROM RTLessorAVSCmtyLineFaqSndworkFixCode WHERE COMQ1=" & dspkey(0) & " AND LINEQ1=" & DSPKEY(1) & " AND faqno='" &dspKey(2) &"' AND PRTNO='" & DSPKEY(3) & "' and fIXcod='" & extDB(i) & "'" ,conn,3,3
           If rs.Eof Or rs.Bof Then
              rs.AddNew
              rs("COMQ1")=dspKey(0)
              rs("LINEQ1")=dspKey(1)
              rs("faqno")=dspKey(2)
              rs("PRTno")=dspKey(3)
              rs("fIXcod")=extDB(i)          
           End If
           rs.Update
           rs.Close
        end if
    Next
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
End Sub
' --------------------------------------------------------------------------------------------  
%>
<!-- #include file="RTGetUserRight.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->