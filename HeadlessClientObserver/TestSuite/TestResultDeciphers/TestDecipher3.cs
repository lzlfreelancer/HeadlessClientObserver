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
            this.assertNumberOfIndividualEntities(4);

            var combinedActions = new List<Tuple<List<string>, int>>
            {
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "Attemping to Create Party",
                        "x",
                        "y"
                    }
                    , 1 ),
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "CreatePartySuccess",
                        "x",
                        "y"
                    }
                    , 1 ),
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "Attemping to Search Party",
                        "x",
                        "y"
                    }
                    , 1 ),
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "Attemping to Search Party",
                        "x",
                        "yxx"
                    }
                    , 1 ),
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "Attemping to Search Party",
                        "asdf"
                    }
                    , 1 ),
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "FoundParty",
                        "x"
                    }
                    , 1 ),
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "PartyNotFound",
                        "x"
                    }
                    , 1 ),
                new Tuple<List<string>, int>(
                    new List<string>()
                    {
                        "PartyNotFound",
                        "asdf"
                    }
                    , 1 )
            };

            this.assertActionArgumentsCombination(combinedActions);

        }

    }
}

