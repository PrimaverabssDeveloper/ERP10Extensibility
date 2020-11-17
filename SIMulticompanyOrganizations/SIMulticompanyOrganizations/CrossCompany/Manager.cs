using SUGIMPL_OME.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Primavera.Extensibility.CustomCode;

namespace SUGIMPL_OME.CrossCompany
{
    public class Manager
    {
        public ERPContext ERPContext { get; }

        public Manager(StdPlatBS100.StdBSInterfPub mPSO, ErpBS100.ErpBS mBSO)
        {
            ERPContext = new ERPContext(mPSO, mBSO);
        }

        public void PR_GlobalPosition(string strTipoEntidade = "", string strCodigoEntidade = "")
        {
            frmPosicaoGlobal FPosicaoGlobal = new frmPosicaoGlobal(ERPContext, strTipoEntidade, strCodigoEntidade);

            ERPContext.PSO.UI.AdicionaFormMDI(FPosicaoGlobal);
            FPosicaoGlobal.Show();
        }

        public void ImportDocuments()
        {
            frmImportDocuments FImportDocuments = new frmImportDocuments(ERPContext);

            ERPContext.PSO.UI.AdicionaFormMDI(FImportDocuments);
            FImportDocuments.Show();
        }

        public List<String> PR_CreditLimitExceeded(string strCustomerCode)
            => CrossCompany.PayablesReceivables.CreditLimitExceeded(ERPContext, strCustomerCode);

        public bool PLT_CheckPassword(string password)
            => CrossCompany.Platform.CheckPassword(ERPContext, password);

        public List<String> UpdateItem_GroupCompanies(String Item)
            => CrossCompany.Base.UpdateItem_GroupCompanies(ERPContext, Item);

        public void BeforeOpenCompany(ref Boolean Cancel)
            => CrossCompany.Platform.BeforeOpenCompany(ERPContext, ref Cancel);

        public List<String> RemoveEntity(string EntityType, string Entity)
            => CrossCompany.Base.RemoveEntity(ERPContext, EntityType, Entity);

        public List<String> UpdateEntity(string EntityType, string Entity)
            => CrossCompany.Base.UpdateEntity(ERPContext, EntityType, Entity);

        public void CheckPendingDocuments()
        {
            Dictionary<String, int> pendingDocuments = CrossCompany.BusinessProcesses.CheckPendingDocuments(ERPContext);

            if (pendingDocuments.Count>0 && (pendingDocuments["Sales"]>0 || pendingDocuments["Purchases"] > 0))
            {
                ERPContext.PSO.Bot.CriaMensagem(
                    "Group Transactions",
                    String.Format("Existem documentos para integração nesta empresa ({0} documento(s) de venda e {1} documento(s) de compra).", pendingDocuments["Sales"], pendingDocuments["Purchases"]),
                    null,
                    false);
            }
        }
    }
}
