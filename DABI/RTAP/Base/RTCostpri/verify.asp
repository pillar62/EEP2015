<%@ Language=VBScript %>
<!-- #include virtual="/webap/include/lockright.inc" -->
<script language=vbscript>
sub window_onload()
    returncode=msgbox("請按[審核撤銷]入帳或[取消]按鈕結束",1,"收款金額審核")
    if returncode=vbok then
       msgbox "ok"
    else
       msgbox "cancel"
    end if
end sub

</script>
