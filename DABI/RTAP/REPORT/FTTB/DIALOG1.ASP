<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>HiBuilding客戶移轉FTTB申請書</TITLE>
<SCRIPT language=VBScript>

Sub cmdSure_onClick
  rpttype=document.all("search1").value
  comn=document.all("search2").value
  stat=document.all("search3").value
  if len(comn)=0 then
  		comn ="*"
  end if

  if rpttype="A" then
  		PGM="/report/fttb/HBReport1a.asp?parm=" 
  elseif rpttype ="B" then
  		PGM="/report/fttb/HBReport1b.asp?parm=" 
  elseif rpttype ="C" then
  		PGM="/report/fttb/HBReport1c.asp?parm=" 
  elseif rpttype ="E" then
  		PGM="/report/fttb/HBReport1e.asp?parm=" 
  elseif rpttype ="F" then
  		PGM="/report/fttb/HBReport1f.asp?parm=" 
  elseif rpttype ="G" then
  		PGM="/report/fttb/HBReport1g.asp?parm=" 
  end if

  'pgm=pgm & comm & ";" & Rtrim(groupid) & ";" & eusr
  pgm=pgm & comn &";"& stat
  window.open pgm 
  'window.close
End Sub

Sub cmdcancel_onClick
  window.close
End Sub

</SCRIPT>
</HEAD>
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codeBase=http://www.cbbn.com.tw/stock/EF2KDT.CAB#version=9,0,0,3 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="background:lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">HI-BUILDING－報表列印</font></i> </DIV>
<DIV align=Center><i><font face="標楷體" size="3" color="#FF00FF">HiBuilding客戶移轉FTTB申請書 & 信封 </font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<tr><td ALIGN="RIGHT"><font face="標楷體">列印種類 :</font></td>
    <td><select name="search1" size="1" class=dataListEntry>
		<option value="A" selected>HiBuilding客戶移轉FTTB申請書</option>
		<option value="B">中華電信大電視申請書</option>
		<option value="C">FTTB申請清單</option>
		<option value="E">FTTB開通社區客戶信封列印</option>
		<option value="F">FTTB開通社區管委會函文列印</option>
		<option value="G">FTTB開通社區管委會信封列印</option>
        </select>
    </td></tr>
<tr><td ALIGN="RIGHT"><font face="標楷體">客戶狀態 :</font></td>
    <td><select name="search3" size="1" class=dataListEntry>
		<option value="A">全部</option>
		<option value="B">有效戶</option>
		<option value="C">退租戶</option>
		<option value="D">欠拆戶</option>
		<option value="E" selected>有效+欠拆</option>
        </select>
    </td></tr>
<tr><td ALIGN="RIGHT"><font face="標楷體">請輸入社區名稱 :</font></td>
	<td><input type="text" size="20" maxlength="30" name="search2" class=dataListEntry >
	</td></tr>
</table>
<p><center><font face="標楷體">
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand">
 </center>
  <HR>
  <P>PS.信封的列印：請設定列印格式, 將紙張大小由[A4]更改為[CBBN信封], 或自行設定寬度21cm, 高度12.7cm
  </P>
</BODY> 
</HTML>

