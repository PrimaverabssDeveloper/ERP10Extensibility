using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Platform.Services;

namespace Primavera.Log
{
    public class LoggerInjector : Plataforma
    {
        public override void AntesDeAbrirEmpresa(ref bool Cancel, ExtensibilityEventArgs e)
        {
            BSO.Extensibility.Logger = new MyLog(true, PSO.Dialogos);
        }
    }
}
