using System;

namespace NPM_Package
{
    class Program
    {
        static void Main(string[] args)
        {
             if (args == null)
            {
                Console.WriteLine("args is null");
            }
            else
            {
                var packageManager = new PackageManager();
                string argument = args[0];
                var dependencies = packageManager.GetAllDependencies(argument);
                foreach(var x in dependencies)
                {
                    Console.WriteLine(x);
                }
            }
            Console.ReadLine();
        }
    }
}
