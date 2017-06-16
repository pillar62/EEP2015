<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM Conn
   Set Conn=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   SET RSyy=Server.CreateObject("ADODB.RECORDSET")
   DSN="DSN=RtLib"
   Conn.Open DSN
 '  On Error Resume Next
   sqlxx="select * FROM RTLessorAVSCustADJDAY WHERE CUSID='" & KEY(0) & "' AND ENTRYNO=" & KEY(1)
   sqlYY="select * FROM RTLessorAVSCust WHERE CUSID='" & KEY(0) & "' "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,Conn
   RSYY.OPEN SQLYY,Conn
   endpgm="1"
   '當用戶已結案，不可重複結案
   IF LEN(TRIM(RSXX("ADJCLOSEDAT"))) <> 0  THEN
      ENDPGM="3"
   '當用戶已作廢時，不可結案
   elseif LEN(TRIM(RSXX("CANCELdat"))) <> 0 then
      endpgm="4"
   '當用戶主檔資料為已作廢或已退租時，不可結案
   elseif LEN(TRIM(RSYY("CANCELdat"))) <> 0 OR LEN(TRIM(RSYY("DROPDAT"))) <> 0 then
      endpgm="5"      
   '當用戶主檔之期數資料與調整資料之期數相加減結果小於零者，則不允許結案
   elseif RSYY("PERIOD") + RSXX("ADJPERIOD") < 0 then
      endpgm="6"           
   ELSE
     '檢查到期日調整後是否小於開始計費日或續約開始日
     XXDUEDAT=RSYY("DUEDAT")
     XXSTRBILLINGDAT=RSYY("STRBILLINGDAT")
     XXNEWBILLINGDAT=RSYY("NEWBILLINGDAT")
     XXADJPERIOD=RSXX("ADJPERIOD")
     XXADJDAY=RSXX("ADJDAY")
     XX2DUEDAT=DATEADD("m",XXADJPERIOD,XXDUEDAT)
     XX2DUEDAT=DATEADD("d",XXADJdat,XXDUEDAT)
     if len(trim(xxnewbillingdat)) > 0 and XX2DUEDAT < xxnewbillingdat then
        endpgm="7"   
     elseif len(trim(XXSTRBILLINGDAT)) > 0 and XX2DUEDAT < xxstrbillingdat then
        endpgm="7"   
     else
       '呼叫store procedure更新相關檔案
        strSP="usp_RTLessorAVSCustAdjDayF " & "'" & key(0) & "'" & "," & key(1) & ",'" & V(0) & "'" 
'response.write strSP
        Set ObjRS = conn.Execute(strSP)
        If Err.number = 0 then
           ENDPGM="1"
           ERRMSG=""
           'conn.CommitTrans
        else
           ENDPGM="2"
           errmsg=cstr(Err.number) & "=" & Err.description
           'conn.rollbackTrans
        end if         
     end if
   END IF
   RSXX.CLOSE
   RSYY.CLOSE
   Conn.Close
   SET RSXX=NOTHING
   SET RSYY=NOTHING
   set Conn=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "AVS-City用戶調整到期日數資料結案成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "當用戶已結案，不可重複結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "當用戶已作廢時，不可結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close             
    elseIF frm1.htmlfld.value="5" then
       msgbox "當用戶主檔資料為已作廢或已退租時，不可結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                
    elseIF frm1.htmlfld.value="6" then
       msgbox "當用戶主檔之期數資料與調整資料之期數相加減結果小於零者，則不允許結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close          
    elseIF frm1.htmlfld.value="7" then
       msgbox "當用戶主檔之到期日經此調整資料調整後其結果會小於開始計費日(或續約開始日)，<br>因為不允許結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                
    else
       msgbox "無法執行用戶調整到期日數資料結案作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTLessorAVSCustADJDAYF.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>