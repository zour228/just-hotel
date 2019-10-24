using System.Collections.Generic;
using App.Domain.UserEntity;

namespace App.Domain.RoomEntity
{
    public class Room : AbstractEntity
    {
        public RoomType RoomType { get; private set; } = RoomType.Single;
        public ISet<User> Employees { get; private set; } = new HashSet<User>();
        public int Cost { get; private set; }
        public User? RentedBy { get; private set; }
        public RentalDates RentalDates { get; private set; } = new RentalDates();
        public bool IsRented => null != RentedBy;

        protected Room()
        {
        }

        public Room(RoomType type, int cost)
        {
            Identify();

            RoomType = type;
            Cost = cost;

            RentalDates = new RentalDates();
        }

        public void Rent(User rentedBy, RentalDates dates)
        {
            RoomAssertion.AssertCanRentRoom(this, rentedBy);
            RoomAssertion.AssertRentalDatesValid(dates.DateFrom, dates.DateTo);

            RentedBy = rentedBy;
            RentalDates = dates;
        }

        public void FinishRental()
        {
            RentedBy = null;
            RentalDates = new RentalDates();
        }

        public void AddEmployee(User employee)
        {
            if (UserRole.Employee != employee.Role)
            {
                throw new RoomException($"Room can't has user with role {employee.Role} as an employee.");
            }

            if (Employees.Contains(employee))
            {
                throw new RoomException($"Room already has employee with identifier '{employee.Id}'");
            }

            Employees.Add(employee);
        }

        public void RemoveEmployee(User employee)
        {
            if (!Employees.Contains(employee))
            {
                throw new RoomException($"Room doesn't has employee with identifier '{employee.Id}'.");
            }

            Employees.Remove(employee);
        }
    }

    public enum RoomType
    {
        Single,
        Double,
        Triple
    }
}