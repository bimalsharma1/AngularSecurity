using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class AppUserAuth
{
    public  AppUserAuth() : base()
    {
        UserName = "Not Authorised";
        BearerToken = string.Empty;
    }
    public string UserName { get; set; }
    public string BearerToken { get; set; }
    public bool IsAuthenticated { get; set; }
    public bool CanAccessProducts { get; set; }
    public bool CanAddProduct { get; set; }
    public bool CanSaveProduct { get; set; }
    public bool CanAccessCategories { get; set; }
    public bool CanAddCategory { get; set; }

    
}