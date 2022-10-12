using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextClassification.Business
{

    public class Tokenization
    {
        private const int SMALLESTWORDLENGTH = 3;

        public static List<string> Tokenize(string originalText)
        {
            List<string> words = new List<string>();
            String [] tokens = originalText.Split(' ');

            
            foreach (string token in tokens)
            {
                if (!IsAShortWord(token)){
                    string cleanWord = RemovePunctuation(token);
                    cleanWord = cleanWord.ToLower();
                    words.Add(cleanWord);
                }
            }
            return words;
        }

        public static bool IsAShortWord(string token)
        {
            if (token.Length < SMALLESTWORDLENGTH)
            {
                return true;
            }
            return false;
        }

        public static string RemovePunctuation(string parsedString)
        {
            // We want to allow some punctuation
            List<string> allowedPunctuation = new List<string>() { "'", "-" };

            /* 
             *  We check every character, if it's allowed or a letter/digit add it
             *  Then we make it into a string
             */
            string returnValue = new string(
                (from c in parsedString
                 where Char.IsLetterOrDigit(c) || allowedPunctuation.Contains(c.ToString())
                 select c
                 ).ToArray());

            // Return the value
            return returnValue;
        }
    }
}
