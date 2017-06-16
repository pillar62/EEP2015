<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="東森AVS管理系統"
  title="AVS用戶服務異動進度查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="移機工單;完工結案;異動記錄"
  functionOptProgram="rtebtcustchgsndworkk2.asp;RTEBTCUSTCHGF.ASP;RTEBTCUSTCHGLOGK.ASP"
  functionOptPrompt="N;Y;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;none;<CENTER>主線</CENTER>;<CENTER>社區名稱</CENTER>;<CENTER>用戶<BR>合約編號</CENTER>;<CENTER>用戶名</CENTER>;<CENTER>異動<BR>申請日</CENTER>;<CENTER>異動項目</CENTER>;<CENTER>異動<BR>作廢日</CENTER>;<CENTER>異動轉檔<BR>審核日</CENTER>;<CENTER>異動<BR>轉檔日</CENTER>;<CENTER>移機<BR>完工日</CENTER>;<CENTER>完工<BR>回報日</CENTER>;<CENTER>新社區<BR>主線</CENTER>;<CENTER>新社區<BR>名稱</CENTER>;<CENTER>移機<BR>派工單</CENTER>;<CENTER>派工日</CENTER>;<CENTER>預定<BR>移機員</CENTER>;<CENTER>實際<BR>移機員</CENTER>"
  sqlDelete="SELECT RTEBTCUSTCHG.COMQ1, RTEBTCUSTCHG.LINEQ1, RTEBTCUSTCHG.CUSID, RTEBTCUSTCHG.ENTRYNO, RTRIM(CONVERT(char(6), " _
           &"RTEBTCUSTCHG.COMQ1)) + '-' + RTRIM(CONVERT(char(6), RTEBTCUSTCHG.LINEQ1)) AS Expr1, RTEBTCMTYH.COMN, " _
           &"RTEBTCUST.AVSNO, RTEBTCUST.CUSNC, RTEBTCUSTCHG.APPLYDAT," _
           &"CASE WHEN RTEBTCUSTCHG.CHGCOD1 = 1 THEN '用戶資料' ELSE '' END + '/' + " _
           &"CASE WHEN RTEBTCUSTCHG.CHGCOD2 = 1 THEN '移機' ELSE '' END + '/' + " _
           &"CASE WHEN RTEBTCUSTCHG.CHGCOD3 = 1 THEN '換號' ELSE '' END + '/' + " _
           &"CASE WHEN RTEBTCUSTCHG.CHGCOD4 = 1 THEN '其它' ELSE '' END, RTEBTCUSTCHG.DROPDAT, RTEBTCUSTCHG.TRANSCHKDAT, " _
           &"RTEBTCUSTCHG.TRANSDAT, RTEBTCUSTCHG.FINISHDAT, RTEBTCUSTCHG.FINISHCHKDAT, RTRIM(CONVERT(char(6), " _
           &"RTEBTCUSTCHG.NCOMQ1)) + '-' + RTRIM(CONVERT(char(6), RTEBTCUSTCHG.NLINEQ1)) AS Expr2, RTEBTCMTYH_1.COMN AS Expr3, " _
           &"RTEBTCUSTCHGSNDWORK.PRTNO, RTEBTCUSTCHGSNDWORK.SENDWORKDAT, case when RTObj.CUSNC is null then RTObj_1.SHORTNC else RTObj.CUSNC end, CASE when RTObj_2.CUSNC is null then  " _
           &"RTObj_3.SHORTNC else RTObj_2.CUSNC end " _
           &"FROM RTObj RTObj_3 RIGHT OUTER JOIN RTObj RTObj_1 INNER JOIN RTEBTCUSTCHGSNDWORK ON RTObj_1.CUSID = " _
           &"RTEBTCUSTCHGSNDWORK.ASSIGNCONSIGNEE ON RTObj_3.CUSID = RTEBTCUSTCHGSNDWORK.REALCONSIGNEE LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_1 INNER JOIN RTObj RTObj_2 ON RTEmployee_1.CUSID = RTObj_2.CUSID ON " _
           &"RTEBTCUSTCHGSNDWORK.REALENGINEER = RTEmployee_1.EMPLY RIGHT OUTER JOIN RTEBTCUSTCHG INNER JOIN RTEBTCMTYH ON " _
           &"RTEBTCUSTCHG.COMQ1 = RTEBTCMTYH.COMQ1 INNER JOIN RTEBTCMTYLINE ON RTEBTCUSTCHG.COMQ1 = RTEBTCMTYLINE.COMQ1 AND " _
           &"RTEBTCUSTCHG.LINEQ1 = RTEBTCMTYLINE.LINEQ1 INNER JOIN RTEBTCUST ON RTEBTCUSTCHG.COMQ1 = RTEBTCUST.COMQ1 AND " _
           &"RTEBTCUSTCHG.LINEQ1 = RTEBTCUST.LINEQ1 AND RTEBTCUSTCHG.CUSID = RTEBTCUST.CUSID LEFT OUTER JOIN " _
           &"RTEBTCMTYLINE RTEBTCMTYLINE_1 ON RTEBTCUSTCHG.NCOMQ1 = RTEBTCMTYLINE_1.COMQ1 AND " _
           &"RTEBTCUSTCHG.NLINEQ1 = RTEBTCMTYLINE_1.LINEQ1 LEFT OUTER JOIN RTEBTCMTYH RTEBTCMTYH_1 ON " _
           &"RTEBTCUSTCHG.NCOMQ1 = RTEBTCMTYH_1.COMQ1 ON RTEBTCUSTCHGSNDWORK.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
           &"RTEBTCUSTCHGSNDWORK.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUSTCHGSNDWORK.CUSID = RTEBTCUSTCHG.CUSID AND " _
           &"RTEBTCUSTCHGSNDWORK.ENTRYNO = RTEBTCUSTCHG.ENTRYNO AND RTEBTCUSTCHGSNDWORK.DROPDAT IS NULL LEFT OUTER JOIN " _
           &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON RTEBTCUSTCHGSNDWORK.ASSIGNENGINEER = RTEmployee.EMPLY " _
           &"WHERE RTEBTCUSTCHG.COMQ1 = 0 "
  dataTable="RTEBTCUSTCHG"
  userDefineDelete="Yes"
  numberOfKey=4
  dataProg="RTEBTCUSTCHGD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=true
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="RTEBTCUSTCHGs2.asp"
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
  '----
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTEBTCUSTCHG.ComQ1<>0 and RTEBTCUSTCHG.FINISHCHKDAT is null  and RTEBTCUSTCHG.dropDAT is null "
     searchShow="未結案 "
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
  
    If searchShow="全部" Then
         sqlList="SELECT RTEBTCUSTCHG.COMQ1, RTEBTCUSTCHG.LINEQ1, RTEBTCUSTCHG.CUSID, RTEBTCUSTCHG.ENTRYNO, RTRIM(CONVERT(char(6), " _
           &"RTEBTCUSTCHG.COMQ1)) + '-' + RTRIM(CONVERT(char(6), RTEBTCUSTCHG.LINEQ1)) AS Expr1, RTEBTCMTYH.COMN, " _
           &"RTEBTCUST.AVSNO, RTEBTCUST.CUSNC, RTEBTCUSTCHG.APPLYDAT, " _
           &"CASE WHEN RTEBTCUSTCHG.CHGCOD1 = 1 THEN '用戶資料' ELSE '' END + '/' + " _
           &"CASE WHEN RTEBTCUSTCHG.CHGCOD2 = 1 THEN '移機' ELSE '' END + '/' + " _
           &"CASE WHEN RTEBTCUSTCHG.CHGCOD3 = 1 THEN '換號' ELSE '' END + '/' + " _
           &"CASE WHEN RTEBTCUSTCHG.CHGCOD4 = 1 THEN '其它' ELSE '' END, RTEBTCUSTCHG.DROPDAT, RTEBTCUSTCHG.TRANSCHKDAT, " _
           &"RTEBTCUSTCHG.TRANSDAT, RTEBTCUSTCHG.FINISHDAT, RTEBTCUSTCHG.FINISHCHKDAT, RTRIM(CONVERT(char(6), " _
           &"RTEBTCUSTCHG.NCOMQ1)) + '-' + RTRIM(CONVERT(char(6), RTEBTCUSTCHG.NLINEQ1)) AS Expr2, RTEBTCMTYH_1.COMN AS Expr3, " _
           &"RTEBTCUSTCHGSNDWORK.PRTNO, RTEBTCUSTCHGSNDWORK.SENDWORKDAT, case when RTObj.CUSNC is null then RTObj_1.SHORTNC else RTObj.CUSNC end, CASE when RTObj_2.CUSNC is null then  " _
           &"RTObj_3.SHORTNC else RTObj_2.CUSNC end " _
           &"FROM RTObj RTObj_3 RIGHT OUTER JOIN RTObj RTObj_1 INNER JOIN RTEBTCUSTCHGSNDWORK ON RTObj_1.CUSID = " _
           &"RTEBTCUSTCHGSNDWORK.ASSIGNCONSIGNEE ON RTObj_3.CUSID = RTEBTCUSTCHGSNDWORK.REALCONSIGNEE LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_1 INNER JOIN RTObj RTObj_2 ON RTEmployee_1.CUSID = RTObj_2.CUSID ON " _
           &"RTEBTCUSTCHGSNDWORK.REALENGINEER = RTEmployee_1.EMPLY RIGHT OUTER JOIN RTEBTCUSTCHG INNER JOIN RTEBTCMTYH ON " _
           &"RTEBTCUSTCHG.COMQ1 = RTEBTCMTYH.COMQ1 INNER JOIN RTEBTCMTYLINE ON RTEBTCUSTCHG.COMQ1 = RTEBTCMTYLINE.COMQ1 AND " _
           &"RTEBTCUSTCHG.LINEQ1 = RTEBTCMTYLINE.LINEQ1 INNER JOIN RTEBTCUST ON RTEBTCUSTCHG.COMQ1 = RTEBTCUST.COMQ1 AND " _
           &"RTEBTCUSTCHG.LINEQ1 = RTEBTCUST.LINEQ1 AND RTEBTCUSTCHG.CUSID = RTEBTCUST.CUSID LEFT OUTER JOIN " _
           &"RTEBTCMTYLINE RTEBTCMTYLINE_1 ON RTEBTCUSTCHG.NCOMQ1 = RTEBTCMTYLINE_1.COMQ1 AND " _
           &"RTEBTCUSTCHG.NLINEQ1 = RTEBTCMTYLINE_1.LINEQ1 LEFT OUTER JOIN RTEBTCMTYH RTEBTCMTYH_1 ON " _
           &"RTEBTCUSTCHG.NCOMQ1 = RTEBTCMTYH_1.COMQ1 ON RTEBTCUSTCHGSNDWORK.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
           &"RTEBTCUSTCHGSNDWORK.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUSTCHGSNDWORK.CUSID = RTEBTCUSTCHG.CUSID AND " _
           &"RTEBTCUSTCHGSNDWORK.ENTRYNO = RTEBTCUSTCHG.ENTRYNO AND RTEBTCUSTCHGSNDWORK.DROPDAT IS NULL LEFT OUTER JOIN " _
           &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON RTEBTCUSTCHGSNDWORK.ASSIGNENGINEER = RTEmployee.EMPLY " _
           &"WHERE RTEBTCUSTCHG.COMQ1 <> 0 AND " & searchqry
    Else
         sqlList="SELECT RTEBTCUSTCHG.COMQ1, RTEBTCUSTCHG.LINEQ1, RTEBTCUSTCHG.CUSID, RTEBTCUSTCHG.ENTRYNO, RTRIM(CONVERT(char(6), " _
           &"RTEBTCUSTCHG.COMQ1)) + '-' + RTRIM(CONVERT(char(6), RTEBTCUSTCHG.LINEQ1)) AS Expr1, RTEBTCMTYH.COMN, " _
           &"RTEBTCUST.AVSNO, RTEBTCUST.CUSNC, RTEBTCUSTCHG.APPLYDAT, " _
           &"CASE WHEN RTEBTCUSTCHG.CHGCOD1 = 1 THEN '用戶資料' ELSE '' END + '/' + " _
           &"CASE WHEN RTEBTCUSTCHG.CHGCOD2 = 1 THEN '移機' ELSE '' END + '/' + " _
           &"CASE WHEN RTEBTCUSTCHG.CHGCOD3 = 1 THEN '換號' ELSE '' END + '/' + " _
           &"CASE WHEN RTEBTCUSTCHG.CHGCOD4 = 1 THEN '其它' ELSE '' END, RTEBTCUSTCHG.DROPDAT, RTEBTCUSTCHG.TRANSCHKDAT, " _
           &"RTEBTCUSTCHG.TRANSDAT, RTEBTCUSTCHG.FINISHDAT, RTEBTCUSTCHG.FINISHCHKDAT, RTRIM(CONVERT(char(6), " _
           &"RTEBTCUSTCHG.NCOMQ1)) + '-' + RTRIM(CONVERT(char(6), RTEBTCUSTCHG.NLINEQ1)) AS Expr2, RTEBTCMTYH_1.COMN AS Expr3, " _
           &"RTEBTCUSTCHGSNDWORK.PRTNO, RTEBTCUSTCHGSNDWORK.SENDWORKDAT, case when RTObj.CUSNC is null then RTObj_1.SHORTNC else RTObj.CUSNC end, CASE when RTObj_2.CUSNC is null then  " _
           &"RTObj_3.SHORTNC else RTObj_2.CUSNC end " _
           &"FROM RTObj RTObj_3 RIGHT OUTER JOIN RTObj RTObj_1 INNER JOIN RTEBTCUSTCHGSNDWORK ON RTObj_1.CUSID = " _
           &"RTEBTCUSTCHGSNDWORK.ASSIGNCONSIGNEE ON RTObj_3.CUSID = RTEBTCUSTCHGSNDWORK.REALCONSIGNEE LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_1 INNER JOIN RTObj RTObj_2 ON RTEmployee_1.CUSID = RTObj_2.CUSID ON " _
           &"RTEBTCUSTCHGSNDWORK.REALENGINEER = RTEmployee_1.EMPLY RIGHT OUTER JOIN RTEBTCUSTCHG INNER JOIN RTEBTCMTYH ON " _
           &"RTEBTCUSTCHG.COMQ1 = RTEBTCMTYH.COMQ1 INNER JOIN RTEBTCMTYLINE ON RTEBTCUSTCHG.COMQ1 = RTEBTCMTYLINE.COMQ1 AND " _
           &"RTEBTCUSTCHG.LINEQ1 = RTEBTCMTYLINE.LINEQ1 INNER JOIN RTEBTCUST ON RTEBTCUSTCHG.COMQ1 = RTEBTCUST.COMQ1 AND " _
           &"RTEBTCUSTCHG.LINEQ1 = RTEBTCUST.LINEQ1 AND RTEBTCUSTCHG.CUSID = RTEBTCUST.CUSID LEFT OUTER JOIN " _
           &"RTEBTCMTYLINE RTEBTCMTYLINE_1 ON RTEBTCUSTCHG.NCOMQ1 = RTEBTCMTYLINE_1.COMQ1 AND " _
           &"RTEBTCUSTCHG.NLINEQ1 = RTEBTCMTYLINE_1.LINEQ1 LEFT OUTER JOIN RTEBTCMTYH RTEBTCMTYH_1 ON " _
           &"RTEBTCUSTCHG.NCOMQ1 = RTEBTCMTYH_1.COMQ1 ON RTEBTCUSTCHGSNDWORK.COMQ1 = RTEBTCUSTCHG.COMQ1 AND " _
           &"RTEBTCUSTCHGSNDWORK.LINEQ1 = RTEBTCUSTCHG.LINEQ1 AND RTEBTCUSTCHGSNDWORK.CUSID = RTEBTCUSTCHG.CUSID AND " _
           &"RTEBTCUSTCHGSNDWORK.ENTRYNO = RTEBTCUSTCHG.ENTRYNO AND RTEBTCUSTCHGSNDWORK.DROPDAT IS NULL LEFT OUTER JOIN " _
           &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON RTEBTCUSTCHGSNDWORK.ASSIGNENGINEER = RTEmployee.EMPLY " _
           &"WHERE RTEBTCUSTCHG.COMQ1 <> 0 AND " & searchqry
    End If  
  'end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>