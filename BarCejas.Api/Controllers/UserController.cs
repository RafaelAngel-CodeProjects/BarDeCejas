using AutoMapper;

using BarCejas.Api.Responses;
using BarCejas.Core.CustomEntities;
using BarCejas.Core.DTOs;
using BarCejas.Core.Entities;
using BarCejas.Core.Interfaces;
using BarCejas.Core.QueryFilters;
using BarCejas.Infrastructure.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BarCejas.Api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public UserController(IUserService userService, IPasswordHasher passwordHasher, IMapper mapper, IUriService uriService)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _uriService = uriService;
        }

        /// <summary>
        /// get user
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetPost))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UserDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<IEnumerable<UserDto>>))]
        public IActionResult GetPost([FromQuery] QueryFilter filters)
        {
            var posts = _userService.GetUsers(filters);

            return Ok(new ApiResponse<IEnumerable<UserDto>>(_mapper.Map<IEnumerable<UserDto>>(posts))
            {
                Meta = new Metadata
                {
                    TotalCount = posts.TotalCount,
                    PageSize = posts.PageSize,
                    CurrentPage = posts.CurrentPage,
                    TotalPages = posts.TotalPages,
                    HasNextPage = posts.HasNextPage,
                    HasPreviousPage = posts.HasPreviousPage,
                    NextPageUrl = _uriService.GetUserPaginationUri(filters, Url.RouteUrl(nameof(GetPost))).ToString()
                }
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            return Ok(new ApiResponse<UserDto>(_mapper.Map<UserDto>(await _userService.GetUserById(id))));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] UserDto model)
        {
            var user = _mapper.Map<User>(model);
            user.Password = _passwordHasher.Hash(user.Password);
            await _userService.InsertUser(user);
            return Ok(new ApiResponse<UserDto>(model));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserDto post)
        {
            //post.Id = id;
            return Ok(new ApiResponse<bool>(await _userService.UpdateUser(_mapper.Map<User>(post))));
        }
    }
}
