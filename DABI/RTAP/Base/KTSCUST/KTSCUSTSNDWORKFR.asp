<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONNXX
   Set connXX=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   SET RSZZ=Server.CreateObject("ADODB.RECORDSET")  
   SET RSyy=Server.CreateObject("ADODB.RECORDSET")
   DSN="DSN=RtLib"
   connXX.Open DSN
   endpgm="1"
 '  On Error Resume Next
   sqlZZ="select * FROM KTSCUST WHERE CUSID='" & KEY(0) & "' "
   RSZZ.Open SQLZZ,CONNXX
   IF RSZZ.EOF THEN
      ENDPGM="5"
   ELSE
      IF NOT ISNULL(RSZZ("DOCKETDAT")) THEN
         ENDPGM="6"
      END IF
   END IF
   RSZZ.CLOSE
IF ENDPGM ="1" THEN
   sqlxx="select * FROM KTSCUSTsndwork WHERE CUSID='" & KEY(0) & "' and prtno='" & key(1) & "' "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   '當尚未完工時，不可執行完工結案返轉或未完工結案返轉
   IF ISNULL(RSXX("CLOSEDAT")) AND ISNULL(RSXX("UNCLOSEDAT"))  THEN
      ENDPGM="3"
   ELSEIF LEN(TRIM(RSXX("BONUSCLOSEYM"))) <> 0  THEN
      ENDPGM="4"      
   ELSE
      sqlyy="select max(entryno) as entryno FROM KTSCUSTsndworklog WHERE CUSID='" & KEY(0) & "' and prtno='" & key(1) & "' "
      rsyy.Open sqlyy,connxx
      
      if len(trim(rsyy("entryno"))) > 0 then
         entryno=rsyy("entryno") + 1
      else
         entryno=1
      end if
      rsyy.close
      set rsyy=nothing
      sqlyy="insert into KTSCUSTsndworklog " _
           &"SELECT   CUSID,PRTNO, " & ENTRYNO & ", getdate(), 'FR','" &  v(0) & "', " _
           &"SENDWORKDAT, PRTUSR, ASSIGNENGINEER,ASSIGNCONSIGNEE, REALENGINEER, REALCONSIGNEE, DROPDAT, 'KTS用戶裝機完工/未完工結案返轉'," _
           &"CLOSEDAT, BONUSCLOSEYM, BONUSCLOSEDAT, " _
           &"BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, STOCKCLOSEDAT, " _
           &"STOCKCLOSEUSR, STOCKFINCHK, PRODNO,ITEMNO, MEMO, PRTDAT,UNCLOSEDAT,DROPUSR,CLOSEUSR ,EUSR, EDAT, UUSR, UDAT,FINISHDAT  " _
           &"FROM KTSCUSTsndwork where  CUSID='" & KEY(0) & "' and prtno='" & key(1) & "' "
     ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update KTSCUSTsndwork set CLOSEdat=NULL,UNCLOSEDAT=NULL,CLOSEUSR='' where CUSID='" & KEY(0) & "' and prtno='" & key(1) & "' "
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM KTSCUSTsndworklog WHERE CUSID='" & KEY(0) & "' and prtno='" & key(1) & "' AND ENTRYNO=" & ENTRYNO
            CONNXX.Execute sqlyy 
         else
            '完工結案，須更新用戶主檔FINISHDAT
            IF NOT ISNULL(RSXX("CLOSEDAT")) THEN
               SQLXX=" update KTSCUST set FINISHdat=NULL,UUSR='',UDAT=GETDATE()  where CUSID='" & KEY(0) & "' "
               connxx.Execute SQLXX
               If Err.number > 0 then
                  endpgm="2"
                  '發生錯誤時，刪除異動檔所新增的異動資料
                  errmsg=cstr(Err.number) & "=" & Err.description
                  sqlyy="delete * FROM KTSCUSTsndworklog WHERE CUSID='" & KEY(0) & "' and prtno='" & key(1) & "' AND ENTRYNO=" & ENTRYNO
                  CONNXX.Execute sqlyy 
               ELSE
                  endpgm="1"
                  errmsg=""
               END IF
            ELSE
               endpgm="1"
               errmsg=""
            END IF
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
       msgbox "KTS用戶裝機派工單完工/未完工結案返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "當派工單尚未完工/未完工結案時，不可執行完工或未完工結案返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此派工單已月結，不可異動" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close      
    elseIF frm1.htmlfld.value="5" then
       msgbox "KTS用戶基本資料檔找不到此用戶，無法執行結案返轉作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close      
    elseIF frm1.htmlfld.value="6" then
       msgbox "KTS用戶已報竣，不可返轉已結案的派工單" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                    
    else
       msgbox "無法執行派工單完工/未完工結案返轉作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=KTSCUSTSNDWORKFR.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>