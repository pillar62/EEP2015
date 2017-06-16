<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="KTS管理系統"
  title="KTS用戶資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="派工單;電話明細; 作 廢 ;作廢返轉;  異  動  ;  退  租  ;繳款查詢"
  functionOptProgram="KTSCUSTSNDWORKK.asp;KTSCUSTtK.asp;KTSCUSTCANCEL.asp;KTSCUSTCANCELRTN.asp;KTSCUSTCHGK.asp;KTSCUSTDROPK.asp;KTSCUSTPAYK.asp"
  functionOptPrompt="N;N;Y;Y;N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;營運點;用戶名稱;none;裝機地址;none;送件<BR>申請日;NCIC編號;裝機<BR>完工日;報竣日;合約<BR>起算日;作廢日;退租日;開發<BR>業務;大盤;none;申請<BR>電話數;開通<BR>電話數;none"
  sqlDelete="SELECT KTSCUST.CUSID, KTSCUST.CUSNC, KTSCUST.SOCIALID,RTCounty.CUTNC + KTSCUST.TOWNSHIP3 + KTSCUST.RADDR3 AS ADDR, " _
           &"KTSCUST.APFORMAPPLYDAT, KTSCUST.APPLYDAT, KTSCUST.NCICCUSID, KTSCUST.FINISHDAT, KTSCUST.DOCKETDAT, KTSCUST.CONTRACTSTRDAT, KTSCUST.CANCELDAT, " _
           &"KTSCUST.DROPDAT, RTObj_2.CUSNC, RTObj_3.SHORTNC, KTSCUST.consignee2, " _
           &"SUM(CASE WHEN KTSCUSTD1.CUSID IS NOT NULL AND KTSCUSTD1.CANCELDAT IS NULL AND KTSCUSTD1.DROPDAT IS NULL THEN 1 ELSE 0 END), " _
           &"SUM(CASE WHEN KTSCUSTD1.CUSID IS NOT NULL AND KTSCUSTD1.CANCELDAT IS NULL AND KTSCUSTD1.DROPDAT IS NULL AND KTSCUSTD1.APPLYDAT IS NOT NULL THEN 1 ELSE 0 END), " _
           &"ktscust.NOTATION " _
           &"FROM KTSCUSTD1 RIGHT OUTER JOIN KTSCUST ON KTSCUSTD1.CUSID = KTSCUST.CUSID AND KTSCUSTD1.DROPDAT IS NULL AND " _
           &"KTSCUSTD1.CANCELDAT IS NULL LEFT OUTER JOIN RTObj RTObj_2 INNER JOIN RTEmployee ON RTObj_2.CUSID = RTEmployee.CUSID ON " _
           &"KTSCUST.EMPLY = RTEmployee.EMPLY LEFT OUTER JOIN RTObj RTObj_1 ON KTSCUST.CONSIGNEE2 = RTObj_1.CUSID LEFT OUTER JOIN " _
           &"RTObj RTObj_3 ON KTSCUST.CONSIGNEE1 = RTObj_3.CUSID LEFT OUTER JOIN RTCounty ON KTSCUST.CUTID3 = RTCounty.CUTID " _
           &"GROUP BY  KTSCUST.CUSID, KTSCUST.CUSNC, KTSCUST.SOCIALID, RTCounty.CUTNC + KTSCUST.TOWNSHIP3 + KTSCUST.RADDR3, " _
           &"KTSCUST.APFORMAPPLYDAT, KTSCUST.APPLYDAT, KTSCUST.NCICCUSID, KTSCUST.FINISHDAT, KTSCUST.DOCKETDAT, KTSCUST.CONTRACTSTRDAT, KTSCUST.CANCELDAT, " _
           &"KTSCUST.DROPDAT, RTObj_2.CUSNC,RTObj_3.SHORTNC, ktscust.consignee2,ktscust.NOTATION "

  dataTable="ktscust"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="KTSCustD.asp"
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
  searchProg="KTSCUSTS.ASP"
  '----
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" KTSCust.CUSID<>'' AND KTSCUST.CANCELDAT IS NULL "
     searchShow="全部用戶(不含作廢)"
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
            DAreaID="='A1'"
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
  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or  Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="T90076" OR _
     Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T89076"then
     DAreaID="<>'*'"
  end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  if userlevel=31 or userlevel =1  or userlevel =5 then DAreaID="<>'*'"
  
    If searchShow="全部" Then
         sqlList="SELECT KTSCUST.CUSID," _
           &"CASE WHEN KTSCUST.CONSIGNEE1<>'' THEN RTOBJ_3.SHORTNC ELSE  case when RTCTYTOWN.operationname=''  OR RTCTYTOWN.operationname IS NULL then " _
           &"CASE WHEN KTSCUST.cutid3 IN ('08','09','10','11','12','13') THEN '第十二營運點' " _
           &"WHEN  KTSCUST.cutid3 IN ('14','15','16','17','18','19','20','21','23') THEN '第十三營運點' ELSE '無法歸屬' END " _
           &"ELSE RTCTYTOWN.operationname END  END," _
           &"SUBSTRING(KTSCUST.CUSNC,1,6)+'....', KTSCUST.SOCIALID,RTCounty.CUTNC + KTSCUST.TOWNSHIP3 + KTSCUST.RADDR3 AS ADDR, " _
           &"KTSCUST.APFORMAPPLYDAT, KTSCUST.APPLYDAT, KTSCUST.NCICCUSID, KTSCUST.FINISHDAT, KTSCUST.DOCKETDAT, KTSCUST.CONTRACTSTRDAT, KTSCUST.CANCELDAT, " _
           &"KTSCUST.DROPDAT, RTObj_2.CUSNC, RTObj_3.SHORTNC, substring(KTSCUST.consignee2,1,5), " _
           &"SUM(CASE WHEN KTSCUSTD1.CUSID IS NOT NULL AND KTSCUSTD1.CANCELDAT IS NULL AND KTSCUSTD1.DROPDAT IS NULL AND KTSCUSTD1.APPLYDAT IS NOT NULL THEN 1 ELSE 0 END), " _
           &"SUM(CASE WHEN KTSCUSTD1.CUSID IS NOT NULL AND KTSCUSTD1.CANCELDAT IS NULL AND KTSCUSTD1.DROPDAT IS NULL AND KTSCUSTD1.OPENDAT IS NOT NULL THEN 1 ELSE 0 END), " _
           &"ktscust.NOTATION " _
           &"FROM KTSCUSTD1 RIGHT OUTER JOIN KTSCUST ON KTSCUSTD1.CUSID = KTSCUST.CUSID AND KTSCUSTD1.DROPDAT IS NULL AND " _
           &"KTSCUSTD1.CANCELDAT IS NULL LEFT OUTER JOIN RTObj RTObj_2 INNER JOIN RTEmployee ON RTObj_2.CUSID = RTEmployee.CUSID ON " _
           &"KTSCUST.EMPLY = RTEmployee.EMPLY LEFT OUTER JOIN RTObj RTObj_1 ON KTSCUST.CONSIGNEE2 = RTObj_1.CUSID LEFT OUTER JOIN " _
           &"RTObj RTObj_3 ON KTSCUST.CONSIGNEE1 = RTObj_3.CUSID LEFT OUTER JOIN RTCounty ON KTSCUST.CUTID3 = RTCounty.CUTID " _
           &"left outer join rtctytown on KTSCUST.cutid3=rtctytown.cutid and KTSCUST.township3=rtctytown.township " _
           &"where " & searchqry & " " _
           &"GROUP BY  KTSCUST.CUSID," _
           &"CASE WHEN KTSCUST.CONSIGNEE1<>'' THEN RTOBJ_3.SHORTNC ELSE  case when RTCTYTOWN.operationname=''  OR RTCTYTOWN.operationname IS NULL then " _
           &"CASE WHEN KTSCUST.cutid3 IN ('08','09','10','11','12','13') THEN '第十二營運點' " _
           &"WHEN  KTSCUST.cutid3 IN ('14','15','16','17','18','19','20','21','23') THEN '第十三營運點' ELSE '無法歸屬' END " _
           &"ELSE RTCTYTOWN.operationname END  END," _
           &"SUBSTRING(KTSCUST.CUSNC,1,6)+'....', KTSCUST.SOCIALID, RTCounty.CUTNC + KTSCUST.TOWNSHIP3 + KTSCUST.RADDR3, " _
           &"KTSCUST.APFORMAPPLYDAT, KTSCUST.APPLYDAT, KTSCUST.NCICCUSID, KTSCUST.FINISHDAT, KTSCUST.DOCKETDAT, KTSCUST.CONTRACTSTRDAT, KTSCUST.CANCELDAT, " _
           &"KTSCUST.DROPDAT, RTObj_2.CUSNC, RTObj_3.SHORTNC, substring(KTSCUST.consignee2,1,5),ktscust.NOTATION " 
    Else
         sqlList="SELECT KTSCUST.CUSID," _
           &"CASE WHEN KTSCUST.CONSIGNEE1<>'' THEN RTOBJ_3.SHORTNC ELSE  case when RTCTYTOWN.operationname=''  OR RTCTYTOWN.operationname IS NULL then " _
           &"CASE WHEN KTSCUST.cutid3 IN ('08','09','10','11','12','13') THEN '第十二營運點' " _
           &"WHEN  KTSCUST.cutid3 IN ('14','15','16','17','18','19','20','21','23') THEN '第十三營運點' ELSE '無法歸屬' END " _
           &"ELSE RTCTYTOWN.operationname END  END," _
           &"SUBSTRING(KTSCUST.CUSNC,1,6)+'....', KTSCUST.SOCIALID,RTCounty.CUTNC + KTSCUST.TOWNSHIP3 + KTSCUST.RADDR3 AS ADDR, " _
           &"KTSCUST.APFORMAPPLYDAT, KTSCUST.APPLYDAT, KTSCUST.NCICCUSID, KTSCUST.FINISHDAT, KTSCUST.DOCKETDAT, KTSCUST.CONTRACTSTRDAT, KTSCUST.CANCELDAT, " _
           &"KTSCUST.DROPDAT, RTObj_2.CUSNC, RTObj_3.SHORTNC,  substring(KTSCUST.consignee2,1,5), " _
           &"SUM(CASE WHEN KTSCUSTD1.CUSID IS NOT NULL AND KTSCUSTD1.CANCELDAT IS NULL AND KTSCUSTD1.DROPDAT IS NULL AND KTSCUSTD1.APPLYDAT IS NOT NULL THEN 1 ELSE 0 END), " _
           &"SUM(CASE WHEN KTSCUSTD1.CUSID IS NOT NULL AND KTSCUSTD1.CANCELDAT IS NULL AND KTSCUSTD1.DROPDAT IS NULL AND KTSCUSTD1.OPENDAT IS NOT NULL THEN 1 ELSE 0 END), " _
           &"ktscust.NOTATION " _
           &"FROM KTSCUSTD1 RIGHT OUTER JOIN KTSCUST ON KTSCUSTD1.CUSID = KTSCUST.CUSID AND KTSCUSTD1.DROPDAT IS NULL AND " _
           &"KTSCUSTD1.CANCELDAT IS NULL LEFT OUTER JOIN RTObj RTObj_2 INNER JOIN RTEmployee ON RTObj_2.CUSID = RTEmployee.CUSID ON " _
           &"KTSCUST.EMPLY = RTEmployee.EMPLY LEFT OUTER JOIN RTObj RTObj_1 ON KTSCUST.CONSIGNEE2 = RTObj_1.CUSID LEFT OUTER JOIN " _
           &"RTObj RTObj_3 ON KTSCUST.CONSIGNEE1 = RTObj_3.CUSID LEFT OUTER JOIN RTCounty ON KTSCUST.CUTID3 = RTCounty.CUTID " _
           &"left outer join rtctytown on KTSCUST.cutid3=rtctytown.cutid and KTSCUST.township3=rtctytown.township " _
           &"where " & searchqry & " " _
           &"GROUP BY  KTSCUST.CUSID, " _
           &"CASE WHEN KTSCUST.CONSIGNEE1<>'' THEN RTOBJ_3.SHORTNC ELSE  case when RTCTYTOWN.operationname=''  OR RTCTYTOWN.operationname IS NULL then " _
           &"CASE WHEN KTSCUST.cutid3 IN ('08','09','10','11','12','13') THEN '第十二營運點' " _
           &"WHEN  KTSCUST.cutid3 IN ('14','15','16','17','18','19','20','21','23') THEN '第十三營運點' ELSE '無法歸屬' END " _
           &"ELSE RTCTYTOWN.operationname END  END," _
           &"SUBSTRING(KTSCUST.CUSNC,1,6)+'....', KTSCUST.SOCIALID, RTCounty.CUTNC + KTSCUST.TOWNSHIP3 + KTSCUST.RADDR3, " _
           &"KTSCUST.APFORMAPPLYDAT, KTSCUST.APPLYDAT, KTSCUST.NCICCUSID, KTSCUST.FINISHDAT, KTSCUST.DOCKETDAT, KTSCUST.CONTRACTSTRDAT, KTSCUST.CANCELDAT, " _
           &"KTSCUST.DROPDAT, RTObj_2.CUSNC, RTObj_3.SHORTNC,substring(KTSCUST.consignee2,1,5),ktscust.NOTATION " 
          
    End If  
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>