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
  system="AVS-City管理系統"
  title="AVS-City用戶設備資料查詢"
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
  sqlDelete="SELECT         RTLessorAVSCustHardware.CUSID, RTLessorAVSCustHardware.PRTNO, " _
                   &"       RTLessorAVSCustHardware.ENTRYNO,'新戶','', RTLessorAVSCustHardware.PRODNO, " _
                   &"        RTLessorAVSCustHardware.ITEMNO, RTProdH.PRODNC, RTProdD1.SPEC, " _
                   &"        RTLessorAVSCustHardware.QTY, RTLessorAVSCustHardware.AMT, " _
                   &"        RTLessorAVSCustSndwork.CLOSEDAT, RTLessorAVSCustHardware.BATCHNO, " _
                   &"        RTLessorAVSCustAR.REALAMT, RTLessorAVSCustHardware.RCVPRTNO, " _
                    &"       RTLessorAVSCustRCVHardware.CLOSEDAT AS Expr1 " _
&" FROM             RTLessorAVSCustHardware LEFT OUTER JOIN " _
   &"                        RTLessorAVSCustRCVHardware ON  " _
      &"                     RTLessorAVSCustHardware.RCVPRTNO = RTLessorAVSCustRCVHardware.RCVPRTNO AND " _
         &"                   RTLessorAVSCustRCVHardware.CANCELDAT IS NULL LEFT OUTER JOIN " _
            &"               RTLessorAVSCustAR ON  " _
               &"            RTLessorAVSCustHardware.BATCHNO = RTLessorAVSCustAR.BATCHNO AND  " _
                  &"         RTLessorAVSCustHardware.CUSID = RTLessorAVSCustAR.CUSID AND  " _
                     &"      RTLessorAVSCustAR.CANCELDAT IS NULL LEFT OUTER JOIN " _
                        &"   RTLessorAVSCustSndwork ON  " _
&"                           RTLessorAVSCustHardware.PRTNO = RTLessorAVSCustSndwork.PRTNO AND  " _
   &"                        RTLessorAVSCustSndwork.DROPDAT IS NULL AND  " _
      &"                     RTLessorAVSCustSndwork.UNCLOSEDAT IS NULL LEFT OUTER JOIN " _
         &"                  RTProdD1 ON RTLessorAVSCustHardware.PRODNO = RTProdD1.PRODNO AND  " _
            &"               RTLessorAVSCustHardware.ITEMNO = RTProdD1.ITEMNO LEFT OUTER JOIN " _
               &"            RTProdH ON  " _
                  &"        RTLessorAVSCustHardware.PRODNO = RTProdH.PRODNO LEFT OUTER JOIN " _
                     &"      RTLessorAVSCust ON  " _
                     &"      RTLessorAVSCustHardware.CUSID = RTLessorAVSCust.CUSID " _
&" WHERE         (RTLessorAVSCustHardware.CUSID ='') " 
  dataTable="RTLessorAVScustHARDWARE"
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
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorAVSCmtyH ON " _
       &"RTCounty.CUTID = RTLessorAVSCmtyH.CUTID RIGHT OUTER JOIN RTLessorAVSCust ON RTLessorAVSCmtyH.COMQ1 = RTLessorAVSCust.COMQ1 " _
       &"where RTLessorAVSCust.cusid='" & ARYPARMKEY(0) & "'"
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
  else
     COMN=""
  end if
  rsYY.Close
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorAVSCmtyLine ON  " _
       &"RTCounty.CUTID = RTLessorAVSCmtyLine.CUTID RIGHT OUTER JOIN " _
       &"RTLessorAVSCust ON RTLessorAVSCmtyLine.COMQ1 = RTLessorAVSCust.COMQ1 AND " _
       &"RTLessorAVSCmtyLine.LINEQ1 = RTLessorAVSCust.LINEQ1 " _
       &"where RTLessorAVSCust.cusid='" & ARYPARMKEY(0) & "'"
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
  sqlYY="select * from RTLessorAVSCUST  where CUSID='" & ARYPARMKEY(0) & "' "
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
     searchQry=" RTLessorAVScusthardware.cusid <> '' "
     searchShow="主線︰"& comq1xx & "-" & lineq1xx & ",社區名稱︰" & COMN & ",主線位址︰" & COMADDR & ",用戶︰" & cusnc & ",派工單號︰" & aryparmkey(1)
  ELSE
     SEARCHFIRST=FALSE
  End If  

  sqlList="SELECT         RTLessorAVSCustHardware.CUSID, RTLessorAVSCustHardware.PRTNO, " _
                   &"       RTLessorAVSCustHardware.ENTRYNO,'新戶','', RTLessorAVSCustHardware.PRODNO, " _
                   &"        RTLessorAVSCustHardware.ITEMNO, RTProdH.PRODNC, RTProdD1.SPEC, " _
                   &"        RTLessorAVSCustHardware.QTY, RTLessorAVSCustHardware.AMT, " _
                   &"        RTLessorAVSCustSndwork.CLOSEDAT, RTLessorAVSCustHardware.BATCHNO, " _
                   &"        ISNULL(RTLessorAVSCustAR.REALAMT,0), RTLessorAVSCustHardware.RCVPRTNO, " _
                    &"       RTLessorAVSCustRCVHardware.CLOSEDAT AS Expr1 " _
&" FROM             RTLessorAVSCustHardware LEFT OUTER JOIN " _
   &"                        RTLessorAVSCustRCVHardware ON  " _
      &"                     RTLessorAVSCustHardware.RCVPRTNO = RTLessorAVSCustRCVHardware.RCVPRTNO AND " _
         &"                   RTLessorAVSCustRCVHardware.CANCELDAT IS NULL LEFT OUTER JOIN " _
            &"               RTLessorAVSCustAR ON  " _
               &"            RTLessorAVSCustHardware.BATCHNO = RTLessorAVSCustAR.BATCHNO AND  " _
                  &"         RTLessorAVSCustHardware.CUSID = RTLessorAVSCustAR.CUSID AND  " _
                     &"      RTLessorAVSCustAR.CANCELDAT IS NULL LEFT OUTER JOIN " _
                        &"   RTLessorAVSCustSndwork ON  " _
&"                           RTLessorAVSCustHardware.PRTNO = RTLessorAVSCustSndwork.PRTNO AND  " _
   &"                        RTLessorAVSCustSndwork.DROPDAT IS NULL AND  " _
      &"                     RTLessorAVSCustSndwork.UNCLOSEDAT IS NULL LEFT OUTER JOIN " _
         &"                  RTProdD1 ON RTLessorAVSCustHardware.PRODNO = RTProdD1.PRODNO AND  " _
            &"               RTLessorAVSCustHardware.ITEMNO = RTProdD1.ITEMNO LEFT OUTER JOIN " _
               &"            RTProdH ON  " _
                  &"        RTLessorAVSCustHardware.PRODNO = RTProdH.PRODNO LEFT OUTER JOIN " _
                     &"      RTLessorAVSCust ON  " _
                     &"      RTLessorAVSCustHardware.CUSID = RTLessorAVSCust.CUSID " _
&" WHERE         (RTLessorAVSCustHardware.DROPDAT IS NULL) and RTLessorAVSCustHardware.cusid='" & ARYPARMKEY(0) & "' " _
&" UNION " _
&" SELECT         RTLessorAVSCustContHardware.CUSID, RTLessorAVSCustContHardware.PRTNO,  " _
 &"                          RTLessorAVSCustContHardware.SEQ,'續約','', RTLessorAVSCustContHardware.PRODNO,  " _
&"                           RTLessorAVSCustContHardware.ITEMNO, RTProdH.PRODNC, RTProdD1.SPEC,  " _
 &"                          RTLessorAVSCustContHardware.QTY, RTLessorAVSCustContHardware.AMT,  " _
  &"                         RTLessorAVSCustContSndwork.CLOSEDAT, RTLessorAVSCustContHardware.BATCHNO,  " _
  &"                         ISNULL(RTLessorAVSCustAR.REALAMT,0), RTLessorAVSCustContHardware.RCVPRTNO,  " _
   &"                        RTLessorAVSCustRCVHardware.CLOSEDAT AS Expr1 " _
&" FROM             RTLessorAVSCustContHardware LEFT OUTER JOIN " _
        &"                   RTLessorAVSCustRCVHardware ON  " _
         &"                  RTLessorAVSCustContHardware.RCVPRTNO = RTLessorAVSCustRCVHardware.RCVPRTNO AND " _
        &"                    RTLessorAVSCustRCVHardware.CANCELDAT IS NULL LEFT OUTER JOIN " _ 
        &"                   RTLessorAVSCustAR ON  " _
          &"                 RTLessorAVSCustContHardware.BATCHNO = RTLessorAVSCustAR.BATCHNO AND  " _
         &"                  RTLessorAVSCustContHardware.CUSID = RTLessorAVSCustAR.CUSID AND  " _
          &"                 RTLessorAVSCustAR.CANCELDAT IS NULL LEFT OUTER JOIN " _
          &"                 RTLessorAVSCustContSndwork ON  " _
           &"                RTLessorAVSCustContHardware.PRTNO = RTLessorAVSCustContSndwork.PRTNO AND  " _
          &"                 RTLessorAVSCustContSndwork.DROPDAT IS NULL AND  " _
          &"                 RTLessorAVSCustContSndwork.UNCLOSEDAT IS NULL LEFT OUTER JOIN " _
          &"                 RTProdD1 ON RTLessorAVSCustContHardware.PRODNO = RTProdD1.PRODNO AND  " _
           &"                RTLessorAVSCustContHardware.ITEMNO = RTProdD1.ITEMNO LEFT OUTER JOIN " _
          &"                 RTProdH ON  " _
          &"                 RTLessorAVSCustContHardware.PRODNO = RTProdH.PRODNO LEFT OUTER JOIN " _
          &"                 RTLessorAVSCust ON  " _
          &"                 RTLessorAVSCustContHardware.CUSID = RTLessorAVSCust.CUSID " _
&" WHERE         (RTLessorAVSCustContHardware.DROPDAT IS NULL) and RTLessorAVSCustContHardware.cusid='" & ARYPARMKEY(0) & "' " _
&" UNION " _
&" SELECT         RTLessorAVSCustReturnHardware.CUSID, RTLessorAVSCustReturnHardware.PRTNO,  " _
       &"                    RTLessorAVSCustReturnHardware.SEQ,'復機','', RTLessorAVSCustReturnHardware.PRODNO,  " _
       &"                    RTLessorAVSCustReturnHardware.ITEMNO, RTProdH.PRODNC, RTProdD1.SPEC,  " _
       &"                    RTLessorAVSCustReturnHardware.QTY, RTLessorAVSCustReturnHardware.AMT,  " _
       &"                    RTLessorAVSCustReturnSndwork.CLOSEDAT, RTLessorAVSCustReturnHardware.BATCHNO,  " _
       &"                    ISNULL(RTLessorAVSCustAR.REALAMT,0), RTLessorAVSCustReturnHardware.RCVPRTNO,  " _
        &"                   RTLessorAVSCustRCVHardware.CLOSEDAT AS Expr1 " _
&" FROM             RTLessorAVSCustReturnHardware LEFT OUTER JOIN " _
         &"                  RTLessorAVSCustRCVHardware ON  " _
         &"                  RTLessorAVSCustReturnHardware.RCVPRTNO = RTLessorAVSCustRCVHardware.RCVPRTNO AND " _
         &"                   RTLessorAVSCustRCVHardware.CANCELDAT IS NULL LEFT OUTER JOIN " _
         &"                  RTLessorAVSCustAR ON  " _
         &"                  RTLessorAVSCustReturnHardware.BATCHNO = RTLessorAVSCustAR.BATCHNO AND  " _
         &"                  RTLessorAVSCustReturnHardware.CUSID = RTLessorAVSCustAR.CUSID AND  " _
         &"                  RTLessorAVSCustAR.CANCELDAT IS NULL LEFT OUTER JOIN " _
          &"                 RTLessorAVSCustReturnSndwork ON  " _
          &"                 RTLessorAVSCustReturnHardware.PRTNO = RTLessorAVSCustReturnSndwork.PRTNO AND  " _
          &"                 RTLessorAVSCustReturnSndwork.DROPDAT IS NULL AND  " _
          &"                 RTLessorAVSCustReturnSndwork.UNCLOSEDAT IS NULL LEFT OUTER JOIN " _
          &"                 RTProdD1 ON RTLessorAVSCustReturnHardware.PRODNO = RTProdD1.PRODNO AND  " _
          &"                 RTLessorAVSCustReturnHardware.ITEMNO = RTProdD1.ITEMNO LEFT OUTER JOIN " _
          &"                 RTProdH ON  " _
         &"                  RTLessorAVSCustReturnHardware.PRODNO = RTProdH.PRODNO LEFT OUTER JOIN " _
         &"                  RTLessorAVSCust ON  " _
         &"                  RTLessorAVSCustReturnHardware.CUSID = RTLessorAVSCust.CUSID " _
&" WHERE         (RTLessorAVSCustReturnHardware.DROPDAT IS NULL) and RTLessorAVSCustReturnHardware.cusid='" & ARYPARMKEY(0) & "' " _
&" UNION " _
&" SELECT         RTLessorAVSCustFaqHardware.CUSID, RTLessorAVSCustFaqHardware.PRTNO,  " _
         &"                  RTLessorAVSCustFaqHardware.seq,'客服',RTLessorAVSCustFaqHardware.FAQNO, RTLessorAVSCustFaqHardware.PRODNO,  " _
         &"                  RTLessorAVSCustFaqHardware.ITEMNO, RTProdH.PRODNC, RTProdD1.SPEC,  " _
         &"                  RTLessorAVSCustFaqHardware.QTY, RTLessorAVSCustFaqHardware.AMT,  " _
         &"                  RTLessorAVSCustFaqSndwork.CLOSEDAT, RTLessorAVSCustFaqHardware.BATCHNO,  " _
         &"                  ISNULL(RTLessorAVSCustAR.REALAMT,0), RTLessorAVSCustFaqHardware.RCVPRTNO,  " _
          &"                 RTLessorAVSCustRCVHardware.CLOSEDAT AS Expr1 " _
&" FROM             RTLessorAVSCustFaqHardware LEFT OUTER JOIN " _
         &"                  RTLessorAVSCustRCVHardware ON  " _
         &"                  RTLessorAVSCustFaqHardware.RCVPRTNO = RTLessorAVSCustRCVHardware.RCVPRTNO AND " _
         &"                   RTLessorAVSCustRCVHardware.CANCELDAT IS NULL LEFT OUTER JOIN " _
         &"                  RTLessorAVSCustAR ON  " _
         &"                  RTLessorAVSCustFaqHardware.BATCHNO = RTLessorAVSCustAR.BATCHNO AND  " _
          &"                 RTLessorAVSCustFaqHardware.CUSID = RTLessorAVSCustAR.CUSID AND  " _
         &"                  RTLessorAVSCustAR.CANCELDAT IS NULL LEFT OUTER JOIN " _
         &"                  RTLessorAVSCustFaqSndwork ON  " _
         &"                  RTLessorAVSCustFaqHardware.PRTNO = RTLessorAVSCustFaqSndwork.PRTNO AND  " _
         &"                  RTLessorAVSCustFaqSndwork.DROPDAT IS NULL AND  " _
         &"                  RTLessorAVSCustFaqSndwork.UNCLOSEDAT IS NULL LEFT OUTER JOIN " _
         &"                  RTProdD1 ON RTLessorAVSCustFaqHardware.PRODNO = RTProdD1.PRODNO AND  " _
         &"                  RTLessorAVSCustFaqHardware.ITEMNO = RTProdD1.ITEMNO LEFT OUTER JOIN " _
         &"                  RTProdH ON  " _
         &"                  RTLessorAVSCustFaqHardware.PRODNO = RTProdH.PRODNO LEFT OUTER JOIN " _
         &"                  RTLessorAVSCust ON  " _
         &"                  RTLessorAVSCustFaqHardware.CUSID = RTLessorAVSCust.CUSID " _
&" WHERE         (RTLessorAVSCustFaqHardware.DROPDAT IS NULL) and RTLessorAVSCustFaqHardware.cusid='" & ARYPARMKEY(0) & "' "

'Response.Write "sql=" & SQLLIST         
End Sub
%>
