using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlyEasy.Models
{
    public class Flight
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfNull] 
        public string? Id { get; set; } 

        [Required(ErrorMessage = "Numer lotu jest wymagany")]
        public string FlightNumber { get; set; } = null!;

        [Required(ErrorMessage = "Miejsce wylotu jest wymagane")]
        public string Departure { get; set; } = null!;

        [Required(ErrorMessage = "Destynacja jest wymagana")]
        public string Destination { get; set; } = null!;

        [Required(ErrorMessage = "Czas odlotu jest wymagany")]
        public DateTime DepartureTime { get; set; }

        public List<Passenger> Passengers { get; set; } = new List<Passenger>();
        public List<Luggage> Luggage { get; set; } = new List<Luggage>();
    }

    public class Passenger
    {
        public string Name { get; set; } = null!;
        public string PassportNumber { get; set; } = null!;
        public string SeatNumber { get; set; } = null!;
    }

    public class Luggage
    {
        public string LuggageId { get; set; } = null!;
        public double Weight { get; set; }
        public string Type { get; set; } = null!;
    }
}