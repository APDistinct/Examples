using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.Model;

namespace FLChat.Core.Routers
{
    public class InviteLinkRouter : BaseLinkRouter //IMessageRouter
    {
        //protected new const string ScenarioName = "Invite";
        //private new static int scNum = Scenario.Values.GetValue(ScenarioName, 2);   // Потом достать из таблицы по имени

        public InviteLinkRouter() : base()
        {
            ScenarioName = "Invite";
            scNum = Scenario.Values.GetValue(ScenarioName, 2);
        }

        //const int firstStepNum = 0;  // Номер стартового шага
        //const int firstViberStepNum = 1;  // Номер стартового шага
        //private static string step0mess = "Добрый день. Общайтесь с личным консультантом и получайте мгновенно ответы на все вопросы."
        //                + " Хотите продолжить? Напишите \"ДА\" в ответ или нажмите кнопку \"Продолжить\".";
        //private readonly string step1OKmess = "Ваш личный консультант #OwnerUser. Вы можете задать ему вопрос прямо сейчас. ";
        //private readonly string step1NOmess = "Чтобы продолжить, поделитесь номером вашего телефона.";


        //public Guid? RouteMessage(ChatEntities entities, IOuterMessage message, Message dbmsg)
        //{
        //    //int scNum = 2;  // Потом достать из таблицы по имени
        //    //  Проверка на наличие в списках работающих сценариев
        //    var scp = entities.ScenarioProcess.Where(x => x.ScenarioStep.ScenarioId == scNum && x.UserId == dbmsg.FromUserId)
        //        .Include(x => x.ScenarioStep)/*.Select(z => z.ScenarioStep.Step)*/.FirstOrDefault();
        //    if (scp != null)
        //    {
        //        //  Нахождение номера шага в сценарии и осуществление следющего
        //        int scStep = scp.ScenarioStep.Step;
        //        if (PerformCommand(entities, message, dbmsg, scp, scStep))
        //                return Global.SystemBotId;
        //        //else
        //        //    MakeMessage(entities, dbmsg, scStep.ToString());

        //        return Global.SystemBotId;
        //    }
        //    //else
        //    //{
        //    //    MakeMessage(entities, dbmsg, "-1  " + dbmsg.FromUserId.ToString() + "  " + scNum.ToString());
        //    //}
        //    //return Global.SystemBotId;
        //    return null;
        //}

        //protected bool PerformCommand(ChatEntities entities, IOuterMessage message, Message dbmsg, ScenarioProcess scp, int stepNumber)
        //{
        //    bool ret = true;
        //    string text = "No";
        //    int? stepId = scp.ScenarioStepId;
        //    switch (stepNumber)
        //    {
        //        case 0:
        //            text = step0mess;
        //            stepId = 1;
        //            FixNextStep(entities, scp, stepId.Value);
        //            break;
        //        case 1:                    
        //            text = step1OKmess;
        //            //  Временно отключаем шаг для тестового показа
        //            //FinishScenario(entities, scp, stepId.Value);
        //            //stepId = null;
        //            //  -------------

        //            //  Проверка - отдан ли телефон
        //            //  Временно для ВК, пока не делаем нового шага
        //            if (GetPhone(message, dbmsg, out string phone) || dbmsg.FromTransportKind == TransportKind.VK)
        //            {
        //                text = step1OKmess;
        //                //  Проверка на наличие
        //                AcceptPhoneNumber(entities, message, dbmsg);

        //                //  Вообще-то уже достаточно, надо завершать. Но пока для тестов.
        //                //step = 1;
        //                //FixNextStep(entities, scp, step.Value);

        //                FinishScenario(entities, scp, stepId.Value);
        //                stepId = null;
        //            }
        //            else
        //            {
        //                //  Надо будет проверять ВК и ему делать продолжение шагов
        //                text = step1NOmess;
        //                stepId = scp?.ScenarioStepId;
        //            }
        //            ////  Отправка сообщения
        //            ////text = "Телефон получен";

        //            MakeMessage(entities, dbmsg, text,stepId, true);
        //            //  Фиксация шага

        //            break;

        //        case 2:
        //            break;                    
        //    }
        //    //FixNextStep(entities, scp, step);
        //    return ret;
        //}

        //private bool GetPhone(IOuterMessage message, Message dbmsg, out string phone)
        //{
        //    bool ret = false;
        //    //phone = null;
        //    //if (dbmsg.FromTransportKind == TransportKind.Viber || dbmsg.FromTransportKind == TransportKind.Telegram)
        //    //{
        //    //    string text = dbmsg.Text;
        //    //    BotCommandsRouter.CommandsEnum? cmd = BotCommandsRouter.GetCommandType(text, out string arg);
        //    //    if (cmd == BotCommandsRouter.CommandsEnum.Phone)
        //    //    {
        //    //        phone = message.PhoneNumber;
        //    //        ret = true;
        //    //    }
        //    //    else
        //    //        ret = false;
        //    //}
        //    phone = message.PhoneNumber;
        //    ret = phone != null;
        //    return ret;
        //}

        ///// <summary>
        ///// Accepted information about user's phone
        ///// If user with that phone already exists, then Merge users.
        ///// If user with that phone not exists, then update phone field and redirect user to operator master
        ///// </summary>
        ///// <param name="entities"></param>
        ///// <param name="message"></param>
        /////         
        //private bool AcceptPhoneNumber(ChatEntities entities, IOuterMessage message, Message dbmsg)
        //{
        //    bool ret = true;
        //    User user = entities.User.Where(u => u.Enabled && u.Phone == message.PhoneNumber).Include(u => u.Transports).SingleOrDefault();
        //    User from = entities.User.Where(u => u.Id == dbmsg.FromUserId).Include(u => u.Transports).SingleOrDefault();
        //    if (from == null || from.IsBot /*|| from.IsTemporary == false*/)
        //        throw new Exception($"Can't merge user {dbmsg.FromUserId.ToString()}");

        //    if (user != null)
        //    {
        //        ret = false;
        //        // Замена ХП на простые действия - нового заблокировать, его транспорт отдать старому
        //        //MergeUsers(from, user, dbmsg.FromTransportTypeId);
        //        //from.Enabled = false;
        //        ////  Пришлось отказаться - проблема в последовательности сохранения данных в БД, возникали задвоения

        //        Guid?[] messages = entities.MergeUsers(user.Id, dbmsg.FromUserId).ToArray(); //.Where(g => g.HasValue).Select(g => g.Value).ToArray();

        //        dbmsg.FromUserId = user.Id;
        //        dbmsg.FromTransport =  //user.Transports.Get((TransportKind)dbmsg.FromTransportTypeId);
        //        entities.Transport
        //        .Where(t => t.Enabled && t.UserId == user.Id && t.TransportTypeId == dbmsg.FromTransportTypeId)
        //        .Single();

        //        if(user.OwnerUserId == null)
        //        {
        //            user.OwnerUserId = from.OwnerUserId;
        //        }
        //    }
        //    else
        //    {
        //        dbmsg.FromTransport.User.Phone = message.PhoneNumber;
        //        if(string.IsNullOrEmpty(dbmsg.FromTransport.User.FullName))
        //        {
        //            dbmsg.FromTransport.User.FullName = !string.IsNullOrEmpty(message.FromName) ? message.FromName : message.PhoneNumber;
        //        }
        //        //dbmsg.FromTransport.User.FullName = message.FromName;
        //    }
        //    return ret;
        //}

        //public void MergeUsers(User from, User to, int FromTransportTypeId)
        //{
        //    DAL.Model.Transport transp = from.Transports.Get((TransportKind)FromTransportTypeId);
        //    if (transp != null)
        //    {
        //        Transport userTransp = to.Transports.Get((TransportKind)FromTransportTypeId);
        //        if (userTransp == null)
        //        {
        //            userTransp = new Transport()
        //            { /*TransportOuterId = transp.TransportOuterId, Enabled = true, */Kind = (TransportKind)FromTransportTypeId };
        //            to.Transports.Add(userTransp);
        //        }
        //        userTransp.TransportOuterId = transp.TransportOuterId;
        //        userTransp.Enabled = true;
        //        transp.TransportOuterId = ""/*null*/;
        //        transp.Enabled = false;                
        //    }
        //}

        //private Message MakeMessage(ChatEntities entities, Message dbmsg, string text, int? step = null, bool change = false)
        //{
        //    Message reply = new Message()
        //    {
        //        Kind = MessageKind.Personal,
        //        FromTransport = entities.SystemBotTransport,
        //        AnswerTo = dbmsg,
        //        Text = text,
        //        ToUsers = new MessageToUser[] {
        //                new MessageToUser() {
        //                    ToTransport = dbmsg.FromTransport
        //                }
        //        },
        //        ScenarioStepId = step,
        //        NeedToChangeText = change,
        //    };
        //    entities.Message.Add(reply);
        //    return reply;
        //}

        //private void FixNextStep(ChatEntities entities, ScenarioProcess scp, int step)
        //{
        //    int? st = entities.ScenarioStep.Where(x => x.ScenarioId == scNum && x.Step == step).Select(z => z.Id).FirstOrDefault();
        //    if (st.HasValue)
        //        scp.ScenarioStepId = st.Value;
        //    else
        //        FinishScenario(entities, scp, step);
        //    //entities.SaveChanges();
        //}

        //private void FinishScenario(ChatEntities entities, ScenarioProcess scp, int step)
        //{
        //    entities.ScenarioProcess.Remove(scp);
        //    //entities.SaveChanges();
        //}

        //public static string StartScenario(ChatEntities entities, string sct, Guid guid, int transport)
        //{
        //    //string inviteString = step0mess;
        //    ////  Переделать поиск в расширение. Вопрос - если НЕ найдено - добавлять?
        //    //int step = transport == (int)TransportKind.Viber ? firstViberStepNum : firstStepNum;
        //    //int? st = entities.ScenarioStep.Where(x => x.ScenarioId == scNum && x.Step == step).Select(z => z.Id).FirstOrDefault();
        //    //ScenarioProcess sp = new ScenarioProcess()
        //    //{ /*ScenarioId = (int)sct, */ScenarioStepId = st.Value, UserId = guid, TransportTypeId = transport };
        //    //entities.ScenarioProcess.Add(sp);
        //    //entities.SaveChanges();
        //    return StartScenario(entities, sct, guid, transport, out int? st);
        //}

        //public static string StartScenario(ChatEntities entities, string sct, Guid guid, int transport, out int? stepId)
        //{
        //    string inviteString = step0mess;
        //    //  Переделать поиск в расширение. Вопрос - если НЕ найдено - добавлять?
        //    int step = transport == (int)TransportKind.Viber ? firstViberStepNum : firstStepNum;
        //    //int step = firstViberStepNum ;
        //    int? st = entities.ScenarioStep.Where(x => x.ScenarioId == scNum && x.Step == step).Select(z => z.Id).FirstOrDefault();
        //    ScenarioProcess sp = new ScenarioProcess()
        //    { /*ScenarioId = (int)sct, */ScenarioStepId = st.Value, UserId = guid, TransportTypeId = transport };
        //    entities.ScenarioProcess.Add(sp);
        //    entities.SaveChanges();
        //    stepId = st;
        //    return inviteString;
        //}
    }
}
