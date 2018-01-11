/*
Test #2:
Host1:
Create Party named "n"

Host 2:
Create Party named "n"

Result:
Host 2 should not be able to create this party.
*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test2 : BaseTest
{
    public override bool isDisabled
    {
        get { return false; }
        set { }
    }

    public Test2()
    {
        m_test_name = "Test #2";
        m_test_description =
            @"
Test #2:
Host1:
Create Party named 'n'

Host 2:
Create Party named 'n'

Result:
Host 2 should not be able to create this party.
";
    }

    public override void Init(RichTextBox outputText)
    {
        base.Init(outputText);
    }

    public override void StartTest()
    {
        base.StartTest();

        TestClient host = new TestClient(m_text_output);
        host.PerformAction(String.Format("-c {0} -t 2 -a 1", Globals.clientIdGenerator.getNextId())); // create party, wait 10, exit
        m_clients.Add(host);

        TestClient host2 = new TestClient(m_text_output);
        host2.PerformAction(String.Format("-c {0} -t 2 -a 1", Globals.clientIdGenerator.getNextId())); // host 2 do the same
        m_clients.Add(host2);

    }

    public override void End()
    {
        base.End();
    }

}
