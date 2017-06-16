<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="Sparq 0809專案管理系統"
  title="速博VoIP客戶異動"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="Y;N;Y;Y;Y;Y"
  'functionOptName="異動作廢"
  'functionOptProgram="RTSparq499CustChgDrop.asp"
  'functionOptPrompt ="Y"
  functionoptopen   ="1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="客戶代號;項次;異動項目;異動前名稱;異動後名稱;異動說明;異動日期"
  sqlDelete ="SELECT CUSID, ENTRYNO, CODENC, OCUSNC, NCUSNC, MODIFYDESC, a.EDAT  "_
			&"FROM   RTSparq0809CustChg a "_
			&"inner join RTCode b on a.MODIFYCODE = b.CODE and b.KIND ='M7' "_
            &"where	 a.cusid='*' "
     
  dataTable="RTSparqVoIPCustChg"
  userDefineDelete=""
  extTable=""
  numberOfKey=2
  dataProg="RTSparq0809CustChgD.asp"
  datawindowFeature=""
  searchWindowFeature="width=700,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage=""
  colSplit=1
  keyListPageSize=20

  searchFirst=false
 ' response.Write "k0=" & aryparmkey(0) & ";k1=" & aryparmkey(1) & ";k2=" & aryparmkey(2)
  'searchQry="a.cusid='" & aryparmkey(0) &"' "
  searchProg="self"
  'searchShow=FrGetCmtyDesc(aryParmKey(0))
  
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  
  sqllist   ="SELECT CUSID, ENTRYNO, CODENC, OCUSNC, NCUSNC, MODIFYDESC, a.EDAT "_
			&"FROM   RTSparq0809CustChg a "_
			&"inner join RTCode b on a.MODIFYCODE = b.CODE and b.KIND ='M7' "_
            &"where a.cusid='" & aryparmkey(0) &"' " _
'response.write "SQL=" & sqllist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>
