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
   '當所屬派工單已計算庫存後，不可作廢返轉之(stockcloseym<>'')
   sqlxx="select * FROM RTSPARQADSLCMTYsndwork WHERE CUTYID=" & KEY(0) & " and prtno='" & key(1) & "' "
   rsxx.Open sqlxx,connxx
   STOCKCLOSE=rsxx("stockcloseym")
   RSXX.CLOSE
   if STOCKCLOSE <> "" then
     endpgm="4"
   else
     sqlxx="select * FROM RTSPARQADSLCMTYhardware WHERE CUTYID=" & KEY(0) & " and prtno='" & key(1) & "' and entryno=" & key(2)
     RSXX.OPEN SQLXX,CONNxx
     endpgm="1"
     '未作廢
     IF not isdate(RSXX("dropdat")) THEN
        ENDPGM="3"
     ELSE
      sqlyy="select max(seq) as seq FROM RTSPARQADSLCMTYhardwarelog WHERE CUTYID=" & KEY(0) & " and prtno='" & key(1) & "' and entryno=" & key(2)
      rsyy.Open sqlyy,connxx
      if len(trim(rsyy("seq"))) > 0 then
         seq=rsyy("seq") + 1
      else
         seq=1
      end if
      rsyy.close
      set rsyy=nothing
      sqlyy="insert into RTSPARQADSLCMTYhardwarelog " _
           &"SELECT  CUTYID, PRTNO, ENTRYNO," & SEQ & ", GETDATE(),'R','" & V(0) & "', PRODNO, ITEMNO, QTY, DROPDAT, '設備作廢返轉', WAREHOUSE, ASSETNO,DROPUSR, UNIT " _
           &"FROM RTSPARQADSLCMTYHARDWARE where CUTYID=" & key(0) & " and prtno='" & key(1) & "' AND ENTRYNO=" & KEY(2)
     ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update RTSPARQADSLCMTYHARDWARE set dropdat=NULL,dropREASON='設備作廢返轉' where CUTYID=" & KEY(0) & " and prtno='" & key(1) & "' and entryno=" & key(2)
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '發生錯誤時，刪除異動檔所新增的異動資料
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM RTSPARQADSLCMTYhardwarelog WHERE CUTYID=" & key(0) & " and prtno='" & key(1) & "' and entryno=" & key(2) & " AND seq=" & seq
            CONNXX.Execute sqlyy 
         else
            endpgm="1"
            errmsg=""
         end if      
       end if
     END IF
   end if
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
       msgbox "設備安裝資料作廢返轉成功",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "此筆設備尚未作廢，不可作廢返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "所屬派工單已執行庫存計算作業，不可執行作廢返轉" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    else
       msgbox "無法執行設備安裝資料作廢返轉,錯誤訊息：" & "  " & frm1.htmlfld1.value
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