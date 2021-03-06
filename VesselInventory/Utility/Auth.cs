﻿namespace VesselInventory.Utility
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
        public string Username { get; set; }
        public string DepartmentName { get; set; }
        public string PersonName { get; set; }
        public int ShipId { get; set; }
    }
}
