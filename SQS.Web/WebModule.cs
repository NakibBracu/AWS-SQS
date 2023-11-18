using Autofac;
using Autofac.Core;
using SQS.Web.Models;
using SQS.Web.QueueService;

namespace SQS.Web
{
    public class WebModule : Autofac.Module
    {
        public WebModule()
        { }

        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<AddMessageModel>().AsSelf()
            //.InstancePerLifetimeScope();

          

            base.Load(builder);
        }
    }
}
