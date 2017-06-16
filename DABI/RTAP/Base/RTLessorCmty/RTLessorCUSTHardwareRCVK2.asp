<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="ET-City管理系統"
  title="ET-City物品領用單資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="領用結案;結案返轉;領用明細;列印領用單;異動記錄"
  functionOptProgram="RTLessorCustHardwareRCVF.ASP;RTLessorCustHardwareRCVFR.ASP;RTLessorCustHardwareRCVDTLK.ASP;RTLessorCustHardwareRCVP.ASP;RTLessorCustHardwareRCVlogk.ASP"
  functionOptPrompt="Y;Y;N;Y;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="領用單號;主線;社區名;用戶名;領用類別;領用申請日;領用申請人;實際領用人;領用結案日;結案人員;作廢日;作廢人員;派工單號;轉領用單人員;領用數量"
  sqlDelete="SELECT  RTLessorCustRCVHardware.RCVPRTNO AS RCVPRTNO, " _
                       &" CASE WHEN RTLessorCustRCVHardware.DATASRC ='01' THEN '裝機派工領用'  WHEN  RTLessorCustRCVHardware.DATASRC ='02'  THEN '續約收款派工領用' WHEN RTLessorCustRCVHardware.DATASRC = '03' THEN '復機收款派工領用' ELSE '' END, " _
                       &" RTLessorCustRCVHardware.APPLYDAT AS APPLYDAT, " _
                       &" CASE WHEN RTObj_7.CUSNC='' OR RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE  RTObj_7.CUSNC END , " _
                       &" CASE WHEN RTObj_1.CUSNC='' OR RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                       &" RTLessorCustRCVHardware.CLOSEDAT AS CLOSEDAT, RTObj_4.CUSNC AS CUSNC5, RTLessorCustRCVHardware.CANCELDAT AS CANCELDAT, RTObj_5.CUSNC AS CUSNC6, " _
                       &" RTLessorCustRCVHardware.PRTNO AS PRTNO, " _
                       &" RTObj_6.CUSNC AS cusnc7,SUM(RTLessorCustRCVHardwareDTL.QTY) " _
&"  FROM             RTEmployee RTEmployee_4 INNER JOIN RTObj RTObj_6 ON RTEmployee_4.CUSID = RTObj_6.CUSID RIGHT OUTER JOIN " _
                       &" RTLessorCustRCVHardware ON RTEmployee_4.EMPLY = RTLessorCustRCVHardware.RCVUSR LEFT OUTER JOIN " _
                       &" RTEmployee RTEmployee_3 INNER JOIN RTObj RTObj_5 ON RTEmployee_3.CUSID = RTObj_5.CUSID ON " _
                       &" RTLessorCustRCVHardware.CANCELUSR = RTEmployee_3.EMPLY LEFT OUTER JOIN " _
                       &" RTEmployee RTEmployee_2 INNER JOIN RTObj RTObj_4 ON RTEmployee_2.CUSID = RTObj_4.CUSID ON " _
                       &" RTLessorCustRCVHardware.CLOSEUSR = RTEmployee_2.EMPLY LEFT OUTER JOIN RTObj RTObj_3 ON " _
                       &" RTLessorCustRCVHardware.REALCONSIGNEE = RTObj_3.CUSID LEFT OUTER JOIN RTObj RTObj_2 ON " _
                       &" RTLessorCustRCVHardware.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN " _
                       &" RTEmployee RTEmployee_1 INNER JOIN RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON " _
                       &" RTLessorCustRCVHardware.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
                       &" RTObj RTObj_7 INNER JOIN RTEmployee RTEmployee_5 ON RTObj_7.CUSID = RTEmployee_5.CUSID ON " _
                       &" RTLessorCustRCVHardware.ASSIGNENGINEER = RTEmployee_5.EMPLY LEFT OUTER JOIN RTLessorCustRCVHardwareDTL ON " _
                       &" RTLessorCustRCVHardware.RCVPRTNO=RTLessorCustRCVHardwareDTL.RCVPRTNO " _
                       &" where " & searchqry & " " _
                       &" GROUP BY  RTLessorCustRCVHardware.RCVPRTNO , " _
                       &" CASE WHEN RTLessorCustRCVHardware.DATASRC ='01' THEN '裝機派工領用'  WHEN  RTLessorCustRCVHardware.DATASRC ='02'  THEN '續約收款派工領用' WHEN RTLessorCustRCVHardware.DATASRC = '03' THEN '復機收款派工領用' ELSE '' END, " _
                       &" RTLessorCustRCVHardware.APPLYDAT , " _
                       &" CASE WHEN RTObj_7.CUSNC='' OR RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE  RTObj_7.CUSNC END , " _
                       &" CASE WHEN RTObj_1.CUSNC='' OR RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                       &" RTLessorCustRCVHardware.CLOSEDAT, RTObj_4.CUSNC, RTObj_5.CUSNC, RTLessorCustRCVHardware.PRTNO, " _
                       &" RTLessorCustRCVHardware.CANCELDAT, RTObj_6.CUSNC " _
                       &" where RTLessorCustRCVHardware.rcvprtno='' "
  dataTable="RTLessorCustRCVHardware"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="RTLessorCustHardwareRCVD.ASP"
  datawindowFeature=""
  searchWindowFeature="width=350,height=160,scrollbars=yes"
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
  searchProg="RTLessorCustHardwareRCVS2.ASP"
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
     searchQry=" RTLessorCustrcvhardware.prtno<>'' and RTLessorCustrcvhardware.canceldat is null and RTLessorCustrcvhardware.closedat is null "
     searchShow="尚未領用結案"
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
  
          sqlList="SELECT RTLessorCustRCVHardware.RCVPRTNO AS RCVPRTNO,  RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.COMQ1))) " _
                 &"+ '-' + RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.LINEQ1))) AS comqline, RTLESSORCMTYH.COMN, " _
                 &"RTLESSORCUST.CUSNC,RTCODE_1.CODENC,RTLessorCustRCVHardware.APPLYDAT AS APPLYDAT, " _ 
                 &"CASE WHEN RTObj_7.CUSNC = '' OR RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                 &"CASE WHEN RTObj_1.CUSNC = '' OR RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                 &"RTLessorCustRCVHardware.CLOSEDAT AS CLOSEDAT, RTObj_4.CUSNC AS CUSNC5,RTLessorCustRCVHardware.CANCELDAT AS CANCELDAT, " _
                 &"RTObj_5.CUSNC AS CUSNC6, RTLessorCustRCVHardware.PRTNO AS PRTNO, RTObj_6.CUSNC AS cusnc7, " _
                 &"SUM(RTLessorCustRCVHardwareDTL.QTY) " _
                 &"FROM   RTEmployee RTEmployee_4 INNER JOIN RTObj RTObj_6 ON RTEmployee_4.CUSID = RTObj_6.CUSID " _
                 &"RIGHT OUTER JOIN RTLessorCustRCVHardware ON RTEmployee_4.EMPLY = RTLessorCustRCVHardware.RCVUSR " _
                 &"LEFT OUTER JOIN RTEmployee RTEmployee_3 INNER JOIN RTObj RTObj_5 ON RTEmployee_3.CUSID = RTObj_5.CUSID " _
                 &"ON RTLessorCustRCVHardware.CANCELUSR = RTEmployee_3.EMPLY LEFT OUTER JOIN RTEmployee RTEmployee_2 " _
                 &"INNER JOIN RTObj RTObj_4 ON RTEmployee_2.CUSID = RTObj_4.CUSID ON " _
                 &"RTLessorCustRCVHardware.CLOSEUSR = RTEmployee_2.EMPLY LEFT OUTER JOIN RTObj RTObj_3 ON " _
                 &"RTLessorCustRCVHardware.REALCONSIGNEE = RTObj_3.CUSID LEFT OUTER JOIN RTObj RTObj_2 ON " _
                 &"RTLessorCustRCVHardware.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 " _
                 &"INNER JOIN RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON " _
                 &"RTLessorCustRCVHardware.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN RTObj RTObj_7 INNER JOIN " _
                 &"RTEmployee RTEmployee_5 ON RTObj_7.CUSID = RTEmployee_5.CUSID ON " _
                 &"RTLessorCustRCVHardware.ASSIGNENGINEER = RTEmployee_5.EMPLY LEFT OUTER JOIN RTLessorCustRCVHardwareDTL ON " _
                 &"RTLessorCustRCVHardware.RCVPRTNO = RTLessorCustRCVHardwareDTL.RCVPRTNO LEFT OUTER JOIN " _
                 &"RTLESSORCUSTHARDWARE ON RTLessorCustRCVHardware.rcvprtno = RTLESSORCUSTHARDWARE.rcvprtno LEFT OUTER JOIN " _
                 &"RTLESSORCUST ON RTLESSORCUSTHARDWARE.CUSID = RTLESSORCUST.CUSID LEFT OUTER JOIN RTLESSORCMTYH ON " _
                 &"RTLESSORCUST.COMQ1 = RTLESSORCMTYH.COMQ1 LEFT OUTER JOIN RTCODE RTCODE_1 ON RTLessorCustRCVHardware.DATASRC=RTCODE_1.CODE AND RTCODE_1.KIND='N8' " _
                 &"WHERE  RTLessorCustrcvhardware.prtno <> '' AND RTLessorCustrcvhardware.datasrc = '01' AND " & searchqry & " " _
                 &"GROUP BY  RTLessorCustRCVHardware.RCVPRTNO, RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.COMQ1))) + '-' + RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.LINEQ1))), " _
                 &"RTLESSORCMTYH.COMN, RTLESSORCUST.CUSNC,RTCODE_1.CODENC,RTLessorCustRCVHardware.APPLYDAT, " _
                 &"CASE WHEN RTObj_7.CUSNC = '' OR RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                 &"CASE WHEN RTObj_1.CUSNC = '' OR RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                 &"RTLessorCustRCVHardware.CLOSEDAT, RTObj_4.CUSNC, RTObj_5.CUSNC, RTLessorCustRCVHardware.PRTNO, " _
                 &"RTLessorCustRCVHardware.CANCELDAT, RTObj_6.CUSNC " _
                 &"UNION " _
                 &"SELECT RTLessorCustRCVHardware.RCVPRTNO AS RCVPRTNO,  RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.COMQ1))) " _
                 &"+ '-' + RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.LINEQ1))) AS comqline, RTLESSORCMTYH.COMN, " _
                 &"RTLESSORCUST.CUSNC,RTCODE_1.CODENC,RTLessorCustRCVHardware.APPLYDAT AS APPLYDAT, " _
                 &"CASE WHEN RTObj_7.CUSNC = '' OR RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                 &"CASE WHEN RTObj_1.CUSNC = '' OR RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                 &"RTLessorCustRCVHardware.CLOSEDAT AS CLOSEDAT, RTObj_4.CUSNC AS CUSNC5,RTLessorCustRCVHardware.CANCELDAT AS CANCELDAT, " _
                 &"RTObj_5.CUSNC AS CUSNC6, RTLessorCustRCVHardware.PRTNO AS PRTNO, RTObj_6.CUSNC AS cusnc7, " _
                 &"SUM(RTLessorCustRCVHardwareDTL.QTY) " _
                 &"FROM   RTEmployee RTEmployee_4 INNER JOIN RTObj RTObj_6 ON RTEmployee_4.CUSID = RTObj_6.CUSID " _
                 &"RIGHT OUTER JOIN RTLessorCustRCVHardware ON RTEmployee_4.EMPLY = RTLessorCustRCVHardware.RCVUSR " _
                 &"LEFT OUTER JOIN RTEmployee RTEmployee_3 INNER JOIN RTObj RTObj_5 ON RTEmployee_3.CUSID = RTObj_5.CUSID " _
                 &"ON RTLessorCustRCVHardware.CANCELUSR = RTEmployee_3.EMPLY LEFT OUTER JOIN RTEmployee RTEmployee_2 " _
                 &"INNER JOIN RTObj RTObj_4 ON RTEmployee_2.CUSID = RTObj_4.CUSID ON " _
                 &"RTLessorCustRCVHardware.CLOSEUSR = RTEmployee_2.EMPLY LEFT OUTER JOIN RTObj RTObj_3 ON " _
                 &"RTLessorCustRCVHardware.REALCONSIGNEE = RTObj_3.CUSID LEFT OUTER JOIN RTObj RTObj_2 ON " _
                 &"RTLessorCustRCVHardware.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 " _
                 &"INNER JOIN RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON " _
                 &"RTLessorCustRCVHardware.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN RTObj RTObj_7 INNER JOIN " _
                 &"RTEmployee RTEmployee_5 ON RTObj_7.CUSID = RTEmployee_5.CUSID ON " _
                 &"RTLessorCustRCVHardware.ASSIGNENGINEER = RTEmployee_5.EMPLY LEFT OUTER JOIN RTLessorCustRCVHardwareDTL ON " _
                 &"RTLessorCustRCVHardware.RCVPRTNO = RTLessorCustRCVHardwareDTL.RCVPRTNO LEFT OUTER JOIN " _
                 &"RTLESSORCUSTcontHARDWARE ON RTLessorCustRCVHardware.rcvprtno = RTLESSORCUSTcontHARDWARE.rcvprtno LEFT OUTER JOIN " _
                 &"RTLESSORCUST ON RTLESSORCUSTcontHARDWARE.CUSID = RTLESSORCUST.CUSID LEFT OUTER JOIN RTLESSORCMTYH ON " _
                 &"RTLESSORCUST.COMQ1 = RTLESSORCMTYH.COMQ1 LEFT OUTER JOIN RTCODE RTCODE_1 ON RTLessorCustRCVHardware.DATASRC=RTCODE_1.CODE AND RTCODE_1.KIND='N8' " _
                 &"WHERE  RTLessorCustrcvhardware.prtno <> '' AND RTLessorCustrcvhardware.datasrc = '02' AND " & searchqry & " " _
                 &"GROUP BY  RTLessorCustRCVHardware.RCVPRTNO, RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.COMQ1))) + '-' + RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.LINEQ1))), " _
                 &"RTLESSORCMTYH.COMN, RTLESSORCUST.CUSNC, RTCODE_1.CODENC,RTLessorCustRCVHardware.APPLYDAT, " _
                 &"CASE WHEN RTObj_7.CUSNC = '' OR RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                 &"CASE WHEN RTObj_1.CUSNC = '' OR RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                 &"RTLessorCustRCVHardware.CLOSEDAT, RTObj_4.CUSNC, RTObj_5.CUSNC, RTLessorCustRCVHardware.PRTNO, " _
                 &"RTLessorCustRCVHardware.CANCELDAT, RTObj_6.CUSNC " _
                 &"UNION " _
                 &"SELECT RTLessorCustRCVHardware.RCVPRTNO AS RCVPRTNO,  RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.COMQ1))) " _
                 &"+ '-' + RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.LINEQ1))) AS comqline, RTLESSORCMTYH.COMN, " _
                 &"RTLESSORCUST.CUSNC,RTCODE_1.CODENC,RTLessorCustRCVHardware.APPLYDAT AS APPLYDAT, " _
                 &"CASE WHEN RTObj_7.CUSNC = '' OR RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                 &"CASE WHEN RTObj_1.CUSNC = '' OR RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                 &"RTLessorCustRCVHardware.CLOSEDAT AS CLOSEDAT, RTObj_4.CUSNC AS CUSNC5,RTLessorCustRCVHardware.CANCELDAT AS CANCELDAT, " _
                 &"RTObj_5.CUSNC AS CUSNC6, RTLessorCustRCVHardware.PRTNO AS PRTNO, RTObj_6.CUSNC AS cusnc7, " _
                 &"SUM(RTLessorCustRCVHardwareDTL.QTY) " _
                 &"FROM   RTEmployee RTEmployee_4 INNER JOIN RTObj RTObj_6 ON RTEmployee_4.CUSID = RTObj_6.CUSID " _
                 &"RIGHT OUTER JOIN RTLessorCustRCVHardware ON RTEmployee_4.EMPLY = RTLessorCustRCVHardware.RCVUSR " _
                 &"LEFT OUTER JOIN RTEmployee RTEmployee_3 INNER JOIN RTObj RTObj_5 ON RTEmployee_3.CUSID = RTObj_5.CUSID " _
                 &"ON RTLessorCustRCVHardware.CANCELUSR = RTEmployee_3.EMPLY LEFT OUTER JOIN RTEmployee RTEmployee_2 " _
                 &"INNER JOIN RTObj RTObj_4 ON RTEmployee_2.CUSID = RTObj_4.CUSID ON " _
                 &"RTLessorCustRCVHardware.CLOSEUSR = RTEmployee_2.EMPLY LEFT OUTER JOIN RTObj RTObj_3 ON " _
                 &"RTLessorCustRCVHardware.REALCONSIGNEE = RTObj_3.CUSID LEFT OUTER JOIN RTObj RTObj_2 ON " _
                 &"RTLessorCustRCVHardware.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 " _
                 &"INNER JOIN RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON " _
                 &"RTLessorCustRCVHardware.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN RTObj RTObj_7 INNER JOIN " _
                 &"RTEmployee RTEmployee_5 ON RTObj_7.CUSID = RTEmployee_5.CUSID ON " _
                 &"RTLessorCustRCVHardware.ASSIGNENGINEER = RTEmployee_5.EMPLY LEFT OUTER JOIN RTLessorCustRCVHardwareDTL ON " _
                 &"RTLessorCustRCVHardware.RCVPRTNO = RTLessorCustRCVHardwareDTL.RCVPRTNO LEFT OUTER JOIN " _
                 &"RTLESSORCUSTRETURNHARDWARE ON RTLessorCustRCVHardware.rcvprtno = RTLESSORCUSTRETURNHARDWARE.rcvprtno LEFT OUTER JOIN " _
                 &"RTLESSORCUST ON RTLESSORCUSTRETURNHARDWARE.CUSID = RTLESSORCUST.CUSID LEFT OUTER JOIN RTLESSORCMTYH ON " _
                 &"RTLESSORCUST.COMQ1 = RTLESSORCMTYH.COMQ1 LEFT OUTER JOIN RTCODE RTCODE_1 ON RTLessorCustRCVHardware.DATASRC=RTCODE_1.CODE AND RTCODE_1.KIND='N8' " _
                 &"WHERE  RTLessorCustrcvhardware.prtno <> '' AND RTLessorCustrcvhardware.datasrc = '03' AND " & searchqry & " " _
                 &"GROUP BY  RTLessorCustRCVHardware.RCVPRTNO, RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.COMQ1))) + '-' + RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.LINEQ1))), " _
                 &"RTLESSORCMTYH.COMN, RTLESSORCUST.CUSNC,RTCODE_1.CODENC,RTLessorCustRCVHardware.APPLYDAT, " _
                 &"CASE WHEN RTObj_7.CUSNC = '' OR RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                 &"CASE WHEN RTObj_1.CUSNC = '' OR RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                 &"RTLessorCustRCVHardware.CLOSEDAT, RTObj_4.CUSNC, RTObj_5.CUSNC, RTLessorCustRCVHardware.PRTNO, " _
                 &"RTLessorCustRCVHardware.CANCELDAT, RTObj_6.CUSNC " _       
                 &"UNION " _
                 &"SELECT RTLessorCustRCVHardware.RCVPRTNO AS RCVPRTNO,  RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.COMQ1))) " _
                 &"+ '-' + RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.LINEQ1))) AS comqline, RTLESSORCMTYH.COMN, " _
                 &"RTLESSORCUST.CUSNC,RTCODE_1.CODENC,RTLessorCustRCVHardware.APPLYDAT AS APPLYDAT, " _
                 &"CASE WHEN RTObj_7.CUSNC = '' OR RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                 &"CASE WHEN RTObj_1.CUSNC = '' OR RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                 &"RTLessorCustRCVHardware.CLOSEDAT AS CLOSEDAT, RTObj_4.CUSNC AS CUSNC5,RTLessorCustRCVHardware.CANCELDAT AS CANCELDAT, " _
                 &"RTObj_5.CUSNC AS CUSNC6, RTLessorCustRCVHardware.PRTNO AS PRTNO, RTObj_6.CUSNC AS cusnc7, " _
                 &"SUM(RTLessorCustRCVHardwareDTL.QTY) " _
                 &"FROM   RTEmployee RTEmployee_4 INNER JOIN RTObj RTObj_6 ON RTEmployee_4.CUSID = RTObj_6.CUSID " _
                 &"RIGHT OUTER JOIN RTLessorCustRCVHardware ON RTEmployee_4.EMPLY = RTLessorCustRCVHardware.RCVUSR " _
                 &"LEFT OUTER JOIN RTEmployee RTEmployee_3 INNER JOIN RTObj RTObj_5 ON RTEmployee_3.CUSID = RTObj_5.CUSID " _
                 &"ON RTLessorCustRCVHardware.CANCELUSR = RTEmployee_3.EMPLY LEFT OUTER JOIN RTEmployee RTEmployee_2 " _
                 &"INNER JOIN RTObj RTObj_4 ON RTEmployee_2.CUSID = RTObj_4.CUSID ON " _
                 &"RTLessorCustRCVHardware.CLOSEUSR = RTEmployee_2.EMPLY LEFT OUTER JOIN RTObj RTObj_3 ON " _
                 &"RTLessorCustRCVHardware.REALCONSIGNEE = RTObj_3.CUSID LEFT OUTER JOIN RTObj RTObj_2 ON " _
                 &"RTLessorCustRCVHardware.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 " _
                 &"INNER JOIN RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON " _
                 &"RTLessorCustRCVHardware.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN RTObj RTObj_7 INNER JOIN " _
                 &"RTEmployee RTEmployee_5 ON RTObj_7.CUSID = RTEmployee_5.CUSID ON " _
                 &"RTLessorCustRCVHardware.ASSIGNENGINEER = RTEmployee_5.EMPLY LEFT OUTER JOIN RTLessorCustRCVHardwareDTL ON " _
                 &"RTLessorCustRCVHardware.RCVPRTNO = RTLessorCustRCVHardwareDTL.RCVPRTNO LEFT OUTER JOIN " _
                 &"RTLESSORCUSTfaqHARDWARE ON RTLessorCustRCVHardware.rcvprtno = RTLESSORCUSTfaqHARDWARE.rcvprtno LEFT OUTER JOIN " _
                 &"RTLESSORCUST ON RTLESSORCUSTfaqHARDWARE.CUSID = RTLESSORCUST.CUSID LEFT OUTER JOIN RTLESSORCMTYH ON " _
                 &"RTLESSORCUST.COMQ1 = RTLESSORCMTYH.COMQ1 LEFT OUTER JOIN RTCODE RTCODE_1 ON RTLessorCustRCVHardware.DATASRC=RTCODE_1.CODE AND RTCODE_1.KIND='N8' " _
                 &"WHERE  RTLessorCustrcvhardware.prtno <> '' AND RTLessorCustrcvhardware.datasrc = '04' AND " & searchqry & " " _
                 &"GROUP BY  RTLessorCustRCVHardware.RCVPRTNO, RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.COMQ1))) + '-' + RTRIM(LTRIM(CONVERT(char(6), RTLessorCust.LINEQ1))), " _
                 &"RTLESSORCMTYH.COMN, RTLESSORCUST.CUSNC, RTCODE_1.CODENC,RTLessorCustRCVHardware.APPLYDAT, " _
                 &"CASE WHEN RTObj_7.CUSNC = '' OR RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                 &"CASE WHEN RTObj_1.CUSNC = '' OR RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                 &"RTLessorCustRCVHardware.CLOSEDAT, RTObj_4.CUSNC, RTObj_5.CUSNC, RTLessorCustRCVHardware.PRTNO, " _
                 &"RTLessorCustRCVHardware.CANCELDAT, RTObj_6.CUSNC " _
                 &"UNION " _
                 &"SELECT RTLessorCustRCVHardware.RCVPRTNO AS RCVPRTNO,  RTRIM(LTRIM(CONVERT(char(6), RTLESSORCmtylineHARDWARE.COMQ1))) " _
                 &"+ '-' + RTRIM(LTRIM(CONVERT(char(6), RTLESSORCmtylineHARDWARE.LINEQ1))) AS comqline, RTLESSORCMTYH.COMN, " _
                 &"'',RTCODE_1.CODENC,RTLessorCustRCVHardware.APPLYDAT AS APPLYDAT, " _
                 &"CASE WHEN RTObj_7.CUSNC = '' OR RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                 &"CASE WHEN RTObj_1.CUSNC = '' OR RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                 &"RTLessorCustRCVHardware.CLOSEDAT AS CLOSEDAT, RTObj_4.CUSNC AS CUSNC5,RTLessorCustRCVHardware.CANCELDAT AS CANCELDAT, " _
                 &"RTObj_5.CUSNC AS CUSNC6, RTLessorCustRCVHardware.PRTNO AS PRTNO, RTObj_6.CUSNC AS cusnc7, " _
                 &"SUM(RTLessorCustRCVHardwareDTL.QTY) " _
                 &"FROM   RTEmployee RTEmployee_4 INNER JOIN RTObj RTObj_6 ON RTEmployee_4.CUSID = RTObj_6.CUSID " _
                 &"RIGHT OUTER JOIN RTLessorCustRCVHardware ON RTEmployee_4.EMPLY = RTLessorCustRCVHardware.RCVUSR " _
                 &"LEFT OUTER JOIN RTEmployee RTEmployee_3 INNER JOIN RTObj RTObj_5 ON RTEmployee_3.CUSID = RTObj_5.CUSID " _
                 &"ON RTLessorCustRCVHardware.CANCELUSR = RTEmployee_3.EMPLY LEFT OUTER JOIN RTEmployee RTEmployee_2 " _
                 &"INNER JOIN RTObj RTObj_4 ON RTEmployee_2.CUSID = RTObj_4.CUSID ON " _
                 &"RTLessorCustRCVHardware.CLOSEUSR = RTEmployee_2.EMPLY LEFT OUTER JOIN RTObj RTObj_3 ON " _
                 &"RTLessorCustRCVHardware.REALCONSIGNEE = RTObj_3.CUSID LEFT OUTER JOIN RTObj RTObj_2 ON " _
                 &"RTLessorCustRCVHardware.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 " _
                 &"INNER JOIN RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON " _
                 &"RTLessorCustRCVHardware.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN RTObj RTObj_7 INNER JOIN " _
                 &"RTEmployee RTEmployee_5 ON RTObj_7.CUSID = RTEmployee_5.CUSID ON " _
                 &"RTLessorCustRCVHardware.ASSIGNENGINEER = RTEmployee_5.EMPLY LEFT OUTER JOIN RTLessorCustRCVHardwareDTL ON " _
                 &"RTLessorCustRCVHardware.RCVPRTNO = RTLessorCustRCVHardwareDTL.RCVPRTNO LEFT OUTER JOIN " _
                 &"RTLESSORCmtylineHARDWARE ON RTLessorCustRCVHardware.rcvprtno = RTLESSORCmtylineHARDWARE.rcvprtno LEFT OUTER JOIN RTLESSORCMTYH ON " _
                 &"RTLESSORCmtylineHARDWARE.COMQ1 = RTLESSORCMTYH.COMQ1 LEFT OUTER JOIN RTCODE RTCODE_1 ON RTLessorCustRCVHardware.DATASRC=RTCODE_1.CODE AND RTCODE_1.KIND='N8' " _
                 &"WHERE  RTLessorCustrcvhardware.prtno <> '' AND RTLessorCustrcvhardware.datasrc = '05' AND " & searchqry & " " _
                 &"GROUP BY  RTLessorCustRCVHardware.RCVPRTNO, RTRIM(LTRIM(CONVERT(char(6), RTLESSORCmtylineHARDWARE.COMQ1))) + '-' + RTRIM(LTRIM(CONVERT(char(6), RTLESSORCmtylineHARDWARE.LINEQ1))), " _
                 &"RTLESSORCMTYH.COMN,RTCODE_1.CODENC,RTLessorCustRCVHardware.APPLYDAT, " _
                 &"CASE WHEN RTObj_7.CUSNC = '' OR RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                 &"CASE WHEN RTObj_1.CUSNC = '' OR RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                 &"RTLessorCustRCVHardware.CLOSEDAT, RTObj_4.CUSNC, RTObj_5.CUSNC, RTLessorCustRCVHardware.PRTNO, " _
                 &"RTLessorCustRCVHardware.CANCELDAT, RTObj_6.CUSNC " _
                 &"UNION " _
                 &"SELECT RTLessorCustRCVHardware.RCVPRTNO AS RCVPRTNO,  RTRIM(LTRIM(CONVERT(char(6), RTLESSORCmtylinefaqHARDWARE.COMQ1))) " _
                 &"+ '-' + RTRIM(LTRIM(CONVERT(char(6), RTLESSORCmtylinefaqHARDWARE.LINEQ1))) AS comqline, RTLESSORCMTYH.COMN, " _
                 &"'',RTCODE_1.CODENC,RTLessorCustRCVHardware.APPLYDAT AS APPLYDAT, " _
                 &"CASE WHEN RTObj_7.CUSNC = '' OR RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                 &"CASE WHEN RTObj_1.CUSNC = '' OR RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                 &"RTLessorCustRCVHardware.CLOSEDAT AS CLOSEDAT, RTObj_4.CUSNC AS CUSNC5,RTLessorCustRCVHardware.CANCELDAT AS CANCELDAT, " _
                 &"RTObj_5.CUSNC AS CUSNC6, RTLessorCustRCVHardware.PRTNO AS PRTNO, RTObj_6.CUSNC AS cusnc7, " _
                 &"SUM(RTLessorCustRCVHardwareDTL.QTY) " _
                 &"FROM   RTEmployee RTEmployee_4 INNER JOIN RTObj RTObj_6 ON RTEmployee_4.CUSID = RTObj_6.CUSID " _
                 &"RIGHT OUTER JOIN RTLessorCustRCVHardware ON RTEmployee_4.EMPLY = RTLessorCustRCVHardware.RCVUSR " _
                 &"LEFT OUTER JOIN RTEmployee RTEmployee_3 INNER JOIN RTObj RTObj_5 ON RTEmployee_3.CUSID = RTObj_5.CUSID " _
                 &"ON RTLessorCustRCVHardware.CANCELUSR = RTEmployee_3.EMPLY LEFT OUTER JOIN RTEmployee RTEmployee_2 " _
                 &"INNER JOIN RTObj RTObj_4 ON RTEmployee_2.CUSID = RTObj_4.CUSID ON " _
                 &"RTLessorCustRCVHardware.CLOSEUSR = RTEmployee_2.EMPLY LEFT OUTER JOIN RTObj RTObj_3 ON " _
                 &"RTLessorCustRCVHardware.REALCONSIGNEE = RTObj_3.CUSID LEFT OUTER JOIN RTObj RTObj_2 ON " _
                 &"RTLessorCustRCVHardware.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 " _
                 &"INNER JOIN RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON " _
                 &"RTLessorCustRCVHardware.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN RTObj RTObj_7 INNER JOIN " _
                 &"RTEmployee RTEmployee_5 ON RTObj_7.CUSID = RTEmployee_5.CUSID ON " _
                 &"RTLessorCustRCVHardware.ASSIGNENGINEER = RTEmployee_5.EMPLY LEFT OUTER JOIN RTLessorCustRCVHardwareDTL ON " _
                 &"RTLessorCustRCVHardware.RCVPRTNO = RTLessorCustRCVHardwareDTL.RCVPRTNO LEFT OUTER JOIN " _
                 &"RTLESSORCmtylinefaqHARDWARE ON RTLessorCustRCVHardware.rcvprtno = RTLESSORCmtylinefaqHARDWARE.rcvprtno LEFT OUTER JOIN RTLESSORCMTYH ON " _
                 &"RTLESSORCmtylinefaqHARDWARE.COMQ1 = RTLESSORCMTYH.COMQ1  LEFT OUTER JOIN RTCODE RTCODE_1 ON RTLessorCustRCVHardware.DATASRC=RTCODE_1.CODE AND RTCODE_1.KIND='N8' " _
                 &"WHERE  RTLessorCustrcvhardware.prtno <> '' AND RTLessorCustrcvhardware.datasrc = '06' AND " & searchqry & " " _
                 &"GROUP BY  RTLessorCustRCVHardware.RCVPRTNO, RTRIM(LTRIM(CONVERT(char(6), RTLESSORCmtylinefaqHARDWARE.COMQ1))) + '-' + RTRIM(LTRIM(CONVERT(char(6), RTLESSORCmtylinefaqHARDWARE.LINEQ1))), " _
                 &"RTLESSORCMTYH.COMN, RTCODE_1.CODENC,RTLessorCustRCVHardware.APPLYDAT, " _
                 &"CASE WHEN RTObj_7.CUSNC = '' OR RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                 &"CASE WHEN RTObj_1.CUSNC = '' OR RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                 &"RTLessorCustRCVHardware.CLOSEDAT, RTObj_4.CUSNC, RTObj_5.CUSNC, RTLessorCustRCVHardware.PRTNO, " _
                 &"RTLessorCustRCVHardware.CANCELDAT, RTObj_6.CUSNC "                                                                                                

   'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>