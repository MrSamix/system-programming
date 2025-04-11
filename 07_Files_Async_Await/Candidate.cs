using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _07_Files_Async_Await
{
    public class Candidate
    {
        public string Name { get; set; }
        public string City { get; set; }
        public double Salary { get; set; }
        public int YearExperience { get; set; }

        public Candidate()
        {
            Name = "";
            City = "";
            Salary = -1;
            YearExperience = -1;
        }

        public Candidate(string name, string city, double salary, int yearexperience)
        {
            Name = name;
            City = city;
            Salary = salary;
            YearExperience = yearexperience;
        }

        public Candidate(Candidate candidate)
        {
            Name = candidate.Name;
            City = candidate.City;
            Salary = candidate.Salary;
            YearExperience = candidate.YearExperience;
        }

        public override string ToString()
        {
            return $"{Name}, {City}, {Salary}, {YearExperience}";
        }

        public void SaveCandidate()
        {
            var str = JsonConvert.SerializeObject(this);
            File.WriteAllText($"../../../CV/{Name.Replace(' ', '_')}.json", str);
        }

        public void LoadCandidate(string path)
        {
            var str = File.ReadAllText(path);
            var candidate = JsonConvert.DeserializeObject<Candidate>(str);
            if (candidate is not null)
            {
                Name = candidate.Name;
                City = candidate.City;
                Salary = candidate.Salary;
                YearExperience = candidate.YearExperience;
            }
        }
    }
}
