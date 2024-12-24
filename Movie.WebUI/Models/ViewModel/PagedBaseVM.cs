namespace Movie.WebUI.Models.ViewModel
{
    public class PagedBaseVM<T> where T : class, new()
    {
        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public PagedResultVM<T> Result { get; set; }

        public void PopulateFields()
        {
            if (Result.TotalCount <= Result.PageSize)
            {
                TotalPages = 1;
            }
            else
            {
                TotalPages = (Result.TotalCount / Result.PageSize) + 1;
            }
        }
    }
}
