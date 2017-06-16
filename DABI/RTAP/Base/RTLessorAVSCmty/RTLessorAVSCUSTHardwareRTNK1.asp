<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="AVS-City管理系統"
  title="AVS-City物品移轉單資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="移轉結案;結案返轉;移轉明細;列印移轉單;異動記錄"
  functionOptProgram="RTLessorAVSCustHardwareRTNF.ASP;RTLessorAVSCustHardwareRTNFR.ASP;RTLessorAVSCustHardwareRTNDTLK.ASP;RTLessorAVSCustHardwareRTNP.ASP;RTLessorAVSCustHardwareRTNlogk.ASP"
  functionOptPrompt="Y;Y;N;Y;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="移轉單號;主線;社區名;移轉類別;移轉申請日;移轉申請人;實際移轉人;移轉結案日;結案人員;作廢日;作廢人員;派工單號;轉移轉單人員;移轉數量"
  sqlDelete="SELECT  RTLessorAVSCustRTNHardware.RCVPRTNO AS RCVPRTNO, " _
                       &" CASE WHEN RTLessorAVSCustRTNHardware.DATASRC ='01' THEN '裝機派工領用'  WHEN  RTLessorAVSCustRTNHardware.DATASRC ='02'  THEN '續約收款派工領用' WHEN RTLessorAVSCustRTNHardware.DATASRC = '03' THEN '復機收款派工領用' ELSE '' END, " _
                       &" RTLessorAVSCustRTNHardware.APPLYDAT AS APPLYDAT, " _
                       &" CASE WHEN RTObj_7.CUSNC='' OR RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE  RTObj_7.CUSNC END , " _
                       &" CASE WHEN RTObj_1.CUSNC='' OR RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                       &" RTLessorAVSCustRTNHardware.CLOSEDAT AS CLOSEDAT, RTObj_4.CUSNC AS CUSNC5, RTLessorAVSCustRTNHardware.CANCELDAT AS CANCELDAT, RTObj_5.CUSNC AS CUSNC6, " _
                       &" RTLessorAVSCustRTNHardware.PRTNO AS PRTNO, " _
                       &" RTObj_6.CUSNC AS cusnc7,SUM(RTLessorAVSCustRTNHardwareDTL.QTY) " _
&"  FROM             RTEmployee RTEmployee_4 INNER JOIN RTObj RTObj_6 ON RTEmployee_4.CUSID = RTObj_6.CUSID RIGHT OUTER JOIN " _
                       &" RTLessorAVSCustRTNHardware ON RTEmployee_4.EMPLY = RTLessorAVSCustRTNHardware.RCVUSR LEFT OUTER JOIN " _
                       &" RTEmployee RTEmployee_3 INNER JOIN RTObj RTObj_5 ON RTEmployee_3.CUSID = RTObj_5.CUSID ON " _
                       &" RTLessorAVSCustRTNHardware.CANCELUSR = RTEmployee_3.EMPLY LEFT OUTER JOIN " _
                       &" RTEmployee RTEmployee_2 INNER JOIN RTObj RTObj_4 ON RTEmployee_2.CUSID = RTObj_4.CUSID ON " _
                       &" RTLessorAVSCustRTNHardware.CLOSEUSR = RTEmployee_2.EMPLY LEFT OUTER JOIN RTObj RTObj_3 ON " _
                       &" RTLessorAVSCustRTNHardware.REALCONSIGNEE = RTObj_3.CUSID LEFT OUTER JOIN RTObj RTObj_2 ON " _
                       &" RTLessorAVSCustRTNHardware.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN " _
                       &" RTEmployee RTEmployee_1 INNER JOIN RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON " _
                       &" RTLessorAVSCustRTNHardware.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
                       &" RTObj RTObj_7 INNER JOIN RTEmployee RTEmployee_5 ON RTObj_7.CUSID = RTEmployee_5.CUSID ON " _
                       &" RTLessorAVSCustRTNHardware.ASSIGNENGINEER = RTEmployee_5.EMPLY LEFT OUTER JOIN RTLessorAVSCustRTNHardwareDTL ON " _
                       &" RTLessorAVSCustRTNHardware.RCVPRTNO=RTLessorAVSCustRTNHardwareDTL.RCVPRTNO " _
                       &" where " & searchqry & " " _
                       &" GROUP BY  RTLessorAVSCustRTNHardware.RCVPRTNO , " _
                       &" CASE WHEN RTLessorAVSCustRTNHardware.DATASRC ='01' THEN '裝機派工領用'  WHEN  RTLessorAVSCustRTNHardware.DATASRC ='02'  THEN '續約收款派工領用' WHEN RTLessorAVSCustRTNHardware.DATASRC = '03' THEN '復機收款派工領用' ELSE '' END, " _
                       &" RTLessorAVSCustRTNHardware.APPLYDAT , " _
                       &" CASE WHEN RTObj_7.CUSNC='' OR RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE  RTObj_7.CUSNC END , " _
                       &" CASE WHEN RTObj_1.CUSNC='' OR RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                       &" RTLessorAVSCustRTNHardware.CLOSEDAT, RTObj_4.CUSNC, RTObj_5.CUSNC, RTLessorAVSCustRTNHardware.PRTNO, " _
                       &" RTLessorAVSCustRTNHardware.CANCELDAT, RTObj_6.CUSNC " _
                       &" where RTLessorAVSCustRTNHardware.rcvprtno='' "
  dataTable="RTLessorAVSCustRTNHardware"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="RTLessorAVSCustHardwareRTND.ASP"
  datawindowFeature=""
  searchWindowFeature="width=350,height=250,scrollbars=yes"
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
  searchProg="RTLessorAVSCustHardwareRTNS1.ASP"
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
     searchQry=" RTLessorAVSCustRTNHardware.prtno<>'' and RTLessorAVSCustRTNHardware.canceldat is null and RTLessorAVSCustRTNHardware.closedat is null "
     searchShow="尚未移轉結案"
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
  
          sqlList="SELECT RTLessorAVSCustRTNHardware.RCVPRTNO AS RCVPRTNO,  RTRIM(LTRIM(CONVERT(char(6), RTLessorAVSCMTYLINEDROPSNDWORK.COMQ1))) " _
                 &"+ '-' + RTRIM(LTRIM(CONVERT(char(6), RTLessorAVSCMTYLINEDROPSNDWORK.LINEQ1))) AS comqline, RTLessorAVSCMTYH.COMN, " _
                 &"RTCODE_1.CODENC,RTLessorAVSCustRTNHardware.APPLYDAT AS APPLYDAT, " _ 
                 &"CASE WHEN RTObj_7.CUSNC = '' OR RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                 &"CASE WHEN RTObj_1.CUSNC = '' OR RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                 &"RTLessorAVSCustRTNHardware.CLOSEDAT AS CLOSEDAT, RTObj_4.CUSNC AS CUSNC5,RTLessorAVSCustRTNHardware.CANCELDAT AS CANCELDAT, " _
                 &"RTObj_5.CUSNC AS CUSNC6, RTLessorAVSCustRTNHardware.PRTNO AS PRTNO, RTObj_6.CUSNC AS cusnc7, " _
                 &"SUM(RTLessorAVSCustRTNHardwareDTL.QTY) " _
                 &"FROM   RTEmployee RTEmployee_4 INNER JOIN RTObj RTObj_6 ON RTEmployee_4.CUSID = RTObj_6.CUSID " _
                 &"RIGHT OUTER JOIN RTLessorAVSCustRTNHardware ON RTEmployee_4.EMPLY = RTLessorAVSCustRTNHardware.RCVUSR " _
                 &"LEFT OUTER JOIN RTEmployee RTEmployee_3 INNER JOIN RTObj RTObj_5 ON RTEmployee_3.CUSID = RTObj_5.CUSID " _
                 &"ON RTLessorAVSCustRTNHardware.CANCELUSR = RTEmployee_3.EMPLY LEFT OUTER JOIN RTEmployee RTEmployee_2 " _
                 &"INNER JOIN RTObj RTObj_4 ON RTEmployee_2.CUSID = RTObj_4.CUSID ON " _
                 &"RTLessorAVSCustRTNHardware.CLOSEUSR = RTEmployee_2.EMPLY LEFT OUTER JOIN RTObj RTObj_3 ON " _
                 &"RTLessorAVSCustRTNHardware.REALCONSIGNEE = RTObj_3.CUSID LEFT OUTER JOIN RTObj RTObj_2 ON " _
                 &"RTLessorAVSCustRTNHardware.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 " _
                 &"INNER JOIN RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON " _
                 &"RTLessorAVSCustRTNHardware.REALENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN RTObj RTObj_7 INNER JOIN " _
                 &"RTEmployee RTEmployee_5 ON RTObj_7.CUSID = RTEmployee_5.CUSID ON " _
                 &"RTLessorAVSCustRTNHardware.ASSIGNENGINEER = RTEmployee_5.EMPLY LEFT OUTER JOIN RTLessorAVSCustRTNHardwareDTL ON " _
                 &"RTLessorAVSCustRTNHardware.RCVPRTNO = RTLessorAVSCustRTNHardwareDTL.RCVPRTNO LEFT OUTER JOIN " _
                 &"RTLessorAVSCMTYLINEDROPSNDWORK ON RTLessorAVSCustRTNHardware.prtno = RTLessorAVSCMTYLINEDROPSNDWORK.prtno LEFT OUTER JOIN " _
                 &"RTLessorAVSCMTYH ON RTLessorAVSCMTYLINEDROPSNDWORK.COMQ1 = RTLessorAVSCMTYH.COMQ1  LEFT OUTER JOIN RTCODE RTCODE_1 ON RTLessorAVSCustRTNHardware.DATASRC=RTCODE_1.CODE AND RTCODE_1.KIND='O1' " _
                 &"WHERE  RTLessorAVSCustRTNHardware.prtno <> '' AND RTLessorAVSCustRTNHardware.datasrc = '01' AND " & searchqry & " " _
                 &"GROUP BY  RTLessorAVSCustRTNHardware.RCVPRTNO, RTRIM(LTRIM(CONVERT(char(6), RTLessorAVSCMTYLINEDROPSNDWORK.COMQ1))) + '-' + RTRIM(LTRIM(CONVERT(char(6), RTLessorAVSCMTYLINEDROPSNDWORK.LINEQ1))), " _
                 &"RTLessorAVSCMTYH.COMN,RTCODE_1.CODENC,RTLessorAVSCustRTNHardware.APPLYDAT, " _
                 &"CASE WHEN RTObj_7.CUSNC = '' OR RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                 &"CASE WHEN RTObj_1.CUSNC = '' OR RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                 &"RTLessorAVSCustRTNHardware.CLOSEDAT, RTObj_4.CUSNC, RTObj_5.CUSNC, RTLessorAVSCustRTNHardware.PRTNO, " _
                 &"RTLessorAVSCustRTNHardware.CANCELDAT, RTObj_6.CUSNC " 
                                                                                             

   'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>