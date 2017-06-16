<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<%
if not Session("passed") then
   Response.Redirect "http://www.cbbn.com.tw/Consignee/logon.asp"
end if

Sub SrEnvironment()
  on error resume next
  company="元訊寬頻網路股份有限公司"
  system="Hi-Building管理系統"
  title="Hi-Building社區及客戶資料維護"
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
  sqlDelete="SELECT RTCmty.COMN, RTCode.CODENC FROM RTCmty LEFT OUTER JOIN " _
           &"RTCode ON RTCmty.COMTYPE = RTCode.CODE AND RTCode.KIND = 'B3' " _
           &"ORDER BY  RTCmty.COMN "
  dataTable="RTCmty"
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
  searchProg="ConsigneeALLhbCmtyS.asp"
' Open search program when first entry this keylist
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=false
  
  If searchQry="" Then
     searchQry=" RTCMTY.Comq1<>0 and rtCMTY.RCOMDROP IS NULL "
    ' searchShow="全部(不含退租、撤銷、不可建置戶)"
    searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  
  sqllist="SELECT RTCmty.COMN, RTCode.CODENC FROM RTCmty LEFT OUTER JOIN " _
           &"RTCode ON RTCmty.COMTYPE = RTCode.CODE AND RTCode.KIND = 'B3' " _
           &"WHERE " &  searchqry & " " _           
           &"ORDER BY  RTCmty.COMN "
 
'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>
