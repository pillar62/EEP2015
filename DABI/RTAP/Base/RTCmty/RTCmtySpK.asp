<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="社區管委會資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;N"  
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName=""
  functionOptProgram=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;職務名稱;姓名;聯絡電話;行動電話"
  sqlDelete="SELECT [RTCmtySp].[COMQ1], [RTCmtySp].[SERNO], [RTCode].[CODENC], " _
           &"[RTCmtySp].[SPNAME], [RTCmtySp].[CONTACT], [RTCmtySp].[MOBILE] " _
           &"FROM RTCmtySp INNER JOIN RTCode ON [RTCmtySp].[TITLE]=[RTCode].[CODE] " _
           &"WHERE [RTCode].[KIND]='XX' "
  dataTable="RTCmtySp"
  extTable=""
  numberOfKey=2
  dataProg="RTCmtySpD.asp"
  datawindowFeature=""
  searchWindowFeature=""
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=False
  goodMorningImage=""
  colSplit=1
  keyListPageSize=20
  searchProg="self"
  searchShow=FrGetCmtyDesc(aryParmKey(0))
  searchQry="RTCmtySp.COMQ1=" &aryParmKey(0) &" "
  sqlList="SELECT [RTCmtySp].[COMQ1], [RTCmtySp].[SERNO], [RTCode].[CODENC], " _
           &"[RTCmtySp].[SPNAME], [RTCmtySp].[CONTACT], [RTCmtySp].[MOBILE] " _
           &"FROM RTCmtySp INNER JOIN RTCode ON [RTCmtySp].[TITLE]=[RTCode].[CODE] " _
           &"WHERE [RTCode].[KIND]='A1' AND " &searchQry
End Sub
%>
<!-- #include file="RTGetCmtyDesc.inc" -->
