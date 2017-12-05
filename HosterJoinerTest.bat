for /L %%I in (1, 1, 5) DO (
	start cmd /k "BZHeadlessClient.exe -c %%I -m 2"
 )

 for /L %%I in (10, 1, 20) DO (
	start cmd /k "BZHeadlessClient.exe -c %%I -m 3"
 )

pause