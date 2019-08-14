using System;
using PuppeteerSharp.Media;

namespace Html2PdfChromium
{
    public class PdfParameters
    {
        /// <summary>
        /// URL to the page that get's converted
        /// </summary>
        public string BodyUrl { get; set; }

        /// <summary>
        /// Scale of the webpage rendering. Defaults to <c>1</c>. Scale amount must be between 0.1 and 2.
        /// </summary>
        public Decimal Scale { get; set; } = Decimal.One;

        /// <summary>
        /// Display header and footer. Defaults to <c>false</c>
        /// </summary>
        public bool DisplayHeaderFooter { get; set; } = false;

        /// <summary>
        /// HTML template for the print header. Should be valid HTML markup with following classes used to inject printing values into them:
        ///   <c>date</c> - formatted print date
        ///   <c>title</c> - document title
        ///   <c>url</c> - document location
        ///   <c>pageNumber</c> - current page number
        ///   <c>totalPages</c> - total pages in the document
        /// </summary>
        public string HeaderTemplate { get; set; } = string.Empty;

        /// <summary>
        /// HTML template for the print footer. Should be valid HTML markup with following classes used to inject printing values into them:
        ///   <c>date</c> - formatted print date
        ///   <c>title</c> - document title
        ///   <c>url</c> - document location
        ///   <c>pageNumber</c> - current page number
        ///   <c>totalPages</c> - total pages in the document
        /// </summary>
        public string FooterTemplate { get; set; } = string.Empty;

        /// <summary>
        /// Print background graphics. Defaults to <c>false</c>
        /// </summary>
        public bool PrintBackground { get; set; } = false;

        /// <summary>
        /// Paper orientation.. Defaults to <c>false</c>
        /// </summary>
        public bool Landscape { get; set; } = false;

        /// <summary>
        /// Paper ranges to print, e.g., <c>1-5, 8, 11-13</c>. Defaults to the empty string, which means print all pages
        /// </summary>
        public string PageRanges { get; set; } = string.Empty;

        /// <summary>
        /// Paper format. Valid formats: Letter, Legal, Tabloid, Ledger, A0, A1, A2, A3, A4, A5, A6 />
        /// </summary>
        public string PaperFormat { get; set; } = "A4";

        /// <summary>Top margin, accepts values labeled with units, defaults to none</summary>
        public string TopMargin { get; set; }

        /// <summary>Left margin, accepts values labeled with units, defaults to none</summary>
        public string LeftMargin { get; set; }

        /// <summary>Bottom margin, accepts values labeled with units, defaults to none</summary>
        public string BottomMargin { get; set; }

        /// <summary>Right margin, accepts values labeled with units, defaults to none</summary>
        public string RightMargin { get; set; }

        /// <summary>
        /// Give any CSS <c>@page</c> size declared in the page priority over what is declared in <c>width</c> and <c>height</c> or <c>format</c> options.
        /// Defaults to <c>false</c>, which will scale the content to fit the paper size.
        /// </summary>
        public bool PreferCSSPageSize { get; set; } = false;
    }
}