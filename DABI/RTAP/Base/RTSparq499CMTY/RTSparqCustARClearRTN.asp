<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
key=REQUEST("KEY")
keyary=split(key,";")
CUSID=KEYARY(0)
batchno=KEYARY(1)
seq=KEYARY(2)

logonid=session("userid")
Call SrGetEmployeeRef(Rtnvalue,1,logonid)
V=split(rtnvalue,";")  
Set conn=Server.CreateObject("ADODB.Connection")
conn.open "DSN=RTLib"
'conn.BeginTrans(改由STORE PROCEDURE內執行TRANSACTION、COMMIT、ROLLBACK)
'呼叫store procedure更新沖帳主檔及沖帳明細檔
if KEYARY(3) ="Sparq399" then
	strSP="usp_RTSparqAdslCustArClear " & "'" & cusid & "'" & ",'" & batchno & "'," & SEQ & ",'" & v(0) & "', 'RTN' "
elseif KEYARY(3) ="Sparq499" then
	strSP="usp_RTSparq499CustArClear " & "'" & cusid & "'" & ",'" & batchno & "'," & SEQ & ",'" & v(0) & "', 'RTN' "
end if
Set ObjRS = conn.Execute(strSP)
If Err.number = 0 then
   ENDPGM="1"
   ERRMSG=""
'   conn.CommitTrans
else
   ENDPGM="2"
   errmsg=cstr(Err.number) & "=" & Err.description
'   conn.rollbackTrans
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
       msgbox "應收(付)帳款沖帳返轉成功。"
       window.close
    else
       msgbox "無法執行應收(付)帳款沖帳返轉,錯誤訊息" & "  " & frm1.htmlfld1.value
       window.close
    end if
End Sub
-->
</script>
</head>
<form name=frm1 method=post action="" ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>