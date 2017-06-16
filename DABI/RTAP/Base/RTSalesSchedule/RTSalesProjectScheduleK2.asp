<%
    csalesarea=request("search1")
    cemply=request("search2")
    cname=request("name")
    cyy=request("search3")
    cmm=request("search4")
    if cyy="" then cyy=DATEPART("yyyy",now())
    if cmm="" then cmm=RIGHT("0" & DATEPART("m",now()),2)
    ckind=request("search5")
    session("cyy")=cyy
    session("cmm")=cmm
%>
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
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
            IF J=EMAILFIELDNO AND EMAILFIELDFLAG="Y" THEN 
               u=u &"<td align=""left""><a href=""mailto:" & rs.fields(j).value & """>" & rs.fields(j).value &"&nbsp;" &"</td>"
            ELSE
               u=u &"<td align=""left"">" &rs.fields(j).value &"&nbsp;" &"</td>"
            END IF
         ElseIf Instr(cTypeN,sType) > 0 Then
            u=u &"<td align=""right"">" & formatnumber(rs.fields(j).value,2) &" &nbsp;" &"</td>"            
         ElseIf Instr(cTypeNumeric,sType) > 0 Then
          '  response.write "j=" & j & ",value=" & rs.fields(j).value & "<br>"
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
     aryS(v)=aryS(v) &"  <tr id=""row" &i &""" class=""keyListNormal" &k &""" " _
         &"onMouseOver=""mOver('" &i &"')"" " _
         &"onMouseOut=""mOut('" &i &"')"" " _
         &"onDblClick=""" &entProgram &" '" &accessMode &"','" &t &"'"" " _
         &"onClick=""mClick('" &i &"')"">" _
         &u &"  </tr>" &vbCrLf
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
  Dim detailwindowFeature,rscount,searchqry2,EMAILFIELDNO,EMAILFIELDFLAG
  searchFirst=False
  EMAILFIELDFLAG="N"
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
    Set diagWindow=Window.Open(prog,"search",StrFeature)
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
<body bgcolor="C3C9D2" background="/WEBAP/IMAGE/bg.gif">
<%End If%>

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
<table width="100%" cellPadding=0 cellSpacing=0> 
  <tr><td align=right>
<%If aryButtonEnable(0)="Y" Then%>
          <input type="button" class=keyListButton value="<%=aryButton(0)%>" onClick="runAUDI 'A','<%=parmKey%>'">&nbsp;&nbsp;
<%End If%>
<%If aryButtonEnable(1)="Y" Then%>
          <input type="button" class=keyListButton value="<%=aryButton(1)%>" onClick="runDelete">&nbsp;&nbsp;
<%End If%>
<%If aryButtonEnable(2)="Y" Then%>
          <input type="button" class=keyListButton value="<%=aryButton(2)%>" onClick="SrClose()">&nbsp;&nbsp;
<%End If%>
<%If aryButtonEnable(3)="Y" Then%>
          <input type="button" class=keyListButton value="<%=aryButton(3)%>" onClick="KeyForm.Submit">
<%End If%>
<div>
<%If aryButtonEnable(4)="Y" Then%>
          <span onMouseOver="" onMouseOut="">
          <input type="button" class=keyListButton 
                 value="<%=aryButton(4) &":" &keyListPage &"/" &TotalPage%>">
          <span id="pageOpt" style="">
             <input type="button" class=keyListButton value="第一頁" 
                onClick="keyForm.currentPage.Value=1:keyForm.Submit">
             <input type="button" class=keyListButton value="上一頁" 
                onClick="keyForm.currentPage.Value=keyForm.currentPage.Value-1:keyForm.Submit">
             <input type="button" class=keyListButton value="下一頁" 
                onClick="keyForm.currentPage.Value=keyForm.currentPage.Value+1:keyForm.Submit">
             <input type="button" class=keyListButton value="最末頁" 
                onClick="keyForm.currentPage.Value=<%=TotalPage%>:keyForm.Submit">
          </span>
          </span>
<%End If%>
<%If aryButtonEnable(5)="Y" Then%>
          <span onMouseOver="" onMouseOut="">
          <input type="button" class=keyListButton 
                 value="<%=aryButton(5)%>">
          <span id="functionOpt" style="">
<%   aryOptName=Split(functionOptName,";")
     For i = 0 To Ubound(aryOptName)%>
             <input type="button" class=keyListButton value="<%=aryOptName(i)%>"
                    onClick="runOptProg('<%=i%>')">
<%   Next%>
          </span>
          </span>
<%End If%>
</div>
  </td></tr>
</table>
<p>
<form method=post name="keyForm">
<%
  If searchProg <> "" Then 
  countshow="  共有(" & rscount & ")筆資料符合" %>
<table width="100%" cellPadding=0 cellSpacing=0>
 <tr><td width="10%"><input type="button" value="搜尋條件" class=keyListSearch onClick="runSearchOpt"></td>
     <td width="90%" class=keyListSearch2><%=searchShow%><%=countshow%>
         <input type="text" name="searchShow" value="<%=searchShow%>" style="display:none" readonly>
         <input type="text" name="searchQry" value="<%=searchQry%>" style="display:none" readonly>
         <input type="text" name="searchQry2" value="<%=searchQry2%>" style="display:none" readonly>
         <input type="text" name="EMAILFIELDNO" value="<%=EMAILFIELDNO%>" style="display:none" readonly></td>
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
<form name="form" method="post">

<P>系統公告
<table widtH="100%" border=1 cellPadding=0 cellSpacing=0 bgcolor="lightyellow">
<!--
  <tr><td background="<%=goodMorningImage%>" height="400" width="400">&nbsp;</td></tr>
-->
  <tr bgcolor="darkseagreen"><TD ALIGN="CENTER">項次</TD><TD ALIGN="CENTER">日期</TD><TD ALIGN="CENTER">系　統　訊　息</TD></TR>
<%
 Set conn=Server.CreateObject("ADODB.Connection")
 Set rs=Server.CreateObject("ADODB.Recordset")
 DSN="DSN=RTLib"
 conn.open DSN
 SQL="SELECT MSGID, TOPIC, CONTENT, MSGDAT, APPEAR, UPDAT, DOWNDAT, IMG FROM RTSYSMSG where appear='Y' and ( UPDAT <= GETDATE() OR UPDAT IS NULL ) AND ( DOWNDAT IS NULL OR DOWNDAT > GETDATE() ) order by msgdat desc"
 RS.Open SQL,CONN
 cnt=0
 do while not rs.eof
    cnt=cnt+1
    K=cnt mod 2
    if k=1 then
       response.Write "<TR bgcolor=lightyellow>"
    else
       response.Write "<TR bgcolor=silver>"
    end if
    response.Write "<TD align=center>" & RS("MSGID") & "</TD>" & "<TD>" & RS("MSGDAT") & "</TD>" & "<TD>" & RS("TOPIC") & "</TD></TR>"
    RS.movenext
  LOOP
rs.Close
conn.Close
set rs=nothing
set conn=nothing
%>  
</table>
</form>
</body>
</html>
<%End Sub%>
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system=""
  title=""
  buttonName=";;;;頁次"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;N;Y;Y"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName=""
  functionOptProgram=""
  functionOPTprompt=""
  If V(1)="Y" then
     accessMode="U"
  Else
    accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;none;none;序號;社區名稱;方案類型;業務;none;none;<CENTER>01</CENTER>;<CENTER>02</CENTER>;<CENTER>03</CENTER>;<CENTER>04</CENTER>;<CENTER>05</CENTER>;<CENTER>06</CENTER>;<CENTER>07</CENTER>;<CENTER>08</CENTER>;<CENTER>09</CENTER>;<CENTER>10</CENTER>;<CENTER>11</CENTER>;<CENTER>12</CENTER>;<CENTER>13</CENTER>;<CENTER>14</CENTER>;<CENTER>15</CENTER>;<CENTER>15</CENTER>;<CENTER>17</CENTER>;<CENTER>18</CENTER>;<CENTER>19</CENTER>;<CENTER>20</CENTER>;<CENTER>21</CENTER>;<CENTER>22</CENTER>;<CENTER>23</CENTER>;<CENTER>24</CENTER>;<CENTER>25</CENTER>;<CENTER>26</CENTER>;<CENTER>27</CENTER>;<CENTER>28</CENTER>;<CENTER>29</CENTER>;<CENTER>30</CENTER>;<CENTER>31</CENTER>"
  sqlDelete="SELECT HBADSLCMTY.comq1,HBADSLCMTY.LINEQ1,HBADSLCMTY.comtype,RTSalesSchedule.syy,RTSalesSchedule.smm,HBADSLCMTY.comq1 AS Expr1,HBADSLCMTY.comn,CASE WHEN HBADSLCMTY.comtype = '1' " _
         &"THEN '元訊599' WHEN HBADSLCMTY.comtype = '2' THEN '中華399' WHEN HBADSLCMTY.comtype = '3' THEN '速博399' WHEN " _
         &"HBADSLCMTY.comtype = '4' THEN '東訊599' WHEN HBADSLCMTY.comtype = '5' THEN '東森499' ELSE '???' END AS comtype,HBADSLCMTY.leader,HBADSLCMTY.T1APPLYDAT, HBADSLCMTY.RCOMDROP, " _
         &"HBSalesManageProjectD_31.STATDESC AS STATDESC1, HBSalesManageProjectD_1.STATDESC AS STATDESC2,HBSalesManageProjectD_2.STATDESC AS STATDESC3, " _
         &"HBSalesManageProjectD_3.STATDESC AS STATDESC4,  HBSalesManageProjectD_4.STATDESC AS STATDESC5, HBSalesManageProjectD_5.STATDESC AS STATDESC6, " _
         &"HBSalesManageProjectD_6.STATDESC AS STATDESC7,  HBSalesManageProjectD_7.STATDESC AS STATDESC8, HBSalesManageProjectD_8.STATDESC AS STATDESC9, " _
         &"HBSalesManageProjectD_9.STATDESC AS STATDESC10, HBSalesManageProjectD_10.STATDESC AS STATDESC11, HBSalesManageProjectD_11.STATDESC AS STATDESC12, " _
         &"HBSalesManageProjectD_12.STATDESC AS STATDESC13, HBSalesManageProjectD_13.STATDESC AS STATDESC14, HBSalesManageProjectD_14.STATDESC AS STATDESC15, " _
         &"HBSalesManageProjectD_15.STATDESC AS STATDESC16, HBSalesManageProjectD_16.STATDESC AS STATDESC17, HBSalesManageProjectD_17.STATDESC AS STATDESC18, " _
         &"HBSalesManageProjectD_18.STATDESC AS STATDESC19, HBSalesManageProjectD_19.STATDESC AS STATDESC20, HBSalesManageProjectD_20.STATDESC AS STATDESC21, " _
         &"HBSalesManageProjectD_21.STATDESC AS STATDESC22, HBSalesManageProjectD_22.STATDESC AS STATDESC23, HBSalesManageProjectD_23.STATDESC AS STATDESC24, " _
         &"HBSalesManageProjectD_24.STATDESC AS STATDESC25, HBSalesManageProjectD_25.STATDESC AS STATDESC26, HBSalesManageProjectD_26.STATDESC AS STATDESC27, " _
         &"HBSalesManageProjectD_27.STATDESC AS STATDESC28, HBSalesManageProjectD_28.STATDESC AS STATDESC29, HBSalesManageProjectD_29.STATDESC AS STATDESC30, " _
         &"HBSalesManageProjectD_30.STATDESC AS STATDESC31 " _
         &"FROM   RTSalesSchedule LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_30 ON " _
         &"RTSalesSchedule.SDD31 = HBSalesManageProjectD_30.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_29 ON " _
         &"RTSalesSchedule.SDD30 = HBSalesManageProjectD_29.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_28 ON " _
         &"RTSalesSchedule.SDD29 = HBSalesManageProjectD_28.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_27 ON " _
         &"RTSalesSchedule.SDD28 = HBSalesManageProjectD_27.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_26 ON " _
         &"RTSalesSchedule.SDD27 = HBSalesManageProjectD_26.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_25 ON " _
         &"RTSalesSchedule.SDD26 = HBSalesManageProjectD_25.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_24 ON " _
         &"RTSalesSchedule.SDD25 = HBSalesManageProjectD_24.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_23 ON " _
         &"RTSalesSchedule.SDD24 = HBSalesManageProjectD_23.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_22 ON " _
         &"RTSalesSchedule.SDD23 = HBSalesManageProjectD_22.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_21 ON " _
         &"RTSalesSchedule.SDD22 = HBSalesManageProjectD_21.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_20 ON " _
         &"RTSalesSchedule.SDD21 = HBSalesManageProjectD_20.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_19 ON " _
         &"RTSalesSchedule.SDD20 = HBSalesManageProjectD_19.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_18 ON " _
         &"RTSalesSchedule.SDD19 = HBSalesManageProjectD_18.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_17 ON " _
         &"RTSalesSchedule.SDD18 = HBSalesManageProjectD_17.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_16 ON " _
         &"RTSalesSchedule.SDD17 = HBSalesManageProjectD_16.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_15 ON " _
         &"RTSalesSchedule.SDD16 = HBSalesManageProjectD_15.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_14 ON " _
         &"RTSalesSchedule.SDD15 = HBSalesManageProjectD_14.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_13 ON " _
         &"RTSalesSchedule.SDD14 = HBSalesManageProjectD_13.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_12 ON " _
         &"RTSalesSchedule.SDD13 = HBSalesManageProjectD_12.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_11 ON " _
         &"RTSalesSchedule.SDD12 = HBSalesManageProjectD_11.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_10 ON " _
         &"RTSalesSchedule.SDD11 = HBSalesManageProjectD_10.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_9 ON " _
         &"RTSalesSchedule.SDD10 = HBSalesManageProjectD_9.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_8 ON " _
         &"RTSalesSchedule.SDD9 = HBSalesManageProjectD_8.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_7 ON " _
         &"RTSalesSchedule.SDD8 = HBSalesManageProjectD_7.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_6 ON " _
         &"RTSalesSchedule.SDD7 = HBSalesManageProjectD_6.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_5 ON " _
         &"RTSalesSchedule.SDD6 = HBSalesManageProjectD_5.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_4 ON " _
         &"RTSalesSchedule.SDD5 = HBSalesManageProjectD_4.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_3 ON " _
         &"RTSalesSchedule.SDD4 = HBSalesManageProjectD_3.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_2 ON " _
         &"RTSalesSchedule.SDD3 = HBSalesManageProjectD_2.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_1 ON " _
         &"RTSalesSchedule.SDD2 = HBSalesManageProjectD_1.STAT LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_31 ON " _
         &"RTSalesSchedule.SDD1 = HBSalesManageProjectD_31.STAT RIGHT OUTER JOIN " _
         &"HBADSLCMTY ON RTSalesSchedule.COMQ1 = HBADSLCMTY.comq1 AND " _
         &"RTSalesSchedule.LINEQ1 = HBADSLCMTY.LINEQ1 AND " _
         &"RTSalesSchedule.CONNECTTYPE = HBADSLCMTY.comtype " _
         &"WHERE hbadslcmty.comn <>'' " &  searchqry & " " _
         &"order by comn "
  dataTable="RTSalesSchedule"
  userDefineDelete="Yes"
  extTable=""
  numberOfKey=5
  dataProg="RTSalesprojectscheduleD2.ASP"
  datawindowFeature=""
  searchWindowFeature="width=700,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=FALSE
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=30
  searchProg="rtcusts.asp"
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" and hbadslcmty.comn <>'' "
     searchShow="全部"
  ELSE
     searchFirst=False
  End If
  logonid=session("userid")
  Call SrGetEmployeeRef(Rtnvalue,1,logonid)
  xyz=split(rtnvalue,";") 
  if ckind <> "" then
     searchqry=searchqry & " and HBADSLCMTY.comtype='" & ckind & "'"
  end if
  IF csalesarea="" THEN csalesarea=";"
  csalesary=split(csalesarea,";")
  if csalesary(0) <> "" then
     set connYY=server.CreateObject("ADODB.connection")
     set rsYY=server.CreateObject("ADODB.recordset")
     dsnYY="DSN=RTLIB"
     sqlYY="SELECT * FROM RTSalesGroup WHERE  (AREAID = '" & csalesary(0) & "') AND (GROUPID = '" &  csalesary(1) & "')"
     connYY.Open dsnYY
     rsYY.Open sqlYY,connYY
     if not rsYY.EOF then
        xgroup=rsyy("groupnc")
     else
        xgroup=""
     end if
     rsYY.Close
     connYY.Close
     set rsYY=nothing
     set connYY=nothing
     '-----------
     searchqry=searchqry & " and HBADSLCMTY.groupnc='" & xgroup & "'" 
  end if 
  if cemply <> "" then
     set connYY=server.CreateObject("ADODB.connection")
     set rsYY=server.CreateObject("ADODB.recordset")
     dsnYY="DSN=RTLIB"
     sqlYY="SELECT * FROM RTemployee inner join rtobj on rtemployee.cusid=rtobj.cusid WHERE  emply='" &  cemply & "'"
     connYY.Open dsnYY
     rsYY.Open sqlYY,connYY
     if not rsYY.EOF then
        xname=rsyy("cusnc")
     else
        xname=""
     end if
     rsYY.Close
     connYY.Close
     set rsYY=nothing
     set connYY=nothing
     '-----------
     searchqry=searchqry & " and HBADSLCMTY.leader='" & xname & "'" 
     session("emply")=cemply
  else
    ' session("emply")=""
  end if 
  '只選直銷社區
  searchqry=searchqry & " and HBADSLCMTY.comsource not in ('友電','錢倉','強泰','菱電','宏韋','合鑫','二煒','亞太欣業','捷揚','驊達','速捷','遠寬','遠端','山資') "
  sqllist="SELECT HBADSLCMTY.comq1,HBADSLCMTY.LINEQ1,HBADSLCMTY.comtype,RTSalesSchedule.syy,RTSalesSchedule.smm,HBADSLCMTY.comq1 AS Expr1,HBADSLCMTY.comn,CASE WHEN HBADSLCMTY.comtype = '1' " _
         &"THEN '元訊599' WHEN HBADSLCMTY.comtype = '2' THEN '中華399' WHEN HBADSLCMTY.comtype = '3' THEN '速博399' WHEN " _
         &"HBADSLCMTY.comtype = '4' THEN '東訊599' WHEN HBADSLCMTY.comtype = '5' THEN '東森499' ELSE '???' END AS comtype,HBADSLCMTY.leader,HBADSLCMTY.T1APPLYDAT, HBADSLCMTY.RCOMDROP, " _
         &"HBSalesManageProjectD_31.STATDESC AS STATDESC1, HBSalesManageProjectD_1.STATDESC AS STATDESC2,HBSalesManageProjectD_2.STATDESC AS STATDESC3, " _
         &"HBSalesManageProjectD_3.STATDESC AS STATDESC4,  HBSalesManageProjectD_4.STATDESC AS STATDESC5, HBSalesManageProjectD_5.STATDESC AS STATDESC6, " _
         &"HBSalesManageProjectD_6.STATDESC AS STATDESC7,  HBSalesManageProjectD_7.STATDESC AS STATDESC8, HBSalesManageProjectD_8.STATDESC AS STATDESC9, " _
         &"HBSalesManageProjectD_9.STATDESC AS STATDESC10, HBSalesManageProjectD_10.STATDESC AS STATDESC11, HBSalesManageProjectD_11.STATDESC AS STATDESC12, " _
         &"HBSalesManageProjectD_12.STATDESC AS STATDESC13, HBSalesManageProjectD_13.STATDESC AS STATDESC14, HBSalesManageProjectD_14.STATDESC AS STATDESC15, " _
         &"HBSalesManageProjectD_15.STATDESC AS STATDESC16, HBSalesManageProjectD_16.STATDESC AS STATDESC17, HBSalesManageProjectD_17.STATDESC AS STATDESC18, " _
         &"HBSalesManageProjectD_18.STATDESC AS STATDESC19, HBSalesManageProjectD_19.STATDESC AS STATDESC20, HBSalesManageProjectD_20.STATDESC AS STATDESC21, " _
         &"HBSalesManageProjectD_21.STATDESC AS STATDESC22, HBSalesManageProjectD_22.STATDESC AS STATDESC23, HBSalesManageProjectD_23.STATDESC AS STATDESC24, " _
         &"HBSalesManageProjectD_24.STATDESC AS STATDESC25, HBSalesManageProjectD_25.STATDESC AS STATDESC26, HBSalesManageProjectD_26.STATDESC AS STATDESC27, " _
         &"HBSalesManageProjectD_27.STATDESC AS STATDESC28, HBSalesManageProjectD_28.STATDESC AS STATDESC29, HBSalesManageProjectD_29.STATDESC AS STATDESC30, " _
         &"HBSalesManageProjectD_30.STATDESC AS STATDESC31 " _
         &"FROM   RTSalesSchedule LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_30 ON " _
         &"RTSalesSchedule.SDD31 = HBSalesManageProjectD_30.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "' LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_29 ON " _
         &"RTSalesSchedule.SDD30 = HBSalesManageProjectD_29.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_28 ON " _
         &"RTSalesSchedule.SDD29 = HBSalesManageProjectD_28.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_27 ON " _
         &"RTSalesSchedule.SDD28 = HBSalesManageProjectD_27.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_26 ON " _
         &"RTSalesSchedule.SDD27 = HBSalesManageProjectD_26.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_25 ON " _
         &"RTSalesSchedule.SDD26 = HBSalesManageProjectD_25.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_24 ON " _
         &"RTSalesSchedule.SDD25 = HBSalesManageProjectD_24.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_23 ON " _
         &"RTSalesSchedule.SDD24 = HBSalesManageProjectD_23.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_22 ON " _
         &"RTSalesSchedule.SDD23 = HBSalesManageProjectD_22.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_21 ON " _
         &"RTSalesSchedule.SDD22 = HBSalesManageProjectD_21.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_20 ON " _
         &"RTSalesSchedule.SDD21 = HBSalesManageProjectD_20.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_19 ON " _
         &"RTSalesSchedule.SDD20 = HBSalesManageProjectD_19.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_18 ON " _
         &"RTSalesSchedule.SDD19 = HBSalesManageProjectD_18.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_17 ON " _
         &"RTSalesSchedule.SDD18 = HBSalesManageProjectD_17.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_16 ON " _
         &"RTSalesSchedule.SDD17 = HBSalesManageProjectD_16.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_15 ON " _
         &"RTSalesSchedule.SDD16 = HBSalesManageProjectD_15.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_14 ON " _
         &"RTSalesSchedule.SDD15 = HBSalesManageProjectD_14.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_13 ON " _
         &"RTSalesSchedule.SDD14 = HBSalesManageProjectD_13.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_12 ON " _
         &"RTSalesSchedule.SDD13 = HBSalesManageProjectD_12.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "' LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_11 ON " _
         &"RTSalesSchedule.SDD12 = HBSalesManageProjectD_11.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_10 ON " _
         &"RTSalesSchedule.SDD11 = HBSalesManageProjectD_10.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_9 ON " _
         &"RTSalesSchedule.SDD10 = HBSalesManageProjectD_9.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_8 ON " _
         &"RTSalesSchedule.SDD9 = HBSalesManageProjectD_8.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_7 ON " _
         &"RTSalesSchedule.SDD8 = HBSalesManageProjectD_7.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_6 ON " _
         &"RTSalesSchedule.SDD7 = HBSalesManageProjectD_6.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_5 ON " _
         &"RTSalesSchedule.SDD6 = HBSalesManageProjectD_5.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_4 ON " _
         &"RTSalesSchedule.SDD5 = HBSalesManageProjectD_4.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_3 ON " _
         &"RTSalesSchedule.SDD4 = HBSalesManageProjectD_3.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_2 ON " _
         &"RTSalesSchedule.SDD3 = HBSalesManageProjectD_2.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_1 ON " _
         &"RTSalesSchedule.SDD2 = HBSalesManageProjectD_1.STAT and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  LEFT OUTER JOIN " _
         &"HBSalesManageProjectD HBSalesManageProjectD_31 ON " _
         &"RTSalesSchedule.SDD1 = HBSalesManageProjectD_31.STAT  and RTSalesSchedule.syy='" & cyy & "' and RTSalesSchedule.smm='" & Cmm & "'  RIGHT OUTER JOIN " _
         &"HBADSLCMTY ON RTSalesSchedule.COMQ1 = HBADSLCMTY.comq1 AND " _
         &"RTSalesSchedule.LINEQ1 = HBADSLCMTY.LINEQ1 AND " _
         &"RTSalesSchedule.CONNECTTYPE = HBADSLCMTY.comtype AND RTSalesSchedule.SYY='" & CYY & "' AND RTSalesSchedule.SMM='" & CMM & "' " _
         &"WHERE hbadslcmty.comn <>'' and hbadslcmty.RCOMDROP is null" &  searchqry & " " _
         &"order by comn "

 'Response.Write "sql=" & SQLLIST
 'RESPONSE.END
End Sub
%>
