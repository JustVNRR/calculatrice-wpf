using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _09_Calculatrice_WPF.Ressources
{
    public partial class CustomResources : ResourceDictionary
    {
        public void Key_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Test");
        }
    }
}
