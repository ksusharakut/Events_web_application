using System;
using System.Collections.Generic;
using Application.UseCases.DTOs;
using Domain.Entities;

namespace Application.Tests
{
    public class TestDataGenerator
    {
        // Генерация тестовых данных для EventReturnDTO
        public static List<EventReturnDTO> GenerateEventReturnDTOs(int count)
        {
            var events = new List<EventReturnDTO>();
            for (int i = 1; i <= count; i++)
            {
                events.Add(new EventReturnDTO
                {
                    Id = i,
                    Title = $"Event {i}",
                    Description = $"Description for Event {i}",
                    DateTime = DateTime.UtcNow.AddDays(i),
                    Location = $"Location {i}",
                    Category = $"Category {i}",
                    MaxParticipants = 100 + i,
                    ImageUrl = $"image{i}.jpg"
                });
            }
            return events;
        }

        // Генерация тестовых данных для сущности Event
        public static List<Event> GenerateEvents(int count)
        {
            var events = new List<Event>();
            for (int i = 1; i <= count; i++)
            {
                events.Add(new Event
                {
                    Id = i,
                    Title = $"Event {i}",
                    Description = $"Description for Event {i}",
                    DateTime = DateTime.UtcNow.AddDays(i),
                    Location = $"Location {i}",
                    Category = $"Category {i}",
                    MaxParticipants = 100 + i,
                    ImageUrl = $"image{i}.jpg"
                });
            }
            return events;
        }

        // Генерация тестовых данных для EventDTO
        public static EventDTO GenerateEventDTO()
        {
            return new EventDTO
            {
                Title = "Test Event",
                Description = "Test Description",
                DateTime = DateTime.UtcNow.AddDays(7),
                Location = "Test Location",
                Category = "Test Category",
                MaxParticipants = 100
            };
        }

        // Генерация тестового события
        public static Event GenerateEvent(int id = 1, string title = "Test Event")
        {
            return new Event
            {
                Id = id,
                Title = title,
                Description = "Test Description",
                DateTime = DateTime.UtcNow.AddDays(7),
                Location = "Test Location",
                Category = "Test Category",
                MaxParticipants = 100,
                ImageUrl = "test.jpg"
            };
        }


    }
}