namespace CleanBrilliantCompany.Models
{
    public class RouteSegment
    {
        public int SegmentId { get; set; }
        public TransportMode Mode { get; set; }
        public float Distance { get; set; }  // in kilometers

        public RouteSegment() { }

        public RouteSegment(int segmentId, TransportMode mode, float distance)
        {
            SegmentId = segmentId;
            Mode = mode;
            Distance = distance;
        }

        public override string ToString()
        {
            return $"Segment {SegmentId}: {Mode} - {Distance} km";
        }
    }
}
