# Introduction 
Docker image to convert html page into pdf's

# build
docker build --no-cache -t greter/html2pdfchromium:release-0.10.6 .
docker push greter/html2pdfchromium:release-0.10.6
docker tag greter/html2pdfchromium:release-0.10.6 greter/html2pdfchromium:latest
docker push greter/html2pdfchromium:latest