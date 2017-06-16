<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% 
'=========================================================================== 
'920925 
'(1)主線頻寬異動(1.5-->512 or 512 --> 1.5)即chgcod1 or chgcod2 為 "1"時才可直接執行本程式
'(2)若chgcod3="1"時表示為"移機"，則完工結案必須由移機派工單執行。
'===========================================================================
   KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONNXX
   Set connXX=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   DSN="DSN=RtLib"
   connXX.Open DSN
   endpgm="1"
 '  On Error Resume Next
   sqlxx="select * FROM RTEBTCMTYLINECHG WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " and PRTNO='" & key(2) & "' "
   rsxx.Open sqlxx,connxx
   CHGCOD1=RSXX("CHGCOD1") 
   CHGCOD2=RSXX("CHGCOD2")
   CHGCOD3=RSXX("CHGCOD3")
   DROPDAT=rsxx("DROPDAT")
   FINISHDAT=RSXX("FINISHDAT")
   UPDEBTCHKDAT =Datevalue(RSXX("UPDEBTCHKDAT"))
   NCOMQ1=RSXX("NCOMQ1")
   NLINEQ1=RSXX("NLINEQ1")
   RSXX.CLOSE
   if LEN(TRIM(DROPDAT)) >= 1 OR not ISNULL(DROPDAT) then
     endpgm="4"
   ELSEIF CHGCOD3=1 THEN
     ENDPGM="5"
   ELSEIF LEN(TRIM(FINISHDAT)) >= 1 OR NOT ISNULL(FINISHDAT) THEN
     ENDPGM="3"     
   else
     sqlXX="select max(ENTRYNO) as ENTRYNO FROM RTEBTCMTYLINECHGLOG WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " and PRTNO='" & key(2) & "' " 
    ' response.Write sqlxx
     rsXX.Open sqlXX,connxx
     if len(trim(rsXX("ENTRYNO"))) > 0 then
        xENTRYNO=rsXX("ENTRYNO") + 1
     else
        xENTRYNO=1
     end if
     rsXX.close
     sqlXX="insert into RTEBTCMTYLINECHGlog " _
          &"select  COMQ1, LINEQ1, PRTNO," & xENTRYNO & ", GETDATE(),'F','" & V(0) & "', " _
          &"APPLYDAT, PRTDAT, PRTUSR, CHGCOD1, CHGCOD2, CHGCOD3, NCOMQ1, " _
          &"NLINEQ1, UPDEBTCHKDAT, UPDEBTTNSDAT, UPDEBTTNSNO, EBTREPLYDAT, " _
          &"EBTREPLYSTS, EUSR, EDAT, UUSR, UDAT, MEMO, FINISHDAT, DOCKETDAT, " _
          &"TRANSDAT,TRANSNO, EBTREPLYFHDAT, EBTREPLYFHSTS,DROPDAT,DROPUSR " _
          &"FROM RTEBTCMTYLINECHG where comq1=" & key(0) & " and lineq1=" & key(1) & " and PRTNO='" & key(2) & "' "
     CONNXX.Execute sqlXX    
     If Err.number > 0 then
        endpgm="2"
        errmsg=cstr(Err.number) & "=" & Err.description
     else
        SQLXX=" update RTEBTCMTYLINECHG set FINISHDAT=GETDATE() where comq1=" & KEY(0) & " and lineq1=" & key(1) & " and PRTNO='" & key(2) & "' "
        connxx.Execute SQLXX
        If Err.number > 0 then
           endpgm="2"
           '發生錯誤時，刪除異動檔所新增的異動資料
           errmsg=cstr(Err.number) & "=" & Err.description
           sqlXX="delete * FROM RTEBTCMTYLINECHGlog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & " and PRTNO='" & key(2) & "' and entryno=" & xentryno 
           CONNXX.Execute sqlXX
        else
           '更新主線檔線路頻寬
           '先產生LOG檔
           sqlXX="select max(ENTRYNO) as ENTRYNO FROM RTEBTCMTYLINELOG WHERE comq1=" & KEY(0) & " and lineq1=" & key(1)  
           rsXX.Open sqlXX,connxx
           if len(trim(rsXX("ENTRYNO"))) > 0 then
              ENTRYNO=rsXX("ENTRYNO") + 1
           else
              ENTRYNO=1
           end if
           rsXX.close
           IF CHGCOD1=1 THEN
           sqlXX="insert into RTEBTCMTYLINElog " _
                &"select  COMQ1, LINEQ1," & ENTRYNO & ", GETDATE(),'WU','" & V(0) & "', " _
                &"CONSIGNEE, AREAID, GROUPID, SALESID, LINEIP, GATEWAY,  SUBNET, DNSIP, PPPOEACCOUNT, PPPOEPASSWORD, LINETEL, LINERATE, " _
                &"CUTID, TOWNSHIP, VILLAGE, COD1, NEIGHBOR, COD2, STREET, COD3, SEC,  COD4, LANE, COD5, TOWN, COD6, ALLEYWAY, COD7, NUM, COD8, FLOOR, " _
                &"COD9, ROOM, COD10, ADDROTHER, RZONE, CUTID1, TOWNSHIP1, RADDR1, RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, RCVDAT, INSPECTDAT, " _
                &"AGREE, UNAGREEREASON, TECHCONTACT, TECHENGNAME, CONTACT1,CONTACT2, CONTACTMOBILE, CONTACTTEL, CONTACTEMAIL, CONTACTTIME1, " _
                &"CONTACTTIME2, UPDEBTCHKDAT, UPDEBTCHKUSR, UPDEBTDAT,EBTREPLYDAT, EBTREPLAYCODE, PROGRESSID, HINETNOTIFYDAT, " _
                &"ADSLAPPLYDAT, APPLYUPLOADDAT, APPLYUPLOADUSR, APPLYUPLOADTNS, EBTERRORCODE, SUPPLYRANGE, COBOSS, COBOSSENG, COID, COBOSSID, " _
                &"APPLYNAMEC, APPLYNAMEE, ENGADDR, CONTACTSTRTIME, CONTACTENDTIME, ADSLAPPLYUSR, APPLYPRTNO, MEMO, APPLYNO, " _
                &"SCHAPPLYDAT, CHTRCVD, SUGGESTTYPE, REPEATREASON, EUSR, EDAT, UUSR, UDAT, TELCOMROOM, DROPDAT, TRANSNOAPPLY, TRANSNODOCKET, " _
                &"LOANNAME, LOANSOCIAL, LOCKDAT,null,'', CANCELDAT, CANCELUSR, MOVETOCOMQ1, MOVETOLINEQ1, MOVETODAT, MOVEFROMCOMQ1,MOVEFROMLINEQ1, MOVEFROMDAT, CONTRACTNO,COTPORT1,COTPORT2,MDF1,MDF2 " _
                &"FROM RTEBTCMTYLINE where comq1=" & key(0) & " and lineq1=" & key(1) 
            connxx.Execute SQLXX      
           ELSEIF CHGCOD2=1 THEN
           sqlXX="insert into RTEBTCMTYLINElog " _
                &"select  COMQ1, LINEQ1," & ENTRYNO & ", GETDATE(),'WD','" & V(0) & "', " _
                &"CONSIGNEE, AREAID, GROUPID, SALESID, LINEIP, GATEWAY,  SUBNET, DNSIP, PPPOEACCOUNT, PPPOEPASSWORD, LINETEL, LINERATE, " _
                &"CUTID, TOWNSHIP, VILLAGE, COD1, NEIGHBOR, COD2, STREET, COD3, SEC,  COD4, LANE, COD5, TOWN, COD6, ALLEYWAY, COD7, NUM, COD8, FLOOR, " _
                &"COD9, ROOM, COD10, ADDROTHER, RZONE, CUTID1, TOWNSHIP1, RADDR1, RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, RCVDAT, INSPECTDAT, " _
                &"AGREE, UNAGREEREASON, TECHCONTACT, TECHENGNAME, CONTACT1,CONTACT2, CONTACTMOBILE, CONTACTTEL, CONTACTEMAIL, CONTACTTIME1, " _
                &"CONTACTTIME2, UPDEBTCHKDAT, UPDEBTCHKUSR, UPDEBTDAT,EBTREPLYDAT, EBTREPLAYCODE, PROGRESSID, HINETNOTIFYDAT, " _
                &"ADSLAPPLYDAT, APPLYUPLOADDAT, APPLYUPLOADUSR, APPLYUPLOADTNS, EBTERRORCODE, SUPPLYRANGE, COBOSS, COBOSSENG, COID, COBOSSID, " _
                &"APPLYNAMEC, APPLYNAMEE, ENGADDR, CONTACTSTRTIME, CONTACTENDTIME, ADSLAPPLYUSR, APPLYPRTNO, MEMO, APPLYNO, " _
                &"SCHAPPLYDAT, CHTRCVD, SUGGESTTYPE, REPEATREASON, EUSR, EDAT, UUSR, UDAT, TELCOMROOM, DROPDAT, TRANSNOAPPLY, TRANSNODOCKET, " _
                &"LOANNAME, LOANSOCIAL, LOCKDAT,null,'', CANCELDAT, CANCELUSR, MOVETOCOMQ1, MOVETOLINEQ1, MOVETODAT, MOVEFROMCOMQ1,MOVEFROMLINEQ1, MOVEFROMDAT, CONTRACTNO,COTPORT1,COTPORT2,MDF1,MDF2 " _
                &"FROM RTEBTCMTYLINE where comq1=" & key(0) & " and lineq1=" & key(1) 
             connxx.Execute SQLXX     
           END IF
           If Err.number > 0 then
              endpgm="2"
           '發生錯誤時，刪除異動檔所新增的異動資料
              errmsg=cstr(Err.number) & "=" & Err.description
              '發生錯誤時，還原資料
              sqlXX="delete * FROM RTEBTCMTYLINEchglog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & " and PRTNO='" & key(2) & "' and entryno=" & xentryno 
              CONNXX.Execute sqlXX
              SQLXX=" update RTEBTCMTYLINECHG set FINISHDAT=null where comq1=" & KEY(0) & " and lineq1=" & key(1) & " and PRTNO='" & key(2) & "' "
              connxx.Execute SQLXX
           else
              IF CHGCOD1=1 THEN
                 SQLXX=" update RTEBTCMTYLINE set LINERATE='03' where comq1=" & KEY(0) & " and lineq1=" & key(1)
                 connxx.Execute SQLXX 
              ELSEIF CHGCOD2=1 THEN
                 SQLXX=" update RTEBTCMTYLINE set LINERATE='01' where comq1=" & KEY(0) & " and lineq1=" & key(1) 
                 connxx.Execute SQLXX
              ELSEIF CHGCOD3=1 THEN
				 SQLXX=" update RTEBTCMTYLINE set EBTAPPLYOKRTN=isnull(EBTAPPLYOKRTN,GETDATE()), UPDEBTCHKDAT = isnull(UPDEBTCHKDAT,'"& UPDEBTCHKDAT &"') where comq1="& NCOMQ1 &" and lineq1=" & NLINEQ1
				 connxx.Execute SQLXX
              END IF 
              If Err.number > 0 then
                 endpgm="2"
                 '發生錯誤時，刪除異動檔所新增的異動資料
                 errmsg=cstr(Err.number) & "=" & Err.description
                 '發生錯誤時，還原資料
                 sqlXX="delete * FROM RTEBTCMTYLINElog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & " and entryno=" & entryno 
                 CONNXX.Execute sqlXX
                 sqlXX="delete * FROM RTEBTCMTYLINEchglog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & " and PRTNO='" & key(2) & "' and entryno=" & xentryno 
                 CONNXX.Execute sqlXX
                 SQLXX=" update RTEBTCMTYLINECHG set FINISHDAT=null where comq1=" & KEY(0) & " and lineq1=" & key(1) & " and PRTNO='" & key(2) & "' "
                 connxx.Execute SQLXX
              else
                 endpgm="1"
                 errmsg=""
              end if
           END IF
        end if      
      end if
    END IF
   connXX.Close
   SET RSXX=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "主線異動資料完工結案成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此筆資料已結案，不可重複執行完工結案作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此筆資料已作廢，不可執行完工結案作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="5" then
       msgbox "此筆異動包含主線移機，必須由移機派工單執行完工結案作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                   
    else
       msgbox "無法執行主線異動資料完工結案,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=rtebtcmtylinechgf.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>