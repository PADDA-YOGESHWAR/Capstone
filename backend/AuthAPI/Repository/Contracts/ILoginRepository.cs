using AuthAPI.Models;


namespace AuthAPI.Repository.Contracts
{
    public interface ILoginRepository
    {
        public Task<IResult> Login(LoginModel model);

    }
}
