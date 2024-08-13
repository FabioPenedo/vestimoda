namespace VestiModa.Services
{
    public interface ISeedUserRoleInitial
    {
        Task CreateRoleAsync();
        Task CreateUserAsync();
    }
}
