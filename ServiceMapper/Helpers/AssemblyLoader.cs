using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ServiceMapper
{
	public class AssemblyLoader
	{
		public static void LoadAllAssemblies()
		{
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				LoadReferencedAssembly(assembly);
			}
		}

		public static void LoadReferencedAssembly(Assembly assembly)
		{
			foreach (AssemblyName name in assembly.GetReferencedAssemblies())
			{
				if (!AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName == name.FullName))
				{
					try
					{
						LoadReferencedAssembly(Assembly.Load(name));
					}
					catch (Exception e)
					{
						//Eat exception as not everything can be loaded always
					}
				}
			}
		}

		public static IEnumerable<Type> GetAllInterfaces(params Assembly[] assemblies)
		{
			if (assemblies.Any())
				return ExtractTypesFromAssemblies(assemblies);
			//Only use / store cached data when extracting all assemblies
			return GetAllTypes(assemblies).Where(x => x.IsInterface);
		}

		private static ConcurrentBag<Type> _types = new ConcurrentBag<Type>();
		public static IEnumerable<Type> GetAllTypes(params Assembly[] assemblies)
		{
			if (_types.Any())
				return _types;

			lock (_types)
			{
				//Check if someone else got the lock before us and performed the loading work
				if (_types.Any())
					return _types;

				assemblies = assemblies.Any() ? assemblies : AppDomain.CurrentDomain.GetAssemblies();
				IEnumerable<Type> types = ExtractTypesFromAssemblies(assemblies);
				_types = new ConcurrentBag<Type>(types.Distinct());
			}
			return _types;
		}

		private static IEnumerable<Type> ExtractTypesFromAssemblies(Assembly[] assemblies)
		{
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

			return types;
		}
	}
}
