using Autofac;
using University.Data.Contexts;
using University.Data.Entities;
using University.Data.Repositories;

namespace University.Api.Modules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UniversityDbContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<StudentRepository>().As<IStudentRepository>().InstancePerLifetimeScope();
        }
    }
}
