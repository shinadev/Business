using Business.Models.Home;
using Business.Models.About;
using Business.Models.Vendor;
using Business.Models.Service;
using System.Collections.Generic;

namespace Business.ViewModels
{
    public class HomePageViewModel
    {
        public List<HomeSection> HomeSections { get; set; } = new List<HomeSection>();
        public AboutSection AboutSection { get; set; } = new AboutSection();
        public ServiceSection ServiceSection { get; set; } = new ServiceSection();
        public List<Vendors> Vendors { get; set; } = new List<Vendors>();
    }
}
