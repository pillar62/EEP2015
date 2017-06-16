<%  
  Dim fieldRole,fieldPa
  fieldRole=Split(FrGetUserRight("RTCustD",Request.ServerVariables("LOGON_USER")),";")
%>
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
		 'response.write "I=" & i & " ;VALUE=" & dspkey(i) & " ;TYPE=" & rs.Fields(i).Type & "<BR>"
              sType=Right("000" &Cstr(aryKeyType(i)),3)
              If Instr(cTypeNumeric,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=0
			  If Instr(cTypeChar,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=""
              If Instr(cTypeDate,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=Null
              'If Instr(cTypeAuto,sType) > 0 Or (dspKey(i)=-1 And i<numberOfKey) Then
              'Else
					'因dspkey(0)為identify欄位，故不搬入值
					if i<>0 then rs.Fields(i).Value=dspKey(i)
              'End if
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
		'response.write "I=" & i & " ;VALUE=" & dspkey(i) & " ;TYPE=" & rs.Fields(i).Type & "<BR>"
              sType=Right("000" &Cstr(aryKeyType(i)),3)
              If Instr(cTypeNumeric,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=0
			  If Instr(cTypeChar,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=""
              If Instr(cTypeDate,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=Null
         '     On Error Resume Next
             '  response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
              if i<>0 then rs.Fields(i).Value=dspKey(i)
          Next
          rs.Update
          rwCnt=rwCnt+1
          If userDefineSave="Yes" Then Call SrSaveExtDB("U")
          sw=""
       End If
    End If
    rs.Close

    ' 將sql自行產生之identify值dspkey(0)讀出至畫面
    if accessmode ="A" then
          rs.open "select max(comq1) AS comq1 from RTfareastCmtyH",conn
          if not rs.eof then
            dspkey(0)=rs("comq1")
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
    
   ' response.write "SQL=" & SQL
    rs.Open sql,conn
    If rs.Eof Then
       dataFound=False
    Else
       For i = 0 To numberOfField-1
		  'response.write "I=" & i & " ;VALUE=" & dspkey(i) & " ;TYPE=" & binary(rs.Fields(i).Attributes) & "<BR>"       
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
<body bgcolor="#FFFFFF" background="/WEBAP/IMAGE/bg.gif" text="#0000FF"  bgproperties="fixed">
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
  numberOfKey=1
  title="遠傳大寬頻社區型社區基本資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB=	"SELECT COMQ1, COMN, CUTID, TOWNSHIP, RADDR, SUPPLYRANGE, TELETYPE, " &_
				"TELEADDR, COMCNT, BUILDCNT, BUILDFLOOR, CONSIGNEE, SALESID, " &_
				"CONTACT, CONTACTTEL, SURVEYDAT, AGREE, UNAGREEDESC, AGREEDAT, " &_
				"REMITAGREE, COPYREMIT, REMITNO, REMITBANK, BANKBRANCH, " &_
				"REMITACCOUNT, REMITNAME, CHECKTITLE, CCUTID, CTOWNSHIP, CADDR, " &_
				"MEMO, UUSR, UDAT, ENGID, CONTACTE, CONTACTTELE " &_
				"FROM RTSonetCmtyH WHERE COMQ1=0 "
  sqlList=		"SELECT COMQ1, COMN, CUTID, TOWNSHIP, RADDR, SUPPLYRANGE, TELETYPE, " &_
				"TELEADDR, COMCNT, BUILDCNT, BUILDFLOOR, CONSIGNEE, SALESID, " &_
				"CONTACT, CONTACTTEL, SURVEYDAT, AGREE, UNAGREEDESC, AGREEDAT, " &_
				"REMITAGREE, COPYREMIT, REMITNO, REMITBANK, BANKBRANCH, " &_
				"REMITACCOUNT, REMITNAME, CHECKTITLE, CCUTID, CTOWNSHIP, CADDR, " &_
				"MEMO, UUSR, UDAT, ENGID, CONTACTE, CONTACTTELE " &_
				"FROM RTfareastCmtyH WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=1
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    If len(dspKey(0)) <= 0 Then
       dspkey(0)=0
    ELSEif len(dspkey(1)) < 1 Then
       formValid=False
       message="請輸入社區名稱"
    ELSEif LEN(TRIM(dspkey(2))) = 0 OR LEN(TRIM(dspkey(3))) = 0 or LEN(TRIM(dspkey(4))) = 0  then
       formValid=False
       message="請輸入社區地址"
    ELSEif len(trim(DSPKEY(8))) = 0 THEN
       formValid=False
       message="請輸入社區規模戶數"
    ELSEif  NOT ISNUMERIC(DSPKEY(8)) THEN
       formValid=False
       message="規模戶數欄位請輸入(數字)資料"
    ELSEif len(trim(DSPKEY(9))) = 0 THEN
       formValid=False
       message="請輸入社區棟數" 
    ELSEif  NOT ISNUMERIC(DSPKEY(9)) THEN
       formValid=False
       message="社區棟數欄位請輸入(數字)資料"
    ELSEif len(trim(DSPKEY(10))) = 0 THEN
       formValid=False
       message="請輸入樓高" 
    ELSEif  NOT ISNUMERIC(DSPKEY(10)) THEN
       formValid=False
       message="樓高欄位請輸入(數字)資料"
    ELSEif LEN(TRIM(dspkey(11))) = 0 and LEN(TRIM(dspkey(12))) = 0 then
       formValid=False
       message="請輸入[直銷業務]或[經銷商]"
    elseif len(trim(dspkey(11))) >0 and len(trim(dspkey(12))) > 0 then
       formValid=False
       message="[經銷商]及[直銷業務]欄位請勿同時填入資料"
    END IF        
'-------UserInformation----------------------
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(31)=V(0)
        dspkey(32)=now()
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
	          document.all("key33").value = trim(Fusrid(2))
	          document.all("key33sales").value = trim(Fusrid(3))
	       End if
       end if
   End Sub

   Sub SrSalesOnclick()
       Dim clickkey
       clickkey="KEY" & mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       sales=clickkey & "sales"
   	   prog="/Webap/include/RTGetSalesD.asp"
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
	       FUsrID=Split(Fusr,";")
	       if Fusrid(3) ="Y" then
	          document.all(clickkey).value = trim(Fusrid(0))
	          document.all(sales).value = trim(Fusrid(1))
	       End if
       end if
   End Sub

   Sub SrBankOnClick()
       prog="/Webap/include/RTGetBank.asp"
       prog=prog & "?KEY=" & document.all("KEY22").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("KEY22").value =  trim(Fusrid(0))
       End if
       end if
   End Sub
   Sub SrBankBranchOnClick()
       prog="/Webap/include/RTGetBankBranch.asp"
       prog=prog & "?KEY=" & document.all("KEY22").VALUE & ";"
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("KEY23").value =  trim(Fusrid(0))
          'document.all("key57").value =  trim(Fusrid(1))
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
                 <%=fieldRole(1)%> readonly size="20" value="<%=dspKey(0)%>" maxlength="8" class=dataListdata></td></tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if len(trim(dspkey(31))) < 1 then
       Call SrGetEmployeeRef(Rtnvalue,1,logonid)
            V=split(rtnvalue,";")  
            DSpkey(31)=V(0)
    End if         
    dspkey(32)=now()
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
    If IsDate(dspKey(18)) Then	'同意書簽訂日
       fieldPa=" class=""dataListData"" readonly "
       fieldPb=""
    Else
       fieldPa=""
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
  <span id="tags1" class="dataListTagsOn">基本資料</span>
                                                            
  <div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td><td>        
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
<tr><td width="20%" class=dataListsearch>社區名稱</td>
    <td bgcolor="silver" COLSPAN=3>
        <input type="text" name="key1" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="30"
               value="<%=dspKey(1)%>" size="30" class=dataListEntry></td>
</tr>
<tr><td class=dataListHead>社區地址</td>
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) Then 
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
		<input type="button" id="B3"  name="B3"   width="100%" style="Z-INDEX: 1"  value="..." <%=SXX3%>  >        
		<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C3" name="C3" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
		<input type="text" name="key3zip" size=8 style="color:blue;border:0px;background:transparent;" value="郵遞：<%=SrGetZipName(dspKey(2),dspKey(3))%>" readonly>
		<input type="text" name="key4" size="32" value="<%=dspkey(4)%>" maxlength="60" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text5">
	</td>
</tr>

<tr><td width="20%" class=dataListHead>可供裝範圍</td>
    <td bgcolor="silver" colspan=3>
        <input type="text" name="key5" size="60" value="<%=dspkey(5)%>" maxlength="60" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text11">
	</td>
</tr>
<tr><td width="20%" class=dataListHead>電信箱(室)類別</td>
    <td bgcolor="silver" colspan=3>
	<%
	    s=""
	    sx=" selected "
	    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 Then  
	       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='R2' " 
	       If len(trim(dspkey(6))) < 1 Then
	          sx=" selected " 
	          s=s & "<option value=""""" & sx & "></option>"  
	          sx=""
	       else
	          s=s & "<option value=""""" & sx & "></option>"  
	          sx=""
	       end if     
	    Else
	       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='R2' AND CODE='" & dspkey(6) & "'"
	    End If
	    rs.Open sql,conn
	    Do While Not rs.Eof
	       If rs("CODE")=dspkey(6) Then sx=" selected "
	       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
	       rs.MoveNext
	       sx=""
	    Loop
	    rs.Close
	%>         
		<select size="1" name="key6" style="font-family: 新細明體; font-size: 10pt"<%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select3">                                                                  
		    <%=s%>
		</select><font size=2>
        <input type="text" name="key7" size="30" value="<%=dspkey(7)%>" maxlength="30" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text11" style="display:none;">
	</td>	
</tr>

<tr><td width="20%" class="dataListHead" height="23">規模戶數</td>
	<td width="30%" height="23" bgcolor="silver">
		<input type="text" name="key8" size="5" value="<%=dspKey(8)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text9">
	</td>
	<td width="20%" class="dataListHead" height="23">社區棟數</td>
	<td width="30%" height="23" bgcolor="silver">
		<input type="text" name="key9" size="5" value="<%=dspKey(9)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text8">
        <font size=2>棟，樓高</font>
        <input type="text" name="key10" size="5" value="<%=dspKey(10)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text16">
        <font size=2>樓</font>
	</td>
 </tr>

<tr><td class=dataListHead>社區聯絡人</td>
	<td bgcolor="silver" >
		<input type="text" name="key13" maxlength="20" size="20" <%=fieldRole(1)%><%=dataProtect%> value="<%=dspKey(13)%>" class=dataListEntry>
	</td>
<td  class=dataListHead>聯絡人電話</td>
	<td  bgcolor="silver" >
		<input type="text" name="key14" maxlength="30" size="30" <%=fieldRole(1)%><%=dataProtect%> value="<%=dspKey(14)%>" class=dataListEntry>
	</td>
</tr>

<tr><td class="dataListsearch" height="23">直銷業務員</td>
    <td height="23" bgcolor="silver">
    	<input type="TEXT" name="key12" size="6" readonly value="<%=dspKey(12)%>"  <%=fieldRole(1)%><%=dataProtect%> class="dataListEntry">
       <input type="BUTTON" id="B12" name="B12" onclick="Srsalesonclick()" <%=fieldRole(1)%>  style="Z-INDEX: 1" value="...." >
       <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" onclick="SrClear" <%=fieldRole(1)%> alt="清除" id="C12"  name="C12"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
		<input type="text" name="key12sales" size=8 style="color:blue;border:0px;background:transparent;" value="<%=SrGetEmployeeName(dspKey(12))%>" readonly>
    </td>

	<td width="20%" class="dataListsearch">經銷商</td>
	<%  
		If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 AND FIELDPA = "" Then 
			sql="SELECT RTObj.CUSID,RTObj.SHORTNC " _
				&"FROM RTObj INNER JOIN " _
				&"RTConsignee ON RTObj.CUSID = RTConsignee.CUSID INNER JOIN " _
				&"RTConsigneeCASE ON RTConsigneeCASE.CUSID = RTConsignee.CUSID " _
				&"WHERE (RTConsigneeCASE.CASEID = '08') " 
			s="<option value="""" >(經銷商)</option>"
		Else
			sql="SELECT RTObj.CUSID,RTObj.SHORTNC " _
				&"FROM RTObj INNER JOIN " _
				&"RTConsignee ON RTObj.CUSID = RTConsignee.CUSID INNER JOIN " _
				&"RTConsigneeCASE ON RTConsigneeCASE.CUSID = RTConsignee.CUSID " _
				&"WHERE (RTConsigneeCASE.CASEID = '08') " _
				&"AND rtobj.cusid='" & dspkey(11) & "' "
		End If
		rs.Open sql,conn
		'If rs.Eof Then s="<option value="""" >(經銷商)</option>"
		's=""
		s="<option value="""" ></option>"
		sx=""
		Do While Not rs.Eof
			If rs("CUSID")=dspkey(11) Then sx=" selected "
			s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
			rs.MoveNext
			sx=""
		Loop
		rs.Close        
		%>
	<td width="35%" bgcolor="silver" >
       <select size="1" name="key11" <%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry">
          <%=s%>
       </select>
	</td>   
</tr>

<tr><td class="dataListsearch" height="23">維護工程師</td>
    <td height="23" bgcolor="silver" colspan=3>
    	<input type="TEXT" name="key33" size="6" readonly value="<%=dspKey(33)%>"  <%=fieldRole(1)%><%=dataProtect%> class="dataListEntry">
       <input type="BUTTON" id="B33" name="B33" onclick="Srsalesonclick()" <%=fieldRole(1)%>  style="Z-INDEX: 1" value="...." >
       <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" onclick="SrClear" <%=fieldRole(1)%> alt="清除" id="C33"  name="C33"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
		<input type="text" name="key33sales" size=8 style="color:blue;border:0px;background:transparent;" value="<%=SrGetEmployeeName(dspKey(33))%>" readonly>
    </td>
</tr>

<tr><td class=dataListHead>勘察日期</td>
<td bgcolor="silver" colspan=3>
	<input type="text" name="key15" maxlength="10" size="12" READONLY value="<%=dspKey(15)%>" <%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" ID="Text7">
   <input type="button" id="B15"  name="B15" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
   <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C15"  name="C15"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>
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
	      If trim(dspKey(16))="Y" Then 
	         rdo11=" checked "    
	      elseIf trim(dspKey(16))="N" Then 
	         rdo12=" checked " 
	      elseif trim(dspkey(16))="" then
	         dspkey(16)=""
	      end if
	%>
    <td bgcolor="silver" colspan=3>
        <input type="radio" value="Y" <%=rdo11%> name="key16" <%=fieldRole(1)%><%=dataProtect%> ID="Radio1"><font size=2>可建</font>
        <input type="radio" value="N" <%=rdo12%> name="key16" <%=fieldRole(1)%><%=dataProtect%> ID="Radio2"><font size=2>不可建置，原因：</font>
		<input type="text" name="key17" maxlength="100" size="80" value="<%=dspKey(17)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> class=dataListEntry ID="Text1">
	</td>
</tr>

<tr><td class="dataListHead">同意書簽訂日</td>
    <td bgcolor="silver" colspan=3>
     	<input type="text" name="key18" size="10" value="<%=dspKey(18)%>" READONLY <%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> maxlength="10" class="dataListEntry" ID="Text21">
		<input type="button" id="B18" name="B18" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
		<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C18" name="C18"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
	</td>
</tr>
	<tr>
			<td  class="dataListHEAD" height="23">修改人員</td>
			<td  height="23" bgcolor="silver">
				<input type="text" name="key31" size="6" READONLY value="<%=dspKey(31)%>" class="dataListDATA" ID="Text2">
				<font size=2><%=SrGetEmployeeName(dspKey(31))%></font>
			</td>  
			<td  class="dataListHEAD" height="23">修改日期</td>
			<td  height="23" bgcolor="silver">
				<input type="text" name="key32" size="25" READONLY value="<%=dspKey(32)%>" class="dataListDATA" ID="Text9">
			</td>       
	</tr>         


    <DIV ID=SRTAB3>
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr><td bgcolor="lightblue" align="center" colspan=5>電費相關</td></tr>
    <tr>
          <td width="20%" class="dataListHead">電費聯絡人</td>
          <td width="30%" bgcolor="silver"> 
          	<input type="text" name="KEY34" value="<%=DSPKEY(34)%>" size="20" maxlength="20" class="dataListEntry">
		  </td>
          <td width="20%" class="dataListHead">電費聯絡人電話</td>
          <td width="30%" bgcolor="silver"> 
          	<input type="text" name="KEY35" value="<%=DSPKEY(35)%>" size="30" maxlength="30" class="dataListEntry">
		  </td>
     </TR>

    <tr>
          <td width="20%" class="dataListHead">支票抬頭</td>
          <td bgcolor="silver" colspan=3>
          	<input type="text" name="KEY26" size="60" value="<%=DSPKEY(26)%>" maxlength="60" class="dataListEntry">
		  </td>
      </TR>

	<tr><td class=dataListhead>支票寄送地址</td>
	<%
		s=""
	    sx=" selected "
	    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) Then 
	       sql="SELECT Cutid,Cutnc FROM RTCounty " 
	       If len(trim(DSPKEY(27))) < 1 Then
	          sx=" selected " 
	       else
	          sx=""
	       end if     
	       s=s &"<option value=""" &"""" &sx &">(縣市別)</option>"       
	       SXX3=" onclick=""SrCountyOnClick()""  "
	    Else
	       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & DSPKEY(27) & "' " 
	       SXX3=""
	    End If
	    sx=""    
	    rs.Open sql,conn
	    Do While Not rs.Eof
	       If rs("cutid")=DSPKEY(27) Then sx=" selected "
	       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
	       rs.MoveNext
	       sx=""
	    Loop
	    rs.Close
	%>
    	<td colspan="3" bgcolor="silver">
			<select size="1" name="KEY27" size="1" class="dataListEntry"><%=s%></select>
        	<input type="text" name="KEY28" size="8" value="<%=DSPKEY(28)%>" maxlength="10" readonly <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
			<input type="button" id="B28" name="B28" width="100%" style="Z-INDEX: 1" value="..." <%=SXX3%> >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C28" name="C28" style="Z-INDEX: 1"   border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
			<input type="text" name="key28zip" size=8 style="color:blue;border:0px;background:transparent;" value="郵遞：<%=SrGetZipName(dspKey(27),dspKey(28))%>" readonly>
        	<input type="text" name="key29" size="60" value="<%=DSPKEY(29)%>" maxlength="60" class="dataListEntry"></td>
	</tr>
   </table>
   </DIV>

    <DIV ID=SRTAB3>
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr><td bgcolor="lightblue" align="center">社區備註</td></tr>

	<TR><TD align="CENTER">
     	<TEXTAREA  cols="100%"  name="key30" rows=8  MAXLENGTH=500  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(30)%>" ID="Textarea1"><%=dspkey(30)%></TEXTAREA>
   		</td>
   </tr>
   </table>
   </DIV>

    <DIV ID=SRTAB4 style="display: none">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
<tr><td bgcolor="lightblue" align="center">暫不使用</td></tr>
<tr><td width="21%" class="dataListHead">公電補助同意書</td>
	<td width="20%" height="23" bgcolor="silver">
            <% 
              If Len(Trim(fieldRole(4) &dataProtect)) < 1 Then
                 rdo5=""
                 rdo6=""
              Else
                 rdo5=" disabled "
                 rdo6=" disabled "
              End If            
                If DSPKEY(19)="Y" Then rdo5=" checked "    
                If DSPKEY(19)="N" Then rdo6=" checked " %>                          
        
               <input type="radio" value="Y" <%=RDO5%> name="key19" <%=fieldpa%><%=dataProtec%>><font size=2>有</font>
               <input type="radio" value="N" <%=RDO6%> name="key19" <%=fieldpa%><%=dataProtect%>><font size=2>無</font>                         
	<%
	    s=""
	    sx=" selected "
	    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
	       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D1' " 
	       If len(trim(DSPKEY(20))) < 1 Then
	          sx=" selected " 
	             s=s & "<option value=""""" & sx & "></option>"  
	          sx=""
	       else
	          s=s & "<option value=""""" & sx & "></option>"  
	          sx=""
	       end if     
	    Else
	       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D1' AND CODE='" & DSPKEY(20) & "'"
	    End If
	    rs.Open sql,conn
	    Do While Not rs.Eof
	       If rs("CODE")=DSPKEY(20) Then sx=" selected "
	       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
	       rs.MoveNext
	       sx=""
	    Loop
	    rs.Close
	    %>         
           <select name="KEY20" <%=fieldpa%><%=FIELDROLE(4)%><%=dataProtect%>  class="dataListEntry">
                <%=S%>
           </select>
	</td>
    <td width="8%" height="23" class="dataListHead">公電補助同意書編號</td>
    <td width="8%" height="23" bgcolor="silver">
        <input  class="dataListEntry" type="text" name="KEY21" <%=fieldpa%><%=dataProtec%> value="<%=DSPKEY(21)%>">
	</td>
</tr>
      <tr><td width="21%" class="dataListHead">匯款銀行</td>
          <td width="26%" bgcolor="silver">
				<%
					name=""
					if DSPKEY(22) <> "" then
						sqlxx=" select headnc from rtbank where headno='" & DSPKEY(22) & "' order by headnc "
						rs.Open sqlxx,conn
						if rs.eof then
							name="(銀行名稱)"
						else
							name=rs("headnc")
						end if
						rs.close
					end if
				%>
				<input type="text" name="KEY22"value="<%=DSPKEY(22)%>" <%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" size="3" maxlength="3" readonly class="dataListDATA">
				<input type="BUTTON" id="B22" name="B22" style="Z-INDEX: 1"  value="...." onclick="SrBankOnClick()">
				<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C22" name="C22" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
				<font size=2><%=name%></font>
			</td>
			<td width="21%" class="dataListHead">匯款分行</td>
			<td width="26%" bgcolor="silver">
				<%
					name=""
					if DSPKEY(23) <> "" then
						sqlxx=" select branchnc from rtbankbranch where headno='" & DSPKEY(22) & "' and branchno='" & DSPKEY(23) & "' "
						rs.Open sqlxx,conn
						if rs.eof then
							name="(分行名稱)"
						else
							name=rs("branchnc")
						end if
						rs.close
					end if
				%>
				<input type="text" name="KEY23"value="<%=DSPKEY(23)%>" <%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" size="5" maxlength="4" readonly class="dataListDATA">
				<input type="BUTTON" id="B23" name="B23" style="Z-INDEX: 1"  value="...." onclick="SrBankBranchOnClick()">
				<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C23" name="C23" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
				<font size=2><%=name%></font>
			</td>
      </tr>
	  <tr><td class="dataListHead">匯款帳號</td>
          <td bgcolor="silver">
				<input type="text" name="KEY24" size="15" value="<%=DSPKEY(24)%>" <%=fieldRole(4)%><%=dataProtect%> maxlength="15" class="dataListEntry">
		  </td>
		  <td class="dataListHead">匯款戶名</td>
          <td bgcolor="silver">
          		<input type="text" name="KEY25" size="38" value="<%=DSPKEY(25)%>" <%=fieldRole(4)%><%=dataProtect%> maxlength="50" class="dataListEntry">
		  </td>
      </TR>
   </table>
   </DIV>


   </table>
  </div> 
<% 
    conn.Close   
    set rs=Nothing   
    set conn=Nothing 
End Sub 
' --------------------------------------------------------------------------------------------  
%>
<!-- #include virtual="/Webap/include/RTGetUserRight.asp" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->