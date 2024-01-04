namespace MyGrpcTestService.Data
{
    public interface IDataProcessing
    {
        List<User> GetNewUsers();
        List<User> GetAllUsers();
        void SetNewUsers(List<User> users);
        void FixUsers(List<User> users);
    }
    public class DataProcessing : IDataProcessing
    {
        private static List<FixUser> _users = new ();
        public List<User> Users { get {  return GetAllUsers(); } }
        public List<User> GetNewUsers()
        {
            List<User> list = _users.Where(u => u.IsNew).Select(x => x.GetUser()).ToList ();
                //new List<User>();
            return list;
        }
        public List<User> GetAllUsers()
        {
            List<User> list = _users.Select(x => x.GetUser()).ToList();
            //new List<User>();
            return list;
        }
        public void SetNewUsers(List<User> users)
        {
            users.ForEach(x =>
            { 
                var u = _users.FirstOrDefault(u => u.Id == x.Id);
                if(u != null) 
                {
                    u.Change(x);
                }
                else
                {
                    _users.Add(new FixUser(x));
                }
            }
            ) ;
        }
        public void FixUsers(List<User> users)
        {
            users.ForEach(x =>
            {
                var u = _users.FirstOrDefault(u => u.Id == x.Id);
                if (u != null)
                {
                    u.Fix();
                }
            }
            );
        }
    }
}
