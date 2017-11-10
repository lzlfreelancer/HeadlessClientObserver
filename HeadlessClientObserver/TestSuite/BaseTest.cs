using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class BaseTest
{

    protected string m_test_name = "Base test";
    protected string m_test_description = "Base test";
    protected List<TestClient> m_clients = new List<TestClient>();
    protected string m_log_path = "testLog.txt";
    protected RichTextBox m_text_output;
    protected bool m_has_test_ended = false;
    abstract public bool isDisabled { get; set; }
    public int TestId = 0;

    public virtual void Init(RichTextBox outputText)
    {
        TestId = Globals.testIdGenerator.getNextId();

        m_text_output = outputText;
    }

    public virtual void StartTest()
    {

        m_text_output.Invoke(new Action(() => {
            m_text_output.SelectionStart = m_text_output.TextLength;
            m_text_output.SelectionLength = 0;

            m_text_output.SelectionColor = Color.Blue;
            m_text_output.AppendText("\n" + "Starting test case: " + m_test_name + ", please wait... \n\n");
            m_text_output.SelectionColor = m_text_output.ForeColor;
        }));

        using (var writer = File.AppendText(m_log_path))
        {
            writer.WriteLine(String.Format("Starting test {0}", m_test_name));
        }
    }

    public virtual bool hasTestEnded(Action<Dictionary<string, string>> on_test_complete)
    {
        if (m_has_test_ended)
        {
            // test has already ended
            return true;
        }
        bool hasAllProcessesExited = true;
        foreach(TestClient testClient in m_clients)
        {
            if (!testClient.hasClientProcessExited)
            {
                hasAllProcessesExited = false;
            }
        }
        

        if (hasAllProcessesExited)
        {

            m_has_test_ended = true;
            End();
            Dictionary<string, string> testInfo = new Dictionary<string, string>();
            testInfo["testName"] = m_test_name;
            testInfo["testDescription"] = m_test_description;
            on_test_complete(testInfo);
            
        }
        return hasAllProcessesExited;
    }

    public virtual void End()
    {
        m_text_output.Invoke(new Action(() =>
        {
            m_text_output.SelectionStart = m_text_output.TextLength;
            m_text_output.SelectionLength = 0;

            m_text_output.SelectionColor = Color.Blue;
            m_text_output.AppendText("\n" + "Test case: " + m_test_name + " complete. \n\n");
            m_text_output.SelectionColor = m_text_output.ForeColor;
        }));
        
    }
}
