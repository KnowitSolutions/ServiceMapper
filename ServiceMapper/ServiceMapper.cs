using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ServiceMapper
{
	public class ServiceMapper 
	{
		public static IServiceMap Create(Func<Type, bool> matcher, IMapper mapper)
		{
			AssemblyLoader.LoadAllAssemblies();
			IEnumerable<Type> interfaces = GetAllInterfaces();
			IEnumerable<Type> matchingInterfaces = interfaces.Where(t => matcher(t));
			return new ServiceMap(matchingInterfaces.ToList(), mapper);
		}

		private static IEnumerable<Type> GetAllInterfaces()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			IEnumerable<Type> types = new List<Type>();
			foreach (Assembly asm in assemblies)
			{
				try
				{
					types = types.Union(asm.GetTypes());
				}
				catch (Exception e)
				{
					Console.WriteLine($"Eating exception {e.Message}");
				}
			}
			return types.Where(x => x.IsInterface).Distinct();
		}
	}
}
