using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Platform.Services;

namespace Primavera.Logger
{
    public class LoggerInjector : Plataforma
    {
        public override void AntesDeAbrirEmpresa(ref bool Cancel, ExtensibilityEventArgs e)
        {
            // Activate the log after the application start's.
            BSO.Extensibility.Logger = new MyLog(true, PSO.Dialogos);
        }
    }
}
