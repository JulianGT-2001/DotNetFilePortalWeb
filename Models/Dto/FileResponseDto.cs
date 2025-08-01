namespace fileuploadweb.Models.Dto
{
    public class FileResponseDto
    {
        public Guid id { get; set; } = default!;
        public string originalName { get; set; } = default!;
        public string path { get; set; } = default!;
        public long sizeInBytes { get; set; }
        public string mimeType { get; set; } = default!;
        public DateTime uploadedAt { get; set; }
    }
}