<%  
  Dim fieldRole,fieldPa
  fieldRole=Split(FrGetUserRight("RTCustD",Request.ServerVariables("LOGON_USER")),";")
%>
<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/cType.inc" -->
<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/dataList.inc" -->
<%
  Dim aryKeyName,aryKeyType(200),aryKeyValue(200),numberOfField,aryKey,aryKeyNameDB(200)
  Dim dspKey(200),userDefineKey,userDefineData,extDBField,extDB(200),userDefineRead,userDefineSave
  Dim conn,rs,i,formatName,sqlList,sqlFormatDB,userdefineactivex
  Dim aryParmKey
 '90/09/03 ADD-START
 '增加EXTDBFIELD2,EXTDBFILELD3(多檔維護)
  dim extDBField2,extDB2(200),extDBField3,extDB3(200),extDBField4,extDB4(200)
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
               if i <> 1 then rs.Fields(i).Value=dspKey(i)
               if i=1 then
                 Set rsc=Server.CreateObject("ADODB.Recordset")
                 rsc.open "select max(lineq1) AS lineq1 from RTfareastCmtyLine where comq1=" & dspkey(0) ,conn
                 if len(rsc("lineq1")) > 0 then
                    dspkey(1)=rsc("lineq1") + 1
                 else
                    dspkey(1)=1
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
'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
             if i<>0 and i<>1 then rs.Fields(i).Value=dspKey(i)
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
          rs.open "select max(lineq1) AS lineq1 from RTfareastCmtyLine where comq1=" & dspkey(0) ,conn
          if not rs.eof then
            dspkey(1)=rs("lineq1")
          end if
          rs.close
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
  numberOfKey=2
  title="遠傳大寬頻社區型主線資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB=	"SELECT COMQ1, LINEQ1, CUTID, TOWNSHIP, RADDR, SUPPLYRANGE, ADDROTHER, " &_
				"LINEISP, LINEIPTYPE, LINERATE, LINETEL, PPPOEACCOUNT, " &_
				"PPPOEPASSWORD, LINEIP, GATEWAY, SUBNET, DNSIP, INSPECTDAT, AGREE, " &_
				"UNAGREEREASON, APPLYDAT, APPLYNAME, APPLYSOCIAL, " &_
				"APPLYCONTACTTEL, APPLYMOBILE, HARDWAREDAT, DROPDAT, DROPKIND, " &_
				"CANCELDAT, CANCELUSR, MEMO, UUSR, UDAT, TELEADDR " &_
				"FROM RTfareastCmtyLine WHERE COMQ1=0 "
  sqlList=		"SELECT COMQ1, LINEQ1, CUTID, TOWNSHIP, RADDR, SUPPLYRANGE, ADDROTHER, " &_
				"LINEISP, LINEIPTYPE, LINERATE, LINETEL, PPPOEACCOUNT, " &_
				"PPPOEPASSWORD, LINEIP, GATEWAY, SUBNET, DNSIP, INSPECTDAT, AGREE, " &_
				"UNAGREEREASON, APPLYDAT, APPLYNAME, APPLYSOCIAL, " &_
				"APPLYCONTACTTEL, APPLYMOBILE, HARDWAREDAT, DROPDAT, DROPKIND, " &_
				"CANCELDAT, CANCELUSR, MEMO, UUSR, UDAT, TELEADDR " &_
				"FROM RTfareastCmtyLine WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=1
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    If len(trim(dspKey(0))) = 0 Then dspkey(0)=0
    If len(trim(dspKey(1))) = 0 Then dspkey(1)=0

    if len(trim(dspkey(2)))=0 then
       formValid=False
       message="地址(縣市)不可空白"
    elseif len(trim(dspkey(3)))=0 and dspkey(2)<>"06" and dspkey(2)<>"15" then
       formValid=False
       message="地址(鄉鎮)不可空白"
    elseif len(trim(dspkey(4)))=0 then
       formValid=False
       message="地址(門牌)不可空白"
    elseif len(trim(dspkey(9)))=0 then
       formValid=False
       message="主線速率不可空白"    
'    elseif len(trim(DSPKEY(60)))<> 0 AND  len(trim(DSPKEY(60)))<> 10 AND  len(trim(DSPKEY(60)))<> 8 THEN
'       formValid=False
'       message="身份證(統編)字號長度不足(必須為10碼或8碼)"                             
    elseif LEN(TRIM(DSPKEY(17)))=0 THEN
       formValid=False
       message="請輸入勘查日期"                                                                                                                                                            
    elseif DSPKEY(19)="N" AND LEN(TRIM(DSPKEY(19)))=0 THEN
       formValid=False
       message="勘察為不可建置時必須輸入原因"  
    elseif len(trim(DSPKEY(20))) > 0 and dspkey(18)<>"Y" THEN
       formValid=False
       message="主線申請必須為勘查為[可建置]狀態"
    elseif len(trim(DSPKEY(20))) > 0 and len(trim(dspkey(7)))=0 THEN
       formValid=False
       message="主線線路申請時，線路ISP不可空白"
    elseif len(trim(DSPKEY(20))) > 0 and len(trim(dspkey(8)))=0 THEN
       formValid=False
       message="主線線路申請時，線路IP種類不可空白"
    elseif len(trim(DSPKEY(20))) > 0 and len(trim(dspkey(9)))=0 THEN
       formValid=False
       message="主線線路申請時，線路速率不可空白"
    elseif len(trim(DSPKEY(25))) > 0 and len(trim(dspkey(10)))=0 THEN
       formValid=False
       message="主線到位時，主線[附掛電話]不可空白"
    elseif len(trim(DSPKEY(25))) > 0 and len(trim(dspkey(14)))=0 THEN
       'formValid=False
       message="主線到位時，(主線網路IP)不可空白"
    elseif len(trim(DSPKEY(25))) > 0 and len(trim(dspkey(14)))=0 THEN
       'formValid=False
       message="主線到位時，(主線網路Gateway IP)不可空白"     
    elseif len(trim(DSPKEY(25))) > 0 and dspkey(8)="02" AND ( len(trim(DSPKEY(11)))=0 OR len(trim(DSPKEY(12)))=0 ) THEN
       formValid=False
       message="主線線路種類為PPPoE時，PPPoE帳號及密碼不可空白"            
    end if
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(31)=V(0)
        dspkey(32)=now()
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
   END SUB
   Sub SrClear()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="C" & clickid
       clearkey="key" & clickid       
       if len(trim(document.all(clearkey).value)) <> 0 then
          document.all(clearkey).value =  ""
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
       <tr><td width="20%" class=dataListHead>社區序號</td><td width="25%"  bgcolor="silver">
           <input type="text" name="key0"
                 <%=fieldRole(1)%> readonly size="10" value="<%=dspKey(0)%>" maxlength="8" class=dataListdata></td>
<td width="20%" class=dataListHead>主線序號</td><td width="25%"  bgcolor="silver">
           <input type="text" name="key1"
                 <%=fieldRole(1)%> readonly size="10" value="<%=dspKey(1)%>" maxlength="8" class=dataListdata></td>                 </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(31))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(31)=V(0)
        End if  
       dspkey(32)=now()
    else
        if len(trim(dspkey(31))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(31)=V(0)
        End if         
        dspkey(32)=now()
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

    '控制點DSPKEY(20)主線申請, 基本資料 protect
    If len(trim(dspKey(20))) > 0 Then
       fieldPa=" class=""dataListData"" readonly "
       FIELDPC=" DISABLED "
    Else
       fieldPa=""
       FIELDPC=""
    End If

	' dspKey(25)主線到位
    If len(trim(dspKey(25))) > 0 and basedata=false Then
       fieldPB=" class=""dataListData"" readonly "
       FIELDPD=" DISABLED "
    Else
       fieldPB=""
       FIELDPD=""
    End If    
      
    %>
  <!--
  <span id="tags1" class="dataListTagsOn"
        onClick="vbscript:tag1.style.display=''    :tags1.classname='dataListTagsOn':
                          tag2.style.display='none':tags2.classname='dataListTagsOf'">基本資料</span>
  <span id="tags2" class="dataListTagsOf"
        onClick="vbscript:tag1.style.display='none':tags1.classname='dataListTagsOf':
                          tag2.style.display=''    :tags2.classname='dataListTagsOn'">發包安裝</span>           
  -->
  <span id="tags1" class="dataListTagsOn">遠傳大寬頻社區型主線資訊</span>
                                                            
  <div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td><td>

<DIV ID="SRTAG0">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    	<tr><td bgcolor="BDB76B" align="CENTER">基本資料</td></tr>
    </table>
</div>
<DIV ID=SRTAB0 >
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
	<tr><td class=dataListHead>社區名稱</td>
		<% 	sql="SELECT comn FROM RTfareastCmtyH where comq1=" & dspkey(0)
			rs.Open sql,conn
			colComn = rs("comn")
			rs.Close
		%>	
	    <td bgcolor="silver" colspan=3>
	        <input type="text" name="keyComn" size=60 style="color:blue;border:0px;background:transparent;" value="<%=colComn%>" readonly>
		</td>
	</tr>

	<tr><td class=dataListHEAD>主線地址</td>
	  <%s=""
	    sx=" selected "
	    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) and len(dspkey(20))=0 Then 
	       sql="SELECT Cutid,Cutnc FROM RTCounty " 
	       If len(trim(dspkey(2))) < 1 Then
	          sx=" selected " 
	       else
	          sx=""
	       end if     
	       s=s &"<option value=""" &"""" &sx &">(縣市別)</option>"       
	       SXX3=" onclick=""SrCountyOnClick()""  "
	    Else
	       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(2) & "' " 
	       SXX3=""
	    End If
	    sx=""    
	    rs.Open sql,conn
	    Do While Not rs.Eof
	       If rs("cutid")=dspkey(2) Then sx=" selected "
	       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
	       rs.MoveNext
	       sx=""
	    Loop
	    rs.Close
	   %>
	    <td bgcolor="silver" COLSPAN=3>
			<select size="1" name="key2"<%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select2"><%=s%></select>
			<input type="text" name="key3" size="8" value="<%=dspkey(3)%>" maxlength="10" readonly <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text4">
			<input type="button" id="B3" name="B3" width="100%" style="Z-INDEX: 1" <%=fieldpc%> value="..." <%=SXX3%>  >        
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C3" name="C3" style="Z-INDEX: 1" <%=fieldpc%> border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
			<input type="text" name="key3zip" size=8 style="color:blue;border:0px;background:transparent;" value="郵遞：<%=SrGetZipName(dspKey(2),dspKey(3))%>" readonly>
			<input type="text" name="key4" size="40" value="<%=dspkey(4)%>" maxlength="60" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text5">
		</td>
	</tr>

	<tr><td class=dataListHead>可供裝範圍</td>
	    <td bgcolor="silver" colspan=3>
	        <input type="text" name="key5" size="80" value="<%=dspkey(5)%>" maxlength="100" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text11">
		</td>
	</tr>
	<tr><td class=dataListHead>設備位置</td>
	    <td bgcolor="silver">
	        <input type="text" name="key6" size="40" value="<%=dspkey(6)%>" maxlength="40" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text11">
		</td>
		<td class=dataListHead>電信室(箱)位置</td>
	    <td bgcolor="silver">
	        <input type="text" name="key33" size="40" value="<%=dspkey(33)%>" maxlength="40" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text11">
		</td>	
	</tr>

	<tr><td width="15%" class=dataListHEAD>申請人姓名</td>
	    <td width="35%" bgcolor="silver" >
	    	<% if len(trim(dspKey(21)))=0 and accessmode="A" then dspkey(21)="遠傳大寬頻" %>
	        <input type="text" maxlength="30" size="30" name="key21" value="<%=dspKey(21)%>" <%=fieldpb%><%=fieldRole(1)%><%=dataProtect%> class=dataListEntry>
	    </td>
	    <td width="15%" class=dataListHEAD>申請人身份證(統編)</td>
	    <td width="35%" bgcolor="silver" >
		    <% if len(trim(dspKey(22)))=0 and accessmode="A" then dspkey(22)="70811495" %>
		    <input type="text" maxlength="10" size="10" name="key22" value="<%=dspKey(22)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListEntry" ID="Text46">
	    </td>        
	</tr>
	<tr><td width="15%" class=dataListHEAD>申請人連絡電話</td>
	    <td width="35%" bgcolor="silver" >
	        <input type="text" maxlength="15" size="15" name="key23" value="<%=dspKey(23)%>" <%=fieldpb%><%=fieldRole(1)%><%=dataProtect%> class=dataListEntry>
	    </td>
	    <td width="15%" class=dataListHEAD>申請人行動電話</td>
	    <td width="35%" bgcolor="silver" >
	    	<input type="text" maxlength="10" size="10" name="key24" value="<%=dspKey(24)%>" <%=fieldpb%><%=fieldRole(1)%> class="dataListEntry" ID="Text46">
	    </td>
	</tr>
</table> 
</div>

<DIV ID="SRTAG3">
	<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table4">
		<tr><td bgcolor="BDB76B" align="CENTER">網路資料</td></tr>
	</table>
</DIV>
<DIV ID=SRTAB3 > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table5">
	<tr><td WIDTH="15%" class="dataListHEAD" height="23">線路ISP</td>
		<%  s=""
		    sx=" selected "
		    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) and basedata=true Then 
		       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='C3' " 
		       If len(trim(dspkey(7))) < 1 Then
		          sx=" selected " 
		          s=s & "<option value=""""" & sx & ">(線路ISP)</option>"  
		          sx=""
		       else
		          s=s & "<option value=""""" & sx & ">(線路ISP)</option>"  
		          sx=""
		       end if     
		    Else
		       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='C3' AND CODE='" & dspkey(7) & "'"
		    End If
		    rs.Open sql,conn
		    Do While Not rs.Eof
		       If rs("CODE")=dspkey(7) Then sx=" selected "
		       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
		       rs.MoveNext
		       sx=""
		    Loop
		    rs.Close
	    %>
        <td WIDTH="35%" height="23" bgcolor="silver" >
		   	<select size="1" name="key7" <%=fieldpb%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35"><%=s%></select>
        </td>

        <td  WIDTH="15%" class="dataListHEAD" height="23">線路IP種類</td>
		<%  s=""
		    sx=" selected "
		    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) and basedata=true Then
		       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M5' " 
		       If len(trim(dspkey(8))) < 1 Then
		          sx=" selected " 
		          s=s & "<option value=""""" & sx & ">(線路IP種類)</option>"  
		          sx=""
		       else
		          s=s & "<option value=""""" & sx & ">(線路IP種類)</option>"  
		          sx=""
		       end if     
		    Else
		       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M5' AND CODE='" & dspkey(8) & "'"
		    End If
		    rs.Open sql,conn
		    Do While Not rs.Eof
		       If rs("CODE")=dspkey(8) Then sx=" selected "
		       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
		       rs.MoveNext
		       sx=""
		    Loop
		    rs.Close
		%>
        <td  WIDTH="35%" height="23" bgcolor="silver">
		   <select size="1" name="key8" <%=fieldpb%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35"><%=s%></select>
		</td>
	</tr>

	<tr><td WIDTH="15%" class="dataListHEAD" height="23">主線速率</td>
		<%  s=""
		    sx=" selected "
		    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) and basedata=true Then 
		       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D3' " 
		       If len(trim(dspkey(9))) < 1 Then
		          sx=" selected " 
		          s=s & "<option value=""""" & sx & ">(主線速率)</option>"  
		          sx=""
		       else
		          s=s & "<option value=""""" & sx & ">(主線速率)</option>"  
		          sx=""
		       end if     
		    Else
		       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D3' AND CODE='" & dspkey(9) & "'"
		    End If
		    rs.Open sql,conn
		    Do While Not rs.Eof
		       If rs("CODE")=dspkey(9) Then sx=" selected "
		       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
		       rs.MoveNext
		       sx=""
		    Loop
		    rs.Close
	    %>
		<td WIDTH="35%" height="23" bgcolor="silver" >
	   		<select size="1" name="key9" <%=fieldpb%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35"><%=s%></select>
		</td>

        <td  WIDTH="15%" class="dataListHEAD" height="23">附掛電話(專線編號)</td>
        <td  WIDTH="35%" height="23" bgcolor="silver">
    	    <input type="text" name="key10" size="15" maxlength="15" value="<%=dspKey(10)%>" <%=fieldpB%><%=fieldRole(1)%> class="dataListEntry" ID="Text43">
		</td>
	</tr>

	<tr><td WIDTH="15%" class="dataListHEAD" height="23">主線網路IP</td>
        <td WIDTH="35%" height="23" bgcolor="silver">
        	<input type="text" name="key13" size="20"  maxlength="20" value="<%=dspKey(13)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListEntry" ID="Text37">
        </td>

     	<td WIDTH="15%" class="dataListHEAD" height="23">主線SUBNET</td>
		<%	s=""
			aryOption=Array("","255.255.255.0","255.255.255.128")
			If Len(Trim(fieldRole(1) &dataProtect)) < 1  AND FIELDPB = "" Then 
			  For i = 0 To Ubound(aryOption)
			      If dspKey(15)=aryOption(i) Then
			         sx=" selected "
			      Else
			         sx=""
			      End If
			      s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
			  Next
			Else
			  s="<option value=""" &dspKey(15) &""">" &dspKey(15) &"</option>"
			End If
		%>                                 
	    <td width="35%" height="23" bgcolor="silver">
		    <select size="1" name="key15" <%=fieldpB%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Select14"><%=s%></select>
		</td>
 	</tr>

	<tr><td  class="dataListHEAD" height="23">閘道IP(Gateway)</td>
        <td  height="23" bgcolor="silver">
	        <input type="text" name="key14" size="20"   maxlength="20" value="<%=dspKey(14)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListEntry" ID="Text38">
        </td>

        <td  class="dataListHEAD" height="23">DNS IP</td>
        <td  height="23" bgcolor="silver">
	        <input type="text" name="key16" size="20" maxlength="20" value="<%=dspKey(16)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListEntry" ID="Text40">
		</td>
 	</tr>

	<tr><td  class="dataListHEAD" height="23">PPPoE撥接帳號</td>
        <td  height="23" bgcolor="silver">
        	<input type="text" size="30" maxlength="30" name="key11" value="<%=dspKey(11)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListEntry" ID="Text41">
		</td>

        <td  class="dataListHEAD" height="23">PPPOE撥接密碼</td>
        <td  height="23" bgcolor="silver">
        	<input type="text" size="10" maxlength="10" name="key12" value="<%=dspKey(12)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListEntry" ID="Text42">
		</td>
	</tr>
	</table>
</DIV>

<DIV ID="SRTAG4" onclick="srtag4" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    	<tr><td bgcolor="BDB76B" align="CENTER">主線施工進度狀態</td></tr>
    </table>
</DIV>

<DIV ID=SRTAB4 >  
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
	<tr><td class=dataListHead>主線勘察日</td>
	<td bgcolor="silver" colspan=3>
		<input type="text" maxlength="10" size="10" name="key17" size="17" READONLY value="<%=dspKey(17)%>" <%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Text7">
	   <input type="button" id="B17" name="B17" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
	   <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C17"  name="C17"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>
	</tr>

	<tr><td class=dataListHead>是否可建置</td>
	    <%  dim rdo11, rdo12
		      If Len(Trim(fieldRole(1) &dataProtect)) < 1 Then
		         rdo11=""
		         rdo12=""
		      Else
		         rdo11=" disabled "
		         rdo12=" disabled "
		      End If
		      If trim(dspKey(18))="Y" Then 
		         rdo11=" checked "    
		      elseIf trim(dspKey(18))="N" Then 
		         rdo12=" checked " 
		      elseif trim(dspkey(18))="" then
		         dspkey(18)=""
		      end if
		%>
	    <td bgcolor="silver" colspan=3>
	        <input type="radio" value="Y" <%=rdo11%> name="key18" <%=fieldRole(1)%><%=dataProtect%> ID="Radio1"><font size=2>可建</font>
	        <input type="radio" value="N" <%=rdo12%>  name="key18" <%=fieldRole(1)%><%=dataProtect%> ID="Radio2"><font size=2>不可建置，原因：</font>
			<input type="text" name="key19" maxlength="100" size="80" value="<%=dspKey(19)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class=dataListEntry ID="Text1">
		</td>
	</tr>

	<tr><td width="15%" class="dataListHEAD" height="23">線路申請日</td>
        <td width="35%" bgcolor="silver">
	        <input type="text" name="key20" size="10"   READONLY value="<%=dspKey(20)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListentry" ID="Text55">
	        <input type="button" id="B20"  <%=FIELDPD%>    name="B20" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
	        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPD%>  alt="清除" id="C20"  name="C20"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
		</td>
        <td class="dataListSEARCH" height="23">主線到位日</td>
        <td height="23" bgcolor="silver" >
	        <input type="text" name="key25" size="10" READONLY value="<%=dspKEY(25)%>"  <%=fieldpb%><%=fieldRole(1)%> class="dataListentry" ID="Text55">     
<!--
	        <input type="button" id="B25" name="B25" <%=FIELDPD%> height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
	        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=FIELDPD%> alt="清除" id="C25"  name="C25"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
-->	        
		</td>
	</tr>

	<tr><td class="dataListHEAD" height="23">撤線日</td>
        <td height="23" bgcolor="silver" colspan=3>
            <input type="text" name="key26" size="10" value="<%=dspKey(26)%>"  <%=fieldpb%><%=fieldRole(1)%> readonly class="dataListDATA" ID="Text71">     
			<FONT SIZE=2>撤線種類︰</FONT>
			<%	s=""
			    sx=" selected "
			    sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='N9' AND CODE='" & dspkey(27) & "'"
			    rs.Open sql,conn
			    Do While Not rs.Eof
			       If rs("CODE")=dspkey(27) Then sx=" selected "
			       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
			       rs.MoveNext
			       sx=""
			    Loop
			    rs.Close
			%>
			<select size="1" name="key27"  READONLY <%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA" ID="Select35"><%=s%></select>
		</TD>
	</tr>

	<tr><td class="dataListHEAD" height="23">作廢人員</td>
        <td height="23" bgcolor="silver" >
        	<input type="text" name="key29" size="6" value="<%=dspKey(29)%>"  <%=fieldpb%><%=fieldRole(1)%> readonly class="dataListDATA" ID="Text71">
        	<font size=2><%=SrGetEmployeeName(dspKey(29))%></font>
		</td>
		<td class="dataListHEAD" height="23">作廢日</td>
        <td height="23" bgcolor="silver" >
        	<input type="text" name="key28" size="25" value="<%=dspKey(28)%>"  <%=fieldpb%><%=fieldRole(1)%> readonly class="dataListDATA" ID="Text72">     
		</td>
	</tr>

	<tr><td class="dataListHEAD" height="23">修改人員</td>
		<td height="23" bgcolor="silver">
			<input type="text" name="key31" size="6" READONLY value="<%=dspKey(31)%>" class="dataListDATA" ID="Text2">
			<font size=2><%=SrGetEmployeeName(dspKey(31))%></font>
		</td>
		<td  class="dataListHEAD" height="23">修改日期</td>
		<td  height="23" bgcolor="silver">
			<input type="text" name="key32" size="25" READONLY value="<%=dspKey(32)%>" class="dataListDATA" ID="Text9">
		</td>
	</tr>
	</table> 
</DIV>

<DIV ID="SRTAG5" onclick="srtag5" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    	<tr><td bgcolor="BDB76B" align="CENTER">備註說明</td></tr>
    </table>
</DIV>
<DIV ID="SRTAB5" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
    <TR><TD align="CENTER">
     	<TEXTAREA cols="100%" name="key30" rows=8  MAXLENGTH=500  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(30)%>" ID="Textarea1"><%=dspkey(30)%></TEXTAREA>
   		</td>
   </tr>
</div>

  </DIV>    
<%  conn.Close   
    set rs=Nothing   
    set conn=Nothing 
End Sub 
' --------------------------------------------------------------------------------------------  
%>
<!-- #include virtual="/Webap/include/RTGetUserRight.asp" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->