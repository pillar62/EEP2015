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
                   case ucase("/webap/rtap/base/RTLessorAVScmty/RTLessorAVSCUSTfaqd.asp")
                    'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       if i <> 1 then rs.Fields(i).Value=dspKey(i)    
                       if i=1 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         cusidxx="FAQ" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
                         rsc.open "select max(FAQNO) AS FAQNO from RTLessorAVSCustFAQH where  FAQNO LIKE '" & cusidxx & "%' " ,conn
                         if len(trim(rsc("FAQNO"))) > 0 then 
                            dspkey(1)=cusidxx & right("0000" & cstr(cint(right(rsc("FAQNO"),4)) + 1),4)
                         else
                            dspkey(1)=cusidxx & "0001"
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
                 case ucase("/webap/rtap/base/RTLessorAVScmty/RTLessorAVSCustFAQd.asp")
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
       if ucase(runpgm)=ucase("/webap/rtap/base/RTLessorAVScmty/RTLessorAVSCustFAQD.asp") then
          cusidxx="FAQ" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
          rsc.open "select max(FAQNO) AS FAQNO from RTLessorAVSCustFAQH where  FAQNO LIKE '" & cusidxx & "%' " ,conn
          if not rsC.eof then
            dspkey(1)=rsC("FAQNO")
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
  numberOfKey=2
  title="AVS-City用戶客服資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT   CUSID,FAQNO,RCVDAT,SERVICETYPE,CONTACTTEL,MOBILE,EMAIL,PRTDAT,FINISHDAT,FUSR,CANCELDAT," _
                     &" CANCELUSR,EUSR,EDAT,UUSR,UDAT,MEMO,SNDWORK,SNDUSR,SNDPRTNO,SNDCLOSEDAT," _
                     &" CALLBACKDAT, CALLBACKUSR,PRTUSR, FAQMAN, RCVUSR, IOLINE " _
             &"FROM  RTLessorAVSCustfaqh WHERE cusid='' "
  sqlList="SELECT CUSID,FAQNO,RCVDAT,SERVICETYPE,CONTACTTEL,MOBILE,EMAIL,PRTDAT,FINISHDAT,FUSR,CANCELDAT," _
                     &" CANCELUSR,EUSR,EDAT,UUSR,UDAT,MEMO,SNDWORK,SNDUSR,SNDPRTNO,SNDCLOSEDAT," _
                     &" CALLBACKDAT, CALLBACKUSR,PRTUSR, FAQMAN, RCVUSR, IOLINE " _
             &"from RTLessorAVSCustfaqh WHERE "
  userDefineRead="Yes"      
  userDefineSave="Yes"       
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=99
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
  if len(trim(dspkey(3)))=0 then
       formValid=False
       message="客服類別不可空白"   
  elseif len(trim(dspkey(4)))=0 and len(trim(dspkey(5)))=0 then
       formValid=False
       message="聯絡電話和行動電話至少需輸入一項"                
  end if
  '檢查客戶主檔狀態是否允許建立客服資料︰(1)已退租則不可轉派工單



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

   Sub SrTAG2()
      ' msgbox window.SRTAB1.style.display
       if window.SRTAB2.style.display="" then
          window.SRTAB2.style.display="none"
       elseif window.SRTAB2.style.display="none" then
          window.SRTAB2.style.display=""
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
  
   Sub SrTAG6()
      ' msgbox window.SRTAB1.style.display
       if window.SRTAB6.style.display="" then
          window.SRTAB6.style.display="none"
       elseif window.SRTAB6.style.display="none" then
          window.SRTAB6.style.display=""
       end if
   End Sub                  
   Sub SrTAG7()
      ' msgbox window.SRTAB1.style.display
       if window.SRTAB7.style.display="" then
          window.SRTAB7.style.display="none"
       elseif window.SRTAB7.style.display="none" then
          window.SRTAB7.style.display=""
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
         <td width="15%" class=dataListHead>用戶序號</td>
           <td width="15%"  bgcolor="silver">
           <input type="text" name="key0"
                 <%=fieldRole(1)%> readonly size="15" value="<%=dspKey(0)%>" maxlength="15" class=dataListdata></td>
           <td width="10%" class=dataListHead>客服單號</td>
           <td width="15%"  bgcolor="silver">
           <input type="text" name="key1"
                 <%=fieldRole(1)%> readonly size="13" value="<%=dspKey(1)%>" maxlength="13" class=dataListdata></td>                 
  </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(12))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(12)=V(0)
			   	dspkey(25)=V(0)
        End if  
       dspkey(13)=now()
       dspkey(3)="06"
    else
        if len(trim(dspkey(14))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(14)=V(0)
        End if         
        dspkey(15)=now()
    end if      
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t

    '客服單結案後 protect
    If len(trim(dspKey(8))) > 0  Then
       fieldPa=" class=""dataListData"" readonly "
       fieldPb=""
       fieldPC=" class=""dataListData"" readonly "
       fieldpD=" disabled "
    Else
       fieldPa=""        
       fieldPC=""
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
  <span id="tags1" class="dataListTagsOn">AVS-City用戶客服資訊</span>
                                                            
  <div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td>
<td>     
      <DIV ID="SRTAG0" onclick="srtag0" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
    <tr>    <td bgcolor="BDB76B" align="center">客戶基本資料</td></tr></table></DIV>
    <DIV ID=SRTAB0 >
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3">
<tr>
     <%sql="SELECT * FROM RTLESSORAVSCUST LEFT OUTER JOIN RTLESSORAVSCMTYH ON RTLESSORAVSCUST.COMQ1=RTLESSORAVSCMTYH.COMQ1 " _
          &" LEFT OUTER JOIN RTLESSORAVSCMTYLINE ON RTLESSORAVSCUST.COMQ1=RTLESSORAVSCMTYLINE.COMQ1 AND " _
          &"RTLESSORAVSCUST.LINEQ1=RTLESSORAVSCMTYLINE.LINEQ1 WHERE CUSID='" & DSPKEY(0) & "'" 
       RS.OPEN SQL,CONN
       IF RS.EOF THEN
          COMN=""
          LINEIP=""
          CUSTIP=""
       ELSE
          COMN=RS("COMN")
          LINEIP=RS("LINEIP")
          IF RS("IP11") <> "" THEN
             CUSTIP=RS("IP11")& "." & RS("IP12")& "." & RS("IP13")& "." & RS("IP14")
          ELSE
             CUSTIP=""
          END IF
       END IF
       RS.CLOSE
     %>
    <td width="10%" class=dataListsearch>社區名稱</td>
    <td width="23%" bgcolor="silver" >
    <%if len(trim(COMN)) > 0 then %>
    <FONT SIZE=2 color=red><%=COMN%></FONT>
    <% else %>
    <FONT SIZE=2>&nbsp;</FONT>
    <%end if %>
    </td>        
    <td width="10%" class=dataListsearch>社區IP</td>
    <td width="23%" bgcolor="silver" >     
    <%if len(trim(LINEIP)) > 0  then %>
    <FONT SIZE=2 color=red><%=LINEIP%></FONT>
    <%else%>
     <FONT SIZE=2>&nbsp;</FONT>
    <%end if %>
    </td>   
    <td width="10%" class=dataListsearch>用戶IP</td>
    <td width="23%" bgcolor="silver" >     
    <%if len(trim(CUSTIP)) > 0  then %>
    <FONT SIZE=2 color=red><%=CUSTIP%></FONT>
    <%else%>
     <FONT SIZE=2>&nbsp;</FONT>
    <%end if %>
    </td>       
</tr>

<%
    sql="SELECT isnull(b.cutnc,'')+a.township2+a.raddr2 as raddr, a.cusnc, a.email, a.contacttel, " &_
		"a.mobile, a.dropdat, a.duedat, c.codenc as casekindnc, d.codenc as paycyclenc " &_
		"FROM RTLessorAVSCUST a " &_
		"left outer join RTCounty b on a.cutid2 = b.cutid " &_
		"left outer join RTCode c on c.code = a.casekind and c.kind ='O9' " &_
		"left outer join RTCode d on d.code = a.paycycle and d.kind ='M8' " &_
		"WHERE CUSID='" & DSPKEY(0) & "'" 
    RS.OPEN SQL,CONN
    IF RS.EOF THEN
       	RADDR=""
        CUSNC=""
        EMAIL=""
        CONTACTTEL=""
        MOBILE=""
        dropdat=""
        duedat=""
		casekindnc=""
		paycyclenc=""
    ELSE
        RADDR=RS("RADDR")
        CUSNC=RS("CUSNC")
        EMAIL=RS("EMAIL")
        CONTACTTEL=RS("CONTACTTEL")
        MOBILE=RS("MOBILE")
        dropdat=rs("dropdat")
        duedat=rs("duedat")
        casekindnc=rs("casekindnc")
        paycyclenc=rs("paycyclenc")
    END IF
    RS.CLOSE
%>
<tr>
    <td width="10%" class=dataListsearch>用戶名稱</td>
	<td width="23%" bgcolor="silver" ><FONT SIZE=2 color=red><%=CUSNC%></FONT></td>

    <td width="10%" class=dataListsearch>用戶退租日</td>
    <td width="23%" bgcolor="silver" >
    <%if len(trim(dropdat)) > 0 then %>
    <FONT SIZE=2 color=red><%=dropdat%></FONT>
    <% else %>
    <FONT SIZE=2>&nbsp;</FONT>
    <%end if %>
    </td>

    <td width="10%" class=dataListsearch>用戶到期日</td>
    <td width="23%" bgcolor="silver" >     
    <%if duedat > now() then %>
    <FONT SIZE=2 color=red><%=duedat%></FONT>
    <%else%>
     <FONT SIZE=2><%=duedat%></FONT>
    <%end if %>
    </td>   
</tr>

<tr>
    <td width="10%" class=dataListsearch>方案類型</td>
		<td width="23%" bgcolor="silver" ><FONT SIZE=2 color=red><%=casekindnc%></FONT>
	</td>
    <td width="10%" class=dataListsearch>繳費週期</td>
		<td width="23%" bgcolor="silver" colspan=3><FONT SIZE=2 color=red><%=paycyclenc%></FONT>
	</td>
</tr>

<tr>
    <td width="10%" class=dataListsearch>用戶裝機地址</td>
	<td width="23%" bgcolor="silver" colspan=5><FONT SIZE=2 color=red><%=RADDR%></FONT></td>
</tr>

</TABLE>
</DIV>
      <DIV ID="SRTAG2" onclick="srtag2" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
    <tr><td bgcolor="BDB76B" align="center">客服資料內容</td></tr></table></DIV>
    <DIV ID=SRTAB2 >
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3">
<tr>
    <td width="10%" class=dataListHEAD>報修人</td>
	<td  height="23" bgcolor="silver">   
		<% IF DSPKEY(24)="" THEN DSPKEY(24)=CUSNC %>
             <input type="text" name="key24"  size="15"  value="<%=dspKey(24)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListentry">
	</TD>

    <td width="10%" class=dataListHEAD>inbound / outbound</td>
    <td height="23" bgcolor="silver" colspan=3>
		<select name="key26" size="1" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListentry">
		<% if len(dspkey(26)) = 0 then %> 
			<option value=''></option>
			<option value='I'>用戶來電</option>
			<option value='O'>客服Call Out</option>
		<% else %>
			<option value='<%=dspkey(26)%>' class="datalistdata"><%=dspkey(26)%> </option>
		<% End if %>
		</select>
	</td> 

</tr>

<TR>         
         <td    class="dataListHEAD" height="23">聯絡電話</td>               
        <td  height="23" bgcolor="silver" >   
          <% IF DSPKEY(4)="" THEN DSPKEY(4)=CONTACTTEL %>
             <input type="text" name="key4"  size="15"  value="<%=dspKey(4)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListentry" ID="Text56">
        </TD>      
  <td   class="dataListHEAD" height="23">行動電話</td>               
        <td height="23" bgcolor="silver" >
        <% IF DSPKEY(5)="" THEN DSPKEY(5)=MOBILE %>
            <input type="text" name="key5"  size="20"  value="<%=dspKey(5)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListentry" ID="Text56">
        </td>        
  <td    class="dataListHEAD" height="23">EMAIL</td>               
        <td  height="23" bgcolor="silver" >
        <% IF DSPKEY(6)="" THEN DSPKEY(6)=EMAIL %>
            <input type="text" name="key6"  size="30" value="<%=dspKey(6)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListentry" ID="Text56">
        </td>   
</tr>

<tr><td  class="dataListHEAD" height="23">受理人員</td>                                 
        <td  height="23" bgcolor="silver">
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  Then  
       sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC " _
		  &"FROM   RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID " _
		  &"WHERE  (DEPT='B400' or RTEmployee.emply ='" &dspkey(25)& "') AND TRAN2='' order by RTObj.CUSNC"
       If len(trim(dspkey(25))) < 1 Then
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
		  &"WHERE  (DEPT='B400' or RTEmployee.emply ='" &dspkey(25)& "') AND TRAN2='' AND RTEmployee.emply ='" &dspkey(25)& "' order by RTObj.CUSNC"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("EMPLY")=dspkey(25) Then sx=" selected "
       s=s &"<option value=""" &rs("EMPLY") &"""" &sx &">" &rs("CUSNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
   <select size="1" name="key25" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
        <%=s%>
   </select></td>

    <td width="10%" class=dataListHEAD>客服收件日</td>
    <td width="23%" bgcolor="silver" >
        <%IF DSPKEY(2)="" THEN DSPKEY(2)=NOW() %> 
        <input type="text" name="key2" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               value="<%=dspKey(2)%>" READONLY size="25" class=dataListDATA>
     </td>        
    <td width="10%" class=dataListHEAD>客服類別</td>
    <td width="23%" bgcolor="silver" >
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 and len(trim(dspkey(18)))=0 Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='N4' " 
       If len(trim(dspkey(3))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='N4' AND CODE='" & dspkey(3) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(3) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>
   <select size="1" name="key3" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35" >                                                                  
        <%=s%>
   </select></td></TR>

<tr>
        <td  class="dataListHEAD" height="23">建檔人員</td>                                 
        <td  height="23" bgcolor="silver">
		<input type="text" name="key12" size="6" READONLY value="<%=dspKey(12)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA"><font size=2><%=SrGetEmployeeName(dspKey(12))%></font>
        </td>  
        <td  class="dataListHEAD" height="23">建檔日期</td>                                 
        <td  height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key13" size="25" READONLY value="<%=dspKey(13)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>  
<tr>
        <td  class="dataListHEAD" height="23">修改人員</td>                                 
        <td  height="23" bgcolor="silver">
	    <input type="text" name="key14" size="6" READONLY value="<%=dspKey(14)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA"><font size=2><%=SrGetEmployeeName(dspKey(14))%></font>
        </td>

        <td  class="dataListHEAD" height="23">修改日期</td>                                 
        <td  height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key15" size="25" READONLY value="<%=dspKey(15)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
   </tr>                 
  </table>     
  </DIV>
   <DIV ID="SRTAG4" onclick="srtag4" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr><td bgcolor="BDB76B" align="center">客服單處理進度狀態</td></tr></table></DIV>
    <DIV ID=SRTAB4 >  
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
        <tr>
        <td  WIDTH="10%" class="dataListHEAD" height="23">轉派工單日期</td>                                 
        <td  WIDTH="23%" height="23" bgcolor="silver" >
        <input type="text" name="key17" size="10" READONLY value="<%=dspKey(17)%>" <%=fieldpC%> <%=fieldRole(1)%> class="dataListDATA" ID="Text56" >
 
        </td>
        <td  WIDTH="10%" class="dataListHEAD" height="23">轉派工單人員</td>                                 
        <td  WIDTH="23%" height="23" bgcolor="silver" >
        <input type="text" name="key18" size="6" READONLY value="<%=dspKey(18)%>"  <%=fieldRole(1)%> class="dataListdata"><font size=2><%=SrGetEmployeeName(dspKey(18))%></font>
      
       <td  WIDTH="10%"  class="dataListHEAD" height="23">派工單號</td>               
        <td  WIDTH="23%" height="23" bgcolor="silver" >   
        <input type="text" name="key19" size="13" readonly value="<%=dspKey(19)%>" <%=fieldRole(1)%><%=fieldpa%><%=dataProtect%> class="dataListdata" maxlength="3" onchange="srrecalulate()">
         </td>
</TR>
        <tr>
        <td   class="dataListHEAD" height="23">派工單列印日</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" name="key7" size="10" READONLY value="<%=dspKey(7)%>"  <%=fieldRole(1)%> class="dataListdata" ID="Text57">
 
        <td   class="dataListHEAD" height="23">派工單列印人員</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" name="key23" size="6" READONLY value="<%=dspKey(23)%>" <%=fieldpC%> <%=fieldRole(1)%> class="dataListdata" ID="Text56" ><font size=2><%=SrGetEmployeeName(dspKey(23))%></font>
        </td>
        <td  class="dataListHEAD" height="23">派工單結案日</td>                                 
        <td  height="23" bgcolor="silver" >
        <input type="text" name="key20" size="10" READONLY value="<%=dspKey(20)%>"  <%=fieldRole(1)%> class="dataListdata" ID="Text57">
 </TR>
        <tr>
        <td  class="dataListHEAD" height="23">客服回CALL日</td>                                 
        <td   height="23" bgcolor="silver" >
        <input type="text" name="key21" size="10" READONLY value="<%=dspKey(21)%>" <%=fieldpC%> <%=fieldRole(1)%> class="dataListdata" ID="Text56" >
        </td>
       <td    class="dataListHEAD" height="23">客服回CALL人員</td>               
        <td  height="23" bgcolor="silver" colspan=3>   
        <input type="text" name="key22" size="6" value="<%=dspKey(22)%>" <%=fieldRole(1)%><%=fieldpa%><%=dataProtect%> class="dataListdata" maxlength="3" ><font size=2><%=SrGetEmployeeName(dspKey(22))%></font>
        </td>
</TR>
       <tr>
       <td   class="dataListHEAD" height="23">客服單結案日</td>                                 
        <td   height="23" bgcolor="silver">
        <input type="text" name="key8" size="10" value="<%=dspKey(8)%>"  <%=fieldpa%><%=fieldRole(1)%> readonly class="dataListdata" ID="Text41">
         </td>
        <td    class="dataListHEAD" height="23">結案人員</td>                                 
        <td    height="23" bgcolor="silver" COLSPAN=3>
        <input type="text" name="key9" size="6" value="<%=dspKey(9)%>" <%=fieldRole(1)%> readonly class="dataListDATA"><font size=2><%=SrGetEmployeeName(dspKey(9))%></font>
        </td></tr>           
       <tr>
       <td   class="dataListHEAD" height="23">作廢日期</td>                                 
        <td   height="23" bgcolor="silver">
        <input type="text" name="key10" size="10" value="<%=dspKey(10)%>"  <%=fieldpa%><%=fieldRole(1)%> readonly class="dataListdata" ID="Text41">
         </td>
        <%  name="" 
           if dspkey(11) <> "" then
              sql=" select rtobj.cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(11) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>         
        <td    class="dataListHEAD" height="23">作廢人員</td>                                 
        <td    height="23" bgcolor="silver" COLSPAN=3>
        <input type="text" name="key11" size="10" value="<%=dspKey(11)%>" <%=fieldRole(1)%> readonly class="dataListDATA"><font size=2><%=SrGetEmployeeName(dspKey(11))%></font>
        </td></tr>           


  </table> 
  </DIV>
    <DIV ID="SRTAG6" onclick="SRTAG6" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="center">客戶問題描述</td></tr></table></DIV>
   <DIV ID="SRTAB6" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
    <TR><TD >
     <%
    Set rsxx=Server.CreateObject("ADODB.Recordset")
    sqlfaqlist="SELECT * FROM RTLessorAVSCustFaqList RIGHT OUTER JOIN " _
            &"RTCode ON RTLessorAVSCustFaqList.faqcod = RTCode.CODE AND faqno ='" & DSPKEY(1) & "'" _
            &"WHERE RTCODE.KIND = 'D6' "
  '  response.write sqlfaqlist
    rsxx.open sqlfaqlist,conn
    Dtlcnt=0
    Do until rsxx.eof
       IF not IsNull(RSxx("faqno")) then
          fieldpc=" checked "
       else
          fieldpc=""
       end if
    '-----
  %>
        <p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 5px; MARGIN-TOP: 5px">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" name="ext<%=Dtlcnt%>"  <%=fieldpc%> <%=fieldpd%> value="<%=rsxx("code")%>"><font size=2><%=rsxx("codenc")%></font></p>  
  <%
    dtlcnt=Dtlcnt+1
    rsxx.MoveNext
    loop
    rsxx.close
    set rsxx=nothing
  %>                               
      <p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 3px; MARGIN-TOP: 3px" align="center">
      </font>
      </td>
      <tr><td bgcolor="BDB76B" align="center">客服處理過程描述</td></tr>
      <tr><td align=center>
     <TEXTAREA  cols="100%"  name="key16" rows=10  MAXLENGTH=800 <%=fieldpa%><%=fieldpb%> class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(16)%>" ID="Textarea1"><%=dspkey(16)%></TEXTAREA>
   </td></tr>
 </table> 
  </div> 
    <DIV ID="SRTAG7" onclick="SRTAG7" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="center">維修派工處理過程描述</td></tr></table></DIV>
   <DIV ID="SRTAB7" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
    <TR><TD >
      <%   Set rsxx=Server.CreateObject("ADODB.Recordset")
           sqlfaqlist="SELECT RTLessorAVSCustFaqSNDWORK.PRTNO,RTLessorAVSCustFaqSNDWORK.SENDWORKDAT,RTLessorAVSCustFaqSNDWORK.memo," _ 
                     &"CASE WHEN RTOBJ_2.SHORTNC <> '' THEN RTOBJ_2.SHORTNC ELSE RTOBJ_1.CUSNC END AS assign, " _
                     &"CASE WHEN RTOBJ_4.SHORTNC <> '' THEN RTOBJ_4.SHORTNC ELSE RTOBJ_3.CUSNC END AS real, " _
                     &"RTLessorAVSCustFaqSNDWORK.closedat, RTLessorAVSCustFaqSNDWORK.batchno, " _
                     &"SUM(CASE WHEN RTLessorAVSCustfaqHARDWARE.dropdat IS NULL AND RTLessorAVSCustfaqHARDWARE.QTY > 0 " _
                     &"THEN RTLessorAVSCustfaqHARDWARE.QTY ELSE 0 END) AS amt1, " _
                     &"SUM(CASE WHEN RTLessorAVSCustfaqHARDWARE.dropdat IS NULL AND RCVPRTNO <> '' " _
                     &"THEN RTLessorAVSCustfaqHARDWARE.QTY ELSE 0 END) AS amt2, " _
                     &"SUM(CASE WHEN RTLessorAVSCustfaqHARDWARE.dropdat IS NULL AND RCVPRTNO <> '' AND " _
                     &"RTLessorAVSCustfaqHARDWARE.rcvfinishdat IS NOT NULL THEN RTLessorAVSCustfaqHARDWARE.QTY ELSE 0 END) AS amt3 " _
                     &"FROM RTLessorAVSCustFaqSNDWORK LEFT OUTER JOIN RTObj RTObj_4 ON RTLessorAVSCustFaqSNDWORK.REALCONSIGNEE = " _
                     &"RTObj_4.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_2 INNER JOIN RTObj RTObj_3 ON " _
                     &"RTEmployee_2.CUSID = RTObj_3.CUSID ON RTLessorAVSCustFaqSNDWORK.REALENGINEER = RTEmployee_2.EMPLY " _
                     &"LEFT OUTER JOIN RTObj RTObj_2 ON RTLessorAVSCustFaqSNDWORK.ASSIGNCONSIGNEE = RTObj_2.CUSID " _
                     &"LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN RTObj RTObj_1 ON RTEmployee_1.CUSID = " _
                     &"RTObj_1.CUSID ON RTLessorAVSCustFaqSNDWORK.ASSIGNENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
                     &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON RTLessorAVSCustFaqSNDWORK.PRTUSR = " _
                     &"RTEmployee.EMPLY LEFT OUTER JOIN RTLessorAVScust ON RTLessorAVSCustFaqSNDWORK.cusid = RTLessorAVScust.cusid " _
                     &"LEFT OUTER JOIN RTLessorAVSCustfaqHARDWARE ON RTLessorAVSCustFaqSNDWORK.cusid = " _
                     &"RTLessorAVSCustfaqHARDWARE.CUSID AND RTLessorAVSCustFaqSNDWORK.PRTNO = RTLessorAVSCustfaqHARDWARE.PRTNO " _
                     &"WHERE RTLessorAVSCustFaqSNDWORK.faqno ='" & dspkey(1) & "' and RTLessorAVSCustFaqSNDWORK.dropdat is null " _
                     &"GROUP BY  RTLessorAVSCustFaqSNDWORK.PRTNO, RTLessorAVSCustFaqSNDWORK.SENDWORKDAT,RTLessorAVSCustFaqSNDWORK.memo, " _
                     &"CASE WHEN RTOBJ_2.SHORTNC <> '' THEN RTOBJ_2.SHORTNC ELSE RTOBJ_1.CUSNC  END, " _
                     &"CASE WHEN RTOBJ_4.SHORTNC <> '' THEN RTOBJ_4.SHORTNC ELSE RTOBJ_3.CUSNC  END, " _
                     &"RTLessorAVSCustFaqSNDWORK.closedat, RTLessorAVSCustFaqSNDWORK.batchno  "
           rsxx.open sqlfaqlist,conn
           IF rsxx.eof then
              xxmemo=""
              xxprtno=""
              xxsendworkdat=""
              xxassign=""
              xxREAL=""
              xxclosedat=""
              xxbatchno=""
              xxamt1=""
              xxamt2=""
              xxamt3=""
           else
              xxmemo=rsxx("memo")
              xxprtno=rsxx("prtno")
              xxsendworkdat=rsxx("sendworkdat")
              xxassign=rsxx("assign")
              xxREAL=rsxx("real")
              xxclosedat=rsxx("closedat")
              xxbatchno=rsxx("batchno")
              xxamt1=rsxx("amt1")
              xxamt2=rsxx("amt2")
              xxamt3=rsxx("amt3")
           end if
           rsxx.close
           set rsxx=nothing
      %>    
       <% if xxprtno <> "" then %>
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr class="dataListsearch"><td colspan=9 align=center>派工單內容</td></tr>
    <tr class="dataListHEAD"><td>派工單號</td><td>派工日期</td><td>預定維修人員</td><td>實際維修人員</td><td>派工單結案日</td><td>帳款編號</td><td>設備數量</td><td>轉領用單數量</td><td>已領數量</td></tr>
           <tr class="dataListentry">
           <td><%=xxprtno%></td>
           <td><%=xxsendworkdat%></td>
           <td><%=xxassign%></td>
           <td><%=xxREAL%></td>
           <td><%=xxclosedat%></td>
           <td><%=xxbatchno%></td>
           <td><%=xxamt1%></td>
           <td><%=xxamt2%></td>
           <td><%=xxamt3%></td>
           </tr>           
      </table>
      <%end if %>
     <%
    Set rsxx=Server.CreateObject("ADODB.Recordset")
    sqlfaqlist="SELECT * FROM RTLessorAVSCustFaqSndworkFixCode inner JOIN " _
            &"RTCode ON RTLessorAVSCustFaqSndworkFixCode.fixcod = RTCode.CODE AND faqno ='" & DSPKEY(1) & "'" _
            &"WHERE RTCODE.KIND = 'A9' "
  '  response.write sqlfaqlist
    rsxx.open sqlfaqlist,conn
    Dtlcnt=0
    Do until rsxx.eof
       IF not IsNull(RSxx("faqno")) then
          fieldpc=" checked "
       else
          fieldpc=""
       end if
    '-----
  %>
        <p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 5px; MARGIN-TOP: 5px">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" name="extzz<%=Dtlcnt%>"  <%=fieldpc%> disabled value="<%=rsxx("code")%>"><font size=2><%=rsxx("codenc")%></font></p>  
  <%
    dtlcnt=Dtlcnt+1
    rsxx.MoveNext
    loop
    rsxx.close
    set rsxx=nothing
  %>                               
      <p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 3px; MARGIN-TOP: 3px" align="center">
      </font>

    <!--  <tr><td bgcolor="BDB76B" align="center">維修處理描述</td></tr> -->
    <p align=center>
     <TEXTAREA  cols="100%"  name="xxx" rows=10 readonly MAXLENGTH=800  class="dataListdata"  <%=dataprotect%>  value="<%=xxmemo%>" ID="Textarea1"><%=xxmemo%></TEXTAREA>
    <% if xxprtno <> "" then %>
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr class="dataListsearch"><td colspan=9 align=center>派工設備明細</td></tr>
    <tr class="dataListHEAD"><td>項次</td><td>設備名稱/規格</td><td>數量</td><td>單價</td><td>金額</td><td>帳款編號</td><td>轉應收帳款日</td><td>領用單號</td><td>領用結案日</td></tr>
    <%
           Set rsxx=Server.CreateObject("ADODB.Recordset")
           sqlfaqlist="SELECT " _
                     &"RTLessorAVSCustFaqHARDWARE.seq, RTProdH.PRODNC + '--' + RTProdD1.itemnc + '('+ RTProdD1.SPEC+')' as pdname, " _
                     &"RTLessorAVSCustFaqHARDWARE.QTY, RTLessorAVSCustFaqHARDWARE.amt,RTLessorAVSCustFaqHARDWARE.QTY * RTLessorAVSCustFaqHARDWARE.amt as totamt, " _
                     &"RTLessorAVSCustFaqHARDWARE.BATCHNO,RTLessorAVSCustFaqHARDWARE.TARDAT,RTLessorAVSCustFaqHARDWARE.rcvprtno," _
                     &"RTLessorAVSCustFaqHARDWARE.rcvfinishdat FROM RTProdH RIGHT OUTER JOIN " _
                     &"RTLessorAVSCustFaqHARDWARE LEFT OUTER JOIN RTObj INNER JOIN RTEmployee ON " _
                     &"RTObj.CUSID = RTEmployee.CUSID ON RTLessorAVSCustFaqHARDWARE.DROPUSR = RTEmployee.EMPLY " _
                     &"LEFT OUTER JOIN HBwarehouse ON RTLessorAVSCustFaqHARDWARE.WAREHOUSE = HBwarehouse.WAREHOUSE " _
                     &"LEFT OUTER JOIN RTProdD1 ON RTLessorAVSCustFaqHARDWARE.PRODNO = RTProdD1.PRODNO AND " _
                     &"RTLessorAVSCustFaqHARDWARE.ITEMNO = RTProdD1.ITEMNO ON RTProdH.PRODNO = RTLessorAVSCustFaqHARDWARE.PRODNO " _
                     &"left outer join RTLessorAVScust on RTLessorAVSCustFaqHARDWARE.cusid=RTLessorAVScust.cusid " _
                     &"WHERE RTLessorAVSCustFaqHARDWARE.cusid='" & dspkey(0) & "' and " _
                     &"RTLessorAVSCustFaqHARDWARE.faqno='" & dspkey(1) & "' and " _
                     &"RTLessorAVSCustFaqHARDWARE.prtno='" & xxprtno & "' and RTLessorAVSCustFaqHARDWARE.dropdat is null order by RTLessorAVSCustFaqHARDWARE.seq " 
           rsxx.open sqlfaqlist,conn
           do until rsxx.eof %>
           <tr class="dataListentry">
           <td><%=rsxx("seq")%></td>
           <td><%=rsxx("pdname")%></td>
           <td><%=rsxx("QTY")%></td>
           <td><%=rsxx("amt")%></td>
           <td><%=rsxx("totamt")%></td>
           <td><%=rsxx("BATCHNO")%></td>
           <td><%=rsxx("TARDAT")%></td>
           <td><%=rsxx("rcvprtno")%></td>
           <td><%=rsxx("rcvfinishdat")%></td>
           </tr>           
        <% rsxx.MoveNext
           loop    
           rsxx.close
           set rsxx=nothing
      %>
      </table>
      <%end if %>
   </td></tr>
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
    Dim conn,rs
' Smode A:add U:update
' extDBField = n
' use extDB(i) for Screen ,and map it to DataBase
'
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open DSN
    Set rs=Server.CreateObject("ADODB.Recordset")
    Set comm=Server.CreateObject("ADODB.Command")
    
'------ RTObj ---------------------------------------------------
    DELFAQlist="delete from RTLessorAVScustfaqlist where faqno='" & dspkey(1) & "'"
    conn.Execute DELFAQlist  
    For i=0 to 99
        if len(trim(extdb(i))) > 0  then
           rs.Open "SELECT * FROM RTLessorAVScustfaqlist WHERE faqno='" &dspKey(1) &"' and faqcod='" & extDB(i) & "'" ,conn,3,3
           If rs.Eof Or rs.Bof Then
              rs.AddNew
              rs("cusid")=dspKey(0)
              rs("faqno")=dspKey(1)
              rs("faqcod")=extDB(i)          
           End If
           rs.Update
           rs.Close
        end if
    Next
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
End Sub
' -------------------------------------------------------------------------------------------- 
' --------------------------------------------------------------------------------------------  
%>
<!-- #include virtual="/Webap/include/checkid.inc" -->
<!-- #include virtual="/Webap/include/companyid.inc" -->
<!-- #include file="RTGetUserRight.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->