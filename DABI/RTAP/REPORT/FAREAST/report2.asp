<%
	Response.ContentType = "application/vnd.ms-excel"
	Response.AddHeader "Content-Disposition","filename=元訊電子檔-" & cstr(datepart("yyyy",now())) & "-" & cstr(datepart("m",now())) & "-" & cstr(datepart("d",now())) & ".xls"

    parm=request("parm")
    xparm=split(parm,";")
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


 <Worksheet ss:Name="元訊開通導入">
  <Table>
   <Row ss:AutoFitHeight="0">
    <Cell ss:MergeAcross="48" ss:StyleID="Title"><Data ss:Type="String">元訊電子轉檔報竣及異動</Data></Cell>
   </Row>
   <Row>
    <Cell ss:MergeAcross="48" ss:StyleID="TitleDate"><Data ss:Type="String">文檔日期：<%=now()%> </Data></Cell>
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
<Cell ss:StyleID="Header"><Data ss:Type="String">戶籍郵遞區號</Data></Cell>
<Cell ss:StyleID="Header"><Data ss:Type="String">戶籍縣市</Data></Cell>
<Cell ss:StyleID="Header"><Data ss:Type="String">戶籍鄉鎮市區</Data></Cell>
<Cell ss:StyleID="Header"><Data ss:Type="String">戶籍地址</Data></Cell>
<Cell ss:StyleID="Header"><Data ss:Type="String">裝機郵遞區號</Data></Cell>
<Cell ss:StyleID="Header"><Data ss:Type="String">裝機縣市</Data></Cell>
<Cell ss:StyleID="Header"><Data ss:Type="String">裝機鄉鎮市區</Data></Cell>
<Cell ss:StyleID="Header"><Data ss:Type="String">裝機地址</Data></Cell>
<Cell ss:StyleID="Header"><Data ss:Type="String">社區名稱</Data></Cell>
<Cell ss:StyleID="Header"><Data ss:Type="String">IP ADDRESS FROM</Data></Cell>
<Cell ss:StyleID="Header"><Data ss:Type="String">IP ADDRESS END</Data></Cell>
<Cell ss:StyleID="Header"><Data ss:Type="String">NCIC預處理日期</Data></Cell>
<Cell ss:StyleID="Header"><Data ss:Type="String">第二證照類別</Data></Cell>
<Cell ss:StyleID="Header"><Data ss:Type="String">第二證照號碼</Data></Cell>
<Cell ss:StyleID="HeaderS"><Data ss:Type="String">申請書編號</Data></Cell>
   </Row>
<%      rs.Open "usp_RTFarEastapplylist '" & xparm(0) & "','" & xparm(1) & "'", CONN
	serno=0				   
	Do While Not rs.Eof
	    serno = serno+1
            xserno=right("0000" & cstr(serno),4)
            yserno=rs("seq") & xserno
            '優惠代碼
             if rs("paycycle")="06" then
                if rs("case_no")="1054" then
                   xxx="SBAA140603-007"
                elseif rs("case_no")="1053" then
                   xxx="SBAA140602-007"
                elseif rs("case_no")="1052" then
                   xxx="SBAA140601-007"
                else
                   xxx=""
               end if
             elseif rs("paycycle")="02" then
               if rs("case_no")="1054" then
                  xxx="SBAA140603-005"
               elseif rs("case_no")="1053" then
                  xxx="SBAA140602-005"
               elseif rs("case_no")="1052" then
                  xxx="SBAA140601-005"
               else
                 xxx=""
               end if
             elseif rs("paycycle")="03" then
               if rs("case_no")="1054" then
                  xxx="SBAA140603-006"
               elseif rs("case_no")="1053" then
                  xxx="SBAA140602-006"
               elseif rs("case_no")="1052" then
                  xxx="SBAA140601-006"
               else
                  xxx=""
               end if
             else
               xxx=""
             end if
	    response.Write "<Row>" &_
		"<Cell ss:StyleID=""toNum""><Data ss:Type=""String"">"& yserno &"</Data></Cell>" &_
		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("applydat") &"</Data></Cell>" &_
		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("docketdat") &"</Data></Cell>" &_
		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("applykind") &"</Data></Cell>" &_
		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("updcode") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("updtel") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("memberid") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("mak_id") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("sale_id") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cust_kind") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("company_name") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("coboss") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cobossid") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("company_id") &"</Data></Cell>" &_
                "<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("case_no") &"</Data></Cell>" &_
		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& xxx &"</Data></Cell>" &_
		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cusnc") &"</Data></Cell>" &_
		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("codenc") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("socialid") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("sex_kind") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("contact_name") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("contact_tel") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("contact_birth") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("contact_mobile") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("agent_cardtype") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("agent_idno") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("agent_callname") &"</Data></Cell>" &_
                "<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("agent_name") &"</Data></Cell>" &_
		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("agent_tel") &"</Data></Cell>" &_
		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("agent_birth") &"</Data></Cell>" &_
		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("zip3") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cutnc3") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("township3") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("raddr3") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("zip2") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cutnc2") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("township2") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("raddr2") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("zip1") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("cutnc1") &"</Data></Cell>" &_
                "<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("township1") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("raddr1") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("comn") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("ip11s") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("ip11e") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("ncicdate") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("codenc2") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("secondno") &"</Data></Cell>" &_
    		"<Cell ss:StyleID=""toChar""><Data ss:Type=""String"">"& rs("apply_no") &"</Data></Cell>" &_
			"</Row>"
      rs.MoveNext
    Loop
    rs.Close
%>
  </Table>
 </Worksheet>


</Workbook>
