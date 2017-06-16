<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="ET-City管理系統"
  title="ET-City主線撤線拆機派工單資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="物品移轉單;列　　印;完工結案;未完工結案;結案返轉;作　　廢;作廢返轉;設備明細;歷史異動"
  functionOptProgram="RTLessorCustHardwareRTNK4.ASP;/RTAP/REPORT/ETCity/RTLessorCmtylineDropSNDWORKPV.asp;RTLessorCmtylineDropsndworkF.asp;RTLessorCmtylineDropsndworkUF.asp;RTLessorCmtylineDropsndworkFR.asp;RTLessorCmtylineDropsndworkdrop.asp;RTLessorCmtylineDropsndworkdropc.asp;RTLessorCmtylineDrophardwareK.asp;RTLessorCmtylineDropsndworkLOGK.asp"
  functionOptPrompt="N;N;Y;N;N;N;N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;none;主線;派工單號;派工日期;列印人員;預定施工員;實際施工員;結案日;未完工結案日;none;none;應收帳款編號;none;none;作廢日;設備數量;轉移轉單數量;已轉數量;待轉數量"
  sqlDelete="SELECT RTLessorCmtyLineDropSNDWORK.comq1,RTLessorCmtyLineDropSNDWORK.lineq1,RTLessorCmtyLineDropSNDWORK.entryno, RTLessorCmtyLineDropSNDWORK.PRTNO,rtrim(convert(char(6),RTLessorCmtyLineDrop.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorCmtyLineDrop.lineQ1))  as comqline, RTLessorCmtyLineDropSNDWORK.PRTNO, RTLessorCmtyLineDropSNDWORK.SENDWORKDAT, " _
           &"RTOBJ.CUSNC,CASE WHEN RTOBJ_2.SHORTNC <>'' THEN RTOBJ_2.SHORTNC ELSE RTOBJ_1.CUSNC END,CASE WHEN RTOBJ_4.SHORTNC <>'' THEN RTOBJ_4.SHORTNC ELSE RTOBJ_3.CUSNC END, " _
           &"RTLessorCmtyLineDropSNDWORK.closedat,RTLessorCmtyLineDropSNDWORK.unclosedat,RTLessorCmtyLineDropSNDWORK.BONUSCLOSEYM, RTLessorCmtyLineDropSNDWORK.BONUSFINCHK,RTLessorCmtyLineDropSNDWORK.batchno, RTLessorCmtyLineDropSNDWORK.STOCKCLOSEYM, RTLessorCmtyLineDropSNDWORK.STOCKFINCHK, " _
           &"RTLessorCmtyLineDropSNDWORK.DROPDAT FROM RTLessorCmtyLineDropSNDWORK LEFT OUTER JOIN RTObj RTObj_4 ON RTLessorCmtyLineDropSNDWORK.REALCONSIGNEE = RTObj_4.CUSID LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_2 INNER JOIN RTObj RTObj_3 ON RTEmployee_2.CUSID = RTObj_3.CUSID ON RTLessorCmtyLineDropSNDWORK.REALENGINEER = RTEmployee_2.EMPLY LEFT OUTER JOIN " _
           &"RTObj RTObj_2 ON RTLessorCmtyLineDropSNDWORK.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN " _
           &"RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON RTLessorCmtyLineDropSNDWORK.ASSIGNENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
           &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON RTLessorCmtyLineDropSNDWORK.PRTUSR = RTEmployee.EMPLY left outer join " _
           &"RTLessorCmtyLineDrop on RTLessorCmtyLineDropSNDWORK.comq1=RTLessorCmtyLineDrop.comq1 and RTLessorCmtyLineDrop.lineq1=RTLessorCmtyLineDropSNDWORK.lineq1 and " _
           &"RTLessorCmtyLineDropSNDWORK.entryno=RTLessorCmtyLineDrop.entryno " _
           &"where RTLessorCmtyLineDropSNDWORK.comq1=0 "
  dataTable="RTLessorCmtyLineDropSNDWORK"
  userDefineDelete="Yes"
  numberOfKey=4
  dataProg="RTLessorCmtyLineDropSNDWORKD.asp"
  datawindowFeature=""
  searchWindowFeature="width=350,height=160,scrollbars=yes"
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
  searchProg="RTLessorCmtyLineDropSNDWORKs.asp"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
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
  '----
   set connYY=server.CreateObject("ADODB.connection")
  set rsYY=server.CreateObject("ADODB.recordset")
  dsnYY="DSN=RTLIB"
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorCmtyH ON " _
       &"RTCounty.CUTID = RTLessorCmtyH.CUTID " _
       &"where RTLessorCmtyH.comq1=" & ARYPARMKEY(0) 
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
  else
     COMN=""
  end if
  rsYY.Close
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorCmtyLine ON  " _
       &"RTCounty.CUTID = RTLessorCmtyLine.CUTID " _
       &"where RTLessorCmtyLine.comq1=" & ARYPARMKEY(0) & " and RTLessorCmtyLine.lineq1=" & ARYPARMKEY(1)
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     comaddr=""
     COMaddr=rsYY("cutnc") & rsyy("township")
     if rsyy("village") <> "" then
         COMaddr= COMaddr & rsyy("village") & rsyy("cod1")
     end if
     if rsyy("NEIGHBOR") <> "" then
         COMaddr= COMaddr & rsyy("NEIGHBOR") & rsyy("cod2")
     end if
     if rsyy("STREET") <> "" then
         COMaddr= COMaddr & rsyy("STREET") & rsyy("cod3")
     end if
     if rsyy("SEC") <> "" then
         COMaddr= COMaddr & rsyy("SEC") & rsyy("cod4")
     end if
     if rsyy("LANE") <> "" then
         COMaddr= COMaddr & rsyy("LANE") & rsyy("cod5")
     end if
     if rsyy("ALLEYWAY") <> "" then
         COMaddr= COMaddr & rsyy("ALLEYWAY") & rsyy("cod7")
     end if
     if rsyy("NUM") <> "" then
         COMaddr= COMaddr & rsyy("NUM") & rsyy("cod8")
     end if
     if rsyy("FLOOR") <> "" then
         COMaddr= COMaddr & rsyy("FLOOR") & rsyy("cod9")
     end if
     if rsyy("ROOM") <> "" then
         COMaddr= COMaddr & rsyy("ROOM") & rsyy("cod10")
     end if
  else
     COMaddr=""
  end if
  RSYY.Close
  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTLessorCmtyLineDropSNDWORK.comq1=" & aryparmkey(0) & " and RTLessorCmtyLineDropSNDWORK.lineq1=" & aryparmkey(1) & " and RTLessorCmtyLineDropSNDWORK.entryno=" & aryparmkey(2) & " and  RTLessorCmtyLineDropSNDWORK.dropdat is null AND RTLessorCmtyLineDropSNDWORK.UNCLOSEDAT IS NULL "
     searchShow="主線序號︰"& aryparmkey(0) &"-" & aryparmkey(1) & ",社區名稱︰" & COMN & ",主線位址︰" & COMADDR
  ELSE
     SEARCHFIRST=FALSE
  End If
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  '讀取登入帳號之群組資料
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
  'if UCASE(emply)="T89001" or Ucase(emply)="T89002" or  Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="T90076" OR _
  '   Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T89076"then
  '   DAreaID="<>'*'"
  'end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  if userlevel=31 then DAreaID="<>'*'"
  
         sqlList="SELECT RTLessorCmtyLineDropSNDWORK.comq1,RTLessorCmtyLineDropSNDWORK.lineq1,RTLessorCmtyLineDropSNDWORK.entryno, RTLessorCmtyLineDropSNDWORK.PRTNO,rtrim(convert(char(6),RTLessorCmtyLineDrop.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorCmtyLineDrop.lineQ1))  as comqline, RTLessorCmtyLineDropSNDWORK.PRTNO, RTLessorCmtyLineDropSNDWORK.SENDWORKDAT, " _
           &"RTOBJ.CUSNC,CASE WHEN RTOBJ_2.SHORTNC <>'' THEN RTOBJ_2.SHORTNC ELSE RTOBJ_1.CUSNC END,CASE WHEN RTOBJ_4.SHORTNC <>'' THEN RTOBJ_4.SHORTNC ELSE RTOBJ_3.CUSNC END, " _
           &"RTLessorCmtyLineDropSNDWORK.closedat,RTLessorCmtyLineDropSNDWORK.unclosedat,RTLessorCmtyLineDropSNDWORK.BONUSCLOSEYM, RTLessorCmtyLineDropSNDWORK.BONUSFINCHK,RTLessorCmtyLineDropSNDWORK.batchno, RTLessorCmtyLineDropSNDWORK.STOCKCLOSEYM, RTLessorCmtyLineDropSNDWORK.STOCKFINCHK, " _
           &"RTLessorCmtyLineDropSNDWORK.DROPDAT,SUM(CASE WHEN RTLessorCmtyLineDropHARDWARE.dropdat IS NULL AND RTLessorCmtyLineDropHARDWARE.QTY > 0 " _
           &"THEN RTLessorCmtyLineDropHARDWARE.QTY ELSE 0 END), SUM(CASE WHEN RTLessorCmtyLineDropHARDWARE.dropdat IS NULL AND " _
           &"RCVPRTNO <> '' THEN RTLessorCmtyLineDropHARDWARE.QTY ELSE 0 END), SUM(CASE WHEN RTLessorCmtyLineDropHARDWARE.dropdat IS NULL " _
           &"AND RCVPRTNO <> '' AND RTLessorCmtyLineDropHARDWARE.rcvfinishdat IS NOT NULL THEN RTLessorCmtyLineDropHARDWARE.QTY ELSE 0 END), " _
           &"SUM(CASE WHEN RTLessorCmtyLineDropHARDWARE.dropdat IS NULL AND RTLessorCmtyLineDropHARDWARE.QTY > 0 THEN RTLessorCmtyLineDropHARDWARE.QTY ELSE 0 END) - " _
           &"SUM(CASE WHEN RTLessorCmtyLineDropHARDWARE.dropdat IS NULL AND RCVPRTNO <> '' AND RTLessorCmtyLineDropHARDWARE.rcvfinishdat IS NOT NULL THEN RTLessorCmtyLineDropHARDWARE.QTY ELSE 0 END) " _
           &"FROM RTLessorCmtyLineDropSNDWORK LEFT OUTER JOIN RTObj RTObj_4 ON RTLessorCmtyLineDropSNDWORK.REALCONSIGNEE = RTObj_4.CUSID LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_2 INNER JOIN RTObj RTObj_3 ON RTEmployee_2.CUSID = RTObj_3.CUSID ON RTLessorCmtyLineDropSNDWORK.REALENGINEER = RTEmployee_2.EMPLY LEFT OUTER JOIN " _
           &"RTObj RTObj_2 ON RTLessorCmtyLineDropSNDWORK.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN " _
           &"RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON RTLessorCmtyLineDropSNDWORK.ASSIGNENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
           &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON RTLessorCmtyLineDropSNDWORK.PRTUSR = RTEmployee.EMPLY left outer join " _
           &"RTLessorCmtyLineDrop on RTLessorCmtyLineDropSNDWORK.comq1=RTLessorCmtyLineDrop.comq1 and " _
           &"RTLessorCmtyLineDropSNDWORK.lineq1=RTLessorCmtyLineDrop.lineq1 and " _
           &"RTLessorCmtyLineDropSNDWORK.entryno=RTLessorCmtyLineDrop.entryno LEFT OUTER JOIN " _
           &"RTLessorCmtyLineDropHARDWARE ON RTLessorCmtyLineDropSNDWORK.comq1 = RTLessorCmtyLineDropHARDWARE.comq1 AND " _
           &"RTLessorCmtyLineDropSNDWORK.lineq1 = RTLessorCmtyLineDropHARDWARE.lineq1 and " _
           &"RTLessorCmtyLineDropSNDWORK.entryno = RTLessorCmtyLineDropHARDWARE.entryno and " _
           &"RTLessorCmtyLineDropSNDWORK.PRTNO = RTLessorCmtyLineDropHARDWARE.PRTNO " _
           &"where RTLessorCmtyLineDropSNDWORK.comq1=" & aryparmkey(0) & " and RTLessorCmtyLineDropSNDWORK.lineq1=" & aryparmkey(1) & " and RTLessorCmtyLineDropSNDWORK.entryno=" & aryparmkey(2) & " and " & searchqry & " " _
           &"GROUP BY  RTLessorCmtyLineDropSNDWORK.comq1,RTLessorCmtyLineDropSNDWORK.lineq1,RTLessorCmtyLineDropSNDWORK.entryno, RTLessorCmtyLineDropSNDWORK.PRTNO, " _
           &"rtrim(CONVERT(char(6), RTLessorCmtyLineDrop.COMQ1)) + '-' + rtrim(CONVERT(char(6), RTLessorCmtyLineDrop.lineQ1)), " _
           &"RTLessorCmtyLineDropSNDWORK.PRTNO, RTLessorCmtyLineDropSNDWORK.SENDWORKDAT, RTOBJ.CUSNC, " _
           &"CASE WHEN RTOBJ_2.SHORTNC <> '' THEN RTOBJ_2.SHORTNC ELSE RTOBJ_1.CUSNC END, " _
           &"CASE WHEN RTOBJ_4.SHORTNC <> '' THEN RTOBJ_4.SHORTNC ELSE RTOBJ_3.CUSNC END, RTLessorCmtyLineDropSNDWORK.closedat, " _
           &"RTLessorCmtyLineDropSNDWORK.unclosedat, RTLessorCmtyLineDropSNDWORK.BONUSCLOSEYM, RTLessorCmtyLineDropSNDWORK.BONUSFINCHK, " _
           &"RTLessorCmtyLineDropSNDWORK.batchno, RTLessorCmtyLineDropSNDWORK.STOCKCLOSEYM, RTLessorCmtyLineDropSNDWORK.STOCKFINCHK, " _
           &"RTLessorCmtyLineDropSNDWORK.DROPDAT "

   'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>