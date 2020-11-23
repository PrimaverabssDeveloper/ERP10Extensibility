using Primavera.Extensibility.CustomCode;
using Primavera.Extensibility.Extensions;

namespace PrimaveraSDK
{
    public class MainCustomCodeClass : CustomCode
    {
        public void ShowDemo1()
        {
            using (var result = BSO.Extensibility.CreateCustomFormInstance(typeof(frmDemo1)))
            {
                if (result.IsSuccess())
                {
                    (result.Result as frmDemo1).Show();
                }
            }
        }

        public void ShowDemoGrid()
        {
            using (var result = BSO.Extensibility.CreateCustomFormInstance(typeof(frmDemoGrid)))
            {
                if (result.IsSuccess())
                {
                    (result.Result as frmDemoGrid).Show();
                }
            }
        }
    }
}
