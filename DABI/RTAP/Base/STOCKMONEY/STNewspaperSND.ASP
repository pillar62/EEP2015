<html>

<head>
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<meta name="GENERATOR" content="Microsoft FrontPage 4.0">
<meta name="ProgId" content="FrontPage.Editor.Document">
<title>元訊寬頻網路股份有限公司</title>
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" height="60" id="objEF2KDT" style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" width="60" VIEWASTEXT>
			<PARAM NAME="_ExtentX" VALUE="1270">
			<PARAM NAME="_ExtentY" VALUE="1270">
		</OBJECT>
</head>
<script language=vbscript>
sub newurl(num)
    if num=1 then
       KEY=document.all("KEY1").value
       IF LEN(TRIM(KEY)) = 0 THEN
          MSGBOX "日期不可空白!"
       ELSE
          if document.all("KEY2").checked=TRUE then
             opt1="Y"
          END IF
          if document.all("KEY3").checked=TRUE then
             opt2="Y"
          END IF
          if document.all("KEY4").checked=TRUE then
             opt3="Y"
          END IF
          IF OPT1="" AND  OPT2="" AND  OPT3="" THEN
             MSGBOX "至少需選擇一種電子報服務!"
          ELSE
             prog="STteatimesndmail.asp?KEY=" & KEY & ";" & opt1 & ";" & opt2 & ";" & opt3 
             xx=(window.screen.width -7)
             yy=(window.screen.height -74)
             features="top=0,left=0,status=yes,location=no,menubar=no,scrollbars=yes" & ",height=" & yy & ",width=" & xx
             result=window.open(prog,"ppAP",features)
             window.event.returnValue=False
          END IF
       END IF
    elseif num=2 then
       prog="STnewspapersndlogk.asp"
       xx=(window.screen.width -7)
       yy=(window.screen.height -74)
       features="top=0,left=0,status=yes,location=no,menubar=no,scrollbars=yes" & ",height=" & yy & ",width=" & xx
       result=window.open(prog,"ppAP",features)
       window.event.returnValue=False
    end if
end sub
Sub Srbtnonclick()
    Dim ClickID
    ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
    clickkey="key" & clickid
    if isdate(document.all(clickkey).value) then
	   objEF2KDT.varDefaultDateTime=document.all(clickkey).value
    end if
    call objEF2KDT.show(1)
    if objEF2KDT.strDateTime <> "" then
       document.all(clickkey).value = objEF2KDT.strDateTime
    end if
END SUB  
Sub SrClear()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="C" & clickid
       clearkey="key" & clickid       
       if len(trim(document.all(clearkey).value)) <> 0 then
          document.all(clearkey).value =  ""
          '當處理人員及處理廠商皆為空白時，才可清除此欄位資料
       end if
End Sub      
Sub ImageIconOver()
       self.event.srcElement.style.borderBottom = "black 1px solid"
       self.event.srcElement.style.borderLeft="white 1px solid"
       self.event.srcElement.style.borderRight="black 1px solid"
       self.event.srcElement.style.borderTop="white 1px solid"   
End Sub
   
Sub ImageIconOut()
       self.event.srcElement.style.borderBottom = ""
       self.event.srcElement.style.borderLeft=""
       self.event.srcElement.style.borderRight=""
       self.event.srcElement.style.borderTop=""
End Sub            
</script>
<body bgcolor="#c3c9d2">
<table border="0" width="100%">
  <tr>
    <td width="20%"></td>
    <td width="60%" align="middle"><font color="#0000ff">元訊寬頻網路股份有限公司</font></td>
    <td width="20%"><font color="#0000ff">日期：<%=DATEVALUE(NOW)%></font></td>
  </tr>
  <tr>
    <td width="20%">　</td>
    <td width="60%" align="middle"><font color="#0000ff">電子報發送作業</font></td>
    <td width="20%"><font color="#0000ff">時間：<%=timeVALUE(TIME)%></font></td>
  </tr>

</table>
<HR>
<CENTER>
<TABLE style="WIDTH: 300px; HEIGHT: 75px" cellSpacing=0 cellPadding=0 bgColor=antiquewhite border=1>
  
  <TR>
    <TD style="WIDTH: 25%"  bgColor=#3e7373>
      <P style="COLOR: yellow;font-size:12pt"" align=center>電子報發送選項</P></TD></TR>
 <TR>
    <TD>
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <center><INPUT type="text" SIZE=10 readonly name="key1" id="KEY1">
        <input type="button" id="B1" name="B1" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick()">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C1" name="C1" style="Z-INDEX: 1" border="0" onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">
        </FONT></center>
     </TD>
  </TR>
  <TR>
    <TD><center>
        <INPUT type="CHECKBOX"  NAME="key2" style="height:30" ><FONT style="COLOR: darkorchid;font-size:12pt">下午茶</FONT>
        <INPUT type="CHECKBOX"  NAME="key3" style="height:30" ><FONT style="COLOR: darkorchid;font-size:12pt">私房菜</FONT>
        <INPUT type="CHECKBOX"  NAME="key4" style="height:30" ><FONT style="COLOR: darkorchid;font-size:12pt">文字稿</FONT>
        </center></TD>
  </TR>
  <TR>
    <TD onclick="newurl(1)" >
        <center><INPUT type="BUTTON"  style="WIDTH: 100%;height:100%;font-size:12pt"  VALUE=" 發                 送 "  ></center>
    </TD>
  </TR>        
</TABLE>
  <p></p>
<TABLE style="WIDTH: 300px; HEIGHT: 75px" cellSpacing=0 cellPadding=0 bgColor=antiquewhite border=1 ID="Table2">
  <TR>
    <TD style="WIDTH: 25%"  bgColor=#3e7373>
      <P style="COLOR: yellow;font-size:12pt"" align=center>追蹤查詢</P></TD></TR>
  <TR>
    <TD onclick="newurl(2)" >
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON"  style="WIDTH: 100%;height:100%;font-size:12pt"  VALUE="電子報發送記錄查詢" >
        </FONT></TD>
  </TR>            
</table>
<p></p>
</CENTER>
</body></HTML>
