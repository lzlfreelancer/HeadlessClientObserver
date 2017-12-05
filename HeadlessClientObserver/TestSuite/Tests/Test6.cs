/*
Test #6
Host 1:
Create Party 6, size 3

Client 1:
Join Party 6 (success)

Client 2:
Join Party 6 (success)

Client 3: 
Join Party 6 (success)

Host 2: 
Create Party 6.1 (success)

Host 1:
Join Party 6.1 (success)

Result:
All Clients should be added to the Host 1, Host 1 then joins host 2

*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test6 : BaseTest
{
    public override bool isDisabled
    {
        get { return false; }
        set { }
    }

    public Test6()
    {
        m_test_name = "Test #6";
        m_test_description =
            @"
Test #6
Host 1:
Create Party 6, size 3

Client 1:
Join Party 6 (success)

Client 2:
Join Party 6 (success)

Client 3: 
Join Party 6 (success)

Host 2: 
Create Party 6.1 (success)

Host 1:
Join Party 6.1 (success)

Result:
All Clients should be added to the Host 1, Host 1 then joins host 2       
        ";
    }

    public override void Init(RichTextBox outputText)
    {

        base.Init(outputText);

    }

    public override void StartTest()
    {
        base.StartTest();

        TestClient host1 = new TestClient();
        host1.PerformAction(String.Format("-c {0} -t 6 -a 1", Globals.clientIdGenerator.getNextId())); // create party, wait 40, join second party 
        m_clients.Add(host1);

        TestClient host2 = new TestClient();
        host2.PerformAction(String.Format("-c {0} -t 6 -a 2", Globals.clientIdGenerator.getNextId())); // create party, wait 100, exit
        m_clients.Add(host2);

        int numOfClients = 2;

        for (int i = 0; i < numOfClients; i++)
        {
            TestClient client = new TestClient();
            client.PerformAction(String.Format("-c {0} -t 6 -a 3", Globals.clientIdGenerator.getNextId())); // wait 10, join party, wait 70 exit
            m_clients.Add(client);
        }

    }

    public override void End()
    {
        base.End();
    }

}