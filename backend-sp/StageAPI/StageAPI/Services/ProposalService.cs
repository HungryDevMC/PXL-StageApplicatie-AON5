using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StageAPI.Data;
using StageAPI.Data.Proposal;
using StageAPI.Database;
using StageAPI.Models;

namespace StageAPI.Services
{
    public class ProposalService
    {
        public static async Task<List<Proposal>> GetAll()
        {
            var retList = new List<Proposal>();

            using (var p_Proposal_GetAll = new StoredProcedure("p_Proposal_GetAll"))
            {
                using (var reader = await p_Proposal_GetAll.RunReader())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            retList.Add(new Proposal
                            {
                                ID = reader.GetInt32(0),
                                FormData = new ProposalForm
                                {
                                    FieldsOfStudy = reader.GetString(1).Split(';'),
                                    Description = reader.GetString(2),
                                    ToolsUsed = reader.GetString(3).Split(';'),
                                    ToolsInformation = reader.GetString(4),
                                    Necessities = reader.GetString(5).Split(';'),
                                    Theme = reader.GetString(6),
                                    Activities = reader.GetString(7).Split(';'),
                                    RequiredStudentAmount = reader.GetByte(8),
                                    PreferedStudents = reader.GetString(9),
                                    OptionalComment = reader.GetString(10),
                                    Periods = reader.GetString(11).Split(';')
                                },
                                State = (ProposalState) reader.GetInt32(12),
                                DateOfState = reader.GetDateTime(13),
                                DateSubmitted = reader.GetDateTime(14),
                                // TODO: Feedback
                            });
                        }
                    }
                }
            }

            return retList;
        }

        public static async Task<bool> Exists(string desc)
        {
            using (var p_Proposal_Exists = new StoredProcedure("p_Proposal_Exists"))
            {
                p_Proposal_Exists.AddParameter("sDescription", desc);

                using (var reader = await p_Proposal_Exists.RunReader())
                {
                    if (reader.HasRows)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static async Task<bool> Create(CreateProposalModel model)
        {
            using (var p_Proposal_Create = new StoredProcedure("p_Proposal_Create"))
            {
                p_Proposal_Create.AddParameter("sStudyFields", string.Join(';', model.Study));
                p_Proposal_Create.AddParameter("sDescription", model.Description);
                p_Proposal_Create.AddParameter("sToolsUsed", string.Join(';', model.Environment));
                p_Proposal_Create.AddParameter("sToolsInformation", model.TechnicalDescription);
                p_Proposal_Create.AddParameter("sNecessities", model.ExtraRequirements);
                p_Proposal_Create.AddParameter("sTheme", model.Theme);
                p_Proposal_Create.AddParameter("sActivities", string.Join(';', model.Activities));
                p_Proposal_Create.AddParameter("nRequiredStudents", model.AmountOfStudents);
                p_Proposal_Create.AddParameter("sPreferedStudents", string.Join(';', model.Names));
                p_Proposal_Create.AddParameter("sOptionalComment", model.Remarks);
                p_Proposal_Create.AddParameter("sPeriods", string.Join(';', model.Period));
                p_Proposal_Create.AddOutput<int>("nID");

                p_Proposal_Create.Run();
                
                return true;
                // Fuck checking temporarily.
                //return (await p_Proposal_Create.Run()).GetOutput<int>("nID") != 0;
            }
        }
    }
}
