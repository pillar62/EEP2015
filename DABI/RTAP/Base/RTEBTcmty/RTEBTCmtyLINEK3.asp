<%
Function keyList(DSN,sql,entProgram,accessMode,numberOfKey)
  Dim i,s,j,t,u,k,v
  Dim conn,rs,aryS(5)
  s="<script language=""vbscript"">" &vbCrLf _
   &"<!--" &vbCrLf _
   &"Sub mOver(i)" &vbCrLf _
   &"    k=((i+" &colSplit &"-1)\" &colSplit &") Mod 2" &vbCrLf _
   &"    If document.all(""sel"" &i).value=""T"" Then" &vbCrLf _
   &"    Else " &vbCrLf _
   &"       document.all(""row"" &i).classname=""keyListOver"" &k" &vbCrLf _
   &"    End If" &vbCrLf _
   &"End Sub" &vbCrLf _
   &"Sub mOut(i)" &vbCrLf _
   &"    k=((i+" &colSplit &"-1)\" &colSplit &") Mod 2" &vbCrLf _
   &"    If document.all(""sel"" &i).value=""T"" Then" &vbCrLf _
   &"    Else " &vbCrLf _
   &"       document.all(""row"" &i).classname=""keyListNormal"" &k" &vbCrLf _
   &"    End If" &vbCrLf _
   &"End Sub" &vbCrLf _
   &"Sub mClick(i)" &vbCrLf _
   &"    k=((i+" &colSplit &"-1)\" &colSplit &") Mod 2" &vbCrLf _
   &"    If document.all(""sel"" &i).value=""T"" Then" &vbCrLf _
   &"       document.all(""sel"" &i).value=""F""" &vbCrLf _
   &"       document.all(""row"" &i).classname=""keyListNornam"" &k" &vbCrLf _
   &"    Else " &vbCrLf _
   &"       document.all(""sel"" &i).value=""T""" &vbCrLf _
   &"       document.all(""row"" &i).classname=""keyListClick""" &vbCrLf _
   &"    End If" &vbCrLf _
   &"    ReturnValue = self.event.srcElement.parentElement.parentElement.id " & vbCrLf _
   &"    window.close " & vbCrLf _
   &"End Sub" &vbCrLf _
   &"-->" &vbCrLf _
   &"</script>" &vbCrLf
  Set conn=Server.CreateObject("ADODB.Connection")
  conn.open DSN
  Set rs=Server.CreateObject("ADODB.Recordset")
' response.write "SQL=" & SQL
  rs.Open sql,conn,1,1
  
  rscount=rs.recordcount  
  If keyListPageSize > 0 Then
     keyListPage=Cint(Request("currentPage"))
     maxPageSize=keyListPageSize
  Else
     maxPageSize=1000
     keyListPageSize=1000
  End If
  If keyListPage < 1 Then
     keyListPage=1
  End If
  rs.Pagesize=maxPageSize
  If keyListPage > rs.PageCount Then 
     keyListPage = rs.PageCount
  End If
  If keyListPage < 1 Then
     keyListPage = 0
  Else
     rs.AbsolutePage=keyListPage
  End If
  totalPage=rs.PageCount
  s=s &"<input type=""text"" style=""display:none"" name=""currentPage""" _
      &"value=""" &keyListPage &""">" &vbCrLf _
      &"<center>" &vbCrLf _
      &"<table width=""100%"" valign=""top"" cellPadding=0 cellSpacing=0>" &vbCrLf _
      &"<tr>" &vbCrLf 
  t=""
  For i = 0 To rs.Fields.Count-1
      If aryKeyName(i) <> "none" Then 
         t=t &"<td>" &aryKeyName(i) &"</td>"
      End If
  Next
  t=t &"<td style=""display:none""></td></tr>" &vbCrLf
  k=Cstr(Int(100/colSplit))
  For i = 0 To colSplit-1
    aryS(i)="<td width=""" &k &"%"" valign=""top"">" &vbCrLf _
           &"<table width=""100%"" border=1 cellPadding=0 cellSpacing=0>" &vbCrLf _
           &"  <tr class=keyListHead id=rowT" &i &">" &vbCrLf &t
  Next
  i=0
  Do While (Not rs.Eof) And i < keyListPageSize
     v=i Mod colSplit
     i=i+1
     t=""
     u=""
     For j=1 To numberOfKey
         t=t &rs.Fields(j-1).Value &";"
     Next
     For j = 0 To rs.Fields.Count-1
      If aryKeyName(j) <> "none" Then 
         sType=Right("000" &Cstr(rs.Fields(j).Type),3)
         If Instr(cTypeChar,sType) > 0 Then
            u=u &"<td align=""left"">" &rs.fields(j).value &"&nbsp;" &"</td>"
         ElseIf Instr(cTypeN,sType) > 0 Then
            u=u &"<td align=""right"">" & formatnumber(rs.fields(j).value,2) &" &nbsp;" &"</td>"            
         ElseIf Instr(cTypeNumeric,sType) > 0 Then
            u=u &"<td align=""right"">" & formatnumber(rs.fields(j).value,0) &" &nbsp;" &"</td>"
         ElseIf Instr(cTypeDate,sType) > 0 Then
            If IsNull(rs.Fields(j).Value) Then
               u=u &"<td align=""left"">" &rs.fields(j).value &"&nbsp;" &"</td>"
            Else
               u=u &"<td align=""left"">" &datevalue(rs.fields(j).value) &"&nbsp;" &"</td>"
            End If
         Else     
	       u=u &"<td align=""center"">" &rs.fields(j).value &"&nbsp;" &"</td>"
         End If
      End If    
'         select case rs.fields(j).type
'              case 129,200,201,202,203
'                   u=u & "<Td align=""left"">" & rs.fields(j).value & "&nbsp;" & "</td>"
'              case 131,5,3,6,2,17
'                   u=u & "<Td align=""right"">" &formatnumber(rs.fields(j).value,0) &" &nbsp;" & "</td>"
'              case 135
'                   if ISNULL(rs.Fields(j).Value) then
'                      u=u & "<Td align=""left"">" & rs.fields(j).value & "&nbsp;" & "</td>"
'                   else
'                      u=u & "<Td align=""left"">" & datevalue(rs.fields(j).value) & "&nbsp;" & "</td>"
'                   end if
'              case else      
'	               u=u & "<Td align=""center"">" & rs.fields(j).value & "&nbsp;" & "</td>"
'          end select         
     Next
     k=((i+colSplit-1)\colSplit) Mod 2
     u=u &"<td style=""display:none"" width=""1""><input type=""text"" style=""display:none;"" name=""key" &i &""" value=""" &t &""">" _
         &"<input type=""text"" style=""display:none;"" name=""sel" &i &""" value=""F""></td>"
     aryS(v)=aryS(v) &"  <span id=""" & rs.fields(3).value & "-" & rs.fields(2).value & """ ><tr id=""row" &i &""" class=""keyListNormal" &k &""" " _
         &"onMouseOver=""mOver('" &i &"')"" " _
         &"onMouseOut=""mOut('" &i &"')"" " _
         &"onDblClick=""" &entProgram &" '" &accessMode &"','" &t &"'"" " _
         &"onClick=""mClick('" &i &"')"">" _
         &u &"  </tr></span>" &vbCrLf
     rs.MoveNext
  Loop
  For i = 0 To colSplit-1
    aryS(i)=aryS(i) &"</table></td>"
    s=s &aryS(i)
  Next
    s=s &"</tr></table></center>" &vbCrLf
  rs.Close
  conn.Close
  Set rs=Nothing
  Set conn=Nothing
  keyList=s
End Function
Function deleteList(DSN,dataTable,sqlDelete,numberOfKey,extTable)
  Dim conn,i,k,sel,key,nextRec,aryKey,delCount,list,rs,j
  Dim aryList(20),sType
  Set conn=Server.CreateObject("ADODB.Connection")
  On Error Resume Next  
  conn.Open DSN
  Set rs=Server.CreateObject("ADODB.Recordset")
  conn.Open sqlDelete
  rs.Open sqlDelete,conn
  For i = 1 To rs.Fields.Count
      aryKeyNameDB(i)=rs.Fields(i-1).Name
      aryKeyType(i)=rs.Fields(i-1).Type
  Next
  rs.Close
  Set rs=Nothing
  For i = 1 To numberOfKey
      aryList(i)=aryKeyNameDB(i) &" IN ("
      extDeleList(i)="("
  Next
  k=0
  delCount=0
  list=""
  Do
    k=k+1
    sel=Request.Form("sel" &k)
    key=Request.Form("key" &k)
    nextRec=True
    If sel="D" Then
       aryKey=Split(key,";")
       delCount=delCount+1
       if delcount > 1 then list = list & " OR " 
       list=list & "("           
       For i=1 To numberOfKey
           If delCount > 1 Then
              extDeleList(i)=extDeleList(i) &","
           End If
           sType=Right("000" &Cstr(aryKeyType(i)),3)
           If Instr(cTypeChar,sType) > 0 Then   
               list=list & arykeynameDb(i) & "='" & arykey(i-1) & "'"
              extDeleList(i)=extDeleList(i) &"'" &aryKey(i-1) &"'"
           Else
              list=list & arykeynameDb(i) & "=" & arykey(i-1) 
              extDeleList(i)=extDeleList(i) &aryKey(i-1) 
           End If
           if i<>numberofkey then list=list & " AND " 
       Next
       list=list & ")"
    ElseIf sel="F" Then
    Else
       nextRec=False
    End If
  Loop Until Not nextRec
  For i = 1 To numberOfKey
      extDeleList(i)=extDeleList(i) &")"
  Next
  If delCount > 0 Then
    conn.Execute "DELETE  FROM " &dataTable &" WHERE " &list &";"
     
     Dim aryExtDB,aryExtDBKs,aryExtDBKey,listExt
     If UserDefineDelete="Yes" Then
        SrRunUserDefineDelete()
     Else
        aryExtDB=Split(extTable,";")
        aryExtDBKs=Split(extTableKey,";")
        For i = 0 To Ubound(aryExtDB)
          aryExtDBKey=Split(aryExtDBKs,":")
          listExt=list
          For j = 1 To numberOfKey
            listExt=Replace(listExt,aryKeyNameDB(j),aryExtDB(i) &"." &aryExtDBKey(j-1))
          Next
          conn.Execute "DELETE  FROM " &aryExtDB(i) &" WHERE " &listExt &";"
        Next
     End If
     list=" NOT(" &list &") "
  Else
     sType=Right("000" &Cstr(aryKeyType(1)),3)
     If Instr(cTypeChar,sType) > 0 Then
        list=aryKeyNameDB(1) &"<>'*' "
     ElseIf Instr(cTypeNumeric &cTypeDate,cType) > 0 Then
        list=aryKeyNameDB(1) &"<>0 "
     Else
        list=aryKeyNameDB(1) &"<>'*' "
     End If
  End If
  conn.Close
  Set conn=Nothing
deleteList=list
End Function
%>
<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/cType.inc" -->
<%
  Dim company,system,title,buttonName,buttonEnable,DSN,formatName,sqlList,sqlListOrder,numberOfKey,sqlDelete
  Dim dataTable,dataProg,dataWindowFeature,accessMode,dataProgParm
  Dim diaWidth,diaHeight,diaTitle,diaButtonName,extTable,extTableKey,extDeleList(20),userDefineDelete
  Dim aryKeyName,aryKeyType(100),aryKeyNameDB(100)
  Dim goodMorning,goodMorningAns,goodMorningImage,colSplit
  Dim keyListPageSize,keyListPage,totalPage
  Dim functionOptName,functionOptProgram,functionOptPrompt,functionoptopen
  Dim searchProg,searchQry,searchShow,searchFirst
  Dim aryParmKey,parmKey,searchwindowfeature,optionwindowfeature
  Dim detailwindowFeature,rscount,searchqry2
  searchFirst=False
  userDefineDelete="No"
  functionOptPrompt=";;;;;;;;;;;;;;;;;;"
  keyListPageSize=0
  keyListPage=1
  colSplit=1
  searchQry=Request("searchQry")
  searchqry2=request("searchqry2")
  searchShow=Request("searchShow")
  parmKey=Request("Key")
  aryParmKey=Split(parmKey &";;;;;;;;;;;;;;;",";")
  Call SrEnvironment
  aryKeyName=Split(formatName,";")
  goodMorningAns=Request("goodMorningAns")
  If goodMorningAns="Yes" Then
     goodMorning=False
  End If
  If goodMorning Then
     Call SrWelcome
  Else
     Call SrNormal
  End If
%>
<%Sub SrNormal%>
<html>
<head>
<meta http-equiv="Pragma" content="no-cache">
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<meta http-equiv="Content-Language" content="zh-tw">
<link REL="stylesheet" HREF="/webUtilityV4EBT/DBAUDI/keyList.css" TYPE="text/css">
<link REL="stylesheet" HREF="keyList.css" TYPE="text/css">
<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/deleteDialogue.inc" -->
<script language="vbscript">
Sub runAUDI(accessMode,key)
    Dim prog,strFeature,msg
    prog="<%=dataProg%>"
    If prog="None" Then
    Else
       Randomize  
       prog="<%=dataProg%>?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
      
       strFeature="<%=detailWindowFeature%>"
       if strfeature="" then
          Scrxx=window.screen.width -7
          Scryy=window.screen.height - 74
          StrFeature="Top=0,left=0,scrollbars=yes,status=yes," _
                    &"location=no,menubar=no,width=" & scrxx & "px" _
                    &",height=" & scryy & "px"
       end if              
       Set diagWindow=Window.Open(prog,"diag",strFeature)
    End If
End Sub
Sub runDelete()
    diawidth="<%=diawidth%>"
    diaheight="<%=diaheight%>"
    if diawidth="" and diaheight="" then
       diawidth=window.screen.width
       diaHeight=window.screen.height - 30
    end if
    Call deleteDialogue(diaWidth,diaHeight,"<%=diaTitle%>","<%=diaButtonName%>",<%=colSplit%>)
End Sub
Sub runOptProg(opt)
    Dim aryOptProg,selItem,prog,diagWindow,sureRun,aryOptPrompt,aryOptName
    aryOptProg=Split("<%=functionOptProgram%>",";")
    aryOptPrompt=Split("<%=functionOptPrompt%>",";")
    aryOptName=Split("<%=functionOptName%>",";")
    '2001/8/30-S
    aryOptOpen=Split("<%=functionOptOpen%>",";")    
    '2001/8/30-E
    StrFeature="<%=optionwindowFeature%>"
    if strfeature="" then
       Scrxx=window.screen.width -7 
       Scryy=window.screen.height - 74
       StrFeature="top=0,left=0,scrollbars=yes,status=yes," _
                 &"location=no,menubar=no,width=" & scrxx & "px" _
                 &",height=" & scryy & "px" 
    end if       
    DiaWidth="<%=Diawidth%>"
    DiaHeight="<%=DiaHeight%>"    
    if DiaWidth="" THEN
       DiaWidth=window.screen.width
    end if
    if DiaHeight="" then
       DiaHeight=window.screen.height - 30
    End if
    selItem=0
    '當為"H"時,不須執行DO ...LOOP迴圈,否則當因挑選不到SEL值而中斷
    if aryoptprompt(opt) <> "H" then
       Do
         i=i+1
         sel=""
         sel=document.all("sel" &i).value
         On Error Resume Next
         If sel="T" Then
            selItem=i
         End IF
       Loop Until sel<>"T" And sel<>"F" Or selItem<>0
    end if
    sureRun=1
    '當aryoptprompt="H"時,表示不需挑選一筆資料,而直接呼叫程式
    if  aryoptprompt(opt)="H" then
        Randomize  
        prog=aryOptProg(opt)
      '當 aryoptopen(OPT)="1" :表一般window開啟,"2"用dialog開啟
        if sureRun="1" then
           If aryoptopen(OPT)="1" Then 
              Set diagWindow=Window.open(prog,"",StrFeature)
           ELSE
              Set diagWindow=Window.showmodaldialog(prog,"d2","dialogWidth:" & diawidth & "px;dialogHeight:" & diaheight &"px;")  
           end if 
        end if
    else
      If selItem <> 0 Then
         Randomize  
         prog=aryOptProg(opt) &"?V=" &Rnd() &"&key=" &document.all("key" &selItem).value
         If aryOptPrompt(opt)<>"N" Then sureRun=Msgbox("確認執行功能選項---" &aryOptName(opt),vbOKCancel)    
      '當 functionoptopen(OPT)="1" :表一般window開啟,"2"用dialog開啟
         If sureRun="1" Then 
            If aryoptopen(OPT)="1" Then 
               Set diagWindow=Window.open(prog,"",StrFeature)
            else
               Set diagWindow=Window.showmodaldialog(prog,"d2","dialogWidth:" & diawidth & "px;dialogHeight:" & diaheight &"px;")  
            end if 
         end if
      Else
         Msgbox("在您執行功能選項前，請先挑選一筆資料")
      End If
    end if
End Sub
Sub runSearchOpt()
    Dim prog,sure
<%If  searchProg="" Or searchProg="self" Then
  Else%>
    StrFeature="<%=SearchwindowFeature%>"
    if strfeature="" then
       Scrxx=window.screen.width -7 
       Scryy=window.screen.height - 74
       StrFeature="Top=0,left=0,scrollbars=yes,status=yes," _
                 &"location=no,menubar=no,width=" & scrxx & "px" _
                 &",height=" & scryy & "px"
    end if        
    prog="<%=searchProg%>"
    Set diagWindow=Window.Open(prog,"",StrFeature)
    diagWindow.focus()
<%End If%>
End Sub
Sub Srclose()  
  on error resume next
  Dim winP
  Set winP=window.Opener
  winP.focus()
  window.close  
End Sub
</script>
</head>
<%If searchFirst Then%>
<body onLoad="runSearchOpt()">
<%Else%>
<body bgcolor="C3C9D2">
<%End If%>
<table width="100%" cellPadding=0 cellSpacing=0 ID="Table1">
  <tr class=keyListTitle><td width="20%" align=left><%=Request.ServerVariables("LOGON_USER")%></td>
                         <td width="60%" align=center><%=company%></td>
                         <td width="20%" align=right><%=datevalue(Now())%></td></tr>
  <tr class=keyListTitle><td>&nbsp;</td><td align=center><%=system%></td><td>&nbsp;</td></tr>
  <tr class=keyListTitle><td>&nbsp;</td><td align=center><%=title%></td><td>&nbsp;</td></tr>
</table>
<p>
<%
  Dim listKey,sql,list,aryButton,aryButtonEnable,i,aryOptName
' -------------- deleteList(DSN,dataTable,sqlDelete,numberOfKey,extTable) ------------------------------------------------
  list=deleteList(DSN,dataTable,sqlDelete,numberOfKey,extTable)
' ---------------------------------
' sql=sqlList &list &sqlListOrder
' ----------------------------------
  sql=sqlList
' -------------- keyList(DSN,sql,entProgram,accessMode,numberOfKey) -----------------------------
  listKey=keyList(DSN,sql,"runAUDI",accessMode,numberOfKey)
  aryButton=Split(buttonName &";;;;",";")
  aryButtonEnable=Split(buttonEnable &";N;N;N;N;N;N",";")
%>
<table width="100%" cellPadding=0 cellSpacing=0 ID="Table2"> 
  <tr><td align=right>
<%If aryButtonEnable(0)="Y" Then%>
          <input type="button" class=keyListButton value="<%=aryButton(0)%>" onClick="runAUDI 'A','<%=parmKey%>'" ID="Button1" NAME="Button1">&nbsp;&nbsp;
<%End If%>
<%If aryButtonEnable(1)="Y" Then%>
          <input type="button" class=keyListButton value="<%=aryButton(1)%>" onClick="runDelete" ID="Button2" NAME="Button2">&nbsp;&nbsp;
<%End If%>
<%If aryButtonEnable(2)="Y" Then%>
          <input type="button" class=keyListButton value="<%=aryButton(2)%>" onClick="SrClose()" ID="Button3" NAME="Button3">&nbsp;&nbsp;
<%End If%>
<%If aryButtonEnable(3)="Y" Then%>
          <input type="button" class=keyListButton value="<%=aryButton(3)%>" onClick="KeyForm.Submit" ID="Button4" NAME="Button4">
<%End If%>
<div>
<%If aryButtonEnable(4)="Y" Then%>
          <span onMouseOver="" onMouseOut="">
          <input type="button" class=keyListButton 
                 value="<%=aryButton(4) &":" &keyListPage &"/" &TotalPage%>" ID="Button5" NAME="Button5">
          <span id="pageOpt" style="">
             <input type="button" class=keyListButton value="第一頁" 
                onClick="keyForm.currentPage.Value=1:keyForm.Submit" ID="Button6" NAME="Button6">
             <input type="button" class=keyListButton value="上一頁" 
                onClick="keyForm.currentPage.Value=keyForm.currentPage.Value-1:keyForm.Submit" ID="Button7" NAME="Button7">
             <input type="button" class=keyListButton value="下一頁" 
                onClick="keyForm.currentPage.Value=keyForm.currentPage.Value+1:keyForm.Submit" ID="Button8" NAME="Button8">
             <input type="button" class=keyListButton value="最末頁" 
                onClick="keyForm.currentPage.Value=<%=TotalPage%>:keyForm.Submit" ID="Button9" NAME="Button9">
          </span>
          </span>
<%End If%>
<%If aryButtonEnable(5)="Y" Then%>
          <span onMouseOver="" onMouseOut="">
          <input type="button" class=keyListButton 
                 value="<%=aryButton(5)%>" ID="Button10" NAME="Button10">
          <span id="functionOpt" style="">
<%   aryOptName=Split(functionOptName,";")
     For i = 0 To Ubound(aryOptName)%>
             <input type="button" class=keyListButton value="<%=aryOptName(i)%>"
                    onClick="runOptProg('<%=i%>')" ID="Button11" NAME="Button11">
<%   Next%>
          </span>
          </span>
<%End If%>
</div>
  </td></tr>
</table>
<p>
<form method=post name="keyForm" ID="Form1">
<%
  If searchProg <> "" Then 
  countshow="  共有(" & rscount & ")筆資料符合" %>
<table width="100%" cellPadding=0 cellSpacing=0 ID="Table3">
 <tr><td width="10%"><input type="button" value="搜尋條件" class=keyListSearch onClick="runSearchOpt" ID="Button12" NAME="Button12"></td>
     <td width="90%" class=keyListSearch2><%=searchShow%><%=countshow%>
         <input type="text" name="searchShow" value="<%=searchShow%>" style="display:none" readonly ID="Text1">
         <input type="text" name="searchQry" value="<%=searchQry%>" style="display:none" readonly ID="Text2">
         <input type="text" name="searchQry2" value="<%=searchQry2%>" style="display:none" readonly ID="Text3"></td>
 </tr>
</table>
<p>
<%End If%>
<%=listKey%>
</form>
<p>
</body>
</html>
<%End Sub%>
<%Sub SrWelcome%>
<html>
<head>
<meta http-equiv="Pragma" content="no-cache">
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<meta http-equiv="Content-Language" content="zh-tw">
<link REL="stylesheet" HREF="keyList.css" TYPE="text/css">
<script language=vbscript>
Sub newWindow
    prog="<%=Request.ServerVariables("PATH_INFO")%>?goodMorningAns=Yes"
    strFeature="<%=dataWindowFeature%>"
    if strfeature="" then
       Scrxx=window.screen.width -7
       Scryy=window.screen.height - 74
       StrFeature="Top=0,left=0,scrollbars=yes,status=yes," _
                 &"location=no,menubar=no,width=" & scrxx & "px" _
                 &",height=" & scryy & "px"
    end if    
    Set objWindow=Window.Open(prog,"NewWindow",strFeature)
    objWindow.focus()
End Sub
</script>
</head>
<center>
<body onClick="newWindow" style="cursor:hand" BGCOLOR="lightblue">
<form name="form" method="post" ID="Form2">
<table width="100%" cellPadding=0 cellSpacing=0 ID="Table4">
  <tr class=keyListTitle><td width="20%" align=left><%=Request.ServerVariables("LOGON_USER")%></td>
                         <td width="60%" align=center><%=company%></td>
                         <td width="20%" align=right><%=Now()%></td><tr>
  <tr class=keyListTitle><td>&nbsp;</td><td align=center><%=title%></td><td>&nbsp;</td><tr>
</table>
<P>系統公告
<table widtH="100%" border=1 cellPadding=0 cellSpacing=0 bgcolor="lightyellow" ID="Table5">
<!--
  <tr><td background="<%=goodMorningImage%>" height="400" width="400">&nbsp;</td></tr>
-->
  <tr bgcolor="darkseagreen"><TD ALIGN="CENTER">日期</TD><TD ALIGN="CENTER">公　告　事　項</TD></TR>
  <TR bgcolor=lightyellow><td>90/08/15<img src="/webap/image/newicon.gif"></TD><TD>分公司(辦事處)ADSL客戶資料建檔作業修改完成!</TD></tr>  
  <TR bgcolor=silver><td>90/08/13</TD><TD>HI-Building客戶退租及撤銷資料查詢作業上線!</TD></tr>    
  <TR bgcolor=lightyellow><td>90/08/09</TD><TD>ADSL客戶線上申請資料查詢作業上線!</TD></tr>      
  <tr><td><input type="text" name="goodMorningAns" value="No" style="display:none;" ID="Text4"></td></tr>
</table>
</form>
</body>
</html>
<%End Sub%>
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="東森AVS管理系統"
  title="AVS用戶移機主線挑選作業"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  functionOptName="選擇確認"
  functionOptProgram="RTEBTCMTYLINESELECT.ASP"
  functionOptPrompt="Y"
 ' If V(1)="Y" then
 '    accessMode="U"
 ' Else
     accessMode="I"
 ' End IF
  DSN="DSN=RTLib"
  formatName="none;none;社區名稱;主線;線路IP;附掛電話;聯單號碼;none;可建置;申請日;none;CHT通知日;測通日;用戶;報竣;none;進度;鎖定"
  sqlDelete="SELECT RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,RTEBTCMTYH.COMN, rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1))  as comqline, " _
           &"RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, " _
           &"RTEBTCMTYLINE.RCVDAT, " _
           &"RTEBTCMTYLINE.AGREE, " _
           &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, " _
           &"RTEBTCMTYLINE.HINETNOTIFYDAT, " _
           &"RTEBTCMTYLINE.ADSLAPPLYDAT, " _
           &"SUM(CASE WHEN rtebtcust.cusid IS NOT NULL THEN 1 ELSE 0 END) AS CUSCNT, " _
           &"SUM(CASE WHEN rtebtcust.transdat IS NOT NULL THEN 1 ELSE 0 END) AS APPLYCNT, " _
           &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END AS DEVPMAN,  " _
           &"case  WHEN RTEBTCMTYLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTEBTCMTYLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null then '未報竣' " _
           &"when RTEBTCMTYLINE.HINETNOTIFYDAT is not null then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL THEN '取得預定施工日' " _
           &"WHEN RTEBTCMTYLINE.linetel <> '' then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' " _
           &"WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTEBTCMTYLINE.AGREE ='N' THEN '勘查為不可建' WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END,case when RTEBTCMTYLINE.lockdat is null then '' else 'Y' END  " _
           &"FROM RTSalesGroup RIGHT OUTER JOIN " _
           &"RTEBTCMTYLINE ON RTSalesGroup.AREAID = RTEBTCMTYLINE.AREAID AND " _
           &"RTSalesGroup.GROUPID = RTEBTCMTYLINE.GROUPID AND " _
           &"RTSalesGroup.EDATE IS NULL LEFT OUTER JOIN " _
           &"RTCounty ON RTEBTCMTYLINE.CUTID = RTCounty.CUTID LEFT OUTER JOIN " _
           &"RTObj ON RTEBTCMTYLINE.CONSIGNEE = RTObj.CUSID LEFT OUTER JOIN " _
           &"RTEBTCUST ON RTEBTCMTYLINE.COMQ1 = RTEBTCUST.COMQ1 AND " _
           &"RTEBTCMTYLINE.LINEQ1 = RTEBTCUST.LINEQ1 inner join rtebtcmtyh on rtebtcmtyline.comq1=rtebtcmtyh.comq1 " _
           &"WHERE RTEBTCMTYLINE.COMQ1= 0 " _                
           &"GROUP BY RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,RTEBTCMTYH.COMN, rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1)) , " _
           &"RTSalesGroup.GROUPNC, RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, " _
           &"RTEBTCMTYLINE.RCVDAT, " _
           &"RTEBTCMTYLINE.AGREE, " _
           &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, " _
           &"RTEBTCMTYLINE.HINETNOTIFYDAT, " _
           &"RTEBTCMTYLINE.ADSLAPPLYDAT,case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END, " _
           &"case  WHEN RTEBTCMTYLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTEBTCMTYLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null then '未報竣' " _
           &"when RTEBTCMTYLINE.HINETNOTIFYDAT is not null then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL THEN '取得預定施工日' " _
           &"WHEN RTEBTCMTYLINE.linetel <> '' then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' " _
           &"WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTEBTCMTYLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END,case when RTEBTCMTYLINE.lockdat is null then '' else 'Y' END  "
 

  dataTable="rtebtcmtyline"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="RTebtCmtylineD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="RTEBTCMTYLINES2.ASP"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTEBTCmtyline.ComQ1<>0 "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  '讀取登入帳號之群組資料
  'Response.Write "GP=" & usergroup
  '-------------------------------------------------------------------------------------------
  'userlevel=2:為業務工程師==>只能看所屬社區資料
  'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
 ' Response.Write "DOMAIN=" & domain & "<BR>"
  Domain=Mid(Emply,1,1)
  select case Domain
         case "T"
            DAreaID="<>'*'"
         case "P"
            DAreaID="='A1'"                        
         case "C"
            DAreaID="='A2'"         
         case "K"
            DAreaID="='A3'"         
         case else
            DareaID="=''"
  end select
  '高階主管可讀取全部資料
  '  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or Ucase(emply)="T89003" or _
  '	 Ucase(emply)="T89018" or Ucase(emply)="T89020" or Ucase(emply)="T89025" or Ucase(emply)="T91099" or _
  '	 Ucase(emply)="T92134" or Ucase(emply)="T93168" or Ucase(emply)="T93177" or Ucase(emply)="T94180" then
  '   DAreaID="<>'*'"
  'end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  if userlevel=31  then DAreaID="<>'*'"
  
  '業務工程師只能讀取該工程師的社區
  'if userlevel=2 then
  '  If searchShow="全部" Then
 '台北窗口
    If DAreaID="<>'*'" Then
       sqlList="SELECT RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,rtebtcmtyh.comn " _
              &",rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1)) as comqline, " _
              &"RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, RTEBTCMTYLINE.RCVDAT, " _
              &"RTEBTCMTYLINE.AGREE, RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, " _
              &"RTEBTCMTYLINE.HINETNOTIFYDAT, RTEBTCMTYLINE.ADSLAPPLYDAT, " _
              &"SUM(CASE WHEN rtebtcust.cusid IS NOT NULL THEN 1 ELSE 0 END) AS CUSCNT, " _
              &"SUM(CASE WHEN rtebtcust.transdat IS NOT NULL THEN 1 ELSE 0 END) AS APPLYCNT, " _
              &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END AS DEVPMAN, " _
              &"case  WHEN RTEBTCMTYLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTEBTCMTYLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null " _
              &"then '未報竣' when RTEBTCMTYLINE.HINETNOTIFYDAT is not null then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL " _
              &"THEN '取得預定施工日' WHEN RTEBTCMTYLINE.linetel <> '' then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' " _
              &"WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' " _
              &"WHEN RTEBTCMTYLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END,case when RTEBTCMTYLINE.lockdat is null then '' else 'Y' END  " _
              &"FROM RTSalesGroup RIGHT OUTER JOIN RTEBTCMTYLINE ON RTSalesGroup.AREAID = RTEBTCMTYLINE.AREAID " _
              &"AND RTSalesGroup.GROUPID = RTEBTCMTYLINE.GROUPID AND RTSalesGroup.EDATE IS NULL LEFT OUTER JOIN RTCounty ON RTEBTCMTYLINE.CUTID = RTCounty.CUTID " _
              &"LEFT OUTER JOIN RTObj ON RTEBTCMTYLINE.CONSIGNEE = RTObj.CUSID LEFT OUTER JOIN RTEBTCUST ON " _
              &"RTEBTCMTYLINE.COMQ1 = RTEBTCUST.COMQ1 AND RTEBTCMTYLINE.LINEQ1 = RTEBTCUST.LINEQ1 inner join rtebtcmtyh on rtebtcmtyline.comq1=rtebtcmtyh.comq1 " _
              &"WHERE RTEBTCMTYLINE.COMQ1<> 0 AND " & SEARCHQRY  _
              &"GROUP BY RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1, RTEBTCMTYh.comn, " _
              &"rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1)), RTObj.SHORTNC, RTSalesGroup.GROUPNC, " _
              &"RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, RTEBTCMTYLINE.RCVDAT, RTEBTCMTYLINE.AGREE, " _
              &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, RTEBTCMTYLINE.HINETNOTIFYDAT, RTEBTCMTYLINE.ADSLAPPLYDAT, " _
              &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END, case  WHEN RTEBTCMTYLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTEBTCMTYLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL " _
              &"THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null then '未報竣' when RTEBTCMTYLINE.HINETNOTIFYDAT is not null " _
              &"then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL THEN '取得預定施工日' WHEN RTEBTCMTYLINE.linetel <> '' " _
              &"then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' " _
              &"WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' WHEN RTEBTCMTYLINE.AGREE = 'N' THEN '勘查為不可建' " _
              &"WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END,case when RTEBTCMTYLINE.lockdat is null then '' else 'Y' END  " 
      Else
         sqlList="SELECT RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,rtebtcmtyh.comn,rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1))  as comqline, " _
           &"RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, " _
           &"RTEBTCMTYLINE.RCVDAT, " _
           &"RTEBTCMTYLINE.AGREE, " _
           &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, " _
           &"RTEBTCMTYLINE.HINETNOTIFYDAT, " _
           &"RTEBTCMTYLINE.ADSLAPPLYDAT, " _
           &"SUM(CASE WHEN rtebtcust.cusid IS NOT NULL THEN 1 ELSE 0 END) AS CUSCNT, " _
           &"SUM(CASE WHEN rtebtcust.transdat IS NOT NULL THEN 1 ELSE 0 END) AS APPLYCNT, " _
           &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END AS DEVPMAN, " _
           &"case  WHEN RTEBTCMTYLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTEBTCMTYLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null then '未報竣' " _
           &"when RTEBTCMTYLINE.HINETNOTIFYDAT is not null then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL THEN '取得預定施工日' " _
           &"WHEN RTEBTCMTYLINE.linetel <> '' then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' " _
           &"WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTEBTCMTYLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END,case when RTEBTCMTYLINE.lockdat is null then '' else 'Y' END  " _
           &"FROM RTSalesGroup RIGHT OUTER JOIN " _
           &"RTEBTCMTYLINE ON RTSalesGroup.AREAID = RTEBTCMTYLINE.AREAID AND " _
           &"RTSalesGroup.GROUPID = RTEBTCMTYLINE.GROUPID AND " _
           &"RTSalesGroup.EDATE IS NULL LEFT OUTER JOIN " _
           &"RTCounty ON RTEBTCMTYLINE.CUTID = RTCounty.CUTID LEFT OUTER JOIN " _
           &"RTObj ON RTEBTCMTYLINE.CONSIGNEE = RTObj.CUSID LEFT OUTER JOIN " _
           &"RTEBTCUST ON RTEBTCMTYLINE.COMQ1 = RTEBTCUST.COMQ1 AND " _
           &"RTEBTCMTYLINE.LINEQ1 = RTEBTCUST.LINEQ1 inner join rtebtcmtyh on rtebtcmtyline.comq1=rtebtcmtyh.comq1 INNER JOIN RTAREATOWNSHIP ON RTEBTCMTYLINE.CUTID=RTAREATOWNSHIP.CUTID AND " _
           &"RTEBTCMTYLINE.TOWNSHIP=RTAREATOWNSHiP.TOWNSHIP  " _
           &"WHERE RTEBTCMTYLINE.COMQ1<> 0 AND " & SEARCHQRY & " AND RTAREATOWNSHIP.AREAID " & DAREAID & " " _
           &"GROUP BY RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1, RTEBTCMTYh.comn, rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1)), RTObj.SHORTNC, " _
           &"RTSalesGroup.GROUPNC, RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, " _
           &"RTEBTCMTYLINE.RCVDAT, " _
           &"RTEBTCMTYLINE.AGREE, " _
           &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, " _
           &"RTEBTCMTYLINE.HINETNOTIFYDAT, " _
           &"RTEBTCMTYLINE.ADSLAPPLYDAT,case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END, " _
           &"case  WHEN RTEBTCMTYLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTEBTCMTYLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null then '未報竣' " _
           &"when RTEBTCMTYLINE.HINETNOTIFYDAT is not null then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL THEN '取得預定施工日' " _
           &"WHEN RTEBTCMTYLINE.linetel <> '' then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' " _
           &"WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTEBTCMTYLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END,case when RTEBTCMTYLINE.lockdat is null then '' else 'Y' END  "
                      
    End If  
  'end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>