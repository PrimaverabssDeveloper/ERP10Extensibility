# Extensibility

In this repository you will find information and code samples that allows you to understand how to use PRIMAVERA extensibility technology. This technology is the Visual Basic For Applications successor.

###  Extensibility Principle

In a ERP, extensibility is a tool that allows you to change the application business workflow or add extra business rules to implement your unique business processes.
The key to extensibility is that you can develop pieces of code that best fit you customer business process and create the necessary functionality for themselves.

With extensibility tools, you can:
1) Add to a entity user-defined fields and user-defined tables.
2) Change or add business rules at the user interface or API level.
3) Add new items to the application menu.
4) Add a new tab to an existing windows/entities.

## Repository Organization

This repository is organized in two sections, in the section **samples** you will found code samples that will help you understand how to use  the extensibility technology. In the section **Implementation** you will found real case solution to specialized market areas.

### In Samples Section

| Sample                                 | Descripition     |
| :------------------------------------- | :--------------- |
| [Custom Code](samples/Custom%20Code) | Uses the https://www.nif.pt/ API to create new entities. |
| [Custom Events](samples/Custom%20Events) | Add to the sales invoice a zip code format validation and exports the document in to PDF. |
| [Custom Form](samples/Custom%20Form) | List all sales document and allows perform drill-down to the source document and to the entity. |
| [Custom Tab](samples/Custom%20Tabs) | Add a custom tab to customer entity. A Tab with goole maps integration. |
| [Custom Ribbon](samples/Custom%20Ribbon) | Add's a new TAB on the ERP Ribbon. |
| [Primavera Logger ](samples/Primavera%20Logger)| Log all the errors and events of your extensibility project. |
| [Primavera.RelatedInfo](samples/Primavera.RelatedInfo) | Adds a new control to the invoice context panel to show the customer pending documents.|
| [ScaleSample](samples/ScaleSample) | Provides an empty sample for dealing with scale within POS.|

### In Implementation Section

| Sample                                 | Descripition     |
| :------------------------------------- | :--------------- |
| [Fito Farmaceiticos](Implementation/SIFitofarmaceuticos) | Implementation Suggetion that handles the "Fitofarmacêuticos" portuguese legal requirements in a PRIMAVERA ERP V10 environment. |
| [BOT Topic](Implementation/DevelopersNetworkTopic) | Implementation suggestion for an BOT topic. |
| [Geolocation Extension](Implementation/GeolocationExtension) | Implementation of the Geolocation extension for the ERP. |

## Contributing and Feedback

Everyone is free to contribute to the repository.

Any bugs detected in the code samples can be reported in the *Issues* section of this repository.

## License

Unless otherwise specified, the code samples are released under the [MIT license](https://pt.wikipedia.org/wiki/Licen%C3%A7a_MIT).
