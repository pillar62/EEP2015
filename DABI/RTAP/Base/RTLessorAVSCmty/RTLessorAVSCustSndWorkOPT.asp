<%
    key=request("key")
    keyary=split(key,";")
    Dim rs,conn
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
    today=datevalue(now())

'------檢查該客戶是否已押完工結案日或開始計費日，若有則不可執行

    sql="SELECT	* " _
	   &"FROM	RTLessorAVSCust " _
       &"where  cusid ='" & keyary(0) &"' "
'response.write "sql="& sql
    rs.Open sql,conn
    if rs.EOF then
       endpgm="2"
    elseif len(rs("strbillingdat"))=0 or len(rs("finishdat"))=0 then
       endpgm="3"
    else    
       endpgm="1"
    end if
'---------------------------------------------------------
    rs.close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>AVS City 裝機派工結案作案</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  'Randomize  
  'accessmode="U"
  'key:key(0)=社區編號 key(1)=客戶編號 key(2)=項次 key(3)=異動項目
  if keyform.htmlfld.value="1" then
     search1=document.all("search1").value
     search2= document.all("search2").value
     search3= document.all("keyary1").value
     search4= document.all("keyary2").value
     'prog="RTLessorAVSCustSNDWORKF.asp?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &search &"&"
     prog="RTLessorAVSCustSNDWORKF.asp?key=" & search3 &";"& search4 &";"& search1 &";"& search2 &";"
     Scrxx=window.screen.width
     Scryy=window.screen.height - 30
     StrFeature="Top=0,left=0,scrollbars=yes,status=yes," _
               &"location=no,menubar=no,width=" & scrxx & "px" _
               &",height=" & scryy & "px"
    'Set diagWindow=Window.Open(prog,"diag",strFeature)
    Window.Open prog,"diag",strFeature 
    'window.open pgm 
    'window.close
   elseif keyform.htmlfld.value="2" then
     msgbox "客戶資料找不到，請檢查客戶檔資料是否已被刪除!"
   elseif keyform.htmlfld.value="3" then
     msgbox "此客戶完工結案日或開始計費日已押，不允許執行本作業!"
   else
     msgbox "程式執行發生異常，請通知資訊人員!"
   end if
End Sub

Sub cmdcancel_onClick
  Dim winP,docP
  Set winP=window.Opener
'  Set docP=winP.document
'  winP.focus()
  window.close
End Sub

Sub Srbtnonclick()
	Dim ClickID
	ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
	clickkey="search" & clickid
	if isdate(document.all(clickkey).value) then
		objEF2KDT.varDefaultDateTime=document.all(clickkey).value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
		document.all(clickkey).value = objEF2KDT.strDateTime
	end if
End Sub 
</SCRIPT>
</HEAD>

<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codeBase=http://www.cbbn.com.tw/stock/EF2KDT.CAB#version=9,0,0,3 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="background:lightblue">
<!--form name=keyform是為了配合模組架構所需-->
<form name="keyform">
<DIV align=Center><i><font face="細明體" size="5" color="#FF00FF">AVS City 裝機派工結案</font></i> </DIV>
<!-- <DIV align=Center><i><font face="細明體" size="3" color="#FF00FF">報竣客戶資料異動</font></i> </DIV> -->
<P></P>

<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
	<tr><td ALIGN="RIGHT"><font face="標楷體">請輸入完工結案日:</font></td>
		<td><input type="text" name="search1" align=right class=dataListEntry value="<%=today%>" readonly>
   			<input type="button" id="B1" name="B1" height=100% width=100% style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"></td></tr>
	<tr><td ALIGN="RIGHT"><font face="標楷體">請輸入開始計費日:</font></td>
		<td><input type="text" name="search2" align=right class=dataListEntry value="<%=today%>" readonly>
   			<input type="button" id="B2" name="B2" height=100% width=100% style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick"></td></tr>

</table>

<p><center><font face="細明體">
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure"   
 style="font-family: 細明體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="結束" ID="cmdcancel"   
 style="font-family: 細明體; color: #FF0000;cursor:hand">
 </center>
 
  <HR><P>
  <center>
  <input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
    <input type="text" name=keyary1 style=display:none value="<%=keyary(0)%>">
    <input type="text" name=keyary2 style=display:none value="<%=keyary(1)%>">
  </form>
</BODY> 
</HTML>