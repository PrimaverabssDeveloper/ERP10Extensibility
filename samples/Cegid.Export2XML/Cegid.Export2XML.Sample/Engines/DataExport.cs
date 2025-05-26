using Cegid.Importer.Entities;
using System;
using System.Collections.Generic;
namespace Export2XML
{

    /// <summary>
    /// Classe que implementa a exportação de dados para XML
    /// </summary>
    public class DataExport : IDisposable
    {
        #region Members

        #endregion

        #region Constructors

        public DataExport()
        {
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Procedimentos de Inicialização de Dados
        /// </summary>
        public void InitializeData()
        {
        }

        /// <summary>
        /// Devolve uma Lista de Países que já entrou como entidade genérica
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lstEntities"></param>
        /// <returns></returns>
        internal T GetCustomers<T>(T lstEntities)
        {
            if (lstEntities is List<Customer> lst)
            {
                foreach (Customer c in lst)
                {
                    c.Name = c.Name.ToUpper();
                }
            }

            return lstEntities;
        }

        /// <summary>
        /// Devolve uma Lista de Fornecedores que já entrou como entidade genérica
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lstEntities"></param>
        /// <returns></returns>
        internal T GetSuppliers<T>(T lstEntities)
        {
            if (lstEntities is List<Supplier> lst)
            {
                foreach (Supplier f in lst)
                {
                    f.Name = f.Name.ToUpper();
                }
            }
            return lstEntities;
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
