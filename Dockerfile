FROM mcr.microsoft.com/dotnet/aspnet:6.0.2 AS base-env
WORKDIR /app
EXPOSE 80



# BUILD APPLICATION
FROM mcr.microsoft.com/dotnet/sdk:6.0.102 AS build-env

WORKDIR /src

# Copy the application (WebAPI)
COPY ./src/Demo.WebAPI/*.csproj /src/Demo.WebAPI/

# Restore nuget packages
RUN dotnet restore /src/Demo.WebAPI/*.csproj

# Copy all the source code and build
COPY ./src /src

# Deploy application. Used the "--no-restore" to benefit the layer caches
FROM build-env AS publish-env
RUN dotnet publish /src/Demo.WebAPI/*.csproj -c Release -o /app/publish



FROM base-env AS final-env
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT Production

# Copy 
COPY --from=publish-env /app/publish .
ENTRYPOINT ["dotnet", "Demo.WebAPI.dll"]