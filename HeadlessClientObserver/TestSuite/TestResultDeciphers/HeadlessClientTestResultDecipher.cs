using System;
using System.Collections.Generic;

namespace HeadlessClientTestResultDecipher
{

    // 3 element tuple describing the status of a test result, with the frist element 
    // being the description of the test, second element a boolean that states of the test
    // has passed, and an extra element as error message should the test be a failure
    public class TestStatusList<T1, T2, T3> : List<Tuple<T1, T2, T3>>
    {
        public void Add(T1 item, T2 item2, T3 item3)
        {
            Add(new Tuple<T1, T2, T3>(item, item2, item3));
        }
    }

    // returns the test decipher object for a given test case, given the name of the test case
    public class TestDecipherFactory
    {
        public TestResultDecipher createTestDecipher(string testName)
        {

            switch (testName)
            {
                case "Test #1":
                    return new TestDecipher1();
                case "Test #2":
                    return new TestDecipher2();
                case "Test #3":
                    return new TestDecipher3();
                case "Test #4":
                    return new TestDecipher4();
                case "Test #5":
                    return new TestDecipher5();
                case "Test #6":
                    return new TestDecipher6();
                case "Test #7":
                    return new TestDecipher7();
                default:
                    return null;
            }

        }
    }

    public class TestActionRecord
    {
        public string type = "client";
        public int id = -1;
        public string photonId = "";
        public string action = "";
        public List<string> references = new List<string>();
    }

    public class TestResultDecipher
    {
        protected bool didPass = false;
        protected List<TestActionRecord> records = new List<TestActionRecord>();
        public string testName = "";
        public string testDescription = "";
        public TestStatusList<string, bool, string> statusList = new TestStatusList<string, bool, string>();

        public bool didTestPass()
        {
            return this.didPass;
        }

        public virtual void assesTestResult()
        {

        }

        public void addTestActionRecord(TestActionRecord rec)
        {
            if(rec != null)
            {
                this.records.Add(rec);
            }
        }

        // asset that there are certain number of individual instances that dumps data 
        // (but multiple dumps per instance is allowed) 
        public void assertNumberOfIndividualEntities(int count)
        {
            HashSet<string> setOfIds = new HashSet<string>();

            foreach(var record in this.records) {
                if(record.photonId == "")
                {
                    this.statusList.Add(String.Format("Assert {0} individual entities", count), false, "Invalid id of empty string");
                    return;
                }
                setOfIds.Add(record.photonId);
            }

            if(setOfIds.Count == count)
            {
                this.statusList.Add(String.Format("Assert {0} individual entities", count), true, "");
                return;
            }else
            {
                this.statusList.Add(String.Format("Assert {0} individual entities", count), false, String.Format("{0} individual entities present, sould be {1}", setOfIds.Count, count));
                return;
            }

        }

        // asset the mumber of dump messages in the test
        public void assertNumberOfMessages(int count)
        {

            if(this.records.Count == count)
            {
                this.statusList.Add(String.Format("Expecting {0} dump messages", count), true, "");
                return;
            }else
            {
                this.statusList.Add(String.Format("Expecting {0} dump messages", count), false, String.Format("Expecting {0} dump messages, counted {1}", count, this.records.Count));
                return;
            }

        }


        // make sure the test results contains the right combination of actions. Any additional actions outside of the
        // expected combination are ignored.
        public void assertActionCombination(List<Tuple<string, int>> actionList, string userType = "")
        {

            if(actionList.Count == 0)
            {
                return;
            }

            foreach (var tuple in actionList)
            {

                // Write description of test
                string description = "Expecting";
                string action = tuple.Item1;
                description = description + String.Format(" {0} occurances of action {1}.", tuple.Item2, action);

                // check the occurances
                int count = 0;
                int expectedCount = tuple.Item2;
                foreach (var record in this.records)
                {

                    if (record.action == action)
                    {
                        if(userType != "" && record.type != userType)
                        {
                            // this is not the right type of user
                            continue;
                        }
                        count++;
                    }

                }
                if (count != expectedCount)
                {
                    this.statusList.Add(description, false, "But counted "+ count);
                }else
                {
                    this.statusList.Add(description, true, "");
                }
            }

        }

        // make sure the test results contains the right combination of action-arguments. Any additional actions or arguments
        // outside of the expected combination are ignored.
        public void assertActionArgumentsCombination(List<Tuple<List<string>, int>> actionList, string userType = "")
        {

            if (actionList.Count == 0)
            {
                return;
            }

            foreach (var tuple in actionList)
            {

                // Write description of test
                string description = "Expecting";
                List<string> arguments = tuple.Item1;
                description = description + String.Format(" {0} occurances of action: '{1}'", tuple.Item2, arguments[0]);
                if(userType != "")
                {
                    description = description + " for user type: '"+userType+"'";
                }
                if(arguments.Count > 1)
                {
                    description = description + " followed by arguments: ";
                    for(int i = 1; i < arguments.Count; i++)
                    {
                        string arg = arguments[i];
                        description = description + "'" + arg +"', ";
                    }
                    int p1 = description.LastIndexOf(",");
                    description = description.Remove(p1, 1).Insert(p1, ".");
                }

                // check the occurances
                int count = 0;
                int expectedCount = tuple.Item2;
                foreach (var record in this.records)
                {

                    bool matchingArguments = true;
                    if (record.action == arguments[0])
                    {
                        if (arguments.Count > 1)
                        {
                            for (int i = 1; i < arguments.Count; i++)
                            {
                                string arg = arguments[i];
                                if (record.references.Count >= i)
                                {
                                    if (record.references[i - 1] != arg)
                                    {
                                        // one of the args does not match
                                        matchingArguments = false;
                                    }
                                }
                                else
                                {
                                    // the args on the log is at least one short
                                    matchingArguments = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        // action not a match
                        matchingArguments = false;
                    }
                    if (matchingArguments)
                    {
                        if(userType != "" && record.type != userType)
                        {
                            // this is not the right type of user
                            continue;
                        }
                        count++;
                    }

                }
                if (count != expectedCount)
                {
                    this.statusList.Add(description, false, "But counted " + count);
                }else
                {
                    this.statusList.Add(description, true, "");
                }

            }
        }


        // asset the order of actions, in this case instance id does not matter 
        public void assertOrderingOfAction(List<string> actionList)
        {

            if(actionList.Count == 0)
            {
                return;
            }

            string description = "Expecting action";
            foreach (var action in actionList)
            {
                description = description + String.Format(" {0} followed by", action);
            }

            int place = description.LastIndexOf(" followed by");
            description = description.Remove(place, 12).Insert(place, ".");

            if(this.records.Count < actionList.Count)
            {
                this.statusList.Add(description, false, String.Format("Expecting {0} dump messages, counted {1}", actionList.Count, this.records.Count));
                return;
            }

            for(var i = 0; i < actionList.Count; i++)
            {
                if(actionList[i] != this.records[i].action)
                {
                    this.statusList.Add(description, false, String.Format("Expecting action {0} on index {1}, but got {2}", actionList[i], i, this.records[i].action));
                    return;
                }
            }

            this.statusList.Add(description, true, "");
            return;

        }

        // asset that all reference strings match a certain input
        public void assertReferenceMatching(List<string> referenceList)
        {

            if (referenceList.Count == 0)
            {
                return;
            }

            string description = "Expecting references";
            foreach (var reference in referenceList)
            {
                description = description + String.Format(" {0} followed by", reference);
            }
            int place = description.LastIndexOf(" followed by");
            description = description.Remove(place, 12).Insert(place, ".");

            foreach (var record in this.records)
            {

                if (record.references.Count < referenceList.Count)
                {
                    this.statusList.Add(description, false, String.Format("Instance {0} missing one or more references", record.id));
                    return;
                }
                else
                {
                    for(var i = 0; i < referenceList.Count; i++)
                    {
                        string reference = referenceList[i];
                        if(reference != record.references[i])
                        {
                            this.statusList.Add(description, false, String.Format("Expecting reference string on index {0} of instance {1} to be {2}, got {3} instead", i, record.id, reference, record.references[i]));
                            return;
                        }
                    }
                }

            }

            this.statusList.Add(description, true, "");
            return;

        }

        // asset that all reference strings match a certain input for instance id
        public void assertReferenceMatching(int id, List<string> referenceList)
        {

            if (referenceList.Count == 0)
            {
                return;
            }

            string description = String.Format("Expecting references for instance {0} to be ", id);
            foreach (var reference in referenceList)
            {
                description = description + String.Format(" {0} followed by", reference);
            }
            int place = description.LastIndexOf(" followed by");
            description = description.Remove(place, 12).Insert(place, ".");

            foreach (var record in this.records)
            {
                if(record.id == id)
                {
                    if (record.references.Count < referenceList.Count)
                    {
                        this.statusList.Add(description, false, String.Format("Instance {0} missing one or more references", record.id));
                        return;
                    }
                    else
                    {
                        for (var i = 0; i < referenceList.Count; i++)
                        {
                            string reference = referenceList[i];
                            if (reference != record.references[i])
                            {
                                this.statusList.Add(description, false, String.Format("Expecting reference string on index {0} of instance {1} to be {2}, got {3} instead", i, record.id, reference, record.references[i]));
                                return;
                            }
                        }
                    }
                }
                
            }

            this.statusList.Add(description, true, "");
            return;

        }

    }

}
