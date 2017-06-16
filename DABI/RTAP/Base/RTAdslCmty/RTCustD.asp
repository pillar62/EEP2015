<%
  Dim fieldRole,fieldPa,fieldPb,fieldpc,fieldpd,fieldpe
  fieldRole=Split(FrGetUserRight("RTCustD",Request.ServerVariables("LOGON_USER")),";")
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  
%>
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzDataList.inc" -->
<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=3
  title="ADSL客戶基本資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  'sqlFormatDB="SELECT * FROM RTCust WHERE Comq1=0 "
  sqlFormatDB="SELECT comq1,CUSID, ENTRYNO,STOCKID,BRANCH,BUSSMAN,BUSSID,SEX,BIRTHDAY,cutid1, "_
			 &"township1,raddr1,rzone1,cutid2,township2,raddr2,rzone2, cutid3,township3,raddr3, "_
			 &"rzone3,SPEED,LINETYPE,CASETYPE,USEKIND,RCVD,HOUSETYPE,HOUSENAME,HOUSEQTY,exttel, "_
			 &"HOME,FAX,CONTACT,OFFICE, EXTENSION, MOBILE, EMAIL, VOUCHER, EUSR,EDAT, "_
			 &"UUSR,UDAT,PROFAC,SNDINFODAT,REQDAT,INSPRTNO,INSPRTDAT,INSPRTUSR,FINISHDAT,DOCKETDAT, "_
			 &"INCOMEDAT,AR,ACTRCVAMT,DROPDAT,RCVDTLNO,RCVDTLPRT,SCHDAT,FINRDFMDAT,FINCFMUSR,BONUSCAL, "_
			 &"DROPDESC,UNFINISHDESC,PAYDTLPRTNO,PAYDTLDAT,PAYDTLUSR,ACCCFMDAT,ACCCFMUSR,ENDCOD,NOTE,OPERENVID, "_
			 &"SETTYPE,SETSALES,PRESETDATE,PRESETHOUR,PRESETMIN,SETFEE,SETFEEDIFF,SETFEEDESC,orderno,ss365, "_
			 &"REPLYDATE,Lookdat,formaldat,deliverdat,socialid,agree,haveroom,homestat,LOOKDESC,CHTSIGNDAT, "_
			 &"SENDWORKING,WORKINGREPLY,cusno,transdat,holdemail,proposer,IP,COTPORT,overdue,freecode,ANO,BNO,IDNUMBERTYPE, "_
			 &"secondidtype,secondno, REVIVEDAT, REVIVEFLAG, GTMONEY, GTVALID, GTSERIAL,DEVELOPERID "_
             &"FROM RTCustADSL where cusid='*'"
           
  sqllist    ="SELECT COMQ1,CUSID, ENTRYNO,STOCKID,BRANCH,BUSSMAN,BUSSID,SEX,BIRTHDAY, " _
             &"cutid1,township1,raddr1,rzone1,cutid2,township2,raddr2,rzone2, " _
             &"cutid3,township3,raddr3,rzone3,SPEED,LINETYPE,CASETYPE,USEKIND, " _
             &"RCVD,HOUSETYPE,HOUSENAME,HOUSEQTY,exttel,HOME,FAX,CONTACT,OFFICE, EXTENSION, MOBILE, EMAIL, " _
             &"VOUCHER, EUSR,EDAT,UUSR,UDAT,PROFAC,SNDINFODAT, REQDAT, INSPRTNO, INSPRTDAT, INSPRTUSR,  " _
             &"FINISHDAT, DOCKETDAT, INCOMEDAT, AR, ACTRCVAMT, DROPDAT, RCVDTLNO,  " _
             &"RCVDTLPRT, SCHDAT, FINRDFMDAT, FINCFMUSR, BONUSCAL, DROPDESC, " _
             &"UNFINISHDESC, PAYDTLPRTNO, PAYDTLDAT, PAYDTLUSR, ACCCFMDAT, " _
             &"ACCCFMUSR, ENDCOD, NOTE,OPERENVID, SETTYPE, " _
             &"SETSALES, PRESETDATE, PRESETHOUR, PRESETMIN, SETFEE, SETFEEDIFF, SETFEEDESC,orderno,ss365, " _
             &"REPLYDATE,Lookdat,formaldat,deliverdat,socialid,agree,haveroom,homestat, LOOKDESC,CHTSIGNDAT, " _
             &"SENDWORKING,WORKINGREPLY,cusno,transdat,holdemail,proposer,IP,COTPORT,overdue,freecode,ANO,BNO, "_
             &"IDNUMBERTYPE,secondidtype,secondno, REVIVEDAT, REVIVEFLAG, GTMONEY, GTVALID, GTSERIAL,DEVELOPERID "_
             &"FROM RTCustADSL where "
 ' Response.write "SQL=" & SQLlist
 ' Response.end            
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=1
  userDefineRead="Yes"
  userDefineSave="Yes"
  userdefineactivex="Yes"  
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
'------社區序號---固定(由上層程式傳入) 
   ' dspkey(92)=SESSION("comq1")
'------社區名稱---固定(由社區序號讀取)
    Set connxx=Server.CreateObject("ADODB.Connection")  
    Set rsxx=Server.CreateObject("ADODB.Recordset")
    DSNXX="DSN=RTLIB"
    connxx.Open DSNxx
    sqlXX="SELECT * FROM RTCustAdslCmty where cutyid=" & dspkey(0)
    rsxx.Open sqlxx,connxx
    s=""
    ADSLAPPLY=""
    If rsxx.Eof Then
       message="社區代號:" &dspkey(0) &"在社區基本資料內找不到"
       formvalud=false
       adslapply="N"
    Else 
       dspkey(27)=rsxx("ComN") 
       IF ISNULL(RSXX("ADSLAPPLY")) THEN
          adslapply="N"
       ELSE
          adslapply="Y"
       END IF
    End If
    rsxx.Close
    Set rsxx=Nothing
    connxx.Close
    Set connxx=Nothing    
    if len(trim(dspkey(23)))=0 then dspkey(23)=""
'-------單次------------------------------
    If Not IsNumeric(dspKey(2)) Then dspKey(2)=0
'-------戶數------------------------------
    If Not IsNumeric(dspKey(28)) or len(trim(dspkey(28))) = 0 Then dspKey(28)=0    
'--------------- -------------------------
    If Not IsNumeric(dspKey(51)) Then dspKey(51)=0   '應收金額
    If Not IsNumeric(dspKey(52)) Then dspKey(52)=0   '實收金額 
    If Not IsNumeric(dspKey(70)) Then dspKey(70)=3   '安裝類別
    If Not IsNumeric(dspKey(75)) Then dspKey(75)=0   '標準施工費
    If Not IsNumeric(dspKey(76)) Then dspKey(76)=0   '施工補助費   
    If Not IsNumeric(dspKey(73)) Then dspKey(73)=0   '裝機(時)
    If Not IsNumeric(dspKey(74)) Then dspKey(74)=0   '裝機(分)
    If Not IsNumeric(dspKey(107)) Then dspKey(107)=0 '保證金    
   ' If len(trim(dspkey(1))) < 1 then
   '    message="請入客戶代碼"
   '    formValid=False
    If dspKey(73) > 24 Or dspKey(74) > 59 Then
       message="請輸入正確預定裝機時間"
       formValid=False
    elseif len(trim(extdb(0))) < 1 then
       message="請輸入客戶名稱"
       formValid=False    
    elseif len(trim(dspkey(6)))=0 and dspkey(42) =""then
       message="業務員欄位不可空白!"
       formValid=False
    elseif not Isdate(dspkey(8)) and len(dspkey(8)) > 0 then
       message="出生日期錯誤"
       formValid=False            
    elseif not Isdate(dspkey(25)) and len(dspkey(25))  > 0 then
       message="申請日期錯誤"
       formValid=False            
    elseif not Isdate(dspkey(43)) and len(dspkey(43))  > 0 then
       message="通知發包日期錯誤"
       formValid=False     
    elseif not Isdate(dspkey(44)) and len(dspkey(44))  > 0 then
       message="發包日期錯誤"
       formValid=False            
    elseif not Isdate(dspkey(48)) and len(dspkey(48))  > 0 then
       message="完工日期錯誤"
       formValid=False     
    elseif not Isdate(dspkey(49)) and len(dspkey(49))  > 0 then
       message="報竣日期錯誤"
       formValid=False             
    elseif not IsNumeric(dspkey(51)) and len(dspkey(51))  > 0 then
       message="應收金額錯誤"
       formValid=False           
    elseif not IsNumeric(dspkey(52)) and len(dspkey(52))  > 0 then
       message="實收金額錯誤"
       formValid=False             
    elseif not Isdate(dspkey(53)) and len(dspkey(53))  > 0 then
       message="撤銷日期錯誤"
       formValid=False             
    elseif not Isdate(dspkey(56)) and len(dspkey(56))  > 0 then
       message="收款日期錯誤"
       formValid=False          
    elseif not Isdate(dspkey(72)) and len(dspkey(72))  > 0 then
       message="預定裝機日期錯誤"
       formValid=False          
    elseif not IsNumeric(dspkey(73)) and len(dspkey(73))  > 0 then
       message="預定裝機時間錯誤"
       formValid=False          
    elseif not IsNumeric(dspkey(74)) and len(dspkey(74))  > 0 then
       message="預定裝機時間錯誤"
       formValid=False              
    elseif not IsNumeric(dspkey(76)) and len(dspkey(76))  > 0 then
       message="施工補助金額錯誤"
       formValid=False                     
    elseif (dspkey(70)="1" or dspkey(70)="2" ) and dspkey(42) <> "" then
       message="安裝人員為(業務)或(技術部)時,施工廠商必須空白"
       formvalid=false
    elseif (dspkey(70)="3" ) and dspkey(42) = "" then
       message="安裝人員為(廠商)時,施工廠商不得空白"
       formvalid=false       
    elseif (dspkey(70)="1" ) and dspkey(51) = "" then
       message="安裝人員為(業務)時,預定安裝人員不得空白"
       formvalid=false              
    elseif LEN(TRIM(dspkey(49)))<>0 and ADSLAPPLY <> "Y" then
       message="主線尚未測通，不得輸入報竣日期"
       formvalid=false                
    elseif LEN(TRIM(dspkey(48)))<>0 and ADSLAPPLY <> "Y" then
       message="主線尚未測通，不得完工日期"
       formvalid=false                       
    elseif LEN(TRIM(dspkey(103))) = 0 and dspkey(102)<>"02" and accessMode ="A" and dspKey(99)<>"Y" then
       message="第二證照類別不可空白"
       formvalid=false                 
    elseif LEN(TRIM(dspkey(104))) = 0 and dspkey(102)<>"02" and accessMode ="A" and dspKey(99)<>"Y" then
       message="第二證照號碼不可空白"
       formvalid=false                       
    End If
    IF formvalid=TRUE THEN
       if (dspkey(102)="01" or dspkey(102)="02") and dspkey(84) <> "" and accessMode ="A" then       
         idno=dspkey(84)
         if UCASE(left(idno,1)) >="A" AND UCASE(left(idno,1)) <="Z" THEN
            AAA=CheckID(idno)
            SELECT CASE AAA
               CASE "True"
               case "False"
                     message="申請用戶身份證字號不合法"
                     formvalid=false 
               case "ERR-1"
                     message="申請用戶身份證號不可留空白或輸入位數錯誤"
                     formvalid=false       
               case "ERR-2"
                     message="申請用戶身份證字號的第一碼必需是合法的英文字母"
                     formvalid=false    
               case "ERR-3"
                     message="申請用戶身份證字號的第二碼必需是數字 1 或 2"
                     formvalid=false   
               case "ERR-4"
                     message="申請用戶身份證字號的後九碼必需是數字"
                     formvalid=false              
               case else
            end select  
         ELSE
            AAA=ValidBID(idno)
            if aaa = false then
                message="申請用戶統一編號不合法"
                formvalid=false   
            end if
         END IF       
       END IF       
    END IF
    if dspkey(7) <> "F" and dspkey(7) <>"M" then dspkey(7)=""
    if dspkey(99) <> "Y" and dspkey(99) <>"N" then dspkey(99)=""
'廠商標準施工費(施工廠商不為空白，且無付款列印批號時，始可變更）
    if len(trim(dspkey(42))) > 0 and len(trim(dspkey(62))) = 0 then
       Dim Connsupp,Rssupp,sqlsupp,dsn
       Set connsupp=server.CreateObject("ADODB.Connection")
       Set rssupp=Server.CreateObject("ADODB.RecordSet")
       DSN="DSN=RTLIB"
       Sqlsupp="select * from RtSupp where cusid='" & dspkey(42) & "'"
       connsupp.open DSN
       rssupp.open sqlsupp,connsupp,1,1
       if rssupp.eof then
          dspkey(75) = 0
       else
          dspkey(75) = rssupp("STDFee")
       end if
    end if
 '收據名稱為空白時，
   IF len(trim(dspkey(37))) = 0 then
      dspkey(37)=extdb(0)
   end if
 '聯絡人為空白時，
 '  IF len(trim(dspkey(32))) = 0 then
 '     dspkey(32)=extdb(0)
 '  end if   
 '申請代表人為空白時，預設為"N"
   IF len(trim(dspkey(95))) = 0 then
      dspkey(95)="N"
   end if   
 '欠費拆機為空白時，預設為"N"
   IF len(trim(dspkey(98))) = 0 then
      dspkey(98)="N"
   end if      
 '是否可建置欄位,若非y或n時,預設為空白
   if trim(dspkey(85)) <>"Y" and trim(dspkey(85)) <>"N" then
      dspkey(85)=""
   end if
'---檢查 HN號碼是否有重複 DSPKEY(92)---------
   IF LEN(TRIM(DSPKEY(92))) > 0 THEN
      Set connxx=Server.CreateObject("ADODB.Connection")  
      Set rsxx=Server.CreateObject("ADODB.Recordset")
      DSNXX="DSN=RTLIB"
      connxx.Open DSNxx
      
	  if LEN(TRIM(DSPKEY(2))) =0 then
		 DSPKEY(2) =1
	  end if   
      sqlXX="SELECT count(*) AS CNT FROM RTCustAdsl where cusno='" & trim(dspkey(92)) & "' and not (cusid='" & dspkey(1) & "' and entryno=" & dspkey(2) & ")"
      rsxx.Open sqlxx,connxx
      s=""
      If RSXX("CNT") > 0 Then
         message="HN號碼已存在ADSL客戶，不可重複輸入!"
         formvalid=false
      End If
      rsxx.Close
      if formvalid then
         sqlXX="SELECT count(*) AS CNT FROM RTCust where cusno='" & trim(dspkey(92)) & "' "
         rsxx.Open sqlxx,connxx
         If RSXX("CNT") > 0 Then
            message="HN號碼已存在HB客戶，不可重複輸入!"
            formvalid=false
         end if
         rsxx.Close            
      End If
      Set rsxx=Nothing
      connxx.Close
      Set connxx=Nothing    
   end IF
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="修改" then
        Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                UUsrNc=V(1)
                DSpkey(40)=V(0)
    end if   
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveXScript()%>
   <SCRIPT Language="VBScript">
   Sub Srbtnonclick()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="KEY" & clickid
	   if isdate(document.all(clickkey).value) then
	      objEF2KDT.varDefaultDateTime=document.all(clickkey).value
       end if
       call objEF2KDT.show(1)
       if objEF2KDT.strDateTime <> "" then
          document.all(clickkey).value = objEF2KDT.strDateTime
       end if
   End Sub 
   Sub SrSelonclick()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="KEY" & clickid
       prog="RTFaQFinishUsrx.asp"
       CUTID=document.all("key13").value
       town=document.all("key14").value
       'showopt="Y;Y;Y;Y"表示對話方塊中要顯示的項目(業務工程師;客服人員;技術部;廠商)
       if clickkey="KEY6" then
          showopt="Y;N;N;N" & ";" & cutid & ";" & town
       else
          showopt="N;N;N;N;;"
       end if
       prog=prog & "?showopt=" & showopt
       FUsr=Window.showModalDialog(prog,"Dialog","dialogWidth:590px;dialogHeight:480px;")  
      'Fusrid(0)=維修人員工號或廠商代號  fusrid(1)=只為於上一畫面中秀出中文名稱(無其它作用) fusrid(2)="1"為業務"2"為技術"3"為廠商"4"為客服(作為資料存放於何欄位之依據)
       IF FUSR <> "" THEN
       FUsrID=Split(Fusr,";")    
       if Fusrid(3) ="Y" then
         '廠商取8位,其餘取6位   
         if Fusrid(2)="3"  then 
            document.all(clickkey).value =  left(Fusrid(0),8)
         else
            document.all(clickkey).value =  left(Fusrid(0),6)
         end if 
       End if
       END IF
    '   Set winP=window.Opener
    '   Set docP=winP.document
    '   docP.all("keyform").Submit
    '   winP.focus()             
    '   window.close
   End Sub    
   Sub Srbranchonclick()
       prog="RTGetBRANCHD.asp"
       prog=prog & "?KEY=" & document.all("KEY3").VALUE 
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key4").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub      
   Sub SrbranchMANonclick()
       prog="RTGetBRANCHMAND.asp"
       prog=prog & "?KEY=" & document.all("KEY3").VALUE & ";" & document.all("KEY4").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key5").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub     
   Sub Srcounty10onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY9").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key10").value =  trim(Fusrid(0))
          document.all("key12").value =  trim(Fusrid(1))
       End if       
       end if
   End Sub    
   Sub Srcounty14onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY13").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key14").value =  trim(Fusrid(0))
          document.all("key16").value =  trim(Fusrid(1))
       End if       
       end if
   End Sub   
   Sub Srcounty18onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY17").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key18").value =  trim(Fusrid(0))
          document.all("key20").value =  trim(Fusrid(1))
       End if       
       end if
   End Sub
   Sub SrDeveloperonclick()
       prog="RTGetDeveloperD.asp"
       prog=prog & "?KEY=" & document.all("KEY81").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key110").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub      
          
   Sub SrBUSonclick()
       prog="RTOBJSTOCKBRANCHBUSSD.asp"
       prog=prog & "?KEY=" & document.all("KEY3").VALUE & ";" & document.all("KEY4").VALUE
       FUsr=Window.open(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       Window.form.Submit
   End Sub    
   
   Sub SrClear()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="C" & clickid
       clearkey="key" & clickid       
       if len(trim(document.all(clearkey).value)) <> 0 then
          document.all(clearkey).value =  ""
       end if
   End Sub    
   
   Sub ImageIconOver()
       self.event.srcElement.style.borderBottom = "black 1px solid"
       self.event.srcElement.style.borderLeft="white 1px solid"
       self.event.srcElement.style.borderRight="black 1px solid"
       self.event.srcElement.style.borderTop="white 1px solid"   
   End Sub
   
   Sub ImageIconOut()
       self.event.srcElement.style.borderBottom = ""
       self.event.srcElement.style.borderLeft=""
       self.event.srcElement.style.borderRight=""
       self.event.srcElement.style.borderTop=""
   End Sub      
   
   Sub SrCmtysel()
       Dim ClickID,prog
       prog="RTCmtySelK.asp"
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="C" & clickid
       clearkey="key" & clickid
       CuTID2=document.all("key13").value
       township2=document.all("key14").value
       prog=prog & "?PARM=" & CutID2 & ";" & township2
       Fcmty=window.showModalDialog(prog,"Dialog","dialogWidth:590px;dialogHeight:480px;scroll:Yes")  
       document.all("key27").value=Fcmty
   End Sub    
   </Script>
<%   
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveX() %>
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"    codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<%	
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()
On error resume next
s=FrGetCmtyDesc(SESSION("comq1"))
%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
</table>

<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr>
    <td width="10%" class=dataListHead>社區序號</td>
    <td width="10%" bgcolor="silver">
        <input type="text" name="key0" <%=fieldRole(1)%><%=keyProtect%>
               style="text-align:left;" maxlength="10" size="6"
               value="<%=dspKey(0)%>" readonly class=dataListEntry></td>
    <td width="10%" class=dataListHead>客戶代號</td>
    <td width="10%" bgcolor="silver">
        <input type="text" name="key1" <%=fieldRole(1)%><%=keyProtect%>
               style="text-align:left;" maxlength="10" size="10"
               value="<%=dspKey(1)%>" class=dataListdata readonly></td>
    <td width="10%" class=dataListHead>客戶單次</td>
    <td width="10%" bgcolor="silver">
        <input type="text" name="key2" readonly
               style="text-align:left;" maxlength="6" size="5"
               value="<%=dspKey(2)%>" class=dataListdata></td>
    <td width="10%" bgcolor="orange" >收件編號</td>
    <td width="10%" bgcolor="silver">
        <input type="text" name="key78" readonly
               style="text-align:left;" maxlength="6" size="10"
               value="<%=dspKey(78)%>" class=dataListdata style="color:red"></td>
 <td width="10%" BGCOLOR=#BDB76B>轉檔報竣日期</td>
    <td width="10%" colspan="7" bgcolor=#DCDCDC>
        <input type="text" name="key93" <%=fieldRole(1)%><%=keyProtect%>
               style="text-align:left;color:red" maxlength="10" size="10"
               value="<%=dspKey(93)%>" readonly  class=dataListData></td>               
    </tr>
</table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(38))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                EUsrNc=V(1) 
                dspkey(38)=V(0)
      '          extdb(46)=v(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(38))
                V=split(rtnvalue,";")      
                EUsrNc=V(1)
        End if  
       dspkey(39)=datevalue(now())
    else
        if len(trim(dspkey(40))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                UUsrNc=V(1)
                DSpkey(40)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(40))
                V=split(rtnvalue,";")      
                UUsrNc=V(1)
        End if         
        Call SrGetEmployeeRef(rtnvalue,2,dspkey(38))
             V=split(rtnvalue,";")      
             EUsrNc=V(1)
        dspkey(41)=datevalue(now())
    end if  
' -------------------------------------------------------------------------------------------- 
    IF len(trim(dspkey(37))) = 0 then
      dspkey(37)=extdb(0)
    end if
    Dim conn,rs,s,sx,sql,t
    '結案碼(控制部份欄位不可修改)
    If dspKey(67)="Y" Then
       fieldPa=" class=""dataListData"" readonly "
       fieldPc=""
       fieldpd=""       
       fieldpe=""      
       fieldpf=""              
    Else
       fieldPa=""
       fieldPc=" Onclick=""Srbtnonclick"" "
       fieldpd=" onclick=""SrSelOnClick"" "       
       fieldpe=" onclick=""SrClear"" "     
       fieldpf=" onclick=""Srcmtysel"" "           
    End If    
    '已轉中華電信報竣時==>全部欄位protect(轉檔欄位)  
    if len(trim(dspkey(93))) > 0 then
       'fieldPc=""
       fieldpd=""       
       fieldpe=""      
       fieldpf=""         
       fieldPg=" class=""dataListData"" readonly "
       fieldph=""
       fieldpi=""       
    else
       'fieldPc=" Onclick=""Srbtnonclick"" "
       fieldpd=" onclick=""SrSelOnClick"" "       
       fieldpe=" onclick=""SrClear"" "     
       fieldpf=" onclick=""Srcmtysel"" " 
       fieldPg=""               
       fieldph=" onclick=""SrBranchonclick()"" "       
       fieldpi=" onclick=""SrBranchmanonclick()"" "       
    end if
    '收款表已列印或安裝員類別為發包(或空白)時，不可按安裝員工鈕，不可更改安裝人員資料（即安裝員工鈕disable)
    If Len(Trim(dspKey(54))) > 0  Then
       fieldPb=" class=""dataListData"" readonly "
    Else
       fieldPb=""
    End If
    if dspkey(67)="Y" or Len(Trim(dspKey(54))) > 0  then
       fieldPbx=""       
    else
       fieldPbx="SrAddusr()"
    end if        
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN%>
  <span id="tags1" class="dataListTagsOn"
        onClick="vbscript:tag1.style.display=''    :tags1.classname='dataListTagsOn':
                          tag2.style.display='none':tags2.classname='dataListTagsOf'"><font size=2>基本資料</span>
  <span id="tags2" class="dataListTagsOf"
        onClick="vbscript:tag1.style.display='none':tags1.classname='dataListTagsOf':
                          tag2.style.display=''    :tags2.classname='dataListTagsOn'"><font size=2>發包安裝</span>                                      
  <div class=dataListTagOn> 
<table width="100%" ><tr><td width="100%">&nbsp;</td></tr>                                                      
    <table border="1" width="100%" cellpadding="0" cellspacing="0" id="tag1" height="354">  
    <tr>
        <td width="20%" class="dataListsearch" height="25">用戶IP</td>
        <td width="18%" height="25" bgcolor="silver"> 
        <input type="text" name="key96" size="18" maxlength="15" value="<%=dspkey(96)%>" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListentry"></td> 
        <td width="17%" class="dataListsearch" height="25">公關機</td>       
       <%  dim freecode1, freecode2
    if len(trim(dspkey(99))) =0 and dspkey(67) <> "Y" then
       If Len(Trim(FIELDROLE(1) &dataProtect)) < 1 Then
          freecode1=""
          freecode2=""
       Else
          freecode1=" disabled "
          freecode2=" disabled "
       end if
    else
   '       freecode1=" disabled "
   '       freecode2=" disabled "
    End If
    If dspKey(99)="Y" Then freecode1=" checked "    
    If dspKey(99)="N" Then freecode2=" checked " %>                          
        <td width="16%" height="25" bgcolor="silver">
        <input type="radio" value="Y"  name="key99" <%=FREECODE1%><%=FIELDROLE(1)%>><font size=2>是</font>
        <input type="radio" name="key99" value="N" <%=FREECODE2%><%=FIELDROLE(1)%>><font size=2>否</font></td> 
    </tr>    
    <tr>
        <td width="20%" class="dataListHead" height="25">HomePNA Port</td>       
        <td width="18%" height="25" bgcolor="silver"><input type="text" name="key97" size="10" maxlength="15" value="<%=dspkey(97)%>" <%=fieldpa%><%=dataProtect%> class="dataListentry"></td>
        <td width="17%" class="dataListHead" height="25">MDF號碼1</td>       
        <td width="17%" height="25" bgcolor="silver"><input type="text" name="key100" size="10" maxlength="10" value="<%=dspkey(100)%>" <%=fieldpa%><%=dataProtect%> class="dataListentry"></td>
        <td width="14%" class="dataListHead" height="25">MDF號碼2</td>       
        <td width="18%" height="25" bgcolor="silver"><input type="text" name="key101" size="10" maxlength="10" value="<%=dspkey(101)%>" <%=fieldpa%><%=dataProtect%> class="dataListentry"></td>
	</tr>    
<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='J5' " 
       If len(trim(dspkey(102))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='J5' AND CODE='" & dspkey(102) &"' " 
       'SXX60=""
    End If
    rs.Open sql,conn
    's=s &"<option value=""" &"""" &sx &">(證照別)</option>"
    s=""
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(102) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>    
    <tr>
        <td width="20%" class="dataListHead" height="25">證照別及證照號碼</td>
        <td width="18%" height="25" bgcolor="silver"> 
		<select size="1" name="key102"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry"><%=s%></select>	        
        <input type="password" name="key84" size="10" maxlength="10" value="<%=dspkey(84)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td> 
        <td width="17%" class="dataListHead" height="25">HN號碼</td>       
        <td width="17%" height="25" bgcolor="silver"> 
        <input type="text" name="key92" size="10" maxlength="8" value="<%=dspkey(92)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td> 
        <td width="14%" bgcolor="orange"  height="25" >保留撥接E-MAIL(HN號碼)</td>       
        <td width="18%" height="25" bgcolor="silver"> 
        <input type="text" name="key94" size="8" maxlength="8" value="<%=dspkey(94)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" style="color:red"></td>                         
    </tr>
<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='L4' " 
       If len(trim(dspkey(103))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='L4' AND CODE='" & dspkey(103) &"' " 
       'SXX60=""
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &">(第二證照別)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(103) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>    
    <tr>
        <td width="20%" class="dataListHead" height="25">第二證照別及證照號碼</td>
        <td width="18%" height="25" bgcolor="silver"> 
		<select size="1" name="key103"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry" ID="Select1"><%=s%></select>	        
        <input type="password" name="key104" size="12" maxlength="12" value="<%=dspkey(104)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" ID="Text1"></td> 

<%
	name=""
	if dspkey(110) <> "" then
		sqlxx=" select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
			 &"where rtemployee.emply='" & dspkey(110) & "' "
		rs.Open sqlxx,conn
		if rs.eof then
			name="(對象檔找不該員工)"
		else
			name=rs("cusnc")
		end if
		rs.close
	end if
%>
		<td WIDTH="15%" class="dataListHEAD" height="23">二線開發人員</td>
		<td width="35%"><input type="text" name="key110"value="<%=dspKey(110)%>" <%=fieldRole(1)%><%=dataProtect%> style="text-align:left;" size="8" maxlength="6" readonly class="dataListDATA" ID="Text2">
			<input type="BUTTON" id="Button1" name="B110" style="Z-INDEX: 1"  value="...." onclick="Srdeveloperonclick()">
			<IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF"  <%=fieldpb%> alt="清除" id="C110" name="C110" style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">
			<font size=2><%=name%></font></td></tr>
    </tr>    
    <tr>                   
        <td width="15%" class="dataListHead" height="25">證券公司</td>                                      
        <td width="30%" height="25" bgcolor="silver">
<%  If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) And protect<1 and  len(trim(dspkey(93)))=0 Then 
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
          &"WHERE (RTObjLink.CUSTYID = '06') AND RTOBJ.CUSID NOT IN ('70770184', '47224065') "
       s="<option value="""" >(券商)</option>"
    Else
       sql="SELECT RTObj.CUSNC, RTObjLink.CUSTYID, RTObj.CUSID,RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID " _
          &"WHERE (RTObjLink.CUSTYID = '06') AND RTOBJ.CUSID NOT IN ('70770184', '47224065') and rtobj.cusid='" & dspkey(3) & "' "
    End If
    rs.Open sql,conn
    If rs.Eof Then s="<option value="""" >(券商)</option>"
    sx=""
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(3) Then sx=" selected "
       s=s &"<option value=""" &rs("CUSID") &"""" &sx &">" &rs("SHORTNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close        
    %>
           <select size="1" name="key3" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry">                                            
              <%=s%>
           </select>
        <input type="text" name="key4" size="10" value="<%=dspkey(4)%>" maxlength="12" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" readonly><font size=2>(分行)               
         <input type="button" id="B4"  name="B4"   width="100%" style="Z-INDEX: 1"  value="..." <%=fieldph%> >  
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C4"  name="C4"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >                      
        </td>                              
        <td width="8%" class="dataListHead" height="25">營業員</td>
        <td width="16%" height="25" bgcolor="silver">
        <input type="text" name="key5" size="8" value="<%=dspkey(5)%>" maxlength="12" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" readonly>
         <input type="button" id="B5"  name="B5"   width="100%" style="Z-INDEX: 1"  value="..." <%=fieldpi%>  >                  
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C5"  name="C5"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >    
        </td>     
<% 
   %>               
        <td width="8%" class="dataListHead" height="25">業務員</td>                              
        <td width="16%" height="25" bgcolor="silver">
      <input type="text" name="key6" size="6" maxlength="50" readonly value="<%=dspkey(6)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%> class="dataListEntry" >
     <input type="button" id="B6"  name="B6"   width="100%" style="Z-INDEX: 1"  value="..." <%=fieldpd%>  >
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C6"  name="C6"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >        
        </TD>
        </TR>
      <tr>                                      
        <td width="15%" class="dataListHead" height="25">客戶名稱</td>                                      
        <td width="30%" height="25" bgcolor="silver">
          <input type="text" name="ext0" size="28" maxlength="50" value="<%=extdb(0)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                              
        <td width="8%" class="dataListHead" height="25">性別</td>
<%  dim sexd1, sexd2
    if len(trim(dspkey(93))) =0 and dspkey(67) <> "Y" then
       If Len(Trim(FIELDROLE(1) &dataProtect)) < 1 Then
          sexd1=""
          sexd2=""
       Else
          sexd1=" disabled "
          sexd2=" disabled "
       end if
    else
          sexd1=" disabled "
          sexd2=" disabled "
    End If
    If dspKey(7)="M" Then sexd1=" checked "    
    If dspKey(7)="F" Then sexd2=" checked " %>                          
        <td width="16%" height="25" bgcolor="silver">
        <input type="radio" value="M" <%=sexd1%> name="key7" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtec%>><font size=2>男</font>
        <input type="radio" name="key7" value="F" <%=sexd2%><%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%>><font size=2>女</font></td>                              
        <td width="8%" class="dataListHead" height="25">出生日期</td>                              
        <td width="16%" height="25" bgcolor="silver">
          <input type="text" name="key8" size="10" value="<%=dspkey(8)%>" maxlength="10" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class=dataListEntry>
          <input type="button" id="B8"  name="B8" height="70%" width="70%" style="Z-INDEX: 1" value="..." <%=fieldpc%>></td>                              
      </tr>                              
      <tr>                              
        <td width="15%" class="dataListHead" height="25">帳單(通訊)地址</td>                              
        <td width="60%" colspan="3" height="25" bgcolor="silver">
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) and len(trim(dspkey(93)))=0 Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(9))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市別)</option>"       
       SXX10=" onclick=""Srcounty10onclick()""  "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(9) & "' " 
       SXX10=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(9) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
         <select size="1" name="key9"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class="dataListEntry"><%=s%></select>
        <input type="text" name="key10" size="8" value="<%=dspkey(10)%>" maxlength="10" readonly <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><font size=2>(鄉鎮市區)                 
         <input type="button" id="B10"  name="B10"   width="100%" style="Z-INDEX: 1"  value="..." <%=SXX10%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C10"  name="C10"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >          
        <input type="text" name="key11" size="30" value="<%=dspkey(11)%>" maxlength="60" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" class="dataListHead" height="25">郵遞區號</td>                                 
        <td width="16%" height="25" bgcolor="silver"><input type="text" name="key12" size="10" value="<%=dspkey(12)%>" maxlength="5" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListdata" readonly></td>                                 
      </tr>                                 
      <tr>                                 
        <td width="15%" class="dataListHead" height="25">裝機地址</td>                                 
        <td width="60%" colspan="3" height="25" bgcolor="silver">
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(13))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市別)</option>"       
       SXX14=" onclick=""Srcounty14onclick()"" "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(13) & "' " 
       sXX14=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(13) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>        
        <select name="key13" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1"  style="text-align:left;" maxlength="8" class="dataListEntry">
        <%=s%></select>
        <input type="text" name="key14" size="8" value="<%=dspkey(14)%>" maxlength="10" readonly <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><font size=2>(鄉鎮市區)                 
         <input type="button" id="B14"  name="B14"   width="100%" style="Z-INDEX: 1"  value="..." <%=SXX14%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C14"  name="C14"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >    
        <input type="text" name="key15" size="30" value="<%=dspkey(15)%>" maxlength="60" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                                
        <td width="8%" class="dataListHead" height="25">郵遞區號</td>                                 
        <td width="16%" height="25" bgcolor="silver"><input type="text" name="key16" size="10" value="<%=dspkey(16)%>" maxlength="5"<%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListdata" readonly></td>                                 
      </tr>
      <tr>                                 
        <td width="15%" class="dataListHead" height="25">戶籍地址</td>                                 
        <td width="60%" colspan="3" height="25" bgcolor="silver">
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false))  Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(17))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if    
       s=s &"<option value=""" &"""" &sx &">(縣市別)</option>"        
       sxx18=" onclick=""Srcounty18onclick()"" "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(17) & "' " 
       sxx18=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(17) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>        
        <select name="key17" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" style="text-align:left;" maxlength="8" class="dataListEntry"><%=s%></select>
        <input type="text" name="key18" size="8" value="<%=dspkey(18)%>" maxlength="10" readonly <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"><font size=2>(鄉鎮市區)                 
         <input type="button" id="B18"  name="B18"   width="100%" style="Z-INDEX: 1"  value="..." <%=sxx18%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C18"  name="C18"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >     
        <input type="text" name="key19" size="30" value="<%=dspkey(19)%>" maxlength="60" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                     
        <td width="8%" class="dataListHead" height="25">郵遞區號</td>                                 
        <td width="16%" height="25" bgcolor="silver"><input type="text" name="key20" size="10" value="<%=dspkey(20)%>" maxlength="5" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListdata" readonly></td>                                 
      </tr>                                
      <tr>          
<script language="vbscript">
Sub SrAddrEqual()
  Dim i,objOpt
  document.All("key13").value=document.All("key9").value
  document.All("key14").value=document.All("key10").value
  document.All("key15").value=document.All("key11").value
  document.All("key16").value=document.All("key12").value
End Sub 
Sub SrAddrEqual2()
  document.All("key17").value=document.All("key9").value
  document.All("key18").value=document.All("key10").value
  document.All("key19").value=document.All("key11").value
  document.All("key20").value=document.All("key12").value
End Sub 
Sub SrAddUsr()
  ExistUsr=document.all("key71").value
  InsType=cstr(document.all("key70").value)
  UsrStr=Window.showModalDialog("RTCustAddUsr.asp?parm=" & existusr & "@" & instype   ,"Dialog","dialogWidth:410px;dialogHeight:400px;")
  if UsrStr<>False then
     UsrStrAry=split(UsrStr,"@")
     document.all("key71").value=UsrStrAry(0)
     document.all("REF01").value=UsrStrAry(1)     
  end if
End Sub

Sub Srpay()
  if document.all("key70").value = "1" then
     document.all("key75").value = 200
  else
     document.all("key75").value = 0
  end if
end sub
</script>                       
        <td width="35%" class="dataListHead" colspan="2" height="34" bgcolor="silver">
<%  dim seld1
    if len(trim(dspkey(93))) =0 and dspkey(67) <> "Y" then
       If Len(Trim(FIELDROLE(1) &dataProtect)) < 1 Then
          seld1=""
       Else
          seld1=" disabled "
       End If
    else
        seld1=" disabled "
    end if
    %>
            <input type="radio" name="rdo1" value="1"<%=seld1%><%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> 
                   onClick="SrAddrEqual()">裝機地址同帳單地址
            <input type="radio" name="rdo2" value="1"<%=seld1%><%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> 
                   onClick="SrAddrEqual2()">戶籍地址同帳單地址</td>                                 
        <td width="8%" class="dataListHead" height="23">申請速度</td>
<% aryOption=Array("512/64Kbps")
   s=""
   If Len(Trim(FIELDROLE(1) &dataProtect)) < 1 Then 
      For i = 0 To Ubound(aryOption)
          If dspKey(21)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(21) &""">" &dspKey(21) &"</option>"
   End If%>                                      
        <td width="16%" height="23" bgcolor="silver"><select size="1" name="key21" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">                                                             
        <%=s%></select></td>      
        <td width="8%" class="dataListHead" height="25">線路種類</td>
<% aryOption=Array("ADSL")
   s=""
   If Len(Trim(FIELDROLE(1) &dataProtect)) < 1 Then   
      For i = 0 To Ubound(aryOption)
          If dspKey(22)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(22) &""">" &dspKey(22) &"</option>"
   End If%>                                  
        <td width="16%" height="25" bgcolor="silver"><select size="1" name="key22" style="font-family: 新細明體; font-size: 10pt"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">                                                                  
        <%=s%></select></td>                                     
      </tr>                                 
      <tr>                            
        <td width="15%" class="dataListHead" height="25">申請方案</td>
 <td width="30%" height="25" bgcolor="silver">
<%
'    s=""
'    sx=" selected "
'    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) and len(trim(dspkey(93)))=0 Then 
'       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='B9' " 
      ' s=s &"<option value=""" &"""" &sx &"></option>"
'    Else
'       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='B9' AND CODE='" & dspkey(23) & "'"
'       sx=" selected "
'       s=s &"<option value=""" &"""" &sx &"></option>"
'    End If
'    rs.Open sql,conn
'    Do While Not rs.Eof
'       If rs("CODE")=dspkey(23) Then sx=" selected "
'       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
'       rs.MoveNext
'       sx=""
'    Loop
'    rs.Close
%>                
    <%  
      aryOption=Array("","券商專案")
      aryOptionV=Array("","01")   
  ' end if
   s=""
   If Len(Trim(fieldPa &fieldRole(1) &dataProtect)) < 1 and len(trim(dspkey(93)))> 0 Then
      'if aryoptionV(trim(dspkey(23)))="" then
      if trim(dspkey(23))="" then  
         J=0
      else
         J=1
      end if
      s="<option value=""" &dspKey(23) &""">" &aryOption(j) &"</option>"
      SXX=""
   Else
      For i = 0 To Ubound(aryOption)
          If dspKey(23)=aryOptionV(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOptionV(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   End If%>                 
   <select size="1" name="key23" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">                                            
        <%=s%>
   </select>
<% aryOption=Array("經濟型","單機型","商業型")
   s=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 and len(trim(dspkey(93)))=0 Then 
      For i = 0 To Ubound(aryOption)
          If dspKey(24)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(24) &""">" &dspKey(24) &"</option>"
   End If%>               
   <select size="1" name="key24" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">                                            
        <%=s%>
   </select><font size=2>
   ＋</font>
<% aryOption=Array("","先看先贏")
   s=""
   sx=""
   If Len(Trim(fieldRole(1) &dataProtect)) < 1 and len(trim(dspkey(93)))=0 Then 
      For i = 0 To Ubound(aryOption)
          If dspKey(79)=aryOption(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
   Else
      s="<option value=""" &dspKey(79) &""">" &dspKey(79) &"</option>"
   End If%>                   
   <select size="1" name="key79" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">                                            
        <%=s%>
   </select>   
   </td>                                     
        <td width="8%" class="dataListHead">意願表日期</td>                     
        <td width="16%"  bgcolor="silver">
          <input type="text" name="key25" size="10" value="<%=dspKey(25)%>"  <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%> class="dataListEntry" maxlength="10" >
          <input type="button" id="B25"  name="B25" height="100%" width="100%" style="Z-INDEX: 1" value="..."  <%=fieldpc%>></td> 
        <td width="8%"  class="dataListHead" height="34">回覆日期</td>                                 
        <td width="16%"  height="34" bgcolor="silver"><input type="text" name="key80" size="10" value="<%=dspKey(80)%>" maxlength="10" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
                  <input type="button" id="B80"  name="B80" height="100%" width="100%" style="Z-INDEX: 1" value="..."  <%=fieldpc%>></td>       
      </tr>                     
      <tr>                            
        <td width="15%" class="dataListHead" height="25">堪查日期</td>
         <td width="30%" height="25" bgcolor="silver">       
          <input type="text" name="key81" size="10" value="<%=dspKey(81)%>"  <%=fieldpg%><%=fieldpa%><%=FIELDROLE(3)%> class="dataListEntry" maxlength="10" >
          <input type="button" id="B81"  name="B81" height="100%" width="100%" style="Z-INDEX: 1" value="..."  <%=fieldpc%>>
          <%  dim rdo1, rdo2
              if len(trim(dspkey(93))) =0 and dspkey(67) <> "Y" then
                 If Len(Trim(fieldRole(3) &dataProtect)) < 1 Then
                    rdo1=""
                    rdo2=""
                 Else
                    rdo1=" disabled "
                    rdo2=" disabled "
                 end if
              else
                 rdo1=" disabled "
                 rdo2=" disabled "
              End If
             ' If Trim(dspKey(85))="" Then dspKey()="Y"
              If trim(dspKey(85))="Y" Then 
                 rdo1=" checked "    
              elseIf trim(dspKey(85))="N" Then 
                 rdo2=" checked " 
              end if
             %>
        <input type="radio" value="Y" <%=rdo1%> name="key85" <%=fieldpg%><%=fieldRole(3)%><%=dataProtect%>><font size=2>可建置
        <input type="radio" value="N" <%=rdo2%>  name="key85" <%=fieldpg%><%=fieldRole(3)%><%=dataProtect%>><font size=2>無法建置
          </td> 
        <td width="8%" class="dataListHead" height="25">堪查結果</td>
         <td width="16%"  height="25" bgcolor="silver">       
         <% aryOption=Array("","有電信室","無電信室","無電信箱")
            s=""
            If Len(Trim(fieldRole(3) &dataProtect)) < 1 and len(trim(dspkey(93)))=0 Then 
               For i = 0 To Ubound(aryOption)
                   If dspKey(86)=aryOption(i) Then
                      sx=" selected "
                   Else
                      sx=""
                   End If
                   s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
               Next
            Else
                   s="<option value=""" &dspKey(86) &""">" &dspKey(86) &"</option>"
            End If%>               
         <select size="1" name="key86" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(3)%><%=dataProtect%> class="dataListEntry">                                            
           <%=s%>
         </select>
         <% aryOption=Array("","跨棟","獨棟","雙拼")
            s=""
            If Len(Trim(fieldRole(3) &dataProtect)) < 1 and len(trim(dspkey(93)))=0 Then 
               For i = 0 To Ubound(aryOption)
                   If dspKey(87)=aryOption(i) Then
                      sx=" selected "
                   Else
                      sx=""
                   End If
                   s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
               Next
            Else
                   s="<option value=""" &dspKey(87) &""">" &dspKey(87) &"</option>"
            End If%>               
         <select size="1" name="key87" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(3)%><%=dataProtect%> class="dataListEntry">                                            
           <%=s%>
         </select>         
         </td>
          <td width="8%" class="dataListHead">正式申請日</td>                     
          <td width="16%"  bgcolor="silver">
          <input type="text" name="key82" size="10" value="<%=dspKey(82)%>"  <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%> class="dataListEntry" maxlength="10" >
          <input type="button" id="B82"  name="B82" height="100%" width="100%" style="Z-INDEX: 1" value="..."  <%=fieldpc%>></td> 
      </tr>
      <tr>
        <td width="15%"  class="dataListHead" height="34">堪查補充說明</td>  
        <td width="30%"  colspan="3" height="21" bgcolor="silver">
        <input type="text" name="key88" style="text-align:left;" maxlength="300" size="60"
               value="<%=dspKey(88)%>"<%=FIELDROLE(3)%> class=dataListentry style="color:red">
        </td>
        <td width="8%"   bgcolor="ORANGE"  height="34">送件日期</td>                                 
        <td width="16%"  height="34" bgcolor="silver">
          <input type="text" name="key83" size="10" value="<%=dspKey(83)%>" maxlength="10" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
          <input type="button" id="B83"  name="B83" height="100%" width="100%" style="Z-INDEX: 1" value="..."  <%=fieldpc%>></td>       
      </tr>            
      <tr style="display:none">
        <td width="15%"  bgcolor="ORANGE"  height="34">CHT簽核日期</td>  
        <td width="30%"  height="21" bgcolor="silver">
        <input type="text" name="key89" style="text-align:left;" maxlength="10" size="10"
               value="<%=dspKey(89)%>" class=dataListentry >
          <input type="button" id="B89"  name="B89" height="100%" width="100%" style="Z-INDEX: 1" value="..."  <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=fieldpc%>>               
        </td>
        <td width="8%"   bgcolor="ORANGE" height="34">送營運處日期</td>                                 
        <td width="16%"  height="34" bgcolor="silver">
          <input type="text" name="key90" size="10" value="<%=dspKey(90)%>" maxlength="10" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
          <input type="button" id="B90"  name="B90" height="100%" width="100%" style="Z-INDEX: 1" value="..."  <%=fieldpc%>></td>       
        <td width="8%"   bgcolor="ORANGE" height="34">取得附掛電話日</td>                                 
        <td width="16%"  height="34" bgcolor="silver">
          <input type="text" name="key91" size="10" value="<%=dspKey(91)%>" maxlength="10" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
          <input type="button" id="B91"  name="B91" height="100%" width="100%" style="Z-INDEX: 1" value="..."  <%=fieldpc%>></td>                 
      </tr>                                            
      <tr>                                    
        <td width="15%" class="dataListHead" height="21">住宅種類</td>                                    
        <td width="30%"  colspan="3" height="21" bgcolor="silver">
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) and len(trim(dspkey(93)))=0 Then 
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='C2' " 
       If len(trim(dspkey(26))) < 1 Then
          sx=" selected " 
        '  s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
        '  s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       end if     
    Else
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='C2' AND CODE='" & dspkey(26) & "'"
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(26) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>         
   <select size="1" name="key26" style="font-family: 新細明體; font-size: 10pt"<%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">                                                                  
        <%=s%>
   </select><font size=2>
   &nbsp;社區名稱<input type="text" name="key27" size="15" MAXLENGTH="30" value="<%=dspKey(27)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(3)%><%=dataProtect%> readonly class="dataListEntry">
        <!--
        <input type="button" id="B26"  name="B26"   width="100%" style="Z-INDEX: 1"  value="..." <%=fieldpf%>  >
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C26"  name="C26"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >        
          -->
   共<input type="text" name="key28" size="4" value="<%=dspKey(28)%>" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> maxlength="4" class="dataListEntry">戶</td>                                 
              <td width="8%"  class="dataListHead" height="34">附掛電話</td>                                 
      <td width="16%"  height="34" bgcolor="silver"><input type="text" name="key29" size="15" value="<%=dspKey(29)%>" maxlength="10" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>    
      </tr>                                 
      <tr>                                    
        <td width="15%" class="dataListHead" height="23">聯絡電話</td>                                 
        <td width="30%" height="23"><input type="text" name="key30" size="15" value="<%=dspkey(30)%>" maxlength="15" <%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" class="dataListHead" height="23">傳真電話</td>                                 
        <td width="16%" height="23" bgcolor="silver"><input type="text" name="key31" size="15" value="<%=dspkey(31)%>" maxlength="15" <%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" class="dataListHead" height="23">聯絡人</td>                                 
        <td width="16%" height="23" bgcolor="silver"><input type="text" name="key32" size="10" value="<%=dspkey(32)%>" maxlength="20" <%=dataProtect%> class="dataListEntry"></td>                                 
      </tr>                                 
      <tr>                                 
        <td width="15%" class="dataListHead" height="23" bgcolor="silver">公司電話</td>                                 
        <td width="30%" height="23"><input type="text" name="key33" size="15" value="<%=dspkey(33)%>" maxlength="15" <%=dataProtect%> class="dataListEntry">
        <font size=2>分機<input type="text" name="key34" size="5" value="<%=dspkey(34)%>" maxlength="5" <%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" class="dataListHead" height="23">行動電話</td>                                 
        <td width="16%"  height="23" bgcolor="silver"><input type="text" name="key35" size="15" value="<%=dspkey(35)%>" maxlength="15" <%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" height="23" class="dataListHead" >申請代表人</td>                     
        <td width="16%" height="23" bgcolor="silver">
        <%  dim OPT1, OPT2
            if len(trim(dspkey(93))) =0 and dspkey(67) <> "Y" then
               If Len(Trim(FIELDROLE(1) &dataProtect)) < 1 Then
                  OPT1=""
                  OPT2=""
               Else
                  OPT1=" disabled "
                  OPT2=" disabled "
               end if
            else
               OPT1=" disabled "
               OPT2=" disabled "
            End If
            If dspKey(95)="Y" Then OPT1=" checked "    
            If dspKey(95)="N" Then OPT2=" checked " %>                          
        
        <input type="radio" value="Y" <%=OPT1%> name="key95" <%=fieldpg%><%=fieldpa%><%=dataProtec%>><font size=2>是</font>
        <input type="radio" value="N" <%=OPT2%> name="key95" <%=fieldpg%><%=fieldpa%><%=dataProtect%>><font size=2>否</font></td>                              
            
      </tr>                                 
      <tr>                                 
        <td width="15%" class="dataListHead" height="25">電子郵件信箱</td>                                 
        <td width="30%" height="25" bgcolor="silver"><input type="text" name="key36" size="30" value="<%=dspkey(36)%>" maxlength="30" <%=dataProtect%> class="dataListEntry"></td>                                 
        <td width="8%" class="dataListHead" height="23">收據名稱</td>                                 
        <td width="16%" colspan="3" height="23" bgcolor="silver"><input type="text" name="key37" size="15" value="<%=dspkey(37)%>" maxlength="50" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
        <font size=2>(空白時預設為客戶名稱)</font></td>                   
      </tr>                                 
      <tr>                                 
        <td width="15%" class="dataListHead" height="23" style="display:none">輸入人員</td>                                 
        <td width="30%" height="23" bgcolor="silver" style="display:none"><input type="text" name="key38" size="10" class="dataListData" value="<%=dspKey(38)%>" readonly style="display:none"><%=EusrNc%></td>                                 
        <td width="8%" class="dataListHead" height="23" style="display:none">輸入日期</td>                                 
        <td width="40%" colspan="3" height="23" bgcolor="silver" style="display:none"><input type="text" name="key39" size="15" class="dataListData" value="<%=dspKey(39)%>" readonly style="display:none"></td>                                 
      </tr>                                 
      <tr>                                 
        <td width="15%" class="dataListHead" height="23" style="display:none">異動人員</td>                                 
        <td width="30%" height="23" bgcolor="silver" style="display:none"><input type="text" name="key40" size="10" class="dataListData" value="<%=dspKey(40)%>" readonly style="display:none"><%=UUsrNc%></td>                                 
        <td width="8%" class="dataListHead" height="23" style="display:none">異動日期</td>                                 
        <td width="40%" colspan="3" height="23" bgcolor="silver" style="display:none"><input type="text" name="key41" size="15" class="dataListData" value="<%=dspKey(41)%>" readonly style="display:none"></td>                                 
      </tr>
    </table>                            
    <table border="1" width="100%" cellpadding="0" cellspacing="0" id="tag2" style="display: none">                           
      <tr>                         
        <td width="15%" class="dataListHead">施工廠商</td>                     
        <td width="15%" bgcolor="silver">
<%
    If (sw="E" Or (accessMode="A" And sw="")) And Len(Trim(fieldPa &fieldPb &fieldRole(1) &dataProtect))<1 Then 
       sql="SELECT RTSuppCty.CUSID, RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN " _
          &"RTSuppCty ON RTObj.CUSID = RTSuppCty.CUSID INNER JOIN " _
          &"RTObjLink ON RTObj.CUSID = RTObjLink.CUSID RIGHT OUTER JOIN " _
          &"RTcustadsl ON RTSuppCty.CUTID = RTCustadsl.CUTID2 " _
          &"WHERE (RTObjLink.CUSTYID = '04') and rtcustadsl.comq1='" & dspkey(0) & "'" _
          &"GROUP BY  RTSuppCty.CUSID, RTObj.SHORTNC "
    Else
       sql="SELECT RTObj.CUSID, RTObj.SHORTNC " _
          &"FROM RTObj INNER JOIN RTSupp ON RTObj.CUSID = RTSupp.CUSID " _
          &"WHERE RTSupp.CUSID='" &dspKey(42) &"' "
    End If
   ' Response.Write "SQL=" & SQL & "<BR>"
    rs.Open sql,conn
    s=""
    If rs.Eof Then 
       s="<option value="""" selected>&nbsp;</option>"
    else
       sx=""
       s="<option value="""">&nbsp;</option>" & vbcrlf      
       Do While Not rs.Eof
          If rs("CusID")=dspKey(42) Then sx=" selected "
          s=s &"<option value=""" &rs("CusID") &"""" &sx &">" &rs("SHORTNC") &"</option>" & vbcrlf
          rs.MoveNext
          sx=""
       Loop
    end if
    rs.Close
%>
        <select name="key42" <%=fieldRole(1)%><%=dataProtect%><%=fieldpg%><%=fieldPa%><%=fieldPb%> size="1"    
               style="text-align:left;" maxlength="8" class="dataListEntry"><%=s%></select></td> 
        <td width="15%" class="dataListHead">通知發包日期</td>                     
        <td width="15%" colspan="1" bgcolor="silver">
          <input type="text" name="key43" size="10" value="<%=dspKey(43)%>" readonly <%=fieldpg%><%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%> class="dataListdata" maxlength="10"></td>                                               
        <td width="10%" class="dataListHead">發包日期</td>                     
        <td width="15%" colspan="1" bgcolor="silver">
          <input type="text" name="key44" size="10" value="<%=dspKey(44)%>" <%=fieldpg%><%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%> class="dataListEntry" maxlength="10">
          <input type="button" id="B44"  name="B44" height="100%" width="100%" style="Z-INDEX: 1" value="..." <%=fieldpc%>>          </td>                   
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">安裝表批號</td>                    
        <td width="15%" bgcolor="silver"><input type="text" name="key45" size="10" class="dataListData" value="<%=dspKey(45)%>" readonly></td>                     
        <td width="15%" class="dataListHead">安裝表列印日</td>                     
        <td width="15%" bgcolor="silver"><input type="text" name="key46" size="10" class="dataListData" value="<%=dspKey(46)%>" readonly></td>                     
        <td width="10%" class="dataListHead">列印人員</td>                     
        <td width="15%" bgcolor="silver"><input type="text" name="key47" size="10" class="dataListData" value="<%=dspKey(47)%>" readonly></td>                   
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">完工日期</td>                    
        <td width="15%" bgcolor="silver">
          <input type="text" name="key48" size="10" value="<%=dspKey(48)%>" <%=fieldpg%><%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="10"></td>                     
        <td width="15%" class="dataListHead">報竣日期</td>   
        <td width="15%" bgcolor="silver">                  
         <input type="text" name="key49" size="10" value="<%=dspKey(49)%>" <%=fieldpg%><%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="10">
          <input type="button" id="B49"  name="B49" height="100%" width="100%" style="Z-INDEX: 1" value="..." <%=fieldpc%>></td>                     
        <td width="10%" class="dataListHead">入帳日期</td>                     
        <td width="15%">
          <input type="text" name="key50" size="10" value="<%=dspKey(50)%>"   class="dataListdata" readonly maxlength="10">
          <input type="button" id="B50"  name="B50" height="100%" width="100%" style="Z-INDEX: 1" value="..." <%=fieldpc%>></td>                   
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">應收金額</td>                    
        <td width="15%" bgcolor="silver">
          <input type="text" name="key51" size="10" value="<%=dspKey(51)%>" <%=fieldpg%><%=fieldPa%><%=fieldPb%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" maxlength="10"></td>                     
        <td width="15%" class="dataListHead">實收金額</td>                     
        <td width="15%" bgcolor="silver">
        <input type="text" name="key52" size="10" value="<%=dspKey(52)%>" <%=fieldpg%><%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="10"></td>                     
        <td width="10%" class="dataListHead">撤銷日期</td>                     
        <td width="15%" bgcolor="silver">
          <input type="text" name="key53" size="10" value="<%=dspKey(53)%>" <%=fieldPa%><%=fieldPb%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" maxlength="10" >
          <input type="button" id="B53"  name="B53" height="100%" width="100%" style="Z-INDEX: 1" value="..." <%=fieldpc%>>
          欠拆︰<input type="text" name="key98" size="1" maxlength="1" value="<%=dspkey(98)%>" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td>                   
      </tr>
      <tr>
        <td width="14%" class="dataListHead">復裝日期</td>
        <td width="18%" bgcolor="silver">
          <input type="text" name="key105" size="10" value="<%=dspKey(105)%>" <%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" maxlength="10">
          <input type="button" id="B105" name="B105" height="100%" width="100%" style="Z-INDEX: 1" value="...." ONCLICK="SRBTNONCLICK"></td>
		<td width="14%" class="dataListHead">已執行復裝</td>
        <td width="18%" bgcolor="silver">		
          <input type="text" name="key106" size="1" maxlength="2" value="<%=dspkey(106)%>" <%=fieldRole(1)%> class="dataListEntry"></td>
        <td width="20%" class="dataListHead">&nbsp;</td>                    
        <td width="18%" bgcolor="silver">&nbsp;</td>
      </tr>
      <tr>
        <td width="20%" class="dataListHead">保證金</td>                    
        <td width="18%" bgcolor="silver">
          <input type="text" name="key107" size="10" value="<%=dspKey(107)%>" <%=fieldPg%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" maxlength="10"></td>
<%
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) Then     
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='L6' " 
       If len(trim(dspkey(108))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='L6' AND CODE='" & dspkey(108) &"' " 
    End If
    rs.Open sql,conn
    s=""
    s=s &"<option value=""" &"""" &sx &"></option>"
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(108) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>
        <td width="20%" class="dataListHead">保證金年限</td>                    
        <td width="18%" bgcolor="silver">
          <select name="key108" <%=fieldRole(1)%><%=dataProtect%><%=fieldpg%><%=fieldPg%> size="1"
                  maxlength="8" class="dataListEntry"><%=s%></select></td>
        <td width="20%" class="dataListHead">保證金序號</td>
        <td width="18%" bgcolor="silver">
          <input type="text" name="key109" size="15" value="<%=dspKey(109)%>" <%=fieldPg%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" maxlength="15"></td>
	  </tr>      
      <tr>                       
        <td width="15%" class="dataListHead">收款表批號</td>                    
        <td width="15%" bgcolor="silver"><input type="text" name="key54" size="10" class="dataListData" value="<%=dspKey(54)%>" readonly></td>                     
        <td width="15%" class="dataListHead">列印人員</td>                     
        <td width="15%" bgcolor="silver"><input type="text" name="key55" size="10" class="dataListData" value="<%=dspKey(55)%>" readonly></td>                     
        <td width="10%" class="dataListHead">收款日期</td>                     
        <td width="15%" bgcolor="silver">
         <input type="text" name="key56" size="10" value="<%=dspKey(56)%>" <%=fieldpg%><%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry" maxlength="10">
          <input type="button" id="B56"  name="B56" height="100%" width="100%" style="Z-INDEX: 1" value="..." <%=fieldpc%>></td>                   
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">財務收款確認日</td>                    
        <td width="15%" bgcolor="silver"><input type="text" name="key57" size="10" class="dataListData" value="<%=dspKey(57)%>" readonly></td>                     
        <td width="15%" class="dataListHead">財務確認人員</td>                     
        <td width="15%" bgcolor="silver"><input type="text" name="key58" size="10" class="dataListData" value="<%=dspKey(58)%>" readonly></td>                     
        <td width="10%" class="dataListHead">獎金計算日期</td>                     
        <td width="15%" bgcolor="silver">
          <input type="text" name="key59" size="10" value="<%=dspKey(59)%>" readonly  class="dataListdata" maxlength="10"></td>                   
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">撤銷原因說明</td>                    
        <td width="15%" colspan="5" bgcolor="silver">
          <input type="text" name="key60" size="70" value="<%=dspKey(60)%>" <%=fieldPa%><%=dataProtect%> class="dataListEntry" maxlength="50"></td>                     
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">未完工原因</td>                    
        <td width="15%" colspan="5" bgcolor="silver">
          <input type="text" name="key61" size="70" value="<%=dspKey(61)%>" <%=fieldPa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="50"></td>                     
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">付款表批號</td>                    
        <td width="15%" bgcolor="silver"><input type="text" name="key62" size="10" class="dataListData" value="<%=dspKey(62)%>" readonly></td>                     
        <td width="15%" class="dataListHead">付款表日期</td>                     
        <td width="15%" bgcolor="silver"><input type="text" name="key63" size="10" class="dataListData" value="<%=dspKey(63)%>" readonly></td>                     
        <td width="10%" class="dataListHead">列印人員</td>                     
        <td width="15%" bgcolor="silver"><input type="text" name="key64" size="10" class="dataListData" value="<%=dspKey(64)%>" readonly></td>                   
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">付款會計審核確認日</td>                    
        <td width="15%" bgcolor="silver"><input type="text" name="key65" size="10" class="dataListData" value="<%=dspKey(65)%>" readonly></td>                     
        <td width="15%" class="dataListHead">會計審核人員</td>                     
        <td width="15%" bgcolor="silver"><input type="text" name="key66" size="10" class="dataListData" value="<%=dspKey(66)%>" readonly></td>                     
        <td width="10%" class="dataListHead">結案碼</td>                     
        <td width="15%" bgcolor="silver"><input type="text" name="key67" size="10" class="dataListData" value="<%=dspKey(67)%>" readonly></td>                   
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">施工備註說明</td>                    
        <td width="15%" colspan="5" bgcolor="silver">
          <input type="text" name="key68" size="70" value="<%=dspKey(68)%>" <%=fieldPa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="50"></td>                     
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">施工環境代碼</td>                    
        <td width="15%" bgcolor="silver">
        <%
    If (sw="E" Or (accessMode="A" And sw="")) And Len(Trim(fieldPa  &FIELDROLE(1) &dataProtect))<1 and len(trim(dspkey(93)))=0 Then 
       sql="SELECT code, codenc " _
          &"FROM RTcode where kind='C4' " 
    Else
       sql="SELECT code, codenc " _
          &"FROM RTcode where kind='C4' and code='" &dspKey(69) &"' "
    End If
    rs.Open sql,conn
    s=""
    If rs.Eof Then s="<option value="""" selected>&nbsp;</option>"
    sx=""
    Do While Not rs.Eof
       If rs("code")=dspKey(69) Then sx=" selected "
       s=s &"<option value=""" &rs("code") &"""" &sx &">" &rs("codenc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>
        <select name="key69" <%=FIELDROLE(1)%><%=dataProtect%><%=fieldpg%><%=fieldPa%> size="1"    
               style="text-align:left;" maxlength="8" class="dataListEntry"><%=s%></select>

        <td width="15%" class="dataListHead">安裝員類別</td>
<%' if userlevel=1 then
  '    aryOption=Array("","業務自行安裝","技術部安裝")      
  '    aryOptionV=Array("0","1","2")
  ' elseif userlevel=4 then
  '    aryOption=Array("","技術部安裝","發包")
  '    aryOptionV=Array("0","2","3")
  ' elseif userlevel=31 then
      aryOption=Array("","業務自行安裝","技術部安裝","發包")
      aryOptionV=Array("0","1","2","3")   
  ' end if
   s=""
   If Len(Trim(fieldPa &fieldRole(1) &dataProtect)) > 0 or len(trim(dspkey(93)))> 0 Then
      s="<option value=""" &dspKey(70) &""">" &aryOption(dspKey(70)) &"</option>"
      SXX=""
   Else
      For i = 0 To Ubound(aryOption)
          If dspKey(70)=aryOptionV(i) Then
             sx=" selected "
          Else
             sx=""
          End If
          s=s &"<option value=""" &aryOptionV(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next
      sxx=" onclick=""SrAddUsr()"" "
   End If%>                    
        <td width="15%" bgcolor="silver"><select size="1" onChange="Srpay()" name="key70" <%=fieldpg%><%=fieldPa%><%=fieldPb%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry">
          <%=s%></select></td>                     
        <td width="10%" class="dataListHead">
        <input type="button" name="EMPLOY" <%=fieldpg%><%=fieldPa%><%=fieldPb%> class=keyListButton <%=SXX%> value="裝機員工"></td>                     
        <td width="15%" bgcolor="silver">
  <% 
    Usrary=split(dspkey(71),";")
    qrystrng=""
    s1=""
    existusr=""
    if Ubound(Usrary) >= 0 then
       existUsr="("
       for i=0 to Ubound(usrary)
           existUsr=existUsr & "'" & usrary(i) & "',"
       next
       existUsr=mid(existUsr,1,len(existUsr)-1)
       existUsr=existUsr & ")"
       qrystring=" and rtemployee.emply in " & existusr
    end if
    if len(trim(qrystring)) < 1 then
       qrystring=" and rtemployee.emply='*' "
    end if
    sql="SELECT RTEmployee.emply, RTObj.CUSNC " _
          &"FROM RTEmployee INNER JOIN " _
          &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
          &"RTObjLink ON RTEmployee.CUSID = RTObjLink.CUSID AND rtobjlink.custyid = '08'" _
          & qrystring _
          &" order by cusnc "
    rs.Open sql,conn
    Do While Not rs.Eof
       s1=s1 & rs("cusnc") & ";"
       rs.MoveNext
    Loop
    if trim(len(s1)) > 0 then 
       s1=mid(s1,1,len(s1)-1)
    else
       dspkey(71)=""
       s1=""
    end if 
    rs.Close
    conn.Close   
    set rs=Nothing   
    set conn=Nothing
   %>       
          <input type="text" name="key71" size="14" value="<%=dspKey(71)%>"  class="dataListData"  readonly maxlength="50" style="display:none">
          <input type="text" name="ref01" size="10" value="<%=S1%>"  class="dataListData"  readonly maxlength="50">
          </td>                   
      </tr>                                     
      <tr>            
        <td width="15%" class="dataListHead">預定裝機日期</td>                    
        <td width="15%" bgcolor="silver">
          <input type="text" name="key72" size="10" value="<%=dspKey(72)%>" <%=fieldpg%><%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" maxlength="10">
          <input type="button" id="B72"  name="B72" height="100%" width="100%" style="Z-INDEX: 1" value="..." <%=fieldpc%>></td>                     
        <td width="15%" class="dataListHead">預定裝機時間(時)</td>                     
        <td width="15%" bgcolor="silver">
          <input type="text" name="key73" size="10" value="<%=dspKey(73)%>" <%=fieldpg%><%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" maxlength="2"></td>                     
        <td width="10%" class="dataListHead">預定裝機時間(分)</td>                     
        <td width="15%" bgcolor="silver">
          <input type="text" name="key74" size="10" value="<%=dspKey(74)%>" <%=fieldpg%><%=fieldPa%><%=fieldRole(1)%><%=dataProtect%> class="dataListEntry" maxlength="2"></td>                   
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">標準施工費</td>                    
        <td width="15%" bgcolor="silver">
        <input type="text" name="key75" size="10" class="dataListData" value="<%=dspKey(75)%>" readonly ></td>                     
        <td width="15%" class="dataListHead">施工補助費</td>                     
        <td width="15%" bgcolor="silver">
        <input type="text" name="key76" size="10" value="<%=dspKey(76)%>" <%=fieldpg%><%=fieldPa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="15"></td>                     
        <td width="15%" colspan="2">　</td>                     
      </tr>                                     
      <tr>                       
        <td width="15%" class="dataListHead">施工補助費說明</td>                    
        <td width="15%" colspan="5" bgcolor="silver">
          <input type="text" name="key77" size="70" value="<%=dspKey(77)%>" <%=fieldPa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry" maxlength="25"></td>                     
      </tr>                                     
    </table>
<table width="100%"><tr><td width="100%">&nbsp;</td></tr>                                                                                                   
  </div>                               
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrReadExtDB()
    Dim conn,rs
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open DSN
    Set rs=Server.CreateObject("ADODB.Recordset")
    rs.Open "SELECT * FROM RTObj WHERE CusID='" &dspKey(1) &"' ",conn
    extDB(0)=rs("CusNC")
   ' extDB(1)=rs("CutID1")
   ' extDB(2)=rs("TownShip1")
   ' extDB(3)=rs("RAddr1")
   ' extDB(4)=rs("RZone1")
   ' extDB(5)=rs("CutID2")
   ' extDB(6)=rs("TownShip2")
   ' extDB(7)=rs("RAddr2")
   ' extDB(8)=rs("RZone2")
    rs.Close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrSaveExtDB(Smode)
    Dim conn,rs
' Smode A:add U:update
' extDBField = n
' use extDB(i) for Screen ,and map it to DataBase
'
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open DSN
    Set rs=Server.CreateObject("ADODB.Recordset")
'------ RTObj ---------------------------------------------------
    rs.Open "SELECT * FROM RTObj WHERE CusID='" &dspKey(1) &"' ",conn,3,3
    If rs.Eof Or rs.Bof Then
       If Smode="A" Then
          rs.AddNew
          rs("CusID")=dspKey(1)
       End If
    End If

    rs("CusNC")=extDB(0)
    rs("ShortNC")=Left(extDB(0),5)
   ' rs("CutID1")=extDB(1)
   ' rs("TownShip1")=extDB(2)
   ' rs("RAddr1")=extDB(3)
   ' rs("RZone1")=extDB(4)
   ' rs("CutID2")=extDB(5)
   ' rs("TownShip2")=extDB(6)
   ' rs("RAddr2")=extDB(7)
   ' rs("RZone2")=extDB(8)
    rs("Eusr")=""
    rs("Edat")=now()
    rs("Uusr")=""
    rs("Udat")=now()
    rs.Update
    rs.Close
'------ RTObjLink -----------------------------------------------
    rs.Open "SELECT * FROM RTObjLink WHERE CustYID='05' AND CusID='" &dspKey(1) &"' ",conn,3,3
    'Response.Write RS.EOF
    If rs.Eof Or rs.Bof Then
       If Smode="A" Then
          rs.AddNew
          rs("CusID")=dspKey(1)
          rs("CustYID")="05"
       End If
    End If
    rs("Eusr")=""
    rs("Edat")=now()
    rs("Uusr")=""
    rs("Udat")=now()
    rs.Update
    rs.Close

    conn.Close
    Set rs=Nothing
    Set conn=Nothing
End Sub
' -------------------------------------------------------------------------------------------- 
%>
<!-- #include virtual="/Webap/include/checkid.inc" -->
<!-- #include virtual="/Webap/include/companyid.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<!-- #include file="RTGetUserRight.inc" -->
<!-- #include file="rtgetBRANCHBUSS.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include file="RTGetCmtyDesc.inc" -->