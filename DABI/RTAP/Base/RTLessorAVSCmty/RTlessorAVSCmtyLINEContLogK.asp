<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="AVS-City管理系統"
  title="AVS-City主線續約異動資料查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""
  functionoptopen=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;異動項次;社區名稱;主線;異動日;異動類別;異動人員;主線IP;none;none;none;主線附掛;線路ISP;IP種類;主線速率;IP數;續約申請日;通測日;測通日;線路到期日;作廢日"
  sqlDelete="SELECT RTLessorAVSCmtyLineContLog.COMQ1, RTLessorAVSCmtyLineContLog.LINEQ1, RTLessorAVSCmtyLineContLog.ENTRYNO, RTLessorAVSCmtyLineContLog.seq,RTLessorAVSCMTYH.COMN,rtrim(convert(char(6),RTLessorAVSCmtyLineContLog.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorAVSCmtyLineContLog.lineQ1)) ,  " _
                &"RTLessorAVSCmtyLineContLog.chgdat,rtcode_5.codenc,rtobj_5.cusnc,RTLessorAVSCmtyLineContLog.LINEIP,RTLessorAVSCmtyLineContLog.GATEWAY, " _
                &"RTLessorAVSCmtyLineContLog.PPPOEACCOUNT, RTLessorAVSCmtyLineContLog.PPPOEPASSWORD, RTLessorAVSCmtyLineContLog.LINETEL, " _
                &"RTCode_1.CODENC, RTCode_3.CODENC AS Expr1, RTCode_2.CODENC AS Expr2, RTLessorAVSCmtyLineContLog.IPCNT, " _
                &"RTLessorAVSCmtyLineContLog.CONTAPPLYDAT,RTLessorAVSCmtyLineContLog.HINETNOTIFYDAT, " _
                &"RTLessorAVSCmtyLineContLog.ADSLAPPLYDAT, RTLessorAVSCmtyLineContLog.LINEDUEDAT, " _
                &"RTLessorAVSCmtyLineContLog.CANCELDAT " _
                &"FROM    RTLessorAVSCmtyLineContLog LEFT OUTER JOIN RTLessorAVSCust ON RTLessorAVSCmtyLineContLog.COMQ1 = RTLessorAVSCust.COMQ1 AND " _
                &"RTLessorAVSCmtyLineContLog.LINEQ1 = RTLessorAVSCust.LINEQ1 LEFT OUTER JOIN RTCode RTCode_3 ON " _
                &"RTLessorAVSCmtyLineContLog.LINEIPTYPE = RTCode_3.CODE AND RTCode_3.KIND = 'M5' LEFT OUTER JOIN " _
                &"RTCode RTCode_1 ON RTLessorAVSCmtyLineContLog.LINEISP = RTCode_1.CODE AND RTCode_1.KIND = 'C3' LEFT OUTER JOIN " _
                &"RTCode RTCode_2 ON RTLessorAVSCmtyLineContLog.LINERATE = RTCode_2.CODE AND RTCode_2.KIND = 'D3' LEFT OUTER JOIN " _
                &"RTEmployee INNER JOIN RTObj RTObj_1 ON RTEmployee.CUSID = RTObj_1.CUSID ON " _
                &"RTLessorAVSCmtyLineContLog.SALESID = RTEmployee.EMPLY LEFT OUTER JOIN RTObj RTObj_2 ON " _
                &"RTLessorAVSCmtyLineContLog.CONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTLessorAVSCMTYH ON " _
                &"RTLessorAVSCmtyLineContLog.COMQ1 = RTLessorAVSCMTYH.COMQ1 LEFT OUTER JOIN RTCOUNTY ON " _
                &"RTLessorAVSCmtyLineContLog.CUTID = RTCOUNTY.CUTID left outer join rtcode rtcode_5 on RTLessorAVSCmtyLineContLog.chgcode=rtcode_5.code and rtcode_5.kind='G2' " _
                &"left outer join rtemployee on RTLessorAVSCmtyLineContLog.chgusr=rtemployee.emply left outer join rtobj rtobj_5 on rtemployee.cusid=rtobj_5.cusid " _
                &"WHERE RTLessorAVSCmtyLineContLog.COMQ1=0 " 

  dataTable="RTLessorAVSCmtyLineContLog"
  userDefineDelete="Yes"
  numberOfKey=4
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
  '----
  set connYY=server.CreateObject("ADODB.connection")
  set rsYY=server.CreateObject("ADODB.recordset")
  dsnYY="DSN=RTLIB"
  sqlYY="select * from RTLessorAVSCmtyH LEFT OUTER JOIN RTCOUNTY ON RTLessorAVSCmtyH.CUTID=RTCOUNTY.CUTID where COMQ1=" & ARYPARMKEY(0)
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
     COMADDR=RSYY("CUTNC") & RSYY("TOWNSHIP") & RSYY("RADDR")
  else
     COMN=""
     COMADDR=""
  end if
  rsYY.Close
  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTLessorAVSCmtyLineContLog.ComQ1=" & aryparmkey(0) & " AND RTLessorAVSCmtyLineContLog.LINEQ1=" & aryparmkey(1)
     searchShow="社區序號︰"& aryparmkey(0) & ",社區名稱︰" & COMN & ",社區地址︰" & COMADDR
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
  '  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or Ucase(emply)="T89003" or _
  '	 Ucase(emply)="T89018" or Ucase(emply)="T89020" or Ucase(emply)="T89025" or Ucase(emply)="T91099" or _
  '	 Ucase(emply)="T92134" or Ucase(emply)="T93168" or Ucase(emply)="T93177" or Ucase(emply)="T94180" then
  '   DAreaID="<>'*'"
  'end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  if userlevel=31 or userlevel =1  or userlevel =5 then DAreaID="<>'*'"
         sqlList="SELECT RTLessorAVSCmtyLineContLog.COMQ1, RTLessorAVSCmtyLineContLog.LINEQ1, RTLessorAVSCmtyLineContLog.ENTRYNO, RTLessorAVSCmtyLineContLog.seq,RTLessorAVSCMTYH.COMN,rtrim(convert(char(6),RTLessorAVSCmtyLineContLog.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorAVSCmtyLineContLog.lineQ1)) ,  " _
                &"RTLessorAVSCmtyLineContLog.chgdat,rtcode_5.codenc,rtobj_5.cusnc,RTLessorAVSCmtyLineContLog.LINEIP,RTLessorAVSCmtyLineContLog.GATEWAY, " _
                &"RTLessorAVSCmtyLineContLog.PPPOEACCOUNT, RTLessorAVSCmtyLineContLog.PPPOEPASSWORD, RTLessorAVSCmtyLineContLog.LINETEL, " _
                &"RTCode_1.CODENC, RTCode_3.CODENC AS Expr1, RTCode_2.CODENC AS Expr2, RTLessorAVSCmtyLineContLog.IPCNT, " _
                &"RTLessorAVSCmtyLineContLog.CONTAPPLYDAT, RTLessorAVSCmtyLineContLog.HINETNOTIFYDAT, " _
                &"RTLessorAVSCmtyLineContLog.ADSLAPPLYDAT, RTLessorAVSCmtyLineContLog.LINEDUEDAT, " _
                &"RTLessorAVSCmtyLineContLog.CANCELDAT " _
                &"FROM    RTLessorAVSCmtyLineContLog  LEFT OUTER JOIN RTCode RTCode_3 ON " _
                &"RTLessorAVSCmtyLineContLog.LINEIPTYPE = RTCode_3.CODE AND RTCode_3.KIND = 'M5' LEFT OUTER JOIN " _
                &"RTCode RTCode_1 ON RTLessorAVSCmtyLineContLog.LINEISP = RTCode_1.CODE AND RTCode_1.KIND = 'C3' LEFT OUTER JOIN " _
                &"RTCode RTCode_2 ON RTLessorAVSCmtyLineContLog.LINERATE = RTCode_2.CODE AND RTCode_2.KIND = 'D3' " _
                &"LEFT OUTER JOIN RTLessorAVSCMTYH ON " _
                &"RTLessorAVSCmtyLineContLog.COMQ1 = RTLessorAVSCMTYH.COMQ1 left outer join rtcode rtcode_5 on RTLessorAVSCmtyLineContLog.chgcode=rtcode_5.code and rtcode_5.kind='G2' " _
                &"left outer join rtemployee on RTLessorAVSCmtyLineContLog.chgusr=rtemployee.emply left outer join rtobj rtobj_5 on rtemployee.cusid=rtobj_5.cusid " _
                &"WHERE " & SEARCHQRY & " ORDER BY ENTRYNO" 
                      

 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>