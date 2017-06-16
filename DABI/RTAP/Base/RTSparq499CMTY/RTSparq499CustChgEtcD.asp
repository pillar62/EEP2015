<%
  Dim fieldRole,fieldPa
  fieldRole=Split(FrGetUserRight("RTCustD",Request.ServerVariables("LOGON_USER")),";")
%>
<%	'#include virtual="/WebUtilityV4EBT/DBAUDI/cType.inc"
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

<%	'#include virtual="/WebUtilityV4EBT/DBAUDI/dataList.inc"
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
  'btnEdit="<input type=""button"" class=dataListButton id=""btnEdit"" value=""" _
  '       &btnEditName &""" style=""cursor:hand;""" _
  '       &" onClick=""vbscript:sw.Value='E':Window.form.Submit"">" 
  btnExit="<input type=""button""  class=dataListButton id=""btnExit"" value=""" _
         &btnExitName &""" style=""cursor:hand;"" onClick=""vbscript:window.close"">"
  'btnNew ="<input type=""button"" class=dataListButton id=""btnNew"" value=""" _
  '       &btnNewName &""" style=""cursor:hand;""" _
  '       &" onClick=""vbscript:sw.Value='':accessMode.Value='A':Window.form.Submit"">" 
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
  Dim aryKeyName,aryKeyType(200),aryKeyValue(200),numberOfField,aryKey,aryKeyNameDB(200)
  Dim dspKey(200),userDefineKey,userDefineData,extDBField,extDB(200),userDefineRead,userDefineSave
  Dim conn,rs,i,formatName,sqlList,sqlFormatDB,userdefineactivex
  Dim aryParmKey
 '90/09/03 ADD-START
 '增加EXTDBFIELD2,EXTDBFILELD3(多檔維護)
  dim extDBField2,extDB2(200),extDBField3,extDB3(200),extDBField4,extDB4(200)
  extDBField=0  
  extDBfield2=0
  extDBField3=0
  extDBField4=0
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
	' CUSID
	dspKey(1)=aryParmKey(2)

    'Dim i,sType
    'For i = 0 To Ubound(aryParmKey)
    '   If Len(Trim(aryParmKey(i))) > 0 Then
    '       dspKey(i)=aryParmKey(i)
    '    End If
    'Next
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
'--------------------------------------------------------------------------------------------
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
'response.write "sType=" & sType & ";I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                 'On Error Resume Next
                 
                 'if i <> 1 then rs.Fields(i).Value=dspKey(i)
				 'if i=1 then
					'Set rsc=Server.CreateObject("ADODB.Recordset")
					'rsc.open "select max(entryno) AS entryno from RTSparqVoIPCustChg where cusid='" & dspkey(0) & "' " ,conn
					'if len(rsc("entryno")) > 0 then
					'	dspkey(i)=rsc("entryno") + 1
					'else
					'	dspkey(i)=1
					'end if
                    'rsc.close
                    rs.Fields(i).Value=dspKey(i) 
				 'end if      
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
'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"              
         '     On Error Resume Next
              runpgm=Request.ServerVariables("PATH_INFO") 
'              select case ucase(runpgm)   
'                 case ucase("/webap/rtap/base/RTSparqVoIPCust/RTSparqVoIPCustChgD.asp")
'                     if i<>0 then rs.Fields(i).Value=dspKey(i)
'                 case else
                     rs.Fields(i).Value=dspKey(i)
                     'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
'               end select
          Next
          rs.Update
          rwCnt=rwCnt+1
          If userDefineSave="Yes" Then Call SrSaveExtDB("U")
          sw=""
       End If
    End If
    rs.Close
    ' 當程式為HB社區基本資料維護作業時,將sql自行產生之identify值dspkey(0)讀出至畫面
    'if accessmode ="A" then
    '   runpgm=Request.ServerVariables("PATH_INFO")
    '      rsc.open "select max(entryno) AS ENTRYNO from RTSparqVoIPCustChg where cusid='" & dspkey(1) & "' " ,conn
    '      if not rsC.eof then
    '        dspkey(1)=rsC("entryno")
    '      end if
    '      rsC.close
    'end if
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
    If  rs.Eof Then
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
       sType=Right("000" & Cstr(aryKey(i)),3)
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
  title="速博499用戶資料其他異動"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT MODIFYNO, CUSID, MODIFYCODE, MODIFYDESC, EUSR, EDAT, nCOMQ1, "_
			 &"nLINEQ1, nCUSNC, nFIRSTIDTYPE, nSOCIALID, nSECONDIDTYPE, "_
			 &"nSECONDNO, nBIRTHDAY, nEMAIL, nCONTACTTEL, nMOBILE, nCUTID1, "_
			 &"nTOWNSHIP1, nRADDR1, nRZONE1, nCUTID2, nTOWNSHIP2, nRADDR2, "_
			 &"nRZONE2, nCUTID3, nTOWNSHIP3, nRADDR3, nRZONE3, nCOCONTACT, "_
			 &"nCOCONTACTTEL, nCOCONTACTTELEXT, nCOMOBILE, nCOBOSS, "_
			 &"nCOBOSSSOCIAL, nTRADETYPE, nAREAID, nGROUPID, nSALESID, nCASETYPE, "_
			 &"nFREECODE, nPMCODE, nPAYTYPE, nAGENTNAME, nAGENTSOCIAL, "_
			 &"nAGENTTEL, nRCVD, nAPPLYDAT, nFINISHDAT, nDOCKETDAT, nTRANSDAT, "_
			 &"nDROPDAT, nCANCELDAT, nCANCELUSR, nOVERDUE, nMEMO, "_
			 &"nMOVETOCOMQ1, nMOVETOLINEQ1, nMOVEFROMCOMQ1, "_
			 &"nMOVEFROMLINEQ1, nMOVETODAT, nMOVEFROMDAT, nNCICCUSNO, "_
			 &"nCUSTIP1, nCUSTIP2, nCUSTIP3, nCUSTIP4, nSPHNNO, nOLDCUSTIP1, "_
			 &"nOLDCUSTIP2, nOLDCUSTIP3, nOLDCUSTIP4, nCREDITTYPE, nCREDITBANK, "_
			 &"nCREDITNO, nCREDITNAME, nVALIDMONTH, nVALIDYEAR, nDEVELOPERID, "_
			 &"nSETEMPLY, nREALRCVAMT, nGAMT, nAPPLYNO, oCOMQ1, oLINEQ1, oCUSNC, "_
			 &"oFIRSTIDTYPE, oSOCIALID, oSECONDIDTYPE, oSECONDNO, oBIRTHDAY, "_
			 &"oEMAIL, oCONTACTTEL, oMOBILE, oCUTID1, oTOWNSHIP1, oRADDR1, "_
			 &"oRZONE1, oCUTID2, oTOWNSHIP2, oRADDR2, oRZONE2, oCUTID3, "_
			 &"oTOWNSHIP3, oRADDR3, oRZONE3, oCOCONTACT, oCOCONTACTTEL, "_
			 &"oCOCONTACTTELEXT, oCOMOBILE, oCOBOSS, oCOBOSSSOCIAL, "_
			 &"oTRADETYPE, oAREAID, oGROUPID, oSALESID, oCASETYPE, oFREECODE, "_
			 &"oPMCODE, oPAYTYPE, oAGENTNAME, oAGENTSOCIAL, oAGENTTEL, oRCVD, "_
			 &"oAPPLYDAT, oFINISHDAT, oDOCKETDAT, oTRANSDAT, oDROPDAT, "_
			 &"oCANCELDAT, oCANCELUSR, oOVERDUE, oMEMO, oMOVETOCOMQ1, "_
			 &"oMOVETOLINEQ1, oMOVEFROMCOMQ1, oMOVEFROMLINEQ1, oMOVETODAT, "_
			 &"oMOVEFROMDAT, oNCICCUSNO, oCUSTIP1, oCUSTIP2, oCUSTIP3, oCUSTIP4, "_
			 &"oSPHNNO, oOLDCUSTIP1, oOLDCUSTIP2, oOLDCUSTIP3, oOLDCUSTIP4, "_
			 &"oCREDITTYPE, oCREDITBANK, oCREDITNO, oCREDITNAME, oVALIDMONTH, "_
			 &"oVALIDYEAR, oDEVELOPERID, oSETEMPLY, oREALRCVAMT, oGAMT, "_
			 &"oAPPLYNO "_
			 &"FROM RTSparq499CustChgEtc "_
			 &"WHERE MODIFYNO='' "

  sqlList    ="SELECT MODIFYNO, CUSID, MODIFYCODE, MODIFYDESC, EUSR, EDAT, nCOMQ1, "_
			 &"nLINEQ1, nCUSNC, nFIRSTIDTYPE, nSOCIALID, nSECONDIDTYPE, "_
			 &"nSECONDNO, nBIRTHDAY, nEMAIL, nCONTACTTEL, nMOBILE, nCUTID1, "_
			 &"nTOWNSHIP1, nRADDR1, nRZONE1, nCUTID2, nTOWNSHIP2, nRADDR2, "_
			 &"nRZONE2, nCUTID3, nTOWNSHIP3, nRADDR3, nRZONE3, nCOCONTACT, "_
			 &"nCOCONTACTTEL, nCOCONTACTTELEXT, nCOMOBILE, nCOBOSS, "_
			 &"nCOBOSSSOCIAL, nTRADETYPE, nAREAID, nGROUPID, nSALESID, nCASETYPE, "_
			 &"nFREECODE, nPMCODE, nPAYTYPE, nAGENTNAME, nAGENTSOCIAL, "_
			 &"nAGENTTEL, nRCVD, nAPPLYDAT, nFINISHDAT, nDOCKETDAT, nTRANSDAT, "_
			 &"nDROPDAT, nCANCELDAT, nCANCELUSR, nOVERDUE, nMEMO, "_
			 &"nMOVETOCOMQ1, nMOVETOLINEQ1, nMOVEFROMCOMQ1, "_
			 &"nMOVEFROMLINEQ1, nMOVETODAT, nMOVEFROMDAT, nNCICCUSNO, "_
			 &"nCUSTIP1, nCUSTIP2, nCUSTIP3, nCUSTIP4, nSPHNNO, nOLDCUSTIP1, "_
			 &"nOLDCUSTIP2, nOLDCUSTIP3, nOLDCUSTIP4, nCREDITTYPE, nCREDITBANK, "_
			 &"nCREDITNO, nCREDITNAME, nVALIDMONTH, nVALIDYEAR, nDEVELOPERID, "_
			 &"nSETEMPLY, nREALRCVAMT, nGAMT, nAPPLYNO, oCOMQ1, oLINEQ1, oCUSNC, "_
			 &"oFIRSTIDTYPE, oSOCIALID, oSECONDIDTYPE, oSECONDNO, oBIRTHDAY, "_
			 &"oEMAIL, oCONTACTTEL, oMOBILE, oCUTID1, oTOWNSHIP1, oRADDR1, "_
			 &"oRZONE1, oCUTID2, oTOWNSHIP2, oRADDR2, oRZONE2, oCUTID3, "_
			 &"oTOWNSHIP3, oRADDR3, oRZONE3, oCOCONTACT, oCOCONTACTTEL, "_
			 &"oCOCONTACTTELEXT, oCOMOBILE, oCOBOSS, oCOBOSSSOCIAL, "_
			 &"oTRADETYPE, oAREAID, oGROUPID, oSALESID, oCASETYPE, oFREECODE, "_
			 &"oPMCODE, oPAYTYPE, oAGENTNAME, oAGENTSOCIAL, oAGENTTEL, oRCVD, "_
			 &"oAPPLYDAT, oFINISHDAT, oDOCKETDAT, oTRANSDAT, oDROPDAT, "_
			 &"oCANCELDAT, oCANCELUSR, oOVERDUE, oMEMO, oMOVETOCOMQ1, "_
			 &"oMOVETOLINEQ1, oMOVEFROMCOMQ1, oMOVEFROMLINEQ1, oMOVETODAT, "_
			 &"oMOVEFROMDAT, oNCICCUSNO, oCUSTIP1, oCUSTIP2, oCUSTIP3, oCUSTIP4, "_
			 &"oSPHNNO, oOLDCUSTIP1, oOLDCUSTIP2, oOLDCUSTIP3, oOLDCUSTIP4, "_
			 &"oCREDITTYPE, oCREDITBANK, oCREDITNO, oCREDITNAME, oVALIDMONTH, "_
			 &"oVALIDYEAR, oDEVELOPERID, oSETEMPLY, oREALRCVAMT, oGAMT, "_
			 &"oAPPLYNO "_
			 &"FROM RTSparq499CustChgEtc "_
			 &"WHERE "

  userDefineRead="Yes"
  userDefineSave="Yes"
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
	'公關機
	if len(trim(dspkey(40)))=0 then dspkey(40)=""
	if len(trim(dspkey(117)))=0 then dspkey(117)=""
	'OLD IP
	if len(trim(dspkey(68)))=0 then dspkey(68)=""
	if len(trim(dspkey(69)))=0 then dspkey(69)=""
	if len(trim(dspkey(70)))=0 then dspkey(70)=""
	if len(trim(dspkey(71)))=0 then dspkey(71)=""
	'OLD IP
	if len(trim(dspkey(145)))=0 then dspkey(145)=""
	if len(trim(dspkey(146)))=0 then dspkey(146)=""
	if len(trim(dspkey(147)))=0 then dspkey(147)=""
	if len(trim(dspkey(148)))=0 then dspkey(148)=""
	'信用卡類型
	if len(trim(dspkey(72)))=0 then dspkey(72)=""
	if len(trim(dspkey(149)))=0 then dspkey(149)=""

  'if len(trim(dspkey(73)))=0 then dspkey(73)=""
  'if len(trim(dspkey(132)))<>0 then 
  '   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
  '   V=split(rtnvalue, ";")  
  '   DSpkey(133)=V(0)
  'end if   
'  IF instr(1,dspkey(67),"-",1) <> 0 THEN
'  RESPONSE.Write "AAA=" & instr(1,dspkey(67),"-",1) & "<BR>"
'  RESPONSE.Write "BBB=" & instr(1,dspkey(69),"-",1) 
'  RESPONSE.END
'  ELSE
'  RESPOSNE.WRITE "XXX"
'  RESPONSE.End
'  END IF

  '身份證檢查 -------------------------
  DSPKEY(10)=UCASE(DSPKEY(10))		'身份證第一碼大寫
  LEADINGCHAR=LEFT(DSPKEY(10),1)	'身份證欄位第一碼,用以判別是個人還是公司,若為公司則出生日期必須空白,反之則不可空白
  IF dspkey(10) ="02" THEN '公司統編
	 COMPANY="Y"     
  ELSE
     COMPANT="N"  
	 if (len(trim(dspkey(10)))=0 or (len(trim(dspkey(10)))<>10 and len(trim(dspkey(10)))<>8 ) ) AND DSPKEY(40) <> "Y" then
		formValid=False
		message="除公關機外,用戶身份證(統編)不可空白或長度不對"
	 end if
  END IF
  
  if len(trim(dspkey(8)))=0 then
       formValid=False
       message="用戶名稱不可空白"          
  elseIf len(trim(dspkey(46)))=0 or Not Isdate(dspkey(46)) then
       formValid=False
       message="收件日不可空白或格式錯誤"    
  elseif len(trim(dspkey(47)))=0 then
       formValid=False
       message="用戶申請日不可空白"   
 ' elseif len(trim(dspkey(98)))=0 then
 '      formValid=False
 '      message="戶籍地址(縣市)不可空白"
 ' elseif len(trim(dspkey(99)))=0 and dspkey(98)<>"06" and dspkey(98)<>"15" then
 '      formValid=False
 '      message="戶籍地址(鄉鎮)不可空白"
 ' elseif len(trim(dspkey(100)))=0 then
 '      formValid=False
 '      message="戶籍地址(地址)不可空白"

  elseif len(trim(dspkey(21)))=0 then
       formValid=False
       message="裝機地址(縣市)不可空白"   
  elseif len(trim(dspkey(22)))=0 and dspkey(21)<>"06" and dspkey(21)<>"15" then
       formValid=False
       message="裝機地址(鄉鎮)不可空白"
  elseif len(trim(dspkey(23)))=0 then
       formValid=False
       message="裝機地址(地址)不可空白"

  elseif len(trim(dspkey(25)))=0 then
       formValid=False
       message="帳單地址(縣市)不可空白"   
  elseif len(trim(dspkey(26)))=0 and dspkey(25)<>"06" and dspkey(25)<>"15" then
       formValid=False
       message="帳單地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(27)))=0 then
       formValid=False
       message="帳單地址(地址)不可空白"   

'  elseif (len(trim(dspkey(6)))=0 or Not Isdate(dspkey(6))) AND COMPANY="N" then
'       formValid=False
'       message="用戶為個人時，出生日期不可空白或格式錯誤"   
  elseif len(trim(dspkey(2)))=0 then
       formValid=False
       message="異動類別不能空白"
  elseif len(trim(dspkey(15)))=0 and len(trim(dspkey(16)))=0 then
       formValid=False
       message="用戶連絡電話及行動電話至少須輸入一項"   
  elseif instr(1,dspkey(15),"-",1) > 0 then
       formValid=False
       message="連絡電話不可包含'-'符號"         
  elseif instr(1,dspkey(16),"-",1) > 0 then
       formValid=False
       message="行動電話不可包含'-'符號"          
  elseif len(trim(dspkey(29)))=0  AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，企業連絡人不可空白"         
  elseif len(trim(dspkey(30)))=0  AND len(trim(dspkey(32)))=0 AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，企業連絡人連絡電話及行動電話至少需輸入一項"    
  elseif len(trim(dspkey(33)))=0  AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，企業負責人不可空白"
  elseif len(trim(dspkey(34)))=0  AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，企業負責人身份證字號不可空白"                     
  elseif len(trim(dspkey(39)))= 0 then
       formValid=False
       message="方案種類不可空白"
  'elseif len(trim(dspkey(161)))=0 and len(trim(dspkey(38))) = 0 then
  '     message="經銷商與業務員不可同時空白!"
  '     formValid=False
  'elseif len(trim(dspkey(40)))= 0 AND DSPKEY(38) <> "Y" then
  '     formValid=False
  '     message="AVS繳款方式不可空白"      
  'elseif len(trim(dspkey(40)))> 0 AND DSPKEY(38) = "Y" then
  '     formValid=False
  '     message="公關機時，AVS繳款方式必須空白"           
  elseif len(trim(dspkey(48)))<> 0 AND len(trim(dspkey(49)))= 0 and dspkey(40)<>"Y" then
       formValid=False
       message="完工日期為空白時不可輸入報竣日"       
  'elseif len(trim(dspkey(46)))<> 0 AND ( len(trim(dspkey(55)))= 0 or len(trim(dspkey(56)))= 0 or len(trim(dspkey(57)))= 0 or len(trim(dspkey(58)))= 0 )then
  '     formValid=False
  '     message="輸入完工日期時，用戶IP不可空白"              
  end if

  IF formValid=TRUE THEN
    IF dspkey(10) <> "" and dspkey(9) ="01" then
       idno=dspkey(10)
        if UCASE(left(idno,1)) >="A" AND UCASE(left(idno,1)) <="Z" THEN
          AAA=CheckID(idno)
          SELECT CASE AAA
             CASE "True"
             case "False"
                   message="申請用戶身份證字號不合法"
                   formvalid=false 
             case "ERR-1"
                   message="申請用戶身份證號不可留空白或輸入位數錯誤"
                   formvalid=false       
             case "ERR-2"
                   message="申請用戶身份證字號的第一碼必需是合法的英文字母"
                   formvalid=false    
             case "ERR-3"
                   message="申請用戶身份證字號的第二碼必需是數字 1 或 2"
                   formvalid=false   
             case "ERR-4"
                   message="申請用戶身份證字號的後九碼必需是數字"
                   formvalid=false              
             case else
          end select  
       ELSE
          AAA=ValidBID(idno)
          if aaa = false then
              message="申請用戶統一編號不合法"
              formvalid=false   
          end if
       END IF
    END IF
  END IF

  IF formValid=TRUE THEN
   if len(trim(dspkey(34)))<> 0 then
      idno=dspkey(34)
        if UCASE(left(idno,1)) >="A" AND UCASE(left(idno,1)) <="Z" THEN
          AAA=CheckID(idno)
          SELECT CASE AAA
             CASE "True"
             case "False"
                   message="企業負責人身份證字號不合法"
                   formvalid=false 
             case "ERR-1"
                   message="企業負責人身份證號不可留空白或輸入位數錯誤"
                   formvalid=false       
             case "ERR-2"
                   message="企業負責人身份證字號的第一碼必需是合法的英文字母"
                   formvalid=false    
             case "ERR-3"
                   message="企業負責人身份證字號的第二碼必需是數字 1 或 2"
                   formvalid=false   
             case "ERR-4"
                   message="企業負責人身份證字號的後九碼必需是數字"
                   formvalid=false              
             case else
          end select  
       ELSE
          AAA=ValidBID(idno)
          if aaa = false then
              message="企業負責人統一編號不合法"
              formvalid=false   
          end if
       END IF
   END IF
  END IF
  
  '檢查主線開發為直銷或經銷==當經銷時,則績效歸屬部份為空白,反之則必須輸入
  'IF formValid=TRUE THEN
  ' Set connxx=Server.CreateObject("ADODB.Connection")
  ' Set rsxx=Server.CreateObject("ADODB.Recordset")
  ' connxx.open DSN
  ' sqlxx="select * from RTSparq499Cmtyline where comq1=" & aryparmkey(0) & " AND LINEQ1=" & ARYPARMKEY(1)
  ' rsxx.Open sqlxx,connxx
  ' if not rsxx.eof then
  '    if len(trim(rsxx("consignee"))) <> 0 then
  '       if len(trim(dspkey(34))) <> 0 or len(trim(dspkey(35))) <> 0 or len(trim(dspkey(36))) <> 0then
  '          formValid=False
  '          message="主線開發為經銷商,績效歸屬必須空白" 
  '       end if
  '    else
  '       if len(trim(dspkey(34))) = 0 or len(trim(dspkey(35))) = 0 or len(trim(dspkey(36))) = 0 then
  '          formValid=False
  '          message="主線開發為直銷,績效歸屬不可空白" 
  '       end if
  '    end if
      '主線未測通者，不可輸入avs申請日
  '    if isnull(rsxx("ADSLOPENDAT")) and len(trim(dspkey(46))) <> 0 then
  '          formValid=False
  '          message="主線未測通，不可輸入用戶完工日" 
  '    ELSEif isnull(rsxx("ADSLOPENDAT")) and len(trim(dspkey(47))) <> 0 then
  '          formValid=False
  '          message="主線未測通，不可輸入用戶報竣日" 
  '    end if

  '   IF NOT ISNULL(RSXX("DROPDAT")) OR NOT ISNULL(RSXX("CANCELDAT")) THEN
  '      formValid=False
  '      message="主線已作廢或撤銷，不可新增及異動用戶資料" 
  '   END IF
  ' end if
  ' rsxx.close
  ' connxx.Close   
  ' set rsxx=Nothing   
  ' set connxx=Nothing 
  'END IF
  
'-------UserInformation----------------------       
    logonid=session("userid")
'    if dspmode="修改" then
'        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
'                V=split(rtnvalue, ";")  
'                DSpkey(6)=V(0)
'        dspkey(7)=datevalue(now())
'    end if  
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

   Sub Srcounty12onclick()
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

   Sub Srcounty16onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY21").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key22").value =  trim(Fusrid(0))
          document.all("key24").value =  trim(Fusrid(1))
       End if       
       end if
    END SUB

   Sub Srcounty20onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY25").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key26").value =  trim(Fusrid(0))
          document.all("key28").value =  trim(Fusrid(1))
       End if       
       end if
    END SUB

  Sub SrSalesGroupOnClick()
       prog="RTGetsalesgroupD.asp"
       prog=prog & "?KEY=" & document.all("KEY36").VALUE 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
		  FUsrID=Split(Fusr,";")   
		  if Fusrid(2) ="Y" then
			 document.all("key37").value =  trim(Fusrid(0))
		  End if       
       end if
   End Sub

   Sub Srsalesonclick()
       prog="RTGetsalesD.asp"
       prog=prog & "?KEY=" & document.all("KEY36").VALUE & ";" & document.all("KEY37").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
		  FUsrID=Split(Fusr,";")   
		  if Fusrid(2) ="Y" then
			 document.all("key38").value =  trim(Fusrid(0))
		  End if       
       end if
   End Sub

   Sub SrDeveloperonclick()
       prog="RTGetDeveloperD.asp"
       prog=prog & "?KEY=" & document.all("KEY78").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key78").value =  trim(Fusrid(0))
       End if
       end if
   End Sub

   Sub SrSetEmplyonclick()
       prog="RTGetSetEmplyD.asp"
       prog=prog & "?KEY=" & document.all("KEY79").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key79").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub      

   Sub SrSetComnOnClick()
       prog="RTGetCmty.asp"
       prog=prog & "?KEY=" & document.all("KEY6").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key6").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub      

Sub SrAddrEqual1()
    document.All("key21").value=document.All("key17").value
    document.All("key22").value=document.All("key18").value
    document.All("key23").value=document.All("key19").value
    document.All("key24").value=document.All("key20").value
End Sub 
Sub SrAddrEqual2()
    document.All("key25").value=document.All("key17").value
    document.All("key26").value=document.All("key18").value
    document.All("key27").value=document.All("key19").value
    document.All("key28").value=document.All("key20").value
End Sub         
Sub SrAddrEqual3()
    document.All("key25").value=document.All("key21").value
    document.All("key26").value=document.All("key22").value
    document.All("key27").value=document.All("key23").value
    document.All("key28").value=document.All("key24").value
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
	<%
		Dim  conn,rs
		' Uyyyymmddxxxx
		modifynoxx="U" & datePART("yyyy",NOW()) & right("0"&datePART("m",NOW()),2) & right("0"&datePART("d",NOW()),2)
		Set conn=Server.CreateObject("ADODB.Connection")
		Set rs=Server.CreateObject("ADODB.Recordset")
		conn.open DSN
		rs.open "select max(MODIFYNO) AS modifyno from RTSparq499CustChgEtc ",conn
		if len(rs("modifyno")) > 0 then
			dspkey(0)=modifynoxx & right("000" & cstr(cint(right(rs("modifyno"),4)) + 1),4)
		else
			dspkey(0)=modifynoxx & "0001"
		end if
		rs.close
		conn.close
		set rs=nothing
		set	conn=nothing
	%>
		<tr><td width="15%" class=dataListHead>異動單號</td>
			<td width="10%"  bgcolor="silver">
			<input type="text" name="key0" size="15" value="<%=dspKey(0)%>" readonly <%=fieldRole(1)%> class=dataListdata></td>

			<td width="25%" class=dataListHead>用戶序號</td>
			<td width="25%"  bgcolor="silver">
			<input type="text" name="key1" size="15" value="<%=dspKey(1)%>" readonly <%=fieldRole(1)%> class=dataListdata></td>
		</tr>
	</table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
	Dim  conn,rs,s,sx,t,sql,rsc
	Set conn=Server.CreateObject("ADODB.Connection")
	Set rs=Server.CreateObject("ADODB.Recordset")    
	conn.open DSN	
	
    logonid=session("userid")
    if dspmode="新增" then
		'建檔人員 & 建檔日期----------------------------------------    
        if len(trim(dspkey(4))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
           V=split(rtnvalue,";")  
           dspkey(4)=V(0)
        End if  
        dspkey(5)=datevalue(now())
        ' 所有欄位先帶入原始預設資料 --------------------------------    
		sql= "SELECT CUSID, COMQ1, LINEQ1, CUSNC, FIRSTIDTYPE, SOCIALID, SECONDIDTYPE, "_
			&"SECONDNO, BIRTHDAY, EMAIL, CONTACTTEL, MOBILE, CUTID1, TOWNSHIP1, "_
			&"RADDR1, RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, CUTID3, "_
			&"TOWNSHIP3, RADDR3, RZONE3, COCONTACT, COCONTACTTEL, "_
			&"COCONTACTTELEXT, COMOBILE, COBOSS, COBOSSSOCIAL, TRADETYPE, "_
			&"AREAID, GROUPID, SALESID, CASETYPE, "_
			&"FREECODE, PMCODE, PAYTYPE, AGENTNAME, AGENTSOCIAL, AGENTTEL, "_
			&"RCVD, APPLYDAT, FINISHDAT, DOCKETDAT, TRANSDAT, DROPDAT, "_
			&"CANCELDAT, CANCELUSR, OVERDUE, MEMO, MOVETOCOMQ1, "_
			&"MOVETOLINEQ1, MOVEFROMCOMQ1, MOVEFROMLINEQ1, MOVETODAT, "_
			&"MOVEFROMDAT, NCICCUSNO, CUSTIP1, CUSTIP2, CUSTIP3, CUSTIP4, "_
			&"SPHNNO, OLDCUSTIP1, OLDCUSTIP2, OLDCUSTIP3, OLDCUSTIP4, "_
			&"CREDITTYPE, CREDITBANK, CREDITNO, CREDITNAME, VALIDMONTH, "_
			&"VALIDYEAR, DEVELOPERID, SETEMPLY, REALRCVAMT, GAMT, APPLYNO "_
			&"FROM RTSparq499Cust "_
			&"WHERE cusid ='" &dspKey(1) &"' "
'response.write sql
		rs.Open sql, conn
    	For i = 1 To rs.Fields.Count-1
			dspkey(i+5)=rs.Fields(i).Value
		  	dspkey(i+82)=rs.Fields(i).Value
		Next
		rs.Close
'    else
'        if len(trim(dspkey(6))) < 1 then
'           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
'                V=split(rtnvalue,";")  
'                DSpkey(6)=V(0)
'        End if         
'        dspkey(7)=datevalue(now())
    end if
         
' -------------------------------------------------------------------------------------------- 
    '異動前資料PROTECT
    fieldLock=" class=""dataListData"" readonly "
    '完工後基本資料PROTECT
    'If len(trim(dspKey(46))) > 0 Then
    '   fieldPa=" class=""dataListData"" readonly "
    '   fieldpb=" disabled "
    'Else
    '   fieldPa=""
    '   fieldpb=""
    'End If
    '報竣日輸入後，寬頻服務+代理人+績效資料PROTECT
    'If len(trim(dspKey(47))) > 0 Then
    '   fieldPC=" class=""dataListData"" readonly "
    '   fieldpD=" disabled "
    'Else
    '   fieldPC=""
    '   fieldpD=""
    'End If
    '報竣轉檔後，報竣日期PROTECT
    'If len(trim(dspKey(48))) > 0 Then
    '   fieldPe=" class=""dataListData"" readonly "
    '   fieldpf=" disabled "
    'Else
    '   fieldPe=""
    '   fieldpf=""
    'End If    
%>
  
<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='M7' " 
       If len(trim(dspkey(2))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='M7' AND CODE='" & dspkey(2) &"' " 
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(異動類別)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(2) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
	<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table11">
	<tr><td width="15%" class="dataListHEAD" height="23">異動類別</td>
		<td width="85%" bgcolor="silver" colspan=3>
			<select name="key2"   <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry"><%=s%></select>
		</td>
	</tr>

	<tr><td width="15%" class="dataListHEAD" height="23">異動說明</td>
		<td width="85%" bgcolor="silver" colspan=3>
        <input type="text" name="key3" size="100" maxlength="255" value="<%=dspKey(3)%>"  <%=fieldRole(1)%> <%=dataProtect%> class=dataListENTRY></td>
	</tr>

<%  
	name="" 
	if dspkey(4) <> "" then
		sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			&"where rtemployee.emply='" & dspkey(4) & "' "
		rs.Open sql,conn
		if rs.eof then
			name=""
		else
			name=rs("cusnc")
		end if
		rs.close
	end if
%>    
	<tr><td class="dataListHEAD" height="23">建檔人員</td>
        <td height="23" bgcolor="silver">
			<input type="text" name="key4" size="6" READONLY value="<%=dspKey(4)%>" <%=fieldRole(1)%> class="dataListDATA">
			<font size=2><%=name%></font>
		</td>
		
        <td class="dataListHEAD" height="23">建檔日期</td>
        <td height="23" bgcolor="silver">
        	<input type="text" name="key5" size="10" READONLY value="<%=dspKey(5)%>" <%=fieldRole(1)%> class="dataListDATA">
		</td>
	</tr>
</table>

  <span id="tags1" class="dataListTagsOn"
        onClick="vbscript:tag1.style.display=''	   :tags1.classname='dataListTagsOn':
						  tag2.style.display='none':tags2.classname='dataListTagsOf'"><u>異動後資料</u> |</span>
  <span id="tags2" class="dataListTagsOf"
        onClick="vbscript:tag1.style.display='none':tags1.classname='dataListTagsOf':
                          tag2.style.display=''    :tags2.classname='dataListTagsOn'"><u>異動前資料</u></span>

<!-- 異動後資料 ========================================================================================================== -->
<!--
<table width="100%" ID="Table12" bgcolor="silver">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
		<tr><td>&nbsp;</td><td>
-->

<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
	<tr><td bgcolor="BDB76B" align="center" colspan="4">基本資料</td></tr>

	<tr><td width="15%" class=dataListHead>社區序號</td>
		<%
			name=""
			if dspkey(6) <> "" then
				sqlxx="select COMN from RTSparq499CmtyH where COMQ1=" & dspkey(6) 
				rs.Open sqlxx,conn
				if rs.eof then
					name="(對象檔找不該社區)"
				else
					name=rs("COMN")
				end if
				rs.close
			end if
		%>
		<td width="35%" bgcolor="silver">
    	    <input type="text" name="key6" value="<%=dspkey(6)%>" size="5" readonly <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry">
			<input type="button" name="B6" id="B6" value="...." onclick="SrSetComnOnClick()" width="100%" style="Z-INDEX: 1" >
			<font size=2><%=name%></font>
		</td>

		
		
		<td width="15%" class=dataListHead>主線序號</td>
		<td width="10%"  bgcolor="silver">
			<input type="text" name="key7" size="5" maxlength="2" value="<%=dspKey(7)%>" <%=fieldRole(1)%> <%=dataProtect%> class=dataListEntry>
		</td>
	</tr>

	<TR><td class="dataListHEAD" height="23">用戶IP</td>
        <td height="23" bgcolor="silver"  ><font color=red>
        	<input type="text" name="key63" size="3" maxlength="3" value="<%=dspKey(63)%>" <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
        		<font size=2>.</font>
        	<input type="text" name="key64" size="3" maxlength="3" value="<%=dspKey(64)%>" <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
        		<font size=2>.</font>
        	<input type="text" name="key65" size="3" maxlength="3" value="<%=dspKey(65)%>" <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
        		<font size=2>.</font>
        	<input type="text" name="key66" size="3" maxlength="3" value="<%=dspKey(66)%>" <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
        </font></td>

		<td class="dataListHEAD" height="23">對帳序號</td>
        <td height="23" bgcolor="silver">
			<input type="text" name="key62" size="12" maxlength="12" value="<%=dspKey(62)%>"  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
        	<FONT SIZE=2>-</FONT>
        	<input type="text" name="key67" size="3" maxlength="3" value="<%=dspKey(67)%>"  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
		</TD>
	</TR>

	<tr><td class="dataListHEAD" height="23">iDslam Port位</td>
        <td height="23" bgcolor="silver" colspan=3>
			<select size="1" name="key35" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select3">	
			<%
				if dspKey(35) ="" then 
					idslamport = ""
				else
					idslamport = " selected "
				end if
				response.Write "<option value="""" " &idslamport& ">(port位)</option>"
				
				for i =1 to 8 
					if dspKey(35) = cstr(i) then
						idslamport = " selected "
					else
						idslamport = ""
					end if
					response.Write "<option value=""" &i&""" "& idslamport& ">" &i& "</option>"
				next
			%>
			</select>	
		</td>
	</tr>

	<tr><td width="15%" class=dataListHEAD>收件日</td>
    	<td width="35%" bgcolor="silver" >
        	<input type="text" name="key46" value="<%=dspKey(46)%>" size="10" maxlength="10" READONLY  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
			<input type="button" name="B46" id="B46" onclick="SrBtnOnClick" value="...." height="100%" width="100%" style="Z-INDEX: 1" <%=fieldpb%> >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" name="C46" id="C46" onclick="SrClear" alt="清除" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" <%=fieldpb%> >
		</td>

		<td width="15%" class=dataListHEAD>用戶申請日</td>
   		<td width="35%" bgcolor="silver">
       		<input type="text" name="key47" value="<%=dspKey(47)%>" size="10" maxlength="10" READONLY  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
			<input type="button" name="B47" id="B47" onclick="SrBtnOnClick" value="...." height="100%" width="100%" style="Z-INDEX: 1" <%=fieldpb%> >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" name="C47" id="C47" onclick="SrClear" alt="清除" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" <%=fieldpb%> >
		</td>
	</tr>

	<tr><td width="15%" class=dataListHEAD>用戶(公司)名稱</td>
    	<td width="35%"  bgcolor="silver" >
       		<input type="text" name="key8" value="<%=dspKey(8)%>" size="30" maxlength="30"  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
		</td>

		<td width="15%" class=dataListHEAD>身分證(統編)</td>
		<%
    		s=""
		    sx=" selected "
    		If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then
				sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='J5' "
       			If len(trim(dspkey(9))) < 1 Then
        			  sx=" selected "
       			else
        			  sx=""
	       		end if
    		Else
       			sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='J5' AND CODE='" & dspkey(9) &"' " 
	    	End If
    		s=""
    		sx=""
	    	s=s &"<option value=""" &"""" &sx &">(第一證照別)</option>"
    		rs.Open sql,conn
    		Do While Not rs.Eof
	       		If rs("CODE")=dspkey(9) Then sx=" selected "
    	   		s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       			rs.MoveNext
       			sx=""
	    	Loop
    		rs.Close
		%>
    	<td width="35%" bgcolor="silver">
			<select name="key9"  <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry"><%=s%></select>
       		<input type="text" name="key10" value="<%=dspKey(10)%>" size="12" maxlength="10"  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
		</td>
	</tr>

	<tr><td width="15%" class=dataListHEAD>申請書編號</td>
    	<td  width="35%"  bgcolor="silver" >
   			<input type="text" name="key82" value="<%=dspKey(82)%>" size="15" maxlength="11"  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
		</td>

        <td width="10%" class="dataListHead" height="25">第二證照別及號碼</td>
		<%
		    s=""
    		sx=" selected "
    		If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
				sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='L3' " 
       			If len(trim(dspkey(11))) < 1 Then
          			sx=" selected " 
       			else
          			sx=""
       			end if
    		Else
       			sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='L3' AND CODE='" & dspkey(11) &"' " 
		    End If
		    rs.Open sql,conn
    		s=""
		    s=s &"<option value=""" &"""" &sx &">(第二證照別)</option>"
    		sx=""
    		Do While Not rs.Eof
		       If rs("CODE")=dspkey(11) Then sx=" selected "
       			s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
		       rs.MoveNext
		       sx=""
    		Loop
    		rs.Close
   		%>
    	<td width="35%" bgcolor="silver">
			<select name="key11"  <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry"><%=s%></select>
       		<input type="text" name="key12" value="<%=dspKey(12)%>" size="15" maxlength="15"  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
		</td>
	</tr>

	<tr><td class="dataListHEAD" height="23">出生日期</td>
   		<td width="35%" bgcolor="silver">
       		<input type="text" name="key13" value="<%=dspKey(13)%>" size="10" maxlength="10" <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
			<input type="button" name="B13" id="B13" onclick="SrBtnOnClick" value="...." height="100%" width="100%" style="Z-INDEX: 1" <%=fieldpb%> >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" name="C13" id="C13" onclick="SrClear" alt="清除" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" <%=fieldpb%> >
        </td>

        <td class="dataListHEAD" height="23">連絡EMAIL</td>
        <td height="23" bgcolor="silver" >
   			<input type="text" name="key14" value="<%=dspKey(14)%>" size="50" maxlength="50"  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
        </td>
 	</tr>

	<TR><td class="dataListHEAD" height="23">連絡電話</td>
        <td  height="23" bgcolor="silver" >
   			<input type="text" name="key15" value="<%=dspKey(15)%>" size="30" maxlength="30"  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
        </td>
        
        <td  class="dataListHEAD" height="23">行動電話</td>
        <td  height="23" bgcolor="silver">
   			<input type="text" name="key16" value="<%=dspKey(16)%>" size="30" maxlength="30"  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
		</td>
 	</tr>

	<tr><td class=dataListHEAD>戶籍/公司地址</td>
		<%
			s=""
    		sx=" selected "
    		If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)  Then 
				sql="SELECT Cutid,Cutnc FROM RTCounty " 
    	   		If len(trim(dspkey(17))) < 1 Then
					sx=" selected "
				else
					sx=""
    	   		end if
       			s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       			SXX12=" onclick=""Srcounty12onclick()""  "
	    	Else
    	   		sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(17) & "' " 
       			SXX12=""
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
		<td bgcolor="silver" colspan="3">
			<select name="key17"  <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry"><%=s%></select>
    	    <input type="text" name="key18" readonly size="8" value="<%=dspkey(18)%>" readonly <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
			<input type="button" name="B18" id="B18" value="...." width="100%" style="Z-INDEX: 1" <%=SXX12%> <%=fieldpb%> >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" name="C18" id="C18" onclick="SrClear" alt="清除" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" <%=fieldpb%> >
			<input type="text" name="key19" value="<%=dspKey(19)%>" size="40" maxlength="60"  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
			<input type="text" name="key20" value="<%=dspKey(20)%>" size="5" readonly  <%=fieldRole(1)%> <%=dataProtect%> class="dataListdata">
		</td></tr>

	<tr><td class=dataListHEAD>裝機地址<br>
			<input type="radio" name="rd1" onClick="SrAddrEqual1()" <%=fieldpb%> >同戶籍
		</td>
		<%
			s=""
    		sx=" selected "
	    	If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)  Then 
				sql="SELECT Cutid,Cutnc FROM RTCounty " 
       			If len(trim(dspkey(21))) < 1 Then
					sx=" selected "
				else
					sx=""
       			end if
	       		s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
    	   		SXX12=" onclick=""Srcounty16onclick()""  "
    		Else
	       		sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(21) & "' " 
    	   		SXX12=""
    		End If
	    	sx=""
    		rs.Open sql,conn
    		Do While Not rs.Eof
				If rs("cutid")=dspkey(21) Then sx=" selected "
				s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
				rs.MoveNext
				sx=""
	    	Loop
    		rs.Close
		%>
		<td bgcolor="silver" colspan="3">
			<select name="key21"  <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry"><%=s%></select>
    	    <input type="text" name="key22" readonly size="8" value="<%=dspkey(22)%>" readonly <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
			<input type="button" name="B22" id="B22" value="...." <%=SXX16%> <%=fieldpb%> width="100%" style="Z-INDEX: 1" >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" name="C22" id="C22" onclick="SrClear"  <%=fieldpb%> alt="清除" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
			<input type="text" name="key23" value="<%=dspKey(23)%>" size="40" maxlength="60"  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
			<input type="text" name="key24" value="<%=dspKey(24)%>" size="5" readonly  <%=fieldRole(1)%> <%=dataProtect%> class="dataListdata">
		</td>
	</tr>

	<tr><td class=dataListHEAD>帳單地址<br>
			<input type="radio" name="rd2" onClick="SrAddrEqual2()" <%=fieldpb%> ><font SIZE=2>同戶籍</font>
			<input type="radio" name="rd2" onClick="SrAddrEqual3()" <%=fieldpb%> ><font SIZE=2>同裝機</font>
    	</td>
		<%
			s=""
    		sx=" selected "
	    	If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)  Then 
				sql="SELECT Cutid,Cutnc FROM RTCounty " 
       			If len(trim(dspkey(25))) < 1 Then
					sx=" selected "
				else
					sx=""
       			end if
       			s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
	       		SXX12=" onclick=""Srcounty20onclick()""  "
    		Else
       			sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(25) & "' " 
       			SXX12=""
	    	End If
    		sx=""
    		rs.Open sql,conn
	    	Do While Not rs.Eof
				If rs("cutid")=dspkey(25) Then sx=" selected "
				s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
				rs.MoveNext
				sx=""
    		Loop
    		rs.Close
		%>
		<td bgcolor="silver" colspan="3">
			<select name="key25"  <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry"><%=s%></select>
    	    <input type="text" name="key26" readonly size="8" value="<%=dspkey(26)%>" readonly <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
			<input type="button" name="B26" id="B26" value="...." <%=SXX20%> <%=fieldpb%> width="100%" style="Z-INDEX: 1" >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" name="C26" id="C26" onclick="SrClear"  <%=fieldpb%> alt="清除" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
			<input type="text" name="key27" value="<%=dspKey(27)%>" size="40" maxlength="60"  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
			<input type="text" name="key28" value="<%=dspKey(28)%>" size="5" readonly  <%=fieldRole(1)%> <%=dataProtect%> class="dataListdata">
		</td>
	</tr>
	
	<tr><td class="dataListHEAD" height="23">企業連絡人</td>
        <td height="23" bgcolor="silver" >
			<input type="text" name="key29" value="<%=dspKey(29)%>" size="15" maxlength="15"  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
		</td>

        <td class="dataListHEAD" height="23">企業連絡電話</td>
        <td height="23" bgcolor="silver">
			<input type="text" name="key30" value="<%=dspKey(30)%>" size="15" maxlength="15"  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
	        <font size=2>分機︰</font>
			<input type="text" name="key31" value="<%=dspKey(31)%>" size="5" maxlength="5"  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
			<font size=2>手機︰</font>
			<input type="text" name="key32" value="<%=dspKey(32)%>" size="10" maxlength="10"  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry" ID="Text1">
	    </td>
	</tr>

	<tr><td class="dataListHEAD" height="23">企業負責人</td>
        <td height="23" bgcolor="silver" >
			<input type="text" name="key33" value="<%=dspKey(33)%>" size="10" maxlength="10"  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
		</td>

		<td class="dataListHEAD" height="23">負責人身份證號</td>
        <td height="23" bgcolor="silver" >
			<input type="text" name="key34" value="<%=dspKey(34)%>" size="10" maxlength="10"  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
		</td>
	</tr>

	<tr><td bgcolor="BDB76B" align="center" colspan="4">績效歸屬</td></tr>

	<tr><td WIDTH="15%" class="dataListHEAD" height="23">業務轄區</td>
		<%
			If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 Then 
       			sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '1') "
       			s="<option value="""" >(業務轄區)</option>"
    		Else
       			sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '1') AND AREAID='" & DSPKEY(36) & "' "
       			s="<option value="""" >(業務轄區)</option>"
    		End If
    		rs.Open sql,conn
    		If rs.Eof Then s="<option value="""" >(業務轄區)</option>"
    		sx=""
    		Do While Not rs.Eof
       			If rs("areaid")=dspkey(36) Then sx=" selected "
       			s=s &"<option value=""" &rs("areaid") &"""" &sx &">" &rs("areanc") &"</option>"
       			rs.MoveNext
       			sx=""
    		Loop
    		rs.Close
    	%>
        <td WIDTH="85%" height="23" colspan=3 bgcolor="silver">
			<select name="key36" size="1"  <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry">
				<%=s%>
			</select>

    	    <input type="text" name="key37" value="<%=dspkey(37)%>" size="15" readonly   <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
			<input type="button" name="B37" id="B37" value="...." onclick="SrsalesGrouponclick()" <%=fieldpb%> width="100%" style="Z-INDEX: 1" >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" name="C37" id="C37" onclick="SrClear"  <%=fieldpb%> alt="清除" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">

    	    <input type="text" name="key38" value="<%=dspkey(38)%>" size="15" readonly   <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
			<input type="button" name="B38" id="B38" value="...." onclick="Srsalesonclick()" <%=fieldpb%> width="100%" style="Z-INDEX: 1" >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" name="C38" id="C38" onclick="SrClear"  <%=fieldpb%> alt="清除" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
        </td>
	</tr>

	<tr><td WIDTH="15%" class="dataListHEAD" height="23">裝機員工</td>
		<%
			name=""
			if dspkey(79) <> "" then
				sqlxx="select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
					 &"where rtemployee.emply='" & dspkey(79) & "' "
				rs.Open sqlxx,conn
				if rs.eof then
					name="(對象檔找不該員工)"
				else
					name=rs("cusnc")
				end if
				rs.close
			end if
		%>
		<td width="35%" bgcolor="silver">
    	    <input type="text" name="key79" value="<%=dspkey(79)%>" size="8" readonly <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry">
			<input type="button" name="B79" id="B79" value="...." onclick="SrSetEmplyonclick()" width="100%" style="Z-INDEX: 1" >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" name="C79" id="C79" onclick="SrClear" <%=fieldpb%> alt="清除" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
			<font size=2><%=name%></font>
		</td>

		<td WIDTH="15%" class="dataListHEAD" height="23">二線開發人員</td>
		<%
			name=""
			if dspkey(78) <> "" then
				sqlxx=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			 		 &"where rtemployee.emply='" & dspkey(78) & "' "
				rs.Open sqlxx,conn
				if rs.eof then
					name="(對象檔找不該員工)"
				else
					name=rs("cusnc")
				end if
				rs.close
			end if
		%>
		<td width="35%" bgcolor="silver">
    	    <input type="text" name="key78" value="<%=dspkey(78)%>" size="8" readonly <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry">
			<input type="button" name="B78" id="B78" value="...." onclick="Srdeveloperonclick()" width="100%" style="Z-INDEX: 1" >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" name="C78" id="C78" onclick="SrClear" <%=fieldpb%> alt="清除" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
			<font size=2><%=name%></font>
		</td>
	</tr>

	<tr><td WIDTH="15%" class="dataListHEAD" height="23">實收金額</td>
        <td WIDTH="35%" height="23" bgcolor="silver">
			<input type="text" name="key80" value="<%=dspKey(80)%>" size="15" maxlength="10"  <%=fieldRole(1)%> class="dataListEntry">
		</td>

		<td WIDTH="15%" class="dataListHEAD" height="23">保證金</td>
        <td WIDTH="35%" height="23" bgcolor="silver">
			<input type="text" name="key81" value="<%=dspKey(81)%>" size="15" maxlength="10"  <%=fieldRole(1)%> class="dataListEntry">
		</td>
	</tr>

    <tr><td bgcolor="BDB76B" align="center" colspan="4">寬頻服務</td></tr>

	<tr><td  WIDTH="15%"  class="dataListHEAD" height="23">方案類別</td>
		<%
    		s=""
    		sx=" selected "
    		If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  Then
       			sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='L9' "
       			If len(trim(dspkey(39))) < 1 Then
          			sx=" selected " 
	          		s=s & "<option value=""""" & sx & "></option>"
          			sx=""
				else
          			s=s & "<option value=""""" & sx & "></option>"
          			sx=""
       			end if
    		Else
		       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='L9' AND CODE='" & dspkey(39) & "'"
    		End If
		    rs.Open sql,conn
		    Do While Not rs.Eof
				If rs("CODE")=dspkey(39) Then sx=" selected "
				s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
				rs.MoveNext
				sx=""
    		Loop
    		rs.Close
		%>
        <td  WIDTH="35%" height="23" bgcolor="silver" >
   			<select name="key39" <%=fieldpC%> <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry">                                                                  
        		<%=s%>
   			</select>
		</td>

        <td WIDTH="15%" class="dataListHEAD" height="23">公關機</td>
		<%
			dim FREECODE1,FREECODE2
    		If Len(Trim(fieldRole(1) &dataProtect)) < 1 and flg = "Y" Then
       			FREECODE1=""
       			FREECODE2=""
    		Else
		      	' sexd1=" disabled "
      			' sexd2=" disabled "
    		End If
    		If dspKey(40)="Y" Then FREECODE1=" checked "
    		If dspKey(40)="N" Then FREECODE2=" checked "
    	%>
		<td WIDTH="35%" height="23" bgcolor="silver" >
        	<input type="radio" name="key40" value="Y" <%=FREECODE1%> <%=fieldRole(1)%> <%=dataProtect%> ID="Radio1">是
        	<input type="radio" name="key40" value="N" <%=FREECODE2%> <%=fieldRole(1)%> <%=dataProtect%> ID="Radio2">否
        </td>
	</tr>

	<tr><td WIDTH="15%" class="dataListHEAD" height="23">PROMOTION CODE</td>
        <td WIDTH="35%" height="23" bgcolor="silver" >
			<input type="text" name="key41" value="<%=dspKey(41)%>" size="15" maxlength="10"  <%=fieldRole(1)%> class="dataListEntry">
        </td>

    	<td WIDTH="15%"  class="dataListHEAD" height="23">繳款方式</td>
		<%
		    s=""
		    sx=" selected "
		    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 Then  
				sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M1' " 
				If len(trim(dspkey(42))) < 1 Then
					sx=" selected " 
	          		s=s & "<option value=""""" & sx & "></option>"  
    	      		sx=""
				else
					s=s & "<option value=""""" & sx & "></option>"  
					sx=""
				end if
    		Else
				sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M1' AND CODE='" & dspkey(42) & "'"
	   		End If
		    rs.Open sql,conn
		    Do While Not rs.Eof
       			If rs("CODE")=dspkey(42) Then sx=" selected "
				s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
				rs.MoveNext
       			sx=""
    		Loop
    		rs.Close
		%>
        <td WIDTH="35%" height="23" bgcolor="silver" >
			<select size="1" name="key42" <%=fieldpC%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
        		<%=s%>
   			</select>
        </td>
	</tr>

	<tr><td bgcolor="BDB76B" align="center" colspan="4">代理人資訊</td></tr>

	<TR><td width=15% class="dataListHEAD" height="23">代理人姓名</td>
        <td width=35% height="23" bgcolor="silver">
			<input type="text" name="key43" value="<%=dspKey(43)%>" size="15" maxlength="10"  <%=fieldRole(1)%> class="dataListEntry">
        </td>                                 
        <td width=15% class="dataListHEAD" height="23">代理人身份證號</td>
        <td width=35% height="23" bgcolor="silver" >
			<input type="text" name="key44" value="<%=dspKey(44)%>" size="15" maxlength="10"  <%=fieldRole(1)%> class="dataListEntry">
        </td>
 	</tr>

	<TR><td class="dataListHEAD" height="23">代理人電話</td>
        <td height="23" bgcolor="silver" colspan=3>
			<input type="text" name="key45" value="<%=dspKey(45)%>" size="15" maxlength="15"  <%=fieldRole(1)%> class="dataListEntry">
		</td>
 	</tr>

	<tr><td bgcolor="BDB76B" align="center" colspan="4">用戶申請及施工進度狀態</td></tr>
	
	<tr><td class="dataListHEAD" height="23">完工日期</td>
        <td height="23" bgcolor="silver" >
       		<input type="text" name="key48" value="<%=dspKey(48)%>" size="10" READONLY  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
			<input type="button" name="B48" id="B48" onclick="SrBtnOnClick" value="...." <%=fieldpb%> height="100%" width="100%" style="Z-INDEX: 1" >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" name="C48" id="C48" onclick="SrClear" <%=fieldpb%> alt="清除" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >
        </td>

        <td class="dataListHEAD" height="23">報竣日期</td>
        <td height="23" bgcolor="silver">
       		<input type="text" name="key49" value="<%=dspKey(49)%>" size="10" READONLY  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
			<input type="button" name="B49" id="B49" onclick="SrBtnOnClick" value="...." <%=fieldpb%> height="100%" width="100%" style="Z-INDEX: 1" >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" name="C49" id="C49" onclick="SrClear" <%=fieldpb%> alt="清除" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >
        </TD>
	</tr> 

	<tr><td class="dataListHEAD" height="23">報竣轉檔日</td>
		<td height="23" bgcolor="silver" >
       		<input type="text" name="key50" value="<%=dspKey(50)%>" size="10" READONLY  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
			<input type="button" name="B50" id="B50" onclick="SrBtnOnClick" value="...." <%=fieldpb%> height="100%" width="100%" style="Z-INDEX: 1" >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" name="C50" id="C50" onclick="SrClear" <%=fieldpb%> alt="清除" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >
        </td>

		<td class="dataListHEAD" height="23">退租/欠拆日</td>
        <td height="23" bgcolor="silver" >
       		<input type="text" name="key51" value="<%=dspKey(51)%>" size="10" READONLY  <%=fieldRole(1)%> <%=dataProtect%> class="dataListEntry">
			<input type="button" name="B51" id="B51" onclick="SrBtnOnClick" value="...." <%=fieldpb%> height="100%" width="100%" style="Z-INDEX: 1" >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" name="C51" id="C51" onclick="SrClear" <%=fieldpb%> alt="清除" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >

        	<font size=2>欠拆︰</font> 
<!--			<input type="text" name="key54" value="<%=dspKey(54)%>" size="2" maxlength="1"  <%=fieldRole(1)%> class="dataListEntry"> -->
			<%
				if dspkey(54) ="Y" then
					select1=""
					select2="selected"
				else
					select1="selected"
					select2=""
				end if
			%>
			<select name="key54" class="dataListEntry" <%=FIELDROLE(1)%> >
				<option value="" <%=select1%> ></option>
				<option value="Y" <%=select2%> >Y</option>
			</select>
			
        </td>
	</tr>

	<tr><td width=15% class="dataListHEAD" height="23">作廢日期</td>
        <td width=35% height="23" bgcolor="silver">
			<input type="text" name="key52" value="<%=dspKey(52)%>" size="10" READONLY  <%=fieldRole(1)%> class="dataListdata">
		</td>

        <td width=15% class="dataListHEAD" height="23">作廢人員</td>
        <%
			name="" 
           if dspkey(53) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(53) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
		%>
		<td width=35% height="23" bgcolor="silver">
			<input type="text" name="key53" value="<%=dspKey(53)%>" size="10" readonly  <%=fieldRole(1)%> class="dataListdata">
			<font size=2><%=name%></font>
        </td>
	</tr>

	<tr><td bgcolor="BDB76B" align="Center" colspan="4">信用卡資料</td></tr>

	<tr><td width="15%" class="dataListHEAD" height="23">信用卡類型</td>
		<%
		    s=""
		    sx=" selected "
		    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
				sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='M6' " 
				If len(trim(dspkey(72))) < 1 Then
          			sx=" selected " 
       			else
          			sx=""
       			end if
    		Else
       			sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='M6' AND CODE='" & dspkey(72) &"' " 
    		End If
		    rs.Open sql,conn
    		s=""
		    s=s &"<option value=""" &"""" &sx &">(信用卡類型)</option>"
		    sx=""
			Do While Not rs.Eof
				If rs("CODE")=dspkey(72) Then sx=" selected "
				s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
				rs.MoveNext
				sx=""
			Loop
		    rs.Close
		%>
		<td width="35%" bgcolor="silver">
			<select name="key72"  <%=FIELDROLE(1)%> <%=dataProtect%> class="dataListEntry">
				<%=s%>
			</select>
		</td>

		<td width="15%" class="dataListHEAD" height="23">發卡銀行</td>
        <td width="35%" height="23" bgcolor="silver">
			<input type="text" name="key73" value="<%=dspKey(73)%>" size="25" maxlength="20"  <%=fieldRole(1)%> class="dataListEntry">
		</td>
	</tr>

	<tr><td class="dataListHEAD" height="23">信用卡卡號</td>
        <td height="23" bgcolor="silver">
			<input type="text" name="key74" value="<%=dspKey(74)%>" size="20" maxlength="16"  <%=fieldRole(1)%> class="dataListEntry">
		</td>

		<td class="dataListHEAD" height="23">持卡人姓名</td>
        <td height="23" bgcolor="silver">
			<input type="text" name="key75" value="<%=dspKey(75)%>" size="20" maxlength="20"  <%=fieldRole(1)%> class="dataListEntry">
		</td>
	</tr>

	<tr><td class="dataListHEAD" height="23">信用卡有效期限</td>
        <td height="23" bgcolor="silver" colspan=3>
			<input type="text" name="key76" value="<%=dspKey(76)%>" size="5" maxlength="2"  <%=fieldRole(1)%> class="dataListEntry">
			月/
			<input type="text" name="key77" value="<%=dspKey(77)%>" size="5" maxlength="2"  <%=fieldRole(1)%> class="dataListEntry">
			年
		</td>
	</tr>

	<tr><td bgcolor="BDB76B" align="center" colspan="4">異動狀態</td></tr>

	<tr><td width=15% class="dataListHEAD" height="23">移出社區主線序號</td>
        <%
			comn=""
			if dspkey(56) <> 0 then
				sqlxx=" select * from RTSparq499Cmtyh where comq1=" & dspkey(56) 
				rs.Open sqlxx,conn
				if rs.eof then
					comn=""
				else
					comn=rs("comn")
              	end if
              	rs.close
           	end if
        %>
        <td width=35% height="23" bgcolor="silver">
			<input type="text" name="key56" value="<%=dspKey(56)%>" size="5" readonly  <%=fieldRole(1)%> class="dataListdata">
			<input type="text" name="key57" value="<%=dspKey(57)%>" size="5" readonly  <%=fieldRole(1)%> class="dataListdata">
	        <font size=2><%=comn%></font>
		</td>

        <td width=15% class="dataListHEAD" height="23">移入社區主線序號</td>
        <td width=35% height="23" bgcolor="silver">
        <%
			comn=""
			if dspkey(58) <> 0 then
				sqlxx=" select * from RTSparq499Cmtyh where comq1=" & dspkey(58) 
				rs.Open sqlxx,conn
				if rs.eof then
					comn=""
				else
					comn=rs("comn")
				end if
				rs.close
			end if
        %>
			<input type="text" name="key58" value="<%=dspKey(58)%>" size="5" readonly  <%=fieldRole(1)%> class="dataListdata">
			<input type="text" name="key59" value="<%=dspKey(59)%>" size="5" readonly  <%=fieldRole(1)%> class="dataListdata">
	        <font size=2><%=comn%></font>
		</td>
	</tr>

	<tr>
        <td width=15% class="dataListHEAD" height="23">移出異動結案日</td>
        <td width=35% height="23" bgcolor="silver">
			<input type="text" name="key60" value="<%=dspKey(60)%>" size="10" readonly  <%=fieldRole(1)%> class="dataListdata">
         </td>
        <td width=15% class="dataListHEAD" height="23">移入異動結案日</td>
        <td width=35% height="23" bgcolor="silver">
			<input type="text" name="key61" value="<%=dspKey(61)%>" size="10" readonly  <%=fieldRole(1)%> class="dataListdata">
        </td>
	</tr>

	<tr><td bgcolor="BDB76B" align="center" colspan="4">備註說明</td></tr>

    <TR><TD align="CENTER" colspan="4" bgcolor="silver">
			<TEXTAREA name="key55" value="<%=dspkey(55)%>" MAXLENGTH=500 cols="100%" rows=8 class="dataListentry" <%=dataprotect%> >
				<%=dspkey(55)%>
			</TEXTAREA>
   		</td>
	</tr>
</table> 


<!-- 異動前資料 ========================================================================================================== -->
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag2" style="display: none">
	<tr><td bgcolor="BDB76B" align="center" colspan="4">基本資料</td></tr>

	<tr><td width="15%" class=dataListHead>社區序號</td>
		<%
			name=""
			if dspkey(83) <> "" then
				sqlxx="select COMN from RTSparq499CmtyH where COMQ1=" & dspkey(83) 
				rs.Open sqlxx,conn
				if rs.eof then
					name="(對象檔找不該社區)"
				else
					name=rs("COMN")
				end if
				rs.close
			end if
		%>
		<td width="35%" bgcolor="silver">
    	    <input type="text" name="key83" value="<%=dspkey(83)%>" size="5" <%=fieldlock%> >
			<font size=2><%=name%></font>
		</td>
		<td width="15%" class=dataListHead>主線序號</td>
		<td width="10%"  bgcolor="silver">
			<input type="text" name="key84" size="5" value="<%=dspKey(84)%>" <%=fieldlock%> ></td>
	</tr>

	<TR><td class="dataListHEAD" height="23">用戶IP</td>
        <td height="23" bgcolor="silver"  ><font color=red>
        	<input type="text" name="key140" size="3" value="<%=dspKey(140)%>" <%=fieldlock%> >
        		<font size=2>.</font>
        	<input type="text" name="key141" size="3" value="<%=dspKey(141)%>" <%=fieldlock%> >
        		<font size=2>.</font>
        	<input type="text" name="key142" size="3" value="<%=dspKey(142)%>" <%=fieldlock%> >
        		<font size=2>.</font>
        	<input type="text" name="key143" size="3" value="<%=dspKey(143)%>" <%=fieldlock%> >
        </font></td>

		<td class="dataListHEAD" height="23">對帳序號</td>
        <td height="23" bgcolor="silver">
			<input type="text" name="key139" size="15" value="<%=dspKey(139)%>" <%=fieldlock%> >
        	<FONT SIZE=2>-</FONT>
        	<input type="text" name="key144" size="3" value="<%=dspKey(144)%>" <%=fieldlock%> >
		</TD>
	</TR>

	<tr><td class="dataListHEAD" height="23">iDslam Port位</td>
        <td height="23" bgcolor="silver" colspan=3 >
			<input type="text" name="key112" value="<%=dspKey(112)%>" size="2" <%=fieldlock%> ID="Text3">
		</td>
	</tr>

	<tr><td width="15%" class=dataListHEAD>收件日</td>
    	<td width="35%" bgcolor="silver" >
        	<input type="text" name="key123" value="<%=dspKey(123)%>" size="10" <%=fieldlock%> >
		</td>

		<td width="15%" class=dataListHEAD>用戶申請日</td>
   		<td width="35%" bgcolor="silver">
       		<input type="text" name="key124" value="<%=dspKey(124)%>" size="10" <%=fieldlock%> >
		</td>
	</tr>

	<tr><td width="15%" class=dataListHEAD>用戶(公司)名稱</td>
    	<td width="35%"  bgcolor="silver" >
       		<input type="text" name="key85" value="<%=dspKey(85)%>" size="30" <%=fieldlock%> >
		</td>

		<td width="15%" class=dataListHEAD>身分證(統編)</td>
		<%
			sql="SELECT CodeNC FROM RTCode WHERE KIND='J5' AND CODE='" & dspkey(86) &"' " 
    		rs.Open sql,conn
			if rs.eof then
   				sx=""
   			else
   				sx=rs("CodeNC")
   			end if
    		rs.Close
		%>
    	<td width="35%" bgcolor="silver">
       		<input type="text" name="col86" value="<%=sx%>" size="12" <%=fieldlock%> >
       		<input type="text" name="key86" value="<%=dspKey(86)%>" size="12" style="display:none;">
       		<input type="text" name="key87" value="<%=dspKey(87)%>" size="12" <%=fieldlock%> >
		</td>
	</tr>

	<tr><td width="15%" class=dataListHEAD>申請書編號</td>
    	<td  width="35%"  bgcolor="silver" >
   			<input type="text" name="key159" value="<%=dspKey(159)%>" size="15" <%=fieldlock%> >
		</td>

        <td width="10%" class="dataListHead" height="25">第二證照別及號碼</td>
		<%
			sql="SELECT CodeNC FROM RTCode WHERE KIND='L3' AND CODE='" & dspkey(88) &"' "
    		rs.Open sql,conn
			if rs.eof then
   				sx=""
   			else
   				sx=rs("CodeNC")
   			end if
    		rs.Close
		%>
    	<td width="35%" bgcolor="silver">
       		<input type="text" name="col88" value="<%=sx%>" size="12" <%=fieldlock%> >
       		<input type="text" name="key88" value="<%=dspKey(88)%>" size="15" style="display:none;">
       		<input type="text" name="key89" value="<%=dspKey(89)%>" size="15" <%=fieldlock%> >
		</td>
	</tr>

	<tr><td class="dataListHEAD" height="23">出生日期</td>
   		<td width="35%" bgcolor="silver">
       		<input type="text" name="key90" value="<%=dspKey(90)%>" size="10" <%=fieldlock%> >
        </td>

        <td class="dataListHEAD" height="23">連絡EMAIL</td>
        <td height="23" bgcolor="silver" >
   			<input type="text" name="key91" value="<%=dspKey(91)%>" size="50" <%=fieldlock%> >
        </td>
 	</tr>

	<TR><td class="dataListHEAD" height="23">連絡電話</td>
        <td  height="23" bgcolor="silver" >
   			<input type="text" name="key92" value="<%=dspKey(92)%>" size="20" <%=fieldlock%> >
        </td>
        
        <td  class="dataListHEAD" height="23">行動電話</td>
        <td  height="23" bgcolor="silver">
   			<input type="text" name="key93" value="<%=dspKey(93)%>" size="30" <%=fieldlock%> >
		</td>
 	</tr>

	<tr><td class=dataListHEAD>戶籍/公司地址</td>
		<%
   	   		sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(94) & "' " 
    		rs.Open sql,conn
			if rs.eof then
   				sx=""
   			else
    			sx= rs("Cutnc")
   			end if
    		rs.Close
		%>
		<td bgcolor="silver" colspan="3">
       		<input type="text" name="col94" value="<%=sx%>" size="12" <%=fieldlock%> >
       		<input type="text" name="key94" value="<%=dspKey(94)%>" size="8" style="display:none;">
    	    <input type="text" name="key95" value="<%=dspkey(95)%>" size="8" <%=fieldlock%> >
			<input type="text" name="key96" value="<%=dspKey(96)%>" size="40" <%=fieldlock%> >
			<input type="text" name="key97" value="<%=dspKey(97)%>" size="5" <%=fieldlock%> >
		</td></tr>

	<tr><td class=dataListHEAD>裝機地址</td>
		<%
   	   		sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(98) & "' " 
    		rs.Open sql,conn
			if rs.eof then
   				sx=""
   			else
    			sx= rs("Cutnc")
   			end if
    		rs.Close
		%>
		<td bgcolor="silver" colspan="3">
       		<input type="text" name="col98" value="<%=sx%>" size="12" <%=fieldlock%> >
       		<input type="text" name="key98" value="<%=dspKey(98)%>" size="8" style="display:none;">
    	    <input type="text" name="key99" value="<%=dspkey(99)%>" size="8" <%=fieldlock%> >
			<input type="text" name="key100" value="<%=dspKey(100)%>" size="40" <%=fieldlock%> >
			<input type="text" name="key101" value="<%=dspKey(101)%>" size="5" <%=fieldlock%> >
		</td>
	</tr>

	<tr><td class=dataListHEAD>帳單地址</td>
		<%
   	   		sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(102) & "' " 
    		rs.Open sql,conn
			if rs.eof then
   				sx=""
   			else
    			sx= rs("Cutnc")
   			end if
    		rs.Close
		%>
		<td bgcolor="silver" colspan="3">
       		<input type="text" name="col102" value="<%=sx%>" size="12" <%=fieldlock%> >
       		<input type="text" name="key102" value="<%=dspKey(102)%>" size="8" style="display:none;">
    	    <input type="text" name="key103" readonly size="8" value="<%=dspkey(103)%>" <%=fieldlock%> >
			<input type="text" name="key104" value="<%=dspKey(104)%>" size="40" <%=fieldlock%> >
			<input type="text" name="key105" value="<%=dspKey(105)%>" size="5" <%=fieldlock%> >
		</td>
	</tr>

	<tr><td class="dataListHEAD" height="23">企業連絡人</td>
        <td height="23" bgcolor="silver" >
			<input type="text" name="key106" value="<%=dspKey(106)%>" size="15" <%=fieldlock%> >
		</td>

        <td class="dataListHEAD" height="23">連絡電話</td>
        <td height="23" bgcolor="silver">
			<input type="text" name="key107" value="<%=dspKey(107)%>" size="15" <%=fieldlock%> >
	        <font size=2>分機︰</font>
			<input type="text" name="key108" value="<%=dspKey(108)%>" size="5" <%=fieldlock%> >
	        <font size=2>手機︰</font>
	        <input type="text" name="key109" value="<%=dspKey(109)%>" size="10" <%=fieldlock%> ID="Text2">			
	    </td>
	</tr>

	<tr><td class="dataListHEAD" height="23">企業負責人</td>
        <td height="23" bgcolor="silver" >
			<input type="text" name="key110" value="<%=dspKey(110)%>" size="10" <%=fieldlock%> >
		</td>

		<td class="dataListHEAD" height="23">負責人身份證號</td>
        <td height="23" bgcolor="silver" >
			<input type="text" name="key111" value="<%=dspKey(111)%>" size="10" <%=fieldlock%> >
		</td>
	</tr>

	<tr><td bgcolor="BDB76B" align="center" colspan="4">績效歸屬</td></tr>

	<tr><td WIDTH="15%" class="dataListHEAD" height="23">業務轄區</td>
		<%
   			sql="SELECT AREANC FROM RTArea WHERE (AREATYPE = '1') AND AREAID='" & DSPKEY(113) & "' "
    		rs.Open sql,conn
			if rs.eof then
				sx=""
			else
				sx = rs("AREANC")
			end if
    		rs.Close
    	%>
        <td WIDTH="85%" height="23" colspan=3 bgcolor="silver">
       		<input type="text" name="col113" value="<%=sx%>" size="15" <%=fieldlock%> >
       		<input type="text" name="key113" value="<%=dspKey(113)%>" size="8" style="display:none;">
    	    <input type="text" name="key114" value="<%=dspkey(114)%>" size="15" <%=fieldlock%> >
    	    <input type="text" name="key115" value="<%=dspkey(115)%>" size="15" <%=fieldlock%> >
        </td>
	</tr>

	<tr><td WIDTH="15%" class="dataListHEAD" height="23">裝機員工</td>
		<%
			name=""
			if dspkey(156) <> "" then
				sqlxx="select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
					 &"where rtemployee.emply='" & dspkey(156) & "' "
				rs.Open sqlxx,conn
				if rs.eof then
					name="(對象檔找不該員工)"
				else
					name=rs("cusnc")
				end if
				rs.close
			end if
		%>
		<td width="35%" bgcolor="silver">
    	    <input type="text" name="key156" value="<%=dspkey(156)%>" size="8" <%=fieldlock%> >
			<font size=2><%=name%></font>
		</td>

		<td WIDTH="15%" class="dataListHEAD" height="23">二線開發人員</td>
		<%
			name=""
			if dspkey(155) <> "" then
				sqlxx=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			 		 &"where rtemployee.emply='" & dspkey(155) & "' "
				rs.Open sqlxx,conn
				if rs.eof then
					name="(對象檔找不該員工)"
				else
					name=rs("cusnc")
				end if
				rs.close
			end if
		%>
		<td width="35%" bgcolor="silver">
    	    <input type="text" name="key155" value="<%=dspkey(155)%>" size="8" <%=fieldlock%> >
			<font size=2><%=name%></font>
		</td>
	</tr>

	<tr><td WIDTH="15%" class="dataListHEAD" height="23">實收金額</td>
        <td WIDTH="35%" height="23" bgcolor="silver">
			<input type="text" name="key157" value="<%=dspKey(157)%>" size="15" <%=fieldlock%> >
		</td>

		<td WIDTH="15%" class="dataListHEAD" height="23">保證金</td>
        <td WIDTH="35%" height="23" bgcolor="silver">
			<input type="text" name="key158" value="<%=dspKey(158)%>" size="15" <%=fieldlock%> >
		</td>
	</tr>

    <tr><td bgcolor="BDB76B" align="center" colspan="4">寬頻服務</td></tr>

	<tr><td  WIDTH="15%"  class="dataListHEAD" height="23">方案類別</td>
		<%
	       sql="SELECT CODENC FROM RTCODE WHERE KIND='L9' AND CODE='" & dspkey(116) & "'"
		    rs.Open sql,conn
			if rs.eof then
   				sx=""
   			else
    			sx= rs("CODENC")
   			end if
    		rs.Close
		%>
        <td  WIDTH="35%" height="23" bgcolor="silver" >
       		<input type="text" name="col116" value="<%=sx%>" size="20" <%=fieldlock%> >
       		<input type="text" name="key116" value="<%=dspKey(116)%>" size="20" style="display:none;">
		</td>

        <td WIDTH="15%" class="dataListHEAD" height="23">公關機</td>
		<%
    		If dspKey(117)="Y" Then FREECODE1=" checked "
    		If dspKey(117)="N" Then FREECODE2=" checked "
    	%>
		<td WIDTH="35%" height="23" bgcolor="silver" >
        	<input type="radio" name="key117" value="Y" <%=FREECODE1%> <%=fieldlock%> ID="Radio1">是
        	<input type="radio" name="key117" value="N" <%=FREECODE2%> <%=fieldlock%> ID="Radio2">否
        </td>
	</tr>

	<tr><td WIDTH="15%" class="dataListHEAD" height="23">PROMOTION CODE</td>
        <td WIDTH="35%" height="23" bgcolor="silver" >
			<input type="text" name="key118" value="<%=dspKey(118)%>" size="15" <%=fieldlock%> >
        </td>

    	<td WIDTH="15%"  class="dataListHEAD" height="23">繳款方式</td>
		<%
			sql="SELECT CODENC FROM RTCODE WHERE KIND='M1' AND CODE='" & dspkey(119) & "'"
		    rs.Open sql,conn
			if rs.eof then
   				sx=""
   			else
    			sx= rs("CODENC")
   			end if
    		rs.Close
		%>
        <td WIDTH="35%" height="23" bgcolor="silver" >
       		<input type="text" name="col119" value="<%=sx%>" size="35" <%=fieldlock%> >
       		<input type="text" name="key119" value="<%=dspKey(119)%>" size="35" style="display:none;">
        </td>
	</tr>

	<tr><td bgcolor="BDB76B" align="center" colspan="4">代理人資訊</td></tr>

	<TR><td width=15% class="dataListHEAD" height="23">代理人姓名</td>
        <td width=35% height="23" bgcolor="silver">
			<input type="text" name="key120" value="<%=dspKey(120)%>" size="15" <%=fieldlock%> >
        </td>                                 
        <td width=15% class="dataListHEAD" height="23">代理人身份證號</td>
        <td width=35% height="23" bgcolor="silver" >
			<input type="text" name="key121" value="<%=dspKey(121)%>" size="15" <%=fieldlock%> >
        </td>
 	</tr>

	<TR><td class="dataListHEAD" height="23">代理人電話</td>
        <td height="23" bgcolor="silver" colspan=3>
			<input type="text" name="key122" value="<%=dspKey(122)%>" size="15" <%=fieldlock%> >
		</td>
 	</tr>

	<tr><td bgcolor="BDB76B" align="center" colspan="4">用戶申請及施工進度狀態</td></tr>
	
	<tr><td class="dataListHEAD" height="23">完工日期</td>
        <td height="23" bgcolor="silver" >
       		<input type="text" name="key125" value="<%=dspKey(125)%>" size="10" <%=fieldlock%> >
        </td>

        <td class="dataListHEAD" height="23">報竣日期</td>
        <td height="23" bgcolor="silver">
       		<input type="text" name="key126" value="<%=dspKey(126)%>" size="10" <%=fieldlock%> >
        </TD>
	</tr> 

	<tr><td class="dataListHEAD" height="23">報竣轉檔日</td>
		<td height="23" bgcolor="silver" >
       		<input type="text" name="key127" value="<%=dspKey(127)%>" size="10" <%=fieldlock%> >
        </td>

		<td class="dataListHEAD" height="23">退租日</td>
        <td height="23" bgcolor="silver" >
       		<input type="text" name="key128" value="<%=dspKey(128)%>" size="10" <%=fieldlock%> >
        	<font size=2>欠費︰</font> 
			<input type="text" name="key131" value="<%=dspKey(131)%>" size="2" <%=fieldlock%> >
        </td>
	</tr>

	<tr><td width=15% class="dataListHEAD" height="23">作廢日期</td>
        <td width=35% height="23" bgcolor="silver">
			<input type="text" name="key129" value="<%=dspKey(129)%>" size="10" <%=fieldlock%> >
		</td>

        <td width=15% class="dataListHEAD" height="23">作廢人員</td>
        <%
			name="" 
           if dspkey(130) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(130) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
		%>
		<td width=35% height="23" bgcolor="silver">
			<input type="text" name="key130" value="<%=dspKey(130)%>" size="10" <%=fieldlock%> >
			<font size=2><%=name%></font>
        </td>
	</tr>

	<tr><td bgcolor="BDB76B" align="Center" colspan="4">信用卡資料</td></tr>

	<tr><td width="15%" class="dataListHEAD" height="23">信用卡類型</td>
		<%
   			sql="SELECT CodeNC FROM RTCode WHERE KIND='M6' AND CODE='" & dspkey(149) &"' " 
		    rs.Open sql,conn
			if rs.eof then
   				sx=""
   			else
    			sx= rs("CODENC")
   			end if
		    rs.Close
		%>
		<td width="35%" bgcolor="silver">
       		<input type="text" name="col149" value="<%=sx%>" size="10" <%=fieldlock%> >
       		<input type="text" name="key149" value="<%=dspKey(149)%>" size="10" style="display:none;">
		</td>

		<td width="15%" class="dataListHEAD" height="23">發卡銀行</td>
        <td width="35%" height="23" bgcolor="silver">
			<input type="text" name="key150" value="<%=dspKey(150)%>" size="25" <%=fieldlock%> >
		</td>
	</tr>

	<tr><td class="dataListHEAD" height="23">信用卡卡號</td>
        <td height="23" bgcolor="silver">
			<input type="text" name="key151" value="<%=dspKey(151)%>" size="20" <%=fieldlock%> >
		</td>

		<td class="dataListHEAD" height="23">持卡人姓名</td>
        <td height="23" bgcolor="silver">
			<input type="text" name="key152" value="<%=dspKey(152)%>" size="20" <%=fieldlock%> >
		</td>
	</tr>

	<tr><td class="dataListHEAD" height="23">信用卡有效期限</td>
        <td height="23" bgcolor="silver" colspan=3>
			<input type="text" name="key153" value="<%=dspKey(153)%>" size="5" <%=fieldlock%> >
			月/
			<input type="text" name="key154" value="<%=dspKey(154)%>" size="5" <%=fieldlock%> >
			年
		</td>
	</tr>

	<tr><td bgcolor="BDB76B" align="center" colspan="4">異動狀態</td></tr>

	<tr><td width=15% class="dataListHEAD" height="23">移出社區主線序號</td>
        <%
			comn=""
			if dspkey(133) <> 0 then
				sqlxx=" select * from RTSparq499Cmtyh where comq1=" & dspkey(133) 
				rs.Open sqlxx,conn
				if rs.eof then
					comn=""
				else
					comn=rs("comn")
              	end if
              	rs.close
           	end if
        %>
        <td width=35% height="23" bgcolor="silver">
			<input type="text" name="key133" value="<%=dspKey(133)%>" size="5" <%=fieldlock%> >
			<input type="text" name="key134" value="<%=dspKey(134)%>" size="5" <%=fieldlock%> >
	        <font size=2><%=comn%></font>
		</td>

        <td width=15% class="dataListHEAD" height="23">移入社區主線序號</td>
        <td width=35% height="23" bgcolor="silver">
        <%
			comn=""
			if dspkey(135) <> 0 then
				sqlxx=" select * from RTSparq499Cmtyh where comq1=" & dspkey(135) 
				rs.Open sqlxx,conn
				if rs.eof then
					comn=""
				else
					comn=rs("comn")
				end if
				rs.close
			end if
        %>
			<input type="text" name="key135" value="<%=dspKey(135)%>" size="5" <%=fieldlock%> >
			<input type="text" name="key136" value="<%=dspKey(136)%>" size="5" <%=fieldlock%> >
	        <font size=2><%=comn%></font>
		</td>
	</tr>

	<tr>
        <td width=15% class="dataListHEAD" height="23">移出異動結案日</td>
        <td width=35% height="23" bgcolor="silver">
			<input type="text" name="key137" value="<%=dspKey(137)%>" size="10" <%=fieldlock%> >
         </td>
        <td width=15% class="dataListHEAD" height="23">移入異動結案日</td>
        <td width=35% height="23" bgcolor="silver">
			<input type="text" name="key138" value="<%=dspKey(138)%>" size="10" <%=fieldlock%> >
        </td>
	</tr>

	<tr><td bgcolor="BDB76B" align="center" colspan="4">備註說明</td></tr>

    <TR><TD align="CENTER" colspan="4" bgcolor="silver">
			<TEXTAREA name="key132" value="<%=dspkey(132)%>" cols="100%" rows=8 <%=fieldlock%> >
				<%=dspkey(132)%>
			</TEXTAREA>
   		</td>
	</tr>
</table>

<%
    conn.Close   
    set rs=Nothing   
    set conn=Nothing 
End Sub 
' -------------------------------------------------------------------------------------------- 
Sub SrReadExtDB()

End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrSaveExtDB(Smode)
    logonid=session("userid")
    Call SrGetEmployeeRef(Rtnvalue,1,logonid)
    V=split(rtnvalue,";")  

    Dim conn
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
	sql="UPDATE 	RTSparq499Cust "_
	   &"SET	COMQ1= nCOMQ1, LINEQ1 = nLINEQ1, CUSNC = nCUSNC, FIRSTIDTYPE = nFIRSTIDTYPE, SOCIALID = nSOCIALID, "_
	   &"SECONDIDTYPE = nSECONDIDTYPE, SECONDNO = nSECONDNO, BIRTHDAY = nBIRTHDAY, EMAIL = nEMAIL, "_
	   &"CONTACTTEL = nCONTACTTEL, MOBILE = nMOBILE, CUTID1 = nCUTID1, TOWNSHIP1 = nTOWNSHIP1, RADDR1 = nRADDR1, "_
	   &"RZONE1 = nRZONE1, CUTID2 = nCUTID2, TOWNSHIP2 = nTOWNSHIP2, RADDR2 = nRADDR2, RZONE2 = nRZONE2, "_ 
	   &"CUTID3 = nCUTID3, TOWNSHIP3 = nTOWNSHIP3, RADDR3 = nRADDR3, RZONE3 = nRZONE3, COCONTACT = nCOCONTACT, "_
	   &"COCONTACTTEL = nCOCONTACTTEL, COCONTACTTELEXT = nCOCONTACTTELEXT, COMOBILE = nCOMOBILE, "_
	   &"COBOSS = nCOBOSS, COBOSSSOCIAL = nCOBOSSSOCIAL, TRADETYPE = nTRADETYPE, AREAID = nAREAID, "_
	   &"GROUPID = nGROUPID, SALESID = nSALESID, CASETYPE = nCASETYPE, FREECODE = nFREECODE, PMCODE = nPMCODE, "_
	   &"PAYTYPE = nPAYTYPE, AGENTNAME = nAGENTNAME, AGENTSOCIAL = nAGENTSOCIAL, AGENTTEL = nAGENTTEL, "_
	   &"RCVD = nRCVD, APPLYDAT = nAPPLYDAT, FINISHDAT = nFINISHDAT, DOCKETDAT = nDOCKETDAT, TRANSDAT = nTRANSDAT, "_
	   &"DROPDAT = nDROPDAT, CANCELDAT = nCANCELDAT, CANCELUSR = nCANCELUSR, OVERDUE = nOVERDUE, MEMO = nMEMO, "_
	   &"MOVETOCOMQ1 = nMOVETOCOMQ1, MOVETOLINEQ1 = nMOVETOLINEQ1, MOVEFROMCOMQ1 = nMOVEFROMCOMQ1, "_
	   &"MOVEFROMLINEQ1 = nMOVEFROMLINEQ1, MOVETODAT = nMOVETODAT, MOVEFROMDAT = nMOVEFROMDAT, "_
	   &"NCICCUSNO = nNCICCUSNO, CUSTIP1 = nCUSTIP1, CUSTIP2 = nCUSTIP2, CUSTIP3 = nCUSTIP3, CUSTIP4 = nCUSTIP4, "_
	   &"SPHNNO = nSPHNNO, OLDCUSTIP1 = nOLDCUSTIP1, OLDCUSTIP2 = nOLDCUSTIP2, OLDCUSTIP3 = nOLDCUSTIP3, "_
	   &"OLDCUSTIP4 = nOLDCUSTIP4, CREDITTYPE = nCREDITTYPE, CREDITBANK = nCREDITBANK, CREDITNO = nCREDITNO, "_
	   &"CREDITNAME = nCREDITNAME, VALIDMONTH = nVALIDMONTH, VALIDYEAR = nVALIDYEAR, DEVELOPERID = nDEVELOPERID, "_
	   &"SETEMPLY = nSETEMPLY, REALRCVAMT = nREALRCVAMT, GAMT = nGAMT, APPLYNO = nAPPLYNO, UUSR= '" &V(0)& "',UDAT ='" &datevalue(now())& "' "_
	   &"from	RTSparq499Cust a "_
	   &"inner join RTSparq499CustChgEtc b on a.CUSID =b.CUSID "_
	   &"WHERE	b.modifyno='" & dspkey(0) & "' and b.cusid ='" & dspkey(1) & "' "
'response.write sql
    conn.open DSN
    conn.execute(sql)
    
	if ( dspkey(6) <> dspkey(83) ) or ( dspkey(7) <> dspkey(84) )then
		'非公關戶要改對帳代號 
		if dspkey(40) <>"Y" then
			sql="declare @cmtytel varchar(20), @max399sp smallint, @max499sp smallint, @sphnno varchar(3) " &_
				"select @cmtytel = linetel from RTSparq499CmtyLine where COMQ1 =" & dspkey(6) & " and LINEQ1 =" & dspkey(7) &_
				" select @max399sp = isnull(max(sphnno),0) from rtsparqadslcust where exttel = @cmtytel " &_
				" select @max499sp = isnull(max(sphnno),0) from rtsparq499cust where nciccusno = @cmtytel " &_
				"if @cmtytel ='' " &_
				"	set @sphnno = '001' " &_
				"else if @max399sp >= @max499sp " &_
				"	set @sphnno = right('00'+ convert(varchar(3), @max399sp +1), 3) " &_
				"else " &_
				"	set @sphnno = right('00'+ convert(varchar(3), @max499sp +1), 3) " &_
				"update RTSparq499Cust set NCICCUSNO = @cmtytel, sphnno = @sphnno where CUSID = '" & dspkey(1) & "' "
	    	conn.execute(sql)
    	end if
		sql="UPDATE RTSparq499CustChg SET COMQ1 =" & dspkey(6) & ", LINEQ1 =" & dspkey(7) & " where CUSID = '" & dspkey(1) & "' "
    	conn.execute(sql)
		sql="UPDATE RTSparq499CustLog SET COMQ1 =" & dspkey(6) & ", LINEQ1 =" & dspkey(7) & " where CUSID = '" & dspkey(1) & "' "
    	conn.execute(sql)
		sql="UPDATE RTSparq499Ftp SET COMQ1 =" & dspkey(6) & ", LINEQ1 =" & dspkey(7) & " where CUSID = '" & dspkey(1) & "' "
    	conn.execute(sql)
		sql="UPDATE RTFaqH SET COMQ1 =" & dspkey(6) & ", ENTRYNO =" & dspkey(7) & " where CUSID = '" & dspkey(1) & "' and COMTYPE ='6' "
    	conn.execute(sql)
		sql="UPDATE RTFaqM SET COMQ1 =" & dspkey(6) & ", LineQ1 =" & dspkey(7) & " where CUSID = '" & dspkey(1) & "' and COMTYPE ='6' "
    	conn.execute(sql)
		sql="UPDATE NCICMonthlyAccountSrc SET COMQ1 =" & dspkey(6) & ", LineQ1 =" & dspkey(7) & " where CUSID = '" & dspkey(1) & "' and lineq1 >0 "
    	conn.execute(sql)
	end if 
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
End Sub
' -------------------------------------------------------------------------------------------- 
' --------------------------------------------------------------------------------------------  
%>
<!-- #include virtual="/Webap/include/checkid.inc" -->
<!-- #include virtual="/Webap/include/companyid.inc" -->
<!-- #include file="RTGetUserRight.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->