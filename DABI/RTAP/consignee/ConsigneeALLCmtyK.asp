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
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""
  'If V(1)="Y" then
  '   accessMode="U"
  'Else
     accessMode="I"
  'End IF
  DSN="DSN=RTLib"
  formatName="社區名稱;經銷商" 
  sqlDelete="SELECT RTSparqADSLcmty.COMN, RTObj.SHORTNC " _
           &"FROM RTSparqADSLcmty INNER JOIN " _
           &"RTObj ON RTSparqADSLcmty.CONSIGNEE = RTObj.CUSID " _
           &"ORDER BY  RTSparqAdslCmty.COMN "
  dataTable="RTSparqAdslCmty"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg=""
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
  colSplit=4
  keyListPageSize=60
  searchProg="ConsigneeALLCmtyS.asp"
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
  
  sqllist="SELECT RTSparqADSLcmty.COMN, RTObj.SHORTNC " _
         &"FROM RTSparqADSLcmty INNER JOIN RTObj ON RTSparqADSLcmty.CONSIGNEE = RTObj.CUSID " _
         &"WHERE " &  searchqry & " " _
         &"ORDER BY  RTSparqAdslCmty.comn "
 
'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>
