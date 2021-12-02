using Primavera.Extensibility.Base.Editors;
using Primavera.Extensibility.CustomTab;
using System.Windows.Forms;

namespace Primavera.Platform.Geolocation
{
    /// <summary>
    /// Classe que configura o separador.
    /// </summary>
    public partial class TabMapOthers : CustomTab<FichaOutrosTerceiros>
    {
        private TabMapBase _baseMap;
        bool loading = false;

        private TabMapBase baseMap
        {
            get
            {
                if (_baseMap == null
                    || (base.ContextService?.OutroTerceiro != null && _baseMap.Entity == null)
                    || (_baseMap?.Entity != null && !(_baseMap.Entity is BasBE100.BasBEOutroTerceiro))
                    || (_baseMap?.Entity != null && (_baseMap.Entity is BasBE100.BasBEOutroTerceiro entity && entity?.Terceiro != base.ContextService?.OutroTerceiro.Terceiro)))
                {
                    _baseMap = new TabMapBase(base.ContextService, base.ContextService?.OutroTerceiro)
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

        public TabMapOthers()
        {
            InitializeComponent();
        }

        private void TabMapOthers_Load(object sender, System.EventArgs e)
        {
            if (this.Controls.IndexOf(baseMap) < 0)
            {
                this.Controls.Add(baseMap);
            }
        }

        /// <summary>
        /// Método responsável pelas alterações aquando da alteracao do separador.
        /// </summary>
        private void TabMapOthers_Saving()
        {
            baseMap.SaveMapPreferences();
        }

        /// <summary>
        /// Método responsável pelas alterações aquando da escolha do Outro Terceiro.
        /// </summary>
        private void TabMapOthers_Loading()
        {
            if (!loading)
            {
                this.baseMap.Loading();
                loading = true;
            }
        }
    }
}
