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
  dim extDBField2,extDB2(300),extDBField3,extDB3(300),extDBField4,extDB4(300)
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
                   case ucase("/webap/rtap/base/ktscust/ktsCUSTteld.asp")
                     '  response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       if i <> 1 then rs.Fields(i).Value=dspKey(i)    
                       if i=1 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         cusidxx="P" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
                         rsc.open "select max(PNO) AS PNO from KTSCUSTTELH where PNO like '" & cusidxx & "%' " ,conn
                         if len(rsc("PNO")) > 0 then
                            dspkey(1)=cusidxx & right("000" & cstr(cint(right(rsc("PNO"),3)) + 1),3)
                         else
                            dspkey(1)=cusidxx & "001"
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
                 case ucase("/webap/rtap/base/KTSCUST/KTScustTELd.asp")
                  ' response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                     if i<>1  then rs.Fields(i).Value=dspKey(i)         
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
       if ucase(runpgm)=ucase("/webap/rtap/base/KTSCUST/KTSCUSTTELD.asp") then
          cusidxx="P" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
          rsc.open "select max(PNO) AS PNO from KTScustTELH where PNO like '" & cusidxx & "%' " ,conn
          if not rsC.eof then
            dspkey(1)=rsC("PNO")
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
<input type="text" name="sw" value="<%=sw%>" style="display:none;" ID="Text17" size="20">
<input type="text" name="reNew" value="N" style="display:none;" ID="Text18" size="20">
<input type="text" name="rwCnt" value="<%=rwCnt%>" style="display:none;" ID="Text19" size="20">
<input type="text" name="accessMode" value="<%=accessMode%>" style="display:none;" ID="Text20" size="20">
<table width="100%" ID="Table1">
  <tr class=dataListTitle><td width="20%">　</td><td width="60%" align=center>
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
  title="KTS用戶申請電話明細資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT  CUSID, PNO, APPLYDAT, TRANSDAT, CANCELDAT, EUSR, EDAT, UUSR,UDAT,CANCELUSR,MEMO FROM KTSCUSTTELH WHERE CUSID='' "
  sqlList="SELECT  CUSID, PNO, APPLYDAT, TRANSDAT, CANCELDAT, EUSR, EDAT, UUSR,UDAT,CANCELUSR,MEMO FROM KTSCUSTTELH WHERE "
  userDefineRead="Yes"      
  userDefineSave="Yes"       
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField2=300
  extDBField3=300
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
  If len(trim(dspkey(4)))<> 0 then
       formValid=False
       message="電話申請/異動單已轉檔不可異動"    
  ELSEIf len(trim(dspkey(2)))=0 or Not Isdate(dspkey(2)) then
       formValid=False
       message="用戶申請日不可空白或格式錯誤"                
  end if
  '資料合理性檢查
  '(a)作廢電話必須已存在系統中且已開通，否則不可輸入
  '(b)新增及作廢電話不可重複建檔
  if formvalid=true then
     Set connXX=Server.CreateObject("ADODB.Connection")
     Set RSXX=Server.CreateObject("ADODB.RECORDSET")
     connXX.open DSN
     J=0
     K=0
     for i=13 to 203 step 10
         j=i+1
         x=i+5
         '作廢電話
         if len(trim(EXTDB3(x))) = 0 then
         if len(trim(EXTDB3(I))) > 0 and len(trim(EXTDB3(j))) > 0 then
            K=K+1
            SQLXX="select COUNT(*) AS CNT FROM KTSCUSTD1 WHERE TEL11='" & EXTDB3(I) & "' AND TEL12='" & EXTDB3(J) & "' " 
            'RESPONSE.Write SQLXX
            RSXX.Open SQLXX,CONNXX
            IF LEN(TRIM(RSXX("CNT")))=0 OR ISNULL(RSXX("CNT")) OR RSXX("CNT") < 1 THEN
               formValid=False
               message="作廢電話不存在已開通電話清單中，不可作廢此電話。電話號碼︰" & EXTDB3(I) & "-" & EXTDB3(J)  
               EXIT FOR
            END IF
            RSXX.CLOSE
         end if
         '新增電話
         if len(trim(EXTDB2(I))) > 0 and len(trim(EXTDB2(j))) > 0 then
            K=K+1
            SQLXX="select COUNT(*) AS CNT FROM KTSCUSTD1 WHERE TEL11='" & EXTDB2(I) & "' AND TEL12='" & EXTDB2(J) & "' AND DROPDAT IS NULL " 
               
            RSXX.Open SQLXX,CONNXX
            IF RSXX("CNT") > 0 THEN
               formValid=False
               message="此電話已存在開通電話清單中，不可重複建檔。電話號碼︰" & EXTDB2(I) & "-" & EXTDB2(J)  
               EXIT FOR
            END IF
            RSXX.CLOSE
         END IF
         end if
     next
  end if

'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(7)=V(0)
        dspkey(8)=datevalue(now())
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
   Sub SrbtnonclickA()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,6,len(window.event.srcElement.id)-1)
       clickkey="EXTA" & clickid
	   if isdate(document.all(clickkey).value) then
	      objEF2KDT.varDefaultDateTime=document.all(clickkey).value
       end if
       call objEF2KDT.show(1)
       if objEF2KDT.strDateTime <> "" then
          document.all(clickkey).value = objEF2KDT.strDateTime
       end if
   END SUB
   Sub SrClearA()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,6,len(window.event.srcElement.id)-1)
      ' MSGBOX CLICKID
       clickkey="CEXTA" & clickid
      ' MSGBOX CLICKKEY
       clearkey="EXTA" & clickid    
      ' MSGBOX CLEARKEY
       if len(trim(document.all(clearkey).value)) <> 0 then
          document.all(clearkey).value =  ""
       end if
       CLICKID2=CSTR(CINT(CLICKID) - 1)
       clearkey="EXTA" & clickid2
       if len(trim(document.all(clearkey).value)) <> 0 then
          document.all(clearkey).value =  ""
       end if
   End Sub    
   Sub SrbtnonclickB()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,6,len(window.event.srcElement.id)-1)
       clickkey="EXTB" & clickid
	   if isdate(document.all(clickkey).value) then
	      objEF2KDT.varDefaultDateTime=document.all(clickkey).value
       end if
       call objEF2KDT.show(1)
       if objEF2KDT.strDateTime <> "" then
          document.all(clickkey).value = objEF2KDT.strDateTime
       end if
   END SUB
   Sub SrClearB()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,6,len(window.event.srcElement.id)-1)
      ' MSGBOX CLICKID
       clickkey="CEXTB" & clickid
      ' MSGBOX CLICKKEY
       clearkey="EXTB" & clickid    
      ' MSGBOX CLEARKEY
       if len(trim(document.all(clearkey).value)) <> 0 then
          document.all(clearkey).value =  ""
       end if
       CLICKID2=CSTR(CINT(CLICKID) - 1)
       clearkey="EXTB" & clickid2
       if len(trim(document.all(clearkey).value)) <> 0 then
          document.all(clearkey).value =  ""
       end if
   End Sub       
   Sub SrCloseA()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,6,len(window.event.srcElement.id)-1)
       key0="EXTA" & cstr(cint(clickid)-7)
       key1="EXTA" & cstr(cint(clickid)-6)
       key2="EXTA" & cstr(cint(clickid)-5)
       K0=document.all(KEY0).value
       K1=document.all(KEY1).value
       K2=document.all(KEY2).value
       StrFeature="top=0,left=0,scrollbars=yes,status=yes," _
                 &"location=no,menubar=no,width=1px" _
                 &",height=1px" 
       prog="KTSCUSTTELF.asp"
       prog=prog & "?KEY=" & K0 & ";" & K1 & ";" & K2
       set FPGM=Window.OPEN(prog,"",StrFeature)
       window.form.submit
       'MSGBOX prog
   End Sub    
   Sub SrCloseB()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,6,len(window.event.srcElement.id)-1)
       key0="EXTB" & cstr(cint(clickid)-7)
       key1="EXTB" & cstr(cint(clickid)-6)
       key2="EXTB" & cstr(cint(clickid)-5)
       K0=document.all(KEY0).value
       K1=document.all(KEY1).value
       K2=document.all(KEY2).value
       StrFeature="top=0,left=0,scrollbars=yes,status=yes," _
                 &"location=no,menubar=no,width=1px" _
                 &",height=1px" 
       prog="KTSCUSTTELF.asp"
       prog=prog & "?KEY=" & K0 & ";" & K1 & ";" & K2
       set FPGM=Window.OPEN(prog,"",StrFeature)
       window.form.submit
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
   document.All("key13").value=document.All("KEY9").value
   document.All("key14").value=document.All("key10").value
   document.All("key15").value=document.All("KEY11").value
   document.All("key16").value=document.All("key12").value
End Sub 
Sub SrAddrEqual2()
   document.All("key17").value=document.All("KEY9").value
   document.All("key18").value=document.All("key10").value
   document.All("key19").value=document.All("KEY11").value
   document.All("key20").value=document.All("key12").value
End Sub         
Sub SrAddrEqual3()
   document.All("key17").value=document.All("KEY13").value
   document.All("key18").value=document.All("key14").value
   document.All("key19").value=document.All("KEY15").value
   document.All("key20").value=document.All("key16").value
End Sub         
Sub SrAddrEqual4()
End Sub       
SUB SrSHOWTELLISTOnClick()
       IF window.SRTAR1.style.display="" THEN
          window.SRTAR1.style.display="none"
          document.all("STL").value="顯示電話明細"
       ELSE
          window.SRTAR1.style.display=""
          document.all("STL").value="隱藏電話明細"
       end if
 END SUB 
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
<form method=post name="keyForm" ID="Form1">
      <table width="55%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="15%" class=dataListHead>用戶代號</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key0"
                 <%=fieldRole(1)%> readonly size="15" value="<%=dspKey(0)%>" maxlength="15" class=dataListdata></td>
<td width="15%" class=dataListHead>電話申請單號</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key1"
                 <%=fieldRole(1)%> readonly size="15" value="<%=dspKey(1)%>" maxlength="15" class=dataListdata ID="Text28"></td>                 
  </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(5))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(5)=V(0)
        End if  
       dspkey(6)=datevalue(now())
    else
        if len(trim(dspkey(7))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(7)=V(0)
        End if         
        dspkey(8)=datevalue(now())
    end if      
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
    '用戶送件申請後,基本資料 protect
    If len(trim(dspKey(3))) > 0 OR len(trim(dspKey(4))) > 0 Then
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
  <span id="tags1" class="dataListTagsOn">KTS電話申請單資訊</span>
                                                            
  <div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">　</td><td width="96%">　</td><td width="2%">　</td></tr>
<tr><td>　</td>
<td>     
    <DIV ID="SRTAG0" onclick="srtag0" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="LEFT">基本資料</td></tr></table></div>
 <DIV ID=SRTAB0 >   
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
<tr>
<td width="14%" class=dataListHEAD>用戶申請日</td>
    <td width="20%" bgcolor="silver" >
        <input type="text" name="key2" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(2)%>"  READONLY size="10" class=dataListEntry ID="Text48">
       <input  type="button" id="B2"  <%=fieldpb%> name="B2" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C2"  name="C2"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
      </td>      
<td width="14%" class=dataListHEAD>申請日轉檔日</td>
    <td width="20%" bgcolor="silver" >
        <input type="text" name="key3" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(3)%>"  READONLY size="10" class=dataListDATA ID="Text4">
       </td></tr>   
<tr><td width="14%" class=dataListHEAD>作廢人員</td>
    <td width="20%" bgcolor="silver" >
            <%  name="" 
           if dspkey(9) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(9) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>   
        <input type="text" name="key9" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="6"
               value="<%=dspKey(9)%>"  READONLY size="6" class=dataListDATA ID="Text7"><font size=2><%=name%></font>
      </td>
<td width="14%" class=dataListHEAD>作廢日</td>
    <td width="20%" bgcolor="silver" >
        <input type="text" name="key4" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(4)%>"  READONLY size="10" class=dataListDATA ID="Text5">
      </td>      
</tr>         
<tr>
        <td  class="dataListHEAD" height="23">建檔人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
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
  %>    <input type="text" name="key5" size="6" READONLY value="<%=dspKey(5)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">建檔日期</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" name="key6" size="10" READONLY value="<%=dspKey(6)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>  
<tr>
        <td  class="dataListHEAD" height="23">修改人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(7) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(7) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>    <input type="text" name="key7" size="6" READONLY value="<%=dspKey(7)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">修改日期</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" name="key8" size="10" READONLY value="<%=dspKey(8)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>         
</table> </div>
    <DIV ID="SRTAG1" onclick="srtag1" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table10">
    <tr><td bgcolor="BDB76B" align="LEFT">電話明細</td></tr></table></div>
     <DIV ID="SRTAB1" >
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table11">
    <tr><td WIDTH=50% ALIGN="center" class=dataListsearch >新申請電話</td>
            <td WIDTH=50% ALIGN="center" class=dataListsearch >作廢電話</td></tr>
    <tr><td><table border="1" width="100%" cellpadding="0" cellspacing="0">
         <tr>
            <td WIDTH=5% ALIGN="center" class=dataListDATA>項次</td>
            <td WIDTH=15% ALIGN="center" class=dataListDATA >電話號碼</td>
            <td WIDTH=6% ALIGN="center" class=dataListDATA>異動別</td>
            <td WIDTH=15% ALIGN="center" class=dataListDATA>開通日</td>
            <td WIDTH=5% ALIGN="center" class=dataListDATA>結案</td>
          </tr>
          <%
    IF (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  AND len(trim(DSPKEY(38))) = 0 THEN
       BTNENABLE=""
    ELSE
       BTNENABLE=" DISABLED "
    END IF
    %>
    <%
     s=""
     sql="SELECT  *  FROM  KTSCUSTTELD1 where cusid='" & dspkey(0) & "' and PNO='" & DSPKEY(1) & "' and aord='A' ORDER BY ENTRYNO"
    ' RESPONSE.Write SQL
     rs.Open sql,conn
     cnt=0
     Do While Not rs.Eof
       IF LEN(TRIM(RS("finishDAT"))) > 0  OR  LEN(TRIM(DSPKEY(4))) > 0 THEN
          IMG="/WEBAP/IMAGE/DENY2.GIF"
          DEX="結案返轉"
          fieldPC=" class=""dataListData"" readonly "
          fieldpD=" disabled "
       ELSE
          IMG="/WEBAP/IMAGE/AGREE2.GIF" 
          DEX="結案"
          fieldPC=""
          fieldpD=""
       END IF
       IF sw="" OR  LEN(TRIM(DSPKEY(3))) = 0 THEN
          fieldPC=" class=""dataListData"" readonly "
          fieldpD=" disabled "
       END IF
       CNT=CNT+1
       RESPONSE.Write "<TR>"
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  BGCOLOR=SILVER STYLE=""DISPLAY:NONE""><input type=""HIDDEN"" name=""extA" & cnt & "0""" & " size=""10"" VALUE=""" & RS("CUSID") & """ " & fieldpa & " " & fieldRole(1) & "  class=""dataListENTRY""></td>" &vbCrLf 
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  BGCOLOR=SILVER STYLE=""DISPLAY:NONE""><input type=""HIDDEN"" name=""extA" & cnt & "1""" & " size=""10"" VALUE=""" & RS("PNO") & """ " & fieldpa & " " & fieldRole(1) & "  class=""dataListENTRY""></td>" &vbCrLf 
       RESPONSE.Write "<td WIDTH=4% ALIGN=""center""  BGCOLOR=SILVER ><input type=""TEXT"" name=""extA" & cnt & "2""" & " size=""2"" READONLY VALUE=" & RS("ENTRYNO") & " " & fieldpa & " " & fieldRole(1) & "  class=""dataListDATA""></td>" &vbCrLf 
       RESPONSE.Write "<td WIDTH=20% ALIGN=""center""  BGCOLOR=SILVER><input type=""TEXT"" name=""extA" & cnt & "3""" & " size=""4"" VALUE=""" & RS("TEL11") & """ " & fieldpa & " " & fieldpC & fieldRole(1) & "  class=""dataListENTRY""><FONT SIZE=2>-</FONT>"
       RESPONSE.Write "<input type=""TEXT"" name=""extA" & cnt & "4""" & " size=""8"" VALUE=""" & RS("TEL12") & """ " & fieldpa & " " & fieldpC & fieldRole(1) & "  class=""dataListENTRY""> " _
                     &"<IMG  SRC=""/WEBAP/IMAGE/IMGDELETE.GIF"" " & fieldpb & fieldpD & " alt=""清除"" id=""CEXTA" & cnt & "4""" & " name=""CEXTA" & cnt & "4""" & "  style=""Z-INDEX: 1""  border=0 onmouseover=""ImageIconOver"" onmouseout=""ImageIconOut"" onclick=""SrClearA""></td>" &vbCrLf 
       RESPONSE.Write "<td WIDTH=4% ALIGN=""center""  BGCOLOR=SILVER><input type=""TEXT"" name=""extA" & cnt & "5""" & " size=""1"" READONLY VALUE=""" & RS("AORD") & """ " & fieldpa & " " & fieldRole(1) & "  class=""dataListDATA""></td>" &vbCrLf 
       RESPONSE.Write "<td WIDTH=15% ALIGN=""center""  BGCOLOR=SILVER><input type=""TEXT"" name=""extA" & cnt & "6""" & " size=""10"" READONLY  VALUE=""" & RS("finishDAT") & """ " & fieldpa & " " & fieldRole(1) & "  class=""dataListDATA""></td>" &vbCrLf 
       RESPONSE.Write "<td WIDTH=5% ALIGN=""center""  BGCOLOR=SILVER><IMG  SRC=""" & IMG & """ " & fieldpD & " alt=""" & DEX & """ id=""REXTA" & cnt & "7""" & " name=""REXTA" & cnt & "7""" & "  style=""Z-INDEX: 1""  border=0 onmouseover=""ImageIconOver"" onmouseout=""ImageIconOut"" onclick=""SrcloseA""></td>" &vbCrLf 
       rs.MoveNext
     Loop
       IF LEN(TRIM(DSPKEY(3))) > 0  OR  LEN(TRIM(DSPKEY(4))) > 0  THEN
          IMG="/WEBAP/IMAGE/DENY2.GIF"
          DEX="結案返轉"
          fieldPC=" class=""dataListData"" readonly "
          fieldpD=" disabled "
       ELSE
          IMG="/WEBAP/IMAGE/AGREE2.GIF" 
          DEX="結案"
          fieldPC=""
          fieldpD=""
       END IF
       IF sw="" OR LEN(TRIM(DSPKEY(3))) = 0 THEN
          fieldPC=" class=""dataListData"" readonly "
          fieldpD=" disabled "
       END IF
     DO WHILE CNT <= 20
       CNT = CNT + 1
       RESPONSE.Write "<TR>"
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  BGCOLOR=SILVER STYLE=""DISPLAY:NONE""><input type=""HIDDEN"" name=""extA" & cnt & "0""" & " size=""10"" VALUE="""" " & fieldpa & " " & fieldRole(1) & "  class=""dataListENTRY""></td>" &vbCrLf 
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  BGCOLOR=SILVER STYLE=""DISPLAY:NONE""><input type=""HIDDEN"" name=""extA" & cnt & "1""" & " size=""10"" VALUE="""" " & fieldpa & " " & fieldRole(1) & "  class=""dataListENTRY""></td>" &vbCrLf 
       RESPONSE.Write "<td WIDTH=4% ALIGN=""center""  BGCOLOR=SILVER ><input type=""TEXT"" name=""extA" & cnt & "2""" & " size=""2"" READONLY  " & " " & fieldpa & " " & fieldRole(1) & "  class=""dataListDATA""></td>" &vbCrLf 
       RESPONSE.Write "<td WIDTH=20% ALIGN=""center""  BGCOLOR=SILVER><input type=""TEXT"" name=""extA" & cnt & "3""" & " size=""4"" VALUE="""" " & fieldpa & fieldpC & " " & fieldRole(1) & "  class=""dataListENTRY""><FONT SIZE=2>-</FONT>"
       RESPONSE.Write "<input type=""TEXT"" name=""extA" & cnt & "4""" & " size=""8"" VALUE="""" " & fieldpa & fieldpC & " " & fieldRole(1) & "  class=""dataListENTRY""><IMG  SRC=""/WEBAP/IMAGE/IMGDELETE.GIF"" " & fieldpb & fieldpD & " alt=""清除"" id=""CEXTA" & cnt & "4""" & " name=""CEXTA" & cnt & "4""" & "  style=""Z-INDEX: 1""  border=0 onmouseover=""ImageIconOver"" onmouseout=""ImageIconOut"" onclick=""SrClearA""></td>" &vbCrLf 
       RESPONSE.Write "<td WIDTH=4% ALIGN=""center""  BGCOLOR=SILVER><input type=""TEXT"" name=""extA" & cnt & "5""" & " size=""1"" READONLY VALUE=""A"" " & fieldpa & " " & fieldRole(1) & "  class=""dataListDATA""></td>" &vbCrLf 
       RESPONSE.Write "<td WIDTH=15% ALIGN=""center""  BGCOLOR=SILVER><input type=""TEXT"" name=""extA" & cnt & "6""" & " size=""10"" READONLY VALUE="""" " & fieldpa & " " & fieldRole(1) & "  class=""dataListDATA""></td>" &vbCrLf
       RESPONSE.Write "<td WIDTH=5% ALIGN=""center""  BGCOLOR=SILVER>&nbsp;</td>" &vbCrLf 
     LOOP
     rs.Close
    %>
    </TR>
    </table>
    </TD>
    <td><table border="1" width="100%" cellpadding="0" cellspacing="0">
         <tr>
            <td WIDTH=5% ALIGN="center" class=dataListDATA>項次</td>
            <td WIDTH=15% ALIGN="center" class=dataListDATA >電話號碼</td>
            <td WIDTH=6% ALIGN="center" class=dataListDATA>異動別</td>
            <td WIDTH=15% ALIGN="center" class=dataListDATA>作廢日</td>
            <td WIDTH=5% ALIGN="center" class=dataListDATA>結案</td>
          </tr>
        <%
     s=""
     sql="SELECT  *  FROM  KTSCUSTTELD1 where cusid='" & dspkey(0) & "' and PNO='" & DSPKEY(1) & "' and aord='D' ORDER BY ENTRYNO"
    ' RESPONSE.Write SQL
     rs.Open sql,conn
     cnt=0
     Do While Not rs.Eof
       IF LEN(TRIM(RS("finishDAT"))) > 0  OR  LEN(TRIM(DSPKEY(4))) > 0 THEN
          IMG="/WEBAP/IMAGE/DENY2.GIF"
          DEX="結案返轉"
          fieldPC=" class=""dataListData"" readonly "
          fieldpD=" disabled "
       ELSE
          IMG="/WEBAP/IMAGE/agree2.GIF" 
          DEX="結案"
          fieldPC=""
          fieldpD=""
       END IF
       IF sw="" OR LEN(TRIM(DSPKEY(3))) = 0 THEN
          fieldPC=" class=""dataListData"" readonly "
          fieldpD=" disabled "
       END IF
       CNT=CNT+1
       RESPONSE.Write "<TR>"
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  BGCOLOR=SILVER STYLE=""DISPLAY:NONE""><input type=""HIDDEN"" name=""extB" & cnt & "0""" & " size=""10"" VALUE=""" & RS("CUSID") & """ " & fieldpa & " " & fieldRole(1) & "  class=""dataListENTRY""></td>" &vbCrLf
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  BGCOLOR=SILVER STYLE=""DISPLAY:NONE""><input type=""HIDDEN"" name=""extB" & cnt & "1""" & "  size=""10"" VALUE=""" & RS("PNO") & """ " & fieldpa & " " & fieldRole(1) & "  class=""dataListENTRY""></td>" &vbCrLf
       RESPONSE.Write "<td WIDTH=4% ALIGN=""center""  BGCOLOR=SILVER ><input type=""TEXT"" name=""extB" & cnt & "2""" & "  size=""2"" READONLY VALUE=" & RS("ENTRYNO") & " " & fieldpa & " " & fieldRole(1) & "  class=""dataListDATA""></td>" &vbCrLf
       RESPONSE.Write "<td WIDTH=20% ALIGN=""center""  BGCOLOR=SILVER><input type=""TEXT"" name=""extB" & cnt & "3""" & " size=""4"" VALUE=""" & RS("TEL11") & """ " & fieldpa & fieldpC & " " & fieldRole(1) & "  class=""dataListENTRY""><FONT SIZE=2>-</FONT>"
       RESPONSE.Write "<input type=""TEXT"" name=""extB" & cnt & "4""" & "  size=""8"" VALUE=""" & RS("TEL12") & """ " & fieldpa & fieldpC & " " & fieldRole(1) & "  class=""dataListENTRY""><IMG  SRC=""/WEBAP/IMAGE/IMGDELETE.GIF"" " & fieldpb & fieldpD & " alt=""清除"" id=""CEXTB" & cnt & "4""" & "  name=""CEXTB" & cnt & "4""" & "   style=""Z-INDEX: 1""  border=0 onmouseover=""ImageIconOver"" onmouseout=""ImageIconOut"" onclick=""SrClearB""></td>" &vbCrLf
       RESPONSE.Write "<td WIDTH=4% ALIGN=""center""  BGCOLOR=SILVER><input type=""TEXT"" name=""extB" & cnt & "5""" & "  size=""1"" READONLY VALUE=""" & RS("AORD") & """ " & fieldpa & " " & fieldRole(1) & "  class=""dataListDATA""></td>" &vbCrLf
       RESPONSE.Write "<td WIDTH=15% ALIGN=""center""  BGCOLOR=SILVER><input type=""TEXT"" name=""extB" & cnt & "6""" & "  size=""10"" READONLY VALUE=""" & RS("finishDAT") & """ " & fieldpa & " " & fieldRole(1) & "  class=""dataListDATA""></td>" &vbCrLf
       RESPONSE.Write "<td WIDTH=5% ALIGN=""center""  BGCOLOR=SILVER><IMG  SRC=""" & IMG & """ " & fieldpD & " alt=""" & DEX & """  id=""REXTB" & cnt & "7""" & " name=""REXTB" & cnt & "7""" & "  style=""Z-INDEX: 1""  border=0 onmouseover=""ImageIconOver"" onmouseout=""ImageIconOut"" onclick=""SrcloseB""></td>" &vbCrLf 
       rs.MoveNext
     Loop
       IF LEN(TRIM(DSPKEY(3))) > 0  OR  LEN(TRIM(DSPKEY(4))) > 0 THEN
          IMG="/WEBAP/IMAGE/DENY2.GIF"
          DEX="結案返轉"
          fieldPC=" class=""dataListData"" readonly "
          fieldpD=" disabled "
       ELSE
          IMG="/WEBAP/IMAGE/agree2.GIF" 
          DEX="結案"
          fieldPC=""
          fieldpD=""
       END IF
       IF sw="" OR LEN(TRIM(DSPKEY(3))) = 0 THEN
          fieldPC=" class=""dataListData"" readonly "
          fieldpD=" disabled "
       END IF
     DO WHILE CNT <= 20

       CNT = CNT + 1
       RESPONSE.Write "<TR>"
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center""  BGCOLOR=SILVER STYLE=""DISPLAY:NONE""><input type=""HIDDEN"" name=""extB" & cnt & "0""" & " size=""10"" VALUE="""" " & fieldpa & " " & fieldRole(1) & "  class=""dataListENTRY""></td>" &vbCrLf
       RESPONSE.Write "<td WIDTH=10% ALIGN=""center"" BGCOLOR=SILVER STYLE=""DISPLAY:NONE""><input type=""HIDDEN"" name=""extB" & cnt & "1""" & " size=""10"" VALUE="""" " & fieldpa & " " & fieldRole(1) & "  class=""dataListENTRY""></td>" &vbCrLf
       RESPONSE.Write "<td WIDTH=4% ALIGN=""center""  BGCOLOR=SILVER><input type=""TEXT"" name=""extB" & cnt & "2""" & "  size=""2"" READONLY  " & " " & fieldpa & " " & fieldRole(1) & "  class=""dataListDATA""></td>" &vbCrLf
       RESPONSE.Write "<td WIDTH=20% ALIGN=""center""  BGCOLOR=SILVER><input type=""TEXT"" name=""extB" & cnt & "3""" & " size=""4"" VALUE="""" " & fieldpa & fieldpC & " " & fieldRole(1) & "  class=""dataListENTRY""><FONT SIZE=2>-</FONT>"
       RESPONSE.Write "<input type=""TEXT"" name=""extB" & cnt & "4""" & " size=""8"" VALUE="""" " & fieldpa & fieldpC & " " & fieldRole(1) & "  class=""dataListENTRY""><IMG  SRC=""/WEBAP/IMAGE/IMGDELETE.GIF"" " & fieldpb & fieldpD & " alt=""清除"" id=""CEXTB" & cnt & "7""" & " name=""CEXTB" & cnt & "7""" & "    style=""Z-INDEX: 1""  border=0 onmouseover=""ImageIconOver"" onmouseout=""ImageIconOut"" onclick=""SrClearB""></td>" &vbCrLf
       RESPONSE.Write "<td WIDTH=4% ALIGN=""center""  BGCOLOR=SILVER><input type=""TEXT"" name=""extB" & cnt & "5""" & " size=""1"" READONLY VALUE=""D"" " & fieldpa & " " & fieldRole(1) & "  class=""dataListDATA""></td>" &vbCrLf
       RESPONSE.Write "<td WIDTH=15% ALIGN=""center""  BGCOLOR=SILVER><input type=""TEXT"" name=""extB" & cnt & "6""" & " size=""10"" READONLY VALUE="""" " & fieldpa & " " & fieldRole(1) & "  class=""dataListDATA""></td>" &vbCrLf
       RESPONSE.Write "<td WIDTH=5% ALIGN=""center""  BGCOLOR=SILVER>&nbsp;</td>" &vbCrLf 
     LOOP

     rs.Close
    %>
    </TR>
    </table>
    </td></tr>
  </table>     
  </DIV>       

    <DIV ID="SRTAG2" onclick="SRTAG2" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="LEFT">備註說明</td></tr></table></DIV>
   <DIV ID="SRTAB2" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
    <TR><TD align="CENTER">
     <TEXTAREA  cols="100%"  name="key10" rows=8  MAXLENGTH=500  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(10)%>" ID="Textarea1"><%=dspkey(124)%></TEXTAREA>
   </td></tr>
 </table> 
  </div> 
  </form>
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
    Dim conn
    Set conn=Server.CreateObject("ADODB.Connection")
    Set RS=Server.CreateObject("ADODB.RECORDSET")
    conn.open DSN
    SQL="DELETE FROM KTSCUSTTELD1 WHERE CUSID='" & DSPKEY(0) & "' AND PNO='" & DSPKEY(1) & "'  "
    'RESPONSE.Write SQL
    CONN.execute SQL
'------ KTSCUSTTELD FOR ADD ---------------------------------------------------
     J=0
     K=0
     for i=13 to 203 step 10
         j=i+1
         Q=I+3
         if len(trim(EXTDB2(I))) > 0 and len(trim(EXTDB2(j))) > 0 then
            K=K+1
            SQL="SELECT * FROM KTSCUSTTELD1"
            RS.OPEN SQL,CONN,3,3
            rs.AddNew
            RS("CUSID")=DSPKEY(0)
            RS("PNO")=DSPKEY(1)
            RS("ENTRYNO")=K
            RS("TEL11")=EXTDB2(I)
            RS("TEL12")=EXTDB2(J)
            RS("AORD")="A"
            IF LEN(TRIM(EXTDB2(Q)))=0 THEN EXTDB2(Q)=NULL
            RS("FINISHDAT")=EXTDB2(Q)
            rs.Update
            RS.CLOSE
        '    SQL="INSERT INTO KTSCUSTTELD1(CUSID,PNO,ENTRYNO,TEL11,TEL12,AORD,FINISHDAT) " _
        '       &"VALUES('" & DSPKEY(0) & "','" & DSPKEY(1) & "'," & K & ",'" & EXTDB2(I) & "','" & EXTDB2(J) & "','A'," & EXTDB2(Q) & ")"
        '    conn.execute SQL
         end if
         if len(trim(EXTDB3(I))) > 0 and len(trim(EXTDB3(j))) > 0 then
            K=K+1
            SQL="SELECT * FROM KTSCUSTTELD1"
            RS.OPEN SQL,CONN,3,3
            rs.AddNew
            RS("CUSID")=DSPKEY(0)
            RS("PNO")=DSPKEY(1)
            RS("ENTRYNO")=K
            RS("TEL11")=EXTDB3(I)
            RS("TEL12")=EXTDB3(J)
            RS("AORD")="D"
            IF LEN(TRIM(EXTDB2(Q)))=0 THEN EXTDB2(Q)=NULL
            RS("FINISHDAT")=EXTDB3(Q)
            rs.Update
            RS.CLOSE
        '    SQL="INSERT INTO KTSCUSTTELD1(CUSID,PNO,ENTRYNO,TEL11,TEL12,AORD,FINISHDAT) " _
        '       &"VALUES('" & DSPKEY(0) & "','" & DSPKEY(1) & "'," & K & ",'" & EXTDB3(I) & "','" & EXTDB3(J) & "','D'," & EXTDB3(Q) & ")"
        '    conn.execute SQL
         END IF
     next
    CONN.Close
    Set conn=Nothing
End Sub
' -------------------------------------------------------------------------------------------- 
' --------------------------------------------------------------------------------------------  
%><!-- #include virtual="/Webap/include/checkid.inc" --><!-- #include virtual="/Webap/include/companyid.inc" --><!-- #include file="RTGetUserRight.inc" --><!-- #include virtual="/Webap/include/employeeref.inc" -->