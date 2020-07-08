using System;
using System.Collections.Generic;

namespace StageAPI.Data.Proposal
{

    public class ProposalForm
    {

        public string[] FieldsOfStudy { get; set; }

        public string Description { get; set; }

        public string[] ToolsUsed { get; set; }

        public string ToolsInformation { get; set; }

        public string[] Necessities { get; set; }

        public string Theme { get; set; }

        public string[] Activities { get; set; }

        public byte RequiredStudentAmount { get; set; }

        public string PreferedStudents { get; set; }

        public string OptionalComment { get; set; }

        public string[] Periods { get; set; }

    }

}