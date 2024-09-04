namespace ALWD.Domain.Models
{
	public class ListModel<T>
	{
		public ListModel(List<T> items) => Items = items;
		public List<T> Items { get; set; } = new();
		public int CurrentPage { get; set; } = 1;
		public int TotalPages { get; set; } = 1;
	}
}
