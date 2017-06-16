
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
                 case ucase("/webap/rtap/base/rtconsignee/RTConsigneed.asp")
                     if i=15  then
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(cusno) AS cusno from rtconsignee "
                        rsc.open sqlstr2,conn
                        tempyy=cstr(cint(datepart("yyyy",now()))-1911)
                        if len(trim(rsc("cusno"))) > 0 then
                           temp15="SP" & tempyy & right("000" & cstr(cint(right(rsc("cusno"),3))+1),3)
                        else
                           temp15="SP" & tempyy & "001"
                        end if
                        dspkey(15)=temp15
                        rs.Fields(i).Value=dspKey(15)
                     else
                        rs.Fields(i).Value=dspKey(i)
                     end if                                       
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
               '      if i<>3 then rs.Fields(i).Value=dspKey(i)                        
               '  case else
                     rs.Fields(i).Value=dspKey(i)
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
  title="經銷商基本資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT CUSID,TEL,FAX,email,RURL,STAFFCNT,TEAM,boss," _
             &"eusr,edat,uusr,udat,CONT,CONTTEL,stdfee,cusno,ctype,EBTCUSID,EBTAGENTID,EBTRETAILSALEID " _
             &"FROM RTConsignee WHERE cusid='*' "
  sqlList="SELECT CUSID,TEL,FAX,email,RURL,STAFFCNT,TEAM,boss," _
             &"eusr,edat,uusr,udat,CONT,CONTTEL,stdfee,cusno,ctype,EBTCUSID,EBTAGENTID,EBTRETAILSALEID  " _
             &"FROM RTConsignee WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  userDefineSave="Yes"  
  extDBField=14
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    if len(trim(dspkey(5))) = 0 then dspkey(5)=0
    if len(trim(dspkey(6))) = 0 then dspkey(6)=0
    if len(trim(dspkey(14))) = 0 then dspkey(14)=0    
    IF LEN(TRIM(DSPKEY(18)))> 0 OR  LEN(TRIM(DSPKEY(19)))> 0 THEN
       DSPKEY(17)="00" & DSPKEY(18) & DSPKEY(19)
    END IF
    If len(trim(dspKey(0))) < 1 Then
       formValid=False
       message="請輸入經銷商統一編號"
    elseif len(trim(extdb(0))) < 1 Then
       formValid=False
       message="請輸入經銷商名稱"
    elseif len(trim(extdb(1))) < 1 Then
       formValid=False
       message="請輸入經銷商簡稱"
    elseif not IsNumeric(dspkey(6))  Then
       formValid=False
       message="公司人數不正確"
    elseif not IsNumeric(dspkey(5))  Then
       formValid=False
       message="施工組別數不正確"
    elseif not IsNumeric(dspkey(14))  Then
       formValid=False
       message="標準施工費不正確"       
    elseif LEN(TRIM(dspkey(18))) > 0 AND LEN(TRIM(dspkey(18))) <> 4  Then
       formValid=False
       message="ISP代理商代碼長度必須為四位"           
    elseif LEN(TRIM(dspkey(19))) > 0 AND LEN(TRIM(dspkey(19))) <> 4  Then
       formValid=False
       message="ISP零售商代碼長度必須為四位"              
    End If        
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="21%" class=dataListHead>經銷商統編</td><td width="29%" bgcolor=silver>
           <input class=dataListEntry type="text" name="key0" <%=keyprotect%> size="10" 
           value="<%=dspKey(0)%>" maxlength="10" >(公司統一編號)</td>
        <td width="15%" class=dataListHead>經銷商代碼</td><td width="25%" bgcolor=silver>
           <input class=dataListdata type="text" name="key15" <%=keyprotect%> size="10" 
           value="<%=dspKey(15)%>" maxlength="8"  readonly></td>           
       </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(8))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                EUsrNc=V(1) 
                dspkey(8)=V(0)
                extdb(10)=v(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(8))
                V=split(rtnvalue,";")      
                EUsrNc=V(1)
        End if  
       dspkey(9)=datevalue(now())
       extdb(11)=datevalue(now())
    else
        if len(trim(dspkey(10))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                UUsrNc=V(1)
                DSpkey(10)=V(0)
                extdb(12)=v(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(10))
                V=split(rtnvalue,";")      
                UUsrNc=V(1)
        End if         
        Call SrGetEmployeeRef(rtnvalue,2,dspkey(8))
             V=split(rtnvalue,";")      
             EUsrNc=V(1)
        dspkey(11)=datevalue(now())
        extdb(13)=datevalue(now())
    end if

'-----EXTDB DATA RETRIVE
DIM conn,rs,dsn,sql
SET conn=server.CreateObject("ADODB.Connection")
set rs=server.CreateObject("ADODB.recordset")
DSN="DSN=RTLIB"
SQL="SELECT CUSNC,SHORTNC,CUTID1,TOWNSHIP1,RADDR1,RZONE1,CUTID2," _ 
   &"TOWNSHIP2,RADDR2,RZONE2,EUSR,EDAT,UUSR,UDAT " _
   &"FROM RTobj where CUSID ='" & dspkey(0) & "'"
conn.Open dsn
rs.Open sql,conn
if not rs.EOF then
   If sw="E" Or (accessMode="A" And sw="") Then 
      if sw="E" then extdb(13)=datevalue(now())
      if sw="" and accessMode="A" then extdb(11)=datevalue(now())
   else
      extdb(0)=rs("cusnc")
      extdb(1)=rs("shortnc")
      extdb(2)=rs("cutid1")
      extdb(3)=rs("township1")
      extdb(4)=rs("raddr1")
      extdb(5)=rs("rzone1")
      extdb(6)=rs("cutid2")
      extdb(7)=rs("township2")
      extdb(8)=rs("raddr2")
      extdb(9)=rs("rzone2")
      extdb(10)=rs("eusr")
      if len(trim(rs("edat"))) > 0 then extdb(11)=datevalue(rs("edat"))
      extdb(12)=rs("uusr")
      if len(trim(rs("udat"))) > 0 then extdb(13)=datevalue(rs("udat")) 
   end if
else
end if
rs.close
%>
<table border="1" width="100%" cellspacing="0" cellpadding="0">
  <tr>
   <td width="15%" bgcolor="#008080"><font color="#FFFFFF">經銷類型</font></td>
    <td width="25%" bgcolor="#C0C0C0">
 <% s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT CODE,CODENC FROM RTCode where kind='E1' ORDER BY CODE "
       If len(trim(dspkey(16))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
    Else
       sql="SELECT code,codenc FROM RTcode WHERE KIND='E1' AND CODE='" &dspkey(16) &"' "
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("code")=dspkey(16) Then sx=" selected "
       s=s &"<option value=""" &rs("code") &"""" &sx &">" &rs("Codenc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>
    <select class=dataListEntry name="key16" <%=dataProtect%> size="1" 
       style="text-align:left;" maxlength="8"><%=s%></select>  
    　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">公司名稱</font></td>
    <td width="45%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="ext0" <%=dataprotect%> maxlength=50 size=40 style="TEXT-ALIGN: left" value
            ="<%=EXTDB(0)%>"></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">簡稱</font></td>
    <td width="25%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="ext1" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=EXTDB(1)%>"></td>
    　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">公司電話</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key1"  <%=dataprotect%> maxlength=15 size=15 style="TEXT-ALIGN: left" value
            ="<%=dspkey(1)%>">　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">傳真電話</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key2" <%=dataprotect%> maxlength=15 size=15 style="TEXT-ALIGN: left" value
            ="<%=dspkey(2)%>">　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">公司網址</font></td>
    <td width="45%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="key4" <%=dataprotect%> maxlength=30 size=30 style="TEXT-ALIGN: left" value
            ="<%=dspkey(4)%>">　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">電子郵件</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key3" <%=dataprotect%> maxlength=30 size=25 style="TEXT-ALIGN: left" value
            ="<%=dspkey(3)%>">　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">負責人</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key7" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(7)%>">
    　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">公司人數</font>　</td>
    <td width="25%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="key6" <%=dataprotect%> maxlength=5 size=5 style="TEXT-ALIGN: left" value
            ="<%=dspkey(6)%>">　</td>
  </tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">聯絡人員</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key12" <%=dataprotect%> maxlength=12 size=12 style="TEXT-ALIGN: left" value
            ="<%=dspkey(12)%>">
    　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">聯絡電話</font>　</td>
    <td width="25%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="key13" <%=dataprotect%> maxlength=15 size=15 style="TEXT-ALIGN: left" value
            ="<%=dspkey(13)%>">　</td>
  </tr>  
  <tr>
      <td width="15%" bgcolor="#008080"><font color="#FFFFFF">施工組別數</font>　</td>
    <td width="25%" bgcolor="#C0C0C0" >
     <input class=dataListEntry name="key5" <%=dataprotect%> maxlength=5 size=5 style="TEXT-ALIGN: left" value
            ="<%=dspkey(5)%>">　</td>
      <td width="15%" bgcolor="#008080"><font color="#FFFFFF">標準施工費</font>　</td>
    <td width="45%" bgcolor="#C0C0C0" colspan="3">
     <input class=dataListEntry name="key14" <%=dataprotect%> maxlength=6 size=7 style="TEXT-ALIGN: left" value
            ="<%=dspkey(14)%>">　</td>            
  </tr>          
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">戶籍地址</font></td>
    <td width="45%" bgcolor="#C0C0C0">
 <% s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT * FROM RTCounty ORDER BY CutID "
       If len(trim(extDB(2))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
    Else
       sql="SELECT * FROM RTCounty WHERE CutID='" &extdb(2) &"' "
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CutID")=extDB(2) Then sx=" selected "
       s=s &"<option value=""" &rs("CutID") &"""" &sx &">" &rs("CutNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>
    <select class=dataListEntry name="ext2" <%=dataProtect%> size="1" 
                onChange="SrRenew()"
               style="text-align:left;" maxlength="8"><%=s%></select>    
<%  If sw="E" Then
       rs.Open "SELECT * FROM RTCtyTown WHERE CutID='" &extDB(2) &"' ORDER BY TownShip ",conn
       s=""
       sx=" selected "
       If len(trim(extDB(3))) < 1 Then sx=" selected "
       s=s & "<option value=""""" & sx & "></option>"
       Do While Not rs.Eof
          If rs("TownShip")=extDB(3) Then sx=" selected "
          s=s &"<option value=""" &rs("TownShip") &"""" &sx &">" &rs("TownShip") &"</option>"
          rs.MoveNext
          sx=""
       Loop
       rs.Close
    Else
       s="<option value=""" &extDB(3) &""" selected>" &extDB(3) &"</option>"
    End If %>
    <select class=dataListEntry name="ext3" <%=dataProtect%> size="1" 
               style="text-align:left;" maxlength="8"><%=s%></select>   
    <input class=dataListEntry name="ext4" <%=dataprotect%> maxlength=40 size=25 style="TEXT-ALIGN: left" value
            ="<%=EXTDB(4)%>"></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">郵遞區號</font></td>
    <td width="25%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="ext5" <%=dataprotect%> maxlength=5 size=5 style="TEXT-ALIGN: left" value
            ="<%=EXTDB(5)%>">　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">通訊地址</font></td>
    <td width="45%" bgcolor="#C0C0C0">
 <% s=""
    sx=" selected " 
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)Then 
       sql="SELECT * FROM RTCounty ORDER BY CutID "
       If len(trim(extDB(6))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if            
    Else
       sql="SELECT * FROM RTCounty WHERE CutID='" &extdb(6) &"' "
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CutID")=extDB(6) Then sx=" selected "
       s=s &"<option value=""" &rs("CutID") &"""" &sx &">" &rs("CutNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>
    <select class=dataListEntry name="ext6" <%=dataProtect%> size="1" 
                onChange="SrRenew()"
               style="text-align:left;" maxlength="8"><%=s%></select>
<%  If sw="E" Then
       rs.Open "SELECT * FROM RTCtyTown WHERE CutID='" &extDB(6) &"' ORDER BY TownShip ",conn
       s=""
       sx=" selected "
       If len(trim(extDB(7))) < 1 Then sx=" selected "
       s=s & "<option value=""""" & sx & "></option>"       
       Do While Not rs.Eof
          If rs("TownShip")=extDB(7) Then sx=" selected "
          s=s &"<option value=""" &rs("TownShip") &"""" &sx &">" &rs("TownShip") &"</option>"
          rs.MoveNext
          sx=""
       Loop
       rs.Close
    Else
       s="<option value=""" &extDB(7) &""" selected>" &extDB(7) &"</option>"
    End If %>
    <select class=dataListEntry name="ext7" <%=dataProtect%> size="1" 
               style="text-align:left;" maxlength="8"><%=s%></select>                   
     <input class=dataListEntry name="ext8" <%=dataprotect%> maxlength=40 size=25 style="TEXT-ALIGN: left" value
            ="<%=EXTDB(8)%>">　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">郵遞區號</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="ext9" <%=dataprotect%> maxlength=5 size=5 style="TEXT-ALIGN: left" value
            ="<%=EXTDB(9)%>">　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">ISP代理商代碼</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key18" <%=dataprotect%> maxlength=4 size=4 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(18)%>" ></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">ISP零售代碼</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key19"  maxlength=4 size=4 style="TEXT-ALIGN: left" value
            ="<%=dspkey(19)%>"  >　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">ISP經銷商代碼</font></td>
    <td width="45%" bgcolor="#C0C0C0" colspan=3>
    <input class=dataListDATA name="key17" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(17)%>" readOnly ID="Text3"></td></tr>
  
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">輸入人員</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListDATA name="key8" <%=dataprotect%> maxlength=6 size=6 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(8)%>" readOnly ID="Text1"><%=EusrNc%>　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">輸入日期</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListDATA name="key9"  maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(9)%>"  readOnly ID="Text2">　</td>
  </tr>  
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">異動人員</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListDATA  name="key10" readOnly size=6 maxlength=6 style="TEXT-ALIGN: left "
            value="<%=dspkey(10)%>"><%=UUsrNC%>　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">異動日期</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListDATA name="key11" maxlength=10 size=10    
            style="TEXT-ALIGN: left" value="<%=dspkey(11)%>" readOnly>　</td>
    <input class=dataListEntry name="ext10" maxlength=6 size=6    
            style="TEXT-ALIGN: left" value="<%=extdb(10)%>" style="display:none">
    <input class=dataListEntry name="ext11" maxlength=10 size=10    
            style="TEXT-ALIGN: left" value="<%=extdb(11)%>" style="display:none">  
    <input class=dataListEntry name="ext12" maxlength=6 size=6    
            style="TEXT-ALIGN: left" value="<%=extdb(12)%>" style="display:none">  
      <input class=dataListEntry name="ext13" maxlength=10 size=10    
            style="TEXT-ALIGN: left" value="<%=extdb(13)%>" style="display:none">
  </tr>
</table>
<% conn.close
   set rs=nothing
   set conn=nothing
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrSaveExtDB(Smode)
' extDBField = n
' use extDB(i) for Screen ,and map it to DataBase
'--------------SAVE RTOBJ FILE
DIM conn,rs,dsn,sql
SET conn=server.CreateObject("ADODB.Connection")
set rs=server.CreateObject("ADODB.recordset")
DSN="DSN=RTLIB"
SQL="SELECT cusid,CUSNC,SHORTNC,CUTID1,TOWNSHIP1,RADDR1,RZONE1,CUTID2," _ 
   &"TOWNSHIP2,RADDR2,RZONE2,EUSR,EDAT,UUSR,UDAT " _
   &"FROM RTobj where CUSID ='" & dspkey(0) & "'"
conn.Open dsn
rs.Open sql,conn,3,3
if not rs.EOF then  
   '--由於對象基本資料檔係共用資料,為避免資料因不得使用者輸入導致資料lose
   '--現象;故判斷當使用者有輸入資料時再取代原本資料
   '===========
   '--?????是否會造成欲將資料清空,卻又無法取代的現象發生??????
   '+++再考慮
   rs("cusnc")    =extdb(0)
   rs("shortnc")  =extdb(1)
   rs("cutid1")   =extdb(2)
   rs("township1")=extdb(3)
   rs("raddr1")   =extdb(4)
   rs("rzone1")   =extdb(5)
   rs("cutid2")   =extdb(6)
   rs("township2")=extdb(7)
   rs("raddr2")   =extdb(8)
   rs("rzone2")   =extdb(9)
   rs("uusr")     =dspkey(10)
   rs("udat")     =dspkey(11)
   rs.update
else
   rs.addnew
   rs("cusid")    =dspkey(0)
   rs("cusnc")    =extdb(0)
   rs("shortnc")  =extdb(1)
   rs("cutid1")   =extdb(2)
   rs("township1")=extdb(3)
   rs("raddr1")   =extdb(4)
   rs("rzone1")   =extdb(5)
   rs("cutid2")   =extdb(6)
   rs("township2")=extdb(7)
   rs("raddr2")   =extdb(8)
   rs("rzone2")   =extdb(9)
   rs("eusr")     =dspkey(8)
   rs("edat")     =dspkey(9)
   rs("uusr")     =dspkey(10)
   rs("udat")     =dspkey(11)
   rs.update
end if
rs.close
'-----save RTOBJLINK
SQL="SELECT cusid,custyid,EUSR,EDAT,UUSR,UDAT " _
   &"FROM RTobjLink where CUSID ='" & dspkey(0) & "' and custyid='02' " 
rs.Open sql,conn,3,3
if not rs.EOF then
   rs("eusr")     =dspkey(8)
   rs("edat")     =dspkey(9)
   rs("uusr")     =dspkey(10)
   rs("udat")     =dspkey(11)
   rs.update
else
   rs.addnew
   rs("cusid")    =dspkey(0)
   rs("custyid")  ="02"  
   rs("eusr")     =dspkey(8)
   rs("edat")     =dspkey(9)
   rs("uusr")     =dspkey(10)
   rs("udat")     =dspkey(11)
   rs.update
end if     
rs.close
conn.close
set rs=nothing
set conn=nothing
objectcontext.setcomplete
End Sub
' -------------------------------------------------------------------------------------------- 
%>
