using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests
{
    public class TestDataGenerator
    {
        public static Event CreateTestEvent()
        {
            return new Event
            {
                Id = 1,
                Title = "Test Event",
                DateTime = DateTime.Now,
                Location = "Test Location",
                Category = "Test Category",
                ImageUrl = "images/test.jpg"
            };

    }   }

}
