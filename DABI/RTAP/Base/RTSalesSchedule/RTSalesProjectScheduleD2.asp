
<%@ Transaction = required %>

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
                 'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
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
               'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
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
   
  logonid=session("userid")
  Call SrGetEmployeeRef(Rtnvalue,1,logonid)
  V=split(rtnvalue,";")  
  'RESPONSE.Write "LOGONID=" & session("userid") & ";V(0)=" & V(0) & ";EMPLY=" & session("emply") & ";YY=" & session("cyy") & ";MM=" & session("cMM")  
  'RESPONSE.END
  if dspkey(3)="" then
     if v(0)=session("emply") and v(0) <> "" then
        accessmode="A"
        dspkey(3)=session("cyy")
        dspkey(4)=session("cmm")
        dspMode=dspModeAdd
        If sw="" Then
           Call SrClearForm
           message=msgDataEnter
           strBotton=btnSaveExit
           keyProtect=""
           dataProtect=""
        Else
           Call SrReceiveForm 
           Call SrCheckForm(message,formValid)
           If reNew="Y" Then formValid=False
           If formValid Then
              If sw="S" Then
                 Call SrSaveData(message)
                 If sw="E" Then
                    strBotton=btnSaveExit
                    keyProtect=""
                    dataProtect=""
                 Else
                    strBotton=btnNewEditExit
                    accessMode="U"
                 End If
              Else
                 message=msgDataCorrect
                 strBotton=btnSaveExit
                 keyProtect=""
                 dataProtect=""
              End IF
           Else
              strBotton=btnSaveExit
              keyProtect=""
              dataProtect=""
           End IF
        End If
     ELSE
        accessmode="I"
        dspkey(3)=session("cyy")
        dspkey(4)=session("cmm")
        dspMode=dspModeInquery
        strBotton=btnExit
        message=""
     END IF
  else
     if v(0)=session("emply") and v(0) <> "" then
        accessmode="U"
        dspMode=dspModeUpdate
        If sw="" Then
           Call SrreadData(dataFound)
           If dataFound Then
              message=msgDataShow
              strBotton=btnEditExit
           Else
              message=msgDataNotFound
              strBotton=btnExit
           End If
        ElseIF sw="E" Then
           Call SrReceiveForm
           strBotton=btnSaveExit
           dataProtect=""
           message=msgDataEnter
        Else
           Call SrreceiveForm
           Call SrCheckForm(message,formValid)
           If reNew="Y" Then formValid=False
           If formValid Then
              If sw="S" Then
                 Call SrSaveData(message)
                 strBotton=btnEditExit
              Else
                 message=msgDataCorrect
                 strBotton=btnSaveExit
                 dataProtect=""
              End If
           Else
              strBotton=btnSaveExit
              dataProtect=""
           End IF
        End IF
     else
        accessmode="I"
        dspMode=dspModeInquery
        strBotton=btnExit
        message=""
     end if
  end if
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
  numberOfKey=5
  title="業務行程資料維護作業"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT   COMQ1, LINEQ1, CONNECTTYPE, SYY, SMM, SDD1, SDD2, SDD3, SDD4, SDD5, SDD6, SDD7, SDD8, SDD9, SDD10, SDD11, SDD12, SDD13, SDD14, " _
             &"SDD15, SDD16, SDD17, SDD18, SDD19, SDD20, SDD21, SDD22, SDD23, SDD24, SDD25, SDD26, SDD27, SDD28, SDD29, SDD30, SDD31 " _
             &"FROM RTSalesSchedule WHERE COMQ1=0 "
  sqlList="SELECT   COMQ1, LINEQ1, CONNECTTYPE, SYY, SMM, SDD1, SDD2, SDD3, SDD4, SDD5, SDD6, SDD7, SDD8, SDD9, SDD10, SDD11, SDD12, SDD13, SDD14, " _
             &"SDD15, SDD16, SDD17, SDD18, SDD19, SDD20, SDD21, SDD22, SDD23, SDD24, SDD25, SDD26, SDD27, SDD28, SDD29, SDD30, SDD31 " _
             &"FROM RTSalesSchedule WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  userDefineSave="Yes"  
  userdefineactivex="Yes"    
  extDBField=0

 ' RESPONSE.Write "V(0)=" & V(0) & ";EMPLY=" & SESSION("EMPLY") & ";ACCESSMODE=" & ACCESSMODE
  'RESPONSE.END
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    IF LEN(TRIM(DSPKEY(33)))=0 THEN DSPKEY(33)=""
    IF LEN(TRIM(DSPKEY(34)))=0 THEN DSPKEY(34)=""
    IF LEN(TRIM(DSPKEY(35)))=0 THEN DSPKEY(35)=""

End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()%>
<p>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
       <tr>
       <td width="10%" class=dataListHead>社區序號</td>
       <td width="10%" bgcolor=silver>
           <input class=dataListEntry type="text" name="key0" <%=keyprotect%> size="5" 
           value="<%=dspKey(0)%>" maxlength="2" ></td>
       <td width="10%" class=dataListHead>主線序號</td>
       <td width="10%" bgcolor=silver>
           <input class=dataListEntry type="text" name="key1" <%=keyprotect%> size="2" 
           value="<%=dspKey(1)%>" maxlength="2" ID="Text1"></td>
       <td width="5%" class=dataListHead>方案</td>
       <td width="15%" bgcolor=silver>
           <input class=dataListEntry type="text" name="key2" <%=keyprotect%> size="2" 
           value="<%=dspKey(2)%>" maxlength="2" ID="Text2">
           <%
           if dspkey(2)="1" or dspkey(2)="4" then
              name="HB599"
           elseif dspkey(2)="2" then
              name="中華399"
           elseif dspkey(2)="3" then
              name="速博399"
           elseif dspkey(2)="5" then
              name="東森499"
           end if
           %>
           <font size=2><%=name%></font></td>
       <td width="5%" class=dataListHead>年</td>
       <td width="10%" bgcolor=silver>
           <input class=dataListEntry type="text" name="key3" <%=keyprotect%> size="4" 
           value="<%=dspKey(3)%>" maxlength="2" ID="Text3"></td>
       <td width="5%" class=dataListHead>月</td>
       <td width="10%" bgcolor=silver>
           <input class=dataListEntry type="text" name="key4" <%=keyprotect%> size="2" 
           value="<%=dspKey(4)%>" maxlength="2" ID="Text4"></td>
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
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
%>
<table border="1" width="100%" cellspacing="0" cellpadding="0">
    <%
    '抓取當月第一天星期
    cyy=session("cyy")
    cmm=session("cmm")
  '  response.Write "k1=" & cyy & ";k2=" & cmm
    firstdat=cyy & "/" & cmm & "/" & "01"
    firstweek=weekday(firstdat)
   ' response.Write "week=" & firstweek & ";"
    '抓取當月日數
    nextmonth=datepart("yyyy",firstdat) & "/" & datepart("m",dateadd("m",1,firstdat)) & "/" & "01"
    nextmonth=dateadd("d",-1,nextmonth)
    maxmonthday=datepart("d",nextmonth)
   ' response.Write "maxday=" & maxmonthday
    RESPONSE.Write "<TR>"
    str="日一二三四五六"
    FOR I=1 TO 7
       RESPONSE.Write "<TD height=40 align=center bgcolor=""darkkhaki"">" & mid(str,i,1) & "</TD>"
    NEXT
    RESPONSE.Write "</TR>"
    xcnt=0
    if ( maxmonthday >=30 and firstweek=7 ) or ( maxmonthday >=31 and firstweek=6 ) then
       k=5
    elseif ( maxmonthday <= 28 and firstweek=1 ) then
       k=3
    else
       k=4
    end if
    FOR I=0 TO k
        RESPONSE.Write "<TR>"
        FOR J=0 TO 6
            cnt=cnt+1
            if cnt>=firstweek  then
               xcnt=xcnt+1
               xxx=cstr(xcnt)
               YYY=XXX+4
               s=""
               sx=" selected "
               If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 Then  
                   sql="SELECT * FROM HBSalesManageProjectD WHERE (SCITEM = 'A1') " 
                   If len(trim(dspkey(YYY))) < 1 Then
                       sx=" selected " 
                       s=s & "<option value=""""" & sx & "></option>"  
                       sx=""
                   else
                       s=s & "<option value=""""" & sx & "></option>"  
                       sx=""
                   end if     
               Else
                   sql="SELECT * FROM HBSalesManageProjectD WHERE (SCITEM = 'A1')  AND STAT='" & dspkey(YYY) & "'"
               End If
               rs.Open sql,conn
               Do While Not rs.Eof
                   If rs("STAT")=dspkey(YYY) Then sx=" selected "
                      s=s &"<option value=""" &rs("STAT") &"""" &sx &">" &rs("STATDESC") &"</option>"
                      rs.MoveNext
                      sx=""
               Loop
               rs.Close
            end if
            if j=0 then
               xbgcolor=" bgcolor=""cornflowerblue"" "
            elseif j=6 then
               xbgcolor=" bgcolor=""honeydew"" "
            else
               xbgcolor=" bgcolor=""aliceblue"" "
            end if
            if xcnt=0 or xcnt > maxmonthday then
               xxx="&nbsp;"
               S=""
            end if
            RESPONSE.Write "<TD " & xbgcolor & " height=80 ID=""" & I&J &""" style=""text-align:center;vertical-align:CENTER;color:#DC143C;font-size:10.0pt"">" & XXX & "<select size=""1"" name=""key" & YYY & """ style=""font-family: 新細明體; font-size: 10pt"" " & dataProtect & " class=""dataListEntry"" ID=""Select3"">" & S & "</select></TD>"
        NEXT
        RESPONSE.Write "</TR>"
    NEXT
    %>
   
</table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrSaveExtDB(Smode)
End Sub
' -------------------------------------------------------------------------------------------- 
%>
