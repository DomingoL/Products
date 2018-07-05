

namespace Products.Backend.Helper
{
    using System;
    using System.IO;
    using System.Web;
    public class FilesHelper
    {
        public static string UnloadPhoto(HttpPostedFileBase file, string folder)
        {
            var path = String.Empty;
            var pic = String.Empty;
            if (file != null )
            {
                pic = Path.GetFileName(file.FileName);
                path = Path.Combine(HttpContext.Current.Server.MapPath(folder), pic);
                file.SaveAs(path);
            }
            return pic;
        }
    }
}