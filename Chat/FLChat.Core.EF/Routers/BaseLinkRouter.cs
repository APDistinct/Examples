using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.Core.Media;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.Logger;
using FLChat.SmsBott;

namespace FLChat.Core.Routers
{
    public abstract class BaseLinkRouter : IMessageRouter
    {
        protected readonly PhoneParser _phoneparser = new PhoneParser();
        protected string ScenarioName;
        protected int scNum;   // Номер сценария

        const int firstStepNum = 0;  // Номер стартового шага
        const int firstViberStepNum = 1;  // Номер стартового шага
        const int firstTgStepNum = 1;  // Номер стартового шага
        const int firstVKStepNum = 0; //2;  // Номер стартового шага
        protected static string step0mess = "Добрый день. Общайтесь с личным консультантом и получайте мгновенно ответы на все вопросы."
                        + " Хотите продолжить? Напишите \"ДА\" в ответ или нажмите кнопку \"Продолжить\".";
        protected string step1OK_Yes_mess = "Ваш личный консультант #OwnerUser. Вы можете задать ему вопрос прямо сейчас. ";
        protected string step1OK_No_mess = "Оставайтесь на связи и получайте актуальную информацию об акциях и новинках. ";
        protected readonly string step1NOmess = "Чтобы продолжить, поделитесь номером вашего телефона.";
        //protected readonly string step2mess = "Введите номер своего телефона в формате код страны номер телефона, например 79012345678";
        protected readonly string step2mess = "Представьтесь, пожалуйста! Введите номер вашего мобильного телефона.";
        protected readonly string step3mess = "Подтвердите правильность номера телефона. Вы ввели ";
        protected readonly string step4mess = "Введите проверочный код, который мы вам отправили по SMS на номер ";
        protected FileLogger _logger;
        protected ISmsBot _smsBot;

        public BaseLinkRouter(ISmsBot smsBot = null)
        {
            _smsBot = smsBot ?? new SmsBot();
            ScenarioName = "Base";
            scNum = Scenario.Values.GetValue(ScenarioName, 0);
            string fname = "C:\\FLChat\\Log.txt";
            _logger = new FileLogger(fname);
        }

        public Guid? RouteMessage(ChatEntities entities, IOuterMessage message, Message dbmsg)
        {
            _logger.Log($"Start RouteMessage scenario {scNum}");
            _logger.LogObject("message", message);
            _logger.LogObject("dbmsg", dbmsg);
            //  Проверка на наличие в списках работающих сценариев
            var scp = entities.ScenarioProcess.Where(x => x.ScenarioStep.ScenarioId == scNum && x.UserId == dbmsg.FromUserId)
                .Include(x => x.ScenarioStep)/*.Select(z => z.ScenarioStep.Step)*/.FirstOrDefault();
            _logger.LogObject("scp", scp);
            if (scp != null)
            {
                //  Нахождение номера шага в сценарии и осуществление следующего
                int scStep = scp.ScenarioStep.Step;
                _logger.Log($"scStep {scStep} ");
                if (PerformCommand(entities, message, dbmsg, scp, scStep))
                    return Global.SystemBotId;
                //else
                //    MakeMessage(entities, dbmsg, scStep.ToString());

                return Global.SystemBotId;
            }
            //else
            //{
            //    MakeMessage(entities, dbmsg, "-1  " + dbmsg.FromUserId.ToString() + "  " + scNum.ToString());
            //}
            //return Global.SystemBotId;
            return null;
        }

        protected bool PerformCommand(ChatEntities entities, IOuterMessage message, Message dbmsg, ScenarioProcess scp, int stepNumber)
        {
            bool ret = true;
            string text = "No";
            int? stepId = scp.ScenarioStepId;
            _logger.LogObject("stepId", stepId);
            switch (stepNumber)
            {
                case 0:
                    text = step0mess;
                    int stepp = 1;
                    if (dbmsg.FromTransportKind == TransportKind.VK)
                    {
                        stepp = 2;
                        text = step2mess;
                    }
                    stepId = GetStepId(entities, stepp);
                    FixNextStep(entities, scp, stepId.Value);
                    break;
                case 1:
                    text = step1OK_Yes_mess;
                    //  Временно отключаем шаг для тестового показа
                    //FinishScenario(entities, scp, stepId.Value);
                    //stepId = null;
                    //  -------------

                    //  Проверка - отдан ли телефон
                    bool getPhone = GetPhone(message, dbmsg, out string phone);
                    _logger.LogObject("getPhone ", getPhone);
                    //  Временно для ВК, пока не делаем нового шага
                    if (getPhone /*|| dbmsg.FromTransportKind == TransportKind.VK*/)
                    {                        
                        //  Проверка на наличие
                        var mret =  AcceptPhoneNumber(entities, message, dbmsg, out bool changed);
                        text = mret ? step1OK_Yes_mess : step1OK_No_mess;

                        FinishScenario(entities, scp);
                        stepId = null;
                    }
                    else
                    {
                        //  Надо будет проверять ВК и ему делать продолжение шагов
                        text = step1NOmess;
                        stepId = scp?.ScenarioStepId;
                    }
                    break;

                case 2:                   
                    string phonenum = message.Text;
                    if(!string.IsNullOrWhiteSpace(phonenum))
                    {
                        // Проверка номера на валидность
                        // Если ДА - идём далее, если НЕТ - возврат на шаг ввода
                        // По идее, надо принять этот номер, зафиксировать, получить кому он приписался. Потом отправлять СМС
                        if (IsPhoneValid(phonenum, out string result))
                        {
                            // result - надо зафиксировать как номер телефона в БД для данного обращающегося
                            int stp = 4;
                            SavePhone(entities, dbmsg.FromUserId, stp, result);

                            SendSms(entities, dbmsg.FromUserId, stp, result);
                            text = step4mess + result;
                            stepId = GetStepId(entities, stp);
                            FixNextStep(entities, scp, stepId.Value);
                        }
                        else
                        {
                            text = "Введите полный номер вашего мобильного телефона, включая код страны";
                        }
                    }
                    else
                    {
                        text = "Введите полный номер вашего мобильного телефона, включая код страны";
                    }
                    break;
                case 3:
                    //  Проверка подтверждения номера телефона
                    // Если ДА - идём далее, если НЕТ - возврат на шаг ввода
                    // В настоящий шаг момент исключён из сценария
                    if(PhoneConfirmed(dbmsg))
                    {
                        //SendSms();
                        text = step4mess;
                        stepId = GetStepId(entities, 4); 
                        FixNextStep(entities, scp, stepId.Value);
                    }
                    else
                    {
                        text = "Повторите ввод номера";
                        stepId = GetStepId(entities, 2); 
                        FixNextStep(entities, scp, stepId.Value);
                    }
                    break;
                case 4:
                    string code = message.Text;
                    if (!string.IsNullOrWhiteSpace(code))
                    {
                        // Проверка кода на соответствие
                        // Если ДА - фиксация всего, мы получили и подтвердили; если НЕТ - остаёмся здесь
                        int stp = 4;
                        if (CodeConfirmed(entities, dbmsg.FromUserId, stp, code))
                        {                            
                            string result = GetPhone(entities, dbmsg.FromUserId, stp);
                            var mret = AcceptPhoneNumber(entities, message, dbmsg, out bool changed, result);
                            text = mret ? step1OK_Yes_mess : step1OK_No_mess;
                            DelData(entities, dbmsg.FromUserId, stp);
                            FinishScenario(entities, scp);
                            stepId = null;
                        }
                        else
                        {
                            text = "Введен неверный код. Проверьте, пожалуйста, входящие сообщения и отправьте код, который мы вам отправили по SMS.";
                        }
                    }
                    else
                    {
                        text = "Введен неверный код. Проверьте, пожалуйста, входящие сообщения и отправьте код, который мы вам отправили по SMS.";
                    }
                    break;
                case 5:
                    //var mret = AcceptPhoneNumber(entities, message, dbmsg);
                    //text = mret ? step1OK_Yes_mess : step1OK_No_mess;

                    //FinishScenario(entities, scp, stepId.Value);
                    //stepId = null;
                    break;
            }
            MakeMessage(entities, dbmsg, text, stepId, true);
            //FixNextStep(entities, scp, step);
            return ret;
        }

        protected void SavePhone(ChatEntities entities, Guid userId, int stp, string data)
        {
            string mainKey = MakeKey(stp,"phone");
            entities.SetUserData(userId, mainKey, data);
        }

        protected string GetPhone(ChatEntities entities, Guid userId, int stp)
        {
            string mainKey = MakeKey(stp,"phone");
            string ret = null;
            ret = entities.GetUserData(userId, mainKey);
            return ret;
        }

        protected void SaveCode(ChatEntities entities, Guid userId, int stp, string data)
        {
            string mainKey = MakeKey(stp,"code");
            entities.SetUserData(userId, mainKey, data);
        }

        protected string GetCode(ChatEntities entities, Guid userId, int stp)
        {
            string mainKey = MakeKey(stp,"code");
            string ret = null;
            ret = entities.GetUserData(userId, mainKey);
            return ret;
        }

        protected void DelData(ChatEntities entities, Guid userId, int stp)
        {
            string mainKey = MakeKey(stp,"phone");
            entities.DelUserData(userId, mainKey);
            mainKey = MakeKey(stp, "code");
            entities.DelUserData(userId, mainKey);
        }

        private string MakeKey(int step, string kind)
        {
            return $"Scenario-{scNum}-{step}-{kind}";
        }

        private bool CodeConfirmed(ChatEntities entities, Guid userId, int stp, string code)
        {
            string savedcode = GetCode(entities, userId, stp);
            bool ret = code == savedcode;
            return ret;
        }

        private bool PhoneConfirmed(Message dbmsg)
        {
            bool ret = false;
            string text = dbmsg.Text;
            //BotCommandsRouter.CommandsEnum? cmd = BotCommandsRouter.GetCommandType(text, out string arg);
            //ret = cmd == BotCommandsRouter.CommandsEnum.Ok;
            ret = text == ScenarioButtons.ButtonOKCaption;
            return ret;
        }

        private bool IsPhoneValid(string phonenum, out string result)
        {
            bool ret = _phoneparser.TryParse(phonenum, out result);
            return ret;
        }
        private static Random _rnd = new Random();
        private static readonly object _rndLock = new object();
        private readonly int _codeDigitsCount = 4;

        private void SendSms(ChatEntities entities, Guid userId, int stp, string phone)
        {
            int smsCode;
            int _maxValue = (int)Math.Pow(10, _codeDigitsCount);
            lock (_rndLock)
            {
                smsCode = _rnd.Next(_maxValue);
            }
            string textCode = smsCode.ToString(new string('0', _codeDigitsCount));
            SaveCode(entities, userId, stp, textCode);
            _smsBot.SendSmsMessage(phone, String.Concat("Your code: ", textCode));
        }

        private bool GetPhone(IOuterMessage message, Message dbmsg, out string phone)
        {
            bool ret = false;
            //phone = null;
            //if (dbmsg.FromTransportKind == TransportKind.Viber || dbmsg.FromTransportKind == TransportKind.Telegram)
            //{
            //    string text = dbmsg.Text;
            //    BotCommandsRouter.CommandsEnum? cmd = BotCommandsRouter.GetCommandType(text, out string arg);
            //    if (cmd == BotCommandsRouter.CommandsEnum.Phone)
            //    {
            //        phone = message.PhoneNumber;
            //        ret = true;
            //    }
            //    else
            //        ret = false;
            //}
            phone = message.PhoneNumber;
            ret = phone != null;
            return ret;
        }

        /// <summary>
        /// Accepted information about user's phone
        /// If user with that phone already exists, then Merge users.
        /// If user with that phone not exists, then update phone field and redirect user to operator master
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="message"></param>
        ///         
        private bool AcceptPhoneNumber(ChatEntities entities, IOuterMessage message, Message dbmsg, out bool changed, string phn = null)
        {
            //bool ret = true;
            changed = false;
            User user = null;
            User from = null;
            string phNumber = phn ?? message.PhoneNumber;
            if (phNumber != null)
            {
                _logger.Log(phNumber);
                //var users = entities.User.Where(u => u.Enabled && u.Phone == phNumber).Select(x => x.Id).ToList();
                //_logger.LogObject("users", users);
                user = entities.User.Where(u => u.Enabled && u.Phone == phNumber).Include(u => u.Transports).SingleOrDefault();
                _logger.LogObject("user", user);
                //var froms = entities.User.Where(u => u.Id == dbmsg.FromUserId).Select(x => x.Id).ToList();
                //_logger.LogObject("froms", froms);
                from = entities.User.Where(u => u.Id == dbmsg.FromUserId).Include(u => u.Transports).SingleOrDefault();
                _logger.LogObject("from", from);
                if (from == null || from.IsBot /*|| from.IsTemporary == false*/)
                    throw new Exception($"Can't merge user {dbmsg.FromUserId.ToString()}");
            }

            if (user != null)
            {
                changed = true;
                //ret = false;
                // Замена ХП на простые действия - нового заблокировать, его транспорт отдать старому
                //MergeUsers(from, user, dbmsg.FromTransportTypeId);
                //from.Enabled = false;
                ////  Пришлось отказаться - проблема в последовательности сохранения данных в БД, возникали задвоения

                Guid?[] messages = entities.MergeUsers(user.Id, dbmsg.FromUserId).ToArray(); //.Where(g => g.HasValue).Select(g => g.Value).ToArray();

                dbmsg.FromUserId = user.Id;
                dbmsg.FromTransport =  //user.Transports.Get((TransportKind)dbmsg.FromTransportTypeId);
                entities.Transport
                .Where(t => t.Enabled && t.UserId == user.Id && t.TransportTypeId == dbmsg.FromTransportTypeId)
                .Single();
                
                //if (user.OwnerUserId == null)
                //{
                //    user.OwnerUserId = from.OwnerUserId;
                //}
            }
            else
            {
                dbmsg.FromTransport.User.Phone = phNumber;
                if (string.IsNullOrEmpty(dbmsg.FromTransport.User.FullName))
                {
                    dbmsg.FromTransport.User.FullName = !string.IsNullOrEmpty(message.FromName) ? message.FromName : phNumber;
                }
                //dbmsg.FromTransport.User.FullName = message.FromName;
            }
            _logger.LogObject("dbmsg", dbmsg);
            return user?.OwnerUserId != null/*ret*/;
        }

        public void MergeUsers(User from, User to, int FromTransportTypeId)
        {
            DAL.Model.Transport transp = from.Transports.Get((TransportKind)FromTransportTypeId);
            if (transp != null)
            {
                Transport userTransp = to.Transports.Get((TransportKind)FromTransportTypeId);
                if (userTransp == null)
                {
                    userTransp = new Transport()
                    { /*TransportOuterId = transp.TransportOuterId, Enabled = true, */Kind = (TransportKind)FromTransportTypeId };
                    to.Transports.Add(userTransp);
                }
                userTransp.TransportOuterId = transp.TransportOuterId;
                userTransp.Enabled = true;
                transp.TransportOuterId = ""/*null*/;
                transp.Enabled = false;
            }
        }

        private Message MakeMessage(ChatEntities entities, Message dbmsg, string text, int? step = null, bool change = false)
        {
            Message reply = new Message()
            {
                Kind = MessageKind.Personal,
                FromTransport = entities.SystemBotTransport,
                AnswerTo = dbmsg,
                Text = text,
                ToUsers = new MessageToUser[] {
                        new MessageToUser() {
                            ToTransport = dbmsg.FromTransport
                        }
                },
                ScenarioStepId = step,
                NeedToChangeText = change,
            };
            entities.Message.Add(reply);
            return reply;
        }

        private int? GetStepId(ChatEntities entities, int step)
        {
            return entities.ScenarioStep.Where(x => x.ScenarioId == scNum && x.Step == step).Select(z => z.Id).FirstOrDefault();
        }

        private void FixNextStep(ChatEntities entities, ScenarioProcess scp, int? st)
        {
            //int? st = entities.ScenarioStep.Where(x => x.ScenarioId == scNum && x.Step == step).Select(z => z.Id).FirstOrDefault();
            if (st.HasValue)
                scp.ScenarioStepId = st.Value;
            else
                FinishScenario(entities, scp);
            //entities.SaveChanges();
        }

        private void FinishScenario(ChatEntities entities, ScenarioProcess scp)
        {
            entities.ScenarioProcess.Remove(scp);
            //entities.SaveChanges();
        }

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

        public string StartScenario(ChatEntities entities, string sct, Guid guid, int transport, out int? stepId)
        {
            stepId = null;
            if (entities.ScenarioProcess.Where(z => z.TransportTypeId == transport && z.UserId == guid).Any())
                return null;
            string inviteString = step0mess;
            //  Переделать поиск в расширение. Вопрос - если НЕ найдено - добавлять?
            int step = firstStepNum;
            switch (transport)
            {
                case (int)TransportKind.Viber:
                    step = firstViberStepNum;
                    break;
                case (int)TransportKind.Telegram:
                    step = firstTgStepNum;
                    inviteString = null; // step1NOmess;
                    break;
                case (int)TransportKind.VK:
                    step = firstVKStepNum;
                    inviteString = null; //step2mess;
                    break;
                default:
                    step = firstStepNum;
                    break;
            }

            //int step = transport == (int)TransportKind.Viber ? firstViberStepNum : firstStepNum;
            
            //int step = firstViberStepNum ;
            int? st = entities.ScenarioStep.Where(x => x.ScenarioId == scNum && x.Step == step).Select(z => z.Id).FirstOrDefault();
            ScenarioProcess sp = new ScenarioProcess()
            { /*ScenarioId = (int)sct, */ScenarioStepId = st.Value, UserId = guid, TransportTypeId = transport };
            entities.ScenarioProcess.Add(sp);
            entities.SaveChanges();
            stepId = st;
            return inviteString;
        }
    }
}
