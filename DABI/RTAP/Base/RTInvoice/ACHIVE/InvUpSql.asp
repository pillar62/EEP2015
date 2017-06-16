<%@ LANGUAGE = VBScript %>
<!-- #include file="uploadx.asp" -->
<%
'Response.Write "<br>Name=""" & GetFormVal("name") & """"
'Response.Write "<br>Sex=""" & GetFormVal("sex") & """"
'Response.Write "<br>province=""" & GetFormVal("province") & """"
'Response.Write "<br>city=""" & GetFormVal("city") & """"
'Response.Write "<br>lover=""" & GetFormVal("lover") & """"
dim filename
path = Server.MapPath("./") & "\excel\"
filename = SaveFile("fruit",path,8192,2)
If	filename <> "*TooBig*" Then
	'匯入EXCEL檔至資料庫
	Dim XSLconn, SQLconn, rs
	Set XSLconn = Server.CreateObject("ADODB.Connection")
	Set SQLconn = Server.CreateObject("ADODB.Connection")
	Set rs = Server.CreateObject("ADODB.Recordset")

	SQLconn.Open "DSN=RTLib"
	XSLconn.Open "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & path & filename & ";Extended Properties=Excel 8.0;"
	
	'刪除舊資料 ===============================================================================		
	sql = "delete from rtinvtemp where batch is null"
	SQLconn.Execute sql					 

	'二聯匯入 =================================================================================
	intI=0
	Set rs = XSLconn.Execute("SELECT 2 as INVTYPE,* FROM [二聯$] WHERE [銷售品名] is not null")
	While Not rs.EOF
		For intJ = 0 to 21 
			if rs.Fields(intJ).Value <> Empty then 
			col = rs.Fields(intJ).Value
			else
			col=""    	       
			end if

			select case intJ
				case 0			'第一欄(二聯or三聯)
					 row = col
				case 1,2,6,10,11,12,13,14,15,16,18,19,20,21
					 row = row &","""& col &""""
				case 3,4,5,7,8	'數字欄
					if len(col)=0 then col="0"
					 row = row &","& col
				case 9			'日期欄
					 row = row &",'"& col &"'"			
				case else 
					 row = row
			end select
			'Response.Write rs.Fields(0) & "<br>"
		Next						 
		'Response.Write row & "<br>"
		sql = "INSERT INTO rtinvtemp (INVTYPE, GROUPNC, PRODNC, QTY, UNITAMT, RCVAMT, "&_
			"TAXTYPE, SALEAMT, TAXAMT, INVDAT, INVNO, UNINO, INVTITLE, "&_
			"社區名稱, 用戶名稱, 地址, 聯絡電話, 施工人員, "&_
			  "業務開發單位, 業務開發人員, 備註) VALUES (" & row & ")"
'response.Write sql				  
		SQLconn.Execute sql					 
		intI = intI +1
		rs.MoveNext
	Wend
	Response.Write "二聯共上傳" & intI  & "筆<br>"
	rs.Close

	'三聯匯入 =================================================================================
	intI=0
	Set rs = XSLconn.Execute("SELECT 3 as INVTYPE,* FROM [三聯$] WHERE [銷售品名] is not null")
	While Not rs.EOF
		For intJ = 0 to 21 
			if rs.Fields(intJ).Value <> Empty then 
			col = rs.Fields(intJ).Value
			else
			col=""    	       
			end if

			select case intJ
				case 0			'第一欄(二聯or三聯)
					row = col
				case 1,2,6,10,11,12,13,14,15,16,18,19,20,21
					row = row &","""& col &""""
				case 3,4,5,7,8	'數字欄
					if len(col)=0 then col="0"
					row = row &","& col
				case 9			'日期欄
					 row = row &",'"& col &"'"			
				case else 
					 row = row
			end select
			'Response.Write rs.Fields(0) & "<br>"
		Next						 
		'Response.Write row & "<br>"
		sql ="INSERT INTO rtinvtemp (INVTYPE, GROUPNC, PRODNC, QTY, UNITAMT, RCVAMT, "&_
			"TAXTYPE, SALEAMT, TAXAMT, INVDAT, INVNO, UNINO, INVTITLE, "&_
			"社區名稱, 用戶名稱, 地址, 聯絡電話, 施工人員, "&_
			  "業務開發單位, 業務開發人員, 備註) VALUES (" & row & ")"
'response.Write sql			  
		SQLconn.Execute sql
		intI = intI +1			 
		rs.MoveNext
	Wend
	Response.Write "三聯共上傳" & intI  & "筆<br>"
	rs.Close

	set rs = nothing
	SQLconn.Close
	XSLconn.Close
	set SQLconn = nothing
	set XSLconn = nothing    
    '更新mis資料庫
    'sql= "usp_HBCustDrop '1' "
    'conn.Execute(sql)
    'set conn = nothing
    Response.Write "<br><br>""" & filename & """ 文字檔上傳完畢..."
    Response.Write "<p><a href=""/rtap/Base/RTInvoice/InvoicePRT.asp"">發票列印</a></p>"
Else
	Response.Write "<br><br>文件大小超出 4 MB的限制"
End IF

'filename = SaveFile("fruit2",path,1024,0)
'If filename <> "*TooBig*" Then
'Response.Write "<br><br>""" & filename & """已經上傳"
'Else
'Response.Write "<br><br>文件超出限制太大"
'End IF
%>
 
<html>
	<head>
		<title>上傳結果</title>
		
<script language=vbscript>
Sub cmdcancel_onClick
  window.close
End Sub
</script>		

	</head>
	<BODY style="BACKGROUND: lightblue">
		<p><center>
			  <INPUT TYPE="button" VALUE="關閉" ID="cmdcancel" NAME="cmdcancel">
		</center><HR>			  
		</p>
	</body>
</html>
