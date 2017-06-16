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
   SET RSzz=Server.CreateObject("ADODB.RECORDSET")
   DSN="DSN=RtLib"
   Conn.Open DSN
 '  On Error Resume Next
   sqlxx="select * FROM RTLessorCustADJDAY WHERE CUSID='" & KEY(0) & "' AND ENTRYNO=" & KEY(1)
   sqlYY="select * FROM RTLessorCust WHERE CUSID='" & KEY(0) & "' "
   sqlzz="select count(*) as cnt FROM RTLessorCustADJDAY WHERE CUSID='" & KEY(0) & "' AND canceldat is null and adjclosedat is null "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,Conn
   RSYY.OPEN SQLYY,Conn
   RSzz.OPEN SQLzz,Conn
   endpgm="1"
   '當用戶尚未結案，不可結案返轉
   IF isnull(RSXX("ADJCLOSEDAT")) THEN
      ENDPGM="3"
   '當用戶已作廢時，不可結案返轉
   elseif LEN(TRIM(RSXX("CANCELdat"))) <> 0 then
      endpgm="4"
   '當用戶主檔資料為已作廢或已退租時，不可結案返轉
   elseif LEN(TRIM(RSYY("CANCELdat"))) <> 0 OR LEN(TRIM(RSYY("DROPDAT"))) <> 0 then
      endpgm="5"      
   '當用戶主檔之期數資料與調整資料之期數相加減結果小於零者，則不允許結案
   elseif RSYY("PERIOD") - RSXX("ADJPERIOD") < 0 then
      endpgm="6"           
   '當已存在其它調整資料時，不可結案返轉
   elseif RSYY("PERIOD") - RSXX("ADJPERIOD") < 0 then
      endpgm="7"                 
   ELSE
     '檢查到期日調整後是否小於開始計費日或續約開始日
     XXDUEDAT=RSYY("DUEDAT")
     XXSTRBILLINGDAT=RSYY("STRBILLINGDAT")
     XXNEWBILLINGDAT=RSYY("NEWBILLINGDAT")
     XXADJPERIOD=RSXX("ADJPERIOD")
     XXADJDAY=RSXX("ADJDAY")
     XX2DUEDAT=DATEADD("m",0-XXADJPERIOD,XXDUEDAT)
     XX2DUEDAT=DATEADD("d",0-XXADJdat,XXDUEDAT)
     if len(trim(xxnewbillingdat)) > 0 and XX2DUEDAT < xxnewbillingdat then
        endpgm="7"   
     elseif len(trim(XXSTRBILLINGDAT)) > 0 and XX2DUEDAT < xxstrbillingdat then
        endpgm="7"   
     else
       '呼叫store procedure更新相關檔案
        strSP="usp_RTLessorCustAdjDayFR " & "'" & key(0) & "'" & "," & key(1) & ",'" & V(0) & "'" 
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
   RSzz.CLOSE
   Conn.Close
   SET RSXX=NOTHING
   SET RSYY=NOTHING
   SET RSzz=NOTHING
   set Conn=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "ET-City用戶調整到期日數資料結案返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "當用戶尚未結案，不可結返返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "當用戶已作廢時，不可結案返轉。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close             
    elseIF frm1.htmlfld.value="5" then
       msgbox "當用戶主檔資料為已作廢或已退租時，不可結案返轉。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                
    elseIF frm1.htmlfld.value="6" then
       msgbox "當用戶主檔之期數資料與調整資料之期數相加減結果小於零者，則不允許結案返轉。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close          
    elseIF frm1.htmlfld.value="7" then
       msgbox "當用戶主檔之到期日經此調整資料調整後其結果會小於開始計費日(或續約開始日)，<br>因為不允許結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                
    else
       msgbox "無法執行用戶調整到期日數資料結案返轉作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTLessorCustADJDAYFR.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>