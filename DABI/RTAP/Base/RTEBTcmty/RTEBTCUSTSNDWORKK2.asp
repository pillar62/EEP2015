<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="東森AVS管理系統"
  title="AVS用戶竣工確認單資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=" 列 印 ;完工結案;未完工結案;結案返轉; 作 廢 ;作廢返轉;設備明細;歷史異動"
  functionOptProgram="\RTAP\BASE\RTEBTCMTY\RTEBTCUSTSNDWORKPV.asp;rtebtCUSTsndworkF.asp;rtebtCUSTsndworkUF.asp;rtebtCUSTsndworkFR.asp;rtebtCUSTsndworkdrop.asp;rtebtCUSTsndworkdropc.asp;rtEBTCUSThardwareK.asp;rtEBTCUSTsndworkLOGK.asp"
  functionOptPrompt="N;Y;N;N;N;N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;none;主線;竣工單號;派工日期;列印人員;預定施工員;實際施工員;結案日;獎金結算月;獎金審核日;庫存結算月;庫存審核日;作廢日"
  sqlDelete="SELECT RTEBTCUSTSNDWORK.COMQ1, RTEBTCUSTSNDWORK.LINEQ1, RTEBTCUSTSNDWORK.CUSID, RTEBTCUSTSNDWORK.PRTNO,rtrim(convert(char(6),RTEBTCUSTSNDWORK.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTCUSTSNDWORK.lineQ1))  as comqline, RTEBTCUSTSNDWORK.PRTNO, RTEBTCUSTSNDWORK.SENDWORKDAT, " _
           &"RTOBJ.CUSNC,CASE WHEN RTOBJ_2.SHORTNC <>'' THEN RTOBJ_2.SHORTNC ELSE RTOBJ_1.CUSNC END,CASE WHEN RTOBJ_4.SHORTNC <>'' THEN RTOBJ_4.SHORTNC ELSE RTOBJ_3.CUSNC END, " _
           &"case when RTEBTCUSTSNDWORK.closedat is not null then  RTEBTCUSTSNDWORK.closedat else RTEBTCUSTSNDWORK.unclosedat end ,RTEBTCUSTSNDWORK.BONUSCLOSEYM, RTEBTCUSTSNDWORK.BONUSFINCHK, RTEBTCUSTSNDWORK.STOCKCLOSEYM, RTEBTCUSTSNDWORK.STOCKFINCHK, " _
           &"RTEBTCUSTSNDWORK.DROPDAT FROM RTEBTCUSTSNDWORK LEFT OUTER JOIN RTObj RTObj_4 ON RTEBTCUSTSNDWORK.REALCONSIGNEE = RTObj_4.CUSID LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_2 INNER JOIN RTObj RTObj_3 ON RTEmployee_2.CUSID = RTObj_3.CUSID ON RTEBTCUSTSNDWORK.REALENGINEER = RTEmployee_2.EMPLY LEFT OUTER JOIN " _
           &"RTObj RTObj_2 ON RTEBTCUSTSNDWORK.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN " _
           &"RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON RTEBTCUSTSNDWORK.ASSIGNENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
           &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON RTEBTCUSTSNDWORK.PRTUSR = RTEmployee.EMPLY "
  dataTable="RTEBTCUSTSNDWORK"
  userDefineDelete="Yes"
  numberOfKey=4
  dataProg="RTEBTCUSTSNDWORKD.asp"
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
  sqlYY="select * from RTEBTCMTYH LEFT OUTER JOIN RTCOUNTY ON RTEBTCMTYH.CUTID=RTCOUNTY.CUTID where COMQ1=" & ARYPARMKEY(0)
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
  else
     COMN=""
  end if
  rsYY.Close
  sqlYY="select * from RTEBTCMTYline LEFT OUTER JOIN RTCOUNTY ON RTEBTCMTYline.CUTID=RTCOUNTY.CUTID where COMQ1=" & ARYPARMKEY(0) & " and lineq1=" & aryparmkey(1)
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
     searchQry=" RTEBTCUSTSNDWORK.ComQ1=" & aryparmkey(0) & " and RTEBTCUSTSNDWORK.lineq1=" & aryparmkey(1) & " and RTEBTCUSTSNDWORK.cusid='" & aryparmkey(2) & "' "
     searchShow="主線序號︰"& aryparmkey(0) &"-" & aryparmkey(1)& ",社區名稱︰" & COMN & ",合約編號︰" & aryparmkey(2) & ",主線位址︰" & COMADDR
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
  
    If searchShow="全部" Then
         sqlList="SELECT RTEBTCUSTSNDWORK.COMQ1, RTEBTCUSTSNDWORK.LINEQ1, RTEBTCUSTSNDWORK.CUSID, RTEBTCUSTSNDWORK.PRTNO,rtrim(convert(char(6),RTEBTCUSTSNDWORK.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTCUSTSNDWORK.lineQ1))  as comqline, RTEBTCUSTSNDWORK.PRTNO, RTEBTCUSTSNDWORK.SENDWORKDAT, " _
           &"RTOBJ.CUSNC,CASE WHEN RTOBJ_2.SHORTNC <>'' THEN RTOBJ_2.SHORTNC ELSE RTOBJ_1.CUSNC END,CASE WHEN RTOBJ_4.SHORTNC <>'' THEN RTOBJ_4.SHORTNC ELSE RTOBJ_3.CUSNC END, " _
           &"case when RTEBTCUSTSNDWORK.closedat is not null then  RTEBTCUSTSNDWORK.closedat else RTEBTCUSTSNDWORK.unclosedat end ,RTEBTCUSTSNDWORK.BONUSCLOSEYM, RTEBTCUSTSNDWORK.BONUSFINCHK, RTEBTCUSTSNDWORK.STOCKCLOSEYM, RTEBTCUSTSNDWORK.STOCKFINCHK, " _
           &"RTEBTCUSTSNDWORK.DROPDAT FROM RTEBTCUSTSNDWORK LEFT OUTER JOIN RTObj RTObj_4 ON RTEBTCUSTSNDWORK.REALCONSIGNEE = RTObj_4.CUSID LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_2 INNER JOIN RTObj RTObj_3 ON RTEmployee_2.CUSID = RTObj_3.CUSID ON RTEBTCUSTSNDWORK.REALENGINEER = RTEmployee_2.EMPLY LEFT OUTER JOIN " _
           &"RTObj RTObj_2 ON RTEBTCUSTSNDWORK.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN " _
           &"RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON RTEBTCUSTSNDWORK.ASSIGNENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
           &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON RTEBTCUSTSNDWORK.PRTUSR = RTEmployee.EMPLY " _
            &"where " & searchqry
    Else
         sqlList="SELECT RTEBTCUSTSNDWORK.COMQ1, RTEBTCUSTSNDWORK.LINEQ1, RTEBTCUSTSNDWORK.CUSID, RTEBTCUSTSNDWORK.PRTNO,rtrim(convert(char(6),RTEBTCUSTSNDWORK.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTCUSTSNDWORK.lineQ1))  as comqline, RTEBTCUSTSNDWORK.PRTNO, RTEBTCUSTSNDWORK.SENDWORKDAT, " _
           &"RTOBJ.CUSNC,CASE WHEN RTOBJ_2.SHORTNC <>'' THEN RTOBJ_2.SHORTNC ELSE RTOBJ_1.CUSNC END,CASE WHEN RTOBJ_4.SHORTNC <>'' THEN RTOBJ_4.SHORTNC ELSE RTOBJ_3.CUSNC END, " _
           &"case when RTEBTCUSTSNDWORK.closedat is not null then  RTEBTCUSTSNDWORK.closedat else RTEBTCUSTSNDWORK.unclosedat end ,RTEBTCUSTSNDWORK.BONUSCLOSEYM, RTEBTCUSTSNDWORK.BONUSFINCHK, RTEBTCUSTSNDWORK.STOCKCLOSEYM, RTEBTCUSTSNDWORK.STOCKFINCHK, " _
           &"RTEBTCUSTSNDWORK.DROPDAT FROM RTEBTCUSTSNDWORK LEFT OUTER JOIN RTObj RTObj_4 ON RTEBTCUSTSNDWORK.REALCONSIGNEE = RTObj_4.CUSID LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_2 INNER JOIN RTObj RTObj_3 ON RTEmployee_2.CUSID = RTObj_3.CUSID ON RTEBTCUSTSNDWORK.REALENGINEER = RTEmployee_2.EMPLY LEFT OUTER JOIN " _
           &"RTObj RTObj_2 ON RTEBTCUSTSNDWORK.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN " _
           &"RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON RTEBTCUSTSNDWORK.ASSIGNENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
           &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON RTEBTCUSTSNDWORK.PRTUSR = RTEmployee.EMPLY " _
           &"where " & searchqry
    End If  
  'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>