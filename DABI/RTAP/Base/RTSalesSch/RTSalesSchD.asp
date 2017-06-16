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
  '修改一
  Dim aryParmKey, aryParm
 '90/09/03 ADD-START
 '增加EXTDBFIELD2,EXTDBFILELD3(多檔維護)
  dim extDBField2,extDB2(150),extDBField3,extDB3(150),extDBField4,extDB4(150)
  extDBfield2=0
  extDBField3=0
  extDBField4=0
 '----90/09/03 add-end
  extDBField=0
  aryParmKey=Split(Request("Key") &";;;;;;;;;;;;;;;",";")
  '修改二
  'aryParm=Split(Request("parm") &";;;;;;;;;;;;;;;",";")
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
'response.write "aryKeyType(" &i& ")=" & aryKeyType(i) &"<BR>"
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
              If Instr(cTypeNumeric,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=0
			  If Instr(cTypeChar,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=""
              If Instr(cTypeDate,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=Null
              '   On Error Resume Next
                   ' 當程式為ADSL社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
                       if i=0 then
                          Set rsc=Server.CreateObject("ADODB.Recordset")
						  cusidxx="S" & datePART("yyyy",NOW()) & right("0" & trim(datePART("m",NOW())),2)
                          rsc.open "select isnull(max(schno),'') AS maxschno from RTSalesSch where schno LIKE '" & cusidxx & "%' " ,conn
                          if len(trim(rsc("maxschno"))) > 0 then
                             dspkey(0)=cusidxx & right("000" & cstr(cint(right(rsc("maxschno"),4)) + 1),4)
                          else
                             dspkey(0)=cusidxx & "0001"
                          end if
                          rsc.close
                       end if 
'response.write "I=" & i & ";VALUE=" & dspkey(i) & "　　;type=" & sType & "<BR>"
                       rs.Fields(i).Value=dspKey(i)                       
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
         '     On Error Resume Next
              'runpgm=Request.ServerVariables("PATH_INFO") 
              'select case ucase(runpgm)   
                 ' 當程式為ADSL社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
              '   case ucase("/webap/rtap/base/HBservice/RTFaqD.asp")
                    'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                     rs.Fields(i).Value=dspKey(i)    
              '   case else
              '       rs.Fields(i).Value=dspKey(i)
              ' end select
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
       'if ucase(runpgm)=ucase("/webap/rtap/base/HBservice/RTFaqD.asp") then
          cusidxx=right("0" & datePART("yyyy",NOW())-1911,3) & right("0" & trim(datePART("m",NOW())),2)
		  rsc.open "select isnull(max(schno),'') AS maxschno from RTSalesSch where schno LIKE '" & cusidxx & "%' " ,conn
          if not rsC.eof then
            dspkey(0)=rsC("maxschno")
          end if
          rsC.close
       'end if
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
<meta http-equiv="Content-Type" content="text/html; charset=big5">
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
  title="客服資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB=	"SELECT SCHNO, COMTYPE, COMQ1, LINEQ1, CUSID, ENTRYNO, WORKNO, DEALUSR, " &_
				"DEALDAT, MEMO, UUSR, UDAT, CANCELUSR, CANCELDAT, SCORE01, " &_
				"SCORE02, SCORE03, SCORE04, SCORE05, SCORE06, SCORE07, SCORE08, " &_ 
				"SCORE09, SCORE10, SCORE11, SCORE12, SCORE13, SCORE14, SCORE15, SCORE16 " &_
				"FROM RTSalesSch " &_
				"WHERE schno ='' "
  sqlList=		"SELECT SCHNO, COMTYPE, COMQ1, LINEQ1, CUSID, ENTRYNO, WORKNO, DEALUSR, " &_
				"DEALDAT, MEMO, UUSR, UDAT, CANCELUSR, CANCELDAT, SCORE01, " &_
				"SCORE02, SCORE03, SCORE04, SCORE05, SCORE06, SCORE07, SCORE08, " &_ 
				"SCORE09, SCORE10, SCORE11, SCORE12, SCORE13, SCORE14, SCORE15, SCORE16 " &_
				"FROM RTSalesSch " &_
				"WHERE "
  userDefineRead="Yes"      
  userDefineSave="Yes"       
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=99
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
	'---檢查 派工單號是否有重複 DSPKEY(6)---------
	IF LEN(TRIM(DSPKEY(6))) > 0 and dspmode="新增" then 
		Set connxx=Server.CreateObject("ADODB.Connection")  
		Set rsxx=Server.CreateObject("ADODB.Recordset")
		DSNXX="DSN=RTLIB"
		connxx.Open DSNxx
		sqlXX="SELECT count(*) AS CNT FROM RTSalesSch where workno='"& dspkey(6) &"' and workno<>'' and canceldat is null "
		rsxx.Open sqlxx,connxx
		'Response.Write "CNT=" & RSXX("CNT")
		If RSXX("CNT") > 0 Then
			message="此派工單已入行程表, 不可重複輸入!"
		 	formvalid=false
		End If
		rsxx.Close
		Set rsxx=Nothing
		connxx.Close
		Set connxx=Nothing
	end if
  'If len(dspKey(20))=0 Then dspKey(20) =""      '客戶來電詢問方案
  'If len(dspKey(2))=0 Then dspKey(2) = 0        'comq1
  'If len(dspKey(3))=0 Then dspKey(3) = 0        'lineq1
  'If len(dspKey(5))=0 Then dspKey(5) = 0        'entryno

  if len(trim(dspkey(7)))=0 then
       formValid=False
       message="[處理人員]不可空白"   
  end if

'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(10)=V(0)
        		dspkey(11)=now()
    end if
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveXScript()%>
   <SCRIPT Language="VBScript">
  Sub SrBtnOnClick()
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

'   Sub SrClear()
'       Dim ClickID
'       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
'       clickkey="C" & clickid
'       clearkey="key" & clickid    
'       if len(trim(document.all(clearkey).value)) <> 0 then
'          document.all(clearkey).value =  ""
'       end if
'   End Sub    

	'處理人搜尋
   Sub Srsalesonclick()
       prog="RTGetsalesD.asp"
       'prog=prog & "?KEY=" & ";"
       prog=prog & "?KEY=internal;"
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
			FUsrID=Split(Fusr,";")
       		if Fusrid(2) ="Y" then
       			clickkey="KEY" & mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
          		document.all(clickkey).value = trim(Fusrid(0))
       			if clickkey = "KEY7" then
          			document.all("colFinisheng").value =  trim(Fusrid(1))
				end if
       		End if
       end if
   End Sub

	'用戶&社區&工單搜尋鈕
   Sub SrSearchOnClick()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,4,len(window.event.srcElement.id)-1)
	   colComn = document.all("colComn").VALUE
	   colCusnc = document.all("colCusnc").VALUE
	   'if colComn ="" then colComn ="*"
       prog="RTGetFaq.asp?KEY=" & ClickID &";"& colComn &";"& colCusnc 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:630px;dialogHeight:480px;")
       'FUsr=Window.open(prog,"d2","resizable=yes", true)
       if fusr <> "" then 
			FUsrID=Split(Fusr,";")
			if Fusrid(0) ="Y" then
				document.all("key1").value =  trim(Fusrid(1))
				document.all("key2").value =  trim(Fusrid(2))
				document.all("key3").value =  trim(Fusrid(3))
				document.all("key4").value =  trim(Fusrid(4))
				document.all("key5").value =  trim(Fusrid(5))
				document.all("key6").value =  trim(Fusrid(38))	'工單號

				document.all("colComtypenc").value =  trim(Fusrid(6))
				'document.all("colBelongnc").value =  trim(Fusrid(7))
				document.all("colSalesnc").value = "["& trim(Fusrid(7)) &"] "& trim(Fusrid(8))
				document.all("colComq").value =  trim(Fusrid(9))
				document.all("colComn").value =  trim(Fusrid(10))
				document.all("colLinetel").value =  trim(Fusrid(11))
				document.all("colCmtyip").value =  trim(Fusrid(12))
				document.all("colLinerate").value =  trim(Fusrid(13))
				document.all("colArrivedat").value =  trim(Fusrid(14))
				document.all("colRcomdrop").value =  trim(Fusrid(15))
				document.all("colCusnc").value =  trim(Fusrid(16))
				document.all("colContacttel").value =  trim(Fusrid(17))
				document.all("colCompanytel").value =  trim(Fusrid(18))
				document.all("colRaddr").value =  trim(Fusrid(19))
				document.all("colCustip").value =  trim(Fusrid(20))
				document.all("colCasekind").value =  trim(Fusrid(21))
				document.all("colPaycycle").value =  trim(Fusrid(22))
				document.all("colPaytype").value =  trim(Fusrid(23))
				document.all("colOverdue").value =  trim(Fusrid(24))
				document.all("colFreecode").value =  trim(Fusrid(25))
				document.all("colDocketdat").value =	trim(Fusrid(26))
				document.all("colStrbillingdat").value =trim(Fusrid(27))
				document.all("colNewbillingdat").value =trim(Fusrid(28))
				document.all("colDuedat").value	=  trim(Fusrid(29))				
				document.all("colDropdat").value =  trim(Fusrid(30))
				document.all("colCanceldat").value =  trim(Fusrid(31))
				document.all("colSecondcase").value =  trim(Fusrid(32))
				document.all("colIdslamip").value =  trim(Fusrid(33))
				document.all("colGateway").value =  trim(Fusrid(34))
				document.all("colNciccusno").value =  trim(Fusrid(35))
				document.all("colSp499cons").value =  trim(Fusrid(36))
				document.all("colWtlApplyDat").value =  trim(Fusrid(37))
				document.all("colFaqman").value =  trim(Fusrid(39))
				document.all("colFaqreason").value =  trim(Fusrid(40))
				document.all("colFaqmemo").value =  trim(Fusrid(41))
				document.all("colFinishmemo").value =  trim(Fusrid(42))
			End if       
       end if
   End Sub

	'社區資料鈕
	sub btnViewOnClick()
		dim comtype, comq1, lineq1, cusid, entryno, XRND, ClickID
		comtype = document.all("key1").value
		comq1 = document.all("key2").value
		lineq1 = document.all("key3").value
		cusid = document.all("key4").value
		entryno = document.all("key5").value
        ClickID = window.event.srcElement.id
		Randomize
		XRND = RND()
		if ClickID = "btnViewComq1" then
			select case comtype
				case "3"
					prog = "/webap/rtap/base/rtsparqadslcmty/RTCmtyK.asp?V=" &XRND& "&accessMode=I&key=" & comq1 &";"
				case "6"
					prog = "/webap/rtap/base/rtsparq499cmty/RTsparq499CmtyK.asp?V=" &XRND& "&accessMode=I&key=" & comq1 &";"
				case "7"
					prog = "/webap/rtap/base/RTlessorAVSCMTY/RTLessorAVSCmtyK2.asp?V=" &XRND& "&accessMode=I&key=" & comq1 &";"
				case "8"
					prog = "/webap/rtap/base/RTlessorCMTY/RTLessorCmtyK2.asp?V=" &XRND& "&accessMode=I&key=" & comq1 &";"
				case "9"
					prog = "/webap/rtap/base/RTPrjCmty/RTPrjCmtyK2.asp?V=" &XRND& "&accessMode=I&key=" & comq1 &";"
				case "A"
					prog = "/webap/rtap/base/RTSonetCMTY/RTSonetCmtyK2.asp?V=" &XRND& "&accessMode=I&key=" & comq1 &";"
				case else
					prog =""
			end select
		elseif ClickID = "btnViewLineq1" then
			select case comtype
				case "6"
					prog = "/webap/rtap/base/rtsparq499cmty/RTSparq499cmtylineK.asp?V=" &XRND& "&accessMode=I&key=" & comq1 &";" &lineq1 &";"
				case "7"
					prog = "/webap/rtap/BASE/RTlessorAVSCMTY/RTLessorAVSCmtyLineK.asp?V=" &XRND& "&accessMode=I&key="& comq1 &";"& lineq1 &";"
				case "8"
					prog = "/webap/rtap/BASE/RTlessorCMTY/RTLessorCmtyLineK.asp?V=" &XRND& "&accessMode=I&key="& comq1 &";"& lineq1 &";"
				case "9"
					prog = "/webap/rtap/BASE/RTPrjCmty/RTPrjCmtyLineK.asp?V=" &XRND& "&accessMode=I&key="& comq1 &";"& lineq1 &";"
				case "A"
					prog = "/webap/rtap/BASE/RTSonetCMTY/RTSonetCmtyLineK2.asp?V=" &XRND& "&accessMode=I&key="& comq1 &";"& lineq1 &";"
				case else
					prog =""       
			end select
		elseif ClickID = "btnViewCusid" then
			select case comtype
				case "3"
					prog = "/webap/rtap/base/rtsparqadslcmty/RTCustK.asp?V=" &XRND& "&faq="& comq1 &";"& cusid &";"& entryno &";"
				case "6"
					prog = "/webap/rtap/base/rtsparq499cmty/RTSparq499CustK.asp?V=" &XRND& "&accessMode=I&key=" & comq1 &";" &lineq1 &";" &cusid &";"
				case "7"
					prog = "/webap/rtap/BASE/RTlessorAVSCMTY/RTLessorAVSCustK.asp?V=" &XRND& "&accessMode=I&key="& comq1 &";"& lineq1 &";"& cusid &";"
				case "8"
					prog = "/webap/rtap/BASE/RTlessorCMTY/RTLessorCustK.asp?V=" &XRND& "&accessMode=I&key="& comq1 &";"& lineq1 &";"& cusid &";"
				case "9"
					prog = "/webap/rtap/BASE/RTPrjCmty/RTPrjCustK.asp?V=" &XRND& "&accessMode=I&key="& comq1 &";"& lineq1 &";"& cusid &";"
				case "A"
					prog = "/webap/rtap/BASE/RTSonetCMTY/RTSonetCustK2.asp?V=" &XRND& "&accessMode=I&key="& comq1 &";"& lineq1 &";"& cusid &";"
				case else
					prog =""       
			end select
		end if
		StrFeature=	"Top=0,left=0,scrollbars=yes,status=yes,location=no,menubar=no," &_
					"width=" & window.screen.width -7 &"px, height="& window.screen.height -74 & "px"
		FUsr=Window.open(prog,"d2", StrFeature)
	end sub

	sub colOnkeypress()
		if window.event.keycode =13 then 
			document.all("Btn" & mid(window.event.srcElement.id,4,len(window.event.srcElement.id)-1)).focus
		end if
	end sub
	'--------------------------------------------------------------------------------------------------
  
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

   'Sub SrChangeCustSrc()
   '     s21 = document.all("key21").value
   '         '元訊用戶
   '     if s21 ="01" then
   '         window.SRTAB0.style.display=""
   '         window.SRTAB2.style.display=""
   '     else
   '         ' 社區潛戶 -> 關閉,cusid,entryno
   '         window.SRTAB0.style.display=""
   '         window.SRTAB2.style.display="none"
   '         document.all("colCusnc").value =""
   '         document.all("colRaddr").value =""
   '         document.all("colContacttel").value =""
   '         document.all("colCompanytel").value =""
   '         document.all("key4").value =""
   '         document.all("key5").value =""
   '         document.all("colFreecode").value =""
   '         document.all("colCustip").value =""
   '         document.all("colCasekind").value =""
   '         document.all("colPaytype").value =""
            ' document.all("colPaycycle").value =""
            ' document.all("colDocketdat").value =""
            ' document.all("colDropdat").value =""
            ' document.all("colOverdue").value =""
            ' document.all("colStrbillingdat").value =""
            ' document.all("colNewbillingdat").value =""
            ' document.all("colSecondcase").value =""
            ' document.all("colDuedat").value =""
            ' document.all("colCanceldat").value =""
            ' document.all("colNciccusno").value =""
            ' document.all("colSp499cons").value =""
            ' document.all("colWtlApplydat").value =""
            ' 其他潛戶 -> 再關閉comtype,comq1,lineq1
            ' if s21 ="03" then   
                ' window.SRTAB0.style.display="none"
                ' document.all("colCOMN").value =""
                ' document.all("colComq").value =""
                ' document.all("key2").value =""
                ' document.all("key3").value =""
                ' document.all("key1").value =""
                ' document.all("colComtypenc").value =""
                ' document.all("colSalesnc").value =""
                ' document.all("colCmtyip").value =""
                ' document.all("colGateway").value =""
                ' document.all("colIdslamip").value =""
                ' document.all("colLinerate").value =""
                ' document.all("colLinetel").value =""
                ' document.all("colArrivedat").value =""
                ' document.all("colRcomdrop").value =""
            ' end if
        ' end if
   ' End Sub
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
		<tr><td width="10%" class=dataListHead>行程單號</td>
			<td width="15%"  bgcolor="silver">
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
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
			V=split(rtnvalue,";")  
			dspkey(7)=V(0)		'處理人員
			dspkey(8)=datevalue(now())	'處理日期
			DSpkey(10)=V(0)
			dspkey(11)=now()
		'修改三
		' dspkey(1) = aryParm(0)
		' dspkey(2) = aryParm(1)
		' dspkey(3) = aryParm(2)
		' dspkey(4) = aryParm(3)
		' dspkey(5) = aryParm(4)

    ' elseif dspmode="修改" then
          ' Call SrGetEmployeeRef(Rtnvalue,1,logonid)
               ' V=split(rtnvalue,";")  
               ' DSpkey(10)=V(0)
    end if
' -------------------------------------------------------------------------------------------- 

    '客服單結案後 protect
    ' If len(trim(dspKey(14))) > 0  Then
       ' fieldPa=" class=""dataListData"" readonly "
    ' Else
       ' fieldPa=""        
    ' end if

    ' If accessMode <>"A" Then
       ' fieldpb=" disabled "
    ' Else
       ' fieldpb=""
    ' End If
      
%>

<span id="tags1" class="dataListTagsOn">行程資訊</span>
                                                            
<div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td>
<td>     

<%
'處理對象為社區
if dspkey(4) ="" and len(dspkey(2))>0 then
	select case dspkey(1)
		case "1", "4"
			sql="select	j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, convert(varchar(5), b.comq1) as comq, b.comn, b.t1attachtel as LINETEL, '' as gateway, " &_
				"e.snmpip as CMTYIP, f.codenc as LINERATE, b.t1arrive as ARRIVEDAT, b.rcomdrop, '' as idslamip, " &_
				"'' as cusnc, '' as contacttel, '' as companytel, '' as raddr, '' as CUSTIP, '' as CASEKIND, '' as paycycle, " &_
				"'' as paytype, '' as overdue, '' as freecode, " &_
				"null as docketdat, null as strbillingdat, null as newbillingdat, null as duedat, null as dropdat, null as canceldat, " &_
				"'' as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat " &_
				"from RTCmty b " &_
				"left outer join HBAdslCmty c on b.comq1 = c.comq1 " &_
				"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
				"left outer join RTsnmp e on e.comq1 = b.comq1 and e.comkind ='3' " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"where (c.comtype ='1' or c.comtype ='4') " &_
				"and b.comq1 =" &dspkey(2)
		case "2"
			sql="select j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, convert(varchar(5), b.cutyid) as comq, b.comn, b.cmtytel as LINETEL, '' as gateway, " &_
				"b.ipaddr as CMTYIP, f.codenc as LINERATE, b.linearrive as ARRIVEDAT, b.rcomdrop, '' as idslamip, " &_
				"'' as cusnc, '' as contacttel, '' as companytel, '' as raddr, '' as CUSTIP, '' as CASEKIND, '' as paycycle, " &_
				"'' as paytype, '' as overdue, '' as freecode, " &_
				"null as docketdat, null as strbillingdat, null as newbillingdat, null as duedat, null as dropdat, null as canceldat, " &_
				"'' as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat " &_
				"from RTCustAdslCmty b " &_
				"left outer join HBAdslCmty c on b.cutyid = c.comq1 " &_
				"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"where c.comtype ='2' " &_
				"and b.cutyid =" &dspkey(2)
		case "3"
			sql="select j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, convert(varchar(5), b.cutyid) as comq, b.comn, b.cmtytel as LINETEL, '' as gateway, " &_
				"b.ipaddr as CMTYIP, f.codenc as LINERATE, b.linearrive as ARRIVEDAT, b.rcomdrop, '' as idslamip, " &_
				"'' as cusnc, '' as contacttel, '' as companytel, '' as raddr, '' as CUSTIP, '' as CASEKIND, '' as paycycle, " &_
				"'' as paytype, '' as overdue, '' as freecode, " &_
				"null as docketdat, null as strbillingdat, null as newbillingdat, null as duedat, null as dropdat, null as canceldat, " &_
				"'' as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat " &_
				"from RTSparqAdslCmty b " &_
				"left outer join HBAdslCmty c on b.cutyid = c.comq1 " &_
				"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"where 	c.comtype ='3' " &_
				"and b.cutyid =" &dspkey(2)
		case "5"
			sql="select j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, convert(varchar(5), b.comq1)+'-'+convert(varchar(5), b.lineq1) as comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
				"b.lineip as CMTYIP, f.codenc as LINERATE, b.hinetnotifydat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
				"'' as cusnc, '' as contacttel, '' as companytel, '' as raddr, '' as CUSTIP, '' as CASEKIND, '' as paycycle, " &_
				"'' as paytype, '' as overdue, '' as freecode, " &_
				"null as docketdat, null as strbillingdat, null as newbillingdat, null as duedat, null as dropdat, null as canceldat, " &_
				"'' as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat " &_
				"from RTEbtCmtyLine b " &_
				"inner join RTEbtCmtyH d on d.comq1 = b.comq1 " &_
				"left outer join HBAdslCmty c on b.comq1 = c.comq1 and b.lineq1 = c.lineq1 " &_
				"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"where c.comtype ='5' " &_
				"and b.comq1 = "&dspkey(2)&" and b.lineq1 ="&dspkey(3)
		case "6"
			sql="select j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, convert(varchar(5), b.comq1)+'-'+convert(varchar(5), b.lineq1) as comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
				"replace(str(b.lineipstr1) +'.'+ str(b.lineipstr2) +'.'+ str(b.lineipstr3) +'.'+ str(b.lineipstr4) +'~'+ str(b.lineipend),' ','')  as CMTYIP, " &_
				"f.codenc as LINERATE, b.linearrivedat as ARRIVEDAT, b.dropdat as RCOMDROP, b.idslamip, " &_
				"'' as cusnc, '' as contacttel, '' as companytel, '' as raddr, '' as CUSTIP, '' as CASEKIND, '' as paycycle, " &_
				"'' as paytype, '' as overdue, '' as freecode, " &_
				"null as docketdat, null as strbillingdat, null as newbillingdat, null as duedat, null as dropdat, null as canceldat, " &_
				"'' as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat " &_
				"from RTSparq499CmtyLine b " &_
				"inner join RTSparq499CmtyH d on d.comq1 = b.comq1 " &_
				"left outer join HBAdslCmty c on b.comq1 = c.comq1 and b.lineq1 = c.lineq1 " &_
				"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"where 	c.comtype ='6' " &_
				"and b.comq1 = "&dspkey(2)&" and b.lineq1 ="&dspkey(3)
		case "7"
			sql="select	j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, convert(varchar(5), b.comq1)+'-'+convert(varchar(5), b.lineq1) as comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
				"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
				"'' as cusnc, '' as contacttel, '' as companytel, '' as raddr, '' as CUSTIP, '' as CASEKIND, '' as paycycle, " &_
				"'' as paytype, '' as overdue, '' as freecode, " &_
				"null as docketdat, null as strbillingdat, null as newbillingdat, null as duedat, null as dropdat, null as canceldat, " &_
				"'' as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat " &_
				"from RTLessorAvsCmtyLine b " &_
				"inner join RTLessorAvsCmtyH d on d.comq1 = b.comq1 " &_
				"left outer join HBAdslCmty c on b.comq1 = c.comq1 and b.lineq1 = c.lineq1 " &_
				"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"where 	c.comtype ='7' " &_
				"and b.comq1 = "&dspkey(2)&" and b.lineq1 ="&dspkey(3)
		case "8"
			sql="select	j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, convert(varchar(5), b.comq1)+'-'+convert(varchar(5), b.lineq1) as comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
				"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
				"'' as cusnc, '' as contacttel, '' as companytel, '' as raddr, '' as CUSTIP, '' as CASEKIND, '' as paycycle, " &_
				"'' as paytype, '' as overdue, '' as freecode, " &_
				"null as docketdat, null as strbillingdat, null as newbillingdat, null as duedat, null as dropdat, null as canceldat, " &_
				"'' as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat " &_
				"from RTLessorCmtyLine b " &_
				"inner join RTLessorCmtyH d on d.comq1 = b.comq1 " &_
				"left outer join HBAdslCmty c on b.comq1 = c.comq1 and b.lineq1 = c.lineq1 " &_
				"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"where 	c.comtype ='8' " &_
				"and b.comq1 = "&dspkey(2)&" and b.lineq1 ="&dspkey(3)
		case "9"
			sql="select	j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, convert(varchar(5), b.comq1)+'-'+convert(varchar(5), b.lineq1) as comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
				"b.lineip  as CMTYIP, f.codenc as LINERATE, b.arrivedat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
				"'' as cusnc, '' as contacttel, '' as companytel, '' as raddr, '' as CUSTIP, '' as CASEKIND, '' as paycycle, " &_
				"'' as paytype, '' as overdue, '' as freecode, " &_
				"null as docketdat, null as strbillingdat, null as newbillingdat, null as duedat, null as dropdat, null as canceldat, " &_
				"'' as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat " &_
				"from RTPrjCmtyLine b " &_
				"inner join RTPrjCmtyH d on d.comq1 = b.comq1 " &_
				"left outer join HBAdslCmty c on b.comq1 = c.comq1 and b.lineq1 = c.lineq1 " &_
				"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"where 	c.comtype ='9' " &_
				"and b.comq1 = "&dspkey(2)&" and b.lineq1 ="&dspkey(3)
		case "A"
			sql="select	j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, convert(varchar(5), b.comq1)+'-'+convert(varchar(5), b.lineq1) as comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
				"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
				"'' as cusnc, '' as contacttel, '' as companytel, '' as raddr, '' as CUSTIP, '' as CASEKIND, '' as paycycle, " &_
				"'' as paytype, '' as overdue, '' as freecode, " &_
				"null as docketdat, null as strbillingdat, null as newbillingdat, null as duedat, null as dropdat, null as canceldat, " &_
				"'' as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat " &_
				"from RTSonetCmtyLine b " &_
				"inner join RTSonetCmtyH d on d.comq1 = b.comq1 " &_
				"left outer join HBAdslCmty c on b.comq1 = c.comq1 and b.lineq1 = c.lineq1 " &_
				"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"where 	c.comtype ='A' " &_
				"and b.comq1 = "&dspkey(2)&" and b.lineq1 ="&dspkey(3)
		case else
			sql=""	
	end select
'處理對象為客戶
elseif len(dspkey(4))>0	then
	select case dspkey(1)
		case "1", "4"
			sql="select	c.comtypenc, c.belongnc, c.salesnc, c.comq, b.comn, b.t1attachtel as LINETEL, '' as gateway, " &_
				"e.snmpip as CMTYIP, f.codenc as LINERATE, b.t1arrive as ARRIVEDAT, b.rcomdrop, '' as idslamip, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, a.usekind as CASEKIND, '' as paycycle, " &_
				"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
				"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
				"'' as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat " &_
				"from 	RTCust a " &_
				"inner join RTCmty b on a.comq1 = b.comq1 " &_
				"left outer join HBAdslCmtyCust c on a.cusid = c.cusid and a.entryno = c.entryno " &_
				"left outer join RTsnmp e on e.comq1 = b.comq1 and e.comkind ='3' " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"where (c.comtype ='1' or c.comtype ='4') " &_
				"and a.cusid = '" &dspkey(4)& "' and a.entryno =" &dspkey(5)
		case "2"
			sql="select c.comtypenc, c.belongnc, c.salesnc, c.comq, b.comn, b.cmtytel as LINETEL, '' as gateway, " &_
				"b.ipaddr as CMTYIP, f.codenc as LINERATE, b.linearrive as ARRIVEDAT, b.rcomdrop, '' as idslamip, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, a.usekind as CASEKIND, '' as paycycle, " &_
				"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
				"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
				"'' as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat " &_
				"from 	RTCustAdsl a " &_
				"inner join RTCustAdslCmty b on a.comq1 = b.cutyid " &_
				"left outer join HBAdslCmtyCust c on a.cusid = c.cusid and a.entryno = c.entryno " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"where 	c.comtype ='2' " &_
				"and a.cusid = '" &dspkey(4)& "' and a.entryno =" &dspkey(5)
		case "3"
			sql="select c.comtypenc, c.belongnc, c.salesnc, c.comq, b.comn, b.cmtytel as LINETEL, '' as gateway, " &_
				"b.ipaddr as CMTYIP, f.codenc as LINERATE, b.linearrive as ARRIVEDAT, b.rcomdrop, '' as idslamip, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, a.usekind as CASEKIND, '' as paycycle, " &_
				"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
				"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
				"'' as secondcase, a.exttel +'-'+ a.sphnno as nciccusno, '' as Sp499cons, null as WtlApplyDat " &_
				"from 	RTSparqAdslCust a " &_
				"inner join RTSparqAdslCmty b on a.comq1 = b.cutyid " &_
				"left outer join HBAdslCmtyCust c on a.cusid = c.cusid and a.entryno = c.entryno " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"where 	c.comtype ='3' " &_
				"and a.cusid = '" &dspkey(4)& "' and a.entryno =" &dspkey(5)
		case "5"
			sql="select c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
				"b.lineip as CMTYIP, f.codenc as LINERATE, b.hinetnotifydat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, '' as CUSTIP, g.codenc as CASEKIND, h.codenc as paycycle, " &_
				"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
				"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
				"'' as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat " &_
				"from 	RTEbtCust a " &_
				"inner join RTEbtCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
				"inner join RTEbtCmtyH d on d.comq1 = b.comq1 " &_
				"left outer join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"left outer join RTCode g on g.code = a.casetype and g.kind ='H5' " &_
				"left outer join RTCode h on h.code = a.paytype and h.kind ='G6' " &_
				"where 	c.comtype ='5' " &_
				"and a.comq1 = "&dspkey(2)&" and a.lineq1 ="&dspkey(3)&" and a.cusid ='" &dspkey(4)& "' "
		case "6"
			sql="select c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
				"replace(str(b.lineipstr1) +'.'+ str(b.lineipstr2) +'.'+ str(b.lineipstr3) +'.'+ str(b.lineipstr4) +'~'+ str(b.lineipend),' ','')  as CMTYIP, " &_
				"f.codenc as LINERATE, b.linearrivedat as ARRIVEDAT, b.dropdat as RCOMDROP, b.idslamip, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.custip1+'.'+ a.custip2 +'.'+ a.custip3 +'.'+ a.custip4, '...', '') as CUSTIP, " &_
				"g.codenc as CASEKIND, h.codenc as paycycle, " &_
				"'' as paytype, replace(a.overdue,'N','') as overdue, replace(a.freecode,'N','') as freecode, " &_
				"c.docketdat, null as strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
				"'' as secondcase, a.nciccusno +'-'+ a.sphnno as nciccusno, case a.consignee when '12973008' then '原遠端用戶' else '' end as Sp499cons, a.WtlApplyDat " &_
				"from 	RTSparq499Cust a " &_
				"inner join RTSparq499CmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
				"inner join RTSparq499CmtyH d on d.comq1 = b.comq1 " &_
				"left outer join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"left outer join RTCode g on g.code = a.casetype and g.kind ='L9' " &_
				"left outer join RTCode h on h.code = a.paytype and h.kind ='M1' " &_
				"where 	c.comtype ='6' " &_
				"and a.comq1 = "&dspkey(2)&" and a.lineq1 ="&dspkey(3)&" and a.cusid ='" &dspkey(4)& "' "
		case "7"
			sql="select	c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
				"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11+'.'+a.ip12+'.'+a.ip13+'.'+a.ip14,'...','') as CUSTIP, " &_
				"g.codenc as CASEKIND, h.codenc as paycycle, i.codenc as paytype, replace(a.overdue,'N','') as overdue, " &_
				"replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, a.newbillingdat, a.duedat, c.dropdat, c.canceldat, " &_
				"replace(a.secondcase,'N','') as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat " &_
				"from 	RTLessorAvsCust a " &_
				"inner join RTLessorAvsCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
				"inner join RTLessorAvsCmtyH d on d.comq1 = b.comq1 " &_
				"left outer join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"left outer join RTCode g on g.code = a.casekind and g.kind ='O9' " &_
				"left outer join RTCode h on h.code = a.paycycle and h.kind ='M8' " &_
				"left outer join RTCode i on i.code = a.paytype and i.kind ='M9' " &_
				"where 	c.comtype ='7' " &_
				"and a.comq1 = "&dspkey(2)&" and a.lineq1 ="&dspkey(3)&" and a.cusid ='" &dspkey(4)& "' "
		case "8"
			sql="select	c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
				"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11+'.'+a.ip12+'.'+a.ip13+'.'+a.ip14,'...','') as CUSTIP, " &_
				"g.codenc as CASEKIND, h.codenc as paycycle, i.codenc as paytype, replace(a.overdue,'N','') as overdue, " &_
				"replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, a.newbillingdat, a.duedat, c.dropdat, c.canceldat, " &_
				"replace(a.secondcase,'N','') as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat " &_
				"from 	RTLessorCust a " &_
				"inner join RTLessorCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
				"inner join RTLessorCmtyH d on d.comq1 = b.comq1 " &_
				"left outer join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"left outer join RTCode g on g.code = a.casekind and g.kind ='O9' " &_
				"left outer join RTCode h on h.code = a.paycycle and h.kind ='M8' " &_
				"left outer join RTCode i on i.code = a.paytype and i.kind ='M9' " &_
				"where 	c.comtype ='8' " &_
				"and a.comq1 = "&dspkey(2)&" and a.lineq1 ="&dspkey(3)&" and a.cusid ='" &dspkey(4)& "' "
		case "9"
			sql="select	c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
				"b.lineip  as CMTYIP, f.codenc as LINERATE, b.arrivedat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, replace(a.ip11+'.'+a.ip12+'.'+a.ip13+'.'+a.ip14,'...','') as CUSTIP, " &_
				"'' as CASEKIND, '' as paycycle, '' as paytype, '' as overdue, " &_
				"replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
				"'' as secondcase, '' as nciccusno, '' as Sp499cons, null as WtlApplyDat " &_
				"from 	RTPrjCust a " &_
				"inner join RTPrjCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
				"inner join RTPrjCmtyH d on d.comq1 = b.comq1 " &_
				"left outer join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"where 	c.comtype ='9' " &_
				"and a.comq1 = "&dspkey(2)&" and a.lineq1 ="&dspkey(3)&" and a.cusid ='" &dspkey(4)& "' "
		case "A"
			sql="select	c.comtypenc, c.belongnc, c.salesnc, c.comq, d.comn, b.linetel as LINETEL, b.gateway, " &_
				"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, '' as idslamip, " &_
				"c.cusnc, c.contacttel, c.companytel, c.raddr, a.ip11 as CUSTIP, " &_
				"'' as CASEKIND, h.codenc as paycycle, '' as paytype, convert(varchar(10), a.overduedat,111) as overdue, " &_
				"replace(a.freecode,'N','') as freecode, c.docketdat, a.strbillingdat, null as newbillingdat, null as duedat, c.dropdat, c.canceldat, " &_
				"'' as secondcase, a.memberid as nciccusno, '' as Sp499cons, null as WtlApplyDat " &_
				"from 	RTSonetCust a " &_
				"inner join RTSonetCmtyLine b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
				"inner join RTSonetCmtyH d on d.comq1 = b.comq1 " &_
				"left outer join HBAdslCmtyCust c on c.comq1 = a.comq1 and c.lineq1 = a.lineq1 and a.cusid = c.cusid " &_
				"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
				"left outer join RTCode h on h.code = a.paycycle and h.kind ='M8' " &_
				"where 	c.comtype ='A' " &_
				"and a.comq1 = "&dspkey(2)&" and a.lineq1 ="&dspkey(3)&" and a.cusid ='" &dspkey(4)& "' "
				
		case else
			sql=""	
	end select
end if

    Dim conn,rs,s,sx,sql,t
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
    if sql <>"" then 
		rs.OPEN sql,CONN
		IF RS.EOF THEN
			comtypenc =""		:   belongnc =""	:	salesnc =""		:	comq =""	
			comn =""	        :	linetel =""		:	cmtyip =""		:	linerate =""	
			arrivedat =""		:	rcomdrop =""	:	idslamip =""	:	gateway =""
			cusnc =""			:	contacttel =""	:   companytel =""	:	raddr =""
			custip =""			:   casekind =""	:	paycycle =""	:   paytype =""		
			overdue =""			:	freecode =""	:   docketdat =""	:   strbillingdat =""
			newbillingdat =""	:   duedat =""		:	dropdat =""		:   canceldat =""
			nciccusno =""		:	sp499cons =""	:	wtlapplydat =""
		ELSE
			comtypenc =rs("comtypenc")	:	belongnc = rs("belongnc")	:	salesnc = rs("salesnc")
			comq = rs("comq")			:	comn = rs("comn")			:	linetel = rs("linetel")
			cmtyip = rs("cmtyip")       :	linerate = rs("linerate")	:	arrivedat = rs("arrivedat")
			rcomdrop = rs("rcomdrop")	:	idslamip = rs("idslamip")	:	gateway = rs("gateway")
			cusnc = rs("cusnc")			:	casekind = rs("casekind")	:	companytel = rs("companytel")
			raddr = rs("raddr")			:	custip = rs("custip")		:	contacttel = rs("contacttel")
			paycycle = rs("paycycle")	:	paytype = rs("paytype")		:	secondcase = rs("secondcase")
			overdue = rs("overdue")     :	freecode = rs("freecode")	:	strbillingdat = rs("strbillingdat")
			docketdat = rs("docketdat")	:	dropdat = rs("dropdat")		:	newbillingdat = rs("newbillingdat")
			canceldat = rs("canceldat") :	duedat = rs("duedat")		:	nciccusno = rs("nciccusno")
			sp499cons = rs("sp499cons")	:	wtlapplydat = rs("wtlapplydat")
		END IF
		RS.CLOSE
	end if

if dspkey(6) ="" then
		faqreason =""	:	faqmemo =""
		faqman =""		:	finishmemo =""
else
	sql="select b.faqman, c.codenc, b.memo as faqmemo, a.memo as finishmemo " &_
		"from RTSndWork a " &_
		"inner join RTFaqM b on a.linkno = b.caseno " &_
		"left outer join RTCode c on c.code = b.faqreason and c.kind ='P7' " &_
		"where a.workno ='"& dspkey(6) &"' "
	rs.OPEN sql,CONN
		faqreason =rs("codenc")	:	faqmemo =rs("faqmemo")
		faqman =rs("faqman")	:	finishmemo =rs("finishmemo")
	RS.CLOSE
end if
%>
<DIV ID="SRTAG0">
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
	<tr><td bgcolor="BDB76B" align="center">客戶基本資料查詢</td></tr>
</table>
</DIV>

<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3">
	<tr><td width="10%"></td><td width="23%"></td><td width="10%"></td><td width="23%"></td><td width="10%"></td><td width="23%"></td></tr>

	<tr><td width="10%" class=dataListHEAD>來源派工單號</td>
		<td bgcolor="silver" colspan=5 >
			<input type="text" name="key6" readonly size="13" value="<%=dspKey(6)%>" class=dataListdata>
			<%
				if accessMode <>"A" then
					btn = " disabled "
				else
					btn =" "
				end if
			%>
			<input type="button" id="BtnWorkno" name="BtnWorkno" width="100%" value="派工單搜尋(最近7天)" <%=btn%> onclick="SrSearchOnClick()">
		</td>
	</tr>
	<tr><td width="10%" class=dataListsearch>報修原因</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" readonly size="25" name="colFaqreason" value="<%=faqreason%>" class="dataListsearch3" ID="Text3">
		</td>
		<td width="10%" class=dataListsearch>聯絡人</td>
		<td width="23%" bgcolor="silver" colspan=3 >
			<input type="text" readonly size="50" name="colFaqMan" value="<%=faqman%>" class="dataListsearch3" ID="Text2">
		</td>
	</tr>
	<tr>
		<td class=dataListsearch>客訴備註</td>
		<td bgcolor="silver" colspan=5>
			<TEXTAREA cols="100%" readonly name="colFaqMemo" value="<%=faqmemo%>" style="overflow-y:visible;" class="dataListsearch3" ID="Textarea1"><%=faqmemo%></TEXTAREA>
		</td>
	</tr>
	<tr><td class="dataListsearch">施工處理</td>
		<td bgcolor="silver" colspan=5>
			<TEXTAREA cols="100%" readonly name="colFinishMemo" value="<%=finishmemo%>" style="overflow-y:visible;" class="dataListsearch3 ID="Textarea2"><%=finishmemo%></TEXTAREA>
		</td>
	</tr>
	
</table>

<DIV ID="SRTAB0">
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table7">
	<tr>
		<td width="10%" class=dataListsearch>社區名稱</td>
		<td bgcolor="silver" colspan=5 >
			<input type="text" size="42" id="colCOMN" NAME="colCOMN" value="<%=comn%>" style="color=blue;" onkeypress="colOnkeypress()" >
			<input type="button" id="BtnComn" name="BtnComn" value="社區名稱搜尋" onclick="SrSearchOnClick()">
				<input type="button" id="btnViewComq1" name="btnViewComq1" value="社區資料" onclick="btnViewOnClick()" style="color:green;">
				<input type="button" id="btnViewLineq1" name="btnViewLineq1" value="主線資料" onclick="btnViewOnClick()" style="color:green;">
		</td>
	</tr>
	<tr>
	    <td width="10%" class=dataListsearch>主線序號</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" readonly size="7" name="colComq" value="<%=comq%>" class="dataListsearch3" ID="Text22">
			<input type="text" name="key2" readonly size="4" value="<%=dspkey(2)%>" class="dataListDATA" ID="Text1" style="display:none;">
			<input type="text" name="key3" readonly size="3" value="<%=dspkey(3)%>" class="dataListDATA" ID="Text2" style="display:none;">
		</td>
		<td width="10%" class=dataListsearch>方案別</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" name="key1" readonly size="2" value="<%=dspkey(1)%>" class="dataListDATA" ID="Text3" style="display:none;">
			<input type="text" readonly size="10" name="colComtypenc" value="<%=comtypenc%>" class="dataListsearch3" ID="Text42">
		</td>
		<td width="10%" class=dataListsearch>轄區業務</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" readonly size="15" name="colSalesnc" value="[<%=belongnc%>] <%=salesnc%>" class="dataListsearch3" ID="Text15">
		</td>
	</tr>
	<tr>
	    <td width="10%" class=dataListsearch>社區IP</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" readonly size="20" name="colCmtyip" value="<%=cmtyip%>" class="dataListsearch3" ID="Text45">
		</td>
	    <td width="10%" class=dataListsearch>Gateway IP</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" readonly size="20" name="colGateway" value="<%=gateway%>" class="dataListsearch3" ID="Text21">
		</td>
	    <td width="10%" class=dataListsearch>iDSLAM IP</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" readonly size="20" name="colIdslamip" value="<%=idslamip%>" class="dataListsearch3" ID="Text44">
		</td>
	</tr>
	<tr>
	    <td class=dataListsearch>主線速率</td>
		<td bgcolor="silver">
			<input type="text" readonly size="15" name="colLinerate" value="<%=linerate%>" class="dataListsearch3" ID="Text23">
		</td>
	    <td width="10%" class=dataListsearch>附掛電話</td>
		<td width="23%" bgcolor="silver" colspan=3>
			<input type="text" readonly size="15" name="colLinetel" value="<%=linetel%>" class="dataListsearch3" ID="Text16">
		</td>
	</tr>
	<tr><td class=dataListsearch>線路到位日</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="12" name="colArrivedat" value="<%=arrivedat%>" class="dataListsearch3" ID="Text24">
		</td>
	    <td class=dataListsearch>撤線日</td>
		<td bgcolor="silver" colspan=3>
			<input type="text" readonly size="12" name="colRcomdrop" value="<%=rcomdrop%>" class="dataListsearch3" style="color:red;" ID="Text25" >
		</td>
	</tr>
</table>
</DIV>

<DIV ID=SRTAB2 >
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
	<tr>
		<td width="10%" class=dataListsearch>用戶名稱</td>
		<td bgcolor="silver" colspan=5>
			<input type="text" size="42" value="<%=cusnc%>" id ="colCusnc" NAME="colCusnc" style="color=blue;" ID="Text41" onkeypress="colOnKeypress()">
		    <input type="button" id="BtnCusnc" name="BtnCusnc" width="100%" value="用戶名稱搜尋" onclick="SrSearchOnClick()">
				<input type="button" id="btnViewCusid" name="btnViewCusid" value="用戶資料" onclick="btnViewOnClick()" style="color:green;">
				<input type="text" readonly size="12" name="colSp499cons" value="<%=sp499cons%>" style="color=red;" class="dataListsearch3">
		</td>
	</tr>
	<tr>
	    <td width="10%" class=dataListsearch>用戶地址</td>
		<td bgcolor="silver" colspan=3>
			<input type="text" readonly size="55" NAME="colRaddr" id="colRaddr" value="<%=raddr%>" class="dataListsearch3" ID="Text26" >
		</td>
		<td class=dataListsearch>WTL申請</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="20" name="colWtlApplyDat" value="<%=WtlApplyDat%>" class="dataListsearch3" ID="Text35">
		</td>
	</tr>

	<tr style="display:none;">
		<td class=dataListsearch>用戶電話</td>
		<td bgcolor="silver" colspan=3>
			<input type="text" readonly size="50" name="colContacttel" value="<%=contacttel%>" class="dataListsearch3" ID="Text31">			
		</td>
	    <td class=dataListsearch>行動電話</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="29" name="colCompanytel" value="<%=companytel%>" class="dataListsearch3" ID="Text30">
		</td>
	</tr>

	<tr>
		<td width="10%" class=dataListsearch>用戶編號</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" readonly size="15" name="key4" value="<%=dspkey(4)%>" class="dataListsearch3" ID="Text28">
			<font size=2 color=black>項次: </font>
			<input type="text" readonly size="2" name="key5" value="<%=dspkey(5)%>" class="dataListsearch3" ID="Text4">
		</td>
	    <td width="10%" class=dataListsearch>公關機</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" readonly size="2" name="colFreecode" value="<%=freecode%>" class="dataListsearch3" style="color:red;" ID="Text5">
		</td>
	    <td width="10%" class=dataListsearch>用戶IP</td>
		<td width="23%" bgcolor="silver" >
			<input type="text" readonly size="20" name="colCustip" value="<%=custip%>" class="dataListsearch3" ID="Text29">
		</td>
	</tr>	
	<tr>
		<td class=dataListsearch>用戶方案</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="29" name="colCasekind" value="<%=casekind%>" class="dataListsearch3" ID="Text32">
		</td>
	    <td class=dataListsearch>繳費方式</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="15" name="colPaytype" value="<%=paytype%>" class="dataListsearch3" ID="Text33">
		</td>
		<td class=dataListsearch>繳費週期</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="29" name="colPaycycle" value="<%=paycycle%>" class="dataListsearch3" ID="Text34">
		</td>
	</tr>
	<tr>
		<td class=dataListsearch>報竣日</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="25" name="colDocketdat" value="<%=docketdat%>" class="dataListsearch3" ID="Text35">
		</td>
		<td class=dataListsearch>退租日</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="10" name="colDropdat" value="<%=dropdat%>" class="dataListsearch3" style="color:red;" ID="Text38">
		</td>
	    <td class=dataListsearch>欠拆</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="10" name="colOverdue" value="<%=overdue%>" class="dataListsearch3" style="color:red;" ID="Text39">
		</td>
	</tr>
	<tr>
	    <td class=dataListsearch>開始計費</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="10" name="colStrbillingdat" value="<%=strbillingdat%>" class="dataListsearch3" ID="Text36">
		</td>
	    <td class=dataListsearch>續約日</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="10" name="colNewbillingdat" value="<%=newbillingdat%>" class="dataListsearch3" ID="Text37">
		</td>
	    <td class=dataListsearch>第二戶</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="2" name="colSecondcase" value="<%=secondcase%>" class="dataListsearch3" ID="Text14">
		</td>
	</tr>
	<tr>
	    <td class=dataListsearch>到期日</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="10" name="colDuedat" value="<%=duedat%>" class="dataListsearch3"
			<%	if len(duedat)> 0 then
					if dateadd("d",1,duedat) < now() then response.Write "style=""color:red;""" 
				end if
			%> ID="Text43">
		</td>
	    <td class=dataListsearch>作廢日</td>
		<td bgcolor="silver">
			<input type="text" readonly size="25" name="colCanceldat" value="<%=canceldat%>" class="dataListsearch3" style="color:red;" ID="Text40">
		</td>
	    <td class=dataListsearch>對帳代號</td>
		<td bgcolor="silver" >
			<input type="text" readonly size="20" name="colNciccusno" value="<%=nciccusno%>" class="dataListsearch3" ID="Text46">
		</td>
	</tr>
</table>
</DIV>


<DIV ID="SRTAG2">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table4">
		<tr><td bgcolor="BDB76B" align="center">行程內容</td></tr>
    </table>
</DIV>

<DIV ID=SRTAB2 >
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table5">
	<tr>
		<td width="10%" class="dataListHEAD" height="23">處理人員</td>
        <td width="23%" bgcolor="silver">
			<input type="text" name="key7" size="6" readonly value="<%=dspKey(7)%>" class="dataListEntry" ID="Text7">
			<input type="BUTTON" id="B7" name="B7" onclick="Srsalesonclick()" style="Z-INDEX: 1" value="...." >   
			<input type="text" readonly size="10" name="colFinisheng" value="<%=SrGetEmployeeName(dspKey(7))%>" class="dataListsearch3" ID="Text77">
        </td>
		
		<td width="10%" class=dataListHEAD>處理日期</td>
		<td bgcolor="silver" colspan=3>
        	<input type="text" name="key8" value="<%=dspKey(8)%>" READONLY size="12" class=dataListEntry>
       		<input type="button" name="B8" id="B8" height="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
        </td>
	</tr>

	<tr><td width="10%" class="dataListHEAD">異動人員</td>
        <td width="23%" bgcolor="silver">
			<input type="text" name="key10" size="6" READONLY value="<%=dspKey(10)%>" class="dataListDATA" ID="Text10">
			<font size=2><%=SrGetEmployeeName(dspKey(10))%></font>
        </td>

        <td width="10%" class="dataListHEAD">異動時間</td>
        <td bgcolor="silver" colspan=3>
	        <input type="text" name="key11" size="25" READONLY value="<%=dspKey(11)%>" class="dataListDATA" ID="Text11">
        </td>
	</tr>

	<tr><td width="10%" class="dataListHEAD" height="23">作廢人員</td>
        <td width="23%" bgcolor="silver">
			<input type="text" name="key12" size="6" READONLY value="<%=dspKey(12)%>" style="color:red;" class="dataListDATA" ID="Text12">
			<font size=2 color=red><%=SrGetEmployeeName(dspKey(12))%></font>
        </td>
        <td width="10%" class="dataListHEAD" height="23">作廢時間</td>
        <td bgcolor="silver" colspan=3>
	        <input type="text" name="key13" size="25" READONLY value="<%=dspKey(13)%>" style="color:red;" class="dataListDATA" ID="Text13">
        </td>
	</tr>

	<tr><td width="10%" class="dataListHEAD">行程事項</td>
		<td width="90%" bgcolor="silver" colspan=5>

	<table cellpadding="0" cellspacing="0" >
	<tr><td bgcolor="silver">
			<% IF DSPKEY(14)="Y" THEN CHECK14=" CHECKED "%>
			<input type="checkbox" READONLY ID="Text14" name="key14" value="Y" <%=CHECK14%> bgcolor="silver"><font size=2>(2點) 申請AVS、FBB(業務)</font>　　
		</td>
		<td bgcolor="silver">
			<% IF DSPKEY(15)="Y" THEN CHECK15=" CHECKED "%>
			<input type="checkbox" READONLY ID="Text15" name="key15" value="Y" <%=CHECK15%> bgcolor="silver"><font size=2>(1點) 安裝Sonet、SQ、AVS Call Out續約與其他</font>
		</td>
	</tr>
	<tr><td bgcolor="silver">		
			<% IF DSPKEY(16)="Y" THEN CHECK16=" CHECKED "%>
			<input type="checkbox" READONLY ID="Text16" name="key16" value="Y" <%=CHECK16%> bgcolor="silver"><font size=2>(1點) 勸用戶退租與撤線</font>　　　
		</td>
		<td bgcolor="silver">
			<% IF DSPKEY(17)="Y" THEN CHECK17=" CHECKED "%>
			<input type="checkbox" READONLY ID="Text17" name="key17" value="Y" <%=CHECK17%> bgcolor="silver"><font size=2>(1點) 主機安裝</font>
		</td>
	</tr>
	<tr><td bgcolor="silver">
			<% IF DSPKEY(18)="Y" THEN CHECK18=" CHECKED "%>
			<input type="checkbox" READONLY ID="Text18" name="key18" value="Y" <%=CHECK18%> bgcolor="silver"><font size=2>(1點) 主機開通</font>　　　　　　
		</td>
		<td bgcolor="silver">
			<% IF DSPKEY(19)="Y" THEN CHECK19=" CHECKED "%>
			<input type="checkbox" READONLY ID="Text19" name="key19" value="Y" <%=CHECK19%> bgcolor="silver"><font size=2>(1點) 用戶端裝機</font>
		</td>
	</tr>
	<tr><td bgcolor="silver">
			<% IF DSPKEY(20)="Y" THEN CHECK20=" CHECKED "%>
			<input type="checkbox" READONLY ID="Text20" name="key20" value="Y" <%=CHECK20%> bgcolor="silver"><font size=2>(1點) 主機拆回、拆 Port</font>　　　
		</td>
		<td bgcolor="silver">
			<% IF DSPKEY(21)="Y" THEN CHECK21=" CHECKED "%>
			<input type="checkbox" READONLY ID="Text21" name="key21" value="Y" <%=CHECK21%> bgcolor="silver"><font size=2>(1點) 退租拆機</font><br>
		</td>
	</tr>
	<tr><td bgcolor="silver">
			<% IF DSPKEY(22)="Y" THEN CHECK22=" CHECKED "%>
			<input type="checkbox" READONLY ID="Text22" name="key22" value="Y" <%=CHECK22%> bgcolor="silver"><font size=2>(1點) 用戶端設備及文件收回</font>　
		</td>
		<td bgcolor="silver">
			<% IF DSPKEY(23)="Y" THEN CHECK23=" CHECKED "%>
			<input type="checkbox" READONLY ID="Text23" name="key23" value="Y" <%=CHECK23%> bgcolor="silver"><font size=2>(0.8點) 維修(需有派工單)</font>
		</td>
	</tr>
	<tr><td bgcolor="silver">
			<% IF DSPKEY(24)="Y" THEN CHECK24=" CHECKED "%>
			<input type="checkbox" READONLY ID="Text24" name="key24" value="Y" <%=CHECK24%> bgcolor="silver"><font size=2>(0.3點) 線上維修</font>　　　　　　
		</td>
		<td bgcolor="silver">
			<% IF DSPKEY(25)="Y" THEN CHECK25=" CHECKED "%>
			<input type="checkbox" READONLY ID="Text25" name="key25" value="Y" <%=CHECK25%> bgcolor="silver"><font size=2 color=red>(1點) 發DM(需有派工單)</font>
		</td>
	</tr>
	<tr><td bgcolor="silver">
			<% IF DSPKEY(26)="Y" THEN CHECK26=" CHECKED "%>
			<input type="checkbox" READONLY ID="Text26" name="key26" value="Y" <%=CHECK26%> bgcolor="silver"><font size=2>(1點) 社區深耕活動(需有派工單)</font>　　　　　
		</td>
		<td bgcolor="silver">
			<% IF DSPKEY(27)="Y" THEN CHECK27=" CHECKED "%>
			<input type="checkbox" READONLY ID="Text27" name="key27" value="Y" <%=CHECK27%> bgcolor="silver"><font size=2 color=red>(0.3點) 放DM、整理DM</font>
		</td>
	</tr>
	<tr><td bgcolor="silver">
			<% IF DSPKEY(28)="Y" THEN CHECK28=" CHECKED "%>
			<input type="checkbox" READONLY ID="Text28" name="key28" value="Y" <%=CHECK28%> bgcolor="silver"><font size=2>(0.3點) 社區電信室勘查</font>
		</td>
		<td bgcolor="silver">
			<% IF DSPKEY(29)="Y" THEN CHECK29=" CHECKED "%>
			<input type="checkbox" READONLY ID="Text29" name="key29" value="Y" <%=CHECK29%> bgcolor="silver"><font size=2>(0點) 業務拜訪</font>
		</td>
	</tr>
	</table>		
		
		</td>
	</TR>

	
	<tr><td width="10%" class="dataListHEAD">行程備註</td>
		<td colspan=5>
			<TEXTAREA cols="100%" name="key9" rows=10 MAXLENGTH=1024 class="dataListentry" <%=dataprotect%> value="<%=dspkey(9)%>" ID="Textarea9"><%=dspkey(9)%></TEXTAREA>
		</td>
	</tr>

</table>

</DIV>

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
'    Dim conn,rs
' Smode A:add U:update
' extDBField = n
' use extDB(i) for Screen ,and map it to DataBase
'
    'Set conn=Server.CreateObject("ADODB.Connection")
    'conn.open DSN
    'Set rs=Server.CreateObject("ADODB.Recordset")
    'Set comm=Server.CreateObject("ADODB.Command")
    
'------ RTObj ---------------------------------------------------
    'DELFAQlist="delete from RTLessorAVScustfaqlist where faqno='" & dspkey(1) & "'"
    'conn.Execute DELFAQlist  
    'For i=0 to 99
    '    if len(trim(extdb(i))) > 0  then
    '       rs.Open "SELECT * FROM RTLessorAVScustfaqlist WHERE faqno='" &dspKey(1) &"' and faqcod='" & extDB(i) & "'" ,conn,3,3
    '       If rs.Eof Or rs.Bof Then
    '          rs.AddNew
    '          rs("cusid")=dspKey(0)
    '          rs("faqno")=dspKey(1)
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
<!-- #include virtual="/Webap/include/RTGetUserRight.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
