/*
Test #6
Host 1:
Create Party 6, size 5

Client 1:
Join Party 6 with 4 slots (success)  

Total members should be 5

Client 1:
leave Party 6, all local users should leave as well (success)  

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
Create Party 6, size 5

Client 1:
Join Party 6 with 4 slots (success)  

Total members should be 5

Client 1:
leave Party 6, all local users should leave as well (success)  
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
        host.PerformAction(String.Format("-c {0} -t 6 -a 1", Globals.clientIdGenerator.getNextId())); // create party, wait 40
        m_clients.Add(host);

        TestClient client = new TestClient();
        client.PerformAction(String.Format("-c {0} -t 6 -a 2", Globals.clientIdGenerator.getNextId())); // join party with 4 people, wait 10, exit
        m_clients.Add(client);

    }

    public override void End()
    {
        base.End();
    }

}