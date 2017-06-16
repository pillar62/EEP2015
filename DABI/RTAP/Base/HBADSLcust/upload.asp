<%@ LANGUAGE = VBScript %>
<!-- #include file="uploadx.asp" -->
<!-- #include virtual="/Webap/include/database.inc" -->
<%
'Response.Write "<br>Name=""" & GetFormVal("name") & """"
'Response.Write "<br>Sex=""" & GetFormVal("sex") & """"
'Response.Write "<br>province=""" & GetFormVal("province") & """"
'Response.Write "<br>city=""" & GetFormVal("city") & """"
'Response.Write "<br>lover=""" & GetFormVal("lover") & """"
dim filename
path = Server.MapPath("./") & "\txt\"
filename = SaveFile("fruit",path,4096,0)
If	filename <> "*TooBig*" Then
    Set conn=Server.CreateObject("ADODB.Connection")
	conn.Open DSN
    '匯入文字檔
    sql= "BULK INSERT RTLib.dbo.HBCustDrop FROM '"& path & filename &"' "&_
      	 "WITH (FIRSTROW =2, FORMATFILE ='"& path & "upload.fmt') "
    conn.Execute(sql)
    '更新mis資料庫
    sql= "usp_HBCustDrop '1' "
    conn.Execute(sql)
    set conn = nothing
    Response.Write "<br><br>""" & filename & """文字檔上傳上傳完畢..."
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
