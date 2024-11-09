using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALWD.Tests
{
	public class APIProductControllerTests
	{
		[Fact]
		public void GetProductsListAsync_DefaultRequest_ReturnCorrectProductsCountAndTotalPagesCount()
		{

		}

		[Fact]
		public void GetProductsListAsync_CorrectRequestedPageNumber_ReturnCorrectProductList()
		{

		}

		[Fact]
		public void GetProductsListAsync_CorrectRequestedCategory_ReturnCorrectProductList()
		{

		}

		[Fact]
		public void GetProductsListAsync_IncorrectRequestedPageNumber_ReturnsDefaultNoncategoryProductList()
		{

		}

		[Fact]
		public void GetProductListAsync_ExceedingRequestPageNumber_ReturnResponseSuccessfullStatusFalse()
		{

		}
	}
}
