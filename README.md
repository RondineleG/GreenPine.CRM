# UintaPine
Simple CRM application, created with Blazor

Used a template or starting point for other similar CRM type applications.

To use this, clone the repo, and then set the MongoDB connection string and SigninKey in the AppSettings.json files. Do not commit the MongoDBConnectionString or SigningKey to source control, rather place them in your server configuration. Ignore changes to your AppSettings.json with these git commands: 
git update-index --assume-unchanged appsettings.json
git update-index --assume-unchanged appsettings.Development.json