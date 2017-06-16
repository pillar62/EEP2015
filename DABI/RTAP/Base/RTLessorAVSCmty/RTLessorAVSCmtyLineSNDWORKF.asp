<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONN
   Set conn=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   DSN="DSN=RtLib"
   conn.Open DSN
   'conn.BeginTrans(改由STORE PROCEDURE內執行TRANSACTION、COMMIT、ROLLBACK)
   '因為當STORE PROCEDURE內OPEN太多TABLE時，ASP無法控制CURSOR而會發生錯誤(不明瞭可以將BEGIN、COMMIT、ROLLBACK的MARK移除並執行後即知)
   '檢查客戶資料
   sqlxx="select * FROM RTLessorAVSCmtyline WHERE comq1=" & KEY(0) & " and lineq1=" & KEY(1)
   rsxx.open sqlxx,conn
   if rsxx.eof then
      '找不到主線基本檔
      endpgm="7"
   elseif LEN(TRIM(RSXX("DROPDAT"))) <> 0 or LEN(TRIM(RSXX("cancelDAT"))) <> 0 then
      '主線已退租或作廢，不可進行主線派工結案
      endpgm="8"
  else
      xxadslapplydat=rsxx("adslapplydat")    
      xxCONTAPPLYDAT=rsxx("CONTAPPLYDAT")   
   end if
   rsxx.close
   '檢查該派工單下的設備是否皆已辦妥物品領用程序
   sqlxx="select count(*) as CNT FROM RTLessorAVScmtylineHardware WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " and prtno='" & key(2) & "' and dropdat is null and rcvfinishdat is null "
   RSXX.OPEN SQLXX,CONN
   IF RSXX.EOF THEN
   ELSEIF RSXX("CNT") > 0 THEN
      ENDPGM="12"
   END IF
   RSXX.CLOSE
   '檢查該派工單下的設備是否皆已辦妥物品領用程序
   sqlxx="select count(*) as CNT FROM RTLessorAVScmtylineHardware WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " and prtno='" & key(2) & "' and dropdat is null "
   RSXX.OPEN SQLXX,CONN
   IF RSXX.EOF OR RSXX("CNT") < 1 THEN
      ENDPGM="13"
   END IF
   RSXX.CLOSE   
'上述正確時
if endpgm="" then
   endpgm="1"
  
   sqlxx="select * FROM RTLessorAVSCmtylineSndwork WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " and prtno='" & key(2) & "' "
   RSXX.OPEN SQLXX,CONN
   '當已作廢時，不可執行完工結案或未完工結案
   IF LEN(TRIM(RSXX("DROPDAT"))) <> 0 THEN
      ENDPGM="3"
   elseif LEN(TRIM(RSXX("CLOSEDAT"))) <> 0 OR LEN(TRIM(RSXX("UNCLOSEDAT"))) <> 0 then
      endpgm="4"
   elseif LEN(TRIM(RSXX("REALENGINEER"))) = 0 AND LEN(TRIM(RSXX("REALCONSIGNEE"))) = 0 then
      endpgm="6"
   '標準工程結案時，設備安裝日及主線測通日不可空白
   elseif (isnull(RSXX("Equipsetupdat")) or isnull(rsxx("adslapplydat"))) AND RSXX("sndkind")="ST" then
      endpgm="9"      
   elseif (NOT isnull(RSXX("Equipsetupdat")) or NOT isnull(rsxx("adslapplydat"))) AND RSXX("sndkind")<>"ST" then
      endpgm="10"            
   '主線已測通時，不可再派"標準工程"項目的派工單
   elseif (LEN(TRIM(xxadslapplydat)) <> 0 or LEN(TRIM(xxCONTAPPLYDAT)) <> 0 )and len(trim("SNDKIND"))="ST"  THEN
      endpgm="5"
   ELSE
      '呼叫store procedure更新相關檔案
      strSP="usp_RTLessorAVScmtylineSndworkF "  & key(0) & "," & key(1) & ",'" & key(2) & "','" & V(0) & "'"
    '  response.write strSP
    '  response.end     
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
   END IF
   RSXX.CLOSE
end if
conn.Close
SET RSXX=NOTHING
set conn=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "AVS-City主線派工單完工結案成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "當已作廢時，不可執行完工結案或未完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "此主線派工單已完工結案或未完工結案，不可重複執行完工結案或未完工結案" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="5" then
       msgbox "此主線已測通，不可再派[主線測通]的派工單。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close               
    elseIF frm1.htmlfld.value="6" then
       msgbox "此主線派工單完工時，必須先輸入實際裝機人員或實際裝機經銷商" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                  
    elseIF frm1.htmlfld.value="7" then
       msgbox "找不到主線基本檔，無法結案。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
    elseIF frm1.htmlfld.value="8" then
       msgbox "主線已退租或作廢，無法結案(派工單必須作廢)。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
    elseIF frm1.htmlfld.value="9" then
       msgbox "標準工程結案時，設備安裝到位日及主線測通日不可空白。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close          
    elseIF frm1.htmlfld.value="10" then
       msgbox "非標準工程結案時，設備安裝到位日及主線測通日必須空白。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                     
    elseIF frm1.htmlfld.value="12" then
       msgbox "此主線派工單設備資料中，尚有設備未辦妥物品領用程序，不可執行完工結案作業。" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
    elseIF frm1.htmlfld.value="13" then
       msgbox "此主線派工單未建立主線設備資料，不可執行完工結案作業。" & "  " & frm1.htmlfld1.value
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
<form name=frm1 method=post action=RTLessorAVScmtylinesndworkf.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>