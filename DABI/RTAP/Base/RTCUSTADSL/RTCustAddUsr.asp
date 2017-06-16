<%@ Language=VBScript%>
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%  '---exist(0):已選擇之員工陣列;exist(1):安裝員類別
    Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
    'userlevel=2:為業務工程師==>只能看所屬社區資料
    'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
    '客服部:B400,技術部:B500,台北業務部:B100,台中:B300,高雄:B200
    Domain=Mid(Emply,1,1)
    Exist=split(request("parm"),"@")
    set conn=server.CreateObject("ADODB.Connection")
    set rs=server.CreateObject("ADODB.Recordset")
    DSN="DSN=RTLib"
    Conn.Open DSN
    Usrary=split(exist(0),";")
    usr=""
    if Ubound(Usrary) >= 0 then
       existUsr="("
       for i=0 to Ubound(usrary)
           existUsr=existUsr & "'" & usrary(i) & "',"
       next
       existUsr=mid(existUsr,1,len(existUsr)-1)
       existUsr=existUsr & ")"
       usr=" and rtemployee.emply not in " & existUsr    
    else
       usr=""
    end if
    '--安裝員類別:("")未選擇,("1")業務自行安裝,("2")技術部安裝,("3")廠商安裝
    select case exist(1)
      case "1"
         if Domain="T" or Domain="P" then
            usr=usr & " and rtemployee.authlevel in ('2') and rtemployee.tran2<>'10' and (rtemployee.dept ='B100' or rtemployee.dept ='B600') "
         elseif Domain="C" then
            usr=usr & " and rtemployee.authlevel in ('2') and rtemployee.tran2<>'10' and rtemployee.dept='B300' "     
         elseif Domain="K" then
            usr=usr & " and rtemployee.authlevel in ('2') and rtemployee.tran2<>'10' and rtemployee.dept='B200' "  
         else
            usr=usr & " and rtemployee.authlevel in ('2') and rtemployee.tran2<>'10' and rtemployee.dept='*' "           
         end if                  
      case "2"
         if Domain="T" or Domain="P" then
            usr=usr & " and rtemployee.tran2<>'10' and rtemployee.dept in ('B700', 'B701')  "
         else
            usr=usr & " and rtemployee.tran2<>'10' and rtemployee.dept='*'  "
         end if
      case else
       '255:目的是不要呈現任何員工供選擇(不可大於256==>tinyint maxnumber)
         usr=usr & " and rtemployee.dept='*' "
    end select
    sql="SELECT RTEmployee.emply, RTObj.CUSNC " _
          &"FROM RTEmployee INNER JOIN " _
          &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
          &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08' " _
          & usr            
    rs.Open sql,conn
    s1=""
    Do While Not rs.Eof
       s1=s1 &"<option value=""" &rs("emply") &""">" &rs("cusnc") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
    
    sql=""
    usr=""
    If UBound(UsrAry) >= 0 Then 
       usr=" rtemployee.emply IN " & existusr
    else
       usr=" rtemployee.emply ='*'"
    end if
    sql="SELECT RTEmployee.emply, RTObj.CUSNC " _
       &"FROM RTEmployee INNER JOIN " _
       &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
       &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08' AND " _
       & usr 
    rs.Open sql,conn
    s2=""
    Do While Not rs.Eof
       s2=s2 &"<option value=""" &rs("emply") &""">" &rs("cusnc") &"</option>"
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
<TITLE>安裝員工選擇清單</TITLE>
</HEAD>
<BODY style="BACKGROUND: lightblue">
<SCRIPT LANGUAGE="VBScript">
  ReturnValue=""
  Sub cmdSure_onClick
    sel=""
    selname=""
    For i = 0 To lstOrder2.Length - 1
      sel=sel & lstOrder2(i).value & ";"
      selname=selname & lstorder2(i).innertext & ";"
    Next
    if len(sel) > 0 then
       returnValue=mid(sel,1,len(sel)-1) & "@" & mid(selname,1,len(selname)-1)
    else
       returnvalue="" & "@" & ""
    end if
    window.close
  End Sub

  Sub lstOrder1_add(valuetext)
    str=split(valuetext,";")
    Set objent=Document.CreateElement("OPTION")
    objent.Text=str(1)
    objent.Value=str(0)
    lstOrder1.Add objent
  End Sub
  
  Sub lstOrder2_add(valuetext)
    str=split(valuetext,";")
    Set objent=Document.CreateElement("OPTION")
    objent.Text=str(1)
    objent.Value=str(0)
    lstOrder2.Add objent
  End Sub

  Sub cmdRight_onclick()
      j=lstOrder1.Length - 1
      For i=0 to j
          if lstOrder1(i).selected then
             lstOrder2_add(lstOrder1(i).value & ";" & lstorder1(i).innertext)
          end if
      Next
      For i=J to 0 step -1
          if lstOrder1(i).selected then
             lstOrder1.remove lstOrder1.SelectedIndex
          end if
      Next      
  End Sub
  
  Sub cmdLeft_onclick()
      j=lstOrder2.Length - 1
      For i=0 to j
          if lstOrder2(i).selected then
             lstOrder1_add(lstOrder2(i).value & ";" & lstorder2(i).innertext)
          end if
      Next  
      For i=J to 0 step -1
          if lstOrder2(i).selected then
             lstOrder2.remove lstOrder2.SelectedIndex
          end if
      Next            
  End Sub
 
  Sub cmdCancel_onClick()
      returnvalue=false
      window.close
  End Sub

</SCRIPT>
<FIELDSET
 STYLE="HEIGHT: 308px; LEFT: 16px; POSITION: absolute; TOP: 15px; WIDTH: 153px" ID=select1>
<LEGEND>可選擇員工</LEGEND>
<SELECT id=lstOrder1 size=5  multiple
style="HEIGHT: 269px; LEFT: 10px; POSITION: absolute; TOP: 26px; WIDTH: 126px">
<%=s1%>
</SELECT>
</FIELDSET>&nbsp; 

<FIELDSET
 STYLE="HEIGHT: 308px; LEFT: 234px; POSITION: absolute; TOP: 15px; WIDTH: 150px" ID=select2>
<LEGEND>已選擇員工</LEGEND>
<SELECT id=lstOrder2 size=5  multiple
style="HEIGHT: 269px; LEFT: 10px; POSITION: absolute; TOP: 26px; WIDTH: 126px">
<%=s2%>
</SELECT>

</FIELDSET>&nbsp;&nbsp; 
<INPUT TYPE="button" VALUE=" >"
       ID="cmdRight" STYLE    ="HEIGHT: 24px; LEFT: 181px; POSITION: absolute; TOP: 86px; WIDTH: 40px"> 
<INPUT id=cmdCancel style="LEFT: 182px; POSITION: absolute; TOP: 227px" type=button value=取消> 
<INPUT TYPE="button" VALUE=" <"
       ID="cmdLeft" STYLE    ="HEIGHT: 24px; LEFT: 181px; POSITION: absolute; TOP: 128px; WIDTH: 40px"> 
<INPUT id=cmdSure style="LEFT: 182px; POSITION: absolute; TOP: 266px" type=button value=確定>
</BODY>

</HTML>
