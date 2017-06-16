<%@ Transaction = required %>
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="倉庫基本資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"
 ' buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="產品存量;可領員工"
  functionOptProgram="hbwarehouseprodK.asp;hbwarehousesalesk.asp"
  functionOptPrompt="N;N"  
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  debug36=false
  formatName="庫別;倉庫名稱;地址;維護人員"
  sqlDelete="SELECT hbwarehouse.warehouse,hbwarehouse.warename,rtcounty.cutnc+ " _
          &"hbwarehouse.township+hbwarehouse.address,rtobj.cusnc " _
          &"from rtobj inner join rtemployee on rtobj.cusid=rtemployee.cusid right outer join " _
          &"hbwarehouse inner join rtcounty on hbwarehouse.cutid=rtcounty.cutid on " _
          &"rtemployee.emply=hbwarehouse.maintainusr " _
          &"where hbwarehouse.warehouse='*' order by hbwarehouse.warehouse "

  dataTable="hbwarehouse"
  userDefineDelete="Yes"  
  extTable=""
  numberOfKey=1
  dataProg="HBwarehouseD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=true
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20
  searchProg="hbwarehouseS.asp"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=false
  If searchQry="" Then
     searchQry=" HBWAREHOUSE.WAREHOUSE <>'*' "
     searchShow="全部"
  End If
  sqlList="SELECT hbwarehouse.warehouse,hbwarehouse.warename,rtcounty.cutnc+ " _
          &"hbwarehouse.township+hbwarehouse.address,rtobj.cusnc " _
          &"from rtobj inner join rtemployee on rtobj.cusid=rtemployee.cusid right outer join " _
          &"hbwarehouse inner join rtcounty on hbwarehouse.cutid=rtcounty.cutid on " _
          &"rtemployee.emply=hbwarehouse.maintainusr " _
          &"where hbwarehouse.warehouse<>'*' and " & searchqry & " order by hbwarehouse.warehouse "
'Response.Write "SQL=" &sqllist           
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>