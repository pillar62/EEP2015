<%
	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=速博低用戶數一覽表.xls"

    parm=request("parm")
    v=split(parm,";")

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


 <Worksheet ss:Name="Sparq399用戶數<%=v(0)%>戶以下">
  <Table>
<!--
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="4" ss:StyleID="Title"><Data ss:Type="String">元訊寬頻網路股份有限公司</Data></Cell>
   </Row>
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="4" ss:StyleID="Title"><Data ss:Type="String">速博零用戶一覽表</Data></Cell>
   </Row>
   <Row>
    <Cell ss:MergeAcross="4" ss:StyleID="TitleDate"><Data ss:Type="String">製表日期：<%=now()%> </Data></Cell>
   </Row>
-->

   <Row>
    <Cell ss:StyleID="Header"><Data ss:Type="String">NO.</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">直/經銷</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區所屬</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">線路速率</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">專線號碼</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">測通日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">用戶數</Data></Cell>
   </Row>
<%
    sql="select case a.consignee when '' then '直銷' else '經銷' end as areanc, "_
       &"isnull(c.shortnc, g.cusnc) as belongnc, "_
       &"a.comn, isnull(h.codenc,'') as lineratenc, a.cmtytel, a.adslapply, b.num "_
       &"from 	RTSparqAdslCmty a "_
       &"left outer join (select comq1, count(*) as num	"_
       &"from RTSparqAdslCmty x "_
       &"inner join RTSparqAdslCust y on x.cutyid = y.comq1 and y.dropdat is null and y.docketdat is not null and y.freecode <>'Y' "_
       &"group by comq1) b on a.cutyid = b.comq1 "_
       &"left outer join RTObj c on c.cusid = a.consignee "_
       &"left outer join rtemployee f inner join rtobj g on g.cusid = f.cusid on f.emply=a.bussid "_
       &"left outer join rtcode h on h.code = a.linerate and h.kind ='D3' "_
       &"where 	a.rcomdrop is null "_
       &"and	 isnull(b.num, 0) <=" &V(0) _
       &"order by 1,2,3 "
	rs.Open sql, CONN
	i=0
	Do While Not rs.Eof
		i=i+1
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& i &"</Data></Cell>" &_
   			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("belongnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("lineratenc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cmtytel") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("adslapply") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("num") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


 <Worksheet ss:Name="Sparq499用戶數<%=v(0)%>戶以下">
  <Table>
   <Row>
    <Cell ss:StyleID="Header"><Data ss:Type="String">NO.</Data></Cell>
	<Cell ss:StyleID="Header"><Data ss:Type="String">直/經銷</Data></Cell>
    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">轄區所屬</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">社區序號</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">社區名稱</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">線路速率</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">附掛號碼</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">測通日</Data></Cell>
    <Cell ss:StyleID="Header"><Data ss:Type="String">用戶數</Data></Cell>
   </Row>
<%
    sql="select case a.consignee when '' then '直銷' else '經銷' end as areanc, "_
	   &"isnull(c.shortnc, j.cusnc) as belongnc, "_
       &"convert(varchar(5),a.comq1)+'-'+convert(varchar(2),a.lineq1) as comq, "_
       &"f.comn, isnull(h.codenc,'') as lineratenc, a.linetel,  a.adslopendat, b.num "_
       &"from RTSparq499CmtyLine a "_
       &"left outer join (select y.comq1, y.lineq1, count(*) as num	"_
       &"from RTSparq499CmtyLine x "_
       &"inner join RTSparq499Cust y on x.comq1 = y.comq1 and y.canceldat is null and y.dropdat is null and y.docketdat is not null and y.freecode <>'Y' "_
       &"group by y.comq1, y.lineq1) b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 "_
       &"left outer join RTObj c on c.cusid = a.consignee "_
	   &"left outer join rtemployee i inner join rtobj j on i.cusid = j.cusid on i.emply=a.salesid "_       
       &"inner join RTSparq499CmtyH f on f.comq1 = a.comq1 "_
       &"left outer join rtcode h on h.code = a.linerate and h.kind ='D3' "_
       &"where 	a.dropdat is null and a.canceldat is null "_
       &"and	 isnull(b.num, 0) <= " &V(0) _
       &" order by 1,2,3 "
	rs.Open sql, CONN
	i=0
	Do While Not rs.Eof
		i=i+1
	    response.Write "<Row>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& i &"</Data></Cell>" &_
   			"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("areanc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("belongnc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comq") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("lineratenc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("linetel") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("adslopendat") &"</Data></Cell>" &_
			"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rs("num") &"</Data></Cell>" &_
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
