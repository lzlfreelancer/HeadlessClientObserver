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

    public async void PerformAction(string cParams)
    {
        string p = "-m 1 ";
        await Task.Run(() =>
        {
            m_process = System.Diagnostics.Process.Start(m_path, p + cParams);
        });
    }

}
