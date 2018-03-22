using System.Collections.Generic;

namespace SearchMachineR.Models
{
    public class SearchEngineResult
    {
        public List<SearchEngineValueReturned> SearchEngineValues { get; set; }

        public long Aggregate
        {
            get
            {
                long aggregate = 0;
                foreach (var searchEngineValue in SearchEngineValues)
                {
                    aggregate = aggregate + searchEngineValue.Counter;
                }
                return aggregate;
            }

        }
    }
}
