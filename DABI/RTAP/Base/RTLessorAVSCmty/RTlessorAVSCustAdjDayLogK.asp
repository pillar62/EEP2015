<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="AVS-City管理系統"
  title="AVS-City用戶調整到期日資料異動查詢"
  buttonName=" 新  增 ; 刪  除 ; 結  束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
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
  formatName="none;none;項次;異動日;異動別;異動員;調整期數;調整日數;結案日;結案員;作廢日;作廢員"
  sqlDelete="SELECT  RTLessorAVSCustAdjDayLog.CUSID, RTLessorAVSCustAdjDayLog.ENTRYNO, RTLessorAVSCustAdjDayLog.seq, " _
                &"          RTLessorAVSCustAdjDayLog.ADJPERIOD, RTLessorAVSCustAdjDayLog.ADJDAY,  " _
                &"          RTLessorAVSCustAdjDayLog.ADJCLOSEDAT, RTObj_2.CUSNC,  " _
                &"          RTLessorAVSCustAdjDayLog.CANCELDAT, RTObj_1.CUSNC AS Expr1 " _
                &"FROM      RTEmployee RTEmployee_1 LEFT OUTER JOIN " _
                &"          RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID RIGHT OUTER JOIN " _
                &"          RTLessorAVSCustAdjDayLog ON  " _
                &"          RTEmployee_1.EMPLY = RTLessorAVSCustAdjDayLog.CANCELUSR LEFT OUTER JOIN " _
                &"          RTObj RTObj_2 RIGHT OUTER JOIN " _
                &"          RTEmployee RTEmployee_2 ON RTObj_2.CUSID = RTEmployee_2.CUSID ON  " _
                &"          RTLessorAVSCustAdjDayLog.ADJCLOSEUSR = RTEmployee_2.EMPLY " _
                &"where RTLessorAVSCustAdjDayLog.CUSID='' "

  dataTable="RTLessorAVSCustAdjDayLog"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=350,height=400,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature="width=850,height=550,scrollbars=yes"
  diaWidth="400"
  diaHeight="250"
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="self"
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
  sqlYY="select * from RTLessorAVSCmtyH LEFT OUTER JOIN RTCOUNTY ON RTLessorAVSCmtyH.CUTID=RTCOUNTY.CUTID " _
       &"left outer join RTLessorAVScust on RTLessorAVSCmtyH.comq1=RTLessorAVScust.comq1 " _
       &"where RTLessorAVScust.cusid='" & ARYPARMKEY(0) & "' "
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
  else
     COMN=""
  end if
  rsYY.Close
  sqlYY="select * from RTLessorAVSCmtyLine LEFT OUTER JOIN RTCOUNTY ON RTLessorAVSCmtyLine.CUTID=RTCOUNTY.CUTID " _
       &"left outer join RTLessorAVScust on RTLessorAVSCmtyLine.comq1=RTLessorAVScust.comq1 and RTLessorAVSCmtyLine.lineq1=RTLessorAVScust.lineq1 " _
       &"where RTLessorAVScust.cusid='" & ARYPARMKEY(0) & "' " 
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     comaddr=""
     COMaddr=rsYY("cutnc") & rsyy("township")
     if rsyy("village") <> "" then
         COMaddr= COMaddr & rsyy("village") & rsyy("cod1")
     end if
     if rsyy("NEIGHBOR") <> "" then
         COMaddr= COMaddr & rsyy("NEIGHBOR") & rsyy("cod2")
     end if
     if rsyy("STREET") <> "" then
         COMaddr= COMaddr & rsyy("STREET") & rsyy("cod3")
     end if
     if rsyy("SEC") <> "" then
         COMaddr= COMaddr & rsyy("SEC") & rsyy("cod4")
     end if
     if rsyy("LANE") <> "" then
         COMaddr= COMaddr & rsyy("LANE") & rsyy("cod5")
     end if
     if rsyy("ALLEYWAY") <> "" then
         COMaddr= COMaddr & rsyy("ALLEYWAY") & rsyy("cod7")
     end if
     if rsyy("NUM") <> "" then
         COMaddr= COMaddr & rsyy("NUM") & rsyy("cod8")
     end if
     if rsyy("FLOOR") <> "" then
         COMaddr= COMaddr & rsyy("FLOOR") & rsyy("cod9")
     end if
     if rsyy("ROOM") <> "" then
         COMaddr= COMaddr & rsyy("ROOM") & rsyy("cod10")
     end if
   else
     COMaddr=""
  end if
  rsYY.Close
  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" and RTLessorAVSCustAdjDayLog.cusid='" & ARYPARMKEY(0) & "' and RTLessorAVSCustAdjDayLog.entryno= " & ARYPARMKEY(1)
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
  'if UCASE(emply)="T89001" or Ucase(emply)="T89002" or  Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="T93168" OR _
  '   Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T89076" then
  '   DAreaID="<>'*'"
  'end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  if userlevel=31 or userlevel =1  or userlevel =5 or userlevel =9 then DAreaID="<>'*'"
  
         sqlList="SELECT  RTLessorAVSCustAdjDayLog.CUSID, RTLessorAVSCustAdjDayLog.ENTRYNO, RTLessorAVSCustAdjDayLog.seq, " _
                &"          RTLessorAVSCustAdjDayLog.CHGDAT,RTCODE.CODENC,RTOBJ_3.CUSNC,RTLessorAVSCustAdjDayLog.ADJPERIOD, RTLessorAVSCustAdjDayLog.ADJDAY,  " _
                &"          RTLessorAVSCustAdjDayLog.ADJCLOSEDAT, RTObj_2.CUSNC,  " _
                &"          RTLessorAVSCustAdjDayLog.CANCELDAT, RTObj_1.CUSNC AS Expr1 " _
                &"FROM      RTEmployee RTEmployee_1 LEFT OUTER JOIN " _
                &"          RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID RIGHT OUTER JOIN " _
                &"          RTLessorAVSCustAdjDayLog ON  " _
                &"          RTEmployee_1.EMPLY = RTLessorAVSCustAdjDayLog.CANCELUSR LEFT OUTER JOIN " _
                &"          RTObj RTObj_2 RIGHT OUTER JOIN " _
                &"          RTEmployee RTEmployee_2 ON RTObj_2.CUSID = RTEmployee_2.CUSID ON  " _
                &"          RTLessorAVSCustAdjDayLog.ADJCLOSEUSR = RTEmployee_2.EMPLY left outer join rtcode on " _
                &"          RTLessorAVSCustAdjDayLog.chgcode=rtcode.code and rtcode.kind='G2' LEFT OUTER JOIN RTEmployee RTEmployee_3 ON " _
                &"          RTLessorAVSCustAdjDayLog.ChGUSR=RTEmployee_3.EMPLY LEFT OUTER JOIN RTOBJ RTOBJ_3 ON RTEmployee_3.CUSID=RTOBJ_3.CUSID " _
                &"where RTLessorAVSCustAdjDayLog.CUSID='" & ARYPARMKEY(0) & "' " & searchqry
  'end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>