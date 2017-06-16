<%@ Language=VBScript%>
<% 
  
  key=Split(Request("key"),";")
  sql=" and c.comn like '%" &key(0)& "%' "

  Dim conn, rs, sql, comn
  set conn=server.CreateObject("ADODB.Connection")
  set rs=server.CreateObject("ADODB.Recordset")
  DSN="DSN=RTLib"
  Conn.Open DSN

'社區搜尋
	sql="select	c.comtype, c.comq1, c.lineq1, " &_
		"j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, " &_
		"convert(varchar(5), b.cutyid) as comq, b.comn, b.cmtytel as LINETEL, " &_
		"b.ipaddr as CMTYIP, f.codenc as LINERATE, b.linearrive as ARRIVEDAT, b.rcomdrop, " &_
		"k.cutnc, b.cutid, b.township, b.addr as raddr, l.zip, m.linenum, m.custnum " &_
		"from RTSparqAdslCmty b " &_
		"inner join HBAdslCmty c on c.comq1 = b.cutyid " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
		"left outer join RTCounty k on k.cutid = b.cutid " &_
		"left outer join RTCtyTown l on l.cutid = b.cutid and l.township = b.township " &_
		"inner join RTCmtyAll m on m.comq1 = b.cutyid and m.comtype='3' " &_
		"where 	c.comtype ='3' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, " &_
		"j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, " &_
		"convert(varchar(5), b.comq1)+'-'+convert(varchar(5), b.lineq1) as comq, d.comn, b.linetel as LINETEL, " &_
		"replace(str(b.lineipstr1) +'.'+ str(b.lineipstr2) +'.'+ str(b.lineipstr3) +'.'+ str(b.lineipstr4) +'~'+ str(b.lineipend),' ','')  as CMTYIP, " &_
		"f.codenc as LINERATE, b.linearrivedat as ARRIVEDAT, b.dropdat as RCOMDROP, " &_
		"k.cutnc, d.cutid, d.township, d.raddr, l.zip, m.linenum, m.custnum " &_
		"from RTSparq499CmtyLine b " &_
		"inner join RTSparq499CmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmty c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1 " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
		"left outer join RTCounty k on k.cutid = d.cutid " &_
		"left outer join RTCtyTown l on l.cutid = d.cutid and l.township = d.township " &_
		"inner join RTCmtyAll m on m.comq1 = d.comq1 and m.comtype='6' " &_
		"where 	c.comtype ='6' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, " &_
		"j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, " &_
		"convert(varchar(5), b.comq1)+'-'+convert(varchar(5), b.lineq1) as comq, d.comn, b.linetel as LINETEL, " &_
		"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, " &_
		"k.cutnc, d.cutid, d.township, d.raddr, l.zip, m.linenum, m.custnum " &_
		"from RTLessorAvsCmtyLine b " &_
		"inner join RTLessorAvsCmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmty c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1 " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
		"left outer join RTCounty k on k.cutid = d.cutid " &_
		"left outer join RTCtyTown l on l.cutid = d.cutid and l.township = d.township " &_
		"inner join RTCmtyAll m on m.comq1 = d.comq1 and m.comtype='7' " &_
		"where 	c.comtype ='7' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, " &_
		"j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, " &_
		"convert(varchar(5), b.comq1)+'-'+convert(varchar(5), b.lineq1) as comq, d.comn, b.linetel as LINETEL, " &_
		"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, " &_
		"k.cutnc, d.cutid, d.township, d.raddr, l.zip, m.linenum, m.custnum " &_
		"from RTLessorCmtyLine b " &_
		"inner join RTLessorCmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmty c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1 " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
		"left outer join RTCounty k on k.cutid = d.cutid " &_
		"left outer join RTCtyTown l on l.cutid = d.cutid and l.township = d.township " &_
		"inner join RTCmtyAll m on m.comq1 = d.comq1 and m.comtype='8' " &_
		"where 	c.comtype ='8' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, " &_
		"j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, " &_
		"convert(varchar(5), b.comq1)+'-'+convert(varchar(5), b.lineq1) as comq, d.comn, b.linetel as LINETEL, " &_
		"b.lineip  as CMTYIP, f.codenc as LINERATE, b.arrivedat as ARRIVEDAT, b.dropdat as RCOMDROP, " &_
		"k.cutnc, d.cutid, d.township, d.raddr, l.zip, m.linenum, m.custnum " &_
		"from RTPrjCmtyLine b " &_
		"inner join RTPrjCmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmty c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1 " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
		"left outer join RTCounty k on k.cutid = d.cutid " &_
		"left outer join RTCtyTown l on l.cutid = d.cutid and l.township = d.township " &_
		"inner join RTCmtyAll m on m.comq1 = d.comq1 and m.comtype='9' " &_
		"where 	c.comtype ='9' " & sql &_

		"UNION " &_

		"select	c.comtype, c.comq1, c.lineq1, " &_
		"j.codenc as comtypenc, case c.groupnc when '' then '直銷' else '經銷' end as belongnc, c.groupnc + c.leader as salesnc, " &_
		"convert(varchar(5), b.comq1)+'-'+convert(varchar(5), b.lineq1) as comq, d.comn, b.linetel as LINETEL, " &_
		"b.lineip  as CMTYIP, f.codenc as LINERATE, b.hardwaredat as ARRIVEDAT, b.dropdat as RCOMDROP, " &_
		"k.cutnc, d.cutid, d.township, d.raddr, l.zip, m.linenum, m.custnum " &_
		"from RTSonetCmtyLine b " &_
		"inner join RTSonetCmtyH d on d.comq1 = b.comq1 " &_
		"inner join HBAdslCmty c on c.comq1 = b.comq1 and c.lineq1 = b.lineq1 " &_
		"left outer join RTCode f on f.code = b.linerate and f.kind ='D3' " &_
		"left outer join RTCode j on j.code = c.comtype and j.kind ='P5' " &_
		"left outer join RTCounty k on k.cutid = d.cutid " &_
		"left outer join RTCtyTown l on l.cutid = d.cutid and l.township = d.township " &_
		"inner join RTCmtyAll m on m.comq1 = d.comq1 and m.comtype='A' " &_
		"where 	c.comtype ='A' " & sql &_

		"order by c.comtype, c.comq "

'Response.Write SQL

  rs.CursorType = 3
  rs.Open sql,conn
  s1=""
  rscnt = rs.RecordCount
  if rscnt <= 200 then
		Do While Not rs.Eof
			s1=s1 &"<option value=""" & _
				rs("comtype")	&";"& rs("comq1")		&";"& rs("lineq1")			&";"& _
				rs("cutid")		&";"& rs("township")	&";"& rs("raddr")			&";"& rs("cutnc")	&";"& _
				rs("zip")		&";"& rs("comtypenc")	&";"& rs("belongnc")		&";"& rs("salesnc") &";"& _
				rs("comq")		&";"& rs("comn")		&";"& rs("linetel")			&";"& rs("cmtyip")	&";"& _
				rs("linerate")	&";"& rs("arrivedat")	&";"& rs("rcomdrop")		&";"& _
				rs("linenum")	&";"& rs("custnum") 	&";"& _
				""">"& rs("comtypenc") &"["&rs("COMQ")&"]" &rs("COMN") &" [" & rs("cutnc") & rs("township") &"]["& rs("linenum") &"線"& rs("custnum") &"戶]</option>"
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
	<TITLE>社區&客戶選擇清單</TITLE>
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
