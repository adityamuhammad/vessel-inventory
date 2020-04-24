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

        public int user_id { get; set; }
        public string username { get; set; }
        public int ship_id { get; set; }
        public int department_id { get; set; }
        public string personalname { get; set; }
    }
}
