# REST API GeoInfo

This is a simple rest api allowing to fetch geographical information based on the ip provided and store it in the mssql db.

### ADD SECRETS FILES :<br />

content of the files:

- **connection.config:**<br />
	`<connectionStrings>`  <br />
		`<add name="DbConnectionString" providerName="System.Data.SqlClient"	connectionString="Server={serverName};Database={databaseName};User={user};Password={password}" />`  <br />
	`</connectionStrings>` <br />

- **C:/Secrets/secrets.xml:**<br />
	`<?xml version="1.0" encoding="utf-8"?>`<br />
	`<root>`<br />
	  `<secrets ver="1.0" >`<br />
		`<secret name="apiKey" value="{yourApiKey}" />`<br />
	  `</secrets>`<br />
	`</root>`<br />

You can get the apiKey from here => https://ipstack.com/ and replace it accordingly as well as connection strings 

## Run the app

    Open solution GeoIpApi.sln in Visual Studio 2019 and try to press F5

## Run the tests

    Go to GeoApiIp.Test/TestGeoInfo.cs and run the tests methods

## Run with docker

    Set docker-compose as tartup project and press F5

# REST API

The REST API documentation is availabe from Swagger. Simply run the solution and go to http://localhost:44325/swagger. You can send http requests from there.

## Live example

    https://geoipapi.azurewebsites.net/
