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
  searchShow=Request("searchShow")
  parmKey=Request("Key")
  aryParmKey=Split(parmKey &";;;;;;;;;;;;;;;",";")
  'parmkey=aryParmKey(2) & ";"
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
<link REL="stylesheet" HREF="/webUtilityV4EBT/DBAUDI/keyList.css" TYPE="text/css">
<link REL="stylesheet" HREF="keyList.css" TYPE="text/css">
<meta http-equiv="Content-Type" content="text/html; charset=big5">
<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/deleteDialogue.inc" -->
<script language="vbscript">
Sub runAUDI(accessMode,key)
    Dim prog,strFeature,msg
    prog="<%=dataProg%>"
    If prog="None" Then
    Else
       Randomize  
       prog="<%=dataProg%>?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
       '修改二
'       if accessMode ="A" then 
'		  prog="<%=dataProg%>?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &"&parm="&key&"&<%=dataProgParm%>"
'       else
'		  prog="<%=dataProg%>?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
'       end if

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
    Set diagWindow=Window.open(prog,"search",StrFeature)
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
<body onLoad="runSearchOpt()" ID="Form1">
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
<!-- 修改一 -->
          <input type="button" class=keyListButton value="<%=aryButton(0)%>" onClick="runAUDI 'A',';<%=parmKey%>'">&nbsp;&nbsp; 
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
                 value="<%=aryButton(5)%>" style="display:none">
          <span id="functionOpt" style="">
<%   aryOptName=Split(functionOptName,";")
     For i = 0 To Ubound(aryOptName)%>
             <input type="button" class=keyListButton value="<%=aryOptName(i)%>"
                    onClick="runOptProg('<%=i%>')" style="background-color:blue;">
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
<link REL="stylesheet" HREF="keyList.css" TYPE="text/css">
<meta http-equiv="Content-Type" content="text/html; charset=big5">
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
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
	company="元訊寬頻網路股份有限公司"
	system="So-net管理系統"
	title="So-net派工作業"
	buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
	V=split(SrAccessPermit,";")
	AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
	ButtonEnable=V(0) & ";N;Y;Y;Y;Y"
	functionOptName="列 印;結 案;結案返轉;作 廢;作廢返轉"
	functionOptProgram="/RTAP/REPORT/Sonet/RTSonetCustSndWrkPV.asp;RTSonetCustSndWrkClose.asp;RTSonetCustSndWrkCloseRtn.asp;RTSonetCustSndWrkCancel.asp;RTSonetCustSndWrkCancelRtn.asp"
	functionOptPrompt="N;Y;Y;Y;Y"
	functionoptopen= "1;1;1;1;1"
	If V(1)="Y" then
		accessMode="U"
	Else
		accessMode="I"
	End IF
	DSN="DSN=RTLib"
	formatName="派工單號;none;none;none;派工別;主線;社區名稱;客戶名稱;FTP資料<BR>匯入日;派工日;預定<BR>施工人;預定<BR>施工日;完工人;完工日;結案人;結案日;作廢日"
	sqlDelete="SELECT c_varchar,i_smallint,i_tinyint,c_varchar,c_varchar,c_varchar,c_varchar,c_varchar,d_datetime,d_datetime," &_
			"c_varchar,d_datetime,c_varchar,d_datetime,c_varchar,d_datetime,d_datetime from RTTemplate"
	dataTable="RTSonetSndWrk"
	userDefineDelete="Yes"
	numberOfKey=4

	'if FrGetUserEmply(Request.ServerVariables("LOGON_USER")) ="T89039" then
    '	dataProg="RTFaqD_test.asp"
    'else
    	dataProg="RTSonetCustSndWrkD.asp"
    'end if        	

	datawindowFeature=""
	searchWindowFeature="width=500,height=350,scrollbars=yes"
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
	searchProg="self"
	searchFirst=FALSE
	If searchQry="" Then
		 searchQry=" a.wrkno <>'' "
		 searchShow="全部"
	'ELSE
	'	 SEARCHFIRST=FALSE
	End If

	if ARYPARMKEY(0) <>"" then 
		searchQry = searchQry & " and a.comq1=" & aryparmkey(0)
	end if
	if ARYPARMKEY(1) <>"" then 
		searchQry = searchQry & " and a.lineq1=" & aryparmkey(1)
	end if
	if ARYPARMKEY(2) <>"" then 
		searchQry = searchQry & " and a.cusid='" & aryparmkey(2) &"' "
	end if

	'修改三
	'if len(trim(ParmKey))>2 then
	'    searchQry = " and a.canceldat is null and a.comtype=" &aryParmKey(0)& " and a.comq1=" &aryParmKey(1)& " and a.lineq1='" &aryParmKey(2)& "' and a.cusid='" &aryParmKey(3)& "' and a.entryno=" &aryParmKey(4)
'		searchShow="主線序號︰"& aryParmKey(1) &"-" & aryParmKey(2) & ",客戶代號︰" & aryParmKey(3) & ",客戶項次︰" & aryParmKey(4)
	'end if

	'If searchQry="" Then
	'	searchQry=" and a.closedat is null and a.canceldat is null "
		'searchShow="主線序號︰"& comq1xx &"-" & lineq1xx & ",社區名稱︰" & COMN & ",用戶名稱︰" & cusnc & ",主線位址︰" & COMADDR
	'ELSE
	'   SEARCHFIRST=FALSE
	'End If

	'讀取登入帳號之群組資料
	'-------------------------------------------------------------------------------------------
  	sqlList="select a.wrkno, a.comq1, a.lineq1, a.cusid, b.codenc, convert(varchar(6), a.comq1) +'-'+ convert(varchar(3),a.lineq1), " &_
  			"c.comn, n.cusnc, impdat, a.sndwrkdat, isnull(f.shortnc, h.cusnc), a.assigndat, isnull(i.shortnc, k.cusnc), " &_
  			"a.finishdat, m.cusnc, a.closedat, a.canceldat " &_
			"from RTSonetSndWrk a " &_
			"	left outer join RTCode b on a.wrktyp = b.code and b.kind ='P6' " &_
			"	left outer join RTSonetCmtyH c on c.comq1 = a.comq1 " &_
			"	left outer join RTObj f on f.cusid = a.assigncons " &_
			"	left outer join RTEmployee g inner join RTObj h on h.cusid = g.cusid on g.emply = a.assigneng " &_
			"	left outer join RTObj i on i.cusid = a.finishcons " &_
			"	left outer join RTEmployee j inner join RTObj k on k.cusid = j.cusid on j.emply = a.finisheng " &_
			"	left outer join RTEmployee l inner join RTObj m on m.cusid = l.cusid on l.emply = a.closeusr " &_
			"	left outer join RTSonetCust n on n.comq1 = a.comq1 and n.lineq1 = a.lineq1 and n.cusid = a.cusid " &_
			"where " & searchqry 
	'response.Write "SQL=" & SQLlist

End Sub
Sub SrRunUserDefineDelete()

End Sub
%>