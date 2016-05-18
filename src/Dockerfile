FROM microsoft/dotnet:1.0.0-preview1

WORKDIR /app
COPY ["./project.json","/app"]
RUN ["dotnet", "restore"]

COPY . /app
RUN ["dotnet","restore"]

EXPOSE 5005/tcp

CMD ["dotnet", "run"]
