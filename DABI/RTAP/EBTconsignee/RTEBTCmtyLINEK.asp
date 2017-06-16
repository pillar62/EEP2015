<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="東森AVS管理系統"
  title="AVS主線資料查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  'functionOptName="主線進度;用戶維護;歷史異動"
  'functionOptProgram="rtEBTcmtylineschK.asp;rtebtcustK.asp;rtEBTcmtylineLOGK.asp"
  functionOptName="用戶查詢"
  functionOptProgram="rtebtcustK.asp"
  functionOptPrompt="N"
  accessMode="I"
  DSN="DSN=RTLib"
  formatName="none;none;社區名稱;主線;線路IP;附掛電話;聯單號碼;none;可建置;申請日;EBT回覆日;CHT通知日;測通日;用戶;報竣;none;進度"
  sqlDelete="SELECT RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,RTEBTCMTYH.COMN, rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1))  as comqline, " _
           &"RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, " _
           &"RTEBTCMTYLINE.RCVDAT," _
           &"RTEBTCMTYLINE.AGREE, " _
           &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, " _
           &"RTEBTCMTYLINE.HINETNOTIFYDAT, " _
           &"RTEBTCMTYLINE.ADSLAPPLYDAT, " _
           &"SUM(CASE WHEN rtebtcust.cusid IS NOT NULL THEN 1 ELSE 0 END) AS CUSCNT, " _
           &"SUM(CASE WHEN rtebtcust.transdat IS NOT NULL THEN 1 ELSE 0 END) AS APPLYCNT, " _
           &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END AS DEVPMAN,  " _
           &"case when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null then '未報竣' " _
           &"when RTEBTCMTYLINE.HINETNOTIFYDAT is not null then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL THEN '取得預定施工日' " _
           &"WHEN RTEBTCMTYLINE.linetel <> '' then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' " _
           &"WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTEBTCMTYLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END " _
           &"FROM RTSalesGroup RIGHT OUTER JOIN " _
           &"RTEBTCMTYLINE ON RTSalesGroup.AREAID = RTEBTCMTYLINE.AREAID AND " _
           &"RTSalesGroup.GROUPID = RTEBTCMTYLINE.GROUPID AND " _
           &"RTSalesGroup.EDATE IS NULL LEFT OUTER JOIN " _
           &"RTCounty ON RTEBTCMTYLINE.CUTID = RTCounty.CUTID LEFT OUTER JOIN " _
           &"RTObj ON RTEBTCMTYLINE.CONSIGNEE = RTObj.CUSID LEFT OUTER JOIN " _
           &"RTEBTCUST ON RTEBTCMTYLINE.COMQ1 = RTEBTCUST.COMQ1 AND " _
           &"RTEBTCMTYLINE.LINEQ1 = RTEBTCUST.LINEQ1  inner join rtebtcmtyh on rtebtcmtyline.comq1=rtebtcmtyh.comq1 " _
           &"WHERE RTEBTCMTYLINE.COMQ1= 0 " _                
           &"GROUP BY RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,RTEBTCMTYH.COMN, rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1)) , " _
           &"RTSalesGroup.GROUPNC, RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, " _
           &"RTEBTCMTYLINE.RCVDAT," _
           &"RTEBTCMTYLINE.AGREE, " _
           &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, " _
           &"RTEBTCMTYLINE.HINETNOTIFYDAT, " _
           &"RTEBTCMTYLINE.ADSLAPPLYDAT,case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END " _
           &"case when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null then '未報竣' " _
           &"when RTEBTCMTYLINE.HINETNOTIFYDAT is not null then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL THEN '取得預定施工日' " _
           &"WHEN RTEBTCMTYLINE.linetel <> '' then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' " _
           &"WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTEBTCMTYLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END " 
           

  dataTable="rtebtcmtyline"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="RTebtCmtylineD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
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
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  '----
  set connYY=server.CreateObject("ADODB.connection")
  set rsYY=server.CreateObject("ADODB.recordset")
  dsnYY="DSN=RTLIB"
  sqlYY="select * from RTEBTCMTYH LEFT OUTER JOIN RTCOUNTY ON RTEBTCMTYH.CUTID=RTCOUNTY.CUTID where COMQ1=" & ARYPARMKEY(0)
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
     COMADDR=RSYY("CUTNC") & RSYY("TOWNSHIP") & RSYY("RADDR")
  else
     COMN=""
     COMADDR=""
  end if
  rsYY.Close
  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTEBTCmtyline.ComQ1=" & aryparmkey(0)
     searchShow="社區序號︰"& aryparmkey(0) & ",社區名稱︰" & COMN & ",社區地址︰" & COMADDR
  ELSE
     SEARCHFIRST=FALSE
  End If
  sqlList="SELECT RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,RTEBTCMTYH.COMN, rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1))  as comqline, " _
           &"RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, " _
           &"RTEBTCMTYLINE.RCVDAT," _
           &"RTEBTCMTYLINE.AGREE, " _
           &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, " _
           &"RTEBTCMTYLINE.HINETNOTIFYDAT, " _
           &"RTEBTCMTYLINE.ADSLAPPLYDAT, " _
           &"SUM(CASE WHEN rtebtcust.cusid IS NOT NULL THEN 1 ELSE 0 END) AS CUSCNT, " _
           &"SUM(CASE WHEN rtebtcust.transdat IS NOT NULL THEN 1 ELSE 0 END) AS APPLYCNT, " _
           &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END AS DEVPMAN, " _
           &"case when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null then '未報竣' " _
           &"when RTEBTCMTYLINE.HINETNOTIFYDAT is not null then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL THEN '取得預定施工日' " _
           &"WHEN RTEBTCMTYLINE.linetel <> '' then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' " _
           &"WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTEBTCMTYLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END " _
           &"FROM RTSalesGroup RIGHT OUTER JOIN " _
           &"RTEBTCMTYLINE ON RTSalesGroup.AREAID = RTEBTCMTYLINE.AREAID AND " _
           &"RTSalesGroup.GROUPID = RTEBTCMTYLINE.GROUPID AND " _
           &"RTSalesGroup.EDATE IS NULL LEFT OUTER JOIN " _
           &"RTCounty ON RTEBTCMTYLINE.CUTID = RTCounty.CUTID LEFT OUTER JOIN " _
           &"RTObj ON RTEBTCMTYLINE.CONSIGNEE = RTObj.CUSID LEFT OUTER JOIN " _
           &"RTEBTCUST ON RTEBTCMTYLINE.COMQ1 = RTEBTCUST.COMQ1 AND " _
           &"RTEBTCMTYLINE.LINEQ1 = RTEBTCUST.LINEQ1  inner join rtebtcmtyh on rtebtcmtyline.comq1=rtebtcmtyh.comq1 " _
           &"WHERE RTEBTCMTYLINE.COMQ1<> 0 AND " & SEARCHQRY & " " _
           &"GROUP BY RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,RTEBTCMTYH.COMN, rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1)) , " _
           &"RTSalesGroup.GROUPNC, RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, " _
           &"RTEBTCMTYLINE.RCVDAT," _
           &"RTEBTCMTYLINE.AGREE, " _
           &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, " _
           &"RTEBTCMTYLINE.HINETNOTIFYDAT, " _
           &"RTEBTCMTYLINE.ADSLAPPLYDAT,case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END, " _
           &"case when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null then '未報竣' " _
           &"when RTEBTCMTYLINE.HINETNOTIFYDAT is not null then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL THEN '取得預定施工日' " _
           &"WHEN RTEBTCMTYLINE.linetel <> '' then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' " _
           &"WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTEBTCMTYLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END " 
           
                       
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>