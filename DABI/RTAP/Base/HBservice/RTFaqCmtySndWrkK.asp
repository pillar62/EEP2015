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
  '修改一
  Dim detailwindowFeature,rscount,comtype,comq1,lineq1,cusid,entryno
  
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
    <link rel="stylesheet" href="/webUtilityV4EBT/DBAUDI/keyList.css" type="text/css">
    <link rel="stylesheet" href="keyList.css" type="text/css">
    <meta http-equiv="Content-Type" content="text/html; charset=big5">
    <!-- #include virtual="/WebUtilityV4EBT/DBAUDI/deleteDialogue.inc" -->

    <script language="vbscript">
Sub runAUDI(accessMode,key)
    Dim prog,strFeature,msg
    prog="<%=dataProg%>"
    If prog="None" Then
    Else
       Randomize  
       'prog="<%=dataProg%>?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
       '修改二
       if accessMode ="A" then 
		  prog="<%=dataProg%>?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &"&parm="&key&"&<%=dataProgParm%>"
       else
		  prog="<%=dataProg%>?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
       end if

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
         if opt =0 then
            prog=aryOptProg(opt)
            Set diagWindow=Window.open(prog,"",StrFeature)
         else
            Msgbox("在您執行功能選項前，請先挑選一筆資料")   
         end if 
         
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
<body onload="runSearchOpt()" id="Form1">
    <%Else%>
    <body bgcolor="C3C9D2" background="/WEBAP/IMAGE/bg.gif">
        <%End If%>
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr class="keyListTitle">
                <td width="20%" align="left">
                    <%=Request.ServerVariables("LOGON_USER")%>
                </td>
                <td width="60%" align="center">
                    <%=company%>
                </td>
                <td width="20%" align="right">
                    <%=datevalue(Now())%>
                </td>
            </tr>
            <tr class="keyListTitle">
                <td>
                    &nbsp;</td>
                <td align="center">
                    <%=system%>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr class="keyListTitle">
                <td>
                    &nbsp;</td>
                <td align="center">
                    <%=title%>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
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
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="right">
                        <%If aryButtonEnable(0)="Y" Then%>
                        <input type="button" class="keyListButton" value="<%=aryButton(0)%>" onclick="runAUDI 'A','<%=parmKey%>'">&nbsp;&nbsp;
                        <%End If%>
                        <%If aryButtonEnable(1)="Y" Then%>
                        <input type="button" class="keyListButton" value="<%=aryButton(1)%>" onclick="runDelete">&nbsp;&nbsp;
                        <%End If%>
                        <%If aryButtonEnable(2)="Y" Then%>
                        <input type="button" class="keyListButton" value="<%=aryButton(2)%>" onclick="SrClose()">&nbsp;&nbsp;
                        <%End If%>
                        <%If aryButtonEnable(3)="Y" Then%>
                        <input type="button" class="keyListButton" value="<%=aryButton(3)%>" onclick="KeyForm.Submit">
                        <%End If%>
                        <div>
                            <%If aryButtonEnable(4)="Y" Then%>
                            <span onmouseover="" onmouseout="">
                                <input type="button" class="keyListButton" value="<%=aryButton(4) &":" &keyListPage &"/" &TotalPage%>">
                                <span id="pageOpt" style="">
                                    <input type="button" class="keyListButton" value="第一頁" onclick="keyForm.currentPage.Value=1:keyForm.Submit">
                                    <input type="button" class="keyListButton" value="上一頁" onclick="keyForm.currentPage.Value=keyForm.currentPage.Value-1:keyForm.Submit">
                                    <input type="button" class="keyListButton" value="下一頁" onclick="keyForm.currentPage.Value=keyForm.currentPage.Value+1:keyForm.Submit">
                                    <input type="button" class="keyListButton" value="最末頁" onclick="keyForm.currentPage.Value=<%=TotalPage%>:keyForm.Submit">
                                </span></span>
                            <%End If%>
                            <%If aryButtonEnable(5)="Y" Then%>
                            <span onmouseover="" onmouseout="">
                                <input type="button" class="keyListButton" value="<%=aryButton(5)%>" style="display: none">
                                <span id="functionOpt" style="">
                                    <%   aryOptName=Split(functionOptName,";")
     For i = 0 To Ubound(aryOptName)%>
                                    <input type="button" class="keyListButton" value="<%=aryOptName(i)%>" onclick="runOptProg('<%=i%>')"
                                        style="background-color: blue;">
                                    <%   Next%>
                                </span></span>
                            <%End If%>
                        </div>
                    </td>
                </tr>
            </table>
            <p>
                <form method="post" name="keyForm">
                    <%
  If searchProg <> "" Then 
  countshow="  共有(" & rscount & ")筆資料符合" %>
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="10%">
                                <input type="button" value="搜尋條件" class="keyListSearch" onclick="runSearchOpt"></td>
                            <td width="90%" class="keyListSearch2">
                                <%=searchShow%>
                                <%=countshow%>
                                <input type="text" name="searchShow" value="<%=searchShow%>" style="display: none"
                                    readonly>
                                <input type="text" name="searchQry" value="<%=searchQry%>" style="display: none"
                                    readonly>
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
    <link rel="stylesheet" href="keyList.css" type="text/css">
    <meta http-equiv="Content-Type" content="text/html; charset=big5">

    <script language="vbscript">
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
    <body onclick="newWindow" style="cursor: hand" bgcolor="lightblue">
        <form name="form" method="post">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr class="keyListTitle">
                    <td width="20%" align="left">
                        <%=Request.ServerVariables("LOGON_USER")%>
                    </td>
                    <td width="60%" align="center">
                        <%=company%>
                    </td>
                    <td width="20%" align="right">
                        <%=Now()%>
                    </td>
                    <tr>
                <tr class="keyListTitle">
                    <td>
                        &nbsp;</td>
                    <td align="center">
                        <%=title%>
                    </td>
                    <td>
                        &nbsp;</td>
                    <tr>
            </table>
            <p>
                系統公告
                <table width="100%" border="1" cellpadding="0" cellspacing="0" bgcolor="lightyellow">
                    <!--
  <tr><td background="<%=goodMorningImage%>" height="400" width="400">&nbsp;</td></tr>
-->
                    <tr bgcolor="darkseagreen">
                        <td align="CENTER">
                            項次</td>
                        <td align="CENTER">
                            日期</td>
                        <td align="CENTER">
                            系 統 訊 息</td>
                    </tr>
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
	system="客服管理系統"
	title="社區主機派修查詢"
	buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
	V=split(SrAccessPermit,";")
	AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
	ButtonEnable=V(0) & ";N;Y;Y;Y;Y;Y"  
	if V(0)="Y" then 
		functionOptName="主機歷史派工記錄;追 件;派工單;結 案;結案返轉;作 廢"
		functionOptProgram="RTFaqLineSndWrkK.asp;RTFaqAddK.asp;RTFaqSndWrkK.asp;RTFaqF.asp;RTFaqFR.asp;RTFaqDrop.asp"
		functionOptPrompt="N;N;N;Y;Y;Y"
		functionOptOpen ="1;1;1;1;1;1"
	else
		functionOptName="主機歷史派工記錄;追 件;派工單"
		functionOptProgram="RTFaqLineSndWrkK.asp;RTFaqAddK.asp;RTFaqSndWrkK.asp"
		functionOptPrompt="N;N;N"
		functionOptOpen ="1;1;1"
	end if
	If V(1)="Y" then
		accessMode="U"
	Else
		accessMode="I"
	End IF
	DSN="DSN=RTLib"
	formatName="none;none;社區名稱;報修聯絡人;主線;方案別;派工人;預定施工人;完工日;施工備註;最早派工日;最後派工日;派工迄今時數"
	sqlDelete=	"select b.caseno, b.comtype, c.comn, b.faqman, comq, c.comtypenc, h.cusnc as sndwrkusr, " &_
	            "isnull(d.shortnc, f.cusnc) as assignusr, a.finishdat, a.memo, " &_
	            "replace(left(convert(varchar(30), i.minsndwrkdat, 120), 16), ' ', '　'), " &_
	            "case when i.maxsndwrkdat = i.minsndwrkdat then '' else replace(left(convert(varchar(30), i.maxsndwrkdat, 120), 16), ' ', '　') end, " &_
	            "i.passedhour " &_
	            "from RTSndWork a " &_
	            "inner join RTFaqM b on a.linkno = b.caseno " &_
	            "left outer join HBAdslCmtyCust c on b.comtype = c.comtype and b.comq1 = c.comq1 and b.lineq1 = c.lineq1 and b.cusid = c.cusid and b.entryno = c.entryno " &_
	            "left outer join RTObj d on d.cusid = a.assigncons left outer join RTEmployee e inner join RTObj f on e.cusid = f.cusid on e.emply = a.assigneng " &_
	            "left outer join RTEmployee g inner join RTObj h on g.cusid = h.cusid on g.emply = a.sndwrkusr " &_
	            "inner join ( " &_
	            "select y.comq1, y.lineq1, y.comtype,  min(x.sndwrkdat) as minsndwrkdat, max(x.sndwrkdat) as maxsndwrkdat, " &_
	            "datediff(hh,min(x.sndwrkdat),getdate()) as passedhour " &_
	            "from RTSndWork x inner join RTFaqM y on x.linkno = y.caseno " &_
	            "where x.canceldat is null and y.canceldat is null and x.finishdat is null and worktype ='09' " &_
	            "group by  y.comq1, y.lineq1, y.comtype " &_
	            ") i on i.comq1 = b.comq1 and i.lineq1 = b.lineq1 and i.comtype = b.comtype and i.maxsndwrkdat = a.sndwrkdat " &_
	            "where a.canceldat is null and b.canceldat is null and a.finishdat is null and worktype ='09' and b.caseno='' " &_
	dataTable="RTFaqM"
	userDefineDelete="Yes"
	numberOfKey=2
	dataProg="RTFaqD.asp"
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

	' Open search program when first entry this keylist
	searchProg=""
	searchFirst=FALSE

	If searchQry="" Then
		searchQry=" and b.caseno<>'' "
		'searchShow="主線序號︰"& comq1xx &"-" & lineq1xx & ",社區名稱︰" & COMN & ",用戶名稱︰" & cusnc & ",主線位址︰" & COMADDR
	ELSE
	   SEARCHFIRST=FALSE
	End If

	'讀取登入帳號之群組資料
	'userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
	'Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
	'Domain=Mid(Emply,1,1)
	'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
	'Response.Write "GP=" & usergroup
	'-------------------------------------------------------------------------------------------
	'userlevel=2:為業務工程師==>只能看所屬社區資料
	sqlList=	"select b.caseno, b.comtype, c.comn, b.faqman, comq, c.comtypenc, h.cusnc as sndwrkusr, " &_
	            "isnull(d.shortnc, f.cusnc) as assignusr, a.finishdat, a.memo, " &_
	            "replace(left(convert(varchar(30), i.minsndwrkdat, 120), 16), ' ', '　'), " &_
	            "case when i.maxsndwrkdat = i.minsndwrkdat then '' else replace(left(convert(varchar(30), i.maxsndwrkdat, 120), 16), ' ', '　') end, " &_
	            "i.passedhour " &_
	            "from RTSndWork a " &_
	            "inner join RTFaqM b on a.linkno = b.caseno " &_
	            "left outer join HBAdslCmtyCust c on b.comtype = c.comtype and b.comq1 = c.comq1 and b.lineq1 = c.lineq1 and b.cusid = c.cusid and b.entryno = c.entryno " &_
	            "left outer join RTObj d on d.cusid = a.assigncons left outer join RTEmployee e inner join RTObj f on e.cusid = f.cusid on e.emply = a.assigneng " &_
	            "left outer join RTEmployee g inner join RTObj h on g.cusid = h.cusid on g.emply = a.sndwrkusr " &_
	            "inner join ( " &_
	            "select y.comq1, y.lineq1, y.comtype,  min(x.sndwrkdat) as minsndwrkdat, max(x.sndwrkdat) as maxsndwrkdat, " &_
	            "datediff(hh,min(x.sndwrkdat),getdate()) as passedhour " &_
	            "from RTSndWork x inner join RTFaqM y on x.linkno = y.caseno " &_
	            "where x.canceldat is null and y.canceldat is null and x.finishdat is null and worktype ='09' " &_
	            "group by  y.comq1, y.lineq1, y.comtype " &_
	            ") i on i.comq1 = b.comq1 and i.lineq1 = b.lineq1 and i.comtype = b.comtype and i.minsndwrkdat = a.sndwrkdat " &_
	            "where a.canceldat is null and b.canceldat is null and a.finishdat is null and worktype ='09' " & searchqry &_
	            "order by i.minsndwrkdat "
	'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>
