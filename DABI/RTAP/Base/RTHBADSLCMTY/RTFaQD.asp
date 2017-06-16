<%  
  Dim fieldRole,fieldPa,DtlCnt  
  fieldRole=Split(FrGetUserRight("RTCustD",Request.ServerVariables("LOGON_USER")),";")
%>
<!-- #include virtual="/WebUtilityV4/DBAUDI/cType.inc" -->
<!-- #include virtual="/WebUtilityV4/DBAUDI/dataList.inc" -->
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
              sType=Right("000" &Cstr(aryKeyType(i)),3)
              If Instr(cTypeDate,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=Null
              If Instr(cTypeAuto,sType) > 0 Or (dspKey(i)=-1 And i<numberOfKey) Then
              Else
              '   On Error Resume Next
              runpgm=Request.ServerVariables("PATH_INFO") 
              select case ucase(runpgm)   
              ' 當程式為客訴資料維護作業時,因其dspkey(0)為自動搶號欄位(yymmdd+SEQ)，故不搬入值（由程式控制產生)   
                 case ucase("/webap/rtap/base/rthbadslcmty/RTFAQD.asp")  
                ' response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                      if i=0 then  
                         YY=cstr(datepart("yyyy",now())-1911)
                         mm=right(cstr("0" & cstr(datepart("m",now()))),2)
                         dd=right(cstr("0" & cstr(datepart("d",now()))),2)
                         YYMMDD="F-" & yy & mm & dd
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         sqlstr2="select max(FIXno) AS FIXno from HBADSLCMTYFIXH where  FIXno like '" & yymmdd & "%'" 
                         rsc.open sqlstr2,conn
                         if len(rsc("FIXno")) > 0 then
                            dspkey(i)=yymmdd & right("0000" & cstr(cint(mid(rsc("FIXno"),9,4)) + 1),4)
                         else
                            dspkey(i)=yymmdd & "0001"
                         end if                                                             
                      end if
                      rs.Fields(i).Value=dspKey(i)
                 case else
                      rs.Fields(i).Value=dspKey(i)
               end select
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
                 case else
                 'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                     rs.Fields(i).Value=dspKey(i)
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
    runpgm=Request.ServerVariables("PATH_INFO")
    ' 當程式為ADSL社區基本資料維護作業時,將sql自行產生之identify值dspkey(0)讀出至畫面
        'response.write "accessmode=" & accessmode &";sw=" & sw & "<BR>"
   ' 當程式為客訴處理措施紀錄時,因其dspkey(1)為identify欄位，故不搬入值（由sql自行產生)
    IF accessMode="A" THEN
      if ucase(runpgm)=ucase("/webap/rtap/base/rthbadslcmty/RTfaqd.asp") then
         rs.open "select max(FIXno) AS FIXNO from HBADSLCMTYFIXH ",conn
         if not rs.eof then
            dspkey(0)=rs("FIXno")
         end if
         rs.close
      end if        
    END IF
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
<body bgcolor="#FFFFFF" text="#0000FF"  background="backgroup.jpg" bgproperties="fixed">
<form method="post" id="form">
<input type="text" name="sw" value="<%=sw%>" style="display:none;" ID="Text2">
<input type="text" name="reNew" value="N" style="display:none;" ID="Text3">
<input type="text" name="rwCnt" value="<%=rwCnt%>" style="display:none;" ID="Text4">
<input type="text" name="accessMode" value="<%=accessMode%>" style="display:none;" ID="Text5">
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
  title="社區主機客訴處理"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT FIXNO, COMQ1, LINEQ1, COMTYPE, DEVICETYPE, RCVMAN, RCVTIME, MEMO1, MEMO2, MEMO3,dropdat,dropusr,closedat,closeusr,CMTYCUSID  FROM HBADSLCMTYFIXH WHERE FIXNO='' "
  sqlList="SELECT FIXNO, COMQ1, LINEQ1, COMTYPE, DEVICETYPE, RCVMAN, RCVTIME, MEMO1, MEMO2, MEMO3,dropdat,dropusr,closedat,closeusr,CMTYCUSID    FROM HBADSLCMTYFIXH WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=99
  extDBField2=99
  extDBField3=99
  userDefineSave="Yes"  
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    if len(dspkey(1)) = 0 then
       formValid=False
       message="社區序號不可空白"     
    elseif len(dspkey(2)) = 0 then
       formvalid=False
       message="主線序號不可空白"       
    elseif len(dspkey(3)) = 0 then
       formvalid=False
       message="社區方案不可空白"             
    elseif len(dspkey(4)) = 0 then
       formvalid=False
       message="社區種類不可空白"               
    elseif len(dspkey(5)) = 0 then
       formvalid=False
       message="受理人員不可空白"                    
    elseif len(dspkey(6)) = 0 then
       formvalid=False
       message="受理時間不可空白"             
    elseif len(dspkey(14)) = 0 then
       formvalid=False
       message="社區報竣用戶不可空白"                    
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
       call objEF2KDT.show(0)
       if objEF2KDT.strDateTime <> "" then
          document.all(clickkey).value = objEF2KDT.strDateTime
       end if
   End Sub 
   Sub SrTAG1()
      ' msgbox window.SRTAB1.style.display
       if window.SRTAB1.style.display="" then
          window.SRTAB1.style.display="none"
       elseif window.SRTAB1.style.display="none" then
          window.SRTAB1.style.display=""
       end if
   End Sub        
   Sub SrTAG2()
      ' msgbox window.SRTAB1.style.display
       if window.SRTAB2.style.display="" then
          window.SRTAB2.style.display="none"
       elseif window.SRTAB2.style.display="none" then
          window.SRTAB2.style.display=""
       end if
   End Sub        
   Sub SrTAG3()
      ' msgbox window.SRTAB1.style.display
       if window.SRTAB3.style.display="" then
          window.SRTAB3.style.display="none"
       elseif window.SRTAB3.style.display="none" then
          window.SRTAB3.style.display=""
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
Sub SrGetUserDefineKey()
 %>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
  <tr>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="23"><font color="#FFFFFF">案件編號</font></td>
    <td width="26%" bgcolor="#c0c0c0" height="23">
    <input name="key0" size="15" class="dataListData" value="<%=dspkey(0)%>" maxlength="15"  readonly ></td>
  </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
       if len(trim(dspkey(5))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                EUsrNc=V(1) 
                dspkey(5)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(5))
                V=split(rtnvalue,";")      
                EUsrNc=V(1)
        End if  
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
    If IsDate(dspKey(14)) or dspkey(17)="Y" or UCASE(trim(dataprotect))="READONLY" Then
       fieldPa=" class=""dataListData"" readonly "
       fieldpb=""
    Else
       fieldPa=""
       fieldpb=" onclick=""SrBtnOnClick"" "
    End If
    if UCASE(trim(dataprotect))="READONLY" then
       fieldpd=" disabled "
       fieldpb=""
    end if
%>

<table border="1" width="100%" cellspacing="0" cellpadding="0" height="0">

  <tr>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="23"><font  color="#FFFFFF">社區/主線序號</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="23">
    <%
    Set CONN=Server.CreateObject("ADODB.connection")
    DSN="DSN=RTLIB"
    CONN.OPEN DSN
    if dspkey(1)="" then  dspkey(1)= session("comq1")
    if dspkey(2)="" then dspkey(2)=session("lineq1")
    IF DSPKEY(3)="" THEN DSPKEY(3)=SESSION("comtype")
    Set rs=Server.CreateObject("ADODB.Recordset")
    SELECT CASE DSPKEY(3)
    '元訊599
       CASE "1"
            SQLxx="SELECT * FROM RTCMTY left outer join rtcmtysp on rtcmty.comq1=rtcmtysp.comq1 left outer join rtcounty on rtcmty.cutid=rtcounty.cutid WHERE rtcmty.COMQ1=" &  session("comq1") 
             rs.open sqlxx,conn
             if rs.eof then
                contact="":contacttel="":lineno="":lineip="":comaddr="":comn=""
             else
                contact=rs("spname")
                if contacttel <> "" and rs("mobile") <> "" then
                   contacttel=rs("contacttel")  & "/" & rs("mobile")
                elseif contacttel <> "" and rs("mobile") = "" then
                   contacttel=rs("contacttel")
                elseif contacttel = "" and rs("mobile") <> "" then
                   contacttel=rs("mobile")
                else 
                   contacttel=""
                end if
                lineno=rs("t1no")
                lineip=rs("cusip") & "/" & rs("setip") & "/" & rs("netip")
                comaddr=rs("cutnc") & rs("township") & rs("addr")
                comn=rs("comn")
             end if
             rs.close
    '中華399   
       CASE "2"
            SQLxx="SELECT * FROM RTCUSTADSLCMTY WHERE CUTYID=" & DSPKEY(1) 
            rs.open sqlxx,conn
            if rs.eof then
               contact="":contacttel="":lineno="":lineip="":comaddr="":comn=""
            else
               contact=""
               contacttel=""
               lineno=rs("cmtytel")
               lineip=rs("ipaddr")
               comaddr=rs("equipaddr")
               comn=rs("comn")
            end if
            rs.close
    '速博399   
       CASE "3"
            SQLxx="SELECT * FROM RTSPARQADSLCMTY WHERE CUTYID=" & DSPKEY(1) 
            rs.open sqlxx,conn
            if rs.eof then
               contact="":contacttel="":lineno="":lineip="":comaddr="":comn=""
            else
               contact=rs("ctycontact")
               contacttel=rs("ctycontacttel")
               lineno=rs("cmtytel")
               lineip=rs("ipaddr")
               comaddr=rs("equipaddr")
               comn=rs("comn")
            end if
            rs.close
    '東訊599   
       CASE "4"
            SQLxx="SELECT * FROM RTCMTY left outer join rtcmtysp on rtcmty.comq1=rtcmtysp.comq1 left outer join rtcounty on rtcmty.cutid=rtcounty.cutid WHERE rtcmty.COMQ1=" &  session("comq1") 
             rs.open sqlxx,conn
             if rs.eof then
                contact="":contacttel="":lineno="":lineip="":comaddr="":comn=""
             else
                contact=rs("spname")
                if contacttel <> "" and rs("mobile") <> "" then
                   contacttel=rs("contacttel")  & "/" & rs("mobile")
                elseif contacttel <> "" and rs("mobile") = "" then
                   contacttel=rs("contacttel")
                elseif contacttel = "" and rs("mobile") <> "" then
                   contacttel=rs("mobile")
                else 
                   contacttel=""
                end if
                lineno=rs("t1no")
                lineip=rs("cusip") & "/" & rs("setip") & "/" & rs("netip")
                comaddr=rs("cutnc") & rs("township") & rs("addr")
                comn=rs("comn")
             end if
             rs.close
     '東森499   
       CASE "5"
            SQLxx="SELECT *, RTEBTCMTYLINE.TOWNSHIP AS XTOWNSHIP, RTEBTCMTYLINE.VILLAGE AS XVILLAGE, RTEBTCMTYLINE.COD1 AS XCOD1, " _
                 &"RTEBTCMTYLINE.NEIGHBOR AS XNEIGHBOR, RTEBTCMTYLINE.COD2 AS XCOD2, RTEBTCMTYLINE.STREET AS XSTREET, RTEBTCMTYLINE.COD3 AS XCOD3, " _
                 &"RTEBTCMTYLINE.SEC AS XSEC,  RTEBTCMTYLINE.COD4 AS XCOD4, RTEBTCMTYLINE.LANE AS XLANE, RTEBTCMTYLINE.COD5 AS XCOD5, RTEBTCMTYLINE.TOWN AS XTOWN, " _
                 &"RTEBTCMTYLINE.COD6 AS XCOD6, RTEBTCMTYLINE.ALLEYWAY AS XALLEYWAY, RTEBTCMTYLINE.COD7 AS XCOD7, RTEBTCMTYLINE.NUM AS XNUM, RTEBTCMTYLINE.COD8 AS XCOD8, " _
                 &"RTEBTCMTYLINE.FLOOR AS XFLOOR, RTEBTCMTYLINE.COD9 AS XCOD9, RTEBTCMTYLINE.ROOM AS XROOM, RTEBTCMTYLINE.COD10 AS XCOD10, RTEBTCMTYLINE.ADDROTHER AS XADDROTHER " _
                 &"FROM RTEBTCMTYLINE INNER JOIN RTEBTCMTYH ON RTEBTCMTYLINE.COMQ1=RTEBTCMTYH.COMQ1 LEFT OUTER JOIN RTCOUNTY ON RTEBTCMTYLINE.CUTID=RTCOUNTY.CUTID WHERE RTEBTCMTYLINE.COMQ1=" &  session("comq1") & " AND RTEBTCMTYLINE.LINEQ1=" &  session("lineq1")
            rs.open sqlxx,conn
            if rs.eof then
               contact="":contacttel="":lineno="":lineip="":comaddr="":comn=""
            else
               contact=rs("contact")
               contacttel=rs("contacttel")
               lineno=rs("linetel")
               lineip=rs("lineip") & "/" & rs("subnet") & "/" & rs("gateway")
               comaddr=RS("CUTNC") & RS("XTOWNSHIP")
               IF RS("XVILLAGE")<> "" THEN COMADDR=COMADDR & RS("XVILLAGE") & RS("XCOD1")
               IF RS("XNEIGHBOR")<> "" THEN COMADDR=COMADDR & RS("XNEIGHBOR") & RS("XCOD2")
               IF RS("XSTREET")<> "" THEN COMADDR=COMADDR & RS("XSTREET") & RS("XCOD3")
               IF RS("XSEC")<> "" THEN COMADDR=COMADDR & RS("XSEC") & RS("XCOD4")
               IF RS("XLANE")<> "" THEN COMADDR=COMADDR & RS("XLANE") & RS("XCOD5")
               IF RS("XTOWN")<> "" THEN COMADDR=COMADDR & RS("XTOWN") & RS("XCOD6")
               IF RS("XALLEYWAY")<> "" THEN COMADDR=COMADDR & RS("XALLEYWAY") & RS("XCOD7")
               IF RS("XNUM")<> "" THEN COMADDR=COMADDR & RS("XNUM") & RS("XCOD8")
               IF RS("XFLOOR")<> "" THEN COMADDR=COMADDR & RS("XFLOOR") & RS("XCOD9")
               IF RS("XROOM")<> "" THEN COMADDR=COMADDR & RS("XROOM") & RS("XCOD10")
               IF RS("XADDROTHER")<> "" THEN COMADDR=COMADDR & RS("XADDROTHER") 
               comn=rs("comn")
            end if
            rs.close
       CASE ELSE
    END SELECT
    set rs=nothing
  %>    
    <input name="key1"  <%=dataprotect%>  size="5" readonly maxlength="5" <%=fieldpa%> class="dataListDATA" value="<%=dspkey(1)%>" >
    <font size=2>-</font>
    <input name="key2"  <%=dataprotect%>  size="4" readonly maxlength="4" <%=fieldpa%> class="dataListDATA" value="<%=dspkey(2)%>" ID="Text6">
    <font size=2><%=comn%></font></td>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="23"><font  color="#FFFFFF">社區方案</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="23">
    <%
    if dspkey(3)="1" then
       comtypenc="元訊599"
    elseif dspkey(3)="2" then
       comtypenc="中華399"
    elseif dspkey(3)="3" then
       comtypenc="速博399"
    elseif dspkey(3)="4" then
       comtypenc="東訊599"
    elseif dspkey(3)="5" then
       comtypenc="東森499"
    end if
    %>    
    <input name="key3"  <%=dataprotect%>  size="5" readonly maxlength="5" <%=fieldpa%> class="dataListDATA" value="<%=dspkey(3)%>" ID="Text7">
    <font size=2><%=comtypenc%> </font>
    </td>
  </tr>
  <tr>
  <%
  Set rs=Server.CreateObject("ADODB.Recordset")
  IF DSPKEY(4)="" THEN
     SELECT CASE DSPKEY(3)
       '元訊599
        CASE "1"
              SQLxx="SELECT * FROM RTCMTY WHERE rtcmty.COMQ1=" &  session("comq1") 
              rs.open sqlxx,conn
              IF NOT RS.EOF THEN
                 '合勤設備(固定)
                 IF RS("COMTYPE")="03" AND ( RS("CONNECTTYPE")="01" OR RS("CONNECTTYPE")="04" ) THEN 
                    DSPKEY(4)="04"
                 '合勤設備(固定+計量)   
                 ELSEIF RS("COMTYPE")="03" AND RS("CONNECTTYPE")="03" THEN
                    DSPKEY(4)="07"
                 '聚宇設備(固定)
                 ELSEIF RS("COMTYPE")="14" AND ( RS("CONNECTTYPE")="01" OR RS("CONNECTTYPE")="04" ) THEN 
                    DSPKEY(4)="05"   
                 '聚宇設備(固定+計量)
                 ELSEIF RS("COMTYPE")="14" AND RS("CONNECTTYPE")="03" THEN 
                    DSPKEY(4)="08"      
                 ELSEIF RS("CONNECTTYPE")="02"  THEN
                    DSPKEY(4)="09"
                 '東訊設備(固定)
                 ELSEIF ( RS("COMTYPE")="01" OR RS("COMTYPE")="02" OR RS("COMTYPE")="04" OR RS("COMTYPE")="05" OR RS("COMTYPE")="06" OR RS("COMTYPE")="07" OR RS("COMTYPE")="08" OR RS("COMTYPE")="09" OR RS("COMTYPE")="10" OR RS("COMTYPE")="11" OR RS("COMTYPE")="12" OR RS("COMTYPE")="13" OR RS("COMTYPE")="15" ) AND ( RS("CONNECTTYPE")="01" OR RS("CONNECTTYPE")="04" ) THEN
                    DSPKEY(4)="03"
                 '東訊設備(固定+計量)
                 ELSEIF ( RS("COMTYPE")="01" OR RS("COMTYPE")="02" OR RS("COMTYPE")="04" OR RS("COMTYPE")="05" OR RS("COMTYPE")="06" OR RS("COMTYPE")="07" OR RS("COMTYPE")="08" OR RS("COMTYPE")="09" OR RS("COMTYPE")="10" OR RS("COMTYPE")="11" OR RS("COMTYPE")="12" OR RS("COMTYPE")="13" OR RS("COMTYPE")="15" ) AND RS("CONNECTTYPE")="03" THEN
                    DSPKEY(4)="06"  
                 ELSE
                    DSPKEY(4)="" 
                 END IF
              END IF 
              RS.CLOSE
       '中華399   
        CASE "2" 
             DSPKEY(4)="01"
       '速博399   
        CASE "3" 
             DSPKEY(4)="02"
       '東訊599   
        CASE "4" 
              SQLxx="SELECT * FROM RTCMTY WHERE rtcmty.COMQ1=" &  session("comq1") 
              rs.open sqlxx,conn
              IF NOT RS.EOF THEN
                 '合勤設備(固定)
                 IF RS("COMTYPE")="03" AND ( RS("CONNECTTYPE")="01" OR RS("CONNECTTYPE")="04" ) THEN 
                    DSPKEY(4)="04"
                 '合勤設備(固定+計量)   
                 ELSEIF RS("COMTYPE")="03" AND RS("CONNECTTYPE")="03" THEN
                    DSPKEY(4)="07"
                 '聚宇設備(固定)
                 ELSEIF RS("COMTYPE")="14" AND ( RS("CONNECTTYPE")="01" OR RS("CONNECTTYPE")="04" ) THEN 
                    DSPKEY(4)="05"   
                 '聚宇設備(固定+計量)
                 ELSEIF RS("COMTYPE")="14" AND RS("CONNECTTYPE")="03" THEN 
                    DSPKEY(4)="08"      
                 ELSEIF RS("CONNECTTYPE")="02"  THEN
                    DSPKEY(4)="09"
                 '東訊設備(固定)
                 ELSEIF ( RS("COMTYPE")="01" OR RS("COMTYPE")="02" OR RS("COMTYPE")="04" OR RS("COMTYPE")="05" OR RS("COMTYPE")="06" OR RS("COMTYPE")="07" OR RS("COMTYPE")="08" OR RS("COMTYPE")="09" OR RS("COMTYPE")="10" OR RS("COMTYPE")="11" OR RS("COMTYPE")="12" OR RS("COMTYPE")="13" OR RS("COMTYPE")="15" ) AND ( RS("CONNECTTYPE")="01" OR RS("CONNECTTYPE")="04" ) THEN
                    DSPKEY(4)="03"
                 '東訊設備(固定+計量)
                 ELSEIF ( RS("COMTYPE")="01" OR RS("COMTYPE")="02" OR RS("COMTYPE")="04" OR RS("COMTYPE")="05" OR RS("COMTYPE")="06" OR RS("COMTYPE")="07" OR RS("COMTYPE")="08" OR RS("COMTYPE")="09" OR RS("COMTYPE")="10" OR RS("COMTYPE")="11" OR RS("COMTYPE")="12" OR RS("COMTYPE")="13" OR RS("COMTYPE")="15" ) AND RS("CONNECTTYPE")="03" THEN
                    DSPKEY(4)="06"  
                 ELSE
                    DSPKEY(4)="" 
                 END IF
              END IF 
              RS.CLOSE
       '東森499   
        CASE "5" 
             DSPKEY(4)="10"
        CASE ELSE
     END SELECT
  END IF
 ' SET RS=NOTHING
  CHECK4A="":CHECK4B="":CHECK4C="":CHECK4D="":CHECK4E="":CHECK4F="":CHECK4G="":CHECK4H="":CHECK4I="":CHECK4J=""
  IF DSPKEY(4)="01" THEN 
     CHECK4A=" CHECKED "
  ELSEIF DSPKEY(4)="02" THEN 
     CHECK4B=" CHECKED "
  ELSEIF DSPKEY(4)="03" THEN 
     CHECK4C=" CHECKED "
  ELSEIF DSPKEY(4)="04" THEN 
     CHECK4D=" CHECKED "
  ELSEIF DSPKEY(4)="05" THEN 
     CHECK4E=" CHECKED "
  ELSEIF DSPKEY(4)="06" THEN 
     CHECK4F=" CHECKED "
  ELSEIF DSPKEY(4)="07" THEN 
     CHECK4G=" CHECKED "
  ELSEIF DSPKEY(4)="08" THEN 
     CHECK4H=" CHECKED "
  ELSEIF DSPKEY(4)="09" THEN 
     CHECK4I=" CHECKED "
  ELSEIF DSPKEY(4)="10" THEN 
     CHECK4J=" CHECKED "                    
  ELSE
     CHECK4A="":CHECK4B="":CHECK4C="":CHECK4D="":CHECK4E="":CHECK4F="":CHECK4G="":CHECK4H="":CHECK4I="":CHECK4J=""
  END IF
  %>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="23"><font  color="#FFFFFF">設備種類</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="23" COLSPAN=3>
    <INPUT type="checkbox" name="key4" value="01"  class="dataListDATA" <%=fieldpd%><%=CHECK4A%>   <%=fieldpa%>   <%=fieldRole(1)%> bgcolor="silver" ID="Text15" ><font size=2>ADSL(中華)</font>
    <INPUT type="checkbox" name="key4" value="02"  class="dataListDATA" <%=fieldpd%><%=CHECK4B%>   <%=fieldpa%>   <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox3" ><font size=2>ADSL(速博)</font>
    <INPUT type="checkbox" name="key4" value="03"  class="dataListDATA" <%=fieldpd%><%=CHECK4C%>   <%=fieldpa%>   <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox4" ><font size=2>固定制(東訊)</font>
    <INPUT type="checkbox" name="key4" value="04"  class="dataListDATA" <%=fieldpd%><%=CHECK4D%>   <%=fieldpa%>   <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox5" ><font size=2>固定制(合勤)</font>
    <INPUT type="checkbox" name="key4" value="05"  class="dataListDATA" <%=fieldpd%><%=CHECK4E%>   <%=fieldpa%>   <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox8" ><font size=2>固定制(聚宇)</font>
    <INPUT type="checkbox" name="key4" value="06"  class="dataListDATA" <%=fieldpd%><%=CHECK4F%>   <%=fieldpa%>   <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox6" ><font size=2>固定制(東訊)+計量制</font><BR>
    <INPUT type="checkbox" name="key4" value="07"  class="dataListDATA" <%=fieldpd%><%=CHECK4G%>   <%=fieldpa%>   <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox9" ><font size=2>固定制(合勤)+計量制</font>
    <INPUT type="checkbox" name="key4" value="08"  class="dataListDATA" <%=fieldpd%><%=CHECK4H%>   <%=fieldpa%>   <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox10"><font size=2>固定制(聚宇)+計量制</font>    
    <INPUT type="checkbox" name="key4" value="09"  class="dataListDATA" <%=fieldpd%><%=CHECK4I%>   <%=fieldpa%>   <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox11"><font size=2>計量制</font>
    <INPUT type="checkbox" name="key4" value="10"  class="dataListDATA" <%=fieldpd%><%=CHECK4J%>   <%=fieldpa%>   <%=fieldRole(1)%> bgcolor="silver" ID="Checkbox7" ><font size=2>ADSL(東森)</font>
    </td>
  </tr>

  <tr>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="23"><font  color="#FFFFFF">社區聯絡人</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="23">
    <input  size="20"  maxlength="15"  READONLY   class="dataListDATA" value="<%=contact%>"></td>
    <td width="15%" bgcolor="#006666" class="DataListHead" height="23"><font  color="#FFFFFF">聯絡電話</font></td>
    <td width="35%" bgcolor="#c0c0c0"  height="23">
    <input   size="40" class="dataListData" readonly value="<%=contacttel%>"></td>
  </tr>
  <tr>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="23"><font  color="#FFFFFF">專線編號</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="23">
    <input size="20" readonly  class="dataListData" value="<%=lineno%>">
    </td>
    <td width="15%" bgcolor="#006666" class="DataListHead" height="23"><font  color="#FFFFFF">社區IP</font></td>
    <td width="35%" bgcolor="#c0c0c0"  height="23">
    <input  size="50" readonly  class="dataListData"  value="<%=lineip%>">
    </td>
  </tr>
  <tr>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="23"><font  color="#FFFFFF">社區地址</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="23" >
    <input  size="40" readonly  class="dataListData" ID="Text8" value="<%=comaddr%>">
    </td>
<%  s=""
    sx=" selected "
If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
    SELECT CASE DSPKEY(3)
        '元訊599
        CASE "1"
           sql="SELECT RTCUST.CUSID,RTOBJ.CUSNC FROM RTCUST LEFT OUTER JOIN RTOBJ ON RTCUST.CUSID=RTOBJ.CUSID WHERE RTCUST.DROPDAT IS NULL AND RTCUST.DOCKETDAT IS NOT NULL AND RTCUST.COMQ1=" & DSPKEY(1)
        '中華399   
        CASE "2" 
           sql="SELECT RTCUSTADSL.CUSID,RTOBJ.CUSNC FROM RTCUSTADSL LEFT OUTER JOIN RTOBJ ON RTCUSTADSL.CUSID=RTOBJ.CUSID WHERE RTCUSTADSL.DROPDAT IS NULL AND RTCUSTADSL.DOCKETDAT IS NOT NULL AND RTCUSTADSL.COMQ1=" & DSPKEY(1) 
        '速博399   
        CASE "3"
           sql="SELECT RTSPARQADSLCUST.CUSID,RTOBJ.CUSNC FROM RTSPARQADSLCUST LEFT OUTER JOIN RTOBJ ON RTSPARQADSLCUST.CUSID=RTOBJ.CUSID WHERE RTSPARQADSLCUST.DROPDAT IS NULL AND RTSPARQADSLCUST.DOCKETDAT IS NOT NULL AND RTSPARQADSLCUST.COMQ1="  & DSPKEY(1) 
        '東訊599   
        CASE "4" 
           sql="SELECT RTCUST.CUSID,RTOBJ.CUSNC FROM RTCUST LEFT OUTER JOIN RTOBJ ON RTCUST.CUSID=RTOBJ.CUSID WHERE RTCUST.DROPDAT IS NULL AND RTCUST.DOCKETDAT IS NOT NULL AND RTCUST.COMQ1=" & DSPKEY(1)
        '東森499   
        CASE "5" 
           sql="SELECT * FROM RTebtCUST WHERE RTEBTCUST.DROPDAT IS NULL AND RTEBTCUST.DOCKETDAT IS NOT NULL AND RTEBTCUST.CANCELDAT IS NULL AND RTEBTCUST.COMQ1=" & DSPKEY(1) & " AND LINEQ1=" & DSPKEY(2)
        CASE ELSE
     END SELECT
     If len(trim(dspkey(14))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
     else
          s=s & "<option value=""""" & sx & "></option>"  
     end if     
Else
    SELECT CASE DSPKEY(3)
        '元訊599
        CASE "1"
           sql="SELECT RTCUST.CUSID,RTOBJ.CUSNC FROM RTCUST LEFT OUTER JOIN RTOBJ ON RTCUST.CUSID=RTOBJ.CUSID WHERE RTCUST.DROPDAT IS NULL AND RTCUST.DOCKETDAT IS NOT NULL AND RTCUST.COMQ1=" & DSPKEY(1) & " AND RTCUST.CUSID='" & DSPKEY(14) & "' "
        '中華399   
        CASE "2" 
           sql="SELECT RTCUSTADSL.CUSID,RTOBJ.CUSNC FROM RTCUSTADSL LEFT OUTER JOIN RTOBJ ON RTCUSTADSL.CUSID=RTOBJ.CUSID WHERE RTCUSTADSL.DROPDAT IS NULL AND RTCUSTADSL.DOCKETDAT IS NOT NULL AND RTCUSTADSL.COMQ1=" & DSPKEY(1) & " AND RTCUSTADSL.CUSID='" & DSPKEY(14) & "' "
        '速博399   
        CASE "3"
           sql="SELECT RTSPARQADSLCUST.CUSID,RTOBJ.CUSNC FROM RTSPARQADSLCUST LEFT OUTER JOIN RTOBJ ON RTSPARQADSLCUST.CUSID=RTOBJ.CUSID WHERE RTSPARQADSLCUST.DROPDAT IS NULL AND RTSPARQADSLCUST.DOCKETDAT IS NOT NULL AND RTSPARQADSLCUST.COMQ1="  & DSPKEY(1) & " AND RTSPARQADSLCUST.CUSID='" & DSPKEY(14) & "' "
        '東訊599   
        CASE "4" 
           sql="SELECT RTCUST.CUSID,RTOBJ.CUSNC FROM RTCUST LEFT OUTER JOIN RTOBJ ON RTCUST.CUSID=RTOBJ.CUSID WHERE RTCUST.DROPDAT IS NULL AND RTCUST.DOCKETDAT IS NOT NULL AND RTCUST.COMQ1=" & DSPKEY(1) & " AND RTCUST.CUSID='" & DSPKEY(14) & "' "
        '東森499   
        CASE "5" 
           sql="SELECT * FROM RTebtCUST WHERE RTEBTCUST.DROPDAT IS NULL AND RTEBTCUST.DOCKETDAT IS NOT NULL AND RTEBTCUST.CANCELDAT IS NULL AND RTEBTCUST.COMQ1=" & DSPKEY(1) & " AND LINEQ1=" & DSPKEY(2) & " AND RTEBTCUST.CUSID='" & DSPKEY(14) & "' "
        CASE ELSE
     END SELECT
End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(14) Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("CUSNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>                  
    <td class=dataListHead>社區報修用戶</td>
    <td  bgcolor="#c0c0c0"  height="25">
    <select name="key14"  <%=fieldRole(1)%> <%=dataProtect%>  class=dataListEntry size="1" 
            style="text-align:left;" maxlength="2" ID="Select1"><%=s%></select>
    <% IF LEN(TRIM(DSPKEY(14))) > 0 THEN
          SELECT CASE DSPKEY(3)
            '元訊599
             CASE "1"
                sql="SELECT office + '-' + extension +','+home+',' + mobile as tel FROM RTCUST WHERE RTCUST.COMQ1=" & DSPKEY(1) & " AND RTCUST.CUSID='" & DSPKEY(14) & "' "
            '中華399   
             CASE "2" 
                sql="SELECT  office + '-' + extension +','+home+',' + mobile as tel  FROM RTCUSTADSL WHERE  RTCUSTADSL.COMQ1=" & DSPKEY(1) & " AND RTCUSTADSL.CUSID='" & DSPKEY(14) & "' "
            '速博399   
             CASE "3"
                sql="SELECT  office + '-' + extension +','+home+',' + mobile as tel  FROM RTSPARQADSLCUST  WHERE RTSPARQADSLCUST.COMQ1="  & DSPKEY(1) & " AND RTSPARQADSLCUST.CUSID='" & DSPKEY(14) & "' "
            '東訊599   
             CASE "4" 
                sql="SELECT office + '-' + extension +','+home+',' + mobile as tel  FROM RTCUST WHERE RTCUST.COMQ1=" & DSPKEY(1) & " AND RTCUST.CUSID='" & DSPKEY(14) & "' "
            '東森499   
             CASE "5" 
                sql="SELECT contacttel+','+ mobile as tel FROM RTebtCUST WHERE RTEBTCUST.COMQ1=" & DSPKEY(1) & " AND LINEQ1=" & DSPKEY(2) & " AND RTEBTCUST.CUSID='" & DSPKEY(14) & "' "
             CASE ELSE
          END SELECT
        '  response.Write sql
          rs.Open sql,conn
          CUSTTEL=rs("tel")
       ELSE
          CUSTTEL=""
       END IF
    %>
        <font size=2>連絡電話︰<%=custtel%></font>
    </td>     
  </tr>  
  <tr>
      <td width="15%" bgcolor="#006666" class="DataListHead" height="25"><font  color="#FFFFFF">受理日期</font></td>
    <td width="35%" bgcolor="#c0c0c0"  height="25">
    <input name="key6"  <%=dataprotect%> size="20"  <%=fieldpa%> readonly class="dataListentry"  value="<%=dspkey(6)%>" ID="Text12">
    <input type="button" id="B6"  name="B6" height="100%" width="100%" style="Z-INDEX: 1"  value="...."  onclick="Srbtnonclick">
    <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  alt="清除" id="C6"  name="C6"   style="Z-INDEX: 1" onclick="SrClear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
    </td> 
    <td width="14%" bgcolor="#006666" class="datalisthead" height="25"><font  color="#FFFFFF">受理人員</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="25">
    <input name="key5"  <%=dataprotect%> size="6"  <%=fieldpa%>  readonly  class="dataListdata"  value="<%=dspkey(5)%>"><font size=2><%=eusrnc%></font></td> 
  </tr> 
  <tr>
    <td width="15%" bgcolor="#006666" class="DataListHead" height="25"><font  color="#FFFFFF">作廢日期</font></td>
    <td width="35%" bgcolor="#c0c0c0"  height="25">
    <input name="key10"  <%=dataprotect%> size="20"  <%=fieldpa%> readonly class="dataListentry"  value="<%=dspkey(10)%>" ID="Text9">
   </td> 
    <td width="14%" bgcolor="#006666" class="datalisthead" height="25"><font  color="#FFFFFF">作廢人員</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="25">
            <%
            name=""
           if dspkey(11) <> "" then
              Set rs=Server.CreateObject("ADODB.Recordset")
              sqlxx=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(11) & "' "
              rs.Open sqlxx,conn
              if rs.eof then
                 name="(對象檔找不到測通人員)"
              else
                 name=rs("cusnc")
              end if
              rs.close
              SET RS=NOTHING
           end if
        %>

    <input name="key11"  <%=dataprotect%> size="6"  <%=fieldpa%>  readonly  class="dataListdata"  value="<%=dspkey(11)%>" ID="Text1"><font size=2><%=NAME%></font></td> 
  </tr> 
  <tr>
        <td width="15%" bgcolor="#006666" class="DataListHead" height="25"><font  color="#FFFFFF">結案日期</font></td>
    <td width="35%" bgcolor="#c0c0c0"  height="25">
    <input name="key12"  <%=dataprotect%> size="20"  <%=fieldpa%> readonly class="dataListentry"  value="<%=dspkey(12)%>" ID="Text11">
   </td> 
  <td width="14%" bgcolor="#006666" class="datalisthead" height="25"><font  color="#FFFFFF">結案人員</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="25">
        <% 
           name=""
           if dspkey(13) <> "" then
              Set rs=Server.CreateObject("ADODB.Recordset")
              sqlxx=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(13) & "' "
              rs.Open sqlxx,conn
              if rs.eof then
                 name="(對象檔找不到測通人員)"
              else
                 name=rs("cusnc")
              end if
              rs.close
              SET RS=NOTHING
           end if
        %>
    
    <input name="key13"  <%=dataprotect%> size="6"  <%=fieldpa%>  readonly  class="dataListdata"  value="<%=dspkey(13)%>" ID="Text10"><font size=2><%=NAME%></font></td> 
  </tr>     
</table>
<DIV ID="SRTAG1" onclick="srtag1" style="cursor:hand">
<table border="1" width="100%" cellspacing="0" cellpadding="0" height="0">  
  <tr> 
    <td width="100%" bgcolor="#a4bcdb" height="11"> 
   <p align="center"><font color="#000000" SIZE=2>用戶端故障原因描述</font></p></td> 
  </tr> 
  </table>
  </DIV>
<DIV id=SRTAB1>
<table border="1" width="100%" cellspacing="0" cellpadding="0" height="0" ID="Table3">  
  <tr> 
    <td width="100%"  bgcolor="#c0c0c0" height="11"> 
      <font size=2><p style="MARGIN-BOTTOM: 5px; MARGIN-TOP: 5px">
      問題：</p></FONT>
 <%
    Set rs=Server.CreateObject("ADODB.Recordset")
    sqlfaqD2="SELECT * FROM  HBADSLCMTYFIXD1 RIGHT OUTER JOIN " _
            &"RTCode ON  HBADSLCMTYFIXD1.OPT = RTCode.CODE AND FIXNO ='" & DSPKEY(0) & "' AND HBADSLCMTYFIXD1.CODE='1' " _
            &"WHERE RTCODE.KIND = 'J2'  "
    rs.open sqlfaqD2,conn
    Dtlcnt=0
    Do until rs.eof
       IF not IsNull(RS("FIXno")) then
          fieldpc=" checked "
       else
          fieldpc=""
       end if
    '-----
  %>
        <p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 5px; MARGIN-TOP: 5px">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" name="ext<%=Dtlcnt%>"  <%=fieldpc%> <%=fieldpd%> value="<%=rs("code")%>"><font size=2><%=rs("codenc")%></font></p>  
  <%
    dtlcnt=Dtlcnt+1
    rs.MoveNext
    loop
    rs.close
    set rs=nothing
  %>                               
      <p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 3px; MARGIN-TOP: 3px" align="center">
      </font>
      <TEXTAREA cols="100%" name="key7" rows=4   <%=fieldpa%>  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(7)%>"><%=dspkey(7)%></TEXTAREA></p>
    </td>
  </tr>
  </table>
  </DIV>
<DIV ID="SRTAG2" onclick="srtag2" style="cursor:hand">
  <table border="1" width="100%" cellspacing="0" cellpadding="0" height="0">
  <tr> 
    <td width="100%" bgcolor="#a4bcdb" height="11"> 
  <p align="center"><font color="#000000" SIZE=2>客服端故障原因描述</font></p></td> 
  </tr> 
  </table>
  </DIV>
<DIV id="SRTAB2">
  <table border="1" width="100%" cellspacing="0" cellpadding="0" height="0">
  <tr> 
    <td width="100%" bgcolor="#c0c0c0" height="11"> 
      <font size=2><p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 5px; MARGIN-TOP: 5px">
      問題：</p></FONT>
 <%
    Set rs=Server.CreateObject("ADODB.Recordset")
    sqlfaqD2="SELECT * FROM  HBADSLCMTYFIXD1 RIGHT OUTER JOIN " _
            &"RTCode ON  HBADSLCMTYFIXD1.OPT = RTCode.CODE AND FIXNO ='" & DSPKEY(0) & "' AND HBADSLCMTYFIXD1.CODE='2' " _
            &"WHERE RTCODE.KIND = 'J3'  "
    rs.open sqlfaqD2,conn
    Dtlcnt=0
    Do until rs.eof
       IF not IsNull(RS("FIXno")) then
          fieldpc=" checked "
       else
          fieldpc=""
       end if
    '-----
  %>
        <p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 5px; MARGIN-TOP: 5px">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" name="extA<%=Dtlcnt%>"  <%=fieldpc%> <%=fieldpd%> value="<%=rs("code")%>" ID="Checkbox1"><font size=2><%=rs("codenc")%></font></p>  
  <%
    dtlcnt=Dtlcnt+1
    rs.MoveNext
    loop
    rs.close
    set rs=nothing
  %>                               
      <p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 3px; MARGIN-TOP: 3px" align="center">
      </font>
      <TEXTAREA cols="100%" name="key8" rows=4   <%=fieldpa%>  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(8)%>" ID="Textarea1"><%=dspkey(8)%></TEXTAREA></p>
    </td>
  </tr>  
  </table>
  </div>
  <DIV ID="SRTAG3" onclick="srtag3" style="cursor:hand">
  <table border="1" width="100%" cellspacing="0" cellpadding="0" height="0">
  <tr> 
    <td width="100%"  bgcolor="#a4bcdb" height="11"> 
   <p align="center"><font color="#000000" SIZE=2>中華電信故障原因描述</font></p></td> 
  </tr> 
  </TABLE></div>
  <DIV  ID=SRTAB3 STYLE="DISPLAY:NONE">
  <TABLE border="1" width="100%" cellspacing="0" cellpadding="0" height="0">
  <tr> 
    <td width="100%"  bgcolor="#c0c0c0" height="11"> 
      <font size=2><p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 5px; MARGIN-TOP: 5px">
      問題：</p></FONT>
 <%
    Set rs=Server.CreateObject("ADODB.Recordset")
    sqlfaqD2="SELECT * FROM  HBADSLCMTYFIXD1 RIGHT OUTER JOIN " _
            &"RTCode ON  HBADSLCMTYFIXD1.OPT = RTCode.CODE AND FIXNO ='" & DSPKEY(0) & "' AND HBADSLCMTYFIXD1.CODE='3' " _
            &"WHERE RTCODE.KIND = 'J4'  "
    rs.open sqlfaqD2,conn
    Dtlcnt=0
    Do until rs.eof
       IF not IsNull(RS("FIXno")) then
          fieldpc=" checked "
       else
          fieldpc=""
       end if
    '-----
  %>
        <p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 5px; MARGIN-TOP: 5px">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" name="extB<%=Dtlcnt%>"  <%=fieldpc%> <%=fieldpd%> value="<%=rs("code")%>" ID="Checkbox2"><font size=2><%=rs("codenc")%></font></p>  
  <%
    dtlcnt=Dtlcnt+1
    rs.MoveNext
    loop
    rs.close
    set rs=nothing
  %>                               
      <p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 3px; MARGIN-TOP: 3px" align="center">
      </font>
      <TEXTAREA cols="100%" name="key9" rows=4   <%=fieldpa%>  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(9)%>" ID="Textarea2"><%=dspkey(9)%></TEXTAREA></p>
    </td>
  </tr>    
    </table>
  </DIV>
 <DIV ID="SRTAG4" onclick="srtag4" style="cursor:hand">    
 <table border="1" width="100%" cellspacing="0" cellpadding="0" height="0">
  <tr>
    <td width="100%"  bgcolor="#a4bcdb" height="11">
      <p align="center"><font color="#000000" SIZE=2>工務處理措施</font></p></td>
  </tr>
  </TABLE>
  <div ID=SRTAB4 STYLE="DISPLAY:NONE">
   <table border="1" width="100%" cellspacing="0" cellpadding="0" height="0" ID="Table2">
  <%
    Set rs=Server.CreateObject("ADODB.Recordset")
    s1=""
    sqlfaqd1="SELECT  HBADSLCMTYFIXSNDWORK.FIXNO, HBADSLCMTYFIXSNDWORK.PRTNO, " _
            &"HBADSLCMTYFIXSNDWORK.SENDWORKDAT, RTCode.CODENC, " _
            &"HBADSLCMTYFIXSNDWORK.CLOSEDAT, " _
            &"CASE WHEN HBADSLCMTYFIXSNDWORK.REALENGINEER = '' OR " _
            &"HBADSLCMTYFIXSNDWORK.REALENGINEER IS NULL " _
            &"THEN RTOBJ_1.SHORTNC ELSE RTOBJ.CUSNC END as processman " _
            &"FROM RTObj RTObj_1 RIGHT OUTER JOIN " _
            &"HBADSLCMTYFIXSNDWORK INNER JOIN " _
            &"HBADSLCMTYSNDWORKD1 ON " _
            &"HBADSLCMTYFIXSNDWORK.FIXNO = HBADSLCMTYSNDWORKD1.FIXNO AND " _
            &"HBADSLCMTYFIXSNDWORK.PRTNO = HBADSLCMTYSNDWORKD1.PRTNO INNER " _
            &"JOIN " _
            &"RTCode ON HBADSLCMTYSNDWORKD1.CODE = RTCode.CODE AND " _
            &"RTCode.KIND = 'J7' ON " _
            &"RTObj_1.CUSID = HBADSLCMTYFIXSNDWORK.REALCONSIGNEE LEFT OUTER JOIN " _
            &"RTObj INNER JOIN " _
            &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
            &"HBADSLCMTYFIXSNDWORK.REALENGINEER = RTEmployee.EMPLY " _
            &"WHERE  (HBADSLCMTYFIXSNDWORK.DROPDAT IS NULL) AND " _
            &"(HBADSLCMTYFIXSNDWORK.UNCLOSEDAT IS NULL) " _
            &"and HBADSLCMTYFIXSNDWORK.fixNO='" & dspkey(0) & "' "
           ' response.Write sqlfaqd1
    rs.open sqlfaqd1,conn,1,1
    Do until rs.eof
       s1=s1 & "派工日期：" & rs("SENDWORKDAT") & "　" & "處理人員：" & rs("processman") & chr(13)&chr(10) & "處理措施：" & chr(13)&chr(10)  & rs("codenc") & chr(13)&chr(10) & chr(13)&chr(10)
    rs.MoveNext
    loop
    rs.close
    set rs=nothing
  %>
  <tr>
    <td width="100%"  bgcolor="#c0c0c0"  height="117">
      <p align="center"><font ><TEXTAREA cols="100%" name=S1 rows=10 class="dataListdata" readonly ><%=S1%></TEXTAREA></font></p>
    </td>
  </tr>
</table>
</div></center>
<% 
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
    
    '--用戶故障原因描述
    DELFAQD1="delete from HBADSLCMTYFIXD1 where FIXno='" & dspkey(0) & "' AND CODE='1' "
    conn.Execute delFAQD1  
    I=0
    For i=0 to 99
        if len(trim(extdb(i))) > 0  then
           rs.Open "SELECT * FROM HBADSLCMTYFIXD1 WHERE FIXno='" &dspKey(0) &"' and CODE='1' AND OPT='" & extDB(i) & "'" ,conn,3,3
           If rs.Eof Or rs.Bof Then
              rs.AddNew
              rs("FIXno")=dspKey(0)
              RS("CODE")="1"
              rs("OPT")=extDB(i)          
           End If
           rs("Eusr")=dspkey(5)
           rs("Edat")=now()
           rs.Update
           rs.Close
        end if
    Next
    '--客服故障原因描述
    I=0
    DELFAQD1="delete from HBADSLCMTYFIXD1 where FIXno='" & dspkey(0) & "' AND CODE='2' "
    conn.Execute delFAQD1  
    For i=0 to 99
   
        if len(trim(extdb2(i))) > 0  then
         'RESPONSE.Write "AAA"
           rs.Open "SELECT * FROM HBADSLCMTYFIXD1 WHERE FIXno='" &dspKey(0) &"' and CODE='2' AND OPT='" & extDB2(i) & "'" ,conn,3,3
           If rs.Eof Or rs.Bof Then
              rs.AddNew
              rs("FIXno")=dspKey(0)
              RS("CODE")="2"
              rs("OPT")=extDB2(i)          
           End If
           rs("Eusr")=dspkey(5)
           rs("Edat")=now()
           rs.Update
           rs.Close
        end if
    Next    
    '--中華電信故障原因描述
    I=0
    DELFAQD1="delete from HBADSLCMTYFIXD1 where FIXno='" & dspkey(0) & "' AND CODE='3' "
    conn.Execute delFAQD1  
    For i=0 to 99

        if len(trim(extdb3(i))) > 0  then
            RESPONSE.Write "BBB"
           rs.Open "SELECT * FROM HBADSLCMTYFIXD1 WHERE FIXno='" &dspKey(0) &"' and CODE='3' AND OPT='" & extDB3(i) & "'" ,conn,3,3
           If rs.Eof Or rs.Bof Then
              rs.AddNew
              rs("FIXno")=dspKey(0)
              RS("CODE")="3"
              rs("OPT")=extDB3(i)          
           End If
           rs("Eusr")=dspkey(5)
           rs("Edat")=now()
           rs.Update
           rs.Close
        end if
    Next        
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
End Sub
' -------------------------------------------------------------------------------------------- 
%>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<!-- #include file="RTGetUserRight.inc" -->
