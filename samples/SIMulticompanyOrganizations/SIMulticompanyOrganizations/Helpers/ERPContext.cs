using PRISDK100;
using StdPlatBS100;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StdBE100.StdBETipos;

namespace SUGIMPL_OME.Helpers
{
    public class ERPContext
    {
        #region ** Public Properties **
        //BSO (PEX instance)
        public ErpBS100.ErpBS BSO { get; set; }
        //PSO (PEX instance)
        public StdBSInterfPub PSO { get; set; }
        //Context do be user by the PRIMAVERA SDK
        public clsSDKContexto sdkContext;
        #endregion

        //Constructors
        public ERPContext(string strInstance, string strCompany, string strUserName, string strPassword)
        {
           // CreateContextERP(strInstance,strCompany, strUserName, strPassword);

            //Navegador = PSO.Navegador;
            //Navegador.AbreLista += new StdPlatBS100.StdBSNavegador.AbreListaHandler(Navegador_AbreLista);
        }


        /// <summary>
        /// Set the ERP context using the PSO and BSO objects already open
        /// </summary>
        /// <param name="mPSO">PSO from the PEX environment</param>
        /// <param name="mBSO">BSO from the PEX environment</param>
        public ERPContext(StdPlatBS100.StdBSInterfPub mPSO, ErpBS100.ErpBS mBSO)
        {
            BSO = mBSO;
            PSO = mPSO;
            InitializeSDKContext();
        }


        /// <summary>
        /// Initialize PRIMAVERA SDK Context
        /// </summary>
        private void InitializeSDKContext()
        {
            if (sdkContext == null)
            {
                sdkContext = new clsSDKContexto();
                sdkContext.Inicializa(BSO, "ERP");
                PSO.InicializaPlataforma(sdkContext);
            }
        }
    }
}
