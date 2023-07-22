using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace StoreApp.Components
{
    public class OrderInProgressViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        public OrderInProgressViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        public string Invoke()//bir görünüm nesnesi geri döndüreceğiz.
        {
            return _manager.OrderService.NumberOfInProcess.ToString();
        }
    }
}