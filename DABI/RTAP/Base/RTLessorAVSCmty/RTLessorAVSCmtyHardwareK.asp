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
<link REL="stylesheet" HREF="/webUtilityV4EBT/DBAUDI/keyList.css" TYPE="text/css">
<link REL="stylesheet" HREF="keyList.css" TYPE="text/css">
<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/deleteDialogue.inc" -->
<script language="vbscript">
Sub runAUDI(accessMode,key)
    Dim prog,strFeature,msg
    'Ū�����u�渹�ĤG�X�ӧP�_�O��ؤu��ҥ[�˪��]��(EK:���ܬ��ȶD���׬��u�AEL:���ܬ��s�u�˾�)
    KEYXXARY=SPLIT(KEY,";")
    KEYKIND=MID(KEYXXARY(3),2,1)
    IF KEYKIND="K" THEN
       prog="RTLessorAVSCmtyLineFAQHARDWARED.ASP"
    ELSEIF KEYKIND="L" THEN
       KEY=KEYXXARY(0) & ";" & KEYXXARY(1) & ";" & KEYXXARY(3) & ";" & KEYXXARY(4)
       prog="RTLessorAVSCmtyLineHARDWARED.ASP"
    '�䥦���w�q���N��RTLessorAVSCMTYHARDWAREK.ASP������ܼƨӨM�w
    ELSE
       prog="RTLessorAVSCmtyLineHARDWARED.asp"
    END IF        

    If prog="None" Then
    Else
       Randomize  
       prog=prog & "?V=" &Rnd() &"&accessMode=" &accessMode &"&key=" &key &"&<%=dataProgParm%>"
      
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
    '����"H"��,��������DO ...LOOP�j��,�_�h���]�D�藍��SEL�ȦӤ��_
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
    '��aryoptprompt="H"��,���ܤ��ݬD��@�����,�Ӫ����I�s�{��
    if  aryoptprompt(opt)="H" then
        Randomize  
        prog=aryOptProg(opt)
      '�� aryoptopen(OPT)="1" :���@��window�}��,"2"��dialog�}��
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
        ' MSGBOX document.all("key" &selItem).value
         KEYXXARY=SPLIT(document.all("key" &selItem).value,";")
         KEYKIND=MID(KEYXXARY(3),2,1)
         IF KEYKIND="K" THEN
            OPT=1
            prog=aryOptProg(opt) &"?V=" &Rnd() &"&key=" &document.all("key" &selItem).value
         ELSEIF KEYKIND="L" THEN
            NEWKEY=KEYXXARY(0) & ";" & KEYXXARY(1) & ";" & KEYXXARY(3) & ";" & KEYXXARY(4)
            OPT=0
            prog=aryOptProg(opt) &"?V=" &Rnd() &"&key=" & NEWKEY
         '�䥦���w�q���N��RTLessorAVSCMTYHARDWAREK.ASP������ܼƨӨM�w
         ELSE
            prog=aryOptProg(opt) &"?V=" &Rnd() &"&key=" &document.all("key" &selItem).value
         END IF              
         
         If aryOptPrompt(opt)<>"N" Then sureRun=Msgbox("�T�{����\��ﶵ---" &aryOptName(opt),vbOKCancel)    
      '�� functionoptopen(OPT)="1" :���@��window�}��,"2"��dialog�}��
         If sureRun="1" Then 
            If aryoptopen(OPT)="1" Then 
               Set diagWindow=Window.open(prog,"",StrFeature)
            else
               Set diagWindow=Window.showmodaldialog(prog,"d2","dialogWidth:" & diawidth & "px;dialogHeight:" & diaheight &"px;")  
            end if 
         end if
      Else
         Msgbox("�b�z����\��ﶵ�e�A�Х��D��@�����")
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
             <input type="button" class=keyListButton value="�Ĥ@��" 
                onClick="keyForm.currentPage.Value=1:keyForm.Submit">
             <input type="button" class=keyListButton value="�W�@��" 
                onClick="keyForm.currentPage.Value=keyForm.currentPage.Value-1:keyForm.Submit">
             <input type="button" class=keyListButton value="�U�@��" 
                onClick="keyForm.currentPage.Value=keyForm.currentPage.Value+1:keyForm.Submit">
             <input type="button" class=keyListButton value="�̥���" 
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
  countshow="  �@��(" & rscount & ")����ƲŦX" %>
<table width="100%" cellPadding=0 cellSpacing=0>
 <tr><td width="10%"><input type="button" value="�j�M����" class=keyListSearch onClick="runSearchOpt"></td>
     <td width="90%" class=keyListSearch2><%=searchShow%><%=countshow%>
         <input type="text" name="searchShow" value="<%=searchShow%>" style="display:none" readonly>
         <input type="text" name="searchQry" value="<%=searchQry%>" style="display:none" readonly>
         <input type="text" name="searchQry2" value="<%=searchQry2%>" style="display:none" readonly>
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
<form name="form" method="post">
<table width="100%" cellPadding=0 cellSpacing=0>
  <tr class=keyListTitle><td width="20%" align=left><%=Request.ServerVariables("LOGON_USER")%></td>
                         <td width="60%" align=center><%=company%></td>
                         <td width="20%" align=right><%=Now()%></td><tr>
  <tr class=keyListTitle><td>&nbsp;</td><td align=center><%=title%></td><td>&nbsp;</td><tr>
</table>
<P>�t�Τ��i
<table widtH="100%" border=1 cellPadding=0 cellSpacing=0 bgcolor="lightyellow">
<!--
  <tr><td background="<%=goodMorningImage%>" height="400" width="400">&nbsp;</td></tr>
-->
  <tr bgcolor="darkseagreen"><TD ALIGN="CENTER">����</TD><TD ALIGN="CENTER">���</TD><TD ALIGN="CENTER">�t�@�Ρ@�T�@��</TD></TR>
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
<%
Sub SrEnvironment()
  company="���T�e�W�����ѥ��������q"
  system="AVS-City�޲z�t��"
  title="AVS-City���ϳ]�Ƹ�Ƭd��"
  buttonName=" �s�W ; �R�� ; ���� ;���s��z;����;�\��ﶵ"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="���ʬd��"
  functionOptProgram="RTLessorAVSCmtyLineHARDWARELOGK.ASP;RTLessorAVSCmtyLineFAQHARDWARELOGK"
  functionOptPrompt="N;N"  
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;none;none;<center>���u�渹</center>;<center>����</center>;<center>���u���</center>;<center>�]�ƦW��/�W��</center>;<center>�ƶq</center>;<center>���B</center>;<center>�X�w�O</center>;<center>��γ渹</center>;<center>��ε��פ�</center>;<center>�]�ƽs��</center>;<center>�W�h�]��<br>�s��</center>;<center>�W�h�]��<br>port��</center>;<center>�@�o���</center>;<center>�@�o��]</center>;none;none;none"
  sqlDelete="SELECT RTLessorAVSCmtyLineHARDWARE.comq1,RTLessorAVSCmtyLineHARDWARE.lineq1, " _
         &"RTLessorAVSCmtyLineHARDWARE.PRTNO, RTLessorAVSCmtyLineHARDWARE.seq,RTLessorAVSCmtyLinesndwork.SENDWORKDAT, " _
         &"RTProdH.PRODNC + '--' + RTProdD1.SPEC, RTLessorAVSCmtyLineHARDWARE.QTY, RTLessorAVSCmtyLineHARDWARE.amt, " _
         &"HBwarehouse.WARENAME,RTLessorAVSCmtyLineHARDWARE.rcvprtno,RTLessorAVSCmtyLineHARDWARE.rcvfinishdat, RTLessorAVSCmtyLineHARDWARE.DROPDAT, RTLessorAVSCmtyLineHARDWARE.DROPREASON, RTObj.CUSNC,RTLessorAVSCmtyLineHARDWARE.BATCHNO,RTLessorAVSCmtyLineHARDWARE.TARDAT " _
         &"FROM RTProdH RIGHT OUTER JOIN RTLessorAVSCmtyLineHARDWARE LEFT OUTER JOIN " _
         &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
         &"RTLessorAVSCmtyLineHARDWARE.DROPUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
         &"HBwarehouse ON RTLessorAVSCmtyLineHARDWARE.WAREHOUSE = HBwarehouse.WAREHOUSE LEFT OUTER " _
         &"JOIN RTProdD1 ON RTLessorAVSCmtyLineHARDWARE.PRODNO = RTProdD1.PRODNO AND " _
         &"RTLessorAVSCmtyLineHARDWARE.ITEMNO = RTProdD1.ITEMNO ON RTProdH.PRODNO = RTLessorAVSCmtyLineHARDWARE.PRODNO " _
         &"left outer join RTLessorAVSCmtyLine on  RTLessorAVSCmtyLinehardware.cusid=RTLessorAVSCmtyLine.cusid " _
         &"WHERE RTLessorAVSCmtyLineHARDWARE.COMQ1=0 "
  dataTable="RTLessorAVSCmtyLineHARDWARE"
  extTable=""
  numberOfKey=5
  dataProg="RTLessorAVSCmtyLineHARDWARED.asp"
  datawindowFeature=""
  searchWindowFeature="width=350,height=160,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="�U�C��ƱN�Q�R���A�Ы��T�{�R�����A�Ϋ������C"
  diaButtonName=" �T�{�R�� ; ���� "
  goodMorning=False
  goodMorningImage=""
  colSplit=1
  keyListPageSize=25
  searchProg="self"
  '----
  set connYY=server.CreateObject("ADODB.connection")
  set rsYY=server.CreateObject("ADODB.recordset")
  dsnYY="DSN=RTLIB"
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorAVSCmtyH ON " _
       &"RTCounty.CUTID = RTLessorAVSCmtyH.CUTID RIGHT OUTER JOIN RTLessorAVSCmtyLine ON RTLessorAVSCmtyH.COMQ1 = RTLessorAVSCmtyLine.COMQ1 " _
       &"where RTLessorAVSCmtyLine.comq1=" & ARYPARMKEY(0) 
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
  else
     COMN=""
  end if
  rsYY.Close

  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTLessorAVSCmtyLinehardware.comq1=" & aryparmkey(0) & " and RTLessorAVSCmtyLineHARDWARE.dropdat is null"
     searchShow="���ϡJ"& aryparmkey(0)  & ",���ϦW�١J" & COMN 
  ELSE
     SEARCHFIRST=FALSE
  End If  

  sqlList="SELECT RTLessorAVSCmtyLineHARDWARE.comq1,RTLessorAVSCmtyLineHARDWARE.lineq1, " _
         &"'',RTLessorAVSCmtyLineHARDWARE.PRTNO, RTLessorAVSCmtyLineHARDWARE.seq,RTLessorAVSCmtyLineHARDWARE.PRTNO, RTLessorAVSCmtyLineHARDWARE.seq,RTLessorAVSCmtyLinesndwork.SENDWORKDAT, " _
         &"RTProdH.PRODNC + '--' + RTProdD1.itemnc + '('+ RTProdD1.SPEC+')', RTLessorAVSCmtyLineHARDWARE.QTY, RTLessorAVSCmtyLineHARDWARE.amt, " _
         &"HBwarehouse.WARENAME,RTLessorAVSCmtyLineHARDWARE.rcvprtno,RTLessorAVSCmtyLineHARDWARE.rcvfinishdat,RTLessorAVSCmtyLineHARDWARE.hostno,RTLessorAVSCmtyLineHARDWARE.prelevelhostno,RTLessorAVSCmtyLineHARDWARE.prelevelportno, RTLessorAVSCmtyLineHARDWARE.DROPDAT, RTLessorAVSCmtyLineHARDWARE.DROPREASON, RTObj.CUSNC,RTLessorAVSCmtyLineHARDWARE.BATCHNO,RTLessorAVSCmtyLineHARDWARE.TARDAT  " _
         &"FROM RTProdH RIGHT OUTER JOIN RTLessorAVSCmtyLineHARDWARE LEFT OUTER JOIN " _
         &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
         &"RTLessorAVSCmtyLineHARDWARE.DROPUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
         &"HBwarehouse ON RTLessorAVSCmtyLineHARDWARE.WAREHOUSE = HBwarehouse.WAREHOUSE LEFT OUTER " _
         &"JOIN RTProdD1 ON RTLessorAVSCmtyLineHARDWARE.PRODNO = RTProdD1.PRODNO AND " _
         &"RTLessorAVSCmtyLineHARDWARE.ITEMNO = RTProdD1.ITEMNO ON RTProdH.PRODNO = RTLessorAVSCmtyLineHARDWARE.PRODNO " _
         &"LEFT OUTER JOIN RTLessorAVSCmtyLinesndwork on RTLessorAVSCmtyLineHARDWARE.comq1=RTLessorAVSCmtyLinesndwork.comq1 and RTLessorAVSCmtyLineHARDWARE.lineq1=RTLessorAVSCmtyLinesndwork.lineq1 and RTLessorAVSCmtyLineHARDWARE.prtno=RTLessorAVSCmtyLinesndwork.prtno " _
         &"WHERE RTLessorAVSCmtyLinehardware.comq1=" & aryparmkey(0) & " and " &searchQry & " " _
         &"union " _
         &"SELECT RTLessorAVSCmtyLineFAQHARDWARE.comq1,RTLessorAVSCmtyLineFAQHARDWARE.lineq1,RTLessorAVSCmtyLineFAQHARDWARE.faqno,RTLessorAVSCmtyLineFAQHARDWARE.PRTNO," _
         &"RTLessorAVSCmtyLineFAQHARDWARE.seq,RTLessorAVSCmtyLineFAQHARDWARE.prtno,RTLessorAVSCmtyLineFAQHARDWARE.seq, RTLessorAVSCmtyLineFAQsndwork.SENDWORKDAT, RTProdH.PRODNC + '--' + RTProdD1.itemnc + '(' + RTProdD1.SPEC + ')', " _
         &"RTLessorAVSCmtyLineFAQHARDWARE.QTY,RTLessorAVSCmtyLineFAQHARDWARE.amt, HBwarehouse.WARENAME, RTLessorAVSCmtyLineFAQHARDWARE.rcvprtno,RTLessorAVSCmtyLineFAQHARDWARE.rcvfinishdat,RTLessorAVSCmtyLinefaqHARDWARE.hostno,RTLessorAVSCmtyLinefaqHARDWARE.prelevelhostno,RTLessorAVSCmtyLinefaqHARDWARE.prelevelportno,RTLessorAVSCmtyLineFAQHARDWARE.DROPDAT, " _
         &"RTLessorAVSCmtyLineFAQHARDWARE.DROPREASON, RTObj.CUSNC, RTLessorAVSCmtyLineFAQHARDWARE.BATCHNO, " _
         &"RTLessorAVSCmtyLineFAQHARDWARE.TARDAT " _
         &"FROM RTProdH RIGHT OUTER JOIN RTLessorAVSCmtyLineFAQHARDWARE LEFT OUTER JOIN RTObj INNER JOIN RTEmployee ON " _
         &"RTObj.CUSID = RTEmployee.CUSID ON RTLessorAVSCmtyLineFAQHARDWARE.DROPUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
         &"HBwarehouse ON RTLessorAVSCmtyLineFAQHARDWARE.WAREHOUSE = HBwarehouse.WAREHOUSE LEFT OUTER JOIN RTProdD1 ON " _
         &"RTLessorAVSCmtyLineFAQHARDWARE.PRODNO = RTProdD1.PRODNO AND RTLessorAVSCmtyLineFAQHARDWARE.ITEMNO = RTProdD1.ITEMNO ON " _
         &"RTProdH.PRODNO = RTLessorAVSCmtyLineFAQHARDWARE.PRODNO LEFT OUTER JOIN RTLessorAVSCmtyLineFAQsndwork ON " _
         &"RTLessorAVSCmtyLineFAQHARDWARE.comq1 = RTLessorAVSCmtyLineFAQsndwork.comq1 AND RTLessorAVSCmtyLineFAQHARDWARE.lineq1 = RTLessorAVSCmtyLineFAQsndwork.lineq1 " _
         &"AND RTLessorAVSCmtyLineFAQHARDWARE.FAQNO = RTLessorAVSCmtyLineFAQsndwork.FAQNO AND " _
         &"RTLessorAVSCmtyLineFAQHARDWARE.prtno = RTLessorAVSCmtyLineFAQsndwork.prtno " _
         &"wHERE RTLessorAVSCmtyLineFAQHARDWARE.comq1 =" & aryparmkey(0) & " AND RTLessorAVSCmtyLineFAQHARDWARE.dropdat IS NULL " _
         &"order by rcvfinishdat "
'Response.Write "sql=" & SQLLIST         
End Sub
%>