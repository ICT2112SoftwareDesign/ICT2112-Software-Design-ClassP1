namespace CleanBrilliantCompany.DTO
{
    public class EmissionPredDTO{
        public DateOnly day {get; set;}
        public float emisission {get; set;}
        public EmissionPredDTO() { }
        public EmissionPredDTO(DateOnly day, float emisission){
            this.day = day;
            this.emisission = emisission;
        }
    }
}