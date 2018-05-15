using BasBE100;
using Primavera.Extensibility.Base.Editors;
using Primavera.Extensibility.CustomTab;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Primavera.Base.CustomTabs.Customer
{
    /// <summary>
    /// Add a new Custom tab to the customer window with map.
    /// </summary>
    [Guid("8CB3FB85-BB4D-46BC-95D7-95DBB4D38117")]
    public partial class GoogleMaps : CustomTab<FichaClientes>
    {
        public GoogleMaps()
        {
            InitializeComponent();
        }      

        /// <summary>
        /// Find the costumer on Google maps based on the address.
        /// </summary>
        private void LoadCustomerLocation()
        {
            BasBECliente cliente = this.ContextService.Cliente;
            StringBuilder costumerLocation = new StringBuilder();
            StringBuilder costumerinfo = new StringBuilder();

            // Build a string with the adress for google api.
            costumerLocation.Append(cliente.Morada + ",");
            costumerLocation.Append(cliente.Localidade);

            // Build a string wit additional information to show when click on the pin.
            costumerinfo.Append(cliente.Cliente + "<BR>");
            costumerinfo.Append(cliente.Nome + "<BR>");
            costumerinfo.Append(cliente.Telefone + "<BR>");

            // Read the HTML file template.
            // The file Maps.html will be copied to the extensions folder on build.
            string percursoDadosComuns = this.ContextService.PSO.RegistryPrimavera.DaPercursoConfigEx("Default");
            StreamReader reader = new StreamReader(percursoDadosComuns + "Extensions\\DEMO10\\Resources\\maps.htm");

            string readFile = reader.ReadToEnd();

            // Replace all the tags on the javascript  with real information.
            readFile = readFile.Replace("@adress", costumerLocation.ToString());
            readFile = readFile.Replace("@pininformation", costumerinfo.ToString());

            reader.Close();
            
            MapsBrowser.DocumentText = readFile;
        }

        /// <summary>
        /// This method is trigged after identify the entity.
        /// </summary>
        private void GoogleMaps_Loading()
        {
            LoadCustomerLocation();
        }
    }
}