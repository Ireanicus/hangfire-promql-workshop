$servers = @("http://localhost:6080", "http://localhost:8080", "http://localhost:5080", "http://localhost:7080")
$endpoints = @("POST", "GET", "PUT", "DELETE")

while (1 -eq 1)
{
$random1 = Get-Random -Minimum 0 -Maximum 4
$random2 = Get-Random -Minimum 0 -Maximum 4


$uri = "$($servers[$random1])/error"
$uri
Invoke-RestMethod -Method $endpoints[$random2] -Uri $uri


Start-Sleep 1
}