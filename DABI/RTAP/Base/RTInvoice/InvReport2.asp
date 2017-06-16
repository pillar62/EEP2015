<%
	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=發票一覽表.xls"
    v=split(request("parm"),";")
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
   <Alignment ss:Horizontal="Left" ss:Vertical="Center"/>
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

	<Style ss:ID="HeaderY">
		<Alignment ss:Horizontal="Center"/>
   		<Borders>
    		<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Bold="1"/>
   		<Interior ss:Color="#FFFF99" ss:Pattern="Solid"/>
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

	<Style ss:ID="toCharBrown">
		<Alignment ss:Vertical="Center"/>
		<Borders>
    		<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Color="brown"/>
   		<NumberFormat ss:Format="@"/>
  	</Style>

	<Style ss:ID="toCharWrap">
		<Alignment ss:Vertical="Center" ss:WrapText="1"/>
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

	<Style ss:ID="toNumBrown">
   		<Borders>
		    <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Color="brown"/>
  	</Style>

	<Style ss:ID="toDate">
   		<Borders>
		    <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman"/>
		<NumberFormat ss:Format="yyyy/m/d;@"/>
  	</Style>

	<Style ss:ID="toDateTime">
   		<Borders>
		    <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman"/>
		<NumberFormat ss:Format="yyyy/m/d\ h:mm;@"/>
  	</Style>

	<Style ss:ID="toDateTimeBrown">
   		<Borders>
		    <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Color="brown"/>
		<NumberFormat ss:Format="yyyy/m/d\ h:mm;@"/>
  	</Style>

 </Styles>

<%
    Dim rs, rsxx, rsyy, conn
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open "DSN=RTLib"
	sql="select year(invdat) as yearinv, convert(varchar(4),year(invdat))+'/1/1' as yearstr, convert(varchar(4),year(invdat))+'/12/31' as yearend " &_
		"from RTinvoice where year(invdat) between "& v(0) &" and " & v(1) &_
		" group by year(invdat), convert(varchar(4),year(invdat))+'/1/1', convert(varchar(4),year(invdat))+'/12/31' order by 1 "
	rs.Open sql, conn
	Do While Not rs.Eof
		yearinv = rs("yearinv")
		yearstr = rs("yearstr")
		yearend = rs("yearend")
%>
	 <Worksheet ss:Name="<%=yearinv %>年">
	  <Table ID="Table1">
	   <Row>
	    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">發票號碼</Data></Cell>
	    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">發票日期</Data></Cell>
		<Cell ss:StyleID="HeaderS"><Data ss:Type="String">發票抬頭</Data></Cell>
		<Cell ss:StyleID="HeaderS"><Data ss:Type="String">公司統編</Data></Cell>
	    <Cell ss:StyleID="HeaderS"><Data ss:Type="String">發票聯數</Data></Cell>

	    <Cell ss:StyleID="Header"><Data ss:Type="String">產品名稱</Data></Cell>
	    <Cell ss:StyleID="Header"><Data ss:Type="String">數量</Data></Cell>
	    <Cell ss:StyleID="Header"><Data ss:Type="String">單價</Data></Cell>
	    <Cell ss:StyleID="Header"><Data ss:Type="String">銷售額</Data></Cell>
	    <Cell ss:StyleID="Header"><Data ss:Type="String">稅額</Data></Cell>
	   </Row>

	<%
	    Set rsxx=Server.CreateObject("ADODB.Recordset")
	    Set rsyy=Server.CreateObject("ADODB.Recordset")

		sqlxx=	"select a.invno, convert(varchar(25), a.invdat, 126) as invdat, a.invtitle, a.unino, a.invtype, count(b.saleamt) as itemnum " &_
				"from RTInvoice a " &_
				"left outer join RTInvoiceSub b on a.invno = b.invno " &_
				"where invdat between '"& yearstr &"' and '"& yearend &"' " &_
				"group by a.invno, a.invdat, a.invtitle, a.unino, a.invtype " &_
				"order by 1 "
		'RESPONSE.WRITE sqlxx
		rsxx.Open sqlxx, CONN

		sqlyy=	"select a.invno, a.entry, a.prodnc, a.qty, a.unitamt, a.saleamt, a.taxamt " &_
		"from RTInvoiceSub a " &_
		"inner join RTInvoice b on a.invno = b.invno " &_
		"where invdat between '"& yearstr &"' and '"& yearend &"' " &_
		"order by 1,2 "
		'RESPONSE.WRITE sqlyy
		rsyy.Open sqlyy, CONN

		afirst = true
		Do While Not rsxx.Eof
			'發票主檔
			if afirst = true then 
				invno = rsxx("invno")	:	invdat = rsxx("invdat")		:	invtitle = rsxx("invtitle")
				unino = rsxx("unino")	:	invtype = rsxx("invtype")	:	itemnum = rsxx("itemnum")
				'memo = server.HTMLEncode(rsxx("memo"))
			else
				invno = ""	:	invdat = ""		:	invtitle = ""
				unino = ""	:	invtype = ""	:	itemnum = ""
			end if

			mergecount = rsxx("itemnum")

			'發票明細
			entry = ""		:	prodnc = ""		:	qty = ""
			unitamt = ""	:	saleamt = ""	:	taxamt = ""
			if not rsyy.EOF then 
				if rsyy("invno") = rsxx("invno") then
					entry = rsyy("entry")		:	prodnc = rsyy("prodnc")		:	qty = rsyy("qty")
					unitamt = rsyy("unitamt")	:	saleamt = rsyy("saleamt")	:	taxamt = rsyy("taxamt")
					rsyy.MoveNext
				end if 
			end if	

		    response.Write "<Row>"
			if afirst = true then
				if mergecount -1 > 0 then
					mergedown = " ss:MergeDown="""& mergecount -1 &""" "
				else
					mergedown = ""
				end if

				response.Write _
					"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& invno &"</Data></Cell>"

				if len(trim(invdat)) = 0 or isnull(invdat) then
					response.Write "<Cell "& mergedown &" ss:StyleID=""toDate""/>"
				else
					response.Write "<Cell "& mergedown &" ss:StyleID=""toDate""><Data ss:Type=""DateTime"">"& invdat &"</Data></Cell>"
				end if

				response.Write _
					"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& invtitle &"</Data></Cell>" &_
					"<Cell "& mergedown &" ss:StyleID=""toChar""><Data ss:Type=""String"">"& unino &"</Data></Cell>" &_
					"<Cell "& mergedown &" ss:StyleID=""toNum""><Data ss:Type=""Number"">"& invtype &"</Data></Cell>"
			end if

			if mergecount -1 > 0 and afirst = false then 
				mergedown = " ss:Index=""6"" "
			else
				mergedown = ""
			end if

			response.Write _
				"<Cell "& mergedown &" ss:StyleID=""toCharBrown""><Data ss:Type=""String"">"& prodnc &"</Data></Cell>"

			if len(trim(qty)) = 0 or isnull(qty) then
				response.Write "<Cell ss:StyleID=""toNumBrown""/>"
			else
				response.Write "<Cell ss:StyleID=""toNumBrown""><Data ss:Type=""Number"">"& qty &"</Data></Cell>"
			end if
			if len(trim(unitamt)) = 0 or isnull(unitamt) then
				response.Write "<Cell ss:StyleID=""toNumBrown""/>"
			else
				response.Write "<Cell ss:StyleID=""toNumBrown""><Data ss:Type=""Number"">"& unitamt &"</Data></Cell>"
			end if
			if len(trim(saleamt)) = 0 or isnull(saleamt) then
				response.Write "<Cell ss:StyleID=""toNumBrown""/>"
			else
				response.Write "<Cell ss:StyleID=""toNumBrown""><Data ss:Type=""Number"">"& saleamt &"</Data></Cell>"
			end if
			if len(trim(taxamt)) = 0 or isnull(taxamt) then
				response.Write "<Cell ss:StyleID=""toNumBrown""/>"
			else
				response.Write "<Cell ss:StyleID=""toNumBrown""><Data ss:Type=""Number"">"& taxamt &"</Data></Cell>"
			end if

		    response.Write "</Row>"

			bend = false
			if rsyy.EOF then
				bend =true
			else
				if rsxx("invno") <> rsyy("invno") then bend =true
			end if 

			afirst = false
			if bend = true then
				afirst = true
				rsxx.MoveNext
			end if
	    Loop

	    rsxx.Close
	    rsyy.Close
	%>
	  </Table>
	 </Worksheet>
<%
		rs.MoveNext
	Loop
	rs.Close
	set rs = nothing
	set rsxx = nothing
	set rsyy = nothing
	conn.Close
	set conn = nothing
%>
</Workbook>
