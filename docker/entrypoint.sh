#!/bin/bash
set -e

sed -i \
    -e 's,${POSTGRESQL_DB},'"${POSTGRESQL_DB}"',g' \
    -e 's,${POSTGRESQL_USER},'"${POSTGRESQL_USER}"',g' \
    -e 's,${POSTGRESQL_PASSWORD},'"${POSTGRESQL_PASSWORD}"',g' \
    -e 's,${ELASTICSEARCH_INDEX},'"${ELASTICSEARCH_INDEX}"',g' \
    ./docker/appsettings.json

# Remove default config and replace with environment variable based config.
rm ./appsettings.json
mv ./docker/appsettings.json ./appsettings.json


echo "START ALL THE THINGS!"

# Exec docker run invokers original command
dnx -p project.json web