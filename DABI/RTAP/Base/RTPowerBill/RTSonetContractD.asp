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
'response.write    SQL
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
               if i<>1 then rs.Fields(i).Value=dspKey(i)
               if i=1 then
               '  Set rsc=Server.CreateObject("ADODB.Recordset")
               '  rsc.open "select max(lineq1) AS lineq1 from RTSonetCmtyLine where comq1=" & dspkey(0) ,conn
               '  if len(rsc("lineq1")) > 0 then
               '     dspkey(1)=rsc("lineq1") + 1
               '  else
               '     dspkey(1)=1
               '  end if
               '  rsc.close
               '  rs.Fields(i).Value=dspKey(i) 
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
             if i<>1 then rs.Fields(i).Value=dspKey(i)
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
          'rs.open "select max(lineq1) AS lineq1 from RTSonetCmtyLine where comq1=" & dspkey(0) ,conn
          'if not rs.eof then
          '  dspkey(1)=rs("lineq1")
          'end if
          'rs.close
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
  countchange()
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
  title="社區合約資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB=	"SELECT COMQ1, CTNO, EDAT, STRDAT, ENDDAT, PERIOD, COUNTTYPE, METERRATE, " &_
				"MPAY, CUSTNUMUP, MPAY2, CUSTNUMINC, NOTE, UDAT, UUSR, CANCELDAT, " &_
				"CANCELUSR " &_
				"FROM RTContract WHERE CTNO=0 "
  sqlList=		"SELECT COMQ1, CTNO, EDAT, STRDAT, ENDDAT, PERIOD, COUNTTYPE, METERRATE, " &_
				"MPAY, CUSTNUMUP, MPAY2, CUSTNUMINC, NOTE, UDAT, UUSR, CANCELDAT, " &_
				"CANCELUSR " &_
				"FROM RTContract WHERE "
  numberOfKey=2
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=1
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
	select case dspKey(6)
		case "01"
			dspKey(7) = request("01_meterrate")
		case "02"
			dspKey(8) = request("02_mpy")
		case "03"
			dspKey(8) = request("03_mpy")
			dspKey(9) = request("03_custnumup")
			dspKey(10) = request("03_mpy2")
		case "04"
			dspKey(8) = request("04_mpy")
			dspKey(9) = request("04_custnumup")
			dspKey(10) = request("04_mpy2")
			dspKey(11) = request("04_custnuminc")
		case else
	       formValid=False
    	   message="計算方式不可空白"
	end select

    If len(dspKey(1)) <= 0 Then
       dspkey(1)=0
    End If

    if len(trim(dspkey(3)))=0 then
       formValid=False
       message="合約起始日不可空白"
    elseif len(trim(dspkey(4)))=0 then
       formValid=False
       message="合約到期日不可空白"
   	elseif dspkey(3) >= dspkey(4) then
       formValid=False
       message="到期日不可大於起始日"
    elseif len(trim(dspkey(5)))=0 then
       formValid=False
       message="電費結算週期不可空白"
	elseif DSPKEY(6)="01" and len(trim(dspkey(7)))=0 then
       formValid=False
       message="電錶計費:每度電應大於0"
	elseif DSPKEY(6)="02" and dspkey(8)=0 then
       formValid=False
       message="每月固定電費應大於0"
	elseif DSPKEY(6)="03" and (dspkey(8)=0 or dspkey(10)=0) then
       formValid=False
       message="每月固定電費應大於0"
	elseif DSPKEY(6)="03" and dspkey(9)=0 then
       formValid=False
       message="電費戶數應大於0"
	elseif DSPKEY(6)="04" and (dspkey(8)=0 or dspkey(10)=0) then
       formValid=False
       message="每月固定電費應大於0"
	elseif DSPKEY(6)="04" and (dspkey(9)=0 or dspkey(11)=0) then
       formValid=False
       message="電費戶數應大於0"
    end if
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(14)=V(0)
        dspkey(13)=now()
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
          document.all(clearkey).value = ""
       end if
   End Sub

   Sub countchange()
   	if len(trim(document.all("key7").value))=0 then document.all("key7").value =0
   	if len(trim(document.all("key8").value))=0 then document.all("key8").value =0
   	if len(trim(document.all("key9").value))=0 then document.all("key9").value =0
   	if len(trim(document.all("key10").value))=0 then document.all("key10").value =0
   	if len(trim(document.all("key11").value))=0 then document.all("key11").value =0

	document.all("01_meterrate").value = document.all("key7").value
	document.all("02_mpy").value = document.all("key8").value
	document.all("03_mpy").value = document.all("key8").value
	document.all("04_mpy").value = document.all("key8").value
	document.all("03_custnumup").value = document.all("key9").value
	document.all("04_custnumup").value = document.all("key9").value
	document.all("03_mpy2").value = document.all("key10").value
	document.all("04_mpy2").value = document.all("key10").value
	document.all("04_custnuminc").value = document.all("key11").value

   	select case document.all("key6").value
       case "01"  
          window.div_count01.style.display=""
          window.div_count02.style.display="none"
          window.div_count03.style.display="none"
          window.div_count04.style.display="none"
       case "02"  
          window.div_count01.style.display="none"
          window.div_count02.style.display=""
          window.div_count03.style.display="none"
          window.div_count04.style.display="none"
       case "03"  
          window.div_count01.style.display="none"
          window.div_count02.style.display="none"
          window.div_count03.style.display=""
          window.div_count04.style.display="none"
       case "04"  
          window.div_count01.style.display="none"
          window.div_count02.style.display="none"
          window.div_count03.style.display="none"
          window.div_count04.style.display=""
       case else
          window.div_count01.style.display="none"
          window.div_count02.style.display="none"
          window.div_count03.style.display="none"
          window.div_count04.style.display="none"
	end select
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
		<tr><td width="20%" class=dataListHead>合約序號</td>
			<td width="25%"  bgcolor="silver"><input type="text" name="key1" size="6" value="<%=dspKey(1)%>" readonly class=dataListdata></td>
			<td width="20%" class=dataListHead>社區序號</td>
			<td width="25%"  bgcolor="silver"><input type="text" name="key0" size="6" value="<%=dspKey(0)%>" readonly class=dataListdata></td>
		</tr>
	</table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
    logonid=session("userid")

    if dspmode="新增" then
        if len(trim(dspkey(2))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(14)=V(0)
        End if  
       dspkey(13)=now()
       dspkey(2)=now()
    else
        if len(trim(dspkey(13))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(14)=V(0)
        End if         
        dspkey(13)=now()
    end if

' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN

    '業助
	sql ="SELECT * FROM XXLib..UserGroup WHERE [GROUP] in('RTADMIN','業助') and userid ='"& logonid &"' "
    rs.Open sql,conn
    If rs.Eof Then
		basedata=False
    Else
		basedata=true
    End If
    rs.Close

    If basedata=false Then
       fieldPB=" class=""dataListData"" readonly "
       FIELDPD=" DISABLED "
    Else
       fieldPB=""
       FIELDPD=""
    End If    
      
    %>

<span id="tags1" class="dataListTagsOn">合約資訊</span>

<DIV ID="SRTAG0">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    	<tr><td bgcolor="BDB76B" align="CENTER">電費相關資料</td></tr>
    </table>
</div>

<DIV ID=SRTAB0>
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
	<tr><td width="15%" class=dataListHead>社區名稱</td>
		<% 	sql="SELECT comn FROM RTSonetCmtyH where comq1=" & dspkey(0)
			rs.Open sql,conn
			colComn = rs("comn")
			rs.Close
		%>	
	    <td width="35%" bgcolor="silver">
	        <input type="text" name="keyComn" size=50 value="<%=colComn%>" readonly style="color:blue;border:0px;background:transparent;">
		</td>
		<td width="15%" class="dataListHEAD" height="23">建檔日</td>
		<td width="35%" bgcolor="silver">
			<input type="text" name="key2" size="25" READONLY value="<%=dspKey(2)%>" class="dataListDATA" ID="Text2">
		</td>
	</tr>

	<tr><td width="15%" class="dataListHEAD" height="23">合約起始日</td>
        <td width="35%" bgcolor="silver">
	        <input type="text" name="key3" value="<%=dspKey(3)%>" ID="Text3" size="10" READONLY <%=fieldpb%> <%=fieldRole(1)%> class="dataListentry">
	        <input type="button" id="B3" name="B3" onclick="SrBtnOnClick" value="...." <%=FIELDPD%> height="100%" width="100%" style="Z-INDEX: 1">
	        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C3" name="C3" onclick="SrClear" <%=FIELDPD%> style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
		</td>
        <td class="dataListSEARCH" height="23">合約到期日</td>
        <td height="23" bgcolor="silver" >
	        <input type="text" name="key4" value="<%=dspKey(4)%>" ID="Text4" size="10" READONLY <%=fieldpb%> <%=fieldRole(1)%> class="dataListentry">
	        <input type="button" id="B4" name="B4" onclick="SrBtnOnClick" value="...." <%=FIELDPD%> height="100%" width="100%" style="Z-INDEX: 1">
	        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C4" name="C4" onclick="SrClear" <%=FIELDPD%> style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut">
		</td>
	</tr>

	<tr><td class=dataListHead>電費計算週期(月)</td>
	    <td bgcolor="silver" colspan=3>
	        <input type="text" name="key5" ID="Text5" value="<%=dspkey(5)%>" size="3" maxlength="3" <%=fieldpb%> class="dataListEntry">
		</td>
	</tr>


	<tr><td WIDTH="15%" class="dataListHEAD" height="23" rowspan="2">電費計算方式</td>
		<%  s=""
		    sx=" selected "
		    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) and basedata=true Then 
		       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='R4' " 
		       If len(trim(dspkey(6))) < 1 Then
		          sx=" selected "
		       else
		          sx=""
		       end if
			   s=s & "<option value=""""" & sx & ">(計算方式)</option>"  
			   sx=""
		    Else
		       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='R4' AND CODE='"& dspkey(6) &"'"
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
        <td WIDTH="35%" height="23" bgcolor="silver" colspan="3">
		   	<select size="1" name="key6" ID="key6" onchange="countchange()" <%=fieldpb%> class="dataListEntry"><%=s%></select>
        </td>
	</tr>

	<tr><td bgcolor="silver" colspan=3>
		<span ID="div_count" style="display:none;">
			<input type="text" name="key7" ID="Text7" value="<%=dspkey(7)%>" size="3">每度電<BR>
			<input type="text" name="key8" ID="Text8" value="<%=dspkey(8)%>" size="3">月固定<BR>
			<input type="text" name="key9" ID="Text9" value="<%=dspkey(9)%>" size="3">n戶以上<BR>
			<input type="text" name="key10" ID="Text10" value="<%=dspkey(10)%>" size="3">月固定(n戶以上)<BR>
			<input type="text" name="key11" ID="Text11" value="<%=dspkey(11)%>" size="3">每增加?戶(n戶以上)<BR>
		</span>
		<span ID="div_count01" style="display:none;">
			依電錶計費，每度電<input type="text" name="01_meterrate" ID="01_meterrate" value="0" size="5" maxlength="5" class="dataListEntry">元<br>
		</span>
		<span ID="div_count02" style="display:none;">
			電費每度月固定<input type="text" name="02_mpy" ID="02_mpy" value="0" size="5" maxlength="5" class="dataListEntry">元<br>
		</span>
		<span ID="div_count03" style="display:none;">
			電費每度月固定<input type="text" name="03_mpy" ID="03_mpy" value="0" size="5" maxlength="5" class="dataListEntry">元，達
			<input type="text" name="03_custnumup" ID="03_custnumup" value="0" size="3" maxlength="3" class="dataListEntry">戶以上，每月電費
			<input type="text" name="03_mpy2" ID="03_mpy2" value="0" size="5" maxlength="5" class="dataListEntry">元<br>
		</span>
		<span ID="div_count04" style="display:none;">
			電費每度月固定<input type="text" name="04_mpy" ID="04_mpy" value="0" size="5" maxlength="5" class="dataListEntry">元，達
			<input type="text" name="04_custnumup" ID="04_custnumup" value="0" size="3" maxlength="3" class="dataListEntry">戶以上，每增加
			<input type="text" name="04_custnuminc" ID="04_custnuminc" value="0" size="2" maxlength="2" class="dataListEntry">戶，加收
			<input type="text" name="04_mpy2" ID="04_mpy2" value="0" size="5" maxlength="5" class="dataListEntry">元
		</span>
		</td>
	</tr>


	<tr><td class="dataListHEAD" height="23">修改人員</td>
		<td height="23" bgcolor="silver">
			<input type="text" name="key14" size="6" value="<%=dspKey(14)%>" ID="Text14" READONLY class="dataListDATA">
			<font size=2><%=SrGetEmployeeName(dspKey(14))%></font>
		</td>
		<td  class="dataListHEAD" height="23">修改日期</td>
		<td  height="23" bgcolor="silver">
			<input type="text" name="key13" size="25" value="<%=dspKey(13)%>" ID="Text13" READONLY class="dataListDATA">
		</td>
	</tr>
	<tr><td class="dataListHEAD" height="23">作廢人員</td>
        <td height="23" bgcolor="silver" >
        	<input type="text" name="key16" ID="Text16" value="<%=dspKey(16)%>" size="6" readonly class="dataListDATA">
        	<font size=2><%=SrGetEmployeeName(dspKey(16))%></font>
		</td>
		<td class="dataListHEAD" height="23">作廢日</td>
        <td height="23" bgcolor="silver" >
        	<input type="text" name="key15" ID="Text15" value="<%=dspKey(15)%>" size="25" readonly class="dataListDATA">
		</td>
	</tr>
	</table> 
</DIV>

<DIV><table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
		<tr><td bgcolor="BDB76B" align="CENTER">備註說明</td></tr>
    </table>
</DIV>
<DIV><table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table12">
    <TR><TD align="CENTER" bgcolor="silver">
     	<TEXTAREA cols="100%" name="key12" value="<%=dspkey(12)%>" ID="Textarea12" MAXLENGTH=1000 rows=8 class="dataListentry"><%=dspkey(12)%></TEXTAREA>
   		</td>
   </tr>
</div>


<%  conn.Close   
    set rs=Nothing   
    set conn=Nothing 
End Sub 
' --------------------------------------------------------------------------------------------  
%>
<!-- #include virtual="/Webap/include/RTGetUserRight.asp" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->