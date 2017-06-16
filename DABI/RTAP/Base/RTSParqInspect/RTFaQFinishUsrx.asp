<%@ Language=VBScript%>
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<% 
   'showopt="Y;Y;Y;Y"表示對話方塊中要顯示的項目(業務工程師;客服人員;技術部;廠商)
    showopt=split(request("showopt"),";")
    userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
    Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
    'userlevel=2:為業務工程師==>只能看所屬社區資料
    'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
    '客服部:B400,技術部:B500,台北業務部:B100,台中:B300,高雄:B200
    Domain=Mid(Emply,1,1)
    set conn=server.CreateObject("ADODB.Connection")
    set rs=server.CreateObject("ADODB.Recordset")
    DSN="DSN=RTLib"
    Conn.Open DSN
    
if showopt(0)="Y" then    
    '讀取縣市所屬轄區
    sql="SELECT RTCounty.CUTID, RTCounty.CUTNC, " _
       &"RTArea.AREAID, RTArea.AREANC " _
       &"FROM RTCounty INNER JOIN " _
       &"RTAreaCty ON RTCounty.CUTID = RTAreaCty.CUTID INNER JOIN " _
       &"RTArea ON RTAreaCty.AREAID = RTArea.AREAID AND " _
       &"RTArea.AREATYPE = '1' " _
       &"WHERE (RTCounty.CUTID = '" & showopt(4) & "') "
    rs.Open sql,conn
    if not rs.EOF then
       salesarea=rs("areaid")
    else
       salesarea=""
    end if
    rs.close
    '北區
    '----黃佳雯(北五姐)因轄區尚未確認故先全列所有業務名單
   ' if salesarea="A1" and emply <> "T92134" and emply <> "T89038" then
       '---業務排除人員
     sql="SELECT DISTINCT  RTAreaTownShip.CUTID, RTAreaTownShip.TOWNSHIP, " _
       &"RTSalesGroupREF.EMPLY, RTObj.CUSNC, RTCounty.CUTNC " _
       &"FROM RTSalesGroupREF INNER JOIN " _
       &"RTAreaTownShip ON " _
       &"RTSalesGroupREF.GROUPID = RTAreaTownShip.GROUPID AND " _
       &"RTSalesGroupREF.AREAID = RTAreaTownShip.AREAID INNER JOIN " _
       &"RTEmployee ON RTSalesGroupREF.EMPLY = RTEmployee.EMPLY INNER JOIN " _
       &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
       &"RTCounty ON RTAreaTownShip.CUTID = RTCounty.CUTID INNER JOIN " _
       &"RTSalesGroup ON RTAreaTownShip.AREAID = RTSalesGroup.AREAID AND " _
       &"RTAreaTownShip.GROUPID = RTSalesGroup.GROUPID " _
       &"where RTAreaTownShip.CUTID ='" & showopt(4) & "' and RTAreaTownShip.TOWNSHIP='" & showopt(5) & "'" _
       &"GROUP BY  RTAreaTownShip.CUTID, RTAreaTownShip.TOWNSHIP, " _
       &"RTSalesGroupREF.EMPLY, RTObj.CUSNC, RTCounty.CUTNC  "
   ' else 
   '    SQL="SELECT RTEmployee.EMPLY, RTObj.CUSNC, RTAreaSales.AREAID " _
   '       &"FROM RTAreaSales INNER JOIN " _
   '       &"RTEmployee ON RTAreaSales.CUSID = RTEmployee.EMPLY INNER JOIN " _
   '       &"RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
   '       &"WHERE (RTAreaSales.AREAID ='" & salesarea & "')  "
   ' end if
 '   Response.Write sql
    '台北
' IF Domain="T" then
'    sql="SELECT RTEmployee.emply, RTObj.CUSNC " _
'          &"FROM RTEmployee INNER JOIN " _
'          &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
'          &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08' " _
'          &"and (rtemployee.dept='B100' or (rtemployee.dept='B600' and authlevel=2)) and tran2<>'10' "    
    '台中
' elseif Domain="C" then 
'     sql="SELECT RTEmployee.emply, RTObj.CUSNC " _
'          &"FROM RTEmployee INNER JOIN " _
'          &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
'          &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08' " _
'          &"and rtemployee.dept='B300' and tran2<>'10'  "   
    '高雄
' elseIF Domain="K" then
'     sql="SELECT RTEmployee.emply, RTObj.CUSNC " _
'          &"FROM RTEmployee INNER JOIN " _
'          &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
'          &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08' " _
'          &"and rtemployee.dept='B200' and tran2<>'10'  "   
    '其他(不挑選資料)
' else
'     sql="SELECT RTEmployee.emply, RTObj.CUSNC " _
'          &"FROM RTEmployee INNER JOIN " _
'          &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
'          &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08' " _
'          &"and rtemployee.dept='*' "   
' end if
    rs.Open sql,conn
    s1=""
    Do While Not rs.Eof
       s1=s1 &"<option value=""" &rs("emply") &""">" & rs("emply") & "--" & rs("cusnc") & "</option>"
       rs.MoveNext
    Loop
    rs.Close
end if

if showopt(2)="Y" then
    '---技術排除人員
    '台北
 if Domain="T" then
    sql="SELECT RTEmployee.emply, RTObj.CUSNC " _
          &"FROM RTEmployee INNER JOIN " _
          &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
          &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08' " _
          &"and rtemployee.dept ='B500' "    
    '其他(不挑選技術部資料) 
 else
     sql="SELECT RTEmployee.emply, RTObj.CUSNC " _
          &"FROM RTEmployee INNER JOIN " _
          &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
          &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08' " _
          &"and rtemployee.dept ='*' "     
 end if
    rs.Open sql,conn
    s2=""
    Do While Not rs.Eof
       s2=s2 &"<option value=""" &rs("emply") &""">" & rs("emply") & "--" & rs("cusnc") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
end if

if showopt(1)="Y" then
    '---客服排除人員
    '台北
 if Domain="T" or Domain="C" then
    sql="SELECT RTEmployee.emply, RTObj.CUSNC " _
          &"FROM RTEmployee INNER JOIN " _
          &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
          &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08' " _
          &"and ( rtemployee.dept='B400' or (rtemployee.dept='B600' and rtemployee.authlevel in ('5','1','4')))  and tran2<>'10' "     
    '高雄
 elseif Domain="K" then
    sql="SELECT RTEmployee.emply, RTObj.CUSNC " _
          &"FROM RTEmployee INNER JOIN " _
          &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
          &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08' " _
          &"and rtemployee.dept='B200' and rtemployee.authlevel in ('5','1','4')  and tran2<>'10' "   
 else
     sql="SELECT RTEmployee.emply, RTObj.CUSNC " _
          &"FROM RTEmployee INNER JOIN " _
          &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
          &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08' " _
          &"and rtemployee.dept='*' " 
 end if
    rs.Open sql,conn
    s4=""
    Do While Not rs.Eof
       s4=s4 &"<option value=""" &rs("emply") &""">" & rs("emply") & "--" & rs("cusnc") &"</option>"
       rs.MoveNext
    Loop
    rs.Close    
end if

if showopt(3)="Y" then    
    '---排除廠商
    '台北
 if Domain="T" then 
    sql="SELECT DISTINCT RTSuppCty.CUSID , RTObj.SHORTNC " _
       &"FROM RTSuppCty INNER JOIN " _
       &"RTAreaCty ON RTSuppCty.CUTID = RTAreaCty.CUTID INNER JOIN " _
       &"RTArea ON RTAreaCty.AREAID = RTArea.AREAID INNER JOIN " _
       &"RTObj ON RTSuppCty.CUSID = RTObj.CUSID INNER JOIN " _
       &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID AND RTObjLink.CUSTYID = '04' " _
       &"WHERE (RTArea.AREATYPE = '2') AND (RTArea.AREAID IN ('B1', 'B2')) " _
       &"ORDER BY shortnc " 
 elseif Domain="C" then 
     sql="SELECT DISTINCT RTSuppCty.CUSID , RTObj.SHORTNC " _
       &"FROM RTSuppCty INNER JOIN " _
       &"RTAreaCty ON RTSuppCty.CUTID = RTAreaCty.CUTID INNER JOIN " _
       &"RTArea ON RTAreaCty.AREAID = RTArea.AREAID INNER JOIN " _
       &"RTObj ON RTSuppCty.CUSID = RTObj.CUSID INNER JOIN " _
       &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID AND RTObjLink.CUSTYID = '04' " _
       &"WHERE (RTArea.AREATYPE = '2') AND (RTArea.AREAID IN ('B3')) " _
       &"ORDER BY shortnc " 
 elseif Domain="K" then 
     sql="SELECT DISTINCT RTSuppCty.CUSID , RTObj.SHORTNC " _
       &"FROM RTSuppCty INNER JOIN " _
       &"RTAreaCty ON RTSuppCty.CUTID = RTAreaCty.CUTID INNER JOIN " _
       &"RTArea ON RTAreaCty.AREAID = RTArea.AREAID INNER JOIN " _
       &"RTObj ON RTSuppCty.CUSID = RTObj.CUSID INNER JOIN " _
       &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID AND RTObjLink.CUSTYID = '04' " _
       &"WHERE (RTArea.AREATYPE = '2') AND (RTArea.AREAID IN ('B4', 'B5')) " _
       &"ORDER BY shortnc " 
 else
     sql="SELECT DISTINCT RTSuppCty.CUSID , RTObj.SHORTNC " _
       &"FROM RTSuppCty INNER JOIN " _
       &"RTAreaCty ON RTSuppCty.CUTID = RTAreaCty.CUTID INNER JOIN " _
       &"RTArea ON RTAreaCty.AREAID = RTArea.AREAID INNER JOIN " _
       &"RTObj ON RTSuppCty.CUSID = RTObj.CUSID INNER JOIN " _
       &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID AND RTObjLink.CUSTYID = '04' " _
       &"WHERE (RTArea.AREATYPE = '2') AND (RTArea.AREAID IN ('*')) " _
       &"ORDER BY shortnc " 
 end if
    rs.Open sql,conn
    s3=""
    Do While Not rs.Eof
       s3=s3 &"<option value=""" &rs("cusid") &""">" & rs("cusid") & "--" & rs("shortnc") &"</option>"
       rs.MoveNext
    Loop
    rs.Close    
end if

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
  Sub lstOrder4_onclick()
      selno=lstorder4.selectedIndex
      if selno >= 0 then
         window.document.all("cmdtext").value= lstOrder4(selno).innerHTML
         window.document.all("cmdtext1").value=lstOrder4(selno).value
         window.document.all("cmdtext2").value="4"      
      end if
  End Sub      
  
  Sub cmdSure_onClick()
    ReturnValue=""
    if len(trim(window.document.all("cmdtext").value)) = 0 then
       msgbox "請選擇安維人員!",vbokonly,"錯誤訊息視窗"
    else    
       returnvalue= window.document.all("cmdtext1").value &";"& window.document.all("cmdtext").value &";"& window.document.all("cmdtext2").value & ";" & "Y"
       window.close
    end if
  End Sub

  Sub cmdCancel_onClick()
      returnvalue=""
      window.close
  End Sub

</SCRIPT>
<Fieldset
 STYLE="HEIGHT: 390px; LEFT: 16px; POSITION: absolute; TOP: 15px; WIDTH: 570px" ID=select0>
<LEGEND>安維人員選擇</LEGEND> 

<FIELDSET
 STYLE="HEIGHT: 308px; LEFT: 16px; POSITION: absolute; TOP: 20px; WIDTH: 135px" ID=select1>
<LEGEND>業務工程師</LEGEND>
<SELECT id=lstOrder1 size=5 
style="HEIGHT: 269px; LEFT: 10px; POSITION: absolute; TOP: 26px; WIDTH: 110px" >
<%=s1%>
</SELECT>
</FIELDSET>&nbsp; 

<FIELDSET
 STYLE="HEIGHT: 308px; LEFT: 155px; POSITION: absolute; TOP: 20px; WIDTH: 125px" ID=select1>
<LEGEND>客服人員</LEGEND>
<SELECT id=lstOrder4 size=5 
style="HEIGHT: 269px; LEFT: 10px; POSITION: absolute; TOP: 26px; WIDTH: 100px" >
<%=s4%>
</SELECT>
</FIELDSET>&nbsp; 

<FIELDSET
 STYLE="HEIGHT: 308px; LEFT: 285px; POSITION: absolute; TOP: 20px; WIDTH: 125px" ID=select2>
<LEGEND>技術發展部</LEGEND>
<SELECT id=lstOrder2 size=5 
style="HEIGHT: 269px; LEFT: 10px; POSITION: absolute; TOP: 26px; WIDTH: 100px">
<%=s2%>
</SELECT>
</FIELDSET>&nbsp; 

<FIELDSET
 STYLE="HEIGHT: 308px; LEFT: 415px; POSITION: absolute; TOP: 20px; WIDTH: 125px" ID=select3>
<LEGEND>廠商</LEGEND>
<SELECT id=lstOrder3 size=5 
style="HEIGHT: 269px; LEFT: 10px; POSITION: absolute; TOP: 26px; WIDTH: 100px">
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
