<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="東森AVS管理系統"
  title="AVS用戶M2欠費拆機派工單資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=" 列 印 ;完工結案;未完工結案;結案返轉; 作 廢 ;作廢返轉;設備明細;歷史異動"
  functionOptProgram="\RTAP\BASE\RTEBTCMTY\RTEBTCUSTm2m3SNDWORKPV.asp;rtebtCUSTm2m3sndworkF.asp;rtebtCUSTm2m3sndworkUF.asp;rtebtCUSTm2m3sndworkFR.asp;rtebtCUSTm2m3sndworkdrop.asp;rtebtCUSTm2m3sndworkdropc.asp;rtEBTCUSTm2m3SNDWORKhardwareK.asp;rtEBTCUSTm2m3sndworkLOGK.asp"
  functionOptPrompt="N;Y;N;N;N;N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;none;主線序號;社區名稱;拆機單號;派工日期;預定施工員;實際施工員;完工結案日;未完工結案日;作廢日"
  sqlDelete="SELECT RTEBTCUSTM2M3SNDWORK.AVSNO, RTEBTCUSTM2M3SNDWORK.M2M3, RTEBTCUSTM2M3SNDWORK.seq, RTEBTCUSTM2M3SNDWORK.PRTNO, RTRIM(LTRIM(CONVERT(char(6), RTEBTCUST.COMQ1))) " _
                &"+ '-' + RTRIM(LTRIM(CONVERT(char(6), RTEBTCUST.LINEQ1))) AS Expr3,RTEBTCMTYH.COMN, RTEBTCUSTM2M3SNDWORK.PRTNO, " _
                &"RTEBTCUSTM2M3SNDWORK.SENDWORKDAT, CASE WHEN RTObj_4.CUSNC IS NULL OR RTObj_4.CUSNC = '' THEN RTObj_1.SHORTNC " _
                &"ELSE RTObj_4.CUSNC END, CASE WHEN RTObj_2.CUSNC IS NULL OR RTObj_2.CUSNC = '' THEN RTObj_3.SHORTNC " _
                &"ELSE RTObj_2.CUSNC END, RTEBTCUSTM2M3SNDWORK.CLOSEDAT, RTEBTCUSTM2M3SNDWORK.UNCLOSEDAT,RTEBTCUSTM2M3SNDWORK.DROPDAT " _
                &"FROM  RTEBTCUSTM2M3SNDWORK LEFT OUTER JOIN  RTObj RTObj_4 INNER JOIN RTEmployee RTEmployee_2 ON RTObj_4.CUSID = RTEmployee_2.CUSID ON " _
                &"RTEBTCUSTM2M3SNDWORK.ASSIGNENGINEER = RTEmployee_2.EMPLY LEFT OUTER JOIN RTEBTCMTYH INNER JOIN RTEBTCUST ON " _
                &"RTEBTCMTYH.COMQ1 = RTEBTCUST.COMQ1 ON RTEBTCUSTM2M3SNDWORK.AVSNO = RTEBTCUST.AVSNO LEFT OUTER JOIN RTObj RTObj_3 ON " _
                &"RTEBTCUSTM2M3SNDWORK.REALCONSIGNEE = RTObj_3.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN RTObj RTObj_2 ON " _
                &"RTEmployee_1.CUSID = RTObj_2.CUSID ON  RTEBTCUSTM2M3SNDWORK.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
                &"RTObj RTObj_1 ON RTEBTCUSTM2M3SNDWORK.ASSIGNCONSIGNEE = RTObj_1.CUSID "
  dataTable="RTEBTCUSTM2M3SNDWORK"
  userDefineDelete="Yes"
  numberOfKey=4
  dataProg="RTEBTCUSTM2M3SNDWORKD.asp"
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
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTEBTCUSTM2M3SNDWORK.AVSNO='" & aryparmkey(0) & "' and RTEBTCUSTM2M3SNDWORK.M2M3='" & aryparmkey(1) & "' and RTEBTCUSTM2M3SNDWORK.seq=" & aryparmkey(2)  
     searchShow="AVS合約編號︰"& aryparmkey(0) & ",M2M3︰" & aryparmkey(1) 
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
  if userlevel=31 then DAreaID="<>'*'"
  
         sqlList="SELECT RTEBTCUSTM2M3SNDWORK.AVSNO, RTEBTCUSTM2M3SNDWORK.M2M3, RTEBTCUSTM2M3SNDWORK.seq, RTEBTCUSTM2M3SNDWORK.PRTNO, RTRIM(LTRIM(CONVERT(char(6), RTEBTCUST.COMQ1))) " _
                &"+ '-' + RTRIM(LTRIM(CONVERT(char(6), RTEBTCUST.LINEQ1))) AS Expr3,RTEBTCMTYH.COMN, RTEBTCUSTM2M3SNDWORK.PRTNO, " _
                &"RTEBTCUSTM2M3SNDWORK.SENDWORKDAT, CASE WHEN RTObj_4.CUSNC IS NULL OR RTObj_4.CUSNC = '' THEN RTObj_1.SHORTNC " _
                &"ELSE RTObj_4.CUSNC END, CASE WHEN RTObj_2.CUSNC IS NULL OR RTObj_2.CUSNC = '' THEN RTObj_3.SHORTNC " _
                &"ELSE RTObj_2.CUSNC END, RTEBTCUSTM2M3SNDWORK.CLOSEDAT, RTEBTCUSTM2M3SNDWORK.UNCLOSEDAT,RTEBTCUSTM2M3SNDWORK.DROPDAT " _
                &"FROM  RTEBTCUSTM2M3SNDWORK LEFT OUTER JOIN  RTObj RTObj_4 INNER JOIN RTEmployee RTEmployee_2 ON RTObj_4.CUSID = RTEmployee_2.CUSID ON " _
                &"RTEBTCUSTM2M3SNDWORK.ASSIGNENGINEER = RTEmployee_2.EMPLY LEFT OUTER JOIN RTEBTCMTYH INNER JOIN RTEBTCUST ON " _
                &"RTEBTCMTYH.COMQ1 = RTEBTCUST.COMQ1 ON RTEBTCUSTM2M3SNDWORK.AVSNO = RTEBTCUST.AVSNO LEFT OUTER JOIN RTObj RTObj_3 ON " _
                &"RTEBTCUSTM2M3SNDWORK.REALCONSIGNEE = RTObj_3.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN RTObj RTObj_2 ON " _
                &"RTEmployee_1.CUSID = RTObj_2.CUSID ON  RTEBTCUSTM2M3SNDWORK.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
                &"RTObj RTObj_1 ON RTEBTCUSTM2M3SNDWORK.ASSIGNCONSIGNEE = RTObj_1.CUSID " _
                &"WHERE " & SEARCHQRY
  'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>