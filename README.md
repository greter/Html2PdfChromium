# Introduction 
Docker image to convert html page into pdf's

# build
docker build --no-cache -t greter/html2pdfchromium:release-0.12.1 .
docker push greter/html2pdfchromium:release-0.12.1
docker tag greter/html2pdfchromium:release-0.12.1 greter/html2pdfchromium:latest
docker push greter/html2pdfchromium:latest
docker tag greter/html2pdfchromium:release-0.12.1 acrzclpquotes.azurecr.io/html2pdfchromium:release-0.12.1
docker push acrzclpquotes.azurecr.io/html2pdfchromium:release-0.12.1