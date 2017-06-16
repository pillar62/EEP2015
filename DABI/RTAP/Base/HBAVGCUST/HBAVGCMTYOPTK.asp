<%@ Language=VBScript %>
<%
keyary=split(request("key"),";")
'RESPONSE.Write "KEY=" & request("key")
'qry=split(session("search2"),";")
'response.Write "s1=" & qry(0) & ";" & qry(1)
select case keyary(0)
'AVS499
   case "AVS499"
  '    response.Redirect "/webap/rtap/base/rtcmty/rtcustk3.asp?key=" & KEYary(0)
      response.Redirect "/webap/rtap/base/HBAVGCUST/RTCMTYAVS499.asp"
'NCIC399 
   case "NCIC399"
  '    response.Redirect "/webap/rtap/base/rtADSLcmty/rtcustk3.asp?key=" & KEYary(0)
      response.Redirect "/webap/rtap/base/HBAVGCUST/RTCMTYNCIC399.asp"
'CHT599T1A(HB599固定制T1線)   
   case "CHT599T1A"
  '    response.Redirect "/webap/rtap/base/rtSPARQADSLcmty/rtcustk3.asp?key=" & KEYary(0)
      response.Redirect "/webap/rtap/base/HBAVGCUST/RTCMTYCHT599T1A.ASP"
'CHT599T1A(HB599計量制T1線)   
   case "CHT599T1B"
    '  response.Redirect "/webap/rtap/base/rtcmty/rtcustk2.asp?key=" & KEYary(0)
      response.Redirect "/webap/rtap/base/HBAVGCUST/RTCMTYCHT599T1B.ASP"
'CHT599ADSL(HB599ADSL線路)
   case "CHT599ADSL"
    '  response.Redirect "/webap/rtap/base/rtcmty/rtcustk2.asp?key=" & KEYary(0)
      response.Redirect "/webap/rtap/base/HBAVGCUST/RTCMTYCHT599ADSL.ASP"
'CHT399
   case "CHT399"
    '  response.Redirect "/webap/rtap/base/rtcmty/rtcustk2.asp?key=" & KEYary(0)
      response.Redirect "/webap/rtap/base/HBAVGCUST/RTCMTYCHT399.ASP"
end select
%>