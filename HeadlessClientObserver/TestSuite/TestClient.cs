using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TestClient
{

    private const string m_path = "BZHeadlessClient.exe";
    private Process m_process;
    public bool hasClientProcessExited
    {
        get { return m_process.HasExited; }
        set { }
    }

    public void PerformAction(string cParams)
    {
        m_process = System.Diagnostics.Process.Start(m_path, cParams);
    }

}
