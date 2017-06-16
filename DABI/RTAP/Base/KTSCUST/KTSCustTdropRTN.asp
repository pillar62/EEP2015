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
 '  On Error Resume Next
      sqlxx="select * FROM ktscustd1 WHERE CUSID='" & key(0) & "' and entryno=" & key(1) & " "
      RSXX.OPEN SQLXX,CONNxx
   '尚未作廢，不可返轉
      IF ISNULL(RSXX("CANCELdat")) THEN
         ENDPGM="3"
      ELSE
         sqlyy="select max(SEQ) as SEQ FROM KTSCUSTD1LOG WHERE CUSID='" & key(0) & "' and ENTRYNO=" & key(1) 
         rsyy.Open sqlyy,connxx
      
         if len(trim(rsyy("SEQ"))) > 0 then
            SEQ=rsyy("SEQ") + 1
         else
            SEQ=1
         end if
         rsyy.close
         set rsyy=nothing
         sqlyy="insert into KTSCUSTD1log " _
              &"SELECT  CUSID, ENTRYNO," & SEQ & ", getdate(), 'R','" &  v(0) & "', " _
              &" TEL11, TEL12, APPLYDAT, OPENDAT, DROPDAT, ENDDAT, APPLYNO, DROPNO, CANCELDAT " _
              &"FROM KTSCUSTD1 where CUSID='" & key(0) & "' and ENTRYNO=" & key(1) 
     ' Response.Write sqlyy
         CONNXX.Execute sqlyy     
         If Err.number > 0 then
            endpgm="2"
            errmsg=cstr(Err.number) & "=" & Err.description
         else
            SQLXX=" update KTSCUSTD1 set CANCELdat=NULL where CUSID='" & key(0) & "' and ENTRYNO=" & key(1) 
            connxx.Execute SQLXX
            If Err.number > 0 then
               endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
               errmsg=cstr(Err.number) & "=" & Err.description
               sqlyy="delete * FROM KTSCUSTD1log WHERE CUSID='" & key(0) & "' and ENTRYNO=" & key(1) & " AND SEQ=" & SEQ
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
       msgbox "KTS電話作廢返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "電話尚未作廢，不可執行作廢返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    else
       msgbox "無法執行KTS電話作廢返轉作業,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=ktscusttdrop.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>