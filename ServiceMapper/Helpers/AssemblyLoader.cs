using System;
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
					}catch(Exception e)
					{
						//Eat exception as not everything can be loaded always
					}
				}
			}
		}

		public static IEnumerable<Type> GetAllInterfaces()
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
