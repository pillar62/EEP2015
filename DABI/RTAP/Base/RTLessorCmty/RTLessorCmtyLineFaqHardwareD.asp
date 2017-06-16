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
              sType=Right("000" &Cstr(aryKeyType(i)),3)
              If Instr(cTypeDate,sType) > 0 And Len(Trim(dspKey(i))) = 0 Then dspKey(i)=Null
              If Instr(cTypeAuto,sType) > 0 Or (dspKey(i)=-1 And i<numberOfKey) Then
              Else
              '   On Error Resume Next
                runpgm=Request.ServerVariables("PATH_INFO") 
                select case ucase(runpgm)   
                   ' 當程式為ADSL社區基本資料維護作業時,因其dspkey(0)為identify欄位，故不搬入值（由sql自行產生)
                   case ucase("/webap/rtap/base/rtLESSORcmty/RTlessorCmtyLineFAQHARDWAREd.asp")
                       'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       IF I=4 THEN
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         rsc.open "select max(SEQ) AS SEQ from RTlessorCmtyLineFAQHARDWARE where COMQ1=" & DSPKEY(0) & " AND LINEQ1=" & DSPKEY(1) & " AND FAQNO='" & DSPKEY(2) & "' AND PRTNO='" & DSPKEY(3) & "' "  ,conn
                         if len(trim(rsc("SEQ"))) > 0 then 
                            dspkey(4)=rsc("SEQ") + 1
                         else
                            dspkey(4)=1
                         end if
                         rsc.close
                       END IF
                      'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       rs.Fields(i).Value=dspKey(i)     
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
                 case ucase("/webap/rtap/base/rtLESSORcmty/RTlessorCmtyLineFAQHARDWAREd.asp")
                    rs.Fields(i).Value=dspKey(i)
                  '   if i<>0 then rs.Fields(i).Value=dspKey(i)         
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
    ' 當程式為HB社區基本資料維護作業時,將sql自行產生之identify值dspkey(2)讀出至畫面
    if accessmode ="A" then
       runpgm=Request.ServerVariables("PATH_INFO")
       if ucase(runpgm)=ucase("/webap/rtap/base/rtLESSORcmty/RTlessorCmtyLineFAQHARDWAREd.asp") then
          rs.open "select max(SEQ) AS SEQ from RTlessorCmtyLineFAQHARDWARE WHERE  COMQ1=" & DSPKEY(0) & " AND LINEQ1=" & DSPKEY(1) & " AND FAQNO='" & DSPKEY(2) & "' AND PRTNO='" & DSPKEY(3) & "' " ,conn
          if not rs.eof then
            dspkey(4)=rs("SEQ")
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
  numberOfKey=5
  title="ET-City主線維修派工單設備資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT COMQ1,LINEQ1,FAQNO,PRTNO, SEQ, PRODNO, ITEMNO, QTY, DROPDAT, DROPREASON," _
             &"WAREHOUSE, DROPUSR,UNIT,amt,eusr,edat,uusr,udat,memo,BATCHNO,TARDAT,TUSR,HOSTNO, PRELEVELHOSTNO, PRELEVELPORTNO " _
             &"FROM RTlessorCmtyLineFaqHARDWARE WHERE COMQ1=0 "
  sqlList="SELECT COMQ1,LINEQ1,FAQNO,PRTNO, SEQ, PRODNO, ITEMNO, QTY, DROPDAT, DROPREASON, WAREHOUSE, " _
             &"DROPUSR,UNIT,amt,eusr,edat,uusr,udat,memo,BATCHNO,TARDAT,TUSR,HOSTNO, PRELEVELHOSTNO, PRELEVELPORTNO " _
             &"FROM RTlessorCmtyLineFaqHARDWARE WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=1
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    IF ACCESSMODE="A" THEN
       set connyy=server.CreateObject("ADODB.Connection")
       set rsyy=server.CreateObject("ADODB.recordset")
       connyy.Open dsn     
       sqlyy="SELECT * FROM RTlessorCmtyLineFaqSNDWORK WHERE COMQ1=" & DSPKEY(0) & " AND LINEQ1=" &  DSPKEY(1) & " AND FAQNO='" & DSPKEY(2) & "' AND PRTNO='" & DSPKEY(3) & "' "
       RSYY.Open SQLYY,CONNYY
       IF LEN(TRIM(RSYY("DROPDAT"))) > 0 OR LEN(TRIM(RSYY("CLOSEDAT"))) > 0 OR LEN(TRIM(RSYY("UNCLOSEDAT"))) > 0 then
          ERRFLG="Y"
       ELSE
          ERRFLG=""
       end if
       RSYY.Close
       CONNYY.Close
       SET RSYY=NOTHING
       SET CONNYY=NOTHING
    END IF
    if dspkey(22) <> "" then
       set connyy=server.CreateObject("ADODB.Connection")
       set rsyy=server.CreateObject("ADODB.recordset")
       set rszz=server.CreateObject("ADODB.recordset")
       connyy.Open dsn     
       '同一社區下的設備網路編號不可相同
       sqlyy="select count(*) as cnt from RTlessorCmtyLineFaqHardware where hostno='" & dspkey(22) & "' and COMQ1=" & DSPKEY(0) & " AND ( LINEQ1<>" &  DSPKEY(1) & " or FAQNO<>'" & DSPKEY(2) & "' or PRTNO<>'" & DSPKEY(3) & "' or seq<>" & dspkey(4) & ")"
       sqlzz="select count(*) as cnt from RTlessorCmtyLineHardware where hostno='" & dspkey(22) & "' and COMQ1=" & DSPKEY(0) 
       'response.write sqlyy
       RSYY.Open SQLYY,CONNYY
       RSzz.Open SQLzz,CONNYY
       if rsyy("cnt") > 0 then
          ERRFLG="X"
       end if
       if rszz("cnt") > 0 then
          ERRFLG="X"
       end if
       RSYY.Close
       RSzz.Close
       CONNYY.Close
       SET RSYY=NOTHING
       SET RSzz=NOTHING
       SET CONNYY=NOTHING
    end if

    IF LEN(DSPKEY(4))=0 THEN DSPKEY(4)=0
    IF LEN(DSPKEY(10))=0 THEN DSPKEY(10)=""
    IF ERRFLG="Y" THEN
       formValid=False
       message="派工單" & DSPKEY(3) & " 已執行庫存計算，不可再輸入設備明細"  
    ELSEIF ERRFLG="X" THEN
       formValid=False
       message="設備網路編號︰" & DSPKEY(22) & " 已被本社區其它設備使用，不可重複"    
    elseif LEN(TRIM(dspkey(23))) > 0 and LEN(TRIM(dspkey(24)))=0 then
       formValid=False
       message="上層設備網路編號不為空白時，必須輸入上層設備網路port號"   
   elseif LEN(TRIM(dspkey(24))) > 0 and LEN(TRIM(dspkey(23)))=0 then
       formValid=False
       message="上層設備網路port號不為空白時，必須輸入上層設備網路編號"          
    ELSEif LEN(TRIM(dspkey(5))) = 0 OR LEN(TRIM(dspkey(6))) = 0  then
       formValid=False
       message="請輸入設備名稱及規格" 
    ELSEif len(trim(DSPKEY(7))) = 0 OR NOT ISNUMERIC(DSPKEY(7))  THEN
       formValid=False
       message="請輸入設備數量或格式不符" 
    ELSEif len(trim(DSPKEY(13))) = 0 OR NOT ISNUMERIC(DSPKEY(13))  THEN
       formValid=False
       message="請輸入金額或格式不符"        
    ELSEif len(trim(DSPKEY(12))) = 0 THEN
       formValid=False
       message="請輸入數量單位" 
    ELSEif len(trim(DSPKEY(13))) = 0 THEN
       formValid=False
       message="請輸入設備收款金額" 
    ELSEif len(trim(DSPKEY(10))) = 0 AND CONSIGNEE="N"  THEN
       formValid=False
       message="請輸入設備出貨庫別"        
    ELSEif len(trim(DSPKEY(10))) <> 0 AND CONSIGNEE="Y"  THEN
       formValid=False
       message="主線開發為經銷商，不可輸入出貨庫別"               
    END IF       
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(16)=V(0)
        dspkey(17)=datevalue(now())
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
   Sub Srprod6onclick()
       prog="RTGetproddetail.asp"
       prog=prog & "?KEY=" & document.all("KEY5").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:700px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key6").value =  trim(Fusrid(0))
          DOCUMENT.ALL("DEVICENAME").VALUE=TRIM(FUSRID(1))
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
      <table width="100%" border=1 cellPadding=0 cellSpacing=0 ID="Table2">
       <tr><td width="10%" class=dataListHead>社區序號</td>
           <td width="14%"  bgcolor="silver">
           <input type="text" name="key0"
                 <%=fieldRole(1)%> readonly size="5" value="<%=dspKey(0)%>" maxlength="5" class=dataListdata ID="Text3"></td>                 
          <td width="10%" class=dataListHead>主線序號</td>
           <td width="14%"  bgcolor="silver">
           <input type="text" name="key1"
                 <%=fieldRole(1)%> readonly size="5" value="<%=dspKey(1)%>" maxlength="5" class=dataListdata ID="Text3"></td>                 
          
           <td width="10%" class=dataListHead>客服單號</td>
           <td width="7%"  bgcolor="silver">
           <input type="text" name="key2"
                 <%=fieldRole(1)%> readonly size="13" value="<%=dspKey(2)%>" maxlength="13" class=dataListdata ID="Text3"></td>                 

           <td width="11%" class=dataListHead>派工單</td>
           <td width="11%"  bgcolor="silver">
           <input type="text" name="key3"
                 <%=fieldRole(1)%> readonly size="12" value="<%=dspKey(3)%>" maxlength="12" class=dataListdata ID="Text3"></td>                 
          <td width="7%" class=dataListHead>項次</td>
           <td width="7%"  bgcolor="silver">
           <input type="text" name="key4"
                 <%=fieldRole(1)%> readonly size="3" value="<%=dspKey(4)%>" maxlength="3" class=dataListdata ID="Text24"></td>
  </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(14))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(14)=V(0)
        End if  
       dspkey(15)=datevalue(now())
    else
        if len(trim(dspkey(16))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(16)=V(0)
        End if         
        dspkey(17)=datevalue(now())
    end if      
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
    '當竣工單已執行庫存計算後，設備資料即不允許異動
    set connyy=server.CreateObject("ADODB.Connection")
    set rsyy=server.CreateObject("ADODB.recordset")
    connyy.Open dsn     
    sqlyy="SELECT * FROM RTlessorCmtyLineFaqSNDWORK WHERE COMQ1=" & DSPKEY(0) & " AND LINEQ1=" & DSPKEY(1) & " AND FAQNO='" & DSPKEY(2) & "' AND PRTNO='" & DSPKEY(3) & "' "
    RSYY.Open SQLYY,CONNYY
    ERRSTOCK=""
    IF LEN(TRIM(RSYY("DROPDAT"))) > 0 OR LEN(TRIM(RSYY("CLOSEDAT"))) > 0 OR LEN(TRIM(RSYY("UNCLOSEDAT"))) > 0 then
       ERRSTOCK="Y"
    ELSE
       ERRSTOCK=""
    end if
    IF LEN(TRIM(RSYY("DROPDAT"))) > 0 OR LEN(TRIM(RSYY("UNCLOSEDAT"))) > 0 THEN
       ERRSTOCKX="Y"
       fieldPB=" class=""dataListData"" readonly "
    ELSE
       ERRSTOCKX=""
       fieldPB=""
    END IF
    RSYY.Close
    CONNYY.Close
    SET RSYY=NOTHING
    SET CONNYY=NOTHING
    If ERRSTOCK="Y" Then
       fieldPa=" class=""dataListData"" readonly "
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
  <span id="tags1" class="dataListTagsOn">ET-City主線設備資料</span>
                                                            
  <div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td><td>        
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
<tr>
           <td width="15%" class=dataListSEARCH>設備網路編號</td>
           <td width="30%" bgcolor=silver colspan=3>
           <input  type="text" name="key22"
                  size="3" value="<%=dspKey(22)%>" <%=fieldpB%><%=dataProtect%> class=dataListEntry maxlength="3" ID="Text25">          
           </TD>
</TR>
<tr>
           <td width="15%" class=dataListSEARCH>上層設備網路編號</td>
           <td width="30%" bgcolor=silver>
<%  set conn=server.CreateObject("ADODB.Connection")
    set rs=server.CreateObject("ADODB.recordset")
    conn.Open dsn
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND ERRSTOCKX="" Then 
       sql="SELECT HOSTNO FROM RTLessorCmtyLineHardware WHERE (DROPDAT IS NULL) AND (HOSTNO <> '') AND " _
          &"HOSTNO<>'" & DSPKEY(22) & "' " _
          &"UNION " _
          &"SELECT HOSTNO FROM RTLessorCmtyLineFAQHardware WHERE (DROPDAT IS NULL) AND (HOSTNO <> '') AND " _
          &"HOSTNO<>'" & DSPKEY(22) & "' and seq<>" & dspkey(4) 
       If len(trim(dspkey(23))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
    Else
       sql="SELECT HOSTNO FROM RTLessorCmtyLineHardware WHERE (DROPDAT IS NULL) AND (HOSTNO <> '') AND " _
          &"HOSTNO='" & DSPKEY(23) & "' "  _
          &"UNION " _
          &"SELECT HOSTNO FROM RTLessorCmtyLineFAQHardware WHERE (DROPDAT IS NULL) AND (HOSTNO <> '') AND " _
          &"HOSTNO='" & DSPKEY(23) & "' "  
    End If
   ' RESPONSE.WRITE SQL
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("HOSTNO")=dspkey(23) Then sx=" selected "
       s=s &"<option value=""" &rs("HOSTNO") &"""" &sx &">" &rs("HOSTNO") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
    %>           
    <select  name="key23" <%=fieldpB%> <%=dataProtect%> class=dataListEntry size="1" 
            style="text-align:left;" maxlength="8" ID="Select7"><br><%=s%></select>
            <BR><font size=2 color=red>空白時表示為最上層的設備，一條主線通常只能有一台</font>    </TD>
           <td width="15%" class=dataListSEARCH>上層設備網路PORT號</td>
           <td width="30%" bgcolor=silver>
           <input  type="text" name="key24"
                  size="3" value="<%=dspKey(24)%>" <%=fieldpB%><%=dataProtect%> class=dataListEntry maxlength="3" ID="Text25">          
           <BR><font size=2 color=red>LAN 請用P1、P2...，WAN 請用W1、W2...</font>
           </TD>
</TR>
<tr>
           <td width="15%" class=dataListHead>設備/規格</td>
           <td width="30%" bgcolor=silver colspan=3>
<%  set conn=server.CreateObject("ADODB.Connection")
    set rs=server.CreateObject("ADODB.recordset")
    conn.Open dsn
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND ERRSTOCK="" Then 
       sql="SELECT * FROM RTprodh ORDER BY prodno "
       If len(trim(dspkey(5))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
       SXX6=" onclick=""Srprod6onclick()""  "             
    Else
       sql="SELECT * FROM RTprodh WHERE prodno='" &dspkey(5) &"' order by prodno"
       SXX6=""
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("prodno")=dspkey(5) Then sx=" selected "
       s=s &"<option value=""" &rs("prodno") &"""" &sx &">" &rs("prodNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
    %>           
    <select  name="key5" <%=fieldpa%> <%=dataProtect%> class=dataListEntry size="1" 
            style="text-align:left;" maxlength="8" ID="Select7"><br><%=s%></select>

    <input  type="text" name="key6"
                 readonly size="5" value="<%=dspKey(6)%>" <%=fieldpa%><%=dataProtect%> class=dataListEntry maxlength="5" ID="Text25">          
    <input type="button" id="B6"  name="B6" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=SXX6%> >
              
    </td>    
</tr>
<td  class=dataListHead>設備/規格名稱</td></td>
    <td  bgcolor="silver" COLSPAN=3>
  <%
  DEVICENAME=""
  IF LEN(DSPKEY(5)) > 0 AND LEN(DSPKEY(6)) > 0 THEN
     Sql="SELECT itemno,itemnc,spec FROM RTprodd1 where prodno='" & dspkey(5) & "' and itemno='" & dspkey(6) & "' "
     rs.Open sql,conn
    if rs.EOF then 
       DEVICEname = ""
    else
       DEVICEname = rs("spec") & "--" & rs("itemnc")
    END IF
    RS.CLOSE
  END IF
  %>
        <input type="text"  READONLY name="DEVICENAME" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="80"
               value="<%=DEVICENAME%>" size="80" class=dataListDATA ID="Text5">
<tr><td width=15% class=dataListHead>數量</td>
    <td width=35% bgcolor="silver" >
        <input type="text" name="key7" size="8" value="<%=dspkey(7)%>" maxlength="10" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text4">
     <%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 AND ERRSTOCK=""  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='B5' " 
       If len(trim(dspkey(12))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='B5' AND CODE='" & dspkey(12) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(12) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%><font size=2>單位</font>         
   <select size="1" name="key12" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select15">                                                                  
        <%=s%>
   </select>        </td>
<td width=15% class=dataListHead>金額</td>
<td width=35% bgcolor="silver" >
    <input type="text" name="key13" size="8" value="<%=dspkey(13)%>" maxlength="10" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text4">   
</td></tr>
<tr>
<td class=dataListHEAD>出庫別</td>
    <td bgcolor="silver" colspan=3>
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND ERRSTOCK="" Then 
       sql="SELECT * FROM HBWAREHOUSE ORDER BY WAREHOUSE "
       If len(trim(dspkey(10))) < 1  Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
    Else
       sql="SELECT * FROM HBWAREHOUSE WHERE WAREHOUSE='" &dspkey(10) &"' "
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("WAREHOUSE")=dspkey(10) Then sx=" selected "
       s=s &"<option value=""" &rs("WAREHOUSE") &"""" &sx &">" &rs("WARENAME") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
   <select size="1" name="key10"<%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select4">
   <%=s%>
   </select>
</td>      
</tr>  
<tr>
        <td  class="dataListHEAD" height="23">作廢日期</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key8" size="10" READONLY value="<%=dspKey(8)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
        <td  class="dataListHEAD" height="23">作廢人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(11) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(11) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>    <input type="text" name="key11" size="6" READONLY value="<%=dspKey(11)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>                                            
 </tr>        
 <tr>
   <td class=dataListHEAD>作廢原因</td>          
   <td  height="23" bgcolor="silver" COLSPAN=3>
   <input type="text" name="key9" size="80" MAXLENGTH=80 value="<%=dspKey(9)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text16">
   </TD>

</tr>  
   <TR>
           <td   class="dataListHEAD" height="23">應收帳款編號</td>               
        <td   height="23" bgcolor="silver" >     
        <input type="text" name="key19" size="15" READONLY value="<%=dspKey(19)%>" <%=fieldRole(1)%><%=fieldpa%><%=dataProtect%> class="dataListDATA" maxlength="30">
        </TD>     
         <td    class="dataListHEAD" height="23">轉應收帳款日</td>               
        <td   height="23" bgcolor="silver" >     
        <input type="text" name="key20" size="10" value="<%=dspKey(20)%>" <%=fieldRole(1)%><%=fieldpa%><%=dataProtect%> class="dataListDATA" maxlength="10">
        </TD>     
    
   </TR>                 
   <TR>
           <td   class="dataListHEAD" height="23">轉應收帳款人員</td>               
        <td   height="23" bgcolor="silver" COLSPAN=3>     
        <%  name="" 
           if dspkey(21) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(21) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>            
        <input type="text" name="key21" size="6" READONLY value="<%=dspKey(21)%>" <%=fieldRole(1)%><%=fieldpa%><%=dataProtect%> class="dataListDATA" maxlength="30"><font size=2><%=name%></font>
        </TD>     
   </TR>                    
<tr>
        <td  class="dataListHEAD" height="23">建檔人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(14) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(14) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>    <input type="text" name="key14" size="6" READONLY value="<%=dspKey(14)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">建檔日期</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key15" size="10" READONLY value="<%=dspKey(15)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>  
<tr>
        <td  class="dataListHEAD" height="23">修改人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(16) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(16) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>    <input type="text" name="key16" size="6" READONLY value="<%=dspKey(16)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">修改日期</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key17" size="10" READONLY value="<%=dspKey(17)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>        
 <tr>
   <td class=dataListHEAD>備註</td>          
   <td  height="23" bgcolor="silver" COLSPAN=3>
   <input type="text" name="key18" size="30" MAXLENGTH=30 value="<%=dspKey(18)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListEntry" ID="Text16">
   </TD>
</tr>         
   </table> 
  </div> 
<% 
End Sub 
' --------------------------------------------------------------------------------------------  
%>
<!-- #include virtual="/Webap/include/RTGetUserRight.asp" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->