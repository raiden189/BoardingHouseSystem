using Android.Provider;
using BoardingHouseSystem.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BoardingHouseSystem.Data
{
    public class Database
    {
        private readonly SQLiteAsyncConnection database;
        public Database(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Login>();
            database.CreateTableAsync<Account>();
            database.CreateTableAsync<Booking>();
            database.CreateTableAsync<Boarding>();
            database.CreateTableAsync<ImagePath>();
        }

        public Task<Login> Authenticate(Login login)
        {
            return database.Table<Login>().Where(w => (w.Username.Equals(login.Username)) && (w.Password.Equals(login.Password))).FirstOrDefaultAsync();
        }

        public Task<Login> CheckUserName(string userName)
        {
            return database.Table<Login>().Where(w => w.Username.Equals(userName)).FirstOrDefaultAsync();
        }

        public Task<Account> GetAccountAsync(string Id)
        {
            return database.Table<Account>().Where(w => w.Id.Equals(Id)).FirstOrDefaultAsync();
        }

        public Task<List<ImagePath>> GetImagesAsync(string id)
        {
            return database.Table<ImagePath>().Where(w => w.ImageId == id).ToListAsync();
        }

        public Task<int> SaveAccountAsync(Account student) 
        {
            return database.InsertAsync(student);
        }

        public Task<int> SaveLoginAsync(Login login)
        {
            return database.InsertAsync(login);
        }

        public async Task<int> SaveBoardingAsync(Boarding boarding, List<ImagePath> images)
        {
            int imageSaved = await SaveImagesAsync(images);
            if (imageSaved > 0)
            {
                int boardingSaved = await database.InsertAsync(boarding);
                if (boardingSaved > 0)
                    return boardingSaved;
            }
            return 0;
        }

        public Task<int> SaveImagesAsync(List<ImagePath> images)
        {
            return database.InsertAllAsync(images);
        }

        public Task<int> SaveBookingAsync(Booking booking)
        {
            return database.InsertAsync(booking);
        }

        public Task<int> UpdateBookingAsync(Booking booking)
        {
            return database.UpdateAsync(booking);
        }

        public Task<List<Booking>> SearchBookingByStatusStudent(string studentId, string status = null)
        {
            if (status != null)
                return database.Table<Booking>()
                             .Where(w => w.StudentId == studentId)
                             .Where(w => w.Status == status).ToListAsync();
            else
                return database.Table<Booking>()
                             .Where(w => w.StudentId == studentId).ToListAsync();
        }

        public Task<List<Booking>> SearchBookingByStatusOwner(string boardingId, string status = null)
        {
            if (status != null)
                return database.Table<Booking>()
                         .Where(w => w.BoardingId == boardingId)
                         .Where(w => w.Status == status).ToListAsync();
            else
                return database.Table<Booking>()
                             .Where(w => w.BoardingId == boardingId)
                             .ToListAsync();
        }

        public Task<List<Booking>> SearchBookingByStatusOwnerStudent(string studentId, string boardingId, string status = null)
        {
            if (status != null)
                return database.Table<Booking>()
                             .Where(w => w.StudentId == studentId)
                             .Where(w => w.BoardingId == boardingId)
                             .Where(w => w.Status == status).ToListAsync();
            else
                return database.Table<Booking>()
                             .Where(w => w.StudentId == studentId)
                             .Where(w => w.BoardingId == boardingId)
                             .ToListAsync();
        }

        public Task<Booking> SearchBooking(string id)
        {
            return database.Table<Booking>()
                         .Where(w => w.StudentId == id).FirstOrDefaultAsync();
        }

        public Task<Booking> SearchBooking(string bookingId, string studentId)
        {
            return database.Table<Booking>()
                        .Where(w => w.BoardingId == bookingId)
                        .Where(w => w.StudentId == studentId).FirstOrDefaultAsync();
        }

        public Task<List<Booking>> GetBookings()
        {
            return database.Table<Booking>().ToListAsync();
        }

        public Task<List<Booking>> GetBookingsByStatus(string id, string status = null)
        {
            return database.Table<Booking>().Where(w => w.BoardingId == id).ToListAsync();
        }

        public Task<Boarding> GetBoardingHouseAsync(string id)
        {
            return database.Table<Boarding>().Where(w => w.id == id).FirstOrDefaultAsync();
        }

        public Task<List<ImagePath>> GetImages(string id)
        {
            return database.Table<ImagePath>().Where(w => w.ImageId == id).ToListAsync();
        }

        public Dictionary<Boarding, List<ImagePath>> GetBoardingHouseWithImagesAsync()
        {
            Dictionary<Boarding, List<ImagePath>> keyValuePairs = new Dictionary<Boarding, List<ImagePath>>();
            var boardingHouses = database.Table<Boarding>().ToListAsync();
            foreach (Boarding board in boardingHouses.Result)
            {
                var imagePaths = database.Table<ImagePath>().Where(w => w.ImageId == board.id).ToListAsync();
                keyValuePairs.Add(board, imagePaths.Result);
            }
            return keyValuePairs;
        }

        public void DropTable()
        {
            TableMapping tableMapping = new TableMapping(typeof(Booking), CreateFlags.None);
            database.DropTableAsync(tableMapping);
        }
    }
}
