using System;
using System.Linq;
using System.ServiceModel.Activation;

namespace ServiceMapper.Mappers
{
	public class ServiceRouteMapper : IMapper
	{
		private readonly INameMapper _nameMapper;
		private readonly ServiceHostFactory _serviceHostFactory;

		public ServiceRouteMapper(INameMapper nameMapper, ServiceHostFactory serviceHostFactory)
		{
			_nameMapper = nameMapper;
			_serviceHostFactory = serviceHostFactory;
		}
		public object Map(Map type)
		{
			if (type.HostingType != null)
				return new ServiceRoute(_nameMapper.Map(type), _serviceHostFactory, type.HostingType);
			var types = AssemblyLoader.GetAllTypes().Where(x => x.GetInterfaces().Contains(type.Type));
			if (types.Count() > 1)
				throw new Exception($"Error: multiple matching types for interface \"{type.Type.Namespace}.{type.Type.Name}\". Candidates are: {string.Join(", ", types.Select(x => x.Namespace + "." + x.Name))}. Please override hostingtype with one of these.");
			if (!types.Any())
				throw new Exception($"Error: No matching types found for interface: \"{type.Type.Namespace}.{type.Type.Name}\". Override with a hostingtype or Ignore()");
			return new ServiceRoute(_nameMapper.Map(type), _serviceHostFactory, types.First());
		}
	}
}
