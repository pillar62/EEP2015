<%  
  Dim fieldRole,fieldPa
  fieldRole=Split(FrGetUserRight("RTCustD",Request.ServerVariables("LOGON_USER")),";")
%>
<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/cType.inc" -->
<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/dataList.inc" -->
<%
  Dim aryKeyName,aryKeyType(150),aryKeyValue(150),numberOfField,aryKey,aryKeyNameDB(150)
  Dim dspKey(150),userDefineKey,userDefineData,extDBField,extDB(150),userDefineRead,userDefineSave
  Dim conn,rs,i,formatName,sqlList,sqlFormatDB,userdefineactivex
  Dim aryParmKey
 '90/09/03 ADD-START
 '增加EXTDBFIELD2,EXTDBFILELD3(多檔維護)
  dim extDBField2,extDB2(150),extDBField3,extDB3(150),extDBField4,extDB4(150)
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
                   case ucase("/AVSCONSIGNEE/rtap/base/rtEBTcmty/RTEBTCUSTd.asp")
                      ' response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       if i <> 2 then rs.Fields(i).Value=dspKey(i)    
                       if i=2 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         cusidxx="AVS-" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
                         rsc.open "select max(cusid) AS cusid from rtEBTcust where cusid like '" & cusidxx & "%' " ,conn
                         if len(rsc("cusid")) > 0 then
                            dspkey(2)=cusidxx & right("00000" & cstr(cint(right(rsc("cusid"),5)) + 1),5)
                         else
                            dspkey(2)=cusidxx & "00001"
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
                 case ucase("/AVSCONSIGNEE/rtap/base/rtEBTcmty/RTEBTcustd.asp")
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
       if ucase(runpgm)=ucase("/AVSCONSIGNEE/rtap/base/rtEBTcmty/RTEBTCUSTD.asp") then
          cusidxx="AVS-" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
          rsc.open "select max(cusid) AS cusid from rtEBTcust where cusid like '" & cusidxx & "%' " ,conn
          if not rsC.eof then
            dspkey(2)=rsC("CUSID")
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
  numberOfKey=3
  title="AVS用戶資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT COMQ1, LINEQ1, CUSID, CUSNC, SOCIALID, CUTID1, TOWNSHIP1, VILLAGE1, " _
             &"COD11, NEIGHBOR1, COD12, STREET1, COD13, SEC1, COD14, LANE1, COD15, " _
             &"ALLEYWAY1, COD16, NUM1, COD17, FLOOR1, COD18, ROOM1, " _
             &"COD19, CUTID2, TOWNSHIP2, VILLAGE2, COD21, NEIGHBOR2, COD22, " _
             &"STREET2, COD23, SEC2, COD24, LANE2, COD25, ALLEYWAY2, " _
             &"COD26, NUM2, COD27, FLOOR2, COD28, ROOM2, COD29, CUTID3, " _
             &"TOWNSHIP3, VILLAGE3, COD31, NEIGHBOR3, COD32, STREET3, COD33, SEC3, " _
             &"COD34, LANE3, COD35, ALLEYWAY3, COD36, NUM3, COD37, FLOOR3, " _
             &"COD38, ROOM3, COD39, BIRTHDAY, CONTACT, MOBILE, EMAIL, CONTACTTEL, " _
             &"PAYTYPE, AVSPMCODE, DIALERPMCODE, DIALERPAYTYPE, AGENTNAME, " _
             &"AGENTTEL, AGENTSOCIAL, CUTID4, TOWNSHIP4, VILLAGE4, NEIGHBOR4, " _
             &"STREET4, SEC4, LANE4, ALLEYWAY4, NUM4, FLOOR4, ROOM4, " _
             &"RCVD, APPLYDAT, APPLYTNSDAT, APPLYAGREE, PROGRESSID, FINISHDAT, " _
             &"DOCKETDAT, TRANSDAT, STRBILLINGDAT,rzone1,rzone2,rzone3,rzone4, " _
             &"areaid,groupid,salesid,cod41,cod42,cod43,cod44,cod45,cod46,cod47,cod48,cod49,custapplydat,OLDSERVICECUTDAT,AVSNO from RTEBTCUST WHERE COMQ1=0 "
  sqlList="SELECT COMQ1, LINEQ1, CUSID, CUSNC, SOCIALID, CUTID1, TOWNSHIP1, VILLAGE1,COD11, NEIGHBOR1, " _
             &"COD12, STREET1, COD13, SEC1, COD14, LANE1, COD15, ALLEYWAY1, COD16,NUM1,   " _
             &"COD17, FLOOR1, COD18, ROOM1,COD19, CUTID2, TOWNSHIP2, VILLAGE2, COD21,NEIGHBOR2,   " _
             &"COD22,STREET2, COD23, SEC2, COD24, LANE2, COD25, ALLEYWAY2,COD26, NUM2,   " _
             &"COD27, FLOOR2, COD28, ROOM2, COD29, CUTID3,TOWNSHIP3, VILLAGE3,COD31, NEIGHBOR3,   " _
             &"COD32, STREET3, COD33, SEC3,COD34, LANE3, COD35, ALLEYWAY3, COD36, NUM3,  " _
             &"COD37, FLOOR3,COD38, ROOM3, COD39, BIRTHDAY, CONTACT,MOBILE, EMAIL, CONTACTTEL,  " _
             &"PAYTYPE, AVSPMCODE, DIALERPMCODE, DIALERPAYTYPE, AGENTNAME,AGENTTEL, AGENTSOCIAL,CUTID4, TOWNSHIP4, VILLAGE4,    " _
             &"NEIGHBOR4,STREET4, SEC4, LANE4, ALLEYWAY4, NUM4,FLOOR4, ROOM4,RCVD, APPLYDAT,   " _
             &"APPLYTNSDAT, APPLYAGREE, PROGRESSID, FINISHDAT,DOCKETDAT, TRANSDAT, STRBILLINGDAT,rzone1,rzone2,rzone3, " _ 
             &"rzone4,areaid,groupid,salesid,cod41,cod42,cod43,cod44,cod45,cod46, " _
             &"cod47,cod48,cod49,custapplydat,OLDSERVICECUTDAT,AVSNO " _
             &"from RTEBTCUST WHERE "
  userDefineRead="Yes"      
  userDefineSave="Yes"       
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=4
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
  'dialer資費方案改由附加服務檔記錄，本程式以預設值儲入檔案
  dspkey(73)=""
  EXTDB(2)=1
  EXTDB(3)="03"
  '身份證欄位第一碼,用以判別是個人還是公司,若為公司則出生日期必須空白,反之則不可空白
  LEADINGCHAR=LEFT(DSPKEY(4),1)
  IF LEADINGCHAR >="0" AND LEADINGCHAR <="9" THEN
     COMPANY="Y"
  ELSE
     COMPANT="N"
  END IF
  '檢查AVS用戶合約編號之重覆性
  IF LEN(TRIM(DSPKEY(115))) > 0 THEN
      Set connxx=Server.CreateObject("ADODB.Connection")  
      Set rsxx=Server.CreateObject("ADODB.Recordset")
      DSNXX="DSN=RTLIB"
      connxx.Open DSNxx
      
	  '排除目前這筆資料本身
      sqlXX="SELECT count(*) AS CNT FROM RTEBTCust where AVSNO='" & trim(dspkey(115)) & "' and not (COMQ1=" & dspkey(0) & " and LINEQ1=" & dspkey(1) & " AND CUSID='" & DSPKEY(2) & "')"
      
      rsxx.Open sqlxx,connxx
      s=""
      'Response.Write "CNT=" & RSXX("CNT")
      If RSXX("CNT") > 0 Then
         message="AVS用戶合約號碼已存在其它客戶資料中，不可重複輸入!"
         formvalid=false
      End If
      rsxx.Close
      Set rsxx=Nothing
      connxx.Close
      Set connxx=Nothing    
   end IF  
  '身份證第一碼大寫
  DSPKEY(4)=UCASE(DSPKEY(4))
  If len(trim(dspkey(88)))=0 or Not Isdate(dspkey(88)) then
       formValid=False
       message="收件日不可空白或格式錯誤"    
  elseif len(trim(dspkey(113)))=0 then
       formValid=False
       message="用戶申請日不可空白"   
  elseif len(trim(dspkey(3)))=0 then
       formValid=False
       message="用戶名稱不可空白"          
  elseif len(trim(dspkey(4)))=0 or (len(trim(dspkey(4)))<>10 and len(trim(dspkey(4)))<>8 )then
       formValid=False
       message="用戶身分證(統編)不可空白或長度不對"    
  elseif len(trim(dspkey(5)))=0 then
       formValid=False
       message="裝機地址(縣市)不可空白"   
  elseif len(trim(dspkey(6)))=0 then
       formValid=False
       message="裝機地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(11)))=0 then
       formValid=False
       message="裝機地址(路/街)不可空白"          
  elseif len(trim(dspkey(19)))=0 then
       formValid=False
       message="裝機地址(號)不可空白"          
  elseif len(trim(dspkey(25)))=0 then
       formValid=False
       message="戶籍地址(縣市)不可空白"   
  elseif len(trim(dspkey(26)))=0 then
       formValid=False
       message="戶籍地址(鄉鎮)不可空白"    
 '920224先不檢查==>有些地址確實存在沒有路街的現象
 ' elseif len(trim(dspkey(31)))=0 then
 '      formValid=False
 '      message="戶籍地址(路/街)不可空白"          
  elseif len(trim(dspkey(39)))=0 then
       formValid=False
       message="戶籍地址(號)不可空白"     
  elseif len(trim(dspkey(45)))=0 then
       formValid=False
       message="戶籍地址(縣市)不可空白"   
  elseif len(trim(dspkey(46)))=0 then
       formValid=False
       message="戶籍地址(鄉鎮)不可空白"    
  elseif len(trim(dspkey(51)))=0 then
       formValid=False
       message="戶籍地址(路/街)不可空白"          
  elseif len(trim(dspkey(59)))=0 then
       formValid=False
       message="戶籍地址(號)不可空白"       
  elseif (len(trim(dspkey(65)))=0 or Not Isdate(dspkey(65))) AND COMPANY="N" then
       formValid=False
       message="用戶為個人時，出生日期不可空白或格式錯誤"   
  elseif len(trim(dspkey(65)))<>0  AND COMPANY="Y" then
       formValid=False
       message="用戶為法人時，出生日期必須空白"          
  elseif len(trim(dspkey(66)))=0 then
       formValid=False
       message="連絡人不可空白"              
  elseif len(trim(dspkey(67)))=0 and len(trim(dspkey(69)))=0then
       formValid=False
       message="連絡電話(白天)及行動電話至少須輸入一項"   
  elseif len(trim(dspkey(70)))= 0 then
       formValid=False
       message="AVS繳款方式不可空白"       
  elseif len(trim(EXTDB(0)))= 0 then
       formValid=False
       message="DIALER資費方案不可空白"       
  elseif len(trim(EXTDB(1)))= 0 then
       formValid=False
       message="DIALER電話號碼不可空白"           
  elseif len(trim(EXTDB(2)))= 0 then
       formValid=False
       message="附加服務(DIALER)建檔項次不可為0"     
  elseif LEN(TRIM(DSPKEY(115))) > 0 AND LEN(TRIM(DSPKEY(115))) <> 15  THEN
       formValid=False
       message="用戶合約編號長度必須為15碼"                    
  elseif left(dspkey(115),4) <> "AVS-" AND LEN(TRIM(DSPKEY(115))) > 0 THEN
       formValid=False
       message="用戶合約編號必須為'AVS-'開始共15碼之規則"         
  elseif len(trim(EXTDB(3)))= 0 then
       formValid=False
       message="附加服務不可空白"                                  
'  elseif len(trim(dspkey(73)))= 0 then
'       formValid=False
'       message="Dialer計費方式不可空白"      
  elseif len(trim(dspkey(74)))<> 0 then
         if len(trim(dspkey(76)))= 0 then
            formValid=False
            message="代理人身份證號不可空白" 
         elseif len(trim(dspkey(75)))= 0 then    
            formValid=False
            message="代理人電話不可空白"     
         elseif len(trim(dspkey(5)))=0 then
            formValid=False
            message="代理人地址(縣市)不可空白"   
         elseif len(trim(dspkey(6)))=0 then
            formValid=False
            message="代理人地址(鄉鎮)不可空白"    
         elseif len(trim(dspkey(11)))=0 then
            formValid=False
            message="代理人地址(路/街)不可空白"          
         elseif len(trim(dspkey(19)))=0 then
            formValid=False
            message="代理人地址(號)不可空白"          
         end if
  elseif len(trim(dspkey(94)))<> 0 AND len(trim(dspkey(93)))= 0 then
       formValid=False
       message="完工日期為空白時不可輸入報竣日"       
  end if
  '檢查主線開發為直銷或經銷==當經銷時,則績效歸屬部份為空白,反之則必須輸入
   Set connxx=Server.CreateObject("ADODB.Connection")
   Set rsxx=Server.CreateObject("ADODB.Recordset")
   connxx.open DSN
   sqlxx="select consignee from rtebtcmtyline where comq1=" & aryparmkey(0)
   rsxx.Open sqlxx,connxx
   if not rsxx.eof then
      if len(trim(rsxx("consignee"))) <> 0 then
         if len(trim(dspkey(101))) <> 0 or len(trim(dspkey(102))) <> 0 or len(trim(dspkey(103))) <> 0then
            formValid=False
            message="主線開發為經銷商,績效歸屬必須空白" 
         end if
      else
         if len(trim(dspkey(101))) = 0 or len(trim(dspkey(102))) = 0 then
            formValid=False
            message="主線開發為直銷,績效歸屬不可空白" 
         end if
      end if
   end if
   connxx.Close   
   set rsxx=Nothing   
   set connxx=Nothing 
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveXScript()%>
   <SCRIPT Language="VBScript">
   Sub Srbtnonclick()
    '   Dim ClickID
    '   ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
    '   clickkey="KEY" & clickid
	'   if isdate(document.all(clickkey).value) then
	'      objEF2KDT.varDefaultDateTime=document.all(clickkey).value
    '   end if
    '   call objEF2KDT.show(1)
    '   if objEF2KDT.strDateTime <> "" then
    '      document.all(clickkey).value = objEF2KDT.strDateTime
    '   end if
   END SUB
   Sub SrClear()
     '  Dim ClickID
     '  ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
     '  clickkey="C" & clickid
     '  clearkey="key" & clickid       
     '  if len(trim(document.all(clearkey).value)) <> 0 then
     '     document.all(clearkey).value =  ""
          '當處理人員及處理廠商皆為空白時，才可清除此欄位資料
     '  end if
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
   Sub Srcounty6onclick()
    '   prog="RTGetcountyD.asp"
    '   prog=prog & "?KEY=" & document.all("KEY5").VALUE
    '   FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
    '   if fusr <> "" then
    '   FUsrID=Split(Fusr,";")   
    '   if Fusrid(3) ="Y" then
    '      document.all("key6").value =  trim(Fusrid(0))
    '      document.all("key97").value =  trim(Fusrid(1))
    '   End if       
    '   end if
   End Sub       
   Sub Srcounty26onclick()
    '   prog="RTGetcountyD.asp"
    '   prog=prog & "?KEY=" & document.all("KEY25").VALUE
    '   FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
    '   if fusr <> "" then
    '   FUsrID=Split(Fusr,";")   
    '   if Fusrid(3) ="Y" then
    '      document.all("key26").value =  trim(Fusrid(0))
    '      document.all("key98").value =  trim(Fusrid(1))
    '   End if       
    '   end if
    END SUB
   Sub Srcounty46onclick()
    '   prog="RTGetcountyD.asp"
    '   prog=prog & "?KEY=" & document.all("KEY45").VALUE
    '   FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
    '   if fusr <> "" then
    '   FUsrID=Split(Fusr,";")   
    '   if Fusrid(3) ="Y" then
    '      document.all("key46").value =  trim(Fusrid(0))
    '      document.all("key99").value =  trim(Fusrid(1))
    '   End if       
    '   end if
    END SUB    
   Sub Srcounty78onclick()
     '  prog="RTGetcountyD.asp"
     '  prog=prog & "?KEY=" & document.all("KEY77").VALUE
     '  FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
     '  if fusr <> "" then
     '  FUsrID=Split(Fusr,";")   
     '  if Fusrid(3) ="Y" then
     '     document.all("key78").value =  trim(Fusrid(0))
     '     document.all("key100").value =  trim(Fusrid(1))
     '  End if       
     '  end if
    END SUB        
   Sub Srsalesgrouponclick()
    '   prog="RTGetsalesgroupD.asp"
    '   prog=prog & "?KEY=" & document.all("KEY101").VALUE 
    '   FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
    '   if fusr <> "" then
    '   FUsrID=Split(Fusr,";")   
    '   if Fusrid(2) ="Y" then
    '      document.all("key102").value =  trim(Fusrid(0))
    '   End if       
    '   end if
   End Sub        
   Sub Srsalesonclick()
    '   prog="RTGetsalesD.asp"
    '   prog=prog & "?KEY=" & document.all("KEY101").VALUE & ";" & document.all("KEY102").VALUE
    '   FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
    '   if fusr <> "" then
    '   FUsrID=Split(Fusr,";")   
    '   if Fusrid(2) ="Y" then
    '      document.all("key103").value =  trim(Fusrid(0))
    '   End if       
    '   end if
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
   Sub SrTAG4()
      ' msgbox window.SRTAB1.style.display
       if window.SRTAB4.style.display="" then
          window.SRTAB4.style.display="none"
       elseif window.SRTAB4.style.display="none" then
          window.SRTAB4.style.display=""
       end if
   End Sub       
   Sub SrTAG5()
      ' msgbox window.SRTAB1.style.display
       if window.SRTAB5.style.display="" then
          window.SRTAB5.style.display="none"
       elseif window.SRTAB5.style.display="none" then
          window.SRTAB5.style.display=""
       end if
   End Sub                   
Sub SrAddrEqual1()
 ' Dim i,j
 ' i=25
 ' j=5
 ' do while i < 45
 '    keyx="key" & i
 '    keyy="key" & j
 '    document.All(keyx).value=document.All(keyy).value
 '    i=i+1
 '    j=j+1
 ' loop
 '  document.All("key98").value=document.All("key97").value
End Sub 
Sub SrAddrEqual2()
 ' Dim i,j
 ' i=45
 ' j=5
 ' do while i < 65
 '    keyx="key" & i
 '    keyy="key" & j
 '    document.All(keyx).value=document.All(keyy).value
 '    i=i+1
 '    j=j+1
 ' loop
 '  document.All("key99").value=document.All("key97").value
End Sub         
Sub SrAddrEqual3()
 ' Dim i,j
 ' i=45
 ' j=25
 ' do while i < 65
 '    keyx="key" & i
 '    keyy="key" & j
 '    document.All(keyx).value=document.All(keyy).value
 '    i=i+1
 '    j=j+1
 ' loop
 '  document.All("key99").value=document.All("key98").value
End Sub         
Sub SrAddrEqual4()
 '  document.All("key66").value=document.All("key3").value
End Sub       
   </Script>
<%   
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveX() %>
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"   codebase="http://w3c.intra.cbbn.com.tw/AVSCONSIGNEE/activex/EF2KDT.CAB#version=9,0,0,3" 
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
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key0"
                 <%=fieldRole(1)%> readonly size="10" value="<%=dspKey(0)%>" maxlength="8" class=dataListdata></td>
           <td width="15%" class=dataListHead>主線序號</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key1"
                 <%=fieldRole(1)%> readonly size="10" value="<%=dspKey(1)%>" maxlength="8" class=dataListdata></td>                 
           <td width="25%" class=dataListHead>用戶序號</td>
           <td width="25%"  bgcolor="silver">
           <input type="text" name="key2"
                 <%=fieldRole(1)%> readonly size="15" value="<%=dspKey(2)%>" maxlength="15" class=dataListdata></td>
  </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
    '用戶申請轉檔後,資料 protect
    If len(trim(dspKey(90))) > 0 Then
       fieldPa=" class=""dataListData"" readonly "
       fieldpb=" disabled "
    Else
       fieldPa=""
       fieldpb=""
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
  <span id="tags1" class="dataListTagsOn">AVS用戶資訊</span>
                                                            
  <div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td>
<td>     
    <DIV ID="SRTAG0" onclick="srtag0" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="LEFT">基本資料</td></tr></table></div>
 <DIV ID=SRTAB0 >   
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
<tr><td width="15%" class=dataListHEAD>收件日</td>
    <td width="35%" bgcolor="silver" >
        <input type="text" name="key88" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(88)%>"  READONLY size="10" class=dataListEntry>
       <input  type="button" id="B88"  <%=fieldpb%> name="B88" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG  SRC="/AVSCONSIGNEE/IMAGES/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C88"  name="C88"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
      </td>
<td width="15%" class=dataListHEAD>用戶申請日</td>
    <td width="35%" bgcolor="silver" >
        <input type="text" name="key113" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(113)%>"  READONLY size="10" class=dataListEntry>
       <input  type="button" id="B113"  <%=fieldpb%> name="B113" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG  SRC="/AVSCONSIGNEE/IMAGES/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C113"  name="C113"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
      </td>      
</tr>
<tr><td width="15%" class=dataListHEAD>用戶(公司)名稱</td>
    <td  width="35%"  bgcolor="silver" >
        <input type="text" name="key3" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="30"
               value="<%=dspKey(3)%>"  size="30" class=dataListENTRY ID="Text22"></td>
<td width="15%" class=dataListHEAD>身分證(統編)</td>
    <td width="35%" bgcolor="silver" >
        <input type="text" name="key4" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(4)%>"   size="12" class=dataListENTRY ID="Text23"></td>               
</tr>
<tr><td class=dataListHEAD>ADSL裝機地址</td>
    <td bgcolor="silver" COLSPAN=3>
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND len(trim(dspKey(90))) = 0 Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(5))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX6=" onclick=""Srcounty6onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(5) & "' " 
       SXX6=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(5) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
         <select size="1" name="key5" <%=fieldpa%><%=fieldpb%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select2"><%=s%></select>
        <input type="text" name="key6" readonly  size="8" value="<%=dspkey(6)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text4"><font size=2>(鄉鎮)                 
         <input type="button" id="B6" <%=fieldpb%> name="B6"   width="100%" style="Z-INDEX: 1"  value="..." <%=SXX6%>  >        
          <IMG SRC="/AVSCONSIGNEE/IMAGES/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C6"  name="C6"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
        
        <input type="text" name="key7" <%=fieldpa%> size="10" value="<%=dspkey(7)%>" maxlength="10" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text5"><font size=2>
        <% aryOption=Array("村","里")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0  Then
      For i = 0 To Ubound(aryOption)
          If dspKey(8)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(8) &""">" &dspKey(8) &"</option>"
   End If%>                                  
       <select size="1" name="key8" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select3">                                            
        <%=s%></select>      
        <input type="text" name="key9"  size="6" value="<%=dspkey(9)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text6"><font size=2>
        <% aryOption=Array("鄰")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(10)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(10) &""">" &dspKey(10) &"</option>"
   End If%>                                  
       <select size="1" name="key10" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select4">                                            
        <%=s%></select>              
        <input type="text" name="key11" size="10" value="<%=dspkey(11)%>" maxlength="10" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text27"><font size=2>
        <% aryOption=Array("路","街")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(12)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(12) &""">" &dspKey(12) &"</option>"
   End If%>                                  
       <select size="1" name="key12" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select5">                                            
        <%=s%></select>                      
        <input type="text" name="key13"  size="6" value="<%=dspkey(13)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text29"><font size=2>
        <% aryOption=Array("段")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(14)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(14) &""">" &dspKey(14) &"</option>"
   End If%>                                  
       <select size="1" name="key14" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select6">                                            
        <%=s%></select>
        <input type="text" name="key15" size="6" value="<%=dspkey(15)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text30"><font size=2>
        <% aryOption=Array("巷")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(16)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(16) &""">" &dspKey(16) &"</option>"
   End If%>                                  
       <select size="1" name="key16" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select9">                                            
        <%=s%></select>        
        <br>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
         <input type="text" name="key97"  readonly size="5" value="<%=dspkey(97)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text12">
        <input type="text" name="key17" size="10" value="<%=dspkey(17)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text31"><font size=2>
                <% aryOption=Array("弄")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(18)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(18) &""">" &dspKey(18) &"</option>"
   End If%>                                  
       <select size="1" name="key18" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select10">                                            
        <%=s%></select>    
        <input type="text" name="key19" size="6" value="<%=dspkey(19)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text32"><font size=2>
                <% aryOption=Array("號")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(20)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(20) &""">" &dspKey(20) &"</option>"
   End If%>                                  
       <select size="1" name="key20" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select11">                                            
        <%=s%></select>            
        <input type="text" name="key21" size="10" value="<%=dspkey(21)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text33"><font size=2>
                <% aryOption=Array("樓")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0  Then
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
       <select size="1" name="key22" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select12">                                            
        <%=s%></select>
        <input type="text" name="key23" size="6" value="<%=dspkey(23)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text34"><font size=2>
                <% aryOption=Array("室")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(24)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(24) &""">" &dspKey(24) &"</option>"
   End If%>                                  
       <select size="1" name="key24" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select13">                                            
        <%=s%></select>       
        </td>                                 
</tr>  
<tr><td class=dataListHEAD>戶籍地址
    <br><input type="radio" name="rd1"  <%=fieldpb%> onClick="SrAddrEqual1()">同裝機</td>
    <td bgcolor="silver" COLSPAN=3>
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND len(trim(dspKey(90))) = 0 Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(25))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX26=" onclick=""Srcounty26onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(25) & "' " 
       SXX26=""
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
         <select size="1" name="key25" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select2"><%=s%></select>
        <input type="text" name="key26" readonly  size="8" value="<%=dspkey(26)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text4"><font size=2>(鄉鎮)                 
         <input type="button" id="B26"  <%=fieldpb%>  name="B26"   width="100%" style="Z-INDEX: 1"  value="..." <%=SXX26%>  >        
          <IMG SRC="/AVSCONSIGNEE/IMAGES/IMGDELETE.GIF" <%=fieldpb%>  alt="清除" id="C26"  name="C26"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
        
        <input type="text" name="key27" <%=fieldpa%> size="10" value="<%=dspkey(27)%>" maxlength="10" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text5"><font size=2>
        <% aryOption=Array("村","里")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(28)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(28) &""">" &dspKey(28) &"</option>"
   End If%>                                  
       <select size="1" name="key28" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select3">                                            
        <%=s%></select>      
        <input type="text" name="key29"  size="6" value="<%=dspkey(29)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text6"><font size=2>
        <% aryOption=Array("鄰")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(30)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(30) &""">" &dspKey(30) &"</option>"
   End If%>                                  
       <select size="1" name="key30" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select4">                                            
        <%=s%></select>              
        <input type="text" name="key31" size="10" value="<%=dspkey(31)%>" maxlength="10" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text27"><font size=2>
        <% aryOption=Array("路","街")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(32)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(32) &""">" &dspKey(32) &"</option>"
   End If%>                                  
       <select size="1" name="key32" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select5">                                            
        <%=s%></select>                      
        <input type="text" name="key33"  size="6" value="<%=dspkey(33)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text29"><font size=2>
        <% aryOption=Array("段")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(34)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(34) &""">" &dspKey(34) &"</option>"
   End If%>                                  
       <select size="1" name="key34" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select6">                                            
        <%=s%></select>
        <input type="text" name="key35" size="6" value="<%=dspkey(35)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text30"><font size=2>
        <% aryOption=Array("巷")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(36)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(36) &""">" &dspKey(36) &"</option>"
   End If%>                                  
       <select size="1" name="key36" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select9">                                            
        <%=s%></select>        
        <br>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
         <input type="text" name="key98"  readonly size="5" value="<%=dspkey(98)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text13">
        <input type="text" name="key37" size="10" value="<%=dspkey(37)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text31"><font size=2>
                <% aryOption=Array("弄")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(38)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(38) &""">" &dspKey(38) &"</option>"
   End If%>                                  
       <select size="1" name="key38" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select10">                                            
        <%=s%></select>    
        <input type="text" name="key39" size="6" value="<%=dspkey(39)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text32"><font size=2>
                <% aryOption=Array("號")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(40)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(40) &""">" &dspKey(40) &"</option>"
   End If%>                                  
       <select size="1" name="key40" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select11">                                            
        <%=s%></select>            
        <input type="text" name="key41" size="10" value="<%=dspkey(41)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text33"><font size=2>
                <% aryOption=Array("樓")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(42)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(42) &""">" &dspKey(42) &"</option>"
   End If%>                                  
       <select size="1" name="key42" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select12">                                            
        <%=s%></select>
        <input type="text" name="key43" size="6" value="<%=dspkey(43)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text34"><font size=2>
                <% aryOption=Array("室")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(44)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(44) &""">" &dspKey(44) &"</option>"
   End If%>                                  
       <select size="1" name="key44" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select13">                                            
        <%=s%></select>       
        </td>                                 
</tr>  
<tr><td class=dataListHEAD>帳單地址
    <br><input type="radio" name="rd2" <%=fieldpb%>  onClick="SrAddrEqual2()"><font size=2>同裝機</font><input type="radio"  <%=fieldpb%> name="rd2" onClick="SrAddrEqual3()"><font size=2>同戶籍</font>
    </td>
    <td bgcolor="silver" COLSPAN=3>
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND len(trim(dspKey(90))) = 0 Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(45))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX46=" onclick=""Srcounty46onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(45) & "' " 
       SXX46=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(45) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
         <select size="1" name="key45" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select2"><%=s%></select>
        <input type="text" name="key46" readonly  size="8" value="<%=dspkey(46)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text4"><font size=2>(鄉鎮)                 
         <input type="button" id="B46"   <%=fieldpb%> name="B46"   width="100%" style="Z-INDEX: 1"  value="..." <%=SXX46%>  >        
          <IMG SRC="/AVSCONSIGNEE/IMAGES/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C46"  name="C46"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
        
        <input type="text" name="key47" <%=fieldpa%> size="10" value="<%=dspkey(47)%>" maxlength="10" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text5"><font size=2>
        <% aryOption=Array("村","里")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(48)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(48) &""">" &dspKey(48) &"</option>"
   End If%>                                  
       <select size="1" name="key48" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select3">                                            
        <%=s%></select>      
        <input type="text" name="key49"  size="6" value="<%=dspkey(49)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text6"><font size=2>
        <% aryOption=Array("鄰")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(50)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(50) &""">" &dspKey(50) &"</option>"
   End If%>                                  
       <select size="1" name="key50" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select4">                                            
        <%=s%></select>              
        <input type="text" name="key51" size="10" value="<%=dspkey(51)%>" maxlength="10" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text27"><font size=2>
        <% aryOption=Array("路","街")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(52)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(52) &""">" &dspKey(52) &"</option>"
   End If%>                                  
       <select size="1" name="key52" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select5">                                            
        <%=s%></select>                      
        <input type="text" name="key53"  size="6" value="<%=dspkey(53)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text29"><font size=2>
        <% aryOption=Array("段")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(54)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(54) &""">" &dspKey(54) &"</option>"
   End If%>                                  
       <select size="1" name="key54" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select6">                                            
        <%=s%></select>
        <input type="text" name="key55" size="6" value="<%=dspkey(55)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text30"><font size=2>
        <% aryOption=Array("巷")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(56)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(56) &""">" &dspKey(56) &"</option>"
   End If%>                                  
       <select size="1" name="key56" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select9">                                            
        <%=s%></select>        
        <br>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
         <input type="text" name="key99"  readonly size="5" value="<%=dspkey(99)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text14">
        <input type="text" name="key57" size="10" value="<%=dspkey(57)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text31"><font size=2>
                <% aryOption=Array("弄")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(58)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(58) &""">" &dspKey(58) &"</option>"
   End If%>                                  
       <select size="1" name="key58" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select10">                                            
        <%=s%></select>    
        <input type="text" name="key59" size="6" value="<%=dspkey(59)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text32"><font size=2>
                <% aryOption=Array("號")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(60)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(60) &""">" &dspKey(60) &"</option>"
   End If%>                                  
       <select size="1" name="key60" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select11">                                            
        <%=s%></select>            
        <input type="text" name="key61" size="10" value="<%=dspkey(61)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text33"><font size=2>
                <% aryOption=Array("樓")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(62)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(62) &""">" &dspKey(62) &"</option>"
   End If%>                                  
       <select size="1" name="key62" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select12">                                            
        <%=s%></select>
        <input type="text" name="key63" size="6" value="<%=dspkey(63)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text34"><font size=2>
                <% aryOption=Array("室")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(64)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(64) &""">" &dspKey(64) &"</option>"
   End If%>                                  
       <select size="1" name="key64" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select13">                                            
        <%=s%></select>       
        </td>                                 
</tr>
<tr>                                 
        <td  class="dataListHEAD" height="23">出生日期</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key65" size="10"  value="<%=dspKey(65)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListentry" ID="Text8">  
        <input type="button" id="B65"  <%=fieldpb%>  name="B65" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/AVSCONSIGNEE/IMAGES/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C65"  name="C65"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"> </td>           
        <td  class="dataListHEAD" height="23">連絡人</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key66" size="12" maxlength="12" value="<%=dspKey(66)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text7">
        <input type="radio" name="rd4"  <%=fieldpb%> onClick="SrAddrEqual4()"><font size=2>同申請人</font></td>                                 
 
 </tr>        
<TR>        
        <td  class="dataListHEAD" height="23">行動電話</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key67" size="15" maxlength="15" value="<%=dspKey(67)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text9"></td>                                 
        <td  class="dataListHEAD" height="23">連絡電話(白天)</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" name="key69" size="15" maxlength="15" value="<%=dspKey(69)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text16"></td>                                 
 </tr>
<TR>        
        <td  class="dataListHEAD" height="23">連絡EMAIL</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" name="key68" size="30" maxlength="30" value="<%=dspKey(68)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text9"></td>                                 
        <td  class="dataListSEARCH" height="23">合約編號</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" name="key115" size="17" maxlength="15" value="<%=dspKey(115)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text11"></td>                                 
 </tr> 
</table> </div>
<!--
    <table border="1" width="100%" cellpadding="0" cellspacing="0" id="tag2" style="display: none"> 
    -->
    <DIV ID="SRTAG1" onclick="srtag1" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="LEFT">績效歸屬</td></tr></table></div>
     <DIV ID=SRTAB1 >
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table7">
<tr>   <td id="tagT1" WIDTH="15%" rowspan=2 class="dataListHEAD" height="23">業務轄區</td>               
        <td  WIDTH="85%" height="23" bgcolor="silver">
<%'  If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(90))) = 0  Then 
  '     sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '1') "
  '     s="<option value="""" >(業務轄區)</option>"
  '  Else
       sql="SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '1') AND AREAID='" & DSPKEY(101) & "' "
       s="<option value="""" >(業務轄區)</option>"
  '  End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(業務轄區)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("areaid")=dspkey(101) Then sx=" selected "
       s=s &"<option value=""" &rs("areaid") &"""" &sx &">" &rs("areanc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>    
           <select size="1" name="key101" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" ID="Select1">                                            
              <%=s%>
           </select>
    <input type="text" name="key102" <%=fieldRole(1)%><%=dataProtect%> 
               style="text-align:left;" size="15" maxlength="10" 
               value="<%=dspKey(102)%>"   readonly class="dataListEntry" ID="Text64">
         <input type="button" id="B102"  <%=fieldpb%>  name="B102"   width="100%" style="Z-INDEX: 1"  value="...." readonly onclick="SrsalesGrouponclick()"  >  
          <IMG SRC="/AVSCONSIGNEE/IMAGES/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C102"  name="C102"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">                               
    <input type="TEXT" name="key103" <%=fieldRole(1)%><%=dataProtect%> 
               style="text-align:left;" size="15" maxlength="10" 
               value="<%=dspKey(103)%>"  readonly class="dataListDATA" ID="Hidden1">
           <input type="BUTTON" id="B103"  <%=fieldpb%>  name="B103"  width="100%" style="Z-INDEX: 1"  value="...." onclick="Srsalesonclick()"  >   
           <IMG SRC="/AVSCONSIGNEE/IMAGES/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C103"  name="C103"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">                               
        </td>
 </tr>        
  </table>     
  </DIV> 
  </DIV>   
      <DIV ID="SRTAG2" onclick="srtag2" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
    <tr><td bgcolor="BDB76B" align="LEFT">促銷代碼</td></tr></table></DIV>
    <DIV ID=SRTAB2 >
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3">
<tr>   <td  WIDTH="15%" class="dataListHEAD" height="23">AVS優惠代碼</td>               
        <td  WIDTH="35%" height="23" bgcolor="silver" >
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(90))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='G8' " 
       If len(trim(dspkey(71))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='G8' AND CODE='" & dspkey(71) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(71) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key71" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35">                                                                  
        <%=s%>
   </select>
        </td>
        <td  WIDTH="15%" class="dataListHEAD" height="23">撥號選接優惠代碼</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(90))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='G9' " 
       If len(trim(dspkey(72))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='G9' AND CODE='" & dspkey(72) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(72) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key72" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35">                                                                  
        <%=s%>
   </select></td>                                 
                              
 </tr>        
 <tr>   <td  WIDTH="15%"  class="dataListHEAD" height="23">AVS繳款方式</td>               
        <td  WIDTH="35%" height="23" bgcolor="silver" >
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(90))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='G6' " 
       If len(trim(dspkey(70))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='G6' AND CODE='" & dspkey(70) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(70) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key70" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35">                                                                  
        <%=s%>
   </select>
        </td>
        <td  WIDTH="15%" class="dataListHEAD" height="23">DIALER資費方案</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(dspKey(89))) = 0 AND len(trim(dspKey(90))) = 0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='G7' " 
       If len(trim(extDB(0))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='G7' AND CODE='" & extDB(0) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=extDB(0) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="ext0" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select7">                                                                  
        <%=s%>
   </select></td>
   </tr>                               
    <TR>
        <td  WIDTH="15%" class="dataListHEAD" height="23">原服務到期日</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver" >
         <input type="text" name="key114" <%=fieldRole(1)%><%=dataProtect%> 
               style="text-align:left;" size="15" maxlength="10" 
               value="<%=dspKey(114)%>"   readonly class="dataListEntry" ID="Text1">        
        <input type="button" id="B114"  name="B114" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/AVSCONSIGNEE/IMAGES/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C114"  name="C114"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"> </td>           
         </td>           
        <td   width=15% class="dataListHEAD" height="23">電話號碼</td>                                 
        <td   width=35% height="23" bgcolor="silver">
        <%
        '當程式為 "修改時" ，不允許修改DIALER電話號碼==>因為DIALER電話為KEY值
        'RESPONSE.Write "SW=" & SW & ";ACCESSMODE=" & ACCESSMODE
        IF sw="E" THEN
           FIELDPX=" CLASS=""DATALISTDATA"" READONLY "
        ELSE
           FIELDPX=""
        END IF
        IF ACCESSMODE="U" THEN
           FIELDPY=" CLASS=""DATALISTDATA"" READONLY "
        ELSE
           FIELDPY=""
        END IF        
        %>
        <input type="text" name="ext1" size="15" maxlength="15" value="<%=extDB(1)%>"  <%=fieldpX%><%=fieldpY%><%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text2">
        <input STYLE="DISPLAY:NONE" type="text" name="ext2" size="10" maxlength="15" value="<%=extDB(2)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text3">
        <input STYLE="DISPLAY:NONE" type="text" name="ext3" size="2" maxlength="2" value="<%=extDB(3)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text10"></td>                                 

 </tr>        
  </table>     
  </DIV>
      <DIV ID="SRTAG5" onclick="srtag5" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
    <tr><td bgcolor="BDB76B" align="LEFT">代理人資訊</td></tr></table></DIV>
    <DIV ID=SRTAB5 >
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3"> 
<TR>        
        <td   width=15% class="dataListHEAD" height="23">代理人姓名</td>                                 
        <td   width=35% height="23" bgcolor="silver">
        <input type="text" name="key74" size="10" maxlength="15" value="<%=dspKey(74)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text9"></td>                                 
        <td   width=15% class="dataListHEAD" height="23">代理人身份證號</td>                                 
        <td  width=35% height="23" bgcolor="silver" >
        <input type="text" name="key76" size="10" maxlength="10" value="<%=dspKey(76)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text16"></td>                                 
 </tr>
<TR>        
        <td  class="dataListHEAD" height="23">代理人電話</td>                                 
        <td  height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key75" size="15" maxlength="15" value="<%=dspKey(75)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text9"></td>                                 
 </tr>     
<tr><td class=dataListHEAD>代理人地址</td>
    <td bgcolor="silver" COLSPAN=3>
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND len(trim(dspKey(90))) = 0 Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(77))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
       SXX78=" onclick=""Srcounty78onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(77) & "' " 
       SXX78=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(77) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
         <select size="1" name="key77" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select2"><%=s%></select>
        <input type="text" name="key78" readonly  size="8" value="<%=dspkey(78)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text4"><font size=2>(鄉鎮)                 
         <input type="button" id="B78"  <%=fieldpb%>  name="B78"   width="100%" style="Z-INDEX: 1"  value="..." <%=SXX78%>  >        
          <IMG SRC="/AVSCONSIGNEE/IMAGES/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C78"  name="C78"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
        
        <input type="text" name="key79" <%=fieldpa%> size="10" value="<%=dspkey(79)%>" maxlength="10" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text5"><font size=2>
        <% aryOption=Array("村","里")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(104)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(104) &""">" &dspKey(104) &"</option>"
   End If%>                                  
       <select size="1" name="key104" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select3">                                            
        <%=s%></select>      
        <input type="text" name="key80"  size="6" value="<%=dspkey(80)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text6"><font size=2>
        <% aryOption=Array("鄰")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(105)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(105) &""">" &dspKey(105) &"</option>"
   End If%>                                  
       <select size="1" name="key105" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select4">                                            
        <%=s%></select>              
        <input type="text" name="key81" size="10" value="<%=dspkey(81)%>" maxlength="10" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text27"><font size=2>
        <% aryOption=Array("路","街")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(106)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(106) &""">" &dspKey(106) &"</option>"
   End If%>                                  
       <select size="1" name="key106" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select5">                                            
        <%=s%></select>                      
        <input type="text" name="key82"  size="6" value="<%=dspkey(82)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text29"><font size=2>
        <% aryOption=Array("段")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(107)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(107) &""">" &dspKey(54) &"</option>"
   End If%>                                  
       <select size="1" name="key107" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select6">                                            
        <%=s%></select>
        <br>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;
         <input type="text" name="key100"  readonly size="5" value="<%=dspkey(100)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text35">
        <input type="text" name="key83" size="6" value="<%=dspkey(83)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text30"><font size=2>
        <% aryOption=Array("巷")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(108)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(108) &""">" &dspKey(108) &"</option>"
   End If%>                                  
       <select size="1" name="key108" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select9">                                            
        <%=s%></select>        
        <input type="text" name="key84" size="6" value="<%=dspkey(84)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text31"><font size=2>
                <% aryOption=Array("弄")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(109)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(109) &""">" &dspKey(109) &"</option>"
   End If%>                                  
       <select size="1" name="key109" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select10">                                            
        <%=s%></select>    
        <input type="text" name="key85" size="6" value="<%=dspkey(85)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text32"><font size=2>
                <% aryOption=Array("號")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(110)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(110) &""">" &dspKey(110) &"</option>"
   End If%>                                  
       <select size="1" name="key110" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select11">                                            
        <%=s%></select>            
        <input type="text" name="key86" size="6" value="<%=dspkey(86)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text33"><font size=2>
                <% aryOption=Array("樓")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(111)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(111) &""">" &dspKey(111) &"</option>"
   End If%>                                  
       <select size="1" name="key111" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select12">                                            
        <%=s%></select>
        <input type="text" name="key87" size="6" value="<%=dspkey(87)%>" maxlength="6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text34"><font size=2>
                <% aryOption=Array("室")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 AND len(trim(dspKey(90))) = 0 Then
      For i = 0 To Ubound(aryOption)
          If dspKey(112)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(112) &""">" &dspKey(112) &"</option>"
   End If%>                                  
       <select size="1" name="key112" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select13">                                            
        <%=s%></select>       
        </td>                                 
</tr></TABLE>    
    </div> 
   <DIV ID="SRTAG4" onclick="srtag4" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr><td bgcolor="BDB76B" align="LEFT">主線申請及施工進度狀態</td></tr></table></DIV>
    <DIV ID=SRTAB4 >  
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
       <tr>
        <td  width=15% class="dataListHEAD" height="23">AVS開通申請日</td>                                 
        <td  width=35% height="23" bgcolor="silver">
        <input type="text" name="key89" size="10" value="<%=dspKey(89)%>"  <%=fieldpa%><%=fieldRole(1)%> readonly class="dataListentry" ID="Text51">     
       <input type="button" id="B89"  name="B89" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
        <IMG SRC="/AVSCONSIGNEE/IMAGES/IMGDELETE.GIF" alt="清除" id="C89"  name="C89"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">     
        <td   width=15% class="dataListHEAD" height="23">AVS申請轉檔日</td>                                 
        <td   width=35% height="23" bgcolor="silver">
        <input type="text" name="key90" size="10" value="<%=dspKey(90)%>"  <%=fieldpb%><%=fieldRole(1)%> readonly class="dataListDATA" ID="Text52">
</td>        
     </tr>             
        <tr>
        <td   class="dataListHEAD" height="23">EBT申請審核碼</td>                                 
        <td   height="23" bgcolor="silver">
        <input type="text" name="key91" size="10" value="<%=dspKey(91)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListdata" ID="Text55">     
        <td   class="dataListHEAD" height="23">施工進度</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key92" size="10" value="<%=dspKey(92)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListdata" ID="Text56">
      </tr>     
        <tr>
        <td   class="dataListHEAD" height="23">完工日期</td>                                 
        <td   height="23" bgcolor="silver" >
        <input type="text" name="key93" size="10" READONLY value="<%=dspKey(93)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListentry" ID="Text57">
                <input type="button" id="B93"  name="B93" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
        <IMG SRC="/AVSCONSIGNEE/IMAGES/IMGDELETE.GIF" alt="清除" id="C93"  name="C93"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">     
        </td>
        <td   class="dataListHEAD" height="23">報竣日期</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key94" size="10" READONLY value="<%=dspKey(94)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListentry" ID="Text56">
                <input type="button" id="B94"  name="B94" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
        <IMG SRC="/AVSCONSIGNEE/IMAGES/IMGDELETE.GIF" alt="清除" id="C94"  name="C94"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">     
        
</tr> 
        <tr>
        <td   class="dataListHEAD" height="23">報竣轉檔日</td>                                 
        <td   height="23" bgcolor="silver" >
        <input type="text" name="key95" size="10" value="<%=dspKey(95)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListDATA" ID="Text57">     
        </td>
        <td   class="dataListHEAD" height="23">EBT開始計費日</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key96" size="10" value="<%=dspKey(96)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListdata" ID="Text56">
</tr>        
       
  </table> 
  </DIV>
  </div> 
<%
    conn.Close   
    set rs=Nothing   
    set conn=Nothing 
End Sub 
' -------------------------------------------------------------------------------------------- 
Sub SrReadExtDB()
    Dim conn,rs
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open DSN
    Set rs=Server.CreateObject("ADODB.Recordset")
    'RESPONSE.Write "SELECT * FROM RTEBTcustext WHERE comq1=" &dspKey(0) &" and lineq1=" & dspkey(1) & " and cusid='" & dspkey(2) & "' and dropdat is null "
    rs.Open "SELECT * FROM RTEBTcustext WHERE comq1=" &dspKey(0) &" and lineq1=" & dspkey(1) & " and cusid='" & dspkey(2) & "' and dropdat is null ",conn
    if rs.eof then
       'dialer資費方案
       extdb(0)=""
       '電話號碼
       extdb(1)=""
       '項次
       extdb(2)=1
       EXTDB(3)="03"
    else
       extDB(0)=rs("DIALERPAYTYPE")
       extdb(1)=rs("telno")
       extDB(2)=RS("ENTRYNO")
       extDB(3)=RS("SRVTYPE")
    end if
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
'------RTEBTcustext ---------------------------------------------------
    rs.Open "SELECT * FROM RTEBTcustext WHERE comq1=" &dspKey(0) &" and lineq1=" & dspkey(1) & " and cusid='" & dspkey(2) & "' AND ENTRYNO=" & EXTDB(2) & " AND TELNO='" & EXTDB(1) & "' ",conn,3,3
    If rs.Eof Or rs.Bof Then
       If Smode="A" Then
          rs.AddNew
          rs("COMQ1")=dspKey(0)
          rs("LINEQ1")=dspKey(1)
          rs("CUSID")=dspKey(2)
          rs("ENTRYNO")=EXTDB(2)
          rs("TELNO")=EXTDB(1)
       End If
    End If
    rs("SRVTYPE")=extDB(3)
    rs("DIALERPAYTYPE")=extDB(0)
    RS("SDATE")=dspKey(113)
    rs.Update
    rs.Close

    conn.Close
    Set rs=Nothing
    Set conn=Nothing
End Sub
' -------------------------------------------------------------------------------------------- 
' --------------------------------------------------------------------------------------------  
%>
<!-- #include file="RTGetUserRight.inc" -->
<!-- #include virtual="/AVSCONSIGNEE/employeeref.inc" -->