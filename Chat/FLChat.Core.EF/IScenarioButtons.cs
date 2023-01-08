using FLChat.Core.Buttons;
using FLChat.Core.Routers;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core
{    
    public interface IScenarioButtons
    {
        IEnumerable<IEnumerable<ITransportButton>> GetButtons(string scenarioName, int stepNumber);
    }

    public class TransportButtonsSourceScenarioAdapter : ITransportButtonsSource
    {
        private readonly IScenarioButtons _buttons;
        private readonly string _name;
        private readonly int _number;

        public TransportButtonsSourceScenarioAdapter(IScenarioButtons buttons, string name, int number)
        {
            _buttons = buttons;
            _name = name;
            _number = number;
        }

        public IEnumerable<IEnumerable<ITransportButton>> GetButtons(MessageToUser mtu)
            => _buttons.GetButtons(_name, _number);
    }

    public class TransportButtonsSourceDBAdapter : ITransportButtonsSource
    {
        private readonly IScenarioButtons _buttons;

        public TransportButtonsSourceDBAdapter(IScenarioButtons buttons)
        {
            _buttons = buttons;
        }

        public IEnumerable<IEnumerable<ITransportButton>> GetButtons(MessageToUser mtu)
            => _buttons.GetButtons(mtu.Message.ScenarioStep.Scenario.Name, mtu.Message.ScenarioStep.Step);
    }

    public static class IScenarioButtonsExtentions
    {
        public static ITransportButtonsSource Adapt(this IScenarioButtons scenario, string name, int step)
            => new TransportButtonsSourceScenarioAdapter(scenario, name, step);

        public static ITransportButtonsSource Adapt(this IScenarioButtons scenario)
            => new TransportButtonsSourceDBAdapter(scenario);
    }
}
