using System;
using System.IO;
using System.Collections.Generic;
using HeadlessClientTestResultDecipher;

namespace HeadlessClientTestResultReader
{

    public class TestReader
    {

        private List<TestResultDecipher> resultsList = new List<TestResultDecipher>();
        private TestDecipherFactory testDecipherFactory = new TestDecipherFactory();
        bool doReadTestResult = false;

        public List<TestResultDecipher> runTests(string fileContent)
        {

            using (StringReader reader = new StringReader(fileContent))
            {
                string line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        // do something with the line
                        this.readLine(line);
                    }

                } while (line != null);
            }

            foreach(var result in this.resultsList)
            {
                result.assesTestResult();
            }

            return this.resultsList;

        }

        public void readLine(string line)
        {

            string[] words = line.Split(';');

            if(words.Length == 2)
            {
                // new test case
                TestResultDecipher newResult = testDecipherFactory.createTestDecipher(words[0]);
                if(newResult != null)
                {
                    this.doReadTestResult = true;
                }else
                {
                    // this is not a valid test case, disable reading until a valid test case appears
                    this.doReadTestResult = false;
                }
                if (this.doReadTestResult)
                {
                    this.resultsList.Add(newResult);
                    newResult.testName = words[0];
                    newResult.testDescription = words[1];
                }
                
            }

            if(this.doReadTestResult && this.resultsList.Count > 0)
            {

                TestResultDecipher currentDecipher = this.resultsList[this.resultsList.Count - 1];
                if (currentDecipher != null)
                {
                    if (words.Length >= 3)
                    {
                        TestActionRecord testActionRecord = new TestActionRecord();
                        testActionRecord.type = words[0];
                        testActionRecord.id = Convert.ToInt32(words[1]);
                        testActionRecord.action = words[2];
                        if (words.Length > 3)
                        {
                            for(var i = 3; i < words.Length; i++)
                            {
                                string reference = words[i];
                                testActionRecord.references.Add(reference);
                            }
                        }
                        currentDecipher.addTestActionRecord(testActionRecord);
                    }
                }

            }
            
        }

    }
}