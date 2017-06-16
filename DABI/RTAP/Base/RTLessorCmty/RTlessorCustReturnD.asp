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
                   case ucase("/webap/rtap/base/rtlessorcmty/RTLessorCustReturnd.asp")
                    'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                       if i <> 1 then rs.Fields(i).Value=dspKey(i)    
                       if i=1 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         rsc.open "select max(entryno) AS entryno from RTLessorCustReturn where  CUSID='" & DSPKEY(0) & "' " ,conn
                         if len(trim(rsc("entryno"))) > 0 then 
                            dspkey(1)=rsc("entryno") + 1
                         else
                            dspkey(1)=1
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
                 case ucase("/webap/rtap/base/rtlessorcmty/RTLessorCustReturnd.asp")
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
       if ucase(runpgm)=ucase("/webap/rtap/base/rtlessorcmty/RTLessorCustReturnD.asp") then
          rsc.open "select max(entryno) AS ENTRYNO from RTLessorCustReturn where  CUSID='" & DSPKEY(0) & "' " ,conn
          if not rsC.eof then
            dspkey(1)=rsC("enTryno")
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
  title="ET-City用戶復機資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT CUSID,ENTRYNO,APPLYDAT,PAYCYCLE,PERIOD,SECONDCASE,AMT,PAYTYPE,CREDITCARDTYPE," _
             &"CREDITBANK,CREDITCARDNO,CREDITNAME,CREDITDUEM,CREDITDUEY,TARDAT,BATCHNO,TUSR,FINISHDAT," _
             &"CANCELDAT, CANCELUSR,MEMO,EDAT,EUSR,UDAT,UUSR,ADJUSTDAY,STRBILLINGDAT,RETURNMONEY,CASEKIND, RCVMONEYDAT " _
             &"FROM  RTLessorCustReturn WHERE cusid='' "
  sqlList="SELECT CUSID,ENTRYNO,APPLYDAT,PAYCYCLE,PERIOD,SECONDCASE,AMT,PAYTYPE,CREDITCARDTYPE," _
             &"CREDITBANK,CREDITCARDNO,CREDITNAME,CREDITDUEM,CREDITDUEY,TARDAT,BATCHNO,TUSR,FINISHDAT," _
             &"CANCELDAT, CANCELUSR,MEMO,EDAT,EUSR,UDAT,UUSR,ADJUSTDAY,STRBILLINGDAT,RETURNMONEY,CASEKIND, RCVMONEYDAT " _
             &"from RTLessorCustReturn WHERE "
  userDefineRead="Yes"      
  userDefineSave="Yes"       
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=4
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
  '第二戶(含)以上
  IF len(trim(dspkey(1)))=0 then dspkey(1)=0
  IF len(trim(dspkey(5)))=0 then dspkey(5)="N"
  IF len(trim(dspkey(25)))=0 then dspkey(25)=0
  IF len(trim(dspkey(27)))=0 then dspkey(27)=0
  if len(trim(dspkey(2)))=0 then
       formValid=False
       message="復機申請日不可空白"   
  elseif len(trim(dspkey(28)))=0  then
       formValid=False
       message="方案類型不可空白"                
  elseif len(trim(dspkey(3)))=0  then
       formValid=False
       message="繳費週期不可空白"                
  elseif len(trim(dspkey(7)))=0  then
       formValid=False
       message="繳費方式不可空白"        
  '信用卡繳費時，信用卡種類及發卡銀行、號碼不可空白      
  elseif len(trim(dspkey(8)))=0 AND dspkey(7)="01" then
       formValid=False
       message="信用卡種類不可空白"            
'  elseif len(trim(dspkey(9)))=0 AND dspkey(7)="01" then
'       formValid=False
'       message="信用卡發卡銀行不可空白"         
  elseif len(trim(dspkey(10)))=0 AND dspkey(7)="01" then
       formValid=False
       message="信用卡卡號不可空白"            
  elseif len(trim(dspkey(11)))=0 AND dspkey(7)="01" then
       formValid=False
       message="信用卡持卡人不可空白"            
  elseif (len(trim(dspkey(12)))=0 OR len(trim(dspkey(13)))=0 ) AND dspkey(7)="01" then
       formValid=False
       message="信用卡有效年月不可空白"                   
  elseif len(trim(dspkey(12)))<>0 and right("00" & dspkey(12),2)<="01" and right("00" & dspkey(12),2)>="12" then
       formValid=False
       message="信用卡有效月超出範圍(01-12)"       
  elseif len(trim(dspkey(13)))<>0 and dspkey(13)< right(datepart("yyyy",now()),2) then
       formValid=False
       message="信用卡已過期(小於系統年)"    
  elseif len(trim(dspkey(13)))<>0 and dspkey(13)< right(datepart("yyyy",now()),2) and len(trim(dspkey(12)))<>0 and dspkey(12) < right("00" & datepart("m",now()),2) then
       formValid=False
       message="信用卡已過期(小於系統月)"                                    
  elseif len(trim(dspkey(4)))=0 then
       formValid=False
       message="可使用[期數]不可空白"             
  elseif dspkey(3)="01" and ( dspkey(4)< 4 or dspkey(4)>8 )  then
       formValid=False
       message="期數範圍有異常，半年繳用戶範圍應介於4-8個月"            
  elseif dspkey(3)="02" and ( dspkey(4)< 12 or dspkey(4)>16 )  then
       formValid=False
       message="期數範圍有異常，年繳用戶範圍應介於12-16個月"       
  elseif dspkey(3)="03" and ( dspkey(4)< 24 or dspkey(4)>30 )  then
       formValid=False
       message="期數範圍有異常，二年繳用戶範圍應介於24-30個月"       
  elseif len(trim(dspkey(26)))=0  then
       formValid=False
       message="開始計費日不可空白"                                                                   
  end if
  '檢查客戶主檔狀態是否允許建立復機資料︰(1)必須有退租日 (2)復機開始計費日不可小於主檔退租日
 IF formValid=TRUE THEN
   Set connxx=Server.CreateObject("ADODB.Connection")
   Set rsxx=Server.CreateObject("ADODB.Recordset")
   connxx.open DSN
   sqlxx="select dropdat from rtlessorcust where cusid='" & aryparmkey(0) & "'"
   rsxx.Open sqlxx,connxx
   if not rsxx.eof then
      xxdate=rsxx("dropdat")
   end if
   rsxx.close
   connxx.Close   
   set rsxx=Nothing   
   set connxx=Nothing 

   if isnull(xxdate) or len(trim(xxdate))=0 then
      formValid=False
      message="客戶主檔無退租日，無法建立復機資料。"         
   elseif cdate(dspkey(26)) < cdate(xxdate) then
      formValid=False
      message="開始計費日不可小於客戶主檔退租日。"   
   end if
 end if
 '檢查是否已存在復機資料(尚未完工或作廢)，若存在則不允許重複建檔。
 IF formValid=TRUE AND accessmode="A" THEN
   Set connxx=Server.CreateObject("ADODB.Connection")
   Set rsxx=Server.CreateObject("ADODB.Recordset")
   connxx.open DSN
   sqlxx="select count(*) as cnt from rtlessorcustReturn where cusid='" & aryparmkey(0) & "' and finishdat is null and canceldat is null "
   rsxx.Open sqlxx,connxx
   if rsxx("cnt") > 0 then
      formValid=False
      message="客戶復機檔尚未復機資料未完工，不可重複建立復機資料。"         
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
                DSpkey(24)=V(0)
        dspkey(23)=now()
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
   Sub srChangeMoney()
      '方案類型
       s28=document.all("key28").value
       '繳費週期
       s3=document.all("key3").value
       '第二戶
       s5X=document.all("key5X").CHECKED
      ' s5Y=document.all("key5Y").value
 
     IF S28="01" THEN
       select case s5X
          case false
               '半年繳
               IF S3="01" THEN
                  document.All("key6").value = 2700
				  document.All("key4").value = 8
               '年繳
               ELSEIF S3="02" THEN
                  document.all("key6").value = 4800
				  document.All("key4").value = 16
               ELSEIF S3="03" THEN
                  document.all("key6").value = 9500
				  document.All("key4").value = 28
               ELSEIF S3="04" THEN
                  document.all("key6").value = 14000
				  document.All("key4").value = 36
               '季繳
               ELSEIF S3="05" THEN
                  document.all("key6").value = 1410
				  document.All("key4").value = 4
               ELSE
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               END IF
          CASE ELSE
               '半年繳
               IF S3="01" THEN
                  document.all("key6").value = 600
				  document.All("key4").value = 8
               '年繳
               ELSEIF S3="02" THEN
                  document.all("key6").value = 1200
				  document.All("key4").value = 16
               ELSEIF S3="03" THEN
                  document.all("key6").value = 2400
				  document.All("key4").value = 28
               ELSEIF S3="04" THEN
                  document.all("key6").value = 3600
				  document.All("key4").value = 36
               '季繳
               ELSEIF S3="05" THEN
                  document.all("key6").value = 300
				  document.All("key4").value = 4
               ELSE
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               END IF
       END SELECT
     ELSEIF S28="02" THEN
        select case s5X
          case false
               '半年繳
               IF S3="01" THEN
                  document.All("key6").value = 2700
				  document.All("key4").value = 6
               '年繳
               ELSEIF S3="02" THEN
                  document.all("key6").value = 4800
				  document.All("key4").value = 12
               '季繳
               ELSEIF S3="05" THEN
                  document.all("key6").value = 1410
				  document.All("key4").value = 3
               ELSE
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               END IF
          CASE ELSE
               '半年繳
               IF S3="01" THEN
                  document.all("key6").value = 600
				  document.All("key4").value = 6
               '年繳
               ELSEIF S3="02" THEN
                  document.all("key6").value = 1200
				  document.All("key4").value = 12
               '季繳
               ELSEIF S3="05" THEN
                  document.all("key6").value = 300
				  document.All("key4").value = 3
               ELSE
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               END IF
       END SELECT
     ELSEIF S28="03" THEN
        select case s5X
          case false
               '半年繳
               IF S3="01" THEN
                  document.All("key6").value = 1500
				  document.All("key4").value = 6
               '年繳
               ELSEIF S3="02" THEN
                  document.all("key6").value = 3000
				  document.All("key4").value = 12
               '季繳
               ELSEIF S3="05" THEN
                  document.all("key6").value = 750
				  document.All("key4").value = 3
               ELSE
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               END IF
          CASE ELSE
               '半年繳
               IF S3="01" THEN
                  document.all("key6").value = 600
				  document.All("key4").value = 6
               '年繳
               ELSEIF S3="02" THEN
                  document.all("key6").value = 1200
				  document.All("key4").value = 12
               '季繳
               ELSEIF S3="05" THEN
                  document.all("key6").value = 300
				  document.All("key4").value = 3
               ELSE
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               END IF
       END SELECT
     ELSEIF S28="05" THEN
        select case s5X
          case false
               '半年繳
               IF S3="01" THEN
                  document.All("key6").value = 2700
				  document.All("key4").value = 8
               '年繳
               ELSEIF S3="02" THEN
                  document.all("key6").value = 4800
				  document.All("key4").value = 16
               '季繳
               ELSEIF S3="05" THEN
                  document.all("key6").value = 1410
				  document.All("key4").value = 4
               ELSE
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               END IF
          CASE ELSE
               '半年繳
               IF S3="01" THEN
                  document.all("key6").value = 600
				  document.All("key4").value = 8
               '年繳
               ELSEIF S3="02" THEN
                  document.all("key6").value = 1200
				  document.All("key4").value = 16
               '季繳
               ELSEIF S3="05" THEN
                  document.all("key6").value = 300
				  document.All("key4").value = 4
               ELSE
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               END IF
       END SELECT

     ELSEIF S28="08" THEN
        select case s5X
          case false
               '半年繳
               IF S3="01" THEN
                  document.All("key6").value = 1800
				  document.All("key4").value = 6
               '年繳
               ELSEIF S3="02" THEN
                  document.all("key6").value = 3600
				  document.All("key4").value = 12
               '季繳
               ELSEIF S3="05" THEN
                  document.all("key6").value = 900
				  document.All("key4").value = 3
               ELSE
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               END IF
          CASE ELSE
               '半年繳
               IF S3="01" THEN
                  document.all("key6").value = 600
				  document.All("key4").value = 6
               '年繳
               ELSEIF S3="02" THEN
                  document.all("key6").value = 1200
				  document.All("key4").value = 12
               '季繳
               ELSEIF S3="05" THEN
                  document.all("key6").value = 300
				  document.All("key4").value = 3
               ELSE
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               END IF
       END SELECT

     ELSEIF S28="09" THEN
        select case s5X
          case false
               '半年繳
               IF S3="01" THEN
                  document.All("key6").value = 2700
				  document.All("key4").value = 7
               '年繳
               ELSEIF S3="02" THEN
                  document.all("key6").value = 4800
				  document.All("key4").value = 14
               '季繳
               ELSEIF S3="05" THEN
                  document.all("key6").value = 1400
				  document.All("key4").value = 3
               ELSE
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               END IF
          CASE ELSE
               '半年繳
               IF S3="01" THEN
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               '年繳
               ELSEIF S3="02" THEN
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               '季繳
               ELSEIF S3="05" THEN
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               ELSE
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               END IF
       END SELECT

     ELSEIF S28="10" THEN
        select case s5X
          case false
               '半年繳
               IF S3="01" THEN
                  document.All("key6").value = 1800
				  document.All("key4").value = 6
               '年繳
               ELSEIF S3="02" THEN
                  document.all("key6").value = 3000
				  document.All("key4").value = 12
               '季繳
               ELSEIF S3="05" THEN
                  document.all("key6").value = 1000
				  document.All("key4").value = 3
               ELSE
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               END IF
          CASE ELSE
               '半年繳
               IF S3="01" THEN
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               '年繳
               ELSEIF S3="02" THEN
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               '季繳
               ELSEIF S3="05" THEN
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               ELSE
                  document.all("key6").value = 0
				  document.All("key4").value = 0
               END IF
       END SELECT

     END IF
   END SUB
   SUB SrSHOWcreditcardOnClick()
       s7=document.all("key7").value
       IF s7="01" THEN
          window.tab1.style.display=""
       ELSE
          window.tab1.style.display="none"
       end if
    END SUB    
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
           <td width="10%" class=dataListHead>項次</td>
           <td width="15%"  bgcolor="silver">
           <input type="text" name="key1"
                 <%=fieldRole(1)%> readonly size="3" value="<%=dspKey(1)%>" maxlength="3" class=dataListdata></td>                 
  </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(22))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(22)=V(0)
        End if  
       dspkey(21)=now()
    else
        if len(trim(dspkey(24))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(24)=V(0)
        End if         
        dspkey(23)=now()
    end if      
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t

    '用戶結案或作廢後,開始計費日資料 protect
    If len(trim(dspKey(17))) > 0 or len(trim(dspKey(18))) > 0 Then
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
  <span id="tags1" class="dataListTagsOn">ET-City用戶復機資訊</span>
                                                            
  <div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td>
<td>     
      <DIV ID="SRTAG2" onclick="srtag2" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
    <tr>    <td bgcolor="BDB76B" align="LEFT">復機資料內容</td></tr></table></DIV>
    <DIV ID=SRTAB2 >
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3">
<tr>
    <td width="10%" class=dataListHEAD>復機申請日</td>
    <td width="28%" bgcolor="silver" >
        <input type="text" name="key2" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(2)%>"  READONLY size="10" class=dataListEntry>
       <input  type="button" id="B2"  <%=fieldpb%> name="B2" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C2"  name="C2"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
      </td>        
    <td width="10%" class=dataListHEAD>第二戶(含)以上</td>
    <td width="23%" bgcolor="silver" >
<%  dim FREECODE1,FREECODE2
    If Len(Trim(fieldRole(1) &dataProtect)) < 1 and flg = "Y" and len(trim(dspkey(18)))=0 Then
       FREECODE1=""
       FREECODE2=""
    Else
      ' sexd1=" disabled "
      ' sexd2=" disabled "
    End If
    If dspKey(5)="Y" Then FREECODE1=" checked "    
    If dspKey(5)="N" Then FREECODE2=" checked " %>                          
        <input type="radio" ID="KEY5X" value="Y" <%=FREECODE1%> name="key5" <%=fieldRole(1)%><%=dataProtect%> ID="Radio1" onCLICK="SrchangeMONEY">是
        <input type="radio" ID="KEY5Y" name="key5" value="N" <%=FREECODE2%><%=fieldRole(1)%><%=dataProtect%> ID="Radio2" onCLICK="SrchangeMONEY">否</td>               
         <td  WIDTH="10%"  class="dataListSEARCH" height="23">應收金額</td>               
        <td  WIDTH="23%" height="23" bgcolor="silver" >   
             <input type="text" name="key6"  size="8" READONLY value="<%=dspKey(6)%>"  <%=fieldRole(1)%> class="dataListDATA" ID="Text56">
        </TD>      
      </TR>
     <tr>
  <td  WIDTH="10%"  class="dataListHEAD" height="23">方案類型</td>               
        <td  WIDTH="23%" height="23" bgcolor="silver"  COLSPAN="3">
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 and len(trim(dspkey(18)))=0 Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='O9' and parm1 like '%ET%' "
       If len(trim(dspkey(28))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='O9' AND CODE='" & dspkey(28) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(28) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key28" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35" onchange="SrchangeMONEY">                                                                  
        <%=s%>
   </select>
        </td> 
         <td  WIDTH="10%"  class="dataListSEARCH" height="23">復機費</td>               
        <td  WIDTH="23%" height="23" bgcolor="silver" >   
        <input type="text" name="key27"  size="8" value="<%=dspKey(27)%>" <%=fieldpa%> <%=fieldRole(1)%> class="dataListENTRY" ID="Text1">     
        </TD>
	</TR>
     <tr>
  <td  WIDTH="10%"  class="dataListHEAD" height="23">繳費週期</td>               
        <td  WIDTH="23%" height="23" bgcolor="silver" >
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 and len(trim(dspkey(18)))=0 Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M8' and code in ('01','02','05') " 
       If len(trim(dspkey(3))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M8' AND CODE='" & dspkey(3) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(3) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key3" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35" onchange="SrchangeMONEY">                                                                  
        <%=s%>
   </select>
        </td>        
  <td  WIDTH="10%"  class="dataListHEAD" height="23">繳費方式</td>               
        <td  WIDTH="23%" height="23" bgcolor="silver" >
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 and len(trim(dspkey(18)))=0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M9' " 
       If len(trim(dspkey(7))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M9' AND CODE='" & dspkey(7) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(7) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key7" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35" onchange="SrSHOWcreditcardOnClick">                                                                  
        <%=s%>
   </select>
        </td>   
         <td  WIDTH="10%"  class="dataListSEARCH" height="23">收款日期</td>               
         <td  WIDTH="23%" height="23" bgcolor="silver" >            
			<input type="text" name="key29" <%=fieldpa%><%=fieldRole(1)%><%=dataProtect%>
               value="<%=dspKey(29)%>"  READONLY size="11" class=dataListEntry ID="Text3">
			<input  type="button" id="B29" name="B29" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
			<IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpb%> alt="清除" id="C29" name="C29" style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
        </TD>
  </tr>          
    <tr>
    <td colspan=6>
    <%
    if dspkey(7)<> "01" then 
       show=" style=""display:none"" "
    else
       show=""
    end if
    %>    
    <table border="1" width="100%" cellpadding="0" cellspacing="0" id="tab1" <%=show%>><tr>
     <td  WIDTH="10%" class="dataListHEAD" height="23">信用卡種類</td>               
        <td  WIDTH="28%" height="23" bgcolor="silver" >   
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  and len(trim(dspkey(18)))=0  Then  
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M6' " 
       If len(trim(dspkey(8))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='M6' AND CODE='" & dspkey(8) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(8) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key8" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35">                                                                  
        <%=s%>
   </select>        
        </td>
   <td  WIDTH="10%"  class="dataListHEAD" height="23">發卡銀行</td>               
        <td WIDTH="23%"  height="23" bgcolor="silver" >          
        <%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1  and len(trim(dspkey(18)))=0  Then  
       sql="SELECT * FROM RTBANK WHERE CREDITCARD='Y' ORDER BY HEADNC " 
       If len(trim(dspkey(9))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT * FROM RTBANK WHERE HEADNO='" & dspkey(9) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("HEADNO")=dspkey(9) Then sx=" selected "
       s=s &"<option value=""" &rs("HEADNO") &"""" &sx &">" &rs("HEADNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key9" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Select35">                                                                  
        <%=s%>
   </select>        
        </TD> 
        <td WIDTH="10%"   class="dataListHEAD" height="23">卡號</td>               
        <td WIDTH="23%" height="23" bgcolor="silver" >     
        <input type="text" name="key10" size="16" value="<%=dspKey(10)%>" <%=fieldRole(1)%><%=fieldpa%><%=dataProtect%> class="dataListEntry" maxlength="16">
        </TD>     
        </tr>    
   <TR>
           <td   class="dataListHEAD" height="23">持卡人姓名</td>               
        <td   height="23" bgcolor="silver" >     
        <input type="text" name="key11" size="12" value="<%=dspKey(11)%>" <%=fieldRole(1)%><%=fieldpa%><%=dataProtect%> class="dataListEntry" maxlength="30">
        </TD>     
         <td    class="dataListHEAD" height="23">信用卡有效期限</td>               
        <td   height="23" bgcolor="silver" COLSPAN=3>     
        <input type="text" name="key12" size="2" value="<%=dspKey(12)%>" <%=fieldRole(1)%><%=fieldpa%><%=dataProtect%> class="dataListEntry" maxlength="2">
        <FONT SIZE=2>月／</FONT>
        <input type="text" name="key13" size="2" value="<%=dspKey(13)%>" <%=fieldRole(1)%><%=fieldpa%><%=dataProtect%> class="dataListEntry" maxlength="2">
        <FONT SIZE=2>年(西元年後二碼，如2005則輸入05)</FONT>
        </TD>     
        </tr>
        </table>
        </td>
   </TR>                 
<tr>
        <td  class="dataListHEAD" height="23">建檔人員</td>                                 
        <td  height="23" bgcolor="silver">
	    <input type="text" name="key22" size="6" READONLY value="<%=dspKey(22)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2">
		<font size=2><%=SrGetEmployeeName(dspKey(22))%></font>
        </td>  
        <td  class="dataListHEAD" height="23">建檔日期</td>                                 
        <td  height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key21" size="25" READONLY value="<%=dspKey(21)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>  
<tr>
        <td  class="dataListHEAD" height="23">修改人員</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" name="key24" size="6" READONLY value="<%=dspKey(24)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text2">
		<font size=2><%=SrGetEmployeeName(dspKey(24))%></font>
        </td>  
        <td  class="dataListHEAD" height="23">修改日期</td>                                 
        <td  height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key23" size="25" READONLY value="<%=dspKey(23)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListDATA" ID="Text9">
        </td>       
 </tr>                 
  </table>     
  </DIV>
   <DIV ID="SRTAG4" onclick="srtag4" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr><td bgcolor="BDB76B" align="LEFT">用戶申請、異動及施工進度狀態</td></tr></table></DIV>
    <DIV ID=SRTAB4 >  
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
        <tr>
        <td  WIDTH="10%" class="dataListHEAD" height="23">開始計費日</td>                                 
        <td  WIDTH="28%" height="23" bgcolor="silver" >
        <%
        '初始值系統日期，因為主約必須是已退租的用戶才可用復機作業，但退租日往往與系統日有段差距，故不能以退租日+1天來預設。
        if len(trim(dspkey(26)))=0 then
           dspkey(26)=datevalue(now())
        end if
        %>
        <input type="text" name="key26" size="10" READONLY value="<%=dspKey(26)%>" <%=fieldpC%> <%=fieldRole(1)%> class="dataListentry" ID="Text56" >
         <input type="button" id="B26"  name="B26" height="100%" width="100%" <%=fieldpD%>style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C26"  name="C26"   <%=fieldpD%>style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">     
        <font size=2 color=red>預設為到期日加1天</font>
        </td>
        <td  WIDTH="10%" class="dataListHEAD" height="23">結案日期</td>                                 
        <td  WIDTH="23%" height="23" bgcolor="silver" >
        <input type="text" name="key17" size="25" READONLY value="<%=dspKey(17)%>"  <%=fieldRole(1)%> class="dataListdata" ID="Text57">
      
       <td  WIDTH="10%"  class="dataListHEAD" height="23">可使用期數</td>               
        <td  WIDTH="23%" height="23" bgcolor="silver" >   
        <input type="text" name="key4" size="3" value="<%=dspKey(4)%>" <%=fieldRole(1)%><%=fieldpa%><%=dataProtect%> class="dataListEntry" maxlength="3" onchange="srrecalulate()">
        <FONT SIZE=2>　調整日數</FONT>
        <input type="text" name="key25" size="3" value="<%=dspKey(25)%>" <%=fieldRole(1)%><%=fieldpa%><%=dataProtect%> class="dataListEntry" maxlength="3" onchange="srrecalulate()">
        </td>
</TR>
        <tr>
        <td  WIDTH="10%" class="dataListHEAD" height="23">應收帳款產生日</td>                                 
        <td  WIDTH="23%" height="23" bgcolor="silver" >
        <input type="text" name="key14" size="25" READONLY value="<%=dspKey(14)%>"  <%=fieldRole(1)%> class="dataListdata" ID="Text57">
 
        <td  WIDTH="10%" class="dataListHEAD" height="23">應收帳款編號</td>                                 
        <td  WIDTH="23%" height="23" bgcolor="silver" >
        <input type="text" name="key15" size="15" READONLY value="<%=dspKey(15)%>" <%=fieldpC%> <%=fieldRole(1)%> class="dataListdata" ID="Text56" >
        </td>
       <td  WIDTH="10%"  class="dataListHEAD" height="23">帳款產生人員</td>               
        <td  WIDTH="23%" height="23" bgcolor="silver" >   
        <input type="text" name="key16" size="6" value="<%=dspKey(16)%>" <%=fieldRole(1)%><%=fieldpa%><%=dataProtect%> class="dataListdata" maxlength="3" >
		<font size=2><%=SrGetEmployeeName(dspKey(16))%></font>
        </td>
</TR>
       <tr>
       <td   class="dataListHEAD" height="23">作廢日期</td>                                 
        <td   height="23" bgcolor="silver">
        <input type="text" name="key18" size="10" value="<%=dspKey(18)%>"  <%=fieldpa%><%=fieldRole(1)%> readonly class="dataListdata" ID="Text41">
         </td>
        <td    class="dataListHEAD" height="23">作廢人員</td>                                 
        <td    height="23" bgcolor="silver" COLSPAN=3>
        <input type="text" name="key19" size="7" value="<%=dspKey(19)%>" <%=fieldRole(1)%> readonly class="dataListDATA" ID="Text43">
		<font size=2><%=SrGetEmployeeName(dspKey(19))%></font>
        </td></tr>           


  </table> 
  </DIV>
    <DIV ID="SRTAG6" onclick="SRTAG6" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="LEFT">備註說明</td></tr></table></DIV>
   <DIV ID="SRTAB6" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
    <TR><TD align="CENTER">
     <TEXTAREA  cols="100%"  name="key20" rows=8  MAXLENGTH=500  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(20)%>" ID="Textarea1"><%=dspkey(20)%></TEXTAREA>
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
End Sub
' -------------------------------------------------------------------------------------------- 
' --------------------------------------------------------------------------------------------  
%>
<!-- #include virtual="/Webap/include/checkid.inc" -->
<!-- #include virtual="/Webap/include/companyid.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserRight.asp" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->