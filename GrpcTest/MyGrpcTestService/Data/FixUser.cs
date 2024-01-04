namespace MyGrpcTestService.Data
{
    public class FixUser : User
    {
        public bool IsNew { get; set; }
        public FixUser(User user) 
        {
            Change(user);
        }
        public void Change(User user)
        {
            //TODO: Сделать сравнение
            this.Id = user.Id;
            this.Name = user.Name;
            this.Money = user.Money;
            this.DateBirth = user.DateBirth;
            IsNew = true;
        }
        public void Fix()
        {
            IsNew = false;
        }
        public User GetUser() 
        {
            return this;
        }
    }
}
