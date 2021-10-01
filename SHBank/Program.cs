using System;
using System.Collections.Generic;
using System.Globalization;
using SHBank.Controller;
using SHBank.Entity;
using SHBank.View;

namespace SHBank
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Menu menu = new Menu();
            menu.GenerateMenu();
        }
    }
}