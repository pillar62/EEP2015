<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="東森AVS管理系統"
  title="東森AVS用戶資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  V(1)="Y"
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="客訴處理;附加服務;繳款記錄;歷史異動"
  functionOptProgram="RTFaqK.ASP;rtebtcustEXTK.asp;rtEBTCUSTPAYK.asp;rtEBTcmtylineLOGK.asp"
  functionOptPrompt="N;N;N;N"
  functionoptopen="1;1;2;1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;用戶;合約編號;裝機地址;連絡電話;none;申請日;申請轉檔;EBT審核;完工日;報竣日;轉檔日;退租日;M2M3"
  sqlDelete="SELECT RTEBTCUST.COMQ1, RTEBTCUST.LINEQ1,RTEBTCUST.CUSID,LEFT(RTEBTCUST.CUSNC,4), right(RTEBTCUST.AVSNO,11), " _
           &"(RTCounty.CUTNC + RTEBTCUST.TOWNSHIP1 + CASE WHEN RTEBTCUST.VILLAGE1 " _
           &"<> '' THEN RTEBTCUST.VILLAGE1 + RTEBTCUST.COD11 ELSE '' END + CASE WHEN " _
           &"RTEBTCUST.NEIGHBOR1 <> '' THEN RTEBTCUST.NEIGHBOR1 + RTEBTCUST.COD12 " _
           &"ELSE '' END + CASE WHEN RTEBTCUST.STREET1 <> '' THEN RTEBTCUST.STREET1 " _
           &"+ RTEBTCUST.COD13 ELSE '' END + CASE WHEN RTEBTCUST.SEC1 <> '' THEN " _
           &"RTEBTCUST.SEC1 + RTEBTCUST.COD14 ELSE '' END + CASE WHEN RTEBTCUST.LANE1 " _
           &"<> '' THEN RTEBTCUST.LANE1 + RTEBTCUST.COD15 ELSE '' END + CASE WHEN " _
           &"RTEBTCUST.ALLEYWAY1 <> '' THEN RTEBTCUST.ALLEYWAY1 + RTEBTCUST.COD16 " _
           &"ELSE '' END + CASE WHEN RTEBTCUST.NUM1 <> '' THEN RTEBTCUST.NUM1 + " _
           &"RTEBTCUST.COD17 ELSE '' END + CASE WHEN RTEBTCUST.FLOOR1 <> '' THEN " _
           &"RTEBTCUST.FLOOR1 + RTEBTCUST.COD18 ELSE '' END + CASE WHEN RTEBTCUST.ROOM1 " _
           &"<> '' THEN RTEBTCUST.ROOM1 + RTEBTCUST.COD19 ELSE '' END) AS raddr, " _
           &"RTEBTCUST.CONTACTTEL, RTCode_2.CODENC, " _
           &"RTEBTCUST.APPLYDAT, RTEBTCUST.APPLYTNSDAT, RTEBTCUST.APPLYAGREE, " _
           &"RTEBTCUST.FINISHDAT, RTEBTCUST.DOCKETDAT, RTEBTCUST.TRANSDAT " _
           &"FROM RTEBTCUST LEFT OUTER JOIN RTCode RTCode_1 ON RTEBTCUST.DIALERPAYTYPE = RTCode_1.CODE " _
           &"AND RTCode_1.KIND = 'G7' LEFT OUTER JOIN RTCode RTCode_2 ON RTEBTCUST.PAYTYPE = RTCode_2.CODE " _
           &"AND RTCode_2.KIND = 'G6' LEFT OUTER JOIN RTCounty ON RTEBTCUST.CUTID1 = RTCounty.CUTID "

  dataTable="rtebtcust"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="RTebtCustD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth="600"
  diaHeight="400"
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
     searchQry=" RTEBTCust.ComQ1=" & aryparmkey(0) & " and RTEBTCust.lineq1=" & aryparmkey(1) & " "
     searchShow="社區序號︰"& aryparmkey(0) & ",社區名稱︰" & COMN &",主線序號︰" & aryparmkey(1) & ",主線位址︰" & COMADDR
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
  
  '業務工程師只能讀取該工程師的社區
  'if userlevel=2 then
  '  If searchShow="全部" Then
  '  sqlList="SELECT RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN,t1no,netip, RTCounty.CUTNC, RTCmty.COMCNT, " _
  '       &"Sum( CASE custype  when '申裝戶'  THEN 1 ELSE 0 end),  " _
  '       &"Sum(CASE custype  when '深耕戶'  THEN 1 ELSE 0 end) , " _ 
  '       &"Sum(CASE custype  when ''  THEN 1 ELSE 0 end), " _
  '       &"Sum(CASE when DROPDAT is Null  THEN 0 ELSE 1 END ), " _                    
  '       &"Sum(CASE when DROPDAT is Null and rtcust.cusid is not null THEN 1 ELSE 0 END), " _            
  '       &"case when RTCmty.COMCNT = 0 then 0 else Sum(CASE when DROPDAT is Null and rtcust.cusid is not null  THEN 1 ELSE 0 END) * 100 / (RTCmty.COMCNT*1.0)  end , "  _                    
  '       &"Sum(CASE when FINISHDAT is Null and dropdat is null  and rtcust.cusid is not null THEN 1 ELSE 0 END), " _                    
  '       &"Sum(CASE when FINISHDAT is not Null and dropdat is null THEN 1 ELSE 0 END), " _                    
  '       &"RTcmty.T1PETITION,RTcmty.T1Apply  " _
  '       &"FROM RTEmployee INNER JOIN " _
  '       &"##RTCmtyGroup ON RTEmployee.CUSID = ##RTCmtyGroup.CUSID INNER JOIN " _
  '       &"RTCounty INNER JOIN " _
  '       &"RTCust RIGHT OUTER JOIN " _               
  '       &"RTCmty ON RTCUST.COMQ1 = RTCMTY.COMQ1 ON RTCounty.CUTID = RTCmty.CUTID INNER JOIN " _
  '       &"RTArea INNER JOIN " _
  '       &"RTAreaCty ON RTArea.AREAID = RTAreaCty.AREAID and rtarea.areaid" & DareaID & " ON " _
  '       &"RTCmty.CUTID = RTAreaCty.CUTID ON ##RTCmtyGroup.COMQ1 = RTCmty.COMQ1 " _
  '       &"WHERE RTArea.AREATYPE='1' AND " &searchQry &" " _         
  '       &"group by RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN,t1no,netip, RTCounty.CUTNC, " _
  '       &"RTCmty.COMCNT, RTCmty.APPLYCNT, RTCmty.T1PETITION, RTCmty.SCHDAT, " _
  '       &"RTCmty.T1APPLY " _
  '       &"ORDER BY RTCmty.COMN "
  '  Else
  '  sqlList="SELECT RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN, t1no,netip,RTCounty.CUTNC, RTCmty.COMCNT, " _
  '       &"sum( CASE custype  when '申裝戶'  THEN 1 ELSE 0 end) ,  " _
  '       &"sum(CASE custype  when '深耕戶'  THEN 1 ELSE 0 end) , " _ 
  '       &"Sum(CASE custype  when ''  THEN 1 ELSE 0 end), " _
  '       &"Sum(CASE when DROPDAT is Null  THEN 0 ELSE 1 END ), " _                    
  '       &"Sum(CASE when DROPDAT is Null and rtcust.cusid is not null THEN 1 ELSE 0 END), " _            
  '       &"case when RTCmty.COMCNT = 0 then 0 else Sum(CASE when DROPDAT is Null and rtcust.cusid is not null  THEN 1 ELSE 0 END) * 100 / (RTCmty.COMCNT*1.0)  end , "  _                    
  '       &"Sum(CASE when FINISHDAT is Null and dropdat is null  and rtcust.cusid is not null THEN 1 ELSE 0 END), " _                    
  '       &"Sum(CASE when FINISHDAT is not Null and dropdat is null THEN 1 ELSE 0 END), " _                    
  '       &"RTcmty.T1PETITION,RTcmty.T1Apply  " _
  '       &"FROM RTCounty INNER JOIN " _
  '       &"##RTCmtyGroup INNER JOIN " _
  '       &"RTCust RIGHT OUTER JOIN " _         
  '       &"RTCmty ON RTCUST.COMQ1 = RTCMTY.COMQ1 ON ##RTCmtyGroup.COMQ1 = RTCmty.COMQ1 ON " _
  '       &"RTCounty.CUTID = RTCmty.CUTID INNER JOIN " _
  '       &"RTArea INNER JOIN " _
  '       &"RTAreaCty ON RTArea.AREAID = RTAreaCty.AREAID and rtarea.areaid" & DareaID & " ON " _
  '       &"RTCmty.CUTID = RTAreaCty.CUTID INNER JOIN " _
  '       &"RTEmployee ON ##RTCmtyGroup.CUSID = RTEmployee.CUSID " _
  '       &"WHERE RTArea.AREATYPE='1' and " &searchQry & " "  _
  '       &"group by RTCmty.COMQ1, RTCmty.COMQ2, RTCmty.COMN,t1no,netip, RTCounty.CUTNC, " _
  '       &"RTCmty.COMCNT, RTCmty.APPLYCNT, RTCmty.T1PETITION, RTCmty.SCHDAT, " _
  '       &"RTCmty.T1APPLY " _                  
  '       &"ORDER BY RTCmty.COMN "
  '  End If
  '業務助理or客服人員
  'else
    If searchShow="全部" Then
         sqlList="SELECT RTEBTCUST.COMQ1, RTEBTCUST.LINEQ1,RTEBTCUST.CUSID,LEFT(RTEBTCUST.CUSnc,4), right(RTEBTCUST.AVSNO,11),  " _
           &"(RTCounty.CUTNC + RTEBTCUST.TOWNSHIP1 + CASE WHEN RTEBTCUST.VILLAGE1 " _
           &"<> '' THEN RTEBTCUST.VILLAGE1 + RTEBTCUST.COD11 ELSE '' END + CASE WHEN " _
           &"RTEBTCUST.NEIGHBOR1 <> '' THEN RTEBTCUST.NEIGHBOR1 + RTEBTCUST.COD12 " _
           &"ELSE '' END + CASE WHEN RTEBTCUST.STREET1 <> '' THEN RTEBTCUST.STREET1 " _
           &"+ RTEBTCUST.COD13 ELSE '' END + CASE WHEN RTEBTCUST.SEC1 <> '' THEN " _
           &"RTEBTCUST.SEC1 + RTEBTCUST.COD14 ELSE '' END + CASE WHEN RTEBTCUST.LANE1 " _
           &"<> '' THEN RTEBTCUST.LANE1 + RTEBTCUST.COD15 ELSE '' END + CASE WHEN " _
           &"RTEBTCUST.ALLEYWAY1 <> '' THEN RTEBTCUST.ALLEYWAY1 + RTEBTCUST.COD16 " _
           &"ELSE '' END + CASE WHEN RTEBTCUST.NUM1 <> '' THEN RTEBTCUST.NUM1 + " _
           &"RTEBTCUST.COD17 ELSE '' END + CASE WHEN RTEBTCUST.FLOOR1 <> '' THEN " _
           &"RTEBTCUST.FLOOR1 + RTEBTCUST.COD18 ELSE '' END + CASE WHEN RTEBTCUST.ROOM1 " _
           &"<> '' THEN RTEBTCUST.ROOM1 + RTEBTCUST.COD19 ELSE '' END) AS raddr, " _
           &"RTEBTCUST.CONTACTTEL, RTCode_2.CODENC, " _
           &"RTEBTCUST.APPLYDAT, RTEBTCUST.APPLYTNSDAT, RTEBTCUST.APPLYAGREE, " _
           &"RTEBTCUST.FINISHDAT, RTEBTCUST.DOCKETDAT, RTEBTCUST.TRANSDAT, RTEBTCUST.dropDAT, " _
           &"CASE WHEN RTEBTCUST.OVERDUE='Y' AND RTEBTCUST.DROPDAT IS NOT NULL THEN 'M3' WHEN RTEBTCUST.OVERDUE='Y' AND RTEBTCUST.DROPDAT IS NULL THEN 'M2' ELSE '' END " _
           &"FROM RTEBTCUST LEFT OUTER JOIN RTCode RTCode_1 ON RTEBTCUST.DIALERPAYTYPE = RTCode_1.CODE " _
           &"AND RTCode_1.KIND = 'G7' LEFT OUTER JOIN RTCode RTCode_2 ON RTEBTCUST.PAYTYPE = RTCode_2.CODE " _
           &"AND RTCode_2.KIND = 'G6' LEFT OUTER JOIN RTCounty ON RTEBTCUST.CUTID1 = RTCounty.CUTID " _
           &"where " & searchqry
    Else
         sqlList="SELECT RTEBTCUST.COMQ1, RTEBTCUST.LINEQ1,RTEBTCUST.CUSID,LEFT(RTEBTCUST.CUSnc,4), right(RTEBTCUST.AVSNO,11),  " _
           &"(RTCounty.CUTNC + RTEBTCUST.TOWNSHIP1 + CASE WHEN RTEBTCUST.VILLAGE1 " _
           &"<> '' THEN RTEBTCUST.VILLAGE1 + RTEBTCUST.COD11 ELSE '' END + CASE WHEN " _
           &"RTEBTCUST.NEIGHBOR1 <> '' THEN RTEBTCUST.NEIGHBOR1 + RTEBTCUST.COD12 " _
           &"ELSE '' END + CASE WHEN RTEBTCUST.STREET1 <> '' THEN RTEBTCUST.STREET1 " _
           &"+ RTEBTCUST.COD13 ELSE '' END + CASE WHEN RTEBTCUST.SEC1 <> '' THEN " _
           &"RTEBTCUST.SEC1 + RTEBTCUST.COD14 ELSE '' END + CASE WHEN RTEBTCUST.LANE1 " _
           &"<> '' THEN RTEBTCUST.LANE1 + RTEBTCUST.COD15 ELSE '' END + CASE WHEN " _
           &"RTEBTCUST.ALLEYWAY1 <> '' THEN RTEBTCUST.ALLEYWAY1 + RTEBTCUST.COD16 " _
           &"ELSE '' END + CASE WHEN RTEBTCUST.NUM1 <> '' THEN RTEBTCUST.NUM1 + " _
           &"RTEBTCUST.COD17 ELSE '' END + CASE WHEN RTEBTCUST.FLOOR1 <> '' THEN " _
           &"RTEBTCUST.FLOOR1 + RTEBTCUST.COD18 ELSE '' END + CASE WHEN RTEBTCUST.ROOM1 " _
           &"<> '' THEN RTEBTCUST.ROOM1 + RTEBTCUST.COD19 ELSE '' END) AS raddr, " _
           &"RTEBTCUST.CONTACTTEL, RTCode_2.CODENC, " _
           &"RTEBTCUST.APPLYDAT, RTEBTCUST.APPLYTNSDAT, RTEBTCUST.APPLYAGREE, " _
           &"RTEBTCUST.FINISHDAT, RTEBTCUST.DOCKETDAT, RTEBTCUST.TRANSDAT, RTEBTCUST.dropDAT, " _
           &"CASE WHEN RTEBTCUST.OVERDUE='Y' AND RTEBTCUST.DROPDAT IS NOT NULL THEN 'M3' WHEN RTEBTCUST.OVERDUE='Y' AND RTEBTCUST.DROPDAT IS NULL THEN 'M2' ELSE '' END " _
           &"FROM RTEBTCUST LEFT OUTER JOIN RTCode RTCode_1 ON RTEBTCUST.DIALERPAYTYPE = RTCode_1.CODE " _
           &"AND RTCode_1.KIND = 'G7' LEFT OUTER JOIN RTCode RTCode_2 ON RTEBTCUST.PAYTYPE = RTCode_2.CODE " _
           &"AND RTCode_2.KIND = 'G6' LEFT OUTER JOIN RTCounty ON RTEBTCUST.CUTID1 = RTCounty.CUTID " _
           &"where " & searchqry
    End If  
  'end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>