<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM conn
   Set conn=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   SET RSyy=Server.CreateObject("ADODB.RECORDSET")
   SET RSZZ=Server.CreateObject("ADODB.RECORDSET")
   DSN="DSN=RtLib"
   conn.Open DSN
   ' conn.BEGINTrans(改由STORE PROCEDURE內執行TRANSACTION、COMMIT、ROLLBACK)
   '因為當STORE PROCEDURE內OPEN太多TABLE時，ASP無法控制CURSOR而會發生錯誤(不明瞭可以將BEGIN、COMMIT、ROLLBACK的MARK移除並執行後即知)
   sqlxx="select * FROM RTLessorCMTYLINEsndwork WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " and prtno='" & key(2) & "' "
   sqlYY="select * FROM RTLessorCMTYLINE WHERE comq1=" & KEY(0) & " and lineq1=" & key(1)
   sqlZZ="select COUNT(*) AS CNT FROM RTLessorCUST WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND canceldat is null and dropdat is null and finishdat is not null "
   RSxx.Open SQLxx,conn
   RSYY.Open SQLYY,conn
   RSzz.Open SQLzz,conn
   endpgm="1"
  '當已作廢時，不可執行完工結案或未完工結案
   IF not isnull(RSXX("DROPDAT")) THEN
      ENDPGM="4"
   elseif isnull(RSXX("CLOSEDAT")) and isnull(RSXX("UNCLOSEDAT")) then
      endpgm="3"
   '找不到主線主檔資料      
   elseif rsyy.eof then
      endpgm="6"      
   '結案返轉時，若主線檔之狀態並非已有測通日，則表示資料有異常      
   elseif rsxx("sndkind")="ST"  AND ( ISNULL(RSYY("ADSLAPPLYDAT")) AND ISNULL(RSYY("CONTAPPLYDAT")) ) THEN
      endpgm="7"    
   ELSEIF RSZZ("CNT") > 0 AND rsxx("sndkind")="ST" THEN
      endpgm="8"   
   ELSE
      '呼叫store procedure更新相關檔案
      strSP="usp_RTLessorCmtylineSndworkFR " & key(0) & "," & key(1) & ",'" & key(2) & "','" & V(0) & "'" 
     ' response.write strSP
      Set ObjRS = conn.Execute(strSP)
      If Err.number = 0 then
         ENDPGM="1"
         ERRMSG=""
      else
         ENDPGM="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      end if         
   END IF
   RSXX.CLOSE
   RSyy.CLOSE
   RSzz.CLOSE
   conn.Close
   SET RSXX=NOTHING
   SET RSYY=NOTHING
   SET RSzz=NOTHING
   set conn=nothing
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "ET-City主線派工單完工/未完工結案返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此主線派工單尚未結案，不可執行結案返轉作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此主線派工單已作廢，不可執行結案返轉作業" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="6" then
       msgbox "無法找到此主線派工單之主檔資料，請確認ET-City主線主檔資料正常" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="7" then
       msgbox "主線檔目前的狀態並非[已測通]，此派工單種類為[標準工程]之返轉，因此無法執行。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close             
    elseIF frm1.htmlfld.value="8" then
       msgbox "此派工單所屬主線已有已完工之用戶，因此不能執行[標準工程]返轉作業。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                        
    else
       msgbox "無法執行主線派工單完工結案返轉作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTLessorcmtylinesndworkfr.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>