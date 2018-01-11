using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;

public class TestClient
{

    private const string m_path = "BZHeadlessClient.exe";
    private Process m_process;
    public bool hasClientProcessExited
    {
        get {
            if(m_process == null)
            {
                // process not started
                return false;
            }
            return m_process.HasExited;
        }
        set { }
    }
    protected RichTextBox m_output_text;

    public TestClient(RichTextBox outputText)
    {

        m_output_text = outputText;

    }


    public async void PerformAction(string cParams)
    {
        string p = "-m 1 ";
        await Task.Run(() =>
        {
            try
            {
                m_process = System.Diagnostics.Process.Start(m_path, p + cParams);
            }
            catch (Exception err)
            {
                m_output_text.Invoke(new Action(() =>
                {
                    m_output_text.SelectionStart = m_output_text.TextLength;
                    m_output_text.SelectionLength = 0;

                    m_output_text.SelectionColor = Color.Red;
                    m_output_text.AppendText("Error executing test client, check that you have BZHeadlessClient.exe under the correct folder. \n\n");
                    m_output_text.SelectionColor = m_output_text.ForeColor;
                }));
            }
            
        });
    }

}
