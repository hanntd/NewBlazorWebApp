#!/bin/bash

if [[ ! -d certs ]]
then
    mkdir certs
    cd certs/
    if [[ ! -f localhost.pfx ]]
    then
        dotnet dev-certs https -v -ep localhost.pfx -p 039af270-522b-4321-ad1b-adf52b00443b -t
    fi
    cd ../
fi

docker-compose up -d
