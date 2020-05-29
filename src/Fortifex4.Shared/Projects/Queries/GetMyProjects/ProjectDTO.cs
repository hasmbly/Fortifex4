namespace Fortifex4.Shared.Projects.Queries.GetMyProjects
{
    public class ProjectDTO
    {
        public int ProjectID { get; set; }
        public string MemberUsername { get; set; }
        public string Name { get; set; }
        public int BlockchainID { get; set; }

        public string BlockchainName { get; set; }

        public ProjectDTO()
        {
        }
    }
}