namespace Movie.WebUI.Models.ViewModel
{
    public class PagedResultVM<T> where T : class, new()
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IList<T> Items { get; set; }
    }
}
