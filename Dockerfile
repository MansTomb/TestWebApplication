FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TestWebApplication/TestWebApplication.csproj", "TestWebApplication/"]
RUN dotnet restore "TestWebApplication/TestWebApplication.csproj"
COPY . .
WORKDIR "/src/TestWebApplication"
RUN dotnet publish "TestWebApplication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TestWebApplication.dll"]


