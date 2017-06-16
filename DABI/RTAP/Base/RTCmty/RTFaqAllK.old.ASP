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
  Dim detailwindowFeature,rscount,searchqry1,searchqry2,searchqry3,searchqry4
  searchFirst=False
  userDefineDelete="No"
  functionOptPrompt=";;;;;;;;;;;;;;;;;;"
  keyListPageSize=0
  keyListPage=1
  colSplit=1
  searchQry=Request("searchQry")
  searchqry1=request("searchqry1")
  searchqry2=request("searchqry2")
  searchqry3=request("searchqry3")
  searchqry4=request("searchqry4")
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
<body background="/WEBAP/IMAGE/bg.gif">
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
         <input type="text" name="searchQry1" value="<%=searchQry1%>" style="display:none" readonly ID="Text3">
         <input type="text" name="searchQry2" value="<%=searchQry2%>" style="display:none" readonly ID="Text5">
         <input type="text" name="searchQry3" value="<%=searchQry3%>" style="display:none" readonly ID="Text6">
         <input type="text" name="searchQry4" value="<%=searchQry4%>" style="display:none" readonly ID="Text7"></td>
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
<%End Sub%><!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%

Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="客訴案件追蹤作業"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  'functionOptName="作廢;作廢反轉;處理措施;結案;結案反轉;列印"
  'functionOptProgram="RTFaqDropk.asp;RTFaqDropRTNk.asp;RTFaqProcessK.asp;RTFaqFinishUsrK.asp;RTFaqFinishRTNK.asp;RTFaQV.asp"

  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))  
  if userlevel="1" or userlevel="5" or userlevel="31" then 
     functionOptName="作廢;作廢反轉;處理措施;結案;結案反轉;列印"
     functionOptProgram="RTFaqDropk.asp;RTFaqDropRTNk.asp;RTFaqProcessK.asp;RTFaqFinishUsrK.asp;RTFaqFinishRTNK.asp;RTFaQV.asp"
  else
	 functionOptName=""
     functionOptProgram=""
  end if
  
  functionOptPrompt="Y;Y;N;Y;Y;Y"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="案件編號;客訴類別;none;方案種類;社區名稱;客戶名稱;RT完工日;RT報竣日;受理日期;備註說明;結案日期;作廢日期;受理人員;排除人員;經銷商 "   
  sqlDelete="SELECT RTFAQH.CASENO, RTCode.CODENC, RTFAQH.SERVICETYPE, CASE WHEN RTFAQH.COMTYPE IN ('1','4') THEN 'HB599' " _
         &"WHEN RTFAQH.COMTYPE ='2' THEN '中華399' WHEN   RTFAQH.COMTYPE ='3' THEN '速博399' " _
         &"WHEN  RTFAQH.COMTYPE ='5' THEN '東森499'  ELSE '' END,RTCMTY.COMN,RTOBJ.CUSNC,RTCUST.FINISHDAT, " _
         &"RTCUST.DOCKETDAT,RTFAQH.RCVDATE,case when   CASE WHEN RTCUST.DOCKETDAT IS NOT NULL AND RTFAQH.RCVDATE IS NOT NULL " _
         &"THEN Datediff(mi, RTCUST.finishdat, RTFAQH.RCVDATE)/1440 ELSE  '0' END > 0 and " _
         &"CASE WHEN RTCUST.DOCKETDAT IS NOT NULL AND RTFAQH.RCVDATE IS NOT NULL THEN " _
         &"Datediff(mi, RTCUST.finishdat, RTFAQH.RCVDATE)/1440 ELSE  '0' END < = 14 Then " _
         &"'裝機14天保固中，不計入維修件' else '' end ,RTFAQH.FINISHDATE,RTFAQH.DROPDATE,RTOBJ4.CUSNC," _
         &"CASE WHEN RTOBJ2.CUSNC ='' OR RTOBJ2.CUSNC IS NULL THEN RTOBJ3.CUSNC ELSE  RTOBJ2.CUSNC END " _
         &"FROM RTFAQH LEFT OUTER JOIN RTCode ON RTFAQH.SERVICETYPE = RTCode.CODE AND RTCode.KIND = 'E8' " _
         &"LEFT OUTER JOIN RTCMTY ON RTFAQH.COMQ1=RTCMTY.COMQ1 LEFT OUTER JOIN RTOBJ ON RTFAQH.CUSID=RTOBJ.CUSID " _
         &"INNER JOIN RTCUST ON RTFAQH.CUSID=RTCUST.CUSID AND RTFAQH.ENTRYNO=RTCUST.ENTRYNO LEFT OUTER JOIN RTEMPLOYEE ON RTFAQH.FINISHUSR = RTEMPLOYEE.EMPLY " _
         &"INNER JOIN RTOBJ RTOBJ2 ON RTEMPLOYEE.CUSID=RTOBJ2.CUSID LEFT OUTER JOIN RTOBJ RTOBJ3 ON RTFAQH.FINISHFAC=RTOBJ3.CUSID " _
         &"LEFT OUTER JOIN RTEMPLOYEE RTEMPLOYEE2 ON RTFAQH.RCVUSR=RTEMPLOYEE2.EMPLY INNER JOIN RTOBJ RTOBJ4 ON " _
         &"RTEMPLOYEE2.CUSID=RTOBJ4.CUSID WHERE RTFAQH.COMTYPE IN ('1','4') and " _
         &"union " _
         &"SELECT RTFAQH.CASENO, RTCode.CODENC, RTFAQH.SERVICETYPE, CASE WHEN RTFAQH.COMTYPE IN ('1','4') THEN 'HB599' " _
         &"WHEN RTFAQH.COMTYPE ='2' THEN '中華399' WHEN   RTFAQH.COMTYPE ='3' THEN '速博399' " _
         &"WHEN  RTFAQH.COMTYPE ='5' THEN '東森499'  ELSE '' END,RTCUSTADSLCMTY.COMN,RTOBJ.CUSNC," _
         &"RTCUSTADSL.FINISHDAT,RTCUSTADSL.DOCKETDAT,RTFAQH.RCVDATE,case when   CASE WHEN RTCUSTADSL.DOCKETDAT IS NOT NULL " _
         &"AND RTFAQH.RCVDATE IS NOT NULL THEN Datediff(mi, RTCUSTADSL.finishdat, RTFAQH.RCVDATE)/1440 ELSE  '0' END > 0 and " _
         &"CASE WHEN RTCUSTADSL.DOCKETDAT IS NOT NULL AND RTFAQH.RCVDATE IS NOT NULL " _
         &"THEN Datediff(mi, RTCUSTADSL.finishdat, RTFAQH.RCVDATE)/1440 ELSE  '0' END < = 14 " _
         &"then '裝機14天保固中，不計入維修件' else '' end, " _
         &"RTFAQH.FINISHDATE,RTFAQH.DROPDATE,RTOBJ4.CUSNC,CASE WHEN RTOBJ2.CUSNC ='' OR RTOBJ2.CUSNC IS NULL THEN " _
         &"RTOBJ3.CUSNC ELSE  RTOBJ2.CUSNC END " _
         &"FROM RTFAQH LEFT OUTER JOIN RTCode ON RTFAQH.SERVICETYPE = RTCode.CODE AND RTCode.KIND = 'E8' " _
         &"LEFT OUTER JOIN RTCUSTADSLCMTY ON RTFAQH.COMQ1=RTCUSTADSLCMTY.CUTYID LEFT OUTER JOIN RTOBJ ON " _
         &"RTFAQH.CUSID=RTOBJ.CUSID LEFT OUTER JOIN RTCUSTADSL ON RTFAQH.CUSID=RTCUSTADSL.CUSID " _
         &"LEFT OUTER JOIN RTEMPLOYEE ON RTFAQH.FINISHUSR = RTEMPLOYEE.EMPLY INNER JOIN RTOBJ RTOBJ2 ON " _
         &"RTEMPLOYEE.CUSID=RTOBJ2.CUSID LEFT OUTER JOIN RTOBJ RTOBJ3 ON RTFAQH.FINISHFAC=RTOBJ3.CUSID " _
         &"LEFT OUTER JOIN RTEMPLOYEE RTEMPLOYEE2 ON RTFAQH.RCVUSR=RTEMPLOYEE2.EMPLY INNER JOIN RTOBJ RTOBJ4 ON " _
         &"RTEMPLOYEE2.CUSID=RTOBJ4.CUSID WHERE RTFAQH.COMTYPE IN ('2') " _
         &"union " _
         &"SELECT         RTFAQH.CASENO, RTCode.CODENC, RTFAQH.SERVICETYPE, " _
         &"CASE WHEN RTFAQH.COMTYPE IN ('1','4') THEN 'HB599' " _
         &"WHEN RTFAQH.COMTYPE ='2' THEN '中華399' WHEN   RTFAQH.COMTYPE ='3' THEN '速博399' " _
         &"WHEN  RTFAQH.COMTYPE ='5' THEN '東森499'  ELSE '' END,RTSPARQADSLCMTY.COMN,RTOBJ.CUSNC,RTSPARQADSLCUST.FINISHDAT,RTSPARQADSLCUST.DOCKETDAT,RTFAQH.RCVDATE," _
         &"case when   CASE WHEN RTSPARQADSLCUST.DOCKETDAT IS NOT NULL AND RTFAQH.RCVDATE IS NOT NULL " _
         &"THEN Datediff(mi, RTSPARQADSLCUST.finishdat, RTFAQH.RCVDATE)/1440 ELSE  '0' END > 0 and " _
         &"CASE WHEN RTSPARQADSLCUST.DOCKETDAT IS NOT NULL AND RTFAQH.RCVDATE IS NOT NULL " _
         &"THEN Datediff(mi, RTSPARQADSLCUST.finishdat, RTFAQH.RCVDATE)/1440 ELSE  '0' END < = 14 " _
         &"then '裝機14天保固中，不計入維修件' else '' end,RTFAQH.FINISHDATE,RTFAQH.DROPDATE,RTOBJ4.CUSNC," _
         &"CASE WHEN RTOBJ2.CUSNC ='' OR RTOBJ2.CUSNC IS NULL THEN RTOBJ3.CUSNC ELSE  RTOBJ2.CUSNC END " _
         &"FROM RTFAQH LEFT OUTER JOIN RTCode ON RTFAQH.SERVICETYPE = RTCode.CODE AND RTCode.KIND = 'E8' " _
         &"LEFT OUTER JOIN RTSPARQADSLCMTY ON RTFAQH.COMQ1=RTSPARQADSLCMTY.CUTYID LEFT OUTER JOIN RTOBJ ON " _
         &"RTFAQH.CUSID=RTOBJ.CUSID LEFT OUTER JOIN RTSPARQADSLCUST ON RTFAQH.CUSID=RTSPARQADSLCUST.CUSID " _
         &"LEFT OUTER JOIN RTEMPLOYEE ON RTFAQH.FINISHUSR = RTEMPLOYEE.EMPLY INNER JOIN RTOBJ RTOBJ2 ON " _
         &"RTEMPLOYEE.CUSID=RTOBJ2.CUSID LEFT OUTER JOIN RTOBJ RTOBJ3 ON RTFAQH.FINISHFAC=RTOBJ3.CUSID " _
         &"LEFT OUTER JOIN RTEMPLOYEE RTEMPLOYEE2 ON RTFAQH.RCVUSR=RTEMPLOYEE2.EMPLY INNER JOIN RTOBJ RTOBJ4 ON " _
         &"RTEMPLOYEE2.CUSID=RTOBJ4.CUSID WHERE RTFAQH.COMTYPE IN ('3') " _
         &"union " _
         &"SELECT RTFAQH.CASENO, RTCode.CODENC, RTFAQH.SERVICETYPE, CASE WHEN RTFAQH.COMTYPE IN ('1','4') THEN 'HB599' " _
         &"WHEN RTFAQH.COMTYPE ='2' THEN '中華399' WHEN   RTFAQH.COMTYPE ='3' THEN '速博399' " _
         &"WHEN  RTFAQH.COMTYPE ='5' THEN '東森499'  ELSE '' END,RTEBTCMTYH.COMN,RTOBJ.CUSNC," _
         &"RTEBTCUST.FINISHDAT,RTEBTCUST.DOCKETDAT,RTFAQH.RCVDATE,case when   CASE WHEN RTEBTCUST.DOCKETDAT IS NOT NULL " _
         &"AND RTFAQH.RCVDATE IS NOT NULL THEN Datediff(mi, RTEBTCUST.finishdat, RTFAQH.RCVDATE)/1440 ELSE  '0' END > 0 and " _
         &"CASE WHEN RTEBTCUST.DOCKETDAT IS NOT NULL AND RTFAQH.RCVDATE IS NOT NULL " _
         &"THEN Datediff(mi, RTEBTCUST.finishdat, RTFAQH.RCVDATE)/1440 ELSE  '0' END < = 14 " _
         &"then '裝機14天保固中，不計入維修件' else '' end,RTFAQH.FINISHDATE,RTFAQH.DROPDATE,RTOBJ4.CUSNC," _
         &"CASE WHEN RTOBJ2.CUSNC ='' OR RTOBJ2.CUSNC IS NULL THEN RTOBJ3.CUSNC ELSE  RTOBJ2.CUSNC END " _
         &"FROM RTFAQH LEFT OUTER JOIN RTCode ON RTFAQH.SERVICETYPE = RTCode.CODE AND RTCode.KIND = 'E8' " _
         &"LEFT OUTER JOIN RTEBTCMTYH ON RTFAQH.COMQ1=RTEBTCMTYH.COMQ1 LEFT OUTER JOIN RTOBJ ON RTFAQH.CUSID=RTOBJ.CUSID " _
         &"LEFT OUTER JOIN RTEBTCUST ON RTFAQH.CUSID=RTEBTCUST.CUSID LEFT OUTER JOIN RTEMPLOYEE ON " _
         &"RTFAQH.FINISHUSR = RTEMPLOYEE.EMPLY INNER JOIN RTOBJ RTOBJ2 ON RTEMPLOYEE.CUSID=RTOBJ2.CUSID " _
         &"LEFT OUTER JOIN RTOBJ RTOBJ3 ON RTFAQH.FINISHFAC=RTOBJ3.CUSID LEFT OUTER JOIN RTEMPLOYEE RTEMPLOYEE2 ON " _
         &"RTFAQH.RCVUSR=RTEMPLOYEE2.EMPLY INNER JOIN RTOBJ RTOBJ4 ON RTEMPLOYEE2.CUSID=RTOBJ4.CUSID " _
         &"WHERE RTFAQH.COMTYPE IN ('5') " _
         &"ORDER BY RTFAQH.RCVDATE DESC "
  dataTable="RTFAQH"
  userDefineDelete=""
  numberOfKey=4
  dataProg="RTFAQCUSTOPTK.asp"
  datawindowFeature=""
  searchWindowFeature="width=500,height=300,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=true
  goodMorningImage="cbbn.JPG"
  colSplit=1
  keyListPageSize=25
  searchProg="RTFaQAllS.asp"
  parmkey=""
First=False
' When first time enter this keylist default query string to RTcmty.CASENO <> '*'
  searchFirst=False
  If searchQry="" Then
     searchQry=" RTFAQH.caseno<>'*' and RTFAQH.Finishdate is null and RTFAQH.DropDate Is Null " 
     searchShow="全部：未結案，未作廢資料"
  End If
  sqlList="SELECT RTFAQH.CASENO, RTCode.CODENC, RTFAQH.SERVICETYPE, CASE WHEN RTFAQH.COMTYPE IN ('1','4') THEN 'HB599' " _
         &"WHEN RTFAQH.COMTYPE ='2' THEN '中華399' WHEN   RTFAQH.COMTYPE ='3' THEN '速博399' " _
         &"WHEN  RTFAQH.COMTYPE ='5' THEN '東森499'  ELSE '' END,RTCMTY.COMN,RTOBJ.CUSNC,RTCUST.FINISHDAT, " _
         &"RTCUST.DOCKETDAT,RTFAQH.RCVDATE,case when   CASE WHEN RTCUST.DOCKETDAT IS NOT NULL AND RTFAQH.RCVDATE IS NOT NULL " _
         &"THEN Datediff(mi, RTCUST.finishdat, RTFAQH.RCVDATE)/1440 ELSE  '0' END > 0 and " _
         &"CASE WHEN RTCUST.DOCKETDAT IS NOT NULL AND RTFAQH.RCVDATE IS NOT NULL THEN " _
         &"Datediff(mi, RTCUST.finishdat, RTFAQH.RCVDATE)/1440 ELSE  '0' END < = 14 Then " _
         &"'裝機14天保固中，不計入維修件' else '' end ,RTFAQH.FINISHDATE,RTFAQH.DROPDATE,RTOBJ4.CUSNC," _
         &"CASE WHEN RTOBJ2.CUSNC ='' OR RTOBJ2.CUSNC IS NULL THEN RTOBJ3.CUSNC ELSE  RTOBJ2.CUSNC END, " _
         &"CASE WHEN RTCODE_1.CODE NOT IN ('01','02','03','04','14') THEN RTCODE_1.CODENC ELSE CASE WHEN  RTCUST.CUTID2 IN " _
         &"('01', '02', '03', '04', '21', '22') AND RTCUST.township2 NOT IN ('三峽鎮', '鶯歌鎮') THEN '台北' " _
         &"WHEN RTCUST.CUTID2 IN ('05', '06', '07', '08') OR (RTCUST.CUTID2 = '03' AND RTCUST.township2 IN " _
         &"('三峽鎮', '鶯歌鎮')) THEN '桃園' WHEN RTCUST.CUTID2 IN ('09', '10', '11', '12', '13') THEN '台中' " _
         &"WHEN RTCUST.CUTID2 IN ('14', '15', '16', '17', '18', '19', '20') THEN '高雄' ELSE '' END END " _
         &"FROM RTFAQH LEFT OUTER JOIN RTCode ON RTFAQH.SERVICETYPE = RTCode.CODE AND RTCode.KIND = 'E8' " _
         &"LEFT OUTER JOIN RTCMTY ON RTFAQH.COMQ1=RTCMTY.COMQ1 LEFT OUTER JOIN RTOBJ ON RTFAQH.CUSID=RTOBJ.CUSID " _
         &"INNER JOIN RTCUST ON RTFAQH.CUSID=RTCUST.CUSID AND RTFAQH.ENTRYNO=RTCUST.ENTRYNO LEFT OUTER JOIN RTEMPLOYEE ON RTFAQH.FINISHUSR = RTEMPLOYEE.EMPLY " _
         &"LEFT OUTER JOIN RTOBJ RTOBJ2 ON RTEMPLOYEE.CUSID=RTOBJ2.CUSID LEFT OUTER JOIN RTOBJ RTOBJ3 ON RTFAQH.FINISHFAC=RTOBJ3.CUSID " _
         &"LEFT OUTER JOIN RTEMPLOYEE RTEMPLOYEE2 ON RTFAQH.RCVUSR=RTEMPLOYEE2.EMPLY LEFT OUTER JOIN RTOBJ RTOBJ4 ON " _
         &"RTEMPLOYEE2.CUSID=RTOBJ4.CUSID left outer join rtcode RTCODE_1 on rtcmty.comtype=rtcode_1.code and rtcode_1.kind='B3' " _
         &"WHERE RTFAQH.COMTYPE IN ('1','4') and " & searchqry & " " & searchqry1 & " " _
         &"union " _
         &"SELECT RTFAQH.CASENO, RTCode.CODENC, RTFAQH.SERVICETYPE, CASE WHEN RTFAQH.COMTYPE IN ('1','4') THEN 'HB599' " _
         &"WHEN RTFAQH.COMTYPE ='2' THEN '中華399' WHEN   RTFAQH.COMTYPE ='3' THEN '速博399' " _
         &"WHEN  RTFAQH.COMTYPE ='5' THEN '東森499'  ELSE '' END,RTCUSTADSLCMTY.COMN,RTOBJ.CUSNC," _
         &"RTCUSTADSL.FINISHDAT,RTCUSTADSL.DOCKETDAT,RTFAQH.RCVDATE,case when   CASE WHEN RTCUSTADSL.DOCKETDAT IS NOT NULL " _
         &"AND RTFAQH.RCVDATE IS NOT NULL THEN Datediff(mi, RTCUSTADSL.finishdat, RTFAQH.RCVDATE)/1440 ELSE  '0' END > 0 and " _
         &"CASE WHEN RTCUSTADSL.DOCKETDAT IS NOT NULL AND RTFAQH.RCVDATE IS NOT NULL " _
         &"THEN Datediff(mi, RTCUSTADSL.finishdat, RTFAQH.RCVDATE)/1440 ELSE  '0' END < = 14 " _
         &"then '裝機14天保固中，不計入維修件' else '' end, " _
         &"RTFAQH.FINISHDATE,RTFAQH.DROPDATE,RTOBJ4.CUSNC,CASE WHEN RTOBJ2.CUSNC ='' OR RTOBJ2.CUSNC IS NULL THEN " _
         &"RTOBJ3.CUSNC ELSE  RTOBJ2.CUSNC END, " _
         &"CASE WHEN RTCODE_1.CODE NOT IN ('01','02','03','04','14') THEN RTCODE_1.CODENC ELSE CASE WHEN  RTCUSTADSL.CUTID1 IN " _
         &"('01', '02', '03', '04', '21', '22') AND RTCUSTADSL.township1 NOT IN ('三峽鎮', '鶯歌鎮') THEN '台北' " _
         &"WHEN RTCUSTADSL.CUTID1 IN ('05', '06', '07', '08') OR (RTCUSTADSL.CUTID1 = '03' AND RTCUSTADSL.township1 IN " _
         &"('三峽鎮', '鶯歌鎮')) THEN '桃園' WHEN RTCUSTADSL.CUTID1 IN ('09', '10', '11', '12', '13') THEN '台中' " _
         &"WHEN RTCUSTADSL.CUTID1 IN ('14', '15', '16', '17', '18', '19', '20') THEN '高雄' ELSE '' END END " _
         &"FROM RTFAQH LEFT OUTER JOIN RTCode ON RTFAQH.SERVICETYPE = RTCode.CODE AND RTCode.KIND = 'E8' " _
         &"LEFT OUTER JOIN RTCUSTADSLCMTY ON RTFAQH.COMQ1=RTCUSTADSLCMTY.CUTYID LEFT OUTER JOIN RTOBJ ON " _
         &"RTFAQH.CUSID=RTOBJ.CUSID INNER JOIN RTCUSTADSL ON RTFAQH.CUSID=RTCUSTADSL.CUSID AND RTFAQH.ENTRYNO=RTCUSTADSL.ENTRYNO " _
         &"LEFT OUTER JOIN RTEMPLOYEE ON RTFAQH.FINISHUSR = RTEMPLOYEE.EMPLY LEFT OUTER JOIN RTOBJ RTOBJ2 ON " _
         &"RTEMPLOYEE.CUSID=RTOBJ2.CUSID LEFT OUTER JOIN RTOBJ RTOBJ3 ON RTFAQH.FINISHFAC=RTOBJ3.CUSID " _
         &"LEFT OUTER JOIN RTEMPLOYEE RTEMPLOYEE2 ON RTFAQH.RCVUSR=RTEMPLOYEE2.EMPLY LEFT OUTER JOIN RTOBJ RTOBJ4 ON " _
         &"RTEMPLOYEE2.CUSID=RTOBJ4.CUSID left outer join rtcode RTCODE_1 on rtCUSTADSLcmty.comtype=rtcode_1.code and rtcode_1.kind='B3' " _
         &"WHERE RTFAQH.COMTYPE IN ('2')  and " & searchqry & " " & searchqry2 & " " _
         &"union " _
         &"SELECT         RTFAQH.CASENO, RTCode.CODENC, RTFAQH.SERVICETYPE, " _
         &"CASE WHEN RTFAQH.COMTYPE IN ('1','4') THEN 'HB599' " _
         &"WHEN RTFAQH.COMTYPE ='2' THEN '中華399' WHEN   RTFAQH.COMTYPE ='3' THEN '速博399' " _
         &"WHEN  RTFAQH.COMTYPE ='5' THEN '東森499'  ELSE '' END,RTSPARQADSLCMTY.COMN,RTOBJ.CUSNC,RTSPARQADSLCUST.FINISHDAT,RTSPARQADSLCUST.DOCKETDAT,RTFAQH.RCVDATE," _
         &"case when   CASE WHEN RTSPARQADSLCUST.DOCKETDAT IS NOT NULL AND RTFAQH.RCVDATE IS NOT NULL " _
         &"THEN Datediff(mi, RTSPARQADSLCUST.finishdat, RTFAQH.RCVDATE)/1440 ELSE  '0' END > 0 and " _
         &"CASE WHEN RTSPARQADSLCUST.DOCKETDAT IS NOT NULL AND RTFAQH.RCVDATE IS NOT NULL " _
         &"THEN Datediff(mi, RTSPARQADSLCUST.finishdat, RTFAQH.RCVDATE)/1440 ELSE  '0' END < = 14 " _
         &"then '裝機14天保固中，不計入維修件' else '' end,RTFAQH.FINISHDATE,RTFAQH.DROPDATE,RTOBJ4.CUSNC," _
         &"CASE WHEN RTOBJ2.CUSNC ='' OR RTOBJ2.CUSNC IS NULL THEN RTOBJ3.CUSNC ELSE  RTOBJ2.CUSNC END, " _
         &"CASE WHEN RTOBJ5.SHORTNC <> '' THEN RTOBJ5.SHORTNC ELSE CASE WHEN  RTSPARQADSLCUST.CUTID1 IN " _
         &"('01', '02', '03', '04', '21', '22') AND RTSPARQADSLCUST.township1 NOT IN ('三峽鎮', '鶯歌鎮') THEN '台北' " _
         &"WHEN RTSPARQADSLCUST.CUTID1 IN ('05', '06', '07', '08') OR (RTSPARQADSLCUST.CUTID1 = '03' AND RTSPARQADSLCUST.township1 IN " _
         &"('三峽鎮', '鶯歌鎮')) THEN '桃園' WHEN RTSPARQADSLCUST.CUTID1 IN ('09', '10', '11', '12', '13') THEN '台中' " _
         &"WHEN RTSPARQADSLCUST.CUTID1 IN ('14', '15', '16', '17', '18', '19', '20') THEN '高雄' ELSE '' END END " _
         &"FROM RTFAQH LEFT OUTER JOIN RTCode ON RTFAQH.SERVICETYPE = RTCode.CODE AND RTCode.KIND = 'E8' " _
         &"LEFT OUTER JOIN RTSPARQADSLCMTY ON RTFAQH.COMQ1=RTSPARQADSLCMTY.CUTYID LEFT OUTER JOIN RTOBJ ON " _
         &"RTFAQH.CUSID=RTOBJ.CUSID INNER JOIN RTSPARQADSLCUST ON RTFAQH.CUSID=RTSPARQADSLCUST.CUSID AND RTFAQH.ENTRYNO=RTSPARQADSLCUST.ENTRYNO " _
         &"LEFT OUTER JOIN RTEMPLOYEE ON RTFAQH.FINISHUSR = RTEMPLOYEE.EMPLY LEFT OUTER JOIN RTOBJ RTOBJ2 ON " _
         &"RTEMPLOYEE.CUSID=RTOBJ2.CUSID LEFT OUTER JOIN RTOBJ RTOBJ3 ON RTFAQH.FINISHFAC=RTOBJ3.CUSID " _
         &"LEFT OUTER JOIN RTEMPLOYEE RTEMPLOYEE2 ON RTFAQH.RCVUSR=RTEMPLOYEE2.EMPLY LEFT OUTER JOIN RTOBJ RTOBJ4 ON " _
         &"RTEMPLOYEE2.CUSID=RTOBJ4.CUSID LEFT OUTER JOIN RTOBJ RTOBJ5 ON RTSPARQADSLCMTY.CONSIGNEE=RTOBJ5.CUSID " _
         &"WHERE RTFAQH.COMTYPE IN ('3')  and " & searchqry & " " & searchqry3 & " " _
         &"union " _
         &"SELECT RTFAQH.CASENO, RTCode.CODENC, RTFAQH.SERVICETYPE, CASE WHEN RTFAQH.COMTYPE IN ('1','4') THEN 'HB599' " _
         &"WHEN RTFAQH.COMTYPE ='2' THEN '中華399' WHEN   RTFAQH.COMTYPE ='3' THEN '速博399' " _
         &"WHEN  RTFAQH.COMTYPE ='5' THEN '東森499'  ELSE '' END,RTEBTCMTYH.COMN,RTEBTCUST.CUSNC," _
         &"RTEBTCUST.FINISHDAT,RTEBTCUST.DOCKETDAT,RTFAQH.RCVDATE,case when   CASE WHEN RTEBTCUST.DOCKETDAT IS NOT NULL " _
         &"AND RTFAQH.RCVDATE IS NOT NULL THEN Datediff(mi, RTEBTCUST.finishdat, RTFAQH.RCVDATE)/1440 ELSE  '0' END > 0 and " _
         &"CASE WHEN RTEBTCUST.DOCKETDAT IS NOT NULL AND RTFAQH.RCVDATE IS NOT NULL " _
         &"THEN Datediff(mi, RTEBTCUST.finishdat, RTFAQH.RCVDATE)/1440 ELSE  '0' END < = 14 " _
         &"then '裝機14天保固中，不計入維修件' else '' end,RTFAQH.FINISHDATE,RTFAQH.DROPDATE,RTOBJ4.CUSNC," _
         &"CASE WHEN RTOBJ2.CUSNC ='' OR RTOBJ2.CUSNC IS NULL THEN RTOBJ3.CUSNC ELSE  RTOBJ2.CUSNC END, " _
         &"CASE WHEN RTOBJ5.SHORTNC <> '' THEN RTOBJ5.SHORTNC ELSE " _
         &"CASE WHEN  RTEBTCUST.CUTID1 IN " _
         &"('01', '02', '03', '04', '21', '22') AND RTEBTCUST.township1 NOT IN ('三峽鎮', '鶯歌鎮') THEN '台北' " _
         &"WHEN RTEBTCUST.CUTID1 IN ('05', '06', '07', '08') OR (RTEBTCUST.CUTID1 = '03' AND RTEBTCUST.township1 IN " _
         &"('三峽鎮', '鶯歌鎮')) THEN '桃園' WHEN RTEBTCUST.CUTID1 IN ('09', '10', '11', '12', '13') THEN '台中' " _
         &"WHEN RTEBTCUST.CUTID1 IN ('14', '15', '16', '17', '18', '19', '20') THEN '高雄' ELSE '' END END " _
         &"FROM RTFAQH LEFT OUTER JOIN RTCode ON RTFAQH.SERVICETYPE = RTCode.CODE AND RTCode.KIND = 'E8' " _
         &"LEFT OUTER JOIN RTEBTCMTYH ON RTFAQH.COMQ1=RTEBTCMTYH.COMQ1 " _
         &"INNER JOIN RTEBTCUST ON RTFAQH.CUSID=RTEBTCUST.CUSID LEFT OUTER JOIN RTEMPLOYEE ON " _
         &"RTFAQH.FINISHUSR = RTEMPLOYEE.EMPLY LEFT OUTER JOIN RTOBJ RTOBJ2 ON RTEMPLOYEE.CUSID=RTOBJ2.CUSID " _
         &"LEFT OUTER JOIN RTOBJ RTOBJ3 ON RTFAQH.FINISHFAC=RTOBJ3.CUSID LEFT OUTER JOIN RTEMPLOYEE RTEMPLOYEE2 ON " _
         &"RTFAQH.RCVUSR=RTEMPLOYEE2.EMPLY LEFT OUTER JOIN RTOBJ RTOBJ4 ON RTEMPLOYEE2.CUSID=RTOBJ4.CUSID " _
         &"LEFT OUTER JOIN RTEBTCMTYLINE ON RTEBTCUST.COMQ1=RTEBTCMTYLINE.COMQ1 AND RTEBTCUST.LINEQ1=RTEBTCMTYLINE.LINEQ1 " _
         &"LEFT OUTER JOIN RTOBJ RTOBJ5 ON RTEBTCMTYLINE.CONSIGNEE=RTOBJ5.CUSID " _
         &"WHERE RTFAQH.COMTYPE IN ('5')  and " & searchqry & " " & searchqry4 & " " _
         &"ORDER BY RTFAQH.RCVDATE DESC "
 'Response.Write "SQL=" & SQLlist
End Sub

%>
