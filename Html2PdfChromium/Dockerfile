#FROM ubuntu
#RUN apt-get update
#RUN apt-get -y install alsa-base chromium-browser xauth
#
#COPY bin/Release/netcoreapp2.2/linux-x64/publish/ /app
#
#WORKDIR /app
#
#ENV WEB_PORT="5001" \
#WEB_SERVER_BASE_URI="https://0.0.0.0" \
#ASPNETCORE_ENVIRONMENT="Production"

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Html2PdfChromium.csproj", "Html2PdfChromium/"]
RUN dotnet restore "Html2PdfChromium/Html2PdfChromium.csproj"
COPY . ./Html2PdfChromium
WORKDIR "/src/Html2PdfChromium"
RUN dotnet build "Html2PdfChromium.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Html2PdfChromium.csproj" -c Release -o /app  -r linux-x64 --self-contained

FROM ubuntu AS final

ENV TZ=Etc/UTC
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

RUN apt update
RUN apt -y upgrade 
RUN apt -y install alsa-base \
                       chromium-browser \ 
                       xauth \
                       ca-certificates \
                       fonts-liberation \
                       libappindicator3-1 \
                       libasound2 \
                       libatk-bridge2.0-0 \
                       libatk1.0-0 \
                       libc6 \
                       libcairo2 \
                       libcups2 \
                       libdbus-1-3 \
                       libexpat1 \
                       libfontconfig1 \
                       libgbm1 \
                       libgcc1 \
                       libglib2.0-0 \
                       libgtk-3-0 \
                       libnspr4 \
                       libnss3 \
                       libpango-1.0-0 \
                       libpangocairo-1.0-0 \
                       libstdc++6 \
                       libx11-6 \
                       libx11-xcb1 \
                       libxcb1 \
                       libxcomposite1 \
                       libxcursor1 \
                       libxdamage1 \
                       libxext6 \
                       libxfixes3 \
                       libxi6 \
                       libxrandr2 \
                       libxrender1 \
                       libxss1 \
                       libxtst6 \
                       lsb-release \
                       wget \
                       xdg-utils 
RUN apt clean
WORKDIR /app
COPY --from=publish /app .
EXPOSE 5000
EXPOSE 5001
ENTRYPOINT ["/app/Html2PdfChromium"]
