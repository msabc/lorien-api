# Lorien.Api

## About

Lorien.Api is .NET Web API for a company challenge. 

The project is a backend service which calls an external API in order to display prices of low cost flights.

You can visit the external API [here](https://developers.amadeus.com/self-service/category/flights/api-doc/flight-offers-search).

## Run the project

1. In order to run the project you first have to create an account and acquire the following credentials from the external API provider: 

 - API Key
 - API Secret

2. In order to do so you can follow the instructions [here](https://developers.amadeus.com/self-service/category/flights/api-doc/flight-offers-search). Once you have acquired the credentials, you will need to paste them in place of empty strings for these two parameters inside the **appsettings.Development.json** file.

3. Now you can start the app.