<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->

<%
logonid=session("userid")
   SqlStr=Session("SQLSTRREVPRT")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
'------------------------------------------------------------
   dim ObjRS,strSP,endpgm,search2,logonid,SQL
       prtno=session("paycanprtno")
       StrSP="USP_okcancel 'H','"& prtno &"', '"& V(0) &"'"
       Set conn=Server.CreateObject("ADODB.Connection")  
       DSN="DSN=RtLib"
       conn.Open DSN
       endpgm=""
       If len(trim(prtno)) > 0 THEN
          Set rs=Server.CreateObject("ADODB.Recordset") 
          SQL="select * from rtcust where paydtlprtno='" & prtno & "'"
          'Response.Write "SQL=" & SQL
          rs.Open sql,conn
          if rs.EOF then
              endpgm="4"
              errmsg=prtno
          elseif rs("settype") <> "2" and rs("settype") <> "3" then
              endpgm="3"
          elseif Not IsNull(rs("acccfmdat")) then
              endpgm="5"
          end if
          rs.Close
          set rs=nothing
       end if
       if endpgm="" then
          On Error Resume Next
          Set ObjRS = conn.Execute(strSP)      
          conn.Close
          If Err.number > 0 then
             endpgm="2"
             errmsg=cstr(Err.number) & "=" & Err.description
          else
             endpgm="1"
             errmsg=""
          end if
       End if       
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    Set winP=window.Opener
    Set docP=winP.document
    if frm1.HTMLfld.value="1" then
       msgbox "RT施工費用明細表列印撤銷完成",0
       docP.all("keyform").Submit
    elseif frm1.HTMLfld.value="2" then
       msgbox "RT施工費用明細表列印撤銷失敗,錯誤訊息：" & "  " & errmsg
    elseif frm1.HTMLfld.value="3" then
       msgbox "RT施工費用明細表列印撤銷失敗,錯誤訊息：此列印資料非(業務)施工，無權限撤銷！"
    elseif frm1.HTMLfld.value="4" then
       msgbox "RT施工費用明細表列印撤銷失敗,錯誤訊息：系統找不到批號--" & errmsg & "的報表資料!"   
    elseif frm1.HTMLfld.value="5" then
       msgbox "RT施工費用明細表列印撤銷失敗,錯誤訊息：此列印批號--會計已審核"              
    end if
    winP.focus()              
    window.close    
 end sub
</script>   
<form name=frm1 method=post action=verify.asp>
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>">
</form> 