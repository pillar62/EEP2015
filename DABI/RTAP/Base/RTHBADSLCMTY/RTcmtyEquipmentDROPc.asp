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
   sqlxx="select * FROM RTCMTYEquipment WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " and connecttype='" & key(2) & "' and seq=" & key(3)
   RSXX.OPEN SQLXX,CONNxx
   endpgm="1"
   '已作廢
   IF isnull(RSXX("canceldat"))  THEN
      ENDPGM="3"
   ELSE
      sqlyy="select max(entryno) as entryno FROM RTCMTYEquipmentlog WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " and connecttype='" & key(2) & "' and seq=" & key(3)
      rsyy.Open sqlyy,connxx
      if len(trim(rsyy("entryno"))) > 0 then
         entryno=rsyy("entryno") + 1
      else
         entryno=1
      end if
      rsyy.close
      set rsyy=nothing
      sqlyy="insert into RTCMTYEquipmentlog " _
           &"SELECT  COMQ1, LINEQ1, connecttype, seq," & entryno & ", GETDATE(),'R','" & V(0) & "', PRODNO, ITEMNO,unit, QTY, assetno,canceldat " _
           &"FROM RTCMTYEquipment where comq1=" & key(0) & " and lineq1=" & key(1) & " and connecttype='" & key(2) & "' AND seq=" & KEY(3)
     ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update RTCMTYEquipment set canceldat=NULL where comq1=" & KEY(0) & " and lineq1=" & key(1) & " and connecttype='" & key(2) & "' and seq=" & key(3)
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM RTCMTYEquipmentlog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & " and connecttype='" & key(2) & "' and seq=" & key(3) & " AND entryno=" & entryno
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
       msgbox "設備資料作廢返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此筆設備尚未作廢，不可執行作廢返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    else
       msgbox "無法執行設備資料作廢返轉,錯誤訊息：" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=rtebtcmtyhardwaredrop.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>