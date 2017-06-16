<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="AVS-City管理系統"
  title="AVS-City用戶退租單異動查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;異動項次;類別;異動日;異動人員;退租種類;退租申請日;預定退租日;退租結案日;結案人員;拆機工單;轉拆機單日期;拆機結案日;作廢日;作廢人員;none;none"
  sqlDelete="SELECT  RTLessorAVSCustDropLog.CUSID, RTLessorAVSCustDropLog.ENTRYNO, RTLessorAVSCustDropLog.seq,RTCODE_1.CODENC," _
                &"RTLessorAVSCustDropLog.chgdat, RTCode.CODENC,RTLessorAVSCustDropLog.APPLYDAT, RTLessorAVSCustDropLog.ENDDAT, " _
                &"RTLessorAVSCustDropLog.FINISHDAT, RTObj_3.CUSNC, RTLessorAVSCustDropLog.SNDPRTNO, RTLessorAVSCustDropLog.SNDWORK," _
                &"RTLessorAVSCustDropLog.SNDWORKCLOSE, RTLessorAVSCustDropLog.CANCELDAT,RTObj_1.CUSNC AS Expr1," _
                &"RTObj_2.CUSNC AS Expr2, RTObj_3.CUSNC AS Expr3 " _
                &"FROM    RTEmployee RTEmployee_3 INNER JOIN RTObj RTObj_3 ON RTEmployee_3.CUSID = RTObj_3.CUSID " _
                &"RIGHT OUTER JOIN RTLessorAVSCustDropLog ON RTEmployee_3.EMPLY = RTLessorAVSCustDropLog.UUSR LEFT OUTER JOIN " _
                &"RTEmployee RTEmployee_2 INNER JOIN RTObj RTObj_2 ON RTEmployee_2.CUSID = RTObj_2.CUSID ON " _
                &"RTLessorAVSCustDropLog.EUSR = RTEmployee_2.EMPLY LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN " _
                &"RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON RTLessorAVSCustDropLog.CANCELUSR = " _
                &"RTEmployee_1.EMPLY LEFT OUTER JOIN RTObj RTObj_4 INNER JOIN RTEmployee RTEmployee_4 ON " _
                &"RTObj_4.CUSID = RTEmployee_4.CUSID ON RTLessorAVSCustDropLog.FUSR = RTEmployee_4.EMPLY LEFT OUTER JOIN " _
                &"RTCode ON RTLessorAVSCustDropLog.DROPKIND = RTCode.CODE AND RTCode.KIND = 'N7' left outer join " _
                &"rtcode rtcode_1 on RTLessorAVSCustDropLog.chgcode=rtcode_1.code and rtcode_1.kind='G2' " _
                &"where RTLessorAVSCustDropLog.cusid='' "
  dataTable="RTLessorAVSCUSTdropLog"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=350,height=160,scrollbars=yes"
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
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorAVSCmtyH ON " _
       &"RTCounty.CUTID = RTLessorAVSCmtyH.CUTID RIGHT OUTER JOIN RTLessorAVSCust ON RTLessorAVSCmtyH.COMQ1 = RTLessorAVSCust.COMQ1 " _
       &"where RTLessorAVSCust.cusid='" & ARYPARMKEY(0) & "'"
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
  else
     COMN=""
  end if
  rsYY.Close
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorAVSCmtyLine ON  " _
       &"RTCounty.CUTID = RTLessorAVSCmtyLine.CUTID RIGHT OUTER JOIN " _
       &"RTLessorAVSCust ON RTLessorAVSCmtyLine.COMQ1 = RTLessorAVSCust.COMQ1 AND " _
       &"RTLessorAVSCmtyLine.LINEQ1 = RTLessorAVSCust.LINEQ1 " _
       &"where RTLessorAVSCust.cusid='" & ARYPARMKEY(0) & "'"
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
  RSYY.Close
  sqlYY="select * from RTLessorAVSCUST  where CUSID='" & ARYPARMKEY(0) & "' "
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     CUSNC=rsYY("CUSNC")
     comq1xx=rsyy("comq1")
     lineq1xx=rsyy("lineq1")
  else
     CUSNC=""
     comq1xx=""
     lineq1xx=""
  end if
  rsYY.Close
  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTLessorAVSCUSTdropLog.cusid='" & aryparmkey(0) & "' and RTLessorAVSCUSTdropLog.entryno='" & aryparmkey(1) & "'"
     searchShow="主線序號︰"& comq1xx &"-" & lineq1xx & ",社區名稱︰" & COMN & ",用戶名稱︰" & cusnc & ",主線位址︰" & COMADDR
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
  
         sqlList="SELECT  RTLessorAVSCustDropLog.CUSID, RTLessorAVSCustDropLog.ENTRYNO, RTLessorAVSCustDropLog.seq,RTCODE_1.CODENC," _
                &"RTLessorAVSCustDropLog.chgdat,rtobj_5.cusnc, RTCode.CODENC,RTLessorAVSCustDropLog.APPLYDAT, RTLessorAVSCustDropLog.ENDDAT, " _
                &"RTLessorAVSCustDropLog.FINISHDAT, RTObj_3.CUSNC, RTLessorAVSCustDropLog.SNDPRTNO, RTLessorAVSCustDropLog.SNDWORK," _
                &"RTLessorAVSCustDropLog.SNDWORKCLOSE, RTLessorAVSCustDropLog.CANCELDAT,RTObj_1.CUSNC AS Expr1," _
                &"RTObj_2.CUSNC AS Expr2, RTObj_3.CUSNC AS Expr3 " _
                &"FROM    RTEmployee RTEmployee_3 INNER JOIN RTObj RTObj_3 ON RTEmployee_3.CUSID = RTObj_3.CUSID " _
                &"RIGHT OUTER JOIN RTLessorAVSCustDropLog ON RTEmployee_3.EMPLY = RTLessorAVSCustDropLog.UUSR LEFT OUTER JOIN " _
                &"RTEmployee RTEmployee_2 INNER JOIN RTObj RTObj_2 ON RTEmployee_2.CUSID = RTObj_2.CUSID ON " _
                &"RTLessorAVSCustDropLog.EUSR = RTEmployee_2.EMPLY LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN " _
                &"RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON RTLessorAVSCustDropLog.CANCELUSR = " _
                &"RTEmployee_1.EMPLY LEFT OUTER JOIN RTObj RTObj_4 INNER JOIN RTEmployee RTEmployee_4 ON " _
                &"RTObj_4.CUSID = RTEmployee_4.CUSID ON RTLessorAVSCustDropLog.FUSR = RTEmployee_4.EMPLY LEFT OUTER JOIN " _
                &"RTCode ON RTLessorAVSCustDropLog.DROPKIND = RTCode.CODE AND RTCode.KIND = 'N7' left outer join " _
                &"rtcode rtcode_1 on RTLessorAVSCustDropLog.chgcode=rtcode_1.code and rtcode_1.kind='G2' " _
                &"left outer join rtemployee rtemployee_5 on RTLessorAVSCustDropLog.chgusr=rtemployee_5.emply inner join rtobj rtobj_5 on " _
                &"rtemployee_5.cusid=rtobj_5.cusid " _
                &"where RTLessorAVSCustDropLog.cusid='" & aryparmkey(0) & "' and " & searchqry 
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>