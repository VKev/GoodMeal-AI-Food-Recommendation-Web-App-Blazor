FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /App

COPY Web.sln ./

COPY src/Blazor/Blazor.csproj ./src/Blazor/
COPY src/Application/Application.csproj ./src/Application/
COPY src/Infrastructure/Infrastructure.csproj ./src/Infrastructure/
COPY src/Domain/Domain.csproj ./src/Domain/
COPY test/test.csproj ./test/

RUN dotnet restore

COPY src/ ./src/

WORKDIR /App/src/Blazor
RUN dotnet publish -c Release -o /App/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /App

ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /App/out .

ENTRYPOINT ["dotnet", "Blazor.dll"]
