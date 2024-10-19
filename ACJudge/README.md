Run this to create a container for the database:
```
sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=ACPassword@1234$" -p 1433:1433 --name ACJudge -d mcr.microsoft.com/mssql/server:latest
```
if you don't have a migration file in the project run
```
dotnet ef migrations add 'init' 
```
then run this command to update the database created, make sure you run the container 
```
dotnet ef database update
```

now you are ready to use the database in the docker.
