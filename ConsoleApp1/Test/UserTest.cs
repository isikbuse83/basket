using System;
using ConsoleApp1.Domain.Entities;

namespace ConsoleApp1.Test;

public class UserTest
{

    //naming convention - classname_methodname_expected result 
    public static void UserCreateSuccess()
    {
        
        //arrange go get variables, whatever neeed clases , go get functions 
        
        string expectedName = "Ali";
        string expectedEmail = "ali@example.com";
        
        //act - execute this function
        var user = new User
        {
            UserName = expectedName,
            Email = expectedEmail
        };
        
        //assert - whatever is returned is it what you want

        if (user.UserName != expectedName)
        {
            Console.WriteLine("FAIL: Name not matched.");
            return;
        }

        if (user.Email != expectedEmail)
        {
            Console.WriteLine("FAIL: Email not matched.");
            return;
        }

        Console.WriteLine("PASS: UserCreateSuccess");
    }
}