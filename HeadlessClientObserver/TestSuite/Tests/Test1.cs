/*
Test #1
Host 1:
Create Party 1, size 4

Client 1:
Join Party 1 (success)

Client 2:
Join Party 1 (success)

Client 3: 
Join Party 1 (success)

Client 4: 
Join Party 1 (fail)

Result:
All Clients should be added to the Host

    //HOST1 CREATE_PARTY, WAIT 30 seconds, QUIT
    //CLIENT1 SEARCH_PARTY, JOIN_PARTY, Wait 10 seconds, LEAVE_PARTY, QUIT
    //CLIENT2 SEARCH_PARTY, JOIN_PARTY, Wait 10 seconds, LEAVE_PARTY, QUIT
    //CLIENT3 SEARCH_PARTY, JOIN_PARTY, Wait 10 seconds, LEAVE_PARTY, QUIT

*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test1: BaseTest
{
    public override bool isDisabled
    {
        get { return false; }
        set { }
    }

    public Test1()
    {
        m_test_name = "Test #1";
        m_test_description =
            @"
Test #1
Host 1:
Create Party 1, size 4

Client 1:
Join Party 1 (success)

Client 2:
Join Party 1 (success)

Client 3: 
Join Party 1 (success)

Client 4: 
Join Party 1 (fail)

Result:
All Clients should be added to the Host

//HOST1 CREATE_PARTY, WAIT 30 seconds, QUIT
//CLIENT1 SEARCH_PARTY, JOIN_PARTY, Wait 10 seconds, LEAVE_PARTY, QUIT
//CLIENT2 SEARCH_PARTY, JOIN_PARTY, Wait 10 seconds, LEAVE_PARTY, QUIT
//CLIENT3 SEARCH_PARTY, JOIN_PARTY, Wait 10 seconds, LEAVE_PARTY, QUIT           
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
        host.PerformAction(String.Format("-c {0} -t 1 -a 1", Globals.clientIdGenerator.getNextId())); // create party, wait 30, exit
        m_clients.Add(host);

        int numOfClients = 4;

        for(int i = 0; i < numOfClients; i++)
        {
            TestClient client = new TestClient();
            client.PerformAction(String.Format("-c {0} -t 1 -a 2", Globals.clientIdGenerator.getNextId())); // join party, wait 10 exit
            m_clients.Add(client);
        }

    }

    public override void End()
    {
        base.End();
    }

}
