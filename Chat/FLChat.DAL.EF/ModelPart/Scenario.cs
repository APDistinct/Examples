using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Model
{
    public partial class Scenario
    {
        private static Lazy<ScenarioDict> _settings = new Lazy<ScenarioDict>(ScenarioLoader, true);

        public static ScenarioDict Values => _settings.Value;

        private static ScenarioDict ScenarioLoader()
        {
            using (ChatEntities entities = new ChatEntities())
            {
                return new ScenarioDict(entities.Scenario.ToDictionary(s => s.Name, s => s.Id));
            }
        }
    }
}
