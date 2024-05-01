namespace Demo.DTO
{
    public class NewswithauthornameDTo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public string AuthorName { get; set; }
        public string ImageUrl { get; set; }
        public DateOnly PublicationDate { get; set; }
        public DateOnly CreationDate { get; set; }

    }
}
