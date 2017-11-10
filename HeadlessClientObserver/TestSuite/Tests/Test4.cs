/*
Test #4:
Host 1:
Create Party

Client 1:
Join party

Client 1:
Disconnect without leaving

Result;
Host should kick the user after the heartbeat timer elapses
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test4 : BaseTest
{
    public override bool isDisabled
    {
        get { return false; }
        set { }
    }

    public Test4()
    {
        m_test_name = "Test #4";
        m_test_description =
            @"
Test #4:
Host 1:
Create Party

Client 1:
Join party

Client 1:
Disconnect without leaving

Result;
Host should kick the user after the heartbeat timer elapses
";
    }

    public override void Init(RichTextBox outputText)
    {
        base.Init(outputText);
    }

    public override void StartTest()
    {
        base.StartTest();

        TestClient host = new TestClient();
        host.PerformAction(String.Format("-c {0} -t 4 -a 1", Globals.clientIdGenerator.getNextId())); // create party, wait 40, exit
        m_clients.Add(host);

        TestClient client1 = new TestClient();
        client1.PerformAction(String.Format("-c {0} -t 4 -a 2", Globals.clientIdGenerator.getNextId())); // wait 10, join party, wait 10, stop heatbeat
        m_clients.Add(client1);

    }

    public override void End()
    {
        base.End();
    }

}