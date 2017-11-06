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
}
