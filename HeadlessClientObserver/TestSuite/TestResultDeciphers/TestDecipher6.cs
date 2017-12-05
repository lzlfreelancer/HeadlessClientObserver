using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeadlessClientTestResultDecipher;

namespace HeadlessClientTestResultDecipher
{
    public class TestDecipher6 : TestResultDecipher
    {

        public override void assesTestResult()
        {
            base.assesTestResult();
            this.assertNumberOfIndividualEntities(4);

            var combinedActions = new List<Tuple<List<string>, int>>
            {
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "Attemping to Create Party",
                        "Party 6"
                    }
                    , 1 ),
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "CreatePartySuccess",
                        "Party 6"
                    }
                    , 1 ),
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "JoinParty",
                        "Party 6"
                    }
                    , 2 ),
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "Attemping to Create Party",
                        "Party 6.1"
                    }
                    , 1 ),
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "CreatePartySuccess",
                        "Party 6.1"
                    }
                    , 1 ),
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "JoinParty",
                        "Party 6.1"
                    }
                    , 1 )
            };

            this.assertActionArgumentsCombination(combinedActions);

        }

    }
}