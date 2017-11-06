using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeadlessClientTestResultDecipher;

namespace HeadlessClientTestResultDecipher
{
    public class TestDecipher3 : TestResultDecipher
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
}

