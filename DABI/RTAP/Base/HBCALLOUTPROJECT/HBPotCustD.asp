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
   ' RESPONSE.WRITE SQL
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
                   case ucase("/webap/rtap/base/hbcalloutproject/HBPotCustD.asp")
         'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       if i <> 0 then rs.Fields(i).Value=dspKey(i)    
                       if i=0 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         cusidxx="P" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
                         rsc.open "select max(caseno) AS caseno from RTPotCustH where caseno LIKE '" & cusidxx & "%' " ,conn
                         if len(trim(rsc("caseno"))) > 0 then 
                            dspkey(0)=cusidxx & right("0000" & cstr(cint(right(rsc("caseno"),4)) + 1),4)
                         else
                            dspkey(0)=cusidxx & "0001"
                         end if
			'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
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
                 case ucase("/webap/rtap/base/hbcalloutproject/HBPotCustD.asp")
                    'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                     rs.Fields(i).Value=dspKey(i)    
                 case else
                     rs.Fields(i).Value=dspKey(i)
                     'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
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
       if ucase(runpgm)=ucase("/webap/rtap/base/hbcalloutproject/HBPotCustD.asp") then
          cusidxx="P" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
          rsc.open "select max(caseno) AS caseno from RTPotCustH where caseno LIKE '" & cusidxx & "%' " ,conn
          if not rsC.eof then
            dspkey(0)=rsC("caseno")
          else
	        dspkey(0)=cusidxx & "0001"
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
  numberOfKey=1
  title="潛在客戶資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB=	"SELECT CASENO, COMN, CUSNC, TEL, MOBILE, CUTID, TOWNSHIP, ADDR, " &_
				"REQUESTID, SALESID, CONSIGNEE, MEMO, RCVDAT, RCVUSR, FINISHDAT, " &_
				"FINISHUSR, CANCELDAT, CANCELUSR, UDAT, UUSR " &_
				"FROM RTPotCustH WHERE caseno='' "
  sqlList=		"SELECT CASENO, COMN, CUSNC, TEL, MOBILE, CUTID, TOWNSHIP, ADDR, " &_
				"REQUESTID, SALESID, CONSIGNEE, MEMO, RCVDAT, RCVUSR, FINISHDAT, " &_
				"FINISHUSR, CANCELDAT, CANCELUSR, UDAT, UUSR " &_
				"FROM RTPotCustH WHERE "			
  userDefineRead="Yes"      
  userDefineSave="Yes"       
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=99
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
  if len(trim(dspkey(2)))=0 then
       formValid=False
       message="客戶名稱不可空白"   
  elseif len(trim(dspkey(4)))=0 and len(trim(dspkey(3)))=0 then
       formValid=False
       message="聯絡電話和行動電話至少需輸入一項"                
  end if

'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(19)=V(0)
        dspkey(18)=now()
    end if        
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveXScript()%>
   <SCRIPT Language="VBScript">
   Sub Srcounty10onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY5").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key6").value =  trim(Fusrid(0))
          'document.all("key12").value =  trim(Fusrid(1))
       End if       
       end if
    END SUB    

   Sub Srsalesonclick()
       prog="RTGetsalesD.asp?KEY=;"
       'prog=prog & document.all("KEY9").VALUE & ";" 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key9").value =  trim(Fusrid(0))
       End if       
       end if
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
	<tr><td width="10%" class=dataListHead>客服單號</td>
		<td width="15%" bgcolor="silver">
			<input type="text" name="key0" readonly size="12" value="<%=dspKey(0)%>" class=dataListdata>
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
        if len(trim(dspkey(19))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(19)=V(0)
        End if  
       dspkey(18)=now()
       dspkey(12)=now()
    else
        if len(trim(dspkey(19))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(19)=V(0)
        End if         
        dspkey(18)=now()
    end if      
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t

    '客服單結案或作廢後 protect
    If len(trim(dspKey(14))) > 0 or len(trim(dspKey(16))) > 0 Then
       fieldPa=" class=""dataListData"" readonly "
       fieldpD=" disabled "
    Else
       fieldPa=""        
       fieldpD=""
    end if
      
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
  <span id="tags1" class="dataListTagsOn">潛在用戶資訊</span>
                                                            
<div class=dataListTagOn> 
<table width="100%">
	<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
	<tr><td>&nbsp;</td>
		<td>     

	<DIV ID="SRTAG2">
		<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
		<tr><td bgcolor="BDB76B" align="center">客戶基本資料</td></tr></table>
	</DIV>

    <DIV ID=SRTAB2 >
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3">
	<tr>
		<td width="10%" class=dataListHEAD>社區名稱</td>
		<td height="23" bgcolor="silver" >   
            <input type="text" name="key1" value="<%=dspKey(1)%>" size="30" maxlength=30 <%=fieldpa%><%=fieldRole(1)%> class="dataListentry" ID="Text1">
		</TD>

		<td width="10%" class=dataListHEAD>客戶名稱</td>
		<td height="23" bgcolor="silver" >   
            <input type="text" name="key2" value="<%=dspKey(2)%>" size="30" maxlength=30 <%=fieldpa%><%=fieldRole(1)%> class="dataListentry">
		</TD>

		<td width="10%" class=dataListHEAD>需求類別</td>
		<%
			s=""
			sx=" selected "
			If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 Then  
			sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='P4' " 
			If len(trim(dspkey(8))) < 1 Then
				sx=" selected " 
				s=s & "<option value=""""" & sx & "></option>"  
				sx=""
			else
				s=s & "<option value=""""" & sx & "></option>"  
				sx=""
			end if     
			Else
			sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='N4' AND CODE='" & dspkey(8) & "'"
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
		<td width="23%" bgcolor="silver" >
			<select size="1" name="key8" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35" >
			<%=s%>
		   </select>
		</td>
	</tr>
	<TR>
		<td class="dataListHEAD" height="23">聯絡電話</td>               
		<td height="23" bgcolor="silver" >   
			<input type="text" name="key3" value="<%=dspKey(3)%>" size="30" maxlength=50 <%=fieldpa%><%=fieldRole(1)%> class="dataListentry">
        </TD>      
		<td class="dataListHEAD" height="23">行動電話</td>               
        <td height="23" bgcolor="silver" colspan=3>
			<input type="text" name="key4" value="<%=dspKey(4)%>" size="30" maxlength=30 <%=fieldpa%><%=fieldRole(1)%> class="dataListentry">
        </td>
	</tr>
	<tr>
		<td class=dataListHEAD>安裝地址</td>
		<%
			s=""
			sx=" selected "
			If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) Then 
				sql="SELECT Cutid,Cutnc FROM RTCounty " 
				If len(trim(dspkey(5))) < 1 Then
					sx=" selected " 
				else
					sx=""
				end if     
				s=s &"<option value=""" &"""" &sx &">(縣市)</option>"       
				SXX10=" onclick=""Srcounty10onclick()""  "
			Else
				sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(5) & "' " 
				SXX10=""
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
		<td bgcolor="silver" COLSPAN=5>
			<select size="1" name="key5" <%=fieldPa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select2" onchange="Srchangetelzip">
				<%=s%>
			</select>
			<input type="text" name="key6" readonly size="8" value="<%=dspkey(6)%>" <%=fieldPa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListDATA">
			<font SIZE=2>(鄉鎮)                 
			<input type="button" width="100%" value="...." id="B6" name="B6" <%=fieldPd%>  <%=SXX10%>  style="Z-INDEX:1" >
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C6" name="C6" <%=fieldPd%>  onclick="SrClear" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >
	        <input type="text" name="key7" value="<%=dspkey(7)%>" size="65" maxlength="60" <%=fieldPa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListENTRY" ID="Text2">
        </td>
	</tr>
	<tr>
		<td width="10%" class=dataListHEAD>備註</td>
		<td bgcolor="silver" height="23" colspan=5>
			<TEXTAREA cols="100%" rows=8 MAXLENGTH=512 <%=fieldPa%> class="dataListentry" <%=FIELDROLE(1)%> <%=dataprotect%> value="<%=dspkey(11)%>" name="key11"><%=dspkey(11)%></TEXTAREA>
		</td>
	</tr>
</table>     
</DIV>


<DIV ID="SRTAG4">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
		<tr><td bgcolor="BDB76B" align="center">客服資料內容</td></tr>
    </table>
</DIV>

<DIV ID=SRTAB4 >  
<table border="1" width="100%" cellpadding="0" cellspacing="0" >
	<tr><td height=0 width="10%"></td><td height=0 width="23%"></td>
		<td height=0 width="10%"></td><td height=0 width="23%"></td>
		<td height=0 width="10%"></td><td height=0 width="23%"></td>
	</tr>

	<tr>
		<td width="10%" class=dataListHEAD>受理時間</td>
		<td width="23%" bgcolor="silver">
			<input type="text" name="key12" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
				value="<%=dspKey(12)%>" READONLY size="25" class=dataListDATA ID="Text3">
		</td>

		<td class="dataListHEAD" width="10%" height="23">受理人員</td>                                 
		<%
			s=""
			sx=" selected "
			If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 and fieldpa="" Then  
			sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC " _
				&"FROM   RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID " _
				&"WHERE  (DEPT='B400' or RTEmployee.emply ='" &dspkey(13)& "') AND TRAN2='' order by RTObj.CUSNC"
			If len(trim(dspkey(13))) < 1 Then
				sx=" selected " 
				s=s & "<option value=""""" & sx & "></option>"  
				sx=""
			else
				s=s & "<option value=""""" & sx & "></option>"  
				sx=""
			end if     
			Else
			sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC " _
				&"FROM   RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID " _
				&"WHERE  (DEPT='B400' or RTEmployee.emply ='" &dspkey(13)& "') AND TRAN2='' AND RTEmployee.emply ='" &dspkey(13)& "' order by RTObj.CUSNC"
			End If
			rs.Open sql,conn
			Do While Not rs.Eof
			If rs("EMPLY")=dspkey(13) Then sx=" selected "
			s=s &"<option value=""" &rs("EMPLY") &"""" &sx &">" &rs("CUSNC") &"</option>"
			rs.MoveNext
			sx=""
			Loop
			rs.Close
		%>
        <td width="23%" bgcolor="silver" colspan=3>
			<select size="1" name="key13" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
				<%=s%>
			</select>
		</td>
	</TR>

	<tr>   
		<td WIDTH="10%" class="dataListHEAD">直銷業務</td>               
		<td width="23%" bgcolor="silver">
			<input type="TEXT" name="key9" size="7" readonly value="<%=dspKey(9)%>"  <%=fieldRole(1)%><%=dataProtect%> class="dataListDATA" ID="Text7">
			<input type="BUTTON" id="B9" name="B9" <%=fieldRole(1)%> <%=fieldpd%>width="100%" style="Z-INDEX: 1"  value="...." onclick="Srsalesonclick()"  >   
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldRole(1)%> <%=fieldpd%>alt="清除" id="C9"  name="C9"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">
			<font size=2><%=SrGetEmployeeName(dspKey(9))%></font>
        </td>
	    <td width="10%" class=dataListHEAD>經銷商</td>
		<%  
			If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 AND FIELDPA = "" Then 
				sql="SELECT RTObj.CUSID,RTObj.SHORTNC " _
					&"FROM RTObj INNER JOIN " _
					&"RTConsignee ON RTObj.CUSID = RTConsignee.CUSID INNER JOIN " _
					&"RTConsigneeCASE ON RTConsigneeCASE.CUSID = RTConsignee.CUSID " _
					&"WHERE (RTConsigneeCASE.CASEID in ('03','06','07','08')) " _
					&"GROUP BY RTObj.CUSID,RTObj.SHORTNC "
				s="<option value="""" >(經銷商)</option>"
			Else
				sql="SELECT RTObj.CUSID,RTObj.SHORTNC " _
					&"FROM RTObj INNER JOIN " _
					&"RTConsignee ON RTObj.CUSID = RTConsignee.CUSID INNER JOIN " _
					&"RTConsigneeCASE ON RTConsigneeCASE.CUSID = RTConsignee.CUSID " _
					&"WHERE (RTConsigneeCASE.CASEID in ('03','06','07','08')) " _
					&"AND rtobj.cusid='" & dspkey(10) & "' " _
					&"GROUP BY RTObj.CUSID,RTObj.SHORTNC "
			End If
			rs.Open sql,conn
			If rs.Eof Then s="<option value="""" >(經銷商)</option>"
			sx=""
			Do While Not rs.Eof
				If rs("CUSID")=dspkey(10) Then sx=" selected "
				s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
				rs.MoveNext
				sx=""
			Loop
			rs.Close        
		%>
		<td width="23%" bgcolor="silver" colspan=3>
			<select size="1" name="key10" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" ID="Select34">                                            
				<%=s%>
			</select>
		</td>   
	</tr>
	<tr>
		<td width="10%" class=dataListHEAD>結案日</td>
		<td width="23%" bgcolor="silver"  >
			<input type="text" name="key14" value="<%=dspKey(14)%>" READONLY size="25" class=dataListData ID="Text8">
		</td>
        <td  class="dataListHEAD" height="23">結案員</td>
        <td  height="23" bgcolor="silver" colspan=3>
			<input type="text" name="key15" size="6" READONLY value="<%=dspKey(15)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text10">
			<font size=2><%=SrGetEmployeeName(dspKey(15))%></font>
        </td>  
	</tr>
	<tr>
		<td width="10%" class=dataListHEAD>作廢日</td>
		<td width="23%" bgcolor="silver"  >
			<input type="text" name="key16" value="<%=dspKey(16)%>" READONLY size="25" class=dataListData ID="Text11">
		</td>
        <td  class="dataListHEAD" height="23">作廢員</td>
        <td  height="23" bgcolor="silver" colspan=3>
			<input type="text" name="key17" size="6" READONLY value="<%=dspKey(17)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text12">
			<font size=2><%=SrGetEmployeeName(dspKey(17))%></font>
        </td>  
	</tr>        
	<tr>
		<td width="10%" class=dataListHEAD>資料修改日</td>
		<td width="23%" bgcolor="silver"  >
			<input type="text" name="key18" value="<%=dspKey(18)%>" READONLY size="25" class=dataListData ID="Text4">
		</td>
        <td  class="dataListHEAD" height="23">資料修改員</td>
        <td  height="23" bgcolor="silver" colspan=3>
			<input type="text" name="key19" size="6" READONLY value="<%=dspKey(19)%>" <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text5">
			<font size=2><%=SrGetEmployeeName(dspKey(19))%></font>
        </td>  
	</tr>        
</table> 
</DIV>


<DIV ID="SRTAG7">
	<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
		<tr><td bgcolor="BDB76B" align="center">追件項目</td></tr>
	</table>
</DIV>

<DIV ID="SRTAB7" > 
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
<tr class="dataListHEAD">
	<td>項次</td><td>追件人員</td><td>追件日期</td><td>備註</td><td>作廢日</td>
</tr>

    <%
		Set rsxx=Server.CreateObject("ADODB.Recordset")
		sqlfaqlist=	"select a.entryno, c.cusnc, a.adddat, a.addmemo, a.canceldat " &_
					"from RTPotCustD a " &_
					"left outer join RTEmployee b inner join RTObj c on c.cusid = b.cusid on b.emply = a.addusr " &_
					"where a.caseno ='" &dspkey(0)&"' "
		rsxx.open sqlfaqlist,conn
		do until rsxx.eof 
	%>
        <tr class="dataListentry">
           <td align=center><%=rsxx("entryno")%>&nbsp;</td>
           <td align=right><%=rsxx("cusnc")%>&nbsp;</td>
           <td align=right><%=rsxx("adddat")%>&nbsp;</td>
           <td align=right><%=rsxx("addmemo")%>&nbsp;</td>
           <td align=right><%=rsxx("canceldat")%>&nbsp;</td>
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
    'Dim conn,rs
    'Set conn=Server.CreateObject("ADODB.Connection")
    'conn.open DSN
    'Set rs=Server.CreateObject("ADODB.Recordset")
    'Set comm=Server.CreateObject("ADODB.Command")
    'DELFAQlist="delete from RTLessorAVScustfaqlist where faqno='" & dspkey(1) & "'"
    'conn.Execute DELFAQlist  
    'For i=0 to 99
    '    if len(trim(extdb(i))) > 0  then
    '       rs.Open "SELECT * FROM RTLessorAVScustfaqlist WHERE faqno='" &dspKey(1) &"' and faqcod='" & extDB(i) & "'" ,conn,3,3
    '       If rs.Eof Or rs.Bof Then
    '          rs.AddNew
    '          rs("cusid")=dspKey(0)
    '         rs("faqno")=dspKey(1)
    '          rs("faqcod")=extDB(i)          
    '       End If
    '       rs.Update
    '       rs.Close
    '    end if
    'Next
    'conn.Close
    'Set rs=Nothing
    'Set conn=Nothing
End Sub
' -------------------------------------------------------------------------------------------- 
' --------------------------------------------------------------------------------------------  
%>
<!-- #include virtual="/Webap/include/checkid.inc" -->
<!-- #include virtual="/Webap/include/companyid.inc" -->
<!-- #include file="RTGetUserRight.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->