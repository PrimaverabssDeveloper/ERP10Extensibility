using BasBE100;
using Primavera.Extensibility.Base.Editors;
using Primavera.Extensibility.CustomTab;
using StdBE100;
using System;

namespace Primavera.Base.CustomTabs.Customer
{
    /// <summary>
    /// Working with user fields.
    /// </summary>
    public partial class CustomerFields : CustomTab<FichaClientes>
    {
        public CustomerFields()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Manual load the customer user fields.
        /// </summary>
        private void CustomerFields_Loading()
        {
            BasBECliente cliente = this.ContextService.Cliente;

            StdBECampos cdu = cliente.CamposUtil;

            CDUCampoVar1.Text = Convert.ToString(cdu["CDU_CampoVar1"].Valor);
            CDUCampoVar2.Text = Convert.ToString(cdu["CDU_CampoVar2"].Valor);
            CDUCampoVar3.Text = Convert.ToString(cdu["CDU_CampoVar3"].Valor);
        }

        /// <summary>
        /// Manual save the customer user fields.
        /// </summary>
        private void CustomerFields_Saving()
        {
            BasBECliente cliente = this.ContextService.Cliente;

            cliente.CamposUtil["CDU_CampoVar1"].Valor = CDUCampoVar1.Text;
            cliente.CamposUtil["CDU_CampoVar2"].Valor = CDUCampoVar2.Text;
            cliente.CamposUtil["CDU_CampoVar3"].Valor = CDUCampoVar3.Text;
        }
    }
}
