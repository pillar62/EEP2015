Set cmdArray = WScript.Arguments
If cmdArray.Count >=1 and cmdArray.Count <>0 Then
	conf_file = cmdArray(0)
Else
	WScript.Quit 99
End If

ScriptFullName = WScript.ScriptFullName
ScriptName = WScript.ScriptName
MainPath = Replace(ScriptFullName,ScriptName,"")

conf_file_path = MainPath & conf_file

Set fso=CreateObject("Scripting.FileSystemObject")

If Not fso.FileExists(conf_file_path) Then
	Wscript.echo "[" & conf_file & "] not found."
End If

Set sh = WScript.CreateObject("WScript.Shell")
sh.Run "cmd.exe"

Set f = fso.OpenTextFile(conf_file_path)
Do While f.AtEndOfStream <> True
	commandline = split(f.ReadLine(),";")
	If commandline(0)<>"" Then
		Dim strPingResults
		Set WshExec = sh.Exec("ping " & commandline(0))
		strPingResults = LCase(WshExec.StdOut.ReadAll)
		If InStr(strPingResults, "reply from") Then
			WScript.Sleep 1000
			sh.SendKeys "telnet{ENTER}"
			WScript.Sleep 1000
			sh.SendKeys "open " & commandline(0)
			WScript.Sleep 1000
			sh.SendKeys "{ENTER}"
			WScript.Sleep 1000
			sh.SendKeys commandline(1) & "{ENTER}"
			WScript.Sleep 1000
			sh.SendKeys "restart{ENTER}"
			WScript.Sleep 40000
			sh.SendKeys "{ENTER}"
			WScript.Sleep 1000
			sh.SendKeys "{ENTER}"
			WScript.Sleep 1000
'sh.SendKeys "@close{ENTER}" 
'WScript.Sleep 1000
'sh.SendKeys "{ENTER}"
'WScript.Sleep 1000
			sh.SendKeys "quit{ENTER}" 
		End If
	End If		
Loop
f.Close
Set fso = Nothing

WScript.Sleep 1000
sh.SendKeys "exit{ENTER}" 

