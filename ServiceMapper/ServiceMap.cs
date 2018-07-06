﻿using System;
using System.Collections.Generic;

namespace ServiceMapper
{
	public class ServiceMap : IServiceMap
	{
		private readonly IList<Type> _interfaces;
		private readonly IDictionary<Type, Map> _maps = new Dictionary<Type, Map>();
		private readonly IMapper _mapper;

		public ServiceMap(IList<Type> interfaces, IMapper mapper)
		{
			_interfaces = interfaces;
			_mapper = mapper;
			foreach (Type t in interfaces)
				_maps.Add(t, new Map(t));
		}

		public IServiceMap Override<T>(Action<Map> overrideFunction)
		{
			overrideFunction(_maps[typeof(T)]);
			return this;
		}

		public IDictionary<Type, object> Map()
		{
			IDictionary<Type, object> mapped = new Dictionary<Type, object>();
			foreach (KeyValuePair<Type, Map> entity in _maps)
			{
				if(!entity.Value.Ignored)
					mapped[entity.Key] = _mapper.Map(entity.Value);
			}
			return mapped;
		}
	}
}
