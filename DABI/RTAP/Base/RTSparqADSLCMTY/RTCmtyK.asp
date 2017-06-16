<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="Sparq* 管理系統"
  title="速博ADSL社區及客戶資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="網路流量;客　戶;管委會;合　約"
  functionOptProgram="RTCMTYFLOW.ASP;RTCustK.asp;RTCmtySpK.asp;RTContractK.asp"
  functionOptPrompt="N;N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;業務轄區;社區名稱;none;none;IP;設備位置;none;測通日;撤銷日;申請;完工;報峻;退租;欠拆;有效" 
  sqlDelete="SELECT RTSparqAdslCmty.CUTYID, RTSparqAdslCmty.COMN, RTSparqAdslCmty.cmtytel, RTSparqAdslCmty.HBNO, " _
           &"RTSparqAdslCmty.IPADDR, IsNull(RTCounty.CUTNC,'')+RTSparqAdslCmty.TOWNSHIP+RTSparqAdslCmty.ADDR, " _
           &"rtsparqadslcmtysndwork.prtno, RTSparqAdslCmty.ADSLAPPLY, RTSparqAdslCmty.RCOMDROP, " _
           &"SUM(CASE WHEN rtsparqadslcust.cusid IS NOT NULL OR rtsparqadslcust.CUSID <> '' THEN 1 ELSE 0 END), " _
           &"SUM(CASE WHEN rtsparqadslcust.finishdat IS NOT NULL OR rtsparqadslcust.finishdat <> '' THEN 1 ELSE 0 END), " _           
           &"SUM(CASE WHEN rtsparqadslcust.docketdat IS NOT NULL OR rtsparqadslcust.docketdat <> '' THEN 1 ELSE 0 END) " _
           &"FROM RTSparqAdslCmty LEFT OUTER JOIN rtsparqadslcust ON RTSparqAdslCmty.CUTYID = rtsparqadslcust.COMQ1 " _
           &"LEFT OUTER JOIN rtsparqadslcmtysndwork ON rtsparqadslcmty.cutyid = rtsparqadslcmtysndwork.cutyid AND " _
           &"rtsparqadslcmtysndwork.dropdat IS NULL AND rtsparqadslcmtysndwork.unclosedat IS NULL " _
           &"LEFT OUTER JOIN RTCounty ON RTSparqAdslCmty.CUTID = RTCounty.CUTID " _
           &"WHERE (RTSparqAdslCmty.COMN <> '*') " _
           &"GROUP BY  RTSparqAdslCmty.CUTYID, RTSparqAdslCmty.COMN, RTSparqAdslCmty.cmtytel, RTSparqAdslCmty.HBNO, " _
           &"RTSparqAdslCmty.IPADDR, IsNull(RTCounty.CUTNC,'')+RTSparqAdslCmty.TOWNSHIP+RTSparqAdslCmty.ADDR,rtsparqadslcmtysndwork.prtno, " _
           &"RTSparqAdslCmty.ADSLAPPLY, RTSparqAdslCmty.RCOMDROP " _
           &"ORDER BY  RTSparqAdslCmty.equipaddr "
  dataTable="RTSparqAdslCmty"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="RTCmtyD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=FALSE
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="RTCmtyS.asp"
  searchFirst=TRUE
  If searchQry="" Then
    'searchQry=" RTSparqAdslCmty.cutyid =0  "
    'searchShow=""
	'修改A
    if ARYPARMKEY(0) ="" then 
	    searchQry=" RTSparqAdslCmty.cutyid =0  "
		searchShow=""
	else
		searchQry=" RTSparqAdslCmty.cutyid=" & aryparmkey(0)
		searchShow="社區序號︰"& aryparmkey(0)
	    searchFirst=FALSE
	end if		
    
  ELSE
     SEARCHFIRST=FALSE
  End If

  'userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  set connXX=server.CreateObject("ADODB.connection")
  set rsXX=server.CreateObject("ADODB.recordset")
  dsnxx="DSN=RTLIB"
  sqlxx="select * from RTAreaSales where cusid='" & Emply & "' and areaid ='D0' "
  connxx.Open dsnxx
  rsxx.Open sqlxx,connxx
  if not rsxx.EOF then
     limitemply	=" and RTSparqAdslCmty.bussid ='" & Emply & "' "
  else
     limitemply =" " 
  end if
  rsxx.Close
  connxx.Close
  set rsxx=nothing
  set connxx=nothing
  '-------------------------------------------------------------------------------------------

  '業務工程師只能讀取該工程師的社區
sqllist="SELECT RTSparqAdslCmty.CUTYID, " _
       &"isnull(RTOBJ.shortnc, isnull(rtobj_a.cusnc,'無歸屬')), " _
       &"RTSparqAdslCmty.COMN, RTSparqAdslCmty.cmtytel, RTSparqAdslCmty.HBNO, " _
       &"RTSparqAdslCmty.IPADDR, substring(IsNull(RTCounty.CUTNC,'')+RTSparqAdslCmty.TOWNSHIP+RTSparqAdslCmty.ADDR,1,25)+'....', " _
       &"rtsparqadslcmtysndwork.prtno, RTSparqAdslCmty.ADSLAPPLY, RTSparqAdslCmty.RCOMDROP, " _
       &"SUM(CASE WHEN rtsparqadslcust.cusid IS NOT NULL OR rtsparqadslcust.cusid <> '' THEN 1 ELSE 0 END), " _ 
       &"SUM(CASE WHEN rtsparqadslcust.finishdat IS NOT NULL OR rtsparqadslcust.finishdat <> '' THEN 1 ELSE 0 END), " _           
       &"SUM(CASE WHEN rtsparqadslcust.docketdat IS NOT NULL OR rtsparqadslcust.docketdat <> '' THEN 1 ELSE 0 END), " _     
       &"SUM(CASE WHEN (rtsparqadslcust.docketdat IS NOT NULL OR rtsparqadslcust.docketdat <> '') AND RTSPARQADSLCUST.DROPDAT IS NOT NULL AND RTSPARQADSLCUST.OVERDUE<>'Y' THEN 1 ELSE 0 END), " _     
       &"SUM(CASE WHEN (rtsparqadslcust.docketdat IS NOT NULL OR rtsparqadslcust.docketdat <> '') AND RTSPARQADSLCUST.DROPDAT IS NOT NULL AND RTSPARQADSLCUST.OVERDUE='Y' THEN 1 ELSE 0 END), " _     
       &"SUM(CASE WHEN rtsparqadslcust.docketdat IS NOT NULL OR rtsparqadslcust.docketdat <> '' THEN 1 ELSE 0 END) - " _     
       &"SUM(CASE WHEN (rtsparqadslcust.docketdat IS NOT NULL OR rtsparqadslcust.docketdat <> '') AND RTSPARQADSLCUST.DROPDAT IS NOT NULL AND RTSPARQADSLCUST.OVERDUE<>'Y' THEN 1 ELSE 0 END) - " _     
       &"SUM(CASE WHEN (rtsparqadslcust.docketdat IS NOT NULL OR rtsparqadslcust.docketdat <> '') AND RTSPARQADSLCUST.DROPDAT IS NOT NULL AND RTSPARQADSLCUST.OVERDUE='Y' THEN 1 ELSE 0 END) " _
       &"FROM RTSparqAdslCmty left outer join rtsparqadslcust ON RTSparqAdslCmty.CUTYID = rtsparqadslcust.COMQ1 " _
       &"LEFT OUTER JOIN rtsparqadslcmtysndwork ON rtsparqadslcmty.cutyid = rtsparqadslcmtysndwork.cutyid AND " _
       &"rtsparqadslcmtysndwork.dropdat IS NULL AND rtsparqadslcmtysndwork.unclosedat IS NULL " _
       &"LEFT OUTER JOIN RTCounty ON RTSparqAdslCmty.CUTID = RTCounty.CUTID " _       
       &"LEFT OUTER JOIN RTObj ON RTSparqAdslCmty.CONSIGNEE = RTObj.CUSID " _
       &"LEFT OUTER JOIN RTEmployee inner join RTObj rtobj_a ON rtobj_a.cusid=RTEmployee.cusid on RTEmployee.emply = RTSparqAdslCmty.bussid " _
       &"left outer join rtctytown on RTSparqAdslCust.cutid2=rtctytown.cutid and RTSparqAdslCust.township2=rtctytown.township " _
       &"where " & searchqry & " " & limitemply _
       &" GROUP BY  RTSparqAdslCmty.CUTYID, " _
       &"isnull(RTOBJ.shortnc, isnull(rtobj_a.cusnc,'無歸屬')), " _
       &"RTSparqAdslCmty.COMN, RTSparqAdslCmty.cmtytel, RTSparqAdslCmty.HBNO, " _
       &"RTSparqAdslCmty.IPADDR, substring(IsNull(RTCounty.CUTNC,'')+RTSparqAdslCmty.TOWNSHIP+RTSparqAdslCmty.ADDR,1,25)+'....',rtsparqadslcmtysndwork.prtno, " _
       &"RTSparqAdslCmty.ADSLAPPLY, RTSparqAdslCmty.RCOMDROP " _
       &"ORDER BY  RTSparqAdslCmty.comn "
 'end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>