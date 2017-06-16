<%
  Dim search1,parm,vk,debug36,search2
  parm=request("Key")
  vk=split(parm,";")
  if ubound(vk) > 0 then  searchX=vK(0)
%>
<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Sub SrEnvironment()
  company=application("company")
  system="HI-Building管理系統"
  title="業績獎金審核確認"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="獎金審核"
  functionOptPrompt="Y"
  functionOptProgram="verify.asp"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  
  DSN="DSN=RTLIb"
  formatName="none;年度;月份;姓名;業績獎金;扣項總額;計算社區數;計算戶數;轉檔日;轉檔人員;入帳日;入帳人員"
  sqlDelete="SELECT CUSID,CYY,CMM,BONUS,MINUS,TOTCUT,TRDAT,TRUSR,ACDAT,ACUSR " _
            & "FROM RTSalesBonus
  'response.write "sql=" &sqldelete
  dataTable="b"
  numberOfKey=1
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=TRUE
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20  
  
 
  searchProg="TRbonusS.asp"
  searchFirst=false
  If searchQry="" Then
     searchQry=" and a.rcvdtlno is null and a.cusid='*' " & ";"
     searchShow="未審核"
  End If   
  v=split(searchqry,";")
  '---列印批號空白"
  if len(trim(V(1)))=0 then
     V(0)=" and a.rcvdtlno is null and a.cusid='*' "
  end if 
  
   sqlList="SELECT CUSID,CYY,CMM,BONUS,MINUS,TOTCUT,TRDAT,TRUSR,ACDAT,ACUSR " _
            & "FROM RTSalesBonus " &V(0)
 session("revcfmprtno")=V(1)
 'Response.Write "SQL=" & SQllist
End Sub
%>