
<%@ Transaction = required %>

<!-- #include virtual="/WebUtilityV4/DBAUDI/cType.inc" -->
<%
  Dim message,sw,formValid,accessMode,dataFound,dspMode,strBotton,reNew,rwCnt
  Dim keyProtect,dataProtect
  Dim msgDateEnter,msgDataShow,msgDataNotFound,msgDataCorrect,msgSaveOK,msgDupKey,msgErrorRec
  Dim btnSave,btnEdit,btnExit,btnNew,btnSaveExit,btnEditExit,btnNewEditExit
  Dim btnSaveName,btnEditName,btnExitName,btnNewName
  Dim dspModeAdd,dspModeInquery,dspModeUpdate
  Dim tempkey3
  msgDataEnter="請輸入資料"
  msgDataShow="資料如下"
  msgDataNotFound="資料找不到"
  msgDataCorrect="資料正確，可按確認存檔"
  msgSaveOK="存檔完成"
  msgDupKey="資料重複輸入，請更改鍵值後存檔"
  msgErrorRec="資料更新有誤"
  dspModeAdd="新增"
  dspModeInquery="查詢"
  dspModeUpdate="修改"
  btnSaveName=" 存檔 "
  btnEditName=" 編輯 "
  btnExitName=" 結束 "
  btnNewName=" 新增 "
' --------------------------------------------------------------------------------------------
  Dim title,numberOfKey,DSN
' --------------------------------------------------------------------------------------------
Sub SrDspInit()
  btnSave="<input type=""button"" class=dataListButton id=""btnSave"" value=""" _
         &btnSaveName &""" style=""cursor:hand;""" _
         &" onClick=""vbscript:sw.Value='S':Window.form.Submit"">" 
  btnEdit="<input type=""button"" class=dataListButton id=""btnEdit"" value=""" _
         &btnEditName &""" style=""cursor:hand;""" _
         &" onClick=""vbscript:sw.Value='E':Window.form.Submit"">" 
  btnExit="<input type=""button""  class=dataListButton id=""btnExit"" value=""" _
         &btnExitName &""" style=""cursor:hand;"" onClick=""vbscript:window.close"">"
  btnNew ="<input type=""button"" class=dataListButton id=""btnNew"" value=""" _
         &btnNewName &""" style=""cursor:hand;""" _
         &" onClick=""vbscript:sw.Value='':accessMode.Value='A':Window.form.Submit"">" 
  btnSaveExit=btnSave &"<span>&nbsp;&nbsp;</span>" &btnExit
  btnEditExit=btnEdit &"<span>&nbsp;&nbsp;</span>" &btnExit
  btnNewEditExit=btnNew &"<span>&nbsp;&nbsp;</span>" &btnEdit &"<span>&nbsp;&nbsp;</span>" &btnExit
End Sub
' --------------------------------------------------------------------------------------------
Sub SrProcessForm()
  Call SrDspInit
  Call SrInit(accessMode,sw)
  keyProtect=" readonly "
  dataProtect=" readonly "
  strBotton=""
  message=""
  dspMode=""
  If accessMode="I" Then
     dspMode=dspModeInquery
     If sw="" Then
        Call SrReadData(dataFound)
        If dataFound Then
           message=msgDataShow
        Else
           message=msgDataNotFound
        End If
     End If
     strBotton=btnExit
  ElseIf accessMode="U" Then
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
  Else
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
  End IF 
  Call SrSendForm(message)
End Sub
%>
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
                     response.Write "I=" & i & ";value=" & dspkey(i) & "<BR>"
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
  Dim winP,docP
  Set winP=window.Opener
  Set docP=winP.document
  docP.all("keyform").Submit
  winP.focus()
  window.close
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
  numberOfKey=2
  title="合約屬性樹(大類別)頁數異動"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT propertyid,category1,category1nm,volumepage,chgflag,lastchgdat FROM HBContractTreeL1 WHERE propertyid='*' "
  sqlList="SELECT  propertyid,category1,category1nm,volumepage,chgflag,lastchgdat FROM HBContractTreeL1 WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  userDefineSave="Yes"  
  extDBField=1
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    If len(trim(dspKey(1))) =0 Then
       formValid=False
       message="請輸入合約大類別代號"
    elseif len(trim(dspKey(2))) = 0 Then
       formValid=False
       message="請輸入合約大類別名稱"       
    ELSEIF DSPKEY(1) < "00" OR DSPKEY(1) > "99" THEN
       formValid=False
       message="合約大類別代號必須介於00-99"
    ELSEIF not Isnumeric(DSPKEY(3)) THEN
       formValid=False
       message="歸檔每冊頁號錯誤!"       
    ELSEIF len(DSPKEY(3)) < 0 THEN
       formValid=False
       message="歸檔每冊頁號不可為零!"              
    End If        
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()%>
<p>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="30%" class=dataListHead>合約性質代號</td>
       <td width="70%" bgcolor=silver>
           <input class=dataListdata type="text" name="key0" <%=keyprotect%> size="10" 
           value="<%=dspKey(0)%>" readonly maxlength="1" ></td>
       </tr>
       <tr><td width="30%" class=dataListHead>合約大類代號</td>
       <td width="70%" bgcolor=silver>
           <input class=dataListEntry type="text" name="key1" <%=keyprotect%> size="10" 
           value="<%=dspKey(1)%>" maxlength="2" ID="Text1"></td>
       </tr>       
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
    logonid=session("userid")
    '---暫存舊頁數
    SESSION("tempkey3")=dspkey(3)
%>
<table border="1" width="100%" cellspacing="0" cellpadding="0">
  <tr>
    <td width="20%" bgcolor="#008080"><font color="#FFFFFF">大類名稱</font></td>
    <td width="30%" bgcolor="#C0C0C0">
     <input class=dataListDATA name="KEY2" <%=dataprotect%> READONLY maxlength=20 size=20 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(2)%>"></td>
    <td width="20%" bgcolor="#008080"><font color="#FFFFFF">每冊頁數</font></td>
    <td width="30%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="KEY3" <%=dataprotect%>  maxlength=20 size=20 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(3)%>" ></td>            
  </tr>
    <tr>
    <td width="20%" bgcolor="#008080"><font color="#FFFFFF">異動碼</font></td>
    <td width="30%" bgcolor="#C0C0C0">
     <input class=dataListDATA name="KEY4" <%=dataprotect%> READONLY maxlength=20 size=20 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(4)%>" ></td>
    <td width="20%" bgcolor="#008080"><font color="#FFFFFF">最近異動日</font></td>
    <td width="30%" bgcolor="#C0C0C0">
     <input class=dataListDATA name="KEY5" <%=dataprotect%> READONLY maxlength=20 size=20 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(5)%>" ></td>            
  </tr>
</table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrSaveExtDB(Smode)
   '---有實質異動到每冊頁數時才做記錄
   IF SESSION("tempkey3") <> dspkey(3) then
     Set conn=Server.CreateObject("ADODB.Connection")
     Set conn2=Server.CreateObject("ADODB.Connection")
    conn.open DSN
    Set rs=Server.CreateObject("ADODB.Recordset")
    Set rs2=Server.CreateObject("ADODB.Recordset")
 '------ RTObj ---------------------------------------------------
    rs.open "select max(entryno) AS entryno from HBContractTreeL1CHG WHERE propertyid='" &dspKey(0) &"' and category1='" & dspkey(1) & "' ",conn
    if LEN(trim(rs("entryno"))) > 0 then
       entryno=rs("entryno") + 1
    ELSE
       ENTRYNO=1
    end if
    rs.close
    conn.Close
    conn2.open DSN
    rs2.Open "SELECT * FROM HBContractTreeL1CHG WHERE propertyid='" &dspKey(0) &"' and category1='" & dspkey(1) & "' ",conn2,3,3
   ' If rs2.Eof Or rs2.Bof Then
    rs2.AddNew
    rs2("propertyid")=dspKey(0)
    rs2("category1")=dspKey(1)
    rs2("entryno")=entryno
   ' END IF
    rs2("Nvolumepage")=dspKey(3)
    rs2("transdat")=now()
    logonid=session("userid")
    Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
    'response.Write "V0=" & V(0)
    Rs2("transUSR")=V(0)
    rs2.Update
    rs2.Close
    rs2.Open "update HBContractTreeL1 SET CHGFLAG='Y',lastchgdat = getdate() where propertyid='" &dspKey(0) &"' and category1='" & dspkey(1) & "' "
    'rs2.close
    Set rs2=Nothing
    Set rs=Nothing
    Set conn=Nothing
    Set conn2=Nothing
    END IF
End Sub
' -------------------------------------------------------------------------------------------- 
%>
