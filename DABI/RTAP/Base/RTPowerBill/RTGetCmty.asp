<%@ Language=VBScript%>
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<% 
  Dim conn, rs, casetype, sql, comn, billno
  
  key=Split(Request("key"),";")

  set conn=server.CreateObject("ADODB.Connection")
  set rs=server.CreateObject("ADODB.Recordset")
  DSN="DSN=RTLib"
  Conn.Open DSN
  sql ="select casetype from RTPowerBillH where BillNo ='" &key(0)&"' "
'Response.Write SQL  
  rs.Open sql,conn  
  if not rs.EOF then casetype = rs("casetype")
  rs.Close

'response.Write "casetype="&casetype  

  if casetype ="01" then		'CHT 599'
		sql="select a.COMQ1, 0 as LINEQ1, min(b.DOCKETDAT) as FIRSTDOCKET, a.COMN "_
		   &"from RTCmty a inner join RTCust b on a.COMQ1 = b.COMQ1 "_
		   &"where a.RCOMDROP is Null and b.DROPDAT is Null and b.DOCKETDAT is not null "_
		   &"and a.comn like '%" &key(1)& "%' "_
		   &"group by a.COMQ1, COMN ORDER BY COMN "
  elseif casetype ="02" then	'CHT 399'
		sql="select b.COMQ1, 0 as LINEQ1, min(b.DOCKETDAT) as FIRSTDOCKET, a.COMN "_
		   &"from RTCustAdslCmty a inner join RTCustAdsl b on a.CUTYID = b.COMQ1 "_
		   &"where a.RCOMDROP is Null and b.DROPDAT is Null and b.DOCKETDAT is not null "_
		   &"and a.comn like '%" &key(1)& "%' "_
		   &"group by b.COMQ1, COMN ORDER BY COMN "
  elseif casetype ="03" then	'Sparq 399'
		sql="select b.COMQ1, 0 as LINEQ1, min(b.DOCKETDAT) as FIRSTDOCKET, a.COMN "_
		   &"from RTSparqAdslCmty a inner join RTSparqAdslCust b on a.CUTYID = b.COMQ1 "_
		   &"where a.RCOMDROP is Null and b.DROPDAT is Null and b.DOCKETDAT is not null "_
		   &"and a.comn like '%" &key(1)& "%' "_
		   &"group by b.COMQ1, COMN ORDER BY COMN "
  elseif casetype ="04" then	'EBT'
		sql="select b.COMQ1, b.LINEQ1, min(c.DOCKETDAT) as FIRSTDOCKET, a.COMN +'-'+ convert(varchar(4),b.LINEQ1) as COMN "_
		   &"from RTEbtCmtyH a inner join RTEbtCmtyLine b on a.COMQ1 = b.COMQ1 "_
		   &"left outer join RTEbtCust c on b.COMQ1 = c.COMQ1 and b.LINEQ1 = c.LINEQ1 "_
		   &"where b.DROPDAT is Null and b.CANCELDAT is Null "_
		   &"and a.comn like '%" &key(1)& "%' "_
		   &"group by b.COMQ1, b.LINEQ1, a.COMN +'-'+ convert(varchar(4),b.LINEQ1) ORDER BY a.COMN +'-'+ convert(varchar(4),b.LINEQ1) "
  elseif casetype ="05" then	'Sparq 499'
		sql="select b.COMQ1, b.LINEQ1, min(c.DOCKETDAT) as FIRSTDOCKET, a.COMN +'-'+ convert(varchar(4),b.LINEQ1) as COMN "_
		   &"from RTSparq499CmtyH a inner join RTSparq499CmtyLine b on a.COMQ1 = b.COMQ1 "_
		   &"left outer join RTSparq499Cust c on b.COMQ1 = c.COMQ1 and b.LINEQ1 = c.LINEQ1 "_
		   &"where b.DROPDAT is Null and b.CANCELDAT is Null "_
		   &"and a.comn like '%" &key(1)& "%' "_
		   &"group by b.COMQ1, b.LINEQ1, a.COMN +'-'+ convert(varchar(4),b.LINEQ1) ORDER BY a.COMN +'-'+ convert(varchar(4),b.LINEQ1) "
  else
		sql=""
  end if
'Response.Write SQL
  rs.Open sql,conn
  s1=""
  Do While Not rs.Eof
     s1=s1 &"<option value=""" &rs("COMQ1") & ";" & rs("LINEQ1") & ";" & rs("FIRSTDOCKET") &""">" &rs("COMN") &"</option>"
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
	
	<FIELDSET STYLE="HEIGHT: 308px; LEFT: 16px; POSITION: absolute; TOP: 20px; WIDTH: 250px" ID="select1">
				<LEGEND>社區名稱</LEGEND>
				<SELECT style="HEIGHT: 269px; LEFT: 10px; POSITION: absolute; TOP: 25px; WIDTH: 200px" id="lstOrder1" size="5">
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
