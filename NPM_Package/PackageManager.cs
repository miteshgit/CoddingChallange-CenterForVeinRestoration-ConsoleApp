using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NPM_Package
{
    public class PackageManager
    {
        private readonly HttpClient client;
        private List<string> allPackages;
        public PackageManager()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri("http://registry.npmjs.org")
            };
            allPackages = new List<string>();
        }

        public string[] GetAllDependencies(string packageName)
        {
            BuildNPMPackageList(packageName);
            return allPackages.Distinct().OrderBy(o => o).ToArray();
        }

        private void BuildNPMPackageList(string packageName)
        {
            var url = string.Format($"/{packageName}/latest");
            var response = client.GetAsync(url).Result;
            var stringResponse = response.Content.ReadAsStringAsync().Result;

            var packageData = JsonConvert.DeserializeObject<PackageData>(stringResponse);

            if (packageData.dependencies != null && packageData.dependencies.ToString() != "{}")
            {
                var dependencies = JsonConvert.DeserializeObject<Dictionary<string, string>>(packageData.dependencies.ToString());
                Parallel.ForEach(dependencies.Keys.ToList(), dependency => BuildNPMPackageList(dependency.ToString()));
            }
            else
            {
                allPackages.Add(packageName);
            }
        }
    }
}
