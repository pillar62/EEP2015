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
   sqlxx="select * FROM hbcmtyarrangesndwork WHERE comq1=" & KEY(0) & " and comtype='" & key(1)  & "' and prtno='" & key(2) & "' "
   'IF1S
   IF ENDPGM="" THEN
      'RESPONSE.Write SQLXX
   '  RESPONSE.END
     RSXX.OPEN SQLXX,CONNxx
     endpgm="1"
     '當已作廢時，不可執行完工結案或未完工結案返轉
     'IF2S
     IF not isnull(RSXX("DROPDAT")) THEN
        ENDPGM="4"
     elseif LEN(TRIM(RSXX("BONUSCLOSEYM"))) <> 0 OR LEN(TRIM(RSXX("STOCKCLOSEYM"))) <> 0 OR LEN(TRIM(RSXX("AUDITDAT"))) <> 0 then
        endpgm="5"
     elseif isnull(RSXX("CLOSEDAT")) then
   '  response.Write "aaa"
   '  response.end
        endpgm="3"      
     ELSE
        sqlyy="select max(entryno) as entryno FROM hbcmtyarrangesndworklog WHERE comq1=" & KEY(0) & " and comtype='" & key(1)  & "' and prtno='" & key(2) & "' "
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
        sqlyy="insert into hbcmtyarrangesndworklog " _
           &"SELECT    COMQ1, COMTYPE, PRTNO, " & ENTRYNO & ", getdate(), 'FR','" &  v(0) & "', " _
           &" SNDDAT, ASSIGNENGINEER, ASSIGNCONSIGNEE, REALENGINEER, REALCONSIGNEE, DROPDAT, " _
           &" CLOSEDAT, EUSR, EDAT, UUSR, UDAT, CLOSEUSR, DROPUSR, EQUIPCUTID, EQUIPTOWNSHIP, EQUIPADDR, ERZONE, " _
           &" PRTDAT, PRTUSR, MEMO, BONUSCLOSEYM, BONUSCLOSEDAT, BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, " _
           &" STOCKCLOSEDAT, STOCKCLOSEUSR, STOCKFINCHK " _
           &" FROM hbcmtyarrangesndwork where comq1=" & key(0) & " and comtype='" & key(1) & "' and prtno='" & key(2) & "' "
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
              SQLXX=" update hbcmtyarrangesndwork set CLOSEdat=NULL,CLOSEUSR='' where comq1=" & key(0) & " and comtype='" & key(1) & "' and prtno='" & key(2) & "' "
              connxx.Execute SQLXX
              'IF5-1S
              If Err.number > 0 then
                 endpgm="2"
                '發生錯誤時，刪除異動檔所新增的異動資料
                 errmsg=cstr(Err.number) & "=" & Err.description
                 sqlyy="delete * FROM hbcmtyarrangesndworklog where comq1=" & key(0) & " and comtype='" & key(1) & "' and prtno='" & key(2) & "' AND ENTRYNO=" & ENTRYNO
                 CONNXX.Execute sqlyy 
                 endpgm="1"
                 errmsg=""
              end if      
              'IF5-1E
             END IF
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
       msgbox "社區整線派工單完工結案返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此整線派工單尚未結案，不可執行結案返轉作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此整線派工單已作廢，不可執行結案返轉作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="5" then
       msgbox "此整線派工單已月結或已審核(請審核人員解除審核控制)，不可異動" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    else
       msgbox "無法執行整線工單完工結案返轉作業,錯誤訊息" & "  " & frm1.htmlfld1.value
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