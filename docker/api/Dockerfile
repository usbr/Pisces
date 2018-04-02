#pisces .net core api
# ref https://github.com/dotnet/dotnet-docker-samples/blob/master/dotnetapp-prod/Dockerfile
FROM microsoft/dotnet AS build-env
WORKDIR /api

# Copy sln and restore as distinct layers
#COPY *.sln ./
#RUN dotnet restore Pisces.sln

# build runtime image
#FROM microsoft/dotnet:2.0-runtime 
#WORKDIR /api
#COPY --from=build-env /api/out ./
#ENTRYPOINT ["dotnet", "dotnetapp.dll"]
