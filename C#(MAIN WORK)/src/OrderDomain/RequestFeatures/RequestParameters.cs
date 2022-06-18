namespace OrderDomain.RequestFeatures
{
    public class RequestParameters
    {
        const int MaxPageSize = 50;
        private int _pageNumber = 1;
        private int _pageSize = 10;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = (value <= 0) ? _pageNumber : value;
        }
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string SearchTerm { get; set; }
        public string OrderBy { get; set; }

        public RequestParameters()
        {
            _pageNumber = 1;
            _pageSize = 10;
        }

    }
}