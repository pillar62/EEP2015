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
 '  On Error Resume Next
   sqlxx="select * FROM RTSparq499Cust WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   endpgm="1"
   '當用戶已完工時，不可直接作廢==>必須執行退租作業
   IF LEN(TRIM(RSXX("finishdat"))) <> 0 THEN
      ENDPGM="3"
   elseif LEN(TRIM(RSXX("CANCELdat"))) <> 0 then
      endpgm="4"
   ELSE
      sqlyy="select max(entryno) as entryno FROM RTSparq499Custlog WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' "
      rsyy.Open sqlyy,connxx
      
      if len(trim(rsyy("entryno"))) > 0 then
         entryno=rsyy("entryno") + 1
      else
         entryno=1
      end if
      rsyy.close
      set rsyy=nothing
      sqlyy="insert into RTSparq499Custlog(COMQ1, LINEQ1, CUSID, entryno, chgdat, chgcode,chgusr, CUSNC, " _
           &"FIRSTIDTYPE, SOCIALID, SECONDIDTYPE,SECONDNO, BIRTHDAY, EMAIL, CONTACTTEL, MOBILE, CUTID1, TOWNSHIP1, " _
           &"RADDR1, RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, CUTID3, TOWNSHIP3, RADDR3, RZONE3, COCONTACT, COCONTACTTEL, " _
           &"COCONTACTTELEXT, COMOBILE, COBOSS, COBOSSSOCIAL, TRADETYPE, EUSR, EDAT, UUSR, UDAT, AREAID, GROUPID, SALESID, " _
           &"CASETYPE, FREECODE, PMCODE, PAYTYPE, AGENTNAME, AGENTSOCIAL, AGENTTEL, RCVD, APPLYDAT, FINISHDAT, DOCKETDAT," _
           &"TRANSDAT, DROPDAT, CANCELDAT, CANCELUSR, OVERDUE, MEMO, MOVETOCOMQ1, MOVETOLINEQ1, MOVEFROMCOMQ1, " _
           &"MOVEFROMLINEQ1, MOVETODAT, MOVEFROMDAT, NCICCUSNO, CUSTIP1, CUSTIP2, CUSTIP3, CUSTIP4,sphnno,OLDCUSTIP1,OLDCUSTIP2,OLDCUSTIP3,OLDCUSTIP4) " _
           &"SELECT  COMQ1, LINEQ1, CUSID, " & ENTRYNO & ", getdate(), 'C','" & v(0) & "', CUSNC, " _
           &"FIRSTIDTYPE, SOCIALID, SECONDIDTYPE,SECONDNO, BIRTHDAY, EMAIL, CONTACTTEL, MOBILE, CUTID1, TOWNSHIP1, " _
           &"RADDR1, RZONE1, CUTID2, TOWNSHIP2, RADDR2, RZONE2, CUTID3, TOWNSHIP3, RADDR3, RZONE3, COCONTACT, COCONTACTTEL, " _
           &"COCONTACTTELEXT, COMOBILE, COBOSS, COBOSSSOCIAL, TRADETYPE, EUSR, EDAT, UUSR, UDAT, AREAID, GROUPID, SALESID, " _
           &"CASETYPE, FREECODE, PMCODE, PAYTYPE, AGENTNAME, AGENTSOCIAL, AGENTTEL, RCVD, APPLYDAT, FINISHDAT, DOCKETDAT," _
           &"TRANSDAT, DROPDAT, CANCELDAT, CANCELUSR, OVERDUE, MEMO, MOVETOCOMQ1, MOVETOLINEQ1, MOVEFROMCOMQ1, " _
           &"MOVEFROMLINEQ1, MOVETODAT, MOVEFROMDAT, NCICCUSNO, CUSTIP1, CUSTIP2, CUSTIP3, CUSTIP4,sphnno,OLDCUSTIP1,OLDCUSTIP2,OLDCUSTIP3,OLDCUSTIP4 " _
           &"FROM RTSparq499Cust where comq1=" & key(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' "
     ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update RTSparq499Cust set CANCELdat=getdate(),CANCELUSR='" & V(0) & "',OLDCUSTIP1=CUSTIP1,OLDCUSTIP2=CUSTIP2,OLDCUSTIP3=CUSTIP3,OLDCUSTIP4=CUSTIP4,CUSTIP1='',CUSTIP2='',CUSTIP3='',CUSTIP4='' where comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' "
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM RTSparq499Custlog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' AND ENTRYNO=" & ENTRYNO
            CONNXX.Execute sqlyy 
         else
            endpgm="1"
            errmsg=""
         end if      
      end if
   END IF
   RSXX.CLOSE
   connXX.Close
   SET RSXX=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "用戶資料作廢成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此用戶已完工，不可作廢；請執行退租作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此用戶已作廢，不可重覆執行作廢作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    else
       msgbox "無法執行用戶資料作廢作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=rtsparq499custcancel.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>