<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="東森AVS管理系統"
  title="東森AVS用戶M2已拆機用戶復機處理"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="復機派工"
  functionOptProgram="RTEBTCUSTM2RTNSNDWORKK.asp"
  functionOptPrompt="N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;M2/M3;主線序號;社區名稱;用戶名稱;AVS合約編號;身份證號;緩催標記;法催標記;入暫繳標記;none;結案日;作廢日;拆機單號;復機單號;復機完工日"
  sqlDelete="SELECT RTEBTCUSTM2M3.AVSNO, RTEBTCUSTM2M3.M2M3, RTEBTCUSTM2M3.seq,CASE WHEN RTEBTCUSTM2M3.M2M3='302' THEN 'M2' " _
              &"WHEN RTEBTCUSTM2M3.M2M3='303' THEN 'M3' ELSE '' END, RTRIM(LTRIM(CONVERT(char(6), RTEBTCUST.COMQ1))) + '-' + " _
              &"RTRIM(LTRIM(CONVERT(char(6), RTEBTCUST.LINEQ1))) AS Expr1, RTEBTCMTYH.COMN, RTEBTCUSTM2M3.CUSNC," _
              &"RTEBTCUSTM2M3.AVSNO, RTEBTCUSTM2M3.SOCIALID, RTEBTCUSTM2M3.ARCSHOLDFLAG, RTEBTCUSTM2M3.ARCSLAWPUSHFLAG," _
              &"RTEBTCUSTM2M3.ARCSTEMPPAYFLAG, RTEBTCUSTM2M3.STOPBILLINGFLAG, RTEBTCUSTM2M3.CLOSEDAT, RTEBTCUSTM2M3.DROPDAT, " _
              &"RTEBTCUSTM2M3SNDWORK.PRTNO FROM RTEBTCUSTM2M3SNDWORK RIGHT OUTER JOIN RTEBTCUSTM2M3 ON " _
              &"RTEBTCUSTM2M3SNDWORK.AVSNO = RTEBTCUSTM2M3.AVSNO AND RTEBTCUSTM2M3SNDWORK.M2M3 = RTEBTCUSTM2M3.M2M3 AND " _
              &"RTEBTCUSTM2M3SNDWORK.DROPDAT IS NULL LEFT OUTER JOIN RTEBTCMTYH INNER JOIN RTEBTCUST ON " _
              &"RTEBTCMTYH.COMQ1 = RTEBTCUST.COMQ1 ON RTEBTCUSTM2M3.AVSNO = RTEBTCUST.AVSNO " _
              &"WHERE (rtebtcust.comq1 <> 0 ) and (RTEBTCmtyH.ComN <> '*' ) AND (RTEBTCustM2M3.M2M3='302') " _
              &"AND (RTEBTCustM2M3.CLOSEDAT IS NOT NULL ) AND RTEBTCUSTM2M3.DROPDAT IS NULL "
           
  dataTable="RTEBTCUSTM2M3"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="None"
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
  searchProg="RTEBTCUSTM2S.ASP"

  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTEBTCUSTM2M3.CLOSEDAT IS not NULL AND RTEBTCUSTM2M3.DROPDAT IS NULL "
     searchShow="全部尚未處理資料"
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
  'if UCASE(emply)="T89001" or Ucase(emply)="T89002" or  Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="T90076" OR _
  '   Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T89076"then
  '   DAreaID="<>'*'"
  'end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  if userlevel=31 or userlevel =1  or userlevel =5 then DAreaID="<>'*'"
  
       sqlList="SELECT RTEBTCUSTM2M3.AVSNO, RTEBTCUSTM2M3.M2M3, RTEBTCUSTM2M3.seq,CASE WHEN RTEBTCUSTM2M3.M2M3='302' THEN 'M2' " _
              &"WHEN RTEBTCUSTM2M3.M2M3='303' THEN 'M3' ELSE '' END, RTRIM(LTRIM(CONVERT(char(6), RTEBTCUST.COMQ1))) + '-' + " _
              &"RTRIM(LTRIM(CONVERT(char(6), RTEBTCUST.LINEQ1))) AS Expr1, RTEBTCMTYH.COMN, RTEBTCUSTM2M3.CUSNC," _
              &"RTEBTCUSTM2M3.AVSNO, RTEBTCUSTM2M3.SOCIALID, RTEBTCUSTM2M3.ARCSHOLDFLAG, RTEBTCUSTM2M3.ARCSLAWPUSHFLAG," _
              &"RTEBTCUSTM2M3.ARCSTEMPPAYFLAG, RTEBTCUSTM2M3.STOPBILLINGFLAG, RTEBTCUSTM2M3.CLOSEDAT, RTEBTCUSTM2M3.DROPDAT, " _
              &"RTEBTCUSTM2M3SNDWORK.PRTNO,RTEBTCUSTM2RTNSNDWORK.RTNNO,RTEBTCUSTM2RTNSNDWORK.CLOSEDAT " _
              &"FROM RTEBTCUSTM2M3SNDWORK RIGHT OUTER JOIN RTEBTCUSTM2M3 ON " _
              &"RTEBTCUSTM2M3SNDWORK.AVSNO = RTEBTCUSTM2M3.AVSNO AND RTEBTCUSTM2M3SNDWORK.M2M3 = RTEBTCUSTM2M3.M2M3 AND " _
              &"RTEBTCUSTM2M3SNDWORK.DROPDAT IS NULL LEFT OUTER JOIN RTEBTCMTYH INNER JOIN RTEBTCUST ON " _
              &"RTEBTCMTYH.COMQ1 = RTEBTCUST.COMQ1 ON RTEBTCUSTM2M3.AVSNO = RTEBTCUST.AVSNO " _
              &"LEFT OUTER JOIN RTEBTCUSTM2RTNSNDWORK ON RTEBTCUSTM2M3.AVSNO=RTEBTCUSTM2RTNSNDWORK.AVSNO AND " _
              &"RTEBTCUSTM2M3.M2M3=RTEBTCUSTM2RTNSNDWORK.M2M3 AND RTEBTCUSTM2RTNSNDWORK.DROPDAT IS NULL AND RTEBTCUSTM2RTNSNDWORK.UNCLOSEDAT IS NULL " _
              &"WHERE (rtebtcust.comq1 <> 0 ) and (RTEBTCmtyH.ComN <> '*' ) AND (RTEBTCustM2M3.M2M3='302') " _
              &"AND (RTEBTCustM2M3.CLOSEDAT IS NOT NULL ) AND RTEBTCUSTM2M3.DROPDAT IS NULL AND RTEBTCUSTM2RTNSNDWORK.CLOSEDAT IS NULL and " & SEARCHQRY & " " _
              &"ORDER BY RTEBTCUST.COMQ1,RTEBTCUST.LINEQ1,RTEBTCUSTM2M3.M2M3  " 
  'end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>