<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>SD Analysis criteria</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  If Category.value <> "" Then
     t=" 證券公司='" & Category.Value & "'"
  else
     t=" 證券公司<>'*'"
  end if
     returnvalue=t
     window.close     
End Sub
</SCRIPT>
</HEAD>
<BODY style="background:lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">ADSL券商專案資料分析</font></i> </DIV>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
  <tr><td><font face="標楷體">請選擇查詢之證券公司 :</font></td>
<td><font face="標楷體"><SELECT size="1" name="Category"  
        style="width:200;">
<option value=''>全部</option>        
<option value='元大'>元大</option>
<option value='日盛'>日盛</option>
<option value='台証'>台証</option>

</SELECT>  
 </font></td><tr>
</table> 
<p><font face="標楷體">按 <INPUT TYPE="button" VALUE="查詢" ID="cmdSure"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand">  
 系統將依您選取之業務轄區，以轄區、縣市別、業務員展開開發數量之樞紐分析圖表。</font></p> 
<p><font face="標楷體">您可依希望展現之角度及測量值(縣市、業務員‧‧‧等)，使用滑鼠，由資料清單將欄位拉至樞紐分析表的相關位置。</font></p> 
<p>　</p> 
</BODY> 
</HTML>