using System.ComponentModel.DataAnnotations;

namespace ALWD.Domain.Entities
{
	public class DbEntity
	{
		[Key]
		public int Id { get; set; }
	}
}
