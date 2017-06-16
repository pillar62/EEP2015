<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="東森AVS管理系統"
  title="AVS主線資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  IF USERLEVEL=31 OR UCASE(EMPLY)="T94200" OR UCASE(EMPLY)="T92134" OR Ucase(emply)="T95222" or Ucase(emply)="T94182" THEN  
     functionOptName="派工查詢;設備查詢;用戶維護;報竣異動;清除申請;返轉申請;申請記錄;主線鎖定;主線解鎖;主線作廢;作廢返轉;歷史異動"
     functionOptProgram="rtebtCMTYlineSNDWORKk2.asp;rtebtcmtyhardwareK2.asp;rtebtcustK.asp;rtEBTcmtylineCHGK.asp;rtEBTcmtylineCLRPRTNO.asp;rtEBTcmtylineCLRPRTNOC.asp;rtEBTcmtylineAPPLYLOGK.asp;rtEBTcmtylineLOCK.asp;rtEBTcmtylineUNLOCK.asp;rtEBTcmtylineLOGK.asp"
     functionOptPrompt="N;N;N;N;Y;Y;N;Y;Y;Y;Y;N"
  ELSE
     functionOptName="派工查詢;設備查詢;用戶維護;報竣異動;清除申請;返轉申請;申請記錄;歷史異動"
     functionOptProgram="rtebtCMTYlineSNDWORKk2.asp;rtebtcmtyhardwareK2.asp;rtebtcustK.asp;rtEBTcmtylineCHGK.asp;rtEBTcmtylineCLRPRTNO.asp;rtEBTcmtylineCLRPRTNOC.asp;rtEBTcmtylineAPPLYLOGK.asp;rtEBTcmtylineLOGK.asp"
     functionOptPrompt="N;N;N;N;Y;Y;N;N"
  END IF
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;社區名稱;主線;線路IP;附掛電話;none;主線頻寬;none;可建置;申請日;none;CHT通知日;測通日;用戶;報竣;退租;欠拆;有效;none;進度;鎖定"
  sqlDelete="SELECT RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,RTEBTCMTYH.COMN,rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1))  as comqline, " _
           &"RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, " _
           &"rtcode.codenc, " _
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
           &"WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTEBTCMTYLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END,case when RTEBTCMTYLINE.lockdat is null then '' else 'Y' END  " _
           &"FROM RTSalesGroup RIGHT OUTER JOIN " _
           &"RTEBTCMTYLINE ON RTSalesGroup.AREAID = RTEBTCMTYLINE.AREAID AND " _
           &"RTSalesGroup.GROUPID = RTEBTCMTYLINE.GROUPID AND " _
           &"RTSalesGroup.EDATE IS NULL LEFT OUTER JOIN " _
           &"RTCounty ON RTEBTCMTYLINE.CUTID = RTCounty.CUTID LEFT OUTER JOIN " _
           &"RTObj ON RTEBTCMTYLINE.CONSIGNEE = RTObj.CUSID LEFT OUTER JOIN " _
           &"RTEBTCUST ON RTEBTCMTYLINE.COMQ1 = RTEBTCUST.COMQ1 AND " _
           &"RTEBTCMTYLINE.LINEQ1 = RTEBTCUST.LINEQ1  inner join rtebtcmtyh on rtebtcmtyline.comq1=rtebtcmtyh.comq1 LEFT OUTER JOIN RTCODE ON rtebtcmtyline.LINERATE=RTCODE.CODE AND RTCODE.KIND='D3' " _
           &"WHERE RTEBTCMTYLINE.COMQ1= 0 " _                
           &"GROUP BY RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,RTEBTCMTYH.COMN, rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1)) , " _
           &"RTSalesGroup.GROUPNC, RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, " _
           &"rtcode.codenc, " _
           &"RTEBTCMTYLINE.RCVDAT," _
           &"RTEBTCMTYLINE.AGREE, " _
           &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, " _
           &"RTEBTCMTYLINE.HINETNOTIFYDAT, " _
           &"RTEBTCMTYLINE.ADSLAPPLYDAT,case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END, " _
           &"case WHEN RTEBTCMTYLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTEBTCMTYLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null then '未報竣' " _
           &"when RTEBTCMTYLINE.HINETNOTIFYDAT is not null then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL THEN '取得預定施工日' " _
           &"WHEN RTEBTCMTYLINE.linetel <> '' then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' " _
           &"WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTEBTCMTYLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END,case when RTEBTCMTYLINE.lockdat is null then '' else 'Y' END  "
           

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
  '  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or Ucase(emply)="T89003" or _
  '	 Ucase(emply)="T89018" or Ucase(emply)="T89020" or Ucase(emply)="T89025" or Ucase(emply)="T91099" or _
  '	 Ucase(emply)="T92134" or Ucase(emply)="T93168" or Ucase(emply)="T93177" or Ucase(emply)="T94180" then
  '   DAreaID="<>'*'"
  'end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  if userlevel=31 or userlevel =1  or userlevel =5 then DAreaID="<>'*'"
  
  '業務工程師只能讀取該工程師的社區
  'if userlevel=2 then
  '  If searchShow="全部" Then
  '  sqlList="SELECT RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN,t1no,netip, RTCounty.CUTNC, RTCmty.COMCNT, " _
  '       &"Sum( CASE custype  when '申裝戶'  THEN 1 ELSE 0 end),  " _
  '       &"Sum(CASE custype  when '深耕戶'  THEN 1 ELSE 0 end) , " _ 
  '       &"Sum(CASE custype  when ''  THEN 1 ELSE 0 end), " _
  '       &"Sum(CASE when DROPDAT is Null  THEN 0 ELSE 1 END ), " _                    
  '       &"Sum(CASE when DROPDAT is Null and rtcust.cusid is not null THEN 1 ELSE 0 END), " _            
  '       &"case when RTCmty.COMCNT = 0 then 0 else Sum(CASE when DROPDAT is Null and rtcust.cusid is not null  THEN 1 ELSE 0 END) * 100 / (RTCmty.COMCNT*1.0)  end , "  _                    
  '       &"Sum(CASE when FINISHDAT is Null and dropdat is null  and rtcust.cusid is not null THEN 1 ELSE 0 END), " _                    
  '       &"Sum(CASE when FINISHDAT is not Null and dropdat is null THEN 1 ELSE 0 END), " _                    
  '       &"RTcmty.T1PETITION,RTcmty.T1Apply  " _
  '       &"FROM RTEmployee INNER JOIN " _
  '       &"##RTCmtyGroup ON RTEmployee.CUSID = ##RTCmtyGroup.CUSID INNER JOIN " _
  '       &"RTCounty INNER JOIN " _
  '       &"RTCust RIGHT OUTER JOIN " _               
  '       &"RTCmty ON RTCUST.COMQ1 = RTCMTY.COMQ1 ON RTCounty.CUTID = RTCmty.CUTID INNER JOIN " _
  '       &"RTArea INNER JOIN " _
  '       &"RTAreaCty ON RTArea.AREAID = RTAreaCty.AREAID and rtarea.areaid" & DareaID & " ON " _
  '       &"RTCmty.CUTID = RTAreaCty.CUTID ON ##RTCmtyGroup.COMQ1 = RTCmty.COMQ1 " _
  '       &"WHERE RTArea.AREATYPE='1' AND " &searchQry &" " _         
  '       &"group by RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN,t1no,netip, RTCounty.CUTNC, " _
  '       &"RTCmty.COMCNT, RTCmty.APPLYCNT, RTCmty.T1PETITION, RTCmty.SCHDAT, " _
  '       &"RTCmty.T1APPLY " _
  '       &"ORDER BY RTCmty.COMN "
  '  Else
  '  sqlList="SELECT RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN, t1no,netip,RTCounty.CUTNC, RTCmty.COMCNT, " _
  '       &"sum( CASE custype  when '申裝戶'  THEN 1 ELSE 0 end) ,  " _
  '       &"sum(CASE custype  when '深耕戶'  THEN 1 ELSE 0 end) , " _ 
  '       &"Sum(CASE custype  when ''  THEN 1 ELSE 0 end), " _
  '       &"Sum(CASE when DROPDAT is Null  THEN 0 ELSE 1 END ), " _                    
  '       &"Sum(CASE when DROPDAT is Null and rtcust.cusid is not null THEN 1 ELSE 0 END), " _            
  '       &"case when RTCmty.COMCNT = 0 then 0 else Sum(CASE when DROPDAT is Null and rtcust.cusid is not null  THEN 1 ELSE 0 END) * 100 / (RTCmty.COMCNT*1.0)  end , "  _                    
  '       &"Sum(CASE when FINISHDAT is Null and dropdat is null  and rtcust.cusid is not null THEN 1 ELSE 0 END), " _                    
  '       &"Sum(CASE when FINISHDAT is not Null and dropdat is null THEN 1 ELSE 0 END), " _                    
  '       &"RTcmty.T1PETITION,RTcmty.T1Apply  " _
  '       &"FROM RTCounty INNER JOIN " _
  '       &"##RTCmtyGroup INNER JOIN " _
  '       &"RTCust RIGHT OUTER JOIN " _         
  '       &"RTCmty ON RTCUST.COMQ1 = RTCMTY.COMQ1 ON ##RTCmtyGroup.COMQ1 = RTCmty.COMQ1 ON " _
  '       &"RTCounty.CUTID = RTCmty.CUTID INNER JOIN " _
  '       &"RTArea INNER JOIN " _
  '       &"RTAreaCty ON RTArea.AREAID = RTAreaCty.AREAID and rtarea.areaid" & DareaID & " ON " _
  '       &"RTCmty.CUTID = RTAreaCty.CUTID INNER JOIN " _
  '       &"RTEmployee ON ##RTCmtyGroup.CUSID = RTEmployee.CUSID " _
  '       &"WHERE RTArea.AREATYPE='1' and " &searchQry & " "  _
  '       &"group by RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN,t1no,netip, RTCounty.CUTNC, " _
  '       &"RTCmty.COMCNT, RTCmty.APPLYCNT, RTCmty.T1PETITION, RTCmty.SCHDAT, " _
  '       &"RTCmty.T1APPLY " _                  
  '       &"ORDER BY RTCmty.COMN "
  '  End If
  '業務助理or客服人員
  'else
    If searchShow="全部" Then
         sqlList="SELECT RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,RTEBTCMTYH.COMN, rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1))  as comqline, " _
           &"RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, " _
           &"rtcode.codenc, " _
           &"RTEBTCMTYLINE.RCVDAT," _
           &"RTEBTCMTYLINE.AGREE, " _
           &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, " _
           &"RTEBTCMTYLINE.HINETNOTIFYDAT, " _
           &"RTEBTCMTYLINE.ADSLAPPLYDAT, " _
           &"SUM(CASE WHEN rtebtcust.cusid IS NOT NULL AND RTEBTCUST.CANCELDAT IS NULL THEN 1 ELSE 0 END) AS CUSCNT, " _
           &"SUM(CASE WHEN rtebtcust.DOCKETDAT IS NOT NULL THEN 1 ELSE 0 END) AS APPLYCNT, " _
           &"SUM(CASE WHEN rtebtcust.DOCKETdat IS NOT NULL AND RTEBTCUST.DROPDAT IS NOT NULL AND RTEBTCUST.OVERDUE <> 'Y' THEN 1 ELSE 0 END) ," _
           &"SUM(CASE WHEN rtebtcust.DOCKETdat IS NOT NULL AND RTEBTCUST.DROPDAT IS NOT NULL AND RTEBTCUST.OVERDUE = 'Y' THEN 1 ELSE 0 END) ," _
           &"SUM(CASE WHEN rtebtcust.DOCKETdat IS NOT NULL THEN 1 ELSE 0 END) - SUM(CASE WHEN rtebtcust.DOCKETdat IS NOT NULL AND RTEBTCUST.DROPDAT IS NOT NULL AND RTEBTCUST.OVERDUE <> 'Y' THEN 1 ELSE 0 END) - SUM(CASE WHEN rtebtcust.DOCKETdat IS NOT NULL AND RTEBTCUST.DROPDAT IS NOT NULL AND RTEBTCUST.OVERDUE = 'Y' THEN 1 ELSE 0 END) , " _
           &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END AS DEVPMAN, " _
           &"case  WHEN RTEBTCMTYLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTEBTCMTYLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null then '未報竣' " _
           &"when RTEBTCMTYLINE.HINETNOTIFYDAT is not null then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL THEN '取得預定施工日' " _
           &"WHEN RTEBTCMTYLINE.linetel <> '' then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' " _
           &"WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTEBTCMTYLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END,case when RTEBTCMTYLINE.lockdat is null then '' else 'Y' END  " _
           &"FROM RTSalesGroup RIGHT OUTER JOIN " _
           &"RTEBTCMTYLINE ON RTSalesGroup.AREAID = RTEBTCMTYLINE.AREAID AND " _
           &"RTSalesGroup.GROUPID = RTEBTCMTYLINE.GROUPID AND " _
           &"RTSalesGroup.EDATE IS NULL LEFT OUTER JOIN " _
           &"RTCounty ON RTEBTCMTYLINE.CUTID = RTCounty.CUTID LEFT OUTER JOIN " _
           &"RTObj ON RTEBTCMTYLINE.CONSIGNEE = RTObj.CUSID LEFT OUTER JOIN " _
           &"RTEBTCUST ON RTEBTCMTYLINE.COMQ1 = RTEBTCUST.COMQ1 AND " _
           &"RTEBTCMTYLINE.LINEQ1 = RTEBTCUST.LINEQ1  inner join rtebtcmtyh on rtebtcmtyline.comq1=rtebtcmtyh.comq1 LEFT OUTER JOIN RTCODE ON rtebtcmtyline.LINERATE=RTCODE.CODE AND RTCODE.KIND='D3' " _
           &"WHERE RTEBTCMTYLINE.COMQ1<> 0 AND " & SEARCHQRY & " " _
           &"GROUP BY RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,RTEBTCMTYH.COMN, rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1)), " _
           &"RTSalesGroup.GROUPNC, RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, " _
           &"rtcode.codenc, " _
           &"RTEBTCMTYLINE.RCVDAT," _
           &"RTEBTCMTYLINE.AGREE, " _
           &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, " _
           &"RTEBTCMTYLINE.HINETNOTIFYDAT, " _
           &"RTEBTCMTYLINE.ADSLAPPLYDAT,case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END, " _
           &"case  WHEN RTEBTCMTYLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTEBTCMTYLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null then '未報竣' " _
           &"when RTEBTCMTYLINE.HINETNOTIFYDAT is not null then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL THEN '取得預定施工日' " _
           &"WHEN RTEBTCMTYLINE.linetel <> '' then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' " _
           &"WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTEBTCMTYLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END,case when RTEBTCMTYLINE.lockdat is null then '' else 'Y' END  "
           
                       
    Else
         sqlList="SELECT RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,RTEBTCMTYH.COMN, rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1))  as comqline, " _
           &"RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, " _
           &"rtcode.codenc, " _
           &"RTEBTCMTYLINE.RCVDAT," _
           &"RTEBTCMTYLINE.AGREE, " _
           &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, " _
           &"RTEBTCMTYLINE.HINETNOTIFYDAT, " _
           &"RTEBTCMTYLINE.ADSLAPPLYDAT, " _
           &"SUM(CASE WHEN rtebtcust.cusid IS NOT NULL AND RTEBTCUST.CANCELDAT IS NULL THEN 1 ELSE 0 END) AS CUSCNT, " _
           &"SUM(CASE WHEN rtebtcust.DOCKETDAT IS NOT NULL THEN 1 ELSE 0 END) AS APPLYCNT, " _
           &"SUM(CASE WHEN rtebtcust.DOCKETdat IS NOT NULL AND RTEBTCUST.DROPDAT IS NOT NULL AND RTEBTCUST.OVERDUE <> 'Y' THEN 1 ELSE 0 END) ," _
           &"SUM(CASE WHEN rtebtcust.DOCKETdat IS NOT NULL AND RTEBTCUST.DROPDAT IS NOT NULL AND RTEBTCUST.OVERDUE = 'Y' THEN 1 ELSE 0 END) ," _
           &"SUM(CASE WHEN rtebtcust.DOCKETdat IS NOT NULL THEN 1 ELSE 0 END) - SUM(CASE WHEN rtebtcust.DOCKETdat IS NOT NULL AND RTEBTCUST.DROPDAT IS NOT NULL AND RTEBTCUST.OVERDUE <> 'Y' THEN 1 ELSE 0 END) - SUM(CASE WHEN rtebtcust.DOCKETdat IS NOT NULL AND RTEBTCUST.DROPDAT IS NOT NULL AND RTEBTCUST.OVERDUE = 'Y' THEN 1 ELSE 0 END) , " _
           &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END AS DEVPMAN, " _
           &"case  WHEN RTEBTCMTYLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTEBTCMTYLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null then '未報竣' " _
           &"when RTEBTCMTYLINE.HINETNOTIFYDAT is not null then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL THEN '取得預定施工日' " _
           &"WHEN RTEBTCMTYLINE.linetel <> '' then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' " _
           &"WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTEBTCMTYLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END,case when RTEBTCMTYLINE.lockdat is null then '' else 'Y' END  " _
           &"FROM RTSalesGroup RIGHT OUTER JOIN " _
           &"RTEBTCMTYLINE ON RTSalesGroup.AREAID = RTEBTCMTYLINE.AREAID AND " _
           &"RTSalesGroup.GROUPID = RTEBTCMTYLINE.GROUPID AND " _
           &"RTSalesGroup.EDATE IS NULL LEFT OUTER JOIN " _
           &"RTCounty ON RTEBTCMTYLINE.CUTID = RTCounty.CUTID LEFT OUTER JOIN " _
           &"RTObj ON RTEBTCMTYLINE.CONSIGNEE = RTObj.CUSID LEFT OUTER JOIN " _
           &"RTEBTCUST ON RTEBTCMTYLINE.COMQ1 = RTEBTCUST.COMQ1 AND " _
           &"RTEBTCMTYLINE.LINEQ1 = RTEBTCUST.LINEQ1  inner join rtebtcmtyh on rtebtcmtyline.comq1=rtebtcmtyh.comq1 LEFT OUTER JOIN RTCODE ON rtebtcmtyline.LINERATE=RTCODE.CODE AND RTCODE.KIND='D3' " _
           &"WHERE RTEBTCMTYLINE.COMQ1<> 0 AND " & SEARCHQRY & " " _
           &"GROUP BY RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,RTEBTCMTYH.COMN, rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1)) , " _
           &"RTSalesGroup.GROUPNC, RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, " _
           &"rtcode.codenc, " _
           &"RTEBTCMTYLINE.RCVDAT," _
           &"RTEBTCMTYLINE.AGREE, " _
           &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, " _
           &"RTEBTCMTYLINE.HINETNOTIFYDAT, " _
           &"RTEBTCMTYLINE.ADSLAPPLYDAT,case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END, " _
           &"case  WHEN RTEBTCMTYLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTEBTCMTYLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null then '未報竣' " _
           &"when RTEBTCMTYLINE.HINETNOTIFYDAT is not null then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL THEN '取得預定施工日' " _
           &"WHEN RTEBTCMTYLINE.linetel <> '' then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' " _
           &"WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTEBTCMTYLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END,case when RTEBTCMTYLINE.lockdat is null then '' else 'Y' END  "
                      
    End If  
  'end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>