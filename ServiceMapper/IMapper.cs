using System;

namespace ServiceMapper
{
	public interface IMapper
	{
		object Map(Map type);
	}
}