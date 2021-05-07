
::创建应用程序池
C:\Windows\System32\inetsrv\appcmd.exe add apppool /name:"%1" /managedRuntimeVersion:"v4.0"
 
:: 删除网站
::C:\Windows\System32\inetsrv\appcmd.exe delete site "%1"

::添加网址 设置端口号为12919，网址路径不能有中文
C:\Windows\System32\inetsrv\appcmd.exe add site /name:"%1" /bindings:"http/%2:80:www.%1,http/%2:80:%1,http/%3:80:www.%1,http/%3:80:%1"  /applicationDefaults.applicationPool:"%1" /physicalPath:"%4\%1" 

