using System.Text.RegularExpressions;

namespace _09_Calculatrice_WPF
{
    public static class Calcul
    {
        public static double Get(string expression)
        {
            string initialExpression = expression;

            expression = Regex.Replace(expression, @"\s+", ""); //On supprime tous les espaces de l'expression
            expression = expression.Replace('.', ','); //On remplace les "." par des "," pour pas faire planter double.Tryparse()
            expression = expression.Replace('[', '('); //On remplace les crochets par des parenthèses
            expression = expression.Replace(']', ')');

            WrapExpression(ref expression, '*'); //Les multiplications et divisions sont prioriétaires vis à vis des additions et soustractions.
            WrapExpression(ref expression, '/'); // => donc on les entoure avec des parenthèses pour les traiter en premières.

            var pattern = @"\(([0-9,*+-/]*)\)"; // Pattern correponsdant à parenthèses ouvrante et fermante avec contenu ne contenant pas de parenthèses

            var matches = Regex.Matches(expression, pattern);

            while (matches.Count > 0) //Tant que notre expression contient des parenthèses
            {
                string m = matches[0].ToString(); // On prend le premier match de la liste

                expression = expression.Replace(m, CalculExpression(m)); //On le remplace dans l'expression par le resultat du calcul associé 

                matches = Regex.Matches(expression, pattern); // On vérifie si l'expression ainsi modifiée contient encore des parenthèses
            }

            //A ce stade notre expression ne contient plus de parenthèses.
            expression = CalculExpression(expression);

            if (initialExpression == expression)
            {
                throw new System.Exception("Expression non valide");
            }

            return double.Parse(expression);
        }

        private static void WrapExpression(ref string expression, char operateur)
        {
            string pattern = @"([-]?[0-9]+[.]?[0-9]*[" + operateur + @"][-]?[0-9]+[.]?[0-9]*)"; //pattern correspondant à un operateur entre 2 nombres décimaux

            var matches = Regex.Matches(expression, pattern);

            foreach (Match m in matches)
            {
                string contenu = m.Groups[1].ToString();

                expression = expression.Replace(contenu, "(" + contenu + ")");
            }
        }

        private static string CalculExpression(string operations)
        {
            operations = Regex.Replace(operations, @"[\(\)]*", ""); //On supprime d'éventuelles parenthèses englobantes de l'expression à évaluer

            CalculOperateur(ref operations, '*'); //On commence par traiter toutes les divisions ou multiplications...
            CalculOperateur(ref operations, '/');
            CalculOperateur(ref operations, '+'); //... et seulement après les additions et soustractions
            CalculOperateur(ref operations, '-');

            return operations;
        }

        private static void CalculOperateur(ref string expression, char opeRator)
        {
            string contenu;

            string pattern = @"([-]?[0-9]+[,]?[0-9]*[" + opeRator + @"][-]?[0-9]+[,]?[0-9]*)";

            var matches = Regex.Matches(expression, pattern);

            while (matches.Count > 0)
            {
                contenu = matches[0].ToString();

                string[] numbers = matches[0].ToString().Split(opeRator);

                if (double.TryParse(numbers[0], out double number1) && double.TryParse(numbers[1], out double number2))
                {
                    switch (opeRator)
                    {
                        case '*':

                            expression = expression.Replace(contenu, (number1 * number2).ToString());

                            break;

                        case '/':

                            expression = expression.Replace(contenu, (number1 / number2).ToString());

                            break;

                        case '+':

                            expression = expression.Replace(contenu, (number1 + number2).ToString());

                            break;

                        case '-':

                            expression = expression.Replace(contenu, (number1 - number2).ToString());

                            break;

                        default:

                            break;
                    }
                }

                matches = Regex.Matches(expression, pattern);
            }
        }
    }
}
