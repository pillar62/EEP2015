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
                   case ucase("/webap/rtap/base/rtLessorcmty/RTLessorCustReturnSNDWORKd.asp")
                       'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       if i <> 2 then rs.Fields(i).Value=dspKey(i)    
                       if i=2 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         cusidxx="ER" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
                         rsc.open "select max(PRTNO) AS PRTNO from RTLessorCustReturnSNDWORK where PRTNO like '" & cusidxx & "%' " ,conn
                         if len(rsc("PRTNO")) > 0 then
                            dspkey(2)=cusidxx & right("0000" & cstr(cint(right(rsc("PRTNO"),4)) + 1),4)
                         else
                            dspkey(2)=cusidxx & "0001"
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
                 case ucase("/webap/rtap/base/rtLESSORcmty/RTLessorCustReturnSNDWORKd.asp")
                 'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                    rs.Fields(i).Value=dspKey(i)
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
       if ucase(runpgm)=ucase("/webap/rtap/base/rtlessorcmty/RTLessorCustReturnSNDWORKd.asp") then
          cusidxx="ER" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
          rsc.open "select max(PRTNO) AS PRTNO from RTLessorCustReturnSNDWORK where PRTNO like '" & cusidxx & "%' " ,conn
          if not rsC.eof then
            dspkey(2)=rsC("PRTNO")
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
  numberOfKey=3
  title="ET-City用戶復機收款派工單資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT cusid,ENTRYNO, PRTNO, SENDWORKDAT, PRTUSR, ASSIGNENGINEER," _
             &"ASSIGNCONSIGNEE,REALENGINEER, REALCONSIGNEE,  " _
             &"DROPDAT, DROPDESC, CLOSEDAT, BONUSCLOSEYM, BONUSCLOSEDAT, BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, STOCKCLOSEDAT, " _
             &" STOCKCLOSEUSR, STOCKFINCHK, MDF1,MDF2,HOSTNO,HOSTPORT, MEMO,PRTDAT,UNCLOSEDAT,DROPUSR,CLOSEUSR,eusr,edat,uusr,udat " _
             &"FROM RTLessorCustReturnSNDWORK WHERE cusid='' "
  sqlList="SELECT cusid,ENTRYNO, PRTNO, SENDWORKDAT, PRTUSR, ASSIGNENGINEER, " _
             &"ASSIGNCONSIGNEE, REALENGINEER, REALCONSIGNEE,  " _
             &"DROPDAT, DROPDESC, CLOSEDAT, BONUSCLOSEYM, BONUSCLOSEDAT, BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, STOCKCLOSEDAT, " _
             &" STOCKCLOSEUSR, STOCKFINCHK, MDF1,MDF2,HOSTNO,HOSTPORT, MEMO,PRTDAT,UNCLOSEDAT,DROPUSR,CLOSEUSR,eusr,edat,uusr,udat " _
             &"FROM RTLessorCustReturnSNDWORK WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=1
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    '結案時檢查是否有設備明細==>當裝機完成時，須有設備才可完工計算業績(扣庫存用)
    Set connXX=Server.CreateObject("ADODB.Connection")
    Set rsXX=Server.CreateObject("ADODB.Recordset")
    connXX.open DSN
    SQLXX="SELECT * FROM RTLessorCustReturnHARDWARE WHERE cusid='" & dspkey(0) & "' and entryno=" & dspkey(1) & " and prtno='" & dspkey(2) & "' "
    RSXX.Open sqlXX,CONNXX
    errcode=""
    IF RSXX.EOF THEN
       ERRCODE="1"
    ELSE
       ERRcode=""
    END IF
    RSXX.Close
    SQLXX="SELECT * FROM RTlessorCUST WHERE cusid='" & dspkey(0) & "'"
    RSXX.OPEN SQLXX,CONNXX
    IF RSXX.EOF THEN
       formValid=False
       message="找不到客戶主檔" 
    ELSEIF LEN(TRIM(RSXX("ip11"))) = 0 or LEN(TRIM(RSXX("ip12"))) = 0 or LEN(TRIM(RSXX("ip13"))) = 0 or LEN(TRIM(RSXX("ip14"))) = 0  THEN
       formValid=False
       message="客戶尚未分配IP，不可建立派工資料。"       
    ELSE       
        
    if LEN(TRIM(dspkey(3))) = 0 then
       formValid=False
       message="派工日期不可空白" 
    ELSEif len(trim(DSPKEY(4))) = 0 and len(trim(DSPKEY(5))) = 0 THEN
       formValid=False
       message="預定裝機人員或經銷商必須輸入其中一項"        
    ELSEif len(trim(DSPKEY(5))) > 0 and len(trim(DSPKEY(6))) > 0 THEN
       formValid=False
       message="預定裝機人員及經銷商只能輸入其中一項"    
    ELSEif len(trim(DSPKEY(7))) = 0 and len(trim(DSPKEY(8))) = 0 AND LEN(TRIM(DSPKEY(11))) > 0 THEN
       formValid=False
       message="實際裝機人員或經銷商必須輸入其中一項" 
    ELSEif len(trim(DSPKEY(7))) > 0 and len(trim(DSPKEY(8))) > 0 AND LEN(TRIM(DSPKEY(11))) > 0 THEN
       formValid=False
       message="實際裝機人員及經銷商只能輸入其中一項"    
    elseif len(trim(DSPKEY(11))) > 0 and len(trim(DSPKEY(9))) = 0  AND ERRCODE="1" THEN
       formValid=False
       message="當裝機完成時則必須輸入用戶設備資料"    
    END IF       
    END IF
    RSXX.CLOSE
    CONNXX.Close
    SET RSXX=NOTHING
    SET CONNXX=NOTHING        
    '檢查此復機資料的付款方式是否為現金? 只有現金才可派工收款，其它如ATM或信用卡者直接產生應收帳款即可
    if formValid=True then
       Set connxx=Server.CreateObject("ADODB.Connection")
       Set rsxx=Server.CreateObject("ADODB.Recordset")
       connxx.open DSN
       sqlxx="select PAYTYPE from rtlessorcustReturn where cusid='" & aryparmkey(0) & "'"
       rsxx.Open sqlxx,connxx
           if rsxx("paytype") <> "02" then
              formValid=False
              message="此復機資料繳費方式非(現金)，不可建立復機收款派工單(請直接轉應收帳款即可)"    
           end if
       rsxx.close
       connxx.Close   
       set rsxx=Nothing   
       set connxx=Nothing 
    end if
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(31)=V(0)
        dspkey(32)=datevalue(now())
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

   Sub Srsales6onclick()
       prog="RTGetsalesD2.asp"
       prog=prog & "?KEY=" & document.all("AREAID").VALUE 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
          FUsrID=Split(Fusr,";")   
          if Fusrid(2) ="Y" then
             document.all("key5").value =  trim(Fusrid(0))
          End if       
       end if
   End Sub               
   Sub Srsales8onclick()
       prog="RTGetsalesD2.asp"
       prog=prog & "?KEY=" & document.all("AREAID").VALUE 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
          FUsrID=Split(Fusr,";")   
          if Fusrid(2) ="Y" then
             document.all("key7").value =  trim(Fusrid(0))
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
       <tr><td width="10%" class=dataListHead>用戶序號</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key0"
                 <%=fieldRole(1)%> readonly size="15" value="<%=dspKey(0)%>" maxlength="15" class=dataListdata ID="Text9"></td>
           <td width="10%" class=dataListHead>項次</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key1"
                 <%=fieldRole(1)%> readonly size="5" value="<%=dspKey(1)%>" maxlength="5" class=dataListdata ID="Text9"></td>
           <td width="10%" class=dataListHead>竣工單號</td>
           <td width="10%"  bgcolor="silver">
           <input type="text" name="key2"
                 <%=fieldRole(1)%> readonly size="12" value="<%=dspKey(2)%>" maxlength="12" class=dataListdata ID="Text24"></td>
  </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(29))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(29)=V(0)
        End if  
       dspkey(30)=datevalue(now())
    else
        if len(trim(dspkey(31))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(31)=V(0)
        End if         
        dspkey(32)=datevalue(now())
    end if          
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
    '獎金計算後，資料protect
    If len(trim(dspKey(12))) > 0 Then
       fieldPa=" class=""dataListData"" readonly "
       fieldPb=""
       FIELDPC=""
       FIELDPD=""
       FIELDP6=""
       FIELDP8=""
    Else
       fieldPa=""
       FIELDPC=" onclick=""SrBtnOnClick"" "
       FIELDPD=" onclick=""SrCLEAR"" "
       FIELDP6="  onclick=""Srsales6onclick()""  "
       FIELDP8="  onclick=""Srsales8onclick()""  "
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
  <span id="tags1" class="dataListTagsOn">竣工單資料</span>
                                                            
  <div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td><td>        
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
<tr><td width="14%" class=dataListHead>派工日期</td>
    <td width="40%" bgcolor="silver" COLSPAN=3>
        <input type="text" name="key3" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(3)%>" size="10" class=dataListEntry ID="Text1">
       <input  type="button" id="B3" name="B3" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=FIELDPC%>>
       <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpa%> alt="清除" id="C3"  name="C3"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" <%=FIELDPD%>>                              
    </td>
 <tr>
    <td width="14%" class=dataListHead>派工單列印日</td></td>
    <td width="40%" bgcolor="silver" >
        <input type="text" name="key25" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(25)%>" size="10" READONLY  class=dataListDATA ID="Text8">
     </td>    
    <td width="13%" class=dataListHead>最近列印人員</td></td>
<%
  name="" 
           if dspkey(4) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(4) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
          
 %>    
    <td width="35%" bgcolor="silver" >
        <input type="text" name="key4" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="6"
               value="<%=dspKey(4)%>" size="6" READONLY class=dataListDATA><font size=2><%=name%></font>
    </td></tr>

<tr><td class=dataListHEAD>預定裝機人員</td>
<td bgcolor="silver" >
<%
  sql="SELECT * FROM RTLessorCmtyLine RIGHT OUTER JOIN RTLessorCust ON RTLessorCmtyLine.COMQ1 = RTLessorCust.COMQ1 AND " _
     &"RTLessorCmtyLine.LINEQ1 = RTLessorCust.LINEQ1 WHERE cusid='" & dspkey(0) & "'"
  rs.Open sql,conn
  IF RS.EOF THEN
     DOMAIN=""
  ELSE
     DOMAIN=RS("AREAID")
     if RS("consignee") <>"" then consignee ="disabled"
  END IF
  RS.CLOSE
  
  name="" 
           if dspkey(5) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(5) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
          
 %>
        <INPUT type="HIDDEN" name="AREAID" value="<%=DOMAIN%>">
        <input type="TEXT" name="key5" <%=fieldRole(1)%><%=dataProtect%> 
               style="text-align:left;" size="6" maxlength="6" 
               value="<%=dspKey(5)%>"  readonly class="dataListENTRY" >
           <input type="BUTTON" id="B5" <%=FIELDPA%> name="B5"  width="100%" style="Z-INDEX: 1"  value="...." <%=FIELDP6%> <%=consignee%> >   
           <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPA%> alt="清除" id="C5"  name="C5" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  <%=FIELDPD%>>
           <font size=2><%=name%></font>
 </td>
<td class=dataListHEAD>實際裝機人員</td>
<td bgcolor="silver" >
<%
  name="" 
           if dspkey(7) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(7) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
          
 %>
        <input type="TEXT" name="key7" <%=fieldRole(1)%><%=dataProtect%> 
               style="text-align:left;" size="6" maxlength="6" 
               value="<%=dspKey(7)%>"  readonly class="dataListENTRY" ID="Text3">
           <input type="BUTTON" id="B7" name="B7"  width="100%" style="Z-INDEX: 1"  value="...."  <%=FIELDP8%>  >   
           <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=FIELDPA%> alt="清除" id="C7"  name="C7"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"   <%=FIELDPD%>>
           <font size=2><%=name%></font>
</td>
 </tr>  
<tr><td  class=dataListHead>預定裝機經銷商</td>
    <td  bgcolor="silver" >
        <%  If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  Then 
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID inner join rtconsigneeisp on rtobj.cusid=rtconsigneeisp.cusid " _
          &"WHERE (RTObjLink.CUSTYID = '02' and rtconsigneeisp.isp='04')  "
       s="<option value="""" >(經銷商)</option>"
    Else
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID  inner join rtconsigneeisp on rtobj.cusid=rtconsigneeisp.cusid " _
          &"WHERE (RTObjLink.CUSTYID = '02')  and rtobj.cusid='" & dspkey(6) & "' and rtconsigneeisp.isp='04' "
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(經銷商)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(6) Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>
           <select size="1" name="key6" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" ID="Select34">                                            
              <%=s%>
           </select>
    </td>
    <td  class=dataListHead>實際裝機經銷商</td>
    <td  bgcolor="silver" >
        <%  If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  Then 
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID inner join rtconsigneeisp on rtobj.cusid=rtconsigneeisp.cusid " _
          &"WHERE (RTObjLink.CUSTYID = '02' and rtconsigneeisp.isp='04')  "
       s="<option value="""" >(經銷商)</option>"
    Else
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID inner join rtconsigneeisp on rtobj.cusid=rtconsigneeisp.cusid " _
          &"WHERE (RTObjLink.CUSTYID = '02')  and rtobj.cusid='" & dspkey(8) & "' and rtconsigneeisp.isp='04' "
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(經銷商)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(8) Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>
           <select size="1" name="key8" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" ID="Select1">                                            
              <%=s%>
           </select>
    </td></tr>
<tr>
    <td  class=dataListHead>完工結案日</td>
    <td  bgcolor="silver" >
        <input type="text" name="key11" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(11)%>" size="10" READONLY class=dataListdata ID="Text13">
               <!--
       <input  type="button" id="B11" name="B11" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=FIELDPC%>>
       <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpa%> alt="清除" id="C11"  name="C11"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" <%=FIELDPD%>>                              
    -->
    </td>   
    <td  class=dataListHead>未完工結案日</td>
    <td  bgcolor="silver" >
        <input type="text" name="key26" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(26)%>" size="10" READONLY class=dataListdata ID="Text7">
               <!--
       <input  type="button" id="B26" name="B26" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=FIELDPC%>>
       <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpa%> alt="清除" id="C26"  name="C26"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" <%=FIELDPD%>>                              
    -->
    </td>       
  </tr>
  <tr>
      <td  class=dataListHead>結案人員</td>
    <%
  name="" 
           if dspkey(28) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(28) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
          
 %>    
    <td  bgcolor="silver"  colspan=3>
        <input type="text" name="key28" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(28)%>" size="10" READONLY class=dataListDATA ID="Text29">
               <font size=2><%=name%></font>
    </td>        
 </TR>
  <tr>
<td  class=dataListHead>作廢日期</td>
    <td  bgcolor="silver" >
        <input type="text" name="key9" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(9)%>" size="10" READONLY class=dataListDATA ID="Text27">
    </td>    
    <td  class=dataListHead>作廢人員</td>
    <%
  name="" 
           if dspkey(27) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(27) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
          
 %>    
    <td  bgcolor="silver" >
        <input type="text" name="key27" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(27)%>" size="10" READONLY class=dataListDATA ID="Text28">
               <font size=2><%=name%></font>
    </td>        
 </TR>

 <tr>
   <td class=dataListHEAD>未完工/作廢原因</td>          
   <td  height="23" bgcolor="silver" COLSPAN=3>
     <input type="text" name="key10" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="90"
               value="<%=dspKey(10)%>" size="90" class=dataListEntry ID="Text2">
   </TD>
   </TR>
 <tr> 
    <td  class=dataListHead>MDF位置</td>
    <td  bgcolor="silver" >
        <input type="text" name="key20" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(20)%>" size="10"  class=dataListENTRY ID="Text12">
        <font size=3>端子版位置</font>
        <input type="text" name="key21" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(21)%>" size="10"  class=dataListENTRY ID="Text4">               
    </td>    
    <td  class=dataListHead>SWITCH(PNA)編號</td>
    <td  bgcolor="silver" >
        <input type="text" name="key22" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(22)%>" size="10"  class=dataListENTRY ID="Text5">
        <font size=3>PORT號</font>
        <input type="text" name="key23" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(23)%>" size="10"  class=dataListENTRY ID="Text6">               
    </td>        
</tr>           
<tr>
 <td class=dataListHEAD>獎金計算月</td>
    <td bgcolor="silver">   
    <input type="text" name="key12" size="6" MAXLENGTH=6 value="<%=dspKey(12)%>"  READONLY  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text10">
    </td>                            
   <td class=dataListHead>獎金計算日</td>          
    <td bgcolor="silver">   
    <input type="text" name="key13" size="10" MAXLENGTH=10 value="<%=dspKey(13)%>"  READONLY  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text11">
    </td>  
  </TR>
  <tr>        
   <td class=dataListHead>獎金結帳員</td>          
    <td  bgcolor="silver">     
   <%   NAME=""
          if dspkey(14) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(14) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name="(對象檔找不到結帳人員)"
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if    %>
    <input type="text" name="key14" size="6" MAXLENGTH=6 value="<%=dspKey(14)%>"  READONLY  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text15"><font size=3><%=NAME%></font>
    </td>
   <td class=dataListHead>獎金會計審核日</td>          
    <td  bgcolor="silver">     
 <input type="text" name="key15" size="10" MAXLENGTH=10 value="<%=dspKey(15)%>"  READONLY  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text16">
    </td>    
</tr>             
<tr>
 <td class=dataListHEAD>庫存計算月</td>
    <td bgcolor="silver">   
    <input type="text" name="key16" size="6" MAXLENGTH=6 value="<%=dspKey(16)%>"  READONLY  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text14">
    </td>                            
   <td class=dataListHead>庫存計算日</td>          
    <td bgcolor="silver">   
    <input type="text" name="key17" size="10" MAXLENGTH=10 value="<%=dspKey(17)%>"  READONLY  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text21">
    </td>  
  </TR>
  <tr>        
   <td class=dataListHead>庫存結帳員</td>          
    <td  bgcolor="silver">     
   <%   NAME=""
          if dspkey(18) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(18) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name="(對象檔找不到庫存結帳人員)"
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if    %>
    <input type="text" name="key18" size="6" MAXLENGTH=6 value="<%=dspKey(18)%>"  READONLY  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text25"><font size=3><%=NAME%></font>
    </td>
   <td class=dataListHead>庫存會計審核日</td>          
    <td  bgcolor="silver">     
    <input type="text" name="key19" size="10" MAXLENGTH=10 value="<%=dspKey(19)%>"  READONLY  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text26">
    </td>    
</tr>  
<tr>
        <td  class="dataListHEAD" height="23">建檔人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(29) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(29) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>    <input type="text" name="key29" size="6" READONLY value="<%=dspKey(29)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">建檔日期</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key30" size="10" READONLY value="<%=dspKey(30)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>  
<tr>
        <td  class="dataListHEAD" height="23">修改人員</td>                                 
        <td  height="23" bgcolor="silver">
        <%  name="" 
           if dspkey(31) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(31) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>    <input type="text" name="key31" size="6" READONLY value="<%=dspKey(31)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">修改日期</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key32" size="10" READONLY value="<%=dspKey(32)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>        
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
    <tr><td bgcolor="BDB76B" align="CENTER">備註說明</td></tr>
    <TR><TD align="CENTER">
     <TEXTAREA  cols="100%" name="key24" rows=5  MAXLENGTH=300 <%=fieldpa%>  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(24)%>" ID="Textarea1"><%=dspkey(24)%></TEXTAREA>
   </td></tr>
 </table>            
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