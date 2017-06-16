<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONNXX
   Set connXX=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   SET RSyy=Server.CreateObject("ADODB.RECORDSET")
   SET RSzz=Server.CreateObject("ADODB.RECORDSET")
   DSN="DSN=RtLib"
   connXX.Open DSN
 '  On Error Resume Next
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   endpgm="1"
   '當主線已產生派工單且該派工單未入廢時或未完工結案、不可清除
    sqlzz="select count(*) as sndcnt FROM RTEBTCMTYLINEsndwork WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " and dropdat is null AND UNCLOSEDAT IS NULL "
    rszz.Open sqlzz,connXX
    if rszz("sndcnt") > 0 then
       endpgm="5"
    else
    sqlxx="select * FROM RTEBTCMTYLINE WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) 
    RSXX.OPEN SQLXX,CONNxx
   '當主線已測通，不可清除申請
    IF LEN(TRIM(RSXX("adslapplyDAT"))) <> 0 THEN
        ENDPGM="3"
    '此主線尚未產生申請列印單號，不須執行清除動作
    elseif LEN(TRIM(RSXX("applyprtno"))) = 0 then
      endpgm="4"
    ELSE
      sqlyy="select max(entryno) as entryno FROM RTEBTCMTYLINEapplylog WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) 
      rsyy.Open sqlyy,connxx
      
      if len(trim(rsyy("entryno"))) > 0 then
         entryno=rsyy("entryno") + 1
      else
         entryno=1
      end if
      rsyy.close
      set rsyy=nothing
      sqlyy="insert into RTEBTCMTYLINEapplylog " _
           &"SELECT   COMQ1, LINEQ1, " & entryno & ",getdate(),'RT','" & v(0) & "',CONSIGNEE, AREAID, GROUPID, SALESID, LINEIP, GATEWAY, " _
           &"SUBNET, DNSIP, PPPOEACCOUNT, PPPOEPASSWORD, LINETEL, LINERATE, " _
           &"CUTID, TOWNSHIP, VILLAGE, COD1, NEIGHBOR, COD2, STREET, COD3, SEC, " _
           &"COD4, LANE, COD5, TOWN, COD6, ALLEYWAY, COD7, NUM, COD8, FLOOR, " _
           &"COD9, ROOM, COD10, ADDROTHER, RZONE, CUTID1, TOWNSHIP1, RADDR1, " _
           &"RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, RCVDAT, INSPECTDAT, " _
           &"AGREE, UNAGREEREASON, TECHCONTACT, TECHENGNAME, CONTACT1, " _
           &"CONTACT2, CONTACTMOBILE, CONTACTTEL, CONTACTEMAIL, CONTACTTIME1, " _
           &"CONTACTTIME2, UPDEBTCHKDAT, UPDEBTCHKUSR, UPDEBTDAT, " _
           &"EBTREPLYDAT, EBTREPLaYCODE, PROGRESSID, HINETNOTIFYDAT, " _
           &"ADSLAPPLYDAT, APPLYUPLOADDAT, APPLYUPLOADUSR, APPLYUPLOADTNS, " _
           &"EBTERRORCODE, SUPPLYRANGE, COBOSS, COBOSSENG, COID, COBOSSID, " _
           &"APPLYNAMEC, APPLYNAMEE, ENGADDR, CONTACTSTRTIME, " _
           &"CONTACTENDTIME, ADSLAPPLYUSR, APPLYPRTNO, MEMO, APPLYNO, " _
           &"SCHAPPLYDAT, CHTRCVD, SUGGESTTYPE, REPEATREASON,EUSR,EDAT,UUSR,UDAT,TELCOMROOM,DROPDAT,TRANSNOAPPLY,TRANSNODOCKET " _
           &"FROM RTEBTCMTYLINE where comq1=" & key(0) & " and lineq1=" & key(1)
     ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update RTEBTCMTYline set PROGRESSID='RT',LINETEL='',UPDEBTCHKDAT=null,UPDEBTCHKUSR='',UPDEBTDAT=NULL,EBTREPLYDAT=NULL,EBTREPLaYCODE='',HINETNOTIFYDAT=NULL,EBTERRORCODE='',APPLYNO='',SCHAPPLYDAT=NULL,CHTRCVD=NULL,SUGGESTTYPE='',REPEATREASON='',TRANSNOAPPLY='',LINEIP='',GATEWAY='',SUBNET='',DNSIP='',PPPOEACCOUNT='',PPPOEPASSWORD='',applyprtno='' where comq1=" & KEY(0) & " and lineq1=" & key(1) 
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM RTEBTCMTYLINEapplylog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & " AND ENTRYNO=" & ENTRYNO
            CONNXX.Execute sqlyy 
         else
            endpgm="1"
            errmsg=""
         end if      
      end if
   END IF
      RSXX.CLOSE
   end if
   connXX.Close
   SET RSXX=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "申請送件單號已清除成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "CHT已通過主線申請，不可清除主線申請列印單號：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此主線尚未產生申請列印單號，不須執行清除動作：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="5" then
       msgbox "當主線已產生派工單且該派工單未入廢時、不可清除" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                     
    else
       msgbox "無法執行送件單號清除作業,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=rtebtcmtylineclrprtno.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>