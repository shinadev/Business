using Business.Models.Service;
using System.Collections.Generic;

namespace Business.ViewModels
{
    public class ServicePageViewModel
    {
        public ServiceHeader? Header { get; set; }
        public ServiceSection? Section { get; set; }
        public List<ServiceItem>? Services { get; set; }
        public CallToAction? CallToAction { get; set; }

        // Convenience properties for easier access in the view
        public string SmallTitle => Section?.SmallTitle ?? "Our Services";
        public string MainTitle => Section?.MainTitle ?? "Custom IT Solutions for Your Successful Business";
        public string ImageUrl => Header?.ImageUrl ?? "/img/default-service.jpg";
    }
}
