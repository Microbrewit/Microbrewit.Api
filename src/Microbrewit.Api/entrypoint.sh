#!/bin/bash
set -e

sed -i \
    -e 's,${POSTGRES_DB},'"${POSTGRES_DB}"',g' \
    -e 's,${POSTGRES_USER},'"${POSTGRES_USER}"',g' \
    -e 's,${POSTGRES_PASSWORD},'"${POSTGRES_PASSWORD}"',g' \
    -e 's,${ELASTICSEARCH_INDEX},'"${ELASTICSEARCH_INDEX}"',g' \
    -e 's,${MICROBREWIT_APIURL},'"${MICROBREWIT_APIURL}"',g' \
	-e 's,${MICROBREWIT_MAILPASSWORD},'"${MICROBREWIT_MAILPASSWORD}"',g' \
    ./docker/appsettings.json

# Remove default config and replace with environment variable based config.
rm ./appsettings.json
mv ./docker/appsettings.json ./appsettings.json


echo "START ALL THE THINGS!"

# Exec docker run invokers original command
dotnet run
