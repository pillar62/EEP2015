<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/keyList.inc" -->
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
  aryparmkey(0)=aryparmkey(2)
  parmkey=aryparmkey(0)&";"
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
<table width="100%" cellPadding=0 cellSpacing=0>
  <tr class=keyListTitle><td width="20%" align=left><%=Request.ServerVariables("LOGON_USER")%></td>
                         <td width="60%" align=center><%=company%></td>
                         <td width="20%" align=right><%=Now()%></td><tr>
  <tr class=keyListTitle><td>&nbsp;</td><td align=center><%=title%></td><td>&nbsp;</td><tr>
</table>
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
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="ET-City管理系統"
  title="ET-City用戶設備資料查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""  
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;派工單號;none;派工種類;客服單號;none;none;設備名稱;規格;數量;金額;派工單結案日;帳款編號;沖帳金額;領用單號;實際領用日"
  sqlDelete="SELECT         RTLessorCustHardware.CUSID, RTLessorCustHardware.PRTNO, " _
                   &"       RTLessorCustHardware.ENTRYNO,'新戶','', RTLessorCustHardware.PRODNO, " _
                   &"        RTLessorCustHardware.ITEMNO, RTProdH.PRODNC, RTProdD1.SPEC, " _
                   &"        RTLessorCustHardware.QTY, RTLessorCustHardware.AMT, " _
                   &"        RTLessorCustSndwork.CLOSEDAT, RTLessorCustHardware.BATCHNO, " _
                   &"        RTLessorCustAR.REALAMT, RTLessorCustHardware.RCVPRTNO, " _
                    &"       RTLessorCustRCVHardware.CLOSEDAT AS Expr1 " _
&" FROM             RTLessorCustHardware LEFT OUTER JOIN " _
   &"                        RTLessorCustRCVHardware ON  " _
      &"                     RTLessorCustHardware.RCVPRTNO = RTLessorCustRCVHardware.RCVPRTNO AND " _
         &"                   RTLessorCustRCVHardware.CANCELDAT IS NULL LEFT OUTER JOIN " _
            &"               RTLessorCustAR ON  " _
               &"            RTLessorCustHardware.BATCHNO = RTLessorCustAR.BATCHNO AND  " _
                  &"         RTLessorCustHardware.CUSID = RTLessorCustAR.CUSID AND  " _
                     &"      RTLessorCustAR.CANCELDAT IS NULL LEFT OUTER JOIN " _
                        &"   RTLessorCustSndwork ON  " _
&"                           RTLessorCustHardware.PRTNO = RTLessorCustSndwork.PRTNO AND  " _
   &"                        RTLessorCustSndwork.DROPDAT IS NULL AND  " _
      &"                     RTLessorCustSndwork.UNCLOSEDAT IS NULL LEFT OUTER JOIN " _
         &"                  RTProdD1 ON RTLessorCustHardware.PRODNO = RTProdD1.PRODNO AND  " _
            &"               RTLessorCustHardware.ITEMNO = RTProdD1.ITEMNO LEFT OUTER JOIN " _
               &"            RTProdH ON  " _
                  &"        RTLessorCustHardware.PRODNO = RTProdH.PRODNO LEFT OUTER JOIN " _
                     &"      RTLessorCust ON  " _
                     &"      RTLessorCustHardware.CUSID = RTLessorCust.CUSID " _
&" WHERE         (RTLessorCustHardware.CUSID ='') " 
  dataTable="RTLessorcustHARDWARE"
  extTable=""
  numberOfKey=3
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=350,height=160,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=False
  goodMorningImage=""
  colSplit=1
  keyListPageSize=25
  searchProg="self"
  '----
  set connYY=server.CreateObject("ADODB.connection")
  set rsYY=server.CreateObject("ADODB.recordset")
  dsnYY="DSN=RTLIB"
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorCmtyH ON " _
       &"RTCounty.CUTID = RTLessorCmtyH.CUTID RIGHT OUTER JOIN RTLessorCust ON RTLessorCmtyH.COMQ1 = RTLessorCust.COMQ1 " _
       &"where RTLessorCust.cusid='" & ARYPARMKEY(0) & "'"
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
  else
     COMN=""
  end if
  rsYY.Close
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorCmtyLine ON  " _
       &"RTCounty.CUTID = RTLessorCmtyLine.CUTID RIGHT OUTER JOIN " _
       &"RTLessorCust ON RTLessorCmtyLine.COMQ1 = RTLessorCust.COMQ1 AND " _
       &"RTLessorCmtyLine.LINEQ1 = RTLessorCust.LINEQ1 " _
       &"where RTLessorCust.cusid='" & ARYPARMKEY(0) & "'"
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     comaddr=""
     COMaddr=rsYY("cutnc") & rsyy("township")
     if rsyy("village") <> "" then
         COMaddr= COMaddr & rsyy("village") & rsyy("cod1")
     end if
     if rsyy("NEIGHBOR") <> "" then
         COMaddr= COMaddr & rsyy("NEIGHBOR") & rsyy("cod2")
     end if
     if rsyy("STREET") <> "" then
         COMaddr= COMaddr & rsyy("STREET") & rsyy("cod3")
     end if
     if rsyy("SEC") <> "" then
         COMaddr= COMaddr & rsyy("SEC") & rsyy("cod4")
     end if
     if rsyy("LANE") <> "" then
         COMaddr= COMaddr & rsyy("LANE") & rsyy("cod5")
     end if
     if rsyy("ALLEYWAY") <> "" then
         COMaddr= COMaddr & rsyy("ALLEYWAY") & rsyy("cod7")
     end if
     if rsyy("NUM") <> "" then
         COMaddr= COMaddr & rsyy("NUM") & rsyy("cod8")
     end if
     if rsyy("FLOOR") <> "" then
         COMaddr= COMaddr & rsyy("FLOOR") & rsyy("cod9")
     end if
     if rsyy("ROOM") <> "" then
         COMaddr= COMaddr & rsyy("ROOM") & rsyy("cod10")
     end if
  else
     COMaddr=""
  end if
  RSYY.Close
  sqlYY="select * from RTLESSORCUST  where CUSID='" & ARYPARMKEY(0) & "' "
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     CUSNC=rsYY("CUSNC")
     comq1xx=rsyy("comq1")
     lineq1xx=rsyy("lineq1")
  else
     CUSNC=""
     comq1xx=""
     lineq1xx=""
  end if
  rsYY.Close
  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTLessorcusthardware.cusid <> '' "
     searchShow="主線︰"& comq1xx & "-" & lineq1xx & ",社區名稱︰" & COMN & ",主線位址︰" & COMADDR & ",用戶︰" & cusnc & ",派工單號︰" & aryparmkey(1)
  ELSE
     SEARCHFIRST=FALSE
  End If  

  sqlList="SELECT         RTLessorCustHardware.CUSID, RTLessorCustHardware.PRTNO, " _
                   &"       RTLessorCustHardware.ENTRYNO,'新戶','', RTLessorCustHardware.PRODNO, " _
                   &"        RTLessorCustHardware.ITEMNO, RTProdH.PRODNC, RTProdD1.SPEC, " _
                   &"        RTLessorCustHardware.QTY, RTLessorCustHardware.AMT, " _
                   &"        RTLessorCustSndwork.CLOSEDAT, RTLessorCustHardware.BATCHNO, " _
                   &"        ISNULL(RTLessorCustAR.REALAMT,0), RTLessorCustHardware.RCVPRTNO, " _
                    &"       RTLessorCustRCVHardware.CLOSEDAT AS Expr1 " _
&" FROM             RTLessorCustHardware LEFT OUTER JOIN " _
   &"                        RTLessorCustRCVHardware ON  " _
      &"                     RTLessorCustHardware.RCVPRTNO = RTLessorCustRCVHardware.RCVPRTNO AND " _
         &"                   RTLessorCustRCVHardware.CANCELDAT IS NULL LEFT OUTER JOIN " _
            &"               RTLessorCustAR ON  " _
               &"            RTLessorCustHardware.BATCHNO = RTLessorCustAR.BATCHNO AND  " _
                  &"         RTLessorCustHardware.CUSID = RTLessorCustAR.CUSID AND  " _
                     &"      RTLessorCustAR.CANCELDAT IS NULL LEFT OUTER JOIN " _
                        &"   RTLessorCustSndwork ON  " _
&"                           RTLessorCustHardware.PRTNO = RTLessorCustSndwork.PRTNO AND  " _
   &"                        RTLessorCustSndwork.DROPDAT IS NULL AND  " _
      &"                     RTLessorCustSndwork.UNCLOSEDAT IS NULL LEFT OUTER JOIN " _
         &"                  RTProdD1 ON RTLessorCustHardware.PRODNO = RTProdD1.PRODNO AND  " _
            &"               RTLessorCustHardware.ITEMNO = RTProdD1.ITEMNO LEFT OUTER JOIN " _
               &"            RTProdH ON  " _
                  &"        RTLessorCustHardware.PRODNO = RTProdH.PRODNO LEFT OUTER JOIN " _
                     &"      RTLessorCust ON  " _
                     &"      RTLessorCustHardware.CUSID = RTLessorCust.CUSID " _
&" WHERE         (RTLessorCustHardware.DROPDAT IS NULL) and RTLessorCustHardware.cusid='" & ARYPARMKEY(0) & "' " _
&" UNION " _
&" SELECT         RTLessorCustContHardware.CUSID, RTLessorCustContHardware.PRTNO,  " _
 &"                          RTLessorCustContHardware.SEQ,'續約','', RTLessorCustContHardware.PRODNO,  " _
&"                           RTLessorCustContHardware.ITEMNO, RTProdH.PRODNC, RTProdD1.SPEC,  " _
 &"                          RTLessorCustContHardware.QTY, RTLessorCustContHardware.AMT,  " _
  &"                         RTLessorCustContSndwork.CLOSEDAT, RTLessorCustContHardware.BATCHNO,  " _
  &"                         ISNULL(RTLessorCustAR.REALAMT,0), RTLessorCustContHardware.RCVPRTNO,  " _
   &"                        RTLessorCustRCVHardware.CLOSEDAT AS Expr1 " _
&" FROM             RTLessorCustContHardware LEFT OUTER JOIN " _
        &"                   RTLessorCustRCVHardware ON  " _
         &"                  RTLessorCustContHardware.RCVPRTNO = RTLessorCustRCVHardware.RCVPRTNO AND " _
        &"                    RTLessorCustRCVHardware.CANCELDAT IS NULL LEFT OUTER JOIN " _ 
        &"                   RTLessorCustAR ON  " _
          &"                 RTLessorCustContHardware.BATCHNO = RTLessorCustAR.BATCHNO AND  " _
         &"                  RTLessorCustContHardware.CUSID = RTLessorCustAR.CUSID AND  " _
          &"                 RTLessorCustAR.CANCELDAT IS NULL LEFT OUTER JOIN " _
          &"                 RTLessorCustContSndwork ON  " _
           &"                RTLessorCustContHardware.PRTNO = RTLessorCustContSndwork.PRTNO AND  " _
          &"                 RTLessorCustContSndwork.DROPDAT IS NULL AND  " _
          &"                 RTLessorCustContSndwork.UNCLOSEDAT IS NULL LEFT OUTER JOIN " _
          &"                 RTProdD1 ON RTLessorCustContHardware.PRODNO = RTProdD1.PRODNO AND  " _
           &"                RTLessorCustContHardware.ITEMNO = RTProdD1.ITEMNO LEFT OUTER JOIN " _
          &"                 RTProdH ON  " _
          &"                 RTLessorCustContHardware.PRODNO = RTProdH.PRODNO LEFT OUTER JOIN " _
          &"                 RTLessorCust ON  " _
          &"                 RTLessorCustContHardware.CUSID = RTLessorCust.CUSID " _
&" WHERE         (RTLessorCustContHardware.DROPDAT IS NULL) and RTLessorCustContHardware.cusid='" & ARYPARMKEY(0) & "' " _
&" UNION " _
&" SELECT         RTLessorCustReturnHardware.CUSID, RTLessorCustReturnHardware.PRTNO,  " _
       &"                    RTLessorCustReturnHardware.SEQ,'復機','', RTLessorCustReturnHardware.PRODNO,  " _
       &"                    RTLessorCustReturnHardware.ITEMNO, RTProdH.PRODNC, RTProdD1.SPEC,  " _
       &"                    RTLessorCustReturnHardware.QTY, RTLessorCustReturnHardware.AMT,  " _
       &"                    RTLessorCustReturnSndwork.CLOSEDAT, RTLessorCustReturnHardware.BATCHNO,  " _
       &"                    ISNULL(RTLessorCustAR.REALAMT,0), RTLessorCustReturnHardware.RCVPRTNO,  " _
        &"                   RTLessorCustRCVHardware.CLOSEDAT AS Expr1 " _
&" FROM             RTLessorCustReturnHardware LEFT OUTER JOIN " _
         &"                  RTLessorCustRCVHardware ON  " _
         &"                  RTLessorCustReturnHardware.RCVPRTNO = RTLessorCustRCVHardware.RCVPRTNO AND " _
         &"                   RTLessorCustRCVHardware.CANCELDAT IS NULL LEFT OUTER JOIN " _
         &"                  RTLessorCustAR ON  " _
         &"                  RTLessorCustReturnHardware.BATCHNO = RTLessorCustAR.BATCHNO AND  " _
         &"                  RTLessorCustReturnHardware.CUSID = RTLessorCustAR.CUSID AND  " _
         &"                  RTLessorCustAR.CANCELDAT IS NULL LEFT OUTER JOIN " _
          &"                 RTLessorCustReturnSndwork ON  " _
          &"                 RTLessorCustReturnHardware.PRTNO = RTLessorCustReturnSndwork.PRTNO AND  " _
          &"                 RTLessorCustReturnSndwork.DROPDAT IS NULL AND  " _
          &"                 RTLessorCustReturnSndwork.UNCLOSEDAT IS NULL LEFT OUTER JOIN " _
          &"                 RTProdD1 ON RTLessorCustReturnHardware.PRODNO = RTProdD1.PRODNO AND  " _
          &"                 RTLessorCustReturnHardware.ITEMNO = RTProdD1.ITEMNO LEFT OUTER JOIN " _
          &"                 RTProdH ON  " _
         &"                  RTLessorCustReturnHardware.PRODNO = RTProdH.PRODNO LEFT OUTER JOIN " _
         &"                  RTLessorCust ON  " _
         &"                  RTLessorCustReturnHardware.CUSID = RTLessorCust.CUSID " _
&" WHERE         (RTLessorCustReturnHardware.DROPDAT IS NULL) and RTLessorCustReturnHardware.cusid='" & ARYPARMKEY(0) & "' " _
&" UNION " _
&" SELECT         RTLessorCustFaqHardware.CUSID, RTLessorCustFaqHardware.PRTNO,  " _
         &"                  RTLessorCustFaqHardware.seq,'客服',RTLessorCustFaqHardware.FAQNO, RTLessorCustFaqHardware.PRODNO,  " _
         &"                  RTLessorCustFaqHardware.ITEMNO, RTProdH.PRODNC, RTProdD1.SPEC,  " _
         &"                  RTLessorCustFaqHardware.QTY, RTLessorCustFaqHardware.AMT,  " _
         &"                  RTLessorCustFaqSndwork.CLOSEDAT, RTLessorCustFaqHardware.BATCHNO,  " _
         &"                  ISNULL(RTLessorCustAR.REALAMT,0), RTLessorCustFaqHardware.RCVPRTNO,  " _
          &"                 RTLessorCustRCVHardware.CLOSEDAT AS Expr1 " _
&" FROM             RTLessorCustFaqHardware LEFT OUTER JOIN " _
         &"                  RTLessorCustRCVHardware ON  " _
         &"                  RTLessorCustFaqHardware.RCVPRTNO = RTLessorCustRCVHardware.RCVPRTNO AND " _
         &"                   RTLessorCustRCVHardware.CANCELDAT IS NULL LEFT OUTER JOIN " _
         &"                  RTLessorCustAR ON  " _
         &"                  RTLessorCustFaqHardware.BATCHNO = RTLessorCustAR.BATCHNO AND  " _
          &"                 RTLessorCustFaqHardware.CUSID = RTLessorCustAR.CUSID AND  " _
         &"                  RTLessorCustAR.CANCELDAT IS NULL LEFT OUTER JOIN " _
         &"                  RTLessorCustFaqSndwork ON  " _
         &"                  RTLessorCustFaqHardware.PRTNO = RTLessorCustFaqSndwork.PRTNO AND  " _
         &"                  RTLessorCustFaqSndwork.DROPDAT IS NULL AND  " _
         &"                  RTLessorCustFaqSndwork.UNCLOSEDAT IS NULL LEFT OUTER JOIN " _
         &"                  RTProdD1 ON RTLessorCustFaqHardware.PRODNO = RTProdD1.PRODNO AND  " _
         &"                  RTLessorCustFaqHardware.ITEMNO = RTProdD1.ITEMNO LEFT OUTER JOIN " _
         &"                  RTProdH ON  " _
         &"                  RTLessorCustFaqHardware.PRODNO = RTProdH.PRODNO LEFT OUTER JOIN " _
         &"                  RTLessorCust ON  " _
         &"                  RTLessorCustFaqHardware.CUSID = RTLessorCust.CUSID " _
&" WHERE         (RTLessorCustFaqHardware.DROPDAT IS NULL) and RTLessorCustFaqHardware.cusid='" & ARYPARMKEY(0) & "' "

'Response.Write "sql=" & SQLLIST         
End Sub
%>
