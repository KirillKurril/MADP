namespace ALWD.Domain.Models
{
	public class ListModel<T>
	{
		public ListModel() { }
        public ListModel(IReadOnlyList<T> items) => Items = items;
		public IReadOnlyList<T> Items { get; set; }
		public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
	}
}
