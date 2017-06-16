<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="KTS管理系統"
  title="KTS用戶申請電話單資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=" 作 廢 ;作廢返轉"
  functionOptProgram="KTSCUSTTELDROP.asp;KTSCUSTTELDROPRTN.asp"
  functionOptPrompt="Y;Y"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="用戶代號;電話申請單號;申請日期;申請轉檔日;作廢日;申請門號數;開通完成數;作廢門號數;作廢完成數"
  sqlDelete="SELECT KTSCUSTTELH.CUSID, KTSCUSTTELH.PNO, KTSCUSTTELH.APPLYDAT,KTSCUSTTELH.TRANSDAT, KTSCUSTTELH.CANCELDAT, " _
           &"SUM(CASE WHEN KTSCUSTTELD1.AORD = 'A' THEN 1 ELSE 0 END), " _
           &"SUM(CASE WHEN KTSCUSTTELD1.AORD = 'A' and KTSCUSTTELD1.finishdat is not null THEN 1 ELSE 0 END), " _
           &"SUM(CASE WHEN KTSCUSTTELD1.AORD = 'D' THEN 1 ELSE 0 END), " _
           &"SUM(CASE WHEN KTSCUSTTELD1.AORD = 'D' and KTSCUSTTELD1.finishdat is not null THEN 1 ELSE 0 END) " _
           &"FROM KTSCUSTTELH LEFT OUTER JOIN KTSCUSTTELD1 ON KTSCUSTTELH.CUSID = KTSCUSTTELD1.CUSID AND " _
           &"KTSCUSTTELH.PNO = KTSCUSTTELD1.PNO GROUP BY  KTSCUSTTELH.CUSID, KTSCUSTTELH.PNO, KTSCUSTTELH.APPLYDAT, " _
           &"KTSCUSTTELH.TRANSDAT, KTSCUSTTELH.CANCELDAT "

  dataTable="ktscusttelh"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="KTSCusttelD.asp"
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
  searchProg="none"
  '----
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" KTSCusttelh.CUSID<>'' and KTSCUSTTELH.CUSID='" & aryparmkey(0) & "' "
     searchShow="全部"
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
  
         sqlList="SELECT KTSCUSTTELH.CUSID, KTSCUSTTELH.PNO, KTSCUSTTELH.APPLYDAT,KTSCUSTTELH.TRANSDAT, KTSCUSTTELH.CANCELDAT, " _
           &"SUM(CASE WHEN  KTSCUSTTELD1.AORD = 'A' THEN 1 ELSE 0 END), " _
           &"SUM(CASE WHEN KTSCUSTTELD1.AORD = 'A' and KTSCUSTTELD1.finishdat is not null THEN 1 ELSE 0 END), " _
           &"SUM(CASE WHEN  KTSCUSTTELD1.AORD = 'D' THEN 1 ELSE 0 END), " _
           &"SUM(CASE WHEN KTSCUSTTELD1.AORD = 'D' and KTSCUSTTELD1.finishdat is not null THEN 1 ELSE 0 END) " _
           &"FROM KTSCUSTTELH LEFT OUTER JOIN KTSCUSTTELD1 ON KTSCUSTTELH.CUSID = KTSCUSTTELD1.CUSID AND " _
           &"KTSCUSTTELH.PNO = KTSCUSTTELD1.PNO " _
           &"where " & searchqry & " " _
           &"GROUP BY  KTSCUSTTELH.CUSID, KTSCUSTTELH.PNO, KTSCUSTTELH.APPLYDAT, " _
           &"KTSCUSTTELH.TRANSDAT, KTSCUSTTELH.CANCELDAT " _
           
'  Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>