@echo off
set /p id="Enter ID: "

dotnet ef migrations add %id% --startup-project ../Database.dummy

dotnet ef database update --startup-project ../Database.dummy