using Primavera.Extensibility.Base.Editors;
using Primavera.Extensibility.CustomTab;
using System.Windows.Forms;

namespace Primavera.Platform.Geolocation
{
    /// <summary>
    /// Classe que configura o separador.
    /// </summary>
    public partial class TabMapCustomers : CustomTab<FichaClientes>
    {
        private TabMapBase _baseMap;
        bool loading = false;

        private TabMapBase baseMap
        {
            get
            {
                if (_baseMap == null 
                    || (base.ContextService?.Cliente != null && _baseMap.Entity == null)
                    || (_baseMap?.Entity != null && !(_baseMap.Entity is BasBE100.BasBECliente))
                    || (_baseMap?.Entity != null && (_baseMap.Entity is BasBE100.BasBECliente entity && entity?.Cliente != base.ContextService?.Cliente.Cliente)))
                {
                    _baseMap = new TabMapBase(base.ContextService, base.ContextService?.Cliente)
                    {
                        Dock = DockStyle.Fill
                    };
                }

                return _baseMap;
            }
            set
            {
                _baseMap = value;
            }
        }

        public TabMapCustomers()
        {
            InitializeComponent();
        }

        private void TabMapCustomers_Load(object sender, System.EventArgs e)
        {
            if (this.Controls.IndexOf(baseMap) < 0)
            {
                this.Controls.Add(baseMap);
            }
        }

        /// <summary>
        /// Método responsável pelas alterações aquando da alteracao do separador.
        /// </summary>
        private void TabMapCustomers_Saving()
        {
            this.baseMap.SaveMapPreferences();
        }

        /// <summary>
        /// Método responsável pelas alterações aquando da escolha do Cliente.
        /// </summary>
        private void TabMapCustomers_Loading()
        {
            if (!loading)
            {
                this.baseMap.Loading();
                loading = true;
            }
        }
    }
}
