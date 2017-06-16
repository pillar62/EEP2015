<%
key=split(request("key"),";")
opt=key(2)
comq1=key(0)
lineq1=key(1)
'call SrGetCMTYLINERef (rtnvalue,opt,comq1,lineq1)
'Function SrGetCMTYLINERef(RTNValue,OPT,COMQ1,LINEQ1)
'-----------------------------------------------------------------
'    RTNValue=回傳值
'      OPT   =執行項目
'      COMQ1 =傳入參數1(社區序號)
'      LINEQ1=傳入參數2(主線序號) 
'-----------------------------------------------------------------
'      OPT=1:移機新社區及主線驗證
'            (A)社區及主線是否存在 ==> 不存在的社區及主線不可被移入線路
'            (B)社區主線是否已測通 ==> 若為測通主線則不允許再被移入線路
'-----------------------------------------------------------------------
'      rtnvalue=當資料驗證失敗時，回傳
'            (A)社區資料不存在
'            (B)主線資料不存在
'            (C)社區及主線資料皆不存在
'            (D)社區主線已測通，不可再被移入線路
'            (E)新社區主線已被其它線路移入，不可重複再被移入線路
'            (F)新社區主線已作廢，不可移入線路
'            (G)新社區主線已撤線，不可移入線路
'            (H)新社區主線已申請，不可移入線路
'            (其它)回傳新主線相關資料 
'           
'-----------------------------------------------------------------
 rtnvalue=""
 IF OPT = 1 THEN
      set conn=server.CreateObject("ADODB.Connection")
      set rs=server.CreateObject("ADODB.recordset")
      DSN="DSN=RTlib"
      Conn.Open DSN
      '檢查新社區
      SQL="SELECT * from rtebtcmtyh where comq1=" & comq1 
      Rs.Open SQL,DSN,1,1
      if rs.eof then
         RTNVALUE= "A"
      ELSE
         COMN=RS("COMN")
      end if
      Rs.Close
      '檢查新主線
      SQL="SELECT * from rtebtcmtyLINE where comq1=" & comq1 & " AND LINEQ1=" & LINEQ1
      Rs.Open SQL,DSN,1,1
      if rs.eof then
         '新社區及新主線不存在時
         IF RTNVALUE="A" THEN
            RTNVALUE= "C"
         '新社區存在但新主線不存在時
         ELSE
            RTNVALUE= "B"
         END IF
      ELSE
         '檢查新社區主線是否已測通 ==> 測通之主線不可再被移入線路
         IF NOT ISNULL(RS("ADSLAPPLYDAT")) THEN
            RTNVALUE="D"
         '檢查新社區主線是否已測通 ==> 測通之主線不可再被移入線路
         ELSEIF NOT ISNULL(RS("UPDEBTCHKDAT")) THEN
            RTNVALUE="H"            
         '檢查新社區主線是否為已撤線社區
         ELSEIF NOT ISNULL(RS("DROPDAT")) THEN
            RTNVALUE="G"
         '檢查新社區主線是否為已作廢社區
         ELSEIF NOT ISNULL(RS("CANCELDAT")) THEN   
            RTNVALUE="F"
         '檢查新社區主線是否已被其它線路移入但尚未測通
         ELSEIF RS("MOVEFROMCOMQ1") > 0 OR RS("MOVEFROMLINEQ1") > 0 THEN
            RTNVALUE="E" & ";" & RS("MOVEFROMCOMQ1") & ";" & RS("MOVEFROMLINEQ1")
         END IF
      end if
     
      '當RTNVALUE="" 表示條件符合，可進行移入作業
      IF RTNVALUE="" THEN
         RTNVALUE=COMN & ";" & rs("COBOSS") & ";" & rs("COid") & ";" & rs("COBOSSENG") & ";" & rs("COBOSSID") & ";" _
                 & rs("APPLYNAMEC") & ";" & rs("APPLYNAMEe") & ";" & RS("CUTID") & ";" & rs("TOWNSHIP") & ";" & rs("VILLAGE") & ";" _
                 & RS("COD1") & ";" & rs("NEIGHBOR") & ";" & RS("COD2")  & ";" & rs("STREET") & ";" & RS("COD3") & ";" & rs("SEC") & ";" _
                 & RS("COD4") & ";" & rs("LANE") & ";" & RS("COD5") & ";" & rs("RZONE") & ";" & rs("ALLEYWAY") & ";" & RS("COD7") & ";" _
                 & rs("NUM") & ";" & RS("COD8") & ";" & rs("floor") & ";" & RS("COD9") & ";" & rs("room") & ";" & RS("COD10") & ";" _
                 & rs("ADDROTHER") & ";" & rs("TELCOMROOM") & ";" & rs("SUPPLYRANGE") & ";" & RS("CUTID1") & ";" & rs("TOWNSHIP1") & ";" _
                 & rs("RADDR1") & ";" & rs("rzone1") & ";" & rs("ENGADDR") & ";" & RS("CUTID2") & ";" & rs("TOWNSHIP2") & ";" _
                 & rs("RADDR2") & ";" & rs("rzone2") & ";" & rs("contact1") & ";" & rs("contact2") & ";" & rs("CONTACTTEL") & ";" _
                 & rs("CONTACTMOBILE") & ";" & rs("CONTACTEMAIL") & ";" & rs("TECHCONTACT") & ";" & rs("CONTACTTIME1") & ";" _
                 & rs("TECHENGNAME") & ";" & rs("CONTACTSTRTIME") & ";" & rs("CONTACTENDTIME") & ";" & rs("CONTACTTIME2") & ";" _
                 & rs("linerate") & ";" & rs("LINETEL") 
      END IF
      Rs.Close
      Conn.Close
      Set Rs=Nothing
      Set Conn=Nothing

 
 End IF
'End Function
%>
<HTML>
<HEAD>
<META name=VI60_DTCScriptingPlatform content="Server (ASP)">
<META name=VI60_defaultClientScript content=VBScript>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
</HEAD>
<BODY style="BACKGROUND: lightblue">
<SCRIPT LANGUAGE="VBScript">
  Sub window_onload()
      returnvalue=document.all("KEYXX").value 
      window.close 
  End Sub
</SCRIPT>
<form>
<input type="text" name="KEYXX" style=display:none value="<%=RTNVALUE%>" ID="Text1">
</form>
</BODY>

</HTML>
