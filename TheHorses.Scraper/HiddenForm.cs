using System;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using TheHorses.Database;

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
            IResultsScraper s = new AtTheRacesScraper();

            var r = await s.ScrapeResults(DateTime.Today.Subtract(new TimeSpan(2,0,0,0)));
            IDatabase db = new SQLServerDatabase(DatabaseCredentials.LoadFromFile(ScraperSettings.Default.dbCredFile));
            var dao = new Dao(db);

            dao.AddResults(r);
            MessageBox.Show(@"Done");
        }
    }
}
