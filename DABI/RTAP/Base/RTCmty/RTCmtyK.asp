<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="社區及客戶資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="固定流量;計量流量;營業員;業務員;管委會;客　戶;合　約;FTTB進度"
  functionOptProgram="rtcmtyflow.asp;rtcmtyflow2.asp;RTCmtyBusK.asp;RTCmtySaleK.asp;RTCmtySpK.asp;RTCustK.asp;RTContractK.asp;/WEBAP/RTAP/BASE/FTTBCMTYSTAT/FTTBCmtyStatK.ASP"
  functionOptPrompt="N;N;N;N;N;N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
 ' formatName="none;營運點;序號;none;社區名稱;none;網路IP;縣市;規模<br>戶數;申裝戶;深耕戶; " _
 '           &"類別<br>未建檔;撤銷戶;現有戶<br>數合計;開發<br>比率%;未完<br>工戶;已完<br>工戶;報竣戶;none;T1開<br>通日"
  formatName="none;營運點;序號;社區名稱;HB網路IP;縣市;規模<br>戶數;HB<br>申請戶;" _
            &"HB<br>退欠戶;HB<br>現有戶;HB<br>開發率%;HB<br>未完工戶;HB<br>完工戶;HB<BR>報竣戶;T1開<br>通日;FTTB進度;供裝<BR>時程;營運處"
            
  sqlDelete="SELECT RTCmty.COMQ1 , RTCmty.COMQ2, RTCmty.COMN, RTCounty.CUTNC, RTCmty.COMCNT, " _
           &"RTCmty.APPLYCNT,RTcmty.T1PETITION,RTCmty.Schdat,RTcmty.T1Apply " _
           &"FROM RTCmty INNER JOIN RTCounty ON RTCmty.CUTID = RTCounty.CUTID " _
           &"WHERE (((RTCmty.COMQ1)=0)) "
  dataTable="RTCmty"
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
  goodMorning=TRUE
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="RTCmtyS.asp"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=TRUE
  If searchQry="" Then
     searchQry=" RTCmty.ComQ1=0 "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  '讀取登入帳號之群組資料
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
  'Response.Write "GP=" & usergroup
  '-------------------------------------------------------------------------------------------
  'userlevel=2:為業務工程師==>只能看所屬社區資料
  'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
 ' Response.Write "DOMAIN=" & domain & "<BR>"
  Domain=Mid(Emply,1,1)
  select case Domain
         case "T"
            DAreaID="<>'*'"
         case "C"
            DAreaID="='A2'"         
         case "K"
            DAreaID="='A3'"         
         case else
            DareaID="=''"
  end select
  '高階主管可讀取全部資料
  'if UCASE(emply)="T89001" or Ucase(emply)="T89002" or Ucase(emply)="T89003" or Ucase(emply)="T89018" OR _
  '   Ucase(emply)="T89020" or Ucase(emply)="T89025"  then
  '   DAreaID="<>'*'"
  'end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  if userlevel=31 or userlevel =1  or userlevel =5 then DAreaID="<>'*'"
  
  '業務工程師只能讀取該工程師的社區
  'if userlevel=2 then

  '  If searchShow="全部" Then
  '       sqlList="SELECT RTCmty.COMQ1," _
  '       &"CASE WHEN RTCode.PARM1 =  'AA'  THEN " _
  '       &"case when RTCTYTOWNX.operationname=''  OR RTCTYTOWNX.operationname IS NULL then " _
  '       &"CASE WHEN rtCMTY.cutid IN ('08','09','10','11','12','13') THEN '第十二營運點' " _
  '       &"WHEN   rtCMTY.cutid IN ('14','15','16','17','18','19','20','21','23') THEN '第十三營運點' " _
  '       &"ELSE '無法歸屬' END ELSE RTCTYTOWNX.operationname END ELSE  rtcode.codenc END," _
  '       &"RTCmty.COMQ1,RTCmty.COMQ2, RTCmty.COMN,t1no,netip, RTCounty.CUTNC, RTCmty.COMCNT, " _
  '       &"sum( CASE custype  when '申裝戶'  THEN 1 ELSE 0 end),  " _
  '       &"sum(CASE custype  when '深耕戶'  THEN 1 ELSE 0 end), " _ 
  '       &"Sum(CASE custype  when ''  THEN 1 ELSE 0 end), " _
  '       &"Sum(CASE when DROPDAT is Null  THEN 0 ELSE 1 END ), " _                    
  '       &"Sum(CASE when DROPDAT is Null and rtcust.cusid is not null THEN 1 ELSE 0 END), " _            
  '       &"case when RTCmty.COMCNT = 0 then 0 else Sum(CASE when DROPDAT is Null and rtcust.cusid is not null  THEN 1 ELSE 0 END) * 100 / (RTCmty.COMCNT*1.0)  end , "  _                    
  '       &"Sum(CASE when FINISHDAT is Null and dropdat is null  and rtcust.cusid is not null THEN 1 ELSE 0 END), " _                    
  '       &"Sum(CASE when FINISHDAT is not Null and dropdat is null THEN 1 ELSE 0 END), " _   
  '       &"Sum(CASE when docketDAT is not Null and dropdat is null THEN 1 ELSE 0 END), " _                  
  '       &"RTcmty.T1PETITION,RTcmty.T1Apply  " _
  '       &"FROM RTArea INNER JOIN " _
  '       &"RTAreaCty ON RTArea.AREAID = RTAreaCty.AREAID  and RTArea.AREATYPE='1' and rtarea.areaid" & DareaID & " RIGHT OUTER JOIN " _
  '       &"RTVCmtyGroup RIGHT OUTER JOIN " _
  '       &"RTCust RIGHT OUTER JOIN " _
  '       &"RTCmty ON RTCUST.COMQ1 = RTCMTY.COMQ1 ON RTVCmtyGroup.COMQ1 = RTCmty.COMQ1  ON  " _
  '       &"RTAreaCty.CUTID = RTCmty.CUTID LEFT OUTER JOIN " _
  '       &"RTCounty ON RTCmty.CUTID = RTCounty.CUTID " _
  '       &" left outer JOIN RTCode ON RTCmty.COMTYPE = RTCode.CODE AND RTCode.KIND = 'B3' " _
  '       &" left outer join rtctytown RTCTYTOWNX on rtcust.cutid1=rtctytownX.cutid and rtcust.township1=rtctytownX.township " _
  '       &"WHERE "& searchQry &" " _
  '       &"group by RTCmty.COMQ1," _
  '       &"CASE WHEN RTCode.PARM1 =  'AA'  THEN " _
  '       &"case when RTCTYTOWNX.operationname=''  OR RTCTYTOWNX.operationname IS NULL then " _
  '       &"CASE WHEN rtCMTY.cutid IN ('08','09','10','11','12','13') THEN '第十二營運點' " _
  '       &"WHEN   rtcMTY.cutid IN ('14','15','16','17','18','19','20','21','23') THEN '第十三營運點' " _
  '       &"ELSE '無法歸屬' END ELSE RTCTYTOWNX.operationname END ELSE  rtcode.codenc END," _
  '       &"RTCmty.COMQ1,RTCmty.COMQ2, RTCmty.COMN,t1no,netip, RTCounty.CUTNC, " _
  '       &"RTCmty.COMCNT, RTCmty.APPLYCNT, RTCmty.T1PETITION, RTCmty.SCHDAT, " _
  '       &"RTCmty.T1APPLY " _         
  '       &"ORDER BY RTCmty.COMN "       
  '  Else
  '       sqlList="SELECT RTCmty.COMQ1," _
  '       &"CASE WHEN RTCode.PARM1 =  'AA'  THEN " _
  '       &"case when RTCTYTOWNX.operationname=''  OR RTCTYTOWNX.operationname IS NULL then " _
  '       &"CASE WHEN rtCMTY.cutid IN ('08','09','10','11','12','13') THEN '第十二營運點' " _
  '       &"WHEN   rtcMTY.cutid IN ('14','15','16','17','18','19','20','21','23') THEN '第十三營運點' " _
  '       &"ELSE '無法歸屬' END ELSE RTCTYTOWNX.operationname END ELSE  rtcode.codenc END," _
  '       &"RTCmty.COMQ1,RTCmty.COMQ2, RTCmty.COMN,t1no,netip ,RTCounty.CUTNC, RTCmty.COMCNT, " _
  '       &"sum( CASE custype  when '申裝戶'  THEN 1 ELSE 0 end) ,  " _
  '       &"sum(CASE custype  when '深耕戶'  THEN 1 ELSE 0 end) , " _ 
  '       &"Sum(CASE custype  when ''  THEN 1 ELSE 0 end), " _
  '       &"Sum(CASE when DROPDAT is Null  THEN 0 ELSE 1 END ), " _                    
  '       &"Sum(CASE when DROPDAT is Null and rtcust.cusid is not null THEN 1 ELSE 0 END), " _            
  '       &"case when RTCmty.COMCNT = 0 then 0 else Sum(CASE when DROPDAT is Null and rtcust.cusid is not null  THEN 1 ELSE 0 END) * 100 / (RTCmty.COMCNT*1.0)  end , "  _                    
  '       &"Sum(CASE when FINISHDAT is Null and dropdat is null  and rtcust.cusid is not null THEN 1 ELSE 0 END), " _                    
  '       &"Sum(CASE when FINISHDAT is not Null and dropdat is null THEN 1 ELSE 0 END), " _   
  '       &"Sum(CASE when DOCKETDAT is not Null and dropdat is null THEN 1 ELSE 0 END), " _                  
  '       &"RTcmty.T1PETITION,RTcmty.T1Apply  " _
  ''       &"FROM RTArea inner JOIN " _
  '       &"RTAreaCty ON RTArea.AREAID = RTAreaCty.AREAID and RTArea.AREATYPE='1' and rtarea.areaid" & DareaID & " RIGHT OUTER JOIN " _
  '       &"RTVCmtyGroup RIGHT OUTER JOIN " _
  '       &"RTCust RIGHT OUTER JOIN " _         
  '       &"RTCmty ON RTCUST.COMQ1 = RTCMTY.COMQ1 ON RTVCmtyGroup.COMQ1 = RTCmty.COMQ1  ON " _
  '       &"RTAreaCty.CUTID = RTCmty.CUTID LEFT OUTER JOIN " _
  '       &"RTCounty ON RTCmty.CUTID = RTCounty.CUTID " _ 
  '       &" left outer JOIN RTCode ON RTCmty.COMTYPE = RTCode.CODE AND RTCode.KIND = 'B3' " _
  '       &" left outer join rtctytown RTCTYTOWNX on rtcust.cutid1=rtctytownX.cutid and rtcust.township1=rtctytownX.township " _
  '       &"WHERE "& searchQry &" " _
  '       &"group by RTCmty.COMQ1," _
  '       &"CASE WHEN RTCode.PARM1 =  'AA'  THEN " _
  '       &"case when RTCTYTOWNX.operationname=''  OR RTCTYTOWNX.operationname IS NULL then " _
  '       &"CASE WHEN rtCMTY.cutid IN ('08','09','10','11','12','13') THEN '第十二營運點' " _
  '       &"WHEN   rtcMTY.cutid IN ('14','15','16','17','18','19','20','21','23') THEN '第十三營運點' " _
  '       &"ELSE '無法歸屬' END ELSE RTCTYTOWNX.operationname END ELSE  rtcode.codenc END," _
  '       &"RTCmty.COMQ1,RTCmty.COMQ2, RTCmty.COMN, t1no,netip,RTCounty.CUTNC, " _
  '       &"RTCmty.COMCNT, RTCmty.APPLYCNT, RTCmty.T1PETITION, RTCmty.SCHDAT, " _
  '       &"RTCmty.T1APPLY " _                  
  '       &"ORDER BY RTCmty.COMN "       
  '  End If  
  'end if
  sqlList="SELECT RTCmty.COMQ1,CASE WHEN RTCode.PARM1 = 'AA' THEN CASE WHEN RTCTYTOWNX.operationname = '' OR " _
         &"RTCTYTOWNX.operationname IS NULL THEN CASE WHEN rtCMTY.cutid IN ('08','09', '10', '11', '12', '13') " _
         &"THEN '第十二營運點' WHEN rtcMTY.cutid IN ('14','15', '16', '17', '18', '19', '20', '21', '23') " _
         &"THEN '第十三營運點' ELSE '無法歸屬' END ELSE RTCTYTOWNX.operationname END ELSE rtcode.codenc END, " _
         &"rtcmty.comq1, RTCmty.COMN, rtcmty.netip,RTCounty.CUTNC, RTCmty.COMCNT, SUM(CASE WHEN rtcust.cusid <> '' AND " _
         &"freecode <> 'Y' THEN 1 ELSE 0 END), SUM(CASE WHEN DROPDAT IS NOT NULL AND rtcust.cusid IS NOT NULL AND " _
         &"freecode <> 'Y' THEN 1 ELSE 0 END), SUM(CASE WHEN DROPDAT IS NULL AND rtcust.cusid IS NOT NULL AND " _
         &"freecode <> 'Y' THEN 1 ELSE 0 END), CASE WHEN RTCmty.COMCNT = 0 THEN 0 ELSE SUM(CASE WHEN DROPDAT IS NULL AND " _
         &"rtcust.cusid IS NOT NULL AND freecode <> 'Y' AND docketdat IS NOT NULL THEN 1 ELSE 0 END) * 100 / " _
         &"(RTCmty.COMCNT * 1.0) END, SUM(CASE WHEN FINISHDAT IS NULL AND dropdat IS NULL AND " _
         &"rtcust.cusid IS NOT NULL AND freecode <> 'Y' THEN 1 ELSE 0 END), " _
         &"SUM(CASE WHEN FINISHDAT IS NOT NULL AND dropdat IS NULL AND freecode <> 'Y' THEN 1 ELSE 0 END), " _
         &"SUM(CASE WHEN DOCKETDAT IS NOT NULL AND dropdat IS NULL AND freecode <> 'Y' THEN 1 ELSE 0 END), " _
         &"RTcmty.T1Apply, FTTBCMTYSTAT.codenc, FTTBCMTYSTAT.parm1, FTTBCHTSERVICEPOINT.CHTNAME " _
         &"FROM (SELECT FTTBCMTYSTAT.comq1, RTCODE.CODENC, rtcode.parm1 FROM fttbcmtystat LEFT OUTER JOIN " _
         &"             rtcode ON fttbcmtystat.smode = rtcode.code AND rtcode.kind = 'O6' " _
         &"             GROUP BY   FTTBCMTYSTAT.comq1, rtcode.codenc, rtcode.parm1) FTTBCMTYSTAT right outer JOIN " _
         &"RTCmty ON FTTBCMTYSTAT.COMQ1 = RTCmty.COMQ1 LEFT OUTER JOIN rtctytown RTCTYTOWNX ON " _
         &"rtcmty.cutid = rtctytownX.cutid AND rtcmty.township = rtctytownX.township LEFT OUTER JOIN " _
         &"RTCode ON RTCmty.COMTYPE = RTCode.CODE AND RTCode.KIND = 'B3' LEFT OUTER JOIN " _
         &"rTCounty ON RTCmty.CUTID = RTCounty.CUTID LEFT OUTER JOIN FTTBCHTSERVICEPOINT ON  " _
         &"RTCMTY.WORKPLACE = FTTBCHTSERVICEPOINT.CHTID LEFT OUTER JOIn rtcust ON rtcmty.comq1 = rtcust.comq1 " _
         &"WHERE rtcmty.comq1 <> 0 and " & searchqry & " " _
         &"GROUP BY  RTCmty.COMQ1, CASE WHEN RTCode.PARM1 = 'AA' THEN CASE WHEN RTCTYTOWNX.operationname = '' OR " _
         &"RTCTYTOWNX.operationname IS NULL THEN CASE WHEN rtCMTY.cutid IN ('08','09', '10', '11', '12', '13') " _
         &"THEN '第十二營運點' WHEN rtcMTY.cutid IN ('14','15', '16', '17', '18', '19', '20', '21', '23') " _
         &"THEN '第十三營運點' ELSE '無法歸屬' END ELSE RTCTYTOWNX.operationname END ELSE rtcode.codenc END, " _
         &"RTCmty.COMN, rtcmty.netip, RTCounty.CUTNC, RTCmty.COMCNT, RTcmty.T1Apply, FTTBCMTYSTAT.codenc, " _
         &"FTTBCHTSERVICEPOINT.CHTNAME, FTTBCMTYSTAT.parm1 ORDER BY  RTCmty.COMN " 
  
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()
  Dim conn,i
  Set conn=Server.CreateObject("ADODB.Connection")
  On Error Resume Next  
  conn.Open DSN
  If Len(extDeleList(1)) > 0 Then
     delSql="DELETE  FROM RTCmtyBus WHERE COMQ1 IN " &extDeleList(1) &" " 
     conn.Execute delSql
     delSql="DELETE  FROM RTCmtySale WHERE COMQ1 IN " &extDeleList(1) &" "
     conn.Execute delSql
     delSql="DELETE  FROM RTCmtySp WHERE COMQ1 IN " &extDeleList(1) &" "
     conn.Execute delSql
  End If
  conn.Close
End Sub
%>