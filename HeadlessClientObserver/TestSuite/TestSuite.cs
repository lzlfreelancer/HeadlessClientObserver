using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class TestSuite
{

    private List<BaseTest> m_tests = new List<BaseTest>();
    protected string m_log_path = "testLog.txt";
    protected int m_num_tests_run = 0;
    protected int m_num_tests_active = 0;
    protected RichTextBox m_output_text;

    public virtual void Init(RichTextBox outputText)
    {
        m_output_text = outputText;

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

    }

    public virtual void StartTests(RichTextBox outputText)
    {
        foreach (BaseTest test in m_tests)
        {
            if (!test.isDisabled)
            {
                m_num_tests_run++;
                m_num_tests_active++;
                test.Init(outputText);
                test.StartTest();
            }
        }
    }

    public virtual List<BaseTest> GetTests()
    {
        return m_tests;
    }

    public virtual async void update(Action<Dictionary<string, string>> super_on_test_complete)
    {

        bool hasAllTestEnded = true;

        Action<Dictionary<string, string>> on_test_complete = testInfo =>
        {
            m_num_tests_active--;
            m_output_text.Invoke(new Action(() =>
            {
                m_output_text.SelectionStart = m_output_text.TextLength;
                m_output_text.SelectionLength = 0;

                m_output_text.SelectionColor = Color.Magenta;
                if(m_num_tests_active > 0)
                {
                    m_output_text.AppendText(m_num_tests_active + "/" + m_num_tests_run + " tests still running...\n\n");
                }else
                {
                    m_output_text.AppendText("All tests complete! \n\n");
                }
                m_output_text.SelectionColor = m_output_text.ForeColor;
            }));
            super_on_test_complete(testInfo);
            
        };

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
        update(super_on_test_complete);

    }

}
