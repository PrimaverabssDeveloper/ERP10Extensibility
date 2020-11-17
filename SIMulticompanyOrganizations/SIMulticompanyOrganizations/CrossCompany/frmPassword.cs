using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.CustomForm;

namespace SUGIMPL_OME.CrossCompany
{
    public partial class frmPassword : Form 
    {
        public string Password { get; set; }

        public frmPassword()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void frmPassword_Load(object sender, EventArgs e)
        {
            txtPassword.DataBindings.Add("Text", this, "Password");
        }

    }
}
