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
   sqlxx="select * FROM RTsparqadslcmtysndwork WHERE cutyid=" & KEY(0) & " and prtno='" & key(1) & "' "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   sndtype=""
   endpgm="1"
   SQLYY="SELECT * FROM RTsparqadslcmty WHERE cutyid=" & KEY(0) 
   RSYY.Open SQLYY,CONNXX
   IF LEN(TRIM(RSYY("CONSIGNEE"))) > 0 THEN
      CONSIGNEE="Y" '經銷 ==> MAIL 玉鳳
   ELSE
      AREAID=RSYY("AREAID")
      GROUPID=RSYY("GROUPID")
      CONSIGNEE="N" '直銷 ==> MAIL給各組業助及玉鳳
   END IF   
   RSYY.CLOSE 
   '當已作廢時，不可執行完工結案或未完工結案
   IF LEN(TRIM(RSXX("DROPDAT"))) <> 0 THEN
      ENDPGM="3"
   '當裝機完工日空白時,不可執行完工或未完工結案
   elseif ISNULL(RSXX("CLOSEDAT"))  then
      endpgm="7"            
   '當主線完工結案日或未完工結案日不為空白時，不可執行完工結案
   elseif LEN(TRIM(RSXX("FINISHDAT"))) <> 0 OR LEN(TRIM(RSXX("UNCLOSEDAT"))) <> 0 then
      endpgm="4"      
   ELSE
      sndtype=rsxx("sndtype")
      sqlyy="select max(entryno) as entryno FROM RTsparqadslcmtysndworklog WHERE cutyid=" & KEY(0) & " and prtno='" & key(1) & "' "
      rsyy.Open sqlyy,connxx
      
      if len(trim(rsyy("entryno"))) > 0 then
         entryno=rsyy("entryno") + 1
      else
         entryno=1
      end if
      rsyy.close
     ' set rsyy=nothing
      sqlyy="insert into RTsparqadslcmtysndworklog " _ 
           &"SELECT   cutyid, PRTNO, " & ENTRYNO & ", GETDATE() ,'F','" & V(0) & "',SENDWORKDAT, PRTUSR, ASSIGNENGINEER1, ASSIGNENGINEER2, " _
           &"ASSIGNENGINEER3, ASSIGNENGINEER4, ASSIGNENGINEER5, ASSIGNCONSIGNEE, REALENGINEER1, REALENGINEER2, REALENGINEER3, " _
           &"REALENGINEER4, REALENGINEER5, REALCONSIGNEE, DROPDAT,DROPDESC, CLOSEDAT, BONUSCLOSEYM, BONUSCLOSEDAT, " _
           &"BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, STOCKCLOSEDAT,STOCKCLOSEUSR, STOCKFINCHK, SNDTYPE, HOSTCABLENO, MEMO, " _
           &"PRTDAT, EUSR, EDAT, UUSR, UDAT, CLOSEUSR, DROPUSR, UNCLOSEDAT,finishdat " _
           &"FROM RTsparqadslcmtysndwork where cutyid=" & key(0) & " and prtno='" & key(1) & "' "
     ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update RTsparqadslcmtysndwork set FINISHdat=getdate() ,CLOSEUSR='" & V(0) & "' where cutyid=" & KEY(0)  & " and prtno='" & key(1) & "' "
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM RTsparqadslcmtysndworklog WHERE cutyid=" & key(0) & "' and prtno='" & key(1) & "' AND ENTRYNO=" & ENTRYNO
            CONNXX.Execute sqlyy 
         else
            '完工結案，若為標準工程、連絡纜工程、測通、測通及加裝設備且主線尚未測通者，須更新主線測通日adslapplydat
            IF sndtype ="OC" OR  sndtype ="OP" OR  sndtype ="ST" OR  sndtype ="CB" THEN
               SQLXX=" update RTsparqadslcmty set adslapply=convert(datetime,convert(varchar(10), getdate(),111)) ,UUSR='" & V(0) & "',UDAT=GETDATE()  where cutyid=" & KEY(0) & " AND RTsparqadslcmty.ADSLAPPLY IS NULL "
               STS=3
               connxx.Execute SQLXX
               If Err.number > 0 then
                  endpgm="2"
               '發生錯誤時，刪除異動檔所新增的異動資料
                  errmsg=cstr(Err.number) & "=" & Err.description
                  sqlyy="delete * FROM RTsparqadslcmtysndworklog WHERE cutyid=" & key(0) & "' and prtno='" & key(1) & "' AND ENTRYNO=" & ENTRYNO
                  CONNXX.Execute sqlyy 
               ELSE
                  endpgm="1"
                  errmsg=""
               END IF
           END IF
           '完工結案，若為標準工程時，則一併更新機櫃派工日、設備到位日及安裝工程師
           IF  sndtype ="ST" THEN
               IF LEN(TRIM(RSXX("REALENGINEER1"))) > 0 THEN
                  SETMAN=RSXX("REALENGINEER1")
               ELSE
                  SETMAN=RSXX("REALCONSIGNEE")
               END IF
               SQLXX=" update RTsparqadslcmty set CASESNDWRK=getdate() ,EQUIPARRIVE=GETDATE(),SETENGINEER='" & SETMAN & "'  where cutyid=" & KEY(0) & "  "
               STS=3
               connxx.Execute SQLXX
               If Err.number > 0 then
                  endpgm="2"
               '發生錯誤時，刪除異動檔所新增的異動資料
                  errmsg=cstr(Err.number) & "=" & Err.description
                  sqlyy="delete * FROM RTsparqadslcmtysndworklog WHERE cutyid=" & key(0) & "' and prtno='" & key(1) & "' AND ENTRYNO=" & ENTRYNO
                  CONNXX.Execute sqlyy 
               ELSE
                  endpgm="1"
                  errmsg=""
               END IF
           END IF
         end if      
      end if
'      rsyy.close
   END IF
   SQLYY="SELECT * FROM RTsparqadslcmty WHERE cutyid=" & KEY(0)
   RSYY.Open SQLYY,CONNXX
   COMN=RSYY("COMN")
   RSYY.CLOSE
   IF ENDPGM="1" AND STS="3" THEN '當需更新主線之完工日的派工種類(STS=3)返轉才須要通知相關人員
      FROMEMAIL="MIS@CBBN.COM.TW"
      Set jmail = Server.CreateObject("Jmail.Message")
      jmail.charset="BIG5"
      jmail.from = "MIS@cbbn.com.tw"
      Jmail.fromname="Sparq* 管理系統系統通知"
      IF STS="3" THEN
         jmail.Subject = "社區︰" & COMN & "，派工單︰" & KEY(1) & "，完工結案通知"
      ELSE
         jmail.Subject = "社區︰" & COMN & "，派工單︰" & KEY(1) & "，未完工結案通知"
      END IF
      
      jmail.priority = 1  
      body="<html><body><table border=0 width=""60%""><tr><td colspan=2>" _
      &"<H3 ALIGN=CENTER>速博Sparq* ADSL主線派工結案通知</h3></td></tr>" _
      &"<tr><td bgcolor=lightblue align=center>社區主線序號</td><td bgcolor=pink align=left>" & KEY(0) &"</td></tr>" _
      &"<tr><td bgcolor=lightblue align=center>社區名稱</td><td bgcolor=pink align=left>" &comn &"</td></tr>" _
      &"<tr><td bgcolor=lightblue align=center>派工單號</td><td bgcolor=pink align=left>" &KEY(1) &"</td></tr>" _
      &"</table>" _
      &"<P><U>此主線已完工結案，可執行用戶裝機派工作業</U></body></html>"  
      jmail.HTMLBody = BODY
      'JMail.AddRecipient "anita@cbbn.com.tw","速博Sparq*總窗口"
      JMail.AddRecipient "mis@cbbn.com.tw","資訊部"
      'JMail.AddRecipient "brian@cbbn.com.tw","工務窗口"
      'IF CONSIGNEE="Y" THEN
       '  JMail.AddRecipient "anita@cbbn.com.tw","經銷業助窗口"
        ' JMail.AddRecipient "EDSON@cbbn.com.tw","坡朋"
      'ELSE
      '   IF AREAID="A2" THEN
      '      JMail.AddRecipient "lini@cbbn.com.tw","台中業助"
      '   ELSEIF AREAID="A3" THEN
      '      JMail.AddRecipient "cute0318@cbbn.com.tw","高雄業助"
      '   ELSEIF AREAID="A1" AND (GROUPID="01" OR GROUPID="07") THEN '桃園
      '      JMail.AddRecipient "s525422@cbbn.com.tw","桃園業助"
      '   ELSEIF AREAID="A1" AND (GROUPID="G1" OR GROUPID="G2" OR GROUPID="G3"  OR GROUPID="G4"  OR GROUPID="02" OR GROUPID="03" OR GROUPID="04" OR GROUPID="05" OR GROUPID="06") THEN '台北
      '      JMail.AddRecipient "tiffany01@cbbn.com.tw","台北業助"            
      '   END IF
      'END IF
      jmail.Send ( "118.163.60.171" )      
   END IF      
   RSXX.CLOSE
   connXX.Close
   SET RSXX=NOTHING
   set rsyy=nothing
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "速博ADSL主線派工單完工結案成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "當已作廢時，不可執行完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "當主線完工結案日或未完工結案日不為空白時，不可執行完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="5" then
       msgbox "此主線派工已月結，不可異動" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close      
    elseIF frm1.htmlfld.value="6" then
       msgbox "此主線派工單結案時，必須先輸入實際裝機人員或實際裝機經銷商" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                  
    elseIF frm1.htmlfld.value="7" then
       msgbox "當裝機完工日空白時,不可執行完工或未完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                     
    else
       msgbox "無法執行主線派工單完工結案作業,錯誤訊息" & "  " & frm1.htmlfld1.value
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