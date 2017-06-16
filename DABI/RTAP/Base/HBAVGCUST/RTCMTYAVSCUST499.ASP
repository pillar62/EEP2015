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
  FunctionOptName=""
  functionOptProgram=""
  functionOptPrompt=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;社區/主線序號;社區名稱;用戶名稱;裝機地址;完工日;報竣日;退租日;連絡電話;經銷商"
  sqlDelete="SELECT  a.COMQ1,a.LINEQ1,isnull(b.cusid,''),rtrim(convert(char(6),A.COMQ1)) +'-'+ rtrim(convert(char(6),A.lineQ1)), " _
         &"a.comn,isnull(B.CUSNC,''),isnull(d.cutnc+b.township1,''),isnull(b.finishdat,''),isnull(b.docketdat,''),b.dropdat," _
         &"isnull(b.CONTACTTEL,''),CASE WHEN (C.SHORTNC = '' or C.SHORTNC is null )  THEN " _
         &"CASE WHEN  a.CUTID IN ('01', '02', '03', '04', '21', '22')  AND a.township NOT IN ('三峽鎮', '鶯歌鎮')  " _
         &"THEN '台北' WHEN a.cutid IN ('05', '06', '07', '08') OR  ( a.cutid = '03' AND  a.township IN ('三峽鎮', '鶯歌鎮')) " _
         &"THEN '桃園' WHEN  a.cutid IN ('09', '10', '11', '12', '13')  THEN '台中' " _
         &"WHEN  a.cutid IN ('14', '15', '16', '17', '18', '19', '20') THEN '高雄' ELSE '' END ELSE C.SHORTNC END " _
         &"FROM " _
         &"( SELECT RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,rtebtcmtyh.comn ,rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) " _
         &"+'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1)) as comqline, RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL," _
         &"RTEBTCMTYLINE.applyno,rtcode.codenc, RTEBTCMTYLINE.RCVDAT, RTEBTCMTYLINE.AGREE, RTEBTCMTYLINE.UPDEBTCHKDAT, " _
         &"RTEBTCMTYLINE.EBTREPLYDAT, RTEBTCMTYLINE.HINETNOTIFYDAT, RTEBTCMTYLINE.ADSLAPPLYDAT, " _
         &"SUM(CASE WHEN rtebtcust.cusid IS NOT NULL AND rtebtcust.DROPDAT IS NULL THEN 1 ELSE 0 END) AS CUSCNT, " _
         &"SUM(CASE WHEN rtebtcust.DOCKETDAT IS NOT NULL AND rtebtcust.DROPDAT IS NULL THEN 1 ELSE 0 END) AS APPLYCNT, " _
         &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END AS DEVPMAN, " _
         &"RTEBTCMTYLINE.CUTID,RTEBTCMTYLINE.TOWNSHIP,RTEBTCMTYLINE.CONSIGNEE " _
         &"FROM RTSalesGroup RIGHT OUTER JOIN RTEBTCMTYLINE ON RTSalesGroup.AREAID = RTEBTCMTYLINE.AREAID AND " _
         &"RTSalesGroup.GROUPID = RTEBTCMTYLINE.GROUPID AND RTSalesGroup.EDATE IS NULL LEFT OUTER JOIN RTCounty ON " _
         &"RTEBTCMTYLINE.CUTID = RTCounty.CUTID LEFT OUTER JOIN RTObj ON RTEBTCMTYLINE.CONSIGNEE = RTObj.CUSID LEFT OUTER JOIN " _
         &"RTEBTCUST ON RTEBTCMTYLINE.COMQ1 = RTEBTCUST.COMQ1 AND RTEBTCMTYLINE.LINEQ1 = RTEBTCUST.LINEQ1 inner join " _
         &"rtebtcmtyh on rtebtcmtyline.comq1=rtebtcmtyh.comq1 LEFT OUTER JOIN RTCODE ON rtebtcmtyline.LINERATE=RTCODE.CODE AND " _
         &"RTCODE.KIND='D3' " _
         &"WHERE RTEBTCMTYLINE.COMQ1<> 0 AND RTEBTCmtyline.ComQ1<>0 AND RTEBTCmtyline.ADSLAPPLYDAT IS NOT NULL AND " _
         &"RTEBTCmtyline.DROPDAT IS null AND RTEBTCmtyline.ComQ1<>0 AND RTEBTCmtyline.ADSLAPPLYDAT IS NOT NULL AND " _
         &"RTEBTCmtyline.DROPDAT IS null " _
         &"GROUP BY RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1, RTEBTCMTYh.comn, rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) " _
         &"+'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1)), RTObj.SHORTNC, RTSalesGroup.GROUPNC, RTEBTCMTYLINE.LINEIP," _
         &"RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno,rtcode.codenc, RTEBTCMTYLINE.RCVDAT, RTEBTCMTYLINE.AGREE, " _
         &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, RTEBTCMTYLINE.HINETNOTIFYDAT, RTEBTCMTYLINE.ADSLAPPLYDAT, " _
         &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END, " _
         &"RTEBTCMTYLINE.CUTID,RTEBTCMTYLINE.TOWNSHIP," _
         &"RTEBTCMTYLINE.CONSIGNEE  ) A LEFT OUTER JOIN RTEBTCUST B ON A.COMQ1=B.COMQ1 AND A.LINEQ1=B.LINEQ1 " _
         &"LEFT OUTER JOIN RTOBJ C ON A.CONSIGNEE = C.CUSID LEFT OUTER JOIN RTCOUNTY D ON B.CUTID1=D.CUTID "
  dataTable="rtebtcmtyline"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="/WEBAP/RTAP/BASE/RTEBTCMTY/RTebtCUSTD.asp"
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
  searchProg="RTCMTYAVSCUST499S.ASP"
  '讀取暫存KEY
  KEYXX=SPLIT(SESSION("search6"),";")
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" A.COMQ1 <> 0 "
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
  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or  Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="T90076" OR _
     Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T89076"  or Ucase(emply)="T91129" or _
     Ucase(emply)="T89031"  or Ucase(emply)="T92134"  or Ucase(emply)="P92010" or Ucase(emply)="T94180" then
     DAreaID="<>'*'"
  end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  if userlevel=31  then DAreaID="<>'*'"
  
  '業務工程師只能讀取該工程師的社區
  'if userlevel=2 then
  '  If searchShow="全部" Then
  sqlList="SELECT  a.COMQ1,a.LINEQ1,isnull(b.cusid,''),rtrim(convert(char(6),A.COMQ1)) +'-'+ rtrim(convert(char(6),A.lineQ1)), " _
         &"a.comn,isnull(B.CUSNC,''),isnull(d.cutnc+b.township1,''),isnull(b.finishdat,''),isnull(b.docketdat,''),b.dropdat," _
         &"isnull(b.CONTACTTEL,''),CASE WHEN (C.SHORTNC = '' or C.SHORTNC is null )  THEN " _
         &"CASE WHEN  a.CUTID IN ('01', '02', '03', '04', '21', '22')  AND a.township NOT IN ('三峽鎮', '鶯歌鎮')  " _
         &"THEN '台北' WHEN a.cutid IN ('05', '06', '07', '08') OR  ( a.cutid = '03' AND  a.township IN ('三峽鎮', '鶯歌鎮')) " _
         &"THEN '桃園' WHEN  a.cutid IN ('09', '10', '11', '12', '13')  THEN '台中' " _
         &"WHEN  a.cutid IN ('14', '15', '16', '17', '18', '19', '20') THEN '高雄' ELSE '' END ELSE C.SHORTNC END " _
         &"FROM " _
         &"( SELECT RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,rtebtcmtyh.comn ,rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) " _
         &"+'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1)) as comqline, RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL," _
         &"RTEBTCMTYLINE.applyno,rtcode.codenc, RTEBTCMTYLINE.RCVDAT, RTEBTCMTYLINE.AGREE, RTEBTCMTYLINE.UPDEBTCHKDAT, " _
         &"RTEBTCMTYLINE.EBTREPLYDAT, RTEBTCMTYLINE.HINETNOTIFYDAT, RTEBTCMTYLINE.ADSLAPPLYDAT, " _
         &"SUM(CASE WHEN rtebtcust.cusid IS NOT NULL AND rtebtcust.DROPDAT IS NULL AND rtebtcust.FREECODE <> 'Y' THEN 1 ELSE 0 END) AS CUSCNT, " _
         &"SUM(CASE WHEN rtebtcust.DOCKETDAT IS NOT NULL AND rtebtcust.DROPDAT IS NULL AND rtebtcust.FREECODE <> 'Y' THEN 1 ELSE 0 END) AS APPLYCNT, " _
         &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END AS DEVPMAN, " _
         &"RTEBTCMTYLINE.CUTID,RTEBTCMTYLINE.TOWNSHIP,RTEBTCMTYLINE.CONSIGNEE " _
         &"FROM RTSalesGroup RIGHT OUTER JOIN RTEBTCMTYLINE ON RTSalesGroup.AREAID = RTEBTCMTYLINE.AREAID AND " _
         &"RTSalesGroup.GROUPID = RTEBTCMTYLINE.GROUPID AND RTSalesGroup.EDATE IS NULL LEFT OUTER JOIN RTCounty ON " _
         &"RTEBTCMTYLINE.CUTID = RTCounty.CUTID LEFT OUTER JOIN RTObj ON RTEBTCMTYLINE.CONSIGNEE = RTObj.CUSID LEFT OUTER JOIN " _
         &"RTEBTCUST ON RTEBTCMTYLINE.COMQ1 = RTEBTCUST.COMQ1 AND RTEBTCMTYLINE.LINEQ1 = RTEBTCUST.LINEQ1 inner join " _
         &"rtebtcmtyh on rtebtcmtyline.comq1=rtebtcmtyh.comq1 LEFT OUTER JOIN RTCODE ON rtebtcmtyline.LINERATE=RTCODE.CODE AND " _
         &"RTCODE.KIND='D3' " _
         &"WHERE RTEBTCMTYLINE.COMQ1<> 0 AND RTEBTCmtyline.ComQ1<>0 AND RTEBTCmtyline.ADSLAPPLYDAT IS NOT NULL AND " _
         &"RTEBTCmtyline.DROPDAT IS null " _
         &"GROUP BY RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1, RTEBTCMTYh.comn, rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) " _
         &"+'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1)), RTObj.SHORTNC, RTSalesGroup.GROUPNC, RTEBTCMTYLINE.LINEIP," _
         &"RTEBTCMTYLINE.LINETEL,RTEBTCMTYLINE.applyno,rtcode.codenc, RTEBTCMTYLINE.RCVDAT, RTEBTCMTYLINE.AGREE, " _
         &"RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.EBTREPLYDAT, RTEBTCMTYLINE.HINETNOTIFYDAT, RTEBTCMTYLINE.ADSLAPPLYDAT, " _
         &"case when RTObj.SHORTNC is NULL then RTSalesGroup.GROUPNC ELSE RTObj.SHORTNC END, " _
         &"RTEBTCMTYLINE.CUTID,RTEBTCMTYLINE.TOWNSHIP," _
         &"RTEBTCMTYLINE.CONSIGNEE " _
         &" " & SEARCHQRY6 &") a LEFT OUTER JOIN RTEBTCUST B ON A.COMQ1=B.COMQ1 AND A.LINEQ1=B.LINEQ1 " _
         &"LEFT OUTER JOIN RTOBJ C ON A.CONSIGNEE = C.CUSID LEFT OUTER JOIN RTCOUNTY D ON B.CUTID1=D.CUTID " _
         &"where b.docketdat is not null and b.dropdat is null AND " & SEARCHQRY & " " _
         &"order by a.comn,B.cusnc "
   ' response.Write sqllist         
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>