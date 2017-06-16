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
<%
  Const cTypeChar="129;130;200;201;202;203"
  Const cTypeN="131"  
  Const cTypeNumeric="002;003;005;006;017;131"
  Const cTypeDate="135"
  Const cTypeBoolean="011"
  Const cTypeVarChar="203"
  Const cTypeInterger="002"
  Const cTypeFloat="006"
  Const cTypeAuto="000"
%>
<%
  Dim company,system,title,buttonName,buttonEnable,DSN,formatName,sqlList,sqlListOrder,numberOfKey,sqlDelete
  Dim dataTable,dataProg,dataWindowFeature,accessMode,dataProgParm
  Dim diaWidth,diaHeight,diaTitle,diaButtonName,extTable,extTableKey,extDeleList(20),userDefineDelete
  Dim aryKeyName,aryKeyType(100),aryKeyNameDB(100)
  Dim goodMorning,goodMorningAns,goodMorningImage,colSplit
  Dim keyListPageSize,keyListPage,totalPage
  Dim functionOptName,functionOptProgram,functionOptPrompt
  Dim searchProg,searchQry,searchShow,searchFirst
  Dim aryParmKey,parmKey,searchwindowfeature,optionwindowfeature
  Dim detailwindowFeature,rscount
  searchFirst=False
  userDefineDelete="No"
  functionOptPrompt=";;;;;;;;;;;;;;;;;;"
  keyListPageSize=0
  keyListPage=1
  colSplit=1
  searchQry=Request("searchQry")
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
<link REL="stylesheet" HREF="/webUtilityV4/DBAUDI/keyList.css" TYPE="text/css">
<link REL="stylesheet" HREF="keyList.css" TYPE="text/css">
<!-- #include virtual="/WebUtilityV4/DBAUDI/deleteDialogue.inc" -->
<script language="vbscript">
Sub runAUDI(accessMode,key)
    Dim prog,strFeature,msg
    prog="<%=dataProg%>"
    If prog="None" Then
       parm=split(key,";")
       returnvalue=parm(2)
       call SrClose
    Else
       Randomize  
       prog="<%=dataProg%>?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
      
       strFeature="<%=detailWindowFeature%>"
       if strfeature="" then
          Scrxx=window.screen.width
          Scryy=window.screen.height - 30
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
    StrFeature="<%=optionwindowFeature%>"
    if strfeature="" then
       Scrxx=window.screen.width
       Scryy=window.screen.height - 30
       StrFeature="top=0,left=0,scrollbars=yes,status=yes," _
                 &"location=no,menubar=no,width=" & scrxx & "px" _
                 &",height=" & scryy & "px" 
    end if       
    selItem=0
    Do
      i=i+1
      sel=""
      sel=document.all("sel" &i).value
      On Error Resume Next
      If sel="T" Then
         selItem=i
      End IF
    Loop Until sel<>"T" And sel<>"F" Or selItem<>0
    sureRun=1
    If selItem <> 0 Then
       Randomize  
       prog=aryOptProg(opt) &"?V=" &Rnd() &"&key=" &document.all("key" &selItem).value
       If aryOptPrompt(opt)="Y" Then sureRun=Msgbox("確認執行功能選項---" &aryOptName(opt),vbOKCancel)     
       If sureRun=1 Then Set diagWindow=Window.Open(prog,"",StrFeature)
    Else
       Msgbox("在您執行功能選項前，請先挑選一筆資料")
    End If
End Sub
Sub runSearchOpt()
    Dim prog,sure
<%If  searchProg="" Or searchProg="self" Then
  Else%>
    StrFeature="<%=SearchwindowFeature%>"
    if strfeature="" then
       Scrxx=window.screen.width
       Scryy=window.screen.height - 30
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
<body>
<%End If%>
<table width="100%" cellPadding=0 cellSpacing=0>
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
         <input type="text" name="searchQry" value="<%=searchQry%>" style="display:none" readonly></td>
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
       Scrxx=window.screen.width
       Scryy=window.screen.height - 30
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
<body onClick="newWindow" style="cursor:hand">
<form name="form" method="post">
<table width="100%" cellPadding=0 cellSpacing=0>
  <tr class=keyListTitle><td width="20%" align=left><%=Request.ServerVariables("LOGON_USER")%></td>
                         <td width="60%" align=center><%=company%></td>
                         <td width="20%" align=right><%=Now()%></td><tr>
  <tr class=keyListTitle><td>&nbsp;</td><td align=center><%=title%></td><td>&nbsp;</td><tr>
</table>
<table widt="100%">
  <tr><td background="<%=goodMorningImage%>" height="400" width="400">&nbsp;</td></tr>
  <tr><td><input type="text" name="goodMorningAns" value="No" style="display:none;"></td></tr>
</table>
</form>
</body>
</html>
<%End Sub%>
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  '---清除session變數
  session("CMTYNC")=""
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="ADSL社區資料選擇"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;" & V(3)
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName=""
  functionOptProgram=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;縣市;鄉鎮;社區名稱;目前申請戶數"
   sqlDelete="SELECT DISTINCT " _
            &"singlecustadsl.cutid2, singlecustadsl.TOWNSHIP2, singlecustadsl.HOUSENAME,RTCounty.CUTNC, singlecustadsl.TOWNSHIP2, singlecustadsl.HOUSENAME,count(*) " _
            &"FROM singlecustadsl INNER JOIN " _
            &"RTCounty ON singlecustadsl.CUTID2 = RTCounty.CUTID " _
            &"GROUP BY singlecustadsl.cutid2, singlecustadsl.TOWNSHIP2, singlecustadsl.HOUSENAME,RTCounty.CUTNC, singlecustadsl.TOWNSHIP2, singlecustadsl.HOUSENAME " 
  dataTable="singlecustadsl"
  userDefineDelete=""
  extTable=""
  numberOfKey=3
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=700,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=2
  keyListPageSize=20
  searchProg="RTCmtySelS.asp"
  searchFirst=false
  parm=split(request("PARM"),";")
  'Response.Write "P1=" & parm(0) & ";P2=" & parm(1)
  if len(trim(parm(0))) > 0 then
     searchqry=" singlecustadsl.cutid2='" & parm(0) & "' "
     CUTNC=SrGetCtyRef(parm(0))
     searchshow="縣市別：" & cutnc
     if len(trim(parm(1))) > 0 then
         searchqry=searchqry & " and " & "singlecustadsl.township2='" & parm(1) & "' "
         searchshow=searchshow & "  鄉鎮市區：" & parm(1) & " "
     end if
  end if
  If searchQry="" Then
     searchShow="全部"
     searchQry="singlecustadsl.cutid2<>'*' "
  ELSE
     SEARCHFIRST=FALSE
  End If  
  sqllist="SELECT DISTINCT " _
            &"singlecustadsl.cutid2, singlecustadsl.TOWNSHIP2, singlecustadsl.HOUSENAME,RTCounty.CUTNC, singlecustadsl.TOWNSHIP2, singlecustadsl.HOUSENAME,count(*) " _
            &"FROM singlecustadsl INNER JOIN " _
            &"RTCounty ON singlecustadsl.CUTID2 = RTCounty.CUTID " _
            &"where " & searchqry & " " _
            &"GROUP BY singlecustadsl.cutid2, singlecustadsl.TOWNSHIP2, singlecustadsl.HOUSENAME,RTCounty.CUTNC, singlecustadsl.TOWNSHIP2, singlecustadsl.HOUSENAME " 
'  Response.Write "sql=" & SQLLIST
End Sub

Function SrGetCtyRef(CUTID)
    Dim conn,rs,sql
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
    sql="SELECT cutnc FROM RTCounty where cutid='" & cutid & "'" 
    rs.Open sql,conn
    If not rs.Eof Then
       SrGetctyref=rs("cutnc")
    end if
End function
%>
