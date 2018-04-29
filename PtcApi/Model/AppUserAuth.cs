using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PtcApi.Model;

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
    public List<AppUserClaim> Claims { get; set; }
}