# GEOLOCATION

In this repository you will find an extensibility project for PRIMAVERA ERP which provides new functionalities to the end customer.<br>
With this feature you will have a service of geolocation with different maps, markers and routes integrated in the ERP v10.

## Repository Organization

This repository provides access to one project:

| Folder | Description |
|----------|----------|
| [_external](_external) | GMap.NET Custom DLLs |
| [Entities](Entities) | Classes for used database tables |
| [Factory](Factory) | Map factory that generates the possible maps |
| [Helpers](Helpers) | Classes to help / centralize variable declaration ... |
| [Images](Images) | Images used on the toolbar |
| [Manager](Manager) | Abstract class of the available map providers  |
| [Providers](Providers) | Classes of the map providers |
| [SQL](SQL) | SQL script for creation of necessary database objects |
| [UserControls](UserControls) | Geolocation tabs seen on the screens of Client, Supplier and Other Third Parties |

## Before Start

### Requirements
- PRIMAVERA ERP v10.0 (or higher).
- Visual Studio 2017 / 2019
- SQL Server and SQL Server Management Studio
- DevExpress WinForms Component - [Download](https://www.devexpress.com/products/net/controls/winforms/)


### How to use the project
To start using this repository with Visual Studio you need:

1. A local installation of PRIMAVERA ERP 10.
2. Reference to .NET Framework 4.7.1
3. Add/update your PRIMAVERA references on the project.
4. Run the SQL script from the `SQL` folder on your Database that corresponds to used company on PRIMAVERA ERP.<br>
This will provide the tables for the Geolocation extension.
5. Compile project.
6. Set **`Apl`** PRIMAVERA ERP folder as the output path for your solution.
7. Copy all references in `_external` folder plus the resulting DLL from this project to the **`Apl`** PRIMAVERA ERP folder.
    7.1 You need to copy `SQLite.Interop_x64.dll` or `SQLite.Interop_x86.dll` depending on the operative system architecture and rename the file to **`SQLite.Interop.dll`**. 
8. Finally you can start using the project.

### Authorizing Map functionalities
The Geolocation extension requires a client key for some map providers.  
Some features are only available when a key is provided.  
Even without the client key, it's possible to explore the map and manage user markers.  
To obtain keys and related information, follow these links:
- Google Map [here](https://developers.google.com/maps/documentation/embed/get-api-key)
- Bing Map [here](https://www.microsoft.com/en-us/maps/create-a-bing-maps-key)

For Google Map is also necessary to activate [`Maps Static Map API`](https://developers.google.com/maps/documentation/maps-static/overview) and [`Directions API`](https://developers.google.com/maps/documentation/directions/overview), both with a [`billing`](https://support.google.com/googleapi/answer/6158867?hl=en) account mandatory association.

## External References
All the references contained in folder **`_external`** have to be used for successfully build the solution.<p>
Both `GMap.NET.Core` and `GMap.NET.WindowsForms` use a specific version of `System.Data.SQLite`, int this case, **`1.0.115`**, and the code was modified to accept some custom modifications, so, if there's a need to add and/or change something on those references refer to the official GitHub repository for GMap.NET [here](https://github.com/judero01col/GMap.NET).



## ERP Documentation
Information for developers about PRIMAVERA ERP [here](https://developers.primaverabss.com/en/v10/)

## Contributing and Feedback
Everyone is free to contribute to the repository.  
Any bugs detected in the code can be reported in the *Issues* section of this repository.

## License

Unless otherwise specified, the code samples are released under the [MIT license](https://pt.wikipedia.org/wiki/Licen%C3%A7a_MIT).