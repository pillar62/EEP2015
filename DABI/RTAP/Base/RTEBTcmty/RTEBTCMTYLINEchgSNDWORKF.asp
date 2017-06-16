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
   sqlxx="select * FROM RTEBTCMTYLINECHGsndwork WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " and prtno='" & key(2) & "' and prtno2='" & key(3) & "' "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   sndtype=""
   endpgm="1"
   SQLYY="SELECT * FROM RTEBTCMTYLINE WHERE COMQ1=" & KEY(0) & " AND LINEQ1=" & KEY(1)
   RSYY.Open SQLYY,CONNXX
   IF LEN(TRIM(RSYY("CONSIGNEE"))) > 0 THEN
      CONSIGNEE="Y" '經銷 ==> MAIL 郁玲
   ELSE
      AREAID=RSYY("AREAID")
      GROUPID=RSYY("GROUPID")
      CONSIGNEE="N" '直銷 ==> MAIL給各組業助及郁玲
   END IF   
   RSYY.CLOSE 
   SQLYY="SELECT * FROM RTEBTCMTYLINECHG WHERE COMQ1=" & KEY(0) & " AND LINEQ1=" & KEY(1) & " AND PRTNO='" & KEY(2) & "' "
   RSYY.Open SQLYY,CONNXX
   NCOMQ1=RSYY("NCOMQ1")
   NLINEQ1=RSYY("NLINEQ1")
  ' IF ISNULL(RSYY("UPDEBTCHKDAT")) OR  ISNULL(RSYY("UPDEBTTNSDAT")) THEN
   IF ISNULL(RSYY("UPDEBTCHKDAT"))  THEN  '(先只檢查申請審核日，而申請轉檔日因尚未正式啟用920429故先不檢查
      ENDPGM="8" '當主線異動檔尚未向EBT提出申請時，不可執行完工作業
   END IF
   RSYY.CLOSE 
   IF ENDPGM <> "8" THEN
    '當主線異動派工單已作廢時，不可執行完工結案或未完工結案
     IF LEN(TRIM(RSXX("DROPDAT"))) <> 0 THEN
        ENDPGM="3"
     '當主線異動派工單裝機完工日空白時,不可執行完工或未完工結案
     elseif ISNULL(RSXX("CLOSEDAT"))  then
        endpgm="7"            
     '當主線異動派工單完工結案日或未完工結案日不為空白時，不可執行完工結案
     elseif LEN(TRIM(RSXX("FINISHDAT"))) <> 0 OR LEN(TRIM(RSXX("UNCLOSEDAT"))) <> 0 then
        endpgm="4"      
     ELSE
      sndtype=rsxx("sndtype")
      sqlyy="select max(entryno) as entryno FROM RTEBTCMTYLINECHGsndworklog WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " and prtno='" & key(2) & "' and prtno2='" & key(3) & "' "
      rsyy.Open sqlyy,connxx
      
      if len(trim(rsyy("entryno"))) > 0 then
         entryno=rsyy("entryno") + 1
      else
         entryno=1
      end if
      rsyy.close
      'set rsyy=nothing
      sqlyy="insert into RTEBTCMTYLINECHGsndworklog " _ 
           &"SELECT   COMQ1, LINEQ1, PRTNO, PRTNO2, " & ENTRYNO & ", GETDATE() ,'F','" & V(0) & "',SENDWORKDAT, PRTUSR, ASSIGNENGINEER1, ASSIGNENGINEER2, " _
           &"ASSIGNENGINEER3, ASSIGNENGINEER4, ASSIGNENGINEER5, ASSIGNCONSIGNEE, REALENGINEER1, REALENGINEER2, REALENGINEER3, " _
           &"REALENGINEER4, REALENGINEER5, REALCONSIGNEE, DROPDAT,DROPDESC, CLOSEDAT, BONUSCLOSEYM, BONUSCLOSEDAT, " _
           &"BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, STOCKCLOSEDAT,STOCKCLOSEUSR, STOCKFINCHK, SNDTYPE, HOSTCABLENO, MEMO, " _
           &"PRTDAT, EUSR, EDAT, UUSR, UDAT, CLOSEUSR, DROPUSR, UNCLOSEDAT,finishdat " _
           &"FROM RTEBTCMTYLINEchgsndwork where comq1=" & key(0) & " and lineq1=" & key(1) & " and prtno='" & key(2) & "' and prtno2='" & key(3) & "' "
     ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update RTEBTcmtylineCHGsndwork set FINISHdat=getdate() ,CLOSEUSR='" & V(0) & "' where comq1=" & KEY(0) & " and lineq1=" & key(1)  & " and prtno='" & key(2) & "' and prtno2='" & key(3) & "' "
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM RTEBTcmtylinechgsndworklog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & "' and prtno='" & key(2) & "' and prtno2='" & key(3) & "' AND ENTRYNO=" & ENTRYNO
            CONNXX.Execute sqlyy 
         else
            '完工結案，若為標準工程、連絡纜工程、測通、測通及加裝設備且主線異動檔尚未移機完成者，須更新主線異動檔移機完成日finishdat
            '使用者再將該筆已完工之異動輸入報竣審核日轉檔至EBT，待EBT回覆OK接收OK後，更新主線主檔之相關異動資料(此部份由轉檔接收程式負責)
            IF sndtype ="OC" OR  sndtype ="OP" OR  sndtype ="ST" OR  sndtype ="CB" THEN
               SQLXX=" update RTEBTcmtylineCHG set FINISHdat=getdate() ,UUSR='" & V(0) & "',UDAT=GETDATE()  where comq1=" & KEY(0) & " and lineq1=" & key(1) & " and PRTNO='" & key(2)  & "' AND RTEBTcmtylineCHG.FINISHdat IS NULL "
               STS="3"
               connxx.Execute SQLXX
               '異動前的主線要更新撤線日
               SQLXX=" update RTEBTcmtyline set DROPDAT=getdate() ,UUSR='" & V(0) & "',UDAT=GETDATE()  where comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND RTEBTcmtyline.ADSLAPPLYDAT IS NULL "
               STS="3"
               connxx.Execute SQLXX
               '異動後的主線要更新測通日
               SQLXX=" update RTEBTcmtyline set ADSLAPPLYDAT=getdate() ,UUSR='" & V(0) & "',UDAT=GETDATE()  where comq1=" & NCOMQ1 & " and lineq1=" & NLINEQ1 & " AND RTEBTcmtyline.ADSLAPPLYDAT IS NULL "
               STS="3"
               connxx.Execute SQLXX
               If Err.number > 0 then
                  endpgm="2"
               '發生錯誤時，刪除異動檔所新增的異動資料
                  errmsg=cstr(Err.number) & "=" & Err.description
                  sqlyy="delete * FROM RTEBTcmtylinechgsndworklog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & "' and prtno='" & key(2) & "' and prtno2='" & key(3)& "' AND ENTRYNO=" & ENTRYNO
                  CONNXX.Execute sqlyy 
               ELSE
                  endpgm="1"
                  errmsg=""
               END IF
            END IF
         end if      
      end if
    END IF
   END IF
   SQLYY="SELECT * FROM RTEBTCMTYH WHERE COMQ1=" & KEY(0)
   RSYY.Open SQLYY,CONNXX
   COMN=RSYY("COMN")
   RSYY.CLOSE
   IF ENDPGM="1" AND STS="3" THEN '當需更新主線之完工日的派工種類(STS=3)返轉才須要通知相關人員
      FROMEMAIL="MIS@CBBN.COM.TW"
      Set jmail = Server.CreateObject("Jmail.Message")
      jmail.charset="BIG5"
      jmail.from = "MIS@cbbn.com.tw"
      Jmail.fromname="AVS系統通知"
      IF STS="1" THEN
         jmail.Subject = "AVS社區︰" & COMN & "，異動派工單︰" & KEY(0) & "-" & KEY(1) & "-" & KEY(2) & "-" & KEY(3) & "，完工結案通知"
      ELSE
         jmail.Subject = "AVS社區︰" & COMN & "，異動派工單︰" & KEY(0) & "-" & KEY(1) & "-" & KEY(2) & "-" & KEY(3) & "，未完工結案通知"
      END IF
      
      jmail.priority = 1  
      body="<html><body><table border=0 width=""60%""><tr><td colspan=2>" _
      &"<H3>東森AVS主線異動派工結案通知</h3></td></tr>" _
      &"<tr><td width=""30%"">&nbsp;</td><td width=""70%"">&nbsp;</td></tr>" _
      &"<tr><td bgcolor=lightblue align=center>社區主線序號</td><td bgcolor=pink align=left>" &KEY(0) & "-" & KEY(1) &"</td></tr>" _
      &"<tr><td bgcolor=lightblue align=center>社區名稱</td><td bgcolor=pink align=left>" &comn & "-" & KEY(1) &"</td></tr>" _
      &"<tr><td bgcolor=lightblue align=center>異動單號</td><td bgcolor=pink align=left>" &KEY(2) &"</td></tr>" _
      &"<tr><td bgcolor=lightblue align=center>派工單號</td><td bgcolor=pink align=left>" &KEY(3) &"</td></tr>" _
      &"</table>" _
      &"<P><U>此主線異動已完工結案</U></body></html>"  
      jmail.HTMLBody = BODY
      JMail.AddRecipient "anita@cbbn.com.tw","AVS總窗口"
      JMail.AddRecipient "mis@cbbn.com.tw","資訊部"
      JMail.AddRecipient "brian@cbbn.com.tw","工務窗口"
      IF CONSIGNEE="Y" THEN
        ' JMail.AddRecipient "EDSON@cbbn.com.tw","坡朋"
      ELSE
         IF AREAID="A2" THEN
            JMail.AddRecipient "lini@cbbn.com.tw","台中業助"
         ELSEIF AREAID="A3" THEN
            JMail.AddRecipient "cute0318@cbbn.com.tw","高雄業助"
         ELSEIF AREAID="A1" AND (GROUPID="01" OR GROUPID="07") THEN '桃園
            JMail.AddRecipient "s525422@cbbn.com.tw","桃園業助"
         ELSEIF AREAID="A1" AND (GROUPID="G1" OR GROUPID="G2" OR GROUPID="G3"  OR GROUPID="G4"  OR GROUPID="02" OR GROUPID="03" OR GROUPID="04" OR GROUPID="05" OR GROUPID="06") THEN '台北
            JMail.AddRecipient "tiffany01@cbbn.com.tw","台北業助"            
         END IF
      END IF
      jmail.Send ( "219.87.146.239" )      
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
       msgbox "AVS主線異動派工單完工結案成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "當異動單已作廢時，不可執行完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "當主線異動單完工結案日或未完工結案日不為空白時，不可執行完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="5" then
       msgbox "此主線異動派工已月結，不可異動" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close      
    elseIF frm1.htmlfld.value="6" then
       msgbox "此主線異動派工單結案時，必須先輸入實際裝機人員或實際裝機經銷商" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                  
    elseIF frm1.htmlfld.value="7" then
       msgbox "當派工單裝機完工日空白時,不可執行完工或未完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                     
    elseIF frm1.htmlfld.value="8" then
       msgbox "當主線異動檔尚未向EBT提出申請時，不可執行完工作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                         
    else
       msgbox "無法執行主線異動派工單完工結案作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=rtebtcmtylineCHGsndworkF.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>