namespace ALWD.UI.ViewModels
{
    public class PaginationViewModel
    {
        public PaginationViewModel(int totalPages, int currentPage, string currentCategoryNormilizedName)
        {
            TotalPages = totalPages;
            CurrentPage = currentPage;
            CurrentCategoryNormilizedName = currentCategoryNormilizedName;
        }

        public int TotalPages;
        public int CurrentPage;
        public string CurrentCategoryNormilizedName;
    }
}
