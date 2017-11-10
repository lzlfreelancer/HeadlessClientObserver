using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeadlessClientTestResultDecipher;

namespace HeadlessClientTestResultDecipher
{
    public class TestDecipher5 : TestResultDecipher
    {

        public override void assesTestResult()
        {
            base.assesTestResult();
            this.assertNumberOfIndividualEntities(3);

            var combinedActions = new List<Tuple<List<string>, int>>
            {
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "Attemping to Create Party",
                        "Party3"
                    }
                    , 1 ),
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "CreatePartySuccess",
                        "Party3"
                    }
                    , 1 ),
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "JoinParty",
                        "Party3"
                    }
                    , 2 )
            };

            this.assertActionArgumentsCombination(combinedActions);

        }

    }
}
