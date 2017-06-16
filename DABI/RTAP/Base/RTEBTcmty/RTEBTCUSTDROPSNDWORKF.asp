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
   SET RSC=Server.CreateObject("ADODB.RECORDSET")
   DSN="DSN=RtLib"
   connXX.Open DSN
 '  On Error Resume Next
   sqlxx="select * FROM RTEBTCUSTDROPsndwork WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' AND ENTRYNO=" & KEY(3) & " and prtno='" & key(4) & "' "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   endpgm="1"
   '當已作廢時，不可執行完工結案或未完工結案
   IF LEN(TRIM(RSXX("DROPDAT"))) <> 0 THEN
      ENDPGM="3"
   elseif LEN(TRIM(RSXX("CLOSEDAT"))) <> 0 OR LEN(TRIM(RSXX("UNCLOSEDAT"))) <> 0 then
      endpgm="4"
   elseif LEN(TRIM(RSXX("REALENGINEER"))) = 0 AND LEN(TRIM(RSXX("REALCONSIGNEE"))) = 0 then
      endpgm="6"
   elseif LEN(TRIM(RSXX("BONUSCLOSEYM"))) <> 0 OR LEN(TRIM(RSXX("STOCKCLOSEYM"))) <> 0 then
      endpgm="5"
   ELSE
      sqlyy="select max(SEQ) as SEQ FROM RTEBTCUSTDROPsndworklog WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' AND ENTRYNO=" & KEY(3)& " and prtno='" & key(4) & "' "
      rsyy.Open sqlyy,connxx
      
      if len(trim(rsyy("SEQ"))) > 0 then
         SEQ=rsyy("SEQ") + 1
      else
         SEQ=1
      end if
      rsyy.close
      set rsyy=nothing
      sqlyy="insert into RTEBTCUSTDROPsndworklog " _
           &"SELECT   COMQ1, LINEQ1, CUSID,ENTRYNO,PRTNO, " & SEQ & ", getdate(), 'F','" &  v(0) & "', " _
           &"SENDWORKDAT, PRTUSR, ASSIGNENGINEER,ASSIGNCONSIGNEE, REALENGINEER, REALCONSIGNEE, DROPDAT, 'AVS用戶拆機完工結案'," _
           &"CLOSEDAT, BONUSCLOSEYM, BONUSCLOSEDAT, " _
           &"BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, STOCKCLOSEDAT, " _
           &"STOCKCLOSEUSR, STOCKFINCHK, MDF1,MDF2, HOSTNO,HOSTPORT, MEMO, PRTDAT,UNCLOSEDAT,DROPUSR,CLOSEUSR  " _
           &"FROM RTEBTCUSTDROPsndwork where comq1=" & key(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' AND ENTRYNO=" & KEY(3) & " and prtno='" & key(4) & "' "
     ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update RTEBTCUSTDROPsndwork set CLOSEdat=getdate(),CLOSEUSR='" & V(0) & "' where comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' AND ENTRYNO=" & KEY(3) & " and prtno='" & key(4) & "' "
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM RTEBTCUSTDROPsndworklog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' AND ENTRYNO=" & KEY(3) & " and prtno='" & key(4) & "' AND SEQ=" & SEQ
            CONNXX.Execute sqlyy 
         else
            '完工結案，須更新用戶退租主檔FINISHDAT
            SQLXX=" update RTEBTCUSTDROP set FINISHdat=getdate(),UUSR='" & V(0) & "',UDAT=GETDATE()  where comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' AND ENTRYNO=" & KEY(3) 
            connxx.Execute SQLXX
            If Err.number > 0 then
               endpgm="2"
               '發生錯誤時，刪除異動檔所新增的異動資料
               errmsg=cstr(Err.number) & "=" & Err.description
               sqlyy="delete * FROM RTEBTCUSTDROPsndworklog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' AND ENTRYNO=" & KEY(3) & " and prtno='" & key(4) & "' AND SEQ=" & SEQ
               CONNXX.Execute sqlyy 
            ELSE
              '完工結案，須更新用戶主檔dropdat
               SQLXX=" update RTEBTCUST set dropdat=getdate(),UUSR='" & V(0) & "',UDAT=GETDATE()  where comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' "
               connxx.Execute SQLXX
               If Err.number > 0 then
                  endpgm="2"
                  '發生錯誤時，刪除異動檔所新增的異動資料
                  errmsg=cstr(Err.number) & "=" & Err.description
                  sqlyy="delete * FROM RTEBTCUSTDROPsndworklog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' AND ENTRYNO=" & KEY(3) & " and prtno='" & key(4) & "' AND SEQ=" & SEQ
                  CONNXX.Execute sqlyy 
                  sqlyy="update RTEBTCUSTDROP set FINISHdat=null,UUSR='" & V(0) & "',UDAT=GETDATE()  where comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' AND ENTRYNO=" & KEY(3) 
                  CONNXX.Execute sqlyy 
               else
                  endpgm="1"
                  errmsg=""
                  '發送EMAIL通知報竣窗口
               '   SQLXX="select * from rtEBTcustdropsndwORk INNER JOIN RTEBTCMTYH ON rtEBTcustdropsndwork.COMQ1=RTEBTCMTYH.COMQ1 " _
               '          &"INNER JOIN  rtEBTcust ON  rtEBTcustdropsndwork.COMQ1= rtEBTcust.COMQ1 AND rtEBTcustdropsndwork.LINEQ1= rtEBTcust.LINEQ1 " _
               '           &"AND rtEBTcustdropSNDWORK.CUSID= rtEBTcust.CUSID INNER JOIN RTEBTCUSTDROP ON " _
               '           &"RTEBTCUSTDROPSNDWORK.COMQ1=RTEBTCUSTDROP.COMQ1 AND  RTEBTCUSTDROPSNDWORK.LINEQ1=RTEBTCUSTDROP.LINEQ1 AND " _
               '           &"RTEBTCUSTDROPSNDWORK.CUSID=RTEBTCUSTDROP.CUSID  AND RTEBTCUSTDROPSNDWORK.ENTRYNO=RTEBTCUSTDROP.ENTRYNO " _
               '           &"where rtEBTcustdropSNDWORK.comq1=" & key(0) & " and rtEBTcustdropSNDWORK.lineq1=" & key(1) & " and " _
               '           &"rtEBTcustdropSNDWORK.cusid='" & key(2) & "' AND rtEBTcustdropSNDWORK.ENTRYNO=" & KEY(3)
               '    RESPONSE.Write SQLXX
                  rsc.open "select * from rtEBTcustdropsndwORk INNER JOIN RTEBTCMTYH ON rtEBTcustdropsndwork.COMQ1=RTEBTCMTYH.COMQ1 " _
                          &"INNER JOIN  rtEBTcust ON  rtEBTcustdropsndwork.COMQ1= rtEBTcust.COMQ1 AND rtEBTcustdropsndwork.LINEQ1= rtEBTcust.LINEQ1 " _
                          &"AND rtEBTcustdropSNDWORK.CUSID= rtEBTcust.CUSID INNER JOIN RTEBTCUSTDROP ON " _
                          &"RTEBTCUSTDROPSNDWORK.COMQ1=RTEBTCUSTDROP.COMQ1 AND  RTEBTCUSTDROPSNDWORK.LINEQ1=RTEBTCUSTDROP.LINEQ1 AND " _
                          &"RTEBTCUSTDROPSNDWORK.CUSID=RTEBTCUSTDROP.CUSID  AND RTEBTCUSTDROPSNDWORK.ENTRYNO=RTEBTCUSTDROP.ENTRYNO " _
                          &"where rtEBTcustdropSNDWORK.comq1=" & key(0) & " and rtEBTcustdropSNDWORK.lineq1=" & key(1) & " and " _
                          &"rtEBTcustdropSNDWORK.cusid='" & key(2) & "' AND rtEBTcustdropSNDWORK.ENTRYNO=" & KEY(3) ,connXX
                  Set jmail = Server.CreateObject("Jmail.Message")
                  jmail.charset="BIG5"
                  jmail.from = "MIS@cbbn.com.tw"
                  Jmail.fromname="東森AVS用戶退租拆機完工通知"
                  jmail.Subject = "東森AVS退租用戶︰" & RSc("CUSNC") & "-" & RSc("AVSNO") & "，已拆機完成通知"
                  jmail.priority = 1  
                  body="<html><body><table border=1 width=""80%""> " 
                  BODY=BODY & "<tr><H3>東森AVS退租用戶拆機完工通知</h3></td></tr>" _
                      &"<tr><td bgcolor=lightblue align=center>主線</td><td bgcolor=lightblue align=center>社區名稱</td>"_
                      &"<td bgcolor=lightblue align=center>用戶名稱</td><td bgcolor=lightblue align=center>合約編號</td>"_
                      &"<td bgcolor=lightblue align=center>退租申請日</td><td bgcolor=lightblue align=center>預計服務中止日</td>" _
                      &"<td bgcolor=LIGHTBLUE align=center>拆機單號</td></tr>"
                  
                  BODY=BODY & "<tr>" _
                      &"<td bgcolor=pink align=left>" &RSc("COMQ1") & "-" & RSc("LINEQ1")  &"</td>" _
                      &"<td bgcolor=pink align=left>" &RSc("COMN")  &"</td>" _
                      &"<td bgcolor=pink align=left>" &RSc("CUSNC")&"</td>" _
                      &"<td bgcolor=pink align=left>" &RSc("AVSNO")&"</td>" _
                      &"<td bgcolor=pink align=left>" &RSc("APPLYDAT")&"</td>" _
                      &"<td bgcolor=pink align=left>" &RSc("EXPECTDAT")&"</td>" _
                      &"<td bgcolor=pink align=left>" &RSc("prtno")&"</td></TR>" 
                       
                  BODY=BODY & "</table><P><U>請執行退租報竣作業</U></body></html>"
                  FROMEMAIL="MIS@CBBN.COM.TW"
                  jmail.HTMLBody = BODY
                  JMail.AddRecipient "mis@cbbn.com.tw","資訊部"
                  JMail.AddRecipient "maybe0606@cbbn.com.tw","東森AVS報竣窗口"
                  jmail.Send ( "219.87.146.239" )      
                  rsC.close
                  set rsc=nothing
               end if
            END IF
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
       msgbox "AVS用戶拆機竣工確認單完工結案成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "當已作廢時，不可執行完工結案或未完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此拆機竣工單已完工結案或未完工結案，不可重複執行完工結案或未完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="5" then
       msgbox "此拆帳竣工單已月結，不可異動" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close      
    elseIF frm1.htmlfld.value="6" then
       msgbox "此拆機竣工單完工時，必須先輸入實際拆機人員或實際拆機經銷商" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                  
    else
       msgbox "無法執行用戶拆機竣工單完工結案作業,錯誤訊息" & "  " & frm1.htmlfld1.value
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