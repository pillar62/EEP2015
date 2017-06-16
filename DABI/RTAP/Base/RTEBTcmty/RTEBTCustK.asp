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
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="竣工單;附加服務;客訴案件; 作 廢 ;作廢返轉;報竣異動;  退  租  ;繳款記錄;歷史異動;清除主線調整"
  functionOptProgram="rtebtcustsndworkk2.asp;rtebtcustEXTK.asp;rtebtcustfaqK.asp;rtebtcustCANCEL.asp;rtebtcustCANCELRTN.asp;rtEBTCUSTCHGK.asp;rtEBTCUSTDROPK.asp;rtEBTCUSTPAYK.asp;rtEBTCUSTLOGK.asp;RTEBTCUSTLINEADJFLGCLR.ASP"
  functionOptPrompt="N;N;N;Y;Y;Y;N;N;N;Y"
  functionoptopen="1;1;1;1;1;1;1;2;1;1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;主線;主線名稱;用戶;AVS合約號;方案;裝機地址;連絡電話;none;申請日;none;none;完工日;報竣日;轉檔日;退租日;作廢日;移入;移出;M2M3;主線調整狀態"
  sqlDelete="SELECT RTEBTCUST.CUSID, CASE WHEN RTEBTCUST.CUSNC ='' THEN RTEBTCUST.CUSNE ELSE RTEBTCUST.CUSNC END, " _
           &"RTCode.CODENC,RTEBTCUST.CUSBIRTHDAY, RTCounty.CUTNC + RTEBTCUST.TOWNSHIP1 + CASE WHEN RTEBTCUST.VILLAGE1 " _
           &"<> '' THEN RTEBTCUST.VILLAGE1 + RTEBTCUST.COD11 ELSE '' END + CASE WHEN RTEBTCUST.NEIGHBOR1 <> '' THEN " _
           &"RTEBTCUST.NEIGHBOR1 + RTEBTCUST.COD12 ELSE '' END + CASE WHEN RTEBTCUST.STREET1 <> '' THEN RTEBTCUST.STREET1 " _
           &"+ RTEBTCUST.COD13 ELSE '' END + CASE WHEN RTEBTCUST.SEC1 <> '' THEN RTEBTCUST.SEC1 + RTEBTCUST.COD14 ELSE '' END " _
           &"+ CASE WHEN RTEBTCUST.LANE1 <> '' THEN RTEBTCUST.LANE1 + RTEBTCUST.COD15 ELSE '' END + CASE WHEN " _
           &"RTEBTCUST.ALLEYWAY1 <> '' THEN RTEBTCUST.ALLEYWAY1 + RTEBTCUST.COD16 ELSE '' END + CASE WHEN RTEBTCUST.NUM1 <> '' " _
           &"THEN RTEBTCUST.NUM1 + RTEBTCUST.COD17 ELSE '' END + CASE WHEN RTEBTCUST.FLOOR1 <> '' THEN RTEBTCUST.FLOOR1 " _
           &"+ RTEBTCUST.COD18 ELSE '' END + CASE WHEN RTEBTCUST.ROOM1 <> '' THEN RTEBTCUST.ROOM1 + RTEBTCUST.COD19 ELSE '' END " _
           &"AS raddr,CASE WHEN RTEBTCUST.COTEL11 <> '' THEN RTEBTCUST.COTEL11 + '-' ELSE '' END + RTEBTCUST.COTEL12 + " _
           &"CASE WHEN RTEBTCUST.COTEL13 <> '' THEN '#' + RTEBTCUST.COTEL13 ELSE '' END AS COTEL, RTEBTCUST.BOSSSOCIALID," _
           &"CASE WHEN RTEBTCUST.BOSSHOMETEL11 <> '' THEN RTEBTCUST.BOSSHOMETEL11 + '-' ELSE '' END + RTEBTCUST.BOSSHOMETEL12 " _
           &"AS BOSSTEL,RTEBTCUST.BOSSOTHERTEL2 " _
           &"FROM RTEBTCUST LEFT OUTER JOIN RTCounty ON RTEBTCUST.CUTID1 = RTCounty.CUTID LEFT OUTER JOIN " _
           &"RTCode ON RTEBTCUST.CUSTTYPE = RTCode.CODE AND RTCode.KIND = 'K2' "

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
     searchQry=" RTEBTCUST.COMQ1=" & ARYPARMKEY(0) & " AND RTEBTCUST.LINEQ1=" & ARYPARMKEY(1)
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
  
         sqlList="SELECT RTEBTCUST.COMQ1, RTEBTCUST.LINEQ1,RTEBTCUST.CUSID,rtrim(ltrim(convert(char(6),RTEBTCUST.COMQ1))) +'-'+ rtrim(ltrim(convert(char(6),RTEBTCUST.lineQ1))) as comqline,rtebtcmtyh.comn,LEFT(RTEBTCUST.CUSnc,4),  right(RTEBTCUST.AVSNO,11),RTCODE_3.CODENC,  " _
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
           &"RTEBTCUST.FINISHDAT, RTEBTCUST.DOCKETDAT, RTEBTCUST.TRANSDAT, RTEBTCUST.DROPDAT, RTEBTCUST.CANCELDAT, " _
           &"case when RTEBTCUST.MOVEFROMCOMQ1 > 0 then rtrim(ltrim(CONVERT(char(6), RTEBTCUST.MOVEFROMCOMQ1))) + '-' + rtrim(ltrim(CONVERT(char(6), RTEBTCUST.MOVEFROMlineQ1))) else '' end, " _
           &"case when RTEBTCUST.MOVETOCOMQ1 > 0 then rtrim(ltrim(CONVERT(char(6), RTEBTCUST.MOVETOCOMQ1))) + '-' + rtrim(ltrim(CONVERT(char(6), RTEBTCUST.MOVETOlineQ1))) else '' end,  " _
           &"CASE WHEN RTEBTCUST.OVERDUE='Y' AND RTEBTCUST.DROPDAT IS NOT NULL THEN 'M3' WHEN RTEBTCUST.OVERDUE='Y' AND RTEBTCUST.DROPDAT IS NULL THEN 'M2' ELSE '' END,RTCODE_4.CODENC " _
           &"FROM RTEBTCUST LEFT OUTER JOIN RTCode RTCode_1 ON RTEBTCUST.DIALERPAYTYPE = RTCode_1.CODE " _
           &"AND RTCode_1.KIND = 'G7' LEFT OUTER JOIN RTCode RTCode_2 ON RTEBTCUST.PAYTYPE = RTCode_2.CODE " _
           &"AND RTCode_2.KIND = 'G6' LEFT OUTER JOIN RTCounty ON RTEBTCUST.CUTID1 = RTCounty.CUTID  inner join rtebtcmtyh on " _
           &"rtebtcust.comq1=rtebtcmtyh.comq1 inner join rtebtcmtyline on rtebtcust.comq1=rtebtcmtyline.comq1 and " _
           &"rtebtcust.lineq1=rtebtcmtyline.lineq1 INNER JOIN RTAREATOWNSHIP ON RTEBTCMTYLINE.CUTID=RTAREATOWNSHIP.CUTID AND " _
           &"RTEBTCMTYLINE.TOWNSHIP=RTAREATOWNSHiP.TOWNSHIP " _ 
           &"LEFT OUTER JOIN RTCODE RTCODE_3 ON RTEBTCUST.CASETYPE = RTCODE_3.CODE AND RTCODE_3.KIND='H5' LEFT OUTER JOIN RTCODE RTCODE_4 ON RTEBTCUST.CUSTLINEADJFLG=RTCODE_4.CODE AND RTCODE_4.KIND='L2' " _
           &"where " & searchqry & " AND RTAREATOWNSHIP.AREAID " & DAREAID & " " _
           &"GROUP BY  RTEBTCUST.COMQ1, RTEBTCUST.LINEQ1,RTEBTCUST.CUSID,rtrim(ltrim(convert(char(6),RTEBTCUST.COMQ1))) +'-'+ rtrim(ltrim(convert(char(6),RTEBTCUST.lineQ1))),rtebtcmtyh.comn,LEFT(RTEBTCUST.CUSnc,4),  right(RTEBTCUST.AVSNO,11),RTCODE_3.CODENC,  " _
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
           &"<> '' THEN RTEBTCUST.ROOM1 + RTEBTCUST.COD19 ELSE '' END) , " _
           &"RTEBTCUST.CONTACTTEL, RTCode_2.CODENC, " _
           &"RTEBTCUST.APPLYDAT, RTEBTCUST.APPLYTNSDAT, RTEBTCUST.APPLYAGREE, " _
           &"RTEBTCUST.FINISHDAT, RTEBTCUST.DOCKETDAT, RTEBTCUST.TRANSDAT, RTEBTCUST.DROPDAT, RTEBTCUST.CANCELDAT, " _
           &"case when RTEBTCUST.MOVEFROMCOMQ1 > 0 then rtrim(ltrim(CONVERT(char(6), RTEBTCUST.MOVEFROMCOMQ1))) + '-' + rtrim(ltrim(CONVERT(char(6),RTEBTCUST.MOVEFROMlineQ1))) else '' end, " _
           &"case when RTEBTCUST.MOVETOCOMQ1 > 0 then rtrim(ltrim(CONVERT(char(6), RTEBTCUST.MOVETOCOMQ1))) + '-' + rtrim(ltrim(CONVERT(char(6),RTEBTCUST.MOVETOlineQ1))) else '' end,  " _
           &"CASE WHEN RTEBTCUST.OVERDUE = 'Y' AND RTEBTCUST.DROPDAT IS NOT NULL THEN 'M3' WHEN RTEBTCUST.OVERDUE = 'Y' AND RTEBTCUST.DROPDAT IS NULL THEN 'M2' ELSE '' END ,RTCODE_4.CODENC " _
           &"ORDER BY RTEBTCUST.COMQ1, RTEBTCUST.LINEQ1,LEFT(RTEBTCUST.CUSnc,4) " 
  'end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>