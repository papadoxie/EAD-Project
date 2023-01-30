using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

using PUCCI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PUCCI.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Build.Framework;
using Microsoft.AspNetCore.Identity;

namespace PUCCI.Data;

public class DbInitializer
{
    public static void Seed(ModelBuilder builder)
    {
        SetPasswords();
        builder.Entity<User>().HasData(
            _seedUsers
        );
    }

    private static void SetPasswords()
    {
        var hasher = new PasswordHasher<User>();

        for (int i = 0; i < _seedUsers.Count; i++)
        {
            _seedUsers[i].PasswordHash = hasher.HashPassword(_seedUsers[i], _passwords[i]);
            _seedUsers[i].EmailConfirmed = true;
            _seedUsers[i].LockoutEnabled = true;
        }
    }

    private static List<User> _seedUsers = new List<User>()
    {
        new User
        {
            UserName = "123",
            NormalizedUserName = "123",
            Email = "123123@123",
            NormalizedEmail = "123@123.123"
        },
        new User
        {
            UserName = "asd",
            NormalizedUserName = "ASD",
            Email = "asd@asd.com",
            NormalizedEmail = "ASD@ASD.COM"
        }
    };

    private static List<string> _passwords = new List<string>()
    {
        "123123",
        "asd"
    };
}
