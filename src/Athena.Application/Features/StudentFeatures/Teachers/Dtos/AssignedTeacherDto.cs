namespace Athena.Application.Features.StudentFeatures.Teachers.Dtos
{
    public class AssignedTeacherDto : IDto 
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }   
        public string? Image { get; set; }  
        public string? Coures { get; set; } 
        public string? Group { get; set; }
        public string? HeadQuarter { get; set; }
        public int MonghlyFee { get; set; }
    }
}