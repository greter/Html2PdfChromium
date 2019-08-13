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

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Html2PdfChromium.csproj", "Html2PdfChromium/"]
RUN dotnet restore "Html2PdfChromium/Html2PdfChromium.csproj"
COPY . ./Html2PdfChromium
WORKDIR "/src/Html2PdfChromium"
RUN dotnet build "Html2PdfChromium.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Html2PdfChromium.csproj" -c Release -o /app  -r linux-x64 --self-contained

FROM ubuntu AS final
RUN apt-get update
RUN apt-get -y install alsa-base chromium-browser xauth
RUN apt-get clean
WORKDIR /app
COPY --from=publish /app .
EXPOSE 5000
EXPOSE 5001
ENTRYPOINT ["/app/Html2PdfChromium"]
