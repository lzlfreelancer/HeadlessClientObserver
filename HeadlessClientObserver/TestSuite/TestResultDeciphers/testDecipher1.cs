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
            this.assertNumberOfIndividualEntities(3);

            var combinedActions = new List<Tuple<string, int>>
            {
                new Tuple<string, int>( "CreatingClient", 5 ),
                new Tuple<string, int>( "Attemping to Create Party", 1 ),
                new Tuple<string, int>( "CreateParty", 1 ),
                new Tuple<string, int>( "JoinParty", 3 ),
                new Tuple<string, int>( "JoinFailed", 1 )
                
            };

            this.assertActionCombination(combinedActions);

            var references = new List<string>
            {
                "Party1"
            };

            this.assertReferenceMatching(references);

        }

    }
}
