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
         <input type="text" name="searchQry1" value="<%=searchQry1%>" style="display:none" readonly ID="Text1">
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
<%End Sub%>
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
	company="元訊寬頻網路股份有限公司"
	system="客服管理系統"
	title="行程資料維護"
	buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
	V=split(SrAccessPermit,";")
	AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
	ButtonEnable=V(0) & ";N;Y;Y;Y;Y;Y"  
		functionOptName="作 廢"
		functionOptProgram="RTSalesSchDrop.asp"
		functionOptPrompt="Y"
	If V(1)="Y" then
		accessMode="U"
	Else
		accessMode="I"
	End IF
	DSN="DSN=RTLib"
	formatName="行程單號;處理<br>人員;處理<br>日期;來源<br>工單號;方案;主線;社區名稱;客戶名稱;作廢日;點數;行程項目"
	sqlDelete="select schno, dealusr, dealdat, workno, comtype, schno, schno, cusid, canceldat, schno, schno " &_
			"from RTSalesSch where a.schno ='' "
	dataTable="RTSalesSch"
	userDefineDelete="Yes"
	numberOfKey=1

	'if FrGetUserEmply(Request.ServerVariables("LOGON_USER")) ="T89039" then
    '	dataProg="RTFaqD_test.asp"
    'else
    	dataProg="RTSalesSchD.asp"
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
	keyListPageSize=50

	' Open search program when first entry this keylist
	searchProg="RTSalesSchS.asp"
	searchFirst=FALSE

	If searchQry="" Then
		searchQry=" and a.udat >= dateadd(d, -30, getdate()) "
		searchShow="最近一個月"
		'userlevel=2:為業務工程師==>只能看所屬社區資料
		userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
		if userlevel =2 then
			Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
			searchQry = searchQry & " and a.DEALUSR='"& Emply &"' "
		end if 
	ELSE
	   SEARCHFIRST=FALSE
	End If

	'-------------------------------------------------------------------------------------------
	sqlList="select schno, f.cusnc, a.dealdat, a.workno, b.codenc, " &_
			"convert(varchar(5), n.comq1)+case when n.lineq1=0 then '' else '-'+convert(varchar(5), n.lineq1) end, " &_
			"n.comn, c.cusnc, a.canceldat, convert(varchar(5), " &_
			"case a.score01 when 'Y' then convert(float,aa.parm1) else 0 end + " &_
			"case a.score02 when 'Y' then convert(float,bb.parm1) else 0 end + " &_
			"case a.score03 when 'Y' then convert(float,cc.parm1) else 0 end + " &_
			"case a.score04 when 'Y' then convert(float,dd.parm1) else 0 end + " &_
			"case a.score05 when 'Y' then convert(float,ee.parm1) else 0 end + " &_
			"case a.score06 when 'Y' then convert(float,ff.parm1) else 0 end + " &_
			"case a.score07 when 'Y' then convert(float,gg.parm1) else 0 end + " &_
			"case a.score08 when 'Y' then convert(float,hh.parm1) else 0 end + " &_
			"case a.score09 when 'Y' then convert(float,ii.parm1) else 0 end + " &_
			"case a.score10 when 'Y' then convert(float,jj.parm1) else 0 end + " &_
			"case a.score11 when 'Y' then convert(float,kk.parm1) else 0 end + " &_
			"case a.score12 when 'Y' then convert(float,ll.parm1) else 0 end + " &_
			"case a.score13 when 'Y' then convert(float,mm.parm1) else 0 end + " &_
			"case a.score14 when 'Y' then convert(float,nn.parm1) else 0 end + " &_
			"case a.score15 when 'Y' then convert(float,oo.parm1) else 0 end + " &_
			"case a.score16 when 'Y' then convert(float,pp.parm1) else 0 end), " &_
			"case a.score01 when 'Y' then '<font color=red>('+aa.parm1+')</font>'+aa.codenc+'<br>' else '' end + " &_
			"case a.score02 when 'Y' then '<font color=red>('+bb.parm1+')</font>'+bb.codenc+'<br>' else '' end + " &_
			"case a.score03 when 'Y' then '<font color=red>('+cc.parm1+')</font>'+cc.codenc+'<br>' else '' end + " &_
			"case a.score04 when 'Y' then '<font color=red>('+dd.parm1+')</font>'+dd.codenc+'<br>' else '' end + " &_
			"case a.score05 when 'Y' then '<font color=red>('+ee.parm1+')</font>'+ee.codenc+'<br>' else '' end + " &_
			"case a.score06 when 'Y' then '<font color=red>('+ff.parm1+')</font>'+ff.codenc+'<br>' else '' end + " &_
			"case a.score07 when 'Y' then '<font color=red>('+gg.parm1+')</font>'+gg.codenc+'<br>' else '' end + " &_
			"case a.score08 when 'Y' then '<font color=red>('+hh.parm1+')</font>'+hh.codenc+'<br>' else '' end + " &_
			"case a.score09 when 'Y' then '<font color=red>('+ii.parm1+')</font>'+ii.codenc+'<br>' else '' end + " &_
			"case a.score10 when 'Y' then '<font color=red>('+jj.parm1+')</font>'+jj.codenc+'<br>' else '' end + " &_
			"case a.score11 when 'Y' then '<font color=red>('+kk.parm1+')</font>'+kk.codenc+'<br>' else '' end + " &_
			"case a.score12 when 'Y' then '<font color=red>('+ll.parm1+')</font>'+ll.codenc+'<br>' else '' end + " &_
			"case a.score13 when 'Y' then '<font color=red>('+mm.parm1+')</font>'+mm.codenc+'<br>' else '' end + " &_
			"case a.score14 when 'Y' then '<font color=red>('+nn.parm1+')</font>'+nn.codenc+'<br>' else '' end + " &_
			"case a.score15 when 'Y' then '<font color=red>('+oo.parm1+')</font>'+oo.codenc+'<br>' else '' end + " &_
			"case a.score16 when 'Y' then '<font color=red>('+pp.parm1+')</font>'+pp.codenc else '' end " &_
			"FROM RTSalesSch a " &_
			"left outer join RTCode b on a.comtype = b.code and b.kind ='P5' " &_
			"left outer join RTEmployee e inner join RTObj f on f.cusid = e.cusid on e.emply = a.DEALUSR " &_
			"left outer join HBAdslCmty n on a.comtype = n.comtype and a.comq1 = n.comq1 and a.lineq1 = n.lineq1 " &_
			"left outer join HBAdslCmtyCust c on a.comtype = c.comtype and a.comq1 = c.comq1 and a.lineq1 = c.lineq1 and a.cusid = c.cusid and a.entryno = c.entryno " &_
			"left outer join RTCode aa on replace(a.score01,'Y','01') = aa.code and aa.kind ='R5' " &_
			"left outer join RTCode bb on replace(a.score02,'Y','02') = bb.code and bb.kind ='R5' " &_
			"left outer join RTCode cc on replace(a.score03,'Y','03') = cc.code and cc.kind ='R5' " &_
			"left outer join RTCode dd on replace(a.score04,'Y','04') = dd.code and dd.kind ='R5' " &_
			"left outer join RTCode ee on replace(a.score05,'Y','05') = ee.code and ee.kind ='R5' " &_
			"left outer join RTCode ff on replace(a.score06,'Y','06') = ff.code and ff.kind ='R5' " &_
			"left outer join RTCode gg on replace(a.score07,'Y','07') = gg.code and gg.kind ='R5' " &_
			"left outer join RTCode hh on replace(a.score08,'Y','08') = hh.code and hh.kind ='R5' " &_
			"left outer join RTCode ii on replace(a.score09,'Y','09') = ii.code and ii.kind ='R5' " &_
			"left outer join RTCode jj on replace(a.score10,'Y','10') = jj.code and jj.kind ='R5' " &_
			"left outer join RTCode kk on replace(a.score11,'Y','11') = kk.code and kk.kind ='R5' " &_
			"left outer join RTCode ll on replace(a.score12,'Y','12') = ll.code and ll.kind ='R5' " &_
			"left outer join RTCode mm on replace(a.score13,'Y','13') = mm.code and mm.kind ='R5' " &_
			"left outer join RTCode nn on replace(a.score14,'Y','14') = nn.code and nn.kind ='R5' " &_
			"left outer join RTCode oo on replace(a.score15,'Y','15') = oo.code and oo.kind ='R5' " &_
			"left outer join RTCode pp on replace(a.score16,'Y','16') = pp.code and pp.kind ='R5' " &_
			"where a.schno >'' " & searchqry &_
			"order by a.canceldat, a.dealusr, a.dealdat desc, n.comn, a.schno desc "
'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>