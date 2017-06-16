<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="ET-City管理系統"
  title="ET-City主線撤線異動資料查詢"
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
  formatName="none;none;none;none;主線;none;異動項次;異動類別;異動日;異動員;撤線種類;撤線申請日;預定撤線日;撤線原因;拆機單號;派工日期;拆機結案日;撤線結案日;作廢日;作廢員"
  sqlDelete="SELECT    RTLessorCmtyLineDropLog.COMQ1, RTLessorCmtyLineDropLog.LINEQ1, " _
                &"          RTLessorCmtyLineDropLog.ENTRYNO,RTLessorCmtyLineDropLog.seq,rtrim(convert(char(6),RTLessorCmtyLineDropLog.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorCmtyLineDropLog.lineQ1))," _
                &"          rtlessorcmtyh.comn,RTLessorCmtyLineDropLog.seq,RTLessorCmtyLineDropLog.chgdat,rtcode_2.codenc,rtobj_2.cusnc, RTCode.CODENC, RTLessorCmtyLineDropLog.DROPAPPLYDAT,  " _
                &"          RTLessorCmtyLineDropLog.SCHDROPDAT,  " _
                &"          RTLessorCmtyLineDropLog.DROPREASON,RTLessorCmtyLineDropLog.SNDPRTNO,RTLessorCmtyLineDropLog.SNDWORKDAT," _
                &"          RTLessorCmtyLineDropLog.SNDCLOSEDAT,RTLessorCmtyLineDropLog.CLOSEDAT,  " _
                &"          RTLessorCmtyLineDropLog.CANCELDAT, RTObj_1.CUSNC " _
                &"FROM      RTEmployee RTEmployee_1 LEFT OUTER JOIN " _
                &"          RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID RIGHT OUTER JOIN " _
                &"          RTLessorCmtyLineDropLog ON  " _
                &"          RTEmployee_1.EMPLY = RTLessorCmtyLineDropLog.CANCELUSR LEFT OUTER JOIN " _
                &"          RTObj RIGHT OUTER JOIN " _
                &"          RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON  " _
                &"          RTLessorCmtyLineDropLog.CLOSEUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
                &"          RTCode ON RTLessorCmtyLineDropLog.DROPKIND = RTCode.CODE AND  " _
                &"          RTCode.KIND = 'N9' " _
                &"WHERE RTLessorCmtyLineDropLog.ComQ1=0 " 

  dataTable="RTLessorCmtyLineDropLog"
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
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorCmtyH ON " _
       &"RTCounty.CUTID = RTLessorCmtyH.CUTID RIGHT OUTER JOIN RTLessorCmtyLine ON RTLessorCmtyH.COMQ1 = RTLessorCmtyLine.COMQ1 " _
       &"where RTLessorCmtyLine.comq1=" & ARYPARMKEY(0) 
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
  else
     COMN=""
  end if
  rsYY.Close
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorCmtyLine ON  " _
       &"RTCounty.CUTID = RTLessorCmtyLine.CUTID " _
       &"where RTLessorCmtyLine.comq1=" & ARYPARMKEY(0) & " and RTLessorCmtyLine.lineq1=" & ARYPARMKEY(1)
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
  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTLessorCmtyLineDropLog.ComQ1=" & aryparmkey(0) & " AND RTLessorCmtyLineDropLog.LINEQ1=" & aryparmkey(1) & " and RTLessorCmtyLineDropLog.entryno=" & aryparmkey(2)
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
         sqlList="SELECT    RTLessorCmtyLineDropLog.COMQ1, RTLessorCmtyLineDropLog.LINEQ1, " _
                &"          RTLessorCmtyLineDropLog.ENTRYNO,RTLessorCmtyLineDropLog.seq,rtrim(convert(char(6),RTLessorCmtyLineDropLog.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorCmtyLineDropLog.lineQ1))," _
                &"          rtlessorcmtyh.comn,RTLessorCmtyLineDropLog.seq,RTLessorCmtyLineDropLog.chgdat,rtcode_2.codenc,rtobj_2.cusnc, RTCode.CODENC, RTLessorCmtyLineDropLog.DROPAPPLYDAT,  " _
                &"          RTLessorCmtyLineDropLog.SCHDROPDAT,  " _
                &"          RTLessorCmtyLineDropLog.DROPREASON,RTLessorCmtyLineDropLog.SNDPRTNO,RTLessorCmtyLineDropLog.SNDWORKDAT," _
                &"          RTLessorCmtyLineDropLog.SNDCLOSEDAT,RTLessorCmtyLineDropLog.CLOSEDAT,  " _
                &"          RTLessorCmtyLineDropLog.CANCELDAT, RTObj_1.CUSNC " _
                &"FROM      RTEmployee RTEmployee_1 LEFT OUTER JOIN " _
                &"          RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID RIGHT OUTER JOIN " _
                &"          RTLessorCmtyLineDropLog ON  " _
                &"          RTEmployee_1.EMPLY = RTLessorCmtyLineDropLog.CANCELUSR LEFT OUTER JOIN " _
                &"          RTObj RIGHT OUTER JOIN " _
                &"          RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON  " _
                &"          RTLessorCmtyLineDropLog.CLOSEUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
                &"          RTCode ON RTLessorCmtyLineDropLog.DROPKIND = RTCode.CODE AND  " _
                &"          RTCode.KIND = 'N9' left outer join rtlessorcmtyh on " _
                &"          RTLessorCmtyLineDropLog.comq1=rtlessorcmtyh.comq1 left outer join rtcode rtcode_2 on " _
                &"          RTLessorCmtyLineDropLog.chgcode=rtcode_2.code and rtcode_2.kind='G2' left outer join rtemployee rtemployee_2 on " _
                &"          RTLessorCmtyLineDropLog.chgusr=rtemployee_2.emply left outer join rtobj rtobj_2 on rtemployee_2.cusid=rtobj_2.cusid " _
                &"WHERE RTLessorCmtyLineDropLog.ComQ1=" & aryparmkey(0) & " AND RTLessorCmtyLineDropLog.LINEQ1=" & aryparmkey(1) & " and RTLessorCmtyLineDropLog.entryno=" & aryparmkey(2) & " and " & SEARCHQRY & " ORDER BY RTLessorCmtyLineDropLog.seq" 
                      

  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>