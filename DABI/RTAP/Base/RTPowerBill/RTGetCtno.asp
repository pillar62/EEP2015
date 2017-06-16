<%@ Language=VBScript%>
<% 
  
  key=Split(Request("key"),";")
  sql=" and b.comn like '%" &key(0)& "%' "

  Dim conn, rs, sql, comn
  set conn=server.CreateObject("ADODB.Connection")
  set rs=server.CreateObject("ADODB.Recordset")
  DSN="DSN=RTLib"
  Conn.Open DSN

'社區合約搜尋
	sql="select a.ctno, a.comtype, c.codenc as comtypenc, b.comq1, b.comn,  d.cutnc, b.township, " &_
		"a.counttype, g.codenc as counttypenc, a.paycycle, e.codenc as paycyclenc, a.paytype, f.codenc as paytypenc, " &_
		"a.ratio1, a.ratio2, a.mpay1, a.mpay2, a.cust1, a.cust2, a.sonetreq " &_
		"from	RTPowBillH a " &_
		"inner join RTCmtyAll b on a.comtype = b.comtype and a.comq1 = b.comq1 " &_
		"left outer join RTCode c on c.code = a.comtype and c.kind ='P5' " &_
		"left outer join RTCounty d on d.cutid = b.cutid " &_
		"left outer join RTCode e on e.code = a.paycycle and e.kind ='K5' " &_
		"left outer join RTCode f on f.code = a.paytype and f.kind ='F5' " &_
		"left outer join RTCode g on g.code = a.counttype and g.kind ='R4' " &_
		"where a.canceldat is null "& sql &_
		" order by a.comtype, b.comn "

'Response.Write SQL

  rs.CursorType = 3
  rs.Open sql,conn
  s1=""
  rscnt = rs.RecordCount
  if rscnt <= 200 then
		Do While Not rs.Eof
			s1=s1 &"<option value=""" & _
				rs("ctno")			&";"& rs("comtype")	&";"& rs("comtypenc")	&";"& rs("comq1")		&";"& _
				rs("comn")			&";"& rs("cutnc")	&";"& rs("township")	&";"& rs("counttype")	&";"& _
				rs("counttypenc")	&";"& rs("paycycle")&";"& rs("paycyclenc")	&";"& rs("paytype") 	&";"& _
				rs("paytypenc")		&";"& rs("ratio1")	&";"& rs("ratio2")		&";"& rs("mpay1")		&";"& _
				rs("mpay2")			&";"& rs("cust1")	&";"& rs("cust2")		&";"& rs("sonetreq")	&";"& _
				""">"& rs("comtypenc") &"["&rs("comq1")&"]" &rs("COMN") &" [" & rs("cutnc") & rs("township") &"]</option>"
			rs.MoveNext
		Loop
  else
  		s1=s1 &"<option>搜尋結果共: "&rscnt&" 筆資料，請縮小搜尋範圍!!</option>"
  end if 
  rs.Close    

  conn.Close   
  set rs=Nothing   
  set conn=Nothing
%>
<HTML>
<HEAD>
	<meta http-equiv="Content-Type" content="text/html; charset=Big5">
	<TITLE>社區合約選擇清單</TITLE>
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
       'returnvalue= window.document.all("cmdtext2").value &";"& window.document.all("cmdtext1").value &";"& window.document.all("cmdtext").value 
       returnvalue= window.document.all("cmdtext2").value &";"& window.document.all("cmdtext1").value
       window.close
    'end if
  End Sub

  Sub cmdCancel_onClick()
      returnvalue=""
      window.close
  End Sub

  Sub lstOrder1_onkeypress()
		'enter
  		if window.event.keycode =13 then 
			lstOrder1_onclick
			cmdSure_onClick()
		'ESC			
  		elseif window.event.keycode =27 then 
			cmdCancel_onClick
		end if
  End Sub
</SCRIPT>
<Fieldset STYLE="HEIGHT: 390px; LEFT: 16px; POSITION: absolute; TOP: 45px; WIDTH: 600px" ID="select0">
	<LEGEND>社區選擇清單</LEGEND> 
	
	<FIELDSET STYLE="HEIGHT: 308px; LEFT: 16px; POSITION: absolute; TOP: 20px; WIDTH: 570px" ID="select1">
		<LEGEND>社區名稱</LEGEND>
		<SELECT style="font-family:細明體;HEIGHT: 269px; LEFT: 10px; POSITION: absolute; TOP: 25px; WIDTH: 550px" id="lstOrder1" size="5">
				<%=s1%>
		</SELECT>
	</FIELDSET>&nbsp;
</Fieldset>&nbsp; <font style="LEFT: 30px; POSITION: absolute; TOP: 380px">目前選擇內容 </font>
		<INPUT id="cmdtext" style="LEFT: 130px; POSITION: absolute; TOP: 380px; " size="58" type="text" readonly>
		<INPUT id="cmdtext1" style="LEFT: 130px; POSITION: absolute; TOP: 380px;display:none" size="30" type="text" readonly>
		<INPUT id="cmdtext2" style="LEFT: 130px; POSITION: absolute; TOP: 380px; display:none" size="30" type="text" readonly>
		<INPUT id="cmdCancel" style="LEFT: 490px; POSITION: absolute; TOP: 405px" type="button" value="取消">
		<INPUT id="cmdSure" style="LEFT: 436px; POSITION: absolute; TOP: 405px" type="button" value="確定">
	</BODY>
</HTML>
