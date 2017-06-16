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
          sql=sql &"[" &aryKeyNameDB(i) &"]='" &dspKey(i) &"' "
      Else
         sql=sql &"[" &aryKeyNameDB(i) &"]=" &dspKey(i)
      End If
    Next
    GetSql=sqlList &sql &";"
  'response.write getsql
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
              If Instr(cTypeNumeric,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=0
			  If Instr(cTypeChar,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=""
              If Instr(cTypeDate,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=Null
'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
				if i <> 2 then rs.Fields(i).Value=dspKey(i)
				if i=2 then
				 Set rsc=Server.CreateObject("ADODB.Recordset")
				 cusidxx="S" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
				 rsc.open "select max(cusid) AS cusid from RTSonetCust where cusid like '" & cusidxx & "%' " ,conn
				 if len(rsc("cusid")) > 0 then
				    dspkey(2)=cusidxx & right("000" & cstr(cint(right(rsc("cusid"),3)) + 1),3)
				 else
				    dspkey(2)=cusidxx & "001"
				 end if
				 rsc.close
				 rs.Fields(i).Value=dspKey(i) 
				end if
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
              If Instr(cTypeNumeric,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=0
			  If Instr(cTypeChar,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=""
              If Instr(cTypeDate,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=Null
'response.write i&")　<font color=red>"& rs.Fields(i).name & "</font>("& replace(replace(replace(replace(replace(sType,"200","Varchar"),"003","Int"),"135","Datetime"),"002","Smallint"),"202","nVarchar") & ") <font color=green>="& dspkey(i) & "</font><BR>"
'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
              if i<>0 and i <> 1 then rs.Fields(i).Value=dspKey(i)
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
       'runpgm=Request.ServerVariables("PATH_INFO")
          cusidxx="S" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
          rsc.open "select max(cusid) AS cusid from RTSonetCust where cusid like '" & cusidxx & "%' " ,conn
          if not rsC.eof then
            dspkey(2)=rsC("CUSID")
          end if
          rsC.close
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
<meta http-equiv="content-type" content="text/html; charset=big5" />
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
  title="So-net用戶資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  

  sqlFormatDB="SELECT COMQ1, LINEQ1, CUSID, CUSNC, IDNUMBERTYPE, SOCIALID, " &_
			"SECONDIDTYPE, SECONDNO, CUTID2, TOWNSHIP2, RADDR2, CUTID3, " &_
			"TOWNSHIP3, RADDR3, BIRTHDAY, CONTACTTEL, MOBILE, EMAIL, COBOSS, " &_
			"COBOSSID, COCONTACT, COCONTACTTEL, COCONTACTMOBILE, CASEKIND, " &_
			"USERRATE, PAYCYCLE, MEMBERID, FREECODE, IP11, MAC, PORT, " &_
			"USERCABLE, COTOUT, COTIN, GTEQUIP, GTMONEY, GTVALID, GTSERIAL, " &_
			"GTPRTDAT, GTPRTUSR, GTREPAYDAT, APPLYDAT, FINISHDAT, DOCKETDAT, " &_
			"STRBILLINGDAT, OVERDUEDAT, DROPDAT, CANCELDAT, CANCELUSR, MEMO, " &_
			"UUSR, UDAT, ACTIVATEDAT " &_
			"FROM RTSonetCust WHERE COMQ1=0 "
  sqlList=	"SELECT COMQ1, LINEQ1, CUSID, CUSNC, IDNUMBERTYPE, SOCIALID, " &_
			"SECONDIDTYPE, SECONDNO, CUTID2, TOWNSHIP2, RADDR2, CUTID3, " &_
			"TOWNSHIP3, RADDR3, BIRTHDAY, CONTACTTEL, MOBILE, EMAIL, COBOSS, " &_
			"COBOSSID, COCONTACT, COCONTACTTEL, COCONTACTMOBILE, CASEKIND, " &_
			"USERRATE, PAYCYCLE, MEMBERID, FREECODE, IP11, MAC, PORT, " &_
			"USERCABLE, COTOUT, COTIN, GTEQUIP, GTMONEY, GTVALID, GTSERIAL, " &_
			"GTPRTDAT, GTPRTUSR, GTREPAYDAT, APPLYDAT, FINISHDAT, DOCKETDAT, " &_
			"STRBILLINGDAT, OVERDUEDAT, DROPDAT, CANCELDAT, CANCELUSR, MEMO, " &_
			"UUSR, UDAT, ACTIVATEDAT " &_
			"FROM RTSonetCust WHERE "
  userDefineRead="Yes"
  userDefineSave="Yes"
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=5
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
	If len(trim(dspKey(2))) = 0 Then dspkey(2)=""

  '身份證第一碼大寫
  if len(trim(dspkey(5))) >0 then DSPKEY(5)=UCASE(DSPKEY(5)) 

  if len(trim(dspkey(3)))=0 then
       formValid=False
       message="用戶名稱不可空白"
  'elseif len(trim(dspkey(4))) =0 and dspkey(27)<>"Y" then
  '     formValid=False
  '     message="第一證照別不可空白"
  'elseif ( len(trim(dspkey(5)))<>0 and (len(trim(dspkey(5)))<>10 and len(trim(dspkey(5)))<>8 ) ) then
  '     formValid=False
  '     message="用戶身分證(統編)長度錯誤"
  'elseif len(trim(dspkey(8)))=0 then
  '     formValid=False
  '     message="裝機地址(縣市)不可空白"   
  'elseif len(trim(dspkey(9)))=0 and dspkey(8)<>"06" and dspkey(8)<>"15" then
  '     formValid=False
  '     message="裝機地址(鄉鎮)不可空白"    
  'elseif len(trim(dspkey(10)))=0 then
  '     formValid=False
  '     message="裝機地址不可空白"     
  'elseif len(trim(dspkey(11)))=0 and instr(dspkey(13),"郵政")=0 then
  '     formValid=False
  '     message="帳單地址(縣市)不可空白"   
  'elseif len(trim(dspkey(12)))=0 and dspkey(11)<>"06" and dspkey(11)<>"15" and instr(dspkey(13),"郵政")=0 then
  '     formValid=False
  '     message="帳單地址(鄉鎮)不可空白"    
  'elseif len(trim(dspkey(13)))=0 then
  '     formValid=False
  '     message="帳單地址不可空白"          
  'elseif len(trim(dspkey(15)))=0 AND len(trim(dspkey(16)))=0 then
  '     formValid=False
  '     message="用戶聯絡電話或行動電話至少需輸入一項"
  elseif len(trim(dspkey(18)))=0 AND dspkey(4)="02" then
       'formValid=False
       message="企業負責人不可空白"                 
  elseif len(trim(dspkey(19)))=0 AND dspkey(4)="02" then
       'formValid=False
       message="企業負責人身份證字號不可空白"
  elseif ( len(trim(dspkey(19)))=0 or (len(trim(dspkey(19)))<>10 and len(trim(dspkey(19)))<>8 ) ) AND dspkey(4)="02" then
       'formValid=False
       message="企業負責人身分證長度錯誤"
  elseif len(trim(dspkey(20)))=0 AND dspkey(4)="02" then
       'formValid=False
       message="企業連絡人不可空白"
  elseif len(trim(dspkey(21)))=0 and len(trim(dspkey(22)))=0 AND dspkey(4)="02" then
       'formValid=False
       message="企業連絡電話(白天)及行動電話至少須輸入一項"
  'elseif len(trim(dspkey(24)))=0 then
  '     formValid=False
  '     message="用戶速率不可空白"
  'elseif len(trim(dspkey(25)))=0 and dspkey(27) <>"Y" then
  '     formValid=False
  '     message="繳費週期不可空白"
  'elseif len(trim(dspkey(28)))=0 then
  '     formValid=False
  '     message="用戶IP不可空白"
  'elseif len(trim(dspkey(41)))=0 then
  '     formValid=False
  '     message="用戶申請日不可空白"
  end if

  IF formValid=TRUE THEN
    IF dspkey(5) <> "" and (dspkey(4)="01" or dspkey(4)="02") and dspkey(27) <>"Y" then
       idno=dspkey(5)
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
   Set connxx=Server.CreateObject("ADODB.Connection")
   Set rsxx=Server.CreateObject("ADODB.Recordset")
   connxx.open DSN
   sqlxx="select * from RTSonetCmtyLine where comq1=" & aryparmkey(0) & " AND LINEQ1=" & ARYPARMKEY(1)
   rsxx.Open sqlxx,connxx
   if not rsxx.eof then
     IF NOT ISNULL(RSXX("DROPDAT")) OR NOT ISNULL(RSXX("CANCELDAT")) THEN
        formValid=False
        message="主線已作廢或撤銷，不可新增及異動用戶資料" 
     END IF
   end if
   rsxx.close
   connxx.Close   
   set rsxx=Nothing   
   set connxx=Nothing 
 END IF


'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(50)=V(0)
        dspkey(51)=now()
    end if        
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveXScript()%>
   <SCRIPT Language="VBScript">
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
       '押上報竣日則押上新保證金序號
       if clickkey ="KEY43" and document.All("key37").value="" then
	       	<%
			    Dim conn,rs,sql, maxgtserial
			    Set conn=Server.CreateObject("ADODB.Connection")
			    conn.open DSN
			    Set rs=Server.CreateObject("ADODB.Recordset")

				sql ="select isnull(max(gtserial),'') as maxgtserial from RTSonetCust where gtserial<>'' "
    			rs.Open sql,conn,3,3
				 if len(trim(rs("maxgtserial"))) =0 then
					maxgtserial="S0000001"
				 else
				    maxgtserial="S" & right("000000" & cstr(cint(right(rs("maxgtserial"),7)) + 1),7)
				 end if

			    rs.Close
			    conn.Close
			    Set rs=Nothing
			    Set conn=Nothing
	       	%>
			document.All("key37").value = "<%=maxgtserial%>"
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

   Sub SrCountyOnClick()
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       key_cutid="KEY" & cstr(clickid-1)
       key_township="KEY" & clickid
       key_zip=key_township & "zip"
       prog="/Webap/include/RTGetCountyD.asp"
       prog=prog & "?KEY=" & document.all(key_cutid).value
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
	       FUsrID=Split(Fusr,";")   
	       if Fusrid(5) ="Y" then
	          document.all(key_township).value = trim(Fusrid(0))
	          document.all(key_zip).value = "郵遞：" & trim(Fusrid(1))
	       End if
       end if
    END SUB

	Sub SrAddrEqual3()
	   document.All("key11").value=document.All("key8").value
	   document.All("key12").value=document.All("key9").value
	   document.All("key13").value=document.All("key10").value
	End Sub 

	Sub SrRadioOnClick()
		Dim clickName, objRadio
		clickName = window.event.srcElement.Name
		set objRadio = document.All(clickName)
        for i =0 to objRadio.length -1 
			if objRadio(i).checked then
               document.All(replace(ClickName,"R","")).value = objRadio(i).value
			end if
        next
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
       <tr><td width="15%" class=dataListHead>社區序號</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key0"
                 <%=fieldRole(1)%> readonly size="5" value="<%=dspKey(0)%>" maxlength="8" class=dataListdata></td>
           <td width="15%" class=dataListHead>主線序號</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key1"
                 <%=fieldRole(1)%> readonly size="3" value="<%=dspKey(1)%>" maxlength="8" class=dataListdata></td>                 
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
    if dspmode="新增" then
        if len(trim(dspkey(50))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(50)=V(0)
        End if  
       dspkey(51)=now()
    else
        if len(trim(dspkey(50))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(50)=V(0)
        End if         
        dspkey(51)=now()
    end if      
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN

    '業助
	sql ="SELECT * FROM XXLib..UserGroup WHERE [GROUP] in('RTADMIN','業助') and userid ='" &logonid&"' "
    rs.Open sql,conn
    If rs.Eof Then
		basedata=False
    Else
		basedata=true
    End If
    rs.Close

    '用戶報竣後,除業助外資料 protect
	If len(trim(dspKey(43))) > 0 and basedata=false Then
       fieldPm=" class=""dataListData"" readonly "
       fieldpn=" disabled "
    Else
       fieldPm=""
       fieldpn=""
    End If

	'公關戶在報竣後, 業助可改
	If len(trim(dspKey(43))) = 0 then
       fieldPo=""
	elseif len(trim(dspKey(43))) > 0 and basedata=true then
       fieldPo=""		
	else
       fieldPo=" disabled "
	end if

    '用戶未完工,資料 protect
    If isnull(dspKey(42)) or len(trim(dspKey(42)))=0 Then
       fieldPh=" disabled "
    Else
       fieldPh=""
    End If

    '用戶開始繳款後,資料 protect
    If len(trim(dspKey(44))) > 0 Then
       fieldPa=" class=""dataListData"" readonly "
       fieldpb=" disabled "
    Else
       fieldPa=""
       fieldpb=""
    End If

    '用戶退租後,開始計費日資料 protect
    If len(trim(dspKey(46))) > 0 Then
       fieldPa=" class=""dataListData"" readonly "
       fieldpb=" disabled "
       fieldPC=" class=""dataListData"" readonly "
       fieldpD=" disabled "
    Else
       fieldPC=""
       fieldpD=""
    End If

    '用戶作廢後,退租日期資料 protect
    If len(trim(dspKey(47))) > 0 Then
       fieldPa=" class=""dataListData"" readonly "
       fieldpb=" disabled "
       fieldPe=" class=""dataListData"" readonly "
    Else
       fieldPe=""
    End If

    '保證金列印後, 保證金相關資料 protect
    If len(trim(dspKey(38))) > 0  Then
		fieldPg=" class=""dataListData"" readonly "
	Else
		fieldPg=""
	END IF

    %>
  <!--
  <span id="tags1" class="dataListTagsOn"
        onClick="vbscript:tag1.style.display=''    :tags1.classname='dataListTagsOn':
                          tag2.style.display='none':tags2.classname='dataListTagsOf'">基本資料</span>
  <span id="tags2" class="dataListTagsOf"
        onClick="vbscript:tag1.style.display='none':tags1.classname='dataListTagsOf':
                          tag2.style.display=''    :tags2.classname='dataListTagsOn'">發包安裝</span>           
  -->
<span id="tags1" class="dataListTagsOn">So-net用戶資訊</span>
<div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td>
<td>

<DIV ID="SRTAG0" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
		<tr><td bgcolor="BDB76B" align="CENTER">基本資料</td></tr>
    </table>
</div>
<DIV ID=SRTAB0 >
	<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
	<tr><td width="10%" class=dataListHEAD>用戶(公司)名稱</td>
	    <td width="23%"  bgcolor="silver">
	        <input type="text" name="key3" value="<%=dspKey(3)%>" maxlength="30" size="30" style="ime-mode:active;" <%=fieldPm%><%=fieldRole(1)%><%=dataProtect%> class=dataListENTRY>
		</td>

		<%
		    s=""
		    sx=" selected "
		    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
		       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='J5' " 
		       If len(trim(dspkey(4))) < 1 Then
		          sx=" selected " 
		       else
		          sx=""
		       end if     
		    Else
		       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='J5' AND CODE='" & dspkey(4) &"' " 
		       'SXX60=""
		    End If
		    rs.Open sql,conn
		    s=""
		    s=s &"<option value=""" &"""" &sx &">(第一證照別)</option>"
		    sx=""
		    Do While Not rs.Eof
		       If rs("CODE")=dspkey(4) Then sx=" selected "
		       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
		       rs.MoveNext
		       sx=""
		    Loop
		    rs.Close
		%>
		<td width="10%" class=dataListHEAD>身分證(統編)</td>
	    <td width="23%" bgcolor="silver" >
			<select size="1" name="key4"<%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry"><%=s%></select>    
			<input type="text" name="key5" value="<%=dspKey(5)%>" maxlength="10" size="12" style="ime-mode:inactive;text-transform:uppercase;" <%=fieldRole(1)%><%=dataProtect%> class=dataListENTRY>
		</td>

		<%
		    s=""
		    sx=" selected "
		    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
		       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='L3' " 
		       If len(trim(dspkey(6))) < 1 Then
		          sx=" selected " 
		       else
		          sx=""
		       end if     
		    Else
		       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='L3' AND CODE='" & dspkey(6) &"' " 
		       'SXX60=""
		    End If
		    rs.Open sql,conn
		    s=""
		    s=s &"<option value=""" &"""" &sx &">(第二證照別)</option>"
		    sx=""
		    Do While Not rs.Eof
		       If rs("CODE")=dspkey(6) Then sx=" selected "
		       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
		       rs.MoveNext
		       sx=""
		    Loop
		    rs.Close
		%>        
        <td width="10%" class="dataListHead" height="25">第二證照及號碼</td>
        <td width="23%" height="25" bgcolor="silver" > 
			<select size="1" name="key6"<%=fieldpg%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select16"><%=s%></select>	
	        <input type="text" name="key7" size="15" maxlength="12" value="<%=dspkey(7)%>" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text49">
		</td> 
	</tr>

	<tr><td  class="dataListHEAD" height="23">出生日期</td>
        <td  height="23" bgcolor="silver">
	        <input type="text" name="key14" size="10"  value="<%=dspKey(14)%>"  <%=fieldRole(1)%> class="dataListentry" ID="Text14">  
	        <input type="button" id="B14" name="B14" onclick="SrBtnOnClick" height="100%" width="100%"  style="Z-INDEX: 1" value="...."> 
    	    <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" id="C14" name="C14" onclick="SrClear" alt="清除" <%=fieldpb%> style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
        </td>                                 
        <td  class="dataListHEAD" height="23">連絡電話(白天)</td>
        <td  height="23" bgcolor="silver" >
        	<input type="text" name="key15" size="30" maxlength="30" value="<%=dspKey(15)%>" <%=fieldRole(1)%> class="dataListEntry" ID="Text15"></td>
        <td  class="dataListHEAD" height="23">行動電話</td>
        <td  height="23" bgcolor="silver">
        	<input type="text" name="key16" size="30" maxlength="30" value="<%=dspKey(16)%>" <%=fieldRole(1)%> class="dataListEntry" ID="Text16">
		</td>
	</tr>

	<tr><td width="10%" class=dataListHEAD>聯絡Email</td>
		<td width="23%" bgcolor="silver" colspan=5  >
			<input type="text" name="key17" size="30" maxlength="50" value="<%=dspKey(17)%>" <%=fieldRole(1)%> class="dataListEntry" ID="Text17"> 
		</td>
	</tr>

	<tr><td class=dataListHEAD>裝機地址</td>
		<%
			s=""
			sx=" selected "
			If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) and basedata=true Then
				sql="SELECT Cutid,Cutnc FROM RTCounty " 
				If len(trim(dspkey(8))) < 1 Then
				  sx=" selected " 
				else
				  sx=""
				end if     
				s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
				SXX10=" onclick=""SrCountyOnClick()""  "
			Else
				sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(8) & "' " 
				SXX10=""
			End If
			sx=""    
			rs.Open sql,conn
			Do While Not rs.Eof
				If rs("cutid")=dspkey(8) Then sx=" selected "
				s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
				rs.MoveNext
				sx=""
			Loop
			rs.Close
		%>
	    <td bgcolor="silver" COLSPAN=5>
			<select size="1" name="key8" <%=fieldPm%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select8"><%=s%></select>
			<input type="text" name="key9" readonly  size="8" value="<%=dspkey(9)%>" maxlength="10" readonly <%=fieldPm%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text4"><font SIZE=2>(鄉鎮)                 
			<input type="button" id="B9" <%=fieldPn%> name="B9"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX10%>  >        
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldPn%> alt="清除" id="C9"  name="C9"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >
			<input type="text" name="key9zip" size=8 style="color:blue;border:0px;background:transparent;" value="郵遞：<%=SrGetZipName(dspKey(8),dspKey(9))%>" readonly>			
			<input type="text" name="key10"  size="50" value="<%=dspkey(10)%>" maxlength="60"  <%=fieldPm%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListENTRY" ID="Text4">
		</td>
	</tr>

	<tr><td class=dataListHEAD>帳單地址</td>
	  <%s=""
	    sx=" selected "
	    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) and basedata=true Then
	       sql="SELECT Cutid,Cutnc FROM RTCounty " 
	       If len(trim(dspkey(11))) < 1 Then
	          sx=" selected " 
	       else
	          sx=""
	       end if     
	       s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
	       SXX14=" onclick=""SrCountyOnClick()""  "
	    Else
	       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(11) & "' " 
	       SXX14=""
	    End If
	    sx=""    
	    rs.Open sql,conn
	    Do While Not rs.Eof
	       If rs("cutid")=dspkey(11) Then sx=" selected "
	       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
	       rs.MoveNext
	       sx=""
	    Loop
	    rs.Close
	   %>
	    <td bgcolor="silver" COLSPAN=5>
			<select size="1" name="key11" <%=fieldPm%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select11"><%=s%></select>
			<input type="text" name="key12" readonly  size="8" value="<%=dspkey(12)%>" maxlength="10" readonly <%=fieldPm%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Text4"><font SIZE=2>(鄉鎮)                 
			<input type="button" id="B12" <%=fieldPn%> name="B12"   width="100%" style="Z-INDEX: 1"  value="...." <%=SXX14%>  >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldPn%> alt="清除" id="C12"  name="C12"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
			<input type="text" name="key12zip" size=8 style="color:blue;border:0px;background:transparent;" value="郵遞：<%=SrGetZipName(dspKey(11),dspKey(12))%>" readonly>
			<input type="text" name="key13"  size="50" value="<%=dspkey(13)%>" maxlength="60"  <%=fieldPm%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListENTRY" ID="Text4">
			<input type="radio" name="rd2"  <%=fieldpb%> onClick="SrAddrEqual3()"><font SIZE=2>同裝機地址</font>
		</td>
	</tr>

	<TR><td class="dataListHEAD" height="23">企業負責人</td>
		<td height="23" bgcolor="silver">
	        <input type="text" name="key18" size="12" maxlength="12" value="<%=dspKey(18)%>"  <%=fieldRole(1)%> class="dataListEntry" ID="Text18">
		</td>
        <td class="dataListHEAD" height="23">負責人身分證</td>
        <td height="23" bgcolor="silver" colspan=3>
	        <input type="text" name="key19" size="10" maxlength="10" value="<%=dspKey(19)%>"  <%=fieldRole(1)%> class="dataListEntry" ID="Text19">
		</td>
	</tr>

	<tr><td class="dataListHEAD" height="23">企業連絡人</td>
		<td height="23" bgcolor="silver">
			<input type="text" name="key20" size="20" maxlength="20" value="<%=dspKey(20)%>"  <%=fieldRole(1)%> class="dataListEntry" ID="Text20">
		</td>
		<td class="dataListHEAD" height="23">企業連絡人電話</td>
		<td height="23" bgcolor="silver" >
			<input type="text" name="key21" size="30" maxlength="30" value="<%=dspKey(21)%>"  <%=fieldRole(1)%> class="dataListEntry" ID="Text21">
		</td>
		<td class="dataListHEAD" height="23">企業連絡人手機</td>
		<td height="23" bgcolor="silver" >
			<input type="text" name="key22" size="30" maxlength="30" value="<%=dspKey(22)%>"  <%=fieldRole(1)%> class="dataListEntry" ID="Text22">
		</td>
	</tr>
	</table>
</div>

<!--
    <table border="1" width="100%" cellpadding="0" cellspacing="0" id="tag2" style="display: none"> 
-->

<DIV ID="SRTAG3">
	<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
		<tr><td bgcolor="BDB76B" align="CENTER">方案內容</td></tr>
	</table>
</DIV>
<DIV ID="SRTAB3">
	<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3">
	<tr><td  WIDTH="10%" class="dataLISTSEARCH" height="23">公關機</td>
		<td  WIDTH="23%" height="23" bgcolor="silver" colspan=5>
			<%  
				If dspKey(27)="Y" THEN
					FREECODEX=" checked "
					FREECODEY=""
				ELSE
					FREECODEX=""
					FREECODEY=" checked "
				END IF 
			%>
	        <input type="radio" name="key27R" value="Y" onClick="SrRadioOnClick" <%=FREECODEX%> <%=fieldPo%> <%=fieldRole(1)%><%=dataProtect%> >是　
	        <input type="radio" name="key27R" value="" onClick="SrRadioOnClick" <%=FREECODEY%> <%=fieldPo%> <%=fieldRole(1)%><%=dataProtect%> >否
	        <input type="text" name="key27" size="5" value="<%=dspKey(27)%>" Style="display:none;" ID="Text27">
		</td> 
			<input type="text" name="key23" size="2" value="<%=dspKey(23)%>" Style="display:none;" ID="Text23">
	</tr>
	
	<tr><td width="10%" class=dataListHEAD>用戶速率</td>
		<%
		    s=""
		    sx=" selected "
		    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) Then 
		       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='R3' " 
		       If len(trim(dspkey(24))) < 1 Then
		          sx=" selected " 
		       else
		          sx=""
		       end if     
		    Else
		       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='R3' AND CODE='" & dspkey(24) &"' " 
		       'SXX60=""
		    End If
		    rs.Open sql,conn
		    s=""
		    s=s &"<option value=""" &"""" &sx &"></option>"
		    sx=""
		    Do While Not rs.Eof
		       If rs("CODE")=dspkey(24) Then sx=" selected "
		       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
		       rs.MoveNext
		       sx=""
		    Loop
		    rs.Close
	   	%>
    	<td width="23%" bgcolor="silver">
        	<select size="1" name="key24" class="dataListEntry" ID="Select24" ><%=s%></select>
     	</td>

  		<td WIDTH="10%" class="dataListHEAD" height="23">繳費週期</td>
		<%
		    s=""
		    sx=" selected "
		    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  and len(trim(dspkey(43)))=0 Then  
		       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M8' and code in ('02','03','06') " 
		       If len(trim(dspkey(25))) < 1 Then
		          sx=" selected " 
		          s=s & "<option value=""""" & sx & "></option>"  
		          sx=""
		       else
		          s=s & "<option value=""""" & sx & "></option>"  
		          sx=""
		       end if     
		    Else
		       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M8' AND CODE='" & dspkey(25) & "'"
		    End If
		    rs.Open sql,conn
		    Do While Not rs.Eof
		       If rs("CODE")=dspkey(25) Then sx=" selected "
		       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
		       rs.MoveNext
		       sx=""
		    Loop
		    rs.Close
		%>
        <td WIDTH="23%" height="23" bgcolor="silver" >
		   <select size="1" name="key25" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select25"><%=s%></select>
        </td>

		<td WIDTH="10%" class="dataListHEAD" height="23">So-net會員帳號</td>
		<td WIDTH="23%" bgcolor="silver" >
			<input type="text" name="key26" size="18" maxlength="16" value="<%=dspKey(26)%>" <%=fieldpa%> <%=fieldRole(1)%> class="dataListEntry" ID="Text26">
		</td>
	</tr>
	</table>
</DIV>

<DIV ID="SRTAG1">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="CENTER">保證金相關</td></tr></table>
</div>
    
<DIV ID="SRTAB1">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table7">
 	<tr><td width="10%" class="dataListHead">保證金序號</td>
        <td width="23%" bgcolor="silver">
			 <input type="text" name="key37" size="13" value="<%=dspKey(37)%>"  class="dataListEntry">
		</td>

        <td width="10%" class="dataListHead">保證金</td>
        <td width="23%" bgcolor="silver">
			<input type="text" name="key35" value="<%=dspKey(35)%>" size="10" maxlength="10" <%=fieldRole(1)%><%=fieldPg%><%=dataProtect%> class="dataListEntry">
		</td>

        <td width="10%" class="dataListHead">用戶保管CPE</td>
		<%
		    s=""
		    sx=" selected "
		    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) Then
		       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='P2' " 
		       If len(trim(dspkey(34))) < 1 Then
		          sx=" selected " 
		       else
		          sx=""
		       end if     
		    Else
		       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='P2' AND CODE='" & dspkey(34) &"' " 
		    End If
		    rs.Open sql,conn
		    s=""
		    s=s &"<option value=""" &"""" &sx &"></option>"
		    sx=""
		    Do While Not rs.Eof
		       If rs("CODE")=dspkey(34) Then sx=" selected "
		       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
		       rs.MoveNext
		       sx=""
		    Loop
		    rs.Close
		%>
		<td width="23%" bgcolor="silver" >
			<select name="key34" <%=fieldPg%><%=fieldRole(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select1"><%=s%></select>
		</td>
	</tr>

	<tr><td  class="dataListHEAD" height="23">保證金收據列印人</td>
		<td height="23" bgcolor="silver">
			<input type="text" name="key39" size="10" READONLY value="<%=dspKey(39)%>" class="dataListDATA" ID="Text1">
			<font size=2>
			<%
				if trim(len(dspKey(39))) =6 then
					response.Write SrGetEmployeeName(dspKey(39))
				else 
					sql="SELECT shortnc FROM RTObj where cusid ='" &dspKey(39)&"' "
					rs.Open sql,conn
					Do While Not rs.Eof
						response.Write rs("shortnc")
						rs.MoveNext
					Loop
					rs.Close
				end if
			%>
			</font>
		</td>
			
        <td  class="dataListHEAD" height="23">保證金收據列印日</td>
        <td  height="23" bgcolor="silver">
			<input type="text" name="key38" size="25" READONLY value="<%=dspKey(38)%>" <%=fieldPg%><%=fieldRole(1)%> class="dataListDATA">
        </td>

		<td WIDTH="10%" class="dataListHEAD" height="23">保證金退還日</td>
        <td WIDTH="23%" height="23" bgcolor="silver">
			<input type="text" name="key40" size="12" READONLY value="<%=dspKey(40)%>" <%=fieldpC%> <%=fieldRole(1)%> class="dataListentry" ID="Text56" >
			<input type="button" id="B40"  name="B40" height="100%" width="100%" <%=fieldpD%> style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C40" name="C40" <%=fieldpD%> style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
		</td
	  </tr>
  	</table>
</DIV>

<DIV ID="SRTAG2">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
    	<tr><td bgcolor="BDB76B" align="CENTER">網路資訊</td></tr>
	</table>
</DIV>
<DIV ID="SRTAB2">
	<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3">
    <tr>
		<td  WIDTH="10%"  class="dataListHEAD" height="23">用戶IP</td>
        <td  WIDTH="23%" height="23" bgcolor="silver" >
			  <input type="text" name="key28" size="15" maxlength="15" value="<%=dspKey(28)%>" <%=fieldRole(1)%><%=fieldPm%><%=dataProtect%> class="dataListentry">
        </TD>
		<td  WIDTH="10%"  class="dataListHEAD" height="23">用戶CPE Mac</td>
        <td  WIDTH="23%" height="23" bgcolor="silver">
			<input type="text" name="key29" size="12" maxlength="12" value="<%=ucase(dspKey(29))%>" <%=fieldRole(1)%> class="dataListEntry">
        </TD>
		<td  WIDTH="10%"  class="dataListHEAD" height="23">用戶連絡纜</td>
        <td  WIDTH="23%" height="23" bgcolor="silver">
			<input type="text" name="key31" size="20" maxlength="20" value="<%=ucase(dspKey(31))%>" <%=fieldRole(1)%> class="dataListEntry">
        </TD>
    </TR>

	<tr><td  WIDTH="10%"  class="dataListHEAD" height="23">COT Out</td>
        <td  WIDTH="23%" height="23" bgcolor="silver" >
			<input type="text" name="key32" size="10" maxlength="10" value="<%=dspKey(32)%>" <%=fieldRole(1)%> class="dataListEntry">
        </td>
		<td  WIDTH="10%"  class="dataListHEAD" height="23">COT In</td>
        <td  WIDTH="23%" height="23" bgcolor="silver">
			<input type="text" name="key33" size="10" maxlength="10" value="<%=dspKey(33)%>" <%=fieldRole(1)%> class="dataListEntry">
        </td>
		<td  WIDTH="10%"  class="dataListHEAD" height="23">MDF</td>
	    <td  WIDTH="23%" height="23" bgcolor="silver" >
			<input type="text" name="key30" size="10" maxlength="10" value="<%=dspKey(30)%>" <%=fieldRole(1)%> class="dataListEntry">
	    </td>
	</tr>
	</TABLE>
</DIV>

<DIV ID="SRTAG4">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr><td bgcolor="BDB76B" align="CENTER">用戶申請、異動及施工進度狀態</td></tr></table>
</DIV>
<DIV ID="SRTAB4">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
	<tr><td width="10%" class=dataListHEAD>用戶申請日</td>
	    <td width="23%" bgcolor="silver"  >
	        <input type="text" name="key41" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
	               style="text-align:left;" maxlength="10"
	               value="<%=dspKey(41)%>"  READONLY size="10" class=dataListEntry>
			<input  type="button" id="B41"  <%=fieldpb%> name="B41" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
	    	<IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C41"  name="C41"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
		</td>
		<td  WIDTH="10%" class="dataListHEAD" height="23">完工日期</td>
        <td  WIDTH="23%" height="23" bgcolor="silver" >
			<input type="text" name="key42" size="12" READONLY value="<%=dspKey(42)%>" class="dataListdata" ID="Text57">
        </td>
        <td  WIDTH="10%" class="dataListHEAD" height="23">報竣日期</td>
        <td  WIDTH="23%" height="23" bgcolor="silver">
	        <input type="text" name="key43" size="12" READONLY value="<%=dspKey(43)%>" <%=fieldPo%> <%=fieldRole(1)%> class="dataListentry" ID="Text43">
			<input type="button" height="100%" width="100%" id="B43" name="B43" onclick="SrBtnOnClick" <%=fieldPo%> style="Z-INDEX: 1" value="....">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C43" name="C43" onclick="SrClear" <%=fieldPo%> style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
        </td>
	</TR>
	<tr><td  WIDTH="10%" class="dataListHEAD" height="23">So-net啟用日</td>
        <td  WIDTH="23%" height="23" bgcolor="silver" >
			<input type="text" name="key52" size="12" READONLY value="<%=dspKey(52)%>" class="dataListData" ID="Text52" >
		</td>
		<td  WIDTH="10%" class="dataListHEAD" height="23">So-net首次繳款日</td>
        <td  WIDTH="23%" height="23" bgcolor="silver" colspan="3">
			<input type="text" name="key44" size="12" READONLY value="<%=dspKey(44)%>" class="dataListData" ID="Text44" >
		</td>
	</TR>
	<tr><td  WIDTH="10%" class="dataListHEAD" height="23">欠拆日</td>
        <td  WIDTH="23%" height="23" bgcolor="silver">
			<input type="text" name="key45" size="12" READONLY value="<%=dspKey(45)%>"  <%=fieldRole(1)%> class="dataListDATA" ID="Text3">
			<!-- <input type="button" id="B45"  name="B45" height="100%" width="100%" <%=fieldpD%>style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C45" name="C45" <%=fieldpD%>style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">     
			-->
        </TD>
		<td WIDTH="10%"  class="dataListHEAD" height="23">退租日</td>
		<td WIDTH="23%"  height="23" bgcolor="silver" colspan="3">
			<input type="text" name="key46" size="12" READONLY value="<%=dspKey(46)%>" <%=fieldpe%> <%=fieldRole(1)%> class="dataListdata" ID="Text6">
			<!--  
			<input type="button" id="B46"  name="B46" height="100%" width="100%" <%=fieldpf%>style="Z-INDEX: 1" value="...." onclick="SrBtn33OnClick">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C46"  name="C46"   <%=fieldpf%>style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="Sr33Clear">     
			-->
       </td>
	</TR>
	<tr><td class="dataListHEAD" height="23">作廢人員</td>
		<td height="23" bgcolor="silver">
			<input type="text" name="key48" size="6" value="<%=dspKey(48)%>" <%=fieldRole(1)%> readonly class="dataListDATA" ID="Text48">
			<font size=2><%=SrGetEmployeeName(dspKey(48))%></font>
		</td>
	 	<td class="dataListHEAD" height="23">作廢日期</td>
        <td height="23" bgcolor="silver" colspan=3>
			<input type="text" name="key47" size="25" value="<%=dspKey(47)%>"  <%=fieldpa%><%=fieldRole(1)%> readonly class="dataListdata" ID="Text47">
		</td>
	</tr>
	<tr><td  class="dataListHEAD" height="23">修改人員</td>
		<td  height="23" bgcolor="silver">
			<input type="text" name="key50" size="6" READONLY value="<%=dspKey(50)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text51">
			<font size=2><%=SrGetEmployeeName(dspKey(50))%></font>
		</td>
		<td class="dataListHEAD" height="23">修改日期</td>
		<td height="23" bgcolor="silver" colspan=3>
			<input type="text" name="key51" size="25" READONLY value="<%=dspKey(51)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text50">
		</td>
	</tr>
	</table>
</DIV>

<DIV ID="SRTAG5">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="CENTER">備註說明</td></tr></table>
</DIV>
<DIV ID="SRTAB5" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
    <TR><TD align="CENTER">
		<TEXTAREA cols="100%" name="key49" value="<%=dspkey(49)%>" rows=8 MAXLENGTH=500 ID="Textarea49" <%=dataprotect%> class="dataListentry"><%=dspkey(49)%></TEXTAREA>
	</td></tr>
 	</table>
</div>

  <DIV ID="SRTAG6">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr><td bgcolor="BDB76B" align="CENTER">So-net繳款記錄</td></tr></table>
</DIV>
    <DIV ID="SRTAB6" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr class="dataListHEAD" align=center><td>社區名</td><td>用戶名</td><td>方案</td><td>速率</td><td>週期</td><td>繳款金額</td><td>繳款日</td><td>啟用日</td></tr>
    <%
           Set rsxx=Server.CreateObject("ADODB.Recordset")
           sqlfaqlist=	"select	community_name, name, promo_name, speed, paycycle, payment_amount, " &_
           				"convert(datetime, payment_date) as payment_date, convert(datetime, activate_date) as activate_date " &_
						"from	RTSonetCustArSrc " &_
                     	"WHERE	cusid ='"& DSPKEY(2) &"' " &_
                     	"order by payment_date "
           rsxx.open sqlfaqlist,conn
           do until rsxx.eof 
	%>
		<tr class="dataListentry">
           <td><%=rsxx("community_name")%>&nbsp;</td>
           <td><%=rsxx("name")%>&nbsp;</td>
           <td><%=rsxx("promo_name")%>&nbsp;</td>
           <td><%=rsxx("speed")%>&nbsp;</td>
           <td><%=rsxx("paycycle")%>&nbsp;</td>
           <td><%=rsxx("payment_amount")%>&nbsp;</td>
           <td><%=rsxx("payment_date")%>&nbsp;</td>
           <td><%=rsxx("activate_date")%>&nbsp;</td>
		</tr>
	<%
		   rsxx.MoveNext
           loop    
           rsxx.close
           set rsxx=nothing
	%>
	</table>
  </div>

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
End Sub
' -------------------------------------------------------------------------------------------- 
' --------------------------------------------------------------------------------------------  
%>
<!-- #include virtual="/Webap/include/checkid.inc" -->
<!-- #include virtual="/Webap/include/companyid.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserRight.asp" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->