 <%
  Dim search1,parm,vk
  parm=request("Key")
  vk=split(parm,";")
  if ubound(vk) > 0 then  searchX=vK(0)
%>
<!-- #include virtual="/WebUtility/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<%
Sub SrEnvironment()
  company=application("company")
  system="HI-Building管理系統"
  title="COT建置自付額審核撤銷"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="自付額審核確認"
  functionOptProgram="Verify.asp"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLIb"
  formatName="社區名稱;估價金額;同意安裝;T1到位日期;T1開通日期;列印日期;列印人員;審核日期;審核人員"
  sqlDelete="SELECT COMN,ASSESS,AGREE,T1ARRIVE,T1APPLY,PAYPRTD,PAYPRTUSR,ACCOUNTCFM,accountusr From RTCmty " _
             &  "where COMN='*' " 
  dataTable="rtCMTY"
  numberOfKey=1
  dataProg=""
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature="width=640,height=460,scrollbars=yes"
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=TRUE
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20
  searchProg="RTCOTpaysearch.asp"
  search1=Request("search1")
  if search1="" then search1=searchx
  'search2=Request("search2")
  If search1="" Then
     searchFirst=true
     where="ACCOUNTCFM IS not NULL and " 
  Else
     searchFirst=False
     where= "PAYPRTUSR='" & request("search1") & "' and "
  End If
  sqlList="SELECT COMN,ASSESS,AGREE,T1ARRIVE,T1APPLY,PAYPRTD,PAYPRTUSR,ACCOUNTCFM,accountusr From RTCmty WHERE  "  &where
End Sub
Sub SrSearch()
%>
  <table>
    <tr>
     <td><input name="search1" type="text" value="<%=search1%>" readonly></td>
     <td><%=keyname%></td>
    </tr>
  </table>
<%
End Sub
%>