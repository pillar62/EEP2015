<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="社區主線派工單資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=" 列 印 ;完工結案;未完工結案;結案返轉; 作 廢 ;作廢返轉;設備明細;歷史異動"
  functionOptProgram="RTcmtyLINESNDPV.asp;rtcmtySNDWORKF.asp;rtcmtySNDWORKUF.asp;rtcmtySNDWORKFR.asp;rtcmtySNDWORKdrop.asp;rtcmtySNDWORKdropc.asp;rtcmtyhardwareK.asp;rtcmtySNDWORKLOGK.asp"
  functionOptPrompt="N;Y;Y;Y;Y;Y;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;派工單號;派工日;列印人員;預定施工員;實際施工員;裝機完成;完工結案;未完工結案;作廢日;獎金結算月;獎金審核日;庫存結算月;庫存審核日"
  sqlDelete="SELECT rtcmtySNDWORK.COMQ1, rtcmtySNDWORK.PRTNO, rtcmtySNDWORK.PRTNO, " _
           &"rtcmtySNDWORK.SENDWORKDAT, RTObj_8.CUSNC, " _
           &"CASE WHEN RTOBJ_6.SHORTNC <> '' THEN RTOBJ_6.SHORTNC ELSE CASE WHEN RTObj.CUSNC <> '' THEN RTObj.CUSNC + '/' ELSE '' END " _
           &"+ CASE WHEN RTObj_1.CUSNC <> '' THEN RTObj_1.CUSNC + '/' ELSE '' END " _
           &"+ CASE WHEN RTObj_2.CUSNC <> '' THEN RTObj_2.CUSNC + '/' ELSE '' END END AS assigneengneer," _
           &"CASE WHEN RTOBJ_7.SHORTNC <> '' THEN RTOBJ_7.SHORTNC ELSE CASE WHEN RTObj_3.CUSNC <> '' THEN RTObj_3.CUSNC + '/' ELSE '' END " _
           &"+ CASE WHEN RTObj_4.CUSNC <> '' THEN RTObj_4.CUSNC + '/' ELSE '' END " _
           &"+ CASE WHEN RTObj_5.CUSNC <> '' THEN RTObj_5.CUSNC + '/' ELSE '' END END AS realengineer, " _
           &"rtcmtySNDWORK.CLOSEDAT,rtcmtySNDWORK.FINISHDAT,rtcmtySNDWORK.UNCLOSEDAT,rtcmtySNDWORK.DROPDAT,rtcmtySNDWORK.BONUSCLOSEYM, rtcmtySNDWORK.BONUSFINCHK, rtcmtySNDWORK.STOCKCLOSEYM, " _
           &"rtcmtySNDWORK.STOCKFINCHK " _
           &"FROM         RTObj RTObj_6 RIGHT OUTER JOIN " _
           &"RTObj RTObj_8 INNER JOIN " _
           &"RTEmployee RTEmployee_6 ON " _
           &"RTObj_8.CUSID = RTEmployee_6.CUSID RIGHT OUTER JOIN " _
           &"rtcmtySNDWORK ON " _
           &"RTEmployee_6.EMPLY = rtcmtySNDWORK.PRTUSR LEFT OUTER JOIN " _
           &"RTObj RTObj_7 ON " _
           &"rtcmtySNDWORK.REALCONSIGNEE = RTObj_7.CUSID ON  " _
           &"RTObj_6.CUSID = rtcmtySNDWORK.ASSIGNCONSIGNEE LEFT OUTER  JOIN " _
           &"RTObj RTObj_5 INNER JOIN " _
           &"RTEmployee RTEmployee_5 ON RTObj_5.CUSID = RTEmployee_5.CUSID ON " _
           &"rtcmtySNDWORK.REALENGINEER3 = RTEmployee_5.EMPLY LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_4 INNER JOIN " _
           &"RTObj RTObj_4 ON RTEmployee_4.CUSID = RTObj_4.CUSID ON " _
           &"rtcmtySNDWORK.REALENGINEER2 = RTEmployee_4.EMPLY LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_3 INNER JOIN " _
           &"RTObj RTObj_3 ON RTEmployee_3.CUSID = RTObj_3.CUSID ON " _
           &"rtcmtySNDWORK.REALENGINEER1 = RTEmployee_3.EMPLY LEFT OUTER JOIN " _
            &"RTObj RTObj_2 INNER JOIN " _
           &"RTEmployee RTEmployee_2 ON RTObj_2.CUSID = RTEmployee_2.CUSID ON " _
            &"rtcmtySNDWORK.ASSIGNENGINEER3 = RTEmployee_2.EMPLY LEFT OUTER JOIN " _
            &"RTEmployee RTEmployee_1 INNER JOIN " _
            &"RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON " _
            &"rtcmtySNDWORK.ASSIGNENGINEER2 = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
            &"RTObj INNER JOIN " _
            &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
            &"rtcmtySNDWORK.ASSIGNENGINEER1 = RTEmployee.EMPLY "
  dataTable="rtcmtySNDWORK"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="rtcmtySNDWORKD.asp"
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
'     searchQry=" RTCmty.COMQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.COMQ1 <> 0
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
'  set connYY=server.CreateObject("ADODB.connection")
'  set rsYY=server.CreateObject("ADODB.recordset")
'  dsnYY="DSN=RTLIB"
'  sqlYY="select * from rtcmty LEFT OUTER JOIN RTCOUNTY ON RTCMTY.CUTID=RTCOUNTY.CUTID where COMQ1=" & ARYPARMKEY(0)
'  connYY.Open dsnYY
'  rsYY.Open sqlYY,connYY
'  if not rsYY.EOF then
'     COMN=rsYY("COMN")
'     TELEADDR=RSYY("CUTNC") & RSYY("TOWNSHIP") & RSYY("ADDR")
'  else
'     COMN=""
'     TELEADDR=""
'  end if
'  rsYY.Close
 ' connYY.Close
 ' set rsYY=nothing
'  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" rtcmtySNDWORK.COMQ1=" & aryparmkey(0) 
     searchShow="社區序號︰"& aryparmkey(0) & ",社區名稱︰" & COMN & ",社區地址︰" & TELEADDR
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
  'if UCASE(emply)="T89001" or Ucase(emply)="T89002" or  Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="P92010" OR _
  '   Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T89076"then
  '   DAreaID="<>'*'"
  'end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  if userlevel=31 or userlevel =1  or userlevel =5 then DAreaID="<>'*'"
  
    If searchShow="全部" Then
         sqlList="SELECT rtcmtySNDWORK.COMQ1, rtcmtySNDWORK.PRTNO, rtcmtySNDWORK.PRTNO,  " _
           &"rtcmtySNDWORK.SENDWORKDAT, RTObj_8.CUSNC, " _
           &"CASE WHEN RTOBJ_6.SHORTNC <> '' THEN RTOBJ_6.SHORTNC ELSE CASE WHEN RTObj.CUSNC <> '' THEN RTObj.CUSNC  ELSE '' END " _
           &"+ CASE WHEN RTObj_1.CUSNC <> '' THEN '/' + RTObj_1.CUSNC ELSE '' END " _
           &"+ CASE WHEN RTObj_2.CUSNC <> '' THEN '/' + RTObj_2.CUSNC ELSE '' END END AS assigneengneer," _
           &"CASE WHEN RTOBJ_7.SHORTNC <> '' THEN RTOBJ_7.SHORTNC ELSE CASE WHEN RTObj_3.CUSNC <> '' THEN RTObj_3.CUSNC ELSE '' END " _
           &"+ CASE WHEN RTObj_4.CUSNC <> '' THEN '/' + RTObj_4.CUSNC  ELSE '' END " _
           &"+ CASE WHEN RTObj_5.CUSNC <> '' THEN '/' + RTObj_5.CUSNC ELSE '' END END AS realengineer, " _
           &"rtcmtySNDWORK.CLOSEDAT,rtcmtySNDWORK.FINISHDAT,rtcmtySNDWORK.UNCLOSEDAT,rtcmtySNDWORK.DROPDAT,rtcmtySNDWORK.BONUSCLOSEYM, rtcmtySNDWORK.BONUSFINCHK, rtcmtySNDWORK.STOCKCLOSEYM, " _
           &"rtcmtySNDWORK.STOCKFINCHK " _
           &"FROM         RTObj RTObj_6 RIGHT OUTER JOIN " _
           &"RTObj RTObj_8 INNER JOIN " _
           &"RTEmployee RTEmployee_6 ON " _
           &"RTObj_8.CUSID = RTEmployee_6.CUSID RIGHT OUTER JOIN " _
           &"rtcmtySNDWORK ON " _
           &"RTEmployee_6.EMPLY = rtcmtySNDWORK.PRTUSR LEFT OUTER JOIN " _
           &"RTObj RTObj_7 ON " _
           &"rtcmtySNDWORK.REALCONSIGNEE = RTObj_7.CUSID ON  " _
           &"RTObj_6.CUSID = rtcmtySNDWORK.ASSIGNCONSIGNEE LEFT OUTER  JOIN " _
           &"RTObj RTObj_5 INNER JOIN " _
           &"RTEmployee RTEmployee_5 ON RTObj_5.CUSID = RTEmployee_5.CUSID ON " _
           &"rtcmtySNDWORK.REALENGINEER3 = RTEmployee_5.EMPLY LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_4 INNER JOIN " _
           &"RTObj RTObj_4 ON RTEmployee_4.CUSID = RTObj_4.CUSID ON " _
           &"rtcmtySNDWORK.REALENGINEER2 = RTEmployee_4.EMPLY LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_3 INNER JOIN " _
           &"RTObj RTObj_3 ON RTEmployee_3.CUSID = RTObj_3.CUSID ON " _
           &"rtcmtySNDWORK.REALENGINEER1 = RTEmployee_3.EMPLY LEFT OUTER JOIN " _
            &"RTObj RTObj_2 INNER JOIN " _
           &"RTEmployee RTEmployee_2 ON RTObj_2.CUSID = RTEmployee_2.CUSID ON " _
            &"rtcmtySNDWORK.ASSIGNENGINEER3 = RTEmployee_2.EMPLY LEFT OUTER JOIN " _
            &"RTEmployee RTEmployee_1 INNER JOIN " _
            &"RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON " _
            &"rtcmtySNDWORK.ASSIGNENGINEER2 = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
            &"RTObj INNER JOIN " _
            &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
            &"rtcmtySNDWORK.ASSIGNENGINEER1 = RTEmployee.EMPLY " _
            &"where " & searchqry
    Else
         sqlList="SELECT rtcmtySNDWORK.COMQ1, rtcmtySNDWORK.PRTNO, rtcmtySNDWORK.PRTNO,  " _
           &"rtcmtySNDWORK.SENDWORKDAT, RTObj_8.CUSNC, " _
           &"CASE WHEN RTOBJ_6.SHORTNC <> '' THEN RTOBJ_6.SHORTNC ELSE CASE WHEN RTObj.CUSNC <> '' THEN RTObj.CUSNC  ELSE '' END " _
           &"+ CASE WHEN RTObj_1.CUSNC <> '' THEN '/' + RTObj_1.CUSNC ELSE '' END " _
           &"+ CASE WHEN RTObj_2.CUSNC <> '' THEN '/' + RTObj_2.CUSNC  ELSE '' END END AS assigneengneer," _
           &"CASE WHEN RTOBJ_7.SHORTNC <> '' THEN RTOBJ_7.SHORTNC ELSE CASE WHEN RTObj_3.CUSNC <> '' THEN RTObj_3.CUSNC  ELSE '' END " _
           &"+ CASE WHEN RTObj_4.CUSNC <> '' THEN '/' + RTObj_4.CUSNC  ELSE '' END " _
           &"+ CASE WHEN RTObj_5.CUSNC <> '' THEN '/' + RTObj_5.CUSNC  ELSE '' END END AS realengineer, " _
           &"rtcmtySNDWORK.CLOSEDAT,rtcmtySNDWORK.FINISHDAT,rtcmtySNDWORK.UNCLOSEDAT,rtcmtySNDWORK.DROPDAT,rtcmtySNDWORK.BONUSCLOSEYM, rtcmtySNDWORK.BONUSFINCHK, rtcmtySNDWORK.STOCKCLOSEYM, " _
           &"rtcmtySNDWORK.STOCKFINCHK " _
           &"FROM         RTObj RTObj_6 RIGHT OUTER JOIN " _
           &"RTObj RTObj_8 INNER JOIN " _
           &"RTEmployee RTEmployee_6 ON " _
           &"RTObj_8.CUSID = RTEmployee_6.CUSID RIGHT OUTER JOIN " _
           &"rtcmtySNDWORK ON " _
           &"RTEmployee_6.EMPLY = rtcmtySNDWORK.PRTUSR LEFT OUTER JOIN " _
           &"RTObj RTObj_7 ON " _
           &"rtcmtySNDWORK.REALCONSIGNEE = RTObj_7.CUSID ON  " _
           &"RTObj_6.CUSID = rtcmtySNDWORK.ASSIGNCONSIGNEE LEFT OUTER  JOIN " _
           &"RTObj RTObj_5 INNER JOIN " _
           &"RTEmployee RTEmployee_5 ON RTObj_5.CUSID = RTEmployee_5.CUSID ON " _
           &"rtcmtySNDWORK.REALENGINEER3 = RTEmployee_5.EMPLY LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_4 INNER JOIN " _
           &"RTObj RTObj_4 ON RTEmployee_4.CUSID = RTObj_4.CUSID ON " _
           &"rtcmtySNDWORK.REALENGINEER2 = RTEmployee_4.EMPLY LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_3 INNER JOIN " _
           &"RTObj RTObj_3 ON RTEmployee_3.CUSID = RTObj_3.CUSID ON " _
           &"rtcmtySNDWORK.REALENGINEER1 = RTEmployee_3.EMPLY LEFT OUTER JOIN " _
            &"RTObj RTObj_2 INNER JOIN " _
           &"RTEmployee RTEmployee_2 ON RTObj_2.CUSID = RTEmployee_2.CUSID ON " _
            &"rtcmtySNDWORK.ASSIGNENGINEER3 = RTEmployee_2.EMPLY LEFT OUTER JOIN " _
            &"RTEmployee RTEmployee_1 INNER JOIN " _
            &"RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON " _
            &"rtcmtySNDWORK.ASSIGNENGINEER2 = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
            &"RTObj INNER JOIN " _
            &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
            &"rtcmtySNDWORK.ASSIGNENGINEER1 = RTEmployee.EMPLY " _
           &"where " & searchqry
    End If  
  'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>