<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="ET-City管理系統"
  title="ET-City主線撤線資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  functionOptName="轉拆機單;派工查詢;撤線結案;作　　廢;作廢返轉;異動查詢"
  functionOptProgram="RTLessorCmtyLineDropTRNSNDWORK.asp;RTLessorCmtyLineDropSNDWORKK.asp;RTLessorCmtyLineDropClose.asp;RTLessorCmtyLineDropCancel.asp;RTLessorCmtyLineDropCancelRTN.asp;RTLessorCmtyLineDropLogK.asp"
  functionOptPrompt="Y;N;Y;Y;Y;N"
  functionoptopen="1;1;1;1;1;1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;項次;主線;社區名稱;撤線種類;撤線申請日;預定撤線日;撤線原因;拆機單號;派工日期;拆機結案日;撤線結案日;作廢日;作廢員"
  sqlDelete="SELECT    RTLessorCmtyLineDrop.COMQ1, RTLessorCmtyLineDrop.LINEQ1, " _
                &"          RTLessorCmtyLineDrop.ENTRYNO,rtrim(convert(char(6),RTLessorCmtyLineDrop.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorCmtyLineDrop.lineQ1))," _
                &"          rtlessorcmtyh.comn, RTCode.CODENC, RTLessorCmtyLineDrop.DROPAPPLYDAT,  " _
                &"          RTLessorCmtyLineDrop.SCHDROPDAT,  " _
                &"          RTLessorCmtyLineDrop.DROPREASON,RTLessorCmtyLineDrop.SNDPRTNO,RTLessorCmtyLineDrop.SNDWORKDAT," _
                &"          RTLessorCmtyLineDrop.SNDCLOSEDAT,RTLessorCmtyLineDrop.CLOSEDAT,  " _
                &"          RTLessorCmtyLineDrop.CANCELDAT, RTObj_1.CUSNC " _
                &"FROM      RTEmployee RTEmployee_1 LEFT OUTER JOIN " _
                &"          RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID RIGHT OUTER JOIN " _
                &"          RTLessorCmtyLineDrop ON  " _
                &"          RTEmployee_1.EMPLY = RTLessorCmtyLineDrop.CANCELUSR LEFT OUTER JOIN " _
                &"          RTObj RIGHT OUTER JOIN " _
                &"          RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON  " _
                &"          RTLessorCmtyLineDrop.CLOSEUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
                &"          RTCode ON RTLessorCmtyLineDrop.DROPKIND = RTCode.CODE AND  " _
                &"          RTCode.KIND = 'N9' " _
                &"WHERE RTLessorCmtyLinedrop.ComQ1=0 " 

  dataTable="RTLessorCmtyLinedrop"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="RTLessorCmtyLinedropD.asp"
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
  searchProg="RTLessorCmtyLinedrops.asp"
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
     searchQry=" RTLessorCmtyLinedrop.ComQ1=" & aryparmkey(0) & " AND RTLessorCmtyLinedrop.LINEQ1=" & aryparmkey(1) & " and RTLessorCmtyLinedrop.canceldat is null "
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
         sqlList="SELECT    RTLessorCmtyLineDrop.COMQ1, RTLessorCmtyLineDrop.LINEQ1, " _
                &"          RTLessorCmtyLineDrop.ENTRYNO,rtrim(convert(char(6),RTLessorCmtyLineDrop.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorCmtyLineDrop.lineQ1)),rtlessorcmtyh.comn, RTCode.CODENC, RTLessorCmtyLineDrop.DROPAPPLYDAT,  " _
                &"          RTLessorCmtyLineDrop.SCHDROPDAT,  " _
                &"          RTLessorCmtyLineDrop.DROPREASON,RTLessorCmtyLineDrop.SNDPRTNO,RTLessorCmtyLineDrop.SNDWORKDAT," _
                &"          RTLessorCmtyLineDrop.SNDCLOSEDAT,RTLessorCmtyLineDrop.CLOSEDAT,  " _
                &"          RTLessorCmtyLineDrop.CANCELDAT, RTObj_1.CUSNC " _
                &"FROM      RTEmployee RTEmployee_1 LEFT OUTER JOIN " _
                &"          RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID RIGHT OUTER JOIN " _
                &"          RTLessorCmtyLineDrop ON  " _
                &"          RTEmployee_1.EMPLY = RTLessorCmtyLineDrop.CANCELUSR LEFT OUTER JOIN " _
                &"          RTObj RIGHT OUTER JOIN " _
                &"          RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON  " _
                &"          RTLessorCmtyLineDrop.CLOSEUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
                &"          RTCode ON RTLessorCmtyLineDrop.DROPKIND = RTCode.CODE AND  " _
                &"          RTCode.KIND = 'N9' left outer join rtlessorcmtyh on RTLessorCmtyLineDrop.comq1=rtlessorcmtyh.comq1 " _
                &"WHERE RTLessorCmtyLinedrop.ComQ1=" & aryparmkey(0) & " AND RTLessorCmtyLinedrop.LINEQ1=" & aryparmkey(1) & " and " & SEARCHQRY & " ORDER BY ENTRYNO" 
                      

 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>