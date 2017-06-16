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
    For i = 0 To numberOfField-1
        sType=Right("000" &Cstr(aryKeyType(i)),3) 
        If Instr(cTypeChar,sType) > 0 Then
           dspKey(i)=""
        ElseIf Instr(cTypeNumeric,sType) > 0 Then
           dspKey(i)=0
        'ElseIf Instr(cTypeDate,sType) > 0 Then
        '   dspKey(i)=Now()
        ElseIf Instr(cTypeBoolean,sType) > 0 Then
           dspKey(i)=True
        Else
           dspKey(i)=""
        End If
    Next
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
	  'response.Write "sType=" &sType&"<br>"
	  if dspKey(i) ="" then dspKey(i) =0
      If Instr(cTypeChar,sType) > 0 Or dspKey(i)=Null Then  
         sql=sql &"[" &aryKeyNameDB(i) &"]='" &dspKey(i) &"'"
      Else
         sql=sql &"[" &aryKeyNameDB(i) &"]=" &dspKey(i)
      End If
    Next
    GetSql=sqlList &sql 
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

              '   On Error Resume Next
                runpgm=Request.ServerVariables("PATH_INFO") 
                select case ucase(runpgm)   
                   ' 因dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
                   case ucase("/webap/rtap/base/RTPowerBill/RTPowerBillD.asp")
'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       if i <> 0 then rs.Fields(i).Value=dspKey(i)
                       if i=0 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         cusidxx="B" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)
                         sql = "select max(BILLNO) AS maxbillno from RTPowBillH where BILLNO like '" & cusidxx & "%' "
                         rsc.open sql,conn
'response.Write "maxbillno="& rsc("maxbillno")
                         if len(rsc("maxbillno")) > 0 then
                            dspkey(0)=cusidxx & right("0000" & cstr(cint(right(rsc("maxbillno"),5)) + 1),5)
                         else
                            dspkey(0)=cusidxx & "00001"
                         end if
                         rsc.close
                         rs.Fields(i).Value=dspKey(i) 
                       end if      
                   case else
                        rs.Fields(i).Value=dspKey(i)
                END SELECT
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
              runpgm=Request.ServerVariables("PATH_INFO") 
              select case ucase(runpgm)   
                 ' 當程式為ADSL社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
                 case ucase("/webap/rtap/base/RTPowerBill/RTPowerBillMMD.asp")                 
' response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                     if i<>0  then rs.Fields(i).Value=dspKey(i)
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
       if ucase(runpgm)=ucase("/webap/rtap/base/RTPowerBill/RTPowerBillD.asp") then
		  cusidxx="C" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)
          rsc.open "select max(billno) AS maxbillno from RTPowBillMM where billno like '" & cusidxx & "%' " ,conn
          if not rsC.eof then
            dspkey(0)=rsC("billno")
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
<input type="text" name="sw" value="<%=sw%>" style="display:none;" ID="Text17" size="20">
<input type="text" name="reNew" value="N" style="display:none;" ID="Text18" size="20">
<input type="text" name="rwCnt" value="<%=rwCnt%>" style="display:none;" ID="Text19" size="20">
<input type="text" name="accessMode" value="<%=accessMode%>" style="display:none;" ID="Text20" size="20">
<table width="100%" ID="Table1">
  <tr><td width="20%">　</td><td width="60%" align=center>
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
  title="電費補助基本資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB=	"SELECT BILLNO, CTNO, EDAT, STRYM, ENDYM, COUNTTYPE, RATIOBEFORE, PAYTYPE, " &_
				"RATIO1, RATIO2, MPAY1, MPAY2, CUST1, CUST2, RATIO, PAY, CHECKNO, " &_
				"POSTNO, CHECKOUTDAT, WORKNO, RECEIPTDAT, MEMO, UDAT, UUSR, " &_
				"CANCELDAT, CANCELUSR, LINENUM, CUSTNUM " &_
				"FROM RTPowBillMM WHERE BILLNO='' "
  sqlList    ="SELECT BILLNO, CTNO, EDAT, STRYM, ENDYM, COUNTTYPE, RATIOBEFORE, PAYTYPE, " &_
				"RATIO1, RATIO2, MPAY1, MPAY2, CUST1, CUST2, RATIO, PAY, CHECKNO, " &_
				"POSTNO, CHECKOUTDAT, WORKNO, RECEIPTDAT, MEMO, UDAT, UUSR, " &_
				"CANCELDAT, CANCELUSR, LINENUM, CUSTNUM " &_
				"FROM RTPowBillMM WHERE "
  userDefineRead="Yes"      
  userDefineSave="Yes"       
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(23)=V(0)
        dspkey(22)=now()
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
       '作廢人
       if ClickID ="24" then
       		document.all("key25").value = document.all("colEditor").value
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

'       if clickkey = "C4" then
'       	  document.all("key5").value = ""
'       	  document.all("colBanknc").value = ""
'       	  document.all("colBranchnc").value = ""
'       elseif clickkey = "C5" then
'       	  document.all("colBranchnc").value = ""
'       end if
'
'       if clickkey = "C7" then
'       	  document.all("key8").value = ""
'       	  document.all("colCutnc").value = ""
'       	  document.all("colZip").value = ""
'       elseif clickkey = "C8" then
'       	  document.all("colZip").value = ""
'       end if
 
       if ClickID ="24" then
       		document.all("key25").value = ""
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

	Sub SrReCalulate()
		document.all("colPaysum").value=""
		document.all("colRatiosum").value=""

		select case document.all("KEY5").value
			case "01"
				document.all("KEY8").value = 0
				document.all("KEY9").value = 0
				document.all("KEY10").value = document.all("colMpay101").value
				document.all("KEY11").value = 0
				document.all("KEY12").value = 0
				document.all("KEY13").value = 0
			case "02"
				document.all("KEY8").value = 0
				document.all("KEY9").value = 0
				document.all("KEY10").value = document.all("colMpay102").value
				document.all("KEY11").value = 0
				document.all("KEY12").value = document.all("colCust102").value
				document.all("KEY13").value = 0
			case "03"
				document.all("KEY8").value = 0
				document.all("KEY9").value = 0
				document.all("KEY10").value = document.all("colMpay103").value
				document.all("KEY11").value = 0
				document.all("KEY12").value = 0
				document.all("KEY13").value = 0
			case "04"
				document.all("KEY8").value = 0
				document.all("KEY9").value = 0
				document.all("KEY10").value =  document.all("colMpay104").value
				document.all("KEY11").value = 0
				document.all("KEY12").value = document.all("colCust104").value
				document.all("KEY13").value = 0
			case "05"
				document.all("KEY8").value = 0
				document.all("KEY9").value = 0
				document.all("KEY10").value = document.all("colMpay105").value
				document.all("KEY11").value = document.all("colMpay205").value
				document.all("KEY12").value = document.all("colCust105").value
				document.all("KEY13").value = 0
			case "06"
				document.all("KEY8").value = 0
				document.all("KEY9").value = 0
				document.all("KEY10").value = document.all("colMpay106").value
				document.all("KEY11").value = document.all("colMpay206").value
				document.all("KEY12").value = document.all("colCust106").value
				document.all("KEY13").value = document.all("colCust206").value
			case "07"
				document.all("KEY8").value = document.all("colRatio107").value
				document.all("KEY9").value = 0
				document.all("KEY10").value = 0
				document.all("KEY11").value = 0
				document.all("KEY12").value = 0
				document.all("KEY13").value = 0
			case "08"
				document.all("KEY8").value = document.all("colRatio108").value
				document.all("KEY9").value = 0
				document.all("KEY10").value = 0
				document.all("KEY11").value = 0
				document.all("KEY12").value = 0
				document.all("KEY13").value = 0
				document.all("KEY15").value = round(document.all("KEY8").value * (document.all("KEY14").value - document.all("KEY6").value))
				document.all("colRatiosum").value = document.all("KEY14").value - document.all("KEY6").value &"度"
			case "09"
				document.all("KEY8").value = document.all("colRatio109").value
				document.all("KEY9").value = 0
				document.all("KEY10").value = document.all("colMpay109").value
				document.all("KEY11").value = 0
				document.all("KEY12").value = 0
				document.all("KEY13").value = 0
				document.all("KEY15").value = round(document.all("KEY8").value * (document.all("KEY14").value - document.all("KEY6").value)) + document.all("KEY10").value * (datediff("m", left(document.all("KEY3").value,4)+"/"+right(document.all("KEY3").value,2)+"/01", left(document.all("KEY4").value,4)+"/"+right(document.all("KEY4").value,2)+"/01")+1 )
				document.all("colRatiosum").value = document.all("KEY14").value - document.all("KEY6").value &"度"
				document.all("colPaysum").value = document.all("KEY8").value &"元 x "& document.all("colRatiosum").value &" = "& round(document.all("KEY8").value * (document.all("KEY14").value - document.all("KEY6").value)) &"元 + ("&_
				document.all("KEY10").value &"元 x "& datediff("m", left(document.all("KEY3").value,4)+"/"+right(document.all("KEY3").value,2)+"/01", left(document.all("KEY4").value,4)+"/"+right(document.all("KEY4").value,2)+"/01")+1 &"個月)"
			case "10"
				document.all("KEY8").value = document.all("colRatio110").value
				document.all("KEY9").value = document.all("colRatio210").value
				document.all("KEY10").value = 0
				document.all("KEY11").value = 0
				document.all("KEY12").value = 0
				document.all("KEY13").value = 0
				document.all("KEY15").value = round(document.all("KEY8").value * (document.all("KEY14").value - document.all("KEY6").value))
				document.all("colRatiosum").value = document.all("KEY14").value - document.all("KEY6").value &"度"
			case "11"
				document.all("KEY8").value = 0
				document.all("KEY9").value = 0
				document.all("KEY10").value = document.all("colMpay111").value
				document.all("KEY11").value = 0
				document.all("KEY12").value = 0
				document.all("KEY13").value = 0
			case else
				'document.all("KEY17").value = 0
				'document.all("KEY18").value = 0
				'document.all("KEY19").value = 0
				'document.all("KEY20").value = 0
				'document.all("KEY21").value = 0
				'document.all("KEY22").value = 0
		end select
   End Sub

	'合約搜尋鈕
   Sub SrSearchOnClick()
	   colComn = document.all("colComn").VALUE
       prog="RTGetCtno.asp?KEY="& colComn &";"
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:630px;dialogHeight:480px;")
       'FUsr=Window.open(prog,"d2","resizable=yes", true)
       if fusr <> "" then 
			FUsrID=Split(Fusr,";")
			if Fusrid(0) ="Y" then
				document.all("key1").value =  trim(Fusrid(1))	'ctno
				document.all("key5").value =  trim(Fusrid(8))	'counttype
				document.all("key7").value =  trim(Fusrid(12))	'paytype
				document.all("key8").value =  trim(Fusrid(14))	'ratio1
				document.all("key9").value =  trim(Fusrid(15))	'ratio2
				document.all("key10").value =  trim(Fusrid(16))	'mpay1
				document.all("key11").value =  trim(Fusrid(17))	'mpay2
				document.all("key12").value =  trim(Fusrid(18))	'cust1
				document.all("key13").value =  trim(Fusrid(19))	'cust2
				'if document.all("key3").value ="" then document.all("key3").value =  trim(Fusrid(13))	'checktitle
				document.all("colComq1").value =  trim(Fusrid(4))
				document.all("colComn").value =  trim(Fusrid(5))
				document.all("colCutnc").value =  trim(Fusrid(6))
				document.all("colTownship").value =  trim(Fusrid(7))
				document.all("colComtypenc").value =  trim(Fusrid(3))
				document.all("colPaycyclenc").value =  trim(Fusrid(10))
				document.all("colSonetreq").value =  trim(Fusrid(20))
			End if       
       end if
   End Sub

   SUB SrChangeCount(countall)
   		SrReCalulate
   		
		if len(document.all("key5").value) =0 then 
			window.DspCount00.style.display =""
		else
			window.DspCount00.style.display ="none"
		End If

		For i = 1 To countall
			CountNow = right("0"& i, 2)
			DspCount = "DspCount"& CountNow
			If document.all("key5").value = CountNow Then
				document.all(DspCount).style.display=""
			else
				document.all(DspCount).style.display="none"
			end if
		Next

'		select case document.all("key12").value
'			case "01"
'				document.all("key17").value =　document.all("colMpay1010").value
'				document.all("key18").value =0
'				document.all("key19").value =0
'				document.all("key20").value =0
'				document.all("key21").value =0
'				document.all("key22").value =0
'			case else
'				document.all("key17").value =0
'				document.all("key18").value =0
'				document.all("key19").value =0
'				document.all("key20").value =0
'				document.all("key21").value =0
'				document.all("key22").value =0
'		end select
	END SUB

 </Script>
<%   
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveX() %>
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"   codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 >
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<%	
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()
	'在最上方顯示key值%>
	<table width="60%" border=1 cellPadding=0 cellSpacing=0>
		<tr><td width="10%" class=dataListHead>電費序號</td>
			<td width="10%" bgcolor="silver">
				<input type="text" name="key0" readonly size="12" class=dataListdata value="<%=dspKey(0)%>">
			</td>
		</tr>
	</table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
	'EUSR,EDAT,UUSR,UDAT四欄取得
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(23))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(23)=V(0)
        End if  
       dspkey(22)=now()
    else
        if len(trim(dspkey(23))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(23)=V(0)
        End if         
        dspkey(22)=now()
    end if      
	
	' by作業流程鎖定欄位--------------------------------------------------------------------------
    '用戶送件申請後,基本資料 protect
'    If len(trim(dspKey(32))) > 0 OR len(trim(dspKey(38))) > 0 OR len(trim(dspKey(39))) > 0 OR len(trim(dspKey(40))) > 0 Then
'       fieldPa=" class=""dataListData"" readonly "
'       fieldpb=" disabled "
'    Else
'       fieldPa=""
'       fieldpb=""
'    End If
    '申請轉檔後，送件申請日protect
'    If len(trim(dspKey(33))) > 0 OR len(trim(dspKey(38))) > 0 OR len(trim(dspKey(39))) > 0 OR len(trim(dspKey(40))) > 0 Then
'       fieldPC=" class=""dataListData"" readonly "
'       fieldpD=" disabled "
'    Else
'       fieldPC=""
'       fieldpD=""
'    End If
        '報竣後，全部資料protect(不含作廢日)
'    If len(trim(dspKey(39))) > 0 OR len(trim(dspKey(38))) > 0  Then
'       fieldPe=" class=""dataListData"" readonly "
'       fieldpf=" disabled "
'    Else
'       fieldPe=""
'       fieldpf=""
'    End If

    Dim conn,rs,s,sx,sql,t      
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN

	sql="select a.comtype, c.codenc as comtypenc, b.comq1, b.comn, d.cutnc, b.township, " &_
		"g.codenc as counttypenc, f.codenc as paytypenc, e.codenc as paycyclenc, " &_
		"a.sonetreq, a.memo as memoct, case when ratio-ratiobefore >0 then convert(varchar(5), ratio-ratiobefore)+'度' else '' end as ratiosum, " &_
		"case when x.counttype ='09' then convert(varchar(4), x.ratio1) +'元 x '+ convert(varchar(6), x.ratio-x.ratiobefore)+'度 ='+ " &_
		"convert(varchar(7),convert(int,floor(x.ratio1*(x.ratio-x.ratiobefore)),0))+'元+('+ convert(varchar(5), x.mpay1)+'元 x '+ convert(varchar(3), datediff(m, strym+'01', endym+'01')+1) +'個月)'else '' end as paysum " &_
		"from	RTPowBillMM x " &_
		"inner join RTPowBillH a on x.ctno = a.ctno " &_
		"inner join RTCmtyAll b on a.comtype = b.comtype and a.comq1 = b.comq1 " &_
		"left outer join RTCode c on c.code = a.comtype and c.kind ='P5' " &_
		"left outer join RTCounty d on d.cutid = b.cutid " &_
		"left outer join RTCode e on e.code = a.paycycle and e.kind ='K5' " &_
		"left outer join RTCode f on f.code = x.paytype and f.kind ='F5' " &_
		"left outer join RTCode g on g.code = x.counttype and g.kind ='R4' " &_
		"where x.ctno ='" &dspkey(1)& "' "
'response.write sql
		rs.OPEN sql,CONN
		if rs.eof then
			comtype =""		:	comtypenc = ""	:	comq1 = ""
			comn = ""		:	cutnc = ""		:	township = ""
			paycyclenc = ""	:	paytypenc = ""	:	counttypenc = ""
			sonetreq = ""	:	memoct = ""		:	ratiosum=""
			paysum =""
		else
			comtype =rs("comtype")		:	comtypenc =rs("comtypenc")	:	comq1 = rs("comq1")
			comn = rs("comn")			:	cutnc = rs("cutnc")			:	township = rs("township")	
			paycyclenc =rs("paycyclenc"):	paytypenc = rs("paytypenc")	:	counttypenc = rs("counttypenc")
			sonetreq = rs("sonetreq")	:	memoct = rs("memoct")		:	ratiosum = rs("ratiosum")
			paysum = rs("paysum")
		end if
		rs.close
		
		
	%>

<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
	<tr><td width="15%" class=dataListsearch>社區名稱</td>
		<td bgcolor="silver" colspan=5 >
			<input type="text" size="42" size="20" id="colCOMN" NAME="colCOMN" value="<%=comn%>" style="color=blue;">
			<input type="button" id="BtnComn" name="BtnComn" value="社區合約搜尋" onclick="SrSearchOnClick()">
		</td>
	</tr>

	<tr><td width="15%" class=dataListsearch>方案別</td>
		<td width="35%" bgcolor="silver" >
			<input type="text" readonly size="10" name="colComtypenc" value="<%=comtypenc%>" class="dataListsearch3" ID="Text42">
		</td>
	    <td width="15%" class=dataListsearch>社區序號</td>
		<td width="35%" bgcolor="silver" >
			<input type="text" readonly size="5" name="colComq1" value="<%=comq1%>" class="dataListsearch3" ID="Text42">
		</td>
	</tr>

	<tr><td width="21%" class="dataListsearch">供電週期</td>
		<td width="26%" bgcolor="silver">
			<input type="text" readonly size="5" name="colPaycyclenc" value="<%=paycyclenc%>" class="dataListsearch3" ID="Text42">
		</td>
		<td width="21%" class="dataListsearch">電費須向Sonet請款</td>
		<td width="26%" bgcolor="silver">
			<input type="text" readonly size="5" name="colSonetreq" value="<%=sonetreq%>" class="dataListsearch3" ID="Text42">
		</td>
	</tr>

	<tr>
		<td class=dataListsearch>合約備註</td>
		<td bgcolor="silver" colspan=5>
			<TEXTAREA cols="100%" readonly name="colMemoct" value="<%=memoct%>" style="overflow-y:visible;" class="dataListsearch3" ID="Textarea1"><%=memoct%></TEXTAREA>
		</td>
	</tr>

	<tr><td width="10%" class=dataListHEAD>合約序號</td>
		<td bgcolor="silver">
        	<input type="text" name="key1" value="<%=dspKey(1)%>" readonly size="11" class=dataListData>
        </td>
		<td width="10%" class=dataListHEAD>建檔日</td>
		<td bgcolor="silver">
        	<input type="text" name="key2" value="<%=dspKey(2)%>" readonly size="25" class=dataListData>
        </td>
	</tr>

	<tr><td width="10%" class=dataListHEAD>本期起迄年月</td>
		<td bgcolor="silver" colspan=3>
        	<input type="text" name="key3" value="<%=dspKey(3)%>" maxlength ="6" size="7" class=dataListEntry>～
        	<input type="text" name="key4" value="<%=dspKey(4)%>" maxlength ="6" size="7" class=dataListEntry>
        </td>
	</tr>

	<tr><%
		    s=""
		    sx=" selected "
		    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  Then  
		       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='R4' " 
		       If len(trim(dspkey(5))) < 1 Then
		          sx=" selected " 
		          s=s & "<option value=""""" & sx & "></option>"  
		          sx=""
		       else
		          s=s & "<option value=""""" & sx & "></option>"  
		          sx=""
		       end if     
		    Else
		       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='R4' AND CODE='" & dspkey(5) & "'"
		    End If
		    rs.Open sql,conn
		    Do While Not rs.Eof
		       If rs("CODE")=dspkey(5) Then sx=" selected "
		       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
		       rs.MoveNext
		       sx=""
		    Loop
		    rs.Close
		    
			sql="SELECT convert(tinyint,MAX(CODE)) as maxcode FROM RTCODE WHERE KIND='R4' " 
			rs.Open sql,conn
				sx= "SrChangeCount(" &rs("maxcode")& ")"
		    rs.Close
		%>
  		<td WIDTH="10%" class="dataListHEAD" height="23">電費計算方式</td>
        <td WIDTH="23%" height="23" bgcolor="silver" colspan=3>
			<select size="1" name="key5" ID="Select5" onchange="<%=sx%>" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ><%=s%></select>　
			<span id="DspCount00" style=" <% if dspkey(5)<>"" then response.write "display: none;" %> ">
				　
			</span>
			<span height="23" bgcolor="silver" colspan=2 id="DspCount01" style=" <% if dspkey(5)<>"01" then response.write "display: none" %> ">
			   每月<input class=dataListEntry type="text" name="colMpay101" maxlength="5" size="5" value="<%=dspKey(10)%>" onchange="SrReCalulate()">元
	        </span>
	       	<span height="23" bgcolor="silver" colspan=2 id="DspCount02" style=" <% if dspkey(5)<>"02" then response.write "display: none" %> ">
			   每月-<input class=dataListEntry type="text" name="colMpay102" maxlength="5" size="5" value="<%=dspKey(10)%>" onchange="SrReCalulate()">元
			   (需<input class=dataListEntry type="text" name="colCust102" maxlength="3" size="3" value="<%=dspKey(12)%>" onchange="SrReCalulate()">戶以上)
	        </span>
			<span height="23" bgcolor="silver" colspan=2 id="DspCount03" style=" <% if dspkey(5)<>"03" then response.write "display: none" %> ">
			   每月-<input class=dataListEntry type="text" name="colMpay103" maxlength="5" size="5" value="<%=dspKey(10)%>" onchange="SrReCalulate()">元/每一集訊設備(每8戶)
	        </span>
			<span height="23" bgcolor="silver" colspan=2 id="DspCount04" style=" <% if dspkey(5)<>"04" then response.write "display: none" %> ">
			   每月-<input class=dataListEntry type="text" name="colMpay104" maxlength="5" size="5" value="<%=dspKey(10)%>" onchange="SrReCalulate()">元/每一主線(需
			   <input class=dataListEntry type="text" name="colCust104" maxlength="3" size="3" value="<%=dspKey(12)%>" onchange="SrReCalulate()">戶以上)
	        </span>
			<span height="23" bgcolor="silver" colspan=2 id="DspCount05" style=" <% if dspkey(5)<>"05" then response.write "display: none" %> ">
			   每月-<input class=dataListEntry type="text" name="colCust105" maxlength="3" size="3" value="<%=dspKey(12)%>" onchange="SrReCalulate()">戶以下
			   <input class=dataListEntry type="text" name="colMpay105" maxlength="5" size="5" value="<%=dspKey(10)%>" onchange="SrReCalulate()">元; 超過則
			   <input class=dataListEntry type="text" name="colMpay205" maxlength="5" size="5" value="<%=dspKey(11)%>" onchange="SrReCalulate()">元
	        </span>
			<span height="23" bgcolor="silver" colspan=2 id="DspCount06" style=" <% if dspkey(5)<>"06" then response.write "display: none" %> ">
			   每月-<input class=dataListEntry type="text" name="colCust106" maxlength="3" size="3" value="<%=dspKey(12)%>" onchange="SrReCalulate()">戶以下
			   <input class=dataListEntry type="text" name="colMpay106" maxlength="5" size="5" value="<%=dspKey(10)%>" onchange="SrReCalulate()">元; 超過則每增加
			   <input class=dataListEntry type="text" name="colCust206" maxlength="3" size="3" value="<%=dspKey(13)%>" onchange="SrReCalulate()">戶加
			   <input class=dataListEntry type="text" name="colMpay206" maxlength="5" size="5" value="<%=dspKey(11)%>" onchange="SrReCalulate()">元
	        </span>
			<span height="23" bgcolor="silver" colspan=2 id="DspCount07" style=" <% if dspkey(5)<>"07" then response.write "display: none" %> ">
			   每月-認列收入之<input class=dataListEntry type="text" name="colRatio107" maxlength="3" size="3" value="<%=dspKey(8)%>" onchange="SrReCalulate()">％
	        </span>
			<span height="23" bgcolor="silver" colspan=2 id="DspCount08" style=" <% if dspkey(5)<>"08" then response.write "display: none" %> ">
			   電錶-<input class=dataListEntry type="text" name="colRatio108" maxlength="3" size="3" value="<%=dspKey(8)%>" onchange="SrReCalulate()">元/度
	        </span>
			<span height="23" bgcolor="silver" colspan=2 id="DspCount09" style=" <% if dspkey(5)<>"09" then response.write "display: none" %> ">
			   電錶-<input class=dataListEntry type="text" name="colRatio109" maxlength="3" size="3" value="<%=dspKey(8)%>" onchange="SrReCalulate()">元/度; 廣告費
			   <input class=dataListEntry type="text" name="colMpay109" maxlength="5" size="5" value="<%=dspKey(10)%>" onchange="SrReCalulate()">元/月
	        </span>
			<span height="23" bgcolor="silver" colspan=2 id="DspCount10" style=" <% if dspkey(5)<>"10" then response.write "display: none" %> ">
			   電錶-夏季(6~9月)<input class=dataListEntry type="text" name="colRatio210" maxlength="3" size="3" value="<%=dspKey(9)%>" onchange="SrReCalulate()">元/度; 非夏季
			   <input class=dataListEntry type="text" name="colRatio110" maxlength="3" size="3" value="<%=dspKey(8)%>" onchange="SrReCalulate()">元/度
	        </span>
			<span height="23" bgcolor="silver" colspan=2 id="DspCount11" style=" <% if dspkey(5)<>"11" then response.write "display: none" %> ">
			   每月-<input class=dataListEntry type="text" name="colMpay111" maxlength="5" size="5" value="<%=dspKey(10)%>" onchange="SrReCalulate()">元/每一主線
	        </span>
        </td>
	</tr>

	<tr style="display: none"><td class=dataListHead height="23">計算相關欄</td>
		<td bgcolor="silver" colspan=3>
			ratio1:<input class=dataListEntry type="text" name="key8" maxlength="5" size="5" value="<%=dspKey(8)%>">
			ratio2:<input class=dataListEntry type="text" name="key9" maxlength="5" size="5" value="<%=dspKey(9)%>">
			mpay1<input class=dataListEntry type="text" name="key10" maxlength="5" size="5" value="<%=dspKey(10)%>">
			mpay2<input class=dataListEntry type="text" name="key11" maxlength="5" size="5" value="<%=dspKey(11)%>">
			cust1<input class=dataListEntry type="text" name="key12" maxlength="5" size="5" value="<%=dspKey(12)%>">
			cust2<input class=dataListEntry type="text" name="key13" maxlength="5" size="5" value="<%=dspKey(13)%>">
		</td>
	</tr>

	<tr><td WIDTH="10%" class="dataListHEAD" height="23">電錶度數</td>
        <td WIDTH="23%" height="23" bgcolor="silver" >
        	<input class=dataListEntry type="text" name="key6" maxlength="8" size="6" value="<%=dspKey(6)%>" onchange="SrReCalulate()">(前期)--->
        	<input class=dataListEntry type="text" name="key14" maxlength="8" size="6" value="<%=dspKey(14)%>" onchange="SrReCalulate()">(本期)
        	<input type="text" bgcolor=silver readonly size="5" name="colRatiosum" value="<%=ratiosum%>" ID="colRatiosum" class="dataListsearch3" style="color=red;">
        </td>
		<td width="10%" class=dataListHEAD>當期社區線戶數</td>
		<td bgcolor="silver">
        	<input type="text" name="key26" value="<%=dspKey(26)%>" size="5" class=dataListEntry>線；
        	<input type="text" name="key27" value="<%=dspKey(27)%>" size="6" class=dataListEntry>戶
        </td>
	</tr>

	<tr><td class=dataListHead height="23">本期金額總計</td>
		<td bgcolor="silver" colspan=3>
	        <input class=dataListEntry type="text" name="key15" maxlength="6" size="6" value="<%=dspKey(15)%>">
	        <input type="text" readonly size="50" name="colPaysum" value="<%=paysum%>" ID="colPaysum" class="dataListsearch3" style="color=red;">
		</td>
	</tr>

	<tr><td  WIDTH="10%"  class="dataListHEAD" height="23">給付方式</td>
		<%
		    s=""
		    sx=" selected "
		    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  Then  
		       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='F5' " 
		       If len(trim(dspkey(7))) < 1 Then
		          sx=" selected " 
		          s=s & "<option value=""""" & sx & "></option>"  
		          sx=""
		       else
		          s=s & "<option value=""""" & sx & "></option>"  
		          sx=""
		       end if     
		    Else
		       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='F5' AND CODE='" & dspkey(7) & "'"
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
	        <td  WIDTH="23%" height="23" bgcolor="silver" >
			<select size="1" name="key7" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select7"><%=s%></select>
		</td>

		<td class=dataListHead height="23">派工單號</td>
		<td bgcolor="silver">
	        <input class=dataListEntry type="text" name="key19" maxlength="12" size="13" value="<%=dspKey(19)%>">
		</td>
	</tr>

	<tr><td class=dataListHead height="23">支票號碼</td>
		<td bgcolor="silver" >
	        <input class=dataListEntry type="text" name="key16" maxlength="9" size="10" value="<%=dspKey(16)%>">
		</td>
		<td class=dataListHead height="23">郵政號碼</td>
		<td bgcolor="silver" >
	        <input class=dataListEntry type="text" name="key17" maxlength="15" size="16" value="<%=dspKey(17)%>">
		</td>
	</tr>

 	<tr><td class="dataListHEAD" height="23">支票寄出日</td>                                 
        <td height="23" bgcolor="silver">
			<input type="text" name="key18" size="12" value="<%=dspKey(18)%>" readonly <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text18">
       		<input type="button" name="B18" id="B18" height="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
       		<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C18" name="C18" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
        </td>

 		<td class="dataListHEAD" height="23">收據回收日</td>                                 
        <td height="23" bgcolor="silver">
			<input type="text" name="key20" size="12" value="<%=dspKey(20)%>" readonly <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text20">
       		<input type="button" name="B20" id="B20" height="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
       		<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C20" name="C20" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
		</td>
	</tr>

	<tr><td width="10%" class="dataListHEAD">備註</td>
		<td colspan=4 bgcolor="silver">
			<TEXTAREA cols="100%" name="key21" rows=10 MAXLENGTH=500 class="dataListentry" <%=dataprotect%> value="<%=dspkey(21)%>" ID="Textarea24"><%=dspkey(21)%></TEXTAREA>
		</td>
	</tr>

 	<tr><td class="dataListHEAD" height="23">作廢人員</td>                                 
        <td height="23" bgcolor="silver">
			<input type="text" name="key25" size="6" value="<%=dspKey(25)%>" <%=fieldRole(1)%> readonly class="dataListDATA" ID="Text25">
			<font size=2><%=SrGetEmployeeName(dspKey(25))%></font>
        </td>

 		<td class="dataListHEAD" height="23">作廢日期</td>                                 
        <td height="23" bgcolor="silver">
			<input type="text" name="key24" size="25" value="<%=dspKey(24)%>" readonly class="dataListEntry" ID="Text24">
       		<input type="button" name="B24" id="B24" height="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
       		<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C24" name="C24" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
		</td>
	</tr>

	<tr><td class="dataListHEAD" height="23">修改人員</td>                                 
        <td height="23" bgcolor="silver">
			<input type="text" name="key23" size="6" value="<%=dspKey(23)%>" <%=fieldRole(1)%> readonly class="dataListDATA" ID="Text23">
			<font size=2><%=SrGetEmployeeName(dspKey(23))%></font>
			<% 	
				CALL SrGetEmployeeRef(Rtnvalue,1,logonid)
                editor=split(rtnvalue,";")  
			%>
			<input type="text" name="colEditor" size="6" value="<%= editor(0) %>" style="display:none">
        </td>

 		<td class="dataListHEAD" height="23">修改日期</td>                                 
        <td height="23" bgcolor="silver">
			<input type="text" name="key22" size="25" value="<%=dspKey(22)%>"  <%=fieldpa%><%=fieldRole(1)%> readonly class="dataListdata" ID="Text22">
		</td>
	</tr>

</table><br><br>

<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="center">列計之社區方案</td></tr>
</table>

<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table4">
	<tr><td class=dataListDATA>社區方案</td>
		<td class=dataListDATA>社區序號</td>
		<td class=dataListDATA>社區名稱</td>
		<td class=dataListDATA>縣市</td>
		<td class=dataListDATA>鄉鎮</td>
		<td class=dataListDATA>附掛電話</td>
		<td class=dataListDATA>用戶數</td>
		<td class=dataListDATA>撤線日</td>
	</tr>
    <%
     s=""
     sql="select c.codenc, case a.comtype when '3' then convert(varchar(5),e.comq1) else convert(varchar(5),e.comq1)+'-'+convert(varchar(5),e.lineq1) end as comq, " &_
		"b.comn, d.cutnc, b.township, e.linetel, usercnt, e.rcomdrop " &_
		"from 	RTPowBillH a " &_
		"inner join RTCmtyAll b on a.comtype = b.comtype and a.comq1 = b.comq1 " &_
		"inner join HBAdslCmty e on e.comq1 = a.comq1 and a.comtype = e.comtype " &_
		"left outer join RTCode c on c.code = a.comtype and c.kind ='P5' " &_
		"left outer join RTCounty d on d.cutid = b.cutid " &_
		"where a.canceldat is null and e.canceldat is null and a.CTNO ='"& dspkey(1) &"' " &_
		"union " &_
		"select c.codenc, case a.comtype when '3' then convert(varchar(5),e.comq1) else convert(varchar(5),e.comq1)+'-'+convert(varchar(5),e.lineq1) end as comq, " &_
		"b.comn, d.cutnc, b.township, e.linetel, usercnt, e.rcomdrop " &_
		"from 	RTPowBillCmty a " &_
		"inner join RTCmtyAll b on a.comtype = b.comtype and a.comq1 = b.comq1 " &_
		"inner join HBAdslCmty e on e.comq1 = a.comq1 and a.comtype = e.comtype " &_
		"left outer join RTCode c on c.code = a.comtype and c.kind ='P5' " &_
		"left outer join RTCounty d on d.cutid = b.cutid " &_
		"where a.canceldat is null and e.canceldat is null and a.CTNO ='"& dspkey(1) &"' " &_
		"order by e.rcomdrop, e.linetel "
'response.write sql
     rs.Open sql,conn
     Do While Not rs.Eof
       RESPONSE.Write "<TR>"
       RESPONSE.Write "<td ALIGN=""center"" class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("codenc") & "&nbsp;</FONT></td>"
       RESPONSE.Write "<td ALIGN=""center"" class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("COMQ") & "&nbsp;</FONT></td>"
       RESPONSE.Write "<td ALIGN=""center"" class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("comn") & "&nbsp;</FONT></td>"
       RESPONSE.Write "<td ALIGN=""center"" class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("cutnc") & "&nbsp;</FONT></td>"
       RESPONSE.Write "<td ALIGN=""center"" class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("township") & "&nbsp;</FONT></td>"
       RESPONSE.Write "<td ALIGN=""center"" class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("linetel") & "&nbsp;</FONT></td>"
       RESPONSE.Write "<td ALIGN=""center"" class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("usercnt") & "&nbsp;</FONT></td>"
       RESPONSE.Write "<td ALIGN=""center"" class=dataListHEAD2><FONT SIZE=2 COLOR=GREEN>" & RS("rcomdrop") & "&nbsp;</FONT></td>"
	   RESPONSE.Write "</TR>"
       rs.MoveNext
     Loop
     rs.Close
    %>
</table>
<p></p>
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
%>
<!-- #include file="RTGetUserRight.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
