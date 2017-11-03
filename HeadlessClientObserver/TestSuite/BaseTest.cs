using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class BaseTest
{

    protected string m_test_name = "Base test";
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

        outputText.SelectionStart = outputText.TextLength;
        outputText.SelectionLength = 0;

        outputText.SelectionColor = Color.Blue;
        outputText.AppendText("\n" + "Starting test case: " + m_test_name + ", please wait. \n\n");
        outputText.SelectionColor = outputText.ForeColor;

    }

    public virtual void StartTest()
    {
        using (var writer = File.AppendText(m_log_path))
        {
            writer.WriteLine(String.Format("Starting test {0}", m_test_name));
        }
    }

    public virtual bool hasTestEnded()
    {
        if (m_has_test_ended)
        {
            return m_has_test_ended;
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
        }
        return hasAllProcessesExited;
    }

    public virtual void End()
    {

        m_text_output.SelectionStart = m_text_output.TextLength;
        m_text_output.SelectionLength = 0;

        m_text_output.SelectionColor = Color.Blue;
        m_text_output.AppendText("\n" + "Test case: " + m_test_name + " complete. \n\n");
        m_text_output.SelectionColor = m_text_output.ForeColor;

    }
}
