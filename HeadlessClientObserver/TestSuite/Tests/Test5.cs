/*
Test #5:
Host 1:
Create Party

Client 1:
Join Party

Client 2:
Join Party

Host 1:
Disconnect without leaving

Result:
All users should disband from the party after the heartbeat timer elapses
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test5 : BaseTest
{
    public override bool isDisabled
    {
        get { return false; }
        set { }
    }

    public Test5()
    {
        m_test_name = "Test #5";
        m_test_description =
            @"
Test #5:
Host 1:
Create Party

Client 1:
Join Party

Client 2:
Join Party

Host 1:
Disconnect without leaving

Result:
All users should disband from the party after the heartbeat timer elapses
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
        host.PerformAction(String.Format("-c {0} -t 5 -a 1", Globals.clientIdGenerator.getNextId())); // create party, wait 10, stop heatbeat
        m_clients.Add(host);

        TestClient client1 = new TestClient(m_text_output);
        client1.PerformAction(String.Format("-c {0} -t 5 -a 2", Globals.clientIdGenerator.getNextId())); // wait 10, join party
        m_clients.Add(client1);

        TestClient client2 = new TestClient(m_text_output);
        client2.PerformAction(String.Format("-c {0} -t 5 -a 2", Globals.clientIdGenerator.getNextId())); // wait 10, join party
        m_clients.Add(client2);

    }

    public override void End()
    {
        base.End();
    }

}
