using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Cegid.Importer.Entities;

namespace Export2XML.XML
{
    [Export(typeof(IExportProcessor))]
    [ExportMetadata("ExportName", "Sample")]
    [ExportMetadata("ExportPlatTypes", "")]

    /// <summary>
    /// Classe base implementadora do MEF
    /// </summary>
    public class SampleProcessor : IExportProcessor
    {

        #region Members

        private string mySourceConfFolder = "";
        private DataExport myDataExport;
        private List<LogItem> myWarningList = new List<LogItem>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Validação das condições necessárias para a exportação
        /// </summary>
        /// <param name="pSourceConfFolder"></param>
        /// <returns></returns>
        public string ValidateEnvironment(string pSourceConfFolder)
        {
            // Validaçoes especifica se estão reunidas as condições para o passo seguinte
            string strError = string.Empty;

            return strError;
        }

        /// <summary>
        /// Implementações específicas da Tranformação de Dados
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public T LoadData<T>(T entity) where T : class
        {
            if (myDataExport == null)
                myDataExport = new DataExport();

            // Neste caso apenas vamos passar os nomes do clientes e fornecedores para maiúsculas
            switch (entity.GetType())
            {
                case Type t when t == typeof(List<Customer>):
                    return myDataExport.GetCustomers(entity);

                case Type t when t == typeof(List<Supplier>):
                    return myDataExport.GetSuppliers(entity);

            }

            return entity;
        }

        /// <summary>
        /// Devolve a definição dos Steps (UserControls ou outras definições) para dar a possibilidade da implementação pelo Provedor, de 2 passos adicionais
        /// </summary>
        /// <returns></returns>
        public StepDefinition GetStepDefinition()
        {
            // Não implementado neste provider
            return null;
        }

        /// <summary>
        /// Validações específicas do UserControl inicial/final
        /// </summary>
        /// <returns></returns>
        public bool ValidateStep(bool pConfigurationStep)
        {
            // Não implementado neste provider
            return true;
        }

        /// <summary>
        /// Indica se a empresa mudou ou não
        /// </summary>
        /// <returns></returns>
        public bool CompanyHasChanged()
        {
            return false;
        }

        /// <summary>
        /// Refrescamentos específicos do UserControl inicial/final
        /// </summary>
        /// <returns></returns>
        public void RefreshStep(bool pConfigurationStep)
        {
            // Não implementado neste provider
        }

        /// <summary>
        /// Executa operações Adicionais específicas
        /// </summary>
        public void ImportDataSpecifics(StepPhase pStep, string pConnectionString)
        {
            //
        }

        /// <summary>
        /// Lista de Avisos consruidos pelo Exportador, e que ficarão disponíveis no final do assistente
        /// </summary>
        /// <returns></returns>
        public List<LogItem> GetWarningList()
        {
            return myWarningList;
        }

        #endregion

    }
}