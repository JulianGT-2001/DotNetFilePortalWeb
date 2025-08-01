namespace fileuploadweb.Models.ViewModel
{
    public class UserFilesViewModel
    {
        public Guid Id { get; set; } = default!;
        public string OriginalName { get; set; } = default!;
        public string Path { get; set; } = default!;
        public long SizeInBytes { get; set; }
        public string MimeType { get; set; } = default!;
        public DateTime UploadedAt { get; set; }
    }
}