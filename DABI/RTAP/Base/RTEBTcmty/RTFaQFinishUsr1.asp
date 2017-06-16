<%@ Language=VBScript%>
<% 
    set conn=server.CreateObject("ADODB.Connection")
    set rs=server.CreateObject("ADODB.Recordset")
    DSN="DSN=RTLib"
    Conn.Open DSN
    '---業務排除人員
    sql="SELECT RTEmployee.emply, RTObj.CUSNC " _
          &"FROM RTEmployee INNER JOIN " _
          &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
          &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08' " _
          &"and rtemployee.authlevel in ('2') "     
    rs.Open sql,conn
    s1=""
    Do While Not rs.Eof
       s1=s1 &"<option value=""" &rs("emply") &""">" &rs("cusnc") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
    '---技術排除人員
    sql="SELECT RTEmployee.emply, RTObj.CUSNC " _
          &"FROM RTEmployee INNER JOIN " _
          &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
          &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08' " _
          &"and rtemployee.authlevel in ('4') "     
    rs.Open sql,conn
    s2=""
    Do While Not rs.Eof
       s2=s2 &"<option value=""" &rs("emply") &""">" &rs("cusnc") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
    '---排除廠商
    sql="SELECT RTobj.cusid, RTObj.CUSNC " _
          &"FROM RTobj INNER JOIN " _
          &"RTObjlink ON RTobj.CUSID = RTObjlink.CUSID AND rtobjlink.custyid = '04' " 
    rs.Open sql,conn
    s3=""
    Do While Not rs.Eof
       s3=s3 &"<option value=""" &rs("cusid") &""">" &rs("cusnc") &"</option>"
       rs.MoveNext
    Loop
    rs.Close    
    conn.Close   
    set rs=Nothing   
    set conn=Nothing
%>
<HTML>
<HEAD>
<META name=VI60_DTCScriptingPlatform content="Server (ASP)">
<META name=VI60_defaultClientScript content=VBScript>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>安維人員選擇清單</TITLE>
</HEAD>
<BODY style="BACKGROUND: lightblue">
<SCRIPT LANGUAGE="VBScript">
  Sub lstOrder1_onclick()
      selno=lstorder1.selectedIndex
      if selno >=0 then
         window.document.all("cmdtext").value= lstOrder1(selno).innerHTML
         window.document.all("cmdtext1").value=lstOrder1(selno).value
         window.document.all("cmdtext2").value="1"         
      end if
  End Sub
  Sub lstOrder2_onclick()
      selno=lstorder2.selectedIndex
      if selno >= 0 then
         window.document.all("cmdtext").value= lstOrder2(selno).innerHTML
         window.document.all("cmdtext1").value=lstOrder2(selno).value
         window.document.all("cmdtext2").value="2"      
      end if
  End Sub
  Sub lstOrder3_onclick()
      selno=lstorder3.selectedIndex
      if selno >= 0 then
         window.document.all("cmdtext").value= lstOrder3(selno).innerHTML
         window.document.all("cmdtext1").value=lstOrder3(selno).value
         window.document.all("cmdtext2").value="3"      
      end if
  End Sub    
  
  Sub cmdSure_onClick()
    ReturnValue=""
    if len(trim(window.document.all("cmdtext").value)) = 0 then
       msgbox "請選擇安維人員!",vbokonly,"錯誤訊息視窗"
    else    
       returnvalue= window.document.all("cmdtext1").value &";"& window.document.all("cmdtext").value &";"& window.document.all("cmdtext2").value
       window.close
    end if
  End Sub

  Sub cmdCancel_onClick()
      returnvalue="N"
      window.close
  End Sub

</SCRIPT>
<Fieldset
 STYLE="HEIGHT: 390px; LEFT: 16px; POSITION: absolute; TOP: 15px; WIDTH: 600px" ID=select0>
<LEGEND>安維人員選擇</LEGEND> 
<FIELDSET
 STYLE="HEIGHT: 308px; LEFT: 16px; POSITION: absolute; TOP: 20px; WIDTH: 153px" ID=select1>
<LEGEND>業務工程師</LEGEND>
<SELECT id=lstOrder1 size=5 
style="HEIGHT: 269px; LEFT: 10px; POSITION: absolute; TOP: 26px; WIDTH: 126px" >
<%=s1%>
</SELECT>
</FIELDSET>&nbsp; 

<FIELDSET
 STYLE="HEIGHT: 308px; LEFT: 216px; POSITION: absolute; TOP: 20px; WIDTH: 153px" ID=select2>
<LEGEND>技術發展部</LEGEND>
<SELECT id=lstOrder2 size=5 
style="HEIGHT: 269px; LEFT: 10px; POSITION: absolute; TOP: 26px; WIDTH: 126px">
<%=s2%>
</SELECT>
</FIELDSET>&nbsp; 

<FIELDSET
 STYLE="HEIGHT: 308px; LEFT: 416px; POSITION: absolute; TOP: 20px; WIDTH: 150px" ID=select3>
<LEGEND>廠商</LEGEND>
<SELECT id=lstOrder3 size=5 
style="HEIGHT: 269px; LEFT: 10px; POSITION: absolute; TOP: 26px; WIDTH: 126px">
<%=s3%>
</SELECT>

</FIELDSET>&nbsp;&nbsp; 
</FIELDSET>&nbsp;&nbsp; 
<font style="LEFT: 30px; POSITION: absolute; TOP: 360px">目前選擇人員 </font>
<INPUT id=cmdtext style="LEFT: 130px; POSITION: absolute; TOP: 360px" size=30 type=text readonly>
<INPUT id=cmdtext1 style="LEFT: 130px; POSITION: absolute; TOP: 360px; display:none" size=30 type=text readonly>
<INPUT id=cmdtext2 style="LEFT: 130px; POSITION: absolute; TOP: 360px; display:none" size=30 type=text readonly>
<INPUT id=cmdCancel style="LEFT: 490px; POSITION: absolute; TOP: 360px" type=button value=取消> 
<INPUT id=cmdSure style="LEFT: 436px; POSITION: absolute; TOP: 360px" type=button value=確定>
</BODY>

</HTML>
