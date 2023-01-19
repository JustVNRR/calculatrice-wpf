using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _09_Calculatrice_WPF
{
    /*
     * Pour déployer l'application en ClickOnce
     * 
     * https://youtu.be/ay3gYyxOWI4
     * 
     * Dans les propriétés => signature => "créer un certificat de test" (mot de passe chois : "test")
     * Dans les propriétés => publier => options (par ex pour mettre un racourcis sur bureau)
     * Dans les propriétés => publier => "publier maintenant"
     * 
     **/

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Pour créer la méthode depuis le designer, clic droit => atteindre la définition
        private void Key_Click(object sender, RoutedEventArgs e)
        {
            //if (sender is not Button button) return;

            if (e.Source is not Button button) return;

            string? key = button?.Content.ToString();

            if(String.IsNullOrEmpty(key)) return;

            int currentPos = txt_Operation.SelectionStart;

            switch (key)
            {
                case "DEL":

                    if (currentPos > 0)
                    {
                        txt_Operation.Text = txt_Operation.Text.Remove(currentPos - 1, 1); ;
                        txt_Operation.SelectionStart = currentPos - 1;
                    }

                    break;

                case "C":

                    txt_Operation.Clear();
                    txt_Result.Text = "";

                    break;

                case "=":

                    try
                    {
                        txt_Result.Foreground = Brushes.Black;
                        txt_Result.Text = Calcul.Get(txt_Operation.Text).ToString();
                    }
                    catch
                    {
                        txt_Result.Foreground = Brushes.Red;
                        txt_Result.Text = "Expression non valide";
                    }

                    break;

                default:

                    txt_Operation.Text = txt_Operation.Text.Insert(currentPos, key.ToString());
                    txt_Operation.SelectionStart = currentPos + 1;

                    break;
            }
        }
    }
}
