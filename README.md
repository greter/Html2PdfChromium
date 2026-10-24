> [!WARNING]
> **This project is no longer maintained.**
> I built Html2PdfChromium for a specific need a few years ago and no longer
> actively develop or support it. The code and the Docker image
> (`greter/html2pdfchromium`) remain available as-is under the MIT license,
> but they will not receive updates, including security fixes for Chromium
> or .NET. Please don't use the image in production without rebuilding and
> reviewing it yourself.
>
> **Alternatives:**
> - [Gotenberg](https://github.com/gotenberg/gotenberg): an actively maintained
>   Docker API for HTML-to-PDF conversion using Chromium
> - [PuppeteerSharp](https://github.com/hardkoded/puppeteer-sharp) or
>   [Playwright for .NET](https://playwright.dev/dotnet/) if you want to
>   generate PDFs directly from your own .NET code
>
> You're welcome to fork it and continue it. If you do, let me know and
> I'll link your fork here.
>
> Thanks to everyone who used it or contributed.
>
>
# Introduction 
Docker image to convert html page into pdf's

# build
docker build --no-cache -t greter/html2pdfchromium:release-0.12.7 .
docker push greter/html2pdfchromium:release-0.12.7
docker tag greter/html2pdfchromium:release-0.12.7 greter/html2pdfchromium:latest
docker push greter/html2pdfchromium:latest
docker tag greter/html2pdfchromium:release-0.12.7 acrzclpquotes.azurecr.io/html2pdfchromium:release-0.12.7
docker push acrzclpquotes.azurecr.io/html2pdfchromium:release-0.12.7
