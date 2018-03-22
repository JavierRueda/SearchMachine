namespace SearchMachineR.Models
{
    public class SearchEngineValueReturned
    {
        public string SearchEngineName { get; set; }
        public SearchEngineOrigin SearchEngineOriginType { get; set; }
        public long Counter { get; set; }
        public string TextQuery { get; set; }
    }
}
