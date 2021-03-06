﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using HeadlessClientTestResultDecipher;

namespace HeadlessClientTestResultReader
{

    public class TestReader
    {

        private List<TestResultDecipher> resultsList = new List<TestResultDecipher>();
        private TestDecipherFactory testDecipherFactory = new TestDecipherFactory();

        public TestResultDecipher runTest(Dictionary<string, string> testInfo, string fileContent)
        {
            TestResultDecipher testDecipher = testDecipherFactory.createTestDecipher(testInfo["testName"]);
            testDecipher.testName = testInfo["testName"];
            testDecipher.testDescription = testInfo["testDescription"];

            using (StringReader reader = new StringReader(fileContent))
            {
                string line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        // do something with the line
                        readLineForTest(testInfo["testName"], line, testDecipher);
                    }

                } while (line != null);
            }

            return testDecipher;
        }

        public TestResultDecipher readLineForTest(string testName, string line, TestResultDecipher testDecipher)
        {

            // Test result goes something like this:
            // testName;ClientId;Action;args
            // has to have 3 words min, args are optional

            string[] words = line.Split(';');

            if(words[0] != testName || words.Length < 3)
            {
                // This is not the required test case, or data is not valid
                return testDecipher;
            }

            if (testDecipher != null)
            {
                if (words.Length == 3)
                {
                    // this is a startAction / endAction statement
                    TestActionRecord testActionRecord = new TestActionRecord();
                    testActionRecord.type = words[1];
                    testActionRecord.action = words[2];
                    testDecipher.addTestActionRecord(testActionRecord);
                }

                else if (words.Length >= 4)
                {
                    TestActionRecord testActionRecord = new TestActionRecord();
                    testActionRecord.type = words[1];
                    testActionRecord.photonId = words[2];
                    testActionRecord.action = words[3];
                    if (words.Length > 4)
                    {
                        for (var i = 4; i < words.Length; i++)
                        {
                            string reference = words[i];
                            testActionRecord.references.Add(reference);
                        }
                    }
                    testDecipher.addTestActionRecord(testActionRecord);
                }
            }

            return testDecipher;

        }

    }
}