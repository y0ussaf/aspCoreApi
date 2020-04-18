using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tp2.Controllers.QueryParameters
{
	public abstract class QueryStringParams
	{
		const int maxPageSize = 50;
		public int PageNumber { get; set; } = 1;

		private int _pageSize = 10;
		public int PageSize
		{
			get
			{
				return _pageSize;
			}
			set
			{
				_pageSize = (value > maxPageSize) ? maxPageSize : value;
			}
		}
		public string OrderBy { get; set; }
		public string fields { get; set; }
	}
}
