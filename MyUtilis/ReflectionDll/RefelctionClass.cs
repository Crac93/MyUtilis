using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyUtilis.ReflectionDll
{
    class RefelctionClass
    {
        static string Dlls = "";

        public static int Initialize(StructReflection lib)
        {
            try
            {
                object result = null;

                if (lib.ref_assembly == null)
                    return reflectClass(lib.ref_class, lib.ref_method, lib.ref_attributes, out result);


                return reflectAssembly(lib.ref_assembly, lib.ref_class, lib.ref_method, lib.ref_attributes, out result);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        public static int Initialize(StructReflection lib, out object result)
        {
            try
            {
                result = null;

                if (lib.ref_assembly == null)
                    return reflectClass(lib.ref_class, lib.ref_method, lib.ref_attributes, out result);

                return reflectAssembly(lib.ref_assembly, lib.ref_class, lib.ref_method, lib.ref_attributes, out result);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        private static int reflectClass(string ref_class, string ref_method, string[] ref_attributes, out object result)
        {
            try
            {
                result = null;

                Type myclass = Type.GetType(ref_class);
                MethodInfo method = null;
                result = method.Invoke(null, ref_attributes);
                return 0;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        private static int reflectAssembly(string ref_assembly, string ref_class, string ref_method, string[] ref_attributes, out object result)
        {
            result = null;

            try
            {
                Dlls = "";

                InitializeReflection();

                ref_assembly = ref_assembly + ".dll";

                if (!Directory.Exists(ref_assembly))
                    throw new ArgumentException("Assembly doesn't exist");

                Assembly mylibrary = Assembly.LoadFile((string)ref_assembly);
                Type myclass = mylibrary.GetType((string)ref_class);
                object instanceclass = Activator.CreateInstance(myclass);
                MethodInfo method = instanceclass.GetType().GetMethod((string)ref_method);
                result = method.Invoke(instanceclass, ref_attributes);

                if (result == null)
                    return 0;

                if (result.GetType().Name.Contains("Exception"))
                    throw ((Exception)result);

                return 0;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        private static void InitializeReflection()
        {
            Environment.ExpandEnvironmentVariables(Dlls);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly assembly = (Assembly)null;
            string empty = string.Empty;
            string str = Dlls + "\\" + args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
            if (File.Exists(str))
            {
                assembly = Assembly.LoadFrom(str);
            }
            return assembly;
        }
    }
}
