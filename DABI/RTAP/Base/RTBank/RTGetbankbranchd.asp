<%@ Language=VBScript%>
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<% 'stock="縣市" 
  bankid=request("key")
  set conn=server.CreateObject("ADODB.Connection")
  set rs=server.CreateObject("ADODB.Recordset")
  DSN="DSN=RTLib"
  Conn.Open DSN
  sql="SELECT headno,branchno, branchnc " _
     &"FROM RTbankno " _
     &"WHERE headno = '" & bankid & "' " _
     &"ORDER BY branchnc " 
  rs.Open sql,conn
 'Response.Write SQL
  s1=""
  Do While Not rs.Eof
     s1=s1 &"<option value=""" &rs("branchno") &""">" &rs("branchnc") &"</option>"
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
<TITLE>分行選擇清單</TITLE>
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
    if len(trim(window.document.all("cmdtext").value)) = 0 then
       msgbox "請選擇銀行分行!",vbokonly,"錯誤訊息視窗"
    else    
       returnvalue= window.document.all("cmdtext1").value &";"& window.document.all("cmdtext").value &";"& window.document.all("cmdtext2").value
       window.close
    end if
  End Sub

  Sub cmdCancel_onClick()
      returnvalue=""
      window.close
  End Sub

</SCRIPT>
<Fieldset
 STYLE="HEIGHT: 390px; LEFT: 16px; POSITION: absolute; TOP: 15px; WIDTH: 550px" ID=select0>
<LEGEND>分行選擇清單</LEGEND> 

<FIELDSET
 STYLE="HEIGHT: 308px; LEFT: 16px; POSITION: absolute; TOP: 20px; WIDTH: 125px" ID=select1>
<LEGEND>分行名稱</LEGEND>
<SELECT id=lstOrder1 size=5 
style="HEIGHT: 269px; LEFT: 10px; POSITION: absolute; TOP: 26px; WIDTH: 100px" >
<%=s1%>
</SELECT>
</FIELDSET>&nbsp; 
<font style="LEFT: 30px; POSITION: absolute; TOP: 360px">目前選擇內容 </font>
<INPUT id=cmdtext style="LEFT: 130px; POSITION: absolute; TOP: 360px" size=30 type=text readonly>
<INPUT id=cmdtext1 style="LEFT: 130px; POSITION: absolute; TOP: 360px; display:none" size=30 type=text readonly>
<INPUT id=cmdtext2 style="LEFT: 130px; POSITION: absolute; TOP: 360px; display:none" size=30 type=text readonly>
<INPUT id=cmdCancel style="LEFT: 490px; POSITION: absolute; TOP: 360px" type=button value=取消> 
<INPUT id=cmdSure style="LEFT: 436px; POSITION: absolute; TOP: 360px" type=button value=確定>
</BODY>

</HTML>
