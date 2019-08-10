using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Web.Services
{
    public class AssetVersionFinder
    {
        private readonly IHostingEnvironment _env;
        private IDictionary<string, string> _fileList = new Dictionary<string, string>();

        public AssetVersionFinder(IHostingEnvironment env)
        {
            _env = env;
        }

        public string GetVersionedAsset(string name)
        {
            if (_fileList.ContainsKey(name) && File.Exists(_fileList[name]))
            {
                var path = _fileList[name];
                if (File.Exists(path))
                {
                    return path;
                }
                _fileList.Remove(name);
            }

            var root = Path.GetFileNameWithoutExtension(name);
            var ext = Path.GetExtension(name);
            var files = Directory.GetFiles(Path.Combine(_env.WebRootPath, "assets"), $"*{ext}", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var filename = Path.GetFileNameWithoutExtension(file);
                if (filename.StartsWith(root))
                {
                    var path = Path.GetRelativePath(_env.WebRootPath, file);
                    path = path.Replace('\\', '/');
                    _fileList[name] = path;
                    return path;
                }
            }
            return "not-found";
        }

    }
}
