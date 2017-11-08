using System;
using System.IO;
using System.Collections;
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

        private string m_file_path = "testLog.txt";

        public Form1(string[] args)
        {
            InitializeComponent();

            if (args.Length != 0)
            {
                // file path via args
                m_file_path = args[0];
            }
            // run tests
            runTests();

        }

        private async void runTests()
        {
            // declear what happens when a test finishes
            Action<Dictionary<string, string>> on_test_complete = testInfo =>
            {
                readLogFileAndPrintResults(testInfo);
            };

            await Task.Run(() =>
            {
                TestSuite tests = new TestSuite();
                tests.Init(outputText);
                tests.update(on_test_complete);
            });
        } 

        private async void readLogFileAndPrintResults(Dictionary<string, string> testInfo)
        {
            try
            {
                StreamReader textFile = new StreamReader(m_file_path);
                string fileContent = textFile.ReadToEnd();
                textFile.Close();
                showResultsGeneratedWithTestLog(testInfo, fileContent);
            }
            catch (System.IO.IOException)
            {
                await Task.Delay(10);
                readLogFileAndPrintResults(testInfo);
            }
        }

        private void showResultsGeneratedWithTestLog(Dictionary<string, string> testInfo, string fileContent)
        {

            TestReader testCaseReader = new TestReader();

            TestResultDecipher result = testCaseReader.runTest(testInfo, fileContent);

            result.assesTestResult();

            outputText.Invoke(new Action(() =>
            {
                resultToScreen(result);
            }));
            
        }

        private void resultToScreen(TestResultDecipher result)
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
