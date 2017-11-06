using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeadlessClientTestResultDecipher;

namespace HeadlessClientTestResultDecipher
{
    public class TestDecipher4 : TestResultDecipher
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
}
