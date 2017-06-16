
<%@ Transaction = required %>

<!-- #include virtual="/WebUtilityV4/DBAUDI/cType.inc" -->
<!-- #include virtual="/WebUtilityV4/DBAUDI/dataList.inc" -->
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
   ' response.Write SQL
    rs.Open sql,conn,3,3
    If rs.Eof Or rs.Bof Then
       If accessMode="A" Then
          rs.AddNew
          For i = 0 To numberOfField-1
              sType=Right("000" &Cstr(aryKeyType(i)),3)
              If Instr(cTypeDate,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=Null
              If Instr(cTypeAuto,sType) > 0 Or (dspKey(i)=-1 And i<numberOfKey) Then
              Else
                     if i=3  then
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(entryno) AS entryno from HBCALLOUTH where  COMQ1=" & dspkey(0) & " and comtype='" & dspkey(1) & "' and cusid='" & dspkey(2) & "' "
                       ' response.Write SQLSTR2
                        rsc.open sqlstr2,conn
                        if len(rsc("entryno")) > 0 then
                           dspkey(i)=rsc("entryno") + 1
                        else
                           dspkey(i)=1
                        end if
                        rsc.close
                     end if                     
                     rs.Fields(i).Value=dspKey(i)
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
             ' select case ucase(runpgm)   
                 ' 當程式為經銷商基本資料維護作業時
               '  case ucase("/webap/rtap/base/rconsignee/RTconsigneed.asp")
               '      if i<>3 then rs.Fields(i).Value=dspKey(i)                        
               '  case else
             '  response.write "I" & i &"=" & DSPKEY(I) & "<BR>"
              if i<> 0 then rs.Fields(i).Value=dspKey(i)
             '  end select
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
    if ucase(runpgm)=ucase("/webap/rtap/base/rtconsignee/RTconsigneed.asp") then
       rs.open "select max(cusno) AS comq1 from rtconsignee",conn
       if not rs.eof and len(trim(cusno)) > 0 then
          dspkey(15)=rs("cusno")
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
<input type="text" name="sw" value="<%=sw%>" style="display:none;">
<input type="text" name="reNew" value="N" style="display:none;">
<input type="text" name="rwCnt" value="<%=rwCnt%>" style="display:none;">
<input type="text" name="accessMode" value="<%=accessMode%>" style="display:none;">
<table width="100%">
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
<!-- #include virtual="/Webap/include/employeeref.inc" -->

<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=1
  title="退租/欠拆/復機資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT SERNO, CUSID, ENTRYNO, CASETYPE, HBNO, STATUS, CUSNC, TEL, ADDR, " _
             &"APPLYDAT, CUSNO, ACTDROP, ACTDROPUSR, CALLOUTFLAG, PRTNO, PRTUSR, UPDDATABASE, " _
             &"OVERDUEDROP, OVERDUETNSDAT, AGREEPAYDAT,actpaydat,actdropconsignee " _
             &"FROM HBCustDrop WHERE SERNO=0 " 
  sqlList="SELECT SERNO, CUSID, ENTRYNO, CASETYPE, HBNO, STATUS, CUSNC, TEL, ADDR, " _
             &"APPLYDAT, CUSNO, ACTDROP, ACTDROPUSR, CALLOUTFLAG, PRTNO, PRTUSR, UPDDATABASE, " _
             &"OVERDUEDROP, OVERDUETNSDAT, AGREEPAYDAT,actpaydat,actdropconsignee " _
             &"FROM HBCustDrop WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  userDefineSave="Yes"  
  userdefineactivex="Yes"    
  extDBField=0
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    IF LEN(TRIM(DSPKEY(21)))=0 THEN DSPKEY(21)=""

    if dspkey(20) > dspkey(19) AND LEN(TRIM(DSPKEY(20))) > 0 AND LEN(TRIM(DSPKEY(19))) > 0 then 
       formValid=False
       message="實際繳款日不可小於同意繳款日"    
    END IF
    IF DSPKEY(5) <> "欠拆" THEN
       IF LEN(DSPKEY(19)) > 0 OR LEN(DSPKEY(20)) > 0 THEN
          FormValid=False
          message="非欠拆客戶，同意繳款日及實際繳款日必須空白"    
       END IF
    END IF
    IF DSPKEY(5) = "欠拆" THEN
       IF LEN(DSPKEY(17)) = 0 AND ( LEN(DSPKEY(11)) > 0 OR LEN(DSPKEY(12)) > 0 ) THEN
          FormValid=False
          message="欠拆客戶，必須轉拆機戶始可輸入實際拆機人員及拆機日期"    
       END IF
    END IF    
    IF (LEN(DSPKEY(11)) > 0 AND LEN(DSPKEY(12))=0 and LEN(DSPKEY(21))=0) OR ((LEN(DSPKEY(12)) > 0 or LEN(DSPKEY(12)) > 0 ) AND LEN(DSPKEY(11))=0) THEN
          FormValid=False
          message="實際拆機人員及拆機日期並須同時存在或同時空白"       
    END IF
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()%>
<p>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="20%" class=dataListHead>序號</td>
       <td width="30%" bgcolor=silver>
           <input class=dataListDATA type="text" name="key0" READONLY <%=keyprotect%> size="5" 
           value="<%=dspKey(0)%>" maxlength="5" ></td>
        <td width="20%" class=dataListHead>客戶代號</td>           
       <td width="30%" bgcolor=silver>
           <input class=dataListDATA type="text" name="key1" READONLY <%=keyprotect%> size="10" 
           value="<%=dspKey(1)%>" maxlength="10" ID="Text1"></td>           
       </tr>
        <td width="20%" class=dataListHead>單次</td>           
       <td width="30%" bgcolor=silver>
           <input class=dataListDATA type="text" name="key2" READONLY <%=keyprotect%> size="5" 
           value="<%=dspKey(2)%>" maxlength="5" ID="Text11"></td>           
        <td width="20%" class=dataListHead>方案</td>           
       <td width="30%" bgcolor=silver>
    <%
    COMN=""
    if DSPKEY(3)="1" THEN COMTYPE="中華599"
    IF DSPKEY(3)="2" THEN COMTYPE="中華399"
    IF DSPKEY(3)="3" THEN COMTYPE="速博399"
    %>
       <input class=dataListDATA type="text" name="key3" READONLY <%=keyprotect%> size="7" 
           value="<%=COMTYPE%>" maxlength="5" ID="Text21"></td>           
       </tr>              
      </table>
<%
End Sub
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
   Sub Sr12salesonclick()
       prog="RTGetsalesD.asp"
      ' prog=prog & "?KEY=" & document.all("KEY9").VALUE & ";" & document.all("KEY10").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key12").value =  trim(Fusrid(0))
       End if       
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
Sub SrGetUserDefineData()
    logonid=session("userid")
%>
<table border="1" width="100%" cellspacing="0" cellpadding="0">
  <tr>
    <td width="20%" bgcolor="#008080" class="dataListsearch"><font color="#FFFFFF">拆機單號</font></td>
    <td width="30%" bgcolor="#C0C0C0" >
     <input class=datalistDATA name="KEY14" READONLY maxlength=11 size=11 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(14)%>" ID="Text19">    
    </td>
    <td width="20%" bgcolor="#008080"><font color="#FFFFFF">列印人員</font></td>
    <td width="30%" bgcolor="#C0C0C0" >
     <input class=datalistDATA name="KEY15" READONLY maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(15)%>" ID="Text20">    
    </td>    
  </tr>                
  <tr>

    <td width="10%" bgcolor="#008080"><font color="#FFFFFF">客戶名稱</font></td>
    <td width="30%" bgcolor="#C0C0C0" COLSPAN=3>
     <input class=datalistDATA name="KEY6" READONLY maxlength=30 size=30 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(6)%>" ID="Text3">
       </td>
  </tr>
  <tr>
    <td width="20%" bgcolor="#008080"><font color="#FFFFFF">社區HBNO</font></td>
    <td width="30%" bgcolor="#C0C0C0">
     <input class=datalistDATA name="KEY4" READONLY maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(4)%>" ID="Text4">    
    </td>
    <td width="20%" bgcolor="#008080"><font color="#FFFFFF">客戶HNNO</font></td>
    <td width="30%" bgcolor="#C0C0C0" >
     <input class=datalistDATA name="KEY10" READONLY maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(10)%>" ID="Text5">    
    </td>    
  </tr>    
  <tr>
    <td width="20%" bgcolor="#008080"><font color="#FFFFFF">退/欠/復機日期</font></td>
    <td width="30%" bgcolor="#C0C0C0" >
     <input class=datalistDATA name="KEY9" READONLY maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(9)%>" ID="Text6">    
    </td>
    <td width="20%" bgcolor="#008080"><font color="#FFFFFF">狀態</font></td>
    <td width="30%" bgcolor="#C0C0C0">
     <input class=datalistDATA name="KEY5" READONLY maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(5)%>" ID="Text7">    
    </td>    
  </tr>      
  <tr>
    <td width="20%" bgcolor="#008080"><font color="#FFFFFF">裝機地址</font></td>
    <td width="30%" bgcolor="#C0C0C0">
     <input class=datalistDATA name="KEY8" READONLY maxlength=50 size=50 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(8)%>" ID="Text8">    
    </td>
    <td width="20%" bgcolor="#008080"><font color="#FFFFFF">連絡電話</font></td>
    <td width="30%" bgcolor="#C0C0C0" >
     <input class=datalistentry name="KEY7"  maxlength=15 size=15 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(7)%>" ID="Text9">    
    </td>    
  </tr>        
  <tr>
    <td width="20%" bgcolor="#008080"><font color="#FFFFFF">已更新MIS</font></td>
    <td width="30%" bgcolor="#C0C0C0" colspan=3>
     <input class=datalistDATA name="KEY16" READONLY maxlength=3 size=3 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(16)%>" ID="Text10">    
    </td>
    <td width="20%" bgcolor="#008080" STYLE="DISPLAY:NONE"><font color="#FFFFFF">已欠拆CALLOUT</font></td>
    <td width="30%" bgcolor="#C0C0C0" STYLE="DISPLAY:NONE">
     <input class=datalistDATA name="KEY13" READONLY maxlength=3 size=3 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(13)%>" ID="Text12">    
    </td>    
  </tr>          
  <tr>
    <td width="20%" bgcolor="#008080"><font color="#FFFFFF">同意繳款起日</font></td>
    <td width="30%" bgcolor="#C0C0C0">
     <input class=datalistentry name="KEY19" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(19)%>" ID="Text17">    
    <input type="button" id="B19"  name="B19" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除"  ID="C19" name="C19"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
            
    </td>
    <td width="20%" bgcolor="#008080"><font color="#FFFFFF">實際繳款日</font></td>
    <td width="30%" bgcolor="#C0C0C0" >
     <input class=datalistentry name="KEY20" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(20)%>" ID="Text18">    
    <input type="button" id="B20"  name="B20" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除"  ID="C20" name="C20"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
            
    </td>    
  </tr>                  
  <tr>
    <td width="20%" bgcolor="#008080"><font color="#FFFFFF">欠拆轉拆機</font></td>
    <td width="30%" bgcolor="#C0C0C0" >
     <input class=datalistDATA name="KEY17" READONLY maxlength=3 size=3 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(17)%>" ID="Text15">    
    </td>
    <td width="20%" bgcolor="#008080"><font color="#FFFFFF">欠拆轉拆機日</font></td>
    <td width="30%" bgcolor="#C0C0C0">
     <input class=datalistDATA name="KEY18" READONLY maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(18)%>" ID="Text16">    
    </td>    
  </tr>              
  <tr>
    <td width="20%" bgcolor="#008080" class="dataListsearch"><font color="#FFFFFF">實際拆機(復裝)日</font></td>
    <td width="30%" bgcolor="#C0C0C0">
     <input class=datalistentry name="KEY11" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(11)%>" ID="Text13">    
    <input type="button" id="B11"  name="B11" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除"  ID="C11" name="C11"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
            
    </td>
    <td width="20%" bgcolor="#008080" class="dataListsearch"><font color="#FFFFFF">實際拆機(復裝)員</font></td>
    <td width="30%" bgcolor="#C0C0C0">
     <input class=datalistentry name="KEY12" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(12)%>" ID="Text14">    
     <input type="button" id="B12"  name="B12"   width="100%" style="Z-INDEX: 1"  value="..." onclick="Sr12salesonclick()"  >
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C12"  name="C12"   style="Z-INDEX: 1" onclick="srclear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >            
    </td>    
  </tr>            
  <tr>
    <td width="30%" bgcolor="#C0C0C0" colspan=2>&nbsp; </td>
    <td width="20%" bgcolor="#008080" class="dataListsearch"><font color="#FFFFFF">實際拆機廠商</font></td>
    <td width="30%" bgcolor="#C0C0C0">
    <%
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN   
    SQL="SELECT * FROM  RTObj INNER JOIN RTObjLink ON RTObj.CUSID = RTObjLink.CUSID WHERE (RTObjLink.CUSTYID = '04') " 
    rs.Open sql,conn
    s="<option value="""" >(廠商)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(21) Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close       %>
   <select size="1" name="key21"  class="dataListEntry" ID="Select2">                                                                  
        <%=s%>
   </select>    
    </td>    
  </tr>             

</table>
<%

End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrSaveExtDB(Smode)
End Sub
' -------------------------------------------------------------------------------------------- 
%>
