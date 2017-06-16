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
   ENDPGM=""
 '  On Error Resume Next
   sqlxx="select * FROM RTEBTCUSTm2m3sndwork WHERE avsno='" & KEY(0) & "' and m2m3='" & key(1) & "' and seq=" & key(2) & " and prtno='" & key(3) & "' "
 '-----------------------------------------------------------------------------------------------------------------------
 '  此段主要是判斷欠費拆機(強停)客戶，其欠拆資料是否已上傳至EBT,若已上傳則不可做結案返轉(但目前因無退租轉檔功能故暫保留)
 '  sqlYY="select * FROM RTEBTCUST WHERE AVSNO='" & KEY(0) & "' "
 '  RSYY.Open SQLYY,CONNXX
 '  ENDPGM=""
 '  IF RSYY.EOF THEN
 '     ENDPGM="6"
 '  ELSE
 '  ===>RSYY("DOCKETDAT") 要改為欠費拆機用戶回報EBT日期(還未開此欄位)
 '     IF LEN(TRIM(RSYY("DOCKETDAT"))) > 0 THEN ENDPGM="7"
 '  END IF
 '  RSYY.CLOSE
 '=================================================================
   'IF1S
   IF ENDPGM="" THEN
      'RESPONSE.Write SQLXX
   '  RESPONSE.END
     RSXX.OPEN SQLXX,CONNxx
     endpgm="1"
     '當已作廢時，不可執行完工結案或未完工結案
     'IF2S
     IF not isnull(RSXX("DROPDAT")) THEN
        ENDPGM="4"
     elseif LEN(TRIM(RSXX("BONUSCLOSEYM"))) <> 0 OR LEN(TRIM(RSXX("STOCKCLOSEYM"))) <> 0 then
        endpgm="5"
     elseif isnull(RSXX("CLOSEDAT")) and isnull(RSXX("UNCLOSEDAT")) then
   '  response.Write "aaa"
   '  response.end
        endpgm="3"      
     ELSE
        sqlyy="select max(entryno) as entryno FROM RTEBTCUSTM2M3sndworklog WHERE AVSNO='" & KEY(0) & "' and M2M3='" & key(1) & "' and seq=" & key(2) & " and prtno='" & key(3) & "' "
        rsyy.Open sqlyy,connxx
        'IF3-1S
        if len(trim(rsyy("entryno"))) > 0 then
           entryno=rsyy("entryno") + 1
        else
           entryno=1
        end if
        'IF3-1E
        rsyy.close
        set rsyy=nothing
        '完工結案返轉
        'IF3-2S
        IF LEN(TRIM(RSXX("CLOSEDAT"))) > 0 THEN
        sqlyy="insert into RTEBTCUSTM2M3sndworklog " _
           &"SELECT   AVSNO,M2M3,seq,PRTNO, " & ENTRYNO & ", getdate(), 'FR','" &  v(0) & "', " _
           &"SENDWORKDAT, PRTUSR, ASSIGNENGINEER,ASSIGNCONSIGNEE, REALENGINEER, REALCONSIGNEE, DROPDAT, 'AVS用戶欠費拆機完工結案返轉'," _
           &"CLOSEDAT, BONUSCLOSEYM, BONUSCLOSEDAT, " _
           &"BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, STOCKCLOSEDAT, " _
           &"STOCKCLOSEUSR, STOCKFINCHK, MDF1,MDF2, HOSTNO,HOSTPORT, MEMO, PRTDAT,UNCLOSEDAT,DROPUSR,CLOSEUSR  " _
           &"FROM RTEBTCUSTM2M3sndwork where AVSNO='" & key(0) & "' and M2M3='" & key(1) & "' and seq=" & key(2) & " and prtno='" & key(3) & "' "
        '未完工結案返轉
        ELSE
        sqlyy="insert into RTEBTCUSTM2M3sndworklog " _
           &"SELECT   AVSNO,M2M3,seq,PRTNO, " & ENTRYNO & ", getdate(), 'UR','" &  v(0) & "', " _
           &"SENDWORKDAT, PRTUSR, ASSIGNENGINEER,ASSIGNCONSIGNEE, REALENGINEER, REALCONSIGNEE, DROPDAT, 'AVS用戶欠費拆機未完工結案返轉'," _
           &"CLOSEDAT, BONUSCLOSEYM, BONUSCLOSEDAT, " _
           &"BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, STOCKCLOSEDAT, " _
           &"STOCKCLOSEUSR, STOCKFINCHK, MDF1,MDF2, HOSTNO,HOSTPORT, MEMO, PRTDAT,UNCLOSEDAT,DROPUSR,CLOSEUSR  " _
           &"FROM RTEBTCUSTM2M3sndwork where AVSNO='" & key(0) & "' and M2M3='" & key(1) & "' and seq=" & key(2) & " and prtno='" & key(3) & "' "
        END IF
        'IF3-2E
     ' Response.Write sqlyy
        CONNXX.Execute sqlyy     
        'IF3-3S
        If Err.number > 0 then
           endpgm="2"
           errmsg=cstr(Err.number) & "=" & Err.description
        else
           '完工結案返轉
           'IF4-1S
           IF LEN(TRIM(RSXX("CLOSEDAT"))) > 0 THEN
              SQLXX=" update RTEBTCUSTM2M3sndwork set CLOSEdat=NULL,CLOSEUSR='' where AVSNO='" & key(0) & "' and M2M3='" & key(1) & "' and seq=" & key(2) & " and prtno='" & key(3) & "' "
              connxx.Execute SQLXX
              'IF5-1S
              If Err.number > 0 then
                 endpgm="2"
                '發生錯誤時，刪除異動檔所新增的異動資料
                 errmsg=cstr(Err.number) & "=" & Err.description
                 sqlyy="delete * FROM RTEBTCUSTM2M3sndworklog WHERE AVSNO='" & key(0) & "' and M2M3='" & key(1) & "' and seq=" & key(2) & " and prtno='" & key(3) & "' AND ENTRYNO=" & ENTRYNO
                 CONNXX.Execute sqlyy 
              else
                '將用戶主檔之退租日期及欠費標記 清空
                 SQLXX=" update RTEBTCUST set DROPdat=NULL,UUSR='" & V(0) & "',UDAT=GETDATE(),OVERDUE=''  where AVSNO='" & key(0) & "'"
                 connxx.Execute SQLXX
                 'IF6-1S
                 If Err.number > 0 then
                    endpgm="2"
                   '發生錯誤時，刪除異動檔所新增的異動資料
                    errmsg=cstr(Err.number) & "=" & Err.description
                    sqlyy="delete * FROM RTEBTCUSTM2M3sndworklog WHERE AVSNO='" & key(0) & "' and M2M3='" & key(1) & "' and seq=" & key(2) & " and prtno='" & key(3) & "' AND ENTRYNO=" & ENTRYNO
                    CONNXX.Execute sqlyy 
                 ELSE
                    endpgm="1"
                    errmsg=""
                 END IF
                 'IF6-1E
              end if      
              'IF5-1E
           ELSE
           '未完工結案返轉
              SQLXX=" update RTEBTCUSTM2M3sndwork set UNCLOSEdat=NULL,CLOSEUSR='' where AVSNO='" & key(0) & "' and M2M3='" & key(1) & "' and seq=" & key(2) & " and prtno='" & key(3) & "' "
              connxx.Execute SQLXX
              'IF5-2S
              If Err.number > 0 then
                 endpgm="2"
                '發生錯誤時，刪除異動檔所新增的異動資料
                 errmsg=cstr(Err.number) & "=" & Err.description
                 sqlyy="delete * FROM RTEBTCUSTM2M3sndworklog WHERE AVSNO='" & key(0) & "' and M2M3='" & key(1) & "' and seq=" & key(2) & " and prtno='" & key(3) & "' AND ENTRYNO=" & ENTRYNO
                 CONNXX.Execute sqlyy 
              else
                    endpgm="1"
                    errmsg=""
             END IF
             'IF5-2E
           end if      
           'IF4-1E
        end if
        'IF3-3E
     END IF
     'IF2E
     RSXX.CLOSE
   END IF
   'IF1E
   connXX.Close
   SET RSXX=NOTHING
   set connXX=nothing
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "AVS用戶欠費拆機工單完工/未完工結案返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此欠費拆機工單尚未結案，不可執行結案返轉作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此欠費拆機工單已作廢，不可執行結案返轉作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="5" then
       msgbox "此欠費拆機工單已月結，不可異動" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="6" then
       msgbox "無法找到此欠費拆機工單之用戶主檔資料，請確認AVS用戶主檔資料正常" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                     
    elseIF frm1.htmlfld.value="7" then
       msgbox "用戶欠拆已轉檔，不可執行完工結案返轉作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                     
    else
       msgbox "無法執行欠費拆機工單完工結案返轉作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTEBTCUSTM2M3SNDWORKFR.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>