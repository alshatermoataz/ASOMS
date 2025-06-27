# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files
COPY ["ASOMS.Cms/ASOMS.Cms.csproj", "ASOMS.Cms/"]
COPY ["ASOMS.DAL/ASOMS.DAL.csproj", "ASOMS.DAL/"]
COPY ["ASOMS.Core/ASOMS.Core.csproj", "ASOMS.Core/"]

# Restore dependencies
RUN dotnet restore "ASOMS.Cms/ASOMS.Cms.csproj"

# Copy entire source
COPY . .

# Build the application
WORKDIR "/src/ASOMS.Cms"
RUN dotnet build "ASOMS.Cms.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "ASOMS.Cms.csproj" -c Release -o /app/publish

# Create final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entrypoint
ENTRYPOINT ["dotnet", "ASOMS.Cms.dll"]
