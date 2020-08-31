namespace VesselInventory.Dto
{
    public class RequestSummaryDto
    {
        public int TotalDraftRequest { get; set; }
        public int TotalReleaseRequest { get; set; }
        public int TotalPendingRequest { get; set; }
        public int TotalRequestEngine { get; set; }
        public int TotalRequestDeck { get; set; }
        public int TotalRequestElectrical { get; set; }
        public int TotalItemDocumentPending { get; set; }
    }
}
