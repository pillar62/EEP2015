<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="東森行動電話管理系統"
  title="行動電話用戶資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=" 作 廢 ;作廢返轉"
  functionOptProgram="EBT3GCUSTCANCEL.asp;EBT3GCUSTCANCELRTN.asp"
  functionOptPrompt="Y;Y"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="用戶代號;用戶名稱;用戶類別;none;戶籍/營業地址;公司電話;公司負責人;負責人電話1;負責人電話2;用戶申請日;申請送件日;申請轉檔日;作廢日"
  sqlDelete="SELECT EBT3GCUST.CUSID, CASE WHEN EBT3GCUST.CUSNC ='' THEN EBT3GCUST.CUSNE ELSE EBT3GCUST.CUSNC END, " _
           &"RTCode.CODENC,EBT3GCUST.CUSBIRTHDAY, RTCounty.CUTNC + EBT3GCUST.TOWNSHIP1 + CASE WHEN EBT3GCUST.VILLAGE1 " _
           &"<> '' THEN EBT3GCUST.VILLAGE1 + EBT3GCUST.COD11 ELSE '' END + CASE WHEN EBT3GCUST.NEIGHBOR1 <> '' THEN " _
           &"EBT3GCUST.NEIGHBOR1 + EBT3GCUST.COD12 ELSE '' END + CASE WHEN EBT3GCUST.STREET1 <> '' THEN EBT3GCUST.STREET1 " _
           &"+ EBT3GCUST.COD13 ELSE '' END + CASE WHEN EBT3GCUST.SEC1 <> '' THEN EBT3GCUST.SEC1 + EBT3GCUST.COD14 ELSE '' END " _
           &"+ CASE WHEN EBT3GCUST.LANE1 <> '' THEN EBT3GCUST.LANE1 + EBT3GCUST.COD15 ELSE '' END + CASE WHEN " _
           &"EBT3GCUST.ALLEYWAY1 <> '' THEN EBT3GCUST.ALLEYWAY1 + EBT3GCUST.COD16 ELSE '' END + CASE WHEN EBT3GCUST.NUM1 <> '' " _
           &"THEN EBT3GCUST.NUM1 + EBT3GCUST.COD17 ELSE '' END + CASE WHEN EBT3GCUST.FLOOR1 <> '' THEN EBT3GCUST.FLOOR1 " _
           &"+ EBT3GCUST.COD18 ELSE '' END + CASE WHEN EBT3GCUST.ROOM1 <> '' THEN EBT3GCUST.ROOM1 + EBT3GCUST.COD19 ELSE '' END " _
           &"AS raddr,CASE WHEN EBT3GCUST.COTEL11 <> '' THEN EBT3GCUST.COTEL11 + '-' ELSE '' END + EBT3GCUST.COTEL12 + " _
           &"CASE WHEN EBT3GCUST.COTEL13 <> '' THEN '#' + EBT3GCUST.COTEL13 ELSE '' END AS COTEL, EBT3GCUST.COBOSS," _
           &"CASE WHEN EBT3GCUST.BOSSHOMETEL11 <> '' THEN EBT3GCUST.BOSSHOMETEL11 + '-' ELSE '' END + EBT3GCUST.BOSSHOMETEL12 " _
           &"AS BOSSTEL,EBT3GCUST.BOSSOTHERTEL2,EBT3GCUST.APFORMDAT, EBT3GCUST.APPLYDAT, EBT3GCUST.TRANSDAT, EBT3GCUST.CANCELDAT  " _
           &"FROM EBT3GCUST LEFT OUTER JOIN RTCounty ON EBT3GCUST.CUTID1 = RTCounty.CUTID LEFT OUTER JOIN " _
           &"RTCode ON EBT3GCUST.CUSTTYPE = RTCode.CODE AND RTCode.KIND = 'K2' "

  dataTable="EBT3Gcust"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="EBT3GCustD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=TRUE
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="EBT3GCUSTS.ASP"
  '----
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" EBT3GCUST.CUSID <> '' "
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
            DAreaID="='A1'"
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
  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or  Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="T90076" OR _
     Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T89076"then
     DAreaID="<>'*'"
  end if
  '資訊部管理員可讀取全部資料
  'if userlevel=31 then DAreaID="<>'*'"
  
  '由於分公司搬家尚未申請到線路，故客服先開放所有區域權限，一律讓台北客服處理
  if userlevel=31 or userlevel =1  or userlevel =5 then DAreaID="<>'*'"
  
         sqlList="SELECT EBT3GCUST.CUSID, CASE WHEN EBT3GCUST.CUSNC ='' THEN EBT3GCUST.CUSNE ELSE EBT3GCUST.CUSNC END, " _
           &"RTCode.CODENC,EBT3GCUST.CUSBIRTHDAY, RTCounty.CUTNC + EBT3GCUST.TOWNSHIP1 + CASE WHEN EBT3GCUST.VILLAGE1 " _
           &"<> '' THEN EBT3GCUST.VILLAGE1 + EBT3GCUST.COD11 ELSE '' END + CASE WHEN EBT3GCUST.NEIGHBOR1 <> '' THEN " _
           &"EBT3GCUST.NEIGHBOR1 + EBT3GCUST.COD12 ELSE '' END + CASE WHEN EBT3GCUST.STREET1 <> '' THEN EBT3GCUST.STREET1 " _
           &"+ EBT3GCUST.COD13 ELSE '' END + CASE WHEN EBT3GCUST.SEC1 <> '' THEN EBT3GCUST.SEC1 + EBT3GCUST.COD14 ELSE '' END " _
           &"+ CASE WHEN EBT3GCUST.LANE1 <> '' THEN EBT3GCUST.LANE1 + EBT3GCUST.COD15 ELSE '' END + CASE WHEN " _
           &"EBT3GCUST.ALLEYWAY1 <> '' THEN EBT3GCUST.ALLEYWAY1 + EBT3GCUST.COD16 ELSE '' END + CASE WHEN EBT3GCUST.NUM1 <> '' " _
           &"THEN EBT3GCUST.NUM1 + EBT3GCUST.COD17 ELSE '' END + CASE WHEN EBT3GCUST.FLOOR1 <> '' THEN EBT3GCUST.FLOOR1 " _
           &"+ EBT3GCUST.COD18 ELSE '' END + CASE WHEN EBT3GCUST.ROOM1 <> '' THEN EBT3GCUST.ROOM1 + EBT3GCUST.COD19 ELSE '' END " _
           &"AS raddr,CASE WHEN EBT3GCUST.COTEL11 <> '' THEN EBT3GCUST.COTEL11 + '-' ELSE '' END + EBT3GCUST.COTEL12 + " _
           &"CASE WHEN EBT3GCUST.COTEL13 <> '' THEN '#' + EBT3GCUST.COTEL13 ELSE '' END AS COTEL, EBT3GCUST.BOSSSOCIALID," _
           &"CASE WHEN EBT3GCUST.BOSSHOMETEL11 <> '' THEN EBT3GCUST.BOSSHOMETEL11 + '-' ELSE '' END + EBT3GCUST.BOSSHOMETEL12 " _
           &"AS BOSSTEL,EBT3GCUST.BOSSOTHERTEL2,EBT3GCUST.APFORMDAT, EBT3GCUST.APPLYDAT, EBT3GCUST.TRANSDAT, EBT3GCUST.CANCELDAT " _
           &"FROM EBT3GCUST LEFT OUTER JOIN RTCounty ON EBT3GCUST.CUTID1 = RTCounty.CUTID LEFT OUTER JOIN " _
           &"RTCode ON EBT3GCUST.CUSTTYPE = RTCode.CODE AND RTCode.KIND = 'K2' " _
           &"where " & searchqry
 'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>