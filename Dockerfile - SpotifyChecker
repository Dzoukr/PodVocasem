FROM mcr.microsoft.com/dotnet/sdk:6.0 as build


WORKDIR /workspace
COPY . .
RUN dotnet tool restore

RUN dotnet run PublishJob


FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
COPY --from=build /workspace/publish/app-job /app
WORKDIR /app
EXPOSE 80
ENTRYPOINT [ "dotnet", "PodVocasem.SpotifyChecker.dll" ]