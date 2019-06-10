using System;
using System.Diagnostics.CodeAnalysis;
using Ninject.Modules;

namespace GroceryStorePOS
{
    [ExcludeFromCodeCoverage]
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ConsoleManager>().ToSelf().InSingletonScope();
            this.Bind<DiscountFactory>().ToSelf().InSingletonScope();
            this.Bind<OrderManager>().ToSelf().InSingletonScope();
            this.Bind<ProductFactory>().ToSelf().InSingletonScope();
            this.Bind<ProductStorage>().ToSelf().InSingletonScope();
        }
    }
}
