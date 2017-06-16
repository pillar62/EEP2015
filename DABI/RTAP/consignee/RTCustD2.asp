<%
  Dim fieldRole,fieldPa,fieldPb,fieldpc,fieldpd,fieldpe
  fieldRole=Split(FrGetUserRight("RTCustD",Request.ServerVariables("LOGON_USER")),";")
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  
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
                 case ucase("/webap/rtap/base/rtcmty/RTCmtyd.asp")
                     if i<>0 AND I <> 52 and i <> 54 and i <> 59 then rs.Fields(i).Value=dspKey(i)
                     '取建置同意書及合約書之現存編號最大值(賦予新編號時兩者之編號欲相同之故
                     if i=52 or i=54 or i=59 then
                        temp52=""
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(agreeno) AS agreeno from rtcmty "
                        rsc.open sqlstr2,conn
                        if len(trim(rsc("agreeno"))) > 0 then
                           temp52="BA" & right("00000" & cstr(cint(mid(rsc("agreeno"),3,5))+1),5)
                        else
                           temp52="BA00001"
                        end if
                        temp52A=mid(temp52,3,5)
                        '----
                        temp54=""
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(contractno) AS contractno from rtcmty "
                        rsc.open sqlstr2,conn
                        if len(trim(rsc("contractno"))) > 0 then
                           temp54="BB" & right("00000" & cstr(cint(mid(rsc("contractno"),3,5))+1),5)
                        else
                           temp54="BB00001"
                        end if
                        temp54A=mid(temp54,3,5)        
                        '----
                        temp59=""
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(remitno) AS remitno from rtcmty "
                        rsc.open sqlstr2,conn
                        if len(trim(rsc("remitno"))) > 0 then
                           temp59="BC" & right("00000" & cstr(cint(mid(rsc("REMITNO"),3,5))+1),5)
                        else
                           temp59="BC00001"
                        end if
                        temp59A=mid(temp59,3,5)                                
                     '----   
                       maxtemp=temp52A
                       if temp54A > maxtemp then maxtemp=temp54A
                       if temp59A > maxtemp then maxtemp=temp59A
                                       
                       if dspkey(51)="Y" AND LEN(TRIM(DSPKEY(52)))=0 then dspkey(52)="BA" & maxtemp
                       if dspkey(53)="Y" AND LEN(TRIM(DSPKEY(54)))=0 then dspkey(54)="BB" & maxtemp
                       if dspkey(58)="Y" AND LEN(TRIM(DSPKEY(59)))=0 then dspkey(59)="BC" & maxtemp                       
                       rs.Fields(i).Value=dspKey(i)
                     end if                                       
                 ' 當程式為ADSL社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtADSLcmty/RTCmtyd.asp")
                     if i<>0  AND i <> 36 and i<> 38 and i<> 43 then rs.Fields(i).Value=dspKey(i)                
                     '取建置同意書及合約書之現存編號最大值(賦予新編號時兩者之編號欲相同之故
                     if i=36 or i=38 or i=43 then
                     '   response.write "I=" & I & "<BR>"
                        temp36=""
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(agreeno) AS agreeno from rtcustadslcmty "
                        rsc.open sqlstr2,conn
                        if len(trim(rsc("agreeno"))) > 0 then
                           temp36="AA" & right("00000" & cstr(cint(mid(rsc("agreeno"),3,5))+1),5)
                        else
                           temp36="AA00001"
                        end if
                        temp36A=mid(temp36,3,5)
                        '----
                        temp38=""
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(contractno) AS contractno from rtcustadslcmty "
                        rsc.open sqlstr2,conn
                        if len(trim(rsc("contractno"))) > 0 then
                           temp38="AB" & right("00000" & cstr(cint(mid(rsc("contractno"),3,5))+1),5)
                        else
                           temp38="AB00001"
                        end if
                        temp38A=mid(temp38,3,5)        
                        '----
                        temp43=""
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(remitno) AS remitno from rtcustadslcmty "
                        rsc.open sqlstr2,conn
                        if len(trim(rsc("remitno"))) > 0 then
                           temp43="AC" & right("00000" & cstr(cint(mid(rsc("remitno"),3,5))+1),5)
                        else
                           temp43="AC00001"
                        end if
                        temp43A=mid(temp43,3,5)                                
                     '----                   
                       maxtemp=temp36A
                       if temp38A > maxtemp then maxtemp=temp38A
                       if temp43A > maxtemp then maxtemp=temp43A
                                       
                       if dspkey(35)="Y" AND LEN(TRIM(DSPKEY(36)))=0 then dspkey(36)="AA" & maxtemp
                       if dspkey(37)="Y" AND LEN(TRIM(DSPKEY(38)))=0 then dspkey(38)="AB" & maxtemp
                       if dspkey(42)="Y" AND LEN(TRIM(DSPKEY(43)))=0 then dspkey(43)="AC" & maxtemp                     
                       rs.Fields(i).Value=dspKey(i)
                     end if                          
                 ' 當程式為ADSL社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtcmtyADSL/RTCmtyd.asp")
                     if i<>0 then rs.Fields(i).Value=dspKey(i)    
                 ' 當程式為速博ADSL社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtsparqADSLcmty/RTCmtyd.asp")
                     if i<>0 then rs.Fields(i).Value=dspKey(i)                         

                 ' 當程式為客戶基本資料維護作業時,因其dspkey(2)為自動搶號欄位(max+1)，故不搬入值（由程式控制產生)   
                 case ucase("/webap/rtap/base/rtcmty/RTCustD.asp")
                     if i=2 then
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(entryno) AS entryno from rtcust where  cusid='" & dspkey(1) & "'"
                        rsc.open "select max(entryno) AS entryno from rtcust where  cusid='" & dspkey(1) & "'",conn
                        if len(rsc("entryno")) > 0 then
                           dspkey(i)=rsc("entryno") + 1
                        else
                           dspkey(i)=1
                        end if
                        rsc.close
                      end if
                      rs.fields(i).value=dspkey(i)
                 ' 當程式為ADSL客戶基本資料維護作業時,因其dspkey(2)為自動搶號欄位(max+1)，故不搬入值（由程式控制產生)   
                 case ucase("/webap/rtap/base/rtcmtyADSL/RTCustD.asp")
                     if i=2 then
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(entryno) AS entryno from rtcustADSL where  cusid='" & dspkey(1) & "'"
                        rsc.open sqlstr2,conn
                        if len(rsc("entryno")) > 0 then
                           dspkey(i)=rsc("entryno") + 1
                        else
                           dspkey(i)=1
                        end if
                        rsc.close
                      end if
                      rs.fields(i).value=dspkey(i)         
                 ' 當程式為客訴資料維護作業時,因其dspkey(0)為自動搶號欄位(yymmdd+SEQ)，故不搬入值（由程式控制產生)   
                 case ucase("/webap/rtap/base/rtcmty/RTFAQD.asp")  
                      if i=0 then  
                         YY=cstr(datepart("yyyy",now())-1911)
                         mm=right(cstr("0" & cstr(datepart("m",now()))),2)
                         dd=right(cstr("0" & cstr(datepart("d",now()))),2)
                         YYMMDD=yy & mm & dd
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         sqlstr2="select max(caseno) AS caseno from rtfaqh where  caseno like '" & yymmdd & "%'" 
                         rsc.open sqlstr2,conn
                         if len(rsc("caseno")) > 0 then
                            dspkey(i)=yymmdd & right("0000" & cstr(cint(mid(rsc("caseno"),7,4)) + 1),4)
                         else
                            dspkey(i)=yymmdd & "0001"
                         end if                                                             
                      end if

                      rs.Fields(i).Value=dspKey(i)
                 ' 當程式為客訴處理措施紀錄時,因其dspkey(1)為identify，故不搬入值（由程式控制產生)   
                 case ucase("/webap/rtap/base/rtcmty/RTFaqprocessD.asp")
                     if i<>1 then rs.Fields(i).Value=dspKey(i)                   
                 ' 當程式為客戶基本資料維護(業務),因其dspkey(1)為自動搶號欄位(max+1)，故不搬入值（由程式控制產生)   
                 case ucase("/webap/rtap/base/rtCUSTTEMP/RTCustD.asp")
                     if i=1 then
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(entryno) AS entryno from rtcusttemp where  cusid='" & dspkey(0) & "'"
                        rsc.open sqlstr2,conn
                        if len(rsc("entryno")) > 0 then
                           dspkey(i)=rsc("entryno") + 1
                        else
                           dspkey(i)=1
                        end if
                        rsc.close
                      end if
                      rs.fields(i).value=dspkey(i)          
                 ' 當程式為ADSL客戶基本資料維護作業時,因其dspkey(77)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtcustadsl/RTCustd.asp")
                      if i=1 then
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(entryno) AS entryno from rtcustadsl where  cusid='" & dspkey(0) & "'"
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
                   
                      if i=77 then
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
                        sqlstr2="select max(orderno) AS orderno from rtcustadsl where  orderno like '" & area & "%'"
                        rsc.open sqlstr2,conn
                        if len(rsc("orderno")) > 0 then
                           dspkey(i)=area & right("000000" & cstr(cint(mid(rsc("orderno"),2,6)) + 1),6)
                        else
                           dspkey(i)=area & "000001"
                        end if
                        rsc.close
                      end if       
                      
                      rs.Fields(i).Value=dspKey(i)    
                 ' 當程式為ADSL客戶基本資料維護作業時,因其dspkey(77)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtcustadslBRANCH/RTCustd.asp")
                      if i=1 then
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(entryno) AS entryno from rtcustadsl where  cusid='" & dspkey(0) & "'"
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
                   
                      if i=77 then
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
                        sqlstr2="select max(orderno) AS orderno from rtcustadsl where  orderno like '" & area & "%'"
                        rsc.open sqlstr2,conn
                        if len(rsc("orderno")) > 0 then
                           dspkey(i)=area & right("000000" & cstr(cint(mid(rsc("orderno"),2,6)) + 1),6)
                        else
                           dspkey(i)=area & "000001"
                        end if
                        rsc.close
                      end if       
                      
                      rs.Fields(i).Value=dspKey(i)               
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
                 ' 當程式為ADSL客戶基本資料維護作業(社區下新增)時,因其dspkey(78)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtADSLCMTY/RTCustd.asp")
                      if i=2 then
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(entryno) AS entryno from rtcustadsl where  cusid='" & dspkey(1) & "'"
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

                      if i=78 then
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
                        sqlstr2="select max(orderno) AS orderno from rtcustadsl where  orderno like '" & area & "%'"
                        rsc.open sqlstr2,conn
                        if len(rsc("orderno")) > 0 then
                           dspkey(i)=area & right("000000" & cstr(cint(mid(rsc("orderno"),2,6)) + 1),6)
                        else
                           dspkey(i)=area & "000001"
                        end if
                        rsc.close
                      end if       
                    '  response.write "i=" & i & ";dspkey(i)=" & dspkey(i) & "<Br>"
                      rs.Fields(i).Value=dspKey(i)                                          
                 ' 當程式為ADSL客訴資料維護作業時,因其dspkey(0)為自動搶號欄位(yymmdd+SEQ)，故不搬入值（由程式控制產生)   
                 case ucase("/webap/rtap/base/rtcustadsl/RTFAQD.asp")  
                      if i=0 then  
                         YY=cstr(datepart("yyyy",now())-1911)
                         mm=right(cstr("0" & cstr(datepart("m",now()))),2)
                         dd=right(cstr("0" & cstr(datepart("d",now()))),2)
                         YYMMDD=yy & mm & dd
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         sqlstr2="select max(caseno) AS caseno from rtfaqh where  caseno like '" & yymmdd & "%'" 
                         rsc.open sqlstr2,conn
                         if len(rsc("caseno")) > 0 then
                            dspkey(i)=yymmdd & right("0000" & cstr(cint(mid(rsc("caseno"),7,4)) + 1),4)
                         else
                            dspkey(i)=yymmdd & "0001"
                         end if                                                             
                      end if
                      rs.Fields(i).Value=dspKey(i)         
                 ' 當程式為ADSL客訴資料維護作業時,因其dspkey(0)為自動搶號欄位(yymmdd+SEQ)，故不搬入值（由程式控制產生)   
                 case ucase("/webap/rtap/base/rtcustadslBRANCH/RTFAQD.asp")  
                      if i=0 then  
                         YY=cstr(datepart("yyyy",now())-1911)
                         mm=right(cstr("0" & cstr(datepart("m",now()))),2)
                         dd=right(cstr("0" & cstr(datepart("d",now()))),2)
                         YYMMDD=yy & mm & dd
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         sqlstr2="select max(caseno) AS caseno from rtfaqh where  caseno like '" & yymmdd & "%'" 
                         rsc.open sqlstr2,conn
                         if len(rsc("caseno")) > 0 then
                            dspkey(i)=yymmdd & right("0000" & cstr(cint(mid(rsc("caseno"),7,4)) + 1),4)
                         else
                            dspkey(i)=yymmdd & "0001"
                         end if                                                             
                      end if
                         '   response.write "i=" & i & ";dspkey(i)=" & dspkey(i) & "<Br>"
                      rs.Fields(i).Value=dspKey(i)                         
                 ' 當程式為ADSL客訴資料維護作業時(社區下),因其dspkey(0)為自動搶號欄位(yymmdd+SEQ)，故不搬入值（由程式控制產生)   
                 case ucase("/webap/rtap/base/rtADSLCMTY/RTFAQD.asp")  
                      if i=0 then  
                         YY=cstr(datepart("yyyy",now())-1911)
                         mm=right(cstr("0" & cstr(datepart("m",now()))),2)
                         dd=right(cstr("0" & cstr(datepart("d",now()))),2)
                         YYMMDD=yy & mm & dd
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         sqlstr2="select max(caseno) AS caseno from rtfaqh where  caseno like '" & yymmdd & "%'" 
                         rsc.open sqlstr2,conn
                         if len(rsc("caseno")) > 0 then
                            dspkey(i)=yymmdd & right("0000" & cstr(cint(mid(rsc("caseno"),7,4)) + 1),4)
                         else
                            dspkey(i)=yymmdd & "0001"
                         end if                                                             
                      end if
                      rs.Fields(i).Value=dspKey(i)                                                 
                 ' 當程式為ADSL客訴處理措施紀錄時,因其dspkey(1)為identify，故不搬入值（由程式控制產生)   
                 case ucase("/webap/rtap/base/rtcustadsl/RTFaqprocessD.asp")
                     if i<>1 then rs.Fields(i).Value=dspKey(i)            
                 ' 當程式為ADSL客訴處理措施紀錄時(社區下),因其dspkey(1)為identify，故不搬入值（由程式控制產生)   
                 case ucase("/webap/rtap/base/rtADSLCMTY/RTFaqprocessD.asp")
                     if i<>1 then rs.Fields(i).Value=dspKey(i)                
                 ' 當程式為ADSL客訴處理措施紀錄時(社區下),因其dspkey(1)為identify，故不搬入值（由程式控制產生)   
                 case ucase("/webap/rtap/base/rtcustadslbranch/RTFaqprocessD.asp")
                     if i<>1 then rs.Fields(i).Value=dspKey(i)                                                      
                 ' 當程式為ADSL客戶(營運處獨享)基本資料維護作業時,因其dspkey(77)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/singlecustadsl/RTCustd.asp")
                      if i=1 then
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(entryno) AS entryno from singlecustadsl where  cusid='" & dspkey(0) & "'"
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
                   
                      if i=77 then
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
                        sqlstr2="select max(orderno) AS orderno from singlecustadsl where  orderno like '" & area & "%'"
                        rsc.open sqlstr2,conn
                        if len(rsc("orderno")) > 0 then
                           dspkey(i)=area & right("000000" & cstr(cint(mid(rsc("orderno"),2,6)) + 1),6)
                        else
                           dspkey(i)=area & "000001"
                        end if
                        rsc.close
                      end if       
                      
                      rs.Fields(i).Value=dspKey(i)    
                 ' 當程式為ADSL(營運處-獨享)客訴資料維護作業時,因其dspkey(0)為自動搶號欄位(yymmdd+SEQ)，故不搬入值（由程式控制產生)   
                 case ucase("/webap/rtap/base/singlecustadsl/RTFAQD.asp")  
                      if i=0 then  
                         YY=cstr(datepart("yyyy",now())-1911)
                         mm=right(cstr("0" & cstr(datepart("m",now()))),2)
                         dd=right(cstr("0" & cstr(datepart("d",now()))),2)
                         YYMMDD=yy & mm & dd
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         sqlstr2="select max(caseno) AS caseno from rtfaqh where  caseno like '" & yymmdd & "%'" 
                         rsc.open sqlstr2,conn
                         if len(rsc("caseno")) > 0 then
                            dspkey(i)=yymmdd & right("0000" & cstr(cint(mid(rsc("caseno"),7,4)) + 1),4)
                         else
                            dspkey(i)=yymmdd & "0001"
                         end if                                                             
                      end if
                      rs.Fields(i).Value=dspKey(i)                        
                 ' 當程式為先看先贏,因其dspkey(2)為IDENTIFY欄位，故不搬入值（由程式控制產生)   
                 case ucase("/webap/rtap/RTSS365/RTDELIVERCUST/RTTELVISITD.asp")  
                  '    response.write "i=" & i & ";dspkey(i)=" & dspkey(i) & "<Br>"
                      if i<>2 then rs.Fields(i).Value=dspKey(i)                
                 ' 當程式為ADSL(營運處-獨享)客訴處理措施紀錄時,因其dspkey(1)為identify，故不搬入值（由程式控制產生)   
                 case ucase("/webap/rtap/base/singlecustadsl/RTFaqprocessD.asp")
                     if i<>1 then rs.Fields(i).Value=dspKey(i)               
                 ' 當程式為社區重大訊息維護時,因其dspkey(2)為seqno，故不搬入值（由程式控制產生)   
                 case ucase("/webap/rtap/base/rthbadslcmty/RTcmtymsgd.asp")
                     if i<>2 then
                        rs.Fields(i).Value=dspKey(i)     
                     else    
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(entryno) AS entryno from rtcmtymsg where  comq1=" & dspkey(0) & " and kind='" & dspkey(1) & "' " 
                        rsc.open sqlstr2,conn
                        if len(trim(rsc("ENTRYNO"))) > 0 then
                           dspkey(i)=rsc("ENTRYNO") + 1
                        else
                           dspkey(i)=1
                        end if
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
              select case ucase(runpgm)   
                 ' 當程式為社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtcmty/RTCmtyd.asp")
                     if i<>0 AND I <> 52 and i <> 54 AND i <> 59 then rs.Fields(i).Value=dspKey(i)
                     '取建置同意書及合約書之現存編號最大值(賦予新編號時兩者之編號欲相同之故
                     if i=52 or i=54 or i=59 then
                        temp52=""
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(agreeno) AS agreeno from rtcmty "
                        rsc.open sqlstr2,conn
                        if len(trim(rsc("agreeno"))) > 0 then
                           temp52="BA" & right("00000" & cstr(cint(mid(rsc("agreeno"),3,5))+1),5)
                        else
                           temp52="BA00001"
                        end if
                        temp52A=mid(temp52,3,5)
                        '----
                        temp54=""
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(contractno) AS contractno from rtcmty "
                        rsc.open sqlstr2,conn
                        if len(trim(rsc("contractno"))) > 0 then
                           temp54="BB" & right("00000" & cstr(cint(mid(rsc("contractno"),3,5))+1),5)
                        else
                           temp54="BB00001"
                        end if
                        temp54A=mid(temp54,3,5)        
                        '----
                        temp59=""
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(REMITNO) AS REMITNO from rtcmty "
                        rsc.open sqlstr2,conn
                        if len(trim(rsc("REMITNO"))) > 0 then
                           temp59="BC" & right("00000" & cstr(cint(mid(rsc("remitno"),3,5))+1),5)
                        else
                           temp59="BC00001"
                        end if
                        temp59A=mid(temp59,3,5)                                
                     '----                   
                        maxtemp=temp52A
                        if temp54A > maxtemp then maxtemp=temp54A
                        if temp59A > maxtemp then maxtemp=temp59A
                                       
                        if dspkey(51)="Y" AND LEN(TRIM(DSPKEY(52)))=0 then dspkey(52)="BA" & maxtemp
                        if dspkey(53)="Y" AND LEN(TRIM(DSPKEY(54)))=0 then dspkey(54)="BB" & maxtemp
                        if dspkey(58)="Y" AND LEN(TRIM(DSPKEY(59)))=0 then dspkey(59)="BC" & maxtemp                       

                       rs.Fields(i).Value=dspKey(i)
                     end if                                        
                 ' 當程式為ADSL社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtADSLcmty/RTCmtyd.asp")
                     if i<>0 then rs.Fields(i).Value=dspKey(i)         
                     '取建置同意書及合約書之現存編號最大值(賦予新編號時兩者之編號欲相同之故
                     if i=36 or i=38 or i=43 then
                        temp36=""
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(agreeno) AS agreeno from rtcustadslcmty "
                        rsc.open sqlstr2,conn
                        if len(trim(rsc("agreeno"))) > 0 then
                           temp36="AA" & right("00000" & cstr(cint(mid(rsc("agreeno"),3,5))+1),5)
                        else
                           temp36="AA00001"
                        end if
                        temp36A=mid(temp36,3,5)
                        '----
                        temp38=""
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(contractno) AS contractno from rtcustadslcmty "
                        rsc.open sqlstr2,conn
                        if len(trim(rsc("contractno"))) > 0 then
                           temp38="AB" & right("00000" & cstr(cint(mid(rsc("contractno"),3,5))+1),5)
                        else
                           temp38="AB00001"
                        end if
                        temp38A=mid(temp38,3,5)      
                        '----
                        temp43=""
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        sqlstr2="select max(remitno) AS remitno from rtcustadslcmty "
                        rsc.open sqlstr2,conn
                        if len(trim(rsc("remitno"))) > 0 then
                           temp43="AC" & right("00000" & cstr(cint(mid(rsc("remitno"),3,5))+1),5)
                        else
                           temp43="AC00001"
                        end if
                        temp43A=mid(temp43,3,5)                                
                     '----                   
                       maxtemp=temp36A
                       if temp38A > maxtemp then maxtemp=temp38A
                       if temp43A > maxtemp then maxtemp=temp43A
                                       
                       if dspkey(35)="Y" AND LEN(TRIM(DSPKEY(36)))=0 then dspkey(36)="AA" & maxtemp
                       if dspkey(37)="Y" AND LEN(TRIM(DSPKEY(38)))=0 then dspkey(38)="AB" & maxtemp
                       if dspkey(42)="Y" AND LEN(TRIM(DSPKEY(43)))=0 then dspkey(43)="AC" & maxtemp       
                       rs.Fields(i).Value=dspKey(i)
                     end if                                                     
                 ' 當程式為ADSL社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtcmtyADSL/RTCmtyd.asp")
                     if i<>0 then rs.Fields(i).Value=dspKey(i)            
                 ' 當程式為速博ADSL社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtsparqADSLcmty/RTCmtyd.asp")
                     if i<>0 then rs.Fields(i).Value=dspKey(i)                                       
                 ' 當程式為客訴處理措施紀錄時,因其dspkey(1)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtcmty/RTfaqprocessd.asp")
                     if i<>1 then rs.Fields(i).Value=dspKey(i)               
                 ' 當程式為adsl客訴處理措施紀錄時,因其dspkey(1)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtadslcmty/RTfaqprocessd.asp")
                     if i<>1 then rs.Fields(i).Value=dspKey(i)                           
                 ' 當程式為adsl客訴處理措施紀錄時,因其dspkey(1)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtcustadslbranch/RTfaqprocessd.asp")
                     if i<>1 then rs.Fields(i).Value=dspKey(i)                                                   
                 ' 當程式為ADSL客戶基本資料維護作業時,因其dspkey(77)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtcustADSL/RTcustd.asp")
                     'if i<>77 then rs.Fields(i).Value=dspKey(i)  
                     'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                     rs.Fields(i).Value=dspKey(i)  
                 ' 當程式為ADSL客戶基本資料維護作業時,因其dspkey(77)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtcustADSLBRANCH/RTcustd.asp")
                     'if i<>77 then rs.Fields(i).Value=dspKey(i)  
                     'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                     rs.Fields(i).Value=dspKey(i)        
                 ' 當程式為速博ADSL客戶基本資料維護作業時,因其dspkey(77)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtsparqADSLcust/RTcustd.asp")
                     'if i<>76 then rs.Fields(i).Value=dspKey(i)  
                     'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                     rs.Fields(i).Value=dspKey(i)                                             
                 ' 當程式為ADSL(營運處-獨享)客戶基本資料維護作業時,因其dspkey(77)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/rtcustADSL/RTcustd.asp")
                     'if i<>77 then rs.Fields(i).Value=dspKey(i)  
               '      response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                     rs.Fields(i).Value=dspKey(i)     
               ' 當程式為先看先贏資料維護作業時,因其dspkey(2)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/RTSS365/rtDELIVERCUST/RTTELVISITD.asp")
                     if i<>2 then rs.Fields(i).Value=dspKey(i)                    
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
    runpgm=Request.ServerVariables("PATH_INFO")
    if ucase(runpgm)=ucase("/webap/rtap/base/rtcmty/RTCmtyd.asp") then
       rs.open "select max(comq1) AS comq1 from rtcmty",conn
       if not rs.eof then
          dspkey(0)=rs("comq1")
       end if
       rs.close
    end if
    ' 當程式為ADSL社區基本資料維護作業時,將sql自行產生之identify值dspkey(0)讀出至畫面
        'response.write "accessmode=" & accessmode &";sw=" & sw & "<BR>"
    if accessmode ="A" then
       runpgm=Request.ServerVariables("PATH_INFO")
       if ucase(runpgm)=ucase("/webap/rtap/base/rtADSLcmty/RTCmtyd.asp") then
          rs.open "select max(CUTYID) AS cutyid from rtCUSTADSLcmty",conn
          if not rs.eof then
             dspkey(0)=rs("CUTYID")
          end if
          rs.close
       end if
    end if    
    ' 當程式為ADSL社區基本資料維護作業時,將sql自行產生之identify值dspkey(0)讀出至畫面
    runpgm=Request.ServerVariables("PATH_INFO")
    if ucase(runpgm)=ucase("/webap/rtap/base/rtcmtyADSL/RTCmtyd.asp") then
       rs.open "select max(comq1) AS comq1 from rtcmtyADSL",conn
       if not rs.eof then
          dspkey(0)=rs("comq1")
       end if
       rs.close
    end if    
   ' 當程式為客訴處理措施紀錄時,因其dspkey(1)為identify欄位，故不搬入值（由sql自行產生)
    if ucase(runpgm)=ucase("/webap/rtap/base/rtcmty/RTfaqprocessd.asp") then
       rs.open "select max(entryno) AS entryno from rtfaqd1",conn
       if not rs.eof then
          dspkey(1)=rs("entryno")
       end if
       rs.close
    end if    
   ' 當程式為ADSL客訴處理措施紀錄時,因其dspkey(1)為identify欄位，故不搬入值（由sql自行產生)
    if ucase(runpgm)=ucase("/webap/rtap/base/rtcustadslbranch/RTfaqprocessd.asp") then
       rs.open "select max(entryno) AS entryno from rtfaqd1",conn
       if not rs.eof then
          dspkey(1)=rs("entryno")
       end if
       rs.close
    end if        
    ' 當程式為先看先贏維護作業時,將sql自行產生之identify值dspkey(2)讀出至畫面
    runpgm=Request.ServerVariables("PATH_INFO")
    if ucase(runpgm)=ucase("/webap/rtap/RTSS365/RTDELIVERCUST/RTTELVISITD.asp") then
       rs.open "select max(seq1) AS SEQ1 from rtSS365TEL",conn
       if not rs.eof then
          dspkey(2)=rs("SEQ1")
       end if
       rs.close
    end if    
    ' 當程式為adsl客戶基本資料維護作業時,將sql自行產生之identify值dspkey(77)讀出至畫面
   ' runpgm=Request.ServerVariables("PATH_INFO")
   ' if ucase(runpgm)=ucase("/webap/rtap/base/rtcustadsl/RTCustd.asp") then
   '    rs.open "select max(orderno) AS orderno from rtcustadsl",conn
   '    if not rs.eof then
   '       dspkey(77)=rs("orderno")
   '    end if
   '    rs.close
   ' end if    
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
  numberOfKey=2
  title="速博ADSL客戶基本資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  'sqlFormatDB="SELECT * FROM RTCust WHERE Comq1=0 "
  sqlFormatDB="SELECT comq1,CUSID, ENTRYNO,STOCKID,BRANCH,BUSSMAN,BUSSID,SEX,BIRTHDAY, " _
             &"cutid1,township1,raddr1,rzone1,cutid2,township2,raddr2,rzone2, " _
             &"cutid3,township3,raddr3,rzone3,SPEED,LINETYPE,USEKIND, " _
             &"HOUSETYPE,HOUSENAME,HOUSEQTY,exttel,HOME,FAX,CONTACT,OFFICE, EXTENSION, MOBILE, EMAIL, " _
             &"VOUCHER, EUSR,EDAT,UUSR,UDAT,PROFAC,SNDINFODAT, REQDAT, INSPRTNO, INSPRTDAT, INSPRTUSR,  " _
             &"FINISHDAT, DOCKETDAT, INCOMEDAT, AR, ACTRCVAMT, DROPDAT, RCVDTLNO,  " _
             &"RCVDTLPRT, SCHDAT, FINRDFMDAT, FINCFMUSR, BONUSCAL, DROPDESC, " _
             &"UNFINISHDESC, PAYDTLPRTNO, PAYDTLDAT, PAYDTLUSR, ACCCFMDAT, " _
             &"ACCCFMUSR, ENDCOD, NOTE,OPERENVID, SETTYPE, " _
             &"SETSALES, PRESETDATE, PRESETHOUR, PRESETMIN, SETFEE, SETFEEDIFF, " _
             &"SETFEEDESC,orderno,Lookdat,formaldat,deliverdat,socialid,agree,haveroom,homestat, " _
             &"LOOKDESC,CHTSIGNDAT,SENDWORKING,WORKINGREPLY,cusno,transdat,holdemail,proposer,SPHNNO,ip,cotport  " _
             &"FROM rtsparqADSLcust where cusid='*'"
           
  sqllist    ="SELECT comq1,CUSID, ENTRYNO,STOCKID,BRANCH,BUSSMAN,BUSSID,SEX,BIRTHDAY, " _
             &"cutid1,township1,raddr1,rzone1,cutid2,township2,raddr2,rzone2, " _
             &"cutid3,township3,raddr3,rzone3,SPEED,LINETYPE,USEKIND, " _
             &"HOUSETYPE,HOUSENAME,HOUSEQTY,exttel,HOME,FAX,CONTACT,OFFICE, EXTENSION, MOBILE, EMAIL, " _
             &"VOUCHER, EUSR,EDAT,UUSR,UDAT,PROFAC,SNDINFODAT, REQDAT, INSPRTNO, INSPRTDAT, INSPRTUSR,  " _
             &"FINISHDAT, DOCKETDAT, INCOMEDAT, AR, ACTRCVAMT, DROPDAT, RCVDTLNO,  " _
             &"RCVDTLPRT, SCHDAT, FINRDFMDAT, FINCFMUSR, BONUSCAL, DROPDESC, " _
             &"UNFINISHDESC, PAYDTLPRTNO, PAYDTLDAT, PAYDTLUSR, ACCCFMDAT, " _
             &"ACCCFMUSR, ENDCOD, NOTE,OPERENVID, SETTYPE, " _
             &"SETSALES, PRESETDATE, PRESETHOUR, PRESETMIN, SETFEE, SETFEEDIFF, " _
             &"SETFEEDESC,orderno,Lookdat,formaldat,deliverdat,socialid,agree,haveroom,homestat, " _
             &"LOOKDESC,CHTSIGNDAT,SENDWORKING,WORKINGREPLY,cusno,transdat,holdemail,proposer,SPHNNO,ip,cotport  " _
             &"FROM rtsparqADSLcust where "
 ' Response.write "SQL=" & SQLlist
 ' Response.end            
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=1
  userDefineRead="Yes"
  userDefineSave="Yes"
  userdefineactivex="Yes"  
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
'------社區序號---固定(由上層程式傳入) 
   ' dspkey(90)=SESSION("comq1")
'------社區名稱---固定(由社區序號讀取)
    Set connxx=Server.CreateObject("ADODB.Connection")  
    Set rsxx=Server.CreateObject("ADODB.Recordset")
    DSNXX="DSN=RTLib"
    connxx.Open DSNxx
    sqlXX="SELECT COMN,consignee FROM RTsparqAdslCmty where cutyid=" & dspkey(0)
    rsxx.Open sqlxx,connxx
    s=""
    If rsxx.Eof Then
       message="社區代號:" &dspkey(0) &"在社區基本資料內找不到"
       formvalud=false
       consigneeXX=""       
    Else 
       dspkey(25)=rsxx("ComN") 
       consigneeXX=rsXX("consignee")    
    End If
    rsxx.Close
    Set rsxx=Nothing
    connxx.Close
    Set connxx=Nothing    
    if len(trim(dspkey(71))) = 0 then dspkey(71)=0
    if len(trim(dspkey(72))) = 0 then dspkey(72)=0
'-------單次------------------------------
    If Not IsNumeric(dspKey(2)) Then dspKey(2)=0
'-------戶數------------------------------
    If Not IsNumeric(dspKey(26)) or len(trim(dspkey(26))) = 0 Then dspKey(26)=0    
'--------------- -------------------------
    If Not IsNumeric(dspKey(49)) Then dspKey(49)=0   '應收金額
    If Not IsNumeric(dspKey(50)) Then dspKey(50)=0   '實收金額 
    If Not IsNumeric(dspKey(68)) Then dspKey(68)=3   '安裝類別
    If Not IsNumeric(dspKey(73)) Then dspKey(73)=0   '標準施工費
    If Not IsNumeric(dspKey(74)) Then dspKey(74)=0   '施工補助費   
    If Not IsNumeric(dspKey(71)) Then dspKey(71)=0   '裝機(時)
    If Not IsNumeric(dspKey(72)) Then dspKey(72)=0   '裝機(分)     
    If len(trim(dspkey(0))) < 1 then
       message="請入客戶代碼"
       formValid=False
    elseIf dspKey(71) > 24 Or dspKey(72) > 59 Then
       message="請輸入正確預定裝機時間"
       formValid=False
    elseif len(trim(extdb(0))) < 1 then
       message="請輸入客戶名稱"
       formValid=False    
    elseif len(trim(dspkey(6)))=0 and len(trim(Consigneexx)) = 0 then
       message="社區檔之經銷商欄位與業務員不可同時空白!"
       formValid=False
    elseif not Isdate(dspkey(8)) and len(dspkey(8)) > 0 then
       message="出生日期錯誤"
       formValid=False            
    elseif not Isdate(dspkey(41)) and len(dspkey(41))  > 0 then
       message="通知發包日期錯誤"
       formValid=False     
    elseif not Isdate(dspkey(42)) and len(dspkey(42))  > 0 then
       message="發包日期錯誤"
       formValid=False            
    elseif not Isdate(dspkey(46)) and len(dspkey(46))  > 0 then
       message="完工日期錯誤"
       formValid=False     
    elseif not Isdate(dspkey(47)) and len(dspkey(47))  > 0 then
       message="報竣日期錯誤"
       formValid=False             
    elseif not IsNumeric(dspkey(49)) and len(dspkey(49))  > 0 then
       message="應收金額錯誤"
       formValid=False           
    elseif not IsNumeric(dspkey(50)) and len(dspkey(50))  > 0 then
       message="實收金額錯誤"
       formValid=False             
    elseif not Isdate(dspkey(51)) and len(dspkey(51))  > 0 then
       message="撤銷日期錯誤"
       formValid=False             
    elseif not Isdate(dspkey(54)) and len(dspkey(54))  > 0 then
       message="收款日期錯誤"
       formValid=False          
    elseif not Isdate(dspkey(70)) and len(dspkey(70))  > 0 then
       message="預定裝機日期錯誤"
       formValid=False          
    elseif not IsNumeric(dspkey(71)) and len(dspkey(71))  > 0 then
       message="預定裝機時間錯誤"
       formValid=False          
    elseif not IsNumeric(dspkey(72)) and len(dspkey(72))  > 0 then
       message="預定裝機時間錯誤"
       formValid=False              
    elseif not IsNumeric(dspkey(74)) and len(dspkey(74))  > 0 then
       message="施工補助金額錯誤"
       formValid=False                     
    elseif (dspkey(68)="1" or dspkey(68)="2" ) and dspkey(40) <> "" then
       message="安裝人員為(業務)或(技術部)時,施工廠商必須空白"
       formvalid=false
    elseif (dspkey(68)="3" ) and dspkey(40) = "" then
       message="安裝人員為(廠商)時,施工廠商不得空白"
       formvalid=false       
    elseif (dspkey(68)="1" ) and dspkey(49) = "" then
       message="安裝人員為(業務)時,預定安裝人員不得空白"
       formvalid=false              
    End If
    if dspkey(7) <> "F" and dspkey(7) <>"M" then dspkey(7)=""
'廠商標準施工費(施工廠商不為空白，且無付款列印批號時，始可變更）
    if len(trim(dspkey(40))) > 0 and len(trim(dspkey(60))) = 0 then
       Dim Connsupp,Rssupp,sqlsupp,dsn
       Set connsupp=server.CreateObject("ADODB.Connection")
       Set rssupp=Server.CreateObject("ADODB.RecordSet")
       DSN="DSN=RTLib"
       Sqlsupp="select * from RtSupp where cusid='" & dspkey(40) & "'"
       connsupp.open DSN
       rssupp.open sqlsupp,connsupp,1,1
       if rssupp.eof then
          dspkey(73) = 0
       else
          dspkey(73) = rssupp("STDFee")
       end if
    end if
 '收據名稱為空白時，
   IF len(trim(dspkey(35))) = 0 then
      dspkey(35)=extdb(0)
   end if
 '申請代表人為空白時，預設為"N"
   IF len(trim(dspkey(91))) = 0 then
      dspkey(91)="N"
   end if   
 '是否可建置欄位,若非y或n時,預設為空白
   if trim(dspkey(81)) <>"Y" and trim(dspkey(81)) <>"N" then
      dspkey(81)=""
   end if
'---檢查 HN號碼是否有重複 dspkey(89)---------
   IF LEN(TRIM(dspkey(88))) > 0 THEN
      Set connxx=Server.CreateObject("ADODB.Connection")  
      Set rsxx=Server.CreateObject("ADODB.Recordset")
      DSNXX="DSN=RTLib"
      connxx.Open DSNxx
      
	  if LEN(TRIM(DSPKEY(2))) =0 then
		 DSPKEY(2) =1
	  end if   
      sqlXX="SELECT count(*) AS CNT FROM RTsparqAdslcust where cusno='" & trim(dspkey(88)) & "' and not (cusid='" & dspkey(1) & "' and entryno=" & dspkey(2) & ")"
      rsxx.Open sqlxx,connxx
      s=""
      If RSXX("CNT") > 0 Then
         message="HN號碼已存在ADSL客戶，不可重複輸入!"
         formvalid=false
      End If
      rsxx.Close
      Set rsxx=Nothing
      connxx.Close
      Set connxx=Nothing    
   end IF
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                UUsrNc=V(1)
                DSpkey(38)=V(0)
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
   End Sub 
   Sub SrSelonclick()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="KEY" & clickid
       prog="RTFaQFinishUsrx.asp"
       CUTID=document.all("key13").value
       town=document.all("key14").value
       'showopt="Y;Y;Y;Y"表示對話方塊中要顯示的項目(業務工程師;客服人員;技術部;廠商)
       if clickkey="KEY6" then
          showopt="Y;N;N;N" & ";" & cutid & ";" & town
       else
          showopt="N;N;N;N;;"
       end if
       prog=prog & "?showopt=" & showopt
       FUsr=Window.showModalDialog(prog,"Dialog","dialogWidth:590px;dialogHeight:480px;")  
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
   Sub Srbranchonclick()
       prog="RTGetBRANCHD.asp"
       prog=prog & "?KEY=" & document.all("KEY3").VALUE 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key4").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub      
   Sub SrbranchMANonclick()	
       prog="RTGetBRANCHMAND.asp"
       prog=prog & "?KEY=" & document.all("KEY3").VALUE & ";" & document.all("KEY4").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key5").value =  trim(Fusrid(0))
       End if       
       end if
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
   Sub SrBUSonclick()
       prog="RTOBJSTOCKBRANCHBUSSD.asp"
       prog=prog & "?KEY=" & document.all("KEY3").VALUE & ";" & document.all("KEY4").VALUE
       FUsr=Window.open(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       Window.form.Submit
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
   
   Sub SrCmtysel()
       Dim ClickID,prog
       prog="RTCmtySelK.asp"
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="C" & clickid
       clearkey="key" & clickid
       CuTID2=document.all("key13").value
       township2=document.all("key14").value
       prog=prog & "?PARM=" & CutID2 & ";" & township2
       Fcmty=window.showModalDialog(prog,"Dialog","dialogWidth:590px;dialogHeight:480px;scroll:Yes")  
       document.all("key25").value=Fcmty
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
Sub SrGetUserDefineKey()
's=FrGetCmtyDesc(SESSION("comq1"))
%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
</table>

<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr>
    <td width="10%" class=dataListHead>社區序號</td>
    <td width="10%" bgcolor="silver">
        <input type="text" name="key0" <%=fieldRole(1)%><%=keyProtect%>
               style="text-align:left;" maxlength="10" size="6"
               value="<%=dspKey(0)%>" readonly class=dataListEntry></td>
    <td width="10%" class=dataListHead>客戶代號</td>
    <td width="10%" bgcolor="silver">
        <input type="text" name="key1" <%=fieldRole(1)%><%=keyProtect%>
               style="text-align:left;" maxlength="10" size="10"
               value="<%=dspKey(1)%>" class=dataListEntry></td>
    <td width="10%" class=dataListHead>客戶單次</td>
    <td width="10%" bgcolor="silver">
        <input type="text" name="key2" readonly
               style="text-align:left;" maxlength="6" size="5"
               value="<%=dspKey(2)%>" class=dataListdata></td>
    <td width="10%" bgcolor="orange" >收件編號</td>
    <td width="10%" bgcolor="silver">
        <input type="text" name="key76" readonly
               style="text-align:left;" maxlength="6" size="10"
               value="<%=dspKey(76)%>" class=dataListdata style="color:red"></td>
 <td width="10%" BGCOLOR=#BDB76B>轉檔報竣日期</td>
    <td width="10%" colspan="7" bgcolor=#DCDCDC>
        <input type="text" name="key89" <%=fieldRole(1)%><%=keyProtect%>
               style="text-align:left;color:red" maxlength="10" size="10"
               value="<%=dspKey(89)%>" readonly  class=dataListData></td>               
    </tr>
</table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(36))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                EUsrNc=V(1) 
                dspkey(36)=V(0)
      '          extdb(46)=v(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(36))
                V=split(rtnvalue,";")      
                EUsrNc=V(1)
        End if  
       dspkey(37)=datevalue(now())
    else
        if len(trim(dspkey(38))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                UUsrNc=V(1)
                DSpkey(38)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(38))
                V=split(rtnvalue,";")      
                UUsrNc=V(1)
        End if         
        Call SrGetEmployeeRef(rtnvalue,2,dspkey(36))
             V=split(rtnvalue,";")      
             EUsrNc=V(1)
        dspkey(39)=datevalue(now())
    end if  
' -------------------------------------------------------------------------------------------- 
    IF len(trim(dspkey(35))) = 0 then
      dspkey(35)=extdb(0)
    end if
    Dim conn,rs,s,sx,sql,t
    '結案碼(控制部份欄位不可修改)
    If dspKey(65)="Y" Then
       fieldPa=" class=""dataListData"" readonly "
       fieldPc=""
       fieldpd=""       
       fieldpe=""      
       fieldpf=""              
    Else
       fieldPa=""
       fieldPc=" Onclick=""Srbtnonclick"" "
       fieldpd=" onclick=""SrSelOnClick"" "       
       fieldpe=" onclick=""SrClear"" "     
       fieldpf=" onclick=""Srcmtysel"" "           
    End If    
    '已轉中華電信報竣時==>全部欄位protect(轉檔欄位)  
    if len(trim(dspkey(89))) > 0 then
       fieldPc=""
       fieldpd=""       
       fieldpe=""      
       fieldpf=""         
       fieldPg=" class=""dataListData"" readonly "
       fieldph=""
       fieldpi=""       
    else
       fieldPc=" Onclick=""Srbtnonclick"" "
       fieldpd=" onclick=""SrSelOnClick"" "       
       fieldpe=" onclick=""SrClear"" "     
       fieldpf=" onclick=""Srcmtysel"" " 
       fieldPg=""               
       fieldph=" onclick=""SrBranchonclick()"" "       
       fieldpi=" onclick=""SrBranchmanonclick()"" "       
    end if
    '收款表已列印或安裝員類別為發包(或空白)時，不可按安裝員工鈕，不可更改安裝人員資料（即安裝員工鈕disable)
  ' If Len(Trim(dspKey(50))) > 0  Then
  '    fieldPb=" class=""dataListData"" readonly "
  ' Else
  '    fieldPb=""
  ' End If
    if dspkey(65)="Y" or Len(Trim(dspKey(50))) > 0  then
       fieldPbx=""       
    else
       fieldPbx="SrAddusr()"
    end if        
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN%>
  <span id="tags1" class="dataListTagsOn"
        onClick="vbscript:tag1.style.display=''    :tags1.classname='dataListTagsOn':
                          tag2.style.display='none':tags2.classname='dataListTagsOf'"><font size=2>基本資料</span>
  <span id="tags2" class="dataListTagsOf"
        onClick="vbscript:tag1.style.display='none':tags1.classname='dataListTagsOf':
                          tag2.style.display=''    :tags2.classname='dataListTagsOn'"><font size=2>發包安裝</span>                                      
  <div class=dataListTagOn> 
<table width="100%" ><tr><td width="100%">&nbsp;</td></tr>                                                      
    <table border="1" width="100%" cellpadding="0" cellspacing="0" id="tag1" height="354">  
    <tr>
        <td width="15%" class="dataListSEARCH" height="25">對帳代號</td>
        <td width="18%" height="25" bgcolor="silver"> 
        <input type="text" name="key27" size="10" maxlength="10" readonly value="<%=dspkey(27)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListdata">
        #<input type="text" name="key92" size="3" maxlength="3" readonly value="<%=dspkey(92)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListdata"></td> 
        <td width="15%" class="dataListSEARCH" height="25">用戶IP</td>
        <td width="18%"  height="25" bgcolor="silver"> 
        <input type="text" name="key93" size="18" maxlength="15"  value="<%=dspkey(93)%>"<%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListentry"></td> 
         <td width="15%" class="dataListSEARCH" height="25">HomePNA Port</td>
        <td width="18%"  height="25" bgcolor="silver"> 
        <input type="text" name="key94" size="15" maxlength="15" value="<%=dspkey(94)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListentry">
    </TR>
    <tr>
        <td width="15%" class="dataListHead" height="25">身份證字號</td>
        <td width="18%" height="25" bgcolor="silver"> 
        <input type="text" name="key80" size="10" maxlength="10" value="<%=dspkey(80)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td> 
        <td width="12%" class="dataListHead" height="25">速博客戶代碼</td>       
        <td width="17%" height="25" bgcolor="silver"> 
        <input type="text" name="key88" size="10" maxlength="10" value="<%=dspkey(88)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td> 
        <td width="14%" bgcolor="orange"  height="25" ><FONT SIZE=2>保留撥接E-MAIL(舊HN號碼)</FONT></td>       
        <td width="20%" height="25" bgcolor="silver"> 
        <input type="text" name="key90" size="8" maxlength="8" value="<%=dspkey(90)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" style="color:red"></td>                         
    </tr>
    <tr>                   
        <td STYLE="DISPLAY:NONE" width="15%" class="dataListHead" height="25">經銷商</td>                                      
        <td STYLE="DISPLAY:NONE" width="30%" height="25" bgcolor="silver">
<%  If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 and  len(trim(dspkey(89)))=0 Then 
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
          &"WHERE (RTObjLink.CUSTYID = '02')  "
       s="<option value="""" >(經銷商)</option>"
    Else
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
          &"WHERE (RTObjLink.CUSTYID = '02')  and rtobj.cusid='" & dspkey(3) & "' "
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(經銷商)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(3) Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>
           <select STYLE="DISPLAY:NONE" size="1" name="key3" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry">                                            
              <%=s%>
           </select>
        <input type="HIDDEN" name="key4" size="10" value="<%=dspkey(4)%>" maxlength="12" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" readonly><font size=2>    
         <input type="HIDDEN" id="B4"  name="B4"   width="100%" style="Z-INDEX: 1"  value="..." <%=fieldph%> >  
         <!-- <IMG SRC="/consignee/images/IMGDELETE.GIF" alt="清除" id="C4"  name="C4"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" > 
         -->
        </td>                              
        <td STYLE="DISPLAY:NONE" width="8%" class="dataListHead" height="25">營業員</td>
        <td STYLE="DISPLAY:NONE" width="16%" height="25" bgcolor="silver">
        <input type="hidden" name="key5" size="8" value="<%=dspkey(5)%>" maxlength="12" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" readonly>
         <input type="hidden" id="B5"  name="B5"   width="100%" style="Z-INDEX: 1"  value="..." <%=fieldpi%>  >                  
        <!--  <IMG SRC="/consignee/images/IMGDELETE.GIF" alt="清除" id="C5"  name="C5"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >    
        -->
        </td>     
<% 
   %>               
        <td width="8%" class="dataListHead" height="25">業務員</td>                              
        <td width="16%" height="25" bgcolor="silver" colspan=3>
      <input type="text" name="key6" size="6" maxlength="50" readonly value="<%=dspkey(6)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%> class="dataListEntry" >
     <input type="button" id="B6"  name="B6"   width="100%" style="Z-INDEX: 1"  value="..." <%=fieldpd%>  >
          <IMG SRC="/consignee/images/IMGDELETE.GIF" alt="清除" id="C6"  name="C6"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >        
        </TD>
        </TR>
      <tr>                                      
        <td width="15%" class="dataListHead" height="25">客戶名稱</td>                                      
        <td width="30%" height="25" bgcolor="silver">
          <input type="text" name="ext0" size="28" maxlength="50" value="<%=extdb(0)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                              
        <td width="8%" class="dataListHead" height="25">性別</td>
<%  dim sexd1, sexd2
    if len(trim(dspkey(89))) =0 and dspkey(65) <> "Y" then
       If Len(Trim(FIELDROLE(1) &dataProtect)) < 1 Then
          sexd1=""
          sexd2=""
       Else
          sexd1=" disabled "
          sexd2=" disabled "
       end if
    else
          sexd1=" disabled "
          sexd2=" disabled "
    End If
    If dspKey(7)="M" Then sexd1=" checked "    
    If dspKey(7)="F" Then sexd2=" checked " %>                          
        <td width="16%" height="25" bgcolor="silver">
        <input type="radio" value="M" <%=sexd1%> name="key7" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtec%>><font size=2>男</font>
        <input type="radio" name="key7" value="F" <%=sexd2%><%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>><font size=2>女</font></td>                              
        <td width="8%" class="dataListHead" height="25">出生日期</td>                              
        <td width="16%" height="25" bgcolor="silver">
          <input type="text" name="key8" size="10" value="<%=dspkey(8)%>" maxlength="10" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class=dataListEntry>
          <input type="button" id="B8"  name="B8" height="70%" width="70%" style="Z-INDEX: 1" value="..." <%=fieldpc%>></td>                              
      </tr>                              
      <tr>                              
        <td width="15%" class="dataListHead" height="25">帳單(通訊)地址</td>                              
        <td width="60%" colspan="3" height="25" bgcolor="silver">
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) and len(trim(dspkey(89)))=0 Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(9))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市別)</option>"       
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
         <select size="1" name="key9"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry"><%=s%></select>
        <input type="text" name="key10" size="8" value="<%=dspkey(10)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><font size=2>(鄉鎮市區)                 
         <input type="button" id="B10"  name="B10"   width="100%" style="Z-INDEX: 1"  value="..." <%=SXX10%>  >        
          <IMG SRC="/consignee/images/IMGDELETE.GIF" alt="清除" id="C10"  name="C10"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
        <input type="text" name="key11" size="32" value="<%=dspkey(11)%>" maxlength="60" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" class="dataListHead" height="25">郵遞區號</td>                                 
        <td width="16%" height="25" bgcolor="silver"><input type="text" name="key12" size="10" value="<%=dspkey(12)%>" maxlength="5" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListdata" readonly></td>                                 
      </tr>                                 
      <tr>                                 
        <td width="15%" class="dataListHead" height="25">裝機地址</td>                                 
        <td width="60%" colspan="3" height="25" bgcolor="silver">
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(13))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市別)</option>"       
       SXX14=" onclick=""Srcounty14onclick()"" "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(13) & "' " 
       sXX14=""
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
        <select name="key13" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1"  style="text-align:left;" maxlength="8" class="dataListEntry">
        <%=s%></select>
        <input type="text" name="key14" size="8" value="<%=dspkey(14)%>" maxlength="10" readonly <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><font size=2>(鄉鎮市區)                 
         <input type="button" id="B14"  name="B14"   width="100%" style="Z-INDEX: 1"  value="..." <%=SXX14%>  >        
          <IMG SRC="/consignee/images/IMGDELETE.GIF" alt="清除" id="C14"  name="C14"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >    
        <input type="text" name="key15" size="32" value="<%=dspkey(15)%>" maxlength="60" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                                
        <td width="8%" class="dataListHead" height="25">郵遞區號</td>                                 
        <td width="16%" height="25" bgcolor="silver"><input type="text" name="key16" size="10" value="<%=dspkey(16)%>" maxlength="5"<%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListdata" readonly></td>                                 
      </tr>
      <tr>                                 
        <td width="15%" class="dataListHead" height="25">戶籍地址</td>                                 
        <td width="60%" colspan="3" height="25" bgcolor="silver">
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(17))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if    
       s=s &"<option value=""" &"""" &sx &">(縣市別)</option>"        
       sxx18=" onclick=""Srcounty18onclick()"" "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(17) & "' " 
       sxx18=""
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
        <select name="key17" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" style="text-align:left;" maxlength="8" class="dataListEntry"><%=s%></select>
        <input type="text" name="key18" size="8" value="<%=dspkey(18)%>" maxlength="10" readonly <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><font size=2>(鄉鎮市區)                 
         <input type="button" id="B18"  name="B18"   width="100%" style="Z-INDEX: 1"  value="..." <%=sxx18%>  >        
          <IMG SRC="/consignee/images/IMGDELETE.GIF" alt="清除" id="C18"  name="C18"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >     
        <input type="text" name="key19" size="32" value="<%=dspkey(19)%>" maxlength="60" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                     
        <td width="8%" class="dataListHead" height="25">郵遞區號</td>                                 
        <td width="16%" height="25" bgcolor="silver"><input type="text" name="key20" size="10" value="<%=dspkey(20)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListdata" readonly></td>                                 
      </tr>                                
      <tr>          
<script language="vbscript">
Sub SrAddrEqual()
  Dim i,objOpt
  document.All("key13").value=document.All("key9").value
  document.All("key14").value=document.All("key10").value
  document.All("key15").value=document.All("key11").value
  document.All("key16").value=document.All("key12").value
End Sub 
Sub SrAddrEqual2()
  document.All("key17").value=document.All("key9").value
  document.All("key18").value=document.All("key10").value
  document.All("key19").value=document.All("key11").value
  document.All("key20").value=document.All("key12").value
End Sub 
Sub SrAddUsr()
  ExistUsr=document.all("key69").value
  InsType=cstr(document.all("key68").value)
  UsrStr=Window.showModalDialog("RTCustAddUsr.asp?parm=" & existusr & "@" & instype   ,"Dialog","dialogWidth:410px;dialogHeight:400px;")
  if UsrStr<>False then
     UsrStrAry=split(UsrStr,"@")
     document.all("key69").value=UsrStrAry(0)
     document.all("REF01").value=UsrStrAry(1)     
  end if
End Sub

Sub Srpay()
  if document.all("key68").value = "1" then
     document.all("key73").value = 200
  else
     document.all("key73").value = 0
  end if
end sub
</script>                       
        <td width="35%" class="dataListHead" colspan="6" height="34" bgcolor="silver">
<%  dim seld1
    if len(trim(dspkey(89))) =0 and dspkey(65) <> "Y" then
       If Len(Trim(FIELDROLE(1) &dataProtect)) < 1 Then
          seld1=""
       Else
          seld1=" disabled "
       End If
    else
        seld1=" disabled "
    end if
    %>
            <input type="radio" name="rdo1" value="1"<%=seld1%><%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> 
                   onClick="SrAddrEqual()">裝機地址同帳單地址
            <input type="radio" name="rdo2" value="1"<%=seld1%><%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> 
                   onClick="SrAddrEqual2()">戶籍地址同帳單地址</td>                                 

      </tr>                                 
      <tr>                            
        <td width="15%" class="dataListHead" height="25">申請方案</td>
 <td width="30%" height="25" bgcolor="silver">
<% aryOption=Array("經濟型","單機型","商業型")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 and len(trim(dspkey(89)))=0 Then 
      For i = 0 To Ubound(aryOption)
          If dspKey(23)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(23) &""">" &dspKey(23) &"</option>"
   End If%>               
   <select size="1" name="key23" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">                                            
        <%=s%>
   </select>
   </td>    
        <td width="8%" class="dataListHead" height="23">申請速度</td>
<% aryOption=Array("512/64Kbps")
   s=""
   If Len(Trim(FIELDROLE(1) &dataProtect)) < 1 Then 
      For i = 0 To Ubound(aryOption)
          If dspKey(21)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(21) &""">" &dspKey(21) &"</option>"
   End If%>                                      
        <td width="16%" height="23" bgcolor="silver"><select size="1" name="key21" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">                                                             
        <%=s%></select></td>      
        <td width="8%" class="dataListHead" height="25">線路種類</td>
<% aryOption=Array("ADSL")
   s=""
   If Len(Trim(FIELDROLE(1) &dataProtect)) < 1 Then   
      For i = 0 To Ubound(aryOption)
          If dspKey(22)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(22) &""">" &dspKey(22) &"</option>"
   End If%>                                  
        <td width="16%" height="25" bgcolor="silver"><select size="1" name="key22" style="font-family: 新細明體; font-size: 10pt"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">                                                                  
        <%=s%></select></td>                                                                         
     </tr>                     
      <tr>                            
        <td width="15%" class="dataListHead" height="25">堪查日期</td>
         <td width="30%" height="25" bgcolor="silver">       
          <input type="text" name="key77" size="10" value="<%=dspKey(77)%>"  <%=fieldpg%><%=fieldpa%><%=FIELDROLE(3)%> class="dataListDATA" maxlength="10"  READONLY>
          <input type="button" id="B77"  name="B77" height="100%" width="100%" style="Z-INDEX: 1" value="..."  <%=fieldpc%> READONLY>
          <%  dim rdo1, rdo2
              if len(trim(dspkey(89))) =0 and dspkey(65) <> "Y" then
                 If Len(Trim(fieldRole(3) &dataProtect)) < 1 Then
                    rdo1=""
                    rdo2=""
                 Else
                    rdo1=" disabled "
                    rdo2=" disabled "
                 end if
              else
                 rdo1=" disabled "
                 rdo2=" disabled "
              End If
             ' If Trim(dspKey(81))="" Then dspKey()="Y"
              If trim(dspKey(81))="Y" Then 
                 rdo1=" checked "    
              elseIf trim(dspKey(81))="N" Then 
                 rdo2=" checked " 
              end if
             %>
        <input type="radio" value="Y" <%=rdo1%> name="key81" <%=fieldpg%><%=fieldRole(3)%><%=dataProtect%>  READONLY><font size=2>可建置
        <input type="radio" value="N" <%=rdo2%>  name="key81" <%=fieldpg%><%=fieldRole(3)%><%=dataProtect%>  READONLY><font size=2>無法建置
          </td> 
        <td width="8%" class="dataListHead" height="25">堪查結果</td>
         <td width="16%"  height="25" bgcolor="silver">       
         <% aryOption=Array("","有電信室","無電信室","無電信箱")
            s=""
            If Len(Trim(fieldRole(3) &dataProtect)) < 1 and len(trim(dspkey(89)))=0 Then 
               For i = 0 To Ubound(aryOption)
                   If dspKey(82)=aryOption(i) Then
                      sx=" selected "
                   Else
                      sx=""
                   End If
                   s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
               Next
            Else
                   s="<option value=""" &dspKey(82) &""">" &dspKey(82) &"</option>"
            End If%>               
         <select size="1" name="key82" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(3)%><%=dataProtect%> class="dataListDATA" READONLY>                                            
           <%=s%>
         </select>
         <% aryOption=Array("","跨棟","獨棟","雙拼")
            s=""
            If Len(Trim(fieldRole(3) &dataProtect)) < 1 and len(trim(dspkey(89)))=0 Then 
               For i = 0 To Ubound(aryOption)
                   If dspKey(83)=aryOption(i) Then
                      sx=" selected "
                   Else
                      sx=""
                   End If
                   s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
               Next
            Else
                   s="<option value=""" &dspKey(83) &""">" &dspKey(83) &"</option>"
            End If%>               
         <select size="1" name="key83" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(3)%><%=dataProtect%> class="dataListDATA"READONLY>                                            
           <%=s%>
         </select>         
         </td>
          <td width="8%" class="dataListHead">正式申請日</td>                     
          <td width="16%"  bgcolor="silver">
          <input type="text" name="key78" size="10" value="<%=dspKey(78)%>"  <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%> class="dataListEntry" maxlength="10" >
          <input type="button" id="B78"  name="B78" height="100%" width="100%" style="Z-INDEX: 1" value="..."  <%=fieldpc%>></td> 
      </tr>
      <tr>
        <td width="15%"  class="dataListHead" height="34">堪查補充說明</td>  
        <td width="30%"  colspan="3" height="21" bgcolor="silver">
        <input type="text" name="key84" style="text-align:left;" maxlength="300" size="60"
               value="<%=dspKey(84)%>"<%=FIELDROLE(3)%> class=dataListDATA style="color:red"  READONLY>
        </td>
        <td width="8%"   bgcolor="ORANGE"  height="34">送件日期</td>                                 
        <td width="16%"  height="34" bgcolor="silver">
          <input type="text" name="key79" size="10" value="<%=dspKey(79)%>" maxlength="10" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
          <input type="button" id="B79"  name="B79" height="100%" width="100%" style="Z-INDEX: 1" value="..."  <%=fieldpc%>></td>       
      </tr>            
      <tr style="display:none">
        <td width="15%"  bgcolor="ORANGE"  height="34">CHT簽核日期</td>  
        <td width="30%"  height="21" bgcolor="silver">
        <input type="text" name="key85" style="text-align:left;" maxlength="10" size="10"
               value="<%=dspKey(85)%>" class=dataListentry >
          <input type="button" id="B85"  name="B85" height="100%" width="100%" style="Z-INDEX: 1" value="..."  <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=fieldpc%>>               
        </td>
        <td width="8%"   bgcolor="ORANGE" height="34">送營運處日期</td>                                 
        <td width="16%"  height="34" bgcolor="silver">
          <input type="text" name="key86" size="10" value="<%=dspKey(86)%>" maxlength="10" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
          <input type="button" id="B86"  name="B86" height="100%" width="100%" style="Z-INDEX: 1" value="..."  <%=fieldpc%>></td>       
        <td width="8%"   bgcolor="ORANGE" height="34">取得附掛電話日</td>                                 
        <td width="16%"  height="34" bgcolor="silver">
          <input type="text" name="key87" size="10" value="<%=dspKey(87)%>" maxlength="10" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
          <input type="button" id="B87"  name="B87" height="100%" width="100%" style="Z-INDEX: 1" value="..."  <%=fieldpc%>></td>                 
      </tr>                                            
      <tr>                                    
        <td width="15%" class="dataListHead" height="21">住宅種類</td>                                    
        <td width="30%"  colspan="5" height="21" bgcolor="silver">
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) and len(trim(dspkey(89)))=0 Then 
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='C2' " 
       If len(trim(dspkey(24))) < 1 Then
          sx=" selected " 
        '  s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
        '  s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='C2' AND CODE='" & dspkey(24) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(24) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key24" style="font-family: 新細明體; font-size: 10pt"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">                                                                  
        <%=s%>
   </select><font size=2>
   &nbsp;社區名稱<input type="text" name="key25" size="15" MAXLENGTH="30" value="<%=dspKey(25)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(3)%><%=dataProtect%> readonly class="dataListDATA">
        <!--
        <input type="button" id="B25"  name="B25"   width="100%" style="Z-INDEX: 1"  value="..." <%=fieldpf%>  >
          <IMG SRC="/consignee/images/IMGDELETE.GIF" alt="清除" id="C25"  name="C25"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >        
          -->
   共<input type="text" name="key26" size="4" value="<%=dspKey(26)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> maxlength="4" class="dataListEntry">戶</td>                                 
      </tr>                                 
      <tr>                                    
        <td width="15%" class="dataListHead" height="23">聯絡電話</td>                                 
        <td width="30%" height="23"><input type="text" name="key28" size="15" value="<%=dspkey(28)%>" maxlength="15" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" class="dataListHead" height="23">傳真電話</td>                                 
        <td width="16%" height="23" bgcolor="silver"><input type="text" name="key29" size="15" value="<%=dspkey(29)%>" maxlength="15" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" class="dataListHead" height="23">聯絡人</td>                                 
        <td width="16%" height="23" bgcolor="silver"><input type="text" name="key30" size="10" value="<%=dspkey(30)%>" maxlength="20" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                                 
      </tr>                                 
      <tr>                                 
        <td width="15%" class="dataListHead" height="23" bgcolor="silver">公司電話</td>                                 
        <td width="30%" height="23"><input type="text" name="key31" size="15" value="<%=dspkey(31)%>" maxlength="15" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
        <font size=2>分機<input type="text" name="key32" size="5" value="<%=dspkey(32)%>" maxlength="5" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" class="dataListHead" height="23">行動電話</td>                                 
        <td width="16%"  height="23" bgcolor="silver"><input type="text" name="key33" size="15" value="<%=dspkey(33)%>" maxlength="15" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" height="23" class="dataListHead" >申請代表人</td>                     
        <td width="16%" height="23" bgcolor="silver">
        <%  dim OPT1, OPT2
            if len(trim(dspkey(89))) =0 and dspkey(65) <> "Y" then
               If Len(Trim(FIELDROLE(1) &dataProtect)) < 1 Then
                  OPT1=""
                  OPT2=""
               Else
                  OPT1=" disabled "
                  OPT2=" disabled "
               end if
            else
               OPT1=" disabled "
               OPT2=" disabled "
            End If
            If dspKey(91)="Y" Then OPT1=" checked "    
            If dspKey(91)="N" Then OPT2=" checked " %>                          
        
        <input type="radio" value="Y" <%=OPT1%> name="key91" <%=fieldpg%><%=fieldpa%><%=dataProtec%>><font size=2>是</font>
        <input type="radio" value="N" <%=OPT2%> name="key91" <%=fieldpg%><%=fieldpa%><%=dataProtect%>><font size=2>否</font></td>                              
            
      </tr>                                 
      <tr>                                 
        <td width="15%" class="dataListHead" height="25">電子郵件信箱</td>                                 
        <td width="30%" height="25" bgcolor="silver"><input type="text" name="key34" size="30" value="<%=dspkey(34)%>" maxlength="30" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" class="dataListHead" height="23">收據名稱</td>                                 
        <td width="16%" colspan="3" height="23" bgcolor="silver"><input type="text" name="key35" size="15" value="<%=dspkey(35)%>" maxlength="50" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
        <font size=2>(空白時預設為客戶名稱)</font></td>                   
      </tr>                                 
      <tr>                                 
        <td width="15%" class="dataListHead" height="23" style="display:none">輸入人員</td>                                 
        <td width="30%" height="23" bgcolor="silver" style="display:none"><input type="text" name="key36" size="10" class="dataListData" value="<%=dspKey(36)%>" readonly style="display:none"><%=EusrNc%></td>                                 
        <td width="8%" class="dataListHead" height="23" style="display:none">輸入日期</td>                                 
        <td width="40%" colspan="3" height="23" bgcolor="silver" style="display:none"><input type="text" name="key37" size="15" class="dataListData" value="<%=dspKey(37)%>" readonly style="display:none"></td>                                 
      </tr>                                 
      <tr>                                 
        <td width="15%" class="dataListHead" height="23" style="display:none">異動人員</td>                                 
        <td width="30%" height="23" bgcolor="silver" style="display:none"><input type="text" name="key38" size="10" class="dataListData" value="<%=dspKey(38)%>" readonly style="display:none"><%=UUsrNc%></td>                                 
        <td width="8%" class="dataListHead" height="23" style="display:none">異動日期</td>                                 
        <td width="40%" colspan="3" height="23" bgcolor="silver" style="display:none"><input type="text" name="key39" size="15" class="dataListData" value="<%=dspKey(39)%>" readonly style="display:none"></td>                                 
      </tr>
     
    <table border="1" width="100%" cellpadding="0" cellspacing="0" id="tag2" style="display: none">                           
      <tr>                         
        <td width="15%" class="dataListHead">施工廠商</td>                     
        <td width="15%" bgcolor="silver">
<%
    If (sw="E" Or (accessMode="A" And sw="")) And Len(Trim(fieldPa &fieldPb &fieldRole(1) &dataProtect))<1 Then 
       sql="SELECT RTObj.CUSID, RTObj.SHORTNC " _
          &"FROM RTObj RIGHT OUTER JOIN " _
          &"RTConsignee ON RTObj.CUSID = RTConsignee.CUSID "
    Else
       sql="SELECT RTObj.CUSID, RTObj.SHORTNC " _
          &"FROM RTSparqADSLcust LEFT OUTER JOIN " _
          &"RTObj ON RTSparqADSLcust.PROFAC = RTObj.CUSID " _
          &"WHERE RTobj.CUSID='" &dspKey(40) &"' "
    End If
   ' Response.Write "SQL=" & SQL & "<BR>"
    rs.Open sql,conn
    s=""
    If rs.Eof Then 
       s="<option value="""" selected>&nbsp;</option>"
    else
       sx=""
       s="<option value="""">&nbsp;</option>" & vbcrlf      
       Do While Not rs.Eof
          If rs("CusID")=dspKey(40) Then sx=" selected "
          s=s &"<option value=""" &rs("CusID") &"""" &sx &">" &rs("SHORTNC") &"</option>" & vbcrlf
          rs.MoveNext
          sx=""
       Loop
    end if
    rs.Close
%>
        <select name="key40" <%=fieldRole(1)%><%=dataProtect%><%=fieldpg%><%=fieldPa%><%=fieldPb%> size="1"    
               style="text-align:left;" maxlength="8" class="dataListEntry"><%=s%></select></td> 
        <td width="15%" class="dataListHead">通知發包日期</td>                     
        <td width="15%" colspan="1" bgcolor="silver">
          <input type="text" name="key41" size="10" value="<%=dspKey(41)%>" readonly <%=fieldpg%><%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%> class="dataListdata" maxlength="10"></td>                                               
        <td width="15%" class="dataListHead">發包日期</td>                     
        <td width="15%" colspan="1" bgcolor="silver">
          <input type="text" name="key42" size="10" value="<%=dspKey(42)%>" <%=fieldpg%><%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%> class="dataListEntry" maxlength="10">
          <input type="button" id="B42"  name="B42" height="100%" width="100%" style="Z-INDEX: 1" value="..." <%=fieldpc%>>          </td>                   
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">安裝表批號</td>                    
        <td width="15%" bgcolor="silver"><input type="text" name="key43" size="10" class="dataListData" value="<%=dspKey(43)%>" readonly></td>                     
        <td width="15%" class="dataListHead">安裝表列印日</td>                     
        <td width="15%" bgcolor="silver"><input type="text" name="key44" size="10" class="dataListData" value="<%=dspKey(44)%>" readonly></td>                     
        <td width="15%" class="dataListHead">列印人員</td>                     
        <td width="15%" bgcolor="silver"><input type="text" name="key45" size="10" class="dataListData" value="<%=dspKey(45)%>" readonly></td>                   
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">完工日期</td>                    
        <td width="15%" bgcolor="silver">
          <input type="text" name="key46" size="10" value="<%=dspKey(46)%>" <%=fieldpg%><%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="10" ></td>                     
        <td width="15%" class="dataListHead">報竣日期</td>   
        <td width="15%" bgcolor="silver">                  
         <input type="text" name="key47" size="10" readonly value="<%=dspKey(47)%>" <%=fieldpg%><%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListdata" maxlength="10">
        <td width="15%" class="dataListHead">入帳日期</td>                     
        <td width="15%">
          <input type="text" name="key48" size="10" value="<%=dspKey(48)%>"   class="dataListdata" readonly maxlength="10">
          <input type="button" id="B48"  name="B48" height="100%" width="100%" style="Z-INDEX: 1" value="..." <%=fieldpc%>></td>                   
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">應收金額</td>                    
        <td width="15%" bgcolor="silver">
          <input type="text" name="key49" size="10" value="<%=dspKey(49)%>" <%=fieldpg%><%=fieldPa%><%=fieldPb%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" maxlength="10"></td>                     
        <td width="15%" class="dataListHead">實收金額</td>                     
        <td width="15%" bgcolor="silver">
        <input type="text" name="key50" size="10" value="<%=dspKey(50)%>" <%=fieldpg%><%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="10"></td>                     
        <td width="15%" class="dataListHead">撤銷日期</td>                     
        <td width="15%" bgcolor="silver">
          <input type="text" name="key51" size="10" value="<%=dspKey(51)%>" <%=fieldPa%><%=fieldPb%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" maxlength="10" >
          <input type="button" id="B51"  name="B51" height="100%" width="100%" style="Z-INDEX: 1" value="..." <%=fieldpc%>></td>                   
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">收款表批號</td>                    
        <td width="15%" bgcolor="silver"><input type="text" name="key52" size="10" class="dataListData" value="<%=dspKey(52)%>" readonly></td>                     
        <td width="15%" class="dataListHead">列印人員</td>                     
        <td width="15%" bgcolor="silver"><input type="text" name="key53" size="10" class="dataListData" value="<%=dspKey(53)%>" readonly></td>                     
        <td width="15%" class="dataListHead">收款日期</td>                     
        <td width="15%" bgcolor="silver">
         <input type="text" name="key54" size="10" value="<%=dspKey(54)%>" <%=fieldpg%><%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" maxlength="10">
          <input type="button" id="B54"  name="B54" height="100%" width="100%" style="Z-INDEX: 1" value="..." <%=fieldpc%>></td>                   
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">財務收款確認日</td>                    
        <td width="15%" bgcolor="silver"><input type="text" name="key55" size="10" class="dataListData" value="<%=dspKey(55)%>" readonly></td>                     
        <td width="15%" class="dataListHead">財務確認人員</td>                     
        <td width="15%" bgcolor="silver"><input type="text" name="key56" size="10" class="dataListData" value="<%=dspKey(56)%>" readonly></td>                     
        <td width="15%" class="dataListHead">獎金計算日期</td>                     
        <td width="15%" bgcolor="silver">
          <input type="text" name="key57" size="10" value="<%=dspKey(57)%>" readonly  class="dataListdata" maxlength="10"></td>                   
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">撤銷原因說明</td>                    
        <td width="15%" colspan="5" bgcolor="silver">
          <input type="text" name="key58" size="70" value="<%=dspKey(58)%>" <%=fieldPa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="50"></td>                     
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">未完工原因</td>                    
        <td width="15%" colspan="5" bgcolor="silver">
          <input type="text" name="key59" size="70" value="<%=dspKey(59)%>" <%=fieldPa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="50"></td>                     
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">付款表批號</td>                    
        <td width="15%" bgcolor="silver"><input type="text" name="key60" size="10" class="dataListData" value="<%=dspKey(60)%>" readonly></td>                     
        <td width="15%" class="dataListHead">付款表日期</td>                     
        <td width="15%" bgcolor="silver"><input type="text" name="key61" size="10" class="dataListData" value="<%=dspKey(61)%>" readonly></td>                     
        <td width="15%" class="dataListHead">列印人員</td>                     
        <td width="15%" bgcolor="silver"><input type="text" name="key62" size="10" class="dataListData" value="<%=dspKey(62)%>" readonly></td>                   
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">付款會計審核確認日</td>                    
        <td width="15%" bgcolor="silver"><input type="text" name="key63" size="10" class="dataListData" value="<%=dspKey(63)%>" readonly></td>                     
        <td width="15%" class="dataListHead">會計審核人員</td>                     
        <td width="15%" bgcolor="silver"><input type="text" name="key64" size="10" class="dataListData" value="<%=dspKey(64)%>" readonly></td>                     
        <td width="15%" class="dataListHead">結案碼</td>                     
        <td width="15%" bgcolor="silver"><input type="text" name="key65" size="10" class="dataListData" value="<%=dspKey(65)%>" readonly></td>                   
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">施工備註說明</td>                    
        <td width="15%" colspan="5" bgcolor="silver">
          <input type="text" name="key66" size="70" value="<%=dspKey(66)%>" <%=fieldPa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="50"></td>                     
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">施工環境代碼</td>                    
        <td width="15%" bgcolor="silver">
        <%
    If (sw="E" Or (accessMode="A" And sw="")) And Len(Trim(fieldPa  &FIELDROLE(1) &dataProtect))<1 and len(trim(dspkey(90)))=0 Then 
       sql="SELECT code, codenc " _
          &"FROM RTcode where kind='C4' " 
    Else
       sql="SELECT code, codenc " _
          &"FROM RTcode where kind='C4' and code='" &dspKey(67) &"' "
    End If
    rs.Open sql,conn
    s=""
    If rs.Eof Then s="<option value="""" selected>&nbsp;</option>"
    sx=""
    Do While Not rs.Eof
       If rs("code")=dspKey(67) Then sx=" selected "
       s=s &"<option value=""" &rs("code") &"""" &sx &">" &rs("codenc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
        <select name="key67" <%=FIELDROLE(1)%><%=dataProtect%><%=fieldpg%><%=fieldPa%> size="1"    
               style="text-align:left;" maxlength="8" class="dataListEntry"><%=s%></select>

        <td width="15%" class="dataListHead">安裝員類別</td>
<%' if userlevel=1 then
  '    aryOption=Array("","業務自行安裝","技術部安裝")      
  '    aryOptionV=Array("0","1","2")
  ' elseif userlevel=4 then
  '    aryOption=Array("","技術部安裝","發包")
  '    aryOptionV=Array("0","2","3")
  ' elseif userlevel=31 then
      aryOption=Array("","業務自行安裝","技術部安裝","發包")
      aryOptionV=Array("0","1","2","3")   
  ' end if
   s=""
   If Len(Trim(fieldPa &fieldRole(1) &dataProtect)) > 0 or len(trim(dspkey(89)))> 0 Then
      s="<option value=""" &dspKey(68) &""">" &aryOption(dspKey(68)) &"</option>"
      SXX=""
   Else
      For i = 0 To Ubound(aryOption)
          If dspKey(68)=aryOptionV(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOptionV(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
      sxx=" onclick=""SrAddUsr()"" "
   End If%>                    
        <td width="15%" bgcolor="silver"><select size="1" onChange="Srpay()" name="key68" <%=fieldpg%><%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
          <%=s%></select></td>                     
        <td width="15%" class="dataListHead">
        <input type="button" name="EMPLOY" <%=fieldpg%><%=fieldPa%><%=fieldPb%> class=keyListButton <%=SXX%> value="裝機員工"></td>                     
        <td width="15%" bgcolor="silver">
  <% 
    Usrary=split(dspkey(69),";")
    qrystrng=""
    s1=""
    existusr=""
    if Ubound(Usrary) >= 0 then
       existUsr="("
       for i=0 to Ubound(usrary)
           existUsr=existUsr & "'" & usrary(i) & "',"
       next
       existUsr=mid(existUsr,1,len(existUsr)-1)
       existUsr=existUsr & ")"
       qrystring=" and rtemployee.emply in " & existusr
    end if
    if len(trim(qrystring)) < 1 then
       qrystring=" and rtemployee.emply='*' "
    end if
    sql="SELECT RTEmployee.emply, RTObj.CUSNC " _
          &"FROM RTEmployee INNER JOIN " _
          &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
          &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08'" _
          & qrystring _
          &" order by cusnc "
    rs.Open sql,conn
    Do While Not rs.Eof
       s1=s1 & rs("cusnc") & ";"
       rs.MoveNext
    Loop
    if trim(len(s1)) > 0 then 
       s1=mid(s1,1,len(s1)-1)
    else
       dspkey(69)=""
       s1=""
    end if 
    rs.Close
    conn.Close   
    set rs=Nothing   
    set conn=Nothing
   %>       
          <input type="text" name="key69" size="14" value="<%=dspKey(69)%>"  class="dataListData"  readonly maxlength="50" style="display:none">
          <input type="text" name="ref01" size="10" value="<%=S1%>"  class="dataListData"  readonly maxlength="50">
          </td>                   
      </tr>                                     
      <tr>            
        <td width="15%" class="dataListHead">預定裝機日期</td>                    
        <td width="15%" bgcolor="silver">
          <input type="text" name="key70" size="10" value="<%=dspKey(70)%>" <%=fieldpg%><%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" maxlength="10">
          <input type="button" id="B70"  name="B71" height="100%" width="100%" style="Z-INDEX: 1" value="..." <%=fieldpc%>></td>                     
        <td width="15%" class="dataListHead">預定裝機時間(時)</td>                     
        <td width="15%" bgcolor="silver">
          <input type="text" name="key71" size="10" value="<%=dspKey(71)%>" <%=fieldpg%><%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" maxlength="2"></td>                     
        <td width="15%" class="dataListHead">預定裝機時間(分)</td>                     
        <td width="15%" bgcolor="silver">
          <input type="text" name="key72" size="10" value="<%=dspKey(72)%>" <%=fieldpg%><%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" maxlength="2"></td>                   
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">標準施工費</td>                    
        <td width="15%" bgcolor="silver">
        <input type="text" name="key73" size="10" class="dataListData" value="<%=dspKey(73)%>" readonly ></td>                     
        <td width="15%" class="dataListHead">施工補助費</td>                     
        <td width="15%" bgcolor="silver">
        <input type="text" name="key74" size="10" value="<%=dspKey(74)%>" <%=fieldpg%><%=fieldPa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="15"></td>                     
        <td width="15%" colspan="2">　</td>                     
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">施工補助費說明</td>                    
        <td width="15%" colspan="5" bgcolor="silver">
          <input type="text" name="key75" size="70" value="<%=dspKey(75)%>" <%=fieldPa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="25"></td>                     
      </tr>                                     
    </table>
<table width="100%"><tr><td width="100%">&nbsp;</td></tr>                                                                                                   
  </div>                               
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrReadExtDB()
    Dim conn,rs
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open DSN
    Set rs=Server.CreateObject("ADODB.Recordset")
    rs.Open "SELECT * FROM RTObj WHERE CusID='" &dspKey(1) &"' ",conn
    extDB(0)=rs("CusNC")
   ' extDB(1)=rs("CutID1")
   ' extDB(2)=rs("TownShip1")
   ' extDB(3)=rs("RAddr1")
   ' extDB(4)=rs("RZone1")
   ' extDB(5)=rs("CutID2")
   ' extDB(6)=rs("TownShip2")
   ' extDB(7)=rs("RAddr2")
   ' extDB(8)=rs("RZone2")
    rs.Close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrSaveExtDB(Smode)
    Dim conn,rs
' Smode A:add U:update
' extDBField = n
' use extDB(i) for Screen ,and map it to DataBase
'
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open DSN
    Set rs=Server.CreateObject("ADODB.Recordset")
'------ RTObj ---------------------------------------------------
    rs.Open "SELECT * FROM RTObj WHERE CusID='" &dspKey(1) &"' ",conn,3,3
    If rs.Eof Or rs.Bof Then
       If Smode="A" Then
          rs.AddNew
          rs("CusID")=dspKey(1)
       End If
    End If

    rs("CusNC")=extDB(0)
    rs("ShortNC")=Left(extDB(0),5)
   ' rs("CutID1")=extDB(1)
   ' rs("TownShip1")=extDB(2)
   ' rs("RAddr1")=extDB(3)
   ' rs("RZone1")=extDB(4)
   ' rs("CutID2")=extDB(5)
   ' rs("TownShip2")=extDB(6)
   ' rs("RAddr2")=extDB(7)
   ' rs("RZone2")=extDB(8)
    rs("Eusr")=""
    rs("Edat")=now()
    rs("Uusr")=""
    rs("Udat")=now()
    rs.Update
    rs.Close
'------ RTObjLink -----------------------------------------------
    rs.Open "SELECT * FROM RTObjLink WHERE CustYID='05' AND CusID='" &dspKey(1) &"' ",conn,3,3
    'Response.Write RS.EOF
    If rs.Eof Or rs.Bof Then
       If Smode="A" Then
          rs.AddNew
          rs("CusID")=dspKey(1)
          rs("CustYID")="05"
       End If
    End If
    rs("Eusr")=""
    rs("Edat")=now()
    rs("Uusr")=""
    rs("Udat")=now()
    rs.Update
    rs.Close

    conn.Close
    Set rs=Nothing
    Set conn=Nothing
End Sub
' -------------------------------------------------------------------------------------------- 
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
      DSN="DSN=RTLib"
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
      DSN="DSN=RTLib"
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
      DSN="DSN=RTLib"
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
      DSN="DSN=RTLib"
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
      DSN="DSN=RTLib"
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
Sub SrGetBranchBuss(accessMode,sw,protect,buss,BRANCH,bussman,s,t,U)
    'USERTYPE:指登入系統的使用者類別=>'1'表營業員 '2'表業務員
    'USERID:指登入系統之使用者身份證字號
    Dim conn,rs,sql,sx,USERTYPE,USERID
    'USERTYPE=SESSION("LOGONUSRTYPE")
    'USERID=SESSION("LOGONUSERID")
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
    '券商
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 Then 
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
          &"WHERE (RTObjLink.CUSTYID = '06') AND RTOBJ.CUSID NOT IN ('70770184', '47224065') "
       s="<option value="""" >(證券公司)</option>"
    Else
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC  " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
          &"WHERE (RTObjLink.CUSTYID = '06') AND RTOBJ.CUSID='" & BUSS & "'"
       s=""
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(證券公司)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CUSID")=BUSS Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
    '分行
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 Then   
       SQL="SELECT * FROM RTBRANCH WHERE CUSID='" & BUSS &"' ORDER BY BRANCH "
       rs.Open SQL,conn     
       t=""
       If rs.Eof Then t="<option value="""" >(分行)</option>"  
       sx=""    
       Do While Not rs.Eof    
          If rs("BRANCH")=BRANCH Then sx=" selected "    
          t=t &"<option value=""" &rs("BRANCH") &"""" &sx &">" &rs("BRANCH") &"</option>"    
          rs.MoveNext    
          sx=""    
       Loop    
       rs.Close    
    Else    
       t="<option value=""" &BRANCH &""" selected>" &BRANCH &"</option>"    
    End If
    '營業員
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 Then    
       rs.Open "SELECT RTBUSSMAN.CUSID,RTOBJ.CUSNC FROM RTBussMan INNER JOIN RTObj ON RTBussMan.CUSID = RTObj.CUSID " _
             & "WHERE STOCKID='" & BUSS &"' AND BRANCH='" & BRANCH & "' ORDER BY BRANCH ",conn    
       U=""
       If rs.Eof Then u="<option value="""" >(營業員)</option>"  
       sx=""    
       Do While Not rs.Eof    
          If rs("CUSID")=BUSSMAN Then sx=" selected "    
          U=U &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("CUSNC") &"</option>"    
          rs.MoveNext    
          sx=""    
       Loop    
       rs.Close    
    Else    
       U="<option value=""" &bussman &""" selected>" &bussman &"</option>"    
    End If    
    conn.Close
    Set rs=Nothing
    Set conn=Nothing 
End Sub
%>    
<%
'role=0:全部人員皆不可輸入;role=1:業務助理;role=2:業務;role=4:技術部;role=8:財會;role=16:洪協理
'ROLE=31:資訊部管理員
Function FrGetUserlevel(user)
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
   FrGetUserlevel=Role
   rs.close
   conn.close
   set rs=nothing
   set conn=nothing
End Function  
%>
<%
  Function FrGetCmtyDesc(Comq1)
  Dim s,conn,rs,sql
  Set conn=Server.CreateObject("ADODB.Connection")  
  Set rs=Server.CreateObject("ADODB.Recordset")
  conn.Open DSN
  sql="SELECT COMN, EQUIPADDR FROM rtsparqadslcmty WHERE CUTYID=" & comq1
  rs.Open sql,conn
  s=""
  If rs.Eof Then
     s="社區代號:" &Comq1 &"在社區基本資料內找不到"
  Else 
     s="社區名稱:" &rs("ComN") & "  機櫃位置:" & rs("equipaddr")
  End If
  rs.Close
  Set rs=Nothing
  conn.Close
  Set conn=Nothing
  FrGetCmtyDesc=s
End Function
%>