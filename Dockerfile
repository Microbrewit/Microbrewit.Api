FROM microsoft/aspnet:1.0.0-rc1-update1-coreclr

WORKDIR /app
COPY ["./project.json","/app"]
RUN ["dnu", "restore", "--runtime=ubuntu.14.04-x64"]

COPY . /app
RUN ["dnu","restore","--runtime=ubuntu.14.04-x64"]

EXPOSE 5005/tcp

ENTRYPOINT ["/app/docker/entrypoint.sh"]
CMD ["dnx", "-p", "project.json", "web"]
