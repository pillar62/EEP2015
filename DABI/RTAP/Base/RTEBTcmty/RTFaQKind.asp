<% KEY=REQUEST("KEY") %>
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>客訴設備種類選擇</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  s1ary=document.all("search1").value
  s1=split(s1ary,";")
  key=document.all("keyfield").value
  Scrxx=window.screen.width
  Scryy=window.screen.height - 30
  StrFeature="Top=0,left=0,scrollbars=yes,status=yes," _
            &"location=no,menubar=no,width=" & scrxx & "px" _
            &",height=" & scryy & "px"
  prog="RTFaqD.asp?V=" & Rnd() &"&accessMode='A'"  &"&key=" &key & "&opt=" & s1(0)
  msgbox prog
  Set diagWindow=Window.Open(prog,"diag2",strFeature)
 ' window.close
End Sub

</SCRIPT>
</HEAD>

<BODY style="background:lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">HI-Building 客訴設備種類選擇</font></i> </DIV>
<INPUT type="hidden" name="Keyfield" value="<%=key%>">
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
  <tr><td><font face="標楷體">請選擇客訴設備種類 :</font></td>
<td><font face="標楷體">
    <SELECT size="1" name="search1" style="width:200;">
        <option value="1;東訊設備" selected>東訊設備</option>
        <option value="2;合勤設備">合勤設備</option>
        <option value="3;ADSL設備">ADSL設備</option>
    </SELECT>  
 </font></td><tr>
</table> 
<p><font face="標楷體">按 <INPUT TYPE="button" VALUE="查詢" ID="cmdSure"
 style="font-family: 標楷體; color: #FF0000;cursor:hand">  
 系統將依您選取之客訴種類，呈現專屬之問題供您選擇。</font></p> 
</BODY> 
</HTML>