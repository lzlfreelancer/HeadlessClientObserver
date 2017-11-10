using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeadlessClientTestResultDecipher;

namespace HeadlessClientTestResultDecipher
{
    public class TestDecipher1 : TestResultDecipher
    {

        public override void assesTestResult()
        {
            base.assesTestResult();
            this.assertNumberOfIndividualEntities(5);

            var combinedActions = new List<Tuple<List<string>, int>>
            {
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "CreatingClient"
                    }
                    , 5 ),
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "Attemping to Create Party",
                        "Party1"
                    }
                    , 1 ),
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "CreateParty",
                        "Party1"
                    }
                    , 1 ),
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "JoinParty",
                        "Party1"
                    }
                    , 3 ),
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "JoinFailed"
                    }
                    , 1 )
            };

            this.assertActionArgumentsCombination(combinedActions);

        }

    }
}
