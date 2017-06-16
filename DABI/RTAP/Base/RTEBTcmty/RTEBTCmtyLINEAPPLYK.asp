<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="東森AVS管理系統"
  title="EBT已確認測通之主線可申請用戶查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  functionOptName="用戶維護"
  functionOptProgram="rtebtcustK.asp"
  functionOptPrompt="N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;主線;社區名稱;主線申請日;主線申請<BR>轉檔日;主線申請<BR>EBT回覆日;主線測通日;主線測通<BR>回報日;主線測通<BR>回報轉檔日;EBT確認<BR>測通日;全部戶數;完工數;報竣數;退租數;公關數;完工<BR>未報竣數;施工中;申請中;待申請數"
  sqlDelete="SELECT RTEBTCMTYLINE.COMQ1,RTEBTCMTYLINE.LINEQ1,rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1))  as comqline,RTEBTCMTYH.COMN,RTEBTCMTYLINE.UPDEBTCHKDAT,RTEBTCMTYLINE.UPDEBTDAT, " _
         &"RTEBTCMTYLINE.EBTREPLYDAT, RTEBTCMTYLINE.ADSLAPPLYDAT, RTEBTCMTYLINE.APPLYUPLOADDAT, RTEBTCMTYLINE.APPLYUPLOADTNS, " _
         &"RTEBTCMTYLINE.EBTAPPLYOKRTN, SUM(CASE WHEN RTEBTCUST.CANCELDAT IS NULL AND RTEBTCUST.COMQ1 IS NOT NULL THEN 1 ELSE 0 END) AS 全部客戶數, " _
         &"SUM(CASE WHEN RTEBTCUST.FINISHDAT IS NOT NULL AND RTEBTCUST.DROPDAT IS NULL AND RTEBTCUST.CANCELDAT IS NULL AND rtebtcust.freecode <> 'Y' THEN 1 ELSE 0 END) AS 完工數, " _
         &"SUM(CASE WHEN RTEBTCUST.CANCELDAT IS NULL AND RTEBTCUST.DOCKETDAT IS NOT NULL AND RTEBTCUST.DROPDAT IS NULL AND RTEBTCUST.CANCELDAT IS NULL AND rtebtcust.freecode <> 'Y' THEN 1 ELSE 0 END) AS 報竣數, " _
         &"SUM(CASE WHEN RTEBTCUST.CANCELDAT IS NULL AND RTEBTCUST.DROPDAT IS NOT NULL AND rtebtcust.freecode <> 'Y' THEN 1 ELSE 0 END) AS 退租數, " _
         &"SUM(CASE WHEN rtebtcust.freecode = 'Y' AND rtebtcust.CANCELDAT IS NULL THEN 1 ELSE 0 END) AS 公關數, " _
         &"SUM(CASE WHEN RTEBTCUST.FINISHDAT IS NOT NULL AND RTEBTCUST.DROPDAT IS NULL AND RTEBTCUST.CANCELDAT IS NULL AND rtebtcust.freecode <> 'Y' THEN 1 ELSE 0 END) " _
         &"- SUM(CASE WHEN RTEBTCUST.CANCELDAT IS NULL AND RTEBTCUST.DOCKETDAT IS NOT NULL AND RTEBTCUST.DROPDAT IS NULL AND RTEBTCUST.CANCELDAT IS NULL AND rtebtcust.freecode <> 'Y' THEN 1 ELSE 0 END) AS 完工未報竣數, " _
         &"SUM(CASE WHEN RTEBTCUST.CANCELDAT IS NULL AND RTEBTCUST.COMQ1 IS NOT NULL AND RTEBTCUST.AVSNO <> '' AND RTEBTCUST.FREECODE <> 'Y' AND RTEBTCUST.FINISHDAT IS NULL AND RTEBTCUST.DOCKETDAT IS NULL AND RTEBTCUST.DROPDAT IS NULL " _
         &"THEN 1 ELSE 0 END) AS 施工中, SUM(CASE WHEN RTEBTCUST.CANCELDAT IS NULL AND RTEBTCUST.COMQ1 IS NOT NULL AND RTEBTCUST.AVSNO = '' AND RTEBTCUST.FREECODE <> 'Y' AND RTEBTCUST.FINISHDAT IS NULL AND RTEBTCUST.DOCKETDAT IS NULL AND RTEBTCUST.DROPDAT IS NULL AND RTEBTCUST.APPLYDAT IS NOT NULL THEN 1 ELSE 0 END) AS 申請中, " _
         &"SUM(CASE WHEN RTEBTCUST.CANCELDAT IS NULL AND RTEBTCUST.COMQ1 IS NOT NULL AND RTEBTCUST.AVSNO = '' AND RTEBTCUST.FREECODE <> 'Y' AND RTEBTCUST.FINISHDAT IS NULL AND RTEBTCUST.DOCKETDAT IS NULL AND RTEBTCUST.DROPDAT IS NULL AND RTEBTCUST.APPLYDAT IS NULL THEN 1 ELSE 0 END) AS 待申請數 " _
         &"FROM RTEBTCMTYLINE INNER JOIN  RTEBTCMTYH ON  RTEBTCMTYLINE.COMQ1 = RTEBTCMTYH.COMQ1 LEFT OUTER JOIN RTEBTCUST ON RTEBTCMTYLINE.LINEQ1 = RTEBTCUST.LINEQ1 AND RTEBTCMTYLINE.COMQ1 = RTEBTCUST.COMQ1 " _
         &"WHERE  RTEBTCMTYLINE.EBTAPPLYOKRTN IS NOT NULL AND RTEBTCMTYLINE.DROPDAT IS NULL AND RTEBTCMTYLINE.CANCELDAT IS NULL " _
         &"GROUP BY  RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1)), RTEBTCMTYH.COMN,  RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.UPDEBTDAT, RTEBTCMTYLINE.EBTREPLYDAT, RTEBTCMTYLINE.ADSLAPPLYDAT, " _
         &"RTEBTCMTYLINE.APPLYUPLOADDAT, RTEBTCMTYLINE.APPLYUPLOADTNS,RTEBTCMTYLINE.EBTAPPLYOKRTN " 

  dataTable="rtebtcmtyline"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=640,height=250,scrollbars=yes"
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
  searchProg="RTEBTCMTYLINEAPPLYS.ASP"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTEBTCmtyline.ComQ1<>0 "
     searchqry2=" having SUM(CASE WHEN RTEBTCUST.CANCELDAT IS NULL AND RTEBTCUST.COMQ1 IS NOT NULL AND RTEBTCUST.AVSNO = '' AND " _
       &"RTEBTCUST.FREECODE <> 'Y' AND RTEBTCUST.FINISHDAT IS NULL AND RTEBTCUST.DOCKETDAT IS NULL AND RTEBTCUST.DROPDAT IS NULL AND " _
       &"RTEBTCUST.APPLYDAT IS NULL THEN 1 ELSE 0 END) > 0 "
     searchShow="待申請戶數 > 0 "
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
            'DAreaID=" and ( ( rtebtcmtyline.cutid in ('01','02','03','04','21','22') and rtebtcmtyline.township not in ('三峽鎮','鶯歌鎮') ) or (( rtebtcmtyline.cutid in  ('05','06','07','08') or (rtebtcmtyline.cutid='03' and rtebtcmtyline.township in ('三峽鎮','鶯歌鎮') ) ) ))"
            DAreaID="<>'*'"
         case "P"
            DAreaID=" and ( ( rtebtcmtyline.cutid in ('01','02','03','04','21','22') and rtebtcmtyline.township not in ('三峽鎮','鶯歌鎮') ) or (( rtebtcmtyline.cutid in  ('05','06','07','08') or (rtebtcmtyline.cutid='03' and rtebtcmtyline.township in ('三峽鎮','鶯歌鎮') ) )) )"
         case "C"
            DAreaID=" and rtebtcmtyline.cutid in ('09','10','11','12','13') "         
         case "K"
            DAreaID=" and rtebtcmtyline.cutid in ('14','15','16','17','18','19','20')  "         
         case else
            DareaID=" "
  end select
  
  '高階主管可讀取全部資料
  'if UCASE(emply)="T89001" or Ucase(emply)="T89002" or  Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="T93168" OR _
  '   Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T89076"  or Ucase(emply)="T91129"  or Ucase(emply)="T89031"  or Ucase(emply)="T92134"  or Ucase(emply)="P92010" then
  '   DAreaID="<>'*'"
  'end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  if userlevel=31  then DAreaID="<>'*'"
  
  '業務工程師只能讀取該工程師的社區
  'if userlevel=2 then
  '  If searchShow="全部" Then
 '台北窗口
  sqlList="SELECT RTEBTCMTYLINE.COMQ1,RTEBTCMTYLINE.LINEQ1,rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1))  as comqline,RTEBTCMTYH.COMN,RTEBTCMTYLINE.UPDEBTCHKDAT,RTEBTCMTYLINE.UPDEBTDAT, " _
         &"RTEBTCMTYLINE.EBTREPLYDAT, RTEBTCMTYLINE.ADSLAPPLYDAT, RTEBTCMTYLINE.APPLYUPLOADDAT, RTEBTCMTYLINE.APPLYUPLOADTNS, " _
         &"RTEBTCMTYLINE.EBTAPPLYOKRTN, SUM(CASE WHEN RTEBTCUST.CANCELDAT IS NULL AND RTEBTCUST.COMQ1 IS NOT NULL THEN 1 ELSE 0 END) AS 全部客戶數, " _
         &"SUM(CASE WHEN RTEBTCUST.FINISHDAT IS NOT NULL AND RTEBTCUST.DROPDAT IS NULL AND RTEBTCUST.CANCELDAT IS NULL AND rtebtcust.freecode <> 'Y' THEN 1 ELSE 0 END) AS 完工數, " _
         &"SUM(CASE WHEN RTEBTCUST.CANCELDAT IS NULL AND RTEBTCUST.DOCKETDAT IS NOT NULL AND RTEBTCUST.DROPDAT IS NULL AND RTEBTCUST.CANCELDAT IS NULL AND rtebtcust.freecode <> 'Y' THEN 1 ELSE 0 END) AS 報竣數, " _
         &"SUM(CASE WHEN RTEBTCUST.CANCELDAT IS NULL AND RTEBTCUST.DROPDAT IS NOT NULL AND rtebtcust.freecode <> 'Y' THEN 1 ELSE 0 END) AS 退租數, " _
         &"SUM(CASE WHEN rtebtcust.freecode = 'Y' AND rtebtcust.CANCELDAT IS NULL THEN 1 ELSE 0 END) AS 公關數, " _
         &"SUM(CASE WHEN RTEBTCUST.FINISHDAT IS NOT NULL AND RTEBTCUST.DROPDAT IS NULL AND RTEBTCUST.CANCELDAT IS NULL AND rtebtcust.freecode <> 'Y' THEN 1 ELSE 0 END) " _
         &"- SUM(CASE WHEN RTEBTCUST.CANCELDAT IS NULL AND RTEBTCUST.DOCKETDAT IS NOT NULL AND RTEBTCUST.DROPDAT IS NULL AND RTEBTCUST.CANCELDAT IS NULL AND rtebtcust.freecode <> 'Y' THEN 1 ELSE 0 END) AS 完工未報竣數, " _
         &"SUM(CASE WHEN RTEBTCUST.CANCELDAT IS NULL AND RTEBTCUST.COMQ1 IS NOT NULL AND RTEBTCUST.AVSNO <> '' AND RTEBTCUST.FREECODE <> 'Y' AND RTEBTCUST.FINISHDAT IS NULL AND RTEBTCUST.DOCKETDAT IS NULL AND RTEBTCUST.DROPDAT IS NULL " _
         &"THEN 1 ELSE 0 END) AS 施工中, SUM(CASE WHEN RTEBTCUST.CANCELDAT IS NULL AND RTEBTCUST.COMQ1 IS NOT NULL AND RTEBTCUST.AVSNO = '' AND RTEBTCUST.FREECODE <> 'Y' AND RTEBTCUST.FINISHDAT IS NULL AND RTEBTCUST.DOCKETDAT IS NULL AND RTEBTCUST.DROPDAT IS NULL AND RTEBTCUST.APPLYDAT IS NOT NULL THEN 1 ELSE 0 END) AS 申請中, " _
         &"SUM(CASE WHEN RTEBTCUST.CANCELDAT IS NULL AND RTEBTCUST.COMQ1 IS NOT NULL AND RTEBTCUST.AVSNO = '' AND RTEBTCUST.FREECODE <> 'Y' AND RTEBTCUST.FINISHDAT IS NULL AND RTEBTCUST.DOCKETDAT IS NULL AND RTEBTCUST.DROPDAT IS NULL AND RTEBTCUST.APPLYDAT IS NULL THEN 1 ELSE 0 END) AS 待申請數 " _
         &"FROM RTEBTCMTYLINE INNER JOIN  RTEBTCMTYH ON  RTEBTCMTYLINE.COMQ1 = RTEBTCMTYH.COMQ1 LEFT OUTER JOIN RTEBTCUST ON RTEBTCMTYLINE.LINEQ1 = RTEBTCUST.LINEQ1 AND RTEBTCMTYLINE.COMQ1 = RTEBTCUST.COMQ1 " _
         &"WHERE  RTEBTCMTYLINE.EBTAPPLYOKRTN IS NOT NULL AND RTEBTCMTYLINE.DROPDAT IS NULL AND RTEBTCMTYLINE.CANCELDAT IS NULL and " & searchqry _
         &"GROUP BY  RTEBTCMTYLINE.COMQ1, RTEBTCMTYLINE.LINEQ1,rtrim(convert(char(6),RTEBTcmtyline.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTcmtyline.lineQ1)), RTEBTCMTYH.COMN,  RTEBTCMTYLINE.UPDEBTCHKDAT, RTEBTCMTYLINE.UPDEBTDAT, RTEBTCMTYLINE.EBTREPLYDAT, RTEBTCMTYLINE.ADSLAPPLYDAT, " _
         &"RTEBTCMTYLINE.APPLYUPLOADDAT, RTEBTCMTYLINE.APPLYUPLOADTNS,RTEBTCMTYLINE.EBTAPPLYOKRTN " & searchqry2
  'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>