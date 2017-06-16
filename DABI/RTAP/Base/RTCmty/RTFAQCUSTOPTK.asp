<%@ Language=VBScript %>
<%
keyary=split(request("key"),";")
'RESPONSE.Write "K1=" & KEYARY(0) & ";K2=" & KEYARY(1) & ";K3=" & KEYARY(2) & ";K4=" & KEYARY(3) & ";K5=" & KEYARY(4)
Randomize
select case keyary(4)
'じ癟599
   case "HB599"
      response.Redirect "http://W3C.INTRA.CBBN.COM.TW/webap/rtap/base/rtcmty/rtFAQD.asp?V=" &RND() & "&accessMode=U" & "&key=" & KEYary(0)
'い地399 
   case "い地399"
      response.Redirect "http://W3C.INTRA.CBBN.COM.TW/webap/rtap/base/rtADSLcmty/rtFAQD.asp?V=" &RND() & "&accessMode=U" & "&key=" & KEYary(0)
'硉痴399   
   case "硉痴399"
      response.Redirect "http://W3C.INTRA.CBBN.COM.TW/webap/rtap/base/rtSPARQADSLcmty/rtFAQD.asp?V=" &RND() & "&accessMode=U" & "&key=" & KEYary(0)
'狥此499   
   case "狥此499"
    '  response.Redirect "http://w3c.intra.cbbn.com.tw/webap/rtap/base/rtcmty/rtcustk2.asp?key=" & KEYary(0)
      response.Redirect "http://W3C.INTRA.CBBN.COM.TW/webap/rtap/base/rtEBTcmty/rtFAQD.asp?V=" &RND() & "&accessMode=U" & "&key=" & KEYary(0)
'硉痴499   
   case "硉痴499"
      response.Redirect "http://W3C.INTRA.CBBN.COM.TW/webap/rtap/base/rtsparq499cmty/rtFAQD.asp?V=" &RND() & "&accessMode=U" & "&key=" & KEYary(0)
end select
%>