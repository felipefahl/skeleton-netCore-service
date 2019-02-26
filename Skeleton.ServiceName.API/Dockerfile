FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Skeleton.ServiceName.API/Skeleton.ServiceName.API.csproj", "Skeleton.ServiceName.API/"]
RUN dotnet restore "Skeleton.ServiceName.API/Skeleton.ServiceName.API.csproj"
COPY . .
WORKDIR "/src/Skeleton.ServiceName.API"
RUN dotnet build "Skeleton.ServiceName.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Skeleton.ServiceName.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Skeleton.ServiceName.API.dll"]
