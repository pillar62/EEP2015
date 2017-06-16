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
  '  RESPONSE.write "opt=" & OPT & ";PROG=" & ARYOPTPROG(OPT)
  '  RESPONSE.END
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
         <input type="text" name="searchQry2" value="<%=searchQry2%>" style="display:none" readonly ID="Text3"></td>
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
<%End Sub%>
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="ADSL+Hi-Building客戶退租/欠拆/復機資料追蹤查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;" & V(3)
  'buttonEnable="Y;Y;Y;Y;Y;N"
 '920603因本作業改為派工單型式故欠退復表列印功能取消；再者欠拆改為直接可派工拆機，故已無欠轉退的作業，因此一併取消
 ' functionOptName="欠拆轉退租;CALLOUT;匯入退租文字檔;欠退復表列印"
 ' functionOptProgram="RTCUSTOVERDUEDROP.ASP;HBCALLOUTK.ASP;RTCustDropUpload.asp;RTCustDropV.asp"
 ' functionOptPrompt="Y;N;N;N"
  functionOptName=" 作 廢 ;作廢返轉;CALLOUT;匯入退租文字檔;欠退復表列印"
  functionOptProgram="RTCustDropDROP.ASP;RTCustDropDROPC.ASP;HBCALLOUTK.ASP;RTCustDropUpload.asp;RTCustDropV.asp"
  functionOptPrompt="Y;Y;N;N;N"  
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;none;社區名稱;客戶名稱;none;退租日;HN號碼;狀態;連絡電話;列印批號;欠轉退;CALL<br>OUT;同意繳<BR>款日;欠轉<BR>退日;實際<BR>拆機員"
 ' sqlDelete="SELECT RTCust.COMQ1,RTCust.CUSID, RTCust.ENTRYNO, RTObj.shortnc, RTCust.CUSTYPE, " _
 '          &"RTCust.LINETYPE, RTCust.RCVD, RTCust.HOME," _
 '          &"RTCust.OFFICE + ' ' + RTCust.EXTENSION  AS Office,RTCust.SNDINFODAT ,rtcust.reqdat " _
 '          &"FROM RTCust INNER JOIN RTObj ON RTCust.CUSID = RTObj.CUSID " _
 '          &"WHERE RTCust.COMQ1=0 " _
 '          &"ORDER BY RTCust.CUSID, RTCust.ENTRYNO "
   sqlDelete="SELECT  HBCustDrop.serno,HBCustDrop.CUSID, HBCustDrop.ENTRYNO,  HBCustDrop.CASETYPE,rtcmty.comn,HBcustdrop.cusnc, HBCustDrop.ENTRYNO,(convert(char(4),datepart(year,applydat)) + '/' + convert(char(4),datepart(month,applydat)) + '/' + convert(char(4),datepart(day,applydat))) AS applydat,hbcustdrop.CUSNO,hbcustdrop.status,hbcustdrop.TEL,hbcustdrop.UPDDATABASE,hbcustdrop.OVERDUEDROP,hbcustdrop.CALLOUTFLAG,hbcustdrop.AGREEPAYDAT,hbcustdrop.OVERDUETNSDAT " _
            &"FROM HBCustDrop INNER JOIN " _
            &"RTCust ON HBCustDrop.CUSID = RTCust.CUSID AND HBCustDrop.ENTRYNO = RTCust.ENTRYNO inner join rtcmty on rtcust.comq1=rtcmty.comq1 " _
            &"WHERE  HBCUSTDROP.CASETYPE = '1' AND HBCustDrop.CUSID ='*' " _
            &"UNION " _
            &"SELECT HBCustDrop.serno,HBCustDrop.CUSID, HBCustDrop.ENTRYNO,   HBCustDrop.CASETYPE,rtcustadslcmty.comn,HBcustdrop.cusnc,(convert(char(4),datepart(year,applydat)) + '/' + convert(char(4),datepart(month,applydat)) + '/' + convert(char(4),datepart(day,applydat))) AS applydatt,hbcustdrop.CUSNO,hbcustdrop.status,hbcustdrop.TEL,hbcustdrop.UPDDATABASE,hbcustdrop.OVERDUEDROP,hbcustdrop.CALLOUTFLAG,hbcustdrop.AGREEPAYDAT,hbcustdrop.OVERDUETNSDAT " _
            &"FROM HBCustDrop INNER JOIN " _
            &"RTCustADSL ON HBCustDrop.CUSID = RTCustADSL.CUSID AND " _
            &"HBCustDrop.ENTRYNO = RTCustADSL.ENTRYNO inner join rtcustadslcmty on rtcustadsl.comq1=rtcustadslcmty.cutyid " _
            &"WHERE HBCUSTDROP.CASETYPE = '2' AND HBCustDrop.CUSID ='*' " _
            &"order by COMN,HBCUSTDROP.CUSNC,APPLYDAT " 
  dataTable="HBCUSTDROP"
  userDefineDelete="Yes"
  extTable=""
  numberOfKey=1
  dataProg="hbCustdropD.asp"
  datawindowFeature=""
  searchWindowFeature="width=700,height=520,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=True
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="rtcustDROPs.asp"
  searchFirst=false
  If searchQry="" Then
     searchQry=" HBCustDrop.CUSID <> '*' and hbCUSTDROP.actdrop is  null " & ";"
     searchqry2=" HBCustDrop.CUSID <> '*' and hbCUSTDROP.actdrop is  null " & ";"
     searchShow="未拆機客戶"
  ELSE
     searchFirst=False
  End If
  '主要是因為查詢條件中有having語法,而having語法在group by 之後,因此需將查詢之語法分成二段來處理
  searchqryary=split(searchqry,";")
  searchqryary2=split(searchqry2,";")
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'userlevel=2:為業務工程師==>只能看所屬社區資料
  'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
 ' Response.Write "DOMAIN=" & domain & "<BR>"
  Domain=Mid(Emply,1,1)
  select case Domain
         case "T"
            DAreaID="='C1'"
         case "P"
            DAreaID="='C1'"
         case "C"
            DAreaID="='C3'"         
         case "K"
            DAreaID="='C4'"         
         case else
            DareaID="='*'"
  end select
  '高階主管可讀取全部資料
  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="T90076" OR Ucase(emply)="T92145" OR _
     Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T91103" or Ucase(emply)="T91119" or Ucase(emply)="T92138" then
     DAreaID="<>'*'"
  end if
    '客服中心權限
  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="T90076" OR _
     Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T91103" or Ucase(emply)="T91104" or Ucase(emply)="T91119" or Ucase(emply)="T92138" then
     DAreaID="<>'*'"
  end if
  '資訊部管理員可讀取全部資料
  if userlevel=31 then DAreaID="<>'*'"  
  sqllist="SELECT         HBCustDrop.serno,HBCustDrop.CUSID, HBCustDrop.ENTRYNO, HBCustDrop.CASETYPE, " _
         &"               RTCmty.COMN, HBCustDrop.CUSNC, HBCustDrop.ENTRYNO AS Expr1, " _
         &"               CONVERT(char(4), DATEPART(year, HBCustDrop.APPLYDAT)) " _
         &"               + '/' + CONVERT(char(4), DATEPART(month, HBCustDrop.APPLYDAT)) " _
         &"               + '/' + CONVERT(char(4), DATEPART(day, HBCustDrop.APPLYDAT)) AS applydat, " _
         &"               HBCustDrop.CUSNO, HBCustDrop.STATUS, HBCustDrop.TEL, " _
         &"               HBCustDrop.prtno,hbcustdrop.OVERDUEDROP, " _
         &"               SUM(CASE WHEN HBCUSTDROPCALLOUT.SERNO IS NOT NULL THEN 1 ELSE 0 END), HBCustDrop.AGREEPAYDAT, " _
         &"               HBCustDrop.OVERDUETNSDAT, " _
         &"               RTObj.CUSNC AS Expr2 " _
         &"FROM           RTAreaCty INNER JOIN " _
         &"               HBCustDrop LEFT OUTER JOIN " _
         &"               RTCust ON HBCustDrop.CUSID = RTCust.CUSID AND " _
         &"               HBCustDrop.ENTRYNO = RTCust.ENTRYNO INNER JOIN " _
         &"               RTCmty ON RTCust.COMQ1 = RTCmty.COMQ1 ON " _
         &"               RTAreaCty.CUTID = RTCmty.CUTID AND RTAreaCty.EXDAT IS NULL INNER JOIN " _
         &"               RTArea ON RTAreaCty.AREAID = RTArea.AREAID AND " _
         &"               RTArea.AREATYPE = '3' LEFT OUTER JOIN HBCUSTDROPCALLOUT ON " _
         &"               HBCustDrop.SERNO = HBCUSTDROPCALLOUT.SERNO LEFT OUTER JOIN " _
         &"               RTEmployee RTEmployee_2 INNER JOIN " _
         &"               RTObj ON RTEmployee_2.CUSID = RTObj.CUSID ON " _
         &"               HBCustDrop.ACTDROPUSR = RTEmployee_2.EMPLY " _
         &"WHERE          ( HBCUSTDROP.UPDDATABASE = 'Y' or  HBCUSTDROP.status='復裝')  AND HBCUSTDROP.CASETYPE  IN ( '1','4') AND flag='' and " & SEARCHQRYary(0) & " AND RTAREA.AREAID " & DAREAID  _
         &" GROUP BY  HBCustDrop.SERNO, HBCustDrop.CUSID, HBCustDrop.ENTRYNO, " _
         &"               HBCustDrop.CASETYPE, RTCmty.COMN, HBCustDrop.CUSNC, " _
         &"               HBCustDrop.ENTRYNO, CONVERT(char(4), DATEPART(year, " _
         &"               HBCustDrop.APPLYDAT)) + '/' + CONVERT(char(4), DATEPART(month, " _
         &"               HBCustDrop.APPLYDAT)) + '/' + CONVERT(char(4), DATEPART(day, " _
         &"               HBCustDrop.APPLYDAT)), HBCustDrop.CUSNO, HBCustDrop.STATUS, " _
         &"               HBCustDrop.TEL, HBCustDrop.prtno, HBCustDrop.OVERDUEDROP, " _
         &"               HBCustDrop.AGREEPAYDAT, " _
         &"               HBCustDrop.OVERDUETNSDAT, RTObj.CUSNC " & searchqryary(1) & " " _
         &" UNION " _
         &"SELECT         HBCustDrop.serno,HBCustDrop.CUSID, HBCustDrop.ENTRYNO, HBCustDrop.CASETYPE, " _
         &"               RTCustAdslCmty.COMN, HBCustDrop.CUSNC, HBCustDrop.ENTRYNO AS Expr1, " _
         &"               CONVERT(char(4), DATEPART(year, HBCustDrop.APPLYDAT)) " _
         &"               + '/' + CONVERT(char(4), DATEPART(month, HBCustDrop.APPLYDAT)) " _
         &"               + '/' + CONVERT(char(4), DATEPART(day, HBCustDrop.APPLYDAT)) AS applydat, " _
         &"               HBCustDrop.CUSNO, HBCustDrop.STATUS, HBCustDrop.TEL, " _
         &"               HBCustDrop.prtno,hbcustdrop.OVERDUEDROP, SUM(CASE WHEN HBCUSTDROPCALLOUT.SERNO IS NOT NULL THEN 1 ELSE 0 END), " _
         &"               HBCustDrop.AGREEPAYDAT, " _
         &"               HBCustDrop.OVERDUETNSDAT, " _
         &"               RTObj.CUSNC AS Expr2 " _
         &"FROM           HBCustDrop LEFT OUTER JOIN " _
         &"               RTCustADSL ON HBCustDrop.CUSID = RTCustADSL.CUSID AND " _
         &"               HBCustDrop.ENTRYNO = RTCustADSL.ENTRYNO INNER JOIN " _
         &"               RTCustAdslCmty ON " _
         &"               RTCustADSL.COMQ1 = RTCustAdslCmty.CUTYID INNER JOIN " _
         &"               RTSalesGroup ON " _
         &"               RTCustAdslCmty.GROUPID = RTSalesGroup.GROUPID INNER JOIN " _
         &"               RTArea ON RTSalesGroup.COMPLOCATION = RTArea.AREAID LEFT OUTER JOIN HBCUSTDROPCALLOUT ON " _
         &"               HBCustDrop.SERNO = HBCUSTDROPCALLOUT.SERNO LEFT OUTER JOIN " _
         &"               RTObj INNER JOIN " _
         &"               RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
         &"               HBCustDrop.ACTDROPUSR = RTEmployee.EMPLY " _
         &"WHERE          ( HBCUSTDROP.UPDDATABASE = 'Y' or  HBCUSTDROP.status='復裝') and HBCUSTDROP.CASETYPE = '2' AND flag='' and " & SEARCHQRYary2(0) & " AND RTAREA.AREAID " & DAREAID  _
         &" GROUP BY  HBCustDrop.SERNO, HBCustDrop.CUSID, HBCustDrop.ENTRYNO, " _
         &"               HBCustDrop.CASETYPE, RTCustAdslCmty.COMN, HBCustDrop.CUSNC, " _
         &"               HBCustDrop.ENTRYNO, CONVERT(char(4), DATEPART(year, " _
         &"               HBCustDrop.APPLYDAT)) + '/' + CONVERT(char(4), DATEPART(month, " _
         &"               HBCustDrop.APPLYDAT)) + '/' + CONVERT(char(4), DATEPART(day, " _
         &"               HBCustDrop.APPLYDAT)), HBCustDrop.CUSNO, HBCustDrop.STATUS, " _
         &"               HBCustDrop.TEL, HBCustDrop.prtno, HBCustDrop.OVERDUEDROP, " _
         &"               HBCustDrop.AGREEPAYDAT, " _
         &"               HBCustDrop.OVERDUETNSDAT, RTObj.CUSNC " & searchqryary2(1) & " " _      
         &" ORDER BY  COMN, hbcustdrop.CUSNC, APPLYDAT " 
 'Response.Write "sql=" & SQLLIST
End Sub
%>
