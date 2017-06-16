 <%@ Transaction = required %>
 <% 
    SET conn=server.CreateObject("ADODB.Connection")
    set rs1=server.CreateObject("ADODB.recordset")
    DSN="DSN=RTLIB"
    conn.Open dsn
    sA=""
    sql="SELECT propertyid,propertynm FROM hbcontractTreeh  ORDER BY propertyid "
    rs1.Open sql,conn,1,1
    propertyid=""
    cnt=0
   ' CNTX=0
    set rsX=server.CreateObject("ADODB.recordset")
   ' set rsY=server.CreateObject("ADODB.recordset")
    Do While Not rs1.Eof
       sA=sA &"<option value=""" &rs1("PROPERTYID") &"""" &sx &">" &rs1("PROPERTYNM") &"</option>"
       sx=""
       if propertyid <> rs1("propertyid") then
          XX="aryproperty(" & CNT+1 & ") = ""<select class=dataListEntry name=""""key2"""">" 
          propertyid = rs1("propertyid")
          cntx=0
       end if
       SQLX="select CATEGORY1,CATEGORY1NM FROM HBCONTRACTTREEL1 WHERE PROPERTYID='" & RS1("PROPERTYID") & "' "
       RSX.OPEN SQLX,CONN,1,1
       Do while Not RSX.eof
             XX=XX & "<option VALUE="""""  & RSX("CATEGORY1") & """"">" & RSX("CATEGORY1NM") & "</OPTION>"    
           '  SQLY="select CATEGORY2,CATEGORY2NM FROM HBCONTRACTTREEL2 WHERE PROPERTYID='" & RS1("PROPERTYID") & "' AND CATEGORY1='" & RSX("CATEGORY1") & "' "
           '  RSY.OPEN SQLY,CONN,1,1
           '  if propertyid <> rs1("propertyid") or category1 <> rsX("category1") then
           '     YY="arypropertyY(" & CNT & "," & CNTX & ") = ""<select class=dataListEntry name=""""key3"""">" 
           '     if propertyid<>rs1("propertyid") then propertyid=rs1("propertyid")
           '     if category1 <> rsX("category1") then category1 = rsx("category1")   
           '     if TEMPCNTX < CNTX then TEMPCNTX=CNTX
           '  end if
           '  Do while Not RSY.eof
           '     YY=YY & "<option VALUE=""""" &  RSY("CATEGORY2") &  """"">" & RSY("CATEGORY2NM") & "</OPTION>"   
           '     RSY.movenext
           '  Loop
           '  RSY.CLOSE
           '  YYS=YYS & YY & "</SELECT>""" & vbCrLf    
             rsX.MoveNext
           '  CNTX=CNTX+1
           '   if TEMPCNTX < CNTX then TEMPCNTX=CNTX
       Loop
       XXS=XXS & XX & "</SELECT>""" & vbCrLf    
       rsx.close
       cnt=cnt+1
       rs1.MoveNext
    Loop
    xxs="Dim aryproperty(" & CNt & ") " &vbCrLf & xxs 
    'YYS="Dim arypropertyY(" & CNt-1 & "," & TEMPCNTX-1 & ") " &vbCrLf & YYS 
    rs1.Close
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
                 if i = 33 then
                    dspkey(33)=trim(dspkey(1)) & trim(dspkey(2)) & trim(dspkey(3)) & trim(dspkey(36))
                 end if
                 if i=34 or i = 35 then
                    '確認是否有冊號異動
                    '只有當I=34才執行==> 避免重覆二次後...VOLUME及PAGE多加一次
                    if I=34 THEN
                  '910913 變更程式==>不由chgflg控制跳冊,改由異動檔加入一筆(冊數+1,頁數為0)
                      SET connYY=server.CreateObject("ADODB.Connection")
                      set rsYY=server.CreateObject("ADODB.recordset")
                      DSN="DSN=RTLIB"
                      connYY.Open dsn
                      sqlYY="select CHGFLAG,volumepage,LASTCHGDAT from HBContractTreeL1 where propertyid='" & dspkey(1) & "' and category1='" & dspkey(2) & "' "
                      RSYY.Open SQLYY,CONNYY,3,3
                      IF not RSYY.EOF THEN
                         CHGFLG= rsyy("CHGFLAG")
                         volume=rsyy("volumepage")
                        '更新異動旗標為"N" (目前旗標控制取消...改由異動頁數時直接於"已使用冊號"檔之冊數+1,頁次歸0
                         RSYY("CHGFLAG")=""
                         RSYY("LASTCHGDAT")=NOW()
                         RSYY.Update
                      ELSE
                         CHGFLG=""
                         volume=0
                      END IF
                      RSYY.CLOSE
                      CONNYY.Close
                      SET RSYY=NOTHING
                      SET CONNYY=NOTHING
                    '讀取目前冊號最大頁數及最大冊數
                      sqlxx="select a.CTproperty,a.CTTree1,a. CTTree2, a.AREACODE, a.volume, a.pagecnt from HBcontractvolumeuse a, (SELECT CTproperty, CTTree1, CTTree2, AREACODE, MAX(VOLUME) AS volume " _
                           &"FROM HBContractVolumeUse " _
                           &"WHERE CTproperty = '" & dspkey(1) & "' AND CTTree1 = '" & dspkey(2) & "' AND CTTree2 = '" & dspkey(3) & "' and AREACODE = '" & dspkey(36) & "' " _
                           &"GROUP BY  CTproperty, CTTree1, CTTree2, AREACODE) b " _
                           &"where a.ctproperty=b.ctproperty and a.cttree1=b.cttree1 and a.cttree2=b.cttree2 and a.areacode=b.areacode and a.volume=b.volume "
                      SET connxx=server.CreateObject("ADODB.Connection")
                      set rsxx=server.CreateObject("ADODB.recordset")
                      DSN="DSN=RTLIB"
                      connxx.Open dsn
                      RSXX.Open sqlxx,connxx
                      if not rsxx.EOF then
                         IF TRIM(rsxx("volume")) > 0 THEN
                       '更新異動旗標為"N" (目前旗標控制取消...改由異動頁數時直接於"已使用冊號"檔之冊數+1,頁次歸0
                        ' IF CHGFLG="Y" THEN
                        '    tempvolume=RSXX("VOLUME") + 1
                        '    TEMPpagecnt=1
                        ' else
                            tempvolume=rsxx("volume")
                            temppagecnt=rsxx("pagecnt")+1
                            if temppagecnt > volume then
                               tempvolume=tempvolume + 1
                               temppagecnt=1
                            end if
                        ' end if
                         '找到現有資料=====>更新最大冊號及頁號
                            IF TEMPVOLUME > RSXX("VOLUME") THEN
                               RSXX.CLOSE
                               connxx.execute "insert into hbcontractvolumeuse (ctproperty,cttree1,cttree2,areacode,volume,pagecnt) values('"& dspkey(1) & "','" & dspkey(2) & "','" & dspkey(3) & "','" & dspkey(36) & "'," & tempvolume & "," & temppagecnt&") "
                          '  RSXX.Open sqlxx,connxx,3,3
                          '  RSXX.AddNew
                          '  RSXX("CTPROPERTY")=DSPKEY(1)
                          '  RSXX("CTTREE1")=DSPKEY(2)
                          '  RSXX("CTTREE2")=DSPKEY(3)
                          '  RSXX("AREACODE")=DSPKEY(36)
                          '  RSXX("VOLUME")=TEMPVOLUME
                          '  RSXX("PAGECNT")=TEMPPAGECNT
                          '  RSXX.UPDATE
                            ELSE
                               rsxx.close
                            'RESPONSE.Write "UPDATE hbcontractvolumeuse SET pagecnt=" & temppagecNt & " WHERE ctproperty='" & dspkey(1) & "' and cttree1='" & dspkey(2) & "' and cttree2='" & dspkey(3) & "' and areacode='" & dspkey(36) & "' "
                               connxx.execute "UPDATE hbcontractvolumeuse SET pagecnt=" & temppagecNt & " WHERE ctproperty='" & dspkey(1) & "' and cttree1='" & dspkey(2) & "' and cttree2='" & dspkey(3) & "' and areacode='" & dspkey(36) & "' "
                          '  RSXX("PAGECNT")=TEMPPAGECNT
                          '  RSXX.UPDATE
                            END IF
                         ELSE
                            tempvolume=1
                            temppagecnt=1
                         '無資料====>新增一筆冊號記錄
                            RSXX.CLOSE
                            connxx.execute "insert into hbcontractvolumeuse (ctproperty,cttree1,cttree2,areacode,volume,pagecnt) values('"& dspkey(1) & "','" & dspkey(2) & "','" & dspkey(3) & "','" & dspkey(36) & "'," & tempvolume & "," & temppagecnt&") "

                        
                      '   RSXX.Open sqlxx,connxx,3,3
                      '   RSXX.AddNew
                      '   RSXX("CTPROPERTY")=DSPKEY(1)
                      '   RSXX("CTTREE1")=DSPKEY(2)
                      '   RSXX("CTTREE2")=DSPKEY(3)
                      '   RSXX("AREACODE")=DSPKEY(36)
                      '   RSXX("VOLUME")=TEMPVOLUME
                      '   RSXX("PAGECNT")=TEMPPAGECNT
                      '   RSXX.UPDATE
                         END IF
                      else
                            tempvolume=1
                            temppagecnt=1
                         '無資料====>新增一筆冊號記錄
                            RSXX.CLOSE
                            connxx.execute "insert into hbcontractvolumeuse (ctproperty,cttree1,cttree2,areacode,volume,pagecnt) values('"& dspkey(1) & "','" & dspkey(2) & "','" & dspkey(3) & "','" & dspkey(36) & "'," & tempvolume & "," & temppagecnt&") "
                      end if
                      CONNXX.Close
                      SET RSXX=NOTHING
                      SET CONNXX=NOTHING
                      DSPKEY(34)=TEMPVOLUME
                      DSPKEY(35)=temppagecnt
                    end if
                'response.Write SQLXX
                 end if
                 if i<> 0  then rs.Fields(i).Value=dspKey(i)
                   '  RESPONSE.Write "I=" & i & ";value=" & dspkey(I) & "<br>"
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
             ' select case ucase(runpgm)   
                 ' 當程式為經銷商基本資料維護作業時
               '  case ucase("/webap/rtap/base/rconsignee/RTconsigneed.asp")
               '      if i<>3 then rs.Fields(i).Value=dspKey(i)                        
               '  case else
                    if i = 33 then
                       dspkey(33)=trim(dspkey(1)) & trim(dspkey(2)) & trim(dspkey(3)) & trim(dspkey(36))
                    end if
                    if i <> 0 then rs.Fields(i).Value=dspKey(i)
   
             '  end select
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
    if ucase(runpgm)=ucase("/webap/rtap/base/rtcontract/RTcontractd.asp") then
       rs.open "select max(ctno) AS comq1 from hbcontracth",conn
       if not rs.eof and len(trim(ctno)) > 0 then
          dspkey(0)=rs("ctno")
       end if
       rs.close
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
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<link REL="stylesheet" HREF="/WebUtilityV4/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="vbscript">
Sub Window_onLoad()
    if TRIM(document.all("parm1").value) <> "" AND TRIM(document.all("parm4").value)="Y" then
       document.all("key4").value=document.all("parm1").value
       document.all("key5").value=document.all("parm2").value
       document.all("key37").value=document.all("parm3").value
       document.all("parm4").value=""
    END IF
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
<form method="post" name="form" id="form">
<input type="text" name="sw" value="<%=sw%>" style="display:none;">
<input type="text" name="reNew" value="N" style="display:none;">
<input type="text" name="rwCnt" value="<%=rwCnt%>" style="display:none;">
<input type="text" name="accessMode" value="<%=accessMode%>" style="display:none;">
<table width="100%">
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
<!-- #include virtual="/Webap/include/employeeref.inc" -->

<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=1
  title="各類合約主檔資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT CTNO,CTPROPERTY,CTTREE1,CTTREE2,CTOBJECT,CTOBJNAME,CTTELNO,CTCONTACT," _
             &"CTEDIT,CUTID,TOWNSHIP,ADDRESS,RZONE,CTSTRDAT,CTENDDAT,ALARMDAT,RCVORPAY,ARAP,AUTOEXTEND,STRBILLINGYM, " _
             &"MONTHLYDAT,ARAPALARMDAT,SPLITRATE,REMITBANK1,REMITBANK2,AC,ACNO,SIGNDEPT,SIGNPERSON,RCVDAT,EDAT,EUSR,memo,CONTRACTNO,VOLUME,PAGECNT,AREACODE,comtype " _             
             &"FROM HBContractH WHERE ctno=0 "
  sqlList="SELECT CTNO,CTPROPERTY,CTTREE1,CTTREE2,CTOBJECT,CTOBJNAME,CTTELNO,CTCONTACT," _
             &"CTEDIT,CUTID,TOWNSHIP,ADDRESS,RZONE,CTSTRDAT,CTENDDAT,ALARMDAT,RCVORPAY,ARAP,AUTOEXTEND,STRBILLINGYM, " _
             &"MONTHLYDAT,ARAPALARMDAT,SPLITRATE,REMITBANK1,REMITBANK2,AC,ACNO,SIGNDEPT,SIGNPERSON,RCVDAT,EDAT,EUSR,memo,CONTRACTNO,VOLUME,PAGECNT,AREACODE,comtype " _             
             &"FROM HBContractH WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  userDefineSave="Yes"  
  userdefineactivex="Yes"  
  extDBField=0
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    if len(trim(dspkey(0))) = 0 then dspkey(0)=0
    if len(trim(dspkey(8))) = 0 then dspkey(8)=0
    if len(trim(dspkey(22))) = 0 then dspkey(22)=0
    if len(trim(dspkey(21))) = 0 then dspkey(21)=0    
    if len(trim(dspkey(15))) = 0 then dspkey(15)=0    
    if len(trim(dspkey(34))) = 0 then dspkey(34)=0
    if len(trim(dspkey(35))) = 0 then dspkey(35)=0
    if len(trim(dspkey(3))) = 0 then dspkey(3)="00"
    If len(trim(dspKey(1))) < 1 Then
       formValid=False
       message="請輸入合約性質"
    elseif len(trim(dspKey(2))) < 1 Then
       formValid=False
       message="請輸入合約類別一"
   ' elseif len(trim(dspKey(3))) < 1 Then
   '    formValid=False
   '    message="請輸入合約類別二"
    elseif len(trim(dspKey(4))) < 1 Then
       formValid=False
       message="請輸入合約對象統一編號(公司)或身份證字號(個人))"
    elseif len(trim(dspKey(5))) < 1  Then
       formValid=False
       message="請輸入合約對象名稱"
    elseif len(trim(dspKey(6))) < 1 Then
       formValid=False
       message="請輸入合約對象連絡電話"       
    elseif len(trim(dspKey(7))) < 1 Then
       formValid=False
       message="請輸入合約對象連絡人"
    elseif len(trim(dspKey(8))) > 0 and not IsNumeric(dspkey(8))Then
       formValid=False
       message="授信額度內容錯誤(僅能數字)。"       
    elseif len(trim(dspKey(15))) > 0 and not IsNumeric(dspkey(15)) Then
       formValid=False
       message="合約到期警示天數內容錯誤(僅能數字)"       
    elseif len(trim(dspKey(9))) < 1 OR (dspKey(9) <>"06" and dspKey(9) <>"15" and len(trim(dspKey(10))) < 1) OR len(trim(dspKey(11))) < 1 Then
       formValid=False
       message="請輸入合約對象地址"         
    elseif len(trim(dspKey(13))) < 1 AND TRIM(DSPKEY(1)) <> "A" Then
       formValid=False
       message="請輸入合約起日"         
    elseif len(trim(dspKey(14))) < 1 AND TRIM(DSPKEY(1)) <> "A" Then
       formValid=False
       message="請輸入合約終止日"                
    elseif len(trim(dspKey(17))) < 1 Then
       formValid=False
       message="請輸入收付別資料"       
    elseif dspKey(13) > dspKey(14) Then
       formValid=False
       message="合約起日不可大於合約迄日，請修正。"              
    elseif len(trim(dspKey(16))) < 1 Then
       formValid=False
       message="請輸入收付方式資料"     
    elseif len(trim(dspKey(19))) > 0 and len(trim(dspKey(19))) <> 6 Then
       formValid=False
       message="開始收付年月長度不符(6位yyyy+mm)"           
    elseif len(trim(dspKey(20))) > 0 and len(trim(dspKey(20))) <> 2 Then
       formValid=False
       message="開始收付日長度不符(2位dd)"          
    elseif len(trim(dspKey(21))) > 0 AND not IsNumeric(dspkey(21)) Then
       formValid=False
       message="合約收付日警示天數內容錯誤(僅能數字)!"         
    elseif len(trim(dspKey(22))) > 0 and not IsNumeric(dspkey(22)) Then
       formValid=False
       message="拆帳比率內容錯誤(僅能數字)!"                    
    elseif len(trim(dspKey(27))) < 1 Then
       formValid=False
       message="請輸入簽約部門資料"         
    elseif len(trim(dspKey(28))) < 1 Then
       formValid=False
       message="請輸入簽約經辦人資料"             
    elseif len(trim(dspKey(29))) < 1 Then
       formValid=False
       message="請輸入收件日期資料"                                     
    End If        
    if dspkey(18) <> "Y" and dspkey(18) <>"N" then dspkey(18)=""    
End Sub
' -------------------------------------------------------------------------------------------- 
' -------------------------------------------------------------------------------------------- 
Sub SrActiveXScript()%>
   <SCRIPT Language="VBScript">
   Sub KEY1_OnChange()
       <%=XXS%>
       document.all("KEY2D").innerHTML=aryproperty(document.all("KEY1").selectedIndex)
   End Sub   
  ' Sub KEY2_OnChange()
  '     <%=YYS%>
  '     document.all("KEY3D").innerHTML=arypropertyY(document.all("KEY1").selectedIndex,document.all("KEY2").selectedIndex)
  '     msgbox document.all("KEY3D").innerHTML
  ' End Sub   
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
   Sub Srbtn4onclick()
       prog="RTallcmtyk.asp"
       Scrxx=window.screen.width
       Scryy=window.screen.height - 30
       StrFeature="Top=0,left=0,scrollbars=yes,status=yes," _
                 &"location=no,menubar=no,width=" & scrxx & "px" _
                 &",height=" & scryy & "px"
      ' DiaWidth=window.screen.width
      ' DiaHeight=window.screen.height - 30
       Fcmty=Window.OPen(prog,"NewWindow",strFeature)
       'Window.form.Submit
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
   Sub Srcounty10onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY9").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key10").value =  trim(Fusrid(0))
          document.all("key12").value =  trim(Fusrid(1))
       End if       
       end if   
   End Sub           
   Sub Srcounty24onclick()
       prog="RTGetbankbranchD.asp"
       prog=prog & "?KEY=" & document.all("KEY23").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key24").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub       
   Sub SrCategory3onclick()
       prog="RTGetcategory2.asp"
       prog=prog & "?KEY=" & document.all("KEY1").VALUE & ";" & document.all("KEY2").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key3").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub       
   Sub Srcounty28onclick()
       prog="RTGetEMPLOYEED.asp"
       prog=prog & "?KEY=" & document.all("KEY27").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key28").value =  trim(Fusrid(0))
       End if       
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
<tr><td width="10%" class=dataListSearch>資料範圍</td>
    <td width="90%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="10%" class=dataListHead>建檔序號</td><td width="90%" bgcolor=silver>
           <input class=dataListdata type="text" name="key0" <%=keyprotect%> size="10" 
           value="<%=dspKey(0)%>"  readonly maxlength="10" ></td>
       </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(31))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                EUsrNc=V(1) 
                dspkey(31)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(31))
                V=split(rtnvalue,";")      
                EUsrNc=V(1)
        End if  
       dspkey(30)=datevalue(now())
    end if

'-----EXTDB DATA RETRIVE
DIM conn,rs,dsn,sql
SET conn=server.CreateObject("ADODB.Connection")
set rs=server.CreateObject("ADODB.recordset")
DSN="DSN=RTLIB"
conn.Open dsn
%>
<table border="1" width="100%" cellspacing="0" cellpadding="0">
  <tr>
   <td width="15%" bgcolor="#008080"><font color="#FFFFFF">合約性質</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <table cellspacing="0" cellpadding="0"><tr>
    <td width="15%"  bgcolor="#C0C0C0">
          <% s=""
    sx=" selected "
   ' If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
    If accessMode="A" And sw="" Then 
       sql="SELECT propertyid,propertynm FROM HBCONTRACTTREEH  ORDER BY propertyid "
       If len(trim(dspkey(1))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
       XXCLASS=" class="" datalistENTRY "" "
    Else
       sql="SELECT propertyid,propertynm FROM HBCONTRACTTREEH where propertyid='" & dspkey(1) & "' "
       XXCLASS=" class="" datalistDATA "" "
    End If
    rs.Open sql,conn,1,1
    Do While Not rs.Eof
       If rs("propertyid")=dspkey(1) Then sx=" selected "
       s=s &"<option value=""" &rs("propertyid") &"""" &sx &">" &rs("propertynm") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>       
       <select <%=XXCLASS%> name="key1" <%=dataProtect%> ><%=s%></select></TD>
       
    <td width="15%" ID="KEY2D" bgcolor="#C0C0C0">       
          <% s=""
    sx=" selected "
    'If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
    If accessMode="A" And sw="" Then     
       sql="SELECT propertyid,category1,category1nm FROM HBCONTRACTTREEL1  ORDER BY propertyid,category1 "
       If len(trim(dspkey(2))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
       SXX3=" onclick=""SrCategory3onclick()""  "
       XXCLASS=" class="" datalistENTRY "" "
    Else
       sql="SELECT propertyid,category1,category1nm FROM HBCONTRACTTREEL1 where propertyid='" & dspkey(1) & "' and category1='" & dspkey(2) & "' "
       SXX3=" "
       XXCLASS=" class="" datalistDATA "" "
    End If
    rs.Open sql,conn,1,1
    Do While Not rs.Eof
       If rs("propertyid")=dspkey(1) and rs("category1")=dspkey(2) Then sx=" selected "
       s=s &"<option value=""" &rs("category1") &"""" &sx &">" &rs("category1nm") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>       
    <select <%=XXCLASS%>  name="key2" <%=dataProtect%> size="1" 
       style="text-align:left;" maxlength="8" ><%=s%></select></TD>
    <td width="25%" ID="KEY3D" bgcolor="#C0C0C0">       
     <input class=dataListDATA name="key3" <%=dataprotect%> READONLY maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(3)%>" >
     <input type="button" id="B13"  name="B13" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=SXX3%>  >         
     <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除"  ID="C3" name="C3"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
     </TD></TD>
    <td width="15%" bgcolor="#C0C0C0">       
      <% s=""
    sx=" selected "
    'If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
    If accessMode="A" And sw="" Then 
       sql="SELECT CODE,CODENC FROM RTCODE where KIND='F2' ORDER BY KIND,CODE "
       If len(trim(dspkey(36))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
       XXCLASS=" class="" datalistENTRY "" "
    Else
       sql="SELECT CODE,CODENC FROM RTCODE where KIND='F2' AND CODE='" & DSPKEY(36) & "' ORDER BY KIND,CODE "
       XXCLASS=" class="" datalistDATA "" "
    End If
    rs.Open sql,conn,1,1
    Do While Not rs.Eof
       If rs("CODE")=dspkey(36) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>       
    <select <%=XXCLASS%>  name="key36" <%=dataProtect%> size="1" 
       style="text-align:left;" maxlength="8" ><%=s%></select>
       </TD>    
      <td width="15%"  bgcolor="#C0C0C0">
     <input class=dataListdata name="key33" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(33)%>" readonly ID="Text1">    </td> </TD></tr></table>   　
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">冊號-頁號</font></td>
    <td width="28%" bgcolor="#C0C0C0">第
     <input class=dataListdata name="key34" <%=dataprotect%> maxlength=3 size=3 style="TEXT-ALIGN: left" value
            ="<%=dspkey(34)%>" readonly ID="Text2">冊／第
     <input class=dataListdata name="key35" <%=dataprotect%> maxlength=3 size=3 style="TEXT-ALIGN: left" value
            ="<%=dspkey(35)%>" readonly ID="Text3">頁</td>        
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">對象統編</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key4" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(4)%>">社區查詢
    <% If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
          XXBTN=" onclick=""srbtn4onclick"" "
       else
          XXBTN="" 
       end if
    %>
   <input type="button" id="B4"  name="B4" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=XXBTN%>>  
            對象名稱
    <input class=dataListEntry name="key5" <%=dataprotect%> maxlength=50 size=30 style="TEXT-ALIGN: left" value
            ="<%=dspkey(5)%>" ID="Text4">　　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">社區種類</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListdata readonly name="key37" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(37)%>" ID="Text12">
    　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">連絡電話</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key6" <%=dataprotect%> maxlength=15 size=15 style="TEXT-ALIGN: left" value
            ="<%=dspkey(6)%>"><font color=black >連絡人</font><input class=dataListEntry name="key7" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(7)%>" ID="Text11">　</td>

  <td width="15%" bgcolor="#008080"><font color="#FFFFFF">授信額度</font>　</td>
    <td width="25%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="key8" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(8)%>" ID="Text7">　</td>
  </tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">地址</font></td>
        <td width="60%" height="25" bgcolor="silver">
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(9))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市別)</option>"       
       SXX10=" onclick=""Srcounty10onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(9) & "' " 
       SXX10=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(9) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
         <select size="1" name="key9"<%=dataProtect%> size="1" class="dataListEntry" ><%=s%></select>
        <input type="text" name="key10" size="8" value="<%=dspkey(10)%>" maxlength="10" readonly <%=dataProtect%> class="dataListEntry" ><font size=2>(鄉鎮市區)                 
         <input type="button" id="B10"  name="B10"   width="100%" style="Z-INDEX: 1"  value="..." <%=SXX10%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C10"  name="C10"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
        <input type="text" name="key11" size="30" value="<%=dspkey(11)%>" maxlength="60" <%=dataProtect%> class="dataListEntry" ></td>                                 
        <td width="15%" class="dataListHead" height="25">郵遞區號</td>                                 
        <td width="25%" height="25" bgcolor="silver"><input type="text" name="key12" size="10" value="<%=dspkey(12)%>" maxlength="5" <%=dataProtect%> class="dataListdata" readonly ></td>                                 
    
  </tr>  
  <tr>
      <td width="15%" bgcolor="#008080"><font color="#FFFFFF">合約起始日</font>　</td>
    <td width="45%" bgcolor="#C0C0C0" >
     <input class=dataListEntry name="key13" <%=dataprotect%> READONLY maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(13)%>">
    <input type="button" id="B13"  name="B13" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除"  ID="C13" name="C13"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
    </td>
      <td width="15%" bgcolor="#008080"><font color="#FFFFFF">合約終止日</font>　</td>
    <td width="25%" bgcolor="#C0C0C0" colspan="3">
     <input class=dataListEntry name="key14" <%=dataprotect%> READONLY maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(14)%>">
    <input type="button" id="B14"  name="B14" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除"  ID="C14" name="C14"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
</td>            
  </tr>          
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">合約到期警示天數</font></td>
    <td width="45%" bgcolor="#C0C0C0">
        <input class=dataListEntry name="KEY15" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=DSPKEY(15)%>"></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">收付別</font></td>
    <td width="25%" bgcolor="#C0C0C0">
      <% s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT CODE,CODENC FROM RTCode where kind='F7' ORDER BY CODE "
       If len(trim(dspkey(16))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
    Else
       sql="SELECT code,codenc FROM RTcode WHERE KIND='F7' AND CODE='" &dspkey(16) &"' "
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("code")=dspkey(16) Then sx=" selected "
       s=s &"<option value=""" &rs("code") &"""" &sx &">" &rs("Codenc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>
    <select class=dataListEntry name="key16" <%=dataProtect%> size="1" 
       style="text-align:left;" maxlength="8" ><%=s%></select>  </td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">收付方式</font></td>
    <td width="45%" bgcolor="#C0C0C0">
      <% s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT CODE,CODENC FROM RTCode where kind='F5' ORDER BY CODE "
       If len(trim(dspkey(17))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
    Else
       sql="SELECT code,codenc FROM RTcode WHERE KIND='F5' AND CODE='" &dspkey(17) &"' "
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("code")=dspkey(17) Then sx=" selected "
       s=s &"<option value=""" &rs("code") &"""" &sx &">" &rs("Codenc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>
    <select class=dataListEntry name="key17" <%=dataProtect%> size="1" 
       style="text-align:left;" maxlength="8" ><%=s%></select> 
　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">自動續約</font></td>
<%  dim AUTO1, AUTO2
       If Len(dataProtect) < 1 Then
          AUTO1=""
          AUTO2=""
       Else
          AUTO1=" disabled "
          AUTO2=" disabled "
       end if
    If dspKey(18)="Y" Then AUTO1=" checked "    
    If dspKey(18)="N" Then AUTO2=" checked " %>                          
        <td width="25%" height="25" bgcolor="silver">
        <input type="radio" name="key18" value="Y" <%=AUTO1%> <%=dataProtec%> ><font size=2>是</font>
        <input type="radio" name="key18" value="N" <%=AUTO2%><%=dataProtect%> ><font size=2>否</font>            
  　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">開始收付年月(yyyy+mm)</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key19" <%=dataprotect%> maxlength=6 size=6 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(19)%>" ></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">開始收付日(dd)</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key20"  maxlength=2 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(20)%>" >　</td>
  </tr>
    <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">收付日警示天數</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key21" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(21)%>" ></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">拆帳比率</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key22"  maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(22)%>"  >　</td>
  </tr>
    <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">匯款銀行代號</font></td>
    <td width="45%" bgcolor="#C0C0C0">
 <% s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT headno,headnc FROM RTbank ORDER BY  HEADNC, HEADNO "
       If len(trim(dspkey(23))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
       SXX24=" onclick=""Srcounty24onclick()""  "
    Else
       sql="SELECT HEADNO,headnc FROM RTbank WHERE headno='" &dspkey(23) &"' ORDER BY  HEADNC, HEADNO "
       SXX24=""
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("headno")=dspkey(23) Then sx=" selected "
       s=s &"<option value=""" &rs("headno") &"""" &sx &">" &rs("headnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>
    <select class=dataListEntry name="key23" <%=dataProtect%> size="1" 
       style="text-align:left;" maxlength="8" ><%=s%></select>     
    </td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">匯款分行代號</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key24"  maxlength=4 size=4 style="TEXT-ALIGN: left" value
            ="<%=dspkey(24)%>"  readOnly >
    <input type="button" id="B24"  name="B24"   width="100%" style="Z-INDEX: 1"  value="..." <%=SXX24%>  >    
    </td>
  </tr>  
    <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">匯款戶名</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key25" <%=dataprotect%> maxlength=30 size=30 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(25)%>"  ID="Text5"></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">匯款帳號</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key26"  maxlength=15 size=15 style="TEXT-ALIGN: left" value
            ="<%=dspkey(26)%>"  ID="Text6"></td>
  </tr>    
    <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">簽約部門</font></td>
    <td width="45%" bgcolor="#C0C0C0">
  <%s=""
    sx=" selected " 
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)Then 
       sql="SELECT * FROM RTdept where tdat <= GETDATE() AND ((exdat IS NULL) OR " _
          &"exdat >= GETDATE()) ORDER BY dept "
       If len(trim(dspkey(27))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  & vbcrlf
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>" & vbcrlf 
       end if            
       SXX28=" onclick=""Srcounty28onclick()""  "       
    Else
       sql="SELECT * FROM RTdept WHERE dept='" &dspkey(27) &"' " & vbcrlf
       SXX28=""
    End if 
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("dept")=dspkey(27) Then sx=" selected "
          s=s &"<option value=""" &rs("dept") &"""" &sx &">" &trim(rs("deptn3")) & trim(rs("deptn4")) & trim(rs("deptn5"))  &"</option>" & vbcrlf
          rs.MoveNext
          sx=""
    Loop
    rs.Close%>      
    <select class=dataListEntry name="key27" <%=dataProtect%> size="1" 
               style="text-align:left;" maxlength="8" ><%=s%></select>         
    </td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">簽約經辦人</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key28"  maxlength=6 size=6 style="TEXT-ALIGN: left" value
            ="<%=dspkey(28)%>"  readOnly ID="Text8">
    <input type="button" id="B28"  name="B28"   width="100%" style="Z-INDEX: 1"  value="..." <%=SXX28%>  >              
    </td>
  </tr>      
    <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">收件日期</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key29" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(29)%>" readOnly ID="Text9">
    <input type="button" id="B29"  name="B29" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除"  ID="C29" name="C29"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
     </td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">建檔日期</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key30"  maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(30)%>"  readOnly ID="Text10"></td>
  </tr>        
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">建檔人員</font></td>
    <td width="45%" COLSPAN=3 bgcolor="#C0C0C0">
    <input class=dataListEntry  name="key31" readOnly size=6 maxlength=6 style="TEXT-ALIGN: left "
            value="<%=dspkey(31)%>"><%=EusrNc%>　</td>

  </tr>
  <tr>
    <td width="15%" colspan=4 bgcolor="#008080" align=center><font color="#FFFFFF">備註</font></td>
  </tr>
    <td width="100%" COLSPAN=4 bgcolor="#C0C0C0" align=center>
    <TEXTAREA cols="90%" name="key32" rows=4  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(32)%>" ><%=dspkey(32)%></TEXTAREA></td>
    <input class=dataListEntry style="display:none" name="parm1"  maxlength=15 size=15 style="TEXT-ALIGN: left" value
            ="<%=session("comq1xx")%>"  >
    <input class=dataListEntry style="display:none" name="parm2"  maxlength=15 size=15 style="TEXT-ALIGN: left" value
            ="<%=session("comnxx")%>" >  
    <input class=dataListEntry style="display:none" name="parm3"  maxlength=15 size=15 style="TEXT-ALIGN: left" value
            ="<%=session("comtypexx")%>" >  
    <input class=dataListEntry style="display:none" name="parm4"  maxlength=15 size=15 style="TEXT-ALIGN: left" value
            ="<%=session("FIRSTPROCESS")%>" >                                    
  </tr>  
</table>
<% conn.close
   set rs=nothing
   set conn=nothing
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrSaveExtDB(Smode)
End Sub
' -------------------------------------------------------------------------------------------- 
%>
