<%@ Language=VBScript%>
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<% 
  Dim conn, rs, sql, comn
  
  key=Split(Request("key"),";")

  set conn=server.CreateObject("ADODB.Connection")
  set rs=server.CreateObject("ADODB.Recordset")
  DSN="DSN=RTLib"
  Conn.Open DSN

  if len(trim(key(1))) = 0 then key(1) ="不存在"
  if key(0) ="03" then		'Sparq 399'
		sql="select cutyid as comq1, 0 as LINEQ1, COMN, convert(varchar(5), cutyid) as comq, ipaddr as ip1, '' as ip2, cmtytel as linetel, rcomdrop as dropdat, '' as canceldat " _
		   &"from RTSparqAdslCmty "_
		   &"where comn like '%" &key(1)& "%' "_
		   &"ORDER BY COMN "
  elseif key(0) ="05" then	'Sparq 499'
		sql="select b.COMQ1, b.LINEQ1, a.COMN, convert(varchar(5), b.comq1)+'-'+ convert(varchar(3), b.lineq1) as comq, b.gateway as ip1, b.idslamip as ip2, b.linetel, b.dropdat, b.canceldat "_
		   &"from RTSparq499CmtyH a inner join RTSparq499CmtyLine b on a.COMQ1 = b.COMQ1 "_
		   &"where a.comn like '%" &key(1)& "%' "_
		   &"ORDER BY a.COMN, b.lineq1 "
  elseif key(0) ="06" then	'ET
		sql="select b.COMQ1, b.LINEQ1, a.COMN, convert(varchar(5), b.comq1)+'-'+ convert(varchar(3), b.lineq1) as comq, b.lineip as ip1, '' as ip2, b.linetel, b.dropdat, b.canceldat "_
		   &"from RTLessorCmtyH a inner join RTLessorCmtyLine b on a.COMQ1 = b.COMQ1 "_
		   &"where a.comn like '%" &key(1)& "%' "_
		   &"ORDER BY a.COMN, b.lineq1 "
  elseif key(0) ="07" then	'AVS
		sql="select b.COMQ1, b.LINEQ1, a.COMN, convert(varchar(5), b.comq1)+'-'+ convert(varchar(3), b.lineq1) as comq, b.lineip as ip1, '' as ip2, b.linetel, b.dropdat, b.canceldat "_
		   &"from RTLessorAvsCmtyH a inner join RTLessorAvsCmtyLine b on a.COMQ1 = b.COMQ1 "_
		   &"where a.comn like '%" &key(1)& "%' "_
		   &"ORDER BY a.COMN, b.lineq1 "
  else
		sql=""
  end if
'Response.Write SQL
  rs.Open sql,conn
  s1=""
  Do While Not rs.Eof
     s1=s1 &"<option value=""" &rs("COMQ1") &";"& rs("LINEQ1") &";"& rs("IP1") &";"& rs("IP2") &";"& rs("linetel") &";"& rs("dropdat") &";"& rs("canceldat") &""">["&rs("COMQ")&"]　" &rs("COMN") &"</option>"
     rs.MoveNext
  Loop
  rs.Close    

  conn.Close   
  set rs=Nothing   
   set conn=Nothing
%>
<HTML>
<HEAD>
	<meta http-equiv="Content-Type" content="text/html; charset=Big5">
	<TITLE>社區選擇清單</TITLE>
</HEAD>
<BODY style="BACKGROUND: lightblue">
<SCRIPT LANGUAGE="VBScript">
  Sub lstOrder1_onclick()
      selno=lstorder1.selectedIndex
      if selno >=0 then
         window.document.all("cmdtext").value= lstOrder1(selno).innerHTML
         window.document.all("cmdtext1").value=lstOrder1(selno).value
         window.document.all("cmdtext2").value="Y"         
      end if
  End Sub
  
  Sub cmdSure_onClick()
    ReturnValue=""
    'if len(trim(window.document.all("cmdtext").value)) = 0 then
    '   msgbox "請選擇鄉鎮市區!",vbokonly,"錯誤訊息視窗"
    'else    
       returnvalue= window.document.all("cmdtext1").value &";"& window.document.all("cmdtext").value &";"& window.document.all("cmdtext2").value
       window.close
    'end if
  End Sub

  Sub cmdCancel_onClick()
      returnvalue=""
      window.close
  End Sub
  
</SCRIPT>
<Fieldset STYLE="HEIGHT: 390px; LEFT: 16px; POSITION: absolute; TOP: 45px; WIDTH: 550px" ID="select0">
	<LEGEND>社區選擇清單</LEGEND> 
	
	<FIELDSET STYLE="HEIGHT: 308px; LEFT: 16px; POSITION: absolute; TOP: 20px; WIDTH: 300px" ID="select1">
		<LEGEND>社區名稱</LEGEND>
		<SELECT style="font-family:細明體;HEIGHT: 269px; LEFT: 10px; POSITION: absolute; TOP: 25px; WIDTH: 270px" id="lstOrder1" size="5">
				<%=s1%>
		</SELECT>
	</FIELDSET>&nbsp;
</Fieldset>&nbsp; <font style="LEFT: 30px; POSITION: absolute; TOP: 380px">目前選擇內容 </font>
		<INPUT id="cmdtext" style="LEFT: 130px; POSITION: absolute; TOP: 380px; " size="30" type="text" readonly>
		<INPUT id="cmdtext1" style="LEFT: 130px; POSITION: absolute; TOP: 380px;display:none" size="30" type="text" readonly>
		<INPUT id="cmdtext2" style="LEFT: 130px; POSITION: absolute; TOP: 380px; display:none" size="30" type="text" readonly>
		<INPUT id="cmdCancel" style="LEFT: 490px; POSITION: absolute; TOP: 380px" type="button" value="取消">
		<INPUT id="cmdSure" style="LEFT: 436px; POSITION: absolute; TOP: 380px" type="button" value="確定">
	</BODY>
</HTML>
