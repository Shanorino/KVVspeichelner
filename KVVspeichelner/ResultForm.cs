using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KVVspeichelner
{
    public partial class ResultForm : Form
    {
        List<fahrtInfo> fi = new List<fahrtInfo>();
        public ResultForm()
        {
            //this.fi = summarise;
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void ResultForm_Load(object sender, EventArgs e)
        {
            
        }
    }
}
