FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ACJudge/ACJudge.csproj", "ACJudge/"]
COPY ["GetProblemsAPI/GetProblemsAPI.csproj", "GetProblemsAPI/"]
RUN dotnet restore "ACJudge/ACJudge.csproj"
COPY . .
WORKDIR "/src/ACJudge"
RUN dotnet build "ACJudge.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ACJudge.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ACJudge.dll"]
