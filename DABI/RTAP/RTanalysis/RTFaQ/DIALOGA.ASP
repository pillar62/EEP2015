<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>SD Analysis criteria</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  If Category.value <> "" Then
     t=" 轄區 " & Category.Value
  End If
  returnvalue=t
  window.close
End Sub

</SCRIPT>
</HEAD>
<BODY style="background:lightblue">
<DIV align=Center><i><font face="標楷體" size="5" color="#FF00FF">HI-Building 客訴資料分析</font></i> </DIV>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
  <tr><td><font face="標楷體">請選擇查詢之業務轄區 :</font></td>
<td><font face="標楷體"><SELECT size="1" name="Category"  
        style="width:200;">
<% 
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))  
  'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
 ' Response.Write "DOMAIN=" & domain & "<BR>"
  Domain=Mid(Emply,1,1)
  select case Domain
         case "T"
            DAreaID="='A1'"
         case "C"
            DAreaID="='A2'"         
         case "K"
            DAreaID="='A3'"         
         case else
            DareaID="=''"
  end select
  '高階主管可讀取全部資料
  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or Ucase(emply)="T89018" or _
     Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T89008" or Ucase(emply)="T89007" then
     DAreaID="<>'*'"
  end if
  '資訊部管理員可讀取全部資料
  if userlevel=31 then DAreaID="<>'*'"
  
    DSN="DSN=rtlib" 
    sql="Select areaid,areanc from rtarea where areatype='1' and areaid " & DAreaid 
    Set conn=Server.CreateObject("ADODB.Connection") 
    conn.Open DSN 
    Set rs=Server.CreateObject("ADODB.Recordset") 
    rs.Open sql,conn 
    dftSel=" SELECTED" 
    if DAreaid = "<>'*'" then
       Response.Write "<OPTION value=""<>'*'""" & dftSel & ">全部</OPTION>"  & vbCRLF
    end if 
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
 </font></td><tr>
</table> 
<p><font face="標楷體">按 <INPUT TYPE="button" VALUE="查詢" ID="cmdSure"   
 style="font-family: 標楷體; color: #FF0000;cursor:hand">  
 系統將依您選取之業務轄區，以轄區、縣市別、業務員展開客訴案件之樞紐分析圖表。</font></p> 
<p><font face="標楷體">您可依希望展現之角度及測量值(縣市、業務員‧‧‧等)，使用滑鼠，由資料清單將欄位拉至樞紐分析表的相關位置。</font></p> 
<p>　</p> 
</BODY> 
</HTML>