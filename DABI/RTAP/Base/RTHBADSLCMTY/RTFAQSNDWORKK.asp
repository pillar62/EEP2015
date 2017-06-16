<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="ADSL及Hibuilding管理系統"
  title="社區主機維修派工單資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="完工結案;未完工結案;結案返轉; 作 廢 ;作廢返轉;歷史異動"
  functionOptProgram="RTFAQsndworkF.asp;RTFAQsndworkUF.asp;RTFAQsndworkFR.asp;RTFAQsndworkdrop.asp;RTFAQsndworkdropc.asp;RTFAQsndworkLOGK.asp"
  functionOptPrompt="N;N;N;N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;派工單號;派工日期;預定施工員;實際施工員;完工結案日;未完工結案日;作廢日;獎金結算月;獎金審核日;庫存結算月;庫存審核日"
  sqlDelete="SELECT HBADSLCMTYFIXSNDWORK.FIXNO, HBADSLCMTYFIXSNDWORK.PRTNO, HBADSLCMTYFIXSNDWORK.SENDWORKDAT, " _
           &"CASE WHEN RTObj_4.CUSNC IS NULL OR RTObj_4.CUSNC = '' THEN RTObj_1.SHORTNC ELSE RTObj_4.CUSNC END, " _
           &"CASE WHEN RTObj_2.CUSNC IS NULL OR RTObj_2.CUSNC = '' THEN RTObj_3.SHORTNC ELSE RTObj_2.CUSNC END, " _
           &"HBADSLCMTYFIXSNDWORK.CLOSEDAT, HBADSLCMTYFIXSNDWORK.UNCLOSEDAT, HBADSLCMTYFIXSNDWORK.DROPDAT, " _
           &"HBADSLCMTYFIXSNDWORK.BONUSCLOSEYM, HBADSLCMTYFIXSNDWORK.BONUSCLOSEDAT, HBADSLCMTYFIXSNDWORK.STOCKCLOSEYM, " _
           &"HBADSLCMTYFIXSNDWORK.STOCKCLOSEDAT " _
           &"FROM RTObj RTObj_3 RIGHT OUTER JOIN HBADSLCMTYFIXSNDWORK ON RTObj_3.CUSID = HBADSLCMTYFIXSNDWORK.REALCONSIGNEE " _
           &"LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN RTObj RTObj_2 ON RTEmployee_1.CUSID = RTObj_2.CUSID ON " _
           &"HBADSLCMTYFIXSNDWORK.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN RTObj RTObj_1 ON " _
           &"HBADSLCMTYFIXSNDWORK.ASSIGNCONSIGNEE = RTObj_1.CUSID LEFT OUTER JOIN RTObj RTObj_4 INNER JOIN " _
           &"RTEmployee RTEmployee_2 ON RTObj_4.CUSID = RTEmployee_2.CUSID ON HBADSLCMTYFIXSNDWORK.ASSIGNENGINEER = RTEmployee_2.EMPLY "
  dataTable="HBADSLCMTYFIXSNDWORK"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="RTFAQSNDWORKD.asp"
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
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" HBADSLCMTYFIXSNDWORK.FIXNO='" & aryparmkey(0) & "' "
     searchShow="客訴單號︰"& aryparmkey(0) 
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
         sqlList="SELECT HBADSLCMTYFIXSNDWORK.FIXNO, HBADSLCMTYFIXSNDWORK.PRTNO, HBADSLCMTYFIXSNDWORK.SENDWORKDAT, " _
           &"CASE WHEN RTObj_4.CUSNC IS NULL OR RTObj_4.CUSNC = '' THEN RTObj_1.SHORTNC ELSE RTObj_4.CUSNC END, " _
           &"CASE WHEN RTObj_2.CUSNC IS NULL OR RTObj_2.CUSNC = '' THEN RTObj_3.SHORTNC ELSE RTObj_2.CUSNC END, " _
           &"HBADSLCMTYFIXSNDWORK.CLOSEDAT, HBADSLCMTYFIXSNDWORK.UNCLOSEDAT, HBADSLCMTYFIXSNDWORK.DROPDAT, " _
           &"HBADSLCMTYFIXSNDWORK.BONUSCLOSEYM, HBADSLCMTYFIXSNDWORK.BONUSCLOSEDAT, HBADSLCMTYFIXSNDWORK.STOCKCLOSEYM, " _
           &"HBADSLCMTYFIXSNDWORK.STOCKCLOSEDAT " _
           &"FROM RTObj RTObj_3 RIGHT OUTER JOIN HBADSLCMTYFIXSNDWORK ON RTObj_3.CUSID = HBADSLCMTYFIXSNDWORK.REALCONSIGNEE " _
           &"LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN RTObj RTObj_2 ON RTEmployee_1.CUSID = RTObj_2.CUSID ON " _
           &"HBADSLCMTYFIXSNDWORK.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN RTObj RTObj_1 ON " _
           &"HBADSLCMTYFIXSNDWORK.ASSIGNCONSIGNEE = RTObj_1.CUSID LEFT OUTER JOIN RTObj RTObj_4 INNER JOIN " _
           &"RTEmployee RTEmployee_2 ON RTObj_4.CUSID = RTEmployee_2.CUSID ON HBADSLCMTYFIXSNDWORK.ASSIGNENGINEER = RTEmployee_2.EMPLY " _
            &"where " & searchqry
    Else
         sqlList="SELECT HBADSLCMTYFIXSNDWORK.FIXNO, HBADSLCMTYFIXSNDWORK.PRTNO, HBADSLCMTYFIXSNDWORK.SENDWORKDAT, " _
           &"CASE WHEN RTObj_4.CUSNC IS NULL OR RTObj_4.CUSNC = '' THEN RTObj_1.SHORTNC ELSE RTObj_4.CUSNC END, " _
           &"CASE WHEN RTObj_2.CUSNC IS NULL OR RTObj_2.CUSNC = '' THEN RTObj_3.SHORTNC ELSE RTObj_2.CUSNC END, " _
           &"HBADSLCMTYFIXSNDWORK.CLOSEDAT, HBADSLCMTYFIXSNDWORK.UNCLOSEDAT, HBADSLCMTYFIXSNDWORK.DROPDAT, " _
           &"HBADSLCMTYFIXSNDWORK.BONUSCLOSEYM, HBADSLCMTYFIXSNDWORK.BONUSCLOSEDAT, HBADSLCMTYFIXSNDWORK.STOCKCLOSEYM, " _
           &"HBADSLCMTYFIXSNDWORK.STOCKCLOSEDAT " _
           &"FROM RTObj RTObj_3 RIGHT OUTER JOIN HBADSLCMTYFIXSNDWORK ON RTObj_3.CUSID = HBADSLCMTYFIXSNDWORK.REALCONSIGNEE " _
           &"LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN RTObj RTObj_2 ON RTEmployee_1.CUSID = RTObj_2.CUSID ON " _
           &"HBADSLCMTYFIXSNDWORK.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN RTObj RTObj_1 ON " _
           &"HBADSLCMTYFIXSNDWORK.ASSIGNCONSIGNEE = RTObj_1.CUSID LEFT OUTER JOIN RTObj RTObj_4 INNER JOIN " _
           &"RTEmployee RTEmployee_2 ON RTObj_4.CUSID = RTEmployee_2.CUSID ON HBADSLCMTYFIXSNDWORK.ASSIGNENGINEER = RTEmployee_2.EMPLY " _
           &"where " & searchqry
    End If  
  'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>