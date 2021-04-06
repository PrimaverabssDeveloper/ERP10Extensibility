using System;
using System.Windows.Forms;

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
