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
       Set diagWindow=Window.Open(prog,"_new",strFeature)
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
<!-- #include virtual="/webap/include/htmlprotect.inc" -->
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
  system="ET-City管理系統"
  title="ET-City社區基本資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="主線維護;社區用戶;設備查詢;合約維護"
  functionOptProgram="rtLESSORcmtylineK.asp;rtLESSORCUSTK1.ASP;rtLESSORcmtyHARDWAREK.asp;RTLessorCmtyContractk.asp"
  functionOptPrompt="N;N;N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="社區序號;社區名稱;縣市;鄉鎮;規模戶數;申請線數;開通線數;申請;撤銷;退租;完工;報竣;計費"
 ' sqlDelete="SELECT * FROM (SELECT        RTLessorCmtyH.COMQ1, RTLessorCmtyH.COMN, " _
 '          &"RTCounty.CUTNC + RTLessorCmtyH.TOWNSHIP + RTLessorCmtyH.VILLAGE + RTLessorCmtyH.NEIGHBOR " _
 '          &"+ RTLessorCmtyH.STREET + RTLessorCmtyH.SEC + RTLessorCmtyH.LANE + RTLessorCmtyH.TOWN " _
 '          &"+ RTLessorCmtyH.ALLEYWAY + RTLessorCmtyH.NUM + RTLessorCmtyH.FLOOR + RTLessorCmtyH.ROOM " _
 '          &"AS raddr, RTLessorCmtyH.COMCNT, " _
 ''          &"RTLessorCmtyH.CONTACT, RTLessorCmtyH.CONTACTRANK, " _
  '         &"RTLessorCmtyH.COMTEL, RTLessorCmtyH.UPDEBTDAT,COUNT(*) as linecnt,SUM(CASE WHEN RTLessorCmtyLine.ADSLAPPLYDAT IS NOT NULL THEN 1 ELSE 0 END) AS APPLYCNT " _
  '         &"FROM RTLessorCust INNER JOIN  RTLessorCmtyLine ON RTLessorCust.COMQ1 = RTLessorCmtyLine.COMQ1 AND " _
  '         &"RTLessorCust.LINEQ1 = RTLessorCmtyLine.LINEQ1 RIGHT OUTER JOIN RTLessorCmtyH ON RTLessorCmtyLine.COMQ1 = RTLessorCmtyH.COMQ1 LEFT OUTER JOIN " _
  '         &"RTCounty ON RTLessorCmtyH.CUTID = RTCounty.CUTID GROUP BY  RTLessorCmtyH.COMQ1,RTLessorCmtyH.COMN, " _
  ''         &"RTCounty.CUTNC + RTLessorCmtyH.TOWNSHIP + RTLessorCmtyH.VILLAGE + RTLessorCmtyH.NEIGHBOR " _
  '         &"+ RTLessorCmtyH.STREET + RTLessorCmtyH.SEC + RTLessorCmtyH.LANE + RTLessorCmtyH.TOWN " _
   '        &"+ RTLessorCmtyH.ALLEYWAY + RTLessorCmtyH.NUM + RTLessorCmtyH.FLOOR + RTLessorCmtyH.ROOM, RTLessorCmtyH.COMCNT, RTLessorCmtyH.AGREEDAT, " _
   '        &"RTLessorCmtyH.CONTACT, RTLessorCmtyH.CONTACTRANK, RTLessorCmtyH.COMTEL, RTLessorCmtyH.UPDEBTDAT) a, " _
   '        &"(SELECT A.COMQ1,COUNT(*) as cuscnt, SUM(CASE WHEN FINISHDAT IS NOT NULL THEN 1 ELSE 0 END) AS FINISHCNT,SUM(CASE WHEN DOCKETDAT IS NOT NULL THEN 1 ELSE 0 END) AS DOCKETCNT " _
   '        &"FROM RTLessorCmtyH A,RTLessorCust B WHERE A.COMQ1=B.COMQ1 GROUP BY A.COMQ1) B " _
   '        &"WHERE A.COMQ1=B.COMQ1 " 
  sqlDelete="SELECT  a.comq1,a.comn,a.cutnc,a.township,a.comcnt,b.linecnt,b.applycnt,c.cuscnt,c.cancelcnt,c.dropcnt,c.finishcnt,c.docketcnt,c.diffcnt " _
           &"FROM (SELECT        RTLessorCmtyH.COMQ1, RTLessorCmtyH.COMN, " _
           &"RTCounty.CUTNC , RTLessorCmtyH.TOWNSHIP  " _
           &", RTLessorCmtyH.COMCNT, " _
           &"RTLessorCmtyH.UPDEBTDAT,SUM(CASE WHEN RTLessorCmtyLine.COMQ1 IS NOT NULL " _
           &"THEN 1 ELSE 0 END) AS LINECNT,SUM(CASE WHEN RTLessorCmtyLine.ADSLAPPLYDAT IS NOT NULL THEN 1 ELSE 0 END) AS APPLYCNT " _
           &"FROM RTLessorCust INNER JOIN  RTLessorCmtyLine ON RTLessorCust.COMQ1 = RTLessorCmtyLine.COMQ1 AND " _
           &"RTLessorCust.LINEQ1 = RTLessorCmtyLine.LINEQ1 RIGHT OUTER JOIN RTLessorCmtyH ON RTLessorCmtyLine.COMQ1 = RTLessorCmtyH.COMQ1 LEFT OUTER JOIN " _
           &"RTCounty ON RTLessorCmtyH.CUTID = RTCounty.CUTID GROUP BY  RTLessorCmtyH.COMQ1,RTLessorCmtyH.COMN, " _
           &"RTCounty.CUTNC, RTLessorCmtyH.TOWNSHIP , RTLessorCmtyH.COMCNT, RTLessorCmtyH.AGREEDAT, " _
           &"RTLessorCmtyH.UPDEBTDAT) a, " _
           &"(SELECT RTLessorCmtyH.COMQ1,SUM(CASE WHEN RTLessorCust.COMQ1 IS NOT NULL " _
           &"THEN 1 ELSE 0 END) AS cuscnt, SUM(CASE WHEN dropdat IS NOT NULL and docketdat is null THEN 1 ELSE 0 END) AS cancelCNT, " _
           &"SUM(CASE WHEN dropdat IS NOT NULL and docketdat is not null THEN 1 ELSE 0 END) AS dropCNT, " _
           &"SUM(CASE WHEN FINISHDAT IS NOT NULL THEN 1 ELSE 0 END) AS FINISHCNT, " _
           &"SUM(CASE WHEN DOCKETDAT IS NOT NULL THEN 1 ELSE 0 END) AS DOCKETCNT, " _
           &"SUM(CASE WHEN RTLessorCust.COMQ1 IS NOT NULL THEN 1 ELSE 0 END) - SUM(CASE WHEN dropdat IS NOT NULL and docketdat is null THEN 1 ELSE 0 END) - " _
           &"SUM(CASE WHEN dropdat IS NOT NULL and docketdat is not null THEN 1 ELSE 0 END) - SUM(CASE WHEN FINISHDAT IS NOT NULL THEN 1 ELSE 0 END) - " _
           &"SUM(CASE WHEN DOCKETDAT IS NOT NULL THEN 1 ELSE 0 END) AS DIFFCNT " _
           &"FROM RTLessorCmtyH LEFT OUTER JOIN RTLessorCust ON RTLessorCmtyH.COMQ1=RTLessorCust.COMQ1 " _
           &"GROUP BY RTLessorCmtyH.COMQ1) B " _
           &"WHERE A.COMQ1 =B.COMQ1 "            
  dataTable="RTLessorCmtyH"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="RTLessorCmtyD.asp"
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
  searchProg="RTLessorCmtyS.asp"
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
     'searchQry=" RTLessorCmtyH.ComQ1<>0 "
     'searchShow="全部"
	'修改A
    if ARYPARMKEY(0) ="" then 
	    searchQry=" a.ComQ1<>0 "
		searchShow="全部"
	else
		searchQry=" a.ComQ1=" & aryparmkey(0)
		searchShow="社區序號︰"& aryparmkey(0)
	end if		
     
  ELSE
     SEARCHFIRST=FALSE
  End If

  '轄區限制
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  'userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  set connXX=server.CreateObject("ADODB.connection")
  set rsXX=server.CreateObject("ADODB.recordset")
  dsnxx="DSN=RTLIB"
  sqlxx="select * from RTAreaSales where cusid='" & Emply & "' and areaid ='D0' "
  connxx.Open dsnxx
  rsxx.Open sqlxx,connxx
  if not rsxx.EOF then
     limitjoin	=" inner "
     limitemply	=" and b.salesid ='" & Emply & "' "
  else
     limitjoin 	=" left outer "
     limitemply =" " 
  end if
  rsxx.Close

  connxx.Close
  set rsxx=nothing
  set connxx=nothing
  '-------------------------------------------------------------------------------------------
     sqlList="select a.comq1, a.comn, d.cutnc, a.township, a.comcnt, isnull(b.linecnt,0) as linecnt, isnull(b.applycnt, 0) as applycnt, " &_
			"isnull(c.cuscnt,0), isnull(c.cancelcnt,0), isnull(c.dropcnt,0), isnull(c.finishcnt,0), isnull(c.docketcnt,0), isnull(c.billingcnt,0) " &_
			"from	RTLessorCmtyH a " &_
			"left outer join RTCounty d on a.cutid = d.cutid " &_
			"left outer join (select x.comq1, y.salesid, count(*) as linecnt, sum(case when y.hardwaredat is not null then 1 else 0 end) as applycnt " &_
			"from RTLessorCmtyH x "& limitjoin &" join RTLessorCmtyLine y on x.comq1 = y.comq1 " &_
			"group by x.comq1, y.salesid "& searchqry2 &") b on b.comq1 = a.comq1 " &_
			"left outer join (select x.comq1, count(*) as cuscnt, sum(case when z.canceldat is not null then 1 else 0 end) as cancelcnt, " &_
			"sum(case when z.dropdat is not null and z.docketdat is not null then 1 else 0 end) as dropcnt, " &_
			"sum(case when z.dropdat is null and z.finishdat is not null then 1 else 0 end) as finishcnt, " &_
			"sum(case when z.dropdat is null and z.docketdat is not null then 1 else 0 end) as docketcnt, " &_
			"sum(case when z.dropdat is null and (z.strbillingdat is not null or z.newbillingdat is not null) then 1 else 0 end) as billingcnt " &_
			"from RTLessorCmtyH x " &_
			"inner join RTLessorCmtyLine y on x.comq1 = y.comq1 " &_
			"inner join RTLessorCust z on y.comq1 = z.comq1 and y.lineq1 = z.lineq1 " &_
			" group by x.comq1 "& searchqry3 &") c on c.comq1 = a.comq1 " &_
			"where " & searchqry & limitemply &_
			"order by 	a.comq1 "
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()
End Sub
%>