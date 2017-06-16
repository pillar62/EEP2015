<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="速博499管理系統"
  title="速博499主線資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=v(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
'  IF USERLEVEL=31 OR UCASE(EMPLY)="T91129" OR UCASE(EMPLY)="P92010" or Ucase(emply)="T94180" THEN  
'     functionOptName="派工查詢;設備查詢;用戶維護;報竣異動;清除申請;返轉申請;申請記錄;主線作廢;作廢返轉;歷史異動"
'     functionOptProgram="RTSparq499cmtylineSNDWORKk2.asp;RTSparq499cmtyhardwareK2.asp;RTSparq499custK.asp;RTSparq499cmtylineCHGK.asp;RTSparq499cmtylineCLRPRTNO.asp;RTSparq499cmtylineCLRPRTNOC.asp;RTSparq499cmtylineAPPLYLOGK.asp;RTSparq499cmtylineLOGK.asp"
'     functionOptPrompt="N;N;N;N;Y;Y;N;Y;Y;Y;Y;N"
'  ELSE
     functionOptName="用戶維護;派工查詢"
     functionOptProgram="RTSparq499custK.asp;RTSparq499CmtyLineSndWorkK.asp"
     functionOptPrompt="N;N"
'  END IF
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;社區名稱;主線;線路IP;附掛電話;主線頻寬;none;連接方式;申請日;速博核准日;測通日;速博開通日;用戶;報竣;退租;欠拆;有效;none;進度"
  sqlDelete="SELECT RTSparq499cmtyLINE.COMQ1, RTSparq499cmtyLINE.LINEQ1,RTSparq499cmtyH.COMN,rtrim(convert(char(6),RTSparq499cmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTSparq499cmtyline.lineQ1))  as comqline, " _
           &"CONVERT(CHAR,RTSparq499cmtyLINE.LINEIPSTR1)+'.'+CONVERT(CHAR,RTSparq499cmtyLINE.LINEIPSTR2)+'.'+CONVERT(CHAR,RTSparq499cmtyLINE.LINEIPSTR3)+'.'+CONVERT(CHAR,RTSparq499cmtyLINE.LINEIPSTR4)+'-'+CONVERT(CHAR,RTSparq499cmtyLINE.LINEIPEND),RTSparq499cmtyLINE.LINETEL, " _
           &"rtcode.codenc, " _
           &"RTSparq499cmtyLINE.RCVDAT," _
           &"RTSparq499cmtyLINE.AGREE, " _
           &"RTSparq499cmtyLINE.ADSLAPPLYDAT, " _
           &"RTSparq499cmtyLINE.NCICAGREEDAT, " _
           &"RTSparq499cmtyLINE.ADSLOPENDAT,RTSparq499cmtyLINE.NCICOPENDAT, " _
           &"SUM(CASE WHEN RTSparq499cust.cusid IS NOT NULL THEN 1 ELSE 0 END) AS CUSCNT, " _
           &"SUM(CASE WHEN RTSparq499cust.transdat IS NOT NULL THEN 1 ELSE 0 END) AS APPLYCNT, " _
           &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END AS DEVPMAN,  " _
           &"case  WHEN RTSparq499cmtyLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTSparq499cmtyLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTSparq499cmtyLINE.NCICOPENDAT IS NOT NULL THEN '速博開通' when RTSparq499cmtyLINE.ADSLOPENDAT is not null then '主線測通' " _
           &"when RTSparq499cmtyLINE.EQUIPARRIVE is not null then '設備到位' WHEN RTSparq499cmtyLINE.BOXARRIVE IS NOT NULL THEN '機櫃到位' " _
           &"WHEN RTSparq499cmtyLINE.LINEARRIVEDAT IS NOT NULL  then '線路到位' when RTSparq499cmtyLINE.TOCHTWORKING IS NOT NULL  then '至營運處' WHEN RTSparq499cmtyLINE.NCICAGREEDAT IS NOT NULL THEN '速博核准' " _
           &"WHEN RTSparq499cmtyLINE.ADSLAPPLYDAT IS NOT NULL  then '主線申請中' " _
           &"WHEN RTSparq499cmtyLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTSparq499cmtyLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTSparq499cmtyLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END  " _
           &"FROM RTSalesGroup RIGHT OUTER JOIN " _
           &"RTSparq499cmtyLINE ON RTSalesGroup.AREAID = RTSparq499cmtyLINE.AREAID AND " _
           &"RTSalesGroup.GROUPID = RTSparq499cmtyLINE.GROUPID AND " _
           &"RTSalesGroup.EDATE IS NULL LEFT OUTER JOIN " _
           &"RTCounty ON RTSparq499cmtyLINE.CUTID = RTCounty.CUTID LEFT OUTER JOIN " _
           &"RTObj ON RTSparq499cmtyLINE.CONSIGNEE = RTObj.CUSID LEFT OUTER JOIN " _
           &"RTSparq499cust ON RTSparq499cmtyLINE.COMQ1 = RTSparq499cust.COMQ1 AND " _
           &"RTSparq499cmtyLINE.LINEQ1 = RTSparq499cust.LINEQ1  inner join RTSparq499cmtyh on RTSparq499cmtyline.comq1=RTSparq499cmtyh.comq1 LEFT OUTER JOIN RTCODE ON RTSparq499cmtyline.LINERATE=RTCODE.CODE AND RTCODE.KIND='D3' " _
           &"WHERE RTSparq499cmtyLINE.COMQ1= 0 " _                
           &"GROUP BY RTSparq499cmtyLINE.COMQ1, RTSparq499cmtyLINE.LINEQ1,RTSparq499cmtyH.COMN, rtrim(convert(char(6),RTSparq499cmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTSparq499cmtyline.lineQ1)) , " _
           &"RTSalesGroup.GROUPNC, RTSparq499cmtyLINE.LINEIPSTR1+'.'+RTSparq499cmtyLINE.LINEIPSTR2+'.'+RTSparq499cmtyLINE.LINEIPSTR3+'.'+RTSparq499cmtyLINE.LINEIPSTR4+'-'+RTSparq499cmtyLINE.LINEIPEND,RTSparq499cmtyLINE.LINETEL, " _
           &"rtcode.codenc, " _
           &"RTSparq499cmtyLINE.RCVDAT," _
           &"RTSparq499cmtyLINE.AGREE, " _
           &"RTSparq499cmtyLINE.ADSLAPPLYDAT, " _
           &"RTSparq499cmtyLINE.NCICAGREEDAT, " _
           &"RTSparq499cmtyLINE.ADSLOPENDAT,RTSparq499cmtyLINE.NCICOPENDAT,case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END, " _
           &"case  WHEN RTSparq499cmtyLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTSparq499cmtyLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTSparq499cmtyLINE.NCICOPENDAT IS NOT NULL THEN '速博開通' when RTSparq499cmtyLINE.ADSLOPENDAT is not null then '主線測通' " _
           &"when RTSparq499cmtyLINE.EQUIPARRIVE is not null then '設備到位' WHEN RTSparq499cmtyLINE.BOXARRIVE IS NOT NULL THEN '機櫃到位' " _
           &"WHEN RTSparq499cmtyLINE.LINEARRIVEDAT IS NOT NULL  then '線路到位' when RTSparq499cmtyLINE.TOCHTWORKING IS NOT NULL  then '至營運處' WHEN RTSparq499cmtyLINE.NCICAGREEDAT IS NOT NULL THEN '速博核准' " _
           &"WHEN RTSparq499cmtyLINE.ADSLAPPLYDAT IS NOT NULL  then '主線申請中' " _
           &"WHEN RTSparq499cmtyLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTSparq499cmtyLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTSparq499cmtyLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END  " 
           

  dataTable="RTSparq499cmtyline"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="RTSparq499cmtylineD.asp"
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
  sqlYY="select * from RTSparq499cmtyH LEFT OUTER JOIN RTCOUNTY ON RTSparq499cmtyH.CUTID=RTCOUNTY.CUTID where COMQ1=" & ARYPARMKEY(0)
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
     'searchQry=" RTSparq499cmtyline.ComQ1=" & aryparmkey(0)
     'searchShow="社區序號︰"& aryparmkey(0) & ",社區名稱︰" & COMN & ",社區地址︰" & COMADDR
	'修改A 
    if ARYPARMKEY(1) ="" then 
		searchQry=" RTSparq499cmtyline.ComQ1=" & aryparmkey(0)
		searchShow="社區序號︰"& aryparmkey(0) & ",社區名稱︰" & COMN & ",社區地址︰" & COMADDR
	else
		searchQry=" RTSparq499cmtyline.ComQ1=" & aryparmkey(0) & " and RTSparq499cmtyline.Lineq1 =" &aryparmkey(1)
		searchShow="主線序號︰"& aryparmkey(0)&"-"&aryparmkey(1) & ",社區名稱︰" & COMN & ",社區地址︰" & COMADDR
	end if		
	    
  ELSE
     SEARCHFIRST=FALSE
  End If
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  '讀取登入帳號之群組資料
  'Response.Write "GP=" & usergroup
  '-------------------------------------------------------------------------------------------
  'userlevel=2:為業務工程師==>只能看所屬社區資料
  'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
 ' Response.Write "DOMAIN=" & domain & "<BR>"
  
  sqlList="SELECT RTSparq499cmtyLINE.COMQ1, RTSparq499cmtyLINE.LINEQ1,RTSparq499cmtyH.COMN, rtrim(convert(char(6),RTSparq499cmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTSparq499cmtyline.lineQ1))  as comqline, " _
    &"CONVERT(CHAR,RTSparq499cmtyLINE.LINEIPSTR1)+'.'+CONVERT(CHAR,RTSparq499cmtyLINE.LINEIPSTR2)+'.'+CONVERT(CHAR,RTSparq499cmtyLINE.LINEIPSTR3)+'.'+CONVERT(CHAR,RTSparq499cmtyLINE.LINEIPSTR4)+'-'+CONVERT(CHAR,RTSparq499cmtyLINE.LINEIPEND),RTSparq499cmtyLINE.LINETEL, " _
    &"rtcode.codenc, " _
    &"RTSparq499cmtyLINE.RCVDAT," _
    &"RTCodeG5.codenc, " _
    &"RTSparq499cmtyLINE.ADSLAPPLYDAT, " _
    &"RTSparq499cmtyLINE.NCICAGREEDAT, " _
    &"RTSparq499cmtyLINE.ADSLOPENDAT,RTSparq499cmtyLINE.NCICOPENDAT, " _
    &"SUM(CASE WHEN RTSparq499cust.cusid IS NOT NULL AND RTSparq499cust.CANCELDAT IS NULL THEN 1 ELSE 0 END) AS CUSCNT, " _
    &"SUM(CASE WHEN RTSparq499cust.DOCKETDAT IS NOT NULL THEN 1 ELSE 0 END) AS APPLYCNT, " _
    &"SUM(CASE WHEN RTSparq499cust.DOCKETdat IS NOT NULL AND RTSparq499cust.DROPDAT IS NOT NULL AND RTSparq499cust.OVERDUE <> 'Y' THEN 1 ELSE 0 END) ," _
    &"SUM(CASE WHEN RTSparq499cust.DOCKETdat IS NOT NULL AND RTSparq499cust.DROPDAT IS NOT NULL AND RTSparq499cust.OVERDUE = 'Y' THEN 1 ELSE 0 END) ," _
    &"SUM(CASE WHEN RTSparq499cust.DOCKETdat IS NOT NULL THEN 1 ELSE 0 END) - SUM(CASE WHEN RTSparq499cust.DOCKETdat IS NOT NULL AND RTSparq499cust.DROPDAT IS NOT NULL AND RTSparq499cust.OVERDUE <> 'Y' THEN 1 ELSE 0 END) - SUM(CASE WHEN RTSparq499cust.DOCKETdat IS NOT NULL AND RTSparq499cust.DROPDAT IS NOT NULL AND RTSparq499cust.OVERDUE = 'Y' THEN 1 ELSE 0 END) , " _
    &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END AS DEVPMAN, " _
    &"case  WHEN RTSparq499cmtyLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTSparq499cmtyLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTSparq499cmtyLINE.NCICOPENDAT IS NOT NULL THEN '速博開通' when RTSparq499cmtyLINE.ADSLOPENDAT is not null then '主線測通' " _
    &"when RTSparq499cmtyLINE.EQUIPARRIVE is not null then '設備到位' WHEN RTSparq499cmtyLINE.BOXARRIVE IS NOT NULL THEN '機櫃到位' " _
    &"WHEN RTSparq499cmtyLINE.LINEARRIVEDAT IS NOT NULL  then '線路到位' when RTSparq499cmtyLINE.TOCHTWORKING IS NOT NULL  then '至營運處' WHEN RTSparq499cmtyLINE.NCICAGREEDAT IS NOT NULL THEN '速博核准' " _
    &"WHEN RTSparq499cmtyLINE.ADSLAPPLYDAT IS NOT NULL  then '主線申請中' " _
    &"WHEN RTSparq499cmtyLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTSparq499cmtyLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTSparq499cmtyLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END  " _
    &"FROM RTSalesGroup RIGHT OUTER JOIN " _
    &"RTSparq499cmtyLINE ON RTSalesGroup.AREAID = RTSparq499cmtyLINE.AREAID AND " _
    &"RTSalesGroup.GROUPID = RTSparq499cmtyLINE.GROUPID AND " _
    &"RTSalesGroup.EDATE IS NULL LEFT OUTER JOIN " _
    &"RTCounty ON RTSparq499cmtyLINE.CUTID = RTCounty.CUTID LEFT OUTER JOIN " _
    &"RTObj ON RTSparq499cmtyLINE.CONSIGNEE = RTObj.CUSID LEFT OUTER JOIN " _
    &"RTSparq499cust ON RTSparq499cmtyLINE.COMQ1 = RTSparq499cust.COMQ1 AND " _
    &"RTSparq499cmtyLINE.LINEQ1 = RTSparq499cust.LINEQ1  inner join RTSparq499cmtyh on RTSparq499cmtyline.comq1=RTSparq499cmtyh.comq1 LEFT OUTER JOIN RTCODE ON RTSparq499cmtyline.LINERATE=RTCODE.CODE AND RTCODE.KIND='D3' " _
	&"left outer join RTCode RTCodeG5 on RTCodeG5.code = RTSparq499cmtyLINE.connecttype and RTCodeG5.KIND='G5' " _    
    &"WHERE RTSparq499cmtyLINE.COMQ1<> 0 AND " & SEARCHQRY & " " _
    &"GROUP BY RTSparq499cmtyLINE.COMQ1, RTSparq499cmtyLINE.LINEQ1,RTSparq499cmtyH.COMN, rtrim(convert(char(6),RTSparq499cmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTSparq499cmtyline.lineQ1)) , " _
    &"RTSalesGroup.GROUPNC, CONVERT(CHAR,RTSparq499cmtyLINE.LINEIPSTR1)+'.'+CONVERT(CHAR,RTSparq499cmtyLINE.LINEIPSTR2)+'.'+CONVERT(CHAR,RTSparq499cmtyLINE.LINEIPSTR3)+'.'+CONVERT(CHAR,RTSparq499cmtyLINE.LINEIPSTR4)+'-'+CONVERT(CHAR,RTSparq499cmtyLINE.LINEIPEND),RTSparq499cmtyLINE.LINETEL, " _
    &"rtcode.codenc, " _
    &"RTSparq499cmtyLINE.RCVDAT," _
    &"RTCodeG5.codenc, " _
    &"RTSparq499cmtyLINE.ADSLAPPLYDAT, " _
    &"RTSparq499cmtyLINE.NCICAGREEDAT, " _
    &"RTSparq499cmtyLINE.ADSLOPENDAT,RTSparq499cmtyLINE.NCICOPENDAT,case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END, " _
    &"case  WHEN RTSparq499cmtyLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTSparq499cmtyLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTSparq499cmtyLINE.NCICOPENDAT IS NOT NULL THEN '速博開通' when RTSparq499cmtyLINE.ADSLOPENDAT is not null then '主線測通' " _
    &"when RTSparq499cmtyLINE.EQUIPARRIVE is not null then '設備到位' WHEN RTSparq499cmtyLINE.BOXARRIVE IS NOT NULL THEN '機櫃到位' " _
    &"WHEN RTSparq499cmtyLINE.LINEARRIVEDAT IS NOT NULL  then '線路到位' when RTSparq499cmtyLINE.TOCHTWORKING IS NOT NULL  then '至營運處' WHEN RTSparq499cmtyLINE.NCICAGREEDAT IS NOT NULL THEN '速博核准' " _
    &"WHEN RTSparq499cmtyLINE.ADSLAPPLYDAT IS NOT NULL  then '主線申請中' " _
    &"WHEN RTSparq499cmtyLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTSparq499cmtyLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTSparq499cmtyLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END  "
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>