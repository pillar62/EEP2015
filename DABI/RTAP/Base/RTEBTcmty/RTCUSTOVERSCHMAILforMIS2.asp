<%@ Language=VBScript %>
<% 
   Set conn=Server.CreateObject("ADODB.Connection")  
   SET RS=Server.CreateObject("ADODB.RECORDSET")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")
   DSN="DSN=RtLib"
   conn.Open DSN
   '經銷
   sql="SELECT  count(*) AS RSCOUNT " _
        &"FROM            RTEBTCUSTSNDWORK INNER JOIN " _
        &"                RTEBTCUST ON RTEBTCUSTSNDWORK.COMQ1 = RTEBTCUST.COMQ1 AND " _
        &"                RTEBTCUSTSNDWORK.LINEQ1 = RTEBTCUST.LINEQ1 AND " _
        &"                RTEBTCUSTSNDWORK.CUSID = RTEBTCUST.CUSID INNER JOIN " _
        &"                RTEBTCMTYLINE ON RTEBTCUST.COMQ1 = RTEBTCMTYLINE.COMQ1 AND " _
        &"                RTEBTCUST.LINEQ1 = RTEBTCMTYLINE.LINEQ1 INNER JOIN " _
        &"                RTEBTCMTYH ON " _
        &"                RTEBTCMTYLINE.COMQ1 = RTEBTCMTYH.COMQ1  LEFT OUTER JOIN " _
        &"                RTObj RTObj_2 ON " _
        &"                RTEBTCUSTSNDWORK.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN " _
        &"                RTObj RTObj_1 INNER JOIN " _
        &"                RTEmployee ON RTObj_1.CUSID = RTEmployee.CUSID ON " _
        &"                RTEBTCUSTSNDWORK.ASSIGNENGINEER = RTEmployee.EMPLY LEFT OUTER JOIN " _
        &"                RTObj RTObj_3 ON " _
        &"                RTEBTCMTYLINE.CONSIGNEE = RTObj_3.CUSID LEFT OUTER JOIN " _
        &"                RTArea INNER JOIN " _
        &"                RTSalesGroup ON RTArea.AREAID = RTSalesGroup.COMPLOCATION ON " _
        &"                RTEBTCUST.AREAID = RTSalesGroup.AREAID AND " _
        &"                RTEBTCUST.GROUPID = RTSalesGroup.GROUPID " _
        &"WHERE           (RTEBTCUSTSNDWORK.DROPDAT IS NULL) AND (RTEBTCUST.CANCELDAT IS NULL) and (RTEBTCUST.freecode <> 'Y' ) and " _
        &"                (RTEBTCUSTSNDWORK.UNCLOSEDAT IS NULL) AND " _
        &"                (RTEBTCUSTSNDWORK.CLOSEDAT IS NULL) AND (DATEDIFF(day, RTEBTCUSTSNDWORK.SENDWORKDAT, GETDATE()) > 6) AND LEN(LTRIM(RTOBJ_3.SHORTNC)) > 0 " 
   RS.Open SQL,CONN
   rscount1=rs("RSCOUNT")
   RS.CLOSE
   sql="SELECT  CASE WHEN RTOBJ_3.SHORTNC IS NULL OR RTOBJ_3.SHORTNC ='' THEN RTAREA.AREANC ELSE RTOBJ_3.SHORTNC END AS 轄區, " _
        &"CASE WHEN RTOBJ_2.SHORTNC IS NULL OR RTOBJ_2.SHORTNC = '' THEN RTOBJ_1.CUSNC ELSE RTOBJ_2.SHORTNC END as 預定裝機人員, " _
        &"RTEBTCUSTSNDWORK.SENDWORKDAT as 派工日期, RTEBTCUSTSNDWORK.PRTNO as 派工單號,RTEBTCMTYH.COMN as 社區名稱,RTEBTCUST.CUSNC as 用戶名稱, " _
        &"DATEDIFF(day, RTEBTCUSTSNDWORK.SENDWORKDAT, GETDATE()) as 已派工日數 " _
        &"FROM            RTEBTCUSTSNDWORK INNER JOIN " _
        &"                RTEBTCUST ON RTEBTCUSTSNDWORK.COMQ1 = RTEBTCUST.COMQ1 AND " _
        &"                RTEBTCUSTSNDWORK.LINEQ1 = RTEBTCUST.LINEQ1 AND " _
        &"                RTEBTCUSTSNDWORK.CUSID = RTEBTCUST.CUSID INNER JOIN " _
        &"                RTEBTCMTYLINE ON RTEBTCUST.COMQ1 = RTEBTCMTYLINE.COMQ1 AND " _
        &"                RTEBTCUST.LINEQ1 = RTEBTCMTYLINE.LINEQ1 INNER JOIN " _
        &"                RTEBTCMTYH ON " _
        &"                RTEBTCMTYLINE.COMQ1 = RTEBTCMTYH.COMQ1  LEFT OUTER JOIN " _
        &"                RTObj RTObj_2 ON " _
        &"                RTEBTCUSTSNDWORK.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN " _
        &"                RTObj RTObj_1 INNER JOIN " _
        &"                RTEmployee ON RTObj_1.CUSID = RTEmployee.CUSID ON " _
        &"                RTEBTCUSTSNDWORK.ASSIGNENGINEER = RTEmployee.EMPLY LEFT OUTER JOIN " _
        &"                RTObj RTObj_3 ON " _
        &"                RTEBTCMTYLINE.CONSIGNEE = RTObj_3.CUSID LEFT OUTER JOIN " _
        &"                RTArea INNER JOIN " _
        &"                RTSalesGroup ON RTArea.AREAID = RTSalesGroup.COMPLOCATION ON " _
        &"                RTEBTCUST.AREAID = RTSalesGroup.AREAID AND " _
        &"                RTEBTCUST.GROUPID = RTSalesGroup.GROUPID " _
        &"WHERE           (RTEBTCUSTSNDWORK.DROPDAT IS NULL) AND (RTEBTCUST.CANCELDAT IS NULL) and (RTEBTCUST.freecode <> 'Y' ) and  " _
        &"                (RTEBTCUSTSNDWORK.UNCLOSEDAT IS NULL) AND " _
        &"                (RTEBTCUSTSNDWORK.CLOSEDAT IS NULL) AND (DATEDIFF(day, RTEBTCUSTSNDWORK.SENDWORKDAT, GETDATE()) > 6) AND LEN(LTRIM(RTOBJ_3.SHORTNC)) > 0 " _
        &"order by  CASE WHEN RTOBJ_3.SHORTNC IS NULL OR RTOBJ_3.SHORTNC ='' THEN RTAREA.AREANC ELSE RTOBJ_3.SHORTNC END, " _
        &"CASE WHEN RTOBJ_2.SHORTNC IS NULL OR RTOBJ_2.SHORTNC = '' THEN RTOBJ_1.CUSNC ELSE RTOBJ_2.SHORTNC END ,RTEBTCMTYH.COMN, RTEBTCUSTSNDWORK.SENDWORKDAT "
   RS.Open SQL,CONN
   CNT=0
   dim setupman(200)
   DIM SETUPMAN2(200)
   Set jmail = Server.CreateObject("Jmail.Message")
   jmail.charset="BIG5"
   jmail.from = "MIS@cbbn.com.tw"
   Jmail.fromname="東森AVS系統自動警示通知"
   jmail.Subject = "AVS用戶裝機超過6天以上未結案明細"
   jmail.priority = 1  
   IF NOT RS.EOF THEN
      body="<html><body><table border=1 width=""80%""> " 
      DO until RS.EOF
         CNT=CNT+1
         FROMEMAIL="MIS@CBBN.COM.TW"
         IF CNT=1 THEN  
            BODY=BODY & "<tr><H3>東森AVS用戶裝機超過6天未結案通知--經銷</h3></td></tr>" _
                &"<tr><td bgcolor=lightblue align=center>轄區</td><td bgcolor=lightblue align=center>預定裝機人員</td>"_
                &"<td bgcolor=lightblue align=center>派工日期</td><td bgcolor=lightblue align=center>派工單號</td>"_
                &"<td bgcolor=lightblue align=center>社區名稱</td><td bgcolor=lightblue align=center>用戶名稱</td>"_
                &"<td bgcolor=lightblue align=center>已派工日數</td></tr>"
         END IF
         BODY=BODY & "<tr>" _
             &"<td bgcolor=pink align=left>" &RS("轄區") &"</td>" _
             &"<td bgcolor=pink align=left>" &RS("預定裝機人員")  &"</td>" _
             &"<td bgcolor=pink align=left>" &RS("派工日期")&"</td>" _
             &"<td bgcolor=pink align=left>" &RS("派工單號")&"</td>" _
             &"<td bgcolor=pink align=left>" &RS("社區名稱")&"</td>" _
             &"<td bgcolor=pink align=left>" &RS("用戶名稱")&"</td>" _
             &"<td bgcolor=pink align=left>" &RS("已派工日數")&"</td></TR>" 
         RS.MoveNext
       loop
       body=body & "<tr><td colspan=7>共計 " & rscount1 & " 筆 </td></tr>"
       BODY=BODY & "</table><P>"
   END IF    
   rs.close  
   '直銷
   sql="SELECT  COUNT(*) AS RSCOUNT " _
        &"FROM            RTEBTCUSTSNDWORK INNER JOIN " _
        &"                RTEBTCUST ON RTEBTCUSTSNDWORK.COMQ1 = RTEBTCUST.COMQ1 AND " _
        &"                RTEBTCUSTSNDWORK.LINEQ1 = RTEBTCUST.LINEQ1 AND " _
        &"                RTEBTCUSTSNDWORK.CUSID = RTEBTCUST.CUSID INNER JOIN " _
        &"                RTEBTCMTYLINE ON RTEBTCUST.COMQ1 = RTEBTCMTYLINE.COMQ1 AND " _
        &"                RTEBTCUST.LINEQ1 = RTEBTCMTYLINE.LINEQ1 INNER JOIN " _
        &"                RTEBTCMTYH ON " _
        &"                RTEBTCMTYLINE.COMQ1 = RTEBTCMTYH.COMQ1  LEFT OUTER JOIN " _
        &"                RTObj RTObj_2 ON " _
        &"                RTEBTCUSTSNDWORK.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN " _
        &"                RTObj RTObj_1 INNER JOIN " _
        &"                RTEmployee ON RTObj_1.CUSID = RTEmployee.CUSID ON " _
        &"                RTEBTCUSTSNDWORK.ASSIGNENGINEER = RTEmployee.EMPLY LEFT OUTER JOIN " _
        &"                RTObj RTObj_3 ON " _
        &"                RTEBTCMTYLINE.CONSIGNEE = RTObj_3.CUSID LEFT OUTER JOIN " _
        &"                RTArea INNER JOIN " _
        &"                RTSalesGroup ON RTArea.AREAID = RTSalesGroup.COMPLOCATION ON " _
        &"                RTEBTCUST.AREAID = RTSalesGroup.AREAID AND " _
        &"                RTEBTCUST.GROUPID = RTSalesGroup.GROUPID " _
        &"WHERE           (RTEBTCUSTSNDWORK.DROPDAT IS NULL) AND (RTEBTCUST.CANCELDAT IS NULL) and (RTEBTCUST.freecode <> 'Y' ) and " _
        &"                (RTEBTCUSTSNDWORK.UNCLOSEDAT IS NULL) AND " _
        &"                (RTEBTCUSTSNDWORK.CLOSEDAT IS NULL) AND (DATEDIFF(day, RTEBTCUSTSNDWORK.SENDWORKDAT, GETDATE()) > 6) AND (LEN(LTRIM(RTOBJ_3.SHORTNC)) = 0 or rtobj_3.shortnc is null) " 
   RS.Open SQL,CONN
   rscount2=RS("RSCOUNT")
   RS.CLOSE
   sql="SELECT  CASE WHEN RTOBJ_3.SHORTNC IS NULL OR RTOBJ_3.SHORTNC ='' THEN RTAREA.AREANC ELSE RTOBJ_3.SHORTNC END AS 轄區, " _
        &"CASE WHEN RTOBJ_2.SHORTNC IS NULL OR RTOBJ_2.SHORTNC = '' THEN RTOBJ_1.CUSNC ELSE RTOBJ_2.SHORTNC END as 預定裝機人員, " _
        &"RTEBTCUSTSNDWORK.SENDWORKDAT as 派工日期, RTEBTCUSTSNDWORK.PRTNO as 派工單號,RTEBTCMTYH.COMN as 社區名稱,RTEBTCUST.CUSNC as 用戶名稱, " _
        &"DATEDIFF(day, RTEBTCUSTSNDWORK.SENDWORKDAT, GETDATE()) as 已派工日數,rtemployee.email as 信箱 " _
        &"FROM            RTEBTCUSTSNDWORK INNER JOIN " _
        &"                RTEBTCUST ON RTEBTCUSTSNDWORK.COMQ1 = RTEBTCUST.COMQ1 AND " _
        &"                RTEBTCUSTSNDWORK.LINEQ1 = RTEBTCUST.LINEQ1 AND " _
        &"                RTEBTCUSTSNDWORK.CUSID = RTEBTCUST.CUSID INNER JOIN " _
        &"                RTEBTCMTYLINE ON RTEBTCUST.COMQ1 = RTEBTCMTYLINE.COMQ1 AND " _
        &"                RTEBTCUST.LINEQ1 = RTEBTCMTYLINE.LINEQ1 INNER JOIN " _
        &"                RTEBTCMTYH ON " _
        &"                RTEBTCMTYLINE.COMQ1 = RTEBTCMTYH.COMQ1  LEFT OUTER JOIN " _
        &"                RTObj RTObj_2 ON " _
        &"                RTEBTCUSTSNDWORK.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN " _
        &"                RTObj RTObj_1 INNER JOIN " _
        &"                RTEmployee ON RTObj_1.CUSID = RTEmployee.CUSID ON " _
        &"                RTEBTCUSTSNDWORK.ASSIGNENGINEER = RTEmployee.EMPLY LEFT OUTER JOIN " _
        &"                RTObj RTObj_3 ON " _
        &"                RTEBTCMTYLINE.CONSIGNEE = RTObj_3.CUSID LEFT OUTER JOIN " _
        &"                RTArea INNER JOIN " _
        &"                RTSalesGroup ON RTArea.AREAID = RTSalesGroup.COMPLOCATION ON " _
        &"                RTEBTCUST.AREAID = RTSalesGroup.AREAID AND " _
        &"                RTEBTCUST.GROUPID = RTSalesGroup.GROUPID " _
        &"WHERE           (RTEBTCUSTSNDWORK.DROPDAT IS NULL) AND  (RTEBTCUST.CANCELDAT IS NULL) and (RTEBTCUST.freecode <> 'Y' ) and " _
        &"                (RTEBTCUSTSNDWORK.UNCLOSEDAT IS NULL) AND " _
        &"                (RTEBTCUSTSNDWORK.CLOSEDAT IS NULL) AND (DATEDIFF(day, RTEBTCUSTSNDWORK.SENDWORKDAT, GETDATE()) > 6) AND (LEN(LTRIM(RTOBJ_3.SHORTNC)) = 0 or rtobj_3.shortnc is null) " _
        &"order by  CASE WHEN RTOBJ_3.SHORTNC IS NULL OR RTOBJ_3.SHORTNC ='' THEN RTAREA.AREANC ELSE RTOBJ_3.SHORTNC END, " _
        &"CASE WHEN RTOBJ_2.SHORTNC IS NULL OR RTOBJ_2.SHORTNC = '' THEN RTOBJ_1.CUSNC ELSE RTOBJ_2.SHORTNC END ,RTEBTCMTYH.COMN, RTEBTCUSTSNDWORK.SENDWORKDAT "
   RS.Open SQL,CONN
   CNT=0
   i=0
   dim ary(10)
   IF NOT RS.EOF THEN
      body=body & "<p><table border=1 width=""80%""> " 
      prvman=""
      DO until RS.EOF
         if rs("轄區")="台北" then ary(0)=1
         if rs("轄區")="桃園" then ary(1)=1
         if rs("轄區")="台中" then ary(2)=1
         if rs("轄區")="高雄" then ary(3)=1
         CNT=CNT+1
         FROMEMAIL="MIS@CBBN.COM.TW"
         IF CNT=1 THEN  
            BODY=BODY & "<tr><H3>東森AVS用戶裝機超過6天未結案通知--直銷</h3></td></tr>" _
                &"<tr><td bgcolor=lightblue align=center>轄區</td><td bgcolor=lightblue align=center>預定裝機人員</td>"_
                &"<td bgcolor=lightblue align=center>派工日期</td><td bgcolor=lightblue align=center>派工單號</td>"_
                &"<td bgcolor=lightblue align=center>社區名稱</td><td bgcolor=lightblue align=center>用戶名稱</td>"_
                &"<td bgcolor=lightblue align=center>已派工日數</td></tr>"
         END IF
         BODY=BODY & "<tr>" _
             &"<td bgcolor=pink align=left>" &RS("轄區") &"</td>" _
             &"<td bgcolor=pink align=left>" &RS("預定裝機人員")  &"</td>" _
             &"<td bgcolor=pink align=left>" &RS("派工日期")&"</td>" _
             &"<td bgcolor=pink align=left>" &RS("派工單號")&"</td>" _
             &"<td bgcolor=pink align=left>" &RS("社區名稱")&"</td>" _
             &"<td bgcolor=pink align=left>" &RS("用戶名稱")&"</td>" _
             &"<td bgcolor=pink align=left>" &RS("已派工日數")&"</td></TR>" 
         
         RS.MoveNext
       LOOP  
       I=0
       body=body & "<tr><td colspan=7>共計 " & rscount2 & " 筆 </td></tr>"
       BODY=BODY & "</table><P><U>請追蹤用戶裝機進度</U></body></html>"  
       jmail.HTMLBody = BODY
       JMail.AddRecipient "mis@cbbn.com.tw","資訊部"
    '   JMail.AddRecipient "andyjkuo@cbbn.com.tw","郭協理"
    '   JMail.AddRecipient "jimmy@cbbn.com.tw","陳祈良主任"
    '   JMail.AddRecipient "kowei@cbbn.com.tw","汪可威主任"
    '   JMail.AddRecipient "jacqueline@cbbn.com.tw","經銷業助窗口"
    '   JMail.AddRecipient "tiffany01@cbbn.com.tw","經銷業助窗口"
   END IF         
   IF RSCOUNT1 > 0 OR RSCOUNT2 > 0 THEN
      IF RSCOUNT2 > 0 THEN
         if ary(0)=1 or ary(1)=1 then
    '        JMail.AddRecipient "thomas@cbbn.com.tw","鄭榮仁主任"
         end if
         if ary(0)=1 then
    '        JMail.AddRecipient "monica@cbbn.com.tw","台北業助"
         end if
         if ary(1)=1 then
    '        JMail.AddRecipient "lilu@cbbn.com.tw","桃園業助"
         end if         
         if ary(2)=1 then
    '        JMail.AddRecipient "rom@cbbn.com.tw","蔡銘裕主任"
    '        JMail.AddRecipient "elaine@cbbn.com.tw","台中業助"
    '        JMail.AddRecipient "sally@cbbn.com.tw","台中業助"
         end if                  
         if ary(3)=1 then
    '        JMail.AddRecipient "joe@cbbn.com.tw","鍾大慶組長"
    '        JMail.AddRecipient "feng@cbbn.com.tw","高雄業助"
         end if                        
      END IF
      jmail.Send ( "219.87.146.239" )   
   END IF
   rs.Close
   conn.Close
   set rs=nothing
   set conn=nothing
%>
