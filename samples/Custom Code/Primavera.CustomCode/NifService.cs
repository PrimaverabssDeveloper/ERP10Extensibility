using Primavera.Extensibility.CustomCode;
using Primavera.Extensibility.Extensions;

namespace Primavera.CustomNifService
{
    public class NifService : CustomCode
    {
        public void GetCustomerByNIF()
        {
            using (var result = BSO.Extensibility.CreateCustomFormInstance(typeof(EntityCreator)))
            {
                if (result.IsSuccess())
                {
                    (result.Result as EntityCreator).Show();
                }
            }
        }
    }
}
