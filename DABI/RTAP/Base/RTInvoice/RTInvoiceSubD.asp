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
                runpgm=Request.ServerVariables("PATH_INFO") 
                select case ucase(runpgm)   
                   case ucase("/webap/rtap/base/RTInvoice/RTInvoiceSubD.asp")
                     if i=1 then
                        Set rsc=Server.CreateObject("ADODB.Recordset")
                        rsc.open "select max(entry) AS entry from RTInvoiceSub where invno='" & dspkey(0) & "' ",conn
                        if len(rsc("entry")) > 0 then
                           dspkey(i)=rsc("entry") + 1
                        else
                           dspkey(i)=1
                        end if
					    rsc.close
						Set rsc=Nothing
					 end if
'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
					 rs.fields(i).value=dspkey(i)
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
                 case ucase("/webap/rtap/base/RTInvoice/RTInvoiceSubD.asp")
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
       if ucase(runpgm)=ucase("/webap/rtap/base/RTInvoice/RTInvoiceSubD.asp") then
          rs.open "select max(entry) AS entry from RTInvoiceSub where invno='" & dspkey(0) & "'",conn
          if not rs.eof then
			 dspkey(1)=rs("entry")
		  end if
          rs.close
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
<meta http-equiv="Content-Type" content="text/html; charset=big5" />
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
  title="發票資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT INVNO, ENTRY, PRODNC, QTY, UNITAMT, SALEAMT, TAXAMT, MEMOSUB, UUSR, UDAT " &_
			  "FROM RTInvoiceSub WHERE INVNO ='' "
  sqlList=	  "SELECT INVNO, ENTRY, PRODNC, QTY, UNITAMT, SALEAMT, TAXAMT, MEMOSUB, UUSR, UDAT " &_
			  "FROM RTInvoiceSub WHERE "
  userDefineRead="Yes"
  userDefineSave="Yes"
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
	'項次
    if len(trim(DSPKEY(1))) = 0 THEN DSPKEY(1)=0
	'金額
	IF LEN(TRIM(DSPKEY(3))) = 0 THEN DSPKEY(3)=0
	IF LEN(TRIM(DSPKEY(4))) = 0 THEN DSPKEY(4)=0
	IF LEN(TRIM(DSPKEY(5))) = 0 THEN DSPKEY(5)=0
	IF LEN(TRIM(DSPKEY(6))) = 0 THEN DSPKEY(6)=0

	IF ERRFLAG <> "Y" THEN
'		elseif len(trim(dspkey(1)))=0 or Not Isdate(dspkey(1)) then
'			formValid=False
'			message="發票日期不可空白或格式錯誤"
		elseif len(trim(dspkey(2)))=0 then
			formValid=False
			message="產品名稱不可空白"
'		elseif len(trim(dspkey(3))) <> 0 and len(trim(dspkey(3))) <> 8 then
'			formValid=False
'			message="公司統編須8位數或空白"
'		END IF  
 
	END IF

	'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" or dspmode="修改" then
		'最後修改人員&日期
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                Dspkey(8)=V(0)
        dspkey(9)=now()
    end if
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveXScript()%>
	<SCRIPT Language="VBScript">

	</Script>
<%   
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveX() %>
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<%	
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()
	'申請轉檔日,完工日,報竣日 三者任一欄有值就鎖定fieldPC, fieldPD
    'If len(trim(dspKey(33))) > 0 OR len(trim(dspKey(38))) > 0 OR len(trim(dspKey(39))) > 0 OR len(trim(dspKey(40))) > 0 Then
    '   fieldPC=" class=""dataListData"" readonly "
    '   fieldpD=" disabled "
    'Else
    '   fieldPC=""
    '   fieldpD=""
    'End If%>
    

	<table width="60%" border=1 cellPadding=0 cellSpacing=0>
		<tr><td width="10%" class=dataListHead>發票號碼</td>
			<td width="10%"  bgcolor="silver">
				<input type="text" name="key0" ID="Text0" value="<%=dspKey(0)%>" size="15" readonly <%=fieldRole(1)%> class=dataListdata>
			</td>
			<td WIDTH=10%  class="dataListsearch" height="23">項次</td>                                 
			<td WIDTH=10% height="23" bgcolor="silver" >
				<input type="text" name="key1" ID="Text1" value="<%=dspKey(1)%>" size="3" readonly <%=fieldpC%> class="dataListdata">
			</td>
		</tr>
	</table>

	
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
'    logonid=session("userid")
'    if dspmode="新增" then
'        if len(trim(dspkey(50))) < 1 then
'           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
'                V=split(rtnvalue,";")  
'                dspkey(50)=V(0)
'        End if  
'       dspkey(51)=datevalue(now())
'    else
'        if len(trim(dspkey(52))) < 1 then
'           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
'                V=split(rtnvalue,";")  
'                DSpkey(52)=V(0)
'        End if         
'        dspkey(53)=datevalue(now())
'    end if      
' -------------------------------------------------------------------------------------------- 

    '用戶送件申請後,基本資料 protect
'    If len(trim(dspKey(32))) > 0 OR len(trim(dspKey(38))) > 0 OR len(trim(dspKey(39))) > 0 OR len(trim(dspKey(40))) > 0 Then
'       fieldPa=" class=""dataListData"" readonly "
'       fieldpb=" disabled "
'    Else
'       fieldPa=""
'       fieldpb=""
'    End If


	Dim conn,rs,s,sx,sql,t      
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
%>
                                                    
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
<tr><td width="14%" class=dataListHEAD>產品名稱</td>
	<td width="20%" bgcolor="silver" colspan=3>
        <input type="text" name="key2" ID="Text2" size="100" maxlength="100" class=dataListENTRY
               value="<%=dspKey(2)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> >
	</td>
</tr>

<tr><td width="14%" class=dataListHEAD>數量</td>
	<td width="20%" bgcolor="silver">
        <input type="text" name="key3" ID="Text3" size="6" maxlength=6 class=dataListENTRY
               value="<%=dspKey(3)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> >
	</td>

	<td width="14%" class=dataListHEAD>單價</td>
	<td width="20%" bgcolor="silver" >
        <input type="text" name="key4" ID="Text4" size="10" maxlength="10" class=dataListENTRY 
               value="<%=dspKey(4)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> >
	</td>
</tr>

<tr><td width="14%" class=dataListHEAD>銷售額</td>
	<td width="20%" bgcolor="silver" >
        <input type="text" name="key5" ID="Text5" size="8" maxlength="8" class=dataListENTRY 
               value="<%=dspKey(5)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> >
	</td>

	<td width="14%" class=dataListHEAD>稅額</td>
	<td width="20%" bgcolor="silver" >
        <input type="text" name="key6" ID="Text6" size="6" maxlength="6" class=dataListENTRY 
               value="<%=dspKey(6)%>" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%> >
	</td>
</tr>

<tr><td class="dataListHEAD" height="23">最後異動人員</td>
	<td height="23" bgcolor="silver">
		<input type="text" name="key8" ID="Text8" value="<%=dspKey(8)%>" size="6" READONLY  class="dataListDATA">
		<font size=2><%=SrGetEmployeeName(dspKey(8))%></font>
	</td>

	<td class="dataListHEAD" height="23">最後異動日期</td>
	<td height="23" bgcolor="silver">
        <input type="text" name="key9" ID="Date9" value="<%=dspKey(9)%>" size="30" READONLY class="dataListDATA">
	</td>
</tr>

<tr><td WIDTH="12%" class="dataListHEAD" height="23">備註</td>
	<TD align="CENTER" bgcolor="silver" colspan=3>
		<TEXTAREA cols="100%" name="key7" ID="Textarea7" rows=8  MAXLENGTH=250 class="dataListentry"
			<%=dataprotect%>  value="<%=dspkey(7)%>" ><%=dspkey(7)%>
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
    Dim conn, rs, strsql, sale, tax, total
    Set conn=Server.CreateObject("ADODB.Connection")
    SET RS=Server.CreateObject("ADODB.RECORDSET")  
    Conn.Open DSN
    
	strsql="select sum(saleamt) as sale, sum(taxamt) as tax, sum(saleamt) + sum(taxamt) as total " &_
		   "from RTInvoiceSub where invno = '"& dspkey(0) & "' "
    rs.Open strsql,conn
    If not rs.Eof Then	  
		sale = rs("sale")
		tax = rs("tax")
		total = rs("total")
	else
		sale = 0
		tax = 0
		total = 0
	end if
	rs.close
	
	strsql="update RTInvoice set salesum="& sale &", taxsum ="& tax &", totalsum="& total &_
		   ",amtc = '"& right("0000000" & cstr(total), 8) &"' where invno = '"& dspkey(0) & "' "
	conn.Execute strsql
	'Set ObjRS = conn.Execute(strsql)

    conn.Close
    Set rs=Nothing
    Set conn=Nothing
End Sub
' -------------------------------------------------------------------------------------------- 
%>
<!-- #include virtual="/Webap/include/checkid.inc" -->
<!-- #include virtual="/Webap/include/companyid.inc" -->
<!-- #include file="RTGetUserRight.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
