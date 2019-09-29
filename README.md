# Uinta Pine CRM
[![Build Status](https://dev.azure.com/dahln/UintaPine.CRM/_apis/build/status/Uinta%20Pine%20CRM%20CI%20Build?branchName=master)](https://dev.azure.com/dahln/UintaPine.CRM/_build/latest?definitionId=8&branchName=master)

Disclaimer: This is an on-going project. Web Assembly Blazor apps are still not fully support in .NET Core 3.0.

This is a Simple CRM application, created with the new Blazor framework. A simple company profile can be created, and customers associated with that company. Customers can have "tags" associated with them. Tags represent state/status/alerts/anything. Tag names are customizable. A company can have other authorized users, added via the settings panel.

To use this, clone the repo, and then set the MongoDB connection string and SigninKey in the AppSettings.json files. Do not commit the MongoDBConnectionString or SigningKey to source control, rather place them in your server configuration. Ignore changes to your AppSettings.json with these git commands: 

git update-index --assume-unchanged appsettings.json

git update-index --assume-unchanged appsettings.Development.json

