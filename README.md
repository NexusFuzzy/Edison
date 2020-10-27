
<p align="center">
<img src="https://github.com/hariomenkel/Edison/raw/master/logo.png"/>
</p>

Tool to decrypt encrypted strings in AgentTesla version 2 and 3

Usage: eddy.exe TeslaSample.exe Output.txt <Version (Can be 2, 3 or 0 for Auto-Detect)

![alt text](https://github.com/hariomenkel/Edison/raw/master/screenshot.PNG)

If you are not interested in compiling it yourself just grab the binary from bin/debug! Please note that most samples are packed and before you can use this tool you should have the dumped sample. You may use [HollowsHunter](https://github.com/hasherezade/hollows_hunter).

It should be clear but this tool invokes (calls) methods of AgentTesla to extract strings. It is therefor strongly advised to run this tool within a secure virtual environment!

Another approach which might be cool is described here:

https://medium.com/@irshaduetian/decrypting-obfuscated-net-malware-strings-using-de4dot-emulation-6614c5a03dab
