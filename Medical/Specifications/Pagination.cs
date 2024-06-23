namespace Medical.Specifications
{
    public class Pagination<T>
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public int Count { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)Count / Size);
        public IEnumerable<T> Items { get; set; }
    }
}
