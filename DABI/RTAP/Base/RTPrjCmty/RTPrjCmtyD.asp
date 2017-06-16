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
              If Instr(cTypeDate,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=Null
              If Instr(cTypeAuto,sType) > 0 Or (dspKey(i)=-1 And i<numberOfKey) Then
              Else
              '   On Error Resume Next
                'runpgm=Request.ServerVariables("PATH_INFO") 
                   '因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
					'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                     if i<>0 then rs.Fields(i).Value=dspKey(i)         
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
                 ' ,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
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
    ' 當程式為HB社區基本資料維護作業時,將sql自行產生之identify值dspkey(0)讀出至畫面
    if accessmode ="A" then
       runpgm=Request.ServerVariables("PATH_INFO")
          rs.open "select max(comq1) AS comq1 from RTPrjCmtyH",conn
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
  title="專案社區基本資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT COMQ1, COMN, CUTID, TOWNSHIP, RADDR, RZONE, COMCNT, CONTACT, CONTACTTEL, " &_
			  "MANAGER, MANAGERTEL, CONSIGNEE, SALESID, MEMO, UUSR, UDAT FROM RTPrjCmtyH " &_
			  "WHERE COMQ1=0 "
  sqlList=	  "SELECT COMQ1, COMN, CUTID, TOWNSHIP, RADDR, RZONE, COMCNT, CONTACT, CONTACTTEL, " &_
			  "MANAGER, MANAGERTEL, CONSIGNEE, SALESID, MEMO, UUSR, UDAT FROM RTPrjCmtyH " &_
			  "WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=1
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    If len(dspKey(0)) <= 0 Then dspkey(0)=0
    if len(trim(DSPKEY(6))) = 0 THEN DSPKEY(6)=0
    if len(trim(DSPKEY(11))) = 0 THEN DSPKEY(11)="" 
    if len(trim(DSPKEY(12))) = 0 THEN DSPKEY(12)="" 

    if len(dspkey(1)) < 1 Then
       formValid=False
       message="請輸入社區名稱"       
    ELSEif LEN(TRIM(dspkey(2))) = 0 OR LEN(TRIM(dspkey(4))) = 0  then
       formValid=False
       message="請輸入社區地址" 
    ELSEif  (NOT ISNUMERIC(DSPKEY(6))) or len(trim(DSPKEY(6))) = 0 THEN
       formValid=False
       message="規模戶數欄位請輸入(數字)資料"         
    ELSEif len(trim(DSPKEY(11))) = 0 and len(trim(DSPKEY(12))) = 0 THEN
       formValid=False
       message="請輸入社區業務員或經銷商" 
    ELSEif len(trim(DSPKEY(11))) > 0 and len(trim(DSPKEY(12))) > 0 THEN
       formValid=False
       message="社區業務員或經銷商只能兩者擇一"
    END IF

'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(14)=V(0)
        dspkey(15)=now()
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
   Sub Srsalesonclick()
       prog="RTGetsalesD.asp"
       'prog=prog & "?KEY=" & document.all("KEY4").VALUE & ";" 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key12").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub      
   Sub Srcounty3onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY2").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key3").value =  trim(Fusrid(0))
          document.all("key5").value =  trim(Fusrid(1))
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
		<tr><td width="20%" class=dataListHead>社區序號</td>
			<td width="80%"  bgcolor="silver">
				<input type="text" name="key0" readonly size="10" value="<%=dspKey(0)%>" class=dataListdata>
			</td>
		</tr>
	</table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(14))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(14)=V(0)
        End if  
       dspkey(15)=now()
    else
        if len(trim(dspkey(14))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(14)=V(0)
        End if         
        dspkey(15)=now()
    end if      

    Dim conn,rs,s,sx,sql,t
    If IsDate(dspKey(32)) Then
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
	<tr><td width="10%" class=dataListHEAD>社區名稱</td>
		<td width="35%" bgcolor="silver">
			<input type="text" name="key1" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
					maxlength="30" size="30" value="<%=dspKey(1)%>" class=dataListEntry>
		</td>
		<td class="dataListHEAD" height="23">規模戶數</td>                                 
		<td height="23" bgcolor="silver">
			<input type="text" name="key6" size="5" maxlength="5" value="<%=dspKey(6)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry">
		</td>                                 
	</tr>

	<tr><td class=dataListsearch>社區地址</td>
	<%
		s=""
		sx=" selected "
		If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) Then 
		sql="SELECT Cutid,Cutnc FROM RTCounty " 
		If len(trim(dspkey(2))) < 1 Then
			sx=" selected " 
		else
			sx=""
		end if     
		s=s &"<option value=""" &"""" &sx &">(縣市別)</option>"       
		SXX3=" onclick=""Srcounty3onclick()""  "
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
		<td bgcolor="silver" >
			<select size="1" name="key2"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select2"><%=s%></select>
			<input type="text" name="key3" size="8" value="<%=dspkey(3)%>" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
			<font size=2>(鄉鎮)
			<input type="button" id="B3" name="B3" width="100%" style="Z-INDEX: 1"  value="..." <%=SXX3%>  >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C3"  name="C3" style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >
			<input type="text" name="key4" size="32" value="<%=dspkey(4)%>" maxlength="60" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
		</td>                                 
        <td width="10%" class="dataListHead" height="25">郵遞區號</td>                                 
        <td width="35%" height="25" bgcolor="silver">
			<input type="text" name="key5" size="10" value="<%=dspkey(5)%>" maxlength="5" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListdata" readonly ID="Text6">
		</td>  
	</tr>  

	<tr><td class=dataListHEAD>社區聯絡人</td>
		<td bgcolor="silver" >
			<input type="text" name="key7" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
				maxlength="20" size="20" value="<%=dspKey(7)%>" class=dataListEntry></td>
	<td  class=dataListsearch>聯絡人電話</td>
		<td  bgcolor="silver" >
			<input type="text" name="key8" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
				maxlength="30" size="30" value="<%=dspKey(8)%>" class=dataListEntry></td>               
	</tr>

	<tr><td class=dataListsearch>社區總幹事</td>
		<td bgcolor="silver" >
			<input type="text" name="key9" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
				maxlength="20" size="20" value="<%=dspKey(9)%>" class=dataListEntry ID="Text4"></td>
	<td  class=dataListsearch>總幹事電話</td>
		<td  bgcolor="silver" >
			<input type="text" name="key10" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
				maxlength="30" size="30" value="<%=dspKey(10)%>" class=dataListEntry ID="Text5"></td>               
	</tr>

	<tr><td class="dataListHEAD" height="23">直銷業務</td>               
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="TEXT" name="key12" size="6" readonly value="<%=dspKey(12)%>"  <%=fieldRole(1)%><%=dataProtect%> class="dataListDATA">
           <input type="BUTTON" id="B6" name="B6" <%=fieldRole(1)%> <%=fieldpc%>width="100%" style="Z-INDEX: 1"  value="...." onclick="Srsalesonclick()"  >   
           <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldRole(1)%> <%=fieldpc%>alt="清除" id="C6"  name="C6"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">
           <font size=2><%=SrGetEmployeeName(dspKey(12))%></font>
        </td>

		<td width="10%" class=dataListHEAD>經銷商</td>
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
			If rs.Eof Then s="<option value="""" >(經銷商)</option>"
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
           <select size="1" name="key11" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry">
              <%=s%>
           </select>
		</td>   
	</tr>

	<tr><td class="dataListHEAD" height="23">資料異動人</td>                                 
		<td height="23" bgcolor="silver">
			<input type="text" name="key14" size="6" READONLY value="<%=dspKey(14)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2">
           <font size=2><%=SrGetEmployeeName(dspKey(14))%></font>
		</td>  
		<td class="dataListHEAD" height="23">資料異動日</td>                                 
		<td height="23" bgcolor="silver">
			<input type="text" name="key15" size="25" READONLY value="<%=dspKey(15)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
		</td>       
	</tr>

	<tr><td class="dataListHEAD" height="23">備註說明</td>                                 
		<td height="23" bgcolor="silver" colspan=3>
			<TEXTAREA cols="100%" name="key13" rows=8 MAXLENGTH=500 class="dataListentry" <%=dataprotect%> value="<%=dspkey(13)%>" ID="Textarea1"><%=dspkey(13)%></TEXTAREA>
		</td>       
	</tr>  
	
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
