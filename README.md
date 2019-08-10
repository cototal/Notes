# Notes

Sample `appsettings.[env].json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "mongodb://user:password@localhost:27017"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  },
  "GoogleAuth": {
    "ClientId": "123.apps.googleusercontent.com",
    "ClientSecret": "secret"
  },
  "AdminEmails": "test@example.com;test1@example.com"
}
```
