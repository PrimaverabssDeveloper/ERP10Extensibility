using System;

namespace Balancas
{
    public class ScaleSample : VndBE100.IProxyBalanca, IDisposable
    {
        #region Private Fields
        #endregion Private Fields

        #region Public Constructors

        public ScaleSample()
        {
        }

        #endregion Public Constructors


        #region Public Properties

        public int Status
        {
            get
            {
                return 0;
            }
        }

        public int TempoAmostragem { get => 0; set => throw new NotImplementedException(); }

        public int UltimoErro
        {
            get
            {
                return 1;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Config()
        {
        }

        public string DaDescritivoErro(int iNumErro = -1)
        {
            if (iNumErro == -1)
                iNumErro = 0;

            switch (iNumErro)
            {
                case 0:
                    return "OK";

                default:
                    return "OK";
            }
        }

        //End Function
        public bool DaPeso(ref double nRet)
        {
            nRet = 99;
            return true;
        }

        public bool DaPesoEstavel(ref double nRet, int tAmost = 3000)
        {
            nRet = 99;
            return true;
        }

        public bool Inicializa(string sPosto, string sCaminho)
        {
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Termina()
        {
            this.Dispose();
        }

        #endregion Public Methods

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        ~ScaleSample()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                disposedValue = true;
            }
        }

        #endregion IDisposable Support
    }
}