###
# @bot-written
#
# WARNING AND NOTICE
# Any access, download, storage, and/or use of this source code is subject to the terms and conditions of the
# Full Software Licence as accepted by you before being granted access to this source code and other materials,
# the terms of which can be accessed on the Codebots website at https://codebots.com/full-software-licence. Any
# commercial use in contravention of the terms of the Full Software Licence may be pursued by Codebots through
# licence termination and further legal action, and be required to indemnify Codebots for any loss or damage,
# including interest and costs. You are deemed to have accepted the terms of the Full Software Licence on any
# access, download, storage, and/or use of this source code.
#
# BOT WARNING
# This file is bot-written.
# Any changes out side of "protected regions" will be lost next time the bot makes any changes.
###

# % protected region % [Customise the build stage] off begin

##### Build Stage #####
###
# SERVERSIDE
# This container will build the serverside of the application.
###
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build-serverside
WORKDIR /build

# Copy and restore project. This is done before copying the rest of the source files to cache dependencies independently
# from code changes.
COPY serverside/src/*.csproj ./
RUN dotnet restore

# Copy & build source
COPY serverside/src ./
RUN dotnet publish -c Release -o out -p:CSHARPBOT_DISABLE_CLIENT_BUILD=1 -p:CSHARPBOT_DISABLE_CLIENT_COPY=1

###
# CLIENTSIDE
# This container will build the clientside of the application.
###
FROM node:12-alpine AS build-clientside
WORKDIR /build

# Copy and restore dependencies. This is done before copying the project source to cache it independently from the
# source files. The lock file needs to be copied as well to maintain consistent builds.
COPY clientside/package.json ./clientside/yarn.lock* ./
RUN yarn

# Copy & build source
COPY clientside ./
RUN yarn run build
# % protected region % [Customise the build stage] end

# % protected region % [Customise the runtime] off begin
###
# RUNTIME
# This is the container that will run the web server that was built from the prior containers.
# If there are any native dependencies needed for your application, they will need to be installed here.
###
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app

# Copy the built serverside
COPY --from=build-serverside /build/out ./

# Copy the built clientside
COPY --from=build-clientside /build/build ./Client

# Expose port 80 for the web server
EXPOSE 80

ENTRYPOINT ["dotnet", "Firstapp2257.dll"]
# % protected region % [Customise the runtime] end
