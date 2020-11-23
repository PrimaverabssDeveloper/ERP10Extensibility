using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRISDK100;

namespace PrimaveraSDK
{
    sealed class PriSDKContext: DisposableBase
    {
        // .NET guarantees thread safety for static initialization
        public static readonly clsSDKContexto SdkContext = new clsSDKContexto();
        private static bool contextInitialized = false;

        /// <summary>
        /// Private constructor
        /// </summary>
        private PriSDKContext()
        {
        }

        public static void Initialize(dynamic BSO, dynamic PSO)
        {
            if (!contextInitialized)
            {
                SdkContext.Inicializa(BSO, "ERP");
                PSO.InicializaPlataforma(SdkContext);

                contextInitialized = true;
            }
        }

        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called
            if (!this.Disposed)
            {
            }

            // Dispose on base class
            base.Dispose(disposing);
        }
        #endregion
    }
}
