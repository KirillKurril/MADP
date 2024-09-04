using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALWD.Domain.Entities
{
	public class Product
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
		public int? CategoryId { get; set; }
		public Category? Category { get; set; }
		public double Price { get; set; }
		public int Quantity { get; set; }
		public string? ImagePath { get; set; }
		public string? ImageMimeType {  get; set; }


		/*public List<Suppplier>? Suppliers { get; set; } = new List<Supplier>();*/
		/*public List<PickupPoint>? PickupPoints { get; set; } = new List<PickupPoint>()*/
		/*public int? ManufacturerId { get; set; }*/
		/*public Manufacturer? Manufacturer { get; set; }*/
	}
}
