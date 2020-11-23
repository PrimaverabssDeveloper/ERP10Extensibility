using System;
using Primavera.CustomNifService.RootEntity;
using System.Windows.Forms;
using Primavera.Extensibility.CustomForm;

namespace Primavera.CustomNifService
{
    public partial class EntityCreator : CustomForm
    {
        public EntityCreator()
        {
            InitializeComponent();
        }

        private void cmdSearch_Click(object sender, EventArgs e)
        {
  
            EntitySupport entity = GetNIF.GetFromNIFPT(txtnif.Text);

            if (entity != null && entity.result != "error")
            {
                Location loc = entity.records[Convert.ToInt32(txtnif.Text)];

                txtAdress.Text = loc.address;
                txtdescription.Text = loc.title;
                txtCity.Text = loc.city;
                txtZipCode.Text = loc.pc4 + "-" + loc.pc3;
                txtPhone.Text = loc.contacts.phone;
            }
            else
            {
                MessageBox.Show("No match found for the given NIF.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
