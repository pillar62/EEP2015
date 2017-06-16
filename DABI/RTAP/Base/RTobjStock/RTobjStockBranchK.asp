<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<%
Dim Debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="證券公司分行基本資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  functionOptName="營業員"
  functionOptProgram="RTobjStockBranchBussK.asp"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;分行名稱;none;輸入人員;輸入日期;none;異動人員;異動日期"
  sqlDelete="SELECT RTBranch.CUSID, RTBranch.BRANCH, RTBranch.EUSR, RTObj.CUSNC, " _
           &"RTBranch.EDAT, RTBranch.UUSR, RTObj1.CUSNC, RTBranch.UDAT " _
           &"FROM RTObj RTObj1 RIGHT OUTER JOIN " _
           &"RTEmployee RTEmployee1 ON " _
           &"RTObj1.CUSID = RTEmployee1.CUSID RIGHT OUTER JOIN " _
           &"RTObj RIGHT OUTER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID RIGHT OUTER JOIN " _
           &"RTBranch ON RTEmployee.EMPLY = RTBranch.EUSR ON " _
           &"RTEmployee1.EMPLY = RTBranch.UUSR " _
           &"WHERE (cusid='*') "
  dataTable="RTBranch"
  numberOfKey=2
  dataProg="RTobjStockBranchD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature="width=640,height=460,scrollbars=yes"
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=False
  goodMorningImage="cbbn.jpg"
  colSplit=2
  keyListPageSize=40
  searchProg="self"
  searchShow=FrGetStockDesc(aryParmKey(0))
  searchQry=" RTBranch.cusid='" &aryParmKey(0) &"' "
  sqlList="SELECT RTBranch.CUSID, RTBranch.BRANCH, RTBranch.EUSR, RTObj.CUSNC, " _
           &"RTBranch.EDAT, RTBranch.UUSR, RTObj1.CUSNC, RTBranch.UDAT " _
           &"FROM RTObj RTObj1 RIGHT OUTER JOIN " _
           &"RTEmployee RTEmployee1 ON " _
           &"RTObj1.CUSID = RTEmployee1.CUSID RIGHT OUTER JOIN " _
           &"RTObj RIGHT OUTER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID RIGHT OUTER JOIN " _
           &"RTBranch ON RTEmployee.EMPLY = RTBranch.EUSR ON " _
           &"RTEmployee1.EMPLY = RTBranch.UUSR " _
           &"WHERE " & searchQry & " " _
           &"ORDER BY RTBranch.cusid "
      
End Sub
%>
<!-- #include file="RTGetStockDesc.inc" -->