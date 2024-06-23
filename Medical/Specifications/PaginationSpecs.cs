namespace Medical.Specifications
{
    public class PaginationSpecs
    {

        private const int _maxSize = 100;
        private int _pageSize = _maxSize;
        private int _pageIndex = 1;

        public int PageIndex { get => _pageIndex; set => _pageIndex = value > 0 ? value : 1; }
        public int PageSize { get => _pageSize; set { _pageSize = value > _maxSize ? _maxSize : value; } }
    }
}
