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
   sqlxx="select * FROM RTEBTCUST WHERE avsno='" & KEY(0) & "' "
   RSXX.Open SQLXX,CONNXX
   IF NOT RSXX.EOF THEN
      IF NOT ISNULL(RSXX("DROPDAT")) THEN
         '用戶主檔已退租不可復機，需重新申請
         ENDPGM="9"
      ELSEIF RSXX("OVERDUE") <> "Y" THEN
         '用戶主檔尚未執行欠拆作業或欠拆作業未完工，不可執行復機完工作業
         ENDPGM="8"
      ELSE
         ENDPGM="1"
      END IF
   ELSE
      '用戶主檔找不到此用戶資料，請通知資訊部!
      endpgm="7"
   END IF
   RSXX.CLOSE
 '  On Error Resume Next
IF ENDPGM="1" THEN     
   sqlxx="select * FROM RTEBTCUSTm2RTNsndwork WHERE avsno='" & KEY(0) & "' and m2m3='" & key(1) & "' and seq=" & key(2) & " and RTNno='" & key(3) & "' "
   RSXX.OPEN SQLXX,CONNxx
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
      sqlyy="select max(entryno) as entryno FROM RTEBTCUSTm2m3sndworklog WHERE avsno='" & KEY(0) & "' and m2m3='" & key(1) & "' and seq=" & key(2) & " and prtno='" & key(3) & "' "
      rsyy.Open sqlyy,connxx
      
      if len(trim(rsyy("entryno"))) > 0 then
         entryno=rsyy("entryno") + 1
      else
         entryno=1
      end if
      rsyy.close
      set rsyy=nothing
      sqlyy="insert into RTEBTCUSTm2RTNsndworklog " _
           &"SELECT   avsno, m2m3,seq,RTNNO, " & ENTRYNO & ", getdate(), 'F','" &  v(0) & "', " _
           &"SENDWORKDAT, PRTUSR, ASSIGNENGINEER,ASSIGNCONSIGNEE, REALENGINEER, REALCONSIGNEE, DROPDAT, 'AVS用戶復機完工結案'," _
           &"CLOSEDAT, BONUSCLOSEYM, BONUSCLOSEDAT, " _
           &"BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, STOCKCLOSEDAT, " _
           &"STOCKCLOSEUSR, STOCKFINCHK, MDF1,MDF2, HOSTNO,HOSTPORT, MEMO, PRTDAT,UNCLOSEDAT,DROPUSR,CLOSEUSR  " _
           &"FROM RTEBTCUSTm2RTNsndwork where avsno='" & key(0) & "' and m2m3='" & key(1) & "' and seq=" & key(2) & " and RTNno='" & key(3) & "' "
     ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update RTEBTCUSTm2RTNsndwork set CLOSEdat=getdate(),CLOSEUSR='" & V(0) & "' where avsno='" & key(0) & "' and m2m3='" & key(1) & "' and seq=" & key(2) & " and RTNno='" & key(3) & "' "
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM RTEBTCUSTm2RTNsndworklog WHERE avsno='" & key(0) & "' and m2m3='" & key(1) & "' and seq=" & key(2) & " and RTNno='" & key(3) & "' AND ENTRYNO=" & ENTRYNO
            CONNXX.Execute sqlyy 
         else
            '完工結案，須更新m2m3用戶主檔FINISHDAT
           ' SQLXX=" update RTEBTCUSTm2m3 set closedat=getdate(),CLOSEUSR='" & V(0) & "' where avsno='" & key(0) & "' and m2m3='" & key(1) & "' "
           ' connxx.Execute SQLXX
           ' If Err.number > 0 then
           '    endpgm="2"
           '    '發生錯誤時，刪除異動檔所新增的異動資料
           '    errmsg=cstr(Err.number) & "=" & Err.description
           '    sqlyy="delete * FROM RTEBTCUSTm2m3sndworklog WHERE avsno='" & key(0) & "' and m2m3='" & key(1) & "'  and prtno='" & key(2) & "' AND ENTRYNO=" & ENTRYNO
           '    CONNXX.Execute sqlyy 
           ' ELSE
               '完工結案，須更新用戶主檔OVERDUE(在M3時才執行更新退租日)
               SQLXX=" update RTEBTCUST set overdue=''  where avsno='" & key(0) & "' "
               connxx.Execute SQLXX
               If Err.number > 0 then
                  endpgm="2"
                 '發生錯誤時，刪除異動檔所新增的異動資料
                  errmsg=cstr(Err.number) & "=" & Err.description
                  sqlyy="delete * FROM RTEBTCUSTm2RTNsndworklog WHERE avsno='" & key(0) & "' and m2m3='" & key(1) & "' and seq=" & key(2) & " and RTNno='" & key(3) & "' AND ENTRYNO=" & ENTRYNO
                  CONNXX.Execute sqlyy 
                 '發生錯誤時，還原已被更新的M2M3完工結案內容
                '  sqlyy="UPDATE RTEBTCUSTm2m3 SET closeDAT=NULL, UDAT=GETDATE()  WHERE avsno='" & key(0) & "' and m2m3='" & key(1) & "' "
                '  CONNXX.Execute sqlyy 
               ELSE
                  endpgm="1"
                  errmsg=""
               END IF
           ' END IF
         end if      
      end if
   END IF
   RSXX.CLOSE
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
       msgbox "AVS用戶復機工單完工結案成功",0
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
       msgbox "此復機工單已完工結案或未完工結案，不可重複執行完工結案或未完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="5" then
       msgbox "此復機工單已月結，不可異動" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close      
    elseIF frm1.htmlfld.value="6" then
       msgbox "此復機工單完工時，必須先輸入實際復機人員或實際復機經銷商" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                  
    elseIF frm1.htmlfld.value="7" then
       msgbox "用戶主檔找不到此用戶資料，請通知資訊部協助確認。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="8" then
       msgbox "此用戶已退租不可復機，需重新申請" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="9" then
       msgbox "用戶主檔尚未執行欠拆作業或欠拆作業未完工，不可執行復機完工作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                                   
    else
       msgbox "無法執行復機工單完工結案作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=rtebtcustm2RTNsndworkf.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>