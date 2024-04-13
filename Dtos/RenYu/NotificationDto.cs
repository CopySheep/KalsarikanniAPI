namespace HotelFuen31.APIs.Dtos.RenYu
{
    public class NotificationDto
    {
        public int Id { get; set; }
        
        public required string Name { get; set; }

        public required string Description { get; set; }

        public DateTime PushTime { get; set; }

        public string? Image {  get; set; }

        public int? LevelId { get; set; }

        public string? LevelName { get; set; }

        public int TypeId { get; set;}
        
        public string TypeName { get; set; }
    }
}
