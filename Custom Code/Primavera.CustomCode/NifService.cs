using Primavera.Extensibility.CustomCode;

namespace Primavera.CustomNifService
{
    public class NifService : CustomCode
    {
        public void GetCustomerByNIF()
        {
            frmEntityCreator entityCreator = new frmEntityCreator();
            entityCreator.ShowDialog();
        }
    }
}
