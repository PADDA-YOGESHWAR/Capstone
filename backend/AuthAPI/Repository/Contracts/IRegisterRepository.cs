using AuthAPI.Models;
using AuthAPI.Static;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Repository.Contracts
{
    public interface IRegisterRepository
    {
        public Task<ActionResult> Register([FromBody] RegisterModel model, params string[] roles);

    }
}
