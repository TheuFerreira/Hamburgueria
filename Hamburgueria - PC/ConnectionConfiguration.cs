using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.IO;

namespace Hamburgueria
{
    public class MyContextConfiguration : DbConfiguration
    {
        public MyContextConfiguration()
        {
            MyDbModelStore cachedDbModelStore = new MyDbModelStore(Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Necos\\@Hamburgueria"));
            IDbDependencyResolver dependencyResolver = new SingletonDependencyResolver<DbModelStore>(cachedDbModelStore);
            AddDependencyResolver(dependencyResolver);
        }

        private class MyDbModelStore : DefaultDbModelStore
        {
            public MyDbModelStore(string location)
                : base(location)
            { }

            public override DbCompiledModel TryLoad(Type contextType)
            {
                string path = GetFilePath(contextType);
                if (File.Exists(path))
                {
                    DateTime lastWriteTime = File.GetLastWriteTimeUtc(path);
                    DateTime lastWriteTimeDomainAssembly = File.GetLastWriteTimeUtc(Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Necos\\@Hamburgueria"));
                    if (lastWriteTimeDomainAssembly > lastWriteTime)
                    {
                        File.Delete(path);
                    }
                }
                else
                {
                }

                return base.TryLoad(contextType);
            }
        }
    }
}
