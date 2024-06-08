	# Use compatible tags for Windows/AMD64
	FROM mcr.microsoft.com/dotnet/aspnet:8.0-windowsservercore-ltsc2019 AS base
	ENV ASPNETCORE_ENVIRONMENT=Development
	WORKDIR /app
	EXPOSE 80
	EXPOSE 443

	# Use the .NET SDK to build the application
	FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2019 AS build
	WORKDIR /src
	COPY ["src/PersonDetails.Api/PersonDetails.Api.csproj", "PersonDetails.Api/"]
	COPY ["src/PersonDetails.Tests/PersonDetails.Tests.csproj", "PersonDetails.Tests/"]
	RUN dotnet restore "PersonDetails.Api/PersonDetails.Api.csproj"
	COPY . .
	WORKDIR "/src/PersonDetails.Api"
	RUN dotnet build "PersonDetails.Api.csproj" -c Release -o /app/build

	# Publish the application
	FROM build AS publish
	RUN dotnet publish "PersonDetails.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

	# Use the ASP.NET Core runtime image to run the application
	FROM base AS final
	WORKDIR /app
	COPY --from=publish /app/publish .
 
	ENTRYPOINT ["dotnet", "PersonDetails.Api.dll"]
