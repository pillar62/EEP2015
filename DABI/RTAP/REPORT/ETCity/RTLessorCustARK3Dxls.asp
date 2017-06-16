<%
    parm=request("key")
    v=split(parm,";")

	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=ET應收應付列表"&v(0)&"-"&v(1)&"("&v(2)&").xls"

    Dim rs,conn
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
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
   <Alignment ss:Horizontal="Right" ss:Vertical="Center" ss:WrapText="1"/>
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


 <Worksheet ss:Name="<%=v(0)%>-<%=v(1)%><%=v(2)%>">
  <Table>
<!--
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="4" ss:StyleID="Title"><Data ss:Type="String">元訊寬頻網路股份有限公司</Data></Cell>
   </Row>
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="4" ss:StyleID="Title"><Data ss:Type="String">速博退租戶一覽表</Data></Cell>
   </Row>
   <Row>
    <Cell ss:MergeAcross="4" ss:StyleID="TitleDate"><Data ss:Type="String">製表日期：<%=now()%> </Data></Cell>
   </Row>
-->
   <Row>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">NO.</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">客戶</Data></Cell>
	<Cell ss:StyleID="HeaderS"><Data ss:Type="String">帳款編號</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">會計科目</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">立帳年月</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">項目名稱</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">正/負</Data></Cell>    
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">應收(付)金額</Data></Cell>    
    <Cell ss:StyleID="Header"><Data ss:Type="String">己沖金額</Data></Cell>    
    <Cell ss:StyleID="Header"><Data ss:Type="String">未沖金額</Data></Cell>    
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">產生日</Data></Cell>    
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">沖帳日</Data></Cell>    
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">作廢日</Data></Cell>    
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">作廢原因</Data></Cell>    
    
   </Row>
<%
     if v(2) ="心視界收款" then
		sql = " and c.RCVMONEYFLAG1 + substring(a.batchno,1,2) ='YBH' "
	 else
		sql =" and c.RCVMONEYFLAG1 + substring(a.batchno,1,2)<>'YBH' "
	 end if  	

	sql="SELECT 	d.comn, c.cusnc, a.BATCHNO, b.ACNAMEC, " &_
		"convert(varchar(4),a.syy)+'/'+convert(varchar(2),a.smm) as yymm, a.ITEMNC, a.PORM, a.AMT, a.REALAMT, " &_
		"a.AMT - a.REALAMT as surplus, a.CDAT, a.MDAT, a.CANCELDAT, a.CANCELMEMO " &_
		"FROM 	RTLessorCustARDTL a " &_
		"left outer join RTAccountNo b ON a.L14 = b.L14 AND a.L23 = b.L23 " &_
		"left outer join RTLessorCUST c ON a.CUSID=c.CUSID " &_
		"left outer join RTLessorCMTYH d ON c.COMQ1=d.COMQ1 " &_
		"WHERE 	a.canceldat is null " &_ 
		"AND 	a.TYY= "& v(0) &" and a.TMM= " & v(1) & sql &_
		" ORDER BY a.CUSID, a.SEQ "
	rs.Open sql, CONN
	serno=0				   
	Do While Not rs.Eof
	    serno = serno+1
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& serno &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("batchno") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("acnamec") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("yymm") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("itemnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("porm") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("amt") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("realamt") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("surplus") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cdat") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("mdat") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("canceldat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cancelmemo") &"</Data></Cell>" &_
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
