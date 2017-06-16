<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="Sparq499 管理系統"
  title="速博ADSL499已報竣客戶退租"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="Y;N;Y;Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="異動作廢"
  functionOptProgram="RTSparq499CustChgDrop.asp"
  functionOptPrompt ="Y"
  functionoptopen   ="1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;none;none;社區;主線;客戶名稱;異動名稱;異動日期;異動說明;報竣日期;轉檔日期;作廢日期"
  sqlDelete ="select a.COMQ1, a.LINEQ1, a.CUSID, a.MODIFYCODE, a.MODIFYDAT, "_
			&"		 a.COMQ1, a.LINEQ1, b.CUSNC, c.CODENC, a.MODIFYDAT, a.MODIFYDESC, a.DOCKETDAT, a.TRANSDAT, a.DROPDAT "_
			&"from 	 RTSparq499CustChg a "_
			&"		 inner join RTSparq499Cust b on a.COMQ1 = b.COMQ1 and a.LINEQ1 = b.LINEQ1 and a.CUSID = b.CUSID "_
			&"		 left outer join RTCode c on c.CODE = a.MODIFYCODE and c.KIND ='K3' "_
            &"where	 a.cusid='*' "_
            &"ORDER BY  a.MODIFYDAT "
     
  dataTable="RTSparq499CustChg"
  userDefineDelete=""
  extTable=""
  numberOfKey=5
  dataProg="RTSparq499CustChgD.asp"
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
  searchQry="a.comq1 =" & aryparmkey(0) & " and a.lineq1='" & aryparmkey(1) & "' and a.cusid='" & aryparmkey(2) &"' "
  searchProg="self"
  searchShow=FrGetCmtyDesc(aryParmKey(0))
  
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  sqllist   ="select a.COMQ1, a.LINEQ1, a.CUSID, a.MODIFYCODE, a.MODIFYDAT, "_
			&"		 a.COMQ1, a.LINEQ1, b.CUSNC, c.CODENC, a.MODIFYDAT, a.MODIFYDESC, a.DOCKETDAT, a.TRANSDAT, a.DROPDAT "_
			&"from 	 RTSparq499CustChg a "_
			&"		 inner join RTSparq499Cust b on a.COMQ1 = b.COMQ1 and a.LINEQ1 = b.LINEQ1 and a.CUSID = b.CUSID "_
			&"		 left outer join RTCode c on c.CODE = a.MODIFYCODE and c.KIND ='K3' "_
            &"where a.comq1=" & aryparmkey(0) & " and a.lineq1='" & aryparmkey(1) & "' and a.cusid='" & aryparmkey(2) &"' " _            
            &"ORDER BY  a.MODIFYDAT "
'response.write "SQL=" & sqllist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>
<!-- #include file="RTGetCmtyDesc.inc" -->
