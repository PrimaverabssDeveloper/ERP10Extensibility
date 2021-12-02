using Primavera.Extensibility.Base.Editors;
using Primavera.Extensibility.CustomTab;
using System.Windows.Forms;

namespace Primavera.Platform.Geolocation
{
    /// <summary>
    /// Classe que configura o separador.
    /// </summary>
    public partial class TabMapSuppliers : CustomTab<FichaFornecedores>
    {
        private TabMapBase _baseMap;
        bool loading = false;

        private TabMapBase baseMap
        {
            get
            {
                if (_baseMap == null
                    || (base.ContextService?.Fornecedor != null && _baseMap.Entity == null)
                    || (_baseMap?.Entity != null && !(_baseMap.Entity is BasBE100.BasBEFornecedor))
                    || (_baseMap?.Entity != null && (_baseMap.Entity is BasBE100.BasBEFornecedor entity && entity?.Fornecedor != base.ContextService?.Fornecedor.Fornecedor)))
                {
                    _baseMap = new TabMapBase(base.ContextService, base.ContextService?.Fornecedor)
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

        public TabMapSuppliers()
        {
            InitializeComponent();
        }

        private void TabMapSuppliers_Load(object sender, System.EventArgs e)
        {
            if (this.Controls.IndexOf(baseMap) < 0)
            {
                this.Controls.Add(baseMap);
            }
        }

        /// <summary>
        /// Método responsável pelas alterações aquando da alteracao do separador.
        /// </summary>
        private void TabMapSuppliers_Saving()
        {
            baseMap.SaveMapPreferences();
        }

        /// <summary>
        /// Método responsável pelas alterações aquando da escolha do Cliente.
        /// </summary>
        private void TabMapSuppliers_Loading()
        {
            if (!loading)
            {
                this.baseMap.Loading();
                loading = true;
            }
        }
    }
}
