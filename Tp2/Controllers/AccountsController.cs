using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entities.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tp2.Controllers.QueryParameters;
using Tp2.Dtos;
using Tp2.Extensions;
using Tp2.repositories;

namespace Tp2.Controllers
{
    [Route("api/owners/{ownerId}/accounts")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class AccountsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public AccountsController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult Get(Guid ownerId,[FromQuery] AccountQueryStringParams accountParams) {

            try
            {
                var owner = _repositoryWrapper.Owner.GetOwnerById(ownerId);
                if (owner == null)
                {
                    return BadRequest("no owner found ");
                }
                else
                {
                    var accounts = _repositoryWrapper.Account.getAccountsByOwner(ownerId, accountParams);
                    var metadata = new
                    {
                        accounts.TotalCount,
                        accounts.PageSize,
                        accounts.CurrentPage,
                        accounts.TotalPages,
                        accounts.HasNext,
                        accounts.HasPrevious
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                    var accountsDto = _mapper.Map<IEnumerable<Account>, IEnumerable<AccountDto>>(accounts);
                    return Ok(accountsDto.SelectByFieldsQuery(accountParams.fields));
                }
               
            } catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "try again later");
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]

        public ActionResult Post (Guid ownerId,[FromBody] AccountForCreation accountForCreation)
        {
            try
            {
                var owner = _repositoryWrapper.Owner.GetOwnerById(ownerId);
                if(owner == null)
                {
                    return BadRequest("no owner found ");
                }
                else
                {
                    var account = _mapper.Map<Account>(accountForCreation);
                    account.OwnerId = ownerId;
                    _repositoryWrapper.Account.Create(account);
                    _repositoryWrapper.Save();
                    var accountDto = _mapper.Map<AccountDto>(account);
                    return CreatedAtRoute("AccountById", new { id = account.AccountId,ownerId }, accountDto);
                }
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("{id}",Name = "AccountById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<OwnerDto> GetOwnerById(Guid ownerId,Guid id,[FromQuery] AccountQueryStringParams accountParams)
        {
            try
            {
                var owner = _repositoryWrapper.Owner.GetOwnerById(ownerId);
                if(owner == null)
                {
                    return NotFound();
                }
                else
                {
                    var account = _repositoryWrapper.Account.GetAccountById(id);
                    var accountDto = _mapper.Map<AccountDto>(account);
                    return Ok(accountDto.shapeObject(accountParams.fields));
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"try again later");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult Update(Guid id, [FromBody]AccountForUpdate accountForUpdate)
        {
            try
            {
                if (accountForUpdate == null)
                {
                    return BadRequest("Owner object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                var account = _repositoryWrapper.Account.GetAccountById(id);
                if (account == null)
                {
                    return NotFound();
                }

                _mapper.Map(accountForUpdate, account);
                _repositoryWrapper.Account.Update(account);
                _repositoryWrapper.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}