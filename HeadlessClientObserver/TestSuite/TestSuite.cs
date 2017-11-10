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

    public virtual void Init(RichTextBox outputText)
    {
        // clear log
        File.WriteAllText(m_log_path, String.Empty);

        Test1 testCase1 = new Test1();
        m_tests.Add(testCase1);

        Test2 testCase2 = new Test2();
        m_tests.Add(testCase2);

        Test3 testCase3 = new Test3();
        m_tests.Add(testCase3);

        Test4 testCase4 = new Test4();
        m_tests.Add(testCase4);

        Test5 testCase5 = new Test5();
        m_tests.Add(testCase5);

        foreach (BaseTest test in m_tests)
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

        bool hasAllTestEnded = true;

        foreach (BaseTest test in m_tests)
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

        await Task.Delay(1000);
        update(on_test_complete);

    }

}
