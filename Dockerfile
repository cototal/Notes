FROM node:lts AS assets
WORKDIR /src
COPY ./Notes.Assets .
RUN npm install
RUN npm run build

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY Notes.Web/ .
RUN dotnet restore "Notes.Web.csproj"
RUN dotnet build "Notes.Web.csproj" -c Release -o /app
RUN dotnet publish "Notes.Web.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim
ENV TZ=America/Chicago
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
WORKDIR /app
EXPOSE 80
COPY --from=build /app .
COPY --from=assets /src/dist ./wwwroot/assets
ENTRYPOINT ["dotnet", "Notes.Web.dll"]
