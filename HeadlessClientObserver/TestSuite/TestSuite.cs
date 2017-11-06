using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class TestSuite
{

    private List<BaseTest> m_tests = new List<BaseTest>();
    protected string m_log_path = "testLog.txt";
    protected bool m_has_tests_completed = false;

    public virtual void Init(RichTextBox outputText)
    {
        // clear log
        File.WriteAllText(m_log_path, String.Empty);

        Test1 testCase1 = new Test1();
        m_tests.Add(testCase1);

        Test2 testCase2 = new Test2();
        m_tests.Add(testCase2);

        foreach(BaseTest test in m_tests)
        {
            if (!test.isDisabled)
            {
                test.Init(outputText);
                test.StartTest();
            }
        }

    }

    public virtual async void update(Action<Dictionary<string, string>> on_test_complete)
    {

        while (!m_has_tests_completed)
        {
            // check if all the tests has ended

            bool hasAllTestEnded = true;

            foreach(BaseTest test in m_tests)
            {
                if (!test.isDisabled && !test.hasTestEnded(on_test_complete))
                {
                    hasAllTestEnded = false;
                }
            }

            if (hasAllTestEnded)
            {
                return;
            }

            await Task.Delay(300);
            update(on_test_complete);

        }

    }

}
