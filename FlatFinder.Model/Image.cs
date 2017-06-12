namespace FlatFinder.Model
{
    public class Image : EntityBase
    {
        public string BytesAsBase64 { get; set; }
        public string MimeType { get; set; }

        public int FlatId { get; set; }
        public virtual Flat Flat { get; set; }
    }
}