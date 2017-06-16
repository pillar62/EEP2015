<%@ Language=VBScript %>
<%
   'logonid=session("userid")
   'Key=split(request("key"),";")
   'Scaseno=key(0)
   'Ecaseno=key(0)   
   'Call SrGetEmployeeRef(Rtnvalue,1,logonid)
   '      V=split(rtnvalue,";")  

    Dim rs,conn,S6
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
    
    S6=""
    rs.Open "SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '3')",CONN
    s6="<option value=""C"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s6=s6 &"<option value=""" &rs("AREAID") & """>" &rs("AREANC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
    
	S7=""
	rs.Open "SELECT PRTNO FROM HBCustDrop GROUP BY PRTNO ORDER BY PRTNO Desc", conn	
    s7="<option value="""" selected></option>" &vbCrLf    
    Do While Not rs.Eof
       s7=s7 &"<option value=""" &rs("PRTNO") & """>" &rs("PRTNO") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
    
    rs.Open "SELECT EMPLY FROM RTEmployee WHERE NETID ='"& session("userid") &"' "
    s8 = rs("EMPLY")
   
	rs.Close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing

   'On error Resume next
   'If Err.number <= 0 then
   '   endpgm="1"
   '   errmsg=""
   '   session("Scaseno")=  Scaseno 
   '   session("Ecaseno")=  Ecaseno 
   'else
   '   endpgm="2"
   '   errmsg=cstr(Err.number) & "=" & Err.description
   'end if
%> 
<HTML>
<Head>
<script language=vbscript>

' sub window_onload()
'    if frm1.htmlfld.value="1" then
'       msgbox "欠退復客戶明細表列印完成",0
'       Set winP=window.Opener
'       Set docP=winP.document       
'       docP.all("keyform").Submit
'       winP.focus()              
'       window.frm2.submit
'    else
'       msgbox "無法列印,錯誤訊息：" & "  " & frm1.htmlfld1.value
'       Set winP=window.Opener
'       winP.focus()
'       window.close
'    end if
   ' window.close    
' end sub
 
 Sub cmdSure_onClick
  PGM="/rtap/base/hbadslcust/RTCustDropP.asp?parm=" 
  emply=document.all("search8").value  
  prtno=document.all("search7").value  
  AREAID=document.all("sEARCH6").value
  'if len(trim(areaid))=0 then 
  '   areaid="*"
  'end if
  dropstatus=document.all("sEARCH5").value
'  drops=document.all("search4").value
'  drope=document.all("search3").value
   
  pgm=pgm & areaid & ";" & dropstatus &";"& prtno &";"& emply
'  msgbox pgm
  window.open pgm 
  window.close
End Sub
Sub cmdcancel_onClick
  window.close
End Sub


'sub b3_onclick()
'	if isdate(document.all("search3").value) then
'		objEF2KDT.varDefaultDateTime=document.all("search3").value
'	end if
'	call objEF2KDT.show(1)
'	if objEF2KDT.strDateTime <> "" then
'	    document.all("search3").value = objEF2KDT.strDateTime
'	end if
'end sub
'sub b4_onclick()
'	if isdate(document.all("search4").value) then
'		objEF2KDT.varDefaultDateTime=document.all("search4").value
'	end if
'	call objEF2KDT.show(1)
'	if objEF2KDT.strDateTime <> "" then
'	    document.all("search4").value = objEF2KDT.strDateTime
'	end if
'end sub
 
</script> 
</head>

<!--
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codeBase=http://www.cbbn.com.tw/stock/EF2KDT.CAB#version=9,0,0,3 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 V
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>
-->
	
<BODY style="BACKGROUND: lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">HI-BUILDING－報表列印</font></i> </DIV>
<DIV align=Center><i><font face="標楷體" size="3" color="#FF00FF">欠退復客戶明細表</font></i> </DIV>
<P><P>
<table align="center" width="50%" border=0 cellPadding=0 cellSpacing=0 ID="Table1">

<tr><td><font face="標楷體">列印批號 :</font></td>
	<td><select name="search7" size="1" length="15" class=dataListEntry>
		  <%=s7%>
	</select></td></tr>

<tr><td><font face="標楷體">區域別 :</font></td>
	<td><select name="search6" size="1" class=dataListEntry>
		  <%=s6%>
	</select></td></tr>
	
<tr><td><font face="標楷體">執行狀態 :</font></td>
	<td><select name="search5" size="1" class=dataListEntry>
        <option value="*" SELECTED>全部</option>          
        <option value="欠拆">欠拆</option>
        <option value="退租">退租</option>  
        <option value="復裝">復裝</option>  
    </select></td></tr>

<!--    
<tr><td><font face="標楷體">申請日期(起) :</font></td>
	<td><input size="10" maxlength="10" name="search4" align=right class=dataListEntry value="" readonly>
		<input type="button" id="B4" name="B4" height="100%" width="100%" style="Z-INDEX: 1" value="...."></td></tr>
		
<tr><td><font face="標楷體">申請日期(迄) :</font></td>
	<td><input size="10" maxlength="10" name="search3" align=right class=dataListEntry value="" readonly>
		<input type="button" id="B3" name="B3" height="100%" width="100%" style="Z-INDEX: 1" value="...."></td></tr>
-->		
		
<tr><td><input type=hidden size="10" maxlength="10" name="search8" align=right value="<%=s8%>" ></td></tr>
		
</table>

<p><center><font face="標楷體">
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure"   
 style="COLOR: #ff0000; CURSOR: hand; FONT-FAMILY: 標楷體" NAME="cmdSure"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel"   
 style="COLOR: #ff0000; CURSOR: hand; FONT-FAMILY: 標楷體" NAME="cmdcancel">
 </center><HR>
   
    
<!-- 
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>"> 
</form>
<form name=frm2 method=post action="RTCustDropP.asp">
</form>
-->

</body>
</html>
