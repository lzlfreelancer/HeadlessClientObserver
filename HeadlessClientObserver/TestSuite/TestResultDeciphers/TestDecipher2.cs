using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeadlessClientTestResultDecipher;

namespace HeadlessClientTestResultDecipher
{
    public class TestDecipher2 : TestResultDecipher
    {

        public override void assesTestResult()
        {
            base.assesTestResult();
            this.assertNumberOfIndividualEntities(2);

            var combinedActions = new List<Tuple<string, int>>
            {
                new Tuple<string, int>( "Attemping to Create Party", 2 ),
                new Tuple<string, int>( "CreateParty", 2 ),
                new Tuple<string, int>( "CreatePartyFailed", 1 )
            };

            this.assertActionCombination(combinedActions);

        }

    }
}
