﻿using Domain;
using Infrastructure.Database;

namespace Application
{
    public class BookMethods
    {
        private readonly FakeDatabas _fakeDatabas;

        public BookMethods(FakeDatabas fakeDatabas) 
        {  
            _fakeDatabas = fakeDatabas; 
        }
        public static Book AddNewBook()
        {
            Book newbooktoadd = new Book(1, "Branislav", "Book of Branislav");
            return newbooktoadd;
        }
    }
}