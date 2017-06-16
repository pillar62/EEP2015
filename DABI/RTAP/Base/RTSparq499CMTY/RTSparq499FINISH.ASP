<%
key1=request("key")
keyary=split(key1,";")
set conn=server.CreateObject("ADODB.connection")
set rs=server.CreateObject("ADODB.recordset")
DSN="DSN=RTLIB"
sql="select LINETEL from rtsparq499cmtyline where COMQ1=" & keyary(0) & " AND LINEQ1=" & keyary(1)
conn.Open DSN
rs.Open sql,conn
if not rs.EOF then
   MainLineNo=rs("LINETEL")
end if
rs.Close
sql="select nciccusno,SPHNNO from rtsparq499cust where comq1=" & keyary(0) & " and lineq1=" & keyary(1) & " and cusid='" & keyary(2) & "'"
rs.Open sql,conn
if not rs.EOF then
   HNNO=rs("nciccusno") & rs("sphnno")
end if
rs.Close
conn.Close
set rs=nothing
set conn=nothing
%>
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>速博499報竣確認作業</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  key1=document.all("search1").value '社區代號 + 主線序號 + 客戶代號
  key2=document.all("search2").value '社區附掛電話號碼
  key3=document.all("search3").value '對帳編號
  FDAT=document.all("search7").value '報竣日期
 
  if Len(trim(FDAT)) = 0 then 
     MSGBOX "報竣日期不可空白!"
  elseif len(trim(key2)) = 0 then
     msgbox "社區附掛電話空白，不可進行報竣作業!"
  elseif len(trim(key3)) > 0 then
     msgbox "此客戶已報竣，不可重覆報竣!"
  else
    prog="RTSPARQ499FinishProcess.asp" & "?key=" & key1 & "&FDAT=" & FDAT & "&TEL=" & KEY2
   ' Scrxx=window.screen.width
   ' Scryy=window.screen.height - 30
    Scrxx=0
    Scryy=0
    StrFeature="top=0,left=0,scrollbars=yes,status=yes," _
                 &"location=no,menubar=no,width=" & scrxx & "px" _
                 &",height=" & scryy & "px,fullscreen=yes" 
    Set diagWindow=Window.open(prog,"d2",StrFeature)  
    diawidth=0
    diaheight=0
  '  Set diagWindow=Window.Showmodaldialog(prog,"d2","dialogWidth:" & diawidth & "px;dialogHeight:" & diaheight &"px;")  
    if returnvalue=ture then
       msgbox "客戶報竣作業完成!"
    else
       msgbox "客戶報竣作業異常，請通知資訊部處理。"
    end if
    returnvalue=True
    window.close  
  end if
End Sub
Sub cmdcancel_onClick
   window.close
End Sub
sub b1_onclick()
	if isdate(document.all("search7").value) then
		objEF2KDT.varDefaultDateTime=document.all("search7").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search7").value = objEF2KDT.strDateTime
	end if
end sub

</SCRIPT>
</HEAD>
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"  codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="background:lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">速博499客戶報竣作業</font></i> </DIV>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
  <tr><td><font face="標楷體" align=right>請選擇報竣日期 :</font></td>

<td><input type="text" size="10" maxlength="10" name="search7" align=right class=dataListEntry value="<%=Sdate%>" readonly> 
    <input type="button" id="B1" name="B1" height=100% width=100% style="Z-INDEX: 1" value="....">
    <input type="TEXT"  name="search1" height=100% width=100%  style="display:none" value="<%=KEY1%>">
    <input type="TEXT"  name="search2" height=100% width=100%  style="display:none" value="<%=MainlineNo%>">   
         <input type="TEXT"  name="search3" height=100% width=100%  style="display:none" value="<%=HNNO%>">    </td>
</tr>
</table>
<center>
<p>
<INPUT TYPE="button" VALUE="確認" ID="cmdSure" style="font-family: 標楷體; color: #FF0000;cursor:hand">   
<INPUT TYPE="button" VALUE="取消" ID="cmdcancel" style="font-family: 標楷體; color: #FF0000;cursor:hand">   
</center>
<p><font size=2 >(1)按[確認]後，系統將依據每位用戶所屬<font size=2 color=red>[社區]</font>中之附掛電話資料，產生對速博<font size=2 color=red>[用戶對帳代碼]</font> 。</font> 
<br><font size=2 >並依據您所選取的報竣日期更新更新<font size=2 color=red>[報竣日期]</font>欄之狀態。</font>
<br><font size=2 color=red>(2)一旦您報竣後，該社區之附掛電話將無法再異動，請先確認社區之附掛電話是否正確!</font> 
</BODY> 
</HTML>