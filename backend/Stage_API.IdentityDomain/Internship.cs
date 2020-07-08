using System;
using System.Collections.Generic;

namespace Stage_API.Domain
{
    public class Internship : Entity
    {
        // ID van de gebruiker die deze internship aangemaakt heeft
        public Guid CreatorId { get; set; }

        // Titel van de opdracht
        public string Title { get; set; }

        // DEZELFDE VOLGORDE ALS CREATE FORM VOOR POSTEN VAN OPDRACHTEN VOOR DE BEDRIJVEN
        public ICollection<string> RequiredFieldsOfStudy { get; set; }

        // Omschrijving van de opdracht
        public string Description { get; set; }

        // Gebruikte programmeertalen, software etc
        public ICollection<string> Environment { get; set; }

        // Meer informatie over de environment
        public string TechnicalDescription { get; set; }

        // Beschikken over een auto, engelse communicatie, ...
        public string ExtraRequirements { get; set; }

        public string ResearchTheme { get; set; }

        // CV, sollicitatie gesprek, ..
        public ICollection<string> Activities { get; set; }

        public byte RequiredStudentsAmount { get; set; }

        // Studentennamen die het bedrijf heeft opgegeven, om (voor te stellen) de opdracht te maken
        public ICollection<string> AssignedStudents { get; set; }

        // Overige opmerkingen
        public string AdditionalRemarks { get; set; }

        // Periode waarover de stage loopt
        public ICollection<string> Periods { get; set; }

        // VELDEN VOOR NA HET INDIENEN VAN DE OPDRACHT, DEFAULT WAARDES ZETTEN BIJ AANMAKEN OPDRACHT

        // Status verandert aan de hand van waar de opdracht staat, review door leerkracht, coordinator, etc
        public InternshipState InternshipState { get; set; }

        // Verandert naar de huidige time, telkens als de internshipstate verandert.
        public DateTime DateOfState { get; set; }

        public ICollection<ReviewerInternships> Reviewers { get; set; }

        // Feedback given from coordinator
        public string Feedback { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

    }
}