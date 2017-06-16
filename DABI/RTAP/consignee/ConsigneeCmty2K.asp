<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<%
if not Session("passed") then
   Response.Redirect "http://www.cbbn.com.tw/Consignee/logon.asp"
end if

Sub SrEnvironment()
  on error resume next
  company="元訊寬頻網路股份有限公司"
  system="Sparq* 管理系統"
  title="速博ADSL社區及客戶資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  'V=split(SrAccessPermit,";")
  'AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  'ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"  
  buttonEnable="N;N;Y;Y;Y;Y"
  functionOptName="客　戶"
  functionOptProgram="ConsigneeCustK.asp"
  functionOptPrompt="N;N"
  'If V(1)="Y" then
  '   accessMode="U"
  'Else
     accessMode="I"
  'End IF
  DSN="DSN=RTLib"
  formatName="序號;社區名稱;HB號碼;IP;設備位置;ADSL測通日;申請戶;已撤銷;已完工;己報峻;已退租" 
  sqlDelete="SELECT RTSparqAdslCmty.CUTYID, RTSparqAdslCmty.COMN, RTSparqAdslCmty.HBNO, " _
           &"RTSparqAdslCmty.IPADDR, RTSparqAdslCmty.EQUIPADDR,RTSparqAdslCmty.ADSLAPPLY, " _
           &"SUM(CASE WHEN rtsparqadslcust.cusid IS NOT NULL OR rtsparqadslcust.CUSID <> '' THEN 1 ELSE 0 END), " _
           &"SUM(CASE WHEN rtsparqadslcust.DROPDAT IS NOT NULL and rtsparqadslcust.FINISHDAT IS NULL THEN 1 ELSE 0 END), " _
           &"SUM(CASE WHEN rtsparqadslcust.finishdat IS NOT NULL OR rtsparqadslcust.finishdat <> '' THEN 1 ELSE 0 END), " _           
           &"SUM(CASE WHEN rtsparqadslcust.docketdat IS NOT NULL OR rtsparqadslcust.docketdat <> '' THEN 1 ELSE 0 END), " _
           &"SUM(CASE WHEN rtsparqadslcust.FINISHdat IS NOT NULL and rtsparqadslcust.DROPDAT IS NOT NULL THEN 1 ELSE 0 END) " _
           &"FROM RTSparqAdslCmty LEFT OUTER JOIN rtsparqadslcust ON RTSparqAdslCmty.CUTYID = rtsparqadslcust.COMQ1 " _
           &"WHERE (RTSparqAdslCmty.COMN <> '*') " _
           &"GROUP BY  RTSparqAdslCmty.CUTYID, RTSparqAdslCmty.COMN, RTSparqAdslCmty.HBNO, " _
           &"RTSparqAdslCmty.IPADDR, RTSparqAdslCmty.EQUIPADDR, " _
           &"RTSparqAdslCmty.ADSLAPPLY " _
           &"ORDER BY  RTSparqAdslCmty.equipaddr "
  dataTable="RTSparqAdslCmty"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="RTCMTYD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=480,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20
  searchProg="ConsigneeCmty2S.asp"
' Open search program when first entry this keylist
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=false
  
  If searchQry="" Then
    ' searchQry=" RTCUSTADSLCMTY.CUTYID<>0 and rtcustadsl.dropdat is null and rtcustadsl.agree <>'N' "
     searchQry="  RTSparqAdslCmty.CUTYID<>0  "
    ' searchShow="全部(不含退租、撤銷、不可建置戶)"
    searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  
  sqllist="SELECT RTSparqAdslCmty.CUTYID, RTSparqAdslCmty.COMN, RTSparqAdslCmty.HBNO, " _
       &"RTSparqAdslCmty.IPADDR, RTSparqAdslCmty.EQUIPADDR, RTSparqAdslCmty.ADSLAPPLY, " _
       &"SUM(CASE WHEN rtsparqadslcust.cusid IS NOT NULL OR rtsparqadslcust.cusid <> '' THEN 1 ELSE 0 END), " _ 
       &"SUM(CASE WHEN rtsparqadslcust.DROPDAT IS NOT NULL and rtsparqadslcust.FINISHDAT IS NULL THEN 1 ELSE 0 END), " _
       &"SUM(CASE WHEN rtsparqadslcust.finishdat IS NOT NULL OR rtsparqadslcust.finishdat <> '' THEN 1 ELSE 0 END), " _           
       &"SUM(CASE WHEN rtsparqadslcust.docketdat IS NOT NULL OR rtsparqadslcust.docketdat <> '' THEN 1 ELSE 0 END), " _     
       &"SUM(CASE WHEN rtsparqadslcust.FINISHdat IS NOT NULL and rtsparqadslcust.DROPDAT IS NOT NULL THEN 1 ELSE 0 END) " _
       &"FROM RTSparqAdslCmty LEFT OUTER JOIN rtsparqadslcust ON RTSparqAdslCmty.CUTYID = rtsparqadslcust.COMQ1 " _
       &"WHERE " &  searchqry  _
       &" GROUP BY  RTSparqAdslCmty.CUTYID, RTSparqAdslCmty.COMN, RTSparqAdslCmty.HBNO, " _
       &"RTSparqAdslCmty.IPADDR, RTSparqAdslCmty.EQUIPADDR, " _
       &"RTSparqAdslCmty.ADSLAPPLY " _
       &"ORDER BY  RTSparqAdslCmty.comn "
 
'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>
