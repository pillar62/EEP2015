<!-- #include virtual="/WebUtilityV4/DBAUDI/keyList.inc" -->
<!-- #include virtual="/WebUtilityV4/DBAUDI/cType.inc" -->
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
<link REL="stylesheet" HREF="/webUtilityV4/DBAUDI/keyList.css" TYPE="text/css">
<link REL="stylesheet" HREF="keyList.css" TYPE="text/css">
<!-- #include virtual="/WebUtilityV4/DBAUDI/deleteDialogue.inc" -->
<script language="vbscript">
Sub runAUDI(accessMode,key)
    Dim prog,strFeature,msg
    prog="<%=dataProg%>"
    strFeature="<%=detailWindowFeature%>"
    If prog="None" Then
    Else
       Randomize  
       if accessmode="A" then
          Scrxx=window.screen.width
          Scryy=window.screen.height - 30
         ' StrFeature="Top=" & (scryy - 200) /2 & ",left=" & (scrxx - 450) /2 & ",scrollbars=yes,status=yes," _
         '           &"location=no,menubar=no,width=450px" _
         '           &",height=200px"
        '  prog="RTFaQKind.asp?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
        prog="<%=dataProg%>?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
       else
          prog="<%=dataProg%>?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
       end if
      
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
    '2001/8/30-S
    aryOptOpen=Split("<%=functionOptOpen%>",";")    
    '2001/8/30-E
    StrFeature="<%=optionwindowFeature%>"
    if strfeature="" then
       Scrxx=window.screen.width
       Scryy=window.screen.height - 30
       StrFeature="top=0,left=0,scrollbars=yes,status=yes," _
                 &"location=no,menubar=no,width=" & scrxx & "px" _
                 &",height=" & scryy & "px" 
    end if       
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
<body bgcolor="C3C9D2">
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
         <input type="text" name="searchQry2" value="<%=searchQry2%>" style="display:none" readonly></td>
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
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="ADSL及Hibuilding管理系統"
  title="社區主機客訴處理"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="Y;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=" 作  廢 ;作廢反轉;維修派工; 結  案 ;結案反轉; 列  印 ;異動記錄"
  functionOptProgram="RTFaqDrop.asp;RTFaqDropC.asp;RTFaqSNDWORKK.asp;RTFaqF.asp;RTFaqFR.asp;/RTap/Base/RTHBAdslCmty/RTFaqCmtyV.asp;RTFaqLOGK.asp"
  functionOptPrompt="Y;Y;N;N;N;Y;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="案件編號;社區序號;主線序號;社區方案;主機設備種類;受理人員;受理日期;結案日期;作廢日期;已結案維修單件數;未結案維修單件數" 
  sqlDelete="SELECT HBADSLCMTYFIXH.FIXNO, HBADSLCMTYFIXH.COMQ1, HBADSLCMTYFIXH.LINEQ1, CASE WHEN HBADSLCMTYFIXH.comtype = '1' THEN '元訊599' WHEN HBADSLCMTYFIXH.comtype " _
           &"='2' THEN '中華399' WHEN HBADSLCMTYFIXH.comtype = '3' THEN '速博399' WHEN HBADSLCMTYFIXH.comtype = '4' THEN '東訊599' WHEN HBADSLCMTYFIXH.comtype " _
           &"='5' THEN '東森499' ELSE '???' END AS comtype, RTCode.CODENC, " _
           &"RTObj.CUSNC, HBADSLCMTYFIXH.RCVTIME, HBADSLCMTYFIXSNDWORK.PRTNO " _
           &"FROM HBADSLCMTYFIXSNDWORK RIGHT OUTER JOIN HBADSLCMTYFIXH INNER JOIN RTCode ON HBADSLCMTYFIXH.DEVICETYPE = RTCode.CODE AND " _ 
           &"RTCode.KIND = 'J1' ON HBADSLCMTYFIXSNDWORK.FIXNO = HBADSLCMTYFIXH.FIXNO AND HBADSLCMTYFIXSNDWORK.DROPDAT IS NULL " _
           &"LEFT OUTER JOIN RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON HBADSLCMTYFIXH.RCVMAN = RTEmployee.EMPLY " _
           &"WHERE HBADSLCMTYFIXH.FIXNO='*' "
  dataTable="RTFAQH"
  userDefineDelete=""
  numberOfKey=1
  dataProg="RTFAQD2.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=False
  goodMorningImage="cbbn.JPG"
  colSplit=1
  keyListPageSize=20
  searchProg="self"
  parmkey=""
  searchFirst=False
' When first time enter this keylist default query string to RTcmty.CASENO <> '*'
  searchFirst=False
  If searchQry="" Then
     searchQry=" HBADSLCMTYFIXH.COMQ1<> 0 AND HBADSLCMTYFIXH.CLOSEDAT IS NULL AND HBADSLCMTYFIXH.DROPDAT IS NULL "
  '   response.Write "0=" & aryparmkey(0) & ";1=" & aryparmkey(1) & ";3=" & aryparmkey(2)
   '  searchShow=FrGetCmtyDesc(ARYPARMKEY(0),ARYPARMKEY(1),ARYPARMKEY(2))
     searchShow="全部未結案、未作廢"
  End If
  sqlList="SELECT HBADSLCMTYFIXH.FIXNO, HBADSLCMTYFIXH.COMQ1, HBADSLCMTYFIXH.LINEQ1, CASE WHEN HBADSLCMTYFIXH.comtype = '1' THEN '元訊599' WHEN HBADSLCMTYFIXH.comtype " _
           &"='2' THEN '中華399' WHEN HBADSLCMTYFIXH.comtype = '3' THEN '速博399' WHEN HBADSLCMTYFIXH.comtype = '4' THEN '東訊599' WHEN HBADSLCMTYFIXH.comtype " _
           &"='5' THEN '東森499' ELSE '???' END AS comtype, RTCode.CODENC, " _
           &"RTObj.CUSNC, HBADSLCMTYFIXH.RCVTIME,HBADSLCMTYFIXH.CLOSEDAT,HBADSLCMTYFIXH.DROPDAT, SUM(CASE WHEN HBADSLCMTYFIXSNDWORK.FIXNO IS NOT NULL AND " _
           &"HBADSLCMTYFIXSNDWORK.DROPDAT IS NULL AND HBADSLCMTYFIXSNDWORK.UNCLOSEDAT IS NULL and HBADSLCMTYFIXSNDWORK.closedat is not null THEN 1 ELSE 0 END), " _
           &"SUM(CASE WHEN HBADSLCMTYFIXSNDWORK.FIXNO IS NOT NULL AND " _
           &"HBADSLCMTYFIXSNDWORK.DROPDAT IS NULL AND HBADSLCMTYFIXSNDWORK.UNCLOSEDAT IS NULL and HBADSLCMTYFIXSNDWORK.closedat is null THEN 1 ELSE 0 END) " _
           &"FROM HBADSLCMTYFIXSNDWORK RIGHT OUTER JOIN HBADSLCMTYFIXH INNER JOIN RTCode ON HBADSLCMTYFIXH.DEVICETYPE = RTCode.CODE AND " _ 
           &"RTCode.KIND = 'J1' ON HBADSLCMTYFIXSNDWORK.FIXNO = HBADSLCMTYFIXH.FIXNO AND HBADSLCMTYFIXSNDWORK.DROPDAT IS NULL " _
           &"LEFT OUTER JOIN RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON HBADSLCMTYFIXH.RCVMAN = RTEmployee.EMPLY " _
           &"WHERE HBADSLCMTYFIXH.FIXNO<>'*' and " &searchQry &" " _
           &"group by HBADSLCMTYFIXH.FIXNO, HBADSLCMTYFIXH.COMQ1, HBADSLCMTYFIXH.LINEQ1, CASE WHEN HBADSLCMTYFIXH.comtype = '1' THEN '元訊599' WHEN HBADSLCMTYFIXH.comtype " _
           &"='2' THEN '中華399' WHEN HBADSLCMTYFIXH.comtype = '3' THEN '速博399' WHEN HBADSLCMTYFIXH.comtype = '4' THEN '東訊599' WHEN HBADSLCMTYFIXH.comtype " _
           &"='5' THEN '東森499' ELSE '???' END , RTCode.CODENC, RTObj.CUSNC, " _
           &"HBADSLCMTYFIXH.RCVTIME, HBADSLCMTYFIXH.CLOSEDAT, HBADSLCMTYFIXH.DROPDAT " _
           &"ORDER BY HBADSLCMTYFIXH.FIXNO "
'  session("comq1")=aryparmkey(0)
'  session("lineq1")=aryparmkey(1)
'  session("comtype")=aryparmkey(2)  
'Response.Write "SQL=" & SQLlist
End Sub

%>
<!-- #include file="RTGetCMTYDESC.inc" -->