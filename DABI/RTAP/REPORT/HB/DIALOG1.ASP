<%
    Dim rs,conn,S6
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
    S6=""
    rs.Open "SELECT AREAID, AREANC FROM RTArea WHERE (AREATYPE = '1')",CONN
    s6="<option value="""" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s6=s6 &"<option value=""" &rs("AREAID") & """>" &rs("AREANC") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'----------
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>HI-BUILDING－各類報表列印</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  PGM="/report/hb/HBCMTYMONTHGROW.asp?parm=" 
  AREAID=document.all("sEARCH6").value
  if len(trim(areaid))=0 then 
     areaid="*"
  end if
  Syy=document.all("search2").value
  Smm=document.all("search3").value
  if len(trim(smm)) < 2 then 
     smm="0" & smm
  end if
  syymm=syy & smm
  cmtytype=document.all("sEARCH1").value
  pgm=pgm & SYYMM & ";" & AREAID & ";" & CMTYTYPE & ";"
'  msgbox pgm
  window.open pgm 
  window.close
End Sub
Sub cmdcancel_onClick
  window.close
End Sub


</SCRIPT>
</HEAD>
<%
YY=YEAR(NOW())
MM=MONTH(NOW())
%>
<BODY style="background:lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">HI-BUILDING－報表列印</font></i> </DIV>
<DIV align=Center><i><font face="標楷體" size="3" color="#FF00FF">社區用戶戶數統計月報表</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
</SELECT>  
 </font></td>
  <tr><td ALIGN="RIGHT"><font face="標楷體">年度月份 :</font></TD>
  <TD>
  <INPUT TYPE="TEXT" NAME="SEARCH2" SIZE="4" VALUE="<%=YY%>">年
  <INPUT TYPE="TEXT" NAME="SEARCH3" SIZE="2" VALUE="<%=MM%>">月
  </td>
</tr> 
  <tr><td ALIGN="RIGHT"><font face="標楷體">業務轄區 :</font></TD>
  <TD><select name="search6" size="1" class=dataListEntry>
      <%=s6%>
      </select>  </td>
</tr>
  <tr><td ALIGN="RIGHT"><font face="標楷體">社區類別 :</font></td>

<td><select name="search1" size="1" class=dataListEntry>
        <option value="05" SELECTED>全部社區</option>          
        <option value="01">元訊社區</option>
        <option value="02">東訊社區</option>
        <option value="03">先銳社區</option>
        <option value="04">元訊+先銳社區</option>  
    </select>
</td></tr>
</table> 
<p><center><font face="標楷體">
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="取消" ID="cmdcancel"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand">
 </center>
  <HR><P>
  <table width="100%"><tr>
  <td width="15%"> 說明：</td>
  <TD WIDTH="85%"><font size="2"></FONT></TD>
  </TR>
  <TABLE></font></p> 
</BODY> 
</HTML>