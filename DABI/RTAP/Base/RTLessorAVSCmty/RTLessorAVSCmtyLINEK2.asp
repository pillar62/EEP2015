<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="AVS-City管理系統"
  title="AVS-City主線資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  functionOptName="主線派工;設備查詢;用戶維護;客服案件;到期續約;撤線作業;作　　廢;作廢返轉;歷史異動"
  functionOptProgram="RTLessorAVSCmtyLineSNDWORKK.asp;RTLessorAVSCmtyLINEHardwareK2.asp;RTLessorAVSCustK.asp;RTLessorAVSCmtyLineFAQK.asp;RTLessorAVSCmtyLineContK.asp;RTLessorAVSCmtyLineDROPK.asp;RTLessorAVSCmtyLineCANCEL.asp;RTLessorAVSCmtyLineCANCELRTN.asp;RTLessorAVSCmtyLineLOGK.asp"
  functionOptPrompt="N;N;N;N;N;N;Y;Y;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;轄區;社區名稱;主線;縣市;鄉鎮;none;主線IP;none;none;none;none;線路<br>ISP;IP<br>種類;主線速率;none;Reset電話;none;none;none;到位日;none;撤線日;作廢日;用戶;報竣;退租;計費"
  sqlDelete="SELECT RTLessorAVSCmtyLine.COMQ1, RTLessorAVSCmtyLine.LINEQ1,RTLessorAVSCmtyLine.salesid, RTLessorAVSCMTYH.COMN,rtrim(convert(char(6),RTLessorAVSCmtyLine.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorAVSCmtyLine.lineQ1)) , " _
                &"RTLessorAVSCMTYLINE.CUTNC,RTLessorAVSCMTYLINE.TOWNSHIP,RTLessorAVSCmtyLine.LINEGROUP,RTLessorAVSCmtyLine.LINEIP,RTLessorAVSCmtyLine.GATEWAY, " _
                &"RTLessorAVSCmtyLine.PPPOEACCOUNT, RTLessorAVSCmtyLine.PPPOEPASSWORD, RTLessorAVSCmtyLine.LINETEL, " _
                &"RTCode_1.CODENC, RTCode_3.CODENC AS Expr1, RTCode_2.CODENC AS Expr2, RTLessorAVSCmtyLine.IPCNT, RTReset.TEL, " _
                &"RTLessorAVSCmtyLine.RCVDAT, RTLessorAVSCmtyLine.INSPECTDAT, RTLessorAVSCmtyLine.HINETNOTIFYDAT, " _
                &"RTLessorAVSCmtyLine.HARDWAREDAT, RTLessorAVSCmtyLine.ADSLAPPLYDAT, " _
                &"RTLessorAVSCmtyLine.DROPDAT, RTLessorAVSCmtyLine.CANCELDAT, " _
                &"SUM(CASE WHEN RTLessorAVSCust.CANCELDAT IS NULL THEN 1 ELSE 0 END), " _
                &"SUM(CASE WHEN RTLessorAVSCust.CANCELDAT IS NULL AND RTLessorAVSCust.FINISHDAT IS NOT NULL THEN 1 ELSE 0 END), " _
                &"SUM(CASE WHEN RTLessorAVSCust.CANCELDAT IS NULL AND RTLessorAVSCust.FINISHDAT IS NOT NULL AND " _
                &"RTLessorAVSCust.DROPDAT IS NOT NULL THEN 1 ELSE 0 END), SUM(CASE WHEN RTLessorAVSCust.CANCELDAT IS NULL AND " _
                &"(RTLessorAVSCust.STRBILLINGDAT IS NOT NULL OR RTLessorAVSCust.NEWBILLINGDAT IS NOT NULL) AND " _
                &"RTLessorAVSCust.DROPDAT IS NULL THEN 1 ELSE 0 END) " _
                &"FROM    RTLessorAVSCmtyLine LEFT OUTER JOIN RTLessorAVSCust ON RTLessorAVSCmtyLine.COMQ1 = RTLessorAVSCust.COMQ1 AND " _
                &"RTLessorAVSCmtyLine.LINEQ1 = RTLessorAVSCust.LINEQ1 LEFT OUTER JOIN RTCode RTCode_3 ON " _
                &"RTLessorAVSCmtyLine.LINEIPTYPE = RTCode_3.CODE AND RTCode_3.KIND = 'M5' LEFT OUTER JOIN " _
                &"RTCode RTCode_1 ON RTLessorAVSCmtyLine.LINEISP = RTCode_1.CODE AND RTCode_1.KIND = 'C3' LEFT OUTER JOIN " _
                &"RTCode RTCode_2 ON RTLessorAVSCmtyLine.LINERATE = RTCode_2.CODE AND RTCode_2.KIND = 'D3' LEFT OUTER JOIN " _
                &"RTEmployee INNER JOIN RTObj RTObj_1 ON RTEmployee.CUSID = RTObj_1.CUSID ON " _
                &"RTLessorAVSCmtyLine.SALESID = RTEmployee.EMPLY LEFT OUTER JOIN RTObj RTObj_2 ON " _
                &"RTLessorAVSCmtyLine.CONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTLessorAVSCMTYH ON " _
                &"RTLessorAVSCmtyLine.COMQ1 = RTLessorAVSCMTYH.COMQ1 LEFT OUTER JOIN RTCOUNTY ON " _
                &"RTLessorAVSCMTYLINE.CUTID = RTCOUNTY.CUTID " _
				&"Left outer join RTReset on RTReset.comq1 = RTLessorAVSCmtyLine.comq1 and RTReset.lineq1 = RTLessorAVSCmtyLine.lineq1 and RTReset.cmtytype ='07' and RTReset.canceldat is null " _
                &"WHERE RTLessorAVSCmtyLine.COMQ1=0 " _
                &"GROUP BY  RTLessorAVSCmtyLine.COMQ1, RTLessorAVSCmtyLine.LINEQ1, RTLessorAVSCMTYH.COMN,rtrim(convert(char(6),RTLessorAVSCmtyLine.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorAVSCmtyLine.lineQ1)) ," _
                &"RTLessorAVSCMTYLINE.TOWNSHIP, RTLessorAVSCmtyLine.LINEGROUP, RTLessorAVSCmtyLine.LINEIP, " _
                &"RTLessorAVSCmtyLine.GATEWAY, RTLessorAVSCmtyLine.PPPOEACCOUNT, RTLessorAVSCmtyLine.PPPOEPASSWORD, " _
                &"RTLessorAVSCmtyLine.LINETEL, RTCode_1.CODENC, RTCode_3.CODENC, RTCode_2.CODENC, " _
                &"RTLessorAVSCmtyLine.IPCNT, RTLessorAVSCmtyLine.RCVDAT, RTLessorAVSCmtyLine.INSPECTDAT, " _
                &"RTLessorAVSCmtyLine.HINETNOTIFYDAT, RTLessorAVSCmtyLine.HARDWAREDAT, RTLessorAVSCmtyLine.ADSLAPPLYDAT, " _
                &"RTReset.Tel,RTLessorAVSCmtyLine.LINEDUEDAT, RTLessorAVSCmtyLine.DROPDAT, RTLessorAVSCmtyLine.CANCELDAT " _
                &"ORDER BY  RTLessorAVSCmtyLine.COMQ1, RTLessorAVSCmtyLine.LINEQ1 "

  dataTable="RTLessorAVSCmtyLine"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="RTLessorAVSCmtyLineD.asp"
  datawindowFeature=""
  searchWindowFeature="width=450,height=350,scrollbars=yes"
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
  searchProg="RTLessorAVSCmtylineS2.asp"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  set connXX=server.CreateObject("ADODB.connection")
  set rsXX=server.CreateObject("ADODB.recordset")
  dsnxx="DSN=RTLIB"
  sqlxx="select * from RTAreaSales where cusid='" & Emply & "' and areaid ='D0' "
  connxx.Open dsnxx
  rsxx.Open sqlxx,connxx
  if not rsxx.EOF then
     limitemply	=" and a.salesid ='" & Emply & "' "
  else
     limitemply =" " 
  end if
  rsxx.Close

  connxx.Close
  set rsxx=nothing
  set connxx=nothing
  '----
 
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" a.ComQ1<>0 "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  '讀取登入帳號之群組資料
  'Response.Write "GP=" & usergroup
  '-------------------------------------------------------------------------------------------
    sqlList="select a.comq1, a.lineq1, isnull(d.shortnc, f.cusnc), c.comn, convert(varchar(6), a.comq1) +'-'+ convert(varchar(6),a.lineq1), " &_
			"g.cutnc, a.township, a.linegroup,a.lineip,a.gateway, a.pppoeaccount, a.pppoepassword, a.linetel, " &_
			"substring(h.codenc,1,2), i.codenc, j.codenc, a.ipcnt, k.tel, a.rcvdat, a.inspectdat, a.hinetnotifydat, " &_
			"a.hardwaredat, a.adslapplydat, a.dropdat, a.canceldat, " &_
			"sum(case when b.cusid is not null and b.canceldat is null then 1 else 0 end), " &_
			"sum(case when b.cusid is not null and b.canceldat is null and b.finishdat is not null then 1 else 0 end), " &_
			"sum(case when b.cusid is not null and b.canceldat is null and b.finishdat is not null and b.dropdat is not null then 1 else 0 end), " &_
			"sum(case when b.cusid is not null and b.canceldat is null and (b.strbillingdat is not null or b.newbillingdat is not null) and b.dropdat is null then 1 else 0 end) " &_
			"from RTLessorAVSCmtyLine a " &_
			"left outer join RTLessorAvsCust b on a.comq1 = b.comq1 and a.lineq1 = b.lineq1 " &_
			"inner join RTLessorAvsCmtyH c on c.comq1 = a.comq1 " &_
			"left outer join RTObj d on a.consignee = d.cusid " &_
			"left outer join RTEmployee e inner join RTObj f on e.cusid = f.cusid on a.salesid = e.emply " &_
			"left outer join RTCounty g on a.cutid=g.cutid " &_
			"left outer join RTCode h on a.lineisp = h.code and h.kind = 'c3' " &_
			"left outer join RTCode i on a.lineiptype = i.code and i.kind = 'm5' " &_
			"left outer join RTCode j on a.linerate = j.code and j.kind = 'd3' " &_
			"left outer join RTReset k on k.comq1 = a.comq1 and k.lineq1 = a.lineq1 and k.cmtytype ='07' and k.canceldat is null " &_
			"where " & searchqry & limitemply &_
			" group by  a.comq1, a.lineq1, isnull(d.shortnc, f.cusnc), c.comn, convert(varchar(6), a.comq1) +'-'+ convert(varchar(6),a.lineq1), " &_
			"g.cutnc, a.township, a.linegroup,a.lineip,a.gateway, a.pppoeaccount, a.pppoepassword, a.linetel, " &_
			"substring(h.codenc,1,2), i.codenc, j.codenc, a.ipcnt, k.tel, a.rcvdat, a.inspectdat, a.hinetnotifydat, " &_
			"a.hardwaredat, a.adslapplydat, a.dropdat, a.canceldat " &_
            "ORDER BY  a.COMQ1, a.LINEQ1 "
 'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>