using FLChat.Core.Routers;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core
{
    public interface IScenarioStarter
    {
        //string StartScenario(ChatEntities entities, string sct, Guid guid, int transport);
        string StartScenario(ChatEntities entities, string sct, Guid guid, int transport, out int? scenarioStepId);
    }

    public class ScenarioStarter : IScenarioStarter
    {
        public string StartScenario(ChatEntities entities, string sct, Guid guid, int transport, out int? scenarioStepId)
        {
            scenarioStepId = null;
            switch (sct)
            {
                case ScenarioType.Invite:
                    return new InviteLinkRouter().StartScenario(entities, sct, guid, transport, out scenarioStepId);

                case ScenarioType.Common:
                    return new CommonLinkRouter().StartScenario(entities, sct, guid, transport, out scenarioStepId);
            }
            return null;
        }

        //public string StartScenario(ChatEntities entities, string sct, Guid guid, int transport)
        //{
        //    return InviteLinkRouter.StartScenario(entities, sct, guid, transport, out int? st);            
        //}
    }
}
