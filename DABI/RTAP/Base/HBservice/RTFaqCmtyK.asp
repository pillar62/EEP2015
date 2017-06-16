<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
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
  searchFirst=True
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
  ' 產生adsl+hb社區暫存檔(by user)
  '91/09/11改由PROCEDURE排程產生且取消工號識別
     logonid=session("userid")
  '   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
  '   xyz=split(rtnvalue,";") 
  '   If not goodMorning Then
  '      Set connXX=Server.CreateObject("ADODB.Connection")  
  '      DSNXX="DSN=RtLib"
  '      connXX.Open DSNXX
  '      strSP="USP_HBADSLcmty " & xyz(0)
  '      Set ObjRS = connXX.Execute(strSP)      
  '      connXX.Close
  '      SET CONNXX=NOTHING  
  '   end if
 '--------------------------------------  
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
<!-- #include virtual="/WebUtilityV4/DBAUDI/deleteDialogue.inc" -->
<script language="vbscript">
Sub runAUDI(accessMode,key)
    Dim prog,strFeature,msg
    prog="<%=dataProg%>"
    progary=split(prog,";")
    keyary=split(key,";")
    If prog="None" Then
    Else
       Randomize  
     '  MSGBOX "COMTYPE=" & KEYARY(1)
       IF keyary(2)="3" then
          prog=progary(0) & "?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
       elseIF keyary(2)="6" then
          prog=progary(1) & "?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
       elseIF keyary(2)="7" then
          prog=progary(2) & "?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
       elseIF keyary(2)="8" then
          prog=progary(3) & "?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
       elseIF keyary(2)="9" then
          prog=progary(4) & "?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
       elseIF keyary(2)="A" then
          prog=progary(5) & "?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
       elseIF keyary(2)="B" then
          prog=progary(6) & "?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
       end if
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
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="ADSL+Hi-Building社區資料查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;" & V(3)
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="客  戶;整線派工;主機客訴;設備管理;Sonet主線流量;Sonet網管工具(ping)"
  functionOptProgram="RTFaqCustK.asp;/webap/rtap/base/hbCmtyarrangesndwork/hbCmtyarrangesndworkk.asp;/webap/RTAP/base/rthbadslcmty/RTFAQK.ASP;/webap/RTAP/base/rthbadslcmty/RTCMTYEquipmentk.asp;/WebAP/RTAP/Base/HBservice/RTFaqCmtyMrtg.asp;/WebAP/RTAP/Base/HBservice/RTFaqCmtyTool.asp"
  functionOPTprompt="N;N;N;N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;序號;社區名稱;社區附掛;規模戶數;用戶數;方案別;開通日;撤線日;作廢日;經銷商;工程師;建置書;Reset電話;整線派工;整線完成;審核完成"
  sqlDelete="SELECT HBADSLCMTY.comq1,hbadslcmty.lineq1, HBADSLCMTY.comtype, HBADSLCMTY.comq1, " _
           &"HBADSLCMTY.comn, HBADSLCMTY.LINETEL, HBADSLCMTY.comcnt, HBADSLCMTY.usercnt, " _
		   &"CASE HBADSLCMTY.comtype when '3' then '速博399' when '6' then '速博499' when '7' then 'AVS-City' when '8' then 'ET-City' ELSE '???' END AS comtype, " _           
           &"HBADSLCMTY.T1APPLYDAT,HBADSLCMTY.rcomdrop, HBADSLCMTY.canceldat, HBADSLCMTY.GROUPNC, " _
           &"HBADSLCMTY.leader, HBADSLCMTY.comagree, " _
		   &"CASE HBADSLCMTY.comtype when '3' then RTReset.Tel when '6' then RTReset1.Tel when '7' then RTReset7.Tel when '8' then RTReset8.Tel ELSE '' END, " _
           &"CASE WHEN hbcmtyarrangesndwork.prtno IS NOT NULL AND hbcmtyarrangesndwork.dropdat IS NULL " _
           &"THEN 'Y' ELSE '' END, CASE WHEN hbcmtyarrangesndwork.prtno IS NOT NULL AND " _
           &"hbcmtyarrangesndwork.dropdat IS NULL AND hbcmtyarrangesndwork.closedat IS NOT NULL THEN 'Y' ELSE '' END, CASE WHEN hbcmtyarrangesndwork.prtno IS NOT NULL AND " _
           &"hbcmtyarrangesndwork.dropdat IS NULL AND hbcmtyarrangesndwork.AUDITdat IS NOT NULL THEN 'Y' ELSE '' END " _
           &"FROM  HBADSLCMTY LEFT OUTER JOIN HBCMTYARRANGESNDWORK ON HBADSLCMTY.comq1 = HBCMTYARRANGESNDWORK.COMQ1 AND " _
           &"HBADSLCMTY.comtype = HBCMTYARRANGESNDWORK.COMTYPE LEFT OUTER JOIN RTCMTYMSG ON HBADSLCMTY.comq1 = RTCMTYMSG.COMQ1 AND " _
           &"HBADSLCMTY.comtype = RTCMTYMSG.KIND " _
		   &"Left outer join RTReset on RTReset.comq1 = HBADSLCMTY.comq1 and RTReset.cmtytype ='03' and RTReset.canceldat is null and HBADSLCMTY.comtype='3' " _
		   &"Left outer join RTReset RTReset1 on RTReset1.comq1 = HBADSLCMTY.comq1 and RTReset1.lineq1 = HBADSLCMTY.lineq1 and RTReset1.cmtytype ='05' and RTReset1.canceldat is null and HBADSLCMTY.comtype='6' " _
		   &"Left outer join RTReset RTReset7 on RTReset7.comq1 = HBADSLCMTY.comq1 and RTReset7.lineq1 = HBADSLCMTY.lineq1 and RTReset7.cmtytype ='07' and RTReset7.canceldat is null and HBADSLCMTY.comtype='7' " _
		   &"Left outer join RTReset RTReset8 on RTReset8.comq1 = HBADSLCMTY.comq1 and RTReset8.lineq1 = HBADSLCMTY.lineq1 and RTReset8.cmtytype ='06' and RTReset8.canceldat is null and HBADSLCMTY.comtype='8' " _
           &"WHERE hbadslcmty.comn <>'' " &  searchqry & " " _
           &"GROUP BY  HBADSLCMTY.comq1,hbadslcmty.lineq1, HBADSLCMTY.comtype, HBADSLCMTY.comq1, " _
           &"HBADSLCMTY.comn, HBADSLCMTY.LINETEL, HBADSLCMTY.comcnt, HBADSLCMTY.usercnt, " _
           &"HBADSLCMTY.comtype, HBADSLCMTY.T1APPLYDAT,HBADSLCMTY.rcomdrop, " _
		   &"CASE HBADSLCMTY.comtype when '3' then RTReset.Tel when '6' then RTReset1.Tel ELSE '' END, " _
           &"HBADSLCMTY.GROUPNC, HBADSLCMTY.leader, " _
           &"HBADSLCMTY.comagree,HBADSLCMTY.contract, HBADSLCMTY.significantCNT,CASE WHEN hbcmtyarrangesndwork.prtno IS NOT NULL AND " _
           &"hbcmtyarrangesndwork.dropdat IS NULL  THEN 'Y' ELSE '' END, " _
           &"CASE WHEN hbcmtyarrangesndwork.prtno IS NOT NULL AND hbcmtyarrangesndwork.dropdat IS NULL AND " _
           &"hbcmtyarrangesndwork.closedat IS NOT NULL THEN 'Y' ELSE '' END, CASE WHEN hbcmtyarrangesndwork.prtno IS NOT NULL AND " _
           &"hbcmtyarrangesndwork.dropdat IS NULL AND hbcmtyarrangesndwork.AUDITdat IS NOT NULL THEN 'Y' ELSE '' END " _
           &"order by comn "
  dataTable="HBADSLCMTY"
  userDefineDelete="Yes"
  extTable=""
  numberOfKey=3
  dataProg="/webap/rtap/base/rtsparqadslcmty/rtcmtyd.asp;/webap/rtap/base/rtsparq499cmty/RTSPARQ499cmtyLINED.asp;/webap/rtap/base/RTLessorAvsCmty/RTLessorAvsCmtyLineD.asp;/webap/rtap/base/RTLessorCmty/RTLessorCmtyLineD.asp;/webap/rtap/base/RTPrjCmty/RTPrjCmtyLineD.asp;/webap/rtap/base/RTSonetCmty/RTSonetCmtyLineD.asp;/webap/rtap/base/RTfareastCmty/RTfareastCmtyLineD.asp"
  datawindowFeature=""
  searchWindowFeature="width=700,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=False
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=30
  searchProg="RTFaqCmtyS.asp"
  searchFirst=TRUE
  If searchQry="" Then
     searchQry=" and a.comq1=0 "
     searchShow=""
  ELSE
     searchFirst=False
  End If
  logonid=session("userid")
  Call SrGetEmployeeRef(Rtnvalue,1,logonid)
  xyz=split(rtnvalue,";") 

  sqllist="SELECT a.comq1, a.lineq1, a.comtype, convert(varchar(6),a.comq1)+ case a.lineq1 when 0 then '' else '-' +convert(varchar(2),a.lineq1) end, " &_
		  "a.comn, a.LINETEL, a.comcnt, a.usercnt, isnull(g.codenc, e.codenc), a.T1APPLYDAT,a.rcomdrop, a.canceldat, a.GROUPNC, a.leader, " &_
		  "a.comagree, CASE a.comtype when '3' then c.Tel when '6' then d.Tel when '7' then i.Tel when '8' then h.Tel ELSE '' END, " &_ 
		  "CASE WHEN b.prtno IS NOT NULL AND b.dropdat IS NULL THEN 'Y' ELSE '' END, CASE WHEN b.prtno IS NOT NULL AND b.dropdat IS NULL AND b.closedat IS NOT NULL THEN 'Y' ELSE '' END, CASE WHEN b.prtno IS NOT NULL AND b.dropdat IS NULL AND b.AUDITdat IS NOT NULL THEN 'Y' ELSE '' END " &_ 
		  "FROM HBADSLCMTY a " &_ 
		  "Left outer join HBCMTYARRANGESNDWORK b ON a.comq1 = b.COMQ1 AND a.comtype = b.COMTYPE " &_ 
		  "Left outer join RTReset c on c.comq1 = a.comq1 and c.cmtytype ='03' and c.canceldat is null and a.comtype='3' " &_ 
		  "Left outer join RTReset d on d.comq1 = a.comq1 and d.lineq1 = a.lineq1 and d.cmtytype ='05' and d.canceldat is null and a.comtype='6' " &_ 
		  "Left outer join RTReset h on h.comq1 = a.comq1 and h.lineq1 = a.lineq1 and h.cmtytype ='06' and h.canceldat is null and a.comtype='8' " &_ 
		  "Left outer join RTReset i on i.comq1 = a.comq1 and i.lineq1 = a.lineq1 and i.cmtytype ='07' and i.canceldat is null and a.comtype='7' " &_ 
		  "Left outer join RTCode e on e.code = a.comtype and e.kind ='P5' " &_ 
		  "Left outer join RTSparq499CmtyLine f inner join RTCode g on g.code = f.connecttype and g.kind ='G5' " &_ 
		  "on f.comq1 = a.comq1 and f.lineq1 = a.lineq1 and a.comtype ='6' " &_ 
		  "WHERE a.comn <>'' " &  searchqry &_
		  " GROUP BY  a.comq1, a.lineq1, a.comtype, convert(varchar(6),a.comq1)+ case a.lineq1 when 0 then '' else '-' +convert(varchar(2),a.lineq1) end, " &_ 
		  "a.comn, a.LINETEL, a.comcnt, a.usercnt, isnull(g.codenc, e.codenc), a.T1APPLYDAT,a.rcomdrop, a.canceldat, a.GROUPNC, a.leader, " &_ 
		  "a.comagree, a.contract, CASE a.comtype when '3' then c.Tel when '6' then d.Tel when '7' then i.Tel when '8' then h.Tel ELSE '' END, " &_ 
		  "CASE WHEN b.prtno IS NOT NULL AND b.dropdat IS NULL THEN 'Y' ELSE '' END, CASE WHEN b.prtno IS NOT NULL AND b.dropdat IS NULL AND b.closedat IS NOT NULL THEN 'Y' ELSE '' END, CASE WHEN b.prtno IS NOT NULL AND b.dropdat IS NULL AND b.AUDITdat IS NOT NULL THEN 'Y' ELSE '' END " &_ 
		  "order by comn "

 'Response.Write "sql=" & SQLLIST
End Sub
%>
