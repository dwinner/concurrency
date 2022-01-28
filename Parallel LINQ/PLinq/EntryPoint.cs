/**
 * Встроенный язык параллельных запросов
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace _13_PLinq
{
   class EntryPoint
   {
      static void Main()
      {
         foreach (var obsoleteMethod in ObsoleteMethods(Assembly.GetAssembly(typeof(Thread))))
         {
            Console.WriteLine(obsoleteMethod);
         }

         Console.ReadKey();
      }

      private static IEnumerable<string> ObsoleteMethods(Assembly assembly)
      {
         return from type in assembly.GetExportedTypes().AsParallel()
                from method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                let obsoleteAttrType = typeof(ObsoleteAttribute)
                where Attribute.IsDefined(method, obsoleteAttrType)
                orderby type.FullName
                let obsoleteAttrObject = (ObsoleteAttribute)Attribute.GetCustomAttribute(method, obsoleteAttrType)
                select string.Format("Type={0}\nMethod={1}\nMessage={2}\n",
                   type.FullName,
                   method,
                   obsoleteAttrObject.Message);
      }
   }
}
