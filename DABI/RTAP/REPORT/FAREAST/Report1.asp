<!-- #include virtual="/Webap/include/employeeref.inc" -->

<%
	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=收款明細表(遠傳大寬頻).xls"

	logonid=Request.ServerVariables("LOGON_USER")
	Call SrGetEmployeeRef(Rtnvalue,1,logonid)
	logonid=split(rtnvalue,";")  

    parm=request("parm")
    v=split(parm,";")

    Dim rs,conn,rsx
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")
    Set rsx=Server.CreateObject("ADODB.Recordset")    
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

  <Style ss:ID="SubTitle">
   <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
   		<Borders>
    		<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   <Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Size="10"/>
   <Interior ss:Color="#FFFF99" ss:Pattern="Solid"/>
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

	<Style ss:ID="toDate">
   		<Borders>
		    <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman"/>
		<NumberFormat ss:Format="Short Date"/>
  	</Style>

	<Style ss:ID="toNum">
   		<Borders>
		    <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
    		<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#000000"/>
   		</Borders>
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman"/>
   		<NumberFormat ss:Format="#,##0_ ;[Red]\-#,##0\ "/>
  	</Style>

	<Style ss:ID="toSum">
   		<Font ss:FontName="新細明體" x:CharSet="136" x:Family="Roman" ss:Color="Blue" ss:Bold="1"/>
   		<NumberFormat ss:Format="#,##0_ ;[Red]\-#,##0\ "/>
  	</Style>
 </Styles>


 <Worksheet ss:Name="fareast">
  <Table>
   <Row ss:AutoFitHeight="0"> 
    <Cell ss:MergeAcross="15" ss:StyleID="Title"><Data ss:Type="String">元訊寬頻網路股份有限公司</Data></Cell>
   </Row>
<!--
   <Row>
    <Cell ss:MergeAcross="4" ss:StyleID="TitleDate"><Data ss:Type="String">製表日期：<%=now()%> </Data></Cell>
   </Row>
-->
<%
	sql="select code, codenc from rtcode where kind ='M9' "
	rs.Open sql, CONN
	Do While Not rs.Eof
		sqlstr="usp_RTRcvMoneyList_fareast '" &v(0)& "', '" &v(1)& "', 'F" &rs("code")& "' "
		rsx.Open sqlstr, CONN

		if Not rsx.Eof then
			response.Write "<Row ss:AutoFitHeight=""0"">" &_
				"<Cell ss:MergeAcross=""15"" ss:StyleID=""SubTitle""><Data ss:Type=""String"">收款明細表(" &rs("codenc")& ")</Data></Cell>" &_
				"</Row>"
			response.Write "<Row>" &_
				"<Cell ss:StyleID=""HeaderS""><Data ss:Type=""String"">組別(工程師)</Data></Cell>" &_
				"<Cell ss:StyleID=""HeaderS""><Data ss:Type=""String"">銷售品名</Data></Cell>" &_
				"<Cell ss:StyleID=""HeaderS""><Data ss:Type=""String"">數量</Data></Cell>" &_
				"<Cell ss:StyleID=""HeaderS""><Data ss:Type=""String"">收款金額</Data></Cell>" &_
				"<Cell ss:StyleID=""HeaderS""><Data ss:Type=""String"">統編</Data></Cell>" &_
				"<Cell ss:StyleID=""HeaderS""><Data ss:Type=""String"">發票抬頭</Data></Cell>" &_
				"<Cell ss:StyleID=""HeaderS""><Data ss:Type=""String"">社區名稱</Data></Cell>" &_
				"<Cell ss:StyleID=""HeaderS""><Data ss:Type=""String"">用戶名稱</Data></Cell>" &_
				"<Cell ss:StyleID=""HeaderS""><Data ss:Type=""String"">帳單地址</Data></Cell>" &_
				"<Cell ss:StyleID=""HeaderS""><Data ss:Type=""String"">電話</Data></Cell>" &_
				"<Cell ss:StyleID=""HeaderS""><Data ss:Type=""String"">報竣日</Data></Cell>" &_
				"<Cell ss:StyleID=""HeaderS""><Data ss:Type=""String"">收款日</Data></Cell>" &_
				"<Cell ss:StyleID=""HeaderS""><Data ss:Type=""String"">起始日</Data></Cell>" &_
				"<Cell ss:StyleID=""HeaderS""><Data ss:Type=""String"">終止日</Data></Cell>" &_
				"<Cell ss:StyleID=""HeaderS""><Data ss:Type=""String"">備註</Data></Cell>" &_
				"<Cell ss:StyleID=""HeaderS""><Data ss:Type=""String"">保證金序號</Data></Cell>" &_
				"</Row>"
		end if

		serno=0
		Do While Not rsx.Eof
			serno = serno + 1
			response.Write "<Row>" &_
				"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsx("belongnc") &"</Data></Cell>" &_
				"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsx("amtnc") &"</Data></Cell>" &_
				"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rsx("qty") &"</Data></Cell>" &_
				"<Cell ss:StyleID=""toNum""><Data ss:Type=""Number"">"& rsx("amt") &"</Data></Cell>" &_
				"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsx("unino") &"</Data></Cell>" &_
				"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsx("invtitle") &"</Data></Cell>" &_
				"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsx("comn") &"</Data></Cell>" &_
				"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsx("cusnc") &"</Data></Cell>" &_
				"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsx("raddr") &"</Data></Cell>" &_
				"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsx("tel")	&"</Data></Cell>" &_
				"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rsx("docketdat") &"</Data></Cell>" &_
				"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rsx("rcvmoneydat")	&"</Data></Cell>" &_
				"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rsx("strbillingdat") &"</Data></Cell>" &_
				"<Cell ss:StyleID=""toDate""><Data ss:Type=""String"">"& rsx("duedat") &"</Data></Cell>" &_
				"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsx("memo") &"</Data></Cell>" &_
				"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rsx("gtserial") &"</Data></Cell>" &_
				"</Row>"
			rsx.MoveNext
		Loop
		rsx.Close
		
		if serno >0 then
			response.Write "<Row>" &_
				"<Cell></Cell>" &_
				"<Cell></Cell>" &_
				"<Cell></Cell>" &_
				"<Cell ss:StyleID=""toSum"" ss:Formula=""=SUM(R[-"&serno&"]C:R[-1]C)""><Data ss:Type=""Number""></Data></Cell>" &_
				"<Cell></Cell>" &_
				"<Cell></Cell>" &_
				"<Cell></Cell>" &_
				"<Cell></Cell>" &_
				"<Cell></Cell>" &_
				"<Cell></Cell>" &_
				"<Cell></Cell>" &_
				"<Cell></Cell>" &_
				"<Cell></Cell>" &_
				"<Cell></Cell>" &_
				"<Cell></Cell>" &_
				"</Row><Row></Row>"
			'response.Write "<Row><Cell ss:MergeAcross=""14""></Cell></Row><Row><Cell ss:MergeAcross=""14""></Cell></Row>"
		end if
		rs.MoveNext
	Loop
	rs.Close
	conn.Close
	
	set rsx = nothing
	set rs = nothing
	set conn = nothing

	'response.Write "<Row ss:AutoFitHeight=""0"">" &_
	'   "<Cell ss:MergeAcross=""14"" ss:StyleID=""SubTitle""><Data ss:Type=""String"">會計:                     財務:                       營運部:                       部門主管:                         單位主管:                        製表人:" &logonid(1)& "</Data></Cell>" &_
	'   "</Row>"
%>
  </Table>
 </Worksheet>

</Workbook>
