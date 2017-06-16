<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
logonid=session("userid")
Call SrGetEmployeeRef(Rtnvalue,1,logonid)
V=split(rtnvalue,";")  
key=REQUEST("KEY")
keyary=split(key,";")
COMQ1=KEYARY(0)
LINEQ1=KEYARY(1)
CUSID=KEYARY(2)
newlineq1=keyary(3)
Set conn=Server.CreateObject("ADODB.Connection")
conn.open "DSN=RTLib"
'呼叫store procedure更新相關檔案
strSP="usp_RTLessorAVSCustMoveExe " & COMQ1 & "," & LINEQ1 & ",'" & CUSID & "','" & V(0) & "'," & newlineq1 
'response.write strSP
'response.end
Set ObjRS = conn.Execute(strSP)
If Err.number = 0 then
   ENDPGM="1"
   ERRMSG=""
else
   ENDPGM="2"
   errmsg=cstr(Err.number) & "=" & Err.description
end if         
conn.Close

Set conn=Nothing
%>
<html>
<head>
<link REL="stylesheet" HREF="/WebUtilityV4ebt/DBAUDI/dataList.css" TYPE="text/css">
<link REL="stylesheet" HREF="dataList.css" TYPE="text/css">
<script language="VBScript">
<!--
Sub window_onload()  
    if frm1.htmlfld.value="1" then
       msgbox "客戶移動主線成功。"
       window.close
    else
       msgbox "無法執行用戶移動作業,錯誤訊息" & "  " & frm1.htmlfld1.value
       window.close
    end if
End Sub
-->
</script>
</head>
<form name=frm1 method=post action=RTLessorAVSCustmoveExe.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>