using System.Web.Http;
using CppDashboard.DataProvider;
using CppDashboard.Initialisers;
using CppDashboard.Models;
using WebGrease.Css.Extensions;

namespace CppDashboard.Controllers
{
    public class DashboardDataController : ApiController
    {
        private readonly PageModelBuilder _pageModelBuilder;
        private readonly IInitialiser _initialiser;
        private readonly ICanReload[] _reloads;

        public DashboardDataController(PageModelBuilder pageModelBuilder, IInitialiser initialiser, ICanReload[] reloads)
        {
            _initialiser = initialiser;
            _reloads = reloads;
            _initialiser.Load();
            _pageModelBuilder = pageModelBuilder;
            
        }

        [HttpGet]
        public PageModel Get()
        {
            var model = _pageModelBuilder.Build();
            
            return model;
        }

        [HttpGet]
        public void Update()
        {
            _reloads.ForEach(f => f.Reload());
        }
    }
}
