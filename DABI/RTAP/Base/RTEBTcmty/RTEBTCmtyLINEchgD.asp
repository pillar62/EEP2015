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
  Dim aryParmKey,ERRMSG,KEY9X,KEY10X
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
    ERRMSG=REQUEST.Form("ERRMSG")
    KEY9X=REQUEST.Form("KEY9X")
    KEY10X=REQUEST.Form("KEY10X")
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
                   case ucase("/webap/rtap/base/rtEBTcmty/RTEBTCmtylineCHGd.asp")
                      ' response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
                     '  response.write "ERRMSG=" & ERRMSG
                       if i <> 2 then rs.Fields(i).Value=dspKey(i)    
                       if i=2 then
                         Set rsc=Server.CreateObject("ADODB.Recordset")
                         cusidxx="M-" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
                         rsc.open "select max(PRTNO) AS PRTNO from rtEBTCMTYLINECHG where PRTNO like '" & cusidxx & "%' " ,conn
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
          '更新被移入主線的社區主線檔之移入社區與主線序號及被移入主線之社區主線異動檔
        IF DSPKEY(9) > 0 OR DSPKEY(10) > 0 THEN
          Set rsYY=Server.CreateObject("ADODB.Recordset")
          SQLYY=""
          sqlyy="select max(ENTRYNO) as ENTRYNO FROM RTEBTCMTYLINElog WHERE comq1=" & DSPKEY(9) & " and lineq1=" & DSPKEY(10) 
         ' response.write "SQLYY=" & sqlyy
          rsyy.Open sqlyy,conn
          if len(trim(rsyy("ENTRYNO"))) > 0 then
             ENTRYNO=rsyy("ENTRYNO") + 1
          else
             ENTRYNO=1
          end if
          rsyy.close
          set rsyy=nothing
          SQLYY=""
          sqlyy="insert into RTEBTCMTYLINElog " _
           &"SELECT  COMQ1, LINEQ1," & ENTRYNO & ", GETDATE(),'MI','" & DSPKEY(16) & "', " _
           &" CONSIGNEE, AREAID, GROUPID, SALESID, LINEIP, GATEWAY, " _
           &"SUBNET, DNSIP, PPPOEACCOUNT, PPPOEPASSWORD, LINETEL, LINERATE, " _
           &"CUTID, TOWNSHIP, VILLAGE, COD1, NEIGHBOR, COD2, STREET, COD3, SEC," _ 
           &"COD4, LANE, COD5, TOWN, COD6, ALLEYWAY, COD7, NUM, COD8, FLOOR, " _
           &"COD9, ROOM, COD10, ADDROTHER, RZONE, CUTID1, TOWNSHIP1, RADDR1, " _
           &"RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, RCVDAT, INSPECTDAT, " _
           &"AGREE, UNAGREEREASON, TECHCONTACT, TECHENGNAME, CONTACT1, " _
           &"CONTACT2, CONTACTMOBILE, CONTACTTEL, CONTACTEMAIL, CONTACTTIME1, " _
           &"CONTACTTIME2, UPDEBTCHKDAT, UPDEBTCHKUSR, UPDEBTDAT, " _
           &"EBTREPLYDAT, EBTREPLAYCODE, PROGRESSID, HINETNOTIFYDAT, " _
           &"ADSLAPPLYDAT, APPLYUPLOADDAT, APPLYUPLOADUSR, APPLYUPLOADTNS, " _
           &"EBTERRORCODE, SUPPLYRANGE, COBOSS, COBOSSENG, COID, COBOSSID, " _
           &"APPLYNAMEC, APPLYNAMEE, ENGADDR, CONTACTSTRTIME, " _
           &"CONTACTENDTIME, ADSLAPPLYUSR, APPLYPRTNO, MEMO, APPLYNO, " _
           &"SCHAPPLYDAT, CHTRCVD, SUGGESTTYPE, REPEATREASON, EUSR, EDAT, " _
           &"UUSR, UDAT, TELCOMROOM, DROPDAT, TRANSNOAPPLY, TRANSNODOCKET, " _
           &"LOANNAME, LOANSOCIAL, LOCKDAT,NULL,'', CANCELDAT, CANCELUSR,MOVETOCOMQ1,MOVETOLINEQ1,MOVETODAT,MOVEFROMCOMQ1,MOVEFROMLINEQ1,MOVEFROMDAT,CONTRACTNO,COTPORT1,COTPORT2,MDF1,MDF2   " _
           &"FROM RTEBTCMTYLINE where comq1=" & DSPKEY(9) & " and lineq1=" & DSPKEY(10)
             
          CONN.EXECUTE SQLYY
          If Err.number > 0 then
             errmsg=cstr(Err.number) & "=" & Err.description
          else
             SQLYY=""
             SQLYY= "UPDATE rtEBTCMTYLINE SET MOVEFROMCOMQ1=" & DSPKEY(0) & ",MOVEFROMLINEQ1=" & DSPKEY(1) & ",MOVEFROMDAT=GETDATE() where COMQ1=" & DSPKEY(9) & " AND LINEQ1=" & DSPKEY(10) 
             conn.Execute SQLYY
             If Err.number > 0 then
                '發生錯誤時，刪除異動檔所新增的異動資料
                errmsg=cstr(Err.number) & "=" & Err.description
                sqlyy="delete * FROM RTEBTCMTYLINElog WHERE comq1=" & DSPKEY(9) & " and lineq1=" & DSPKEY(10) & " and entryno=" & ENTRYNO
                CONN.Execute sqlyy 
             else
                errmsg=""
             end if      
          end if
          '更新移出主線的社區主線檔之移出社區與主線序號及被移出主線之社區主線異動檔
          Set rsYY=Server.CreateObject("ADODB.Recordset")
          SQLYY=""
          sqlyy="select max(ENTRYNO) as ENTRYNO FROM RTEBTCMTYLINElog WHERE comq1=" & DSPKEY(0) & " and lineq1=" & DSPKEY(1) 
          rsyy.Open sqlyy,conn
          if len(trim(rsyy("ENTRYNO"))) > 0 then
             ENTRYNO=rsyy("ENTRYNO") + 1
          else
             ENTRYNO=1
          end if
          rsyy.close
          set rsyy=nothing
          SQLYY=""
          sqlyy="insert into RTEBTCMTYLINElog " _
           &"SELECT  COMQ1, LINEQ1," & ENTRYNO & ", GETDATE(),'MO','" & DSPKEY(16) & "', " _
           &" CONSIGNEE, AREAID, GROUPID, SALESID, LINEIP, GATEWAY, " _
           &"SUBNET, DNSIP, PPPOEACCOUNT, PPPOEPASSWORD, LINETEL, LINERATE, " _
           &"CUTID, TOWNSHIP, VILLAGE, COD1, NEIGHBOR, COD2, STREET, COD3, SEC," _ 
           &"COD4, LANE, COD5, TOWN, COD6, ALLEYWAY, COD7, NUM, COD8, FLOOR, " _
           &"COD9, ROOM, COD10, ADDROTHER, RZONE, CUTID1, TOWNSHIP1, RADDR1, " _
           &"RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, RCVDAT, INSPECTDAT, " _
           &"AGREE, UNAGREEREASON, TECHCONTACT, TECHENGNAME, CONTACT1, " _
           &"CONTACT2, CONTACTMOBILE, CONTACTTEL, CONTACTEMAIL, CONTACTTIME1, " _
           &"CONTACTTIME2, UPDEBTCHKDAT, UPDEBTCHKUSR, UPDEBTDAT, " _
           &"EBTREPLYDAT, EBTREPLAYCODE, PROGRESSID, HINETNOTIFYDAT, " _
           &"ADSLAPPLYDAT, APPLYUPLOADDAT, APPLYUPLOADUSR, APPLYUPLOADTNS, " _
           &"EBTERRORCODE, SUPPLYRANGE, COBOSS, COBOSSENG, COID, COBOSSID, " _
           &"APPLYNAMEC, APPLYNAMEE, ENGADDR, CONTACTSTRTIME, " _
           &"CONTACTENDTIME, ADSLAPPLYUSR, APPLYPRTNO, MEMO, APPLYNO, " _
           &"SCHAPPLYDAT, CHTRCVD, SUGGESTTYPE, REPEATREASON, EUSR, EDAT, " _
           &"UUSR, UDAT, TELCOMROOM, DROPDAT, TRANSNOAPPLY, TRANSNODOCKET, " _
           &"LOANNAME, LOANSOCIAL, LOCKDAT,NULL,'', CANCELDAT, CANCELUSR,MOVETOCOMQ1,MOVETOLINEQ1,MOVETODAT,MOVEFROMCOMQ1,MOVEFROMLINEQ1,MOVEFROMDAT,CONTRACTNO,COTPORT1,COTPORT2,MDF1,MDF2   " _
           &"FROM RTEBTCMTYLINE where comq1=" & DSPKEY(0) & " and lineq1=" & DSPKEY(1)
             
          CONN.EXECUTE SQLYY
          If Err.number > 0 then
             errmsg=cstr(Err.number) & "=" & Err.description
          else
             SQLYY=""
             SQLYY= "UPDATE rtEBTCMTYLINE SET MOVETOCOMQ1=" & DSPKEY(9) & ",MOVETOLINEQ1=" & DSPKEY(10) & ",MOVETODAT=GETDATE() where COMQ1=" & DSPKEY(0) & " AND LINEQ1=" & DSPKEY(1) 
             conn.Execute SQLYY
             If Err.number > 0 then
                '發生錯誤時，刪除異動檔所新增的異動資料
                errmsg=cstr(Err.number) & "=" & Err.description
                SQLYY=""
                sqlyy="delete * FROM RTEBTCMTYLINElog WHERE comq1=" & DSPKEY(0) & " and lineq1=" & DSPKEY(1) & " and entryno=" & ENTRYNO
                CONN.Execute sqlyy 
             else
                errmsg=""
             end if      
          end if
       END IF
      '-------------------------------------------------------------------------
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
                 case ucase("/webap/rtap/base/rtEBTcmty/RTEBTCmtyLINEd.asp")
                    'response.write "I=" & i & ";VALUE=" & dspkey(i) & "<BR>"
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
       '修改時，若有異動到新社區及主線序號者，則必須執行下列動作
       IF DSPKEY(9) <> KEY9X OR DSPKEY(10) <> KEY10X THEN
          '更新異動前的被移入主線的社區主線檔之移入社區與主線序號及被移入主線之社區主線異動檔
          '(異動前的新社區主線不為0時才執行)
          IF KEY9X > 0 OR KEY10X > 0 THEN
             Set rsYY=Server.CreateObject("ADODB.Recordset")
             SQLYY=""
             sqlyy="select max(ENTRYNO) as ENTRYNO FROM RTEBTCMTYLINElog WHERE comq1=" & KEY9X & " and lineq1=" & KEY10X
             rsyy.Open sqlyy,conn
             if len(trim(rsyy("ENTRYNO"))) > 0 then
                ENTRYNO=rsyy("ENTRYNO") + 1
             else
                ENTRYNO=1
             end if
             rsyy.close
             set rsyy=nothing
             SQLYY=""
             sqlyy="insert into RTEBTCMTYLINElog " _
              &"SELECT  COMQ1, LINEQ1," & ENTRYNO & ", GETDATE(),'UI','" & DSPKEY(18) & "', " _
              &" CONSIGNEE, AREAID, GROUPID, SALESID, LINEIP, GATEWAY, " _
              &"SUBNET, DNSIP, PPPOEACCOUNT, PPPOEPASSWORD, LINETEL, LINERATE, " _
              &"CUTID, TOWNSHIP, VILLAGE, COD1, NEIGHBOR, COD2, STREET, COD3, SEC," _ 
              &"COD4, LANE, COD5, TOWN, COD6, ALLEYWAY, COD7, NUM, COD8, FLOOR, " _
              &"COD9, ROOM, COD10, ADDROTHER, RZONE, CUTID1, TOWNSHIP1, RADDR1, " _
              &"RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, RCVDAT, INSPECTDAT, " _
              &"AGREE, UNAGREEREASON, TECHCONTACT, TECHENGNAME, CONTACT1, " _
              &"CONTACT2, CONTACTMOBILE, CONTACTTEL, CONTACTEMAIL, CONTACTTIME1, " _
              &"CONTACTTIME2, UPDEBTCHKDAT, UPDEBTCHKUSR, UPDEBTDAT, " _
              &"EBTREPLYDAT, EBTREPLAYCODE, PROGRESSID, HINETNOTIFYDAT, " _
              &"ADSLAPPLYDAT, APPLYUPLOADDAT, APPLYUPLOADUSR, APPLYUPLOADTNS, " _
              &"EBTERRORCODE, SUPPLYRANGE, COBOSS, COBOSSENG, COID, COBOSSID, " _
              &"APPLYNAMEC, APPLYNAMEE, ENGADDR, CONTACTSTRTIME, " _
              &"CONTACTENDTIME, ADSLAPPLYUSR, APPLYPRTNO, MEMO, APPLYNO, " _
              &"SCHAPPLYDAT, CHTRCVD, SUGGESTTYPE, REPEATREASON, EUSR, EDAT, " _
              &"UUSR, UDAT, TELCOMROOM, DROPDAT, TRANSNOAPPLY, TRANSNODOCKET, " _
              &"LOANNAME, LOANSOCIAL, LOCKDAT,NULL,'', CANCELDAT, CANCELUSR,MOVETOCOMQ1,MOVETOLINEQ1,MOVETODAT,MOVEFROMCOMQ1,MOVEFROMLINEQ1,MOVEFROMDAT,CONTRACTNO,COTPORT1,COTPORT2,MDF1,MDF2   " _
              &"FROM RTEBTCMTYLINE where comq1=" & KEY9X & " and lineq1=" & KEY10X
             
             CONN.EXECUTE SQLYY
             If Err.number > 0 then
                errmsg=cstr(Err.number) & "=" & Err.description
             else
                SQLYY=""
                SQLYY= "UPDATE rtEBTCMTYLINE SET MOVEFROMCOMQ1=0,MOVEFROMLINEQ1=0,MOVEFROMDAT=NULL where COMQ1=" & KEY9X & " AND LINEQ1=" & KEY10X
                conn.Execute SQLYY
                If Err.number > 0 then
                   '發生錯誤時，刪除異動檔所新增的異動資料
                   errmsg=cstr(Err.number) & "=" & Err.description
                   SQLYY=""
                   sqlyy="delete * FROM RTEBTCMTYLINElog WHERE comq1=" & DSPKEY(9) & " and lineq1=" & DSPKEY(10) & " and entryno=" & ENTRYNO
                   CONN.Execute sqlyy 
                else
                   errmsg=""
                end if      
             end if
          END IF
          '更新被移入主線的社區主線檔之移入社區與主線序號及被移入主線之社區主線異動檔
         IF DSPKEY(9) > 0 OR DSPKEY(10) > 0 THEN
          Set rsYY=Server.CreateObject("ADODB.Recordset")
          SQLYY=""
          sqlyy="select max(ENTRYNO) as ENTRYNO FROM RTEBTCMTYLINElog WHERE comq1=" & DSPKEY(9) & " and lineq1=" & DSPKEY(10) 
          rsyy.Open sqlyy,conn
          if len(trim(rsyy("ENTRYNO"))) > 0 then
             ENTRYNO=rsyy("ENTRYNO") + 1
          else
             ENTRYNO=1
          end if
          rsyy.close
          set rsyy=nothing
          SQLYY=""
          sqlyy="insert into RTEBTCMTYLINElog " _
           &"SELECT  COMQ1, LINEQ1," & ENTRYNO & ", GETDATE(),'MI','" & DSPKEY(18) & "', " _
           &" CONSIGNEE, AREAID, GROUPID, SALESID, LINEIP, GATEWAY, " _
           &"SUBNET, DNSIP, PPPOEACCOUNT, PPPOEPASSWORD, LINETEL, LINERATE, " _
           &"CUTID, TOWNSHIP, VILLAGE, COD1, NEIGHBOR, COD2, STREET, COD3, SEC," _ 
           &"COD4, LANE, COD5, TOWN, COD6, ALLEYWAY, COD7, NUM, COD8, FLOOR, " _
           &"COD9, ROOM, COD10, ADDROTHER, RZONE, CUTID1, TOWNSHIP1, RADDR1, " _
           &"RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, RCVDAT, INSPECTDAT, " _
           &"AGREE, UNAGREEREASON, TECHCONTACT, TECHENGNAME, CONTACT1, " _
           &"CONTACT2, CONTACTMOBILE, CONTACTTEL, CONTACTEMAIL, CONTACTTIME1, " _
           &"CONTACTTIME2, UPDEBTCHKDAT, UPDEBTCHKUSR, UPDEBTDAT, " _
           &"EBTREPLYDAT, EBTREPLAYCODE, PROGRESSID, HINETNOTIFYDAT, " _
           &"ADSLAPPLYDAT, APPLYUPLOADDAT, APPLYUPLOADUSR, APPLYUPLOADTNS, " _
           &"EBTERRORCODE, SUPPLYRANGE, COBOSS, COBOSSENG, COID, COBOSSID, " _
           &"APPLYNAMEC, APPLYNAMEE, ENGADDR, CONTACTSTRTIME, " _
           &"CONTACTENDTIME, ADSLAPPLYUSR, APPLYPRTNO, MEMO, APPLYNO, " _
           &"SCHAPPLYDAT, CHTRCVD, SUGGESTTYPE, REPEATREASON, EUSR, EDAT, " _
           &"UUSR, UDAT, TELCOMROOM, DROPDAT, TRANSNOAPPLY, TRANSNODOCKET, " _
           &"LOANNAME, LOANSOCIAL, LOCKDAT,NULL,'', CANCELDAT, CANCELUSR,MOVETOCOMQ1,MOVETOLINEQ1,MOVETODAT,MOVEFROMCOMQ1,MOVEFROMLINEQ1,MOVEFROMDAT,CONTRACTNO,COTPORT1,COTPORT2,MDF1,MDF2   " _
           &"FROM RTEBTCMTYLINE where comq1=" & DSPKEY(9) & " and lineq1=" & DSPKEY(10)
             
          CONN.EXECUTE SQLYY
          If Err.number > 0 then
             errmsg=cstr(Err.number) & "=" & Err.description
          else
             SQLYY=""
             SQLYY= "UPDATE rtEBTCMTYLINE SET MOVEFROMCOMQ1=" & DSPKEY(0) & ",MOVEFROMLINEQ1=" & DSPKEY(1) & ",MOVEFROMDAT=GETDATE() where COMQ1=" & DSPKEY(9) & " AND LINEQ1=" & DSPKEY(10) 
             conn.Execute SQLYY
             If Err.number > 0 then
                '發生錯誤時，刪除異動檔所新增的異動資料
                errmsg=cstr(Err.number) & "=" & Err.description
                SQLYY=""
                sqlyy="delete * FROM RTEBTCMTYLINElog WHERE comq1=" & DSPKEY(9) & " and lineq1=" & DSPKEY(10) & " and entryno=" & ENTRYNO
                CONN.Execute sqlyy 
             else
                errmsg=""
             end if      
          end if
          '更新移出主線的社區主線檔之移出社區與主線序號及被移出主線之社區主線異動檔
          Set rsYY=Server.CreateObject("ADODB.Recordset")
          SQLYY=""
          sqlyy="select max(ENTRYNO) as ENTRYNO FROM RTEBTCMTYLINElog WHERE comq1=" & DSPKEY(0) & " and lineq1=" & DSPKEY(1) 
          rsyy.Open sqlyy,conn
          if len(trim(rsyy("ENTRYNO"))) > 0 then
             ENTRYNO=rsyy("ENTRYNO") + 1
          else
             ENTRYNO=1
          end if
          rsyy.close
          set rsyy=nothing
          SQLYY=""
          sqlyy="insert into RTEBTCMTYLINElog " _
           &"SELECT  COMQ1, LINEQ1," & ENTRYNO & ", GETDATE(),'MO','" & DSPKEY(18) & "', " _
           &" CONSIGNEE, AREAID, GROUPID, SALESID, LINEIP, GATEWAY, " _
           &"SUBNET, DNSIP, PPPOEACCOUNT, PPPOEPASSWORD, LINETEL, LINERATE, " _
           &"CUTID, TOWNSHIP, VILLAGE, COD1, NEIGHBOR, COD2, STREET, COD3, SEC," _ 
           &"COD4, LANE, COD5, TOWN, COD6, ALLEYWAY, COD7, NUM, COD8, FLOOR, " _
           &"COD9, ROOM, COD10, ADDROTHER, RZONE, CUTID1, TOWNSHIP1, RADDR1, " _
           &"RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, RCVDAT, INSPECTDAT, " _
           &"AGREE, UNAGREEREASON, TECHCONTACT, TECHENGNAME, CONTACT1, " _
           &"CONTACT2, CONTACTMOBILE, CONTACTTEL, CONTACTEMAIL, CONTACTTIME1, " _
           &"CONTACTTIME2, UPDEBTCHKDAT, UPDEBTCHKUSR, UPDEBTDAT, " _
           &"EBTREPLYDAT, EBTREPLAYCODE, PROGRESSID, HINETNOTIFYDAT, " _
           &"ADSLAPPLYDAT, APPLYUPLOADDAT, APPLYUPLOADUSR, APPLYUPLOADTNS, " _
           &"EBTERRORCODE, SUPPLYRANGE, COBOSS, COBOSSENG, COID, COBOSSID, " _
           &"APPLYNAMEC, APPLYNAMEE, ENGADDR, CONTACTSTRTIME, " _
           &"CONTACTENDTIME, ADSLAPPLYUSR, APPLYPRTNO, MEMO, APPLYNO, " _
           &"SCHAPPLYDAT, CHTRCVD, SUGGESTTYPE, REPEATREASON, EUSR, EDAT, " _
           &"UUSR, UDAT, TELCOMROOM, DROPDAT, TRANSNOAPPLY, TRANSNODOCKET, " _
           &"LOANNAME, LOANSOCIAL, LOCKDAT,NULL,'', CANCELDAT, CANCELUSR,MOVETOCOMQ1,MOVETOLINEQ1,MOVETODAT,MOVEFROMCOMQ1,MOVEFROMLINEQ1,MOVEFROMDAT,CONTRACTNO,COTPORT1,COTPORT2,MDF1,MDF2   " _
           &"FROM RTEBTCMTYLINE where comq1=" & DSPKEY(0) & " and lineq1=" & DSPKEY(1)
             
          CONN.EXECUTE SQLYY
          If Err.number > 0 then
             errmsg=cstr(Err.number) & "=" & Err.description
          else
             SQLYY=""
             SQLYY= "UPDATE rtEBTCMTYLINE SET MOVETOCOMQ1=" & DSPKEY(9) & ",MOVETOLINEQ1=" & DSPKEY(10) & ",MOVETODAT=GETDATE() where COMQ1=" & DSPKEY(0) & " AND LINEQ1=" & DSPKEY(1) 
             conn.Execute SQLYY
             If Err.number > 0 then
                '發生錯誤時，刪除異動檔所新增的異動資料
                errmsg=cstr(Err.number) & "=" & Err.description
                SQLYY=""
                sqlyy="delete * FROM RTEBTCMTYLINElog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & " and entryno=" & ENTRYNO
                CONN.Execute sqlyy 
             else
                errmsg=""
             end if      
          end if
        END IF
      END IF
    End If
    rs.Close   
    ' 當程式為HB社區基本資料維護作業時,將sql自行產生之identify值dspkey(0)讀出至畫面
    if accessmode ="A" then
       runpgm=Request.ServerVariables("PATH_INFO")
       if ucase(runpgm)=ucase("/webap/rtap/base/rtEBTcmty/RTEBTCmtyLINECHGd.asp") then
          cusidxx="M-" & right("00" & trim(datePART("yyyy",NOW())),2) & right("00" & trim(datePART("m",NOW())),2)& right("00" & trim(datePART("d",NOW())),2)
          rsc.open "select max(PRTNO) AS PRTNO from rtEBTCMTYLINECHG where PRTNO like '" & cusidxx & "%' " ,conn
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
    KEY9X=rs.Fields(9).Value
    KEY10X=rs.Fields(10).Value
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
  numberOfKey=3
  title="AVS主線異動申請單維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="select COMQ1,LINEQ1,PRTNO,APPLYDAT,PRTDAT,PRTUSR,CHGCOD1,CHGCOD2,CHGCOD3,NCOMQ1,NLINEQ1,UPDEBTCHKDAT,UPDEBTTNSDAT," _
             &"UPDEBTTNSNO, EBTREPLYDAT, EBTREPLYSTS, EUSR, EDAT, UUSR, UDAT, MEMO,FINISHDAT,DOCKETDAT,TRANSDAT,TRANSNO,EBTREPLYFHDAT,EBTREPLYFHSTS,DROPDAT,DROPUSR " _
             &"FROM RTEBTCmtyLINEchg WHERE COMQ1=0 "
  sqlList="select COMQ1, LINEQ1, PRTNO,APPLYDAT,PRTDAT,PRTUSR,CHGCOD1,CHGCOD2,CHGCOD3,NCOMQ1,NLINEQ1,UPDEBTCHKDAT,UPDEBTTNSDAT," _
             &"UPDEBTTNSNO, EBTREPLYDAT,EBTREPLYSTS, EUSR, EDAT, UUSR, UDAT, MEMO,FINISHDAT,DOCKETDAT,TRANSDAT,TRANSNO,EBTREPLYFHDAT,EBTREPLYFHSTS,DROPDAT,DROPUSR " _
             &"FROM RTEBTCmtyLINEchg WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    '檢查該主線是否已測通,已測通之主線才可進行線路移機作業
    Set connXX=Server.CreateObject("ADODB.Connection")
    Set rsXX=Server.CreateObject("ADODB.Recordset")
    connXX.open DSN
    SQLXX="SELECT * FROM RTEBTCMTYLINE WHERE COMQ1=" & DSPKEY(0) & " AND LINEQ1=" & DSPKEY(1)
    RSXX.Open sqlXX,CONNXX
    errcode=""
    IF RSXX.EOF THEN
       formValid=False
       message="主線資料不存在，無法移機"  
    ELSE
       '主線尚未測通不可移機
       IF ISNULL(RSXX("ADSLAPPLYDAT")) then
          formValid=False
          message="主線尚未測通不可移機"    
       ELSE
          ERRcode=""
          if len(trim(DSPKEY(6))) = 0 or dspkey(6) < 1  THEN DSPKEY(6)=0
          if len(trim(DSPKEY(7))) = 0 or dspkey(7) < 1  THEN DSPKEY(7)=0
          if len(trim(DSPKEY(8))) = 0 or dspkey(8) < 1  THEN DSPKEY(8)=0
          if len(DSPKEY(9)) = 0 or dspkey(9) ="" THEN DSPKEY(9)=0
          if len(DSPKEY(10)) = 0 or dspkey(10) = ""  THEN DSPKEY(10)=0
          If (len(trim(DSPKEY(6))) = 0 or dspkey(6) < 1) and  (len(trim(DSPKEY(7))) = 0 or dspkey(7) < 1) and  (len(trim(DSPKEY(8))) = 0 or dspkey(8) < 1)    then
             formValid=False
             message="異動項目至少需選取一項"    
          elseif (dspkey(6) > 0) and  (dspkey(7) > 0) then
             formValid=False
             message="頻寬異動不可同時選擇二項"    
          elseif isnull(dspkey(3)) or len(trim(dspkey(3))) = 0  THEN
             formValid=False
             message="異動申請不可空白"          
          elseif DSPKEY(8) > 0 and (len(DSPKEY(9)) = 0 or dspkey(9) ="") or (len(DSPKEY(10)) = 0 or dspkey(10) = "" ) THEN
             formValid=False
             message="移機時，必須輸入新社區及新主線序號"        
          elseif DSPKEY(8) > 0 AND errmsg <> "OK" AND (LEN(TRIM(DSPKEY(11)))=0 OR ISNULL(DSPKEY(11)) ) THEN
             formValid=False
             message="移機異動尚未申請前，請先驗證新社區及主線資料(主線檢查鈕)"         
          elseif LEN(TRIM(DSPKEY(22))) > 0 AND (LEN(TRIM(DSPKEY(21)))=0 OR ISNULL(DSPKEY(21)) ) THEN
             formValid=False
             message="本異動尚未完工，不可輸入報竣審核日"                                            
          end if
       END IF
    END  IF
    RSXX.Close
   '檢查新社區主線之線路(進度)狀況
    IF DSPKEY(9) > 0 OR DSPKEY(10) > 0 THEN
       SQLXX="SELECT * FROM RTEBTCMTYLINE WHERE COMQ1=" & DSPKEY(9) & " AND LINEQ1=" & DSPKEY(10)
       RSXX.Open sqlXX,CONNXX
       IF RSXX.EOF THEN
          formValid=False
          message="新社區主線資料不存在，無法移機"  
       ELSEIF not ISNULL(RSXX("ADSLAPPLYDAT")) then
          formValid=False
          message="新社區主線已提出申請，不可再被移入主線"  
       ELSEIF not ISNULL(RSXX("UPDEBTCHKDAT")) then
          formValid=False
          message="新社區主線已開通，不可再被移入主線"            
       ELSEIF RSXX("MOVETOCOMQ1") > 0 OR RSXX("MOVETOLINEQ1") > 0 OR NOT ISNULL(RSXX("MOVETODAT")) THEN 
          formValid=False
          message="新社區主線已被其它線路(" & RSXX("MOVETOCOMQ1") & "-" & RSXX("MOVETOLINEQ1") & ")移入，不可重複再被移入線路!"
       END IF
       RSXX.CLOSE
    END IF
    
CONNXX.Close
SET RSXX=NOTHING
SET CONNXX=NOTHING
    
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(18)=V(0)
        dspkey(19)=datevalue(now())
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
       IF CLICKID="10" THEN
          document.all("KEY9").value =  ""
          document.all("ERRMSG").value =  ""
          I=0
          DO WHILE I <= 52
             IF I <> 46 AND I <> 50 THEN
                PARM="REF" & I
                document.All(PARM).value=""
             END IF
             I=I+1
          LOOP
          document.All("REF46X").CHECKED = FALSE
          document.All("REF46Y").CHECKED = FALSE
          document.All("REF50X").CHECKED = FALSE
          document.All("REF50Y").CHECKED = FALSE 
          document.all("key9").classname=" dataListentry "
          document.all("key10").classname=" dataListentry "
          document.all("key9").readonly=false
          document.all("key10").readonly=false
       END IF
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
   Sub SrTAG0()
       'msgbox window.SRTAB1.style.display
       if window.SRTAB0.style.display="" then
          window.SRTAB0.style.display="none"
       elseif window.SRTAB0.style.display="none" then
          window.SRTAB0.style.display=""
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
   SUB srkey8click()
     '控制當異動申請審核日不為空白時==>表示已提出異動申請,或己作廢之異動申請，不可再變更異動項目
     IF  Document.all("KEY11").VALUE ="" AND Document.all("KEY27").VALUE ="" THEN
       IF Document.all("KEY8").checked THEN
          window.SRTAR1.style.display=""
       ELSE
          window.SRTAR1.style.display="none"
          document.all("KEY9").value =  ""
          document.all("KEY10").value =  ""
          document.all("ERRMSG").value =  ""
          I=0
          DO WHILE I <= 52
             IF I <> 46 AND I <> 50 THEN
                PARM="REF" & I
                document.All(PARM).value=""
             END IF
             I=I+1
          LOOP
          document.All("REF46X").CHECKED = FALSE
          document.All("REF46Y").CHECKED = FALSE
          document.All("REF50X").CHECKED = FALSE
          document.All("REF50Y").CHECKED = FALSE 
          document.all("key9").classname=" dataListentry "
          document.all("key10").classname=" dataListentry "
          document.all("key9").readonly=false
          document.all("key10").readonly=false
       END IF
     ELSE
       IF Document.all("KEY8").checked THEN
          Document.all("KEY8").checked=FALSE
       ELSE
          Document.all("KEY8").checked=TRUE
       END IF
     END IF
   END SUB
   SUB SrSHOWDETAILOnClick()
       IF window.SRTAR1.style.display="" THEN
          window.SRTAR1.style.display="none"
          document.all("SHD").value="顯示明細"
       ELSE
          window.SRTAR1.style.display=""
          document.all("SHD").value="隱藏明細"
       end if
   END SUB
   SUB sroptclick()
     '控制當異動申請審核日不為空白時==表示已提出異動申請，不可再變更異動項目
     eventid=window.event.srcElement.id
     IF  Document.all("KEY11").VALUE ="" AND Document.all("KEY27").VALUE ="" THEN
       IF eventid="key6" then
          if Document.all("KEY7").checked=true then
             Document.all("KEY7").checked=false
          end if
       elseif eventid="key7" then
          if Document.all("KEY6").checked=true then
             Document.all("KEY6").checked=false
          end if
       END IF
     ELSE
       IF eventid="key6" THEN
          IF Document.all("KEY6").checked THEN
             Document.all("KEY6").checked=FALSE
          ELSE
             Document.all("KEY6").checked=TRUE
          END IF
       ELSEIF eventid="key7" THEN
          IF Document.all("KEY7").checked THEN
             Document.all("KEY7").checked=FALSE
          ELSE
             Document.all("KEY7").checked=TRUE
          END IF     
       END IF  
     END IF
   END SUB   
   SUB SrGETCOMLINEOnClick()
       IF Document.all("KEY8").checked=true then
          KEY9=Document.all("KEY9").VALUE
          KEY10=Document.all("KEY10").VALUE
          IF LEN(TRIM(KEY9))=0 OR KEY9="" THEN
             MSGBOX "新社區序號不可空白"
          ELSEIF LEN(TRIM(KEY10))=0 OR KEY10="" THEN
             MSGBOX "新主線序號不可空白"
          ELSE
             prog="RTGetCMTYLINEDATA.asp?KEY=" & Document.all("KEY9").VALUE & ";" & Document.all("KEY10").VALUE & ";" & "1" 
             RTN=Window.showModalDialog(prog,"d2","dialogWidth:600px;dialogHeight:400px;")  
             if RTN <> "" then
                RTNXX=Split(RTN,";") 
                IF RTNXX(0)="A" THEN
                   document.All("ERRMSG").value="新社區資料不存在!"
                ELSEIF RTNXX(0)="B" THEN
                   document.All("ERRMSG").value="新主線資料不存在!"
                ELSEIF RTNXX(0)="C" THEN
                   document.All("ERRMSG").value="新社區及新主線資料皆不存在!"
                ELSEIF RTNXX(0)="D" THEN
                   document.All("ERRMSG").value="新社區主線已測通，不可再被移入線路!"
                ELSEIF RTNXX(0)="E" THEN
                   document.All("ERRMSG").value="新社區主線已被其它線路(" & RTNXX(1) & "-" & RTNXX(2) & ")移入，不可重複再被移入線路!"
                ELSEIF RTNXX(0)="F" THEN
                   document.All("ERRMSG").value="新社區主線已作廢，不可移入線路!"
                ELSEIF RTNXX(0)="G" THEN
                   document.All("ERRMSG").value="新社區主線已撤線，不可移入線路!"
                ELSEIF RTNXX(0)="H" THEN
                   document.All("ERRMSG").value="新社區主線已向EBT提出申請，不可再被移入線路!"                   
                ELSE
                   document.All("ERRMSG").value="OK"
                   document.all("key9").classname=" dataListData "
                   document.all("key10").classname=" dataListData "
                   document.all("key9").readonly=true
                   document.all("key10").readonly=true
             '將新社區主線資料搬入畫面
                   I=0
                   DO WHILE I <= 52
                      IF I <> 46 AND I <> 50 THEN
                         PARM="REF" & I
                         document.All(PARM).value=RTNXX(I)
                      END IF
                      I=I+1
                   LOOP
                   IF RTNXX(46)="01" THEN
                      document.All("REF46X").CHECKED = TRUE
                   ELSEIF RTNXX(46)="02" THEN
                      document.ALL("REF46Y").CHECKED = TRUE
                   END IF
                   IF RTNXX(50)="01" THEN
                      document.All("REF50X").CHECKED = TRUE
                   ELSEIF RTNXX(50)="02" THEN
                      document.All("REF50Y").CHECKED = TRUE
                   END IF
                   IF RTNXX(51)="01" THEN
                      document.All("REF51").value="512K/64K"
                   ELSEIF RTNXX(51)="02" THEN
                      document.All("REF51").value="768K/128K"
                   ELSEIF RTNXX(51)="03" THEN
                      document.All("REF51").value="1.5M/384K"   
                   ELSEIF RTNXX(51)="04" THEN
                      document.All("REF51").value="512K/512K"   
                   END IF
                end if
                IF RTNXX(0) >="A" AND RTNXX(0) <="G" THEN
                   I=0
                   DO WHILE I <= 52
                      IF I <> 46 AND I <> 50 THEN
                         PARM="REF" & I
                         document.All(PARM).value=""
                      END IF
                      I=I+1
                   LOOP
                   document.All("REF46X").CHECKED = FALSE
                   document.All("REF46Y").CHECKED = FALSE
                   document.All("REF50X").CHECKED = FALSE
                   document.All("REF50Y").CHECKED = FALSE
                 END IF
             END IF 
          END IF
       else
          document.all("KEY9").value =  ""
          document.all("KEY10").value =  ""
          document.all("ERRMSG").value =  ""
          I=0
          DO WHILE I <= 52
             IF I <> 46 AND I <> 50 THEN
                PARM="REF" & I
                document.All(PARM).value=""
             END IF
             I=I+1
          LOOP
          document.All("REF46X").CHECKED = FALSE
          document.All("REF46Y").CHECKED = FALSE
          document.All("REF50X").CHECKED = FALSE
          document.All("REF50Y").CHECKED = FALSE 
          document.all("key9").classname=" dataListentry "
          document.all("key10").classname=" dataListentry "
          document.all("key9").readonly=false
          document.all("key10").readonly=false
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
       <tr><td width="10%" class=dataListHead>社區序號</td>
           <td width="15%"  bgcolor="silver">
           <input type="text" name="key0"
                 <%=fieldRole(1)%> readonly size="10" value="<%=dspKey(0)%>" maxlength="8" class=dataListdata></td>
           <td width="10%" class=dataListHead>主線序號</td>
           <td width="15%"  bgcolor="silver">
           <input type="text" name="key1"
                 <%=fieldRole(1)%> readonly size="10" value="<%=dspKey(1)%>" maxlength="8" class=dataListdata></td>   
          <td width="10%" class=dataListHead>異動單號</td>
          <td width="15%"  bgcolor="silver">
           <input type="text" name="key2"
                 <%=fieldRole(1)%> readonly size="15" value="<%=dspKey(2)%>" maxlength="15" class=dataListdata ID="Text37"></td>  </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(16))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(16)=V(0)
        End if  
       dspkey(17)=datevalue(now())
    else
        if len(trim(dspkey(18))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                DSpkey(18)=V(0)
        End if         
        dspkey(19)=datevalue(now())
    end if      
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
    '主線檢查ok時 將新社區及新主線序號protect
    If errmsg = "OK" Then
       fieldPa=" class=""dataListData"" readonly "
    Else
       fieldPa=""
    End If
   '主線異動已提出申請(申請審核日不為空白時) 除[移機報竣審核日及備註欄外其餘皆PROTECT
    IF LEN(TRIM(DSPKEY(11))) > 0 THEN
       fieldPB=" class=""dataListData"" readonly "
       FIELDPC=" DISABLED "
    ELSE
       FIELDB=""
       FIELDPC=""
    END IF
   '主線移機完工時,控制不可清除申請日
    IF LEN(TRIM(DSPKEY(21))) > 0 THEN
       FIELDPD=" DISABLED "
    ELSE
       FIELDPD=""
    END IF    
   '主線移機報竣確認時,控制不可更改報竣審核日
    IF LEN(TRIM(DSPKEY(22))) > 0 THEN
       FIELDPE=" DISABLED "
    ELSE
       FIELDPE=""
    END IF        
   '主線移機報竣確認時,控制不可更改報竣審核日
    IF LEN(TRIM(DSPKEY(23))) > 0 THEN
       FIELDPF=" DISABLED "
    ELSE
       FIELDPF=""
    END IF            
   '主線異動作廢時,控制全部欄位皆不可更改    
    IF LEN(TRIM(DSPKEY(27))) > 0 THEN   
       fieldPa=" class=""dataListData"" readonly "
       fieldPB=" class=""dataListData"" readonly "
       FIELDPC=" DISABLED "
       FIELDPD=" DISABLED "
       FIELDPE=" DISABLED "
       FIELDPF=" DISABLED "
       fieldPG=" class=""dataListData"" readonly "
    END IF            
    
      
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    Set rsXX=Server.CreateObject("ADODB.Recordset")
    conn.open DSN%>
  <!--
  <span id="tags1" class="dataListTagsOn"
        onClick="vbscript:tag1.style.display=''    :tags1.classname='dataListTagsOn':
                          tag2.style.display='none':tags2.classname='dataListTagsOf'">基本資料</span>
  <span id="tags2" class="dataListTagsOf"
        onClick="vbscript:tag1.style.display='none':tags1.classname='dataListTagsOf':
                          tag2.style.display=''    :tags2.classname='dataListTagsOn'">發包安裝</span>           
  -->
  <span id="tags1" class="dataListTagsOn">AVS主線資訊</span>
                                                            
  <div class=dataListTagOn> 
<table width="100%">
<tr><td width="2%">&nbsp;</td><td width="96%">&nbsp;</td><td width="2%">&nbsp;</td></tr>
<tr><td>&nbsp;</td><td>     
    <DIV ID="SRTAG0" onclick="srtag0" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table6">
    <tr><td bgcolor="BDB76B" align="LEFT">原始主線資料</td></tr></table></div>
 <DIV ID=SRTAB0  >   
<table width="100%" border=1 cellPadding=0 cellSpacing=0 id="tag1">
<tr><td  class=dataListHEAD>社區大樓名稱</td>
    <td  bgcolor="silver" COLSPAN=3>
<% sql="SELECT * FROM RTebtcmtyH where comq1=" & DSPkey(0) 
   rsXX.open sql,conn
   if not rsXX.eof then
      COMN=RSXX("COMN")
   else
      COMN=""
   end if
   RSXX.CLOSE
%>          
     <input type="text" style="text-align:left;" maxlength="30"
               value="<%=COMN%>"  readonly size="30" class=dataListdata ID="Text100" NAME="Text100">
    </td>
</tr>
<tr><td rowspan=2 class=dataListHEAD>公司負責人<br>(個人申請免填)</td>
    <td  bgcolor="silver" ><font size=2>中文︰</font>
<% sql="SELECT * FROM RTebtcmtyline where comq1=" & DSPkey(0) & " and lineq1=" & DSPKEY(1) 
   rs.open sql,conn
   if not rs.eof then
      coboss=rs("COBOSS")
   else
      coboss=""
   end if
%>      
        <input type="text" style="text-align:left;" maxlength="10"
               value="<%=coboss%>"  readonly size="10" class=dataListdata ID="Text22"></td>
<td width="15%" class=dataListHEAD>申請人統一編號</td>
    <td width="35%" bgcolor="silver" >
<% if not rs.eof then
      coid=rs("COid")
   else
      coid=""
   end if
%>     
        <input type="text"  style="text-align:left;" maxlength="10"
               value="<%=coid%>"  readonly  size="10" class=dataListDATA ID="Text23"></td>               
</tr>
<tr>
    <td  bgcolor="silver" ><font size=2>英文︰</font>
<% if not rs.eof then
      COBOSSENG=rs("COBOSSENG")
   else
      COBOSSENG=""
   end if
%>
 <input type="text" style="text-align:left;" maxlength="30"
               value="<%=cobosseng%>"  readonly  size="30" class=dataListDATA ID="Text24"></td>
<td  class=dataListHEAD>負責人身份證字號</td>
    <td  bgcolor="silver" >
<% if not rs.eof then
      COBOSSID=rs("COBOSSID")
   else
      COBOSSID=""
   end if
%>
        <input type="password"  style="text-align:left;" maxlength="10"
               value="<%=COBOSSID%>"  readonly  size="10" class=dataListDATA ID="Text25"></td>               
</tr>
<tr><td rowspan=2 class=dataListHEAD>申請人姓名<br>/公司名稱</td>
    <td  bgcolor="silver" colspan=3><font size=2>中文︰</font>
<% if not rs.eof then
      APPLYNAMEC=rs("APPLYNAMEC")
   else
      APPLYNAMEC=""
   end if %>    
       <input type="text" style="text-align:left;" maxlength="30"
               value="<%=APPLYNAMEC%>"  readonly  size="30" class=dataListDATA ID="Text26"></td>
</tr>
<tr>
    <td  bgcolor="silver" colspan=3><font size=2>英文︰</font>
<% if not rs.eof then
      APPLYNAMEe=rs("APPLYNAMEe")
   else
      APPLYNAMEe=""
   end if %>    
           <input type="text" style="text-align:left;" maxlength="50"
               value="<%=APPLYNAMEe%>"  readonly  size="50" class=dataListDATA ID="Text28"></td>
</tr>
<tr><td class=dataListHEAD>ADSL裝機地址</td>
    <td bgcolor="silver" COLSPAN=3><font size=2>中文︰</font>
  <%s=""
    sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & RS("CUTID") & "' " 
    sx=""    
    rsXX.Open sql,conn
    IF NOT RSXX.EOF THEN
       CUTNC=RSXX("CUTNC")
    ELSE
       CUTNC=""
    END IF
    RSXX.CLOSE
   %>
        <input type="text"  readonly  size="6" value="<%=CUTNC%>" maxlength="10" readonly  class="dataListDATA" ID="Text43" NAME="Text38">
        <input type="text"  readonly  size="8" value="<%=RS("TOWNSHIP")%>" maxlength="10" readonly class="dataListDATA" ID="Text4"><font size=2>(鄉鎮)                 
        <input type="text"  readonly  size="10" value="<%=RS("VILLAGE")%>" maxlength="10"class="dataListdata" ID="Text5"><font size=2>
        <% aryOption=Array("村","里")
      s=""
      s="<option value=""" &RS("COD1") &""">" &RS("COD1") &"</option>"
        %>                                  
       <select size="1"  class="dataListdata" ID="Select3">                                            
        <%=s%></select>      
        <input type="text" readonly size="6" value="<%=RS("NEIGHBOR")%>" maxlength="6"  class="dataListdata" ID="Text6"><font size=2>
        <% aryOption=Array("鄰")
      s=""
      s="<option value=""" &RS("COD2") &""">" &RS("COD2") &"</option>"
      %>
       <select size="1" class="dataListdata" ID="Select4">                                            
        <%=s%></select>              
        <input type="text" readonly size="10" value="<%=RS("STREET")%>" maxlength="10" class="dataListdata" ID="Text27"><font size=2>
        <% aryOption=Array("路","街")
      s=""
      s="<option value=""" &RS("COD3") &""">" &RS("COD3") &"</option>"
      %>
       <select size="1" class="dataListdata" ID="Select5">                                            
        <%=s%></select>                      
        <input type="text" readonly  size="6" value="<%=RS("Sec")%>" maxlength="6"  class="dataListdata" ID="Text29"><font size=2>
        <% aryOption=Array("段")
      s=""
      s="<option value=""" &RS("COD4") &""">" &RS("COD4") &"</option>"
      %>
       <select size="1"  class="dataListdata" ID="Select6">                                            
        <%=s%></select>
        <input type="text" readonly  size="6" value="<%=RS("lane")%>" maxlength="6" class="dataListdata" ID="Text30"><font size=2>
        <% aryOption=Array("巷")
      s=""
      s="<option value=""" &RS("COD5") &""">" &RS("COD5") &"</option>"
      %>
       <select size="1"  class="dataListdata" ID="Select9">                                            
        <%=s%></select>        
        <br>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         <input type="text" readonly size="5" value="<%=RS("RZONE")%>" maxlength="5"  class="dataListDATA" ID="Text35">
        
        <input type="text"  readonly size="10" value="<%=RS("ALLEYWAY")%>" maxlength="6" class="dataListdata" ID="Text31"><font size=2>
                <% aryOption=Array("弄")
      s=""
      s="<option value=""" &RS("COD6") &""">" &RS("COD6") &"</option>"
      %>
      <select size="1" class="dataListdata" ID="Select10">                                            
        <%=s%></select>    
        <input type="text"  readonly size="6" value="<%=RS("num")%>" maxlength="6"  class="dataListdata" ID="Text32"><font size=2>
                <% aryOption=Array("號")
      s=""
      s="<option value=""" &RS("COD7") &""">" &RS("COD7") &"</option>"
      %>
       <select size="1"  class="dataListdata" ID="Select11">                                            
        <%=s%></select>            
        <input type="text" readonly name="key30" size="10" value="<%=RS("floor")%>" maxlength="6"  class="dataListdata" ID="Text33"><font size=2>
                <% aryOption=Array("樓")
      s=""
      s="<option value=""" &RS("COD8") &""">" &RS("COD8") &"</option>"
      %>
       <select size="1"  class="dataListdata" ID="Select12">                                            
        <%=s%></select>
        <input type="text"  readonly size="6" value="<%=RS("room")%>" maxlength="6"  class="dataListdata" ID="Text34"><font size=2>
                <% aryOption=Array("室")
      s=""
      s="<option value=""" &RS("COD9") &""">" &RS("COD9") &"</option>"
      %>
       <select size="1"  class="dataListdata" ID="Select13">                                            
        <%=s%></select><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <font size=2>設備位置</font>       
 <input type="text" readonly size="30" value="<%=rs("ADDROTHER")%>" maxlength="30"  class="dataListdata" ID="Text21">             
        </td>                                 
</tr>  
<tr><td class=dataListHEAD >電信室/箱位置</td>
<td bgcolor="silver" COLSPAN=3>
 <input type="text"  readonly size="90" value="<%=rs("TELCOMROOM")%>" maxlength="60"  class="dataListdata" ID="Text68">             
</tr>
<tr><td class=dataListHEAD >可供裝範圍</td>
<td bgcolor="silver" COLSPAN=3>
<input type="text" readonly size="90" value="<%=rs("SUPPLYRANGE")%>" maxlength="90" class="dataListdata" ID="Text62">
</tr>
<tr><td class=dataListHEAD rowspan=2>戶籍地址<br>/營業地址</td>
    <td bgcolor="silver" COLSPAN=3><font size=2>中文︰</font>
  <%s=""
    sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & RS("CUTID1") & "' " 
    sx=""    
    rsXX.Open sql,conn
    IF NOT RSXX.EOF THEN
       CUTNC=RSXX("CUTNC")
    ELSE
       CUTNC=""
    END IF
    RSXX.CLOSE
   %>
         <input type="text"  readonly  size="6" value="<%=CUTNC%>" maxlength="10" class="dataListDATA" ID="Text47" NAME="Text38">
        <input type="text"  readonly size="8" value="<%=rs("TOWNSHIP1")%>" maxlength="10"  class="dataListDATA" ID="Text10"><font size=2>(鄉鎮)                 
        <input type="text"  readonly size="40" value="<%=rs("RADDR1")%>" maxlength="60" class="dataListDATA" ID="Text11"><font size=2>郵遞區號</font>  
        <input type="text"   READONLY size="5"   value="<%=rs("rzone1")%>"  class="dataListDATA" ID="Text15">               
        </td>                                            
    </TR>
    <TR>
    <td bgcolor="silver" COLSPAN=3><font size=2>英文︰</font>
        <input type="text"  readonly size="60" value="<%=rs("ENGADDR")%>" maxlength="60" class="dataListDATA" ID="Text45"><font size=2>        
      </td>                                         
</tr>  
<tr><td class=dataListHEAD>ADSL帳寄地址</td>
    <td bgcolor="silver" COLSPAN=3><font size=2>中文︰</font>
  <%s=""
    sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & RS("CUTID2") & "' " 
    sx=""    
    rsXX.Open sql,conn
    IF NOT RSXX.EOF THEN
       CUTNC=RSXX("CUTNC")
    ELSE
       CUTNC=""
    END IF
    RSXX.CLOSE
   %>
         <input type="text"  readonly  size="6" value="<%=CUTNC%>" maxlength="10" class="dataListDATA" ID="Text12" NAME="Text38">
        <input type="text"  readonly size="8" value="<%=rs("TOWNSHIP2")%>" maxlength="10"  class="dataListDATA" ID="Text13" NAME="Text13"><font size=2>(鄉鎮)                 
        <input type="text"  readonly size="40" value="<%=rs("RADDR2")%>" maxlength="60" class="dataListDATA" ID="Text14" NAME="Text14"><font size=2>郵遞區號</font>  
        <input type="text"   READONLY size="5"   value="<%=rs("rzone2")%>"  class="dataListDATA" ID="Text48" NAME="Text48">               
 </td>          
</tr>        
<tr>                                 
        <td  class="dataListHEAD" height="23">連絡人姓名</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" READONLY size="15" value="<%=rs("contact1")%>"  class="dataListdata" ID="Text8"></td>        
        <input type="text" READONLY STYLE="DISPLAY:NONE" size="15" value="<%=rs("contact2")%>" class="dataListdata" ID="Text36"></td>           
        <td  class="dataListHEAD" height="23">連絡電話(白天)</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" readonly size="15" maxlength="15" value="<%=rs("CONTACTTEL")%>"  class="dataListdata" ID="Text7"></td>                                 
 
 </tr>        
<TR>        
        <td  class="dataListHEAD" height="23">行動電話</td>                                 
        <td  height="23" bgcolor="silver">
        <input type="text" readonly size="20" maxlength="20" value="<%=rs("CONTACTMOBILE")%>" class="dataListdata" ID="Text9"></td>                                 
        <td  class="dataListHEAD" height="23">連絡EMAIL</td>        
        <td  height="23" bgcolor="silver" >
        <input type="text"  readonly size="30" maxlength="30" value="<%=rs("CONTACTEMAIL")%>"  class="dataListdata" ID="Text16"></td>                                 
 </tr>
 <tr>                               
        <td  rowspan=2 class="dataListHEAD" height="23">技術連絡人</td>                                 
        <td  height="23" bgcolor="silver"><font size=2>中文︰</font>
        <input type="text" readonly size="15" value="<%=rs("TECHCONTACT")%>"   class="dataListdata" ID="Text1"></td>                                 
        <td  rowspan=2 class="dataListHEAD" height="23">方便聯絡時間</td>                                 
        <td  height="23" bgcolor="silver">
<%     If RS("CONTACTTIME1")="01" Then sexd1=" checked "    
       If RS("CONTACTTIME1")="02" Then sexd2=" checked " %>            
        <input type="RADIO" <%=sexd1%>  size="1"  value="01"  disabled  class="dataListdata" ID="Text2">週一~週五
        <input type="RADIO" <%=sexd2%>  size="1"  value="02"   disabled   class="dataListdata" ID="Checkbox1">週六~週日</td>                                 
 </tr><tr>        
        <td  height="23" bgcolor="silver"><font size=2>英文︰</font>
        <input type="text" readonly size="15" value="<%=rs("TECHENGNAME")%>"  class="dataListdata" ID="Text3"></td>           
        <td  height="23" bgcolor="silver">
<%   If RS("CONTACTTIME2")="01" Then sexd3=" checked "    
     If RS("CONTACTTIME2")="02" Then sexd4=" checked " %>         
        <input type="text" readonly size="2" value="<%=rs("CONTACTSTRTIME")%>"  class="dataListdata" ID="Text49">點至
        <input type="text" readonly size="2" value="<%=rs("CONTACTENDTIME")%>"   class="dataListdata" ID="Text50">點(
        <input type="RADIO" <%=sexd3%>   size="1" value="01"  disabled   class="dataListdata" ID="Checkbox2">上午
        <input type="RADIO" <%=sexd4%>   size="1" value="02"  disabled   class="dataListdata" ID="Checkbox3">下午                
        </td>                     
 </tr> 

<tr>   <td  WIDTH="15%" rowspan=2 class="dataListHEAD" height="23">新租服務</td>               
        <td  WIDTH="35%" height="23" bgcolor="silver" >
<%
    sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D3' AND CODE='" & rs("linerate") & "'"
    rsxx.Open sql,conn
    IF NOT RSXX.EOF THEN
       linerate=RSXX("codenc")
    ELSE
       linerate=""
    END IF
    RSXX.CLOSE%>         
    <input type="text" readonly  size="15" maxlength="15" value="<%=linerate%>" class="dataListdata" ID="Text51">
        </td>
        <td  WIDTH="15%" class="dataListSEARCH" height="23">附掛電話</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text"  readonly size="15" maxlength="15" value="<%=rs("LINETEL")%>"  class="dataListdata" ID="Text38"></td>                                 
<%rs.close%>                              
 </tr>               
</table> </div>
<!--
    <table border="1" width="100%" cellpadding="0" cellspacing="0" id="tag2" style="display: none"> 
    -->

      <DIV ID="SRTAG1" onclick="srtag1" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table2">
    <tr><td bgcolor="BDB76B" align="LEFT">異動主線資料</td></tr></table></DIV>
    <DIV ID=SRTAB1 >
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table3">
<tr><td width="15%" class=dataListHEAD>異動申請日</td>
    <td width="35%" bgcolor="silver" COLSPAN=3>
        <input type="text" name="key3" <%=fieldpB%><%=fieldRole(1)%><%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(3)%>"  READONLY size="10" class=dataListEntry ID="Text52">
       <input  type="button" id="B3" name="B3" height="100%" <%=fieldpC%> width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  alt="清除" id="C3"  <%=fieldpC%> name="C3"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
                             </td>
</tr>
<tr><td width="15%"  class=dataListHEAD>異動項目</td>
    <td width="35%" bgcolor="silver" COLSPAN=3>
<%   IF DSPKEY(6)=1 THEN CHECK6=" CHECKED "%>
<INPUT type="checkbox" name="key6" value=1 <%=CHECK6%>  <%=fieldRole(1)%> bgcolor="silver" ID="key6"  onclick="sroptclick" ><font size=2>512K/64K轉1536K/384K</font>
<%   IF DSPKEY(7)=1 THEN CHECK7=" CHECKED "%>
<INPUT type="checkbox" name="key7" value=1 <%=CHECK7%>  <%=fieldRole(1)%> bgcolor="silver" ID="key7"  onclick="sroptclick" ><font size=2>1536K/384K轉512K/64K</font>
<%   IF DSPKEY(8)=1 THEN CHECK8=" CHECKED "%>
<INPUT type="checkbox" name="key8" value=1 <%=CHECK8%>   <%=fieldRole(1)%> bgcolor="silver" ID="key8" onclick="srkey8click" ><font size=2>移機</font>
 <input  type="button" id="SHD" name="SHD"  height="100%" width="100%" style="Z-INDEX: 1" value="顯示明細" onclick="SrSHOWDETAILOnClick">
</td>
</tr>
<tr><td width="15%" class=dataListHEAD>新社區主線序號</td>
    <td width="35%" bgcolor="silver" COLSPAN=3>
    <%IF LEN(DSPKEY(9))=0 OR DSPKEY(9) ="" THEN DSPKEY(9)=0
   IF LEN(DSPKEY(10))=0 OR DSPKEY(10) ="" THEN DSPKEY(10)=0
    %>
     <input type="text" name="key9" size="4"  value="<%=dspKey(9)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListentry" ID="Text98"><font size=2>－</font>
     <input type="text" name="key10" size="4"  value="<%=dspKey(10)%>"  <%=fieldpa%><%=fieldRole(1)%> class="dataListentry" ID="Text99">
     <input type="hidden" name="key9x" size="4"  value="<%=KEY9X%>" readonly  class="dataListdata" ID="Text122"><font size=2>－</font>
     <input type="hidden" name="key10x" size="4"  value="<%=KEY10X%>"   readonly  class="dataListdata" ID="Text123">
     
     <input  type="button" id="B10" name="B10"  height="100%" width="100%" style="Z-INDEX: 1" value="主線檢查" onclick="SrGETCOMLINEOnClick">
    <IMG  SRC="/WEBAP/IMAGE/IMGDELETE.GIF" <%=fieldpC%> alt="清除" id="C10"  name="C10"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
    <font size=2>訊息︰</font><input  type="type" id="errmsg" name="errmsg" size="80" value="<%=errmsg%>" readonly class=dataListdata>

</tr>
<tr id=SRTAR1 STYLE="display:none"><td colspan=4>
<table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table4">
<tr><td  class=dataListHEAD2>新社區大樓名稱</td>
    <td  bgcolor="Honeydew" COLSPAN=3>
<% IF LEN(TRIM(DSPKEY(9)))=0 OR DSPKEY(9) < 1 THEN DSPKEY(9)=0
   sql="SELECT * FROM RTebtcmtyH where comq1=" & DSPkey(9) 
   rsXX.open sql,conn
   if not rsXX.eof then
      COMN=RSXX("COMN")
   else
      COMN=""
   end if
   RSXX.CLOSE
%>          
     <input type="text" style="text-align:left;" NAME="REF0" maxlength="30"
               value="<%=COMN%>"  readonly size="30" class=dataListdata2 ID="Text101" >
    </td>
</tr>
<tr><td rowspan=2 class=dataListHEAD2>公司負責人<br>(個人申請免填)</td>
    <td  bgcolor="Honeydew" ><font size=2>中文︰</font>
<% IF LEN(TRIM(DSPKEY(9)))=0 OR DSPKEY(9) < 1 THEN DSPKEY(9)=0
   IF LEN(TRIM(DSPKEY(10)))=0 OR DSPKEY(10) < 1 THEN DSPKEY(10)=0
   sql="SELECT * FROM RTebtcmtyline where comq1=" & DSPkey(9) & " and lineq1=" & DSPKEY(10) 
   rs.open sql,conn
   if not rs.eof then
      coboss=rs("COBOSS")
   else
      coboss=""
   end if
%>      
        <input type="text" style="text-align:left;" maxlength="10"  NAME="REF1"
               value="<%=coboss%>"  readonly size="10" class=dataListdata2 ID="Text55" ></td>
<td width="15%" class=dataListHEAD2>申請人統一編號</td>
    <td width="35%" bgcolor="Honeydew" >
<% if not rs.eof then
      coid=rs("COid")
   else
      coid=""
   end if
%>     
        <input type="text"  style="text-align:left;" maxlength="10"
               value="<%=coid%>"  NAME="REF2" readonly  size="10" class=dataListDATA2 ID="Text56" ></td>               
</tr>
<tr>
    <td  bgcolor="Honeydew" ><font size=2>英文︰</font>
<% if not rs.eof then
      COBOSSENG=rs("COBOSSENG")
   else
      COBOSSENG=""
   end if
%>
 <input type="text" style="text-align:left;" maxlength="30"
               value="<%=cobosseng%>"  NAME="REF3"  readonly  size="30" class=dataListDATA2 ID="Text57" ></td>
<td  class=dataListHEAD2>負責人身份證字號</td>
    <td  bgcolor="Honeydew" >
<% if not rs.eof then
      COBOSSID=rs("COBOSSID")
   else
      COBOSSID=""
   end if
%>
        <input type="password"  style="text-align:left;" maxlength="10"
               value="<%=COBOSSID%>"   NAME="REF4" readonly  size="10" class=dataListDATA2 ID="Text58" ></td>               
</tr>
<tr><td rowspan=2 class=dataListHEAD2>申請人姓名<br>/公司名稱</td>
    <td  bgcolor="Honeydew" colspan=3><font size=2>中文︰</font>
<% if not rs.eof then
      APPLYNAMEC=rs("APPLYNAMEC")
   else
      APPLYNAMEC=""
   end if %>    
       <input type="text" style="text-align:left;" maxlength="30"
               value="<%=APPLYNAMEC%>"   NAME="REF5" readonly  size="30" class=dataListDATA2 ID="Text59" ></td>
</tr>
<tr>
    <td  bgcolor="Honeydew" colspan=3><font size=2>英文︰</font>
<% if not rs.eof then
      APPLYNAMEe=rs("APPLYNAMEe")
   else
      APPLYNAMEe=""
   end if %>    
           <input type="text" style="text-align:left;" maxlength="50"
               value="<%=APPLYNAMEe%>"   NAME="REF6" readonly  size="50" class=dataListDATA2 ID="Text60" ></td>
</tr>
<tr><td class=dataListHEAD2>ADSL裝機地址</td>
    <td bgcolor="Honeydew" COLSPAN=3><font size=2>中文︰</font>
  <%IF NOT RS.EOF THEN
       s=""
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & RS("CUTID") & "' " 
       sx=""    
       rsXX.Open sql,conn
       IF NOT RSXX.EOF THEN
          CUTNC=RSXX("CUTNC")
       ELSE
          CUTNC=""
       END IF
       RSXX.CLOSE
    ELSE
       CUTNC=""
    END IF
   %>
        <input type="text"  readonly  size="6"  NAME="REF7" value="<%=CUTNC%>" maxlength="10" readonly  class="dataListDATA2" ID="Text61" >
<% if not rs.eof then
      TOWNSHIP=rs("TOWNSHIP")
   else
      TOWNSHIP=""
   end if %>            
        <input type="text"  readonly  size="8"  NAME="REF8" value="<%=TOWNSHIP%>" maxlength="10" readonly class="dataListDATA2" ID="Text63"><font size=2>(鄉鎮)                 
 <% if not rs.eof then
       VILLAGE=rs("VILLAGE")
   else
       VILLAGE=""
   end if %>    
   <input type="text"  readonly  size="10"  NAME="REF9" value="<%=VILLAGE%>" maxlength="10"class="dataListdata2" ID="Text64" ><font size=2>
<%IF NOT RS.EOF THEN      
      COD1=RS("COD1")
  ELSE
      COD1=""
  END IF
        %>         
       <input type="text"  readonly  size="2"  NAME="REF10" value="<%=COD1%>" maxlength="10"class="dataListdata2" ID="Text102" ><font size=2>
<% if not rs.eof then
      NEIGHBOR=rs("NEIGHBOR")
   else
      NEIGHBOR=""
   end if %>             
        <input type="text" readonly size="6"  NAME="REF11" value="<%=NEIGHBOR%>" maxlength="6"  class="dataListdata2" ID="Text65" ><font size=2>
        <% 
 IF NOT RS.EOF THEN              
      COD2=RS("COD2")
 elsE
      COD2=""
 end if
      %>
       <input type="text"  readonly  size="2"  NAME="REF12" value="<%=COD2%>" maxlength="10"class="dataListdata2" ID="Text103" ><font size=2>
<% if not rs.eof then
      STREET=rs("STREET")
   else
      STREET=""
   end if %>                      
        <input type="text" readonly size="10"  NAME="REF13" value="<%=STREET%>" maxlength="10" class="dataListdata2" ID="Text66" ><font size=2>
        <% 
   IF NOT RS.EOF THEN   
      COD3=RS("COD3")
   ELSE
      COD3=""
   END IF
      %>
       <input type="text"  readonly  size="2"  NAME="REF14" value="<%=COD3%>" maxlength="10"class="dataListdata2" ID="Text104" ><font size=2>
<% if not rs.eof then
      SEC=rs("SEC")
   else
      SEC=""
   end if %>                                   
        <input type="text" readonly  size="6"  NAME="REF15" value="<%=Sec%>" maxlength="6"  class="dataListdata2" ID="Text67" ><font size=2>
        <% 
   IF NOT RS.EOF THEN   
      COD4=RS("COD4")
   ELSE
      COD4=""
   END IF
      %>
       <input type="text"  readonly  size="2"  NAME="REF16" value="<%=COD4%>" maxlength="10"class="dataListdata2" ID="Text105" ><font size=2>
<% if not rs.eof then
      LANE=rs("LANE")
   else
      LANE=""
   end if %>          
        <input type="text" readonly  size="6"  NAME="REF17" value="<%=lane%>" maxlength="6" class="dataListdata2" ID="Text69" ><font size=2>
        <% 
   IF NOT RS.EOF THEN   
      COD5=RS("COD5")
   ELSE
      COD5=""
   END IF
      %>
        <input type="text" readonly  size="6"  NAME="REF18" value="<%=COD5%>" maxlength="6" class="dataListdata2" ID="Text106" ><font size=2>
        <br>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<% if not rs.eof then
      RZONE=rs("RZONE")
   else
      RZONE=""
   end if %>                 
         <input type="text" readonly size="5"  NAME="REF19" value="<%=RZONE%>" maxlength="5"  class="dataListDATA2" ID="Text70" >
<% if not rs.eof then
      ALLEYWAY=rs("ALLEYWAY")
   else
      ALLEYWAY=""
   end if %>               
        <input type="text"  readonly size="10"  NAME="REF20" value="<%=ALLEYWAY%>" maxlength="6" class="dataListdata2" ID="Text71" ><font size=2>
                <% 
   IF NOT RS.EOF THEN   
      COD7=RS("COD7")
   ELSE
      COD7=""
   END IF
      %>
        <input type="text" readonly  size="6"  NAME="REF21" value="<%=COD7%>" maxlength="6" class="dataListdata2" ID="Text107" ><font size=2>
<% if not rs.eof then
      NUM=rs("NUM")
   else
      NUM=""
   end if %>              
        <input type="text"  readonly size="6"  NAME="REF22" value="<%=num%>" maxlength="6"  class="dataListdata2" ID="Text72" ><font size=2>
                <% 
   IF NOT RS.EOF THEN   
      COD8=RS("COD8")
   ELSE
      COD8=""
   END IF
      %>
        <input type="text" readonly  size="6"  NAME="REF23" value="<%=COD8%>" maxlength="6" class="dataListdata2" ID="Text108" ><font size=2>
<% if not rs.eof then
      floor=rs("floor")
   else
      floor=""
   end if %>                     
        <input type="text" readonly  NAME="REF24" size="10" value="<%=floor%>" maxlength="6"  class="dataListdata2" ID="Text73"><font size=2>
                <% 
   IF NOT RS.EOF THEN   
      COD9=RS("COD9")
   ELSE
      COD9=""
   END IF
      %>
        <input type="text" readonly  size="6"  NAME="REF25" value="<%=COD9%>" maxlength="6" class="dataListdata2" ID="Text109" ><font size=2>
<% if not rs.eof then
      room=rs("room")
   else
      room=""
   end if %>            
        <input type="text"  readonly size="6"  NAME="REF26" value="<%=room%>" maxlength="6"  class="dataListdata2" ID="Text74" ><font size=2>
                <%
   IF NOT RS.EOF THEN   
      COD10=RS("COD10")
   ELSE
      COD10=""
   END IF
      %>
        <input type="text" readonly  size="6"  NAME="REF27" value="<%=COD10%>" maxlength="6" class="dataListdata2" ID="Text110" ><font size=2>
<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <font size=2>設備位置</font>   
<% if not rs.eof then
      ADDROTHER=rs("ADDROTHER")
   else
      ADDROTHER=""
   end if %>                      
 <input type="text" readonly  NAME="REF28" size="30" value="<%=ADDROTHER%>" maxlength="30"  class="dataListdata2" ID="Text75" >             
        </td>                                 
</tr>  
<tr><td class=dataListHEAD2 >電信室/箱位置</td>
<td bgcolor="Honeydew" COLSPAN=3>
<% if not rs.eof then
      TELCOMROOM=rs("TELCOMROOM")
   else
      TELCOMROOM=""
   end if %>         
 <input type="text"  readonly  NAME="REF29" size="90" value="<%=TELCOMROOM%>" maxlength="60"  class="dataListdata2" ID="Text76" >             
</tr>
<tr><td class=dataListHEAD2 >可供裝範圍</td>
<td bgcolor="Honeydew" COLSPAN=3>
<% if not rs.eof then
     SUPPLYRANGE=rs("SUPPLYRANGE")
   else
     SUPPLYRANGE=""
   end if %>     
<input type="text" readonly size="90" value="<%=SUPPLYRANGE%>" maxlength="90" class="dataListdata2" ID="Text77"  NAME="REF30" >
</tr>
<tr><td class=dataListHEAD2 rowspan=2>戶籍地址<br>/營業地址</td>
    <td bgcolor="Honeydew" COLSPAN=3><font size=2>中文︰</font>
  <%
  if not rs.eof then
    s=""
    sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & RS("CUTID1") & "' " 
    sx=""    
    rsXX.Open sql,conn
    IF NOT RSXX.EOF THEN
       CUTNC=RSXX("CUTNC")
    ELSE
       CUTNC=""
    END IF
    RSXX.CLOSE
   else
    CUTNC=""
   end if
   %>
         <input type="text"  readonly  size="6" value="<%=CUTNC%>" maxlength="10" class="dataListDATA2" ID="Text78"  NAME="REF31" >
<% if not rs.eof then
     TOWNSHIP1=rs("TOWNSHIP1")
   else
     TOWNSHIP1=""
   end if %>             
        <input type="text"  readonly size="8" value="<%=TOWNSHIP1%>" maxlength="10"  class="dataListDATA2" ID="Text79"  NAME="REF32" ><font size=2>(鄉鎮)                 
<% if not rs.eof then
     RADDR1=rs("RADDR1")
   else
     RADDR1=""
   end if %>                
        <input type="text"  readonly size="40" value="<%=RADDR1%>" maxlength="60" class="dataListDATA2" ID="Text80"  NAME="REF33" ><font size=2>郵遞區號</font>  
<% if not rs.eof then
     rzone1=rs("rzone1")
   else
     rzone1=""
   end if %>                
        <input type="text"   READONLY size="5"   value="<%=rzone1%>"  class="dataListDATA2" ID="Text81"  NAME="REF34" >               
        </td>                                            
    </TR>
    <TR>
    <td bgcolor="Honeydew" COLSPAN=3><font size=2>英文︰</font>
<% if not rs.eof then
     ENGADDR=rs("ENGADDR")
   else
     ENGADDR=""
   end if %>           
        <input type="text"  readonly size="60" value="<%=ENGADDR%>" maxlength="60" class="dataListDATA2" ID="Text82"  NAME="REF35" ><font size=2>        
      </td>                                         
</tr>  
<tr><td class=dataListHEAD2>ADSL帳寄地址</td>
    <td bgcolor="Honeydew" COLSPAN=3><font size=2>中文︰</font>
  <%
   if not rs.eof then
    s=""
    sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & RS("CUTID2") & "' " 
    sx=""    
    rsXX.Open sql,conn
    IF NOT RSXX.EOF THEN
       CUTNC=RSXX("CUTNC")
    ELSE
       CUTNC=""
    END IF
    RSXX.CLOSE
   else
    cutnc=""
   end if
   %>
         <input type="text"  readonly  size="6" value="<%=CUTNC%>" maxlength="10" class="dataListDATA2" ID="Text83"  NAME="REF36" >
<% if not rs.eof then
     TOWNSHIP2=rs("TOWNSHIP2")
   else
     TOWNSHIP2=""
   end if %>                
        <input type="text"  readonly size="8" value="<%=TOWNSHIP2%>" maxlength="10"  class="dataListDATA2" ID="Text84"  NAME="REF37" ><font size=2>(鄉鎮)                 
<% if not rs.eof then
     RADDR2=rs("RADDR2")
   else
     RADDR2=""
   end if %>         
        <input type="text"  readonly size="40" value="<%=RADDR2%>" maxlength="60" class="dataListDATA2" ID="Text85"  NAME="REF38" ><font size=2>郵遞區號</font>  
<% if not rs.eof then
     rzone2=rs("rzone2")
   else
     rzone2=""
   end if %>        
        <input type="text"   READONLY size="5"   value="<%=rzone2%>"  class="dataListDATA2" ID="Text86"  NAME="REF39" >               
 </td>          
</tr>        
<tr>                                 
        <td  class="dataListHEAD2" height="23">連絡人姓名</td>                                 
        <td  height="23" bgcolor="Honeydew">
<% if not rs.eof then
     contact1=rs("contact1")
   else
     contact1=""
   end if %>         
        <input type="text" READONLY size="15" value="<%=contact1%>"  class="dataListdata2" ID="Text87"  NAME="REF40" ></td>        
<% if not rs.eof then
     contact2=rs("contact2")
   else
     contact2=""
   end if %>         
        <input type="text" READONLY STYLE="DISPLAY:NONE" size="15" value="<%=contact2%>" class="dataListdata2" ID="Text88"  NAME="REF41" ></td>           
        <td  class="dataListHEAD2" height="23">連絡電話(白天)</td>                                 
        <td  height="23" bgcolor="Honeydew">
<% if not rs.eof then
     CONTACTTEL=rs("CONTACTTEL")
   else
     CONTACTTEL=""
   end if %>             
        <input type="text" readonly size="15" maxlength="15" value="<%=CONTACTTEL%>"  class="dataListdata2" ID="Text89"  NAME="REF42" ></td>                                 
 
 </tr>        
<TR>        
        <td  class="dataListHEAD2" height="23">行動電話</td>                                 
        <td  height="23" bgcolor="Honeydew">
<% if not rs.eof then
     CONTACTMOBILE=rs("CONTACTMOBILE")
   else
     CONTACTMOBILE=""
   end if %>               
        <input type="text" readonly size="20" maxlength="20" value="<%=CONTACTMOBILE%>" class="dataListdata2" ID="Text90"  NAME="REF43" ></td>                                 
        <td  class="dataListHEAD2" height="23">連絡EMAIL</td>        
        <td  height="23" bgcolor="Honeydew" >
<% if not rs.eof then
     CONTACTEMAIL=rs("CONTACTEMAIL")
   else
     CONTACTEMAIL=""
   end if %>              
        <input type="text"  readonly size="30" maxlength="30" value="<%=CONTACTEMAIL%>"  class="dataListdata2" ID="Text91"  NAME="REF44" ></td>                                 
 </tr>
 <tr>                               
        <td  rowspan=2 class="dataListHEAD2" height="23">技術連絡人</td>                                 
        <td  height="23" bgcolor="Honeydew"><font size=2>中文︰</font>
<% if not rs.eof then
     TECHCONTACT=rs("TECHCONTACT")
   else
     TECHCONTACT=""
   end if %>           
        <input type="text" readonly size="15" value="<%=TECHCONTACT%>"   class="dataListdata2" ID="Text92"  NAME="REF45" ></td>                                 
        <td  rowspan=2 class="dataListHEAD2" height="23">方便聯絡時間</td>                                 
        <td  height="23" bgcolor="Honeydew">
<%  IF NOT RS.EOF THEN
       If RS("CONTACTTIME1")="01" Then sexd1=" checked "    
       If RS("CONTACTTIME1")="02" Then sexd2=" checked "
    END IF %>            
        <input type="RADIO" <%=sexd1%>  size="1"  NAME="REF46X"  value="01" DISABLED   class="dataListdata2" ID="REF46X" >週一~週五
        <input type="RADIO" <%=sexd2%>  size="1"  NAME="REF46Y"  value="02"   DISABLED    class="dataListdata2" ID="REF46Y" >週六~週日</td>                                 
 </tr><tr>        
        <td  height="23" bgcolor="Honeydew"><font size=2>英文︰</font>
<% if not rs.eof then
     TECHENGNAME=rs("TECHENGNAME")
   else
     TECHENGNAME=""
   end if %>                
        <input type="text" readonly size="15" value="<%=TECHENGNAME%>"  class="dataListdata2" ID="Text93"  NAME="REF47" ></td>           
        <td  height="23" bgcolor="Honeydew">
 
<% if not rs.eof then
     CONTACTSTRTIME=rs("CONTACTSTRTIME")
   else
     CONTACTSTRTIME=""
   end if %>       
        <input type="text" readonly size="2" value="<%=CONTACTSTRTIME%>"  class="dataListdata2" ID="Text94"  NAME="REF48" >點至
<% if not rs.eof then
     CONTACTENDTIME=rs("CONTACTENDTIME")
   else
     CONTACTENDTIME=""
   end if %>             
        <input type="text" readonly size="2" value="<%=CONTACTENDTIME%>"   class="dataListdata2" ID="Text95"  NAME="REF49" >點(
<%  IF NOT RS.EOF THEN
       If RS("CONTACTTIME2")="01" Then sexd3=" checked "    
       If RS("CONTACTTIME2")="02" Then sexd4=" checked "
    END IF %>                
        <input type="RADIO" <%=sexd3%>   NAME="REF50X"  size="1" value="01"  DISABLED  class="dataListdata2" ID="REF50X" >上午
        <input type="RADIO" <%=sexd4%>   NAME="REF50Y"  size="1" value="02"   DISABLED  class="dataListdata2" ID="REF50Y" >下午                
        </td>                     
 </tr> 

<tr>   <td  WIDTH="15%" rowspan=2 class="dataListHEAD2" height="23">新租服務</td>               
        <td  WIDTH="35%" height="23" bgcolor="Honeydew" >
<%
if not rs.eof then
    sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='D3' AND CODE='" & rs("linerate") & "'"
    rsxx.Open sql,conn
    IF NOT RSXX.EOF THEN
       linerate=RSXX("codenc")
    ELSE
       linerate=""
    END IF
    RSXX.CLOSE
 else
    linerate=""
 end if%>         
    <input type="text" readonly  size="15" maxlength="15" value="<%=linerate%>" class="dataListdata2" ID="Text96"  NAME="REF51" >
        </td>
        <td  WIDTH="15%" class="dataListHEAD2" height="23">附掛電話</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="Honeydew">
<% if not rs.eof then
     LINETEL=rs("LINETEL")
   else
     LINETEL=""
   end if %>                    
        <input type="text"  readonly size="15" maxlength="15" value="<%=LINETEL%>"  class="dataListdata2" ID="Text97"  NAME="REF52" ></td>                                 
<%rs.close%>                              
 </tr> 
</table>
</td>
</tr>
<tr>
        <td  width="15%" class="dataListHEAD" height="23">異動單列印日</td>                                 
        <td  width="35%" height="23" bgcolor="SILVER">
         <input type="text" name="key4" size="10" READONLY value="<%=dspKey(4)%>"   class="dataListDATA" ID="Text53">
        </td>  
        <td  class="dataListHEAD" height="23">列印人員</td>  
        <td  width="35%" height="23" bgcolor="SILVER">
        <%  name="" 
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
  %>    <input type="text" name="key5" size="6" READONLY value="<%=dspKey(5)%>" class="dataListDATA" ID="Text54"><font size=2><%=name%></font>
        </td>                                         
 </tr>  
<tr>
        <td  width="15%" class="dataListHEAD" height="23">建檔人員</td>                                 
        <td  width="35%" height="23" bgcolor="SILVER">
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
  %>    <input type="text" name="key16" size="6" READONLY value="<%=dspKey(16)%>"   class="dataListDATA" ID="Text39"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">建檔日期</td>                                 
        <td  height="23" bgcolor="SILVER">
        <input type="text" name="key17" size="10" READONLY value="<%=dspKey(17)%>"   class="dataListDATA" ID="Text40">
        </td>       
 </tr>  
<tr>
        <td  class="dataListHEAD" height="23">修改人員</td>                                 
        <td  height="23" bgcolor="SILVER">
        <%  name="" 
           if dspkey(18) <> "" then
              sql=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
                   &"where rtemployee.emply='" & dspkey(18) & "' "
              rs.Open sql,conn
              if rs.eof then
                 name=""
              else
                 name=rs("cusnc")
              end if
              rs.close
           end if
  %>    <input type="text" name="key18" size="6" READONLY value="<%=dspKey(18)%>"  class="dataListDATA" ID="Text41"><font size=2><%=name%></font>
        </td>  
        <td  class="dataListHEAD" height="23">修改日期</td>                                 
        <td  height="23" bgcolor="SILVER">
        <input type="text" name="key19" size="10" READONLY value="<%=dspKey(19)%>"   class="dataListDATA" ID="Text42">
        </td>       
 </tr>           
  </table>     
  </DIV>

      <DIV ID="SRTAG2" onclick="srtag2" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr><td bgcolor="BDB76B" align="LEFT">主線異動進度狀態</td></tr></table></DIV>
    <DIV ID=SRTAB2 >  
    <table border="1" width="100%" cellpadding="0" cellspacing="0" >
    <tr>
        <td  WIDTH="15%" class="dataListHEAD" height="23">異動申請審核日</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key11" size="10"  READONLY  value="<%=dspKey(11)%>"  <%=fieldpB%><%=fieldRole(1)%> class="dataListEntry" ID="Text44">     
        <input type="button" id="B11"   name="B11" height="100%" <%=fieldpC%> width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"   alt="清除"  <%=fieldpD%> id="C11"  name="C11"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">  </td>         
        <td  WIDTH="15%" class="dataListHEAD" height="23">異動申請轉檔日</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key12" size="10"  READONLY  value="<%=dspKey(12)%>"   class="dataListdata" ID="Text111">     
         </td></tr>
    <tr>
        <td   class="dataListHEAD" height="23">異動申請轉檔批號</td>                                 
        <td   height="23" bgcolor="silver" colspan=3>
        <input type="text" name="key13" size="15" READONLY MAXLENGTH=15 value="<%=dspKey(13)%>"  class="dataListdata" ID="Text46"></td>        
    </tr>
    <tr>
        <td  WIDTH="15%" class="dataListHEAD" height="23">EBT申請回覆日</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key14" size="10"  READONLY  value="<%=dspKey(14)%>"   class="dataListDATA" ID="Text112">     
        </td>         
        <td  WIDTH="15%" class="dataListHEAD" height="23">EBT申請回覆狀態</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key15" size="10"  READONLY  value="<%=dspKey(15)%>"   class="dataListdata" ID="Text113">     
         </td></tr>
    <tr>
        <td  WIDTH="15%" class="dataListHEAD" height="23">移機完工日</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key21" size="10"  READONLY  value="<%=dspKey(21)%>"  class="dataListDATA" ID="Text114">     
         </td>         
        <td  WIDTH="15%" class="dataListHEAD" height="23">移機報竣審核日</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key22" size="10"  READONLY <%=fieldpG%> value="<%=dspKey(22)%>"   class="dataListENTRY" ID="Text115">  
        <input type="button" id="B22"   name="B22" height="100%"  <%=fieldpE%>  width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"> 
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"   alt="清除" id="C22" <%=fieldpF%>  name="C22"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">             
         </td></tr>
    <tr>
        <td  WIDTH="15%" class="dataListHEAD" height="23">移機報竣轉檔日</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key23" size="10"  READONLY  value="<%=dspKey(23)%>"  class="dataListDATA" ID="Text116">     
         </td>         
        <td  WIDTH="15%" class="dataListHEAD" height="23">移機報竣轉檔批號</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key24" size="15"  READONLY  value="<%=dspKey(24)%>"   class="dataListDATA" ID="Text117">  
         </td></tr>
    <tr>
        <td  WIDTH="15%" class="dataListHEAD" height="23">EBT報竣轉檔回覆日</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key25" size="10"  READONLY  value="<%=dspKey(25)%>"   class="dataListDATA" ID="Text118">     
        </td>         
        <td  WIDTH="15%" class="dataListHEAD" height="23">EBT報竣轉檔回覆狀態</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key26" size="10"  READONLY  value="<%=dspKey(26)%>"   class="dataListdata" ID="Text119">     
         </td></tr>
    <tr>
        <td  WIDTH="15%" class="dataListHEAD" height="23">作廢日期</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <input type="text" name="key27" size="10"  READONLY  value="<%=dspKey(27)%>"   class="dataListDATA" ID="Text120">     
        </td>        
        <td  WIDTH="15%" class="dataListHEAD" height="23">作廢人員</td>                                 
        <td  WIDTH="35%" height="23" bgcolor="silver">
        <%  name="" 
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
        <input type="text" name="key28" size="6"  READONLY  value="<%=dspKey(28)%>"   class="dataListDATA" ID="Text121"><font size=2><%=name%></font>     
        </td>                 
    </tr>         
  </table> 
  </DIV>
    <DIV ID="SRTAG3" onclick="srtag3" style="cursor:hand">
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table8">
    <tr><td bgcolor="BDB76B" align="LEFT">備註說明</td></tr></table></DIV>
   <DIV ID="SRTAB3" > 
    <table border="1" width="100%" cellpadding="0" cellspacing="0" ID="Table9">
    <TR><TD align="CENTER">
     <TEXTAREA  cols="100%"  name="key20" rows=8  MAXLENGTH=500  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(20)%>" ID="Textarea1"><%=dspkey(20)%></TEXTAREA>
   </td></tr>
 </table> 
  </DIV>    
<tr>                                   
  </div> 
<%
    conn.Close   
    set rs=Nothing   
    set conn=Nothing 
End Sub 
' --------------------------------------------------------------------------------------------  
%>
<!-- #include file="RTGetUserRight.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
