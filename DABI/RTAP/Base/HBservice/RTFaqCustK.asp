<!-- #include virtual="/WebUtilityV4ebt/DBAUDI/keyList.inc" -->
<!-- #include virtual="/WebUtilityV4ebt/DBAUDI/cType.inc" -->
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
  Dim detailwindowFeature,rscount,searchqry2, searchQryAVS
  searchFirst=False
  userDefineDelete="No"
  functionOptPrompt=";;;;;;;;;;;;;;;;;;"
  keyListPageSize=0
  keyListPage=1
  colSplit=1
  searchQry=Request("searchQry")
  searchqry2=request("searchqry2")
  'searchqry3=request("searchqry3")
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
<link REL="stylesheet" HREF="/webUtilityV4ebt/DBAUDI/keyList.css" TYPE="text/css">
<link REL="stylesheet" HREF="keyList.css" TYPE="text/css">
<!-- #include virtual="/WebUtilityV4/DBAUDI/deleteDialogue.inc" -->
<script language="vbscript">
Sub runAUDI(accessMode,key)
    Dim prog,strFeature,msg
    prog="<%=dataProg%>"		'資料程式
    progary=split(prog,";")		'資料程式
    keyary=split(key,";")								'comtype,comq1,lineq1,cusid,entryno
    key=keyary(1) & ";" & keyary(3) & ";" & keyary(4)	'Cht,Sp399
    keyx=keyary(1) & ";" & keyary(2) & ";" & keyary(3)	'非Cht,Sp399
    If prog="None" Then
    Else
       Randomize  
           if keyary(0)="1" then
          prog=progary(1) & "?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
       elseIF keyary(0)="2" then
          prog=progary(0) & "?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
       elseIF keyary(0)="3" then
          prog=progary(2) & "?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
       elseIF keyary(0)="4" then
          prog=progary(3) & "?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
       elseIF keyary(0)="5" then
          prog=progary(4) & "?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &keyx &"&<%=dataProgParm%>"                            
       elseIF keyary(0)="6" then
          prog=progary(5) & "?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &keyx &"&<%=dataProgParm%>"
       elseIF keyary(0)="7" then
          prog=progary(6) & "?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &keyx &"&<%=dataProgParm%>"
       elseIF keyary(0)="8" then
          prog=progary(7) & "?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &keyx &"&<%=dataProgParm%>"
       elseIF keyary(0)="9" then
          prog=progary(8) & "?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &keyx &"&<%=dataProgParm%>"
       elseIF keyary(0)="A" then
          prog=progary(9) & "?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &keyx &"&<%=dataProgParm%>"
       elseIF keyary(0)="B" then
          prog=progary(10) & "?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &keyx &"&<%=dataProgParm%>"
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

    aryOptName=Split("<%=functionOptName%>",";")
    aryOptProg=Split("<%=functionOptProgram%>",";")
    aryOptPrompt=Split("<%=functionOptPrompt%>",";")
    aryOptOpen=Split("<%=functionOptOpen%>",";")    

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
 ' MSGBOX "opt=" & OPT & ";PROG=" & ARYOPTPROG(OPT)
    '當aryoptprompt="H"時,表示不需挑選一筆資料,而直接呼叫程式
    if  aryoptprompt(opt)="H" then
        Randomize  
       if opt=0 then		'客訴	
	  	  prog=aryOptProg(opt)
       elseif opt=1 then	'速博繳款
		  if keyary(0)="3" then		'sp399
			 Prog=aryOptProg(1)
		  elseif keyary(0)="6" then	'sp499
			 Prog=aryOptProg(2)    
		  end if
       end if

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
         keyary=split(document.all("key" &selItem).value,";")	'comtype,comq1,lineq1,cusid,entryno
         key=keyary(1) & ";" & keyary(3) & ";" & keyary(4) &";"        
         keyx=keyary(1) & ";" & keyary(2) & ";" & keyary(3) &";"
         keyz=keyary(0) & ";" & keyary(1) & ";" & keyary(2) & ";" & keyary(3) & ";" & keyary(4) &";"

       if opt=0 then		'客訴	
	  	  prog=aryOptProg(0) &"?V=" &Rnd() &"&key=" & keyz 
       elseif opt=1 then	'速博繳款
		  if keyary(0)="3" then		'sp399
		     prog=aryOptProg(1) &"?V=" &Rnd() &"&key=" & KEY
		  elseif keyary(0)="6" then	'sp499
			 prog=aryOptProg(2) &"?V=" &Rnd() &"&key=" & KEYX
		  else
		     sureRun=""
		  end if
       end if

      ' MSGBOX OPT & ";" & PROG
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
<!--
         <input type="text" name="searchQry3" value="<%=searchQry3%>" style="display:none" readonly ID="Text3">
-->
	</td>
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
<meta http-equiv="Content-Type" content="text/html; charset=big5">
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
<%End Sub%>
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="ADSL+Hi-Building客戶資料查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;" & V(3)
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="客訴處理;速博繳款記錄"
  functionOptProgram="/webap/rtap/base/HBService/RTFaqK.asp;/WEBAP/RTAP/BASE/rtsparqadslcmty/RTSparqCustPayK.asp;/WEBAP/RTAP/BASE/rtsparq499cmty/RTSparq499CustPayK.asp;/WEBAP/RTAP/BASE/rtfareastcmty/RTfareastCustK.asp"
  functionOptPrompt="N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;none;none;社區名稱;客戶名稱;單次;方案;申請日;完工日;報竣日;退租日;作廢日;裝機地址;客訴次數;未結數"

  sqlDelete="SELECT comtype, comq1, lineq1, cusid, entryno, comn, cusnc, entryno, comtypenc, " &_
			"rcvdat, finishdat, docketdat, dropdat, canceldat, RADDR, faqcnt, unfaqcnt " &_
			"FROM HBAdslCmtyCust where comtype ='' "

  dataTable="HBADSLCMTYcust"
  userDefineDelete="Yes"
  extTable=""
  numberOfKey=5
  dataProg="/webap/rtap/base/rtadslcmty/rtcustd.asp;/webap/rtap/base/rtcmty/rtcustd.asp;/webap/rtap/base/rtsparqadslcmty/rtcustd.asp;/webap/rtap/base/rtcmty/rtcustd.asp;/webap/rtap/base/rtEBTcmty/rtEBTcustd.asp;/webap/rtap/base/rtsparq499cmty/RTSparq499CustD.asp;/webap/rtap/base/RTLessorAVSCmty/RTlessoravscustD.asp;/webap/rtap/base/RTLessorCmty/RTlessorcustD.asp;/webap/rtap/base/RTPrjCmty/RTPrjCustD.asp;/webap/rtap/base/RTSonetCmty/RTSonetCustD.asp;/webap/rtap/base/RTfareastcmty/RTfareastCustD.asp"
  datawindowFeature=""
  searchWindowFeature="width=700,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth="600"
  diaHeight="400"
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=False
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=30
  searchProg="RTFaqCustS.asp"
  searchFirst=False
  If searchQry="" Then
	'修改A
     'searchQry=" HBADSLCMTYcust.COMQ1 = 0 "
     'searchqry2=""
     'searchShow="全部"
    if ARYPARMKEY(0) ="" then 
		searchQry=" HBADSLCMTYcust.COMQ1 = 0 "
		searchqry2=""
		searchShow="全部"
	    searchFirst=true
	else
		searchQry=" HBADSLCMTYcust.comtype='" &aryparmkey(2)& "' and HBADSLCMTYcust.comq1=" &aryparmkey(0)& " and HBADSLCMTYcust.lineq1=" &aryparmkey(1)
		searchqry2=""
		searchShow="社區序號︰"& aryparmkey(0) &", 主線序號︰"& aryparmkey(1)
	end if		
  End If
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'userlevel=2:為業務工程師==>只能看所屬社區資料
  'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
 ' Response.Write "DOMAIN=" & domain & "<BR>"
'  Domain=Mid(Emply,1,1)
  SQLLIST="SELECT HBADSLCMTYcust.comtype, HBADSLCMTYcust.comq1, HBADSLCMTYcust.lineq1, HBADSLCMTYcust.cusid, HBADSLCMTYcust.entryno," _
         &"HBADSLCMTYcust.comn, HBADSLCMTYcust.cusnc, HBADSLCMTYcust.entryno AS Expr1,HBADSLCMTYcust.comtypenc, " _
         &"HBADSLCMTYcust.rcvdat, HBADSLCMTYcust.finishdat, HBADSLCMTYcust.docketdat, HBADSLCMTYcust.dropdat, HBADSLCMTYcust.canceldat, HBADSLCMTYcust.RADDR, " _
         &"SUM(CASE WHEN RTFaqM.CASENO IS NOT NULL THEN 1 ELSE 0 END), SUM(CASE WHEN RTFaqM.CASENO IS NOT NULL and RTFaqM.closedat IS NULL THEN 1 ELSE 0 END) " _
         &"FROM HBADSLCMTYcust LEFT OUTER JOIN RTFaqM ON HBADSLCMTYcust.comq1 = RTFaqM.COMQ1 AND HBADSLCMTYcust.ENTRYNO = RTFaqM.ENTRYNO " _
         &"AND HBADSLCMTYcust.comtype = RTFaqM.COMTYPE AND HBADSLCMTYcust.cusid = RTFaqM.CUSID and HBADSLCMTYcust.lineq1 = RTFaqM.lineq1 " _
         &"WHERE HBADSLCMTYcust.comtype in ('3','6','7','8','9','A','B') AND " & SEARCHQRY & " " _
         &"GROUP BY  HBADSLCMTYcust.comq1, HBADSLCMTYcust.lineq1, HBADSLCMTYcust.comtype, HBADSLCMTYcust.cusid, " _
         &"HBADSLCMTYcust.entryno, HBADSLCMTYcust.comn, HBADSLCMTYcust.cusnc, HBADSLCMTYcust.entryno,HBADSLCMTYcust.comtypenc, " _
         &"HBADSLCMTYcust.rcvdat, HBADSLCMTYcust.finishdat, HBADSLCMTYcust.docketdat, HBADSLCMTYcust.dropdat, HBADSLCMTYcust.canceldat, " _
         &"HBADSLCMTYcust.RADDR " & searchqry2 & " "
'RESPONSE.WRITE "SQLLIST=" & SQLLIST
End Sub
%>
