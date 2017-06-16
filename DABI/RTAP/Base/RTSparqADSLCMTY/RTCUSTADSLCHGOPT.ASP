<%
    keyXX=request("key")
   ' RESPONSE.Write "KEYXX=" & KEYXX
    keyary=split(keyXX,";")
    
    Dim rs,conn,S6
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open "DSN=RTLib"
    Set rs=Server.CreateObject("ADODB.Recordset")    
    S6=""
    today=datevalue(now())
    updsql="SELECT RTCode.CODE, RTCode.CODENC " _
          &"FROM RTCode " _
          &"WHERE (RTCode.KIND = 'K1') and ( rtcode.code not in ( select  modifycode from RTSparqADSLCHG where " _
          &"RTSparqADSLCHG.cusid='" & keyary(1) & "' and RTSparqADSLCHG.entryno=" & keyary(2)  _
          &" and RTSparqADSLCHG.modifydat='" & today & "' and RTSparqADSLCHG.transdat is NOT null ) )"
    'Response.Write "UPDSQL=" & updsql
    rs.Open updsql,CONN
  '  s1="<option value=""<>'*';全部"" selected>全部</option>" &vbCrLf    
    Do While Not rs.Eof
       s6=s6 &"<option value=""" &rs("code") & """>" &rs("codenc") &"</option>"
       rs.MoveNext
    Loop
    rs.Close
'------檢查該客戶是否已報竣且已轉檔,只有已轉檔過才可由此異動
    sql="select * from RTSparqADSLCUST where comq1=" & keyary(0) & " and  cusid='" & keyary(1) & "' and entryno=" & keyary(2) & " "
    rs.Open sql,conn
    IF NOT RS.EOF THEN
       DROPDAT=RS("DROPDAT")
       OVERDUE=RS("OVERDUE")
    END IF
   ' Response.Write "KEY3=" & keyary(3)
 '   Response.end
    if rs.EOF then
       endpgm="2"
    'elseif len(trim(rs("transdat"))) = 0 or Isnull(rs("transdat")) then
    '   endpgm="3"
    else
       endpgm="1"
    end if
    
'---------------------------------------------------------
    rs.close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
%>
<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>速博ADSL分享型--已報竣客戶資料異動</TITLE>
<SCRIPT language=VBScript>
Sub cmdSure_onClick
  Randomize  
  accessmode="U"
  IF document.all("search6").value = "9X" AND LEN(TRIM(document.all("search2").value)) > 0  AND document.all("search3").value = "Y" THEN
     msgbox "此客戶已為欠拆戶，不可重複執行欠費拆機異動"        
  ELSEIF document.all("search6").value = "9Y" AND (len(trim(document.all("search2").value)) = 0  OR  document.all("search3").value <> "Y" ) THEN
     msgbox "此客戶非欠拆戶，不可執行復機異動"        
  ELSEIF document.all("search6").value = "DR" AND len(trim(document.all("search2").value)) > 0 THEN
     msgbox "此客戶已退租，不可重複執行退租異動"           
  ELSEIF document.all("search6").value = "XX" AND len(trim(document.all("search2").value)) = 0 THEN
     msgbox "此客戶非退租戶，不可執行取消退租的作業"           
  ELSE
'msgbox "keyform.htmlfld.value="& keyform.htmlfld.value  
     if keyform.htmlfld.value="1" then
        key=document.all("search1").value
        key=key & document.all("search6").value
        prog="RTCustchgD.asp?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &";"
        Scrxx=window.screen.width
        Scryy=window.screen.height - 30
        StrFeature="Top=0,left=0,scrollbars=yes,status=yes," _
               &"location=no,menubar=no,width=" & scrxx & "px" _
               &",height=" & scryy & "px"
       'Set diagWindow=Window.Open(prog,"diag",strFeature)
        Window.Open prog,"diag",strFeature 
       ' window.open pgm 
       ' window.close
      elseif keyform.htmlfld.value="3" then
		msgbox "此客戶尚未轉檔!"
      else
        msgbox "程式執行發生異常，請通知資訊人員!"
      END IF
  end if
 ' window.close
End Sub
Sub cmdcancel_onClick
  Dim winP,docP
  Set winP=window.Opener
'  Set docP=winP.document
'  winP.focus()
  window.close
End Sub

sub b1_onclick()
	if isdate(document.all("search1").value) then
		objEF2KDT.varDefaultDateTime=document.all("search1").value
	end if
	call objEF2KDT.show(1)
	if objEF2KDT.strDateTime <> "" then
	    document.all("search1").value = objEF2KDT.strDateTime
	end if
end sub

</SCRIPT>
</HEAD>
<OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" codeBase=http://www.cbbn.com.tw/stock/EF2KDT.CAB#version=9,0,0,3 
	height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270"></OBJECT>

<BODY style="background:lightblue">
<!--form name=keyform是為了配合模組架構所需-->
<form name="keyform">
<DIV align=Center><i><font face="細明體" size="5" color="#FF00FF">速博ADSL399專案</font></i> </DIV>
<DIV align=Center><i><font face="細明體" size="3" color="#FF00FF">已報竣客戶資料異動</font></i> </DIV>
<P><P>
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0>
<tr>
 </font></td>
  <tr><td ALIGN="RIGHT"><font face="細明體">請選擇異動項目 :</font></TD>
  <TD><select name="search6" size="1" class=dataListEntry>
      <%=s6%>
      </select>  </td>
  <td style="display:none"><input type="text" name="search1" value="<%=keyXX%>">
  <input type="text" name="search2" value="<%=DROPDAT%>" ID="Text1">
  <input type="text" name="search3" value="<%=OVERDUE%>" ID="Text2"></td>
</tr>
</table> 
<p><center><font face="細明體">
 <INPUT TYPE="button" VALUE="送出" ID="cmdSure"   
 style="font-family: 細明體; color: #FF0000;cursor:hand"> 
  <INPUT TYPE="button" VALUE="結束" ID="cmdcancel"   
 style="font-family: 細明體; color: #FF0000;cursor:hand">
 </center>
  <HR><P>
  <center>
  <table width="80%">
  <tr><td colspan="2" width="15%">作業說明</td></tr>
  <tr><TD WIDTH="5%" align="top">(1)</td><td  WIDTH="95%"align="left">本項作業之異動必須有客戶之"異動申請單"做為憑證! 資料更改後須交相關業管人員以做為後續資料轉檔至速博之依據!</td></tr>
  <tr><TD WIDTH="5%" align="top">(2)</td><td  WIDTH="95%"align="left">異動項目依每一客戶實際狀況而有所不同，即若該客戶同一日同一異動項目已做異動且已轉檔時，則該異動項目將不再呈現!</td></tr>
  <TABLE>
  <input type="text" name=HTMLfld style=display:none value="<%=endpgm%>">
  </form>
</BODY> 
</HTML>