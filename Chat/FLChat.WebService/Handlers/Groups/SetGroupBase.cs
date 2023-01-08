using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace FLChat.WebService
{
    public class SetGroupBase
    {
        public bool IsReusable => true;
        protected static Dictionary<string, string> dicNames;
        public SetGroupBase()
        {
            List<string> vsIn = new List<string>();  //{ "FullName", "Phone", "Email" };
            vsIn.Add(nameof(GroupInfoShort.Name));
            vsIn.Add(nameof(GroupInfoShort.IsEqual));            

            dicNames = typeof(GroupInfoShort).GetJsonPropertyName(vsIn);
        }

        public GroupInfo ProcessRequest(ChatEntities entities, GroupInfoSet groupInfoSet, Group group)
            //(ChatEntities entities, Guid id, JObject json, bool getAll = false)
        {
            SaveChanges(entities, groupInfoSet, group);
            return new GroupInfo(group);
            //throw new ErrorResponseException(
            //    (int)HttpStatusCode.NotFound,
            //    new ErrorResponse(ErrorResponse.Kind.user_not_found, $"User with id {id} not found"));
        }

        private void SaveChanges(ChatEntities entities, GroupInfoSet groupInfoSet, Group group)
        {
            //  Разделение на общую информацию и списки
            //  Для списков стандартные алгоритмф, для остальной информации - поиск и обработка, если найдено
            //GroupInfoSet groupInfoSet = JsonConvert.DeserializeObject<GroupInfoSet>(json);
            
            var propertys = groupInfoSet._commonInfo;
            foreach (var property in propertys)
            {
                string propName = property.Key;
                try
                {
                    if (dicNames.ContainsKey(propName))
                    {
                        var fieldname = dicNames[propName];
                        switch (fieldname)
                        {
                            case nameof(GroupInfo.Name) :
                                group.Name = (string)property.Value; 
                                break;
                            case nameof(GroupInfo.IsEqual) :
                                group.IsEqual = (bool)property.Value; 
                                break;
                            
                                //case "":
                                //    break;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new ErrorResponseException(
                        (int)HttpStatusCode.BadRequest,
                        new ErrorResponse(ErrorResponse.Kind.input_data_error, $"Input data field {propName} are not correct", e.ToString()));
                }
            }
            // Предварительно - проверка признака группы и выполнение-невыполнение всего, связанного с админством.
            
            group.AddMembers(groupInfoSet.Members);
            group.AddAdmins(groupInfoSet.Admins);
            group.DelMembers(groupInfoSet.RemoveMmembers);
            group.DelAdmins(groupInfoSet.RemoveAdmins);
            entities.SaveChanges();
        }

        //private void AddMembers(ChatEntities entities, IEnumerable<Guid> list, Group group)
        //{
        //    AddAdmins(entities, list, group, false);
        //    //var guid = group.Id;
        //    //foreach(var mem in list)
        //    //{
        //    //    if(group.Members.Where(x => x.UserId == mem) == null)
        //    //    {
        //    //        group.Members.Add(new GroupMember(){ GroupId = guid, UserId = mem});
        //    //    }
        //    //}
        //    //entities.SaveChanges();
        //}

        //private void AddAdmins(ChatEntities entities, IEnumerable<Guid> list, Group group, bool isAdmin = true)
        //{
        //    var guid = group.Id;
        //    if (list != null)
        //    {
        //        foreach (var mem in list)
        //        {
        //            var g_mem = group.Members.Where(x => x.UserId == mem).FirstOrDefault();
        //            if (g_mem == null)
        //            {
        //                g_mem = new GroupMember() { GroupId = guid, UserId = mem };
        //                group.Members.Add(g_mem);
        //            }
        //            g_mem.IsAdmin = isAdmin;
        //        }
        //        entities.SaveChanges();
        //    }
        //}

        //private void DelMembers(ChatEntities entities, IEnumerable<Guid> list, Group group)
        //{
        //    var guid = group.Id;
        //    if (list != null)
        //    {
        //        foreach (var mem in list)
        //        {
        //            var grm = group.Members.Where(x => x.UserId == mem && x.GroupId == guid).FirstOrDefault();
        //            if(grm != null)
        //               group.Members.Remove(grm);
        //            //entities.GroupMember.Where(x => x.UserId == mem && x.GroupId == guid).Delete();
        //        }
        //        entities.SaveChanges();
        //    }
        //}
        //private void DelAdmins(ChatEntities entities, IEnumerable<Guid> list, Group group)
        //{
        //    var guid = group.Id;
        //    if (list != null)
        //    {
        //        foreach (var mem in list)
        //        {
        //            var g_mem = group.Members.Where(x => x.UserId == mem).FirstOrDefault();
        //            if (g_mem != null)
        //            {
        //                g_mem.IsAdmin = false;
        //            }
        //        }
        //        entities.SaveChanges();
        //    }
        //}
    }
}


