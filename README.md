# Extensibility

In this repository you will find information and code samples that allows you to understand how to use PRIMAVERA extensibility technology. This technology is the Visual Basic For Applications successor.

###  Extensibility Principle
In a ERP, extensibility is a tool that allows you to change the application business workflow or add extra business rules to implement your unique business processes.
The key to extensibility is that you can develop pieces of code that best fit you customer business process and create the necessary functionality for themselves.

With extensibility tools, you can:
1. Add to a system entity user-defined fields.
2. Add new tables *user-defined tables*.
3. Change or add business rules at the user interface or API level.
4. Integrate into/with external systems.
5. Add new items to the application menu.
6. Add a new tab to an existing window.

### How to use?
To start developing with the extensibility technology you need:

1. PRIMAVERA ERP 10.
2. Reference to .NET Framework 4.7.
3. Add to your project a reference to `Primavera.Extensibility.BusinessEntities`.This will provide access to events that you well need override.
4. Add to your project a reference to `Primavera.Extensibility.Integration`.
5. Add to your project a reference to `ErpBS100` and `StdPlatBS100`.This will provide access to `PSO` and `BSO` objects.
6. Add to your project a reference to the module that you want extend. For sales is `Primavera.Extensibility.Sales`. Whit this reference you can access to the sales classes (UI side - *EditorVendas*) and services (API side - *VndBSVendas*).
7. Finally you can start coding your project.

### Contributing and Feedback
Everyone is free to contribute to the repository.

Any bugs detected in the code samples can be reported in the *Issues* section of this repository.

### License
Unless otherwise specified, the code samples are released under the MIT license.
