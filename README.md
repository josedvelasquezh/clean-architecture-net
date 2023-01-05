#Creation Container
docker pull mcr.microsoft.com/mssql/server:2022-latest

docker run --name=mssql-cleanarquitecture -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Querty123."  -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest

#Select proyect Data
add-migration MigrationInicial

update-database



add-migration agregar-tablas-entidades-relaciones
update-database