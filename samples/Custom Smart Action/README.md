# Custom Smart Action

This sample demonstrates how to implement **Smart Actions of type Extensibility Function** in Cegid ERP. Smart Actions let you inject your own data and prompt into the ERP's built-in AI (Pulse) and handle the result, all through your extensibility assembly.

## What this sample covers

| File | Description |
|---|---|
| `SmartAction.cs` | Implements the `CustomSmartAction` base class with `BeforeExecution` and `AfterExecution` hooks for four use cases: article enquiry, customer credit-risk diagnosis, sales document analysis, and HTML-to-PDF quotation printing. |
| `CriaArtigos.cs` | Uses the Cegid Pulse LLM API (`UserPromptFunction`) to convert a price-table CSV into structured article records and persist them via the ERP API. |
| `PriceTableConverter.cs` | Reads price tables from PDF or Excel files and converts them to CSV. PDF extraction uses `pdftotext`; Excel extraction uses EPPlus. |
| `HtmlPdfPrinter.cs` | Converts an LLM-generated HTML document to PDF using `wkhtmltopdf`. |
| `ScriptSQL_CustomSmartActions/UseCasesSmartActions.sql` | T-SQL script that seeds the SmartActions configuration tables with five ready-to-run use cases. |

## Prerequisites

- A local installation of Cegid ERP.
- Visual Studio 2019 or later with .NET Framework 4.7.1.

## External Programs

The following tools must be installed for all examples to run:

| Tool | Required by | Default path |
|---|---|---|
| [wkhtmltopdf](https://wkhtmltopdf.org/) | Smart Action `VND.003` — converts LLM-generated HTML to PDF | `C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe` |
| [pdftotext](https://poppler.freedesktop.org/) (poppler-utils) | `PriceTableConverter` — extracts text from PDF price tables | `C:\Program Files\poppler\Library\bin\pdftotext.exe` or any directory on `PATH` |

## SQL Setup

Run `ScriptSQL_CustomSmartActions\UseCasesSmartActions.sql` against your company database to register the five sample Smart Actions and their SQL queries:

| Smart Action | Form | Description |
|---|---|---|
| `CLI.001` | Customers | Customer credit-risk diagnosis |
| `VND.002` | Sales Documents | Sales document analysis |
| `VND.003` | Sales Documents | HTML quotation generation and PDF printing |
| `CMP.001` | Purchase Invoices | Purchase price anomaly detection |
| `GPR.001` | Manufacturing Orders | Manufacturing order analysis |

## Build & Deploy

1. Open `CustomSmartAction.sln` in Visual Studio.
2. Build the solution (**Build > Build Solution**).
3. Register the output assembly as an extensibility project in Cegid ERP.
