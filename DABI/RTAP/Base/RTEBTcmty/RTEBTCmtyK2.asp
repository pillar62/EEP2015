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
  Dim detailwindowFeature,rscount,searchqry2,searchqry3
  searchFirst=False
  userDefineDelete="No"
  functionOptPrompt=";;;;;;;;;;;;;;;;;;"
  keyListPageSize=0
  keyListPage=1
  colSplit=1
  searchQry=Request("searchQry")
  searchqry2=request("searchqry2")
  searchqry3=request("searchqry3")  
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
       Scrxx=(window.screen.width -7) /1.5
       Scryy=(window.screen.height - 74)/1.5
       scrxx2=(window.screen.width - scrXX)/2
       scryy2=(window.screen.height - scryy)/2
       features="top=" & scrxx2 & ",left=" & scryy2 & ",status=yes,location=no,menubar=no,scrollbars=yes" & ",height=" & scryy & ",width=" & scrxx       
     '  StrFeature="Top=0,left=0,scrollbars=yes,status=yes," _
     '            &"location=no,menubar=no,width=" & scrxx & "px" _
     '            &",height=" & scryy & "px"
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
         <input type="text" name="searchQry2" value="<%=searchQry2%>" style="display:none" readonly ID="Text3">
         <input type="text" name="searchQry3" value="<%=searchQry3%>" style="display:none" readonly ID="Text5"></td>
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
  title="AVS社區基本資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="主線維護;報竣異動;設備查詢;歷史異動"
  functionOptProgram="rtebtcmtylineK.asp;rtEBTcmtyUPDK.asp;rtEBTcmtyHARDWAREK3.asp;rtEBTcmtyLOGK.asp"
  functionOptPrompt="N;N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="社區序號;社區名稱;縣市;鄉鎮;規模戶數;社區轉檔日;申請線數;開通線數;none;申請;撤銷;退租;完工;報竣;差異"
 ' sqlDelete="SELECT * FROM (SELECT        RTEBTCMTYH.COMQ1, RTEBTCMTYH.COMN, " _
 '          &"RTCounty.CUTNC + RTEBTCMTYH.TOWNSHIP + RTEBTCMTYH.VILLAGE + RTEBTCMTYH.NEIGHBOR " _
 '          &"+ RTEBTCMTYH.STREET + RTEBTCMTYH.SEC + RTEBTCMTYH.LANE + RTEBTCMTYH.TOWN " _
 '          &"+ RTEBTCMTYH.ALLEYWAY + RTEBTCMTYH.NUM + RTEBTCMTYH.FLOOR + RTEBTCMTYH.ROOM " _
 '          &"AS raddr, RTEBTCMTYH.COMCNT, " _
 ''          &"RTEBTCMTYH.CONTACT, RTEBTCMTYH.CONTACTRANK, " _
  '         &"RTEBTCMTYH.COMTEL, RTEBTCMTYH.UPDEBTDAT,COUNT(*) as linecnt,SUM(CASE WHEN RTEBTCMTYLINE.ADSLAPPLYDAT IS NOT NULL THEN 1 ELSE 0 END) AS APPLYCNT " _
  '         &"FROM RTEBTCUST INNER JOIN  RTEBTCMTYLINE ON RTEBTCUST.COMQ1 = RTEBTCMTYLINE.COMQ1 AND " _
  '         &"RTEBTCUST.LINEQ1 = RTEBTCMTYLINE.LINEQ1 RIGHT OUTER JOIN RTEBTCMTYH ON RTEBTCMTYLINE.COMQ1 = RTEBTCMTYH.COMQ1 LEFT OUTER JOIN " _
  '         &"RTCounty ON RTEBTCMTYH.CUTID = RTCounty.CUTID GROUP BY  RTEBTCMTYH.COMQ1,RTEBTCMTYH.COMN, " _
  ''         &"RTCounty.CUTNC + RTEBTCMTYH.TOWNSHIP + RTEBTCMTYH.VILLAGE + RTEBTCMTYH.NEIGHBOR " _
  '         &"+ RTEBTCMTYH.STREET + RTEBTCMTYH.SEC + RTEBTCMTYH.LANE + RTEBTCMTYH.TOWN " _
   '        &"+ RTEBTCMTYH.ALLEYWAY + RTEBTCMTYH.NUM + RTEBTCMTYH.FLOOR + RTEBTCMTYH.ROOM, RTEBTCMTYH.COMCNT, RTEBTCMTYH.AGREEDAT, " _
   '        &"RTEBTCMTYH.CONTACT, RTEBTCMTYH.CONTACTRANK, RTEBTCMTYH.COMTEL, RTEBTCMTYH.UPDEBTDAT) a, " _
   '        &"(SELECT A.COMQ1,COUNT(*) as cuscnt, SUM(CASE WHEN FINISHDAT IS NOT NULL THEN 1 ELSE 0 END) AS FINISHCNT,SUM(CASE WHEN DOCKETDAT IS NOT NULL THEN 1 ELSE 0 END) AS DOCKETCNT " _
   '        &"FROM RTEBTCMTYH A,RTEBTCUST B WHERE A.COMQ1=B.COMQ1 GROUP BY A.COMQ1) B " _
   '        &"WHERE A.COMQ1=B.COMQ1 " 
  sqlDelete="SELECT  a.comq1,a.comn,a.cutnc,a.township,a.comcnt,a.UPDEBTDAT,b.linecnt,b.applycnt,C.COMQ1,c.cuscnt,c.cancelcnt,c.dropcnt,c.finishcnt,c.docketcnt,c.diffcnt " _
           &"FROM (SELECT        RTEBTCMTYH.COMQ1, RTEBTCMTYH.COMN, " _
           &"RTCounty.CUTNC , RTEBTCMTYH.TOWNSHIP  " _
           &", RTEBTCMTYH.COMCNT, " _
           &"RTEBTCMTYH.UPDEBTDAT,SUM(CASE WHEN RTEBTCMTYLINE.COMQ1 IS NOT NULL " _
           &"THEN 1 ELSE 0 END) AS LINECNT,SUM(CASE WHEN RTEBTCMTYLINE.ADSLAPPLYDAT IS NOT NULL THEN 1 ELSE 0 END) AS APPLYCNT " _
           &"FROM RTEBTCUST INNER JOIN  RTEBTCMTYLINE ON RTEBTCUST.COMQ1 = RTEBTCMTYLINE.COMQ1 AND " _
           &"RTEBTCUST.LINEQ1 = RTEBTCMTYLINE.LINEQ1 RIGHT OUTER JOIN RTEBTCMTYH ON RTEBTCMTYLINE.COMQ1 = RTEBTCMTYH.COMQ1 LEFT OUTER JOIN " _
           &"RTCounty ON RTEBTCMTYH.CUTID = RTCounty.CUTID GROUP BY  RTEBTCMTYH.COMQ1,RTEBTCMTYH.COMN, " _
           &"RTCounty.CUTNC, RTEBTCMTYH.TOWNSHIP , RTEBTCMTYH.COMCNT, RTEBTCMTYH.AGREEDAT, " _
           &"RTEBTCMTYH.UPDEBTDAT) a, " _
           &"(SELECT RTEBTCMTYH.COMQ1,SUM(CASE WHEN RTEBTCUST.COMQ1 IS NOT NULL " _
           &"THEN 1 ELSE 0 END) AS cuscnt, SUM(CASE WHEN dropdat IS NOT NULL and docketdat is null THEN 1 ELSE 0 END) AS cancelCNT, " _
           &"SUM(CASE WHEN dropdat IS NOT NULL and docketdat is not null THEN 1 ELSE 0 END) AS dropCNT, " _
           &"SUM(CASE WHEN FINISHDAT IS NOT NULL THEN 1 ELSE 0 END) AS FINISHCNT, " _
           &"SUM(CASE WHEN DOCKETDAT IS NOT NULL THEN 1 ELSE 0 END) AS DOCKETCNT, " _
           &"SUM(CASE WHEN RTEBTCUST.COMQ1 IS NOT NULL THEN 1 ELSE 0 END) - SUM(CASE WHEN dropdat IS NOT NULL and docketdat is null THEN 1 ELSE 0 END) - " _
           &"SUM(CASE WHEN dropdat IS NOT NULL and docketdat is not null THEN 1 ELSE 0 END) - SUM(CASE WHEN FINISHDAT IS NOT NULL THEN 1 ELSE 0 END) - " _
           &"SUM(CASE WHEN DOCKETDAT IS NOT NULL THEN 1 ELSE 0 END) AS DIFFCNT " _
           &"FROM RTEBTCMTYH LEFT OUTER JOIN RTEBTCUST ON RTEBTCMTYH.COMQ1=RTEBTCUST.COMQ1 " _
           &"GROUP BY RTEBTCMTYH.COMQ1) B " _
           &"WHERE A.COMQ1 =B.COMQ1 "            
  dataTable="rtebtcmtyh"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="RTebtCmtyD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=550,scrollbars=yes"
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
  searchProg="RTebtCmtyS.asp"
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
  If searchQry="" AND searchQry2="" AND searchQry3="" Then
     searchQry=" RTEBTCmtyH.ComQ1<>0 "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  '讀取登入帳號之群組資料
  set connXX=server.CreateObject("ADODB.connection")
  set rsXX=server.CreateObject("ADODB.recordset")
  dsnxx="DSN=XXLIB"
  sqlxx="select * from usergroup where userid='" & Request.ServerVariables("LOGON_USER") & "'"
  connxx.Open dsnxx
  rsxx.Open sqlxx,connxx
  if not rsxx.EOF then
     usergroup=rsxx("group")
  else
     usergroup=""
  end if
  rsxx.Close
  connxx.Close
  set rsxx=nothing
  set connxx=nothing
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
'  if userlevel=31 or userlevel =1  or userlevel =5 then DAreaID="<>'*'"
if userlevel=31  then DAreaID="<>'*'"  
  
  '業務工程師只能讀取該工程師的社區
  'if userlevel=2 then
  '  If searchShow="全部" Then
  '  sqlList="SELECT RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN,t1no,netip, RTCounty.CUTNC, RTCmty.COMCNT, " _
  '       &"Sum( CASE custype  when '申裝戶'  THEN 1 ELSE 0 end),  " _
  '       &"Sum(CASE custype  when '深耕戶'  THEN 1 ELSE 0 end) , " _ 
  '       &"Sum(CASE custype  when ''  THEN 1 ELSE 0 end), " _
  '       &"Sum(CASE when DROPDAT is Null  THEN 0 ELSE 1 END ), " _                    
  '       &"Sum(CASE when DROPDAT is Null and rtcust.cusid is not null THEN 1 ELSE 0 END), " _            
  '       &"case when RTCmty.COMCNT = 0 then 0 else Sum(CASE when DROPDAT is Null and rtcust.cusid is not null  THEN 1 ELSE 0 END) * 100 / (RTCmty.COMCNT*1.0)  end , "  _                    
  '       &"Sum(CASE when FINISHDAT is Null and dropdat is null  and rtcust.cusid is not null THEN 1 ELSE 0 END), " _                    
  '       &"Sum(CASE when FINISHDAT is not Null and dropdat is null THEN 1 ELSE 0 END), " _                    
  '       &"RTcmty.T1PETITION,RTcmty.T1Apply  " _
  '       &"FROM RTEmployee INNER JOIN " _
  '       &"##RTCmtyGroup ON RTEmployee.CUSID = ##RTCmtyGroup.CUSID INNER JOIN " _
  '       &"RTCounty INNER JOIN " _
  '       &"RTCust RIGHT OUTER JOIN " _               
  '       &"RTCmty ON RTCUST.COMQ1 = RTCMTY.COMQ1 ON RTCounty.CUTID = RTCmty.CUTID INNER JOIN " _
  '       &"RTArea INNER JOIN " _
  '       &"RTAreaCty ON RTArea.AREAID = RTAreaCty.AREAID and rtarea.areaid" & DareaID & " ON " _
  '       &"RTCmty.CUTID = RTAreaCty.CUTID ON ##RTCmtyGroup.COMQ1 = RTCmty.COMQ1 " _
  '       &"WHERE RTArea.AREATYPE='1' AND " &searchQry &" " _         
  '       &"group by RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN,t1no,netip, RTCounty.CUTNC, " _
  '       &"RTCmty.COMCNT, RTCmty.APPLYCNT, RTCmty.T1PETITION, RTCmty.SCHDAT, " _
  '       &"RTCmty.T1APPLY " _
  '       &"ORDER BY RTCmty.COMN "
  '  Else
  '  sqlList="SELECT RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN, t1no,netip,RTCounty.CUTNC, RTCmty.COMCNT, " _
  '       &"sum( CASE custype  when '申裝戶'  THEN 1 ELSE 0 end) ,  " _
  '       &"sum(CASE custype  when '深耕戶'  THEN 1 ELSE 0 end) , " _ 
  '       &"Sum(CASE custype  when ''  THEN 1 ELSE 0 end), " _
  '       &"Sum(CASE when DROPDAT is Null  THEN 0 ELSE 1 END ), " _                    
  '       &"Sum(CASE when DROPDAT is Null and rtcust.cusid is not null THEN 1 ELSE 0 END), " _            
  '       &"case when RTCmty.COMCNT = 0 then 0 else Sum(CASE when DROPDAT is Null and rtcust.cusid is not null  THEN 1 ELSE 0 END) * 100 / (RTCmty.COMCNT*1.0)  end , "  _                    
  '       &"Sum(CASE when FINISHDAT is Null and dropdat is null  and rtcust.cusid is not null THEN 1 ELSE 0 END), " _                    
  '       &"Sum(CASE when FINISHDAT is not Null and dropdat is null THEN 1 ELSE 0 END), " _                    
  '       &"RTcmty.T1PETITION,RTcmty.T1Apply  " _
  '       &"FROM RTCounty INNER JOIN " _
  '       &"##RTCmtyGroup INNER JOIN " _
  '       &"RTCust RIGHT OUTER JOIN " _         
  '       &"RTCmty ON RTCUST.COMQ1 = RTCMTY.COMQ1 ON ##RTCmtyGroup.COMQ1 = RTCmty.COMQ1 ON " _
  '       &"RTCounty.CUTID = RTCmty.CUTID INNER JOIN " _
  '       &"RTArea INNER JOIN " _
  '       &"RTAreaCty ON RTArea.AREAID = RTAreaCty.AREAID and rtarea.areaid" & DareaID & " ON " _
  '       &"RTCmty.CUTID = RTAreaCty.CUTID INNER JOIN " _
  '       &"RTEmployee ON ##RTCmtyGroup.CUSID = RTEmployee.CUSID " _
  '       &"WHERE RTArea.AREATYPE='1' and " &searchQry & " "  _
  '       &"group by RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN,t1no,netip, RTCounty.CUTNC, " _
  '       &"RTCmty.COMCNT, RTCmty.APPLYCNT, RTCmty.T1PETITION, RTCmty.SCHDAT, " _
  '       &"RTCmty.T1APPLY " _                  
  '       &"ORDER BY RTCmty.COMN "
  '  End If
  '業務助理or客服人員
  'else
  '  If searchShow="全部" Then
  '       sqlList="SELECT * FROM (SELECT RTEBTCMTYH.COMQ1, RTEBTCMTYH.COMN, RTCounty.CUTNC, " _
  '         &"RTEBTCMTYH.TOWNSHIP, RTEBTCMTYH.COMCNT, RTEBTCMTYH.UPDEBTDAT, " _
  '         &"SUM(CASE WHEN RTEBTCMTYLINE.COMQ1 IS NOT NULL THEN 1 ELSE 0 END) AS LINECNT, " _
  '         &"SUM(CASE WHEN RTEBTCMTYLINE.ADSLAPPLYDAT IS NOT NULL THEN 1 ELSE 0 END) AS APPLYCNT " _
  '         &"from RTEBTCMTYLINE RIGHT OUTER JOIN RTEBTCMTYH ON RTEBTCMTYLINE.COMQ1 = RTEBTCMTYH.COMQ1 LEFT OUTER JOIN " _
  '         &"RTCounty ON RTEBTCMTYH.CUTID = RTCounty.CUTID WHERE " & searchqry & " GROUP BY   RTEBTCMTYH.COMQ1, " _
  '         &"RTEBTCMTYH.COMN, RTCounty.CUTNC, RTEBTCMTYH.TOWNSHIP, RTEBTCMTYH.COMCNT, " _
  '         &"RTEBTCMTYH.AGREEDAT, RTEBTCMTYH.UPDEBTDAT " & SEARCHQRY2 & " ) a, " _
  '         &"(SELECT RTEBTCMTYH.COMQ1,SUM(CASE WHEN RTEBTCUST.COMQ1 IS NOT NULL " _
  '         &"THEN 1 ELSE 0 END) AS cuscnt, SUM(CASE WHEN dropdat IS NOT NULL and docketdat is null THEN 1 ELSE 0 END) AS cancelCNT, " _
  '         &"SUM(CASE WHEN dropdat IS NOT NULL and docketdat is not null THEN 1 ELSE 0 END) AS dropCNT, " _
  '         &"SUM(CASE WHEN FINISHDAT IS NOT NULL THEN 1 ELSE 0 END) AS FINISHCNT, " _
  '         &"SUM(CASE WHEN DOCKETDAT IS NOT NULL THEN 1 ELSE 0 END) AS DOCKETCNT, " _
  '         &"SUM(CASE WHEN RTEBTCUST.COMQ1 IS NOT NULL THEN 1 ELSE 0 END) - SUM(CASE WHEN dropdat IS NOT NULL and docketdat is null THEN 1 ELSE 0 END) - " _
  '         &"SUM(CASE WHEN dropdat IS NOT NULL and docketdat is not null THEN 1 ELSE 0 END) - SUM(CASE WHEN FINISHDAT IS NOT NULL THEN 1 ELSE 0 END)  - " _
  '         &"SUM(CASE WHEN DOCKETDAT IS NOT NULL THEN 1 ELSE 0 END) AS DIFFCNT " _
  '         &"FROM RTEBTCMTYH LEFT OUTER JOIN RTEBTCUST ON RTEBTCMTYH.COMQ1=RTEBTCUST.COMQ1 " _
  '         &"GROUP BY RTEBTCMTYH.COMQ1 " & SEARCHQRY3 & " ) B " _
  '         &"WHERE A.COMQ1 =B.COMQ1 ORDER BY A.COMQ1 "       
  '  Else
         sqlList="SELECT   a.comq1, a.comn, a.cutnc, a.township, a.comcnt, a.UPDEBTDAT, b.linecnt, b.applycnt, c.comq1," _
                &"CASE WHEN c.cuscnt IS NULL THEN 0 ELSE c.cuscnt END, " _
                &"CASE WHEN c.cancelcnt IS NULL THEN 0 ELSE c.cancelcnt END, " _
                &"CASE WHEN c.dropcnt IS NULL THEN 0 ELSE c.dropcnt END, " _
                &"CASE WHEN c.finishcnt IS NULL THEN 0 ELSE c.finishcnt END, " _
                &"CASE WHEN c.docketcnt IS NULL THEN 0 ELSE c.docketcnt END, " _
                &"CASE WHEN c.diffcnt IS NULL THEN 0 ELSE c.diffcnt END " _
           &"FROM (SELECT RTEBTCMTYH.COMQ1, RTEBTCMTYH.COMN, RTCounty.CUTNC, " _
           &"RTEBTCMTYH.TOWNSHIP, RTEBTCMTYH.COMCNT, RTEBTCMTYH.UPDEBTDAT " _
           &"from RTEBTCMTYLINE RIGHT OUTER JOIN RTEBTCMTYH ON RTEBTCMTYLINE.COMQ1 = RTEBTCMTYH.COMQ1 LEFT OUTER JOIN " _
           &"RTCounty ON RTEBTCMTYH.CUTID = RTCounty.CUTID INNER JOIN RTAREATOWNSHIP ON RTEBTCMTYH.CUTID=RTAREATOWNSHIP.CUTID AND " _
           &"RTEBTCMTYH.TOWNSHIP=RTAREATOWNSHiP.TOWNSHIP WHERE " & searchqry & " AND RTAREATOWNSHIP.AREAID " & DAREAID & " GROUP BY   RTEBTCMTYH.COMQ1, " _
           &"RTEBTCMTYH.COMN, RTCounty.CUTNC, RTEBTCMTYH.TOWNSHIP, RTEBTCMTYH.COMCNT, " _
           &"RTEBTCMTYH.AGREEDAT, RTEBTCMTYH.UPDEBTDAT " & SEARCHQRY2 & " ) a LEFT OUTER JOIN " _
           &"(SELECT RTEBTCMTYH.COMQ1, RTEBTCMTYH.COMN, RTCounty.CUTNC, " _
           &"RTEBTCMTYH.TOWNSHIP, RTEBTCMTYH.COMCNT, RTEBTCMTYH.UPDEBTDAT, " _
           &"SUM(CASE WHEN RTEBTCMTYLINE.COMQ1 IS NOT NULL THEN 1 ELSE 0 END) AS LINECNT, " _
           &"SUM(CASE WHEN RTEBTCMTYLINE.ADSLAPPLYDAT IS NOT NULL THEN 1 ELSE 0 END) AS APPLYCNT " _
           &"from RTEBTCMTYLINE RIGHT OUTER JOIN RTEBTCMTYH ON RTEBTCMTYLINE.COMQ1 = RTEBTCMTYH.COMQ1 LEFT OUTER JOIN " _
           &"RTCounty ON RTEBTCMTYH.CUTID = RTCounty.CUTID  WHERE " & searchqry & " GROUP BY   RTEBTCMTYH.COMQ1, " _
           &"RTEBTCMTYH.COMN, RTCounty.CUTNC, RTEBTCMTYH.TOWNSHIP, RTEBTCMTYH.COMCNT, " _
           &"RTEBTCMTYH.AGREEDAT, RTEBTCMTYH.UPDEBTDAT " & SEARCHQRY2 & " ) B ON A.COMQ1=B.COMQ1 LEFT OUTER JOIN " _
           &"(SELECT RTEBTCMTYH.COMQ1, " _
           &"SUM(CASE WHEN RTEBTCUST.COMQ1 IS NOT NULL THEN 1 ELSE 0 END) AS cuscnt, " _
           &"SUM(CASE WHEN CANCELdat IS NOT NULL and docketdat is null THEN 1 ELSE 0 END) AS cancelCNT, " _
           &"SUM(CASE WHEN dropdat IS NOT NULL and docketdat is not null THEN 1 ELSE 0 END) AS dropCNT, " _
           &"SUM(CASE WHEN FINISHDAT IS NOT NULL AND dropdat IS NULL THEN 1 ELSE 0 END) AS FINISHCNT, " _
           &"SUM(CASE WHEN DOCKETDAT IS NOT NULL AND dropdat IS NULL THEN 1 ELSE 0 END) AS DOCKETCNT, " _
           &"SUM(CASE WHEN RTEBTCUST.COMQ1 IS NOT NULL THEN 1 ELSE 0 END) - SUM(CASE WHEN CANCELdat IS NOT NULL and docketdat is null THEN 1 ELSE 0 END) - " _
           &"SUM(CASE WHEN dropdat IS NOT NULL and docketdat is not null THEN 1 ELSE 0 END) - " _
           &"SUM(CASE WHEN DOCKETDAT IS NOT NULL AND dropdat IS NULL THEN 1 ELSE 0 END) AS DIFFCNT " _
           &"FROM RTEBTCMTYH LEFT OUTER JOIN RTEBTCUST ON RTEBTCMTYH.COMQ1=RTEBTCUST.COMQ1 " _
           &"GROUP BY RTEBTCMTYH.COMQ1 " & SEARCHQRY3 & " ) C ON A.COMQ1=C.COMQ1 " _
           &"ORDER BY A.COMQ1 "        
   ' End If  
  'end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()
End Sub
%>