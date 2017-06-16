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
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  FunctionOptName="派工查詢;設備查詢;用戶維護"
  functionOptProgram="/WEBAP/RTAP/BASE/RTEBTCMTY/rtebtCMTYlineSNDWORKk2.asp;/WEBAP/RTAP/BASE/RTEBTCMTY/rtebtcmtyhardwareK2.asp;/WEBAP/RTAP/BASE/RTEBTCMTY/rtebtcustK.asp"
  functionOptPrompt="N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;社區名稱;主線;線路IP;附掛電話;none;主線頻寬;none;可建置;申請日;none;CHT通知日;測通日;用戶;報竣;none;進度;鎖定"
  sqlDelete="SELECT RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,RTEBTCMTYH.COMN, rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1))  as comqline, " _
           &"RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, " _
           &"rtcode.codenc, " _
           &"RTEBTCMTYLINE.RCVDAT, " _
           &"RTEBTCMTYLINE.AGREE, " _
           &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, " _
           &"RTEBTCMTYLINE.HINETNOTIFYDAT, " _
           &"RTEBTCMTYLINE.ADSLAPPLYDAT, " _
           &"SUM(CASE WHEN rtebtcust.cusid IS NOT NULL THEN 1 ELSE 0 END) AS CUSCNT, " _
           &"SUM(CASE WHEN rtebtcust.transdat IS NOT NULL THEN 1 ELSE 0 END) AS APPLYCNT, " _
           &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END AS DEVPMAN,  " _
           &"case  WHEN RTEBTCMTYLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTEBTCMTYLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null then '未報竣' " _
           &"when RTEBTCMTYLINE.HINETNOTIFYDAT is not null then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL THEN '取得預定施工日' " _
           &"WHEN RTEBTCMTYLINE.linetel <> '' then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' " _
           &"WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTEBTCMTYLINE.AGREE ='N' THEN '勘查為不可建' WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END,case when RTEBTCMTYLINE.lockdat is null then '' else 'Y' END  " _
           &"FROM RTSalesGroup RIGHT OUTER JOIN " _
           &"RTEBTCMTYLINE ON RTSalesGroup.AREAID = RTEBTCMTYLINE.AREAID AND " _
           &"RTSalesGroup.GROUPID = RTEBTCMTYLINE.GROUPID AND " _
           &"RTSalesGroup.EDATE IS NULL LEFT OUTER JOIN " _
           &"RTCounty ON RTEBTCMTYLINE.CUTID = RTCounty.CUTID LEFT OUTER JOIN " _
           &"RTObj ON RTEBTCMTYLINE.CONSIGNEE = RTObj.CUSID LEFT OUTER JOIN " _
           &"RTEBTCUST ON RTEBTCMTYLINE.COMQ1 = RTEBTCUST.COMQ1 AND " _
           &"RTEBTCMTYLINE.LINEQ1 = RTEBTCUST.LINEQ1 inner join rtebtcmtyh on rtebtcmtyline.comq1=rtebtcmtyh.comq1 LEFT OUTER JOIN RTCODE ON rtebtcmtyline.LINERATE=RTCODE.CODE AND RTCODE.KIND='D3' " _
           &"WHERE RTEBTCMTYLINE.COMQ1= 0 " _                
           &"GROUP BY RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,RTEBTCMTYH.COMN, rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1)) , " _
           &"RTSalesGroup.GROUPNC, RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, " _
           &"rtcode.codenc, " _
           &"RTEBTCMTYLINE.RCVDAT, " _
           &"RTEBTCMTYLINE.AGREE, " _
           &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, " _
           &"RTEBTCMTYLINE.HINETNOTIFYDAT, " _
           &"RTEBTCMTYLINE.ADSLAPPLYDAT,case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END, " _
           &"case  WHEN RTEBTCMTYLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTEBTCMTYLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null then '未報竣' " _
           &"when RTEBTCMTYLINE.HINETNOTIFYDAT is not null then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL THEN '取得預定施工日' " _
           &"WHEN RTEBTCMTYLINE.linetel <> '' then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' " _
           &"WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTEBTCMTYLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END,case when RTEBTCMTYLINE.lockdat is null then '' else 'Y' END  "
 

  dataTable="rtebtcmtyline"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="/WEBAP/RTAP/BASE/RTEBTCMTY/RTebtCmtylineD.asp"
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
  searchProg="RTEBTCMTYLINES2.ASP"
  '讀取暫存KEY
  KEYXX=SPLIT(SESSION("search6"),";")
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTEBTCmtyline.ComQ1<>0 AND RTEBTCmtyline.ADSLAPPLYDAT IS NOT NULL AND RTEBTCmtyline.DROPDAT IS null "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  if len(trim(keyxx(0)))> 0 then
     searchQry6=" HAVING SUM(CASE WHEN rtebtcust.DOCKETdat IS NOT NULL AND rtebtcust.DROPDAT IS NULL THEN 1 ELSE 0 END) " & KEYXX(0) & " " & KEYXX(1)
  else
     SEARCHQRY6=""
  END IF
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
  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="T90076" OR _
     Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T89076" or Ucase(emply)="T91129" or _
     Ucase(emply)="T89031" or Ucase(emply)="T92134" or Ucase(emply)="P92010" or Ucase(emply)="T94180" then
     DAreaID="<>'*'"
  end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  if userlevel=31  then DAreaID="<>'*'"
  
  '業務工程師只能讀取該工程師的社區
  'if userlevel=2 then
  '  If searchShow="全部" Then
 '台北窗口
    If DAreaID="<>'*'" Then
       sqlList="SELECT RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,rtebtcmtyh.comn " _
              &",rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1)) as comqline, " _
              &"RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno,rtcode.codenc, RTEBTCMTYLINE.RCVDAT, " _
              &"RTEBTCMTYLINE.AGREE, RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, " _
              &"RTEBTCMTYLINE.HINETNOTIFYDAT, RTEBTCMTYLINE.ADSLAPPLYDAT, " _
              &"SUM(CASE WHEN rtebtcust.cusid IS NOT NULL AND rtebtcust.DROPDAT IS NULL THEN 1 ELSE 0 END) AS CUSCNT, " _
              &"SUM(CASE WHEN rtebtcust.DOCKETDAT IS NOT NULL AND rtebtcust.DROPDAT IS NULL THEN 1 ELSE 0 END) AS APPLYCNT, " _
              &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END AS DEVPMAN, " _
              &"case  WHEN RTEBTCMTYLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTEBTCMTYLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null " _
              &"then '未報竣' when RTEBTCMTYLINE.HINETNOTIFYDAT is not null then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL " _
              &"THEN '取得預定施工日' WHEN RTEBTCMTYLINE.linetel <> '' then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' " _
              &"WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' " _
              &"WHEN RTEBTCMTYLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END,case when RTEBTCMTYLINE.lockdat is null then '' else 'Y' END  " _
              &"FROM RTSalesGroup RIGHT OUTER JOIN RTEBTCMTYLINE ON RTSalesGroup.AREAID = RTEBTCMTYLINE.AREAID " _
              &"AND RTSalesGroup.GROUPID = RTEBTCMTYLINE.GROUPID AND RTSalesGroup.EDATE IS NULL LEFT OUTER JOIN RTCounty ON RTEBTCMTYLINE.CUTID = RTCounty.CUTID " _
              &"LEFT OUTER JOIN RTObj ON RTEBTCMTYLINE.CONSIGNEE = RTObj.CUSID LEFT OUTER JOIN RTEBTCUST ON " _
              &"RTEBTCMTYLINE.COMQ1 = RTEBTCUST.COMQ1 AND RTEBTCMTYLINE.LINEQ1 = RTEBTCUST.LINEQ1 inner join rtebtcmtyh on rtebtcmtyline.comq1=rtebtcmtyh.comq1 LEFT OUTER JOIN RTCODE ON rtebtcmtyline.LINERATE=RTCODE.CODE AND RTCODE.KIND='D3' " _
              &"WHERE RTEBTCMTYLINE.COMQ1<> 0 AND  RTEBTCmtyline.ComQ1<>0 AND RTEBTCmtyline.ADSLAPPLYDAT IS NOT NULL AND RTEBTCmtyline.DROPDAT IS null AND ( RTEBTCmtyline.MOVETOCOMQ1 IS NULL OR RTEBTCmtyline.MOVETOCOMQ1=0 ) AND " & SEARCHQRY  _
              &"GROUP BY RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1, RTEBTCMTYh.comn, " _
              &"rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1)), RTObj.SHORTNC, RTSalesGroup.GROUPNC, " _
              &"RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno,rtcode.codenc, RTEBTCMTYLINE.RCVDAT, RTEBTCMTYLINE.AGREE, " _
              &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, RTEBTCMTYLINE.HINETNOTIFYDAT, RTEBTCMTYLINE.ADSLAPPLYDAT, " _
              &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END, case  WHEN RTEBTCMTYLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTEBTCMTYLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL " _
              &"THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null then '未報竣' when RTEBTCMTYLINE.HINETNOTIFYDAT is not null " _
              &"then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL THEN '取得預定施工日' WHEN RTEBTCMTYLINE.linetel <> '' " _
              &"then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' " _
              &"WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' WHEN RTEBTCMTYLINE.AGREE = 'N' THEN '勘查為不可建' " _
              &"WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END,case when RTEBTCMTYLINE.lockdat is null then '' else 'Y' END  " _
              &" " & SEARCHQRY6
      Else
         sqlList="SELECT RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,rtebtcmtyh.comn,rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1))  as comqline, " _
           &"RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, " _
           &"rtcode.codenc, " _
           &"RTEBTCMTYLINE.RCVDAT, " _
           &"RTEBTCMTYLINE.AGREE, " _
           &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, " _
           &"RTEBTCMTYLINE.HINETNOTIFYDAT, " _
           &"RTEBTCMTYLINE.ADSLAPPLYDAT, " _
           &"SUM(CASE WHEN rtebtcust.cusid IS NOT NULL AND rtebtcust.DROPDAT IS NULL THEN 1 ELSE 0 END) AS CUSCNT, " _
           &"SUM(CASE WHEN rtebtcust.DOCKETdat IS NOT NULL AND rtebtcust.DROPDAT IS NULL THEN 1 ELSE 0 END) AS APPLYCNT, " _
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
           &"RTEBTCMTYLINE.LINEQ1 = RTEBTCUST.LINEQ1 inner join rtebtcmtyh on rtebtcmtyline.comq1=rtebtcmtyh.comq1 INNER JOIN RTAREATOWNSHIP ON RTEBTCMTYLINE.CUTID=RTAREATOWNSHIP.CUTID AND " _
           &"RTEBTCMTYLINE.TOWNSHIP=RTAREATOWNSHiP.TOWNSHIP  LEFT OUTER JOIN RTCODE ON rtebtcmtyline.LINERATE=RTCODE.CODE AND RTCODE.KIND='D3' " _
           &"WHERE RTEBTCMTYLINE.COMQ1<> 0 AND  RTEBTCmtyline.ComQ1<>0 AND RTEBTCmtyline.ADSLAPPLYDAT IS NOT NULL  AND RTEBTCmtyline.DROPDAT IS null  AND ( RTEBTCmtyline.MOVETOCOMQ1 IS NULL OR RTEBTCmtyline.MOVETOCOMQ1=0 ) AND " & SEARCHQRY & " AND RTAREATOWNSHIP.AREAID " & DAREAID & " " _
           &"GROUP BY RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1, RTEBTCMTYh.comn, rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1)), RTObj.SHORTNC, " _
           &"RTSalesGroup.GROUPNC, RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno, " _
           &"rtcode.codenc, " _
           &"RTEBTCMTYLINE.RCVDAT, " _
           &"RTEBTCMTYLINE.AGREE, " _
           &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, " _
           &"RTEBTCMTYLINE.HINETNOTIFYDAT, " _
           &"RTEBTCMTYLINE.ADSLAPPLYDAT,case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END, " _
           &"case  WHEN RTEBTCMTYLINE.CANCELDAT IS NOT NULL THEN '已作廢' WHEN RTEBTCMTYLINE.DROPDAT IS NOT NULL THEN '已撤線' when RTEBTCMTYLINE.APPLYUPLOADDAT IS NOT NULL THEN '開通結案' when RTEBTCMTYLINE.ADSLAPPLYDAT is not null then '未報竣' " _
           &"when RTEBTCMTYLINE.HINETNOTIFYDAT is not null then 'CHT已通知但尚未測通' WHEN RTEBTCMTYLINE.SCHAPPLYDAT IS NOT NULL THEN '取得預定施工日' " _
           &"WHEN RTEBTCMTYLINE.linetel <> '' then '取得附掛電話' when RTEBTCMTYLINE.lineip <> '' then '取得IP' WHEN RTEBTCMTYLINE.UPDEBTCHKDAT IS NOT NULL THEN '送件申請' " _
           &"WHEN RTEBTCMTYLINE.AGREE = 'Y' THEN '勘查為可建' WHEN  RTEBTCMTYLINE.AGREE = 'N' THEN '勘查為不可建' WHEN RTEBTCMTYLINE.INSPECTDAT IS NULL THEN '尚未勘查' ELSE '???未明???' END,case when RTEBTCMTYLINE.lockdat is null then '' else 'Y' END  " _
           &" " & SEARCHQRY6
             
    End If  
            '     response.Write sqllist         

End Sub
Sub SrRunUserDefineDelete()

End Sub
%>