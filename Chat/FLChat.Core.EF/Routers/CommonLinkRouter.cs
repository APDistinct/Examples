using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core.Routers
{
    public class CommonLinkRouter : BaseLinkRouter
    {
        //protected new const string ScenarioName = "Common";
        //protected new static int scNum = Scenario.Values.GetValue(ScenarioName, 3);   // Потом достать из таблицы по имени

        public CommonLinkRouter() : base()
        {
             ScenarioName = "Common";
            scNum = Scenario.Values.GetValue(ScenarioName, 3);
            //step1OK_Yes_mess = "Оставайтесь на связи и получайте актуальную информацию об акциях и новинках. ";
    }

        //private static string step0mess = "Добрый день. Общайтесь с личным консультантом и получайте мгновенно ответы на все вопросы."
        //                + " Хотите продолжить? Напишите \"ДА\" в ответ или нажмите кнопку \"Продолжить\".";
        //private readonly string step1OKmess = "Ваш личный консультант #OwnerUser. Вы можете задать ему вопрос прямо сейчас. ";
        //private readonly string step1NOmess = "Чтобы продолжить, поделитесь номером вашего телефона.";

    }
}
