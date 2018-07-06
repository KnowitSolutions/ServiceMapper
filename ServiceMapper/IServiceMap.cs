using StructureMap;
using System;
using System.Collections.Generic;

namespace ServiceMapper
{
	public interface IServiceMap
	{
		IDictionary<Type, object> Map();
		IServiceMap Override<T>(Action<Map> overrideFunction);
	}
}