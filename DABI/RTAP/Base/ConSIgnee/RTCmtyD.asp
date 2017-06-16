<%  
  Dim fieldRole,fieldPa
  fieldRole=Split(FrGetUserRight("RTCustD",Request.ServerVariables("LOGON_USER")),";")
%>
<%
  Const cTypeChar="129;130;200;201;202;203"
  Const cTypeN="131"  
  Const cTypeNumeric="002;003;005;006;017;131"
  Const cTypeDate="135"
  Const cTypeBoolean="011"
  Const cTypeVarChar="203"
  Const cTypeInterger="002"
  Const cTypeFloat="006"
  Const cTypeAuto="000"
%>
<%
  Dim message,sw,formValid,accessMode,dataFound,dspMode,strBotton,reNew,rwCnt
  Dim keyProtect,dataProtect
  Dim msgDateEnter,msgDataShow,msgDataNotFound,msgDataCorrect,msgSaveOK,msgDupKey,msgErrorRec
  Dim btnSave,btnEdit,btnExit,btnNew,btnSaveExit,btnEditExit,btnNewEditExit
  Dim btnSaveName,btnEditName,btnExitName,btnNewName
  Dim dspModeAdd,dspModeInquery,dspModeUpdate
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
                 ' 當程式為速博ADSL社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtsparqADSLcmty/RTCmtyd.asp")
                     if i<>0 then rs.Fields(i).Value=dspKey(i)                         

                ' 當程式為速博ADSL客戶基本資料維護作業時,因其dspkey(76)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtsparqadslcmty/RTCustd.asp")
                      if i=2 then
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(entryno) AS entryno from rtsparqadslcust where  cusid='" & dspkey(1) & "'"
                        rsc.open sqlstr2,conn
                        if not rsc.eof then
                           if len(trim(rsc("entryno"))) > 0 then
                              dspkey(i)=rsc("entryno") + 1
                           else
                              dspkey(i)=1
                           end if
                        else
                           dspkey(i)=1
                        end if
                        rsc.close
                      end if          
                 
                      if i=76 then
                        logonid=session("userid")
                        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                        V=split(rtnvalue,";") 
                        area=mid(v(0),1,1)
                        '---桃園服務中心之地區別改為"U"(部門代號B600)
                        Call SrGetEmployeeRef(Rtnvalue,5,logonid)
                        V=rtnvalue
                        if trim(V) = "B600" then
                           AREA="U"
                        end if
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(orderno) AS orderno from rtsparqadslcust where  orderno like '" & area & "%'"
                        rsc.open sqlstr2,conn
                        if len(rsc("orderno")) > 0 then
                           dspkey(i)=area & right("000000" & cstr(cint(mid(rsc("orderno"),2,6)) + 1),6)
                        else
                           dspkey(i)=area & "000001"
                        end if
                        rsc.close
                      end if       
                      
                      rs.Fields(i).Value=dspKey(i)             
                '        response.write "i=" & i & ";dspkey(i)=" & dspkey(i) & "<Br>"             
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
              select case ucase(runpgm)   
           ' 當程式為速博ADSL社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtsparqADSLcmty/RTCmtyd.asp")
                     if i<>0 then rs.Fields(i).Value=dspKey(i)                                       
                 ' 當程式為速博ADSL客戶基本資料維護作業時,因其dspkey(77)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtsparqADSLcust/RTcustd.asp")
                     'if i<>76 then rs.Fields(i).Value=dspKey(i)  
                     'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
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
<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=1
  title="速博ADSL社區及客戶資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT * FROM RTSparqAdslCmty WHERE cutyid=0 "
  sqlList="SELECT * FROM RTSparqAdslCmty WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=2
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    if len(trim(dspkey(31)))= 0 then dspkey(31)=0
    if len(trim(dspkey(32)))= 0 then dspkey(32)=0
    If len(dspKey(0)) <= 0 Then
       dspkey(0)=0
    End If
    if len(dspkey(1)) < 1 Then
       formValid=False
       message="請輸入社區名稱"       
    end if
    if dspkey(28) <> "Y" and dspkey(28) <> "N" then
       dspkey(28)=""
    end if
'---檢查 附掛號碼是否有重複 dspkey(3)---------
    if len(trim(dspkey(3))) > 0 then
       IF LEN(TRIM(dspkey(0))) > 0 THEN
          Set connxx=Server.CreateObject("ADODB.Connection")  
          Set rsxx=Server.CreateObject("ADODB.Recordset")
          DSNXX="DSN=RTLIB"
          connxx.Open DSNxx

	     if LEN(TRIM(DSPKEY(0))) =0 then
	        DSPKEY(0) =0
	     end if   
         sqlXX="SELECT count(*) AS CNT FROM RTsparqAdslcmty where CMTYTEL='" & dspkey(3) & "' and not (cutyid=" & dspkey(0) & ")"
         rsxx.Open sqlxx,connxx
         s=""
         If RSXX("CNT") > 0 Then
            message="主線附掛電話號碼已存在ADSL其它社區，不可重複輸入!"
            formvalid=false
         End If
         rsxx.Close
         Set rsxx=Nothing
         connxx.Close
         Set connxx=Nothing    
       end IF  
    end if  
    'ADSL社區default="N"...HB==>DEFAULT="Y"
    if len(trim(dspkey(35)))=0 then dspkey(35)=""
    if len(trim(dspkey(37)))=0 then dspkey(37)=""
    if len(trim(dspkey(42)))=0 then dspkey(42)=""    
    if trim(dspkey(35))<>"Y" then
       dspkey(36)=""
    end if
    if trim(dspkey(37))<>"Y" then
       dspkey(38)=""
    end if     
    if trim(dspkey(42))<>"Y" then
       dspkey(43)=""
    end if         
   
    '---若合約編號有值且標識為"有"==>表先前已存在號碼
    ',且同意書編號為空白且標示為"有"==>表新增號碼,則賦予同意書編號等於合約編號
    if trim(dspkey(35))="Y" and len(trim(dspkey(36)))=0 and trim(dspkey(37))="Y" and len(trim(dspkey(38))) > 0 then
       dspkey(36)="AA" & mid(dspkey(38),3,5)
    elseif trim(dspkey(35))="Y" and len(trim(dspkey(36)))=0 and trim(dspkey(42))="Y" and len(trim(dspkey(43))) > 0 then
       dspkey(36)="AA" & mid(dspkey(43),3,5)
    end if
    '---若同意書編號有值且標識為"有"==>表先前已存在號碼
    ',且合約書編號為空白且標示為"有"==>表新增號碼,則賦予合約書編號等於同意書編號    
    if trim(dspkey(37))="Y" and len(trim(dspkey(38)))=0 and trim(dspkey(35))="Y" and len(trim(dspkey(36))) > 0 then
       dspkey(38)="AB" & mid(dspkey(36),3,5)
    elseif trim(dspkey(37))="Y" and len(trim(dspkey(38)))=0 and trim(dspkey(42))="Y" and len(trim(dspkey(43))) > 0 then
       dspkey(38)="AB" & mid(dspkey(43),3,5)       
    end if
    '---若匯款同意書編號有值且標識為"有"==>表先前已存在號碼
    ',且合約書編號為空白且標示為"有"==>表新增號碼,則賦予合約書編號等於同意書編號    
    if trim(dspkey(42))="Y" and len(trim(dspkey(43)))=0 and trim(dspkey(35))="Y" and len(trim(dspkey(36))) > 0 then
       dspkey(43)="AC" & mid(dspkey(36),3,5)
    elseif trim(dspkey(42))="Y" and len(trim(dspkey(43)))=0 and trim(dspkey(37))="Y" and len(trim(dspkey(38))) > 0 then
       dspkey(43)="AC" & mid(dspkey(38),3,5)              
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
   Sub Srbranchonclick()
       prog="RTGetBRANCHD.asp"
       prog=prog & "?KEY=" & document.all("KEY21").VALUE 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key22").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub         
   Sub SrbranchMANonclick()
       prog="RTGetBRANCHMAND.asp"
       prog=prog & "?KEY=" & document.all("KEY21").VALUE & ";" & document.all("KEY22").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key23").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub        
   Sub Srsalesgrouponclick()
       prog="RTGetsalesgroupD.asp"
       prog=prog & "?KEY=" & document.all("KEY9").VALUE 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key10").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub        
   Sub Srsalesonclick()
       prog="RTGetsalesD.asp"
       prog=prog & "?KEY=" & document.all("KEY9").VALUE & ";" & document.all("KEY10").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key8").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub            
   Sub Sr34salesonclick()
       prog="RTGetsalesD.asp"
       prog=prog & "?KEY=" & document.all("KEY9").VALUE & ";" & document.all("KEY10").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key34").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub               
   Sub SrWorkmanonclick()
       prog="RTGetworkmanD.asp"
       prog=prog & "?KEY=" & document.all("KEY9").VALUE & ";" & document.all("KEY10").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key15").value =  trim(Fusrid(0))
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
       <tr><td width="20%" class=dataListHead>社區序號</td><td width="80%"  bgcolor="silver">
           <input type="text" name="key0"
                 <%=fieldRole(1)%> readonly size="20" value="<%=dspKey(0)%>" maxlength="8" class=dataListEntry></td></tr>
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
                EUsrNc=V(1) 
                dspkey(25)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(24))
                V=split(rtnvalue,";")      
                EUsrNc=V(1)
        End if  
       dspkey(25)=datevalue(now())
    else
        if len(trim(dspkey(26))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                UUsrNc=V(1)
                DSpkey(27)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(26))
                V=split(rtnvalue,";")      
                UUsrNc=V(1)
        End if         
        Call SrGetEmployeeRef(rtnvalue,2,dspkey(24))
             V=split(rtnvalue,";")      
             EUsrNc=V(1)
        dspkey(27)=now()
    end if      
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
 '   If IsDate(dspKey(26)) Then
 '      fieldPa=" class=""dataListData"" readonly "
 '   Else
 '      fieldPa=""
 '   End If
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
  <span id="tags1" class="dataListTagsOn">基本資料及發包安裝</span>
                                                            
  <div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td><td>        
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
<tr><td width="20%" class=dataListSearch>經銷商</td>
    <td width="35%" colspan=3  bgcolor="silver">
<%  If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  Then 
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
          &"WHERE (RTObjLink.CUSTYID = '02')  "
       s="<option value="""" >(經銷商)</option>"
    Else
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
          &"WHERE (RTObjLink.CUSTYID = '02')  and rtobj.cusid='" & dspkey(50) & "' "
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(經銷商)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(50) Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>
           <select size="1" name="key50" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" readonly>                                            
              <%=s%>
           </select></td>
</tr>
<tr><td width="20%" class=dataListHead>社區名稱</td>
    <td width="35%" bgcolor="silver">
        <input type="text" name="key1" <%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="50"
               value="<%=dspKey(1)%>" size="40" class=dataListEntry readonly></td>
    <td width="15%" class=dataListHead>速博社區電腦代碼</td>
    <td width="25%" bgcolor="silver">
        <input type="text" name="key2" <%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="30"
               value="<%=dspKey(2)%>" size="20" class=dataListEntry readonly></td></tr>

<tr><td class=dataListsearch>主線附掛電話號碼</td>
    <td bgcolor="silver">
        <input type="text" name="key3" <%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="15"
               value="<%=dspKey(3)%>" size="15" class=dataListEntry readonly></td>

    <td class=dataListHead>IP位址</td><td>
        <input type="text" name="key4" <%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="15"
               value="<%=dspKey(4)%>" size="15" class=dataListEntry readonly></td>
</tr>  

<tr><td class=dataListHead>設備位置</td>
    <td colspan="3" bgcolor="silver">
        <input type="text" name="key5" <%=fieldRole(1)%><%=dataProtect%> 
               style="text-align:left;" size="60" maxlength="60"
               value="<%=dspKey(5)%>" class=dataListEntry readonly></td></tr> 
<tr><td class=dataListHead>電信室位置</td> 
    <td colspan="3" bgcolor="silver"> 
        <input type="text" name="key6" <%=fieldRole(1)%><%=dataProtect%> 
               style="text-align:left;" size="60" maxlength="60" 
               value="<%=dspKey(6)%>"   class="dataListEntry" readonly>
               </td>
</tr>   
<tr><td class=dataListHead>可供裝範圍</td> 
    <td COLSPAN="3" bgcolor="silver"><input type="text" name="key7" <%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="150"
               value="<%=dspKey(7)%>" size="90" class=dataListEntry readonly></td>
</tr>   
<tr><td class=dataListHead>業務轄區</td> 
    <td COLSPAN="3" bgcolor="silver">
<%  If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 Then 
       sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '1') "
       s="<option value="""" >(業務轄區)</option>"
    Else
       sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '1') "
       s="<option value="""" >(業務轄區)</option>"
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(業務轄區)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("areaid")=dspkey(9) Then sx=" selected "
       s=s &"<option value=""" &rs("areaid") &"""" &sx &">" &rs("areanc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>    
           <select size="1" name="key9" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" readonly>                                            
              <%=s%>
           </select>
    <input type="text" name="key10" <%=fieldRole(1)%><%=dataProtect%> 
               style="text-align:left;" size="15" maxlength="10" 
               value="<%=dspKey(10)%>"   readonly class="dataListEntry">
         <input type="button" id="B10"  name="B10"   width="100%" style="Z-INDEX: 1"  value="...." readonly onclick="SrsalesGrouponclick()"  >  
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C10"  name="C10"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">                               
    <input type="hidden" name="key8" <%=fieldRole(1)%><%=dataProtect%> 
               style="text-align:left;" size="15" maxlength="10" 
               value="<%=dspKey(8)%>"  readonly class="dataListEntry">
           <input type="hidden" id="B8"  name="B8"   width="100%" style="Z-INDEX: 1"  value="...." onclick="Srsalesonclick()"  >                  
</tr>   
<tr STYLE="DISPLAY:NONE"><td class=dataListHead STYLE="DISPLAY:NONO">券商</td> 
<%    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 Then 
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
          &"WHERE (RTObjLink.CUSTYID = '06') AND RTOBJ.CUSID NOT IN ('70770184', '47224065') "
       s="<option value="""" >(證券公司)</option>"
    Else
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
          &"WHERE (RTObjLink.CUSTYID = '06') AND RTOBJ.CUSID NOT IN ('70770184', '47224065') "
       s="<option value="""" >(證券公司)</option>"
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(證券公司)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(21) Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>
    <td COLSPAN="3" bgcolor="silver" STYLE="DISPLAY:NONE">
           <select size="1"  STYLE="DISPLAY:NONE" name="key21" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry">                                            
              <%=s%>
           </select>
    <input  STYLE="DISPLAY:NONE" type="text" name="key22" <%=fieldRole(1)%><%=dataProtect%> 
               style="text-align:left;" size="15" maxlength="10" 
               value="<%=dspKey(22)%>"   class="dataListEntry" readonly>
         <input  STYLE="DISPLAY:NONE" type="button" id="B22"  name="B22"   width="100%" style="Z-INDEX: 1"  value="...." readonly onclick="SrBranchonclick()"  >  
          <IMG  STYLE="DISPLAY:NONE" SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C22"  name="C22"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">                
    <input STYLE="DISPLAY:NONE" type="text" name="key23" <%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(23)%>" size="15" class=dataListEntry readonly>
         <input  STYLE="DISPLAY:NONE" type="button" id="B23"  name="B23"   width="100%" style="Z-INDEX: 1"  value="...." onclick="SrBranchmanonclick()"  >                  
          <IMG  STYLE="DISPLAY:NONE" SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C23"  name="C23"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">  
    </td>               
</tr>   
<tr>
    <td class=dataListHead>CHT營運處</td>
    <td  bgcolor="silver"><input  type="text" name="key20" <%=fieldRole(1)%><%=dataProtect%> 
               style="text-align:left;" size="15" maxlength="10" 
               value="<%=dspKey(20)%>"   class="dataListEntry"></TD>
    <td class=dataListHead>是否可建置</td>
    <td  bgcolor="silver">     
      <%  dim rdo1, rdo2
              If Len(Trim(fieldRole(1) &dataProtect)) < 1 Then
                 rdo1=""
                 rdo2=""
              Else
                 rdo1=" disabled "
                 rdo2=" disabled "
              End If
             ' If Trim(dspKey(84))="" Then dspKey()="Y"
              If trim(dspKey(28))="Y" Then 
                 rdo1=" checked "    
              elseIf trim(dspKey(28))="N" Then 
                 rdo2=" checked " 
              elseif trim(dspkey(28))="" then
                 dspkey(28)=""                 
              end if
             %>
        <input type="radio" value="Y" <%=rdo1%> name="key28" <%=fieldRole(1)%><%=dataProtect%> readonly><font size=2>可建置
        <input type="radio" value="N" <%=rdo2%>  name="key28" <%=fieldRole(1)%><%=dataProtect%> readonly><font size=2>無法建置</TD>               
</tr>   
<tr><td class=dataListHead>不可建置原因</td> 
    <td COLSPAN="3" bgcolor="silver"><input type="text" name="key19" <%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="80"
               value="<%=dspKey(19)%>" size="60" class=dataListEntry></td>
</tr>   
<tr>                                 
        <td  class="dataListHead" height="23">社區戶數</td>                                 
        <td  height="23" bgcolor="silver"><input type="text" name="key31" size="15" value="<%=dspKey(31)%>"  <%=fieldRole(1)%> class="dataListEntry" readonly></td>                                 
        <td  class="dataListHead" height="23">社區棟數</td>                                 
        <td  height="23" bgcolor="silver"><input type="text" name="key32" size="15" value="<%=dspKey(32)%>"  <%=fieldRole(1)%> class="dataListEntry" readonly></td>                                 
      </tr>                                 
      <tr>                                 
        <td  class="dataListHead" height="23">社區電源</td>                                 
        <td  width="25%" height="23" bgcolor="silver">
       <% aryOption=Array("","110V","220V")
          s=""
          If Len(Trim(fieldRole(1) &dataProtect)) < 1  Then 
             For i = 0 To Ubound(aryOption)
                If dspKey(33)=aryOption(i) Then
                   sx=" selected "
                Else
                   sx=""
                End If
                s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
             Next
           Else
             s="<option value=""" &dspKey(33) &""">" &dspKey(33) &"</option>"
           End If%>               
   <select size="1" name="key33" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" readonly>                                            
        <%=s%>
   </select>
   </td>           
    <td  class="dataListHead" height="23">建置聯絡人</td>                                 
        <td  height="23" bgcolor="silver"><input type="text" name="key34" size="15" value="<%=dspKey(34)%>" <%=fieldRole(1)%> class="dataListEntry" readonly>                         
     <input type="button" id="B34"  name="B34"   width="100%" style="Z-INDEX: 1"  value="..." onclick="Sr34salesonclick()"  >
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C34"  name="C34"   style="Z-INDEX: 1" onclick="srclear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" ></td>      
      </tr>            
<tr><td class=dataListHead>備註</td> 
    <td COLSPAN="3" bgcolor="silver"><input type="text" name="key49" <%=fieldRole(1)%>
               style="text-align:left;" maxlength="150"
               value="<%=dspKey(49)%>" size="90" maxlength="150"  class=dataListEntry readonly></td>
</tr>   
<tr style="display:none">                                 
        <td  class="dataListHead" height="23">輸入人員</td>                                 
        <td  height="23" bgcolor="silver"><input type="text" name="key24" size="15" value="<%=dspKey(24)%>" readonly class="dataListData"><%=EusrNC%></td>                                 
        <td  class="dataListHead" height="23">輸入日期</td>                                 
        <td  colspan="3" height="23" bgcolor="silver"><input type="text" name="key25" size="15" value="<%=dspKey(25)%>" readonly class="dataListData"></td>                                 
      </tr>                                 
      <tr style="display:none">                                 
        <td  class="dataListHead" height="23">異動人員</td>                                 
        <td  height="23" bgcolor="silver"><input type="text" name="key26" size="15" value="<%=dspKey(26)%>" readonly class="dataListData"><%=UUsrNc%></td>                                 
        <td  class="dataListHead" height="23">異動日期</td>                                 
        <td  colspan="3" height="23" bgcolor="silver"><input type="text" name="key27" size="15" value="<%=dspKey(27)%>" readonly class="dataListData"></td>                                 
      </tr>      
</table> 
<!--
    <table border="1" width="100%" cellpadding="0" cellspacing="0" id="tag2" style="display: none"> 
    -->
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr><td bgcolor="lightblue" align="center">施工進度狀況</td></tr></table>
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >

      <tr><td width="20%" class="dataListHead">勘察日期</td>
          <td width="35%" bgcolor="silver"><input type="text" name="key11" size="15" READONLY value="<%=dspKey(11)%>" <%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> maxlength="10" class="dataListEntry" >
                           <input type="button" id="B11"  name="B11" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
                           <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C11"  name="C11"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>
          <td width="15%" class="dataListHead">線路申請日</td>
          <td width="25%" bgcolor="silver"><input type="text" name="key12" size="15" READONLY value="<%=dspKey(12)%>" <%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> maxlength="10" class="dataListEntry">
                           <input type="button" id="B12"  name="B12" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
                           <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C12"  name="C121"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td></tr>
      <tr><td width="20%" class="dataListHead">機櫃派工日</td>
          <td width="35%" bgcolor="silver"><input type="text" name="key13" size="15" READONLY value="<%=dspKey(13)%>" <%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> maxlength="10" class="dataListEntry">
                           <input type="button" id="B13"  name="B13" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
                           <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C13"  name="C13"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>
          <td width="15%" class="dataListHead">設備到位日</td>
          <td width="25%" bgcolor="silver"><input type="text" name="key14" size="15" READONLY value="<%=dspKey(14)%>" <%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> maxlength="30" class="dataListEntry">
                            <input type="button" id="B14"  name="B14" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
                            <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C14"  name="C14"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td></tr>
      <tr><td width="20%" class="dataListHead">安裝工程師</td>
          <td width="35%" bgcolor="silver"><input type="text" name="key15" size="15" value="<%=dspKey(15)%>" <%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> maxlength="10" class="dataListEntry" >
            <input type="button" id="B15"  name="B15"   width="100%" style="Z-INDEX: 1"  value="...." onclick="Srworkmanonclick()"  >                  
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C15"  name="C15"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear"> </td>
          <td width="15%" class="dataListHead">至營運處日</td>
          <td width="25%" bgcolor="silver"><input type="text" name="key16" size="15" READONLY value="<%=dspKey(16)%>" <%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> maxlength="10" class="dataListEntry">
                           <input type="button" id="B16"  name="B16" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
                           <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C16"  name="C16"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td></tr>
      <tr>
          <td width="15%" class="dataListHead">線路到位日</td>
          <td width="25%" bgcolor="silver"><input   type="text" name="key17" size="15" READONLY value="<%=dspKey(17)%>" <%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> maxlength="10" class="dataListEntry" >
                           <input  type="button" id="B17"  name="B17" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
                           <IMG style="display:none"  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C17"  name="C17"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>
          <td width="20%" class="dataListHead">測通日期</td>
          <td width="35%" bgcolor="silver"><input type="text" name="key18" size="15" READONLY value="<%=dspKey(18)%>" <%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> maxlength="10" class="dataListEntry" >
                          <input type="button" id="B18"  name="B18" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
                          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C18"  name="C18"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>
      <TR>
      <td width="20%" class="dataListHead">退租(撤銷)日</td>
          <td width="35%" colspan="5" bgcolor="silver"><input type="text" name="key29" size="15" READONLY value="<%=dspKey(29)%>" <%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> maxlength="10" class="dataListEntry" >
                          <input type="button" id="B29"  name="B29" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
                          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C29"  name="C29"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>                           
      </TR>
      <tr>
          <td width="150%" class="dataListHead">退租(撤銷)原因</td>
          <td width="25%" COLSPAN="3" bgcolor="silver"><input type="text" name="key30" size="90" value="<%=dspKey(30)%>" <%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> maxlength="120" class="dataListEntry" readonly></td>
      </tr>
    </table> 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
       <tr><td bgcolor="lightblue" align="center">合約狀況</td></tr></table>
       <table border="1" width="100%" cellpadding="0" cellspacing="0" >
       <tr>
           <td width="8%" height="23" class="dataListHead" >建置同意書</td>                     
           <td width="15%" height="23" bgcolor="silver">
            <% 
              If Len(Trim(fieldRole(4) &dataProtect)) < 1 Then
                 rdo1=""
                 rdo2=""
              Else
                 rdo1=" disabled "
                 rdo2=" disabled "
              End If
  
                If trim(dspKey(35))="Y" Then rdo1=" checked "    
                If trim(dspKey(35))="N" Then rdo2=" checked " %>                          
        
               <input type="radio" value="Y" <%=RDO1%> name="key35" <%=fieldpg%><%=fieldpa%><%=dataProtec%> readonly><font size=2>有</font>
               <input type="radio" value="N" <%=RDO2%> name="key35" <%=fieldpg%><%=fieldpa%><%=dataProtect%> readonly><font size=2>免</font>
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D1' " 
       If len(trim(dspkey(40))) < 1 Then
          sx=" selected " 
            s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D1' AND CODE='" & dspkey(40) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(40) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
               <select name="key40" <%=fieldpa%><%=FIELDROLE(4)%><%=dataProtect%>  class="dataListEntry">
                    <%=S%>
               </select>
               </td>                              
           <td width="8%" height="23" class="dataListHead" >同意書編號</td>                     
           <td width="8%" height="23" bgcolor="silver">
               <input  class="dataListdata" readonly type="text"  size="7" maxlength="7" name="key36" <%=fieldpg%><%=fieldpa%><%=dataProtec%> value="<%=dspkey(36)%>" readonly></td>          
           <td width="8%" height="23" class="dataListHead" >合作約定書</td>                     
           <td width="15%" height="23" bgcolor="silver">
            <%  
              If Len(Trim(fieldRole(4) &dataProtect)) < 1 Then
                 rdo3=""
                 rdo4=""
              Else
                 rdo3=" disabled "
                 rdo4=" disabled "
              End If

                If trim(dspKey(37))="Y" Then rdo3=" checked "    
                If trim(dspKey(37))="N" Then rdo4=" checked " %>                          
        
               <input type="radio" value="Y" <%=RDO3%> name="key37" <%=fieldRole(4)%><%=fieldpg%><%=fieldpa%><%=dataProtec%> readonly><font size=2>有</font>
               <input type="radio" value="N" <%=RDO4%> name="key37" <%=fieldRole(4)%><%=fieldpg%><%=fieldpa%><%=dataProtect%> readonly><font size=2>無</font>
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D1' " 
       If len(trim(dspkey(41))) < 1 Then
          sx=" selected " 
             s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D1' AND CODE='" & dspkey(41) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(41) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
               <select name="key41" <%=fieldpa%><%=FIELDROLE(4)%><%=dataProtect%>  class="dataListEntry">
                    <%=S%>
               </select>               
               </td>                              
           <td width="8%" height="23" class="dataListHead" >合約編號</td>                     
           <td width="5%" height="23" bgcolor="silver">
               <input  class="dataListdata" readonly type="text" size="7" maxlength="7" name="key38" <%=fieldpg%><%=fieldpa%><%=dataProtec%> value="<%=dspkey(38)%>"></td>          
       
        </tr>                
       </table>
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
       <tr><td bgcolor="lightblue" align="center">匯款資訊</td></tr></table>
       <table border="1" width="100%" cellpadding="0" cellspacing="0" >
       <tr>
           <td width="8%" height="23" class="dataListHead" >匯款同意書</td>                     
           <td width="15%"  height="23" bgcolor="silver">
            <% 
              If Len(Trim(fieldRole(4) &dataProtect)) < 1 Then
                 rdo5=""
                 rdo6=""
              Else
                 rdo5=" disabled "
                 rdo6=" disabled "
              End If
  
                If trim(dspKey(42))="Y" Then rdo5=" checked "    
                If trim(dspKey(42))="N" Then rdo6=" checked " %>                          
        
               <input type="radio" value="Y" <%=RDO5%> name="key42" <%=fieldpg%><%=fieldpa%><%=dataProtec%> readonly><font size=2>有</font>
               <input type="radio" value="N" <%=RDO6%> name="key42" <%=fieldpg%><%=fieldpa%><%=dataProtect%> readonly><font size=2>無</font>
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D1' " 
       If len(trim(dspkey(48))) < 1 Then
          sx=" selected " 
            s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D1' AND CODE='" & dspkey(48) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(48) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
               <select name="key48" <%=fieldpa%><%=FIELDROLE(4)%><%=dataProtect%>  class="dataListEntry">
                    <%=S%>
               </select>
               </td>                              
           <td width="8%" height="23" class="dataListHead" >匯款書編號</td>                     
           <td width="8%" height="23" bgcolor="silver">
               <input  class="dataListdata" readonly type="text"  size="7" maxlength="7" name="key43" <%=fieldpg%><%=fieldpa%><%=dataProtec%> value="<%=dspkey(43)%>" readonly></td>          
        </tr>
      <tr><td width="21%" class="dataListHead">匯款銀行</td>
          <td width="26%" bgcolor="silver"><input type="text" name="key44" size="3" value="<%=dspKey(44)%>" <%=fieldRole(4)%><%=dataProtect%> maxlength="3" class="dataListEntry" readonly>
                                           <input type="text" name="key45" size="4" value="<%=dspKey(45)%>" <%=fieldRole(4)%><%=dataProtect%> maxlength="4" class="dataListEntry" readonly></td>
          <td width="22%" class="dataListHead">匯款帳號</td>
          <td width="31%" bgcolor="silver"><input type="text" name="key46" size="15" value="<%=dspkey(46)%>" <%=fieldRole(4)%><%=dataProtect%> maxlength="15" class="dataListEntry" readonly></td>
      </tr>
      <tr><td width="21%" class="dataListHead">匯款戶名</td>
          <td width="26%" colspan="3" bgcolor="silver"><input type="text" name="key47" size="40" value="<%=dspKey(47)%>" <%=fieldRole(4)%><%=dataProtect%> maxlength="50" class="dataListEntry" readonly></td>
      </TR>        
       </table>       
</td><td>&nbsp</td></tr>
<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>
</table>  
  </div> 
<% 
End Sub 
' --------------------------------------------------------------------------------------------  
%>
<%
'role=0:全部人員皆不可輸入;role=1:業務助理;role=2:業務;role=4:技術部;role=8:財會;role=16:洪協理
'ROLE=31:資訊部管理員
Function FrGetUserRight(program,user)
   Dim Role,i,r
  ' response.write "user=" & user
   dim conn,rs,sql,dsn
   set conn=server.createobject("ADODB.Connection")
   set rs=server.createobject("ADODB.recordset")
   DSN="DSN=RTLib"
   SQL="select authlevel from rtemployee where netid='" & user & "'"
   conn.open dsn
   rs.open sql,conn
   if not rs.eof then
     Role=rs("authlevel")
   else
     Role=0
   end if
   r=""
   For i = 0 To 32
      If Role And i Then 
         r=r &";"
      Else
         r=r &" class=dataListData readonly;"
      End If
   Next
   FrGetUserRight=r
   'response.write "R=" & r & "<BR>"
   rs.close
   conn.close
   set rs=nothing
   set conn=nothing
End Function  
%>
<%
Function SrGetEmployeeRef(RTNValue,OPT,Code)
'-----------------------------------------------------------------
' RTNValue=回傳值
'      OPT=執行項目
'     CODE=傳入參數 
'-----------------------------------------------------------------
'      OPT=1:由(logon user)讀取員工相關資料
'      OPT=2:由(工號)讀取相關資料
' array(0)=Emply(工號)
' array(1)=cusnc(對象名稱)
' array(2)=shortnc(對象簡稱)
' array(3)=CUSID(身份證字號)
'-----------------------------------------------------------------
'      OPT=3:由(logon user)讀取判斷員工資料是否存在
'      OPT=4:由(工號)讀取判斷員工資料是否存在
'      OPT=5:由(logon user)讀取員工部門資料
' rtnvalue="Y":表員工資料存在
'-----------------------------------------------------------------
 rtnvalue=""
 SELECT Case OPT
   Case 1
      set conn=server.CreateObject("ADODB.Connection")
      set rs=server.CreateObject("ADODB.recordset")
      DSN="DSN=RTlib"
      SQL="SELECT RTObj.CUSNC , RTObj.SHORTNC, RTEmployee.EMPLY, " _
         &"RTEmployee.NETID,RTEmployee.CUSID " _
         &"FROM RTObj INNER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID " _
         &"WHERE netid = '" & code & "'"
        ' response.write "SQL=" & SQL
      Conn.Open DSN
      Rs.Open SQL,DSN,1,1
      if rs.recordcount > 0 then
         RTNVALUE= rs("EMPLY") & ";" & rs("CUSNC") & ";" & rs("SHORTNC") & ";" & rs("CUSID")
      else
         rtnvalue=";;;"
      end if
      Rs.Close
      Conn.Close
      Set Rs=Nothing
      Set Conn=Nothing
   Case 2
      set conn=server.CreateObject("ADODB.Connection")
      set rs=server.CreateObject("ADODB.recordset")
      DSN="DSN=RTlib"
      SQL="SELECT RTObj.CUSNC, RTObj.SHORTNC, RTEmployee.EMPLY, " _
         &"RTEmployee.NETID,RTEmployee.CUSID  " _ 
         &"FROM RTObj INNER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID " _
         &"WHERE emply = '" & code & "'"
      Conn.Open DSN
      Rs.Open SQL,DSN,1,1
      If rs.recordcount > 0 then
         RTNVALUE= rs("EMPLY") & ";" & rs("CUSNC") & ";" & rs("SHORTNC") & ";" & rs("CUSID")
      else
         Rtnvalue=";;;"
      end if
      Rs.Close
      Conn.Close
      Set Rs=Nothing
      Set Conn=Nothing
   Case 3
      set conn=server.CreateObject("ADODB.Connection")
      set rs=server.CreateObject("ADODB.recordset")
      DSN="DSN=RTlib"
      SQL="SELECT RTObj.CUSNC , RTObj.SHORTNC, RTEmployee.EMPLY, " _
         &"RTEmployee.NETID,RTEmployee.CUSID  " _
         &"FROM RTObj INNER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID " _
         &"WHERE netid = '" & code & "'"
      Conn.Open DSN
      Rs.Open SQL,DSN,1,1
      if rs.recordcount > 0 then
         RTNVALUE= "Y"
      else
         rtnvalue=""
      end if
      Rs.Close
      Conn.Close
      Set Rs=Nothing
      Set Conn=Nothing      
   Case 4
      set conn=server.CreateObject("ADODB.Connection")
      set rs=server.CreateObject("ADODB.recordset")
      DSN="DSN=RTlib"
      SQL="SELECT RTObj.CUSNC, RTObj.SHORTNC, RTEmployee.EMPLY, " _
         &"RTEmployee.NETID,RTEmployee.CUSID  " _ 
         &"FROM RTObj INNER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID " _
         &"WHERE emply = '" & code & "'"
      Conn.Open DSN
      Rs.Open SQL,DSN,1,1
      If rs.recordcount > 0 then
         RTNVALUE= "Y"
      else
         Rtnvalue=""
      end if
      Rs.Close
      Conn.Close
      Set Rs=Nothing
      Set Conn=Nothing      
   Case 5
      set conn=server.CreateObject("ADODB.Connection")
      set rs=server.CreateObject("ADODB.recordset")
      DSN="DSN=RTlib"
      SQL="SELECT RTObj.CUSNC, RTObj.SHORTNC, RTEmployee.EMPLY, " _
         &"RTEmployee.NETID,RTEmployee.CUSID,rtemployee.dept  " _ 
         &"FROM RTObj INNER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID " _
         &"WHERE netid = '" & code & "'"
      Conn.Open DSN
      Rs.Open SQL,DSN,1,1
      If rs.recordcount > 0 then
         RTNVALUE= rs("dept")
      else
         Rtnvalue=""
      end if
      Rs.Close
      Conn.Close
      Set Rs=Nothing
      Set Conn=Nothing            
   Case Else
 
 End Select
End Function
%>