<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="東森AVS管理系統"
  title="AVS主線異動申請單維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  functionOptName="完工結案;異動派工;歷史異動"
  functionOptProgram="rtEBTcmtylineCHGf.asp;rtEBTcmtylineCHGsndworkk.asp;rtEBTcmtylineCHGLOGK.asp"
  functionOptPrompt="Y;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;社區名稱;異動單號;主線序號;異動項目;主線IP;主線附掛;異動申請日;新主線序號;新社區名稱;異動派工單號;派工日期"
  sqlDelete="SELECT RTEBTCMTYLINECHG.COMQ1, RTEBTCMTYLINECHG.LINEQ1,RTEBTCMTYLINECHG.PRTNO, RTEBTCMTYH.COMN, " _
           &"RTEBTCMTYLINECHG.PRTNO AS Expr1, RTRIM(CONVERT(char(6),RTEBTCMTYLINECHG.COMQ1)) + '-' + RTRIM(CONVERT(char(6), RTEBTCMTYLINECHG.LINEQ1)) AS comqline, " _
           &"CASE WHEN RTEBTCMTYLINECHG.CHGCOD1 = 1 THEN '512K/64K轉1.5M/384K' ELSE '' END + CASE WHEN RTEBTCMTYLINECHG.CHGCOD1 = 1 AND " _
           &"RTEBTCMTYLINECHG.CHGCOD2 = 1 THEN '／' ELSE '' END + CASE WHEN RTEBTCMTYLINECHG.CHGCOD2 = 1 THEN '1.5M/384K轉512K/64K' ELSE '' END " _
           &"+ CASE WHEN RTEBTCMTYLINECHG.CHGCOD1 = 1 AND RTEBTCMTYLINECHG.CHGCOD3 = 1 OR RTEBTCMTYLINECHG.CHGCOD2 = 1 AND RTEBTCMTYLINECHG.CHGCOD3 = 1 THEN '／' ELSE '' END " _
           &"+ CASE WHEN RTEBTCMTYLINECHG.CHGCOD3 = 1 THEN '移機' ELSE '' END, RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL, RTEBTCMTYLINECHG.APPLYDAT, " _
           &"RTRIM(CONVERT(char(6), RTEBTCMTYLINECHG.NCOMQ1)) + '-' + RTRIM(CONVERT(char(6), RTEBTCMTYLINECHG.NLINEQ1))  AS Ncomqline, RTEBTCMTYH_1.COMN AS Expr2, " _
           &"RTEBTCMTYLINECHGSNDWORK.PRTNO2,RTEBTCMTYLINECHGSNDWORK.SENDWORKDAT " _
           &"FROM RTEBTCMTYLINECHG INNER JOIN RTEBTCMTYH ON RTEBTCMTYLINECHG.COMQ1 = RTEBTCMTYH.COMQ1 INNER JOIN RTEBTCMTYLINE ON " _
           &"RTEBTCMTYLINECHG.COMQ1 = RTEBTCMTYLINE.COMQ1 AND RTEBTCMTYLINECHG.LINEQ1 = RTEBTCMTYLINE.LINEQ1 LEFT OUTER JOIN " _
           &"RTEBTCMTYLINECHGSNDWORK ON RTEBTCMTYLINECHG.COMQ1 = RTEBTCMTYLINECHGSNDWORK.COMQ1 AND RTEBTCMTYLINECHG.LINEQ1 = RTEBTCMTYLINECHGSNDWORK.LINEQ1 AND " _
           &"RTEBTCMTYLINECHG.PRTNO = RTEBTCMTYLINECHGSNDWORK.PRTNO AND RTEBTCMTYLINECHGSNDWORK.DROPDAT IS NULL LEFT OUTER JOIN " _
           &"RTEBTCMTYH RTEBTCMTYH_1 ON RTEBTCMTYLINECHG.NCOMQ1 = RTEBTCMTYH_1.COMQ1 " _
           &"WHERE RTEBTCMTYLINECHG.COMQ1=0 "
  dataTable="RTEBTCMTYLINECHG"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="RTebtCmtylineCHGD.asp"
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

  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTEBTCMTYLINECHG.FINISHDAT IS NULL "
     searchShow="全部待派工主線"
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
  
       sqlList="SELECT RTEBTCMTYLINECHG.COMQ1, RTEBTCMTYLINECHG.LINEQ1,RTEBTCMTYLINECHG.PRTNO, RTEBTCMTYH.COMN, " _
           &"RTEBTCMTYLINECHG.PRTNO AS Expr1, RTRIM(CONVERT(char(6),RTEBTCMTYLINECHG.COMQ1)) + '-' + RTRIM(CONVERT(char(6), RTEBTCMTYLINECHG.LINEQ1)) AS comqline, " _
           &"CASE WHEN RTEBTCMTYLINECHG.CHGCOD1 = 1 THEN '512K/64K轉1.5M/384K' ELSE '' END + CASE WHEN RTEBTCMTYLINECHG.CHGCOD1 = 1 AND " _
           &"RTEBTCMTYLINECHG.CHGCOD2 = 1 THEN '／' ELSE '' END + CASE WHEN RTEBTCMTYLINECHG.CHGCOD2 = 1 THEN '1.5M/384K轉512K/64K' ELSE '' END " _
           &"+ CASE WHEN RTEBTCMTYLINECHG.CHGCOD1 = 1 AND RTEBTCMTYLINECHG.CHGCOD3 = 1 OR RTEBTCMTYLINECHG.CHGCOD2 = 1 AND RTEBTCMTYLINECHG.CHGCOD3 = 1 THEN '／' ELSE '' END " _
           &"+ CASE WHEN RTEBTCMTYLINECHG.CHGCOD3 = 1 THEN '移機' ELSE '' END, RTEBTCMTYLINE.LINEIP,RTEBTCMTYLINE.LINETEL, RTEBTCMTYLINECHG.APPLYDAT, " _
           &"RTRIM(CONVERT(char(6), RTEBTCMTYLINECHG.NCOMQ1)) + '-' + RTRIM(CONVERT(char(6), RTEBTCMTYLINECHG.NLINEQ1))  AS Ncomqline, RTEBTCMTYH_1.COMN AS Expr2, " _
           &"RTEBTCMTYLINECHGSNDWORK.PRTNO2,RTEBTCMTYLINECHGSNDWORK.SENDWORKDAT " _
           &"FROM RTEBTCMTYLINECHG INNER JOIN RTEBTCMTYH ON RTEBTCMTYLINECHG.COMQ1 = RTEBTCMTYH.COMQ1 INNER JOIN RTEBTCMTYLINE ON " _
           &"RTEBTCMTYLINECHG.COMQ1 = RTEBTCMTYLINE.COMQ1 AND RTEBTCMTYLINECHG.LINEQ1 = RTEBTCMTYLINE.LINEQ1 LEFT OUTER JOIN " _
           &"RTEBTCMTYLINECHGSNDWORK ON RTEBTCMTYLINECHG.COMQ1 = RTEBTCMTYLINECHGSNDWORK.COMQ1 AND RTEBTCMTYLINECHG.LINEQ1 = RTEBTCMTYLINECHGSNDWORK.LINEQ1 AND " _
           &"RTEBTCMTYLINECHG.PRTNO = RTEBTCMTYLINECHGSNDWORK.PRTNO AND RTEBTCMTYLINECHGSNDWORK.DROPDAT IS NULL LEFT OUTER JOIN " _
           &"RTEBTCMTYH RTEBTCMTYH_1 ON RTEBTCMTYLINECHG.NCOMQ1 = RTEBTCMTYH_1.COMQ1 " _
           &"WHERE  " & SEARCHQRY
                      
  'end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>