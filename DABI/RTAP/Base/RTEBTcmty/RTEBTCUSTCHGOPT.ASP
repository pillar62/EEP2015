<%
    key=request("key")
    keyary=split(key,";")
    Dim rs,conn,S6
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
    S6=""
    today=datevalue(now())
    updsql="SELECT RTCode.CODE, RTCode.CODENC " _
          &"FROM RTCode " _
          &"WHERE (RTCode.KIND = 'C8') and rtcode.code not in ( select  modifycode from RTEBTcustCHGLOG where " _
          &"RTEBTcustCHGLOG.COMQ1=" & keyary(0) & " and RTEBTcustCHGLOG.LINEQ1=" & keyary(1)  _
          &" and RTEBTcustCHGLOG.CUSID='" & KEYARY(2) & "' AND RTEBTcustCHGLOG.modifyDAT='" & today & "' and RTEBTcustCHGLOG.transdat is NOT null ) "
    'Response.Write "UPDSQL=" & updsql
    rs.Open updsql,CONN
  '  s1="<option value=""<>'*';全部"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s6=s6 &"<option value=""" &rs("code") & """>" &rs("codenc") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'------檢查該客戶是否已報竣且已轉檔,只有已轉檔過才可由此異動
    sql="select * from RTEBTCUST where COMQ1=" & keyary(0) & " and LINEQ1=" & KEYARY(1) & " AND CUSID='" & keyary(2) & "' "
    rs.Open sql,conn
 '   Response.Write "DATALEN=" & rs("transdat") 
 '   Response.end
    if rs.EOF then
       endpgm="2"
    elseif NOT ISDATE(rs("transdat")) then
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
<TITLE>東森AVS管理系統--已報竣客戶資料異動</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  Randomize  
  accessmode="U"
  'key:key(0)=社區編號 key(1)=客戶編號 key(2)=項次 key(3)=異動項目
  if keyform.htmlfld.value="1" then
     key=document.all("search1").value
     key=key & document.all("search6").value
     prog="RTEBTCustCHGD.asp?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&"
     Scrxx=window.screen.width
     Scryy=window.screen.height - 30
     StrFeature="Top=0,left=0,scrollbars=yes,status=yes," _
               &"location=no,menubar=no,width=" & scrxx & "px" _
               &",height=" & scryy & "px"
    'Set diagWindow=Window.Open(prog,"diag",strFeature)
     Window.Open prog,"diag",strFeature 
    ' window.open pgm 
    ' window.close
   elseif keyform.htmlfld.value="2" then
     msgbox "客戶資料找不到，請檢查客戶檔資料是否已被刪除!"
   elseif keyform.htmlfld.value="3" then
     msgbox "此客戶尚未至東森報竣(尚未轉檔)，不允許執行本作業==> 請至客戶資料維護作業直接修正即可!"
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

sub b1_onclick()
	if isdate(document.all("search1").value) then
		objEF2KDT.varDefaultDateTime=document.all("search1").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search1").value = objEF2KDT.strDateTime
	end if
end sub

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
<DIV align=Center><i><font face="細明體" size="5" color="#FF00FF">東森AVS管理系統</font></i> </DIV>
<DIV align=Center><i><font face="細明體" size="3" color="#FF00FF">已報竣客戶資料異動</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<tr>
 </font></td>
  <tr><td ALIGN="RIGHT"><font face="細明體">請選擇異動項目 :</font></TD>
  <TD><select name="search6" size="1" class=dataListEntry>
      <%=s6%>
      </select>  </td>
  <td style="display:none"><input type="text" name="search1" value="<%=key%>"></td>
</tr>
</table> 
<p><center><font face="細明體">
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure"   
 style="font-family: 細明體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="結束" ID="cmdcancel"   
 style="font-family: 細明體; color: #FF0000;cursor:hand">
 </center>
  <HR><P>
  <center>
  <table width="80%">
  <tr><td colspan="2" width="15%">作業說明</td></tr>
  <tr><TD WIDTH="5%" align="top">(1)</td><td  WIDTH="95%"align="left">本項作業之異動必須有客戶之"異動申請單"做為憑證! 資料更改後須交相關業管人員以做為後續資料轉檔至東森之依據!</td></tr>
  <tr><TD WIDTH="5%" align="top">(2)</td><td  WIDTH="95%"align="left">異動項目依每一客戶實際狀況而有所不同，即若該客戶同一日同一異動項目已做異動且已轉檔時，則該異動項目將不再呈現!</td></tr>
  <TABLE>
  <input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
  </form>
</BODY> 
</HTML>