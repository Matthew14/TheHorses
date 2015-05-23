using System.Windows.Forms;
using log4net;
using log4net.Config;

namespace TheHorses.Scraper
{
    public partial class HiddenForm : Form
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof (HiddenForm));

        public HiddenForm()
        {
            InitializeComponent();

            XmlConfigurator.Configure();
            
            DoStuff();

        }

        async void DoStuff()
        {
            var s = new AtTheRacesScraper();


            var r = await s.ScrapeResults();
        }
    }
}
