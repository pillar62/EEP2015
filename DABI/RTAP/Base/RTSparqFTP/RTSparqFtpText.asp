<%
    parm=request("parm")
    v=split(parm,";")

	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename="& replace(v(0), "/", "") & ".xls"
%>

<?xml version="1.0" encoding="Big5"?>
<Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet"
 xmlns:o="urn:schemas-microsoft-com:office:office"
 xmlns:x="urn:schemas-microsoft-com:office:excel"
 xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet"
 xmlns:html="http://www.w3.org/TR/REC-html40">

 <ExcelWorkbook xmlns="urn:schemas-microsoft-com:office:excel">
  <ActiveSheet>0</ActiveSheet>
  <ProtectStructure>False</ProtectStructure>
  <ProtectWindows>False</ProtectWindows>
 </ExcelWorkbook>
 
 <Styles>
	<Style ss:ID="Default" ss:Name="Normal">
		<Alignment ss:Vertical="Center"/>
		<Borders/>
		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Size="12"/>
		<Interior/>
		<NumberFormat/>
		<Protection/>
  	</Style>

  <Style ss:ID="Title">
   <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
   <Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Size="12" ss:Bold="1"/>
  </Style>

  <Style ss:ID="TitleDate">
   <Alignment ss:Horizontal="Left" ss:Vertical="Center" ss:WrapText="1"/>
   <Borders>
    <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   </Borders>
   <Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman"/>
  </Style>

	<Style ss:ID="Header">
		<Alignment ss:Horizontal="Center"/>
   		<Borders>
    		<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Bold="1"/>
   		<Interior ss:Color="peachpuff" ss:Pattern="Solid"/>
  	</Style>

	<Style ss:ID="HeaderS">
		<Alignment ss:Horizontal="Center"/>
   		<Borders>
    		<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Bold="1"/>
   		<Interior ss:Color="Silver" ss:Pattern="Solid"/>
  	</Style>

	<Style ss:ID="toChar">
		<Borders>
    		<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman"/>
   		<NumberFormat ss:Format="@"/>
  	</Style>

	<Style ss:ID="toNum">
   		<Borders>
		    <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman"/>
  	</Style>
 </Styles>


 <Worksheet ss:Name="Sparq399">
  <Table ID="Table1">
<!--
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="4" ss:StyleID="Title"><Data ss:Type="String">元訊寬頻網路股份有限公司</Data></Cell>
   </Row>
-->   
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="49" ss:StyleID="Title"><Data ss:Type="String">元訊電子轉檔報竣及異動</Data></Cell>
   </Row>
   <Row>
    <Cell ss:MergeAcross="49" ss:StyleID="TitleDate"><Data ss:Type="String">文檔日期：<%=now()%> </Data></Cell>
   </Row>

   <Row>
    <Cell ss:StyleID="Header"><Data ss:Type="String">流水號</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">申請日期</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">竣工日期</Data></Cell>
	<Cell ss:StyleID="Header"><Data ss:Type="String">申請種類</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">異動代碼</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">電話號碼</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">對帳號碼</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">協力商代碼</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">業務員代碼</Data></Cell>    
    <Cell ss:StyleID="Header"><Data ss:Type="String">客戶類別</Data></Cell>    
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">公司名稱</Data></Cell>    
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">公司負責人</Data></Cell>    
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">負責人身份證字號</Data></Cell>    
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">公司統編</Data></Cell>    
    <Cell ss:StyleID="Header"><Data ss:Type="String">服務方案</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">優惠代碼</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">用戶名稱</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">聯絡人證照類別</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">聯絡人證照號碼</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">聯絡人稱謂</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">聯絡人名稱</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">聯絡人聯絡電話</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">聯絡人出生日期</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">聯絡人行動電話</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">代理人證照類別</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">代理人證照號碼</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">代理人稱謂</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">代理人名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">代理人聯絡電話</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">代理人出生日期</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">帳寄郵遞區號</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">帳寄縣市</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">帳寄鄉鎮市區</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">帳寄地址</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">戶籍郵遞區號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">戶籍縣市</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">戶籍鄉鎮市區</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">戶籍地址</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">裝機郵遞區號</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">裝機縣市</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">裝機鄉鎮市區</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">裝機地址</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">IP ADDRESS FROM</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">IP ADDRESS END</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">NCIC預處理日期</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">第二證照類別</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">第二證照號碼</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">申請書編號</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">預繳款單編號</Data></Cell>
   </Row>
<%
    Dim rs,conn
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
	sql= "usp_RTSparqFtp '2','" & v(0) & "' "
	
	rs.Open sql, CONN
	Do While Not rs.Eof
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("FORMID") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("RCVD") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("DOCKETDAT") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("APPLYKIND") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("UPDEVENT") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("EXTTEL") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("SPHNNO") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("COCOCODE") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("SALESID") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CUSTKIND") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CONAME") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CORESPMAN") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CORESPID") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("COID") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CONNECTTYPE") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CONNFEEOFF") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CUSNC") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CONTACTIDTYPE") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CONTACTID") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CONTACTSALU") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CONTACT") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CONTACTTEL") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CONTACTBIRTH") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CONTACTNOBILE") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("AGENTIDTYPE") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("AGENTID") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("AGENTSALU") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("AGENT") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("AGENTTEL") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("AGENTBIRTH") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("RZONE1") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CUTNC1") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("TOWNSHIP1") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("RADDR1") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("RZONE3") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CUTNC3") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("TOWNSHIP3") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("RADDR3") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("RZONE2") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CUTNC2") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("TOWNSHIP2") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("RADDR2") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("COMN") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("IPADDR") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("IPADDR") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("NCICPREDATE") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CONTACTIDTYPE2") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("CONTACTID2") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("APPLYNO") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String""></Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
	conn.Close
	set rs = nothing
	set conn = nothing
%>
  </Table>
 </Worksheet>

</Workbook>
