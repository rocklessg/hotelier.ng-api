namespace hotel_booking_utilities
{
    /// <summary>
    /// A class responsible for paginating IEnumerable requests.
    /// </summary>
    public class Paginator
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }

        public Paginator()
        {
            PageSize = 10;
            CurrentPage = 1;
        }

        public Paginator(int pageSize, int currentPage)
        {
            PageSize = pageSize > 20 ? 20 : pageSize;
            CurrentPage = currentPage < 1 ? 1 : currentPage;
        }
        //To implement this pagenator, accept a pagenator input
        //params, instanciate the paginator class using the "new" keyword.
        //Apply the skip and take methods
        //e.g _userRepo.GetAllUsers().Skip((pagenator.CurrentPage - 1) * pagenator.PageSize).Take(pagenator.PageSize).ToArray()
        //If an empty constructor is initiated, it takes the default values set here for default constructor.
    }
}
