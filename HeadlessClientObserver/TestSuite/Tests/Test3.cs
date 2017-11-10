/*
Test #3
Host:
Create Private Party with name "x" password "y"

Client 1:
Search for room with proper name and password (success)

Client 2:
Search for room with proper name and random password (fail)

Client 3:
Search for room with wrong name (fail)
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test3 : BaseTest
{
    public override bool isDisabled
    {
        get { return false; }
        set { }
    }

    public Test3()
    {
        m_test_name = "Test #3";
        m_test_description =
            @"
Test #3
Host:
Create Private Party with name 'x' password 'y'

Client 1:
Search for room with proper name and password(success)

Client 2:
Search for room with proper name and random password(fail)

Client 3:
Search for room with wrong name(fail)
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
        host.PerformAction(String.Format("-c {0} -t 3 -a 1", Globals.clientIdGenerator.getNextId())); // create party, wait 10, exit
        m_clients.Add(host);

        TestClient client1 = new TestClient();
        client1.PerformAction(String.Format("-c {0} -t 3 -a 2", Globals.clientIdGenerator.getNextId())); // search success
        m_clients.Add(client1);

        TestClient client2 = new TestClient();
        client2.PerformAction(String.Format("-c {0} -t 3 -a 3", Globals.clientIdGenerator.getNextId())); // search fail wrong password
        m_clients.Add(client2);

        TestClient client3 = new TestClient();
        client3.PerformAction(String.Format("-c {0} -t 3 -a 4", Globals.clientIdGenerator.getNextId())); // search fail wrong name
        m_clients.Add(client3);

    }

    public override void End()
    {
        base.End();
    }

}
