using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.CustomCode;
using SUGIMPL_OME.Helpers;

namespace SUGIMPL_OME.ERP_ExtensibilityMacros
{
    public class ExtensibilityMacros : CustomCode

    {
        public void ImportDocuments()
        {
            CrossCompany.Manager mngr = new CrossCompany.Manager(PSO, BSO);

            mngr.ImportDocuments();
        }

    }
}
