using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Tp2.Controllers
{
    public class LinkCollectionWrapper
    {
		public List<ExpandoObject> Value { get; set; } = new List<ExpandoObject>();

		public LinkCollectionWrapper()
		{

		}
		public List<Link> Links { get; set; } = new List<Link>();
		public LinkCollectionWrapper(List<ExpandoObject> value)
		{
			Value = value;
		}
	}
}
