using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TextClassification.Business;
using TextClassification.Controller;
using TextClassification.Domain;
using TextClassification.FileIO;
using TextClassificationWPF._3_Business;

namespace TextClassification
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KnowledgeBuilder nb = new KnowledgeBuilder();
        Knowledge k;
        BagOfWords bof;
        List<string> entries;
        int kNN = 3;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TrainBtnClicked(object sender, RoutedEventArgs e)
        {
            DateTime start = DateTime.Now;
            nb.Train();
            this.k = nb.GetKnowledge();
            this.bof = this.k.GetBagOfWords();
            this.entries = this.bof.GetEntriesInDictionary();
            DateTime end = DateTime.Now;
            int timeTakenInMilliseconds = (end - start).Milliseconds;
            timeTaken.Text = "Time taken: " + timeTakenInMilliseconds.ToString() + " ms";

            foreach (string entry in this.entries)
            {
                dictOfWords.Items.Add(entry);
            }

            //PerformBenchmark();
            List<string> testFiles = GetAllFiles(testPath);
            List<bool> testVector = GetTestFileVector(testFiles[2]);
            string testLabel = VectorOperation.Classify(testVector, this.kNN, this.nb.GetVectors());
            MessageBox.Show(testLabel);

            dictLen.Text = dictOfWords.Items.Count.ToString() + " Words in Dictionary";
        }

        string trainingPath = "C:\\Users\\Oliver Bundgaard\\source\\repos\\TextClassification\\TextClassification\\bin\\Debug";
        private List<string> GetAllFiles(string path)
        {

            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] Files = d.GetFiles("*.txt");

            List<string> files = new List<string>();

            foreach (FileInfo file in Files)
            {
                files.Add(file.Name);
            }

            return files;
        }

        string currentClass = "none";
        private void ViewClassAFiles(object sender, RoutedEventArgs e)
        {
            List<string> files = GetAllFiles(trainingPath + "\\" + "ClassA");

            fileNames.Items.Clear();

            foreach (string file in files)
            {
                fileNames.Items.Add(file);
            }

            this.currentClass = "ClassA";

        }

        private void ViewClassBFiles(object sender, RoutedEventArgs e)
        {
            List<string> files = GetAllFiles(trainingPath + "\\" + "ClassB");

            fileNames.Items.Clear();

            foreach (string file in files)
            {
                fileNames.Items.Add(file);
            }

            this.currentClass = "ClassB";

        }

        private void SetTokenCount(object sender, RoutedEventArgs e)
        {
            if (fileNames.Items.Count > 0)
            {
                tokensInText.Text = GetTokenCount(fileNames.SelectedValue.ToString()).ToString() + " tokens in text";
            }
        }

        private int GetTokenCount(string file)
        {
            List<string> tokens =
                Tokenization.Tokenize(
                    File.ReadAllText(
                        trainingPath + "\\" + currentClass + "\\" + file
                    )
                );

            return tokens.Count;
        }

        void PerformBenchmark()
        {
            List<string> testFiles = GetAllFiles(testPath);

            string[] expectedLabel =
            {
                "Sport texts",
                "Sport texts",
                "Fairy tales",
                "Sport texts",
                "Fairy tales",
                "Fairy tales",
                "Fairy tales",
                "Fairy tales",
                "Sport texts",
                "Sport texts",
            };

            int correctTests = 0;
            for (int i = 0; i < testFiles.Count; i++)
            {
                List<bool> testVector = GetTestFileVector(testFiles[i]);
                string testLabel = VectorOperation.Classify(testVector, this.kNN, this.nb.GetVectors());

                if (testLabel == expectedLabel[i])
                {
                    correctTests++;
                }
            }
        }

        // Test path
        string testPath = "C:\\Users\\Oliver Bundgaard\\source\\repos\\TextClassification\\TextClassification\\bin\\Debug\\Test";
        private List<bool> GetTestFileVector(string fileName)
        {
            string filePath = testPath + "\\" + fileName;

            FileAdapter _fileAdapter = new TextFile("txt");

            List<bool> vector = new List<bool>();
            string text = _fileAdapter.GetAllTextFromTestFile(filePath);
            List<string> wordsInFile = Tokenization.Tokenize(text);
            foreach (string key in bof.GetAllWordsInDictionary())
            {
                if (wordsInFile.Contains(key))
                {
                    vector.Add(true);
                }
                else
                {
                    vector.Add(false);
                }
            }

            return vector;

        }
    }
}
