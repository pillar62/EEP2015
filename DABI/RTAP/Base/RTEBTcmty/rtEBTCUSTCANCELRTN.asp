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
   sqlxx="select * FROM RTEBTCUST WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   endpgm="1"
   '當用戶無作廢記錄時，不可執行作廢返轉功能
   IF ISNULL(RSXX("CANCELDAT")) THEN
      ENDPGM="3"
   ELSE
      sqlyy="select max(entryno) as entryno FROM RTEBTCUSTlog WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' "
      rsyy.Open sqlyy,connxx
      
      if len(trim(rsyy("entryno"))) > 0 then
         entryno=rsyy("entryno") + 1
      else
         entryno=1
      end if
      rsyy.close
      set rsyy=nothing
      sqlyy="insert into RTEBTCUSTlog " _
           &"SELECT  COMQ1, LINEQ1, CUSID, " & ENTRYNO & ", getdate(), 'R','" & v(0) & "', CUSNC, " _
           &"SOCIALID, CUTID1, TOWNSHIP1, VILLAGE1, COD11, NEIGHBOR1, COD12, " _
                          &"STREET1, COD13, SEC1, COD14, LANE1, COD15, TOWN1, ALLEYWAY1, " _
                          &"COD16, NUM1, COD17, FLOOR1, COD18, ROOM1, COD19, CUTID2, " _
                          &"TOWNSHIP2, VILLAGE2, COD21, NEIGHBOR2, COD22, STREET2, COD23, SEC2, " _
                          &"COD24, LANE2, COD25, TOWN2, ALLEYWAY2, COD26, NUM2, COD27, FLOOR2, " _
                          &"COD28, ROOM2, COD29, CUTID3, TOWNSHIP3, VILLAGE3, COD31, " _
                          &"NEIGHBOR3, COD32, STREET3, COD33, SEC3, COD34, LANE3, COD35, TOWN3, " _
                          &"ALLEYWAY3, COD36, NUM3, COD37, FLOOR3, COD38, ROOM3, COD39, " _
                          &"BIRTHDAY, CONTACT, MOBILE, EMAIL, CONTACTTEL, PAYTYPE, AVSPMCODE, " _
                          &"DIALERPMCODE, DIALERPAYTYPE, AGENTNAME, AGENTTEL, AGENTSOCIAL, " _
                          &"CUTID4, TOWNSHIP4, VILLAGE4, NEIGHBOR4, STREET4, SEC4, LANE4, " _
                          &"TOWN4, ALLEYWAY4, NUM4, FLOOR4, ROOM4, RCVD, APPLYDAT, " _
                          &"APPLYTNSDAT, APPLYAGREE, PROGRESSID, FINISHDAT, DOCKETDAT, " _
                          &"TRANSDAT, STRBILLINGDAT, RZONE1, RZONE2, RZONE3, RZONE4, AREAID, " _
                          &"GROUPID, SALESID, COD41, COD42, COD43, COD44, COD45, COD46, COD47, " _
                          &"COD48, COD49, DROPDAT, FREECODE, OVERDUE, CUSTAPPLYDAT, " _
                          &"OLDSERVICECUTDAT, AVSNO, EUSR, EDAT, UUSR, UDAT, TRANSNOAPPLY, " _
                          &"TRANSNODOCKET, CASETYPE, MEMO, CANCELDAT, CANCELUSR,TNSCUSTCASE, MOVETOCOMQ1, MOVETOLINEQ1, MOVEFROMCOMQ1, MOVEFROMLINEQ1, MOVETODAT, MOVEFROMDAT, ENDBILLINGDAT  " _
           &"FROM RTEBTCUST where comq1=" & key(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' "
     ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update RTEBTCUST set CANCELdat=NULL,CANCELUSR='' where comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' "
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM RTEBTCUSTlog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' AND ENTRYNO=" & ENTRYNO
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
       msgbox "AVS用戶申請作廢返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此用戶尚未作廢，不可作廢返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    else
       msgbox "無法執行用戶申請作廢作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=rtebtcmtylinesndworkdrop.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>