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
                    return new Test1();
                case "Test #2":
                    return new Test2();
                case "Test #3":
                    return new Test3();
                case "Test #4":
                    return new Test4();
                case "Test #5":
                    return new Test5();
                case "Test #6":
                    return new Test6();
                case "Test #7":
                    return new Test7();
                default:
                    return null;
            }

        }
    }

    public class TestActionRecord
    {
        public string type = "client";
        public int id = -1;
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
            HashSet<int> setOfIds = new HashSet<int>();

            foreach(var record in this.records) {
                if(record.id < 0)
                {
                    this.statusList.Add(String.Format("Assert {0} individual entities", count), false, "Invalid id of < 0");
                    return;
                }
                setOfIds.Add(record.id);
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
        public void assertActionCombination(List<Tuple<string, int>> actionList)
        {

            if(actionList.Count == 0)
            {
                return;
            }

            string description = "Expecting";
            foreach (var tuple in actionList)
            {
                string action = tuple.Item1;
                description = description + String.Format(" {0} occurances of action {1} and", tuple.Item2, action);
            }

            int place = description.LastIndexOf(" and");
            description = description.Remove(place, 4).Insert(place, ".");

            foreach (var tuple in actionList)
            {

                int count = 0;
                string action = tuple.Item1;
                int expectedCount = tuple.Item2;
                foreach (var record in this.records)
                {

                    if(record.action == action)
                    {
                        count++;
                    }

                }
                if(count != expectedCount)
                {
                    this.statusList.Add(description, false, String.Format("Expecting {0} x {1}, counted {2}", expectedCount, action, count));
                    return;
                }

            }

            this.statusList.Add(description, true, "");

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

    public class Test1: TestResultDecipher
    {

        public override void assesTestResult()
        {
            base.assesTestResult();
            this.assertNumberOfIndividualEntities(3);

            var combinedActions = new List<Tuple<string, int>>
            {
                new Tuple<string, int>( "CREATE", 1 ),
                new Tuple<string, int>( "SEARCH", 2 )
            };

            this.assertActionCombination(combinedActions);

            var orderOfAction = new List<string>
            {
                "CREATE",
                "JOIN",
                "JOIN"
            };

            this.assertOrderingOfAction(orderOfAction);

            var references = new List<string>
            {
                "foo"
            };

            this.assertReferenceMatching(references);

        }

    }

    public class Test2: TestResultDecipher
    {

        public override void assesTestResult()
        {
            base.assesTestResult();
            this.assertNumberOfIndividualEntities(3);

            var combinedActions = new List<Tuple<string, int>>
            {
                new Tuple<string, int>( "CREATE", 2 ),
                new Tuple<string, int>( "CREATEFAIL", 1 )
            };

            this.assertActionCombination(combinedActions);

            var orderOfAction = new List<string>
            {
                "CREATE",
                "CREATE",
                "CREATEFAIL"
            };

            this.assertOrderingOfAction(orderOfAction);

            var references = new List<string>
            {
                "foo"
            };

            this.assertReferenceMatching(references);

        }

    }

    public class Test3 : TestResultDecipher
    {

        public override void assesTestResult()
        {
            base.assesTestResult();
            this.assertNumberOfIndividualEntities(2);

            var combinedActions = new List<Tuple<string, int>>
            {
                new Tuple<string, int>( "CREATE", 1 ),
                new Tuple<string, int>( "SEARCH", 1 ),
                new Tuple<string, int>( "SEARCHFAIL", 1 )
            };

            this.assertActionCombination(combinedActions);

            var orderOfAction = new List<string>
            {
                "CREATE",
                "SEARCH",
                "SEARCHFAIL"
            };

            this.assertOrderingOfAction(orderOfAction);

            var references1 = new List<string>
            {
                "n",
                "pass"
            };

            this.assertReferenceMatching(1, references1);

            var references2 = new List<string>
            {
                "n"
            };

            this.assertReferenceMatching(2, references2);

        }

    }

    public class Test4 : TestResultDecipher
    {

        public override void assesTestResult()
        {
            base.assesTestResult();
            this.assertNumberOfIndividualEntities(5);

            var combinedActions = new List<Tuple<string, int>>
            {
                new Tuple<string, int>( "CREATE", 1 ),
                new Tuple<string, int>( "JOIN", 3 ),
                new Tuple<string, int>( "JOINFAIL", 1 )
            };

            this.assertActionCombination(combinedActions);

            var orderOfAction = new List<string>
            {
                "CREATE",
                "JOIN",
                "JOIN",
                "JOIN",
                "JOINFAIL"
            };

            this.assertOrderingOfAction(orderOfAction);

            var references1 = new List<string>
            {
                "foo",
                "3"
            };

            this.assertReferenceMatching(1, references1);

            var references2 = new List<string>
            {
                "foo"
            };

            this.assertReferenceMatching(2, references2);
            this.assertReferenceMatching(3, references2);
            this.assertReferenceMatching(4, references2);
            this.assertReferenceMatching(5, references2);

        }

    }

    public class Test5 : TestResultDecipher
    {

        public override void assesTestResult()
        {
            base.assesTestResult();
            this.assertNumberOfIndividualEntities(3);

            var combinedActions = new List<Tuple<string, int>>
            {
                new Tuple<string, int>( "CREATE", 2 ),
                new Tuple<string, int>( "JOIN", 2 ),
                new Tuple<string, int>( "KICK", 1 )
            };

            this.assertActionCombination(combinedActions);

            var orderOfAction = new List<string>
            {
                "CREATE",
                "CREATE",
                "JOIN",
                "JOIN",
                "KICK"
            };

            this.assertOrderingOfAction(orderOfAction);

        }

    }

    public class Test6 : TestResultDecipher
    {



    }

    public class Test7 : TestResultDecipher
    {



    }



}
