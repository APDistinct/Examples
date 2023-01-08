using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Model
{
    public partial class Group
    {
        /// <summary>
        /// List of available transport's kind
        /// </summary>
        public IEnumerable<Guid> MemberList => GetMembers();
        public IEnumerable<Guid> AdminList => GetAdmins();

        private IEnumerable<Guid> GetMembers()
        {
            return Members
                //.Where(t => t.GroupId == Id)
                .Select(t => t.UserId)
                .ToList();
        }

        private IEnumerable<Guid> GetAdmins()
        {
            return Members
                .Where(t => /*t.GroupId == Id && */t.IsAdmin)
                .Select(t => t.UserId)
                .ToList();
        }
    
    
        public void AddMembers(IEnumerable<Guid> list)
        {
            AddAdmins(list, false);
        }

        public void AddAdmins(IEnumerable<Guid> list, bool isAdmin = true)
        {
            //var guid = Id;
            if (list != null)
            {
                foreach (var mem in list)
                {
                    var g_mem = Members.Where(x => x.UserId == mem).FirstOrDefault();
                    if (g_mem == null)
                    {
                        g_mem = new GroupMember() { /*GroupId = guid, */UserId = mem };
                        Members.Add(g_mem);
                    }
                    g_mem.IsAdmin = isAdmin;
                }
            }
        }

        public void DelMembers(IEnumerable<Guid> list)
        {
            //var guid = Id;
            if (list != null)
            {
                foreach (var mem in list)
                {
                    var grm = Members.Where(x => x.UserId == mem /*&& x.GroupId == guid*/).FirstOrDefault();
                    if (grm != null)
                        Members.Remove(grm);
                }
            }
        }
        public void DelAdmins(IEnumerable<Guid> list)
        {
            //var guid = Id;
            if (list != null)
            {
                foreach (var mem in list)
                {
                    var g_mem = Members.Where(x => x.UserId == mem).FirstOrDefault();
                    if (g_mem != null)
                    {
                        g_mem.IsAdmin = false;
                    }
                }
            }
        }
    }
}
