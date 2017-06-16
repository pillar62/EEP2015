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
   CANCELDAT=rsxx("CANCELDAT")
   RSXX.CLOSE
   if LEN(TRIM(CANCELDAT)) = 0 OR ISNULL(CANCELDAT) then
     endpgm="3"
   else
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
           &"SELECT  COMQ1, LINEQ1," & ENTRYNO & ", GETDATE(),'R','" & V(0) & "', " _
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
           &"LOANNAME, LOANSOCIAL, LOCKDAT,NULL,'', CANCELDAT, CANCELUSR,MOVETOCOMQ1,MOVETOLINEQ1,MOVETODAT,MOVEFROMCOMQ1,MOVEFROMLINEQ1,MOVEFROMDAT   " _
           &"FROM RTEBTCMTYLINE where comq1=" & key(0) & " and lineq1=" & key(1)  
     ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update RTEBTCMTYLINE set CANCELdat=NULL,CANCELUSR='' where comq1=" & KEY(0) & " and lineq1=" & key(1) 
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
   connXX.Close
   SET RSXX=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "主線資料作廢返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此主線尚未作廢，不可執行作廢返轉" & "  " & frm1.htmlfld1.value
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