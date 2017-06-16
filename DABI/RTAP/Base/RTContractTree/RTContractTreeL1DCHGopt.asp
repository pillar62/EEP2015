<%
    key=request("key")
    keyary=split(key,";")
%>
<HTML>
<HEAD>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<TITLE>合約</TITLE>
<SCRIPT language=VBScript>
Sub window_onload()
  Randomize  
  accessmode="U"
  'key:key(0)=社區編號 key(1)=客戶編號 key(2)=項次 key(3)=異動項目
     key=document.all("search1").value
     key=key & document.all("search1").value
     prog="RTContractTreeL1dchg.asp?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&"
     Scrxx=window.screen.width
     Scryy=window.screen.height - 30
     StrFeature="Top=0,left=0,scrollbars=yes,status=yes," _
               &"location=no,menubar=no,width=800,height=220 "
    'Set diagWindow=Window.Open(prog,"diag",strFeature)
     Window.Open prog,"diag",strFeature 
    ' window.open pgm 
     window.close
End Sub
</SCRIPT>
</HEAD>
<BODY style="background:lightblue">
<!--form name=keyform是為了配合模組架構所需-->
<form name="keyform" ID="Form1">
<table align="center" width="90%" border=0 cellPadding=0 cellSpacing=0 ID="Table1">
<tr>
  <td style="display:none"><input type="text" name="search1" value="<%=key%>" ID="Text1"></td>
</tr>
</table> 
<p>
  <input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text2">
  </form>
</BODY> 
</HTML>