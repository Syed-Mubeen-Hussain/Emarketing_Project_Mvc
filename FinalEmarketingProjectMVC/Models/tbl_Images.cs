//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FinalEmarketingProjectMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public partial class tbl_Images
    {
        public int i_id { get; set; }
        public string i_image { get; set; }
        public Nullable<int> p_id { get; set; }

        public HttpPostedFileBase ImgFile { get; set; }

        public virtual tbl_Products tbl_Products { get; set; }
    }
}
