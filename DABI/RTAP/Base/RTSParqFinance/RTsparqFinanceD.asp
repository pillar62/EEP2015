
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
                 ' 當程式為社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtsparqinspect/RTsparqinspectd.asp")
                     if i<>0 then rs.Fields(i).Value=dspKey(i)         
                 case else
                      rs.Fields(i).Value=dspKey(i)
               end select
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
                     if i<>0 then rs.Fields(i).Value=dspKey(i)                  
               '  case else
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
  '  if accessmode ="A" then    
    runpgm=Request.ServerVariables("PATH_INFO")
    if ucase(runpgm)=ucase("/webap/rtap/base/rtsparqinspect/RTsparqinspectd.asp") then
       rs.open "select max(SEQ1) AS SEQ1 from rtSPARQADSLINSPECT ",conn
       if not rs.eof and len(trim(RS("SEQ1"))) > 0 then
          dspkey(0)=rs("SEQ1")
       end if
       rs.close
    end if
  '  end if
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
  title="未送件客戶資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT SEQ1, CUSID, CUSNC, CASESOURCE, COMN, TELNO, MOBILE, CUTID, " _
         &"TOWNSHIP, ADDRESS,RZONE, RCVDAT, INSPECTDAT, REPLYDAT, INSPECTMAN, " _
         &"INSPECTCONSIGNEE, INSEPCTRESULT, DROPDAT,TRANSCODE,reason " _
         &"FROM RTSparqADSLINSPECT  WHERE SEQ1=0 "
  sqlList="SELECT SEQ1, CUSID, CUSNC, CASESOURCE, COMN, TELNO, MOBILE, CUTID, " _
         &"TOWNSHIP, ADDRESS,RZONE, RCVDAT, INSPECTDAT, REPLYDAT, INSPECTMAN, " _
         &"INSPECTCONSIGNEE, INSEPCTRESULT, DROPDAT,TRANSCODE,reason " _
         &"FROM RTSparqADSLINSPECT WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  userDefineSave=""  
  userdefineactivex="Yes"    
  extDBField=0
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    if len(trim(dspkey(0))) = 0 then dspkey(0)=0
  '  if len(trim(dspkey(6))) = 0 then dspkey(6)=0
  '  if len(trim(dspkey(14))) = 0 then dspkey(14)=0    
    If len(trim(dspKey(1))) < 1 Then
       formValid=False
       message="請輸入用戶身份證號)經銷商統一編號"
    elseif len(trim(dspKey(2))) < 1 Then
       formValid=False
       message="請輸入用戶名稱"
    elseif len(trim(dspKey(3))) < 1 Then
       formValid=False
       message="請輸入用戶來源類別"
    elseif len(trim(dspKey(7))) < 1  or len(trim(dspKey(8))) < 1 or len(trim(dspKey(9))) < 1  Then
       formValid=False
       message="請輸入完整用戶裝機地址"
    elseif not Isdate(dspkey(11))  Then
       formValid=False
       message="請輸入收件日期"
    End If        
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
   End Sub 
   Sub SrSelonclick()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="KEY" & clickid
       prog="RTFaQFinishUsrx.asp"
       CUTID=document.all("key7").value
       town=document.all("key8").value
       'showopt="Y;Y;Y;Y"表示對話方塊中要顯示的項目(業務工程師;客服人員;技術部;廠商)
       if clickkey="KEY14" then
          showopt="Y;N;N;N" & ";" & cutid & ";" & town
       else
          showopt="N;N;N;N;;"
       end if
       prog=prog & "?showopt=" & showopt
       FUsr=Window.showModalDialog(prog,"Dialog","dialogWidth:620px;dialogHeight:480px;")  
      'Fusrid(0)=維修人員工號或廠商代號  fusrid(1)=只為於上一畫面中秀出中文名稱(無其它作用) fusrid(2)="1"為業務"2"為技術"3"為廠商"4"為客服(作為資料存放於何欄位之依據)
       IF FUSR <> "" THEN
       FUsrID=Split(Fusr,";")    
       if Fusrid(3) ="Y" then
         '廠商取8位,其餘取6位   
         if Fusrid(2)="3"  then 
            document.all(clickkey).value =  left(Fusrid(0),8)
         else
            document.all(clickkey).value =  left(Fusrid(0),6)
         end if 
       End if
       END IF
    '   Set winP=window.Opener
    '   Set docP=winP.document
    '   docP.all("keyform").Submit
    '   winP.focus()             
    '   window.close
   End Sub    
   Sub Srcounty8onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY7").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key8").value =  trim(Fusrid(0))
          document.all("key10").value =  trim(Fusrid(1))
       End if       
       end if
   End Sub    
   Sub SrClear()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="C" & clickid
       clearkey="key" & clickid       
       if len(trim(document.all(clearkey).value)) <> 0 then
          document.all(clearkey).value =  ""
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
   </Script>
<%   
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveX() %>
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"    codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
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
<tr><td width="15%" class=dataListSearch>資料範圍</td>
    <td width="85%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="15%" class=dataListHead>建檔序號</td><td width="85%" bgcolor=silver>
           <input class=dataListDATA READONLY type="text" name="key0" <%=keyprotect%> size="10" 
           value="<%=dspKey(0)%>" maxlength="8" ></td>

      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
If dspKey(17)="Y" Then
   fieldPa=" class=""dataListData"" readonly "
ELSE
   FIELDPA=""
END IF
'-----EXTDB DATA RETRIVE
DIM conn,rs,dsn,sql
SET conn=server.CreateObject("ADODB.Connection")
set rs=server.CreateObject("ADODB.recordset")
DSN="DSN=RTLIB"
conn.Open dsn
%>
<table border="1" width="100%" cellspacing="0" cellpadding="0">
  <tr>
   <td width="13%" bgcolor="#008080"><font color="#FFFFFF">身份證號(統編)</font></td>
    <td width="25%" colspan=3 bgcolor="#C0C0C0">
     <input class=dataListEntry name="KEY1" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(1)%>" ID="Text1"></td>
    　</td>
  </tr>
  <tr>
    <td width="10%" bgcolor="#008080"><font color="#FFFFFF">用戶名稱</font></td>
    <td width="55%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="KEY2" <%=dataprotect%> maxlength=40 size=40 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(2)%>"></td>
    <td width="10%" bgcolor="#008080"><font color="#FFFFFF">用戶來源</font></td>
    <td width="20%" bgcolor="#C0C0C0">
<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='E9' " 
       If len(trim(dspkey(3))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='E9' AND CODE='" & dspkey(3) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(3) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>                  
   <select size="1" name="key3" <%=fieldpa%><%=dataProtect%> class="dataListEntry" ID="Select1">                                            
        <%=s%>
   </select>
    　</td>
  </tr>
  <tr>
    <td width="10%" bgcolor="#008080"><font color="#FFFFFF">社區名稱</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key4"  <%=dataprotect%> maxlength=30 size=40 style="TEXT-ALIGN: left" value
            ="<%=dspkey(4)%>">　</td>
    <td width="10%" bgcolor="#008080"><font color="#FFFFFF">連絡電話</font></td>
    <td width="20%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key5" <%=dataprotect%> maxlength=15 size=15 style="TEXT-ALIGN: left" value
            ="<%=dspkey(5)%>">　</td>
  </tr>
  <tr>
    <td width="10%" bgcolor="#008080"><font color="#FFFFFF">行動電話</font></td>
    <td width="45%" COLSPAN=3 bgcolor="#C0C0C0">
     <input class=dataListEntry name="key6" <%=dataprotect%> maxlength=15 size=15 style="TEXT-ALIGN: left" value
            ="<%=dspkey(6)%>">　</td>
  </tr>
  <tr>
    <td width="10%" bgcolor="#008080"><font color="#FFFFFF">裝機地址</font></td>
    <td width="45%" bgcolor="#C0C0C0">
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(7))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市別)</option>"       
       SXX8=" onclick=""Srcounty8onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(7) & "' " 
       SXX8=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(7) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
         <select size="1" name="key7"<%=fieldpa%><%=dataProtect%> size="1" class="dataListEntry" ID="Select2"><%=s%></select>
        <input type="text" name="key8" size="8" value="<%=dspkey(8)%>" maxlength="10" readonly <%=fieldpa%><%=dataProtect%> class="dataListEntry" ID="Text2"><font size=2>(鄉鎮市區)                 
         <input type="button" id="B8"  name="B8"   width="100%" style="Z-INDEX: 1"  value="..." <%=SXX8%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C10"  name="C10"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
        <input type="text" name="key9" size="30" value="<%=dspkey(9)%>" maxlength="60" <%=fieldpa%><%=dataProtect%> class="dataListEntry" ID="Text3"></td>                                 
        <td width="8%" class="dataListHead" height="25">郵遞區號</td>                                 
        <td width="16%" height="20" bgcolor="silver"><input type="text" name="key10" size="5" value="<%=dspkey(10)%>" maxlength="5" <%=fieldpa%><%=dataProtect%> class="dataListdata" readonly ID="Text4"></td>                        
  </tr>
  <tr>
    <td width="10%" bgcolor="#008080"><font color="#FFFFFF">收件日期</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key11" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(11)%>">
    <input type="button" id="B11"  name="B11" height="70%" width="70%" style="Z-INDEX: 1" value="..." Onclick="Srbtnonclick">            
    　</td>
    <td width="10%" bgcolor="#008080"><font color="#FFFFFF">派堪日期</font>　</td>
    <td width="20%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="key12" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(12)%>">
    <input type="button" id="B12"  name="B12" height="70%" width="70%" style="Z-INDEX: 1" value="..." Onclick="Srbtnonclick">            
            　</td>
  </tr>  
  <tr>
      <td width="10%" bgcolor="#008080"><font color="#FFFFFF">應回覆日期</font>　</td>
    <td width="25%" bgcolor="#C0C0C0" >
    <%
    if len(trim(dspkey(12)))=0 then 
       X=""
    else
       X=dateadd("d",7,dspkey(12))
    END IF
    %>
     <input class=dataListdata name="otherx" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=X%>">　</td> 
      <td width="10%" bgcolor="#008080"><font color="#FFFFFF">實際回覆日</font>　</td>
    <td width="20%" bgcolor="#C0C0C0" >
     <input class=dataListEntry name="key13" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(13)%>">
    <input type="button" id="B13"  name="B13" height="70%" width="70%" style="Z-INDEX: 1" value="..." Onclick="Srbtnonclick">            
            　</td>            
  </tr>          
  <tr>
    <td width="10%" bgcolor="#008080"><font color="#FFFFFF">派堪工程師</font></td>
    <td width="45%" bgcolor="#C0C0C0">
      <input type="text" name="key14" size="6" maxlength="50" readonly value="<%=dspkey(14)%>" <%=fieldpa%> class="dataListEntry" ID="Text6">
     <input type="button" id="B14"  name="B14"   width="100%" style="Z-INDEX: 1"  value="..." onclick="SrSelOnClick"  >
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C14"  name="C14"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" ></td>
    <td width="10%" bgcolor="#008080"><font color="#FFFFFF">派堪經銷商</font></td>
    <td width="20%" bgcolor="#C0C0C0">
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
       sql="SELECT RTConsignee.CUSID, RTObj.SHORTNC FROM RTConsignee LEFT OUTER JOIN " _
          &"RTObj ON RTConsignee.CUSID = RTObj.CUSID " 
       If len(trim(dspkey(15))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(經銷商)</option>"       
    Else
       sql="SELECT RTConsignee.CUSID, RTObj.SHORTNC FROM RTConsignee LEFT OUTER JOIN " _
          &"RTObj ON RTConsignee.CUSID = RTObj.CUSID " 
       s=s &"<option value=""" &"""" &sx &">(經銷商)</option>"           
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cusid")=dspkey(15) Then sx=" selected "
       s=s &"<option value=""" &rs("Cusid") &"""" &sx &">" &rs("shortnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
         <select size="1" name="key15"<%=fieldpa%><%=dataProtect%> size="1" class="dataListEntry" ID="Select3"><%=s%></select>
      </td>
  </tr>
  <tr>
    <td width="10%" bgcolor="#008080"><font color="#FFFFFF">回覆結果</font></td>
    <td width="35%" bgcolor="#C0C0C0">
<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='F1' " 
       If len(trim(dspkey(16))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='F1' AND CODE='" & dspkey(16) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(16) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>                  
   <select size="1" name="key16" <%=fieldpa%><%=dataProtect%> class="dataListEntry" ID="Select4">                                            
        <%=s%>
   </select>
    　</td>
    <td width="10%" bgcolor="#008080"><font color="#FFFFFF">作廢日期</font></td>
    <td width="20%" bgcolor="#C0C0C0">
    <input class=dataListEntry  name="key17" readOnly size=10 maxlength=10 style="TEXT-ALIGN: left "
            value="<%=dspkey(17)%>" ID="Text5">
    <input type="button" id="B17"  name="B17" height="70%" width="70%" style="Z-INDEX: 1" value="..." Onclick="Srbtnonclick">            
            </td>    
  </tr>
  <tr>
      <td width="10%" bgcolor="#008080"><font color="#FFFFFF">已送件記錄</font>　</td>
    <td width="25%" COLSPAN=3 bgcolor="#C0C0C0" >
     <input class=dataListdata name="KEY18" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(18)%>" ID="Text7">　</td> 
  </tr>     
  <tr>
    <td width="10%" bgcolor="#008080" colspan=4 align="center"><font color="#FFFFFF">不可建置(待回覆)原因</font>　</td>
  </tr>
  <tr>            
    <td width="100%" colspan="4" bgcolor="#c0c0c0"  height="117">
      <p align="center"><font ><TEXTAREA cols="95%" name="KEY19" rows=5 class="dataListENTRY" r ID="Textarea1"><%=DSPKEY(19)%></TEXTAREA></font></p>
    </td> 
  </tr>       
</table>
<% conn.close
   set rs=nothing
   set conn=nothing
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrSaveExtDB(Smode)
End Sub
' -------------------------------------------------------------------------------------------- 
%>
