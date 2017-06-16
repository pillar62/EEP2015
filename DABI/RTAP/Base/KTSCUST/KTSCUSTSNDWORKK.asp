<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="KTS管理系統"
  title="KTS用戶裝機派工單資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=" 列 印 ;完工結案;未完工結案;結案返轉; 作 廢 ;作廢返轉;歷史異動"
  functionOptProgram="/RTap/Base/KTSCust/KTSCustSndWrkP.asp;KTSCUSTSNDWORKF.asp;KTSCUSTSNDWORKUF.asp;KTSCUSTSNDWORKFR.asp;KTSCUSTSNDWORKdrop.asp;KTSCUSTSNDWORKdropc.asp;KTSCUSTSNDWORKLOGK.asp"
  functionOptPrompt="N;Y;Y;Y;Y;Y;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;派工單號;派工日期;列印人員;預定施工員;實際施工員;完工結案日;未完工結案;作廢日;獎金結算月;獎金審核日;庫存結算月;庫存審核日"
  sqlDelete="SELECT KTSCUSTSNDWORK.CUSID, KTSCUSTSNDWORK.PRTNO, KTSCUSTSNDWORK.SENDWORKDAT, " _
           &"RTOBJ.CUSNC,CASE WHEN RTOBJ_2.SHORTNC <>'' THEN RTOBJ_2.SHORTNC ELSE RTOBJ_1.CUSNC END,CASE WHEN RTOBJ_4.SHORTNC <>'' THEN RTOBJ_4.SHORTNC ELSE RTOBJ_3.CUSNC END, " _
           &"KTSCUSTSNDWORK.CLOSEDAT,KTSCUSTSNDWORK.UNCLOSEDAT,KTSCUSTSNDWORK.DROPDAT,KTSCUSTSNDWORK.BONUSCLOSEYM, KTSCUSTSNDWORK.BONUSFINCHK, KTSCUSTSNDWORK.STOCKCLOSEYM, KTSCUSTSNDWORK.STOCKFINCHK " _
           &"FROM KTSCUSTSNDWORK LEFT OUTER JOIN RTObj RTObj_4 ON KTSCUSTSNDWORK.REALCONSIGNEE = RTObj_4.CUSID LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_2 INNER JOIN RTObj RTObj_3 ON RTEmployee_2.CUSID = RTObj_3.CUSID ON KTSCUSTSNDWORK.REALENGINEER = RTEmployee_2.EMPLY LEFT OUTER JOIN " _
           &"RTObj RTObj_2 ON KTSCUSTSNDWORK.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN " _
           &"RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON KTSCUSTSNDWORK.ASSIGNENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
           &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON KTSCUSTSNDWORK.PRTUSR = RTEmployee.EMPLY "
  dataTable="KTSCUSTSNDWORK"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="KTSCUSTSNDWORKD.asp"
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
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" KTSCUSTSNDWORK.CUSID='" & aryparmkey(0) & "' "
     searchShow="用戶編號︰"& aryparmkey(0) 
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
  if userlevel=31 then DAreaID="<>'*'"
  
    If searchShow="全部" Then
         sqlList="SELECT KTSCUSTSNDWORK.CUSID, KTSCUSTSNDWORK.PRTNO, KTSCUSTSNDWORK.SENDWORKDAT, " _
           &"RTOBJ.CUSNC,CASE WHEN KTSCUSTSNDWORK.ASSIGNCONSIGNEE <>'' THEN KTSCUSTSNDWORK.ASSIGNCONSIGNEE ELSE RTOBJ_1.CUSNC END,CASE WHEN KTSCUSTSNDWORK.REALCONSIGNEE <>'' THEN KTSCUSTSNDWORK.REALCONSIGNEE ELSE RTOBJ_3.CUSNC END, " _
           &"KTSCUSTSNDWORK.CLOSEDAT,KTSCUSTSNDWORK.UNCLOSEDAT,KTSCUSTSNDWORK.DROPDAT,KTSCUSTSNDWORK.BONUSCLOSEYM, KTSCUSTSNDWORK.BONUSFINCHK, KTSCUSTSNDWORK.STOCKCLOSEYM, KTSCUSTSNDWORK.STOCKFINCHK " _
           &"FROM KTSCUSTSNDWORK LEFT OUTER JOIN RTObj RTObj_4 ON KTSCUSTSNDWORK.REALCONSIGNEE = RTObj_4.CUSID LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_2 INNER JOIN RTObj RTObj_3 ON RTEmployee_2.CUSID = RTObj_3.CUSID ON KTSCUSTSNDWORK.REALENGINEER = RTEmployee_2.EMPLY LEFT OUTER JOIN " _
           &"RTObj RTObj_2 ON KTSCUSTSNDWORK.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN " _
           &"RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON KTSCUSTSNDWORK.ASSIGNENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
           &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON KTSCUSTSNDWORK.PRTUSR = RTEmployee.EMPLY " _
            &"where " & searchqry
    Else
         sqlList="SELECT KTSCUSTSNDWORK.CUSID, KTSCUSTSNDWORK.PRTNO, KTSCUSTSNDWORK.SENDWORKDAT, " _
           &"RTOBJ.CUSNC,CASE WHEN KTSCUSTSNDWORK.ASSIGNCONSIGNEE <>'' THEN KTSCUSTSNDWORK.ASSIGNCONSIGNEE ELSE RTOBJ_1.CUSNC END,CASE WHEN KTSCUSTSNDWORK.REALCONSIGNEE <>'' THEN KTSCUSTSNDWORK.REALCONSIGNEE ELSE RTOBJ_3.CUSNC END, " _
           &"KTSCUSTSNDWORK.CLOSEDAT,KTSCUSTSNDWORK.UNCLOSEDAT,KTSCUSTSNDWORK.DROPDAT,KTSCUSTSNDWORK.BONUSCLOSEYM, KTSCUSTSNDWORK.BONUSFINCHK, KTSCUSTSNDWORK.STOCKCLOSEYM, KTSCUSTSNDWORK.STOCKFINCHK " _
           &"FROM KTSCUSTSNDWORK LEFT OUTER JOIN RTObj RTObj_4 ON KTSCUSTSNDWORK.REALCONSIGNEE = RTObj_4.CUSID LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_2 INNER JOIN RTObj RTObj_3 ON RTEmployee_2.CUSID = RTObj_3.CUSID ON KTSCUSTSNDWORK.REALENGINEER = RTEmployee_2.EMPLY LEFT OUTER JOIN " _
           &"RTObj RTObj_2 ON KTSCUSTSNDWORK.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN " _
           &"RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON KTSCUSTSNDWORK.ASSIGNENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
           &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON KTSCUSTSNDWORK.PRTUSR = RTEmployee.EMPLY " _
           &"where " & searchqry
    End If  
  'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>
