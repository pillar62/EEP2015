<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>SD Analysis criteria</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  If Category.value <> "" Then
     returnValue=Category.Value
     window.close
  '  document.write returnValue
  End If
End Sub
</SCRIPT>
</HEAD>
<BODY style="background:lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">HI-Building 券商營業員別開發數量分析</font></i> </DIV>
<DIV align=Center>
  <p align="center"><font face="標楷體">請選擇查詢之業務轄區 :</font> </DIV>   
<DIV align="center"><font face="標楷體"><SELECT size="1" name="Category"  
        style="width:200;">  
<% 
    DSN="DSN=rtlib" 
    sql="Select areaid,areanc from rtarea where areatype='1'" 
    Set conn=Server.CreateObject("ADODB.Connection") 
    conn.Open DSN 
    Set rs=Server.CreateObject("ADODB.Recordset") 
    rs.Open sql,conn 
    dftSel="" 
    Do While Not rs.Eof  
       Response.Write "<OPTION value=""='" & rs("areanc") & "'""" _ 
                      & dftSel & ">" & rs("areanc") & "</OPTION>" & vbCRLF 
       rs.MoveNext  
    Loop  
    rs.Close 
    Set rs=Nothing 
    Set conn=Nothing 
%>  
</SELECT>  
 </font></DIV>  
<p><font face="標楷體">按 <INPUT TYPE="button" VALUE="查詢" ID="cmdSure"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand">  
 系統將依您選取之業務轄區，以轄區、縣市別、營業員展開開發數量之樞紐分析圖表。</font></p> 
<p><font face="標楷體">您可依希望展現之角度及測量值(縣市、業務員‧‧‧等)，使用滑鼠，由資料清單將欄位拉至樞紐分析表的相關位置。</font></p> 
<p>　</p> 
</BODY> 
</HTML>