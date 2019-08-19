# Notes

For development, you can set up an `appsettings.[env].json`:

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

For deployment, you can set up the secrets with environment variables using `docker-compose.yml`:

```yaml
version: "3.4"
services:
  app:
    container_name: notes
    image: notes
    ports:
      - 5000:80
    env_file:
      - notes.env
networks:
  default:
    external:
      name: services
```

With a `notes.env` file like:

```
AdminEmails=test@example.com;test1@example.com
GoogleAuth:ClientId=123.apps.googleusercontent.com
GoogleAuth:ClientSecret=secret
ConnectionStrings:DefaultConnection=mongodb://user:password@localhost:27017
```
