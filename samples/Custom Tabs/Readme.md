# CustomTab

Customer tabs or User tabs technology allow you to add more Tab's with extra features to a system entity.

### How to use?
To start developing with the extensibility technology you need:

1. A local installation of PRIMAVERA ERP 10.
2. Reference to .NET Framework 4.7.
3. Create a new class library project and add a new item - User Control.
4. Add to your project a reference to `Primavera.Extensibility.BusinessEntities`.This will provide access to events that you will need override.
5. Add to your project a reference to `Primavera.Extensibility.Integration`, `Primavera.Extensibility.CustomTab` and `Primavera.Extensibility.Patterns`.
6. Add to your project a reference to `ErpBS100` and `StdPlatBS100`. This is only necessary if will use the API or the platform.
7. Add to your project a reference to the module that you want extend. For sales is `Primavera.Extensibility.Sales`. Whit this reference you can access to the sales classes (UI side - *EditorVendas*) and services (API side - *VndBSVendas*).

