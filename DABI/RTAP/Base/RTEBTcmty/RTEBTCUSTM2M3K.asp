<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="東森AVS管理系統"
  title="東森AVS用戶欠費M2/M3資料處理"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="M2派拆機;M3欠拆; 作  廢 ;作廢返轉"
  functionOptProgram="RTEBTCUSTM2M3SNDWORKK.asp;RTEBTCUSTM3UPD.asp;RTEBTCUSTM2M3DROP.ASP;RTEBTCUSTM2M3DROPRTN.ASP"
  functionOptPrompt="N;Y;Y;Y"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;M2/M3;單次;主線序號;社區名稱;用戶名稱;AVS合約編號;身份證號;緩催標記;法催標記;入暫繳標記;停止計費標記;結案日;作廢日;拆機單號"
  sqlDelete="SELECT RTEBTCUSTM2M3.AVSNO, RTEBTCUSTM2M3.M2M3, RTEBTCUSTM2M3.SEQ,CASE WHEN RTEBTCUSTM2M3.M2M3='302' THEN 'M2' WHEN RTEBTCUSTM2M3.M2M3='303' THEN 'M3' ELSE '' END, RTEBTCUSTM2M3.SEQ, RTRIM(LTRIM(CONVERT(char(6), RTEBTCUST.COMQ1))) " _
           &"+ '-' + RTRIM(LTRIM(CONVERT(char(6), RTEBTCUST.LINEQ1))) AS Expr1, RTEBTCMTYH.COMN, RTEBTCUSTM2M3.CUSNC,RTEBTCUSTM2M3.AVSNO, " _
           &"RTEBTCUSTM2M3.SOCIALID, RTEBTCUSTM2M3.ARCSHOLDFLAG, RTEBTCUSTM2M3.ARCSLAWPUSHFLAG,RTEBTCUSTM2M3.ARCSTEMPPAYFLAG, " _
           &"RTEBTCUSTM2M3.STOPBILLINGFLAG, RTEBTCUSTM2M3.CLOSEDAT, RTEBTCUSTM2M3.DROPDAT, RTEBTCUSTM2M3SNDWORK.PRTNO " _
           &"FROM RTEBTCUSTM2M3SNDWORK RIGHT OUTER JOIN RTEBTCUSTM2M3 ON RTEBTCUSTM2M3SNDWORK.AVSNO = RTEBTCUSTM2M3.AVSNO AND " _
           &"RTEBTCUSTM2M3SNDWORK.M2M3 = RTEBTCUSTM2M3.M2M3 AND RTEBTCUSTM2M3SNDWORK.DROPDAT IS NULL LEFT OUTER JOIN " _
           &"RTEBTCMTYH INNER JOIN RTEBTCUST ON RTEBTCMTYH.COMQ1 = RTEBTCUST.COMQ1 ON RTEBTCUSTM2M3.AVSNO = RTEBTCUST.AVSNO "
           
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
  searchProg="RTEBTCUSTM2M3S.ASP"

  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTEBTCUSTM2M3.CLOSEDAT IS NULL AND RTEBTCUSTM2M3.DROPDAT IS NULL "
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
  
       sqlList="SELECT RTEBTCUSTM2M3.AVSNO, RTEBTCUSTM2M3.M2M3, RTEBTCUSTM2M3.SEQ,CASE WHEN RTEBTCUSTM2M3.M2M3='302' THEN 'M2' WHEN RTEBTCUSTM2M3.M2M3='303' THEN 'M3' WHEN RTEBTCUSTM2M3.M2M3='304' THEN 'M4' ELSE '' END, RTEBTCUSTM2M3.SEQ, RTRIM(LTRIM(CONVERT(char(6), RTEBTCUST.COMQ1))) " _
           &"+ '-' + RTRIM(LTRIM(CONVERT(char(6), RTEBTCUST.LINEQ1))) AS Expr1, RTEBTCMTYH.COMN, RTEBTCUSTM2M3.CUSNC,RTEBTCUSTM2M3.AVSNO, " _
           &"RTEBTCUSTM2M3.SOCIALID, RTEBTCUSTM2M3.ARCSHOLDFLAG, RTEBTCUSTM2M3.ARCSLAWPUSHFLAG,RTEBTCUSTM2M3.ARCSTEMPPAYFLAG, " _
           &"RTEBTCUSTM2M3.STOPBILLINGFLAG, RTEBTCUSTM2M3.CLOSEDAT, RTEBTCUSTM2M3.DROPDAT, RTEBTCUSTM2M3SNDWORK.PRTNO " _
           &"FROM RTEBTCUSTM2M3SNDWORK RIGHT OUTER JOIN RTEBTCUSTM2M3 ON RTEBTCUSTM2M3SNDWORK.AVSNO = RTEBTCUSTM2M3.AVSNO AND " _
           &"RTEBTCUSTM2M3SNDWORK.M2M3 = RTEBTCUSTM2M3.M2M3 AND RTEBTCUSTM2M3SNDWORK.SEQ=RTEBTCUSTM2M3.SEQ AND RTEBTCUSTM2M3SNDWORK.DROPDAT IS NULL LEFT OUTER JOIN " _
           &"RTEBTCMTYH INNER JOIN RTEBTCUST ON RTEBTCMTYH.COMQ1 = RTEBTCUST.COMQ1 ON RTEBTCUSTM2M3.AVSNO = RTEBTCUST.AVSNO " _
           &"WHERE " & SEARCHQRY & " " _
           &"ORDER BY RTEBTCUST.COMQ1,RTEBTCUST.LINEQ1,RTEBTCUSTM2M3.M2M3, RTEBTCUSTM2M3.SEQ "
  'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>