<%
stocknc=session("name")
%>
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
    select case num
      case 1
        prog="dialog1.asp"
      case 2
        prog="dialog2.asp"        
      case 3
        prog="dialog3.asp"         
      case 4
        prog="dialog4.asp"      
      case 5
        prog="dialog5.asp"              
      case 6
        prog="dialog6.asp"      
      case 7
        prog="dialog7.asp"        
      case 8
        prog="dialog8.asp"           
      case 9
        prog="dialog9.asp"                                                            
      case 10
        prog="dialog10.asp"           
      case 11
        prog="dialog11.asp"             
      case 12
        prog="dialog12.asp"    
      case 13
        prog="dialog13.asp"                 
      case 14
        prog="dialog14.asp"             
      case 15
        prog="dialog15.asp"      
      case 16
        prog="dialog16.asp"                                                                                                
      case 17
        prog="dialog17.asp"
      case 18
        prog="dialog18.asp"
      case 19
        prog="dialog19.asp"
      case 20
        prog="dialog20.asp"      
	  case 21
        prog="dialog21.asp"      
	  case 22
        prog="dialog22.asp"              
      case else
    end select
    xx=(window.screen.width -7)/2
    yy=(window.screen.height -74)/2
    xx2=(window.screen.width - XX)/2
    yy2=(window.screen.height - yy)/2
    features="top=" & xx2 & ",left=" & yy2 & ",status=yes,location=no,menubar=no,scrollbars=yes" & ",height=" & yy & ",width=" & xx
    result=window.open(prog,"ppAP",features)
    window.event.returnValue=False
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
    <td width="60%" align="middle"><font color="#0000ff">ADSL券商專案－各類報表列印</font></td>
    <td width="20%"><font color="#0000ff">時間：<%=timeVALUE(TIME)%></font></td>
  </tr>

</table>
<HR>
<CENTER>
<TABLE style="WIDTH: 587px; HEIGHT: 75px" cellSpacing=0 cellPadding=0 width=587 
bgColor=antiquewhite border=1>
  
  <TR>
    <TD style="WIDTH: 27%" width="27%" bgColor=#3e7373>
      <P style="COLOR: yellow" align=center>週報表</P></TD>
    <TD style="WIDTH: 27%" width="27%" bgColor=#3e7373>
      <P style="COLOR: yellow" align=center>月報表</P></TD>
    <TD style="WIDTH: 46%" width="46" bgColor=#3e7373>
      <P style="COLOR: yellow" align=center>其它管理報表</P></TD></TR>
  <TR>
    <TD onclick="newurl(1)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt" >(1)本週送件客戶明細表</FONT></TD>
    <TD onclick="newurl(16)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt" >(1)本月各組業績目標達成比率表</FONT></TD>
    <TD onclick="newurl(2)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(1)客戶申裝進度統計表(含累計)</FONT></TD>
  </TR>
  <TR>
    <TD onclick="newurl(3)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(2)本週完工客戶明細表</FONT></TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(4)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(2)三人成行已送件客戶明細表</FONT></TD></TR>    
  <TR>
    <TD onclick="newurl(15)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(3)本週各組業績目標達成比率表</FONT></TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(5)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(3)已達三人未送件客戶明細表</FONT></TD></TR>   
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(6)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(4)券商專案客戶明細表(全部-依地址排序)</FONT></TD></TR>      
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(7)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(5)券商專案客戶明細表(全部-依券商分行排序)</FONT></TD></TR>          
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(8)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(6)券商專案客戶明細表(三人成行篩選表)</FONT></TD></TR>              
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(9)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(7)客戶明細表(依鄉鎮區排列)</FONT></TD></TR>                                
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(10)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(8)券商專案現有客戶地址一覽表</FONT></TD></TR> 
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(11)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(9)已有二人申請客戶明細表(不含三人以上)</FONT></TD></TR>         
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(12)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(10)只有一人申請客戶明細表</FONT></TD></TR>                 
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(17)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(11)券商專案社區資料表</FONT></TD></TR>                 
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(18)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(12)券商專案客戶安裝進度明細表</FONT></TD></TR>
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(19)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(13)券商專案客戶建置明細表</FONT></TD></TR>
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(20)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(14)中華電信報竣客戶明細表(轉檔)</FONT></TD></TR>
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(21)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(15)ADSL券商專案送營運處社區資料表</FONT></TD></TR>
  <TR>
    <TD>&nbsp;</TD>
    <TD>&nbsp;</TD>
    <TD onclick="newurl(22)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt">(16)ADSL社區各組累計報竣數統計表</FONT></TD></TR>
       
    </TABLE>
    <p>
    <TABLE style="WIDTH: 587px; HEIGHT: 75px" cellSpacing=0 cellPadding=0 width=587 
bgColor=antiquewhite border=1>
        <TD style="WIDTH: 100%" bgColor=#3e7373 COLSPAN="2"> <P style="COLOR: yellow" align=center>其他報表</P></TD></TR>
  <TR>
    <TD WIDTH="50%" onclick="newurl(14)" onmouseover="rollIn(this)" onmouseout="rollOut(this)"
        language="Javascript1.2" dynamicanimation="fpAnimformatRolloverFP1" fprolloverstyle="font-size: 10pt; color: #FF0000; font-family: 細明體; background-color: #e0ffff; cursor:hand">
        <FONT style="COLOR: darkorchid;font-size:10pt" >券商專案回函信封列印</FONT></TD>
    <TD WIDTH="50%">&nbsp;</TD>
  </TR>
    </TABLE>
</CENTER>
</body></HTML>
