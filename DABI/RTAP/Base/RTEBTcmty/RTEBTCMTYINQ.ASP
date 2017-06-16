<html>

<head>
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<meta name="GENERATOR" content="Microsoft FrontPage 4.0">
<meta name="ProgId" content="FrontPage.Editor.Document">
<title>元訊寬頻網路股份有限公司</title>
</head>
<script language="JavaScript" fptype="dynamicanimation">
<!--
function dynAnimation() {}
function clickSwapImg() {}
//-->
</script>
<script language="JavaScript1.2" fptype="dynamicanimation" src="animate.js">
</script>
<script language=vbscript>
  sub newurl(num)
    sym=document.all("strym").value
    select case num
      case 1
        prog="RTEBTCMTYK2.asp"
      case 2
        prog="RTEBTCMTYLINeK2.asp"
      case 3
        prog="RTEBTCUSTK2.asp"
      case 4
        prog="RTEBTCUSTK5.asp"        
      case 5
        prog="RTEBTCUSTK6.asp"         
      case 6
        prog="RTEBTCUSTM2M3K.asp"                    
      case 7
        prog="RTEBTCUSTM2K.asp"         
      case 8
        prog="RTEBTCMTYLINEAPPLYK.asp"            
      case 9
        prog="RTEBTcustK7.asp"          
      case 10
        prog="RTEBTcustAR.asp?key=" & Sym           
      case else
    end select
    if sym="" AND num=10 then
       msgbox "應收帳款年月不可空白!"
    else
       xx=(window.screen.width -7)
       yy=(window.screen.height -74)
       features="top=0,left=0,status=yes,location=no,menubar=no,scrollbars=yes" & ",height=" & yy & ",width=" & xx
       result=window.open(prog,"ppAP",features)
       window.event.returnValue=False
    end if
  end sub
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
    <td width="60%" align="middle"><font color="#0000ff">AVS各項查詢作業</font></td>
    <td width="20%"><font color="#0000ff">時間：<%=timeVALUE(TIME)%></font></td>
  </tr>

</table>
<HR>
<CENTER>
<TABLE style="WIDTH: 300px; HEIGHT: 75px" cellSpacing=0 cellPadding=0 bgColor=antiquewhite border=1>
  
  <TR>
    <TD style="WIDTH: 25%"  bgColor=#3e7373>
      <P style="COLOR: yellow;font-size:12pt"" align=center>資料查詢</P></TD></TR>
  <TR>
    <TD onclick="newurl(1)" >
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON" style="WIDTH: 100%;height:30;font-size:12pt"  VALUE="社  區  查  詢" ID="B1" NAME="B1">
        </FONT></TD>
  </TR>
  <TR>
    <TD onclick="newurl(2)" >
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:12pt"  VALUE="主  線  查  詢" ID="B2" NAME="B2">
        </FONT></TD>
  </TR>
  <TR>
    <TD onclick="newurl(3)" >
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:12pt"  VALUE="用  戶  查  詢" ID="B3" NAME="B3">
        </FONT></TD>
  </TR>    
</TABLE>
  <p></p>
<TABLE style="WIDTH: 300px; HEIGHT: 75px" cellSpacing=0 cellPadding=0 bgColor=antiquewhite border=1 ID="Table2">
  <TR>
    <TD style="WIDTH: 25%"  bgColor=#3e7373>
      <P style="COLOR: yellow;font-size:12pt"" align=center>追蹤查詢</P></TD></TR>
  <TR>
    <TD onclick="newurl(8)" >
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:12pt"  VALUE="EBT已確認測通之主線可申請用戶" ID="Button5" NAME="B3">
        </FONT></TD>
  </TR>            
  <TR>
    <TD onclick="newurl(6)" >
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:12pt"  VALUE="M2/M3用戶追蹤處理" ID="Button3" NAME="B3">
        </FONT></TD>
  </TR>    
  <TR>
    <TD onclick="newurl(7)" >
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:12pt"  VALUE="M2已拆機用戶復機處理" ID="Button4" NAME="B3">
        </FONT></TD>
  </TR>      
</table>
<p></p>
<TABLE style="WIDTH: 300px; HEIGHT: 75px" cellSpacing=0 cellPadding=0 bgColor=antiquewhite border=1 ID="Table1">
  
  <TR>
    <TD style="WIDTH: 25%"  bgColor=#3e7373>
      <P style="COLOR: yellow;font-size:12pt"" align=center>作業稽核</P></TD></TR>
  <TR>
    <TD onclick="newurl(4)" >
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:12pt"  VALUE="合約終止尚未完成退租用戶查詢" ID="Button2" NAME="B3">
        </FONT></TD>
  </TR>
  <TR>
    <TD onclick="newurl(5)" >
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:12pt"  VALUE="已報竣而無合約生效日用戶查詢" ID="Button1" NAME="B3">
        </FONT></TD>
  </TR>  
    <TR>
    <TD onclick="newurl(9)" >
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:12pt"  VALUE="合約到期日用戶查詢" ID="Button6" NAME="B3">
        </FONT></TD>
  </TR>  

  </table>

<p></p>
<TABLE style="WIDTH: 300px; HEIGHT: 30px" cellSpacing=0 cellPadding=0 bgColor=antiquewhite border=1 ID="Table1">
  
  <TR>
    <TD style="WIDTH: 25%"  bgColor=#3e7373 colspan=2>
      <P style="COLOR: yellow;font-size:12pt"" align=center>帳務稽核</P></TD></TR>
  <TR>
    <td  style="WIDTH: 30%" align=center> <input type="text" name=strym size=6 maxlength=6></td>
    <TD onclick="newurl(10)"  style="WIDTH: 70%">
        <FONT style="COLOR: darkorchid;font-size:12pt">
        <INPUT type="BUTTON"  style="WIDTH: 100%;height:30;font-size:12pt"  VALUE="欠費用戶明細" ID="Button2" NAME="B3">
        </FONT>
  </TD>  </TR>
<tr><td colspan=2><font size=2>請輸入應收帳款年月，如︰200411</font></td></tr>


  </table>

</CENTER>
</body></HTML>
