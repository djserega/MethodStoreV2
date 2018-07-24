set DOTNETFX4=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX4%
echo ---------------------------------------------------
installutil.exe "%~dp0src\MethodStoreService\bin\Debug\MethodStoreService.exe" 
echo ---------------------------------------------------
pause