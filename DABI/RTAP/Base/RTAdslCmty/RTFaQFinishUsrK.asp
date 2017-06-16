<%
parmKey=Request("Key")
aryParmKey=Split(parmKey &";;;;;;;;;;;;;;;",";")
%>
<HTML>
<HEAD>
<META name=VI60_DTCScriptingPlatform content="Server (ASP)">
<META name=VI60_defaultClientScript content=VBScript>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>安維員工選擇清單</TITLE>
</HEAD>
<BODY style="BACKGROUND: lightblue">
<SCRIPT LANGUAGE="VBScript">
  Sub window_onload()
      k1=document.all("key1").value
      k2=document.all("key2").value
      prog="RTFaQFinishUsr.asp"
      'showopt="Y;Y;Y;Y"表示對話方塊中要顯示的項目(業務工程師;客服人員;技術部;廠商)
      showopt="Y;Y;Y;Y"
      prog=prog & "?showopt=" & showopt      
      FUsr=Window.showModalDialog(prog,"Dialog","dialogWidth:590px;dialogHeight:480px;")  
      'Fusrid(0)=維修人員工號或廠商代號  fusrid(1)=只為於上一畫面中秀出中文名稱(無其它作用) fusrid(2)="1"為業務"2"為技術"3"為廠商"4"為客服(作為資料存放於何欄位之依據)
      if Fusr <> "N" then
         FUsrID=Split(Fusr,";")      
         prog="RTFaQFinishK.asp?FUSR=" & Fusrid(0) & ";" & Fusrid(2) & "&key=" & k1 &";" & k2
         Window.Open prog
      End if
      Set winP=window.Opener
      Set docP=winP.document
      docP.all("keyform").Submit
      winP.focus()             
      window.close
  End Sub
</SCRIPT>
</BODY>
<input type=text name=key1 value="<%=aryparmkey(0)%>" style="display:none">
<input type=text name=key2 value="<%=aryparmkey(1)%>" style="display:none">
</HTML>
