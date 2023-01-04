using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CanCapter
{
    public partial class mainUserControl : UserControl
    {
        public mainUserControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserControlForEtudient userControlForEtudient = new UserControlForEtudient(this.main);
            this.main.Controls.Clear();
            this.main.Controls.Add(userControlForEtudient);
            userControlForEtudient.Height = main.Height;
            userControlForEtudient.Width = main.Width;
        }
    }
}
