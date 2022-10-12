using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TextClassification.Domain;

namespace TextClassificationWPF._3_Business
{
    public static class VectorOperation
    {

        public static String Classify(List<bool> unknown, int k, Vectors classes)
        {
            List<Pair> texts = new List<Pair>();

            List<bool> toCompare = new List<bool>();
            
            for (int a = 0; a < classes.GetVectorsInA().Count; a++)
            {
                toCompare = classes.GetVectorsInA()[a];
                int similarity = GetSimilarity(unknown, toCompare);
                //MessageBox.Show($"Similarity {similarity}");
                texts.Add(new Pair("Sport texts", similarity));
            }

            toCompare.Clear();

            for (int b = 0; b < classes.GetVectorsInB().Count; b++)
            {
                toCompare = classes.GetVectorsInB()[b];
                int similarity = GetSimilarity(unknown, toCompare);
                //MessageBox.Show($"Similarity {similarity}");
                texts.Add(new Pair("Fairy tales", similarity));
            }

            List<Pair> sortedTexts = texts.OrderByDescending(p => p.similarity).ToList();

            

            int sportTexts = 0;
            int fairyTales = 0;
            
            for (int i = 0; i < k; i++)
            {
                if (sortedTexts[i].label == "Sport texts")
                {
                    sportTexts++;
                } else
                {
                    fairyTales++;
                }
            }

            if (sportTexts >= fairyTales)
            {
                return "Sport texts";
            } else
            {
                return "Fairy tales";
            }
        }

        private static int GetSimilarity(List<bool> unknown_vector, List<bool> known_vector)
        {

            if (unknown_vector.Count != known_vector.Count)
            {
                MessageBox.Show("Vectors are not equal");
                return 0;
            }

            int points = 0;

            for (int i = 0; i < unknown_vector.Count; i++)
            {
                if (unknown_vector[i] == known_vector[i])
                {
                    points++;
                }
            }

            return points;
        }

    }

    class Pair
    {
        public string label { get; set; }
        public int similarity { get; set; }

        public Pair (string pLabel, int pSimilarity)
        {
            this.label = pLabel;
            this.similarity = pSimilarity;
        }
    }
}
