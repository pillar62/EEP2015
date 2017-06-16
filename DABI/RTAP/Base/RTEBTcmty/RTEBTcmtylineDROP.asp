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
   DSN="DSN=RtLib"
   connXX.Open DSN
   endpgm="1"
 '  On Error Resume Next

   sqlxx="select * FROM RTEBTCMTYline WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) 
   rsxx.Open sqlxx,connxx
   DROPDAT=rsxx("DROPDAT")
   CANCELDAT=rsxx("CANCELDAT")
   UPDEBTCHKDAT=rsxx("UPDEBTCHKDAT")
   ADSLAPPLYDAT=rsxx("ADSLAPPLYDAT")
   RSXX.CLOSE
   if LEN(TRIM(CANCELDAT)) > 0 then
     endpgm="3"
   ELSEif LEN(TRIM(DROPDAT)) > 0 then
     endpgm="6"
   ELSEif LEN(TRIM(UPDEBTCHKDAT)) > 0 then
     endpgm="7"
   ELSEif LEN(TRIM(ADSLAPPLYDAT)) > 0 then
     endpgm="4"  
   else
     '檢查該主線下的用戶是否皆已退租或作廢
     sqlxx="select COUNT(*) AS CNT FROM RTEBTCUST WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND dropdat is null and canceldat is null "
     RSXX.OPEN SQLXX,CONNxx
     endpgm="1"
     '已作廢
     IF CNT > 0 THEN
      ENDPGM="5"
     ELSE
      sqlyy="select max(ENTRYNO) as ENTRYNO FROM RTEBTCMTYLINElog WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) 
      rsyy.Open sqlyy,connxx
      if len(trim(rsyy("ENTRYNO"))) > 0 then
         ENTRYNO=rsyy("ENTRYNO") + 1
      else
         ENTRYNO=1
      end if
      rsyy.close
      set rsyy=nothing
      sqlyy="insert into RTEBTCMTYLINElog " _
           &"SELECT  COMQ1, LINEQ1," & ENTRYNO & ", GETDATE(),'C','" & V(0) & "', " _
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
           &"LOANNAME, LOANSOCIAL, LOCKDAT,NULL,'', CANCELDAT, CANCELUSR, " _
           &"MOVETOCOMQ1,MOVETOLINEQ1,MOVETODAT,MOVEFROMCOMQ1,MOVEFROMLINEQ1,MOVEFROMDAT, " _
           &"contractno,COTPORT1, COTPORT2, MDF1, MDF2 " _
           &"FROM RTEBTCMTYLINE where comq1=" & key(0) & " and lineq1=" & key(1)  
    ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update RTEBTCMTYLINE set CANCELdat=getdate(),CANCELUSR='" & V(0) & "' where comq1=" & KEY(0) & " and lineq1=" & key(1) 
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM RTEBTCMTYLINElog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & " and entryno=" & ENTRYNO
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
       msgbox "主線資料作廢成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此主線已作廢，不可重覆作廢" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "主線已測通，不可作廢(必須以撤線方式執行)" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="5" then
       msgbox "主線下已有用戶資料(且用戶尚未作廢)，請先作廢用戶資料再重新執行主線作廢" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                  
    elseIF frm1.htmlfld.value="6" then
       msgbox "此主線已撤銷，不可執行作廢" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="7" then
       msgbox "此主線已提出申請，請先清除申請記錄及作廢主機派工資料" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
       
    else
       msgbox "無法執行設備安裝資料作廢,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=rtebtcmtyhardwaredrop.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>