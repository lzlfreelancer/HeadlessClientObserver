using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Documents;
using HeadlessClientTestResultReader;
using HeadlessClientTestResultDecipher;

namespace HeadlessClientTest
{
    public partial class Form1 : Form
    {
        public Form1(string[] args)
        {
            InitializeComponent();

            // run tests
            TestSuite tests = new TestSuite();
            tests.Init(outputText);
            tests.update();

            // default test file
            string pathToTextfile = "testLog.txt";

            if (args.Length != 0)
            {
                // or file via args
                pathToTextfile = args[0];
            }

            // generate report and print on screen
            readLogFileAndPrintResults(pathToTextfile);

        }

        private async void readLogFileAndPrintResults(string pathToTextfile)
        {
            try
            {
                StreamReader textFile = new StreamReader(pathToTextfile);
                string fileContents = textFile.ReadToEnd();
                textFile.Close();
                showResultsGeneratedWithTestLog(fileContents);
            }
            catch (System.IO.IOException)
            {
                await Task.Delay(10);
                readLogFileAndPrintResults(pathToTextfile);
            }
        }

        private void showResultsGeneratedWithTestLog(string fileContents)
        {

            TestReader testCaseReader = new TestReader();

            List<TestResultDecipher> resultsList = testCaseReader.runTests(fileContents);

            foreach (var result in resultsList)
            {
                outputText.AppendText(result.testName + "\n");
                outputText.AppendText(result.testDescription + "\n");

                foreach (var testStatus in result.statusList)
                {
                    if (testStatus.Item2)
                    {
                        // it's a pass
                        outputText.SelectionStart = outputText.TextLength;
                        outputText.SelectionLength = 0;

                        outputText.SelectionColor = Color.Green;
                        outputText.AppendText("\n" + testStatus.Item1 + " : PASSED " + "\n\n");
                        outputText.SelectionColor = outputText.ForeColor;

                    }
                    else
                    {
                        // Test failed
                        outputText.SelectionStart = outputText.TextLength;
                        outputText.SelectionLength = 0;

                        outputText.SelectionColor = Color.Red;
                        outputText.AppendText("\n" + testStatus.Item1 + " : FAILED - " + testStatus.Item3 + "\n\n");
                        outputText.SelectionColor = outputText.ForeColor;
                    }
                }

            }
        }

    }
}
