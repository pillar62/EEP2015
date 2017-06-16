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
  functionOptName="經銷客戶"
  functionOptProgram="consigneecmty3k.asp"
  functionOptPrompt="N"
  'If V(1)="Y" then
  '   accessMode="U"
  'Else
     accessMode="I"
  'End IF
  DSN="DSN=RTLib"
  formatName="none;經銷商;申請戶;已撤銷;已完工;已報竣;已退租" 
  sqlDelete="SELECT RTSPARQADSLCMTY.CONSIGNEE,CASE WHEN RTOBJ.SHORTNC IS NULL " _
         &"THEN '元訊' ELSE RTOBJ.SHORTNC END, " _
         &"SUM(CASE WHEN rtsparqadslcust.cusid IS NOT NULL OR " _
         &"rtsparqadslcust.cusid <> '' THEN 1 ELSE 0 END), " _
         &"SUM(CASE WHEN rtsparqadslcust.DROPDAT IS NOT NULL AND " _
         &"rtsparqadslcust.FINISHDAT IS NULL THEN 1 ELSE 0 END), " _
         &"SUM(CASE WHEN rtsparqadslcust.finishdat IS NOT NULL OR " _
         &"rtsparqadslcust.finishdat <> '' THEN 1 ELSE 0 END), " _
         &"SUM(CASE WHEN rtsparqadslcust.docketdat IS NOT NULL OR " _
         &"rtsparqadslcust.docketdat <> '' THEN 1 ELSE 0 END), " _
         &"SUM(CASE WHEN rtsparqadslcust.FINISHdat IS NOT NULL AND " _
         &"rtsparqadslcust.DROPDAT IS NOT NULL THEN 1 ELSE 0 END) " _
         &"FROM RTSparqAdslCmty LEFT OUTER JOIN " _
         &"rtsparqadslcust ON " _
         &"RTSparqAdslCmty.CUTYID = rtsparqadslcust.COMQ1 LEFT OUTER JOIN " _
         &"RTOBJ ON RTSPARQADSLCMTY.CONSIGNEE = RTOBJ.CUSID " _
         &"WHERE RTSparqAdslCmty.CUTYID = 0 " _
         &"GROUP BY  RTSPARQADSLCMTY.CONSIGNEE,RTSPARQADSLCMTY.SHORTNC "
  dataTable="RTSparqAdslCmty"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg=""
  datawindowFeature=""
  searchWindowFeature="width=400,height=300,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=15
  searchProg="self"
' Open search program when first entry this keylist
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=false
  
  If searchQry="" Then
     searchQry=" RTSPARQADSLCMTY.CUTYID<>0 and rtSPARQADSLCMTY.RCOMDROP IS NULL "
    ' searchShow="全部(不含退租、撤銷、不可建置戶)"
    searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  
  sqllist="SELECT  RTSPARQADSLCMTY.CONSIGNEE,CASE WHEN RTOBJ.SHORTNC IS NULL " _
         &"THEN '元訊' ELSE RTOBJ.SHORTNC END, " _
         &"SUM(CASE WHEN rtsparqadslcust.cusid IS NOT NULL OR " _
         &"rtsparqadslcust.cusid <> '' THEN 1 ELSE 0 END), " _
         &"SUM(CASE WHEN rtsparqadslcust.DROPDAT IS NOT NULL AND " _
         &"rtsparqadslcust.FINISHDAT IS NULL THEN 1 ELSE 0 END), " _
         &"SUM(CASE WHEN rtsparqadslcust.finishdat IS NOT NULL OR " _
         &"rtsparqadslcust.finishdat <> '' THEN 1 ELSE 0 END), " _
         &"SUM(CASE WHEN rtsparqadslcust.docketdat IS NOT NULL OR " _
         &"rtsparqadslcust.docketdat <> '' THEN 1 ELSE 0 END), " _
         &"SUM(CASE WHEN rtsparqadslcust.FINISHdat IS NOT NULL AND " _
         &"rtsparqadslcust.DROPDAT IS NOT NULL THEN 1 ELSE 0 END) " _
         &"FROM RTSparqAdslCmty LEFT OUTER JOIN " _
         &"rtsparqadslcust ON " _
         &"RTSparqAdslCmty.CUTYID = rtsparqadslcust.COMQ1 LEFT OUTER JOIN " _
         &"RTOBJ ON RTSPARQADSLCMTY.CONSIGNEE = RTOBJ.CUSID " _
         &"WHERE " &  searchqry & " " _
         &"GROUP BY   RTSPARQADSLCMTY.CONSIGNEE,RTOBJ.SHORTNC " 
'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>
