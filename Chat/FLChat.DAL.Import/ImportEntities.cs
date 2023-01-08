using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;
using System.Reflection;
using System.IO;
using System.Data.SqlClient;

namespace FLChat.DAL.Import
{
    [Obsolete]
    public static class ImportEntities
    {
        private static readonly string _sqlDisableUsers;

        public const int COMMIT_AFTER_EACH_ROWS = 50;

        static ImportEntities() {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("FLChat.DAL.Import.Sql.disable_users.sql")) {
                using (StreamReader reader = new StreamReader(stream)) {
                    _sqlDisableUsers = reader.ReadToEnd();
                }
            }
        }

        public static ImportResultOld FullImport(this ChatEntities entities, IImportSource<IUserInfo> source, IListener listener = null) {            
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> cities = entities.LoadCities();
            Dictionary<string, int> titles = entities.Rank.ToArray().ToDictionary(r => r.Name, r => r.Id);

            List<Tuple<int, int?>> links = new List<Tuple<int, int?>>();
            ImportResultOld result = new ImportResultOld();

            int index = 0;
            int savedCount = 0;
            bool updated;

            DbContextTransaction trans = null;
            try {
                trans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                foreach (IUserInfo su in source) {
                    result.TotalRows += 1;

                    int? cityId;
                    int? rankId;

                    try {
                        cityId = (su.Country != null && su.Region != null && su.City != null) ? entities.GetCityId(cities, su) : (int?)null;
                        rankId = su.Title != null ? entities.GetRank(titles, su.Title) : (int?)null;
                    } catch (System.Data.Entity.Infrastructure.DbUpdateException e) {
                        listener.Warning(index, $"Cons number {su.FLUserNumber.ToString()} is dublicate {e.Message}; {su.Country ?? "-"} {su.Region ?? "-"} {su.City ?? "-"}");
                        throw;
                    }

                    User user = entities.User.Where(u => u.FLUserNumber == su.FLUserNumber).SingleOrDefault();
                    if (user == null) {
                        updated = false;
                        user = new User() {
                            FLUserNumber = su.FLUserNumber,
                            IsBot = false,
                            IsTemporary = false,
                            IsConsultant = true
                        };
                        entities.User.Add(user);
                        result.NewbeCount += 1;
                    } else {
                        updated = true;
                        //if (user.OwnerUserId != null && user.OwnerUser.FLUserNumber != su.ParentFLUserNumber)
                        //    user.OwnerUserId = null;
                        //result.UpdatedCount += 1;
                    }
                    if (updated == false || user.Enabled != !(su.IsDeleted ?? false))
                        user.Enabled = !(su.IsDeleted ?? false);
                    if (updated == false || user.ParentFLUserNumber != su.ParentFLUserNumber)
                        user.ParentFLUserNumber = su.ParentFLUserNumber;
                    if (updated == false || user.FullName != su.GetFullName())
                        user.FullName = su.GetFullName();
                    if (updated == false || user.Birthday != su.Birthday)
                        user.Birthday = su.Birthday;
                    if (updated == false || user.Phone != su.Phone.RemoveNonDigits())
                        user.Phone = su.Phone.RemoveNonDigits();
                    if (updated == false || user.Email != su.Email)
                        user.Email = su.Email;
                    if (updated == false || user.RankId != rankId)
                        user.RankId = rankId;
                    if (updated == false || user.ZipCode != su.ZipCode)
                        user.ZipCode = su.ZipCode;
                    if (updated == false || user.CityId != cityId)
                        user.CityId = cityId;
                    if (updated == false || user.RegistrationDate != su.RegDate)
                        user.RegistrationDate = su.RegDate;
                    if (updated == false || user.EmailPermission != (su.EmailPermission ?? false))
                        user.EmailPermission = su.EmailPermission ?? false;
                    if (updated == false || user.SmsPermission != (su.SmsPermission ?? false))
                        user.SmsPermission = su.SmsPermission ?? false;
                    if (updated == false || user.IsDirector != (su.IsDirector ?? false))
                        user.IsDirector = su.IsDirector ?? false;
                    if (updated == false || user.LastOrderDate != su.LastOrder)
                        user.LastOrderDate = su.LastOrder;
                    if (updated == false || user.LoBonusScores != su.LoScores)
                        user.LoBonusScores = su.LoScores;
                    if (updated == false || user.PeriodsWolo != su.PeriodWoLo)
                        user.PeriodsWolo = su.PeriodWoLo;
                    if (updated == false || user.OlgBonusScores != su.OlgScores)
                        user.OlgBonusScores = su.OlgScores;
                    if (updated == false || user.GoBonusScores != su.GoScores)
                        user.GoBonusScores = su.GoScores;
                    if (updated == false || user.CashBackBalance != su.CashbackBalance)
                        user.CashBackBalance = su.CashbackBalance;
                    if (updated == false || user.FLClubPoints != su.FLClubPoints)
                        user.FLClubPoints = su.FLClubPoints;
                    if (updated == false || user.FLClubPointsBurn != su.FLClubPointsBurn)
                        user.FLClubPointsBurn = su.FLClubPointsBurn;
                    bool alteredEmail = false;
                    bool alteredPhone = false;
                    do {
                        try {
                            if (updated == false || entities.ChangeTracker.HasChanges()) {
                                entities.SaveChanges();
                                savedCount += 1;
                                if (updated)
                                    result.UpdatedCount += 1;
                            }
                            break;
                        } catch (System.Data.Entity.Infrastructure.DbUpdateException e) {
                            string fulltext = e.ToString();
                            if (fulltext.Contains("UNQ__UsrUser_Email")) {
                                if (alteredEmail)
                                    throw;
                                listener.Warning(index, $"Clear email {user.Email?.ToString()}(dublicate)");
                                user.Email = null;
                                alteredEmail = true;
                            } else if (fulltext.Contains("UNQ__UsrUser_Phone")) {
                                if (alteredPhone)
                                    throw;
                                listener.Warning(index, $"Clear phone {user.Phone?.ToString()}(dublicate)");
                                user.Phone = null;
                                alteredPhone = true;
                            } else {
                                listener.Warning(index, $"Cons number {user.FLUserNumber.ToString()} is dublicate {e.Message}");
                                throw;
                            }
                        }
                    } while (true);

                    //if ((su.ParentFLUserNumber.HasValue && user.OwnerUserId == null)
                    //    || (su.ParentFLUserNumber != user.ParentFLUserNumber))
                    //    links.Add(Tuple.Create(su.FLUserNumber, su.ParentFLUserNumber));
                    result.Users.Add(user.Id);

                    index += 1;
                    listener?.Progress(0, index, updated);

                    if (savedCount > 0 && savedCount >= COMMIT_AFTER_EACH_ROWS) {
                        savedCount = 0;
                        trans.Commit();
                        listener?.OnCommit(index);
                        trans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    }
                }

                //index = 0;
                //foreach (Tuple<int, int?> su in links) {
                //    User user = entities.User.Where(u => u.FLUserNumber == su.Item1).Single();
                //    User userOwner = null;
                //    if (su.Item2 != null)
                //        userOwner = entities.User.Where(u => u.FLUserNumber == su.Item2).SingleOrDefault();

                //    if (su.Item2 != null && userOwner == null && su.Item1 != source.TopUserNumber)
                //        listener.Warning(index, $"{su.Item1.ToString()} {user.FullName} - miss ParentFLUserNumber({su.Item2.ToString()})");
                //    updated = userOwner != null;

                //    user.OwnerUser = userOwner;
                //    entities.SaveChanges();
                //    index += 1;
                //    result.OwnerUpdated += 1;
                //    listener?.Progress(1, index, updated);

                //    if (index % COMMIT_AFTER_EACH_ROWS == 0) {
                //        trans.Commit();
                //        listener?.OnCommit(index);
                //        trans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                //    }

                //}

                trans.Commit();
                listener?.OnCommit(index);                
                trans.Dispose();
                trans = null;

                User head = entities
                    .User
                    .Where(u => u.FLUserNumber == source.TopUserNumber)                    
                    .Single();
                if (head.IsUseDeepChilds) {
                    entities.User_UpdateDeepChilds(head.Id);
                }
            } finally {
                if (trans != null) {
                    trans.Rollback();
                    trans.Dispose();
                }
            }

            return result;
        }

        public static int DisableUsers(this ChatEntities entities, int headFLUserNumber, List<Guid> users) {
            User head = entities.User.Where(u => u.FLUserNumber == headFLUserNumber).Single();
            return entities.Database.ExecuteSqlCommand(_sqlDisableUsers,
                new SqlParameter("@userId", head.Id),
                users.CreateGuidListParameter("@updated"));
        }

        public static int UpdateOwners(this ChatEntities entities) {
            return entities.Update_OwnerUserId_By_ParentFLUserNumber().FirstOrDefault().Value;
        }

        /// <summary>
        /// Load all cities, regions and contries
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, Dictionary<string, int>>> LoadCities(this ChatEntities entities) {
            using (var trans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted)) {
                City[] list = entities.City.Include(c => c.Region).Include(c => c.Region.Country).ToArray();
                Dictionary<string, Dictionary<string, Dictionary<string, int>>> result = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
                foreach (City city in list) {
                    Dictionary<string, Dictionary<string, int>> regDict;

                    if (!result.TryGetValue(city.Region.Country.Name.ToUpper(), out regDict)) {
                        regDict = new Dictionary<string, Dictionary<string, int>>();
                        result[city.Region.Country.Name.ToUpper()] = regDict;
                    }

                    Dictionary<string, int> cityDict;
                    if (!regDict.TryGetValue(city.Region.Name.ToUpper(), out cityDict)) {
                        cityDict = new Dictionary<string, int>();
                        regDict[city.Region.Name.ToUpper()] = cityDict;
                    }

                    cityDict[city.Name.ToUpper()] = city.Id;
                }

                //result[city.Region.Country.Name][city.Region.Name][city.Name] = city.Id;
                trans.Commit();
                return result;
            }
        }

        public static int GetCityId(
            this ChatEntities entities, 
            Dictionary<string, Dictionary<string, Dictionary<string, int>>> cities,
            IUserInfo su) {

            if (cities.TryGetValue(su.Country.ToUpper(), out var regDict)) {
                if (regDict.TryGetValue(su.Region.ToUpper(), out var cityDict)) {
                    if (cityDict.TryGetValue(su.City.ToUpper(), out int id))
                        return id;
                    else {
                        Country country = entities.Country.Where(c => c.Name == su.Country).Single();
                        Region reg = entities.Region.Where(r => r.Name == su.Region && r.CountryId == country.Id).Single();
                        City city = entities.City.Add(new City() {
                            Name = su.City,
                            RegionId = reg.Id,
                        });
                        entities.SaveChanges();
                        cityDict[su.City.ToUpper()] = city.Id;
                        return city.Id;
                    }
                } else {
                    Country country = entities.Country.Where(c => c.Name == su.Country).Single();
                    //Region reg =
                    Region reg = entities.Region.Add(new Region() {
                        Name = su.Region,
                        CountryId = country.Id
                    });
                    City city = entities.City.Add(new City() {
                        Name = su.City,
                        RegionId = reg.Id,
                    });
                    entities.SaveChanges();
                    regDict[su.Region.ToUpper()] = new Dictionary<string, int>() { [su.City.ToUpper()] = city.Id };
                    return city.Id;
                }
            } else {
                Country country = entities.Country.Add(new Country() {
                    Name = su.Country
                });
                Region reg = entities.Region.Add(new Region() {
                    Name = su.Region,
                    Country = country
                });
                City city = entities.City.Add(new City() {
                    Name = su.City,
                    Region = reg,
                });
                entities.SaveChanges();
                cities[su.Country.ToUpper()] = new Dictionary<string, Dictionary<string, int>> {
                    [su.Region.ToUpper()] = new Dictionary<string, int>() { { su.City.ToUpper(), city.Id } }
                };
                return city.Id;
            }
        }

        public static int GetRank(this ChatEntities entities, Dictionary<string, int> titles, string title) {
            if (titles.TryGetValue(title, out int id))
                return id;
            Rank rank = entities.Rank.Add(new Rank() {
                Name = title
            });
            entities.SaveChanges();
            titles[title] = rank.Id;
            return rank.Id;
        }

        public static string GetFullName(this IUserInfo su) {
            StringBuilder sb = new StringBuilder();
            if (su.Surname != null)
                sb.Append(su.Surname);
            if (su.Name != null) {
                if (sb.Length != 0)
                    sb.Append(' ');
                sb.Append(su.Name);
            }
            if (su.Partronymic != null) {
                if (sb.Length != 0)
                    sb.Append(' ');
                sb.Append(su.Partronymic);
            }
            return sb.ToString();
        }
    }
}
