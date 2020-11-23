# Custom Events 

Custom events are actions performed by users which trigger the technology extensibility execute custom code.

### How to use?
To start developing with the extensibility technology you need:

1. A local installation of PRIMAVERA ERP 10.
2. Reference to .NET Framework 4.7.
3. Create a new class library project.
4. Add to your project a reference to `Primavera.Extensibility.BusinessEntities`.This will provide access to events that you well need override.
5. Add to your project a reference to `Primavera.Extensibility.Integration`.
6. Add to your project a reference to `ErpBS100` and `StdPlatBS100`.
7. Add to your project a reference to the module that you want extend. For sales is `Primavera.Extensibility.Sales`. Whit this reference you can access to the sales classes (UI side - *EditorVendas*) and services (API side - *VndBSVendas*).
8. Finally you can start coding your project.
