using Business.Models.Service;

namespace Business.ViewModels
{
    public class ServiceSectionViewModel
    {
        public ServiceSection ServiceSection { get; set; } = new ServiceSection();
        public CallToAction? CallToAction { get; set; }
    }
}
