using VesselInventory.Models;

namespace VesselInventory.Utility
{
    public class Auth
    {
        private static Auth _auth;
        private Auth() { }
        public static Auth Instance
        {
            get
            {
                if (_auth is null)
                    _auth = new Auth();
                return _auth;
            }
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ShipId { get; set; }
        public int DepartmentId { get; set; }
        public string PersonName { get; set; }
    }
}
