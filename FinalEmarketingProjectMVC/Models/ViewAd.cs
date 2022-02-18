using System;

namespace FinalEmarketingProjectMVC.Models
{
    public class ViewAd
    {
        public int pro_id { get; set; }
        public string pro_name { get; set; }
        public string pro_price { get; set; }
        public string pro_description { get; set; }
        public Nullable<int> pro_fk_user { get; set; }
        public Nullable<int> pro_fk_cat { get; set; }
        public string i_image { get; set; }
        public System.DateTime pro_add_date { get; set; }
        public string cat_name { get; set; }
        public string u_uid { get; set; }
        public string u_username { get; set; }
        public string u_phone { get; set; }
        public string u_image { get; set; }
    }
}