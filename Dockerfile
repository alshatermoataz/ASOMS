# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy project files
COPY ["ASOMS.Cms/ASOMS.Cms.csproj", "ASOMS.Cms/"]
COPY ["ASOMS.DAL/ASOMS.DAL.csproj", "ASOMS.DAL/"]
COPY ["ASOMS.Core/ASOMS.Core.csproj", "ASOMS.Core/"]

# Restore dependencies
RUN dotnet restore "ASOMS.Cms/ASOMS.Cms.csproj"

# Copy all source files
COPY . .

# Build the project
WORKDIR "/src/ASOMS.Cms"
RUN dotnet build "ASOMS.Cms.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "ASOMS.Cms.csproj" -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Start the app
ENTRYPOINT ["dotnet", "ASOMS.Cms.dll"]
