using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.Base.Editors;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;

namespace DEMO.Base
{
    public class UiFichaClientes : FichaClientes
    {
        public override void AntesDeGravar(ref bool Cancel, ExtensibilityEventArgs e)
        {
            base.AntesDeGravar(ref Cancel, e);

            try
            {
                if ((bool)this.Cliente.CamposUtil["CDU_OperadorFitofarmaceuticos"].Valor)
                {
                    if (string.IsNullOrEmpty(this.Cliente.CamposUtil["CDU_FitofarmaceuticoNumero"].Valor.ToString()))
                    {
                        throw new Exception("O número de autorização ou de aplicador é obrigatório nos operadores de fitofarmacêuticos.");
                    }
                }
            }
            catch (Exception ex)
            {
                PSO.Dialogos.MostraErro(ex.Message, StdPlatBS100.StdBSTipos.IconId.PRI_Exclama);
                Cancel = true;
            }
        }
    }
}
